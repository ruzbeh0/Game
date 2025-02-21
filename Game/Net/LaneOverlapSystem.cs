// Decompiled with JetBrains decompiler
// Type: Game.Net.LaneOverlapSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.City;
using Game.Common;
using Game.Pathfind;
using Game.Prefabs;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class LaneOverlapSystem : GameSystemBase
  {
    private CityConfigurationSystem m_CityConfigurationSystem;
    private EntityQuery m_UpdatedOwnersQuery;
    private EntityQuery m_UpdatedLanesQuery;
    private EntityQuery m_AllOwnersQuery;
    private EntityQuery m_AllLanesQuery;
    private bool m_Loaded;
    private LaneOverlapSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedOwnersQuery = this.GetEntityQuery(ComponentType.ReadOnly<SubLane>(), ComponentType.ReadOnly<Updated>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedLanesQuery = this.GetEntityQuery(ComponentType.ReadOnly<Lane>(), ComponentType.ReadOnly<Updated>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<SecondaryLane>());
      // ISSUE: reference to a compiler-generated field
      this.m_AllOwnersQuery = this.GetEntityQuery(ComponentType.ReadOnly<SubLane>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_AllLanesQuery = this.GetEntityQuery(ComponentType.ReadOnly<Lane>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<SecondaryLane>());
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      int num = this.GetLoaded() ? 1 : 0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery entityQuery = num != 0 ? this.m_AllOwnersQuery : this.m_UpdatedOwnersQuery;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery query1 = num != 0 ? this.m_AllLanesQuery : this.m_UpdatedLanesQuery;
      if (entityQuery.IsEmptyIgnoreFilter)
        return;
      int entityCount = query1.CalculateEntityCount();
      NativeParallelMultiHashMap<PathNode, LaneOverlapSystem.LaneSourceData> parallelMultiHashMap1 = new NativeParallelMultiHashMap<PathNode, LaneOverlapSystem.LaneSourceData>(entityCount, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeParallelMultiHashMap<PathNode, LaneOverlapSystem.LaneTargetData> parallelMultiHashMap2 = new NativeParallelMultiHashMap<PathNode, LaneOverlapSystem.LaneTargetData>(entityCount, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle outJobHandle;
      NativeList<Entity> entityListAsync = entityQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneOverlap_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LaneOverlapSystem.UpdateLaneOverlapsJob jobData1 = new LaneOverlapSystem.UpdateLaneOverlapsJob()
      {
        m_Entities = entityListAsync.AsDeferredJobArray(),
        m_LeftHandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_PedestrianLaneData = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_SecondaryLaneData = this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_PrefabParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_NodeLaneData = this.__TypeHandle.__Game_Net_NodeLane_RW_ComponentLookup,
        m_Overlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LaneOverlapSystem.CollectLaneDirectionsJob jobData2 = new LaneOverlapSystem.CollectLaneDirectionsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_LaneType = this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle,
        m_EdgeLaneType = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_CarLaneType = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentTypeHandle,
        m_TrackLaneType = this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentTypeHandle,
        m_SlaveLaneType = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentTypeHandle,
        m_MasterLaneType = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentTypeHandle,
        m_ConnectionLaneType = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentTypeHandle,
        m_SourceMap = parallelMultiHashMap1.AsParallelWriter(),
        m_TargetMap = parallelMultiHashMap2.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrackLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      LaneOverlapSystem.UpdateLaneFlagsJob jobData3 = new LaneOverlapSystem.UpdateLaneFlagsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_LaneType = this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_MasterLaneType = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentTypeHandle,
        m_LaneOverlapType = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferTypeHandle,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_OutsideConnectionData = this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_EdgeLaneType = this.__TypeHandle.__Game_Net_EdgeLane_RW_ComponentTypeHandle,
        m_NodeLaneType = this.__TypeHandle.__Game_Net_NodeLane_RW_ComponentTypeHandle,
        m_CarLaneType = this.__TypeHandle.__Game_Net_CarLane_RW_ComponentTypeHandle,
        m_TrackLaneType = this.__TypeHandle.__Game_Net_TrackLane_RW_ComponentTypeHandle,
        m_SlaveLaneType = this.__TypeHandle.__Game_Net_SlaveLane_RW_ComponentTypeHandle,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_LaneOverlapData = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_SourceMap = parallelMultiHashMap1,
        m_TargetMap = parallelMultiHashMap2
      };
      JobHandle jobHandle = jobData1.Schedule<LaneOverlapSystem.UpdateLaneOverlapsJob, Entity>(entityListAsync, 1, JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      JobHandle job1 = jobData2.ScheduleParallel<LaneOverlapSystem.CollectLaneDirectionsJob>(query1, this.Dependency);
      EntityQuery query2 = query1;
      JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle, job1);
      JobHandle inputDeps = jobData3.ScheduleParallel<LaneOverlapSystem.UpdateLaneFlagsJob>(query2, dependsOn);
      parallelMultiHashMap1.Dispose(inputDeps);
      parallelMultiHashMap2.Dispose(inputDeps);
      entityListAsync.Dispose(jobHandle);
      this.Dependency = inputDeps;
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
    public LaneOverlapSystem()
    {
    }

    [BurstCompile]
    private struct UpdateLaneFlagsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Lane> m_LaneType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<MasterLane> m_MasterLaneType;
      [ReadOnly]
      public BufferTypeHandle<LaneOverlap> m_LaneOverlapType;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<OutsideConnection> m_OutsideConnectionData;
      [ReadOnly]
      public BufferLookup<SubLane> m_SubLanes;
      public ComponentTypeHandle<EdgeLane> m_EdgeLaneType;
      public ComponentTypeHandle<NodeLane> m_NodeLaneType;
      public ComponentTypeHandle<CarLane> m_CarLaneType;
      public ComponentTypeHandle<TrackLane> m_TrackLaneType;
      public ComponentTypeHandle<SlaveLane> m_SlaveLaneType;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public BufferLookup<LaneOverlap> m_LaneOverlapData;
      [ReadOnly]
      public NativeParallelMultiHashMap<PathNode, LaneOverlapSystem.LaneSourceData> m_SourceMap;
      [ReadOnly]
      public NativeParallelMultiHashMap<PathNode, LaneOverlapSystem.LaneTargetData> m_TargetMap;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Lane> nativeArray2 = chunk.GetNativeArray<Lane>(ref this.m_LaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarLane> nativeArray3 = chunk.GetNativeArray<CarLane>(ref this.m_CarLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TrackLane> nativeArray4 = chunk.GetNativeArray<TrackLane>(ref this.m_TrackLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray5 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<EdgeLane> nativeArray6 = chunk.GetNativeArray<EdgeLane>(ref this.m_EdgeLaneType);
        if (nativeArray6.Length != 0)
        {
          for (int index = 0; index < nativeArray6.Length; ++index)
          {
            Lane lane = nativeArray2[index];
            EdgeLane edgeLane = nativeArray6[index];
            bool carLanes = nativeArray3.Length != 0;
            bool trackLanes = nativeArray4.Length != 0 & !carLanes;
            // ISSUE: reference to a compiler-generated method
            edgeLane.m_ConnectedStartCount = (byte) math.clamp(this.CalculateConnectedSources(lane.m_StartNode, carLanes, trackLanes), 0, (int) byte.MaxValue);
            // ISSUE: reference to a compiler-generated method
            edgeLane.m_ConnectedEndCount = (byte) math.clamp(this.CalculateConnectedTargets(lane.m_EndNode, carLanes, trackLanes), 0, (int) byte.MaxValue);
            Edge componentData1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (edgeLane.m_ConnectedStartCount == (byte) 0 && nativeArray5.Length != 0 && ((double) edgeLane.m_EdgeDelta.x == 0.0 || (double) edgeLane.m_EdgeDelta.x == 1.0) && this.m_EdgeData.TryGetComponent(nativeArray5[index].m_Owner, out componentData1) && this.m_OutsideConnectionData.HasComponent((double) edgeLane.m_EdgeDelta.x == 0.0 ? componentData1.m_Start : componentData1.m_End))
              edgeLane.m_ConnectedStartCount = (byte) 1;
            Edge componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (edgeLane.m_ConnectedEndCount == (byte) 0 && nativeArray5.Length != 0 && ((double) edgeLane.m_EdgeDelta.y == 0.0 || (double) edgeLane.m_EdgeDelta.y == 1.0) && this.m_EdgeData.TryGetComponent(nativeArray5[index].m_Owner, out componentData2) && this.m_OutsideConnectionData.HasComponent((double) edgeLane.m_EdgeDelta.y == 0.0 ? componentData2.m_Start : componentData2.m_End))
              edgeLane.m_ConnectedEndCount = (byte) 1;
            nativeArray6[index] = edgeLane;
          }
          if (nativeArray3.Length != 0)
          {
            NativeParallelHashSet<PathNode> nativeParallelHashSet = new NativeParallelHashSet<PathNode>();
            NativeList<LaneOverlapSystem.LaneTargetData> nativeList = new NativeList<LaneOverlapSystem.LaneTargetData>();
            // ISSUE: reference to a compiler-generated field
            NativeArray<Curve> nativeArray7 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<SlaveLane> nativeArray8 = chunk.GetNativeArray<SlaveLane>(ref this.m_SlaveLaneType);
            // ISSUE: reference to a compiler-generated field
            bool flag1 = chunk.Has<MasterLane>(ref this.m_MasterLaneType);
            for (int index = 0; index < nativeArray2.Length; ++index)
            {
              Lane lane = nativeArray2[index];
              EdgeLane edgeLane = nativeArray6[index];
              Curve curve1 = nativeArray7[index];
              CarLane carLane = nativeArray3[index];
              SlaveLane slaveLane = new SlaveLane();
              if (nativeArray8.Length != 0)
              {
                slaveLane = nativeArray8[index];
                slaveLane.m_Flags &= ~(SlaveLaneFlags.MergingLane | SlaveLaneFlags.SplitLeft | SlaveLaneFlags.SplitRight | SlaveLaneFlags.MergeLeft | SlaveLaneFlags.MergeRight);
              }
              carLane.m_Flags &= CarLaneFlags.Unsafe | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter;
              bool c = (carLane.m_Flags & CarLaneFlags.Highway) > ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter);
              if ((double) carLane.m_Curviness > (double) math.select((float) Math.PI / 180f, (float) Math.PI / 360f, c) || edgeLane.m_ConnectedEndCount == (byte) 0)
                carLane.m_Flags |= CarLaneFlags.ForbidPassing;
              if (edgeLane.m_ConnectedStartCount == (byte) 0 | edgeLane.m_ConnectedEndCount == (byte) 0)
              {
                if (nativeArray5.Length != 0 && edgeLane.m_ConnectedEndCount == (byte) 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  slaveLane.m_Flags |= this.GetMergeLaneFlags(nativeArray5[index].m_Owner, slaveLane, lane, (carLane.m_Flags & CarLaneFlags.Invert) > ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter));
                  if ((slaveLane.m_Flags & (SlaveLaneFlags.MergeLeft | SlaveLaneFlags.MergeRight)) != (SlaveLaneFlags) 0)
                    carLane.m_Flags |= CarLaneFlags.Approach;
                }
                else
                  slaveLane.m_Flags |= SlaveLaneFlags.MergingLane;
              }
              bool flag2 = false;
              // ISSUE: variable of a compiler-generated type
              LaneOverlapSystem.LaneSourceData laneSourceData;
              NativeParallelMultiHashMapIterator<PathNode> it1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SourceMap.TryGetFirstValue(lane.m_StartNode, out laneSourceData, out it1))
              {
                // ISSUE: reference to a compiler-generated field
                while (laneSourceData.m_IsTrack)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_SourceMap.TryGetNextValue(out laneSourceData, ref it1))
                    goto label_29;
                }
                // ISSUE: reference to a compiler-generated field
                if (!laneSourceData.m_IsEdge)
                {
                  bool flag3 = false;
                  bool flag4 = false;
                  // ISSUE: reference to a compiler-generated field
                  do
                  {
                    // ISSUE: reference to a compiler-generated field
                    flag3 |= (laneSourceData.m_SlaveFlags & SlaveLaneFlags.OpenEndLeft) != 0;
                    // ISSUE: reference to a compiler-generated field
                    flag4 |= (laneSourceData.m_SlaveFlags & SlaveLaneFlags.OpenEndRight) != 0;
                  }
                  while (this.m_SourceMap.TryGetNextValue(out laneSourceData, ref it1));
                  if (!flag3)
                    slaveLane.m_Flags |= SlaveLaneFlags.SplitLeft;
                  if (!flag4)
                    slaveLane.m_Flags |= SlaveLaneFlags.SplitRight;
                }
              }
label_29:
              // ISSUE: variable of a compiler-generated type
              LaneOverlapSystem.LaneTargetData laneTargetData1;
              NativeParallelMultiHashMapIterator<PathNode> it2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_TargetMap.TryGetFirstValue(lane.m_EndNode, out laneTargetData1, out it2))
              {
                // ISSUE: reference to a compiler-generated field
                while (laneTargetData1.m_IsTrack)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_TargetMap.TryGetNextValue(out laneTargetData1, ref it2))
                    goto label_66;
                }
                // ISSUE: reference to a compiler-generated field
                bool flag5 = !laneTargetData1.m_IsEdge;
                // ISSUE: reference to a compiler-generated field
                if (laneTargetData1.m_IsEdge)
                {
                  if (!c)
                  {
                    // ISSUE: reference to a compiler-generated field
                    laneTargetData1.m_CarFlags &= CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter;
                  }
                  // ISSUE: reference to a compiler-generated field
                  carLane.m_Flags |= laneTargetData1.m_CarFlags;
                  bool flag6 = false;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_TargetMap.TryGetFirstValue(laneTargetData1.m_EndNode, out laneTargetData1, out it2))
                  {
                    // ISSUE: reference to a compiler-generated field
                    while (laneTargetData1.m_IsTrack)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (!this.m_TargetMap.TryGetNextValue(out laneTargetData1, ref it2))
                        goto label_38;
                    }
                    flag6 = true;
                  }
label_38:
                  if (!flag6 & c)
                    carLane.m_Flags |= CarLaneFlags.ForbidPassing;
                }
                // ISSUE: reference to a compiler-generated field
                if (!laneTargetData1.m_IsEdge)
                {
                  bool flag7 = false;
                  bool flag8 = false;
                  // ISSUE: reference to a compiler-generated field
                  do
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (!laneTargetData1.m_IsTrack)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if ((laneTargetData1.m_CarFlags & CarLaneFlags.Roundabout) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                      {
                        if (!flag2)
                        {
                          if (nativeParallelHashSet.IsCreated)
                          {
                            nativeParallelHashSet.Clear();
                            nativeList.Clear();
                          }
                          else
                          {
                            nativeParallelHashSet = new NativeParallelHashSet<PathNode>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                            nativeList = new NativeList<LaneOverlapSystem.LaneTargetData>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                          }
                          nativeParallelHashSet.Add(lane.m_EndNode);
                          flag2 = true;
                        }
                        nativeList.Add(in laneTargetData1);
                        // ISSUE: reference to a compiler-generated field
                        CarLaneFlags carLaneFlags = laneTargetData1.m_CarFlags & (CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.RightOfWay);
                        if (carLaneFlags != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                          carLane.m_Flags |= carLaneFlags | CarLaneFlags.Approach;
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        CarLaneFlags carLaneFlags = laneTargetData1.m_CarFlags & (CarLaneFlags.UTurnLeft | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights);
                        // ISSUE: reference to a compiler-generated field
                        if (!flag1 && laneTargetData1.m_IsMaster)
                          carLaneFlags &= CarLaneFlags.Unsafe | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter;
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        if (!flag1 && (carLaneFlags & CarLaneFlags.Yield) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) && (laneTargetData1.m_IsMaster || !this.HasRoadOverlaps(laneTargetData1.m_Entity)))
                          carLaneFlags &= CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter;
                        if ((carLaneFlags & (CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter)) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                          carLane.m_Flags |= CarLaneFlags.Approach;
                        carLane.m_Flags |= carLaneFlags;
                      }
                      // ISSUE: reference to a compiler-generated field
                      flag7 |= (laneTargetData1.m_SlaveFlags & SlaveLaneFlags.OpenStartLeft) != 0;
                      // ISSUE: reference to a compiler-generated field
                      flag8 |= (laneTargetData1.m_SlaveFlags & SlaveLaneFlags.OpenStartRight) != 0;
                    }
                  }
                  while (this.m_TargetMap.TryGetNextValue(out laneTargetData1, ref it2));
                  if (flag5)
                  {
                    if (!flag7)
                      slaveLane.m_Flags |= SlaveLaneFlags.SplitLeft;
                    if (!flag8)
                      slaveLane.m_Flags |= SlaveLaneFlags.SplitRight;
                  }
                }
              }
label_66:
              if (flag2)
              {
                float3 float3 = MathUtils.EndTangent(curve1.m_Bezier);
                float2 startDirection = math.normalizesafe(float3.xz);
                CarLaneFlags carLaneFlags = CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.ForbidPassing;
                int num = 0;
                while (num < nativeList.Length)
                {
                  // ISSUE: variable of a compiler-generated type
                  LaneOverlapSystem.LaneTargetData laneTargetData2 = nativeList[num++];
                  // ISSUE: reference to a compiler-generated field
                  if (nativeParallelHashSet.Add(laneTargetData2.m_EndNode))
                  {
                    bool flag9 = false;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_TargetMap.TryGetFirstValue(laneTargetData2.m_EndNode, out laneTargetData1, out it2))
                    {
                      // ISSUE: reference to a compiler-generated field
                      do
                      {
                        // ISSUE: reference to a compiler-generated field
                        if ((laneTargetData1.m_CarFlags & CarLaneFlags.Roundabout) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          laneTargetData1.m_TurnAmount += laneTargetData2.m_TurnAmount;
                          // ISSUE: reference to a compiler-generated field
                          if ((double) math.abs(laneTargetData2.m_TurnAmount) < 6.2831854820251465)
                            nativeList.Add(in laneTargetData1);
                          flag9 = true;
                        }
                      }
                      while (this.m_TargetMap.TryGetNextValue(out laneTargetData1, ref it2));
                    }
                    if (!flag9)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      Curve curve2 = this.m_CurveData[laneTargetData2.m_Entity];
                      float2 endDirection = -startDirection;
                      if ((double) curve2.m_Length > 0.10000000149011612)
                      {
                        float3 = MathUtils.EndTangent(curve2.m_Bezier);
                        endDirection = math.normalizesafe(-float3.xz);
                      }
                      bool right;
                      bool gentle;
                      bool uturn;
                      if (NetUtils.IsTurn(curve1.m_Bezier.d.xz, startDirection, curve2.m_Bezier.d.xz, endDirection, out right, out gentle, out uturn))
                      {
                        // ISSUE: reference to a compiler-generated field
                        if ((double) laneTargetData2.m_TurnAmount > 0.0 == right)
                        {
                          if (gentle)
                            carLaneFlags |= right ? CarLaneFlags.GentleTurnRight : CarLaneFlags.GentleTurnLeft;
                          else if (uturn)
                            carLaneFlags |= right ? CarLaneFlags.UTurnRight : CarLaneFlags.UTurnLeft;
                          else
                            carLaneFlags |= right ? CarLaneFlags.TurnRight : CarLaneFlags.TurnLeft;
                        }
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        if ((double) math.abs(laneTargetData2.m_TurnAmount) < 1.5707963705062866)
                          carLaneFlags |= CarLaneFlags.Forward;
                      }
                    }
                  }
                }
                if ((carLaneFlags & (CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward)) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                  carLaneFlags &= CarLaneFlags.Unsafe | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter;
                carLane.m_Flags |= carLaneFlags;
              }
              nativeArray3[index] = carLane;
              if (nativeArray8.Length != 0)
                nativeArray8[index] = slaveLane;
            }
            if (nativeParallelHashSet.IsCreated)
              nativeParallelHashSet.Dispose();
            if (nativeList.IsCreated)
              nativeList.Dispose();
          }
        }
        else if (nativeArray3.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<NodeLane> nativeArray9 = chunk.GetNativeArray<NodeLane>(ref this.m_NodeLaneType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<MasterLane> nativeArray10 = chunk.GetNativeArray<MasterLane>(ref this.m_MasterLaneType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<LaneOverlap> bufferAccessor = chunk.GetBufferAccessor<LaneOverlap>(ref this.m_LaneOverlapType);
          if (nativeArray9.Length != 0)
          {
            for (int index = 0; index < nativeArray3.Length; ++index)
            {
              if ((nativeArray3[index].m_Flags & CarLaneFlags.Roundabout) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
              {
                Lane lane = nativeArray2[index];
                NodeLane nodeLane = nativeArray9[index];
                // ISSUE: reference to a compiler-generated method
                if (lane.m_EndNode.OwnerEquals(lane.m_MiddleNode) && this.CalculateConnectedTargets(lane.m_EndNode, true, false) == 0)
                  nodeLane.m_SharedEndCount = byte.MaxValue;
                // ISSUE: reference to a compiler-generated method
                if (lane.m_StartNode.OwnerEquals(lane.m_MiddleNode) && this.CalculateConnectedSources(lane.m_StartNode, true, false) == 0)
                  nodeLane.m_SharedStartCount = byte.MaxValue;
                nativeArray9[index] = nodeLane;
              }
            }
          }
          for (int index1 = 0; index1 < nativeArray3.Length; ++index1)
          {
            CarLane carLane = nativeArray3[index1] with
            {
              m_LaneCrossCount = 0
            };
            if (nativeArray10.Length != 0)
            {
              MasterLane masterLane = nativeArray10[index1];
              Owner owner = nativeArray5[index1];
              int num = 256;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SubLanes.HasBuffer(owner.m_Owner))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<SubLane> subLane = this.m_SubLanes[owner.m_Owner];
                for (int minIndex = (int) masterLane.m_MinIndex; minIndex <= (int) masterLane.m_MaxIndex; ++minIndex)
                {
                  DynamicBuffer<LaneOverlap> bufferData;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_LaneOverlapData.TryGetBuffer(subLane[minIndex].m_SubLane, out bufferData))
                  {
                    int y = 0;
                    for (int index2 = 0; index2 < bufferData.Length; ++index2)
                    {
                      LaneOverlap laneOverlap = bufferData[index2];
                      y += math.select(0, 1, (laneOverlap.m_Flags & (OverlapFlags.MergeStart | OverlapFlags.MergeEnd | OverlapFlags.MergeMiddleStart | OverlapFlags.MergeMiddleEnd | OverlapFlags.Unsafe | OverlapFlags.Road)) == OverlapFlags.Road | (laneOverlap.m_Flags & (OverlapFlags.Road | OverlapFlags.MergeFlip)) == (OverlapFlags.Road | OverlapFlags.MergeFlip));
                    }
                    num = math.min(math.min((int) byte.MaxValue, y), num);
                  }
                }
              }
              carLane.m_LaneCrossCount = (byte) math.select(num, 0, num == 256);
            }
            else if (bufferAccessor.Length != 0)
            {
              DynamicBuffer<LaneOverlap> dynamicBuffer = bufferAccessor[index1];
              int y = 0;
              for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
              {
                LaneOverlap laneOverlap = dynamicBuffer[index3];
                y += math.select(0, 1, (laneOverlap.m_Flags & (OverlapFlags.MergeStart | OverlapFlags.MergeEnd | OverlapFlags.MergeMiddleStart | OverlapFlags.MergeMiddleEnd | OverlapFlags.Unsafe | OverlapFlags.Road)) == OverlapFlags.Road | (laneOverlap.m_Flags & (OverlapFlags.Road | OverlapFlags.MergeFlip)) == (OverlapFlags.Road | OverlapFlags.MergeFlip));
              }
              carLane.m_LaneCrossCount = (byte) math.min((int) byte.MaxValue, y);
            }
            nativeArray3[index1] = carLane;
          }
        }
        if (nativeArray4.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LaneOverlap> bufferAccessor1 = chunk.GetBufferAccessor<LaneOverlap>(ref this.m_LaneOverlapType);
        for (int index4 = 0; index4 < nativeArray4.Length; ++index4)
        {
          Entity entity = nativeArray1[index4];
          TrackLane trackLane = nativeArray4[index4];
          Lane lane = nativeArray2[index4];
          trackLane.m_Flags &= ~(TrackLaneFlags.Switch | TrackLaneFlags.DiamondCrossing | TrackLaneFlags.CrossingTraffic | TrackLaneFlags.MergingTraffic | TrackLaneFlags.DoubleSwitch);
          trackLane.m_Flags |= TrackLaneFlags.StartingLane | TrackLaneFlags.EndingLane;
          if (bufferAccessor1.Length != 0)
          {
            DynamicBuffer<LaneOverlap> dynamicBuffer = bufferAccessor1[index4];
            OverlapFlags overlapFlags1 = (OverlapFlags) 0;
            for (int index5 = 0; index5 < dynamicBuffer.Length; ++index5)
            {
              LaneOverlap laneOverlap = dynamicBuffer[index5];
              if ((laneOverlap.m_Flags & OverlapFlags.Track) != (OverlapFlags) 0)
              {
                OverlapFlags overlapFlags2 = laneOverlap.m_Flags & (OverlapFlags.MergeStart | OverlapFlags.MergeEnd);
                overlapFlags1 |= overlapFlags2;
                if (overlapFlags2 == (OverlapFlags) 0)
                  trackLane.m_Flags |= TrackLaneFlags.DiamondCrossing;
                else if (overlapFlags1 == (OverlapFlags.MergeStart | OverlapFlags.MergeEnd))
                  trackLane.m_Flags |= TrackLaneFlags.Switch | TrackLaneFlags.DoubleSwitch;
                else
                  trackLane.m_Flags |= TrackLaneFlags.Switch;
              }
              else if (nativeArray6.Length == 0)
              {
                if ((laneOverlap.m_Flags & OverlapFlags.MergeEnd) != (OverlapFlags) 0)
                  trackLane.m_Flags |= TrackLaneFlags.MergingTraffic;
                else if ((laneOverlap.m_Flags & OverlapFlags.MergeStart) == (OverlapFlags) 0)
                  trackLane.m_Flags |= TrackLaneFlags.CrossingTraffic;
              }
            }
          }
          // ISSUE: variable of a compiler-generated type
          LaneOverlapSystem.LaneSourceData laneSourceData1;
          NativeParallelMultiHashMapIterator<PathNode> it3;
          // ISSUE: reference to a compiler-generated field
          if ((trackLane.m_Flags & TrackLaneFlags.StartingLane) != (TrackLaneFlags) 0 && this.m_SourceMap.TryGetFirstValue(lane.m_StartNode, out laneSourceData1, out it3))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            while (!(laneSourceData1.m_Entity != entity) || !laneSourceData1.m_IsTrack)
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_SourceMap.TryGetNextValue(out laneSourceData1, ref it3))
                goto label_150;
            }
            trackLane.m_Flags &= ~TrackLaneFlags.StartingLane;
          }
label_150:
          // ISSUE: variable of a compiler-generated type
          LaneOverlapSystem.LaneTargetData laneTargetData3;
          NativeParallelMultiHashMapIterator<PathNode> it4;
          // ISSUE: reference to a compiler-generated field
          if ((trackLane.m_Flags & TrackLaneFlags.StartingLane) != (TrackLaneFlags) 0 && this.m_TargetMap.TryGetFirstValue(lane.m_StartNode, out laneTargetData3, out it4))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            while (!(laneTargetData3.m_Entity != entity) || !laneTargetData3.m_IsTrack)
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_TargetMap.TryGetNextValue(out laneTargetData3, ref it4))
                goto label_154;
            }
            trackLane.m_Flags &= ~TrackLaneFlags.StartingLane;
          }
label_154:
          // ISSUE: variable of a compiler-generated type
          LaneOverlapSystem.LaneSourceData laneSourceData2;
          NativeParallelMultiHashMapIterator<PathNode> it5;
          // ISSUE: reference to a compiler-generated field
          if ((trackLane.m_Flags & TrackLaneFlags.EndingLane) != (TrackLaneFlags) 0 && this.m_SourceMap.TryGetFirstValue(lane.m_EndNode, out laneSourceData2, out it5))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            while (!(laneSourceData2.m_Entity != entity) || !laneSourceData2.m_IsTrack)
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_SourceMap.TryGetNextValue(out laneSourceData2, ref it5))
                goto label_158;
            }
            trackLane.m_Flags &= ~TrackLaneFlags.EndingLane;
          }
label_158:
          // ISSUE: variable of a compiler-generated type
          LaneOverlapSystem.LaneTargetData laneTargetData4;
          NativeParallelMultiHashMapIterator<PathNode> it6;
          // ISSUE: reference to a compiler-generated field
          if ((trackLane.m_Flags & TrackLaneFlags.EndingLane) != (TrackLaneFlags) 0 && this.m_TargetMap.TryGetFirstValue(lane.m_EndNode, out laneTargetData4, out it6))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            while (!(laneTargetData4.m_Entity != entity) || !laneTargetData4.m_IsTrack)
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_TargetMap.TryGetNextValue(out laneTargetData4, ref it6))
                goto label_162;
            }
            trackLane.m_Flags &= ~TrackLaneFlags.EndingLane;
          }
label_162:
          if (nativeArray6.Length != 0 && nativeArray5.Length != 0 && (trackLane.m_Flags & (TrackLaneFlags.StartingLane | TrackLaneFlags.EndingLane)) != (TrackLaneFlags) 0)
          {
            EdgeLane edgeLane = nativeArray6[index4];
            Edge componentData3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((trackLane.m_Flags & TrackLaneFlags.StartingLane) != (TrackLaneFlags) 0 && ((double) edgeLane.m_EdgeDelta.x == 0.0 || (double) edgeLane.m_EdgeDelta.x == 1.0) && this.m_EdgeData.TryGetComponent(nativeArray5[index4].m_Owner, out componentData3) && this.m_OutsideConnectionData.HasComponent((double) edgeLane.m_EdgeDelta.x == 0.0 ? componentData3.m_Start : componentData3.m_End))
              trackLane.m_Flags &= ~TrackLaneFlags.StartingLane;
            Edge componentData4;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((trackLane.m_Flags & TrackLaneFlags.EndingLane) != (TrackLaneFlags) 0 && ((double) edgeLane.m_EdgeDelta.y == 0.0 || (double) edgeLane.m_EdgeDelta.y == 1.0) && this.m_EdgeData.TryGetComponent(nativeArray5[index4].m_Owner, out componentData4) && this.m_OutsideConnectionData.HasComponent((double) edgeLane.m_EdgeDelta.y == 0.0 ? componentData4.m_Start : componentData4.m_End))
              trackLane.m_Flags &= ~TrackLaneFlags.EndingLane;
          }
          nativeArray4[index4] = trackLane;
        }
      }

      private SlaveLaneFlags GetMergeLaneFlags(
        Entity owner,
        SlaveLane slaveLane,
        Lane lane,
        bool invert)
      {
        bool flag1 = false;
        bool flag2 = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubLanes.HasBuffer(owner))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SubLane> subLane = this.m_SubLanes[owner];
          for (int minIndex = (int) slaveLane.m_MinIndex; minIndex <= (int) slaveLane.m_MaxIndex; ++minIndex)
          {
            // ISSUE: reference to a compiler-generated field
            Lane other = this.m_LaneData[subLane[minIndex].m_SubLane];
            if (lane.Equals(other))
            {
              if (flag1)
                return !invert ? SlaveLaneFlags.MergingLane | SlaveLaneFlags.MergeLeft : SlaveLaneFlags.MergingLane | SlaveLaneFlags.MergeRight;
              flag2 = true;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_TargetMap.ContainsKey(other.m_EndNode))
            {
              if (flag2)
                return !invert ? SlaveLaneFlags.MergingLane | SlaveLaneFlags.MergeRight : SlaveLaneFlags.MergingLane | SlaveLaneFlags.MergeLeft;
              flag1 = true;
            }
          }
        }
        return SlaveLaneFlags.MergingLane;
      }

      private bool HasRoadOverlaps(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_LaneOverlapData.HasBuffer(entity))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<LaneOverlap> dynamicBuffer = this.m_LaneOverlapData[entity];
          for (int index = 0; index < dynamicBuffer.Length; ++index)
          {
            if ((dynamicBuffer[index].m_Flags & (OverlapFlags.Unsafe | OverlapFlags.Road)) == OverlapFlags.Road)
              return true;
          }
        }
        return false;
      }

      private int CalculateConnectedSources(PathNode node, bool carLanes, bool trackLanes)
      {
        int connectedSources = 0;
        // ISSUE: variable of a compiler-generated type
        LaneOverlapSystem.LaneSourceData laneSourceData;
        NativeParallelMultiHashMapIterator<PathNode> it;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SourceMap.TryGetFirstValue(node, out laneSourceData, out it))
        {
          // ISSUE: reference to a compiler-generated field
          do
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (laneSourceData.m_IsRoad & carLanes | laneSourceData.m_IsTrack & trackLanes)
              ++connectedSources;
          }
          while (this.m_SourceMap.TryGetNextValue(out laneSourceData, ref it));
        }
        return connectedSources;
      }

      private int CalculateConnectedTargets(PathNode node, bool carLanes, bool trackLanes)
      {
        int connectedTargets = 0;
        // ISSUE: variable of a compiler-generated type
        LaneOverlapSystem.LaneTargetData laneTargetData;
        NativeParallelMultiHashMapIterator<PathNode> it;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TargetMap.TryGetFirstValue(node, out laneTargetData, out it))
        {
          // ISSUE: reference to a compiler-generated field
          do
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (laneTargetData.m_IsRoad & carLanes | laneTargetData.m_IsTrack & trackLanes)
              ++connectedTargets;
          }
          while (this.m_TargetMap.TryGetNextValue(out laneTargetData, ref it));
        }
        return connectedTargets;
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

    private struct LaneSourceData
    {
      public Entity m_Entity;
      public PathNode m_StartNode;
      public SlaveLaneFlags m_SlaveFlags;
      public bool m_IsEdge;
      public bool m_IsRoad;
      public bool m_IsTrack;
    }

    private struct LaneTargetData
    {
      public Entity m_Entity;
      public PathNode m_EndNode;
      public CarLaneFlags m_CarFlags;
      public SlaveLaneFlags m_SlaveFlags;
      public float m_TurnAmount;
      public bool m_IsEdge;
      public bool m_IsMaster;
      public bool m_IsRoad;
      public bool m_IsTrack;
    }

    [BurstCompile]
    private struct CollectLaneDirectionsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Lane> m_LaneType;
      [ReadOnly]
      public ComponentTypeHandle<EdgeLane> m_EdgeLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<CarLane> m_CarLaneType;
      [ReadOnly]
      public ComponentTypeHandle<TrackLane> m_TrackLaneType;
      [ReadOnly]
      public ComponentTypeHandle<SlaveLane> m_SlaveLaneType;
      [ReadOnly]
      public ComponentTypeHandle<MasterLane> m_MasterLaneType;
      [ReadOnly]
      public ComponentTypeHandle<ConnectionLane> m_ConnectionLaneType;
      public NativeParallelMultiHashMap<PathNode, LaneOverlapSystem.LaneSourceData>.ParallelWriter m_SourceMap;
      public NativeParallelMultiHashMap<PathNode, LaneOverlapSystem.LaneTargetData>.ParallelWriter m_TargetMap;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Lane> nativeArray2 = chunk.GetNativeArray<Lane>(ref this.m_LaneType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<EdgeLane>(ref this.m_EdgeLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarLane> nativeArray3 = chunk.GetNativeArray<CarLane>(ref this.m_CarLaneType);
        if (nativeArray3.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Curve> nativeArray4 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<SlaveLane> nativeArray5 = chunk.GetNativeArray<SlaveLane>(ref this.m_SlaveLaneType);
          // ISSUE: reference to a compiler-generated field
          bool flag2 = chunk.Has<MasterLane>(ref this.m_MasterLaneType);
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Lane lane = nativeArray2[index];
            Curve curve = nativeArray4[index];
            CarLane carLane = nativeArray3[index];
            if ((carLane.m_Flags & CarLaneFlags.Unsafe) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
            {
              SlaveLaneFlags slaveLaneFlags1 = (SlaveLaneFlags) 0;
              SlaveLaneFlags slaveLaneFlags2 = (SlaveLaneFlags) 0;
              CarLaneFlags carLaneFlags = ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter);
              if (flag1)
              {
                if ((carLane.m_Flags & CarLaneFlags.Highway) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) && (double) carLane.m_Curviness > Math.PI / 360.0)
                  carLaneFlags |= CarLaneFlags.ForbidPassing;
              }
              else
              {
                if (nativeArray5.Length != 0)
                {
                  SlaveLane slaveLane = nativeArray5[index];
                  slaveLaneFlags1 = slaveLane.m_Flags & (SlaveLaneFlags.OpenEndLeft | SlaveLaneFlags.OpenEndRight);
                  slaveLaneFlags2 = slaveLane.m_Flags & (SlaveLaneFlags.OpenStartLeft | SlaveLaneFlags.OpenStartRight);
                }
                carLaneFlags = carLane.m_Flags & (CarLaneFlags.UTurnLeft | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Roundabout | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_SourceMap.Add(lane.m_EndNode, new LaneOverlapSystem.LaneSourceData()
              {
                m_Entity = entity,
                m_StartNode = lane.m_StartNode,
                m_SlaveFlags = slaveLaneFlags1,
                m_IsEdge = flag1,
                m_IsRoad = true
              });
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: object of a compiler-generated type is created
              this.m_TargetMap.Add(lane.m_StartNode, new LaneOverlapSystem.LaneTargetData()
              {
                m_Entity = entity,
                m_EndNode = lane.m_EndNode,
                m_SlaveFlags = slaveLaneFlags2,
                m_CarFlags = carLaneFlags,
                m_TurnAmount = this.CalculateTurnAmount(curve),
                m_IsEdge = flag1,
                m_IsMaster = flag2,
                m_IsRoad = true
              });
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<ConnectionLane>(ref this.m_ConnectionLaneType))
            return;
          // ISSUE: reference to a compiler-generated field
          bool flag3 = chunk.Has<TrackLane>(ref this.m_TrackLaneType);
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Lane lane = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_SourceMap.Add(lane.m_EndNode, new LaneOverlapSystem.LaneSourceData()
            {
              m_Entity = entity,
              m_StartNode = lane.m_StartNode,
              m_IsEdge = flag1,
              m_IsTrack = flag3
            });
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_TargetMap.Add(lane.m_StartNode, new LaneOverlapSystem.LaneTargetData()
            {
              m_Entity = entity,
              m_EndNode = lane.m_EndNode,
              m_IsEdge = flag1,
              m_IsTrack = flag3
            });
          }
        }
      }

      private float CalculateTurnAmount(Curve curve)
      {
        float2 float2 = math.normalizesafe(MathUtils.StartTangent(curve.m_Bezier).xz);
        float2 y = math.normalizesafe(MathUtils.EndTangent(curve.m_Bezier).xz);
        double a = (double) math.acos(math.clamp(math.dot(float2, y), -1f, 1f));
        return math.select((float) a, (float) -a, (double) math.dot(MathUtils.Right(float2), y) < 0.0);
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
    private struct UpdateLaneOverlapsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Entity> m_Entities;
      [ReadOnly]
      public bool m_LeftHandTraffic;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<PedestrianLane> m_PedestrianLaneData;
      [ReadOnly]
      public ComponentLookup<ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<SecondaryLane> m_SecondaryLaneData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabLaneData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
      [ReadOnly]
      public BufferLookup<SubLane> m_SubLanes;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<NodeLane> m_NodeLaneData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<LaneOverlap> m_Overlaps;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_Entities[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateOverlaps(this.m_SubLanes[this.m_Entities[index]], this.m_EdgeData.HasComponent(entity));
      }

      private void UpdateOverlaps(DynamicBuffer<SubLane> lanes, bool isEdge)
      {
        for (int index1 = 0; index1 < lanes.Length; ++index1)
        {
          Entity subLane1 = lanes[index1].m_SubLane;
          DynamicBuffer<LaneOverlap> bufferData1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Overlaps.TryGetBuffer(subLane1, out bufferData1) && !this.m_SecondaryLaneData.HasComponent(subLane1))
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef1 = this.m_PrefabRefData[subLane1];
            // ISSUE: reference to a compiler-generated field
            Curve curve1 = this.m_CurveData[subLane1];
            // ISSUE: reference to a compiler-generated field
            Lane lane1 = this.m_LaneData[subLane1];
            // ISSUE: reference to a compiler-generated field
            NetLaneData netLaneData1 = this.m_PrefabLaneData[prefabRef1.m_Prefab];
            if ((netLaneData1.m_Flags & (LaneFlags.Parking | LaneFlags.Virtual)) != (LaneFlags.Parking | LaneFlags.Virtual))
            {
              CarLane carLane1 = new CarLane();
              if ((netLaneData1.m_Flags & LaneFlags.Road) != (LaneFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                carLane1 = this.m_CarLaneData[subLane1];
              }
              PedestrianLane pedestrianLane1 = new PedestrianLane();
              if ((netLaneData1.m_Flags & LaneFlags.Pedestrian) != (LaneFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                pedestrianLane1 = this.m_PedestrianLaneData[subLane1];
              }
              float angle1 = 0.0f;
              ParkingLane componentData1;
              ParkingLaneData componentData2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((netLaneData1.m_Flags & LaneFlags.Parking) != (LaneFlags) 0 && this.m_ParkingLaneData.TryGetComponent(subLane1, out componentData1) && this.m_PrefabParkingLaneData.TryGetComponent(prefabRef1.m_Prefab, out componentData2) && (double) componentData2.m_SlotAngle > 0.25)
                angle1 = math.select(1.57079637f - componentData2.m_SlotAngle, componentData2.m_SlotAngle - 1.57079637f, (componentData1.m_Flags & ParkingLaneFlags.ParkingLeft) != 0);
              NodeLane componentData3;
              // ISSUE: reference to a compiler-generated field
              bool component1 = this.m_NodeLaneData.TryGetComponent(subLane1, out componentData3);
              bufferData1.Clear();
              componentData3.m_SharedStartCount = (byte) 0;
              componentData3.m_SharedEndCount = (byte) 0;
              for (int index2 = 0; index2 < index1; ++index2)
              {
                Entity subLane2 = lanes[index2].m_SubLane;
                DynamicBuffer<LaneOverlap> bufferData2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Overlaps.TryGetBuffer(subLane2, out bufferData2) && !this.m_SecondaryLaneData.HasComponent(subLane2))
                {
                  // ISSUE: reference to a compiler-generated field
                  Curve curve2 = this.m_CurveData[subLane2];
                  // ISSUE: reference to a compiler-generated field
                  Lane lane2 = this.m_LaneData[subLane2];
                  float3 float3;
                  if (lane1.m_StartNode.Equals(lane2.m_EndNode))
                  {
                    float3 = MathUtils.StartTangent(curve1.m_Bezier);
                    float2 x = math.normalizesafe(float3.xz);
                    float3 = MathUtils.EndTangent(curve2.m_Bezier);
                    float2 y = math.normalizesafe(float3.xz);
                    if ((double) math.dot(x, y) >= 0.0)
                      continue;
                  }
                  if (lane1.m_EndNode.Equals(lane2.m_StartNode))
                  {
                    float3 = MathUtils.EndTangent(curve1.m_Bezier);
                    float2 x = math.normalizesafe(float3.xz);
                    float3 = MathUtils.StartTangent(curve2.m_Bezier);
                    float2 y = math.normalizesafe(float3.xz);
                    if ((double) math.dot(x, y) >= 0.0)
                      continue;
                  }
                  // ISSUE: reference to a compiler-generated field
                  PrefabRef prefabRef2 = this.m_PrefabRefData[subLane2];
                  // ISSUE: reference to a compiler-generated field
                  NetLaneData netLaneData2 = this.m_PrefabLaneData[prefabRef2.m_Prefab];
                  if ((netLaneData2.m_Flags & LaneFlags.Parking) != (LaneFlags) 0)
                  {
                    if ((netLaneData1.m_Flags & LaneFlags.Road) == (LaneFlags) 0 || !component1 || (netLaneData2.m_Flags & LaneFlags.Virtual) != (LaneFlags) 0)
                      continue;
                  }
                  else if ((netLaneData1.m_Flags & LaneFlags.Parking) != (LaneFlags) 0 && (netLaneData2.m_Flags & LaneFlags.Road) == (LaneFlags) 0)
                    continue;
                  CarLane carLane2 = new CarLane();
                  if ((netLaneData2.m_Flags & LaneFlags.Road) != (LaneFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    carLane2 = this.m_CarLaneData[subLane2];
                  }
                  PedestrianLane pedestrianLane2 = new PedestrianLane();
                  if ((netLaneData2.m_Flags & LaneFlags.Pedestrian) != (LaneFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    pedestrianLane2 = this.m_PedestrianLaneData[subLane2];
                  }
                  float angle2 = 0.0f;
                  ParkingLane componentData4;
                  ParkingLaneData componentData5;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((netLaneData2.m_Flags & LaneFlags.Parking) != (LaneFlags) 0 && this.m_ParkingLaneData.TryGetComponent(subLane2, out componentData4) && this.m_PrefabParkingLaneData.TryGetComponent(prefabRef2.m_Prefab, out componentData5) && (double) componentData5.m_SlotAngle > 0.25)
                    angle2 = math.select(1.57079637f - componentData5.m_SlotAngle, componentData5.m_SlotAngle - 1.57079637f, (componentData4.m_Flags & ParkingLaneFlags.ParkingLeft) != 0);
                  NodeLane componentData6;
                  // ISSUE: reference to a compiler-generated field
                  bool component2 = this.m_NodeLaneData.TryGetComponent(subLane2, out componentData6);
                  if (component2 || (netLaneData1.m_Flags & LaneFlags.Parking) == (LaneFlags) 0 && (!isEdge || component1))
                  {
                    Bezier4x2 xz1 = curve1.m_Bezier.xz;
                    Bezier4x2 xz2 = curve2.m_Bezier.xz;
                    float2 radius1 = (netLaneData1.m_Width + componentData3.m_WidthOffset) * 0.4f;
                    float2 radius2 = (netLaneData2.m_Width + componentData6.m_WidthOffset) * 0.4f;
                    float4 t;
                    // ISSUE: reference to a compiler-generated method
                    if (LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapRange(xz1, xz2, radius1, radius2, angle1, angle2, out t))
                    {
                      // ISSUE: reference to a compiler-generated method
                      float parallelism = LaneOverlapSystem.UpdateLaneOverlapsJob.CalculateParallelism(xz1, xz2, t);
                      OverlapFlags flags1 = (OverlapFlags) 0;
                      OverlapFlags flags2 = (OverlapFlags) 0;
                      if ((netLaneData1.m_Flags & LaneFlags.Road) != (LaneFlags) 0)
                        flags2 |= OverlapFlags.Road;
                      if ((netLaneData1.m_Flags & LaneFlags.Track) != (LaneFlags) 0)
                        flags2 |= OverlapFlags.Track;
                      if ((netLaneData2.m_Flags & LaneFlags.Road) != (LaneFlags) 0)
                        flags1 |= OverlapFlags.Road;
                      if ((netLaneData2.m_Flags & LaneFlags.Track) != (LaneFlags) 0)
                        flags1 |= OverlapFlags.Track;
                      if (lane1.m_StartNode.Equals(lane2.m_StartNode))
                      {
                        flags1 |= OverlapFlags.MergeStart;
                        flags2 |= OverlapFlags.MergeStart;
                      }
                      else if (lane1.m_StartNode.EqualsIgnoreCurvePos(lane2.m_MiddleNode))
                      {
                        flags1 |= OverlapFlags.MergeStart;
                        flags2 |= OverlapFlags.MergeMiddleStart;
                      }
                      else if (lane2.m_StartNode.EqualsIgnoreCurvePos(lane1.m_MiddleNode))
                      {
                        flags1 |= OverlapFlags.MergeMiddleStart;
                        flags2 |= OverlapFlags.MergeStart;
                      }
                      else if (lane1.m_StartNode.Equals(lane2.m_EndNode))
                      {
                        flags1 |= OverlapFlags.MergeStart | OverlapFlags.MergeFlip;
                        flags2 |= OverlapFlags.MergeEnd | OverlapFlags.MergeFlip;
                      }
                      if (lane1.m_EndNode.Equals(lane2.m_EndNode))
                      {
                        flags1 |= OverlapFlags.MergeEnd;
                        flags2 |= OverlapFlags.MergeEnd;
                      }
                      else if (lane1.m_EndNode.EqualsIgnoreCurvePos(lane2.m_MiddleNode))
                      {
                        flags1 |= OverlapFlags.MergeEnd;
                        flags2 |= OverlapFlags.MergeMiddleEnd;
                      }
                      else if (lane2.m_EndNode.EqualsIgnoreCurvePos(lane1.m_MiddleNode))
                      {
                        flags1 |= OverlapFlags.MergeMiddleEnd;
                        flags2 |= OverlapFlags.MergeEnd;
                      }
                      else if (lane1.m_EndNode.Equals(lane2.m_StartNode))
                      {
                        flags1 |= OverlapFlags.MergeEnd | OverlapFlags.MergeFlip;
                        flags2 |= OverlapFlags.MergeStart | OverlapFlags.MergeFlip;
                      }
                      if ((carLane1.m_Flags & CarLaneFlags.Unsafe) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) || (pedestrianLane1.m_Flags & PedestrianLaneFlags.Unsafe) != (PedestrianLaneFlags) 0)
                        flags2 |= OverlapFlags.Unsafe;
                      if ((carLane2.m_Flags & CarLaneFlags.Unsafe) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) || (pedestrianLane2.m_Flags & PedestrianLaneFlags.Unsafe) != (PedestrianLaneFlags) 0)
                        flags1 |= OverlapFlags.Unsafe;
                      if ((carLane1.m_Flags & CarLaneFlags.Approach) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                        carLane1.m_Flags &= CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter;
                      if ((carLane2.m_Flags & CarLaneFlags.Approach) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                        carLane2.m_Flags &= CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter;
                      LaneFlags laneFlags = netLaneData1.m_Flags ^ netLaneData2.m_Flags;
                      CarLaneFlags carLaneFlags = carLane1.m_Flags ^ carLane2.m_Flags;
                      float2 float2_1;
                      if (((flags1 | flags2) & (OverlapFlags.MergeStart | OverlapFlags.MergeEnd)) == (OverlapFlags) 0)
                      {
                        float2_1 = new float2(curve1.m_Length, curve2.m_Length);
                        float2_1 = math.max((float2) 1f, float2_1 * (t.yw - t.xz));
                        float2_1 *= parallelism / float2_1.yx;
                        flags1 |= OverlapFlags.OverlapLeft | OverlapFlags.OverlapRight;
                        flags2 |= OverlapFlags.OverlapLeft | OverlapFlags.OverlapRight;
                      }
                      else
                      {
                        float2_1 = (float2) parallelism;
                        if ((carLaneFlags & CarLaneFlags.Unsafe) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) && (laneFlags & LaneFlags.Road) == (LaneFlags) 0)
                        {
                          if ((flags1 & OverlapFlags.MergeStart) != (OverlapFlags) 0)
                            componentData3.m_SharedStartCount = (byte) math.min(254, (int) componentData3.m_SharedStartCount + 1);
                          if ((flags2 & OverlapFlags.MergeStart) != (OverlapFlags) 0)
                            componentData6.m_SharedStartCount = (byte) math.min(254, (int) componentData6.m_SharedStartCount + 1);
                          if ((flags1 & OverlapFlags.MergeEnd) != (OverlapFlags) 0)
                            componentData3.m_SharedEndCount = (byte) math.min(254, (int) componentData3.m_SharedEndCount + 1);
                          if ((flags2 & OverlapFlags.MergeEnd) != (OverlapFlags) 0)
                            componentData6.m_SharedEndCount = (byte) math.min(254, (int) componentData6.m_SharedEndCount + 1);
                          if (component2)
                          {
                            // ISSUE: reference to a compiler-generated field
                            this.m_NodeLaneData[subLane2] = componentData6;
                          }
                        }
                        float2 forward;
                        if ((flags1 & (OverlapFlags.MergeStart | OverlapFlags.MergeEnd)) == OverlapFlags.MergeStart)
                        {
                          float3 = MathUtils.Tangent(curve1.m_Bezier, t.y);
                          float2 xz3 = float3.xz;
                          float2 xz4 = curve1.m_Bezier.d.xz;
                          float3 = MathUtils.Position(curve1.m_Bezier, math.lerp(t.x, t.y, 0.5f));
                          float2 xz5 = float3.xz;
                          float2 y = xz4 - xz5;
                          float2 a = xz3;
                          float2 b = y;
                          float3 = MathUtils.StartTangent(curve1.m_Bezier);
                          int num = (double) math.dot(float3.xz, y) >= 0.0 ? 1 : 0;
                          forward = math.select(a, b, num != 0);
                        }
                        else if ((flags1 & (OverlapFlags.MergeStart | OverlapFlags.MergeEnd)) == OverlapFlags.MergeEnd)
                        {
                          float3 = MathUtils.Tangent(curve1.m_Bezier, t.x);
                          float2 xz6 = float3.xz;
                          float3 = MathUtils.Position(curve1.m_Bezier, math.lerp(t.x, t.y, 0.5f));
                          float2 y = float3.xz - curve1.m_Bezier.a.xz;
                          float2 a = xz6;
                          float2 b = y;
                          float3 = MathUtils.EndTangent(curve1.m_Bezier);
                          int num = (double) math.dot(float3.xz, y) >= 0.0 ? 1 : 0;
                          forward = math.select(a, b, num != 0);
                        }
                        else
                          forward = curve1.m_Bezier.d.xz - curve1.m_Bezier.a.xz;
                        float2 y1;
                        if ((flags2 & (OverlapFlags.MergeStart | OverlapFlags.MergeEnd)) == OverlapFlags.MergeStart)
                        {
                          float3 = MathUtils.Tangent(curve2.m_Bezier, t.w);
                          float2 xz7 = float3.xz;
                          float2 xz8 = curve2.m_Bezier.d.xz;
                          float3 = MathUtils.Position(curve2.m_Bezier, math.lerp(t.z, t.w, 0.5f));
                          float2 xz9 = float3.xz;
                          float2 y2 = xz8 - xz9;
                          float2 a = xz7;
                          float2 b = y2;
                          float3 = MathUtils.StartTangent(curve2.m_Bezier);
                          int num = (double) math.dot(float3.xz, y2) >= 0.0 ? 1 : 0;
                          y1 = math.select(a, b, num != 0);
                        }
                        else if ((flags2 & (OverlapFlags.MergeStart | OverlapFlags.MergeEnd)) == OverlapFlags.MergeEnd)
                        {
                          float3 = MathUtils.Tangent(curve2.m_Bezier, t.z);
                          float2 xz10 = float3.xz;
                          float3 = MathUtils.Position(curve2.m_Bezier, math.lerp(t.z, t.w, 0.5f));
                          float2 y3 = float3.xz - curve2.m_Bezier.a.xz;
                          float2 a = xz10;
                          float2 b = y3;
                          float3 = MathUtils.EndTangent(curve2.m_Bezier);
                          int num = (double) math.dot(float3.xz, y3) >= 0.0 ? 1 : 0;
                          y1 = math.select(a, b, num != 0);
                        }
                        else
                          y1 = curve2.m_Bezier.d.xz - curve2.m_Bezier.a.xz;
                        bool flag = (double) math.dot(MathUtils.Right(forward), y1) > 0.0 == ((flags1 & OverlapFlags.MergeFlip) == (OverlapFlags) 0);
                        if ((flags1 & OverlapFlags.MergeStart) != (OverlapFlags) 0)
                        {
                          t.x = 0.0f;
                          flags1 |= flag ? OverlapFlags.OverlapRight : OverlapFlags.OverlapLeft;
                        }
                        if ((flags2 & OverlapFlags.MergeStart) != (OverlapFlags) 0)
                        {
                          t.z = 0.0f;
                          flags2 |= flag ? OverlapFlags.OverlapLeft : OverlapFlags.OverlapRight;
                        }
                        if ((flags1 & OverlapFlags.MergeEnd) != (OverlapFlags) 0)
                        {
                          t.y = 1f;
                          flags1 |= flag ? OverlapFlags.OverlapLeft : OverlapFlags.OverlapRight;
                        }
                        if ((flags2 & OverlapFlags.MergeEnd) != (OverlapFlags) 0)
                        {
                          t.w = 1f;
                          flags2 |= flag ? OverlapFlags.OverlapRight : OverlapFlags.OverlapLeft;
                        }
                      }
                      int priorityDelta = 0;
                      if (((netLaneData1.m_Flags | netLaneData2.m_Flags) & LaneFlags.Pedestrian) != (LaneFlags) 0)
                      {
                        if ((netLaneData1.m_Flags & LaneFlags.Road) != (LaneFlags) 0 && (pedestrianLane2.m_Flags & PedestrianLaneFlags.Unsafe) == (PedestrianLaneFlags) 0)
                          priorityDelta = 1;
                        else if ((netLaneData2.m_Flags & LaneFlags.Road) != (LaneFlags) 0 && (pedestrianLane1.m_Flags & PedestrianLaneFlags.Unsafe) == (PedestrianLaneFlags) 0)
                          priorityDelta = -1;
                      }
                      else if ((flags1 & (OverlapFlags.MergeStart | OverlapFlags.MergeMiddleStart)) == (OverlapFlags) 0)
                      {
                        if ((carLaneFlags & CarLaneFlags.Stop) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                          priorityDelta = math.select(1, -1, (carLane2.m_Flags & CarLaneFlags.Stop) > ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter));
                        else if ((carLaneFlags & CarLaneFlags.Yield) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                          priorityDelta = math.select(1, -1, (carLane2.m_Flags & CarLaneFlags.Yield) > ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter));
                        else if ((carLaneFlags & CarLaneFlags.RightOfWay) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                          priorityDelta = math.select(1, -1, (carLane1.m_Flags & CarLaneFlags.RightOfWay) > ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter));
                        else if ((carLaneFlags & CarLaneFlags.Unsafe) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                        {
                          priorityDelta = math.select(1, -1, (carLane2.m_Flags & CarLaneFlags.Unsafe) > ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter));
                        }
                        else
                        {
                          float2 float2_2 = math.lerp(t.xz, t.yw, 0.5f);
                          float3 = MathUtils.Tangent(curve1.m_Bezier, float2_2.x);
                          float2 xz11 = float3.xz;
                          float3 = MathUtils.Tangent(curve2.m_Bezier, float2_2.y);
                          float2 xz12 = float3.xz;
                          // ISSUE: reference to a compiler-generated field
                          priorityDelta = math.csum(math.select(new int2(), new int2(1, -1), (!this.m_LeftHandTraffic ? new float2(math.dot(MathUtils.Left(xz11), xz12), math.dot(MathUtils.Left(xz12), xz11)) : new float2(math.dot(MathUtils.Right(xz11), xz12), math.dot(MathUtils.Right(xz12), xz11))) > 0.0f));
                        }
                      }
                      if ((netLaneData2.m_Flags & LaneFlags.Parking) == (LaneFlags) 0)
                        bufferData1.Add(new LaneOverlap(subLane2, t, flags1, float2_1.x, priorityDelta));
                      if ((netLaneData1.m_Flags & LaneFlags.Parking) == (LaneFlags) 0)
                        bufferData2.Add(new LaneOverlap(subLane1, t.zwxy, flags2, float2_1.y, -priorityDelta));
                    }
                  }
                }
              }
              if (component1)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_NodeLaneData[subLane1] = componentData3;
              }
            }
          }
        }
        for (int index = 0; index < lanes.Length; ++index)
        {
          Entity subLane = lanes[index].m_SubLane;
          DynamicBuffer<LaneOverlap> bufferData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Overlaps.TryGetBuffer(subLane, out bufferData) && !this.m_SecondaryLaneData.HasComponent(subLane))
            bufferData.AsNativeArray().Sort<LaneOverlap>();
        }
      }

      private static float CalculateParallelism(
        Bezier4x2 curve1,
        Bezier4x2 curve2,
        float4 overlapRange)
      {
        float2 float2 = math.lerp(overlapRange.xz, overlapRange.yw, 0.5f);
        float2 x1 = math.normalizesafe(MathUtils.Tangent(curve1, overlapRange.x));
        float2 x2 = math.normalizesafe(MathUtils.Tangent(curve1, float2.x));
        float2 x3 = math.normalizesafe(MathUtils.Tangent(curve1, overlapRange.y));
        float2 y1 = math.normalizesafe(MathUtils.Tangent(curve2, overlapRange.z));
        float2 y2 = math.normalizesafe(MathUtils.Tangent(curve2, float2.y));
        float2 y3 = math.normalizesafe(MathUtils.Tangent(curve2, overlapRange.w));
        return math.max(0.0f, (float) (((double) math.dot(x1, y1) + (double) math.dot(x2, y2) + (double) math.dot(x3, y3)) * 0.3333333432674408));
      }

      private static unsafe bool FindOverlapRange(
        Bezier4x2 curve1,
        Bezier4x2 curve2,
        float2 radius1,
        float2 radius2,
        float angle1,
        float angle2,
        out float4 t)
      {
        bool overlapRange = false;
        t = new float4(2f, -1f, 2f, -1f);
        LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem* overlapStackItemPtr1 = stackalloc LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem[13];
        int num1 = 0;
        float2 float2_1;
        float2_1.x = MathUtils.Length(curve1);
        float2_1.y = MathUtils.Length(curve2);
        float2 float2_2 = math.sqrt(float2_1 / math.max((float2) 0.1f, math.min(new float2(radius1.x, radius2.x), new float2(radius1.y, radius2.y))));
        float2 float2_3 = 1f / math.cos(new float2(angle1, angle2));
        float2 float2_4 = math.tan(new float2(angle1, angle2));
        int2 x1;
        x1.x = Mathf.RoundToInt(float2_2.x);
        x1.y = Mathf.RoundToInt(float2_2.y);
        int2 int2_1 = math.min(x1, (int2) 4);
        LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem* overlapStackItemPtr2 = overlapStackItemPtr1;
        int num2 = num1;
        int num3 = num2 + 1;
        IntPtr num4 = (IntPtr) num2 * sizeof (LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem);
        // ISSUE: object of a compiler-generated type is created
        *(LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem*) ((IntPtr) overlapStackItemPtr2 + num4) = new LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem()
        {
          m_Curve1 = curve1,
          m_Curve2 = curve2,
          m_CurvePos1 = new float2(0.0f, 1f),
          m_CurvePos2 = new float2(0.0f, 1f),
          m_Iterations = int2_1
        };
        while (num3 != 0)
        {
          // ISSUE: explicit reference operation
          ref LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem local = @overlapStackItemPtr1[--num3];
          // ISSUE: reference to a compiler-generated field
          float2 x2 = math.lerp(radius1.xx, radius1.yy, local.m_CurvePos1);
          // ISSUE: reference to a compiler-generated field
          float2 x3 = math.lerp(radius2.xx, radius2.yy, local.m_CurvePos2);
          // ISSUE: reference to a compiler-generated field
          Bounds2 bounds2_1 = MathUtils.Bounds(local.m_Curve1);
          // ISSUE: reference to a compiler-generated field
          Bounds2 bounds2_2 = MathUtils.Bounds(local.m_Curve2);
          float2 x4 = new float2(math.cmax(x2), math.cmax(x3)) * float2_3;
          float num5 = math.csum(x4);
          if ((double) MathUtils.DistanceSquared(bounds2_1, bounds2_2) < (double) num5 * (double) num5)
          {
            // ISSUE: reference to a compiler-generated field
            int2 int2_2 = local.m_Iterations - 1;
            // ISSUE: variable of a compiler-generated type
            LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem overlapStackItem1;
            if (math.all(int2_2 < 0))
            {
              // ISSUE: reference to a compiler-generated field
              float2 forward1 = MathUtils.Right(MathUtils.StartTangent(local.m_Curve1));
              // ISSUE: reference to a compiler-generated field
              float2 float2_5 = MathUtils.Right(MathUtils.EndTangent(local.m_Curve1));
              // ISSUE: reference to a compiler-generated field
              float2 float2_6 = MathUtils.Right(MathUtils.StartTangent(local.m_Curve2));
              // ISSUE: reference to a compiler-generated field
              float2 float2_7 = MathUtils.Right(MathUtils.EndTangent(local.m_Curve2));
              MathUtils.TryNormalize(ref forward1);
              MathUtils.TryNormalize(ref float2_5);
              MathUtils.TryNormalize(ref float2_6);
              MathUtils.TryNormalize(ref float2_7);
              forward1 *= x2.x;
              float2 forward2 = float2_5 * x2.y;
              float2 forward3 = float2_6 * x3.x;
              float2 forward4 = float2_7 * x3.y;
              forward1 += MathUtils.Left(forward1) * float2_4.x;
              float2 float2_8 = forward2 + MathUtils.Left(forward2) * float2_4.x;
              float2 float2_9 = forward3 + MathUtils.Left(forward3) * float2_4.y;
              float2 float2_10 = forward4 + MathUtils.Left(forward4) * float2_4.y;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Quad2 quad2_1 = new Quad2(local.m_Curve1.a + forward1, local.m_Curve1.d + float2_8, local.m_Curve1.d - float2_8, local.m_Curve1.a - forward1);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Quad2 quad2_2 = new Quad2(local.m_Curve2.a + float2_9, local.m_Curve2.d + float2_10, local.m_Curve2.d - float2_10, local.m_Curve2.a - float2_9);
              Line2.Segment ab1 = quad2_1.ab;
              Line2.Segment dc1 = quad2_1.dc;
              Line2.Segment ad1 = quad2_1.ad;
              Line2.Segment bc1 = quad2_1.bc;
              Line2.Segment ab2 = quad2_2.ab;
              Line2.Segment dc2 = quad2_2.dc;
              Line2.Segment ad2 = quad2_2.ad;
              Line2.Segment bc2 = quad2_2.bc;
              MathUtils.Expand(bounds2_1, (float2) x4.x);
              Bounds2 bounds2_3 = MathUtils.Expand(bounds2_2, (float2) x4.y);
              float2 t1;
              if (MathUtils.Intersect(MathUtils.Bounds(ab1), bounds2_3))
              {
                if (MathUtils.Intersect(ab1, ab2, out t1))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = math.lerp(new float2(local.m_CurvePos1.x, local.m_CurvePos2.x), new float2(local.m_CurvePos1.y, local.m_CurvePos2.y), t1);
                  t.xz = math.min(t.xz, t1);
                  t.yw = math.max(t.yw, t1);
                  overlapRange = true;
                }
                if (MathUtils.Intersect(ab1, dc2, out t1))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = math.lerp(new float2(local.m_CurvePos1.x, local.m_CurvePos2.x), new float2(local.m_CurvePos1.y, local.m_CurvePos2.y), t1);
                  t.xz = math.min(t.xz, t1);
                  t.yw = math.max(t.yw, t1);
                  overlapRange = true;
                }
                if (MathUtils.Intersect(ab1, ad2, out t1))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = new float2(math.lerp(local.m_CurvePos1.x, local.m_CurvePos1.y, t1.x), local.m_CurvePos2.x);
                  t.xz = math.min(t.xz, t1);
                  t.yw = math.max(t.yw, t1);
                  overlapRange = true;
                }
                if (MathUtils.Intersect(ab1, bc2, out t1))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = new float2(math.lerp(local.m_CurvePos1.x, local.m_CurvePos1.y, t1.x), local.m_CurvePos2.y);
                  t.xz = math.min(t.xz, t1);
                  t.yw = math.max(t.yw, t1);
                  overlapRange = true;
                }
              }
              if (MathUtils.Intersect(MathUtils.Bounds(dc1), bounds2_3))
              {
                if (MathUtils.Intersect(dc1, ab2, out t1))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = math.lerp(new float2(local.m_CurvePos1.x, local.m_CurvePos2.x), new float2(local.m_CurvePos1.y, local.m_CurvePos2.y), t1);
                  t.xz = math.min(t.xz, t1);
                  t.yw = math.max(t.yw, t1);
                  overlapRange = true;
                }
                if (MathUtils.Intersect(dc1, dc2, out t1))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = math.lerp(new float2(local.m_CurvePos1.x, local.m_CurvePos2.x), new float2(local.m_CurvePos1.y, local.m_CurvePos2.y), t1);
                  t.xz = math.min(t.xz, t1);
                  t.yw = math.max(t.yw, t1);
                  overlapRange = true;
                }
                if (MathUtils.Intersect(dc1, ad2, out t1))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = new float2(math.lerp(local.m_CurvePos1.x, local.m_CurvePos1.y, t1.x), local.m_CurvePos2.x);
                  t.xz = math.min(t.xz, t1);
                  t.yw = math.max(t.yw, t1);
                  overlapRange = true;
                }
                if (MathUtils.Intersect(dc1, bc2, out t1))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = new float2(math.lerp(local.m_CurvePos1.x, local.m_CurvePos1.y, t1.x), local.m_CurvePos2.y);
                  t.xz = math.min(t.xz, t1);
                  t.yw = math.max(t.yw, t1);
                  overlapRange = true;
                }
              }
              if (MathUtils.Intersect(MathUtils.Bounds(ad1), bounds2_3))
              {
                if (MathUtils.Intersect(ad1, ab2, out t1))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = new float2(local.m_CurvePos1.x, math.lerp(local.m_CurvePos2.x, local.m_CurvePos2.y, t1.y));
                  t.xz = math.min(t.xz, t1);
                  t.yw = math.max(t.yw, t1);
                  overlapRange = true;
                }
                if (MathUtils.Intersect(ad1, dc2, out t1))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = new float2(local.m_CurvePos1.x, math.lerp(local.m_CurvePos2.x, local.m_CurvePos2.y, t1.y));
                  t.xz = math.min(t.xz, t1);
                  t.yw = math.max(t.yw, t1);
                  overlapRange = true;
                }
                if (MathUtils.Intersect(ad1, ad2, out t1))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = new float2(local.m_CurvePos1.x, local.m_CurvePos2.x);
                  t.xz = math.min(t.xz, t1);
                  t.yw = math.max(t.yw, t1);
                  overlapRange = true;
                }
                if (MathUtils.Intersect(ad1, bc2, out t1))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = new float2(local.m_CurvePos1.x, local.m_CurvePos2.y);
                  t.xz = math.min(t.xz, t1);
                  t.yw = math.max(t.yw, t1);
                  overlapRange = true;
                }
              }
              if (MathUtils.Intersect(MathUtils.Bounds(bc1), bounds2_3))
              {
                if (MathUtils.Intersect(bc1, ab2, out t1))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = new float2(local.m_CurvePos1.y, math.lerp(local.m_CurvePos2.x, local.m_CurvePos2.y, t1.y));
                  t.xz = math.min(t.xz, t1);
                  t.yw = math.max(t.yw, t1);
                  overlapRange = true;
                }
                if (MathUtils.Intersect(bc1, dc2, out t1))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = new float2(local.m_CurvePos1.y, math.lerp(local.m_CurvePos2.x, local.m_CurvePos2.y, t1.y));
                  t.xz = math.min(t.xz, t1);
                  t.yw = math.max(t.yw, t1);
                  overlapRange = true;
                }
                if (MathUtils.Intersect(bc1, ad2, out t1))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = new float2(local.m_CurvePos1.y, local.m_CurvePos2.x);
                  t.xz = math.min(t.xz, t1);
                  t.yw = math.max(t.yw, t1);
                  overlapRange = true;
                }
                if (MathUtils.Intersect(bc1, bc2, out t1))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = new float2(local.m_CurvePos1.y, local.m_CurvePos2.y);
                  t.xz = math.min(t.xz, t1);
                  t.yw = math.max(t.yw, t1);
                  overlapRange = true;
                }
              }
            }
            else if (math.all(int2_2 >= 0))
            {
              Bezier4x2 output1_1;
              Bezier4x2 output2_1;
              // ISSUE: reference to a compiler-generated field
              MathUtils.Divide(local.m_Curve1, out output1_1, out output2_1, 0.5f);
              Bezier4x2 output1_2;
              Bezier4x2 output2_2;
              // ISSUE: reference to a compiler-generated field
              MathUtils.Divide(local.m_Curve2, out output1_2, out output2_2, 0.5f);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 float3_1 = new float3(local.m_CurvePos1.x, math.lerp(local.m_CurvePos1.x, local.m_CurvePos1.y, 0.5f), local.m_CurvePos1.y);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 float3_2 = new float3(local.m_CurvePos2.x, math.lerp(local.m_CurvePos2.x, local.m_CurvePos2.y, 0.5f), local.m_CurvePos2.y);
              LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem* overlapStackItemPtr3 = overlapStackItemPtr1;
              int num6 = num3;
              int num7 = num6 + 1;
              IntPtr num8 = (IntPtr) num6 * sizeof (LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem);
              IntPtr num9 = (IntPtr) overlapStackItemPtr3 + num8;
              // ISSUE: object of a compiler-generated type is created
              overlapStackItem1 = new LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem();
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Curve1 = output1_1;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Curve2 = output1_2;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_CurvePos1 = float3_1.xy;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_CurvePos2 = float3_2.xy;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Iterations = int2_2;
              // ISSUE: variable of a compiler-generated type
              LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem overlapStackItem2 = overlapStackItem1;
              *(LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem*) num9 = overlapStackItem2;
              LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem* overlapStackItemPtr4 = overlapStackItemPtr1;
              int num10 = num7;
              int num11 = num10 + 1;
              IntPtr num12 = (IntPtr) num10 * sizeof (LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem);
              IntPtr num13 = (IntPtr) overlapStackItemPtr4 + num12;
              // ISSUE: object of a compiler-generated type is created
              overlapStackItem1 = new LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem();
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Curve1 = output1_1;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Curve2 = output2_2;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_CurvePos1 = float3_1.xy;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_CurvePos2 = float3_2.yz;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Iterations = int2_2;
              // ISSUE: variable of a compiler-generated type
              LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem overlapStackItem3 = overlapStackItem1;
              *(LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem*) num13 = overlapStackItem3;
              LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem* overlapStackItemPtr5 = overlapStackItemPtr1;
              int num14 = num11;
              int num15 = num14 + 1;
              IntPtr num16 = (IntPtr) num14 * sizeof (LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem);
              IntPtr num17 = (IntPtr) overlapStackItemPtr5 + num16;
              // ISSUE: object of a compiler-generated type is created
              overlapStackItem1 = new LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem();
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Curve1 = output2_1;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Curve2 = output1_2;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_CurvePos1 = float3_1.yz;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_CurvePos2 = float3_2.xy;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Iterations = int2_2;
              // ISSUE: variable of a compiler-generated type
              LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem overlapStackItem4 = overlapStackItem1;
              *(LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem*) num17 = overlapStackItem4;
              LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem* overlapStackItemPtr6 = overlapStackItemPtr1;
              int num18 = num15;
              num3 = num18 + 1;
              IntPtr num19 = (IntPtr) num18 * sizeof (LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem);
              IntPtr num20 = (IntPtr) overlapStackItemPtr6 + num19;
              // ISSUE: object of a compiler-generated type is created
              overlapStackItem1 = new LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem();
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Curve1 = output2_1;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Curve2 = output2_2;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_CurvePos1 = float3_1.yz;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_CurvePos2 = float3_2.yz;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Iterations = int2_2;
              // ISSUE: variable of a compiler-generated type
              LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem overlapStackItem5 = overlapStackItem1;
              *(LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem*) num20 = overlapStackItem5;
            }
            else if (int2_2.x >= 0)
            {
              Bezier4x2 output1;
              Bezier4x2 output2;
              // ISSUE: reference to a compiler-generated field
              MathUtils.Divide(local.m_Curve1, out output1, out output2, 0.5f);
              // ISSUE: reference to a compiler-generated field
              curve2 = local.m_Curve2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 float3 = new float3(local.m_CurvePos1.x, math.lerp(local.m_CurvePos1.x, local.m_CurvePos1.y, 0.5f), local.m_CurvePos1.y);
              // ISSUE: reference to a compiler-generated field
              float2 curvePos2 = local.m_CurvePos2;
              LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem* overlapStackItemPtr7 = overlapStackItemPtr1;
              int num21 = num3;
              int num22 = num21 + 1;
              IntPtr num23 = (IntPtr) num21 * sizeof (LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem);
              IntPtr num24 = (IntPtr) overlapStackItemPtr7 + num23;
              // ISSUE: object of a compiler-generated type is created
              overlapStackItem1 = new LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem();
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Curve1 = output1;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Curve2 = curve2;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_CurvePos1 = float3.xy;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_CurvePos2 = curvePos2;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Iterations = int2_2;
              // ISSUE: variable of a compiler-generated type
              LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem overlapStackItem6 = overlapStackItem1;
              *(LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem*) num24 = overlapStackItem6;
              LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem* overlapStackItemPtr8 = overlapStackItemPtr1;
              int num25 = num22;
              num3 = num25 + 1;
              IntPtr num26 = (IntPtr) num25 * sizeof (LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem);
              IntPtr num27 = (IntPtr) overlapStackItemPtr8 + num26;
              // ISSUE: object of a compiler-generated type is created
              overlapStackItem1 = new LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem();
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Curve1 = output2;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Curve2 = curve2;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_CurvePos1 = float3.yz;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_CurvePos2 = curvePos2;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Iterations = int2_2;
              // ISSUE: variable of a compiler-generated type
              LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem overlapStackItem7 = overlapStackItem1;
              *(LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem*) num27 = overlapStackItem7;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              curve1 = local.m_Curve1;
              Bezier4x2 output1;
              Bezier4x2 output2;
              // ISSUE: reference to a compiler-generated field
              MathUtils.Divide(local.m_Curve2, out output1, out output2, 0.5f);
              // ISSUE: reference to a compiler-generated field
              float2 curvePos1 = local.m_CurvePos1;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 float3 = new float3(local.m_CurvePos2.x, math.lerp(local.m_CurvePos2.x, local.m_CurvePos2.y, 0.5f), local.m_CurvePos2.y);
              LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem* overlapStackItemPtr9 = overlapStackItemPtr1;
              int num28 = num3;
              int num29 = num28 + 1;
              IntPtr num30 = (IntPtr) num28 * sizeof (LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem);
              IntPtr num31 = (IntPtr) overlapStackItemPtr9 + num30;
              // ISSUE: object of a compiler-generated type is created
              overlapStackItem1 = new LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem();
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Curve1 = curve1;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Curve2 = output1;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_CurvePos1 = curvePos1;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_CurvePos2 = float3.xy;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Iterations = int2_2;
              // ISSUE: variable of a compiler-generated type
              LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem overlapStackItem8 = overlapStackItem1;
              *(LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem*) num31 = overlapStackItem8;
              LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem* overlapStackItemPtr10 = overlapStackItemPtr1;
              int num32 = num29;
              num3 = num32 + 1;
              IntPtr num33 = (IntPtr) num32 * sizeof (LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem);
              IntPtr num34 = (IntPtr) overlapStackItemPtr10 + num33;
              // ISSUE: object of a compiler-generated type is created
              overlapStackItem1 = new LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem();
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Curve1 = curve1;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Curve2 = output2;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_CurvePos1 = curvePos1;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_CurvePos2 = float3.yz;
              // ISSUE: reference to a compiler-generated field
              overlapStackItem1.m_Iterations = int2_2;
              // ISSUE: variable of a compiler-generated type
              LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem overlapStackItem9 = overlapStackItem1;
              *(LaneOverlapSystem.UpdateLaneOverlapsJob.FindOverlapStackItem*) num34 = overlapStackItem9;
            }
          }
        }
        return overlapRange;
      }

      private struct FindOverlapStackItem
      {
        public Bezier4x2 m_Curve1;
        public Bezier4x2 m_Curve2;
        public float2 m_CurvePos1;
        public float2 m_CurvePos2;
        public int2 m_Iterations;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SecondaryLane> __Game_Net_SecondaryLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubLane> __Game_Net_SubLane_RO_BufferLookup;
      public ComponentLookup<NodeLane> __Game_Net_NodeLane_RW_ComponentLookup;
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RW_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Lane> __Game_Net_Lane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EdgeLane> __Game_Net_EdgeLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CarLane> __Game_Net_CarLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TrackLane> __Game_Net_TrackLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SlaveLane> __Game_Net_SlaveLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<MasterLane> __Game_Net_MasterLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<OutsideConnection> __Game_Net_OutsideConnection_RO_ComponentLookup;
      public ComponentTypeHandle<EdgeLane> __Game_Net_EdgeLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NodeLane> __Game_Net_NodeLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CarLane> __Game_Net_CarLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<TrackLane> __Game_Net_TrackLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<SlaveLane> __Game_Net_SlaveLane_RW_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentLookup = state.GetComponentLookup<PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SecondaryLane_RO_ComponentLookup = state.GetComponentLookup<SecondaryLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeLane_RW_ComponentLookup = state.GetComponentLookup<NodeLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RW_BufferLookup = state.GetBufferLookup<LaneOverlap>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrackLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TrackLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_MasterLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MasterLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferTypeHandle = state.GetBufferTypeHandle<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NodeLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrackLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TrackLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SlaveLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
      }
    }
  }
}
