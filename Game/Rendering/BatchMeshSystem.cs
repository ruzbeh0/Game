// Decompiled with JetBrains decompiler
// Type: Game.Rendering.BatchMeshSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.IO.AssetDatabase;
using Colossal.Rendering;
using Game.Prefabs;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class BatchMeshSystem : GameSystemBase
  {
    public const string kDisableMeshLoadingKey = "bh.devtools.disableMeshLoadingKey";
    public const string kForceMeshUnloadingKey = "bh.devtools.forceMeshUnloadingKey";
    public const uint MAX_LOADING_COUNT = 30;
    public const int MIN_BATCH_PRIORITY = -1000000;
    public const int SHAPEBUFFER_ELEMENT_SIZE = 8;
    public const uint SHAPEBUFFER_MEMORY_DEFAULT = 33554432;
    public const uint SHAPEBUFFER_MEMORY_INCREMENT = 8388608;
    public const ulong DEFAULT_MEMORY_BUDGET = 1610612736;
    public const bool DEFAULT_MEMORY_BUDGET_IS_STRICT = false;
    private GeometryAssetLoadingSystem m_GeometryLoadingSystem;
    private BatchManagerSystem m_BatchManagerSystem;
    private PrefabSystem m_PrefabSystem;
    private Mesh m_DefaultObjectMesh;
    private Mesh m_DefaultBaseMesh;
    private Mesh m_DefaultLaneMesh;
    private Mesh m_ZoneBlockMesh;
    private Mesh m_ZoneLodMesh;
    private Mesh m_DefaultEdgeMesh;
    private Mesh m_DefaultNodeMesh;
    private Mesh m_DefaultRoundaboutMesh;
    private List<Mesh> m_GeneratedMeshes;
    private List<int> m_FreeMeshIndices;
    private HashSet<GeometryAsset> m_UnloadGeometryAssets;
    private HashSet<GeometryAsset> m_LoadingGeometries;
    private System.Collections.Generic.Dictionary<Entity, BatchMeshSystem.CacheInfo> m_CachingMeshes;
    private System.Collections.Generic.Dictionary<Entity, BatchMeshSystem.MeshInfo> m_MeshInfos;
    private NativeList<int> m_BatchPriority;
    private NativeList<MeshLoadingState> m_LoadingState;
    private NativeList<BatchMeshSystem.LoadingData> m_LoadingData;
    private NativeList<BatchMeshSystem.LoadingData> m_UnloadingData;
    private NativeList<Entity> m_GenerateMeshEntities;
    private NativeHeapAllocator m_ShapeAllocator;
    private JobHandle m_PriorityDeps;
    private JobHandle m_StateDeps;
    private JobHandle m_GenerateMeshDeps;
    private GraphicsBuffer m_ShapeBuffer;
    private Mesh.MeshDataArray m_GenerateMeshDataArray;
    private int m_ShapeCount;
    private int m_PriorityLimit;
    private bool m_AddMeshes;

    public ulong memoryBudget { get; set; }

    public bool strictMemoryBudget { get; set; }

    public bool enableMeshLoading { get; set; }

    public bool forceMeshUnloading { get; set; }

    public ulong totalSizeInMemory { get; private set; }

    public int loadedMeshCount => this.m_MeshInfos.Count;

    public int loadingRemaining { get; private set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_GeometryLoadingSystem = this.World.GetOrCreateSystemManaged<GeometryAssetLoadingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchManagerSystem = this.World.GetOrCreateSystemManaged<BatchManagerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GeneratedMeshes = new List<Mesh>();
      // ISSUE: reference to a compiler-generated field
      this.m_FreeMeshIndices = new List<int>();
      // ISSUE: reference to a compiler-generated field
      this.m_UnloadGeometryAssets = new HashSet<GeometryAsset>();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadingGeometries = new HashSet<GeometryAsset>();
      // ISSUE: reference to a compiler-generated field
      this.m_CachingMeshes = new System.Collections.Generic.Dictionary<Entity, BatchMeshSystem.CacheInfo>();
      // ISSUE: reference to a compiler-generated field
      this.m_MeshInfos = new System.Collections.Generic.Dictionary<Entity, BatchMeshSystem.MeshInfo>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchPriority = new NativeList<int>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LoadingState = new NativeList<MeshLoadingState>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LoadingData = new NativeList<BatchMeshSystem.LoadingData>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_UnloadingData = new NativeList<BatchMeshSystem.LoadingData>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ShapeAllocator = new NativeHeapAllocator(4194304U, 1U, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ShapeAllocator.Allocate(1U);
      // ISSUE: reference to a compiler-generated method
      this.ResizeShapeBuffer();
      this.memoryBudget = 1610612736UL;
      this.strictMemoryBudget = false;
      this.enableMeshLoading = true;
      this.forceMeshUnloading = false;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_DefaultObjectMesh != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_DefaultObjectMesh);
      }
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_DefaultBaseMesh != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_DefaultBaseMesh);
      }
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_DefaultLaneMesh != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_DefaultLaneMesh);
      }
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_ZoneBlockMesh != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_ZoneBlockMesh);
      }
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_ZoneLodMesh != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_ZoneLodMesh);
      }
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_DefaultEdgeMesh != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_DefaultEdgeMesh);
      }
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_DefaultNodeMesh != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_DefaultNodeMesh);
      }
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_DefaultRoundaboutMesh != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_DefaultRoundaboutMesh);
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_GeneratedMeshes.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_GeneratedMeshes[index]);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_GeneratedMeshes.Clear();
      // ISSUE: reference to a compiler-generated field
      foreach (GeometryAsset loadingGeometry in this.m_LoadingGeometries)
        loadingGeometry.UnloadPartial();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadingGeometries.Clear();
      // ISSUE: reference to a compiler-generated method
      this.UnloadMeshAndGeometryAssets();
      // ISSUE: reference to a compiler-generated field
      this.m_PriorityDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_StateDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      foreach (KeyValuePair<Entity, BatchMeshSystem.CacheInfo> cachingMesh in this.m_CachingMeshes)
      {
        // ISSUE: variable of a compiler-generated type
        BatchMeshSystem.CacheInfo cacheInfo = cachingMesh.Value;
        // ISSUE: reference to a compiler-generated field
        cacheInfo.m_Dependency.Complete();
        cacheInfo = cachingMesh.Value;
        // ISSUE: reference to a compiler-generated field
        cacheInfo.m_CommandBuffer.Dispose();
        // ISSUE: reference to a compiler-generated field
        if ((AssetData) cachingMesh.Value.m_GeometryAsset != (IAssetData) null)
        {
          // ISSUE: reference to a compiler-generated field
          cachingMesh.Value.m_GeometryAsset.UnloadPartial();
        }
      }
      // ISSUE: reference to a compiler-generated field
      foreach (KeyValuePair<Entity, BatchMeshSystem.MeshInfo> meshInfo in this.m_MeshInfos)
      {
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) meshInfo.Value.m_Prefab != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < meshInfo.Value.m_BatchCount; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            meshInfo.Value.m_Prefab.ReleaseMeshes();
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ShapeBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ShapeBuffer.Release();
        // ISSUE: reference to a compiler-generated field
        this.m_ShapeBuffer = (GraphicsBuffer) null;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_GenerateMeshEntities.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_GenerateMeshDeps.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_GenerateMeshEntities.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_GenerateMeshDataArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_BatchPriority.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadingState.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadingData.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_UnloadingData.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ShapeAllocator.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      ulong loadingMemorySize = 0;
      ulong neededMemorySize = 0;
      // ISSUE: reference to a compiler-generated method
      this.LoadMeshes(ref loadingMemorySize, ref neededMemorySize);
      // ISSUE: reference to a compiler-generated method
      this.UnloadMeshes(loadingMemorySize, neededMemorySize);
      // ISSUE: reference to a compiler-generated method
      this.GenerateMeshes();
    }

    public void ReplaceMesh(Entity oldMesh, Entity newMesh)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_GenerateMeshEntities.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_GenerateMeshEntities.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_GenerateMeshEntities[index] == oldMesh)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_GenerateMeshEntities[index] = newMesh;
            break;
          }
        }
      }
      // ISSUE: reference to a compiler-generated method
      this.CompleteCaching(oldMesh);
      // ISSUE: reference to a compiler-generated method
      this.TryCopyBuffer<MeshVertex>(oldMesh, newMesh);
      // ISSUE: reference to a compiler-generated method
      this.TryCopyBuffer<MeshIndex>(oldMesh, newMesh);
      // ISSUE: reference to a compiler-generated method
      this.TryCopyBuffer<MeshNode>(oldMesh, newMesh);
      // ISSUE: reference to a compiler-generated method
      this.TryCopyBuffer<MeshNormal>(oldMesh, newMesh);
      // ISSUE: variable of a compiler-generated type
      BatchMeshSystem.MeshInfo meshInfo;
      // ISSUE: reference to a compiler-generated field
      if (this.m_MeshInfos.TryGetValue(oldMesh, out meshInfo))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        meshInfo.m_Prefab = this.m_PrefabSystem.GetPrefab<RenderPrefab>(newMesh);
        // ISSUE: reference to a compiler-generated field
        this.m_MeshInfos.Add(newMesh, meshInfo);
        // ISSUE: reference to a compiler-generated field
        this.m_MeshInfos.Remove(oldMesh);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ManagedBatches<OptionalProperties> managedBatches = this.m_BatchManagerSystem.GetManagedBatches();
      int batchCount = managedBatches.BatchCount;
      for (int batchIndex = 0; batchIndex < batchCount; ++batchIndex)
        ((CustomBatch) managedBatches.GetBatch(batchIndex))?.ReplaceMesh(oldMesh, newMesh);
    }

    private void TryCopyBuffer<T>(Entity source, Entity target) where T : unmanaged, IBufferElementData
    {
      if (!this.EntityManager.HasBuffer<T>(source))
        return;
      this.EntityManager.AddBuffer<T>(target).CopyFrom(this.EntityManager.GetBuffer<T>(source));
    }

    public Mesh GetDefaultMesh(MeshType type, BatchFlags flags, GeneratedType generatedType)
    {
      switch (type)
      {
        case MeshType.Object:
          if (generatedType == GeneratedType.ObjectBase)
          {
            // ISSUE: reference to a compiler-generated field
            if ((UnityEngine.Object) this.m_DefaultBaseMesh == (UnityEngine.Object) null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_DefaultBaseMesh = ObjectMeshHelpers.CreateDefaultBaseMesh();
            }
            // ISSUE: reference to a compiler-generated field
            return this.m_DefaultBaseMesh;
          }
          // ISSUE: reference to a compiler-generated field
          if ((UnityEngine.Object) this.m_DefaultObjectMesh == (UnityEngine.Object) null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_DefaultObjectMesh = ObjectMeshHelpers.CreateDefaultMesh();
          }
          // ISSUE: reference to a compiler-generated field
          return this.m_DefaultObjectMesh;
        case MeshType.Net:
          if ((flags & BatchFlags.Roundabout) != (BatchFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if ((UnityEngine.Object) this.m_DefaultRoundaboutMesh == (UnityEngine.Object) null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_DefaultRoundaboutMesh = NetMeshHelpers.CreateDefaultRoundaboutMesh();
            }
            // ISSUE: reference to a compiler-generated field
            return this.m_DefaultRoundaboutMesh;
          }
          if ((flags & BatchFlags.Node) != (BatchFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if ((UnityEngine.Object) this.m_DefaultNodeMesh == (UnityEngine.Object) null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_DefaultNodeMesh = NetMeshHelpers.CreateDefaultNodeMesh();
            }
            // ISSUE: reference to a compiler-generated field
            return this.m_DefaultNodeMesh;
          }
          // ISSUE: reference to a compiler-generated field
          if ((UnityEngine.Object) this.m_DefaultEdgeMesh == (UnityEngine.Object) null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_DefaultEdgeMesh = NetMeshHelpers.CreateDefaultEdgeMesh();
          }
          // ISSUE: reference to a compiler-generated field
          return this.m_DefaultEdgeMesh;
        case MeshType.Lane:
          // ISSUE: reference to a compiler-generated field
          if ((UnityEngine.Object) this.m_DefaultLaneMesh == (UnityEngine.Object) null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_DefaultLaneMesh = NetMeshHelpers.CreateDefaultLaneMesh();
          }
          // ISSUE: reference to a compiler-generated field
          return this.m_DefaultLaneMesh;
        case MeshType.Zone:
          if ((flags & BatchFlags.Lod) != (BatchFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if ((UnityEngine.Object) this.m_ZoneLodMesh == (UnityEngine.Object) null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ZoneLodMesh = ZoneMeshHelpers.CreateMesh(new int2(5, 3), (int2) 2);
            }
            // ISSUE: reference to a compiler-generated field
            return this.m_ZoneLodMesh;
          }
          // ISSUE: reference to a compiler-generated field
          if ((UnityEngine.Object) this.m_ZoneBlockMesh == (UnityEngine.Object) null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ZoneBlockMesh = ZoneMeshHelpers.CreateMesh(new int2(10, 6), (int2) 1);
          }
          // ISSUE: reference to a compiler-generated field
          return this.m_ZoneBlockMesh;
        default:
          return (Mesh) null;
      }
    }

    public NativeList<int> GetBatchPriority(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_PriorityDeps;
      // ISSUE: reference to a compiler-generated field
      return this.m_BatchPriority;
    }

    public NativeList<MeshLoadingState> GetLoadingState(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_StateDeps;
      // ISSUE: reference to a compiler-generated field
      return this.m_LoadingState;
    }

    public void AddBatchPriorityWriter(JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PriorityDeps = dependencies;
    }

    public void AddLoadingStateReader(JobHandle dependencies) => this.m_StateDeps = dependencies;

    public void UpdateMeshes()
    {
      // ISSUE: reference to a compiler-generated method
      this.AddMeshes();
      // ISSUE: reference to a compiler-generated method
      this.UpdateMeshesForAddedInstances();
      // ISSUE: reference to a compiler-generated method
      this.UnloadMeshAndGeometryAssets();
    }

    public void CompleteMeshes()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_GenerateMeshEntities.IsCreated)
        return;
      // ISSUE: reference to a compiler-generated field
      Mesh[] meshes = new Mesh[this.m_GenerateMeshEntities.Length];
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_GenerateMeshEntities.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity generateMeshEntity = this.m_GenerateMeshEntities[index];
        // ISSUE: variable of a compiler-generated type
        BatchMeshSystem.MeshInfo meshInfo;
        // ISSUE: reference to a compiler-generated field
        if (this.m_MeshInfos.TryGetValue(generateMeshEntity, out meshInfo))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Mesh generatedMesh = this.m_GeneratedMeshes[meshInfo.m_GeneratedIndex];
          Bounds bounds = new Bounds();
          Game.Prefabs.MeshData component1;
          if (this.EntityManager.TryGetComponent<Game.Prefabs.MeshData>(generateMeshEntity, out component1))
          {
            bounds.SetMinMax((Vector3) component1.m_Bounds.min, (Vector3) component1.m_Bounds.max);
          }
          else
          {
            NetCompositionMeshData component2;
            if (this.EntityManager.TryGetComponent<NetCompositionMeshData>(generateMeshEntity, out component2))
              bounds.SetMinMax((Vector3) (new float3(component2.m_Width * -0.5f, component2.m_HeightRange.min, component2.m_Width * -0.5f) - 500f), (Vector3) (new float3(component2.m_Width * 0.5f, component2.m_HeightRange.max, component2.m_Width * 0.5f) + 500f));
          }
          generatedMesh.bounds = bounds;
          meshes[index] = generatedMesh;
        }
        else
          meshes[index] = new Mesh();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_GenerateMeshDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      Mesh.ApplyAndDisposeWritableMeshData(this.m_GenerateMeshDataArray, meshes, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_GenerateMeshEntities.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_MeshInfos.ContainsKey(this.m_GenerateMeshEntities[index]))
          meshes[index].UploadMeshData(true);
        else
          UnityEngine.Object.Destroy((UnityEngine.Object) meshes[index]);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_GenerateMeshEntities.Dispose();
    }

    public void UpdateBatchPriorities()
    {
      if (!this.enableMeshLoading)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new BatchMeshSystem.LoadingPriorityJob()
      {
        m_PriorityLimit = this.m_PriorityLimit,
        m_BatchPriority = this.m_BatchPriority,
        m_LoadingState = this.m_LoadingState,
        m_LoadingData = this.m_LoadingData,
        m_UnloadingData = this.m_UnloadingData
      }.Schedule<BatchMeshSystem.LoadingPriorityJob>(JobHandle.CombineDependencies(this.m_PriorityDeps, this.m_StateDeps));
      // ISSUE: reference to a compiler-generated field
      this.m_PriorityDeps = jobHandle;
      // ISSUE: reference to a compiler-generated field
      this.m_StateDeps = jobHandle;
    }

    public void CompleteCaching()
    {
      List<Entity> entityList = (List<Entity>) null;
      // ISSUE: reference to a compiler-generated field
      foreach (KeyValuePair<Entity, BatchMeshSystem.CacheInfo> cachingMesh in this.m_CachingMeshes)
      {
        // ISSUE: variable of a compiler-generated type
        BatchMeshSystem.CacheInfo cacheInfo = cachingMesh.Value;
        // ISSUE: reference to a compiler-generated field
        if (cacheInfo.m_Dependency.IsCompleted)
        {
          // ISSUE: reference to a compiler-generated method
          this.CompleteCaching(cachingMesh.Key, cachingMesh.Value);
          if (entityList == null)
            entityList = new List<Entity>();
          entityList.Add(cachingMesh.Key);
        }
      }
      if (entityList == null)
        return;
      foreach (Entity key in entityList)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CachingMeshes.Remove(key);
      }
    }

    private void CompleteCaching(Entity entity)
    {
      // ISSUE: variable of a compiler-generated type
      BatchMeshSystem.CacheInfo cacheInfo;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CachingMeshes.TryGetValue(entity, out cacheInfo))
        return;
      // ISSUE: reference to a compiler-generated method
      this.CompleteCaching(entity, cacheInfo);
      // ISSUE: reference to a compiler-generated field
      this.m_CachingMeshes.Remove(entity);
    }

    private void CompleteCaching(Entity entity, BatchMeshSystem.CacheInfo cacheInfo)
    {
      // ISSUE: reference to a compiler-generated field
      cacheInfo.m_Dependency.Complete();
      if (this.EntityManager.Exists(entity))
      {
        // ISSUE: reference to a compiler-generated field
        cacheInfo.m_CommandBuffer.Playback(this.EntityManager);
      }
      // ISSUE: reference to a compiler-generated field
      cacheInfo.m_CommandBuffer.Dispose();
      // ISSUE: reference to a compiler-generated field
      if (!((AssetData) cacheInfo.m_GeometryAsset != (IAssetData) null))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UnloadGeometryAssets.Add(cacheInfo.m_GeometryAsset);
    }

    private void AddCaching(Entity entity, BatchMeshSystem.CacheInfo cacheInfo)
    {
      // ISSUE: reference to a compiler-generated method
      this.CompleteCaching(entity);
      // ISSUE: reference to a compiler-generated field
      this.m_CachingMeshes.Add(entity, cacheInfo);
    }

    public void AddBatch(CustomBatch batch, int batchIndex)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PriorityDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      while (this.m_BatchPriority.Length <= batchIndex)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BatchPriority.Add(-1000000);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_BatchPriority[batchIndex] = -1000000;
      // ISSUE: reference to a compiler-generated field
      this.m_StateDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      while (this.m_LoadingState.Length <= batchIndex)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LoadingState.Add(MeshLoadingState.None);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_LoadingState[batchIndex] = MeshLoadingState.None;
      if ((batch.sourceType & (MeshType.Net | MeshType.Zone)) == (MeshType) 0)
      {
        if ((this.EntityManager.GetComponentData<Game.Prefabs.MeshData>(batch.sourceMeshEntity).m_State & MeshFlags.Default) == (MeshFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_LoadingState[batchIndex] = MeshLoadingState.Default;
      }
      else
      {
        if ((batch.sourceType & MeshType.Net) == (MeshType) 0 || (this.EntityManager.GetComponentData<NetCompositionMeshData>(batch.sourceMeshEntity).m_State & MeshFlags.Default) == (MeshFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_LoadingState[batchIndex] = MeshLoadingState.Default;
      }
    }

    public void RemoveBatch(CustomBatch batch, int batchIndex)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_StateDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      MeshLoadingState meshLoadingState = this.m_LoadingState[batchIndex];
      // ISSUE: variable of a compiler-generated type
      BatchMeshSystem.MeshInfo meshInfo;
      // ISSUE: reference to a compiler-generated field
      if ((meshLoadingState == MeshLoadingState.Copying || meshLoadingState == MeshLoadingState.Complete || meshLoadingState == MeshLoadingState.Obsolete) && this.m_MeshInfos.TryGetValue(batch.sharedMeshEntity, out meshInfo))
      {
        // ISSUE: reference to a compiler-generated field
        if (meshInfo.m_BatchCount > 1)
        {
          // ISSUE: reference to a compiler-generated field
          --meshInfo.m_BatchCount;
          // ISSUE: reference to a compiler-generated field
          this.m_MeshInfos[batch.sharedMeshEntity] = meshInfo;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (meshInfo.m_GeneratedIndex >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UnityEngine.Object.Destroy((UnityEngine.Object) this.m_GeneratedMeshes[meshInfo.m_GeneratedIndex]);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_GeneratedMeshes[meshInfo.m_GeneratedIndex] = (Mesh) null;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (meshInfo.m_GeneratedIndex == this.m_GeneratedMeshes.Count - 1)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_GeneratedMeshes.RemoveAt(meshInfo.m_GeneratedIndex);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_FreeMeshIndices.Add(meshInfo.m_GeneratedIndex);
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.RemoveShapeData(meshInfo.m_ShapeAllocations);
          // ISSUE: reference to a compiler-generated method
          this.UncacheMeshData(batch.sharedMeshEntity, batch.sourceType);
          // ISSUE: reference to a compiler-generated field
          this.totalSizeInMemory -= meshInfo.m_SizeInMemory;
          // ISSUE: reference to a compiler-generated field
          this.m_MeshInfos.Remove(batch.sharedMeshEntity);
        }
        // ISSUE: reference to a compiler-generated field
        if (batch.generatedType == GeneratedType.None && (UnityEngine.Object) meshInfo.m_Prefab != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          meshInfo.m_Prefab.ReleaseMeshes();
        }
      }
      if (meshLoadingState == MeshLoadingState.Pending || meshLoadingState == MeshLoadingState.Loading || meshLoadingState == MeshLoadingState.Copying)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_LoadingData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_LoadingData.ElementAt(index).m_BatchIndex == batchIndex)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LoadingData.RemoveAt(index);
            break;
          }
        }
      }
      if (meshLoadingState == MeshLoadingState.Obsolete)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_UnloadingData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_UnloadingData.ElementAt(index).m_BatchIndex == batchIndex)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_UnloadingData.RemoveAt(index);
            break;
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (batchIndex == this.m_LoadingState.Length - 1)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LoadingState.RemoveAt(batchIndex);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LoadingState[batchIndex] = MeshLoadingState.None;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_PriorityDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      if (batchIndex == this.m_BatchPriority.Length - 1)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BatchPriority.RemoveAt(batchIndex);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BatchPriority[batchIndex] = -1000000;
      }
    }

    private void LoadMeshes(ref ulong loadingMemorySize, ref ulong neededMemorySize)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PriorityLimit = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_StateDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      this.loadingRemaining = this.m_LoadingData.Length;
      if (this.loadingRemaining == 0)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ManagedBatches<OptionalProperties> managedBatches = this.m_BatchManagerSystem.GetManagedBatches();
      int num1 = 0;
      // ISSUE: reference to a compiler-generated field
      foreach (GeometryAsset loadingGeometry in this.m_LoadingGeometries)
      {
        if (!loadingGeometry.loading.m_AsyncLoadingDone)
          ++num1;
      }
      // ISSUE: reference to a compiler-generated field
      for (int index1 = 0; index1 < this.m_LoadingData.Length; ++index1)
      {
        // ISSUE: reference to a compiler-generated field
        ref BatchMeshSystem.LoadingData local1 = ref this.m_LoadingData.ElementAt(index1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ref MeshLoadingState local2 = ref this.m_LoadingState.ElementAt(local1.m_BatchIndex);
        if (local2 == MeshLoadingState.Pending || local2 == MeshLoadingState.Loading)
        {
          // ISSUE: reference to a compiler-generated field
          CustomBatch batch = (CustomBatch) managedBatches.GetBatch(local1.m_BatchIndex);
          // ISSUE: reference to a compiler-generated field
          if (!this.m_CachingMeshes.ContainsKey(batch.sharedMeshEntity))
          {
            // ISSUE: variable of a compiler-generated type
            BatchMeshSystem.MeshInfo meshInfo1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_MeshInfos.TryGetValue(batch.sharedMeshEntity, out meshInfo1))
            {
              // ISSUE: reference to a compiler-generated field
              ++meshInfo1.m_BatchCount;
              // ISSUE: reference to a compiler-generated field
              this.m_MeshInfos[batch.sharedMeshEntity] = meshInfo1;
              local2 = MeshLoadingState.Copying;
              // ISSUE: reference to a compiler-generated field
              this.m_AddMeshes = true;
              if ((batch.sourceFlags & BatchFlags.BlendWeights) != (BatchFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.SetShapeParameters(batch, meshInfo1.m_ShapeAllocations);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              meshInfo1.m_Prefab = (RenderPrefab) null;
              // ISSUE: reference to a compiler-generated field
              meshInfo1.m_ShapeAllocations = (BatchMeshSystem.ShapeAllocation[]) null;
              // ISSUE: reference to a compiler-generated field
              meshInfo1.m_SizeInMemory = 0UL;
              // ISSUE: reference to a compiler-generated field
              meshInfo1.m_GeneratedIndex = -1;
              // ISSUE: reference to a compiler-generated field
              meshInfo1.m_BatchCount = 1;
              bool flag1 = false;
              bool flag2 = false;
              string str = (string) null;
              DynamicBuffer<NetCompositionPiece> buffer;
              EntityManager entityManager;
              if (this.EntityManager.TryGetBuffer<NetCompositionPiece>(batch.sharedMeshEntity, true, out buffer))
              {
                flag2 = true;
                if (local2 == MeshLoadingState.Pending)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PriorityLimit > local1.m_Priority)
                  {
                    --this.loadingRemaining;
                    continue;
                  }
                  ulong num2 = 0;
                  for (int index2 = 0; index2 < buffer.Length; ++index2)
                  {
                    NetCompositionPiece compositionPiece = buffer[index2];
                    // ISSUE: object of a compiler-generated type is created
                    // ISSUE: variable of a compiler-generated type
                    BatchMeshSystem.MeshInfo meshInfo2 = new BatchMeshSystem.MeshInfo();
                    entityManager = this.EntityManager;
                    if (entityManager.HasComponent<MeshVertex>(compositionPiece.m_Piece))
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_MeshInfos.TryGetValue(compositionPiece.m_Piece, out meshInfo2))
                      {
                        // ISSUE: reference to a compiler-generated field
                        num2 += meshInfo2.m_SizeInMemory;
                      }
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_MeshInfos.TryGetValue(compositionPiece.m_Piece, out meshInfo2))
                      {
                        // ISSUE: reference to a compiler-generated field
                        num2 += meshInfo2.m_SizeInMemory;
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        // ISSUE: reference to a compiler-generated method
                        ulong num3 = this.EstimateSizeInMemory(this.m_PrefabSystem.GetPrefab<RenderPrefab>(compositionPiece.m_Piece));
                        num2 += num3;
                        // ISSUE: reference to a compiler-generated field
                        if (!this.m_CachingMeshes.ContainsKey(compositionPiece.m_Piece))
                          num2 += num3;
                      }
                    }
                  }
                  if (this.strictMemoryBudget && this.totalSizeInMemory + loadingMemorySize + num2 > this.memoryBudget)
                  {
                    neededMemorySize += num2;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_PriorityLimit = local1.m_Priority;
                    --this.loadingRemaining;
                    continue;
                  }
                  local2 = MeshLoadingState.Loading;
                }
                for (int index3 = 0; index3 < buffer.Length; ++index3)
                {
                  NetCompositionPiece compositionPiece = buffer[index3];
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  BatchMeshSystem.MeshInfo meshInfo3 = new BatchMeshSystem.MeshInfo();
                  entityManager = this.EntityManager;
                  if (entityManager.HasComponent<MeshVertex>(compositionPiece.m_Piece))
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_MeshInfos.TryGetValue(compositionPiece.m_Piece, out meshInfo3))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      meshInfo1.m_SizeInMemory += meshInfo3.m_SizeInMemory;
                    }
                  }
                  else
                  {
                    flag1 = true;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_MeshInfos.TryGetValue(compositionPiece.m_Piece, out meshInfo3))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      meshInfo1.m_SizeInMemory += meshInfo3.m_SizeInMemory;
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated method
                      RenderPrefab prefab = this.m_PrefabSystem.GetPrefab<RenderPrefab>(compositionPiece.m_Piece);
                      // ISSUE: reference to a compiler-generated field
                      meshInfo3.m_Prefab = prefab;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated method
                      meshInfo3.m_SizeInMemory = this.EstimateSizeInMemory(prefab);
                      // ISSUE: reference to a compiler-generated field
                      meshInfo3.m_GeneratedIndex = -1;
                      // ISSUE: reference to a compiler-generated field
                      meshInfo3.m_BatchCount = 0;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      meshInfo1.m_SizeInMemory += meshInfo3.m_SizeInMemory;
                      // ISSUE: reference to a compiler-generated field
                      if (!this.m_CachingMeshes.ContainsKey(compositionPiece.m_Piece))
                      {
                        uint mask = 23;
                        GeometryAsset geometryAsset = prefab.geometryAsset;
                        if ((AssetData) geometryAsset != (IAssetData) null)
                        {
                          // ISSUE: reference to a compiler-generated field
                          if (this.m_LoadingGeometries.Contains(geometryAsset))
                          {
                            if (geometryAsset.loading.m_AsyncLoadingDone)
                            {
                              try
                              {
                                // ISSUE: reference to a compiler-generated method
                                this.CacheMeshData(prefab, geometryAsset, compositionPiece.m_Piece, batch.sourceType);
                              }
                              catch (Exception ex)
                              {
                                Debug.LogError((object) ("Error when accessing mesh data (" + prefab.name + "):\n" + ex.Message), (UnityEngine.Object) prefab);
                              }
                              // ISSUE: reference to a compiler-generated field
                              // ISSUE: reference to a compiler-generated field
                              meshInfo1.m_SizeInMemory -= meshInfo3.m_SizeInMemory;
                              // ISSUE: reference to a compiler-generated field
                              // ISSUE: reference to a compiler-generated method
                              meshInfo3.m_SizeInMemory = this.GetSizeInMemory(geometryAsset);
                              // ISSUE: reference to a compiler-generated field
                              // ISSUE: reference to a compiler-generated field
                              meshInfo1.m_SizeInMemory += meshInfo3.m_SizeInMemory;
                              // ISSUE: reference to a compiler-generated field
                              this.m_LoadingGeometries.Remove(geometryAsset);
                            }
                            else
                            {
                              // ISSUE: reference to a compiler-generated field
                              loadingMemorySize += meshInfo3.m_SizeInMemory;
                              continue;
                            }
                          }
                          else
                          {
                            if (num1 < 30)
                            {
                              // ISSUE: reference to a compiler-generated field
                              this.m_LoadingGeometries.Add(geometryAsset);
                              // ISSUE: reference to a compiler-generated field
                              geometryAsset.RequestDataAsync(this.m_GeometryLoadingSystem, mask);
                              ++num1;
                            }
                            // ISSUE: reference to a compiler-generated field
                            loadingMemorySize += meshInfo3.m_SizeInMemory;
                            continue;
                          }
                        }
                        // ISSUE: reference to a compiler-generated field
                        if (!this.m_CachingMeshes.ContainsKey(compositionPiece.m_Piece) && (AssetData) geometryAsset != (IAssetData) null)
                        {
                          // ISSUE: reference to a compiler-generated field
                          this.m_UnloadGeometryAssets.Add(geometryAsset);
                        }
                        // ISSUE: reference to a compiler-generated field
                        this.totalSizeInMemory += meshInfo3.m_SizeInMemory;
                        // ISSUE: reference to a compiler-generated field
                        this.m_MeshInfos.Add(compositionPiece.m_Piece, meshInfo3);
                      }
                    }
                  }
                }
                if (flag1)
                {
                  // ISSUE: reference to a compiler-generated field
                  loadingMemorySize += meshInfo1.m_SizeInMemory;
                  continue;
                }
                for (int index4 = 0; index4 < buffer.Length; ++index4)
                {
                  NetCompositionPiece compositionPiece = buffer[index4];
                  // ISSUE: variable of a compiler-generated type
                  BatchMeshSystem.MeshInfo meshInfo4;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_MeshInfos.TryGetValue(compositionPiece.m_Piece, out meshInfo4))
                  {
                    // ISSUE: reference to a compiler-generated field
                    ++meshInfo4.m_BatchCount;
                    // ISSUE: reference to a compiler-generated field
                    this.m_MeshInfos[compositionPiece.m_Piece] = meshInfo4;
                  }
                }
                if (flag2)
                  str = string.Format("Net composition {0}", (object) batch.sharedMeshEntity.Index);
              }
              else
              {
                RenderPrefab prefab;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                if (this.m_PrefabSystem.TryGetPrefab<RenderPrefab>(batch.sharedMeshEntity, out prefab) && (UnityEngine.Object) prefab != (UnityEngine.Object) null)
                {
                  // ISSUE: reference to a compiler-generated field
                  meshInfo1.m_Prefab = prefab;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  meshInfo1.m_SizeInMemory = this.EstimateSizeInMemory(prefab);
                  GeometryAsset geometryAsset = prefab.geometryAsset;
                  entityManager = this.EntityManager;
                  Game.Prefabs.MeshData componentData = entityManager.GetComponentData<Game.Prefabs.MeshData>(batch.sharedMeshEntity);
                  entityManager = this.EntityManager;
                  bool flag3 = !entityManager.HasComponent<MeshVertex>(batch.sharedMeshEntity);
                  flag2 = (componentData.m_State & MeshFlags.Base) > (MeshFlags) 0;
                  if (flag3)
                  {
                    if ((AssetData) geometryAsset != (IAssetData) null)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_LoadingGeometries.Contains(geometryAsset))
                      {
                        if (geometryAsset.loading.m_AsyncLoadingDone)
                        {
                          try
                          {
                            // ISSUE: reference to a compiler-generated method
                            this.CacheMeshData(prefab, geometryAsset, batch.sharedMeshEntity, batch.sourceType);
                          }
                          catch (Exception ex)
                          {
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            Debug.LogError((object) ("Error when accessing mesh data (" + meshInfo1.m_Prefab.name + "):\n" + ex.Message), (UnityEngine.Object) meshInfo1.m_Prefab);
                          }
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated method
                          meshInfo1.m_SizeInMemory = this.GetSizeInMemory(geometryAsset);
                          // ISSUE: reference to a compiler-generated field
                          this.m_LoadingGeometries.Remove(geometryAsset);
                        }
                        else
                        {
                          // ISSUE: reference to a compiler-generated field
                          loadingMemorySize += meshInfo1.m_SizeInMemory;
                          continue;
                        }
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_PriorityLimit > local1.m_Priority)
                        {
                          --this.loadingRemaining;
                          continue;
                        }
                        // ISSUE: reference to a compiler-generated field
                        if (this.strictMemoryBudget && this.totalSizeInMemory + loadingMemorySize + meshInfo1.m_SizeInMemory > this.memoryBudget)
                        {
                          // ISSUE: reference to a compiler-generated field
                          neededMemorySize += meshInfo1.m_SizeInMemory;
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          this.m_PriorityLimit = local1.m_Priority;
                          --this.loadingRemaining;
                          continue;
                        }
                        if (num1 < 30)
                        {
                          // ISSUE: reference to a compiler-generated field
                          loadingMemorySize += meshInfo1.m_SizeInMemory;
                          // ISSUE: reference to a compiler-generated field
                          this.m_LoadingGeometries.Add(geometryAsset);
                          // ISSUE: reference to a compiler-generated field
                          geometryAsset.RequestDataAsync(this.m_GeometryLoadingSystem, 16383U);
                          local2 = MeshLoadingState.Loading;
                          ++num1;
                          continue;
                        }
                        continue;
                      }
                    }
                  }
                  else if ((AssetData) geometryAsset != (IAssetData) null)
                  {
                    if (geometryAsset.data.attrData.IsCreated)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated method
                      meshInfo1.m_SizeInMemory = this.GetSizeInMemory(geometryAsset);
                    }
                    else
                      Debug.Log((object) ("Geometry asset not loaded: " + geometryAsset.name));
                  }
                  if (flag2 & flag3)
                  {
                    // ISSUE: reference to a compiler-generated field
                    loadingMemorySize += meshInfo1.m_SizeInMemory;
                    continue;
                  }
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_CachingMeshes.ContainsKey(batch.sharedMeshEntity) && (AssetData) geometryAsset != (IAssetData) null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_UnloadGeometryAssets.Add(geometryAsset);
                  }
                  if (flag2 && (AssetData) geometryAsset != (IAssetData) null)
                    str = "Generated base (" + geometryAsset.name + ")";
                }
              }
              if (flag2)
              {
                Mesh mesh = new Mesh();
                mesh.name = str;
                // ISSUE: reference to a compiler-generated field
                if (this.m_FreeMeshIndices.Count != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  meshInfo1.m_GeneratedIndex = this.m_FreeMeshIndices[this.m_FreeMeshIndices.Count - 1];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_FreeMeshIndices.RemoveAt(this.m_FreeMeshIndices.Count - 1);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_GeneratedMeshes[meshInfo1.m_GeneratedIndex] = mesh;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  meshInfo1.m_GeneratedIndex = this.m_GeneratedMeshes.Count;
                  // ISSUE: reference to a compiler-generated field
                  this.m_GeneratedMeshes.Add(mesh);
                }
                // ISSUE: reference to a compiler-generated field
                if (!this.m_GenerateMeshEntities.IsCreated)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_GenerateMeshEntities = new NativeList<Entity>(30, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_GenerateMeshEntities.Add(batch.sharedMeshEntity);
              }
              if ((batch.sourceFlags & BatchFlags.BlendWeights) != (BatchFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                meshInfo1.m_ShapeAllocations = this.AddShapeData(meshInfo1.m_Prefab);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.SetShapeParameters(batch, meshInfo1.m_ShapeAllocations);
              }
              // ISSUE: reference to a compiler-generated field
              this.totalSizeInMemory += meshInfo1.m_SizeInMemory;
              // ISSUE: reference to a compiler-generated field
              this.m_MeshInfos.Add(batch.sharedMeshEntity, meshInfo1);
              local2 = MeshLoadingState.Copying;
              // ISSUE: reference to a compiler-generated field
              this.m_AddMeshes = true;
            }
          }
        }
      }
    }

    private ulong EstimateSizeInMemory(RenderPrefab meshPrefab)
    {
      int indexCount = meshPrefab.indexCount;
      int vertexCount = meshPrefab.vertexCount;
      int num1 = vertexCount > 65536 ? 4 : 2;
      int num2 = 32;
      return (ulong) ((long) indexCount * (long) num1 + (long) vertexCount * (long) num2);
    }

    private ulong GetSizeInMemory(GeometryAsset geometryAsset)
    {
      return (ulong) geometryAsset.data.attrData.Length + (ulong) geometryAsset.data.indexData.Length;
    }

    private void AddMeshes()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_AddMeshes)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_AddMeshes = false;
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> nativeBatchGroups = this.m_BatchManagerSystem.GetNativeBatchGroups(false, out dependencies);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ManagedBatches<OptionalProperties> managedBatches = this.m_BatchManagerSystem.GetManagedBatches();
      dependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_LoadingData.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        ref BatchMeshSystem.LoadingData local1 = ref this.m_LoadingData.ElementAt(index);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ref MeshLoadingState local2 = ref this.m_LoadingState.ElementAt(local1.m_BatchIndex);
        if (local2 == MeshLoadingState.Copying)
        {
          // ISSUE: reference to a compiler-generated field
          CustomBatch batch = (CustomBatch) managedBatches.GetBatch(local1.m_BatchIndex);
          try
          {
            // ISSUE: variable of a compiler-generated type
            BatchMeshSystem.MeshInfo meshInfo;
            // ISSUE: reference to a compiler-generated field
            if (this.m_MeshInfos.TryGetValue(batch.sharedMeshEntity, out meshInfo))
            {
              if (batch.generatedType != GeneratedType.None)
              {
                // ISSUE: reference to a compiler-generated field
                if (meshInfo.m_GeneratedIndex >= 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  managedBatches.SetMesh<CullingData, GroupData, BatchData, InstanceData>(local1.m_BatchIndex, this.m_GeneratedMeshes[meshInfo.m_GeneratedIndex], batch.sourceSubMeshIndex, nativeBatchGroups);
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if ((UnityEngine.Object) meshInfo.m_Prefab != (UnityEngine.Object) null)
                {
                  int subMeshIndex;
                  // ISSUE: reference to a compiler-generated field
                  Mesh mesh = meshInfo.m_Prefab.ObtainMesh(batch.sourceSubMeshIndex, out subMeshIndex);
                  // ISSUE: reference to a compiler-generated field
                  managedBatches.SetMesh<CullingData, GroupData, BatchData, InstanceData>(local1.m_BatchIndex, mesh, subMeshIndex, nativeBatchGroups);
                }
              }
            }
            if ((UnityEngine.Object) batch.defaultMaterial != (UnityEngine.Object) batch.loadedMaterial)
            {
              // ISSUE: reference to a compiler-generated field
              managedBatches.SetMaterial<CullingData, GroupData, BatchData, InstanceData>(local1.m_BatchIndex, batch.loadedMaterial, nativeBatchGroups);
            }
          }
          catch (Exception ex)
          {
            // ISSUE: variable of a compiler-generated type
            BatchMeshSystem.MeshInfo meshInfo;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_MeshInfos.TryGetValue(batch.sharedMeshEntity, out meshInfo) && (UnityEngine.Object) meshInfo.m_Prefab != (UnityEngine.Object) null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              COSystemBase.baseLog.ErrorFormat((UnityEngine.Object) meshInfo.m_Prefab, ex, "Error when setting mesh for {0}", (object) meshInfo.m_Prefab.name);
            }
            else
              COSystemBase.baseLog.ErrorFormat(ex, "Error when setting mesh for {0}", (object) batch.sourceMeshEntity);
          }
          local2 = MeshLoadingState.Complete;
          // ISSUE: reference to a compiler-generated field
          this.m_LoadingData.RemoveAtSwapBack(index--);
        }
      }
    }

    private void UpdateMeshesForAddedInstances()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_StateDeps.Complete();
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> nativeBatchGroups = this.m_BatchManagerSystem.GetNativeBatchGroups(false, out dependencies1);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> nativeBatchInstances = this.m_BatchManagerSystem.GetNativeBatchInstances(false, out dependencies2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ManagedBatches<OptionalProperties> managedBatches = this.m_BatchManagerSystem.GetManagedBatches();
      dependencies1.Complete();
      dependencies2.Complete();
      AddedInstanceEnumerator addedInstances = nativeBatchInstances.GetAddedInstances();
      int groupIndex;
      while (addedInstances.GetNextUpdatedGroup(out groupIndex))
      {
        NativeBatchAccessor<BatchData> batchAccessor = nativeBatchGroups.GetBatchAccessor(groupIndex);
        for (int index = 0; index < batchAccessor.Length; ++index)
        {
          int managedBatchIndex = batchAccessor.GetManagedBatchIndex(index);
          if (managedBatchIndex >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            ref MeshLoadingState local = ref this.m_LoadingState.ElementAt(managedBatchIndex);
            if (local == MeshLoadingState.None)
            {
              CustomBatch batch = (CustomBatch) managedBatches.GetBatch(managedBatchIndex);
              // ISSUE: variable of a compiler-generated type
              BatchMeshSystem.MeshInfo meshInfo;
              // ISSUE: reference to a compiler-generated field
              if (this.m_MeshInfos.TryGetValue(batch.sharedMeshEntity, out meshInfo))
              {
                // ISSUE: reference to a compiler-generated field
                ++meshInfo.m_BatchCount;
                // ISSUE: reference to a compiler-generated field
                this.m_MeshInfos[batch.sharedMeshEntity] = meshInfo;
                if ((batch.sourceFlags & BatchFlags.BlendWeights) != (BatchFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.SetShapeParameters(batch, meshInfo.m_ShapeAllocations);
                }
                try
                {
                  if (batch.generatedType != GeneratedType.None)
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (meshInfo.m_GeneratedIndex >= 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      managedBatches.SetMesh<CullingData, GroupData, BatchData, InstanceData>(managedBatchIndex, this.m_GeneratedMeshes[meshInfo.m_GeneratedIndex], batch.sourceSubMeshIndex, nativeBatchGroups);
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if ((UnityEngine.Object) meshInfo.m_Prefab != (UnityEngine.Object) null)
                    {
                      int subMeshIndex;
                      // ISSUE: reference to a compiler-generated field
                      Mesh mesh = meshInfo.m_Prefab.ObtainMesh(batch.sourceSubMeshIndex, out subMeshIndex);
                      managedBatches.SetMesh<CullingData, GroupData, BatchData, InstanceData>(managedBatchIndex, mesh, subMeshIndex, nativeBatchGroups);
                    }
                  }
                  if ((UnityEngine.Object) batch.defaultMaterial != (UnityEngine.Object) batch.loadedMaterial)
                    managedBatches.SetMaterial<CullingData, GroupData, BatchData, InstanceData>(managedBatchIndex, batch.loadedMaterial, nativeBatchGroups);
                }
                catch (Exception ex)
                {
                  // ISSUE: reference to a compiler-generated field
                  if ((UnityEngine.Object) meshInfo.m_Prefab != (UnityEngine.Object) null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    COSystemBase.baseLog.ErrorFormat((UnityEngine.Object) meshInfo.m_Prefab, ex, "Error when setting mesh for {0}", (object) meshInfo.m_Prefab.name);
                  }
                  else
                    COSystemBase.baseLog.ErrorFormat(ex, "Error when setting mesh for {0}", (object) batch.sourceMeshEntity);
                }
                local = MeshLoadingState.Complete;
              }
            }
          }
        }
      }
      nativeBatchInstances.ClearAddedInstances();
    }

    private void UnloadMeshes(ulong loadingMemorySize, ulong neededMemorySize)
    {
      if (!this.forceMeshUnloading && this.totalSizeInMemory + loadingMemorySize + neededMemorySize <= this.memoryBudget)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_StateDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      if (this.m_UnloadingData.Length == 0)
        return;
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> nativeBatchGroups = this.m_BatchManagerSystem.GetNativeBatchGroups(false, out dependencies);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ManagedBatches<OptionalProperties> managedBatches = this.m_BatchManagerSystem.GetManagedBatches();
      int num = 0;
      bool flag = false;
      dependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      for (int index1 = 0; index1 < this.m_UnloadingData.Length; ++index1)
      {
        // ISSUE: reference to a compiler-generated field
        ref BatchMeshSystem.LoadingData local1 = ref this.m_UnloadingData.ElementAt(index1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ref MeshLoadingState local2 = ref this.m_LoadingState.ElementAt(local1.m_BatchIndex);
        if (local2 == MeshLoadingState.Obsolete)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((this.forceMeshUnloading || this.totalSizeInMemory + loadingMemorySize + neededMemorySize > this.memoryBudget) && (this.forceMeshUnloading || this.totalSizeInMemory + loadingMemorySize > this.memoryBudget || -local1.m_Priority < this.m_PriorityLimit))
          {
            // ISSUE: reference to a compiler-generated field
            CustomBatch batch = (CustomBatch) managedBatches.GetBatch(local1.m_BatchIndex);
            // ISSUE: variable of a compiler-generated type
            BatchMeshSystem.MeshInfo meshInfo1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_MeshInfos.TryGetValue(batch.sharedMeshEntity, out meshInfo1))
            {
              // ISSUE: reference to a compiler-generated field
              if (meshInfo1.m_BatchCount > 1)
              {
                // ISSUE: reference to a compiler-generated field
                --meshInfo1.m_BatchCount;
                // ISSUE: reference to a compiler-generated field
                this.m_MeshInfos[batch.sharedMeshEntity] = meshInfo1;
              }
              else if (num < 30)
              {
                ++num;
                DynamicBuffer<NetCompositionPiece> buffer;
                if (this.EntityManager.TryGetBuffer<NetCompositionPiece>(batch.sharedMeshEntity, true, out buffer))
                {
                  for (int index2 = 0; index2 < buffer.Length; ++index2)
                  {
                    NetCompositionPiece compositionPiece = buffer[index2];
                    // ISSUE: variable of a compiler-generated type
                    BatchMeshSystem.MeshInfo meshInfo2;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_MeshInfos.TryGetValue(compositionPiece.m_Piece, out meshInfo2))
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (meshInfo2.m_BatchCount > 1)
                      {
                        // ISSUE: reference to a compiler-generated field
                        --meshInfo2.m_BatchCount;
                        // ISSUE: reference to a compiler-generated field
                        this.m_MeshInfos[compositionPiece.m_Piece] = meshInfo2;
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.UncacheMeshData(compositionPiece.m_Piece, batch.sourceType);
                        // ISSUE: reference to a compiler-generated field
                        this.totalSizeInMemory -= meshInfo2.m_SizeInMemory;
                        // ISSUE: reference to a compiler-generated field
                        this.m_MeshInfos.Remove(compositionPiece.m_Piece);
                      }
                    }
                  }
                }
                // ISSUE: reference to a compiler-generated field
                if (meshInfo1.m_GeneratedIndex >= 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  UnityEngine.Object.Destroy((UnityEngine.Object) this.m_GeneratedMeshes[meshInfo1.m_GeneratedIndex]);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_GeneratedMeshes[meshInfo1.m_GeneratedIndex] = (Mesh) null;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_FreeMeshIndices.Add(meshInfo1.m_GeneratedIndex);
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.RemoveShapeData(meshInfo1.m_ShapeAllocations);
                // ISSUE: reference to a compiler-generated method
                this.UncacheMeshData(batch.sharedMeshEntity, batch.sourceType);
                // ISSUE: reference to a compiler-generated field
                this.totalSizeInMemory -= meshInfo1.m_SizeInMemory;
                // ISSUE: reference to a compiler-generated field
                this.m_MeshInfos.Remove(batch.sharedMeshEntity);
              }
              else
                continue;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              managedBatches.SetMesh<CullingData, GroupData, BatchData, InstanceData>(local1.m_BatchIndex, this.GetDefaultMesh(batch.sourceType, batch.sourceFlags, batch.generatedType), 0, nativeBatchGroups);
              // ISSUE: reference to a compiler-generated field
              if (batch.generatedType == GeneratedType.None && (UnityEngine.Object) meshInfo1.m_Prefab != (UnityEngine.Object) null)
              {
                // ISSUE: reference to a compiler-generated field
                meshInfo1.m_Prefab.ReleaseMeshes();
              }
            }
            local2 = MeshLoadingState.Unloading;
            flag = true;
          }
          else
            break;
        }
      }
      if (!flag)
        return;
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_UnloadingData.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        ref BatchMeshSystem.LoadingData local3 = ref this.m_UnloadingData.ElementAt(index);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ref MeshLoadingState local4 = ref this.m_LoadingState.ElementAt(local3.m_BatchIndex);
        if (local4 == MeshLoadingState.Unloading)
        {
          // ISSUE: reference to a compiler-generated field
          CustomBatch batch = (CustomBatch) managedBatches.GetBatch(local3.m_BatchIndex);
          if ((UnityEngine.Object) batch.defaultMaterial != (UnityEngine.Object) batch.loadedMaterial)
          {
            // ISSUE: reference to a compiler-generated field
            managedBatches.SetMaterial<CullingData, GroupData, BatchData, InstanceData>(local3.m_BatchIndex, batch.defaultMaterial, nativeBatchGroups);
          }
          local4 = MeshLoadingState.None;
          // ISSUE: reference to a compiler-generated field
          this.m_UnloadingData.RemoveAtSwapBack(index--);
        }
      }
    }

    private void UnloadMeshAndGeometryAssets()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_UnloadGeometryAssets.Count == 0)
        return;
      // ISSUE: reference to a compiler-generated field
      foreach (GeometryAsset unloadGeometryAsset in this.m_UnloadGeometryAssets)
        unloadGeometryAsset.UnloadPartial();
      // ISSUE: reference to a compiler-generated field
      this.m_UnloadGeometryAssets.Clear();
    }

    private void CacheMeshData(
      RenderPrefab meshPrefab,
      GeometryAsset asset,
      Entity entity,
      MeshType type)
    {
      switch (type)
      {
        case MeshType.Object:
        case MeshType.Lane:
          int boneCount = 0;
          DynamicBuffer<ProceduralBone> buffer;
          if (this.EntityManager.TryGetBuffer<ProceduralBone>(entity, true, out buffer))
            boneCount = buffer.Length;
          bool cacheNormals = false;
          Game.Prefabs.MeshData component;
          if (this.EntityManager.TryGetComponent<Game.Prefabs.MeshData>(entity, out component))
            cacheNormals = (component.m_State & MeshFlags.Base) > (MeshFlags) 0;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          BatchMeshSystem.CacheInfo cacheInfo1 = new BatchMeshSystem.CacheInfo()
          {
            m_GeometryAsset = asset,
            m_CommandBuffer = new EntityCommandBuffer(Allocator.Persistent, PlaybackPolicy.SinglePlayback)
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cacheInfo1.m_Dependency = ObjectMeshHelpers.CacheMeshData(meshPrefab, asset, entity, boneCount, cacheNormals, cacheInfo1.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.CompleteCaching(entity);
          // ISSUE: reference to a compiler-generated method
          this.AddCaching(entity, cacheInfo1);
          break;
        case MeshType.Net:
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          BatchMeshSystem.CacheInfo cacheInfo2 = new BatchMeshSystem.CacheInfo()
          {
            m_GeometryAsset = asset,
            m_CommandBuffer = new EntityCommandBuffer(Allocator.Persistent, PlaybackPolicy.SinglePlayback)
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cacheInfo2.m_Dependency = NetMeshHelpers.CacheMeshData(asset, entity, this.EntityManager, cacheInfo2.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.AddCaching(entity, cacheInfo2);
          break;
      }
    }

    private void CacheMeshData(Mesh mesh, Entity entity, MeshType type)
    {
      switch (type)
      {
        case MeshType.Object:
        case MeshType.Lane:
          bool cacheNormals = false;
          Game.Prefabs.MeshData component;
          if (this.EntityManager.TryGetComponent<Game.Prefabs.MeshData>(entity, out component))
            cacheNormals = (component.m_State & MeshFlags.Base) > (MeshFlags) 0;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          BatchMeshSystem.CacheInfo cacheInfo1 = new BatchMeshSystem.CacheInfo();
          // ISSUE: reference to a compiler-generated field
          cacheInfo1.m_CommandBuffer = new EntityCommandBuffer(Allocator.Persistent, PlaybackPolicy.SinglePlayback);
          // ISSUE: reference to a compiler-generated field
          ObjectMeshHelpers.CacheMeshData(mesh, entity, cacheNormals, cacheInfo1.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.AddCaching(entity, cacheInfo1);
          break;
        case MeshType.Net:
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          BatchMeshSystem.CacheInfo cacheInfo2 = new BatchMeshSystem.CacheInfo();
          // ISSUE: reference to a compiler-generated field
          cacheInfo2.m_CommandBuffer = new EntityCommandBuffer(Allocator.Persistent, PlaybackPolicy.SinglePlayback);
          // ISSUE: reference to a compiler-generated field
          NetMeshHelpers.CacheMeshData(mesh, entity, this.EntityManager, cacheInfo2.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.AddCaching(entity, cacheInfo2);
          break;
      }
    }

    private void UncacheMeshData(Entity mesh, MeshType type)
    {
      switch (type)
      {
        case MeshType.Object:
        case MeshType.Lane:
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          BatchMeshSystem.CacheInfo cacheInfo1 = new BatchMeshSystem.CacheInfo();
          // ISSUE: reference to a compiler-generated field
          cacheInfo1.m_CommandBuffer = new EntityCommandBuffer(Allocator.Persistent, PlaybackPolicy.SinglePlayback);
          // ISSUE: reference to a compiler-generated field
          ObjectMeshHelpers.UncacheMeshData(mesh, cacheInfo1.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.AddCaching(mesh, cacheInfo1);
          break;
        case MeshType.Net:
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          BatchMeshSystem.CacheInfo cacheInfo2 = new BatchMeshSystem.CacheInfo();
          // ISSUE: reference to a compiler-generated field
          cacheInfo2.m_CommandBuffer = new EntityCommandBuffer(Allocator.Persistent, PlaybackPolicy.SinglePlayback);
          // ISSUE: reference to a compiler-generated field
          NetMeshHelpers.UncacheMeshData(mesh, cacheInfo2.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.AddCaching(mesh, cacheInfo2);
          break;
      }
    }

    private void GenerateMeshes()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_GenerateMeshEntities.IsCreated)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_GenerateMeshDataArray = Mesh.AllocateWritableMeshData(this.m_GenerateMeshEntities.Length);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_GenerateMeshDeps = BatchMeshHelpers.GenerateMeshes(this, this.m_GenerateMeshEntities, this.m_GenerateMeshDataArray, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.Dependency = this.m_GenerateMeshDeps;
    }

    private BatchMeshSystem.ShapeAllocation[] AddShapeData(RenderPrefab meshPrefab)
    {
      GeometryAsset geometryAsset = meshPrefab.geometryAsset;
      if ((AssetData) geometryAsset == (IAssetData) null)
        return (BatchMeshSystem.ShapeAllocation[]) null;
      NativeArray<byte> shapeDataBuffer = geometryAsset.shapeDataBuffer;
      if (!shapeDataBuffer.IsCreated)
        return (BatchMeshSystem.ShapeAllocation[]) null;
      NativeArray<ulong> data = shapeDataBuffer.Reinterpret<ulong>(1);
      BatchMeshSystem.ShapeAllocation[] shapeAllocationArray = new BatchMeshSystem.ShapeAllocation[meshPrefab.meshCount];
      for (int meshIndex = 0; meshIndex < shapeAllocationArray.Length; ++meshIndex)
      {
        int shapeDataSize = geometryAsset.GetShapeDataSize(meshIndex);
        if (shapeDataSize != 0)
        {
          // ISSUE: reference to a compiler-generated field
          shapeAllocationArray[meshIndex].m_Stride = geometryAsset.GetVertexCount(meshIndex);
          // ISSUE: reference to a compiler-generated field
          shapeAllocationArray[meshIndex].m_PositionExtent = (float3) geometryAsset.GetShapePositionExtent(meshIndex);
          // ISSUE: reference to a compiler-generated field
          shapeAllocationArray[meshIndex].m_NormalExtent = (float3) geometryAsset.GetShapeNormalExtent(meshIndex);
          uint size = (uint) shapeDataSize / 8U;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          shapeAllocationArray[meshIndex].m_Allocation = this.m_ShapeAllocator.Allocate(size);
          // ISSUE: reference to a compiler-generated field
          if (shapeAllocationArray[meshIndex].m_Allocation.Empty)
          {
            uint num = 1048576;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_ShapeAllocator.Resize(this.m_ShapeAllocator.Size + (uint) ((int) num + (int) size - 1) / num * num);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            shapeAllocationArray[meshIndex].m_Allocation = this.m_ShapeAllocator.Allocate(size);
          }
          // ISSUE: reference to a compiler-generated field
          ++this.m_ShapeCount;
        }
      }
      // ISSUE: reference to a compiler-generated method
      this.ResizeShapeBuffer();
      for (int meshIndex = 0; meshIndex < shapeAllocationArray.Length; ++meshIndex)
      {
        int shapeStartOffset = geometryAsset.GetShapeStartOffset(meshIndex);
        int shapeDataSize = geometryAsset.GetShapeDataSize(meshIndex);
        if (shapeDataSize != 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ShapeBuffer.SetData<ulong>(data, shapeStartOffset / 8, (int) shapeAllocationArray[meshIndex].m_Allocation.Begin, shapeDataSize / 8);
        }
      }
      return shapeAllocationArray;
    }

    private void RemoveShapeData(BatchMeshSystem.ShapeAllocation[] allocations)
    {
      if (allocations == null)
        return;
      for (int index = 0; index < allocations.Length; ++index)
      {
        // ISSUE: variable of a compiler-generated type
        BatchMeshSystem.ShapeAllocation allocation = allocations[index];
        // ISSUE: reference to a compiler-generated field
        if (!allocation.m_Allocation.Empty)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ShapeAllocator.Release(allocation.m_Allocation);
          // ISSUE: reference to a compiler-generated field
          --this.m_ShapeCount;
        }
      }
    }

    public void SetShapeParameters(
      MaterialPropertyBlock customProps,
      Entity sharedMeshEntity,
      int subMeshIndex)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BatchMeshSystem.ShapeAllocation allocation = new BatchMeshSystem.ShapeAllocation();
      // ISSUE: variable of a compiler-generated type
      BatchMeshSystem.MeshInfo meshInfo;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_MeshInfos.TryGetValue(sharedMeshEntity, out meshInfo) && meshInfo.m_ShapeAllocations != null && meshInfo.m_ShapeAllocations.Length > subMeshIndex)
      {
        // ISSUE: reference to a compiler-generated field
        allocation = meshInfo.m_ShapeAllocations[subMeshIndex];
      }
      // ISSUE: reference to a compiler-generated method
      this.SetShapeParameters(customProps, allocation);
    }

    public void GetShapeStats(out uint allocatedSize, out uint bufferSize, out uint count)
    {
      // ISSUE: reference to a compiler-generated field
      allocatedSize = this.m_ShapeAllocator.UsedSpace * 8U;
      // ISSUE: reference to a compiler-generated field
      bufferSize = this.m_ShapeAllocator.Size * 8U;
      // ISSUE: reference to a compiler-generated field
      count = (uint) this.m_ShapeCount;
    }

    private void SetShapeParameters(
      CustomBatch batch,
      BatchMeshSystem.ShapeAllocation[] allocations)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BatchMeshSystem.ShapeAllocation allocation = new BatchMeshSystem.ShapeAllocation();
      if (allocations != null && allocations.Length > batch.sourceSubMeshIndex)
        allocation = allocations[batch.sourceSubMeshIndex];
      // ISSUE: reference to a compiler-generated method
      this.SetShapeParameters(batch.customProps, allocation);
      // ISSUE: reference to a compiler-generated method
      this.BatchPropertyUpdated(batch);
    }

    private void SetShapeParameters(
      MaterialPropertyBlock customProps,
      BatchMeshSystem.ShapeAllocation allocation)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      PropertyData propertyData1 = this.m_BatchManagerSystem.GetPropertyData(MaterialProperty.ShapeParameters1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      PropertyData propertyData2 = this.m_BatchManagerSystem.GetPropertyData(MaterialProperty.ShapeParameters2);
      Vector4 zero1 = Vector4.zero;
      Vector4 zero2 = Vector4.zero;
      // ISSUE: reference to a compiler-generated field
      if (!allocation.m_Allocation.Empty)
      {
        // ISSUE: reference to a compiler-generated field
        zero1.x = allocation.m_PositionExtent.x;
        // ISSUE: reference to a compiler-generated field
        zero1.y = allocation.m_PositionExtent.y;
        // ISSUE: reference to a compiler-generated field
        zero1.z = allocation.m_PositionExtent.z;
        // ISSUE: reference to a compiler-generated field
        zero1.w = math.asfloat(allocation.m_Allocation.Begin);
        // ISSUE: reference to a compiler-generated field
        zero2.x = allocation.m_NormalExtent.x;
        // ISSUE: reference to a compiler-generated field
        zero2.y = allocation.m_NormalExtent.y;
        // ISSUE: reference to a compiler-generated field
        zero2.z = allocation.m_NormalExtent.z;
        // ISSUE: reference to a compiler-generated field
        zero2.w = math.asfloat(allocation.m_Stride);
      }
      customProps.SetVector(propertyData1.m_NameID, zero1);
      customProps.SetVector(propertyData2.m_NameID, zero2);
    }

    private void BatchPropertyUpdated(CustomBatch batch)
    {
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> nativeBatchGroups = this.m_BatchManagerSystem.GetNativeBatchGroups(false, out dependencies);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ManagedBatches<OptionalProperties> managedBatches = this.m_BatchManagerSystem.GetManagedBatches();
      BatchFlags sourceFlags = batch.sourceFlags;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.m_BatchManagerSystem.IsMotionVectorsEnabled())
        sourceFlags &= ~BatchFlags.MotionVectors;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.m_BatchManagerSystem.IsLodFadeEnabled())
        sourceFlags &= ~BatchFlags.LodFade;
      OptionalProperties optionalProperties = new OptionalProperties(sourceFlags, batch.sourceType);
      dependencies.Complete();
      NativeBatchProperties batchProperties = managedBatches.GetBatchProperties(batch.material.shader, optionalProperties);
      WriteableBatchDefaultsAccessor defaultsAccessor = nativeBatchGroups.GetBatchDefaultsAccessor(batch.groupIndex, batch.batchIndex);
      if ((AssetData) batch.sourceSurface != (IAssetData) null)
      {
        // ISSUE: reference to a compiler-generated method
        managedBatches.SetDefaults(ManagedBatchSystem.GetTemplate(batch.sourceSurface), batch.sourceSurface.floats, batch.sourceSurface.ints, batch.sourceSurface.vectors, batch.sourceSurface.colors, batch.customProps, batchProperties, defaultsAccessor);
      }
      else
        managedBatches.SetDefaults(batch.sourceMaterial, batch.customProps, batchProperties, defaultsAccessor);
    }

    private void ResizeShapeBuffer()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int count = this.m_ShapeBuffer != null ? this.m_ShapeBuffer.count : 0;
      // ISSUE: reference to a compiler-generated field
      int size = (int) this.m_ShapeAllocator.Size;
      if (count == size)
        return;
      GraphicsBuffer graphicsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, size, 8);
      graphicsBuffer.name = "Shape buffer";
      Shader.SetGlobalBuffer("shapeBuffer", graphicsBuffer);
      // ISSUE: reference to a compiler-generated field
      if (this.m_ShapeBuffer != null)
      {
        ulong[] data = new ulong[count];
        // ISSUE: reference to a compiler-generated field
        this.m_ShapeBuffer.GetData((System.Array) data);
        graphicsBuffer.SetData((System.Array) data, 0, 0, count);
        // ISSUE: reference to a compiler-generated field
        this.m_ShapeBuffer.Release();
      }
      else
        graphicsBuffer.SetData((System.Array) new ulong[1], 0, 0, 1);
      // ISSUE: reference to a compiler-generated field
      this.m_ShapeBuffer = graphicsBuffer;
    }

    [UnityEngine.Scripting.Preserve]
    public BatchMeshSystem()
    {
    }

    private struct LoadingData : IComparable<BatchMeshSystem.LoadingData>
    {
      public int m_Priority;
      public int m_BatchIndex;

      public LoadingData(int priority, int batchIndex)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Priority = priority;
        // ISSUE: reference to a compiler-generated field
        this.m_BatchIndex = batchIndex;
      }

      public int CompareTo(BatchMeshSystem.LoadingData other) => other.m_Priority - this.m_Priority;
    }

    private struct MeshInfo
    {
      public RenderPrefab m_Prefab;
      public BatchMeshSystem.ShapeAllocation[] m_ShapeAllocations;
      public ulong m_SizeInMemory;
      public int m_GeneratedIndex;
      public int m_BatchCount;
    }

    private struct CacheInfo
    {
      public GeometryAsset m_GeometryAsset;
      public EntityCommandBuffer m_CommandBuffer;
      public JobHandle m_Dependency;
    }

    private struct ShapeAllocation
    {
      public NativeHeapBlock m_Allocation;
      public int m_Stride;
      public float3 m_PositionExtent;
      public float3 m_NormalExtent;
    }

    [BurstCompile]
    private struct LoadingPriorityJob : IJob
    {
      [ReadOnly]
      public int m_PriorityLimit;
      public NativeList<int> m_BatchPriority;
      public NativeList<MeshLoadingState> m_LoadingState;
      public NativeList<BatchMeshSystem.LoadingData> m_LoadingData;
      public NativeList<BatchMeshSystem.LoadingData> m_UnloadingData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_LoadingData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          ref BatchMeshSystem.LoadingData local1 = ref this.m_LoadingData.ElementAt(index);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ref int local2 = ref this.m_BatchPriority.ElementAt(local1.m_BatchIndex);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ref MeshLoadingState local3 = ref this.m_LoadingState.ElementAt(local1.m_BatchIndex);
          if (local3 == MeshLoadingState.Pending)
          {
            // ISSUE: reference to a compiler-generated field
            if (local2 < this.m_PriorityLimit)
            {
              local3 = MeshLoadingState.None;
              // ISSUE: reference to a compiler-generated field
              this.m_LoadingData.RemoveAtSwapBack(index--);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              local1.m_Priority = local2;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            local1.m_Priority = 1000256 + local2;
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_UnloadingData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          ref BatchMeshSystem.LoadingData local4 = ref this.m_UnloadingData.ElementAt(index);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ref int local5 = ref this.m_BatchPriority.ElementAt(local4.m_BatchIndex);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ref MeshLoadingState local6 = ref this.m_LoadingState.ElementAt(local4.m_BatchIndex);
          if (local6 == MeshLoadingState.Obsolete)
          {
            // ISSUE: reference to a compiler-generated field
            if (local5 >= this.m_PriorityLimit)
            {
              local6 = MeshLoadingState.Complete;
              // ISSUE: reference to a compiler-generated field
              this.m_UnloadingData.RemoveAtSwapBack(index--);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              local4.m_Priority = -local5;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            local4.m_Priority = 1000256 - local5;
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_BatchPriority.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          ref int local7 = ref this.m_BatchPriority.ElementAt(index);
          // ISSUE: reference to a compiler-generated field
          ref MeshLoadingState local8 = ref this.m_LoadingState.ElementAt(index);
          // ISSUE: variable of a compiler-generated type
          BatchMeshSystem.LoadingData loadingData;
          // ISSUE: reference to a compiler-generated field
          if (local7 >= this.m_PriorityLimit)
          {
            if (local8 == MeshLoadingState.None)
            {
              local8 = MeshLoadingState.Pending;
              // ISSUE: reference to a compiler-generated field
              ref NativeList<BatchMeshSystem.LoadingData> local9 = ref this.m_LoadingData;
              // ISSUE: object of a compiler-generated type is created
              loadingData = new BatchMeshSystem.LoadingData(local7, index);
              ref BatchMeshSystem.LoadingData local10 = ref loadingData;
              local9.Add(in local10);
            }
            local7 -= 256;
          }
          else
          {
            if (local8 == MeshLoadingState.Complete)
            {
              local8 = MeshLoadingState.Obsolete;
              // ISSUE: reference to a compiler-generated field
              ref NativeList<BatchMeshSystem.LoadingData> local11 = ref this.m_UnloadingData;
              // ISSUE: object of a compiler-generated type is created
              loadingData = new BatchMeshSystem.LoadingData(-local7, index);
              ref BatchMeshSystem.LoadingData local12 = ref loadingData;
              local11.Add(in local12);
            }
            local7 = math.max(-1000000, local7 - 1);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_LoadingData.Length >= 2)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LoadingData.Sort<BatchMeshSystem.LoadingData>();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_UnloadingData.Length < 2)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_UnloadingData.Sort<BatchMeshSystem.LoadingData>();
      }
    }
  }
}
