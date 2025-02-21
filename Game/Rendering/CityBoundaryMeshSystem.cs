// Decompiled with JetBrains decompiler
// Type: Game.Rendering.CityBoundaryMeshSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Common;
using Game.Prefabs;
using Game.Serialization;
using Game.Simulation;
using Game.Tools;
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
  public class CityBoundaryMeshSystem : GameSystemBase, IPreDeserialize
  {
    private PrefabSystem m_PrefabSystem;
    private ToolSystem m_ToolSystem;
    private MapTileSystem m_MapTileSystem;
    private TerrainSystem m_TerrainSystem;
    private SearchSystem m_AreaSearchSystem;
    private EntityQuery m_UpdatedQuery;
    private EntityQuery m_MapTileQuery;
    private EntityQuery m_SettingsQuery;
    private Mesh m_BoundaryMesh;
    private Material m_BoundaryMaterial;
    private JobHandle m_MeshDependencies;
    private NativeList<float3> m_Vertices;
    private NativeList<float2> m_UVs;
    private NativeList<Color32> m_Colors;
    private NativeList<int> m_Indices;
    private NativeValue<Bounds3> m_Bounds;
    private bool m_Loaded;
    private CityBoundaryMeshSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileSystem = this.World.GetOrCreateSystemManaged<MapTileSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<MapTile>(),
          ComponentType.ReadOnly<Area>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileQuery = this.GetEntityQuery(ComponentType.ReadOnly<MapTile>(), ComponentType.ReadOnly<Area>(), ComponentType.ReadOnly<Game.Areas.Node>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_SettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<CityBoundaryData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_SettingsQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated method
      this.Clear();
      base.OnDestroy();
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated method
      this.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    private void Clear()
    {
      // ISSUE: reference to a compiler-generated method
      this.DisposeMeshData();
      // ISSUE: reference to a compiler-generated method
      this.DestroyMesh();
      // ISSUE: reference to a compiler-generated field
      this.m_BoundaryMaterial = (Material) null;
    }

    private void DestroyMesh()
    {
      // ISSUE: reference to a compiler-generated field
      if (!((Object) this.m_BoundaryMesh != (Object) null))
        return;
      // ISSUE: reference to a compiler-generated field
      Object.Destroy((Object) this.m_BoundaryMesh);
      // ISSUE: reference to a compiler-generated field
      this.m_BoundaryMesh = (Mesh) null;
    }

    private void DisposeMeshData()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Vertices.IsCreated)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_MeshDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_MeshDependencies = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      this.m_Vertices.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_UVs.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Colors.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Indices.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Bounds.Dispose();
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
      if (!this.GetLoaded() && this.m_UpdatedQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated method
      this.DisposeMeshData();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      CityBoundaryPrefab prefab = this.m_PrefabSystem.GetPrefab<CityBoundaryPrefab>(this.m_SettingsQuery.GetSingletonEntity());
      // ISSUE: reference to a compiler-generated field
      this.m_BoundaryMaterial = prefab.m_Material;
      NativeQueue<CityBoundaryMeshSystem.Boundary> nativeQueue = new NativeQueue<CityBoundaryMeshSystem.Boundary>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      this.m_Vertices = new NativeList<float3>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      this.m_UVs = new NativeList<float2>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      this.m_Colors = new NativeList<Color32>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      this.m_Indices = new NativeList<int>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      this.m_Bounds = new NativeValue<Bounds3>(Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_MapTile_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Native_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      CityBoundaryMeshSystem.FillBoundaryQueueJob jobData1 = new CityBoundaryMeshSystem.FillBoundaryQueueJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_NativeType = this.__TypeHandle.__Game_Common_Native_RO_ComponentTypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle,
        m_MapTileData = this.__TypeHandle.__Game_Areas_MapTile_RO_ComponentLookup,
        m_NativeData = this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup,
        m_Nodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_Triangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_SearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies),
        m_StartTiles = this.m_MapTileSystem.GetStartTiles(),
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_CityBorderColor = (Color32) prefab.m_CityBorderColor.linear,
        m_MapBorderColor = (Color32) prefab.m_MapBorderColor.linear,
        m_BoundaryQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CityBoundaryMeshSystem.FillBoundaryMeshDataJob jobData2 = new CityBoundaryMeshSystem.FillBoundaryMeshDataJob()
      {
        m_Width = prefab.m_Width,
        m_TilingLength = prefab.m_TilingLength,
        m_BoundaryQueue = nativeQueue,
        m_Vertices = this.m_Vertices,
        m_UVs = this.m_UVs,
        m_Colors = this.m_Colors,
        m_Indices = this.m_Indices,
        m_Bounds = this.m_Bounds
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.ScheduleParallel<CityBoundaryMeshSystem.FillBoundaryQueueJob>(this.m_MapTileQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      JobHandle dependsOn = jobHandle;
      JobHandle inputDeps = jobData2.Schedule<CityBoundaryMeshSystem.FillBoundaryMeshDataJob>(dependsOn);
      nativeQueue.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_MeshDependencies = inputDeps;
      this.Dependency = jobHandle;
    }

    public bool GetBoundaryMesh(out Mesh mesh, out Material material)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_Vertices.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_MeshDependencies.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_MeshDependencies = new JobHandle();
        // ISSUE: reference to a compiler-generated field
        if (this.m_Vertices.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          if ((Object) this.m_BoundaryMesh == (Object) null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_BoundaryMesh = new Mesh();
            // ISSUE: reference to a compiler-generated field
            this.m_BoundaryMesh.name = "City boundaries";
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_BoundaryMesh.Clear();
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_BoundaryMesh.SetVertices<float3>(this.m_Vertices.AsArray());
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_BoundaryMesh.SetUVs<float2>(0, this.m_UVs.AsArray());
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_BoundaryMesh.SetColors<Color32>(this.m_Colors.AsArray());
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_BoundaryMesh.SetIndices<int>(this.m_Indices.AsArray(), MeshTopology.Triangles, 0, false);
          // ISSUE: reference to a compiler-generated field
          float2 heightScaleOffset = this.m_TerrainSystem.heightScaleOffset;
          // ISSUE: reference to a compiler-generated field
          Bounds3 bounds = this.m_Bounds.value;
          bounds.min.y = heightScaleOffset.y;
          bounds.max.y = heightScaleOffset.y + heightScaleOffset.x;
          // ISSUE: reference to a compiler-generated field
          this.m_BoundaryMesh.bounds = RenderingUtils.ToBounds(bounds);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.DestroyMesh();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Vertices.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_UVs.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_Colors.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_Indices.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_Bounds.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      mesh = this.m_BoundaryMesh;
      // ISSUE: reference to a compiler-generated field
      material = this.m_BoundaryMaterial;
      // ISSUE: reference to a compiler-generated field
      return (Object) this.m_BoundaryMesh != (Object) null;
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
    public CityBoundaryMeshSystem()
    {
    }

    private struct Boundary
    {
      public Line3.Segment m_Line;
      public Color32 m_Color;
    }

    [BurstCompile]
    private struct FillBoundaryQueueJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Native> m_NativeType;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> m_NodeType;
      [ReadOnly]
      public ComponentLookup<MapTile> m_MapTileData;
      [ReadOnly]
      public ComponentLookup<Native> m_NativeData;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_Nodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_Triangles;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_SearchTree;
      [ReadOnly]
      public NativeList<Entity> m_StartTiles;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public Color32 m_CityBorderColor;
      [ReadOnly]
      public Color32 m_MapBorderColor;
      public NativeQueue<CityBoundaryMeshSystem.Boundary>.ParallelWriter m_BoundaryQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Areas.Node> bufferAccessor = chunk.GetBufferAccessor<Game.Areas.Node>(ref this.m_NodeType);
        // ISSUE: reference to a compiler-generated field
        if (this.m_EditorMode)
        {
          // ISSUE: reference to a compiler-generated field
          NativeParallelHashSet<Entity> startTiles = new NativeParallelHashSet<Entity>(this.m_StartTiles.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_StartTiles.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            startTiles.Add(this.m_StartTiles[index]);
          }
          for (int index1 = 0; index1 < nativeArray.Length; ++index1)
          {
            Entity area = nativeArray[index1];
            bool isNative = !startTiles.Contains(area);
            DynamicBuffer<Game.Areas.Node> dynamicBuffer = bufferAccessor[index1];
            if (dynamicBuffer.Length >= 2)
            {
              Line3.Segment line;
              line.a = dynamicBuffer[dynamicBuffer.Length - 1].m_Position;
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                line.b = dynamicBuffer[index2].m_Position;
                // ISSUE: reference to a compiler-generated method
                this.CheckLine(area, line, isNative, startTiles);
                line.a = line.b;
              }
            }
          }
          startTiles.Dispose();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          bool isNative = chunk.Has<Native>(ref this.m_NativeType);
          for (int index3 = 0; index3 < nativeArray.Length; ++index3)
          {
            Entity area = nativeArray[index3];
            DynamicBuffer<Game.Areas.Node> dynamicBuffer = bufferAccessor[index3];
            if (dynamicBuffer.Length >= 2)
            {
              Line3.Segment line;
              line.a = dynamicBuffer[dynamicBuffer.Length - 1].m_Position;
              for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
              {
                line.b = dynamicBuffer[index4].m_Position;
                // ISSUE: reference to a compiler-generated method
                this.CheckLine(area, line, isNative, new NativeParallelHashSet<Entity>());
                line.a = line.b;
              }
            }
          }
        }
      }

      private void CheckLine(
        Entity area,
        Line3.Segment line,
        bool isNative,
        NativeParallelHashSet<Entity> startTiles)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CityBoundaryMeshSystem.FillBoundaryQueueJob.Iterator iterator = new CityBoundaryMeshSystem.FillBoundaryQueueJob.Iterator()
        {
          m_Line = line.xz,
          m_Area = area,
          m_StartTiles = startTiles,
          m_MapTileData = this.m_MapTileData,
          m_NativeData = this.m_NativeData,
          m_Nodes = this.m_Nodes,
          m_Triangles = this.m_Triangles,
          m_EditorMode = this.m_EditorMode,
          m_IsNative = isNative
        };
        // ISSUE: reference to a compiler-generated field
        this.m_SearchTree.Iterate<CityBoundaryMeshSystem.FillBoundaryQueueJob.Iterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        if (iterator.m_TileFound)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_BoundaryQueue.Enqueue(new CityBoundaryMeshSystem.Boundary()
        {
          m_Line = line,
          m_Color = isNative ? this.m_MapBorderColor : this.m_CityBorderColor
        });
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

      private struct Iterator : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public Line2.Segment m_Line;
        public Entity m_Area;
        public NativeParallelHashSet<Entity> m_StartTiles;
        public ComponentLookup<MapTile> m_MapTileData;
        public ComponentLookup<Native> m_NativeData;
        public BufferLookup<Game.Areas.Node> m_Nodes;
        public BufferLookup<Triangle> m_Triangles;
        public bool m_EditorMode;
        public bool m_IsNative;
        public bool m_TileFound;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(MathUtils.Expand(bounds.m_Bounds.xz, (float2) 1f), this.m_Line, out float2 _);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem areaItem)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(MathUtils.Expand(bounds.m_Bounds.xz, (float2) 1f), this.m_Line, out float2 _) || this.m_Area == areaItem.m_Area || !this.m_MapTileData.HasComponent(areaItem.m_Area))
            return;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_IsNative)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_EditorMode)
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_StartTiles.Contains(areaItem.m_Area))
                return;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_NativeData.HasComponent(areaItem.m_Area))
                return;
            }
          }
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Areas.Node> node = this.m_Nodes[areaItem.m_Area];
          // ISSUE: reference to a compiler-generated field
          Triangle triangle = this.m_Triangles[areaItem.m_Area][areaItem.m_Triangle];
          Triangle2 triangle2 = AreaUtils.GetTriangle2(node, triangle);
          bool3 bool3 = AreaUtils.IsEdge(node, triangle);
          if (bool3.x)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckLine(triangle2.ab);
          }
          if (bool3.y)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckLine(triangle2.bc);
          }
          if (!bool3.z)
            return;
          // ISSUE: reference to a compiler-generated method
          this.CheckLine(triangle2.ca);
        }

        private void CheckLine(Line2.Segment line)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_TileFound = ((this.m_TileFound ? 1 : 0) | ((double) math.distancesq(line.a, this.m_Line.a) >= 1.0 || (double) math.distancesq(line.b, this.m_Line.b) >= 1.0 ? ((double) math.distancesq(line.b, this.m_Line.a) >= 1.0 ? 0 : ((double) math.distancesq(line.a, this.m_Line.b) < 1.0 ? 1 : 0)) : 1)) != 0;
        }
      }
    }

    [BurstCompile]
    private struct FillBoundaryMeshDataJob : IJob
    {
      [ReadOnly]
      public float m_Width;
      [ReadOnly]
      public float m_TilingLength;
      public NativeQueue<CityBoundaryMeshSystem.Boundary> m_BoundaryQueue;
      public NativeList<float3> m_Vertices;
      public NativeList<float2> m_UVs;
      public NativeList<Color32> m_Colors;
      public NativeList<int> m_Indices;
      public NativeValue<Bounds3> m_Bounds;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_BoundaryQueue.Count;
        // ISSUE: reference to a compiler-generated field
        this.m_Vertices.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_UVs.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_Colors.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_Indices.Clear();
        int num1 = 0;
        Bounds3 bounds3 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        for (int index1 = 0; index1 < count; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CityBoundaryMeshSystem.Boundary boundary = this.m_BoundaryQueue.Dequeue();
          // ISSUE: reference to a compiler-generated field
          float num2 = MathUtils.Length(boundary.m_Line);
          if ((double) num2 >= 1.0)
          {
            float3 float3_1 = new float3();
            ref float3 local1 = ref float3_1;
            // ISSUE: reference to a compiler-generated field
            float3 float3_2 = boundary.m_Line.ab;
            // ISSUE: reference to a compiler-generated field
            float2 float2_1 = MathUtils.Right(float3_2.xz) * (this.m_Width * 0.5f / num2);
            local1.xz = float2_1;
            // ISSUE: reference to a compiler-generated field
            int num3 = math.max(1, Mathf.RoundToInt(num2 / this.m_TilingLength));
            float num4 = 1f / (float) num3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            bounds3 = bounds3 | boundary.m_Line.a + float3_1 | boundary.m_Line.a - float3_1 | boundary.m_Line.b + float3_1 | boundary.m_Line.b - float3_1;
            for (int index2 = 0; index2 < num3; ++index2)
            {
              float2 t = new float2((float) index2 + 0.25f, (float) index2 + 0.75f) * num4;
              // ISSUE: reference to a compiler-generated field
              Line3.Segment segment = MathUtils.Cut(boundary.m_Line, t);
              // ISSUE: reference to a compiler-generated field
              this.m_Indices.Add(in num1);
              // ISSUE: reference to a compiler-generated field
              this.m_Indices.Add(num1 + 1);
              // ISSUE: reference to a compiler-generated field
              this.m_Indices.Add(num1 + 2);
              // ISSUE: reference to a compiler-generated field
              this.m_Indices.Add(num1 + 2);
              // ISSUE: reference to a compiler-generated field
              this.m_Indices.Add(num1 + 1);
              // ISSUE: reference to a compiler-generated field
              this.m_Indices.Add(num1 + 3);
              // ISSUE: reference to a compiler-generated field
              ref NativeList<float3> local2 = ref this.m_Vertices;
              float3_2 = segment.a + float3_1;
              ref float3 local3 = ref float3_2;
              local2.Add(in local3);
              // ISSUE: reference to a compiler-generated field
              ref NativeList<float2> local4 = ref this.m_UVs;
              float2 float2_2 = new float2(1f, 0.0f);
              ref float2 local5 = ref float2_2;
              local4.Add(in local5);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_Colors.Add(in boundary.m_Color);
              // ISSUE: reference to a compiler-generated field
              ref NativeList<float3> local6 = ref this.m_Vertices;
              float3_2 = segment.a - float3_1;
              ref float3 local7 = ref float3_2;
              local6.Add(in local7);
              // ISSUE: reference to a compiler-generated field
              ref NativeList<float2> local8 = ref this.m_UVs;
              float2_2 = new float2(0.0f, 0.0f);
              ref float2 local9 = ref float2_2;
              local8.Add(in local9);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_Colors.Add(in boundary.m_Color);
              // ISSUE: reference to a compiler-generated field
              ref NativeList<float3> local10 = ref this.m_Vertices;
              float3_2 = segment.b + float3_1;
              ref float3 local11 = ref float3_2;
              local10.Add(in local11);
              // ISSUE: reference to a compiler-generated field
              ref NativeList<float2> local12 = ref this.m_UVs;
              float2_2 = new float2(1f, 1f);
              ref float2 local13 = ref float2_2;
              local12.Add(in local13);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_Colors.Add(in boundary.m_Color);
              // ISSUE: reference to a compiler-generated field
              ref NativeList<float3> local14 = ref this.m_Vertices;
              float3_2 = segment.b - float3_1;
              ref float3 local15 = ref float3_2;
              local14.Add(in local15);
              // ISSUE: reference to a compiler-generated field
              ref NativeList<float2> local16 = ref this.m_UVs;
              float2_2 = new float2(0.0f, 1f);
              ref float2 local17 = ref float2_2;
              local16.Add(in local17);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_Colors.Add(in boundary.m_Color);
              num1 += 4;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Bounds.value = bounds3;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Native> __Game_Common_Native_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> __Game_Areas_Node_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<MapTile> __Game_Areas_MapTile_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Native> __Game_Common_Native_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_MapTile_RO_ComponentLookup = state.GetComponentLookup<MapTile>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentLookup = state.GetComponentLookup<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
      }
    }
  }
}
