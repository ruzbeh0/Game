// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialPolicyAdjustmentTriggerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Common;
using Game.Policies;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tutorials
{
  [CompilerGenerated]
  public class TutorialPolicyAdjustmentTriggerSystem : TutorialTriggerSystemBase
  {
    private EntityQuery m_AdjustmentQuery;
    private EntityQuery m_PolicyQuery;
    private EntityArchetype m_UnlockEventArchetype;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_ActiveTriggerQuery = this.GetEntityQuery(ComponentType.ReadOnly<PolicyAdjustmentTriggerData>(), ComponentType.ReadOnly<TriggerActive>(), ComponentType.Exclude<TriggerCompleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_PolicyQuery = this.GetEntityQuery(ComponentType.ReadOnly<Policy>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_AdjustmentQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Common.Event>(), ComponentType.ReadOnly<Modify>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
      this.RequireForUpdate(this.m_ActiveTriggerQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      // ISSUE: reference to a compiler-generated field
      if (this.triggersChanged && !this.m_PolicyQuery.IsEmptyIgnoreFilter)
      {
        EntityCommandBuffer commandBuffer = this.m_BarrierSystem.CreateCommandBuffer();
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray1 = this.m_PolicyQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeArray<PolicyAdjustmentTriggerData> componentDataArray = this.m_ActiveTriggerQuery.ToComponentDataArray<PolicyAdjustmentTriggerData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeArray<Entity> entityArray2 = this.m_ActiveTriggerQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray2.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          if (this.FirstTimeCheck(componentDataArray[index], entityArray1))
          {
            commandBuffer.AddComponent<TriggerPreCompleted>(entityArray2[index]);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            TutorialSystem.ManualUnlock(entityArray2[index], this.m_UnlockEventArchetype, this.EntityManager, commandBuffer);
          }
        }
        componentDataArray.Dispose();
        entityArray2.Dispose();
        entityArray1.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_AdjustmentQuery.IsEmptyIgnoreFilter)
        return;
      EntityCommandBuffer commandBuffer1 = this.m_BarrierSystem.CreateCommandBuffer();
      NativeArray<PolicyAdjustmentTriggerData> componentDataArray1 = this.m_ActiveTriggerQuery.ToComponentDataArray<PolicyAdjustmentTriggerData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeArray<Entity> entityArray = this.m_ActiveTriggerQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<Modify> componentDataArray2 = this.m_AdjustmentQuery.ToComponentDataArray<Modify>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < componentDataArray1.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        if (this.Check(componentDataArray1[index], componentDataArray2))
        {
          commandBuffer1.AddComponent<TriggerCompleted>(entityArray[index]);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          TutorialSystem.ManualUnlock(entityArray[index], this.m_UnlockEventArchetype, this.EntityManager, commandBuffer1);
        }
      }
      componentDataArray1.Dispose();
      entityArray.Dispose();
      componentDataArray2.Dispose();
    }

    private bool Check(PolicyAdjustmentTriggerData data, NativeArray<Modify> adjustments)
    {
      for (int index = 0; index < adjustments.Length; ++index)
      {
        if ((data.m_TargetFlags & PolicyAdjustmentTriggerTargetFlags.District) != (PolicyAdjustmentTriggerTargetFlags) 0 && this.EntityManager.HasComponent<District>(adjustments[index].m_Entity) && (adjustments[index].m_Flags & PolicyFlags.Active) != (PolicyFlags) 0 && (data.m_Flags & PolicyAdjustmentTriggerFlags.Activated) != (PolicyAdjustmentTriggerFlags) 0)
          return true;
      }
      return false;
    }

    private bool FirstTimeCheck(
      PolicyAdjustmentTriggerData data,
      NativeArray<Entity> policyEntities)
    {
      for (int index = 0; index < policyEntities.Length; ++index)
      {
        DynamicBuffer<Policy> buffer = this.EntityManager.GetBuffer<Policy>(policyEntities[index], true);
        // ISSUE: reference to a compiler-generated method
        if ((data.m_TargetFlags & PolicyAdjustmentTriggerTargetFlags.District) != (PolicyAdjustmentTriggerTargetFlags) 0 && this.EntityManager.HasComponent<District>(policyEntities[index]) && this.AnyActive(buffer) && (data.m_Flags & PolicyAdjustmentTriggerFlags.Activated) != (PolicyAdjustmentTriggerFlags) 0)
          return true;
      }
      return false;
    }

    private bool AnyActive(DynamicBuffer<Policy> policies)
    {
      for (int index = 0; index < policies.Length; ++index)
      {
        if ((policies[index].m_Flags & PolicyFlags.Active) != (PolicyFlags) 0)
          return true;
      }
      return false;
    }

    [Preserve]
    public TutorialPolicyAdjustmentTriggerSystem()
    {
    }
  }
}
