// Decompiled with JetBrains decompiler
// Type: Game.SceneFlow.AssetLibrary
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Logging;
using Game.Prefabs;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

#nullable disable
namespace Game.SceneFlow
{
  [CreateAssetMenu(fileName = "AssetLibrary", menuName = "Colossal/Prefabs/AssetLibrary", order = 10)]
  public class AssetLibrary : ScriptableObject
  {
    public List<AssetCollection> m_Collections;
    private int m_AssetCount;
    private int m_ProgressCount;

    public float progress
    {
      get
      {
        return this.m_AssetCount <= 0 ? 0.0f : (float) this.m_ProgressCount / (float) (this.m_AssetCount - 1);
      }
    }

    public void Load(PrefabSystem prefabSystem, CancellationToken token)
    {
      ILog log = LogManager.GetLogger("SceneFlow");
      // ISSUE: reference to a compiler-generated method
      prefabSystem.ClearAvailabilityCache();
      this.m_ProgressCount = 0;
      this.m_AssetCount = this.GetCount();
      using (PerformanceCounter.Start((Action<TimeSpan>) (t => log.InfoFormat("Added {0}/{1} explicitly referenced prefabs in {2}s", (object) this.m_ProgressCount, (object) this.m_AssetCount, (object) t.TotalSeconds))))
      {
        foreach (AssetCollection collection in this.m_Collections)
        {
          token.ThrowIfCancellationRequested();
          this.m_ProgressCount += collection.Count;
          collection.AddPrefabsTo(prefabSystem);
        }
      }
    }

    private int GetCount()
    {
      int count = 0;
      foreach (AssetCollection collection in this.m_Collections)
        count += collection.Count;
      return count;
    }
  }
}
