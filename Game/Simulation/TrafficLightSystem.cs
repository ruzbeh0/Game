// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TrafficLightSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TrafficLightSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 64;
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_TrafficLightQuery;
    private TrafficLightSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 4;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TrafficLightQuery = this.GetEntityQuery(ComponentType.ReadWrite<TrafficLights>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TrafficLightQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TrafficLightQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TrafficLightQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(SimulationUtils.GetUpdateFrameWithInterval(this.m_SimulationSystem.frameIndex, (uint) this.GetUpdateInterval(SystemUpdatePhase.GameSimulation), 16)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TrafficLight_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneSignal_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrafficLights_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      this.Dependency = new TrafficLightSystem.UpdateTrafficLightsJob()
      {
        m_SubLaneType = this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle,
        m_SubObjectType = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle,
        m_TrafficLightsType = this.__TypeHandle.__Game_Net_TrafficLights_RW_ComponentTypeHandle,
        m_LaneSignalData = this.__TypeHandle.__Game_Net_LaneSignal_RW_ComponentLookup,
        m_TrafficLightData = this.__TypeHandle.__Game_Objects_TrafficLight_RW_ComponentLookup
      }.ScheduleParallel<TrafficLightSystem.UpdateTrafficLightsJob>(this.m_TrafficLightQuery, this.Dependency);
    }

    public static void UpdateLaneSignal(TrafficLights trafficLights, ref Game.Net.LaneSignal laneSignal)
    {
      int num1 = 0;
      int num2 = 0;
      if (trafficLights.m_CurrentSignalGroup > (byte) 0)
        num1 |= 1 << (int) trafficLights.m_CurrentSignalGroup - 1;
      if (trafficLights.m_NextSignalGroup > (byte) 0)
        num2 |= 1 << (int) trafficLights.m_NextSignalGroup - 1;
      switch (trafficLights.m_State)
      {
        case Game.Net.TrafficLightState.Beginning:
          if (((int) laneSignal.m_GroupMask & num2) != 0)
          {
            if (laneSignal.m_Signal == LaneSignalType.Go)
              break;
            laneSignal.m_Signal = LaneSignalType.Yield;
            break;
          }
          laneSignal.m_Signal = LaneSignalType.Stop;
          break;
        case Game.Net.TrafficLightState.Ongoing:
          if (((int) laneSignal.m_GroupMask & num1) != 0)
          {
            laneSignal.m_Signal = LaneSignalType.Go;
            break;
          }
          laneSignal.m_Signal = LaneSignalType.Stop;
          break;
        case Game.Net.TrafficLightState.Ending:
          if (laneSignal.m_Signal == LaneSignalType.Go)
          {
            if (((int) laneSignal.m_GroupMask & num2) != 0)
              break;
            laneSignal.m_Signal = LaneSignalType.SafeStop;
            break;
          }
          laneSignal.m_Signal = LaneSignalType.Stop;
          break;
        case Game.Net.TrafficLightState.Changing:
          if (laneSignal.m_Signal == LaneSignalType.Go && ((int) laneSignal.m_GroupMask & num2) != 0)
            break;
          laneSignal.m_Signal = LaneSignalType.Stop;
          break;
        case Game.Net.TrafficLightState.Extending:
          if ((laneSignal.m_Flags & LaneSignalFlags.CanExtend) != (LaneSignalFlags) 0)
          {
            if (((int) laneSignal.m_GroupMask & num1) != 0)
            {
              laneSignal.m_Signal = LaneSignalType.Go;
              break;
            }
            laneSignal.m_Signal = LaneSignalType.Stop;
            break;
          }
          if (laneSignal.m_Signal == LaneSignalType.Go)
          {
            if (((int) laneSignal.m_GroupMask & num2) != 0)
              break;
            laneSignal.m_Signal = LaneSignalType.SafeStop;
            break;
          }
          laneSignal.m_Signal = LaneSignalType.Stop;
          break;
        case Game.Net.TrafficLightState.Extended:
          if ((laneSignal.m_Flags & LaneSignalFlags.CanExtend) != (LaneSignalFlags) 0 && ((int) laneSignal.m_GroupMask & num1) != 0)
          {
            laneSignal.m_Signal = LaneSignalType.Go;
            break;
          }
          laneSignal.m_Signal = LaneSignalType.Stop;
          break;
        default:
          laneSignal.m_Signal = LaneSignalType.None;
          break;
      }
    }

    public static void UpdateTrafficLightState(
      TrafficLights trafficLights,
      ref TrafficLight trafficLight)
    {
      int num1 = 0;
      int num2 = 0;
      if (trafficLights.m_CurrentSignalGroup > (byte) 0)
        num1 |= 1 << (int) trafficLights.m_CurrentSignalGroup - 1;
      if (trafficLights.m_NextSignalGroup > (byte) 0)
        num2 |= 1 << (int) trafficLights.m_NextSignalGroup - 1;
      Game.Objects.TrafficLightState trafficLightState1 = trafficLight.m_State & (Game.Objects.TrafficLightState.Red | Game.Objects.TrafficLightState.Yellow | Game.Objects.TrafficLightState.Green | Game.Objects.TrafficLightState.Flashing);
      Game.Objects.TrafficLightState trafficLightState2 = (Game.Objects.TrafficLightState) ((int) trafficLight.m_State >> 4 & 15);
      Game.Objects.TrafficLightState trafficLightState3 = (trafficLights.m_Flags & TrafficLightFlags.LevelCrossing) != (TrafficLightFlags) 0 ? Game.Objects.TrafficLightState.Yellow | Game.Objects.TrafficLightState.Flashing : Game.Objects.TrafficLightState.Yellow;
      Game.Objects.TrafficLightState trafficLightState4 = (trafficLights.m_Flags & TrafficLightFlags.LevelCrossing) != (TrafficLightFlags) 0 ? Game.Objects.TrafficLightState.Red | Game.Objects.TrafficLightState.Flashing : Game.Objects.TrafficLightState.Red;
      switch (trafficLights.m_State)
      {
        case Game.Net.TrafficLightState.Beginning:
          if (((int) trafficLight.m_GroupMask0 & num2) != 0)
          {
            if (trafficLightState1 != Game.Objects.TrafficLightState.Green)
              trafficLightState1 = trafficLightState4 | trafficLightState3;
          }
          else
            trafficLightState1 = trafficLightState4;
          trafficLightState2 = ((int) trafficLight.m_GroupMask1 & num2) == 0 ? Game.Objects.TrafficLightState.Red : Game.Objects.TrafficLightState.Green;
          break;
        case Game.Net.TrafficLightState.Ongoing:
          trafficLightState1 = ((int) trafficLight.m_GroupMask0 & num1) == 0 ? trafficLightState4 : Game.Objects.TrafficLightState.Green;
          trafficLightState2 = ((int) trafficLight.m_GroupMask1 & num1) == 0 ? Game.Objects.TrafficLightState.Red : Game.Objects.TrafficLightState.Green;
          break;
        case Game.Net.TrafficLightState.Ending:
          if (trafficLightState1 == Game.Objects.TrafficLightState.Green)
          {
            if (((int) trafficLight.m_GroupMask0 & num2) == 0)
              trafficLightState1 = trafficLightState3;
          }
          else
            trafficLightState1 = trafficLightState4;
          if (trafficLightState2 == Game.Objects.TrafficLightState.Green)
          {
            if (((int) trafficLight.m_GroupMask1 & num2) == 0)
            {
              trafficLightState2 = Game.Objects.TrafficLightState.Green | Game.Objects.TrafficLightState.Flashing;
              break;
            }
            break;
          }
          trafficLightState2 = Game.Objects.TrafficLightState.Red;
          break;
        case Game.Net.TrafficLightState.Changing:
          if (trafficLightState1 != Game.Objects.TrafficLightState.Green || ((int) trafficLight.m_GroupMask0 & num2) == 0)
            trafficLightState1 = trafficLightState4;
          if (trafficLightState2 != Game.Objects.TrafficLightState.Green || ((int) trafficLight.m_GroupMask1 & num2) == 0)
          {
            trafficLightState2 = Game.Objects.TrafficLightState.Red;
            break;
          }
          break;
        case Game.Net.TrafficLightState.Extending:
          trafficLightState1 = ((int) trafficLight.m_GroupMask0 & num1) == 0 ? trafficLightState4 : Game.Objects.TrafficLightState.Green;
          if (trafficLightState2 == Game.Objects.TrafficLightState.Green)
          {
            if (((int) trafficLight.m_GroupMask1 & num2) == 0)
            {
              trafficLightState2 = Game.Objects.TrafficLightState.Green | Game.Objects.TrafficLightState.Flashing;
              break;
            }
            break;
          }
          trafficLightState2 = Game.Objects.TrafficLightState.Red;
          break;
        case Game.Net.TrafficLightState.Extended:
          trafficLightState1 = ((int) trafficLight.m_GroupMask0 & num1) == 0 ? trafficLightState4 : Game.Objects.TrafficLightState.Green;
          if (trafficLightState2 != Game.Objects.TrafficLightState.Green || ((int) trafficLight.m_GroupMask1 & num2) == 0)
          {
            trafficLightState2 = Game.Objects.TrafficLightState.Red;
            break;
          }
          break;
        default:
          trafficLightState1 = Game.Objects.TrafficLightState.None;
          trafficLightState2 = Game.Objects.TrafficLightState.None;
          break;
      }
      trafficLight.m_State = trafficLightState1 | (Game.Objects.TrafficLightState) ((int) trafficLightState2 << 4);
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
    public TrafficLightSystem()
    {
    }

    [BurstCompile]
    private struct UpdateTrafficLightsJob : IJobChunk
    {
      [ReadOnly]
      public BufferTypeHandle<SubLane> m_SubLaneType;
      [ReadOnly]
      public BufferTypeHandle<SubObject> m_SubObjectType;
      public ComponentTypeHandle<TrafficLights> m_TrafficLightsType;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Net.LaneSignal> m_LaneSignalData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<TrafficLight> m_TrafficLightData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<TrafficLights> nativeArray = chunk.GetNativeArray<TrafficLights>(ref this.m_TrafficLightsType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubLane> bufferAccessor1 = chunk.GetBufferAccessor<SubLane>(ref this.m_SubLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubObject> bufferAccessor2 = chunk.GetBufferAccessor<SubObject>(ref this.m_SubObjectType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          TrafficLights trafficLights = nativeArray[index];
          // ISSUE: reference to a compiler-generated method
          this.UpdateTrafficLightState(bufferAccessor1[index], bufferAccessor2[index], ref trafficLights);
          nativeArray[index] = trafficLights;
        }
      }

      private void UpdateTrafficLightState(
        DynamicBuffer<SubLane> subLanes,
        DynamicBuffer<SubObject> subObjects,
        ref TrafficLights trafficLights)
      {
        bool canExtend1;
        switch (trafficLights.m_State)
        {
          case Game.Net.TrafficLightState.None:
            if (++trafficLights.m_Timer >= (byte) 1)
            {
              trafficLights.m_State = Game.Net.TrafficLightState.Beginning;
              trafficLights.m_CurrentSignalGroup = (byte) 0;
              // ISSUE: reference to a compiler-generated method
              trafficLights.m_NextSignalGroup = (byte) this.GetNextSignalGroup(subLanes, trafficLights, true, out canExtend1);
              trafficLights.m_Timer = (byte) 0;
              // ISSUE: reference to a compiler-generated method
              this.UpdateLaneSignals(subLanes, trafficLights);
              // ISSUE: reference to a compiler-generated method
              this.UpdateTrafficLightObjects(subObjects, trafficLights);
              break;
            }
            // ISSUE: reference to a compiler-generated method
            this.ClearPriority(subLanes);
            break;
          case Game.Net.TrafficLightState.Beginning:
            if (++trafficLights.m_Timer >= (byte) 1)
            {
              trafficLights.m_State = Game.Net.TrafficLightState.Ongoing;
              trafficLights.m_CurrentSignalGroup = trafficLights.m_NextSignalGroup;
              trafficLights.m_NextSignalGroup = (byte) 0;
              trafficLights.m_Timer = (byte) 0;
              // ISSUE: reference to a compiler-generated method
              this.UpdateLaneSignals(subLanes, trafficLights);
              // ISSUE: reference to a compiler-generated method
              this.UpdateTrafficLightObjects(subObjects, trafficLights);
              break;
            }
            // ISSUE: reference to a compiler-generated method
            this.ClearPriority(subLanes);
            break;
          case Game.Net.TrafficLightState.Ongoing:
            if (++trafficLights.m_Timer >= (byte) 2)
            {
              bool canExtend2;
              // ISSUE: reference to a compiler-generated method
              int nextSignalGroup = this.GetNextSignalGroup(subLanes, trafficLights, trafficLights.m_Timer >= (byte) 6, out canExtend2);
              if (nextSignalGroup == (int) trafficLights.m_CurrentSignalGroup)
                break;
              if (canExtend2)
              {
                trafficLights.m_State = Game.Net.TrafficLightState.Extending;
                trafficLights.m_NextSignalGroup = (byte) nextSignalGroup;
                trafficLights.m_Timer = (byte) 0;
                // ISSUE: reference to a compiler-generated method
                this.UpdateLaneSignals(subLanes, trafficLights);
                // ISSUE: reference to a compiler-generated method
                this.UpdateTrafficLightObjects(subObjects, trafficLights);
                break;
              }
              trafficLights.m_State = Game.Net.TrafficLightState.Ending;
              trafficLights.m_NextSignalGroup = (byte) nextSignalGroup;
              trafficLights.m_Timer = (byte) 0;
              // ISSUE: reference to a compiler-generated method
              this.UpdateLaneSignals(subLanes, trafficLights);
              // ISSUE: reference to a compiler-generated method
              this.UpdateTrafficLightObjects(subObjects, trafficLights);
              break;
            }
            // ISSUE: reference to a compiler-generated method
            this.ClearPriority(subLanes);
            break;
          case Game.Net.TrafficLightState.Ending:
            if (++trafficLights.m_Timer >= (byte) 2)
            {
              // ISSUE: reference to a compiler-generated method
              int nextSignalGroup = this.GetNextSignalGroup(subLanes, trafficLights, true, out canExtend1);
              if (nextSignalGroup != (int) trafficLights.m_NextSignalGroup)
              {
                // ISSUE: reference to a compiler-generated method
                if (this.RequireEnding(subLanes, nextSignalGroup))
                  trafficLights.m_CurrentSignalGroup = trafficLights.m_NextSignalGroup;
                else
                  trafficLights.m_State = Game.Net.TrafficLightState.Changing;
                trafficLights.m_NextSignalGroup = (byte) nextSignalGroup;
              }
              else
                trafficLights.m_State = Game.Net.TrafficLightState.Changing;
              trafficLights.m_Timer = (byte) 0;
              // ISSUE: reference to a compiler-generated method
              this.UpdateLaneSignals(subLanes, trafficLights);
              // ISSUE: reference to a compiler-generated method
              this.UpdateTrafficLightObjects(subObjects, trafficLights);
              break;
            }
            // ISSUE: reference to a compiler-generated method
            this.ClearPriority(subLanes);
            break;
          case Game.Net.TrafficLightState.Changing:
            if (++trafficLights.m_Timer >= (byte) 1)
            {
              // ISSUE: reference to a compiler-generated method
              int nextSignalGroup = this.GetNextSignalGroup(subLanes, trafficLights, true, out canExtend1);
              if (nextSignalGroup != (int) trafficLights.m_NextSignalGroup)
              {
                // ISSUE: reference to a compiler-generated method
                if (this.RequireEnding(subLanes, nextSignalGroup))
                {
                  trafficLights.m_CurrentSignalGroup = trafficLights.m_NextSignalGroup;
                  trafficLights.m_State = Game.Net.TrafficLightState.Ending;
                }
                else
                  trafficLights.m_State = Game.Net.TrafficLightState.Beginning;
                trafficLights.m_NextSignalGroup = (byte) nextSignalGroup;
              }
              else
                trafficLights.m_State = Game.Net.TrafficLightState.Beginning;
              trafficLights.m_Timer = (byte) 0;
              // ISSUE: reference to a compiler-generated method
              this.UpdateLaneSignals(subLanes, trafficLights);
              // ISSUE: reference to a compiler-generated method
              this.UpdateTrafficLightObjects(subObjects, trafficLights);
              break;
            }
            // ISSUE: reference to a compiler-generated method
            this.ClearPriority(subLanes);
            break;
          case Game.Net.TrafficLightState.Extending:
            if (++trafficLights.m_Timer >= (byte) 2)
            {
              bool canExtend3;
              // ISSUE: reference to a compiler-generated method
              int nextSignalGroup = this.GetNextSignalGroup(subLanes, trafficLights, true, out canExtend3);
              if (nextSignalGroup == (int) trafficLights.m_CurrentSignalGroup)
              {
                trafficLights.m_State = Game.Net.TrafficLightState.Beginning;
                trafficLights.m_CurrentSignalGroup = (byte) 0;
              }
              else
                trafficLights.m_State = canExtend3 ? Game.Net.TrafficLightState.Extended : Game.Net.TrafficLightState.Ending;
              trafficLights.m_NextSignalGroup = (byte) nextSignalGroup;
              trafficLights.m_Timer = (byte) 0;
              // ISSUE: reference to a compiler-generated method
              this.UpdateLaneSignals(subLanes, trafficLights);
              // ISSUE: reference to a compiler-generated method
              this.UpdateTrafficLightObjects(subObjects, trafficLights);
              break;
            }
            // ISSUE: reference to a compiler-generated method
            this.ClearPriority(subLanes);
            break;
          case Game.Net.TrafficLightState.Extended:
            if (++trafficLights.m_Timer >= (byte) 2)
            {
              bool canExtend4;
              // ISSUE: reference to a compiler-generated method
              int nextSignalGroup = this.GetNextSignalGroup(subLanes, trafficLights, true, out canExtend4);
              if (nextSignalGroup == (int) trafficLights.m_CurrentSignalGroup)
              {
                trafficLights.m_State = Game.Net.TrafficLightState.Beginning;
                trafficLights.m_CurrentSignalGroup = (byte) 0;
                trafficLights.m_NextSignalGroup = (byte) nextSignalGroup;
                trafficLights.m_Timer = (byte) 0;
                // ISSUE: reference to a compiler-generated method
                this.UpdateLaneSignals(subLanes, trafficLights);
                // ISSUE: reference to a compiler-generated method
                this.UpdateTrafficLightObjects(subObjects, trafficLights);
                break;
              }
              if (trafficLights.m_Timer < (byte) 4 && canExtend4)
                break;
              trafficLights.m_State = Game.Net.TrafficLightState.Ending;
              trafficLights.m_NextSignalGroup = (byte) nextSignalGroup;
              trafficLights.m_Timer = (byte) 0;
              // ISSUE: reference to a compiler-generated method
              this.UpdateLaneSignals(subLanes, trafficLights);
              // ISSUE: reference to a compiler-generated method
              this.UpdateTrafficLightObjects(subObjects, trafficLights);
              break;
            }
            // ISSUE: reference to a compiler-generated method
            this.ClearPriority(subLanes);
            break;
        }
      }

      private bool RequireEnding(DynamicBuffer<SubLane> subLanes, int nextSignalGroup)
      {
        int num = 0;
        if (nextSignalGroup > 0)
          num |= 1 << nextSignalGroup - 1;
        for (int index = 0; index < subLanes.Length; ++index)
        {
          Entity subLane = subLanes[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneSignalData.HasComponent(subLane))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Net.LaneSignal laneSignal = this.m_LaneSignalData[subLane];
            if (laneSignal.m_Signal == LaneSignalType.Go && ((int) laneSignal.m_GroupMask & num) == 0)
              return true;
          }
        }
        return false;
      }

      private int GetNextSignalGroup(
        DynamicBuffer<SubLane> subLanes,
        TrafficLights trafficLights,
        bool preferChange,
        out bool canExtend)
      {
        Entity petitioner = Entity.Null;
        Entity blocker = Entity.Null;
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        for (int index = 0; index < subLanes.Length; ++index)
        {
          Entity subLane = subLanes[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneSignalData.HasComponent(subLane))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Net.LaneSignal laneSignal = this.m_LaneSignalData[subLane];
            if ((int) laneSignal.m_Priority > num1)
            {
              petitioner = laneSignal.m_Petitioner;
              num1 = (int) laneSignal.m_Priority;
              num2 = (int) laneSignal.m_GroupMask;
              num3 = math.select(0, (int) laneSignal.m_GroupMask, (laneSignal.m_Flags & LaneSignalFlags.CanExtend) != 0);
            }
            else if ((int) laneSignal.m_Priority == num1)
            {
              num2 |= (int) laneSignal.m_GroupMask;
              num3 |= math.select(0, (int) laneSignal.m_GroupMask, (laneSignal.m_Flags & LaneSignalFlags.CanExtend) != 0);
            }
            else if (laneSignal.m_Priority < (sbyte) 0)
              num4 |= (int) laneSignal.m_GroupMask;
            if (laneSignal.m_Blocker != Entity.Null)
              blocker = laneSignal.m_Blocker;
            laneSignal.m_Petitioner = Entity.Null;
            laneSignal.m_Priority = laneSignal.m_Default;
            // ISSUE: reference to a compiler-generated field
            this.m_LaneSignalData[subLane] = laneSignal;
          }
        }
        if (petitioner != blocker)
        {
          for (int index = 0; index < subLanes.Length; ++index)
          {
            Entity subLane = subLanes[index].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneSignalData.HasComponent(subLane))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Net.LaneSignal laneSignal = this.m_LaneSignalData[subLane];
              laneSignal.m_Blocker = (num2 & (int) laneSignal.m_GroupMask) == 0 ? petitioner : Entity.Null;
              // ISSUE: reference to a compiler-generated field
              this.m_LaneSignalData[subLane] = laneSignal;
            }
          }
        }
        if (num1 == 0)
        {
          preferChange = false;
          num2 &= ~num4;
        }
        int b = (int) (byte) math.select((int) trafficLights.m_CurrentSignalGroup + 1, 1, (int) trafficLights.m_CurrentSignalGroup >= (int) trafficLights.m_SignalGroupCount);
        int num5 = math.select(math.max(1, (int) trafficLights.m_CurrentSignalGroup), b, preferChange);
        int num6 = math.select((int) trafficLights.m_CurrentSignalGroup - 1, (int) trafficLights.m_CurrentSignalGroup, preferChange);
        canExtend = preferChange && trafficLights.m_CurrentSignalGroup >= (byte) 1 && (num3 & 1 << (int) trafficLights.m_CurrentSignalGroup - 1) != 0;
        for (int nextSignalGroup = num5; nextSignalGroup <= (int) trafficLights.m_SignalGroupCount; ++nextSignalGroup)
        {
          if ((num2 & 1 << nextSignalGroup - 1) != 0)
            return nextSignalGroup;
        }
        for (int nextSignalGroup = 1; nextSignalGroup <= num6; ++nextSignalGroup)
        {
          if ((num2 & 1 << nextSignalGroup - 1) != 0)
            return nextSignalGroup;
        }
        return (int) trafficLights.m_CurrentSignalGroup;
      }

      private void UpdateLaneSignals(DynamicBuffer<SubLane> subLanes, TrafficLights trafficLights)
      {
        for (int index = 0; index < subLanes.Length; ++index)
        {
          Entity subLane = subLanes[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneSignalData.HasComponent(subLane))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Net.LaneSignal laneSignal = this.m_LaneSignalData[subLane];
            // ISSUE: reference to a compiler-generated method
            TrafficLightSystem.UpdateLaneSignal(trafficLights, ref laneSignal);
            laneSignal.m_Petitioner = Entity.Null;
            laneSignal.m_Priority = laneSignal.m_Default;
            // ISSUE: reference to a compiler-generated field
            this.m_LaneSignalData[subLane] = laneSignal;
          }
        }
      }

      private void UpdateTrafficLightObjects(
        DynamicBuffer<SubObject> subObjects,
        TrafficLights trafficLights)
      {
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TrafficLightData.HasComponent(subObject))
          {
            // ISSUE: reference to a compiler-generated field
            TrafficLight trafficLight = this.m_TrafficLightData[subObject];
            // ISSUE: reference to a compiler-generated method
            TrafficLightSystem.UpdateTrafficLightState(trafficLights, ref trafficLight);
            // ISSUE: reference to a compiler-generated field
            this.m_TrafficLightData[subObject] = trafficLight;
          }
        }
      }

      private void ClearPriority(DynamicBuffer<SubLane> subLanes)
      {
        for (int index = 0; index < subLanes.Length; ++index)
        {
          Entity subLane = subLanes[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneSignalData.HasComponent(subLane))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Net.LaneSignal laneSignal = this.m_LaneSignalData[subLane] with
            {
              m_Petitioner = Entity.Null
            };
            laneSignal.m_Priority = laneSignal.m_Default;
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
      [ReadOnly]
      public BufferTypeHandle<SubObject> __Game_Objects_SubObject_RO_BufferTypeHandle;
      public ComponentTypeHandle<TrafficLights> __Game_Net_TrafficLights_RW_ComponentTypeHandle;
      public ComponentLookup<Game.Net.LaneSignal> __Game_Net_LaneSignal_RW_ComponentLookup;
      public ComponentLookup<TrafficLight> __Game_Objects_TrafficLight_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrafficLights_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TrafficLights>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneSignal_RW_ComponentLookup = state.GetComponentLookup<Game.Net.LaneSignal>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TrafficLight_RW_ComponentLookup = state.GetComponentLookup<TrafficLight>();
      }
    }
  }
}
