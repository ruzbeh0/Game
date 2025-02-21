// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.ParkingLaneDataSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Pathfind
{
  [CompilerGenerated]
  public class ParkingLaneDataSystem : GameSystemBase
  {
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private CitySystem m_CitySystem;
    private EntityQuery m_LaneQuery;
    private ParkingLaneDataSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Net.ParkingLane>(),
          ComponentType.ReadOnly<GarageLane>(),
          ComponentType.ReadOnly<Game.Objects.SpawnLocation>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PathfindUpdated>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Net.ParkingLane>(),
          ComponentType.ReadOnly<GarageLane>(),
          ComponentType.ReadOnly<Game.Objects.SpawnLocation>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_LaneQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_BuildingModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_City_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ParkingFacility_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_District_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_GarageLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new ParkingLaneDataSystem.UpdateLaneDataJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_LaneType = this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_LaneOverlapType = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferTypeHandle,
        m_ParkingLaneType = this.__TypeHandle.__Game_Net_ParkingLane_RW_ComponentTypeHandle,
        m_ConnectionLaneType = this.__TypeHandle.__Game_Net_ConnectionLane_RW_ComponentTypeHandle,
        m_GarageLaneType = this.__TypeHandle.__Game_Net_GarageLane_RW_ComponentTypeHandle,
        m_SpawnLocationType = this.__TypeHandle.__Game_Objects_SpawnLocation_RW_ComponentTypeHandle,
        m_LaneObjectType = this.__TypeHandle.__Game_Net_LaneObject_RW_BufferTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_RoadData = this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ParkedTrainData = this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_BorderDistrictData = this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentLookup,
        m_DistrictData = this.__TypeHandle.__Game_Areas_District_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_ParkingFacilityData = this.__TypeHandle.__Game_Buildings_ParkingFacility_RO_ComponentLookup,
        m_CityData = this.__TypeHandle.__Game_City_City_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_PrefabParkingFacilityData = this.__TypeHandle.__Game_Prefabs_ParkingFacilityData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabBuildingPropertyData = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_PrefabWorkplaceData = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_DistrictModifiers = this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup,
        m_BuildingModifiers = this.__TypeHandle.__Game_Buildings_BuildingModifier_RO_BufferLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_ActivityLocations = this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup,
        m_City = this.m_CitySystem.City,
        m_MovingObjectSearchTree = this.m_ObjectSearchSystem.GetMovingSearchTree(true, out dependencies)
      }.ScheduleParallel<ParkingLaneDataSystem.UpdateLaneDataJob>(this.m_LaneQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddMovingSearchTreeReader(jobHandle);
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
    public ParkingLaneDataSystem()
    {
    }

    [BurstCompile]
    private struct UpdateLaneDataJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Lane> m_LaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<LaneOverlap> m_LaneOverlapType;
      public ComponentTypeHandle<Game.Net.ParkingLane> m_ParkingLaneType;
      public ComponentTypeHandle<Game.Net.ConnectionLane> m_ConnectionLaneType;
      public ComponentTypeHandle<GarageLane> m_GarageLaneType;
      public ComponentTypeHandle<Game.Objects.SpawnLocation> m_SpawnLocationType;
      public BufferTypeHandle<LaneObject> m_LaneObjectType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<Road> m_RoadData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> m_ParkedTrainData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> m_BorderDistrictData;
      [ReadOnly]
      public ComponentLookup<District> m_DistrictData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ParkingFacility> m_ParkingFacilityData;
      [ReadOnly]
      public ComponentLookup<Game.City.City> m_CityData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
      [ReadOnly]
      public ComponentLookup<Attachment> m_AttachmentData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<ParkingFacilityData> m_PrefabParkingFacilityData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_PrefabBuildingPropertyData;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> m_PrefabWorkplaceData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public BufferLookup<DistrictModifier> m_DistrictModifiers;
      [ReadOnly]
      public BufferLookup<BuildingModifier> m_BuildingModifiers;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> m_ActivityLocations;
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_MovingObjectSearchTree;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.ParkingLane> nativeArray1 = chunk.GetNativeArray<Game.Net.ParkingLane>(ref this.m_ParkingLaneType);
        if (nativeArray1.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Curve> nativeArray2 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Lane> nativeArray4 = chunk.GetNativeArray<Lane>(ref this.m_LaneType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<LaneObject> bufferAccessor1 = chunk.GetBufferAccessor<LaneObject>(ref this.m_LaneObjectType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<LaneOverlap> bufferAccessor2 = chunk.GetBufferAccessor<LaneOverlap>(ref this.m_LaneOverlapType);
          ushort num = 0;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_City != Entity.Null && CityUtils.CheckOption(this.m_CityData[this.m_City], CityOption.PaidTaxiStart))
          {
            float f = 0.0f;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            CityUtils.ApplyModifier(ref f, this.m_CityModifiers[this.m_City], CityModifierType.TaxiStartingFee);
            num = (ushort) math.clamp(Mathf.RoundToInt(f), 0, (int) ushort.MaxValue);
          }
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Curve curve = nativeArray2[index];
            Owner owner = nativeArray3[index];
            Lane laneData = nativeArray4[index];
            Game.Net.ParkingLane parkingLane = nativeArray1[index];
            PrefabRef prefabRef = nativeArray5[index];
            DynamicBuffer<LaneObject> laneObjects = bufferAccessor1[index];
            DynamicBuffer<LaneOverlap> laneOverlaps = bufferAccessor2[index];
            // ISSUE: reference to a compiler-generated field
            ParkingLaneData parkingLaneData = this.m_ParkingLaneData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated method
            Bounds1 blockedRange = this.GetBlockedRange(owner, laneData);
            parkingLane.m_Flags &= ~(ParkingLaneFlags.ParkingDisabled | ParkingLaneFlags.AllowEnter | ParkingLaneFlags.AllowExit);
            laneObjects.AsNativeArray().Sort<LaneObject>();
            // ISSUE: reference to a compiler-generated method
            parkingLane.m_FreeSpace = this.CalculateFreeSpace(curve, parkingLane, parkingLaneData, laneObjects, laneOverlaps, blockedRange);
            bool disabled;
            bool allowEnter;
            bool allowExit;
            // ISSUE: reference to a compiler-generated method
            this.GetParkingStats(owner, parkingLane, out parkingLane.m_AccessRestriction, out ushort _, out parkingLane.m_ParkingFee, out parkingLane.m_ComfortFactor, out disabled, out allowEnter, out allowExit);
            parkingLane.m_TaxiFee = num;
            if (disabled)
              parkingLane.m_Flags |= ParkingLaneFlags.ParkingDisabled;
            if (allowEnter)
              parkingLane.m_Flags |= ParkingLaneFlags.AllowEnter;
            if (allowExit)
              parkingLane.m_Flags |= ParkingLaneFlags.AllowExit;
            nativeArray1[index] = parkingLane;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<GarageLane> nativeArray6 = chunk.GetNativeArray<GarageLane>(ref this.m_GarageLaneType);
          if (nativeArray6.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray7 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Curve> nativeArray8 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Owner> nativeArray9 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Net.ConnectionLane> nativeArray10 = chunk.GetNativeArray<Game.Net.ConnectionLane>(ref this.m_ConnectionLaneType);
            for (int index = 0; index < nativeArray6.Length; ++index)
            {
              Entity entity = nativeArray7[index];
              Curve curve = nativeArray8[index];
              Owner owner = nativeArray9[index];
              GarageLane garageLane = nativeArray6[index];
              Game.Net.ConnectionLane connectionLane = nativeArray10[index];
              connectionLane.m_Flags &= ~(ConnectionLaneFlags.Disabled | ConnectionLaneFlags.AllowEnter | ConnectionLaneFlags.AllowExit);
              bool disabled;
              bool allowEnter;
              bool allowExit;
              // ISSUE: reference to a compiler-generated method
              this.GetParkingStats(owner, new Game.Net.ParkingLane(), out connectionLane.m_AccessRestriction, out garageLane.m_VehicleCapacity, out garageLane.m_ParkingFee, out garageLane.m_ComfortFactor, out disabled, out allowEnter, out allowExit);
              // ISSUE: reference to a compiler-generated method
              garageLane.m_VehicleCount = this.CountVehicles(entity, owner, curve, connectionLane);
              if (disabled)
                connectionLane.m_Flags |= ConnectionLaneFlags.Disabled;
              if (allowEnter)
                connectionLane.m_Flags |= ConnectionLaneFlags.AllowEnter;
              if (allowExit)
                connectionLane.m_Flags |= ConnectionLaneFlags.AllowExit;
              nativeArray6[index] = garageLane;
              nativeArray10[index] = connectionLane;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Objects.SpawnLocation> nativeArray11 = chunk.GetNativeArray<Game.Objects.SpawnLocation>(ref this.m_SpawnLocationType);
            if (nativeArray11.Length == 0)
              return;
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray12 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Objects.Transform> nativeArray13 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray14 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            for (int index = 0; index < nativeArray11.Length; ++index)
            {
              Game.Prefabs.SpawnLocationData componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabSpawnLocationData.TryGetComponent(nativeArray14[index].m_Prefab, out componentData) && ((componentData.m_RoadTypes & RoadTypes.Helicopter) != RoadTypes.None && componentData.m_ConnectionType == RouteConnectionType.Air || componentData.m_ConnectionType == RouteConnectionType.Track))
              {
                Entity entity = nativeArray12[index];
                Game.Objects.SpawnLocation spawnLocation = nativeArray11[index];
                Game.Objects.Transform transform = nativeArray13[index];
                // ISSUE: reference to a compiler-generated method
                if (this.CountVehicles(entity, transform, spawnLocation, componentData) != (ushort) 0)
                  spawnLocation.m_Flags |= SpawnLocationFlags.ParkedVehicle;
                else
                  spawnLocation.m_Flags &= ~SpawnLocationFlags.ParkedVehicle;
                nativeArray11[index] = spawnLocation;
              }
            }
          }
        }
      }

      private Bounds1 GetBlockedRange(Owner owner, Lane laneData)
      {
        Bounds1 blockedRange = new Bounds1(2f, -1f);
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubLanes.HasBuffer(owner.m_Owner))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[owner.m_Owner];
          for (int index = 0; index < subLane1.Length; ++index)
          {
            Entity subLane2 = subLane1[index].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            Lane lane = this.m_LaneData[subLane2];
            // ISSUE: reference to a compiler-generated field
            if (laneData.m_StartNode.EqualsIgnoreCurvePos(lane.m_MiddleNode) && this.m_CarLaneData.HasComponent(subLane2))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Net.CarLane carLane = this.m_CarLaneData[subLane2];
              if ((int) carLane.m_BlockageEnd >= (int) carLane.m_BlockageStart)
              {
                Bounds1 blockageBounds = carLane.blockageBounds;
                blockageBounds.min = math.select(blockageBounds.min - 0.01f, 0.0f, (double) blockageBounds.min <= 0.50999999046325684);
                blockageBounds.max = math.select(blockageBounds.max + 0.01f, 1f, (double) blockageBounds.max >= 0.49000000953674316);
                blockedRange |= blockageBounds;
              }
            }
          }
        }
        return blockedRange;
      }

      private void GetParkingStats(
        Owner owner,
        Game.Net.ParkingLane parkingLane,
        out Entity restriction,
        out ushort garageCapacity,
        out ushort fee,
        out ushort comfort,
        out bool disabled,
        out bool allowEnter,
        out bool allowExit)
      {
        restriction = Entity.Null;
        garageCapacity = (ushort) 0;
        fee = (ushort) 0;
        comfort = (ushort) 0;
        disabled = false;
        allowEnter = false;
        allowExit = false;
        Owner owner1 = owner;
        Owner componentData1;
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.TryGetComponent(owner1.m_Owner, out componentData1))
          owner1 = componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_BuildingData.HasComponent(owner1.m_Owner))
        {
          ParkingFacilityData parkingFacilityData = new ParkingFacilityData();
          bool flag = false;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef1 = this.m_PrefabRefData[owner1.m_Owner];
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabParkingFacilityData.HasComponent(prefabRef1.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            parkingFacilityData = this.m_PrefabParkingFacilityData[prefabRef1.m_Prefab];
            flag = true;
          }
          DynamicBuffer<InstalledUpgrade> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_InstalledUpgrades.TryGetBuffer(owner1.m_Owner, out bufferData))
          {
            for (int index = 0; index < bufferData.Length; ++index)
            {
              InstalledUpgrade installedUpgrade = bufferData[index];
              if (!BuildingUtils.CheckOption(installedUpgrade, BuildingOption.Inactive))
              {
                // ISSUE: reference to a compiler-generated field
                PrefabRef prefabRef2 = this.m_PrefabRefData[installedUpgrade.m_Upgrade];
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabParkingFacilityData.HasComponent(prefabRef2.m_Prefab))
                {
                  // ISSUE: reference to a compiler-generated field
                  parkingFacilityData.Combine(this.m_PrefabParkingFacilityData[prefabRef2.m_Prefab]);
                  flag = true;
                }
              }
            }
          }
          Entity entity = owner1.m_Owner;
          Attachment componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_AttachmentData.TryGetComponent(entity, out componentData2) && componentData2.m_Attached != Entity.Null)
            entity = componentData2.m_Attached;
          PrefabRef componentData3;
          Game.Prefabs.BuildingData componentData4;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.TryGetComponent(entity, out componentData3) && this.m_PrefabBuildingData.TryGetComponent(componentData3.m_Prefab, out componentData4))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_RoadData.HasComponent(owner.m_Owner))
              componentData4.m_Flags &= ~(Game.Prefabs.BuildingFlags.RestrictedPedestrian | Game.Prefabs.BuildingFlags.RestrictedCar);
            restriction = entity;
            allowEnter = (componentData4.m_Flags & (Game.Prefabs.BuildingFlags.RestrictedPedestrian | Game.Prefabs.BuildingFlags.RestrictedCar)) == (Game.Prefabs.BuildingFlags) 0;
            allowExit = (componentData4.m_Flags & Game.Prefabs.BuildingFlags.RestrictedParking) == (Game.Prefabs.BuildingFlags) 0;
          }
          PrefabRef componentData5;
          NetGeometryData componentData6;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.TryGetComponent(owner.m_Owner, out componentData5) && this.m_PrefabGeometryData.TryGetComponent(componentData5.m_Prefab, out componentData6) && (componentData6.m_Flags & Game.Net.GeometryFlags.SubOwner) != (Game.Net.GeometryFlags) 0)
          {
            restriction = Entity.Null;
            allowEnter = false;
            allowExit = false;
          }
          if (!flag)
          {
            parkingFacilityData.m_GarageMarkerCapacity = 2;
            parkingFacilityData.m_ComfortFactor = 0.8f;
            BuildingPropertyData componentData7;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabBuildingPropertyData.TryGetComponent(prefabRef1.m_Prefab, out componentData7))
            {
              parkingFacilityData.m_GarageMarkerCapacity = math.max(1, Mathf.RoundToInt(componentData7.m_SpaceMultiplier));
            }
            else
            {
              WorkplaceData componentData8;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabWorkplaceData.TryGetComponent(prefabRef1.m_Prefab, out componentData8))
                parkingFacilityData.m_GarageMarkerCapacity = math.max(2, componentData8.m_MaxWorkers / 20);
            }
          }
          Game.Buildings.ParkingFacility componentData9;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ParkingFacilityData.TryGetComponent(owner1.m_Owner, out componentData9))
          {
            disabled = (componentData9.m_Flags & ParkingFacilityFlags.ParkingSpacesActive) == (ParkingFacilityFlags) 0 && (parkingLane.m_Flags & ParkingLaneFlags.VirtualLane) == (ParkingLaneFlags) 0;
            parkingFacilityData.m_ComfortFactor = componentData9.m_ComfortFactor;
          }
          garageCapacity = (ushort) math.clamp(parkingFacilityData.m_GarageMarkerCapacity, 0, (int) ushort.MaxValue);
          comfort = (ushort) math.clamp(Mathf.RoundToInt(parkingFacilityData.m_ComfortFactor * (float) ushort.MaxValue), 0, (int) ushort.MaxValue);
          // ISSUE: reference to a compiler-generated method
          fee = this.GetBuildingParkingFee(owner1.m_Owner);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_BorderDistrictData.HasComponent(owner.m_Owner))
            return;
          // ISSUE: reference to a compiler-generated field
          BorderDistrict borderDistrict = this.m_BorderDistrictData[owner.m_Owner];
          if ((parkingLane.m_Flags & ParkingLaneFlags.RightSide) != (ParkingLaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            fee = this.GetDistrictParkingFee(borderDistrict.m_Right);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            fee = this.GetDistrictParkingFee(borderDistrict.m_Left);
          }
        }
      }

      private ushort GetDistrictParkingFee(Entity district)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_DistrictData.HasComponent(district) || !AreaUtils.CheckOption(this.m_DistrictData[district], DistrictOption.PaidParking))
          return 0;
        float f = 0.0f;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<DistrictModifier> districtModifier = this.m_DistrictModifiers[district];
        AreaUtils.ApplyModifier(ref f, districtModifier, DistrictModifierType.ParkingFee);
        return (ushort) math.clamp(Mathf.RoundToInt(f), 0, (int) ushort.MaxValue);
      }

      private ushort GetBuildingParkingFee(Entity building)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_BuildingData.HasComponent(building) || !BuildingUtils.CheckOption(this.m_BuildingData[building], BuildingOption.PaidParking))
          return 0;
        float f = 0.0f;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<BuildingModifier> buildingModifier = this.m_BuildingModifiers[building];
        BuildingUtils.ApplyModifier(ref f, buildingModifier, BuildingModifierType.ParkingFee);
        return (ushort) math.clamp(Mathf.RoundToInt(f), 0, (int) ushort.MaxValue);
      }

      private ushort CountVehicles(
        Entity entity,
        Owner owner,
        Curve curve,
        Game.Net.ConnectionLane connectionLane)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ParkingLaneDataSystem.UpdateLaneDataJob.CountVehiclesIterator iterator = new ParkingLaneDataSystem.UpdateLaneDataJob.CountVehiclesIterator()
        {
          m_Lane = entity,
          m_Bounds = VehicleUtils.GetConnectionParkingBounds(connectionLane, curve.m_Bezier),
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
          Game.Objects.Transform transform = this.m_TransformData[owner1.m_Owner];
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
        this.m_MovingObjectSearchTree.Iterate<ParkingLaneDataSystem.UpdateLaneDataJob.CountVehiclesIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        return (ushort) math.clamp(iterator.m_Result, 0, (int) ushort.MaxValue);
      }

      private ushort CountVehicles(
        Entity entity,
        Game.Objects.Transform transform,
        Game.Objects.SpawnLocation spawnLocation,
        Game.Prefabs.SpawnLocationData spawnLocationData)
      {
        switch (spawnLocationData.m_ConnectionType)
        {
          case RouteConnectionType.Track:
            int x = 0;
            DynamicBuffer<LaneObject> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneObjects.TryGetBuffer(spawnLocation.m_ConnectedLane1, out bufferData))
            {
              for (int index = 0; index < bufferData.Length; ++index)
              {
                ParkedTrain componentData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_ParkedTrainData.TryGetComponent(bufferData[index].m_LaneObject, out componentData) && componentData.m_ParkingLocation == entity)
                  ++x;
              }
            }
            return (ushort) math.clamp(x, 0, (int) ushort.MaxValue);
          case RouteConnectionType.Air:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            ParkingLaneDataSystem.UpdateLaneDataJob.CountVehiclesIterator iterator = new ParkingLaneDataSystem.UpdateLaneDataJob.CountVehiclesIterator()
            {
              m_Lane = entity,
              m_Bounds = new Bounds3(transform.m_Position - 1f, transform.m_Position + 1f),
              m_ParkedCarData = this.m_ParkedCarData,
              m_ControllerData = this.m_ControllerData
            };
            // ISSUE: reference to a compiler-generated field
            this.m_MovingObjectSearchTree.Iterate<ParkingLaneDataSystem.UpdateLaneDataJob.CountVehiclesIterator>(ref iterator);
            // ISSUE: reference to a compiler-generated field
            return (ushort) math.clamp(iterator.m_Result, 0, (int) ushort.MaxValue);
          default:
            return 0;
        }
      }

      private float CalculateFreeSpace(
        Curve curve,
        Game.Net.ParkingLane parkingLane,
        ParkingLaneData parkingLaneData,
        DynamicBuffer<LaneObject> laneObjects,
        DynamicBuffer<LaneOverlap> laneOverlaps,
        Bounds1 blockedRange)
      {
        if ((parkingLane.m_Flags & ParkingLaneFlags.VirtualLane) != (ParkingLaneFlags) 0)
          return 0.0f;
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
          float num4 = 2f;
          int num5 = 0;
          while (num5 < laneObjects.Length)
          {
            LaneObject laneObject = laneObjects[num5++];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ParkedCarData.HasComponent(laneObject.m_LaneObject) && !this.m_UnspawnedData.HasComponent(laneObject.m_LaneObject))
            {
              num4 = laneObject.m_CurvePosition.x;
              break;
            }
          }
          float2 float2_2 = (float2) 2f;
          int num6 = 0;
          if (num6 < laneOverlaps.Length)
          {
            LaneOverlap laneOverlap = laneOverlaps[num6++];
            float2_2 = new float2((float) laneOverlap.m_ThisStart, (float) laneOverlap.m_ThisEnd) * 0.003921569f;
          }
          for (int index = 1; index <= 16; ++index)
          {
            float num7 = (float) index * (1f / 16f);
            float3 y = MathUtils.Position(curve.m_Bezier, num7);
            for (num1 += math.distance(x1, y); (double) num1 >= (double) num2 || index == 16 && num3 < parkingSlotCount; ++num3)
            {
              float2_1.y = math.select(num7, math.lerp(float2_1.x, num7, num2 / num1), (double) num2 < (double) num1);
              bool flag = false;
              if ((double) num4 <= (double) float2_1.y)
              {
                num4 = 2f;
                flag = true;
                while (num5 < laneObjects.Length)
                {
                  LaneObject laneObject = laneObjects[num5++];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ParkedCarData.HasComponent(laneObject.m_LaneObject) && !this.m_UnspawnedData.HasComponent(laneObject.m_LaneObject) && (double) laneObject.m_CurvePosition.x > (double) float2_1.y)
                  {
                    num4 = laneObject.m_CurvePosition.x;
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
                  while (num6 < laneOverlaps.Length)
                  {
                    LaneOverlap laneOverlap = laneOverlaps[num6++];
                    float2 float2_3 = new float2((float) laneOverlap.m_ThisStart, (float) laneOverlap.m_ThisEnd) * 0.003921569f;
                    if ((double) float2_3.y > (double) float2_1.y)
                    {
                      float2_2 = float2_3;
                      break;
                    }
                  }
                }
              }
              if (!flag && num3 >= 0 && num3 < parkingSlotCount && ((double) float2_1.x > (double) blockedRange.max || (double) float2_1.y < (double) blockedRange.min))
                return parkingLaneData.m_MaxCarLength;
              num1 -= num2;
              float2_1.x = float2_1.y;
              num2 = parkingSlotInterval;
            }
            x1 = y;
          }
          return 0.0f;
        }
        float x3 = 0.0f;
        float2 x4 = (float2) math.select(0.0f, 0.5f, (parkingLane.m_Flags & ParkingLaneFlags.StartingLane) == (ParkingLaneFlags) 0);
        float3 x5 = curve.m_Bezier.a;
        float num8 = 2f;
        float2 float2_4 = (float2) 0.0f;
        int num9 = 0;
        while (num9 < laneObjects.Length)
        {
          LaneObject laneObject = laneObjects[num9++];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ParkedCarData.HasComponent(laneObject.m_LaneObject) && !this.m_UnspawnedData.HasComponent(laneObject.m_LaneObject))
          {
            num8 = laneObject.m_CurvePosition.x;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float2_4 = VehicleUtils.GetParkingOffsets(laneObject.m_LaneObject, ref this.m_PrefabRefData, ref this.m_ObjectGeometryData) + 1f;
            break;
          }
        }
        float2 float2_5 = (float2) 2f;
        int num10 = 0;
        if (num10 < laneOverlaps.Length)
        {
          LaneOverlap laneOverlap = laneOverlaps[num10++];
          float2_5 = new float2((float) laneOverlap.m_ThisStart, (float) laneOverlap.m_ThisEnd) * 0.003921569f;
        }
        float3 y1 = new float3();
        float3 float3_1 = new float3();
        if ((double) blockedRange.max >= (double) blockedRange.min)
        {
          y1 = MathUtils.Position(curve.m_Bezier, MathUtils.Center(blockedRange));
          float3_1.x = math.distance(MathUtils.Position(curve.m_Bezier, blockedRange.min), y1);
          float3_1.y = math.distance(MathUtils.Position(curve.m_Bezier, blockedRange.max), y1);
        }
        while ((double) num8 != 2.0 || (double) float2_5.x != 2.0)
        {
          float2 float2_6;
          float num11;
          if ((double) num8 <= (double) float2_5.x)
          {
            float2_6 = (float2) num8;
            x4.y = float2_4.x;
            num11 = float2_4.y;
            num8 = 2f;
            while (num9 < laneObjects.Length)
            {
              LaneObject laneObject = laneObjects[num9++];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_ParkedCarData.HasComponent(laneObject.m_LaneObject) && !this.m_UnspawnedData.HasComponent(laneObject.m_LaneObject))
              {
                num8 = laneObject.m_CurvePosition.x;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float2_4 = VehicleUtils.GetParkingOffsets(laneObject.m_LaneObject, ref this.m_PrefabRefData, ref this.m_ObjectGeometryData) + 1f;
                break;
              }
            }
          }
          else
          {
            float2_6 = float2_5;
            x4.y = 0.5f;
            num11 = 0.5f;
            float2_5 = (float2) 2f;
            while (num10 < laneOverlaps.Length)
            {
              LaneOverlap laneOverlap = laneOverlaps[num10++];
              float2 float2_7 = new float2((float) laneOverlap.m_ThisStart, (float) laneOverlap.m_ThisEnd) * 0.003921569f;
              if ((double) float2_7.x <= (double) float2_6.y)
              {
                float2_6.y = math.max(float2_6.y, float2_7.y);
              }
              else
              {
                float2_5 = float2_7;
                break;
              }
            }
          }
          float3 float3_2 = MathUtils.Position(curve.m_Bezier, float2_6.x);
          float num12 = math.distance(x5, float3_2) - math.csum(x4);
          if ((double) blockedRange.max >= (double) blockedRange.min)
          {
            float x6 = math.distance(x5, y1) - x4.x - float3_1.x;
            float y2 = math.distance(float3_2, y1) - x4.y - float3_1.y;
            num12 = math.min(num12, math.max(x6, y2));
          }
          x3 = math.max(x3, num12);
          x4.x = num11;
          x5 = MathUtils.Position(curve.m_Bezier, float2_6.y);
        }
        x4.y = math.select(0.0f, 0.5f, (parkingLane.m_Flags & ParkingLaneFlags.EndingLane) == (ParkingLaneFlags) 0);
        float num13 = math.distance(x5, curve.m_Bezier.d) - math.csum(x4);
        if ((double) blockedRange.max >= (double) blockedRange.min)
        {
          float x7 = math.distance(x5, y1) - x4.x - float3_1.x;
          float y3 = math.distance(curve.m_Bezier.d, y1) - x4.y - float3_1.y;
          num13 = math.min(num13, math.max(x7, y3));
        }
        float num14 = math.max(x3, num13);
        return math.select(num14, math.min(num14, parkingLaneData.m_MaxCarLength), (double) parkingLaneData.m_MaxCarLength != 0.0);
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

      private struct CountVehiclesIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Entity m_Lane;
        public Bounds3 m_Bounds;
        public int m_Result;
        public ComponentLookup<ParkedCar> m_ParkedCarData;
        public ComponentLookup<Controller> m_ControllerData;

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
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || this.m_ControllerData.TryGetComponent(entity, out componentData1) && componentData1.m_Controller != entity || !this.m_ParkedCarData.TryGetComponent(entity, out componentData2) || !(componentData2.m_Lane == this.m_Lane))
            return;
          // ISSUE: reference to a compiler-generated field
          ++this.m_Result;
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Lane> __Game_Net_Lane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Net.ParkingLane> __Game_Net_ParkingLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<GarageLane> __Game_Net_GarageLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RW_ComponentTypeHandle;
      public BufferTypeHandle<LaneObject> __Game_Net_LaneObject_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Road> __Game_Net_Road_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> __Game_Vehicles_ParkedTrain_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> __Game_Areas_BorderDistrict_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<District> __Game_Areas_District_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ParkingFacility> __Game_Buildings_ParkingFacility_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.City.City> __Game_City_City_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attachment> __Game_Objects_Attachment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingFacilityData> __Game_Prefabs_ParkingFacilityData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> __Game_Prefabs_WorkplaceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<DistrictModifier> __Game_Areas_DistrictModifier_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<BuildingModifier> __Game_Buildings_BuildingModifier_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> __Game_Prefabs_ActivityLocationElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferTypeHandle = state.GetBufferTypeHandle<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.ParkingLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.ConnectionLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_GarageLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<GarageLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.SpawnLocation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RW_BufferTypeHandle = state.GetBufferTypeHandle<LaneObject>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentLookup = state.GetComponentLookup<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedTrain_RO_ComponentLookup = state.GetComponentLookup<ParkedTrain>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_BorderDistrict_RO_ComponentLookup = state.GetComponentLookup<BorderDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_District_RO_ComponentLookup = state.GetComponentLookup<District>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ParkingFacility_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.ParkingFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_City_RO_ComponentLookup = state.GetComponentLookup<Game.City.City>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attachment_RO_ComponentLookup = state.GetComponentLookup<Attachment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingFacilityData_RO_ComponentLookup = state.GetComponentLookup<ParkingFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkplaceData_RO_ComponentLookup = state.GetComponentLookup<WorkplaceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_DistrictModifier_RO_BufferLookup = state.GetBufferLookup<DistrictModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_BuildingModifier_RO_BufferLookup = state.GetBufferLookup<BuildingModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup = state.GetBufferLookup<ActivityLocationElement>(true);
      }
    }
  }
}
