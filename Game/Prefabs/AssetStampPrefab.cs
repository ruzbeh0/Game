// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AssetStampPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
using Game.Rendering;
using Game.UI.Widgets;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new System.Type[] {})]
  public class AssetStampPrefab : ObjectPrefab
  {
    [InputField]
    [Range(1f, 1000f)]
    public int m_Width = 4;
    [InputField]
    [Range(1f, 1000f)]
    public int m_Depth = 4;
    public uint m_ConstructionCost;
    public uint m_UpKeepCost;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ObjectGeometryData>());
      components.Add(ComponentType.ReadWrite<AssetStampData>());
      components.Add(ComponentType.ReadWrite<PlaceableObjectData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Static>());
      components.Add(ComponentType.ReadWrite<AssetStamp>());
      components.Add(ComponentType.ReadWrite<CullingInfo>());
      components.Add(ComponentType.ReadWrite<PseudoRandomSeed>());
    }

    public override bool canIgnoreUnlockDependencies => false;
  }
}
