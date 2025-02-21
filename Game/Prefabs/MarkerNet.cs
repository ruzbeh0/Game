// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MarkerNet
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
  [ComponentMenu("Net/", new Type[] {typeof (PathwayPrefab), typeof (TaxiwayPrefab), typeof (PowerLinePrefab), typeof (PipelinePrefab)})]
  public class MarkerNet : ComponentBase
  {
    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<MarkerNetData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (components.Contains(ComponentType.ReadWrite<Edge>()))
      {
        components.Add(ComponentType.ReadWrite<Marker>());
      }
      else
      {
        if (!components.Contains(ComponentType.ReadWrite<Game.Net.Node>()))
          return;
        components.Add(ComponentType.ReadWrite<Marker>());
      }
    }
  }
}
