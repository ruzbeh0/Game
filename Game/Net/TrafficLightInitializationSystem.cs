// Decompiled with JetBrains decompiler
// Type: Game.Net.TrafficLightInitializationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class TrafficLightInitializationSystem : GameSystemBase
  {
    private EntityQuery m_TrafficLightsQuery;
    private TrafficLightInitializationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TrafficLightsQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<TrafficLights>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<Updated>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TrafficLightsQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneSignal_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrafficLights_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      this.Dependency = new TrafficLightInitializationSystem.InitializeTrafficLightsJob()
      {
        m_SubLaneType = this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle,
        m_TrafficLightsType = this.__TypeHandle.__Game_Net_TrafficLights_RW_ComponentTypeHandle,
        m_MasterLaneData = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_PedestrianLaneData = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup,
        m_SecondaryLaneData = this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_Overlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_LaneSignalData = this.__TypeHandle.__Game_Net_LaneSignal_RW_ComponentLookup
      }.ScheduleParallel<TrafficLightInitializationSystem.InitializeTrafficLightsJob>(this.m_TrafficLightsQuery, this.Dependency);
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
    public TrafficLightInitializationSystem()
    {
    }

    private struct LaneGroup
    {
      public float2 m_StartDirection;
      public float2 m_EndDirection;
      public int2 m_LaneRange;
      public int m_GroupIndex;
      public ushort m_GroupMask;
      public bool m_IsStraight;
      public bool m_IsCombined;
      public bool m_IsUnsafe;
      public bool m_IsTrack;
    }

    [BurstCompile]
    private struct InitializeTrafficLightsJob : IJobChunk
    {
      [ReadOnly]
      public BufferTypeHandle<SubLane> m_SubLaneType;
      public ComponentTypeHandle<TrafficLights> m_TrafficLightsType;
      [ReadOnly]
      public ComponentLookup<MasterLane> m_MasterLaneData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<PedestrianLane> m_PedestrianLaneData;
      [ReadOnly]
      public ComponentLookup<SecondaryLane> m_SecondaryLaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public BufferLookup<LaneOverlap> m_Overlaps;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<LaneSignal> m_LaneSignalData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeList<TrafficLightInitializationSystem.LaneGroup> groups = new NativeList<TrafficLightInitializationSystem.LaneGroup>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<TrafficLightInitializationSystem.LaneGroup> vehicleLanes = new NativeList<TrafficLightInitializationSystem.LaneGroup>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<TrafficLightInitializationSystem.LaneGroup> pedestrianLanes = new NativeList<TrafficLightInitializationSystem.LaneGroup>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TrafficLights> nativeArray = chunk.GetNativeArray<TrafficLights>(ref this.m_TrafficLightsType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubLane> bufferAccessor = chunk.GetBufferAccessor<SubLane>(ref this.m_SubLaneType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          TrafficLights trafficLights = nativeArray[index];
          DynamicBuffer<SubLane> subLanes = bufferAccessor[index];
          bool isLevelCrossing = (trafficLights.m_Flags & TrafficLightFlags.LevelCrossing) != 0;
          // ISSUE: reference to a compiler-generated method
          this.FillLaneBuffers(subLanes, vehicleLanes, pedestrianLanes);
          int groupCount;
          // ISSUE: reference to a compiler-generated method
          this.ProcessVehicleLaneGroups(vehicleLanes, groups, isLevelCrossing, out groupCount);
          // ISSUE: reference to a compiler-generated method
          this.ProcessPedestrianLaneGroups(subLanes, pedestrianLanes, groups, isLevelCrossing, ref groupCount);
          // ISSUE: reference to a compiler-generated method
          this.InitializeTrafficLights(subLanes, groups, groupCount, isLevelCrossing, ref trafficLights);
          nativeArray[index] = trafficLights;
          groups.Clear();
          vehicleLanes.Clear();
          pedestrianLanes.Clear();
        }
        groups.Dispose();
        vehicleLanes.Dispose();
        pedestrianLanes.Dispose();
      }

      private void FillLaneBuffers(
        DynamicBuffer<SubLane> subLanes,
        NativeList<TrafficLightInitializationSystem.LaneGroup> vehicleLanes,
        NativeList<TrafficLightInitializationSystem.LaneGroup> pedestrianLanes)
      {
        for (int index = 0; index < subLanes.Length; ++index)
        {
          Entity subLane = subLanes[index].m_SubLane;
          // ISSUE: variable of a compiler-generated type
          TrafficLightInitializationSystem.LaneGroup laneGroup1;
          float3 float3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneSignalData.HasComponent(subLane) && !this.m_SecondaryLaneData.HasComponent(subLane))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PedestrianLaneData.HasComponent(subLane))
            {
              // ISSUE: reference to a compiler-generated field
              PedestrianLane pedestrianLane = this.m_PedestrianLaneData[subLane];
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[subLane];
              // ISSUE: object of a compiler-generated type is created
              laneGroup1 = new TrafficLightInitializationSystem.LaneGroup();
              ref TrafficLightInitializationSystem.LaneGroup local1 = ref laneGroup1;
              float3 = MathUtils.StartTangent(curve.m_Bezier);
              float2 float2_1 = math.normalizesafe(float3.xz);
              // ISSUE: reference to a compiler-generated field
              local1.m_StartDirection = float2_1;
              ref TrafficLightInitializationSystem.LaneGroup local2 = ref laneGroup1;
              float3 = MathUtils.EndTangent(curve.m_Bezier);
              float2 float2_2 = math.normalizesafe(-float3.xz);
              // ISSUE: reference to a compiler-generated field
              local2.m_EndDirection = float2_2;
              // ISSUE: reference to a compiler-generated field
              laneGroup1.m_LaneRange = new int2(index, index);
              // ISSUE: reference to a compiler-generated field
              laneGroup1.m_IsUnsafe = (pedestrianLane.m_Flags & PedestrianLaneFlags.Unsafe) != 0;
              // ISSUE: variable of a compiler-generated type
              TrafficLightInitializationSystem.LaneGroup laneGroup2 = laneGroup1;
              pedestrianLanes.Add(in laneGroup2);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_MasterLaneData.HasComponent(subLane))
              {
                // ISSUE: reference to a compiler-generated field
                MasterLane masterLane = this.m_MasterLaneData[subLane];
                // ISSUE: reference to a compiler-generated field
                Curve curve = this.m_CurveData[subLane];
                // ISSUE: object of a compiler-generated type is created
                laneGroup1 = new TrafficLightInitializationSystem.LaneGroup();
                ref TrafficLightInitializationSystem.LaneGroup local3 = ref laneGroup1;
                float3 = MathUtils.StartTangent(curve.m_Bezier);
                float2 float2_3 = math.normalizesafe(float3.xz);
                // ISSUE: reference to a compiler-generated field
                local3.m_StartDirection = float2_3;
                ref TrafficLightInitializationSystem.LaneGroup local4 = ref laneGroup1;
                float3 = MathUtils.EndTangent(curve.m_Bezier);
                float2 float2_4 = math.normalizesafe(-float3.xz);
                // ISSUE: reference to a compiler-generated field
                local4.m_EndDirection = float2_4;
                // ISSUE: reference to a compiler-generated field
                laneGroup1.m_LaneRange = new int2((int) masterLane.m_MinIndex - 1, (int) masterLane.m_MaxIndex);
                // ISSUE: variable of a compiler-generated type
                TrafficLightInitializationSystem.LaneGroup laneGroup3 = laneGroup1;
                // ISSUE: reference to a compiler-generated field
                if (this.m_CarLaneData.HasComponent(subLane))
                {
                  // ISSUE: reference to a compiler-generated field
                  CarLane carLane = this.m_CarLaneData[subLane];
                  // ISSUE: reference to a compiler-generated field
                  laneGroup3.m_IsStraight = (carLane.m_Flags & (CarLaneFlags.UTurnLeft | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight)) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter);
                  // ISSUE: reference to a compiler-generated field
                  laneGroup3.m_IsUnsafe = (carLane.m_Flags & CarLaneFlags.Unsafe) > ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  laneGroup3.m_IsStraight = true;
                }
                vehicleLanes.Add(in laneGroup3);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.m_SlaveLaneData.HasComponent(subLane))
                {
                  // ISSUE: reference to a compiler-generated field
                  Curve curve = this.m_CurveData[subLane];
                  // ISSUE: object of a compiler-generated type is created
                  laneGroup1 = new TrafficLightInitializationSystem.LaneGroup();
                  ref TrafficLightInitializationSystem.LaneGroup local5 = ref laneGroup1;
                  float3 = MathUtils.StartTangent(curve.m_Bezier);
                  float2 float2_5 = math.normalizesafe(float3.xz);
                  // ISSUE: reference to a compiler-generated field
                  local5.m_StartDirection = float2_5;
                  ref TrafficLightInitializationSystem.LaneGroup local6 = ref laneGroup1;
                  float3 = MathUtils.EndTangent(curve.m_Bezier);
                  float2 float2_6 = math.normalizesafe(-float3.xz);
                  // ISSUE: reference to a compiler-generated field
                  local6.m_EndDirection = float2_6;
                  // ISSUE: reference to a compiler-generated field
                  laneGroup1.m_LaneRange = new int2(index, index);
                  // ISSUE: variable of a compiler-generated type
                  TrafficLightInitializationSystem.LaneGroup laneGroup4 = laneGroup1;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_CarLaneData.HasComponent(subLane))
                  {
                    // ISSUE: reference to a compiler-generated field
                    CarLane carLane = this.m_CarLaneData[subLane];
                    // ISSUE: reference to a compiler-generated field
                    laneGroup4.m_IsStraight = (carLane.m_Flags & (CarLaneFlags.UTurnLeft | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight)) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter);
                    // ISSUE: reference to a compiler-generated field
                    laneGroup4.m_IsUnsafe = (carLane.m_Flags & CarLaneFlags.Unsafe) > ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    laneGroup4.m_IsStraight = true;
                    // ISSUE: reference to a compiler-generated field
                    laneGroup4.m_IsTrack = true;
                  }
                  vehicleLanes.Add(in laneGroup4);
                }
              }
            }
          }
        }
      }

      private void ProcessVehicleLaneGroups(
        NativeList<TrafficLightInitializationSystem.LaneGroup> vehicleLanes,
        NativeList<TrafficLightInitializationSystem.LaneGroup> groups,
        bool isLevelCrossing,
        out int groupCount)
      {
        groupCount = 0;
        while (vehicleLanes.Length > 0)
        {
          // ISSUE: variable of a compiler-generated type
          TrafficLightInitializationSystem.LaneGroup vehicleLane1 = vehicleLanes[0] with
          {
            m_GroupIndex = groupCount++
          };
          groups.Add(in vehicleLane1);
          vehicleLanes.RemoveAtSwapBack(0);
          int index = 0;
          while (index < vehicleLanes.Length)
          {
            // ISSUE: variable of a compiler-generated type
            TrafficLightInitializationSystem.LaneGroup vehicleLane2 = vehicleLanes[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!isLevelCrossing | vehicleLane1.m_IsTrack == vehicleLane2.m_IsTrack && (double) math.dot(vehicleLane1.m_StartDirection, vehicleLane2.m_StartDirection) > 0.99900001287460327)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              vehicleLane2.m_GroupIndex = vehicleLane1.m_GroupIndex;
              groups.Add(in vehicleLane2);
              vehicleLanes.RemoveAtSwapBack(index);
            }
            else
              ++index;
          }
        }
        int index1 = 0;
        groupCount = 0;
        while (index1 < groups.Length)
        {
          // ISSUE: variable of a compiler-generated type
          TrafficLightInitializationSystem.LaneGroup group1 = groups[index1++];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          groupCount = math.select(group1.m_GroupIndex + 1, groupCount, group1.m_IsCombined);
          float2 x1 = new float2();
          float2 x2 = new float2();
          float num1 = 1f;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (group1.m_IsStraight && !group1.m_IsCombined)
          {
            // ISSUE: reference to a compiler-generated field
            x1 = group1.m_StartDirection;
            // ISSUE: reference to a compiler-generated field
            x2 = group1.m_EndDirection;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            num1 = math.dot(group1.m_StartDirection, group1.m_EndDirection);
          }
          for (; index1 < groups.Length; ++index1)
          {
            // ISSUE: variable of a compiler-generated type
            TrafficLightInitializationSystem.LaneGroup group2 = groups[index1];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (group2.m_GroupIndex == group1.m_GroupIndex)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (group2.m_IsStraight && !group2.m_IsCombined)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float num2 = math.dot(group2.m_StartDirection, group2.m_EndDirection);
                if ((double) num2 < (double) num1)
                {
                  // ISSUE: reference to a compiler-generated field
                  x1 = group2.m_StartDirection;
                  // ISSUE: reference to a compiler-generated field
                  x2 = group2.m_EndDirection;
                  num1 = num2;
                }
              }
            }
            else
              break;
          }
          if ((double) num1 < 0.0)
          {
            int index2 = index1;
            while (index2 < groups.Length)
            {
              int num3 = index2;
              int num4 = index2;
              // ISSUE: variable of a compiler-generated type
              TrafficLightInitializationSystem.LaneGroup group3 = groups[index2++];
              bool flag = false;
              // ISSUE: reference to a compiler-generated field
              if (!group3.m_IsCombined)
              {
                if (isLevelCrossing)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (group1.m_IsTrack == group3.m_IsTrack)
                    flag = true;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (group3.m_IsStraight && (double) math.dot(x1, group3.m_EndDirection) > 0.99900001287460327 && (double) math.dot(x2, group3.m_StartDirection) > 0.99900001287460327)
                    flag = true;
                }
              }
              for (; index2 < groups.Length; num4 = index2++)
              {
                // ISSUE: variable of a compiler-generated type
                TrafficLightInitializationSystem.LaneGroup group4 = groups[index2];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (group4.m_GroupIndex == group3.m_GroupIndex)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (!group4.m_IsCombined)
                  {
                    if (isLevelCrossing)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (group1.m_IsTrack == group4.m_IsTrack)
                        flag = true;
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (group4.m_IsStraight && (double) math.dot(x1, group4.m_EndDirection) > 0.99900001287460327 && (double) math.dot(x2, group4.m_StartDirection) > 0.99900001287460327)
                        flag = true;
                    }
                  }
                }
                else
                  break;
              }
              if (flag)
              {
                for (int index3 = num3; index3 <= num4; ++index3)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: variable of a compiler-generated type
                  TrafficLightInitializationSystem.LaneGroup group5 = groups[index3] with
                  {
                    m_GroupIndex = group1.m_GroupIndex,
                    m_IsCombined = true
                  };
                  groups[index3] = group5;
                }
                for (int index4 = num4 + 1; index4 < groups.Length; ++index4)
                {
                  // ISSUE: variable of a compiler-generated type
                  TrafficLightInitializationSystem.LaneGroup group6 = groups[index4];
                  // ISSUE: reference to a compiler-generated field
                  if (!group6.m_IsCombined)
                  {
                    // ISSUE: reference to a compiler-generated field
                    --group6.m_GroupIndex;
                    groups[index4] = group6;
                  }
                }
              }
            }
          }
        }
        for (int index5 = 0; index5 < groups.Length; ++index5)
        {
          // ISSUE: variable of a compiler-generated type
          TrafficLightInitializationSystem.LaneGroup group = groups[index5];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          group.m_GroupMask = (ushort) (1 << (group.m_GroupIndex & 15));
          groups[index5] = group;
        }
      }

      private void ProcessPedestrianLaneGroups(
        DynamicBuffer<SubLane> subLanes,
        NativeList<TrafficLightInitializationSystem.LaneGroup> pedestrianLanes,
        NativeList<TrafficLightInitializationSystem.LaneGroup> groups,
        bool isLevelCrossing,
        ref int groupCount)
      {
        if (groupCount <= 1)
        {
          int num = groupCount++;
          for (int index = 0; index < pedestrianLanes.Length; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            TrafficLightInitializationSystem.LaneGroup pedestrianLane = pedestrianLanes[index] with
            {
              m_GroupMask = (ushort) (1 << (num & 15))
            };
            groups.Add(in pedestrianLane);
          }
        }
        else
        {
          int length = groups.Length;
          int num = -1;
          for (int index1 = 0; index1 < pedestrianLanes.Length; ++index1)
          {
            // ISSUE: variable of a compiler-generated type
            TrafficLightInitializationSystem.LaneGroup pedestrianLane = pedestrianLanes[index1] with
            {
              m_GroupMask = (ushort) ((1 << math.min(16, groupCount)) - 1)
            };
            // ISSUE: reference to a compiler-generated field
            Entity subLane = subLanes[pedestrianLane.m_LaneRange.x].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!pedestrianLane.m_IsUnsafe && this.m_Overlaps.HasBuffer(subLane))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<LaneOverlap> overlap = this.m_Overlaps[subLane];
              for (int index2 = 0; index2 < length; ++index2)
              {
                // ISSUE: variable of a compiler-generated type
                TrafficLightInitializationSystem.LaneGroup group = groups[index2];
                bool flag1;
                if (isLevelCrossing)
                {
                  // ISSUE: reference to a compiler-generated field
                  flag1 = !group.m_IsTrack;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  flag1 = !group.m_IsStraight;
                  if (flag1)
                  {
                    float4 x;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    x.x = math.dot(pedestrianLane.m_StartDirection, group.m_StartDirection);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    x.y = math.dot(pedestrianLane.m_StartDirection, group.m_EndDirection);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    x.z = math.dot(pedestrianLane.m_EndDirection, group.m_StartDirection);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    x.w = math.dot(pedestrianLane.m_EndDirection, group.m_EndDirection);
                    x = math.abs(x);
                    flag1 = (double) x.x + (double) x.z > (double) x.y + (double) x.w;
                  }
                }
                bool flag2 = false;
                if (!flag1)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  for (int x = group.m_LaneRange.x; x <= group.m_LaneRange.y; ++x)
                  {
                    for (int index3 = 0; index3 < overlap.Length; ++index3)
                    {
                      LaneOverlap laneOverlap = overlap[index3];
                      flag2 |= laneOverlap.m_Other == subLanes[x].m_SubLane;
                    }
                  }
                }
                if (!flag1 & flag2)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  pedestrianLane.m_GroupMask &= ~group.m_GroupMask;
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (pedestrianLane.m_GroupMask == (ushort) 0)
            {
              if (num == -1)
                num = groupCount++;
              // ISSUE: reference to a compiler-generated field
              pedestrianLane.m_GroupMask = (ushort) (1 << (num & 15));
            }
            groups.Add(in pedestrianLane);
          }
        }
      }

      private void InitializeTrafficLights(
        DynamicBuffer<SubLane> subLanes,
        NativeList<TrafficLightInitializationSystem.LaneGroup> groups,
        int groupCount,
        bool isLevelCrossing,
        ref TrafficLights trafficLights)
      {
        trafficLights.m_SignalGroupCount = (byte) math.min(16, groupCount);
        if ((int) trafficLights.m_CurrentSignalGroup > (int) trafficLights.m_SignalGroupCount || (int) trafficLights.m_NextSignalGroup > (int) trafficLights.m_SignalGroupCount)
        {
          trafficLights.m_CurrentSignalGroup = (byte) 0;
          trafficLights.m_NextSignalGroup = (byte) 0;
          trafficLights.m_Timer = (byte) 0;
          trafficLights.m_State = TrafficLightState.None;
        }
        for (int index = 0; index < groups.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          TrafficLightInitializationSystem.LaneGroup group = groups[index];
          // ISSUE: reference to a compiler-generated field
          sbyte num = (sbyte) math.select(0, -1, isLevelCrossing & group.m_IsTrack);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          for (int x = group.m_LaneRange.x; x <= group.m_LaneRange.y; ++x)
          {
            Entity subLane = subLanes[x].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            LaneSignal laneSignal = this.m_LaneSignalData[subLane] with
            {
              m_GroupMask = group.m_GroupMask,
              m_Default = num
            };
            // ISSUE: reference to a compiler-generated field
            if (this.m_CarLaneData.HasComponent(subLane))
              laneSignal.m_Flags |= LaneSignalFlags.CanExtend;
            // ISSUE: reference to a compiler-generated method
            TrafficLightSystem.UpdateLaneSignal(trafficLights, ref laneSignal);
            // ISSUE: reference to a compiler-generated field
            this.m_LaneSignalData[subLane] = laneSignal;
          }
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

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferTypeHandle<SubLane> __Game_Net_SubLane_RO_BufferTypeHandle;
      public ComponentTypeHandle<TrafficLights> __Game_Net_TrafficLights_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<MasterLane> __Game_Net_MasterLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SecondaryLane> __Game_Net_SecondaryLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;
      public ComponentLookup<LaneSignal> __Game_Net_LaneSignal_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrafficLights_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TrafficLights>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_MasterLane_RO_ComponentLookup = state.GetComponentLookup<MasterLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentLookup = state.GetComponentLookup<PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SecondaryLane_RO_ComponentLookup = state.GetComponentLookup<SecondaryLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneSignal_RW_ComponentLookup = state.GetComponentLookup<LaneSignal>();
      }
    }
  }
}
