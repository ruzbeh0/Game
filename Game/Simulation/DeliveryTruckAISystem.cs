// Decompiled with JetBrains decompiler
// Type: Game.Simulation.DeliveryTruckAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using Game.Vehicles;
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
  public class DeliveryTruckAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private DeliveryTruckAISystem.Actions m_Actions;
    private EntityQuery m_DeliveryTruckQuery;
    private DeliveryTruckAISystem.TypeHandle __TypeHandle;

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
      this.m_Actions = this.World.GetOrCreateSystemManaged<DeliveryTruckAISystem.Actions>();
      // ISSUE: reference to a compiler-generated field
      this.m_DeliveryTruckQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Vehicles.DeliveryTruck>(), ComponentType.ReadOnly<CarCurrentLane>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>(), ComponentType.Exclude<OutOfControl>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint index = this.m_SimulationSystem.frameIndex % 16U;
      // ISSUE: reference to a compiler-generated field
      this.m_DeliveryTruckQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_DeliveryTruckQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(index));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Actions.m_DeliveredQueue = new NativeQueue<DeliveryTruckAISystem.DeliveredStack>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ReturnLoad_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new DeliveryTruckAISystem.DeliveryTruckTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_LayoutElementType = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_QuantityData = this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PropertyData = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_DeliveryTruckData = this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RW_ComponentLookup,
        m_ReturnLoadData = this.__TypeHandle.__Game_Vehicles_ReturnLoad_RW_ComponentLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_DeliveredQueue = this.m_Actions.m_DeliveredQueue.AsParallelWriter()
      }.ScheduleParallel<DeliveryTruckAISystem.DeliveryTruckTickJob>(this.m_DeliveryTruckQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Actions.m_Dependency = jobHandle;
      this.Dependency = jobHandle;
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
    public DeliveryTruckAISystem()
    {
    }

    public struct DeliveredStack
    {
      public Entity vehicle;
      public Entity target;
      public Resource resource;
      public int amount;
      public Entity costPayer;
      public float distance;
      public bool storageTransfer;
      public bool moneyRefund;
    }

    [BurstCompile]
    private struct DeliverJob : IJob
    {
      public NativeQueue<DeliveryTruckAISystem.DeliveredStack> m_DeliveredQueue;
      public BufferLookup<Game.Economy.Resources> m_Resources;
      public ComponentLookup<Game.Vehicles.DeliveryTruck> m_DeliveryTrucks;
      public ComponentLookup<BuyingCompany> m_BuyingCompanies;
      public ComponentLookup<Game.Companies.StorageCompany> m_StorageCompanies;
      [ReadOnly]
      public BufferLookup<Renter> m_Renters;
      [ReadOnly]
      public ComponentLookup<CompanyData> m_Companies;
      [ReadOnly]
      public ComponentLookup<Owner> m_Owners;
      [ReadOnly]
      public ComponentLookup<Controller> m_Controllers;
      public BufferLookup<StorageTransferRequest> m_StorageTransferRequests;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      public EconomyParameterData m_EconomyParameters;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;

      private Entity FindCompany(DynamicBuffer<Renter> renters)
      {
        for (int index = 0; index < renters.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_Companies.HasComponent(renters[index].m_Renter))
            return renters[index].m_Renter;
        }
        return Entity.Null;
      }

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        DeliveryTruckAISystem.DeliveredStack deliveredStack;
        // ISSUE: reference to a compiler-generated field
        while (this.m_DeliveredQueue.TryDequeue(out deliveredStack))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!(this.m_ResourcePrefabs[deliveredStack.resource] == Entity.Null))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int amount = Mathf.RoundToInt((float) deliveredStack.amount * EconomyUtils.GetMarketPrice(deliveredStack.resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float weight = EconomyUtils.GetWeight(deliveredStack.resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int num1 = Mathf.RoundToInt((float) EconomyUtils.GetTransportCost(deliveredStack.distance, deliveredStack.resource, deliveredStack.amount, weight));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Resources.HasBuffer(deliveredStack.target) && this.m_Renters.HasBuffer(deliveredStack.target))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              deliveredStack.target = this.FindCompany(this.m_Renters[deliveredStack.target]);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Resources.HasBuffer(deliveredStack.costPayer) && this.m_Renters.HasBuffer(deliveredStack.costPayer))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              deliveredStack.costPayer = this.FindCompany(this.m_Renters[deliveredStack.costPayer]);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Resources.HasBuffer(deliveredStack.target))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (deliveredStack.moneyRefund && this.m_Resources.HasBuffer(deliveredStack.costPayer) && !this.m_StorageCompanies.HasComponent(deliveredStack.costPayer))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                EconomyUtils.AddResources(Resource.Money, amount, this.m_Resources[deliveredStack.costPayer]);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Game.Economy.Resources> resource = this.m_Resources[deliveredStack.target];
              // ISSUE: reference to a compiler-generated field
              if (deliveredStack.amount < 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                int num2 = math.min(-deliveredStack.amount, EconomyUtils.GetResources(deliveredStack.resource, resource));
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Game.Vehicles.DeliveryTruck deliveryTruck = this.m_DeliveryTrucks[deliveredStack.vehicle];
                deliveryTruck.m_Amount += num2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_DeliveryTrucks[deliveredStack.vehicle] = deliveryTruck;
                // ISSUE: reference to a compiler-generated field
                EconomyUtils.AddResources(deliveredStack.resource, -num2, resource);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (!deliveredStack.moneyRefund)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  EconomyUtils.AddResources(deliveredStack.resource, deliveredStack.amount, resource);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
                  {
                    m_Statistic = StatisticType.CargoCountTruck,
                    m_Change = (float) deliveredStack.amount
                  });
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Resources.HasBuffer(deliveredStack.costPayer))
                {
                  // ISSUE: reference to a compiler-generated field
                  if (deliveredStack.moneyRefund)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (!this.m_StorageCompanies.HasComponent(deliveredStack.costPayer))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      EconomyUtils.AddResources(Resource.Money, amount, this.m_Resources[deliveredStack.costPayer]);
                    }
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (!this.m_StorageCompanies.HasComponent(deliveredStack.target))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      EconomyUtils.AddResources(Resource.Money, -amount, this.m_Resources[deliveredStack.target]);
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (deliveredStack.storageTransfer)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (!this.m_StorageCompanies.HasComponent(deliveredStack.costPayer))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        EconomyUtils.AddResources(Resource.Money, -num1, this.m_Resources[deliveredStack.costPayer]);
                      }
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_BuyingCompanies.HasComponent(deliveredStack.costPayer))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        BuyingCompany buyingCompany = this.m_BuyingCompanies[deliveredStack.costPayer];
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        buyingCompany.m_MeanInputTripLength = (double) buyingCompany.m_MeanInputTripLength <= 0.0 ? deliveredStack.distance : math.lerp(buyingCompany.m_MeanInputTripLength, deliveredStack.distance, 0.5f);
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        this.m_BuyingCompanies[deliveredStack.costPayer] = buyingCompany;
                      }
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (!this.m_StorageCompanies.HasComponent(deliveredStack.costPayer))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        EconomyUtils.AddResources(Resource.Money, amount, this.m_Resources[deliveredStack.costPayer]);
                      }
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (!this.m_StorageCompanies.HasComponent(deliveredStack.target))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        EconomyUtils.AddResources(Resource.Money, -amount, this.m_Resources[deliveredStack.target]);
                      }
                    }
                  }
                }
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
                if (deliveredStack.amount > 0 && this.m_StorageCompanies.HasComponent(deliveredStack.target) && this.m_StorageTransferRequests.HasBuffer(deliveredStack.target) && (this.m_Owners.HasComponent(deliveredStack.vehicle) || this.m_Controllers.HasComponent(deliveredStack.vehicle) && this.m_Owners.HasComponent(this.m_Controllers[deliveredStack.vehicle].m_Controller)))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Entity owner = this.m_Owners[this.m_Controllers.HasComponent(deliveredStack.vehicle) ? this.m_Controllers[deliveredStack.vehicle].m_Controller : deliveredStack.vehicle].m_Owner;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<StorageTransferRequest> storageTransferRequest1 = this.m_StorageTransferRequests[deliveredStack.target];
                  for (int index = 0; index < storageTransferRequest1.Length; ++index)
                  {
                    StorageTransferRequest storageTransferRequest2 = storageTransferRequest1[index];
                    // ISSUE: reference to a compiler-generated field
                    if ((storageTransferRequest2.m_Flags & StorageTransferFlags.Incoming) != (StorageTransferFlags) 0 && storageTransferRequest2.m_Target == owner && storageTransferRequest2.m_Resource == deliveredStack.resource)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (storageTransferRequest2.m_Amount > deliveredStack.amount)
                      {
                        // ISSUE: reference to a compiler-generated field
                        storageTransferRequest2.m_Amount -= deliveredStack.amount;
                        storageTransferRequest1[index] = storageTransferRequest2;
                        break;
                      }
                      // ISSUE: reference to a compiler-generated field
                      deliveredStack.amount -= storageTransferRequest2.m_Amount;
                      storageTransferRequest1.RemoveAtSwapBack(index);
                      --index;
                    }
                  }
                }
              }
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct DeliveryTruckTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> m_LayoutElementType;
      public ComponentTypeHandle<CarCurrentLane> m_CurrentLaneType;
      public ComponentTypeHandle<Game.Common.Target> m_TargetType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Quantity> m_QuantityData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyData;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> m_DeliveryTruckData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<ReturnLoad> m_ReturnLoadData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<PathElement> m_PathElements;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public NativeQueue<DeliveryTruckAISystem.DeliveredStack>.ParallelWriter m_DeliveredQueue;

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
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarCurrentLane> nativeArray4 = chunk.GetNativeArray<CarCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Common.Target> nativeArray5 = chunk.GetNativeArray<Game.Common.Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray6 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathInformation> nativeArray7 = chunk.GetNativeArray<PathInformation>(ref this.m_PathInformationType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LayoutElement> bufferAccessor = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutElementType);
        // ISSUE: reference to a compiler-generated field
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          PrefabRef prefabRef = nativeArray3[index];
          PathInformation pathInformation = nativeArray7[index];
          ref CarCurrentLane local1 = ref nativeArray4.ElementAt<CarCurrentLane>(index);
          ref PathOwner local2 = ref nativeArray6.ElementAt<PathOwner>(index);
          ref Game.Common.Target local3 = ref nativeArray5.ElementAt<Game.Common.Target>(index);
          // ISSUE: reference to a compiler-generated field
          ref Game.Vehicles.DeliveryTruck local4 = ref this.m_DeliveryTruckData.GetRefRW(entity).ValueRW;
          Owner owner;
          CollectionUtils.TryGet<Owner>(nativeArray2, index, out owner);
          DynamicBuffer<LayoutElement> layout;
          CollectionUtils.TryGet<LayoutElement>(bufferAccessor, index, out layout);
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.CheckUnspawned(unfilteredChunkIndex, entity, local1, isUnspawned, this.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, owner, prefabRef, pathInformation, layout, ref local4, ref local1, ref local2, ref local3);
        }
      }

      private void CancelTransaction(
        int jobIndex,
        Entity vehicleEntity,
        ref Game.Vehicles.DeliveryTruck deliveryTruck,
        DynamicBuffer<LayoutElement> layout,
        PathInformation pathInformation,
        Owner owner)
      {
        if ((deliveryTruck.m_State & DeliveryTruckFlags.TransactionCancelled) != (DeliveryTruckFlags) 0)
          return;
        if (layout.IsCreated && layout.Length != 0)
        {
          for (int index = 0; index < layout.Length; ++index)
          {
            Entity vehicle = layout[index].m_Vehicle;
            // ISSUE: reference to a compiler-generated field
            if (vehicle != vehicleEntity && this.m_DeliveryTruckData.HasComponent(vehicle))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Vehicles.DeliveryTruck deliveryTruck1 = this.m_DeliveryTruckData[vehicle];
              if ((deliveryTruck1.m_State & DeliveryTruckFlags.Loaded) != (DeliveryTruckFlags) 0 && deliveryTruck1.m_Amount > 0)
              {
                if ((deliveryTruck.m_State & DeliveryTruckFlags.Returning) != (DeliveryTruckFlags) 0)
                {
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  DeliveryTruckAISystem.DeliveredStack deliveredStack = new DeliveryTruckAISystem.DeliveredStack();
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack.vehicle = vehicle;
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack.target = owner.m_Owner;
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack.resource = deliveryTruck1.m_Resource;
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack.amount = deliveryTruck1.m_Amount;
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack.costPayer = owner.m_Owner;
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack.distance = pathInformation.m_Distance;
                  // ISSUE: reference to a compiler-generated field
                  this.m_DeliveredQueue.Enqueue(deliveredStack);
                  // ISSUE: reference to a compiler-generated field
                  if (deliveredStack.amount != 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.TargetQuantityUpdated(jobIndex, deliveredStack.target);
                  }
                }
                else
                {
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  DeliveryTruckAISystem.DeliveredStack deliveredStack = new DeliveryTruckAISystem.DeliveredStack();
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack.vehicle = vehicle;
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack.target = pathInformation.m_Origin;
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack.resource = deliveryTruck1.m_Resource;
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack.amount = deliveryTruck1.m_Amount;
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack.costPayer = pathInformation.m_Destination;
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack.distance = 0.0f;
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack.storageTransfer = (deliveryTruck.m_State & DeliveryTruckFlags.StorageTransfer) > (DeliveryTruckFlags) 0;
                  // ISSUE: reference to a compiler-generated field
                  this.m_DeliveredQueue.Enqueue(deliveredStack);
                  // ISSUE: reference to a compiler-generated field
                  if (deliveredStack.amount != 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.TargetQuantityUpdated(jobIndex, deliveredStack.target);
                  }
                }
              }
              else if ((deliveryTruck1.m_State & DeliveryTruckFlags.Loaded) == (DeliveryTruckFlags) 0 && (deliveryTruck.m_State & DeliveryTruckFlags.Returning) == (DeliveryTruckFlags) 0)
              {
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                DeliveryTruckAISystem.DeliveredStack deliveredStack = new DeliveryTruckAISystem.DeliveredStack();
                // ISSUE: reference to a compiler-generated field
                deliveredStack.vehicle = vehicle;
                // ISSUE: reference to a compiler-generated field
                deliveredStack.target = pathInformation.m_Destination;
                // ISSUE: reference to a compiler-generated field
                deliveredStack.resource = deliveryTruck1.m_Resource;
                // ISSUE: reference to a compiler-generated field
                deliveredStack.amount = deliveryTruck1.m_Amount;
                // ISSUE: reference to a compiler-generated field
                deliveredStack.distance = 0.0f;
                // ISSUE: reference to a compiler-generated field
                deliveredStack.costPayer = owner.m_Owner;
                // ISSUE: reference to a compiler-generated field
                deliveredStack.moneyRefund = true;
                // ISSUE: reference to a compiler-generated field
                this.m_DeliveredQueue.Enqueue(deliveredStack);
                // ISSUE: reference to a compiler-generated field
                if (deliveredStack.amount < 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.TargetQuantityUpdated(jobIndex, deliveredStack.target);
                }
              }
            }
          }
        }
        else if ((deliveryTruck.m_State & DeliveryTruckFlags.Loaded) != (DeliveryTruckFlags) 0 && deliveryTruck.m_Amount > 0)
        {
          if ((deliveryTruck.m_State & DeliveryTruckFlags.Returning) != (DeliveryTruckFlags) 0)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            DeliveryTruckAISystem.DeliveredStack deliveredStack = new DeliveryTruckAISystem.DeliveredStack();
            // ISSUE: reference to a compiler-generated field
            deliveredStack.vehicle = vehicleEntity;
            // ISSUE: reference to a compiler-generated field
            deliveredStack.target = owner.m_Owner;
            // ISSUE: reference to a compiler-generated field
            deliveredStack.resource = deliveryTruck.m_Resource;
            // ISSUE: reference to a compiler-generated field
            deliveredStack.amount = deliveryTruck.m_Amount;
            // ISSUE: reference to a compiler-generated field
            deliveredStack.costPayer = owner.m_Owner;
            // ISSUE: reference to a compiler-generated field
            deliveredStack.distance = pathInformation.m_Distance;
            // ISSUE: reference to a compiler-generated field
            this.m_DeliveredQueue.Enqueue(deliveredStack);
            // ISSUE: reference to a compiler-generated field
            if (deliveredStack.amount == 0)
              return;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.TargetQuantityUpdated(jobIndex, deliveredStack.target);
          }
          else
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            DeliveryTruckAISystem.DeliveredStack deliveredStack = new DeliveryTruckAISystem.DeliveredStack();
            // ISSUE: reference to a compiler-generated field
            deliveredStack.vehicle = vehicleEntity;
            // ISSUE: reference to a compiler-generated field
            deliveredStack.target = pathInformation.m_Origin;
            // ISSUE: reference to a compiler-generated field
            deliveredStack.resource = deliveryTruck.m_Resource;
            // ISSUE: reference to a compiler-generated field
            deliveredStack.amount = deliveryTruck.m_Amount;
            // ISSUE: reference to a compiler-generated field
            deliveredStack.costPayer = pathInformation.m_Destination;
            // ISSUE: reference to a compiler-generated field
            deliveredStack.storageTransfer = (deliveryTruck.m_State & DeliveryTruckFlags.StorageTransfer) > (DeliveryTruckFlags) 0;
            // ISSUE: reference to a compiler-generated field
            this.m_DeliveredQueue.Enqueue(deliveredStack);
            // ISSUE: reference to a compiler-generated field
            if (deliveredStack.amount == 0)
              return;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.TargetQuantityUpdated(jobIndex, deliveredStack.target);
          }
        }
        else
        {
          if ((deliveryTruck.m_State & DeliveryTruckFlags.Loaded) != (DeliveryTruckFlags) 0 || (deliveryTruck.m_State & DeliveryTruckFlags.Returning) != (DeliveryTruckFlags) 0)
            return;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          DeliveryTruckAISystem.DeliveredStack deliveredStack = new DeliveryTruckAISystem.DeliveredStack();
          // ISSUE: reference to a compiler-generated field
          deliveredStack.vehicle = vehicleEntity;
          // ISSUE: reference to a compiler-generated field
          deliveredStack.target = pathInformation.m_Destination;
          // ISSUE: reference to a compiler-generated field
          deliveredStack.resource = deliveryTruck.m_Resource;
          // ISSUE: reference to a compiler-generated field
          deliveredStack.amount = deliveryTruck.m_Amount;
          // ISSUE: reference to a compiler-generated field
          deliveredStack.distance = 0.0f;
          // ISSUE: reference to a compiler-generated field
          deliveredStack.costPayer = owner.m_Owner;
          // ISSUE: reference to a compiler-generated field
          deliveredStack.moneyRefund = true;
          // ISSUE: reference to a compiler-generated field
          this.m_DeliveredQueue.Enqueue(deliveredStack);
          // ISSUE: reference to a compiler-generated field
          if (deliveredStack.amount >= 0)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.TargetQuantityUpdated(jobIndex, deliveredStack.target);
        }
      }

      private void Tick(
        int jobIndex,
        Entity vehicleEntity,
        Owner owner,
        PrefabRef prefabRef,
        PathInformation pathInformation,
        DynamicBuffer<LayoutElement> layout,
        ref Game.Vehicles.DeliveryTruck deliveryTruck,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
      {
        if (VehicleUtils.ResetUpdatedPath(ref pathOwner))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<PathElement> pathElement = this.m_PathElements[vehicleEntity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PathUtils.ResetPath(ref currentLane, pathElement, this.m_SlaveLaneData, this.m_OwnerData, this.m_SubLanes);
          if ((deliveryTruck.m_State & DeliveryTruckFlags.UpdateOwnerQuantity) != (DeliveryTruckFlags) 0)
          {
            deliveryTruck.m_State &= ~DeliveryTruckFlags.UpdateOwnerQuantity;
            // ISSUE: reference to a compiler-generated method
            this.TargetQuantityUpdated(jobIndex, owner.m_Owner);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabRefData.HasComponent(target.m_Target) || VehicleUtils.PathfindFailed(pathOwner))
        {
          if ((deliveryTruck.m_State & DeliveryTruckFlags.DummyTraffic) != (DeliveryTruckFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, vehicleEntity, layout);
            return;
          }
          if (VehicleUtils.IsStuck(pathOwner) || (deliveryTruck.m_State & DeliveryTruckFlags.Returning) != (DeliveryTruckFlags) 0)
          {
            if (VehicleUtils.PathfindFailed(pathOwner) || VehicleUtils.IsStuck(pathOwner))
            {
              // ISSUE: reference to a compiler-generated method
              this.CancelTransaction(jobIndex, vehicleEntity, ref deliveryTruck, layout, pathInformation, owner);
              deliveryTruck.m_State |= DeliveryTruckFlags.TransactionCancelled;
            }
            // ISSUE: reference to a compiler-generated field
            VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, vehicleEntity, layout);
            return;
          }
          if (VehicleUtils.PathfindFailed(pathOwner) || VehicleUtils.IsStuck(pathOwner))
          {
            // ISSUE: reference to a compiler-generated method
            this.CancelTransaction(jobIndex, vehicleEntity, ref deliveryTruck, layout, pathInformation, owner);
            deliveryTruck.m_State |= DeliveryTruckFlags.TransactionCancelled;
          }
          deliveryTruck.m_State |= DeliveryTruckFlags.Returning;
          VehicleUtils.SetTarget(ref pathOwner, ref target, owner.m_Owner);
        }
        else if (VehicleUtils.PathEndReached(currentLane))
        {
          if ((deliveryTruck.m_State & DeliveryTruckFlags.DummyTraffic) != (DeliveryTruckFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, vehicleEntity, layout);
            return;
          }
          // ISSUE: reference to a compiler-generated method
          this.DeliverCargo(jobIndex, vehicleEntity, owner.m_Owner, pathInformation, layout, ref deliveryTruck);
          // ISSUE: reference to a compiler-generated field
          if ((deliveryTruck.m_State & DeliveryTruckFlags.Returning) != (DeliveryTruckFlags) 0 || !this.m_PrefabRefData.HasComponent(owner.m_Owner))
          {
            // ISSUE: reference to a compiler-generated field
            VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, vehicleEntity, layout);
            return;
          }
          deliveryTruck.m_State |= DeliveryTruckFlags.Returning;
          VehicleUtils.SetTarget(ref pathOwner, ref target, owner.m_Owner);
        }
        // ISSUE: reference to a compiler-generated method
        this.FindPathIfNeeded(vehicleEntity, prefabRef, ref currentLane, ref pathOwner, ref target);
      }

      private void DeliverCargo(
        int jobIndex,
        Entity truck,
        Entity truckOwner,
        PathInformation pathInformation,
        DynamicBuffer<LayoutElement> layout,
        ref Game.Vehicles.DeliveryTruck truckDelivery)
      {
        bool resourceChanged1 = false;
        bool flag = false;
        // ISSUE: variable of a compiler-generated type
        DeliveryTruckAISystem.DeliveredStack deliveredStack1;
        if (layout.IsCreated && layout.Length != 0)
        {
          for (int index = 0; index < layout.Length; ++index)
          {
            Entity vehicle = layout[index].m_Vehicle;
            // ISSUE: reference to a compiler-generated field
            if (vehicle != truck && this.m_DeliveryTruckData.HasComponent(vehicle))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Vehicles.DeliveryTruck deliveryTruck = this.m_DeliveryTruckData[vehicle];
              if ((deliveryTruck.m_State & DeliveryTruckFlags.Loaded) != (DeliveryTruckFlags) 0 && deliveryTruck.m_Amount > 0 && (truckDelivery.m_State & DeliveryTruckFlags.NoUnloading) == (DeliveryTruckFlags) 0)
              {
                // ISSUE: object of a compiler-generated type is created
                deliveredStack1 = new DeliveryTruckAISystem.DeliveredStack();
                // ISSUE: reference to a compiler-generated field
                deliveredStack1.vehicle = vehicle;
                // ISSUE: reference to a compiler-generated field
                deliveredStack1.target = pathInformation.m_Destination;
                // ISSUE: reference to a compiler-generated field
                deliveredStack1.resource = deliveryTruck.m_Resource;
                // ISSUE: reference to a compiler-generated field
                deliveredStack1.amount = deliveryTruck.m_Amount;
                // ISSUE: reference to a compiler-generated field
                deliveredStack1.costPayer = truckOwner;
                // ISSUE: reference to a compiler-generated field
                deliveredStack1.distance = pathInformation.m_Distance;
                // ISSUE: variable of a compiler-generated type
                DeliveryTruckAISystem.DeliveredStack deliveredStack2 = deliveredStack1;
                // ISSUE: reference to a compiler-generated field
                this.m_DeliveredQueue.Enqueue(deliveredStack2);
                // ISSUE: reference to a compiler-generated field
                flag |= deliveredStack2.amount != 0;
              }
              deliveryTruck.m_State ^= DeliveryTruckFlags.Loaded;
              // ISSUE: reference to a compiler-generated field
              if ((deliveryTruck.m_State & DeliveryTruckFlags.Loaded) == (DeliveryTruckFlags) 0 && (truckDelivery.m_State & DeliveryTruckFlags.Returning) == (DeliveryTruckFlags) 0 && this.m_ReturnLoadData.HasComponent(vehicle))
              {
                // ISSUE: reference to a compiler-generated field
                ReturnLoad returnLoad = this.m_ReturnLoadData[vehicle];
                if (returnLoad.m_Amount > 0)
                {
                  // ISSUE: object of a compiler-generated type is created
                  deliveredStack1 = new DeliveryTruckAISystem.DeliveredStack();
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack1.vehicle = vehicle;
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack1.target = pathInformation.m_Destination;
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack1.resource = returnLoad.m_Resource;
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack1.amount = -returnLoad.m_Amount;
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack1.costPayer = truckOwner;
                  // ISSUE: reference to a compiler-generated field
                  deliveredStack1.distance = pathInformation.m_Distance;
                  // ISSUE: variable of a compiler-generated type
                  DeliveryTruckAISystem.DeliveredStack deliveredStack3 = deliveredStack1;
                  // ISSUE: reference to a compiler-generated field
                  this.m_DeliveredQueue.Enqueue(deliveredStack3);
                  // ISSUE: reference to a compiler-generated field
                  flag |= deliveredStack3.amount != 0;
                  deliveryTruck.m_State |= DeliveryTruckFlags.Loaded;
                  deliveryTruck.m_Resource = returnLoad.m_Resource;
                  deliveryTruck.m_Amount = 0;
                  resourceChanged1 = true;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_ReturnLoadData[vehicle] = new ReturnLoad();
              }
              // ISSUE: reference to a compiler-generated field
              this.m_DeliveryTruckData[vehicle] = deliveryTruck;
              // ISSUE: reference to a compiler-generated method
              this.QuantityUpdated(jobIndex, vehicle, resourceChanged1);
            }
          }
        }
        if ((truckDelivery.m_State & DeliveryTruckFlags.Loaded) != (DeliveryTruckFlags) 0 && truckDelivery.m_Amount > 0)
        {
          // ISSUE: object of a compiler-generated type is created
          deliveredStack1 = new DeliveryTruckAISystem.DeliveredStack();
          // ISSUE: reference to a compiler-generated field
          deliveredStack1.vehicle = truck;
          // ISSUE: reference to a compiler-generated field
          deliveredStack1.target = pathInformation.m_Destination;
          // ISSUE: reference to a compiler-generated field
          deliveredStack1.resource = truckDelivery.m_Resource;
          // ISSUE: reference to a compiler-generated field
          deliveredStack1.amount = truckDelivery.m_Amount;
          // ISSUE: reference to a compiler-generated field
          deliveredStack1.costPayer = truckOwner;
          // ISSUE: reference to a compiler-generated field
          deliveredStack1.distance = pathInformation.m_Distance;
          // ISSUE: variable of a compiler-generated type
          DeliveryTruckAISystem.DeliveredStack deliveredStack4 = deliveredStack1;
          // ISSUE: reference to a compiler-generated field
          this.m_DeliveredQueue.Enqueue(deliveredStack4);
          // ISSUE: reference to a compiler-generated field
          flag |= deliveredStack4.amount != 0;
        }
        truckDelivery.m_State ^= DeliveryTruckFlags.Loaded;
        bool resourceChanged2 = false;
        // ISSUE: reference to a compiler-generated field
        if ((truckDelivery.m_State & (DeliveryTruckFlags.Returning | DeliveryTruckFlags.Loaded)) == (DeliveryTruckFlags) 0 && this.m_ReturnLoadData.HasComponent(truck))
        {
          // ISSUE: reference to a compiler-generated field
          ReturnLoad returnLoad = this.m_ReturnLoadData[truck];
          if (returnLoad.m_Amount > 0)
          {
            // ISSUE: object of a compiler-generated type is created
            deliveredStack1 = new DeliveryTruckAISystem.DeliveredStack();
            // ISSUE: reference to a compiler-generated field
            deliveredStack1.vehicle = truck;
            // ISSUE: reference to a compiler-generated field
            deliveredStack1.target = pathInformation.m_Destination;
            // ISSUE: reference to a compiler-generated field
            deliveredStack1.resource = returnLoad.m_Resource;
            // ISSUE: reference to a compiler-generated field
            deliveredStack1.amount = -returnLoad.m_Amount;
            // ISSUE: reference to a compiler-generated field
            deliveredStack1.costPayer = truckOwner;
            // ISSUE: reference to a compiler-generated field
            deliveredStack1.distance = pathInformation.m_Distance;
            // ISSUE: variable of a compiler-generated type
            DeliveryTruckAISystem.DeliveredStack deliveredStack5 = deliveredStack1;
            // ISSUE: reference to a compiler-generated field
            this.m_DeliveredQueue.Enqueue(deliveredStack5);
            // ISSUE: reference to a compiler-generated field
            flag |= deliveredStack5.amount != 0;
            truckDelivery.m_State |= DeliveryTruckFlags.Loaded;
            truckDelivery.m_Resource = returnLoad.m_Resource;
            truckDelivery.m_Amount = 0;
            resourceChanged2 = true;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_ReturnLoadData[truck] = new ReturnLoad();
        }
        if (resourceChanged2)
          truckDelivery.m_State |= DeliveryTruckFlags.Buying;
        // ISSUE: reference to a compiler-generated method
        this.QuantityUpdated(jobIndex, truck, resourceChanged2);
        if (!flag)
          return;
        // ISSUE: reference to a compiler-generated method
        this.TargetQuantityUpdated(jobIndex, pathInformation.m_Destination);
      }

      private void QuantityUpdated(int jobIndex, Entity vehicleEntity, bool resourceChanged)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.HasBuffer(vehicleEntity))
          return;
        if (resourceChanged)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(jobIndex, vehicleEntity, new Updated());
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Objects.SubObject> subObject1 = this.m_SubObjects[vehicleEntity];
          for (int index = 0; index < subObject1.Length; ++index)
          {
            Entity subObject2 = subObject1[index].m_SubObject;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, subObject2, new BatchesUpdated());
          }
        }
      }

      private void TargetQuantityUpdated(int jobIndex, Entity buildingEntity, bool updateAll = false)
      {
        PropertyRenter componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PropertyData.TryGetComponent(buildingEntity, out componentData))
          buildingEntity = componentData.m_Property;
        DynamicBuffer<Game.Objects.SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(buildingEntity, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity subObject = bufferData[index].m_SubObject;
          bool updateAll1 = false;
          // ISSUE: reference to a compiler-generated field
          if (updateAll || this.m_QuantityData.HasComponent(subObject))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, subObject, new BatchesUpdated());
            updateAll1 = true;
          }
          // ISSUE: reference to a compiler-generated method
          this.TargetQuantityUpdated(jobIndex, subObject, updateAll1);
        }
      }

      private void FindPathIfNeeded(
        Entity vehicleEntity,
        PrefabRef prefabRef,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
      {
        if (!VehicleUtils.RequireNewPath(pathOwner))
          return;
        // ISSUE: reference to a compiler-generated field
        CarData carData = this.m_PrefabCarData[prefabRef.m_Prefab];
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) carData.m_MaxSpeed,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
          m_Methods = PathMethod.Road | PathMethod.CargoLoading,
          m_IgnoredRules = VehicleUtils.GetIgnoredPathfindRules(carData)
        };
        SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
        setupQueueTarget.m_Methods = PathMethod.Road | PathMethod.CargoLoading;
        setupQueueTarget.m_RoadTypes = RoadTypes.Car;
        setupQueueTarget.m_RandomCost = 30f;
        SetupQueueTarget origin = setupQueueTarget;
        setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
        setupQueueTarget.m_Methods = PathMethod.Road | PathMethod.CargoLoading;
        setupQueueTarget.m_RoadTypes = RoadTypes.Car;
        setupQueueTarget.m_Entity = target.m_Target;
        setupQueueTarget.m_RandomCost = 30f;
        SetupQueueTarget destination = setupQueueTarget;
        SetupQueueItem setupQueueItem = new SetupQueueItem(vehicleEntity, parameters, origin, destination);
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.SetupPathfind(ref currentLane, ref pathOwner, this.m_PathfindQueue, setupQueueItem);
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
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> __Game_Objects_Unspawned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferTypeHandle;
      public ComponentTypeHandle<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Common.Target> __Game_Common_Target_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Quantity> __Game_Objects_Quantity_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      public ComponentLookup<Game.Vehicles.DeliveryTruck> __Game_Vehicles_DeliveryTruck_RW_ComponentLookup;
      public ComponentLookup<ReturnLoad> __Game_Vehicles_ReturnLoad_RW_ComponentLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Common.Target>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Quantity_RO_ComponentLookup = state.GetComponentLookup<Quantity>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_DeliveryTruck_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.DeliveryTruck>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ReturnLoad_RW_ComponentLookup = state.GetComponentLookup<ReturnLoad>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
      }
    }
  }
}
