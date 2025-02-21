// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TransportStop
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using Game.Routes;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Routes/", new System.Type[] {typeof (ObjectPrefab)})]
  public class TransportStop : ComponentBase
  {
    public TransportType m_TransportType;
    public RouteConnectionType m_AccessConnectionType = RouteConnectionType.Pedestrian;
    public RouteConnectionType m_RouteConnectionType = RouteConnectionType.Road;
    public TrackTypes m_AccessTrackType;
    public TrackTypes m_RouteTrackType;
    public RoadTypes m_AccessRoadType;
    public RoadTypes m_RouteRoadType;
    public float m_EnterDistance;
    public float m_ExitDistance;
    public float m_AccessDistance;
    public float m_BoardingTime;
    public float m_ComfortFactor;
    public float m_LoadingFactor;
    public bool m_PassengerTransport = true;
    public bool m_CargoTransport;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<TransportStopData>());
      components.Add(ComponentType.ReadWrite<RouteConnectionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Routes.TransportStop>());
      components.Add(ComponentType.ReadWrite<Game.Objects.Color>());
      switch (this.m_TransportType)
      {
        case TransportType.Bus:
          components.Add(ComponentType.ReadWrite<ConnectedRoute>());
          components.Add(ComponentType.ReadWrite<BoardingVehicle>());
          components.Add(ComponentType.ReadWrite<BusStop>());
          break;
        case TransportType.Train:
          components.Add(ComponentType.ReadWrite<ConnectedRoute>());
          components.Add(ComponentType.ReadWrite<BoardingVehicle>());
          components.Add(ComponentType.ReadWrite<TrainStop>());
          break;
        case TransportType.Taxi:
          components.Add(ComponentType.ReadWrite<BoardingVehicle>());
          components.Add(ComponentType.ReadWrite<RouteVehicle>());
          components.Add(ComponentType.ReadWrite<TaxiStand>());
          components.Add(ComponentType.ReadWrite<DispatchedRequest>());
          if (this.m_AccessConnectionType != RouteConnectionType.None)
            components.Add(ComponentType.ReadWrite<AccessLane>());
          if (this.m_RouteConnectionType != RouteConnectionType.None)
            components.Add(ComponentType.ReadWrite<RouteLane>());
          if (!this.m_PassengerTransport)
            break;
          components.Add(ComponentType.ReadWrite<WaitingPassengers>());
          break;
        case TransportType.Tram:
          components.Add(ComponentType.ReadWrite<ConnectedRoute>());
          components.Add(ComponentType.ReadWrite<BoardingVehicle>());
          components.Add(ComponentType.ReadWrite<TramStop>());
          break;
        case TransportType.Ship:
          components.Add(ComponentType.ReadWrite<ConnectedRoute>());
          components.Add(ComponentType.ReadWrite<BoardingVehicle>());
          components.Add(ComponentType.ReadWrite<ShipStop>());
          break;
        case TransportType.Helicopter:
        case TransportType.Rocket:
          components.Add(ComponentType.ReadWrite<BoardingVehicle>());
          components.Add(ComponentType.ReadWrite<ConnectedRoute>());
          break;
        case TransportType.Airplane:
          components.Add(ComponentType.ReadWrite<BoardingVehicle>());
          components.Add(ComponentType.ReadWrite<ConnectedRoute>());
          components.Add(ComponentType.ReadWrite<AirplaneStop>());
          if (!((UnityEngine.Object) this.GetComponent<OutsideConnection>() != (UnityEngine.Object) null))
            break;
          components.Add(ComponentType.ReadWrite<Game.Net.SubLane>());
          break;
        case TransportType.Subway:
          components.Add(ComponentType.ReadWrite<ConnectedRoute>());
          components.Add(ComponentType.ReadWrite<BoardingVehicle>());
          components.Add(ComponentType.ReadWrite<SubwayStop>());
          break;
      }
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      RouteConnectionData componentData1;
      componentData1.m_AccessConnectionType = this.m_AccessConnectionType;
      componentData1.m_RouteConnectionType = this.m_RouteConnectionType;
      componentData1.m_AccessTrackType = this.m_AccessTrackType;
      componentData1.m_RouteTrackType = this.m_RouteTrackType;
      componentData1.m_AccessRoadType = this.m_AccessRoadType;
      componentData1.m_RouteRoadType = this.m_RouteRoadType;
      componentData1.m_StartLaneOffset = this.m_EnterDistance;
      componentData1.m_EndMargin = this.m_ExitDistance;
      TransportStopData componentData2 = new TransportStopData();
      componentData2.m_ComfortFactor = this.m_ComfortFactor;
      componentData2.m_LoadingFactor = this.m_LoadingFactor;
      componentData2.m_AccessDistance = this.m_AccessDistance;
      componentData2.m_BoardingTime = this.m_BoardingTime;
      componentData2.m_TransportType = this.m_TransportType;
      componentData2.m_PassengerTransport = this.m_PassengerTransport;
      componentData2.m_CargoTransport = this.m_CargoTransport;
      entityManager.SetComponentData<RouteConnectionData>(entity, componentData1);
      entityManager.SetComponentData<TransportStopData>(entity, componentData2);
    }
  }
}
