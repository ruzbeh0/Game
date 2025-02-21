// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CarTrailerPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Objects;
using Game.Rendering;
using Game.Vehicles;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Vehicles/", new System.Type[] {})]
  public class CarTrailerPrefab : CarBasePrefab
  {
    public CarTrailerType m_TrailerType = CarTrailerType.Towbar;
    public TrailerMovementType m_MovementType;
    public float3 m_AttachOffset = new float3(0.0f, 0.5f, 0.0f);
    public CarBasePrefab m_FixedTractor;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (!((UnityEngine.Object) this.m_FixedTractor != (UnityEngine.Object) null))
        return;
      prefabs.Add((PrefabBase) this.m_FixedTractor);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<CarData>());
      components.Add(ComponentType.ReadWrite<CarTrailerData>());
      components.Add(ComponentType.ReadWrite<SwayingData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<CarTrailer>());
      components.Add(ComponentType.ReadWrite<Controller>());
      components.Add(ComponentType.ReadWrite<BlockedLane>());
      if (components.Contains(ComponentType.ReadWrite<Stopped>()))
        components.Add(ComponentType.ReadWrite<ParkedCar>());
      if (!components.Contains(ComponentType.ReadWrite<Moving>()))
        return;
      components.Add(ComponentType.ReadWrite<CarTrailerLane>());
      components.Add(ComponentType.ReadWrite<Swaying>());
    }
  }
}
