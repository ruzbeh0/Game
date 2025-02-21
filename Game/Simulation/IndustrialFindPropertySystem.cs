// Decompiled with JetBrains decompiler
// Type: Game.Simulation.IndustrialFindPropertySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Agents;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class IndustrialFindPropertySystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private ResourceSystem m_ResourceSystem;
    private PropertyProcessingSystem m_PropertyProcessingSystem;
    private IndustrialDemandSystem m_IndustrialDemandSystem;
    private CountCompanyDataSystem m_CountCompanyDataSystem;
    private ClimateSystem m_ClimateSystem;
    private EntityQuery m_IndustryQuery;
    private EntityQuery m_ExtractorQuery;
    private EntityQuery m_FreePropertyQuery;
    private EntityQuery m_EconomyParameterQuery;
    private EntityQuery m_ZonePreferenceQuery;
    private EntityQuery m_FreeExtractorQuery;
    private EntityQuery m_CompanyPrefabQuery;
    private EntityQuery m_ExtractorParameterQuery;
    private IndustrialFindPropertySystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PropertyProcessingSystem = this.World.GetOrCreateSystemManaged<PropertyProcessingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialDemandSystem = this.World.GetOrCreateSystemManaged<IndustrialDemandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CountCompanyDataSystem = this.World.GetOrCreateSystemManaged<CountCompanyDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<IndustrialCompanyData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ExtractorParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<ExtractorParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_IndustryQuery = this.GetEntityQuery(ComponentType.ReadWrite<IndustrialCompany>(), ComponentType.ReadWrite<CompanyData>(), ComponentType.ReadWrite<PropertySeeker>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<ServiceAvailable>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Created>(), ComponentType.Exclude<Game.Companies.ExtractorCompany>());
      // ISSUE: reference to a compiler-generated field
      this.m_ExtractorQuery = this.GetEntityQuery(ComponentType.ReadWrite<IndustrialCompany>(), ComponentType.ReadWrite<CompanyData>(), ComponentType.ReadWrite<PropertySeeker>(), ComponentType.ReadOnly<Game.Companies.ExtractorCompany>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<ServiceAvailable>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Created>());
      // ISSUE: reference to a compiler-generated field
      this.m_FreeExtractorQuery = this.GetEntityQuery(ComponentType.ReadWrite<PropertyOnMarket>(), ComponentType.ReadWrite<ExtractorProperty>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Abandoned>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Condemned>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_FreePropertyQuery = this.GetEntityQuery(ComponentType.ReadWrite<PropertyOnMarket>(), ComponentType.ReadWrite<IndustrialProperty>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<ExtractorProperty>(), ComponentType.Exclude<Abandoned>(), ComponentType.Exclude<Condemned>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ZonePreferenceQuery = this.GetEntityQuery(ComponentType.ReadOnly<ZonePreferenceData>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_IndustryQuery, this.m_ExtractorQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      JobHandle deps1 = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_IndustryQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Signature_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_CommercialCompany_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_LandValue_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_PropertyRenter_RW_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_PropertyOnMarket_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_CompanyData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Agents_PropertySeeker_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        JobHandle outJobHandle1;
        JobHandle outJobHandle2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.Dependency = new PropertyUtils.CompanyFindPropertyJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_PropertySeekerType = this.__TypeHandle.__Game_Agents_PropertySeeker_RW_ComponentTypeHandle,
          m_CompanyDataType = this.__TypeHandle.__Game_Companies_CompanyData_RW_ComponentTypeHandle,
          m_StorageCompanyType = this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentTypeHandle,
          m_FreePropertyEntities = this.m_FreePropertyQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle1),
          m_PropertyPrefabs = this.m_FreePropertyQuery.ToComponentDataListAsync<PrefabRef>((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle2),
          m_BuildingPropertyDatas = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
          m_IndustrialProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
          m_PrefabFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_PropertiesOnMarket = this.__TypeHandle.__Game_Buildings_PropertyOnMarket_RO_ComponentLookup,
          m_Availabilities = this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup,
          m_BuildingDatas = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
          m_Buildings = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
          m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RW_ComponentLookup,
          m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
          m_LandValues = this.__TypeHandle.__Game_Net_LandValue_RO_ComponentLookup,
          m_ServiceCompanies = this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup,
          m_SpawnableDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
          m_WorkplaceDatas = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup,
          m_CommercialCompanies = this.__TypeHandle.__Game_Companies_CommercialCompany_RO_ComponentLookup,
          m_Signatures = this.__TypeHandle.__Game_Buildings_Signature_RO_ComponentLookup,
          m_Renters = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
          m_EconomyParameters = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
          m_ZonePreferences = this.m_ZonePreferenceQuery.GetSingleton<ZonePreferenceData>(),
          m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
          m_Commercial = false,
          m_RentActionQueue = this.m_PropertyProcessingSystem.GetRentActionQueue(out deps1).AsParallelWriter(),
          m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
        }.ScheduleParallel<PropertyUtils.CompanyFindPropertyJob>(this.m_IndustryQuery, JobUtils.CombineDependencies(outJobHandle1, outJobHandle2, deps1, this.Dependency));
        // ISSUE: reference to a compiler-generated field
        this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PropertyProcessingSystem.AddWriter(this.Dependency);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Extractor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle3;
      JobHandle outJobHandle4;
      JobHandle outJobHandle5;
      JobHandle deps2;
      JobHandle deps3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.Dependency = new PropertyUtils.ExtractorFindCompanyJob()
      {
        m_Entities = this.m_FreeExtractorQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle3),
        m_ExtractorCompanyEntities = this.m_ExtractorQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle4),
        m_CompanyPrefabs = this.m_CompanyPrefabQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle5),
        m_Attached = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_ExtractorAreas = this.__TypeHandle.__Game_Areas_Extractor_RO_ComponentLookup,
        m_ExtractorDatas = this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup,
        m_Geometries = this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup,
        m_Lots = this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_Processes = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_Properties = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_WorkplaceDatas = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_Productions = this.m_CountCompanyDataSystem.GetProduction(out deps2),
        m_Consumptions = this.m_IndustrialDemandSystem.GetConsumption(out deps3),
        m_ExtractorParameters = this.m_ExtractorParameterQuery.GetSingleton<ExtractorParameterData>(),
        m_EconomyParameters = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_RentActionQueue = this.m_PropertyProcessingSystem.GetRentActionQueue(out deps1),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer(),
        m_AverageTemperature = this.m_ClimateSystem.averageTemperature
      }.Schedule<PropertyUtils.ExtractorFindCompanyJob>(JobUtils.CombineDependencies(this.Dependency, outJobHandle3, outJobHandle4, outJobHandle5, deps2, deps3, deps1));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IndustrialDemandSystem.AddReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CountCompanyDataSystem.AddReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PropertyProcessingSystem.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
    }

    public static float Evaluate(
      Entity company,
      Entity property,
      ref IndustrialProcessData process,
      ref PropertySeeker propertySeeker,
      ComponentLookup<Building> buildings,
      ComponentLookup<PropertyOnMarket> propertiesOnMarket,
      ComponentLookup<PrefabRef> prefabFromEntity,
      ComponentLookup<Game.Prefabs.BuildingData> buildingDatas,
      ComponentLookup<SpawnableBuildingData> spawnableDatas,
      ComponentLookup<WorkplaceData> workplaceDatas,
      ComponentLookup<LandValue> landValues,
      BufferLookup<ResourceAvailability> availabilities,
      EconomyParameterData economyParameters,
      ResourcePrefabs resourcePrefabs,
      ComponentLookup<ResourceData> resourceDatas,
      ComponentLookup<BuildingPropertyData> propertyDatas,
      bool storage)
    {
      if (!buildings.HasComponent(property) || !availabilities.HasBuffer(buildings[property].m_RoadEdge))
        return 0.0f;
      Building building = buildings[property];
      Entity prefab1 = prefabFromEntity[property].m_Prefab;
      Entity prefab2 = prefabFromEntity[company].m_Prefab;
      float num1 = 0.0f;
      float num2;
      if (storage)
      {
        DynamicBuffer<ResourceAvailability> availability = availabilities[building.m_RoadEdge];
        float weight = EconomyUtils.GetWeight(process.m_Output.m_Resource, resourcePrefabs, ref resourceDatas);
        num2 = num1 + 50f * weight * (float) process.m_Output.m_Amount * NetUtils.GetAvailability(availability, EconomyUtils.GetAvailableResourceSupply(process.m_Output.m_Resource), building.m_CurvePosition);
      }
      else
      {
        if (!workplaceDatas.HasComponent(prefab2))
          return -1f;
        float num3 = num1 + 500f;
        DynamicBuffer<ResourceAvailability> availability = availabilities[building.m_RoadEdge];
        num2 = num3 + 10f * NetUtils.GetAvailability(availability, AvailableResource.UneducatedCitizens, building.m_CurvePosition);
        if (process.m_Input1.m_Resource != Resource.NoResource)
        {
          float weight = EconomyUtils.GetWeight(process.m_Input1.m_Resource, resourcePrefabs, ref resourceDatas);
          num2 += 50f * weight * (float) process.m_Input1.m_Amount * NetUtils.GetAvailability(availability, EconomyUtils.GetAvailableResourceSupply(process.m_Input1.m_Resource), building.m_CurvePosition);
        }
        if (process.m_Input2.m_Resource != Resource.NoResource)
        {
          float weight = EconomyUtils.GetWeight(process.m_Input2.m_Resource, resourcePrefabs, ref resourceDatas);
          num2 += 50f * weight * (float) process.m_Input2.m_Amount * NetUtils.GetAvailability(availability, EconomyUtils.GetAvailableResourceSupply(process.m_Input2.m_Resource), building.m_CurvePosition);
        }
      }
      float landValue = landValues[building.m_RoadEdge].m_LandValue;
      float num4 = 1f;
      int num5 = 1;
      SpawnableBuildingData componentData;
      if (spawnableDatas.TryGetComponent(property, out componentData))
        num5 = (int) componentData.m_Level;
      float num6 = propertyDatas[prefab1].m_SpaceMultiplier * (float) (1.0 + 0.5 * (double) num5);
      if (!storage)
      {
        num4 = num6 * process.m_MaxWorkersPerCell;
        if ((double) EconomyUtils.GetWeight(process.m_Output.m_Resource, resourcePrefabs, ref resourceDatas) == 0.0)
          num4 *= 3f;
      }
      return 250f + (num2 - landValue / num4);
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

    [Preserve]
    public IndustrialFindPropertySystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<PropertySeeker> __Game_Agents_PropertySeeker_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CompanyData> __Game_Companies_CompanyData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Companies.StorageCompany> __Game_Companies_StorageCompany_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyOnMarket> __Game_Buildings_PropertyOnMarket_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ResourceAvailability> __Game_Net_ResourceAvailability_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LandValue> __Game_Net_LandValue_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> __Game_Companies_ServiceCompanyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> __Game_Prefabs_WorkplaceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CommercialCompany> __Game_Companies_CommercialCompany_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Signature> __Game_Buildings_Signature_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Extractor> __Game_Areas_Extractor_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ExtractorAreaData> __Game_Prefabs_ExtractorAreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Geometry> __Game_Areas_Geometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> __Game_Areas_Lot_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_PropertySeeker_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PropertySeeker>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CompanyData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CompanyData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageCompany_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Companies.StorageCompany>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyOnMarket_RO_ComponentLookup = state.GetComponentLookup<PropertyOnMarket>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ResourceAvailability_RO_BufferLookup = state.GetBufferLookup<ResourceAvailability>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RW_ComponentLookup = state.GetComponentLookup<PropertyRenter>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LandValue_RO_ComponentLookup = state.GetComponentLookup<LandValue>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceCompanyData_RO_ComponentLookup = state.GetComponentLookup<ServiceCompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkplaceData_RO_ComponentLookup = state.GetComponentLookup<WorkplaceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CommercialCompany_RO_ComponentLookup = state.GetComponentLookup<CommercialCompany>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Signature_RO_ComponentLookup = state.GetComponentLookup<Signature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Extractor_RO_ComponentLookup = state.GetComponentLookup<Extractor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup = state.GetComponentLookup<ExtractorAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Geometry_RO_ComponentLookup = state.GetComponentLookup<Geometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Lot_RO_ComponentLookup = state.GetComponentLookup<Game.Areas.Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
      }
    }
  }
}
