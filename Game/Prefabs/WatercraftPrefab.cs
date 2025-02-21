// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WatercraftPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
using Game.Pathfind;
using Game.Vehicles;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Vehicles/", new Type[] {})]
  public class WatercraftPrefab : VehiclePrefab
  {
    public SizeClass m_SizeClass = SizeClass.Large;
    public EnergyTypes m_EnergyType = EnergyTypes.Fuel;
    public float m_MaxSpeed = 150f;
    public float m_Acceleration = 1f;
    public float m_Braking = 2f;
    public float2 m_Turning = new float2(30f, 5f);
    public float m_AngularAcceleration = 5f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<WatercraftData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Watercraft>());
      if (!components.Contains(ComponentType.ReadWrite<Moving>()))
        return;
      components.Add(ComponentType.ReadWrite<WatercraftNavigation>());
      components.Add(ComponentType.ReadWrite<WatercraftNavigationLane>());
      components.Add(ComponentType.ReadWrite<WatercraftCurrentLane>());
      components.Add(ComponentType.ReadWrite<PathOwner>());
      components.Add(ComponentType.ReadWrite<PathElement>());
      components.Add(ComponentType.ReadWrite<Target>());
      components.Add(ComponentType.ReadWrite<Blocker>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      WatercraftData componentData;
      componentData.m_SizeClass = this.m_SizeClass;
      componentData.m_EnergyType = this.m_EnergyType;
      componentData.m_MaxSpeed = this.m_MaxSpeed / 3.6f;
      componentData.m_Acceleration = this.m_Acceleration;
      componentData.m_Braking = this.m_Braking;
      componentData.m_Turning = math.radians(this.m_Turning);
      componentData.m_AngularAcceleration = math.radians(this.m_AngularAcceleration);
      entityManager.SetComponentData<WatercraftData>(entity, componentData);
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(8));
    }

    public override IEnumerable<string> modTags
    {
      get
      {
        foreach (string modTag in base.modTags)
          yield return modTag;
        yield return "Ship";
      }
    }
  }
}
