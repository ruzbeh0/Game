// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrafficLightObject
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Objects;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {typeof (StaticObjectPrefab), typeof (MarkerObjectPrefab)})]
  public class TrafficLightObject : ComponentBase
  {
    public bool m_VehicleLeft;
    public bool m_VehicleRight;
    public bool m_CrossingLeft;
    public bool m_CrossingRight;
    public bool m_AllowFlipped;
    public Bounds1 m_ReachOffset;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<TrafficLightData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<TrafficLight>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      TrafficLightData componentData;
      componentData.m_Type = (TrafficLightType) 0;
      componentData.m_ReachOffset = this.m_ReachOffset;
      if (this.m_VehicleLeft)
        componentData.m_Type |= TrafficLightType.VehicleLeft;
      if (this.m_VehicleRight)
        componentData.m_Type |= TrafficLightType.VehicleRight;
      if (this.m_CrossingLeft)
        componentData.m_Type |= TrafficLightType.CrossingLeft;
      if (this.m_CrossingRight)
        componentData.m_Type |= TrafficLightType.CrossingRight;
      if (this.m_AllowFlipped)
        componentData.m_Type |= TrafficLightType.AllowFlipped;
      entityManager.SetComponentData<TrafficLightData>(entity, componentData);
    }
  }
}
