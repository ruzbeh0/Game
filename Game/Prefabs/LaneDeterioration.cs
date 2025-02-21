// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LaneDeterioration
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/", new Type[] {typeof (NetLanePrefab)})]
  public class LaneDeterioration : ComponentBase
  {
    public float m_TrafficDeterioration = 0.01f;
    public float m_TimeDeterioration = 0.5f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<LaneDeteriorationData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (components.Contains(ComponentType.ReadWrite<MasterLane>()))
        return;
      components.Add(ComponentType.ReadWrite<LaneCondition>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      LaneDeteriorationData componentData;
      componentData.m_TrafficFactor = this.m_TrafficDeterioration;
      componentData.m_TimeFactor = this.m_TimeDeterioration;
      entityManager.SetComponentData<LaneDeteriorationData>(entity, componentData);
    }
  }
}
