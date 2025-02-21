// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathfindTargetSeeker`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
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
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Pathfind
{
  public struct PathfindTargetSeeker<TBuffer> where TBuffer : IPathfindTargetBuffer
  {
    public PathfindParameters m_PathfindParameters;
    public SetupQueueTarget m_SetupQueueTarget;
    public TBuffer m_Buffer;
    [ReadOnly]
    public RandomSeed m_RandomSeed;
    [ReadOnly]
    public AirwayHelpers.AirwayData m_AirwayData;
    [ReadOnly]
    public ComponentLookup<Owner> m_Owner;
    [ReadOnly]
    public ComponentLookup<Game.Objects.Transform> m_Transform;
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

    public PathfindTargetSeeker(
      PathfindTargetSeekerData data,
      PathfindParameters pathfindParameters,
      SetupQueueTarget setupQueueTarget,
      TBuffer buffer,
      RandomSeed randomSeed)
    {
      this.m_PathfindParameters = pathfindParameters;
      this.m_SetupQueueTarget = setupQueueTarget;
      this.m_Buffer = buffer;
      this.m_RandomSeed = randomSeed;
      this.m_AirwayData = data.m_AirwayData;
      this.m_Owner = data.m_Owner;
      this.m_Transform = data.m_Transform;
      this.m_Attached = data.m_Attached;
      this.m_SpawnLocation = data.m_SpawnLocation;
      this.m_Stopped = data.m_Stopped;
      this.m_HumanCurrentLane = data.m_HumanCurrentLane;
      this.m_CarCurrentLane = data.m_CarCurrentLane;
      this.m_TrainCurrentLane = data.m_TrainCurrentLane;
      this.m_WatercraftCurrentLane = data.m_WatercraftCurrentLane;
      this.m_AircraftCurrentLane = data.m_AircraftCurrentLane;
      this.m_ParkedCar = data.m_ParkedCar;
      this.m_ParkedTrain = data.m_ParkedTrain;
      this.m_Train = data.m_Train;
      this.m_Airplane = data.m_Airplane;
      this.m_Building = data.m_Building;
      this.m_PropertyRenter = data.m_PropertyRenter;
      this.m_CurrentBuilding = data.m_CurrentBuilding;
      this.m_CurrentTransport = data.m_CurrentTransport;
      this.m_Curve = data.m_Curve;
      this.m_PedestrianLane = data.m_PedestrianLane;
      this.m_ParkingLane = data.m_ParkingLane;
      this.m_CarLane = data.m_CarLane;
      this.m_MasterLane = data.m_MasterLane;
      this.m_SlaveLane = data.m_SlaveLane;
      this.m_ConnectionLane = data.m_ConnectionLane;
      this.m_NodeLane = data.m_NodeLane;
      this.m_LaneConnection = data.m_LaneConnection;
      this.m_RouteLane = data.m_RouteLane;
      this.m_AccessLane = data.m_AccessLane;
      this.m_PrefabRef = data.m_PrefabRef;
      this.m_BuildingData = data.m_BuildingData;
      this.m_CarPathfindData = data.m_CarPathfindData;
      this.m_SpawnLocationData = data.m_SpawnLocationData;
      this.m_NetLaneData = data.m_NetLaneData;
      this.m_CarLaneData = data.m_CarLaneData;
      this.m_ParkingLaneData = data.m_ParkingLaneData;
      this.m_TrackLaneData = data.m_TrackLaneData;
      this.m_SubLane = data.m_SubLane;
      this.m_AreaNode = data.m_AreaNode;
      this.m_AreaTriangle = data.m_AreaTriangle;
      this.m_SpawnLocations = data.m_SpawnLocations;
      this.m_VehicleLayout = data.m_VehicleLayout;
      this.m_CarNavigationLanes = data.m_CarNavigationLanes;
      this.m_WatercraftNavigationLanes = data.m_WatercraftNavigationLanes;
      this.m_AircraftNavigationLanes = data.m_AircraftNavigationLanes;
    }

    public void AddTarget(
      ref Unity.Mathematics.Random random,
      Entity target,
      Entity entity,
      float delta,
      float cost,
      EdgeFlags flags)
    {
      cost += random.NextFloat(this.m_SetupQueueTarget.m_RandomCost);
      this.m_Buffer.Enqueue(new PathTarget(target, entity, delta, cost, flags));
    }

    public int FindTargets(Entity entity, float cost)
    {
      return this.FindTargets(entity, entity, cost, EdgeFlags.DefaultMask, true, false);
    }

    public int FindTargets(
      Entity target,
      Entity entity,
      float cost,
      EdgeFlags flags,
      bool allowAccessRestriction,
      bool navigationEnd)
    {
      Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(entity.Index);
      int targets = 0;
      if ((this.m_PathfindParameters.m_PathfindFlags & PathfindFlags.SkipPathfind) != (PathfindFlags) 0)
      {
        this.AddTarget(ref random, target, entity, 0.0f, cost, flags);
        return 1;
      }
      if (this.m_CurrentTransport.HasComponent(entity))
        entity = this.m_CurrentTransport[entity].m_CurrentTransport;
      if (this.m_HumanCurrentLane.HasComponent(entity))
      {
        HumanCurrentLane humanCurrentLane = this.m_HumanCurrentLane[entity];
        if (this.m_Curve.HasComponent(humanCurrentLane.m_Lane))
        {
          if ((this.m_SetupQueueTarget.m_Methods & PathMethod.Pedestrian) != (PathMethod) 0)
            return targets + this.AddPedestrianLaneTargets(ref random, target, humanCurrentLane.m_Lane, humanCurrentLane.m_CurvePosition.y, cost, 0.0f, flags, allowAccessRestriction);
          float3 comparePosition = MathUtils.Position(this.m_Curve[humanCurrentLane.m_Lane].m_Bezier, humanCurrentLane.m_CurvePosition.y);
          Entity edge;
          if (this.GetEdge(humanCurrentLane.m_Lane, out edge))
            return targets + this.AddEdgeTargets(ref random, target, cost, flags, edge, comparePosition, 0.0f, false, allowAccessRestriction);
          entity = humanCurrentLane.m_Lane;
        }
        else if (this.m_SpawnLocation.HasComponent(humanCurrentLane.m_Lane))
        {
          targets += this.AddSpawnLocation(ref random, target, humanCurrentLane.m_Lane, cost, flags, true, allowAccessRestriction);
          if (targets != 0)
            return targets;
          entity = humanCurrentLane.m_Lane;
        }
        else
          entity = humanCurrentLane.m_Lane;
      }
      if (this.m_CarCurrentLane.HasComponent(entity))
      {
        Entity lane;
        float curvePos;
        Game.Vehicles.CarLaneFlags flags1;
        this.GetCarLane(entity, navigationEnd, out lane, out curvePos, out flags1);
        if (this.m_Curve.HasComponent(lane))
        {
          float3 comparePosition = MathUtils.Position(this.m_Curve[lane].m_Bezier, curvePos);
          if ((this.m_SetupQueueTarget.m_Methods & PathMethod.Road) != (PathMethod) 0 && (this.m_SetupQueueTarget.m_RoadTypes & RoadTypes.Car) != RoadTypes.None)
            return targets + this.AddCarLaneTargets(ref random, target, lane, comparePosition, 0.0f, curvePos, cost, flags, (flags1 & (Game.Vehicles.CarLaneFlags.EnteringRoad | Game.Vehicles.CarLaneFlags.IsBlocked)) > (Game.Vehicles.CarLaneFlags) 0, this.m_Stopped.HasComponent(entity), allowAccessRestriction);
          Entity edge;
          if (this.GetEdge(lane, out edge))
            return targets + this.AddEdgeTargets(ref random, target, cost, flags, edge, comparePosition, 0.0f, false, allowAccessRestriction);
          entity = lane;
        }
        else if (this.m_SpawnLocation.HasComponent(lane))
        {
          targets += this.AddSpawnLocation(ref random, target, lane, cost, flags, true, allowAccessRestriction);
          if (targets != 0)
            return targets;
          entity = lane;
        }
        else
          entity = lane;
      }
      ParkedCar componentData1;
      if (this.m_ParkedCar.TryGetComponent(entity, out componentData1))
      {
        Curve componentData2;
        if (this.m_Curve.TryGetComponent(componentData1.m_Lane, out componentData2))
        {
          if ((this.m_SetupQueueTarget.m_Methods & (PathMethod.Parking | PathMethod.SpecialParking)) != (PathMethod) 0)
            return targets + this.AddParkingLaneTargets(ref random, target, componentData1.m_Lane, componentData1.m_CurvePosition, cost, flags, allowAccessRestriction);
          float3 comparePosition = MathUtils.Position(componentData2.m_Bezier, componentData1.m_CurvePosition);
          LaneConnection componentData3;
          if (this.m_LaneConnection.TryGetComponent(componentData1.m_Lane, out componentData3))
            return targets + this.AddLaneConnectionTargets(ref random, target, cost, flags, componentData3, comparePosition, 0.0f, true, allowAccessRestriction);
          Entity edge;
          if (this.GetEdge(componentData1.m_Lane, out edge))
            return targets + this.AddEdgeTargets(ref random, target, cost, flags, edge, comparePosition, 0.0f, false, allowAccessRestriction);
          entity = componentData1.m_Lane;
        }
        else if (this.m_SpawnLocation.HasComponent(componentData1.m_Lane))
        {
          targets += this.AddSpawnLocation(ref random, target, componentData1.m_Lane, cost, flags, false, allowAccessRestriction);
          if (targets != 0)
            return targets;
          entity = componentData1.m_Lane;
        }
        else
          entity = componentData1.m_Lane;
      }
      if (this.m_WatercraftCurrentLane.HasComponent(entity))
      {
        Entity lane;
        float curvePos;
        WatercraftLaneFlags flags2;
        this.GetWatercraftLane(entity, navigationEnd, out lane, out curvePos, out flags2);
        if (this.m_Curve.HasComponent(lane))
        {
          float3 comparePosition = MathUtils.Position(this.m_Curve[lane].m_Bezier, curvePos);
          return targets + this.AddCarLaneTargets(ref random, target, lane, comparePosition, 0.0f, curvePos, cost, flags, (flags2 & WatercraftLaneFlags.IsBlocked) > (WatercraftLaneFlags) 0, this.m_Stopped.HasComponent(entity), allowAccessRestriction);
        }
        entity = lane;
      }
      if (this.m_AircraftCurrentLane.HasComponent(entity))
      {
        Entity lane1;
        float curvePos;
        AircraftLaneFlags flags3;
        this.GetAircraftLane(entity, navigationEnd, out lane1, out curvePos, out flags3);
        if ((flags3 & (AircraftLaneFlags.TransformTarget | AircraftLaneFlags.Flying)) != (AircraftLaneFlags) 0)
        {
          if (this.m_Transform.HasComponent(lane1))
          {
            float3 position = this.m_Transform[lane1].m_Position;
            AirwayHelpers.AirwayMap airwayMap = this.m_Airplane.HasComponent(entity) ? this.m_AirwayData.airplaneMap : this.m_AirwayData.helicopterMap;
            Entity lane2 = Entity.Null;
            float maxValue = float.MaxValue;
            airwayMap.FindClosestLane(position, this.m_Curve, ref lane2, ref curvePos, ref maxValue);
            if (lane2 != Entity.Null)
            {
              this.AddTarget(ref random, entity, lane2, curvePos, cost, ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary));
              ++targets;
            }
          }
          else if (this.m_Curve.HasComponent(lane1))
          {
            this.AddTarget(ref random, entity, lane1, curvePos, cost, flags);
            ++targets;
          }
          return targets;
        }
        if (this.m_Curve.HasComponent(lane1))
        {
          float3 comparePosition = MathUtils.Position(this.m_Curve[lane1].m_Bezier, curvePos);
          return targets + this.AddCarLaneTargets(ref random, target, lane1, comparePosition, 0.0f, curvePos, cost, flags, true, this.m_Stopped.HasComponent(entity), allowAccessRestriction);
        }
        entity = lane1;
      }
      if (this.m_Train.HasComponent(entity))
        return targets + this.AddTrainTargets(ref random, target, entity, cost, flags);
      bool flag = false;
      if (this.m_RouteLane.HasComponent(entity))
      {
        RouteLane routeLane = this.m_RouteLane[entity];
        if (routeLane.m_StartLane != Entity.Null)
          targets += this.AddLaneTarget(ref random, target, Entity.Null, routeLane.m_StartLane, routeLane.m_StartCurvePos, cost, flags, allowAccessRestriction);
        flag = true;
      }
      if (this.m_AccessLane.HasComponent(entity))
      {
        AccessLane accessLane = this.m_AccessLane[entity];
        if (accessLane.m_Lane != Entity.Null)
        {
          if (this.m_SpawnLocation.HasComponent(accessLane.m_Lane))
            targets += this.AddSpawnLocation(ref random, target, accessLane.m_Lane, cost, flags, true, allowAccessRestriction);
          else
            targets += this.AddLaneTarget(ref random, target, Entity.Null, accessLane.m_Lane, accessLane.m_CurvePos, cost, flags, allowAccessRestriction);
        }
        flag = true;
      }
      if (flag && targets != 0)
        return targets;
      if (this.m_Attached.HasComponent(entity) && !this.m_Building.HasComponent(entity))
      {
        Attached attached = this.m_Attached[entity];
        if (this.m_SubLane.HasBuffer(attached.m_Parent))
        {
          Game.Objects.Transform transform = this.m_Transform[entity];
          return targets + this.AddEdgeTargets(ref random, target, cost, flags, attached.m_Parent, transform.m_Position, 0.0f, false, allowAccessRestriction);
        }
      }
      Entity entity1;
      for (entity1 = !this.m_PropertyRenter.HasComponent(entity) ? (!this.m_CurrentBuilding.HasComponent(entity) ? entity : this.m_CurrentBuilding[entity].m_CurrentBuilding) : this.m_PropertyRenter[entity].m_Property; !this.m_Building.HasComponent(entity1); entity1 = this.m_Owner[entity1].m_Owner)
      {
        if (this.m_SubLane.HasBuffer(entity1))
          targets += this.AddSubLaneTargets(ref random, target, entity1, cost, flags);
        if (!this.m_Owner.HasComponent(entity1))
          return targets;
      }
      bool addFrontConnection = targets == 0;
      if ((this.m_PathfindParameters.m_PathfindFlags & PathfindFlags.Simplified) == (PathfindFlags) 0 && this.m_SpawnLocations.HasBuffer(entity1))
      {
        DynamicBuffer<SpawnLocationElement> spawnLocation1 = this.m_SpawnLocations[entity1];
        int num1 = 0;
        if ((double) this.m_SetupQueueTarget.m_RandomCost != 0.0)
        {
          int max = 0;
          for (int index = 0; index < spawnLocation1.Length; ++index)
          {
            Entity spawnLocation2 = spawnLocation1[index].m_SpawnLocation;
            if (spawnLocation1[index].m_Type == SpawnLocationType.ParkingLane)
              max += this.AddParkingLane(ref random, target, spawnLocation2, cost, flags, allowAccessRestriction, true, ref addFrontConnection);
            else
              max += this.AddSpawnLocation(ref random, target, spawnLocation2, cost, flags, false, allowAccessRestriction, true, false, ref addFrontConnection);
          }
          num1 = random.NextInt(max);
        }
        for (int index = 0; index < spawnLocation1.Length; ++index)
        {
          Entity spawnLocation3 = spawnLocation1[index].m_SpawnLocation;
          float cost1 = math.select(cost, cost + this.m_SetupQueueTarget.m_RandomCost, num1 != 0);
          int num2 = spawnLocation1[index].m_Type != SpawnLocationType.ParkingLane ? this.AddSpawnLocation(ref random, target, spawnLocation3, cost1, flags, false, allowAccessRestriction, false, false, ref addFrontConnection) : this.AddParkingLane(ref random, target, spawnLocation3, cost, flags, allowAccessRestriction, false, ref addFrontConnection);
          targets += num2;
          num1 -= num2;
        }
      }
      if (addFrontConnection)
      {
        PrefabRef prefabRef = this.m_PrefabRef[entity1];
        Game.Objects.Transform transform = this.m_Transform[entity1];
        Building building = this.m_Building[entity1];
        if (building.m_RoadEdge != Entity.Null)
        {
          BuildingData buildingData = this.m_BuildingData[prefabRef.m_Prefab];
          float3 comparePosition = transform.m_Position;
          Owner componentData4;
          if (!this.m_Owner.TryGetComponent(building.m_RoadEdge, out componentData4) || componentData4.m_Owner != entity1)
            comparePosition = BuildingUtils.CalculateFrontPosition(transform, buildingData.m_LotSize.y);
          targets += this.AddEdgeTargets(ref random, target, cost, flags, building.m_RoadEdge, comparePosition, 0.0f, true, false);
        }
      }
      return targets;
    }

    private bool CheckAccessRestriction(bool allowAccessRestriction, Game.Objects.SpawnLocation spawnLocation)
    {
      return allowAccessRestriction || spawnLocation.m_AccessRestriction == Entity.Null || (spawnLocation.m_Flags & SpawnLocationFlags.AllowEnter) != 0;
    }

    private bool CheckAccessRestriction(bool allowAccessRestriction, Game.Net.PedestrianLane pedestrianLane)
    {
      return allowAccessRestriction || pedestrianLane.m_AccessRestriction == Entity.Null || (pedestrianLane.m_Flags & PedestrianLaneFlags.AllowEnter) != 0;
    }

    private bool CheckAccessRestriction(bool allowAccessRestriction, Game.Net.CarLane carLane)
    {
      return allowAccessRestriction || carLane.m_AccessRestriction == Entity.Null || (carLane.m_Flags & Game.Net.CarLaneFlags.AllowEnter) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
    }

    private bool CheckAccessRestriction(bool allowAccessRestriction, Game.Net.ParkingLane parkingLane)
    {
      return allowAccessRestriction || parkingLane.m_AccessRestriction == Entity.Null || (parkingLane.m_Flags & ParkingLaneFlags.AllowEnter) != 0;
    }

    private bool CheckAccessRestriction(bool allowAccessRestriction, Game.Net.ConnectionLane connectionLane)
    {
      return allowAccessRestriction || connectionLane.m_AccessRestriction == Entity.Null || (connectionLane.m_Flags & ConnectionLaneFlags.AllowEnter) != 0;
    }

    private int AddParkingLane(
      ref Unity.Mathematics.Random random,
      Entity target,
      Entity parkingLaneEntity,
      float cost,
      EdgeFlags flags,
      bool allowAccessRestriction,
      bool countOnly,
      ref bool addFrontConnection)
    {
      ParkingLaneData componentData;
      if ((this.m_SetupQueueTarget.m_Methods & (PathMethod.Parking | PathMethod.SpecialParking)) == (PathMethod) 0 || !this.m_ParkingLaneData.TryGetComponent(this.m_PrefabRef[parkingLaneEntity].m_Prefab, out componentData) || (this.m_SetupQueueTarget.m_RoadTypes & componentData.m_RoadTypes) == RoadTypes.None)
        return 0;
      Game.Net.ParkingLane parkingLane = this.m_ParkingLane[parkingLaneEntity];
      PathMethod pathMethod = (parkingLane.m_Flags & ParkingLaneFlags.SpecialVehicles) != (ParkingLaneFlags) 0 ? PathMethod.SpecialParking : PathMethod.Parking;
      float x = VehicleUtils.GetParkingSize(componentData).x;
      float y = math.max(1f, parkingLane.m_FreeSpace);
      if ((this.m_SetupQueueTarget.m_Methods & pathMethod) == (PathMethod) 0 || math.any(this.m_PathfindParameters.m_ParkingSize > new float2(x, y)) || !this.CheckAccessRestriction(allowAccessRestriction, parkingLane))
        return 0;
      if (!countOnly)
      {
        this.AddTarget(ref random, target, parkingLaneEntity, 0.5f, cost, flags);
        addFrontConnection = false;
      }
      return 1;
    }

    private int AddSpawnLocation(
      ref Unity.Mathematics.Random random,
      Entity target,
      Entity spawnLocationEntity,
      float cost,
      EdgeFlags flags,
      bool ignoreActivityMask,
      bool allowAccessRestriction)
    {
      bool addFrontConnection = false;
      return this.AddSpawnLocation(ref random, target, spawnLocationEntity, cost, flags, ignoreActivityMask, allowAccessRestriction, false, true, ref addFrontConnection);
    }

    private int AddSpawnLocation(
      ref Unity.Mathematics.Random random,
      Entity target,
      Entity spawnLocationEntity,
      float cost,
      EdgeFlags flags,
      bool ignoreActivityMask,
      bool allowAccessRestriction,
      bool countOnly,
      bool ignoreParked,
      ref bool addFrontConnection)
    {
      Game.Prefabs.SpawnLocationData componentData1;
      if (!this.m_SpawnLocationData.TryGetComponent(this.m_PrefabRef[spawnLocationEntity].m_Prefab, out componentData1))
        return 0;
      switch (componentData1.m_ConnectionType)
      {
        case RouteConnectionType.Road:
          if ((this.m_SetupQueueTarget.m_Methods & PathMethod.Road) == (PathMethod) 0 || (this.m_SetupQueueTarget.m_RoadTypes & componentData1.m_RoadTypes) == RoadTypes.None)
            return 0;
          if (!ignoreParked && (this.m_SetupQueueTarget.m_Methods & PathMethod.SpecialParking) != (PathMethod) 0)
          {
            cost += this.m_SetupQueueTarget.m_RandomCost;
            break;
          }
          break;
        case RouteConnectionType.Pedestrian:
          if ((this.m_SetupQueueTarget.m_Methods & PathMethod.Pedestrian) == (PathMethod) 0)
            return 0;
          break;
        case RouteConnectionType.Track:
          if ((this.m_SetupQueueTarget.m_Methods & PathMethod.Track) == (PathMethod) 0 || (this.m_SetupQueueTarget.m_TrackTypes & componentData1.m_TrackTypes) == TrackTypes.None)
            return 0;
          break;
        case RouteConnectionType.Cargo:
          if ((this.m_SetupQueueTarget.m_Methods & PathMethod.CargoLoading) == (PathMethod) 0 || (this.m_SetupQueueTarget.m_RoadTypes & componentData1.m_RoadTypes) == RoadTypes.None)
            return 0;
          break;
        case RouteConnectionType.Air:
          if ((this.m_SetupQueueTarget.m_Methods & PathMethod.Road) == (PathMethod) 0 || (this.m_SetupQueueTarget.m_RoadTypes & componentData1.m_RoadTypes) == RoadTypes.None)
            return 0;
          break;
        case RouteConnectionType.Parking:
          if ((this.m_SetupQueueTarget.m_Methods & PathMethod.Parking) == (PathMethod) 0 || (this.m_SetupQueueTarget.m_RoadTypes & componentData1.m_RoadTypes) == RoadTypes.None)
            return 0;
          break;
        default:
          return 0;
      }
      if (!ignoreActivityMask && componentData1.m_ActivityMask.m_Mask != 0U && ((int) componentData1.m_ActivityMask.m_Mask & (int) this.m_SetupQueueTarget.m_ActivityMask.m_Mask) == 0)
        return 0;
      Game.Objects.SpawnLocation componentData2;
      if (this.m_SpawnLocation.TryGetComponent(spawnLocationEntity, out componentData2))
      {
        if (this.CheckAccessRestriction(allowAccessRestriction, componentData2))
        {
          if (!countOnly)
          {
            cost += math.select(math.max(this.m_SetupQueueTarget.m_RandomCost * 3f, 30f), 0.0f, ignoreParked || (componentData2.m_Flags & SpawnLocationFlags.ParkedVehicle) == (SpawnLocationFlags) 0);
            this.AddTarget(ref random, target, spawnLocationEntity, 1f, cost, flags);
            addFrontConnection &= componentData2.m_ConnectedLane1 == Entity.Null;
          }
          return 1;
        }
      }
      else
      {
        DynamicBuffer<Game.Net.SubLane> bufferData;
        if (this.m_SubLane.TryGetBuffer(spawnLocationEntity, out bufferData))
        {
          int num = 0;
          int2 int2 = new int2(0, bufferData.Length - 1);
          if (bufferData.Length == 0)
            Debug.Log((object) string.Format("Empty subLanes: {0}", (object) spawnLocationEntity.Index));
          else if ((double) this.m_SetupQueueTarget.m_RandomCost != 0.0)
            int2 = (int2) random.NextInt(bufferData.Length);
          for (int x = int2.x; x <= int2.y; ++x)
          {
            Entity subLane = bufferData[x].m_SubLane;
            Game.Net.ConnectionLane componentData3;
            if (this.m_ConnectionLane.TryGetComponent(subLane, out componentData3) && this.CheckAccessRestriction(allowAccessRestriction, componentData3) && ((this.m_SetupQueueTarget.m_Methods & PathMethod.Pedestrian) != (PathMethod) 0 && (componentData3.m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0 || (this.m_SetupQueueTarget.m_Methods & PathMethod.Offroad) != (PathMethod) 0 && (componentData3.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0 && (this.m_SetupQueueTarget.m_RoadTypes & componentData3.m_RoadTypes) != RoadTypes.None))
            {
              if (!countOnly)
              {
                this.AddTarget(ref random, target, subLane, 0.5f, cost, flags);
                addFrontConnection = false;
              }
              ++num;
            }
          }
          return num;
        }
      }
      return 0;
    }

    private void GetCarLane(
      Entity entity,
      bool navigationEnd,
      out Entity lane,
      out float curvePos,
      out Game.Vehicles.CarLaneFlags flags)
    {
      CarCurrentLane carCurrentLane = this.m_CarCurrentLane[entity];
      lane = carCurrentLane.m_Lane;
      curvePos = math.select(carCurrentLane.m_CurvePosition.y, carCurrentLane.m_CurvePosition.z, navigationEnd || (carCurrentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.ClearedForPathfind) > (Game.Vehicles.CarLaneFlags) 0);
      flags = carCurrentLane.m_LaneFlags;
      DynamicBuffer<CarNavigationLane> bufferData;
      if (!this.m_CarNavigationLanes.TryGetBuffer(entity, out bufferData))
        return;
      if (navigationEnd)
      {
        if (bufferData.Length == 0)
          return;
        CarNavigationLane carNavigationLane = bufferData[bufferData.Length - 1];
        lane = carNavigationLane.m_Lane;
        curvePos = carNavigationLane.m_CurvePosition.y;
        flags = carNavigationLane.m_Flags;
      }
      else
      {
        for (int index = 0; index < bufferData.Length; ++index)
        {
          CarNavigationLane carNavigationLane = bufferData[index];
          if ((carNavigationLane.m_Flags & (Game.Vehicles.CarLaneFlags.Reserved | Game.Vehicles.CarLaneFlags.ClearedForPathfind)) == (Game.Vehicles.CarLaneFlags) 0)
            break;
          lane = carNavigationLane.m_Lane;
          curvePos = carNavigationLane.m_CurvePosition.y;
          flags = carNavigationLane.m_Flags;
        }
      }
    }

    private void GetWatercraftLane(
      Entity entity,
      bool navigationEnd,
      out Entity lane,
      out float curvePos,
      out WatercraftLaneFlags flags)
    {
      WatercraftCurrentLane watercraftCurrentLane = this.m_WatercraftCurrentLane[entity];
      lane = watercraftCurrentLane.m_Lane;
      curvePos = math.select(watercraftCurrentLane.m_CurvePosition.y, watercraftCurrentLane.m_CurvePosition.z, navigationEnd);
      flags = watercraftCurrentLane.m_LaneFlags;
      if (!this.m_WatercraftNavigationLanes.HasBuffer(entity))
        return;
      DynamicBuffer<WatercraftNavigationLane> watercraftNavigationLane1 = this.m_WatercraftNavigationLanes[entity];
      if (navigationEnd)
      {
        if (watercraftNavigationLane1.Length == 0)
          return;
        WatercraftNavigationLane watercraftNavigationLane2 = watercraftNavigationLane1[watercraftNavigationLane1.Length - 1];
        lane = watercraftNavigationLane2.m_Lane;
        curvePos = watercraftNavigationLane2.m_CurvePosition.y;
        flags = watercraftNavigationLane2.m_Flags;
      }
      else
      {
        for (int index = 0; index < watercraftNavigationLane1.Length; ++index)
        {
          WatercraftNavigationLane watercraftNavigationLane3 = watercraftNavigationLane1[index];
          if ((watercraftNavigationLane3.m_Flags & WatercraftLaneFlags.Reserved) == (WatercraftLaneFlags) 0)
            break;
          lane = watercraftNavigationLane3.m_Lane;
          curvePos = watercraftNavigationLane3.m_CurvePosition.y;
          flags = watercraftNavigationLane3.m_Flags;
        }
      }
    }

    private void GetAircraftLane(
      Entity entity,
      bool navigationEnd,
      out Entity lane,
      out float curvePos,
      out AircraftLaneFlags flags)
    {
      AircraftCurrentLane aircraftCurrentLane = this.m_AircraftCurrentLane[entity];
      lane = aircraftCurrentLane.m_Lane;
      curvePos = math.select(aircraftCurrentLane.m_CurvePosition.y, aircraftCurrentLane.m_CurvePosition.z, navigationEnd);
      flags = aircraftCurrentLane.m_LaneFlags;
      if (!this.m_AircraftNavigationLanes.HasBuffer(entity))
        return;
      DynamicBuffer<AircraftNavigationLane> aircraftNavigationLane1 = this.m_AircraftNavigationLanes[entity];
      if (navigationEnd)
      {
        if (aircraftNavigationLane1.Length == 0)
          return;
        AircraftNavigationLane aircraftNavigationLane2 = aircraftNavigationLane1[aircraftNavigationLane1.Length - 1];
        lane = aircraftNavigationLane2.m_Lane;
        curvePos = aircraftNavigationLane2.m_CurvePosition.y;
        flags = aircraftNavigationLane2.m_Flags;
      }
      else
      {
        for (int index = 0; index < aircraftNavigationLane1.Length; ++index)
        {
          AircraftNavigationLane aircraftNavigationLane3 = aircraftNavigationLane1[index];
          if ((aircraftNavigationLane3.m_Flags & AircraftLaneFlags.Reserved) == (AircraftLaneFlags) 0)
            break;
          lane = aircraftNavigationLane3.m_Lane;
          curvePos = aircraftNavigationLane3.m_CurvePosition.y;
          flags = aircraftNavigationLane3.m_Flags;
        }
      }
    }

    private bool GetEdge(Entity lane, out Entity edge)
    {
      if (this.m_Owner.HasComponent(lane))
      {
        Owner owner = this.m_Owner[lane];
        if (this.m_SubLane.HasBuffer(owner.m_Owner))
        {
          edge = owner.m_Owner;
          return true;
        }
      }
      edge = Entity.Null;
      return false;
    }

    private int AddSubLaneTargets(
      ref Unity.Mathematics.Random random,
      Entity target,
      Entity entity,
      float cost,
      EdgeFlags flags)
    {
      int num = 0;
      Entity entity1 = entity;
      while (this.m_Owner.HasComponent(entity1))
        entity1 = this.m_Owner[entity1].m_Owner;
      DynamicBuffer<Game.Net.SubLane> dynamicBuffer = this.m_SubLane[entity];
      for (int index = 0; index < dynamicBuffer.Length; ++index)
      {
        Entity subLane = dynamicBuffer[index].m_SubLane;
        num += this.AddLaneTarget(ref random, target, entity1, subLane, 0.5f, cost, flags, false);
      }
      return num;
    }

    public int AddAreaTargets(
      ref Unity.Mathematics.Random random,
      Entity target,
      Entity entity,
      Entity subItem,
      DynamicBuffer<Game.Areas.SubArea> subAreas,
      float cost,
      bool addDistanceCost,
      EdgeFlags flags)
    {
      if (!this.m_Transform.HasComponent(subItem))
      {
        int num = 0;
        if (this.m_SubLane.HasBuffer(entity))
          num += this.AddSubLaneTargets(ref random, target, entity, cost, flags);
        if (subAreas.IsCreated)
        {
          for (int index = 0; index < subAreas.Length; ++index)
          {
            Game.Areas.SubArea subArea = subAreas[index];
            if (this.m_SubLane.HasBuffer(subArea.m_Area))
              num += this.AddSubLaneTargets(ref random, target, subArea.m_Area, cost, flags);
          }
        }
        return num;
      }
      Game.Objects.Transform transform = this.m_Transform[subItem];
      int num1 = 0;
      if (subAreas.IsCreated)
        num1 = subAreas.Length;
      float b = float.MaxValue;
      Entity entity1 = Entity.Null;
      float delta = 0.0f;
      for (int index1 = -1; index1 < num1; ++index1)
      {
        if (index1 >= 0)
          entity = subAreas[index1].m_Area;
        DynamicBuffer<Game.Net.SubLane> bufferData;
        if (this.m_SubLane.TryGetBuffer(entity, out bufferData) && bufferData.Length != 0)
        {
          DynamicBuffer<Game.Areas.Node> nodes = this.m_AreaNode[entity];
          DynamicBuffer<Triangle> dynamicBuffer = this.m_AreaTriangle[entity];
          float num2 = b;
          float3 position = transform.m_Position;
          Triangle3 triangle3_1 = new Triangle3();
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            Triangle3 triangle3_2 = AreaUtils.GetTriangle3(nodes, dynamicBuffer[index2]);
            float2 t;
            float num3 = MathUtils.Distance(triangle3_2, transform.m_Position, out t);
            if ((double) num3 < (double) num2)
            {
              num2 = num3;
              position = MathUtils.Position(triangle3_2, t);
              triangle3_1 = triangle3_2;
            }
          }
          if ((double) num2 != (double) b)
          {
            float num4 = float.MaxValue;
            for (int index3 = 0; index3 < bufferData.Length; ++index3)
            {
              Entity subLane = bufferData[index3].m_SubLane;
              if (this.m_ConnectionLane.HasComponent(subLane))
              {
                Game.Net.ConnectionLane connectionLane = this.m_ConnectionLane[subLane];
                if ((this.m_SetupQueueTarget.m_Methods & PathMethod.Pedestrian) != (PathMethod) 0 && (connectionLane.m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0 || (this.m_SetupQueueTarget.m_Methods & PathMethod.Offroad) != (PathMethod) 0 && (connectionLane.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0 && (this.m_SetupQueueTarget.m_RoadTypes & connectionLane.m_RoadTypes) != RoadTypes.None)
                {
                  Curve curve = this.m_Curve[subLane];
                  float2 t1;
                  if (MathUtils.Intersect(triangle3_1.xz, curve.m_Bezier.a.xz, out t1) || MathUtils.Intersect(triangle3_1.xz, curve.m_Bezier.d.xz, out t1))
                  {
                    float t2;
                    float num5 = MathUtils.Distance(curve.m_Bezier, position, out t2);
                    if ((double) num5 < (double) num4)
                    {
                      b = num2;
                      entity1 = subLane;
                      delta = t2;
                      num4 = num5;
                    }
                  }
                }
              }
            }
          }
        }
      }
      if (!(entity1 != Entity.Null))
        return 0;
      cost += math.select(0.0f, b, addDistanceCost);
      this.AddTarget(ref random, target, entity1, delta, cost, flags);
      return 1;
    }

    private int AddLaneTarget(
      ref Unity.Mathematics.Random random,
      Entity target,
      Entity accessRequirement,
      Entity lane,
      float curvePos,
      float cost,
      EdgeFlags flags,
      bool allowAccessRestriction)
    {
      if ((this.m_SetupQueueTarget.m_Methods & PathMethod.Pedestrian) != (PathMethod) 0 && this.m_PedestrianLane.HasComponent(lane))
      {
        Game.Net.PedestrianLane pedestrianLane = this.m_PedestrianLane[lane];
        if (this.CheckAccessRestriction(allowAccessRestriction, pedestrianLane) || pedestrianLane.m_AccessRestriction == accessRequirement)
        {
          this.AddTarget(ref random, target, lane, curvePos, cost, flags);
          return 1;
        }
      }
      if ((this.m_SetupQueueTarget.m_Methods & PathMethod.Road) != (PathMethod) 0 && this.m_CarLane.HasComponent(lane) && !this.m_SlaveLane.HasComponent(lane))
      {
        Game.Net.CarLane carLane = this.m_CarLane[lane];
        if ((this.CheckAccessRestriction(allowAccessRestriction, carLane) || carLane.m_AccessRestriction == accessRequirement) && (this.m_SetupQueueTarget.m_RoadTypes & this.m_CarLaneData[this.m_PrefabRef[lane].m_Prefab].m_RoadTypes) != RoadTypes.None)
        {
          this.AddTarget(ref random, target, lane, curvePos, cost, flags);
          return 1;
        }
      }
      Game.Net.ParkingLane componentData;
      if ((this.m_SetupQueueTarget.m_Methods & (PathMethod.Parking | PathMethod.Boarding | PathMethod.SpecialParking)) != (PathMethod) 0 && this.m_ParkingLane.TryGetComponent(lane, out componentData) && (this.CheckAccessRestriction(allowAccessRestriction, componentData) || componentData.m_AccessRestriction == accessRequirement))
      {
        ParkingLaneData parkingLaneData = this.m_ParkingLaneData[this.m_PrefabRef[lane].m_Prefab];
        PathMethod pathMethod = (componentData.m_Flags & ParkingLaneFlags.SpecialVehicles) != (ParkingLaneFlags) 0 ? PathMethod.Boarding | PathMethod.SpecialParking : PathMethod.Parking | PathMethod.Boarding;
        if ((this.m_SetupQueueTarget.m_RoadTypes & parkingLaneData.m_RoadTypes) != RoadTypes.None && (this.m_SetupQueueTarget.m_Methods & pathMethod) != (PathMethod) 0)
        {
          this.AddTarget(ref random, target, lane, curvePos, cost, flags);
          return 1;
        }
      }
      if ((this.m_SetupQueueTarget.m_Methods & PathMethod.Track) != (PathMethod) 0)
      {
        PrefabRef prefabRef = this.m_PrefabRef[lane];
        if (this.m_TrackLaneData.HasComponent(prefabRef.m_Prefab) && (this.m_SetupQueueTarget.m_TrackTypes & this.m_TrackLaneData[prefabRef.m_Prefab].m_TrackTypes) != TrackTypes.None)
        {
          this.AddTarget(ref random, target, lane, curvePos, cost, flags);
          return 1;
        }
      }
      if (this.m_ConnectionLane.HasComponent(lane))
      {
        Game.Net.ConnectionLane connectionLane = this.m_ConnectionLane[lane];
        if ((this.CheckAccessRestriction(allowAccessRestriction, connectionLane) || connectionLane.m_AccessRestriction == accessRequirement) && (connectionLane.m_Flags & ConnectionLaneFlags.Inside) == (ConnectionLaneFlags) 0 && ((this.m_SetupQueueTarget.m_Methods & PathMethod.Pedestrian) != (PathMethod) 0 && (connectionLane.m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0 || (this.m_SetupQueueTarget.m_Methods & PathMethod.Road) != (PathMethod) 0 && (connectionLane.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0 && (this.m_SetupQueueTarget.m_RoadTypes & connectionLane.m_RoadTypes) != RoadTypes.None || (this.m_SetupQueueTarget.m_Methods & PathMethod.Track) != (PathMethod) 0 && (connectionLane.m_Flags & ConnectionLaneFlags.Track) != (ConnectionLaneFlags) 0 && (this.m_SetupQueueTarget.m_TrackTypes & connectionLane.m_TrackTypes) != TrackTypes.None || (this.m_SetupQueueTarget.m_Methods & PathMethod.CargoLoading) != (PathMethod) 0 && (connectionLane.m_Flags & ConnectionLaneFlags.AllowCargo) != (ConnectionLaneFlags) 0))
        {
          curvePos = math.select(curvePos, 1f, (connectionLane.m_Flags & ConnectionLaneFlags.Start) != 0);
          this.AddTarget(ref random, target, lane, curvePos, cost, flags);
          return 1;
        }
      }
      return 0;
    }

    private int AddTrainTargets(
      ref Unity.Mathematics.Random random,
      Entity target,
      Entity entity,
      float cost,
      EdgeFlags flags)
    {
      int num = 0;
      DynamicBuffer<LayoutElement> bufferData;
      if (this.m_VehicleLayout.TryGetBuffer(entity, out bufferData) && bufferData.Length != 0)
      {
        Entity vehicle1 = bufferData[0].m_Vehicle;
        Entity vehicle2 = bufferData[bufferData.Length - 1].m_Vehicle;
        TrainCurrentLane componentData1;
        if (this.m_TrainCurrentLane.TryGetComponent(vehicle1, out componentData1))
        {
          num += this.AddTrainTarget(ref random, target, cost, flags, vehicle1, componentData1.m_Front.m_Lane, componentData1.m_Front.m_CurvePosition.w, true);
        }
        else
        {
          ParkedTrain componentData2;
          if (this.m_ParkedTrain.TryGetComponent(vehicle1, out componentData2))
            num += this.AddTrainTarget(ref random, target, cost, flags, vehicle1, componentData2.m_FrontLane, componentData2.m_CurvePosition.x, true);
        }
        TrainCurrentLane componentData3;
        if (this.m_TrainCurrentLane.TryGetComponent(vehicle2, out componentData3))
        {
          num += this.AddTrainTarget(ref random, target, cost, flags, vehicle2, componentData3.m_Rear.m_Lane, componentData3.m_Rear.m_CurvePosition.y, false);
        }
        else
        {
          ParkedTrain componentData4;
          if (this.m_ParkedTrain.TryGetComponent(vehicle2, out componentData4))
            num += this.AddTrainTarget(ref random, target, cost, flags, vehicle2, componentData4.m_RearLane, componentData4.m_CurvePosition.y, false);
        }
        if (num != 0)
          return num;
      }
      TrainCurrentLane componentData5;
      if (this.m_TrainCurrentLane.TryGetComponent(entity, out componentData5))
      {
        num = num + this.AddTrainTarget(ref random, target, cost, flags, entity, componentData5.m_Front.m_Lane, componentData5.m_Front.m_CurvePosition.w, true) + this.AddTrainTarget(ref random, target, cost, flags, entity, componentData5.m_Rear.m_Lane, componentData5.m_Rear.m_CurvePosition.y, false);
      }
      else
      {
        ParkedTrain componentData6;
        if (this.m_ParkedTrain.TryGetComponent(entity, out componentData6))
          num = num + this.AddTrainTarget(ref random, target, cost, flags, entity, componentData6.m_FrontLane, componentData6.m_CurvePosition.x, true) + this.AddTrainTarget(ref random, target, cost, flags, entity, componentData6.m_RearLane, componentData6.m_CurvePosition.y, false);
      }
      return num;
    }

    private int AddTrainTarget(
      ref Unity.Mathematics.Random random,
      Entity target,
      float cost,
      EdgeFlags flags,
      Entity carriage,
      Entity lane,
      float curvePosition,
      bool trainForward)
    {
      Curve componentData;
      if (!this.m_Curve.TryGetComponent(lane, out componentData))
        return 0;
      Train train = this.m_Train[carriage];
      bool flag = (double) math.dot(math.forward(this.m_Transform[carriage].m_Rotation), MathUtils.Tangent(componentData.m_Bezier, curvePosition)) >= 0.0 ^ (train.m_Flags & Game.Vehicles.TrainFlags.Reversed) > (Game.Vehicles.TrainFlags) 0 == trainForward;
      flags &= (EdgeFlags) ~(flag ? 2 : 1);
      this.AddTarget(ref random, target, lane, curvePosition, cost, flags);
      return 1;
    }

    public int AddLaneConnectionTargets(
      ref Unity.Mathematics.Random random,
      Entity target,
      float cost,
      EdgeFlags flags,
      LaneConnection laneConnection,
      float3 comparePosition,
      float maxDistance,
      bool allowLaneGroupSwitch,
      bool allowAccessRestriction)
    {
      int num1 = 0;
      if ((this.m_SetupQueueTarget.m_Methods & PathMethod.Pedestrian) != (PathMethod) 0 && laneConnection.m_EndLane != Entity.Null)
      {
        Game.Net.PedestrianLane componentData1;
        if (this.m_PedestrianLane.TryGetComponent(laneConnection.m_EndLane, out componentData1))
        {
          if (!this.CheckAccessRestriction(allowAccessRestriction, componentData1))
            goto label_5;
        }
        else
        {
          Game.Net.ConnectionLane componentData2;
          if (!this.m_ConnectionLane.TryGetComponent(laneConnection.m_EndLane, out componentData2) || (componentData2.m_Flags & ConnectionLaneFlags.Pedestrian) == (ConnectionLaneFlags) 0 || !this.CheckAccessRestriction(allowAccessRestriction, componentData2))
            goto label_5;
        }
        float t;
        float distance = MathUtils.Distance(this.m_Curve[laneConnection.m_EndLane].m_Bezier, comparePosition, out t);
        num1 += this.AddPedestrianLaneTargets(ref random, target, laneConnection.m_EndLane, t, cost, distance, flags, allowAccessRestriction);
      }
label_5:
      if ((this.m_SetupQueueTarget.m_Methods & PathMethod.Road) != (PathMethod) 0 && laneConnection.m_StartLane != Entity.Null)
      {
        PrefabRef componentData3;
        CarLaneData componentData4;
        if (this.m_PrefabRef.TryGetComponent(laneConnection.m_StartLane, out componentData3) && this.m_CarLaneData.TryGetComponent(componentData3.m_Prefab, out componentData4) && !this.m_MasterLane.HasComponent(laneConnection.m_StartLane))
        {
          Game.Net.CarLane carLane = this.m_CarLane[laneConnection.m_StartLane];
          if ((this.m_SetupQueueTarget.m_RoadTypes & componentData4.m_RoadTypes) == RoadTypes.None || !this.CheckAccessRestriction(allowAccessRestriction, carLane))
            goto label_11;
        }
        else if (this.m_ConnectionLane.HasComponent(laneConnection.m_StartLane))
        {
          Game.Net.ConnectionLane connectionLane = this.m_ConnectionLane[laneConnection.m_StartLane];
          if ((connectionLane.m_Flags & ConnectionLaneFlags.Road) == (ConnectionLaneFlags) 0 || (connectionLane.m_RoadTypes & this.m_SetupQueueTarget.m_RoadTypes) == RoadTypes.None || !this.CheckAccessRestriction(allowAccessRestriction, connectionLane))
            goto label_11;
        }
        else
          goto label_11;
        float t;
        double num2 = (double) MathUtils.Distance(this.m_Curve[laneConnection.m_StartLane].m_Bezier, comparePosition, out t);
        num1 += this.AddCarLaneTargets(ref random, target, laneConnection.m_StartLane, comparePosition, maxDistance, t, cost, flags, allowLaneGroupSwitch, false, allowAccessRestriction);
      }
label_11:
      return num1;
    }

    public int AddEdgeTargets(
      ref Unity.Mathematics.Random random,
      Entity target,
      float cost,
      EdgeFlags flags,
      Entity edge,
      float3 comparePosition,
      float maxDistance,
      bool allowLaneGroupSwitch,
      bool allowAccessRestriction)
    {
      DynamicBuffer<Game.Net.SubLane> dynamicBuffer = this.m_SubLane[edge];
      float distance = float.MaxValue;
      float curvePos1 = 0.0f;
      Entity subLane1 = Entity.Null;
      float num1 = float.MaxValue;
      float curvePos2 = 0.0f;
      Entity subLane2 = Entity.Null;
      float num2 = float.MaxValue;
      float delta1 = 0.0f;
      Entity subLane3 = Entity.Null;
      float num3 = float.MaxValue;
      float delta2 = 0.0f;
      Entity subLane4 = Entity.Null;
      for (int index = 0; index < dynamicBuffer.Length; ++index)
      {
        Game.Net.SubLane subLane5 = dynamicBuffer[index];
        PathMethod pathMethod = this.m_SetupQueueTarget.m_Methods & subLane5.m_PathMethods;
        if (pathMethod != (PathMethod) 0)
        {
          if ((pathMethod & PathMethod.Pedestrian) != (PathMethod) 0)
          {
            if (this.m_PedestrianLane.HasComponent(subLane5.m_SubLane))
            {
              Game.Net.PedestrianLane pedestrianLane = this.m_PedestrianLane[subLane5.m_SubLane];
              if (!this.CheckAccessRestriction(allowAccessRestriction, pedestrianLane))
                goto label_10;
            }
            else if (this.m_ConnectionLane.HasComponent(subLane5.m_SubLane))
            {
              Game.Net.ConnectionLane connectionLane = this.m_ConnectionLane[subLane5.m_SubLane];
              if ((connectionLane.m_Flags & ConnectionLaneFlags.Pedestrian) == (ConnectionLaneFlags) 0 || !this.CheckAccessRestriction(allowAccessRestriction, connectionLane))
                goto label_10;
            }
            else
              goto label_10;
            Curve curve = this.m_Curve[subLane5.m_SubLane];
            if ((double) MathUtils.Distance(MathUtils.Bounds(curve.m_Bezier), comparePosition) < (double) distance)
            {
              float t;
              float num4 = MathUtils.Distance(curve.m_Bezier, comparePosition, out t);
              if ((double) num4 < (double) distance)
              {
                distance = num4;
                curvePos1 = t;
                subLane1 = subLane5.m_SubLane;
                continue;
              }
            }
          }
label_10:
          if ((pathMethod & PathMethod.Road) != (PathMethod) 0)
          {
            PrefabRef prefabRef = this.m_PrefabRef[subLane5.m_SubLane];
            if (this.m_CarLaneData.HasComponent(prefabRef.m_Prefab) && !this.m_MasterLane.HasComponent(subLane5.m_SubLane))
            {
              Game.Net.CarLane carLane = this.m_CarLane[subLane5.m_SubLane];
              if ((this.m_SetupQueueTarget.m_RoadTypes & this.m_CarLaneData[prefabRef.m_Prefab].m_RoadTypes) == RoadTypes.None || !this.CheckAccessRestriction(allowAccessRestriction, carLane))
                goto label_18;
            }
            else if (this.m_ConnectionLane.HasComponent(subLane5.m_SubLane))
            {
              Game.Net.ConnectionLane connectionLane = this.m_ConnectionLane[subLane5.m_SubLane];
              if ((connectionLane.m_Flags & ConnectionLaneFlags.Road) == (ConnectionLaneFlags) 0 || (connectionLane.m_RoadTypes & this.m_SetupQueueTarget.m_RoadTypes) == RoadTypes.None || !this.CheckAccessRestriction(allowAccessRestriction, connectionLane))
                goto label_18;
            }
            else
              goto label_18;
            Curve curve = this.m_Curve[subLane5.m_SubLane];
            if ((double) MathUtils.Distance(MathUtils.Bounds(curve.m_Bezier), comparePosition) < (double) num1)
            {
              float t;
              float num5 = MathUtils.Distance(curve.m_Bezier, comparePosition, out t);
              if ((double) num5 < (double) num1)
              {
                num1 = num5;
                curvePos2 = t;
                subLane2 = subLane5.m_SubLane;
                continue;
              }
            }
          }
label_18:
          if ((pathMethod & (PathMethod.Parking | PathMethod.Boarding | PathMethod.SpecialParking)) != (PathMethod) 0)
          {
            ParkingLaneData componentData1;
            if (this.m_ParkingLaneData.TryGetComponent(this.m_PrefabRef[subLane5.m_SubLane].m_Prefab, out componentData1))
            {
              Game.Net.ParkingLane parkingLane = this.m_ParkingLane[subLane5.m_SubLane];
              if ((this.m_SetupQueueTarget.m_RoadTypes & componentData1.m_RoadTypes) == RoadTypes.None || !this.CheckAccessRestriction(allowAccessRestriction, parkingLane))
                goto label_25;
            }
            else
            {
              Game.Net.ConnectionLane componentData2;
              if (!this.m_ConnectionLane.TryGetComponent(subLane5.m_SubLane, out componentData2) || (componentData2.m_Flags & ConnectionLaneFlags.Parking) == (ConnectionLaneFlags) 0 || (componentData2.m_RoadTypes & this.m_SetupQueueTarget.m_RoadTypes) == RoadTypes.None || !this.CheckAccessRestriction(allowAccessRestriction, componentData2))
                goto label_25;
            }
            Curve curve = this.m_Curve[subLane5.m_SubLane];
            if ((double) MathUtils.Distance(MathUtils.Bounds(curve.m_Bezier), comparePosition) < (double) num2)
            {
              float t;
              float num6 = MathUtils.Distance(curve.m_Bezier, comparePosition, out t);
              if ((double) num6 < (double) num2)
              {
                num2 = num6;
                delta1 = t;
                subLane3 = subLane5.m_SubLane;
                continue;
              }
            }
          }
label_25:
          if ((pathMethod & PathMethod.Track) != (PathMethod) 0)
          {
            PrefabRef prefabRef = this.m_PrefabRef[subLane5.m_SubLane];
            if (this.m_TrackLaneData.HasComponent(prefabRef.m_Prefab))
            {
              if ((this.m_SetupQueueTarget.m_TrackTypes & this.m_TrackLaneData[prefabRef.m_Prefab].m_TrackTypes) == TrackTypes.None)
                continue;
            }
            else if (this.m_ConnectionLane.HasComponent(subLane5.m_SubLane))
            {
              Game.Net.ConnectionLane connectionLane = this.m_ConnectionLane[subLane5.m_SubLane];
              if ((connectionLane.m_Flags & ConnectionLaneFlags.Track) == (ConnectionLaneFlags) 0 || (connectionLane.m_TrackTypes & this.m_SetupQueueTarget.m_TrackTypes) == TrackTypes.None)
                continue;
            }
            else
              continue;
            Curve curve = this.m_Curve[subLane5.m_SubLane];
            if ((double) MathUtils.Distance(MathUtils.Bounds(curve.m_Bezier), comparePosition) < (double) num3)
            {
              float t;
              float num7 = MathUtils.Distance(curve.m_Bezier, comparePosition, out t);
              if ((double) num7 < (double) num3)
              {
                num3 = num7;
                delta2 = t;
                subLane4 = subLane5.m_SubLane;
              }
            }
          }
        }
      }
      int num8 = 0;
      if (subLane1 != Entity.Null)
        num8 += this.AddPedestrianLaneTargets(ref random, target, subLane1, curvePos1, cost, distance, flags, allowAccessRestriction);
      if (subLane2 != Entity.Null)
        num8 += this.AddCarLaneTargets(ref random, target, subLane2, comparePosition, maxDistance, curvePos2, cost, flags, allowLaneGroupSwitch, false, allowAccessRestriction);
      if (subLane3 != Entity.Null)
      {
        this.AddTarget(ref random, target, subLane3, delta1, cost, flags);
        ++num8;
      }
      if (subLane4 != Entity.Null)
      {
        this.AddTarget(ref random, target, subLane4, delta2, cost, flags);
        ++num8;
      }
      return num8;
    }

    private int AddPedestrianLaneTargets(
      ref Unity.Mathematics.Random random,
      Entity target,
      Entity lane,
      float curvePos,
      float cost,
      float distance,
      EdgeFlags flags,
      bool allowAccessRestriction)
    {
      Game.Net.PedestrianLane componentData1;
      if (this.m_PedestrianLane.TryGetComponent(lane, out componentData1))
      {
        if (!this.CheckAccessRestriction(allowAccessRestriction, componentData1))
          return 0;
      }
      else
      {
        Game.Net.ConnectionLane componentData2;
        if (this.m_ConnectionLane.TryGetComponent(lane, out componentData2) && !this.CheckAccessRestriction(allowAccessRestriction, componentData2))
          return 0;
      }
      float cost1 = cost + this.CalculatePedestrianTargetCost(ref random, distance);
      this.AddTarget(ref random, target, lane, curvePos, cost1, flags);
      return 1;
    }

    private int AddParkingLaneTargets(
      ref Unity.Mathematics.Random random,
      Entity target,
      Entity lane,
      float curvePos,
      float cost,
      EdgeFlags flags,
      bool allowAccessRestriction)
    {
      Game.Net.ParkingLane componentData1;
      if (this.m_ParkingLane.TryGetComponent(lane, out componentData1))
      {
        if (!this.CheckAccessRestriction(allowAccessRestriction, componentData1))
          return 0;
      }
      else
      {
        Game.Net.ConnectionLane componentData2;
        if (this.m_ConnectionLane.TryGetComponent(lane, out componentData2) && !this.CheckAccessRestriction(allowAccessRestriction, componentData2))
          return 0;
      }
      this.AddTarget(ref random, target, lane, curvePos, cost, flags);
      return 1;
    }

    private int AddCarLaneTargets(
      ref Unity.Mathematics.Random random,
      Entity target,
      Entity lane,
      float3 comparePosition,
      float maxDistance,
      float curvePos,
      float cost,
      EdgeFlags flags,
      bool allowLaneGroupSwitch,
      bool allowBlocked,
      bool allowAccessRestriction)
    {
      if (!this.m_CarLane.HasComponent(lane))
      {
        if (this.m_ConnectionLane.HasComponent(lane))
        {
          Game.Net.ConnectionLane connectionLane = this.m_ConnectionLane[lane];
          if (!this.CheckAccessRestriction(allowAccessRestriction, connectionLane))
            return 0;
        }
        this.AddTarget(ref random, target, lane, curvePos, cost, flags);
        return 1;
      }
      Owner owner = this.m_Owner[lane];
      Game.Net.CarLane carLane = this.m_CarLane[lane];
      SlaveLane slaveLane = new SlaveLane();
      if (!this.CheckAccessRestriction(allowAccessRestriction, carLane))
        return 0;
      NetLaneData netLaneData = this.m_NetLaneData[this.m_PrefabRef[lane].m_Prefab];
      PathfindCarData pathfindCarData = this.m_CarPathfindData[netLaneData.m_PathfindPrefab];
      float num1 = 0.0f;
      NodeLane componentData1;
      if (this.m_NodeLane.TryGetComponent(lane, out componentData1))
        num1 = (float) (((double) netLaneData.m_Width + (double) math.lerp(componentData1.m_WidthOffset.x, componentData1.m_WidthOffset.y, curvePos)) * 0.5);
      bool flag = false;
      if (this.m_SlaveLane.HasComponent(lane))
      {
        slaveLane = this.m_SlaveLane[lane];
        num1 *= (float) ((int) slaveLane.m_MaxIndex - (int) slaveLane.m_MinIndex + 1);
        flag = true;
      }
      DynamicBuffer<Game.Net.SubLane> dynamicBuffer = this.m_SubLane[owner.m_Owner];
      int b = (int) slaveLane.m_MaxIndex - (int) slaveLane.m_MinIndex + 1;
      int num2 = 0;
      for (int index = 0; index < dynamicBuffer.Length; ++index)
      {
        Game.Net.SubLane subLane = dynamicBuffer[index];
        if ((subLane.m_PathMethods & PathMethod.Road) != (PathMethod) 0 && this.m_CarLane.HasComponent(subLane.m_SubLane) && !this.m_SlaveLane.HasComponent(subLane.m_SubLane))
        {
          Game.Net.CarLane carLaneData = this.m_CarLane[subLane.m_SubLane];
          if ((int) carLaneData.m_CarriagewayGroup == (int) carLane.m_CarriagewayGroup && !(carLaneData.m_AccessRestriction != carLane.m_AccessRestriction))
          {
            bool c;
            int num3;
            if (this.m_MasterLane.HasComponent(subLane.m_SubLane))
            {
              MasterLane masterLane = this.m_MasterLane[subLane.m_SubLane];
              c = !flag || (int) masterLane.m_Group != (int) slaveLane.m_Group;
              num3 = (int) masterLane.m_MaxIndex - (int) masterLane.m_MinIndex + 1;
            }
            else
            {
              c = subLane.m_SubLane != lane;
              num3 = 1;
            }
            float t = math.select(curvePos, 1f - curvePos, ((carLane.m_Flags ^ carLaneData.m_Flags) & Game.Net.CarLaneFlags.Invert) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter));
            if (c)
            {
              if ((carLane.m_Flags & (Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout)) != Game.Net.CarLaneFlags.Roundabout)
              {
                if ((double) num1 != 0.0)
                {
                  NodeLane componentData2;
                  if (this.m_NodeLane.TryGetComponent(subLane.m_SubLane, out componentData2))
                  {
                    Curve curve = this.m_Curve[lane];
                    double num4 = (double) MathUtils.Distance(this.m_Curve[subLane.m_SubLane].m_Bezier, MathUtils.Position(curve.m_Bezier, curvePos), out t);
                    float num5 = (float) (((double) this.m_NetLaneData[this.m_PrefabRef[subLane.m_SubLane].m_Prefab].m_Width + (double) math.lerp(componentData2.m_WidthOffset.x, componentData2.m_WidthOffset.y, t)) * 0.5) * (float) num3;
                    double num6 = (double) num1 + (double) num5 + 3.0;
                    if (num4 > num6)
                      continue;
                  }
                  else
                    continue;
                }
              }
              else
                continue;
            }
            if ((int) carLaneData.m_BlockageEnd >= (int) carLaneData.m_BlockageStart && (this.m_PathfindParameters.m_IgnoredRules & RuleFlags.HasBlockage) == (RuleFlags) 0)
            {
              Bounds1 blockageBounds = carLaneData.blockageBounds;
              if ((double) maxDistance != 0.0 && (carLaneData.m_Flags & Game.Net.CarLaneFlags.Twoway) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) && carLaneData.m_BlockageStart > (byte) 0 && (double) math.distance(MathUtils.Position(this.m_Curve[subLane.m_SubLane].m_Bezier, blockageBounds.min), comparePosition) <= (double) maxDistance)
                t = math.max(0.0f, blockageBounds.min - 0.01f);
              else if (MathUtils.Intersect(blockageBounds, t))
              {
                if (((double) t - (double) blockageBounds.min < (double) blockageBounds.max - (double) t && (carLaneData.m_Flags & Game.Net.CarLaneFlags.Twoway) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) || carLaneData.m_BlockageEnd == byte.MaxValue) && carLaneData.m_BlockageStart > (byte) 0)
                  t = math.max(0.0f, blockageBounds.min - 0.01f);
                else if (carLaneData.m_BlockageEnd < byte.MaxValue && (allowBlocked || (double) blockageBounds.max + 0.0099999997764825821 <= (double) t))
                  t = math.min(1f, blockageBounds.max + 0.01f);
                else
                  continue;
              }
            }
            float distance = math.distance(comparePosition, MathUtils.Position(this.m_Curve[subLane.m_SubLane].m_Bezier, curvePos));
            float cost1 = cost + this.CalculateCarTargetCost(ref random, pathfindCarData, carLaneData, distance, math.select(0, b, c), !allowLaneGroupSwitch & c);
            this.AddTarget(ref random, target, subLane.m_SubLane, t, cost1, flags);
            ++num2;
          }
        }
      }
      return num2;
    }

    private float CalculatePedestrianTargetCost(ref Unity.Mathematics.Random random, float distance)
    {
      return PathUtils.CalculateCost(ref random, new PathSpecification()
      {
        m_Flags = EdgeFlags.Forward | EdgeFlags.Backward,
        m_Methods = PathMethod.Pedestrian,
        m_Length = distance,
        m_MaxSpeed = 5.555556f,
        m_Density = 0.0f,
        m_AccessRequirement = -1
      }, in this.m_PathfindParameters);
    }

    private float CalculateCarTargetCost(
      ref Unity.Mathematics.Random random,
      PathfindCarData pathfindCarData,
      Game.Net.CarLane carLaneData,
      float distance,
      int laneCrossCount,
      bool unsafeUTurn)
    {
      PathSpecification pathSpecification = new PathSpecification();
      pathSpecification.m_Flags = EdgeFlags.Forward | EdgeFlags.Backward;
      pathSpecification.m_Methods = PathMethod.Road;
      pathSpecification.m_Length = distance;
      pathSpecification.m_MaxSpeed = carLaneData.m_SpeedLimit;
      pathSpecification.m_Density = 0.0f;
      pathSpecification.m_AccessRequirement = -1;
      PathUtils.TryAddCosts(ref pathSpecification.m_Costs, pathfindCarData.m_LaneCrossCost, (float) laneCrossCount);
      PathUtils.TryAddCosts(ref pathSpecification.m_Costs, pathfindCarData.m_UnsafeUTurnCost, unsafeUTurn);
      return PathUtils.CalculateCost(ref random, in pathSpecification, in this.m_PathfindParameters);
    }
  }
}
