// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialInputTriggerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Input;
using Game.Prefabs;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tutorials
{
  public class TutorialInputTriggerSystem : TutorialTriggerSystemBase
  {
    private EntityArchetype m_UnlockEventArchetype;
    private PrefabSystem m_PrefabSystem;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_ActiveTriggerQuery = this.GetEntityQuery(ComponentType.ReadOnly<InputTriggerData>(), ComponentType.ReadOnly<TriggerActive>(), ComponentType.Exclude<TriggerCompleted>());
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      this.RequireForUpdate(this.m_ActiveTriggerQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      NativeArray<InputTriggerData> componentDataArray = this.m_ActiveTriggerQuery.ToComponentDataArray<InputTriggerData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeArray<Entity> entityArray = this.m_ActiveTriggerQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      EntityCommandBuffer commandBuffer = this.m_BarrierSystem.CreateCommandBuffer();
      for (int index = 0; index < componentDataArray.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        if (this.Performed(this.m_PrefabSystem.GetPrefab<TutorialInputTriggerPrefab>(entityArray[index])))
        {
          commandBuffer.AddComponent<TriggerCompleted>(entityArray[index]);
          // ISSUE: reference to a compiler-generated method
          TutorialSystem.ManualUnlock(entityArray[index], this.m_UnlockEventArchetype, this.EntityManager, commandBuffer);
        }
      }
      componentDataArray.Dispose();
      entityArray.Dispose();
    }

    private bool Performed(TutorialInputTriggerPrefab prefab)
    {
      for (int index = 0; index < prefab.m_Actions.Length; ++index)
      {
        ProxyAction action;
        if (InputManager.instance.TryFindAction(prefab.m_Actions[index].m_Map, prefab.m_Actions[index].m_Action, out action) && action.WasPerformedThisFrame())
          return true;
      }
      return false;
    }

    [Preserve]
    public TutorialInputTriggerSystem()
    {
    }
  }
}
