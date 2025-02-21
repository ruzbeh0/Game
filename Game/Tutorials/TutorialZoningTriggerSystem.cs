// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialZoningTriggerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Prefabs;
using Game.Tools;
using Game.Zones;
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
  public class TutorialZoningTriggerSystem : TutorialTriggerSystemBase
  {
    private ZoneSystem m_ZoneSystem;
    private EntityQuery m_CreatedZonesQuery;
    private EntityQuery m_ZonesQuery;
    private EntityArchetype m_UnlockEventArchetype;
    private TutorialZoningTriggerSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_ActiveTriggerQuery = this.GetEntityQuery(ComponentType.ReadOnly<ZoningTriggerData>(), ComponentType.ReadOnly<TriggerActive>(), ComponentType.Exclude<TriggerCompleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedZonesQuery = this.GetEntityQuery(ComponentType.ReadOnly<Cell>(), ComponentType.ReadOnly<Updated>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Created>(), ComponentType.Exclude<Native>());
      // ISSUE: reference to a compiler-generated field
      this.m_ZonesQuery = this.GetEntityQuery(ComponentType.ReadOnly<Cell>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Created>(), ComponentType.Exclude<Native>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneSystem = this.World.GetOrCreateSystemManaged<ZoneSystem>();
      this.RequireForUpdate(this.m_ActiveTriggerQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      // ISSUE: reference to a compiler-generated field
      if (this.triggersChanged && !this.m_ZonesQuery.IsEmptyIgnoreFilter)
      {
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
        this.__TypeHandle.__Game_Tutorials_ZoningTriggerData_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Zones_Cell_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TutorialZoningTriggerSystem.CheckZonesJob jobData = new TutorialZoningTriggerSystem.CheckZonesJob()
        {
          m_ZoneChunks = this.m_ZonesQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
          m_CellType = this.__TypeHandle.__Game_Zones_Cell_RO_BufferTypeHandle,
          m_ZoningTriggerType = this.__TypeHandle.__Game_Tutorials_ZoningTriggerData_RO_BufferTypeHandle,
          m_UnlockRequirementFromEntity = this.__TypeHandle.__Game_Prefabs_UnlockRequirement_RO_BufferLookup,
          m_ForcedUnlockDataFromEntity = this.__TypeHandle.__Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_ZonePrefabs = this.m_ZoneSystem.GetPrefabs(),
          m_UnlockEventArchetype = this.m_UnlockEventArchetype,
          m_CommandBuffer = this.m_BarrierSystem.CreateCommandBuffer().AsParallelWriter(),
          m_FirstTimeCheck = true
        };
        this.Dependency = jobData.ScheduleParallel<TutorialZoningTriggerSystem.CheckZonesJob>(this.m_ActiveTriggerQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle));
        // ISSUE: reference to a compiler-generated field
        jobData.m_ZoneChunks.Dispose(this.Dependency);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ZoneSystem.AddPrefabsReader(this.Dependency);
        this.m_BarrierSystem.AddJobHandleForProducer(this.Dependency);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_CreatedZonesQuery.IsEmptyIgnoreFilter)
          return;
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
        this.__TypeHandle.__Game_Tutorials_ZoningTriggerData_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Zones_Cell_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TutorialZoningTriggerSystem.CheckZonesJob jobData = new TutorialZoningTriggerSystem.CheckZonesJob()
        {
          m_ZoneChunks = this.m_CreatedZonesQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
          m_CellType = this.__TypeHandle.__Game_Zones_Cell_RO_BufferTypeHandle,
          m_ZoningTriggerType = this.__TypeHandle.__Game_Tutorials_ZoningTriggerData_RO_BufferTypeHandle,
          m_UnlockRequirementFromEntity = this.__TypeHandle.__Game_Prefabs_UnlockRequirement_RO_BufferLookup,
          m_ForcedUnlockDataFromEntity = this.__TypeHandle.__Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_ZonePrefabs = this.m_ZoneSystem.GetPrefabs(),
          m_UnlockEventArchetype = this.m_UnlockEventArchetype,
          m_CommandBuffer = this.m_BarrierSystem.CreateCommandBuffer().AsParallelWriter(),
          m_FirstTimeCheck = false
        };
        this.Dependency = jobData.ScheduleParallel<TutorialZoningTriggerSystem.CheckZonesJob>(this.m_ActiveTriggerQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle));
        // ISSUE: reference to a compiler-generated field
        jobData.m_ZoneChunks.Dispose(this.Dependency);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ZoneSystem.AddPrefabsReader(this.Dependency);
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
    public TutorialZoningTriggerSystem()
    {
    }

    [BurstCompile]
    private struct CheckZonesJob : IJobChunk
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_ZoneChunks;
      [ReadOnly]
      public BufferTypeHandle<Cell> m_CellType;
      [ReadOnly]
      public BufferTypeHandle<ZoningTriggerData> m_ZoningTriggerType;
      [ReadOnly]
      public BufferLookup<UnlockRequirement> m_UnlockRequirementFromEntity;
      [ReadOnly]
      public BufferLookup<ForceUIGroupUnlockData> m_ForcedUnlockDataFromEntity;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ZonePrefabs m_ZonePrefabs;
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
        BufferAccessor<ZoningTriggerData> bufferAccessor = chunk.GetBufferAccessor<ZoningTriggerData>(ref this.m_ZoningTriggerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        for (int index = 0; index < bufferAccessor.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          if (this.Check(bufferAccessor[index]))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_FirstTimeCheck)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<TriggerPreCompleted>(unfilteredChunkIndex, nativeArray[index]);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<TriggerCompleted>(unfilteredChunkIndex, nativeArray[index]);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            TutorialSystem.ManualUnlock(nativeArray[index], this.m_UnlockEventArchetype, ref this.m_ForcedUnlockDataFromEntity, ref this.m_UnlockRequirementFromEntity, this.m_CommandBuffer, unfilteredChunkIndex);
          }
        }
      }

      private bool Check(DynamicBuffer<ZoningTriggerData> triggerDatas)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_ZoneChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.Check(triggerDatas, this.m_ZoneChunks[index].GetBufferAccessor<Cell>(ref this.m_CellType)))
            return true;
        }
        return false;
      }

      private bool Check(
        DynamicBuffer<ZoningTriggerData> triggerDatas,
        BufferAccessor<Cell> cellAccessor)
      {
        for (int index = 0; index < cellAccessor.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          if (this.Check(triggerDatas, cellAccessor[index]))
            return true;
        }
        return false;
      }

      private bool Check(DynamicBuffer<ZoningTriggerData> triggerDatas, DynamicBuffer<Cell> cells)
      {
        for (int index = 0; index < cells.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.Check(triggerDatas, this.m_ZonePrefabs[cells[index].m_Zone]))
            return true;
        }
        return false;
      }

      private bool Check(DynamicBuffer<ZoningTriggerData> triggerDatas, Entity zone)
      {
        for (int index = 0; index < triggerDatas.Length; ++index)
        {
          if (triggerDatas[index].m_Zone == zone)
            return true;
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
      public BufferTypeHandle<Cell> __Game_Zones_Cell_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ZoningTriggerData> __Game_Tutorials_ZoningTriggerData_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferLookup<UnlockRequirement> __Game_Prefabs_UnlockRequirement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ForceUIGroupUnlockData> __Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Cell_RO_BufferTypeHandle = state.GetBufferTypeHandle<Cell>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tutorials_ZoningTriggerData_RO_BufferTypeHandle = state.GetBufferTypeHandle<ZoningTriggerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UnlockRequirement_RO_BufferLookup = state.GetBufferLookup<UnlockRequirement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup = state.GetBufferLookup<ForceUIGroupUnlockData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
      }
    }
  }
}
