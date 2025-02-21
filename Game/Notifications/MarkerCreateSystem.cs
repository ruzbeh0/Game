// Decompiled with JetBrains decompiler
// Type: Game.Notifications.MarkerCreateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Routes;
using Game.Serialization;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Notifications
{
  [CompilerGenerated]
  public class MarkerCreateSystem : GameSystemBase, IPostDeserialize
  {
    private ToolSystem m_ToolSystem;
    private IconCommandSystem m_IconCommandSystem;
    private EntityQuery m_EntityQuery;
    private EntityQuery m_UpdatedQuery;
    private EntityQuery m_InfomodeQuery;
    private EntityQuery m_IconQuery;
    private uint m_TransportTypeMask;
    private uint m_BuildingTypeMask;
    private uint m_BuildingStatusTypeMask;
    private uint m_VehicleTypeMask;
    private uint m_MarkerTypeMask;
    private bool m_Loaded;
    private MarkerCreateSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EntityQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Routes.TransportStop>(),
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Vehicle>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>(),
          ComponentType.ReadOnly<CarTrailer>()
        }
      }, new EntityQueryDesc()
      {
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<Marker>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Owner>()
        }
      }, new EntityQueryDesc()
      {
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Objects.OutsideConnection>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Routes.TransportStop>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Building>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Vehicle>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<CarTrailer>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Marker>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Owner>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Objects.OutsideConnection>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_InfomodeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<InfomodeActive>()
        },
        Any = new ComponentType[5]
        {
          ComponentType.ReadOnly<InfoviewTransportStopData>(),
          ComponentType.ReadOnly<InfoviewBuildingData>(),
          ComponentType.ReadOnly<InfoviewBuildingStatusData>(),
          ComponentType.ReadOnly<InfoviewVehicleData>(),
          ComponentType.ReadOnly<InfoviewMarkerData>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_IconQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[5]
        {
          ComponentType.ReadOnly<TransportStopMarkerData>(),
          ComponentType.ReadOnly<BuildingMarkerData>(),
          ComponentType.ReadOnly<VehicleMarkerData>(),
          ComponentType.ReadOnly<MarkerMarkerData>(),
          ComponentType.ReadOnly<PrefabData>()
        }
      });
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TransportTypeMask = uint.MaxValue;
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingTypeMask = uint.MaxValue;
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingStatusTypeMask = uint.MaxValue;
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleTypeMask = uint.MaxValue;
      // ISSUE: reference to a compiler-generated field
      this.m_MarkerTypeMask = uint.MaxValue;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery entityQuery = this.GetLoaded() ? this.m_EntityQuery : this.m_UpdatedQuery;
      TransportType transportType = TransportType.None;
      MarkerType markerType = MarkerType.None;
      uint num1 = 0;
      uint num2 = 0;
      uint num3 = 0;
      uint num4 = 0;
      uint num5 = 0;
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ToolSystem.activeTool.requireStops != TransportType.None)
        {
          // ISSUE: reference to a compiler-generated field
          num1 |= 1U << (int) (this.m_ToolSystem.activeTool.requireStops & (TransportType) 31);
          // ISSUE: reference to a compiler-generated field
          transportType = this.m_ToolSystem.activeTool.requireStops;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ToolSystem.activeTool.requireStopIcons)
          num1 = uint.MaxValue;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.actionMode.IsEditor())
      {
        num5 |= 1U;
        markerType = MarkerType.CreatureSpawner;
      }
      NativeArray<ArchetypeChunk> nativeArray1 = new NativeArray<ArchetypeChunk>();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_InfomodeQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfoviewTransportStopData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<InfoviewTransportStopData> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_InfoviewTransportStopData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfoviewBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<InfoviewBuildingData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_InfoviewBuildingData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfoviewBuildingStatusData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<InfoviewBuildingStatusData> componentTypeHandle3 = this.__TypeHandle.__Game_Prefabs_InfoviewBuildingStatusData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfoviewVehicleData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<InfoviewVehicleData> componentTypeHandle4 = this.__TypeHandle.__Game_Prefabs_InfoviewVehicleData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfoviewMarkerData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<InfoviewMarkerData> componentTypeHandle5 = this.__TypeHandle.__Game_Prefabs_InfoviewMarkerData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        nativeArray1 = this.m_InfomodeQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = nativeArray1[index1];
          NativeArray<InfoviewTransportStopData> nativeArray2 = archetypeChunk.GetNativeArray<InfoviewTransportStopData>(ref componentTypeHandle1);
          NativeArray<InfoviewBuildingData> nativeArray3 = archetypeChunk.GetNativeArray<InfoviewBuildingData>(ref componentTypeHandle2);
          NativeArray<InfoviewBuildingStatusData> nativeArray4 = archetypeChunk.GetNativeArray<InfoviewBuildingStatusData>(ref componentTypeHandle3);
          NativeArray<InfoviewVehicleData> nativeArray5 = archetypeChunk.GetNativeArray<InfoviewVehicleData>(ref componentTypeHandle4);
          NativeArray<InfoviewMarkerData> nativeArray6 = archetypeChunk.GetNativeArray<InfoviewMarkerData>(ref componentTypeHandle5);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            InfoviewTransportStopData transportStopData = nativeArray2[index2];
            if (transportStopData.m_Type != TransportType.None)
              num1 |= 1U << (int) (transportStopData.m_Type & (TransportType) 31);
          }
          for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
          {
            InfoviewBuildingData infoviewBuildingData = nativeArray3[index3];
            num2 |= 1U << (int) (infoviewBuildingData.m_Type & BuildingType.SignatureResidential);
          }
          for (int index4 = 0; index4 < nativeArray4.Length; ++index4)
          {
            InfoviewBuildingStatusData buildingStatusData = nativeArray4[index4];
            num3 |= 1U << (int) (buildingStatusData.m_Type & (BuildingStatusType.NoisePollutionSource | BuildingStatusType.Wellbeing));
          }
          for (int index5 = 0; index5 < nativeArray5.Length; ++index5)
          {
            InfoviewVehicleData infoviewVehicleData = nativeArray5[index5];
            num4 |= 1U << (int) (infoviewVehicleData.m_Type & (VehicleType) 31);
          }
          for (int index6 = 0; index6 < nativeArray6.Length; ++index6)
          {
            InfoviewMarkerData infoviewMarkerData = nativeArray6[index6];
            num5 |= 1U << (int) (infoviewMarkerData.m_Type & (MarkerType) 31);
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((int) num1 == (int) this.m_TransportTypeMask && (int) num2 == (int) this.m_BuildingTypeMask && (int) num3 == (int) this.m_BuildingStatusTypeMask && (int) num4 == (int) this.m_VehicleTypeMask && (int) num5 == (int) this.m_MarkerTypeMask && (num1 == 0U && num2 == 0U && num3 == 0U && num4 == 0U && num5 == 0U || entityQuery.IsEmptyIgnoreFilter))
      {
        if (!nativeArray1.IsCreated)
          return;
        nativeArray1.Dispose();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TransportTypeMask = num1;
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingTypeMask = num2;
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingStatusTypeMask = num3;
        // ISSUE: reference to a compiler-generated field
        this.m_VehicleTypeMask = num4;
        // ISSUE: reference to a compiler-generated field
        this.m_MarkerTypeMask = num5;
        JobHandle outJobHandle;
        // ISSUE: reference to a compiler-generated field
        NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_IconQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_TransportStopData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_WaterPipeOutsideConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_ElectricityOutsideConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Creatures_CreatureSpawner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        this.__TypeHandle.__Game_Buildings_ExtractorProperty_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        this.__TypeHandle.__Game_Buildings_Park_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        this.__TypeHandle.__Game_Buildings_Battery_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_UniqueObject_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Marker_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_MarkerMarkerData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_VehicleMarkerData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingMarkerData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_TransportStopMarkerData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfoviewMarkerData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfoviewVehicleData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfoviewBuildingStatusData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfoviewBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfoviewTransportStopData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        JobHandle jobHandle = new MarkerCreateSystem.MarkerCreateJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_HiddenType = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentTypeHandle,
          m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_InfoviewTransportStopType = this.__TypeHandle.__Game_Prefabs_InfoviewTransportStopData_RO_ComponentTypeHandle,
          m_InfoviewBuildingType = this.__TypeHandle.__Game_Prefabs_InfoviewBuildingData_RO_ComponentTypeHandle,
          m_InfoviewBuildingStatusType = this.__TypeHandle.__Game_Prefabs_InfoviewBuildingStatusData_RO_ComponentTypeHandle,
          m_InfoviewVehicleType = this.__TypeHandle.__Game_Prefabs_InfoviewVehicleData_RO_ComponentTypeHandle,
          m_InfoviewMarkerType = this.__TypeHandle.__Game_Prefabs_InfoviewMarkerData_RO_ComponentTypeHandle,
          m_InfomodeActiveType = this.__TypeHandle.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle,
          m_TransportStopMarkerType = this.__TypeHandle.__Game_Prefabs_TransportStopMarkerData_RO_ComponentTypeHandle,
          m_BuildingMarkerType = this.__TypeHandle.__Game_Prefabs_BuildingMarkerData_RO_ComponentTypeHandle,
          m_VehicleMarkerType = this.__TypeHandle.__Game_Prefabs_VehicleMarkerData_RO_ComponentTypeHandle,
          m_MarkerMarkerType = this.__TypeHandle.__Game_Prefabs_MarkerMarkerData_RO_ComponentTypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_TransportStopType = this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentTypeHandle,
          m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
          m_VehicleType = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentTypeHandle,
          m_MarkerType = this.__TypeHandle.__Game_Objects_Marker_RO_ComponentTypeHandle,
          m_ControllerType = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentTypeHandle,
          m_ParkedCarType = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentTypeHandle,
          m_ParkedTrainType = this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentTypeHandle,
          m_UniqueObjectType = this.__TypeHandle.__Game_Objects_UniqueObject_RO_ComponentTypeHandle,
          m_IconElementType = this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferTypeHandle,
          m_BusStopType = this.__TypeHandle.__Game_Routes_BusStop_RO_ComponentTypeHandle,
          m_TrainStopType = this.__TypeHandle.__Game_Routes_TrainStop_RO_ComponentTypeHandle,
          m_TaxiStandType = this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentTypeHandle,
          m_TramStopType = this.__TypeHandle.__Game_Routes_TramStop_RO_ComponentTypeHandle,
          m_ShipStopType = this.__TypeHandle.__Game_Routes_ShipStop_RO_ComponentTypeHandle,
          m_MailBoxType = this.__TypeHandle.__Game_Routes_MailBox_RO_ComponentTypeHandle,
          m_AirplaneStopType = this.__TypeHandle.__Game_Routes_AirplaneStop_RO_ComponentTypeHandle,
          m_SubwayStopType = this.__TypeHandle.__Game_Routes_SubwayStop_RO_ComponentTypeHandle,
          m_HospitalType = this.__TypeHandle.__Game_Buildings_Hospital_RO_ComponentTypeHandle,
          m_ElectricityProducerType = this.__TypeHandle.__Game_Buildings_ElectricityProducer_RO_ComponentTypeHandle,
          m_TransformerType = this.__TypeHandle.__Game_Buildings_Transformer_RO_ComponentTypeHandle,
          m_BatteryType = this.__TypeHandle.__Game_Buildings_Battery_RO_ComponentTypeHandle,
          m_WaterPumpingStationType = this.__TypeHandle.__Game_Buildings_WaterPumpingStation_RO_ComponentTypeHandle,
          m_WaterTowerType = this.__TypeHandle.__Game_Buildings_WaterTower_RO_ComponentTypeHandle,
          m_SewageOutletType = this.__TypeHandle.__Game_Buildings_SewageOutlet_RO_ComponentTypeHandle,
          m_WastewaterTreatmentPlantType = this.__TypeHandle.__Game_Buildings_WastewaterTreatmentPlant_RO_ComponentTypeHandle,
          m_TransportDepotType = this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentTypeHandle,
          m_TransportStationType = this.__TypeHandle.__Game_Buildings_TransportStation_RO_ComponentTypeHandle,
          m_GarbageFacilityType = this.__TypeHandle.__Game_Buildings_GarbageFacility_RO_ComponentTypeHandle,
          m_FireStationType = this.__TypeHandle.__Game_Buildings_FireStation_RO_ComponentTypeHandle,
          m_PoliceStationType = this.__TypeHandle.__Game_Buildings_PoliceStation_RO_ComponentTypeHandle,
          m_RoadMaintenanceType = this.__TypeHandle.__Game_Buildings_RoadMaintenance_RO_ComponentTypeHandle,
          m_ParkMaintenanceType = this.__TypeHandle.__Game_Buildings_ParkMaintenance_RO_ComponentTypeHandle,
          m_PostFacilityType = this.__TypeHandle.__Game_Buildings_PostFacility_RO_ComponentTypeHandle,
          m_TelecomFacilityType = this.__TypeHandle.__Game_Buildings_TelecomFacility_RO_ComponentTypeHandle,
          m_SchoolType = this.__TypeHandle.__Game_Buildings_School_RO_ComponentTypeHandle,
          m_EmergencyShelterType = this.__TypeHandle.__Game_Buildings_EmergencyShelter_RO_ComponentTypeHandle,
          m_DisasterFacilityType = this.__TypeHandle.__Game_Buildings_DisasterFacility_RO_ComponentTypeHandle,
          m_FirewatchTowerType = this.__TypeHandle.__Game_Buildings_FirewatchTower_RO_ComponentTypeHandle,
          m_ParkType = this.__TypeHandle.__Game_Buildings_Park_RO_ComponentTypeHandle,
          m_DeathcareFacilityType = this.__TypeHandle.__Game_Buildings_DeathcareFacility_RO_ComponentTypeHandle,
          m_PrisonType = this.__TypeHandle.__Game_Buildings_Prison_RO_ComponentTypeHandle,
          m_AdminBuildingType = this.__TypeHandle.__Game_Buildings_AdminBuilding_RO_ComponentTypeHandle,
          m_WelfareOfficeType = this.__TypeHandle.__Game_Buildings_WelfareOffice_RO_ComponentTypeHandle,
          m_ResearchFacilityType = this.__TypeHandle.__Game_Buildings_ResearchFacility_RO_ComponentTypeHandle,
          m_ParkingFacilityType = this.__TypeHandle.__Game_Buildings_ParkingFacility_RO_ComponentTypeHandle,
          m_ResidentialPropertyType = this.__TypeHandle.__Game_Buildings_ResidentialProperty_RO_ComponentTypeHandle,
          m_CommercialPropertyType = this.__TypeHandle.__Game_Buildings_CommercialProperty_RO_ComponentTypeHandle,
          m_IndustrialPropertyType = this.__TypeHandle.__Game_Buildings_IndustrialProperty_RO_ComponentTypeHandle,
          m_OfficePropertyType = this.__TypeHandle.__Game_Buildings_OfficeProperty_RO_ComponentTypeHandle,
          m_ExtractorPropertyType = this.__TypeHandle.__Game_Buildings_ExtractorProperty_RO_ComponentTypeHandle,
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
          m_CreatureSpawnerType = this.__TypeHandle.__Game_Creatures_CreatureSpawner_RO_ComponentTypeHandle,
          m_OutsideConnectionType = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle,
          m_ElectricityOutsideConnectionType = this.__TypeHandle.__Game_Objects_ElectricityOutsideConnection_RO_ComponentTypeHandle,
          m_WaterPipeOutsideConnectionType = this.__TypeHandle.__Game_Objects_WaterPipeOutsideConnection_RO_ComponentTypeHandle,
          m_IconData = this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentLookup,
          m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_TransportStopData = this.__TypeHandle.__Game_Prefabs_TransportStopData_RO_ComponentLookup,
          m_InfomodeChunks = nativeArray1,
          m_IconChunks = archetypeChunkListAsync,
          m_RequiredTransportStopType = transportType,
          m_RequiredMarkerType = markerType,
          m_RequireStandaloneStops = (num1 == uint.MaxValue),
          m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer()
        }.ScheduleParallel<MarkerCreateSystem.MarkerCreateJob>(this.m_EntityQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle));
        if (nativeArray1.IsCreated)
          nativeArray1.Dispose(jobHandle);
        archetypeChunkListAsync.Dispose(jobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_IconCommandSystem.AddCommandBufferWriter(jobHandle);
        this.Dependency = jobHandle;
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
    public MarkerCreateSystem()
    {
    }

    [BurstCompile]
    private struct MarkerCreateJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Hidden> m_HiddenType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewTransportStopData> m_InfoviewTransportStopType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewBuildingData> m_InfoviewBuildingType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewBuildingStatusData> m_InfoviewBuildingStatusType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewVehicleData> m_InfoviewVehicleType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewMarkerData> m_InfoviewMarkerType;
      [ReadOnly]
      public ComponentTypeHandle<InfomodeActive> m_InfomodeActiveType;
      [ReadOnly]
      public ComponentTypeHandle<TransportStopMarkerData> m_TransportStopMarkerType;
      [ReadOnly]
      public ComponentTypeHandle<BuildingMarkerData> m_BuildingMarkerType;
      [ReadOnly]
      public ComponentTypeHandle<VehicleMarkerData> m_VehicleMarkerType;
      [ReadOnly]
      public ComponentTypeHandle<MarkerMarkerData> m_MarkerMarkerType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.TransportStop> m_TransportStopType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<Vehicle> m_VehicleType;
      [ReadOnly]
      public ComponentTypeHandle<Marker> m_MarkerType;
      [ReadOnly]
      public ComponentTypeHandle<Controller> m_ControllerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.UniqueObject> m_UniqueObjectType;
      [ReadOnly]
      public ComponentTypeHandle<ParkedCar> m_ParkedCarType;
      [ReadOnly]
      public ComponentTypeHandle<ParkedTrain> m_ParkedTrainType;
      [ReadOnly]
      public BufferTypeHandle<IconElement> m_IconElementType;
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
      public ComponentTypeHandle<Game.Buildings.Hospital> m_HospitalType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityProducer> m_ElectricityProducerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Transformer> m_TransformerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Battery> m_BatteryType;
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
      public ComponentTypeHandle<Game.Buildings.EmergencyShelter> m_EmergencyShelterType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.DisasterFacility> m_DisasterFacilityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.FirewatchTower> m_FirewatchTowerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Park> m_ParkType;
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
      public ComponentTypeHandle<ResidentialProperty> m_ResidentialPropertyType;
      [ReadOnly]
      public ComponentTypeHandle<CommercialProperty> m_CommercialPropertyType;
      [ReadOnly]
      public ComponentTypeHandle<IndustrialProperty> m_IndustrialPropertyType;
      [ReadOnly]
      public ComponentTypeHandle<OfficeProperty> m_OfficePropertyType;
      [ReadOnly]
      public ComponentTypeHandle<ExtractorProperty> m_ExtractorPropertyType;
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
      public ComponentTypeHandle<Game.Creatures.CreatureSpawner> m_CreatureSpawnerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.ElectricityOutsideConnection> m_ElectricityOutsideConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.WaterPipeOutsideConnection> m_WaterPipeOutsideConnectionType;
      [ReadOnly]
      public ComponentLookup<Icon> m_IconData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<TransportStopData> m_TransportStopData;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_InfomodeChunks;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_IconChunks;
      [ReadOnly]
      public TransportType m_RequiredTransportStopType;
      [ReadOnly]
      public MarkerType m_RequiredMarkerType;
      [ReadOnly]
      public bool m_RequireStandaloneStops;
      public IconCommandBuffer m_IconCommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        Entity prefab1 = Entity.Null;
        Entity entity = Entity.Null;
        bool disallowCluster = false;
        bool flag1 = false;
        bool flag2 = false;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Game.Routes.TransportStop>(ref this.m_TransportStopType))
        {
          TransportType transportType;
          // ISSUE: reference to a compiler-generated method
          if (this.GetTransportStopType(chunk, out transportType))
          {
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType))
            {
              MarkerType markerType;
              Entity markerPrefab;
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              if (this.GetMarkerType(transportType, out markerType) && this.FindMarkerPrefab(markerType, out markerPrefab))
                prefab1 = markerPrefab;
            }
            else
            {
              Entity markerPrefabA;
              Entity markerPrefabB;
              // ISSUE: reference to a compiler-generated method
              if (this.FindMarkerPrefab(transportType, out markerPrefabA, out markerPrefabB))
              {
                prefab1 = markerPrefabA;
                entity = markerPrefabB;
                flag2 = true;
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Building>(ref this.m_BuildingType))
          {
            BuildingType buildingType;
            Entity markerPrefab;
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            if (this.GetBuildingType(chunk, out buildingType) && this.FindMarkerPrefab(buildingType, out markerPrefab))
              prefab1 = markerPrefab;
          }
          else
          {
            VehicleType vehicleType;
            Entity markerPrefab;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            if (chunk.Has<Vehicle>(ref this.m_VehicleType) && this.GetVehicleType(chunk, out vehicleType) && this.FindMarkerPrefab(vehicleType, out markerPrefab))
            {
              prefab1 = markerPrefab;
              disallowCluster = true;
              flag1 = true;
            }
          }
        }
        MarkerType markerType1;
        Entity markerPrefab1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        if (prefab1 == Entity.Null && entity == Entity.Null && chunk.Has<Marker>(ref this.m_MarkerType) && this.GetMarkerType(chunk, out markerType1) && this.FindMarkerPrefab(markerType1, out markerPrefab1))
          prefab1 = markerPrefab1;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<IconElement> bufferAccessor = chunk.GetBufferAccessor<IconElement>(ref this.m_IconElementType);
        if (prefab1 != Entity.Null || flag2 && entity != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
          // ISSUE: reference to a compiler-generated field
          bool isHidden = chunk.Has<Hidden>(ref this.m_HiddenType);
          bool isTemp = nativeArray2.Length != 0;
          if (flag1)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Controller> nativeArray3 = chunk.GetNativeArray<Controller>(ref this.m_ControllerType);
            if (nativeArray3.Length != 0)
            {
              for (int index = 0; index < nativeArray1.Length; ++index)
              {
                Entity owner = nativeArray1[index];
                if (isTemp)
                {
                  Entity original = nativeArray2[index].m_Original;
                  Controller componentData;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ControllerData.TryGetComponent(original, out componentData))
                  {
                    if (componentData.m_Controller != original)
                      continue;
                  }
                  else if (nativeArray3[index].m_Controller != owner)
                    continue;
                }
                else if (nativeArray3[index].m_Controller != owner)
                  continue;
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Add(owner, prefab1, clusterLayer: IconClusterLayer.Marker, flags: IconFlags.Unique, target: Entity.Null, isTemp: isTemp, isHidden: isHidden, disallowCluster: disallowCluster);
              }
            }
            else
              flag1 = false;
          }
          if (flag2)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Entity owner = nativeArray1[index];
              PrefabRef prefabRef = nativeArray4[index];
              // ISSUE: reference to a compiler-generated field
              if (this.m_TransportStopData.HasComponent(prefabRef.m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                TransportStopData transportStopData = this.m_TransportStopData[prefabRef.m_Prefab];
                Entity prefab2;
                if (transportStopData.m_PassengerTransport && prefab1 != Entity.Null)
                  prefab2 = prefab1;
                else if (transportStopData.m_CargoTransport && entity != Entity.Null)
                  prefab2 = entity;
                else
                  continue;
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Add(owner, prefab2, clusterLayer: IconClusterLayer.Marker, flags: IconFlags.Unique, target: Entity.Null, isTemp: isTemp, isHidden: isHidden, disallowCluster: disallowCluster);
              }
            }
          }
          if (!flag1 && !flag2)
          {
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Add(nativeArray1[index], prefab1, clusterLayer: IconClusterLayer.Marker, flags: IconFlags.Unique, target: Entity.Null, isTemp: isTemp, isHidden: isHidden, disallowCluster: disallowCluster);
            }
          }
        }
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          Entity owner = nativeArray1[index1];
          DynamicBuffer<IconElement> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            IconElement iconElement = dynamicBuffer[index2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_IconData[iconElement.m_Icon].m_ClusterLayer == IconClusterLayer.Marker)
            {
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef = this.m_PrefabRefData[iconElement.m_Icon];
              if (prefabRef.m_Prefab != prefab1 && prefabRef.m_Prefab != entity)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Remove(owner, prefabRef.m_Prefab);
              }
            }
          }
        }
      }

      private bool GetTransportStopType(ArchetypeChunk chunk, out TransportType transportType)
      {
        transportType = TransportType.None;
        int num = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        if (this.m_InfomodeChunks.IsCreated)
        {
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
                int priority = nativeArray2[index2].m_Priority;
                if (priority < num)
                {
                  TransportType type = nativeArray1[index2].m_Type;
                  // ISSUE: reference to a compiler-generated method
                  if (this.IsTransportStopType(chunk, type))
                  {
                    transportType = type;
                    num = priority;
                  }
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (transportType == TransportType.None && this.IsTransportStopType(chunk, this.m_RequiredTransportStopType))
        {
          // ISSUE: reference to a compiler-generated field
          transportType = this.m_RequiredTransportStopType;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (transportType == TransportType.None && this.m_RequireStandaloneStops && !chunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType) && !chunk.Has<Owner>(ref this.m_OwnerType))
        {
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<BusStop>(ref this.m_BusStopType))
          {
            transportType = TransportType.Bus;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<TaxiStand>(ref this.m_TaxiStandType))
            {
              transportType = TransportType.Taxi;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (chunk.Has<TramStop>(ref this.m_TramStopType))
              {
                transportType = TransportType.Tram;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (chunk.Has<Game.Routes.MailBox>(ref this.m_MailBoxType))
                  transportType = TransportType.Post;
              }
            }
          }
        }
        return transportType != TransportType.None;
      }

      private bool IsTransportStopType(ArchetypeChunk chunk, TransportType transportType)
      {
        switch (transportType)
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

      private bool FindMarkerPrefab(
        TransportType transportType,
        out Entity markerPrefabA,
        out Entity markerPrefabB)
      {
        bool markerPrefab = false;
        markerPrefabA = Entity.Null;
        markerPrefabB = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_IconChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk iconChunk = this.m_IconChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = iconChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<TransportStopMarkerData> nativeArray2 = iconChunk.GetNativeArray<TransportStopMarkerData>(ref this.m_TransportStopMarkerType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            TransportStopMarkerData transportStopMarkerData = nativeArray2[index2];
            if (transportStopMarkerData.m_TransportType == transportType)
            {
              if (transportStopMarkerData.m_PassengerTransport)
              {
                markerPrefabA = nativeArray1[index2];
                markerPrefab = true;
              }
              else if (transportStopMarkerData.m_CargoTransport)
              {
                markerPrefabB = nativeArray1[index2];
                markerPrefab = true;
              }
            }
          }
        }
        return markerPrefab;
      }

      private bool GetBuildingType(ArchetypeChunk chunk, out BuildingType buildingType)
      {
        buildingType = BuildingType.None;
        int num = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        if (this.m_InfomodeChunks.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
          {
            // ISSUE: reference to a compiler-generated field
            ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index1];
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewBuildingData> nativeArray1 = infomodeChunk.GetNativeArray<InfoviewBuildingData>(ref this.m_InfoviewBuildingType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewBuildingStatusData> nativeArray2 = infomodeChunk.GetNativeArray<InfoviewBuildingStatusData>(ref this.m_InfoviewBuildingStatusType);
            if (nativeArray1.Length != 0 || nativeArray2.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<InfomodeActive> nativeArray3 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
              for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
              {
                int priority = nativeArray3[index2].m_Priority;
                if (priority < num)
                {
                  BuildingType type = nativeArray1[index2].m_Type;
                  // ISSUE: reference to a compiler-generated method
                  if (this.IsBuildingType(chunk, type))
                  {
                    buildingType = type;
                    num = priority;
                  }
                }
              }
              for (int index3 = 0; index3 < nativeArray2.Length; ++index3)
              {
                int priority = nativeArray3[index3].m_Priority;
                if (priority < num)
                {
                  // ISSUE: reference to a compiler-generated method
                  BuildingType buildingType1 = this.GetBuildingType(nativeArray2[index3].m_Type);
                  // ISSUE: reference to a compiler-generated method
                  if (this.IsBuildingType(chunk, buildingType1))
                  {
                    buildingType = buildingType1;
                    num = priority;
                  }
                }
              }
            }
          }
        }
        return buildingType != BuildingType.None;
      }

      private BuildingType GetBuildingType(BuildingStatusType buildingStatusType)
      {
        switch (buildingStatusType)
        {
          case BuildingStatusType.SignatureResidential:
            return BuildingType.SignatureResidential;
          case BuildingStatusType.SignatureCommercial:
            return BuildingType.SignatureCommercial;
          case BuildingStatusType.SignatureIndustrial:
            return BuildingType.SignatureIndustrial;
          case BuildingStatusType.SignatureOffice:
            return BuildingType.SignatureOffice;
          default:
            return BuildingType.None;
        }
      }

      private bool IsBuildingType(ArchetypeChunk chunk, BuildingType buildingType)
      {
        switch (buildingType)
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
            return chunk.Has<Game.Buildings.Park>(ref this.m_ParkType);
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
          default:
            return false;
        }
      }

      private bool FindMarkerPrefab(BuildingType buildingType, out Entity markerPrefab)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_IconChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk iconChunk = this.m_IconChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = iconChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<BuildingMarkerData> nativeArray2 = iconChunk.GetNativeArray<BuildingMarkerData>(ref this.m_BuildingMarkerType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            if (nativeArray2[index2].m_BuildingType == buildingType)
            {
              markerPrefab = nativeArray1[index2];
              return true;
            }
          }
        }
        markerPrefab = Entity.Null;
        return false;
      }

      private bool GetVehicleType(ArchetypeChunk chunk, out VehicleType vehicleType)
      {
        vehicleType = VehicleType.None;
        int num = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_InfomodeChunks.IsCreated && !chunk.Has<ParkedCar>(ref this.m_ParkedCarType) && !chunk.Has<ParkedTrain>(ref this.m_ParkedTrainType))
        {
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
                int priority = nativeArray2[index2].m_Priority;
                if (priority < num)
                {
                  VehicleType type = nativeArray1[index2].m_Type;
                  // ISSUE: reference to a compiler-generated method
                  if (this.IsVehicleType(chunk, type))
                  {
                    vehicleType = type;
                    num = priority;
                  }
                }
              }
            }
          }
        }
        return vehicleType != VehicleType.None;
      }

      private bool IsVehicleType(ArchetypeChunk chunk, VehicleType vehicleType)
      {
        switch (vehicleType)
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

      private bool FindMarkerPrefab(VehicleType vehicleType, out Entity markerPrefab)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_IconChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk iconChunk = this.m_IconChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = iconChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<VehicleMarkerData> nativeArray2 = iconChunk.GetNativeArray<VehicleMarkerData>(ref this.m_VehicleMarkerType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            if (nativeArray2[index2].m_VehicleType == vehicleType)
            {
              markerPrefab = nativeArray1[index2];
              return true;
            }
          }
        }
        markerPrefab = Entity.Null;
        return false;
      }

      private bool GetMarkerType(ArchetypeChunk chunk, out MarkerType markerType)
      {
        markerType = MarkerType.None;
        int num = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        if (this.m_InfomodeChunks.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
          {
            // ISSUE: reference to a compiler-generated field
            ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index1];
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewMarkerData> nativeArray1 = infomodeChunk.GetNativeArray<InfoviewMarkerData>(ref this.m_InfoviewMarkerType);
            if (nativeArray1.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<InfomodeActive> nativeArray2 = infomodeChunk.GetNativeArray<InfomodeActive>(ref this.m_InfomodeActiveType);
              for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
              {
                int priority = nativeArray2[index2].m_Priority;
                if (priority < num)
                {
                  MarkerType type = nativeArray1[index2].m_Type;
                  // ISSUE: reference to a compiler-generated method
                  if (this.IsMarkerType(chunk, type))
                  {
                    markerType = type;
                    num = priority;
                  }
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (markerType == MarkerType.None && this.IsMarkerType(chunk, this.m_RequiredMarkerType))
        {
          // ISSUE: reference to a compiler-generated field
          markerType = this.m_RequiredMarkerType;
        }
        return markerType != MarkerType.None;
      }

      private bool GetMarkerType(TransportType transportType, out MarkerType markerType)
      {
        switch (transportType)
        {
          case TransportType.Bus:
            markerType = MarkerType.RoadOutsideConnection;
            return true;
          case TransportType.Train:
            markerType = MarkerType.TrainOutsideConnection;
            return true;
          case TransportType.Ship:
            markerType = MarkerType.ShipOutsideConnection;
            return true;
          case TransportType.Airplane:
            markerType = MarkerType.AirplaneOutsideConnection;
            return true;
          default:
            markerType = MarkerType.None;
            return false;
        }
      }

      private bool IsMarkerType(ArchetypeChunk chunk, MarkerType markerType)
      {
        switch (markerType)
        {
          case MarkerType.CreatureSpawner:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Creatures.CreatureSpawner>(ref this.m_CreatureSpawnerType);
          case MarkerType.RoadOutsideConnection:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<BusStop>(ref this.m_BusStopType) && chunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType);
          case MarkerType.TrainOutsideConnection:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<TrainStop>(ref this.m_TrainStopType) && chunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType);
          case MarkerType.ShipOutsideConnection:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<ShipStop>(ref this.m_ShipStopType) && chunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType);
          case MarkerType.AirplaneOutsideConnection:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<AirplaneStop>(ref this.m_AirplaneStopType) && chunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType);
          case MarkerType.ElectricityOutsideConnection:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Objects.ElectricityOutsideConnection>(ref this.m_ElectricityOutsideConnectionType);
          case MarkerType.WaterPipeOutsideConnection:
            // ISSUE: reference to a compiler-generated field
            return chunk.Has<Game.Objects.WaterPipeOutsideConnection>(ref this.m_WaterPipeOutsideConnectionType);
          default:
            return false;
        }
      }

      private bool FindMarkerPrefab(MarkerType markerType, out Entity markerPrefab)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_IconChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk iconChunk = this.m_IconChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = iconChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<MarkerMarkerData> nativeArray2 = iconChunk.GetNativeArray<MarkerMarkerData>(ref this.m_MarkerMarkerType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            if (nativeArray2[index2].m_MarkerType == markerType)
            {
              markerPrefab = nativeArray1[index2];
              return true;
            }
          }
        }
        markerPrefab = Entity.Null;
        return false;
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
      public ComponentTypeHandle<InfoviewTransportStopData> __Game_Prefabs_InfoviewTransportStopData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewBuildingData> __Game_Prefabs_InfoviewBuildingData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewBuildingStatusData> __Game_Prefabs_InfoviewBuildingStatusData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewVehicleData> __Game_Prefabs_InfoviewVehicleData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewMarkerData> __Game_Prefabs_InfoviewMarkerData_RO_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Hidden> __Game_Tools_Hidden_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfomodeActive> __Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TransportStopMarkerData> __Game_Prefabs_TransportStopMarkerData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BuildingMarkerData> __Game_Prefabs_BuildingMarkerData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<VehicleMarkerData> __Game_Prefabs_VehicleMarkerData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<MarkerMarkerData> __Game_Prefabs_MarkerMarkerData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.TransportStop> __Game_Routes_TransportStop_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Marker> __Game_Objects_Marker_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Controller> __Game_Vehicles_Controller_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ParkedTrain> __Game_Vehicles_ParkedTrain_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.UniqueObject> __Game_Objects_UniqueObject_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<IconElement> __Game_Notifications_IconElement_RO_BufferTypeHandle;
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
      public ComponentTypeHandle<Game.Buildings.Hospital> __Game_Buildings_Hospital_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityProducer> __Game_Buildings_ElectricityProducer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Transformer> __Game_Buildings_Transformer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Battery> __Game_Buildings_Battery_RO_ComponentTypeHandle;
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
      public ComponentTypeHandle<Game.Buildings.EmergencyShelter> __Game_Buildings_EmergencyShelter_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.DisasterFacility> __Game_Buildings_DisasterFacility_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.FirewatchTower> __Game_Buildings_FirewatchTower_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Park> __Game_Buildings_Park_RO_ComponentTypeHandle;
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
      public ComponentTypeHandle<ResidentialProperty> __Game_Buildings_ResidentialProperty_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CommercialProperty> __Game_Buildings_CommercialProperty_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<IndustrialProperty> __Game_Buildings_IndustrialProperty_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<OfficeProperty> __Game_Buildings_OfficeProperty_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ExtractorProperty> __Game_Buildings_ExtractorProperty_RO_ComponentTypeHandle;
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
      public ComponentTypeHandle<Game.Creatures.CreatureSpawner> __Game_Creatures_CreatureSpawner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.ElectricityOutsideConnection> __Game_Objects_ElectricityOutsideConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.WaterPipeOutsideConnection> __Game_Objects_WaterPipeOutsideConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Icon> __Game_Notifications_Icon_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportStopData> __Game_Prefabs_TransportStopData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewTransportStopData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewTransportStopData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewBuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewBuildingStatusData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewBuildingStatusData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewVehicleData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewMarkerData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewMarkerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfomodeActive>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportStopMarkerData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TransportStopMarkerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingMarkerData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingMarkerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_VehicleMarkerData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<VehicleMarkerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MarkerMarkerData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MarkerMarkerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TransportStop_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Routes.TransportStop>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Marker_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Marker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedTrain_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ParkedTrain>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UniqueObject_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.UniqueObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_IconElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<IconElement>(true);
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
        this.__Game_Buildings_Hospital_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Hospital>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityProducer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Transformer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Transformer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Battery_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Battery>(true);
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
        this.__Game_Buildings_EmergencyShelter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.EmergencyShelter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_DisasterFacility_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.DisasterFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_FirewatchTower_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.FirewatchTower>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Park_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Park>(true);
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
        this.__Game_Buildings_ResidentialProperty_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ResidentialProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CommercialProperty_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CommercialProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_IndustrialProperty_RO_ComponentTypeHandle = state.GetComponentTypeHandle<IndustrialProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_OfficeProperty_RO_ComponentTypeHandle = state.GetComponentTypeHandle<OfficeProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ExtractorProperty_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ExtractorProperty>(true);
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
        this.__Game_Creatures_CreatureSpawner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Creatures.CreatureSpawner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_ElectricityOutsideConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.ElectricityOutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_WaterPipeOutsideConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.WaterPipeOutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Icon_RO_ComponentLookup = state.GetComponentLookup<Icon>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportStopData_RO_ComponentLookup = state.GetComponentLookup<TransportStopData>(true);
      }
    }
  }
}
