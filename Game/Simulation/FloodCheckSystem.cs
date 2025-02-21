// Decompiled with JetBrains decompiler
// Type: Game.Simulation.FloodCheckSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Events;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class FloodCheckSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 16;
    private SimulationSystem m_SimulationSystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_TargetQuery;
    private EntityQuery m_WaterLevelChangeQuery;
    private EntityArchetype m_SubmergeArchetype;
    private FloodCheckSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_TargetQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.Exclude<Flooded>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_WaterLevelChangeQuery = this.GetEntityQuery(ComponentType.ReadOnly<WaterLevelChange>(), ComponentType.ReadOnly<Duration>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_SubmergeArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Submerge>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TargetQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint num = this.m_SimulationSystem.frameIndex >> 4 & 15U;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterLevelChangeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_WaterLevelChange_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InDanger_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Duration_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_WaterLevelChange_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle deps;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      FloodCheckSystem.FloodCheckJob jobData = new FloodCheckSystem.FloodCheckJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_WaterLevelChangeType = this.__TypeHandle.__Game_Events_WaterLevelChange_RO_ComponentTypeHandle,
        m_DurationType = this.__TypeHandle.__Game_Events_Duration_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_InDangerData = this.__TypeHandle.__Game_Events_InDanger_RO_ComponentLookup,
        m_WaterLevelChangeData = this.__TypeHandle.__Game_Events_WaterLevelChange_RO_ComponentLookup,
        m_PrefabWaterLevelChangeData = this.__TypeHandle.__Game_Prefabs_WaterLevelChangeData_RO_ComponentLookup,
        m_PrefaObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_UpdateFrameIndex = num,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_WaterLevelChangeChunks = this.m_WaterLevelChangeQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_SubmergeArchetype = this.m_SubmergeArchetype,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<FloodCheckSystem.FloodCheckJob>(this.m_TargetQuery, JobHandle.CombineDependencies(this.Dependency, deps, outJobHandle));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
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
    public FloodCheckSystem()
    {
    }

    [BurstCompile]
    private struct FloodCheckJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<WaterLevelChange> m_WaterLevelChangeType;
      [ReadOnly]
      public ComponentTypeHandle<Duration> m_DurationType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<InDanger> m_InDangerData;
      [ReadOnly]
      public ComponentLookup<WaterLevelChange> m_WaterLevelChangeData;
      [ReadOnly]
      public ComponentLookup<WaterLevelChangeData> m_PrefabWaterLevelChangeData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefaObjectGeometryData;
      [ReadOnly]
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_WaterLevelChangeChunks;
      [ReadOnly]
      public EntityArchetype m_SubmergeArchetype;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((int) chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index != (int) this.m_UpdateFrameIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray2 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Transform transform = nativeArray2[index];
          float depth;
          // ISSUE: reference to a compiler-generated method
          if (this.IsFlooded(transform.m_Position, out depth))
          {
            Entity entity1 = nativeArray1[index];
            ObjectGeometryData componentData;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PrefaObjectGeometryData.TryGetComponent(nativeArray3[index].m_Prefab, out componentData) || (componentData.m_Flags & GeometryFlags.CanSubmerge) == GeometryFlags.None)
            {
              // ISSUE: reference to a compiler-generated method
              Entity floodEvent = this.FindFloodEvent(entity1, transform.m_Position);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity2 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_SubmergeArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Submerge>(unfilteredChunkIndex, entity2, new Submerge()
              {
                m_Event = floodEvent,
                m_Target = entity1,
                m_Depth = depth
              });
            }
          }
        }
      }

      private Entity FindFloodEvent(Entity entity, float3 position)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_InDangerData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          InDanger inDanger = this.m_InDangerData[entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_WaterLevelChangeData.HasComponent(inDanger.m_Event))
            return inDanger.m_Event;
        }
        Entity floodEvent = Entity.Null;
        float num1 = 1f / 1000f;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_WaterLevelChangeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk levelChangeChunk = this.m_WaterLevelChangeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = levelChangeChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<WaterLevelChange> nativeArray2 = levelChangeChunk.GetNativeArray<WaterLevelChange>(ref this.m_WaterLevelChangeType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Duration> nativeArray3 = levelChangeChunk.GetNativeArray<Duration>(ref this.m_DurationType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray4 = levelChangeChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            WaterLevelChange waterLevelChange = nativeArray2[index2];
            Duration duration = nativeArray3[index2];
            // ISSUE: reference to a compiler-generated field
            WaterLevelChangeData waterLevelChangeData = this.m_PrefabWaterLevelChangeData[nativeArray4[index2].m_Prefab];
            // ISSUE: reference to a compiler-generated field
            if (duration.m_StartFrame <= this.m_SimulationFrame && waterLevelChangeData.m_ChangeType == WaterLevelChangeType.Sine)
            {
              // ISSUE: reference to a compiler-generated field
              double num2 = (double) (this.m_SimulationFrame - duration.m_StartFrame) / 60.0;
              float num3 = (float) ((long) duration.m_EndFrame - (long) WaterLevelChangeSystem.TsunamiEndDelay - (long) duration.m_StartFrame) / 60f;
              float num4 = WaterSystem.WaveSpeed * 60f;
              double num5 = (double) num4;
              double x = num2 * num5;
              float y = (float) (x - (double) num3 * (double) num4);
              // ISSUE: reference to a compiler-generated field
              float2 _a = (float) (WaterSystem.kMapSize / 2) * -waterLevelChange.m_Direction;
              float num6 = MathUtils.Distance(new Line2(_a, _a + MathUtils.Right(waterLevelChange.m_Direction)), position.xz, out float _);
              float num7 = math.lerp((float) x, y, 0.5f);
              float num8 = math.smoothstep((float) ((x - (double) y) * 0.75), 0.0f, num6 - num7);
              if ((double) num8 > (double) num1)
              {
                floodEvent = nativeArray1[index2];
                num1 = num8;
              }
            }
          }
        }
        return floodEvent;
      }

      private bool IsFlooded(float3 position, out float depth)
      {
        // ISSUE: reference to a compiler-generated field
        float num1 = WaterUtils.SampleDepth(ref this.m_WaterSurfaceData, position);
        if ((double) num1 > 0.5)
        {
          // ISSUE: reference to a compiler-generated field
          float num2 = num1 + (TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, position) - position.y);
          if ((double) num2 > 0.5)
          {
            depth = num2;
            return true;
          }
        }
        depth = 0.0f;
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
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterLevelChange> __Game_Events_WaterLevelChange_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Duration> __Game_Events_Duration_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<InDanger> __Game_Events_InDanger_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterLevelChange> __Game_Events_WaterLevelChange_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterLevelChangeData> __Game_Prefabs_WaterLevelChangeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_WaterLevelChange_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterLevelChange>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Duration_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Duration>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InDanger_RO_ComponentLookup = state.GetComponentLookup<InDanger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_WaterLevelChange_RO_ComponentLookup = state.GetComponentLookup<WaterLevelChange>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterLevelChangeData_RO_ComponentLookup = state.GetComponentLookup<WaterLevelChangeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
      }
    }
  }
}
