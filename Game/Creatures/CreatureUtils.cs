// Decompiled with JetBrains decompiler
// Type: Game.Creatures.CreatureUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Rendering;
using Game.Routes;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Creatures
{
  public static class CreatureUtils
  {
    public const float MAX_HUMAN_WALK_SPEED = 5.555556f;
    public const float AVG_HUMAN_WALK_SPEED = 1.66666675f;
    public const float MIN_MOVE_SPEED = 0.1f;
    public const float RESIDENT_PATHFIND_RANDOM_COST = 30f;
    public const int MAX_TRANSPORT_WAIT_TICKS = 5000;
    public const float QUEUE_TICKS_TO_SECONDS = 0.13333334f;

    public static bool PathfindFailed(PathOwner pathOwner)
    {
      return (pathOwner.m_State & (PathFlags.Failed | PathFlags.Stuck)) != 0;
    }

    public static bool EndReached(HumanCurrentLane currentLane)
    {
      return (currentLane.m_Flags & CreatureLaneFlags.EndReached) > (CreatureLaneFlags) 0;
    }

    public static bool PathEndReached(HumanCurrentLane currentLane)
    {
      return (currentLane.m_Flags & (CreatureLaneFlags.EndOfPath | CreatureLaneFlags.EndReached)) == (CreatureLaneFlags.EndOfPath | CreatureLaneFlags.EndReached);
    }

    public static bool PathEndReached(AnimalCurrentLane currentLane)
    {
      return (currentLane.m_Flags & (CreatureLaneFlags.EndOfPath | CreatureLaneFlags.EndReached)) == (CreatureLaneFlags.EndOfPath | CreatureLaneFlags.EndReached);
    }

    public static bool ParkingSpaceReached(HumanCurrentLane currentLane)
    {
      return (currentLane.m_Flags & (CreatureLaneFlags.EndReached | CreatureLaneFlags.ParkingSpace)) == (CreatureLaneFlags.EndReached | CreatureLaneFlags.ParkingSpace);
    }

    public static bool ActionLocationReached(HumanCurrentLane currentLane)
    {
      return (currentLane.m_Flags & (CreatureLaneFlags.EndReached | CreatureLaneFlags.Action)) == (CreatureLaneFlags.EndReached | CreatureLaneFlags.Action);
    }

    public static bool TransportStopReached(HumanCurrentLane currentLane)
    {
      return (currentLane.m_Flags & (CreatureLaneFlags.EndReached | CreatureLaneFlags.Transport)) == (CreatureLaneFlags.EndReached | CreatureLaneFlags.Transport);
    }

    public static bool RequireNewPath(PathOwner pathOwner)
    {
      return (pathOwner.m_State & (PathFlags.Obsolete | PathFlags.DivertObsolete)) != (PathFlags) 0 && (pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Stuck)) == (PathFlags) 0;
    }

    public static bool IsStuck(PathOwner pathOwner) => (pathOwner.m_State & PathFlags.Stuck) != 0;

    public static bool IsStuck(AnimalCurrentLane currentLane)
    {
      return (currentLane.m_Flags & CreatureLaneFlags.Stuck) > (CreatureLaneFlags) 0;
    }

    public static bool ResetUncheckedLane(ref HumanCurrentLane currentLane)
    {
      int num = (currentLane.m_Flags & CreatureLaneFlags.Checked) == (CreatureLaneFlags) 0 ? 1 : 0;
      currentLane.m_Flags |= CreatureLaneFlags.Checked;
      return num != 0;
    }

    public static void SetupPathfind(
      ref HumanCurrentLane currentLane,
      ref PathOwner pathOwner,
      NativeQueue<SetupQueueItem>.ParallelWriter queue,
      SetupQueueItem item)
    {
      if ((pathOwner.m_State & (PathFlags.Obsolete | PathFlags.Divert)) == (PathFlags.Obsolete | PathFlags.Divert))
        pathOwner.m_State |= PathFlags.CachedObsolete;
      pathOwner.m_State &= ~(PathFlags.Failed | PathFlags.Obsolete | PathFlags.DivertObsolete);
      pathOwner.m_State |= PathFlags.Pending;
      currentLane.m_Flags &= ~(CreatureLaneFlags.EndOfPath | CreatureLaneFlags.ParkingSpace | CreatureLaneFlags.Transport | CreatureLaneFlags.Taxi | CreatureLaneFlags.Action);
      queue.Enqueue(item);
    }

    public static bool DivertDestination(
      ref SetupQueueTarget destination,
      ref PathOwner pathOwner,
      Divert divert)
    {
      if (divert.m_Purpose == Game.Citizens.Purpose.None)
        return true;
      if (divert.m_Target != Entity.Null)
      {
        destination.m_Entity = divert.m_Target;
        pathOwner.m_State |= PathFlags.Divert;
        return true;
      }
      switch (divert.m_Purpose)
      {
        case Game.Citizens.Purpose.Safety:
        case Game.Citizens.Purpose.Escape:
          destination.m_Type = SetupTargetType.Safety;
          pathOwner.m_State |= PathFlags.Divert;
          return true;
        case Game.Citizens.Purpose.SendMail:
          destination.m_Type = SetupTargetType.MailBox;
          pathOwner.m_State |= PathFlags.AddDestination | PathFlags.Divert;
          return true;
        case Game.Citizens.Purpose.Disappear:
          destination.m_Type = SetupTargetType.OutsideConnection;
          pathOwner.m_State |= PathFlags.AddDestination | PathFlags.Divert;
          return true;
        case Game.Citizens.Purpose.WaitingHome:
        case Game.Citizens.Purpose.PathFailed:
          return false;
        default:
          return true;
      }
    }

    public static bool ResetUpdatedPath(ref PathOwner pathOwner)
    {
      int num = (pathOwner.m_State & PathFlags.Updated) != 0 ? 1 : 0;
      pathOwner.m_State &= ~PathFlags.Updated;
      return num != 0;
    }

    public static Game.Objects.Transform GetVehicleDoorPosition(
      ref Unity.Mathematics.Random random,
      ActivityType activityType,
      ActivityCondition conditions,
      Game.Objects.Transform vehicleTransform,
      float3 targetPosition,
      bool isDriver,
      bool lefthandTraffic,
      Entity creaturePrefab,
      Entity vehicle,
      DynamicBuffer<MeshGroup> meshGroups,
      ref ComponentLookup<Game.Vehicles.PublicTransport> publicTransports,
      ref ComponentLookup<Train> trains,
      ref ComponentLookup<Controller> controllers,
      ref ComponentLookup<PrefabRef> prefabRefs,
      ref ComponentLookup<CarData> prefabCarDatas,
      ref BufferLookup<ActivityLocationElement> prefabActivityLocations,
      ref BufferLookup<SubMeshGroup> subMeshGroupBuffers,
      ref BufferLookup<CharacterElement> characterElementBuffers,
      ref BufferLookup<SubMesh> subMeshBuffers,
      ref BufferLookup<Game.Prefabs.AnimationClip> animationClipBuffers,
      ref BufferLookup<AnimationMotion> animationMotionBuffers,
      out ActivityMask activityMask,
      out AnimatedPropID propID)
    {
      PrefabRef prefabRef = prefabRefs[vehicle];
      Game.Objects.Transform vehicleDoorPosition = vehicleTransform;
      activityMask = new ActivityMask();
      propID = new AnimatedPropID(-1);
      DynamicBuffer<ActivityLocationElement> bufferData;
      if (prefabActivityLocations.TryGetBuffer(prefabRef.m_Prefab, out bufferData))
      {
        ActivityMask activityMask1 = new ActivityMask(ActivityType.Enter);
        ActivityMask activityMask2 = new ActivityMask(ActivityType.Driving);
        activityMask1.m_Mask |= new ActivityMask(ActivityType.Exit).m_Mask;
        int num1 = 0;
        int num2 = -1;
        bool a = true;
        bool b = true;
        Game.Vehicles.PublicTransport componentData1;
        if (publicTransports.TryGetComponent(vehicle, out componentData1))
        {
          bool flag = false;
          Controller componentData2;
          Game.Vehicles.PublicTransport componentData3;
          if (controllers.TryGetComponent(vehicle, out componentData2) && publicTransports.TryGetComponent(componentData2.m_Controller, out componentData3))
          {
            componentData1 = componentData3;
            Train componentData4;
            Train componentData5;
            if (trains.TryGetComponent(vehicle, out componentData4) && trains.TryGetComponent(componentData2.m_Controller, out componentData5))
              flag = ((componentData4.m_Flags ^ componentData5.m_Flags) & Game.Vehicles.TrainFlags.Reversed) > (Game.Vehicles.TrainFlags) 0;
          }
          a = (componentData1.m_State & PublicTransportFlags.StopRight) == (PublicTransportFlags) 0;
          b = (componentData1.m_State & PublicTransportFlags.StopLeft) == (PublicTransportFlags) 0;
          if (flag)
            CommonUtils.Swap<bool>(ref a, ref b);
        }
        else if (prefabCarDatas.HasComponent(prefabRef.m_Prefab))
        {
          float num3 = float.MinValue;
          for (int index = 0; index < bufferData.Length; ++index)
          {
            ActivityLocationElement activityLocationElement = bufferData[index];
            if (((int) activityLocationElement.m_ActivityMask.m_Mask & (int) activityMask1.m_Mask) == (int) activityMask1.m_Mask)
            {
              bool c = (activityLocationElement.m_ActivityFlags & ActivityFlags.InvertLefthandTraffic) != 0 & lefthandTraffic || (activityLocationElement.m_ActivityFlags & ActivityFlags.InvertRighthandTraffic) != (ActivityFlags) 0 && !lefthandTraffic;
              activityLocationElement.m_Position.x = math.select(activityLocationElement.m_Position.x, -activityLocationElement.m_Position.x, c);
              if (((double) math.abs(activityLocationElement.m_Position.x) < 0.5 || (double) activityLocationElement.m_Position.x >= 0.0 == lefthandTraffic) && (double) activityLocationElement.m_Position.z > (double) num3)
              {
                num2 = index;
                num3 = activityLocationElement.m_Position.z;
              }
            }
          }
        }
        isDriver &= num2 != -1;
        ObjectUtils.ActivityStartPositionCache cache = new ObjectUtils.ActivityStartPositionCache();
        for (int index = 0; index < bufferData.Length; ++index)
        {
          ActivityLocationElement activityLocationElement = bufferData[index];
          ActivityMask activityMask3 = new ActivityMask(activityType);
          activityMask3.m_Mask &= activityLocationElement.m_ActivityMask.m_Mask;
          if (activityMask3.m_Mask != 0U && isDriver == (index == num2))
          {
            bool c = (activityLocationElement.m_ActivityFlags & ActivityFlags.InvertLefthandTraffic) != 0 & lefthandTraffic || (activityLocationElement.m_ActivityFlags & ActivityFlags.InvertRighthandTraffic) != (ActivityFlags) 0 && !lefthandTraffic;
            activityLocationElement.m_Position.x = math.select(activityLocationElement.m_Position.x, -activityLocationElement.m_Position.x, c);
            if ((double) math.abs(activityLocationElement.m_Position.x) < 0.5 || ((double) activityLocationElement.m_Position.x >= 0.0 ? (b ? 1 : 0) : (a ? 1 : 0)) != 0)
            {
              if (activityType == ActivityType.Exit && ((int) activityLocationElement.m_ActivityMask.m_Mask & (int) activityMask1.m_Mask) == (int) activityMask1.m_Mask && ((int) activityLocationElement.m_ActivityMask.m_Mask & (int) activityMask2.m_Mask) == 0)
                activityLocationElement.m_Rotation = math.mul(quaternion.RotateY(3.14159274f), activityLocationElement.m_Rotation);
              Game.Objects.Transform activityTransform = ObjectUtils.LocalToWorld(vehicleTransform, activityLocationElement.m_Position, activityLocationElement.m_Rotation);
              if (activityType == ActivityType.Enter && ((int) activityLocationElement.m_ActivityMask.m_Mask & (int) activityMask2.m_Mask) != 0)
                activityTransform = ObjectUtils.GetActivityStartPosition(creaturePrefab, meshGroups, activityTransform, TransformState.Action, activityType, activityLocationElement.m_PropID, conditions, ref subMeshGroupBuffers, ref characterElementBuffers, ref subMeshBuffers, ref animationClipBuffers, ref animationMotionBuffers, ref cache);
              if ((double) math.distancesq(activityTransform.m_Position, targetPosition) < 0.0099999997764825821)
              {
                activityMask = activityLocationElement.m_ActivityMask;
                propID = activityLocationElement.m_PropID;
                return activityTransform;
              }
              if (random.NextInt(++num1) == 0)
              {
                vehicleDoorPosition = activityTransform;
                activityMask = activityLocationElement.m_ActivityMask;
                propID = activityLocationElement.m_PropID;
              }
            }
          }
        }
      }
      return vehicleDoorPosition;
    }

    public static ActivityCondition GetConditions(Human human)
    {
      ActivityCondition conditions = (ActivityCondition) 0;
      if ((human.m_Flags & HumanFlags.Homeless) != (HumanFlags) 0)
        conditions |= ActivityCondition.Homeless;
      if ((human.m_Flags & HumanFlags.Angry) != (HumanFlags) 0)
        conditions |= ActivityCondition.Angry;
      else if ((human.m_Flags & HumanFlags.Waiting) != (HumanFlags) 0)
        conditions |= ActivityCondition.Waiting;
      else if ((human.m_Flags & HumanFlags.Sad) != (HumanFlags) 0)
        conditions |= ActivityCondition.Sad;
      else if ((human.m_Flags & HumanFlags.Happy) != (HumanFlags) 0)
        conditions |= ActivityCondition.Happy;
      return conditions;
    }

    public static bool CalculateTransformPosition(
      Entity creature,
      Entity creaturePrefab,
      DynamicBuffer<MeshGroup> meshGroups,
      ref Unity.Mathematics.Random random,
      ref Game.Objects.Transform result,
      ref ActivityType activity,
      CurrentVehicle currentVehicle,
      Entity entity,
      bool leftHandTraffic,
      ActivityMask activityMask,
      ActivityCondition conditions,
      NativeQuadTree<Entity, QuadTreeBoundsXZ> movingObjectSearchTree,
      ref ComponentLookup<Game.Objects.Transform> transforms,
      ref ComponentLookup<Position> positions,
      ref ComponentLookup<Game.Vehicles.PublicTransport> publicTransports,
      ref ComponentLookup<Train> trains,
      ref ComponentLookup<Controller> controllers,
      ref ComponentLookup<PrefabRef> prefabRefs,
      ref ComponentLookup<BuildingData> prefabBuildingDatas,
      ref ComponentLookup<CarData> prefabCarDatas,
      ref BufferLookup<ActivityLocationElement> prefabActivityLocations,
      ref BufferLookup<SubMeshGroup> subMeshGroupBuffers,
      ref BufferLookup<CharacterElement> characterElementBuffers,
      ref BufferLookup<SubMesh> subMeshBuffers,
      ref BufferLookup<Game.Prefabs.AnimationClip> animationClipBuffers,
      ref BufferLookup<AnimationMotion> animationMotionBuffers)
    {
      if (transforms.HasComponent(entity))
      {
        Game.Objects.Transform transform = transforms[entity];
        PrefabRef prefabRef = prefabRefs[entity];
        if (entity == currentVehicle.m_Vehicle)
        {
          float3 position = result.m_Position;
          bool isDriver = (currentVehicle.m_Flags & CreatureVehicleFlags.Driver) > (CreatureVehicleFlags) 0;
          ActivityMask activityMask1;
          result = CreatureUtils.GetVehicleDoorPosition(ref random, ActivityType.Enter, conditions, transform, position, isDriver, leftHandTraffic, creaturePrefab, entity, meshGroups, ref publicTransports, ref trains, ref controllers, ref prefabRefs, ref prefabCarDatas, ref prefabActivityLocations, ref subMeshGroupBuffers, ref characterElementBuffers, ref subMeshBuffers, ref animationClipBuffers, ref animationMotionBuffers, out activityMask1, out AnimatedPropID _);
          if (((int) activityMask1.m_Mask & (int) new ActivityMask(ActivityType.Driving).m_Mask) != 0)
            activity = ActivityType.Enter;
          return true;
        }
        if (prefabBuildingDatas.HasComponent(prefabRef.m_Prefab))
        {
          BuildingData buildingData = prefabBuildingDatas[prefabRef.m_Prefab];
          result.m_Position = BuildingUtils.CalculateFrontPosition(transform, buildingData.m_LotSize.y);
          return true;
        }
        if (prefabActivityLocations.HasBuffer(prefabRef.m_Prefab))
        {
          DynamicBuffer<ActivityLocationElement> dynamicBuffer = prefabActivityLocations[prefabRef.m_Prefab];
          float num1 = float.MaxValue;
          float3 position = result.m_Position;
          CreatureUtils.ActivityLocationIterator iterator = new CreatureUtils.ActivityLocationIterator()
          {
            m_Ignore = creature,
            m_TransformData = transforms
          };
          ObjectUtils.ActivityStartPositionCache cache = new ObjectUtils.ActivityStartPositionCache();
          for (int index1 = 0; index1 < dynamicBuffer.Length; ++index1)
          {
            ActivityLocationElement activityLocationElement = dynamicBuffer[index1];
            ActivityMask activityMask2 = activityMask;
            activityMask2.m_Mask &= activityLocationElement.m_ActivityMask.m_Mask;
            if (activityMask2.m_Mask != 0U)
            {
              Game.Objects.Transform world = ObjectUtils.LocalToWorld(transform, activityLocationElement.m_Position, activityLocationElement.m_Rotation);
              float3 float3 = math.forward(world.m_Rotation);
              iterator.m_Line = new Line3.Segment(world.m_Position, world.m_Position + float3);
              iterator.m_Found = false;
              movingObjectSearchTree.Iterate<CreatureUtils.ActivityLocationIterator>(ref iterator);
              if (!iterator.m_Found)
              {
                int num2 = random.NextInt(math.countbits(activityMask2.m_Mask));
                for (int index2 = 1; index2 <= 64; ++index2)
                {
                  ActivityType type = (ActivityType) index2;
                  if (((int) activityMask2.m_Mask & (int) new ActivityMask(type).m_Mask) != 0 && num2-- == 0)
                  {
                    activity = type;
                    break;
                  }
                }
                Game.Objects.Transform activityStartPosition = ObjectUtils.GetActivityStartPosition(creaturePrefab, meshGroups, world, TransformState.Start, activity, activityLocationElement.m_PropID, conditions, ref subMeshGroupBuffers, ref characterElementBuffers, ref subMeshBuffers, ref animationClipBuffers, ref animationMotionBuffers, ref cache);
                float num3 = math.distance(activityStartPosition.m_Position, position) * random.NextFloat(0.5f, 1.5f);
                if ((double) num3 < (double) num1)
                {
                  num1 = num3;
                  result = activityStartPosition;
                }
              }
            }
          }
          return (double) num1 != 3.4028234663852886E+38;
        }
        result.m_Position = transform.m_Position;
        return true;
      }
      if (!positions.HasComponent(entity))
        return false;
      result.m_Position = positions[entity].m_Position;
      return true;
    }

    public static void GetAreaActivity(
      ref Unity.Mathematics.Random random,
      ref ActivityType activity,
      Entity laneEntity,
      ActivityMask activityMask,
      ComponentLookup<Owner> owners,
      ComponentLookup<PrefabRef> prefabRefs,
      ComponentLookup<Game.Prefabs.SpawnLocationData> prefabSpawnLocationDatas)
    {
      if (!owners.HasComponent(laneEntity))
        return;
      Entity owner = owners[laneEntity].m_Owner;
      PrefabRef prefabRef = prefabRefs[owner];
      if (!prefabSpawnLocationDatas.HasComponent(prefabRef.m_Prefab))
        return;
      Game.Prefabs.SpawnLocationData spawnLocationData = prefabSpawnLocationDatas[prefabRef.m_Prefab];
      activityMask.m_Mask &= spawnLocationData.m_ActivityMask.m_Mask;
      if (activityMask.m_Mask == 0U)
        return;
      int num = random.NextInt(math.countbits(activityMask.m_Mask));
      for (int index = 1; index <= 64; ++index)
      {
        ActivityType type = (ActivityType) index;
        if (((int) activityMask.m_Mask & (int) new ActivityMask(type).m_Mask) != 0 && num-- == 0)
        {
          activity = type;
          break;
        }
      }
    }

    public static bool SetTriangleTarget(
      float3 left,
      float3 right,
      float3 next,
      float3 comparePosition,
      PathElement nextElement,
      int elementIndex,
      DynamicBuffer<PathElement> pathElements,
      ref float3 targetPosition,
      float minDistance,
      float lanePosition,
      float curveDelta,
      float navigationSize,
      bool isSingle,
      ComponentLookup<Game.Objects.Transform> transforms,
      ComponentLookup<TaxiStand> taxiStands,
      ComponentLookup<AreaLane> areaLanes,
      ComponentLookup<Curve> curves)
    {
      targetPosition = CreatureUtils.CalculateTriangleTarget(left, right, next, targetPosition, nextElement, elementIndex, pathElements, lanePosition, curveDelta, navigationSize, isSingle, transforms, taxiStands, areaLanes, curves);
      return (double) math.distance(comparePosition, targetPosition) >= (double) minDistance;
    }

    private static float3 CalculateTriangleTarget(
      float3 left,
      float3 right,
      float3 next,
      float3 lastTarget,
      PathElement nextElement,
      int elementIndex,
      DynamicBuffer<PathElement> pathElements,
      float lanePosition,
      float curveDelta,
      float navigationSize,
      bool isSingle,
      ComponentLookup<Game.Objects.Transform> transforms,
      ComponentLookup<TaxiStand> taxiStands,
      ComponentLookup<AreaLane> areaLanes,
      ComponentLookup<Curve> curves)
    {
      if (nextElement.m_Target == Entity.Null && pathElements.IsCreated && elementIndex < pathElements.Length)
        nextElement = pathElements[elementIndex];
      if (nextElement.m_Target != Entity.Null)
      {
        Game.Objects.Transform componentData1;
        if (transforms.TryGetComponent(nextElement.m_Target, out componentData1) && !taxiStands.HasComponent(nextElement.m_Target))
          return CreatureUtils.CalculateTriangleTarget(left, right, next, componentData1.m_Position, navigationSize, isSingle);
        if (areaLanes.HasComponent(nextElement.m_Target))
          return CreatureUtils.CalculateTriangleTarget(left, right, next, lastTarget, navigationSize, isSingle);
        Curve componentData2;
        if (curves.TryGetComponent(nextElement.m_Target, out componentData2))
        {
          float3 target = MathUtils.Position(componentData2.m_Bezier, nextElement.m_TargetDelta.x);
          return CreatureUtils.CalculateTriangleTarget(left, right, next, target, navigationSize, isSingle);
        }
      }
      return CreatureUtils.CalculateTriangleTarget(left, right, next, lanePosition, curveDelta, navigationSize, isSingle);
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
      PathElement nextElement,
      int elementIndex,
      DynamicBuffer<PathElement> pathElements,
      ref float3 targetPosition,
      float minDistance,
      float lanePosition,
      float curveDelta,
      float navigationSize,
      bool isBackward,
      ComponentLookup<Game.Objects.Transform> transforms,
      ComponentLookup<TaxiStand> taxiStands,
      ComponentLookup<AreaLane> areaLanes,
      ComponentLookup<Curve> curves,
      ComponentLookup<Owner> owners)
    {
      float num1 = navigationSize * 0.5f;
      Line3.Segment segment1 = new Line3.Segment(left, right);
      float num2 = 1f / MathUtils.Length(segment1.xz);
      Bounds1 other = new Bounds1(math.min(0.5f, num1 * num2), math.max(0.5f, (float) (1.0 - (double) num1 * (double) num2)));
      int y1 = 0;
      int num3 = elementIndex;
      if (pathElements.IsCreated)
      {
        y1 = pathElements.Length;
        elementIndex = math.min(elementIndex, y1);
      }
      elementIndex -= math.select(0, 1, nextElement.m_Target != Entity.Null);
      int num4 = elementIndex;
      for (; elementIndex < y1; ++elementIndex)
      {
        PathElement pathElement = elementIndex >= num3 ? pathElements[elementIndex] : nextElement;
        Owner componentData;
        if (owners.TryGetComponent(pathElement.m_Target, out componentData) && componentData.m_Owner == areaEntity)
        {
          AreaLane areaLane = areaLanes[pathElement.m_Target];
          bool4 bool4 = new bool4(pathElement.m_TargetDelta < 0.5f, pathElement.m_TargetDelta > 0.5f);
          if (math.any(bool4.xy & bool4.wz))
          {
            Line3.Segment segment2 = new Line3.Segment(comparePosition, nodes[areaLane.m_Nodes.y].m_Position);
            Line3.Segment segment3 = new Line3.Segment(comparePosition, nodes[areaLane.m_Nodes.z].m_Position);
            Bounds1 bounds1_1 = other;
            Bounds1 bounds1_2 = other;
            float2 t;
            if (MathUtils.Intersect((Line2) segment1.xz, (Line2) segment2.xz, out t))
            {
              float num5 = math.max(math.max(0.0f, 0.4f * math.min(t.y, 1f - t.y) * MathUtils.Length(segment2.xz) * num2), math.max(t.x - other.max, other.min - t.x));
              if ((double) num5 < (double) other.max - (double) other.min)
                bounds1_1 = new Bounds1(math.max(other.min, math.min(other.max, t.x) - num5), math.min(other.max, math.max(other.min, t.x) + num5));
            }
            if (MathUtils.Intersect((Line2) segment1.xz, (Line2) segment3.xz, out t))
            {
              float num6 = math.max(math.max(0.0f, 0.4f * math.min(t.y, 1f - t.y) * MathUtils.Length(segment2.xz) * num2), math.max(t.x - other.max, other.min - t.x));
              if ((double) num6 < (double) other.max - (double) other.min)
                bounds1_2 = new Bounds1(math.max(other.min, math.min(other.max, t.x) - num6), math.min(other.max, math.max(other.min, t.x) + num6));
            }
            if (bounds1_1.Equals(other) & bounds1_2.Equals(other))
            {
              elementIndex = y1;
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
        break;
      }
      if (elementIndex - 1 < y1)
      {
        float3 triangleTarget;
        if (elementIndex > num4)
        {
          PathElement pathElement = elementIndex - 1 >= num3 ? pathElements[elementIndex - 1] : nextElement;
          AreaLane areaLane = areaLanes[pathElement.m_Target];
          bool c = (double) pathElement.m_TargetDelta.y > 0.5;
          float3 position1 = nodes[areaLane.m_Nodes.y].m_Position;
          float3 position2 = nodes[areaLane.m_Nodes.z].m_Position;
          float3 position3 = nodes[math.select(areaLane.m_Nodes.x, areaLane.m_Nodes.w, c)].m_Position;
          float num7 = math.select(lanePosition, -lanePosition, (double) pathElement.m_TargetDelta.y < (double) pathElement.m_TargetDelta.x != isBackward);
          float3 right1 = position2;
          float3 next1 = position3;
          float3 lastTarget = targetPosition;
          PathElement nextElement1 = new PathElement();
          int elementIndex1 = elementIndex;
          DynamicBuffer<PathElement> pathElements1 = pathElements;
          double lanePosition1 = (double) num7;
          double y2 = (double) pathElement.m_TargetDelta.y;
          double navigationSize1 = (double) navigationSize;
          ComponentLookup<Game.Objects.Transform> transforms1 = transforms;
          ComponentLookup<TaxiStand> taxiStands1 = taxiStands;
          ComponentLookup<AreaLane> areaLanes1 = areaLanes;
          ComponentLookup<Curve> curves1 = curves;
          triangleTarget = CreatureUtils.CalculateTriangleTarget(position1, right1, next1, lastTarget, nextElement1, elementIndex1, pathElements1, (float) lanePosition1, (float) y2, (float) navigationSize1, false, transforms1, taxiStands1, areaLanes1, curves1);
        }
        else
          triangleTarget = CreatureUtils.CalculateTriangleTarget(left, right, next, targetPosition, nextElement, elementIndex, pathElements, lanePosition, curveDelta, navigationSize, false, transforms, taxiStands, areaLanes, curves);
        Line3.Segment segment4 = new Line3.Segment(comparePosition, triangleTarget);
        float2 t;
        if (MathUtils.Intersect((Line2) segment1.xz, (Line2) segment4.xz, out t))
        {
          float num8 = math.max(math.max(0.0f, 0.4f * math.min(t.y, 1f - t.y) * MathUtils.Length(segment4.xz) * num2), math.max(t.x - other.max, other.min - t.x));
          if ((double) num8 < (double) other.max - (double) other.min)
            other = new Bounds1(math.max(other.min, math.min(other.max, t.x) - num8), math.min(other.max, math.max(other.min, t.x) + num8));
        }
      }
      float lanePosition2 = math.lerp(other.min, other.max, lanePosition + 0.5f);
      bool farEnough;
      targetPosition = CreatureUtils.CalculateAreaTarget(prev2, prev, left, right, comparePosition, minDistance, lanePosition2, navigationSize, out farEnough);
      return farEnough || (double) math.distance(comparePosition, targetPosition) >= (double) minDistance;
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
      float num2 = MathUtils.Distance(line, comparePosition, out t1);
      float t2 = t1 - math.sqrt(math.max(0.0f, (float) ((double) minDistance * (double) minDistance - (double) num2 * (double) num2)) / MathUtils.LengthSquared(line));
      if ((double) t2 >= 0.0)
      {
        farEnough = true;
        return MathUtils.Position(line, t2);
      }
      farEnough = false;
      return line.a;
    }

    public static float GetNavigationSize(ObjectGeometryData prefabObjectGeometryData)
    {
      return prefabObjectGeometryData.m_Bounds.max.x - prefabObjectGeometryData.m_Bounds.min.x;
    }

    public static float GetLaneOffset(
      ObjectGeometryData prefabObjectGeometryData,
      NetLaneData prefabLaneData,
      float lanePosition)
    {
      float navigationSize = CreatureUtils.GetNavigationSize(prefabObjectGeometryData);
      float num = math.max(0.0f, prefabLaneData.m_Width - navigationSize);
      return lanePosition * num;
    }

    public static float3 GetLanePosition(Bezier4x3 curve, float curvePosition, float laneOffset)
    {
      float3 lanePosition = MathUtils.Position(curve, curvePosition);
      float2 forward = math.normalizesafe(MathUtils.Tangent(curve, curvePosition).xz);
      lanePosition.xz += MathUtils.Right(forward) * laneOffset;
      return lanePosition;
    }

    public static float GetMaxBrakingSpeed(
      HumanData prefabHumanData,
      float distance,
      float timeStep)
    {
      float num = timeStep * prefabHumanData.m_Acceleration;
      return math.sqrt((float) ((double) num * (double) num + 2.0 * (double) prefabHumanData.m_Acceleration * (double) distance)) - num;
    }

    public static float GetMaxBrakingSpeed(
      HumanData prefabHumanData,
      float distance,
      float maxResultSpeed,
      float timeStep)
    {
      float num = timeStep * prefabHumanData.m_Acceleration;
      return math.sqrt((float) ((double) num * (double) num + 2.0 * (double) prefabHumanData.m_Acceleration * (double) distance + (double) maxResultSpeed * (double) maxResultSpeed)) - num;
    }

    public static float GetBrakingDistance(HumanData prefabHumanData, float speed, float timeStep)
    {
      return (float) (0.5 * (double) speed * (double) speed / (double) prefabHumanData.m_Acceleration + (double) speed * (double) timeStep);
    }

    public static float GetMaxBrakingSpeed(
      AnimalData prefabAnimalData,
      float distance,
      float timeStep)
    {
      float num = timeStep * prefabAnimalData.m_Acceleration;
      return math.sqrt((float) ((double) num * (double) num + 2.0 * (double) prefabAnimalData.m_Acceleration * (double) distance)) - num;
    }

    public static float GetMaxBrakingSpeed(
      AnimalData prefabAnimalData,
      float distance,
      float maxResultSpeed,
      float timeStep)
    {
      float num = timeStep * prefabAnimalData.m_Acceleration;
      return math.sqrt((float) ((double) num * (double) num + 2.0 * (double) prefabAnimalData.m_Acceleration * (double) distance + (double) maxResultSpeed * (double) maxResultSpeed)) - num;
    }

    public static float GetBrakingDistance(
      AnimalData prefabAnimalData,
      float speed,
      float timeStep)
    {
      return (float) (0.5 * (double) speed * (double) speed / (double) prefabAnimalData.m_Acceleration + (double) speed * (double) timeStep);
    }

    public static Sphere3 GetQueueArea(ObjectGeometryData prefabObjectGeometryData, float3 position)
    {
      Sphere3 queueArea;
      queueArea.radius = (float) (((double) prefabObjectGeometryData.m_Bounds.max.x - (double) prefabObjectGeometryData.m_Bounds.min.x) * 0.5 + 0.25);
      queueArea.position = position;
      return queueArea;
    }

    public static Sphere3 GetQueueArea(
      ObjectGeometryData prefabObjectGeometryData,
      float3 position1,
      float3 position2)
    {
      Sphere3 queueArea;
      queueArea.radius = (float) (((double) prefabObjectGeometryData.m_Bounds.max.x - (double) prefabObjectGeometryData.m_Bounds.min.x + (double) math.distance(position1, position2)) * 0.5 + 0.25);
      queueArea.position = math.lerp(position1, position2, 0.5f);
      return queueArea;
    }

    public static void SetQueue(
      ref Entity queueEntity,
      ref Sphere3 queueArea,
      Entity setEntity,
      Sphere3 setArea)
    {
      if ((double) queueArea.radius > 0.0 && (double) setArea.radius > 0.0 && queueEntity == setEntity)
      {
        queueArea = MathUtils.Sphere(queueArea, setArea);
      }
      else
      {
        queueEntity = setEntity;
        queueArea = setArea;
      }
    }

    public static void FixPathStart(
      ref Unity.Mathematics.Random random,
      float3 position,
      int elementIndex,
      DynamicBuffer<PathElement> path,
      ref ComponentLookup<Owner> ownerData,
      ref ComponentLookup<Lane> laneData,
      ref ComponentLookup<EdgeLane> edgeLaneData,
      ref ComponentLookup<Game.Net.ConnectionLane> connectionLaneData,
      ref ComponentLookup<Curve> curveData,
      ref BufferLookup<Game.Net.SubLane> subLanes,
      ref BufferLookup<Game.Areas.Node> areaNodes,
      ref BufferLookup<Triangle> areaTriangles)
    {
      if (path.Length <= elementIndex)
        return;
      PathElement pathElement = path[elementIndex];
      Game.Net.ConnectionLane componentData;
      if (connectionLaneData.TryGetComponent(pathElement.m_Target, out componentData))
      {
        if ((componentData.m_Flags & ConnectionLaneFlags.Area) == (ConnectionLaneFlags) 0)
          return;
        CreatureUtils.FixPathStart_AreaLane(ref random, position, elementIndex, path, ref ownerData, ref curveData, ref laneData, ref connectionLaneData, ref subLanes, ref areaNodes, ref areaTriangles);
      }
      else
      {
        if (!curveData.HasComponent(pathElement.m_Target))
          return;
        CreatureUtils.FixPathStart_EdgeLane(ref random, position, elementIndex, path, ref ownerData, ref laneData, ref edgeLaneData, ref curveData, ref subLanes);
      }
    }

    private static void FixPathStart_AreaLane(
      ref Unity.Mathematics.Random random,
      float3 position,
      int elementIndex,
      DynamicBuffer<PathElement> path,
      ref ComponentLookup<Owner> ownerData,
      ref ComponentLookup<Curve> curveData,
      ref ComponentLookup<Lane> laneData,
      ref ComponentLookup<Game.Net.ConnectionLane> connectionLaneData,
      ref BufferLookup<Game.Net.SubLane> subLanes,
      ref BufferLookup<Game.Areas.Node> areaNodes,
      ref BufferLookup<Triangle> areaTriangles)
    {
      PathElement pathElement1 = path[elementIndex];
      Entity owner = ownerData[pathElement1.m_Target].m_Owner;
      DynamicBuffer<Game.Areas.Node> nodes = areaNodes[owner];
      DynamicBuffer<Triangle> dynamicBuffer = areaTriangles[owner];
      int index1 = -1;
      float num1 = float.MaxValue;
      float2 t1 = (float2) 0.0f;
      for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
      {
        float2 t2;
        float num2 = MathUtils.Distance(AreaUtils.GetTriangle3(nodes, dynamicBuffer[index2]), position, out t2) + random.NextFloat(0.5f);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          index1 = index2;
          t1 = t2;
        }
      }
      if (index1 == -1)
        return;
      DynamicBuffer<Game.Net.SubLane> lanes = subLanes[owner];
      Triangle3 triangle3 = AreaUtils.GetTriangle3(nodes, dynamicBuffer[index1]);
      float3 position1 = MathUtils.Position(triangle3, t1);
      float num3 = float.MaxValue;
      Entity startEntity = Entity.Null;
      float startCurvePos = 0.0f;
      for (int index3 = 0; index3 < lanes.Length; ++index3)
      {
        Entity subLane = lanes[index3].m_SubLane;
        if (connectionLaneData.HasComponent(subLane) && (connectionLaneData[subLane].m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0)
        {
          Curve curve = curveData[subLane];
          float2 t3;
          bool2 x = new bool2(MathUtils.Intersect(triangle3.xz, curve.m_Bezier.a.xz, out t3), MathUtils.Intersect(triangle3.xz, curve.m_Bezier.d.xz, out t3));
          if (math.any(x))
          {
            float t4;
            float num4 = MathUtils.Distance(curve.m_Bezier, position1, out t4);
            if ((double) num4 < (double) num3)
            {
              float2 float2 = math.select(new float2(0.0f, 0.49f), math.select(new float2(0.51f, 1f), new float2(0.0f, 1f), x.x), x.y);
              num3 = num4;
              startEntity = subLane;
              startCurvePos = math.clamp(t4, float2.x, float2.y);
            }
          }
        }
      }
      if (startEntity == Entity.Null)
      {
        Debug.Log((object) string.Format("Start path lane not found ({0}, {1}, {2})", (object) position.x, (object) position.y, (object) position.z));
      }
      else
      {
        int index4;
        for (index4 = elementIndex; index4 < path.Length - 1; ++index4)
        {
          PathElement pathElement2 = path[index4 + 1];
          Owner componentData;
          if (!ownerData.TryGetComponent(pathElement2.m_Target, out componentData) || componentData.m_Owner != owner)
            break;
        }
        NativeList<PathElement> path1 = new NativeList<PathElement>(lanes.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        PathElement pathElement3 = path[index4];
        AreaUtils.FindAreaPath(ref random, path1, lanes, startEntity, startCurvePos, pathElement3.m_Target, pathElement3.m_TargetDelta.y, laneData, curveData);
        if (path1.Length != 0)
        {
          int x = index4 - elementIndex + 1;
          int num5 = math.min(x, path1.Length);
          for (int index5 = 0; index5 < num5; ++index5)
            path[elementIndex + index5] = path1[index5];
          if (path1.Length < x)
          {
            path.RemoveRange(elementIndex + path1.Length, x - path1.Length);
          }
          else
          {
            for (int index6 = x; index6 < path1.Length; ++index6)
              path.Insert(elementIndex + index6, path1[index6]);
          }
        }
        path1.Dispose();
      }
    }

    private static void FixPathStart_EdgeLane(
      ref Unity.Mathematics.Random random,
      float3 position,
      int elementIndex,
      DynamicBuffer<PathElement> path,
      ref ComponentLookup<Owner> ownerData,
      ref ComponentLookup<Lane> laneData,
      ref ComponentLookup<EdgeLane> edgeLaneData,
      ref ComponentLookup<Curve> curveData,
      ref BufferLookup<Game.Net.SubLane> subLanes)
    {
      PathElement elem = path[elementIndex];
      if (!edgeLaneData.HasComponent(elem.m_Target))
      {
        Lane lane = laneData[elem.m_Target];
        bool startNode = (double) elem.m_TargetDelta.x < 0.5;
        if (!NetUtils.FindEdgeLane(ref elem.m_Target, ref ownerData, ref laneData, ref subLanes, startNode))
          return;
        elem.m_TargetDelta = (float2) (startNode ? lane.m_StartNode.GetCurvePos() : lane.m_EndNode.GetCurvePos());
        path.Insert(elementIndex, elem);
      }
      Curve curve1 = curveData[elem.m_Target];
      Entity entity = elem.m_Target;
      float t1;
      float num1 = MathUtils.Distance(curve1.m_Bezier, position, out t1) + random.NextFloat(0.5f);
      Entity target1 = elem.m_Target;
      if (NetUtils.FindPrevLane(ref target1, ref ownerData, ref laneData, ref subLanes))
      {
        float t2;
        float num2 = MathUtils.Distance(curveData[target1].m_Bezier, position, out t2) + random.NextFloat(0.5f);
        if ((double) num2 < (double) num1)
        {
          entity = target1;
          num1 = num2;
          t1 = t2;
        }
      }
      Entity target2 = elem.m_Target;
      if (NetUtils.FindNextLane(ref target2, ref ownerData, ref laneData, ref subLanes))
      {
        float t3;
        if ((double) (MathUtils.Distance(curveData[target2].m_Bezier, position, out t3) + random.NextFloat(0.5f)) < (double) num1)
        {
          entity = target2;
          t1 = t3;
        }
      }
      Curve curve2 = curveData[entity];
      float length1 = random.NextFloat(-0.5f, 0.5f);
      float x;
      if ((double) length1 >= 0.0)
      {
        Bounds1 t4 = new Bounds1(t1, 1f);
        x = !MathUtils.ClampLength(curve2.m_Bezier.xz, ref t4, length1) ? math.saturate(t1 + (float) ((1.0 - (double) t1) * (double) length1 / 0.5)) : t4.max;
      }
      else
      {
        float length2 = -length1;
        Bounds1 t5 = new Bounds1(0.0f, t1);
        x = !MathUtils.ClampLengthInverse(curve2.m_Bezier.xz, ref t5, length2) ? math.saturate(t1 - (float) ((double) t1 * (double) length2 / 0.5)) : t5.min;
      }
      if (entity == elem.m_Target)
      {
        elem.m_TargetDelta.x = x;
        path[elementIndex] = elem;
      }
      else if (entity == target2)
      {
        if (elementIndex < path.Length - 1 && path[elementIndex + 1].m_Target == target2)
        {
          path.RemoveAt(elementIndex);
          elem = path[elementIndex];
          elem.m_TargetDelta.x = x;
          path[elementIndex] = elem;
        }
        else
        {
          path.Insert(elementIndex + 1, new PathElement()
          {
            m_Target = elem.m_Target,
            m_TargetDelta = new float2(1f, elem.m_TargetDelta.y)
          });
          elem.m_Target = target2;
          elem.m_TargetDelta = new float2(x, 0.0f);
          path[elementIndex] = elem;
        }
      }
      else
      {
        if (!(entity == target1))
          return;
        if (elementIndex < path.Length - 1 && path[elementIndex + 1].m_Target == target1)
        {
          path.RemoveAt(elementIndex);
          elem = path[elementIndex];
          elem.m_TargetDelta.x = x;
          path[elementIndex] = elem;
        }
        else
        {
          path.Insert(elementIndex + 1, new PathElement()
          {
            m_Target = elem.m_Target,
            m_TargetDelta = new float2(0.0f, elem.m_TargetDelta.y)
          });
          elem.m_Target = target1;
          elem.m_TargetDelta = new float2(x, 1f);
          path[elementIndex] = elem;
        }
      }
    }

    public static void FixEnterPath(
      ref Unity.Mathematics.Random random,
      float3 position,
      int elementIndex,
      DynamicBuffer<PathElement> path,
      ref ComponentLookup<Owner> ownerData,
      ref ComponentLookup<Lane> laneData,
      ref ComponentLookup<EdgeLane> edgeLaneData,
      ref ComponentLookup<Game.Net.ConnectionLane> connectionLaneData,
      ref ComponentLookup<Curve> curveData,
      ref BufferLookup<Game.Net.SubLane> subLanes,
      ref BufferLookup<Game.Areas.Node> areaNodes,
      ref BufferLookup<Triangle> areaTriangles)
    {
      if (path.Length <= elementIndex)
        return;
      PathElement pathElement = path[elementIndex];
      Game.Net.ConnectionLane componentData;
      if (connectionLaneData.TryGetComponent(pathElement.m_Target, out componentData))
      {
        if ((componentData.m_Flags & ConnectionLaneFlags.Area) == (ConnectionLaneFlags) 0)
          return;
        CreatureUtils.FixEnterPath_AreaLane(ref random, position, elementIndex, path, ref ownerData, ref curveData, ref laneData, ref connectionLaneData, ref subLanes, ref areaNodes, ref areaTriangles);
      }
      else
      {
        if (!curveData.HasComponent(pathElement.m_Target))
          return;
        CreatureUtils.FixEnterPath_EdgeLane(ref random, position, elementIndex, path, ref ownerData, ref laneData, ref edgeLaneData, ref curveData, ref subLanes);
      }
    }

    private static void FixEnterPath_AreaLane(
      ref Unity.Mathematics.Random random,
      float3 position,
      int elementIndex,
      DynamicBuffer<PathElement> path,
      ref ComponentLookup<Owner> ownerData,
      ref ComponentLookup<Curve> curveData,
      ref ComponentLookup<Lane> laneData,
      ref ComponentLookup<Game.Net.ConnectionLane> connectionLaneData,
      ref BufferLookup<Game.Net.SubLane> subLanes,
      ref BufferLookup<Game.Areas.Node> areaNodes,
      ref BufferLookup<Triangle> areaTriangles)
    {
      PathElement pathElement1 = path[elementIndex];
      Entity owner = ownerData[pathElement1.m_Target].m_Owner;
      DynamicBuffer<Game.Areas.Node> nodes = areaNodes[owner];
      DynamicBuffer<Triangle> dynamicBuffer = areaTriangles[owner];
      int index1 = -1;
      float num1 = float.MaxValue;
      float2 t1 = (float2) 0.0f;
      for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
      {
        float2 t2;
        float num2 = MathUtils.Distance(AreaUtils.GetTriangle3(nodes, dynamicBuffer[index2]), position, out t2) + random.NextFloat(0.5f);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          index1 = index2;
          t1 = t2;
        }
      }
      if (index1 == -1)
        return;
      DynamicBuffer<Game.Net.SubLane> lanes = subLanes[owner];
      Triangle3 triangle3 = AreaUtils.GetTriangle3(nodes, dynamicBuffer[index1]);
      float3 position1 = MathUtils.Position(triangle3, t1);
      float num3 = float.MaxValue;
      Entity endEntity = Entity.Null;
      float endCurvePos = 0.0f;
      for (int index3 = 0; index3 < lanes.Length; ++index3)
      {
        Entity subLane = lanes[index3].m_SubLane;
        if (connectionLaneData.HasComponent(subLane) && (connectionLaneData[subLane].m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0)
        {
          Curve curve = curveData[subLane];
          float2 t3;
          bool2 x = new bool2(MathUtils.Intersect(triangle3.xz, curve.m_Bezier.a.xz, out t3), MathUtils.Intersect(triangle3.xz, curve.m_Bezier.d.xz, out t3));
          if (math.any(x))
          {
            float t4;
            float num4 = MathUtils.Distance(curve.m_Bezier, position1, out t4);
            if ((double) num4 < (double) num3)
            {
              float2 float2 = math.select(new float2(0.0f, 0.49f), math.select(new float2(0.51f, 1f), new float2(0.0f, 1f), x.x), x.y);
              num3 = num4;
              endEntity = subLane;
              endCurvePos = math.clamp(t4, float2.x, float2.y);
            }
          }
        }
      }
      if (endEntity == Entity.Null)
      {
        Debug.Log((object) string.Format("Enter path lane not found ({0}, {1}, {2})", (object) position.x, (object) position.y, (object) position.z));
      }
      else
      {
        NativeList<PathElement> path1 = new NativeList<PathElement>(lanes.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        PathElement pathElement2 = path[elementIndex];
        AreaUtils.FindAreaPath(ref random, path1, lanes, pathElement2.m_Target, pathElement2.m_TargetDelta.x, endEntity, endCurvePos, laneData, curveData);
        if (path1.Length != 0)
        {
          path[elementIndex] = path1[0];
          for (int index4 = 1; index4 < path1.Length; ++index4)
            path.Insert(elementIndex + index4, path1[index4]);
        }
        path1.Dispose();
      }
    }

    private static void FixEnterPath_EdgeLane(
      ref Unity.Mathematics.Random random,
      float3 position,
      int elementIndex,
      DynamicBuffer<PathElement> path,
      ref ComponentLookup<Owner> ownerData,
      ref ComponentLookup<Lane> laneData,
      ref ComponentLookup<EdgeLane> edgeLaneData,
      ref ComponentLookup<Curve> curveData,
      ref BufferLookup<Game.Net.SubLane> subLanes)
    {
      PathElement elem = path[elementIndex];
      if (!edgeLaneData.HasComponent(elem.m_Target))
      {
        Lane lane = laneData[elem.m_Target];
        bool startNode = (double) elem.m_TargetDelta.y < 0.5;
        if (!NetUtils.FindEdgeLane(ref elem.m_Target, ref ownerData, ref laneData, ref subLanes, startNode))
          return;
        elem.m_TargetDelta = (float2) (startNode ? lane.m_StartNode.GetCurvePos() : lane.m_EndNode.GetCurvePos());
        path.Insert(elementIndex + 1, elem);
      }
      Curve curve1 = curveData[elem.m_Target];
      Entity entity = elem.m_Target;
      float t1;
      float num1 = MathUtils.Distance(curve1.m_Bezier, position, out t1) + random.NextFloat(0.5f);
      Entity target1 = elem.m_Target;
      if (NetUtils.FindPrevLane(ref target1, ref ownerData, ref laneData, ref subLanes))
      {
        float t2;
        float num2 = MathUtils.Distance(curveData[target1].m_Bezier, position, out t2) + random.NextFloat(0.5f);
        if ((double) num2 < (double) num1)
        {
          entity = target1;
          num1 = num2;
          t1 = t2;
        }
      }
      Entity target2 = elem.m_Target;
      if (NetUtils.FindNextLane(ref target2, ref ownerData, ref laneData, ref subLanes))
      {
        float t3;
        if ((double) (MathUtils.Distance(curveData[target2].m_Bezier, position, out t3) + random.NextFloat(0.5f)) < (double) num1)
        {
          entity = target2;
          t1 = t3;
        }
      }
      Curve curve2 = curveData[entity];
      float length1 = random.NextFloat(-0.5f, 0.5f);
      float y;
      if ((double) length1 >= 0.0)
      {
        Bounds1 t4 = new Bounds1(t1, 1f);
        y = !MathUtils.ClampLength(curve2.m_Bezier.xz, ref t4, length1) ? math.saturate(t1 + (float) ((1.0 - (double) t1) * (double) length1 / 0.5)) : t4.max;
      }
      else
      {
        float length2 = -length1;
        Bounds1 t5 = new Bounds1(0.0f, t1);
        y = !MathUtils.ClampLengthInverse(curve2.m_Bezier.xz, ref t5, length2) ? math.saturate(t1 - (float) ((double) t1 * (double) length2 / 0.5)) : t5.min;
      }
      if (entity == elem.m_Target)
      {
        elem.m_TargetDelta.y = y;
        path[elementIndex] = elem;
      }
      else if (entity == target2)
      {
        path.Insert(elementIndex + 1, new PathElement()
        {
          m_Target = target2,
          m_TargetDelta = new float2(0.0f, y)
        });
        elem.m_TargetDelta.y = 1f;
        path[elementIndex] = elem;
      }
      else
      {
        if (!(entity == target1))
          return;
        path.Insert(elementIndex + 1, new PathElement()
        {
          m_Target = target1,
          m_TargetDelta = new float2(1f, y)
        });
        elem.m_TargetDelta.y = 0.0f;
        path[elementIndex] = elem;
      }
    }

    public static void SetRandomAreaTarget(
      ref Unity.Mathematics.Random random,
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
      Entity owner = ownerData[pathElement1.m_Target].m_Owner;
      DynamicBuffer<Game.Areas.Node> areaNode = areaNodes[owner];
      DynamicBuffer<Triangle> areaTriangle = areaTriangles[owner];
      int index1 = -1;
      float max = 0.0f;
      for (int index2 = 0; index2 < areaTriangle.Length; ++index2)
      {
        float num = MathUtils.Area(AreaUtils.GetTriangle3(areaNode, areaTriangle[index2]).xz);
        max += num;
        if ((double) random.NextFloat(max) < (double) num)
          index1 = index2;
      }
      if (index1 == -1)
        return;
      DynamicBuffer<Game.Net.SubLane> subLane1 = subLanes[owner];
      float2 float2_1 = random.NextFloat2((float2) 1f);
      float2 t1 = math.select(float2_1, 1f - float2_1, (double) math.csum(float2_1) > 1.0);
      Triangle3 triangle3 = AreaUtils.GetTriangle3(areaNode, areaTriangle[index1]);
      float3 position = MathUtils.Position(triangle3, t1);
      float num1 = float.MaxValue;
      Entity endEntity = Entity.Null;
      float endCurvePos = 0.0f;
      for (int index3 = 0; index3 < subLane1.Length; ++index3)
      {
        Entity subLane2 = subLane1[index3].m_SubLane;
        if (connectionLaneData.HasComponent(subLane2) && (connectionLaneData[subLane2].m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0)
        {
          Curve curve = curveData[subLane2];
          float2 t2;
          bool2 x = new bool2(MathUtils.Intersect(triangle3.xz, curve.m_Bezier.a.xz, out t2), MathUtils.Intersect(triangle3.xz, curve.m_Bezier.d.xz, out t2));
          if (math.any(x))
          {
            float num2 = MathUtils.Distance(curve.m_Bezier, position, out float _);
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
        return;
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

    public static void CheckUnspawned(
      int jobIndex,
      Entity entity,
      HumanCurrentLane currentLane,
      Human human,
      bool isUnspawned,
      EntityCommandBuffer.ParallelWriter commandBuffer)
    {
      if ((currentLane.m_Flags & CreatureLaneFlags.Connection) != (CreatureLaneFlags) 0 || (human.m_Flags & (HumanFlags.Dead | HumanFlags.Carried)) != (HumanFlags) 0)
      {
        if (isUnspawned)
          return;
        commandBuffer.AddComponent<Unspawned>(jobIndex, entity, new Unspawned());
        commandBuffer.AddComponent<BatchesUpdated>(jobIndex, entity, new BatchesUpdated());
      }
      else
      {
        if ((currentLane.m_Flags & CreatureLaneFlags.TransformTarget) != (CreatureLaneFlags) 0 || !(currentLane.m_Lane != Entity.Null) || !isUnspawned)
          return;
        commandBuffer.RemoveComponent<Unspawned>(jobIndex, entity);
        commandBuffer.AddComponent<BatchesUpdated>(jobIndex, entity, new BatchesUpdated());
      }
    }

    public static void CheckUnspawned(
      int jobIndex,
      Entity entity,
      AnimalCurrentLane currentLane,
      Animal animal,
      bool isUnspawned,
      EntityCommandBuffer.ParallelWriter commandBuffer)
    {
      if ((currentLane.m_Flags & CreatureLaneFlags.Connection) != (CreatureLaneFlags) 0)
      {
        if (isUnspawned)
          return;
        commandBuffer.AddComponent<Unspawned>(jobIndex, entity, new Unspawned());
        commandBuffer.AddComponent<BatchesUpdated>(jobIndex, entity, new BatchesUpdated());
      }
      else
      {
        if ((currentLane.m_Flags & CreatureLaneFlags.TransformTarget) != (CreatureLaneFlags) 0 || !(currentLane.m_Lane != Entity.Null) && (animal.m_Flags & AnimalFlags.Roaming) == (AnimalFlags) 0 || !isUnspawned)
          return;
        commandBuffer.RemoveComponent<Unspawned>(jobIndex, entity);
        commandBuffer.AddComponent<BatchesUpdated>(jobIndex, entity, new BatchesUpdated());
      }
    }

    private struct ActivityLocationIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public Entity m_Ignore;
      public Line3.Segment m_Line;
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      public bool m_Found;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        return !this.m_Found && MathUtils.Intersect(MathUtils.Expand(bounds.m_Bounds, (float3) 0.5f), this.m_Line, out float2 _);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
      {
        if (this.m_Found || !MathUtils.Intersect(MathUtils.Expand(bounds.m_Bounds, (float3) 0.5f), this.m_Line, out float2 _) || entity == this.m_Ignore || !this.m_TransformData.HasComponent(entity))
          return;
        this.m_Found |= (double) MathUtils.Distance(this.m_Line, this.m_TransformData[entity].m_Position, out float _) < 0.5;
      }
    }
  }
}
