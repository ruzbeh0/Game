// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LivePathPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Routes;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Routes/", new Type[] {})]
  public class LivePathPrefab : RoutePrefab
  {
    public override void GetDependencies(List<PrefabBase> prefabs) => base.GetDependencies(prefabs);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      if (components.Contains(ComponentType.ReadWrite<Route>()))
        components.Add(ComponentType.ReadWrite<LivePath>());
      else if (components.Contains(ComponentType.ReadWrite<Waypoint>()))
      {
        components.Add(ComponentType.ReadWrite<LivePath>());
      }
      else
      {
        if (!components.Contains(ComponentType.ReadWrite<Segment>()))
          return;
        components.Add(ComponentType.ReadWrite<LivePath>());
        components.Add(ComponentType.ReadWrite<PathSource>());
        components.Add(ComponentType.ReadWrite<CurveSource>());
      }
    }
  }
}
