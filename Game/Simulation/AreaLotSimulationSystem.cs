// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AreaLotSimulationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Areas;
using Game.City;
using Game.Common;
using Game.Economy;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
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
  public class AreaLotSimulationSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 512;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private NaturalResourceSystem m_NaturalResourceSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private CitySystem m_CitySystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_Watersystem;
    private GroundWaterSystem m_GroundWaterSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_AreaQuery;
    private EntityQuery m_ExtractorQuery;
    private EntityQuery m_VehiclePrefabQuery;
    private EntityQuery m_ExtractorParameterQuery;
    private WorkVehicleSelectData m_WorkVehicleSelectData;
    private AreaLotSimulationSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 512;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NaturalResourceSystem = this.World.GetOrCreateSystemManaged<NaturalResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_Watersystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundWaterSystem = this.World.GetOrCreateSystemManaged<GroundWaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_WorkVehicleSelectData = new WorkVehicleSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_AreaQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Lot>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadWrite<Extractor>(),
          ComponentType.ReadWrite<Storage>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ExtractorQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Lot>(),
          ComponentType.ReadWrite<Extractor>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_VehiclePrefabQuery = this.GetEntityQuery(WorkVehicleSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.m_ExtractorParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<ExtractorParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_AreaQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_WorkVehicleSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_VehiclePrefabQuery, Allocator.TempJob, out jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WorkVehicle_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StorageAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Storage_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Extractor_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_WoodResource_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      JobHandle jobHandle2 = new AreaLotSimulationSystem.ManageVehiclesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_WoodResourceType = this.__TypeHandle.__Game_Areas_WoodResource_RO_BufferTypeHandle,
        m_PathElementType = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferTypeHandle,
        m_ExtractorType = this.__TypeHandle.__Game_Areas_Extractor_RW_ComponentTypeHandle,
        m_StorageType = this.__TypeHandle.__Game_Areas_Storage_RW_ComponentTypeHandle,
        m_OwnedVehicleType = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RW_BufferTypeHandle,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabExtractorAreaData = this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup,
        m_PrefabStorageAreaData = this.__TypeHandle.__Game_Prefabs_StorageAreaData_RO_ComponentLookup,
        m_PrefabWorkVehicleData = this.__TypeHandle.__Game_Prefabs_WorkVehicleData_RO_ComponentLookup,
        m_VehicleLayouts = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_WorkVehicleData = this.__TypeHandle.__Game_Vehicles_WorkVehicle_RW_ComponentLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_WorkVehicleSelectData = this.m_WorkVehicleSelectData,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 512).AsParallelWriter()
      }.ScheduleParallel<AreaLotSimulationSystem.ManageVehiclesJob>(this.m_AreaQuery, JobHandle.CombineDependencies(this.Dependency, jobHandle1));
      // ISSUE: reference to a compiler-generated field
      this.m_WorkVehicleSelectData.PostUpdate(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(jobHandle2);
      this.Dependency = jobHandle2;
      // ISSUE: reference to a compiler-generated field
      if (this.m_ExtractorQuery.IsEmptyIgnoreFilter)
        return;
      NativeList<Entity> nativeList = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      CellMapData<NaturalResourceCell> data = this.m_NaturalResourceSystem.GetData(false, out dependencies1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Extractor_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_MapFeatureElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AreaLotSimulationSystem.ExtractResourcesJob jobData1 = new AreaLotSimulationSystem.ExtractResourcesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle,
        m_TriangleType = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle,
        m_MapFeatureElements = this.__TypeHandle.__Game_Areas_MapFeatureElement_RO_BufferLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_ExtractorAreaData = this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup,
        m_ExtractorData = this.__TypeHandle.__Game_Areas_Extractor_RW_ComponentLookup,
        m_Chunks = this.m_ExtractorQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_AreaTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies2),
        m_City = this.m_CitySystem.City,
        m_RandomSeed = RandomSeed.Next(),
        m_ExtractorParameters = this.m_ExtractorParameterQuery.GetSingleton<ExtractorParameterData>(),
        m_NaturalResourceData = data,
        m_UpdateList = nativeList
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_MapFeatureElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_WoodResource_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Extractor_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TreeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Plant_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies3;
      JobHandle dependencies4;
      JobHandle deps;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AreaResourceSystem.UpdateAreaResourcesJob jobData2 = new AreaResourceSystem.UpdateAreaResourcesJob()
      {
        m_City = this.m_CitySystem.City,
        m_FullUpdate = false,
        m_UpdateList = nativeList.AsDeferredJobArray(),
        m_ObjectTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies3),
        m_NaturalResourceData = data,
        m_GroundWaterResourceData = this.m_GroundWaterSystem.GetData(true, out dependencies4),
        m_GeometryData = this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup,
        m_TreeData = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup,
        m_PlantData = this.__TypeHandle.__Game_Objects_Plant_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_DamagedData = this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ExtractorAreaData = this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup,
        m_PrefabTreeData = this.__TypeHandle.__Game_Prefabs_TreeData_RO_ComponentLookup,
        m_Nodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_Triangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_ExtractorData = this.__TypeHandle.__Game_Areas_Extractor_RW_ComponentLookup,
        m_WoodResources = this.__TypeHandle.__Game_Areas_WoodResource_RW_BufferLookup,
        m_MapFeatureElements = this.__TypeHandle.__Game_Areas_MapFeatureElement_RW_BufferLookup,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_Watersystem.GetSurfaceData(out deps)
      };
      JobHandle jobHandle3 = jobData1.Schedule<AreaLotSimulationSystem.ExtractResourcesJob>(JobHandle.CombineDependencies(this.Dependency, JobHandle.CombineDependencies(outJobHandle, dependencies2, dependencies1)));
      NativeList<Entity> list = nativeList;
      JobHandle dependsOn = JobUtils.CombineDependencies(jobHandle3, dependencies3, deps, dependencies4);
      JobHandle jobHandle4 = jobData2.Schedule<AreaResourceSystem.UpdateAreaResourcesJob, Entity>(list, 1, dependsOn);
      nativeList.Dispose(jobHandle4);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle4);
      // ISSUE: reference to a compiler-generated field
      this.m_NaturalResourceSystem.AddWriter(jobHandle4);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle4);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_Watersystem.AddSurfaceReader(jobHandle4);
      // ISSUE: reference to a compiler-generated field
      this.m_GroundWaterSystem.AddReader(jobHandle4);
      this.Dependency = jobHandle4;
    }

    public static int GetUnlimitedTotalAmount(int used, int originalAmount, float mu)
    {
      return Mathf.RoundToInt(math.log((float) (originalAmount / 10000)) - math.log((float) ((originalAmount - used) / 10000)) / mu);
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
    public AreaLotSimulationSystem()
    {
    }

    [BurstCompile]
    private struct ManageVehiclesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<WoodResource> m_WoodResourceType;
      [ReadOnly]
      public BufferTypeHandle<PathElement> m_PathElementType;
      public ComponentTypeHandle<Extractor> m_ExtractorType;
      public ComponentTypeHandle<Storage> m_StorageType;
      public BufferTypeHandle<OwnedVehicle> m_OwnedVehicleType;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Attachment> m_AttachmentData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ExtractorAreaData> m_PrefabExtractorAreaData;
      [ReadOnly]
      public ComponentLookup<StorageAreaData> m_PrefabStorageAreaData;
      [ReadOnly]
      public ComponentLookup<WorkVehicleData> m_PrefabWorkVehicleData;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_VehicleLayouts;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Vehicles.WorkVehicle> m_WorkVehicleData;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public WorkVehicleSelectData m_WorkVehicleSelectData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Extractor> nativeArray2 = chunk.GetNativeArray<Extractor>(ref this.m_ExtractorType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Storage> nativeArray3 = chunk.GetNativeArray<Storage>(ref this.m_StorageType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathInformation> nativeArray4 = chunk.GetNativeArray<PathInformation>(ref this.m_PathInformationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        if (nativeArray4.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<PathElement> bufferAccessor = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
          for (int index = 0; index < nativeArray4.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            PathInformation pathInformation = nativeArray4[index];
            DynamicBuffer<PathElement> path = bufferAccessor[index];
            PrefabRef prefabRef = nativeArray5[index];
            if (nativeArray2.Length != 0)
            {
              Extractor extractor = nativeArray2[index];
              // ISSUE: reference to a compiler-generated field
              ExtractorAreaData extractorAreaData = this.m_PrefabExtractorAreaData[prefabRef.m_Prefab];
              switch (extractor.m_WorkType)
              {
                case VehicleWorkType.Harvest:
                  // ISSUE: reference to a compiler-generated method
                  this.TrySpawnVehicle(unfilteredChunkIndex, ref random, entity, pathInformation, path, extractor.m_WorkType, extractorAreaData.m_MapFeature, Resource.NoResource, WorkVehicleFlags.ExtractorVehicle, ref extractor.m_WorkAmount);
                  break;
                case VehicleWorkType.Collect:
                  // ISSUE: reference to a compiler-generated method
                  this.TrySpawnVehicle(unfilteredChunkIndex, ref random, entity, pathInformation, path, extractor.m_WorkType, extractorAreaData.m_MapFeature, Resource.NoResource, WorkVehicleFlags.ExtractorVehicle, ref extractor.m_HarvestedAmount);
                  break;
              }
              nativeArray2[index] = extractor;
            }
            if (nativeArray3.Length != 0)
            {
              Storage storage = nativeArray3[index];
              // ISSUE: reference to a compiler-generated field
              StorageAreaData storageAreaData = this.m_PrefabStorageAreaData[prefabRef.m_Prefab];
              // ISSUE: reference to a compiler-generated method
              this.TrySpawnVehicle(unfilteredChunkIndex, ref random, entity, pathInformation, path, VehicleWorkType.Collect, MapFeature.None, storageAreaData.m_Resources, WorkVehicleFlags.StorageVehicle, ref storage.m_WorkAmount);
              nativeArray3[index] = storage;
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<PathInformation>(unfilteredChunkIndex, entity);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<PathElement>(unfilteredChunkIndex, entity);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray6 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<WoodResource> bufferAccessor1 = chunk.GetBufferAccessor<WoodResource>(ref this.m_WoodResourceType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<OwnedVehicle> bufferAccessor2 = chunk.GetBufferAccessor<OwnedVehicle>(ref this.m_OwnedVehicleType);
          for (int index1 = 0; index1 < bufferAccessor2.Length; ++index1)
          {
            Entity entity = nativeArray1[index1];
            PrefabRef prefabRef = nativeArray5[index1];
            DynamicBuffer<OwnedVehicle> dynamicBuffer = bufferAccessor2[index1];
            Owner owner;
            CollectionUtils.TryGet<Owner>(nativeArray6, index1, out owner);
            Extractor extractor;
            CollectionUtils.TryGet<Extractor>(nativeArray2, index1, out extractor);
            Storage storage;
            CollectionUtils.TryGet<Storage>(nativeArray3, index1, out storage);
            DynamicBuffer<WoodResource> woodResources;
            CollectionUtils.TryGet<WoodResource>(bufferAccessor1, index1, out woodResources);
            float pendingWorkAmount1 = 0.0f;
            float pendingWorkAmount2 = 0.0f;
            float pendingWorkAmount3 = 0.0f;
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              Entity vehicle1 = dynamicBuffer[index2].m_Vehicle;
              Game.Vehicles.WorkVehicle componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_WorkVehicleData.TryGetComponent(vehicle1, out componentData))
              {
                DynamicBuffer<LayoutElement> bufferData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_VehicleLayouts.TryGetBuffer(vehicle1, out bufferData) && bufferData.Length != 0)
                {
                  for (int index3 = 0; index3 < bufferData.Length; ++index3)
                  {
                    Entity vehicle2 = bufferData[index3].m_Vehicle;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    WorkVehicleData workVehicleData = this.m_PrefabWorkVehicleData[this.m_PrefabRefData[vehicle2].m_Prefab];
                    if ((componentData.m_State & WorkVehicleFlags.ExtractorVehicle) != (WorkVehicleFlags) 0)
                    {
                      switch (workVehicleData.m_WorkType)
                      {
                        case VehicleWorkType.Harvest:
                          // ISSUE: reference to a compiler-generated method
                          this.CheckVehicle(vehicle2, workVehicleData, ref extractor.m_WorkAmount, ref pendingWorkAmount1);
                          continue;
                        case VehicleWorkType.Collect:
                          // ISSUE: reference to a compiler-generated method
                          this.CheckVehicle(vehicle2, workVehicleData, ref extractor.m_HarvestedAmount, ref pendingWorkAmount2);
                          continue;
                        default:
                          continue;
                      }
                    }
                    else if ((componentData.m_State & WorkVehicleFlags.StorageVehicle) != (WorkVehicleFlags) 0)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.CheckVehicle(vehicle2, workVehicleData, ref storage.m_WorkAmount, ref pendingWorkAmount3);
                    }
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  WorkVehicleData workVehicleData = this.m_PrefabWorkVehicleData[this.m_PrefabRefData[vehicle1].m_Prefab];
                  if ((componentData.m_State & WorkVehicleFlags.ExtractorVehicle) != (WorkVehicleFlags) 0)
                  {
                    switch (workVehicleData.m_WorkType)
                    {
                      case VehicleWorkType.Harvest:
                        // ISSUE: reference to a compiler-generated method
                        this.CheckVehicle(vehicle1, workVehicleData, ref extractor.m_WorkAmount, ref pendingWorkAmount1);
                        continue;
                      case VehicleWorkType.Collect:
                        // ISSUE: reference to a compiler-generated method
                        this.CheckVehicle(vehicle1, workVehicleData, ref extractor.m_HarvestedAmount, ref pendingWorkAmount2);
                        continue;
                      default:
                        continue;
                    }
                  }
                  else if ((componentData.m_State & WorkVehicleFlags.StorageVehicle) != (WorkVehicleFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.CheckVehicle(vehicle1, workVehicleData, ref storage.m_WorkAmount, ref pendingWorkAmount3);
                  }
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.m_PrefabRefData.HasComponent(vehicle1))
                  dynamicBuffer.RemoveAtSwapBack(index2--);
              }
            }
            if (nativeArray2.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              ExtractorAreaData extractorAreaData = this.m_PrefabExtractorAreaData[prefabRef.m_Prefab];
              if (extractorAreaData.m_MapFeature == MapFeature.Forest)
                extractor.m_ExtractedAmount = extractor.m_WorkAmount + pendingWorkAmount1;
              if ((double) extractor.m_WorkAmount >= 1000.0)
              {
                // ISSUE: reference to a compiler-generated method
                this.FindTarget(unfilteredChunkIndex, entity, owner, extractorAreaData.m_MapFeature, VehicleWorkType.Harvest, woodResources, ref extractor.m_WorkAmount);
                extractor.m_WorkType = VehicleWorkType.Harvest;
              }
              else if ((double) extractor.m_HarvestedAmount >= 1000.0)
              {
                // ISSUE: reference to a compiler-generated method
                this.FindTarget(unfilteredChunkIndex, entity, owner, extractorAreaData.m_MapFeature, VehicleWorkType.Collect, woodResources, ref extractor.m_HarvestedAmount);
                extractor.m_WorkType = VehicleWorkType.Collect;
              }
              nativeArray2[index1] = extractor;
            }
            if (nativeArray3.Length != 0)
            {
              if ((double) storage.m_WorkAmount >= 1000.0)
              {
                // ISSUE: reference to a compiler-generated method
                this.FindTarget(unfilteredChunkIndex, entity, owner, MapFeature.None, VehicleWorkType.Collect, woodResources, ref storage.m_WorkAmount);
              }
              nativeArray3[index1] = storage;
            }
          }
        }
      }

      private void CheckVehicle(
        Entity vehicle,
        WorkVehicleData workVehicleData,
        ref float lotWorkAmount,
        ref float pendingWorkAmount)
      {
        // ISSUE: reference to a compiler-generated field
        Game.Vehicles.WorkVehicle workVehicle = this.m_WorkVehicleData[vehicle];
        if ((double) lotWorkAmount >= 1.0)
        {
          float num = (workVehicle.m_State & WorkVehicleFlags.Returning) != (WorkVehicleFlags) 0 ? math.min(lotWorkAmount, workVehicle.m_DoneAmount - workVehicle.m_WorkAmount) : math.min(lotWorkAmount, workVehicleData.m_MaxWorkAmount - workVehicle.m_WorkAmount);
          if ((double) num > 0.0)
          {
            workVehicle.m_WorkAmount += num;
            lotWorkAmount -= num;
            // ISSUE: reference to a compiler-generated field
            this.m_WorkVehicleData[vehicle] = workVehicle;
          }
        }
        pendingWorkAmount += workVehicle.m_WorkAmount - workVehicle.m_DoneAmount;
      }

      private void FindTarget(
        int jobIndex,
        Entity entity,
        Owner owner,
        MapFeature mapFeature,
        VehicleWorkType workType,
        DynamicBuffer<WoodResource> woodResources,
        ref float extractorWorkAmount)
      {
        Entity source = Entity.Null;
        Attachment componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_AttachmentData.TryGetComponent(owner.m_Owner, out componentData))
        {
          source = componentData.m_Attached;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransformData.HasComponent(owner.m_Owner))
            source = owner.m_Owner;
        }
        if (source != Entity.Null)
        {
          if (mapFeature == MapFeature.Forest)
          {
            if (woodResources.IsCreated && woodResources.Length != 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.FindTarget(jobIndex, entity, source, SetupTargetType.WoodResource, workType);
            }
            else
              extractorWorkAmount = 0.0f;
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.FindTarget(jobIndex, entity, source, SetupTargetType.AreaLocation, workType);
          }
        }
        else
          extractorWorkAmount = 0.0f;
      }

      private void FindTarget(
        int jobIndex,
        Entity owner,
        Entity source,
        SetupTargetType targetType,
        VehicleWorkType workType)
      {
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) 277.777771f,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
          m_Methods = PathMethod.Road | PathMethod.Offroad
        };
        SetupQueueTarget origin = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Methods = PathMethod.Road | PathMethod.Offroad,
          m_RoadTypes = RoadTypes.Car,
          m_Entity = source
        };
        SetupQueueTarget destination = new SetupQueueTarget()
        {
          m_Type = targetType,
          m_Methods = PathMethod.Road | PathMethod.Offroad,
          m_RoadTypes = RoadTypes.Car,
          m_Entity = owner,
          m_Value = (int) workType
        };
        // ISSUE: reference to a compiler-generated field
        this.m_PathfindQueue.Enqueue(new SetupQueueItem(owner, parameters, origin, destination));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathInformation>(jobIndex, owner, new PathInformation());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddBuffer<PathElement>(jobIndex, owner);
      }

      private void TrySpawnVehicle(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        PathInformation pathInformation,
        DynamicBuffer<PathElement> path,
        VehicleWorkType workType,
        MapFeature mapFeature,
        Resource resource,
        WorkVehicleFlags flags,
        ref float lotWorkAmount)
      {
        if (pathInformation.m_Destination != Entity.Null)
        {
          float workAmount = lotWorkAmount;
          // ISSUE: reference to a compiler-generated method
          this.SpawnVehicle(jobIndex, ref random, owner, pathInformation, path, workType, mapFeature, resource, flags, ref workAmount);
          lotWorkAmount -= workAmount;
        }
        else
          lotWorkAmount = 0.0f;
      }

      private void SpawnVehicle(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity owner,
        PathInformation pathInformation,
        DynamicBuffer<PathElement> path,
        VehicleWorkType workType,
        MapFeature mapFeature,
        Resource resource,
        WorkVehicleFlags flags,
        ref float workAmount)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TransformData.HasComponent(pathInformation.m_Origin))
          return;
        // ISSUE: reference to a compiler-generated field
        Game.Objects.Transform transform = this.m_TransformData[pathInformation.m_Origin];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity vehicle = this.m_WorkVehicleSelectData.CreateVehicle(this.m_CommandBuffer, jobIndex, ref random, workType, mapFeature, resource, ref workAmount, transform, pathInformation.m_Origin, flags);
        if (!(vehicle != Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Game.Common.Target>(jobIndex, vehicle, new Game.Common.Target(pathInformation.m_Destination));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Owner>(jobIndex, vehicle, new Owner(owner));
        if (path.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> targetElements = this.m_CommandBuffer.SetBuffer<PathElement>(jobIndex, vehicle);
        PathUtils.CopyPath(path, new PathOwner(), 0, targetElements);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PathOwner>(jobIndex, vehicle, new PathOwner(PathFlags.Updated));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PathInformation>(jobIndex, vehicle, pathInformation);
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
    private struct ExtractResourcesJob : IJob
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> m_NodeType;
      [ReadOnly]
      public BufferTypeHandle<Triangle> m_TriangleType;
      [ReadOnly]
      public ComponentLookup<ExtractorAreaData> m_ExtractorAreaData;
      [ReadOnly]
      public BufferLookup<MapFeatureElement> m_MapFeatureElements;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      public ComponentLookup<Extractor> m_ExtractorData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaTree;
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public ExtractorParameterData m_ExtractorParameters;
      public CellMapData<NaturalResourceCell> m_NaturalResourceData;
      public NativeList<Entity> m_UpdateList;

      public void Execute()
      {
        NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AreaLotSimulationSystem.ExtractResourcesJob.AreaIterator iterator = new AreaLotSimulationSystem.ExtractResourcesJob.AreaIterator()
        {
          m_ExtractorData = this.m_ExtractorData,
          m_MapFeatureElements = this.m_MapFeatureElements,
          m_UpdateSet = nativeParallelHashSet,
          m_UpdateList = this.m_UpdateList
        };
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(0);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Game.Areas.Node> bufferAccessor1 = chunk.GetBufferAccessor<Game.Areas.Node>(ref this.m_NodeType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Triangle> bufferAccessor2 = chunk.GetBufferAccessor<Triangle>(ref this.m_TriangleType);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            // ISSUE: reference to a compiler-generated field
            ExtractorAreaData extractorAreaData = this.m_ExtractorAreaData[nativeArray2[index2].m_Prefab];
            if (extractorAreaData.m_MapFeature == MapFeature.Forest)
            {
              if (nativeParallelHashSet.Add(entity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_UpdateList.Add(in entity);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              Extractor extractor = this.m_ExtractorData[entity];
              if ((double) extractor.m_ExtractedAmount >= (double) math.max(1f, extractorAreaData.m_MapFeature == MapFeature.Ore || extractorAreaData.m_MapFeature == MapFeature.Oil ? 1f : extractor.m_ResourceAmount * (1f / 1000f)))
              {
                switch (extractorAreaData.m_MapFeature)
                {
                  case MapFeature.FertileLand:
                  case MapFeature.Oil:
                  case MapFeature.Ore:
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
                    // ISSUE: reference to a compiler-generated method
                    this.ExtractNaturalResources(ref random, ref iterator, bufferAccessor1[index2], bufferAccessor2[index2], cityModifier, ref extractor, extractorAreaData.m_MapFeature);
                    break;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_ExtractorData[entity] = extractor;
              }
            }
          }
        }
        nativeParallelHashSet.Dispose();
      }

      private int GetUnlimitedUsage(
        float originalConcentration,
        float currentConcentration,
        float mu,
        ref Unity.Mathematics.Random random,
        int extractedAmount)
      {
        float num = math.log(originalConcentration) - math.log(currentConcentration);
        return MathUtils.RoundToIntRandom(ref random, (float) ((double) mu * (double) originalConcentration * (double) math.exp(-num) * (double) extractedAmount * 10000.0));
      }

      private void ExtractNaturalResources(
        ref Unity.Mathematics.Random random,
        ref AreaLotSimulationSystem.ExtractResourcesJob.AreaIterator iterator,
        DynamicBuffer<Game.Areas.Node> nodes,
        DynamicBuffer<Triangle> triangles,
        DynamicBuffer<CityModifier> cityModifiers,
        ref Extractor extractor,
        MapFeature mapFeature)
      {
        // ISSUE: reference to a compiler-generated field
        float4 xyxy1 = (1f / this.m_NaturalResourceData.m_CellSize).xyxy;
        // ISSUE: reference to a compiler-generated field
        float4 xyxy2 = ((float2) this.m_NaturalResourceData.m_TextureSize * 0.5f).xyxy;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num1 = (float) (1.0 / ((double) this.m_NaturalResourceData.m_CellSize.x * (double) this.m_NaturalResourceData.m_CellSize.y));
        int y1 = Mathf.FloorToInt(extractor.m_ExtractedAmount);
        do
        {
          int index1 = -1;
          int extractedAmount = 0;
          float num2 = 0.0f;
          bool flag = mapFeature == MapFeature.Ore || mapFeature == MapFeature.Oil;
          Bounds2 bounds2_1 = new Bounds2();
          for (int index2 = 0; index2 < triangles.Length; ++index2)
          {
            Triangle2 triangle2 = AreaUtils.GetTriangle2(nodes, triangles[index2]);
            Bounds2 bounds2_2 = MathUtils.Bounds(triangle2);
            // ISSUE: reference to a compiler-generated field
            int4 int4 = math.clamp((int4) math.floor(new float4(bounds2_2.min, bounds2_2.max) * xyxy1 + xyxy2), (int4) 0, this.m_NaturalResourceData.m_TextureSize.xyxy - 1);
            float num3 = 0.0f;
            float num4 = 0.0f;
            Bounds2 bounds2_3 = new Bounds2();
            int num5 = 0;
            float f = 0.0f;
            for (int y2 = int4.y; y2 <= int4.w; ++y2)
            {
              Bounds2 bounds;
              // ISSUE: reference to a compiler-generated field
              bounds.min.y = ((float) y2 - xyxy2.y) * this.m_NaturalResourceData.m_CellSize.y;
              // ISSUE: reference to a compiler-generated field
              bounds.max.y = bounds.min.y + this.m_NaturalResourceData.m_CellSize.y;
              for (int x1 = int4.x; x1 <= int4.z; ++x1)
              {
                // ISSUE: reference to a compiler-generated field
                int index3 = x1 + this.m_NaturalResourceData.m_TextureSize.x * y2;
                // ISSUE: reference to a compiler-generated field
                NaturalResourceCell naturalResourceCell = this.m_NaturalResourceData.m_Buffer[index3];
                float x2;
                switch (mapFeature)
                {
                  case MapFeature.FertileLand:
                    x2 = (float) naturalResourceCell.m_Fertility.m_Base;
                    x2 -= (float) naturalResourceCell.m_Fertility.m_Used;
                    break;
                  case MapFeature.Oil:
                    x2 = (float) naturalResourceCell.m_Oil.m_Base;
                    CityUtils.ApplyModifier(ref x2, cityModifiers, CityModifierType.OilResourceAmount);
                    x2 -= (float) naturalResourceCell.m_Oil.m_Used;
                    break;
                  case MapFeature.Ore:
                    x2 = (float) naturalResourceCell.m_Ore.m_Base;
                    CityUtils.ApplyModifier(ref x2, cityModifiers, CityModifierType.OreResourceAmount);
                    x2 -= (float) naturalResourceCell.m_Ore.m_Used;
                    break;
                  default:
                    x2 = 0.0f;
                    break;
                }
                x2 = math.clamp(x2, 0.0f, (float) ushort.MaxValue);
                if ((double) x2 != 0.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  bounds.min.x = ((float) x1 - xyxy2.x) * this.m_NaturalResourceData.m_CellSize.x;
                  // ISSUE: reference to a compiler-generated field
                  bounds.max.x = bounds.min.x + this.m_NaturalResourceData.m_CellSize.x;
                  float area;
                  if (MathUtils.Intersect(bounds, triangle2, out area))
                  {
                    num3 += area * random.NextFloat(0.99f, 1.01f) * math.min(x2 * 0.0001f, 1f);
                    num4 += area;
                    if ((double) x2 * (double) area * (double) num1 > (double) f)
                    {
                      f = x2 * area * num1;
                      num5 = index3;
                      bounds2_3 = bounds;
                    }
                  }
                }
              }
            }
            float num6 = (double) num4 > 0.0099999997764825821 ? num3 / num4 : 0.0f;
            if ((double) num6 > (double) num2)
            {
              index1 = num5;
              extractedAmount = flag ? y1 : math.min(Mathf.RoundToInt(f), y1);
              num2 = num6;
              bounds2_1 = bounds2_3;
            }
          }
          if (extractedAmount > 0)
          {
            // ISSUE: reference to a compiler-generated field
            NaturalResourceCell naturalResourceCell = this.m_NaturalResourceData.m_Buffer[index1];
            switch (mapFeature)
            {
              case MapFeature.FertileLand:
                naturalResourceCell.m_Fertility.m_Used = (ushort) math.min((int) ushort.MaxValue, (int) naturalResourceCell.m_Fertility.m_Used + extractedAmount);
                break;
              case MapFeature.Oil:
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                int unlimitedUsage1 = this.GetUnlimitedUsage((float) naturalResourceCell.m_Oil.m_Base * 0.0001f, (float) ((int) naturalResourceCell.m_Oil.m_Base - (int) naturalResourceCell.m_Oil.m_Used) * 0.0001f, 1f / this.m_ExtractorParameters.m_OilConsumption, ref random, extractedAmount);
                naturalResourceCell.m_Oil.m_Used = (ushort) math.min((int) ushort.MaxValue, (int) naturalResourceCell.m_Oil.m_Used + unlimitedUsage1);
                break;
              case MapFeature.Ore:
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                int unlimitedUsage2 = this.GetUnlimitedUsage((float) naturalResourceCell.m_Ore.m_Base * 0.0001f, (float) ((int) naturalResourceCell.m_Ore.m_Base - (int) naturalResourceCell.m_Ore.m_Used) * 0.0001f, 1f / this.m_ExtractorParameters.m_OreConsumption, ref random, extractedAmount);
                naturalResourceCell.m_Ore.m_Used = (ushort) math.min((int) ushort.MaxValue, (int) naturalResourceCell.m_Ore.m_Used + unlimitedUsage2);
                break;
            }
            // ISSUE: reference to a compiler-generated field
            this.m_NaturalResourceData.m_Buffer[index1] = naturalResourceCell;
            extractor.m_ExtractedAmount -= (float) extractedAmount;
            // ISSUE: reference to a compiler-generated field
            iterator.m_Bounds = bounds2_1;
            // ISSUE: reference to a compiler-generated field
            this.m_AreaTree.Iterate<AreaLotSimulationSystem.ExtractResourcesJob.AreaIterator>(ref iterator);
            y1 = Mathf.FloorToInt(extractor.m_ExtractedAmount);
          }
          else
            goto label_24;
        }
        while (y1 > 0);
        goto label_5;
label_24:
        return;
label_5:;
      }

      private struct AreaIterator : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public ComponentLookup<Extractor> m_ExtractorData;
        public BufferLookup<MapFeatureElement> m_MapFeatureElements;
        public NativeParallelHashSet<Entity> m_UpdateSet;
        public NativeList<Entity> m_UpdateList;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem item)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || !this.m_ExtractorData.HasComponent(item.m_Area) && !this.m_MapFeatureElements.HasBuffer(item.m_Area) || !this.m_UpdateSet.Add(item.m_Area))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateList.Add(in item.m_Area);
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<WoodResource> __Game_Areas_WoodResource_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<PathElement> __Game_Pathfind_PathElement_RO_BufferTypeHandle;
      public ComponentTypeHandle<Extractor> __Game_Areas_Extractor_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Storage> __Game_Areas_Storage_RW_ComponentTypeHandle;
      public BufferTypeHandle<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attachment> __Game_Objects_Attachment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ExtractorAreaData> __Game_Prefabs_ExtractorAreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StorageAreaData> __Game_Prefabs_StorageAreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkVehicleData> __Game_Prefabs_WorkVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      public ComponentLookup<Game.Vehicles.WorkVehicle> __Game_Vehicles_WorkVehicle_RW_ComponentLookup;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> __Game_Areas_Node_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Triangle> __Game_Areas_Triangle_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferLookup<MapFeatureElement> __Game_Areas_MapFeatureElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;
      public ComponentLookup<Extractor> __Game_Areas_Extractor_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Geometry> __Game_Areas_Geometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Tree> __Game_Objects_Tree_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Plant> __Game_Objects_Plant_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Damaged> __Game_Objects_Damaged_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TreeData> __Game_Prefabs_TreeData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      public BufferLookup<WoodResource> __Game_Areas_WoodResource_RW_BufferLookup;
      public BufferLookup<MapFeatureElement> __Game_Areas_MapFeatureElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_WoodResource_RO_BufferTypeHandle = state.GetBufferTypeHandle<WoodResource>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Extractor_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Extractor>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Storage_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Storage>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RW_BufferTypeHandle = state.GetBufferTypeHandle<OwnedVehicle>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attachment_RO_ComponentLookup = state.GetComponentLookup<Attachment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup = state.GetComponentLookup<ExtractorAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StorageAreaData_RO_ComponentLookup = state.GetComponentLookup<StorageAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkVehicleData_RO_ComponentLookup = state.GetComponentLookup<WorkVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WorkVehicle_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.WorkVehicle>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferTypeHandle = state.GetBufferTypeHandle<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_MapFeatureElement_RO_BufferLookup = state.GetBufferLookup<MapFeatureElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Extractor_RW_ComponentLookup = state.GetComponentLookup<Extractor>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Geometry_RO_ComponentLookup = state.GetComponentLookup<Geometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentLookup = state.GetComponentLookup<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Plant_RO_ComponentLookup = state.GetComponentLookup<Plant>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RO_ComponentLookup = state.GetComponentLookup<Damaged>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TreeData_RO_ComponentLookup = state.GetComponentLookup<TreeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_WoodResource_RW_BufferLookup = state.GetBufferLookup<WoodResource>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_MapFeatureElement_RW_BufferLookup = state.GetBufferLookup<MapFeatureElement>();
      }
    }
  }
}
