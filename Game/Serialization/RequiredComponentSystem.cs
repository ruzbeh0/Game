// Decompiled with JetBrains decompiler
// Type: Game.Serialization.RequiredComponentSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Agents;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Creatures;
using Game.Economy;
using Game.Effects;
using Game.Events;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Policies;
using Game.Prefabs;
using Game.Rendering;
using Game.Routes;
using Game.Simulation;
using Game.Triggers;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class RequiredComponentSystem : GameSystemBase
  {
    private LoadGameSystem m_LoadGameSystem;
    private EntityQuery m_BlockedLaneQuery;
    private EntityQuery m_CarLaneQuery;
    private EntityQuery m_BuildingEfficiencyQuery;
    private EntityQuery m_PolicyQuery;
    private EntityQuery m_CityModifierQuery;
    private EntityQuery m_ServiceDispatchQuery;
    private EntityQuery m_PathInformationQuery;
    private EntityQuery m_NodeGeometryQuery;
    private EntityQuery m_MeshColorQuery;
    private EntityQuery m_MeshBatchQuery;
    private EntityQuery m_RoutePolicyQuery;
    private EntityQuery m_RouteModifierQuery;
    private EntityQuery m_EdgeQuery;
    private EntityQuery m_StorageTaxQuery;
    private EntityQuery m_CityFeeQuery;
    private EntityQuery m_CityFeeQuery2;
    private EntityQuery m_ServiceFeeParameterQuery;
    private EntityQuery m_OutsideGarbageQuery;
    private EntityQuery m_OutsideFireStationQuery;
    private EntityQuery m_OutsidePoliceStationQuery;
    private EntityQuery m_OutsideEfficiencyQuery;
    private EntityQuery m_RouteInfoQuery;
    private EntityQuery m_CompanyProfitabilityQuery;
    private EntityQuery m_StorageQuery;
    private EntityQuery m_RouteBufferIndexQuery;
    private EntityQuery m_CurveElementQuery;
    private EntityQuery m_CitizenPrefabQuery;
    private EntityQuery m_CitizenNameQuery;
    private EntityQuery m_HouseholdNameQuery;
    private EntityQuery m_LabelVertexQuery;
    private EntityQuery m_DistrictNameQuery;
    private EntityQuery m_AnimalNameQuery;
    private EntityQuery m_HouseholdPetQuery;
    private EntityQuery m_RoadNameQuery;
    private EntityQuery m_RouteNumberQuery;
    private EntityQuery m_ChirpRandomLocQuery;
    private EntityQuery m_BlockerQuery;
    private EntityQuery m_CitizenPresenceQuery;
    private EntityQuery m_SubLaneQuery;
    private EntityQuery m_SubObjectQuery;
    private EntityQuery m_NativeQuery;
    private EntityQuery m_GuestVehicleQuery;
    private EntityQuery m_TravelPurposeQuery;
    private EntityQuery m_TreeEffectQuery;
    private EntityQuery m_TakeoffLocationQuery;
    private EntityQuery m_LeisureQuery;
    private EntityQuery m_PlayerMoneyQuery;
    private EntityQuery m_PseudoRandomSeedQuery;
    private EntityQuery m_TransportDepotQuery;
    private EntityQuery m_ServiceUsageQuery;
    private EntityQuery m_OutsideSellerQuery;
    private EntityQuery m_LoadingResourcesQuery;
    private EntityQuery m_CompanyVehicleQuery;
    private EntityQuery m_LaneRestrictionQuery;
    private EntityQuery m_LaneOverlapQuery;
    private EntityQuery m_DispatchedRequestQuery;
    private EntityQuery m_HomelessShelterQuery;
    private EntityQuery m_QueueQuery;
    private EntityQuery m_BoneHistoryQuery;
    private EntityQuery m_UnspawnedQuery;
    private EntityQuery m_ConnectionLaneQuery;
    private EntityQuery m_AreaLaneQuery;
    private EntityQuery m_OfficeQuery;
    private EntityQuery m_VehicleModelQuery;
    private EntityQuery m_PassengerTransportQuery;
    private EntityQuery m_ObjectColorQuery;
    private EntityQuery m_OutsideConnectionQuery;
    private EntityQuery m_NetConditionQuery;
    private EntityQuery m_NetPollutionQuery;
    private EntityQuery m_TrafficSpawnerQuery;
    private EntityQuery m_AreaExpandQuery;
    private EntityQuery m_EmissiveQuery;
    private EntityQuery m_TrainBogieFrameQuery;
    private EntityQuery m_EditorContainerQuery;
    private EntityQuery m_CullingInfoQuery;
    private EntityQuery m_ProcessingTradeCostQuery;
    private EntityQuery m_StorageConditionQuery;
    private EntityQuery m_LaneColorQuery;
    private EntityQuery m_CompanyNotificationQuery;
    private EntityQuery m_PlantQuery;
    private EntityQuery m_CityPopulationQuery;
    private EntityQuery m_CityTourismQuery;
    private EntityQuery m_LaneElevationQuery;
    private EntityQuery m_BuildingNotificationQuery;
    private EntityQuery m_AreaElevationQuery;
    private EntityQuery m_BuildingLotQuery;
    private EntityQuery m_AreaTerrainQuery;
    private EntityQuery m_OwnedVehicleQuery;
    private EntityQuery m_EdgeMappingQuery;
    private EntityQuery m_SubFlowQuery;
    private EntityQuery m_PointOfInterestQuery;
    private EntityQuery m_BuildableAreaQuery;
    private EntityQuery m_SubAreaQuery;
    private EntityQuery m_CrimeVictimQuery;
    private EntityQuery m_ArrivedQuery;
    private EntityQuery m_MailSenderQuery;
    private EntityQuery m_CarKeeperQuery;
    private EntityQuery m_NeedAddHasJobSeekerQuery;
    private EntityQuery m_AgeGroupQuery;
    private EntityQuery m_PrefabRefQuery;
    private EntityQuery m_LabelMaterialQuery;
    private EntityQuery m_ArrowMaterialQuery;
    private EntityQuery m_LockedQuery;
    private EntityQuery m_OutsideUpdateQuery;
    private EntityQuery m_WaitingPassengersQuery;
    private EntityQuery m_ObjectSurfaceQuery;
    private EntityQuery m_WaitingPassengersQuery2;
    private EntityQuery m_PillarQuery;
    private EntityQuery m_LegacyEfficiencyQuery;
    private EntityQuery m_SignatureQuery;
    private EntityQuery m_SubObjectOwnerQuery;
    private EntityQuery m_DangerLevelMissingQuery;
    private EntityQuery m_MeshGroupQuery;
    private EntityQuery m_UpdateFrameQuery;
    private EntityQuery m_FenceQuery;
    private EntityQuery m_NetGeometrySectionQuery;
    private EntityQuery m_NetLaneArchetypeDataQuery;
    private EntityQuery m_PathfindUpdatedQuery;
    private EntityQuery m_RouteColorQuery;
    private EntityQuery m_CitizenQuery;
    private RequiredComponentSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_1938549531_0;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadGameSystem = this.World.GetOrCreateSystemManaged<LoadGameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BlockedLaneQuery = this.GetEntityQuery(ComponentType.ReadOnly<Car>(), ComponentType.Exclude<BlockedLane>());
      // ISSUE: reference to a compiler-generated field
      this.m_CarLaneQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.CarLane>(), ComponentType.Exclude<MasterLane>(), ComponentType.Exclude<LaneFlow>());
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingEfficiencyQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.TransportDepot>(), ComponentType.ReadOnly<Building>(), ComponentType.Exclude<Efficiency>());
      // ISSUE: reference to a compiler-generated field
      this.m_PolicyQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.City.City>(), ComponentType.Exclude<Policy>());
      // ISSUE: reference to a compiler-generated field
      this.m_CityModifierQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.City.City>(), ComponentType.Exclude<CityModifier>());
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceDispatchQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Vehicles.PublicTransport>(),
          ComponentType.ReadOnly<Game.Vehicles.CargoTransport>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<ServiceDispatch>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_PathInformationQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Vehicles.PublicTransport>(),
          ComponentType.ReadOnly<Game.Vehicles.CargoTransport>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<PathInformation>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_NodeGeometryQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.Node>(), ComponentType.ReadOnly<Game.Net.SubLane>(), ComponentType.Exclude<NodeGeometry>());
      // ISSUE: reference to a compiler-generated field
      this.m_MeshColorQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Tree>(),
          ComponentType.ReadOnly<Plant>(),
          ComponentType.ReadOnly<Human>()
        },
        None = new ComponentType[1]
        {
          ComponentType.Exclude<MeshColor>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_MeshBatchQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[6]
        {
          ComponentType.ReadOnly<NodeGeometry>(),
          ComponentType.ReadOnly<EdgeGeometry>(),
          ComponentType.ReadOnly<LaneGeometry>(),
          ComponentType.ReadOnly<ObjectGeometry>(),
          ComponentType.ReadOnly<Game.Objects.Marker>(),
          ComponentType.ReadOnly<Game.Zones.Block>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<MeshBatch>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_RoutePolicyQuery = this.GetEntityQuery(ComponentType.ReadOnly<Route>(), ComponentType.Exclude<Policy>(), ComponentType.Exclude<RouteModifier>());
      // ISSUE: reference to a compiler-generated field
      this.m_RouteModifierQuery = this.GetEntityQuery(ComponentType.ReadOnly<Route>(), ComponentType.Exclude<RouteModifier>());
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.Edge>(), ComponentType.ReadOnly<ConnectedBuilding>(), ComponentType.Exclude<Density>());
      // ISSUE: reference to a compiler-generated field
      this.m_StorageTaxQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Companies.StorageCompany>(), ComponentType.ReadOnly<TaxPayer>());
      // ISSUE: reference to a compiler-generated field
      this.m_CityFeeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.City.City>(), ComponentType.Exclude<ServiceFee>());
      // ISSUE: reference to a compiler-generated field
      this.m_CityFeeQuery2 = this.GetEntityQuery(ComponentType.ReadOnly<Game.City.City>(), ComponentType.ReadWrite<ServiceFee>());
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceFeeParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<ServiceFeeParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideGarbageQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Game.Buildings.GarbageFacility>());
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideFireStationQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Game.Buildings.FireStation>());
      // ISSUE: reference to a compiler-generated field
      this.m_OutsidePoliceStationQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Game.Buildings.PoliceStation>());
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideEfficiencyQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Efficiency>());
      // ISSUE: reference to a compiler-generated field
      this.m_RouteInfoQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Routes.Segment>(), ComponentType.ReadOnly<PathTargets>(), ComponentType.Exclude<RouteInfo>());
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyProfitabilityQuery = this.GetEntityQuery(ComponentType.ReadOnly<CompanyData>(), ComponentType.Exclude<Profitability>(), ComponentType.Exclude<Game.Companies.StorageCompany>());
      // ISSUE: reference to a compiler-generated field
      this.m_StorageQuery = this.GetEntityQuery(ComponentType.ReadOnly<IndustrialProperty>(), ComponentType.Exclude<StorageProperty>());
      // ISSUE: reference to a compiler-generated field
      this.m_RouteBufferIndexQuery = this.GetEntityQuery(ComponentType.ReadOnly<Route>(), ComponentType.Exclude<RouteBufferIndex>());
      // ISSUE: reference to a compiler-generated field
      this.m_CurveElementQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Routes.Segment>(), ComponentType.Exclude<CurveElement>());
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.Exclude<PrefabRef>());
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenNameQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<RandomLocalizationIndex>());
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdNameQuery = this.GetEntityQuery(ComponentType.ReadOnly<Household>(), ComponentType.Exclude<RandomLocalizationIndex>());
      // ISSUE: reference to a compiler-generated field
      this.m_DistrictNameQuery = this.GetEntityQuery(ComponentType.ReadOnly<District>(), ComponentType.ReadOnly<Area>(), ComponentType.Exclude<RandomLocalizationIndex>());
      // ISSUE: reference to a compiler-generated field
      this.m_AnimalNameQuery = this.GetEntityQuery(ComponentType.ReadOnly<Animal>(), ComponentType.Exclude<RandomLocalizationIndex>());
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdPetQuery = this.GetEntityQuery(ComponentType.ReadOnly<HouseholdPet>(), ComponentType.Exclude<RandomLocalizationIndex>());
      // ISSUE: reference to a compiler-generated field
      this.m_RoadNameQuery = this.GetEntityQuery(ComponentType.ReadOnly<Aggregate>(), ComponentType.ReadOnly<LabelMaterial>(), ComponentType.Exclude<RandomLocalizationIndex>());
      // ISSUE: reference to a compiler-generated field
      this.m_LabelVertexQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Areas.LabelExtents>(), ComponentType.Exclude<Game.Areas.LabelVertex>());
      // ISSUE: reference to a compiler-generated field
      this.m_RouteNumberQuery = this.GetEntityQuery(ComponentType.ReadOnly<TransportLine>(), ComponentType.Exclude<RouteNumber>());
      // ISSUE: reference to a compiler-generated field
      this.m_ChirpRandomLocQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<LifePathEntry>(),
          ComponentType.ReadOnly<ChirpEntity>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<RandomLocalizationIndex>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_BlockerQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<HumanCurrentLane>(),
          ComponentType.ReadOnly<AnimalCurrentLane>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Blocker>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenPresenceQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.Exclude<CitizenPresence>());
      // ISSUE: reference to a compiler-generated field
      this.m_SubLaneQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.Exclude<Game.Net.SubLane>());
      // ISSUE: reference to a compiler-generated field
      this.m_SubObjectQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.Exclude<Game.Objects.SubObject>());
      // ISSUE: reference to a compiler-generated field
      this.m_NativeQuery = this.GetEntityQuery(ComponentType.ReadOnly<MapTile>(), ComponentType.Exclude<Native>(), ComponentType.Exclude<Owner>());
      // ISSUE: reference to a compiler-generated field
      this.m_GuestVehicleQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Buildings.PostFacility>(),
          ComponentType.ReadOnly<Game.Buildings.GarbageFacility>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>(),
          ComponentType.ReadOnly<Game.Objects.OutsideConnection>(),
          ComponentType.ReadOnly<GuestVehicle>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_TravelPurposeQuery = this.GetEntityQuery(ComponentType.ReadOnly<TravelPurpose>());
      // ISSUE: reference to a compiler-generated field
      this.m_TreeEffectQuery = this.GetEntityQuery(ComponentType.ReadOnly<Tree>(), ComponentType.Exclude<EnabledEffect>());
      // ISSUE: reference to a compiler-generated field
      this.m_TakeoffLocationQuery = this.GetEntityQuery(ComponentType.ReadOnly<AirplaneStop>(), ComponentType.ReadOnly<Game.Net.SubLane>(), ComponentType.Exclude<Game.Routes.TakeoffLocation>());
      // ISSUE: reference to a compiler-generated field
      this.m_LeisureQuery = this.GetEntityQuery(ComponentType.ReadOnly<CompanyData>(), ComponentType.Exclude<Game.Buildings.LeisureProvider>(), ComponentType.ReadOnly<PrefabRef>());
      // ISSUE: reference to a compiler-generated field
      this.m_PlayerMoneyQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.City.City>(), ComponentType.ReadWrite<Game.Economy.Resources>(), ComponentType.Exclude<PlayerMoney>());
      // ISSUE: reference to a compiler-generated field
      this.m_PseudoRandomSeedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[6]
        {
          ComponentType.ReadOnly<NodeGeometry>(),
          ComponentType.ReadOnly<EdgeGeometry>(),
          ComponentType.ReadOnly<ObjectGeometry>(),
          ComponentType.ReadOnly<AssetStamp>(),
          ComponentType.ReadOnly<Game.Objects.Marker>(),
          ComponentType.ReadOnly<Game.Areas.Lot>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<PseudoRandomSeed>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_TransportDepotQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.OutsideConnection>(), ComponentType.ReadOnly<Game.Buildings.GarbageFacility>(), ComponentType.Exclude<Game.Buildings.TransportDepot>());
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceUsageQuery = this.GetEntityQuery(ComponentType.ReadOnly<CityServiceUpkeep>(), ComponentType.Exclude<ServiceUsage>());
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideSellerQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.OutsideConnection>(), ComponentType.Exclude<ResourceSeller>());
      // ISSUE: reference to a compiler-generated field
      this.m_LoadingResourcesQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Vehicles.CargoTransport>(), ComponentType.Exclude<LoadingResources>());
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyVehicleQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Companies.ProcessingCompany>(), ComponentType.Exclude<OwnedVehicle>());
      // ISSUE: reference to a compiler-generated field
      this.m_LaneRestrictionQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Owner>()
        },
        Any = new ComponentType[7]
        {
          ComponentType.ReadOnly<Game.Net.CarLane>(),
          ComponentType.ReadOnly<Game.Net.ParkingLane>(),
          ComponentType.ReadOnly<Game.Net.PedestrianLane>(),
          ComponentType.ReadOnly<Game.Net.ConnectionLane>(),
          ComponentType.ReadOnly<Game.Routes.TransportStop>(),
          ComponentType.ReadOnly<Game.Routes.TakeoffLocation>(),
          ComponentType.ReadOnly<Game.Objects.SpawnLocation>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_LaneOverlapQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.ParkingLane>(), ComponentType.Exclude<LaneOverlap>());
      // ISSUE: reference to a compiler-generated field
      this.m_DispatchedRequestQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<TransportLine>(),
          ComponentType.ReadOnly<TaxiStand>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<DispatchedRequest>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_HomelessShelterQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Buildings.Park>(),
          ComponentType.ReadOnly<Abandoned>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Renter>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_QueueQuery = this.GetEntityQuery(ComponentType.ReadOnly<Human>(), ComponentType.Exclude<Queue>());
      // ISSUE: reference to a compiler-generated field
      this.m_BoneHistoryQuery = this.GetEntityQuery(ComponentType.ReadOnly<Bone>(), ComponentType.Exclude<BoneHistory>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnspawnedQuery = this.GetEntityQuery(ComponentType.ReadOnly<CurrentVehicle>(), ComponentType.Exclude<Unspawned>());
      // ISSUE: reference to a compiler-generated field
      this.m_ConnectionLaneQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.ConnectionLane>(), ComponentType.ReadOnly<NodeLane>());
      // ISSUE: reference to a compiler-generated field
      this.m_AreaLaneQuery = this.GetEntityQuery(ComponentType.ReadOnly<Area>(), ComponentType.ReadOnly<Game.Net.SubLane>());
      // ISSUE: reference to a compiler-generated field
      this.m_OfficeQuery = this.GetEntityQuery(ComponentType.ReadOnly<IndustrialProperty>(), ComponentType.Exclude<OfficeProperty>());
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleModelQuery = this.GetEntityQuery(ComponentType.ReadOnly<TransportLine>());
      // ISSUE: reference to a compiler-generated field
      this.m_PassengerTransportQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Vehicles.PublicTransport>(), ComponentType.Exclude<PassengerTransport>(), ComponentType.Exclude<EvacuatingTransport>(), ComponentType.Exclude<PrisonerTransport>());
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectColorQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[5]
        {
          ComponentType.ReadOnly<Tree>(),
          ComponentType.ReadOnly<Vehicle>(),
          ComponentType.ReadOnly<Creature>(),
          ComponentType.ReadOnly<Extension>(),
          ComponentType.ReadOnly<Game.Objects.UtilityObject>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Objects.Color>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideConnectionQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Objects.ElectricityOutsideConnection>(),
          ComponentType.ReadOnly<Game.Objects.WaterPipeOutsideConnection>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Objects.OutsideConnection>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_NetConditionQuery = this.GetEntityQuery(ComponentType.ReadOnly<Road>(), ComponentType.Exclude<NetCondition>());
      // ISSUE: reference to a compiler-generated field
      this.m_NetPollutionQuery = this.GetEntityQuery(ComponentType.ReadOnly<Road>(), ComponentType.Exclude<Game.Net.Pollution>());
      // ISSUE: reference to a compiler-generated field
      this.m_TrafficSpawnerQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.OutsideConnection>(), ComponentType.ReadOnly<OwnedVehicle>(), ComponentType.Exclude<Game.Buildings.TrafficSpawner>());
      // ISSUE: reference to a compiler-generated field
      this.m_AreaExpandQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Areas.Surface>(), ComponentType.Exclude<Expand>());
      // ISSUE: reference to a compiler-generated field
      this.m_EmissiveQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<TrafficLight>(),
          ComponentType.ReadOnly<Car>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Emissive>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_TrainBogieFrameQuery = this.GetEntityQuery(ComponentType.ReadOnly<TrainCurrentLane>(), ComponentType.Exclude<TrainBogieFrame>());
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessingTradeCostQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Companies.ProcessingCompany>(), ComponentType.Exclude<TradeCost>());
      // ISSUE: reference to a compiler-generated field
      this.m_EditorContainerQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Net.Node>(),
          ComponentType.ReadOnly<Game.Net.Edge>(),
          ComponentType.ReadOnly<Game.Objects.Object>()
        },
        None = new ComponentType[6]
        {
          ComponentType.ReadOnly<NodeGeometry>(),
          ComponentType.ReadOnly<EdgeGeometry>(),
          ComponentType.ReadOnly<ObjectGeometry>(),
          ComponentType.ReadOnly<AssetStamp>(),
          ComponentType.ReadOnly<Game.Objects.Marker>(),
          ComponentType.ReadOnly<Game.Tools.EditorContainer>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_CullingInfoQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[8]
        {
          ComponentType.ReadOnly<NodeGeometry>(),
          ComponentType.ReadOnly<EdgeGeometry>(),
          ComponentType.ReadOnly<LaneGeometry>(),
          ComponentType.ReadOnly<ObjectGeometry>(),
          ComponentType.ReadOnly<Game.Objects.Marker>(),
          ComponentType.ReadOnly<AssetStamp>(),
          ComponentType.ReadOnly<Game.Zones.Block>(),
          ComponentType.ReadOnly<Game.Tools.EditorContainer>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<CullingInfo>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_StorageConditionQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Companies.StorageCompany>(), ComponentType.ReadOnly<PropertyRenter>());
      // ISSUE: reference to a compiler-generated field
      this.m_LaneColorQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Net.TrackLane>(),
          ComponentType.ReadOnly<Game.Net.UtilityLane>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<LaneColor>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyNotificationQuery = this.GetEntityQuery(ComponentType.ReadOnly<CompanyData>(), ComponentType.Exclude<CompanyNotifications>());
      // ISSUE: reference to a compiler-generated field
      this.m_PlantQuery = this.GetEntityQuery(ComponentType.ReadOnly<Tree>(), ComponentType.Exclude<Plant>());
      // ISSUE: reference to a compiler-generated field
      this.m_CityPopulationQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.City.City>(), ComponentType.Exclude<Population>());
      // ISSUE: reference to a compiler-generated field
      this.m_CityTourismQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.City.City>(), ComponentType.Exclude<Tourism>());
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingNotificationQuery = this.GetEntityQuery(ComponentType.ReadOnly<ResidentialProperty>(), ComponentType.Exclude<BuildingNotifications>());
      // ISSUE: reference to a compiler-generated field
      this.m_LaneElevationQuery = this.GetEntityQuery(ComponentType.ReadOnly<Lane>(), ComponentType.ReadOnly<Owner>(), ComponentType.Exclude<EdgeLane>(), ComponentType.Exclude<AreaLane>(), ComponentType.Exclude<Game.Net.ConnectionLane>(), ComponentType.Exclude<Game.Net.Elevation>());
      // ISSUE: reference to a compiler-generated field
      this.m_AreaElevationQuery = this.GetEntityQuery(ComponentType.ReadOnly<Area>(), ComponentType.ReadOnly<Owner>());
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingLotQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.Exclude<Game.Buildings.Lot>());
      // ISSUE: reference to a compiler-generated field
      this.m_AreaTerrainQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Areas.Lot>(), ComponentType.ReadOnly<Storage>(), ComponentType.Exclude<Game.Areas.Terrain>());
      // ISSUE: reference to a compiler-generated field
      this.m_OwnedVehicleQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Areas.Lot>(), ComponentType.ReadOnly<Storage>(), ComponentType.Exclude<OwnedVehicle>());
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeMappingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.UtilityLane>(), ComponentType.Exclude<EdgeMapping>());
      // ISSUE: reference to a compiler-generated field
      this.m_SubFlowQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.UtilityLane>(), ComponentType.Exclude<SubFlow>());
      // ISSUE: reference to a compiler-generated field
      this.m_PointOfInterestQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Vehicles.PoliceCar>(),
          ComponentType.ReadOnly<RenewableElectricityProduction>(),
          ComponentType.ReadOnly<Game.Buildings.ExtractorFacility>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<PointOfInterest>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_BuildableAreaQuery = this.GetEntityQuery(ComponentType.ReadOnly<MapFeatureElement>(), ComponentType.Exclude<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_SubAreaQuery = this.GetEntityQuery(ComponentType.ReadOnly<Extractor>(), ComponentType.Exclude<Game.Areas.SubArea>());
      // ISSUE: reference to a compiler-generated field
      this.m_CrimeVictimQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.Exclude<CrimeVictim>());
      // ISSUE: reference to a compiler-generated field
      this.m_ArrivedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.Exclude<Arrived>());
      // ISSUE: reference to a compiler-generated field
      this.m_MailSenderQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.Exclude<MailSender>());
      // ISSUE: reference to a compiler-generated field
      this.m_CarKeeperQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.Exclude<CarKeeper>());
      // ISSUE: reference to a compiler-generated field
      this.m_NeedAddHasJobSeekerQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.Exclude<HasJobSeeker>());
      // ISSUE: reference to a compiler-generated field
      this.m_AgeGroupQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadWrite<Citizen>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Citizens.Child>(),
          ComponentType.ReadOnly<Teen>(),
          ComponentType.ReadOnly<Adult>(),
          ComponentType.ReadOnly<Elderly>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabRefQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabRef>());
      // ISSUE: reference to a compiler-generated field
      this.m_LabelMaterialQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.LabelExtents>(), ComponentType.Exclude<LabelMaterial>());
      // ISSUE: reference to a compiler-generated field
      this.m_ArrowMaterialQuery = this.GetEntityQuery(ComponentType.ReadOnly<ArrowPosition>(), ComponentType.Exclude<ArrowMaterial>());
      // ISSUE: reference to a compiler-generated field
      this.m_LockedQuery = this.GetEntityQuery(ComponentType.ReadOnly<UnlockRequirement>(), ComponentType.Exclude<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideUpdateQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.OutsideConnection>());
      // ISSUE: reference to a compiler-generated field
      this.m_WaitingPassengersQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<AccessLane>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<TaxiStand>(),
          ComponentType.ReadOnly<Waypoint>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<WaitingPassengers>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_WaitingPassengersQuery2 = this.GetEntityQuery(ComponentType.ReadOnly<WaitingPassengers>(), ComponentType.Exclude<TaxiStand>(), ComponentType.Exclude<Waypoint>());
      // ISSUE: reference to a compiler-generated field
      this.m_PillarQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<ObjectGeometry>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Objects.UtilityObject>(),
          ComponentType.ReadOnly<Game.Objects.NetObject>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Pillar>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_LegacyEfficiencyQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.BuildingEfficiency>());
      // ISSUE: reference to a compiler-generated field
      this.m_SignatureQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<Game.Objects.UniqueObject>(), ComponentType.ReadOnly<Renter>(), ComponentType.Exclude<Game.Buildings.Park>(), ComponentType.Exclude<Game.Buildings.Signature>());
      // ISSUE: reference to a compiler-generated field
      this.m_SubObjectOwnerQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Objects.Object>(),
          ComponentType.ReadOnly<Owner>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Creatures.CreatureSpawner>(),
          ComponentType.ReadOnly<Game.Tools.EditorContainer>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Vehicle>(),
          ComponentType.ReadOnly<Creature>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_DangerLevelMissingQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Events.Event>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Events.WeatherPhenomenon>(),
          ComponentType.ReadOnly<WaterLevelChange>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Events.DangerLevel>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_MeshGroupQuery = this.GetEntityQuery(ComponentType.ReadOnly<Human>(), ComponentType.ReadOnly<MeshBatch>(), ComponentType.Exclude<MeshGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSurfaceQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<ObjectGeometry>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Creature>(),
          ComponentType.ReadOnly<Vehicle>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Objects.Surface>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<ObjectGeometry>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Objects.Surface>(),
          ComponentType.ReadOnly<Owner>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Road>(),
          ComponentType.ReadOnly<Game.Net.Node>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Objects.Surface>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateFrameQuery = this.GetEntityQuery(ComponentType.ReadOnly<Plant>(), ComponentType.Exclude<UpdateFrame>());
      // ISSUE: reference to a compiler-generated field
      this.m_FenceQuery = this.GetEntityQuery(ComponentType.ReadOnly<LaneGeometry>(), ComponentType.ReadOnly<Game.Net.UtilityLane>(), ComponentType.Exclude<PseudoRandomSeed>(), ComponentType.Exclude<MeshColor>(), ComponentType.Exclude<UpdateFrame>());
      // ISSUE: reference to a compiler-generated field
      this.m_NetGeometrySectionQuery = this.GetEntityQuery(ComponentType.ReadOnly<NetGeometryData>(), ComponentType.Exclude<NetGeometrySection>());
      // ISSUE: reference to a compiler-generated field
      this.m_NetLaneArchetypeDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<NetLaneData>(), ComponentType.Exclude<NetLaneArchetypeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindUpdatedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Owner>()
        },
        Any = new ComponentType[8]
        {
          ComponentType.ReadOnly<Game.Net.CarLane>(),
          ComponentType.ReadOnly<Game.Net.PedestrianLane>(),
          ComponentType.ReadOnly<Game.Net.TrackLane>(),
          ComponentType.ReadOnly<Game.Net.ParkingLane>(),
          ComponentType.ReadOnly<Game.Net.ConnectionLane>(),
          ComponentType.ReadOnly<Game.Routes.TransportStop>(),
          ComponentType.ReadOnly<Game.Routes.TakeoffLocation>(),
          ComponentType.ReadOnly<Game.Objects.SpawnLocation>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Routes.MailBox>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_RouteColorQuery = this.GetEntityQuery(ComponentType.ReadOnly<CurrentRoute>(), ComponentType.Exclude<Game.Routes.Color>());
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>());
    }

    [Preserve]
    protected override void OnUpdate()
    {
      EntityManager entityManager;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_BlockedLaneQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<BlockedLane>(this.m_BlockedLaneQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CarLaneQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<LaneFlow>(this.m_CarLaneQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_BuildingEfficiencyQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Efficiency>(this.m_BuildingEfficiencyQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PolicyQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Policy>(this.m_PolicyQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CityModifierQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<CityModifier>(this.m_CityModifierQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ServiceDispatchQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<ServiceDispatch>(this.m_ServiceDispatchQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PathInformationQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<PathInformation>(this.m_PathInformationQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_NodeGeometryQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<NodeGeometry>(this.m_NodeGeometryQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_MeshBatchQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<MeshBatch>(this.m_MeshBatchQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RoutePolicyQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Policy>(this.m_RoutePolicyQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RouteModifierQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<RouteModifier>(this.m_RouteModifierQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_EdgeQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Density>(this.m_EdgeQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_StorageTaxQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.RemoveComponent<TaxPayer>(this.m_StorageTaxQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CityFeeQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        ServiceFeeParameterData singleton = this.m_ServiceFeeParameterQuery.GetSingleton<ServiceFeeParameterData>();
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_CityFeeQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          DynamicBuffer<ServiceFee> dynamicBuffer = entityManager.AddBuffer<ServiceFee>(entityArray[index]);
          foreach (ServiceFee defaultFee in singleton.GetDefaultFees())
            dynamicBuffer.Add(defaultFee);
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CityFeeQuery2.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        Entity singletonEntity = this.m_CityFeeQuery2.GetSingletonEntity();
        entityManager = this.EntityManager;
        DynamicBuffer<ServiceFee> buffer = entityManager.GetBuffer<ServiceFee>(singletonEntity);
        bool flag = false;
        for (int index = 0; index < buffer.Length; ++index)
        {
          if (buffer[index].m_Resource == PlayerResource.Water)
            flag = true;
        }
        if (!flag)
        {
          ServiceFee elem = new ServiceFee()
          {
            m_Resource = PlayerResource.Water
          };
          elem.m_Fee = elem.GetDefaultFee(elem.m_Resource);
          buffer.Add(elem);
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_OutsideGarbageQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_OutsideGarbageQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          Entity prefab = entityManager.GetComponentData<PrefabRef>(entityArray[index]).m_Prefab;
          entityManager = this.EntityManager;
          if (entityManager.HasComponent<GarbageFacilityData>(prefab))
          {
            entityManager = this.EntityManager;
            GarbageFacilityData componentData = entityManager.GetComponentData<GarbageFacilityData>(prefab);
            DynamicBuffer<Game.Economy.Resources> buffer;
            if (this.EntityManager.TryGetBuffer<Game.Economy.Resources>(entityArray[index], false, out buffer))
              EconomyUtils.SetResources(Resource.Garbage, buffer, componentData.m_GarbageCapacity / 2);
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<ServiceDispatch>(entityArray[index]))
            {
              entityManager = this.EntityManager;
              entityManager.AddBuffer<ServiceDispatch>(entityArray[index]);
            }
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<OwnedVehicle>(entityArray[index]))
            {
              entityManager = this.EntityManager;
              entityManager.AddBuffer<OwnedVehicle>(entityArray[index]);
            }
          }
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_OutsideFireStationQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_OutsideFireStationQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          Entity prefab = entityManager.GetComponentData<PrefabRef>(entityArray[index]).m_Prefab;
          entityManager = this.EntityManager;
          if (entityManager.HasComponent<FireStationData>(prefab))
          {
            entityManager = this.EntityManager;
            entityManager.GetComponentData<FireStationData>(prefab);
            entityManager = this.EntityManager;
            entityManager.AddComponentData<Game.Buildings.FireStation>(entityArray[index], new Game.Buildings.FireStation());
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<ServiceDispatch>(entityArray[index]))
            {
              entityManager = this.EntityManager;
              entityManager.AddBuffer<ServiceDispatch>(entityArray[index]);
            }
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<OwnedVehicle>(entityArray[index]))
            {
              entityManager = this.EntityManager;
              entityManager.AddBuffer<OwnedVehicle>(entityArray[index]);
            }
          }
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_OutsidePoliceStationQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_OutsidePoliceStationQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          Entity prefab = entityManager.GetComponentData<PrefabRef>(entityArray[index]).m_Prefab;
          entityManager = this.EntityManager;
          if (entityManager.HasComponent<PoliceStationData>(prefab))
          {
            entityManager = this.EntityManager;
            PoliceStationData componentData = entityManager.GetComponentData<PoliceStationData>(prefab);
            entityManager = this.EntityManager;
            entityManager.AddComponentData<Game.Buildings.PoliceStation>(entityArray[index], new Game.Buildings.PoliceStation()
            {
              m_PurposeMask = componentData.m_PurposeMask
            });
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<ServiceDispatch>(entityArray[index]))
            {
              entityManager = this.EntityManager;
              entityManager.AddBuffer<ServiceDispatch>(entityArray[index]);
            }
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<OwnedVehicle>(entityArray[index]))
            {
              entityManager = this.EntityManager;
              entityManager.AddBuffer<OwnedVehicle>(entityArray[index]);
            }
          }
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_OutsideEfficiencyQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Efficiency>(this.m_OutsideEfficiencyQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RouteInfoQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<RouteInfo>(this.m_RouteInfoQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CompanyProfitabilityQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_CompanyProfitabilityQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.AddComponentData<Profitability>(entityArray[index], new Profitability()
          {
            m_Profitability = (byte) 127
          });
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_StorageQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_StorageQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          PrefabRef component1;
          BuildingPropertyData component2;
          if (this.EntityManager.TryGetComponent<PrefabRef>(entityArray[index], out component1) && this.EntityManager.TryGetComponent<BuildingPropertyData>(component1.m_Prefab, out component2) && component2.m_AllowedStored != Resource.NoResource)
          {
            entityManager = this.EntityManager;
            entityManager.AddComponent<StorageProperty>(entityArray[index]);
          }
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RouteBufferIndexQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<RouteBufferIndex>(this.m_RouteBufferIndexQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CurveElementQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<CurveElement>(this.m_CurveElementQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CitizenPrefabQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<PrefabRef>(this.m_CitizenPrefabQuery);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_CitizenPrefabQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.AddBuffer<RandomLocalizationIndex>(entityArray[index]).Add(RandomLocalizationIndex.kNone);
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CitizenNameQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_CitizenNameQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.AddBuffer<RandomLocalizationIndex>(entityArray[index]).Add(RandomLocalizationIndex.kNone);
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_HouseholdNameQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_HouseholdNameQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.AddBuffer<RandomLocalizationIndex>(entityArray[index]).Add(RandomLocalizationIndex.kNone);
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_DistrictNameQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_DistrictNameQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.AddBuffer<RandomLocalizationIndex>(entityArray[index]).Add(RandomLocalizationIndex.kNone);
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_AnimalNameQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_AnimalNameQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.AddBuffer<RandomLocalizationIndex>(entityArray[index]).Add(RandomLocalizationIndex.kNone);
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_HouseholdPetQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_HouseholdPetQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.AddBuffer<RandomLocalizationIndex>(entityArray[index]).Add(RandomLocalizationIndex.kNone);
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RoadNameQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_RoadNameQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.AddBuffer<RandomLocalizationIndex>(entityArray[index]).Add(RandomLocalizationIndex.kNone);
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ChirpRandomLocQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_ChirpRandomLocQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.AddBuffer<RandomLocalizationIndex>(entityArray[index]).Add(RandomLocalizationIndex.kNone);
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_LabelVertexQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Game.Areas.LabelVertex>(this.m_LabelVertexQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RouteNumberQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<RouteNumber>(this.m_RouteNumberQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_BlockerQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Blocker>(this.m_BlockerQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CitizenPresenceQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_CitizenPresenceQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.AddComponentData<CitizenPresence>(entityArray[index], new CitizenPresence()
          {
            m_Presence = (byte) 128
          });
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_SubLaneQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Game.Net.SubLane>(this.m_SubLaneQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_SubObjectQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Game.Objects.SubObject>(this.m_SubObjectQuery);
      }
      Colossal.Serialization.Entities.Context context = this.World.GetOrCreateSystemManaged<LoadGameSystem>().context;
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.netUpkeepCost && !this.m_NativeQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Native>(this.m_NativeQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_GuestVehicleQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<GuestVehicle>(this.m_GuestVehicleQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_TravelPurposeQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_TravelPurposeQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          TravelPurpose componentData = entityManager.GetComponentData<TravelPurpose>(entityArray[index]);
          if (componentData.m_Purpose == Game.Citizens.Purpose.GoingToWork || componentData.m_Purpose == Game.Citizens.Purpose.Working)
          {
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<Worker>(entityArray[index]))
            {
              entityManager = this.EntityManager;
              entityManager.RemoveComponent<TravelPurpose>(entityArray[index]);
              continue;
            }
          }
          if (componentData.m_Purpose == Game.Citizens.Purpose.GoingToSchool || componentData.m_Purpose == Game.Citizens.Purpose.Studying)
          {
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<Game.Citizens.Student>(entityArray[index]))
            {
              entityManager = this.EntityManager;
              entityManager.RemoveComponent<TravelPurpose>(entityArray[index]);
            }
          }
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_TreeEffectQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<EnabledEffect>(this.m_TreeEffectQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_TakeoffLocationQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Game.Routes.TakeoffLocation>(this.m_TakeoffLocationQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_LeisureQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_LeisureQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> componentDataArray = this.m_LeisureQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < componentDataArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          if (entityManager.HasComponent<LeisureProviderData>(componentDataArray[index].m_Prefab))
          {
            entityManager = this.EntityManager;
            entityManager.AddComponent<Game.Buildings.LeisureProvider>(entityArray[index]);
          }
        }
        componentDataArray.Dispose();
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_TransportDepotQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Game.Buildings.TransportDepot>(this.m_TransportDepotQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ServiceUsageQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_ServiceUsageQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.AddComponentData<ServiceUsage>(entityArray[index], new ServiceUsage()
          {
            m_Usage = 1f
          });
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_OutsideSellerQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<ResourceSeller>(this.m_OutsideSellerQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_LoadingResourcesQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<LoadingResources>(this.m_LoadingResourcesQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CompanyVehicleQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<OwnedVehicle>(this.m_CompanyVehicleQuery);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoadGameSystem.context.version < Game.Version.pathfindAccessRestriction && !this.m_LaneRestrictionQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<PathfindUpdated>(this.m_LaneRestrictionQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_LaneOverlapQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<LaneOverlap>(this.m_LaneOverlapQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_DispatchedRequestQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<DispatchedRequest>(this.m_DispatchedRequestQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_HomelessShelterQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_HomelessShelterQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.AddBuffer<Renter>(entityArray[index]);
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_QueueQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Queue>(this.m_QueueQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_BoneHistoryQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<BoneHistory>(this.m_BoneHistoryQuery);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoadGameSystem.context.version < Game.Version.currentVehicleRefactoring && !this.m_UnspawnedQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Unspawned>(this.m_UnspawnedQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoadGameSystem.context.version < Game.Version.areaLaneComponent)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ConnectionLaneQuery.IsEmptyIgnoreFilter)
        {
          entityManager = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager.RemoveComponent<NodeLane>(this.m_ConnectionLaneQuery);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_AreaLaneQuery.IsEmptyIgnoreFilter)
        {
          entityManager = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager.AddComponent<Updated>(this.m_AreaLaneQuery);
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoadGameSystem.context.version < Game.Version.officePropertyComponent && !this.m_OfficeQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_OfficeQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          PrefabRef component3;
          BuildingPropertyData component4;
          if (this.EntityManager.TryGetComponent<PrefabRef>(entityArray[index], out component3) && this.EntityManager.TryGetComponent<BuildingPropertyData>(component3.m_Prefab, out component4) && EconomyUtils.IsOfficeResource(component4.m_AllowedManufactured))
          {
            entityManager = this.EntityManager;
            entityManager.AddComponent<OfficeProperty>(entityArray[index]);
          }
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PassengerTransportQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<PassengerTransport>(this.m_PassengerTransportQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ObjectColorQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Game.Objects.Color>(this.m_ObjectColorQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_OutsideConnectionQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Game.Objects.OutsideConnection>(this.m_OutsideConnectionQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_NetConditionQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<NetCondition>(this.m_NetConditionQuery);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoadGameSystem.context.version < Game.Version.netPollutionAccumulation && !this.m_NetPollutionQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Game.Net.Pollution>(this.m_NetPollutionQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_TrafficSpawnerQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Game.Buildings.TrafficSpawner>(this.m_TrafficSpawnerQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_AreaExpandQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Expand>(this.m_AreaExpandQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_EmissiveQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<LightState>(this.m_EmissiveQuery);
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Emissive>(this.m_EmissiveQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_TrainBogieFrameQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<TrainBogieFrame>(this.m_TrainBogieFrameQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ProcessingTradeCostQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<TradeCost>(this.m_ProcessingTradeCostQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.editorContainerFix && !this.m_EditorContainerQuery.IsEmptyIgnoreFilter)
      {
        if (context.purpose == Colossal.Serialization.Entities.Purpose.LoadMap)
        {
          entityManager = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager.AddComponent<CullingInfo>(this.m_EditorContainerQuery);
          entityManager = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager.AddComponent<Game.Tools.EditorContainer>(this.m_EditorContainerQuery);
        }
        else
        {
          entityManager = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager.DestroyEntity(this.m_EditorContainerQuery);
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CullingInfoQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<CullingInfo>(this.m_CullingInfoQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_StorageConditionQuery.IsEmptyIgnoreFilter && context.version < Game.Version.storageConditionReset)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_StorageConditionQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          PropertyRenter component5;
          BuildingCondition component6;
          DynamicBuffer<Game.Economy.Resources> buffer;
          if (this.EntityManager.TryGetComponent<PropertyRenter>(entityArray[index], out component5) && this.EntityManager.TryGetComponent<BuildingCondition>(component5.m_Property, out component6) && this.EntityManager.TryGetBuffer<Game.Economy.Resources>(entityArray[index], false, out buffer))
          {
            component6.m_Condition = 0;
            entityManager = this.EntityManager;
            entityManager.SetComponentData<BuildingCondition>(component5.m_Property, component6);
            EconomyUtils.SetResources(Resource.Money, buffer, 0);
          }
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_LaneColorQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<LaneColor>(this.m_LaneColorQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CompanyNotificationQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<CompanyNotifications>(this.m_CompanyNotificationQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PlantQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Plant>(this.m_PlantQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CityPopulationQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Population>(this.m_CityPopulationQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CityTourismQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Tourism>(this.m_CityTourismQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_BuildingNotificationQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<BuildingNotifications>(this.m_BuildingNotificationQuery);
      }
      if (context.version < Game.Version.laneElevation)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_LaneElevationQuery.IsEmptyIgnoreFilter)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> entityArray = this.m_LaneElevationQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
          for (int index = 0; index < entityArray.Length; ++index)
          {
            Entity entity = entityArray[index];
            entityManager = this.EntityManager;
            Game.Objects.Transform component;
            if (this.EntityManager.TryGetComponent<Game.Objects.Transform>(entityManager.GetComponentData<Owner>(entity).m_Owner, out component))
            {
              entityManager = this.EntityManager;
              Curve componentData1 = entityManager.GetComponentData<Curve>(entity);
              Game.Net.Elevation componentData2;
              componentData2.m_Elevation.x = componentData1.m_Bezier.a.y - component.m_Position.y;
              componentData2.m_Elevation.y = componentData1.m_Bezier.d.y - component.m_Position.y;
              entityManager = this.EntityManager;
              entityManager.RemoveComponent<NodeLane>(entity);
              bool2 bool2 = math.abs(componentData2.m_Elevation) >= 0.1f;
              if (math.any(bool2))
              {
                componentData2.m_Elevation = math.select((float2) float.MinValue, componentData2.m_Elevation, bool2);
                entityManager = this.EntityManager;
                entityManager.AddComponentData<Game.Net.Elevation>(entity, componentData2);
              }
            }
          }
          entityArray.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_AreaElevationQuery.IsEmptyIgnoreFilter)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> entityArray = this.m_AreaElevationQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
          for (int index1 = 0; index1 < entityArray.Length; ++index1)
          {
            Entity entity = entityArray[index1];
            entityManager = this.EntityManager;
            DynamicBuffer<Game.Areas.Node> buffer = entityManager.GetBuffer<Game.Areas.Node>(entity);
            entityManager = this.EntityManager;
            Owner component7;
            Game.Objects.Transform component8;
            if (entityManager.HasComponent<Game.Areas.Space>(entity) && this.EntityManager.TryGetComponent<Owner>(entity, out component7) && this.EntityManager.TryGetComponent<Game.Objects.Transform>(component7.m_Owner, out component8))
            {
              for (int index2 = 0; index2 < buffer.Length; ++index2)
              {
                ref Game.Areas.Node local = ref buffer.ElementAt(index2);
                local.m_Elevation = local.m_Position.y - component8.m_Position.y;
                local.m_Elevation = math.select(float.MinValue, local.m_Elevation, (double) math.abs(local.m_Elevation) >= 0.10000000149011612);
              }
            }
            else
            {
              for (int index3 = 0; index3 < buffer.Length; ++index3)
                buffer.ElementAt(index3).m_Elevation = float.MinValue;
            }
          }
          entityArray.Dispose();
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_BuildingLotQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Game.Buildings.Lot>(this.m_BuildingLotQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_AreaTerrainQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Game.Areas.Terrain>(this.m_AreaTerrainQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_OwnedVehicleQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<OwnedVehicle>(this.m_OwnedVehicleQuery);
      }
      if (context.version < Game.Version.laneSubFlow)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EdgeMappingQuery.IsEmptyIgnoreFilter)
        {
          entityManager = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager.AddComponent<EdgeMapping>(this.m_EdgeMappingQuery);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubFlowQuery.IsEmptyIgnoreFilter)
        {
          entityManager = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager.AddComponent<SubFlow>(this.m_SubFlowQuery);
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PointOfInterestQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<PointOfInterest>(this.m_PointOfInterestQuery);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoadGameSystem.context.version < Game.Version.buildableArea && !this.m_BuildableAreaQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Updated>(this.m_BuildableAreaQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.extractorSubAreas && !this.m_SubAreaQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Game.Areas.SubArea>(this.m_SubAreaQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.enableableCrimeVictim && !this.m_CrimeVictimQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_CrimeVictimQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<CrimeVictim>(this.m_CrimeVictimQuery);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.SetComponentEnabled<CrimeVictim>(entityArray[index], false);
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.enableableCrimeVictim && !this.m_ArrivedQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_ArrivedQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Arrived>(this.m_ArrivedQuery);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.SetComponentEnabled<Arrived>(entityArray[index], false);
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.enableableCrimeVictim && !this.m_MailSenderQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_MailSenderQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<MailSender>(this.m_MailSenderQuery);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.SetComponentEnabled<MailSender>(entityArray[index], false);
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.enableableCrimeVictim && !this.m_CarKeeperQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_CarKeeperQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<CarKeeper>(this.m_CarKeeperQuery);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.SetComponentEnabled<CarKeeper>(entityArray[index], false);
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.findJobOptimize && !this.m_NeedAddHasJobSeekerQuery.IsEmpty)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_NeedAddHasJobSeekerQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<HasJobSeeker>(this.m_NeedAddHasJobSeekerQuery);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.SetComponentEnabled<HasJobSeeker>(entityArray[index], false);
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_AgeGroupQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_AgeGroupQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          Entity entity = entityArray[index];
          entityManager = this.EntityManager;
          Citizen componentData = entityManager.GetComponentData<Citizen>(entity);
          entityManager = this.EntityManager;
          CitizenAge newAge;
          if (entityManager.HasComponent<Game.Citizens.Child>(entity))
          {
            newAge = CitizenAge.Child;
            entityManager = this.EntityManager;
            entityManager.RemoveComponent<Game.Citizens.Child>(entity);
          }
          else
          {
            entityManager = this.EntityManager;
            if (entityManager.HasComponent<Teen>(entity))
            {
              newAge = CitizenAge.Teen;
              entityManager = this.EntityManager;
              entityManager.RemoveComponent<Teen>(entity);
            }
            else
            {
              entityManager = this.EntityManager;
              if (entityManager.HasComponent<Adult>(entity))
              {
                newAge = CitizenAge.Adult;
                entityManager = this.EntityManager;
                entityManager.RemoveComponent<Adult>(entity);
              }
              else
              {
                newAge = CitizenAge.Elderly;
                entityManager = this.EntityManager;
                entityManager.RemoveComponent<Elderly>(entity);
              }
            }
          }
          componentData.SetAge(newAge);
          entityManager = this.EntityManager;
          entityManager.SetComponentData<Citizen>(entity, componentData);
        }
        entityArray.Dispose();
      }
      if (context.version < Game.Version.prefabRefAbuseFix)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_PrefabRefQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> componentDataArray = this.m_PrefabRefQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < componentDataArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          if (!entityManager.HasComponent<PrefabData>((Entity) componentDataArray[index]))
          {
            entityManager = this.EntityManager;
            entityManager.DestroyEntity(entityArray[index]);
          }
        }
        entityArray.Dispose();
        componentDataArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_LabelMaterialQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<LabelMaterial>(this.m_LabelMaterialQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ArrowMaterialQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<ArrowMaterial>(this.m_ArrowMaterialQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.trainRouteSecondaryModelFix && !this.m_VehicleModelQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_VehicleModelQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          VehicleModel component;
          if (this.EntityManager.TryGetComponent<VehicleModel>(entityArray[index], out component))
          {
            if (component.m_SecondaryPrefab != Entity.Null)
            {
              entityManager = this.EntityManager;
              if (entityManager.HasComponent<TrainEngineData>(component.m_PrimaryPrefab))
              {
                component.m_SecondaryPrefab = Entity.Null;
                entityManager = this.EntityManager;
                entityManager.SetComponentData<VehicleModel>(entityArray[index], component);
              }
            }
          }
          else
          {
            entityManager = this.EntityManager;
            entityManager.AddComponentData<VehicleModel>(entityArray[index], new VehicleModel());
          }
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.enableableLocked && !this.m_LockedQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_LockedQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          if (!entityManager.HasComponent<Locked>(entityArray[index]))
          {
            entityManager = this.EntityManager;
            entityManager.AddComponent<Locked>(entityArray[index]);
            entityManager = this.EntityManager;
            entityManager.SetComponentEnabled<Locked>(entityArray[index], false);
          }
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.pedestrianBorderCost && !this.m_OutsideUpdateQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Updated>(this.m_OutsideUpdateQuery);
      }
      if (context.version < Game.Version.passengerWaitTimeCost)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_WaitingPassengersQuery.IsEmptyIgnoreFilter)
        {
          entityManager = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager.AddComponent<WaitingPassengers>(this.m_WaitingPassengersQuery);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_WaitingPassengersQuery2.IsEmptyIgnoreFilter)
        {
          entityManager = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager.RemoveComponent<WaitingPassengers>(this.m_WaitingPassengersQuery2);
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.pillarTerrainModification && !this.m_PillarQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_PillarQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> componentDataArray = this.m_PillarQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          if (entityManager.HasComponent<PillarData>(componentDataArray[index].m_Prefab))
          {
            entityManager = this.EntityManager;
            entityManager.AddComponent<Pillar>(entityArray[index]);
          }
        }
        entityArray.Dispose();
        componentDataArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.buildingEfficiencyRework && !this.m_LegacyEfficiencyQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Efficiency>(this.m_LegacyEfficiencyQuery);
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.RemoveComponent<Game.Buildings.BuildingEfficiency>(this.m_LegacyEfficiencyQuery);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_LegacyEfficiencyQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          PrefabRef component9;
          ConsumptionData component10;
          if (this.EntityManager.TryGetComponent<PrefabRef>(entityArray[index], out component9) && this.EntityManager.TryGetComponent<ConsumptionData>((Entity) component9, out component10) && (double) component10.m_TelecomNeed > 0.0)
          {
            entityManager = this.EntityManager;
            entityManager.AddComponent<TelecomConsumer>(entityArray[index]);
          }
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.signatureBuildingComponent && !this.m_SignatureQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Game.Buildings.Signature>(this.m_SignatureQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.missingOwnerFix && !this.m_SubObjectOwnerQuery.IsEmptyIgnoreFilter)
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_SubObjectOwnerQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          Owner component;
          if (this.EntityManager.TryGetComponent<Owner>(entityArray[index], out component))
          {
            entityManager = this.EntityManager;
            if (!entityManager.Exists(component.m_Owner))
            {
              entityManager = this.EntityManager;
              entityManager.DestroyEntity(entityArray[index]);
              ++num;
            }
          }
        }
        entityArray.Dispose();
        if (num != 0)
          UnityEngine.Debug.LogWarning((object) string.Format("Destroyed {0} entities with missing owners", (object) num));
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.dangerLevel && !this.m_DangerLevelMissingQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Game.Events.DangerLevel>(this.m_DangerLevelMissingQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.meshGroups && !this.m_MeshGroupQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<MeshGroup>(this.m_MeshGroupQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.surfaceStates && !this.m_ObjectSurfaceQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Game.Objects.Surface>(this.m_ObjectSurfaceQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.meshColors && !this.m_MeshColorQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<MeshColor>(this.m_MeshColorQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.plantUpdateFrame && !this.m_UpdateFrameQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_UpdateFrameQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.AddSharedComponent<UpdateFrame>(entityArray[index], new UpdateFrame((uint) (index & 15)));
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.fenceColors && !this.m_PseudoRandomSeedQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_PseudoRandomSeedQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        Unity.Mathematics.Random random = new Unity.Mathematics.Random(math.max(1U, (uint) DateTime.Now.Ticks));
        for (int index = 0; index < entityArray.Length; ++index)
        {
          entityManager = this.EntityManager;
          entityManager.AddComponentData<PseudoRandomSeed>(entityArray[index], new PseudoRandomSeed(ref random));
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.fenceColors && !this.m_FenceQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_FenceQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          Entity entity1 = entityArray[index];
          PrefabRef component11;
          if (this.EntityManager.TryGetComponent<PrefabRef>(entity1, out component11))
          {
            NetLaneData component12;
            if (this.EntityManager.TryGetComponent<NetLaneData>(component11.m_Prefab, out component12) && (component12.m_Flags & Game.Prefabs.LaneFlags.PseudoRandom) != (Game.Prefabs.LaneFlags) 0)
            {
              PseudoRandomSeed component13 = new PseudoRandomSeed();
              Entity entity2 = entity1;
              Owner component14;
              while (this.EntityManager.TryGetComponent<Owner>(entity2, out component14) && !this.EntityManager.TryGetComponent<PseudoRandomSeed>(component14.m_Owner, out component13))
                entity2 = component14.m_Owner;
              entityManager = this.EntityManager;
              entityManager.AddComponentData<PseudoRandomSeed>(entity1, component13);
              entityManager = this.EntityManager;
              entityManager.AddComponent<MeshColor>(entity1);
            }
            entityManager = this.EntityManager;
            if (entityManager.HasComponent<PlantData>(component11.m_Prefab))
            {
              entityManager = this.EntityManager;
              entityManager.AddComponent<Plant>(entity1);
              entityManager = this.EntityManager;
              entityManager.AddSharedComponent<UpdateFrame>(entity1, new UpdateFrame((uint) (index & 15)));
            }
          }
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.obsoleteNetPrefabs && !this.m_NetGeometrySectionQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<NetGeometryComposition>(this.m_NetGeometrySectionQuery);
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<NetGeometrySection>(this.m_NetGeometrySectionQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.obsoleteNetLanePrefabs && !this.m_NetLaneArchetypeDataQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<NetLaneArchetypeData>(this.m_NetLaneArchetypeDataQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.pathfindRestrictions && !this.m_PathfindUpdatedQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<PathfindUpdated>(this.m_PathfindUpdatedQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (context.version < Game.Version.cacheRouteColors && !this.m_RouteColorQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_RouteColorQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          CurrentRoute component15;
          Game.Routes.Color component16;
          if (this.EntityManager.TryGetComponent<CurrentRoute>(entityArray[index], out component15) && this.EntityManager.TryGetComponent<Game.Routes.Color>(component15.m_Route, out component16))
          {
            entityManager = this.EntityManager;
            entityManager.AddComponentData<Game.Routes.Color>(entityArray[index], component16);
          }
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!(context.version < Game.Version.deathWaveMitigation) || this.m_CitizenQuery.IsEmptyIgnoreFilter)
        return;
      Unity.Mathematics.Random random1 = RandomSeed.Next().GetRandom(0);
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray1 = this.m_CitizenQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      TimeData singleton1 = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>()).GetSingleton<TimeData>();
      uint frameIndex = this.World.GetOrCreateSystemManaged<SimulationSystem>().frameIndex;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int day = TimeSystem.GetDay(frameIndex, this.__query_1938549531_0.GetSingleton<TimeData>());
      for (int index = 0; index < entityArray1.Length; ++index)
      {
        HealthProblem component17;
        if (this.EntityManager.TryGetComponent<HealthProblem>(entityArray1[index], out component17) && CitizenUtils.IsDead(component17))
        {
          if (!(component17.m_HealthcareRequest == Entity.Null))
          {
            entityManager = this.EntityManager;
            if (entityManager.HasComponent<Dispatched>(component17.m_HealthcareRequest))
              goto label_452;
          }
          entityManager = this.EntityManager;
          entityManager.AddComponent<Deleted>(entityArray1[index]);
          continue;
        }
label_452:
        Citizen component18;
        // ISSUE: reference to a compiler-generated method
        if (this.EntityManager.TryGetComponent<Citizen>(entityArray1[index], out component18) && (double) component18.GetAgeInDays(frameIndex, singleton1) >= (double) AgingSystem.GetElderAgeLimitInDays() && random1.NextInt(100) > 1)
        {
          switch (random1.NextInt(3))
          {
            case 0:
              component18.m_BirthDay = (short) (day - 54 + random1.NextInt(18));
              break;
            case 1:
              component18.m_BirthDay = (short) (day - 69 + random1.NextInt(21));
              break;
            default:
              component18.m_BirthDay = (short) (day - 84 + random1.NextInt(21));
              break;
          }
          component18.SetAge(CitizenAge.Adult);
          entityManager = this.EntityManager;
          entityManager.SetComponentData<Citizen>(entityArray1[index], component18);
        }
      }
      entityArray1.Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_1938549531_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<TimeData>()
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

    [Preserve]
    public RequiredComponentSystem()
    {
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct TypeHandle
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
      }
    }
  }
}
