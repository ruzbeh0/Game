// Decompiled with JetBrains decompiler
// Type: Game.Routes.SegmentCurveSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Routes
{
  [CompilerGenerated]
  public class SegmentCurveSystem : GameSystemBase
  {
    private EntityQuery m_UpdatedRoutesQuery;
    private EntityQuery m_AllRoutesQuery;
    private bool m_Loaded;
    private SegmentCurveSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedRoutesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Segment>(),
          ComponentType.ReadOnly<CurveElement>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<LivePath>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Common.Event>(),
          ComponentType.ReadOnly<PathUpdated>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AllRoutesQuery = this.GetEntityQuery(ComponentType.ReadOnly<Segment>(), ComponentType.ReadOnly<CurveElement>());
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery entityQuery = this.GetLoaded() ? this.m_AllRoutesQuery : this.m_UpdatedRoutesQuery;
      if (entityQuery.IsEmptyIgnoreFilter)
        return;
      NativeList<Entity> nativeList = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle outJobHandle;
      NativeList<ArchetypeChunk> archetypeChunkListAsync = entityQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_CurveElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SegmentCurveSystem.FindUpdatedSegmentsJob jobData1 = new SegmentCurveSystem.FindUpdatedSegmentsJob()
      {
        m_Chunks = archetypeChunkListAsync,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PathUpdatedType = this.__TypeHandle.__Game_Pathfind_PathUpdated_RO_ComponentTypeHandle,
        m_CurveElements = this.__TypeHandle.__Game_Routes_CurveElement_RO_BufferLookup,
        m_SegmentList = nativeList
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_CurveSource_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_CurveElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainNavigationLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftNavigationLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftNavigationLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Aircraft_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PersonalCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Taxi_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_PathSource_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_PathTargets_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Segment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      SegmentCurveSystem.UpdateSegmentCurvesJob jobData2 = new SegmentCurveSystem.UpdateSegmentCurvesJob()
      {
        m_SegmentList = nativeList.AsDeferredJobArray(),
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_SegmentData = this.__TypeHandle.__Game_Routes_Segment_RO_ComponentLookup,
        m_PositionData = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
        m_TransportStopData = this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentLookup,
        m_PathTargetsData = this.__TypeHandle.__Game_Routes_PathTargets_RO_ComponentLookup,
        m_PathSourceData = this.__TypeHandle.__Game_Routes_PathSource_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_PedestrianLaneData = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_PathOwnerData = this.__TypeHandle.__Game_Pathfind_PathOwner_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_CarCurrentLaneData = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup,
        m_WatercraftCurrentLaneData = this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentLookup,
        m_AircraftCurrentLaneData = this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RO_ComponentLookup,
        m_TrainCurrentLaneData = this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup,
        m_PublicTransportData = this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup,
        m_TaxiData = this.__TypeHandle.__Game_Vehicles_Taxi_RO_ComponentLookup,
        m_PersonalCarData = this.__TypeHandle.__Game_Vehicles_PersonalCar_RO_ComponentLookup,
        m_AircraftData = this.__TypeHandle.__Game_Vehicles_Aircraft_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_GroupMemberData = this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentLookup,
        m_HumanCurrentLaneData = this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabRouteData = this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup,
        m_RouteWaypoints = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup,
        m_CarNavigationLanes = this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RO_BufferLookup,
        m_WatercraftNavigationLanes = this.__TypeHandle.__Game_Vehicles_WatercraftNavigationLane_RO_BufferLookup,
        m_AircraftNavigationLanes = this.__TypeHandle.__Game_Vehicles_AircraftNavigationLane_RO_BufferLookup,
        m_TrainNavigationLanes = this.__TypeHandle.__Game_Vehicles_TrainNavigationLane_RO_BufferLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_CurveElements = this.__TypeHandle.__Game_Routes_CurveElement_RW_BufferLookup,
        m_CurveSources = this.__TypeHandle.__Game_Routes_CurveSource_RW_BufferLookup
      };
      JobHandle jobHandle = jobData1.Schedule<SegmentCurveSystem.FindUpdatedSegmentsJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      NativeList<Entity> list = nativeList;
      JobHandle dependsOn = jobHandle;
      JobHandle inputDeps = jobData2.Schedule<SegmentCurveSystem.UpdateSegmentCurvesJob, Entity>(list, 1, dependsOn);
      nativeList.Dispose(inputDeps);
      archetypeChunkListAsync.Dispose(inputDeps);
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
    public SegmentCurveSystem()
    {
    }

    [BurstCompile]
    private struct FindUpdatedSegmentsJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PathUpdated> m_PathUpdatedType;
      [ReadOnly]
      public BufferLookup<CurveElement> m_CurveElements;
      public NativeList<Entity> m_SegmentList;

      public void Execute()
      {
        int initialCapacity = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          initialCapacity += this.m_Chunks[index].Count;
        }
        NativeHashSet<Entity> nativeHashSet = new NativeHashSet<Entity>(initialCapacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        this.m_SegmentList.Capacity = initialCapacity;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<PathUpdated> nativeArray1 = chunk.GetNativeArray<PathUpdated>(ref this.m_PathUpdatedType);
          if (nativeArray1.Length != 0)
          {
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              PathUpdated pathUpdated = nativeArray1[index2];
              // ISSUE: reference to a compiler-generated field
              if (this.m_CurveElements.HasBuffer(pathUpdated.m_Owner) && nativeHashSet.Add(pathUpdated.m_Owner))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_SegmentList.Add(in pathUpdated.m_Owner);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.m_EntityType);
            for (int index3 = 0; index3 < nativeArray2.Length; ++index3)
            {
              Entity entity = nativeArray2[index3];
              if (nativeHashSet.Add(entity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_SegmentList.Add(in entity);
              }
            }
          }
        }
        nativeHashSet.Dispose();
      }
    }

    [BurstCompile]
    private struct UpdateSegmentCurvesJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Entity> m_SegmentList;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Segment> m_SegmentData;
      [ReadOnly]
      public ComponentLookup<Position> m_PositionData;
      [ReadOnly]
      public ComponentLookup<TransportStop> m_TransportStopData;
      [ReadOnly]
      public ComponentLookup<PathTargets> m_PathTargetsData;
      [ReadOnly]
      public ComponentLookup<PathSource> m_PathSourceData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> m_PedestrianLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<PathOwner> m_PathOwnerData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<CarCurrentLane> m_CarCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<WatercraftCurrentLane> m_WatercraftCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<AircraftCurrentLane> m_AircraftCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<TrainCurrentLane> m_TrainCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Taxi> m_TaxiData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PersonalCar> m_PersonalCarData;
      [ReadOnly]
      public ComponentLookup<Aircraft> m_AircraftData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public ComponentLookup<GroupMember> m_GroupMemberData;
      [ReadOnly]
      public ComponentLookup<HumanCurrentLane> m_HumanCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<RouteData> m_PrefabRouteData;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_RouteWaypoints;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public BufferLookup<CarNavigationLane> m_CarNavigationLanes;
      [ReadOnly]
      public BufferLookup<WatercraftNavigationLane> m_WatercraftNavigationLanes;
      [ReadOnly]
      public BufferLookup<AircraftNavigationLane> m_AircraftNavigationLanes;
      [ReadOnly]
      public BufferLookup<TrainNavigationLane> m_TrainNavigationLanes;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [NativeDisableParallelForRestriction]
      public BufferLookup<CurveElement> m_CurveElements;
      [NativeDisableParallelForRestriction]
      public BufferLookup<CurveSource> m_CurveSources;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity segment1 = this.m_SegmentList[index];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CurveElement> curveElement = this.m_CurveElements[segment1];
        DynamicBuffer<CurveSource> bufferData1;
        // ISSUE: reference to a compiler-generated field
        this.m_CurveSources.TryGetBuffer(segment1, out bufferData1);
        Owner componentData1;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_OwnerData.TryGetComponent(segment1, out componentData1))
        {
          curveElement.Clear();
          if (!bufferData1.IsCreated)
            return;
          bufferData1.Clear();
        }
        else
        {
          Entity owner = componentData1.m_Owner;
          // ISSUE: reference to a compiler-generated field
          Segment segment2 = this.m_SegmentData[segment1];
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[owner];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<RouteWaypoint> routeWaypoint = this.m_RouteWaypoints[owner];
          // ISSUE: reference to a compiler-generated field
          RouteData routeData = this.m_PrefabRouteData[prefabRef.m_Prefab];
          float nodeDistance = routeData.m_Width * 2.5f;
          float segmentLength = routeData.m_SegmentLength;
          float3 lastPosition1 = new float3();
          float3 nextNodePos = new float3();
          float3 lastTangent = new float3();
          bool isFirst = true;
          bool airway = false;
          bool area = false;
          bool hasLastPos1 = false;
          bool hasNextPos1 = false;
          int connectionCount = 0;
          if (routeWaypoint.Length >= 2)
          {
            Entity waypoint1 = routeWaypoint[segment2.m_Index].m_Waypoint;
            Entity waypoint2 = routeWaypoint[math.select(segment2.m_Index + 1, 0, segment2.m_Index + 1 >= routeWaypoint.Length)].m_Waypoint;
            // ISSUE: reference to a compiler-generated field
            float3 lastPosition2 = this.m_PositionData[waypoint1].m_Position;
            // ISSUE: reference to a compiler-generated field
            float3 float3 = this.m_PositionData[waypoint2].m_Position;
            PathTargets componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PathTargetsData.TryGetComponent(segment1, out componentData2))
            {
              lastPosition2 = componentData2.m_ReadyStartPosition;
              float3 = componentData2.m_ReadyEndPosition;
            }
            bool hasLastPos2 = true;
            bool hasNextPos2 = true;
            curveElement.Clear();
            DynamicBuffer<PathElement> bufferData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PathElements.TryGetBuffer(segment1, out bufferData2))
            {
              // ISSUE: reference to a compiler-generated method
              this.TryAddSegments(curveElement, bufferData1, bufferData2, new PathOwner(), lastPosition2, float3, nodeDistance, segmentLength, ref lastPosition2, ref lastTangent, false, true, false, ref isFirst, ref airway, ref area, ref hasLastPos2, ref hasNextPos2, ref connectionCount);
            }
            // ISSUE: reference to a compiler-generated method
            this.TryAddSegments(curveElement, bufferData1, float3, new float3(), nodeDistance, segmentLength, ref lastPosition2, ref lastTangent, ref airway, ref area);
          }
          else
          {
            PathSource componentData3;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PathSourceData.TryGetComponent(segment1, out componentData3))
              return;
            bool flag1 = false;
            bool flag2 = false;
            bool isPedestrian = false;
            bool skipAirway = true;
            bool stayMidAir = false;
            // ISSUE: reference to a compiler-generated field
            this.m_UpdatedData.HasComponent(segment1);
            curveElement.Clear();
            bufferData1.Clear();
            GroupMember componentData4;
            // ISSUE: reference to a compiler-generated field
            this.m_GroupMemberData.TryGetComponent(componentData3.m_Entity, out componentData4);
            CarCurrentLane componentData5;
            DynamicBuffer<CarNavigationLane> bufferData3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_CarCurrentLaneData.TryGetComponent(componentData3.m_Entity, out componentData5) && this.m_CarNavigationLanes.TryGetBuffer(componentData3.m_Entity, out bufferData3))
            {
              // ISSUE: reference to a compiler-generated method
              flag1 = this.TryAddSegments(curveElement, bufferData1, componentData5, bufferData3, lastPosition1, nextNodePos, nodeDistance, segmentLength, ref lastPosition1, ref lastTangent, ref isFirst, ref airway, ref area, ref hasLastPos1, ref hasNextPos1, ref connectionCount);
            }
            else
            {
              WatercraftCurrentLane componentData6;
              DynamicBuffer<WatercraftNavigationLane> bufferData4;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_WatercraftCurrentLaneData.TryGetComponent(componentData3.m_Entity, out componentData6) && this.m_WatercraftNavigationLanes.TryGetBuffer(componentData3.m_Entity, out bufferData4))
              {
                // ISSUE: reference to a compiler-generated method
                flag1 = this.TryAddSegments(curveElement, bufferData1, componentData6, bufferData4, lastPosition1, nextNodePos, nodeDistance, segmentLength, ref lastPosition1, ref lastTangent, ref isFirst, ref airway, ref area, ref hasLastPos1, ref hasNextPos1, ref connectionCount);
              }
              else
              {
                AircraftCurrentLane componentData7;
                DynamicBuffer<AircraftNavigationLane> bufferData5;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_AircraftCurrentLaneData.TryGetComponent(componentData3.m_Entity, out componentData7) && this.m_AircraftNavigationLanes.TryGetBuffer(componentData3.m_Entity, out bufferData5))
                {
                  airway = (componentData7.m_LaneFlags & AircraftLaneFlags.Flying) > (AircraftLaneFlags) 0;
                  skipAirway = false;
                  // ISSUE: reference to a compiler-generated field
                  stayMidAir = (this.m_AircraftData[componentData3.m_Entity].m_Flags & AircraftFlags.StayMidAir) > (AircraftFlags) 0;
                  // ISSUE: reference to a compiler-generated method
                  flag1 = this.TryAddSegments(curveElement, bufferData1, componentData7, bufferData5, lastPosition1, nextNodePos, nodeDistance, segmentLength, stayMidAir, ref lastPosition1, ref lastTangent, ref isFirst, ref airway, ref area, ref hasLastPos1, ref hasNextPos1, ref connectionCount);
                }
                else
                {
                  DynamicBuffer<TrainNavigationLane> bufferData6;
                  DynamicBuffer<LayoutElement> bufferData7;
                  TrainCurrentLane componentData8;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_TrainNavigationLanes.TryGetBuffer(componentData3.m_Entity, out bufferData6) && this.m_LayoutElements.TryGetBuffer(componentData3.m_Entity, out bufferData7) && bufferData7.Length != 0 && this.m_TrainCurrentLaneData.TryGetComponent(bufferData7[0].m_Vehicle, out componentData8))
                  {
                    // ISSUE: reference to a compiler-generated method
                    flag1 = this.TryAddSegments(curveElement, bufferData1, componentData8, bufferData6, lastPosition1, nextNodePos, nodeDistance, segmentLength, ref lastPosition1, ref lastTangent, ref isFirst, ref airway, ref area, ref hasLastPos1, ref hasNextPos1, ref connectionCount);
                  }
                  else
                  {
                    HumanCurrentLane componentData9;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_HumanCurrentLaneData.TryGetComponent(componentData3.m_Entity, out componentData9))
                    {
                      // ISSUE: reference to a compiler-generated method
                      flag1 = this.TryAddSegments(curveElement, bufferData1, componentData9, lastPosition1, nextNodePos, nodeDistance, segmentLength, ref lastPosition1, ref lastTangent, ref isFirst, ref airway, ref area, ref hasLastPos1, ref hasNextPos1, ref connectionCount);
                      isPedestrian = true;
                    }
                    else
                    {
                      CurrentVehicle componentData10;
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_CurrentVehicleData.TryGetComponent(componentData3.m_Entity, out componentData10))
                      {
                        Controller componentData11;
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_ControllerData.TryGetComponent(componentData10.m_Vehicle, out componentData11) && componentData11.m_Controller != Entity.Null)
                          componentData10.m_Vehicle = componentData11.m_Controller;
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_TaxiData.HasComponent(componentData10.m_Vehicle) || this.m_PersonalCarData.HasComponent(componentData10.m_Vehicle))
                        {
                          componentData3.m_Entity = componentData10.m_Vehicle;
                          flag2 = true;
                        }
                        else
                        {
                          // ISSUE: reference to a compiler-generated field
                          if (this.m_PublicTransportData.HasComponent(componentData10.m_Vehicle))
                            flag2 = true;
                          else
                            flag1 = true;
                        }
                        isPedestrian = true;
                      }
                    }
                  }
                }
              }
            }
            PathElement pathElement1 = new PathElement();
            DynamicBuffer<PathElement> bufferData8;
            // ISSUE: reference to a compiler-generated field
            if (!flag1 && this.m_PathElements.TryGetBuffer(componentData3.m_Entity, out bufferData8))
            {
              PathOwner componentData12;
              // ISSUE: reference to a compiler-generated field
              this.m_PathOwnerData.TryGetComponent(componentData3.m_Entity, out componentData12);
              if (flag2)
              {
                // ISSUE: reference to a compiler-generated method
                while (componentData12.m_ElementIndex < bufferData8.Length && !this.IsPedestrianTarget(bufferData8[componentData12.m_ElementIndex].m_Target))
                  ++componentData12.m_ElementIndex;
              }
              else if (componentData12.m_ElementIndex < bufferData8.Length)
                pathElement1 = bufferData8[bufferData8.Length - 1];
              // ISSUE: reference to a compiler-generated method
              this.TryAddSegments(curveElement, bufferData1, bufferData8, componentData12, lastPosition1, nextNodePos, nodeDistance, segmentLength, ref lastPosition1, ref lastTangent, isPedestrian, skipAirway, stayMidAir, ref isFirst, ref airway, ref area, ref hasLastPos1, ref hasNextPos1, ref connectionCount);
            }
            // ISSUE: reference to a compiler-generated field
            if (!flag1 && this.m_PathElements.TryGetBuffer(componentData4.m_Leader, out bufferData8))
            {
              PathOwner componentData13;
              // ISSUE: reference to a compiler-generated field
              this.m_PathOwnerData.TryGetComponent(componentData4.m_Leader, out componentData13);
              if (flag2)
              {
                // ISSUE: reference to a compiler-generated method
                while (componentData13.m_ElementIndex < bufferData8.Length && !this.IsPedestrianTarget(bufferData8[componentData13.m_ElementIndex].m_Target))
                  ++componentData13.m_ElementIndex;
              }
              else if (pathElement1.m_Target != Entity.Null)
              {
                for (int elementIndex = componentData13.m_ElementIndex; elementIndex < bufferData8.Length; ++elementIndex)
                {
                  PathElement pathElement2 = bufferData8[elementIndex];
                  if (pathElement2.m_Target == pathElement1.m_Target && pathElement2.m_TargetDelta.Equals(pathElement1.m_TargetDelta))
                  {
                    componentData13.m_ElementIndex = elementIndex + 1;
                    break;
                  }
                }
              }
              // ISSUE: reference to a compiler-generated method
              this.TryAddSegments(curveElement, bufferData1, bufferData8, componentData13, lastPosition1, nextNodePos, nodeDistance, segmentLength, ref lastPosition1, ref lastTangent, isPedestrian, skipAirway, stayMidAir, ref isFirst, ref airway, ref area, ref hasLastPos1, ref hasNextPos1, ref connectionCount);
            }
            if (connectionCount != 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.ProcessConnectionSegments(curveElement, bufferData1, nodeDistance, segmentLength, ref lastPosition1, ref lastTangent, ref airway, ref area, ref hasLastPos1, ref hasNextPos1, ref connectionCount);
            }
            if (!(airway & hasLastPos1) || curveElement.Length != 0)
              return;
            curveElement.Add(new CurveElement()
            {
              m_Curve = new Bezier4x3(lastPosition1, lastPosition1, lastPosition1, lastPosition1)
            });
            if (!bufferData1.IsCreated)
              return;
            bufferData1.Add(new CurveSource());
          }
        }
      }

      private bool TryAddSegments(
        DynamicBuffer<CurveElement> curveElements,
        DynamicBuffer<CurveSource> curveSources,
        CarCurrentLane carCurrentLane,
        DynamicBuffer<CarNavigationLane> navLanes,
        float3 lastNodePos,
        float3 nextNodePos,
        float nodeDistance,
        float segmentLength,
        ref float3 lastPosition,
        ref float3 lastTangent,
        ref bool isFirst,
        ref bool airway,
        ref bool area,
        ref bool hasLastPos,
        ref bool hasNextPos,
        ref int connectionCount)
      {
        int index1 = 0;
        bool airway1 = false;
        bool area1 = false;
        bool flag = false;
        if (hasNextPos)
        {
          index1 = navLanes.Length;
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          if (this.ShouldEndPath(carCurrentLane.m_Lane, false))
            return true;
          for (; index1 < navLanes.Length; ++index1)
          {
            // ISSUE: reference to a compiler-generated method
            if (this.ShouldEndPath(navLanes[index1].m_Lane, false))
            {
              flag = true;
              break;
            }
          }
        }
        int num;
        for (int index2 = num = index1 - 1; index2 >= 0; --index2)
        {
          CarNavigationLane navLane = navLanes[index2];
          // ISSUE: reference to a compiler-generated method
          this.GetMasterLane(ref navLane.m_Lane);
          // ISSUE: reference to a compiler-generated method
          if (this.IsLast(navLane.m_Lane, navLane.m_CurvePosition, nextNodePos, nodeDistance, hasNextPos, true, ref airway1, ref area1))
          {
            num = index2;
            break;
          }
        }
        // ISSUE: reference to a compiler-generated method
        if (num == -1 && !this.IsLast(carCurrentLane.m_Lane, carCurrentLane.m_CurvePosition.xz, nextNodePos, nodeDistance, hasNextPos, true, ref airway1, ref area1))
          num = -2;
        if (num >= -1)
        {
          // ISSUE: reference to a compiler-generated method
          this.GetMasterLane(ref carCurrentLane.m_Lane);
          // ISSUE: reference to a compiler-generated method
          this.TryAddSegments(curveElements, curveSources, lastNodePos, nextNodePos, nodeDistance, segmentLength, carCurrentLane.m_Lane, carCurrentLane.m_CurvePosition.xz, num == -1, true, false, ref isFirst, ref lastPosition, ref lastTangent, ref airway, ref area, ref hasLastPos, ref hasNextPos, ref connectionCount);
        }
        for (int index3 = 0; index3 <= num; ++index3)
        {
          CarNavigationLane navLane = navLanes[index3];
          // ISSUE: reference to a compiler-generated method
          this.GetMasterLane(ref navLane.m_Lane);
          // ISSUE: reference to a compiler-generated method
          this.TryAddSegments(curveElements, curveSources, lastNodePos, nextNodePos, nodeDistance, segmentLength, navLane.m_Lane, navLane.m_CurvePosition, index3 == num, true, false, ref isFirst, ref lastPosition, ref lastTangent, ref airway, ref area, ref hasLastPos, ref hasNextPos, ref connectionCount);
        }
        airway |= airway1;
        area |= area1;
        return flag;
      }

      private bool TryAddSegments(
        DynamicBuffer<CurveElement> curveElements,
        DynamicBuffer<CurveSource> curveSources,
        WatercraftCurrentLane watercraftCurrentLane,
        DynamicBuffer<WatercraftNavigationLane> navLanes,
        float3 lastNodePos,
        float3 nextNodePos,
        float nodeDistance,
        float segmentLength,
        ref float3 lastPosition,
        ref float3 lastTangent,
        ref bool isFirst,
        ref bool airway,
        ref bool area,
        ref bool hasLastPos,
        ref bool hasNextPos,
        ref int connectionCount)
      {
        int index1 = 0;
        bool airway1 = false;
        bool area1 = false;
        bool flag = false;
        if (hasNextPos)
        {
          index1 = navLanes.Length;
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          if (this.ShouldEndPath(watercraftCurrentLane.m_Lane, false))
            return true;
          for (; index1 < navLanes.Length; ++index1)
          {
            // ISSUE: reference to a compiler-generated method
            if (this.ShouldEndPath(navLanes[index1].m_Lane, false))
            {
              flag = true;
              break;
            }
          }
        }
        int num;
        for (int index2 = num = index1 - 1; index2 >= 0; --index2)
        {
          WatercraftNavigationLane navLane = navLanes[index2];
          // ISSUE: reference to a compiler-generated method
          this.GetMasterLane(ref navLane.m_Lane);
          // ISSUE: reference to a compiler-generated method
          if (this.IsLast(navLane.m_Lane, navLane.m_CurvePosition, nextNodePos, nodeDistance, hasNextPos, true, ref airway1, ref area1))
          {
            num = index2;
            break;
          }
        }
        // ISSUE: reference to a compiler-generated method
        if (num == -1 && !this.IsLast(watercraftCurrentLane.m_Lane, watercraftCurrentLane.m_CurvePosition.xz, nextNodePos, nodeDistance, hasNextPos, true, ref airway1, ref area1))
          num = -2;
        if (num >= -1)
        {
          // ISSUE: reference to a compiler-generated method
          this.GetMasterLane(ref watercraftCurrentLane.m_Lane);
          // ISSUE: reference to a compiler-generated method
          this.TryAddSegments(curveElements, curveSources, lastNodePos, nextNodePos, nodeDistance, segmentLength, watercraftCurrentLane.m_Lane, watercraftCurrentLane.m_CurvePosition.xz, num == -1, true, false, ref isFirst, ref lastPosition, ref lastTangent, ref airway, ref area, ref hasLastPos, ref hasNextPos, ref connectionCount);
        }
        for (int index3 = 0; index3 <= num; ++index3)
        {
          WatercraftNavigationLane navLane = navLanes[index3];
          // ISSUE: reference to a compiler-generated method
          this.GetMasterLane(ref navLane.m_Lane);
          // ISSUE: reference to a compiler-generated method
          this.TryAddSegments(curveElements, curveSources, lastNodePos, nextNodePos, nodeDistance, segmentLength, navLane.m_Lane, navLane.m_CurvePosition, index3 == num, true, false, ref isFirst, ref lastPosition, ref lastTangent, ref airway, ref area, ref hasLastPos, ref hasNextPos, ref connectionCount);
        }
        airway |= airway1;
        area |= area1;
        return flag;
      }

      private bool TryAddSegments(
        DynamicBuffer<CurveElement> curveElements,
        DynamicBuffer<CurveSource> curveSources,
        AircraftCurrentLane aircraftCurrentLane,
        DynamicBuffer<AircraftNavigationLane> navLanes,
        float3 lastNodePos,
        float3 nextNodePos,
        float nodeDistance,
        float segmentLength,
        bool stayMidAir,
        ref float3 lastPosition,
        ref float3 lastTangent,
        ref bool isFirst,
        ref bool airway,
        ref bool area,
        ref bool hasLastPos,
        ref bool hasNextPos,
        ref int connectionCount)
      {
        int index1 = 0;
        bool flag1 = false;
        bool area1 = false;
        bool airway1 = false;
        bool flag2 = false;
        if (hasNextPos)
        {
          index1 = navLanes.Length;
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          if (this.ShouldEndPath(aircraftCurrentLane.m_Lane, false))
            return true;
          for (; index1 < navLanes.Length; ++index1)
          {
            // ISSUE: reference to a compiler-generated method
            if (this.ShouldEndPath(navLanes[index1].m_Lane, false))
            {
              flag2 = true;
              break;
            }
          }
        }
        int num;
        for (int index2 = num = index1 - 1; index2 >= 0; --index2)
        {
          AircraftNavigationLane navLane = navLanes[index2];
          airway1 = false;
          // ISSUE: reference to a compiler-generated method
          if (this.IsLast(navLane.m_Lane, navLane.m_CurvePosition, nextNodePos, nodeDistance, hasNextPos, false, ref airway1, ref area1))
          {
            num = index2;
            break;
          }
          flag1 |= airway1;
        }
        if (num == -1)
        {
          airway1 = false;
          // ISSUE: reference to a compiler-generated method
          if (!this.IsLast(aircraftCurrentLane.m_Lane, aircraftCurrentLane.m_CurvePosition.xz, nextNodePos, nodeDistance, hasNextPos, false, ref airway1, ref area1))
          {
            num = -2;
            flag1 |= airway1;
          }
        }
        if (num == -2)
          airway1 = airway;
        if (airway1 && num + 1 < navLanes.Length)
          ++num;
        if (num >= -1)
        {
          // ISSUE: reference to a compiler-generated method
          this.TryAddSegments(curveElements, curveSources, lastNodePos, nextNodePos, nodeDistance, segmentLength, aircraftCurrentLane.m_Lane, aircraftCurrentLane.m_CurvePosition.xz, num == -1, false, stayMidAir, ref isFirst, ref lastPosition, ref lastTangent, ref airway, ref area, ref hasLastPos, ref hasNextPos, ref connectionCount);
        }
        for (int index3 = 0; index3 <= num; ++index3)
        {
          AircraftNavigationLane navLane = navLanes[index3];
          // ISSUE: reference to a compiler-generated method
          this.TryAddSegments(curveElements, curveSources, lastNodePos, nextNodePos, nodeDistance, segmentLength, navLane.m_Lane, navLane.m_CurvePosition, index3 == num, false, stayMidAir, ref isFirst, ref lastPosition, ref lastTangent, ref airway, ref area, ref hasLastPos, ref hasNextPos, ref connectionCount);
        }
        airway |= flag1;
        area |= area1;
        return flag2;
      }

      private bool TryAddSegments(
        DynamicBuffer<CurveElement> curveElements,
        DynamicBuffer<CurveSource> curveSources,
        TrainCurrentLane trainCurrentLane,
        DynamicBuffer<TrainNavigationLane> navLanes,
        float3 lastNodePos,
        float3 nextNodePos,
        float nodeDistance,
        float segmentLength,
        ref float3 lastPosition,
        ref float3 lastTangent,
        ref bool isFirst,
        ref bool airway,
        ref bool area,
        ref bool hasLastPos,
        ref bool hasNextPos,
        ref int connectionCount)
      {
        int index1 = 0;
        bool airway1 = false;
        bool area1 = false;
        bool flag = false;
        if (hasNextPos)
        {
          index1 = navLanes.Length;
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          if (this.ShouldEndPath(trainCurrentLane.m_Front.m_Lane, false))
            return true;
          for (; index1 < navLanes.Length; ++index1)
          {
            // ISSUE: reference to a compiler-generated method
            if (this.ShouldEndPath(navLanes[index1].m_Lane, false))
            {
              flag = true;
              break;
            }
          }
        }
        int num;
        for (int index2 = num = index1 - 1; index2 >= 0; --index2)
        {
          TrainNavigationLane navLane = navLanes[index2];
          // ISSUE: reference to a compiler-generated method
          if (this.IsLast(navLane.m_Lane, navLane.m_CurvePosition, nextNodePos, nodeDistance, hasNextPos, true, ref airway1, ref area1))
          {
            num = index2;
            break;
          }
        }
        // ISSUE: reference to a compiler-generated method
        if (num == -1 && !this.IsLast(trainCurrentLane.m_Front.m_Lane, trainCurrentLane.m_Front.m_CurvePosition.yw, nextNodePos, nodeDistance, hasNextPos, true, ref airway1, ref area1))
          num = -2;
        if (num >= -1)
        {
          // ISSUE: reference to a compiler-generated method
          this.TryAddSegments(curveElements, curveSources, lastNodePos, nextNodePos, nodeDistance, segmentLength, trainCurrentLane.m_Front.m_Lane, trainCurrentLane.m_Front.m_CurvePosition.yw, num == -1, true, false, ref isFirst, ref lastPosition, ref lastTangent, ref airway, ref area, ref hasLastPos, ref hasNextPos, ref connectionCount);
        }
        for (int index3 = 0; index3 <= num; ++index3)
        {
          TrainNavigationLane navLane = navLanes[index3];
          // ISSUE: reference to a compiler-generated method
          this.TryAddSegments(curveElements, curveSources, lastNodePos, nextNodePos, nodeDistance, segmentLength, navLane.m_Lane, navLane.m_CurvePosition, index3 == num, true, false, ref isFirst, ref lastPosition, ref lastTangent, ref airway, ref area, ref hasLastPos, ref hasNextPos, ref connectionCount);
        }
        airway |= airway1;
        area |= area1;
        return flag;
      }

      private bool TryAddSegments(
        DynamicBuffer<CurveElement> curveElements,
        DynamicBuffer<CurveSource> curveSources,
        HumanCurrentLane humanCurrentLane,
        float3 lastNodePos,
        float3 nextNodePos,
        float nodeDistance,
        float segmentLength,
        ref float3 lastPosition,
        ref float3 lastTangent,
        ref bool isFirst,
        ref bool airway,
        ref bool area,
        ref bool hasLastPos,
        ref bool hasNextPos,
        ref int connectionCount)
      {
        // ISSUE: reference to a compiler-generated method
        if (!hasNextPos && this.ShouldEndPath(humanCurrentLane.m_Lane, true))
          return true;
        // ISSUE: reference to a compiler-generated method
        this.TryAddSegments(curveElements, curveSources, lastNodePos, nextNodePos, nodeDistance, segmentLength, humanCurrentLane.m_Lane, humanCurrentLane.m_CurvePosition, true, true, false, ref isFirst, ref lastPosition, ref lastTangent, ref airway, ref area, ref hasLastPos, ref hasNextPos, ref connectionCount);
        return false;
      }

      private void TryAddSegments(
        DynamicBuffer<CurveElement> curveElements,
        DynamicBuffer<CurveSource> curveSources,
        DynamicBuffer<PathElement> path,
        PathOwner pathOwner,
        float3 lastNodePos,
        float3 nextNodePos,
        float nodeDistance,
        float segmentLength,
        ref float3 lastPosition,
        ref float3 lastTangent,
        bool isPedestrian,
        bool skipAirway,
        bool stayMidAir,
        ref bool isFirst,
        ref bool airway,
        ref bool area,
        ref bool hasLastPos,
        ref bool hasNextPos,
        ref int connectionCount)
      {
        int index1 = pathOwner.m_ElementIndex;
        bool flag = false;
        bool area1 = false;
        bool airway1 = false;
        if (hasNextPos)
        {
          index1 = path.Length;
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          while (index1 < path.Length && !this.ShouldEndPath(path[index1].m_Target, isPedestrian))
            ++index1;
        }
        int num;
        for (int index2 = num = index1 - 1; index2 >= pathOwner.m_ElementIndex; --index2)
        {
          PathElement pathElement = path[index2];
          airway1 = false;
          // ISSUE: reference to a compiler-generated method
          if (this.IsLast(pathElement.m_Target, pathElement.m_TargetDelta, nextNodePos, nodeDistance, hasNextPos, skipAirway, ref airway1, ref area1))
          {
            num = index2;
            break;
          }
          flag |= airway1;
        }
        if (num == pathOwner.m_ElementIndex - 1)
          airway1 = airway;
        if (!skipAirway & airway1 && num + 1 < path.Length)
          ++num;
        for (int elementIndex = pathOwner.m_ElementIndex; elementIndex <= num; ++elementIndex)
        {
          PathElement pathElement = path[elementIndex];
          // ISSUE: reference to a compiler-generated method
          this.TryAddSegments(curveElements, curveSources, lastNodePos, nextNodePos, nodeDistance, segmentLength, pathElement.m_Target, pathElement.m_TargetDelta, elementIndex == num, skipAirway, stayMidAir, ref isFirst, ref lastPosition, ref lastTangent, ref airway, ref area, ref hasLastPos, ref hasNextPos, ref connectionCount);
        }
        airway |= flag;
        area |= area1;
      }

      private bool IsLast(
        Entity target,
        float2 targetDelta,
        float3 nextNodePos,
        float nodeDistance,
        bool hasNextPos,
        bool skipAirway,
        ref bool airway,
        ref bool area)
      {
        // ISSUE: reference to a compiler-generated method
        if (this.ShouldSkipTarget(target, skipAirway, ref airway, ref area))
          return false;
        // ISSUE: reference to a compiler-generated field
        Bezier4x3 bezier4x3 = MathUtils.Cut(this.m_CurveData[target].m_Bezier, targetDelta);
        return !hasNextPos || (double) math.distance(nextNodePos, bezier4x3.a) > (double) nodeDistance;
      }

      private void TryAddSegments(
        DynamicBuffer<CurveElement> curveElements,
        DynamicBuffer<CurveSource> curveSources,
        float3 lastNodePos,
        float3 nextNodePos,
        float nodeDistance,
        float segmentLength,
        Entity target,
        float2 targetDelta,
        bool isLast,
        bool skipAirway,
        bool stayMidAir,
        ref bool isFirst,
        ref float3 lastPosition,
        ref float3 lastTangent,
        ref bool airway,
        ref bool area,
        ref bool hasLastPos,
        ref bool hasNextPos,
        ref int connectionCount)
      {
        bool airway1 = false;
        bool area1 = false;
        // ISSUE: reference to a compiler-generated method
        if (this.ShouldSkipTarget(target, skipAirway, ref airway1, ref area1))
        {
          if (!skipAirway & airway)
          {
            Game.Net.ConnectionLane componentData1;
            Owner componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectionLaneData.TryGetComponent(target, out componentData1) && (componentData1.m_Flags & (ConnectionLaneFlags.Start | ConnectionLaneFlags.Outside)) == (ConnectionLaneFlags.Start | ConnectionLaneFlags.Outside) && this.m_OwnerData.TryGetComponent(target, out componentData2))
              target = componentData2.m_Owner;
            Game.Objects.Transform componentData3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransformData.TryGetComponent(target, out componentData3))
            {
              if (connectionCount != 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.ProcessConnectionSegments(curveElements, curveSources, nodeDistance, segmentLength, ref lastPosition, ref lastTangent, ref airway, ref area, ref hasLastPos, ref hasNextPos, ref connectionCount);
              }
              if (stayMidAir)
                componentData3.m_Position.y += 100f;
              if (hasLastPos)
              {
                airway = false;
                area = false;
                // ISSUE: reference to a compiler-generated method
                this.TryAddSegments(curveElements, curveSources, componentData3.m_Position, new float3(), nodeDistance, segmentLength, ref lastPosition, ref lastTangent, ref airway, ref area);
              }
              else
              {
                lastPosition = componentData3.m_Position;
                lastTangent = new float3();
                hasLastPos = true;
              }
            }
          }
          airway |= airway1;
          area |= area1;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          Curve curve1 = this.m_CurveData[target];
          Bezier4x3 curve2 = MathUtils.Cut(curve1.m_Bezier, targetDelta);
          if (airway1 | area1)
          {
            if (airway1 && !hasLastPos && connectionCount == 0)
              curve2 = (double) targetDelta.y < (double) targetDelta.x || (double) targetDelta.y == 0.0 ? MathUtils.Invert(curve1.m_Bezier) : curve1.m_Bezier;
            curveElements.Add(new CurveElement()
            {
              m_Curve = curve2
            });
            if (curveSources.IsCreated)
              curveSources.Add(new CurveSource());
            ++connectionCount;
            airway |= airway1;
            area |= area1;
          }
          else
          {
            if (connectionCount != 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.ProcessConnectionSegments(curveElements, curveSources, nodeDistance, segmentLength, ref lastPosition, ref lastTangent, ref airway, ref area, ref hasLastPos, ref hasNextPos, ref connectionCount);
              airway = false;
              area = false;
            }
            if (isFirst)
            {
              if (hasLastPos && (double) math.distance(lastNodePos, curve2.a) < (double) nodeDistance)
              {
                if ((double) math.distance(lastNodePos, curve2.d) <= (double) nodeDistance)
                  return;
                // ISSUE: reference to a compiler-generated method
                float num = this.MoveCurvePosition(lastNodePos, nodeDistance, curve2);
                curve2 = MathUtils.Cut(curve2, new float2(num, 1f));
                targetDelta.x = math.lerp(targetDelta.x, targetDelta.y, num);
              }
              isFirst = false;
            }
            if (isLast && hasNextPos && (double) math.distance(nextNodePos, curve2.d) < (double) nodeDistance)
            {
              if ((double) math.distance(nextNodePos, curve2.a) <= (double) nodeDistance)
                return;
              // ISSUE: reference to a compiler-generated method
              float s = this.MoveCurvePosition(nextNodePos, nodeDistance, MathUtils.Invert(curve2));
              curve2 = MathUtils.Cut(curve2, new float2(0.0f, 1f - s));
              targetDelta.y = math.lerp(targetDelta.y, targetDelta.x, s);
            }
            // ISSUE: reference to a compiler-generated method
            this.TryAddSegments(curveElements, curveSources, target, targetDelta, curve2, nodeDistance, segmentLength, ref lastPosition, ref lastTangent, ref airway, ref area, ref hasLastPos);
          }
        }
      }

      private void ProcessConnectionSegments(
        DynamicBuffer<CurveElement> curveElements,
        DynamicBuffer<CurveSource> curveSources,
        float nodeDistance,
        float segmentLength,
        ref float3 lastPosition,
        ref float3 lastTangent,
        ref bool airway,
        ref bool area,
        ref bool hasLastPos,
        ref bool hasNextPos,
        ref int connectionCount)
      {
        int index1 = curveElements.Length - connectionCount;
        bool airway1 = false;
        bool area1 = false;
        if (area && connectionCount == 1)
        {
          CurveElement curveElement = curveElements[index1];
          float3 position = MathUtils.Position(curveElement.m_Curve, 0.5f);
          float3 nextTangent = MathUtils.Tangent(curveElement.m_Curve, 0.5f);
          if (hasLastPos)
          {
            // ISSUE: reference to a compiler-generated method
            this.TryAddSegments(curveElements, curveSources, position, nextTangent, nodeDistance, segmentLength, ref lastPosition, ref lastTangent, ref airway1, ref area1);
          }
          else
          {
            lastPosition = position;
            lastTangent = nextTangent;
            hasLastPos = true;
          }
        }
        for (int index2 = 1; index2 < connectionCount; ++index2)
        {
          CurveElement curveElement1 = curveElements[index1 + index2 - 1];
          CurveElement curveElement2 = curveElements[index1 + index2];
          float3 position = (curveElement1.m_Curve.a + curveElement1.m_Curve.d + curveElement2.m_Curve.a + curveElement2.m_Curve.d) * 0.25f;
          float3 nextTangent = curveElement2.m_Curve.d - curveElement1.m_Curve.a;
          if (hasLastPos)
          {
            // ISSUE: reference to a compiler-generated method
            this.TryAddSegments(curveElements, curveSources, position, nextTangent, nodeDistance, segmentLength, ref lastPosition, ref lastTangent, ref airway1, ref area1);
          }
          else
          {
            lastPosition = position;
            lastTangent = nextTangent;
            hasLastPos = true;
          }
        }
        curveElements.RemoveRange(index1, connectionCount);
        curveSources.RemoveRange(index1, connectionCount);
        connectionCount = 0;
      }

      private void GetMasterLane(ref Entity lane)
      {
        SlaveLane componentData1;
        Owner componentData2;
        DynamicBuffer<Game.Net.SubLane> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SlaveLaneData.TryGetComponent(lane, out componentData1) || !this.m_OwnerData.TryGetComponent(lane, out componentData2) || !this.m_SubLanes.TryGetBuffer(componentData2.m_Owner, out bufferData) || bufferData.Length <= (int) componentData1.m_MasterIndex)
          return;
        lane = bufferData[(int) componentData1.m_MasterIndex].m_SubLane;
      }

      private bool ShouldEndPath(Entity target, bool isPedestrian)
      {
        Game.Net.ConnectionLane componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_ParkingLaneData.HasComponent(target) || (isPedestrian ? (this.m_CarLaneData.HasComponent(target) ? 1 : 0) : (this.m_PedestrianLaneData.HasComponent(target) ? 1 : 0)) != 0 || this.m_ConnectionLaneData.TryGetComponent(target, out componentData) && (componentData.m_Flags & (isPedestrian ? ConnectionLaneFlags.Road | ConnectionLaneFlags.Parking : ConnectionLaneFlags.Pedestrian | ConnectionLaneFlags.Parking)) != (ConnectionLaneFlags) 0 || this.m_PositionData.HasComponent(target) || this.m_TransportStopData.HasComponent(target);
      }

      private bool IsPedestrianTarget(Entity target)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_PedestrianLaneData.HasComponent(target))
          return true;
        Game.Net.ConnectionLane componentData;
        // ISSUE: reference to a compiler-generated field
        return this.m_ConnectionLaneData.TryGetComponent(target, out componentData) && (componentData.m_Flags & ConnectionLaneFlags.Pedestrian) != 0;
      }

      private bool ShouldSkipTarget(
        Entity target,
        bool skipAirway,
        ref bool airway,
        ref bool area)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CurveData.HasComponent(target))
          return true;
        Game.Net.CarLane componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarLaneData.TryGetComponent(target, out componentData1) && (componentData1.m_Flags & Game.Net.CarLaneFlags.Runway) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        {
          airway = true;
          return skipAirway;
        }
        Game.Net.ConnectionLane componentData2;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ConnectionLaneData.TryGetComponent(target, out componentData2))
          return false;
        if ((componentData2.m_Flags & ConnectionLaneFlags.Airway) != (ConnectionLaneFlags) 0)
        {
          airway = true;
          return skipAirway;
        }
        if ((componentData2.m_Flags & ConnectionLaneFlags.Area) == (ConnectionLaneFlags) 0)
          return true;
        area = true;
        return false;
      }

      private float MoveCurvePosition(float3 comparePosition, float minDistance, Bezier4x3 curve)
      {
        float2 float2 = new float2(0.0f, 1f);
        for (int index = 0; index < 8; ++index)
        {
          float t = math.lerp(float2.x, float2.y, 0.5f);
          float3 y = MathUtils.Position(curve, t);
          if ((double) math.distance(comparePosition, y) < (double) minDistance)
            float2.x = t;
          else
            float2.y = t;
        }
        return math.lerp(float2.x, float2.y, 0.5f);
      }

      private void TryAddSegments(
        DynamicBuffer<CurveElement> curveElements,
        DynamicBuffer<CurveSource> curveSources,
        Entity target,
        float2 targetDelta,
        Bezier4x3 curve,
        float nodeDistance,
        float segmentLength,
        ref float3 lastPosition,
        ref float3 lastTangent,
        ref bool airway,
        ref bool area,
        ref bool hasLastPos)
      {
        float3 nextTangent = MathUtils.StartTangent(curve);
        if (hasLastPos)
        {
          // ISSUE: reference to a compiler-generated method
          this.TryAddSegments(curveElements, curveSources, curve.a, nextTangent, nodeDistance, segmentLength, ref lastPosition, ref lastTangent, ref airway, ref area);
        }
        else
        {
          lastPosition = curve.a;
          lastTangent = nextTangent;
          hasLastPos = true;
        }
        if ((double) math.distance(curve.a, curve.d) <= 0.0099999997764825821)
          return;
        curveElements.Add(new CurveElement()
        {
          m_Curve = curve
        });
        if (curveSources.IsCreated)
          curveSources.Add(new CurveSource()
          {
            m_Entity = target,
            m_Range = targetDelta
          });
        lastPosition = curve.d;
        lastTangent = MathUtils.EndTangent(curve);
        airway = false;
      }

      private void TryAddSegments(
        DynamicBuffer<CurveElement> curveElements,
        DynamicBuffer<CurveSource> curveSources,
        float3 position,
        float3 nextTangent,
        float nodeDistance,
        float segmentLength,
        ref float3 lastPosition,
        ref float3 lastTangent,
        ref bool airway,
        ref bool area)
      {
        float num1 = math.distance(lastPosition, position);
        if ((double) num1 <= (double) nodeDistance * 0.25)
          return;
        Bezier4x3 curve1;
        if (airway)
        {
          lastTangent = position - lastPosition;
          nextTangent = position - lastPosition;
          lastTangent.y += num1;
          nextTangent.y -= num1;
          lastTangent = MathUtils.Normalize(lastTangent, lastTangent.xz);
          nextTangent = MathUtils.Normalize(nextTangent, nextTangent.xz);
          curve1 = NetUtils.FitCurve(lastPosition, lastTangent, nextTangent, position);
          num1 = MathUtils.Length(curve1);
          nextTangent = new float3();
        }
        else
        {
          bool2 x1 = new bool2(!lastTangent.Equals(new float3()), !nextTangent.Equals(new float3()));
          if (math.any(x1))
          {
            float3 y1 = position - lastPosition;
            bool2 x2 = (bool2) false;
            bool2 bool2 = (bool2) false;
            if (math.all(x1))
            {
              lastTangent = MathUtils.Normalize(lastTangent, lastTangent.xz);
              nextTangent = MathUtils.Normalize(nextTangent, nextTangent.xz);
              x2.x = (double) math.dot(lastTangent, y1) < (double) nodeDistance * 0.20000000298023224;
              x2.y = (double) math.dot(nextTangent, y1) < (double) nodeDistance * 0.20000000298023224;
              bool2.x = (double) math.dot(lastTangent, y1) < (double) nodeDistance * 0.05000000074505806;
              bool2.y = (double) math.dot(nextTangent, y1) < (double) nodeDistance * 0.05000000074505806;
            }
            else if (x1.x)
            {
              float3 y2 = y1 / num1;
              lastTangent = MathUtils.Normalize(lastTangent, lastTangent.xz);
              nextTangent = y2 * (math.dot(lastTangent, y2) * 2f) - lastTangent;
              x2 = (bool2) ((double) math.dot(lastTangent, y1) < (double) nodeDistance * 0.20000000298023224);
              bool2 = (bool2) ((double) math.dot(lastTangent, y1) < (double) nodeDistance * 0.05000000074505806);
            }
            else if (x1.y)
            {
              float3 y3 = y1 / num1;
              nextTangent = MathUtils.Normalize(nextTangent, nextTangent.xz);
              lastTangent = y3 * (math.dot(nextTangent, y3) * 2f) - nextTangent;
              x2 = (bool2) ((double) math.dot(nextTangent, y1) < (double) nodeDistance * 0.20000000298023224);
              bool2 = (bool2) ((double) math.dot(nextTangent, y1) < (double) nodeDistance * 0.05000000074505806);
            }
            if (math.any(x2))
            {
              float3 float3_1 = lastPosition;
              float3 float3_2 = position;
              if (!bool2.x)
              {
                float num2 = math.dot(lastTangent, y1);
                float3_1 = lastPosition + lastTangent * num2;
                Bezier4x3 curve2 = NetUtils.StraightCurve(lastPosition, float3_1);
                int num3 = Mathf.RoundToInt(num2 / segmentLength);
                if (num3 > 1)
                {
                  for (int x3 = 0; x3 < num3; ++x3)
                  {
                    float2 t = new float2((float) x3, (float) (x3 + 1)) / (float) num3;
                    curveElements.Add(new CurveElement()
                    {
                      m_Curve = MathUtils.Cut(curve2, t)
                    });
                    if (curveSources.IsCreated)
                      curveSources.Add(new CurveSource());
                  }
                }
                else
                {
                  curveElements.Add(new CurveElement()
                  {
                    m_Curve = curve2
                  });
                  if (curveSources.IsCreated)
                    curveSources.Add(new CurveSource());
                }
              }
              if (!bool2.y)
              {
                float num4 = math.dot(nextTangent, y1);
                float3_2 = position - nextTangent * num4;
              }
              float num5 = math.distance(float3_1, float3_2);
              if ((double) num5 >= (double) nodeDistance * 0.5)
              {
                Bezier4x3 curve3 = NetUtils.StraightCurve(float3_1, float3_2);
                int num6 = Mathf.RoundToInt(num5 / segmentLength);
                if (num6 > 1)
                {
                  for (int x4 = 0; x4 < num6; ++x4)
                  {
                    float2 t = new float2((float) x4, (float) (x4 + 1)) / (float) num6;
                    curveElements.Add(new CurveElement()
                    {
                      m_Curve = MathUtils.Cut(curve3, t)
                    });
                    if (curveSources.IsCreated)
                      curveSources.Add(new CurveSource());
                  }
                }
                else
                {
                  curveElements.Add(new CurveElement()
                  {
                    m_Curve = curve3
                  });
                  if (curveSources.IsCreated)
                    curveSources.Add(new CurveSource());
                }
              }
              if (!bool2.y)
              {
                float num7 = math.dot(nextTangent, y1);
                Bezier4x3 curve4 = NetUtils.StraightCurve(float3_2, position);
                int num8 = Mathf.RoundToInt(num7 / segmentLength);
                if (num8 > 1)
                {
                  for (int x5 = 0; x5 < num8; ++x5)
                  {
                    float2 t = new float2((float) x5, (float) (x5 + 1)) / (float) num8;
                    curveElements.Add(new CurveElement()
                    {
                      m_Curve = MathUtils.Cut(curve4, t)
                    });
                    if (curveSources.IsCreated)
                      curveSources.Add(new CurveSource());
                  }
                }
                else
                {
                  curveElements.Add(new CurveElement()
                  {
                    m_Curve = curve4
                  });
                  if (curveSources.IsCreated)
                    curveSources.Add(new CurveSource());
                }
              }
              lastPosition = position;
              lastTangent = nextTangent;
              airway = false;
              area = false;
              return;
            }
            curve1 = NetUtils.FitCurve(lastPosition, lastTangent, nextTangent, position);
            num1 = MathUtils.Length(curve1);
          }
          else
          {
            curve1 = NetUtils.StraightCurve(lastPosition, position);
            nextTangent = position - lastPosition;
          }
        }
        int num9 = Mathf.RoundToInt(num1 / segmentLength);
        if (num9 > 1)
        {
          for (int x = 0; x < num9; ++x)
          {
            float2 t = new float2((float) x, (float) (x + 1)) / (float) num9;
            curveElements.Add(new CurveElement()
            {
              m_Curve = MathUtils.Cut(curve1, t)
            });
            if (curveSources.IsCreated)
              curveSources.Add(new CurveSource());
          }
        }
        else
        {
          curveElements.Add(new CurveElement()
          {
            m_Curve = curve1
          });
          if (curveSources.IsCreated)
            curveSources.Add(new CurveSource());
        }
        lastPosition = position;
        lastTangent = nextTangent;
        airway = false;
        area = false;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathUpdated> __Game_Pathfind_PathUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<CurveElement> __Game_Routes_CurveElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Segment> __Game_Routes_Segment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Position> __Game_Routes_Position_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportStop> __Game_Routes_TransportStop_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathTargets> __Game_Routes_PathTargets_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathSource> __Game_Routes_PathSource_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathOwner> __Game_Pathfind_PathOwner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WatercraftCurrentLane> __Game_Vehicles_WatercraftCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AircraftCurrentLane> __Game_Vehicles_AircraftCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainCurrentLane> __Game_Vehicles_TrainCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> __Game_Vehicles_PublicTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Taxi> __Game_Vehicles_Taxi_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PersonalCar> __Game_Vehicles_PersonalCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Aircraft> __Game_Vehicles_Aircraft_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GroupMember> __Game_Creatures_GroupMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HumanCurrentLane> __Game_Creatures_HumanCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteData> __Game_Prefabs_RouteData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CarNavigationLane> __Game_Vehicles_CarNavigationLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<WatercraftNavigationLane> __Game_Vehicles_WatercraftNavigationLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<AircraftNavigationLane> __Game_Vehicles_AircraftNavigationLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<TrainNavigationLane> __Game_Vehicles_TrainNavigationLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      public BufferLookup<CurveElement> __Game_Routes_CurveElement_RW_BufferLookup;
      public BufferLookup<CurveSource> __Game_Routes_CurveSource_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_CurveElement_RO_BufferLookup = state.GetBufferLookup<CurveElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Segment_RO_ComponentLookup = state.GetComponentLookup<Segment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentLookup = state.GetComponentLookup<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TransportStop_RO_ComponentLookup = state.GetComponentLookup<TransportStop>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_PathTargets_RO_ComponentLookup = state.GetComponentLookup<PathTargets>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_PathSource_RO_ComponentLookup = state.GetComponentLookup<PathSource>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RO_ComponentLookup = state.GetComponentLookup<PathOwner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup = state.GetComponentLookup<CarCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentLookup = state.GetComponentLookup<WatercraftCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftCurrentLane_RO_ComponentLookup = state.GetComponentLookup<AircraftCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup = state.GetComponentLookup<TrainCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PublicTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Taxi_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.Taxi>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PersonalCar_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PersonalCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Aircraft_RO_ComponentLookup = state.GetComponentLookup<Aircraft>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupMember_RO_ComponentLookup = state.GetComponentLookup<GroupMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanCurrentLane_RO_ComponentLookup = state.GetComponentLookup<HumanCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteData_RO_ComponentLookup = state.GetComponentLookup<RouteData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferLookup = state.GetBufferLookup<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarNavigationLane_RO_BufferLookup = state.GetBufferLookup<CarNavigationLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftNavigationLane_RO_BufferLookup = state.GetBufferLookup<WatercraftNavigationLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftNavigationLane_RO_BufferLookup = state.GetBufferLookup<AircraftNavigationLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainNavigationLane_RO_BufferLookup = state.GetBufferLookup<TrainNavigationLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_CurveElement_RW_BufferLookup = state.GetBufferLookup<CurveElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_CurveSource_RW_BufferLookup = state.GetBufferLookup<CurveSource>();
      }
    }
  }
}
