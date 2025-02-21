// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TrafficFlowSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using Game.Pathfind;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TrafficFlowSystem : GameSystemBase
  {
    public const int UPDATES_PER_DAY = 32;
    private SimulationSystem m_SimulationSystem;
    private PathfindQueueSystem m_PathfindQueueSystem;
    private TimeSystem m_TimeSystem;
    private EntityQuery m_LaneQuery;
    private EntityQuery m_RoadQuery;
    private TrafficFlowSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 512;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindQueueSystem = this.World.GetOrCreateSystemManaged<PathfindQueueSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery = this.GetEntityQuery(ComponentType.ReadWrite<LaneFlow>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_RoadQuery = this.GetEntityQuery(ComponentType.ReadWrite<Road>(), ComponentType.ReadOnly<SubLane>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_LaneQuery, this.m_RoadQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      float num = this.m_TimeSystem.normalizedTime * 4f;
      float4 float4 = math.saturate(new float4(math.max(num - 3f, 1f - num), 1f - math.abs(num - new float3(1f, 2f, 3f))));
      FlowAction action = new FlowAction(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, 32, 16)));
      // ISSUE: reference to a compiler-generated field
      this.m_RoadQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_RoadQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, 32, 16)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneFlow_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TrafficFlowSystem.UpdateLaneFlowJob jobData1 = new TrafficFlowSystem.UpdateLaneFlowJob()
      {
        m_TimeFactors = float4,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_LaneFlowType = this.__TypeHandle.__Game_Net_LaneFlow_RW_ComponentTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneFlow_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      TrafficFlowSystem.UpdateRoadFlowJob jobData2 = new TrafficFlowSystem.UpdateRoadFlowJob()
      {
        m_TimeFactors = float4,
        m_SubLaneType = this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle,
        m_LaneFlowData = this.__TypeHandle.__Game_Net_LaneFlow_RO_ComponentLookup,
        m_MasterLaneData = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_RoadType = this.__TypeHandle.__Game_Net_Road_RW_ComponentTypeHandle,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RW_ComponentLookup,
        m_FlowActions = action.m_FlowData.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.ScheduleParallel<TrafficFlowSystem.UpdateLaneFlowJob>(this.m_LaneQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      EntityQuery roadQuery = this.m_RoadQuery;
      JobHandle dependsOn = jobHandle;
      JobHandle dependencies = jobData2.ScheduleParallel<TrafficFlowSystem.UpdateRoadFlowJob>(roadQuery, dependsOn);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindQueueSystem.Enqueue(action, dependencies);
      this.Dependency = dependencies;
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
    public TrafficFlowSystem()
    {
    }

    [BurstCompile]
    private struct UpdateLaneFlowJob : IJobChunk
    {
      [ReadOnly]
      public float4 m_TimeFactors;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<LaneFlow> m_LaneFlowType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<LaneFlow> nativeArray = chunk.GetNativeArray<LaneFlow>(ref this.m_LaneFlowType);
        // ISSUE: reference to a compiler-generated field
        float4 s = this.m_TimeFactors * 0.125f;
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          LaneFlow laneFlow = nativeArray[index];
          laneFlow.m_Duration = math.lerp(laneFlow.m_Duration, (float4) laneFlow.m_Next.x, s);
          laneFlow.m_Distance = math.lerp(laneFlow.m_Distance, (float4) laneFlow.m_Next.y, s);
          laneFlow.m_Next = new float2();
          nativeArray[index] = laneFlow;
        }
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

    [BurstCompile]
    private struct UpdateRoadFlowJob : IJobChunk
    {
      [ReadOnly]
      public float4 m_TimeFactors;
      [ReadOnly]
      public BufferTypeHandle<SubLane> m_SubLaneType;
      [ReadOnly]
      public ComponentLookup<LaneFlow> m_LaneFlowData;
      [ReadOnly]
      public ComponentLookup<MasterLane> m_MasterLaneData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public BufferLookup<SubLane> m_SubLanes;
      public ComponentTypeHandle<Road> m_RoadType;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<CarLane> m_CarLaneData;
      public NativeQueue<FlowActionData>.ParallelWriter m_FlowActions;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Road> nativeArray = chunk.GetNativeArray<Road>(ref this.m_RoadType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubLane> bufferAccessor = chunk.GetBufferAccessor<SubLane>(ref this.m_SubLaneType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          Road road = nativeArray[index1];
          DynamicBuffer<SubLane> dynamicBuffer = bufferAccessor[index1];
          road.m_TrafficFlowDuration0 = new float4();
          road.m_TrafficFlowDuration1 = new float4();
          road.m_TrafficFlowDistance0 = new float4();
          road.m_TrafficFlowDistance1 = new float4();
          int index2 = -1;
          MasterLane masterLane = new MasterLane();
          float4 duration = new float4();
          float4 distance = new float4();
          bool isRoundabout1;
          for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
          {
            Entity subLane = dynamicBuffer[index3].m_SubLane;
            if (index2 != -1 && index3 > (int) masterLane.m_MaxIndex)
            {
              // ISSUE: reference to a compiler-generated method
              this.UpdateLaneFlow(dynamicBuffer[index2].m_SubLane, NetUtils.GetTrafficFlowSpeed(duration, distance), out isRoundabout1);
              index2 = -1;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneFlowData.HasComponent(subLane))
            {
              // ISSUE: reference to a compiler-generated field
              LaneFlow laneFlow = this.m_LaneFlowData[subLane];
              float4 trafficFlowSpeed = NetUtils.GetTrafficFlowSpeed(laneFlow.m_Duration, laneFlow.m_Distance);
              bool isRoundabout2;
              // ISSUE: reference to a compiler-generated method
              this.UpdateLaneFlow(subLane, trafficFlowSpeed, out isRoundabout2);
              EdgeLane componentData;
              // ISSUE: reference to a compiler-generated field
              float2 float2 = !this.m_EdgeLaneData.TryGetComponent(subLane, out componentData) ? (float2) math.select(1f, 0.333333343f, isRoundabout2) : math.select((float2) 0.0f, (float2) 1f, new bool2(math.any(componentData.m_EdgeDelta == 0.0f), math.any(componentData.m_EdgeDelta == 1f)));
              road.m_TrafficFlowDuration0 += laneFlow.m_Duration * float2.x;
              road.m_TrafficFlowDuration1 += laneFlow.m_Duration * float2.y;
              road.m_TrafficFlowDistance0 += laneFlow.m_Distance * float2.x;
              road.m_TrafficFlowDistance1 += laneFlow.m_Distance * float2.y;
              duration += laneFlow.m_Duration;
              distance += laneFlow.m_Distance;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_MasterLaneData.HasComponent(subLane))
              {
                index2 = index3;
                // ISSUE: reference to a compiler-generated field
                masterLane = this.m_MasterLaneData[subLane];
                duration = new float4();
                distance = new float4();
              }
            }
          }
          if (index2 != -1)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateLaneFlow(dynamicBuffer[index2].m_SubLane, NetUtils.GetTrafficFlowSpeed(duration, distance), out isRoundabout1);
          }
          nativeArray[index1] = road;
        }
      }

      private void UpdateLaneFlow(Entity lane, float4 flowSpeed, out bool isRoundabout)
      {
        isRoundabout = false;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CarLaneData.HasComponent(lane))
          return;
        // ISSUE: reference to a compiler-generated field
        CarLane carLane = this.m_CarLaneData[lane];
        isRoundabout = (carLane.m_Flags & (CarLaneFlags.Approach | CarLaneFlags.Roundabout)) == CarLaneFlags.Roundabout;
        // ISSUE: reference to a compiler-generated field
        byte num = (byte) math.clamp(256 - Mathf.RoundToInt(math.dot(flowSpeed, this.m_TimeFactors) * 256f), 0, (int) byte.MaxValue);
        if ((int) num == (int) carLane.m_FlowOffset)
          return;
        carLane.m_FlowOffset = num;
        // ISSUE: reference to a compiler-generated field
        this.m_CarLaneData[lane] = carLane;
        // ISSUE: reference to a compiler-generated field
        this.m_FlowActions.Enqueue(new FlowActionData()
        {
          m_Owner = lane,
          m_FlowOffset = num
        });
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
      public ComponentTypeHandle<LaneFlow> __Game_Net_LaneFlow_RW_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubLane> __Game_Net_SubLane_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<LaneFlow> __Game_Net_LaneFlow_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MasterLane> __Game_Net_MasterLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubLane> __Game_Net_SubLane_RO_BufferLookup;
      public ComponentTypeHandle<Road> __Game_Net_Road_RW_ComponentTypeHandle;
      public ComponentLookup<CarLane> __Game_Net_CarLane_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneFlow_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LaneFlow>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneFlow_RO_ComponentLookup = state.GetComponentLookup<LaneFlow>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_MasterLane_RO_ComponentLookup = state.GetComponentLookup<MasterLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Road>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RW_ComponentLookup = state.GetComponentLookup<CarLane>();
      }
    }
  }
}
