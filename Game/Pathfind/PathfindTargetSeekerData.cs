// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathfindTargetSeekerData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Routes;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Pathfind
{
  public struct PathfindTargetSeekerData
  {
    [ReadOnly]
    public AirwayHelpers.AirwayData m_AirwayData;
    [ReadOnly]
    public ComponentLookup<Owner> m_Owner;
    [ReadOnly]
    public ComponentLookup<Transform> m_Transform;
    [ReadOnly]
    public ComponentLookup<Attached> m_Attached;
    [ReadOnly]
    public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocation;
    [ReadOnly]
    public ComponentLookup<Stopped> m_Stopped;
    [ReadOnly]
    public ComponentLookup<HumanCurrentLane> m_HumanCurrentLane;
    [ReadOnly]
    public ComponentLookup<CarCurrentLane> m_CarCurrentLane;
    [ReadOnly]
    public ComponentLookup<TrainCurrentLane> m_TrainCurrentLane;
    [ReadOnly]
    public ComponentLookup<WatercraftCurrentLane> m_WatercraftCurrentLane;
    [ReadOnly]
    public ComponentLookup<AircraftCurrentLane> m_AircraftCurrentLane;
    [ReadOnly]
    public ComponentLookup<ParkedCar> m_ParkedCar;
    [ReadOnly]
    public ComponentLookup<ParkedTrain> m_ParkedTrain;
    [ReadOnly]
    public ComponentLookup<Train> m_Train;
    [ReadOnly]
    public ComponentLookup<Airplane> m_Airplane;
    [ReadOnly]
    public ComponentLookup<Building> m_Building;
    [ReadOnly]
    public ComponentLookup<PropertyRenter> m_PropertyRenter;
    [ReadOnly]
    public ComponentLookup<CurrentBuilding> m_CurrentBuilding;
    [ReadOnly]
    public ComponentLookup<CurrentTransport> m_CurrentTransport;
    [ReadOnly]
    public ComponentLookup<Curve> m_Curve;
    [ReadOnly]
    public ComponentLookup<Game.Net.PedestrianLane> m_PedestrianLane;
    [ReadOnly]
    public ComponentLookup<Game.Net.ParkingLane> m_ParkingLane;
    [ReadOnly]
    public ComponentLookup<Game.Net.CarLane> m_CarLane;
    [ReadOnly]
    public ComponentLookup<MasterLane> m_MasterLane;
    [ReadOnly]
    public ComponentLookup<SlaveLane> m_SlaveLane;
    [ReadOnly]
    public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLane;
    [ReadOnly]
    public ComponentLookup<NodeLane> m_NodeLane;
    [ReadOnly]
    public ComponentLookup<LaneConnection> m_LaneConnection;
    [ReadOnly]
    public ComponentLookup<RouteLane> m_RouteLane;
    [ReadOnly]
    public ComponentLookup<AccessLane> m_AccessLane;
    [ReadOnly]
    public ComponentLookup<PrefabRef> m_PrefabRef;
    [ReadOnly]
    public ComponentLookup<BuildingData> m_BuildingData;
    [ReadOnly]
    public ComponentLookup<PathfindCarData> m_CarPathfindData;
    [ReadOnly]
    public ComponentLookup<Game.Prefabs.SpawnLocationData> m_SpawnLocationData;
    [ReadOnly]
    public ComponentLookup<NetLaneData> m_NetLaneData;
    [ReadOnly]
    public ComponentLookup<CarLaneData> m_CarLaneData;
    [ReadOnly]
    public ComponentLookup<ParkingLaneData> m_ParkingLaneData;
    [ReadOnly]
    public ComponentLookup<TrackLaneData> m_TrackLaneData;
    [ReadOnly]
    public BufferLookup<Game.Net.SubLane> m_SubLane;
    [ReadOnly]
    public BufferLookup<Game.Areas.Node> m_AreaNode;
    [ReadOnly]
    public BufferLookup<Triangle> m_AreaTriangle;
    [ReadOnly]
    public BufferLookup<SpawnLocationElement> m_SpawnLocations;
    [ReadOnly]
    public BufferLookup<LayoutElement> m_VehicleLayout;
    [ReadOnly]
    public BufferLookup<CarNavigationLane> m_CarNavigationLanes;
    [ReadOnly]
    public BufferLookup<WatercraftNavigationLane> m_WatercraftNavigationLanes;
    [ReadOnly]
    public BufferLookup<AircraftNavigationLane> m_AircraftNavigationLanes;

    public PathfindTargetSeekerData(SystemBase system)
    {
      this.m_AirwayData = new AirwayHelpers.AirwayData();
      this.m_Owner = system.GetComponentLookup<Owner>(true);
      this.m_Transform = system.GetComponentLookup<Transform>(true);
      this.m_Attached = system.GetComponentLookup<Attached>(true);
      this.m_SpawnLocation = system.GetComponentLookup<Game.Objects.SpawnLocation>(true);
      this.m_Stopped = system.GetComponentLookup<Stopped>(true);
      this.m_HumanCurrentLane = system.GetComponentLookup<HumanCurrentLane>(true);
      this.m_CarCurrentLane = system.GetComponentLookup<CarCurrentLane>(true);
      this.m_TrainCurrentLane = system.GetComponentLookup<TrainCurrentLane>(true);
      this.m_WatercraftCurrentLane = system.GetComponentLookup<WatercraftCurrentLane>(true);
      this.m_AircraftCurrentLane = system.GetComponentLookup<AircraftCurrentLane>(true);
      this.m_ParkedCar = system.GetComponentLookup<ParkedCar>(true);
      this.m_ParkedTrain = system.GetComponentLookup<ParkedTrain>(true);
      this.m_Train = system.GetComponentLookup<Train>(true);
      this.m_Airplane = system.GetComponentLookup<Airplane>(true);
      this.m_Building = system.GetComponentLookup<Building>(true);
      this.m_PropertyRenter = system.GetComponentLookup<PropertyRenter>(true);
      this.m_CurrentBuilding = system.GetComponentLookup<CurrentBuilding>(true);
      this.m_CurrentTransport = system.GetComponentLookup<CurrentTransport>(true);
      this.m_Curve = system.GetComponentLookup<Curve>(true);
      this.m_PedestrianLane = system.GetComponentLookup<Game.Net.PedestrianLane>(true);
      this.m_ParkingLane = system.GetComponentLookup<Game.Net.ParkingLane>(true);
      this.m_CarLane = system.GetComponentLookup<Game.Net.CarLane>(true);
      this.m_MasterLane = system.GetComponentLookup<MasterLane>(true);
      this.m_SlaveLane = system.GetComponentLookup<SlaveLane>(true);
      this.m_ConnectionLane = system.GetComponentLookup<Game.Net.ConnectionLane>(true);
      this.m_NodeLane = system.GetComponentLookup<NodeLane>(true);
      this.m_LaneConnection = system.GetComponentLookup<LaneConnection>(true);
      this.m_RouteLane = system.GetComponentLookup<RouteLane>(true);
      this.m_AccessLane = system.GetComponentLookup<AccessLane>(true);
      this.m_PrefabRef = system.GetComponentLookup<PrefabRef>(true);
      this.m_BuildingData = system.GetComponentLookup<BuildingData>(true);
      this.m_CarPathfindData = system.GetComponentLookup<PathfindCarData>(true);
      this.m_SpawnLocationData = system.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
      this.m_NetLaneData = system.GetComponentLookup<NetLaneData>(true);
      this.m_CarLaneData = system.GetComponentLookup<CarLaneData>(true);
      this.m_ParkingLaneData = system.GetComponentLookup<ParkingLaneData>(true);
      this.m_TrackLaneData = system.GetComponentLookup<TrackLaneData>(true);
      this.m_SubLane = system.GetBufferLookup<Game.Net.SubLane>(true);
      this.m_AreaNode = system.GetBufferLookup<Game.Areas.Node>(true);
      this.m_AreaTriangle = system.GetBufferLookup<Triangle>(true);
      this.m_SpawnLocations = system.GetBufferLookup<SpawnLocationElement>(true);
      this.m_VehicleLayout = system.GetBufferLookup<LayoutElement>(true);
      this.m_CarNavigationLanes = system.GetBufferLookup<CarNavigationLane>(true);
      this.m_WatercraftNavigationLanes = system.GetBufferLookup<WatercraftNavigationLane>(true);
      this.m_AircraftNavigationLanes = system.GetBufferLookup<AircraftNavigationLane>(true);
    }

    public void Update(SystemBase system, AirwayHelpers.AirwayData airwayData)
    {
      this.m_AirwayData = airwayData;
      this.m_Owner.Update(system);
      this.m_Transform.Update(system);
      this.m_Attached.Update(system);
      this.m_SpawnLocation.Update(system);
      this.m_Stopped.Update(system);
      this.m_HumanCurrentLane.Update(system);
      this.m_CarCurrentLane.Update(system);
      this.m_TrainCurrentLane.Update(system);
      this.m_WatercraftCurrentLane.Update(system);
      this.m_AircraftCurrentLane.Update(system);
      this.m_ParkedCar.Update(system);
      this.m_ParkedTrain.Update(system);
      this.m_Train.Update(system);
      this.m_Airplane.Update(system);
      this.m_Building.Update(system);
      this.m_PropertyRenter.Update(system);
      this.m_CurrentBuilding.Update(system);
      this.m_CurrentTransport.Update(system);
      this.m_Curve.Update(system);
      this.m_PedestrianLane.Update(system);
      this.m_ParkingLane.Update(system);
      this.m_CarLane.Update(system);
      this.m_MasterLane.Update(system);
      this.m_SlaveLane.Update(system);
      this.m_ConnectionLane.Update(system);
      this.m_NodeLane.Update(system);
      this.m_LaneConnection.Update(system);
      this.m_RouteLane.Update(system);
      this.m_AccessLane.Update(system);
      this.m_PrefabRef.Update(system);
      this.m_BuildingData.Update(system);
      this.m_CarPathfindData.Update(system);
      this.m_SpawnLocationData.Update(system);
      this.m_NetLaneData.Update(system);
      this.m_CarLaneData.Update(system);
      this.m_ParkingLaneData.Update(system);
      this.m_TrackLaneData.Update(system);
      this.m_SubLane.Update(system);
      this.m_AreaNode.Update(system);
      this.m_AreaTriangle.Update(system);
      this.m_SpawnLocations.Update(system);
      this.m_VehicleLayout.Update(system);
      this.m_CarNavigationLanes.Update(system);
      this.m_WatercraftNavigationLanes.Update(system);
      this.m_AircraftNavigationLanes.Update(system);
    }
  }
}
