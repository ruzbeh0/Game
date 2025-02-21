// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.LineVisualizerSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Rendering;
using Game.Routes;
using Game.Simulation;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class LineVisualizerSection : InfoSectionBase
  {
    private RenderingSystem m_RenderingSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private NativeArray<bool> m_BoolResult;
    private NativeArray<Entity> m_EntityResult;
    private NativeList<LineVisualizerSection.LineSegment> m_SegmentsResult;
    private NativeList<LineVisualizerSection.LineStop> m_StopsResult;
    private NativeList<LineVisualizerSection.LineVehicle> m_VehiclesResult;
    private NativeArray<Color32> m_ColorResult;
    private NativeArray<int> m_StopCapacityResult;
    private LineVisualizerSection.TypeHandle __TypeHandle;

    protected override string group => nameof (LineVisualizerSection);

    protected override bool displayForDestroyedObjects => true;

    protected override bool displayForOutsideConnections => true;

    private UnityEngine.Color color { get; set; }

    private int stopCapacity { get; set; }

    private NativeList<LineVisualizerSection.LineStop> stops { get; set; }

    private NativeList<LineVisualizerSection.LineVehicle> vehicles { get; set; }

    private NativeList<LineVisualizerSection.LineSegment> segments { get; set; }

    protected override void Reset()
    {
      this.color = new UnityEngine.Color();
      this.stopCapacity = 0;
      this.stops.Clear();
      this.vehicles.Clear();
      this.segments.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_SegmentsResult.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_StopsResult.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_VehiclesResult.Clear();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      this.stops = new NativeList<LineVisualizerSection.LineStop>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.vehicles = new NativeList<LineVisualizerSection.LineVehicle>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.segments = new NativeList<LineVisualizerSection.LineSegment>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BoolResult = new NativeArray<bool>(3, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_EntityResult = new NativeArray<Entity>(1, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ColorResult = new NativeArray<Color32>(1, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_StopCapacityResult = new NativeArray<int>(1, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_SegmentsResult = new NativeList<LineVisualizerSection.LineSegment>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_StopsResult = new NativeList<LineVisualizerSection.LineStop>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_VehiclesResult = new NativeList<LineVisualizerSection.LineVehicle>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      this.stops.Dispose();
      this.vehicles.Dispose();
      this.segments.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_BoolResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_EntityResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ColorResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_StopCapacityResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_SegmentsResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_StopsResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_VehiclesResult.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_ConnectedRoute_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TransportLine_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Route_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      new LineVisualizerSection.VisibilityJob()
      {
        m_SelectedEntity = this.selectedEntity,
        m_SelectedRouteEntity = this.m_InfoUISystem.selectedRoute,
        m_Routes = this.__TypeHandle.__Game_Routes_Route_RO_ComponentLookup,
        m_TransportLines = this.__TypeHandle.__Game_Routes_TransportLine_RO_ComponentLookup,
        m_TransportStops = this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentLookup,
        m_TaxiStands = this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentLookup,
        m_Vehicles = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup,
        m_Owners = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PublicTransports = this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup,
        m_CurrentRoutes = this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentLookup,
        m_RouteWaypointBuffers = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
        m_RouteSegmentBuffers = this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferLookup,
        m_RouteVehicleBuffers = this.__TypeHandle.__Game_Routes_RouteVehicle_RO_BufferLookup,
        m_ConnectedRouteBuffers = this.__TypeHandle.__Game_Routes_ConnectedRoute_RO_BufferLookup,
        m_SubObjectBuffers = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_InstalledUpgradeBuffers = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_BoolResult = this.m_BoolResult,
        m_EntityResult = this.m_EntityResult
      }.Schedule<LineVisualizerSection.VisibilityJob>(this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      this.visible = this.m_BoolResult[0];
      if (!this.visible)
        return;
      // ISSUE: reference to a compiler-generated field
      if (this.m_BoolResult[1])
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_InfoUISystem.selectedRoute = this.m_EntityResult[0];
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftNavigationLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftNavigationLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainNavigationLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Pet_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_WaitingPassengers_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Color_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      new LineVisualizerSection.UpdateJob()
      {
        m_RightHandTraffic = (!this.m_CityConfigurationSystem.leftHandTraffic),
        m_RouteEntity = this.m_InfoUISystem.selectedRoute,
        m_RenderingFrameIndex = this.m_RenderingSystem.frameIndex,
        m_RenderingFrameTime = this.m_RenderingSystem.frameTime,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_UpdateFrames = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_Colors = this.__TypeHandle.__Game_Routes_Color_RO_ComponentLookup,
        m_PathInformation = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_Connected = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup,
        m_WaitingPassengers = this.__TypeHandle.__Game_Routes_WaitingPassengers_RO_ComponentLookup,
        m_Positions = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
        m_RouteLanes = this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup,
        m_CurrentRoutes = this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentLookup,
        m_Targets = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
        m_PathOwners = this.__TypeHandle.__Game_Pathfind_PathOwner_RO_ComponentLookup,
        m_Owners = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_Waypoints = this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup,
        m_Trains = this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup,
        m_Curves = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_SlaveLanes = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_CarCurrentLanes = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup,
        m_TrainCurrentLanes = this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup,
        m_WatercraftCurrentLanes = this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentLookup,
        m_AircraftCurrentLanes = this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RO_ComponentLookup,
        m_Pets = this.__TypeHandle.__Game_Creatures_Pet_RO_ComponentLookup,
        m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_TransportLineData = this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup,
        m_TrainDatas = this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup,
        m_PublicTransportVehicleDatas = this.__TypeHandle.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup,
        m_CargoTransportVehicleDatas = this.__TypeHandle.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup,
        m_CullingInfos = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup,
        m_Transforms = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_TransportStops = this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentLookup,
        m_OutsideConnections = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_EconomyResourcesBuffers = this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup,
        m_RouteWaypointBuffers = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
        m_RouteSegmentBuffers = this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferLookup,
        m_RouteVehicleBuffers = this.__TypeHandle.__Game_Routes_RouteVehicle_RO_BufferLookup,
        m_LayoutElementBuffers = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_CarNavigationLaneBuffers = this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RO_BufferLookup,
        m_TrainNavigationLaneBuffers = this.__TypeHandle.__Game_Vehicles_TrainNavigationLane_RO_BufferLookup,
        m_WatercraftNavigationLaneBuffers = this.__TypeHandle.__Game_Vehicles_WatercraftNavigationLane_RO_BufferLookup,
        m_AircraftNavigationLaneBuffers = this.__TypeHandle.__Game_Vehicles_AircraftNavigationLane_RO_BufferLookup,
        m_PathElementBuffers = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup,
        m_SubLaneBuffers = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_PassengerBuffers = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferLookup,
        m_TransformFrames = this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferLookup,
        m_SegmentsResult = this.m_SegmentsResult,
        m_StopsResult = this.m_StopsResult,
        m_VehiclesResult = this.m_VehiclesResult,
        m_ColorResult = this.m_ColorResult,
        m_StopCapacityResult = this.m_StopCapacityResult,
        m_BoolResult = this.m_BoolResult
      }.Schedule<LineVisualizerSection.UpdateJob>(this.Dependency).Complete();
    }

    protected override void OnProcess()
    {
      // ISSUE: reference to a compiler-generated field
      this.color = (UnityEngine.Color) this.m_ColorResult[0];
      // ISSUE: reference to a compiler-generated field
      this.stopCapacity = this.m_StopCapacityResult[0];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_InfoUISystem.tooltipTags.Add(this.m_BoolResult[2] ? TooltipTags.CargoRoute : (this.EntityManager.HasComponent<Game.Routes.TransportStop>(this.selectedEntity) ? TooltipTags.TransportStop : TooltipTags.TransportLine));
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_SegmentsResult.Length; ++index)
      {
        NativeList<LineVisualizerSection.LineSegment> segments = this.segments;
        ref NativeList<LineVisualizerSection.LineSegment> local1 = ref segments;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        LineVisualizerSection.LineSegment lineSegment = this.m_SegmentsResult[index];
        ref LineVisualizerSection.LineSegment local2 = ref lineSegment;
        local1.Add(in local2);
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_VehiclesResult.Length; ++index)
      {
        NativeList<LineVisualizerSection.LineVehicle> vehicles = this.vehicles;
        ref NativeList<LineVisualizerSection.LineVehicle> local3 = ref vehicles;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        LineVisualizerSection.LineVehicle lineVehicle = this.m_VehiclesResult[index];
        ref LineVisualizerSection.LineVehicle local4 = ref lineVehicle;
        local3.Add(in local4);
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_StopsResult.Length; ++index)
      {
        NativeList<LineVisualizerSection.LineStop> stops = this.stops;
        ref NativeList<LineVisualizerSection.LineStop> local5 = ref stops;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        LineVisualizerSection.LineStop lineStop = this.m_StopsResult[index];
        ref LineVisualizerSection.LineStop local6 = ref lineStop;
        local5.Add(in local6);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Dirty = true;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("color");
      writer.Write(this.color);
      writer.PropertyName("stops");
      writer.ArrayBegin(this.stops.Length);
      for (int index = 0; index < this.stops.Length; ++index)
      {
        // ISSUE: variable of a compiler-generated type
        LineVisualizerSection.LineStop stop = this.stops[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        stop.Bind(writer, this.m_NameSystem);
      }
      writer.ArrayEnd();
      writer.PropertyName("vehicles");
      IJsonWriter writer1 = writer;
      NativeList<LineVisualizerSection.LineVehicle> vehicles = this.vehicles;
      int length1 = vehicles.Length;
      writer1.ArrayBegin(length1);
      int index1 = 0;
      while (true)
      {
        int num = index1;
        vehicles = this.vehicles;
        int length2 = vehicles.Length;
        if (num < length2)
        {
          vehicles = this.vehicles;
          // ISSUE: variable of a compiler-generated type
          LineVisualizerSection.LineVehicle lineVehicle = vehicles[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          lineVehicle.Bind(writer, this.m_NameSystem);
          ++index1;
        }
        else
          break;
      }
      writer.ArrayEnd();
      writer.PropertyName("segments");
      IJsonWriter writer2 = writer;
      NativeList<LineVisualizerSection.LineSegment> segments = this.segments;
      int length3 = segments.Length;
      writer2.ArrayBegin(length3);
      int index2 = 0;
      while (true)
      {
        int num = index2;
        segments = this.segments;
        int length4 = segments.Length;
        if (num < length4)
        {
          IJsonWriter writer3 = writer;
          segments = this.segments;
          // ISSUE: variable of a compiler-generated type
          LineVisualizerSection.LineSegment lineSegment = segments[index2];
          writer3.Write<LineVisualizerSection.LineSegment>(lineSegment);
          ++index2;
        }
        else
          break;
      }
      writer.ArrayEnd();
      writer.PropertyName("stopCapacity");
      writer.Write(this.stopCapacity);
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
    public LineVisualizerSection()
    {
    }

    private readonly struct LineStop
    {
      public Entity entity { get; }

      public float position { get; }

      public int cargo { get; }

      public bool isCargo { get; }

      public bool isOutsideConnection { get; }

      public LineStop(
        Entity entity,
        float position,
        int cargo,
        bool isCargo = false,
        bool isOutsideConnection = false)
      {
        this.entity = entity;
        this.position = position;
        this.cargo = cargo;
        this.isCargo = isCargo;
        this.isOutsideConnection = isOutsideConnection;
      }

      public void Bind(IJsonWriter binder, NameSystem nameSystem)
      {
        binder.TypeBegin(this.GetType().FullName);
        binder.PropertyName("entity");
        binder.Write(this.entity);
        binder.PropertyName("name");
        // ISSUE: reference to a compiler-generated method
        nameSystem.BindName(binder, this.entity);
        binder.PropertyName("position");
        binder.Write(this.position);
        binder.PropertyName("cargo");
        binder.Write(this.cargo);
        binder.PropertyName("isCargo");
        binder.Write(this.isCargo);
        binder.PropertyName("isOutsideConnection");
        binder.Write(this.isOutsideConnection);
        binder.TypeEnd();
      }
    }

    private readonly struct LineVehicle
    {
      public Entity entity { get; }

      public float position { get; }

      public int cargo { get; }

      public int capacity { get; }

      public bool isCargo { get; }

      public LineVehicle(Entity entity, float position, int cargo, int capacity, bool isCargo = false)
      {
        this.entity = entity;
        this.position = position;
        this.cargo = cargo;
        this.capacity = capacity;
        this.isCargo = isCargo;
      }

      public void Bind(IJsonWriter binder, NameSystem nameSystem)
      {
        binder.TypeBegin(this.GetType().FullName);
        binder.PropertyName("entity");
        binder.Write(this.entity);
        binder.PropertyName("name");
        // ISSUE: reference to a compiler-generated method
        nameSystem.BindName(binder, this.entity);
        binder.PropertyName("cargo");
        binder.Write(this.cargo);
        binder.PropertyName("capacity");
        binder.Write(this.capacity);
        binder.PropertyName("position");
        binder.Write(this.position);
        binder.PropertyName("isCargo");
        binder.Write(this.isCargo);
        binder.TypeEnd();
      }
    }

    private readonly struct LineSegment : IJsonWritable
    {
      public float start { get; }

      public float end { get; }

      public bool broken { get; }

      public LineSegment(float start, float end, bool broken)
      {
        this.start = start;
        this.end = end;
        this.broken = broken;
      }

      public void Write(IJsonWriter binder)
      {
        binder.TypeBegin(this.GetType().FullName);
        binder.PropertyName("start");
        binder.Write(this.start);
        binder.PropertyName("end");
        binder.Write(this.end);
        binder.PropertyName("broken");
        binder.Write(this.broken);
        binder.TypeEnd();
      }
    }

    private enum Result
    {
      Color = 0,
      Entity = 0,
      IsVisible = 0,
      StopsCapacity = 0,
      ShouldUpdateSelected = 1,
      IsCargo = 2,
    }

    [BurstCompile]
    private struct VisibilityJob : IJob
    {
      [ReadOnly]
      public Entity m_SelectedEntity;
      [ReadOnly]
      public Entity m_SelectedRouteEntity;
      [ReadOnly]
      public ComponentLookup<Route> m_Routes;
      [ReadOnly]
      public ComponentLookup<TransportLine> m_TransportLines;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TransportStop> m_TransportStops;
      [ReadOnly]
      public ComponentLookup<TaxiStand> m_TaxiStands;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_Vehicles;
      [ReadOnly]
      public ComponentLookup<Owner> m_Owners;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransports;
      [ReadOnly]
      public ComponentLookup<CurrentRoute> m_CurrentRoutes;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_RouteWaypointBuffers;
      [ReadOnly]
      public BufferLookup<RouteSegment> m_RouteSegmentBuffers;
      [ReadOnly]
      public BufferLookup<RouteVehicle> m_RouteVehicleBuffers;
      [ReadOnly]
      public BufferLookup<ConnectedRoute> m_ConnectedRouteBuffers;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjectBuffers;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgradeBuffers;
      public NativeArray<bool> m_BoolResult;
      public NativeArray<Entity> m_EntityResult;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.IsLine(this.m_SelectedEntity))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_BoolResult[0] = true;
          // ISSUE: reference to a compiler-generated field
          this.m_BoolResult[1] = true;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_EntityResult[0] = this.m_SelectedEntity;
        }
        else
        {
          NativeList<ConnectedRoute> connectedRoutes = new NativeList<ConnectedRoute>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.TryGetStationRoutes(this.m_SelectedEntity, connectedRoutes) | this.TryGetStopRoutes(this.m_SelectedEntity, connectedRoutes))
          {
            bool flag = false;
            Entity owner = Entity.Null;
            for (int index = connectedRoutes.Length - 1; index >= 0; --index)
            {
              Owner componentData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (this.m_Owners.TryGetComponent(connectedRoutes[index].m_Waypoint, out componentData) && this.IsLine(componentData.m_Owner))
              {
                owner = componentData.m_Owner;
                // ISSUE: reference to a compiler-generated field
                if (owner == this.m_SelectedRouteEntity)
                  flag = true;
              }
            }
            if (!flag)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_BoolResult[0] = true;
              // ISSUE: reference to a compiler-generated field
              this.m_BoolResult[1] = true;
              // ISSUE: reference to a compiler-generated field
              this.m_EntityResult[0] = owner;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_BoolResult[0] = true;
              // ISSUE: reference to a compiler-generated field
              this.m_BoolResult[1] = false;
              // ISSUE: reference to a compiler-generated field
              this.m_EntityResult[0] = Entity.Null;
            }
          }
          else
          {
            Entity routeEntity;
            // ISSUE: reference to a compiler-generated method
            if (this.IsVehicle(out routeEntity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_BoolResult[0] = true;
              // ISSUE: reference to a compiler-generated field
              this.m_BoolResult[1] = true;
              // ISSUE: reference to a compiler-generated field
              this.m_EntityResult[0] = routeEntity;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_BoolResult[0] = false;
              // ISSUE: reference to a compiler-generated field
              this.m_BoolResult[1] = false;
              // ISSUE: reference to a compiler-generated field
              this.m_EntityResult[0] = Entity.Null;
            }
          }
        }
      }

      private bool IsLine(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Routes.HasComponent(entity) && this.m_TransportLines.HasComponent(entity) && this.m_RouteWaypointBuffers.HasBuffer(entity) && this.m_RouteSegmentBuffers.HasBuffer(entity) && this.m_RouteVehicleBuffers.HasBuffer(entity);
      }

      private bool TryGetStationRoutes(Entity entity, NativeList<ConnectedRoute> connectedRoutes)
      {
        DynamicBuffer<Game.Objects.SubObject> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubObjectBuffers.TryGetBuffer(entity, out bufferData1))
        {
          for (int index = 0; index < bufferData1.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            this.TryGetStopRoutes(bufferData1[index].m_SubObject, connectedRoutes);
          }
        }
        DynamicBuffer<InstalledUpgrade> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_InstalledUpgradeBuffers.TryGetBuffer(entity, out bufferData2))
        {
          foreach (InstalledUpgrade installedUpgrade in bufferData2)
          {
            // ISSUE: reference to a compiler-generated method
            this.TryGetStationRoutes(installedUpgrade.m_Upgrade, connectedRoutes);
          }
        }
        return connectedRoutes.Length > 0;
      }

      private bool TryGetStopRoutes(Entity entity, NativeList<ConnectedRoute> connectedRoutes)
      {
        DynamicBuffer<ConnectedRoute> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ConnectedRouteBuffers.TryGetBuffer(entity, out bufferData) || !this.m_TransportStops.HasComponent(entity) || this.m_TaxiStands.HasComponent(entity) || bufferData.Length <= 0)
          return false;
        connectedRoutes.AddRange(bufferData.AsNativeArray());
        return true;
      }

      private bool IsVehicle(out Entity routeEntity)
      {
        CurrentRoute componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_Vehicles.HasComponent(this.m_SelectedEntity) && this.m_Owners.HasComponent(this.m_SelectedEntity) && this.m_PublicTransports.HasComponent(this.m_SelectedEntity) && this.m_CurrentRoutes.TryGetComponent(this.m_SelectedEntity, out componentData) && this.IsLine(componentData.m_Route))
        {
          routeEntity = componentData.m_Route;
          return true;
        }
        routeEntity = Entity.Null;
        return false;
      }
    }

    [BurstCompile]
    private struct UpdateJob : IJob
    {
      [ReadOnly]
      public bool m_RightHandTraffic;
      [ReadOnly]
      public Entity m_RouteEntity;
      [ReadOnly]
      public uint m_RenderingFrameIndex;
      [ReadOnly]
      public float m_RenderingFrameTime;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrames;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Color> m_Colors;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformation;
      [ReadOnly]
      public ComponentLookup<Connected> m_Connected;
      [ReadOnly]
      public ComponentLookup<WaitingPassengers> m_WaitingPassengers;
      [ReadOnly]
      public ComponentLookup<Position> m_Positions;
      [ReadOnly]
      public ComponentLookup<RouteLane> m_RouteLanes;
      [ReadOnly]
      public ComponentLookup<CurrentRoute> m_CurrentRoutes;
      [ReadOnly]
      public ComponentLookup<Game.Common.Target> m_Targets;
      [ReadOnly]
      public ComponentLookup<PathOwner> m_PathOwners;
      [ReadOnly]
      public ComponentLookup<Owner> m_Owners;
      [ReadOnly]
      public ComponentLookup<Waypoint> m_Waypoints;
      [ReadOnly]
      public ComponentLookup<Train> m_Trains;
      [ReadOnly]
      public ComponentLookup<Curve> m_Curves;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLanes;
      [ReadOnly]
      public ComponentLookup<CarCurrentLane> m_CarCurrentLanes;
      [ReadOnly]
      public ComponentLookup<TrainCurrentLane> m_TrainCurrentLanes;
      [ReadOnly]
      public ComponentLookup<WatercraftCurrentLane> m_WatercraftCurrentLanes;
      [ReadOnly]
      public ComponentLookup<AircraftCurrentLane> m_AircraftCurrentLanes;
      [ReadOnly]
      public ComponentLookup<Game.Creatures.Pet> m_Pets;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      [ReadOnly]
      public ComponentLookup<TransportLineData> m_TransportLineData;
      [ReadOnly]
      public ComponentLookup<TrainData> m_TrainDatas;
      [ReadOnly]
      public ComponentLookup<PublicTransportVehicleData> m_PublicTransportVehicleDatas;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> m_CargoTransportVehicleDatas;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TransportStop> m_TransportStops;
      [ReadOnly]
      public ComponentLookup<CullingInfo> m_CullingInfos;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_Transforms;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> m_EconomyResourcesBuffers;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_RouteWaypointBuffers;
      [ReadOnly]
      public BufferLookup<RouteSegment> m_RouteSegmentBuffers;
      [ReadOnly]
      public BufferLookup<RouteVehicle> m_RouteVehicleBuffers;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElementBuffers;
      [ReadOnly]
      public BufferLookup<CarNavigationLane> m_CarNavigationLaneBuffers;
      [ReadOnly]
      public BufferLookup<TrainNavigationLane> m_TrainNavigationLaneBuffers;
      [ReadOnly]
      public BufferLookup<WatercraftNavigationLane> m_WatercraftNavigationLaneBuffers;
      [ReadOnly]
      public BufferLookup<AircraftNavigationLane> m_AircraftNavigationLaneBuffers;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElementBuffers;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLaneBuffers;
      [ReadOnly]
      public BufferLookup<Passenger> m_PassengerBuffers;
      [ReadOnly]
      public BufferLookup<TransformFrame> m_TransformFrames;
      public NativeList<LineVisualizerSection.LineSegment> m_SegmentsResult;
      public NativeList<LineVisualizerSection.LineStop> m_StopsResult;
      public NativeList<LineVisualizerSection.LineVehicle> m_VehiclesResult;
      public NativeArray<Color32> m_ColorResult;
      public NativeArray<int> m_StopCapacityResult;
      public NativeArray<bool> m_BoolResult;

      public void Execute()
      {
        NativeList<float> nativeList = new NativeList<float>((AllocatorManager.AllocatorHandle) Allocator.Temp);
        float num1 = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_BoolResult[2] = false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ColorResult[0] = this.m_Colors[this.m_RouteEntity].m_Color;
        // ISSUE: reference to a compiler-generated field
        this.m_StopCapacityResult[0] = 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteWaypoint> routeWaypointBuffer = this.m_RouteWaypointBuffers[this.m_RouteEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteSegment> routeSegmentBuffer = this.m_RouteSegmentBuffers[this.m_RouteEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteVehicle> routeVehicleBuffer = this.m_RouteVehicleBuffers[this.m_RouteEntity];
        for (int segmentIndex = 0; segmentIndex < routeSegmentBuffer.Length; ++segmentIndex)
        {
          nativeList.Add(in num1);
          // ISSUE: reference to a compiler-generated method
          num1 += this.GetSegmentLength(routeWaypointBuffer, routeSegmentBuffer, segmentIndex);
        }
        if ((double) num1 == 0.0)
          return;
        for (int index = 0; index < routeSegmentBuffer.Length; ++index)
        {
          PathInformation componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathInformation.TryGetComponent(routeSegmentBuffer[index].m_Segment, out componentData))
          {
            float start = nativeList[index] / num1;
            float end = index < routeSegmentBuffer.Length - 1 ? nativeList[index + 1] / num1 : 1f;
            bool broken = componentData.m_Origin == Entity.Null && componentData.m_Destination == Entity.Null;
            // ISSUE: reference to a compiler-generated field
            ref NativeList<LineVisualizerSection.LineSegment> local1 = ref this.m_SegmentsResult;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            LineVisualizerSection.LineSegment lineSegment = new LineVisualizerSection.LineSegment(start, end, broken);
            ref LineVisualizerSection.LineSegment local2 = ref lineSegment;
            local1.Add(in local2);
          }
        }
        PrefabRef componentData1;
        TransportLineData componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool isCargo = this.m_PrefabRefs.TryGetComponent(this.m_RouteEntity, out componentData1) && this.m_TransportLineData.TryGetComponent(componentData1.m_Prefab, out componentData2) && componentData2.m_CargoTransport;
        for (int index = 0; index < routeVehicleBuffer.Length; ++index)
        {
          Entity vehicle = routeVehicleBuffer[index].m_Vehicle;
          int prevWaypointIndex;
          float distanceFromWaypoint;
          float distanceToWaypoint;
          bool unknownPath;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.GetVehiclePosition(this.m_RouteEntity, vehicle, out prevWaypointIndex, out distanceFromWaypoint, out distanceToWaypoint, out unknownPath))
          {
            int num2 = prevWaypointIndex;
            // ISSUE: reference to a compiler-generated method
            float segmentLength = this.GetSegmentLength(routeWaypointBuffer, routeSegmentBuffer, num2);
            float num3 = nativeList[num2];
            float num4 = !unknownPath ? num3 + (segmentLength - distanceToWaypoint) : num3 + segmentLength * distanceFromWaypoint / math.max(1f, distanceFromWaypoint + distanceToWaypoint);
            // ISSUE: reference to a compiler-generated method
            (int cargo, int capacity) = this.GetCargo(vehicle);
            float num5 = math.frac(num4 / num1);
            // ISSUE: reference to a compiler-generated field
            ref NativeList<LineVisualizerSection.LineVehicle> local3 = ref this.m_VehiclesResult;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            LineVisualizerSection.LineVehicle lineVehicle = new LineVisualizerSection.LineVehicle(vehicle, this.m_RightHandTraffic ? 1f - num5 : num5, cargo, capacity, isCargo);
            ref LineVisualizerSection.LineVehicle local4 = ref lineVehicle;
            local3.Add(in local4);
            // ISSUE: reference to a compiler-generated field
            if (capacity > this.m_StopCapacityResult[0])
            {
              // ISSUE: reference to a compiler-generated field
              this.m_StopCapacityResult[0] = capacity;
            }
          }
        }
        for (int index1 = 0; index1 < routeWaypointBuffer.Length; ++index1)
        {
          Connected componentData3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Connected.TryGetComponent(routeWaypointBuffer[index1].m_Waypoint, out componentData3) && this.m_TransportStops.HasComponent(componentData3.m_Connected))
          {
            float num6 = nativeList[index1] / num1;
            int cargo = 0;
            Entity entity = componentData3.m_Connected;
            WaitingPassengers componentData4;
            // ISSUE: reference to a compiler-generated field
            if (!isCargo && this.m_WaitingPassengers.TryGetComponent(routeWaypointBuffer[index1].m_Waypoint, out componentData4))
            {
              cargo = componentData4.m_Count;
            }
            else
            {
              DynamicBuffer<Game.Economy.Resources> bufferData1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_EconomyResourcesBuffers.TryGetBuffer(componentData3.m_Connected, out bufferData1))
              {
                for (int index2 = 0; index2 < bufferData1.Length; ++index2)
                  cargo += bufferData1[index2].m_Amount;
              }
              else
              {
                Owner componentData5;
                DynamicBuffer<Game.Economy.Resources> bufferData2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Owners.TryGetComponent(componentData3.m_Connected, out componentData5) && this.m_EconomyResourcesBuffers.TryGetBuffer(componentData5.m_Owner, out bufferData2))
                {
                  for (int index3 = 0; index3 < bufferData2.Length; ++index3)
                    cargo += bufferData2[index3].m_Amount;
                  entity = componentData5.m_Owner;
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            ref NativeList<LineVisualizerSection.LineStop> local5 = ref this.m_StopsResult;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            LineVisualizerSection.LineStop lineStop = new LineVisualizerSection.LineStop(entity, this.m_RightHandTraffic ? 1f - num6 : num6, cargo, isCargo, this.m_OutsideConnections.HasComponent(entity));
            ref LineVisualizerSection.LineStop local6 = ref lineStop;
            local5.Add(in local6);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_BoolResult[2] = isCargo;
      }

      private float GetSegmentLength(
        DynamicBuffer<RouteWaypoint> waypoints,
        DynamicBuffer<RouteSegment> routeSegments,
        int segmentIndex)
      {
        int index1 = segmentIndex;
        int index2 = math.select(segmentIndex + 1, 0, index1 == waypoints.Length - 1);
        PathInformation componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PathInformation.TryGetComponent(routeSegments[segmentIndex].m_Segment, out componentData1) && componentData1.m_Destination != Entity.Null)
        {
          float distance = componentData1.m_Distance;
          RouteLane componentData2;
          Curve componentData3;
          Curve componentData4;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_RouteLanes.TryGetComponent(waypoints[index2].m_Waypoint, out componentData2) && this.m_Curves.TryGetComponent(componentData2.m_StartLane, out componentData3) && this.m_Curves.TryGetComponent(componentData2.m_EndLane, out componentData4))
            distance += math.distance(MathUtils.Position(componentData3.m_Bezier, componentData2.m_StartCurvePos), MathUtils.Position(componentData4.m_Bezier, componentData2.m_EndCurvePos));
          return distance;
        }
        float3 position1;
        float3 position2;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        return this.GetWaypointPosition(waypoints[index1].m_Waypoint, out position1) && this.GetWaypointPosition(waypoints[index2].m_Waypoint, out position2) ? math.max(0.0f, math.distance(position1, position2)) : 0.0f;
      }

      private bool GetWaypointPosition(Entity waypoint, out float3 position)
      {
        Position componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Positions.TryGetComponent(waypoint, out componentData1))
        {
          RouteLane componentData2;
          Curve componentData3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_RouteLanes.TryGetComponent(waypoint, out componentData2) && this.m_Curves.TryGetComponent(componentData2.m_EndLane, out componentData3))
          {
            position = MathUtils.Position(componentData3.m_Bezier, componentData2.m_EndCurvePos);
            return true;
          }
          position = componentData1.m_Position;
          return true;
        }
        position = new float3();
        return false;
      }

      private bool GetVehiclePosition(
        Entity transportRoute,
        Entity transportVehicle,
        out int prevWaypointIndex,
        out float distanceFromWaypoint,
        out float distanceToWaypoint,
        out bool unknownPath)
      {
        prevWaypointIndex = 0;
        distanceFromWaypoint = 0.0f;
        distanceToWaypoint = 0.0f;
        unknownPath = true;
        CurrentRoute componentData1;
        Game.Common.Target componentData2;
        PathOwner componentData3;
        Waypoint componentData4;
        float3 position1;
        DynamicBuffer<RouteWaypoint> bufferData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CurrentRoutes.TryGetComponent(transportVehicle, out componentData1) || !this.m_Targets.TryGetComponent(transportVehicle, out componentData2) || !this.m_PathOwners.TryGetComponent(transportVehicle, out componentData3) || !this.m_Waypoints.TryGetComponent(componentData2.m_Target, out componentData4) || !this.GetWaypointPosition(componentData2.m_Target, out position1) || !this.m_RouteWaypointBuffers.TryGetBuffer(transportRoute, out bufferData1) || componentData1.m_Route != transportRoute)
          return false;
        Entity entity = transportVehicle;
        DynamicBuffer<LayoutElement> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_LayoutElementBuffers.TryGetBuffer(transportVehicle, out bufferData2) && bufferData2.Length != 0)
        {
          for (int index = 0; index < bufferData2.Length; ++index)
          {
            PrefabRef componentData5;
            TrainData componentData6;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefs.TryGetComponent(bufferData2[index].m_Vehicle, out componentData5) && this.m_TrainDatas.TryGetComponent(componentData5.m_Prefab, out componentData6))
            {
              float num = math.csum(componentData6.m_AttachOffsets);
              distanceFromWaypoint -= num * 0.5f;
              distanceToWaypoint -= num * 0.5f;
            }
          }
          entity = bufferData2[0].m_Vehicle;
        }
        else
        {
          PrefabRef componentData7;
          TrainData componentData8;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefs.TryGetComponent(transportVehicle, out componentData7) && this.m_TrainDatas.TryGetComponent(componentData7.m_Prefab, out componentData8))
          {
            float num = math.csum(componentData8.m_AttachOffsets);
            distanceFromWaypoint -= num * 0.5f;
            distanceToWaypoint -= num * 0.5f;
          }
        }
        Train componentData9;
        PrefabRef componentData10;
        TrainData componentData11;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Trains.TryGetComponent(entity, out componentData9) && this.m_PrefabRefs.TryGetComponent(entity, out componentData10) && this.m_TrainDatas.TryGetComponent(componentData10.m_Prefab, out componentData11))
        {
          if ((componentData9.m_Flags & Game.Vehicles.TrainFlags.Reversed) != (Game.Vehicles.TrainFlags) 0)
            distanceToWaypoint -= componentData11.m_AttachOffsets.y;
          else
            distanceToWaypoint -= componentData11.m_AttachOffsets.x;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EntityLookup.Exists(entity))
          return false;
        // ISSUE: reference to a compiler-generated field
        ArchetypeChunk chunk = this.m_EntityLookup[entity].Chunk;
        DynamicBuffer<TransformFrame> bufferData3;
        float3 position2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformFrames.TryGetBuffer(entity, out bufferData3) && chunk.Has<UpdateFrame>(this.m_UpdateFrames))
        {
          uint updateFrame1;
          uint updateFrame2;
          float framePosition;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.CalculateUpdateFrames(this.m_RenderingFrameIndex, this.m_RenderingFrameTime, chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrames).m_Index, out updateFrame1, out updateFrame2, out framePosition);
          // ISSUE: reference to a compiler-generated method
          position2 = ObjectInterpolateSystem.CalculateTransform(bufferData3[(int) updateFrame1], bufferData3[(int) updateFrame2], framePosition).m_Position;
        }
        else
        {
          Game.Objects.Transform componentData12;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Transforms.TryGetComponent(entity, out componentData12))
            return false;
          position2 = componentData12.m_Position;
        }
        prevWaypointIndex = math.select(componentData4.m_Index - 1, bufferData1.Length - 1, componentData4.m_Index == 0);
        float3 position3;
        // ISSUE: reference to a compiler-generated method
        if (prevWaypointIndex >= bufferData1.Length || !this.GetWaypointPosition(bufferData1[prevWaypointIndex].m_Waypoint, out position3))
          return false;
        distanceFromWaypoint += math.distance(position3, position2);
        float3 position4 = position2;
        if ((componentData3.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Obsolete | PathFlags.Updated | PathFlags.DivertObsolete)) == (PathFlags) 0 || (componentData3.m_State & (PathFlags.Failed | PathFlags.Append)) == PathFlags.Append)
        {
          unknownPath = false;
          CarCurrentLane componentData13;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CarCurrentLanes.TryGetComponent(entity, out componentData13))
          {
            // ISSUE: reference to a compiler-generated method
            this.AddDistance(ref distanceToWaypoint, ref position4, componentData13.m_Lane, componentData13.m_CurvePosition.xz);
          }
          else
          {
            TrainCurrentLane componentData14;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TrainCurrentLanes.TryGetComponent(entity, out componentData14))
            {
              // ISSUE: reference to a compiler-generated method
              this.AddDistance(ref distanceToWaypoint, ref position4, componentData14.m_Front.m_Lane, componentData14.m_Front.m_CurvePosition.yw);
            }
            else
            {
              WatercraftCurrentLane componentData15;
              // ISSUE: reference to a compiler-generated field
              if (this.m_WatercraftCurrentLanes.TryGetComponent(entity, out componentData15))
              {
                // ISSUE: reference to a compiler-generated method
                this.AddDistance(ref distanceToWaypoint, ref position4, componentData15.m_Lane, componentData15.m_CurvePosition.xz);
              }
              else
              {
                AircraftCurrentLane componentData16;
                // ISSUE: reference to a compiler-generated field
                if (this.m_AircraftCurrentLanes.TryGetComponent(entity, out componentData16))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddDistance(ref distanceToWaypoint, ref position4, componentData16.m_Lane, componentData16.m_CurvePosition.xz);
                }
              }
            }
          }
          DynamicBuffer<CarNavigationLane> bufferData4;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CarNavigationLaneBuffers.TryGetBuffer(transportVehicle, out bufferData4))
          {
            for (int index = 0; index < bufferData4.Length; ++index)
            {
              CarNavigationLane carNavigationLane = bufferData4[index];
              // ISSUE: reference to a compiler-generated method
              this.AddDistance(ref distanceToWaypoint, ref position4, carNavigationLane.m_Lane, carNavigationLane.m_CurvePosition);
            }
          }
          else
          {
            DynamicBuffer<TrainNavigationLane> bufferData5;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TrainNavigationLaneBuffers.TryGetBuffer(transportVehicle, out bufferData5))
            {
              for (int index = 0; index < bufferData5.Length; ++index)
              {
                TrainNavigationLane trainNavigationLane = bufferData5[index];
                // ISSUE: reference to a compiler-generated method
                this.AddDistance(ref distanceToWaypoint, ref position4, trainNavigationLane.m_Lane, trainNavigationLane.m_CurvePosition);
              }
            }
            else
            {
              DynamicBuffer<WatercraftNavigationLane> bufferData6;
              // ISSUE: reference to a compiler-generated field
              if (this.m_WatercraftNavigationLaneBuffers.TryGetBuffer(transportVehicle, out bufferData6))
              {
                for (int index = 0; index < bufferData6.Length; ++index)
                {
                  WatercraftNavigationLane watercraftNavigationLane = bufferData6[index];
                  // ISSUE: reference to a compiler-generated method
                  this.AddDistance(ref distanceToWaypoint, ref position4, watercraftNavigationLane.m_Lane, watercraftNavigationLane.m_CurvePosition);
                }
              }
              else
              {
                DynamicBuffer<AircraftNavigationLane> bufferData7;
                // ISSUE: reference to a compiler-generated field
                if (this.m_AircraftNavigationLaneBuffers.TryGetBuffer(transportVehicle, out bufferData7))
                {
                  for (int index = 0; index < bufferData7.Length; ++index)
                  {
                    AircraftNavigationLane aircraftNavigationLane = bufferData7[index];
                    // ISSUE: reference to a compiler-generated method
                    this.AddDistance(ref distanceToWaypoint, ref position4, aircraftNavigationLane.m_Lane, aircraftNavigationLane.m_CurvePosition);
                  }
                }
              }
            }
          }
          DynamicBuffer<PathElement> bufferData8;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathElementBuffers.TryGetBuffer(transportVehicle, out bufferData8))
          {
            for (int elementIndex = componentData3.m_ElementIndex; elementIndex < bufferData8.Length; ++elementIndex)
            {
              PathElement pathElement = bufferData8[elementIndex];
              // ISSUE: reference to a compiler-generated method
              this.AddDistance(ref distanceToWaypoint, ref position4, pathElement.m_Target, pathElement.m_TargetDelta);
            }
          }
          DynamicBuffer<RouteSegment> bufferData9;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((componentData3.m_State & (PathFlags.Pending | PathFlags.Obsolete)) != (PathFlags) 0 && (componentData3.m_State & PathFlags.Append) != (PathFlags) 0 && this.m_RouteSegmentBuffers.TryGetBuffer(transportRoute, out bufferData9) && this.m_PathElementBuffers.TryGetBuffer(bufferData9[prevWaypointIndex].m_Segment, out bufferData8))
          {
            for (int index = 0; index < bufferData8.Length; ++index)
            {
              PathElement pathElement = bufferData8[index];
              // ISSUE: reference to a compiler-generated method
              this.AddDistance(ref distanceToWaypoint, ref position4, pathElement.m_Target, pathElement.m_TargetDelta);
            }
          }
        }
        distanceToWaypoint += math.distance(position4, position1);
        distanceFromWaypoint = math.max(0.0f, distanceFromWaypoint);
        distanceToWaypoint = math.max(0.0f, distanceToWaypoint);
        return true;
      }

      private void AddDistance(
        ref float distance,
        ref float3 position,
        Entity lane,
        float2 curveDelta)
      {
        SlaveLane componentData1;
        Owner componentData2;
        DynamicBuffer<Game.Net.SubLane> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_SlaveLanes.TryGetComponent(lane, out componentData1) && this.m_Owners.TryGetComponent(lane, out componentData2) && this.m_SubLaneBuffers.TryGetBuffer(componentData2.m_Owner, out bufferData) && (int) componentData1.m_MasterIndex < bufferData.Length)
          lane = bufferData[(int) componentData1.m_MasterIndex].m_SubLane;
        Curve componentData3;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Curves.TryGetComponent(lane, out componentData3))
          return;
        distance += math.distance(position, MathUtils.Position(componentData3.m_Bezier, curveDelta.x));
        if ((double) curveDelta.x == 0.0 && (double) curveDelta.y == 1.0 || (double) curveDelta.x == 1.0 && (double) curveDelta.y == 0.0)
          distance += componentData3.m_Length;
        else
          distance += MathUtils.Length(componentData3.m_Bezier, new Bounds1(curveDelta));
        position = MathUtils.Position(componentData3.m_Bezier, curveDelta.y);
      }

      private (int, int) GetCargo(Entity entity)
      {
        int num1 = 0;
        int num2 = 0;
        PrefabRef componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRefs.TryGetComponent(entity, out componentData1))
        {
          DynamicBuffer<LayoutElement> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LayoutElementBuffers.TryGetBuffer(entity, out bufferData1))
          {
            for (int index1 = 0; index1 < bufferData1.Length; ++index1)
            {
              Entity vehicle = bufferData1[index1].m_Vehicle;
              DynamicBuffer<Passenger> bufferData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PassengerBuffers.TryGetBuffer(vehicle, out bufferData2))
              {
                for (int index2 = 0; index2 < bufferData2.Length; ++index2)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_Pets.HasComponent(bufferData2[index2].m_Passenger))
                    ++num1;
                }
              }
              else
              {
                DynamicBuffer<Game.Economy.Resources> bufferData3;
                // ISSUE: reference to a compiler-generated field
                if (this.m_EconomyResourcesBuffers.TryGetBuffer(vehicle, out bufferData3))
                {
                  for (int index3 = 0; index3 < bufferData3.Length; ++index3)
                    num1 += bufferData3[index3].m_Amount;
                }
              }
              PrefabRef componentData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefs.TryGetComponent(vehicle, out componentData2))
              {
                Entity prefab = componentData2.m_Prefab;
                PublicTransportVehicleData componentData3;
                // ISSUE: reference to a compiler-generated field
                if (this.m_PublicTransportVehicleDatas.TryGetComponent(prefab, out componentData3))
                {
                  num2 += componentData3.m_PassengerCapacity;
                }
                else
                {
                  CargoTransportVehicleData componentData4;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_CargoTransportVehicleDatas.TryGetComponent(prefab, out componentData4))
                    num2 += componentData4.m_CargoCapacity;
                }
              }
            }
          }
          else
          {
            DynamicBuffer<Passenger> bufferData4;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PassengerBuffers.TryGetBuffer(entity, out bufferData4))
            {
              for (int index = 0; index < bufferData4.Length; ++index)
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.m_Pets.HasComponent(bufferData4[index].m_Passenger))
                  ++num1;
              }
            }
            else
            {
              DynamicBuffer<Game.Economy.Resources> bufferData5;
              // ISSUE: reference to a compiler-generated field
              if (this.m_EconomyResourcesBuffers.TryGetBuffer(entity, out bufferData5))
              {
                for (int index = 0; index < bufferData5.Length; ++index)
                  num1 += bufferData5[index].m_Amount;
              }
            }
            PublicTransportVehicleData componentData5;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PublicTransportVehicleDatas.TryGetComponent(componentData1.m_Prefab, out componentData5))
            {
              num2 = componentData5.m_PassengerCapacity;
            }
            else
            {
              CargoTransportVehicleData componentData6;
              // ISSUE: reference to a compiler-generated field
              if (this.m_CargoTransportVehicleDatas.TryGetComponent(componentData1.m_Prefab, out componentData6))
                num2 += componentData6.m_CargoCapacity;
            }
          }
        }
        return (num1, num2);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Route> __Game_Routes_Route_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportLine> __Game_Routes_TransportLine_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TransportStop> __Game_Routes_TransportStop_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TaxiStand> __Game_Routes_TaxiStand_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> __Game_Vehicles_PublicTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentRoute> __Game_Routes_CurrentRoute_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteSegment> __Game_Routes_RouteSegment_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteVehicle> __Game_Routes_RouteVehicle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedRoute> __Game_Routes_ConnectedRoute_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Color> __Game_Routes_Color_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Connected> __Game_Routes_Connected_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaitingPassengers> __Game_Routes_WaitingPassengers_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Position> __Game_Routes_Position_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteLane> __Game_Routes_RouteLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Common.Target> __Game_Common_Target_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathOwner> __Game_Pathfind_PathOwner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Waypoint> __Game_Routes_Waypoint_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Train> __Game_Vehicles_Train_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainCurrentLane> __Game_Vehicles_TrainCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WatercraftCurrentLane> __Game_Vehicles_WatercraftCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AircraftCurrentLane> __Game_Vehicles_AircraftCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Creatures.Pet> __Game_Creatures_Pet_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportLineData> __Game_Prefabs_TransportLineData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainData> __Game_Prefabs_TrainData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PublicTransportVehicleData> __Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> __Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CullingInfo> __Game_Rendering_CullingInfo_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CarNavigationLane> __Game_Vehicles_CarNavigationLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<TrainNavigationLane> __Game_Vehicles_TrainNavigationLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<WatercraftNavigationLane> __Game_Vehicles_WatercraftNavigationLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<AircraftNavigationLane> __Game_Vehicles_AircraftNavigationLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Passenger> __Game_Vehicles_Passenger_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<TransformFrame> __Game_Objects_TransformFrame_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Route_RO_ComponentLookup = state.GetComponentLookup<Route>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TransportLine_RO_ComponentLookup = state.GetComponentLookup<TransportLine>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TransportStop_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.TransportStop>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TaxiStand_RO_ComponentLookup = state.GetComponentLookup<TaxiStand>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentLookup = state.GetComponentLookup<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PublicTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_CurrentRoute_RO_ComponentLookup = state.GetComponentLookup<CurrentRoute>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteSegment_RO_BufferLookup = state.GetBufferLookup<RouteSegment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteVehicle_RO_BufferLookup = state.GetBufferLookup<RouteVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_ConnectedRoute_RO_BufferLookup = state.GetBufferLookup<ConnectedRoute>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Color_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.Color>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RO_ComponentLookup = state.GetComponentLookup<Connected>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_WaitingPassengers_RO_ComponentLookup = state.GetComponentLookup<WaitingPassengers>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentLookup = state.GetComponentLookup<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteLane_RO_ComponentLookup = state.GetComponentLookup<RouteLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentLookup = state.GetComponentLookup<Game.Common.Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RO_ComponentLookup = state.GetComponentLookup<PathOwner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Waypoint_RO_ComponentLookup = state.GetComponentLookup<Waypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Train_RO_ComponentLookup = state.GetComponentLookup<Train>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup = state.GetComponentLookup<CarCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup = state.GetComponentLookup<TrainCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentLookup = state.GetComponentLookup<WatercraftCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftCurrentLane_RO_ComponentLookup = state.GetComponentLookup<AircraftCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Pet_RO_ComponentLookup = state.GetComponentLookup<Game.Creatures.Pet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportLineData_RO_ComponentLookup = state.GetComponentLookup<TransportLineData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrainData_RO_ComponentLookup = state.GetComponentLookup<TrainData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup = state.GetComponentLookup<PublicTransportVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup = state.GetComponentLookup<CargoTransportVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RO_ComponentLookup = state.GetComponentLookup<CullingInfo>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarNavigationLane_RO_BufferLookup = state.GetBufferLookup<CarNavigationLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainNavigationLane_RO_BufferLookup = state.GetBufferLookup<TrainNavigationLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftNavigationLane_RO_BufferLookup = state.GetBufferLookup<WatercraftNavigationLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftNavigationLane_RO_BufferLookup = state.GetBufferLookup<AircraftNavigationLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferLookup = state.GetBufferLookup<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RO_BufferLookup = state.GetBufferLookup<Passenger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RO_BufferLookup = state.GetBufferLookup<TransformFrame>(true);
      }
    }
  }
}
