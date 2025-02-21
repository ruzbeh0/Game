// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialUIDeactivationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

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
  public class TutorialUIDeactivationSystem : 
    TutorialDeactivationSystemBase,
    ITutorialUIDeactivationSystem
  {
    private PrefabSystem m_PrefabSystem;
    private readonly HashSet<string> m_Deactivate = new HashSet<string>();
    private EntityQuery m_PendingTutorialQuery;
    private EntityQuery m_ActiveTutorialQuery;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PendingTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<UIActivationData>(), ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<TutorialActivated>(), ComponentType.ReadOnly<PrefabData>(), ComponentType.Exclude<TutorialActive>(), ComponentType.Exclude<TutorialCompleted>(), ComponentType.Exclude<ForceActivation>());
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<UIActivationData>(), ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<TutorialActivated>(), ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<TutorialActive>(), ComponentType.Exclude<TutorialCompleted>(), ComponentType.Exclude<ForceActivation>());
    }

    public void DeactivateTag(string tag) => this.m_Deactivate.Add(tag);

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_Deactivate.Count > 0)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PendingTutorialQuery.IsEmptyIgnoreFilter)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckDeactivate(this.m_PendingTutorialQuery);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ActiveTutorialQuery.IsEmptyIgnoreFilter && this.phaseCanDeactivate)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckDeactivate(this.m_ActiveTutorialQuery);
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Deactivate.Clear();
    }

    private void CheckDeactivate(EntityQuery query)
    {
      NativeArray<PrefabData> componentDataArray = query.ToComponentDataArray<PrefabData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeArray<Entity> entityArray = query.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      EntityCommandBuffer commandBuffer = this.m_BarrierSystem.CreateCommandBuffer();
      for (int index1 = 0; index1 < entityArray.Length; ++index1)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        string[] strArray = this.m_PrefabSystem.GetPrefab<TutorialPrefab>(componentDataArray[index1]).GetComponent<TutorialUIActivation>().m_UITagProvider.uiTag?.Split('|', StringSplitOptions.None);
        if (strArray != null)
        {
          for (int index2 = 0; index2 < strArray.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_Deactivate.Contains(strArray[index2].Trim()))
              commandBuffer.RemoveComponent<TutorialActivated>(entityArray[index1]);
          }
        }
      }
      componentDataArray.Dispose();
      entityArray.Dispose();
    }

    [Preserve]
    public TutorialUIDeactivationSystem()
    {
    }
  }
}
