// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CarPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
using Game.Pathfind;
using Game.Rendering;
using Game.Vehicles;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Vehicles/", new Type[] {})]
  public class CarPrefab : CarBasePrefab
  {
    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<CarData>());
      components.Add(ComponentType.ReadWrite<SwayingData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Car>());
      components.Add(ComponentType.ReadWrite<BlockedLane>());
      if (components.Contains(ComponentType.ReadWrite<Stopped>()))
        components.Add(ComponentType.ReadWrite<ParkedCar>());
      if (!components.Contains(ComponentType.ReadWrite<Moving>()))
        return;
      components.Add(ComponentType.ReadWrite<CarNavigation>());
      components.Add(ComponentType.ReadWrite<CarNavigationLane>());
      components.Add(ComponentType.ReadWrite<CarCurrentLane>());
      components.Add(ComponentType.ReadWrite<PathOwner>());
      components.Add(ComponentType.ReadWrite<PathElement>());
      components.Add(ComponentType.ReadWrite<Target>());
      components.Add(ComponentType.ReadWrite<Blocker>());
      components.Add(ComponentType.ReadWrite<Swaying>());
    }
  }
}
