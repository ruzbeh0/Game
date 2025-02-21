// Decompiled with JetBrains decompiler
// Type: Game.Routes.RouteUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Areas;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Routes
{
  public static class RouteUtils
  {
    public const float WAYPOINT_CONNECTION_DISTANCE = 10f;
    public const float ROUTE_VISIBLE_THROUGH_DISTANCE = 100f;
    public const float TRANSPORT_DAY_START_TIME = 0.25f;
    public const float TRANSPORT_DAY_END_TIME = 0.9166667f;
    public const float DEFAULT_TRAVEL_TIME = 0.020833334f;
    public const float TAXI_DISTANCE_FEE = 0.03f;

    public static float GetMinWaypointDistance(RouteData routeData)
    {
      return routeData.m_SnapDistance * 0.5f;
    }

    public static Bounds3 CalculateBounds(Position waypointPosition, RouteData routeData)
    {
      float snapDistance = routeData.m_SnapDistance;
      return new Bounds3(waypointPosition.m_Position - snapDistance, waypointPosition.m_Position + snapDistance);
    }

    public static Bounds3 CalculateBounds(CurveElement curveElement, RouteData routeData)
    {
      float snapDistance = routeData.m_SnapDistance;
      return MathUtils.Expand(MathUtils.Bounds(curveElement.m_Curve), (float3) snapDistance);
    }

    public static void StripTransportSegments<TTransportEstimateBuffer>(
      ref Unity.Mathematics.Random random,
      int length,
      DynamicBuffer<PathElement> path,
      ComponentLookup<Connected> connectedData,
      ComponentLookup<BoardingVehicle> boardingVehicleData,
      ComponentLookup<Owner> ownerData,
      ComponentLookup<Lane> laneData,
      ComponentLookup<Game.Net.ConnectionLane> connectionLaneData,
      ComponentLookup<Curve> curveData,
      ComponentLookup<PrefabRef> prefabRefData,
      ComponentLookup<TransportStopData> prefabTransportStopData,
      BufferLookup<Game.Net.SubLane> subLanes,
      BufferLookup<Game.Areas.Node> areaNodes,
      BufferLookup<Triangle> areaTriangles,
      TTransportEstimateBuffer transportEstimateBuffer)
      where TTransportEstimateBuffer : unmanaged, RouteUtils.ITransportEstimateBuffer
    {
      int index1 = 0;
      while (index1 < length)
      {
        PathElement pathElement1 = path[index1++];
        Entity entity = Entity.Null;
        int num = -1;
        if (connectedData.HasComponent(pathElement1.m_Target))
        {
          Connected connected = connectedData[pathElement1.m_Target];
          if (boardingVehicleData.HasComponent(connected.m_Connected))
          {
            entity = connected.m_Connected;
            num = index1 - 2;
          }
          int index2;
          for (index2 = index1; index2 < length; ++index2)
          {
            PathElement pathElement2 = path[index2];
            if (connectedData.HasComponent(pathElement2.m_Target))
              break;
          }
          if (index2 > index1)
          {
            path.RemoveRange(index1, index2 - index1);
            length -= index2 - index1;
          }
          index1 = index2;
        }
        else if (boardingVehicleData.HasComponent(pathElement1.m_Target))
        {
          entity = pathElement1.m_Target;
          num = index1 - 2;
        }
        if (entity != Entity.Null)
        {
          PrefabRef prefabRef = prefabRefData[entity];
          TransportStopData componentData1;
          if (prefabTransportStopData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
          {
            if (num >= 0 && (double) componentData1.m_AccessDistance > 0.0)
            {
              PathElement pathElement3 = path[num];
              int length1 = path.Length;
              Game.Net.ConnectionLane componentData2;
              if (connectionLaneData.TryGetComponent(pathElement3.m_Target, out componentData2))
              {
                if ((componentData2.m_Flags & ConnectionLaneFlags.Area) != (ConnectionLaneFlags) 0)
                  RouteUtils.OffsetPathTarget_AreaLane(ref random, componentData1.m_AccessDistance, num, path, ownerData, curveData, laneData, connectionLaneData, subLanes, areaNodes, areaTriangles);
              }
              else if (curveData.HasComponent(pathElement3.m_Target))
                RouteUtils.OffsetPathTarget_EdgeLane(ref random, componentData1.m_AccessDistance, num, path, ownerData, laneData, curveData, subLanes);
              index1 += path.Length - length1;
              length += path.Length - length1;
            }
            if ((double) componentData1.m_BoardingTime > 0.0)
            {
              int intRandom = MathUtils.RoundToIntRandom(ref random, componentData1.m_BoardingTime);
              if (intRandom > 0)
                transportEstimateBuffer.AddWaitEstimate(pathElement1.m_Target, intRandom);
            }
          }
        }
      }
    }

    private static void OffsetPathTarget_AreaLane(
      ref Unity.Mathematics.Random random,
      float distance,
      int elementIndex,
      DynamicBuffer<PathElement> path,
      ComponentLookup<Owner> ownerData,
      ComponentLookup<Curve> curveData,
      ComponentLookup<Lane> laneData,
      ComponentLookup<Game.Net.ConnectionLane> connectionLaneData,
      BufferLookup<Game.Net.SubLane> subLanes,
      BufferLookup<Game.Areas.Node> areaNodes,
      BufferLookup<Triangle> areaTriangles)
    {
      PathElement pathElement1 = path[elementIndex];
      Curve curve1 = curveData[pathElement1.m_Target];
      Entity owner = ownerData[pathElement1.m_Target].m_Owner;
      float3 position1 = MathUtils.Position(curve1.m_Bezier, pathElement1.m_TargetDelta.y);
      DynamicBuffer<Game.Areas.Node> areaNode = areaNodes[owner];
      DynamicBuffer<Triangle> areaTriangle = areaTriangles[owner];
      int index1 = -1;
      float max = 0.0f;
      float2 t1;
      for (int index2 = 0; index2 < areaTriangle.Length; ++index2)
      {
        Triangle3 triangle3 = AreaUtils.GetTriangle3(areaNode, areaTriangle[index2]);
        if ((double) MathUtils.Distance(triangle3, position1, out t1) < (double) distance)
        {
          float num = MathUtils.Area(triangle3.xz);
          max += num;
          if ((double) random.NextFloat(max) < (double) num)
            index1 = index2;
        }
      }
      if (index1 == -1)
        return;
      DynamicBuffer<Game.Net.SubLane> subLane1 = subLanes[owner];
      float2 float2_1 = random.NextFloat2((float2) 1f);
      float2 t2 = math.select(float2_1, 1f - float2_1, (double) math.csum(float2_1) > 1.0);
      Triangle3 triangle3_1 = AreaUtils.GetTriangle3(areaNode, areaTriangle[index1]);
      float3 position2 = MathUtils.Position(triangle3_1, t2);
      float num1 = float.MaxValue;
      Entity endEntity = Entity.Null;
      float endCurvePos = 0.0f;
      for (int index3 = 0; index3 < subLane1.Length; ++index3)
      {
        Entity subLane2 = subLane1[index3].m_SubLane;
        if (connectionLaneData.HasComponent(subLane2) && (connectionLaneData[subLane2].m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0)
        {
          Curve curve2 = curveData[subLane2];
          bool2 x = new bool2(MathUtils.Intersect(triangle3_1.xz, curve2.m_Bezier.a.xz, out t1), MathUtils.Intersect(triangle3_1.xz, curve2.m_Bezier.d.xz, out t1));
          if (math.any(x))
          {
            float num2 = MathUtils.Distance(curve2.m_Bezier, position2, out float _);
            if ((double) num2 < (double) num1)
            {
              float2 float2_2 = math.select(new float2(0.0f, 0.49f), math.select(new float2(0.51f, 1f), new float2(0.0f, 1f), x.x), x.y);
              num1 = num2;
              endEntity = subLane2;
              endCurvePos = random.NextFloat(float2_2.x, float2_2.y);
            }
          }
        }
      }
      if (endEntity == Entity.Null)
      {
        Debug.Log((object) string.Format("Target path lane not found ({0}, {1}, {2})", (object) position2.x, (object) position2.y, (object) position2.z));
      }
      else
      {
        int index4;
        for (index4 = elementIndex; index4 > 0; --index4)
        {
          PathElement pathElement2 = path[index4 - 1];
          Owner componentData;
          if (!ownerData.TryGetComponent(pathElement2.m_Target, out componentData) || componentData.m_Owner != owner)
            break;
        }
        NativeList<PathElement> path1 = new NativeList<PathElement>(subLane1.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        PathElement pathElement3 = path[index4];
        AreaUtils.FindAreaPath(ref random, path1, subLane1, pathElement3.m_Target, pathElement3.m_TargetDelta.x, endEntity, endCurvePos, laneData, curveData);
        if (path1.Length != 0)
        {
          int x = elementIndex - index4 + 1;
          int num3 = math.min(x, path1.Length);
          for (int index5 = 0; index5 < num3; ++index5)
            path[index4 + index5] = path1[index5];
          if (path1.Length < x)
          {
            path.RemoveRange(index4 + path1.Length, x - path1.Length);
          }
          else
          {
            for (int index6 = x; index6 < path1.Length; ++index6)
              path.Insert(index4 + index6, path1[index6]);
          }
        }
        path1.Dispose();
      }
    }

    private static void OffsetPathTarget_EdgeLane(
      ref Unity.Mathematics.Random random,
      float distance,
      int elementIndex,
      DynamicBuffer<PathElement> path,
      ComponentLookup<Owner> ownerData,
      ComponentLookup<Lane> laneData,
      ComponentLookup<Curve> curveData,
      BufferLookup<Game.Net.SubLane> subLanes)
    {
      PathElement pathElement1 = path[elementIndex];
      Curve curve = curveData[pathElement1.m_Target];
      float num1 = random.NextFloat(-distance, distance);
      if ((double) num1 >= 0.0)
      {
        Bounds1 t = new Bounds1(pathElement1.m_TargetDelta.y, 1f);
        float length1 = num1;
        if (MathUtils.ClampLength(curve.m_Bezier.xz, ref t, ref length1))
        {
          pathElement1.m_TargetDelta.y = t.max;
          path[elementIndex] = pathElement1;
        }
        else
        {
          Entity target = pathElement1.m_Target;
          if (NetUtils.FindNextLane(ref target, ref ownerData, ref laneData, ref subLanes))
          {
            float length2 = math.max(0.0f, num1 - length1);
            t = new Bounds1(0.0f, 1f);
            curve = curveData[target];
            MathUtils.ClampLength(curve.m_Bezier.xz, ref t, length2);
            if (elementIndex > 0 && path[elementIndex - 1].m_Target == target)
            {
              path.RemoveAt(elementIndex--);
              PathElement pathElement2 = path[elementIndex];
              pathElement2.m_TargetDelta.y = t.max;
              path[elementIndex] = pathElement2;
            }
            else
            {
              path.Insert(elementIndex++, new PathElement()
              {
                m_Target = pathElement1.m_Target,
                m_TargetDelta = new float2(pathElement1.m_TargetDelta.x, 1f)
              });
              pathElement1.m_Target = target;
              pathElement1.m_TargetDelta = new float2(0.0f, t.max);
              path[elementIndex] = pathElement1;
            }
          }
          else
          {
            pathElement1.m_TargetDelta.y = math.saturate(pathElement1.m_TargetDelta.y + (1f - pathElement1.m_TargetDelta.y) * num1 / distance);
            path[elementIndex] = pathElement1;
          }
        }
      }
      else
      {
        float num2 = -num1;
        Bounds1 t = new Bounds1(0.0f, pathElement1.m_TargetDelta.y);
        float length3 = num2;
        if (MathUtils.ClampLengthInverse(curve.m_Bezier.xz, ref t, ref length3))
        {
          pathElement1.m_TargetDelta.y = t.min;
          path[elementIndex] = pathElement1;
        }
        else
        {
          Entity target = pathElement1.m_Target;
          if (NetUtils.FindPrevLane(ref target, ref ownerData, ref laneData, ref subLanes))
          {
            float length4 = math.max(0.0f, num2 - length3);
            t = new Bounds1(0.0f, 1f);
            MathUtils.ClampLengthInverse(curveData[target].m_Bezier.xz, ref t, length4);
            if (elementIndex > 0 && path[elementIndex - 1].m_Target == target)
            {
              path.RemoveAt(elementIndex--);
              PathElement pathElement3 = path[elementIndex];
              pathElement3.m_TargetDelta.y = t.min;
              path[elementIndex] = pathElement3;
            }
            else
            {
              path.Insert(elementIndex++, new PathElement()
              {
                m_Target = pathElement1.m_Target,
                m_TargetDelta = new float2(pathElement1.m_TargetDelta.x, 0.0f)
              });
              pathElement1.m_Target = target;
              pathElement1.m_TargetDelta = new float2(1f, t.min);
              path[elementIndex] = pathElement1;
            }
          }
          else
          {
            pathElement1.m_TargetDelta.y = math.saturate(pathElement1.m_TargetDelta.y - pathElement1.m_TargetDelta.y * num2 / distance);
            path[elementIndex] = pathElement1;
          }
        }
      }
    }

    public static bool GetBoardingVehicle(
      Entity currentLane,
      Entity currentWaypoint,
      Entity targetWaypoint,
      uint minDeparture,
      ref ComponentLookup<Owner> ownerData,
      ref ComponentLookup<Connected> connectedData,
      ref ComponentLookup<BoardingVehicle> boardingVehicleData,
      ref ComponentLookup<CurrentRoute> currentRouteData,
      ref ComponentLookup<AccessLane> accessLaneData,
      ref ComponentLookup<Game.Vehicles.PublicTransport> publicTransportData,
      ref ComponentLookup<Game.Vehicles.Taxi> taxiData,
      ref BufferLookup<ConnectedRoute> connectedRoutes,
      out Entity vehicle,
      out bool testing,
      out bool obsolete)
    {
      AccessLane componentData1;
      if (currentLane != currentWaypoint && accessLaneData.TryGetComponent(currentWaypoint, out componentData1))
      {
        Entity owner1 = Entity.Null;
        Entity owner2 = Entity.Null;
        Owner componentData2;
        if (ownerData.TryGetComponent(currentLane, out componentData2))
          owner1 = componentData2.m_Owner;
        Owner componentData3;
        if (ownerData.TryGetComponent(componentData1.m_Lane, out componentData3))
          owner2 = componentData3.m_Owner;
        Connected componentData4;
        if (owner1 != owner2 && (!connectedData.TryGetComponent(currentWaypoint, out componentData4) || componentData4.m_Connected != currentLane))
        {
          vehicle = Entity.Null;
          testing = false;
          obsolete = true;
          return false;
        }
      }
      BoardingVehicle componentData5;
      if (boardingVehicleData.TryGetComponent(currentWaypoint, out componentData5))
      {
        Game.Vehicles.Taxi componentData6;
        if (componentData5.m_Vehicle != Entity.Null && taxiData.TryGetComponent(componentData5.m_Vehicle, out componentData6) && (componentData6.m_State & TaxiFlags.Boarding) != (TaxiFlags) 0)
        {
          vehicle = componentData5.m_Vehicle;
          testing = false;
          obsolete = false;
          return true;
        }
        vehicle = Entity.Null;
        testing = false;
        obsolete = false;
        return false;
      }
      Connected componentData7;
      Connected componentData8;
      if (connectedData.TryGetComponent(currentWaypoint, out componentData7) && connectedData.TryGetComponent(targetWaypoint, out componentData8))
      {
        Entity connected1 = componentData7.m_Connected;
        Entity connected2 = componentData8.m_Connected;
        DynamicBuffer<ConnectedRoute> bufferData;
        if (boardingVehicleData.TryGetComponent(connected1, out componentData5) && connectedRoutes.TryGetBuffer(connected2, out bufferData))
        {
          CurrentRoute componentData9;
          Game.Vehicles.PublicTransport componentData10;
          if (currentRouteData.TryGetComponent(componentData5.m_Vehicle, out componentData9) && (!publicTransportData.TryGetComponent(componentData5.m_Vehicle, out componentData10) || (componentData10.m_State & (PublicTransportFlags.EnRoute | PublicTransportFlags.Boarding)) == (PublicTransportFlags.EnRoute | PublicTransportFlags.Boarding) && (componentData10.m_DepartureFrame >= minDeparture || (double) componentData10.m_MaxBoardingDistance != 3.4028234663852886E+38)))
          {
            for (int index = 0; index < bufferData.Length; ++index)
            {
              if (ownerData[bufferData[index].m_Waypoint].m_Owner == componentData9.m_Route)
              {
                vehicle = componentData5.m_Vehicle;
                testing = false;
                obsolete = false;
                return true;
              }
            }
          }
          Game.Vehicles.PublicTransport componentData11;
          if (currentRouteData.TryGetComponent(componentData5.m_Testing, out componentData9) && (!publicTransportData.TryGetComponent(componentData5.m_Testing, out componentData11) || (componentData11.m_State & (PublicTransportFlags.EnRoute | PublicTransportFlags.Testing | PublicTransportFlags.RequireStop)) == (PublicTransportFlags.EnRoute | PublicTransportFlags.Testing)))
          {
            for (int index = 0; index < bufferData.Length; ++index)
            {
              if (ownerData[bufferData[index].m_Waypoint].m_Owner == componentData9.m_Route)
              {
                vehicle = componentData5.m_Testing;
                testing = true;
                obsolete = false;
                return false;
              }
            }
          }
          vehicle = Entity.Null;
          testing = false;
          obsolete = false;
          return false;
        }
      }
      vehicle = Entity.Null;
      testing = false;
      obsolete = true;
      return false;
    }

    public static bool ShouldExitVehicle(
      Entity nextLane,
      Entity targetWaypoint,
      Entity currentVehicle,
      ref ComponentLookup<Owner> ownerData,
      ref ComponentLookup<Connected> connectedData,
      ref ComponentLookup<BoardingVehicle> boardingVehicleData,
      ref ComponentLookup<CurrentRoute> currentRouteData,
      ref ComponentLookup<AccessLane> accessLaneData,
      ref ComponentLookup<Game.Vehicles.PublicTransport> publicTransportData,
      ref BufferLookup<ConnectedRoute> connectedRoutes,
      bool testing,
      out bool obsolete)
    {
      Connected componentData1;
      CurrentRoute componentData2;
      if (connectedData.TryGetComponent(targetWaypoint, out componentData1) && currentRouteData.TryGetComponent(currentVehicle, out componentData2))
      {
        Entity connected = componentData1.m_Connected;
        BoardingVehicle componentData3;
        DynamicBuffer<ConnectedRoute> bufferData;
        if (boardingVehicleData.TryGetComponent(connected, out componentData3) && connectedRoutes.TryGetBuffer(connected, out bufferData))
        {
          if ((testing ? componentData3.m_Testing : componentData3.m_Vehicle) == currentVehicle)
          {
            obsolete = false;
            AccessLane componentData4;
            if (nextLane != Entity.Null && accessLaneData.TryGetComponent(targetWaypoint, out componentData4))
            {
              Entity owner1 = Entity.Null;
              Entity owner2 = Entity.Null;
              Owner componentData5;
              if (ownerData.TryGetComponent(nextLane, out componentData5))
                owner1 = componentData5.m_Owner;
              Owner componentData6;
              if (ownerData.TryGetComponent(componentData4.m_Lane, out componentData6))
                owner2 = componentData6.m_Owner;
              if (owner1 != owner2)
                obsolete = true;
            }
            return true;
          }
          Game.Vehicles.PublicTransport componentData7;
          if (publicTransportData.TryGetComponent(currentVehicle, out componentData7) && (componentData7.m_State & PublicTransportFlags.EnRoute) == (PublicTransportFlags) 0)
          {
            obsolete = true;
            return true;
          }
          for (int index = 0; index < bufferData.Length; ++index)
          {
            if (ownerData[bufferData[index].m_Waypoint].m_Owner == componentData2.m_Route)
            {
              obsolete = false;
              return false;
            }
          }
        }
      }
      obsolete = true;
      return true;
    }

    public static float UpdateAverageTravelTime(
      float oldTravelTime,
      uint departureFrame,
      uint arrivalFrame)
    {
      if (departureFrame == 0U)
        return oldTravelTime;
      float y = (float) (arrivalFrame - departureFrame) / 60f;
      return (double) oldTravelTime == 0.0 ? y : math.lerp(oldTravelTime, y, 0.5f);
    }

    public static float GetStopDuration(
      TransportLineData prefabLineData,
      TransportStop transportStop)
    {
      return prefabLineData.m_StopDuration / math.max(0.25f, transportStop.m_LoadingFactor);
    }

    public static uint CalculateDepartureFrame(
      TransportLine transportLine,
      TransportLineData prefabLineData,
      DynamicBuffer<RouteModifier> routeModifiers,
      float targetStopTime,
      uint lastDepartureFrame,
      uint simulationFrame)
    {
      float num1 = (float) (simulationFrame - lastDepartureFrame) / 60f;
      if ((double) num1 < 0.0)
        return simulationFrame;
      float defaultVehicleInterval = prefabLineData.m_DefaultVehicleInterval;
      RouteUtils.ApplyModifier(ref defaultVehicleInterval, routeModifiers, RouteModifierType.VehicleInterval);
      float vehicleInterval = transportLine.m_VehicleInterval;
      float unbunchingFactor = transportLine.m_UnbunchingFactor;
      float num2 = math.max(math.min(defaultVehicleInterval, (float) (2.0 * (double) vehicleInterval * (double) vehicleInterval / ((double) num1 + (double) vehicleInterval)) - vehicleInterval) * unbunchingFactor + targetStopTime, 1f);
      return simulationFrame + (uint) ((double) num2 * 60.0);
    }

    public static PathMethod GetPathMethods(
      RouteConnectionType routeConnectionType,
      TrackTypes trackTypes,
      RoadTypes roadTypes)
    {
      switch (routeConnectionType)
      {
        case RouteConnectionType.Road:
        case RouteConnectionType.Air:
          return (roadTypes & (RoadTypes.Helicopter | RoadTypes.Airplane)) != RoadTypes.None ? PathMethod.Road | PathMethod.Flying : PathMethod.Road;
        case RouteConnectionType.Pedestrian:
          return PathMethod.Pedestrian;
        case RouteConnectionType.Track:
          return PathMethod.Track;
        default:
          return (PathMethod) 0;
      }
    }

    public static bool CheckOption(Route route, RouteOption option)
    {
      return (route.m_OptionMask & 1U << (int) (option & (RouteOption) 31)) > 0U;
    }

    public static bool HasOption(RouteOptionData optionData, RouteOption option)
    {
      return (optionData.m_OptionMask & 1U << (int) (option & (RouteOption) 31)) > 0U;
    }

    public static void ApplyModifier(
      ref float value,
      DynamicBuffer<RouteModifier> modifiers,
      RouteModifierType type)
    {
      if ((RouteModifierType) modifiers.Length <= type)
        return;
      float2 delta = modifiers[(int) type].m_Delta;
      value += delta.x;
      value += value * delta.y;
    }

    public static PathMethod GetTaxiMethods(Game.Creatures.Resident resident)
    {
      return (resident.m_Flags & ResidentFlags.IgnoreTaxi) != ResidentFlags.None ? (PathMethod) 0 : PathMethod.Taxi;
    }

    public static PathMethod GetPublicTransportMethods(float timeOfDay, float predictionOffset = 0.020833334f)
    {
      timeOfDay = math.frac(timeOfDay + predictionOffset);
      return (double) timeOfDay < 0.25 || (double) timeOfDay >= 0.91666668653488159 ? PathMethod.PublicTransportNight : PathMethod.PublicTransportDay;
    }

    public static PathMethod GetPublicTransportMethods(
      Game.Creatures.Resident resident,
      float timeOfDay,
      float predictionOffset = 0.020833334f)
    {
      if ((resident.m_Flags & ResidentFlags.IgnoreTransport) != ResidentFlags.None)
        return (PathMethod) 0;
      timeOfDay = math.frac(timeOfDay + predictionOffset);
      return (double) timeOfDay < 0.25 || (double) timeOfDay >= 0.91666668653488159 ? PathMethod.PublicTransportNight : PathMethod.PublicTransportDay;
    }

    public static bool CheckVehicleModel(
      VehicleModel vehicleModel,
      PrefabRef prefabRef,
      DynamicBuffer<LayoutElement> layout,
      ref ComponentLookup<PrefabRef> prefabRefData)
    {
      if (vehicleModel.m_PrimaryPrefab != Entity.Null)
      {
        if (prefabRef.m_Prefab != vehicleModel.m_PrimaryPrefab)
          return false;
        if (vehicleModel.m_SecondaryPrefab != Entity.Null)
        {
          if (layout.IsCreated)
          {
            for (int index = 0; index < layout.Length; ++index)
            {
              prefabRef = prefabRefData[layout[index].m_Vehicle];
              if ((Entity) prefabRef == vehicleModel.m_SecondaryPrefab)
                return true;
            }
          }
          return false;
        }
      }
      return true;
    }

    public static int GetMaxTaxiCount(WaitingPassengers waitingPassengers)
    {
      return 3 + (waitingPassengers.m_Count + 3 >> 2);
    }

    public interface ITransportEstimateBuffer
    {
      void AddWaitEstimate(Entity waypoint, int seconds);
    }
  }
}
