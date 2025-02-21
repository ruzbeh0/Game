// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetPollution
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/", new Type[] {typeof (NetPrefab)})]
  public class NetPollution : ComponentBase
  {
    public float m_NoisePollutionFactor = 1f;
    public float m_AirPollutionFactor = 1f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<NetPollutionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (!components.Contains(ComponentType.ReadWrite<Game.Net.Node>()) && !components.Contains(ComponentType.ReadWrite<Edge>()))
        return;
      components.Add(ComponentType.ReadWrite<Game.Net.Pollution>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      NetPollutionData componentData;
      componentData.m_Factors = new float2(this.m_NoisePollutionFactor, this.m_AirPollutionFactor);
      entityManager.SetComponentData<NetPollutionData>(entity, componentData);
    }
  }
}
