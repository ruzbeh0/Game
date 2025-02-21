// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PlaceableNet
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/", new System.Type[] {typeof (NetPrefab)})]
  public class PlaceableNet : ComponentBase
  {
    public Bounds1 m_ElevationRange = new Bounds1(-50f, 50f);
    public bool m_AllowParallelMode = true;
    public NetPrefab m_UndergroundPrefab;
    public int m_XPReward;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (!((UnityEngine.Object) this.m_UndergroundPrefab != (UnityEngine.Object) null))
        return;
      prefabs.Add((PrefabBase) this.m_UndergroundPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PlaceableNetData>());
      components.Add(ComponentType.ReadWrite<PlaceableInfoviewItem>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (!components.Contains(ComponentType.ReadWrite<NetCompositionData>()))
        return;
      components.Add(ComponentType.ReadWrite<PlaceableNetComposition>());
    }
  }
}
