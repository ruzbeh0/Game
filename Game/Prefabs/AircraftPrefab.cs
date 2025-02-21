// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AircraftPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
using Game.Pathfind;
using Game.Vehicles;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public abstract class AircraftPrefab : VehiclePrefab
  {
    public SizeClass m_SizeClass;
    public float m_GroundMaxSpeed = 100f;
    public float m_GroundAcceleration = 3f;
    public float m_GroundBraking = 5f;
    public float2 m_GroundTurning = new float2(90f, 15f);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<AircraftData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Aircraft>());
      if (components.Contains(ComponentType.ReadWrite<Stopped>()))
        components.Add(ComponentType.ReadWrite<ParkedCar>());
      if (!components.Contains(ComponentType.ReadWrite<Moving>()))
        return;
      components.Add(ComponentType.ReadWrite<AircraftNavigation>());
      components.Add(ComponentType.ReadWrite<AircraftNavigationLane>());
      components.Add(ComponentType.ReadWrite<AircraftCurrentLane>());
      components.Add(ComponentType.ReadWrite<PathOwner>());
      components.Add(ComponentType.ReadWrite<PathElement>());
      components.Add(ComponentType.ReadWrite<Target>());
      components.Add(ComponentType.ReadWrite<Blocker>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<AircraftData>(entity, new AircraftData()
      {
        m_SizeClass = this.m_SizeClass,
        m_GroundMaxSpeed = this.m_GroundMaxSpeed / 3.6f,
        m_GroundAcceleration = this.m_GroundAcceleration,
        m_GroundBraking = this.m_GroundBraking,
        m_GroundTurning = math.radians(this.m_GroundTurning)
      });
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(10));
    }
  }
}
