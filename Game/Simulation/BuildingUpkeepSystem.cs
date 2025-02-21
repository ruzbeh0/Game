// Decompiled with JetBrains decompiler
// Type: Game.Simulation.BuildingUpkeepSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Economy;
using Game.Net;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using Game.Triggers;
using Game.Vehicles;
using Game.Zones;
using System;
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
  public class BuildingUpkeepSystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 16;
    public static readonly int kMaterialUpkeep = 4;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private ResourceSystem m_ResourceSystem;
    private ClimateSystem m_ClimateSystem;
    private CitySystem m_CitySystem;
    private IconCommandSystem m_IconCommandSystem;
    private TriggerSystem m_TriggerSystem;
    private ZoneBuiltRequirementSystem m_ZoneBuiltRequirementSystemSystem;
    private Game.Zones.SearchSystem m_ZoneSearchSystem;
    private ElectricityRoadConnectionGraphSystem m_ElectricityRoadConnectionGraphSystem;
    private WaterPipeRoadConnectionGraphSystem m_WaterPipeRoadConnectionGraphSystem;
    private NativeQueue<BuildingUpkeepSystem.UpkeepPayment> m_UpkeepExpenseQueue;
    private NativeQueue<Entity> m_LevelupQueue;
    private NativeQueue<Entity> m_LeveldownQueue;
    private EntityQuery m_BuildingPrefabGroup;
    private EntityQuery m_BuildingSettingsQuery;
    private EntityQuery m_BuildingGroup;
    public bool debugFastLeveling;
    private BuildingUpkeepSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (BuildingUpkeepSystem.kUpdatesPerDay * 16);
    }

    public static float GetHeatingMultiplier(float temperature)
    {
      return math.max(0.0f, 15f - temperature);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneBuiltRequirementSystemSystem = this.World.GetOrCreateSystemManaged<ZoneBuiltRequirementSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneSearchSystem = this.World.GetOrCreateSystemManaged<Game.Zones.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityRoadConnectionGraphSystem = this.World.GetOrCreateSystemManaged<ElectricityRoadConnectionGraphSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterPipeRoadConnectionGraphSystem = this.World.GetOrCreateSystemManaged<WaterPipeRoadConnectionGraphSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpkeepExpenseQueue = new NativeQueue<BuildingUpkeepSystem.UpkeepPayment>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<BuildingConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_LevelupQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LeveldownQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingGroup = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<BuildingCondition>(),
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadOnly<UpdateFrame>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Abandoned>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingPrefabGroup = this.GetEntityQuery(ComponentType.ReadOnly<Game.Prefabs.BuildingData>(), ComponentType.ReadOnly<BuildingSpawnGroupData>(), ComponentType.ReadOnly<PrefabData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BuildingGroup);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BuildingSettingsQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_UpkeepExpenseQueue.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_LevelupQueue.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_LeveldownQueue.Dispose();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, BuildingUpkeepSystem.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      BuildingConfigurationData singleton = this.m_BuildingSettingsQuery.GetSingleton<BuildingConfigurationData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_BuildingCondition_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BuildingUpkeepSystem.BuildingUpkeepJob jobData1 = new BuildingUpkeepSystem.BuildingUpkeepJob()
      {
        m_ConditionType = this.__TypeHandle.__Game_Buildings_BuildingCondition_RW_ComponentTypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_RenterType = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_ConsumptionDatas = this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup,
        m_Availabilities = this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup,
        m_BuildingDatas = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_BuildingPropertyDatas = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_CityModifierBufs = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_SignatureDatas = this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup,
        m_Abandoned = this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup,
        m_Destroyed = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_SpawnableBuildingDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_ZoneDatas = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup,
        m_Households = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_OwnedVehicles = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_DeliveryTrucks = this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup,
        m_City = this.m_CitySystem.City,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup,
        m_BuildingConfigurationData = singleton,
        m_UpdateFrameIndex = updateFrame,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_UpkeepExpenseQueue = this.m_UpkeepExpenseQueue.AsParallelWriter(),
        m_LevelupQueue = this.m_LevelupQueue.AsParallelWriter(),
        m_LevelDownQueue = this.m_LeveldownQueue.AsParallelWriter(),
        m_DebugFastLeveling = this.debugFastLeveling,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_TemperatureUpkeep = BuildingUpkeepSystem.GetHeatingMultiplier((float) this.m_ClimateSystem.temperature)
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<BuildingUpkeepSystem.BuildingUpkeepJob>(this.m_BuildingGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_ValidArea_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingSpawnGroupData_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
      JobHandle dependencies;
      JobHandle deps1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BuildingUpkeepSystem.LevelupJob jobData2 = new BuildingUpkeepSystem.LevelupJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_SpawnableBuildingType = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentTypeHandle,
        m_BuildingPropertyType = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentTypeHandle,
        m_ObjectGeometryType = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentTypeHandle,
        m_BuildingSpawnGroupType = this.__TypeHandle.__Game_Prefabs_BuildingSpawnGroupData_SharedComponentTypeHandle,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_BlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
        m_ValidAreaData = this.__TypeHandle.__Game_Zones_ValidArea_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabDatas = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup,
        m_SpawnableBuildings = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_Buildings = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_BuildingPropertyDatas = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_OfficeBuilding = this.__TypeHandle.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup,
        m_ZoneData = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup,
        m_Cells = this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup,
        m_BuildingConfigurationData = singleton,
        m_SpawnableBuildingChunks = this.m_BuildingPrefabGroup.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_ZoneSearchTree = this.m_ZoneSearchSystem.GetSearchTree(true, out dependencies),
        m_RandomSeed = RandomSeed.Next(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_LevelupQueue = this.m_LevelupQueue,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer(),
        m_TriggerBuffer = this.m_TriggerSystem.CreateActionBuffer(),
        m_ZoneBuiltLevelQueue = this.m_ZoneBuiltRequirementSystemSystem.GetZoneBuiltLevelQueue(out deps1)
      };
      this.Dependency = jobData2.Schedule<BuildingUpkeepSystem.LevelupJob>(JobUtils.CombineDependencies(this.Dependency, outJobHandle, dependencies, deps1));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ZoneSearchSystem.AddSearchTreeReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ZoneBuiltRequirementSystemSystem.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CrimeProducer_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BuildingUpkeepSystem.LeveldownJob jobData3 = new BuildingUpkeepSystem.LeveldownJob()
      {
        m_BuildingDatas = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_SpawnableBuildings = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_Buildings = this.__TypeHandle.__Game_Buildings_Building_RW_ComponentLookup,
        m_ElectricityConsumers = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup,
        m_GarbageProducers = this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup,
        m_MailProducers = this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup,
        m_WaterConsumers = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup,
        m_BuildingPropertyDatas = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_OfficeBuilding = this.__TypeHandle.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup,
        m_TriggerBuffer = this.m_TriggerSystem.CreateActionBuffer(),
        m_CrimeProducers = this.__TypeHandle.__Game_Buildings_CrimeProducer_RW_ComponentLookup,
        m_Renters = this.__TypeHandle.__Game_Buildings_Renter_RW_BufferLookup,
        m_BuildingConfigurationData = singleton,
        m_LeveldownQueue = this.m_LeveldownQueue,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer(),
        m_UpdatedElectricityRoadEdges = this.m_ElectricityRoadConnectionGraphSystem.GetEdgeUpdateQueue(out deps2),
        m_UpdatedWaterPipeRoadEdges = this.m_WaterPipeRoadConnectionGraphSystem.GetEdgeUpdateQueue(out deps3),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex
      };
      this.Dependency = jobData3.Schedule<BuildingUpkeepSystem.LeveldownJob>(JobHandle.CombineDependencies(this.Dependency, deps2, deps3));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ElectricityRoadConnectionGraphSystem.AddQueueWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BuildingUpkeepSystem.UpkeepPaymentJob jobData4 = new BuildingUpkeepSystem.UpkeepPaymentJob()
      {
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup,
        m_UpkeepExpenseQueue = this.m_UpkeepExpenseQueue
      };
      this.Dependency = jobData4.Schedule<BuildingUpkeepSystem.UpkeepPaymentJob>(this.Dependency);
    }

    public void DebugLevelUp(
      Entity building,
      ComponentLookup<BuildingCondition> conditions,
      ComponentLookup<SpawnableBuildingData> spawnables,
      ComponentLookup<PrefabRef> prefabRefs,
      ComponentLookup<ZoneData> zoneDatas,
      ComponentLookup<BuildingPropertyData> propertyDatas)
    {
      if (!conditions.HasComponent(building) || !prefabRefs.HasComponent(building))
        return;
      BuildingCondition condition = conditions[building];
      Entity prefab = prefabRefs[building].m_Prefab;
      if (!spawnables.HasComponent(prefab) || !propertyDatas.HasComponent(prefab))
        return;
      SpawnableBuildingData spawnable = spawnables[prefab];
      if (!zoneDatas.HasComponent(spawnable.m_ZonePrefab))
        return;
      ZoneData zoneData = zoneDatas[spawnable.m_ZonePrefab];
      // ISSUE: reference to a compiler-generated field
      this.m_LevelupQueue.Enqueue(building);
    }

    public void DebugLevelDown(
      Entity building,
      ComponentLookup<BuildingCondition> conditions,
      ComponentLookup<SpawnableBuildingData> spawnables,
      ComponentLookup<PrefabRef> prefabRefs,
      ComponentLookup<ZoneData> zoneDatas,
      ComponentLookup<BuildingPropertyData> propertyDatas)
    {
      if (!conditions.HasComponent(building) || !prefabRefs.HasComponent(building))
        return;
      BuildingCondition condition = conditions[building];
      Entity prefab = prefabRefs[building].m_Prefab;
      if (!spawnables.HasComponent(prefab) || !propertyDatas.HasComponent(prefab))
        return;
      SpawnableBuildingData spawnable = spawnables[prefab];
      if (!zoneDatas.HasComponent(spawnable.m_ZonePrefab))
        return;
      // ISSUE: reference to a compiler-generated field
      int levelingCost = BuildingUtils.GetLevelingCost(zoneDatas[spawnable.m_ZonePrefab].m_AreaType, propertyDatas[prefab], (int) spawnable.m_Level, this.EntityManager.GetBuffer<CityModifier>(this.m_CitySystem.City, true));
      condition.m_Condition = -3 * levelingCost / 2;
      conditions[building] = condition;
      // ISSUE: reference to a compiler-generated field
      this.m_LeveldownQueue.Enqueue(building);
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
    public BuildingUpkeepSystem()
    {
    }

    private struct UpkeepPayment
    {
      public Entity m_RenterEntity;
      public int m_Price;
    }

    [BurstCompile]
    private struct BuildingUpkeepJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<BuildingCondition> m_ConditionType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public BufferTypeHandle<Renter> m_RenterType;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDatas;
      [ReadOnly]
      public ComponentLookup<ZoneData> m_ZoneDatas;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> m_Resources;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDatas;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyDatas;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifierBufs;
      [ReadOnly]
      public ComponentLookup<Abandoned> m_Abandoned;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_Destroyed;
      [ReadOnly]
      public ComponentLookup<SignatureBuildingData> m_SignatureDatas;
      [ReadOnly]
      public ComponentLookup<Household> m_Households;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> m_OwnedVehicles;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> m_DeliveryTrucks;
      [ReadOnly]
      public BuildingConfigurationData m_BuildingConfigurationData;
      [ReadOnly]
      public ComponentLookup<ConsumptionData> m_ConsumptionDatas;
      [ReadOnly]
      public BufferLookup<ResourceAvailability> m_Availabilities;
      [ReadOnly]
      public Entity m_City;
      public uint m_UpdateFrameIndex;
      public uint m_SimulationFrame;
      public float m_TemperatureUpkeep;
      public bool m_DebugFastLeveling;
      public NativeQueue<BuildingUpkeepSystem.UpkeepPayment>.ParallelWriter m_UpkeepExpenseQueue;
      public NativeQueue<Entity>.ParallelWriter m_LevelupQueue;
      public NativeQueue<Entity>.ParallelWriter m_LevelDownQueue;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

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
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<BuildingCondition> nativeArray3 = chunk.GetNativeArray<BuildingCondition>(ref this.m_ConditionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Building> nativeArray4 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Renter> bufferAccessor = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          int num1 = 0;
          Entity entity1 = nativeArray1[index1];
          Entity prefab = nativeArray2[index1].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          ConsumptionData consumptionData = this.m_ConsumptionDatas[prefab];
          // ISSUE: reference to a compiler-generated field
          Game.Prefabs.BuildingData buildingData = this.m_BuildingDatas[prefab];
          // ISSUE: reference to a compiler-generated field
          BuildingPropertyData buildingPropertyData1 = this.m_BuildingPropertyDatas[prefab];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<CityModifier> cityModifierBuf = this.m_CityModifierBufs[this.m_City];
          // ISSUE: reference to a compiler-generated field
          SpawnableBuildingData spawnableBuildingData = this.m_SpawnableBuildingDatas[prefab];
          // ISSUE: reference to a compiler-generated field
          AreaType areaType = this.m_ZoneDatas[spawnableBuildingData.m_ZonePrefab].m_AreaType;
          // ISSUE: reference to a compiler-generated field
          BuildingPropertyData buildingPropertyData2 = this.m_BuildingPropertyDatas[prefab];
          int levelingCost = BuildingUtils.GetLevelingCost(areaType, buildingPropertyData2, (int) spawnableBuildingData.m_Level, cityModifierBuf);
          int num2 = spawnableBuildingData.m_Level == (byte) 5 ? BuildingUtils.GetLevelingCost(areaType, buildingPropertyData2, 4, cityModifierBuf) : levelingCost;
          if (areaType == AreaType.Residential && buildingPropertyData2.m_ResidentialProperties > 1)
            num2 = Mathf.RoundToInt((float) (num2 * (6 - (int) spawnableBuildingData.m_Level)) / math.sqrt((float) buildingPropertyData2.m_ResidentialProperties));
          DynamicBuffer<Renter> dynamicBuffer = bufferAccessor[index1];
          // ISSUE: reference to a compiler-generated field
          int val1 = consumptionData.m_Upkeep / BuildingUpkeepSystem.kUpdatesPerDay;
          // ISSUE: reference to a compiler-generated field
          int num3 = val1 / BuildingUpkeepSystem.kMaterialUpkeep;
          int num4 = num1 + (val1 - num3);
          // ISSUE: reference to a compiler-generated field
          Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint) (1UL + (ulong) entity1.Index * (ulong) this.m_SimulationFrame));
          Resource resource1 = random.NextBool() ? Resource.Timber : Resource.Concrete;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float marketPrice1 = EconomyUtils.GetMarketPrice(this.m_ResourceDatas[this.m_ResourcePrefabs[resource1]]);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num5 = math.sqrt((float) (buildingData.m_LotSize.x * buildingData.m_LotSize.y * buildingPropertyData1.CountProperties())) * this.m_TemperatureUpkeep / (float) BuildingUpkeepSystem.kUpdatesPerDay;
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex);
          GoodsDeliveryRequest goodsDeliveryRequest;
          if (random.NextInt(Mathf.RoundToInt(4000f * marketPrice1)) < num3)
          {
            // ISSUE: reference to a compiler-generated field
            ref EntityCommandBuffer.ParallelWriter local = ref this.m_CommandBuffer;
            int sortKey = unfilteredChunkIndex;
            Entity e = entity2;
            goodsDeliveryRequest = new GoodsDeliveryRequest();
            goodsDeliveryRequest.m_Amount = Math.Max(val1, 4000);
            goodsDeliveryRequest.m_Flags = GoodsDeliveryFlags.BuildingUpkeep | GoodsDeliveryFlags.CommercialAllowed | GoodsDeliveryFlags.IndustrialAllowed | GoodsDeliveryFlags.ImportAllowed;
            goodsDeliveryRequest.m_Resource = resource1;
            goodsDeliveryRequest.m_Target = entity1;
            GoodsDeliveryRequest component = goodsDeliveryRequest;
            local.AddComponent<GoodsDeliveryRequest>(sortKey, e, component);
          }
          Building building = nativeArray4[index1];
          // ISSUE: reference to a compiler-generated field
          if (this.m_Availabilities.HasBuffer(building.m_RoadEdge))
          {
            // ISSUE: reference to a compiler-generated field
            float availability1 = NetUtils.GetAvailability(this.m_Availabilities[building.m_RoadEdge], AvailableResource.WoodSupply, building.m_CurvePosition);
            // ISSUE: reference to a compiler-generated field
            float availability2 = NetUtils.GetAvailability(this.m_Availabilities[building.m_RoadEdge], AvailableResource.PetrochemicalsSupply, building.m_CurvePosition);
            float max = availability1 + availability2;
            Resource resource2;
            if ((double) max < 1.0 / 1000.0)
            {
              resource2 = random.NextBool() ? Resource.Wood : Resource.Petrochemicals;
            }
            else
            {
              resource2 = (double) random.NextFloat(max) <= (double) availability1 ? Resource.Wood : Resource.Petrochemicals;
              val1 = resource2 == Resource.Wood ? 4000 : 800;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float marketPrice2 = EconomyUtils.GetMarketPrice(this.m_ResourceDatas[this.m_ResourcePrefabs[resource2]]);
            if ((double) random.NextFloat((float) val1 * marketPrice2) < (double) num5)
            {
              // ISSUE: reference to a compiler-generated field
              Entity entity3 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex);
              int num6 = Mathf.RoundToInt((float) val1 * marketPrice2);
              // ISSUE: reference to a compiler-generated field
              ref EntityCommandBuffer.ParallelWriter local = ref this.m_CommandBuffer;
              int sortKey = unfilteredChunkIndex;
              Entity e = entity3;
              goodsDeliveryRequest = new GoodsDeliveryRequest();
              goodsDeliveryRequest.m_Amount = val1;
              goodsDeliveryRequest.m_Flags = GoodsDeliveryFlags.BuildingUpkeep | GoodsDeliveryFlags.CommercialAllowed | GoodsDeliveryFlags.IndustrialAllowed | GoodsDeliveryFlags.ImportAllowed;
              goodsDeliveryRequest.m_Resource = resource2;
              goodsDeliveryRequest.m_Target = entity1;
              GoodsDeliveryRequest component = goodsDeliveryRequest;
              local.AddComponent<GoodsDeliveryRequest>(sortKey, e, component);
              num4 += num6;
            }
          }
          int num7 = 0;
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            DynamicBuffer<Game.Economy.Resources> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Resources.TryGetBuffer(dynamicBuffer[index2].m_Renter, out bufferData))
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_Households.HasComponent(dynamicBuffer[index2].m_Renter))
              {
                num7 += EconomyUtils.GetResources(Resource.Money, bufferData);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_OwnedVehicles.HasBuffer(dynamicBuffer[index2].m_Renter))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  num7 += EconomyUtils.GetCompanyTotalWorth(bufferData, this.m_OwnedVehicles[dynamicBuffer[index2].m_Renter], this.m_LayoutElements, this.m_DeliveryTrucks, this.m_ResourcePrefabs, this.m_ResourceDatas);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  num7 += EconomyUtils.GetCompanyTotalWorth(bufferData, this.m_ResourcePrefabs, this.m_ResourceDatas);
                }
              }
            }
          }
          BuildingCondition buildingCondition = nativeArray3[index1];
          int num8 = 0;
          if (num4 > num7)
          {
            // ISSUE: reference to a compiler-generated field
            num8 = -this.m_BuildingConfigurationData.m_BuildingConditionDecrement * (int) math.pow(2f, (float) spawnableBuildingData.m_Level) * math.max(1, dynamicBuffer.Length);
          }
          else if (dynamicBuffer.Length > 0)
          {
            // ISSUE: reference to a compiler-generated field
            num8 = this.m_BuildingConfigurationData.m_BuildingConditionIncrement * (int) math.pow(2f, (float) spawnableBuildingData.m_Level) * math.max(1, dynamicBuffer.Length);
            int num9 = num4 / dynamicBuffer.Length;
            for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_UpkeepExpenseQueue.Enqueue(new BuildingUpkeepSystem.UpkeepPayment()
              {
                m_RenterEntity = dynamicBuffer[index3].m_Renter,
                m_Price = num9
              });
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_DebugFastLeveling)
            buildingCondition.m_Condition = levelingCost;
          else
            buildingCondition.m_Condition += num8;
          if (buildingCondition.m_Condition >= levelingCost)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LevelupQueue.Enqueue(nativeArray1[index1]);
            buildingCondition.m_Condition -= levelingCost;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Abandoned.HasComponent(nativeArray1[index1]) ? 0 : (!this.m_Destroyed.HasComponent(nativeArray1[index1]) ? 1 : 0)) != 0 && nativeArray3[index1].m_Condition <= -num2 && !this.m_SignatureDatas.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LevelDownQueue.Enqueue(nativeArray1[index1]);
            buildingCondition.m_Condition += levelingCost;
          }
          nativeArray3[index1] = buildingCondition;
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
    private struct UpkeepPaymentJob : IJob
    {
      public BufferLookup<Game.Economy.Resources> m_Resources;
      public NativeQueue<BuildingUpkeepSystem.UpkeepPayment> m_UpkeepExpenseQueue;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        BuildingUpkeepSystem.UpkeepPayment upkeepPayment;
        // ISSUE: reference to a compiler-generated field
        while (this.m_UpkeepExpenseQueue.TryDequeue(out upkeepPayment))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Resources.HasBuffer(upkeepPayment.m_RenterEntity))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            EconomyUtils.AddResources(Resource.Money, upkeepPayment.m_Price, this.m_Resources[upkeepPayment.m_RenterEntity]);
          }
        }
      }
    }

    [BurstCompile]
    private struct LeveldownJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildings;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDatas;
      public ComponentLookup<Building> m_Buildings;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> m_ElectricityConsumers;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> m_WaterConsumers;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> m_GarbageProducers;
      [ReadOnly]
      public ComponentLookup<MailProducer> m_MailProducers;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyDatas;
      [ReadOnly]
      public ComponentLookup<OfficeBuilding> m_OfficeBuilding;
      public NativeQueue<TriggerAction> m_TriggerBuffer;
      public ComponentLookup<CrimeProducer> m_CrimeProducers;
      public BufferLookup<Renter> m_Renters;
      [ReadOnly]
      public BuildingConfigurationData m_BuildingConfigurationData;
      public NativeQueue<Entity> m_LeveldownQueue;
      public EntityCommandBuffer m_CommandBuffer;
      public NativeQueue<Entity> m_UpdatedElectricityRoadEdges;
      public NativeQueue<Entity> m_UpdatedWaterPipeRoadEdges;
      public IconCommandBuffer m_IconCommandBuffer;
      public uint m_SimulationFrame;

      public void Execute()
      {
        Entity entity;
        // ISSUE: reference to a compiler-generated field
        while (this.m_LeveldownQueue.TryDequeue(out entity))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_Prefabs.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            Entity prefab = this.m_Prefabs[entity].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SpawnableBuildings.HasComponent(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              SpawnableBuildingData spawnableBuilding = this.m_SpawnableBuildings[prefab];
              // ISSUE: reference to a compiler-generated field
              Game.Prefabs.BuildingData buildingData = this.m_BuildingDatas[prefab];
              // ISSUE: reference to a compiler-generated field
              BuildingPropertyData buildingPropertyData = this.m_BuildingPropertyDatas[prefab];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Abandoned>(entity, new Abandoned()
              {
                m_AbandonmentTime = this.m_SimulationFrame
              });
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
              // ISSUE: reference to a compiler-generated field
              if (this.m_ElectricityConsumers.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<ElectricityConsumer>(entity);
                // ISSUE: reference to a compiler-generated field
                Entity roadEdge = this.m_Buildings[entity].m_RoadEdge;
                if (roadEdge != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_UpdatedElectricityRoadEdges.Enqueue(roadEdge);
                }
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_WaterConsumers.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<WaterConsumer>(entity);
                // ISSUE: reference to a compiler-generated field
                Entity roadEdge = this.m_Buildings[entity].m_RoadEdge;
                if (roadEdge != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_UpdatedWaterPipeRoadEdges.Enqueue(roadEdge);
                }
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_GarbageProducers.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<GarbageProducer>(entity);
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_MailProducers.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<MailProducer>(entity);
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_CrimeProducers.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                CrimeProducer crimeProducer = this.m_CrimeProducers[entity];
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<CrimeProducer>(entity, new CrimeProducer()
                {
                  m_Crime = crimeProducer.m_Crime * 2f,
                  m_PatrolRequest = crimeProducer.m_PatrolRequest
                });
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_Renters.HasBuffer(entity))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<Renter> renter = this.m_Renters[entity];
                for (int index = renter.Length - 1; index >= 0; --index)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<PropertyRenter>(renter[index].m_Renter);
                  renter.RemoveAt(index);
                }
              }
              // ISSUE: reference to a compiler-generated field
              if ((this.m_Buildings[entity].m_Flags & Game.Buildings.BuildingFlags.HighRentWarning) != Game.Buildings.BuildingFlags.None)
              {
                // ISSUE: reference to a compiler-generated field
                Building building = this.m_Buildings[entity];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Remove(entity, this.m_BuildingConfigurationData.m_HighRentNotification);
                building.m_Flags &= ~Game.Buildings.BuildingFlags.HighRentWarning;
                // ISSUE: reference to a compiler-generated field
                this.m_Buildings[entity] = building;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Remove(entity, IconPriority.Problem);
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Remove(entity, IconPriority.FatalProblem);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Add(entity, this.m_BuildingConfigurationData.m_AbandonedNotification, IconPriority.FatalProblem);
              if (buildingPropertyData.CountProperties(AreaType.Commercial) > 0)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.LevelDownCommercialBuilding, Entity.Null, entity, entity));
              }
              if (buildingPropertyData.CountProperties(AreaType.Industrial) > 0)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_OfficeBuilding.HasComponent(prefab))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.LevelDownOfficeBuilding, Entity.Null, entity, entity));
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.LevelDownIndustrialBuilding, Entity.Null, entity, entity));
                }
              }
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct LevelupJob : IJob
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<SpawnableBuildingData> m_SpawnableBuildingType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Prefabs.BuildingData> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<BuildingPropertyData> m_BuildingPropertyType;
      [ReadOnly]
      public ComponentTypeHandle<ObjectGeometryData> m_ObjectGeometryType;
      [ReadOnly]
      public SharedComponentTypeHandle<BuildingSpawnGroupData> m_BuildingSpawnGroupType;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> m_BlockData;
      [ReadOnly]
      public ComponentLookup<ValidArea> m_ValidAreaData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<PrefabData> m_PrefabDatas;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildings;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_Buildings;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyDatas;
      [ReadOnly]
      public ComponentLookup<OfficeBuilding> m_OfficeBuilding;
      [ReadOnly]
      public ComponentLookup<ZoneData> m_ZoneData;
      [ReadOnly]
      public BufferLookup<Cell> m_Cells;
      [ReadOnly]
      public BuildingConfigurationData m_BuildingConfigurationData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_SpawnableBuildingChunks;
      [ReadOnly]
      public NativeQuadTree<Entity, Bounds2> m_ZoneSearchTree;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      public IconCommandBuffer m_IconCommandBuffer;
      public NativeQueue<Entity> m_LevelupQueue;
      public EntityCommandBuffer m_CommandBuffer;
      public NativeQueue<TriggerAction> m_TriggerBuffer;
      public NativeQueue<ZoneBuiltLevelUpdate> m_ZoneBuiltLevelQueue;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(0);
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        while (this.m_LevelupQueue.TryDequeue(out entity1))
        {
          // ISSUE: reference to a compiler-generated field
          Entity prefab = this.m_Prefabs[entity1].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SpawnableBuildings.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            SpawnableBuildingData spawnableBuilding = this.m_SpawnableBuildings[prefab];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabDatas.IsComponentEnabled(spawnableBuilding.m_ZonePrefab))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Prefabs.BuildingData building = this.m_Buildings[prefab];
              // ISSUE: reference to a compiler-generated field
              BuildingPropertyData buildingPropertyData = this.m_BuildingPropertyDatas[prefab];
              // ISSUE: reference to a compiler-generated field
              ZoneData zoneData = this.m_ZoneData[spawnableBuilding.m_ZonePrefab];
              // ISSUE: reference to a compiler-generated method
              float maxHeight = this.GetMaxHeight(entity1, building);
              // ISSUE: reference to a compiler-generated method
              Entity entity2 = this.SelectSpawnableBuilding(zoneData.m_ZoneType, (int) spawnableBuilding.m_Level + 1, building.m_LotSize, maxHeight, building.m_Flags & (Game.Prefabs.BuildingFlags.LeftAccess | Game.Prefabs.BuildingFlags.RightAccess), buildingPropertyData, ref random);
              if (entity2 != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<UnderConstruction>(entity1, new UnderConstruction()
                {
                  m_NewPrefab = entity2,
                  m_Progress = byte.MaxValue
                });
                if (buildingPropertyData.CountProperties(AreaType.Residential) > 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.LevelUpResidentialBuilding, Entity.Null, entity1, entity1));
                }
                if (buildingPropertyData.CountProperties(AreaType.Commercial) > 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.LevelUpCommercialBuilding, Entity.Null, entity1, entity1));
                }
                if (buildingPropertyData.CountProperties(AreaType.Industrial) > 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_OfficeBuilding.HasComponent(prefab))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.LevelUpOfficeBuilding, Entity.Null, entity1, entity1));
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.LevelUpIndustrialBuilding, Entity.Null, entity1, entity1));
                  }
                }
                // ISSUE: reference to a compiler-generated field
                this.m_ZoneBuiltLevelQueue.Enqueue(new ZoneBuiltLevelUpdate()
                {
                  m_Zone = spawnableBuilding.m_ZonePrefab,
                  m_FromLevel = (int) spawnableBuilding.m_Level,
                  m_ToLevel = (int) spawnableBuilding.m_Level + 1,
                  m_Squares = building.m_LotSize.x * building.m_LotSize.y
                });
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Add(entity1, this.m_BuildingConfigurationData.m_LevelUpNotification, clusterLayer: IconClusterLayer.Transaction);
              }
            }
          }
        }
      }

      private Entity SelectSpawnableBuilding(
        ZoneType zoneType,
        int level,
        int2 lotSize,
        float maxHeight,
        Game.Prefabs.BuildingFlags accessFlags,
        BuildingPropertyData buildingPropertyData,
        ref Unity.Mathematics.Random random)
      {
        int max = 0;
        Entity entity = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_SpawnableBuildingChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk spawnableBuildingChunk = this.m_SpawnableBuildingChunks[index1];
          // ISSUE: reference to a compiler-generated field
          if (spawnableBuildingChunk.GetSharedComponent<BuildingSpawnGroupData>(this.m_BuildingSpawnGroupType).m_ZoneType.Equals(zoneType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray1 = spawnableBuildingChunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<SpawnableBuildingData> nativeArray2 = spawnableBuildingChunk.GetNativeArray<SpawnableBuildingData>(ref this.m_SpawnableBuildingType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Prefabs.BuildingData> nativeArray3 = spawnableBuildingChunk.GetNativeArray<Game.Prefabs.BuildingData>(ref this.m_BuildingType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<BuildingPropertyData> nativeArray4 = spawnableBuildingChunk.GetNativeArray<BuildingPropertyData>(ref this.m_BuildingPropertyType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<ObjectGeometryData> nativeArray5 = spawnableBuildingChunk.GetNativeArray<ObjectGeometryData>(ref this.m_ObjectGeometryType);
            for (int index2 = 0; index2 < spawnableBuildingChunk.Count; ++index2)
            {
              SpawnableBuildingData spawnableBuildingData = nativeArray2[index2];
              Game.Prefabs.BuildingData buildingData = nativeArray3[index2];
              BuildingPropertyData buildingPropertyData1 = nativeArray4[index2];
              ObjectGeometryData objectGeometryData = nativeArray5[index2];
              if (level == (int) spawnableBuildingData.m_Level && lotSize.Equals(buildingData.m_LotSize) && (double) objectGeometryData.m_Size.y <= (double) maxHeight && (buildingData.m_Flags & (Game.Prefabs.BuildingFlags.LeftAccess | Game.Prefabs.BuildingFlags.RightAccess)) == accessFlags && buildingPropertyData.m_ResidentialProperties <= buildingPropertyData1.m_ResidentialProperties && buildingPropertyData.m_AllowedManufactured == buildingPropertyData1.m_AllowedManufactured && buildingPropertyData.m_AllowedSold == buildingPropertyData1.m_AllowedSold && buildingPropertyData.m_AllowedStored == buildingPropertyData1.m_AllowedStored)
              {
                int num = 100;
                max += num;
                if (random.NextInt(max) < num)
                  entity = nativeArray1[index2];
              }
            }
          }
        }
        return entity;
      }

      private float GetMaxHeight(Entity building, Game.Prefabs.BuildingData prefabBuildingData)
      {
        // ISSUE: reference to a compiler-generated field
        Game.Objects.Transform transform = this.m_TransformData[building];
        float2 xz1 = math.rotate(transform.m_Rotation, new float3(8f, 0.0f, 0.0f)).xz;
        float2 xz2 = math.rotate(transform.m_Rotation, new float3(0.0f, 0.0f, 8f)).xz;
        float2 x1 = xz1 * (float) ((double) prefabBuildingData.m_LotSize.x * 0.5 - 0.5);
        float2 x2 = xz2 * (float) ((double) prefabBuildingData.m_LotSize.y * 0.5 - 0.5);
        float2 float2 = math.abs(x2) + math.abs(x1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        BuildingUpkeepSystem.LevelupJob.Iterator iterator = new BuildingUpkeepSystem.LevelupJob.Iterator()
        {
          m_Bounds = new Bounds2(transform.m_Position.xz - float2, transform.m_Position.xz + float2),
          m_LotSize = prefabBuildingData.m_LotSize,
          m_StartPosition = transform.m_Position.xz + x2 + x1,
          m_Right = xz1,
          m_Forward = xz2,
          m_MaxHeight = int.MaxValue,
          m_BlockData = this.m_BlockData,
          m_ValidAreaData = this.m_ValidAreaData,
          m_Cells = this.m_Cells
        };
        // ISSUE: reference to a compiler-generated field
        this.m_ZoneSearchTree.Iterate<BuildingUpkeepSystem.LevelupJob.Iterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        return (float) iterator.m_MaxHeight - transform.m_Position.y;
      }

      private struct Iterator : 
        INativeQuadTreeIterator<Entity, Bounds2>,
        IUnsafeQuadTreeIterator<Entity, Bounds2>
      {
        public Bounds2 m_Bounds;
        public int2 m_LotSize;
        public float2 m_StartPosition;
        public float2 m_Right;
        public float2 m_Forward;
        public int m_MaxHeight;
        public ComponentLookup<Game.Zones.Block> m_BlockData;
        public ComponentLookup<ValidArea> m_ValidAreaData;
        public BufferLookup<Cell> m_Cells;

        public bool Intersect(Bounds2 bounds) => MathUtils.Intersect(bounds, this.m_Bounds);

        public void Iterate(Bounds2 bounds, Entity blockEntity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds, this.m_Bounds))
            return;
          // ISSUE: reference to a compiler-generated field
          ValidArea validArea = this.m_ValidAreaData[blockEntity];
          if (validArea.m_Area.y <= validArea.m_Area.x)
            return;
          // ISSUE: reference to a compiler-generated field
          Game.Zones.Block block = this.m_BlockData[blockEntity];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Cell> cell1 = this.m_Cells[blockEntity];
          // ISSUE: reference to a compiler-generated field
          float2 startPosition = this.m_StartPosition;
          int2 int2;
          // ISSUE: reference to a compiler-generated field
          for (int2.y = 0; int2.y < this.m_LotSize.y; ++int2.y)
          {
            float2 position = startPosition;
            // ISSUE: reference to a compiler-generated field
            for (int2.x = 0; int2.x < this.m_LotSize.x; ++int2.x)
            {
              int2 cellIndex = ZoneUtils.GetCellIndex(block, position);
              if (math.all(cellIndex >= validArea.m_Area.xz & cellIndex < validArea.m_Area.yw))
              {
                int index = cellIndex.y * block.m_Size.x + cellIndex.x;
                Cell cell2 = cell1[index];
                if ((cell2.m_State & CellFlags.Visible) != CellFlags.None)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_MaxHeight = math.min(this.m_MaxHeight, (int) cell2.m_Height);
                }
              }
              // ISSUE: reference to a compiler-generated field
              position -= this.m_Right;
            }
            // ISSUE: reference to a compiler-generated field
            startPosition -= this.m_Forward;
          }
        }
      }
    }

    private struct TypeHandle
    {
      public ComponentTypeHandle<BuildingCondition> __Game_Buildings_BuildingCondition_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Renter> __Game_Buildings_Renter_RO_BufferTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ConsumptionData> __Game_Prefabs_ConsumptionData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ResourceAvailability> __Game_Net_ResourceAvailability_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<SignatureBuildingData> __Game_Prefabs_SignatureBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Abandoned> __Game_Buildings_Abandoned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> __Game_Vehicles_DeliveryTruck_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<BuildingSpawnGroupData> __Game_Prefabs_BuildingSpawnGroupData_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> __Game_Zones_Block_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ValidArea> __Game_Zones_ValidArea_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OfficeBuilding> __Game_Prefabs_OfficeBuilding_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Cell> __Game_Zones_Cell_RO_BufferLookup;
      public ComponentLookup<Building> __Game_Buildings_Building_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> __Game_Buildings_GarbageProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MailProducer> __Game_Buildings_MailProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> __Game_Buildings_WaterConsumer_RO_ComponentLookup;
      public ComponentLookup<CrimeProducer> __Game_Buildings_CrimeProducer_RW_ComponentLookup;
      public BufferLookup<Renter> __Game_Buildings_Renter_RW_BufferLookup;
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_BuildingCondition_RW_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingCondition>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferTypeHandle = state.GetBufferTypeHandle<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ConsumptionData_RO_ComponentLookup = state.GetComponentLookup<ConsumptionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ResourceAvailability_RO_BufferLookup = state.GetBufferLookup<ResourceAvailability>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup = state.GetComponentLookup<SignatureBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Abandoned_RO_ComponentLookup = state.GetComponentLookup<Abandoned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentLookup = state.GetComponentLookup<ZoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RO_BufferLookup = state.GetBufferLookup<OwnedVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.DeliveryTruck>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingSpawnGroupData_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<BuildingSpawnGroupData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Block_RO_ComponentLookup = state.GetComponentLookup<Game.Zones.Block>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_ValidArea_RO_ComponentLookup = state.GetComponentLookup<ValidArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentLookup = state.GetComponentLookup<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup = state.GetComponentLookup<OfficeBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Cell_RO_BufferLookup = state.GetBufferLookup<Cell>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RW_ComponentLookup = state.GetComponentLookup<Building>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup = state.GetComponentLookup<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageProducer_RO_ComponentLookup = state.GetComponentLookup<GarbageProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_MailProducer_RO_ComponentLookup = state.GetComponentLookup<MailProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RO_ComponentLookup = state.GetComponentLookup<WaterConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CrimeProducer_RW_ComponentLookup = state.GetComponentLookup<CrimeProducer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RW_BufferLookup = state.GetBufferLookup<Renter>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>();
      }
    }
  }
}
