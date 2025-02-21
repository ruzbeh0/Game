// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PropertyProcessingSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using Game.Triggers;
using System.Runtime.CompilerServices;
using Unity.Assertions;
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
  public class PropertyProcessingSystem : GameSystemBase
  {
    private EntityQuery m_CommercialCompanyPrefabQuery;
    private EntityQuery m_IndustrialCompanyPrefabQuery;
    private EntityQuery m_PropertyGroupQuery;
    private EntityQuery m_EconomyParameterQuery;
    private EndFrameBarrier m_EndFrameBarrier;
    private TriggerSystem m_TriggerSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private ResourceSystem m_ResourceSystem;
    private EntityArchetype m_RentEventArchetype;
    private EntityArchetype m_MovedEventArchetype;
    private NativeQueue<RentAction> m_RentActionQueue;
    private NativeList<Entity> m_ReservedProperties;
    private JobHandle m_Writers;
    private PropertyProcessingSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RentActionQueue = new NativeQueue<RentAction>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ReservedProperties = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RentEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<RentersUpdated>());
      // ISSUE: reference to a compiler-generated field
      this.m_MovedEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<PathTargetMoved>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PropertyGroupQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadWrite<PropertyToBeOnMarket>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<CommercialProperty>(),
          ComponentType.ReadOnly<ResidentialProperty>(),
          ComponentType.ReadOnly<IndustrialProperty>(),
          ComponentType.ReadOnly<ExtractorProperty>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialCompanyPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Prefabs.ArchetypeData>(), ComponentType.ReadOnly<CommercialCompanyData>(), ComponentType.ReadOnly<IndustrialProcessData>());
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialCompanyPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Prefabs.ArchetypeData>(), ComponentType.ReadOnly<IndustrialCompanyData>(), ComponentType.ReadOnly<IndustrialProcessData>(), ComponentType.Exclude<StorageCompanyData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
    }

    public NativeQueue<RentAction> GetRentActionQueue(out JobHandle deps)
    {
      Assert.IsTrue(this.Enabled, "Can not write to queue when system isn't running");
      // ISSUE: reference to a compiler-generated field
      deps = this.m_Writers;
      // ISSUE: reference to a compiler-generated field
      return this.m_RentActionQueue;
    }

    public void AddWriter(JobHandle writer)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Writers = JobHandle.CombineDependencies(this.m_Writers, writer);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_RentActionQueue.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ReservedProperties.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PropertyGroupQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_LandValue_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_PropertyOnMarket_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ArchetypeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        PropertyProcessingSystem.PutPropertyOnMarketJob jobData = new PropertyProcessingSystem.PutPropertyOnMarketJob()
        {
          m_CommercialCompanyPrefabs = this.m_CommercialCompanyPrefabQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle1),
          m_IndustrialCompanyPrefabs = this.m_IndustrialCompanyPrefabQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle2),
          m_Archetypes = this.__TypeHandle.__Game_Prefabs_ArchetypeData_RO_ComponentLookup,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
          m_BuildingPropertyDatas = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
          m_ConsumptionDatas = this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup,
          m_PropertyOnMarkets = this.__TypeHandle.__Game_Buildings_PropertyOnMarket_RO_ComponentLookup,
          m_LandValues = this.__TypeHandle.__Game_Net_LandValue_RO_ComponentLookup,
          m_SpawnableBuildingDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
          m_IndustrialProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
          m_BuildingDatas = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
          m_ZoneDatas = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup,
          m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
          m_Lots = this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup,
          m_Geometries = this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup,
          m_Attacheds = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
          m_EconomyParameterData = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
          m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        this.Dependency = jobData.ScheduleParallel<PropertyProcessingSystem.PutPropertyOnMarketJob>(this.m_PropertyGroupQuery, JobHandle.CombineDependencies(outJobHandle1, outJobHandle2, this.Dependency));
        // ISSUE: reference to a compiler-generated field
        this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ExtractorCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Park_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HomelessHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CommercialCompany_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_IndustrialCompany_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyOnMarket_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PropertyProcessingSystem.PropertyRentJob jobData1 = new PropertyProcessingSystem.PropertyRentJob()
      {
        m_RentEventArchetype = this.m_RentEventArchetype,
        m_MovedEventArchetype = this.m_MovedEventArchetype,
        m_PropertiesOnMarket = this.__TypeHandle.__Game_Buildings_PropertyOnMarket_RO_ComponentLookup,
        m_Renters = this.__TypeHandle.__Game_Buildings_Renter_RW_BufferLookup,
        m_BuildingPropertyDatas = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_ParkDatas = this.__TypeHandle.__Game_Prefabs_ParkData_RO_ComponentLookup,
        m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_Companies = this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup,
        m_Households = this.__TypeHandle.__Game_Citizens_Household_RW_ComponentLookup,
        m_Industrials = this.__TypeHandle.__Game_Companies_IndustrialCompany_RO_ComponentLookup,
        m_Commercials = this.__TypeHandle.__Game_Companies_CommercialCompany_RO_ComponentLookup,
        m_TriggerQueue = this.m_TriggerSystem.CreateActionBuffer(),
        m_BuildingDatas = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_ServiceCompanyDatas = this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup,
        m_IndustrialProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_WorkProviders = this.__TypeHandle.__Game_Companies_WorkProvider_RW_ComponentLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_Abandoneds = this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup,
        m_HomelessHouseholds = this.__TypeHandle.__Game_Citizens_HomelessHousehold_RO_ComponentLookup,
        m_Parks = this.__TypeHandle.__Game_Buildings_Park_RO_ComponentLookup,
        m_Employees = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup,
        m_SpawnableBuildingDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_Attacheds = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_ExtractorCompanyDatas = this.__TypeHandle.__Game_Prefabs_ExtractorCompanyData_RO_ComponentLookup,
        m_SubAreaBufs = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_Geometries = this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup,
        m_Lots = this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_Resources = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_ZoneDatas = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup,
        m_StatisticsQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps),
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer(),
        m_RentActionQueue = this.m_RentActionQueue,
        m_ReservedProperties = this.m_ReservedProperties,
        m_DebugDisableHomeless = false
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.Schedule<PropertyProcessingSystem.PropertyRentJob>(JobHandle.CombineDependencies(this.Dependency, this.m_Writers, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
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
    public PropertyProcessingSystem()
    {
    }

    [BurstCompile]
    public struct PutPropertyOnMarketJob : IJobChunk
    {
      [ReadOnly]
      public NativeList<Entity> m_CommercialCompanyPrefabs;
      [ReadOnly]
      public NativeList<Entity> m_IndustrialCompanyPrefabs;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.ArchetypeData> m_Archetypes;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDatas;
      [ReadOnly]
      public ComponentLookup<ConsumptionData> m_ConsumptionDatas;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyDatas;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDatas;
      [ReadOnly]
      public ComponentLookup<ZoneData> m_ZoneDatas;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> m_Lots;
      [ReadOnly]
      public ComponentLookup<Geometry> m_Geometries;
      [ReadOnly]
      public ComponentLookup<Attached> m_Attacheds;
      [ReadOnly]
      public ComponentLookup<LandValue> m_LandValues;
      [ReadOnly]
      public ComponentLookup<PropertyOnMarket> m_PropertyOnMarkets;
      [ReadOnly]
      public EconomyParameterData m_EconomyParameterData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Building> nativeArray3 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity prefab = nativeArray2[index1].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          BuildingPropertyData buildingPropertyData1 = this.m_BuildingPropertyDatas[prefab];
          // ISSUE: reference to a compiler-generated field
          Game.Prefabs.BuildingData buildingData = this.m_BuildingDatas[prefab];
          // ISSUE: reference to a compiler-generated field
          ConsumptionData consumptionData = this.m_ConsumptionDatas[prefab];
          Entity roadEdge = nativeArray3[index1].m_RoadEdge;
          float num1 = 0.0f;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LandValues.HasComponent(roadEdge))
          {
            // ISSUE: reference to a compiler-generated field
            num1 = this.m_LandValues[roadEdge].m_LandValue;
          }
          Game.Zones.AreaType areaType = Game.Zones.AreaType.None;
          // ISSUE: reference to a compiler-generated field
          int buildingLevel1 = PropertyUtils.GetBuildingLevel(prefab, this.m_SpawnableBuildingDatas);
          // ISSUE: reference to a compiler-generated field
          if (this.m_SpawnableBuildingDatas.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            areaType = this.m_ZoneDatas[this.m_SpawnableBuildingDatas[prefab].m_ZonePrefab].m_AreaType;
          }
          int num2 = buildingData.m_LotSize.x * buildingData.m_LotSize.y;
          Attached componentData;
          DynamicBuffer<Game.Areas.SubArea> bufferData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Attacheds.TryGetComponent(nativeArray1[index1], out componentData) && this.m_SubAreas.TryGetBuffer(componentData.m_Parent, out bufferData))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            float area = ExtractorAISystem.GetArea(bufferData, this.m_Lots, this.m_Geometries);
            num2 += Mathf.RoundToInt(area / 64f);
          }
          BuildingPropertyData buildingPropertyData2 = buildingPropertyData1;
          int buildingLevel2 = buildingLevel1;
          int lotSize = num2;
          double landValueBase = (double) num1;
          int num3 = (int) areaType;
          // ISSUE: reference to a compiler-generated field
          ref EconomyParameterData local = ref this.m_EconomyParameterData;
          int rentPricePerRenter = PropertyUtils.GetRentPricePerRenter(consumptionData, buildingPropertyData2, buildingLevel2, lotSize, (float) landValueBase, (Game.Zones.AreaType) num3, ref local);
          if (chunk.Has<Signature>())
          {
            if (buildingPropertyData1.m_AllowedSold != Resource.NoResource)
            {
              // ISSUE: reference to a compiler-generated field
              for (int index2 = 0; index2 < this.m_CommercialCompanyPrefabs.Length; ++index2)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Resource resource = this.m_IndustrialProcessDatas[this.m_CommercialCompanyPrefabs[index2]].m_Output.m_Resource;
                if (buildingPropertyData1.m_AllowedSold == resource)
                {
                  // ISSUE: reference to a compiler-generated field
                  Entity commercialCompanyPrefab = this.m_CommercialCompanyPrefabs[index2];
                  // ISSUE: reference to a compiler-generated field
                  Game.Prefabs.ArchetypeData archetype = this.m_Archetypes[commercialCompanyPrefab];
                  // ISSUE: reference to a compiler-generated field
                  Entity entity = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, archetype.m_Archetype);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<PrefabRef>(unfilteredChunkIndex, entity, new PrefabRef()
                  {
                    m_Prefab = commercialCompanyPrefab
                  });
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<PropertyRenter>(unfilteredChunkIndex, entity, new PropertyRenter()
                  {
                    m_Property = nativeArray1[index1]
                  });
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<PropertyToBeOnMarket>(unfilteredChunkIndex, nativeArray1[index1]);
                  return;
                }
              }
            }
            else if (buildingPropertyData1.m_AllowedManufactured != Resource.NoResource)
            {
              // ISSUE: reference to a compiler-generated field
              for (int index3 = 0; index3 < this.m_IndustrialCompanyPrefabs.Length; ++index3)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Resource resource = this.m_IndustrialProcessDatas[this.m_IndustrialCompanyPrefabs[index3]].m_Output.m_Resource;
                if (buildingPropertyData1.m_AllowedManufactured == resource)
                {
                  // ISSUE: reference to a compiler-generated field
                  Entity industrialCompanyPrefab = this.m_IndustrialCompanyPrefabs[index3];
                  // ISSUE: reference to a compiler-generated field
                  Game.Prefabs.ArchetypeData archetype = this.m_Archetypes[industrialCompanyPrefab];
                  // ISSUE: reference to a compiler-generated field
                  Entity entity = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, archetype.m_Archetype);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<PrefabRef>(unfilteredChunkIndex, entity, new PrefabRef()
                  {
                    m_Prefab = industrialCompanyPrefab
                  });
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<PropertyRenter>(unfilteredChunkIndex, entity, new PropertyRenter()
                  {
                    m_Property = nativeArray1[index1]
                  });
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<PropertyToBeOnMarket>(unfilteredChunkIndex, nativeArray1[index1]);
                  return;
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PropertyOnMarkets.HasComponent(nativeArray1[index1]))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<PropertyToBeOnMarket>(unfilteredChunkIndex, nativeArray1[index1]);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<PropertyOnMarket>(unfilteredChunkIndex, nativeArray1[index1], new PropertyOnMarket()
            {
              m_AskingRent = rentPricePerRenter
            });
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<PropertyOnMarket>(unfilteredChunkIndex, nativeArray1[index1]);
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

    [BurstCompile]
    public struct PropertyRentJob : IJob
    {
      [ReadOnly]
      public EntityArchetype m_RentEventArchetype;
      [ReadOnly]
      public EntityArchetype m_MovedEventArchetype;
      public ComponentLookup<WorkProvider> m_WorkProviders;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDatas;
      [ReadOnly]
      public ComponentLookup<ParkData> m_ParkDatas;
      [ReadOnly]
      public ComponentLookup<PropertyOnMarket> m_PropertiesOnMarket;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyDatas;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDatas;
      [ReadOnly]
      public ComponentLookup<ZoneData> m_ZoneDatas;
      [ReadOnly]
      public ComponentLookup<CompanyData> m_Companies;
      [ReadOnly]
      public ComponentLookup<CommercialCompany> m_Commercials;
      [ReadOnly]
      public ComponentLookup<IndustrialCompany> m_Industrials;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> m_ServiceCompanyDatas;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public ComponentLookup<Abandoned> m_Abandoneds;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Park> m_Parks;
      [ReadOnly]
      public ComponentLookup<HomelessHousehold> m_HomelessHouseholds;
      [ReadOnly]
      public BufferLookup<Employee> m_Employees;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreaBufs;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> m_Lots;
      [ReadOnly]
      public ComponentLookup<Geometry> m_Geometries;
      [ReadOnly]
      public ComponentLookup<Attached> m_Attacheds;
      [ReadOnly]
      public ComponentLookup<ExtractorCompanyData> m_ExtractorCompanyDatas;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_Resources;
      public ComponentLookup<Household> m_Households;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      public BufferLookup<Renter> m_Renters;
      public EntityCommandBuffer m_CommandBuffer;
      public NativeQueue<RentAction> m_RentActionQueue;
      public NativeList<Entity> m_ReservedProperties;
      public NativeQueue<TriggerAction> m_TriggerQueue;
      public NativeQueue<StatisticsEvent> m_StatisticsQueue;
      public bool m_DebugDisableHomeless;

      public void Execute()
      {
        RentAction rentAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_RentActionQueue.TryDequeue(out rentAction))
        {
          Entity property = rentAction.m_Property;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Renters.HasBuffer(property) && this.m_PrefabRefs.HasComponent(rentAction.m_Renter))
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_ReservedProperties.Contains<Entity, Entity>(property))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Renter> renter1 = this.m_Renters[property];
              // ISSUE: reference to a compiler-generated field
              Entity prefab = this.m_PrefabRefs[property].m_Prefab;
              int num1 = 0;
              bool flag1 = false;
              bool flag2 = false;
              bool flag3 = false;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Zones.AreaType areaType = BuildingUtils.GetAreaType(prefab, ref this.m_SpawnableBuildingDatas, ref this.m_ZoneDatas);
              // ISSUE: reference to a compiler-generated field
              if (this.m_BuildingPropertyDatas.HasComponent(prefab))
              {
                // ISSUE: reference to a compiler-generated field
                BuildingPropertyData buildingPropertyData = this.m_BuildingPropertyDatas[prefab];
                // ISSUE: reference to a compiler-generated field
                bool flag4 = PropertyUtils.IsMixedBuilding(prefab, ref this.m_BuildingPropertyDatas);
                if (areaType == Game.Zones.AreaType.Residential)
                {
                  num1 = buildingPropertyData.CountProperties(Game.Zones.AreaType.Residential);
                  if (flag4)
                    flag1 = true;
                }
                else
                  flag1 = true;
                for (int index = 0; index < renter1.Length; ++index)
                {
                  Entity renter2 = renter1[index].m_Renter;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_Households.HasComponent(renter2))
                  {
                    --num1;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_Companies.HasComponent(renter2))
                      flag2 = true;
                  }
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_BuildingDatas.HasComponent(prefab) && BuildingUtils.IsHomelessShelterBuilding(property, ref this.m_Parks, ref this.m_Abandoneds))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  num1 = HomelessShelterAISystem.GetShelterCapacity(this.m_BuildingDatas[prefab], this.m_BuildingPropertyDatas.HasComponent(prefab) ? this.m_BuildingPropertyDatas[prefab] : new BuildingPropertyData()) - this.m_Renters[property].Length;
                  flag3 = true;
                }
              }
              // ISSUE: reference to a compiler-generated field
              bool flag5 = this.m_Companies.HasComponent(rentAction.m_Renter);
              if (!flag5 && num1 > 0 || flag5 & flag1 && !flag2)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity propertyFromRenter = BuildingUtils.GetPropertyFromRenter(rentAction.m_Renter, ref this.m_HomelessHouseholds, ref this.m_PropertyRenters);
                Renter renter3;
                if (propertyFromRenter != Entity.Null && propertyFromRenter != property)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_WorkProviders.HasComponent(rentAction.m_Renter) || !this.m_Employees.HasBuffer(rentAction.m_Renter) || this.m_WorkProviders[rentAction.m_Renter].m_MaxWorkers >= this.m_Employees[rentAction.m_Renter].Length)
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_Renters.HasBuffer(propertyFromRenter))
                    {
                      // ISSUE: reference to a compiler-generated field
                      DynamicBuffer<Renter> renter4 = this.m_Renters[propertyFromRenter];
                      for (int index = 0; index < renter4.Length; ++index)
                      {
                        renter3 = renter4[index];
                        if (renter3.m_Renter.Equals(rentAction.m_Renter))
                        {
                          renter4.RemoveAt(index);
                          break;
                        }
                      }
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.SetComponent<RentersUpdated>(this.m_CommandBuffer.CreateEntity(this.m_RentEventArchetype), new RentersUpdated(propertyFromRenter));
                    }
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_PrefabRefs.HasComponent(propertyFromRenter) && !this.m_PropertiesOnMarket.HasComponent(propertyFromRenter))
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddComponent<PropertyToBeOnMarket>(propertyFromRenter, new PropertyToBeOnMarket());
                    }
                  }
                  else
                    continue;
                }
                if (!flag3)
                {
                  if (property == Entity.Null)
                    UnityEngine.Debug.LogWarning((object) "trying to rent null property");
                  int num2 = 0;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PropertiesOnMarket.HasComponent(property))
                  {
                    // ISSUE: reference to a compiler-generated field
                    num2 = this.m_PropertiesOnMarket[property].m_AskingRent;
                  }
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<PropertyRenter>(rentAction.m_Renter, new PropertyRenter()
                  {
                    m_Property = property,
                    m_Rent = num2
                  });
                }
                ref DynamicBuffer<Renter> local = ref renter1;
                renter3 = new Renter();
                renter3.m_Renter = rentAction.m_Renter;
                Renter elem = renter3;
                local.Add(elem);
                PrefabRef componentData;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (flag5 && this.m_PrefabRefs.TryGetComponent(rentAction.m_Renter, out componentData) && this.m_Companies[rentAction.m_Renter].m_Brand != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_TriggerQueue.Enqueue(new TriggerAction()
                  {
                    m_PrimaryTarget = rentAction.m_Renter,
                    m_SecondaryTarget = rentAction.m_Property,
                    m_TriggerPrefab = componentData.m_Prefab,
                    m_TriggerType = TriggerType.BrandRented
                  });
                }
                // ISSUE: reference to a compiler-generated field
                if (this.m_WorkProviders.HasComponent(rentAction.m_Renter))
                {
                  Entity renter5 = rentAction.m_Renter;
                  // ISSUE: reference to a compiler-generated field
                  WorkProvider workProvider = this.m_WorkProviders[renter5];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  int maxFittingWorkers = CompanyUtils.GetCompanyMaxFittingWorkers(rentAction.m_Renter, rentAction.m_Property, ref this.m_PrefabRefs, ref this.m_ServiceCompanyDatas, ref this.m_BuildingDatas, ref this.m_BuildingPropertyDatas, ref this.m_SpawnableBuildingDatas, ref this.m_IndustrialProcessDatas, ref this.m_ExtractorCompanyDatas, ref this.m_Attacheds, ref this.m_SubAreaBufs, ref this.m_Lots, ref this.m_Geometries);
                  workProvider.m_MaxWorkers = math.max(math.min(workProvider.m_MaxWorkers, maxFittingWorkers), 2 * maxFittingWorkers / 3);
                  // ISSUE: reference to a compiler-generated field
                  this.m_WorkProviders[renter5] = workProvider;
                }
                // ISSUE: reference to a compiler-generated field
                if (this.m_HouseholdCitizens.HasBuffer(rentAction.m_Renter))
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<HouseholdCitizen> householdCitizen1 = this.m_HouseholdCitizens[rentAction.m_Renter];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_BuildingPropertyDatas.HasComponent(prefab) && this.m_HomelessHouseholds.HasComponent(rentAction.m_Renter) && !flag3)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.RemoveComponent<HomelessHousehold>(rentAction.m_Renter);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (!this.m_DebugDisableHomeless & flag3)
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddComponent<HomelessHousehold>(rentAction.m_Renter, new HomelessHousehold()
                      {
                        m_TempHome = property
                      });
                      // ISSUE: reference to a compiler-generated field
                      Household household = this.m_Households[rentAction.m_Renter] with
                      {
                        m_Resources = 0
                      };
                      // ISSUE: reference to a compiler-generated field
                      this.m_Households[rentAction.m_Renter] = household;
                    }
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_BuildingPropertyDatas.HasComponent(prefab) && this.m_PropertyRenters.HasComponent(rentAction.m_Renter))
                  {
                    foreach (HouseholdCitizen householdCitizen2 in householdCitizen1)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.m_TriggerQueue.Enqueue(new TriggerAction(TriggerType.CitizenMovedHouse, Entity.Null, householdCitizen2.m_Citizen, this.m_PropertyRenters[rentAction.m_Renter].m_Property));
                    }
                  }
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_BuildingPropertyDatas.HasComponent(prefab) && renter1.Length >= this.m_BuildingPropertyDatas[prefab].CountProperties())
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ReservedProperties.Add(in property);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<PropertyOnMarket>(property);
                }
                else if (flag3 && num1 <= 1)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ReservedProperties.Add(in property);
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<RentersUpdated>(this.m_CommandBuffer.CreateEntity(this.m_RentEventArchetype), new RentersUpdated(property));
                // ISSUE: reference to a compiler-generated field
                if (this.m_MovedEventArchetype.Valid)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<PathTargetMoved>(this.m_CommandBuffer.CreateEntity(this.m_MovedEventArchetype), new PathTargetMoved(rentAction.m_Renter, new float3(), new float3()));
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_BuildingPropertyDatas.HasComponent(prefab) && renter1.Length >= this.m_BuildingPropertyDatas[prefab].CountProperties())
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<PropertyOnMarket>(property);
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<PropertySeeker>(rentAction.m_Renter);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ReservedProperties.Clear();
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.ArchetypeData> __Game_Prefabs_ArchetypeData_RO_ComponentLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ConsumptionData> __Game_Prefabs_ConsumptionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyOnMarket> __Game_Buildings_PropertyOnMarket_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LandValue> __Game_Net_LandValue_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> __Game_Areas_Lot_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Geometry> __Game_Areas_Geometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      public BufferLookup<Renter> __Game_Buildings_Renter_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ParkData> __Game_Prefabs_ParkData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CompanyData> __Game_Companies_CompanyData_RO_ComponentLookup;
      public ComponentLookup<Household> __Game_Citizens_Household_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialCompany> __Game_Companies_IndustrialCompany_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CommercialCompany> __Game_Companies_CommercialCompany_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> __Game_Companies_ServiceCompanyData_RO_ComponentLookup;
      public ComponentLookup<WorkProvider> __Game_Companies_WorkProvider_RW_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Abandoned> __Game_Buildings_Abandoned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HomelessHousehold> __Game_Citizens_HomelessHousehold_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Park> __Game_Buildings_Park_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Employee> __Game_Companies_Employee_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ExtractorCompanyData> __Game_Prefabs_ExtractorCompanyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ArchetypeData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.ArchetypeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ConsumptionData_RO_ComponentLookup = state.GetComponentLookup<ConsumptionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyOnMarket_RO_ComponentLookup = state.GetComponentLookup<PropertyOnMarket>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LandValue_RO_ComponentLookup = state.GetComponentLookup<LandValue>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentLookup = state.GetComponentLookup<ZoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Lot_RO_ComponentLookup = state.GetComponentLookup<Game.Areas.Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Geometry_RO_ComponentLookup = state.GetComponentLookup<Geometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RW_BufferLookup = state.GetBufferLookup<Renter>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkData_RO_ComponentLookup = state.GetComponentLookup<ParkData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CompanyData_RO_ComponentLookup = state.GetComponentLookup<CompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RW_ComponentLookup = state.GetComponentLookup<Household>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_IndustrialCompany_RO_ComponentLookup = state.GetComponentLookup<IndustrialCompany>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CommercialCompany_RO_ComponentLookup = state.GetComponentLookup<CommercialCompany>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceCompanyData_RO_ComponentLookup = state.GetComponentLookup<ServiceCompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RW_ComponentLookup = state.GetComponentLookup<WorkProvider>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Abandoned_RO_ComponentLookup = state.GetComponentLookup<Abandoned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HomelessHousehold_RO_ComponentLookup = state.GetComponentLookup<HomelessHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Park_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Park>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferLookup = state.GetBufferLookup<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ExtractorCompanyData_RO_ComponentLookup = state.GetComponentLookup<ExtractorCompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
      }
    }
  }
}
