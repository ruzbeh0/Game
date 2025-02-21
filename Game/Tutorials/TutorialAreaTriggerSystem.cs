// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialAreaTriggerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Tutorials
{
  [CompilerGenerated]
  public class TutorialAreaTriggerSystem : TutorialTriggerSystemBase
  {
    private EntityQuery m_AreaModificationQuery;
    private EntityQuery m_AreaQuery;
    private EntityArchetype m_UnlockEventArchetype;
    private TutorialAreaTriggerSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaModificationQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Area>(), ComponentType.ReadOnly<Updated>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Native>());
      // ISSUE: reference to a compiler-generated field
      this.m_AreaQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Area>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Native>());
      this.m_ActiveTriggerQuery = this.GetEntityQuery(ComponentType.ReadOnly<AreaTriggerData>(), ComponentType.ReadOnly<TriggerActive>(), ComponentType.Exclude<TriggerCompleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
      this.RequireForUpdate(this.m_ActiveTriggerQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (this.triggersChanged)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_UnlockRequirement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tutorials_AreaTriggerData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        JobHandle outJobHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TutorialAreaTriggerSystem.CheckModifiedAreasJob jobData = new TutorialAreaTriggerSystem.CheckModifiedAreasJob()
        {
          m_AreaModificationChunks = this.m_AreaQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
          m_TriggerType = this.__TypeHandle.__Game_Tutorials_AreaTriggerData_RO_ComponentTypeHandle,
          m_UnlockRequirementFromEntity = this.__TypeHandle.__Game_Prefabs_UnlockRequirement_RO_BufferLookup,
          m_ForcedUnlockDataFromEntity = this.__TypeHandle.__Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
          m_UnlockEventArchetype = this.m_UnlockEventArchetype,
          m_CommandBuffer = this.m_BarrierSystem.CreateCommandBuffer().AsParallelWriter(),
          m_FirstTimeCheck = true
        };
        this.Dependency = jobData.ScheduleParallel<TutorialAreaTriggerSystem.CheckModifiedAreasJob>(this.m_ActiveTriggerQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle));
        // ISSUE: reference to a compiler-generated field
        jobData.m_AreaModificationChunks.Dispose(this.Dependency);
        this.m_BarrierSystem.AddJobHandleForProducer(this.Dependency);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_AreaModificationQuery.IsEmptyIgnoreFilter)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_UnlockRequirement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tutorials_AreaTriggerData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        JobHandle outJobHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TutorialAreaTriggerSystem.CheckModifiedAreasJob jobData = new TutorialAreaTriggerSystem.CheckModifiedAreasJob()
        {
          m_AreaModificationChunks = this.m_AreaModificationQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
          m_TriggerType = this.__TypeHandle.__Game_Tutorials_AreaTriggerData_RO_ComponentTypeHandle,
          m_UnlockRequirementFromEntity = this.__TypeHandle.__Game_Prefabs_UnlockRequirement_RO_BufferLookup,
          m_ForcedUnlockDataFromEntity = this.__TypeHandle.__Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
          m_UnlockEventArchetype = this.m_UnlockEventArchetype,
          m_CommandBuffer = this.m_BarrierSystem.CreateCommandBuffer().AsParallelWriter(),
          m_FirstTimeCheck = false
        };
        this.Dependency = jobData.ScheduleParallel<TutorialAreaTriggerSystem.CheckModifiedAreasJob>(this.m_ActiveTriggerQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle));
        // ISSUE: reference to a compiler-generated field
        jobData.m_AreaModificationChunks.Dispose(this.Dependency);
        this.m_BarrierSystem.AddJobHandleForProducer(this.Dependency);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [UnityEngine.Scripting.Preserve]
    public TutorialAreaTriggerSystem()
    {
    }

    [BurstCompile]
    private struct CheckModifiedAreasJob : IJobChunk
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_AreaModificationChunks;
      [ReadOnly]
      public ComponentTypeHandle<AreaTriggerData> m_TriggerType;
      [ReadOnly]
      public BufferLookup<UnlockRequirement> m_UnlockRequirementFromEntity;
      [ReadOnly]
      public BufferLookup<ForceUIGroupUnlockData> m_ForcedUnlockDataFromEntity;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public EntityArchetype m_UnlockEventArchetype;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public bool m_FirstTimeCheck;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<AreaTriggerData> nativeArray1 = chunk.GetNativeArray<AreaTriggerData>(ref this.m_TriggerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.m_EntityType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          if (this.Check(nativeArray1[index]))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_FirstTimeCheck)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<TriggerPreCompleted>(unfilteredChunkIndex, nativeArray2[index]);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<TriggerCompleted>(unfilteredChunkIndex, nativeArray2[index]);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            TutorialSystem.ManualUnlock(nativeArray2[index], this.m_UnlockEventArchetype, ref this.m_ForcedUnlockDataFromEntity, ref this.m_UnlockRequirementFromEntity, this.m_CommandBuffer, unfilteredChunkIndex);
          }
        }
      }

      private bool Check(AreaTriggerData areaTriggerData)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_AreaModificationChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          AreaTriggerFlags areaTriggerFlags = this.m_AreaModificationChunks[index1].Has<Created>(ref this.m_CreatedType) ? AreaTriggerFlags.Created : AreaTriggerFlags.Modified;
          if ((areaTriggerData.m_Flags & areaTriggerFlags) != (AreaTriggerFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray = this.m_AreaModificationChunks[index1].GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            for (int index2 = 0; index2 < nativeArray.Length; ++index2)
            {
              if (areaTriggerData.m_Prefab == nativeArray[index2].m_Prefab)
                return true;
            }
          }
        }
        return false;
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<AreaTriggerData> __Game_Tutorials_AreaTriggerData_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<UnlockRequirement> __Game_Prefabs_UnlockRequirement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ForceUIGroupUnlockData> __Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tutorials_AreaTriggerData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AreaTriggerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UnlockRequirement_RO_BufferLookup = state.GetBufferLookup<UnlockRequirement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup = state.GetBufferLookup<ForceUIGroupUnlockData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
      }
    }
  }
}
