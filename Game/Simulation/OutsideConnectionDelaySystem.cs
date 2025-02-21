// Decompiled with JetBrains decompiler
// Type: Game.Simulation.OutsideConnectionDelaySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using Game.Vehicles;
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
  public class OutsideConnectionDelaySystem : GameSystemBase
  {
    public const int UPDATES_PER_DAY = 64;
    private PathfindQueueSystem m_PathfindQueueSystem;
    private EntityQuery m_NodeQuery;
    private OutsideConnectionDelaySystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 4096;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindQueueSystem = this.World.GetOrCreateSystemManaged<PathfindQueueSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NodeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Node>(), ComponentType.ReadOnly<Game.Net.OutsideConnection>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_NodeQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      TimeAction action = new TimeAction(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_OutsideConnection_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PathfindConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle dependencies = new OutsideConnectionDelaySystem.OutsideConnectionDelayJob()
      {
        m_SubLaneType = this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle,
        m_ConnectedEdgeType = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferTypeHandle,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_CarCurrentLaneData = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabNetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_PrefabPathfindConnectionData = this.__TypeHandle.__Game_Prefabs_PathfindConnectionData_RO_ComponentLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_OutsideConnectionData = this.__TypeHandle.__Game_Net_OutsideConnection_RW_ComponentLookup,
        m_TimeActions = action.m_TimeData.AsParallelWriter()
      }.ScheduleParallel<OutsideConnectionDelaySystem.OutsideConnectionDelayJob>(this.m_NodeQuery, this.Dependency);
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
    public OutsideConnectionDelaySystem()
    {
    }

    private struct AccumulationData
    {
      public Entity m_Lane;
      public float2 m_Delay;
    }

    [BurstCompile]
    private struct OutsideConnectionDelayJob : IJobChunk
    {
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubLane> m_SubLaneType;
      [ReadOnly]
      public BufferTypeHandle<ConnectedEdge> m_ConnectedEdgeType;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<CarCurrentLane> m_CarCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabNetLaneData;
      [ReadOnly]
      public ComponentLookup<PathfindConnectionData> m_PrefabPathfindConnectionData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Net.OutsideConnection> m_OutsideConnectionData;
      public NativeQueue<TimeActionData>.ParallelWriter m_TimeActions;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Net.SubLane> bufferAccessor1 = chunk.GetBufferAccessor<Game.Net.SubLane>(ref this.m_SubLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ConnectedEdge> bufferAccessor2 = chunk.GetBufferAccessor<ConnectedEdge>(ref this.m_ConnectedEdgeType);
        NativeParallelHashMap<PathNode, OutsideConnectionDelaySystem.AccumulationData> nativeParallelHashMap = new NativeParallelHashMap<PathNode, OutsideConnectionDelaySystem.AccumulationData>();
        for (int index1 = 0; index1 < bufferAccessor1.Length; ++index1)
        {
          DynamicBuffer<Game.Net.SubLane> dynamicBuffer1 = bufferAccessor1[index1];
          bool flag = false;
          for (int index2 = 0; index2 < dynamicBuffer1.Length; ++index2)
          {
            Game.Net.SubLane subLane = dynamicBuffer1[index2];
            // ISSUE: reference to a compiler-generated field
            if ((subLane.m_PathMethods & PathMethod.Road) != (PathMethod) 0 && this.m_ConnectionLaneData.HasComponent(subLane.m_SubLane))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Net.ConnectionLane connectionLane = this.m_ConnectionLaneData[subLane.m_SubLane];
              if ((connectionLane.m_Flags & ConnectionLaneFlags.Start) != (ConnectionLaneFlags) 0 && (connectionLane.m_RoadTypes & RoadTypes.Car) != RoadTypes.None)
              {
                // ISSUE: reference to a compiler-generated field
                Lane lane = this.m_LaneData[subLane.m_SubLane];
                if (!nativeParallelHashMap.IsCreated)
                  nativeParallelHashMap = new NativeParallelHashMap<PathNode, OutsideConnectionDelaySystem.AccumulationData>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                // ISSUE: object of a compiler-generated type is created
                nativeParallelHashMap.TryAdd(lane.m_StartNode, new OutsideConnectionDelaySystem.AccumulationData()
                {
                  m_Lane = subLane.m_SubLane
                });
                flag = true;
              }
            }
          }
          if (flag)
          {
            DynamicBuffer<ConnectedEdge> dynamicBuffer2 = bufferAccessor2[index1];
            for (int index3 = 0; index3 < dynamicBuffer2.Length; ++index3)
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[dynamicBuffer2[index3].m_Edge];
              for (int index4 = 0; index4 < subLane1.Length; ++index4)
              {
                Game.Net.SubLane subLane2 = subLane1[index4];
                if ((subLane2.m_PathMethods & PathMethod.Road) != (PathMethod) 0)
                {
                  SlaveLane componentData;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Lane lane = !this.m_SlaveLaneData.TryGetComponent(subLane2.m_SubLane, out componentData) ? this.m_LaneData[subLane2.m_SubLane] : this.m_LaneData[subLane1[(int) componentData.m_MasterIndex].m_SubLane];
                  // ISSUE: variable of a compiler-generated type
                  OutsideConnectionDelaySystem.AccumulationData accumulationData;
                  if (nativeParallelHashMap.TryGetValue(lane.m_StartNode, out accumulationData))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    accumulationData.m_Delay.x += this.CalculateDelay(subLane2.m_SubLane);
                    // ISSUE: reference to a compiler-generated field
                    ++accumulationData.m_Delay.y;
                    nativeParallelHashMap[lane.m_StartNode] = accumulationData;
                  }
                }
              }
            }
            NativeParallelHashMap<PathNode, OutsideConnectionDelaySystem.AccumulationData>.Enumerator enumerator = nativeParallelHashMap.GetEnumerator();
            while (enumerator.MoveNext())
            {
              // ISSUE: variable of a compiler-generated type
              OutsideConnectionDelaySystem.AccumulationData accumulationData = enumerator.Current.Value;
              Game.Net.OutsideConnection componentData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_OutsideConnectionData.TryGetComponent(accumulationData.m_Lane, out componentData))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                componentData.m_Delay = math.select(accumulationData.m_Delay.x / accumulationData.m_Delay.y, 0.0f, (double) accumulationData.m_Delay.y == 0.0);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_OutsideConnectionData[accumulationData.m_Lane] = componentData;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Lane lane = this.m_LaneData[accumulationData.m_Lane];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                PathfindConnectionData pathfindConnectionData = this.m_PrefabPathfindConnectionData[this.m_PrefabNetLaneData[this.m_PrefabRefData[accumulationData.m_Lane].m_Prefab].m_PathfindPrefab];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_TimeActions.Enqueue(new TimeActionData()
                {
                  m_Owner = accumulationData.m_Lane,
                  m_StartNode = lane.m_StartNode,
                  m_EndNode = lane.m_EndNode,
                  m_SecondaryStartNode = lane.m_StartNode,
                  m_SecondaryEndNode = lane.m_EndNode,
                  m_Flags = TimeActionFlags.SetPrimary | TimeActionFlags.SetSecondary | TimeActionFlags.EnableForward | TimeActionFlags.EnableBackward,
                  m_Time = pathfindConnectionData.m_BorderCost.m_Value.x + componentData.m_Delay
                });
              }
            }
            enumerator.Dispose();
            nativeParallelHashMap.Clear();
          }
        }
        if (!nativeParallelHashMap.IsCreated)
          return;
        nativeParallelHashMap.Dispose();
      }

      private float CalculateDelay(Entity lane)
      {
        float delay = 0.0f;
        DynamicBuffer<LaneObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_LaneObjects.TryGetBuffer(lane, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            CarCurrentLane componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CarCurrentLaneData.TryGetComponent(bufferData[index].m_LaneObject, out componentData) && (componentData.m_LaneFlags & Game.Vehicles.CarLaneFlags.ResetSpeed) != (Game.Vehicles.CarLaneFlags) 0)
              delay += componentData.m_Duration;
          }
        }
        return delay;
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
      public BufferTypeHandle<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathfindConnectionData> __Game_Prefabs_PathfindConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      public ComponentLookup<Game.Net.OutsideConnection> __Game_Net_OutsideConnection_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup = state.GetComponentLookup<CarCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindConnectionData_RO_ComponentLookup = state.GetComponentLookup<PathfindConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_OutsideConnection_RW_ComponentLookup = state.GetComponentLookup<Game.Net.OutsideConnection>();
      }
    }
  }
}
