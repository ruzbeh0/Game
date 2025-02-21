// Decompiled with JetBrains decompiler
// Type: Game.Areas.AreaResourceSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.City;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Serialization;
using Game.Simulation;
using Game.Tools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Areas
{
  [CompilerGenerated]
  public class AreaResourceSystem : GameSystemBase, IPostDeserialize
  {
    private Game.Objects.UpdateCollectSystem m_ObjectUpdateCollectSystem;
    private SearchSystem m_AreaSearchSystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private NaturalResourceSystem m_NaturalResourceSystem;
    private GroundWaterSystem m_GroundWaterSystem;
    private CitySystem m_CitySystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private EntityQuery m_UpdatedAreaQuery;
    private EntityQuery m_MapTileQuery;
    private EntityQuery m_BrushQuery;
    private NativeArray<float2> m_LastCityModifiers;
    private AreaResourceSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_596039173_0;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectUpdateCollectSystem = this.World.GetOrCreateSystemManaged<Game.Objects.UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NaturalResourceSystem = this.World.GetOrCreateSystemManaged<NaturalResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundWaterSystem = this.World.GetOrCreateSystemManaged<GroundWaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedAreaQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Extractor>(),
          ComponentType.ReadOnly<MapFeatureElement>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileQuery = this.GetEntityQuery(ComponentType.ReadOnly<MapFeatureElement>(), ComponentType.Exclude<Native>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_BrushQuery = this.GetEntityQuery(ComponentType.ReadOnly<Brush>(), ComponentType.ReadOnly<Applied>());
      // ISSUE: reference to a compiler-generated field
      this.m_LastCityModifiers = new NativeArray<float2>(2, Allocator.Persistent);
      this.RequireForUpdate<AreasConfigurationData>();
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      if (!(context.version < Version.buildableAreaFix))
        return;
      using (EntityQuery entityQuery = this.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<MapFeatureElement>()))
        this.EntityManager.AddComponent<Updated>(entityQuery);
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      // ISSUE: reference to a compiler-generated field
      if (this.m_CitySystem.City != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> buffer = this.EntityManager.GetBuffer<CityModifier>(this.m_CitySystem.City, true);
        // ISSUE: reference to a compiler-generated field
        this.m_LastCityModifiers[0] = CityUtils.GetModifier(buffer, CityModifierType.OreResourceAmount);
        // ISSUE: reference to a compiler-generated field
        this.m_LastCityModifiers[1] = CityUtils.GetModifier(buffer, CityModifierType.OilResourceAmount);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        ref NativeArray<float2> local1 = ref this.m_LastCityModifiers;
        // ISSUE: reference to a compiler-generated field
        ref NativeArray<float2> local2 = ref this.m_LastCityModifiers;
        float2 float2_1 = new float2();
        float2 float2_2 = float2_1;
        local2[1] = float2_2;
        float2 float2_3 = float2_1;
        local1[0] = float2_3;
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_LastCityModifiers.Dispose();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      bool flag = !this.m_BrushQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_UpdatedAreaQuery.IsEmptyIgnoreFilter && !flag && !this.m_ObjectUpdateCollectSystem.isUpdated)
        return;
      NativeQueue<Entity> nativeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<Entity> nativeList = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<Entity>.ParallelWriter parallelWriter = nativeQueue.AsParallelWriter();
      if (flag)
      {
        JobHandle outJobHandle;
        // ISSUE: reference to a compiler-generated field
        NativeList<Brush> componentDataListAsync = this.m_BrushQuery.ToComponentDataListAsync<Brush>((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_MapFeatureElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_WoodResource_RO_BufferLookup.Update(ref this.CheckedStateRef);
        JobHandle dependencies;
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
        JobHandle jobHandle = new AreaResourceSystem.FindUpdatedAreasWithBrushesJob()
        {
          m_Brushes = componentDataListAsync.AsDeferredJobArray(),
          m_AreaTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies),
          m_WoodResourceData = this.__TypeHandle.__Game_Areas_WoodResource_RO_BufferLookup,
          m_MapFeatureElements = this.__TypeHandle.__Game_Areas_MapFeatureElement_RO_BufferLookup,
          m_Nodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
          m_Triangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
          m_UpdateBuffer = parallelWriter
        }.Schedule<AreaResourceSystem.FindUpdatedAreasWithBrushesJob, Brush>(componentDataListAsync, 1, JobHandle.CombineDependencies(this.Dependency, outJobHandle, dependencies));
        componentDataListAsync.Dispose(jobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle);
        this.Dependency = jobHandle;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ObjectUpdateCollectSystem.isUpdated)
      {
        JobHandle dependencies1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedBounds = this.m_ObjectUpdateCollectSystem.GetUpdatedBounds(out dependencies1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_MapFeatureElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_WoodResource_RO_BufferLookup.Update(ref this.CheckedStateRef);
        JobHandle dependencies2;
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
        JobHandle jobHandle = new AreaResourceSystem.FindUpdatedAreasWithBoundsJob()
        {
          m_Bounds = updatedBounds.AsDeferredJobArray(),
          m_AreaTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies2),
          m_WoodResourceData = this.__TypeHandle.__Game_Areas_WoodResource_RO_BufferLookup,
          m_MapFeatureElements = this.__TypeHandle.__Game_Areas_MapFeatureElement_RO_BufferLookup,
          m_Nodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
          m_Triangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
          m_UpdateBuffer = parallelWriter
        }.Schedule<AreaResourceSystem.FindUpdatedAreasWithBoundsJob, Bounds2>(updatedBounds, 1, JobHandle.CombineDependencies(this.Dependency, dependencies1, dependencies2));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ObjectUpdateCollectSystem.AddBoundsReader(jobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle);
        this.Dependency = jobHandle;
      }
      JobHandle outJobHandle1;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync1 = this.m_UpdatedAreaQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle1);
      JobHandle outJobHandle2;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync2 = this.m_MapTileQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
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
      AreaResourceSystem.CollectUpdatedAreasJob jobData1 = new AreaResourceSystem.CollectUpdatedAreasJob()
      {
        m_UpdatedAreaChunks = archetypeChunkListAsync1,
        m_MapTileChunks = archetypeChunkListAsync2,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_UpdateBuffer = nativeQueue,
        m_UpdateList = nativeList,
        m_LastCityModifiers = this.m_LastCityModifiers,
        m_City = this.m_CitySystem.City
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
      JobHandle dependencies5;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AreaResourceSystem.UpdateAreaResourcesJob jobData2 = new AreaResourceSystem.UpdateAreaResourcesJob()
      {
        m_City = this.m_CitySystem.City,
        m_FullUpdate = true,
        m_UpdateList = nativeList.AsDeferredJobArray(),
        m_ObjectTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies3),
        m_NaturalResourceData = this.m_NaturalResourceSystem.GetData(true, out dependencies4),
        m_GroundWaterResourceData = this.m_GroundWaterSystem.GetData(true, out dependencies5),
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
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_BuildableLandMaxSlope = this.__query_596039173_0.GetSingleton<AreasConfigurationData>().m_BuildableLandMaxSlope
      };
      JobHandle jobHandle1 = jobData1.Schedule<AreaResourceSystem.CollectUpdatedAreasJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle1, outJobHandle2));
      NativeList<Entity> list = nativeList;
      JobHandle dependsOn = JobUtils.CombineDependencies(jobHandle1, dependencies3, dependencies4, deps, dependencies5);
      JobHandle jobHandle2 = jobData2.Schedule<AreaResourceSystem.UpdateAreaResourcesJob, Entity>(list, 1, dependsOn);
      nativeQueue.Dispose(jobHandle1);
      nativeList.Dispose(jobHandle2);
      archetypeChunkListAsync1.Dispose(jobHandle1);
      archetypeChunkListAsync2.Dispose(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_NaturalResourceSystem.AddReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_GroundWaterSystem.AddReader(jobHandle2);
      this.Dependency = jobHandle2;
    }

    public static float CalculateBuildable(
      float3 worldPos,
      float2 cellSize,
      WaterSurfaceData m_WaterSurfaceData,
      TerrainHeightData terrainHeightData,
      Bounds1 buildableLandMaxSlope)
    {
      double num1 = (double) WaterUtils.SampleDepth(ref m_WaterSurfaceData, worldPos);
      float buildable = 0.0f;
      if (num1 < 0.10000000149011612)
      {
        float num2 = TerrainUtils.SampleHeight(ref terrainHeightData, worldPos + new float3(-0.5f * cellSize.x, 0.0f, 0.0f));
        float num3 = TerrainUtils.SampleHeight(ref terrainHeightData, worldPos + new float3(0.5f * cellSize.x, 0.0f, 0.0f));
        float3 x1 = new float3(cellSize.x, num3 - num2, 0.0f);
        float num4 = TerrainUtils.SampleHeight(ref terrainHeightData, worldPos + new float3(0.0f, 0.0f, -cellSize.y));
        float3 y1 = new float3(0.0f, TerrainUtils.SampleHeight(ref terrainHeightData, worldPos + new float3(0.0f, 0.0f, cellSize.y)) - num4, cellSize.y);
        float3 x2 = math.cross(x1, y1);
        float3 y2 = math.up();
        float x3 = math.length(math.cross(x2, y2)) / math.dot(x2, y2);
        buildable = math.saturate(math.unlerp(buildableLandMaxSlope.max, buildableLandMaxSlope.min, math.abs(x3)));
      }
      return buildable;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_596039173_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<AreasConfigurationData>()
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

    [UnityEngine.Scripting.Preserve]
    public AreaResourceSystem()
    {
    }

    [BurstCompile]
    private struct FindUpdatedAreasWithBrushesJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Brush> m_Brushes;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaTree;
      [ReadOnly]
      public BufferLookup<WoodResource> m_WoodResourceData;
      [ReadOnly]
      public BufferLookup<MapFeatureElement> m_MapFeatureElements;
      [ReadOnly]
      public BufferLookup<Node> m_Nodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_Triangles;
      public NativeQueue<Entity>.ParallelWriter m_UpdateBuffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AreaResourceSystem.FindUpdatedAreasWithBrushesJob.AreaIterator iterator = new AreaResourceSystem.FindUpdatedAreasWithBrushesJob.AreaIterator()
        {
          m_Bounds = ToolUtils.GetBounds(this.m_Brushes[index]),
          m_WoodResourceData = this.m_WoodResourceData,
          m_MapFeatureElements = this.m_MapFeatureElements,
          m_Nodes = this.m_Nodes,
          m_Triangles = this.m_Triangles,
          m_UpdateBuffer = this.m_UpdateBuffer
        };
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTree.Iterate<AreaResourceSystem.FindUpdatedAreasWithBrushesJob.AreaIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_WoodResourceData = iterator.m_WoodResourceData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_MapFeatureElements = iterator.m_MapFeatureElements;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Nodes = iterator.m_Nodes;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Triangles = iterator.m_Triangles;
      }

      private struct AreaIterator : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public BufferLookup<WoodResource> m_WoodResourceData;
        public BufferLookup<MapFeatureElement> m_MapFeatureElements;
        public BufferLookup<Node> m_Nodes;
        public BufferLookup<Triangle> m_Triangles;
        public NativeQueue<Entity>.ParallelWriter m_UpdateBuffer;

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
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || !this.m_WoodResourceData.HasBuffer(item.m_Area) && !this.m_MapFeatureElements.HasBuffer(item.m_Area) || !MathUtils.Intersect(this.m_Bounds, AreaUtils.GetTriangle2(this.m_Nodes[item.m_Area], this.m_Triangles[item.m_Area][item.m_Triangle])))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateBuffer.Enqueue(item.m_Area);
        }
      }
    }

    [BurstCompile]
    private struct FindUpdatedAreasWithBoundsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Bounds2> m_Bounds;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaTree;
      [ReadOnly]
      public BufferLookup<WoodResource> m_WoodResourceData;
      [ReadOnly]
      public BufferLookup<MapFeatureElement> m_MapFeatureElements;
      [ReadOnly]
      public BufferLookup<Node> m_Nodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_Triangles;
      public NativeQueue<Entity>.ParallelWriter m_UpdateBuffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AreaResourceSystem.FindUpdatedAreasWithBoundsJob.AreaIterator iterator = new AreaResourceSystem.FindUpdatedAreasWithBoundsJob.AreaIterator()
        {
          m_Bounds = this.m_Bounds[index],
          m_WoodResourceData = this.m_WoodResourceData,
          m_MapFeatureElements = this.m_MapFeatureElements,
          m_Nodes = this.m_Nodes,
          m_Triangles = this.m_Triangles,
          m_UpdateBuffer = this.m_UpdateBuffer
        };
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTree.Iterate<AreaResourceSystem.FindUpdatedAreasWithBoundsJob.AreaIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_WoodResourceData = iterator.m_WoodResourceData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_MapFeatureElements = iterator.m_MapFeatureElements;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Nodes = iterator.m_Nodes;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Triangles = iterator.m_Triangles;
      }

      private struct AreaIterator : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public BufferLookup<WoodResource> m_WoodResourceData;
        public BufferLookup<MapFeatureElement> m_MapFeatureElements;
        public BufferLookup<Node> m_Nodes;
        public BufferLookup<Triangle> m_Triangles;
        public NativeQueue<Entity>.ParallelWriter m_UpdateBuffer;

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
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || !this.m_WoodResourceData.HasBuffer(item.m_Area) && !this.m_MapFeatureElements.HasBuffer(item.m_Area) || !MathUtils.Intersect(this.m_Bounds, AreaUtils.GetTriangle2(this.m_Nodes[item.m_Area], this.m_Triangles[item.m_Area][item.m_Triangle])))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateBuffer.Enqueue(item.m_Area);
        }
      }
    }

    [BurstCompile]
    private struct CollectUpdatedAreasJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_UpdatedAreaChunks;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_MapTileChunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      public NativeQueue<Entity> m_UpdateBuffer;
      public NativeList<Entity> m_UpdateList;
      public NativeArray<float2> m_LastCityModifiers;
      public Entity m_City;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_UpdateBuffer.Count;
        int num1 = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_UpdatedAreaChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          num1 += this.m_UpdatedAreaChunks[index].Count;
        }
        // ISSUE: reference to a compiler-generated method
        bool flag = this.UpdateResourceModifiers();
        if (flag)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_MapTileChunks.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            num1 += this.m_MapTileChunks[index].Count;
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateList.ResizeUninitialized(count + num1);
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateList[index] = this.m_UpdateBuffer.Dequeue();
        }
        ArchetypeChunk archetypeChunk;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_UpdatedAreaChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          archetypeChunk = this.m_UpdatedAreaChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray = archetypeChunk.GetNativeArray(this.m_EntityType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_UpdateList[count++] = nativeArray[index2];
          }
        }
        if (flag)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index3 = 0; index3 < this.m_MapTileChunks.Length; ++index3)
          {
            // ISSUE: reference to a compiler-generated field
            archetypeChunk = this.m_MapTileChunks[index3];
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray = archetypeChunk.GetNativeArray(this.m_EntityType);
            for (int index4 = 0; index4 < nativeArray.Length; ++index4)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_UpdateList[count++] = nativeArray[index4];
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        NativeList<Entity> updateList = this.m_UpdateList;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AreaResourceSystem.CollectUpdatedAreasJob.EntityComparer entityComparer = new AreaResourceSystem.CollectUpdatedAreasJob.EntityComparer();
        // ISSUE: variable of a compiler-generated type
        AreaResourceSystem.CollectUpdatedAreasJob.EntityComparer comp = entityComparer;
        updateList.Sort<Entity, AreaResourceSystem.CollectUpdatedAreasJob.EntityComparer>(comp);
        Entity entity = Entity.Null;
        int num2 = 0;
        int index5 = 0;
        // ISSUE: reference to a compiler-generated field
        while (num2 < this.m_UpdateList.Length)
        {
          // ISSUE: reference to a compiler-generated field
          Entity update = this.m_UpdateList[num2++];
          if (update != entity)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_UpdateList[index5++] = update;
            entity = update;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (index5 >= this.m_UpdateList.Length)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateList.RemoveRange(index5, this.m_UpdateList.Length - index5);
      }

      private bool UpdateResourceModifiers()
      {
        float2 rhs1;
        float2 rhs2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_City != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
          rhs1 = CityUtils.GetModifier(cityModifier, CityModifierType.OreResourceAmount);
          rhs2 = CityUtils.GetModifier(cityModifier, CityModifierType.OilResourceAmount);
        }
        else
        {
          rhs2 = new float2();
          rhs1 = rhs2;
        }
        // ISSUE: reference to a compiler-generated field
        float2 lastCityModifier = this.m_LastCityModifiers[0];
        int num;
        if (lastCityModifier.Equals(rhs1))
        {
          // ISSUE: reference to a compiler-generated field
          lastCityModifier = this.m_LastCityModifiers[1];
          num = !lastCityModifier.Equals(rhs2) ? 1 : 0;
        }
        else
          num = 1;
        // ISSUE: reference to a compiler-generated field
        this.m_LastCityModifiers[0] = rhs1;
        // ISSUE: reference to a compiler-generated field
        this.m_LastCityModifiers[1] = rhs2;
        return num != 0;
      }

      [StructLayout(LayoutKind.Sequential, Size = 1)]
      private struct EntityComparer : IComparer<Entity>
      {
        public int Compare(Entity x, Entity y) => x.Index - y.Index;
      }
    }

    [BurstCompile]
    public struct UpdateAreaResourcesJob : IJobParallelForDefer
    {
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public bool m_FullUpdate;
      [ReadOnly]
      public NativeArray<Entity> m_UpdateList;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectTree;
      [ReadOnly]
      public CellMapData<NaturalResourceCell> m_NaturalResourceData;
      [ReadOnly]
      public CellMapData<GroundWater> m_GroundWaterResourceData;
      [ReadOnly]
      public ComponentLookup<Geometry> m_GeometryData;
      [ReadOnly]
      public ComponentLookup<Tree> m_TreeData;
      [ReadOnly]
      public ComponentLookup<Plant> m_PlantData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Damaged> m_DamagedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ExtractorAreaData> m_ExtractorAreaData;
      [ReadOnly]
      public ComponentLookup<TreeData> m_PrefabTreeData;
      [ReadOnly]
      public BufferLookup<Node> m_Nodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_Triangles;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Extractor> m_ExtractorData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<WoodResource> m_WoodResources;
      [NativeDisableParallelForRestriction]
      public BufferLookup<MapFeatureElement> m_MapFeatureElements;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      public Bounds1 m_BuildableLandMaxSlope;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity update = this.m_UpdateList[index];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Node> node = this.m_Nodes[update];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Triangle> triangle = this.m_Triangles[update];
        DynamicBuffer<CityModifier> cityModifiers = new DynamicBuffer<CityModifier>();
        // ISSUE: reference to a compiler-generated field
        if (this.m_City != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cityModifiers = this.m_CityModifiers[this.m_City];
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ExtractorData.HasComponent(update))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ExtractorAreaData extractorAreaData = this.m_ExtractorAreaData[this.m_PrefabRefData[update].m_Prefab];
          // ISSUE: reference to a compiler-generated field
          Extractor extractor = this.m_ExtractorData[update] with
          {
            m_ResourceAmount = 0.0f,
            m_MaxConcentration = 0.0f
          };
          switch (extractorAreaData.m_MapFeature)
          {
            case MapFeature.FertileLand:
            case MapFeature.Oil:
            case MapFeature.Ore:
              // ISSUE: reference to a compiler-generated method
              this.CalculateNaturalResources(node, triangle, cityModifiers, ref extractor, extractorAreaData.m_MapFeature);
              break;
            case MapFeature.Forest:
              // ISSUE: reference to a compiler-generated field
              if (this.m_WoodResources.HasBuffer(update))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<WoodResource> woodResource = this.m_WoodResources[update];
                // ISSUE: reference to a compiler-generated method
                this.CalculateWoodResources(node, triangle, ref extractor, woodResource);
                break;
              }
              break;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_ExtractorData[update] = extractor;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_MapFeatureElements.HasBuffer(update))
          return;
        // ISSUE: reference to a compiler-generated field
        Geometry geometry = this.m_GeometryData[update];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<MapFeatureElement> mapFeatureElement = this.m_MapFeatureElements[update];
        CollectionUtils.ResizeInitialized<MapFeatureElement>(mapFeatureElement, 8);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AreaResourceSystem.WoodIterator iterator = new AreaResourceSystem.WoodIterator()
        {
          m_TreeData = this.m_TreeData,
          m_PlantData = this.m_PlantData,
          m_TransformData = this.m_TransformData,
          m_DamagedData = this.m_DamagedData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabTreeData = this.m_PrefabTreeData
        };
        for (int index1 = 0; index1 < triangle.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          iterator.m_Triangle = AreaUtils.GetTriangle2(node, triangle[index1]);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          iterator.m_Bounds = MathUtils.Bounds(iterator.m_Triangle);
          // ISSUE: reference to a compiler-generated field
          this.m_ObjectTree.Iterate<AreaResourceSystem.WoodIterator>(ref iterator);
        }
        float4 zero1 = float4.zero;
        float groundWater = 0.0f;
        float4 zero2 = float4.zero;
        // ISSUE: reference to a compiler-generated method
        this.CalculateNaturalResources(node, triangle, cityModifiers, ref zero1, ref zero2, ref groundWater);
        mapFeatureElement[0] = new MapFeatureElement(geometry.m_SurfaceArea, 0.0f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        mapFeatureElement[3] = new MapFeatureElement(iterator.m_WoodAmount, iterator.m_GrowthRate);
        mapFeatureElement[2] = new MapFeatureElement(zero1.x, zero2.x);
        mapFeatureElement[5] = new MapFeatureElement(zero1.y, zero2.y);
        mapFeatureElement[4] = new MapFeatureElement(zero1.z, zero2.z);
        mapFeatureElement[7] = new MapFeatureElement(groundWater, 0.0f);
        mapFeatureElement[1] = new MapFeatureElement(zero1.w, 0.0f);
      }

      private void CalculateWoodResources(
        DynamicBuffer<Node> nodes,
        DynamicBuffer<Triangle> triangles,
        ref Extractor extractor,
        DynamicBuffer<WoodResource> woodResources)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_FullUpdate)
        {
          woodResources.Clear();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          AreaResourceSystem.TreeIterator iterator = new AreaResourceSystem.TreeIterator()
          {
            m_TransformData = this.m_TransformData,
            m_PrefabRefData = this.m_PrefabRefData,
            m_PrefabTreeData = this.m_PrefabTreeData,
            m_Buffer = woodResources
          };
          for (int index = 0; index < triangles.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            iterator.m_Triangle = AreaUtils.GetTriangle2(nodes, triangles[index]);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            iterator.m_Bounds = MathUtils.Bounds(iterator.m_Triangle);
            // ISSUE: reference to a compiler-generated field
            this.m_ObjectTree.Iterate<AreaResourceSystem.TreeIterator>(ref iterator);
          }
          NativeArray<WoodResource> array = woodResources.AsNativeArray();
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          AreaResourceSystem.UpdateAreaResourcesJob.WoodResourceComparer resourceComparer = new AreaResourceSystem.UpdateAreaResourcesJob.WoodResourceComparer();
          // ISSUE: variable of a compiler-generated type
          AreaResourceSystem.UpdateAreaResourcesJob.WoodResourceComparer comp = resourceComparer;
          array.Sort<WoodResource, AreaResourceSystem.UpdateAreaResourcesJob.WoodResourceComparer>(comp);
          WoodResource woodResource1 = new WoodResource();
          int num = 0;
          int index1 = 0;
          while (num < woodResources.Length)
          {
            WoodResource woodResource2 = woodResources[num++];
            if (woodResource2.m_Tree != woodResource1.m_Tree)
            {
              woodResources[index1++] = woodResource2;
              woodResource1 = woodResource2;
            }
          }
          if (index1 < woodResources.Length)
            woodResources.RemoveRange(index1, woodResources.Length - index1);
        }
        for (int index = 0; index < woodResources.Length; ++index)
        {
          Entity tree1 = woodResources[index].m_Tree;
          // ISSUE: reference to a compiler-generated field
          Tree tree2 = this.m_TreeData[tree1];
          // ISSUE: reference to a compiler-generated field
          Plant plant = this.m_PlantData[tree1];
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[tree1];
          Damaged componentData1;
          // ISSUE: reference to a compiler-generated field
          this.m_DamagedData.TryGetComponent(tree1, out componentData1);
          TreeData componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabTreeData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
          {
            float woodAmount = ObjectUtils.CalculateWoodAmount(tree2, plant, componentData1, componentData2);
            if ((double) woodAmount > 0.0)
            {
              extractor.m_ResourceAmount += woodAmount;
              extractor.m_MaxConcentration = math.max(extractor.m_MaxConcentration, woodAmount * (1f / componentData2.m_WoodAmount));
            }
          }
        }
        extractor.m_MaxConcentration = math.min(extractor.m_MaxConcentration, 1f);
      }

      private void CalculateNaturalResources(
        DynamicBuffer<Node> nodes,
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
        for (int index = 0; index < triangles.Length; ++index)
        {
          Triangle2 triangle2 = AreaUtils.GetTriangle2(nodes, triangles[index]);
          Bounds2 bounds2 = MathUtils.Bounds(triangle2);
          // ISSUE: reference to a compiler-generated field
          int4 int4 = math.clamp((int4) math.floor(new float4(bounds2.min, bounds2.max) * xyxy1 + xyxy2), (int4) 0, this.m_NaturalResourceData.m_TextureSize.xyxy - 1);
          float num2 = 0.0f;
          float num3 = 0.0f;
          for (int y = int4.y; y <= int4.w; ++y)
          {
            Bounds2 bounds;
            // ISSUE: reference to a compiler-generated field
            bounds.min.y = ((float) y - xyxy2.y) * this.m_NaturalResourceData.m_CellSize.y;
            // ISSUE: reference to a compiler-generated field
            bounds.max.y = bounds.min.y + this.m_NaturalResourceData.m_CellSize.y;
            for (int x1 = int4.x; x1 <= int4.z; ++x1)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              NaturalResourceCell naturalResourceCell = this.m_NaturalResourceData.m_Buffer[x1 + this.m_NaturalResourceData.m_TextureSize.x * y];
              float x2;
              switch (mapFeature)
              {
                case MapFeature.FertileLand:
                  x2 = (float) naturalResourceCell.m_Fertility.m_Base;
                  x2 -= (float) naturalResourceCell.m_Fertility.m_Used;
                  break;
                case MapFeature.Oil:
                  x2 = (float) naturalResourceCell.m_Oil.m_Base;
                  if (cityModifiers.IsCreated)
                    CityUtils.ApplyModifier(ref x2, cityModifiers, CityModifierType.OilResourceAmount);
                  x2 -= (float) naturalResourceCell.m_Oil.m_Used;
                  break;
                case MapFeature.Ore:
                  x2 = (float) naturalResourceCell.m_Ore.m_Base;
                  if (cityModifiers.IsCreated)
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
                  num2 += area * math.min(x2 * 0.0001f, 1f);
                  num3 += area;
                  extractor.m_ResourceAmount += x2 * area * num1;
                }
              }
            }
          }
          float y1 = (double) num3 > 0.0099999997764825821 ? num2 / num3 : 0.0f;
          extractor.m_MaxConcentration = math.max(extractor.m_MaxConcentration, y1);
        }
      }

      private void CalculateNaturalResources(
        DynamicBuffer<Node> nodes,
        DynamicBuffer<Triangle> triangles,
        DynamicBuffer<CityModifier> cityModifiers,
        ref float4 resources,
        ref float4 renewal,
        ref float groundWater)
      {
        float3 b = new float3(800f, 0.0f, 0.0f);
        // ISSUE: reference to a compiler-generated field
        float4 xyxy1 = (1f / this.m_NaturalResourceData.m_CellSize).xyxy;
        // ISSUE: reference to a compiler-generated field
        float4 xyxy2 = ((float2) this.m_NaturalResourceData.m_TextureSize * 0.5f).xyxy;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num1 = (float) (1.0 / ((double) this.m_NaturalResourceData.m_CellSize.x * (double) this.m_NaturalResourceData.m_CellSize.y));
        for (int index = 0; index < triangles.Length; ++index)
        {
          Triangle2 triangle2 = AreaUtils.GetTriangle2(nodes, triangles[index]);
          Bounds2 bounds2 = MathUtils.Bounds(triangle2);
          // ISSUE: reference to a compiler-generated field
          int4 int4 = math.clamp((int4) math.floor(new float4(bounds2.min, bounds2.max) * xyxy1 + xyxy2), (int4) 0, this.m_NaturalResourceData.m_TextureSize.xyxy - 1);
          for (int y = int4.y; y <= int4.w; ++y)
          {
            Bounds2 bounds;
            // ISSUE: reference to a compiler-generated field
            bounds.min.y = ((float) y - xyxy2.y) * this.m_NaturalResourceData.m_CellSize.y;
            // ISSUE: reference to a compiler-generated field
            bounds.max.y = bounds.min.y + this.m_NaturalResourceData.m_CellSize.y;
            for (int x1 = int4.x; x1 <= int4.z; ++x1)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              NaturalResourceCell naturalResourceCell = this.m_NaturalResourceData.m_Buffer[x1 + this.m_NaturalResourceData.m_TextureSize.x * y];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              GroundWater groundWater1 = this.m_GroundWaterResourceData.m_Buffer[x1 + this.m_GroundWaterResourceData.m_TextureSize.x * y];
              float3 x2 = naturalResourceCell.GetBaseResources();
              float3 usedResources = naturalResourceCell.GetUsedResources();
              if (cityModifiers.IsCreated)
              {
                CityUtils.ApplyModifier(ref x2.y, cityModifiers, CityModifierType.OreResourceAmount);
                CityUtils.ApplyModifier(ref x2.z, cityModifiers, CityModifierType.OilResourceAmount);
              }
              float3 xyz = math.clamp(x2, (float3) 0.0f, b);
              x2 -= usedResources;
              x2 = math.clamp(x2, (float3) 0.0f, (float3) (float) ushort.MaxValue);
              // ISSUE: reference to a compiler-generated field
              bounds.min.x = ((float) x1 - xyxy2.x) * this.m_NaturalResourceData.m_CellSize.x;
              // ISSUE: reference to a compiler-generated field
              bounds.max.x = bounds.min.x + this.m_NaturalResourceData.m_CellSize.x;
              float3 worldPos = new float3((float) (0.5 * ((double) bounds.min.x + (double) bounds.max.x)), 0.0f, (float) (0.5 * ((double) bounds.min.y + (double) bounds.max.y)));
              groundWater += (float) groundWater1.m_Amount;
              // ISSUE: reference to a compiler-generated field
              float2 cellSize = this.m_NaturalResourceData.m_CellSize;
              // ISSUE: reference to a compiler-generated field
              WaterSurfaceData waterSurfaceData = this.m_WaterSurfaceData;
              // ISSUE: reference to a compiler-generated field
              TerrainHeightData terrainHeightData = this.m_TerrainHeightData;
              // ISSUE: reference to a compiler-generated field
              Bounds1 buildableLandMaxSlope = this.m_BuildableLandMaxSlope;
              // ISSUE: reference to a compiler-generated method
              float buildable = AreaResourceSystem.CalculateBuildable(worldPos, cellSize, waterSurfaceData, terrainHeightData, buildableLandMaxSlope);
              float area;
              if (MathUtils.Intersect(bounds, triangle2, out area))
              {
                float num2 = area * num1;
                resources += new float4(x2 * num2, buildable * area);
                renewal += new float4(xyz, 0.0f) * num2;
              }
            }
          }
        }
      }

      [StructLayout(LayoutKind.Sequential, Size = 1)]
      private struct WoodResourceComparer : IComparer<WoodResource>
      {
        public int Compare(WoodResource x, WoodResource y) => x.m_Tree.Index - y.m_Tree.Index;
      }
    }

    private struct TreeIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public Bounds2 m_Bounds;
      public Triangle2 m_Triangle;
      public ComponentLookup<Transform> m_TransformData;
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public ComponentLookup<TreeData> m_PrefabTreeData;
      public DynamicBuffer<WoodResource> m_Buffer;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (bounds.m_Mask & (BoundsMask.IsTree | BoundsMask.NotOverridden)) == (BoundsMask.IsTree | BoundsMask.NotOverridden) && MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) && MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Triangle);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((bounds.m_Mask & (BoundsMask.IsTree | BoundsMask.NotOverridden)) != (BoundsMask.IsTree | BoundsMask.NotOverridden) || !MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || !MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Triangle) || !MathUtils.Intersect(this.m_Triangle, this.m_TransformData[entity].m_Position.xz))
          return;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabTreeData.HasComponent(prefabRef.m_Prefab) || (double) this.m_PrefabTreeData[prefabRef.m_Prefab].m_WoodAmount < 1.0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_Buffer.Add(new WoodResource(entity));
      }
    }

    private struct WoodIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public Bounds2 m_Bounds;
      public Triangle2 m_Triangle;
      public ComponentLookup<Tree> m_TreeData;
      public ComponentLookup<Plant> m_PlantData;
      public ComponentLookup<Transform> m_TransformData;
      public ComponentLookup<Damaged> m_DamagedData;
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public ComponentLookup<TreeData> m_PrefabTreeData;
      public float m_WoodAmount;
      public float m_GrowthRate;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (bounds.m_Mask & (BoundsMask.IsTree | BoundsMask.NotOverridden)) == (BoundsMask.IsTree | BoundsMask.NotOverridden) && MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) && MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Triangle);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((bounds.m_Mask & (BoundsMask.IsTree | BoundsMask.NotOverridden)) != (BoundsMask.IsTree | BoundsMask.NotOverridden) || !MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || !MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Triangle) || !MathUtils.Intersect(this.m_Triangle, this.m_TransformData[entity].m_Position.xz))
          return;
        // ISSUE: reference to a compiler-generated field
        Tree tree = this.m_TreeData[entity];
        // ISSUE: reference to a compiler-generated field
        Plant plant = this.m_PlantData[entity];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity];
        Damaged componentData1;
        // ISSUE: reference to a compiler-generated field
        this.m_DamagedData.TryGetComponent(entity, out componentData1);
        TreeData componentData2;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabTreeData.TryGetComponent(prefabRef.m_Prefab, out componentData2) || (double) componentData2.m_WoodAmount < 1.0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_WoodAmount += ObjectUtils.CalculateWoodAmount(tree, plant, componentData1, componentData2);
        // ISSUE: reference to a compiler-generated field
        this.m_GrowthRate += ObjectUtils.CalculateGrowthRate(tree, plant, componentData2);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferLookup<WoodResource> __Game_Areas_WoodResource_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MapFeatureElement> __Game_Areas_MapFeatureElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Geometry> __Game_Areas_Geometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Tree> __Game_Objects_Tree_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Plant> __Game_Objects_Plant_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Damaged> __Game_Objects_Damaged_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ExtractorAreaData> __Game_Prefabs_ExtractorAreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TreeData> __Game_Prefabs_TreeData_RO_ComponentLookup;
      public ComponentLookup<Extractor> __Game_Areas_Extractor_RW_ComponentLookup;
      public BufferLookup<WoodResource> __Game_Areas_WoodResource_RW_BufferLookup;
      public BufferLookup<MapFeatureElement> __Game_Areas_MapFeatureElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_WoodResource_RO_BufferLookup = state.GetBufferLookup<WoodResource>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_MapFeatureElement_RO_BufferLookup = state.GetBufferLookup<MapFeatureElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Geometry_RO_ComponentLookup = state.GetComponentLookup<Geometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentLookup = state.GetComponentLookup<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Plant_RO_ComponentLookup = state.GetComponentLookup<Plant>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RO_ComponentLookup = state.GetComponentLookup<Damaged>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup = state.GetComponentLookup<ExtractorAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TreeData_RO_ComponentLookup = state.GetComponentLookup<TreeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Extractor_RW_ComponentLookup = state.GetComponentLookup<Extractor>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_WoodResource_RW_BufferLookup = state.GetBufferLookup<WoodResource>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_MapFeatureElement_RW_BufferLookup = state.GetBufferLookup<MapFeatureElement>();
      }
    }
  }
}
