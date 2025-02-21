// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.VehicleUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Vehicles
{
  public static class VehicleUtils
  {
    public const float MAX_VEHICLE_SPEED = 277.777771f;
    public const float MAX_CAR_SPEED = 111.111115f;
    public const float MAX_TRAIN_SPEED = 138.888885f;
    public const float MAX_WATERCRAFT_SPEED = 55.5555573f;
    public const float MAX_HELICOPTER_SPEED = 83.3333359f;
    public const float MAX_AIRPLANE_SPEED = 277.777771f;
    public const float MAX_CAR_LENGTH = 20f;
    public const float PARALLEL_PARKING_OFFSET = 1f;
    public const float CAR_CRAWL_SPEED = 3f;
    public const float CAR_AREA_SPEED = 11.1111116f;
    public const float MIN_HIGHWAY_SPEED = 22.2222233f;
    public const float MAX_FIRE_ENGINE_EXTINGUISH_DISTANCE = 30f;
    public const float MAX_POLICE_ACCIDENT_TARGET_DISTANCE = 30f;
    public const float MAX_MAINTENANCE_TARGET_DISTANCE = 30f;
    public const uint MAINTENANCE_DESTROYED_CLEAR_AMOUNT = 500;
    public const float MAX_TRAIN_LENGTH = 200f;
    public const float TRAIN_CRAWL_SPEED = 3f;
    public const float MAX_TRAIN_CARRIAGE_LENGTH = 20f;
    public const float MAX_TRAM_CARRIAGE_LENGTH = 16f;
    public const float MAX_SUBWAY_LENGTH = 200f;
    public const float MAX_SUBWAY_CARRIAGE_LENGTH = 18f;
    public const int CAR_NAVIGATION_LANE_CAPACITY = 8;
    public const int CAR_PARALLEL_LANE_CAPACITY = 8;
    public const int WATERCRAFT_NAVIGATION_LANE_CAPACITY = 8;
    public const int WATERCRAFT_PARALLEL_LANE_CAPACITY = 8;
    public const int AIRCRAFT_NAVIGATION_LANE_CAPACITY = 8;
    public const float MIN_HELICOPTER_NAVIGATION_DISTANCE = 750f;
    public const float MIN_AIRPLANE_NAVIGATION_DISTANCE = 1500f;
    public const float AIRPLANE_FLY_HEIGHT = 1000f;
    public const float HELICOPTER_FLY_HEIGHT = 100f;
    public const float ROCKET_FLY_HEIGHT = 10000f;
    public const uint BOTTLENECK_LIMIT = 50;
    public const uint STUCK_MAX_COUNT = 100;
    public const int STUCK_MAX_SPEED = 6;
    public const float TEMP_WAIT_TIME = 5f;
    public const float DELIVERY_PATHFIND_RANDOM_COST = 30f;
    public const float SERVICE_PATHFIND_RANDOM_COST = 30f;
    public const int PRIORITY_OFFSET = 2;
    public const int NORMAL_CAR_PRIORITY = 100;
    public const int TRACK_RESERVE_PRIORITY = 98;
    public const int REQUEST_SPACE_PRIORITY = 96;
    public const int EMERGENCY_YIELD_PRIORITY = 102;
    public const int NORMAL_TRAIN_PRIORITY = 104;
    public const int EMERGENCY_FLEE_PRIORITY = 106;
    public const int EMERGENCY_CAR_PRIORITY = 108;
    public const int PRIMARY_TRAIN_PRIORITY = 110;
    public const int SMALL_WATERCRAFT_PRIORITY = 100;
    public const int MEDIUM_WATERCRAFT_PRIORITY = 102;
    public const int LARGE_WATERCRAFT_PRIORITY = 104;
    public const int NORMAL_AIRCRAFT_PRIORITY = 104;

    public static bool PathfindFailed(PathOwner pathOwner)
    {
      return (pathOwner.m_State & (PathFlags.Failed | PathFlags.Stuck)) != 0;
    }

    public static bool PathEndReached(CarCurrentLane currentLane)
    {
      return (currentLane.m_LaneFlags & (CarLaneFlags.EndOfPath | CarLaneFlags.EndReached | CarLaneFlags.ParkingSpace | CarLaneFlags.Waypoint)) == (CarLaneFlags.EndOfPath | CarLaneFlags.EndReached);
    }

    public static bool PathEndReached(TrainCurrentLane currentLane)
    {
      return (currentLane.m_Front.m_LaneFlags & (TrainLaneFlags.EndOfPath | TrainLaneFlags.EndReached)) == (TrainLaneFlags.EndOfPath | TrainLaneFlags.EndReached);
    }

    public static bool ReturnEndReached(TrainCurrentLane currentLane)
    {
      return (currentLane.m_Front.m_LaneFlags & (TrainLaneFlags.EndReached | TrainLaneFlags.Return)) == (TrainLaneFlags.EndReached | TrainLaneFlags.Return);
    }

    public static bool PathEndReached(WatercraftCurrentLane currentLane)
    {
      return (currentLane.m_LaneFlags & (WatercraftLaneFlags.EndOfPath | WatercraftLaneFlags.EndReached)) == (WatercraftLaneFlags.EndOfPath | WatercraftLaneFlags.EndReached);
    }

    public static bool PathEndReached(AircraftCurrentLane currentLane)
    {
      return (currentLane.m_LaneFlags & (AircraftLaneFlags.EndOfPath | AircraftLaneFlags.EndReached)) == (AircraftLaneFlags.EndOfPath | AircraftLaneFlags.EndReached);
    }

    public static bool ParkingSpaceReached(CarCurrentLane currentLane, PathOwner pathOwner)
    {
      return (currentLane.m_LaneFlags & (CarLaneFlags.EndReached | CarLaneFlags.ParkingSpace)) == (CarLaneFlags.EndReached | CarLaneFlags.ParkingSpace) && (pathOwner.m_State & PathFlags.Pending) == (PathFlags) 0;
    }

    public static bool ParkingSpaceReached(AircraftCurrentLane currentLane, PathOwner pathOwner)
    {
      return (currentLane.m_LaneFlags & (AircraftLaneFlags.EndReached | AircraftLaneFlags.ParkingSpace)) == (AircraftLaneFlags.EndReached | AircraftLaneFlags.ParkingSpace) && (pathOwner.m_State & PathFlags.Pending) == (PathFlags) 0;
    }

    public static bool WaypointReached(CarCurrentLane currentLane)
    {
      return (currentLane.m_LaneFlags & (CarLaneFlags.EndReached | CarLaneFlags.Waypoint)) == (CarLaneFlags.EndReached | CarLaneFlags.Waypoint);
    }

    public static bool QueueReached(CarCurrentLane currentLane)
    {
      return (currentLane.m_LaneFlags & (CarLaneFlags.Queue | CarLaneFlags.QueueReached)) == (CarLaneFlags.Queue | CarLaneFlags.QueueReached);
    }

    public static bool RequireNewPath(PathOwner pathOwner)
    {
      return (pathOwner.m_State & (PathFlags.Obsolete | PathFlags.DivertObsolete)) != (PathFlags) 0 && (pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Stuck)) == (PathFlags) 0;
    }

    public static bool IsStuck(PathOwner pathOwner) => (pathOwner.m_State & PathFlags.Stuck) != 0;

    public static void SetTarget(ref PathOwner pathOwner, ref Target targetData, Entity newTarget)
    {
      targetData.m_Target = newTarget;
      pathOwner.m_State &= ~PathFlags.Failed;
      pathOwner.m_State |= PathFlags.Obsolete;
    }

    public static void SetupPathfind(
      ref CarCurrentLane currentLane,
      ref PathOwner pathOwner,
      NativeQueue<SetupQueueItem>.ParallelWriter queue,
      SetupQueueItem item)
    {
      if ((pathOwner.m_State & (PathFlags.Obsolete | PathFlags.Divert)) == (PathFlags.Obsolete | PathFlags.Divert))
        pathOwner.m_State |= PathFlags.CachedObsolete;
      pathOwner.m_State &= ~(PathFlags.Failed | PathFlags.Obsolete | PathFlags.DivertObsolete);
      pathOwner.m_State |= PathFlags.Pending;
      currentLane.m_LaneFlags &= ~CarLaneFlags.EndOfPath;
      currentLane.m_LaneFlags |= CarLaneFlags.FixedLane;
      queue.Enqueue(item);
    }

    public static void SetupPathfind(
      ref TrainCurrentLane currentLane,
      ref PathOwner pathOwner,
      NativeQueue<SetupQueueItem>.ParallelWriter queue,
      SetupQueueItem item)
    {
      if ((pathOwner.m_State & (PathFlags.Obsolete | PathFlags.Divert)) == (PathFlags.Obsolete | PathFlags.Divert))
        pathOwner.m_State |= PathFlags.CachedObsolete;
      pathOwner.m_State &= ~(PathFlags.Failed | PathFlags.Obsolete | PathFlags.DivertObsolete);
      pathOwner.m_State |= PathFlags.Pending;
      currentLane.m_Front.m_LaneFlags &= ~TrainLaneFlags.EndOfPath;
      currentLane.m_Rear.m_LaneFlags &= ~TrainLaneFlags.EndOfPath;
      queue.Enqueue(item);
    }

    public static void SetupPathfind(
      ref WatercraftCurrentLane currentLane,
      ref PathOwner pathOwner,
      NativeQueue<SetupQueueItem>.ParallelWriter queue,
      SetupQueueItem item)
    {
      if ((pathOwner.m_State & (PathFlags.Obsolete | PathFlags.Divert)) == (PathFlags.Obsolete | PathFlags.Divert))
        pathOwner.m_State |= PathFlags.CachedObsolete;
      pathOwner.m_State &= ~(PathFlags.Failed | PathFlags.Obsolete | PathFlags.DivertObsolete);
      pathOwner.m_State |= PathFlags.Pending;
      currentLane.m_LaneFlags &= ~WatercraftLaneFlags.EndOfPath;
      currentLane.m_LaneFlags |= WatercraftLaneFlags.FixedLane;
      queue.Enqueue(item);
    }

    public static void SetupPathfind(
      ref AircraftCurrentLane currentLane,
      ref PathOwner pathOwner,
      NativeQueue<SetupQueueItem>.ParallelWriter queue,
      SetupQueueItem item)
    {
      if ((pathOwner.m_State & (PathFlags.Obsolete | PathFlags.Divert)) == (PathFlags.Obsolete | PathFlags.Divert))
        pathOwner.m_State |= PathFlags.CachedObsolete;
      pathOwner.m_State &= ~(PathFlags.Failed | PathFlags.Obsolete | PathFlags.DivertObsolete);
      pathOwner.m_State |= PathFlags.Pending;
      currentLane.m_LaneFlags &= ~AircraftLaneFlags.EndOfPath;
      queue.Enqueue(item);
    }

    public static bool ResetUpdatedPath(ref PathOwner pathOwner)
    {
      int num = (pathOwner.m_State & PathFlags.Updated) != 0 ? 1 : 0;
      pathOwner.m_State &= ~PathFlags.Updated;
      return num != 0;
    }

    public static Transform CalculateParkingSpaceTarget(
      Game.Net.ParkingLane parkingLane,
      ParkingLaneData parkingLaneData,
      ObjectGeometryData prefabGeometryData,
      Curve curve,
      Transform ownerTransform,
      float curvePos)
    {
      Transform parkingSpaceTarget = new Transform();
      float3 forward;
      float3 up;
      VehicleUtils.CalculateParkingSpaceTarget(parkingLane, parkingLaneData, prefabGeometryData, curve, ownerTransform, curvePos, out parkingSpaceTarget.m_Position, out forward, out up);
      parkingSpaceTarget.m_Rotation = quaternion.LookRotationSafe(forward, up);
      return parkingSpaceTarget;
    }

    public static void CalculateParkingSpaceTarget(
      Game.Net.ParkingLane parkingLane,
      ParkingLaneData parkingLaneData,
      ObjectGeometryData prefabGeometryData,
      Curve curve,
      Transform ownerTransform,
      float curvePos,
      out float3 position,
      out float3 forward,
      out float3 up)
    {
      position = MathUtils.Position(curve.m_Bezier, curvePos);
      float3 float3_1 = MathUtils.Tangent(curve.m_Bezier, curvePos);
      float3 float3_2 = new float3();
      float3_2.xz = MathUtils.Right(float3_1.xz);
      if (!ownerTransform.m_Rotation.Equals(new quaternion()))
        float3_2.y -= math.dot(float3_2, math.rotate(ownerTransform.m_Rotation, math.up()));
      float angle = math.select(parkingLaneData.m_SlotAngle, -parkingLaneData.m_SlotAngle, (parkingLane.m_Flags & ParkingLaneFlags.ParkingLeft) != 0);
      up = math.cross(float3_1, float3_2);
      forward = math.rotate(quaternion.AxisAngle(math.normalizesafe(up), angle), float3_1);
      if ((double) parkingLaneData.m_SlotAngle > 0.25)
      {
        float num = math.max(0.0f, MathUtils.Size(prefabGeometryData.m_Bounds.z) - parkingLaneData.m_SlotSize.y);
        position += math.normalizesafe(forward) * ((float) (((double) parkingLaneData.m_SlotSize.y + (double) num) * 0.5) - prefabGeometryData.m_Bounds.max.z);
      }
      else
      {
        float num = math.select(math.select(0.0f, -0.5f, (parkingLane.m_Flags & ParkingLaneFlags.ParkingLeft) != 0), 0.5f, (parkingLane.m_Flags & ParkingLaneFlags.ParkingRight) != 0);
        position += math.normalizesafe(float3_2) * ((parkingLaneData.m_SlotSize.x - prefabGeometryData.m_Size.x) * num);
      }
    }

    public static Transform CalculateTransform(Curve curve, float curvePos)
    {
      return new Transform()
      {
        m_Position = MathUtils.Position(curve.m_Bezier, curvePos),
        m_Rotation = quaternion.LookRotationSafe(MathUtils.Tangent(curve.m_Bezier, curvePos), math.up())
      };
    }

    public static int SetParkingCurvePos(
      Entity entity,
      ref Random random,
      CarCurrentLane currentLane,
      PathOwner pathOwner,
      DynamicBuffer<PathElement> path,
      ref ComponentLookup<ParkedCar> parkedCarData,
      ref ComponentLookup<Unspawned> unspawnedData,
      ref ComponentLookup<Curve> curveData,
      ref ComponentLookup<Game.Net.ParkingLane> parkingLaneData,
      ref ComponentLookup<Game.Net.ConnectionLane> connectionLaneData,
      ref ComponentLookup<PrefabRef> prefabRefData,
      ref ComponentLookup<ObjectGeometryData> prefabObjectGeometryData,
      ref ComponentLookup<ParkingLaneData> prefabParkingLaneData,
      ref BufferLookup<LaneObject> laneObjectData,
      ref BufferLookup<LaneOverlap> laneOverlapData,
      bool ignoreDriveways)
    {
      for (int elementIndex = pathOwner.m_ElementIndex; elementIndex < path.Length; ++elementIndex)
      {
        PathElement pathElement = path[elementIndex];
        if (VehicleUtils.IsParkingLane(pathElement.m_Target, ref parkingLaneData, ref connectionLaneData))
        {
          float curvePos = -1f;
          if (parkingLaneData.HasComponent(pathElement.m_Target))
          {
            float offset;
            float y = VehicleUtils.GetParkingSize(entity, ref prefabRefData, ref prefabObjectGeometryData, out offset).y;
            if (!VehicleUtils.FindFreeParkingSpace(ref random, pathElement.m_Target, pathElement.m_TargetDelta.x, y, offset, ref curvePos, ref parkedCarData, ref curveData, ref unspawnedData, ref parkingLaneData, ref prefabRefData, ref prefabParkingLaneData, ref prefabObjectGeometryData, ref laneObjectData, ref laneOverlapData, ignoreDriveways, false))
              curvePos = random.NextFloat(0.05f, 0.95f);
          }
          else
            curvePos = random.NextFloat(0.05f, 0.95f);
          VehicleUtils.SetParkingCurvePos(path, pathOwner, elementIndex, currentLane.m_Lane, curvePos, ref curveData);
          return elementIndex;
        }
      }
      return path.Length;
    }

    public static void SetParkingCurvePos(
      DynamicBuffer<PathElement> path,
      PathOwner pathOwner,
      int index,
      Entity currentLane,
      float curvePos,
      ref ComponentLookup<Curve> curveData)
    {
      if (index >= pathOwner.m_ElementIndex)
      {
        PathElement pathElement = path[index] with
        {
          m_TargetDelta = (float2) curvePos
        };
        path[index] = pathElement;
        currentLane = pathElement.m_Target;
      }
      Curve componentData;
      if (!curveData.TryGetComponent(currentLane, out componentData))
        return;
      float3 position = MathUtils.Position(componentData.m_Bezier, curvePos);
      if (index > pathOwner.m_ElementIndex)
      {
        PathElement pathElement = path[index - 1];
        if (curveData.TryGetComponent(pathElement.m_Target, out componentData))
        {
          double num = (double) MathUtils.Distance(componentData.m_Bezier, position, out curvePos);
          pathElement.m_TargetDelta.y = curvePos;
          path[index - 1] = pathElement;
        }
      }
      if (index >= path.Length - 1)
        return;
      PathElement pathElement1 = path[index + 1];
      if (!curveData.TryGetComponent(pathElement1.m_Target, out componentData))
        return;
      double num1 = (double) MathUtils.Distance(componentData.m_Bezier, position, out curvePos);
      pathElement1.m_TargetDelta.x = curvePos;
      path[index + 1] = pathElement1;
    }

    public static void ResetParkingLaneStatus(
      Entity entity,
      ref CarCurrentLane currentLane,
      ref PathOwner pathOwner,
      DynamicBuffer<PathElement> path,
      ref EntityStorageInfoLookup entityLookup,
      ref ComponentLookup<Curve> curveData,
      ref ComponentLookup<Game.Net.ParkingLane> parkingLaneData,
      ref ComponentLookup<Game.Net.CarLane> carLaneData,
      ref ComponentLookup<Game.Net.ConnectionLane> connectionLaneData,
      ref ComponentLookup<Game.Objects.SpawnLocation> spawnLocationData,
      ref ComponentLookup<PrefabRef> prefabRefData,
      ref ComponentLookup<Game.Prefabs.SpawnLocationData> prefabSpawnLocationData)
    {
      if (VehicleUtils.IsParkingLane(currentLane.m_Lane, ref parkingLaneData, ref connectionLaneData))
      {
        currentLane.m_LaneFlags |= CarLaneFlags.ParkingSpace;
        bool flag = false;
        while (pathOwner.m_ElementIndex < path.Length)
        {
          PathElement pathElement = path[pathOwner.m_ElementIndex];
          if (VehicleUtils.IsParkingLane(pathElement.m_Target, ref parkingLaneData, ref connectionLaneData))
          {
            VehicleUtils.SetParkingCurvePos(path, pathOwner, pathOwner.m_ElementIndex++, currentLane.m_Lane, currentLane.m_CurvePosition.z, ref curveData);
            flag = true;
          }
          else
          {
            if (!flag)
              VehicleUtils.SetParkingCurvePos(path, pathOwner, pathOwner.m_ElementIndex - 1, currentLane.m_Lane, currentLane.m_CurvePosition.z, ref curveData);
            if (!VehicleUtils.IsCarLane(pathElement.m_Target, ref carLaneData, ref connectionLaneData, ref spawnLocationData, ref prefabRefData, ref prefabSpawnLocationData) && entityLookup.Exists(pathElement.m_Target))
              break;
            currentLane.m_LaneFlags &= ~CarLaneFlags.ParkingSpace;
            break;
          }
        }
      }
      else
      {
        if (!VehicleUtils.IsCarLane(currentLane.m_Lane, ref carLaneData, ref connectionLaneData, ref spawnLocationData, ref prefabRefData, ref prefabSpawnLocationData))
          return;
        currentLane.m_LaneFlags &= ~CarLaneFlags.ParkingSpace;
      }
    }

    public static bool IsParkingLane(
      Entity lane,
      ref ComponentLookup<Game.Net.ParkingLane> parkingLaneData,
      ref ComponentLookup<Game.Net.ConnectionLane> connectionLaneData)
    {
      if (parkingLaneData.HasComponent(lane))
        return true;
      Game.Net.ConnectionLane componentData;
      return connectionLaneData.TryGetComponent(lane, out componentData) && (componentData.m_Flags & ConnectionLaneFlags.Parking) != 0;
    }

    public static bool IsCarLane(
      Entity lane,
      ref ComponentLookup<Game.Net.CarLane> carLaneData,
      ref ComponentLookup<Game.Net.ConnectionLane> connectionLaneData,
      ref ComponentLookup<Game.Objects.SpawnLocation> spawnLocationData,
      ref ComponentLookup<PrefabRef> prefabRefData,
      ref ComponentLookup<Game.Prefabs.SpawnLocationData> prefabSpawnLocationData)
    {
      if (carLaneData.HasComponent(lane))
        return true;
      Game.Net.ConnectionLane componentData1;
      if (connectionLaneData.TryGetComponent(lane, out componentData1))
        return (componentData1.m_Flags & ConnectionLaneFlags.Road) != 0;
      if (spawnLocationData.HasComponent(lane))
      {
        PrefabRef prefabRef = prefabRefData[lane];
        Game.Prefabs.SpawnLocationData componentData2;
        if (prefabSpawnLocationData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
          return componentData2.m_ConnectionType == RouteConnectionType.Road || componentData2.m_ConnectionType == RouteConnectionType.Parking;
      }
      return false;
    }

    public static void SetParkingCurvePos(
      DynamicBuffer<PathElement> path,
      PathOwner pathOwner,
      ref CarCurrentLane currentLaneData,
      DynamicBuffer<CarNavigationLane> navLanes,
      int navIndex,
      float curvePos,
      ref ComponentLookup<Curve> curveData)
    {
      Entity lane = currentLaneData.m_Lane;
      if (navIndex >= 0)
      {
        CarNavigationLane navLane = navLanes[navIndex] with
        {
          m_CurvePosition = (float2) curvePos
        };
        navLanes[navIndex] = navLane;
        lane = navLane.m_Lane;
      }
      if (!curveData.HasComponent(lane))
        return;
      float3 position = MathUtils.Position(curveData[lane].m_Bezier, curvePos);
      if (navIndex > 0)
      {
        CarNavigationLane navLane = navLanes[navIndex - 1];
        if (curveData.HasComponent(navLane.m_Lane))
        {
          double num = (double) MathUtils.Distance(curveData[navLane.m_Lane].m_Bezier, position, out curvePos);
          navLane.m_CurvePosition.y = curvePos;
          navLanes[navIndex - 1] = navLane;
        }
      }
      else if (navIndex == 0 && curveData.HasComponent(currentLaneData.m_Lane))
      {
        double num = (double) MathUtils.Distance(curveData[currentLaneData.m_Lane].m_Bezier, position, out curvePos);
        currentLaneData.m_CurvePosition.z = curvePos;
      }
      if (navIndex < navLanes.Length - 1)
      {
        CarNavigationLane navLane = navLanes[navIndex + 1];
        if (!curveData.HasComponent(navLane.m_Lane))
          return;
        double num = (double) MathUtils.Distance(curveData[navLane.m_Lane].m_Bezier, position, out curvePos);
        navLane.m_CurvePosition.x = curvePos;
        navLanes[navIndex + 1] = navLane;
      }
      else
      {
        if (navIndex != navLanes.Length - 1 || path.Length <= pathOwner.m_ElementIndex)
          return;
        PathElement pathElement = path[pathOwner.m_ElementIndex];
        if (!curveData.HasComponent(pathElement.m_Target))
          return;
        double num = (double) MathUtils.Distance(curveData[pathElement.m_Target].m_Bezier, position, out curvePos);
        pathElement.m_TargetDelta.x = curvePos;
        path[pathOwner.m_ElementIndex] = pathElement;
      }
    }

    public static void CalculateTrainNavigationPivots(
      Transform transform,
      TrainData prefabTrainData,
      out float3 pivot1,
      out float3 pivot2)
    {
      float3 float3 = math.forward(transform.m_Rotation);
      pivot1 = transform.m_Position + float3 * prefabTrainData.m_BogieOffsets.x;
      pivot2 = transform.m_Position - float3 * prefabTrainData.m_BogieOffsets.y;
    }

    public static void CalculateShipNavigationPivots(
      Transform transform,
      ObjectGeometryData prefabGeometryData,
      out float3 pivot1,
      out float3 pivot2)
    {
      float3 float3 = math.forward(transform.m_Rotation) * math.max(1f, (float) (((double) prefabGeometryData.m_Size.z - (double) prefabGeometryData.m_Size.x) * 0.5));
      pivot1 = transform.m_Position + float3;
      pivot2 = transform.m_Position - float3;
    }

    public static bool CalculateTransformPosition(
      ref float3 position,
      Entity entity,
      ComponentLookup<Transform> transforms,
      ComponentLookup<Position> positions,
      ComponentLookup<PrefabRef> prefabRefs,
      ComponentLookup<BuildingData> prefabBuildingDatas)
    {
      if (transforms.HasComponent(entity))
      {
        Transform transform = transforms[entity];
        PrefabRef prefabRef = prefabRefs[entity];
        if (prefabBuildingDatas.HasComponent(prefabRef.m_Prefab))
        {
          BuildingData prefabBuildingData = prefabBuildingDatas[prefabRef.m_Prefab];
          position = BuildingUtils.CalculateFrontPosition(transform, prefabBuildingData.m_LotSize.y);
          return true;
        }
        position = transform.m_Position;
        return true;
      }
      if (!positions.HasComponent(entity))
        return false;
      position = positions[entity].m_Position;
      return true;
    }

    public static float GetNavigationSize(ObjectGeometryData prefabObjectGeometryData)
    {
      return (float) ((double) prefabObjectGeometryData.m_Bounds.max.x - (double) prefabObjectGeometryData.m_Bounds.min.x + 2.0);
    }

    public static float GetMaxDriveSpeed(CarData prefabCarData, Game.Net.CarLane carLaneData)
    {
      return VehicleUtils.GetMaxDriveSpeed(prefabCarData, carLaneData.m_SpeedLimit, carLaneData.m_Curviness);
    }

    public static float GetMaxDriveSpeed(CarData prefabCarData, float speedLimit, float curviness)
    {
      float y = math.max(1f, prefabCarData.m_Turning.x * prefabCarData.m_MaxSpeed / math.max(1E-06f, curviness * prefabCarData.m_MaxSpeed + prefabCarData.m_Turning.x - prefabCarData.m_Turning.y));
      return math.min(speedLimit, y);
    }

    public static void ModifyDriveSpeed(ref float driveSpeed, LaneCondition condition)
    {
      float num = math.saturate((float) (((double) condition.m_Wear - 2.5) * 0.13333334028720856));
      driveSpeed *= (float) (1.0 - (double) num * (double) num * 0.5);
    }

    public static float GetMaxBrakingSpeed(CarData prefabCarData, float distance, float timeStep)
    {
      float num = timeStep * prefabCarData.m_Braking;
      return math.max(0.0f, math.sqrt(math.max(0.0f, (float) ((double) num * (double) num + 2.0 * (double) prefabCarData.m_Braking * (double) distance))) - num);
    }

    public static float GetMaxBrakingSpeed(
      CarData prefabCarData,
      float distance,
      float maxResultSpeed,
      float timeStep)
    {
      float num = timeStep * prefabCarData.m_Braking;
      return math.max(0.0f, math.sqrt(math.max(0.0f, (float) ((double) num * (double) num + 2.0 * (double) prefabCarData.m_Braking * (double) distance + (double) maxResultSpeed * (double) maxResultSpeed))) - num);
    }

    public static float GetBrakingDistance(CarData prefabCarData, float speed, float timeStep)
    {
      return (float) (0.5 * (double) speed * (double) speed / (double) prefabCarData.m_Braking + (double) speed * (double) timeStep);
    }

    public static Bounds1 CalculateSpeedRange(
      CarData prefabCarData,
      float currentSpeed,
      float timeStep)
    {
      float y = MathUtils.InverseSmoothStep(prefabCarData.m_MaxSpeed, 0.0f, currentSpeed) * prefabCarData.m_Acceleration;
      float2 float2 = currentSpeed + new float2(-prefabCarData.m_Braking, y) * timeStep;
      float2.x = math.max(0.0f, float2.x);
      float2.y = math.min(float2.y, math.max(float2.x, prefabCarData.m_MaxSpeed));
      return new Bounds1(float2.x, float2.y);
    }

    public static int GetPriority(Car carData)
    {
      return math.select(100, 108, (carData.m_Flags & CarFlags.Emergency) > (CarFlags) 0);
    }

    public static Game.Net.CarLaneFlags GetForbiddenLaneFlags(Car carData)
    {
      return (carData.m_Flags & CarFlags.UsePublicTransportLanes) == (CarFlags) 0 ? Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Forbidden : Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.Forbidden;
    }

    public static Game.Net.CarLaneFlags GetPreferredLaneFlags(Car carData)
    {
      return (carData.m_Flags & CarFlags.PreferPublicTransportLanes) == (CarFlags) 0 ? ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) : Game.Net.CarLaneFlags.PublicOnly;
    }

    public static float GetSpeedLimitFactor(Car carData)
    {
      return math.select(1f, 2f, (carData.m_Flags & CarFlags.Emergency) > (CarFlags) 0);
    }

    public static void GetDrivingStyle(
      uint simulationFrame,
      PseudoRandomSeed randomSeed,
      out float safetyTime)
    {
      Random random = randomSeed.GetRandom((uint) PseudoRandomSeed.kDrivingStyle);
      float x = (float) (simulationFrame & 4095U) * 0.00153398083f + random.NextFloat(6.28318548f);
      safetyTime = (float) (0.30000001192092896 + 0.20000000298023224 * (double) math.sin(x));
    }

    public static float GetMaxDriveSpeed(TrainData prefabTrainData, Game.Net.TrackLane trackLaneData)
    {
      return VehicleUtils.GetMaxDriveSpeed(prefabTrainData, trackLaneData.m_SpeedLimit, trackLaneData.m_Curviness);
    }

    public static float GetMaxDriveSpeed(
      TrainData prefabTrainData,
      float speedLimit,
      float curviness)
    {
      float y = math.max(1f, prefabTrainData.m_Turning.x * prefabTrainData.m_MaxSpeed / math.max(1E-06f, curviness * prefabTrainData.m_MaxSpeed + prefabTrainData.m_Turning.x - prefabTrainData.m_Turning.y));
      return math.min(speedLimit, y);
    }

    public static float GetMaxBrakingSpeed(
      TrainData prefabTrainData,
      float distance,
      float timeStep)
    {
      float num = timeStep * prefabTrainData.m_Braking;
      return math.max(0.0f, math.sqrt(math.max(0.0f, (float) ((double) num * (double) num + 2.0 * (double) prefabTrainData.m_Braking * (double) distance))) - num);
    }

    public static float GetMaxBrakingSpeed(
      TrainData prefabTrainData,
      float distance,
      float maxResultSpeed,
      float timeStep)
    {
      float num = timeStep * prefabTrainData.m_Braking;
      return math.max(0.0f, math.sqrt(math.max(0.0f, (float) ((double) num * (double) num + 2.0 * (double) prefabTrainData.m_Braking * (double) distance + (double) maxResultSpeed * (double) maxResultSpeed))) - num);
    }

    public static float GetBrakingDistance(TrainData prefabTrainData, float speed, float timeStep)
    {
      return (float) (0.5 * (double) speed * (double) speed / (double) prefabTrainData.m_Braking + (double) speed * (double) timeStep);
    }

    public static float GetSignalDistance(TrainData prefabTrainData, float speed)
    {
      return math.select(0.0f, speed * 4f, (prefabTrainData.m_TrackType & (TrackTypes.Train | TrackTypes.Subway)) != 0);
    }

    public static Bounds1 CalculateSpeedRange(
      TrainData prefabTrainData,
      float currentSpeed,
      float timeStep)
    {
      float y = MathUtils.InverseSmoothStep(prefabTrainData.m_MaxSpeed, 0.0f, currentSpeed) * prefabTrainData.m_Acceleration;
      float2 float2 = currentSpeed + new float2(-prefabTrainData.m_Braking, y) * timeStep;
      float2.x = math.max(0.0f, float2.x);
      float2.y = math.min(float2.y, math.max(float2.x, prefabTrainData.m_MaxSpeed));
      return new Bounds1(float2.x, float2.y);
    }

    public static int GetPriority(TrainData trainData)
    {
      return math.select(104, 110, (trainData.m_TrackType & (TrackTypes.Train | TrackTypes.Subway)) != 0);
    }

    public static float GetMaxDriveSpeed(WatercraftData prefabWatercraftData, Game.Net.CarLane carLaneData)
    {
      return VehicleUtils.GetMaxDriveSpeed(prefabWatercraftData, carLaneData.m_SpeedLimit, carLaneData.m_Curviness);
    }

    public static float GetMaxDriveSpeed(
      WatercraftData prefabWatercraftData,
      float speedLimit,
      float curviness)
    {
      float y = math.max(1f, prefabWatercraftData.m_Turning.x * prefabWatercraftData.m_MaxSpeed / math.max(1E-06f, curviness * prefabWatercraftData.m_MaxSpeed + prefabWatercraftData.m_Turning.x - prefabWatercraftData.m_Turning.y));
      return math.min(speedLimit, y);
    }

    public static float GetMaxBrakingSpeed(
      WatercraftData prefabWatercraftData,
      float distance,
      float timeStep)
    {
      float num = timeStep * prefabWatercraftData.m_Braking;
      return math.max(0.0f, math.sqrt(math.max(0.0f, (float) ((double) num * (double) num + 2.0 * (double) prefabWatercraftData.m_Braking * (double) distance))) - num);
    }

    public static float GetMaxBrakingSpeed(
      WatercraftData prefabWatercraftData,
      float distance,
      float maxResultSpeed,
      float timeStep)
    {
      float num = timeStep * prefabWatercraftData.m_Braking;
      return math.max(0.0f, math.sqrt(math.max(0.0f, (float) ((double) num * (double) num + 2.0 * (double) prefabWatercraftData.m_Braking * (double) distance + (double) maxResultSpeed * (double) maxResultSpeed))) - num);
    }

    public static float GetBrakingDistance(
      WatercraftData prefabWatercraftData,
      float speed,
      float timeStep)
    {
      return (float) (0.5 * (double) speed * (double) speed / (double) prefabWatercraftData.m_Braking + (double) speed * (double) timeStep);
    }

    public static Bounds1 CalculateSpeedRange(
      WatercraftData prefabWatercraftData,
      float currentSpeed,
      float timeStep)
    {
      float y = MathUtils.InverseSmoothStep(prefabWatercraftData.m_MaxSpeed, 0.0f, currentSpeed) * prefabWatercraftData.m_Acceleration;
      float2 float2 = currentSpeed + new float2(-prefabWatercraftData.m_Braking, y) * timeStep;
      float2.y = math.min(float2.y, math.max(float2.x, prefabWatercraftData.m_MaxSpeed));
      return new Bounds1(float2.x, float2.y);
    }

    public static int GetPriority(WatercraftData prefabWatercraftData)
    {
      switch (prefabWatercraftData.m_SizeClass)
      {
        case SizeClass.Small:
          return 100;
        case SizeClass.Medium:
          return 102;
        case SizeClass.Large:
          return 104;
        default:
          return 100;
      }
    }

    public static float GetMaxDriveSpeed(AircraftData prefabAircraftData, Game.Net.CarLane carLaneData)
    {
      return VehicleUtils.GetMaxDriveSpeed(prefabAircraftData, carLaneData.m_SpeedLimit, carLaneData.m_Curviness);
    }

    public static float GetMaxDriveSpeed(
      AircraftData prefabAircraftData,
      float speedLimit,
      float curviness)
    {
      float y = math.max(1f, prefabAircraftData.m_GroundTurning.x * prefabAircraftData.m_GroundMaxSpeed / math.max(1E-06f, curviness * prefabAircraftData.m_GroundMaxSpeed + prefabAircraftData.m_GroundTurning.x - prefabAircraftData.m_GroundTurning.y));
      return math.min(speedLimit, y);
    }

    public static float GetMaxBrakingSpeed(
      AircraftData prefabAircraftData,
      float distance,
      float timeStep)
    {
      float num = timeStep * prefabAircraftData.m_GroundBraking;
      return math.max(0.0f, math.sqrt(math.max(0.0f, (float) ((double) num * (double) num + 2.0 * (double) prefabAircraftData.m_GroundBraking * (double) distance))) - num);
    }

    public static float GetMaxBrakingSpeed(
      HelicopterData prefabHelicopterData,
      float distance,
      float timeStep)
    {
      float num = timeStep * prefabHelicopterData.m_FlyingAcceleration;
      return math.max(0.0f, math.sqrt(math.max(0.0f, (float) ((double) num * (double) num + 2.0 * (double) prefabHelicopterData.m_FlyingAcceleration * (double) distance))) - num);
    }

    public static float GetMaxBrakingSpeed(
      AirplaneData prefabAirplaneData,
      float distance,
      float maxResultSpeed,
      float timeStep)
    {
      float num = timeStep * prefabAirplaneData.m_FlyingBraking;
      return math.max(0.0f, math.sqrt(math.max(0.0f, (float) ((double) num * (double) num + 2.0 * (double) prefabAirplaneData.m_FlyingBraking * (double) distance + (double) maxResultSpeed * (double) maxResultSpeed))) - num);
    }

    public static float GetMaxBrakingSpeed(
      AircraftData prefabAircraftData,
      float distance,
      float maxResultSpeed,
      float timeStep)
    {
      float num = timeStep * prefabAircraftData.m_GroundBraking;
      return math.max(0.0f, math.sqrt(math.max(0.0f, (float) ((double) num * (double) num + 2.0 * (double) prefabAircraftData.m_GroundBraking * (double) distance + (double) maxResultSpeed * (double) maxResultSpeed))) - num);
    }

    public static float GetBrakingDistance(
      AircraftData prefabAircraftData,
      float speed,
      float timeStep)
    {
      return (float) (0.5 * (double) speed * (double) speed / (double) prefabAircraftData.m_GroundBraking + (double) speed * (double) timeStep);
    }

    public static float GetBrakingDistance(
      HelicopterData prefabHelicopterData,
      float speed,
      float timeStep)
    {
      return (float) (0.5 * (double) speed * (double) speed / (double) prefabHelicopterData.m_FlyingAcceleration + (double) speed * (double) timeStep);
    }

    public static float GetBrakingDistance(
      AirplaneData prefabAirplaneData,
      float speed,
      float timeStep)
    {
      return (float) (0.5 * (double) speed * (double) speed / (double) prefabAirplaneData.m_FlyingBraking + (double) speed * (double) timeStep);
    }

    public static Bounds1 CalculateSpeedRange(
      AircraftData prefabAircraftData,
      float currentSpeed,
      float timeStep)
    {
      float y = MathUtils.InverseSmoothStep(prefabAircraftData.m_GroundMaxSpeed, 0.0f, currentSpeed) * prefabAircraftData.m_GroundAcceleration;
      float2 float2 = currentSpeed + new float2(-prefabAircraftData.m_GroundBraking, y) * timeStep;
      float2.x = math.max(0.0f, float2.x);
      float2.y = math.min(float2.y, math.max(float2.x, prefabAircraftData.m_GroundMaxSpeed));
      return new Bounds1(float2.x, float2.y);
    }

    public static Bounds1 CalculateSpeedRange(
      HelicopterData prefabHelicopterData,
      float currentSpeed,
      float timeStep)
    {
      float y = MathUtils.InverseSmoothStep(prefabHelicopterData.m_FlyingMaxSpeed, 0.0f, currentSpeed) * prefabHelicopterData.m_FlyingAcceleration;
      float2 float2 = currentSpeed + new float2(-prefabHelicopterData.m_FlyingAcceleration, y) * timeStep;
      float2.x = math.max(0.0f, float2.x);
      float2.y = math.min(float2.y, math.max(float2.x, prefabHelicopterData.m_FlyingMaxSpeed));
      return new Bounds1(float2.x, float2.y);
    }

    public static Bounds1 CalculateSpeedRange(
      AirplaneData prefabAirplaneData,
      float currentSpeed,
      float timeStep)
    {
      float y = MathUtils.InverseSmoothStep(prefabAirplaneData.m_FlyingSpeed.y, 0.0f, currentSpeed) * prefabAirplaneData.m_FlyingAcceleration;
      float2 float2 = currentSpeed + new float2(-prefabAirplaneData.m_FlyingBraking, y) * timeStep;
      float2 = new float2(math.max(float2.x, math.min(float2.y, prefabAirplaneData.m_FlyingSpeed.x)), math.min(float2.y, math.max(float2.x, prefabAirplaneData.m_FlyingSpeed.y)));
      return new Bounds1(float2.x, float2.y);
    }

    public static int GetPriority(AircraftData prefabAircraftData) => 104;

    public static void DeleteVehicle(
      EntityCommandBuffer commandBuffer,
      Entity vehicle,
      DynamicBuffer<LayoutElement> layout)
    {
      if (layout.IsCreated && layout.Length != 0)
      {
        for (int index = 0; index < layout.Length; ++index)
          commandBuffer.AddComponent<Deleted>(layout[index].m_Vehicle, new Deleted());
      }
      else
        commandBuffer.AddComponent<Deleted>(vehicle, new Deleted());
    }

    public static void DeleteVehicle(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      Entity vehicle,
      DynamicBuffer<LayoutElement> layout)
    {
      if (layout.IsCreated && layout.Length != 0)
      {
        for (int index = 0; index < layout.Length; ++index)
          commandBuffer.AddComponent<Deleted>(jobIndex, layout[index].m_Vehicle, new Deleted());
      }
      else
        commandBuffer.AddComponent<Deleted>(jobIndex, vehicle, new Deleted());
    }

    public static float2 GetParkingSize(ParkingLaneData parkingLaneData)
    {
      return math.select(new float2(parkingLaneData.m_SlotSize.x, parkingLaneData.m_MaxCarLength), new float2(parkingLaneData.m_SlotSize.x * 1.25f, 1000000f), new bool2((double) parkingLaneData.m_SlotAngle < 0.0099999997764825821, (double) parkingLaneData.m_MaxCarLength == 0.0));
    }

    public static Entity GetParkingSource(
      Entity entity,
      CarCurrentLane currentLane,
      ref ComponentLookup<Game.Net.ParkingLane> parkingLaneData,
      ref ComponentLookup<Game.Net.ConnectionLane> connectionLaneData)
    {
      Game.Net.ConnectionLane componentData;
      return parkingLaneData.HasComponent(currentLane.m_Lane) || connectionLaneData.TryGetComponent(currentLane.m_Lane, out componentData) && (componentData.m_Flags & ConnectionLaneFlags.Parking) != (ConnectionLaneFlags) 0 ? currentLane.m_Lane : entity;
    }

    public static float2 GetParkingSize(
      Entity car,
      ref ComponentLookup<PrefabRef> prefabRefData,
      ref ComponentLookup<ObjectGeometryData> objectGeometryData)
    {
      return VehicleUtils.GetParkingSize(car, ref prefabRefData, ref objectGeometryData, out float _);
    }

    public static float2 GetParkingSize(
      Entity car,
      ref ComponentLookup<PrefabRef> prefabRefData,
      ref ComponentLookup<ObjectGeometryData> objectGeometryData,
      out float offset)
    {
      PrefabRef componentData1;
      ObjectGeometryData componentData2;
      if (prefabRefData.TryGetComponent(car, out componentData1) && objectGeometryData.TryGetComponent(componentData1.m_Prefab, out componentData2))
        return VehicleUtils.GetParkingSize(componentData2, out offset);
      offset = 0.0f;
      return (float2) 0.0f;
    }

    public static float2 GetParkingSize(ObjectGeometryData objectGeometry, out float offset)
    {
      offset = -MathUtils.Center(objectGeometry.m_Bounds.z);
      return math.max(new float2(0.01f, 1.01f), MathUtils.Size(objectGeometry.m_Bounds.xz));
    }

    public static float2 GetParkingOffsets(
      Entity car,
      ref ComponentLookup<PrefabRef> prefabRefData,
      ref ComponentLookup<ObjectGeometryData> objectGeometryData)
    {
      PrefabRef componentData1;
      ObjectGeometryData componentData2;
      return prefabRefData.TryGetComponent(car, out componentData1) && objectGeometryData.TryGetComponent(componentData1.m_Prefab, out componentData2) ? math.max((float2) 0.1f, new float2(-componentData2.m_Bounds.min.z, componentData2.m_Bounds.max.z)) : (float2) 0.0f;
    }

    public static RuleFlags GetIgnoredPathfindRules(CarData carData)
    {
      RuleFlags ignoredPathfindRules = (RuleFlags) 0;
      if (carData.m_SizeClass < SizeClass.Large)
        ignoredPathfindRules |= RuleFlags.ForbidHeavyTraffic;
      if (carData.m_EnergyType == EnergyTypes.Electricity)
        ignoredPathfindRules |= RuleFlags.ForbidCombustionEngines;
      if ((double) carData.m_MaxSpeed >= 22.222223281860352)
        ignoredPathfindRules |= RuleFlags.ForbidSlowTraffic;
      return ignoredPathfindRules;
    }

    public static RuleFlags GetIgnoredPathfindRulesTaxiDefaults()
    {
      return RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic;
    }

    public static bool IsReversedPath(
      DynamicBuffer<PathElement> path,
      PathOwner pathOwner,
      Entity vehicle,
      DynamicBuffer<LayoutElement> layout,
      ComponentLookup<Curve> curveData,
      ComponentLookup<TrainCurrentLane> currentLaneData,
      ComponentLookup<Train> trainData,
      ComponentLookup<Transform> transformData)
    {
      if (path.Length <= pathOwner.m_ElementIndex)
        return false;
      PathElement pathElement = path[pathOwner.m_ElementIndex];
      if (!curveData.HasComponent(pathElement.m_Target))
        return false;
      float3 x = MathUtils.Position(curveData[pathElement.m_Target].m_Bezier, pathElement.m_TargetDelta.x);
      Entity entity1 = vehicle;
      Entity entity2 = vehicle;
      if (layout.Length != 0)
      {
        entity1 = layout[0].m_Vehicle;
        entity2 = layout[layout.Length - 1].m_Vehicle;
      }
      TrainCurrentLane trainCurrentLane1 = currentLaneData[entity1];
      TrainCurrentLane trainCurrentLane2 = currentLaneData[entity2];
      float3 y1;
      float3 y2;
      if ((trainCurrentLane1.m_Front.m_Lane != trainCurrentLane2.m_Rear.m_Lane || (double) trainCurrentLane1.m_Front.m_CurvePosition.w != (double) trainCurrentLane2.m_Rear.m_CurvePosition.y) && curveData.HasComponent(trainCurrentLane1.m_Front.m_Lane) && curveData.HasComponent(trainCurrentLane2.m_Rear.m_Lane))
      {
        Curve curve1 = curveData[trainCurrentLane1.m_Front.m_Lane];
        Curve curve2 = curveData[trainCurrentLane2.m_Rear.m_Lane];
        y1 = MathUtils.Position(curve1.m_Bezier, trainCurrentLane1.m_Front.m_CurvePosition.w);
        y2 = MathUtils.Position(curve2.m_Bezier, trainCurrentLane2.m_Rear.m_CurvePosition.y);
      }
      else
      {
        Train train1 = trainData[entity1];
        Train train2 = trainData[entity2];
        Transform transform1 = transformData[entity1];
        Transform transform2 = transformData[entity2];
        float3 a1 = math.forward(transform1.m_Rotation);
        float3 a2 = math.forward(transform2.m_Rotation);
        float3 float3_1 = math.select(a1, -a1, (train1.m_Flags & TrainFlags.Reversed) > (TrainFlags) 0);
        float3 float3_2 = math.select(a2, -a2, (train2.m_Flags & TrainFlags.Reversed) > (TrainFlags) 0);
        y1 = transform1.m_Position + float3_1;
        y2 = transform2.m_Position - float3_2;
      }
      return (double) math.distancesq(x, y1) > (double) math.distancesq(x, y2);
    }

    public static void ReverseTrain(
      Entity vehicle,
      DynamicBuffer<LayoutElement> layout,
      ref ComponentLookup<Train> trainData,
      ref ComponentLookup<TrainCurrentLane> currentLaneData,
      ref ComponentLookup<TrainNavigation> navigationData)
    {
      if (layout.Length != 0)
      {
        TrainCurrentLane trainCurrentLane1 = currentLaneData[layout[0].m_Vehicle];
        TrainCurrentLane trainCurrentLane2 = currentLaneData[layout[layout.Length - 1].m_Vehicle];
        TrainBogieCache rearCache = new TrainBogieCache(trainCurrentLane1.m_Front);
        TrainFlags trainFlags = trainData[layout[0].m_Vehicle].m_Flags & TrainFlags.IgnoreParkedVehicle;
        for (int index = 0; index < layout.Length; ++index)
          VehicleUtils.ReverseCarriage(layout[index].m_Vehicle, trainCurrentLane2.m_Rear.m_Lane, trainCurrentLane2.m_Rear.m_CurvePosition.y, ref trainData, ref currentLaneData, ref navigationData, ref rearCache);
        CollectionUtils.Reverse<LayoutElement>(layout.AsNativeArray());
        Train train = trainData[layout[0].m_Vehicle];
        train.m_Flags |= trainFlags;
        trainData[layout[0].m_Vehicle] = train;
      }
      else
      {
        TrainCurrentLane trainCurrentLane = currentLaneData[vehicle];
        TrainBogieCache rearCache = new TrainBogieCache(trainCurrentLane.m_Front);
        VehicleUtils.ReverseCarriage(vehicle, trainCurrentLane.m_Rear.m_Lane, trainCurrentLane.m_Rear.m_CurvePosition.y, ref trainData, ref currentLaneData, ref navigationData, ref rearCache);
      }
    }

    public static void ReverseCarriage(
      Entity vehicle,
      Entity lastLane,
      float lastCurvePos,
      ref ComponentLookup<Train> trainData,
      ref ComponentLookup<TrainCurrentLane> currentLaneData,
      ref ComponentLookup<TrainNavigation> navigationData,
      ref TrainBogieCache rearCache)
    {
      Train train = trainData[vehicle];
      TrainCurrentLane trainCurrentLane = currentLaneData[vehicle];
      TrainNavigation trainNavigation = navigationData[vehicle];
      train.m_Flags &= ~TrainFlags.IgnoreParkedVehicle;
      train.m_Flags ^= TrainFlags.Reversed;
      CommonUtils.Swap<TrainBogieLane>(ref trainCurrentLane.m_Front, ref trainCurrentLane.m_Rear);
      CommonUtils.Swap<TrainBogieCache>(ref trainCurrentLane.m_RearCache, ref rearCache);
      trainCurrentLane.m_Front.m_CurvePosition = trainCurrentLane.m_Front.m_CurvePosition.wyyx;
      trainCurrentLane.m_FrontCache.m_CurvePosition = trainCurrentLane.m_FrontCache.m_CurvePosition.yx;
      trainCurrentLane.m_Rear.m_CurvePosition = trainCurrentLane.m_Rear.m_CurvePosition.wyyx;
      trainCurrentLane.m_RearCache.m_CurvePosition = trainCurrentLane.m_RearCache.m_CurvePosition.yx;
      if (trainCurrentLane.m_Front.m_Lane == lastLane)
        trainCurrentLane.m_Front.m_CurvePosition.w = lastCurvePos;
      if (trainCurrentLane.m_FrontCache.m_Lane == lastLane)
        trainCurrentLane.m_FrontCache.m_CurvePosition.y = lastCurvePos;
      if (trainCurrentLane.m_Rear.m_Lane == lastLane)
        trainCurrentLane.m_Rear.m_CurvePosition.w = lastCurvePos;
      if (trainCurrentLane.m_RearCache.m_Lane == lastLane)
        trainCurrentLane.m_RearCache.m_CurvePosition.y = lastCurvePos;
      CommonUtils.Swap<TrainBogiePosition>(ref trainNavigation.m_Front, ref trainNavigation.m_Rear);
      trainNavigation.m_Front.m_Direction = -trainNavigation.m_Front.m_Direction;
      trainNavigation.m_Rear.m_Direction = -trainNavigation.m_Rear.m_Direction;
      trainCurrentLane.m_Front.m_LaneFlags &= ~TrainLaneFlags.Return;
      trainCurrentLane.m_FrontCache.m_LaneFlags &= ~TrainLaneFlags.Return;
      trainCurrentLane.m_Rear.m_LaneFlags &= ~TrainLaneFlags.Return;
      trainCurrentLane.m_RearCache.m_LaneFlags &= ~TrainLaneFlags.Return;
      trainData[vehicle] = train;
      currentLaneData[vehicle] = trainCurrentLane;
      navigationData[vehicle] = trainNavigation;
    }

    public static float CalculateLength(
      Entity vehicle,
      DynamicBuffer<LayoutElement> layout,
      ref ComponentLookup<PrefabRef> prefabRefData,
      ref ComponentLookup<TrainData> prefabTrainData)
    {
      if (layout.Length == 0)
        return math.csum(prefabTrainData[prefabRefData[vehicle].m_Prefab].m_AttachOffsets);
      float length = 0.0f;
      for (int index = 0; index < layout.Length; ++index)
      {
        TrainData trainData = prefabTrainData[prefabRefData[layout[index].m_Vehicle].m_Prefab];
        length += math.csum(trainData.m_AttachOffsets);
      }
      return length;
    }

    public static void UpdateCarriageLocations(
      DynamicBuffer<LayoutElement> layout,
      NativeList<PathElement> laneBuffer,
      ref ComponentLookup<Train> trainData,
      ref ComponentLookup<ParkedTrain> parkedTrainData,
      ref ComponentLookup<TrainCurrentLane> currentLaneData,
      ref ComponentLookup<TrainNavigation> navigationData,
      ref ComponentLookup<Transform> transformData,
      ref ComponentLookup<Curve> curveData,
      ref ComponentLookup<Game.Net.ConnectionLane> connectionLaneData,
      ref ComponentLookup<PrefabRef> prefabRefData,
      ref ComponentLookup<TrainData> prefabTrainData)
    {
      if (laneBuffer.Length == 0)
        return;
      int num1 = 0;
      ref NativeList<PathElement> local = ref laneBuffer;
      int index1 = num1;
      int num2 = index1 + 1;
      PathElement pathElement = local[index1];
      float y = pathElement.m_TargetDelta.y;
      float3 float3 = MathUtils.Position(curveData[pathElement.m_Target].m_Bezier, pathElement.m_TargetDelta.y);
      float num3 = 0.0f;
      for (int index2 = 0; index2 < layout.Length; ++index2)
      {
        Entity vehicle = layout[index2].m_Vehicle;
        Train train = trainData[vehicle];
        PrefabRef prefabRef = prefabRefData[vehicle];
        TrainData trainData1 = prefabTrainData[prefabRef.m_Prefab];
        bool c = (train.m_Flags & TrainFlags.Reversed) > (TrainFlags) 0;
        if (c)
        {
          trainData1.m_BogieOffsets = trainData1.m_BogieOffsets.yx;
          trainData1.m_AttachOffsets = trainData1.m_AttachOffsets.yx;
        }
        TrainCurrentLane trainCurrentLane = new TrainCurrentLane();
        TrainNavigation trainNavigation = new TrainNavigation();
        float num4 = num3 + (trainData1.m_AttachOffsets.x - trainData1.m_BogieOffsets.x);
        while (true)
        {
          Curve curve = curveData[pathElement.m_Target];
          if (!VehicleUtils.MoveBufferPosition(float3, ref trainNavigation.m_Front, num4, curve.m_Bezier, ref pathElement.m_TargetDelta) && num2 < laneBuffer.Length)
          {
            pathElement = laneBuffer[num2++];
            y = pathElement.m_TargetDelta.y;
          }
          else
            break;
        }
        TrainLaneFlags laneFlags1 = (TrainLaneFlags) 0;
        if (connectionLaneData.HasComponent(pathElement.m_Target))
          laneFlags1 |= TrainLaneFlags.Connection;
        trainCurrentLane.m_Front = new TrainBogieLane(pathElement.m_Target, new float4(pathElement.m_TargetDelta.xyy, y), laneFlags1);
        trainCurrentLane.m_FrontCache = new TrainBogieCache(trainCurrentLane.m_Front);
        if (index2 != 0)
          VehicleUtils.ClampPosition(ref trainNavigation.m_Front.m_Position, float3, num4);
        float3 position = trainNavigation.m_Front.m_Position;
        float num5 = math.csum(trainData1.m_BogieOffsets);
        while (true)
        {
          Curve curve = curveData[pathElement.m_Target];
          if (!VehicleUtils.MoveBufferPosition(position, ref trainNavigation.m_Rear, num5, curve.m_Bezier, ref pathElement.m_TargetDelta) && num2 < laneBuffer.Length)
          {
            pathElement = laneBuffer[num2++];
            y = pathElement.m_TargetDelta.y;
          }
          else
            break;
        }
        TrainLaneFlags laneFlags2 = (TrainLaneFlags) 0;
        if (connectionLaneData.HasComponent(pathElement.m_Target))
          laneFlags2 |= TrainLaneFlags.Connection;
        trainCurrentLane.m_Rear = new TrainBogieLane(pathElement.m_Target, new float4(pathElement.m_TargetDelta.xyy, y), laneFlags2);
        trainCurrentLane.m_RearCache = new TrainBogieCache(trainCurrentLane.m_Rear);
        VehicleUtils.ClampPosition(ref trainNavigation.m_Rear.m_Position, position, num5);
        float3 = trainNavigation.m_Rear.m_Position;
        num3 = trainData1.m_AttachOffsets.y - trainData1.m_BogieOffsets.y;
        float3 b = trainNavigation.m_Rear.m_Position - trainNavigation.m_Front.m_Position;
        MathUtils.TryNormalize(ref b, trainData1.m_BogieOffsets.x);
        Transform transform;
        transform.m_Position = trainNavigation.m_Front.m_Position + b;
        float3 forward = math.select(-b, b, c);
        transform.m_Rotation = !MathUtils.TryNormalize(ref forward) ? quaternion.identity : quaternion.LookRotationSafe(forward, math.up());
        transformData[vehicle] = transform;
        ParkedTrain componentData;
        if (parkedTrainData.TryGetComponent(vehicle, out componentData))
        {
          componentData.m_FrontLane = trainCurrentLane.m_Front.m_Lane;
          componentData.m_RearLane = trainCurrentLane.m_Rear.m_Lane;
          componentData.m_CurvePosition = new float2(trainCurrentLane.m_Front.m_CurvePosition.y, trainCurrentLane.m_Rear.m_CurvePosition.y);
          parkedTrainData[vehicle] = componentData;
        }
        else if (currentLaneData.HasComponent(vehicle))
        {
          currentLaneData[vehicle] = trainCurrentLane;
          navigationData[vehicle] = trainNavigation;
        }
      }
    }

    private static void ClampPosition(ref float3 position, float3 original, float maxDistance)
    {
      position = original + MathUtils.ClampLength(position - original, maxDistance);
    }

    private static bool MoveBufferPosition(
      float3 comparePosition,
      ref TrainBogiePosition targetPosition,
      float minDistance,
      Bezier4x3 curve,
      ref float2 curveDelta)
    {
      float3 y1 = MathUtils.Position(curve, curveDelta.x);
      if ((double) math.distance(comparePosition, y1) < (double) minDistance)
      {
        curveDelta.y = curveDelta.x;
        targetPosition.m_Position = y1;
        targetPosition.m_Direction = MathUtils.Tangent(curve, curveDelta.x);
        targetPosition.m_Direction *= math.sign(curveDelta.y - curveDelta.x);
        return false;
      }
      float2 float2 = curveDelta;
      for (int index = 0; index < 8; ++index)
      {
        float t = math.lerp(float2.x, float2.y, 0.5f);
        float3 y2 = MathUtils.Position(curve, t);
        if ((double) math.distance(comparePosition, y2) < (double) minDistance)
          float2.y = t;
        else
          float2.x = t;
      }
      curveDelta.y = float2.x;
      targetPosition.m_Position = MathUtils.Position(curve, float2.x);
      targetPosition.m_Direction = MathUtils.Tangent(curve, float2.x);
      targetPosition.m_Direction *= math.sign(curveDelta.y - curveDelta.x);
      return true;
    }

    public static void ClearEndOfPath(
      ref CarCurrentLane currentLane,
      DynamicBuffer<CarNavigationLane> navigationLanes)
    {
      if (navigationLanes.Length != 0)
      {
        CarNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 1];
        navigationLane.m_Flags &= ~CarLaneFlags.EndOfPath;
        navigationLanes[navigationLanes.Length - 1] = navigationLane;
      }
      else
        currentLane.m_LaneFlags &= ~CarLaneFlags.EndOfPath;
    }

    public static void ClearEndOfPath(
      ref WatercraftCurrentLane currentLane,
      DynamicBuffer<WatercraftNavigationLane> navigationLanes)
    {
      if (navigationLanes.Length != 0)
      {
        WatercraftNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 1];
        navigationLane.m_Flags &= ~WatercraftLaneFlags.EndOfPath;
        navigationLanes[navigationLanes.Length - 1] = navigationLane;
      }
      else
        currentLane.m_LaneFlags &= ~WatercraftLaneFlags.EndOfPath;
    }

    public static void ClearEndOfPath(
      ref AircraftCurrentLane currentLane,
      DynamicBuffer<AircraftNavigationLane> navigationLanes)
    {
      if (navigationLanes.Length != 0)
      {
        AircraftNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 1];
        navigationLane.m_Flags &= ~AircraftLaneFlags.EndOfPath;
        navigationLanes[navigationLanes.Length - 1] = navigationLane;
      }
      else
        currentLane.m_LaneFlags &= ~AircraftLaneFlags.EndOfPath;
    }

    public static void ClearEndOfPath(
      ref TrainCurrentLane currentLane,
      DynamicBuffer<TrainNavigationLane> navigationLanes)
    {
      while (navigationLanes.Length != 0)
      {
        TrainNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 1];
        if ((navigationLane.m_Flags & TrainLaneFlags.ParkingSpace) != (TrainLaneFlags) 0)
        {
          navigationLanes.RemoveAt(navigationLanes.Length - 1);
        }
        else
        {
          navigationLane.m_Flags &= ~TrainLaneFlags.EndOfPath;
          navigationLanes[navigationLanes.Length - 1] = navigationLane;
          return;
        }
      }
      currentLane.m_Front.m_LaneFlags &= ~TrainLaneFlags.EndOfPath;
    }

    public static Entity ValidateParkingSpace(
      Entity entity,
      ref Random random,
      ref CarCurrentLane currentLane,
      ref PathOwner pathOwner,
      DynamicBuffer<CarNavigationLane> navigationLanes,
      DynamicBuffer<PathElement> path,
      ref ComponentLookup<ParkedCar> parkedCarData,
      ref ComponentLookup<Blocker> blockerData,
      ref ComponentLookup<Curve> curveData,
      ref ComponentLookup<Unspawned> unspawnedData,
      ref ComponentLookup<Game.Net.ParkingLane> parkingLaneData,
      ref ComponentLookup<GarageLane> garageLaneData,
      ref ComponentLookup<Game.Net.ConnectionLane> connectionLaneData,
      ref ComponentLookup<PrefabRef> prefabRefData,
      ref ComponentLookup<ParkingLaneData> prefabParkingLaneData,
      ref ComponentLookup<ObjectGeometryData> prefabObjectGeometryData,
      ref BufferLookup<LaneObject> laneObjectData,
      ref BufferLookup<LaneOverlap> laneOverlapData,
      bool ignoreDriveways,
      bool ignoreDisabled,
      bool boardingOnly)
    {
      for (int index = 0; index < navigationLanes.Length; ++index)
      {
        CarNavigationLane navigationLane1 = navigationLanes[index];
        if ((navigationLane1.m_Flags & CarLaneFlags.ParkingSpace) != (CarLaneFlags) 0)
        {
          if (parkingLaneData.HasComponent(navigationLane1.m_Lane))
          {
            float minT;
            if (index == 0)
            {
              minT = currentLane.m_CurvePosition.y;
            }
            else
            {
              CarNavigationLane navigationLane2 = navigationLanes[index - 1];
              minT = (navigationLane2.m_Flags & CarLaneFlags.Reserved) == (CarLaneFlags) 0 ? navigationLane2.m_CurvePosition.x : navigationLane2.m_CurvePosition.y;
            }
            float offset;
            float y = VehicleUtils.GetParkingSize(entity, ref prefabRefData, ref prefabObjectGeometryData, out offset).y;
            float x = navigationLane1.m_CurvePosition.x;
            if (VehicleUtils.FindFreeParkingSpace(ref random, navigationLane1.m_Lane, minT, y, offset, ref x, ref parkedCarData, ref curveData, ref unspawnedData, ref parkingLaneData, ref prefabRefData, ref prefabParkingLaneData, ref prefabObjectGeometryData, ref laneObjectData, ref laneOverlapData, ignoreDriveways, ignoreDisabled))
            {
              if ((navigationLane1.m_Flags & CarLaneFlags.Validated) == (CarLaneFlags) 0)
              {
                navigationLane1.m_Flags |= CarLaneFlags.Validated;
                navigationLanes[index] = navigationLane1;
              }
              if ((double) x != (double) navigationLane1.m_CurvePosition.x)
                VehicleUtils.SetParkingCurvePos(path, pathOwner, ref currentLane, navigationLanes, index, x, ref curveData);
            }
            else
            {
              if ((navigationLane1.m_Flags & CarLaneFlags.Validated) != (CarLaneFlags) 0)
              {
                navigationLane1.m_Flags &= ~CarLaneFlags.Validated;
                navigationLanes[index] = navigationLane1;
              }
              if (boardingOnly)
              {
                if (index == 0)
                {
                  currentLane.m_LaneFlags |= CarLaneFlags.EndOfPath;
                  navigationLanes.Clear();
                }
                else
                {
                  Blocker componentData1;
                  ParkedCar componentData2;
                  if (blockerData.TryGetComponent(entity, out componentData1) && parkedCarData.TryGetComponent(componentData1.m_Blocker, out componentData2) && componentData2.m_Lane == navigationLane1.m_Lane)
                  {
                    CarNavigationLane navigationLane3 = navigationLanes[index - 1];
                    navigationLane3.m_Flags |= CarLaneFlags.EndOfPath;
                    navigationLanes[index - 1] = navigationLane3;
                    navigationLanes.RemoveRange(index, navigationLanes.Length - index);
                  }
                }
              }
              else
              {
                if (index == 0)
                  currentLane.m_CurvePosition.z = 1f;
                pathOwner.m_State |= PathFlags.Obsolete;
              }
            }
          }
          else
          {
            GarageLane componentData;
            if (!boardingOnly && garageLaneData.TryGetComponent(navigationLane1.m_Lane, out componentData))
            {
              Game.Net.ConnectionLane connectionLane = connectionLaneData[navigationLane1.m_Lane];
              if ((int) componentData.m_VehicleCount < (int) componentData.m_VehicleCapacity && (ignoreDisabled || (connectionLane.m_Flags & ConnectionLaneFlags.Disabled) == (ConnectionLaneFlags) 0))
              {
                if ((navigationLane1.m_Flags & CarLaneFlags.Validated) == (CarLaneFlags) 0)
                {
                  navigationLane1.m_Flags |= CarLaneFlags.Validated;
                  navigationLanes[index] = navigationLane1;
                }
              }
              else
              {
                if ((navigationLane1.m_Flags & CarLaneFlags.Validated) != (CarLaneFlags) 0)
                {
                  navigationLane1.m_Flags &= ~CarLaneFlags.Validated;
                  navigationLanes[index] = navigationLane1;
                }
                pathOwner.m_State |= PathFlags.Obsolete;
              }
            }
            else if ((navigationLane1.m_Flags & CarLaneFlags.Validated) == (CarLaneFlags) 0)
            {
              navigationLane1.m_Flags |= CarLaneFlags.Validated;
              navigationLanes[index] = navigationLane1;
            }
          }
          return navigationLane1.m_Lane;
        }
      }
      return Entity.Null;
    }

    public static bool FindFreeParkingSpace(
      ref Random random,
      Entity lane,
      float minT,
      float parkingLength,
      float parkingOffset,
      ref float curvePos,
      ref ComponentLookup<ParkedCar> parkedCarData,
      ref ComponentLookup<Curve> curveData,
      ref ComponentLookup<Unspawned> unspawnedData,
      ref ComponentLookup<Game.Net.ParkingLane> parkingLaneData,
      ref ComponentLookup<PrefabRef> prefabRefData,
      ref ComponentLookup<ParkingLaneData> prefabParkingLaneData,
      ref ComponentLookup<ObjectGeometryData> prefabObjectGeometryData,
      ref BufferLookup<LaneObject> laneObjectData,
      ref BufferLookup<LaneOverlap> laneOverlapData,
      bool ignoreDriveways,
      bool ignoreDisabled)
    {
      Curve curve = curveData[lane];
      Game.Net.ParkingLane parkingLane = parkingLaneData[lane];
      if ((parkingLane.m_Flags & ParkingLaneFlags.VirtualLane) != (ParkingLaneFlags) 0 || ignoreDisabled && (parkingLane.m_Flags & ParkingLaneFlags.ParkingDisabled) != (ParkingLaneFlags) 0)
        return false;
      PrefabRef prefabRef = prefabRefData[lane];
      DynamicBuffer<LaneObject> dynamicBuffer1 = laneObjectData[lane];
      DynamicBuffer<LaneOverlap> dynamicBuffer2 = new DynamicBuffer<LaneOverlap>();
      if (!ignoreDriveways)
        dynamicBuffer2 = laneOverlapData[lane];
      ParkingLaneData prefabParkingLane = prefabParkingLaneData[prefabRef.m_Prefab];
      if ((double) prefabParkingLane.m_MaxCarLength != 0.0 && (double) prefabParkingLane.m_MaxCarLength < (double) parkingLength)
        return false;
      if ((double) prefabParkingLane.m_SlotInterval != 0.0)
      {
        int parkingSlotCount = NetUtils.GetParkingSlotCount(curve, parkingLane, prefabParkingLane);
        float parkingSlotInterval = NetUtils.GetParkingSlotInterval(curve, parkingLane, prefabParkingLane, parkingSlotCount);
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
        int max = 0;
        float num4 = curvePos;
        float num5 = 2f;
        int num6 = 0;
        while (num6 < dynamicBuffer1.Length)
        {
          LaneObject laneObject = dynamicBuffer1[num6++];
          if (parkedCarData.HasComponent(laneObject.m_LaneObject) && !unspawnedData.HasComponent(laneObject.m_LaneObject))
          {
            num5 = laneObject.m_CurvePosition.x;
            break;
          }
        }
        float2 float2_2 = (float2) 2f;
        int num7 = 0;
        if (!ignoreDriveways && num7 < dynamicBuffer2.Length)
        {
          LaneOverlap laneOverlap = dynamicBuffer2[num7++];
          float2_2 = new float2((float) laneOverlap.m_ThisStart, (float) laneOverlap.m_ThisEnd) * 0.003921569f;
        }
        for (int index = 1; index <= 16; ++index)
        {
          float num8 = (float) index * (1f / 16f);
          float3 y = MathUtils.Position(curve.m_Bezier, num8);
          for (num1 += math.distance(x1, y); (double) num1 >= (double) num2 || index == 16 && num3 < parkingSlotCount; ++num3)
          {
            float2_1.y = math.select(num8, math.lerp(float2_1.x, num8, num2 / num1), (double) num2 < (double) num1);
            bool flag = false;
            if ((double) num5 <= (double) float2_1.y)
            {
              num5 = 2f;
              flag = true;
              while (num6 < dynamicBuffer1.Length)
              {
                LaneObject laneObject = dynamicBuffer1[num6++];
                if (parkedCarData.HasComponent(laneObject.m_LaneObject) && !unspawnedData.HasComponent(laneObject.m_LaneObject) && (double) laneObject.m_CurvePosition.x > (double) float2_1.y)
                {
                  num5 = laneObject.m_CurvePosition.x;
                  break;
                }
              }
            }
            if (!ignoreDriveways && (double) float2_2.x < (double) float2_1.y)
            {
              flag = true;
              if ((double) float2_2.y <= (double) float2_1.y)
              {
                float2_2 = (float2) 2f;
                while (num7 < dynamicBuffer2.Length)
                {
                  LaneOverlap laneOverlap = dynamicBuffer2[num7++];
                  float2 float2_3 = new float2((float) laneOverlap.m_ThisStart, (float) laneOverlap.m_ThisEnd) * 0.003921569f;
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
              if ((double) curvePos >= (double) float2_1.x && (double) curvePos <= (double) float2_1.y)
              {
                curvePos = math.lerp(float2_1.x, float2_1.y, 0.5f);
                return true;
              }
              if ((double) float2_1.y > (double) minT)
              {
                ++max;
                if (random.NextInt(max) == 0)
                  num4 = math.lerp(float2_1.x, float2_1.y, 0.5f);
              }
            }
            num1 -= num2;
            float2_1.x = float2_1.y;
            num2 = parkingSlotInterval;
          }
          x1 = y;
        }
        if ((double) num4 != (double) curvePos && (double) prefabParkingLane.m_SlotAngle <= 0.25)
        {
          if ((double) parkingOffset > 0.0)
          {
            Bounds1 t = new Bounds1(num4, 1f);
            MathUtils.ClampLength(curve.m_Bezier, ref t, parkingOffset);
            num4 = t.max;
          }
          else if ((double) parkingOffset < 0.0)
          {
            Bounds1 t = new Bounds1(0.0f, num4);
            MathUtils.ClampLengthInverse(curve.m_Bezier, ref t, -parkingOffset);
            num4 = t.min;
          }
        }
        curvePos = num4;
        return max != 0;
      }
      float2 float2_4 = new float2();
      float2 float2_5 = new float2();
      int max1 = 0;
      float3 float3 = new float3();
      float2 x3 = (float2) math.select(0.0f, 0.5f, (parkingLane.m_Flags & ParkingLaneFlags.StartingLane) == (ParkingLaneFlags) 0);
      float3 x4 = curve.m_Bezier.a;
      float num9 = 2f;
      float2 float2_6 = (float2) 0.0f;
      int num10 = 0;
      while (num10 < dynamicBuffer1.Length)
      {
        LaneObject laneObject = dynamicBuffer1[num10++];
        if (parkedCarData.HasComponent(laneObject.m_LaneObject) && !unspawnedData.HasComponent(laneObject.m_LaneObject))
        {
          num9 = laneObject.m_CurvePosition.x;
          float2_6 = VehicleUtils.GetParkingOffsets(laneObject.m_LaneObject, ref prefabRefData, ref prefabObjectGeometryData) + 1f;
          break;
        }
      }
      float2 float2_7 = (float2) 2f;
      int num11 = 0;
      if (!ignoreDriveways && num11 < dynamicBuffer2.Length)
      {
        LaneOverlap laneOverlap = dynamicBuffer2[num11++];
        float2_7 = new float2((float) laneOverlap.m_ThisStart, (float) laneOverlap.m_ThisEnd) * 0.003921569f;
      }
      while ((double) num9 != 2.0 || (double) float2_7.x != 2.0)
      {
        float num12;
        if (ignoreDriveways || (double) num9 <= (double) float2_7.x)
        {
          float3.yz = (float2) num9;
          x3.y = float2_6.x;
          num12 = float2_6.y;
          num9 = 2f;
          while (num10 < dynamicBuffer1.Length)
          {
            LaneObject laneObject = dynamicBuffer1[num10++];
            if (parkedCarData.HasComponent(laneObject.m_LaneObject) && !unspawnedData.HasComponent(laneObject.m_LaneObject))
            {
              num9 = laneObject.m_CurvePosition.x;
              float2_6 = VehicleUtils.GetParkingOffsets(laneObject.m_LaneObject, ref prefabRefData, ref prefabObjectGeometryData) + 1f;
              break;
            }
          }
        }
        else
        {
          float3.yz = float2_7;
          x3.y = 0.5f;
          num12 = 0.5f;
          float2_7 = (float2) 2f;
          while (num11 < dynamicBuffer2.Length)
          {
            LaneOverlap laneOverlap = dynamicBuffer2[num11++];
            float2 float2_8 = new float2((float) laneOverlap.m_ThisStart, (float) laneOverlap.m_ThisEnd) * 0.003921569f;
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
        if ((double) math.distance(x4, y) - (double) math.csum(x3) >= (double) parkingLength)
        {
          if ((double) curvePos > (double) float3.x && (double) curvePos < (double) float3.y)
          {
            ++max1;
            float2_4 = float3.xy;
            float2_5 = x3;
            goto label_76;
          }
          else if ((double) float3.y > (double) minT)
          {
            ++max1;
            if (random.NextInt(max1) == 0)
            {
              float2_4 = float3.xy;
              float2_5 = x3;
            }
          }
        }
        float3.x = float3.z;
        x3.x = num12;
        x4 = MathUtils.Position(curve.m_Bezier, float3.z);
      }
      float3.y = 1f;
      x3.y = math.select(0.0f, 0.5f, (parkingLane.m_Flags & ParkingLaneFlags.EndingLane) == (ParkingLaneFlags) 0);
      if ((double) math.distance(x4, curve.m_Bezier.d) - (double) math.csum(x3) >= (double) parkingLength)
      {
        if ((double) curvePos > (double) float3.x && (double) curvePos < (double) float3.y)
        {
          ++max1;
          float2_4 = float3.xy;
          float2_5 = x3;
        }
        else if ((double) float3.y > (double) minT)
        {
          ++max1;
          if (random.NextInt(max1) == 0)
          {
            float2_4 = float3.xy;
            float2_5 = x3;
          }
        }
      }
label_76:
      if (max1 == 0)
        return false;
      float2 float2_9 = float2_5 + parkingLength * 0.5f;
      float2_9.x += parkingOffset;
      float2_9.y -= parkingOffset;
      Bounds1 t1 = new Bounds1(float2_4.x, float2_4.y);
      Bounds1 t2 = new Bounds1(float2_4.x, float2_4.y);
      MathUtils.ClampLength(curve.m_Bezier, ref t1, float2_9.x);
      MathUtils.ClampLengthInverse(curve.m_Bezier, ref t2, float2_9.y);
      if ((double) curvePos < (double) t1.max || (double) curvePos > (double) t2.min)
      {
        t1.max = math.min(math.max(t1.max, minT), t2.min);
        curvePos = (double) t1.max >= (double) t2.min ? math.lerp(t1.max, t2.min, 0.5f) : random.NextFloat(t1.max, t2.min);
      }
      return true;
    }

    public static float GetLaneOffset(
      ObjectGeometryData prefabObjectGeometryData,
      NetLaneData prefabLaneData,
      float lanePosition)
    {
      float num1 = prefabObjectGeometryData.m_Bounds.max.x - prefabObjectGeometryData.m_Bounds.min.x;
      float num2 = math.max(0.0f, prefabLaneData.m_Width - num1);
      return lanePosition * num2;
    }

    public static float3 GetLanePosition(Bezier4x3 curve, float curvePosition, float laneOffset)
    {
      float3 lanePosition = MathUtils.Position(curve, curvePosition);
      float2 forward = math.normalizesafe(MathUtils.Tangent(curve, curvePosition).xz);
      lanePosition.xz += MathUtils.Right(forward) * laneOffset;
      return lanePosition;
    }

    public static float3 GetConnectionParkingPosition(
      Game.Net.ConnectionLane connectionLane,
      Bezier4x3 curve,
      float curvePosition)
    {
      float3 float3_1 = math.frac(curvePosition * new float3(100f, 10000f, 1000000f));
      float3 float3_2;
      if ((connectionLane.m_Flags & ConnectionLaneFlags.Outside) != (ConnectionLaneFlags) 0)
      {
        float3_1.z -= 0.5f;
        float3_2 = float3_1 * new float3(40f, 10f, 50f);
      }
      else
      {
        float3_1.xz -= 0.5f;
        float3_2 = float3_1 * new float3(25f, 10f, 25f);
      }
      float3 connectionParkingPosition = MathUtils.Position(curve, curvePosition);
      float2 float2_1 = math.sign(connectionParkingPosition.xz);
      float2 float2_2 = math.abs(connectionParkingPosition.xz);
      float2 forward = math.select(new float2(float2_1.x, 0.0f), new float2(0.0f, float2_1.y), (double) float2_2.y > (double) float2_2.x);
      float2 float2_3 = MathUtils.Right(forward);
      connectionParkingPosition.xz += forward * float3_2.x + float2_3 * float3_2.z;
      connectionParkingPosition.y += float3_2.y;
      return connectionParkingPosition;
    }

    public static Bounds3 GetConnectionParkingBounds(Game.Net.ConnectionLane connectionLane, Bezier4x3 curve)
    {
      Bounds3 connectionParkingBounds = MathUtils.Bounds(curve);
      float3 float3 = math.select(new float3(25f, 10f, 25f), new float3(80f, 10f, 80f), (connectionLane.m_Flags & ConnectionLaneFlags.Outside) != 0);
      float3.xz *= 0.5f;
      connectionParkingBounds.min.xz -= float3.xz;
      connectionParkingBounds.max += float3;
      return connectionParkingBounds;
    }

    public static void CheckUnspawned(
      int jobIndex,
      Entity entity,
      CarCurrentLane currentLane,
      bool isUnspawned,
      EntityCommandBuffer.ParallelWriter commandBuffer)
    {
      if ((currentLane.m_LaneFlags & (CarLaneFlags.Connection | CarLaneFlags.ResetSpeed)) != (CarLaneFlags) 0)
      {
        if (isUnspawned)
          return;
        commandBuffer.AddComponent<Unspawned>(jobIndex, entity, new Unspawned());
        commandBuffer.AddComponent<BatchesUpdated>(jobIndex, entity, new BatchesUpdated());
      }
      else
      {
        if ((currentLane.m_LaneFlags & (CarLaneFlags.TransformTarget | CarLaneFlags.ParkingSpace)) != (CarLaneFlags) 0 || !isUnspawned)
          return;
        commandBuffer.RemoveComponent<Unspawned>(jobIndex, entity);
        commandBuffer.AddComponent<BatchesUpdated>(jobIndex, entity, new BatchesUpdated());
      }
    }

    public static void CheckUnspawned(
      int jobIndex,
      Entity entity,
      TrainCurrentLane currentLane,
      bool isUnspawned,
      EntityCommandBuffer.ParallelWriter commandBuffer)
    {
      if ((currentLane.m_Front.m_LaneFlags & (TrainLaneFlags.ResetSpeed | TrainLaneFlags.Connection)) != (TrainLaneFlags) 0)
      {
        if (isUnspawned)
          return;
        commandBuffer.AddComponent<Unspawned>(jobIndex, entity, new Unspawned());
        commandBuffer.AddComponent<BatchesUpdated>(jobIndex, entity, new BatchesUpdated());
      }
      else
      {
        if (!isUnspawned)
          return;
        commandBuffer.RemoveComponent<Unspawned>(jobIndex, entity);
        commandBuffer.AddComponent<BatchesUpdated>(jobIndex, entity, new BatchesUpdated());
      }
    }

    public static void CheckUnspawned(
      int jobIndex,
      Entity entity,
      AircraftCurrentLane currentLane,
      bool isUnspawned,
      EntityCommandBuffer.ParallelWriter commandBuffer)
    {
      if ((currentLane.m_LaneFlags & (AircraftLaneFlags.Connection | AircraftLaneFlags.ResetSpeed)) != (AircraftLaneFlags) 0)
      {
        if (isUnspawned)
          return;
        commandBuffer.AddComponent<Unspawned>(jobIndex, entity, new Unspawned());
        commandBuffer.AddComponent<BatchesUpdated>(jobIndex, entity, new BatchesUpdated());
      }
      else
      {
        if ((currentLane.m_LaneFlags & AircraftLaneFlags.TransformTarget) != (AircraftLaneFlags) 0 || !isUnspawned)
          return;
        commandBuffer.RemoveComponent<Unspawned>(jobIndex, entity);
        commandBuffer.AddComponent<BatchesUpdated>(jobIndex, entity, new BatchesUpdated());
      }
    }

    public static void CheckUnspawned(
      int jobIndex,
      Entity entity,
      WatercraftCurrentLane currentLane,
      bool isUnspawned,
      EntityCommandBuffer.ParallelWriter commandBuffer)
    {
      if ((currentLane.m_LaneFlags & (WatercraftLaneFlags.ResetSpeed | WatercraftLaneFlags.Connection)) != (WatercraftLaneFlags) 0)
      {
        if (isUnspawned)
          return;
        commandBuffer.AddComponent<Unspawned>(jobIndex, entity, new Unspawned());
        commandBuffer.AddComponent<BatchesUpdated>(jobIndex, entity, new BatchesUpdated());
      }
      else
      {
        if ((currentLane.m_LaneFlags & WatercraftLaneFlags.TransformTarget) != (WatercraftLaneFlags) 0 || !isUnspawned)
          return;
        commandBuffer.RemoveComponent<Unspawned>(jobIndex, entity);
        commandBuffer.AddComponent<BatchesUpdated>(jobIndex, entity, new BatchesUpdated());
      }
    }

    public static bool GetPathElement(
      int elementIndex,
      DynamicBuffer<CarNavigationLane> navigationLanes,
      NativeArray<PathElement> pathElements,
      out PathElement pathElement)
    {
      if (elementIndex < navigationLanes.Length)
      {
        CarNavigationLane navigationLane = navigationLanes[elementIndex];
        pathElement = new PathElement(navigationLane.m_Lane, navigationLane.m_CurvePosition);
        return true;
      }
      elementIndex -= navigationLanes.Length;
      if (elementIndex < pathElements.Length)
      {
        pathElement = pathElements[elementIndex];
        return true;
      }
      pathElement = new PathElement();
      return false;
    }

    public static bool SetTriangleTarget(
      float3 left,
      float3 right,
      float3 next,
      float3 comparePosition,
      int elementIndex,
      DynamicBuffer<CarNavigationLane> navigationLanes,
      NativeArray<PathElement> pathElements,
      ref float3 targetPosition,
      float minDistance,
      float lanePosition,
      float curveDelta,
      float navigationSize,
      bool isSingle,
      ComponentLookup<Transform> transforms,
      ComponentLookup<AreaLane> areaLanes,
      ComponentLookup<Curve> curves)
    {
      targetPosition = VehicleUtils.CalculateTriangleTarget(left, right, next, targetPosition, elementIndex, navigationLanes, pathElements, lanePosition, curveDelta, navigationSize, isSingle, transforms, areaLanes, curves);
      return (double) math.distance(comparePosition.xz, targetPosition.xz) >= (double) minDistance;
    }

    public static bool SetTriangleTarget(
      float3 left,
      float3 right,
      float3 next,
      float3 comparePosition,
      float3 lastTarget,
      ref float3 targetPosition,
      float minDistance,
      float navigationSize,
      bool isSingle)
    {
      targetPosition = VehicleUtils.CalculateTriangleTarget(left, right, next, lastTarget, navigationSize, isSingle);
      return (double) math.distance(comparePosition.xz, targetPosition.xz) >= (double) minDistance;
    }

    private static float3 CalculateTriangleTarget(
      float3 left,
      float3 right,
      float3 next,
      float3 lastTarget,
      int elementIndex,
      DynamicBuffer<CarNavigationLane> navigationLanes,
      NativeArray<PathElement> pathElements,
      float lanePosition,
      float curveDelta,
      float navigationSize,
      bool isSingle,
      ComponentLookup<Transform> transforms,
      ComponentLookup<AreaLane> areaLanes,
      ComponentLookup<Curve> curves)
    {
      PathElement pathElement;
      if (VehicleUtils.GetPathElement(elementIndex, navigationLanes, pathElements, out pathElement))
      {
        Transform componentData1;
        if (transforms.TryGetComponent(pathElement.m_Target, out componentData1))
          return VehicleUtils.CalculateTriangleTarget(left, right, next, componentData1.m_Position, navigationSize, isSingle);
        if (areaLanes.HasComponent(pathElement.m_Target))
          return VehicleUtils.CalculateTriangleTarget(left, right, next, lastTarget, navigationSize, isSingle);
        Curve componentData2;
        if (curves.TryGetComponent(pathElement.m_Target, out componentData2))
        {
          float3 target = MathUtils.Position(componentData2.m_Bezier, pathElement.m_TargetDelta.x);
          return VehicleUtils.CalculateTriangleTarget(left, right, next, target, navigationSize, isSingle);
        }
      }
      return VehicleUtils.CalculateTriangleTarget(left, right, next, lanePosition, curveDelta, navigationSize, isSingle);
    }

    private static float3 CalculateTriangleTarget(
      float3 left,
      float3 right,
      float3 next,
      float3 target,
      float navigationSize,
      bool isSingle)
    {
      float num1 = navigationSize * 0.5f;
      Triangle3 triangle = new Triangle3(next, left, right);
      if (isSingle)
      {
        float3 float3 = MathUtils.Incenter(triangle, out float _);
        float radius;
        MathUtils.Incenter(triangle.xz, out radius);
        float num2 = math.saturate(num1 / radius);
        triangle.a += (float3 - triangle.a) * num2;
        triangle.b += (float3 - triangle.b) * num2;
        triangle.c += (float3 - triangle.c) * num2;
        float2 t;
        if ((double) MathUtils.Distance(triangle.xz, target.xz, out t) != 0.0)
          target = MathUtils.Position(triangle, t);
      }
      else
      {
        float2 float2_1;
        float2 float2_2;
        float2_1.x = MathUtils.Distance(triangle.ba.xz, target.xz, out float2_2.x);
        float2_1.y = MathUtils.Distance(triangle.ca.xz, target.xz, out float2_2.y);
        float2 float2_3 = !MathUtils.Intersect(triangle.xz, target.xz) ? math.select(new float2(float2_1.x, -float2_1.y), new float2(-float2_1.x, float2_1.y), (double) float2_1.x > (double) float2_1.y) : -float2_1;
        if (math.any(float2_3 > -num1))
        {
          if ((double) float2_3.y <= -(double) num1)
          {
            float2 float2_4 = math.normalizesafe(MathUtils.Right(left.xz - next.xz)) * num1;
            target = MathUtils.Position(triangle.ba, float2_2.x);
            target.xz += math.select(float2_4, -float2_4, (double) math.dot(float2_4, right.xz - next.xz) < 0.0);
          }
          else if ((double) float2_3.x <= -(double) num1)
          {
            float2 float2_5 = math.normalizesafe(MathUtils.Left(right.xz - next.xz)) * num1;
            target = MathUtils.Position(triangle.ca, float2_2.y);
            target.xz += math.select(float2_5, -float2_5, (double) math.dot(float2_5, left.xz - next.xz) < 0.0);
          }
          else
            target = math.lerp(MathUtils.Position(triangle.ba, float2_2.x), MathUtils.Position(triangle.ca, float2_2.y), 0.5f);
        }
      }
      return target;
    }

    private static float3 CalculateTriangleTarget(
      float3 left,
      float3 right,
      float3 next,
      float lanePosition,
      float curveDelta,
      float navigationSize,
      bool isSingle)
    {
      float num1 = navigationSize * 0.5f;
      Line3.Segment line = new Line3.Segment(left, right);
      float num2 = lanePosition * math.saturate((float) (1.0 - (double) navigationSize / (double) MathUtils.Length(line.xz)));
      line.a = MathUtils.Position(line, num2 + 0.5f);
      line.b = next;
      float t;
      if (isSingle)
      {
        t = (float) (((double) math.sqrt(math.saturate(1f - curveDelta)) - 0.5) * (double) math.saturate((float) (1.0 - (double) navigationSize / (double) MathUtils.Length(line.xz))) + 0.5);
      }
      else
      {
        float num3 = curveDelta * 2f;
        t = math.sqrt(math.saturate(1f - math.select(1f - num3, num3 - 1f, (double) curveDelta > 0.5))) * math.saturate((float) (1.0 - (double) num1 / (double) MathUtils.Length(line.xz)));
      }
      return MathUtils.Position(line, t);
    }

    public static bool SetAreaTarget(
      float3 prev2,
      float3 prev,
      float3 left,
      float3 right,
      float3 next,
      Entity areaEntity,
      DynamicBuffer<Game.Areas.Node> nodes,
      float3 comparePosition,
      int elementIndex,
      DynamicBuffer<CarNavigationLane> navigationLanes,
      NativeArray<PathElement> pathElements,
      ref float3 targetPosition,
      float minDistance,
      float lanePosition,
      float curveDelta,
      float navigationSize,
      bool isBackward,
      ComponentLookup<Transform> transforms,
      ComponentLookup<AreaLane> areaLanes,
      ComponentLookup<Curve> curves,
      ComponentLookup<Owner> owners)
    {
      float num1 = navigationSize * 0.5f;
      Line3.Segment segment1 = new Line3.Segment(left, right);
      float num2 = 1f / MathUtils.Length(segment1.xz);
      Bounds1 other = new Bounds1(math.min(0.5f, num1 * num2), math.max(0.5f, (float) (1.0 - (double) num1 * (double) num2)));
      int num3 = elementIndex;
      PathElement pathElement1;
      Owner componentData;
      for (; VehicleUtils.GetPathElement(elementIndex, navigationLanes, pathElements, out pathElement1) && owners.TryGetComponent(pathElement1.m_Target, out componentData) && componentData.m_Owner == areaEntity; ++elementIndex)
      {
        AreaLane areaLane = areaLanes[pathElement1.m_Target];
        bool4 bool4 = new bool4(pathElement1.m_TargetDelta < 0.5f, pathElement1.m_TargetDelta > 0.5f);
        if (math.any(bool4.xy & bool4.wz))
        {
          Line3.Segment segment2 = new Line3.Segment(comparePosition, nodes[areaLane.m_Nodes.y].m_Position);
          Line3.Segment segment3 = new Line3.Segment(comparePosition, nodes[areaLane.m_Nodes.z].m_Position);
          Bounds1 bounds1_1 = other;
          Bounds1 bounds1_2 = other;
          float2 t;
          if (MathUtils.Intersect((Line2) segment1.xz, (Line2) segment2.xz, out t))
          {
            float num4 = math.max(math.max(0.0f, 0.4f * math.min(t.y, 1f - t.y) * MathUtils.Length(segment2.xz) * num2), math.max(t.x - other.max, other.min - t.x));
            if ((double) num4 < (double) other.max - (double) other.min)
              bounds1_1 = new Bounds1(math.max(other.min, math.min(other.max, t.x) - num4), math.min(other.max, math.max(other.min, t.x) + num4));
          }
          if (MathUtils.Intersect((Line2) segment1.xz, (Line2) segment3.xz, out t))
          {
            float num5 = math.max(math.max(0.0f, 0.4f * math.min(t.y, 1f - t.y) * MathUtils.Length(segment2.xz) * num2), math.max(t.x - other.max, other.min - t.x));
            if ((double) num5 < (double) other.max - (double) other.min)
              bounds1_2 = new Bounds1(math.max(other.min, math.min(other.max, t.x) - num5), math.min(other.max, math.max(other.min, t.x) + num5));
          }
          if (bounds1_1.Equals(other) & bounds1_2.Equals(other))
          {
            elementIndex = navigationLanes.Length + pathElements.Length;
          }
          else
          {
            other = bounds1_1 | bounds1_2;
            continue;
          }
        }
        ++elementIndex;
        break;
      }
      if (elementIndex - 1 < navigationLanes.Length + pathElements.Length)
      {
        float3 triangleTarget;
        if (elementIndex > num3)
        {
          PathElement pathElement2;
          VehicleUtils.GetPathElement(elementIndex - 1, navigationLanes, pathElements, out pathElement2);
          AreaLane areaLane = areaLanes[pathElement2.m_Target];
          bool c = (double) pathElement2.m_TargetDelta.y > 0.5;
          float3 position1 = nodes[areaLane.m_Nodes.y].m_Position;
          float3 position2 = nodes[areaLane.m_Nodes.z].m_Position;
          float3 position3 = nodes[math.select(areaLane.m_Nodes.x, areaLane.m_Nodes.w, c)].m_Position;
          float num6 = math.select(lanePosition, -lanePosition, (double) pathElement2.m_TargetDelta.y < (double) pathElement2.m_TargetDelta.x != isBackward);
          float3 right1 = position2;
          float3 next1 = position3;
          float3 lastTarget = targetPosition;
          int elementIndex1 = elementIndex;
          DynamicBuffer<CarNavigationLane> navigationLanes1 = navigationLanes;
          NativeArray<PathElement> pathElements1 = pathElements;
          double lanePosition1 = (double) num6;
          double y = (double) pathElement2.m_TargetDelta.y;
          double navigationSize1 = (double) navigationSize;
          ComponentLookup<Transform> transforms1 = transforms;
          ComponentLookup<AreaLane> areaLanes1 = areaLanes;
          ComponentLookup<Curve> curves1 = curves;
          triangleTarget = VehicleUtils.CalculateTriangleTarget(position1, right1, next1, lastTarget, elementIndex1, navigationLanes1, pathElements1, (float) lanePosition1, (float) y, (float) navigationSize1, false, transforms1, areaLanes1, curves1);
        }
        else
          triangleTarget = VehicleUtils.CalculateTriangleTarget(left, right, next, targetPosition, elementIndex, navigationLanes, pathElements, lanePosition, curveDelta, navigationSize, false, transforms, areaLanes, curves);
        Line3.Segment segment4 = new Line3.Segment(comparePosition, triangleTarget);
        float2 t;
        if (MathUtils.Intersect((Line2) segment1.xz, (Line2) segment4.xz, out t))
        {
          float num7 = math.max(math.max(0.0f, 0.4f * math.min(t.y, 1f - t.y) * MathUtils.Length(segment4.xz) * num2), math.max(t.x - other.max, other.min - t.x));
          if ((double) num7 < (double) other.max - (double) other.min)
            other = new Bounds1(math.max(other.min, math.min(other.max, t.x) - num7), math.min(other.max, math.max(other.min, t.x) + num7));
        }
      }
      float lanePosition2 = math.lerp(other.min, other.max, lanePosition + 0.5f);
      bool farEnough;
      targetPosition = VehicleUtils.CalculateAreaTarget(prev2, prev, left, right, comparePosition, minDistance, lanePosition2, navigationSize, out farEnough);
      return farEnough || (double) math.distance(comparePosition.xz, targetPosition.xz) >= (double) minDistance;
    }

    private static float3 CalculateAreaTarget(
      float3 prev2,
      float3 prev,
      float3 left,
      float3 right,
      float3 comparePosition,
      float minDistance,
      float lanePosition,
      float navigationSize,
      out bool farEnough)
    {
      float num1 = navigationSize * 0.5f;
      Line3.Segment line = new Line3.Segment(left, right);
      line.a = MathUtils.Position(line, lanePosition);
      if (!prev2.Equals(prev))
      {
        Line3.Segment segment = new Line3.Segment(prev2, prev);
        line.b = comparePosition;
        float2 t;
        if (MathUtils.Intersect(line.xz, segment.xz, out t) && (double) math.min(t.y, 1f - t.y) >= (double) num1 / (double) MathUtils.Length(segment.xz))
        {
          farEnough = false;
          return line.a;
        }
      }
      Triangle3 triangle3 = new Triangle3(prev, left, right);
      float2 float2_1;
      float2 float2_2;
      float2_1.x = MathUtils.Distance(triangle3.ba.xz, comparePosition.xz, out float2_2.x);
      float2_1.y = MathUtils.Distance(triangle3.ca.xz, comparePosition.xz, out float2_2.y);
      float2 float2_3 = !MathUtils.Intersect(triangle3.xz, comparePosition.xz) ? math.select(new float2(float2_1.x, -float2_1.y), new float2(-float2_1.x, float2_1.y), (double) float2_1.x > (double) float2_1.y) : -float2_1;
      if (math.all(float2_3 <= -num1))
      {
        farEnough = false;
        return line.a;
      }
      if ((double) float2_3.y <= -(double) num1)
      {
        float2 float2_4 = math.normalizesafe(MathUtils.Right(left.xz - prev.xz)) * num1;
        line.b = MathUtils.Position(triangle3.ba, float2_2.x);
        line.b.xz += math.select(float2_4, -float2_4, (double) math.dot(float2_4, right.xz - prev.xz) < 0.0);
      }
      else if ((double) float2_3.x <= -(double) num1)
      {
        float2 float2_5 = math.normalizesafe(MathUtils.Left(right.xz - prev.xz)) * num1;
        line.b = MathUtils.Position(triangle3.ca, float2_2.y);
        line.b.xz += math.select(float2_5, -float2_5, (double) math.dot(float2_5, left.xz - prev.xz) < 0.0);
      }
      else
        line.b = prev;
      float t1;
      float num2 = MathUtils.Distance(line.xz, comparePosition.xz, out t1);
      float t2 = t1 - math.sqrt(math.max(0.0f, (float) ((double) minDistance * (double) minDistance - (double) num2 * (double) num2)) / MathUtils.LengthSquared(line.xz));
      if ((double) t2 >= 0.0)
      {
        farEnough = true;
        return MathUtils.Position(line, t2);
      }
      farEnough = false;
      return line.a;
    }

    public static void ClearNavigationForPathfind(
      Moving moving,
      CarData prefabCarData,
      ref CarCurrentLane currentLane,
      DynamicBuffer<CarNavigationLane> navigationLanes,
      ref ComponentLookup<Game.Net.CarLane> carLaneLookup,
      ref ComponentLookup<Curve> curveLookup)
    {
      float timeStep = 0.266666681f;
      float num1 = 1.06666672f + timeStep;
      float speed = math.min(math.length(moving.m_Velocity), prefabCarData.m_MaxSpeed);
      if (carLaneLookup.HasComponent(currentLane.m_Lane))
      {
        Curve curve1 = curveLookup[currentLane.m_Lane];
        bool c1 = (double) currentLane.m_CurvePosition.z < (double) currentLane.m_CurvePosition.x;
        float num2 = math.max(0.0f, (float) ((double) VehicleUtils.GetBrakingDistance(prefabCarData, speed, timeStep) + (double) speed * (double) num1 - 0.0099999997764825821));
        float a1 = (float) ((double) num2 / (double) math.max(1E-06f, curve1.m_Length) + 9.9999999747524271E-07);
        float num3 = currentLane.m_CurvePosition.x + math.select(a1, -a1, c1);
        currentLane.m_LaneFlags |= CarLaneFlags.ClearedForPathfind;
        if ((c1 ? ((double) currentLane.m_CurvePosition.z <= (double) num3 ? 1 : 0) : ((double) num3 <= (double) currentLane.m_CurvePosition.z ? 1 : 0)) != 0)
        {
          currentLane.m_CurvePosition.z = num3;
          navigationLanes.Clear();
        }
        else
        {
          float num4 = num2 - curve1.m_Length * math.abs(currentLane.m_CurvePosition.z - currentLane.m_CurvePosition.x);
          int index;
          // ISSUE: variable of a reference type
          CarNavigationLane& local;
          Curve curve2;
          for (index = 0; index < navigationLanes.Length && (double) num4 > 0.0; num4 -= curve2.m_Length * math.abs(local.m_CurvePosition.y - local.m_CurvePosition.x))
          {
            local = ref navigationLanes.ElementAt(index);
            if (carLaneLookup.HasComponent(local.m_Lane))
            {
              curve2 = curveLookup[local.m_Lane];
              bool c2 = (double) local.m_CurvePosition.y < (double) local.m_CurvePosition.x;
              float a2 = num4 / math.max(1E-06f, curve2.m_Length);
              float num5 = local.m_CurvePosition.x + math.select(a2, -a2, c2);
              local.m_Flags |= CarLaneFlags.ClearedForPathfind;
              ++index;
              if ((c2 ? ((double) local.m_CurvePosition.y <= (double) num5 ? 1 : 0) : ((double) num5 <= (double) local.m_CurvePosition.y ? 1 : 0)) != 0)
              {
                local.m_CurvePosition.y = num5;
                break;
              }
            }
            else
              break;
          }
          if (index >= navigationLanes.Length)
            return;
          navigationLanes.RemoveRange(index, navigationLanes.Length - index);
        }
      }
      else
        currentLane.m_CurvePosition.z = currentLane.m_CurvePosition.y;
    }
  }
}
