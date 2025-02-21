// Decompiled with JetBrains decompiler
// Type: Game.Simulation.HumanNavigationSystem
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
using Game.Events;
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
  public class HumanNavigationSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private HumanNavigationSystem.Actions m_Actions;
    private EntityQuery m_CreatureQuery;
    private HumanNavigationSystem.TypeHandle __TypeHandle;

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
      this.m_Actions = this.World.GetOrCreateSystemManaged<HumanNavigationSystem.Actions>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatureQuery = this.GetEntityQuery(ComponentType.ReadOnly<Human>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadWrite<HumanCurrentLane>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint index = this.m_SimulationSystem.frameIndex % 16U;
      // ISSUE: reference to a compiler-generated field
      this.m_CreatureQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatureQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(index));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Actions.m_LaneSignalQueue = new NativeQueue<HumanNavigationHelpers.LaneSignal>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
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
      this.__TypeHandle.__Game_Prefabs_HumanData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CreatureData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_ActivityLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Routes_TakeoffLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_AreaLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneReservation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneSignal_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Queue_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Blocker_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanNavigation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Human_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new HumanNavigationSystem.UpdateNavigationJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_MovingType = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentTypeHandle,
        m_GroupMemberType = this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentTypeHandle,
        m_StumblingType = this.__TypeHandle.__Game_Creatures_Stumbling_RO_ComponentTypeHandle,
        m_TripSourceType = this.__TypeHandle.__Game_Objects_TripSource_RO_ComponentTypeHandle,
        m_HumanType = this.__TypeHandle.__Game_Creatures_Human_RO_ComponentTypeHandle,
        m_CurrentVehicleType = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle,
        m_InvolvedInAccidentType = this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_MeshGroupType = this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferTypeHandle,
        m_NavigationType = this.__TypeHandle.__Game_Creatures_HumanNavigation_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RW_ComponentTypeHandle,
        m_BlockerType = this.__TypeHandle.__Game_Vehicles_Blocker_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_QueueType = this.__TypeHandle.__Game_Creatures_Queue_RW_BufferTypeHandle,
        m_PathElementType = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PedestrianLaneData = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_LaneSignalData = this.__TypeHandle.__Game_Net_LaneSignal_RO_ComponentLookup,
        m_LaneReservationData = this.__TypeHandle.__Game_Net_LaneReservation_RO_ComponentLookup,
        m_AreaLaneData = this.__TypeHandle.__Game_Net_AreaLane_RO_ComponentLookup,
        m_WaypointData = this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup,
        m_TaxiStandData = this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentLookup,
        m_PositionData = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
        m_ConnectedData = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup,
        m_TakeoffLocationData = this.__TypeHandle.__Game_Routes_TakeoffLocation_RO_ComponentLookup,
        m_PropertyRenterData = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_MovingData = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_ActivityLocationData = this.__TypeHandle.__Game_Objects_ActivityLocation_RO_ComponentLookup,
        m_CreatureData = this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup,
        m_GroupMemberData = this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentLookup,
        m_HangaroundLocationData = this.__TypeHandle.__Game_Areas_HangaroundLocation_RO_ComponentLookup,
        m_PublicTransportData = this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup,
        m_TrainData = this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabCreatureData = this.__TypeHandle.__Game_Prefabs_CreatureData_RO_ComponentLookup,
        m_PrefabHumanData = this.__TypeHandle.__Game_Prefabs_HumanData_RO_ComponentLookup,
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
        m_PrefabActivityLocations = this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup,
        m_SubMeshGroups = this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup,
        m_CharacterElements = this.__TypeHandle.__Game_Prefabs_CharacterElement_RO_BufferLookup,
        m_AnimationClips = this.__TypeHandle.__Game_Prefabs_AnimationClip_RO_BufferLookup,
        m_AnimationMotions = this.__TypeHandle.__Game_Prefabs_AnimationMotion_RO_BufferLookup,
        m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_LeftHandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies1),
        m_StaticObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies2),
        m_MovingObjectSearchTree = this.m_ObjectSearchSystem.GetMovingSearchTree(true, out dependencies3),
        m_AreaSearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies4),
        m_LaneObjectBuffer = this.m_Actions.m_LaneObjectUpdater.Begin(Allocator.TempJob),
        m_LaneSignals = this.m_Actions.m_LaneSignalQueue.AsParallelWriter()
      }.ScheduleParallel<HumanNavigationSystem.UpdateNavigationJob>(this.m_CreatureQuery, JobUtils.CombineDependencies(this.Dependency, dependencies1, dependencies2, dependencies3, dependencies4));
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
    public HumanNavigationSystem()
    {
    }

    [BurstCompile]
    private struct GroupNavigationJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<GroupMember> m_GroupMemberType;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<HumanCurrentLane> m_CurrentLaneData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<PathOwner> m_PathOwnerData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<PathElement> m_Paths;

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
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          Entity entity = nativeArray2[index1];
          GroupMember groupMember = nativeArray1[index1];
          // ISSUE: reference to a compiler-generated field
          HumanCurrentLane humanCurrentLane1 = this.m_CurrentLaneData[entity];
          // ISSUE: reference to a compiler-generated field
          PathOwner pathOwner1 = this.m_PathOwnerData[entity];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<PathElement> path1 = this.m_Paths[entity];
          if (pathOwner1.m_ElementIndex > 0)
          {
            path1.RemoveRange(0, pathOwner1.m_ElementIndex);
            pathOwner1.m_ElementIndex = 0;
          }
          humanCurrentLane1.m_Flags &= ~CreatureLaneFlags.Leader;
          pathOwner1.m_State &= PathFlags.Stuck;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentVehicleData.HasComponent(groupMember.m_Leader))
          {
            if (path1.Length == 0 && (humanCurrentLane1.m_Flags & (CreatureLaneFlags.ParkingSpace | CreatureLaneFlags.Transport)) == (CreatureLaneFlags) 0)
              humanCurrentLane1.m_Flags |= CreatureLaneFlags.Transport;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurrentLaneData.HasComponent(groupMember.m_Leader))
            {
              // ISSUE: reference to a compiler-generated field
              HumanCurrentLane humanCurrentLane2 = this.m_CurrentLaneData[groupMember.m_Leader];
              // ISSUE: reference to a compiler-generated field
              PathOwner pathOwner2 = this.m_PathOwnerData[groupMember.m_Leader];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<PathElement> path2 = this.m_Paths[groupMember.m_Leader];
              if ((pathOwner2.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Obsolete | PathFlags.Updated)) == (PathFlags) 0)
              {
                int index2 = -1;
                if (humanCurrentLane1.m_Lane == humanCurrentLane2.m_Lane && (double) humanCurrentLane1.m_CurvePosition.y == (double) humanCurrentLane2.m_CurvePosition.y && ((humanCurrentLane1.m_Flags ^ humanCurrentLane2.m_Flags) & (CreatureLaneFlags.Taxi | CreatureLaneFlags.WaitPosition)) == (CreatureLaneFlags) 0)
                {
                  humanCurrentLane1.m_Flags |= CreatureLaneFlags.Leader;
                  index2 = 0;
                }
                else
                {
                  for (int index3 = 0; index3 < path1.Length; ++index3)
                  {
                    PathElement pathElement = path1[index3];
                    if (pathElement.m_Target == humanCurrentLane2.m_Lane && (double) pathElement.m_TargetDelta.y == (double) humanCurrentLane2.m_CurvePosition.y)
                    {
                      pathElement.m_Flags |= PathElementFlags.Leader;
                      path1[index3] = pathElement;
                      index2 = index3 + 1;
                      break;
                    }
                    pathElement.m_Flags &= ~PathElementFlags.Leader;
                    path1[index3] = pathElement;
                  }
                }
                if (index2 == -1)
                {
                  PathElementFlags flags = PathElementFlags.Leader;
                  if ((humanCurrentLane2.m_Flags & CreatureLaneFlags.Taxi) != (CreatureLaneFlags) 0)
                    flags |= PathElementFlags.Secondary;
                  if ((humanCurrentLane2.m_Flags & CreatureLaneFlags.WaitPosition) != (CreatureLaneFlags) 0)
                    flags |= PathElementFlags.WaitPosition;
                  path1.Clear();
                  path1.Add(new PathElement(humanCurrentLane2.m_Lane, humanCurrentLane2.m_CurvePosition, flags));
                }
                else if (index2 < path1.Length)
                  path1.RemoveRange(index2, path1.Length - index2);
                if ((humanCurrentLane2.m_Flags & CreatureLaneFlags.Area) != (CreatureLaneFlags) 0)
                {
                  Entity owner = Entity.Null;
                  Owner componentData;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_OwnerData.TryGetComponent(humanCurrentLane2.m_Lane, out componentData))
                    owner = componentData.m_Owner;
                  for (int elementIndex = pathOwner2.m_ElementIndex; elementIndex < path2.Length; ++elementIndex)
                  {
                    PathElement elem = path2[elementIndex];
                    path1.Add(elem);
                    // ISSUE: reference to a compiler-generated field
                    if (!this.m_OwnerData.TryGetComponent(elem.m_Target, out componentData) || componentData.m_Owner != owner)
                      break;
                  }
                }
                else
                {
                  int num = math.min(pathOwner2.m_ElementIndex + 2, path2.Length);
                  for (int elementIndex = pathOwner2.m_ElementIndex; elementIndex < num; ++elementIndex)
                    path1.Add(path2[elementIndex]);
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CurrentLaneData[entity] = humanCurrentLane1;
          // ISSUE: reference to a compiler-generated field
          this.m_PathOwnerData[entity] = pathOwner1;
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
      public ComponentTypeHandle<Human> m_HumanType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> m_CurrentVehicleType;
      [ReadOnly]
      public ComponentTypeHandle<InvolvedInAccident> m_InvolvedInAccidentType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<MeshGroup> m_MeshGroupType;
      public ComponentTypeHandle<HumanNavigation> m_NavigationType;
      public ComponentTypeHandle<HumanCurrentLane> m_CurrentLaneType;
      public ComponentTypeHandle<Blocker> m_BlockerType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public BufferTypeHandle<Queue> m_QueueType;
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> m_PedestrianLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneSignal> m_LaneSignalData;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneReservation> m_LaneReservationData;
      [ReadOnly]
      public ComponentLookup<AreaLane> m_AreaLaneData;
      [ReadOnly]
      public ComponentLookup<Waypoint> m_WaypointData;
      [ReadOnly]
      public ComponentLookup<TaxiStand> m_TaxiStandData;
      [ReadOnly]
      public ComponentLookup<Position> m_PositionData;
      [ReadOnly]
      public ComponentLookup<Connected> m_ConnectedData;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TakeoffLocation> m_TakeoffLocationData;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenterData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Moving> m_MovingData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.ActivityLocation> m_ActivityLocationData;
      [ReadOnly]
      public ComponentLookup<Creature> m_CreatureData;
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
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<CreatureData> m_PrefabCreatureData;
      [ReadOnly]
      public ComponentLookup<HumanData> m_PrefabHumanData;
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
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public bool m_LeftHandTraffic;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_StaticObjectSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_MovingObjectSearchTree;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      public LaneObjectCommandBuffer m_LaneObjectBuffer;
      public NativeQueue<HumanNavigationHelpers.LaneSignal>.ParallelWriter m_LaneSignals;

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
        NativeArray<HumanNavigation> nativeArray5 = chunk.GetNativeArray<HumanNavigation>(ref this.m_NavigationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HumanCurrentLane> nativeArray6 = chunk.GetNativeArray<HumanCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Blocker> nativeArray7 = chunk.GetNativeArray<Blocker>(ref this.m_BlockerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray8 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray9 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Queue> bufferAccessor1 = chunk.GetBufferAccessor<Queue>(ref this.m_QueueType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<PathElement> bufferAccessor2 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<MeshGroup> bufferAccessor3 = chunk.GetBufferAccessor<MeshGroup>(ref this.m_MeshGroupType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Stumbling>(ref this.m_StumblingType))
        {
          for (int index = 0; index < chunk.Count; ++index)
          {
            Entity entity = nativeArray1[index];
            Game.Objects.Transform transform = nativeArray2[index];
            HumanNavigation navigation = nativeArray5[index];
            HumanCurrentLane currentLane = nativeArray6[index];
            Blocker blocker = nativeArray7[index];
            PathOwner pathOwner = nativeArray8[index];
            PrefabRef prefabRef = nativeArray9[index];
            DynamicBuffer<Queue> dynamicBuffer = bufferAccessor1[index];
            DynamicBuffer<PathElement> pathElements = bufferAccessor2[index];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
            Moving moving;
            CollectionUtils.TryGet<Moving>(nativeArray3, index, out moving);
            GroupMember groupMember;
            CollectionUtils.TryGet<GroupMember>(nativeArray4, index, out groupMember);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            HumanNavigationHelpers.CurrentLaneCache currentLaneCache = new HumanNavigationHelpers.CurrentLaneCache(ref currentLane, this.m_EntityLookup, this.m_MovingObjectSearchTree);
            dynamicBuffer.Clear();
            // ISSUE: reference to a compiler-generated method
            this.UpdateStumbling(entity, transform, groupMember, objectGeometryData, ref navigation, ref currentLane, ref blocker, ref pathOwner, pathElements);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            currentLaneCache.CheckChanges(entity, ref currentLane, this.m_LaneObjectBuffer, this.m_LaneObjects, transform, moving, navigation, objectGeometryData);
            nativeArray5[index] = navigation;
            nativeArray6[index] = currentLane;
            nativeArray7[index] = blocker;
            nativeArray8[index] = pathOwner;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<TripSource> nativeArray10 = chunk.GetNativeArray<TripSource>(ref this.m_TripSourceType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Human> nativeArray11 = chunk.GetNativeArray<Human>(ref this.m_HumanType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<CurrentVehicle> nativeArray12 = chunk.GetNativeArray<CurrentVehicle>(ref this.m_CurrentVehicleType);
          // ISSUE: reference to a compiler-generated field
          bool isInvolvedInAccident = chunk.Has<InvolvedInAccident>(ref this.m_InvolvedInAccidentType);
          for (int index = 0; index < chunk.Count; ++index)
          {
            Entity entity = nativeArray1[index];
            Game.Objects.Transform transform = nativeArray2[index];
            Moving moving = nativeArray3[index];
            Human human = nativeArray11[index];
            HumanNavigation navigation = nativeArray5[index];
            HumanCurrentLane currentLane = nativeArray6[index];
            Blocker blocker = nativeArray7[index];
            PathOwner pathOwner = nativeArray8[index];
            PrefabRef prefabRef = nativeArray9[index];
            DynamicBuffer<Queue> queues = bufferAccessor1[index];
            DynamicBuffer<PathElement> pathElements = bufferAccessor2[index];
            // ISSUE: reference to a compiler-generated field
            CreatureData prefabCreatureData = this.m_PrefabCreatureData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            HumanData prefabHumanData = this.m_PrefabHumanData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
            GroupMember groupMember;
            CollectionUtils.TryGet<GroupMember>(nativeArray4, index, out groupMember);
            TripSource tripSource;
            CollectionUtils.TryGet<TripSource>(nativeArray10, index, out tripSource);
            CurrentVehicle currentVehicle;
            CollectionUtils.TryGet<CurrentVehicle>(nativeArray12, index, out currentVehicle);
            DynamicBuffer<MeshGroup> meshGroups;
            CollectionUtils.TryGet<MeshGroup>(bufferAccessor3, index, out meshGroups);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            HumanNavigationHelpers.CurrentLaneCache currentLaneCache = new HumanNavigationHelpers.CurrentLaneCache(ref currentLane, this.m_EntityLookup, this.m_MovingObjectSearchTree);
            if ((currentLane.m_Lane == Entity.Null || (currentLane.m_Flags & CreatureLaneFlags.Obsolete) != (CreatureLaneFlags) 0) && (human.m_Flags & HumanFlags.Carried) == (HumanFlags) 0)
            {
              if ((currentLane.m_Flags & (CreatureLaneFlags.Obsolete | CreatureLaneFlags.FindLane)) == CreatureLaneFlags.FindLane)
              {
                // ISSUE: reference to a compiler-generated method
                this.TryFindCurrentLane(ref currentLane, transform);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.TryFindCurrentLane(ref currentLane, transform);
                if (groupMember.m_Leader == Entity.Null)
                {
                  pathElements.Clear();
                  pathOwner.m_ElementIndex = 0;
                  pathOwner.m_State |= PathFlags.Obsolete;
                }
              }
            }
            // ISSUE: reference to a compiler-generated method
            this.UpdateQueues(currentVehicle, ref pathOwner, queues);
            // ISSUE: reference to a compiler-generated method
            this.UpdateNavigationTarget(ref random, isInvolvedInAccident, entity, transform, moving, tripSource, currentVehicle, human, groupMember, prefabRef, prefabCreatureData, prefabHumanData, objectGeometryData, ref navigation, ref currentLane, ref blocker, ref pathOwner, queues, pathElements, meshGroups);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            currentLaneCache.CheckChanges(entity, ref currentLane, this.m_LaneObjectBuffer, this.m_LaneObjects, transform, moving, navigation, objectGeometryData);
            nativeArray5[index] = navigation;
            nativeArray6[index] = currentLane;
            nativeArray7[index] = blocker;
            nativeArray8[index] = pathOwner;
          }
        }
      }

      private void UpdateStumbling(
        Entity entity,
        Game.Objects.Transform transform,
        GroupMember groupMember,
        ObjectGeometryData prefabObjectGeometryData,
        ref HumanNavigation navigation,
        ref HumanCurrentLane currentLane,
        ref Blocker blocker,
        ref PathOwner pathOwner,
        DynamicBuffer<PathElement> pathElements)
      {
        // ISSUE: reference to a compiler-generated method
        this.TryFindCurrentLane(ref currentLane, transform);
        navigation = new HumanNavigation()
        {
          m_TargetPosition = transform.m_Position
        };
        blocker = new Blocker();
        pathOwner.m_ElementIndex = 0;
        pathElements.Clear();
        if (!(groupMember.m_Leader == Entity.Null))
          return;
        pathOwner.m_State |= PathFlags.Obsolete;
      }

      private void TryFindCurrentLane(ref HumanCurrentLane currentLane, Game.Objects.Transform transform)
      {
        bool flag = (currentLane.m_Flags & CreatureLaneFlags.EmergeUnspawned) > (CreatureLaneFlags) 0;
        currentLane.m_Flags &= ~(CreatureLaneFlags.EndOfPath | CreatureLaneFlags.EndReached | CreatureLaneFlags.TransformTarget | CreatureLaneFlags.ParkingSpace | CreatureLaneFlags.Obsolete | CreatureLaneFlags.Transport | CreatureLaneFlags.Connection | CreatureLaneFlags.Taxi | CreatureLaneFlags.FindLane | CreatureLaneFlags.Area | CreatureLaneFlags.Hangaround | CreatureLaneFlags.WaitPosition | CreatureLaneFlags.EmergeUnspawned);
        currentLane.m_Lane = Entity.Null;
        float3 position = transform.m_Position;
        Bounds3 bounds3 = new Bounds3(position - 100f, position + 100f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        HumanNavigationHelpers.FindLaneIterator iterator = new HumanNavigationHelpers.FindLaneIterator()
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
        this.m_NetSearchTree.Iterate<HumanNavigationHelpers.FindLaneIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        this.m_StaticObjectSearchTree.Iterate<HumanNavigationHelpers.FindLaneIterator>(ref iterator);
        if (!flag)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_AreaSearchTree.Iterate<HumanNavigationHelpers.FindLaneIterator>(ref iterator);
        }
        currentLane = iterator.m_Result;
      }

      private float GetTargetSpeed(
        TripSource tripSource,
        Human human,
        HumanData prefabHumanData,
        ref HumanCurrentLane currentLane)
      {
        float targetSpeed = 0.0f;
        if (tripSource.m_Source == Entity.Null)
        {
          if ((human.m_Flags & HumanFlags.Run) != (HumanFlags) 0)
            return prefabHumanData.m_RunSpeed;
          targetSpeed = prefabHumanData.m_WalkSpeed;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_PedestrianLaneData.HasComponent(currentLane.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.PedestrianLane pedestrianLane = this.m_PedestrianLaneData[currentLane.m_Lane];
          if ((pedestrianLane.m_Flags & PedestrianLaneFlags.Unsafe) != (PedestrianLaneFlags) 0)
            return prefabHumanData.m_RunSpeed;
          if ((pedestrianLane.m_Flags & PedestrianLaneFlags.Crosswalk) != (PedestrianLaneFlags) 0)
            targetSpeed = math.lerp(prefabHumanData.m_WalkSpeed, prefabHumanData.m_RunSpeed, 0.25f);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_LaneSignalData.HasComponent(currentLane.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.LaneSignal laneSignal = this.m_LaneSignalData[currentLane.m_Lane];
          if (laneSignal.m_Signal == LaneSignalType.SafeStop || laneSignal.m_Signal == LaneSignalType.Stop)
            return prefabHumanData.m_RunSpeed;
        }
        return targetSpeed;
      }

      private void UpdateQueues(
        CurrentVehicle currentVehicle,
        ref PathOwner pathOwner,
        DynamicBuffer<Queue> queues)
      {
        if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Obsolete | PathFlags.Updated)) != (PathFlags) 0 || currentVehicle.m_Vehicle != Entity.Null)
        {
          queues.Clear();
        }
        else
        {
          for (int index = 0; index < queues.Length; ++index)
          {
            Queue queue = queues[index];
            if (++queue.m_ObsoleteTime >= (ushort) 500)
              queues.RemoveAt(index--);
            else
              queues[index] = queue;
          }
        }
      }

      private void UpdateNavigationTarget(
        ref Unity.Mathematics.Random random,
        bool isInvolvedInAccident,
        Entity entity,
        Game.Objects.Transform transform,
        Moving moving,
        TripSource tripSource,
        CurrentVehicle currentVehicle,
        Human human,
        GroupMember groupMember,
        PrefabRef prefabRef,
        CreatureData prefabCreatureData,
        HumanData prefabHumanData,
        ObjectGeometryData prefabObjectGeometryData,
        ref HumanNavigation navigation,
        ref HumanCurrentLane currentLane,
        ref Blocker blocker,
        ref PathOwner pathOwner,
        DynamicBuffer<Queue> queues,
        DynamicBuffer<PathElement> pathElements,
        DynamicBuffer<MeshGroup> meshGroups)
      {
        float timeStep = 0.266666681f;
        float x = math.length(moving.m_Velocity);
        if ((currentLane.m_Flags & CreatureLaneFlags.Connection) != (CreatureLaneFlags) 0)
        {
          prefabHumanData.m_WalkSpeed = 277.777771f;
          prefabHumanData.m_RunSpeed = 277.777771f;
          prefabHumanData.m_Acceleration = 277.777771f;
        }
        else
          x = math.min(x, prefabHumanData.m_RunSpeed);
        Bounds1 bounds1 = new Bounds1(x + new float2(-prefabHumanData.m_Acceleration, prefabHumanData.m_Acceleration) * timeStep);
        // ISSUE: reference to a compiler-generated method
        float targetSpeed = this.GetTargetSpeed(tripSource, human, prefabHumanData, ref currentLane);
        float y1 = prefabHumanData.m_Acceleration * 0.1f;
        if ((double) x <= (double) prefabHumanData.m_WalkSpeed)
        {
          float position = math.min(targetSpeed, math.max(prefabHumanData.m_WalkSpeed, x + y1 * timeStep));
          navigation.m_MaxSpeed = MathUtils.Clamp(position, bounds1);
        }
        else
        {
          Bounds1 bounds2 = new Bounds1(x + new float2(-y1, y1) * timeStep);
          navigation.m_MaxSpeed = MathUtils.Clamp(targetSpeed, bounds2);
        }
        float num1 = math.max(prefabObjectGeometryData.m_Bounds.max.z, (float) (((double) prefabObjectGeometryData.m_Bounds.max.x - (double) prefabObjectGeometryData.m_Bounds.min.x) * 0.5));
        float a;
        if ((currentLane.m_Flags & (CreatureLaneFlags.EndReached | CreatureLaneFlags.TransformTarget | CreatureLaneFlags.Area)) != (CreatureLaneFlags) 0 || currentLane.m_Lane == Entity.Null || (currentLane.m_Flags & CreatureLaneFlags.Connection) != (CreatureLaneFlags) 0 && (currentLane.m_Flags & (CreatureLaneFlags.ParkingSpace | CreatureLaneFlags.WaitPosition)) != (CreatureLaneFlags) 0)
        {
          a = math.distance(transform.m_Position, navigation.m_TargetPosition);
          float distance = math.select(a, math.max(0.0f, a - num1), (currentLane.m_Flags & CreatureLaneFlags.TransformTarget) == (CreatureLaneFlags) 0);
          float y2 = MathUtils.Clamp(CreatureUtils.GetMaxBrakingSpeed(prefabHumanData, distance, timeStep), bounds1);
          navigation.m_MaxSpeed = math.min(navigation.m_MaxSpeed, y2);
        }
        else
        {
          if ((currentLane.m_Flags & CreatureLaneFlags.WaitSignal) != (CreatureLaneFlags) 0)
          {
            navigation.m_TargetPosition = transform.m_Position;
            navigation.m_TargetDirection = new float2();
            navigation.m_TargetActivity = (byte) 0;
            a = 0.0f;
            if (pathOwner.m_ElementIndex < pathElements.Length)
            {
              PathElement pathElement = pathElements[pathOwner.m_ElementIndex];
              // ISSUE: reference to a compiler-generated field
              if (this.m_CurveData.HasComponent(pathElement.m_Target))
              {
                float lanePosition = math.select(currentLane.m_LanePosition, -currentLane.m_LanePosition, (currentLane.m_Flags & CreatureLaneFlags.Backward) > (CreatureLaneFlags) 0 != (double) pathElement.m_TargetDelta.y < (double) pathElement.m_TargetDelta.x);
                // ISSUE: reference to a compiler-generated method
                Line3.Segment targetPos = this.CalculateTargetPos(prefabObjectGeometryData, pathElement.m_Target, pathElement.m_TargetDelta, lanePosition);
                navigation.m_TargetPosition = targetPos.a;
                navigation.m_TargetDirection = math.normalizesafe(targetPos.b.xz - targetPos.a.xz);
                a = math.distance(transform.m_Position, navigation.m_TargetPosition);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            navigation.m_TargetPosition = this.CalculateTargetPos(prefabObjectGeometryData, currentLane.m_Lane, currentLane.m_CurvePosition.x, currentLane.m_LanePosition);
            navigation.m_TargetDirection = new float2();
            navigation.m_TargetActivity = (byte) 0;
            a = math.distance(transform.m_Position, navigation.m_TargetPosition);
          }
          float brakingDistance = CreatureUtils.GetBrakingDistance(prefabHumanData, navigation.m_MaxSpeed, timeStep);
          float distance = math.max(0.0f, a - num1);
          if ((double) distance < (double) brakingDistance)
          {
            float y3 = MathUtils.Clamp(CreatureUtils.GetMaxBrakingSpeed(prefabHumanData, distance, timeStep), bounds1);
            navigation.m_MaxSpeed = math.min(navigation.m_MaxSpeed, y3);
          }
        }
        navigation.m_MaxSpeed = math.select(navigation.m_MaxSpeed, 0.0f, (double) navigation.m_MaxSpeed < 0.10000000149011612);
        Entity blocker1 = blocker.m_Blocker;
        float maxSpeed = navigation.m_MaxSpeed;
        blocker.m_Blocker = Entity.Null;
        blocker.m_Type = BlockerType.None;
        currentLane.m_QueueEntity = Entity.Null;
        currentLane.m_QueueArea = new Sphere3();
        float minDistance = num1 + math.max(1f, navigation.m_MaxSpeed * timeStep) + CreatureUtils.GetBrakingDistance(prefabHumanData, navigation.m_MaxSpeed, timeStep);
        if ((double) x >= 0.10000000149011612)
        {
          float num2 = x * timeStep;
          float num3 = random.NextFloat(0.0f, 1f);
          float num4 = num3 * num3;
          // ISSUE: reference to a compiler-generated field
          float y4 = math.select(0.5f - num4, num4 - 0.5f, this.m_LeftHandTraffic != (currentLane.m_Flags & CreatureLaneFlags.Backward) > (CreatureLaneFlags) 0);
          currentLane.m_LanePosition = math.lerp(currentLane.m_LanePosition, y4, math.min(1f, num2 * 0.01f));
        }
        if ((double) a < (double) minDistance)
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
          PathElement pathElement;
          CreatureLaneFlags creatureLaneFlags;
          Game.Net.LaneSignal componentData1;
          while (true)
          {
            do
            {
              byte activity1 = 0;
              if ((currentLane.m_Flags & (CreatureLaneFlags.EndReached | CreatureLaneFlags.WaitSignal)) == (CreatureLaneFlags) 0 && currentLane.m_Lane != Entity.Null)
              {
                if ((currentLane.m_Flags & CreatureLaneFlags.TransformTarget) != (CreatureLaneFlags) 0)
                {
                  if ((currentLane.m_Flags & CreatureLaneFlags.WaitPosition) != (CreatureLaneFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    if (this.MoveTransformTarget(entity, prefabRef.m_Prefab, meshGroups, ref random, human, currentVehicle, transform.m_Position, ref navigation.m_TargetPosition, ref navigation.m_TargetDirection, ref activity1, 0.0f, currentLane.m_Lane, prefabCreatureData.m_SupportedActivities))
                    {
                      navigation.m_TargetPosition = VehicleUtils.GetConnectionParkingPosition(new Game.Net.ConnectionLane(), new Bezier4x3(navigation.m_TargetPosition, navigation.m_TargetPosition, navigation.m_TargetPosition, navigation.m_TargetPosition), currentLane.m_CurvePosition.y);
                      navigation.m_TargetDirection = new float2();
                      navigation.m_TargetActivity = (byte) 0;
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated method
                    if (this.MoveTransformTarget(entity, prefabRef.m_Prefab, meshGroups, ref random, human, currentVehicle, transform.m_Position, ref navigation.m_TargetPosition, ref navigation.m_TargetDirection, ref activity1, minDistance, currentLane.m_Lane, prefabCreatureData.m_SupportedActivities))
                      goto label_115;
                  }
                }
                else if ((currentLane.m_Flags & CreatureLaneFlags.Connection) != (CreatureLaneFlags) 0 && (currentLane.m_Flags & (CreatureLaneFlags.ParkingSpace | CreatureLaneFlags.WaitPosition)) != (CreatureLaneFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  Curve curve = this.m_CurveData[currentLane.m_Lane];
                  // ISSUE: reference to a compiler-generated field
                  Game.Net.ConnectionLane connectionLane = this.m_ConnectionLaneData[currentLane.m_Lane];
                  navigation.m_TargetPosition = VehicleUtils.GetConnectionParkingPosition(connectionLane, curve.m_Bezier, currentLane.m_CurvePosition.y);
                  navigation.m_TargetDirection = new float2();
                  navigation.m_TargetActivity = (byte) 0;
                }
                else if ((currentLane.m_Flags & CreatureLaneFlags.Area) != (CreatureLaneFlags) 0)
                {
                  navigation.m_TargetActivity = (byte) 0;
                  float navigationSize = CreatureUtils.GetNavigationSize(prefabObjectGeometryData);
                  // ISSUE: reference to a compiler-generated method
                  if (this.MoveAreaTarget(ref random, transform.m_Position, pathOwner, pathElements, ref navigation.m_TargetPosition, ref navigation.m_TargetDirection, ref activity1, minDistance, currentLane.m_Lane, prefabCreatureData.m_SupportedActivities, ref currentLane.m_CurvePosition, currentLane.m_LanePosition, navigationSize))
                    goto label_115;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  Curve curve = this.m_CurveData[currentLane.m_Lane];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  NetLaneData prefabLaneData = this.m_PrefabLaneData[this.m_PrefabRefData[currentLane.m_Lane].m_Prefab];
                  float laneOffset = CreatureUtils.GetLaneOffset(prefabObjectGeometryData, prefabLaneData, currentLane.m_LanePosition);
                  navigation.m_TargetDirection = new float2();
                  navigation.m_TargetActivity = (byte) 0;
                  // ISSUE: reference to a compiler-generated method
                  if (this.MoveLaneTarget(ref targetIterator, currentLane.m_Lane, transform.m_Position, ref navigation.m_TargetPosition, minDistance, curve.m_Bezier, ref currentLane.m_CurvePosition, laneOffset))
                    goto label_115;
                }
              }
              if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Obsolete | PathFlags.Updated)) != 0 | isInvolvedInAccident)
              {
                if ((currentLane.m_Flags & CreatureLaneFlags.Action) != (CreatureLaneFlags) 0)
                {
                  if ((currentLane.m_Flags & CreatureLaneFlags.EndReached) != (CreatureLaneFlags) 0)
                  {
                    if (navigation.m_TargetActivity == (byte) 0 || navigation.m_TransformState == TransformState.Idle)
                    {
                      navigation.m_TargetActivity = (byte) 0;
                      currentLane.m_Flags |= CreatureLaneFlags.ActivityDone;
                      goto label_115;
                    }
                    else
                      goto label_115;
                  }
                  else
                  {
                    currentLane.m_Flags &= ~CreatureLaneFlags.Action;
                    goto label_115;
                  }
                }
                else
                  goto label_115;
              }
              else if ((currentLane.m_Flags & CreatureLaneFlags.EndOfPath) != (CreatureLaneFlags) 0 || pathOwner.m_ElementIndex >= pathElements.Length)
              {
                if (groupMember.m_Leader != Entity.Null)
                {
                  if ((currentLane.m_Flags & CreatureLaneFlags.EndOfPath) == (CreatureLaneFlags) 0)
                  {
                    targetIterator.m_Blocker = groupMember.m_Leader;
                    targetIterator.m_BlockerType = BlockerType.Continuing;
                  }
                  else if (pathOwner.m_ElementIndex < pathElements.Length)
                    currentLane.m_Flags &= ~CreatureLaneFlags.EndOfPath;
                }
                else
                {
                  currentLane.m_Flags &= ~(CreatureLaneFlags.ParkingSpace | CreatureLaneFlags.Transport | CreatureLaneFlags.Taxi);
                  currentLane.m_Flags |= CreatureLaneFlags.EndOfPath;
                }
                if ((double) math.distance(transform.m_Position, navigation.m_TargetPosition) < (double) math.select(0.1f, num1 + 1f, (currentLane.m_Flags & CreatureLaneFlags.TransformTarget) == (CreatureLaneFlags) 0) && (double) x < 0.10000000149011612)
                {
                  if ((currentLane.m_Flags & CreatureLaneFlags.EndReached) != (CreatureLaneFlags) 0)
                  {
                    if (navigation.m_TargetActivity == (byte) 0)
                    {
                      currentLane.m_Flags |= CreatureLaneFlags.ActivityDone;
                      goto label_115;
                    }
                    else
                      goto label_115;
                  }
                  else
                  {
                    navigation.m_TargetActivity = activity1;
                    currentLane.m_Flags |= CreatureLaneFlags.EndReached;
                    if (navigation.m_TargetActivity == (byte) 0)
                    {
                      currentLane.m_Flags |= CreatureLaneFlags.ActivityDone;
                      goto label_115;
                    }
                    else
                      goto label_115;
                  }
                }
                else
                  goto label_115;
              }
              else
              {
                pathElement = pathElements[pathOwner.m_ElementIndex];
                creatureLaneFlags = (double) pathElement.m_TargetDelta.y < (double) pathElement.m_TargetDelta.x ? CreatureLaneFlags.Backward : (CreatureLaneFlags) 0;
                if ((pathElement.m_Flags & PathElementFlags.Leader) != (PathElementFlags) 0)
                  creatureLaneFlags |= CreatureLaneFlags.Leader;
                currentLane.m_Flags &= ~(CreatureLaneFlags.ParkingSpace | CreatureLaneFlags.Transport | CreatureLaneFlags.Taxi | CreatureLaneFlags.Action);
                if ((pathElement.m_Flags & PathElementFlags.Action) != (PathElementFlags) 0)
                {
                  currentLane.m_Flags |= CreatureLaneFlags.Action;
                  if ((double) math.distance(transform.m_Position, navigation.m_TargetPosition) < (double) math.select(0.1f, num1 + 1f, (currentLane.m_Flags & CreatureLaneFlags.TransformTarget) == (CreatureLaneFlags) 0) && (double) x < 0.10000000149011612)
                  {
                    if ((currentLane.m_Flags & CreatureLaneFlags.EndReached) != (CreatureLaneFlags) 0)
                    {
                      if (navigation.m_TargetActivity == (byte) 0 || navigation.m_TransformState == TransformState.Idle)
                      {
                        navigation.m_TargetActivity = (byte) 0;
                        currentLane.m_Flags |= CreatureLaneFlags.ActivityDone;
                        goto label_115;
                      }
                      else
                        goto label_115;
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.SetActionTarget(ref navigation, transform, human, pathElement);
                      currentLane.m_Flags &= ~CreatureLaneFlags.ActivityDone;
                      currentLane.m_Flags |= CreatureLaneFlags.EndReached;
                      goto label_115;
                    }
                  }
                  else
                    goto label_115;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_PedestrianLaneData.HasComponent(pathElement.m_Target))
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_ParkingLaneData.HasComponent(pathElement.m_Target))
                    {
                      currentLane.m_Flags |= CreatureLaneFlags.ParkingSpace;
                      // ISSUE: reference to a compiler-generated field
                      if ((pathElement.m_Flags & PathElementFlags.Secondary) != (PathElementFlags) 0 && (this.m_ParkingLaneData[pathElement.m_Target].m_Flags & ParkingLaneFlags.SecondaryStart) == (ParkingLaneFlags) 0)
                        currentLane.m_Flags |= CreatureLaneFlags.Taxi;
                      if ((double) math.distance(transform.m_Position, navigation.m_TargetPosition) < (double) math.select(0.1f, num1 + 1f, (currentLane.m_Flags & CreatureLaneFlags.TransformTarget) == (CreatureLaneFlags) 0) && (double) x < 0.10000000149011612)
                      {
                        currentLane.m_Flags |= CreatureLaneFlags.EndReached;
                        goto label_115;
                      }
                      else
                        goto label_115;
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_WaypointData.HasComponent(pathElement.m_Target) || this.m_TaxiStandData.HasComponent(pathElement.m_Target))
                      {
                        currentLane.m_Flags |= CreatureLaneFlags.Transport;
                        if ((currentLane.m_Flags & CreatureLaneFlags.EndReached) == (CreatureLaneFlags) 0)
                        {
                          a = math.distance(transform.m_Position, navigation.m_TargetPosition);
                          float num5 = math.select(0.1f, num1 + 1f, (currentLane.m_Flags & CreatureLaneFlags.TransformTarget) == (CreatureLaneFlags) 0);
                          if ((double) a < (double) num5 && (double) x < 0.10000000149011612)
                          {
                            if ((currentLane.m_Flags & CreatureLaneFlags.TransformTarget) == (CreatureLaneFlags) 0)
                            {
                              Entity entity1 = pathElement.m_Target;
                              // ISSUE: reference to a compiler-generated field
                              if (this.m_ConnectedData.HasComponent(entity1))
                              {
                                // ISSUE: reference to a compiler-generated field
                                entity1 = this.m_ConnectedData[entity1].m_Connected;
                              }
                              byte activity2 = 0;
                              float3 targetPosition = navigation.m_TargetPosition;
                              float2 targetDirection = navigation.m_TargetDirection;
                              // ISSUE: reference to a compiler-generated method
                              this.MoveTransformTarget(entity, prefabRef.m_Prefab, meshGroups, ref random, human, currentVehicle, transform.m_Position, ref targetPosition, ref targetDirection, ref activity2, minDistance, entity1, prefabCreatureData.m_SupportedActivities);
                              if (activity2 != (byte) 0)
                              {
                                currentLane.m_Lane = entity1;
                                currentLane.m_CurvePosition = (float2) 0.0f;
                                currentLane.m_Flags = CreatureLaneFlags.TransformTarget;
                                navigation.m_TargetPosition = targetPosition;
                                navigation.m_TargetDirection = targetDirection;
                                continue;
                              }
                            }
                            if (activity1 == (byte) 0)
                            {
                              navigation.m_TargetPosition = transform.m_Position;
                              Position componentData2;
                              // ISSUE: reference to a compiler-generated field
                              if (this.m_PositionData.TryGetComponent(pathElement.m_Target, out componentData2))
                              {
                                navigation.m_TargetDirection = math.normalizesafe(componentData2.m_Position.xz - transform.m_Position.xz);
                              }
                              else
                              {
                                Game.Objects.Transform componentData3;
                                // ISSUE: reference to a compiler-generated field
                                if (this.m_TransformData.TryGetComponent(pathElement.m_Target, out componentData3))
                                  navigation.m_TargetDirection = math.normalizesafe(componentData3.m_Position.xz - transform.m_Position.xz);
                              }
                            }
                            navigation.m_TargetActivity = activity1;
                            currentLane.m_Flags |= CreatureLaneFlags.EndReached;
                            goto label_115;
                          }
                          else
                            goto label_115;
                        }
                        else
                          goto label_115;
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_ConnectionLaneData.HasComponent(pathElement.m_Target))
                        {
                          // ISSUE: reference to a compiler-generated field
                          Game.Net.ConnectionLane connectionLane = this.m_ConnectionLaneData[pathElement.m_Target];
                          if ((connectionLane.m_Flags & ConnectionLaneFlags.Parking) != (ConnectionLaneFlags) 0)
                          {
                            currentLane.m_Flags |= CreatureLaneFlags.ParkingSpace;
                            if ((pathElement.m_Flags & PathElementFlags.Secondary) != (PathElementFlags) 0)
                              currentLane.m_Flags |= CreatureLaneFlags.Taxi;
                            if ((double) math.distance(transform.m_Position, navigation.m_TargetPosition) < (double) math.select(0.1f, num1 + 1f, (currentLane.m_Flags & CreatureLaneFlags.TransformTarget) == (CreatureLaneFlags) 0) && (double) x < 0.10000000149011612)
                            {
                              currentLane.m_Flags |= CreatureLaneFlags.EndReached;
                              goto label_115;
                            }
                            else
                              goto label_115;
                          }
                          else
                          {
                            if ((pathElement.m_Flags & PathElementFlags.WaitPosition) != (PathElementFlags) 0)
                              creatureLaneFlags |= CreatureLaneFlags.WaitPosition;
                            if ((connectionLane.m_Flags & ConnectionLaneFlags.Area) != (ConnectionLaneFlags) 0)
                            {
                              creatureLaneFlags |= CreatureLaneFlags.Area;
                              // ISSUE: reference to a compiler-generated field
                              // ISSUE: reference to a compiler-generated field
                              // ISSUE: reference to a compiler-generated field
                              if (this.m_OwnerData.HasComponent(pathElement.m_Target) && this.m_HangaroundLocationData.HasComponent(this.m_OwnerData[pathElement.m_Target].m_Owner))
                              {
                                creatureLaneFlags |= CreatureLaneFlags.Hangaround;
                                goto label_103;
                              }
                              else
                                goto label_103;
                            }
                            else
                            {
                              creatureLaneFlags |= CreatureLaneFlags.Connection;
                              goto label_103;
                            }
                          }
                        }
                        else
                        {
                          // ISSUE: reference to a compiler-generated field
                          if (this.m_SpawnLocationData.HasComponent(pathElement.m_Target))
                          {
                            ++pathOwner.m_ElementIndex;
                            currentLane.m_Lane = pathElement.m_Target;
                            currentLane.m_CurvePosition = (float2) 0.0f;
                            currentLane.m_Flags = CreatureLaneFlags.TransformTarget;
                            // ISSUE: reference to a compiler-generated field
                            if (this.m_ActivityLocationData.HasComponent(pathElement.m_Target))
                              currentLane.m_Flags |= CreatureLaneFlags.Hangaround;
                            if ((pathElement.m_Flags & PathElementFlags.WaitPosition) != (PathElementFlags) 0)
                            {
                              currentLane.m_CurvePosition = (float2) pathElement.m_TargetDelta.y;
                              currentLane.m_Flags |= CreatureLaneFlags.WaitPosition;
                            }
                          }
                          else
                            goto label_96;
                        }
                      }
                    }
                  }
                  else
                    goto label_100;
                }
              }
            }
            while (pathOwner.m_ElementIndex < pathElements.Length);
            currentLane.m_Flags |= CreatureLaneFlags.EndOfPath;
            continue;
label_96:
            // ISSUE: reference to a compiler-generated field
            if (this.m_TakeoffLocationData.HasComponent(pathElement.m_Target))
            {
              ++pathOwner.m_ElementIndex;
              continue;
            }
            // ISSUE: reference to a compiler-generated method
            if (this.GetTransformTarget(ref currentLane.m_Lane, pathElement.m_Target))
            {
              ++pathOwner.m_ElementIndex;
              navigation.m_TargetActivity = (byte) 0;
              currentLane.m_CurvePosition = (float2) 0.0f;
              currentLane.m_Flags = CreatureLaneFlags.EndOfPath | CreatureLaneFlags.TransformTarget;
              continue;
            }
            goto label_103;
label_100:
            // ISSUE: reference to a compiler-generated field
            if (pathElement.m_Target != currentLane.m_Lane && (human.m_Flags & HumanFlags.Emergency) == (HumanFlags) 0 && this.m_LaneSignalData.TryGetComponent(pathElement.m_Target, out componentData1))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LaneSignals.Enqueue(new HumanNavigationHelpers.LaneSignal(entity, pathElement.m_Target, 100));
              if (componentData1.m_Signal == LaneSignalType.Stop || componentData1.m_Signal == LaneSignalType.SafeStop)
                break;
            }
label_103:
            if (((currentLane.m_Flags & ~creatureLaneFlags & CreatureLaneFlags.Connection) == (CreatureLaneFlags) 0 || (double) a < (double) num1 + 1.0) && (!(groupMember.m_Leader != Entity.Null) || (currentLane.m_Flags & CreatureLaneFlags.Leader) == (CreatureLaneFlags) 0))
            {
              ++pathOwner.m_ElementIndex;
              // ISSUE: reference to a compiler-generated field
              if (this.m_CurveData.HasComponent(pathElement.m_Target))
              {
                for (int index = 0; index < queues.Length; ++index)
                {
                  if (queues[index].m_TargetEntity == currentLane.m_Lane)
                    queues.RemoveAt(index--);
                }
                if (((currentLane.m_Flags ^ creatureLaneFlags) & CreatureLaneFlags.Backward) != (CreatureLaneFlags) 0)
                  currentLane.m_LanePosition = -currentLane.m_LanePosition;
                currentLane.m_Lane = pathElement.m_Target;
                currentLane.m_CurvePosition = pathElement.m_TargetDelta;
                currentLane.m_Flags = creatureLaneFlags;
              }
              else
                goto label_105;
            }
            else
              goto label_115;
          }
          currentLane.m_Flags |= CreatureLaneFlags.WaitSignal;
          float lanePosition = math.select(currentLane.m_LanePosition, -currentLane.m_LanePosition, ((currentLane.m_Flags ^ creatureLaneFlags) & CreatureLaneFlags.Backward) > (CreatureLaneFlags) 0);
          // ISSUE: reference to a compiler-generated method
          Line3.Segment targetPos = this.CalculateTargetPos(prefabObjectGeometryData, pathElement.m_Target, pathElement.m_TargetDelta, lanePosition);
          navigation.m_TargetPosition = targetPos.a;
          navigation.m_TargetDirection = math.normalizesafe(targetPos.b.xz - targetPos.a.xz);
          navigation.m_TargetActivity = (byte) 0;
          targetIterator.m_Blocker = componentData1.m_Blocker;
          targetIterator.m_BlockerType = BlockerType.Signal;
          targetIterator.m_QueueEntity = pathElement.m_Target;
          targetIterator.m_QueueArea = CreatureUtils.GetQueueArea(prefabObjectGeometryData, targetPos.a, targetPos.b);
          goto label_115;
label_105:
          pathElements.Clear();
          pathOwner.m_ElementIndex = 0;
          if (groupMember.m_Leader == Entity.Null)
            pathOwner.m_State |= PathFlags.Obsolete;
label_115:
          blocker.m_Blocker = targetIterator.m_Blocker;
          blocker.m_Type = targetIterator.m_BlockerType;
          currentLane.m_QueueEntity = targetIterator.m_QueueEntity;
          currentLane.m_QueueArea = targetIterator.m_QueueArea;
        }
        if (groupMember.m_Leader != Entity.Null)
        {
          Game.Objects.Transform componentData;
          // ISSUE: reference to a compiler-generated field
          if ((currentLane.m_Flags & CreatureLaneFlags.Leader) != (CreatureLaneFlags) 0 && this.m_TransformData.TryGetComponent(groupMember.m_Leader, out componentData))
          {
            Line3.Segment line = new Line3.Segment(transform.m_Position, navigation.m_TargetPosition);
            float t;
            double num6 = (double) MathUtils.Distance(line, componentData.m_Position, out t);
            float distance = MathUtils.Length(line) * t;
            float y5 = MathUtils.Clamp(CreatureUtils.GetMaxBrakingSpeed(prefabHumanData, distance, timeStep), bounds1);
            if ((double) y5 < (double) navigation.m_MaxSpeed)
            {
              navigation.m_MaxSpeed = math.min(navigation.m_MaxSpeed, y5);
              blocker.m_Blocker = groupMember.m_Leader;
            }
          }
          if (blocker.m_Blocker == groupMember.m_Leader && (double) currentLane.m_QueueArea.radius <= 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            Creature creature = this.m_CreatureData[groupMember.m_Leader];
            if ((double) creature.m_QueueArea.radius > 0.0)
            {
              Sphere3 queueArea = CreatureUtils.GetQueueArea(prefabObjectGeometryData, transform.m_Position, navigation.m_TargetPosition);
              currentLane.m_QueueEntity = creature.m_QueueEntity;
              currentLane.m_QueueArea = MathUtils.Sphere(creature.m_QueueArea, queueArea);
            }
          }
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
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          CreatureCollisionIterator collisionIterator = new CreatureCollisionIterator()
          {
            m_OwnerData = this.m_OwnerData,
            m_TransformData = this.m_TransformData,
            m_MovingData = this.m_MovingData,
            m_CreatureData = this.m_CreatureData,
            m_GroupMemberData = this.m_GroupMemberData,
            m_WaypointData = this.m_WaypointData,
            m_TaxiStandData = this.m_TaxiStandData,
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
            m_CurrentLane = currentLane.m_Lane,
            m_CurrentVehicle = currentVehicle.m_Vehicle,
            m_CurvePosition = currentLane.m_CurvePosition.y,
            m_TimeStep = timeStep,
            m_PrefabObjectGeometry = prefabObjectGeometryData,
            m_SpeedRange = bounds1,
            m_CurrentPosition = transform.m_Position,
            m_CurrentDirection = math.forward(transform.m_Rotation),
            m_CurrentVelocity = moving.m_Velocity,
            m_TargetDistance = minDistance,
            m_PathOwner = pathOwner,
            m_PathElements = pathElements,
            m_MinSpeed = random.NextFloat(0.4f, 0.6f),
            m_TargetPosition = navigation.m_TargetPosition,
            m_MaxSpeed = navigation.m_MaxSpeed,
            m_LanePosition = currentLane.m_LanePosition,
            m_Blocker = blocker.m_Blocker,
            m_BlockerType = blocker.m_Type,
            m_QueueEntity = currentLane.m_QueueEntity,
            m_QueueArea = currentLane.m_QueueArea,
            m_Queues = queues
          };
          if (blocker1 != Entity.Null)
          {
            collisionIterator.IterateBlocker(prefabHumanData, blocker1);
            collisionIterator.m_MaxSpeed = math.select(collisionIterator.m_MaxSpeed, 0.0f, (double) collisionIterator.m_MaxSpeed < 0.10000000149011612);
          }
          if ((double) collisionIterator.m_MaxSpeed != 0.0)
          {
            if ((currentLane.m_Flags & CreatureLaneFlags.Connection) == (CreatureLaneFlags) 0)
            {
              bool isBackward = (currentLane.m_Flags & CreatureLaneFlags.Backward) > (CreatureLaneFlags) 0;
              if ((currentLane.m_Flags & CreatureLaneFlags.WaitSignal) != (CreatureLaneFlags) 0)
              {
                int elementIndex = pathOwner.m_ElementIndex;
                if (elementIndex < pathElements.Length)
                {
                  ref DynamicBuffer<PathElement> local = ref pathElements;
                  int index = elementIndex;
                  int num7 = index + 1;
                  PathElement pathElement = local[index];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_CurveData.HasComponent(pathElement.m_Target) && collisionIterator.IterateFirstLane(currentLane.m_Lane, pathElement.m_Target, currentLane.m_CurvePosition, pathElement.m_TargetDelta, isBackward))
                  {
                    while (collisionIterator.IterateNextLane(pathElement.m_Target, pathElement.m_TargetDelta) && num7 < pathElements.Length)
                      pathElement = pathElements[num7++];
                  }
                }
              }
              else if (collisionIterator.IterateFirstLane(currentLane.m_Lane, currentLane.m_CurvePosition, isBackward))
              {
                int elementIndex = pathOwner.m_ElementIndex;
                if (elementIndex < pathElements.Length)
                {
                  ref DynamicBuffer<PathElement> local = ref pathElements;
                  int index = elementIndex;
                  int num8 = index + 1;
                  PathElement pathElement = local[index];
                  while (collisionIterator.IterateNextLane(pathElement.m_Target, pathElement.m_TargetDelta) && num8 < pathElements.Length)
                    pathElement = pathElements[num8++];
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
          maxSpeed = collisionIterator.m_MaxSpeed;
        }
        blocker.m_MaxSpeed = (byte) math.clamp(Mathf.RoundToInt(maxSpeed * 45.8999977f), 0, (int) byte.MaxValue);
        if ((human.m_Flags & (HumanFlags.Waiting | HumanFlags.Sad | HumanFlags.Happy | HumanFlags.Angry)) == (HumanFlags) 0 || (double) navigation.m_MaxSpeed >= 0.10000000149011612 || navigation.m_TargetActivity != (byte) 0 || random.NextInt(100) != 0)
          return;
        navigation.m_TargetActivity = (byte) 21;
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

      private void SetActionTarget(
        ref HumanNavigation navigation,
        Game.Objects.Transform transform,
        Human human,
        PathElement pathElement)
      {
        bool flag = false;
        if ((human.m_Flags & HumanFlags.Selfies) != (HumanFlags) 0)
        {
          navigation.m_TargetActivity = (byte) 7;
          flag = true;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TransformData.HasComponent(pathElement.m_Target))
          return;
        // ISSUE: reference to a compiler-generated field
        Game.Objects.Transform transform1 = this.m_TransformData[pathElement.m_Target];
        if (flag)
          navigation.m_TargetDirection = math.normalizesafe(transform.m_Position.xz - transform1.m_Position.xz);
        else
          navigation.m_TargetDirection = math.normalizesafe(transform1.m_Position.xz - transform.m_Position.xz);
      }

      private bool MoveAreaTarget(
        ref Unity.Mathematics.Random random,
        float3 comparePosition,
        PathOwner pathOwner,
        DynamicBuffer<PathElement> pathElements,
        ref float3 targetPosition,
        ref float2 targetDirection,
        ref byte activity,
        float minDistance,
        Entity target,
        ActivityMask activityMask,
        ref float2 curveDelta,
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
        targetDirection = new float2();
        activity = (byte) 0;
        if (areaLane1.m_Nodes.y == areaLane1.m_Nodes.z)
        {
          float3 position1 = areaNode[areaLane1.m_Nodes.x].m_Position;
          float3 position2 = areaNode[areaLane1.m_Nodes.y].m_Position;
          float3 position3 = areaNode[areaLane1.m_Nodes.w].m_Position;
          float3 right = position2;
          float3 next = position3;
          float3 comparePosition1 = comparePosition;
          PathElement nextElement = new PathElement();
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
          if (CreatureUtils.SetTriangleTarget(position1, right, next, comparePosition1, nextElement, elementIndex, pathElements1, ref local, (float) minDistance1, (float) lanePosition1, (float) y, (float) navigationSize1, true, transformData, taxiStandData, areaLaneData, curveData))
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
            if (CreatureUtils.SetAreaTarget(position4, position4, position5, position6, position7, owner, areaNode, comparePosition, new PathElement(), pathOwner.m_ElementIndex, pathElements, ref targetPosition, minDistance, lanePosition, curveDelta.y, navigationSize, isBackward, this.m_TransformData, this.m_TaxiStandData, this.m_AreaLaneData, this.m_CurveData, this.m_OwnerData))
              return true;
            curveDelta.x = 0.5f;
            bool4_1.xz = (bool2) false;
          }
          if (pathElements.Length > pathOwner.m_ElementIndex)
          {
            PathElement pathElement = pathElements[pathOwner.m_ElementIndex];
            Owner componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_OwnerData.TryGetComponent(pathElement.m_Target, out componentData) && componentData.m_Owner == owner)
            {
              bool4 bool4_2 = new bool4(pathElement.m_TargetDelta < 0.5f, pathElement.m_TargetDelta > 0.5f);
              if (math.any(!bool4_1.xz) & math.any(bool4_1.yw) & math.any(bool4_2.xy & bool4_2.wz))
              {
                // ISSUE: reference to a compiler-generated field
                AreaLane areaLane2 = this.m_AreaLaneData[pathElement.m_Target];
                bool flag = (double) pathElement.m_TargetDelta.y < (double) pathElement.m_TargetDelta.x;
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
                PathElement nextElement = new PathElement();
                int elementIndex = pathOwner.m_ElementIndex + 1;
                DynamicBuffer<PathElement> pathElements2 = pathElements;
                ref float3 local = ref targetPosition;
                double minDistance2 = (double) minDistance;
                double lanePosition2 = (double) lanePosition;
                double y = (double) pathElement.m_TargetDelta.y;
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
                if (CreatureUtils.SetAreaTarget(prev2, prev, left, right, next, areaEntity, nodes, comparePosition2, nextElement, elementIndex, pathElements2, ref local, (float) minDistance2, (float) lanePosition2, (float) y, (float) navigationSize2, num != 0, transformData, taxiStandData, areaLaneData, curveData, ownerData))
                  return true;
              }
              curveDelta.x = curveDelta.y;
              return false;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (CreatureUtils.SetTriangleTarget(position5, position6, position7, comparePosition, new PathElement(), pathOwner.m_ElementIndex, pathElements, ref targetPosition, minDistance, lanePosition, curveDelta.y, navigationSize, false, this.m_TransformData, this.m_TaxiStandData, this.m_AreaLaneData, this.m_CurveData))
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
        Human human,
        CurrentVehicle currentVehicle,
        float3 comparePosition,
        ref float3 targetPosition,
        ref float2 targetDirection,
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
        ActivityCondition conditions = CreatureUtils.GetConditions(human);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!CreatureUtils.CalculateTransformPosition(creature, creaturePrefab, meshGroups, ref random, ref result, ref activity1, currentVehicle, target, this.m_LeftHandTraffic, activityMask, conditions, this.m_MovingObjectSearchTree, ref this.m_TransformData, ref this.m_PositionData, ref this.m_PublicTransportData, ref this.m_TrainData, ref this.m_ControllerData, ref this.m_PrefabRefData, ref this.m_PrefabBuildingData, ref this.m_PrefabCarData, ref this.m_PrefabActivityLocations, ref this.m_SubMeshGroups, ref this.m_CharacterElements, ref this.m_SubMeshes, ref this.m_AnimationClips, ref this.m_AnimationMotions))
          return false;
        targetPosition = result.m_Position;
        if (result.m_Rotation.Equals(new quaternion()))
          targetDirection = new float2();
        else
          targetDirection = math.normalizesafe(math.forward(result.m_Rotation).xz);
        activity = (byte) activity1;
        return (double) math.distance(comparePosition, targetPosition) >= (double) minDistance;
      }

      private bool GetTransformTarget(ref Entity entity, Entity target)
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

    [BurstCompile]
    private struct UpdateLaneSignalsJob : IJob
    {
      public NativeQueue<HumanNavigationHelpers.LaneSignal> m_LaneSignalQueue;
      public ComponentLookup<Game.Net.LaneSignal> m_LaneSignalData;

      public void Execute()
      {
        HumanNavigationHelpers.LaneSignal laneSignal1;
        // ISSUE: reference to a compiler-generated field
        while (this.m_LaneSignalQueue.TryDequeue(out laneSignal1))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.LaneSignal laneSignal2 = this.m_LaneSignalData[laneSignal1.m_Lane];
          if ((int) laneSignal1.m_Priority > (int) laneSignal2.m_Priority)
          {
            laneSignal2.m_Petitioner = laneSignal1.m_Petitioner;
            laneSignal2.m_Priority = laneSignal1.m_Priority;
            // ISSUE: reference to a compiler-generated field
            this.m_LaneSignalData[laneSignal1.m_Lane] = laneSignal2;
          }
        }
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
      public ComponentTypeHandle<Human> __Game_Creatures_Human_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InvolvedInAccident> __Game_Events_InvolvedInAccident_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<MeshGroup> __Game_Rendering_MeshGroup_RO_BufferTypeHandle;
      public ComponentTypeHandle<HumanNavigation> __Game_Creatures_HumanNavigation_RW_ComponentTypeHandle;
      public ComponentTypeHandle<HumanCurrentLane> __Game_Creatures_HumanCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Blocker> __Game_Vehicles_Blocker_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public BufferTypeHandle<Queue> __Game_Creatures_Queue_RW_BufferTypeHandle;
      public BufferTypeHandle<PathElement> __Game_Pathfind_PathElement_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneSignal> __Game_Net_LaneSignal_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneReservation> __Game_Net_LaneReservation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaLane> __Game_Net_AreaLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Waypoint> __Game_Routes_Waypoint_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TaxiStand> __Game_Routes_TaxiStand_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Position> __Game_Routes_Position_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Connected> __Game_Routes_Connected_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TakeoffLocation> __Game_Routes_TakeoffLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Moving> __Game_Objects_Moving_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.ActivityLocation> __Game_Objects_ActivityLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Creature> __Game_Creatures_Creature_RO_ComponentLookup;
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
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CreatureData> __Game_Prefabs_CreatureData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HumanData> __Game_Prefabs_HumanData_RO_ComponentLookup;
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
        this.__Game_Creatures_Human_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Human>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InvolvedInAccident_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InvolvedInAccident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshGroup_RO_BufferTypeHandle = state.GetBufferTypeHandle<MeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanNavigation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<HumanNavigation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<HumanCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Blocker_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Blocker>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Queue_RW_BufferTypeHandle = state.GetBufferTypeHandle<Queue>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneSignal_RO_ComponentLookup = state.GetComponentLookup<Game.Net.LaneSignal>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneReservation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.LaneReservation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_AreaLane_RO_ComponentLookup = state.GetComponentLookup<AreaLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Waypoint_RO_ComponentLookup = state.GetComponentLookup<Waypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TaxiStand_RO_ComponentLookup = state.GetComponentLookup<TaxiStand>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentLookup = state.GetComponentLookup<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RO_ComponentLookup = state.GetComponentLookup<Connected>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TakeoffLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.TakeoffLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentLookup = state.GetComponentLookup<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_ActivityLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.ActivityLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Creature_RO_ComponentLookup = state.GetComponentLookup<Creature>(true);
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
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CreatureData_RO_ComponentLookup = state.GetComponentLookup<CreatureData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HumanData_RO_ComponentLookup = state.GetComponentLookup<HumanData>(true);
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
      }
    }
  }
}
