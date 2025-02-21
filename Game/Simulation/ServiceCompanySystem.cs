// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ServiceCompanySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Agents;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Notifications;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class ServiceCompanySystem : GameSystemBase
  {
    private EntityQuery m_CompanyGroup;
    private EntityQuery m_EconomyParameterQuery;
    private EntityQuery m_CompanyNotificationParameterQuery;
    private EntityQuery m_BuildingParameterQuery;
    private SimulationSystem m_SimulationSystem;
    private ResourceSystem m_ResourceSystem;
    private TaxSystem m_TaxSystem;
    private IconCommandSystem m_IconCommandSystem;
    private ServiceCompanySystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      return 262144 / (EconomyUtils.kCompanyUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem = this.World.GetOrCreateSystemManaged<TaxSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyGroup = this.GetEntityQuery(ComponentType.ReadOnly<CompanyData>(), ComponentType.ReadWrite<ServiceAvailable>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<PropertyRenter>(), ComponentType.ReadOnly<WorkProvider>(), ComponentType.ReadOnly<Employee>(), ComponentType.ReadOnly<UpdateFrame>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyNotificationParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<CompanyNotificationParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<BuildingConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CompanyGroup);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CompanyNotificationParameterQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BuildingParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, EconomyUtils.kCompanyUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_TaxPayer_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CompanyNotifications_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ServiceAvailable_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_LodgingProvider_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ServiceCompanySystem.UpdateServiceJob jobData = new ServiceCompanySystem.UpdateServiceJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PropertyRenterType = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
        m_LodgingProviderType = this.__TypeHandle.__Game_Companies_LodgingProvider_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_ServiceAvailableType = this.__TypeHandle.__Game_Companies_ServiceAvailable_RW_ComponentTypeHandle,
        m_CompanyNotificationsType = this.__TypeHandle.__Game_Companies_CompanyNotifications_RW_ComponentTypeHandle,
        m_EmployeeType = this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle,
        m_RenterType = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ServiceCompanyDatas = this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup,
        m_BuildingEfficiencies = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup,
        m_Buildings = this.__TypeHandle.__Game_Buildings_Building_RW_ComponentLookup,
        m_IndustrialProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_DistrictModifiers = this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup,
        m_Districts = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup,
        m_TaxPayers = this.__TypeHandle.__Game_Agents_TaxPayer_RW_ComponentLookup,
        m_TaxRates = this.m_TaxSystem.GetTaxRates(),
        m_EconomyParameters = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_CompanyNotificationParameters = this.m_CompanyNotificationParameterQuery.GetSingleton<CompanyNotificationParameterData>(),
        m_BuildingConfigurationData = this.m_BuildingParameterQuery.GetSingleton<BuildingConfigurationData>(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_RandomSeed = RandomSeed.Next(),
        m_UpdateFrameIndex = updateFrame
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<ServiceCompanySystem.UpdateServiceJob>(this.m_CompanyGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TaxSystem.AddReader(this.Dependency);
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
    public ServiceCompanySystem()
    {
    }

    [BurstCompile]
    private struct UpdateServiceJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_PropertyRenterType;
      public ComponentTypeHandle<ServiceAvailable> m_ServiceAvailableType;
      [ReadOnly]
      public ComponentTypeHandle<LodgingProvider> m_LodgingProviderType;
      public ComponentTypeHandle<CompanyNotifications> m_CompanyNotificationsType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public BufferTypeHandle<Employee> m_EmployeeType;
      [ReadOnly]
      public BufferTypeHandle<Renter> m_RenterType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> m_ServiceCompanyDatas;
      [ReadOnly]
      public BufferLookup<Efficiency> m_BuildingEfficiencies;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> m_Resources;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_Districts;
      [ReadOnly]
      public BufferLookup<DistrictModifier> m_DistrictModifiers;
      [ReadOnly]
      public NativeArray<int> m_TaxRates;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<TaxPayer> m_TaxPayers;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Building> m_Buildings;
      [ReadOnly]
      public EconomyParameterData m_EconomyParameters;
      [ReadOnly]
      public CompanyNotificationParameterData m_CompanyNotificationParameters;
      [ReadOnly]
      public BuildingConfigurationData m_BuildingConfigurationData;
      public IconCommandBuffer m_IconCommandBuffer;
      public RandomSeed m_RandomSeed;
      public uint m_UpdateFrameIndex;

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
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PropertyRenter> nativeArray2 = chunk.GetNativeArray<PropertyRenter>(ref this.m_PropertyRenterType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ServiceAvailable> nativeArray3 = chunk.GetNativeArray<ServiceAvailable>(ref this.m_ServiceAvailableType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<LodgingProvider> nativeArray4 = chunk.GetNativeArray<LodgingProvider>(ref this.m_LodgingProviderType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CompanyNotifications> nativeArray5 = chunk.GetNativeArray<CompanyNotifications>(ref this.m_CompanyNotificationsType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Employee> bufferAccessor1 = chunk.GetBufferAccessor<Employee>(ref this.m_EmployeeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Renter> bufferAccessor2 = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity = nativeArray1[index];
          Entity property = nativeArray2[index].m_Property;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Buildings.HasComponent(property))
          {
            // ISSUE: reference to a compiler-generated field
            Entity prefab = this.m_Prefabs[entity].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ServiceCompanyDatas.HasComponent(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              ServiceCompanyData serviceCompanyData = this.m_ServiceCompanyDatas[prefab];
              ServiceAvailable serviceAvailable = nativeArray3[index];
              CompanyNotifications companyNotifications = nativeArray5[index];
              // ISSUE: reference to a compiler-generated field
              Resource resource = this.m_IndustrialProcessDatas[prefab].m_Output.m_Resource;
              DynamicBuffer<Employee> employees = bufferAccessor1[index];
              float buildingEfficiency = 1f;
              DynamicBuffer<Efficiency> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_BuildingEfficiencies.TryGetBuffer(property, out bufferData))
                buildingEfficiency = BuildingUtils.GetEfficiency(bufferData);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int intRandom = MathUtils.RoundToIntRandom(ref random, 1f * (float) EconomyUtils.GetCompanyProductionPerDay(buildingEfficiency, false, employees, this.m_IndustrialProcessDatas[prefab], this.m_ResourcePrefabs, this.m_ResourceDatas, this.m_Citizens, ref this.m_EconomyParameters) / (float) EconomyUtils.kCompanyUpdatesPerDay);
              serviceAvailable.m_ServiceAvailable = math.min(serviceCompanyData.m_MaxService, serviceAvailable.m_ServiceAvailable + intRandom);
              nativeArray3[index] = serviceAvailable;
              // ISSUE: reference to a compiler-generated field
              if (this.m_TaxPayers.HasComponent(entity))
              {
                int commercialTaxRate;
                // ISSUE: reference to a compiler-generated field
                if (this.m_Districts.HasComponent(property))
                {
                  // ISSUE: reference to a compiler-generated field
                  Entity district = this.m_Districts[property].m_District;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  commercialTaxRate = TaxSystem.GetModifiedCommercialTaxRate(resource, this.m_TaxRates, district, this.m_DistrictModifiers);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  commercialTaxRate = TaxSystem.GetCommercialTaxRate(resource, this.m_TaxRates);
                }
                // ISSUE: reference to a compiler-generated field
                TaxPayer taxPayer = this.m_TaxPayers[entity];
                if ((double) intRandom > 0.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  int num = (int) math.ceil(math.max(0.0f, (float) intRandom * EconomyUtils.GetServicePrice(resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas)));
                  taxPayer.m_UntaxedIncome += num;
                  if (num > 0)
                    taxPayer.m_AverageTaxRate = Mathf.RoundToInt(math.lerp((float) taxPayer.m_AverageTaxRate, (float) commercialTaxRate, (float) num / (float) (num + taxPayer.m_UntaxedIncome)));
                  // ISSUE: reference to a compiler-generated field
                  this.m_TaxPayers[entity] = taxPayer;
                }
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              bool flag = (double) serviceAvailable.m_ServiceAvailable / (double) math.max(1f, (float) serviceCompanyData.m_MaxService) > (double) this.m_CompanyNotificationParameters.m_NoCustomersServiceLimit && (resource == Resource.NoResource || EconomyUtils.GetResources(resource, this.m_Resources[entity]) > 200);
              if (flag && nativeArray4.Length > 0 && bufferAccessor2.Length > 0 && nativeArray4[index].m_FreeRooms > 0)
              {
                // ISSUE: reference to a compiler-generated field
                flag = 1.0 * (double) nativeArray4[index].m_FreeRooms / (double) (nativeArray4[index].m_FreeRooms + bufferAccessor2[index].Length) > (double) this.m_CompanyNotificationParameters.m_NoCustomersHotelLimit;
              }
              if (companyNotifications.m_NoCustomersEntity == new Entity())
              {
                if (flag)
                {
                  // ISSUE: reference to a compiler-generated field
                  if ((this.m_Buildings[property].m_Flags & Game.Buildings.BuildingFlags.HighRentWarning) != Game.Buildings.BuildingFlags.None)
                  {
                    // ISSUE: reference to a compiler-generated field
                    Building building = this.m_Buildings[property];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_IconCommandBuffer.Remove(property, this.m_BuildingConfigurationData.m_HighRentNotification);
                    building.m_Flags &= ~Game.Buildings.BuildingFlags.HighRentWarning;
                    // ISSUE: reference to a compiler-generated field
                    this.m_Buildings[property] = building;
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Add(property, this.m_CompanyNotificationParameters.m_NoCustomersNotificationPrefab, IconPriority.Problem);
                  companyNotifications.m_NoCustomersEntity = property;
                  nativeArray5[index] = companyNotifications;
                }
              }
              else if (!flag)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Remove(companyNotifications.m_NoCustomersEntity, this.m_CompanyNotificationParameters.m_NoCustomersNotificationPrefab);
                companyNotifications.m_NoCustomersEntity = Entity.Null;
                nativeArray5[index] = companyNotifications;
              }
              else if (property != companyNotifications.m_NoCustomersEntity)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Remove(companyNotifications.m_NoCustomersEntity, this.m_CompanyNotificationParameters.m_NoCustomersNotificationPrefab);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Add(property, this.m_CompanyNotificationParameters.m_NoCustomersNotificationPrefab, IconPriority.Problem);
                companyNotifications.m_NoCustomersEntity = property;
                nativeArray5[index] = companyNotifications;
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
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<LodgingProvider> __Game_Companies_LodgingProvider_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      public ComponentTypeHandle<ServiceAvailable> __Game_Companies_ServiceAvailable_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CompanyNotifications> __Game_Companies_CompanyNotifications_RW_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Employee> __Game_Companies_Employee_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Renter> __Game_Buildings_Renter_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> __Game_Companies_ServiceCompanyData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Efficiency> __Game_Buildings_Efficiency_RO_BufferLookup;
      public ComponentLookup<Building> __Game_Buildings_Building_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<DistrictModifier> __Game_Areas_DistrictModifier_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentLookup;
      public ComponentLookup<TaxPayer> __Game_Agents_TaxPayer_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_LodgingProvider_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LodgingProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceAvailable_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceAvailable>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CompanyNotifications_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CompanyNotifications>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferTypeHandle = state.GetBufferTypeHandle<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferTypeHandle = state.GetBufferTypeHandle<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceCompanyData_RO_ComponentLookup = state.GetComponentLookup<ServiceCompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferLookup = state.GetBufferLookup<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RW_ComponentLookup = state.GetComponentLookup<Building>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_DistrictModifier_RO_BufferLookup = state.GetBufferLookup<DistrictModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentLookup = state.GetComponentLookup<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_TaxPayer_RW_ComponentLookup = state.GetComponentLookup<TaxPayer>();
      }
    }
  }
}
