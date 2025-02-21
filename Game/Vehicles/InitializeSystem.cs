// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.InitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
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
namespace Game.Vehicles
{
  [CompilerGenerated]
  public class InitializeSystem : GameSystemBase
  {
    private Game.Objects.SearchSystem m_SearchSystem;
    private EntityQuery m_VehicleQuery;
    private InitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadOnly<Updated>(), ComponentType.ReadOnly<Vehicle>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_VehicleQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainBogieFrame_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarTrailerLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainNavigation_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedTrain_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarTrailerData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarTractorData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftNavigation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftNavigation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarNavigation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TripSource_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      InitializeSystem.InitializeVehiclesJob jobData1 = new InitializeSystem.InitializeVehiclesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TrainType = this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentTypeHandle,
        m_TripSourceType = this.__TypeHandle.__Game_Objects_TripSource_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_HelicopterType = this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_LayoutElementType = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle,
        m_CarNavigationType = this.__TypeHandle.__Game_Vehicles_CarNavigation_RW_ComponentTypeHandle,
        m_CarCurrentLaneType = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle,
        m_WatercraftNavigationType = this.__TypeHandle.__Game_Vehicles_WatercraftNavigation_RW_ComponentTypeHandle,
        m_WatercraftCurrentLaneType = this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RW_ComponentTypeHandle,
        m_AircraftNavigationType = this.__TypeHandle.__Game_Vehicles_AircraftNavigation_RW_ComponentTypeHandle,
        m_AircraftCurrentLaneType = this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RW_ComponentTypeHandle,
        m_ParkedCarType = this.__TypeHandle.__Game_Vehicles_ParkedCar_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_PathElementType = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_TrainData = this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup,
        m_DeliveryTruckData = this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_OutsideConnectionData = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabTrainData = this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabCarTractorData = this.__TypeHandle.__Game_Prefabs_CarTractorData_RO_ComponentLookup,
        m_PrefabCarTrailerData = this.__TypeHandle.__Game_Prefabs_CarTrailerData_RO_ComponentLookup,
        m_SpawnLocations = this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RO_BufferLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup,
        m_TrainCurrentLaneData = this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RW_ComponentLookup,
        m_ParkedTrainData = this.__TypeHandle.__Game_Vehicles_ParkedTrain_RW_ComponentLookup,
        m_TrainNavigationData = this.__TypeHandle.__Game_Vehicles_TrainNavigation_RW_ComponentLookup,
        m_CarTrailerLaneData = this.__TypeHandle.__Game_Vehicles_CarTrailerLane_RW_ComponentLookup,
        m_TrainBogieFrames = this.__TypeHandle.__Game_Vehicles_TrainBogieFrame_RW_BufferLookup,
        m_RandomSeed = RandomSeed.Next()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarTrailerLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      InitializeSystem.TreeFixJob jobData2 = new InitializeSystem.TreeFixJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_CarCurrentLaneType = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentTypeHandle,
        m_CarTrailerLaneType = this.__TypeHandle.__Game_Vehicles_CarTrailerLane_RO_ComponentTypeHandle,
        m_SearchTree = this.m_SearchSystem.GetMovingSearchTree(false, out dependencies),
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle job0 = jobData1.ScheduleParallel<InitializeSystem.InitializeVehiclesJob>(this.m_VehicleQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      EntityQuery vehicleQuery = this.m_VehicleQuery;
      JobHandle dependsOn = JobHandle.CombineDependencies(job0, dependencies);
      JobHandle jobHandle = jobData2.Schedule<InitializeSystem.TreeFixJob>(vehicleQuery, dependsOn);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SearchSystem.AddMovingSearchTreeWriter(jobHandle);
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
    public InitializeSystem()
    {
    }

    [BurstCompile]
    private struct TreeFixJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<CarCurrentLane> m_CarCurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<CarTrailerLane> m_CarTrailerLaneType;
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;
      public BufferLookup<LaneObject> m_LaneObjects;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Created>(ref this.m_CreatedType))
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarCurrentLane> nativeArray2 = chunk.GetNativeArray<CarCurrentLane>(ref this.m_CarCurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarTrailerLane> nativeArray3 = chunk.GetNativeArray<CarTrailerLane>(ref this.m_CarTrailerLaneType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity laneObject1 = nativeArray1[index];
          CarCurrentLane carCurrentLane = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneObjects.HasBuffer(carCurrentLane.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<LaneObject> laneObject2 = this.m_LaneObjects[carCurrentLane.m_Lane];
            if (!CollectionUtils.ContainsValue<LaneObject>(laneObject2, new LaneObject(laneObject1)))
              NetUtils.AddLaneObject(laneObject2, laneObject1, carCurrentLane.m_CurvePosition.xy);
            // ISSUE: reference to a compiler-generated field
            this.m_SearchTree.TryRemove(laneObject1);
          }
        }
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Entity laneObject3 = nativeArray1[index];
          CarTrailerLane carTrailerLane = nativeArray3[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneObjects.HasBuffer(carTrailerLane.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<LaneObject> laneObject4 = this.m_LaneObjects[carTrailerLane.m_Lane];
            if (!CollectionUtils.ContainsValue<LaneObject>(laneObject4, new LaneObject(laneObject3)))
              NetUtils.AddLaneObject(laneObject4, laneObject3, carTrailerLane.m_CurvePosition.xy);
            // ISSUE: reference to a compiler-generated field
            this.m_SearchTree.TryRemove(laneObject3);
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

    [BurstCompile]
    private struct InitializeVehiclesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Train> m_TrainType;
      [ReadOnly]
      public ComponentTypeHandle<TripSource> m_TripSourceType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<Helicopter> m_HelicopterType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> m_LayoutElementType;
      public ComponentTypeHandle<CarNavigation> m_CarNavigationType;
      public ComponentTypeHandle<CarCurrentLane> m_CarCurrentLaneType;
      public ComponentTypeHandle<WatercraftNavigation> m_WatercraftNavigationType;
      public ComponentTypeHandle<WatercraftCurrentLane> m_WatercraftCurrentLaneType;
      public ComponentTypeHandle<AircraftNavigation> m_AircraftNavigationType;
      public ComponentTypeHandle<AircraftCurrentLane> m_AircraftCurrentLaneType;
      public ComponentTypeHandle<ParkedCar> m_ParkedCarType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Train> m_TrainData;
      [ReadOnly]
      public ComponentLookup<DeliveryTruck> m_DeliveryTruckData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnectionData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<TrainData> m_PrefabTrainData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<CarTractorData> m_PrefabCarTractorData;
      [ReadOnly]
      public ComponentLookup<CarTrailerData> m_PrefabCarTrailerData;
      [ReadOnly]
      public BufferLookup<SpawnLocationElement> m_SpawnLocations;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Transform> m_TransformData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<TrainCurrentLane> m_TrainCurrentLaneData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<TrainNavigation> m_TrainNavigationData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<CarTrailerLane> m_CarTrailerLaneData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<ParkedTrain> m_ParkedTrainData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<TrainBogieFrame> m_TrainBogieFrames;
      [ReadOnly]
      public RandomSeed m_RandomSeed;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarNavigation> nativeArray2 = chunk.GetNativeArray<CarNavigation>(ref this.m_CarNavigationType);
        if (nativeArray2.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<CarCurrentLane> nativeArray3 = chunk.GetNativeArray<CarCurrentLane>(ref this.m_CarCurrentLaneType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<TripSource> nativeArray4 = chunk.GetNativeArray<TripSource>(ref this.m_TripSourceType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PathOwner> nativeArray5 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray6 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<LayoutElement> bufferAccessor1 = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutElementType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<PathElement> bufferAccessor2 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
          // ISSUE: reference to a compiler-generated field
          bool flag = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
          for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
          {
            Entity entity = nativeArray1[index1];
            CarCurrentLane currentLane = nativeArray3[index1];
            CarNavigation carNavigation = new CarNavigation();
            if (flag && nativeArray4.Length != 0)
            {
              TripSource tripSource = nativeArray4[index1];
              PathOwner pathOwner = nativeArray5[index1];
              DynamicBuffer<PathElement> path = bufferAccessor2[index1];
              // ISSUE: reference to a compiler-generated method
              this.InitializeRoadVehicle(ref random, entity, RoadTypes.Car, tripSource, pathOwner, path);
              if (currentLane.m_Lane == Entity.Null && path.Length > pathOwner.m_ElementIndex)
              {
                PathElement pathElement = path[pathOwner.m_ElementIndex];
                CarLaneFlags flags = CarLaneFlags.FixedLane;
                Game.Net.ConnectionLane componentData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_ConnectionLaneData.TryGetComponent(pathElement.m_Target, out componentData))
                {
                  if ((componentData.m_Flags & ConnectionLaneFlags.Area) != (ConnectionLaneFlags) 0)
                    flags |= CarLaneFlags.Area;
                  else
                    flags |= CarLaneFlags.Connection;
                }
                currentLane = new CarCurrentLane(pathElement, flags);
              }
              if (bufferAccessor1.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                Transform transform1 = this.m_TransformData[entity];
                DynamicBuffer<LayoutElement> dynamicBuffer = bufferAccessor1[index1];
                // ISSUE: reference to a compiler-generated field
                CarTractorData carTractorData = this.m_PrefabCarTractorData[nativeArray6[index1].m_Prefab];
                for (int index2 = 1; index2 < dynamicBuffer.Length; ++index2)
                {
                  Entity vehicle = dynamicBuffer[index2].m_Vehicle;
                  // ISSUE: reference to a compiler-generated field
                  CarTrailerLane carTrailerLane = this.m_CarTrailerLaneData[vehicle];
                  // ISSUE: reference to a compiler-generated field
                  PrefabRef prefabRef = this.m_PrefabRefData[vehicle];
                  // ISSUE: reference to a compiler-generated field
                  CarTrailerData carTrailerData = this.m_PrefabCarTrailerData[prefabRef.m_Prefab];
                  Transform transform2 = transform1;
                  transform2.m_Position += math.rotate(transform1.m_Rotation, carTractorData.m_AttachPosition);
                  transform2.m_Position -= math.rotate(transform2.m_Rotation, carTrailerData.m_AttachPosition);
                  // ISSUE: reference to a compiler-generated field
                  this.m_TransformData[vehicle] = transform2;
                  if (carTrailerLane.m_Lane == Entity.Null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CarTrailerLaneData[vehicle] = new CarTrailerLane(currentLane);
                  }
                  if (index2 + 1 < dynamicBuffer.Length)
                  {
                    transform1 = transform2;
                    // ISSUE: reference to a compiler-generated field
                    carTractorData = this.m_PrefabCarTractorData[prefabRef.m_Prefab];
                  }
                }
              }
            }
            currentLane.m_LanePosition = random.NextFloat(-0.25f, 0.25f);
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransformData.HasComponent(currentLane.m_Lane))
              currentLane.m_LaneFlags |= CarLaneFlags.TransformTarget;
            // ISSUE: reference to a compiler-generated field
            carNavigation.m_TargetPosition = this.m_TransformData[entity].m_Position;
            carNavigation.m_TargetRotation = new quaternion();
            nativeArray2[index1] = carNavigation;
            nativeArray3[index1] = currentLane;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<WatercraftNavigation> nativeArray7 = chunk.GetNativeArray<WatercraftNavigation>(ref this.m_WatercraftNavigationType);
          if (nativeArray7.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<WatercraftCurrentLane> nativeArray8 = chunk.GetNativeArray<WatercraftCurrentLane>(ref this.m_WatercraftCurrentLaneType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<TripSource> nativeArray9 = chunk.GetNativeArray<TripSource>(ref this.m_TripSourceType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PathOwner> nativeArray10 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<PathElement> bufferAccessor = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
            // ISSUE: reference to a compiler-generated field
            bool flag = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              WatercraftCurrentLane watercraftCurrentLane = nativeArray8[index];
              WatercraftNavigation watercraftNavigation = new WatercraftNavigation();
              if (flag && nativeArray9.Length != 0)
              {
                TripSource tripSource = nativeArray9[index];
                PathOwner pathOwner = nativeArray10[index];
                DynamicBuffer<PathElement> path = bufferAccessor[index];
                // ISSUE: reference to a compiler-generated method
                this.InitializeRoadVehicle(ref random, entity, RoadTypes.Watercraft, tripSource, pathOwner, path);
                if (watercraftCurrentLane.m_Lane == Entity.Null && path.Length > pathOwner.m_ElementIndex)
                {
                  PathElement pathElement = path[pathOwner.m_ElementIndex];
                  WatercraftLaneFlags flags = WatercraftLaneFlags.FixedLane;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ConnectionLaneData.HasComponent(pathElement.m_Target))
                    flags |= WatercraftLaneFlags.Connection;
                  watercraftCurrentLane = new WatercraftCurrentLane(pathElement, flags);
                }
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_TransformData.HasComponent(watercraftCurrentLane.m_Lane))
                watercraftCurrentLane.m_LaneFlags |= WatercraftLaneFlags.TransformTarget;
              // ISSUE: reference to a compiler-generated field
              watercraftNavigation.m_TargetPosition = this.m_TransformData[entity].m_Position;
              watercraftNavigation.m_TargetDirection = new float3();
              nativeArray8[index] = watercraftCurrentLane;
              nativeArray7[index] = watercraftNavigation;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<AircraftNavigation> nativeArray11 = chunk.GetNativeArray<AircraftNavigation>(ref this.m_AircraftNavigationType);
            if (nativeArray11.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<AircraftCurrentLane> nativeArray12 = chunk.GetNativeArray<AircraftCurrentLane>(ref this.m_AircraftCurrentLaneType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<TripSource> nativeArray13 = chunk.GetNativeArray<TripSource>(ref this.m_TripSourceType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<PathOwner> nativeArray14 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
              // ISSUE: reference to a compiler-generated field
              BufferAccessor<PathElement> bufferAccessor = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
              // ISSUE: reference to a compiler-generated field
              bool flag1 = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
              // ISSUE: reference to a compiler-generated field
              bool flag2 = chunk.Has<Helicopter>(ref this.m_HelicopterType);
              for (int index = 0; index < nativeArray1.Length; ++index)
              {
                Entity entity = nativeArray1[index];
                AircraftCurrentLane aircraftCurrentLane = nativeArray12[index];
                AircraftNavigation aircraftNavigation = new AircraftNavigation();
                if (flag1 && nativeArray13.Length != 0)
                {
                  PathOwner pathOwner = nativeArray14[index];
                  DynamicBuffer<PathElement> path = bufferAccessor[index];
                  // ISSUE: reference to a compiler-generated method
                  this.InitializeRoadVehicle(ref random, entity, flag2 ? RoadTypes.Helicopter : RoadTypes.Airplane, nativeArray13[index], pathOwner, path);
                  if (aircraftCurrentLane.m_Lane == Entity.Null && path.Length > pathOwner.m_ElementIndex)
                  {
                    PathElement pathElement = path[pathOwner.m_ElementIndex];
                    AircraftLaneFlags laneFlags = (AircraftLaneFlags) 0;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_ConnectionLaneData.HasComponent(pathElement.m_Target))
                      laneFlags |= AircraftLaneFlags.Connection;
                    aircraftCurrentLane = new AircraftCurrentLane(pathElement, laneFlags);
                  }
                }
                // ISSUE: reference to a compiler-generated field
                if (this.m_TransformData.HasComponent(aircraftCurrentLane.m_Lane))
                {
                  aircraftCurrentLane.m_LaneFlags |= AircraftLaneFlags.TransformTarget;
                  Game.Prefabs.SpawnLocationData componentData;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_SpawnLocationData.HasComponent(aircraftCurrentLane.m_Lane) && this.m_PrefabSpawnLocationData.TryGetComponent(this.m_PrefabRefData[aircraftCurrentLane.m_Lane].m_Prefab, out componentData) && componentData.m_ConnectionType == RouteConnectionType.Air)
                    aircraftCurrentLane.m_LaneFlags |= AircraftLaneFlags.ParkingSpace;
                }
                // ISSUE: reference to a compiler-generated field
                aircraftNavigation.m_TargetPosition = this.m_TransformData[entity].m_Position;
                aircraftNavigation.m_TargetDirection = new float3();
                nativeArray12[index] = aircraftCurrentLane;
                nativeArray11[index] = aircraftNavigation;
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<ParkedCar> nativeArray15 = chunk.GetNativeArray<ParkedCar>(ref this.m_ParkedCarType);
              if (nativeArray15.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                NativeArray<TripSource> nativeArray16 = chunk.GetNativeArray<TripSource>(ref this.m_TripSourceType);
                // ISSUE: reference to a compiler-generated field
                NativeArray<PrefabRef> nativeArray17 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
                // ISSUE: reference to a compiler-generated field
                if (!chunk.Has<Unspawned>(ref this.m_UnspawnedType))
                  return;
                for (int index = 0; index < nativeArray16.Length; ++index)
                {
                  Entity entity = nativeArray1[index];
                  ParkedCar parkedCar = nativeArray15[index];
                  TripSource tripSource = nativeArray16[index];
                  PrefabRef prefabRef = nativeArray17[index];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_TransformData.HasComponent(tripSource.m_Source))
                  {
                    // ISSUE: reference to a compiler-generated field
                    float3 position = this.m_TransformData[tripSource.m_Source].m_Position;
                    // ISSUE: reference to a compiler-generated method
                    if (this.FindParkingSpace(position, tripSource.m_Source, ref random, out parkedCar.m_Lane, out parkedCar.m_CurvePosition))
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_CurveData.HasComponent(parkedCar.m_Lane))
                      {
                        // ISSUE: reference to a compiler-generated field
                        Curve curve = this.m_CurveData[parkedCar.m_Lane];
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_ParkingLaneData.HasComponent(parkedCar.m_Lane))
                        {
                          // ISSUE: reference to a compiler-generated field
                          Game.Net.ParkingLane parkingLane = this.m_ParkingLaneData[parkedCar.m_Lane];
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          ParkingLaneData parkingLaneData = this.m_PrefabParkingLaneData[this.m_PrefabRefData[parkedCar.m_Lane].m_Prefab];
                          // ISSUE: reference to a compiler-generated field
                          ObjectGeometryData prefabGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
                          Transform ownerTransform = new Transform();
                          Owner componentData;
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          if (this.m_OwnerData.TryGetComponent(parkedCar.m_Lane, out componentData) && this.m_TransformData.HasComponent(componentData.m_Owner))
                          {
                            // ISSUE: reference to a compiler-generated field
                            ownerTransform = this.m_TransformData[componentData.m_Owner];
                          }
                          // ISSUE: reference to a compiler-generated field
                          this.m_TransformData[entity] = VehicleUtils.CalculateParkingSpaceTarget(parkingLane, parkingLaneData, prefabGeometryData, curve, ownerTransform, parkedCar.m_CurvePosition);
                        }
                        else
                        {
                          Transform transform = VehicleUtils.CalculateTransform(curve, parkedCar.m_CurvePosition);
                          // ISSUE: reference to a compiler-generated field
                          if (this.m_ConnectionLaneData.HasComponent(parkedCar.m_Lane))
                          {
                            // ISSUE: reference to a compiler-generated field
                            Game.Net.ConnectionLane connectionLane = this.m_ConnectionLaneData[parkedCar.m_Lane];
                            if ((connectionLane.m_Flags & ConnectionLaneFlags.Parking) != (ConnectionLaneFlags) 0)
                            {
                              parkedCar.m_CurvePosition = random.NextFloat(0.0f, 1f);
                              transform.m_Position = VehicleUtils.GetConnectionParkingPosition(connectionLane, curve.m_Bezier, parkedCar.m_CurvePosition);
                            }
                          }
                          // ISSUE: reference to a compiler-generated field
                          this.m_TransformData[entity] = transform;
                        }
                      }
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      Transform transform = this.m_TransformData[entity];
                      parkedCar.m_CurvePosition = random.NextFloat(0.0f, 1f);
                      Game.Net.ConnectionLane connectionLane = new Game.Net.ConnectionLane();
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_OutsideConnectionData.HasComponent(tripSource.m_Source))
                        connectionLane.m_Flags |= ConnectionLaneFlags.Outside;
                      transform.m_Position = VehicleUtils.GetConnectionParkingPosition(connectionLane, new Bezier4x3(position, position, position, position), parkedCar.m_CurvePosition);
                      // ISSUE: reference to a compiler-generated field
                      this.m_TransformData[entity] = transform;
                    }
                    nativeArray15[index] = parkedCar;
                  }
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                NativeArray<Train> nativeArray18 = chunk.GetNativeArray<Train>(ref this.m_TrainType);
                if (nativeArray18.Length == 0)
                  return;
                // ISSUE: reference to a compiler-generated field
                NativeArray<TripSource> nativeArray19 = chunk.GetNativeArray<TripSource>(ref this.m_TripSourceType);
                // ISSUE: reference to a compiler-generated field
                NativeArray<PathOwner> nativeArray20 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
                // ISSUE: reference to a compiler-generated field
                NativeArray<PrefabRef> nativeArray21 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
                // ISSUE: reference to a compiler-generated field
                BufferAccessor<LayoutElement> bufferAccessor3 = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutElementType);
                // ISSUE: reference to a compiler-generated field
                BufferAccessor<PathElement> bufferAccessor4 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
                NativeList<PathElement> laneBuffer = new NativeList<PathElement>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                // ISSUE: reference to a compiler-generated field
                bool flag3 = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
                // ISSUE: reference to a compiler-generated field
                bool flag4 = chunk.Has<Temp>(ref this.m_TempType);
                for (int index = 0; index < nativeArray18.Length; ++index)
                {
                  Entity entity = nativeArray1[index];
                  ParkedTrain componentData;
                  // ISSUE: reference to a compiler-generated field
                  bool component = this.m_ParkedTrainData.TryGetComponent(entity, out componentData);
                  DynamicBuffer<LayoutElement> layout;
                  bool flag5 = CollectionUtils.TryGet<LayoutElement>(bufferAccessor3, index, out layout);
                  if (component & flag4 || flag3 && nativeArray19.Length != 0)
                  {
                    if (flag5)
                    {
                      PathOwner pathOwner = new PathOwner();
                      DynamicBuffer<PathElement> path = new DynamicBuffer<PathElement>();
                      if (!component)
                      {
                        pathOwner = nativeArray20[index];
                        path = bufferAccessor4[index];
                      }
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      float length = VehicleUtils.CalculateLength(entity, layout, ref this.m_PrefabRefData, ref this.m_PrefabTrainData);
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      PathUtils.InitializeSpawnPath(path, laneBuffer, componentData.m_ParkingLocation, ref pathOwner, length, ref this.m_CurveData, ref this.m_LaneData, ref this.m_EdgeLaneData, ref this.m_OwnerData, ref this.m_EdgeData, ref this.m_SpawnLocationData, ref this.m_ConnectedEdges, ref this.m_SubLanes);
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      VehicleUtils.UpdateCarriageLocations(layout, laneBuffer, ref this.m_TrainData, ref this.m_ParkedTrainData, ref this.m_TrainCurrentLaneData, ref this.m_TrainNavigationData, ref this.m_TransformData, ref this.m_CurveData, ref this.m_ConnectionLaneData, ref this.m_PrefabRefData, ref this.m_PrefabTrainData);
                      if (!component)
                        nativeArray20[index] = pathOwner;
                      laneBuffer.Clear();
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_TrainNavigationData.HasComponent(entity))
                    {
                      Train train = nativeArray18[index];
                      // ISSUE: reference to a compiler-generated field
                      Transform transform = this.m_TransformData[entity];
                      // ISSUE: reference to a compiler-generated field
                      TrainData prefabTrainData = this.m_PrefabTrainData[nativeArray21[index].m_Prefab];
                      float3 pivot1;
                      float3 pivot2;
                      VehicleUtils.CalculateTrainNavigationPivots(transform, prefabTrainData, out pivot1, out pivot2);
                      if ((train.m_Flags & TrainFlags.Reversed) != (TrainFlags) 0)
                        CommonUtils.Swap<float3>(ref pivot1, ref pivot2);
                      TrainNavigation trainNavigation = new TrainNavigation()
                      {
                        m_Front = new TrainBogiePosition(transform),
                        m_Rear = new TrainBogiePosition(transform)
                      };
                      trainNavigation.m_Front.m_Position = pivot1;
                      trainNavigation.m_Rear.m_Position = pivot2;
                      // ISSUE: reference to a compiler-generated field
                      this.m_TrainNavigationData[entity] = trainNavigation;
                    }
                  }
                  if (flag5)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.UpdateBogieFrames(layout);
                  }
                }
              }
            }
          }
        }
      }

      private void InitializeRoadVehicle(
        ref Random random,
        Entity vehicle,
        RoadTypes roadType,
        TripSource tripSource,
        PathOwner pathOwner,
        DynamicBuffer<PathElement> path)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SpawnLocations.HasBuffer(tripSource.m_Source))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SpawnLocationElement> spawnLocation = this.m_SpawnLocations[tripSource.m_Source];
          bool positionFound1;
          bool rotationFound1;
          // ISSUE: reference to a compiler-generated method
          Transform pathTransform = this.CalculatePathTransform(vehicle, pathOwner, path, roadType, out positionFound1, out rotationFound1);
          if (!positionFound1 || !rotationFound1)
          {
            PathMethod pathMethods = PathMethod.Road;
            // ISSUE: reference to a compiler-generated field
            if ((roadType & RoadTypes.Car) != RoadTypes.None && this.m_DeliveryTruckData.HasComponent(vehicle))
              pathMethods |= PathMethod.CargoLoading;
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_TransformData[tripSource.m_Source];
            bool positionFound2;
            bool rotationFound2;
            // ISSUE: reference to a compiler-generated method
            Transform closestSpawnLocation = this.FindClosestSpawnLocation(ref random, pathTransform, pathMethods, TrackTypes.None, roadType, spawnLocation, transform.Equals(pathTransform), out positionFound2, out rotationFound2);
            if (!positionFound1 & positionFound2)
            {
              pathTransform.m_Position = closestSpawnLocation.m_Position;
              positionFound1 = true;
            }
            if (!rotationFound1 & rotationFound2)
            {
              pathTransform.m_Rotation = closestSpawnLocation.m_Rotation;
              rotationFound1 = true;
            }
          }
          if (!rotationFound1)
          {
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_TransformData[tripSource.m_Source];
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[tripSource.m_Source];
            float3 forward;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabBuildingData.HasComponent(prefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              BuildingData buildingData = this.m_PrefabBuildingData[prefabRef.m_Prefab];
              forward = BuildingUtils.CalculateFrontPosition(transform, buildingData.m_LotSize.y) - pathTransform.m_Position;
            }
            else
              forward = transform.m_Position - pathTransform.m_Position;
            if (MathUtils.TryNormalize(ref forward))
            {
              pathTransform.m_Rotation = quaternion.LookRotationSafe(forward, math.up());
              rotationFound1 = true;
            }
            if (!positionFound1)
              pathTransform.m_Position = transform.m_Position;
            if (!rotationFound1)
              pathTransform.m_Rotation = transform.m_Rotation;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_TransformData[vehicle] = pathTransform;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_TransformData.HasComponent(tripSource.m_Source))
            return;
          bool positionFound3;
          bool rotationFound3;
          // ISSUE: reference to a compiler-generated method
          Transform pathTransform = this.CalculatePathTransform(vehicle, pathOwner, path, roadType, out positionFound3, out rotationFound3);
          // ISSUE: reference to a compiler-generated field
          if (!positionFound3 && !rotationFound3 && this.m_OutsideConnectionData.HasComponent(tripSource.m_Source))
          {
            bool positionFound4;
            bool rotationFound4;
            // ISSUE: reference to a compiler-generated method
            Transform connectionLocation = this.FindRandomConnectionLocation(ref random, roadType, tripSource.m_Source, out positionFound4, out rotationFound4);
            if (!positionFound3 & positionFound4)
            {
              pathTransform.m_Position = connectionLocation.m_Position;
              positionFound3 = true;
            }
            if (!rotationFound3 & rotationFound4)
            {
              pathTransform.m_Rotation = connectionLocation.m_Rotation;
              rotationFound3 = true;
            }
          }
          if (!rotationFound3)
          {
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_TransformData[tripSource.m_Source];
            float3 forward = transform.m_Position - pathTransform.m_Position;
            if (MathUtils.TryNormalize(ref forward))
            {
              pathTransform.m_Rotation = quaternion.LookRotationSafe(forward, math.up());
              rotationFound3 = true;
            }
            if (!positionFound3)
              pathTransform.m_Position = transform.m_Position;
            if (!rotationFound3)
              pathTransform.m_Rotation = transform.m_Rotation;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_TransformData[vehicle] = pathTransform;
        }
      }

      private Transform FindRandomConnectionLocation(
        ref Random random,
        RoadTypes roadType,
        Entity source,
        out bool positionFound,
        out bool rotationFound)
      {
        Transform connectionLocation = new Transform();
        positionFound = false;
        rotationFound = false;
        int num = 0;
        float3 float3 = new float3();
        Owner componentData1;
        DynamicBuffer<Game.Net.SubLane> bufferData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.TryGetComponent(source, out componentData1) && this.m_SubLanes.TryGetBuffer(componentData1.m_Owner, out bufferData1))
        {
          for (int index = 0; index < bufferData1.Length; ++index)
          {
            Game.Net.SubLane subLane = bufferData1[index];
            Game.Net.ConnectionLane componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectionLaneData.TryGetComponent(subLane.m_SubLane, out componentData2) && (componentData2.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0 && (componentData2.m_RoadTypes & roadType) != RoadTypes.None && random.NextInt(++num) == 0)
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[subLane.m_SubLane];
              connectionLocation.m_Position = MathUtils.Position(curve.m_Bezier, 0.5f);
              float3 = MathUtils.Tangent(curve.m_Bezier, 0.5f);
              positionFound = true;
            }
          }
        }
        DynamicBuffer<Game.Net.SubLane> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubLanes.TryGetBuffer(source, out bufferData2))
        {
          for (int index = 0; index < bufferData2.Length; ++index)
          {
            Game.Net.SubLane subLane = bufferData2[index];
            Game.Net.ConnectionLane componentData3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectionLaneData.TryGetComponent(subLane.m_SubLane, out componentData3) && (componentData3.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0 && (componentData3.m_RoadTypes & roadType) != RoadTypes.None && random.NextInt(++num) == 0)
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[subLane.m_SubLane];
              connectionLocation.m_Position = MathUtils.Position(curve.m_Bezier, 0.5f);
              float3 = MathUtils.Tangent(curve.m_Bezier, 0.5f);
              positionFound = true;
            }
          }
        }
        if (positionFound && MathUtils.TryNormalize(ref float3))
        {
          connectionLocation.m_Rotation = quaternion.LookRotationSafe(-float3, math.up());
          rotationFound = true;
        }
        return connectionLocation;
      }

      private void UpdateBogieFrames(DynamicBuffer<LayoutElement> layout)
      {
        for (int index1 = 0; index1 < layout.Length; ++index1)
        {
          Entity vehicle = layout[index1].m_Vehicle;
          TrainCurrentLane componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TrainCurrentLaneData.TryGetComponent(vehicle, out componentData))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<TrainBogieFrame> trainBogieFrame = this.m_TrainBogieFrames[vehicle];
            trainBogieFrame.ResizeUninitialized(4);
            for (int index2 = 0; index2 < trainBogieFrame.Length; ++index2)
              trainBogieFrame[index2] = new TrainBogieFrame()
              {
                m_FrontLane = componentData.m_Front.m_Lane,
                m_RearLane = componentData.m_Rear.m_Lane
              };
          }
        }
      }

      private Transform CalculatePathTransform(
        Entity vehicle,
        PathOwner pathOwner,
        DynamicBuffer<PathElement> path,
        RoadTypes roadType,
        out bool positionFound,
        out bool rotationFound)
      {
        // ISSUE: reference to a compiler-generated field
        Transform pathTransform = this.m_TransformData[vehicle];
        positionFound = false;
        rotationFound = false;
        for (int elementIndex = pathOwner.m_ElementIndex; elementIndex < path.Length; ++elementIndex)
        {
          PathElement pathElement = path[elementIndex];
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransformData.HasComponent(pathElement.m_Target))
          {
            // ISSUE: reference to a compiler-generated field
            float3 position = this.m_TransformData[pathElement.m_Target].m_Position;
            if (positionFound)
            {
              float3 forward = position - pathTransform.m_Position;
              if ((roadType & (RoadTypes.Watercraft | RoadTypes.Helicopter)) != RoadTypes.None)
                forward.y = 0.0f;
              if (MathUtils.TryNormalize(ref forward))
              {
                pathTransform.m_Rotation = quaternion.LookRotationSafe(forward, math.up());
                rotationFound = true;
                return pathTransform;
              }
            }
            else
            {
              pathTransform.m_Position = position;
              positionFound = true;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurveData.HasComponent(pathElement.m_Target))
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[pathElement.m_Target];
              float3 float3_1 = MathUtils.Position(curve.m_Bezier, pathElement.m_TargetDelta.x);
              if (positionFound)
              {
                float3 forward = float3_1 - pathTransform.m_Position;
                if ((roadType & (RoadTypes.Watercraft | RoadTypes.Helicopter)) != RoadTypes.None)
                  forward.y = 0.0f;
                if (MathUtils.TryNormalize(ref forward))
                {
                  pathTransform.m_Rotation = quaternion.LookRotationSafe(forward, math.up());
                  rotationFound = true;
                  return pathTransform;
                }
              }
              else
              {
                pathTransform.m_Position = float3_1;
                positionFound = true;
              }
              if ((double) pathElement.m_TargetDelta.x != (double) pathElement.m_TargetDelta.y)
              {
                float3 float3_2 = MathUtils.Tangent(curve.m_Bezier, pathElement.m_TargetDelta.x);
                float3_2 = math.select(float3_2, -float3_2, (double) pathElement.m_TargetDelta.y < (double) pathElement.m_TargetDelta.x);
                if ((roadType & (RoadTypes.Watercraft | RoadTypes.Helicopter)) != RoadTypes.None)
                  float3_2.y = 0.0f;
                if (MathUtils.TryNormalize(ref float3_2))
                {
                  pathTransform.m_Rotation = quaternion.LookRotationSafe(float3_2, math.up());
                  rotationFound = true;
                  return pathTransform;
                }
              }
            }
          }
        }
        return pathTransform;
      }

      private Transform FindClosestSpawnLocation(
        ref Random random,
        Transform compareTransform,
        PathMethod pathMethods,
        TrackTypes trackTypes,
        RoadTypes roadTypes,
        DynamicBuffer<SpawnLocationElement> spawnLocations,
        bool selectRandom,
        out bool positionFound,
        out bool rotationFound)
      {
        Transform closestSpawnLocation = compareTransform;
        positionFound = false;
        rotationFound = false;
        Entity entity = Entity.Null;
        float num1 = float.MaxValue;
        int num2 = 0;
        for (int index = 0; index < spawnLocations.Length; ++index)
        {
          if (spawnLocations[index].m_Type == SpawnLocationType.SpawnLocation)
          {
            Entity spawnLocation = spawnLocations[index].m_SpawnLocation;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Game.Prefabs.SpawnLocationData spawnLocationData = this.m_PrefabSpawnLocationData[this.m_PrefabRefData[spawnLocation].m_Prefab];
            if (spawnLocationData.m_ConnectionType != RouteConnectionType.Air || (pathMethods & PathMethod.Road) == (PathMethod) 0 || (roadTypes & spawnLocationData.m_RoadTypes) == RoadTypes.None)
            {
              PathMethod pathMethod;
              switch (spawnLocationData.m_ConnectionType)
              {
                case RouteConnectionType.Road:
                  pathMethod = pathMethods & PathMethod.Road;
                  break;
                case RouteConnectionType.Pedestrian:
                  pathMethod = pathMethods & PathMethod.Pedestrian;
                  break;
                case RouteConnectionType.Track:
                  pathMethod = pathMethods & PathMethod.Track;
                  break;
                case RouteConnectionType.Cargo:
                  pathMethod = pathMethods & PathMethod.CargoLoading;
                  break;
                case RouteConnectionType.Air:
                  pathMethod = pathMethods & PathMethod.Flying;
                  break;
                default:
                  pathMethod = (PathMethod) 0;
                  break;
              }
              if (pathMethod != (PathMethod) 0)
              {
                TrackTypes trackTypes1 = trackTypes & spawnLocationData.m_TrackTypes;
                RoadTypes roadTypes1 = roadTypes & spawnLocationData.m_RoadTypes;
                if (((pathMethod & PathMethod.Track) == (PathMethod) 0 || trackTypes1 == TrackTypes.None) && ((pathMethod & (PathMethod.Road | PathMethod.CargoLoading)) == (PathMethod) 0 || roadTypes1 == RoadTypes.None))
                  continue;
              }
              else
                continue;
            }
            Transform componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransformData.TryGetComponent(spawnLocation, out componentData))
            {
              if (selectRandom)
              {
                if (random.NextInt(++num2) == 0)
                {
                  closestSpawnLocation.m_Position = componentData.m_Position;
                  positionFound = true;
                  entity = spawnLocation;
                }
              }
              else
              {
                float num3 = math.distance(componentData.m_Position, compareTransform.m_Position);
                if ((double) num3 < (double) num1)
                {
                  closestSpawnLocation.m_Position = componentData.m_Position;
                  positionFound = true;
                  entity = spawnLocation;
                  num1 = num3;
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_SpawnLocationData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Objects.SpawnLocation spawnLocation = this.m_SpawnLocationData[entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurveData.HasComponent(spawnLocation.m_ConnectedLane1))
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[spawnLocation.m_ConnectedLane1];
            float3 forward1 = MathUtils.Position(curve.m_Bezier, spawnLocation.m_CurvePosition1) - closestSpawnLocation.m_Position;
            if ((roadTypes & (RoadTypes.Watercraft | RoadTypes.Helicopter)) != RoadTypes.None)
              forward1.y = 0.0f;
            if (MathUtils.TryNormalize(ref forward1))
            {
              closestSpawnLocation.m_Rotation = quaternion.LookRotationSafe(forward1, math.up());
              rotationFound = true;
              return closestSpawnLocation;
            }
            float3 forward2 = MathUtils.Tangent(curve.m_Bezier, spawnLocation.m_CurvePosition1);
            if ((roadTypes & (RoadTypes.Watercraft | RoadTypes.Helicopter)) != RoadTypes.None)
              forward2.y = 0.0f;
            if (MathUtils.TryNormalize(ref forward2))
            {
              closestSpawnLocation.m_Rotation = quaternion.LookRotationSafe(forward2, math.up());
              rotationFound = true;
              return closestSpawnLocation;
            }
          }
        }
        if (positionFound)
        {
          float3 forward = closestSpawnLocation.m_Position - compareTransform.m_Position;
          if ((roadTypes & (RoadTypes.Watercraft | RoadTypes.Helicopter)) != RoadTypes.None)
            forward.y = 0.0f;
          if (MathUtils.TryNormalize(ref forward))
          {
            closestSpawnLocation.m_Rotation = quaternion.LookRotationSafe(forward, math.up());
            rotationFound = true;
            return closestSpawnLocation;
          }
        }
        return closestSpawnLocation;
      }

      private bool FindParkingSpace(
        float3 comparePosition,
        Entity source,
        ref Random random,
        out Entity lane,
        out float curvePos)
      {
        while (true)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_SpawnLocations.HasBuffer(source))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<SpawnLocationElement> spawnLocation1 = this.m_SpawnLocations[source];
            for (int index = 0; index < spawnLocation1.Length; ++index)
            {
              if (spawnLocation1[index].m_Type == SpawnLocationType.SpawnLocation)
              {
                Entity spawnLocation2 = spawnLocation1[index].m_SpawnLocation;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabSpawnLocationData[this.m_PrefabRefData[spawnLocation2].m_Prefab].m_ConnectionType == RouteConnectionType.Road && this.m_SpawnLocationData.HasComponent(spawnLocation2))
                {
                  // ISSUE: reference to a compiler-generated field
                  Game.Objects.SpawnLocation spawnLocation3 = this.m_SpawnLocationData[spawnLocation2];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_OwnerData.HasComponent(spawnLocation3.m_ConnectedLane1))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Owner owner = this.m_OwnerData[spawnLocation3.m_ConnectedLane1];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    if (this.m_SubLanes.HasBuffer(owner.m_Owner) && this.FindParkingSpace(comparePosition, this.m_SubLanes[owner.m_Owner], ref random, out lane, out curvePos))
                      return true;
                  }
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (!this.m_SubLanes.HasBuffer(source) || !this.FindParkingSpace(comparePosition, this.m_SubLanes[source], ref random, out lane, out curvePos))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingData.HasComponent(source))
            {
              // ISSUE: reference to a compiler-generated field
              Building building = this.m_BuildingData[source];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (this.m_SubLanes.HasBuffer(building.m_RoadEdge) && this.FindParkingSpace(comparePosition, this.m_SubLanes[building.m_RoadEdge], ref random, out lane, out curvePos))
                goto label_13;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_OwnerData.HasComponent(source))
            {
              // ISSUE: reference to a compiler-generated field
              source = this.m_OwnerData[source].m_Owner;
            }
            else
              goto label_16;
          }
          else
            break;
        }
        return true;
label_13:
        return true;
label_16:
        lane = Entity.Null;
        curvePos = 0.0f;
        return false;
      }

      private bool FindParkingSpace(
        float3 comparePosition,
        DynamicBuffer<Game.Net.SubLane> lanes,
        ref Random random,
        out Entity lane,
        out float curvePos)
      {
        lane = Entity.Null;
        curvePos = 0.0f;
        float num1 = float.MaxValue;
        for (int index = 0; index < lanes.Length; ++index)
        {
          Entity subLane = lanes[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ParkingLaneData.HasComponent(subLane))
          {
            float t;
            // ISSUE: reference to a compiler-generated field
            float num2 = MathUtils.Distance(this.m_CurveData[subLane].m_Bezier, comparePosition, out t);
            if ((double) num2 < (double) num1)
            {
              num1 = num2;
              curvePos = t;
              lane = subLane;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectionLaneData.HasComponent(subLane) && (this.m_ConnectionLaneData[subLane].m_Flags & ConnectionLaneFlags.Parking) != (ConnectionLaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              float num3 = MathUtils.Distance(this.m_CurveData[subLane].m_Bezier, comparePosition, out float _);
              if ((double) num3 < (double) num1)
              {
                num1 = num3;
                curvePos = random.NextFloat(0.0f, 1f);
                lane = subLane;
              }
            }
          }
        }
        curvePos = math.clamp(curvePos, 0.05f, 0.95f);
        curvePos = random.NextFloat(math.max(0.05f, curvePos - 0.2f), math.min(0.95f, curvePos + 0.2f));
        return lane != Entity.Null;
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
      public ComponentTypeHandle<Train> __Game_Vehicles_Train_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TripSource> __Game_Objects_TripSource_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> __Game_Objects_Unspawned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Helicopter> __Game_Vehicles_Helicopter_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferTypeHandle;
      public ComponentTypeHandle<CarNavigation> __Game_Vehicles_CarNavigation_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<WatercraftNavigation> __Game_Vehicles_WatercraftNavigation_RW_ComponentTypeHandle;
      public ComponentTypeHandle<WatercraftCurrentLane> __Game_Vehicles_WatercraftCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<AircraftNavigation> __Game_Vehicles_AircraftNavigation_RW_ComponentTypeHandle;
      public ComponentTypeHandle<AircraftCurrentLane> __Game_Vehicles_AircraftCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<ParkedCar> __Game_Vehicles_ParkedCar_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public BufferTypeHandle<PathElement> __Game_Pathfind_PathElement_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Train> __Game_Vehicles_Train_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<DeliveryTruck> __Game_Vehicles_DeliveryTruck_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainData> __Game_Prefabs_TrainData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarTractorData> __Game_Prefabs_CarTractorData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarTrailerData> __Game_Prefabs_CarTrailerData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SpawnLocationElement> __Game_Buildings_SpawnLocationElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      public ComponentLookup<Transform> __Game_Objects_Transform_RW_ComponentLookup;
      public ComponentLookup<TrainCurrentLane> __Game_Vehicles_TrainCurrentLane_RW_ComponentLookup;
      public ComponentLookup<ParkedTrain> __Game_Vehicles_ParkedTrain_RW_ComponentLookup;
      public ComponentLookup<TrainNavigation> __Game_Vehicles_TrainNavigation_RW_ComponentLookup;
      public ComponentLookup<CarTrailerLane> __Game_Vehicles_CarTrailerLane_RW_ComponentLookup;
      public BufferLookup<TrainBogieFrame> __Game_Vehicles_TrainBogieFrame_RW_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CarTrailerLane> __Game_Vehicles_CarTrailerLane_RO_ComponentTypeHandle;
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Train_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Train>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TripSource_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TripSource>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Helicopter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Helicopter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarNavigation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarNavigation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftNavigation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WatercraftNavigation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WatercraftCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftNavigation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AircraftNavigation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AircraftCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ParkedCar>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Train_RO_ComponentLookup = state.GetComponentLookup<Train>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup = state.GetComponentLookup<DeliveryTruck>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrainData_RO_ComponentLookup = state.GetComponentLookup<TrainData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarTractorData_RO_ComponentLookup = state.GetComponentLookup<CarTractorData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarTrailerData_RO_ComponentLookup = state.GetComponentLookup<CarTrailerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_SpawnLocationElement_RO_BufferLookup = state.GetBufferLookup<SpawnLocationElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RW_ComponentLookup = state.GetComponentLookup<Transform>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainCurrentLane_RW_ComponentLookup = state.GetComponentLookup<TrainCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedTrain_RW_ComponentLookup = state.GetComponentLookup<ParkedTrain>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainNavigation_RW_ComponentLookup = state.GetComponentLookup<TrainNavigation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarTrailerLane_RW_ComponentLookup = state.GetComponentLookup<CarTrailerLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainBogieFrame_RW_BufferLookup = state.GetBufferLookup<TrainBogieFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CarCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarTrailerLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CarTrailerLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RW_BufferLookup = state.GetBufferLookup<LaneObject>();
      }
    }
  }
}
