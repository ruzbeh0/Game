// Decompiled with JetBrains decompiler
// Type: Game.Objects.QuantityUpdateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Creatures;
using Game.Economy;
using Game.Prefabs;
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
namespace Game.Objects
{
  [CompilerGenerated]
  public class QuantityUpdateSystem : GameSystemBase
  {
    private EntityQuery m_QuantityQuery;
    private EntityQuery m_PostConfigurationQuery;
    private EntityQuery m_GarbageConfigurationQuery;
    private QuantityUpdateSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_QuantityQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadWrite<Quantity>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<BatchesUpdated>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_PostConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<PostConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<GarbageParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_QuantityQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DeliveryTruckData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityServiceUpkeep_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_IndustrialProperty_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageFacility_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WorkVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Quantity_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      this.Dependency = new QuantityUpdateSystem.UpdateQuantityJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_QuantityType = this.__TypeHandle.__Game_Objects_Quantity_RW_ComponentTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_VehicleData = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup,
        m_DeliveryTruckData = this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup,
        m_WorkVehicleData = this.__TypeHandle.__Game_Vehicles_WorkVehicle_RO_ComponentLookup,
        m_CreatureData = this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup,
        m_MailProducerData = this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup,
        m_GarbageProducerData = this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup,
        m_GarbageFacilityData = this.__TypeHandle.__Game_Buildings_GarbageFacility_RO_ComponentLookup,
        m_IndustrialPropertyData = this.__TypeHandle.__Game_Buildings_IndustrialProperty_RO_ComponentLookup,
        m_CityServiceUpkeepData = this.__TypeHandle.__Game_City_CityServiceUpkeep_RO_ComponentLookup,
        m_StorageLimitData = this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabQuantityObjectData = this.__TypeHandle.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup,
        m_PrefabDeliveryTruckData = this.__TypeHandle.__Game_Prefabs_DeliveryTruckData_RO_ComponentLookup,
        m_PrefabWorkVehicleData = this.__TypeHandle.__Game_Prefabs_WorkVehicleData_RO_ComponentLookup,
        m_PrefabCargoTransportVehicleData = this.__TypeHandle.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup,
        m_PrefabStorageCompanyData = this.__TypeHandle.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup,
        m_PrefabSpawnableBuildingData = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabGarbageFacilityData = this.__TypeHandle.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup,
        m_EconomyResources = this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup,
        m_Renters = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_PrefabServiceUpkeepDatas = this.__TypeHandle.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup,
        m_PostConfigurationData = (this.m_PostConfigurationQuery.IsEmptyIgnoreFilter ? new PostConfigurationData() : this.m_PostConfigurationQuery.GetSingleton<PostConfigurationData>()),
        m_GarbageConfigurationData = (this.m_GarbageConfigurationQuery.IsEmptyIgnoreFilter ? new GarbageParameterData() : this.m_GarbageConfigurationQuery.GetSingleton<GarbageParameterData>())
      }.ScheduleParallel<QuantityUpdateSystem.UpdateQuantityJob>(this.m_QuantityQuery, this.Dependency);
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
    public QuantityUpdateSystem()
    {
    }

    [BurstCompile]
    private struct UpdateQuantityJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Quantity> m_QuantityType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> m_DeliveryTruckData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.WorkVehicle> m_WorkVehicleData;
      [ReadOnly]
      public ComponentLookup<Creature> m_CreatureData;
      [ReadOnly]
      public ComponentLookup<MailProducer> m_MailProducerData;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> m_GarbageProducerData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.GarbageFacility> m_GarbageFacilityData;
      [ReadOnly]
      public ComponentLookup<IndustrialProperty> m_IndustrialPropertyData;
      [ReadOnly]
      public ComponentLookup<CityServiceUpkeep> m_CityServiceUpkeepData;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> m_StorageLimitData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<QuantityObjectData> m_PrefabQuantityObjectData;
      [ReadOnly]
      public ComponentLookup<DeliveryTruckData> m_PrefabDeliveryTruckData;
      [ReadOnly]
      public ComponentLookup<WorkVehicleData> m_PrefabWorkVehicleData;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> m_PrefabCargoTransportVehicleData;
      [ReadOnly]
      public ComponentLookup<StorageCompanyData> m_PrefabStorageCompanyData;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_PrefabSpawnableBuildingData;
      [ReadOnly]
      public ComponentLookup<BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<GarbageFacilityData> m_PrefabGarbageFacilityData;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> m_EconomyResources;
      [ReadOnly]
      public BufferLookup<Renter> m_Renters;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public BufferLookup<ServiceUpkeepData> m_PrefabServiceUpkeepDatas;
      [ReadOnly]
      public PostConfigurationData m_PostConfigurationData;
      [ReadOnly]
      public GarbageParameterData m_GarbageConfigurationData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Quantity> nativeArray2 = chunk.GetNativeArray<Quantity>(ref this.m_QuantityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray3 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          Entity entity1 = nativeArray1[index1];
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          Entity entity2 = nativeArray3.Length == 0 || !(nativeArray3[index1].m_Original != Entity.Null) ? this.GetOwner(entity1) : this.GetOwner(nativeArray3[index1].m_Original);
          // ISSUE: reference to a compiler-generated field
          QuantityObjectData quantityObjectData = this.m_PrefabQuantityObjectData[nativeArray4[index1].m_Prefab];
          Quantity quantity = new Quantity();
          PrefabRef componentData1;
          DynamicBuffer<Renter> bufferData1;
          SpawnableBuildingData componentData2;
          BuildingData componentData3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (quantityObjectData.m_Resources != Resource.NoResource && this.m_IndustrialPropertyData.HasComponent(entity2) && this.m_PrefabRefData.TryGetComponent(entity2, out componentData1) && this.m_Renters.TryGetBuffer(entity2, out bufferData1) && this.m_PrefabSpawnableBuildingData.TryGetComponent(componentData1.m_Prefab, out componentData2) && this.m_PrefabBuildingData.TryGetComponent(componentData1.m_Prefab, out componentData3))
          {
            int num1 = 0;
            int y = 0;
            bool flag = false;
            for (int index2 = 0; index2 < bufferData1.Length; ++index2)
            {
              Entity renter = bufferData1[index2].m_Renter;
              PrefabRef componentData4;
              DynamicBuffer<Game.Economy.Resources> bufferData2;
              StorageCompanyData componentData5;
              StorageLimitData componentData6;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefData.TryGetComponent(renter, out componentData4) && this.m_EconomyResources.TryGetBuffer(renter, out bufferData2) && this.m_PrefabStorageCompanyData.TryGetComponent(componentData4.m_Prefab, out componentData5) && this.m_StorageLimitData.TryGetComponent(componentData4.m_Prefab, out componentData6))
              {
                Resource resource = quantityObjectData.m_Resources & componentData5.m_StoredResources;
                if (resource != Resource.NoResource)
                {
                  y += componentData6.GetAdjustedLimit(componentData2, componentData3);
                  for (int index3 = 0; index3 < bufferData2.Length; ++index3)
                  {
                    Game.Economy.Resources resources = bufferData2[index3];
                    num1 += math.select(0, resources.m_Amount, (resources.m_Resource & resource) > Resource.NoResource);
                  }
                  flag = true;
                }
              }
            }
            if (flag)
            {
              float num2 = (float) num1 / (float) math.max(1, y);
              quantity.m_Fullness = (byte) math.clamp(Mathf.RoundToInt(num2 * 100f), 0, (int) byte.MaxValue);
              quantityObjectData.m_Resources = Resource.NoResource;
            }
          }
          PrefabRef componentData7;
          DynamicBuffer<Game.Economy.Resources> bufferData3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (quantityObjectData.m_Resources != Resource.NoResource && this.m_CityServiceUpkeepData.HasComponent(entity2) && this.m_PrefabRefData.TryGetComponent(entity2, out componentData7) && this.m_EconomyResources.TryGetBuffer(entity2, out bufferData3))
          {
            Resource resource1 = Resource.NoResource;
            DynamicBuffer<ServiceUpkeepData> bufferData4;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabServiceUpkeepDatas.TryGetBuffer(componentData7.m_Prefab, out bufferData4))
            {
              for (int index4 = 0; index4 < bufferData4.Length; ++index4)
                resource1 |= bufferData4[index4].m_Upkeep.m_Resource;
            }
            StorageCompanyData componentData8;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabStorageCompanyData.TryGetComponent(componentData7.m_Prefab, out componentData8))
              resource1 |= componentData8.m_StoredResources;
            // ISSUE: reference to a compiler-generated field
            if ((quantityObjectData.m_Resources & Resource.Garbage) != Resource.NoResource && this.m_GarbageFacilityData.HasComponent(entity2))
              resource1 |= Resource.Garbage;
            Resource resource2 = resource1 & quantityObjectData.m_Resources;
            if (resource2 != Resource.NoResource)
            {
              int num3 = 0;
              int y = 0;
              StorageLimitData componentData9;
              // ISSUE: reference to a compiler-generated field
              if (this.m_StorageLimitData.TryGetComponent(componentData7.m_Prefab, out componentData9))
              {
                DynamicBuffer<InstalledUpgrade> bufferData5;
                // ISSUE: reference to a compiler-generated field
                if (this.m_InstalledUpgrades.TryGetBuffer(entity2, out bufferData5))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  UpgradeUtils.CombineStats<StorageLimitData>(ref componentData9, bufferData5, ref this.m_PrefabRefData, ref this.m_StorageLimitData);
                }
                y += componentData9.m_Limit;
              }
              GarbageFacilityData componentData10;
              // ISSUE: reference to a compiler-generated field
              if ((quantityObjectData.m_Resources & Resource.Garbage) != Resource.NoResource && this.m_PrefabGarbageFacilityData.TryGetComponent(componentData7.m_Prefab, out componentData10))
                y += componentData10.m_GarbageCapacity;
              for (int index5 = 0; index5 < bufferData3.Length; ++index5)
              {
                Game.Economy.Resources resources = bufferData3[index5];
                num3 += math.select(0, resources.m_Amount, (resources.m_Resource & resource2) > Resource.NoResource);
              }
              float num4 = (float) num3 / (float) math.max(1, y);
              quantity.m_Fullness = (byte) math.clamp(Mathf.RoundToInt(num4 * 100f), 0, (int) byte.MaxValue);
              quantityObjectData.m_Resources = Resource.NoResource;
            }
          }
          Game.Vehicles.DeliveryTruck componentData11;
          // ISSUE: reference to a compiler-generated field
          if (quantityObjectData.m_Resources != Resource.NoResource && this.m_DeliveryTruckData.TryGetComponent(entity2, out componentData11) && (componentData11.m_State & DeliveryTruckFlags.Loaded) != (DeliveryTruckFlags) 0 && (componentData11.m_Resource & quantityObjectData.m_Resources) != Resource.NoResource)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DeliveryTruckData deliveryTruckData = this.m_PrefabDeliveryTruckData[this.m_PrefabRefData[entity2].m_Prefab];
            float num = (float) componentData11.m_Amount / (float) math.max(1, deliveryTruckData.m_CargoCapacity);
            quantity.m_Fullness = (byte) math.clamp(Mathf.RoundToInt(num * 100f), 0, (int) byte.MaxValue);
            quantityObjectData.m_Resources = Resource.NoResource;
          }
          MailProducer componentData12;
          // ISSUE: reference to a compiler-generated field
          if ((quantityObjectData.m_Resources & Resource.LocalMail) != Resource.NoResource && this.m_MailProducerData.TryGetComponent(entity2, out componentData12))
          {
            // ISSUE: reference to a compiler-generated field
            quantity.m_Fullness = (byte) math.select(100, 0, !componentData12.mailDelivered || componentData12.receivingMail >= this.m_PostConfigurationData.m_MailAccumulationTolerance);
            quantityObjectData.m_Resources = Resource.NoResource;
          }
          GarbageProducer componentData13;
          // ISSUE: reference to a compiler-generated field
          if ((quantityObjectData.m_Resources & Resource.Garbage) != Resource.NoResource && this.m_GarbageProducerData.TryGetComponent(entity2, out componentData13))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int num = math.select(math.select(0, 100, componentData13.m_Garbage >= this.m_GarbageConfigurationData.m_RequestGarbageLimit), (int) byte.MaxValue, componentData13.m_Garbage >= this.m_GarbageConfigurationData.m_WarningGarbageLimit);
            quantity.m_Fullness = (byte) num;
            quantityObjectData.m_Resources = Resource.NoResource;
          }
          // ISSUE: reference to a compiler-generated field
          if (quantityObjectData.m_Resources != Resource.NoResource && this.m_EconomyResources.HasBuffer(entity2))
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[entity2];
            Resource resources1 = quantityObjectData.m_Resources;
            int y = 0;
            CargoTransportVehicleData componentData14;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabCargoTransportVehicleData.TryGetComponent(prefabRef.m_Prefab, out componentData14))
            {
              resources1 &= componentData14.m_Resources;
              y = componentData14.m_CargoCapacity;
            }
            if (resources1 != Resource.NoResource)
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Game.Economy.Resources> economyResource = this.m_EconomyResources[entity2];
              int num5 = 0;
              for (int index6 = 0; index6 < economyResource.Length; ++index6)
              {
                Game.Economy.Resources resources2 = economyResource[index6];
                num5 += math.select(0, resources2.m_Amount, (resources2.m_Resource & resources1) > Resource.NoResource);
              }
              float num6 = (float) num5 / math.max(1f, (float) y);
              quantity.m_Fullness = (byte) math.clamp(Mathf.RoundToInt(num6 * 100f), 0, (int) byte.MaxValue);
              quantityObjectData.m_Resources = Resource.NoResource;
            }
          }
          Game.Vehicles.WorkVehicle componentData15;
          // ISSUE: reference to a compiler-generated field
          if (quantityObjectData.m_MapFeature != MapFeature.None && this.m_WorkVehicleData.TryGetComponent(entity2, out componentData15) && (double) componentData15.m_DoneAmount != 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            WorkVehicleData workVehicleData = this.m_PrefabWorkVehicleData[this.m_PrefabRefData[entity2].m_Prefab];
            if (workVehicleData.m_MapFeature == quantityObjectData.m_MapFeature)
            {
              float num = componentData15.m_DoneAmount / math.max(1f, workVehicleData.m_MaxWorkAmount);
              quantity.m_Fullness = (byte) math.clamp(Mathf.RoundToInt(num * 100f), 0, (int) byte.MaxValue);
              quantityObjectData.m_MapFeature = MapFeature.None;
            }
          }
          Game.Vehicles.WorkVehicle componentData16;
          // ISSUE: reference to a compiler-generated field
          if (quantityObjectData.m_Resources != Resource.NoResource && this.m_WorkVehicleData.TryGetComponent(entity2, out componentData16) && (double) componentData16.m_DoneAmount != 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            WorkVehicleData workVehicleData = this.m_PrefabWorkVehicleData[this.m_PrefabRefData[entity2].m_Prefab];
            if ((workVehicleData.m_Resources & quantityObjectData.m_Resources) != Resource.NoResource)
            {
              float num = componentData16.m_DoneAmount / math.max(1f, workVehicleData.m_MaxWorkAmount);
              quantity.m_Fullness = (byte) math.clamp(Mathf.RoundToInt(num * 100f), 0, (int) byte.MaxValue);
              quantityObjectData.m_Resources = Resource.NoResource;
            }
          }
          nativeArray2[index1] = quantity;
        }
      }

      public Entity GetOwner(Entity entity)
      {
        Entity entity1 = entity;
        Owner componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.TryGetComponent(entity1, out componentData) && !this.m_VehicleData.HasComponent(entity1) && !this.m_CreatureData.HasComponent(entity1))
          entity1 = componentData.m_Owner;
        return entity1;
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
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Quantity> __Game_Objects_Quantity_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> __Game_Vehicles_DeliveryTruck_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.WorkVehicle> __Game_Vehicles_WorkVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Creature> __Game_Creatures_Creature_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MailProducer> __Game_Buildings_MailProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> __Game_Buildings_GarbageProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.GarbageFacility> __Game_Buildings_GarbageFacility_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProperty> __Game_Buildings_IndustrialProperty_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CityServiceUpkeep> __Game_City_CityServiceUpkeep_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> __Game_Companies_StorageLimitData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<QuantityObjectData> __Game_Prefabs_QuantityObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<DeliveryTruckData> __Game_Prefabs_DeliveryTruckData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkVehicleData> __Game_Prefabs_WorkVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> __Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StorageCompanyData> __Game_Prefabs_StorageCompanyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageFacilityData> __Game_Prefabs_GarbageFacilityData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ServiceUpkeepData> __Game_Prefabs_ServiceUpkeepData_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Quantity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Quantity>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentLookup = state.GetComponentLookup<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.DeliveryTruck>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WorkVehicle_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.WorkVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Creature_RO_ComponentLookup = state.GetComponentLookup<Creature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_MailProducer_RO_ComponentLookup = state.GetComponentLookup<MailProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageProducer_RO_ComponentLookup = state.GetComponentLookup<GarbageProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageFacility_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.GarbageFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_IndustrialProperty_RO_ComponentLookup = state.GetComponentLookup<IndustrialProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityServiceUpkeep_RO_ComponentLookup = state.GetComponentLookup<CityServiceUpkeep>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageLimitData_RO_ComponentLookup = state.GetComponentLookup<StorageLimitData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup = state.GetComponentLookup<QuantityObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DeliveryTruckData_RO_ComponentLookup = state.GetComponentLookup<DeliveryTruckData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkVehicleData_RO_ComponentLookup = state.GetComponentLookup<WorkVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup = state.GetComponentLookup<CargoTransportVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup = state.GetComponentLookup<StorageCompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup = state.GetComponentLookup<GarbageFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup = state.GetBufferLookup<ServiceUpkeepData>(true);
      }
    }
  }
}
