// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ProfitabilitySection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Net;
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
  public class ProfitabilitySection : InfoSectionBase
  {
    private TaxSystem m_TaxSystem;
    private ResourceSystem m_ResourceSystem;
    private EntityQuery m_DistrictBuildingQuery;
    private EntityQuery m_ProcessQuery;
    private EntityQuery m_EconomyParameterQuery;
    public NativeArray<int> m_Results;
    private NativeArray<int2> m_Factors;
    private ProfitabilitySection.TypeHandle __TypeHandle;

    protected override string group => nameof (ProfitabilitySection);

    private CompanyProfitability profitability { get; set; }

    private NativeList<FactorInfo> profitabilityFactors { get; set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem = this.World.GetOrCreateSystemManaged<TaxSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DistrictBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Renter>(), ComponentType.ReadOnly<CurrentDistrict>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessQuery = this.GetEntityQuery(ComponentType.ReadOnly<IndustrialProcessData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_Factors = new NativeArray<int2>(28, Allocator.Persistent);
      this.profitabilityFactors = new NativeList<FactorInfo>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
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
      this.profitabilityFactors.Dispose();
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
      this.profitability = new CompanyProfitability();
      this.profitabilityFactors.Clear();
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
      NativeArray<Entity> entityArray = this.m_ProcessQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      EconomyParameterData singleton = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> taxRates = this.m_TaxSystem.GetTaxRates();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      if (this.EntityManager.HasComponent<District>(this.selectedEntity) && this.EntityManager.HasComponent<Area>(this.selectedEntity))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_TradeCost_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ZonePropertiesData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        this.__TypeHandle.__Game_Companies_Profitability_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        new ProfitabilitySection.DistrictProfitabilityJob()
        {
          m_SelectedEntity = this.selectedEntity,
          m_EntityHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_PrefabRefHandle = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_CitizenFromEntity = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
          m_CurrentDistrictHandle = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle,
          m_AbandonedFromEntity = this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup,
          m_HealthProblemFromEntity = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
          m_ProfitabilityFromEntity = this.__TypeHandle.__Game_Companies_Profitability_RO_ComponentLookup,
          m_RenterFromEntity = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
          m_PrefabRefFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_SpawnableBuildingDataFromEntity = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
          m_BuildingPropertyDataFromEntity = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
          m_BuildingFromEntity = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
          m_CompanyDataFromEntity = this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup,
          m_BuildingDataFromEntity = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
          m_OfficeBuildingFromEntity = this.__TypeHandle.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup,
          m_IndustrialProcessDataFromEntity = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
          m_WorkProviderFromEntity = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup,
          m_ServiceAvailableFromEntity = this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentLookup,
          m_BuildingEfficiencyFromEntity = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup,
          m_WorkplaceDataFromEntity = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup,
          m_ResourceDataFromEntity = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
          m_ZonePropertyDataFromEntity = this.__TypeHandle.__Game_Prefabs_ZonePropertiesData_RO_ComponentLookup,
          m_ServiceCompanyDataFromEntity = this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup,
          m_ResourceAvailabilityFromEntity = this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup,
          m_TradeCostFromEntity = this.__TypeHandle.__Game_Companies_TradeCost_RO_BufferLookup,
          m_EmployeeFromEntity = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup,
          m_EconomyParameters = singleton,
          m_ResourcePrefabs = prefabs,
          m_TaxRates = taxRates,
          m_Processes = entityArray,
          m_Factors = this.m_Factors,
          m_Results = this.m_Results
        }.Schedule<ProfitabilitySection.DistrictProfitabilityJob>(this.m_DistrictBuildingQuery, this.Dependency).Complete();
        // ISSUE: reference to a compiler-generated field
        this.visible = this.m_Results[0] > 0;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_TradeCost_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ZonePropertiesData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_Profitability_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        new ProfitabilitySection.ProfitabilityJob()
        {
          m_SelectedEntity = this.selectedEntity,
          m_SelectedPrefab = this.selectedPrefab,
          m_BuildingFromEntity = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
          m_HealthProblemFromEntity = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
          m_ProfitabilityFromEntity = this.__TypeHandle.__Game_Companies_Profitability_RO_ComponentLookup,
          m_AbandonedFromEntity = this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup,
          m_CitizenFromEntity = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
          m_RenterFromEntity = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
          m_PrefabRefFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_SpawnableBuildingDataFromEntity = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
          m_BuildingPropertyDataFromEntity = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
          m_CompanyDataFromEntity = this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup,
          m_BuildingDataFromEntity = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
          m_OfficeBuildingFromEntity = this.__TypeHandle.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup,
          m_IndustrialProcessDataFromEntity = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
          m_WorkProviderFromEntity = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup,
          m_ServiceAvailableFromEntity = this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentLookup,
          m_BuildingEfficiencyFromEntity = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup,
          m_WorkplaceDataFromEntity = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup,
          m_ResourceDataFromEntity = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
          m_ZonePropertyDataFromEntity = this.__TypeHandle.__Game_Prefabs_ZonePropertiesData_RO_ComponentLookup,
          m_ServiceCompanyDataFromEntity = this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup,
          m_ResourceAvailabilityFromEntity = this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup,
          m_TradeCostFromEntity = this.__TypeHandle.__Game_Companies_TradeCost_RO_BufferLookup,
          m_EmployeeFromEntity = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup,
          m_EconomyParameters = singleton,
          m_ResourcePrefabs = prefabs,
          m_TaxRates = taxRates,
          m_Processes = entityArray,
          m_Factors = this.m_Factors,
          m_Results = this.m_Results
        }.Schedule<ProfitabilitySection.ProfitabilityJob>(this.Dependency).Complete();
        // ISSUE: reference to a compiler-generated field
        this.visible = this.m_Results[0] > 0;
      }
    }

    protected override void OnProcess()
    {
      // ISSUE: reference to a compiler-generated field
      int result = this.m_Results[1];
      // ISSUE: reference to a compiler-generated field
      this.profitability = new CompanyProfitability(result == 0 ? 0 : (int) math.round((float) ((double) this.m_Results[2] / (double) result * 2.0 - (double) byte.MaxValue)));
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
            this.profitabilityFactors.Add(new FactorInfo(index, (int) weight));
        }
      }
      this.profitabilityFactors.Sort<FactorInfo>();
      if (this.EntityManager.HasComponent<Building>(this.selectedEntity))
        this.tooltipKeys.Add("Company");
      else
        this.tooltipKeys.Add("District");
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("profitability");
      writer.Write<CompanyProfitability>(this.profitability);
      int size = math.min(10, this.profitabilityFactors.Length);
      writer.PropertyName("profitabilityFactors");
      writer.ArrayBegin(size);
      for (int index = 0; index < size; ++index)
        this.profitabilityFactors[index].WriteBuildingHappinessFactor(writer);
      writer.ArrayEnd();
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
    public ProfitabilitySection()
    {
    }

    public enum Result
    {
      Visible,
      CompanyCount,
      Profitability,
      ResultCount,
    }

    [BurstCompile]
    private struct ProfitabilityJob : IJob
    {
      [ReadOnly]
      public Entity m_SelectedEntity;
      [ReadOnly]
      public Entity m_SelectedPrefab;
      [ReadOnly]
      public BufferLookup<Employee> m_EmployeeFromEntity;
      [ReadOnly]
      public BufferLookup<Renter> m_RenterFromEntity;
      [ReadOnly]
      public BufferLookup<ResourceAvailability> m_ResourceAvailabilityFromEntity;
      [ReadOnly]
      public BufferLookup<TradeCost> m_TradeCostFromEntity;
      [ReadOnly]
      public ComponentLookup<Abandoned> m_AbandonedFromEntity;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingFromEntity;
      [ReadOnly]
      public BufferLookup<Efficiency> m_BuildingEfficiencyFromEntity;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenFromEntity;
      [ReadOnly]
      public ComponentLookup<CompanyData> m_CompanyDataFromEntity;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblemFromEntity;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDataFromEntity;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyDataFromEntity;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDataFromEntity;
      [ReadOnly]
      public ComponentLookup<OfficeBuilding> m_OfficeBuildingFromEntity;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefFromEntity;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDataFromEntity;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDataFromEntity;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> m_WorkplaceDataFromEntity;
      [ReadOnly]
      public ComponentLookup<ZonePropertiesData> m_ZonePropertyDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Profitability> m_ProfitabilityFromEntity;
      [ReadOnly]
      public ComponentLookup<ServiceAvailable> m_ServiceAvailableFromEntity;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> m_ServiceCompanyDataFromEntity;
      [ReadOnly]
      public ComponentLookup<WorkProvider> m_WorkProviderFromEntity;
      public EconomyParameterData m_EconomyParameters;
      public ResourcePrefabs m_ResourcePrefabs;
      public NativeArray<int> m_TaxRates;
      public NativeArray<Entity> m_Processes;
      public NativeArray<int2> m_Factors;
      public NativeArray<int> m_Results;

      public void Execute()
      {
        byte num1 = 0;
        int num2 = 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_BuildingFromEntity.HasComponent(this.m_SelectedEntity) || !this.m_SpawnableBuildingDataFromEntity.HasComponent(this.m_SelectedPrefab))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag = this.m_AbandonedFromEntity.HasComponent(this.m_SelectedEntity);
        Entity company;
        Profitability componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (CompanyUIUtils.HasCompany(this.m_SelectedEntity, this.m_SelectedPrefab, ref this.m_RenterFromEntity, ref this.m_BuildingPropertyDataFromEntity, ref this.m_CompanyDataFromEntity, out company) && this.m_ProfitabilityFromEntity.TryGetComponent(company, out componentData))
        {
          num1 = componentData.m_Profitability;
          num2 = 1;
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BuildingHappiness.GetCompanyHappinessFactors(this.m_SelectedEntity, this.m_Factors, ref this.m_PrefabRefFromEntity, ref this.m_SpawnableBuildingDataFromEntity, ref this.m_BuildingPropertyDataFromEntity, ref this.m_BuildingFromEntity, ref this.m_OfficeBuildingFromEntity, ref this.m_RenterFromEntity, ref this.m_BuildingDataFromEntity, ref this.m_CompanyDataFromEntity, ref this.m_IndustrialProcessDataFromEntity, ref this.m_WorkProviderFromEntity, ref this.m_EmployeeFromEntity, ref this.m_WorkplaceDataFromEntity, ref this.m_CitizenFromEntity, ref this.m_HealthProblemFromEntity, ref this.m_ServiceAvailableFromEntity, ref this.m_ResourceDataFromEntity, ref this.m_ZonePropertyDataFromEntity, ref this.m_BuildingEfficiencyFromEntity, ref this.m_ServiceCompanyDataFromEntity, ref this.m_ResourceAvailabilityFromEntity, ref this.m_TradeCostFromEntity, this.m_EconomyParameters, this.m_TaxRates, this.m_Processes, this.m_ResourcePrefabs);
        // ISSUE: reference to a compiler-generated field
        this.m_Results[1] = num2;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[2] = (int) num1;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[0] = num2 > 0 | flag ? 1 : 0;
      }
    }

    [BurstCompile]
    public struct DistrictProfitabilityJob : IJobChunk
    {
      [ReadOnly]
      public Entity m_SelectedEntity;
      [ReadOnly]
      public EntityTypeHandle m_EntityHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> m_CurrentDistrictHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefHandle;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingFromEntity;
      [ReadOnly]
      public ComponentLookup<CompanyData> m_CompanyDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Abandoned> m_AbandonedFromEntity;
      [ReadOnly]
      public BufferLookup<Renter> m_RenterFromEntity;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenFromEntity;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblemFromEntity;
      [ReadOnly]
      public ComponentLookup<Profitability> m_ProfitabilityFromEntity;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefFromEntity;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDataFromEntity;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDataFromEntity;
      [ReadOnly]
      public ComponentLookup<OfficeBuilding> m_OfficeBuildingFromEntity;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDataFromEntity;
      [ReadOnly]
      public ComponentLookup<WorkProvider> m_WorkProviderFromEntity;
      [ReadOnly]
      public ComponentLookup<ServiceAvailable> m_ServiceAvailableFromEntity;
      [ReadOnly]
      public BufferLookup<Efficiency> m_BuildingEfficiencyFromEntity;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> m_WorkplaceDataFromEntity;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDataFromEntity;
      [ReadOnly]
      public ComponentLookup<ZonePropertiesData> m_ZonePropertyDataFromEntity;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> m_ServiceCompanyDataFromEntity;
      [ReadOnly]
      public BufferLookup<ResourceAvailability> m_ResourceAvailabilityFromEntity;
      [ReadOnly]
      public BufferLookup<TradeCost> m_TradeCostFromEntity;
      [ReadOnly]
      public BufferLookup<Employee> m_EmployeeFromEntity;
      public EconomyParameterData m_EconomyParameters;
      public ResourcePrefabs m_ResourcePrefabs;
      public NativeArray<int> m_TaxRates;
      public NativeArray<Entity> m_Processes;
      public NativeArray<int2> m_Factors;
      public NativeArray<int> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentDistrict> nativeArray3 = chunk.GetNativeArray<CurrentDistrict>(ref this.m_CurrentDistrictHandle);
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Entity prefab = nativeArray2[index].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!(nativeArray3[index].m_District != this.m_SelectedEntity) && this.m_SpawnableBuildingDataFromEntity.HasComponent(prefab))
          {
            Entity company;
            Profitability componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (CompanyUIUtils.HasCompany(entity, prefab, ref this.m_RenterFromEntity, ref this.m_BuildingPropertyDataFromEntity, ref this.m_CompanyDataFromEntity, out company) && this.m_ProfitabilityFromEntity.TryGetComponent(company, out componentData))
            {
              num2 += (int) componentData.m_Profitability;
              ++num3;
              num1 = 1;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_AbandonedFromEntity.HasComponent(entity))
              num1 = 1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            BuildingHappiness.GetCompanyHappinessFactors(entity, this.m_Factors, ref this.m_PrefabRefFromEntity, ref this.m_SpawnableBuildingDataFromEntity, ref this.m_BuildingPropertyDataFromEntity, ref this.m_BuildingFromEntity, ref this.m_OfficeBuildingFromEntity, ref this.m_RenterFromEntity, ref this.m_BuildingDataFromEntity, ref this.m_CompanyDataFromEntity, ref this.m_IndustrialProcessDataFromEntity, ref this.m_WorkProviderFromEntity, ref this.m_EmployeeFromEntity, ref this.m_WorkplaceDataFromEntity, ref this.m_CitizenFromEntity, ref this.m_HealthProblemFromEntity, ref this.m_ServiceAvailableFromEntity, ref this.m_ResourceDataFromEntity, ref this.m_ZonePropertyDataFromEntity, ref this.m_BuildingEfficiencyFromEntity, ref this.m_ServiceCompanyDataFromEntity, ref this.m_ResourceAvailabilityFromEntity, ref this.m_TradeCostFromEntity, this.m_EconomyParameters, this.m_TaxRates, this.m_Processes, this.m_ResourcePrefabs);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Results[0] += num1;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[1] += num3;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[2] += num2;
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Abandoned> __Game_Buildings_Abandoned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Profitability> __Game_Companies_Profitability_RO_ComponentLookup;
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
      public ComponentLookup<CompanyData> __Game_Companies_CompanyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OfficeBuilding> __Game_Prefabs_OfficeBuilding_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkProvider> __Game_Companies_WorkProvider_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceAvailable> __Game_Companies_ServiceAvailable_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Efficiency> __Game_Buildings_Efficiency_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> __Game_Prefabs_WorkplaceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZonePropertiesData> __Game_Prefabs_ZonePropertiesData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> __Game_Companies_ServiceCompanyData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ResourceAvailability> __Game_Net_ResourceAvailability_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<TradeCost> __Game_Companies_TradeCost_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Employee> __Game_Companies_Employee_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Abandoned_RO_ComponentLookup = state.GetComponentLookup<Abandoned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentLookup = state.GetComponentLookup<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Profitability_RO_ComponentLookup = state.GetComponentLookup<Profitability>(true);
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
        this.__Game_Companies_CompanyData_RO_ComponentLookup = state.GetComponentLookup<CompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup = state.GetComponentLookup<OfficeBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RO_ComponentLookup = state.GetComponentLookup<WorkProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceAvailable_RO_ComponentLookup = state.GetComponentLookup<ServiceAvailable>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferLookup = state.GetBufferLookup<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkplaceData_RO_ComponentLookup = state.GetComponentLookup<WorkplaceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZonePropertiesData_RO_ComponentLookup = state.GetComponentLookup<ZonePropertiesData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceCompanyData_RO_ComponentLookup = state.GetComponentLookup<ServiceCompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ResourceAvailability_RO_BufferLookup = state.GetBufferLookup<ResourceAvailability>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_TradeCost_RO_BufferLookup = state.GetBufferLookup<TradeCost>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferLookup = state.GetBufferLookup<Employee>(true);
      }
    }
  }
}
