// Decompiled with JetBrains decompiler
// Type: Game.Rendering.ObjectColorSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Creatures;
using Game.Economy;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Routes;
using Game.Simulation;
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
namespace Game.Rendering
{
  [CompilerGenerated]
  public class ObjectColorSystem : GameSystemBase
  {
    private EntityQuery m_ObjectQuery;
    private EntityQuery m_MiddleObjectQuery;
    private EntityQuery m_TempObjectQuery;
    private EntityQuery m_SubObjectQuery;
    private EntityQuery m_InfomodeQuery;
    private EntityQuery m_HappinessParameterQuery;
    private EntityQuery m_EconomyParameterQuery;
    private EntityQuery m_PollutionParameterQuery;
    private EntityQuery m_FireConfigQuery;
    private ToolSystem m_ToolSystem;
    private LocalEffectSystem m_LocalEffectSystem;
    private ClimateSystem m_ClimateSystem;
    private FireHazardSystem m_FireHazardSystem;
    private TelecomCoverageSystem m_TelecomCoverageSystem;
    private CitySystem m_CitySystem;
    private PrefabSystem m_PrefabSystem;
    private ObjectColorSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LocalEffectSystem = this.World.GetOrCreateSystemManaged<LocalEffectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_FireHazardSystem = this.World.GetOrCreateSystemManaged<FireHazardSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomCoverageSystem = this.World.GetOrCreateSystemManaged<TelecomCoverageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Objects.Object>(),
          ComponentType.ReadWrite<Game.Objects.Color>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Hidden>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Owner>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Objects.Object>(),
          ComponentType.ReadOnly<Owner>(),
          ComponentType.ReadWrite<Game.Objects.Color>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Vehicle>(),
          ComponentType.ReadOnly<Creature>(),
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Game.Objects.UtilityObject>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Hidden>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_MiddleObjectQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Owner>(),
          ComponentType.ReadWrite<Game.Objects.Color>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Hidden>(),
          ComponentType.ReadOnly<Deleted>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Vehicle>(),
          ComponentType.ReadOnly<Controller>(),
          ComponentType.ReadWrite<Game.Objects.Color>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Hidden>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_TempObjectQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Objects.Object>(),
          ComponentType.ReadWrite<Game.Objects.Color>(),
          ComponentType.ReadOnly<Temp>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Hidden>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_SubObjectQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Objects.Object>(),
          ComponentType.ReadOnly<Owner>(),
          ComponentType.ReadWrite<Game.Objects.Color>()
        },
        None = new ComponentType[6]
        {
          ComponentType.ReadOnly<Hidden>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Vehicle>(),
          ComponentType.ReadOnly<Creature>(),
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Game.Objects.UtilityObject>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_InfomodeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<InfomodeActive>()
        },
        Any = new ComponentType[6]
        {
          ComponentType.ReadOnly<InfoviewBuildingData>(),
          ComponentType.ReadOnly<InfoviewBuildingStatusData>(),
          ComponentType.ReadOnly<InfoviewTransportStopData>(),
          ComponentType.ReadOnly<InfoviewVehicleData>(),
          ComponentType.ReadOnly<InfoviewObjectStatusData>(),
          ComponentType.ReadOnly<InfoviewNetStatusData>()
        },
        None = new ComponentType[0]
      });
      // ISSUE: reference to a compiler-generated field
      this.m_HappinessParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<CitizenHappinessParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PollutionParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<PollutionParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_FireConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<FireConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HappinessParameterQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_ToolSystem.activeInfoview == (UnityEngine.Object) null || this.m_ObjectQuery.IsEmptyIgnoreFilter)
        return;
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      LocalEffectSystem.ReadData readData = this.m_LocalEffectSystem.GetReadData(out dependencies1);
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_InfomodeQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      FireConfigurationPrefab prefab = this.m_PrefabSystem.GetPrefab<FireConfigurationPrefab>(this.m_FireConfigQuery.GetSingletonEntity());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Color_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_LeisureProvider_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Profitability_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SewageOutletData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PollutionModifierData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PollutionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TreeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Placeholder_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_UniqueObject_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_UtilityObject_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Plant_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PrisonerTransport_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PostVan_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PoliceCar_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Hearse_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_GarbageTruck_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_FireEngine_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_EvacuatingTransport_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Ambulance_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_RoadMaintenanceVehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkMaintenanceVehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Taxi_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CargoTransport_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PassengerTransport_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_SubwayStop_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_AirplaneStop_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_MailBox_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_ShipStop_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TramStop_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TrainStop_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_BusStop_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_LeisureProvider_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ExtractorProperty_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_StorageProperty_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_OfficeProperty_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_IndustrialProperty_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CommercialProperty_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ResidentialProperty_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_BuildingCondition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Battery_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ParkingFacility_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ResearchFacility_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WelfareOffice_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_AdminBuilding_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Prison_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_DeathcareFacility_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_FirewatchTower_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_DisasterFacility_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_EmergencyShelter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_AttractivenessProvider_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Park_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_School_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_TelecomFacility_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PostFacility_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ParkMaintenance_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_RoadMaintenance_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PoliceStation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_FireStation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageFacility_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_TransportStation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WastewaterTreatmentPlant_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_SewageOutlet_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterTower_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterPumpingStation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Transformer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityProducer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Hospital_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewObjectStatusData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewVehicleData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewTransportStopData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewBuildingStatusData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      ObjectColorSystem.UpdateObjectColorsJob jobData1 = new ObjectColorSystem.UpdateObjectColorsJob()
      {
        m_InfomodeChunks = archetypeChunkListAsync,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_InfomodeActiveType = this.__TypeHandle.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle,
        m_InfoviewBuildingType = this.__TypeHandle.__Game_Prefabs_InfoviewBuildingData_RO_ComponentTypeHandle,
        m_InfoviewBuildingStatusType = this.__TypeHandle.__Game_Prefabs_InfoviewBuildingStatusData_RO_ComponentTypeHandle,
        m_InfoviewTransportStopType = this.__TypeHandle.__Game_Prefabs_InfoviewTransportStopData_RO_ComponentTypeHandle,
        m_InfoviewVehicleType = this.__TypeHandle.__Game_Prefabs_InfoviewVehicleData_RO_ComponentTypeHandle,
        m_InfoviewObjectStatusType = this.__TypeHandle.__Game_Prefabs_InfoviewObjectStatusData_RO_ComponentTypeHandle,
        m_InfoviewNetStatusType = this.__TypeHandle.__Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_HospitalType = this.__TypeHandle.__Game_Buildings_Hospital_RO_ComponentTypeHandle,
        m_ElectricityProducerType = this.__TypeHandle.__Game_Buildings_ElectricityProducer_RO_ComponentTypeHandle,
        m_TransformerType = this.__TypeHandle.__Game_Buildings_Transformer_RO_ComponentTypeHandle,
        m_WaterPumpingStationType = this.__TypeHandle.__Game_Buildings_WaterPumpingStation_RO_ComponentTypeHandle,
        m_WaterTowerType = this.__TypeHandle.__Game_Buildings_WaterTower_RO_ComponentTypeHandle,
        m_SewageOutletType = this.__TypeHandle.__Game_Buildings_SewageOutlet_RO_ComponentTypeHandle,
        m_WastewaterTreatmentPlantType = this.__TypeHandle.__Game_Buildings_WastewaterTreatmentPlant_RO_ComponentTypeHandle,
        m_TransportDepotType = this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentTypeHandle,
        m_TransportStationType = this.__TypeHandle.__Game_Buildings_TransportStation_RO_ComponentTypeHandle,
        m_GarbageFacilityType = this.__TypeHandle.__Game_Buildings_GarbageFacility_RO_ComponentTypeHandle,
        m_FireStationType = this.__TypeHandle.__Game_Buildings_FireStation_RO_ComponentTypeHandle,
        m_PoliceStationType = this.__TypeHandle.__Game_Buildings_PoliceStation_RO_ComponentTypeHandle,
        m_CrimeProducerType = this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentTypeHandle,
        m_RoadMaintenanceType = this.__TypeHandle.__Game_Buildings_RoadMaintenance_RO_ComponentTypeHandle,
        m_ParkMaintenanceType = this.__TypeHandle.__Game_Buildings_ParkMaintenance_RO_ComponentTypeHandle,
        m_PostFacilityType = this.__TypeHandle.__Game_Buildings_PostFacility_RO_ComponentTypeHandle,
        m_TelecomFacilityType = this.__TypeHandle.__Game_Buildings_TelecomFacility_RO_ComponentTypeHandle,
        m_SchoolType = this.__TypeHandle.__Game_Buildings_School_RO_ComponentTypeHandle,
        m_ParkType = this.__TypeHandle.__Game_Buildings_Park_RO_ComponentTypeHandle,
        m_AttractivenessProviderType = this.__TypeHandle.__Game_Buildings_AttractivenessProvider_RO_ComponentTypeHandle,
        m_EmergencyShelterType = this.__TypeHandle.__Game_Buildings_EmergencyShelter_RO_ComponentTypeHandle,
        m_DisasterFacilityType = this.__TypeHandle.__Game_Buildings_DisasterFacility_RO_ComponentTypeHandle,
        m_FirewatchTowerType = this.__TypeHandle.__Game_Buildings_FirewatchTower_RO_ComponentTypeHandle,
        m_DeathcareFacilityType = this.__TypeHandle.__Game_Buildings_DeathcareFacility_RO_ComponentTypeHandle,
        m_PrisonType = this.__TypeHandle.__Game_Buildings_Prison_RO_ComponentTypeHandle,
        m_AdminBuildingType = this.__TypeHandle.__Game_Buildings_AdminBuilding_RO_ComponentTypeHandle,
        m_WelfareOfficeType = this.__TypeHandle.__Game_Buildings_WelfareOffice_RO_ComponentTypeHandle,
        m_ResearchFacilityType = this.__TypeHandle.__Game_Buildings_ResearchFacility_RO_ComponentTypeHandle,
        m_ParkingFacilityType = this.__TypeHandle.__Game_Buildings_ParkingFacility_RO_ComponentTypeHandle,
        m_BatteryType = this.__TypeHandle.__Game_Buildings_Battery_RO_ComponentTypeHandle,
        m_MailProducerType = this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentTypeHandle,
        m_BuildingConditionType = this.__TypeHandle.__Game_Buildings_BuildingCondition_RO_ComponentTypeHandle,
        m_ResidentialPropertyType = this.__TypeHandle.__Game_Buildings_ResidentialProperty_RO_ComponentTypeHandle,
        m_CommercialPropertyType = this.__TypeHandle.__Game_Buildings_CommercialProperty_RO_ComponentTypeHandle,
        m_IndustrialPropertyType = this.__TypeHandle.__Game_Buildings_IndustrialProperty_RO_ComponentTypeHandle,
        m_OfficePropertyType = this.__TypeHandle.__Game_Buildings_OfficeProperty_RO_ComponentTypeHandle,
        m_StoragePropertyType = this.__TypeHandle.__Game_Buildings_StorageProperty_RO_ComponentTypeHandle,
        m_ExtractorPropertyType = this.__TypeHandle.__Game_Buildings_ExtractorProperty_RO_ComponentTypeHandle,
        m_GarbageProducerType = this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentTypeHandle,
        m_AbandonedType = this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentTypeHandle,
        m_LeisureProviderType = this.__TypeHandle.__Game_Buildings_LeisureProvider_RO_ComponentTypeHandle,
        m_ElectricityConsumerType = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle,
        m_WaterConsumerType = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentTypeHandle,
        m_BuildingEfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_RenterType = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_TransportStopType = this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentTypeHandle,
        m_BusStopType = this.__TypeHandle.__Game_Routes_BusStop_RO_ComponentTypeHandle,
        m_TrainStopType = this.__TypeHandle.__Game_Routes_TrainStop_RO_ComponentTypeHandle,
        m_TaxiStandType = this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentTypeHandle,
        m_TramStopType = this.__TypeHandle.__Game_Routes_TramStop_RO_ComponentTypeHandle,
        m_ShipStopType = this.__TypeHandle.__Game_Routes_ShipStop_RO_ComponentTypeHandle,
        m_MailBoxType = this.__TypeHandle.__Game_Routes_MailBox_RO_ComponentTypeHandle,
        m_AirplaneStopType = this.__TypeHandle.__Game_Routes_AirplaneStop_RO_ComponentTypeHandle,
        m_SubwayStopType = this.__TypeHandle.__Game_Routes_SubwayStop_RO_ComponentTypeHandle,
        m_VehicleType = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentTypeHandle,
        m_PassengerTransportType = this.__TypeHandle.__Game_Vehicles_PassengerTransport_RO_ComponentTypeHandle,
        m_CargoTransportType = this.__TypeHandle.__Game_Vehicles_CargoTransport_RO_ComponentTypeHandle,
        m_TaxiType = this.__TypeHandle.__Game_Vehicles_Taxi_RO_ComponentTypeHandle,
        m_ParkMaintenanceVehicleType = this.__TypeHandle.__Game_Vehicles_ParkMaintenanceVehicle_RO_ComponentTypeHandle,
        m_RoadMaintenanceVehicleType = this.__TypeHandle.__Game_Vehicles_RoadMaintenanceVehicle_RO_ComponentTypeHandle,
        m_AmbulanceType = this.__TypeHandle.__Game_Vehicles_Ambulance_RO_ComponentTypeHandle,
        m_EvacuatingTransportType = this.__TypeHandle.__Game_Vehicles_EvacuatingTransport_RO_ComponentTypeHandle,
        m_FireEngineType = this.__TypeHandle.__Game_Vehicles_FireEngine_RO_ComponentTypeHandle,
        m_GarbageTruckType = this.__TypeHandle.__Game_Vehicles_GarbageTruck_RO_ComponentTypeHandle,
        m_HearseType = this.__TypeHandle.__Game_Vehicles_Hearse_RO_ComponentTypeHandle,
        m_PoliceCarType = this.__TypeHandle.__Game_Vehicles_PoliceCar_RO_ComponentTypeHandle,
        m_PostVanType = this.__TypeHandle.__Game_Vehicles_PostVan_RO_ComponentTypeHandle,
        m_PrisonerTransportType = this.__TypeHandle.__Game_Vehicles_PrisonerTransport_RO_ComponentTypeHandle,
        m_PassengerType = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle,
        m_ResidentType = this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentTypeHandle,
        m_TreeType = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentTypeHandle,
        m_PlantType = this.__TypeHandle.__Game_Objects_Plant_RO_ComponentTypeHandle,
        m_UtilityObjectType = this.__TypeHandle.__Game_Objects_UtilityObject_RO_ComponentTypeHandle,
        m_DestroyedType = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_DamagedType = this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentTypeHandle,
        m_UnderConstructionType = this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentTypeHandle,
        m_UniqueObjectType = this.__TypeHandle.__Game_Objects_UniqueObject_RO_ComponentTypeHandle,
        m_PlaceholderType = this.__TypeHandle.__Game_Objects_Placeholder_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_CurrentDistrictType = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle,
        m_AttachedType = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle,
        m_TreeData = this.__TypeHandle.__Game_Prefabs_TreeData_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_BuildingPropertyData = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_PollutionData = this.__TypeHandle.__Game_Prefabs_PollutionData_RO_ComponentLookup,
        m_PollutionModifierData = this.__TypeHandle.__Game_Prefabs_PollutionModifierData_RO_ComponentLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_Households = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_SpawnableBuildingDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_PlaceholderBuildingData = this.__TypeHandle.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup,
        m_SewageOutletData = this.__TypeHandle.__Game_Prefabs_SewageOutletData_RO_ComponentLookup,
        m_ResidentData = this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup,
        m_Profitabilities = this.__TypeHandle.__Game_Companies_Profitability_RO_ComponentLookup,
        m_LeisureProviders = this.__TypeHandle.__Game_Buildings_LeisureProvider_RO_ComponentLookup,
        m_IndustrialProcessData = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_Employees = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup,
        m_ResourceBuffs = this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ZoneDatas = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup,
        m_PrefabUtilityObjectData = this.__TypeHandle.__Game_Prefabs_UtilityObjectData_RO_ComponentLookup,
        m_ElectricityConsumerData = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup,
        m_ElectricityNodeConnectionData = this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup,
        m_ElectricityFlowEdgeData = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup,
        m_ConnectedFlowEdges = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_FireHazardData = new EventHelpers.FireHazardData((SystemBase) this, readData, prefab, (float) this.m_ClimateSystem.temperature, this.m_FireHazardSystem.noRainDays),
        m_TelecomCoverageData = this.m_TelecomCoverageSystem.GetData(true, out dependencies2),
        m_PollutionParameters = this.m_PollutionParameterQuery.GetSingleton<PollutionParameterData>(),
        m_City = this.m_CitySystem.City,
        m_ColorType = this.__TypeHandle.__Game_Objects_Color_RW_ComponentTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Color_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewObjectStatusData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      ObjectColorSystem.UpdateMiddleObjectColorsJob jobData2 = new ObjectColorSystem.UpdateMiddleObjectColorsJob()
      {
        m_InfomodeChunks = archetypeChunkListAsync,
        m_InfomodeActiveType = this.__TypeHandle.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle,
        m_InfoviewObjectStatusType = this.__TypeHandle.__Game_Prefabs_InfoviewObjectStatusData_RO_ComponentTypeHandle,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_ControllerType = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_ColorData = this.__TypeHandle.__Game_Objects_Color_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Color_RW_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ObjectColorSystem.UpdateTempObjectColorsJob jobData3 = new ObjectColorSystem.UpdateTempObjectColorsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_ColorData = this.__TypeHandle.__Game_Objects_Color_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Color_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Plant_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ObjectColorSystem.UpdateSubObjectColorsJob jobData4 = new ObjectColorSystem.UpdateSubObjectColorsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_ElevationType = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentTypeHandle,
        m_TreeType = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentTypeHandle,
        m_PlantType = this.__TypeHandle.__Game_Objects_Plant_RO_ComponentTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_VehicleData = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup,
        m_ColorData = this.__TypeHandle.__Game_Objects_Color_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.ScheduleParallel<ObjectColorSystem.UpdateObjectColorsJob>(this.m_ObjectQuery, JobHandle.CombineDependencies(this.Dependency, JobHandle.CombineDependencies(dependencies2, dependencies1, outJobHandle)));
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = jobData2.ScheduleParallel<ObjectColorSystem.UpdateMiddleObjectColorsJob>(this.m_MiddleObjectQuery, jobHandle1);
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle3 = jobData3.ScheduleParallel<ObjectColorSystem.UpdateTempObjectColorsJob>(this.m_TempObjectQuery, jobHandle2);
      // ISSUE: reference to a compiler-generated field
      EntityQuery subObjectQuery = this.m_SubObjectQuery;
      JobHandle dependsOn = jobHandle3;
      JobHandle jobHandle4 = jobData4.ScheduleParallel<ObjectColorSystem.UpdateSubObjectColorsJob>(subObjectQuery, dependsOn);
      archetypeChunkListAsync.Dispose(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LocalEffectSystem.AddLocalEffectReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomCoverageSystem.AddReader(jobHandle1);
      this.Dependency = jobHandle4;
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
    public ObjectColorSystem()
    {
    }

    [BurstCompile]
    private struct UpdateObjectColorsJob : IJobChunk
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_InfomodeChunks;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<InfomodeActive> m_InfomodeActiveType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewBuildingData> m_InfoviewBuildingType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewBuildingStatusData> m_InfoviewBuildingStatusType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewTransportStopData> m_InfoviewTransportStopType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewVehicleData> m_InfoviewVehicleType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewObjectStatusData> m_InfoviewObjectStatusType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewNetStatusData> m_InfoviewNetStatusType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Hospital> m_HospitalType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityProducer> m_ElectricityProducerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Transformer> m_TransformerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.WaterPumpingStation> m_WaterPumpingStationType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.WaterTower> m_WaterTowerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.SewageOutlet> m_SewageOutletType;
      [ReadOnly]
      public ComponentTypeHandle<WastewaterTreatmentPlant> m_WastewaterTreatmentPlantType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.TransportDepot> m_TransportDepotType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.TransportStation> m_TransportStationType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.GarbageFacility> m_GarbageFacilityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.FireStation> m_FireStationType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.PoliceStation> m_PoliceStationType;
      [ReadOnly]
      public ComponentTypeHandle<CrimeProducer> m_CrimeProducerType;
      [ReadOnly]
      public ComponentTypeHandle<RoadMaintenance> m_RoadMaintenanceType;
      [ReadOnly]
      public ComponentTypeHandle<ParkMaintenance> m_ParkMaintenanceType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.PostFacility> m_PostFacilityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.TelecomFacility> m_TelecomFacilityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.School> m_SchoolType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Park> m_ParkType;
      [ReadOnly]
      public ComponentTypeHandle<AttractivenessProvider> m_AttractivenessProviderType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.EmergencyShelter> m_EmergencyShelterType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.DisasterFacility> m_DisasterFacilityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.FirewatchTower> m_FirewatchTowerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.DeathcareFacility> m_DeathcareFacilityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Prison> m_PrisonType;
      [ReadOnly]
      public ComponentTypeHandle<AdminBuilding> m_AdminBuildingType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.WelfareOffice> m_WelfareOfficeType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.ResearchFacility> m_ResearchFacilityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.ParkingFacility> m_ParkingFacilityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Battery> m_BatteryType;
      [ReadOnly]
      public ComponentTypeHandle<MailProducer> m_MailProducerType;
      [ReadOnly]
      public ComponentTypeHandle<BuildingCondition> m_BuildingConditionType;
      [ReadOnly]
      public ComponentTypeHandle<ResidentialProperty> m_ResidentialPropertyType;
      [ReadOnly]
      public ComponentTypeHandle<CommercialProperty> m_CommercialPropertyType;
      [ReadOnly]
      public ComponentTypeHandle<IndustrialProperty> m_IndustrialPropertyType;
      [ReadOnly]
      public ComponentTypeHandle<OfficeProperty> m_OfficePropertyType;
      [ReadOnly]
      public ComponentTypeHandle<StorageProperty> m_StoragePropertyType;
      [ReadOnly]
      public ComponentTypeHandle<ExtractorProperty> m_ExtractorPropertyType;
      [ReadOnly]
      public ComponentTypeHandle<GarbageProducer> m_GarbageProducerType;
      [ReadOnly]
      public ComponentTypeHandle<Abandoned> m_AbandonedType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.LeisureProvider> m_LeisureProviderType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityConsumer> m_ElectricityConsumerType;
      [ReadOnly]
      public ComponentTypeHandle<WaterConsumer> m_WaterConsumerType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_BuildingEfficiencyType;
      [ReadOnly]
      public BufferTypeHandle<Renter> m_RenterType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.TransportStop> m_TransportStopType;
      [ReadOnly]
      public ComponentTypeHandle<BusStop> m_BusStopType;
      [ReadOnly]
      public ComponentTypeHandle<TrainStop> m_TrainStopType;
      [ReadOnly]
      public ComponentTypeHandle<TaxiStand> m_TaxiStandType;
      [ReadOnly]
      public ComponentTypeHandle<TramStop> m_TramStopType;
      [ReadOnly]
      public ComponentTypeHandle<ShipStop> m_ShipStopType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.MailBox> m_MailBoxType;
      [ReadOnly]
      public ComponentTypeHandle<AirplaneStop> m_AirplaneStopType;
      [ReadOnly]
      public ComponentTypeHandle<SubwayStop> m_SubwayStopType;
      [ReadOnly]
      public ComponentTypeHandle<Vehicle> m_VehicleType;
      [ReadOnly]
      public ComponentTypeHandle<PassengerTransport> m_PassengerTransportType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.CargoTransport> m_CargoTransportType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.Taxi> m_TaxiType;
      [ReadOnly]
      public ComponentTypeHandle<ParkMaintenanceVehicle> m_ParkMaintenanceVehicleType;
      [ReadOnly]
      public ComponentTypeHandle<RoadMaintenanceVehicle> m_RoadMaintenanceVehicleType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.Ambulance> m_AmbulanceType;
      [ReadOnly]
      public ComponentTypeHandle<EvacuatingTransport> m_EvacuatingTransportType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.FireEngine> m_FireEngineType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.GarbageTruck> m_GarbageTruckType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.Hearse> m_HearseType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.PoliceCar> m_PoliceCarType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.PostVan> m_PostVanType;
      [ReadOnly]
      public ComponentTypeHandle<PrisonerTransport> m_PrisonerTransportType;
      [ReadOnly]
      public BufferTypeHandle<Passenger> m_PassengerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Creatures.Resident> m_ResidentType;
      [ReadOnly]
      public ComponentTypeHandle<Tree> m_TreeType;
      [ReadOnly]
      public ComponentTypeHandle<Plant> m_PlantType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.UtilityObject> m_UtilityObjectType;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> m_DestroyedType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Damaged> m_DamagedType;
      [ReadOnly]
      public ComponentTypeHandle<UnderConstruction> m_UnderConstructionType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.UniqueObject> m_UniqueObjectType;
      [ReadOnly]
      public ComponentTypeHandle<Placeholder> m_PlaceholderType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> m_CurrentDistrictType;
      [ReadOnly]
      public ComponentTypeHandle<Attached> m_AttachedType;
      [ReadOnly]
      public ComponentLookup<TreeData> m_TreeData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyData;
      [ReadOnly]
      public ComponentLookup<PollutionData> m_PollutionData;
      [ReadOnly]
      public ComponentLookup<PollutionModifierData> m_PollutionModifierData;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDatas;
      [ReadOnly]
      public ComponentLookup<PlaceholderBuildingData> m_PlaceholderBuildingData;
      [ReadOnly]
      public ComponentLookup<SewageOutletData> m_SewageOutletData;
      [ReadOnly]
      public ComponentLookup<Game.Creatures.Resident> m_ResidentData;
      [ReadOnly]
      public ComponentLookup<Household> m_Households;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public ComponentLookup<Profitability> m_Profitabilities;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.LeisureProvider> m_LeisureProviders;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessData;
      [ReadOnly]
      public ComponentLookup<ZoneData> m_ZoneDatas;
      [ReadOnly]
      public BufferLookup<Employee> m_Employees;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> m_ResourceBuffs;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.UtilityObjectData> m_PrefabUtilityObjectData;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> m_ElectricityConsumerData;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> m_ElectricityNodeConnectionData;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> m_ElectricityFlowEdgeData;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_ConnectedFlowEdges;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public EventHelpers.FireHazardData m_FireHazardData;
      [ReadOnly]
      public CellMapData<TelecomCoverage> m_TelecomCoverageData;
      public PollutionParameterData m_PollutionParameters;
      public Entity m_City;
      public ComponentTypeHandle<Game.Objects.Color> m_ColorType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Color> nativeArray = chunk.GetNativeArray<Game.Objects.Color>(ref this.m_ColorType);
        // ISSUE: reference to a compiler-generated field
        bool isBuilding = chunk.Has<Building>(ref this.m_BuildingType);
        bool isSubBuilding = false;
        if (isBuilding)
        {
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Owner>(ref this.m_OwnerType))
          {
            isSubBuilding = true;
          }
          else
          {
            int index1;
            // ISSUE: reference to a compiler-generated method
            if (this.GetBuildingColor(chunk, out index1))
            {
              for (int index2 = 0; index2 < nativeArray.Length; ++index2)
                nativeArray[index2] = new Game.Objects.Color((byte) index1, (byte) 0);
              // ISSUE: reference to a compiler-generated method
              this.CheckColors(nativeArray, chunk, isBuilding);
              return;
            }
            InfoviewBuildingStatusData statusData;
            InfomodeActive activeData;
            // ISSUE: reference to a compiler-generated method
            if (this.GetBuildingStatusType(chunk, out statusData, out activeData))
            {
              // ISSUE: reference to a compiler-generated method
              this.GetBuildingStatusColors(nativeArray, chunk, statusData, activeData);
              // ISSUE: reference to a compiler-generated method
              this.CheckColors(nativeArray, chunk, isBuilding);
              return;
            }
          }
        }
        int index3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (chunk.Has<Game.Routes.TransportStop>(ref this.m_TransportStopType) && this.GetTransportStopColor(chunk, out index3))
        {
          for (int index4 = 0; index4 < nativeArray.Length; ++index4)
            nativeArray[index4] = new Game.Objects.Color((byte) index3, (byte) 0);
        }
        else
        {
          int index5;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (chunk.Has<Vehicle>(ref this.m_VehicleType) && this.GetVehicleColor(chunk, out index5))
          {
            for (int index6 = 0; index6 < nativeArray.Length; ++index6)
              nativeArray[index6] = new Game.Objects.Color((byte) index5, (byte) 0);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (chunk.Has<Game.Objects.UtilityObject>(ref this.m_UtilityObjectType) && this.GetNetStatusColors(nativeArray, chunk))
              return;
            InfoviewObjectStatusData statusData;
            InfomodeActive activeData;
            // ISSUE: reference to a compiler-generated method
            if (this.GetObjectStatusType(chunk, isSubBuilding, out statusData, out activeData))
            {
              // ISSUE: reference to a compiler-generated method
              this.GetObjectStatusColors(nativeArray, chunk, statusData, activeData);
              // ISSUE: reference to a compiler-generated method
              this.CheckColors(nativeArray, chunk, isBuilding);
            }
            else
            {
              for (int index7 = 0; index7 < nativeArray.Length; ++index7)
                nativeArray[index7] = new Game.Objects.Color();
              // ISSUE: reference to a compiler-generated method
              this.CheckColors(nativeArray, chunk, isBuilding);
            }
          }
        }
      }

      private void CheckColors(NativeArray<Game.Objects.Color> colors, ArchetypeChunk chunk, bool isBuilding)
      {
        if (!isBuilding)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Destroyed> nativeArray1 = chunk.GetNativeArray<Destroyed>(ref this.m_DestroyedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<UnderConstruction> nativeArray2 = chunk.GetNativeArray<UnderConstruction>(ref this.m_UnderConstructionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Destroyed destroyed;
          UnderConstruction underConstruction;
          // ISSUE: reference to a compiler-generated field
          if ((this.m_BuildingData[nativeArray3[index].m_Prefab].m_Flags & Game.Prefabs.BuildingFlags.ColorizeLot) != (Game.Prefabs.BuildingFlags) 0 || CollectionUtils.TryGet<Destroyed>(nativeArray1, index, out destroyed) && (double) destroyed.m_Cleared >= 0.0 || CollectionUtils.TryGet<UnderConstruction>(nativeArray2, index, out underConstruction) && underConstruction.m_NewPrefab == Entity.Null)
          {
            Game.Objects.Color color = colors[index] with
            {
              m_SubColor = true
            };
            colors[index] = color;
          }
        }
      }

      private bool GetBuildingColor(ArchetypeChunk chunk, out int index)
      {
        index = 0;
        int num = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<InfoviewBuildingData> nativeArray1 = infomodeChunk.GetNativeArray<InfoviewBuildingData>(ref this.m_InfoviewBuildingType);
          if (nativeArray1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfomodeActive> nativeArray2 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              InfomodeActive infomodeActive = nativeArray2[index2];
              int priority = infomodeActive.m_Priority;
              // ISSUE: reference to a compiler-generated method
              if (priority < num && this.HasBuildingColor(nativeArray1[index2], chunk))
              {
                index = infomodeActive.m_Index;
                num = priority;
              }
            }
          }
        }
        return num != int.MaxValue;
      }

      private bool HasBuildingColor(InfoviewBuildingData infoviewBuildingData, ArchetypeChunk chunk)
      {
        switch (infoviewBuildingData.m_Type)
        {
          case BuildingType.Hospital:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.Hospital>(ref this.m_HospitalType);
          case BuildingType.PowerPlant:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<ElectricityProducer>(ref this.m_ElectricityProducerType);
          case BuildingType.Transformer:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.Transformer>(ref this.m_TransformerType);
          case BuildingType.FreshWaterBuilding:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.WaterPumpingStation>(ref this.m_WaterPumpingStationType) || chunk.Has<Game.Buildings.WaterTower>(ref this.m_WaterTowerType);
          case BuildingType.SewageBuilding:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.SewageOutlet>(ref this.m_SewageOutletType) || chunk.Has<WastewaterTreatmentPlant>(ref this.m_WastewaterTreatmentPlantType);
          case BuildingType.TransportDepot:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.TransportDepot>(ref this.m_TransportDepotType);
          case BuildingType.TransportStation:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.TransportStation>(ref this.m_TransportStationType);
          case BuildingType.GarbageFacility:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.GarbageFacility>(ref this.m_GarbageFacilityType);
          case BuildingType.FireStation:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.FireStation>(ref this.m_FireStationType);
          case BuildingType.PoliceStation:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.PoliceStation>(ref this.m_PoliceStationType);
          case BuildingType.RoadMaintenanceDepot:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<RoadMaintenance>(ref this.m_RoadMaintenanceType);
          case BuildingType.PostFacility:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.PostFacility>(ref this.m_PostFacilityType);
          case BuildingType.TelecomFacility:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.TelecomFacility>(ref this.m_TelecomFacilityType);
          case BuildingType.School:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.School>(ref this.m_SchoolType);
          case BuildingType.EmergencyShelter:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.EmergencyShelter>(ref this.m_EmergencyShelterType);
          case BuildingType.DisasterFacility:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.DisasterFacility>(ref this.m_DisasterFacilityType);
          case BuildingType.FirewatchTower:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.FirewatchTower>(ref this.m_FirewatchTowerType);
          case BuildingType.Park:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.Park>(ref this.m_ParkType) || chunk.Has<AttractivenessProvider>(ref this.m_AttractivenessProviderType);
          case BuildingType.DeathcareFacility:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.DeathcareFacility>(ref this.m_DeathcareFacilityType);
          case BuildingType.Prison:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.Prison>(ref this.m_PrisonType);
          case BuildingType.AdminBuilding:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<AdminBuilding>(ref this.m_AdminBuildingType);
          case BuildingType.WelfareOffice:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.WelfareOffice>(ref this.m_WelfareOfficeType);
          case BuildingType.ResearchFacility:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.ResearchFacility>(ref this.m_ResearchFacilityType);
          case BuildingType.ParkMaintenanceDepot:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<ParkMaintenance>(ref this.m_ParkMaintenanceType);
          case BuildingType.ParkingFacility:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.ParkingFacility>(ref this.m_ParkingFacilityType);
          case BuildingType.Battery:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.Battery>(ref this.m_BatteryType);
          case BuildingType.ResidentialBuilding:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<ResidentialProperty>(ref this.m_ResidentialPropertyType);
          case BuildingType.CommercialBuilding:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<CommercialProperty>(ref this.m_CommercialPropertyType);
          case BuildingType.IndustrialBuilding:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<IndustrialProperty>(ref this.m_IndustrialPropertyType) && !chunk.Has<OfficeProperty>(ref this.m_OfficePropertyType);
          case BuildingType.OfficeBuilding:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<OfficeProperty>(ref this.m_OfficePropertyType);
          case BuildingType.SignatureResidential:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<ResidentialProperty>(ref this.m_ResidentialPropertyType) && chunk.Has<Game.Objects.UniqueObject>(ref this.m_UniqueObjectType);
          case BuildingType.ExtractorBuilding:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<ExtractorProperty>(ref this.m_ExtractorPropertyType);
          case BuildingType.SignatureCommercial:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<CommercialProperty>(ref this.m_CommercialPropertyType) && chunk.Has<Game.Objects.UniqueObject>(ref this.m_UniqueObjectType);
          case BuildingType.SignatureIndustrial:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<IndustrialProperty>(ref this.m_IndustrialPropertyType) && chunk.Has<Game.Objects.UniqueObject>(ref this.m_UniqueObjectType) && !chunk.Has<OfficeProperty>(ref this.m_OfficePropertyType);
          case BuildingType.SignatureOffice:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<OfficeProperty>(ref this.m_OfficePropertyType) && chunk.Has<Game.Objects.UniqueObject>(ref this.m_UniqueObjectType);
          case BuildingType.LandValueSources:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<CommercialProperty>(ref this.m_CommercialPropertyType) || chunk.Has<Game.Buildings.School>(ref this.m_SchoolType) || chunk.Has<Game.Buildings.Hospital>(ref this.m_HospitalType) || chunk.Has<AttractivenessProvider>(ref this.m_AttractivenessProviderType);
          default:
            return false;
        }
      }

      private bool GetBuildingStatusType(
        ArchetypeChunk chunk,
        out InfoviewBuildingStatusData statusData,
        out InfomodeActive activeData)
      {
        statusData = new InfoviewBuildingStatusData();
        activeData = new InfomodeActive();
        int num = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<InfoviewBuildingStatusData> nativeArray1 = infomodeChunk.GetNativeArray<InfoviewBuildingStatusData>(ref this.m_InfoviewBuildingStatusType);
          if (nativeArray1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfomodeActive> nativeArray2 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              InfomodeActive infomodeActive = nativeArray2[index2];
              int priority = infomodeActive.m_Priority;
              if (priority < num)
              {
                InfoviewBuildingStatusData buildingStatusData = nativeArray1[index2];
                // ISSUE: reference to a compiler-generated method
                if (this.HasBuildingStatus(nativeArray1[index2], chunk))
                {
                  statusData = buildingStatusData;
                  activeData = infomodeActive;
                  num = priority;
                }
              }
            }
          }
        }
        return num != int.MaxValue;
      }

      private bool HasBuildingStatus(
        InfoviewBuildingStatusData infoviewBuildingStatusData,
        ArchetypeChunk chunk)
      {
        switch (infoviewBuildingStatusData.m_Type)
        {
          case BuildingStatusType.CrimeProbability:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<CrimeProducer>(ref this.m_CrimeProducerType);
          case BuildingStatusType.MailAccumulation:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<MailProducer>(ref this.m_MailProducerType);
          case BuildingStatusType.Wealth:
          case BuildingStatusType.Education:
          case BuildingStatusType.Health:
          case BuildingStatusType.Age:
          case BuildingStatusType.Happiness:
          case BuildingStatusType.Wellbeing:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<ResidentialProperty>(ref this.m_ResidentialPropertyType);
          case BuildingStatusType.Level:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<BuildingCondition>(ref this.m_BuildingConditionType) && !chunk.Has<Abandoned>(ref this.m_AbandonedType) && !chunk.Has<Destroyed>(ref this.m_DestroyedType) && !chunk.Has<Game.Objects.UniqueObject>(ref this.m_UniqueObjectType);
          case BuildingStatusType.GarbageAccumulation:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<GarbageProducer>(ref this.m_GarbageProducerType);
          case BuildingStatusType.Profitability:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return (chunk.Has<CommercialProperty>(ref this.m_CommercialPropertyType) || chunk.Has<IndustrialProperty>(ref this.m_IndustrialPropertyType)) && !chunk.Has<StorageProperty>(ref this.m_StoragePropertyType);
          case BuildingStatusType.LeisureProvider:
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<Game.Buildings.LeisureProvider>(ref this.m_LeisureProviderType))
              return true;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Renter>(ref this.m_RenterType) && chunk.Has<CommercialProperty>(ref this.m_CommercialPropertyType);
          case BuildingStatusType.ElectricityConsumption:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<ElectricityConsumer>(ref this.m_ElectricityConsumerType);
          case BuildingStatusType.NetworkQuality:
            return true;
          case BuildingStatusType.AirPollutionSource:
          case BuildingStatusType.GroundPollutionSource:
          case BuildingStatusType.NoisePollutionSource:
            return true;
          case BuildingStatusType.LodgingProvider:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<CommercialProperty>(ref this.m_CommercialPropertyType);
          case BuildingStatusType.WaterPollutionSource:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Buildings.SewageOutlet>(ref this.m_SewageOutletType);
          case BuildingStatusType.LandValue:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Renter>(ref this.m_RenterType);
          case BuildingStatusType.WaterConsumption:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<WaterConsumer>(ref this.m_WaterConsumerType);
          case BuildingStatusType.ResidentialBuilding:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<ResidentialProperty>(ref this.m_ResidentialPropertyType);
          case BuildingStatusType.CommercialBuilding:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<CommercialProperty>(ref this.m_CommercialPropertyType);
          case BuildingStatusType.IndustrialBuilding:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<IndustrialProperty>(ref this.m_IndustrialPropertyType) && !chunk.Has<OfficeProperty>(ref this.m_OfficePropertyType);
          case BuildingStatusType.OfficeBuilding:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<OfficeProperty>(ref this.m_OfficePropertyType);
          case BuildingStatusType.SignatureResidential:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<ResidentialProperty>(ref this.m_ResidentialPropertyType) && chunk.Has<Game.Objects.UniqueObject>(ref this.m_UniqueObjectType);
          case BuildingStatusType.SignatureCommercial:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<CommercialProperty>(ref this.m_CommercialPropertyType) && chunk.Has<Game.Objects.UniqueObject>(ref this.m_UniqueObjectType);
          case BuildingStatusType.SignatureIndustrial:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<IndustrialProperty>(ref this.m_IndustrialPropertyType) && chunk.Has<Game.Objects.UniqueObject>(ref this.m_UniqueObjectType) && !chunk.Has<OfficeProperty>(ref this.m_OfficePropertyType);
          case BuildingStatusType.SignatureOffice:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<OfficeProperty>(ref this.m_OfficePropertyType) && chunk.Has<Game.Objects.UniqueObject>(ref this.m_UniqueObjectType);
          default:
            return false;
        }
      }

      private void GetBuildingStatusColors(
        NativeArray<Game.Objects.Color> results,
        ArchetypeChunk chunk,
        InfoviewBuildingStatusData statusData,
        InfomodeActive activeData)
      {
        switch (statusData.m_Type)
        {
          case BuildingStatusType.CrimeProbability:
            // ISSUE: reference to a compiler-generated field
            NativeArray<CrimeProducer> nativeArray1 = chunk.GetNativeArray<CrimeProducer>(ref this.m_CrimeProducerType);
            for (int index = 0; index < nativeArray1.Length; ++index)
              results[index] = new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, nativeArray1[index].m_Crime) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
            break;
          case BuildingStatusType.MailAccumulation:
            // ISSUE: reference to a compiler-generated field
            NativeArray<MailProducer> nativeArray2 = chunk.GetNativeArray<MailProducer>(ref this.m_MailProducerType);
            for (int index = 0; index < nativeArray2.Length; ++index)
            {
              MailProducer mailProducer = nativeArray2[index];
              float status = (float) math.max((int) mailProducer.m_SendingMail, mailProducer.receivingMail);
              results[index] = new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, status) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
            }
            break;
          case BuildingStatusType.Wealth:
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Renter> bufferAccessor1 = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
            for (int index1 = 0; index1 < bufferAccessor1.Length; ++index1)
            {
              float2 float2 = new float2();
              DynamicBuffer<Renter> dynamicBuffer = bufferAccessor1[index1];
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                Entity renter = dynamicBuffer[index2].m_Renter;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Households.HasComponent(renter) && this.m_HouseholdCitizens.HasBuffer(renter) && this.m_ResourceBuffs.HasBuffer(renter))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  int householdTotalWealth = EconomyUtils.GetHouseholdTotalWealth(this.m_Households[renter], this.m_ResourceBuffs[renter]);
                  float2.x += (float) householdTotalWealth;
                  ++float2.y;
                }
              }
              results[index1] = (double) float2.y <= 0.0 ? new Game.Objects.Color() : new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, float2.x / float2.y) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
            }
            break;
          case BuildingStatusType.Education:
          case BuildingStatusType.Health:
          case BuildingStatusType.Age:
          case BuildingStatusType.Happiness:
          case BuildingStatusType.Wellbeing:
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Renter> bufferAccessor2 = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
            for (int index3 = 0; index3 < bufferAccessor2.Length; ++index3)
            {
              DynamicBuffer<Renter> dynamicBuffer = bufferAccessor2[index3];
              float2 float2 = new float2();
              for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
              {
                Entity renter = dynamicBuffer[index4].m_Renter;
                // ISSUE: reference to a compiler-generated field
                if (this.m_HouseholdCitizens.HasBuffer(renter))
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<HouseholdCitizen> householdCitizen = this.m_HouseholdCitizens[renter];
                  for (int index5 = 0; index5 < householdCitizen.Length; ++index5)
                  {
                    Entity citizen1 = householdCitizen[index5].m_Citizen;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_Citizens.HasComponent(citizen1))
                    {
                      // ISSUE: reference to a compiler-generated field
                      Citizen citizen2 = this.m_Citizens[citizen1];
                      CitizenAge age = citizen2.GetAge();
                      switch (statusData.m_Type)
                      {
                        case BuildingStatusType.Education:
                          if (age == CitizenAge.Adult)
                          {
                            float2 += new float2((float) citizen2.GetEducationLevel(), 1f);
                            continue;
                          }
                          continue;
                        case BuildingStatusType.Health:
                          float2 += new float2((float) citizen2.m_Health, 1f);
                          continue;
                        case BuildingStatusType.Age:
                          float2 += new float2((float) age, 1f);
                          continue;
                        case BuildingStatusType.Happiness:
                          float2 += new float2((float) citizen2.Happiness, 1f);
                          continue;
                        case BuildingStatusType.Wellbeing:
                          float2 += new float2((float) citizen2.m_WellBeing, 1f);
                          continue;
                        default:
                          continue;
                      }
                    }
                  }
                }
              }
              results[index3] = (double) float2.y <= 0.0 ? new Game.Objects.Color() : new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, float2.x / float2.y) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
            }
            break;
          case BuildingStatusType.Level:
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            for (int index = 0; index < nativeArray3.Length; ++index)
            {
              Entity prefab = nativeArray3[index].m_Prefab;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SpawnableBuildingDatas.HasComponent(prefab))
              {
                // ISSUE: reference to a compiler-generated field
                float level = (float) this.m_SpawnableBuildingDatas[prefab].m_Level;
                results[index] = new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, level) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
              }
              else
                results[index] = new Game.Objects.Color();
            }
            break;
          case BuildingStatusType.GarbageAccumulation:
            // ISSUE: reference to a compiler-generated field
            NativeArray<GarbageProducer> nativeArray4 = chunk.GetNativeArray<GarbageProducer>(ref this.m_GarbageProducerType);
            for (int index = 0; index < nativeArray4.Length; ++index)
              results[index] = new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, (float) nativeArray4[index].m_Garbage) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
            break;
          case BuildingStatusType.Profitability:
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Renter> bufferAccessor3 = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
            for (int index6 = 0; index6 < bufferAccessor3.Length; ++index6)
            {
              DynamicBuffer<Renter> dynamicBuffer = bufferAccessor3[index6];
              bool flag = false;
              for (int index7 = 0; index7 < dynamicBuffer.Length; ++index7)
              {
                Entity renter = dynamicBuffer[index7].m_Renter;
                // ISSUE: reference to a compiler-generated field
                if (this.m_Profitabilities.HasComponent(renter))
                {
                  // ISSUE: reference to a compiler-generated field
                  results[index6] = new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, (float) this.m_Profitabilities[renter].m_Profitability) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
                  flag = true;
                  break;
                }
              }
              if (!flag)
                results[index6] = new Game.Objects.Color();
            }
            break;
          case BuildingStatusType.LeisureProvider:
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<Renter>(ref this.m_RenterType))
            {
              // ISSUE: reference to a compiler-generated field
              BufferAccessor<Renter> bufferAccessor4 = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
              for (int index = 0; index < bufferAccessor4.Length; ++index)
              {
                DynamicBuffer<Renter> dynamicBuffer = bufferAccessor4[index];
                bool flag = false;
                foreach (Renter renter in dynamicBuffer)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_LeisureProviders.HasComponent(renter.m_Renter))
                  {
                    results[index] = new Game.Objects.Color((byte) activeData.m_Index, byte.MaxValue);
                    flag = true;
                  }
                }
                if (!flag)
                  results[index] = new Game.Objects.Color();
              }
              break;
            }
            for (int index = 0; index < chunk.Count; ++index)
              results[index] = new Game.Objects.Color((byte) activeData.m_Index, byte.MaxValue);
            break;
          case BuildingStatusType.ElectricityConsumption:
            // ISSUE: reference to a compiler-generated field
            NativeArray<ElectricityConsumer> nativeArray5 = chunk.GetNativeArray<ElectricityConsumer>(ref this.m_ElectricityConsumerType);
            for (int index = 0; index < nativeArray5.Length; ++index)
            {
              ElectricityConsumer electricityConsumer = nativeArray5[index];
              if (electricityConsumer.m_WantedConsumption > 0)
              {
                // ISSUE: reference to a compiler-generated field
                if ((int) electricityConsumer.m_CooldownCounter >= (int) DispatchElectricitySystem.kAlertCooldown)
                {
                  results[index] = new Game.Objects.Color((byte) activeData.m_Index, (byte) 0);
                }
                else
                {
                  float status = math.log10((float) math.max(electricityConsumer.m_WantedConsumption, 1)) / math.log10(20000f);
                  results[index] = new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, status) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
                }
              }
              else
                results[index] = new Game.Objects.Color();
            }
            break;
          case BuildingStatusType.NetworkQuality:
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Objects.Transform> nativeArray6 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray7 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            for (int index = 0; index < nativeArray6.Length; ++index)
            {
              Game.Objects.Transform transform = nativeArray6[index];
              // ISSUE: reference to a compiler-generated field
              if ((this.m_BuildingData[nativeArray7[index].m_Prefab].m_Flags & Game.Prefabs.BuildingFlags.RequireRoad) != (Game.Prefabs.BuildingFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                float status = TelecomCoverage.SampleNetworkQuality(this.m_TelecomCoverageData, transform.m_Position);
                results[index] = new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, status) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
              }
              else
                results[index] = new Game.Objects.Color();
            }
            break;
          case BuildingStatusType.AirPollutionSource:
          case BuildingStatusType.GroundPollutionSource:
          case BuildingStatusType.NoisePollutionSource:
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray8 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            bool flag1 = chunk.Has<Destroyed>(ref this.m_DestroyedType);
            // ISSUE: reference to a compiler-generated field
            bool flag2 = chunk.Has<Abandoned>(ref this.m_AbandonedType);
            // ISSUE: reference to a compiler-generated field
            bool flag3 = chunk.Has<Game.Buildings.Park>(ref this.m_ParkType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Efficiency> bufferAccessor5 = chunk.GetBufferAccessor<Efficiency>(ref this.m_BuildingEfficiencyType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Renter> bufferAccessor6 = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<InstalledUpgrade> bufferAccessor7 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
            DynamicBuffer<CityModifier> bufferData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CityModifiers.TryGetBuffer(this.m_City, out bufferData);
            for (int index = 0; index < nativeArray8.Length; ++index)
            {
              Entity prefab = nativeArray8[index].m_Prefab;
              float efficiency1 = BuildingUtils.GetEfficiency(bufferAccessor5, index);
              DynamicBuffer<Renter> dynamicBuffer1 = bufferAccessor6.Length != 0 ? bufferAccessor6[index] : new DynamicBuffer<Renter>();
              DynamicBuffer<InstalledUpgrade> dynamicBuffer2 = bufferAccessor7.Length != 0 ? bufferAccessor7[index] : new DynamicBuffer<InstalledUpgrade>();
              int num1 = flag1 ? 1 : 0;
              int num2 = flag2 ? 1 : 0;
              int num3 = flag3 ? 1 : 0;
              double efficiency2 = (double) efficiency1;
              DynamicBuffer<Renter> renters = dynamicBuffer1;
              DynamicBuffer<InstalledUpgrade> installedUpgrades = dynamicBuffer2;
              // ISSUE: reference to a compiler-generated field
              PollutionParameterData pollutionParameters = this.m_PollutionParameters;
              DynamicBuffer<CityModifier> cityModifiers = bufferData;
              // ISSUE: reference to a compiler-generated field
              ref ComponentLookup<PrefabRef> local1 = ref this.m_Prefabs;
              // ISSUE: reference to a compiler-generated field
              ref ComponentLookup<Game.Prefabs.BuildingData> local2 = ref this.m_BuildingData;
              // ISSUE: reference to a compiler-generated field
              ref ComponentLookup<SpawnableBuildingData> local3 = ref this.m_SpawnableBuildingDatas;
              // ISSUE: reference to a compiler-generated field
              ref ComponentLookup<PollutionData> local4 = ref this.m_PollutionData;
              // ISSUE: reference to a compiler-generated field
              ref ComponentLookup<PollutionModifierData> local5 = ref this.m_PollutionModifierData;
              // ISSUE: reference to a compiler-generated field
              ref ComponentLookup<ZoneData> local6 = ref this.m_ZoneDatas;
              // ISSUE: reference to a compiler-generated field
              ref BufferLookup<Employee> local7 = ref this.m_Employees;
              // ISSUE: reference to a compiler-generated field
              ref BufferLookup<HouseholdCitizen> local8 = ref this.m_HouseholdCitizens;
              // ISSUE: reference to a compiler-generated field
              ref ComponentLookup<Citizen> local9 = ref this.m_Citizens;
              // ISSUE: reference to a compiler-generated method
              float status = BuildingPollutionAddSystem.GetBuildingPollution(prefab, num1 != 0, num2 != 0, num3 != 0, (float) efficiency2, renters, installedUpgrades, pollutionParameters, cityModifiers, ref local1, ref local2, ref local3, ref local4, ref local5, ref local6, ref local7, ref local8, ref local9).GetValue(statusData.m_Type);
              results[index] = (double) status <= 0.0 ? new Game.Objects.Color() : new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, status) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
            }
            break;
          case BuildingStatusType.LodgingProvider:
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Renter> bufferAccessor8 = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
            for (int index = 0; index < bufferAccessor8.Length; ++index)
            {
              DynamicBuffer<Renter> dynamicBuffer = bufferAccessor8[index];
              bool flag4 = false;
              foreach (Renter renter in dynamicBuffer)
              {
                PrefabRef componentData1;
                IndustrialProcessData componentData2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Prefabs.TryGetComponent(renter.m_Renter, out componentData1) && this.m_IndustrialProcessData.TryGetComponent(componentData1.m_Prefab, out componentData2) && (componentData2.m_Output.m_Resource & Resource.Lodging) != Resource.NoResource)
                {
                  results[index] = new Game.Objects.Color((byte) activeData.m_Index, byte.MaxValue);
                  flag4 = true;
                }
              }
              if (!flag4)
                results[index] = new Game.Objects.Color();
            }
            break;
          case BuildingStatusType.WaterPollutionSource:
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray9 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            for (int index = 0; index < nativeArray9.Length; ++index)
            {
              SewageOutletData componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SewageOutletData.TryGetComponent(nativeArray9[index].m_Prefab, out componentData))
              {
                float status = 1f - componentData.m_Purification;
                results[index] = new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, status) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
              }
              else
                results[index] = new Game.Objects.Color();
            }
            break;
          case BuildingStatusType.WaterConsumption:
            // ISSUE: reference to a compiler-generated field
            NativeArray<WaterConsumer> nativeArray10 = chunk.GetNativeArray<WaterConsumer>(ref this.m_WaterConsumerType);
            for (int index = 0; index < nativeArray10.Length; ++index)
            {
              WaterConsumer waterConsumer = nativeArray10[index];
              if (waterConsumer.m_WantedConsumption > 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((int) waterConsumer.m_FreshCooldownCounter >= (int) DispatchWaterSystem.kAlertCooldown || (int) waterConsumer.m_SewageCooldownCounter >= (int) DispatchWaterSystem.kAlertCooldown)
                {
                  results[index] = new Game.Objects.Color((byte) activeData.m_Index, (byte) 0);
                }
                else
                {
                  float status = math.log10((float) math.max(waterConsumer.m_WantedConsumption, 1)) / math.log10(20000f);
                  results[index] = new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, status) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
                }
              }
              else
                results[index] = new Game.Objects.Color();
            }
            break;
          case BuildingStatusType.ResidentialBuilding:
          case BuildingStatusType.CommercialBuilding:
          case BuildingStatusType.IndustrialBuilding:
          case BuildingStatusType.OfficeBuilding:
          case BuildingStatusType.SignatureResidential:
          case BuildingStatusType.SignatureCommercial:
          case BuildingStatusType.SignatureIndustrial:
          case BuildingStatusType.SignatureOffice:
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Renter> bufferAccessor9 = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray11 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            for (int index = 0; index < bufferAccessor9.Length; ++index)
            {
              DynamicBuffer<Renter> dynamicBuffer = bufferAccessor9[index];
              PrefabRef prefabRef = nativeArray11[index];
              float num = statusData.m_Range.max;
              BuildingPropertyData componentData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (chunk.Has<ResidentialProperty>(ref this.m_ResidentialPropertyType) && this.m_BuildingPropertyData.TryGetComponent(prefabRef.m_Prefab, out componentData))
                num = (float) componentData.m_ResidentialProperties;
              Bounds1 bounds1;
              bounds1.min = statusData.m_Range.min;
              bounds1.max = num;
              statusData.m_Range = bounds1;
              results[index] = new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, (float) dynamicBuffer.Length) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
            }
            break;
        }
      }

      private bool GetTransportStopColor(ArchetypeChunk chunk, out int index)
      {
        index = 0;
        int num = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<InfoviewTransportStopData> nativeArray1 = infomodeChunk.GetNativeArray<InfoviewTransportStopData>(ref this.m_InfoviewTransportStopType);
          if (nativeArray1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfomodeActive> nativeArray2 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              InfomodeActive infomodeActive = nativeArray2[index2];
              int priority = infomodeActive.m_Priority;
              // ISSUE: reference to a compiler-generated method
              if (priority < num && this.HasTransportStopColor(nativeArray1[index2], chunk))
              {
                index = infomodeActive.m_Index;
                num = priority;
              }
            }
          }
        }
        return num != int.MaxValue;
      }

      private bool HasTransportStopColor(
        InfoviewTransportStopData infoviewTransportStopData,
        ArchetypeChunk chunk)
      {
        switch (infoviewTransportStopData.m_Type)
        {
          case TransportType.Bus:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<BusStop>(ref this.m_BusStopType);
          case TransportType.Train:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<TrainStop>(ref this.m_TrainStopType);
          case TransportType.Taxi:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<TaxiStand>(ref this.m_TaxiStandType);
          case TransportType.Tram:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<TramStop>(ref this.m_TramStopType);
          case TransportType.Ship:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<ShipStop>(ref this.m_ShipStopType);
          case TransportType.Post:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Routes.MailBox>(ref this.m_MailBoxType);
          case TransportType.Airplane:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<AirplaneStop>(ref this.m_AirplaneStopType);
          case TransportType.Subway:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<SubwayStop>(ref this.m_SubwayStopType);
          default:
            return false;
        }
      }

      private bool GetVehicleColor(ArchetypeChunk chunk, out int index)
      {
        index = 0;
        int num = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<InfoviewVehicleData> nativeArray1 = infomodeChunk.GetNativeArray<InfoviewVehicleData>(ref this.m_InfoviewVehicleType);
          if (nativeArray1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfomodeActive> nativeArray2 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              InfomodeActive infomodeActive = nativeArray2[index2];
              int priority = infomodeActive.m_Priority;
              // ISSUE: reference to a compiler-generated method
              if (priority < num && this.HasVehicleColor(nativeArray1[index2], chunk))
              {
                index = infomodeActive.m_Index;
                num = priority;
              }
            }
          }
        }
        return num != int.MaxValue;
      }

      private bool HasVehicleColor(InfoviewVehicleData infoviewVehicleData, ArchetypeChunk chunk)
      {
        switch (infoviewVehicleData.m_Type)
        {
          case VehicleType.PassengerTransport:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<PassengerTransport>(ref this.m_PassengerTransportType);
          case VehicleType.CargoTransport:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Vehicles.CargoTransport>(ref this.m_CargoTransportType);
          case VehicleType.Taxi:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Vehicles.Taxi>(ref this.m_TaxiType);
          case VehicleType.ParkMaintenance:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<ParkMaintenanceVehicle>(ref this.m_ParkMaintenanceVehicleType);
          case VehicleType.RoadMaintenance:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<RoadMaintenanceVehicle>(ref this.m_RoadMaintenanceVehicleType);
          case VehicleType.Ambulance:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Vehicles.Ambulance>(ref this.m_AmbulanceType);
          case VehicleType.EvacuatingTransport:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<EvacuatingTransport>(ref this.m_EvacuatingTransportType);
          case VehicleType.FireEngine:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Vehicles.FireEngine>(ref this.m_FireEngineType);
          case VehicleType.GarbageTruck:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Vehicles.GarbageTruck>(ref this.m_GarbageTruckType);
          case VehicleType.Hearse:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Vehicles.Hearse>(ref this.m_HearseType);
          case VehicleType.PoliceCar:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Vehicles.PoliceCar>(ref this.m_PoliceCarType);
          case VehicleType.PostVan:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Vehicles.PostVan>(ref this.m_PostVanType);
          case VehicleType.PrisonerTransport:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<PrisonerTransport>(ref this.m_PrisonerTransportType);
          default:
            return false;
        }
      }

      private bool GetObjectStatusType(
        ArchetypeChunk chunk,
        bool isSubBuilding,
        out InfoviewObjectStatusData statusData,
        out InfomodeActive activeData)
      {
        statusData = new InfoviewObjectStatusData();
        activeData = new InfomodeActive();
        int num = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<InfoviewObjectStatusData> nativeArray1 = infomodeChunk.GetNativeArray<InfoviewObjectStatusData>(ref this.m_InfoviewObjectStatusType);
          if (nativeArray1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfomodeActive> nativeArray2 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              InfomodeActive infomodeActive = nativeArray2[index2];
              int priority = infomodeActive.m_Priority;
              if (priority < num)
              {
                InfoviewObjectStatusData objectStatusData = nativeArray1[index2];
                // ISSUE: reference to a compiler-generated method
                if (this.HasObjectStatus(nativeArray1[index2], chunk, isSubBuilding))
                {
                  statusData = objectStatusData;
                  activeData = infomodeActive;
                  num = priority;
                }
              }
            }
          }
        }
        return num != int.MaxValue;
      }

      private bool HasObjectStatus(
        InfoviewObjectStatusData infoviewObjectStatusData,
        ArchetypeChunk chunk,
        bool isSubBuilding)
      {
        switch (infoviewObjectStatusData.m_Type)
        {
          case ObjectStatusType.WoodResource:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Tree>(ref this.m_TreeType);
          case ObjectStatusType.FireHazard:
            if (isSubBuilding)
              return false;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Building>(ref this.m_BuildingType) || chunk.Has<Tree>(ref this.m_TreeType);
          case ObjectStatusType.Damage:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Damaged>(ref this.m_DamagedType);
          case ObjectStatusType.Destroyed:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Destroyed>(ref this.m_DestroyedType);
          case ObjectStatusType.ExtractorPlaceholder:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Placeholder>(ref this.m_PlaceholderType);
          case ObjectStatusType.Tourist:
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<Game.Creatures.Resident>(ref this.m_ResidentType))
              return true;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Vehicle>(ref this.m_VehicleType) && chunk.Has<Passenger>(ref this.m_PassengerType);
          default:
            return false;
        }
      }

      private void GetObjectStatusColors(
        NativeArray<Game.Objects.Color> results,
        ArchetypeChunk chunk,
        InfoviewObjectStatusData statusData,
        InfomodeActive activeData)
      {
        switch (statusData.m_Type)
        {
          case ObjectStatusType.WoodResource:
            // ISSUE: reference to a compiler-generated field
            NativeArray<Tree> nativeArray1 = chunk.GetNativeArray<Tree>(ref this.m_TreeType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Plant> nativeArray2 = chunk.GetNativeArray<Plant>(ref this.m_PlantType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Damaged> nativeArray3 = chunk.GetNativeArray<Damaged>(ref this.m_DamagedType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            for (int index = 0; index < nativeArray4.Length; ++index)
            {
              Tree tree = nativeArray1[index];
              Plant plant = nativeArray2[index];
              PrefabRef prefabRef = nativeArray4[index];
              Damaged damaged;
              CollectionUtils.TryGet<Damaged>(nativeArray3, index, out damaged);
              // ISSUE: reference to a compiler-generated field
              if (this.m_TreeData.HasComponent(prefabRef.m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                TreeData treeData = this.m_TreeData[prefabRef.m_Prefab];
                float woodAmount = ObjectUtils.CalculateWoodAmount(tree, plant, damaged, treeData);
                results[index] = new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, woodAmount) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
              }
              else
                results[index] = new Game.Objects.Color();
            }
            break;
          case ObjectStatusType.FireHazard:
            // ISSUE: reference to a compiler-generated field
            NativeArray<Building> nativeArray5 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray6 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            float fireHazard;
            if (nativeArray5.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<CurrentDistrict> nativeArray7 = chunk.GetNativeArray<CurrentDistrict>(ref this.m_CurrentDistrictType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<Damaged> nativeArray8 = chunk.GetNativeArray<Damaged>(ref this.m_DamagedType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<UnderConstruction> nativeArray9 = chunk.GetNativeArray<UnderConstruction>(ref this.m_UnderConstructionType);
              for (int index = 0; index < nativeArray5.Length; ++index)
              {
                PrefabRef prefabRef = nativeArray6[index];
                Building building = nativeArray5[index];
                CurrentDistrict currentDistrict = nativeArray7[index];
                Damaged damaged;
                CollectionUtils.TryGet<Damaged>(nativeArray8, index, out damaged);
                UnderConstruction underConstruction;
                if (!CollectionUtils.TryGet<UnderConstruction>(nativeArray9, index, out underConstruction))
                  underConstruction = new UnderConstruction()
                  {
                    m_Progress = byte.MaxValue
                  };
                float riskFactor;
                // ISSUE: reference to a compiler-generated field
                results[index] = !this.m_FireHazardData.GetFireHazard(prefabRef, building, currentDistrict, damaged, underConstruction, out fireHazard, out riskFactor) ? new Game.Objects.Color() : new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, riskFactor) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
              }
              break;
            }
            // ISSUE: reference to a compiler-generated field
            NativeArray<Damaged> nativeArray10 = chunk.GetNativeArray<Damaged>(ref this.m_DamagedType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Objects.Transform> nativeArray11 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
            for (int index = 0; index < nativeArray11.Length; ++index)
            {
              PrefabRef prefabRef = nativeArray6[index];
              Game.Objects.Transform transform = nativeArray11[index];
              Damaged damaged;
              CollectionUtils.TryGet<Damaged>(nativeArray10, index, out damaged);
              float riskFactor;
              // ISSUE: reference to a compiler-generated field
              results[index] = !this.m_FireHazardData.GetFireHazard(prefabRef, new Tree(), transform, damaged, out fireHazard, out riskFactor) ? new Game.Objects.Color() : new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, riskFactor) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
            }
            break;
          case ObjectStatusType.Damage:
            // ISSUE: reference to a compiler-generated field
            NativeArray<Damaged> nativeArray12 = chunk.GetNativeArray<Damaged>(ref this.m_DamagedType);
            for (int index = 0; index < nativeArray12.Length; ++index)
            {
              float totalDamage = ObjectUtils.GetTotalDamage(nativeArray12[index]);
              results[index] = new Game.Objects.Color((byte) activeData.m_Index, (byte) math.clamp(Mathf.RoundToInt(InfoviewUtils.GetColor(statusData, totalDamage) * (float) byte.MaxValue), 0, (int) byte.MaxValue));
            }
            break;
          case ObjectStatusType.Destroyed:
            for (int index = 0; index < results.Length; ++index)
              results[index] = new Game.Objects.Color((byte) activeData.m_Index, (byte) 0);
            break;
          case ObjectStatusType.ExtractorPlaceholder:
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray13 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            for (int index = 0; index < nativeArray13.Length; ++index)
            {
              PlaceholderBuildingData componentData;
              // ISSUE: reference to a compiler-generated field
              results[index] = !this.m_PlaceholderBuildingData.TryGetComponent(nativeArray13[index].m_Prefab, out componentData) || componentData.m_Type != BuildingType.ExtractorBuilding ? new Game.Objects.Color() : new Game.Objects.Color((byte) activeData.m_Index, byte.MaxValue);
            }
            break;
          case ObjectStatusType.Tourist:
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Creatures.Resident> nativeArray14 = chunk.GetNativeArray<Game.Creatures.Resident>(ref this.m_ResidentType);
            if (nativeArray14.Length != 0)
            {
              for (int index = 0; index < nativeArray14.Length; ++index)
              {
                Citizen componentData;
                // ISSUE: reference to a compiler-generated field
                results[index] = !this.m_Citizens.TryGetComponent(nativeArray14[index].m_Citizen, out componentData) || (componentData.m_State & CitizenFlags.Tourist) == CitizenFlags.None ? new Game.Objects.Color() : new Game.Objects.Color((byte) activeData.m_Index, byte.MaxValue);
              }
              break;
            }
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Passenger> bufferAccessor = chunk.GetBufferAccessor<Passenger>(ref this.m_PassengerType);
            for (int index = 0; index < bufferAccessor.Length; ++index)
            {
              DynamicBuffer<Passenger> dynamicBuffer = bufferAccessor[index];
              bool flag = false;
              foreach (Passenger passenger in dynamicBuffer)
              {
                Game.Creatures.Resident componentData1;
                Citizen componentData2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_ResidentData.TryGetComponent(passenger.m_Passenger, out componentData1) && this.m_Citizens.TryGetComponent(componentData1.m_Citizen, out componentData2) && (componentData2.m_State & CitizenFlags.Tourist) != CitizenFlags.None)
                {
                  results[index] = new Game.Objects.Color((byte) activeData.m_Index, byte.MaxValue);
                  flag = true;
                  break;
                }
              }
              if (!flag)
                results[index] = new Game.Objects.Color();
            }
            break;
        }
      }

      private bool GetNetStatusColors(NativeArray<Game.Objects.Color> results, ArchetypeChunk chunk)
      {
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        int num5 = int.MaxValue;
        int num6 = int.MaxValue;
        int num7 = int.MaxValue;
        int num8 = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<InfoviewNetStatusData> nativeArray1 = infomodeChunk.GetNativeArray<InfoviewNetStatusData>(ref this.m_InfoviewNetStatusType);
          if (nativeArray1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfomodeActive> nativeArray2 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              InfoviewNetStatusData infoviewNetStatusData = nativeArray1[index2];
              InfomodeActive infomodeActive = nativeArray2[index2];
              int priority = infomodeActive.m_Priority;
              switch (infoviewNetStatusData.m_Type)
              {
                case NetStatusType.LowVoltageFlow:
                  if (priority < num5)
                  {
                    num1 = infomodeActive.m_Index;
                    num5 = priority;
                    break;
                  }
                  break;
                case NetStatusType.HighVoltageFlow:
                  if (priority < num6)
                  {
                    num2 = infomodeActive.m_Index;
                    num6 = priority;
                    break;
                  }
                  break;
                case NetStatusType.PipeWaterFlow:
                  if (priority < num7)
                  {
                    num3 = infomodeActive.m_Index;
                    num7 = priority;
                    break;
                  }
                  break;
                case NetStatusType.PipeSewageFlow:
                  if (priority < num8)
                  {
                    num4 = infomodeActive.m_Index;
                    num8 = priority;
                    break;
                  }
                  break;
              }
            }
          }
        }
        if (num1 == 0 && num2 == 0 && num3 == 0 && num4 == 0)
          return false;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index3 = 0; index3 < results.Length; ++index3)
        {
          Game.Prefabs.UtilityObjectData componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabUtilityObjectData.TryGetComponent(nativeArray4[index3].m_Prefab, out componentData1))
          {
            int index4 = 0;
            if ((componentData1.m_UtilityTypes & UtilityTypes.LowVoltageLine) != UtilityTypes.None && num1 != 0)
              index4 = num1;
            else if ((componentData1.m_UtilityTypes & UtilityTypes.HighVoltageLine) != UtilityTypes.None && num2 != 0)
              index4 = num2;
            else if ((componentData1.m_UtilityTypes & UtilityTypes.WaterPipe) != UtilityTypes.None && num3 != 0)
              index4 = num3;
            else if ((componentData1.m_UtilityTypes & UtilityTypes.SewagePipe) != UtilityTypes.None && num4 != 0)
              index4 = num4;
            if (index4 != 0)
            {
              int num9 = 0;
              if (nativeArray3.Length != 0)
              {
                Owner owner = nativeArray3[index3];
                if (index4 == num1 || index4 == num2)
                {
                  ElectricityNodeConnection componentData2;
                  DynamicBuffer<ConnectedFlowEdge> bufferData;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ElectricityNodeConnectionData.TryGetComponent(owner.m_Owner, out componentData2) && this.m_ConnectedFlowEdges.TryGetBuffer(componentData2.m_ElectricityNode, out bufferData))
                  {
                    ElectricityFlowEdgeFlags electricityFlowEdgeFlags = ElectricityFlowEdgeFlags.None;
                    for (int index5 = 0; index5 < bufferData.Length; ++index5)
                    {
                      ElectricityFlowEdge componentData3;
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_ElectricityFlowEdgeData.TryGetComponent(bufferData[index5].m_Edge, out componentData3))
                      {
                        num9 = math.max(num9, math.select(0, 128, componentData3.m_Flow != 0));
                        electricityFlowEdgeFlags |= componentData3.m_Flags;
                      }
                    }
                    int a = math.select(num9, 224, num9 != 0 && (electricityFlowEdgeFlags & ElectricityFlowEdgeFlags.BeyondBottleneck) != 0);
                    num9 = math.select(a, (int) byte.MaxValue, a != 0 && (electricityFlowEdgeFlags & ElectricityFlowEdgeFlags.Bottleneck) != 0);
                  }
                  else
                  {
                    ElectricityConsumer componentData4;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_ElectricityConsumerData.TryGetComponent(owner.m_Owner, out componentData4))
                    {
                      int a = math.max(num9, math.select(0, 128, componentData4.m_FulfilledConsumption != 0));
                      num9 = math.select(a, 224, a != 0 && (componentData4.m_Flags & ElectricityConsumerFlags.BottleneckWarning) != 0);
                    }
                  }
                }
              }
              results[index3] = new Game.Objects.Color((byte) index4, (byte) num9);
              continue;
            }
          }
          results[index3] = new Game.Objects.Color();
        }
        return true;
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
    private struct UpdateMiddleObjectColorsJob : IJobChunk
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_InfomodeChunks;
      [ReadOnly]
      public ComponentTypeHandle<InfomodeActive> m_InfomodeActiveType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewObjectStatusData> m_InfoviewObjectStatusType;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Controller> m_ControllerType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Objects.Color> m_ColorData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Building>(ref this.m_BuildingType))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          // ISSUE: reference to a compiler-generated method
          int2 buildingActiveIndices = this.GetExcludedSubBuildingActiveIndices();
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Owner owner = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            Game.Objects.Color color = this.m_ColorData[entity];
            Game.Objects.Color componentData;
            // ISSUE: reference to a compiler-generated field
            if (color.m_Index == (byte) 0 && this.m_ColorData.TryGetComponent(owner.m_Owner, out componentData) && math.all((int) componentData.m_Index != buildingActiveIndices))
            {
              color.m_Index = componentData.m_Index;
              color.m_Value = componentData.m_Value;
              // ISSUE: reference to a compiler-generated field
              this.m_ColorData[entity] = color;
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Controller> nativeArray3 = chunk.GetNativeArray<Controller>(ref this.m_ControllerType);
          if (nativeArray3.Length != 0)
          {
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              Controller controller = nativeArray3[index];
              if (controller.m_Controller != entity)
              {
                // ISSUE: reference to a compiler-generated field
                Game.Objects.Color componentData = this.m_ColorData[entity];
                // ISSUE: reference to a compiler-generated field
                if (componentData.m_Index == (byte) 0 && this.m_ColorData.TryGetComponent(controller.m_Controller, out componentData))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ColorData[entity] = componentData;
                }
              }
            }
          }
          else
          {
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ColorData[nativeArray1[index]] = new Game.Objects.Color();
            }
          }
        }
      }

      private int2 GetExcludedSubBuildingActiveIndices()
      {
        int2 buildingActiveIndices = (int2) -1;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<InfoviewObjectStatusData> nativeArray1 = infomodeChunk.GetNativeArray<InfoviewObjectStatusData>(ref this.m_InfoviewObjectStatusType);
          if (nativeArray1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfomodeActive> nativeArray2 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              InfomodeActive infomodeActive = nativeArray2[index2];
              switch (nativeArray1[index2].m_Type)
              {
                case ObjectStatusType.Damage:
                  buildingActiveIndices.x = infomodeActive.m_Index;
                  break;
                case ObjectStatusType.Destroyed:
                  buildingActiveIndices.y = infomodeActive.m_Index;
                  break;
              }
            }
          }
        }
        return buildingActiveIndices;
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
    private struct UpdateTempObjectColorsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Objects.Color> m_ColorData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Game.Objects.Color componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ColorData.TryGetComponent(nativeArray2[index].m_Original, out componentData))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ColorData[nativeArray1[index]] = componentData;
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
    private struct UpdateSubObjectColorsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Elevation> m_ElevationType;
      [ReadOnly]
      public ComponentTypeHandle<Tree> m_TreeType;
      [ReadOnly]
      public ComponentTypeHandle<Plant> m_PlantType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Objects.Color> m_ColorData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Tree>(ref this.m_TreeType))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Objects.Elevation> nativeArray3 = chunk.GetNativeArray<Game.Objects.Elevation>(ref this.m_ElevationType);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Owner owner = nativeArray2[index];
            Game.Objects.Elevation componentData1;
            bool flag1 = CollectionUtils.TryGet<Game.Objects.Elevation>(nativeArray3, index, out componentData1) && (componentData1.m_Flags & ElevationFlags.OnGround) == (ElevationFlags) 0;
            // ISSUE: reference to a compiler-generated field
            bool flag2 = flag1 && !this.m_ColorData.HasComponent(owner.m_Owner);
            Owner componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            for (; this.m_OwnerData.TryGetComponent(owner.m_Owner, out componentData2) && !this.m_BuildingData.HasComponent(owner.m_Owner) && !this.m_VehicleData.HasComponent(owner.m_Owner); owner = componentData2)
            {
              if (flag2)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_ColorData.HasComponent(owner.m_Owner))
                {
                  flag2 = false;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  flag1 = ((flag1 ? 1 : 0) & (!this.m_ElevationData.TryGetComponent(owner.m_Owner, out componentData1) ? 0 : ((componentData1.m_Flags & ElevationFlags.OnGround) == (ElevationFlags) 0 ? 1 : 0))) != 0;
                }
              }
            }
            Game.Objects.Color color = new Game.Objects.Color();
            Game.Objects.Color componentData3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ColorData.TryGetComponent(owner.m_Owner, out componentData3) && (flag1 || componentData3.m_SubColor))
              color = componentData3;
            // ISSUE: reference to a compiler-generated field
            this.m_ColorData[entity] = color;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray4 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Owner owner = nativeArray4[index];
            Owner componentData4;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            while (this.m_OwnerData.TryGetComponent(owner.m_Owner, out componentData4) && !this.m_BuildingData.HasComponent(owner.m_Owner) && !this.m_VehicleData.HasComponent(owner.m_Owner))
              owner = componentData4;
            Game.Objects.Color componentData5;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_ColorData[entity] = !this.m_ColorData.TryGetComponent(owner.m_Owner, out componentData5) ? new Game.Objects.Color() : componentData5;
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfomodeActive> __Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewBuildingData> __Game_Prefabs_InfoviewBuildingData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewBuildingStatusData> __Game_Prefabs_InfoviewBuildingStatusData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewTransportStopData> __Game_Prefabs_InfoviewTransportStopData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewVehicleData> __Game_Prefabs_InfoviewVehicleData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewObjectStatusData> __Game_Prefabs_InfoviewObjectStatusData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewNetStatusData> __Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Hospital> __Game_Buildings_Hospital_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityProducer> __Game_Buildings_ElectricityProducer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Transformer> __Game_Buildings_Transformer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.WaterPumpingStation> __Game_Buildings_WaterPumpingStation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.WaterTower> __Game_Buildings_WaterTower_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.SewageOutlet> __Game_Buildings_SewageOutlet_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WastewaterTreatmentPlant> __Game_Buildings_WastewaterTreatmentPlant_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.TransportDepot> __Game_Buildings_TransportDepot_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.TransportStation> __Game_Buildings_TransportStation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.GarbageFacility> __Game_Buildings_GarbageFacility_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.FireStation> __Game_Buildings_FireStation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.PoliceStation> __Game_Buildings_PoliceStation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CrimeProducer> __Game_Buildings_CrimeProducer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<RoadMaintenance> __Game_Buildings_RoadMaintenance_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ParkMaintenance> __Game_Buildings_ParkMaintenance_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.PostFacility> __Game_Buildings_PostFacility_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.TelecomFacility> __Game_Buildings_TelecomFacility_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.School> __Game_Buildings_School_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Park> __Game_Buildings_Park_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<AttractivenessProvider> __Game_Buildings_AttractivenessProvider_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.EmergencyShelter> __Game_Buildings_EmergencyShelter_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.DisasterFacility> __Game_Buildings_DisasterFacility_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.FirewatchTower> __Game_Buildings_FirewatchTower_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.DeathcareFacility> __Game_Buildings_DeathcareFacility_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Prison> __Game_Buildings_Prison_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<AdminBuilding> __Game_Buildings_AdminBuilding_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.WelfareOffice> __Game_Buildings_WelfareOffice_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.ResearchFacility> __Game_Buildings_ResearchFacility_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.ParkingFacility> __Game_Buildings_ParkingFacility_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Battery> __Game_Buildings_Battery_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<MailProducer> __Game_Buildings_MailProducer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BuildingCondition> __Game_Buildings_BuildingCondition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ResidentialProperty> __Game_Buildings_ResidentialProperty_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CommercialProperty> __Game_Buildings_CommercialProperty_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<IndustrialProperty> __Game_Buildings_IndustrialProperty_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<OfficeProperty> __Game_Buildings_OfficeProperty_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<StorageProperty> __Game_Buildings_StorageProperty_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ExtractorProperty> __Game_Buildings_ExtractorProperty_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<GarbageProducer> __Game_Buildings_GarbageProducer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Abandoned> __Game_Buildings_Abandoned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.LeisureProvider> __Game_Buildings_LeisureProvider_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterConsumer> __Game_Buildings_WaterConsumer_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Renter> __Game_Buildings_Renter_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.TransportStop> __Game_Routes_TransportStop_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BusStop> __Game_Routes_BusStop_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TrainStop> __Game_Routes_TrainStop_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TaxiStand> __Game_Routes_TaxiStand_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TramStop> __Game_Routes_TramStop_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ShipStop> __Game_Routes_ShipStop_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.MailBox> __Game_Routes_MailBox_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<AirplaneStop> __Game_Routes_AirplaneStop_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SubwayStop> __Game_Routes_SubwayStop_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PassengerTransport> __Game_Vehicles_PassengerTransport_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.CargoTransport> __Game_Vehicles_CargoTransport_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.Taxi> __Game_Vehicles_Taxi_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ParkMaintenanceVehicle> __Game_Vehicles_ParkMaintenanceVehicle_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<RoadMaintenanceVehicle> __Game_Vehicles_RoadMaintenanceVehicle_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.Ambulance> __Game_Vehicles_Ambulance_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EvacuatingTransport> __Game_Vehicles_EvacuatingTransport_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.FireEngine> __Game_Vehicles_FireEngine_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.GarbageTruck> __Game_Vehicles_GarbageTruck_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.Hearse> __Game_Vehicles_Hearse_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.PoliceCar> __Game_Vehicles_PoliceCar_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.PostVan> __Game_Vehicles_PostVan_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrisonerTransport> __Game_Vehicles_PrisonerTransport_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Passenger> __Game_Vehicles_Passenger_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Creatures.Resident> __Game_Creatures_Resident_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Tree> __Game_Objects_Tree_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Plant> __Game_Objects_Plant_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.UtilityObject> __Game_Objects_UtilityObject_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> __Game_Common_Destroyed_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Damaged> __Game_Objects_Damaged_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<UnderConstruction> __Game_Objects_UnderConstruction_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.UniqueObject> __Game_Objects_UniqueObject_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Placeholder> __Game_Objects_Placeholder_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Attached> __Game_Objects_Attached_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<TreeData> __Game_Prefabs_TreeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PollutionData> __Game_Prefabs_PollutionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PollutionModifierData> __Game_Prefabs_PollutionModifierData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceholderBuildingData> __Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SewageOutletData> __Game_Prefabs_SewageOutletData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Creatures.Resident> __Game_Creatures_Resident_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Profitability> __Game_Companies_Profitability_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.LeisureProvider> __Game_Buildings_LeisureProvider_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Employee> __Game_Companies_Employee_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.UtilityObjectData> __Game_Prefabs_UtilityObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> __Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;
      public ComponentTypeHandle<Game.Objects.Color> __Game_Objects_Color_RW_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Controller> __Game_Vehicles_Controller_RO_ComponentTypeHandle;
      public ComponentLookup<Game.Objects.Color> __Game_Objects_Color_RW_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfomodeActive>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewBuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewBuildingStatusData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewBuildingStatusData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewTransportStopData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewTransportStopData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewVehicleData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewObjectStatusData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewObjectStatusData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewNetStatusData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Hospital_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Hospital>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityProducer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Transformer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Transformer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterPumpingStation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.WaterPumpingStation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterTower_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.WaterTower>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_SewageOutlet_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.SewageOutlet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WastewaterTreatmentPlant_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WastewaterTreatmentPlant>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TransportDepot_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.TransportDepot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TransportStation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.TransportStation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageFacility_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.GarbageFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_FireStation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.FireStation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PoliceStation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.PoliceStation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CrimeProducer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CrimeProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_RoadMaintenance_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RoadMaintenance>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ParkMaintenance_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ParkMaintenance>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PostFacility_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.PostFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TelecomFacility_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.TelecomFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_School_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.School>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Park_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Park>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_AttractivenessProvider_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AttractivenessProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_EmergencyShelter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.EmergencyShelter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_DisasterFacility_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.DisasterFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_FirewatchTower_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.FirewatchTower>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_DeathcareFacility_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.DeathcareFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Prison_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Prison>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_AdminBuilding_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AdminBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WelfareOffice_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.WelfareOffice>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ResearchFacility_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.ResearchFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ParkingFacility_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.ParkingFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Battery_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Battery>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_MailProducer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MailProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_BuildingCondition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingCondition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ResidentialProperty_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ResidentialProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CommercialProperty_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CommercialProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_IndustrialProperty_RO_ComponentTypeHandle = state.GetComponentTypeHandle<IndustrialProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_OfficeProperty_RO_ComponentTypeHandle = state.GetComponentTypeHandle<OfficeProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_StorageProperty_RO_ComponentTypeHandle = state.GetComponentTypeHandle<StorageProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ExtractorProperty_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ExtractorProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageProducer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<GarbageProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Abandoned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Abandoned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_LeisureProvider_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.LeisureProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferTypeHandle = state.GetBufferTypeHandle<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TransportStop_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Routes.TransportStop>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_BusStop_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BusStop>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TrainStop_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TrainStop>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TaxiStand_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TaxiStand>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TramStop_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TramStop>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_ShipStop_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ShipStop>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_MailBox_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Routes.MailBox>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_AirplaneStop_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AirplaneStop>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_SubwayStop_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SubwayStop>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PassengerTransport_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PassengerTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CargoTransport_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.CargoTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Taxi_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.Taxi>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkMaintenanceVehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ParkMaintenanceVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_RoadMaintenanceVehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RoadMaintenanceVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Ambulance_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.Ambulance>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_EvacuatingTransport_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EvacuatingTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_FireEngine_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.FireEngine>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_GarbageTruck_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.GarbageTruck>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Hearse_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.Hearse>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PoliceCar_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.PoliceCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PostVan_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.PostVan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PrisonerTransport_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrisonerTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RO_BufferTypeHandle = state.GetBufferTypeHandle<Passenger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Resident_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Creatures.Resident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Plant_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Plant>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UtilityObject_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.UtilityObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Damaged>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UnderConstruction_RO_ComponentTypeHandle = state.GetComponentTypeHandle<UnderConstruction>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UniqueObject_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.UniqueObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Placeholder_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Placeholder>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TreeData_RO_ComponentLookup = state.GetComponentLookup<TreeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PollutionData_RO_ComponentLookup = state.GetComponentLookup<PollutionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PollutionModifierData_RO_ComponentLookup = state.GetComponentLookup<PollutionModifierData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup = state.GetComponentLookup<PlaceholderBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SewageOutletData_RO_ComponentLookup = state.GetComponentLookup<SewageOutletData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Resident_RO_ComponentLookup = state.GetComponentLookup<Game.Creatures.Resident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Profitability_RO_ComponentLookup = state.GetComponentLookup<Profitability>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_LeisureProvider_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.LeisureProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferLookup = state.GetBufferLookup<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentLookup = state.GetComponentLookup<ZoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityObjectData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.UtilityObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup = state.GetComponentLookup<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup = state.GetComponentLookup<ElectricityFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Color_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Color>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Color_RW_ComponentLookup = state.GetComponentLookup<Game.Objects.Color>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentLookup = state.GetComponentLookup<Vehicle>(true);
      }
    }
  }
}
