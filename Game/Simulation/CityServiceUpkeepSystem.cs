// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CityServiceUpkeepSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CityServiceUpkeepSystem : GameSystemBase
  {
    private static readonly int kUpdatesPerDay = 64;
    private CitySystem m_CitySystem;
    private SimulationSystem m_SimulationSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private ResourceSystem m_ResourceSystem;
    private IconCommandSystem m_IconCommandSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_UpkeepGroup;
    private EntityQuery m_BudgetDataQuery;
    private CityServiceUpkeepSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (CityServiceUpkeepSystem.kUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpkeepGroup = this.GetEntityQuery(ComponentType.ReadOnly<CityServiceUpkeep>(), ComponentType.ReadWrite<Game.Economy.Resources>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_BudgetDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<ServiceBudgetData>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, CityServiceUpkeepSystem.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_PlayerMoney_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceBudgetData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ServiceUsage_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceConsumerData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UpkeepModifierData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ResourceConsumer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_GuestVehicle_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle deps;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CityServiceUpkeepSystem.CityServiceUpkeepJob jobData = new CityServiceUpkeepSystem.CityServiceUpkeepJob()
      {
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_GuestVehicleType = this.__TypeHandle.__Game_Vehicles_GuestVehicle_RO_BufferTypeHandle,
        m_ResourcesType = this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle,
        m_ResourceConsumerType = this.__TypeHandle.__Game_Buildings_ResourceConsumer_RW_ComponentTypeHandle,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ServiceObjects = this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_ServiceUpkeepDatas = this.__TypeHandle.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup,
        m_UpkeepModifiers = this.__TypeHandle.__Game_Prefabs_UpkeepModifierData_RO_BufferLookup,
        m_ResourceConsumerDatas = this.__TypeHandle.__Game_Prefabs_ResourceConsumerData_RO_ComponentLookup,
        m_ServiceUsages = this.__TypeHandle.__Game_Buildings_ServiceUsage_RO_ComponentLookup,
        m_Limits = this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup,
        m_ServiceBudgetDatas = this.__TypeHandle.__Game_Simulation_ServiceBudgetData_RO_BufferLookup,
        m_DeliveryTrucks = this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_QuantityData = this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_PlayerMoney = this.__TypeHandle.__Game_City_PlayerMoney_RW_ComponentLookup,
        m_UpdateFrameIndex = updateFrame,
        m_City = this.m_CitySystem.City,
        m_BudgetDataEntity = this.m_BudgetDataQuery.GetSingletonEntity(),
        m_RandomSeed = RandomSeed.Next(),
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps).AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<CityServiceUpkeepSystem.CityServiceUpkeepJob>(this.m_UpkeepGroup, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(this.Dependency);
    }

    public static byte GetResourceAvailability(
      NativeList<ServiceUpkeepData> upkeeps,
      DynamicBuffer<Game.Economy.Resources> resources,
      NativeArray<int> storageTargets)
    {
      byte resourceAvailability = byte.MaxValue;
      foreach (ServiceUpkeepData upkeep in upkeeps)
      {
        Resource resource = upkeep.m_Upkeep.m_Resource;
        int storageTarget = storageTargets[EconomyUtils.GetResourceIndex(resource)];
        if (storageTarget > 0)
        {
          byte num = (byte) math.clamp(math.ceil((float) byte.MaxValue * (float) EconomyUtils.GetResources(resource, resources) / (float) storageTarget), 0.0f, (float) byte.MaxValue);
          if ((int) num < (int) resourceAvailability)
            resourceAvailability = num;
        }
      }
      return resourceAvailability;
    }

    public static int CalculateUpkeep(
      int amount,
      Entity prefabEntity,
      Entity budgetEntity,
      EntityManager entityManager)
    {
      Entity service = Entity.Null;
      ServiceObjectData component;
      if (entityManager.TryGetComponent<ServiceObjectData>(prefabEntity, out component))
        service = component.m_Service;
      int num = 100;
      DynamicBuffer<ServiceBudgetData> buffer;
      if (entityManager.TryGetBuffer<ServiceBudgetData>(budgetEntity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          if (buffer[index].m_Service == service)
            num = buffer[index].m_Budget;
        }
      }
      return (int) math.round((float) amount * ((float) num / 100f));
    }

    public static void GetUpkeepModifierData(
      NativeList<UpkeepModifierData> upkeepModifierList,
      BufferLookup<InstalledUpgrade> installedUpgrades,
      ComponentLookup<PrefabRef> prefabs,
      BufferLookup<UpkeepModifierData> upkeepModifiers,
      Entity entity)
    {
      DynamicBuffer<InstalledUpgrade> bufferData;
      if (!installedUpgrades.TryGetBuffer(entity, out bufferData))
        return;
      UpgradeUtils.CombineStats<UpkeepModifierData>(upkeepModifierList, bufferData, ref prefabs, ref upkeepModifiers);
    }

    public static bool IsMaterialResource(
      ComponentLookup<ResourceData> resourceDatas,
      ResourcePrefabs resourcePrefabs,
      ResourceStack upkeep)
    {
      return (double) resourceDatas[resourcePrefabs[upkeep.m_Resource]].m_Weight > 0.0;
    }

    public static int GetUpkeepOfEmployeeWage(
      BufferLookup<Employee> employeeBufs,
      Entity entity,
      EconomyParameterData economyParameterData,
      bool mainBuildingDisabled)
    {
      if (mainBuildingDisabled)
        return 0;
      int upkeepOfEmployeeWage = 0;
      DynamicBuffer<Employee> bufferData;
      if (employeeBufs.TryGetBuffer(entity, out bufferData))
      {
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Employee employee = bufferData[index];
          upkeepOfEmployeeWage += economyParameterData.GetWage((int) employee.m_Level, true);
        }
      }
      return upkeepOfEmployeeWage;
    }

    public static void GetUpkeepWithUsageScale(
      NativeList<ServiceUpkeepData> totalUpkeepDatas,
      BufferLookup<ServiceUpkeepData> serviceUpkeepDatas,
      BufferLookup<InstalledUpgrade> installedUpgradeBufs,
      ComponentLookup<PrefabRef> prefabRefs,
      ComponentLookup<ServiceUsage> serviceUsages,
      Entity entity,
      Entity prefab,
      bool mainBuildingDisabled)
    {
      DynamicBuffer<ServiceUpkeepData> bufferData1;
      if (serviceUpkeepDatas.TryGetBuffer(prefab, out bufferData1))
      {
        foreach (ServiceUpkeepData serviceUpkeepData in bufferData1)
        {
          ServiceUsage componentData;
          if (serviceUpkeepData.m_ScaleWithUsage && serviceUsages.TryGetComponent(entity, out componentData))
            totalUpkeepDatas.Add(serviceUpkeepData.ApplyServiceUsage(componentData.m_Usage));
          else
            totalUpkeepDatas.Add(in serviceUpkeepData);
        }
      }
      DynamicBuffer<InstalledUpgrade> bufferData2;
      if (!installedUpgradeBufs.TryGetBuffer(entity, out bufferData2))
        return;
      foreach (InstalledUpgrade installedUpgrade in bufferData2)
      {
        bool flag = BuildingUtils.CheckOption(installedUpgrade, BuildingOption.Inactive);
        PrefabRef componentData1;
        DynamicBuffer<ServiceUpkeepData> bufferData3;
        if (prefabRefs.TryGetComponent(installedUpgrade.m_Upgrade, out componentData1) && serviceUpkeepDatas.TryGetBuffer(componentData1.m_Prefab, out bufferData3))
        {
          for (int index = 0; index < bufferData3.Length; ++index)
          {
            ServiceUpkeepData combineData = bufferData3[index];
            if (combineData.m_Upkeep.m_Resource == Resource.Money)
            {
              if (!mainBuildingDisabled & flag)
                combineData.m_Upkeep.m_Amount = (combineData.m_Upkeep.m_Amount + 5) / 10;
            }
            else if (flag)
              continue;
            ServiceUsage componentData2;
            if (combineData.m_ScaleWithUsage && serviceUsages.TryGetComponent(installedUpgrade.m_Upgrade, out componentData2))
              UpgradeUtils.CombineStats<ServiceUpkeepData>(totalUpkeepDatas, combineData.ApplyServiceUsage(componentData2.m_Usage));
            else
              UpgradeUtils.CombineStats<ServiceUpkeepData>(totalUpkeepDatas, combineData);
          }
        }
      }
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
    public CityServiceUpkeepSystem()
    {
    }

    [BurstCompile]
    private struct CityServiceUpkeepJob : IJobChunk
    {
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public BufferTypeHandle<GuestVehicle> m_GuestVehicleType;
      public BufferTypeHandle<Game.Economy.Resources> m_ResourcesType;
      public ComponentTypeHandle<Game.Buildings.ResourceConsumer> m_ResourceConsumerType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> m_ServiceObjects;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public BufferLookup<ServiceUpkeepData> m_ServiceUpkeepDatas;
      [ReadOnly]
      public BufferLookup<UpkeepModifierData> m_UpkeepModifiers;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public ComponentLookup<ResourceConsumerData> m_ResourceConsumerDatas;
      [ReadOnly]
      public ComponentLookup<ServiceUsage> m_ServiceUsages;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> m_Limits;
      [ReadOnly]
      public BufferLookup<ServiceBudgetData> m_ServiceBudgetDatas;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> m_DeliveryTrucks;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [ReadOnly]
      public ComponentLookup<Quantity> m_QuantityData;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      public ComponentLookup<PlayerMoney> m_PlayerMoney;
      public uint m_UpdateFrameIndex;
      public Entity m_City;
      public Entity m_BudgetDataEntity;
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      public EntityCommandBuffer m_CommandBuffer;
      public IconCommandBuffer m_IconCommandBuffer;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;

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
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ServiceBudgetData> serviceBudgetData = this.m_ServiceBudgetDatas[this.m_BudgetDataEntity];
        NativeList<ServiceUpkeepData> nativeList1 = new NativeList<ServiceUpkeepData>(4, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<UpkeepModifierData> nativeList2 = new NativeList<UpkeepModifierData>(4, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeParallelHashMap<Entity, bool> notifications = new NativeParallelHashMap<Entity, bool>(4, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeArray<int> nativeArray1 = new NativeArray<int>(EconomyUtils.ResourceCount, Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<GuestVehicle> bufferAccessor1 = chunk.GetBufferAccessor<GuestVehicle>(ref this.m_GuestVehicleType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Economy.Resources> bufferAccessor2 = chunk.GetBufferAccessor<Game.Economy.Resources>(ref this.m_ResourcesType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.ResourceConsumer> nativeArray4 = chunk.GetNativeArray<Game.Buildings.ResourceConsumer>(ref this.m_ResourceConsumerType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          nativeList1.Clear();
          nativeList2.Clear();
          nativeArray1.Fill<int>(0);
          Entity entity = nativeArray2[index1];
          Entity prefab = nativeArray3[index1].m_Prefab;
          DynamicBuffer<Game.Economy.Resources> resources1 = bufferAccessor2[index1];
          // ISSUE: reference to a compiler-generated method
          int serviceBudget = this.GetServiceBudget(prefab, serviceBudgetData);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          CityServiceUpkeepSystem.GetUpkeepWithUsageScale(nativeList1, this.m_ServiceUpkeepDatas, this.m_InstalledUpgrades, this.m_Prefabs, this.m_ServiceUsages, entity, prefab, false);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          CityServiceUpkeepSystem.GetUpkeepModifierData(nativeList2, this.m_InstalledUpgrades, this.m_Prefabs, this.m_UpkeepModifiers, entity);
          // ISSUE: reference to a compiler-generated field
          Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(entity.Index);
          random.NextBool();
          // ISSUE: reference to a compiler-generated method
          this.GetStorageTargets(nativeArray1, nativeList2, entity, prefab);
          StorageLimitData componentData1;
          // ISSUE: reference to a compiler-generated field
          this.m_Limits.TryGetComponent(prefab, out componentData1);
          DynamicBuffer<InstalledUpgrade> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_InstalledUpgrades.TryGetBuffer(entity, out bufferData1))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UpgradeUtils.CombineStats<StorageLimitData>(ref componentData1, bufferData1, ref this.m_Prefabs, ref this.m_Limits);
          }
          // ISSUE: reference to a compiler-generated method
          bool flag = this.TickConsumer(serviceBudget, componentData1.m_Limit, nativeList1, nativeList2, resources1, ref random);
          int num1 = 0;
          foreach (int num2 in nativeArray1)
            num1 += num2;
          if (num1 > 0)
          {
            notifications.Clear();
            if (nativeArray4.Length != 0)
            {
              ref Game.Buildings.ResourceConsumer local = ref nativeArray4.ElementAt<Game.Buildings.ResourceConsumer>(index1);
              bool wasEmpty = local.m_ResourceAvailability == (byte) 0;
              // ISSUE: reference to a compiler-generated method
              local.m_ResourceAvailability = CityServiceUpkeepSystem.GetResourceAvailability(nativeList1, resources1, nativeArray1);
              bool isEmpty = local.m_ResourceAvailability == (byte) 0;
              // ISSUE: reference to a compiler-generated method
              this.UpdateNotification(notifications, prefab, wasEmpty, isEmpty);
            }
            foreach (KeyValue<Entity, bool> keyValue in notifications)
            {
              if (keyValue.Value)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Add(entity, keyValue.Key, IconPriority.Problem);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Remove(entity, keyValue.Key);
              }
            }
            int num3;
            if (componentData1.m_Limit > 0)
            {
              num3 = componentData1.m_Limit;
              float num4 = (float) componentData1.m_Limit / (float) num1;
              for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
                nativeArray1[index2] = Mathf.RoundToInt(num4 * (float) nativeArray1[index2]);
            }
            else
              num3 = num1;
            int y = num3 - -EconomyUtils.GetTotalStorageUsed(resources1);
            if (bufferAccessor1.Length != 0)
            {
              DynamicBuffer<GuestVehicle> dynamicBuffer = bufferAccessor1[index1];
              for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
              {
                Entity vehicle = dynamicBuffer[index3].m_Vehicle;
                Game.Vehicles.DeliveryTruck componentData2;
                // ISSUE: reference to a compiler-generated field
                if (this.m_DeliveryTrucks.TryGetComponent(vehicle, out componentData2) && (componentData2.m_State & DeliveryTruckFlags.DummyTraffic) == (DeliveryTruckFlags) 0)
                {
                  DynamicBuffer<LayoutElement> bufferData2;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_LayoutElements.TryGetBuffer(vehicle, out bufferData2) && bufferData2.Length != 0)
                  {
                    for (int index4 = 0; index4 < bufferData2.Length; ++index4)
                    {
                      Game.Vehicles.DeliveryTruck componentData3;
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_DeliveryTrucks.TryGetComponent(bufferData2[index4].m_Vehicle, out componentData3) && componentData3.m_Resource != Resource.NoResource)
                      {
                        y -= componentData3.m_Amount;
                        nativeArray1[EconomyUtils.GetResourceIndex(componentData3.m_Resource)] -= componentData3.m_Amount;
                      }
                    }
                  }
                  else if (componentData2.m_Resource != Resource.NoResource)
                  {
                    y -= componentData2.m_Amount;
                    nativeArray1[EconomyUtils.GetResourceIndex(componentData2.m_Resource)] -= componentData2.m_Amount;
                  }
                }
              }
            }
            for (int index5 = 0; index5 < nativeArray1.Length; ++index5)
            {
              int num5 = nativeArray1[index5];
              if (num5 > 0)
              {
                Resource resource = EconomyUtils.GetResource(index5);
                int resources2 = EconomyUtils.GetResources(resource, resources1);
                int x = num5 - resources2;
                if (y > 0 && x > 0 && random.NextInt(math.max(1, num5 * 3 / 4)) > resources2 - num5 / 4)
                {
                  int num6 = math.min(x, y);
                  y -= num6;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<GoodsDeliveryRequest>(this.m_CommandBuffer.CreateEntity(), new GoodsDeliveryRequest()
                  {
                    m_Amount = num6,
                    m_Flags = GoodsDeliveryFlags.CommercialAllowed | GoodsDeliveryFlags.IndustrialAllowed | GoodsDeliveryFlags.ImportAllowed,
                    m_Resource = resource,
                    m_Target = entity
                  });
                }
              }
            }
          }
          EconomyUtils.GetResources(Resource.Money, resources1);
          if (flag)
          {
            // ISSUE: reference to a compiler-generated method
            this.QuantityUpdated(entity);
          }
        }
      }

      private void GetStorageTargets(
        NativeArray<int> storageTargets,
        NativeList<UpkeepModifierData> upkeepModifiers,
        Entity entity,
        Entity prefab)
      {
        DynamicBuffer<ServiceUpkeepData> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceUpkeepDatas.TryGetBuffer(prefab, out bufferData1))
        {
          foreach (ServiceUpkeepData serviceUpkeepData in bufferData1)
          {
            // ISSUE: reference to a compiler-generated method
            float x = CityServiceUpkeepSystem.CityServiceUpkeepJob.GetUpkeepModifier(serviceUpkeepData.m_Upkeep.m_Resource, upkeepModifiers).Transform((float) serviceUpkeepData.m_Upkeep.m_Amount);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (CityServiceUpkeepSystem.IsMaterialResource(this.m_ResourceDatas, this.m_ResourcePrefabs, serviceUpkeepData.m_Upkeep))
              storageTargets[EconomyUtils.GetResourceIndex(serviceUpkeepData.m_Upkeep.m_Resource)] += (int) math.round(x);
          }
        }
        DynamicBuffer<InstalledUpgrade> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_InstalledUpgrades.TryGetBuffer(entity, out bufferData2))
          return;
        foreach (InstalledUpgrade installedUpgrade in bufferData2)
        {
          PrefabRef componentData;
          DynamicBuffer<ServiceUpkeepData> bufferData3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!BuildingUtils.CheckOption(installedUpgrade, BuildingOption.Inactive) && this.m_Prefabs.TryGetComponent(installedUpgrade.m_Upgrade, out componentData) && this.m_ServiceUpkeepDatas.TryGetBuffer(componentData.m_Prefab, out bufferData3))
          {
            foreach (ServiceUpkeepData serviceUpkeepData in bufferData3)
            {
              // ISSUE: reference to a compiler-generated method
              float x = CityServiceUpkeepSystem.CityServiceUpkeepJob.GetUpkeepModifier(serviceUpkeepData.m_Upkeep.m_Resource, upkeepModifiers).Transform((float) serviceUpkeepData.m_Upkeep.m_Amount);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (CityServiceUpkeepSystem.IsMaterialResource(this.m_ResourceDatas, this.m_ResourcePrefabs, serviceUpkeepData.m_Upkeep))
                storageTargets[EconomyUtils.GetResourceIndex(serviceUpkeepData.m_Upkeep.m_Resource)] += (int) math.round(x);
            }
          }
        }
      }

      private int GetServiceBudget(Entity prefab, DynamicBuffer<ServiceBudgetData> serviceBudgets)
      {
        ServiceObjectData componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceObjects.TryGetComponent(prefab, out componentData))
        {
          for (int index = 0; index < serviceBudgets.Length; ++index)
          {
            if (serviceBudgets[index].m_Service == componentData.m_Service)
              return serviceBudgets[index].m_Budget;
          }
        }
        return 100;
      }

      private bool TickConsumer(
        int serviceBudget,
        int storageLimit,
        NativeList<ServiceUpkeepData> serviceUpkeepDatas,
        NativeList<UpkeepModifierData> upkeepModifiers,
        DynamicBuffer<Game.Economy.Resources> resources,
        ref Unity.Mathematics.Random random)
      {
        bool flag1 = false;
        foreach (ServiceUpkeepData serviceUpkeepData in serviceUpkeepDatas)
        {
          Resource resource = serviceUpkeepData.m_Upkeep.m_Resource;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          bool flag2 = CityServiceUpkeepSystem.IsMaterialResource(this.m_ResourceDatas, this.m_ResourcePrefabs, serviceUpkeepData.m_Upkeep);
          if (serviceUpkeepData.m_Upkeep.m_Amount > 0)
          {
            // ISSUE: reference to a compiler-generated method
            float num1 = CityServiceUpkeepSystem.CityServiceUpkeepJob.GetUpkeepModifier(resource, upkeepModifiers).Transform((float) serviceUpkeepData.m_Upkeep.m_Amount);
            // ISSUE: reference to a compiler-generated field
            int intRandom = MathUtils.RoundToIntRandom(ref random, num1 / (float) CityServiceUpkeepSystem.kUpdatesPerDay);
            if (intRandom > 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              PlayerMoney playerMoney = this.m_PlayerMoney[this.m_City];
              if (flag2)
              {
                int num2 = EconomyUtils.AddResources(resource, -intRandom, resources);
                int num3 = Mathf.RoundToInt((float) ((double) (num2 + intRandom) / (double) math.max(1, storageLimit) * 100.0));
                int num4 = Mathf.RoundToInt((float) ((double) num2 / (double) math.max(1, storageLimit) * 100.0));
                int4 int4 = new int4(0, 33, 50, 66);
                flag1 |= math.any(num3 > int4 != num4 > int4);
              }
              else if (resource != Resource.Money)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                int num5 = Mathf.RoundToInt(EconomyUtils.GetMarketPrice(serviceUpkeepData.m_Upkeep.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas) * (float) intRandom);
                if (num5 != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
                  {
                    m_Statistic = StatisticType.Expense,
                    m_Change = (float) num5,
                    m_Parameter = 5
                  });
                }
              }
            }
          }
        }
        return flag1;
      }

      private void QuantityUpdated(Entity buildingEntity, bool updateAll = false)
      {
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
            this.m_CommandBuffer.AddComponent<BatchesUpdated>(subObject, new BatchesUpdated());
            updateAll1 = true;
          }
          // ISSUE: reference to a compiler-generated method
          this.QuantityUpdated(subObject, updateAll1);
        }
      }

      private static UpkeepModifierData GetUpkeepModifier(
        Resource resource,
        NativeList<UpkeepModifierData> upkeepModifiers)
      {
        foreach (UpkeepModifierData upkeepModifier in upkeepModifiers)
        {
          if (upkeepModifier.m_Resource == resource)
            return upkeepModifier;
        }
        return new UpkeepModifierData()
        {
          m_Resource = resource,
          m_Multiplier = 1f
        };
      }

      private void UpdateNotification(
        NativeParallelHashMap<Entity, bool> notifications,
        Entity prefab,
        bool wasEmpty,
        bool isEmpty)
      {
        ResourceConsumerData componentData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ResourceConsumerDatas.TryGetComponent(prefab, out componentData) || !(componentData.m_NoResourceNotificationPrefab != Entity.Null) || wasEmpty == isEmpty || isEmpty && notifications.ContainsKey(componentData.m_NoResourceNotificationPrefab))
          return;
        notifications[componentData.m_NoResourceNotificationPrefab] = isEmpty;
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
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<GuestVehicle> __Game_Vehicles_GuestVehicle_RO_BufferTypeHandle;
      public BufferTypeHandle<Game.Economy.Resources> __Game_Economy_Resources_RW_BufferTypeHandle;
      public ComponentTypeHandle<Game.Buildings.ResourceConsumer> __Game_Buildings_ResourceConsumer_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> __Game_Prefabs_ServiceObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ServiceUpkeepData> __Game_Prefabs_ServiceUpkeepData_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<UpkeepModifierData> __Game_Prefabs_UpkeepModifierData_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ResourceConsumerData> __Game_Prefabs_ResourceConsumerData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceUsage> __Game_Buildings_ServiceUsage_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> __Game_Companies_StorageLimitData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ServiceBudgetData> __Game_Simulation_ServiceBudgetData_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> __Game_Vehicles_DeliveryTruck_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Quantity> __Game_Objects_Quantity_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      public ComponentLookup<PlayerMoney> __Game_City_PlayerMoney_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_GuestVehicle_RO_BufferTypeHandle = state.GetBufferTypeHandle<GuestVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferTypeHandle = state.GetBufferTypeHandle<Game.Economy.Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ResourceConsumer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.ResourceConsumer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup = state.GetComponentLookup<ServiceObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup = state.GetBufferLookup<ServiceUpkeepData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UpkeepModifierData_RO_BufferLookup = state.GetBufferLookup<UpkeepModifierData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceConsumerData_RO_ComponentLookup = state.GetComponentLookup<ResourceConsumerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ServiceUsage_RO_ComponentLookup = state.GetComponentLookup<ServiceUsage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageLimitData_RO_ComponentLookup = state.GetComponentLookup<StorageLimitData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceBudgetData_RO_BufferLookup = state.GetBufferLookup<ServiceBudgetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.DeliveryTruck>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Quantity_RO_ComponentLookup = state.GetComponentLookup<Quantity>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_PlayerMoney_RW_ComponentLookup = state.GetComponentLookup<PlayerMoney>();
      }
    }
  }
}
