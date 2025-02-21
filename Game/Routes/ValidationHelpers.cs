// Decompiled with JetBrains decompiler
// Type: Game.Routes.ValidationHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Routes
{
  public static class ValidationHelpers
  {
    public static void ValidateRoute(
      Entity entity,
      Temp temp,
      PrefabRef prefabRef,
      DynamicBuffer<RouteWaypoint> waypoints,
      DynamicBuffer<RouteSegment> segments,
      ValidationSystem.EntityData data,
      NativeQueue<ErrorData>.ParallelWriter errorQueue)
    {
      TransportLineData componentData1;
      // ISSUE: reference to a compiler-generated field
      if (data.m_TransportLineData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
      {
        for (int index = 0; index < waypoints.Length; ++index)
        {
          Entity waypoint = waypoints[index].m_Waypoint;
          Connected componentData2;
          PrefabRef componentData3;
          TransportStopData componentData4;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (data.m_RouteConnected.TryGetComponent(waypoint, out componentData2) && data.m_PrefabRef.TryGetComponent(componentData2.m_Connected, out componentData3) && data.m_TransportStopData.TryGetComponent(componentData3.m_Prefab, out componentData4))
          {
            if (componentData1.m_PassengerTransport & !componentData4.m_PassengerTransport)
              errorQueue.Enqueue(new ErrorData()
              {
                m_ErrorSeverity = ErrorSeverity.Error,
                m_ErrorType = ErrorType.NoPedestrianAccess,
                m_Position = (float3) float.NaN,
                m_TempEntity = waypoint
              });
            if (componentData1.m_CargoTransport & !componentData4.m_CargoTransport)
              errorQueue.Enqueue(new ErrorData()
              {
                m_ErrorSeverity = ErrorSeverity.Error,
                m_ErrorType = ErrorType.NoCargoAccess,
                m_Position = (float3) float.NaN,
                m_TempEntity = waypoint
              });
          }
        }
      }
      bool flag = false;
      Route componentData5;
      // ISSUE: reference to a compiler-generated field
      if (data.m_Route.TryGetComponent(temp.m_Original, out componentData5))
        flag = (componentData5.m_Flags & RouteFlags.Complete) != 0;
      for (int index = 0; index < segments.Length; ++index)
      {
        Entity segment = segments[index].m_Segment;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (data.m_PathInformation.HasComponent(segment) && (double) data.m_PathInformation[segment].m_Distance < 0.0)
        {
          Entity waypoint1 = waypoints[index].m_Waypoint;
          Entity waypoint2 = waypoints[math.select(index + 1, 0, index + 1 == waypoints.Length)].m_Waypoint;
          ErrorData errorData = new ErrorData();
          errorData.m_ErrorSeverity = flag ? ErrorSeverity.Warning : ErrorSeverity.Error;
          errorData.m_ErrorType = ErrorType.PathfindFailed;
          errorData.m_Position = (float3) float.NaN;
          errorData.m_TempEntity = waypoint1;
          errorQueue.Enqueue(errorData);
          errorData.m_TempEntity = waypoint2;
          errorQueue.Enqueue(errorData);
        }
      }
    }

    public static void ValidateStop(
      bool editorMode,
      Entity entity,
      Temp temp,
      Owner owner,
      Transform transform,
      PrefabRef prefabRef,
      Attached attached,
      ValidationSystem.EntityData data,
      NativeQueue<ErrorData>.ParallelWriter errorQueue)
    {
      if ((temp.m_Flags & (TempFlags.Create | TempFlags.Modify)) == (TempFlags) 0)
        return;
      PlaceableObjectData placeableObjectData = new PlaceableObjectData();
      // ISSUE: reference to a compiler-generated field
      if (data.m_PlaceableObject.HasComponent(prefabRef.m_Prefab))
      {
        // ISSUE: reference to a compiler-generated field
        placeableObjectData = data.m_PlaceableObject[prefabRef.m_Prefab];
      }
      if ((placeableObjectData.m_Flags & Game.Objects.PlacementFlags.NetObject) == Game.Objects.PlacementFlags.None || attached.m_Parent != Entity.Null && owner.m_Owner != Entity.Null)
        return;
      // ISSUE: reference to a compiler-generated field
      RouteConnectionData connectionData = data.m_RouteConnectionData[prefabRef.m_Prefab];
      bool2 stopLanes = ValidationHelpers.FindStopLanes(attached, connectionData, data);
      if (math.all(stopLanes))
        return;
      errorQueue.Enqueue(new ErrorData()
      {
        m_ErrorSeverity = !editorMode ? ErrorSeverity.Error : ErrorSeverity.Warning,
        m_ErrorType = math.any(stopLanes) ? (!stopLanes.x ? ValidationHelpers.RouteConnectionToError(connectionData.m_RouteConnectionType) : ValidationHelpers.RouteConnectionToError(connectionData.m_AccessConnectionType)) : (editorMode || connectionData.m_RouteConnectionType != RouteConnectionType.Road || connectionData.m_AccessConnectionType != RouteConnectionType.Pedestrian ? (editorMode || connectionData.m_RouteConnectionType != RouteConnectionType.Track || connectionData.m_AccessConnectionType != RouteConnectionType.Pedestrian ? ValidationHelpers.RouteConnectionToError(connectionData.m_RouteConnectionType) : ErrorType.NoTrackAccess) : ErrorType.NoRoadAccess),
        m_TempEntity = entity,
        m_Position = (float3) float.NaN
      });
    }

    private static ErrorType RouteConnectionToError(RouteConnectionType type)
    {
      switch (type)
      {
        case RouteConnectionType.Road:
          return ErrorType.NoCarAccess;
        case RouteConnectionType.Pedestrian:
          return ErrorType.NoPedestrianAccess;
        case RouteConnectionType.Track:
          return ErrorType.NoTrainAccess;
        case RouteConnectionType.Cargo:
          return ErrorType.NoCargoAccess;
        default:
          return ErrorType.None;
      }
    }

    private static bool2 FindStopLanes(
      Attached attached,
      RouteConnectionData connectionData,
      ValidationSystem.EntityData data)
    {
      bool2 x1 = new bool2();
      x1.x = connectionData.m_RouteConnectionType == RouteConnectionType.None;
      x1.y = connectionData.m_AccessConnectionType == RouteConnectionType.None;
      // ISSUE: reference to a compiler-generated field
      if (!data.m_Lanes.HasBuffer(attached.m_Parent))
        return x1;
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<Game.Net.SubLane> lane = data.m_Lanes[attached.m_Parent];
      bool2 x2 = new bool2();
      x2.x = connectionData.m_RouteConnectionType == RouteConnectionType.Road;
      x2.y = connectionData.m_AccessConnectionType == RouteConnectionType.Road;
      bool2 x3 = new bool2();
      x3.x = connectionData.m_RouteConnectionType == RouteConnectionType.Track;
      x3.y = connectionData.m_AccessConnectionType == RouteConnectionType.Track;
      bool2 x4 = new bool2();
      x4.x = connectionData.m_RouteConnectionType == RouteConnectionType.Pedestrian;
      x4.y = connectionData.m_AccessConnectionType == RouteConnectionType.Pedestrian;
      for (int index = 0; index < lane.Length; ++index)
      {
        Entity subLane = lane[index].m_SubLane;
        // ISSUE: reference to a compiler-generated field
        if (math.any(x2) && data.m_CarLane.HasComponent(subLane))
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = data.m_PrefabRef[subLane];
          // ISSUE: reference to a compiler-generated field
          CarLaneData carLaneData = data.m_CarLaneData[prefabRef.m_Prefab];
          x1 |= x2 & new bool2()
          {
            x = (carLaneData.m_RoadTypes & connectionData.m_RouteRoadType) != 0,
            y = (carLaneData.m_RoadTypes & connectionData.m_AccessRoadType) != 0
          };
        }
        // ISSUE: reference to a compiler-generated field
        if (math.any(x3) && data.m_TrackLane.HasComponent(subLane))
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = data.m_PrefabRef[subLane];
          // ISSUE: reference to a compiler-generated field
          TrackLaneData trackLaneData = data.m_TrackLaneData[prefabRef.m_Prefab];
          x1 |= x3 & new bool2()
          {
            x = (trackLaneData.m_TrackTypes & connectionData.m_RouteTrackType) != 0,
            y = (trackLaneData.m_TrackTypes & connectionData.m_AccessTrackType) != 0
          };
        }
        // ISSUE: reference to a compiler-generated field
        if (math.any(x4) && data.m_PedestrianLane.HasComponent(subLane))
          x1 |= x4;
        if (math.all(x1))
          return x1;
      }
      return x1;
    }
  }
}
