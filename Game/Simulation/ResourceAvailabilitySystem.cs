// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ResourceAvailabilitySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
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
namespace Game.Simulation
{
  [CompilerGenerated]
  public class ResourceAvailabilitySystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private const uint UPDATE_INTERVAL = 64;
    private EntityQuery m_EdgeGroup;
    private EntityQuery m_WorkplaceGroup;
    private EntityQuery m_ServiceGroup;
    private EntityQuery m_RenterGroup;
    private EntityQuery m_ConvenienceFoodStoreGroup;
    private EntityQuery m_OutsideConnectionGroup;
    private EntityQuery m_AttractionGroup;
    private EntityQuery m_ResourceSellerGroup;
    private EntityQuery m_TaxiQuery;
    private EntityQuery m_BusStopQuery;
    private EntityQuery m_TramSubwayQuery;
    private EntityQuery m_ParkingLaneQuery;
    private SimulationSystem m_SimulationSystem;
    private PathfindQueueSystem m_PathfindQueueSystem;
    private AirwaySystem m_AirwaySystem;
    private ResourceSystem m_ResourceSystem;
    private PathfindTargetSeekerData m_TargetSeekerData;
    private Entity m_AvailabilityContainer;
    private AvailableResource m_LastQueriedResource;
    private AvailableResource m_LastWrittenResource;
    private ResourceAvailabilitySystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    public AvailableResource appliedResource { get; private set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindQueueSystem = this.World.GetOrCreateSystemManaged<PathfindQueueSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirwaySystem = this.World.GetOrCreateSystemManaged<AirwaySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeGroup = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.Edge>(), ComponentType.ReadWrite<ResourceAvailability>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_WorkplaceGroup = this.GetEntityQuery(ComponentType.ReadOnly<WorkProvider>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Game.Objects.OutsideConnection>());
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceGroup = this.GetEntityQuery(ComponentType.ReadOnly<ServiceAvailable>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_RenterGroup = this.GetEntityQuery(ComponentType.ReadOnly<ResidentialProperty>(), ComponentType.ReadOnly<Renter>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ConvenienceFoodStoreGroup = this.GetEntityQuery(ComponentType.ReadOnly<ServiceAvailable>(), ComponentType.ReadOnly<ResourceSeller>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideConnectionGroup = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Game.Objects.ElectricityOutsideConnection>(), ComponentType.Exclude<Game.Objects.WaterPipeOutsideConnection>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_AttractionGroup = this.GetEntityQuery(ComponentType.ReadOnly<AttractivenessProvider>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSellerGroup = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<ResourceSeller>(),
          ComponentType.ReadOnly<Game.Companies.StorageCompany>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<ServiceAvailable>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_TaxiQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<ServiceDispatch>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Buildings.TransportDepot>(),
          ComponentType.ReadOnly<Game.Vehicles.Taxi>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_BusStopQuery = this.GetEntityQuery(ComponentType.ReadOnly<BusStop>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_TramSubwayQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Routes.TransportStop>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<TramStop>(),
          ComponentType.ReadOnly<SubwayStop>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ParkingLaneQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.ParkingLane>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_TargetSeekerData = new PathfindTargetSeekerData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_AvailabilityContainer = this.EntityManager.CreateEntity(ComponentType.ReadWrite<AvailabilityElement>());
    }

    private AvailabilityParameters GetAvailabilityParameters(
      AvailableResource resource,
      ResourcePrefabs prefabs,
      ComponentLookup<ResourceData> datas)
    {
      switch (resource)
      {
        case AvailableResource.Workplaces:
          return new AvailabilityParameters()
          {
            m_DensityWeight = 0.05f,
            m_CostFactor = 0.1f,
            m_ResultFactor = 0.08f
          };
        case AvailableResource.Services:
        case AvailableResource.ConvenienceFoodStore:
        case AvailableResource.Attractiveness:
          return new AvailabilityParameters()
          {
            m_DensityWeight = 0.05f,
            m_CostFactor = 0.1f,
            m_ResultFactor = 1f
          };
        case AvailableResource.UneducatedCitizens:
        case AvailableResource.EducatedCitizens:
          return new AvailabilityParameters()
          {
            m_DensityWeight = 0.05f,
            m_CostFactor = 0.5f,
            m_ResultFactor = 1f
          };
        case AvailableResource.OutsideConnection:
          return new AvailabilityParameters()
          {
            m_DensityWeight = 0.05f,
            m_CostFactor = 0.1f,
            m_ResultFactor = 3f
          };
        case AvailableResource.GrainSupply:
        case AvailableResource.VegetableSupply:
        case AvailableResource.WoodSupply:
        case AvailableResource.TextilesSupply:
        case AvailableResource.ConvenienceFoodSupply:
        case AvailableResource.PaperSupply:
        case AvailableResource.VehiclesSupply:
        case AvailableResource.OilSupply:
        case AvailableResource.PetrochemicalsSupply:
        case AvailableResource.OreSupply:
        case AvailableResource.MetalsSupply:
        case AvailableResource.ElectronicsSupply:
        case AvailableResource.PlasticsSupply:
        case AvailableResource.CoalSupply:
        case AvailableResource.StoneSupply:
        case AvailableResource.LivestockSupply:
        case AvailableResource.CottonSupply:
        case AvailableResource.SteelSupply:
        case AvailableResource.MineralSupply:
        case AvailableResource.ChemicalSupply:
        case AvailableResource.MachinerySupply:
        case AvailableResource.BeveragesSupply:
        case AvailableResource.TimberSupply:
          Resource resource1 = EconomyUtils.GetResource(resource);
          float num = 0.1f;
          if (resource1 != Resource.NoResource)
            num = 0.1f * EconomyUtils.GetTransportCost(1f, 0, EconomyUtils.GetWeight(resource1, prefabs, ref datas), StorageTransferFlags.Car);
          return new AvailabilityParameters()
          {
            m_DensityWeight = 0.05f,
            m_CostFactor = num,
            m_ResultFactor = 0.01f
          };
        case AvailableResource.Taxi:
          return new AvailabilityParameters()
          {
            m_DensityWeight = 0.1f,
            m_CostFactor = 0.05f,
            m_ResultFactor = 1f
          };
        case AvailableResource.Bus:
          return new AvailabilityParameters()
          {
            m_DensityWeight = 0.05f,
            m_CostFactor = 0.1f,
            m_ResultFactor = 1f
          };
        case AvailableResource.TramSubway:
          return new AvailabilityParameters()
          {
            m_DensityWeight = 0.05f,
            m_CostFactor = 0.1f,
            m_ResultFactor = 1f
          };
        default:
          return new AvailabilityParameters()
          {
            m_DensityWeight = 1f,
            m_CostFactor = 1f,
            m_ResultFactor = 1f
          };
      }
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write((int) this.m_LastWrittenResource);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      int x;
      reader.Read(out x);
      // ISSUE: reference to a compiler-generated field
      this.m_LastQueriedResource = AvailableResource.Count;
      // ISSUE: reference to a compiler-generated field
      this.m_LastWrittenResource = (AvailableResource) math.clamp(x, 0, 32);
      this.appliedResource = AvailableResource.Count;
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LastQueriedResource = AvailableResource.Count;
      // ISSUE: reference to a compiler-generated field
      this.m_LastWrittenResource = AvailableResource.TramSubway;
      this.appliedResource = AvailableResource.Count;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_LastQueriedResource != AvailableResource.Count)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_LastWrittenResource = this.m_LastQueriedResource;
        // ISSUE: reference to a compiler-generated field
        this.appliedResource = this.m_LastWrittenResource;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_LastQueriedResource = this.m_LastWrittenResource;
        // ISSUE: reference to a compiler-generated field
        this.m_LastWrittenResource = AvailableResource.Count;
      }
      // ISSUE: reference to a compiler-generated field
      if (++this.m_LastQueriedResource == AvailableResource.Count)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LastQueriedResource = AvailableResource.Workplaces;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      AvailabilityAction action = new AvailabilityAction(Allocator.Persistent, this.GetAvailabilityParameters(this.m_LastQueriedResource, this.m_ResourceSystem.GetPrefabs(), this.GetComponentLookup<ResourceData>(true)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      JobHandle locations = this.FindLocations(this.m_LastQueriedResource, action.data.m_Sources, action.data.m_Providers, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      if (this.m_LastWrittenResource != AvailableResource.Count)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Dependency = JobHandle.CombineDependencies(this.ApplyAvailability(this.m_LastWrittenResource, this.Dependency, locations), locations);
      }
      else
        this.Dependency = locations;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindQueueSystem.Enqueue(action, this.m_AvailabilityContainer, locations, this.m_SimulationSystem.frameIndex + 64U, (object) this);
    }

    private JobHandle ApplyAvailability(
      AvailableResource resource,
      JobHandle inputDeps,
      JobHandle pathDeps)
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<AvailabilityElement> nativeArray = this.EntityManager.GetBuffer<AvailabilityElement>(this.m_AvailabilityContainer, true).AsNativeArray();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ResourceAvailability_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = new ResourceAvailabilitySystem.ClearAvailabilityJob()
      {
        m_ResourceType = resource,
        m_ResourceAvailabilityType = this.__TypeHandle.__Game_Net_ResourceAvailability_RW_BufferTypeHandle
      }.ScheduleParallel<ResourceAvailabilitySystem.ClearAvailabilityJob>(this.m_EdgeGroup, inputDeps);
      JobHandle jobHandle2;
      if (resource == AvailableResource.Taxi)
      {
        TimeAction action = new TimeAction(Allocator.Persistent);
        // ISSUE: reference to a compiler-generated field
        NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(this.m_TaxiQuery.CalculateEntityCount(), (AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_ServiceDistrict_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_Taxi_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ResourceAvailabilitySystem.FindTaxiDistrictsJob jobData1 = new ResourceAvailabilitySystem.FindTaxiDistrictsJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_TransportDepotType = this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentTypeHandle,
          m_TaxiType = this.__TypeHandle.__Game_Vehicles_Taxi_RO_ComponentTypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_TransportDepotData = this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentLookup,
          m_PrefabTransportDepotData = this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentLookup,
          m_ServiceDistricts = this.__TypeHandle.__Game_Areas_ServiceDistrict_RO_BufferLookup,
          m_Districts = nativeParallelHashSet.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ResourceAvailability_RW_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ParkingLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        ResourceAvailabilitySystem.ApplyTaxiAvailabilityJob jobData2 = new ResourceAvailabilitySystem.ApplyTaxiAvailabilityJob()
        {
          m_AvailabilityElements = nativeArray,
          m_Districts = nativeParallelHashSet,
          m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_CurrentDistrictData = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup,
          m_BorderDistrictData = this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentLookup,
          m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
          m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RW_ComponentLookup,
          m_ResourceAvailability = this.__TypeHandle.__Game_Net_ResourceAvailability_RW_BufferLookup
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ParkingLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PathfindTransportData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ResourceAvailabilitySystem.RefreshTaxiAvailabilityJob jobData3 = new ResourceAvailabilitySystem.RefreshTaxiAvailabilityJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_LaneType = this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_NetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
          m_PathfindTransportData = this.__TypeHandle.__Game_Prefabs_PathfindTransportData_RO_ComponentLookup,
          m_ParkingLaneType = this.__TypeHandle.__Game_Net_ParkingLane_RW_ComponentTypeHandle,
          m_TimeActions = action.m_TimeData.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        JobHandle job1 = jobData1.ScheduleParallel<ResourceAvailabilitySystem.FindTaxiDistrictsJob>(this.m_TaxiQuery, inputDeps);
        JobHandle inputDeps1 = jobData2.Schedule<ResourceAvailabilitySystem.ApplyTaxiAvailabilityJob>(nativeArray.Length, 4, JobHandle.CombineDependencies(jobHandle1, job1, pathDeps));
        // ISSUE: reference to a compiler-generated field
        EntityQuery parkingLaneQuery = this.m_ParkingLaneQuery;
        JobHandle dependsOn = inputDeps1;
        JobHandle dependencies = jobData3.ScheduleParallel<ResourceAvailabilitySystem.RefreshTaxiAvailabilityJob>(parkingLaneQuery, dependsOn);
        nativeParallelHashSet.Dispose(inputDeps1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PathfindQueueSystem.Enqueue(action, dependencies);
        jobHandle2 = dependencies;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ResourceAvailability_RW_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        jobHandle2 = new ResourceAvailabilitySystem.ApplyAvailabilityJob()
        {
          m_ResourceType = resource,
          m_AvailabilityElements = nativeArray,
          m_ResourceAvailability = this.__TypeHandle.__Game_Net_ResourceAvailability_RW_BufferLookup
        }.Schedule<ResourceAvailabilitySystem.ApplyAvailabilityJob>(nativeArray.Length, 16, jobHandle1);
      }
      return jobHandle2;
    }

    private JobHandle FindLocations(
      AvailableResource resource,
      UnsafeQueue<PathTarget> pathTargets,
      UnsafeQueue<AvailabilityProvider> providers,
      JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TargetSeekerData.Update((SystemBase) this, this.m_AirwaySystem.GetAirwayData());
      PathfindParameters pathfindParameters = new PathfindParameters();
      pathfindParameters.m_MaxSpeed = (float2) 111.111115f;
      pathfindParameters.m_WalkSpeed = (float2) 5.555556f;
      pathfindParameters.m_Weights = new PathfindWeights(1f, 1f, 1f, 1f);
      pathfindParameters.m_Methods = PathMethod.Road;
      pathfindParameters.m_PathfindFlags = PathfindFlags.Stable | PathfindFlags.IgnoreFlow | PathfindFlags.Simplified;
      pathfindParameters.m_IgnoredRules = RuleFlags.HasBlockage | RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidSlowTraffic;
      SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
      setupQueueTarget.m_Methods = PathMethod.Road;
      setupQueueTarget.m_RoadTypes = RoadTypes.Car;
      switch (resource)
      {
        case AvailableResource.Workplaces:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          return new ResourceAvailabilitySystem.FindWorkplaceLocationsJob()
          {
            m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
            m_WorkProviderType = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle,
            m_TargetSeeker = new PathfindTargetSeeker<PathfindTargetBuffer>(this.m_TargetSeekerData, pathfindParameters, setupQueueTarget, (PathfindTargetBuffer) pathTargets.AsParallelWriter(), RandomSeed.Next()),
            m_Providers = providers.AsParallelWriter()
          }.ScheduleParallel<ResourceAvailabilitySystem.FindWorkplaceLocationsJob>(this.m_WorkplaceGroup, inputDeps);
        case AvailableResource.Services:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          return new ResourceAvailabilitySystem.FindServiceLocationsJob()
          {
            m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
            m_ServiceAvailableType = this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentTypeHandle,
            m_TargetSeeker = new PathfindTargetSeeker<PathfindTargetBuffer>(this.m_TargetSeekerData, pathfindParameters, setupQueueTarget, (PathfindTargetBuffer) pathTargets.AsParallelWriter(), RandomSeed.Next()),
            m_Providers = providers.AsParallelWriter()
          }.ScheduleParallel<ResourceAvailabilitySystem.FindServiceLocationsJob>(this.m_ServiceGroup, inputDeps);
        case AvailableResource.UneducatedCitizens:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          return new ResourceAvailabilitySystem.FindConsumerLocationsJob()
          {
            m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
            m_RenterType = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle,
            m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
            m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
            m_TargetSeeker = new PathfindTargetSeeker<PathfindTargetBuffer>(this.m_TargetSeekerData, pathfindParameters, setupQueueTarget, (PathfindTargetBuffer) pathTargets.AsParallelWriter(), RandomSeed.Next()),
            m_Providers = providers.AsParallelWriter(),
            m_Educated = false
          }.ScheduleParallel<ResourceAvailabilitySystem.FindConsumerLocationsJob>(this.m_RenterGroup, inputDeps);
        case AvailableResource.EducatedCitizens:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          return new ResourceAvailabilitySystem.FindConsumerLocationsJob()
          {
            m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
            m_RenterType = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle,
            m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
            m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
            m_TargetSeeker = new PathfindTargetSeeker<PathfindTargetBuffer>(this.m_TargetSeekerData, pathfindParameters, setupQueueTarget, (PathfindTargetBuffer) pathTargets.AsParallelWriter(), RandomSeed.Next()),
            m_Providers = providers.AsParallelWriter(),
            m_Educated = true
          }.ScheduleParallel<ResourceAvailabilitySystem.FindConsumerLocationsJob>(this.m_RenterGroup, inputDeps);
        case AvailableResource.OutsideConnection:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          return new ResourceAvailabilitySystem.FindOutsideConnectionLocationsJob()
          {
            m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
            m_OutsideConnectionType = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle,
            m_TargetSeeker = new PathfindTargetSeeker<PathfindTargetBuffer>(this.m_TargetSeekerData, pathfindParameters, setupQueueTarget, (PathfindTargetBuffer) pathTargets.AsParallelWriter(), RandomSeed.Next()),
            m_Providers = providers.AsParallelWriter()
          }.ScheduleParallel<ResourceAvailabilitySystem.FindOutsideConnectionLocationsJob>(this.m_OutsideConnectionGroup, inputDeps);
        case AvailableResource.ConvenienceFoodStore:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          return new ResourceAvailabilitySystem.FindConvenienceFoodStoreLocationsJob()
          {
            m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
            m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
            m_IndustrialProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
            m_TargetSeeker = new PathfindTargetSeeker<PathfindTargetBuffer>(this.m_TargetSeekerData, pathfindParameters, setupQueueTarget, (PathfindTargetBuffer) pathTargets.AsParallelWriter(), RandomSeed.Next()),
            m_Providers = providers.AsParallelWriter()
          }.ScheduleParallel<ResourceAvailabilitySystem.FindConvenienceFoodStoreLocationsJob>(this.m_ConvenienceFoodStoreGroup, inputDeps);
        case AvailableResource.GrainSupply:
        case AvailableResource.VegetableSupply:
        case AvailableResource.WoodSupply:
        case AvailableResource.TextilesSupply:
        case AvailableResource.ConvenienceFoodSupply:
        case AvailableResource.PaperSupply:
        case AvailableResource.VehiclesSupply:
        case AvailableResource.OilSupply:
        case AvailableResource.PetrochemicalsSupply:
        case AvailableResource.OreSupply:
        case AvailableResource.MetalsSupply:
        case AvailableResource.ElectronicsSupply:
        case AvailableResource.PlasticsSupply:
        case AvailableResource.CoalSupply:
        case AvailableResource.StoneSupply:
        case AvailableResource.LivestockSupply:
        case AvailableResource.CottonSupply:
        case AvailableResource.SteelSupply:
        case AvailableResource.MineralSupply:
        case AvailableResource.ChemicalSupply:
        case AvailableResource.MachinerySupply:
        case AvailableResource.BeveragesSupply:
        case AvailableResource.TimberSupply:
          Resource resource1;
          switch (resource - 6)
          {
            case AvailableResource.Workplaces:
              resource1 = Resource.Grain;
              break;
            case AvailableResource.Services:
              resource1 = Resource.Vegetables;
              break;
            case AvailableResource.UneducatedCitizens:
              resource1 = Resource.Wood;
              break;
            case AvailableResource.EducatedCitizens:
              resource1 = Resource.Textiles;
              break;
            case AvailableResource.OutsideConnection:
              resource1 = Resource.ConvenienceFood;
              break;
            case AvailableResource.ConvenienceFoodStore:
              resource1 = Resource.Paper;
              break;
            case AvailableResource.GrainSupply:
              resource1 = Resource.Vehicles;
              break;
            case AvailableResource.VegetableSupply:
              resource1 = Resource.Oil;
              break;
            case AvailableResource.WoodSupply:
              resource1 = Resource.Petrochemicals;
              break;
            case AvailableResource.TextilesSupply:
              resource1 = Resource.Ore;
              break;
            case AvailableResource.ConvenienceFoodSupply:
              resource1 = Resource.Metals;
              break;
            case AvailableResource.PaperSupply:
              resource1 = Resource.Electronics;
              break;
            case AvailableResource.OilSupply:
              resource1 = Resource.Plastics;
              break;
            case AvailableResource.PetrochemicalsSupply:
              resource1 = Resource.Coal;
              break;
            case AvailableResource.OreSupply:
              resource1 = Resource.Stone;
              break;
            case AvailableResource.MetalsSupply:
              resource1 = Resource.Livestock;
              break;
            case AvailableResource.ElectronicsSupply:
              resource1 = Resource.Cotton;
              break;
            case AvailableResource.Attractiveness:
              resource1 = Resource.Steel;
              break;
            case AvailableResource.PlasticsSupply:
              resource1 = Resource.Minerals;
              break;
            case AvailableResource.CoalSupply:
              resource1 = Resource.Chemicals;
              break;
            case AvailableResource.StoneSupply:
              resource1 = Resource.Machinery;
              break;
            case AvailableResource.LivestockSupply:
              resource1 = Resource.Beverages;
              break;
            case AvailableResource.CottonSupply:
              resource1 = Resource.Timber;
              break;
            default:
              return inputDeps;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Companies_TradeCost_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          return new ResourceAvailabilitySystem.FindSellerLocationsJob()
          {
            m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
            m_TargetSeeker = new PathfindTargetSeeker<PathfindTargetBuffer>(this.m_TargetSeekerData, pathfindParameters, setupQueueTarget, (PathfindTargetBuffer) pathTargets.AsParallelWriter(), RandomSeed.Next()),
            m_Providers = providers.AsParallelWriter(),
            m_Resource = resource1,
            m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
            m_ProcessData = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
            m_StorageCompanies = this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup,
            m_StorageDatas = this.__TypeHandle.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup,
            m_TradeCosts = this.__TypeHandle.__Game_Companies_TradeCost_RO_BufferLookup
          }.ScheduleParallel<ResourceAvailabilitySystem.FindSellerLocationsJob>(this.m_ResourceSellerGroup, inputDeps);
        case AvailableResource.Attractiveness:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Buildings_AttractivenessProvider_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          return new ResourceAvailabilitySystem.FindAttractionLocationsJob()
          {
            m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
            m_AttractivenessProviderType = this.__TypeHandle.__Game_Buildings_AttractivenessProvider_RW_ComponentTypeHandle,
            m_TargetSeeker = new PathfindTargetSeeker<PathfindTargetBuffer>(this.m_TargetSeekerData, pathfindParameters, setupQueueTarget, (PathfindTargetBuffer) pathTargets.AsParallelWriter(), RandomSeed.Next()),
            m_Providers = providers.AsParallelWriter()
          }.ScheduleParallel<ResourceAvailabilitySystem.FindAttractionLocationsJob>(this.m_AttractionGroup, inputDeps);
        case AvailableResource.Taxi:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Pathfind_PathOwner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Vehicles_Taxi_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          return new ResourceAvailabilitySystem.FindTaxiLocationsJob()
          {
            m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
            m_TransportDepotType = this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentTypeHandle,
            m_TaxiType = this.__TypeHandle.__Game_Vehicles_Taxi_RO_ComponentTypeHandle,
            m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
            m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RO_ComponentTypeHandle,
            m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
            m_PathElementType = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferTypeHandle,
            m_TransportDepotData = this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentLookup,
            m_PrefabTransportDepotData = this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentLookup,
            m_TargetSeeker = new PathfindTargetSeeker<PathfindTargetBuffer>(this.m_TargetSeekerData, pathfindParameters, setupQueueTarget, (PathfindTargetBuffer) pathTargets.AsParallelWriter(), RandomSeed.Next()),
            m_Providers = providers.AsParallelWriter()
          }.ScheduleParallel<ResourceAvailabilitySystem.FindTaxiLocationsJob>(this.m_TaxiQuery, inputDeps);
        case AvailableResource.Bus:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          return new ResourceAvailabilitySystem.FindBusStopLocationsJob()
          {
            m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
            m_TargetSeeker = new PathfindTargetSeeker<PathfindTargetBuffer>(this.m_TargetSeekerData, pathfindParameters, setupQueueTarget, (PathfindTargetBuffer) pathTargets.AsParallelWriter(), RandomSeed.Next()),
            m_Providers = providers.AsParallelWriter()
          }.ScheduleParallel<ResourceAvailabilitySystem.FindBusStopLocationsJob>(this.m_BusStopQuery, inputDeps);
        case AvailableResource.TramSubway:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Routes_SubwayStop_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          return new ResourceAvailabilitySystem.FindTramSubwayLocationsJob()
          {
            m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
            m_SubWayStopData = this.__TypeHandle.__Game_Routes_SubwayStop_RO_ComponentLookup,
            m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
            m_TargetSeeker = new PathfindTargetSeeker<PathfindTargetBuffer>(this.m_TargetSeekerData, pathfindParameters, setupQueueTarget, (PathfindTargetBuffer) pathTargets.AsParallelWriter(), RandomSeed.Next()),
            m_Providers = providers.AsParallelWriter()
          }.ScheduleParallel<ResourceAvailabilitySystem.FindTramSubwayLocationsJob>(this.m_TramSubwayQuery, inputDeps);
        default:
          return inputDeps;
      }
    }

    private static void AddProvider(
      Entity provider,
      float capacity,
      UnsafeQueue<AvailabilityProvider>.ParallelWriter providers,
      ref PathfindTargetSeeker<PathfindTargetBuffer> targetSeeker,
      float cost)
    {
      if (targetSeeker.FindTargets(provider, cost) == 0)
        return;
      providers.Enqueue(new AvailabilityProvider(provider, capacity, cost));
    }

    private static void AddProvider(
      Entity provider,
      float capacity,
      UnsafeQueue<AvailabilityProvider>.ParallelWriter providers,
      ref PathfindTargetSeeker<PathfindTargetBuffer> targetSeeker)
    {
      if (targetSeeker.FindTargets(provider, 0.0f) == 0)
        return;
      providers.Enqueue(new AvailabilityProvider(provider, capacity, 0.0f));
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
    public ResourceAvailabilitySystem()
    {
    }

    [BurstCompile]
    private struct FindWorkplaceLocationsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> m_WorkProviderType;
      public PathfindTargetSeeker<PathfindTargetBuffer> m_TargetSeeker;
      public UnsafeQueue<AvailabilityProvider>.ParallelWriter m_Providers;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WorkProvider> nativeArray2 = chunk.GetNativeArray<WorkProvider>(ref this.m_WorkProviderType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ResourceAvailabilitySystem.AddProvider(nativeArray1[index], (float) (4 * nativeArray2[index].m_MaxWorkers), this.m_Providers, ref this.m_TargetSeeker);
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
    private struct FindAttractionLocationsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<AttractivenessProvider> m_AttractivenessProviderType;
      public PathfindTargetSeeker<PathfindTargetBuffer> m_TargetSeeker;
      public UnsafeQueue<AvailabilityProvider>.ParallelWriter m_Providers;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AttractivenessProvider> nativeArray2 = chunk.GetNativeArray<AttractivenessProvider>(ref this.m_AttractivenessProviderType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ResourceAvailabilitySystem.AddProvider(nativeArray1[index], (float) nativeArray2[index].m_Attractiveness, this.m_Providers, ref this.m_TargetSeeker);
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
    private struct FindServiceLocationsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ServiceAvailable> m_ServiceAvailableType;
      public PathfindTargetSeeker<PathfindTargetBuffer> m_TargetSeeker;
      public UnsafeQueue<AvailabilityProvider>.ParallelWriter m_Providers;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ServiceAvailable> nativeArray2 = chunk.GetNativeArray<ServiceAvailable>(ref this.m_ServiceAvailableType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ResourceAvailabilitySystem.AddProvider(nativeArray1[index], (float) (5.0 + (double) nativeArray2[index].m_ServiceAvailable / 100.0), this.m_Providers, ref this.m_TargetSeeker);
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
    private struct FindConsumerLocationsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<Renter> m_RenterType;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      public PathfindTargetSeeker<PathfindTargetBuffer> m_TargetSeeker;
      public UnsafeQueue<AvailabilityProvider>.ParallelWriter m_Providers;
      public bool m_Educated;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Renter> bufferAccessor = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          Entity provider = nativeArray[index1];
          DynamicBuffer<Renter> dynamicBuffer = bufferAccessor[index1];
          int num = 0;
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_HouseholdCitizens.HasBuffer(dynamicBuffer[index2].m_Renter))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<HouseholdCitizen> householdCitizen = this.m_HouseholdCitizens[dynamicBuffer[index2].m_Renter];
              for (int index3 = 0; index3 < householdCitizen.Length; ++index3)
              {
                Entity citizen1 = householdCitizen[index3].m_Citizen;
                // ISSUE: reference to a compiler-generated field
                if (this.m_Citizens.HasComponent(citizen1))
                {
                  // ISSUE: reference to a compiler-generated field
                  Citizen citizen2 = this.m_Citizens[citizen1];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_Citizens[citizen1].GetAge() == CitizenAge.Adult)
                  {
                    int educationLevel = citizen2.GetEducationLevel();
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (educationLevel > 1 && this.m_Educated || educationLevel <= 1 && !this.m_Educated)
                      ++num;
                  }
                }
              }
            }
          }
          if (num != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            ResourceAvailabilitySystem.AddProvider(provider, 2f * (float) num, this.m_Providers, ref this.m_TargetSeeker);
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
    private struct FindConvenienceFoodStoreLocationsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      public PathfindTargetSeeker<PathfindTargetBuffer> m_TargetSeeker;
      public UnsafeQueue<AvailabilityProvider>.ParallelWriter m_Providers;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity provider = nativeArray1[index];
          Entity prefab = nativeArray2[index].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_IndustrialProcessDatas.HasComponent(prefab) && (this.m_IndustrialProcessDatas[prefab].m_Output.m_Resource & Resource.ConvenienceFood) != Resource.NoResource)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            ResourceAvailabilitySystem.AddProvider(provider, 10f, this.m_Providers, ref this.m_TargetSeeker);
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
    private struct FindOutsideConnectionLocationsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
      public PathfindTargetSeeker<PathfindTargetBuffer> m_TargetSeeker;
      public UnsafeQueue<AvailabilityProvider>.ParallelWriter m_Providers;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ResourceAvailabilitySystem.AddProvider(nativeArray[index], 10f, this.m_Providers, ref this.m_TargetSeeker);
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
    private struct FindSellerLocationsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_ProcessData;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> m_StorageCompanies;
      [ReadOnly]
      public ComponentLookup<StorageCompanyData> m_StorageDatas;
      [ReadOnly]
      public BufferLookup<TradeCost> m_TradeCosts;
      public PathfindTargetSeeker<PathfindTargetBuffer> m_TargetSeeker;
      public UnsafeQueue<AvailabilityProvider>.ParallelWriter m_Providers;
      public Resource m_Resource;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Entity prefab = nativeArray2[index].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ProcessData.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_StorageCompanies.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((this.m_Resource & this.m_StorageDatas[prefab].m_StoredResources) != Resource.NoResource)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                TradeCost tradeCost = EconomyUtils.GetTradeCost(this.m_Resource, this.m_TradeCosts[entity]);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                ResourceAvailabilitySystem.AddProvider(entity, 100f, this.m_Providers, ref this.m_TargetSeeker, 100f * tradeCost.m_BuyCost);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((this.m_Resource & this.m_ProcessData[prefab].m_Output.m_Resource) != Resource.NoResource)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                ResourceAvailabilitySystem.AddProvider(entity, 100f, this.m_Providers, ref this.m_TargetSeeker);
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

    [BurstCompile]
    private struct FindTaxiLocationsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.TransportDepot> m_TransportDepotType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.Taxi> m_TaxiType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportDepot> m_TransportDepotData;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> m_PrefabTransportDepotData;
      public PathfindTargetSeeker<PathfindTargetBuffer> m_TargetSeeker;
      public UnsafeQueue<AvailabilityProvider>.ParallelWriter m_Providers;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.TransportDepot> nativeArray2 = chunk.GetNativeArray<Game.Buildings.TransportDepot>(ref this.m_TransportDepotType);
        if (nativeArray2.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            Game.Buildings.TransportDepot transportDepot = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            if ((transportDepot.m_Flags & (TransportDepotFlags.HasAvailableVehicles | TransportDepotFlags.HasDispatchCenter)) == (TransportDepotFlags.HasAvailableVehicles | TransportDepotFlags.HasDispatchCenter) && this.m_PrefabTransportDepotData[nativeArray3[index].m_Prefab].m_TransportType == TransportType.Taxi)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              ResourceAvailabilitySystem.AddProvider(nativeArray1[index], (float) transportDepot.m_AvailableVehicles, this.m_Providers, ref this.m_TargetSeeker);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Vehicles.Taxi> nativeArray4 = chunk.GetNativeArray<Game.Vehicles.Taxi>(ref this.m_TaxiType);
          if (nativeArray4.Length == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray5 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PathOwner> nativeArray6 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<PathElement> bufferAccessor = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
          for (int index = 0; index < nativeArray4.Length; ++index)
          {
            Owner owner = nativeArray5[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransportDepotData.HasComponent(owner.m_Owner) && (this.m_TransportDepotData[owner.m_Owner].m_Flags & TransportDepotFlags.HasDispatchCenter) != (TransportDepotFlags) 0)
            {
              Game.Vehicles.Taxi taxi = nativeArray4[index];
              DynamicBuffer<PathElement> dynamicBuffer = bufferAccessor[index];
              Entity entity = nativeArray1[index];
              PathOwner pathOwner = nativeArray6[index];
              if ((taxi.m_State & TaxiFlags.Dispatched) != (TaxiFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                ResourceAvailabilitySystem.AddProvider(entity, 0.1f, this.m_Providers, ref this.m_TargetSeeker);
              }
              else
              {
                int num = dynamicBuffer.Length - taxi.m_ExtraPathElementCount;
                if (num <= 0 || num > dynamicBuffer.Length)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  ResourceAvailabilitySystem.AddProvider(entity, 1f, this.m_Providers, ref this.m_TargetSeeker);
                }
                else
                {
                  float cost = math.max(0.0f, (float) (num - pathOwner.m_ElementIndex) * taxi.m_PathElementTime);
                  PathElement pathElement = dynamicBuffer[num - 1];
                  // ISSUE: reference to a compiler-generated field
                  this.m_TargetSeeker.m_Buffer.Enqueue(new PathTarget(entity, pathElement.m_Target, pathElement.m_TargetDelta.y, 0.0f));
                  // ISSUE: reference to a compiler-generated field
                  this.m_Providers.Enqueue(new AvailabilityProvider(entity, 1f, cost));
                }
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

    [BurstCompile]
    private struct FindBusStopLocationsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public PathfindTargetSeeker<PathfindTargetBuffer> m_TargetSeeker;
      public UnsafeQueue<AvailabilityProvider>.ParallelWriter m_Providers;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ResourceAvailabilitySystem.AddProvider(nativeArray[index], 10f, this.m_Providers, ref this.m_TargetSeeker);
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
    private struct FindTramSubwayLocationsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentLookup<SubwayStop> m_SubWayStopData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      public PathfindTargetSeeker<PathfindTargetBuffer> m_TargetSeeker;
      public UnsafeQueue<AvailabilityProvider>.ParallelWriter m_Providers;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          Entity owner = nativeArray[index];
          Owner componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubWayStopData.HasComponent(owner) && this.m_OwnerData.TryGetComponent(owner, out componentData))
            owner = componentData.m_Owner;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ResourceAvailabilitySystem.AddProvider(owner, 10f, this.m_Providers, ref this.m_TargetSeeker);
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
    private struct ClearAvailabilityJob : IJobChunk
    {
      [ReadOnly]
      public AvailableResource m_ResourceType;
      public BufferTypeHandle<ResourceAvailability> m_ResourceAvailabilityType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ResourceAvailability> bufferAccessor = chunk.GetBufferAccessor<ResourceAvailability>(ref this.m_ResourceAvailabilityType);
        for (int index = 0; index < bufferAccessor.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          bufferAccessor[index][(int) this.m_ResourceType] = new ResourceAvailability();
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
    private struct ApplyAvailabilityJob : IJobParallelFor
    {
      [ReadOnly]
      public AvailableResource m_ResourceType;
      [ReadOnly]
      public NativeArray<AvailabilityElement> m_AvailabilityElements;
      [NativeDisableParallelForRestriction]
      public BufferLookup<ResourceAvailability> m_ResourceAvailability;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        AvailabilityElement availabilityElement = this.m_AvailabilityElements[index];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ResourceAvailability.HasBuffer(availabilityElement.m_Edge))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ResourceAvailability[availabilityElement.m_Edge][(int) this.m_ResourceType] = new ResourceAvailability()
        {
          m_Availability = availabilityElement.m_Availability
        };
      }
    }

    [BurstCompile]
    private struct FindTaxiDistrictsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.TransportDepot> m_TransportDepotType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.Taxi> m_TaxiType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportDepot> m_TransportDepotData;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> m_PrefabTransportDepotData;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> m_ServiceDistricts;
      public NativeParallelHashSet<Entity>.ParallelWriter m_Districts;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.TransportDepot> nativeArray2 = chunk.GetNativeArray<Game.Buildings.TransportDepot>(ref this.m_TransportDepotType);
        if (nativeArray2.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
          {
            // ISSUE: reference to a compiler-generated field
            if ((nativeArray2[index1].m_Flags & (TransportDepotFlags.HasAvailableVehicles | TransportDepotFlags.HasDispatchCenter)) == (TransportDepotFlags.HasAvailableVehicles | TransportDepotFlags.HasDispatchCenter) && this.m_PrefabTransportDepotData[nativeArray3[index1].m_Prefab].m_TransportType == TransportType.Taxi)
            {
              DynamicBuffer<ServiceDistrict> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ServiceDistricts.TryGetBuffer(nativeArray1[index1], out bufferData) && bufferData.Length != 0)
              {
                for (int index2 = 0; index2 < bufferData.Length; ++index2)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_Districts.Add(bufferData[index2].m_District);
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_Districts.Add(Entity.Null);
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Vehicles.Taxi> nativeArray4 = chunk.GetNativeArray<Game.Vehicles.Taxi>(ref this.m_TaxiType);
          if (nativeArray4.Length == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray5 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          for (int index3 = 0; index3 < nativeArray4.Length; ++index3)
          {
            Owner owner = nativeArray5[index3];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransportDepotData.HasComponent(owner.m_Owner) && (this.m_TransportDepotData[owner.m_Owner].m_Flags & TransportDepotFlags.HasDispatchCenter) != (TransportDepotFlags) 0)
            {
              DynamicBuffer<ServiceDistrict> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ServiceDistricts.TryGetBuffer(owner.m_Owner, out bufferData) && bufferData.Length != 0)
              {
                for (int index4 = 0; index4 < bufferData.Length; ++index4)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_Districts.Add(bufferData[index4].m_District);
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_Districts.Add(Entity.Null);
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

    [BurstCompile]
    private struct ApplyTaxiAvailabilityJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeArray<AvailabilityElement> m_AvailabilityElements;
      [ReadOnly]
      public NativeParallelHashSet<Entity> m_Districts;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> m_BorderDistrictData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<ResourceAvailability> m_ResourceAvailability;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        AvailabilityElement availabilityElement = this.m_AvailabilityElements[index];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Districts.Contains(Entity.Null))
        {
          BorderDistrict componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_BorderDistrictData.TryGetComponent(availabilityElement.m_Edge, out componentData1))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Districts.Contains(componentData1.m_Left) && !this.m_Districts.Contains(componentData1.m_Right))
              availabilityElement.m_Availability = (float2) 0.0f;
          }
          else
          {
            Owner componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_OwnerData.TryGetComponent(availabilityElement.m_Edge, out componentData2))
            {
              CurrentDistrict componentData3;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              for (; !this.m_CurrentDistrictData.TryGetComponent(componentData2.m_Owner, out componentData3); componentData2 = this.m_OwnerData[componentData2.m_Owner])
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.m_OwnerData.HasComponent(componentData2.m_Owner))
                {
                  availabilityElement.m_Availability = (float2) 0.0f;
                  goto label_11;
                }
              }
              // ISSUE: reference to a compiler-generated field
              if (!this.m_Districts.Contains(componentData3.m_District))
                availabilityElement.m_Availability = (float2) 0.0f;
            }
          }
        }
label_11:
        // ISSUE: reference to a compiler-generated field
        if (this.m_ResourceAvailability.HasBuffer(availabilityElement.m_Edge))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ResourceAvailability[availabilityElement.m_Edge][30] = new ResourceAvailability()
          {
            m_Availability = availabilityElement.m_Availability
          };
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubLanes.HasBuffer(availabilityElement.m_Edge))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[availabilityElement.m_Edge];
        int num1 = Mathf.RoundToInt(math.min((float) ushort.MaxValue, math.csum(availabilityElement.m_Availability) * 32767.5f));
        for (int index1 = 0; index1 < subLane1.Length; ++index1)
        {
          Entity subLane2 = subLane1[index1].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ParkingLaneData.HasComponent(subLane2))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Net.ParkingLane parkingLane = this.m_ParkingLaneData[subLane2];
            int num2 = math.select((int) parkingLane.m_TaxiAvailability * 3 + num1 + 3 >> 2, 0, num1 == 0);
            if (num2 != (int) parkingLane.m_TaxiAvailability)
            {
              parkingLane.m_TaxiAvailability = (ushort) num2;
              parkingLane.m_Flags |= ParkingLaneFlags.TaxiAvailabilityChanged;
            }
            parkingLane.m_Flags |= ParkingLaneFlags.TaxiAvailabilityUpdated;
            // ISSUE: reference to a compiler-generated field
            this.m_ParkingLaneData[subLane2] = parkingLane;
          }
        }
      }
    }

    [BurstCompile]
    private struct RefreshTaxiAvailabilityJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Lane> m_LaneType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_NetLaneData;
      [ReadOnly]
      public ComponentLookup<PathfindTransportData> m_PathfindTransportData;
      public ComponentTypeHandle<Game.Net.ParkingLane> m_ParkingLaneType;
      public NativeQueue<TimeActionData>.ParallelWriter m_TimeActions;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Lane> nativeArray2 = chunk.GetNativeArray<Lane>(ref this.m_LaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.ParkingLane> nativeArray3 = chunk.GetNativeArray<Game.Net.ParkingLane>(ref this.m_ParkingLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Game.Net.ParkingLane parkingLaneData = nativeArray3[index];
          if ((parkingLaneData.m_Flags & ParkingLaneFlags.TaxiAvailabilityUpdated) == (ParkingLaneFlags) 0 && parkingLaneData.m_TaxiAvailability != (ushort) 0)
          {
            parkingLaneData.m_TaxiAvailability = (ushort) 0;
            parkingLaneData.m_Flags |= ParkingLaneFlags.TaxiAvailabilityChanged;
          }
          if ((parkingLaneData.m_Flags & ParkingLaneFlags.TaxiAvailabilityChanged) != (ParkingLaneFlags) 0)
          {
            Lane lane = nativeArray2[index];
            TimeActionData timeActionData = new TimeActionData()
            {
              m_Owner = nativeArray1[index],
              m_StartNode = lane.m_StartNode,
              m_EndNode = lane.m_EndNode,
              m_Flags = TimeActionFlags.SetSecondary | TimeActionFlags.EnableForward
            };
            if ((parkingLaneData.m_Flags & ParkingLaneFlags.SecondaryStart) != (ParkingLaneFlags) 0)
            {
              timeActionData.m_SecondaryStartNode = parkingLaneData.m_SecondaryStartNode;
              timeActionData.m_SecondaryEndNode = lane.m_EndNode;
            }
            else
            {
              timeActionData.m_SecondaryStartNode = lane.m_StartNode;
              timeActionData.m_SecondaryEndNode = lane.m_EndNode;
            }
            if (parkingLaneData.m_TaxiAvailability != (ushort) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              PathfindTransportData pathfindTransportData = this.m_PathfindTransportData[this.m_NetLaneData[nativeArray4[index].m_Prefab].m_PathfindPrefab];
              timeActionData.m_Flags |= TimeActionFlags.EnableBackward;
              timeActionData.m_Time = pathfindTransportData.m_OrderingCost.m_Value.x + pathfindTransportData.m_StartingCost.m_Value.x + PathUtils.GetTaxiAvailabilityDelay(parkingLaneData);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_TimeActions.Enqueue(timeActionData);
          }
          parkingLaneData.m_Flags &= ~(ParkingLaneFlags.TaxiAvailabilityUpdated | ParkingLaneFlags.TaxiAvailabilityChanged);
          nativeArray3[index] = parkingLaneData;
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
      public BufferTypeHandle<ResourceAvailability> __Game_Net_ResourceAvailability_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.TransportDepot> __Game_Buildings_TransportDepot_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.Taxi> __Game_Vehicles_Taxi_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportDepot> __Game_Buildings_TransportDepot_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> __Game_Prefabs_TransportDepotData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> __Game_Areas_ServiceDistrict_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> __Game_Areas_BorderDistrict_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RW_ComponentLookup;
      public BufferLookup<ResourceAvailability> __Game_Net_ResourceAvailability_RW_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Lane> __Game_Net_Lane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathfindTransportData> __Game_Prefabs_PathfindTransportData_RO_ComponentLookup;
      public ComponentTypeHandle<Game.Net.ParkingLane> __Game_Net_ParkingLane_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> __Game_Companies_WorkProvider_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ServiceAvailable> __Game_Companies_ServiceAvailable_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Renter> __Game_Buildings_Renter_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> __Game_Companies_StorageCompany_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StorageCompanyData> __Game_Prefabs_StorageCompanyData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<TradeCost> __Game_Companies_TradeCost_RO_BufferLookup;
      public ComponentTypeHandle<AttractivenessProvider> __Game_Buildings_AttractivenessProvider_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<PathElement> __Game_Pathfind_PathElement_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<SubwayStop> __Game_Routes_SubwayStop_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ResourceAvailability_RW_BufferTypeHandle = state.GetBufferTypeHandle<ResourceAvailability>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TransportDepot_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.TransportDepot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Taxi_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.Taxi>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TransportDepot_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.TransportDepot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportDepotData_RO_ComponentLookup = state.GetComponentLookup<TransportDepotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_ServiceDistrict_RO_BufferLookup = state.GetBufferLookup<ServiceDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentLookup = state.GetComponentLookup<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_BorderDistrict_RO_ComponentLookup = state.GetComponentLookup<BorderDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RW_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ResourceAvailability_RW_BufferLookup = state.GetBufferLookup<ResourceAvailability>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindTransportData_RO_ComponentLookup = state.GetComponentLookup<PathfindTransportData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.ParkingLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WorkProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceAvailable_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceAvailable>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferTypeHandle = state.GetBufferTypeHandle<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageCompany_RO_ComponentLookup = state.GetComponentLookup<Game.Companies.StorageCompany>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup = state.GetComponentLookup<StorageCompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_TradeCost_RO_BufferLookup = state.GetBufferLookup<TradeCost>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_AttractivenessProvider_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AttractivenessProvider>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_SubwayStop_RO_ComponentLookup = state.GetComponentLookup<SubwayStop>(true);
      }
    }
  }
}
