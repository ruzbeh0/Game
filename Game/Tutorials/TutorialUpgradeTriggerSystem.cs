// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialUpgradeTriggerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Prefabs;
using Game.Tools;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tutorials
{
  public class TutorialUpgradeTriggerSystem : TutorialTriggerSystemBase
  {
    private EntityQuery m_CreatedUpgradeQuery;
    private EntityQuery m_UpgradeQuery;
    private EntityArchetype m_UnlockEventArchetype;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_CreatedUpgradeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Native>());
      this.m_UpgradeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Native>());
      this.m_ActiveTriggerQuery = this.GetEntityQuery(ComponentType.ReadOnly<UpgradeTriggerData>(), ComponentType.ReadOnly<TriggerActive>(), ComponentType.Exclude<TriggerCompleted>());
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
      this.RequireForUpdate(this.m_ActiveTriggerQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (this.triggersChanged && !this.m_UpgradeQuery.IsEmptyIgnoreFilter)
      {
        EntityCommandBuffer commandBuffer = this.m_BarrierSystem.CreateCommandBuffer();
        NativeArray<Entity> entityArray = this.m_ActiveTriggerQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          commandBuffer.AddComponent<TriggerPreCompleted>(entityArray[index]);
          // ISSUE: reference to a compiler-generated method
          TutorialSystem.ManualUnlock(entityArray[index], this.m_UnlockEventArchetype, this.EntityManager, commandBuffer);
        }
        entityArray.Dispose();
      }
      if (this.m_CreatedUpgradeQuery.IsEmptyIgnoreFilter)
        return;
      EntityCommandBuffer commandBuffer1 = this.m_BarrierSystem.CreateCommandBuffer();
      NativeArray<Entity> entityArray1 = this.m_ActiveTriggerQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < entityArray1.Length; ++index)
      {
        commandBuffer1.AddComponent<TriggerCompleted>(entityArray1[index]);
        // ISSUE: reference to a compiler-generated method
        TutorialSystem.ManualUnlock(entityArray1[index], this.m_UnlockEventArchetype, this.EntityManager, commandBuffer1);
      }
      entityArray1.Dispose();
    }

    [Preserve]
    public TutorialUpgradeTriggerSystem()
    {
    }
  }
}
