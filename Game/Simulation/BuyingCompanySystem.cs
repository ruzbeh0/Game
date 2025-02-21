// Decompiled with JetBrains decompiler
// Type: Game.Simulation.BuyingCompanySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Notifications;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class BuyingCompanySystem : GameSystemBase
  {
    private static readonly float kNotificationCostLimit = 5f;
    private static readonly int kResourceLowStockAmount = 4000;
    private static readonly int kResourceMinimumRequestAmount = 2000;
    private SimulationSystem m_SimulationSystem;
    private ResourceSystem m_ResourceSystem;
    private VehicleCapacitySystem m_VehicleCapacitySystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private IconCommandSystem m_IconCommandSystem;
    private EntityQuery m_CompanyNotificationParameterQuery;
    private EntityQuery m_CompanyGroup;
    private BuyingCompanySystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleCapacitySystem = this.World.GetOrCreateSystemManaged<VehicleCapacitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyGroup = this.GetEntityQuery(ComponentType.ReadOnly<BuyingCompany>(), ComponentType.ReadOnly<Resources>(), ComponentType.ReadWrite<OwnedVehicle>(), ComponentType.ReadOnly<PropertyRenter>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<TradeCost>(), ComponentType.ReadWrite<CompanyNotifications>(), ComponentType.ReadWrite<TripNeeded>(), ComponentType.Exclude<ResourceBuyer>(), ComponentType.Exclude<Deleted>(), ComponentType.ReadOnly<UpdateFrame>());
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyNotificationParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<CompanyNotificationParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CompanyGroup);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CompanyNotificationParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint frameWithInterval = SimulationUtils.GetUpdateFrameWithInterval(this.m_SimulationSystem.frameIndex, (uint) this.GetUpdateInterval(SystemUpdatePhase.GameSimulation), 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CompanyNotifications_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_TradeCost_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TripNeeded_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BuyingCompanySystem.CompanyBuyJob jobData = new BuyingCompanySystem.CompanyBuyJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ResourceType = this.__TypeHandle.__Game_Economy_Resources_RO_BufferTypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_VehicleType = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle,
        m_TripType = this.__TypeHandle.__Game_Citizens_TripNeeded_RO_BufferTypeHandle,
        m_TradeCostType = this.__TypeHandle.__Game_Companies_TradeCost_RO_BufferTypeHandle,
        m_PropertyRenterType = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
        m_CompanyNotificationsType = this.__TypeHandle.__Game_Companies_CompanyNotifications_RW_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_IndustrialProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_StorageLimits = this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup,
        m_Trucks = this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup,
        m_Transforms = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_Layouts = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_CompanyNotificationParameters = this.m_CompanyNotificationParameterQuery.GetSingleton<CompanyNotificationParameterData>(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_DeliveryTruckSelectData = this.m_VehicleCapacitySystem.GetDeliveryTruckSelectData(),
        m_UpdateFrameIndex = frameWithInterval,
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_RandomSeed = RandomSeed.Next()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<BuyingCompanySystem.CompanyBuyJob>(this.m_CompanyGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(this.Dependency);
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
    public BuyingCompanySystem()
    {
    }

    [BurstCompile]
    private struct CompanyBuyJob : IJobChunk
    {
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<OwnedVehicle> m_VehicleType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public BufferTypeHandle<Resources> m_ResourceType;
      [ReadOnly]
      public BufferTypeHandle<TripNeeded> m_TripType;
      [ReadOnly]
      public BufferTypeHandle<TradeCost> m_TradeCostType;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_PropertyRenterType;
      public ComponentTypeHandle<CompanyNotifications> m_CompanyNotificationsType;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> m_StorageLimits;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> m_Trucks;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public ComponentLookup<Transform> m_Transforms;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_Layouts;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public DeliveryTruckSelectData m_DeliveryTruckSelectData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public IconCommandBuffer m_IconCommandBuffer;
      public CompanyNotificationParameterData m_CompanyNotificationParameters;
      public RandomSeed m_RandomSeed;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((int) chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index != (int) this.m_UpdateFrameIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<OwnedVehicle> bufferAccessor1 = chunk.GetBufferAccessor<OwnedVehicle>(ref this.m_VehicleType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TripNeeded> bufferAccessor2 = chunk.GetBufferAccessor<TripNeeded>(ref this.m_TripType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Resources> bufferAccessor3 = chunk.GetBufferAccessor<Resources>(ref this.m_ResourceType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TradeCost> bufferAccessor4 = chunk.GetBufferAccessor<TradeCost>(ref this.m_TradeCostType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CompanyNotifications> nativeArray3 = chunk.GetNativeArray<CompanyNotifications>(ref this.m_CompanyNotificationsType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PropertyRenter> nativeArray4 = chunk.GetNativeArray<PropertyRenter>(ref this.m_PropertyRenterType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity entity = nativeArray1[index1];
          CompanyNotifications companyNotifications = nativeArray3[index1];
          DynamicBuffer<OwnedVehicle> dynamicBuffer1 = bufferAccessor1[index1];
          DynamicBuffer<TradeCost> costs = bufferAccessor4[index1];
          int num1 = int.MaxValue;
          Entity prefab = nativeArray2[index1].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          if (this.m_StorageLimits.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            num1 = this.m_StorageLimits[prefab].m_Limit;
          }
          // ISSUE: reference to a compiler-generated field
          IndustrialProcessData industrialProcessData = this.m_IndustrialProcessDatas[prefab];
          Entity owner = entity;
          if (nativeArray4.Length > 0)
            owner = nativeArray4[index1].m_Property;
          Resource resource = Resource.NoResource;
          ResourceIterator iterator = ResourceIterator.GetIterator();
          int num2 = 0;
          int y = num1;
          bool flag1 = false;
          bool flag2 = industrialProcessData.m_Input2.m_Resource > Resource.NoResource;
          while (iterator.Next())
          {
            if (iterator.resource == industrialProcessData.m_Input1.m_Resource || iterator.resource == industrialProcessData.m_Input2.m_Resource || iterator.resource == industrialProcessData.m_Output.m_Resource)
            {
              bool flag3 = iterator.resource == industrialProcessData.m_Input1.m_Resource || iterator.resource == industrialProcessData.m_Input2.m_Resource;
              // ISSUE: reference to a compiler-generated field
              if ((double) EconomyUtils.GetTradeCost(iterator.resource, costs).m_BuyCost > (double) BuyingCompanySystem.kNotificationCostLimit)
                flag1 = true;
              int resources = EconomyUtils.GetResources(iterator.resource, bufferAccessor3[index1]);
              for (int index2 = 0; index2 < dynamicBuffer1.Length; ++index2)
              {
                Entity vehicle1 = dynamicBuffer1[index2].m_Vehicle;
                // ISSUE: reference to a compiler-generated field
                if (this.m_Trucks.HasComponent(vehicle1))
                {
                  // ISSUE: reference to a compiler-generated field
                  Game.Vehicles.DeliveryTruck truck1 = this.m_Trucks[vehicle1];
                  DynamicBuffer<LayoutElement> dynamicBuffer2 = new DynamicBuffer<LayoutElement>();
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_Layouts.HasBuffer(vehicle1))
                  {
                    // ISSUE: reference to a compiler-generated field
                    dynamicBuffer2 = this.m_Layouts[vehicle1];
                  }
                  // ISSUE: reference to a compiler-generated field
                  if (dynamicBuffer2.IsCreated && this.m_Layouts[vehicle1].Length != 0)
                  {
                    for (int index3 = 0; index3 < dynamicBuffer2.Length; ++index3)
                    {
                      Entity vehicle2 = dynamicBuffer2[index3].m_Vehicle;
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_Trucks.HasComponent(vehicle2))
                      {
                        // ISSUE: reference to a compiler-generated field
                        Game.Vehicles.DeliveryTruck truck2 = this.m_Trucks[vehicle2];
                        if (truck2.m_Resource == iterator.resource && (truck1.m_State & DeliveryTruckFlags.Buying) != (DeliveryTruckFlags) 0)
                          resources += truck2.m_Amount;
                      }
                    }
                  }
                  else if (truck1.m_Resource == iterator.resource && (truck1.m_State & DeliveryTruckFlags.Buying) != (DeliveryTruckFlags) 0)
                    resources += truck1.m_Amount;
                }
              }
              DynamicBuffer<TripNeeded> dynamicBuffer3 = bufferAccessor2[index1];
              for (int index4 = 0; index4 < dynamicBuffer3.Length; ++index4)
              {
                TripNeeded tripNeeded = dynamicBuffer3[index4];
                if (tripNeeded.m_Purpose == Game.Citizens.Purpose.Shopping && tripNeeded.m_Resource == iterator.resource)
                  resources += tripNeeded.m_Data;
              }
              // ISSUE: reference to a compiler-generated field
              if (((resource != Resource.NoResource ? 0 : (resources < BuyingCompanySystem.kResourceLowStockAmount ? 1 : 0)) & (flag3 ? 1 : 0)) != 0)
              {
                resource = iterator.resource;
                num2 = resources;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (EconomyUtils.IsMaterial(iterator.resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas))
                y -= resources;
            }
          }
          if (companyNotifications.m_NoInputEntity == new Entity() & flag1)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Add(owner, this.m_CompanyNotificationParameters.m_NoInputsNotificationPrefab, IconPriority.Problem);
            companyNotifications.m_NoInputEntity = owner;
            nativeArray3[index1] = companyNotifications;
          }
          else if (companyNotifications.m_NoInputEntity != new Entity())
          {
            if (!flag1)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Remove(companyNotifications.m_NoInputEntity, this.m_CompanyNotificationParameters.m_NoInputsNotificationPrefab);
              companyNotifications.m_NoInputEntity = Entity.Null;
              nativeArray3[index1] = companyNotifications;
            }
            else if (owner != companyNotifications.m_NoInputEntity)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Remove(companyNotifications.m_NoInputEntity, this.m_CompanyNotificationParameters.m_NoInputsNotificationPrefab);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Add(owner, this.m_CompanyNotificationParameters.m_NoInputsNotificationPrefab, IconPriority.Problem);
              companyNotifications.m_NoInputEntity = owner;
              nativeArray3[index1] = companyNotifications;
            }
          }
          if (resource != Resource.NoResource)
          {
            // ISSUE: reference to a compiler-generated field
            int num3 = BuyingCompanySystem.kResourceMinimumRequestAmount;
            DeliveryTruckSelectItem deliveryTruckSelectItem = new DeliveryTruckSelectItem();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((double) this.m_ResourceDatas[this.m_ResourcePrefabs[resource]].m_Weight > 0.0)
            {
              num3 = math.min((flag2 ? num1 / 3 : num1 / 2) - num2, y);
              // ISSUE: reference to a compiler-generated field
              if (num3 > BuyingCompanySystem.kResourceMinimumRequestAmount)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_DeliveryTruckSelectData.TrySelectItem(ref random, resource, num3, out deliveryTruckSelectItem);
              }
              else
                continue;
            }
            else
              deliveryTruckSelectItem.m_Capacity = num3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PropertyRenters.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              Entity property = this.m_PropertyRenters[entity].m_Property;
              // ISSUE: reference to a compiler-generated field
              if (this.m_Transforms.HasComponent(property))
              {
                // ISSUE: reference to a compiler-generated field
                ResourceBuyer component = new ResourceBuyer()
                {
                  m_Payer = entity,
                  m_AmountNeeded = math.min(num3, deliveryTruckSelectItem.m_Capacity),
                  m_Flags = SetupTargetFlags.Industrial | SetupTargetFlags.Import,
                  m_Location = this.m_Transforms[property].m_Position,
                  m_ResourceNeeded = resource
                };
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<ResourceBuyer>(unfilteredChunkIndex, entity, component);
              }
            }
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
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Resources> __Game_Economy_Resources_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<TripNeeded> __Game_Citizens_TripNeeded_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<TradeCost> __Game_Companies_TradeCost_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentTypeHandle;
      public ComponentTypeHandle<CompanyNotifications> __Game_Companies_CompanyNotifications_RW_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> __Game_Companies_StorageLimitData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> __Game_Vehicles_DeliveryTruck_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferTypeHandle = state.GetBufferTypeHandle<Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle = state.GetBufferTypeHandle<OwnedVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TripNeeded_RO_BufferTypeHandle = state.GetBufferTypeHandle<TripNeeded>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_TradeCost_RO_BufferTypeHandle = state.GetBufferTypeHandle<TradeCost>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CompanyNotifications_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CompanyNotifications>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageLimitData_RO_ComponentLookup = state.GetComponentLookup<StorageLimitData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.DeliveryTruck>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
      }
    }
  }
}
