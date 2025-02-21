// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialUITriggerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tutorials
{
  public class TutorialUITriggerSystem : TutorialTriggerSystemBase, ITutorialUITriggerSystem
  {
    private PrefabSystem m_PrefabSystem;
    private EntityArchetype m_UnlockEventArchetype;
    private readonly List<string> m_ActivatedTriggers = new List<string>();

    public void ActivateTrigger(string trigger) => this.m_ActivatedTriggers.Add(trigger);

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      this.m_ActiveTriggerQuery = this.GetEntityQuery(ComponentType.ReadOnly<UITriggerData>(), ComponentType.ReadOnly<TriggerActive>(), ComponentType.Exclude<TriggerCompleted>());
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (this.m_ActivatedTriggers.Count > 0 && !this.m_ActiveTriggerQuery.IsEmptyIgnoreFilter)
      {
        EntityCommandBuffer commandBuffer = this.m_BarrierSystem.CreateCommandBuffer();
        NativeArray<Entity> entityArray = this.m_ActiveTriggerQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index1 = 0; index1 < entityArray.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated method
          foreach (TutorialUITriggerPrefab.UITriggerInfo uiTrigger in this.m_PrefabSystem.GetPrefab<TutorialUITriggerPrefab>(entityArray[index1]).m_UITriggers)
          {
            string[] strArray = uiTrigger.m_UITagProvider.uiTag?.Split('|', StringSplitOptions.None);
            if (strArray != null)
            {
              bool flag = false;
              for (int index2 = 0; index2 < strArray.Length; ++index2)
              {
                if (this.m_ActivatedTriggers.Contains(strArray[index2]))
                {
                  if ((UnityEngine.Object) uiTrigger.m_GoToPhase != (UnityEngine.Object) null)
                  {
                    // ISSUE: reference to a compiler-generated method
                    Entity entity = this.m_PrefabSystem.GetEntity((PrefabBase) uiTrigger.m_GoToPhase);
                    commandBuffer.AddComponent<TutorialNextPhase>(entityArray[index1], new TutorialNextPhase()
                    {
                      m_NextPhase = entity
                    });
                    commandBuffer.AddComponent<TriggerPreCompleted>(entityArray[index1]);
                  }
                  else
                    commandBuffer.AddComponent<TriggerCompleted>(entityArray[index1]);
                  // ISSUE: reference to a compiler-generated method
                  TutorialSystem.ManualUnlock(entityArray[index1], this.m_UnlockEventArchetype, this.EntityManager, commandBuffer);
                  flag = true;
                  break;
                }
              }
              if (flag)
                break;
            }
          }
        }
        entityArray.Dispose();
      }
      this.m_ActivatedTriggers.Clear();
    }

    [Preserve]
    public TutorialUITriggerSystem()
    {
    }
  }
}
