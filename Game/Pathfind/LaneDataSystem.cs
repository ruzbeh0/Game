// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.LaneDataSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Events;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Pathfind
{
  [CompilerGenerated]
  public class LaneDataSystem : GameSystemBase
  {
    private CitySystem m_CitySystem;
    private EntityQuery m_LaneQuery;
    private EntityQuery m_LaneQuery2;
    private LaneDataSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Lane>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Net.CarLane>(),
          ComponentType.ReadOnly<Game.Net.PedestrianLane>(),
          ComponentType.ReadOnly<Game.Net.TrackLane>(),
          ComponentType.ReadOnly<Game.Net.ConnectionLane>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<GarageLane>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PathfindUpdated>(),
          ComponentType.ReadOnly<Lane>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Net.CarLane>(),
          ComponentType.ReadOnly<Game.Net.PedestrianLane>(),
          ComponentType.ReadOnly<Game.Net.TrackLane>(),
          ComponentType.ReadOnly<Game.Net.ConnectionLane>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<GarageLane>(),
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery2 = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Lane>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Net.CarLane>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PathfindUpdated>(),
          ComponentType.ReadOnly<Lane>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Net.CarLane>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_LaneQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_TargetElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_AccidentSite_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_District_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_City_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrackLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_PedestrianLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      LaneDataSystem.UpdateLaneDataJob jobData1 = new LaneDataSystem.UpdateLaneDataJob()
      {
        m_LaneType = this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle,
        m_EdgeLaneType = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentTypeHandle,
        m_MasterLaneType = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentTypeHandle,
        m_SlaveLaneType = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_LaneObjectType = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferTypeHandle,
        m_CarLaneType = this.__TypeHandle.__Game_Net_CarLane_RW_ComponentTypeHandle,
        m_PedestrianLaneType = this.__TypeHandle.__Game_Net_PedestrianLane_RW_ComponentTypeHandle,
        m_TrackLaneType = this.__TypeHandle.__Game_Net_TrackLane_RW_ComponentTypeHandle,
        m_ConnectionLaneType = this.__TypeHandle.__Game_Net_ConnectionLane_RW_ComponentTypeHandle,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_RoadData = this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup,
        m_CityData = this.__TypeHandle.__Game_City_City_RO_ComponentLookup,
        m_BorderDistrictData = this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentLookup,
        m_DistrictData = this.__TypeHandle.__Game_Areas_District_RO_ComponentLookup,
        m_MovingData = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup,
        m_CarData = this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentLookup,
        m_InvolvedInAccidenteData = this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentLookup,
        m_AccidentSiteData = this.__TypeHandle.__Game_Events_AccidentSite_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabCarLaneData = this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_ConnectedNodes = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_DistrictModifiers = this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup,
        m_TargetElements = this.__TypeHandle.__Game_Events_TargetElement_RO_BufferLookup,
        m_City = this.m_CitySystem.City
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<LaneDataSystem.UpdateLaneDataJob>(this.m_LaneQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      if (this.m_LaneQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LaneDataSystem.UpdateLaneData2Job jobData2 = new LaneDataSystem.UpdateLaneData2Job()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData2.ScheduleParallel<LaneDataSystem.UpdateLaneData2Job>(this.m_LaneQuery2, this.Dependency);
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
    public LaneDataSystem()
    {
    }

    [BurstCompile]
    private struct UpdateLaneDataJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Lane> m_LaneType;
      [ReadOnly]
      public ComponentTypeHandle<EdgeLane> m_EdgeLaneType;
      [ReadOnly]
      public ComponentTypeHandle<MasterLane> m_MasterLaneType;
      [ReadOnly]
      public ComponentTypeHandle<SlaveLane> m_SlaveLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<LaneObject> m_LaneObjectType;
      public ComponentTypeHandle<Game.Net.CarLane> m_CarLaneType;
      public ComponentTypeHandle<Game.Net.PedestrianLane> m_PedestrianLaneType;
      public ComponentTypeHandle<Game.Net.TrackLane> m_TrackLaneType;
      public ComponentTypeHandle<Game.Net.ConnectionLane> m_ConnectionLaneType;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Road> m_RoadData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Attachment> m_AttachmentData;
      [ReadOnly]
      public ComponentLookup<Game.City.City> m_CityData;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> m_BorderDistrictData;
      [ReadOnly]
      public ComponentLookup<District> m_DistrictData;
      [ReadOnly]
      public ComponentLookup<Moving> m_MovingData;
      [ReadOnly]
      public ComponentLookup<Car> m_CarData;
      [ReadOnly]
      public ComponentLookup<InvolvedInAccident> m_InvolvedInAccidenteData;
      [ReadOnly]
      public ComponentLookup<AccidentSite> m_AccidentSiteData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<CarLaneData> m_PrefabCarLaneData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<ConnectedNode> m_ConnectedNodes;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<DistrictModifier> m_DistrictModifiers;
      [ReadOnly]
      public BufferLookup<TargetElement> m_TargetElements;
      [ReadOnly]
      public Entity m_City;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Lane> nativeArray1 = chunk.GetNativeArray<Lane>(ref this.m_LaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.CarLane> nativeArray2 = chunk.GetNativeArray<Game.Net.CarLane>(ref this.m_CarLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.PedestrianLane> nativeArray3 = chunk.GetNativeArray<Game.Net.PedestrianLane>(ref this.m_PedestrianLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.TrackLane> nativeArray4 = chunk.GetNativeArray<Game.Net.TrackLane>(ref this.m_TrackLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.ConnectionLane> nativeArray5 = chunk.GetNativeArray<Game.Net.ConnectionLane>(ref this.m_ConnectionLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray6 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        bool isEdgeLane = chunk.Has<EdgeLane>(ref this.m_EdgeLaneType);
        bool flag1;
        if (nativeArray2.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<MasterLane> nativeArray7 = chunk.GetNativeArray<MasterLane>(ref this.m_MasterLaneType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<SlaveLane> nativeArray8 = chunk.GetNativeArray<SlaveLane>(ref this.m_SlaveLaneType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray9 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          Game.City.City city = new Game.City.City();
          // ISSUE: reference to a compiler-generated field
          if (this.m_City != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            city = this.m_CityData[this.m_City];
          }
          if (nativeArray7.Length != 0)
          {
            for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
            {
              Game.Net.CarLane carLane = nativeArray2[index1] with
              {
                m_AccessRestriction = Entity.Null
              };
              carLane.m_Flags &= Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden;
              carLane.m_SpeedLimit = carLane.m_DefaultSpeedLimit;
              carLane.m_BlockageStart = byte.MaxValue;
              carLane.m_BlockageEnd = (byte) 0;
              carLane.m_CautionStart = byte.MaxValue;
              carLane.m_CautionEnd = (byte) 0;
              if (nativeArray6.Length != 0)
              {
                MasterLane masterLane = nativeArray7[index1];
                Owner owner = nativeArray6[index1];
                PrefabRef prefabRef = nativeArray9[index1];
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[owner.m_Owner];
                Bounds1 bounds = new Bounds1(1f, 0.0f);
                int minIndex = (int) masterLane.m_MinIndex;
                int num = math.min((int) masterLane.m_MaxIndex, subLane1.Length - 1);
                bool flag2 = true;
                bool isSideConnection = (carLane.m_Flags & Game.Net.CarLaneFlags.SideConnection) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
                for (int index2 = minIndex; index2 <= num; ++index2)
                {
                  Entity subLane2 = subLane1[index2].m_SubLane;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_LaneObjects.HasBuffer(subLane2))
                  {
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<LaneObject> laneObject = this.m_LaneObjects[subLane2];
                    if (flag2)
                    {
                      bool isSecured;
                      // ISSUE: reference to a compiler-generated method
                      bounds = this.CheckBlockage(laneObject, out flag1, out isSecured);
                      flag2 = false;
                      if (isSecured)
                        carLane.m_Flags |= Game.Net.CarLaneFlags.IsSecured;
                    }
                    else
                    {
                      bool isSecured;
                      // ISSUE: reference to a compiler-generated method
                      bounds &= this.CheckBlockage(laneObject, out flag1, out isSecured);
                      if (isSecured)
                        carLane.m_Flags |= Game.Net.CarLaneFlags.IsSecured;
                    }
                  }
                }
                // ISSUE: reference to a compiler-generated field
                CarLaneData carLaneData = this.m_PrefabCarLaneData[prefabRef.m_Prefab];
                Game.Prefabs.BuildingFlags flag3 = (carLaneData.m_RoadTypes & (RoadTypes.Car | RoadTypes.Helicopter | RoadTypes.Airplane)) != RoadTypes.None ? Game.Prefabs.BuildingFlags.RestrictedCar : (Game.Prefabs.BuildingFlags) 0;
                bool allowEnter;
                // ISSUE: reference to a compiler-generated method
                carLane.m_AccessRestriction = this.GetAccessRestriction(owner, flag3, isEdgeLane, isSideConnection, nativeArray1[index1], out allowEnter, out flag1);
                if (allowEnter)
                  carLane.m_Flags |= Game.Net.CarLaneFlags.AllowEnter;
                // ISSUE: reference to a compiler-generated method
                this.AddOptionData(ref carLane, city);
                // ISSUE: reference to a compiler-generated method
                this.AddOptionData(ref carLane, owner, carLaneData);
                // ISSUE: reference to a compiler-generated method
                this.AddBlockageData(ref carLane, bounds, false);
              }
              nativeArray2[index1] = carLane;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<LaneObject> bufferAccessor = chunk.GetBufferAccessor<LaneObject>(ref this.m_LaneObjectType);
            for (int index = 0; index < nativeArray2.Length; ++index)
            {
              Game.Net.CarLane carLane = nativeArray2[index] with
              {
                m_AccessRestriction = Entity.Null
              };
              carLane.m_Flags &= Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden;
              carLane.m_SpeedLimit = carLane.m_DefaultSpeedLimit;
              carLane.m_BlockageStart = byte.MaxValue;
              carLane.m_BlockageEnd = (byte) 0;
              carLane.m_CautionStart = byte.MaxValue;
              carLane.m_CautionEnd = (byte) 0;
              // ISSUE: reference to a compiler-generated method
              this.AddOptionData(ref carLane, city);
              if (nativeArray6.Length != 0)
              {
                Owner owner = nativeArray6[index];
                // ISSUE: reference to a compiler-generated field
                CarLaneData carLaneData = this.m_PrefabCarLaneData[nativeArray9[index].m_Prefab];
                Game.Prefabs.BuildingFlags flag4 = (carLaneData.m_RoadTypes & (RoadTypes.Car | RoadTypes.Helicopter | RoadTypes.Airplane)) != RoadTypes.None ? Game.Prefabs.BuildingFlags.RestrictedCar : (Game.Prefabs.BuildingFlags) 0;
                bool isSideConnection = (carLane.m_Flags & Game.Net.CarLaneFlags.SideConnection) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
                bool allowEnter;
                // ISSUE: reference to a compiler-generated method
                carLane.m_AccessRestriction = this.GetAccessRestriction(owner, flag4, isEdgeLane, isSideConnection, nativeArray1[index], out allowEnter, out flag1);
                if (allowEnter)
                  carLane.m_Flags |= Game.Net.CarLaneFlags.AllowEnter;
                // ISSUE: reference to a compiler-generated method
                this.AddOptionData(ref carLane, owner, carLaneData);
              }
              if (bufferAccessor.Length != 0)
              {
                bool isEmergency;
                bool isSecured;
                // ISSUE: reference to a compiler-generated method
                Bounds1 bounds = this.CheckBlockage(bufferAccessor[index], out isEmergency, out isSecured);
                bool addCaution = isEmergency;
                if ((double) bounds.min <= (double) bounds.max && !addCaution)
                  addCaution = nativeArray8.Length == 0 || (nativeArray8[index].m_Flags & (SlaveLaneFlags.StartingLane | SlaveLaneFlags.EndingLane)) != (SlaveLaneFlags.StartingLane | SlaveLaneFlags.EndingLane);
                // ISSUE: reference to a compiler-generated method
                this.AddBlockageData(ref carLane, bounds, addCaution);
                if (isSecured)
                  carLane.m_Flags |= Game.Net.CarLaneFlags.IsSecured;
              }
              nativeArray2[index] = carLane;
            }
          }
        }
        if (nativeArray3.Length != 0)
        {
          for (int index = 0; index < nativeArray3.Length; ++index)
          {
            Game.Net.PedestrianLane pedestrianLane = nativeArray3[index] with
            {
              m_AccessRestriction = Entity.Null
            };
            pedestrianLane.m_Flags &= ~(PedestrianLaneFlags.AllowEnter | PedestrianLaneFlags.ForbidTransitTraffic);
            if (nativeArray6.Length != 0)
            {
              Owner owner = nativeArray6[index];
              // ISSUE: reference to a compiler-generated field
              if (this.m_BorderDistrictData.HasComponent(owner.m_Owner))
              {
                // ISSUE: reference to a compiler-generated field
                BorderDistrict borderDistrict = this.m_BorderDistrictData[owner.m_Owner];
                PedestrianLaneFlags pedestrianLaneFlags1 = (PedestrianLaneFlags) 0;
                PedestrianLaneFlags pedestrianLaneFlags2 = (PedestrianLaneFlags) 0;
                // ISSUE: reference to a compiler-generated field
                if (this.m_DistrictData.HasComponent(borderDistrict.m_Left))
                {
                  // ISSUE: reference to a compiler-generated field
                  if (AreaUtils.CheckOption(this.m_DistrictData[borderDistrict.m_Left], DistrictOption.ForbidTransitTraffic))
                    pedestrianLaneFlags1 |= PedestrianLaneFlags.ForbidTransitTraffic;
                  else
                    pedestrianLaneFlags2 |= PedestrianLaneFlags.ForbidTransitTraffic;
                }
                // ISSUE: reference to a compiler-generated field
                if (this.m_DistrictData.HasComponent(borderDistrict.m_Right))
                {
                  // ISSUE: reference to a compiler-generated field
                  if (AreaUtils.CheckOption(this.m_DistrictData[borderDistrict.m_Right], DistrictOption.ForbidTransitTraffic))
                    pedestrianLaneFlags1 |= PedestrianLaneFlags.ForbidTransitTraffic;
                  else
                    pedestrianLaneFlags2 |= PedestrianLaneFlags.ForbidTransitTraffic;
                }
                pedestrianLane.m_Flags |= pedestrianLaneFlags1 & ~pedestrianLaneFlags2;
              }
              Game.Prefabs.BuildingFlags flag5 = (pedestrianLane.m_Flags & PedestrianLaneFlags.OnWater) == (PedestrianLaneFlags) 0 ? Game.Prefabs.BuildingFlags.RestrictedPedestrian : (Game.Prefabs.BuildingFlags) 0;
              bool isSideConnection = (pedestrianLane.m_Flags & PedestrianLaneFlags.SideConnection) != 0;
              bool allowEnter;
              // ISSUE: reference to a compiler-generated method
              pedestrianLane.m_AccessRestriction = this.GetAccessRestriction(owner, flag5, isEdgeLane, isSideConnection, nativeArray1[index], out allowEnter, out flag1);
              if (allowEnter)
                pedestrianLane.m_Flags |= PedestrianLaneFlags.AllowEnter;
            }
            nativeArray3[index] = pedestrianLane;
          }
        }
        bool flag6;
        if (nativeArray4.Length != 0)
        {
          for (int index = 0; index < nativeArray4.Length; ++index)
          {
            Game.Net.TrackLane trackLane = nativeArray4[index] with
            {
              m_AccessRestriction = Entity.Null
            };
            if (nativeArray6.Length != 0)
            {
              Owner owner = nativeArray6[index];
              Game.Prefabs.BuildingFlags flag7 = (trackLane.m_Flags & TrackLaneFlags.Station) != (TrackLaneFlags) 0 ? Game.Prefabs.BuildingFlags.RestrictedTrack : (Game.Prefabs.BuildingFlags) 0;
              // ISSUE: reference to a compiler-generated method
              trackLane.m_AccessRestriction = this.GetAccessRestriction(owner, flag7, isEdgeLane, false, nativeArray1[index], out flag1, out flag6);
            }
            nativeArray4[index] = trackLane;
          }
        }
        if (nativeArray5.Length == 0)
          return;
        for (int index = 0; index < nativeArray5.Length; ++index)
        {
          Game.Net.ConnectionLane connectionLane = nativeArray5[index] with
          {
            m_AccessRestriction = Entity.Null
          };
          connectionLane.m_Flags &= ~(ConnectionLaneFlags.AllowEnter | ConnectionLaneFlags.AllowExit);
          if (nativeArray6.Length != 0)
          {
            Owner owner = nativeArray6[index];
            if ((connectionLane.m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0)
            {
              bool allowEnter;
              // ISSUE: reference to a compiler-generated method
              connectionLane.m_AccessRestriction = this.GetAccessRestriction(owner, Game.Prefabs.BuildingFlags.RestrictedPedestrian, isEdgeLane, false, nativeArray1[index], out allowEnter, out flag6);
              if (allowEnter)
                connectionLane.m_Flags |= ConnectionLaneFlags.AllowEnter;
            }
            else if ((connectionLane.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0)
            {
              Game.Prefabs.BuildingFlags flag8 = (connectionLane.m_RoadTypes & (RoadTypes.Car | RoadTypes.Helicopter | RoadTypes.Airplane)) != RoadTypes.None ? Game.Prefabs.BuildingFlags.RestrictedCar : (Game.Prefabs.BuildingFlags) 0;
              bool allowEnter;
              // ISSUE: reference to a compiler-generated method
              connectionLane.m_AccessRestriction = this.GetAccessRestriction(owner, flag8, isEdgeLane, false, nativeArray1[index], out allowEnter, out flag6);
              if (allowEnter)
                connectionLane.m_Flags |= ConnectionLaneFlags.AllowEnter;
            }
            else if ((connectionLane.m_Flags & ConnectionLaneFlags.Parking) != (ConnectionLaneFlags) 0)
            {
              bool allowEnter;
              bool allowExit;
              // ISSUE: reference to a compiler-generated method
              connectionLane.m_AccessRestriction = this.GetAccessRestriction(owner, Game.Prefabs.BuildingFlags.RestrictedPedestrian | Game.Prefabs.BuildingFlags.RestrictedCar, isEdgeLane, false, nativeArray1[index], out allowEnter, out allowExit);
              if (allowEnter)
                connectionLane.m_Flags |= ConnectionLaneFlags.AllowEnter;
              if (allowExit)
                connectionLane.m_Flags |= ConnectionLaneFlags.AllowExit;
            }
            else if ((connectionLane.m_Flags & ConnectionLaneFlags.AllowCargo) != (ConnectionLaneFlags) 0)
            {
              Game.Prefabs.BuildingFlags flag9 = Game.Prefabs.BuildingFlags.RestrictedCar;
              bool allowEnter;
              // ISSUE: reference to a compiler-generated method
              connectionLane.m_AccessRestriction = this.GetAccessRestriction(owner, flag9, isEdgeLane, false, nativeArray1[index], out allowEnter, out flag6);
              if (allowEnter)
                connectionLane.m_Flags |= ConnectionLaneFlags.AllowEnter;
            }
            else if ((connectionLane.m_Flags & ConnectionLaneFlags.Track) != (ConnectionLaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              connectionLane.m_AccessRestriction = this.GetAccessRestriction(owner, Game.Prefabs.BuildingFlags.RestrictedTrack, isEdgeLane, false, nativeArray1[index], out flag6, out flag1);
            }
          }
          nativeArray5[index] = connectionLane;
        }
      }

      private bool IsSecured(InvolvedInAccident involvedInAccident)
      {
        // ISSUE: reference to a compiler-generated method
        Entity accidentSite = this.FindAccidentSite(involvedInAccident.m_Event);
        // ISSUE: reference to a compiler-generated field
        return !(accidentSite != Entity.Null) || (this.m_AccidentSiteData[accidentSite].m_Flags & AccidentSiteFlags.Secured) > (AccidentSiteFlags) 0;
      }

      private Entity FindAccidentSite(Entity _event)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_TargetElements.HasBuffer(_event))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<TargetElement> targetElement = this.m_TargetElements[_event];
          for (int index = 0; index < targetElement.Length; ++index)
          {
            Entity entity = targetElement[index].m_Entity;
            // ISSUE: reference to a compiler-generated field
            if (this.m_AccidentSiteData.HasComponent(entity))
              return entity;
          }
        }
        return Entity.Null;
      }

      private Entity GetAccessRestriction(
        Owner owner,
        Game.Prefabs.BuildingFlags flag,
        bool isEdgeLane,
        bool isSideConnection,
        Lane lane,
        out bool allowEnter,
        out bool allowExit)
      {
        allowEnter = false;
        allowExit = false;
        // ISSUE: reference to a compiler-generated field
        isSideConnection = ((isSideConnection ? 1 : 0) | (isEdgeLane ? 0 : (this.m_ConnectedNodes.HasBuffer(owner.m_Owner) ? 1 : 0))) != 0;
        if (isSideConnection)
        {
          DynamicBuffer<ConnectedNode> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConnectedNodes.TryGetBuffer(owner.m_Owner, out bufferData1))
          {
            for (int index1 = 0; index1 < bufferData1.Length; ++index1)
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ConnectedEdge> connectedEdge1 = this.m_ConnectedEdges[bufferData1[index1].m_Node];
              for (int index2 = 0; index2 < connectedEdge1.Length; ++index2)
              {
                ConnectedEdge connectedEdge2 = connectedEdge1[index2];
                DynamicBuffer<Game.Net.SubLane> bufferData2;
                // ISSUE: reference to a compiler-generated field
                if (!(connectedEdge2.m_Edge == owner.m_Owner) && this.m_SubLanes.TryGetBuffer(connectedEdge2.m_Edge, out bufferData2))
                {
                  for (int index3 = 0; index3 < bufferData2.Length; ++index3)
                  {
                    // ISSUE: reference to a compiler-generated field
                    Lane lane1 = this.m_LaneData[bufferData2[index3].m_SubLane];
                    if (lane1.m_StartNode.Equals(lane.m_StartNode) || lane1.m_EndNode.Equals(lane.m_StartNode) || lane1.m_StartNode.Equals(lane.m_EndNode) || lane1.m_EndNode.Equals(lane.m_EndNode))
                    {
                      owner.m_Owner = connectedEdge2.m_Edge;
                      goto label_31;
                    }
                  }
                }
              }
            }
          }
          else
          {
            DynamicBuffer<ConnectedEdge> bufferData3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectedEdges.TryGetBuffer(owner.m_Owner, out bufferData3))
            {
              for (int index4 = 0; index4 < bufferData3.Length; ++index4)
              {
                ConnectedEdge connectedEdge3 = bufferData3[index4];
                // ISSUE: reference to a compiler-generated field
                Game.Net.Edge edge = this.m_EdgeData[connectedEdge3.m_Edge];
                if (!(edge.m_Start != owner.m_Owner) || !(edge.m_End != owner.m_Owner))
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<ConnectedNode> connectedNode = this.m_ConnectedNodes[connectedEdge3.m_Edge];
                  for (int index5 = 0; index5 < connectedNode.Length; ++index5)
                  {
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<ConnectedEdge> connectedEdge4 = this.m_ConnectedEdges[connectedNode[index5].m_Node];
                    for (int index6 = 0; index6 < connectedEdge4.Length; ++index6)
                    {
                      ConnectedEdge connectedEdge5 = connectedEdge4[index6];
                      DynamicBuffer<Game.Net.SubLane> bufferData4;
                      // ISSUE: reference to a compiler-generated field
                      if (!(connectedEdge5.m_Edge == connectedEdge3.m_Edge) && this.m_SubLanes.TryGetBuffer(connectedEdge5.m_Edge, out bufferData4))
                      {
                        for (int index7 = 0; index7 < bufferData4.Length; ++index7)
                        {
                          // ISSUE: reference to a compiler-generated field
                          Lane lane2 = this.m_LaneData[bufferData4[index7].m_SubLane];
                          if (lane2.m_StartNode.Equals(lane.m_StartNode) || lane2.m_EndNode.Equals(lane.m_StartNode) || lane2.m_StartNode.Equals(lane.m_EndNode) || lane2.m_EndNode.Equals(lane.m_EndNode))
                          {
                            owner.m_Owner = connectedEdge5.m_Edge;
                            goto label_31;
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
label_31:
        PrefabRef componentData1;
        NetGeometryData componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRefData.TryGetComponent(owner.m_Owner, out componentData1) && this.m_PrefabGeometryData.TryGetComponent(componentData1.m_Prefab, out componentData2) && (componentData2.m_Flags & Game.Net.GeometryFlags.SubOwner) != (Game.Net.GeometryFlags) 0)
          return Entity.Null;
        // ISSUE: reference to a compiler-generated method
        Entity topLevelOwner = this.GetTopLevelOwner(owner.m_Owner);
        // ISSUE: reference to a compiler-generated field
        if (this.m_BuildingData.HasComponent(topLevelOwner))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Game.Prefabs.BuildingData buildingData = this.m_PrefabBuildingData[this.m_PrefabRefData[topLevelOwner].m_Prefab];
          // ISSUE: reference to a compiler-generated field
          if (this.m_RoadData.HasComponent(owner.m_Owner))
            buildingData.m_Flags &= ~(Game.Prefabs.BuildingFlags.RestrictedPedestrian | Game.Prefabs.BuildingFlags.RestrictedCar);
          bool flag1 = (buildingData.m_Flags & flag) > (Game.Prefabs.BuildingFlags) 0;
          bool flag2 = (flag & Game.Prefabs.BuildingFlags.RestrictedCar) > (Game.Prefabs.BuildingFlags) 0;
          if (flag1 || (flag & (Game.Prefabs.BuildingFlags.RestrictedPedestrian | Game.Prefabs.BuildingFlags.RestrictedCar)) != (Game.Prefabs.BuildingFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (!isEdgeLane && !isSideConnection && this.m_ConnectedEdges.HasBuffer(owner.m_Owner))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ConnectedEdge> connectedEdge6 = this.m_ConnectedEdges[owner.m_Owner];
              bool2 x = (bool2) false;
              for (int index8 = 0; index8 < connectedEdge6.Length; ++index8)
              {
                ConnectedEdge connectedEdge7 = connectedEdge6[index8];
                // ISSUE: reference to a compiler-generated field
                Game.Net.Edge edge = this.m_EdgeData[connectedEdge7.m_Edge];
                // ISSUE: reference to a compiler-generated method
                if ((!(edge.m_Start != owner.m_Owner) || !(edge.m_End != owner.m_Owner)) && !(topLevelOwner == this.GetTopLevelOwner(connectedEdge7.m_Edge)))
                {
                  if (!flag2)
                    return Entity.Null;
                  DynamicBuffer<Game.Net.SubLane> bufferData;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_SubLanes.TryGetBuffer(connectedEdge7.m_Edge, out bufferData))
                  {
                    for (int index9 = 0; index9 < bufferData.Length; ++index9)
                    {
                      // ISSUE: reference to a compiler-generated field
                      Lane lane3 = this.m_LaneData[bufferData[index9].m_SubLane];
                      ref bool local1 = ref x.x;
                      local1 = ((local1 ? 1 : 0) | (lane3.m_StartNode.Equals(lane.m_StartNode) ? 1 : (lane3.m_EndNode.Equals(lane.m_StartNode) ? 1 : 0))) != 0;
                      ref bool local2 = ref x.y;
                      local2 = ((local2 ? 1 : 0) | (lane3.m_StartNode.Equals(lane.m_EndNode) ? 1 : (lane3.m_EndNode.Equals(lane.m_EndNode) ? 1 : 0))) != 0;
                      if (math.all(x))
                        return Entity.Null;
                    }
                  }
                }
              }
            }
            allowEnter = !flag1;
            allowExit = (flag & Game.Prefabs.BuildingFlags.RestrictedParking) == (Game.Prefabs.BuildingFlags) 0;
            return topLevelOwner;
          }
        }
        return Entity.Null;
      }

      private Entity GetTopLevelOwner(Entity entity)
      {
        Entity entity1 = entity;
        Owner componentData1;
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.TryGetComponent(entity1, out componentData1))
          entity1 = componentData1.m_Owner;
        Attachment componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_AttachmentData.TryGetComponent(entity1, out componentData2) && componentData2.m_Attached != Entity.Null)
          entity1 = componentData2.m_Attached;
        return entity1;
      }

      private void AddOptionData(ref Game.Net.CarLane carLane, Game.City.City city)
      {
        if ((carLane.m_Flags & Game.Net.CarLaneFlags.Highway) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) || !CityUtils.CheckOption(city, CityOption.UnlimitedHighwaySpeed))
          return;
        carLane.m_SpeedLimit = 111.111115f;
      }

      private void AddOptionData(ref Game.Net.CarLane carLane, Owner owner, CarLaneData carLaneData)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_BorderDistrictData.HasComponent(owner.m_Owner))
          return;
        // ISSUE: reference to a compiler-generated field
        BorderDistrict borderDistrict = this.m_BorderDistrictData[owner.m_Owner];
        Game.Net.CarLaneFlags carLaneFlags1 = ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        Game.Net.CarLaneFlags carLaneFlags2 = ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        float2 speedLimit = (float2) carLane.m_SpeedLimit;
        // ISSUE: reference to a compiler-generated field
        if (this.m_DistrictData.HasComponent(borderDistrict.m_Left))
        {
          // ISSUE: reference to a compiler-generated field
          District district = this.m_DistrictData[borderDistrict.m_Left];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<DistrictModifier> districtModifier = this.m_DistrictModifiers[borderDistrict.m_Left];
          if ((carLaneData.m_RoadTypes & RoadTypes.Airplane) == RoadTypes.None)
          {
            if (AreaUtils.CheckOption(district, DistrictOption.ForbidCombustionEngines))
              carLaneFlags1 |= Game.Net.CarLaneFlags.ForbidCombustionEngines;
            else
              carLaneFlags2 |= Game.Net.CarLaneFlags.ForbidCombustionEngines;
            if (AreaUtils.CheckOption(district, DistrictOption.ForbidTransitTraffic))
              carLaneFlags1 |= Game.Net.CarLaneFlags.ForbidTransitTraffic;
            else
              carLaneFlags2 |= Game.Net.CarLaneFlags.ForbidTransitTraffic;
          }
          if ((carLaneData.m_RoadTypes & RoadTypes.Car) != RoadTypes.None && (carLane.m_Flags & Game.Net.CarLaneFlags.Highway) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
          {
            if (AreaUtils.CheckOption(district, DistrictOption.ForbidHeavyTraffic))
              carLaneFlags1 |= Game.Net.CarLaneFlags.ForbidHeavyTraffic;
            else
              carLaneFlags2 |= Game.Net.CarLaneFlags.ForbidHeavyTraffic;
            AreaUtils.ApplyModifier(ref speedLimit.x, districtModifier, DistrictModifierType.StreetSpeedLimit);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_DistrictData.HasComponent(borderDistrict.m_Right))
        {
          // ISSUE: reference to a compiler-generated field
          District district = this.m_DistrictData[borderDistrict.m_Right];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<DistrictModifier> districtModifier = this.m_DistrictModifiers[borderDistrict.m_Right];
          if ((carLaneData.m_RoadTypes & RoadTypes.Airplane) == RoadTypes.None)
          {
            if (AreaUtils.CheckOption(district, DistrictOption.ForbidCombustionEngines))
              carLaneFlags1 |= Game.Net.CarLaneFlags.ForbidCombustionEngines;
            else
              carLaneFlags2 |= Game.Net.CarLaneFlags.ForbidCombustionEngines;
            if (AreaUtils.CheckOption(district, DistrictOption.ForbidTransitTraffic))
              carLaneFlags1 |= Game.Net.CarLaneFlags.ForbidTransitTraffic;
            else
              carLaneFlags2 |= Game.Net.CarLaneFlags.ForbidTransitTraffic;
          }
          if ((carLaneData.m_RoadTypes & RoadTypes.Car) != RoadTypes.None && (carLane.m_Flags & Game.Net.CarLaneFlags.Highway) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
          {
            if (AreaUtils.CheckOption(district, DistrictOption.ForbidHeavyTraffic))
              carLaneFlags1 |= Game.Net.CarLaneFlags.ForbidHeavyTraffic;
            else
              carLaneFlags2 |= Game.Net.CarLaneFlags.ForbidHeavyTraffic;
            AreaUtils.ApplyModifier(ref speedLimit.y, districtModifier, DistrictModifierType.StreetSpeedLimit);
          }
        }
        carLane.m_Flags |= carLaneFlags1 & ~carLaneFlags2;
        if ((double) math.cmax(speedLimit) >= (double) carLane.m_SpeedLimit)
          carLane.m_SpeedLimit = math.max(carLane.m_SpeedLimit, math.cmin(speedLimit));
        else
          carLane.m_SpeedLimit = math.min(carLane.m_SpeedLimit, math.cmax(speedLimit));
      }

      private Bounds1 CheckBlockage(
        DynamicBuffer<LaneObject> laneObjects,
        out bool isEmergency,
        out bool isSecured)
      {
        Bounds1 bounds1 = new Bounds1(1f, 0.0f);
        isEmergency = false;
        isSecured = false;
        for (int index = 0; index < laneObjects.Length; ++index)
        {
          LaneObject laneObject = laneObjects[index];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_MovingData.HasComponent(laneObject.m_LaneObject))
          {
            bounds1 |= MathUtils.Bounds(laneObject.m_CurvePosition.x, laneObject.m_CurvePosition.y);
            InvolvedInAccident componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_InvolvedInAccidenteData.TryGetComponent(laneObject.m_LaneObject, out componentData1))
            {
              // ISSUE: reference to a compiler-generated method
              isSecured |= this.IsSecured(componentData1);
              isEmergency = true;
            }
            else
            {
              Car componentData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_CarData.TryGetComponent(laneObject.m_LaneObject, out componentData2))
                isEmergency |= (componentData2.m_Flags & CarFlags.Emergency) > (CarFlags) 0;
            }
          }
        }
        return bounds1;
      }

      private void AddBlockageData(ref Game.Net.CarLane carLane, Bounds1 bounds, bool addCaution)
      {
        if ((double) bounds.min > (double) bounds.max)
          return;
        carLane.m_BlockageStart = (byte) math.max(0, Mathf.FloorToInt(bounds.min * (float) byte.MaxValue));
        carLane.m_BlockageEnd = (byte) math.min((int) byte.MaxValue, Mathf.CeilToInt(bounds.max * (float) byte.MaxValue));
        if (!addCaution)
          return;
        carLane.m_CautionStart = carLane.m_BlockageStart;
        carLane.m_CautionEnd = carLane.m_BlockageEnd;
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
    private struct UpdateLaneData2Job : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          Entity entity = nativeArray1[index1];
          Owner owner = nativeArray2[index1];
          // ISSUE: reference to a compiler-generated field
          Game.Net.CarLane carLane1 = this.m_CarLaneData[entity];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[owner.m_Owner];
          bool flag = false;
          for (int index2 = 0; index2 < subLane1.Length; ++index2)
          {
            Entity subLane2 = subLane1[index2].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            if (subLane2 != entity && this.m_CarLaneData.HasComponent(subLane2))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Net.CarLane carLane2 = this.m_CarLaneData[subLane2];
              if ((int) carLane2.m_CarriagewayGroup == (int) carLane1.m_CarriagewayGroup && (int) carLane2.m_CautionEnd >= (int) carLane2.m_CautionStart)
              {
                if (((carLane1.m_Flags ^ carLane2.m_Flags) & Game.Net.CarLaneFlags.Invert) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
                {
                  carLane1.m_CautionStart = (byte) math.min((int) carLane1.m_CautionStart, (int) byte.MaxValue - (int) carLane2.m_CautionEnd);
                  carLane1.m_CautionEnd = (byte) math.max((int) carLane1.m_CautionEnd, (int) byte.MaxValue - (int) carLane2.m_CautionStart);
                }
                else
                {
                  carLane1.m_CautionStart = (byte) math.min((int) carLane1.m_CautionStart, (int) carLane2.m_CautionStart);
                  carLane1.m_CautionEnd = (byte) math.max((int) carLane1.m_CautionEnd, (int) carLane2.m_CautionEnd);
                }
                carLane1.m_Flags |= carLane2.m_Flags & Game.Net.CarLaneFlags.IsSecured;
                flag = true;
              }
            }
          }
          if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CarLaneData[entity] = carLane1;
          }
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

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Lane> __Game_Net_Lane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EdgeLane> __Game_Net_EdgeLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<MasterLane> __Game_Net_MasterLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SlaveLane> __Game_Net_SlaveLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LaneObject> __Game_Net_LaneObject_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Net.CarLane> __Game_Net_CarLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Net.PedestrianLane> __Game_Net_PedestrianLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Net.TrackLane> __Game_Net_TrackLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Road> __Game_Net_Road_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attachment> __Game_Objects_Attachment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.City.City> __Game_City_City_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> __Game_Areas_BorderDistrict_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<District> __Game_Areas_District_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Moving> __Game_Objects_Moving_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Car> __Game_Vehicles_Car_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<InvolvedInAccident> __Game_Events_InvolvedInAccident_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AccidentSite> __Game_Events_AccidentSite_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLaneData> __Game_Prefabs_CarLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedNode> __Game_Net_ConnectedNode_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<DistrictModifier> __Game_Areas_DistrictModifier_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<TargetElement> __Game_Events_TargetElement_RO_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_MasterLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MasterLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.CarLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.PedestrianLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrackLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.TrackLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.ConnectionLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentLookup = state.GetComponentLookup<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attachment_RO_ComponentLookup = state.GetComponentLookup<Attachment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_City_RO_ComponentLookup = state.GetComponentLookup<Game.City.City>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_BorderDistrict_RO_ComponentLookup = state.GetComponentLookup<BorderDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_District_RO_ComponentLookup = state.GetComponentLookup<District>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentLookup = state.GetComponentLookup<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RO_ComponentLookup = state.GetComponentLookup<Car>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InvolvedInAccident_RO_ComponentLookup = state.GetComponentLookup<InvolvedInAccident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_AccidentSite_RO_ComponentLookup = state.GetComponentLookup<AccidentSite>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarLaneData_RO_ComponentLookup = state.GetComponentLookup<CarLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RO_BufferLookup = state.GetBufferLookup<ConnectedNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_DistrictModifier_RO_BufferLookup = state.GetBufferLookup<DistrictModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_TargetElement_RO_BufferLookup = state.GetBufferLookup<TargetElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RW_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>();
      }
    }
  }
}
