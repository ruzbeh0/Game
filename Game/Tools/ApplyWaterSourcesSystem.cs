// Decompiled with JetBrains decompiler
// Type: Game.Tools.ApplyWaterSourcesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
using Game.Prefabs;
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
  public class ApplyWaterSourcesSystem : GameSystemBase
  {
    private ToolOutputBarrier m_ToolOutputBarrier;
    private EntityQuery m_TempQuery;
    private ApplyWaterSourcesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier = this.World.GetOrCreateSystemManaged<ToolOutputBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Game.Simulation.WaterSourceData>(), ComponentType.Exclude<PrefabRef>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TempQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterSourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new ApplyWaterSourcesSystem.HandleTempEntitiesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_WaterSourceData = this.__TypeHandle.__Game_Simulation_WaterSourceData_RO_ComponentLookup,
        m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<ApplyWaterSourcesSystem.HandleTempEntitiesJob>(this.m_TempQuery, this.Dependency);
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
    public ApplyWaterSourcesSystem()
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
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Simulation.WaterSourceData> m_WaterSourceData;
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
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Temp temp = nativeArray2[index];
          if ((temp.m_Flags & TempFlags.Cancel) != (TempFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.Cancel(unfilteredChunkIndex, entity, temp);
          }
          else if ((temp.m_Flags & TempFlags.Delete) != (TempFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.Delete(unfilteredChunkIndex, entity, temp);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_WaterSourceData.HasComponent(temp.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateComponent<Game.Simulation.WaterSourceData>(unfilteredChunkIndex, entity, temp.m_Original, this.m_WaterSourceData, true);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateComponent<Transform>(unfilteredChunkIndex, entity, temp.m_Original, this.m_TransformData, true);
              // ISSUE: reference to a compiler-generated method
              this.Update(unfilteredChunkIndex, entity, temp);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.Create(unfilteredChunkIndex, entity, temp);
            }
          }
        }
      }

      private void Cancel(int chunkIndex, Entity entity, Temp temp)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_HiddenData.HasComponent(temp.m_Original))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<BatchesUpdated>(chunkIndex, temp.m_Original, new BatchesUpdated());
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Hidden>(chunkIndex, temp.m_Original);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, entity, new Deleted());
      }

      private void Delete(int chunkIndex, Entity entity, Temp temp)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_WaterSourceData.HasComponent(temp.m_Original))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, temp.m_Original, new Deleted());
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, entity, new Deleted());
      }

      private void UpdateComponent<T>(
        int chunkIndex,
        Entity entity,
        Entity original,
        ComponentLookup<T> data,
        bool updateValue)
        where T : unmanaged, IComponentData
      {
        if (data.HasComponent(entity))
        {
          if (data.HasComponent(original))
          {
            if (!updateValue)
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<T>(chunkIndex, original, data[entity]);
          }
          else if (updateValue)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<T>(chunkIndex, original, data[entity]);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<T>(chunkIndex, original, default (T));
          }
        }
        else
        {
          if (!data.HasComponent(original))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<T>(chunkIndex, original);
        }
      }

      private void UpdateComponent<T>(
        int chunkIndex,
        Entity entity,
        Entity original,
        BufferLookup<T> data,
        bool updateValue)
        where T : unmanaged, IBufferElementData
      {
        if (data.HasBuffer(entity))
        {
          if (data.HasBuffer(original))
          {
            if (!updateValue)
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetBuffer<T>(chunkIndex, original).CopyFrom(data[entity]);
          }
          else if (updateValue)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddBuffer<T>(chunkIndex, original).CopyFrom(data[entity]);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddBuffer<T>(chunkIndex, original);
          }
        }
        else
        {
          if (!data.HasBuffer(original))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<T>(chunkIndex, original);
        }
      }

      private void Update(int chunkIndex, Entity entity, Temp temp)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_HiddenData.HasComponent(temp.m_Original))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<BatchesUpdated>(chunkIndex, temp.m_Original, new BatchesUpdated());
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Hidden>(chunkIndex, temp.m_Original);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(chunkIndex, temp.m_Original, new Updated());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, entity, new Deleted());
      }

      private void Create(int chunkIndex, Entity entity, Temp temp)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Temp>(chunkIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(chunkIndex, entity, new Updated());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Created>(chunkIndex, entity, new Created());
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
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Simulation.WaterSourceData> __Game_Simulation_WaterSourceData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterSourceData_RO_ComponentLookup = state.GetComponentLookup<Game.Simulation.WaterSourceData>(true);
      }
    }
  }
}
