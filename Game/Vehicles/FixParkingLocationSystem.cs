// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.FixParkingLocationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Vehicles
{
  [CompilerGenerated]
  public class FixParkingLocationSystem : GameSystemBase
  {
    private ModificationBarrier5 m_ModificationBarrier;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private EntityQuery m_FixQuery;
    private FixParkingLocationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_FixQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Net.ParkingLane>(),
          ComponentType.ReadOnly<Game.Objects.SpawnLocation>(),
          ComponentType.ReadOnly<Game.Net.ConnectionLane>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Net.ParkingLane>(),
          ComponentType.ReadOnly<Game.Objects.SpawnLocation>(),
          ComponentType.ReadOnly<FixParkingLocation>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_FixQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<Entity> nativeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_MovedLocation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_FixParkingLocation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      FixParkingLocationSystem.CollectParkedCarsJob jobData = new FixParkingLocationSystem.CollectParkedCarsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_FixParkingLocationType = this.__TypeHandle.__Game_Vehicles_FixParkingLocation_RO_ComponentTypeHandle,
        m_ConnectionLaneType = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_SpawnLocationType = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle,
        m_MovedLocationType = this.__TypeHandle.__Game_Objects_MovedLocation_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_LaneObjectType = this.__TypeHandle.__Game_Net_LaneObject_RW_BufferTypeHandle,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ParkedTrainData = this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_ActivityLocations = this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup,
        m_MovingObjectSearchTree = this.m_ObjectSearchSystem.GetMovingSearchTree(true, out dependencies1),
        m_VehicleQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CarKeeper_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PersonalCar_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedTrain_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Relative_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_FixParkingLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies2;
      JobHandle dependencies3;
      JobHandle dependencies4;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new FixParkingLocationSystem.FixParkingLocationJob()
      {
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_FixParkingLocationData = this.__TypeHandle.__Game_Vehicles_FixParkingLocation_RO_ComponentLookup,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_RelativeData = this.__TypeHandle.__Game_Objects_Relative_RO_ComponentLookup,
        m_HelicopterData = this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_TrainData = this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_PrefabTrainData = this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_LaneOverlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_ActivityLocations = this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RW_ComponentLookup,
        m_ParkedTrainData = this.__TypeHandle.__Game_Vehicles_ParkedTrain_RW_ComponentLookup,
        m_PersonalCarData = this.__TypeHandle.__Game_Vehicles_PersonalCar_RW_ComponentLookup,
        m_CarKeeperData = this.__TypeHandle.__Game_Citizens_CarKeeper_RW_ComponentLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RW_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_LaneSearchTree = this.m_NetSearchSystem.GetLaneSearchTree(true, out dependencies2),
        m_StaticObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies3),
        m_VehicleQueue = nativeQueue,
        m_MovingObjectSearchTree = this.m_ObjectSearchSystem.GetMovingSearchTree(false, out dependencies4),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<FixParkingLocationSystem.FixParkingLocationJob>(JobUtils.CombineDependencies(jobData.ScheduleParallel<FixParkingLocationSystem.CollectParkedCarsJob>(this.m_FixQuery, JobHandle.CombineDependencies(this.Dependency, dependencies1)), dependencies2, dependencies3, dependencies4));
      nativeQueue.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddLaneSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddMovingSearchTreeWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
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
    public FixParkingLocationSystem()
    {
    }

    [BurstCompile]
    private struct CollectParkedCarsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<FixParkingLocation> m_FixParkingLocationType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.ConnectionLane> m_ConnectionLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.SpawnLocation> m_SpawnLocationType;
      [ReadOnly]
      public ComponentTypeHandle<MovedLocation> m_MovedLocationType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public BufferTypeHandle<LaneObject> m_LaneObjectType;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> m_ParkedTrainData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> m_ActivityLocations;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_MovingObjectSearchTree;
      public NativeQueue<Entity>.ParallelWriter m_VehicleQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<FixParkingLocation>(ref this.m_FixParkingLocationType))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_VehicleQueue.Enqueue(nativeArray[index]);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<LaneObject> bufferAccessor = chunk.GetBufferAccessor<LaneObject>(ref this.m_LaneObjectType);
          if (bufferAccessor.Length != 0)
          {
            for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
            {
              DynamicBuffer<LaneObject> dynamicBuffer = bufferAccessor[index1];
              int index2 = 0;
              for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
              {
                LaneObject laneObject = dynamicBuffer[index3];
                // ISSUE: reference to a compiler-generated field
                if (this.m_ParkedCarData.HasComponent(laneObject.m_LaneObject))
                {
                  Entity entity = laneObject.m_LaneObject;
                  Controller componentData;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ControllerData.TryGetComponent(laneObject.m_LaneObject, out componentData))
                    entity = componentData.m_Controller;
                  // ISSUE: reference to a compiler-generated field
                  this.m_VehicleQueue.Enqueue(entity);
                }
                else
                  dynamicBuffer[index2++] = laneObject;
              }
              if (index2 != 0)
              {
                if (index2 < dynamicBuffer.Length)
                  dynamicBuffer.RemoveRange(index2, dynamicBuffer.Length - index2);
              }
              else
                dynamicBuffer.Clear();
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Net.ConnectionLane> nativeArray1 = chunk.GetNativeArray<Game.Net.ConnectionLane>(ref this.m_ConnectionLaneType);
            if (nativeArray1.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.m_EntityType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<Curve> nativeArray3 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<Owner> nativeArray4 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
              for (int index = 0; index < nativeArray1.Length; ++index)
              {
                Game.Net.ConnectionLane connectionLane = nativeArray1[index];
                if ((connectionLane.m_Flags & ConnectionLaneFlags.Parking) != (ConnectionLaneFlags) 0)
                {
                  Entity entity = nativeArray2[index];
                  Curve curve = nativeArray3[index];
                  Owner owner = nativeArray4[index];
                  // ISSUE: reference to a compiler-generated method
                  this.AddVehicles(entity, owner, curve, connectionLane);
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<Transform> nativeArray5 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
              if (nativeArray5.Length == 0)
                return;
              // ISSUE: reference to a compiler-generated field
              NativeArray<Entity> nativeArray6 = chunk.GetNativeArray(this.m_EntityType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<Game.Objects.SpawnLocation> nativeArray7 = chunk.GetNativeArray<Game.Objects.SpawnLocation>(ref this.m_SpawnLocationType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<MovedLocation> nativeArray8 = chunk.GetNativeArray<MovedLocation>(ref this.m_MovedLocationType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<PrefabRef> nativeArray9 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
              for (int index = 0; index < nativeArray5.Length; ++index)
              {
                Game.Prefabs.SpawnLocationData componentData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabSpawnLocationData.TryGetComponent(nativeArray9[index].m_Prefab, out componentData) && ((componentData.m_RoadTypes & RoadTypes.Helicopter) != RoadTypes.None && componentData.m_ConnectionType == RouteConnectionType.Air || componentData.m_ConnectionType == RouteConnectionType.Track))
                {
                  Entity entity = nativeArray6[index];
                  Transform transform = nativeArray5[index];
                  Game.Objects.SpawnLocation spawnLocation = nativeArray7[index];
                  MovedLocation movedLocation;
                  if (CollectionUtils.TryGet<MovedLocation>(nativeArray8, index, out movedLocation))
                    transform.m_Position = movedLocation.m_OldPosition;
                  // ISSUE: reference to a compiler-generated method
                  this.AddVehicles(entity, transform, spawnLocation, componentData);
                }
              }
            }
          }
        }
      }

      private void AddVehicles(
        Entity entity,
        Owner owner,
        Curve curve,
        Game.Net.ConnectionLane connectionLane)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        FixParkingLocationSystem.CollectParkedCarsJob.AddVehiclesIterator iterator = new FixParkingLocationSystem.CollectParkedCarsJob.AddVehiclesIterator()
        {
          m_Lane = entity,
          m_Bounds = VehicleUtils.GetConnectionParkingBounds(connectionLane, curve.m_Bezier),
          m_VehicleQueue = this.m_VehicleQueue,
          m_ParkedCarData = this.m_ParkedCarData,
          m_ControllerData = this.m_ControllerData
        };
        Owner owner1 = owner;
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.HasComponent(owner1.m_Owner))
        {
          // ISSUE: reference to a compiler-generated field
          owner1 = this.m_OwnerData[owner1.m_Owner];
        }
        DynamicBuffer<ActivityLocationElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_BuildingData.HasComponent(owner1.m_Owner) && this.m_ActivityLocations.TryGetBuffer(this.m_PrefabRefData[owner1.m_Owner].m_Prefab, out bufferData))
        {
          // ISSUE: reference to a compiler-generated field
          Transform transform = this.m_TransformData[owner1.m_Owner];
          ActivityMask activityMask = new ActivityMask(ActivityType.GarageSpot);
          for (int index = 0; index < bufferData.Length; ++index)
          {
            ActivityLocationElement activityLocationElement = bufferData[index];
            if (((int) activityLocationElement.m_ActivityMask.m_Mask & (int) activityMask.m_Mask) != 0)
            {
              float3 world = ObjectUtils.LocalToWorld(transform, activityLocationElement.m_Position);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              iterator.m_Bounds.min = math.min(iterator.m_Bounds.min, world - 1f);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              iterator.m_Bounds.max = math.max(iterator.m_Bounds.max, world + 1f);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_MovingObjectSearchTree.Iterate<FixParkingLocationSystem.CollectParkedCarsJob.AddVehiclesIterator>(ref iterator);
      }

      private void AddVehicles(
        Entity entity,
        Transform transform,
        Game.Objects.SpawnLocation spawnLocation,
        Game.Prefabs.SpawnLocationData spawnLocationData)
      {
        switch (spawnLocationData.m_ConnectionType)
        {
          case RouteConnectionType.Track:
            DynamicBuffer<LaneObject> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_LaneObjects.TryGetBuffer(spawnLocation.m_ConnectedLane1, out bufferData))
              break;
            for (int index = 0; index < bufferData.Length; ++index)
            {
              LaneObject laneObject = bufferData[index];
              Controller componentData1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ControllerData.TryGetComponent(laneObject.m_LaneObject, out componentData1))
                laneObject.m_LaneObject = componentData1.m_Controller;
              ParkedTrain componentData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ParkedTrainData.TryGetComponent(laneObject.m_LaneObject, out componentData2) && componentData2.m_ParkingLocation == entity)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_VehicleQueue.Enqueue(laneObject.m_LaneObject);
              }
            }
            break;
          case RouteConnectionType.Air:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            FixParkingLocationSystem.CollectParkedCarsJob.AddVehiclesIterator iterator = new FixParkingLocationSystem.CollectParkedCarsJob.AddVehiclesIterator()
            {
              m_Lane = entity,
              m_Bounds = new Bounds3(transform.m_Position - 1f, transform.m_Position + 1f),
              m_VehicleQueue = this.m_VehicleQueue,
              m_ParkedCarData = this.m_ParkedCarData,
              m_ControllerData = this.m_ControllerData
            };
            // ISSUE: reference to a compiler-generated field
            this.m_MovingObjectSearchTree.Iterate<FixParkingLocationSystem.CollectParkedCarsJob.AddVehiclesIterator>(ref iterator);
            break;
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

      private struct AddVehiclesIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Entity m_Lane;
        public Bounds3 m_Bounds;
        public NativeQueue<Entity>.ParallelWriter m_VehicleQueue;
        public ComponentLookup<ParkedCar> m_ParkedCarData;
        public ComponentLookup<Controller> m_ControllerData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds))
            return;
          Controller componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControllerData.TryGetComponent(entity, out componentData1))
            entity = componentData1.m_Controller;
          ParkedCar componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_ParkedCarData.TryGetComponent(entity, out componentData2) || !(componentData2.m_Lane == this.m_Lane))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_VehicleQueue.Enqueue(entity);
        }
      }
    }

    [BurstCompile]
    private struct FixParkingLocationJob : IJob
    {
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<FixParkingLocation> m_FixParkingLocationData;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Relative> m_RelativeData;
      [ReadOnly]
      public ComponentLookup<Helicopter> m_HelicopterData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<Train> m_TrainData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<TrainData> m_PrefabTrainData;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [ReadOnly]
      public BufferLookup<LaneOverlap> m_LaneOverlaps;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> m_ActivityLocations;
      public ComponentLookup<Transform> m_TransformData;
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      public ComponentLookup<ParkedTrain> m_ParkedTrainData;
      public ComponentLookup<PersonalCar> m_PersonalCarData;
      public ComponentLookup<CarKeeper> m_CarKeeperData;
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_LaneSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_StaticObjectSearchTree;
      public NativeQueue<Entity> m_VehicleQueue;
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_MovingObjectSearchTree;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_VehicleQueue.Count == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(this.m_VehicleQueue.Count * 2, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<PathElement> laneBuffer = new NativeList<PathElement>();
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(0);
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
        FixParkingLocationSystem.FixParkingLocationJob.LaneIterator iterator = new FixParkingLocationSystem.FixParkingLocationJob.LaneIterator()
        {
          m_MovingObjectSearchTree = this.m_MovingObjectSearchTree,
          m_ParkedCarData = this.m_ParkedCarData,
          m_ParkedTrainData = this.m_ParkedTrainData,
          m_ControllerData = this.m_ControllerData,
          m_ParkingLaneData = this.m_ParkingLaneData,
          m_CurveData = this.m_CurveData,
          m_UnspawnedData = this.m_UnspawnedData,
          m_TransformData = this.m_TransformData,
          m_SpawnLocationData = this.m_SpawnLocationData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabParkingLaneData = this.m_PrefabParkingLaneData,
          m_PrefabObjectGeometryData = this.m_PrefabObjectGeometryData,
          m_PrefabSpawnLocationData = this.m_PrefabSpawnLocationData,
          m_LaneObjects = this.m_LaneObjects,
          m_LaneOverlaps = this.m_LaneOverlaps
        };
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        while (this.m_VehicleQueue.TryDequeue(out entity1))
        {
          if (nativeParallelHashSet.Add(entity1))
          {
            ParkedCar componentData1;
            // ISSUE: reference to a compiler-generated field
            bool component1 = this.m_ParkedCarData.TryGetComponent(entity1, out componentData1);
            ParkedTrain componentData2;
            // ISSUE: reference to a compiler-generated field
            bool component2 = this.m_ParkedTrainData.TryGetComponent(entity1, out componentData2);
            if (!component1 && !component2)
            {
              // ISSUE: reference to a compiler-generated field
              FixParkingLocation fixParkingLocation = this.m_FixParkingLocationData[entity1];
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<FixParkingLocation>(entity1);
              DynamicBuffer<LaneObject> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_LaneObjects.TryGetBuffer(fixParkingLocation.m_ChangeLane, out bufferData))
                NetUtils.RemoveLaneObject(bufferData, entity1);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              Transform transform1 = this.m_TransformData[entity1];
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef = this.m_PrefabRefData[entity1];
              // ISSUE: reference to a compiler-generated field
              ObjectGeometryData objectGeometryData1 = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
              DynamicBuffer<LayoutElement> bufferData1;
              // ISSUE: reference to a compiler-generated field
              this.m_LayoutElements.TryGetBuffer(entity1, out bufferData1);
              Transform transform2 = transform1;
              bool flag1 = false;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              bool flag2 = component1 && this.m_LaneObjects.HasBuffer(componentData1.m_Lane) && this.m_UnspawnedData.HasComponent(entity1);
              FixParkingLocation componentData3;
              // ISSUE: reference to a compiler-generated field
              if (this.m_FixParkingLocationData.TryGetComponent(entity1, out componentData3))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<FixParkingLocation>(entity1);
                DynamicBuffer<LaneObject> bufferData2;
                // ISSUE: reference to a compiler-generated field
                if (this.m_LaneObjects.TryGetBuffer(componentData3.m_ChangeLane, out bufferData2))
                  NetUtils.RemoveLaneObject(bufferData2, entity1);
                if (componentData3.m_ResetLocation != entity1)
                {
                  Transform componentData4;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_TransformData.TryGetComponent(componentData3.m_ResetLocation, out componentData4))
                    transform1 = componentData4;
                  else
                    flag1 = true;
                  // ISSUE: reference to a compiler-generated method
                  this.RemoveCarKeeper(entity1);
                }
                // ISSUE: reference to a compiler-generated field
                if (component1 && this.m_LaneObjects.TryGetBuffer(componentData1.m_Lane, out bufferData2))
                {
                  NetUtils.RemoveLaneObject(bufferData2, entity1);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ParkingLaneData.HasComponent(componentData1.m_Lane) && nativeParallelHashSet.Add(componentData1.m_Lane) && !this.m_UpdatedData.HasComponent(componentData1.m_Lane))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<PathfindUpdated>(componentData1.m_Lane, new PathfindUpdated());
                  }
                }
                else
                {
                  Entity entity2 = component1 ? componentData1.m_Lane : componentData2.m_ParkingLocation;
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_DeletedData.HasComponent(entity2) && !flag1)
                  {
                    Game.Net.ConnectionLane componentData5;
                    // ISSUE: reference to a compiler-generated field
                    if (component1 && this.m_ConnectionLaneData.TryGetComponent(componentData1.m_Lane, out componentData5))
                    {
                      if ((componentData5.m_Flags & ConnectionLaneFlags.Parking) != (ConnectionLaneFlags) 0)
                      {
                        // ISSUE: reference to a compiler-generated method
                        if (this.FindGarageSpot(ref random, entity1, componentData1.m_Lane, ref transform1))
                        {
                          // ISSUE: reference to a compiler-generated field
                          this.m_TransformData[entity1] = transform1;
                          // ISSUE: reference to a compiler-generated field
                          if (this.m_UnspawnedData.HasComponent(entity1))
                          {
                            // ISSUE: reference to a compiler-generated field
                            this.m_CommandBuffer.RemoveComponent<Unspawned>(entity1);
                            // ISSUE: reference to a compiler-generated field
                            this.m_CommandBuffer.AddComponent<BatchesUpdated>(entity1, new BatchesUpdated());
                          }
                        }
                        // ISSUE: reference to a compiler-generated method
                        this.AddToSearchTree(entity1, transform1, objectGeometryData1);
                        continue;
                      }
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_SpawnLocationData.HasComponent(entity2))
                      {
                        // ISSUE: reference to a compiler-generated field
                        Transform transform3 = this.m_TransformData[entity2];
                        Game.Prefabs.SpawnLocationData componentData6;
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        // ISSUE: reference to a compiler-generated method
                        if (this.m_PrefabSpawnLocationData.TryGetComponent(this.m_PrefabRefData[entity2].m_Prefab, out componentData6) && (component1 && (componentData6.m_RoadTypes & RoadTypes.Helicopter) != RoadTypes.None && componentData6.m_ConnectionType == RouteConnectionType.Air || component2 && componentData6.m_TrackTypes != TrackTypes.None && this.ValidateParkedTrain(bufferData1)) && iterator.TryFindParkingSpace(entity2, entity1, transform3))
                        {
                          if (component2)
                          {
                            // ISSUE: reference to a compiler-generated method
                            this.UpdateTrainLocation(entity1, componentData2.m_ParkingLocation, bufferData1, ref laneBuffer);
                            continue;
                          }
                          transform1.m_Position = transform3.m_Position;
                          // ISSUE: reference to a compiler-generated field
                          this.m_TransformData[entity1] = transform1;
                          // ISSUE: reference to a compiler-generated method
                          this.AddToSearchTree(entity1, transform1, objectGeometryData1);
                          continue;
                        }
                      }
                    }
                  }
                  if (bufferData1.IsCreated && bufferData1.Length != 0)
                  {
                    for (int index = 0; index < bufferData1.Length; ++index)
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_MovingObjectSearchTree.TryRemove(bufferData1[index].m_Vehicle);
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_MovingObjectSearchTree.TryRemove(entity1);
                  }
                }
                componentData1.m_Lane = Entity.Null;
                componentData2.m_ParkingLocation = Entity.Null;
              }
              // ISSUE: reference to a compiler-generated field
              iterator.m_VehicleEntity = entity1;
              // ISSUE: reference to a compiler-generated field
              iterator.m_Position = transform1.m_Position;
              // ISSUE: reference to a compiler-generated field
              iterator.m_MaxDistance = 100f;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              iterator.m_ParkingSize = VehicleUtils.GetParkingSize(entity1, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData, out iterator.m_ParkingOffset);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              iterator.m_Bounds = new Bounds3(iterator.m_Position - iterator.m_MaxDistance, iterator.m_Position + iterator.m_MaxDistance);
              // ISSUE: reference to a compiler-generated field
              iterator.m_SelectedLane = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              iterator.m_KeepUnspawned = flag2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              iterator.m_SpecialVehicle = !this.m_PersonalCarData.HasComponent(entity1);
              // ISSUE: reference to a compiler-generated field
              iterator.m_TrackType = TrackTypes.None;
              TrainData componentData7;
              // ISSUE: reference to a compiler-generated field
              if (component2 && this.m_PrefabTrainData.TryGetComponent(prefabRef.m_Prefab, out componentData7))
              {
                // ISSUE: reference to a compiler-generated field
                iterator.m_TrackType = componentData7.m_TrackType;
              }
              Game.Net.ConnectionLane componentData8;
              // ISSUE: reference to a compiler-generated field
              if (component1 && this.m_ConnectionLaneData.TryGetComponent(componentData1.m_Lane, out componentData8))
              {
                flag1 |= (componentData8.m_Flags & ConnectionLaneFlags.Outside) != 0;
              }
              else
              {
                Game.Net.ParkingLane componentData9;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (component1 && this.m_ParkingLaneData.TryGetComponent(componentData1.m_Lane, out componentData9) && !this.m_DeletedData.HasComponent(componentData1.m_Lane))
                {
                  // ISSUE: reference to a compiler-generated field
                  Curve curve = this.m_CurveData[componentData1.m_Lane];
                  // ISSUE: reference to a compiler-generated method
                  if (flag2 || iterator.TryFindParkingSpace(componentData1.m_Lane, curve, false, ref componentData1.m_CurvePosition))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    ParkingLaneData parkingLaneData = this.m_PrefabParkingLaneData[this.m_PrefabRefData[componentData1.m_Lane].m_Prefab];
                    Transform ownerTransform = new Transform();
                    Owner componentData10;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_OwnerData.TryGetComponent(componentData1.m_Lane, out componentData10) && this.m_TransformData.HasComponent(componentData10.m_Owner))
                    {
                      // ISSUE: reference to a compiler-generated field
                      ownerTransform = this.m_TransformData[componentData10.m_Owner];
                    }
                    transform1 = VehicleUtils.CalculateParkingSpaceTarget(componentData9, parkingLaneData, objectGeometryData1, curve, ownerTransform, componentData1.m_CurvePosition);
                    // ISSUE: reference to a compiler-generated field
                    NetUtils.AddLaneObject(this.m_LaneObjects[componentData1.m_Lane], entity1, (float2) componentData1.m_CurvePosition);
                    // ISSUE: reference to a compiler-generated method
                    this.UpdateParkedCar(entity1, transform2, transform1, componentData1);
                    continue;
                  }
                }
              }
              if (!flag1)
              {
                if (component1)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_LaneSearchTree.Iterate<FixParkingLocationSystem.FixParkingLocationJob.LaneIterator>(ref iterator);
                }
                // ISSUE: reference to a compiler-generated field
                if (((!component1 ? 0 : (this.m_HelicopterData.HasComponent(entity1) ? 1 : 0)) | (component2 ? 1 : 0)) != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_StaticObjectSearchTree.Iterate<FixParkingLocationSystem.FixParkingLocationJob.LaneIterator>(ref iterator);
                }
              }
              // ISSUE: reference to a compiler-generated field
              if (iterator.m_SelectedLane != Entity.Null)
              {
                Game.Net.ParkingLane componentData11;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_ParkingLaneData.TryGetComponent(iterator.m_SelectedLane, out componentData11))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Curve curve = this.m_CurveData[iterator.m_SelectedLane];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  ParkingLaneData parkingLaneData = this.m_PrefabParkingLaneData[this.m_PrefabRefData[iterator.m_SelectedLane].m_Prefab];
                  Transform ownerTransform = new Transform();
                  Owner componentData12;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_OwnerData.TryGetComponent(iterator.m_SelectedLane, out componentData12) && this.m_TransformData.HasComponent(componentData12.m_Owner))
                  {
                    // ISSUE: reference to a compiler-generated field
                    ownerTransform = this.m_TransformData[componentData12.m_Owner];
                  }
                  if (flag2)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    iterator.m_SelectedCurvePos = math.clamp(iterator.m_SelectedCurvePos, 0.05f, 0.95f);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    iterator.m_SelectedCurvePos = random.NextFloat(math.max(0.05f, iterator.m_SelectedCurvePos - 0.2f), math.min(0.95f, iterator.m_SelectedCurvePos + 0.2f));
                  }
                  // ISSUE: reference to a compiler-generated field
                  transform1 = VehicleUtils.CalculateParkingSpaceTarget(componentData11, parkingLaneData, objectGeometryData1, curve, ownerTransform, iterator.m_SelectedCurvePos);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  NetUtils.AddLaneObject(this.m_LaneObjects[iterator.m_SelectedLane], entity1, (float2) iterator.m_SelectedCurvePos);
                  // ISSUE: reference to a compiler-generated field
                  this.m_MovingObjectSearchTree.TryRemove(entity1);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  transform1 = this.m_TransformData[iterator.m_SelectedLane];
                  if (!component2)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddToSearchTree(entity1, transform1, objectGeometryData1);
                  }
                }
                // ISSUE: reference to a compiler-generated field
                componentData1.m_Lane = iterator.m_SelectedLane;
                // ISSUE: reference to a compiler-generated field
                componentData1.m_CurvePosition = iterator.m_SelectedCurvePos;
                // ISSUE: reference to a compiler-generated field
                componentData2.m_ParkingLocation = iterator.m_SelectedLane;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (nativeParallelHashSet.Add(iterator.m_SelectedLane) && !this.m_UpdatedData.HasComponent(iterator.m_SelectedLane))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<PathfindUpdated>(iterator.m_SelectedLane, new PathfindUpdated());
                }
                // ISSUE: reference to a compiler-generated field
                if (this.m_UnspawnedData.HasComponent(entity1) && !flag2)
                {
                  if (bufferData1.IsCreated && bufferData1.Length != 0)
                  {
                    for (int index = 0; index < bufferData1.Length; ++index)
                    {
                      Entity vehicle = bufferData1[index].m_Vehicle;
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.RemoveComponent<Unspawned>(vehicle);
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddComponent<BatchesUpdated>(vehicle, new BatchesUpdated());
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.RemoveComponent<Unspawned>(entity1);
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<BatchesUpdated>(entity1, new BatchesUpdated());
                  }
                }
                if (component1)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.UpdateParkedCar(entity1, transform2, transform1, componentData1);
                }
                else if (component2)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.UpdateTrainLocation(entity1, componentData2.m_ParkingLocation, bufferData1, ref laneBuffer);
                  // ISSUE: reference to a compiler-generated field
                  transform1 = this.m_TransformData[entity1];
                  bool flag3 = transform1.Equals(transform2);
                  for (int index = 0; index < bufferData1.Length; ++index)
                  {
                    Entity vehicle = bufferData1[index].m_Vehicle;
                    // ISSUE: reference to a compiler-generated field
                    ParkedTrain parkedTrain = this.m_ParkedTrainData[vehicle] with
                    {
                      m_ParkingLocation = componentData2.m_ParkingLocation
                    };
                    // ISSUE: reference to a compiler-generated field
                    this.m_ParkedTrainData[vehicle] = parkedTrain;
                    if (flag3)
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddComponent<Updated>(vehicle, new Updated());
                    }
                  }
                }
              }
              else
              {
                componentData1.m_Lane = Entity.Null;
                componentData2.m_ParkingLocation = Entity.Null;
                // ISSUE: reference to a compiler-generated method
                this.RemoveCarKeeper(entity1);
                if (bufferData1.IsCreated && bufferData1.Length != 0)
                {
                  for (int index = 0; index < bufferData1.Length; ++index)
                  {
                    Entity vehicle = bufferData1[index].m_Vehicle;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    ObjectGeometryData objectGeometryData2 = this.m_PrefabObjectGeometryData[this.m_PrefabRefData[vehicle].m_Prefab];
                    if (component1)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.AddToSearchTree(vehicle, transform1, objectGeometryData2);
                    }
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Unspawned>(vehicle, new Unspawned());
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Updated>(vehicle, new Updated());
                  }
                }
                else
                {
                  if (component1)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddToSearchTree(entity1, transform1, objectGeometryData1);
                  }
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Unspawned>(entity1, new Unspawned());
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Updated>(entity1, new Updated());
                }
                if (component1)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.UpdateParkedCar(entity1, transform2, transform1, componentData1);
                }
                else if (component2)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.RemoveTrainFromLanes(bufferData1);
                  for (int index = 0; index < bufferData1.Length; ++index)
                  {
                    Entity vehicle = bufferData1[index].m_Vehicle;
                    // ISSUE: reference to a compiler-generated field
                    ParkedTrain parkedTrain = this.m_ParkedTrainData[vehicle] with
                    {
                      m_ParkingLocation = componentData2.m_ParkingLocation,
                      m_FrontLane = Entity.Null,
                      m_RearLane = Entity.Null
                    };
                    // ISSUE: reference to a compiler-generated field
                    this.m_ParkedTrainData[vehicle] = parkedTrain;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.UpdateParkedTrain(vehicle, this.m_TransformData[vehicle], transform1, parkedTrain);
                  }
                }
              }
            }
          }
        }
        nativeParallelHashSet.Dispose();
        if (!laneBuffer.IsCreated)
          return;
        laneBuffer.Dispose();
      }

      private void UpdateTrainLocation(
        Entity entity,
        Entity parkingLocation,
        DynamicBuffer<LayoutElement> layout,
        ref NativeList<PathElement> laneBuffer)
      {
        // ISSUE: reference to a compiler-generated method
        this.RemoveTrainFromLanes(layout);
        PathOwner pathOwner = new PathOwner();
        ComponentLookup<TrainCurrentLane> currentLaneData = new ComponentLookup<TrainCurrentLane>();
        ComponentLookup<TrainNavigation> navigationData = new ComponentLookup<TrainNavigation>();
        if (laneBuffer.IsCreated)
          laneBuffer.Clear();
        else
          laneBuffer = new NativeList<PathElement>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
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
        PathUtils.InitializeSpawnPath(new DynamicBuffer<PathElement>(), laneBuffer, parkingLocation, ref pathOwner, length, ref this.m_CurveData, ref this.m_LaneData, ref this.m_EdgeLaneData, ref this.m_OwnerData, ref this.m_EdgeData, ref this.m_SpawnLocationData, ref this.m_ConnectedEdges, ref this.m_SubLanes);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.UpdateCarriageLocations(layout, laneBuffer, ref this.m_TrainData, ref this.m_ParkedTrainData, ref currentLaneData, ref navigationData, ref this.m_TransformData, ref this.m_CurveData, ref this.m_ConnectionLaneData, ref this.m_PrefabRefData, ref this.m_PrefabTrainData);
        // ISSUE: reference to a compiler-generated method
        this.AddTrainToLanes(layout);
      }

      private void RemoveTrainFromLanes(DynamicBuffer<LayoutElement> layout)
      {
        for (int index = 0; index < layout.Length; ++index)
        {
          Entity vehicle = layout[index].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          ParkedTrain parkedTrain = this.m_ParkedTrainData[vehicle];
          DynamicBuffer<LaneObject> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneObjects.TryGetBuffer(parkedTrain.m_FrontLane, out bufferData))
            NetUtils.RemoveLaneObject(bufferData, vehicle);
          // ISSUE: reference to a compiler-generated field
          if (parkedTrain.m_RearLane != parkedTrain.m_FrontLane && this.m_LaneObjects.TryGetBuffer(parkedTrain.m_RearLane, out bufferData))
            NetUtils.RemoveLaneObject(bufferData, vehicle);
        }
      }

      private void AddTrainToLanes(DynamicBuffer<LayoutElement> layout)
      {
        for (int index = 0; index < layout.Length; ++index)
        {
          Entity vehicle = layout[index].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          ParkedTrain parkedTrain = this.m_ParkedTrainData[vehicle];
          float2 pos1;
          float2 pos2;
          TrainNavigationHelpers.GetCurvePositions(ref parkedTrain, out pos1, out pos2);
          DynamicBuffer<LaneObject> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneObjects.TryGetBuffer(parkedTrain.m_FrontLane, out bufferData))
            NetUtils.AddLaneObject(bufferData, vehicle, pos1);
          // ISSUE: reference to a compiler-generated field
          if (parkedTrain.m_RearLane != parkedTrain.m_FrontLane && this.m_LaneObjects.TryGetBuffer(parkedTrain.m_RearLane, out bufferData))
            NetUtils.AddLaneObject(bufferData, vehicle, pos2);
        }
      }

      private bool ValidateParkedTrain(DynamicBuffer<LayoutElement> layout)
      {
        for (int index = 0; index < layout.Length; ++index)
        {
          ParkedTrain componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_ParkedTrainData.TryGetComponent(layout[index].m_Vehicle, out componentData) || !this.m_EntityLookup.Exists(componentData.m_FrontLane) || !this.m_EntityLookup.Exists(componentData.m_RearLane))
            return false;
        }
        return true;
      }

      private void AddToSearchTree(
        Entity entity,
        Transform transform,
        ObjectGeometryData objectGeometryData)
      {
        Bounds3 bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, objectGeometryData);
        // ISSUE: reference to a compiler-generated field
        this.m_MovingObjectSearchTree.AddOrUpdate(entity, new QuadTreeBoundsXZ(bounds));
      }

      private void UpdateParkedCar(
        Entity entity,
        Transform oldTransform,
        Transform transform,
        ParkedCar parkedCar)
      {
        if (!transform.Equals(oldTransform))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TransformData[entity] = transform;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
          // ISSUE: reference to a compiler-generated method
          this.UpdateSubObjects(entity, transform);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ParkedCarData[entity] = parkedCar;
      }

      private void UpdateSubObjects(Entity entity, Transform transform)
      {
        DynamicBuffer<Game.Objects.SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(entity, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Game.Objects.SubObject subObject = bufferData[index];
          Relative componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_RelativeData.TryGetComponent(subObject.m_SubObject, out componentData))
          {
            Transform world = ObjectUtils.LocalToWorld(transform, new Transform(componentData.m_Position, componentData.m_Rotation));
            // ISSUE: reference to a compiler-generated field
            this.m_TransformData[subObject.m_SubObject] = world;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(subObject.m_SubObject, new Updated());
            // ISSUE: reference to a compiler-generated method
            this.UpdateSubObjects(subObject.m_SubObject, world);
          }
        }
      }

      private void UpdateParkedTrain(
        Entity entity,
        Transform oldTransform,
        Transform transform,
        ParkedTrain parkedTrain)
      {
        if (!transform.Equals(oldTransform))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TransformData[entity] = transform;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
          // ISSUE: reference to a compiler-generated method
          this.UpdateSubObjects(entity, transform);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ParkedTrainData[entity] = parkedTrain;
      }

      private void RemoveCarKeeper(Entity entity)
      {
        PersonalCar componentData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PersonalCarData.TryGetComponent(entity, out componentData))
          return;
        CarKeeper component;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarKeeperData.TryGetEnabledComponent<CarKeeper>(componentData.m_Keeper, out component) && component.m_Car == entity)
        {
          component.m_Car = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          this.m_CarKeeperData[componentData.m_Keeper] = component;
        }
        componentData.m_Keeper = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        this.m_PersonalCarData[entity] = componentData;
      }

      private bool FindGarageSpot(
        ref Unity.Mathematics.Random random,
        Entity vehicle,
        Entity lane,
        ref Transform transform)
      {
        Entity entity = lane;
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          entity = this.m_OwnerData[entity].m_Owner;
        }
        DynamicBuffer<ActivityLocationElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_BuildingData.HasComponent(entity) || !this.m_ActivityLocations.TryGetBuffer(this.m_PrefabRefData[entity].m_Prefab, out bufferData))
          return false;
        // ISSUE: reference to a compiler-generated field
        Transform transform1 = this.m_TransformData[entity];
        ActivityMask activityMask = new ActivityMask(ActivityType.GarageSpot);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        FixParkingLocationSystem.FixParkingLocationJob.OccupySpotsIterator iterator = new FixParkingLocationSystem.FixParkingLocationJob.OccupySpotsIterator()
        {
          m_Lane = lane,
          m_Ignore = vehicle,
          m_Bounds = new Bounds3((float3) float.MaxValue, (float3) float.MinValue),
          m_Spots = new NativeList<FixParkingLocationSystem.FixParkingLocationJob.SpotData>(bufferData.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp),
          m_ParkedCarData = this.m_ParkedCarData,
          m_TransformData = this.m_TransformData
        };
        int index1 = -1;
        for (int index2 = 0; index2 < bufferData.Length; ++index2)
        {
          ActivityLocationElement activityLocationElement = bufferData[index2];
          if (((int) activityLocationElement.m_ActivityMask.m_Mask & (int) activityMask.m_Mask) != 0)
          {
            float3 world = ObjectUtils.LocalToWorld(transform1, activityLocationElement.m_Position);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            iterator.m_Bounds.min = math.min(iterator.m_Bounds.min, world - 1f);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            iterator.m_Bounds.max = math.max(iterator.m_Bounds.max, world + 1f);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            iterator.m_Spots.Add(new FixParkingLocationSystem.FixParkingLocationJob.SpotData()
            {
              m_Position = world,
              m_Index = index2
            });
            if ((double) math.distancesq(transform.m_Position, world) < 1.0)
              index1 = index2;
          }
        }
        bool garageSpot = false;
        // ISSUE: reference to a compiler-generated field
        if (iterator.m_Spots.Length > 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (iterator.m_Spots.Length >= 2)
          {
            // ISSUE: reference to a compiler-generated field
            float3 float3 = MathUtils.Size(iterator.m_Bounds);
            // ISSUE: reference to a compiler-generated field
            iterator.m_Order = math.select(0, 1, (double) float3.y > (double) float3.x);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            iterator.m_Order = math.select(iterator.m_Order, 2, math.all(float3.z > float3.xy));
          }
          // ISSUE: reference to a compiler-generated field
          for (int index3 = 0; index3 < iterator.m_Spots.Length; ++index3)
          {
            // ISSUE: reference to a compiler-generated field
            ref FixParkingLocationSystem.FixParkingLocationJob.SpotData local = ref iterator.m_Spots.ElementAt(index3);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            local.m_Order = local.m_Position[iterator.m_Order];
          }
          // ISSUE: reference to a compiler-generated field
          if (iterator.m_Spots.Length >= 2)
          {
            // ISSUE: reference to a compiler-generated field
            iterator.m_Spots.Sort<FixParkingLocationSystem.FixParkingLocationJob.SpotData>();
          }
          // ISSUE: reference to a compiler-generated field
          this.m_MovingObjectSearchTree.Iterate<FixParkingLocationSystem.FixParkingLocationJob.OccupySpotsIterator>(ref iterator);
          int max = 0;
          bool flag = false;
          // ISSUE: reference to a compiler-generated field
          for (int index4 = 0; index4 < iterator.m_Spots.Length; ++index4)
          {
            // ISSUE: reference to a compiler-generated field
            ref FixParkingLocationSystem.FixParkingLocationJob.SpotData local = ref iterator.m_Spots.ElementAt(index4);
            // ISSUE: reference to a compiler-generated field
            max += math.select(1, 0, local.m_Occupied);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            flag = ((flag ? 1 : 0) | (local.m_Index != index1 ? 0 : (!local.m_Occupied ? 1 : 0))) != 0;
          }
          if (max != 0 && !flag)
          {
            int num = random.NextInt(max);
            // ISSUE: reference to a compiler-generated field
            for (int index5 = 0; index5 < iterator.m_Spots.Length; ++index5)
            {
              // ISSUE: reference to a compiler-generated field
              ref FixParkingLocationSystem.FixParkingLocationJob.SpotData local = ref iterator.m_Spots.ElementAt(index5);
              // ISSUE: reference to a compiler-generated field
              if (!local.m_Occupied && num-- == 0)
              {
                // ISSUE: reference to a compiler-generated field
                index1 = local.m_Index;
                flag = true;
                break;
              }
            }
          }
          if (flag)
          {
            ActivityLocationElement activityLocationElement = bufferData[index1];
            transform = ObjectUtils.LocalToWorld(transform1, activityLocationElement.m_Position, activityLocationElement.m_Rotation);
            garageSpot = true;
          }
        }
        // ISSUE: reference to a compiler-generated field
        iterator.m_Spots.Dispose();
        return garageSpot;
      }

      private struct SpotData : IComparable<FixParkingLocationSystem.FixParkingLocationJob.SpotData>
      {
        public float3 m_Position;
        public float m_Order;
        public int m_Index;
        public bool m_Occupied;

        public int CompareTo(
          FixParkingLocationSystem.FixParkingLocationJob.SpotData other)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return math.select(0, math.select(-1, 1, (double) this.m_Order > (double) other.m_Order), (double) this.m_Order != (double) other.m_Order);
        }
      }

      private struct OccupySpotsIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Entity m_Lane;
        public Entity m_Ignore;
        public Bounds3 m_Bounds;
        public int m_Order;
        public NativeList<FixParkingLocationSystem.FixParkingLocationJob.SpotData> m_Spots;
        public ComponentLookup<ParkedCar> m_ParkedCarData;
        public ComponentLookup<Transform> m_TransformData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          ParkedCar componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_ParkedCarData.TryGetComponent(entity, out componentData) || !(entity != this.m_Ignore) || !(componentData.m_Lane == this.m_Lane))
            return;
          // ISSUE: reference to a compiler-generated field
          Transform transform = this.m_TransformData[entity];
          // ISSUE: reference to a compiler-generated field
          float num1 = transform.m_Position[this.m_Order] - 1f;
          // ISSUE: reference to a compiler-generated field
          float num2 = transform.m_Position[this.m_Order] + 1f;
          int num3 = 0;
          // ISSUE: reference to a compiler-generated field
          int num4 = this.m_Spots.Length;
          while (num4 > num3)
          {
            int index = num3 + num4 >> 1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((double) this.m_Spots[index].m_Order < (double) num1)
              num3 = index + 1;
            else
              num4 = index;
          }
          // ISSUE: reference to a compiler-generated field
          int length = this.m_Spots.Length;
          while (num3 < length)
          {
            // ISSUE: reference to a compiler-generated field
            ref FixParkingLocationSystem.FixParkingLocationJob.SpotData local = ref this.m_Spots.ElementAt(num3++);
            // ISSUE: reference to a compiler-generated field
            if ((double) local.m_Order > (double) num2)
              break;
            // ISSUE: reference to a compiler-generated field
            if ((double) math.distancesq(transform.m_Position, local.m_Position) < 1.0)
            {
              // ISSUE: reference to a compiler-generated field
              local.m_Occupied = true;
            }
          }
        }
      }

      private struct SpawnLocationIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Entity m_Lane;
        public Entity m_Ignore;
        public Bounds3 m_Bounds;
        public ComponentLookup<ParkedCar> m_ParkedCarData;
        public ComponentLookup<Controller> m_ControllerData;
        public bool m_Occupied;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          Controller componentData1;
          ParkedCar componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || entity == this.m_Ignore || this.m_ControllerData.TryGetComponent(entity, out componentData1) && componentData1.m_Controller == this.m_Ignore || !this.m_ParkedCarData.TryGetComponent(entity, out componentData2) || !(componentData2.m_Lane == this.m_Lane))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_Occupied = true;
        }
      }

      private struct LaneIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Entity m_VehicleEntity;
        public Bounds3 m_Bounds;
        public float3 m_Position;
        public float2 m_ParkingSize;
        public float m_MaxDistance;
        public float m_ParkingOffset;
        public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_MovingObjectSearchTree;
        public ComponentLookup<ParkedCar> m_ParkedCarData;
        public ComponentLookup<ParkedTrain> m_ParkedTrainData;
        public ComponentLookup<Controller> m_ControllerData;
        public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
        public ComponentLookup<Curve> m_CurveData;
        public ComponentLookup<Unspawned> m_UnspawnedData;
        public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
        public ComponentLookup<Transform> m_TransformData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
        public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
        public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
        public BufferLookup<LaneObject> m_LaneObjects;
        public BufferLookup<LaneOverlap> m_LaneOverlaps;
        public Entity m_SelectedLane;
        public float m_SelectedCurvePos;
        public bool m_KeepUnspawned;
        public bool m_SpecialVehicle;
        public TrackTypes m_TrackType;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds))
            return;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ParkingLaneData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[entity];
            float t;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if ((double) MathUtils.Distance(curve.m_Bezier, this.m_Position, out t) >= (double) this.m_MaxDistance || !this.m_KeepUnspawned && !this.TryFindParkingSpace(entity, curve, true, ref t))
              return;
            // ISSUE: reference to a compiler-generated field
            float num = math.distance(this.m_Position, MathUtils.Position(curve.m_Bezier, t));
            // ISSUE: reference to a compiler-generated field
            if ((double) num >= (double) this.m_MaxDistance)
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_MaxDistance = num;
            // ISSUE: reference to a compiler-generated field
            this.m_SelectedLane = entity;
            // ISSUE: reference to a compiler-generated field
            this.m_SelectedCurvePos = t;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Bounds = new Bounds3(this.m_Position - this.m_MaxDistance, this.m_Position + this.m_MaxDistance);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_SpawnLocationData.HasComponent(entity))
              return;
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_TransformData[entity];
            // ISSUE: reference to a compiler-generated field
            float num = math.distance(transform.m_Position, this.m_Position);
            Game.Prefabs.SpawnLocationData componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if ((double) num >= (double) this.m_MaxDistance || !this.m_PrefabSpawnLocationData.TryGetComponent(this.m_PrefabRefData[entity].m_Prefab, out componentData) || (this.m_TrackType != TrackTypes.None || (componentData.m_RoadTypes & RoadTypes.Helicopter) == RoadTypes.None || componentData.m_ConnectionType != RouteConnectionType.Air) && (componentData.m_TrackTypes & this.m_TrackType) == TrackTypes.None || !this.TryFindParkingSpace(entity, this.m_VehicleEntity, transform))
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_MaxDistance = num;
            // ISSUE: reference to a compiler-generated field
            this.m_SelectedLane = entity;
            // ISSUE: reference to a compiler-generated field
            this.m_SelectedCurvePos = 0.0f;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Bounds = new Bounds3(this.m_Position - this.m_MaxDistance, this.m_Position + this.m_MaxDistance);
          }
        }

        public bool TryFindParkingSpace(Entity lane, Entity vehicle, Transform transform)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          switch (this.m_PrefabSpawnLocationData[this.m_PrefabRefData[lane].m_Prefab].m_ConnectionType)
          {
            case RouteConnectionType.Track:
              DynamicBuffer<LaneObject> bufferData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_LaneObjects.TryGetBuffer(this.m_SpawnLocationData[lane].m_ConnectedLane1, out bufferData))
              {
                for (int index = 0; index < bufferData.Length; ++index)
                {
                  LaneObject laneObject = bufferData[index];
                  Controller componentData1;
                  ParkedTrain componentData2;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!(laneObject.m_LaneObject == vehicle) && (!this.m_ControllerData.TryGetComponent(laneObject.m_LaneObject, out componentData1) || !(componentData1.m_Controller == vehicle)) && this.m_ParkedTrainData.TryGetComponent(laneObject.m_LaneObject, out componentData2) && componentData2.m_ParkingLocation == lane)
                    return false;
                }
              }
              return true;
            case RouteConnectionType.Air:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              FixParkingLocationSystem.FixParkingLocationJob.SpawnLocationIterator iterator = new FixParkingLocationSystem.FixParkingLocationJob.SpawnLocationIterator()
              {
                m_Lane = lane,
                m_Bounds = new Bounds3(transform.m_Position - 1f, transform.m_Position + 1f),
                m_Ignore = vehicle,
                m_ParkedCarData = this.m_ParkedCarData,
                m_ControllerData = this.m_ControllerData
              };
              // ISSUE: reference to a compiler-generated field
              this.m_MovingObjectSearchTree.Iterate<FixParkingLocationSystem.FixParkingLocationJob.SpawnLocationIterator>(ref iterator);
              // ISSUE: reference to a compiler-generated field
              return !iterator.m_Occupied;
            default:
              return false;
          }
        }

        public bool TryFindParkingSpace(
          Entity lane,
          Curve curve,
          bool ignoreDisabled,
          ref float curvePos)
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.ParkingLane parkingLane = this.m_ParkingLaneData[lane];
          // ISSUE: reference to a compiler-generated field
          if ((parkingLane.m_Flags & ParkingLaneFlags.VirtualLane) != (ParkingLaneFlags) 0 || ignoreDisabled && (parkingLane.m_Flags & ParkingLaneFlags.ParkingDisabled) != (ParkingLaneFlags) 0 || this.m_SpecialVehicle != ((parkingLane.m_Flags & ParkingLaneFlags.SpecialVehicles) != 0))
            return false;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[lane];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<LaneObject> laneObject1 = this.m_LaneObjects[lane];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<LaneOverlap> laneOverlap1 = this.m_LaneOverlaps[lane];
          // ISSUE: reference to a compiler-generated field
          ParkingLaneData parkingLaneData = this.m_PrefabParkingLaneData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          if (math.any(this.m_ParkingSize > VehicleUtils.GetParkingSize(parkingLaneData)))
            return false;
          if ((double) parkingLaneData.m_SlotInterval != 0.0)
          {
            int parkingSlotCount = NetUtils.GetParkingSlotCount(curve, parkingLane, parkingLaneData);
            float parkingSlotInterval = NetUtils.GetParkingSlotInterval(curve, parkingLane, parkingLaneData, parkingSlotCount);
            float3 x1 = curve.m_Bezier.a;
            float2 float2_1 = (float2) 0.0f;
            float num1 = 0.0f;
            float x2;
            switch (parkingLane.m_Flags & (ParkingLaneFlags.StartingLane | ParkingLaneFlags.EndingLane))
            {
              case ParkingLaneFlags.StartingLane:
                x2 = curve.m_Length - (float) parkingSlotCount * parkingSlotInterval;
                break;
              case ParkingLaneFlags.EndingLane:
                x2 = 0.0f;
                break;
              default:
                x2 = (float) (((double) curve.m_Length - (double) parkingSlotCount * (double) parkingSlotInterval) * 0.5);
                break;
            }
            float num2 = math.max(x2, 0.0f);
            int num3 = -1;
            float num4 = 1f;
            float num5 = curvePos;
            float num6 = 2f;
            int num7 = 0;
            while (num7 < laneObject1.Length)
            {
              LaneObject laneObject2 = laneObject1[num7++];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_ParkedCarData.HasComponent(laneObject2.m_LaneObject) && !this.m_UnspawnedData.HasComponent(laneObject2.m_LaneObject))
              {
                num6 = laneObject2.m_CurvePosition.x;
                break;
              }
            }
            float2 float2_2 = (float2) 2f;
            int num8 = 0;
            if (num8 < laneOverlap1.Length)
            {
              LaneOverlap laneOverlap2 = laneOverlap1[num8++];
              float2_2 = new float2((float) laneOverlap2.m_ThisStart, (float) laneOverlap2.m_ThisEnd) * 0.003921569f;
            }
            for (int index = 1; index <= 16; ++index)
            {
              float num9 = (float) index * (1f / 16f);
              float3 y = MathUtils.Position(curve.m_Bezier, num9);
              for (num1 += math.distance(x1, y); (double) num1 >= (double) num2 || index == 16 && num3 < parkingSlotCount; ++num3)
              {
                float2_1.y = math.select(num9, math.lerp(float2_1.x, num9, num2 / num1), (double) num2 < (double) num1);
                bool flag = false;
                if ((double) num6 <= (double) float2_1.y)
                {
                  num6 = 2f;
                  flag = true;
                  while (num7 < laneObject1.Length)
                  {
                    LaneObject laneObject3 = laneObject1[num7++];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_ParkedCarData.HasComponent(laneObject3.m_LaneObject) && !this.m_UnspawnedData.HasComponent(laneObject3.m_LaneObject) && (double) laneObject3.m_CurvePosition.x > (double) float2_1.y)
                    {
                      num6 = laneObject3.m_CurvePosition.x;
                      break;
                    }
                  }
                }
                if ((double) float2_2.x < (double) float2_1.y)
                {
                  flag = true;
                  if ((double) float2_2.y <= (double) float2_1.y)
                  {
                    float2_2 = (float2) 2f;
                    while (num8 < laneOverlap1.Length)
                    {
                      LaneOverlap laneOverlap3 = laneOverlap1[num8++];
                      float2 float2_3 = new float2((float) laneOverlap3.m_ThisStart, (float) laneOverlap3.m_ThisEnd) * 0.003921569f;
                      if ((double) float2_3.y > (double) float2_1.y)
                      {
                        float2_2 = float2_3;
                        break;
                      }
                    }
                  }
                }
                if (!flag && num3 >= 0 && num3 < parkingSlotCount)
                {
                  float num10 = math.max(float2_1.x - curvePos, curvePos - float2_1.y);
                  if ((double) num10 < (double) num4)
                  {
                    num5 = math.lerp(float2_1.x, float2_1.y, 0.5f);
                    num4 = num10;
                  }
                }
                num1 -= num2;
                float2_1.x = float2_1.y;
                num2 = parkingSlotInterval;
              }
              x1 = y;
            }
            if ((double) num5 != (double) curvePos && (double) parkingLaneData.m_SlotAngle <= 0.25)
            {
              // ISSUE: reference to a compiler-generated field
              if ((double) this.m_ParkingOffset > 0.0)
              {
                Bounds1 t = new Bounds1(num5, 1f);
                // ISSUE: reference to a compiler-generated field
                MathUtils.ClampLength(curve.m_Bezier, ref t, this.m_ParkingOffset);
                num5 = t.max;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if ((double) this.m_ParkingOffset < 0.0)
                {
                  Bounds1 t = new Bounds1(0.0f, num5);
                  // ISSUE: reference to a compiler-generated field
                  MathUtils.ClampLengthInverse(curve.m_Bezier, ref t, -this.m_ParkingOffset);
                  num5 = t.min;
                }
              }
            }
            curvePos = num5;
            return (double) num4 != 1.0;
          }
          float2 float2_4 = new float2();
          float2 float2_5 = new float2();
          float num11 = 1f;
          float3 float3 = new float3();
          float2 x3 = (float2) math.select(0.0f, 0.5f, (parkingLane.m_Flags & ParkingLaneFlags.StartingLane) == (ParkingLaneFlags) 0);
          float3 x4 = curve.m_Bezier.a;
          float num12 = 2f;
          float2 float2_6 = (float2) 0.0f;
          int num13 = 0;
          while (num13 < laneObject1.Length)
          {
            LaneObject laneObject4 = laneObject1[num13++];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ParkedCarData.HasComponent(laneObject4.m_LaneObject) && !this.m_UnspawnedData.HasComponent(laneObject4.m_LaneObject))
            {
              num12 = laneObject4.m_CurvePosition.x;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float2_6 = VehicleUtils.GetParkingOffsets(laneObject4.m_LaneObject, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData) + 1f;
              break;
            }
          }
          float2 float2_7 = (float2) 2f;
          int num14 = 0;
          if (num14 < laneOverlap1.Length)
          {
            LaneOverlap laneOverlap4 = laneOverlap1[num14++];
            float2_7 = new float2((float) laneOverlap4.m_ThisStart, (float) laneOverlap4.m_ThisEnd) * 0.003921569f;
          }
          while ((double) num12 != 2.0 || (double) float2_7.x != 2.0)
          {
            float num15;
            if ((double) num12 <= (double) float2_7.x)
            {
              float3.yz = (float2) num12;
              x3.y = float2_6.x;
              num15 = float2_6.y;
              num12 = 2f;
              while (num13 < laneObject1.Length)
              {
                LaneObject laneObject5 = laneObject1[num13++];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_ParkedCarData.HasComponent(laneObject5.m_LaneObject) && !this.m_UnspawnedData.HasComponent(laneObject5.m_LaneObject))
                {
                  num12 = laneObject5.m_CurvePosition.x;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float2_6 = VehicleUtils.GetParkingOffsets(laneObject5.m_LaneObject, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData) + 1f;
                  break;
                }
              }
            }
            else
            {
              float3.yz = float2_7;
              x3.y = 0.5f;
              num15 = 0.5f;
              float2_7 = (float2) 2f;
              while (num14 < laneOverlap1.Length)
              {
                LaneOverlap laneOverlap5 = laneOverlap1[num14++];
                float2 float2_8 = new float2((float) laneOverlap5.m_ThisStart, (float) laneOverlap5.m_ThisEnd) * 0.003921569f;
                if ((double) float2_8.x <= (double) float3.z)
                {
                  float3.z = math.max(float3.z, float2_8.y);
                }
                else
                {
                  float2_7 = float2_8;
                  break;
                }
              }
            }
            float3 y = MathUtils.Position(curve.m_Bezier, float3.y);
            // ISSUE: reference to a compiler-generated field
            if ((double) math.distance(x4, y) - (double) math.csum(x3) >= (double) this.m_ParkingSize.y)
            {
              float num16 = math.max(float3.x - curvePos, curvePos - float3.y);
              if ((double) num16 < (double) num11)
              {
                float2_4 = float3.xy;
                float2_5 = x3;
                num11 = num16;
              }
            }
            float3.x = float3.z;
            x3.x = num15;
            x4 = MathUtils.Position(curve.m_Bezier, float3.z);
          }
          float3.y = 1f;
          x3.y = math.select(0.0f, 0.5f, (parkingLane.m_Flags & ParkingLaneFlags.EndingLane) == (ParkingLaneFlags) 0);
          // ISSUE: reference to a compiler-generated field
          if ((double) math.distance(x4, curve.m_Bezier.d) - (double) math.csum(x3) >= (double) this.m_ParkingSize.y)
          {
            float num17 = math.max(float3.x - curvePos, curvePos - float3.y);
            if ((double) num17 < (double) num11)
            {
              float2_4 = float3.xy;
              float2_5 = x3;
              num11 = num17;
            }
          }
          if ((double) num11 == 1.0)
            return false;
          // ISSUE: reference to a compiler-generated field
          float2 float2_9 = float2_5 + this.m_ParkingSize.y * 0.5f;
          // ISSUE: reference to a compiler-generated field
          float2_9.x += this.m_ParkingOffset;
          // ISSUE: reference to a compiler-generated field
          float2_9.y -= this.m_ParkingOffset;
          Bounds1 t1 = new Bounds1(float2_4.x, float2_4.y);
          Bounds1 t2 = new Bounds1(float2_4.x, float2_4.y);
          MathUtils.ClampLength(curve.m_Bezier, ref t1, float2_9.x);
          MathUtils.ClampLengthInverse(curve.m_Bezier, ref t2, float2_9.y);
          if ((double) curvePos < (double) t1.max || (double) curvePos > (double) t2.min)
            curvePos = (double) t1.max >= (double) t2.min ? math.lerp(t1.max, t2.min, 0.5f) : math.select(t1.max, t2.min, (double) curvePos > (double) t2.min);
          return true;
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<FixParkingLocation> __Game_Vehicles_FixParkingLocation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<MovedLocation> __Game_Objects_MovedLocation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public BufferTypeHandle<LaneObject> __Game_Net_LaneObject_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> __Game_Vehicles_ParkedTrain_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> __Game_Prefabs_ActivityLocationElement_RO_BufferLookup;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<FixParkingLocation> __Game_Vehicles_FixParkingLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Relative> __Game_Objects_Relative_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Helicopter> __Game_Vehicles_Helicopter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Train> __Game_Vehicles_Train_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainData> __Game_Prefabs_TrainData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      public ComponentLookup<Transform> __Game_Objects_Transform_RW_ComponentLookup;
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RW_ComponentLookup;
      public ComponentLookup<ParkedTrain> __Game_Vehicles_ParkedTrain_RW_ComponentLookup;
      public ComponentLookup<PersonalCar> __Game_Vehicles_PersonalCar_RW_ComponentLookup;
      public ComponentLookup<CarKeeper> __Game_Citizens_CarKeeper_RW_ComponentLookup;
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_FixParkingLocation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<FixParkingLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_MovedLocation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MovedLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RW_BufferTypeHandle = state.GetBufferTypeHandle<LaneObject>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedTrain_RO_ComponentLookup = state.GetComponentLookup<ParkedTrain>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup = state.GetBufferLookup<ActivityLocationElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_FixParkingLocation_RO_ComponentLookup = state.GetComponentLookup<FixParkingLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Relative_RO_ComponentLookup = state.GetComponentLookup<Relative>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Helicopter_RO_ComponentLookup = state.GetComponentLookup<Helicopter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Train_RO_ComponentLookup = state.GetComponentLookup<Train>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrainData_RO_ComponentLookup = state.GetComponentLookup<TrainData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RW_ComponentLookup = state.GetComponentLookup<Transform>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RW_ComponentLookup = state.GetComponentLookup<ParkedCar>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedTrain_RW_ComponentLookup = state.GetComponentLookup<ParkedTrain>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PersonalCar_RW_ComponentLookup = state.GetComponentLookup<PersonalCar>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CarKeeper_RW_ComponentLookup = state.GetComponentLookup<CarKeeper>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RW_BufferLookup = state.GetBufferLookup<LaneObject>();
      }
    }
  }
}
