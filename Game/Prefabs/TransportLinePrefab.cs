// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TransportLinePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using Game.Pathfind;
using Game.Policies;
using Game.Routes;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Routes/", new Type[] {})]
  public class TransportLinePrefab : RoutePrefab
  {
    public RouteConnectionType m_AccessConnectionType = RouteConnectionType.Pedestrian;
    public RouteConnectionType m_RouteConnectionType = RouteConnectionType.Road;
    public TrackTypes m_AccessTrackType;
    public TrackTypes m_RouteTrackType;
    public RoadTypes m_AccessRoadType;
    public RoadTypes m_RouteRoadType;
    public TransportType m_TransportType;
    public float m_DefaultVehicleInterval = 15f;
    public float m_DefaultUnbunchingFactor = 0.75f;
    public float m_StopDuration = 1f;
    public bool m_PassengerTransport = true;
    public bool m_CargoTransport;
    public PathfindPrefab m_PathfindPrefab;
    public NotificationIconPrefab m_VehicleNotification;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_PathfindPrefab);
      prefabs.Add((PrefabBase) this.m_VehicleNotification);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<RouteConnectionData>());
      components.Add(ComponentType.ReadWrite<TransportLineData>());
      components.Add(ComponentType.ReadWrite<PlaceableInfoviewItem>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      if (components.Contains(ComponentType.ReadWrite<Route>()))
      {
        components.Add(ComponentType.ReadWrite<TransportLine>());
        components.Add(ComponentType.ReadWrite<VehicleModel>());
        components.Add(ComponentType.ReadWrite<DispatchedRequest>());
        components.Add(ComponentType.ReadWrite<RouteNumber>());
        components.Add(ComponentType.ReadWrite<RouteVehicle>());
        components.Add(ComponentType.ReadWrite<RouteModifier>());
        components.Add(ComponentType.ReadWrite<Policy>());
      }
      else if (components.Contains(ComponentType.ReadWrite<Waypoint>()))
      {
        if (this.m_AccessConnectionType != RouteConnectionType.None)
          components.Add(ComponentType.ReadWrite<AccessLane>());
        if (this.m_RouteConnectionType != RouteConnectionType.None)
          components.Add(ComponentType.ReadWrite<RouteLane>());
        if (components.Contains(ComponentType.ReadWrite<Connected>()))
          components.Add(ComponentType.ReadWrite<VehicleTiming>());
        if (!this.m_PassengerTransport)
          return;
        components.Add(ComponentType.ReadWrite<WaitingPassengers>());
      }
      else
      {
        if (!components.Contains(ComponentType.ReadWrite<Game.Routes.Segment>()))
          return;
        components.Add(ComponentType.ReadWrite<PathTargets>());
        components.Add(ComponentType.ReadWrite<RouteInfo>());
        components.Add(ComponentType.ReadWrite<PathElement>());
        components.Add(ComponentType.ReadWrite<PathInformation>());
      }
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      entityManager.SetComponentData<RouteConnectionData>(entity, new RouteConnectionData()
      {
        m_AccessConnectionType = this.m_AccessConnectionType,
        m_RouteConnectionType = this.m_RouteConnectionType,
        m_AccessTrackType = this.m_AccessTrackType,
        m_RouteTrackType = this.m_RouteTrackType,
        m_AccessRoadType = this.m_AccessRoadType,
        m_RouteRoadType = this.m_RouteRoadType,
        m_StartLaneOffset = 0.0f,
        m_EndMargin = 0.0f
      });
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<TransportLineData>(entity, new TransportLineData()
      {
        m_PathfindPrefab = existingSystemManaged.GetEntity((PrefabBase) this.m_PathfindPrefab),
        m_TransportType = this.m_TransportType,
        m_DefaultVehicleInterval = this.m_DefaultVehicleInterval,
        m_DefaultUnbunchingFactor = this.m_DefaultUnbunchingFactor,
        m_StopDuration = this.m_StopDuration,
        m_PassengerTransport = this.m_PassengerTransport,
        m_CargoTransport = this.m_CargoTransport,
        m_VehicleNotification = existingSystemManaged.GetEntity((PrefabBase) this.m_VehicleNotification)
      });
    }
  }
}
