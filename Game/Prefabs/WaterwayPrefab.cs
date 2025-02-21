// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WaterwayPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using Game.Simulation;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/Prefab/", new Type[] {})]
  public class WaterwayPrefab : NetGeometryPrefab
  {
    public float m_SpeedLimit = 200f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<WaterwayData>());
      components.Add(ComponentType.ReadWrite<DefaultNetLane>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      if (components.Contains(ComponentType.ReadWrite<Edge>()))
      {
        components.Add(ComponentType.ReadWrite<UpdateFrame>());
        components.Add(ComponentType.ReadWrite<EdgeColor>());
        components.Add(ComponentType.ReadWrite<Waterway>());
      }
      else if (components.Contains(ComponentType.ReadWrite<Game.Net.Node>()))
      {
        components.Add(ComponentType.ReadWrite<UpdateFrame>());
        components.Add(ComponentType.ReadWrite<EdgeColor>());
        components.Add(ComponentType.ReadWrite<Waterway>());
      }
      else
      {
        if (!components.Contains(ComponentType.ReadWrite<NetCompositionData>()))
          return;
        components.Add(ComponentType.ReadWrite<WaterwayComposition>());
      }
    }
  }
}
