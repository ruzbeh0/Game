// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.VehicleSideEffects
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Vehicles/", new Type[] {typeof (VehiclePrefab)})]
  public class VehicleSideEffects : ComponentBase
  {
    public float2 m_RoadWear = new float2(0.5f, 1f);
    public float2 m_NoisePollution = new float2(0.5f, 1f);
    public float2 m_AirPollution = new float2(0.5f, 1f);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<VehicleSideEffectData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      VehicleSideEffectData componentData;
      componentData.m_Min = new float3(this.m_RoadWear.x, this.m_NoisePollution.x, this.m_AirPollution.x);
      componentData.m_Max = new float3(this.m_RoadWear.y, this.m_NoisePollution.y, this.m_AirPollution.y);
      entityManager.SetComponentData<VehicleSideEffectData>(entity, componentData);
    }
  }
}
