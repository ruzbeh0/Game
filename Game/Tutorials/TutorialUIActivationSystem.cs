// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialUIActivationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Prefabs;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tutorials
{
  [CompilerGenerated]
  public class TutorialUIActivationSystem : GameSystemBase, ITutorialUIActivationSystem
  {
    protected EntityCommandBufferSystem m_BarrierSystem;
    private readonly Dictionary<string, List<Entity>> m_TutorialMap = new Dictionary<string, List<Entity>>();
    private readonly List<string> m_ActiveTags = new List<string>();
    private EntityQuery m_TutorialQuery;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_BarrierSystem = (EntityCommandBufferSystem) this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<UIActivationData>(), ComponentType.ReadOnly<PrefabData>());
      // ISSUE: reference to a compiler-generated method
      this.RebuildTutorialMap();
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated method
      this.RebuildTutorialMap();
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveTags.Clear();
    }

    private void RebuildTutorialMap()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialMap.Clear();
      // ISSUE: reference to a compiler-generated field
      if (this.m_TutorialQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_TutorialQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      for (int index1 = 0; index1 < entityArray.Length; ++index1)
      {
        Entity entity = entityArray[index1];
        // ISSUE: reference to a compiler-generated method
        string[] strArray = systemManaged.GetPrefab<TutorialPrefab>(entityArray[index1]).GetComponent<TutorialUIActivation>().m_UITagProvider?.uiTag?.Split('|', StringSplitOptions.None);
        if (strArray != null)
        {
          for (int index2 = 0; index2 < strArray.Length; ++index2)
          {
            string key = strArray[index2].Trim();
            // ISSUE: reference to a compiler-generated field
            if (!this.m_TutorialMap.ContainsKey(key))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_TutorialMap[key] = new List<Entity>();
            }
            // ISSUE: reference to a compiler-generated field
            if (!this.m_TutorialMap[key].Contains(entity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_TutorialMap[key].Add(entity);
            }
          }
        }
      }
      entityArray.Dispose();
    }

    public void SetTag(string tag, bool active)
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_TutorialMap.ContainsKey(tag))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveTags.Remove(tag);
      if (!active)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveTags.Add(tag);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ActiveTags.Count <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer = this.m_BarrierSystem.CreateCommandBuffer();
      // ISSUE: reference to a compiler-generated field
      foreach (string activeTag in this.m_ActiveTags)
      {
        List<Entity> entityList;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TutorialMap.TryGetValue(activeTag, out entityList))
        {
          foreach (Entity entity in entityList)
          {
            if (!this.EntityManager.HasComponent<TutorialCompleted>(entity))
            {
              this.EntityManager.AddComponent<TutorialActivated>(entity);
              if (!this.EntityManager.GetComponentData<UIActivationData>(entity).m_CanDeactivate)
                commandBuffer.AddComponent<ForceActivation>(entity);
            }
          }
        }
      }
    }

    [Preserve]
    public TutorialUIActivationSystem()
    {
    }
  }
}
