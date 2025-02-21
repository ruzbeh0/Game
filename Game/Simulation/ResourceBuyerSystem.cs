// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ResourceBuyerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

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
using Game.Routes;
using Game.Tools;
using Game.Vehicles;
using System;
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
  public class ResourceBuyerSystem : GameSystemBase
  {
    private const int UPDATE_INTERVAL = 16;
    private EntityQuery m_BuyerQuery;
    private EntityQuery m_CarPrefabQuery;
    private EntityQuery m_EconomyParameterQuery;
    private EntityQuery m_ResidentPrefabQuery;
    private EntityQuery m_PopulationQuery;
    private ComponentTypeSet m_PathfindTypes;
    private EndFrameBarrier m_EndFrameBarrier;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private ResourceSystem m_ResourceSystem;
    private TaxSystem m_TaxSystem;
    private TimeSystem m_TimeSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private PersonalCarSelectData m_PersonalCarSelectData;
    private CitySystem m_CitySystem;
    private NativeQueue<ResourceBuyerSystem.SalesEvent> m_SalesQueue;
    private ResourceBuyerSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem = this.World.GetOrCreateSystemManaged<TaxSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PersonalCarSelectData = new PersonalCarSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SalesQueue = new NativeQueue<ResourceBuyerSystem.SalesEvent>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BuyerQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadWrite<ResourceBuyer>(),
          ComponentType.ReadWrite<TripNeeded>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<TravelPurpose>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<ResourceBought>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_CarPrefabQuery = this.GetEntityQuery(PersonalCarSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PopulationQuery = this.GetEntityQuery(ComponentType.ReadOnly<Population>());
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<ObjectData>(), ComponentType.ReadOnly<HumanData>(), ComponentType.ReadOnly<ResidentData>(), ComponentType.ReadOnly<PrefabData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindTypes = new ComponentTypeSet(ComponentType.ReadWrite<PathInformation>(), ComponentType.ReadWrite<PathElement>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BuyerQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PopulationQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SalesQueue.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStopRunning() => base.OnStopRunning();

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_BuyerQuery.CalculateEntityCount() <= 0)
        return;
      JobHandle jobHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PersonalCarSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_CarPrefabQuery, Allocator.TempJob, out jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_Population_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CoordinatedMeeting_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HumanData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CommuterHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PersonalCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CarKeeper_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_AttendingMeeting_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResidentData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CreatureData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_ResourceBought_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ResourceBuyer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ResourceBuyerSystem.HandleBuyersJob jobData1 = new ResourceBuyerSystem.HandleBuyersJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_BuyerType = this.__TypeHandle.__Game_Companies_ResourceBuyer_RO_ComponentTypeHandle,
        m_BoughtType = this.__TypeHandle.__Game_Citizens_ResourceBought_RO_ComponentTypeHandle,
        m_TripType = this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle,
        m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle,
        m_CreatureDataType = this.__TypeHandle.__Game_Prefabs_CreatureData_RO_ComponentTypeHandle,
        m_ResidentDataType = this.__TypeHandle.__Game_Prefabs_ResidentData_RO_ComponentTypeHandle,
        m_AttendingMeetingType = this.__TypeHandle.__Game_Citizens_AttendingMeeting_RO_ComponentTypeHandle,
        m_ServiceAvailables = this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentLookup,
        m_PathInformation = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_Properties = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_CarKeepers = this.__TypeHandle.__Game_Citizens_CarKeeper_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_PersonalCarData = this.__TypeHandle.__Game_Vehicles_PersonalCar_RO_ComponentLookup,
        m_Targets = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
        m_CurrentBuildings = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup,
        m_OutsideConnections = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_HouseholdMembers = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_Households = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_TouristHouseholds = this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup,
        m_CommuterHouseholds = this.__TypeHandle.__Game_Citizens_CommuterHousehold_RO_ComponentLookup,
        m_ServiceCompanyDatas = this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup,
        m_Workers = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup,
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabHumanData = this.__TypeHandle.__Game_Prefabs_HumanData_RO_ComponentLookup,
        m_CoordinatedMeetings = this.__TypeHandle.__Game_Citizens_CoordinatedMeeting_RW_ComponentLookup,
        m_HaveCoordinatedMeetingDatas = this.__TypeHandle.__Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup,
        m_Populations = this.__TypeHandle.__Game_City_Population_RW_ComponentLookup,
        m_TimeOfDay = this.m_TimeSystem.normalizedTime,
        m_RandomSeed = RandomSeed.Next(),
        m_PathfindTypes = this.m_PathfindTypes,
        m_HumanChunks = this.m_ResidentPrefabQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 80, 16).AsParallelWriter(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_EconomyParameterData = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_City = this.m_CitySystem.City,
        m_SalesQueue = this.m_SalesQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<ResourceBuyerSystem.HandleBuyersJob>(this.m_BuyerQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_Population_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_TradeCost_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_BuyingCompany_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ServiceAvailable_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ResourceBuyerSystem.BuyJob jobData2 = new ResourceBuyerSystem.BuyJob()
      {
        m_EconomyParameters = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup,
        m_SalesQueue = this.m_SalesQueue,
        m_Services = this.__TypeHandle.__Game_Companies_ServiceAvailable_RW_ComponentLookup,
        m_TransformDatas = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_OwnedVehicles = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_HouseholdAnimals = this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RO_BufferLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ServiceCompanies = this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup,
        m_Storages = this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup,
        m_Households = this.__TypeHandle.__Game_Citizens_Household_RW_ComponentLookup,
        m_BuyingCompanies = this.__TypeHandle.__Game_Companies_BuyingCompany_RW_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_TradeCosts = this.__TypeHandle.__Game_Companies_TradeCost_RW_BufferLookup,
        m_OutsideConnections = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_RandomSeed = RandomSeed.Next(),
        m_PersonalCarSelectData = this.m_PersonalCarSelectData,
        m_TaxRates = this.m_TaxSystem.GetTaxRates(),
        m_Districts = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup,
        m_DistrictModifiers = this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup,
        m_PopulationData = this.__TypeHandle.__Game_City_Population_RO_ComponentLookup,
        m_PopulationEntity = this.m_PopulationQuery.GetSingletonEntity(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer()
      };
      this.Dependency = jobData2.Schedule<ResourceBuyerSystem.BuyJob>(JobHandle.CombineDependencies(this.Dependency, jobHandle));
      // ISSUE: reference to a compiler-generated field
      this.m_PersonalCarSelectData.PostUpdate(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TaxSystem.AddReader(this.Dependency);
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
    public ResourceBuyerSystem()
    {
    }

    [Flags]
    private enum SaleFlags : byte
    {
      None = 0,
      CommercialSeller = 1,
      ImportFromOC = 2,
      Virtual = 4,
    }

    private struct SalesEvent
    {
      public ResourceBuyerSystem.SaleFlags m_Flags;
      public Entity m_Buyer;
      public Entity m_Seller;
      public Resource m_Resource;
      public int m_Amount;
      public float m_Distance;
    }

    [BurstCompile]
    private struct BuyJob : IJob
    {
      public NativeQueue<ResourceBuyerSystem.SalesEvent> m_SalesQueue;
      public EconomyParameterData m_EconomyParameters;
      public BufferLookup<Game.Economy.Resources> m_Resources;
      public ComponentLookup<ServiceAvailable> m_Services;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Household> m_Households;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<BuyingCompany> m_BuyingCompanies;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformDatas;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> m_ServiceCompanies;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> m_OwnedVehicles;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public BufferLookup<HouseholdAnimal> m_HouseholdAnimals;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> m_Storages;
      public BufferLookup<TradeCost> m_TradeCosts;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public PersonalCarSelectData m_PersonalCarSelectData;
      [ReadOnly]
      public NativeArray<int> m_TaxRates;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_Districts;
      [ReadOnly]
      public BufferLookup<DistrictModifier> m_DistrictModifiers;
      [ReadOnly]
      public ComponentLookup<Population> m_PopulationData;
      public Entity m_PopulationEntity;
      public RandomSeed m_RandomSeed;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(0);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Population population = this.m_PopulationData[this.m_PopulationEntity];
        // ISSUE: variable of a compiler-generated type
        ResourceBuyerSystem.SalesEvent salesEvent;
        // ISSUE: reference to a compiler-generated field
        while (this.m_SalesQueue.TryDequeue(out salesEvent))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Resources.HasBuffer(salesEvent.m_Buyer))
          {
            // ISSUE: reference to a compiler-generated field
            bool flag = (salesEvent.m_Flags & ResourceBuyerSystem.SaleFlags.CommercialSeller) != 0;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float f = (flag ? EconomyUtils.GetMarketPrice(salesEvent.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas) : EconomyUtils.GetIndustrialPrice(salesEvent.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas)) * (float) salesEvent.m_Amount;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_TradeCosts.HasBuffer(salesEvent.m_Seller))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<TradeCost> tradeCost1 = this.m_TradeCosts[salesEvent.m_Seller];
              // ISSUE: reference to a compiler-generated field
              TradeCost tradeCost2 = EconomyUtils.GetTradeCost(salesEvent.m_Resource, tradeCost1);
              // ISSUE: reference to a compiler-generated field
              f += (float) salesEvent.m_Amount * tradeCost2.m_BuyCost;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float weight = EconomyUtils.GetWeight(salesEvent.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas);
              // ISSUE: reference to a compiler-generated field
              Assert.IsTrue(salesEvent.m_Amount != -1);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float num = (float) EconomyUtils.GetTransportCost(salesEvent.m_Distance, salesEvent.m_Resource, salesEvent.m_Amount, weight) / (1f + (float) salesEvent.m_Amount);
              TradeCost tradeCost3 = new TradeCost();
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_TradeCosts.HasBuffer(salesEvent.m_Buyer))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                tradeCost3 = EconomyUtils.GetTradeCost(salesEvent.m_Resource, this.m_TradeCosts[salesEvent.m_Buyer]);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!this.m_OutsideConnections.HasComponent(salesEvent.m_Seller) && (salesEvent.m_Flags & ResourceBuyerSystem.SaleFlags.CommercialSeller) != ResourceBuyerSystem.SaleFlags.None)
              {
                tradeCost2.m_SellCost = math.lerp(tradeCost2.m_SellCost, num + tradeCost3.m_SellCost, 0.5f);
                // ISSUE: reference to a compiler-generated field
                EconomyUtils.SetTradeCost(salesEvent.m_Resource, tradeCost2, tradeCost1, true);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_TradeCosts.HasBuffer(salesEvent.m_Buyer) && !this.m_OutsideConnections.HasComponent(salesEvent.m_Buyer))
              {
                tradeCost3.m_BuyCost = math.lerp(tradeCost3.m_BuyCost, num + tradeCost2.m_BuyCost, 0.5f);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                EconomyUtils.SetTradeCost(salesEvent.m_Resource, tradeCost2, this.m_TradeCosts[salesEvent.m_Buyer], true);
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Resources.HasBuffer(salesEvent.m_Seller) || EconomyUtils.GetResources(salesEvent.m_Resource, this.m_Resources[salesEvent.m_Seller]) > 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              TaxSystem.GetIndustrialTaxRate(salesEvent.m_Resource, this.m_TaxRates);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (flag && this.m_Services.HasComponent(salesEvent.m_Seller) && this.m_PropertyRenters.HasComponent(salesEvent.m_Seller))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity prefab = this.m_Prefabs[salesEvent.m_Seller].m_Prefab;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                ServiceAvailable service = this.m_Services[salesEvent.m_Seller];
                // ISSUE: reference to a compiler-generated field
                ServiceCompanyData serviceCompany = this.m_ServiceCompanies[prefab];
                f *= EconomyUtils.GetServicePriceMultiplier((float) service.m_ServiceAvailable, serviceCompany.m_MaxService);
                // ISSUE: reference to a compiler-generated field
                service.m_ServiceAvailable = math.max(0, Mathf.RoundToInt((float) (service.m_ServiceAvailable - salesEvent.m_Amount)));
                service.m_MeanPriority = (double) service.m_MeanPriority <= 0.0 ? math.min(1f, (float) service.m_ServiceAvailable / (float) serviceCompany.m_MaxService) : math.min(1f, math.lerp(service.m_MeanPriority, (float) service.m_ServiceAvailable / (float) serviceCompany.m_MaxService, 0.1f));
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_Services[salesEvent.m_Seller] = service;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity property = this.m_PropertyRenters[salesEvent.m_Seller].m_Property;
                // ISSUE: reference to a compiler-generated field
                if (this.m_Districts.HasComponent(property))
                {
                  // ISSUE: reference to a compiler-generated field
                  Entity district = this.m_Districts[property].m_District;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  TaxSystem.GetModifiedCommercialTaxRate(salesEvent.m_Resource, this.m_TaxRates, district, this.m_DistrictModifiers);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  TaxSystem.GetCommercialTaxRate(salesEvent.m_Resource, this.m_TaxRates);
                }
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_Resources.HasBuffer(salesEvent.m_Seller))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<Game.Economy.Resources> resource = this.m_Resources[salesEvent.m_Seller];
                // ISSUE: reference to a compiler-generated field
                int resources = EconomyUtils.GetResources(salesEvent.m_Resource, resource);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                EconomyUtils.AddResources(salesEvent.m_Resource, -math.min(resources, Mathf.RoundToInt((float) salesEvent.m_Amount)), resource);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              EconomyUtils.AddResources(Resource.Money, -Mathf.RoundToInt(f), this.m_Resources[salesEvent.m_Buyer]);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_Households.HasComponent(salesEvent.m_Buyer))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Household household = this.m_Households[salesEvent.m_Buyer];
                household.m_Resources = (int) math.clamp((long) ((double) household.m_Resources + (double) f), (long) int.MinValue, (long) int.MaxValue);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_Households[salesEvent.m_Buyer] = household;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_BuyingCompanies.HasComponent(salesEvent.m_Buyer))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  BuyingCompany buyingCompany = this.m_BuyingCompanies[salesEvent.m_Buyer] with
                  {
                    m_LastTradePartner = salesEvent.m_Seller
                  };
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_BuyingCompanies[salesEvent.m_Buyer] = buyingCompany;
                  // ISSUE: reference to a compiler-generated field
                  if ((salesEvent.m_Flags & ResourceBuyerSystem.SaleFlags.Virtual) != ResourceBuyerSystem.SaleFlags.None)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    EconomyUtils.AddResources(salesEvent.m_Resource, salesEvent.m_Amount, this.m_Resources[salesEvent.m_Buyer]);
                  }
                }
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!this.m_Storages.HasComponent(salesEvent.m_Seller) && this.m_PropertyRenters.HasComponent(salesEvent.m_Seller))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<Game.Economy.Resources> resource = this.m_Resources[salesEvent.m_Seller];
                EconomyUtils.AddResources(Resource.Money, Mathf.RoundToInt(f), resource);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (salesEvent.m_Resource == Resource.Vehicles && salesEvent.m_Amount == HouseholdBehaviorSystem.kCarAmount && this.m_PropertyRenters.HasComponent(salesEvent.m_Seller))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity property = this.m_PropertyRenters[salesEvent.m_Seller].m_Property;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_TransformDatas.HasComponent(property) && this.m_HouseholdCitizens.HasBuffer(salesEvent.m_Buyer))
                {
                  // ISSUE: reference to a compiler-generated field
                  Entity buyer = salesEvent.m_Buyer;
                  // ISSUE: reference to a compiler-generated field
                  Game.Objects.Transform transformData = this.m_TransformDatas[property];
                  // ISSUE: reference to a compiler-generated field
                  int length1 = this.m_HouseholdCitizens[buyer].Length;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  int length2 = this.m_HouseholdAnimals.HasBuffer(buyer) ? this.m_HouseholdAnimals[buyer].Length : 0;
                  int passengerAmount;
                  int baggageAmount;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_OwnedVehicles.HasBuffer(buyer) && this.m_OwnedVehicles[buyer].Length >= 1)
                  {
                    passengerAmount = random.NextInt(1, 1 + length1);
                    baggageAmount = random.NextInt(1, 2 + length2);
                  }
                  else
                  {
                    passengerAmount = length1;
                    baggageAmount = 1 + length2;
                  }
                  if (random.NextInt(20) == 0)
                    baggageAmount += 5;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Entity vehicle = this.m_PersonalCarSelectData.CreateVehicle(this.m_CommandBuffer, ref random, passengerAmount, baggageAmount, true, false, transformData, property, Entity.Null, (PersonalCarFlags) 0, true);
                  if (vehicle != Entity.Null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Owner>(vehicle, new Owner(buyer));
                    // ISSUE: reference to a compiler-generated field
                    if (!this.m_OwnedVehicles.HasBuffer(buyer))
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddBuffer<OwnedVehicle>(buyer);
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
    private struct HandleBuyersJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<ResourceBuyer> m_BuyerType;
      [ReadOnly]
      public ComponentTypeHandle<ResourceBought> m_BoughtType;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public BufferTypeHandle<TripNeeded> m_TripType;
      [ReadOnly]
      public ComponentTypeHandle<Citizen> m_CitizenType;
      [ReadOnly]
      public ComponentTypeHandle<CreatureData> m_CreatureDataType;
      [ReadOnly]
      public ComponentTypeHandle<ResidentData> m_ResidentDataType;
      [ReadOnly]
      public ComponentTypeHandle<AttendingMeeting> m_AttendingMeetingType;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformation;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_Properties;
      [ReadOnly]
      public ComponentLookup<ServiceAvailable> m_ServiceAvailables;
      [ReadOnly]
      public ComponentLookup<CarKeeper> m_CarKeepers;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PersonalCar> m_PersonalCarData;
      [ReadOnly]
      public ComponentLookup<Game.Common.Target> m_Targets;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> m_CurrentBuildings;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMembers;
      [ReadOnly]
      public ComponentLookup<Household> m_Households;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> m_TouristHouseholds;
      [ReadOnly]
      public ComponentLookup<CommuterHousehold> m_CommuterHouseholds;
      [ReadOnly]
      public ComponentLookup<Worker> m_Workers;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> m_ServiceCompanyDatas;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> m_Resources;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<CoordinatedMeeting> m_CoordinatedMeetings;
      [ReadOnly]
      public BufferLookup<HaveCoordinatedMeetingData> m_HaveCoordinatedMeetingDatas;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<HumanData> m_PrefabHumanData;
      [ReadOnly]
      public ComponentLookup<Population> m_Populations;
      [ReadOnly]
      public float m_TimeOfDay;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public ComponentTypeSet m_PathfindTypes;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_HumanChunks;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public EconomyParameterData m_EconomyParameterData;
      public Entity m_City;
      public NativeQueue<ResourceBuyerSystem.SalesEvent>.ParallelWriter m_SalesQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ResourceBuyer> nativeArray2 = chunk.GetNativeArray<ResourceBuyer>(ref this.m_BuyerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ResourceBought> nativeArray3 = chunk.GetNativeArray<ResourceBought>(ref this.m_BoughtType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TripNeeded> bufferAccessor = chunk.GetBufferAccessor<TripNeeded>(ref this.m_TripType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Citizen> nativeArray4 = chunk.GetNativeArray<Citizen>(ref this.m_CitizenType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AttendingMeeting> nativeArray5 = chunk.GetNativeArray<AttendingMeeting>(ref this.m_AttendingMeetingType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: variable of a compiler-generated type
        ResourceBuyerSystem.SalesEvent salesEvent1;
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Entity e = nativeArray1[index];
          ResourceBought resourceBought = nativeArray3[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.HasComponent(resourceBought.m_Payer) && this.m_PrefabRefData.HasComponent(resourceBought.m_Seller))
          {
            // ISSUE: object of a compiler-generated type is created
            salesEvent1 = new ResourceBuyerSystem.SalesEvent();
            // ISSUE: reference to a compiler-generated field
            salesEvent1.m_Amount = resourceBought.m_Amount;
            // ISSUE: reference to a compiler-generated field
            salesEvent1.m_Buyer = resourceBought.m_Payer;
            // ISSUE: reference to a compiler-generated field
            salesEvent1.m_Seller = resourceBought.m_Seller;
            // ISSUE: reference to a compiler-generated field
            salesEvent1.m_Resource = resourceBought.m_Resource;
            // ISSUE: reference to a compiler-generated field
            salesEvent1.m_Flags = ResourceBuyerSystem.SaleFlags.None;
            // ISSUE: reference to a compiler-generated field
            salesEvent1.m_Distance = resourceBought.m_Distance;
            // ISSUE: variable of a compiler-generated type
            ResourceBuyerSystem.SalesEvent salesEvent2 = salesEvent1;
            // ISSUE: reference to a compiler-generated field
            this.m_SalesQueue.Enqueue(salesEvent2);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<ResourceBought>(unfilteredChunkIndex, e);
        }
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          ResourceBuyer resourceBuyer = nativeArray2[index];
          Entity entity = nativeArray1[index];
          DynamicBuffer<TripNeeded> dynamicBuffer = bufferAccessor[index];
          bool virtualGood = false;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ResourceDatas.HasComponent(this.m_ResourcePrefabs[resourceBuyer.m_ResourceNeeded]))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            virtualGood = (double) EconomyUtils.GetWeight(resourceBuyer.m_ResourceNeeded, this.m_ResourcePrefabs, ref this.m_ResourceDatas) == 0.0;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathInformation.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            PathInformation pathInformation = this.m_PathInformation[entity];
            if ((pathInformation.m_State & PathFlags.Pending) == (PathFlags) 0)
            {
              Entity destination = pathInformation.m_Destination;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_Properties.HasComponent(destination) || this.m_OutsideConnections.HasComponent(destination))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<Game.Economy.Resources> resource = this.m_Resources[destination];
                int resources = EconomyUtils.GetResources(resourceBuyer.m_ResourceNeeded, resource);
                if (virtualGood || resourceBuyer.m_AmountNeeded < 2 * resources)
                {
                  resourceBuyer.m_AmountNeeded = math.min(resourceBuyer.m_AmountNeeded, resources);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: variable of a compiler-generated type
                  ResourceBuyerSystem.SaleFlags saleFlags = this.m_ServiceAvailables.HasComponent(destination) ? ResourceBuyerSystem.SaleFlags.CommercialSeller : ResourceBuyerSystem.SaleFlags.None;
                  if (virtualGood)
                    saleFlags |= ResourceBuyerSystem.SaleFlags.Virtual;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_OutsideConnections.HasComponent(destination))
                    saleFlags |= ResourceBuyerSystem.SaleFlags.ImportFromOC;
                  // ISSUE: object of a compiler-generated type is created
                  salesEvent1 = new ResourceBuyerSystem.SalesEvent();
                  // ISSUE: reference to a compiler-generated field
                  salesEvent1.m_Amount = resourceBuyer.m_AmountNeeded;
                  // ISSUE: reference to a compiler-generated field
                  salesEvent1.m_Buyer = resourceBuyer.m_Payer;
                  // ISSUE: reference to a compiler-generated field
                  salesEvent1.m_Seller = destination;
                  // ISSUE: reference to a compiler-generated field
                  salesEvent1.m_Resource = resourceBuyer.m_ResourceNeeded;
                  // ISSUE: reference to a compiler-generated field
                  salesEvent1.m_Flags = saleFlags;
                  // ISSUE: reference to a compiler-generated field
                  salesEvent1.m_Distance = pathInformation.m_Distance;
                  // ISSUE: variable of a compiler-generated type
                  ResourceBuyerSystem.SalesEvent salesEvent3 = salesEvent1;
                  // ISSUE: reference to a compiler-generated field
                  this.m_SalesQueue.Enqueue(salesEvent3);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent(unfilteredChunkIndex, entity, in this.m_PathfindTypes);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<ResourceBuyer>(unfilteredChunkIndex, entity);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  int population = this.m_Populations[this.m_City].m_Population;
                  // ISSUE: reference to a compiler-generated field
                  bool flag = random.NextInt(100) < 100 - Mathf.RoundToInt(100f / math.max(1f, math.sqrt((float) ((double) this.m_EconomyParameterData.m_TrafficReduction * (double) population * 0.10000000149011612))));
                  if (!virtualGood && !flag)
                  {
                    dynamicBuffer.Add(new TripNeeded()
                    {
                      m_TargetAgent = destination,
                      m_Purpose = Game.Citizens.Purpose.Shopping,
                      m_Data = resourceBuyer.m_AmountNeeded,
                      m_Resource = resourceBuyer.m_ResourceNeeded
                    });
                    // ISSUE: reference to a compiler-generated field
                    if (!this.m_Targets.HasComponent(nativeArray1[index]))
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddComponent<Game.Common.Target>(unfilteredChunkIndex, entity, new Game.Common.Target()
                      {
                        m_Target = destination
                      });
                    }
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent(unfilteredChunkIndex, entity, in this.m_PathfindTypes);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<ResourceBuyer>(unfilteredChunkIndex, entity);
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<ResourceBuyer>(unfilteredChunkIndex, entity);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent(unfilteredChunkIndex, entity, in this.m_PathfindTypes);
                if (nativeArray5.IsCreated)
                {
                  AttendingMeeting attendingMeeting = nativeArray5[index];
                  // ISSUE: reference to a compiler-generated field
                  Entity prefab = this.m_PrefabRefData[attendingMeeting.m_Meeting].m_Prefab;
                  // ISSUE: reference to a compiler-generated field
                  CoordinatedMeeting coordinatedMeeting = this.m_CoordinatedMeetings[attendingMeeting.m_Meeting];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_HaveCoordinatedMeetingDatas[prefab][coordinatedMeeting.m_Phase].m_TravelPurpose.m_Purpose == Game.Citizens.Purpose.Shopping)
                  {
                    coordinatedMeeting.m_Status = MeetingStatus.Done;
                    // ISSUE: reference to a compiler-generated field
                    this.m_CoordinatedMeetings[attendingMeeting.m_Meeting] = coordinatedMeeting;
                  }
                }
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((!this.m_HouseholdMembers.HasComponent(entity) || !this.m_TouristHouseholds.HasComponent(this.m_HouseholdMembers[entity].m_Household) && !this.m_CommuterHouseholds.HasComponent(this.m_HouseholdMembers[entity].m_Household)) && this.m_CurrentBuildings.HasComponent(entity) && this.m_OutsideConnections.HasComponent(this.m_CurrentBuildings[entity].m_CurrentBuilding) && !nativeArray5.IsCreated)
            {
              // ISSUE: variable of a compiler-generated type
              ResourceBuyerSystem.SaleFlags saleFlags = ResourceBuyerSystem.SaleFlags.ImportFromOC;
              // ISSUE: object of a compiler-generated type is created
              salesEvent1 = new ResourceBuyerSystem.SalesEvent();
              // ISSUE: reference to a compiler-generated field
              salesEvent1.m_Amount = resourceBuyer.m_AmountNeeded;
              // ISSUE: reference to a compiler-generated field
              salesEvent1.m_Buyer = resourceBuyer.m_Payer;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              salesEvent1.m_Seller = this.m_CurrentBuildings[entity].m_CurrentBuilding;
              // ISSUE: reference to a compiler-generated field
              salesEvent1.m_Resource = resourceBuyer.m_ResourceNeeded;
              // ISSUE: reference to a compiler-generated field
              salesEvent1.m_Flags = saleFlags;
              // ISSUE: reference to a compiler-generated field
              salesEvent1.m_Distance = 0.0f;
              // ISSUE: variable of a compiler-generated type
              ResourceBuyerSystem.SalesEvent salesEvent4 = salesEvent1;
              // ISSUE: reference to a compiler-generated field
              this.m_SalesQueue.Enqueue(salesEvent4);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<ResourceBuyer>(unfilteredChunkIndex, entity);
            }
            else
            {
              Citizen citizen = new Citizen();
              if (nativeArray4.Length > 0)
              {
                Citizen citizenData = nativeArray4[index];
                // ISSUE: reference to a compiler-generated field
                Entity household1 = this.m_HouseholdMembers[entity].m_Household;
                // ISSUE: reference to a compiler-generated field
                Household household2 = this.m_Households[household1];
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<HouseholdCitizen> householdCitizen = this.m_HouseholdCitizens[household1];
                // ISSUE: reference to a compiler-generated method
                this.FindShopForCitizen(chunk, unfilteredChunkIndex, entity, resourceBuyer.m_ResourceNeeded, resourceBuyer.m_AmountNeeded, resourceBuyer.m_Flags, citizenData, household2, householdCitizen.Length, virtualGood);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.FindShopForCompany(chunk, unfilteredChunkIndex, entity, resourceBuyer.m_ResourceNeeded, resourceBuyer.m_AmountNeeded, resourceBuyer.m_Flags, virtualGood);
              }
            }
          }
        }
      }

      private void FindShopForCitizen(
        ArchetypeChunk chunk,
        int index,
        Entity buyer,
        Resource resource,
        int amount,
        SetupTargetFlags flags,
        Citizen citizenData,
        Household householdData,
        int householdCitizenCount,
        bool virtualGood)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(index, buyer, in this.m_PathfindTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PathInformation>(index, buyer, new PathInformation()
        {
          m_State = PathFlags.Pending
        });
        CreatureData creatureData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Entity entity = ObjectEmergeSystem.SelectResidentPrefab(citizenData, this.m_HumanChunks, this.m_EntityType, ref this.m_CreatureDataType, ref this.m_ResidentDataType, out creatureData, out PseudoRandomSeed _);
        HumanData humanData = new HumanData();
        if (entity != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          humanData = this.m_PrefabHumanData[entity];
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) 277.777771f,
          m_WalkSpeed = (float2) humanData.m_WalkSpeed,
          m_Weights = CitizenUtils.GetPathfindWeights(citizenData, householdData, householdCitizenCount),
          m_Methods = PathMethod.Pedestrian | PathMethod.Taxi | RouteUtils.GetPublicTransportMethods(this.m_TimeOfDay),
          m_SecondaryIgnoredRules = VehicleUtils.GetIgnoredPathfindRulesTaxiDefaults(),
          m_MaxCost = CitizenBehaviorSystem.kMaxPathfindCost
        };
        SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
        setupQueueTarget.m_Methods = PathMethod.Pedestrian;
        setupQueueTarget.m_RandomCost = 30f;
        SetupQueueTarget origin = setupQueueTarget;
        setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.ResourceSeller;
        setupQueueTarget.m_Methods = PathMethod.Pedestrian;
        setupQueueTarget.m_Resource = resource;
        setupQueueTarget.m_Value = amount;
        setupQueueTarget.m_Flags = flags;
        setupQueueTarget.m_RandomCost = 30f;
        setupQueueTarget.m_ActivityMask = creatureData.m_SupportedActivities;
        SetupQueueTarget destination = setupQueueTarget;
        if (virtualGood)
          parameters.m_PathfindFlags |= PathfindFlags.SkipPathfind;
        // ISSUE: reference to a compiler-generated field
        if (this.m_HouseholdMembers.HasComponent(buyer))
        {
          // ISSUE: reference to a compiler-generated field
          Entity household = this.m_HouseholdMembers[buyer].m_Household;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Properties.HasComponent(household))
          {
            // ISSUE: reference to a compiler-generated field
            parameters.m_Authorization1 = this.m_Properties[household].m_Property;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_Workers.HasComponent(buyer))
        {
          // ISSUE: reference to a compiler-generated field
          Worker worker = this.m_Workers[buyer];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          parameters.m_Authorization2 = !this.m_Properties.HasComponent(worker.m_Workplace) ? worker.m_Workplace : this.m_Properties[worker.m_Workplace].m_Property;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarKeepers.IsComponentEnabled(buyer))
        {
          // ISSUE: reference to a compiler-generated field
          Entity car = this.m_CarKeepers[buyer].m_Car;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ParkedCarData.HasComponent(car))
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[car];
            // ISSUE: reference to a compiler-generated field
            ParkedCar parkedCar = this.m_ParkedCarData[car];
            // ISSUE: reference to a compiler-generated field
            CarData carData = this.m_PrefabCarData[prefabRef.m_Prefab];
            parameters.m_MaxSpeed.x = carData.m_MaxSpeed;
            parameters.m_ParkingTarget = parkedCar.m_Lane;
            parameters.m_ParkingDelta = parkedCar.m_CurvePosition;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            parameters.m_ParkingSize = VehicleUtils.GetParkingSize(car, ref this.m_PrefabRefData, ref this.m_ObjectGeometryData);
            parameters.m_Methods |= PathMethod.Road | PathMethod.Parking;
            parameters.m_IgnoredRules = VehicleUtils.GetIgnoredPathfindRules(carData);
            Game.Vehicles.PersonalCar componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PersonalCarData.TryGetComponent(car, out componentData) && (componentData.m_State & PersonalCarFlags.HomeTarget) == (PersonalCarFlags) 0)
              parameters.m_PathfindFlags |= PathfindFlags.ParkingReset;
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_PathfindQueue.Enqueue(new SetupQueueItem(buyer, parameters, origin, destination));
      }

      private void FindShopForCompany(
        ArchetypeChunk chunk,
        int index,
        Entity buyer,
        Resource resource,
        int amount,
        SetupTargetFlags flags,
        bool virtualGood)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(index, buyer, in this.m_PathfindTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PathInformation>(index, buyer, new PathInformation()
        {
          m_State = PathFlags.Pending
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float transportCost = EconomyUtils.GetTransportCost(100f, amount, this.m_ResourceDatas[this.m_ResourcePrefabs[resource]].m_Weight, StorageTransferFlags.Car);
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) 111.111115f,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, transportCost, 1f),
          m_Methods = PathMethod.Road | PathMethod.CargoLoading,
          m_IgnoredRules = RuleFlags.ForbidSlowTraffic
        };
        SetupQueueTarget origin = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Methods = PathMethod.Road | PathMethod.CargoLoading,
          m_RoadTypes = RoadTypes.Car
        };
        SetupQueueTarget destination = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.ResourceSeller,
          m_Methods = PathMethod.Road | PathMethod.CargoLoading,
          m_RoadTypes = RoadTypes.Car,
          m_Resource = resource,
          m_Value = amount,
          m_Flags = flags
        };
        if (virtualGood)
          parameters.m_PathfindFlags |= PathfindFlags.SkipPathfind;
        // ISSUE: reference to a compiler-generated field
        this.m_PathfindQueue.Enqueue(new SetupQueueItem(buyer, parameters, origin, destination));
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
      public ComponentTypeHandle<ResourceBuyer> __Game_Companies_ResourceBuyer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ResourceBought> __Game_Citizens_ResourceBought_RO_ComponentTypeHandle;
      public BufferTypeHandle<TripNeeded> __Game_Citizens_TripNeeded_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Citizen> __Game_Citizens_Citizen_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CreatureData> __Game_Prefabs_CreatureData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ResidentData> __Game_Prefabs_ResidentData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<AttendingMeeting> __Game_Citizens_AttendingMeeting_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ServiceAvailable> __Game_Companies_ServiceAvailable_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarKeeper> __Game_Citizens_CarKeeper_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PersonalCar> __Game_Vehicles_PersonalCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Common.Target> __Game_Common_Target_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> __Game_Citizens_TouristHousehold_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CommuterHousehold> __Game_Citizens_CommuterHousehold_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> __Game_Companies_ServiceCompanyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Worker> __Game_Citizens_Worker_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HumanData> __Game_Prefabs_HumanData_RO_ComponentLookup;
      public ComponentLookup<CoordinatedMeeting> __Game_Citizens_CoordinatedMeeting_RW_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HaveCoordinatedMeetingData> __Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup;
      public ComponentLookup<Population> __Game_City_Population_RW_ComponentLookup;
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RW_BufferLookup;
      public ComponentLookup<ServiceAvailable> __Game_Companies_ServiceAvailable_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdAnimal> __Game_Citizens_HouseholdAnimal_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> __Game_Companies_StorageCompany_RO_ComponentLookup;
      public ComponentLookup<Household> __Game_Citizens_Household_RW_ComponentLookup;
      public ComponentLookup<BuyingCompany> __Game_Companies_BuyingCompany_RW_ComponentLookup;
      public BufferLookup<TradeCost> __Game_Companies_TradeCost_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<DistrictModifier> __Game_Areas_DistrictModifier_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Population> __Game_City_Population_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ResourceBuyer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ResourceBuyer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_ResourceBought_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ResourceBought>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TripNeeded_RW_BufferTypeHandle = state.GetBufferTypeHandle<TripNeeded>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CreatureData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CreatureData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResidentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ResidentData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_AttendingMeeting_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AttendingMeeting>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceAvailable_RO_ComponentLookup = state.GetComponentLookup<ServiceAvailable>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CarKeeper_RO_ComponentLookup = state.GetComponentLookup<CarKeeper>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PersonalCar_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PersonalCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentLookup = state.GetComponentLookup<Game.Common.Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentLookup = state.GetComponentLookup<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TouristHousehold_RO_ComponentLookup = state.GetComponentLookup<TouristHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CommuterHousehold_RO_ComponentLookup = state.GetComponentLookup<CommuterHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceCompanyData_RO_ComponentLookup = state.GetComponentLookup<ServiceCompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Worker_RO_ComponentLookup = state.GetComponentLookup<Worker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HumanData_RO_ComponentLookup = state.GetComponentLookup<HumanData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CoordinatedMeeting_RW_ComponentLookup = state.GetComponentLookup<CoordinatedMeeting>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup = state.GetBufferLookup<HaveCoordinatedMeetingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_Population_RW_ComponentLookup = state.GetComponentLookup<Population>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceAvailable_RW_ComponentLookup = state.GetComponentLookup<ServiceAvailable>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RO_BufferLookup = state.GetBufferLookup<OwnedVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdAnimal_RO_BufferLookup = state.GetBufferLookup<HouseholdAnimal>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageCompany_RO_ComponentLookup = state.GetComponentLookup<Game.Companies.StorageCompany>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RW_ComponentLookup = state.GetComponentLookup<Household>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_BuyingCompany_RW_ComponentLookup = state.GetComponentLookup<BuyingCompany>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_TradeCost_RW_BufferLookup = state.GetBufferLookup<TradeCost>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentLookup = state.GetComponentLookup<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_DistrictModifier_RO_BufferLookup = state.GetBufferLookup<DistrictModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_Population_RO_ComponentLookup = state.GetComponentLookup<Population>(true);
      }
    }
  }
}
