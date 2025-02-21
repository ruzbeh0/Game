// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AssetCollection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [CreateAssetMenu(fileName = "AssetCollection", menuName = "Colossal/Prefabs/AssetCollection", order = 0)]
  public class AssetCollection : ScriptableObject
  {
    public bool isActive = true;
    [ContextMenuItem("Sort Assets Alphabetically", "SortAssets")]
    public List<PrefabBase> m_Prefabs;
    public List<AssetCollection> m_Collections;

    public int Count => this.m_Prefabs.Count;

    public void AddPrefabsTo(PrefabSystem prefabSystem)
    {
      if (!this.isActive)
        return;
      foreach (PrefabBase prefab in this.m_Prefabs)
      {
        // ISSUE: reference to a compiler-generated method
        prefabSystem.AddPrefab(prefab, this.name);
      }
      foreach (AssetCollection collection in this.m_Collections)
        collection.AddPrefabsTo(prefabSystem);
    }

    public void SortAssets()
    {
      this.m_Prefabs.Sort((Comparison<PrefabBase>) ((a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal)));
    }
  }
}
