// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TransportLineSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Common;
using Game.Notifications;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TransportLineSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    public const uint UPDATE_INTERVAL = 256;
    private EntityQuery m_LineQuery;
    private EntityArchetype m_VehicleRequestArchetype;
    private NativeArray<float> m_MaxTransportSpeed;
    private JobHandle m_MaxTransportSpeedDeps;
    private TimeSystem m_TimeSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private IconCommandSystem m_IconCommandSystem;
    private TransportLineSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LineQuery = this.GetEntityQuery(ComponentType.ReadOnly<Route>(), ComponentType.ReadWrite<TransportLine>(), ComponentType.ReadOnly<RouteWaypoint>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<TransportVehicleRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_MaxTransportSpeed = new NativeArray<float>(2, Allocator.Persistent);
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_MaxTransportSpeed.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_MaxTransportSpeed[0] = 0.0f;
      // ISSUE: reference to a compiler-generated field
      this.m_MaxTransportSpeed[1] = 0.0f;
      // ISSUE: reference to a compiler-generated field
      if (this.m_LineQuery.IsEmptyIgnoreFilter)
        return;
      NativeQueue<TransportLineSystem.VehicleAction> nativeQueue = new NativeQueue<TransportLineSystem.VehicleAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      float normalizedTime = this.m_TimeSystem.normalizedTime;
      bool flag = (double) normalizedTime < 0.25 || (double) normalizedTime >= 0.91666668653488159;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteInfo_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Odometer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CargoTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_VehicleTiming_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_DispatchedRequest_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteVehicle_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TransportLine_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteModifier_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_VehicleModel_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Route_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TransportLineSystem.TransportLineTickJob jobData1 = new TransportLineSystem.TransportLineTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_RouteType = this.__TypeHandle.__Game_Routes_Route_RO_ComponentTypeHandle,
        m_VehicleModelType = this.__TypeHandle.__Game_Routes_VehicleModel_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_RouteWaypointType = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferTypeHandle,
        m_RouteSegmentType = this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferTypeHandle,
        m_RouteModifierType = this.__TypeHandle.__Game_Routes_RouteModifier_RO_BufferTypeHandle,
        m_TransportLineType = this.__TypeHandle.__Game_Routes_TransportLine_RW_ComponentTypeHandle,
        m_RouteVehicleType = this.__TypeHandle.__Game_Routes_RouteVehicle_RW_BufferTypeHandle,
        m_DispatchedRequestType = this.__TypeHandle.__Game_Routes_DispatchedRequest_RW_BufferTypeHandle,
        m_ServiceRequestData = this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_VehicleTimingData = this.__TypeHandle.__Game_Routes_VehicleTiming_RO_ComponentLookup,
        m_ConnectedData = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup,
        m_TransportStopData = this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentLookup,
        m_CurrentRouteData = this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentLookup,
        m_DispatchedData = this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentLookup,
        m_CargoTransportData = this.__TypeHandle.__Game_Vehicles_CargoTransport_RO_ComponentLookup,
        m_PublicTransportData = this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup,
        m_OdometerData = this.__TypeHandle.__Game_Vehicles_Odometer_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_TransportLineDataData = this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_RouteInfoData = this.__TypeHandle.__Game_Routes_RouteInfo_RW_ComponentLookup,
        m_VehicleRequestArchetype = this.m_VehicleRequestArchetype,
        m_IsNight = flag,
        m_MaxTransportSpeed = this.m_MaxTransportSpeed,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_ActionQueue = nativeQueue.AsParallelWriter(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CargoTransport_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TransportLineSystem.VehicleActionJob jobData2 = new TransportLineSystem.VehicleActionJob()
      {
        m_CargoTransportData = this.__TypeHandle.__Game_Vehicles_CargoTransport_RW_ComponentLookup,
        m_PublicTransportData = this.__TypeHandle.__Game_Vehicles_PublicTransport_RW_ComponentLookup,
        m_ActionQueue = nativeQueue
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.ScheduleParallel<TransportLineSystem.TransportLineTickJob>(this.m_LineQuery, this.Dependency);
      JobHandle dependsOn = jobHandle;
      JobHandle inputDeps = jobData2.Schedule<TransportLineSystem.VehicleActionJob>(dependsOn);
      nativeQueue.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      this.m_MaxTransportSpeedDeps = jobHandle;
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(jobHandle);
      this.Dependency = inputDeps;
    }

    public void GetMaxTransportSpeed(
      out float maxPassengerTransportSpeed,
      out float maxCargoTransportSpeed)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_MaxTransportSpeedDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      maxPassengerTransportSpeed = this.m_MaxTransportSpeed[0];
      // ISSUE: reference to a compiler-generated field
      maxCargoTransportSpeed = this.m_MaxTransportSpeed[1];
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_MaxTransportSpeed[0]);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_MaxTransportSpeed[1]);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      float num1;
      reader.Read(out num1);
      float num2;
      reader.Read(out num2);
      // ISSUE: reference to a compiler-generated field
      this.m_MaxTransportSpeed[0] = num1;
      // ISSUE: reference to a compiler-generated field
      this.m_MaxTransportSpeed[1] = num2;
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_MaxTransportSpeed[0] = 277.777771f;
      // ISSUE: reference to a compiler-generated field
      this.m_MaxTransportSpeed[1] = 277.777771f;
    }

    public static int CalculateVehicleCount(float vehicleInterval, float lineDuration)
    {
      return math.max(1, (int) math.round(lineDuration / math.max(1f, vehicleInterval)));
    }

    public static float CalculateVehicleInterval(float lineDuration, int vehicleCount)
    {
      return lineDuration / (float) math.max(1, vehicleCount);
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
    public TransportLineSystem()
    {
    }

    private struct SortedVehicle : IComparable<TransportLineSystem.SortedVehicle>
    {
      public Entity m_Vehicle;
      public float m_Distance;

      public int CompareTo(TransportLineSystem.SortedVehicle other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.select(0, math.select(1, -1, (double) this.m_Distance < (double) other.m_Distance), (double) this.m_Distance != (double) other.m_Distance);
      }
    }

    private struct VehicleAction
    {
      public TransportLineSystem.VehicleActionType m_Type;
      public Entity m_Vehicle;
    }

    private enum VehicleActionType
    {
      AbandonRoute,
      CancelAbandon,
    }

    [BurstCompile]
    private struct TransportLineTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Route> m_RouteType;
      [ReadOnly]
      public ComponentTypeHandle<VehicleModel> m_VehicleModelType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<RouteWaypoint> m_RouteWaypointType;
      [ReadOnly]
      public BufferTypeHandle<RouteSegment> m_RouteSegmentType;
      [ReadOnly]
      public BufferTypeHandle<RouteModifier> m_RouteModifierType;
      public ComponentTypeHandle<TransportLine> m_TransportLineType;
      public BufferTypeHandle<RouteVehicle> m_RouteVehicleType;
      public BufferTypeHandle<DispatchedRequest> m_DispatchedRequestType;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> m_ServiceRequestData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<VehicleTiming> m_VehicleTimingData;
      [ReadOnly]
      public ComponentLookup<Connected> m_ConnectedData;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TransportStop> m_TransportStopData;
      [ReadOnly]
      public ComponentLookup<CurrentRoute> m_CurrentRouteData;
      [ReadOnly]
      public ComponentLookup<Dispatched> m_DispatchedData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.CargoTransport> m_CargoTransportData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
      [ReadOnly]
      public ComponentLookup<Odometer> m_OdometerData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<TransportLineData> m_TransportLineDataData;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<RouteInfo> m_RouteInfoData;
      [ReadOnly]
      public EntityArchetype m_VehicleRequestArchetype;
      [ReadOnly]
      public bool m_IsNight;
      [NativeDisableParallelForRestriction]
      public NativeArray<float> m_MaxTransportSpeed;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<TransportLineSystem.VehicleAction>.ParallelWriter m_ActionQueue;
      public IconCommandBuffer m_IconCommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Route> nativeArray2 = chunk.GetNativeArray<Route>(ref this.m_RouteType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<VehicleModel> nativeArray3 = chunk.GetNativeArray<VehicleModel>(ref this.m_VehicleModelType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TransportLine> nativeArray5 = chunk.GetNativeArray<TransportLine>(ref this.m_TransportLineType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<RouteWaypoint> bufferAccessor1 = chunk.GetBufferAccessor<RouteWaypoint>(ref this.m_RouteWaypointType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<RouteSegment> bufferAccessor2 = chunk.GetBufferAccessor<RouteSegment>(ref this.m_RouteSegmentType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<RouteModifier> bufferAccessor3 = chunk.GetBufferAccessor<RouteModifier>(ref this.m_RouteModifierType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<RouteVehicle> bufferAccessor4 = chunk.GetBufferAccessor<RouteVehicle>(ref this.m_RouteVehicleType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<DispatchedRequest> bufferAccessor5 = chunk.GetBufferAccessor<DispatchedRequest>(ref this.m_DispatchedRequestType);
        NativeList<TransportLineSystem.SortedVehicle> sortBuffer = new NativeList<TransportLineSystem.SortedVehicle>();
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Route route = nativeArray2[index];
          VehicleModel vehicleModel = nativeArray3[index];
          PrefabRef prefabRef = nativeArray4[index];
          TransportLine transportLine = nativeArray5[index];
          DynamicBuffer<RouteWaypoint> waypoints = bufferAccessor1[index];
          DynamicBuffer<RouteSegment> segments = bufferAccessor2[index];
          DynamicBuffer<RouteModifier> modifiers = bufferAccessor3[index];
          DynamicBuffer<RouteVehicle> vehicles = bufferAccessor4[index];
          DynamicBuffer<DispatchedRequest> requests = bufferAccessor5[index];
          // ISSUE: reference to a compiler-generated field
          TransportLineData prefabLineData = this.m_TransportLineDataData[prefabRef.m_Prefab];
          float defaultVehicleInterval = prefabLineData.m_DefaultVehicleInterval;
          RouteUtils.ApplyModifier(ref defaultVehicleInterval, modifiers, RouteModifierType.VehicleInterval);
          ushort num1 = 0;
          if (RouteUtils.CheckOption(route, RouteOption.PaidTicket))
          {
            float f = 0.0f;
            RouteUtils.ApplyModifier(ref f, modifiers, RouteModifierType.TicketPrice);
            num1 = (ushort) math.clamp(Mathf.RoundToInt(f), 0, (int) ushort.MaxValue);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool3 isActive = !RouteUtils.CheckOption(route, RouteOption.Inactive) ? (!RouteUtils.CheckOption(route, RouteOption.Day) ? (!RouteUtils.CheckOption(route, RouteOption.Night) ? (bool3) true : new bool3(this.m_IsNight, false, true)) : new bool3(!this.m_IsNight, true, false)) : (bool3) false;
          float lineDuration;
          float stableDuration;
          // ISSUE: reference to a compiler-generated method
          this.RefreshLineSegments(unfilteredChunkIndex, prefabLineData, waypoints, segments, isActive, out lineDuration, out stableDuration);
          // ISSUE: reference to a compiler-generated method
          int vehicleCount = TransportLineSystem.CalculateVehicleCount(defaultVehicleInterval, stableDuration);
          // ISSUE: reference to a compiler-generated method
          float num2 = math.min(defaultVehicleInterval * 10f, TransportLineSystem.CalculateVehicleInterval(lineDuration, vehicleCount));
          bool flag1 = false;
          if ((double) math.abs(num2 - transportLine.m_VehicleInterval) >= 1.0)
          {
            transportLine.m_VehicleInterval = num2;
            flag1 = true;
          }
          if ((int) num1 != (int) transportLine.m_TicketPrice)
          {
            transportLine.m_TicketPrice = num1;
            flag1 = true;
          }
          if (flag1)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateStopPathfind(unfilteredChunkIndex, waypoints);
          }
          int totalCount;
          int continuingCount;
          // ISSUE: reference to a compiler-generated method
          this.CheckVehicles(entity, vehicleModel, vehicles, out totalCount, out continuingCount);
          int targetCount = math.select(0, vehicleCount, isActive.x);
          if (continuingCount < targetCount && continuingCount < totalCount)
          {
            int count = targetCount - continuingCount;
            // ISSUE: reference to a compiler-generated method
            this.CancelAbandon(vehicleModel, vehicles, count, ref sortBuffer);
          }
          else if (continuingCount > targetCount)
          {
            int count = continuingCount - targetCount;
            // ISSUE: reference to a compiler-generated method
            this.AbandonVehicles(vehicleModel, vehicles, count, ref sortBuffer);
          }
          // ISSUE: reference to a compiler-generated method
          this.CheckRequests(ref transportLine, requests);
          bool flag2 = false;
          if (totalCount < targetCount)
          {
            transportLine.m_Flags |= TransportLineFlags.RequireVehicles;
            totalCount += requests.Length;
            if (totalCount < targetCount)
            {
              // ISSUE: reference to a compiler-generated method
              flag2 = !this.RequestNewVehicleIfNeeded(unfilteredChunkIndex, entity, transportLine, totalCount, targetCount);
            }
          }
          else
            transportLine.m_Flags &= ~TransportLineFlags.RequireVehicles;
          if (flag2)
          {
            if ((transportLine.m_Flags & TransportLineFlags.NotEnoughVehicles) == (TransportLineFlags) 0 && waypoints.Length != 0)
            {
              transportLine.m_Flags |= TransportLineFlags.NotEnoughVehicles;
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Add(entity, prefabLineData.m_VehicleNotification, IconPriority.Problem);
            }
          }
          else if ((transportLine.m_Flags & TransportLineFlags.NotEnoughVehicles) != (TransportLineFlags) 0)
          {
            transportLine.m_Flags &= ~TransportLineFlags.NotEnoughVehicles;
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Remove(entity, prefabLineData.m_VehicleNotification);
          }
          nativeArray5[index] = transportLine;
        }
        if (!sortBuffer.IsCreated)
          return;
        sortBuffer.Dispose();
      }

      private void UpdateStopPathfind(int jobIndex, DynamicBuffer<RouteWaypoint> waypoints)
      {
        for (int index = 0; index < waypoints.Length; ++index)
        {
          Entity waypoint = waypoints[index].m_Waypoint;
          // ISSUE: reference to a compiler-generated field
          if (this.m_VehicleTimingData.HasComponent(waypoint))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, waypoint, new PathfindUpdated());
          }
        }
      }

      private void CheckVehicles(
        Entity route,
        VehicleModel vehicleModel,
        DynamicBuffer<RouteVehicle> vehicles,
        out int totalCount,
        out int continuingCount)
      {
        totalCount = 0;
        continuingCount = 0;
        while (totalCount < vehicles.Length)
        {
          Entity vehicle = vehicles[totalCount].m_Vehicle;
          CurrentRoute currentRoute = new CurrentRoute();
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentRouteData.HasComponent(vehicle))
          {
            // ISSUE: reference to a compiler-generated field
            currentRoute = this.m_CurrentRouteData[vehicle];
          }
          if (currentRoute.m_Route == route)
          {
            ++totalCount;
            Game.Vehicles.CargoTransport cargoTransport = new Game.Vehicles.CargoTransport();
            // ISSUE: reference to a compiler-generated field
            if (this.m_CargoTransportData.HasComponent(vehicle))
            {
              // ISSUE: reference to a compiler-generated field
              cargoTransport = this.m_CargoTransportData[vehicle];
            }
            Game.Vehicles.PublicTransport publicTransport = new Game.Vehicles.PublicTransport();
            // ISSUE: reference to a compiler-generated field
            if (this.m_PublicTransportData.HasComponent(vehicle))
            {
              // ISSUE: reference to a compiler-generated field
              publicTransport = this.m_PublicTransportData[vehicle];
            }
            if ((cargoTransport.m_State & CargoTransportFlags.AbandonRoute) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.AbandonRoute) == (PublicTransportFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef = this.m_PrefabRefData[vehicle];
              DynamicBuffer<LayoutElement> bufferData;
              // ISSUE: reference to a compiler-generated field
              this.m_LayoutElements.TryGetBuffer(vehicle, out bufferData);
              // ISSUE: reference to a compiler-generated field
              if (RouteUtils.CheckVehicleModel(vehicleModel, prefabRef, bufferData, ref this.m_PrefabRefData))
              {
                ++continuingCount;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_ActionQueue.Enqueue(new TransportLineSystem.VehicleAction()
                {
                  m_Type = TransportLineSystem.VehicleActionType.AbandonRoute,
                  m_Vehicle = vehicle
                });
              }
            }
          }
          else
            vehicles.RemoveAt(totalCount);
        }
      }

      private void AbandonVehicles(
        VehicleModel vehicleModel,
        DynamicBuffer<RouteVehicle> vehicles,
        int count,
        ref NativeList<TransportLineSystem.SortedVehicle> sortBuffer)
      {
        if (!sortBuffer.IsCreated)
          sortBuffer = new NativeList<TransportLineSystem.SortedVehicle>(vehicles.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < vehicles.Length; ++index)
        {
          Entity vehicle = vehicles[index].m_Vehicle;
          Game.Vehicles.CargoTransport cargoTransport = new Game.Vehicles.CargoTransport();
          // ISSUE: reference to a compiler-generated field
          if (this.m_CargoTransportData.HasComponent(vehicle))
          {
            // ISSUE: reference to a compiler-generated field
            cargoTransport = this.m_CargoTransportData[vehicle];
          }
          Game.Vehicles.PublicTransport publicTransport = new Game.Vehicles.PublicTransport();
          // ISSUE: reference to a compiler-generated field
          if (this.m_PublicTransportData.HasComponent(vehicle))
          {
            // ISSUE: reference to a compiler-generated field
            publicTransport = this.m_PublicTransportData[vehicle];
          }
          if ((cargoTransport.m_State & CargoTransportFlags.AbandonRoute) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.AbandonRoute) == (PublicTransportFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[vehicle];
            DynamicBuffer<LayoutElement> bufferData;
            // ISSUE: reference to a compiler-generated field
            this.m_LayoutElements.TryGetBuffer(vehicle, out bufferData);
            // ISSUE: reference to a compiler-generated field
            if (RouteUtils.CheckVehicleModel(vehicleModel, prefabRef, bufferData, ref this.m_PrefabRefData))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              sortBuffer.Add(new TransportLineSystem.SortedVehicle()
              {
                m_Vehicle = vehicle,
                m_Distance = this.m_OdometerData[vehicle].m_Distance
              });
            }
          }
        }
        sortBuffer.Sort<TransportLineSystem.SortedVehicle>();
        count = math.min(count, sortBuffer.Length);
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Entity vehicle = sortBuffer[sortBuffer.Length - index - 1].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ActionQueue.Enqueue(new TransportLineSystem.VehicleAction()
          {
            m_Type = TransportLineSystem.VehicleActionType.AbandonRoute,
            m_Vehicle = vehicle
          });
        }
        sortBuffer.Clear();
      }

      private void CancelAbandon(
        VehicleModel vehicleModel,
        DynamicBuffer<RouteVehicle> vehicles,
        int count,
        ref NativeList<TransportLineSystem.SortedVehicle> sortBuffer)
      {
        if (!sortBuffer.IsCreated)
          sortBuffer = new NativeList<TransportLineSystem.SortedVehicle>(vehicles.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < vehicles.Length; ++index)
        {
          Entity vehicle = vehicles[index].m_Vehicle;
          Game.Vehicles.CargoTransport cargoTransport = new Game.Vehicles.CargoTransport();
          // ISSUE: reference to a compiler-generated field
          if (this.m_CargoTransportData.HasComponent(vehicle))
          {
            // ISSUE: reference to a compiler-generated field
            cargoTransport = this.m_CargoTransportData[vehicle];
          }
          Game.Vehicles.PublicTransport publicTransport = new Game.Vehicles.PublicTransport();
          // ISSUE: reference to a compiler-generated field
          if (this.m_PublicTransportData.HasComponent(vehicle))
          {
            // ISSUE: reference to a compiler-generated field
            publicTransport = this.m_PublicTransportData[vehicle];
          }
          if ((cargoTransport.m_State & (CargoTransportFlags.AbandonRoute | CargoTransportFlags.Disabled)) == CargoTransportFlags.AbandonRoute || (publicTransport.m_State & (PublicTransportFlags.AbandonRoute | PublicTransportFlags.Disabled)) == PublicTransportFlags.AbandonRoute)
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[vehicle];
            DynamicBuffer<LayoutElement> bufferData;
            // ISSUE: reference to a compiler-generated field
            this.m_LayoutElements.TryGetBuffer(vehicle, out bufferData);
            // ISSUE: reference to a compiler-generated field
            if (RouteUtils.CheckVehicleModel(vehicleModel, prefabRef, bufferData, ref this.m_PrefabRefData))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              sortBuffer.Add(new TransportLineSystem.SortedVehicle()
              {
                m_Vehicle = vehicle,
                m_Distance = this.m_OdometerData[vehicle].m_Distance
              });
            }
          }
        }
        sortBuffer.Sort<TransportLineSystem.SortedVehicle>();
        count = math.min(count, sortBuffer.Length);
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Entity vehicle = sortBuffer[index].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ActionQueue.Enqueue(new TransportLineSystem.VehicleAction()
          {
            m_Type = TransportLineSystem.VehicleActionType.CancelAbandon,
            m_Vehicle = vehicle
          });
        }
        sortBuffer.Clear();
      }

      private unsafe void RefreshLineSegments(
        int jobIndex,
        TransportLineData prefabLineData,
        DynamicBuffer<RouteWaypoint> waypoints,
        DynamicBuffer<RouteSegment> segments,
        bool3 isActive,
        out float lineDuration,
        out float stableDuration)
      {
        lineDuration = 0.0f;
        stableDuration = 0.0f;
        float x1 = 0.0f;
        float y1 = 0.0f;
        float y2 = 0.0f;
        float x2 = 0.0f;
        int num1 = 0;
        int num2 = 0;
        for (int index = 0; index < waypoints.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_VehicleTimingData.HasComponent(waypoints[index].m_Waypoint))
          {
            num2 = index;
            break;
          }
        }
        RouteInfoFlags routeInfoFlags = (RouteInfoFlags) 0;
        if (!isActive.y)
          routeInfoFlags |= RouteInfoFlags.InactiveDay;
        if (!isActive.z)
          routeInfoFlags |= RouteInfoFlags.InactiveNight;
        int num3 = num2;
        for (int index1 = 0; index1 < waypoints.Length; ++index1)
        {
          int2 a1 = (int2) (num2 + index1);
          ++a1.y;
          a1 = math.select(a1, a1 - waypoints.Length, a1 >= waypoints.Length);
          ++num1;
          Entity waypoint = waypoints[a1.y].m_Waypoint;
          Entity segment1 = segments[a1.x].m_Segment;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathInformationData.HasComponent(segment1))
          {
            // ISSUE: reference to a compiler-generated field
            PathInformation pathInformation = this.m_PathInformationData[segment1];
            x1 += pathInformation.m_Duration;
            y1 += pathInformation.m_Distance;
            y2 += pathInformation.m_Duration;
            stableDuration += pathInformation.m_Duration;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_VehicleTimingData.HasComponent(waypoint))
          {
            // ISSUE: reference to a compiler-generated field
            VehicleTiming vehicleTiming = this.m_VehicleTimingData[waypoint];
            float stopDuration = prefabLineData.m_StopDuration;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectedData.HasComponent(waypoint))
            {
              // ISSUE: reference to a compiler-generated field
              Connected connected = this.m_ConnectedData[waypoint];
              // ISSUE: reference to a compiler-generated field
              if (this.m_TransportStopData.HasComponent(connected.m_Connected))
              {
                // ISSUE: reference to a compiler-generated field
                stopDuration = RouteUtils.GetStopDuration(prefabLineData, this.m_TransportStopData[connected.m_Connected]);
              }
            }
            float y3 = math.max(x1, vehicleTiming.m_AverageTravelTime) + stopDuration;
            lineDuration += y3;
            stableDuration += prefabLineData.m_StopDuration;
            for (int index2 = 0; index2 < num1; ++index2)
            {
              int a2 = num3 + index2;
              int index3 = math.select(a2, a2 - waypoints.Length, a2 >= waypoints.Length);
              Entity segment2 = segments[index3].m_Segment;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PathInformationData.HasComponent(segment2))
              {
                // ISSUE: reference to a compiler-generated field
                PathInformation pathInformation = this.m_PathInformationData[segment2];
                // ISSUE: reference to a compiler-generated field
                RouteInfo routeInfo1 = this.m_RouteInfoData[segment2];
                RouteInfo routeInfo2 = routeInfo1 with
                {
                  m_Duration = pathInformation.m_Duration * math.max(1f, y3 / math.max(1f, y2)),
                  m_Distance = pathInformation.m_Distance,
                  m_Flags = routeInfoFlags
                };
                if ((double) routeInfo2.m_Distance != (double) routeInfo1.m_Distance || (double) math.abs(routeInfo2.m_Duration - routeInfo1.m_Duration) >= 1.0 || routeInfo2.m_Flags != routeInfo1.m_Flags)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_RouteInfoData[segment2] = routeInfo2;
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, segment2, new PathfindUpdated());
                }
              }
            }
            x2 = math.max(x2, math.max(1f, y1) / math.max(1f, y3));
            x1 = 0.0f;
            y1 = 0.0f;
            y2 = 0.0f;
            num1 = 0;
            num3 = a1.y;
          }
        }
        if (prefabLineData.m_PassengerTransport)
        {
          // ISSUE: reference to a compiler-generated field
          float* unsafePtr = (float*) this.m_MaxTransportSpeed.GetUnsafePtr<float>();
          if ((double) x2 > (double) *unsafePtr)
          {
            float num4;
            do
            {
              num4 = x2;
              x2 = Interlocked.Exchange(ref *unsafePtr, num4);
            }
            while ((double) x2 > (double) num4);
          }
        }
        if (!prefabLineData.m_CargoTransport)
          return;
        // ISSUE: reference to a compiler-generated field
        float* numPtr = (float*) ((IntPtr) this.m_MaxTransportSpeed.GetUnsafePtr<float>() + 4);
        if ((double) x2 <= (double) *numPtr)
          return;
        float num5;
        do
        {
          num5 = x2;
          x2 = Interlocked.Exchange(ref *numPtr, num5);
        }
        while ((double) x2 > (double) num5);
      }

      private void CheckRequests(
        ref TransportLine transportLine,
        DynamicBuffer<DispatchedRequest> requests)
      {
        for (int index = 0; index < requests.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_ServiceRequestData.HasComponent(requests[index].m_VehicleRequest))
            requests.RemoveAtSwapBack(index--);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_DispatchedData.HasComponent(transportLine.m_VehicleRequest))
          return;
        requests.Add(new DispatchedRequest()
        {
          m_VehicleRequest = transportLine.m_VehicleRequest
        });
        transportLine.m_VehicleRequest = Entity.Null;
      }

      private bool RequestNewVehicleIfNeeded(
        int jobIndex,
        Entity entity,
        TransportLine transportLine,
        int vehicleCount,
        int targetCount)
      {
        ServiceRequest componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceRequestData.TryGetComponent(transportLine.m_VehicleRequest, out componentData))
          return componentData.m_FailCount < (byte) 2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_VehicleRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<TransportVehicleRequest>(jobIndex, entity1, new TransportVehicleRequest(entity, (float) (1.0 - (double) vehicleCount / (double) targetCount)));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(8U));
        return true;
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
    private struct VehicleActionJob : IJob
    {
      public ComponentLookup<Game.Vehicles.CargoTransport> m_CargoTransportData;
      public ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
      public NativeQueue<TransportLineSystem.VehicleAction> m_ActionQueue;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_ActionQueue.Count;
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          TransportLineSystem.VehicleAction vehicleAction = this.m_ActionQueue.Dequeue();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          TransportLineSystem.VehicleActionType type = vehicleAction.m_Type;
          switch (type)
          {
            case TransportLineSystem.VehicleActionType.AbandonRoute:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CargoTransportData.HasComponent(vehicleAction.m_Vehicle))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Game.Vehicles.CargoTransport cargoTransport = this.m_CargoTransportData[vehicleAction.m_Vehicle];
                cargoTransport.m_State |= CargoTransportFlags.AbandonRoute;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CargoTransportData[vehicleAction.m_Vehicle] = cargoTransport;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PublicTransportData.HasComponent(vehicleAction.m_Vehicle))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Game.Vehicles.PublicTransport publicTransport = this.m_PublicTransportData[vehicleAction.m_Vehicle];
                publicTransport.m_State |= PublicTransportFlags.AbandonRoute;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_PublicTransportData[vehicleAction.m_Vehicle] = publicTransport;
                break;
              }
              break;
            case TransportLineSystem.VehicleActionType.CancelAbandon:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CargoTransportData.HasComponent(vehicleAction.m_Vehicle))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Game.Vehicles.CargoTransport cargoTransport = this.m_CargoTransportData[vehicleAction.m_Vehicle];
                cargoTransport.m_State &= ~CargoTransportFlags.AbandonRoute;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CargoTransportData[vehicleAction.m_Vehicle] = cargoTransport;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PublicTransportData.HasComponent(vehicleAction.m_Vehicle))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Game.Vehicles.PublicTransport publicTransport = this.m_PublicTransportData[vehicleAction.m_Vehicle];
                publicTransport.m_State &= ~PublicTransportFlags.AbandonRoute;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_PublicTransportData[vehicleAction.m_Vehicle] = publicTransport;
                break;
              }
              break;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Route> __Game_Routes_Route_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<VehicleModel> __Game_Routes_VehicleModel_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<RouteSegment> __Game_Routes_RouteSegment_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<RouteModifier> __Game_Routes_RouteModifier_RO_BufferTypeHandle;
      public ComponentTypeHandle<TransportLine> __Game_Routes_TransportLine_RW_ComponentTypeHandle;
      public BufferTypeHandle<RouteVehicle> __Game_Routes_RouteVehicle_RW_BufferTypeHandle;
      public BufferTypeHandle<DispatchedRequest> __Game_Routes_DispatchedRequest_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> __Game_Simulation_ServiceRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<VehicleTiming> __Game_Routes_VehicleTiming_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Connected> __Game_Routes_Connected_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TransportStop> __Game_Routes_TransportStop_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentRoute> __Game_Routes_CurrentRoute_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Dispatched> __Game_Simulation_Dispatched_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.CargoTransport> __Game_Vehicles_CargoTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> __Game_Vehicles_PublicTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Odometer> __Game_Vehicles_Odometer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportLineData> __Game_Prefabs_TransportLineData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      public ComponentLookup<RouteInfo> __Game_Routes_RouteInfo_RW_ComponentLookup;
      public ComponentLookup<Game.Vehicles.CargoTransport> __Game_Vehicles_CargoTransport_RW_ComponentLookup;
      public ComponentLookup<Game.Vehicles.PublicTransport> __Game_Vehicles_PublicTransport_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Route_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Route>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_VehicleModel_RO_ComponentTypeHandle = state.GetComponentTypeHandle<VehicleModel>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferTypeHandle = state.GetBufferTypeHandle<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteSegment_RO_BufferTypeHandle = state.GetBufferTypeHandle<RouteSegment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteModifier_RO_BufferTypeHandle = state.GetBufferTypeHandle<RouteModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TransportLine_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TransportLine>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteVehicle_RW_BufferTypeHandle = state.GetBufferTypeHandle<RouteVehicle>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_DispatchedRequest_RW_BufferTypeHandle = state.GetBufferTypeHandle<DispatchedRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RO_ComponentLookup = state.GetComponentLookup<ServiceRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_VehicleTiming_RO_ComponentLookup = state.GetComponentLookup<VehicleTiming>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RO_ComponentLookup = state.GetComponentLookup<Connected>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TransportStop_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.TransportStop>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_CurrentRoute_RO_ComponentLookup = state.GetComponentLookup<CurrentRoute>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Dispatched_RO_ComponentLookup = state.GetComponentLookup<Dispatched>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CargoTransport_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.CargoTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PublicTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Odometer_RO_ComponentLookup = state.GetComponentLookup<Odometer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportLineData_RO_ComponentLookup = state.GetComponentLookup<TransportLineData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteInfo_RW_ComponentLookup = state.GetComponentLookup<RouteInfo>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CargoTransport_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.CargoTransport>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PublicTransport>();
      }
    }
  }
}
