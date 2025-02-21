// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PedestrianLane
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
  public class PedestrianLane : ComponentBase
  {
    public float m_Width;
    public bool m_OnWater;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PedestrianLaneData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Net.PedestrianLane>());
      components.Add(ComponentType.ReadWrite<LaneObject>());
      components.Add(ComponentType.ReadWrite<LaneOverlap>());
    }
  }
}
