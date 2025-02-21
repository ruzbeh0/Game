// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AnimalNavigationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Rendering;
using Game.Routes;
using Game.Tools;
using Game.Vehicles;
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
  public class AnimalNavigationSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private AnimalNavigationSystem.Actions m_Actions;
    private EntityQuery m_CreatureQuery;
    private AnimalNavigationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_Actions = this.World.GetOrCreateSystemManaged<AnimalNavigationSystem.Actions>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatureQuery = this.GetEntityQuery(ComponentType.ReadOnly<Animal>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadWrite<AnimalCurrentLane>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint index = this.m_SimulationSystem.frameIndex % 16U;
      switch (index)
      {
        case 5:
        case 9:
        case 13:
          // ISSUE: reference to a compiler-generated field
          this.m_CreatureQuery.ResetFilter();
          // ISSUE: reference to a compiler-generated field
          this.m_CreatureQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(index));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_AnimationMotion_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_AnimationClip_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_CharacterElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_AnimalData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_CreatureData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Pathfind_PathOwner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Areas_HangaroundLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Creatures_Animal_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Net_AreaLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Net_LaneReservation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Vehicles_Blocker_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Creatures_AnimalNavigation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Creatures_Animal_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Objects_TripSource_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Creatures_Stumbling_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Objects_Moving_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
          JobHandle deps;
          JobHandle dependencies1;
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
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
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
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          JobHandle jobHandle = new AnimalNavigationSystem.UpdateNavigationJob()
          {
            m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
            m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
            m_MovingType = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentTypeHandle,
            m_GroupMemberType = this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentTypeHandle,
            m_StumblingType = this.__TypeHandle.__Game_Creatures_Stumbling_RO_ComponentTypeHandle,
            m_TripSourceType = this.__TypeHandle.__Game_Objects_TripSource_RO_ComponentTypeHandle,
            m_AnimalType = this.__TypeHandle.__Game_Creatures_Animal_RO_ComponentTypeHandle,
            m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
            m_MeshGroupType = this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferTypeHandle,
            m_NavigationType = this.__TypeHandle.__Game_Creatures_AnimalNavigation_RW_ComponentTypeHandle,
            m_BlockerType = this.__TypeHandle.__Game_Vehicles_Blocker_RW_ComponentTypeHandle,
            m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
            m_HumanCurrentLaneData = this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentLookup,
            m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
            m_PedestrianLaneData = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup,
            m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
            m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
            m_LaneReservationData = this.__TypeHandle.__Game_Net_LaneReservation_RO_ComponentLookup,
            m_AreaLaneData = this.__TypeHandle.__Game_Net_AreaLane_RO_ComponentLookup,
            m_PositionData = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
            m_TaxiStandData = this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentLookup,
            m_PropertyRenterData = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
            m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
            m_MovingData = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup,
            m_CreatureData = this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup,
            m_AnimalData = this.__TypeHandle.__Game_Creatures_Animal_RO_ComponentLookup,
            m_GroupMemberData = this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentLookup,
            m_HangaroundLocationData = this.__TypeHandle.__Game_Areas_HangaroundLocation_RO_ComponentLookup,
            m_PublicTransportData = this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup,
            m_TrainData = this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup,
            m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
            m_PathOwnerData = this.__TypeHandle.__Game_Pathfind_PathOwner_RO_ComponentLookup,
            m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
            m_PrefabCreatureData = this.__TypeHandle.__Game_Prefabs_CreatureData_RO_ComponentLookup,
            m_PrefabAnimalData = this.__TypeHandle.__Game_Prefabs_AnimalData_RO_ComponentLookup,
            m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
            m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
            m_PrefabLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
            m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
            m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
            m_Lanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
            m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
            m_LaneOverlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
            m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
            m_AreaTriangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
            m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup,
            m_PrefabActivityLocations = this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup,
            m_SubMeshGroups = this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup,
            m_CharacterElements = this.__TypeHandle.__Game_Prefabs_CharacterElement_RO_BufferLookup,
            m_AnimationClips = this.__TypeHandle.__Game_Prefabs_AnimationClip_RO_BufferLookup,
            m_AnimationMotions = this.__TypeHandle.__Game_Prefabs_AnimationMotion_RO_BufferLookup,
            m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
            m_AnimalCurrentLaneData = this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentLookup,
            m_RandomSeed = RandomSeed.Next(),
            m_LeftHandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
            m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
            m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
            m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies1),
            m_StaticObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies2),
            m_MovingObjectSearchTree = this.m_ObjectSearchSystem.GetMovingSearchTree(true, out dependencies3),
            m_AreaSearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies4),
            m_LaneObjectBuffer = this.m_Actions.m_LaneObjectUpdater.Begin(Allocator.TempJob)
          }.ScheduleParallel<AnimalNavigationSystem.UpdateNavigationJob>(this.m_CreatureQuery, JobUtils.CombineDependencies(this.Dependency, dependencies1, dependencies2, dependencies3, dependencies4, deps));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_WaterSystem.AddSurfaceReader(jobHandle);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_ObjectSearchSystem.AddMovingSearchTreeReader(jobHandle);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Actions.m_Dependency = jobHandle;
          this.Dependency = jobHandle;
          break;
      }
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
    public AnimalNavigationSystem()
    {
    }

    public class Actions : GameSystemBase
    {
      private SimulationSystem m_SimulationSystem;
      public LaneObjectUpdater m_LaneObjectUpdater;
      public JobHandle m_Dependency;

      [UnityEngine.Scripting.Preserve]
      protected override void OnCreate()
      {
        base.OnCreate();
        // ISSUE: reference to a compiler-generated field
        this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
        // ISSUE: reference to a compiler-generated field
        this.m_LaneObjectUpdater = new LaneObjectUpdater((SystemBase) this);
      }

      [UnityEngine.Scripting.Preserve]
      protected override void OnUpdate()
      {
        // ISSUE: reference to a compiler-generated field
        switch (this.m_SimulationSystem.frameIndex % 16U)
        {
          case 5:
          case 9:
          case 13:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.Dependency = this.m_LaneObjectUpdater.Apply((SystemBase) this, JobHandle.CombineDependencies(this.Dependency, this.m_Dependency));
            break;
        }
      }

      [UnityEngine.Scripting.Preserve]
      public Actions()
      {
      }
    }

    [BurstCompile]
    private struct GroupNavigationJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<GroupMember> m_GroupMemberType;
      [ReadOnly]
      public ComponentLookup<HumanCurrentLane> m_HumanCurrentLaneData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<AnimalCurrentLane> m_AnimalCurrentLaneData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<GroupMember> nativeArray1 = chunk.GetNativeArray<GroupMember>(ref this.m_GroupMemberType);
        if (nativeArray1.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.m_EntityType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity entity = nativeArray2[index];
          GroupMember groupMember = nativeArray1[index];
          // ISSUE: reference to a compiler-generated field
          AnimalCurrentLane animalCurrentLane = this.m_AnimalCurrentLaneData[entity];
          HumanCurrentLane componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_HumanCurrentLaneData.TryGetComponent(groupMember.m_Leader, out componentData1))
          {
            if (componentData1.m_Lane != animalCurrentLane.m_Lane)
            {
              if (componentData1.m_Lane != animalCurrentLane.m_NextLane)
              {
                animalCurrentLane.m_NextLane = componentData1.m_Lane;
                animalCurrentLane.m_NextPosition = componentData1.m_CurvePosition;
              }
              else
                animalCurrentLane.m_NextPosition.y = componentData1.m_CurvePosition.y;
              animalCurrentLane.m_NextFlags = componentData1.m_Flags & ~(CreatureLaneFlags.EndOfPath | CreatureLaneFlags.EndReached | CreatureLaneFlags.Obsolete | CreatureLaneFlags.WaitSignal);
            }
            else
            {
              if ((double) animalCurrentLane.m_CurvePosition.y != (double) componentData1.m_CurvePosition.y)
              {
                animalCurrentLane.m_CurvePosition.y = componentData1.m_CurvePosition.y;
                animalCurrentLane.m_Flags = componentData1.m_Flags & ~(CreatureLaneFlags.EndOfPath | CreatureLaneFlags.EndReached | CreatureLaneFlags.Obsolete | CreatureLaneFlags.WaitSignal);
              }
              animalCurrentLane.m_NextLane = Entity.Null;
            }
          }
          else
          {
            AnimalCurrentLane componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_AnimalCurrentLaneData.TryGetComponent(groupMember.m_Leader, out componentData2))
            {
              if (componentData2.m_Lane != animalCurrentLane.m_Lane)
              {
                if (componentData2.m_Lane != animalCurrentLane.m_NextLane)
                {
                  animalCurrentLane.m_NextLane = componentData2.m_Lane;
                  animalCurrentLane.m_NextPosition = componentData2.m_CurvePosition;
                }
                else
                  animalCurrentLane.m_NextPosition.y = componentData2.m_CurvePosition.y;
                animalCurrentLane.m_NextFlags = componentData2.m_Flags & ~(CreatureLaneFlags.EndOfPath | CreatureLaneFlags.EndReached | CreatureLaneFlags.Obsolete | CreatureLaneFlags.WaitSignal);
              }
              else
              {
                if ((double) animalCurrentLane.m_CurvePosition.y != (double) componentData2.m_CurvePosition.y)
                {
                  animalCurrentLane.m_CurvePosition.y = componentData2.m_CurvePosition.y;
                  animalCurrentLane.m_Flags = componentData2.m_Flags & ~(CreatureLaneFlags.EndOfPath | CreatureLaneFlags.EndReached | CreatureLaneFlags.Obsolete | CreatureLaneFlags.WaitSignal);
                }
                animalCurrentLane.m_NextLane = Entity.Null;
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_AnimalCurrentLaneData[entity] = animalCurrentLane;
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
    private struct UpdateNavigationJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Moving> m_MovingType;
      [ReadOnly]
      public ComponentTypeHandle<GroupMember> m_GroupMemberType;
      [ReadOnly]
      public ComponentTypeHandle<Stumbling> m_StumblingType;
      [ReadOnly]
      public ComponentTypeHandle<TripSource> m_TripSourceType;
      [ReadOnly]
      public ComponentTypeHandle<Animal> m_AnimalType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<MeshGroup> m_MeshGroupType;
      public ComponentTypeHandle<AnimalNavigation> m_NavigationType;
      public ComponentTypeHandle<Blocker> m_BlockerType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<HumanCurrentLane> m_HumanCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> m_PedestrianLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneReservation> m_LaneReservationData;
      [ReadOnly]
      public ComponentLookup<AreaLane> m_AreaLaneData;
      [ReadOnly]
      public ComponentLookup<Position> m_PositionData;
      [ReadOnly]
      public ComponentLookup<TaxiStand> m_TaxiStandData;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenterData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Moving> m_MovingData;
      [ReadOnly]
      public ComponentLookup<Creature> m_CreatureData;
      [ReadOnly]
      public ComponentLookup<Animal> m_AnimalData;
      [ReadOnly]
      public ComponentLookup<GroupMember> m_GroupMemberData;
      [ReadOnly]
      public ComponentLookup<HangaroundLocation> m_HangaroundLocationData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
      [ReadOnly]
      public ComponentLookup<Train> m_TrainData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<PathOwner> m_PathOwnerData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<CreatureData> m_PrefabCreatureData;
      [ReadOnly]
      public ComponentLookup<AnimalData> m_PrefabAnimalData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_Lanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<LaneOverlap> m_LaneOverlaps;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_AreaTriangles;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> m_PrefabActivityLocations;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> m_SubMeshGroups;
      [ReadOnly]
      public BufferLookup<CharacterElement> m_CharacterElements;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.AnimationClip> m_AnimationClips;
      [ReadOnly]
      public BufferLookup<AnimationMotion> m_AnimationMotions;
      [ReadOnly]
      public BufferLookup<SubMesh> m_SubMeshes;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<AnimalCurrentLane> m_AnimalCurrentLaneData;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public bool m_LeftHandTraffic;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_StaticObjectSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_MovingObjectSearchTree;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      public LaneObjectCommandBuffer m_LaneObjectBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Transform> nativeArray2 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Moving> nativeArray3 = chunk.GetNativeArray<Moving>(ref this.m_MovingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<GroupMember> nativeArray4 = chunk.GetNativeArray<GroupMember>(ref this.m_GroupMemberType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Animal> nativeArray5 = chunk.GetNativeArray<Animal>(ref this.m_AnimalType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AnimalNavigation> nativeArray6 = chunk.GetNativeArray<AnimalNavigation>(ref this.m_NavigationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Blocker> nativeArray7 = chunk.GetNativeArray<Blocker>(ref this.m_BlockerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray8 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<MeshGroup> bufferAccessor = chunk.GetBufferAccessor<MeshGroup>(ref this.m_MeshGroupType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Stumbling>(ref this.m_StumblingType))
        {
          for (int index = 0; index < chunk.Count; ++index)
          {
            Entity entity = nativeArray1[index];
            Game.Objects.Transform transform = nativeArray2[index];
            Animal animal = nativeArray5[index];
            AnimalNavigation navigation = nativeArray6[index];
            Blocker blocker = nativeArray7[index];
            PrefabRef prefabRef = nativeArray8[index];
            // ISSUE: reference to a compiler-generated field
            AnimalCurrentLane currentLane = this.m_AnimalCurrentLaneData[entity];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
            Moving moving;
            CollectionUtils.TryGet<Moving>(nativeArray3, index, out moving);
            GroupMember groupMember;
            CollectionUtils.TryGet<GroupMember>(nativeArray4, index, out groupMember);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            AnimalNavigationHelpers.CurrentLaneCache currentLaneCache = new AnimalNavigationHelpers.CurrentLaneCache(ref currentLane, this.m_PrefabRefData, this.m_MovingObjectSearchTree);
            // ISSUE: reference to a compiler-generated method
            this.UpdateStumbling(entity, transform, groupMember, animal, objectGeometryData, ref navigation, ref currentLane, ref blocker);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            currentLaneCache.CheckChanges(entity, ref currentLane, this.m_LaneObjectBuffer, this.m_LaneObjects, transform, moving, navigation, objectGeometryData);
            nativeArray6[index] = navigation;
            nativeArray7[index] = blocker;
            // ISSUE: reference to a compiler-generated field
            this.m_AnimalCurrentLaneData[entity] = currentLane;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<TripSource> nativeArray9 = chunk.GetNativeArray<TripSource>(ref this.m_TripSourceType);
          for (int index = 0; index < chunk.Count; ++index)
          {
            Entity entity = nativeArray1[index];
            Game.Objects.Transform transform = nativeArray2[index];
            Moving moving = nativeArray3[index];
            Animal animal = nativeArray5[index];
            AnimalNavigation navigation = nativeArray6[index];
            Blocker blocker = nativeArray7[index];
            PrefabRef prefabRef = nativeArray8[index];
            // ISSUE: reference to a compiler-generated field
            AnimalCurrentLane currentLane = this.m_AnimalCurrentLaneData[entity];
            // ISSUE: reference to a compiler-generated field
            CreatureData prefabCreatureData = this.m_PrefabCreatureData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            AnimalData prefabAnimalData = this.m_PrefabAnimalData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
            GroupMember groupMember;
            CollectionUtils.TryGet<GroupMember>(nativeArray4, index, out groupMember);
            TripSource tripSource;
            CollectionUtils.TryGet<TripSource>(nativeArray9, index, out tripSource);
            DynamicBuffer<MeshGroup> meshGroups;
            CollectionUtils.TryGet<MeshGroup>(bufferAccessor, index, out meshGroups);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            AnimalNavigationHelpers.CurrentLaneCache currentLaneCache = new AnimalNavigationHelpers.CurrentLaneCache(ref currentLane, this.m_PrefabRefData, this.m_MovingObjectSearchTree);
            if (currentLane.m_Lane == Entity.Null || (currentLane.m_Flags & CreatureLaneFlags.Obsolete) != (CreatureLaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.TryFindCurrentLane(ref currentLane, transform, animal);
            }
            // ISSUE: reference to a compiler-generated method
            this.UpdateNavigationTarget(ref random, entity, transform, moving, tripSource, groupMember, animal, prefabRef, prefabCreatureData, prefabAnimalData, objectGeometryData, ref navigation, ref currentLane, ref blocker, meshGroups);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            currentLaneCache.CheckChanges(entity, ref currentLane, this.m_LaneObjectBuffer, this.m_LaneObjects, transform, moving, navigation, objectGeometryData);
            nativeArray6[index] = navigation;
            nativeArray7[index] = blocker;
            // ISSUE: reference to a compiler-generated field
            this.m_AnimalCurrentLaneData[entity] = currentLane;
          }
        }
      }

      private void UpdateStumbling(
        Entity entity,
        Game.Objects.Transform transform,
        GroupMember groupMember,
        Animal animal,
        ObjectGeometryData prefabObjectGeometryData,
        ref AnimalNavigation navigation,
        ref AnimalCurrentLane currentLane,
        ref Blocker blocker)
      {
        // ISSUE: reference to a compiler-generated method
        this.TryFindCurrentLane(ref currentLane, transform, animal);
        navigation = new AnimalNavigation()
        {
          m_TargetPosition = transform.m_Position
        };
        blocker = new Blocker();
      }

      private void TryFindCurrentLane(
        ref AnimalCurrentLane currentLane,
        Game.Objects.Transform transformData,
        Animal animal)
      {
        currentLane.m_Flags &= ~CreatureLaneFlags.Obsolete;
        currentLane.m_Lane = Entity.Null;
        currentLane.m_NextLane = Entity.Null;
        if ((animal.m_Flags & AnimalFlags.Roaming) != (AnimalFlags) 0)
          return;
        bool flag = (currentLane.m_Flags & CreatureLaneFlags.EmergeUnspawned) > (CreatureLaneFlags) 0;
        currentLane.m_Flags &= ~(CreatureLaneFlags.EndOfPath | CreatureLaneFlags.EndReached | CreatureLaneFlags.TransformTarget | CreatureLaneFlags.ParkingSpace | CreatureLaneFlags.Transport | CreatureLaneFlags.Connection | CreatureLaneFlags.Taxi | CreatureLaneFlags.FindLane | CreatureLaneFlags.Area | CreatureLaneFlags.Hangaround | CreatureLaneFlags.WaitPosition | CreatureLaneFlags.EmergeUnspawned);
        float3 position = transformData.m_Position;
        Bounds3 bounds3 = new Bounds3(position - 100f, position + 100f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        AnimalNavigationHelpers.FindLaneIterator iterator = new AnimalNavigationHelpers.FindLaneIterator()
        {
          m_Bounds = bounds3,
          m_Position = position,
          m_MinDistance = 1000f,
          m_UnspawnedEmerge = flag,
          m_Result = currentLane,
          m_SubLanes = this.m_Lanes,
          m_AreaNodes = this.m_AreaNodes,
          m_AreaTriangles = this.m_AreaTriangles,
          m_PedestrianLaneData = this.m_PedestrianLaneData,
          m_ConnectionLaneData = this.m_ConnectionLaneData,
          m_CurveData = this.m_CurveData,
          m_HangaroundLocationData = this.m_HangaroundLocationData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<AnimalNavigationHelpers.FindLaneIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        this.m_StaticObjectSearchTree.Iterate<AnimalNavigationHelpers.FindLaneIterator>(ref iterator);
        if (!flag)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_AreaSearchTree.Iterate<AnimalNavigationHelpers.FindLaneIterator>(ref iterator);
        }
        currentLane = iterator.m_Result;
      }

      private void UpdateNavigationTarget(
        ref Unity.Mathematics.Random random,
        Entity entity,
        Game.Objects.Transform transform,
        Moving moving,
        TripSource tripSource,
        GroupMember groupMember,
        Animal animal,
        PrefabRef prefabRef,
        CreatureData prefabCreatureData,
        AnimalData prefabAnimalData,
        ObjectGeometryData prefabObjectGeometryData,
        ref AnimalNavigation navigation,
        ref AnimalCurrentLane currentLane,
        ref Blocker blocker,
        DynamicBuffer<MeshGroup> meshGroups)
      {
        float timeStep = 0.266666681f;
        float x1 = math.length(moving.m_Velocity);
        if ((animal.m_Flags & AnimalFlags.SwimmingTarget) != (AnimalFlags) 0)
          currentLane.m_Flags |= CreatureLaneFlags.Swimming;
        else
          prefabAnimalData.m_SwimDepth.min = 0.0f;
        if ((animal.m_Flags & AnimalFlags.FlyingTarget) != (AnimalFlags) 0)
          currentLane.m_Flags |= CreatureLaneFlags.Flying;
        else
          prefabAnimalData.m_FlyHeight.min = 0.0f;
        if ((currentLane.m_Flags & CreatureLaneFlags.Connection) != (CreatureLaneFlags) 0)
        {
          prefabAnimalData.m_MoveSpeed = 277.777771f;
          prefabAnimalData.m_Acceleration = 277.777771f;
        }
        else
        {
          if ((currentLane.m_Flags & CreatureLaneFlags.Swimming) != (CreatureLaneFlags) 0)
            prefabAnimalData.m_MoveSpeed = prefabAnimalData.m_SwimSpeed;
          else if ((currentLane.m_Flags & CreatureLaneFlags.Flying) != (CreatureLaneFlags) 0)
            prefabAnimalData.m_MoveSpeed = prefabAnimalData.m_FlySpeed;
          x1 = math.min(x1, prefabAnimalData.m_MoveSpeed);
        }
        Bounds1 bounds1 = new Bounds1(x1 + new float2(-prefabAnimalData.m_Acceleration, prefabAnimalData.m_Acceleration) * timeStep);
        float position = math.select(prefabAnimalData.m_MoveSpeed * random.NextFloat(0.9f, 1f), 0.0f, tripSource.m_Source != Entity.Null);
        navigation.m_MaxSpeed = MathUtils.Clamp(position, bounds1);
        float num1 = math.max(prefabObjectGeometryData.m_Bounds.max.z, (float) (((double) prefabObjectGeometryData.m_Bounds.max.x - (double) prefabObjectGeometryData.m_Bounds.min.x) * 0.5));
        float a;
        if ((currentLane.m_Flags & (CreatureLaneFlags.EndReached | CreatureLaneFlags.TransformTarget | CreatureLaneFlags.Area)) != (CreatureLaneFlags) 0 || currentLane.m_Lane == Entity.Null || (currentLane.m_Flags & CreatureLaneFlags.Connection) != (CreatureLaneFlags) 0 && (currentLane.m_Flags & (CreatureLaneFlags.ParkingSpace | CreatureLaneFlags.WaitPosition)) != (CreatureLaneFlags) 0)
        {
          if ((animal.m_Flags & AnimalFlags.Roaming) != (AnimalFlags) 0 && (double) math.distance(transform.m_Position.xz, navigation.m_TargetPosition.xz) < (double) num1 + 1.0)
          {
            if ((currentLane.m_Flags & CreatureLaneFlags.Swimming) != (CreatureLaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Bounds1 bounds2 = WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, navigation.m_TargetPosition) - MathUtils.Invert(prefabAnimalData.m_SwimDepth);
              navigation.m_TargetPosition.y = MathUtils.Clamp(navigation.m_TargetPosition.y, bounds2);
            }
            else if ((currentLane.m_Flags & CreatureLaneFlags.Flying) != (CreatureLaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Bounds1 bounds3 = WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, navigation.m_TargetPosition) + prefabAnimalData.m_FlyHeight;
              navigation.m_TargetPosition.y = MathUtils.Clamp(navigation.m_TargetPosition.y, bounds3);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              navigation.m_TargetPosition.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, navigation.m_TargetPosition);
            }
          }
          a = math.distance(transform.m_Position, navigation.m_TargetPosition);
          float distance = math.select(a, math.max(0.0f, a - num1), (currentLane.m_Flags & (CreatureLaneFlags.TransformTarget | CreatureLaneFlags.Swimming | CreatureLaneFlags.Flying)) == (CreatureLaneFlags) 0);
          float y = MathUtils.Clamp(CreatureUtils.GetMaxBrakingSpeed(prefabAnimalData, distance, timeStep), bounds1);
          navigation.m_MaxSpeed = math.min(navigation.m_MaxSpeed, y);
        }
        else
        {
          if ((currentLane.m_Flags & CreatureLaneFlags.WaitSignal) != (CreatureLaneFlags) 0)
          {
            navigation.m_TargetPosition = transform.m_Position;
            navigation.m_TargetDirection = new float3();
            navigation.m_TargetActivity = (byte) 0;
            a = 0.0f;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PathOwnerData.HasComponent(groupMember.m_Leader))
            {
              // ISSUE: reference to a compiler-generated field
              PathOwner pathOwner = this.m_PathOwnerData[groupMember.m_Leader];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<PathElement> pathElement1 = this.m_PathElements[groupMember.m_Leader];
              if (pathOwner.m_ElementIndex < pathElement1.Length)
              {
                PathElement pathElement2 = pathElement1[pathOwner.m_ElementIndex];
                // ISSUE: reference to a compiler-generated field
                if (this.m_CurveData.HasComponent(pathElement2.m_Target))
                {
                  float lanePosition = math.select(currentLane.m_LanePosition, -currentLane.m_LanePosition, (currentLane.m_Flags & CreatureLaneFlags.Backward) > (CreatureLaneFlags) 0 != (double) pathElement2.m_TargetDelta.y < (double) pathElement2.m_TargetDelta.x);
                  // ISSUE: reference to a compiler-generated method
                  Line3.Segment targetPos = this.CalculateTargetPos(prefabObjectGeometryData, pathElement2.m_Target, pathElement2.m_TargetDelta, lanePosition);
                  navigation.m_TargetPosition = targetPos.a;
                  navigation.m_TargetDirection = math.normalizesafe(targetPos.b - targetPos.a);
                  a = math.distance(transform.m_Position, navigation.m_TargetPosition);
                }
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            navigation.m_TargetPosition = this.CalculateTargetPos(prefabObjectGeometryData, currentLane.m_Lane, currentLane.m_CurvePosition.x, currentLane.m_LanePosition);
            navigation.m_TargetDirection = new float3();
            navigation.m_TargetActivity = (byte) 0;
            a = math.distance(transform.m_Position, navigation.m_TargetPosition);
          }
          float brakingDistance = CreatureUtils.GetBrakingDistance(prefabAnimalData, navigation.m_MaxSpeed, timeStep);
          float distance = math.max(0.0f, a - num1);
          if ((double) distance < (double) brakingDistance)
          {
            float y = MathUtils.Clamp(CreatureUtils.GetMaxBrakingSpeed(prefabAnimalData, distance, timeStep), bounds1);
            navigation.m_MaxSpeed = math.min(navigation.m_MaxSpeed, y);
          }
        }
        navigation.m_MaxSpeed = math.select(navigation.m_MaxSpeed, 0.0f, (double) navigation.m_MaxSpeed < 0.10000000149011612);
        Entity blocker1 = blocker.m_Blocker;
        float num2 = navigation.m_MaxSpeed;
        blocker.m_Blocker = Entity.Null;
        blocker.m_Type = BlockerType.None;
        currentLane.m_QueueEntity = Entity.Null;
        currentLane.m_QueueArea = new Sphere3();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_HumanCurrentLaneData.HasComponent(groupMember.m_Leader) && this.m_HumanCurrentLaneData[groupMember.m_Leader].m_Lane == currentLane.m_Lane)
        {
          // ISSUE: reference to a compiler-generated field
          Game.Objects.Transform transform1 = this.m_TransformData[groupMember.m_Leader];
          Moving moving1 = new Moving();
          // ISSUE: reference to a compiler-generated field
          if (this.m_MovingData.HasComponent(groupMember.m_Leader))
          {
            // ISSUE: reference to a compiler-generated field
            moving1 = this.m_MovingData[groupMember.m_Leader];
          }
          float3 float3 = math.normalizesafe(navigation.m_TargetPosition - transform.m_Position);
          float3 x2 = transform1.m_Position - transform.m_Position;
          if ((double) math.dot(x2, float3) < 0.0)
          {
            float distance = math.max(0.0f, 3f - math.length(x2));
            float maxResultSpeed = math.max(0.0f, math.dot(float3, moving1.m_Velocity));
            float num3 = MathUtils.Clamp(CreatureUtils.GetMaxBrakingSpeed(prefabAnimalData, distance, maxResultSpeed, timeStep), bounds1);
            if ((double) num3 < (double) navigation.m_MaxSpeed)
            {
              navigation.m_MaxSpeed = num3;
              num2 = num3;
              blocker.m_Blocker = groupMember.m_Leader;
              blocker.m_Type = BlockerType.Continuing;
            }
          }
        }
        float num4 = num1 + math.max(1f, navigation.m_MaxSpeed * timeStep) + CreatureUtils.GetBrakingDistance(prefabAnimalData, navigation.m_MaxSpeed, timeStep);
        float num5 = num1 + 1f;
        if ((double) x1 > 0.0099999997764825821 && (animal.m_Flags & AnimalFlags.Roaming) == (AnimalFlags) 0)
        {
          float num6 = x1 * timeStep;
          float num7 = random.NextFloat(0.0f, 1f);
          float num8 = num7 * num7;
          // ISSUE: reference to a compiler-generated field
          float y = math.select(0.5f - num8, num8 - 0.5f, this.m_LeftHandTraffic != (currentLane.m_Flags & CreatureLaneFlags.Backward) > (CreatureLaneFlags) 0);
          currentLane.m_LanePosition = math.lerp(currentLane.m_LanePosition, y, math.min(1f, num6 * 0.01f));
        }
        if ((double) a < (double) num4)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          CreatureTargetIterator targetIterator = new CreatureTargetIterator()
          {
            m_MovingData = this.m_MovingData,
            m_CurveData = this.m_CurveData,
            m_LaneReservationData = this.m_LaneReservationData,
            m_LaneOverlaps = this.m_LaneOverlaps,
            m_LaneObjects = this.m_LaneObjects,
            m_PrefabObjectGeometry = prefabObjectGeometryData,
            m_Blocker = blocker.m_Blocker,
            m_BlockerType = blocker.m_Type,
            m_QueueEntity = currentLane.m_QueueEntity,
            m_QueueArea = currentLane.m_QueueArea
          };
          Animal componentData;
          while (true)
          {
            // ISSUE: reference to a compiler-generated field
            do
            {
              byte activity = 0;
              if ((currentLane.m_Flags & (CreatureLaneFlags.EndReached | CreatureLaneFlags.WaitSignal)) == (CreatureLaneFlags) 0 && (animal.m_Flags & AnimalFlags.Roaming) == (AnimalFlags) 0 && currentLane.m_Lane != Entity.Null)
              {
                if ((currentLane.m_Flags & CreatureLaneFlags.TransformTarget) != (CreatureLaneFlags) 0)
                {
                  CurrentVehicle currentVehicle = new CurrentVehicle();
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_CurrentVehicleData.HasComponent(groupMember.m_Leader))
                  {
                    // ISSUE: reference to a compiler-generated field
                    currentVehicle = this.m_CurrentVehicleData[groupMember.m_Leader];
                  }
                  if ((currentLane.m_Flags & CreatureLaneFlags.WaitPosition) != (CreatureLaneFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    if (this.MoveTransformTarget(entity, prefabRef.m_Prefab, meshGroups, ref random, currentVehicle, transform.m_Position, ref navigation.m_TargetPosition, ref navigation.m_TargetDirection, ref activity, 0.0f, currentLane.m_Lane, prefabCreatureData.m_SupportedActivities))
                    {
                      navigation.m_TargetPosition = VehicleUtils.GetConnectionParkingPosition(new Game.Net.ConnectionLane(), new Bezier4x3(navigation.m_TargetPosition, navigation.m_TargetPosition, navigation.m_TargetPosition, navigation.m_TargetPosition), currentLane.m_CurvePosition.y);
                      navigation.m_TargetDirection = new float3();
                      navigation.m_TargetActivity = (byte) 0;
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated method
                    if (this.MoveTransformTarget(entity, prefabRef.m_Prefab, meshGroups, ref random, currentVehicle, transform.m_Position, ref navigation.m_TargetPosition, ref navigation.m_TargetDirection, ref activity, num4, currentLane.m_Lane, prefabCreatureData.m_SupportedActivities))
                      goto label_98;
                  }
                }
                else if ((currentLane.m_Flags & CreatureLaneFlags.Connection) != (CreatureLaneFlags) 0 && (currentLane.m_Flags & (CreatureLaneFlags.ParkingSpace | CreatureLaneFlags.WaitPosition)) != (CreatureLaneFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  Curve curve = this.m_CurveData[currentLane.m_Lane];
                  // ISSUE: reference to a compiler-generated field
                  Game.Net.ConnectionLane connectionLane = this.m_ConnectionLaneData[currentLane.m_Lane];
                  navigation.m_TargetPosition = VehicleUtils.GetConnectionParkingPosition(connectionLane, curve.m_Bezier, currentLane.m_CurvePosition.y);
                  navigation.m_TargetDirection = new float3();
                  navigation.m_TargetActivity = (byte) 0;
                }
                else if ((currentLane.m_Flags & CreatureLaneFlags.Area) != (CreatureLaneFlags) 0)
                {
                  navigation.m_TargetActivity = (byte) 0;
                  float navigationSize = CreatureUtils.GetNavigationSize(prefabObjectGeometryData);
                  PathOwner pathOwner = new PathOwner();
                  DynamicBuffer<PathElement> pathElements = new DynamicBuffer<PathElement>();
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_HumanCurrentLaneData.HasComponent(groupMember.m_Leader) && this.m_HumanCurrentLaneData[groupMember.m_Leader].m_Lane == currentLane.m_Lane)
                  {
                    // ISSUE: reference to a compiler-generated field
                    pathOwner = this.m_PathOwnerData[groupMember.m_Leader];
                    // ISSUE: reference to a compiler-generated field
                    pathElements = this.m_PathElements[groupMember.m_Leader];
                  }
                  // ISSUE: reference to a compiler-generated method
                  if (this.MoveAreaTarget(ref random, transform.m_Position, pathOwner, pathElements, ref navigation.m_TargetPosition, ref navigation.m_TargetDirection, ref activity, num4, currentLane.m_Lane, currentLane.m_NextLane, prefabCreatureData.m_SupportedActivities, ref currentLane.m_CurvePosition, currentLane.m_NextPosition, currentLane.m_LanePosition, navigationSize))
                    goto label_98;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  Curve curve = this.m_CurveData[currentLane.m_Lane];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  NetLaneData prefabLaneData = this.m_PrefabLaneData[this.m_PrefabRefData[currentLane.m_Lane].m_Prefab];
                  float laneOffset = CreatureUtils.GetLaneOffset(prefabObjectGeometryData, prefabLaneData, currentLane.m_LanePosition);
                  navigation.m_TargetDirection = new float3();
                  navigation.m_TargetActivity = (byte) 0;
                  // ISSUE: reference to a compiler-generated method
                  if (this.MoveLaneTarget(ref targetIterator, currentLane.m_Lane, transform.m_Position, ref navigation.m_TargetPosition, num4, curve.m_Bezier, ref currentLane.m_CurvePosition, laneOffset))
                    goto label_98;
                }
              }
              if ((currentLane.m_Flags & CreatureLaneFlags.EndOfPath) != (CreatureLaneFlags) 0)
              {
                float num9 = math.distance(transform.m_Position, navigation.m_TargetPosition);
                if ((currentLane.m_Flags & CreatureLaneFlags.EndReached) == (CreatureLaneFlags) 0 && (double) num9 < (double) num5 && (double) x1 < 0.10000000149011612)
                {
                  navigation.m_TargetActivity = activity;
                  currentLane.m_Flags |= CreatureLaneFlags.EndReached;
                  if ((animal.m_Flags & AnimalFlags.SwimmingTarget) == (AnimalFlags) 0)
                    currentLane.m_Flags &= ~CreatureLaneFlags.Swimming;
                  if ((animal.m_Flags & AnimalFlags.FlyingTarget) == (AnimalFlags) 0)
                  {
                    currentLane.m_Flags &= ~CreatureLaneFlags.Flying;
                    goto label_98;
                  }
                  else
                    goto label_98;
                }
                else
                  goto label_98;
              }
              else if ((animal.m_Flags & AnimalFlags.Roaming) == (AnimalFlags) 0 && currentLane.m_NextLane != Entity.Null)
              {
                if (((currentLane.m_Flags ^ currentLane.m_NextFlags) & CreatureLaneFlags.Backward) != (CreatureLaneFlags) 0)
                  currentLane.m_LanePosition = -currentLane.m_LanePosition;
                currentLane.m_Lane = currentLane.m_NextLane;
                currentLane.m_Flags = currentLane.m_NextFlags;
                currentLane.m_CurvePosition = currentLane.m_NextPosition;
                currentLane.m_NextLane = Entity.Null;
              }
              else
                goto label_65;
            }
            while ((currentLane.m_Flags & CreatureLaneFlags.Area) != (CreatureLaneFlags) 0 || !this.m_CurveData.HasComponent(currentLane.m_Lane));
            // ISSUE: reference to a compiler-generated field
            double num10 = (double) MathUtils.Distance(this.m_CurveData[currentLane.m_Lane].m_Bezier, transform.m_Position, out currentLane.m_CurvePosition.x);
            continue;
label_65:
            if (groupMember.m_Leader != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_HumanCurrentLaneData.HasComponent(groupMember.m_Leader))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (!this.m_AnimalCurrentLaneData.HasComponent(groupMember.m_Leader) || !this.m_AnimalData.TryGetComponent(groupMember.m_Leader, out componentData))
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_CurrentVehicleData.HasComponent(groupMember.m_Leader))
                  {
                    // ISSUE: reference to a compiler-generated field
                    CurrentVehicle currentVehicle = this.m_CurrentVehicleData[groupMember.m_Leader];
                    currentLane.m_Lane = currentVehicle.m_Vehicle;
                    currentLane.m_CurvePosition = (float2) 0.0f;
                    currentLane.m_Flags = CreatureLaneFlags.EndOfPath | CreatureLaneFlags.TransformTarget;
                    continue;
                  }
                }
                else
                  goto label_76;
              }
              else
                break;
            }
            if (!(tripSource.m_Source != Entity.Null))
              currentLane.m_Flags |= CreatureLaneFlags.EndOfPath;
            else
              goto label_98;
          }
          // ISSUE: reference to a compiler-generated field
          if ((this.m_HumanCurrentLaneData[groupMember.m_Leader].m_Flags & CreatureLaneFlags.WaitSignal) != (CreatureLaneFlags) 0)
          {
            currentLane.m_Flags |= CreatureLaneFlags.WaitSignal;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PathOwnerData.HasComponent(groupMember.m_Leader))
            {
              // ISSUE: reference to a compiler-generated field
              PathOwner pathOwner = this.m_PathOwnerData[groupMember.m_Leader];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<PathElement> pathElement3 = this.m_PathElements[groupMember.m_Leader];
              if (pathOwner.m_ElementIndex < pathElement3.Length)
              {
                PathElement pathElement4 = pathElement3[pathOwner.m_ElementIndex];
                // ISSUE: reference to a compiler-generated field
                if (this.m_CurveData.HasComponent(pathElement4.m_Target))
                {
                  float lanePosition = math.select(currentLane.m_LanePosition, -currentLane.m_LanePosition, (currentLane.m_Flags & CreatureLaneFlags.Backward) > (CreatureLaneFlags) 0 != (double) pathElement4.m_TargetDelta.y < (double) pathElement4.m_TargetDelta.x);
                  // ISSUE: reference to a compiler-generated method
                  Line3.Segment targetPos = this.CalculateTargetPos(prefabObjectGeometryData, pathElement4.m_Target, pathElement4.m_TargetDelta, lanePosition);
                  navigation.m_TargetPosition = targetPos.a;
                  navigation.m_TargetDirection = math.normalizesafe(targetPos.b - targetPos.a);
                  navigation.m_TargetActivity = (byte) 0;
                }
              }
            }
          }
          else if ((double) math.distance(transform.m_Position, navigation.m_TargetPosition) < (double) num5 && (double) x1 < 0.10000000149011612)
            currentLane.m_Flags |= CreatureLaneFlags.EndReached;
          targetIterator.m_Blocker = groupMember.m_Leader;
          targetIterator.m_BlockerType = BlockerType.Continuing;
          goto label_98;
label_76:
          if (((animal.m_Flags ^ componentData.m_Flags) & AnimalFlags.Roaming) != (AnimalFlags) 0)
            currentLane.m_Flags |= CreatureLaneFlags.EndReached;
          else if ((componentData.m_Flags & AnimalFlags.Roaming) != (AnimalFlags) 0)
          {
            currentLane.m_Lane = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            Game.Objects.Transform transform2 = this.m_TransformData[groupMember.m_Leader];
            float2 float2 = MathUtils.RotateLeft(new float2(0.0f, num1 * -2f), currentLane.m_LanePosition * 6.28318548f);
            float3 worldPosition = transform2.m_Position + math.mul(transform2.m_Rotation, new float3(float2.x, 0.0f, float2.y));
            if ((currentLane.m_Flags & CreatureLaneFlags.Swimming) != (CreatureLaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Bounds1 bounds4 = WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, worldPosition) - MathUtils.Invert(prefabAnimalData.m_SwimDepth);
              worldPosition.y = MathUtils.Clamp(worldPosition.y, bounds4);
            }
            else if ((currentLane.m_Flags & CreatureLaneFlags.Flying) != (CreatureLaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Bounds1 bounds5 = WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, worldPosition) + prefabAnimalData.m_FlyHeight;
              worldPosition.y = MathUtils.Clamp(worldPosition.y, bounds5);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              worldPosition.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, worldPosition);
            }
            float3 x3 = worldPosition - navigation.m_TargetPosition;
            if ((double) math.length(x3) >= 0.10000000149011612)
            {
              navigation.m_TargetPosition += MathUtils.ClampLength(x3, num4);
            }
            else
            {
              targetIterator.m_Blocker = groupMember.m_Leader;
              targetIterator.m_BlockerType = BlockerType.Continuing;
              if ((double) a < (double) num5 && (double) x1 < 0.10000000149011612)
              {
                navigation.m_TargetActivity = (byte) 0;
                if ((animal.m_Flags & AnimalFlags.SwimmingTarget) == (AnimalFlags) 0)
                  currentLane.m_Flags &= ~CreatureLaneFlags.Swimming;
                if ((animal.m_Flags & AnimalFlags.FlyingTarget) == (AnimalFlags) 0)
                  currentLane.m_Flags &= ~CreatureLaneFlags.Flying;
              }
            }
          }
          else
          {
            if ((double) math.distance(transform.m_Position, navigation.m_TargetPosition) < (double) num5 && (double) x1 < 0.10000000149011612)
              currentLane.m_Flags |= CreatureLaneFlags.EndReached;
            targetIterator.m_Blocker = groupMember.m_Leader;
            targetIterator.m_BlockerType = BlockerType.Continuing;
          }
label_98:
          blocker.m_Blocker = targetIterator.m_Blocker;
          blocker.m_Type = targetIterator.m_BlockerType;
        }
        if (navigation.m_TargetActivity == (byte) 0)
        {
          if ((currentLane.m_Flags & CreatureLaneFlags.Swimming) != (CreatureLaneFlags) 0)
            navigation.m_TargetActivity = (byte) 8;
          else if ((currentLane.m_Flags & CreatureLaneFlags.Flying) != (CreatureLaneFlags) 0)
            navigation.m_TargetActivity = (byte) 9;
        }
        if ((double) navigation.m_MaxSpeed != 0.0 || blocker1 != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          CreatureCollisionIterator collisionIterator = new CreatureCollisionIterator()
          {
            m_OwnerData = this.m_OwnerData,
            m_TransformData = this.m_TransformData,
            m_MovingData = this.m_MovingData,
            m_CreatureData = this.m_CreatureData,
            m_GroupMemberData = this.m_GroupMemberData,
            m_CurveData = this.m_CurveData,
            m_AreaLaneData = this.m_AreaLaneData,
            m_PrefabRefData = this.m_PrefabRefData,
            m_PrefabObjectGeometryData = this.m_PrefabObjectGeometryData,
            m_PrefabLaneData = this.m_PrefabLaneData,
            m_LaneObjects = this.m_LaneObjects,
            m_AreaNodes = this.m_AreaNodes,
            m_StaticObjectSearchTree = this.m_StaticObjectSearchTree,
            m_MovingObjectSearchTree = this.m_MovingObjectSearchTree,
            m_Entity = entity,
            m_Leader = groupMember.m_Leader,
            m_TimeStep = timeStep,
            m_PrefabObjectGeometry = prefabObjectGeometryData,
            m_SpeedRange = bounds1,
            m_CurrentPosition = transform.m_Position,
            m_CurrentDirection = math.forward(transform.m_Rotation),
            m_CurrentVelocity = moving.m_Velocity,
            m_TargetDistance = num4,
            m_MinSpeed = random.NextFloat(0.4f, 0.6f),
            m_TargetPosition = navigation.m_TargetPosition,
            m_MaxSpeed = navigation.m_MaxSpeed,
            m_LanePosition = currentLane.m_LanePosition,
            m_Blocker = blocker.m_Blocker,
            m_BlockerType = blocker.m_Type,
            m_QueueEntity = currentLane.m_QueueEntity,
            m_QueueArea = currentLane.m_QueueArea
          };
          if (blocker1 != Entity.Null)
          {
            collisionIterator.IterateBlocker(prefabAnimalData, blocker1);
            collisionIterator.m_MaxSpeed = math.select(collisionIterator.m_MaxSpeed, 0.0f, (double) collisionIterator.m_MaxSpeed < 0.10000000149011612);
          }
          if ((double) collisionIterator.m_MaxSpeed != 0.0 && (currentLane.m_Flags & CreatureLaneFlags.Connection) == (CreatureLaneFlags) 0)
          {
            bool isBackward = (currentLane.m_Flags & CreatureLaneFlags.Backward) > (CreatureLaneFlags) 0;
            if ((currentLane.m_Flags & CreatureLaneFlags.WaitSignal) != (CreatureLaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_PathOwnerData.HasComponent(groupMember.m_Leader))
              {
                // ISSUE: reference to a compiler-generated field
                PathOwner pathOwner = this.m_PathOwnerData[groupMember.m_Leader];
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<PathElement> pathElement5 = this.m_PathElements[groupMember.m_Leader];
                int elementIndex = pathOwner.m_ElementIndex;
                if (elementIndex < pathElement5.Length)
                {
                  ref DynamicBuffer<PathElement> local = ref pathElement5;
                  int index = elementIndex;
                  int num11 = index + 1;
                  PathElement pathElement6 = local[index];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_CurveData.HasComponent(pathElement6.m_Target) && collisionIterator.IterateFirstLane(currentLane.m_Lane, pathElement6.m_Target, currentLane.m_CurvePosition, pathElement6.m_TargetDelta, isBackward))
                  {
                    while (collisionIterator.IterateNextLane(pathElement6.m_Target, pathElement6.m_TargetDelta) && num11 < pathElement5.Length)
                      pathElement6 = pathElement5[num11++];
                  }
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (collisionIterator.IterateFirstLane(currentLane.m_Lane, currentLane.m_CurvePosition, isBackward) && this.m_PathOwnerData.HasComponent(groupMember.m_Leader))
              {
                // ISSUE: reference to a compiler-generated field
                PathOwner pathOwner = this.m_PathOwnerData[groupMember.m_Leader];
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<PathElement> pathElement7 = this.m_PathElements[groupMember.m_Leader];
                int elementIndex = pathOwner.m_ElementIndex;
                if (elementIndex < pathElement7.Length)
                {
                  ref DynamicBuffer<PathElement> local = ref pathElement7;
                  int index = elementIndex;
                  int num12 = index + 1;
                  PathElement pathElement8 = local[index];
                  while (collisionIterator.IterateNextLane(pathElement8.m_Target, pathElement8.m_TargetDelta) && num12 < pathElement7.Length)
                    pathElement8 = pathElement7[num12++];
                }
              }
            }
            collisionIterator.m_MaxSpeed = math.select(collisionIterator.m_MaxSpeed, 0.0f, (double) collisionIterator.m_MaxSpeed < 0.10000000149011612);
          }
          navigation.m_TargetPosition = collisionIterator.m_TargetPosition;
          navigation.m_MaxSpeed = collisionIterator.m_MaxSpeed;
          currentLane.m_LanePosition = math.lerp(currentLane.m_LanePosition, collisionIterator.m_LanePosition, 0.5f);
          currentLane.m_QueueEntity = collisionIterator.m_QueueEntity;
          currentLane.m_QueueArea = collisionIterator.m_QueueArea;
          blocker.m_Blocker = collisionIterator.m_Blocker;
          blocker.m_Type = collisionIterator.m_BlockerType;
          num2 = collisionIterator.m_MaxSpeed;
        }
        blocker.m_MaxSpeed = (byte) math.clamp(Mathf.RoundToInt(num2 * 45.8999977f), 0, (int) byte.MaxValue);
      }

      private float3 CalculateTargetPos(
        ObjectGeometryData prefabObjectGeometryData,
        Entity lane,
        float curvePosition,
        float lanePosition)
      {
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[lane];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetLaneData prefabLaneData = this.m_PrefabLaneData[this.m_PrefabRefData[lane].m_Prefab];
        float laneOffset = CreatureUtils.GetLaneOffset(prefabObjectGeometryData, prefabLaneData, lanePosition);
        return CreatureUtils.GetLanePosition(curve.m_Bezier, curvePosition, laneOffset);
      }

      private Line3.Segment CalculateTargetPos(
        ObjectGeometryData prefabObjectGeometryData,
        Entity lane,
        float2 curvePosition,
        float lanePosition)
      {
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[lane];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetLaneData prefabLaneData = this.m_PrefabLaneData[this.m_PrefabRefData[lane].m_Prefab];
        float laneOffset = CreatureUtils.GetLaneOffset(prefabObjectGeometryData, prefabLaneData, lanePosition);
        Line3.Segment targetPos;
        targetPos.a = CreatureUtils.GetLanePosition(curve.m_Bezier, curvePosition.x, laneOffset);
        targetPos.b = CreatureUtils.GetLanePosition(curve.m_Bezier, curvePosition.y, laneOffset);
        return targetPos;
      }

      private bool MoveAreaTarget(
        ref Unity.Mathematics.Random random,
        float3 comparePosition,
        PathOwner pathOwner,
        DynamicBuffer<PathElement> pathElements,
        ref float3 targetPosition,
        ref float3 targetDirection,
        ref byte activity,
        float minDistance,
        Entity target,
        Entity nextTarget,
        ActivityMask activityMask,
        ref float2 curveDelta,
        float2 nextCurveDelta,
        float lanePosition,
        float navigationSize)
      {
        if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Obsolete | PathFlags.Updated)) != (PathFlags) 0)
          return true;
        // ISSUE: reference to a compiler-generated field
        Entity owner = this.m_OwnerData[target].m_Owner;
        // ISSUE: reference to a compiler-generated field
        AreaLane areaLane1 = this.m_AreaLaneData[target];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Areas.Node> areaNode = this.m_AreaNodes[owner];
        bool isBackward = (double) curveDelta.y < (double) curveDelta.x;
        PathElement nextElement1 = new PathElement(nextTarget, nextCurveDelta);
        targetDirection = new float3();
        activity = (byte) 0;
        if (areaLane1.m_Nodes.y == areaLane1.m_Nodes.z)
        {
          float3 position1 = areaNode[areaLane1.m_Nodes.x].m_Position;
          float3 position2 = areaNode[areaLane1.m_Nodes.y].m_Position;
          float3 position3 = areaNode[areaLane1.m_Nodes.w].m_Position;
          float3 right = position2;
          float3 next = position3;
          float3 comparePosition1 = comparePosition;
          PathElement nextElement2 = nextElement1;
          int elementIndex = pathOwner.m_ElementIndex;
          DynamicBuffer<PathElement> pathElements1 = pathElements;
          ref float3 local = ref targetPosition;
          double minDistance1 = (double) minDistance;
          double lanePosition1 = (double) lanePosition;
          double y = (double) curveDelta.y;
          double navigationSize1 = (double) navigationSize;
          // ISSUE: reference to a compiler-generated field
          ComponentLookup<Game.Objects.Transform> transformData = this.m_TransformData;
          // ISSUE: reference to a compiler-generated field
          ComponentLookup<TaxiStand> taxiStandData = this.m_TaxiStandData;
          // ISSUE: reference to a compiler-generated field
          ComponentLookup<AreaLane> areaLaneData = this.m_AreaLaneData;
          // ISSUE: reference to a compiler-generated field
          ComponentLookup<Curve> curveData = this.m_CurveData;
          if (CreatureUtils.SetTriangleTarget(position1, right, next, comparePosition1, nextElement2, elementIndex, pathElements1, ref local, (float) minDistance1, (float) lanePosition1, (float) y, (float) navigationSize1, true, transformData, taxiStandData, areaLaneData, curveData))
            return true;
          curveDelta.x = curveDelta.y;
        }
        else
        {
          bool4 bool4_1 = new bool4(curveDelta < 0.5f, curveDelta > 0.5f);
          int2 int2_1 = math.select((int2) areaLane1.m_Nodes.x, (int2) areaLane1.m_Nodes.w, bool4_1.zw);
          float3 position4 = areaNode[int2_1.x].m_Position;
          float3 position5 = areaNode[areaLane1.m_Nodes.y].m_Position;
          float3 position6 = areaNode[areaLane1.m_Nodes.z].m_Position;
          float3 position7 = areaNode[int2_1.y].m_Position;
          if (math.any(bool4_1.xy & bool4_1.wz))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (CreatureUtils.SetAreaTarget(position4, position4, position5, position6, position7, owner, areaNode, comparePosition, nextElement1, pathOwner.m_ElementIndex, pathElements, ref targetPosition, minDistance, lanePosition, curveDelta.y, navigationSize, isBackward, this.m_TransformData, this.m_TaxiStandData, this.m_AreaLaneData, this.m_CurveData, this.m_OwnerData))
              return true;
            curveDelta.x = 0.5f;
            bool4_1.xz = (bool2) false;
          }
          if (nextElement1.m_Target == Entity.Null && pathElements.IsCreated && pathOwner.m_ElementIndex < pathElements.Length)
            nextElement1 = pathElements[pathOwner.m_ElementIndex++];
          Owner componentData;
          // ISSUE: reference to a compiler-generated field
          if (nextElement1.m_Target != Entity.Null && this.m_OwnerData.TryGetComponent(nextElement1.m_Target, out componentData) && componentData.m_Owner == owner)
          {
            bool4 bool4_2 = new bool4(nextElement1.m_TargetDelta < 0.5f, nextElement1.m_TargetDelta > 0.5f);
            if (math.any(!bool4_1.xz) & math.any(bool4_1.yw) & math.any(bool4_2.xy & bool4_2.wz))
            {
              // ISSUE: reference to a compiler-generated field
              AreaLane areaLane2 = this.m_AreaLaneData[nextElement1.m_Target];
              bool flag = (double) nextElement1.m_TargetDelta.y < (double) nextElement1.m_TargetDelta.x;
              lanePosition = math.select(lanePosition, -lanePosition, flag != isBackward);
              int2 int2_2 = math.select((int2) areaLane2.m_Nodes.x, (int2) areaLane2.m_Nodes.w, bool4_2.zw);
              float3 position8 = areaNode[int2_2.x].m_Position;
              float3 prev2 = math.select(position5, position6, position8.Equals(position5));
              float3 position9 = areaNode[areaLane2.m_Nodes.y].m_Position;
              float3 position10 = areaNode[areaLane2.m_Nodes.z].m_Position;
              float3 position11 = areaNode[int2_2.y].m_Position;
              float3 prev = position8;
              float3 left = position9;
              float3 right = position10;
              float3 next = position11;
              Entity areaEntity = owner;
              DynamicBuffer<Game.Areas.Node> nodes = areaNode;
              float3 comparePosition2 = comparePosition;
              PathElement nextElement3 = new PathElement();
              int elementIndex = pathOwner.m_ElementIndex;
              DynamicBuffer<PathElement> pathElements2 = pathElements;
              ref float3 local = ref targetPosition;
              double minDistance2 = (double) minDistance;
              double lanePosition2 = (double) lanePosition;
              double y = (double) nextElement1.m_TargetDelta.y;
              double navigationSize2 = (double) navigationSize;
              int num = flag ? 1 : 0;
              // ISSUE: reference to a compiler-generated field
              ComponentLookup<Game.Objects.Transform> transformData = this.m_TransformData;
              // ISSUE: reference to a compiler-generated field
              ComponentLookup<TaxiStand> taxiStandData = this.m_TaxiStandData;
              // ISSUE: reference to a compiler-generated field
              ComponentLookup<AreaLane> areaLaneData = this.m_AreaLaneData;
              // ISSUE: reference to a compiler-generated field
              ComponentLookup<Curve> curveData = this.m_CurveData;
              // ISSUE: reference to a compiler-generated field
              ComponentLookup<Owner> ownerData = this.m_OwnerData;
              if (CreatureUtils.SetAreaTarget(prev2, prev, left, right, next, areaEntity, nodes, comparePosition2, nextElement3, elementIndex, pathElements2, ref local, (float) minDistance2, (float) lanePosition2, (float) y, (float) navigationSize2, num != 0, transformData, taxiStandData, areaLaneData, curveData, ownerData))
                return true;
            }
            curveDelta.x = curveDelta.y;
            return false;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (CreatureUtils.SetTriangleTarget(position5, position6, position7, comparePosition, nextElement1, pathOwner.m_ElementIndex, pathElements, ref targetPosition, minDistance, lanePosition, curveDelta.y, navigationSize, false, this.m_TransformData, this.m_TaxiStandData, this.m_AreaLaneData, this.m_CurveData))
            return true;
          curveDelta.x = curveDelta.y;
        }
        ActivityType activity1 = ActivityType.None;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        CreatureUtils.GetAreaActivity(ref random, ref activity1, target, activityMask, this.m_OwnerData, this.m_PrefabRefData, this.m_PrefabSpawnLocationData);
        if (activity1 != ActivityType.Standing)
          activity = (byte) activity1;
        return (double) math.distance(comparePosition, targetPosition) >= (double) minDistance;
      }

      private bool MoveTransformTarget(
        Entity creature,
        Entity creaturePrefab,
        DynamicBuffer<MeshGroup> meshGroups,
        ref Unity.Mathematics.Random random,
        CurrentVehicle currentVehicle,
        float3 comparePosition,
        ref float3 targetPosition,
        ref float3 targetDirection,
        ref byte activity,
        float minDistance,
        Entity target,
        ActivityMask activityMask)
      {
        Game.Objects.Transform result = new Game.Objects.Transform()
        {
          m_Position = targetPosition
        };
        ActivityType activity1 = ActivityType.None;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!CreatureUtils.CalculateTransformPosition(creature, creaturePrefab, meshGroups, ref random, ref result, ref activity1, currentVehicle, target, this.m_LeftHandTraffic, activityMask, (ActivityCondition) 0, this.m_MovingObjectSearchTree, ref this.m_TransformData, ref this.m_PositionData, ref this.m_PublicTransportData, ref this.m_TrainData, ref this.m_ControllerData, ref this.m_PrefabRefData, ref this.m_PrefabBuildingData, ref this.m_PrefabCarData, ref this.m_PrefabActivityLocations, ref this.m_SubMeshGroups, ref this.m_CharacterElements, ref this.m_SubMeshes, ref this.m_AnimationClips, ref this.m_AnimationMotions))
          return false;
        targetPosition = result.m_Position;
        if (result.m_Rotation.Equals(new quaternion()))
          targetDirection = new float3();
        else
          targetDirection = math.forward(result.m_Rotation);
        activity = (byte) activity1;
        return (double) math.distance(comparePosition, targetPosition) >= (double) minDistance;
      }

      private bool GetTransformTarget(
        ref Entity entity,
        Game.Objects.Transform transform,
        Entity target,
        Entity prevLane,
        float prevCurvePosition,
        float prevLanePosition,
        ObjectGeometryData prefabObjectGeometryData)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_PropertyRenterData.HasComponent(target))
        {
          // ISSUE: reference to a compiler-generated field
          target = this.m_PropertyRenterData[target].m_Property;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.HasComponent(target))
        {
          entity = target;
          return true;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PositionData.HasComponent(target))
          return false;
        entity = target;
        return true;
      }

      private bool MoveLaneTarget(
        ref CreatureTargetIterator targetIterator,
        Entity lane,
        float3 comparePosition,
        ref float3 targetPosition,
        float minDistance,
        Bezier4x3 curve,
        ref float2 curveDelta,
        float laneOffset)
      {
        float3 lanePosition1 = CreatureUtils.GetLanePosition(curve, curveDelta.y, laneOffset);
        if ((double) math.distance(comparePosition, lanePosition1) < (double) minDistance)
        {
          if (targetIterator.IterateLane(lane, ref curveDelta.x, curveDelta.y))
          {
            targetPosition = lanePosition1;
            return false;
          }
          targetPosition = CreatureUtils.GetLanePosition(curve, curveDelta.x, laneOffset);
          return true;
        }
        float2 float2 = curveDelta;
        for (int index = 0; index < 8; ++index)
        {
          float curvePosition = math.lerp(float2.x, float2.y, 0.5f);
          float3 lanePosition2 = CreatureUtils.GetLanePosition(curve, curvePosition, laneOffset);
          if ((double) math.distance(comparePosition, lanePosition2) < (double) minDistance)
            float2.x = curvePosition;
          else
            float2.y = curvePosition;
        }
        targetIterator.IterateLane(lane, ref curveDelta.x, float2.y);
        targetPosition = CreatureUtils.GetLanePosition(curve, curveDelta.x, laneOffset);
        return true;
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
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Moving> __Game_Objects_Moving_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<GroupMember> __Game_Creatures_GroupMember_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Stumbling> __Game_Creatures_Stumbling_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TripSource> __Game_Objects_TripSource_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Animal> __Game_Creatures_Animal_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<MeshGroup> __Game_Rendering_MeshGroup_RO_BufferTypeHandle;
      public ComponentTypeHandle<AnimalNavigation> __Game_Creatures_AnimalNavigation_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Blocker> __Game_Vehicles_Blocker_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HumanCurrentLane> __Game_Creatures_HumanCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneReservation> __Game_Net_LaneReservation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaLane> __Game_Net_AreaLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Position> __Game_Routes_Position_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TaxiStand> __Game_Routes_TaxiStand_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Moving> __Game_Objects_Moving_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Creature> __Game_Creatures_Creature_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Animal> __Game_Creatures_Animal_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GroupMember> __Game_Creatures_GroupMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HangaroundLocation> __Game_Areas_HangaroundLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> __Game_Vehicles_PublicTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Train> __Game_Vehicles_Train_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathOwner> __Game_Pathfind_PathOwner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CreatureData> __Game_Prefabs_CreatureData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AnimalData> __Game_Prefabs_AnimalData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> __Game_Prefabs_ActivityLocationElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> __Game_Prefabs_SubMeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CharacterElement> __Game_Prefabs_CharacterElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.AnimationClip> __Game_Prefabs_AnimationClip_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<AnimationMotion> __Game_Prefabs_AnimationMotion_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      public ComponentLookup<AnimalCurrentLane> __Game_Creatures_AnimalCurrentLane_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupMember_RO_ComponentTypeHandle = state.GetComponentTypeHandle<GroupMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Stumbling_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Stumbling>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TripSource_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TripSource>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Animal_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Animal>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshGroup_RO_BufferTypeHandle = state.GetBufferTypeHandle<MeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalNavigation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AnimalNavigation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Blocker_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Blocker>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanCurrentLane_RO_ComponentLookup = state.GetComponentLookup<HumanCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneReservation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.LaneReservation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_AreaLane_RO_ComponentLookup = state.GetComponentLookup<AreaLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentLookup = state.GetComponentLookup<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TaxiStand_RO_ComponentLookup = state.GetComponentLookup<TaxiStand>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentLookup = state.GetComponentLookup<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Creature_RO_ComponentLookup = state.GetComponentLookup<Creature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Animal_RO_ComponentLookup = state.GetComponentLookup<Animal>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupMember_RO_ComponentLookup = state.GetComponentLookup<GroupMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_HangaroundLocation_RO_ComponentLookup = state.GetComponentLookup<HangaroundLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PublicTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Train_RO_ComponentLookup = state.GetComponentLookup<Train>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RO_ComponentLookup = state.GetComponentLookup<PathOwner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CreatureData_RO_ComponentLookup = state.GetComponentLookup<CreatureData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimalData_RO_ComponentLookup = state.GetComponentLookup<AnimalData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferLookup = state.GetBufferLookup<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup = state.GetBufferLookup<ActivityLocationElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMeshGroup_RO_BufferLookup = state.GetBufferLookup<SubMeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CharacterElement_RO_BufferLookup = state.GetBufferLookup<CharacterElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimationClip_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.AnimationClip>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimationMotion_RO_BufferLookup = state.GetBufferLookup<AnimationMotion>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalCurrentLane_RW_ComponentLookup = state.GetComponentLookup<AnimalCurrentLane>();
      }
    }
  }
}
