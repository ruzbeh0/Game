// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.AverageHappinessSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class AverageHappinessSection : InfoSectionBase
  {
    private GroundPollutionSystem m_GroundPollutionSystem;
    private NoisePollutionSystem m_NoisePollutionSystem;
    private AirPollutionSystem m_AirPollutionSystem;
    private TelecomCoverageSystem m_TelecomCoverageSystem;
    private TaxSystem m_TaxSystem;
    private CitySystem m_CitySystem;
    private EntityQuery m_DistrictBuildingQuery;
    public NativeArray<int> m_Results;
    private NativeArray<int2> m_Factors;
    protected EntityQuery m_CitizenHappinessParameterQuery;
    protected EntityQuery m_GarbageParameterQuery;
    protected EntityQuery m_HappinessFactorParameterQuery;
    protected EntityQuery m_HealthcareParameterQuery;
    protected EntityQuery m_ParkParameterQuery;
    protected EntityQuery m_EducationParameterQuery;
    protected EntityQuery m_TelecomParameterQuery;
    private AverageHappinessSection.TypeHandle __TypeHandle;
    private EntityQuery __query_1244325462_0;

    protected override string group => nameof (AverageHappinessSection);

    private CitizenHappiness averageHappiness { get; set; }

    private NativeList<FactorInfo> happinessFactors { get; set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionSystem = this.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NoisePollutionSystem = this.World.GetOrCreateSystemManaged<NoisePollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionSystem = this.World.GetOrCreateSystemManaged<AirPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomCoverageSystem = this.World.GetOrCreateSystemManaged<TelecomCoverageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem = this.World.GetOrCreateSystemManaged<TaxSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DistrictBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Renter>(), ComponentType.ReadOnly<CurrentDistrict>(), ComponentType.ReadOnly<ResidentialProperty>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenHappinessParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<CitizenHappinessParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<GarbageParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_HappinessFactorParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<HappinessFactorParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<HealthcareParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ParkParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<ParkParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EducationParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EducationParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<TelecomParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_Factors = new NativeArray<int2>(28, Allocator.Persistent);
      this.happinessFactors = new NativeList<FactorInfo>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<int>(3, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Results.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Factors.Dispose();
      this.happinessFactors.Dispose();
      base.OnDestroy();
    }

    protected override void Reset()
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Factors.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Factors[index] = (int2) 0;
      }
      this.averageHappiness = new CitizenHappiness();
      this.happinessFactors.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_Results[0] = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_Results[1] = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_Results[2] = 0;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      CitizenHappinessParameterData singleton1 = this.m_CitizenHappinessParameterQuery.GetSingleton<CitizenHappinessParameterData>();
      // ISSUE: reference to a compiler-generated field
      GarbageParameterData singleton2 = this.m_GarbageParameterQuery.GetSingleton<GarbageParameterData>();
      this.CheckedStateRef.EntityManager.CompleteDependencyBeforeRW<HappinessFactorParameterData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HappinessFactorParameterData_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<HappinessFactorParameterData> dynamicBuffer = this.__TypeHandle.__Game_Prefabs_HappinessFactorParameterData_RW_BufferLookup[this.m_HappinessFactorParameterQuery.GetSingletonEntity()];
      // ISSUE: reference to a compiler-generated field
      HealthcareParameterData singleton3 = this.m_HealthcareParameterQuery.GetSingleton<HealthcareParameterData>();
      // ISSUE: reference to a compiler-generated field
      ParkParameterData singleton4 = this.m_ParkParameterQuery.GetSingleton<ParkParameterData>();
      // ISSUE: reference to a compiler-generated field
      EducationParameterData singleton5 = this.m_EducationParameterQuery.GetSingleton<EducationParameterData>();
      // ISSUE: reference to a compiler-generated field
      TelecomParameterData singleton6 = this.m_TelecomParameterQuery.GetSingleton<TelecomParameterData>();
      // ISSUE: reference to a compiler-generated field
      ServiceFeeParameterData singleton7 = this.__query_1244325462_0.GetSingleton<ServiceFeeParameterData>();
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      NativeArray<GroundPollution> buffer1 = this.m_GroundPollutionSystem.GetData(true, out dependencies1).m_Buffer;
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      NativeArray<NoisePollution> buffer2 = this.m_NoisePollutionSystem.GetData(true, out dependencies2).m_Buffer;
      JobHandle dependencies3;
      // ISSUE: reference to a compiler-generated field
      NativeArray<AirPollution> buffer3 = this.m_AirPollutionSystem.GetData(true, out dependencies3).m_Buffer;
      JobHandle dependencies4;
      // ISSUE: reference to a compiler-generated field
      CellMapData<TelecomCoverage> data = this.m_TelecomCoverageSystem.GetData(true, out dependencies4);
      dependencies1.Complete();
      dependencies2.Complete();
      dependencies3.Complete();
      dependencies4.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> taxRates = this.m_TaxSystem.GetTaxRates();
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<ServiceFee> buffer4 = this.EntityManager.GetBuffer<ServiceFee>(this.m_CitySystem.City);
      // ISSUE: reference to a compiler-generated method
      float num1 = ServiceFeeSystem.GetFee(PlayerResource.Electricity, buffer4) / singleton7.m_ElectricityFee.m_Default;
      // ISSUE: reference to a compiler-generated method
      float num2 = ServiceFeeSystem.GetFee(PlayerResource.Water, buffer4) / singleton7.m_WaterFee.m_Default;
      if (this.EntityManager.HasComponent<District>(this.selectedEntity) && this.EntityManager.HasComponent<Area>(this.selectedEntity))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        new AverageHappinessSection.CountDistrictHappinessJob()
        {
          m_SelectedEntity = this.selectedEntity,
          m_EntityHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_CitizenFromEntity = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
          m_HouseholdFromEntity = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
          m_CurrentDistrictHandle = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle,
          m_AbandonedFromEntity = this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup,
          m_HealthProblemFromEntity = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
          m_HouseholdCitizenFromEntity = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
          m_RenterFromEntity = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
          m_PrefabRefFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_SpawnableBuildingDataFromEntity = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
          m_BuildingPropertyDataFromEntity = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
          m_BuildingFromEntity = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
          m_ElectricityConsumerFromEntity = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup,
          m_WaterConsumerFromEntity = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup,
          m_LockedFromEntity = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup,
          m_TransformFromEntity = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
          m_GarbageProducersFromEntity = this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup,
          m_CrimeProducersFromEntity = this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentLookup,
          m_MailProducerFromEntity = this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup,
          m_BuildingDataFromEntity = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
          m_CityModifierFromEntity = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
          m_ServiceCoverageFromEntity = this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup,
          m_CitizenHappinessParameters = singleton1,
          m_GarbageParameters = singleton2,
          m_HealthcareParameters = singleton3,
          m_ParkParameters = singleton4,
          m_EducationParameters = singleton5,
          m_TelecomParameters = singleton6,
          m_HappinessFactorParameters = dynamicBuffer,
          m_TelecomCoverage = data,
          m_PollutionMap = buffer1,
          m_NoisePollutionMap = buffer2,
          m_AirPollutionMap = buffer3,
          m_TaxRates = taxRates,
          m_Factors = this.m_Factors,
          m_Results = this.m_Results,
          m_City = this.m_CitySystem.City,
          m_RelativeElectricityFee = num1,
          m_RelativeWaterFee = num2
        }.Schedule<AverageHappinessSection.CountDistrictHappinessJob>(this.m_DistrictBuildingQuery, this.Dependency).Complete();
        // ISSUE: reference to a compiler-generated field
        this.visible = this.m_Results[0] > 0;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_ResidentialProperty_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        new AverageHappinessSection.CountHappinessJob()
        {
          m_SelectedEntity = this.selectedEntity,
          m_BuildingFromEntity = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
          m_ResidentialPropertyFromEntity = this.__TypeHandle.__Game_Buildings_ResidentialProperty_RO_ComponentLookup,
          m_HouseholdFromEntity = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
          m_HealthProblemFromEntity = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
          m_PropertyRenterFromEntity = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
          m_AbandonedFromEntity = this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup,
          m_CitizenFromEntity = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
          m_HouseholdCitizenFromEntity = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
          m_RenterFromEntity = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
          m_PrefabRefFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_SpawnableBuildingDataFromEntity = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
          m_BuildingPropertyDataFromEntity = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
          m_ElectricityConsumerFromEntity = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup,
          m_WaterConsumerFromEntity = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup,
          m_LockedFromEntity = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup,
          m_TransformFromEntity = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
          m_GarbageProducersFromEntity = this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup,
          m_CrimeProducersFromEntity = this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentLookup,
          m_MailProducerFromEntity = this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup,
          m_BuildingDataFromEntity = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
          m_CityModifierFromEntity = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
          m_ServiceCoverageFromEntity = this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup,
          m_CitizenHappinessParameters = singleton1,
          m_GarbageParameters = singleton2,
          m_HealthcareParameters = singleton3,
          m_ParkParameters = singleton4,
          m_EducationParameters = singleton5,
          m_TelecomParameters = singleton6,
          m_HappinessFactorParameters = dynamicBuffer,
          m_TelecomCoverage = data,
          m_PollutionMap = buffer1,
          m_NoisePollutionMap = buffer2,
          m_AirPollutionMap = buffer3,
          m_TaxRates = taxRates,
          m_Factors = this.m_Factors,
          m_Results = this.m_Results,
          m_City = this.m_CitySystem.City,
          m_RelativeElectricityFee = num1,
          m_RelativeWaterFee = num2
        }.Schedule<AverageHappinessSection.CountHappinessJob>(this.Dependency).Complete();
        // ISSUE: reference to a compiler-generated field
        this.visible = this.m_Results[0] > 0;
      }
    }

    protected override void OnProcess()
    {
      // ISSUE: reference to a compiler-generated field
      int result = this.m_Results[1];
      // ISSUE: reference to a compiler-generated field
      this.averageHappiness = CitizenUIUtils.GetCitizenHappiness(this.m_Results[2] / math.select(result, 1, result == 0));
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Factors.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        int x = this.m_Factors[index].x;
        if (x > 0)
        {
          // ISSUE: reference to a compiler-generated field
          float weight = math.round((float) this.m_Factors[index].y / (float) x);
          if ((double) weight != 0.0)
            this.happinessFactors.Add(new FactorInfo(index, (int) weight));
        }
      }
      this.happinessFactors.Sort<FactorInfo>();
      if (this.EntityManager.HasComponent<Building>(this.selectedEntity))
        this.tooltipKeys.Add("Building");
      else if (this.EntityManager.HasComponent<Household>(this.selectedEntity))
        this.tooltipKeys.Add("Household");
      else
        this.tooltipKeys.Add("District");
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("averageHappiness");
      writer.Write<CitizenHappiness>(this.averageHappiness);
      int size = math.min(10, this.happinessFactors.Length);
      writer.PropertyName("happinessFactors");
      writer.ArrayBegin(size);
      for (int index = 0; index < size; ++index)
        this.happinessFactors[index].WriteBuildingHappinessFactor(writer);
      writer.ArrayEnd();
    }

    private static bool TryAddPropertyHappiness(
      ref int happiness,
      ref int citizenCount,
      Entity entity,
      ComponentLookup<Household> householdFromEntity,
      ComponentLookup<Citizen> citizenFromEntity,
      ComponentLookup<HealthProblem> healthProblemFromEntity,
      BufferLookup<Renter> renterFromEntity,
      BufferLookup<HouseholdCitizen> householdCitizenFromEntity)
    {
      bool flag = false;
      DynamicBuffer<Renter> bufferData1;
      if (renterFromEntity.TryGetBuffer(entity, out bufferData1))
      {
        for (int index1 = 0; index1 < bufferData1.Length; ++index1)
        {
          Entity renter = bufferData1[index1].m_Renter;
          DynamicBuffer<HouseholdCitizen> bufferData2;
          if (householdFromEntity.HasComponent(renter) && householdCitizenFromEntity.TryGetBuffer(renter, out bufferData2))
          {
            flag = true;
            for (int index2 = 0; index2 < bufferData2.Length; ++index2)
            {
              Entity citizen = bufferData2[index2].m_Citizen;
              if (citizenFromEntity.HasComponent(citizen) && !CitizenUtils.IsDead(citizen, ref healthProblemFromEntity))
              {
                happiness += citizenFromEntity[citizen].Happiness;
                ++citizenCount;
              }
            }
          }
        }
      }
      return flag;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_1244325462_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<ServiceFeeParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
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
    public AverageHappinessSection()
    {
    }

    public enum Result
    {
      Visible,
      ResidentCount,
      Happiness,
      ResultCount,
    }

    [BurstCompile]
    private struct CountHappinessJob : IJob
    {
      [ReadOnly]
      public Entity m_SelectedEntity;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingFromEntity;
      [ReadOnly]
      public ComponentLookup<ResidentialProperty> m_ResidentialPropertyFromEntity;
      [ReadOnly]
      public ComponentLookup<Household> m_HouseholdFromEntity;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenFromEntity;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblemFromEntity;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenterFromEntity;
      [ReadOnly]
      public ComponentLookup<Abandoned> m_AbandonedFromEntity;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizenFromEntity;
      [ReadOnly]
      public BufferLookup<Renter> m_RenterFromEntity;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefFromEntity;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDataFromEntity;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyDataFromEntity;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> m_ElectricityConsumerFromEntity;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> m_WaterConsumerFromEntity;
      [ReadOnly]
      public ComponentLookup<Locked> m_LockedFromEntity;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformFromEntity;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> m_GarbageProducersFromEntity;
      [ReadOnly]
      public ComponentLookup<CrimeProducer> m_CrimeProducersFromEntity;
      [ReadOnly]
      public ComponentLookup<MailProducer> m_MailProducerFromEntity;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDataFromEntity;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifierFromEntity;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> m_ServiceCoverageFromEntity;
      public CitizenHappinessParameterData m_CitizenHappinessParameters;
      public GarbageParameterData m_GarbageParameters;
      public HealthcareParameterData m_HealthcareParameters;
      public ParkParameterData m_ParkParameters;
      public EducationParameterData m_EducationParameters;
      public TelecomParameterData m_TelecomParameters;
      [ReadOnly]
      public DynamicBuffer<HappinessFactorParameterData> m_HappinessFactorParameters;
      [ReadOnly]
      public CellMapData<TelecomCoverage> m_TelecomCoverage;
      [ReadOnly]
      public NativeArray<GroundPollution> m_PollutionMap;
      [ReadOnly]
      public NativeArray<NoisePollution> m_NoisePollutionMap;
      [ReadOnly]
      public NativeArray<AirPollution> m_AirPollutionMap;
      [ReadOnly]
      public NativeArray<int> m_TaxRates;
      public NativeArray<int2> m_Factors;
      public NativeArray<int> m_Results;
      public Entity m_City;
      public float m_RelativeElectricityFee;
      public float m_RelativeWaterFee;

      public void Execute()
      {
        int happiness = 0;
        int citizenCount = 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_BuildingFromEntity.HasComponent(this.m_SelectedEntity) && this.m_ResidentialPropertyFromEntity.HasComponent(this.m_SelectedEntity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool flag = this.m_AbandonedFromEntity.HasComponent(this.m_SelectedEntity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          BuildingHappiness.GetResidentialBuildingHappinessFactors(this.m_City, this.m_TaxRates, this.m_SelectedEntity, this.m_Factors, ref this.m_PrefabRefFromEntity, ref this.m_SpawnableBuildingDataFromEntity, ref this.m_BuildingPropertyDataFromEntity, ref this.m_CityModifierFromEntity, ref this.m_BuildingFromEntity, ref this.m_ElectricityConsumerFromEntity, ref this.m_WaterConsumerFromEntity, ref this.m_ServiceCoverageFromEntity, ref this.m_LockedFromEntity, ref this.m_TransformFromEntity, ref this.m_GarbageProducersFromEntity, ref this.m_CrimeProducersFromEntity, ref this.m_MailProducerFromEntity, ref this.m_RenterFromEntity, ref this.m_CitizenFromEntity, ref this.m_HouseholdCitizenFromEntity, ref this.m_BuildingDataFromEntity, this.m_CitizenHappinessParameters, this.m_GarbageParameters, this.m_HealthcareParameters, this.m_ParkParameters, this.m_EducationParameters, this.m_TelecomParameters, this.m_HappinessFactorParameters, this.m_PollutionMap, this.m_NoisePollutionMap, this.m_AirPollutionMap, this.m_TelecomCoverage, this.m_RelativeElectricityFee, this.m_RelativeWaterFee);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (AverageHappinessSection.TryAddPropertyHappiness(ref happiness, ref citizenCount, this.m_SelectedEntity, this.m_HouseholdFromEntity, this.m_CitizenFromEntity, this.m_HealthProblemFromEntity, this.m_RenterFromEntity, this.m_HouseholdCitizenFromEntity))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Results[1] = citizenCount;
            // ISSUE: reference to a compiler-generated field
            this.m_Results[2] = happiness;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_Results[0] = citizenCount > 0 | flag ? 1 : 0;
        }
        else
        {
          DynamicBuffer<HouseholdCitizen> bufferData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_HouseholdCitizenFromEntity.TryGetBuffer(this.m_SelectedEntity, out bufferData))
            return;
          for (int index = 0; index < bufferData.Length; ++index)
          {
            Entity citizen = bufferData[index].m_Citizen;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_CitizenFromEntity.HasComponent(citizen) && !CitizenUtils.IsDead(citizen, ref this.m_HealthProblemFromEntity))
            {
              // ISSUE: reference to a compiler-generated field
              happiness += this.m_CitizenFromEntity[citizen].Happiness;
              ++citizenCount;
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_Results[0] = 1;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[1] = citizenCount;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[2] = happiness;
          PropertyRenter componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PropertyRenterFromEntity.TryGetComponent(this.m_SelectedEntity, out componentData))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          BuildingHappiness.GetResidentialBuildingHappinessFactors(this.m_City, this.m_TaxRates, componentData.m_Property, this.m_Factors, ref this.m_PrefabRefFromEntity, ref this.m_SpawnableBuildingDataFromEntity, ref this.m_BuildingPropertyDataFromEntity, ref this.m_CityModifierFromEntity, ref this.m_BuildingFromEntity, ref this.m_ElectricityConsumerFromEntity, ref this.m_WaterConsumerFromEntity, ref this.m_ServiceCoverageFromEntity, ref this.m_LockedFromEntity, ref this.m_TransformFromEntity, ref this.m_GarbageProducersFromEntity, ref this.m_CrimeProducersFromEntity, ref this.m_MailProducerFromEntity, ref this.m_RenterFromEntity, ref this.m_CitizenFromEntity, ref this.m_HouseholdCitizenFromEntity, ref this.m_BuildingDataFromEntity, this.m_CitizenHappinessParameters, this.m_GarbageParameters, this.m_HealthcareParameters, this.m_ParkParameters, this.m_EducationParameters, this.m_TelecomParameters, this.m_HappinessFactorParameters, this.m_PollutionMap, this.m_NoisePollutionMap, this.m_AirPollutionMap, this.m_TelecomCoverage, this.m_RelativeElectricityFee, this.m_RelativeWaterFee);
        }
      }
    }

    [BurstCompile]
    public struct CountDistrictHappinessJob : IJobChunk
    {
      [ReadOnly]
      public Entity m_SelectedEntity;
      [ReadOnly]
      public EntityTypeHandle m_EntityHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> m_CurrentDistrictHandle;
      [ReadOnly]
      public ComponentLookup<Abandoned> m_AbandonedFromEntity;
      [ReadOnly]
      public ComponentLookup<Household> m_HouseholdFromEntity;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenFromEntity;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblemFromEntity;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizenFromEntity;
      [ReadOnly]
      public BufferLookup<Renter> m_RenterFromEntity;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefFromEntity;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDataFromEntity;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingFromEntity;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> m_ElectricityConsumerFromEntity;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> m_WaterConsumerFromEntity;
      [ReadOnly]
      public ComponentLookup<Locked> m_LockedFromEntity;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformFromEntity;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> m_GarbageProducersFromEntity;
      [ReadOnly]
      public ComponentLookup<CrimeProducer> m_CrimeProducersFromEntity;
      [ReadOnly]
      public ComponentLookup<MailProducer> m_MailProducerFromEntity;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDataFromEntity;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifierFromEntity;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> m_ServiceCoverageFromEntity;
      public CitizenHappinessParameterData m_CitizenHappinessParameters;
      public GarbageParameterData m_GarbageParameters;
      public HealthcareParameterData m_HealthcareParameters;
      public ParkParameterData m_ParkParameters;
      public EducationParameterData m_EducationParameters;
      public TelecomParameterData m_TelecomParameters;
      [ReadOnly]
      public DynamicBuffer<HappinessFactorParameterData> m_HappinessFactorParameters;
      [ReadOnly]
      public CellMapData<TelecomCoverage> m_TelecomCoverage;
      [ReadOnly]
      public NativeArray<GroundPollution> m_PollutionMap;
      [ReadOnly]
      public NativeArray<NoisePollution> m_NoisePollutionMap;
      [ReadOnly]
      public NativeArray<AirPollution> m_AirPollutionMap;
      [ReadOnly]
      public NativeArray<int> m_TaxRates;
      public NativeArray<int2> m_Factors;
      public NativeArray<int> m_Results;
      public Entity m_City;
      public float m_RelativeElectricityFee;
      public float m_RelativeWaterFee;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentDistrict> nativeArray2 = chunk.GetNativeArray<CurrentDistrict>(ref this.m_CurrentDistrictHandle);
        int num = 0;
        int happiness = 0;
        int citizenCount = 0;
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
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
          if (!(nativeArray2[index].m_District != this.m_SelectedEntity) && this.m_SpawnableBuildingDataFromEntity.HasComponent(this.m_PrefabRefFromEntity[entity].m_Prefab) && (AverageHappinessSection.TryAddPropertyHappiness(ref happiness, ref citizenCount, entity, this.m_HouseholdFromEntity, this.m_CitizenFromEntity, this.m_HealthProblemFromEntity, this.m_RenterFromEntity, this.m_HouseholdCitizenFromEntity) || this.m_AbandonedFromEntity.HasComponent(entity)))
          {
            num = 1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            BuildingHappiness.GetResidentialBuildingHappinessFactors(this.m_City, this.m_TaxRates, entity, this.m_Factors, ref this.m_PrefabRefFromEntity, ref this.m_SpawnableBuildingDataFromEntity, ref this.m_BuildingPropertyDataFromEntity, ref this.m_CityModifierFromEntity, ref this.m_BuildingFromEntity, ref this.m_ElectricityConsumerFromEntity, ref this.m_WaterConsumerFromEntity, ref this.m_ServiceCoverageFromEntity, ref this.m_LockedFromEntity, ref this.m_TransformFromEntity, ref this.m_GarbageProducersFromEntity, ref this.m_CrimeProducersFromEntity, ref this.m_MailProducerFromEntity, ref this.m_RenterFromEntity, ref this.m_CitizenFromEntity, ref this.m_HouseholdCitizenFromEntity, ref this.m_BuildingDataFromEntity, this.m_CitizenHappinessParameters, this.m_GarbageParameters, this.m_HealthcareParameters, this.m_ParkParameters, this.m_EducationParameters, this.m_TelecomParameters, this.m_HappinessFactorParameters, this.m_PollutionMap, this.m_NoisePollutionMap, this.m_AirPollutionMap, this.m_TelecomCoverage, this.m_RelativeElectricityFee, this.m_RelativeWaterFee);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Results[0] += num;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[1] += citizenCount;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[2] += happiness;
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
      public BufferLookup<HappinessFactorParameterData> __Game_Prefabs_HappinessFactorParameterData_RW_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Abandoned> __Game_Buildings_Abandoned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> __Game_Buildings_WaterConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Locked> __Game_Prefabs_Locked_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> __Game_Buildings_GarbageProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CrimeProducer> __Game_Buildings_CrimeProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MailProducer> __Game_Buildings_MailProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> __Game_Net_ServiceCoverage_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ResidentialProperty> __Game_Buildings_ResidentialProperty_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HappinessFactorParameterData_RW_BufferLookup = state.GetBufferLookup<HappinessFactorParameterData>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Abandoned_RO_ComponentLookup = state.GetComponentLookup<Abandoned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentLookup = state.GetComponentLookup<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup = state.GetComponentLookup<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RO_ComponentLookup = state.GetComponentLookup<WaterConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentLookup = state.GetComponentLookup<Locked>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageProducer_RO_ComponentLookup = state.GetComponentLookup<GarbageProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CrimeProducer_RO_ComponentLookup = state.GetComponentLookup<CrimeProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_MailProducer_RO_ComponentLookup = state.GetComponentLookup<MailProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ServiceCoverage_RO_BufferLookup = state.GetBufferLookup<Game.Net.ServiceCoverage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ResidentialProperty_RO_ComponentLookup = state.GetComponentLookup<ResidentialProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
      }
    }
  }
}
