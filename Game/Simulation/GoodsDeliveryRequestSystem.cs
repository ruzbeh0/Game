// Decompiled with JetBrains decompiler
// Type: Game.Simulation.GoodsDeliveryRequestSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Citizens;
using Game.Companies;
using Game.Economy;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class GoodsDeliveryRequestSystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private ResourceSystem m_ResourceSystem;
    private CitySystem m_CitySystem;
    private PropertyRenterSystem m_PropertyRenterSystem;
    private EntityQuery m_RequestGroup;
    private NativeQueue<GoodsDeliveryRequestSystem.DeliveryOrder> m_DeliveryQueue;
    private GoodsDeliveryRequestSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PropertyRenterSystem = this.World.GetOrCreateSystemManaged<PropertyRenterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RequestGroup = this.GetEntityQuery(ComponentType.ReadOnly<GoodsDeliveryRequest>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeliveryQueue = new NativeQueue<GoodsDeliveryRequestSystem.DeliveryOrder>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_RequestGroup);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DeliveryQueue.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_GoodsDeliveryRequest_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GoodsDeliveryRequestSystem.FindSellerJob jobData1 = new GoodsDeliveryRequestSystem.FindSellerJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_RequestType = this.__TypeHandle.__Game_Simulation_GoodsDeliveryRequest_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_DeliveryQueue = this.m_DeliveryQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<GoodsDeliveryRequestSystem.FindSellerJob>(this.m_RequestGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GoodsDeliveryRequestSystem.DispatchJob jobData2 = new GoodsDeliveryRequestSystem.DispatchJob()
      {
        m_TripNeededs = this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferLookup,
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup,
        m_OutsideConnections = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_StorageCompanies = this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_DeliveryQueue = this.m_DeliveryQueue
      };
      this.Dependency = jobData2.Schedule<GoodsDeliveryRequestSystem.DispatchJob>(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
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
    public GoodsDeliveryRequestSystem()
    {
    }

    private struct DeliveryOrder
    {
      public Entity m_Entity;
      public TripNeeded m_Trip;
    }

    [BurstCompile]
    private struct DispatchJob : IJob
    {
      public BufferLookup<TripNeeded> m_TripNeededs;
      public NativeQueue<GoodsDeliveryRequestSystem.DeliveryOrder> m_DeliveryQueue;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> m_StorageCompanies;
      public BufferLookup<Game.Economy.Resources> m_Resources;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        GoodsDeliveryRequestSystem.DeliveryOrder deliveryOrder;
        // ISSUE: reference to a compiler-generated field
        while (this.m_DeliveryQueue.TryDequeue(out deliveryOrder))
        {
          // ISSUE: reference to a compiler-generated field
          Entity targetAgent = deliveryOrder.m_Trip.m_TargetAgent;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_ResourceDatas[this.m_ResourcePrefabs[deliveryOrder.m_Trip.m_Resource]].m_Weight > 0.0 && this.m_TripNeededs.HasBuffer(deliveryOrder.m_Entity))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_TripNeededs[deliveryOrder.m_Entity].Add(deliveryOrder.m_Trip);
          }
          // ISSUE: reference to a compiler-generated field
          int x = deliveryOrder.m_Trip.m_Data;
          // ISSUE: reference to a compiler-generated field
          if (deliveryOrder.m_Trip.m_Purpose == Game.Citizens.Purpose.Collect)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_Resources.HasBuffer(targetAgent))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Game.Economy.Resources> resource = this.m_Resources[targetAgent];
              // ISSUE: reference to a compiler-generated field
              x = math.min(x, EconomyUtils.GetResources(deliveryOrder.m_Trip.m_Resource, resource));
              // ISSUE: reference to a compiler-generated field
              EconomyUtils.AddResources(deliveryOrder.m_Trip.m_Resource, -x, resource);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Resources.HasBuffer(deliveryOrder.m_Entity))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Game.Economy.Resources> resource = this.m_Resources[deliveryOrder.m_Entity];
              // ISSUE: reference to a compiler-generated field
              x = math.min(x, EconomyUtils.GetResources(deliveryOrder.m_Trip.m_Resource, resource));
              // ISSUE: reference to a compiler-generated field
              EconomyUtils.AddResources(deliveryOrder.m_Trip.m_Resource, -x, resource);
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int amount = Mathf.RoundToInt(EconomyUtils.GetMarketPrice(deliveryOrder.m_Trip.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas) * (float) x);
          // ISSUE: reference to a compiler-generated field
          if (deliveryOrder.m_Trip.m_Purpose == Game.Citizens.Purpose.Collect)
            amount *= -1;
          // ISSUE: reference to a compiler-generated field
          Entity entity = deliveryOrder.m_Entity;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_OutsideConnections.HasComponent(entity) && !this.m_StorageCompanies.HasComponent(entity) && this.m_Resources.HasBuffer(entity))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Game.Economy.Resources> resource = this.m_Resources[entity];
            EconomyUtils.AddResources(Resource.Money, amount, resource);
          }
        }
      }
    }

    [BurstCompile]
    private struct FindSellerJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<GoodsDeliveryRequest> m_RequestType;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      public NativeQueue<GoodsDeliveryRequestSystem.DeliveryOrder>.ParallelWriter m_DeliveryQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        if (!chunk.Has<PathInformation>(ref this.m_PathInformationType))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<GoodsDeliveryRequest> nativeArray2 = chunk.GetNativeArray<GoodsDeliveryRequest>(ref this.m_RequestType);
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            Entity requestEntity = nativeArray1[index];
            GoodsDeliveryRequest requestData = nativeArray2[index];
            // ISSUE: reference to a compiler-generated method
            this.FindVehicleSource(unfilteredChunkIndex, requestEntity, requestData);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<PathInformation> nativeArray3 = chunk.GetNativeArray<PathInformation>(ref this.m_PathInformationType);
          if (nativeArray3.Length == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray4 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<GoodsDeliveryRequest> nativeArray5 = chunk.GetNativeArray<GoodsDeliveryRequest>(ref this.m_RequestType);
          for (int index = 0; index < nativeArray5.Length; ++index)
          {
            Entity e = nativeArray4[index];
            GoodsDeliveryRequest goodsDeliveryRequest = nativeArray5[index];
            PathInformation pathInformation = nativeArray3[index];
            if ((pathInformation.m_State & PathFlags.Pending) == (PathFlags) 0)
            {
              if (pathInformation.m_Origin != Entity.Null)
              {
                Game.Citizens.Purpose purpose = (goodsDeliveryRequest.m_Flags & GoodsDeliveryFlags.ResourceExportTarget) == (GoodsDeliveryFlags) 0 ? ((goodsDeliveryRequest.m_Flags & GoodsDeliveryFlags.BuildingUpkeep) == (GoodsDeliveryFlags) 0 ? Game.Citizens.Purpose.Delivery : Game.Citizens.Purpose.UpkeepDelivery) : Game.Citizens.Purpose.Collect;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_DeliveryQueue.Enqueue(new GoodsDeliveryRequestSystem.DeliveryOrder()
                {
                  m_Entity = pathInformation.m_Origin,
                  m_Trip = new TripNeeded()
                  {
                    m_Data = goodsDeliveryRequest.m_Amount,
                    m_Purpose = purpose,
                    m_Resource = goodsDeliveryRequest.m_Resource,
                    m_TargetAgent = goodsDeliveryRequest.m_Target
                  }
                });
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, e);
            }
          }
        }
      }

      private void FindVehicleSource(
        int jobIndex,
        Entity requestEntity,
        GoodsDeliveryRequest requestData)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float transportCost = EconomyUtils.GetTransportCost(100f, requestData.m_Amount, this.m_ResourceDatas[this.m_ResourcePrefabs[requestData.m_Resource]].m_Weight, StorageTransferFlags.Car);
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) 111.111115f,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, transportCost, 1f),
          m_Methods = PathMethod.Road | PathMethod.CargoLoading,
          m_IgnoredRules = RuleFlags.ForbidSlowTraffic
        };
        SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = (requestData.m_Flags & GoodsDeliveryFlags.ResourceExportTarget) != (GoodsDeliveryFlags) 0 ? SetupTargetType.ResourceExport : SetupTargetType.ResourceSeller;
        setupQueueTarget.m_Methods = PathMethod.Road | PathMethod.CargoLoading;
        setupQueueTarget.m_RoadTypes = RoadTypes.Car;
        setupQueueTarget.m_Value = requestData.m_Amount;
        setupQueueTarget.m_Resource = requestData.m_Resource;
        SetupQueueTarget origin = setupQueueTarget;
        setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
        setupQueueTarget.m_Methods = PathMethod.Road | PathMethod.CargoLoading;
        setupQueueTarget.m_RoadTypes = RoadTypes.Car;
        setupQueueTarget.m_Entity = requestData.m_Target;
        SetupQueueTarget destination = setupQueueTarget;
        if ((requestData.m_Flags & GoodsDeliveryFlags.CommercialAllowed) != (GoodsDeliveryFlags) 0)
          origin.m_Flags |= SetupTargetFlags.Commercial;
        if ((requestData.m_Flags & GoodsDeliveryFlags.IndustrialAllowed) != (GoodsDeliveryFlags) 0)
          origin.m_Flags |= SetupTargetFlags.Industrial;
        if ((requestData.m_Flags & GoodsDeliveryFlags.ImportAllowed) != (GoodsDeliveryFlags) 0)
          origin.m_Flags |= SetupTargetFlags.Import;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((double) this.m_ResourceDatas[this.m_ResourcePrefabs[requestData.m_Resource]].m_Weight > 0.0)
          origin.m_Flags |= SetupTargetFlags.RequireTransport;
        // ISSUE: reference to a compiler-generated field
        this.m_PathfindQueue.Enqueue(new SetupQueueItem(requestEntity, parameters, origin, destination));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathInformation>(jobIndex, requestEntity, new PathInformation()
        {
          m_State = PathFlags.Pending
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddBuffer<PathElement>(jobIndex, requestEntity);
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
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<GoodsDeliveryRequest> __Game_Simulation_GoodsDeliveryRequest_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      public BufferLookup<TripNeeded> __Game_Citizens_TripNeeded_RW_BufferLookup;
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> __Game_Companies_StorageCompany_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_GoodsDeliveryRequest_RO_ComponentTypeHandle = state.GetComponentTypeHandle<GoodsDeliveryRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TripNeeded_RW_BufferLookup = state.GetBufferLookup<TripNeeded>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageCompany_RO_ComponentLookup = state.GetComponentLookup<Game.Companies.StorageCompany>(true);
      }
    }
  }
}
