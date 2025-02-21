// Decompiled with JetBrains decompiler
// Type: Game.Tools.ApplyZonesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Zones;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class ApplyZonesSystem : GameSystemBase
  {
    private ToolOutputBarrier m_ToolOutputBarrier;
    private EntityQuery m_TempQuery;
    private ComponentTypeSet m_AppliedTypes;
    private ApplyZonesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier = this.World.GetOrCreateSystemManaged<ToolOutputBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Game.Zones.Block>());
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedTypes = new ComponentTypeSet(ComponentType.ReadWrite<Applied>(), ComponentType.ReadWrite<Created>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TempQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Cell_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new ApplyZonesSystem.HandleTempEntitiesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_ZoneCellType = this.__TypeHandle.__Game_Zones_Cell_RO_BufferTypeHandle,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_Cells = this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup,
        m_AppliedTypes = this.m_AppliedTypes,
        m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<ApplyZonesSystem.HandleTempEntitiesJob>(this.m_TempQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
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
    public ApplyZonesSystem()
    {
    }

    [BurstCompile]
    private struct HandleTempEntitiesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public BufferTypeHandle<Cell> m_ZoneCellType;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public BufferLookup<Cell> m_Cells;
      [ReadOnly]
      public ComponentTypeSet m_AppliedTypes;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Cell> bufferAccessor = chunk.GetBufferAccessor<Cell>(ref this.m_ZoneCellType);
        if (bufferAccessor.Length != 0)
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Temp temp = nativeArray2[index];
            if ((temp.m_Flags & TempFlags.Delete) != (TempFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.Delete(unfilteredChunkIndex, entity, temp);
            }
            else if (temp.m_Original != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated method
              this.Update(unfilteredChunkIndex, entity, temp, bufferAccessor[index]);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.Create(unfilteredChunkIndex, entity);
            }
          }
        }
        else
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Temp temp = nativeArray2[index];
            if ((temp.m_Flags & TempFlags.Delete) != (TempFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.Delete(unfilteredChunkIndex, entity, temp);
            }
            else if (temp.m_Original != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated method
              this.Update(unfilteredChunkIndex, entity, temp);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.Create(unfilteredChunkIndex, entity);
            }
          }
        }
      }

      private void Delete(int chunkIndex, Entity entity, Temp temp)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_Cells.HasBuffer(temp.m_Original))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, temp.m_Original, new Deleted());
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, entity, new Deleted());
      }

      private void Update(int chunkIndex, Entity entity, Temp temp, bool updateOriginal = true)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_HiddenData.HasComponent(temp.m_Original))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Hidden>(chunkIndex, temp.m_Original);
          if (!updateOriginal)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<BatchesUpdated>(chunkIndex, temp.m_Original, new BatchesUpdated());
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (updateOriginal && this.m_Cells.HasBuffer(temp.m_Original))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(chunkIndex, temp.m_Original, new Updated());
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, entity, new Deleted());
      }

      private void Update(int chunkIndex, Entity entity, Temp temp, DynamicBuffer<Cell> cells)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_Cells.HasBuffer(temp.m_Original))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Cell> cell1 = this.m_Cells[temp.m_Original];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Cell> dynamicBuffer = this.m_CommandBuffer.SetBuffer<Cell>(chunkIndex, temp.m_Original);
          dynamicBuffer.ResizeUninitialized(cell1.Length);
          for (int index = 0; index < cell1.Length; ++index)
          {
            Cell cell2 = cells[index];
            Cell cell3 = cell1[index];
            if ((cell2.m_State & CellFlags.Selected) != CellFlags.None)
            {
              if ((cell3.m_State & CellFlags.Overridden) != CellFlags.None)
              {
                if (cell3.m_Zone.Equals(cell2.m_Zone))
                  cell3.m_State &= ~CellFlags.Overridden;
              }
              else
                cell3.m_Zone = cell2.m_Zone;
            }
            dynamicBuffer[index] = cell3;
          }
        }
        // ISSUE: reference to a compiler-generated method
        this.Update(chunkIndex, entity, temp);
      }

      private void Create(int chunkIndex, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Temp>(chunkIndex, entity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(chunkIndex, entity, in this.m_AppliedTypes);
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
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Cell> __Game_Zones_Cell_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Cell> __Game_Zones_Cell_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Cell_RO_BufferTypeHandle = state.GetBufferTypeHandle<Cell>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Cell_RO_BufferLookup = state.GetBufferLookup<Cell>(true);
      }
    }
  }
}
