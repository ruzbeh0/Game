// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ProcessingRequirementSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Economy;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class ProcessingRequirementSystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private ProcessingCompanySystem m_ProcessingCompanySystem;
    private EntityQuery m_RequirementQuery;
    private EntityArchetype m_UnlockEventArchetype;
    private ProcessingRequirementSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessingCompanySystem = this.World.GetOrCreateSystemManaged<ProcessingCompanySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RequirementQuery = this.GetEntityQuery(ComponentType.ReadOnly<ProcessingRequirementData>(), ComponentType.ReadWrite<UnlockRequirementData>(), ComponentType.ReadOnly<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_RequirementQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProcessingRequirementData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new ProcessingRequirementSystem.ProcessingRequirementJob()
      {
        m_ProducedResources = this.m_ProcessingCompanySystem.GetProducedResourcesArray(out dependencies),
        m_UnlockEventArchetype = this.m_UnlockEventArchetype,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ProcessingRequirementType = this.__TypeHandle.__Game_Prefabs_ProcessingRequirementData_RO_ComponentTypeHandle,
        m_UnlockRequirementType = this.__TypeHandle.__Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle
      }.ScheduleParallel<ProcessingRequirementSystem.ProcessingRequirementJob>(this.m_RequirementQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ProcessingCompanySystem.AddProducedResourcesReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = jobHandle;
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
    public ProcessingRequirementSystem()
    {
    }

    [BurstCompile]
    private struct ProcessingRequirementJob : IJobChunk
    {
      [ReadOnly]
      public NativeArray<long> m_ProducedResources;
      [ReadOnly]
      public EntityArchetype m_UnlockEventArchetype;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ProcessingRequirementData> m_ProcessingRequirementType;
      public ComponentTypeHandle<UnlockRequirementData> m_UnlockRequirementType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ProcessingRequirementData> nativeArray2 = chunk.GetNativeArray<ProcessingRequirementData>(ref this.m_ProcessingRequirementType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<UnlockRequirementData> nativeArray3 = chunk.GetNativeArray<UnlockRequirementData>(ref this.m_UnlockRequirementType);
        ChunkEntityEnumerator entityEnumerator = new ChunkEntityEnumerator(useEnabledMask, chunkEnabledMask, chunk.Count);
        int nextIndex;
        while (entityEnumerator.NextEntityIndex(out nextIndex))
        {
          ProcessingRequirementData processingRequirement = nativeArray2[nextIndex];
          UnlockRequirementData unlockRequirement = nativeArray3[nextIndex];
          // ISSUE: reference to a compiler-generated method
          if (this.ShouldUnlock(processingRequirement, ref unlockRequirement))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_UnlockEventArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Unlock>(unfilteredChunkIndex, entity, new Unlock(nativeArray1[nextIndex]));
          }
          nativeArray3[nextIndex] = unlockRequirement;
        }
      }

      private bool ShouldUnlock(
        ProcessingRequirementData processingRequirement,
        ref UnlockRequirementData unlockRequirement)
      {
        // ISSUE: reference to a compiler-generated field
        long producedResource = processingRequirement.m_ResourceType == Resource.NoResource ? 0L : this.m_ProducedResources[EconomyUtils.GetResourceIndex(processingRequirement.m_ResourceType)];
        if (producedResource >= (long) processingRequirement.m_MinimumProducedAmount)
        {
          unlockRequirement.m_Progress = processingRequirement.m_MinimumProducedAmount;
          return true;
        }
        unlockRequirement.m_Progress = (int) producedResource;
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
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ProcessingRequirementData> __Game_Prefabs_ProcessingRequirementData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<UnlockRequirementData> __Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ProcessingRequirementData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ProcessingRequirementData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnlockRequirementData>();
      }
    }
  }
}
