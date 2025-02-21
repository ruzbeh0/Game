// Decompiled with JetBrains decompiler
// Type: Game.Rendering.AreaBatchSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Common;
using Game.Prefabs;
using Game.Serialization;
using Game.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class AreaBatchSystem : GameSystemBase, IPreDeserialize
  {
    public const uint AREABUFFER_MEMORY_DEFAULT = 4194304;
    public const uint AREABUFFER_MEMORY_INCREMENT = 1048576;
    private PrefabSystem m_PrefabSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private RenderingSystem m_RenderingSystem;
    private BatchDataSystem m_BatchDataSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private ComputeBuffer m_AreaTriangleBuffer;
    private ComputeBuffer m_AreaColorBuffer;
    private List<AreaBatchSystem.ManagedBatchData> m_ManagedBatchData;
    private NativeHeapAllocator m_AreaBufferAllocator;
    private NativeReference<int> m_AllocationCount;
    private NativeList<AreaBatchSystem.NativeBatchData> m_NativeBatchData;
    private NativeList<AreaTriangleData> m_AreaTriangleData;
    private NativeList<AreaBatchSystem.TriangleMetaData> m_TriangleMetaData;
    private NativeList<AreaColorData> m_AreaColorData;
    private NativeList<NativeHeapBlock> m_UpdatedTriangles;
    private EntityQuery m_UpdatedQuery;
    private EntityQuery m_PrefabQuery;
    private JobHandle m_DataDependencies;
    private float3 m_PrevCameraPosition;
    private float3 m_PrevCameraDirection;
    private float4 m_PrevLodParameters;
    private int m_AreaParameters;
    private int m_DecalLayerMask;
    private bool m_Loaded;
    private bool m_ColorsUpdated;
    private AreaBatchSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchDataSystem = this.World.GetOrCreateSystemManaged<BatchDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Areas.Batch>()
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
      this.m_PrefabQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<RenderedAreaData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ManagedBatchData = new List<AreaBatchSystem.ManagedBatchData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaBufferAllocator = new NativeHeapAllocator(4194304U / AreaBatchSystem.GetTriangleSize(), 1U, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_AllocationCount = new NativeReference<int>(0, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchData = new NativeList<AreaBatchSystem.NativeBatchData>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_AreaTriangleData = new NativeList<AreaTriangleData>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_TriangleMetaData = new NativeList<AreaBatchSystem.TriangleMetaData>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_AreaColorData = new NativeList<AreaColorData>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedTriangles = new NativeList<NativeHeapBlock>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AreaTriangleData.ResizeUninitialized((int) this.m_AreaBufferAllocator.Size);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TriangleMetaData.ResizeUninitialized((int) this.m_AreaBufferAllocator.Size);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AreaColorData.ResizeUninitialized((int) this.m_AreaBufferAllocator.Size);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AreaTriangleBuffer = new ComputeBuffer(this.m_AreaTriangleData.Capacity, sizeof (AreaTriangleData));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AreaColorBuffer = new ComputeBuffer(this.m_AreaColorData.Capacity, sizeof (AreaColorData));
      // ISSUE: reference to a compiler-generated field
      this.m_AreaParameters = Shader.PropertyToID("colossal_AreaParameters");
      // ISSUE: reference to a compiler-generated field
      this.m_DecalLayerMask = Shader.PropertyToID("colossal_DecalLayerMask");
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_AreaTriangleBuffer.Release();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaColorBuffer.Release();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_ManagedBatchData.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        AreaBatchSystem.ManagedBatchData managedBatchData = this.m_ManagedBatchData[index];
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) managedBatchData.m_Material != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          UnityEngine.Object.Destroy((UnityEngine.Object) managedBatchData.m_Material);
        }
        // ISSUE: reference to a compiler-generated field
        if (managedBatchData.m_VisibleIndices != null)
        {
          // ISSUE: reference to a compiler-generated field
          managedBatchData.m_VisibleIndices.Release();
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_DataDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_NativeBatchData.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        ref AreaBatchSystem.NativeBatchData local = ref this.m_NativeBatchData.ElementAt(index);
        // ISSUE: reference to a compiler-generated field
        if (local.m_AreaMetaData.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          local.m_AreaMetaData.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (local.m_VisibleIndices.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          local.m_VisibleIndices.Dispose();
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_AreaBufferAllocator.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_AllocationCount.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_NativeBatchData.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaTriangleData.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_TriangleMetaData.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaColorData.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedTriangles.Dispose();
      base.OnDestroy();
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DataDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_AllocationCount.Value = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_AreaBufferAllocator.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedTriangles.Clear();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_NativeBatchData.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        ref AreaBatchSystem.NativeBatchData local = ref this.m_NativeBatchData.ElementAt(index);
        // ISSUE: reference to a compiler-generated field
        if (local.m_AreaMetaData.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          local.m_AreaMetaData.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (local.m_VisibleIndices.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          local.m_VisibleIndices.Clear();
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    public int GetBatchCount() => this.m_ManagedBatchData.Count;

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    public unsafe bool GetAreaBatch(
      int index,
      out ComputeBuffer buffer,
      out ComputeBuffer colors,
      out GraphicsBuffer indices,
      out Material material,
      out Bounds bounds,
      out int count,
      out int rendererPriority)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DataDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      AreaBatchSystem.ManagedBatchData managedBatchData = this.m_ManagedBatchData[index];
      // ISSUE: reference to a compiler-generated field
      ref AreaBatchSystem.NativeBatchData local = ref this.m_NativeBatchData.ElementAt(index);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_AreaTriangleBuffer.count != this.m_AreaTriangleData.Capacity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTriangleBuffer.Release();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTriangleBuffer = new ComputeBuffer(this.m_AreaTriangleData.Capacity, sizeof (AreaTriangleData));
        // ISSUE: reference to a compiler-generated field
        this.m_UpdatedTriangles.Clear();
        // ISSUE: reference to a compiler-generated field
        uint highestUsedAddress = this.m_AreaBufferAllocator.OnePastHighestUsedAddress;
        if (highestUsedAddress != 0U)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UpdatedTriangles.Add(new NativeHeapBlock(new UnsafeHeapBlock(0U, highestUsedAddress)));
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_UpdatedTriangles.Length != 0)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_UpdatedTriangles.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          NativeHeapBlock updatedTriangle = this.m_UpdatedTriangles[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_AreaTriangleBuffer.SetData<AreaTriangleData>(this.m_AreaTriangleData.AsArray(), (int) updatedTriangle.Begin, (int) updatedTriangle.Begin, (int) updatedTriangle.Length);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_UpdatedTriangles.Clear();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_AreaColorBuffer.count != this.m_AreaColorData.Capacity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AreaColorBuffer.Release();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AreaColorBuffer = new ComputeBuffer(this.m_AreaColorData.Capacity, sizeof (AreaColorData));
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ColorsUpdated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ColorsUpdated = false;
        // ISSUE: reference to a compiler-generated field
        uint highestUsedAddress = this.m_AreaBufferAllocator.OnePastHighestUsedAddress;
        if (highestUsedAddress != 0U)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_AreaColorBuffer.SetData<AreaColorData>(this.m_AreaColorData.AsArray(), 0, 0, (int) highestUsedAddress);
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (managedBatchData.m_VisibleIndices.count != local.m_VisibleIndices.Capacity)
      {
        // ISSUE: reference to a compiler-generated field
        managedBatchData.m_VisibleIndices.Release();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        managedBatchData.m_VisibleIndices = new GraphicsBuffer(GraphicsBuffer.Target.Structured, local.m_VisibleIndices.Capacity, 4);
      }
      // ISSUE: reference to a compiler-generated field
      if (local.m_VisibleIndicesUpdated)
      {
        // ISSUE: reference to a compiler-generated field
        local.m_VisibleIndicesUpdated = false;
        // ISSUE: reference to a compiler-generated field
        if (local.m_VisibleIndices.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<int> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>((void*) local.m_VisibleIndices.Ptr, local.m_VisibleIndices.Length, Allocator.None);
          // ISSUE: reference to a compiler-generated field
          managedBatchData.m_VisibleIndices.SetData<int>(nativeArray, 0, 0, nativeArray.Length);
        }
      }
      // ISSUE: reference to a compiler-generated field
      buffer = this.m_AreaTriangleBuffer;
      // ISSUE: reference to a compiler-generated field
      colors = this.m_AreaColorBuffer;
      // ISSUE: reference to a compiler-generated field
      indices = managedBatchData.m_VisibleIndices;
      // ISSUE: reference to a compiler-generated field
      material = managedBatchData.m_Material;
      // ISSUE: reference to a compiler-generated field
      bounds = RenderingUtils.ToBounds(local.m_Bounds);
      // ISSUE: reference to a compiler-generated field
      count = local.m_VisibleIndices.Length;
      // ISSUE: reference to a compiler-generated field
      rendererPriority = managedBatchData.m_RendererPriority;
      return count != 0;
    }

    public NativeList<AreaColorData> GetColorData(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ColorsUpdated = true;
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_DataDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_AreaColorData;
    }

    public void AddColorWriter(JobHandle jobHandle) => this.m_DataDependencies = jobHandle;

    public void GetAreaStats(out uint allocatedSize, out uint bufferSize, out uint count)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DataDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      allocatedSize = this.m_AreaBufferAllocator.UsedSpace * AreaBatchSystem.GetTriangleSize();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      bufferSize = this.m_AreaBufferAllocator.Size * AreaBatchSystem.GetTriangleSize();
      // ISSUE: reference to a compiler-generated field
      count = (uint) this.m_AllocationCount.Value;
    }

    private static uint GetTriangleSize()
    {
      return (uint) (sizeof (AreaTriangleData) + sizeof (AreaColorData));
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      int num = this.GetLoaded() ? 1 : 0;
      // ISSUE: reference to a compiler-generated field
      this.m_DataDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedTriangles.Clear();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PrefabQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdatePrefabs();
      }
      // ISSUE: reference to a compiler-generated field
      float3 float3_1 = this.m_PrevCameraPosition;
      // ISSUE: reference to a compiler-generated field
      float3 float3_2 = this.m_PrevCameraDirection;
      // ISSUE: reference to a compiler-generated field
      float4 float4 = this.m_PrevLodParameters;
      LODParameters lodParameters;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (this.m_CameraUpdateSystem.TryGetLODParameters(out lodParameters))
      {
        float3_1 = (float3) lodParameters.cameraPosition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        float4 = RenderingUtils.CalculateLodParameters(this.m_BatchDataSystem.GetLevelOfDetail(this.m_RenderingSystem.frameLod, this.m_CameraUpdateSystem.activeCameraController), lodParameters);
        // ISSUE: reference to a compiler-generated field
        float3_2 = this.m_CameraUpdateSystem.activeViewer.forward;
      }
      BoundsMask boundsMask1 = BoundsMask.NormalLayers;
      BoundsMask boundsMask2 = BoundsMask.NormalLayers;
      if (num != 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PrevCameraPosition = float3_1;
        // ISSUE: reference to a compiler-generated field
        this.m_PrevCameraDirection = float3_2;
        // ISSUE: reference to a compiler-generated field
        this.m_PrevLodParameters = float4;
        boundsMask2 = (BoundsMask) 0;
      }
      NativeParallelQueue<AreaBatchSystem.CullingAction> nativeParallelQueue = new NativeParallelQueue<AreaBatchSystem.CullingAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<AreaBatchSystem.AllocationAction> nativeQueue = new NativeQueue<AreaBatchSystem.AllocationAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeArray<int> nativeArray1 = new NativeArray<int>(512, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
      NativeArray<int> nativeArray2 = new NativeArray<int>(512, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AreaBatchSystem.TreeCullingJob1 jobData1 = new AreaBatchSystem.TreeCullingJob1()
      {
        m_AreaSearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies),
        m_LodParameters = float4,
        m_PrevLodParameters = this.m_PrevLodParameters,
        m_CameraPosition = float3_1,
        m_PrevCameraPosition = this.m_PrevCameraPosition,
        m_CameraDirection = float3_2,
        m_PrevCameraDirection = this.m_PrevCameraDirection,
        m_VisibleMask = boundsMask1,
        m_PrevVisibleMask = boundsMask2,
        m_NodeBuffer = nativeArray1,
        m_SubDataBuffer = nativeArray2,
        m_ActionQueue = nativeParallelQueue.AsWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AreaBatchSystem.TreeCullingJob2 jobData2 = new AreaBatchSystem.TreeCullingJob2()
      {
        m_AreaSearchTree = jobData1.m_AreaSearchTree,
        m_LodParameters = float4,
        m_PrevLodParameters = this.m_PrevLodParameters,
        m_CameraPosition = float3_1,
        m_PrevCameraPosition = this.m_PrevCameraPosition,
        m_CameraDirection = float3_2,
        m_PrevCameraDirection = this.m_PrevCameraDirection,
        m_VisibleMask = boundsMask1,
        m_PrevVisibleMask = boundsMask2,
        m_NodeBuffer = nativeArray1,
        m_SubDataBuffer = nativeArray2,
        m_ActionQueue = nativeParallelQueue.AsWriter()
      };
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AreaBatchSystem.QueryCullingJob jobData3 = new AreaBatchSystem.QueryCullingJob()
      {
        m_EntityType = this.GetEntityTypeHandle(),
        m_BatchType = this.GetComponentTypeHandle<Game.Areas.Batch>(true),
        m_DeletedType = this.GetComponentTypeHandle<Deleted>(true),
        m_PrefabRefType = this.GetComponentTypeHandle<PrefabRef>(true),
        m_NodeType = this.GetBufferTypeHandle<Game.Areas.Node>(true),
        m_TriangleType = this.GetBufferTypeHandle<Triangle>(true),
        m_PrefabAreaGeometryData = this.GetComponentLookup<AreaGeometryData>(true),
        m_LodParameters = float4,
        m_CameraPosition = float3_1,
        m_CameraDirection = float3_2,
        m_VisibleMask = boundsMask1,
        m_ActionQueue = nativeParallelQueue.AsWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AreaBatchSystem.CullingActionJob jobData4 = new AreaBatchSystem.CullingActionJob()
      {
        m_PrefabRefData = this.GetComponentLookup<PrefabRef>(true),
        m_PrefabRenderedAreaData = this.GetComponentLookup<RenderedAreaData>(true),
        m_Triangles = this.GetBufferLookup<Triangle>(true),
        m_CullingActions = nativeParallelQueue.AsReader(),
        m_AllocationActions = nativeQueue.AsParallelWriter(),
        m_BatchData = this.GetComponentLookup<Game.Areas.Batch>(false),
        m_TriangleMetaData = this.m_TriangleMetaData
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AreaBatchSystem.BatchAllocationJob jobData5 = new AreaBatchSystem.BatchAllocationJob()
      {
        m_BatchData = this.GetComponentLookup<Game.Areas.Batch>(false),
        m_NativeBatchData = this.m_NativeBatchData,
        m_TriangleMetaData = this.m_TriangleMetaData,
        m_AreaTriangleData = this.m_AreaTriangleData,
        m_AreaColorData = this.m_AreaColorData,
        m_UpdatedTriangles = this.m_UpdatedTriangles,
        m_AllocationActions = nativeQueue,
        m_AreaBufferAllocator = this.m_AreaBufferAllocator,
        m_AllocationCount = this.m_AllocationCount
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AreaBatchSystem.TriangleUpdateJob jobData6 = new AreaBatchSystem.TriangleUpdateJob()
      {
        m_AreaData = this.GetComponentLookup<Area>(true),
        m_OwnerData = this.GetComponentLookup<Owner>(true),
        m_TransformData = this.GetComponentLookup<Game.Objects.Transform>(true),
        m_PrefabRefData = this.GetComponentLookup<PrefabRef>(true),
        m_PrefabRenderedAreaData = this.GetComponentLookup<RenderedAreaData>(true),
        m_Nodes = this.GetBufferLookup<Game.Areas.Node>(true),
        m_Triangles = this.GetBufferLookup<Triangle>(true),
        m_Expands = this.GetBufferLookup<Expand>(true),
        m_CullingActions = nativeParallelQueue.AsReader(),
        m_BatchData = this.GetComponentLookup<Game.Areas.Batch>(false),
        m_TriangleMetaData = this.m_TriangleMetaData,
        m_AreaTriangleData = this.m_AreaTriangleData,
        m_NativeBatchData = this.m_NativeBatchData
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AreaBatchSystem.VisibleUpdateJob jobData7 = new AreaBatchSystem.VisibleUpdateJob()
      {
        m_NativeBatchData = this.m_NativeBatchData
      };
      JobHandle dependsOn1 = jobData1.Schedule<AreaBatchSystem.TreeCullingJob1>(dependencies);
      JobHandle jobHandle1 = jobData2.Schedule<AreaBatchSystem.TreeCullingJob2>(nativeArray1.Length, 1, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      JobHandle job1 = jobData3.ScheduleParallel<AreaBatchSystem.QueryCullingJob>(this.m_UpdatedQuery, this.Dependency);
      JobHandle dependsOn2 = jobData4.Schedule<AreaBatchSystem.CullingActionJob>(nativeParallelQueue.HashRange, 1, JobHandle.CombineDependencies(jobHandle1, job1));
      JobHandle jobHandle2 = jobData5.Schedule<AreaBatchSystem.BatchAllocationJob>(dependsOn2);
      JobHandle inputDeps = jobData6.Schedule<AreaBatchSystem.TriangleUpdateJob>(nativeParallelQueue.HashRange, 1, jobHandle2);
      // ISSUE: reference to a compiler-generated field
      int count = this.m_ManagedBatchData.Count;
      JobHandle dependsOn3 = inputDeps;
      JobHandle jobHandle3 = jobData7.Schedule<AreaBatchSystem.VisibleUpdateJob>(count, 1, dependsOn3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle1);
      nativeParallelQueue.Dispose(inputDeps);
      nativeQueue.Dispose(jobHandle2);
      nativeArray1.Dispose(jobHandle1);
      nativeArray2.Dispose(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      this.m_PrevCameraPosition = float3_1;
      // ISSUE: reference to a compiler-generated field
      this.m_PrevCameraDirection = float3_2;
      // ISSUE: reference to a compiler-generated field
      this.m_PrevLodParameters = float4;
      this.Dependency = inputDeps;
      // ISSUE: reference to a compiler-generated field
      this.m_DataDependencies = jobHandle3;
    }

    public void EnabledShadersUpdated()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DataDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_ManagedBatchData.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        AreaBatchSystem.ManagedBatchData managedBatchData = this.m_ManagedBatchData[index];
        // ISSUE: reference to a compiler-generated field
        ref AreaBatchSystem.NativeBatchData local = ref this.m_NativeBatchData.ElementAt(index);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        bool flag = this.m_RenderingSystem.IsShaderEnabled(managedBatchData.m_Material.shader);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        local.m_VisibleUpdated |= flag != local.m_IsEnabled;
        // ISSUE: reference to a compiler-generated field
        local.m_IsEnabled = flag;
      }
    }

    private void UpdatePrefabs()
    {
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_PrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Deleted> componentTypeHandle1 = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<AreaGeometryData> componentTypeHandle3 = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentTypeHandle;
        this.CompleteDependency();
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
          NativeArray<PrefabData> nativeArray2 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle2);
          NativeArray<AreaGeometryData> nativeArray3 = archetypeChunk.GetNativeArray<AreaGeometryData>(ref componentTypeHandle3);
          EntityManager entityManager;
          if (archetypeChunk.Has<Deleted>(ref componentTypeHandle1))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_DataDependencies.Complete();
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              Entity entity = nativeArray1[index2];
              entityManager = this.EntityManager;
              RenderedAreaData componentData1 = entityManager.GetComponentData<RenderedAreaData>(entity);
              // ISSUE: reference to a compiler-generated field
              if (this.m_ManagedBatchData.Count > componentData1.m_BatchIndex)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: variable of a compiler-generated type
                AreaBatchSystem.ManagedBatchData managedBatchData1 = this.m_ManagedBatchData[componentData1.m_BatchIndex];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: variable of a compiler-generated type
                AreaBatchSystem.NativeBatchData nativeBatchData1 = this.m_NativeBatchData[componentData1.m_BatchIndex];
                // ISSUE: reference to a compiler-generated field
                if (!(nativeBatchData1.m_Prefab != entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  if ((UnityEngine.Object) managedBatchData1.m_Material != (UnityEngine.Object) null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    UnityEngine.Object.Destroy((UnityEngine.Object) managedBatchData1.m_Material);
                  }
                  // ISSUE: reference to a compiler-generated field
                  if (managedBatchData1.m_VisibleIndices != null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    managedBatchData1.m_VisibleIndices.Release();
                  }
                  // ISSUE: reference to a compiler-generated field
                  if (nativeBatchData1.m_AreaMetaData.IsCreated)
                  {
                    // ISSUE: reference to a compiler-generated field
                    nativeBatchData1.m_AreaMetaData.Dispose();
                  }
                  // ISSUE: reference to a compiler-generated field
                  if (nativeBatchData1.m_VisibleIndices.IsCreated)
                  {
                    // ISSUE: reference to a compiler-generated field
                    nativeBatchData1.m_VisibleIndices.Dispose();
                  }
                  // ISSUE: reference to a compiler-generated field
                  if (componentData1.m_BatchIndex != this.m_ManagedBatchData.Count - 1)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: variable of a compiler-generated type
                    AreaBatchSystem.ManagedBatchData managedBatchData2 = this.m_ManagedBatchData[this.m_ManagedBatchData.Count - 1];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: variable of a compiler-generated type
                    AreaBatchSystem.NativeBatchData nativeBatchData2 = this.m_NativeBatchData[this.m_ManagedBatchData.Count - 1];
                    entityManager = this.EntityManager;
                    // ISSUE: reference to a compiler-generated field
                    RenderedAreaData componentData2 = entityManager.GetComponentData<RenderedAreaData>(nativeBatchData2.m_Prefab) with
                    {
                      m_BatchIndex = componentData1.m_BatchIndex
                    };
                    entityManager = this.EntityManager;
                    // ISSUE: reference to a compiler-generated field
                    entityManager.SetComponentData<RenderedAreaData>(nativeBatchData2.m_Prefab, componentData2);
                    // ISSUE: reference to a compiler-generated field
                    this.m_ManagedBatchData[componentData1.m_BatchIndex] = managedBatchData2;
                    // ISSUE: reference to a compiler-generated field
                    this.m_NativeBatchData[componentData1.m_BatchIndex] = nativeBatchData2;
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_ManagedBatchData.RemoveAt(this.m_ManagedBatchData.Count - 1);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_NativeBatchData.RemoveAt(this.m_NativeBatchData.Length - 1);
                }
              }
            }
          }
          else
          {
            for (int index3 = 0; index3 < nativeArray1.Length; ++index3)
            {
              Entity entity = nativeArray1[index3];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              RenderedArea component = this.m_PrefabSystem.GetPrefab<AreaPrefab>(nativeArray2[index3]).GetComponent<RenderedArea>();
              float minNodeDistance = AreaUtils.GetMinNodeDistance(nativeArray3[index3].m_Type);
              float y = minNodeDistance * 2f;
              float x = math.clamp(component.m_Roundness, 0.01f, 0.99f) * minNodeDistance;
              entityManager = this.EntityManager;
              // ISSUE: reference to a compiler-generated field
              RenderedAreaData componentData = entityManager.GetComponentData<RenderedAreaData>(entity) with
              {
                m_HeightOffset = y,
                m_ExpandAmount = x * 0.5f,
                m_BatchIndex = this.m_ManagedBatchData.Count
              };
              entityManager = this.EntityManager;
              entityManager.SetComponentData<RenderedAreaData>(entity, componentData);
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              AreaBatchSystem.ManagedBatchData managedBatchData = new AreaBatchSystem.ManagedBatchData();
              // ISSUE: reference to a compiler-generated field
              managedBatchData.m_Material = new Material(component.m_Material);
              // ISSUE: reference to a compiler-generated field
              managedBatchData.m_Material.name = "Area batch (" + component.m_Material.name + ")";
              // ISSUE: reference to a compiler-generated field
              managedBatchData.m_Material.renderQueue = component.m_Material.shader.renderQueue;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              managedBatchData.m_Material.SetVector(this.m_AreaParameters, new Vector4(x, y, 0.0f, 0.0f));
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              managedBatchData.m_Material.SetFloat(this.m_DecalLayerMask, math.asfloat((int) component.m_DecalLayerMask));
              // ISSUE: reference to a compiler-generated field
              managedBatchData.m_RendererPriority = component.m_RendererPriority;
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              AreaBatchSystem.NativeBatchData nativeBatchData = new AreaBatchSystem.NativeBatchData();
              // ISSUE: reference to a compiler-generated field
              nativeBatchData.m_AreaMetaData = new UnsafeList<AreaBatchSystem.AreaMetaData>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
              // ISSUE: reference to a compiler-generated field
              nativeBatchData.m_VisibleIndices = new UnsafeList<int>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
              // ISSUE: reference to a compiler-generated field
              nativeBatchData.m_Prefab = entity;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              nativeBatchData.m_IsEnabled = this.m_RenderingSystem.IsShaderEnabled(managedBatchData.m_Material.shader);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              managedBatchData.m_VisibleIndices = new GraphicsBuffer(GraphicsBuffer.Target.Structured, nativeBatchData.m_VisibleIndices.Capacity, 4);
              // ISSUE: reference to a compiler-generated field
              this.m_ManagedBatchData.Add(managedBatchData);
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchData.Add(in nativeBatchData);
            }
          }
        }
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
    public AreaBatchSystem()
    {
    }

    private class ManagedBatchData
    {
      public GraphicsBuffer m_VisibleIndices;
      public Material m_Material;
      public int m_RendererPriority;
    }

    private struct NativeBatchData
    {
      public UnsafeList<AreaBatchSystem.AreaMetaData> m_AreaMetaData;
      public UnsafeList<int> m_VisibleIndices;
      public Bounds3 m_Bounds;
      public Entity m_Prefab;
      public bool m_VisibleUpdated;
      public bool m_BoundsUpdated;
      public bool m_VisibleIndicesUpdated;
      public bool m_IsEnabled;
    }

    [BurstCompile]
    private struct TreeCullingJob1 : IJob
    {
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      [ReadOnly]
      public float4 m_LodParameters;
      [ReadOnly]
      public float4 m_PrevLodParameters;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public float3 m_PrevCameraPosition;
      [ReadOnly]
      public float3 m_CameraDirection;
      [ReadOnly]
      public float3 m_PrevCameraDirection;
      [ReadOnly]
      public BoundsMask m_VisibleMask;
      [ReadOnly]
      public BoundsMask m_PrevVisibleMask;
      public NativeArray<int> m_NodeBuffer;
      public NativeArray<int> m_SubDataBuffer;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelQueue<AreaBatchSystem.CullingAction>.Writer m_ActionQueue;

      public void Execute()
      {
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
        AreaBatchSystem.TreeCullingIterator iterator = new AreaBatchSystem.TreeCullingIterator()
        {
          m_LodParameters = this.m_LodParameters,
          m_PrevLodParameters = this.m_PrevLodParameters,
          m_CameraPosition = this.m_CameraPosition,
          m_PrevCameraPosition = this.m_PrevCameraPosition,
          m_CameraDirection = this.m_CameraDirection,
          m_PrevCameraDirection = this.m_PrevCameraDirection,
          m_VisibleMask = this.m_VisibleMask,
          m_PrevVisibleMask = this.m_PrevVisibleMask,
          m_ActionQueue = this.m_ActionQueue
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AreaSearchTree.Iterate<AreaBatchSystem.TreeCullingIterator, int>(ref iterator, 3, this.m_NodeBuffer, this.m_SubDataBuffer);
      }
    }

    [BurstCompile]
    private struct TreeCullingJob2 : IJobParallelFor
    {
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      [ReadOnly]
      public float4 m_LodParameters;
      [ReadOnly]
      public float4 m_PrevLodParameters;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public float3 m_PrevCameraPosition;
      [ReadOnly]
      public float3 m_CameraDirection;
      [ReadOnly]
      public float3 m_PrevCameraDirection;
      [ReadOnly]
      public BoundsMask m_VisibleMask;
      [ReadOnly]
      public BoundsMask m_PrevVisibleMask;
      [ReadOnly]
      public NativeArray<int> m_NodeBuffer;
      [ReadOnly]
      public NativeArray<int> m_SubDataBuffer;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelQueue<AreaBatchSystem.CullingAction>.Writer m_ActionQueue;

      public void Execute(int index)
      {
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
        AreaBatchSystem.TreeCullingIterator iterator = new AreaBatchSystem.TreeCullingIterator()
        {
          m_LodParameters = this.m_LodParameters,
          m_PrevLodParameters = this.m_PrevLodParameters,
          m_CameraPosition = this.m_CameraPosition,
          m_PrevCameraPosition = this.m_PrevCameraPosition,
          m_CameraDirection = this.m_CameraDirection,
          m_PrevCameraDirection = this.m_PrevCameraDirection,
          m_VisibleMask = this.m_VisibleMask,
          m_PrevVisibleMask = this.m_PrevVisibleMask,
          m_ActionQueue = this.m_ActionQueue
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AreaSearchTree.Iterate<AreaBatchSystem.TreeCullingIterator, int>(ref iterator, this.m_SubDataBuffer[index], this.m_NodeBuffer[index]);
      }
    }

    private struct TreeCullingIterator : 
      INativeQuadTreeIteratorWithSubData<AreaSearchItem, QuadTreeBoundsXZ, int>,
      IUnsafeQuadTreeIteratorWithSubData<AreaSearchItem, QuadTreeBoundsXZ, int>
    {
      public float4 m_LodParameters;
      public float3 m_CameraPosition;
      public float3 m_CameraDirection;
      public float3 m_PrevCameraPosition;
      public float4 m_PrevLodParameters;
      public float3 m_PrevCameraDirection;
      public BoundsMask m_VisibleMask;
      public BoundsMask m_PrevVisibleMask;
      public NativeParallelQueue<AreaBatchSystem.CullingAction>.Writer m_ActionQueue;

      public bool Intersect(QuadTreeBoundsXZ bounds, ref int subData)
      {
        switch (subData)
        {
          case 1:
            // ISSUE: reference to a compiler-generated field
            BoundsMask boundsMask1 = this.m_VisibleMask & bounds.m_Mask;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance1 = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod1 = RenderingUtils.CalculateLod((float) (minDistance1 * minDistance1), this.m_LodParameters);
            if (boundsMask1 == (BoundsMask) 0 || lod1 < (int) bounds.m_MinLod)
              return false;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double maxDistance1 = (double) RenderingUtils.CalculateMaxDistance(bounds.m_Bounds, this.m_PrevCameraPosition, this.m_PrevCameraDirection, this.m_PrevLodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod2 = RenderingUtils.CalculateLod((float) (maxDistance1 * maxDistance1), this.m_PrevLodParameters);
            // ISSUE: reference to a compiler-generated field
            if ((boundsMask1 & ~this.m_PrevVisibleMask) != (BoundsMask) 0)
              return true;
            return lod2 < (int) bounds.m_MaxLod && lod1 > lod2;
          case 2:
            // ISSUE: reference to a compiler-generated field
            BoundsMask boundsMask2 = this.m_PrevVisibleMask & bounds.m_Mask;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance2 = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_PrevCameraPosition, this.m_PrevCameraDirection, this.m_PrevLodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod3 = RenderingUtils.CalculateLod((float) (minDistance2 * minDistance2), this.m_PrevLodParameters);
            if (boundsMask2 == (BoundsMask) 0 || lod3 < (int) bounds.m_MinLod)
              return false;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double maxDistance2 = (double) RenderingUtils.CalculateMaxDistance(bounds.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod4 = RenderingUtils.CalculateLod((float) (maxDistance2 * maxDistance2), this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            if ((boundsMask2 & ~this.m_VisibleMask) != (BoundsMask) 0)
              return true;
            return lod4 < (int) bounds.m_MaxLod && lod3 > lod4;
          default:
            // ISSUE: reference to a compiler-generated field
            BoundsMask boundsMask3 = this.m_VisibleMask & bounds.m_Mask;
            // ISSUE: reference to a compiler-generated field
            BoundsMask boundsMask4 = this.m_PrevVisibleMask & bounds.m_Mask;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float minDistance3 = RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance4 = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_PrevCameraPosition, this.m_PrevCameraDirection, this.m_PrevLodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod5 = RenderingUtils.CalculateLod(minDistance3 * minDistance3, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod6 = RenderingUtils.CalculateLod((float) (minDistance4 * minDistance4), this.m_PrevLodParameters);
            subData = 0;
            if (boundsMask3 != (BoundsMask) 0 && lod5 >= (int) bounds.m_MinLod)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              double maxDistance3 = (double) RenderingUtils.CalculateMaxDistance(bounds.m_Bounds, this.m_PrevCameraPosition, this.m_PrevCameraDirection, this.m_PrevLodParameters);
              // ISSUE: reference to a compiler-generated field
              int lod7 = RenderingUtils.CalculateLod((float) (maxDistance3 * maxDistance3), this.m_PrevLodParameters);
              // ISSUE: reference to a compiler-generated field
              subData |= math.select(0, 1, (boundsMask3 & ~this.m_PrevVisibleMask) != (BoundsMask) 0 || lod7 < (int) bounds.m_MaxLod && lod5 > lod7);
            }
            if (boundsMask4 != (BoundsMask) 0 && lod6 >= (int) bounds.m_MinLod)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              double maxDistance4 = (double) RenderingUtils.CalculateMaxDistance(bounds.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
              // ISSUE: reference to a compiler-generated field
              int lod8 = RenderingUtils.CalculateLod((float) (maxDistance4 * maxDistance4), this.m_LodParameters);
              // ISSUE: reference to a compiler-generated field
              subData |= math.select(0, 2, (boundsMask4 & ~this.m_VisibleMask) != (BoundsMask) 0 || lod8 < (int) bounds.m_MaxLod && lod6 > lod8);
            }
            return subData != 0;
        }
      }

      public void Iterate(QuadTreeBoundsXZ bounds, int subData, AreaSearchItem item)
      {
        switch (subData)
        {
          case 1:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance1 = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod1 = RenderingUtils.CalculateLod((float) (minDistance1 * minDistance1), this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            if ((this.m_VisibleMask & bounds.m_Mask) == (BoundsMask) 0 || lod1 < (int) bounds.m_MinLod)
              break;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance2 = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_PrevCameraPosition, this.m_PrevCameraDirection, this.m_PrevLodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod2 = RenderingUtils.CalculateLod((float) (minDistance2 * minDistance2), this.m_PrevLodParameters);
            // ISSUE: reference to a compiler-generated field
            if ((this.m_PrevVisibleMask & bounds.m_Mask) != (BoundsMask) 0 && lod2 >= (int) bounds.m_MaxLod)
              break;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ActionQueue.Enqueue(new AreaBatchSystem.CullingAction()
            {
              m_Item = item,
              m_PassedCulling = true
            });
            break;
          case 2:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance3 = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_PrevCameraPosition, this.m_PrevCameraDirection, this.m_PrevLodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod3 = RenderingUtils.CalculateLod((float) (minDistance3 * minDistance3), this.m_PrevLodParameters);
            // ISSUE: reference to a compiler-generated field
            if ((this.m_PrevVisibleMask & bounds.m_Mask) == (BoundsMask) 0 || lod3 < (int) bounds.m_MinLod)
              break;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance4 = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod4 = RenderingUtils.CalculateLod((float) (minDistance4 * minDistance4), this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            if ((this.m_VisibleMask & bounds.m_Mask) != (BoundsMask) 0 && lod4 >= (int) bounds.m_MaxLod)
              break;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ActionQueue.Enqueue(new AreaBatchSystem.CullingAction()
            {
              m_Item = item
            });
            break;
          default:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float minDistance5 = RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance6 = (double) RenderingUtils.CalculateMinDistance(bounds.m_Bounds, this.m_PrevCameraPosition, this.m_PrevCameraDirection, this.m_PrevLodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod5 = RenderingUtils.CalculateLod(minDistance5 * minDistance5, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod6 = RenderingUtils.CalculateLod((float) (minDistance6 * minDistance6), this.m_PrevLodParameters);
            // ISSUE: reference to a compiler-generated field
            bool flag1 = (this.m_VisibleMask & bounds.m_Mask) != (BoundsMask) 0 && lod5 >= (int) bounds.m_MinLod;
            // ISSUE: reference to a compiler-generated field
            bool flag2 = (this.m_PrevVisibleMask & bounds.m_Mask) != (BoundsMask) 0 && lod6 >= (int) bounds.m_MaxLod;
            if (flag1 == flag2)
              break;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ActionQueue.Enqueue(new AreaBatchSystem.CullingAction()
            {
              m_Item = item,
              m_PassedCulling = flag1
            });
            break;
        }
      }
    }

    [BurstCompile]
    private struct QueryCullingJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Areas.Batch> m_BatchType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> m_NodeType;
      [ReadOnly]
      public BufferTypeHandle<Triangle> m_TriangleType;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_PrefabAreaGeometryData;
      [ReadOnly]
      public float4 m_LodParameters;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public float3 m_CameraDirection;
      [ReadOnly]
      public BoundsMask m_VisibleMask;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelQueue<AreaBatchSystem.CullingAction>.Writer m_ActionQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Areas.Batch> nativeArray2 = chunk.GetNativeArray<Game.Areas.Batch>(ref this.m_BatchType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
          for (int index = 0; index < chunk.Count; ++index)
          {
            Entity area = nativeArray1[index];
            if (nativeArray2[index].m_AllocatedSize != 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_ActionQueue.Enqueue(new AreaBatchSystem.CullingAction()
              {
                m_Item = new AreaSearchItem(area, -1)
              });
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Game.Areas.Node> bufferAccessor1 = chunk.GetBufferAccessor<Game.Areas.Node>(ref this.m_NodeType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Triangle> bufferAccessor2 = chunk.GetBufferAccessor<Triangle>(ref this.m_TriangleType);
          BoundsMask boundsMask = BoundsMask.Debug | BoundsMask.NormalLayers | BoundsMask.NotOverridden | BoundsMask.NotWalkThrough;
          for (int index1 = 0; index1 < chunk.Count; ++index1)
          {
            Entity area = nativeArray1[index1];
            // ISSUE: variable of a compiler-generated type
            AreaBatchSystem.CullingAction cullingAction1;
            if (nativeArray2[index1].m_AllocatedSize != 0)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeParallelQueue<AreaBatchSystem.CullingAction>.Writer local = ref this.m_ActionQueue;
              // ISSUE: object of a compiler-generated type is created
              cullingAction1 = new AreaBatchSystem.CullingAction();
              // ISSUE: reference to a compiler-generated field
              cullingAction1.m_Item = new AreaSearchItem(area, -1);
              // ISSUE: variable of a compiler-generated type
              AreaBatchSystem.CullingAction cullingAction2 = cullingAction1;
              local.Enqueue(cullingAction2);
            }
            // ISSUE: reference to a compiler-generated field
            if ((this.m_VisibleMask & boundsMask) != (BoundsMask) 0)
            {
              PrefabRef prefabRef = nativeArray3[index1];
              DynamicBuffer<Game.Areas.Node> nodes = bufferAccessor1[index1];
              DynamicBuffer<Triangle> dynamicBuffer = bufferAccessor2[index1];
              // ISSUE: reference to a compiler-generated field
              AreaGeometryData areaData = this.m_PrefabAreaGeometryData[prefabRef.m_Prefab];
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                Triangle triangle = dynamicBuffer[index2];
                Triangle3 triangle3 = AreaUtils.GetTriangle3(nodes, triangle);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                double minDistance = (double) RenderingUtils.CalculateMinDistance(AreaUtils.GetBounds(triangle, triangle3, areaData), this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
                // ISSUE: reference to a compiler-generated field
                if (RenderingUtils.CalculateLod((float) (minDistance * minDistance), this.m_LodParameters) >= triangle.m_MinLod)
                {
                  // ISSUE: reference to a compiler-generated field
                  ref NativeParallelQueue<AreaBatchSystem.CullingAction>.Writer local = ref this.m_ActionQueue;
                  // ISSUE: object of a compiler-generated type is created
                  cullingAction1 = new AreaBatchSystem.CullingAction();
                  // ISSUE: reference to a compiler-generated field
                  cullingAction1.m_Item = new AreaSearchItem(area, index2);
                  // ISSUE: reference to a compiler-generated field
                  cullingAction1.m_PassedCulling = true;
                  // ISSUE: variable of a compiler-generated type
                  AreaBatchSystem.CullingAction cullingAction3 = cullingAction1;
                  local.Enqueue(cullingAction3);
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

    private struct AreaMetaData
    {
      public Entity m_Entity;
      public Bounds3 m_Bounds;
      public int m_StartIndex;
      public int m_VisibleCount;
    }

    private struct TriangleMetaData
    {
      public int m_Index;
      public bool m_IsVisible;
    }

    private struct TriangleSortData : IComparable<AreaBatchSystem.TriangleSortData>
    {
      public int m_Index;
      public int m_MinLod;

      public int CompareTo(AreaBatchSystem.TriangleSortData other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_MinLod - other.m_MinLod;
      }
    }

    private struct CullingAction
    {
      public AreaSearchItem m_Item;
      public bool m_PassedCulling;

      public override int GetHashCode() => this.m_Item.m_Area.GetHashCode();
    }

    private struct AllocationAction
    {
      public Entity m_Entity;
      public int m_TriangleCount;
    }

    [BurstCompile]
    private struct CullingActionJob : IJobParallelFor
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<RenderedAreaData> m_PrefabRenderedAreaData;
      [ReadOnly]
      public BufferLookup<Triangle> m_Triangles;
      [ReadOnly]
      public NativeParallelQueue<AreaBatchSystem.CullingAction>.Reader m_CullingActions;
      public NativeQueue<AreaBatchSystem.AllocationAction>.ParallelWriter m_AllocationActions;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Areas.Batch> m_BatchData;
      [NativeDisableParallelForRestriction]
      public NativeList<AreaBatchSystem.TriangleMetaData> m_TriangleMetaData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        NativeParallelQueue<AreaBatchSystem.CullingAction>.Enumerator enumerator = this.m_CullingActions.GetEnumerator(index);
        while (enumerator.MoveNext())
        {
          // ISSUE: variable of a compiler-generated type
          AreaBatchSystem.CullingAction current = enumerator.Current;
          // ISSUE: reference to a compiler-generated field
          if (current.m_PassedCulling)
          {
            // ISSUE: reference to a compiler-generated method
            this.PassedCulling(current);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.FailedCulling(current);
          }
        }
        enumerator.Dispose();
      }

      private void PassedCulling(AreaBatchSystem.CullingAction cullingAction)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ref Game.Areas.Batch local = ref this.m_BatchData.GetRefRW(cullingAction.m_Item.m_Area).ValueRW;
        if (local.m_VisibleCount != 0)
          return;
        local.m_VisibleCount = -1;
        RenderedAreaData componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRenderedAreaData.TryGetComponent(this.m_PrefabRefData[cullingAction.m_Item.m_Area].m_Prefab, out componentData))
        {
          local.m_BatchIndex = componentData.m_BatchIndex;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_AllocationActions.Enqueue(new AreaBatchSystem.AllocationAction()
          {
            m_Entity = cullingAction.m_Item.m_Area,
            m_TriangleCount = this.m_Triangles[cullingAction.m_Item.m_Area].Length
          });
        }
        else
          local.m_BatchIndex = -1;
      }

      private void FailedCulling(AreaBatchSystem.CullingAction cullingAction)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ref Game.Areas.Batch local1 = ref this.m_BatchData.GetRefRW(cullingAction.m_Item.m_Area).ValueRW;
        if (local1.m_AllocatedSize == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        if (cullingAction.m_Item.m_Triangle == -1)
        {
          if (local1.m_VisibleCount <= 0)
            return;
          for (int index = 0; index < local1.m_AllocatedSize; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_TriangleMetaData.ElementAt((int) local1.m_BatchAllocation.Begin + index).m_IsVisible = false;
          }
          local1.m_VisibleCount = 0;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_AllocationActions.Enqueue(new AreaBatchSystem.AllocationAction()
          {
            m_Entity = cullingAction.m_Item.m_Area
          });
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ref AreaBatchSystem.TriangleMetaData local2 = ref this.m_TriangleMetaData.ElementAt((int) local1.m_BatchAllocation.Begin + cullingAction.m_Item.m_Triangle);
          // ISSUE: reference to a compiler-generated field
          if (!local2.m_IsVisible)
            return;
          // ISSUE: reference to a compiler-generated field
          local2.m_IsVisible = false;
          if (--local1.m_VisibleCount != 0)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_AllocationActions.Enqueue(new AreaBatchSystem.AllocationAction()
          {
            m_Entity = cullingAction.m_Item.m_Area
          });
        }
      }
    }

    [BurstCompile]
    private struct BatchAllocationJob : IJob
    {
      public ComponentLookup<Game.Areas.Batch> m_BatchData;
      public NativeList<AreaBatchSystem.NativeBatchData> m_NativeBatchData;
      public NativeList<AreaBatchSystem.TriangleMetaData> m_TriangleMetaData;
      public NativeList<AreaTriangleData> m_AreaTriangleData;
      public NativeList<AreaColorData> m_AreaColorData;
      public NativeList<NativeHeapBlock> m_UpdatedTriangles;
      public NativeQueue<AreaBatchSystem.AllocationAction> m_AllocationActions;
      public NativeHeapAllocator m_AreaBufferAllocator;
      public NativeReference<int> m_AllocationCount;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        AreaBatchSystem.AllocationAction allocationAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_AllocationActions.TryDequeue(out allocationAction))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          RefRW<Game.Areas.Batch> refRw = this.m_BatchData.GetRefRW(allocationAction.m_Entity);
          ref Game.Areas.Batch local1 = ref refRw.ValueRW;
          // ISSUE: reference to a compiler-generated field
          ref AreaBatchSystem.NativeBatchData local2 = ref this.m_NativeBatchData.ElementAt(local1.m_BatchIndex);
          // ISSUE: reference to a compiler-generated field
          local2.m_BoundsUpdated = true;
          // ISSUE: reference to a compiler-generated field
          if (allocationAction.m_TriangleCount != 0)
          {
            if (local1.m_AllocatedSize == 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.Allocate(ref local1, allocationAction.m_TriangleCount);
              // ISSUE: reference to a compiler-generated field
              local1.m_MetaIndex = local2.m_AreaMetaData.Length;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              local2.m_AreaMetaData.Add(new AreaBatchSystem.AreaMetaData()
              {
                m_Entity = allocationAction.m_Entity,
                m_StartIndex = (int) local1.m_BatchAllocation.Begin
              });
              // ISSUE: reference to a compiler-generated field
              ++this.m_AllocationCount.Value;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              ref AreaBatchSystem.AreaMetaData local3 = ref local2.m_AreaMetaData.ElementAt(local1.m_MetaIndex);
              // ISSUE: reference to a compiler-generated field
              local3.m_VisibleCount = 0;
              // ISSUE: reference to a compiler-generated field
              if (allocationAction.m_TriangleCount != local1.m_AllocatedSize)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_AreaBufferAllocator.Release(local1.m_BatchAllocation);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.Allocate(ref local1, allocationAction.m_TriangleCount);
                // ISSUE: reference to a compiler-generated field
                local3.m_StartIndex = (int) local1.m_BatchAllocation.Begin;
              }
            }
            // ISSUE: reference to a compiler-generated field
            this.m_UpdatedTriangles.Add(in local1.m_BatchAllocation);
          }
          else if (local1.m_VisibleCount == 0)
          {
            // ISSUE: reference to a compiler-generated field
            --this.m_AllocationCount.Value;
            // ISSUE: reference to a compiler-generated field
            this.m_AreaBufferAllocator.Release(local1.m_BatchAllocation);
            local1.m_BatchAllocation = new NativeHeapBlock();
            local1.m_AllocatedSize = 0;
            // ISSUE: reference to a compiler-generated field
            local2.m_AreaMetaData.RemoveAtSwapBack(local1.m_MetaIndex);
            // ISSUE: reference to a compiler-generated field
            local2.m_VisibleUpdated = true;
            // ISSUE: reference to a compiler-generated field
            if (local1.m_MetaIndex < local2.m_AreaMetaData.Length)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              refRw = this.m_BatchData.GetRefRW(local2.m_AreaMetaData[local1.m_MetaIndex].m_Entity);
              refRw.ValueRW.m_MetaIndex = local1.m_MetaIndex;
            }
          }
        }
      }

      private void Allocate(ref Game.Areas.Batch batch, int allocationSize)
      {
        // ISSUE: reference to a compiler-generated field
        batch.m_BatchAllocation = this.m_AreaBufferAllocator.Allocate((uint) allocationSize);
        batch.m_AllocatedSize = allocationSize;
        if (!batch.m_BatchAllocation.Empty)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AreaBufferAllocator.Resize(this.m_AreaBufferAllocator.Size + 1048576U / AreaBatchSystem.GetTriangleSize());
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TriangleMetaData.ResizeUninitialized((int) this.m_AreaBufferAllocator.Size);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTriangleData.ResizeUninitialized((int) this.m_AreaBufferAllocator.Size);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AreaColorData.ResizeUninitialized((int) this.m_AreaBufferAllocator.Size);
        // ISSUE: reference to a compiler-generated field
        batch.m_BatchAllocation = this.m_AreaBufferAllocator.Allocate((uint) allocationSize);
      }
    }

    [BurstCompile]
    private struct TriangleUpdateJob : IJobParallelFor
    {
      [ReadOnly]
      public ComponentLookup<Area> m_AreaData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<RenderedAreaData> m_PrefabRenderedAreaData;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_Nodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_Triangles;
      [ReadOnly]
      public BufferLookup<Expand> m_Expands;
      [ReadOnly]
      public NativeParallelQueue<AreaBatchSystem.CullingAction>.Reader m_CullingActions;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Areas.Batch> m_BatchData;
      [NativeDisableParallelForRestriction]
      public NativeList<AreaBatchSystem.TriangleMetaData> m_TriangleMetaData;
      [NativeDisableParallelForRestriction]
      public NativeList<AreaTriangleData> m_AreaTriangleData;
      [NativeDisableParallelForRestriction]
      public NativeList<AreaBatchSystem.NativeBatchData> m_NativeBatchData;
      [NativeDisableContainerSafetyRestriction]
      private NativeParallelHashMap<AreaBatchSystem.TriangleUpdateJob.Border, int2> m_BorderMap;
      [NativeDisableContainerSafetyRestriction]
      private NativeList<int2> m_AdjacentNodes;
      [NativeDisableContainerSafetyRestriction]
      private NativeList<Game.Areas.Node> m_NodeList;
      [NativeDisableContainerSafetyRestriction]
      private NativeList<AreaBatchSystem.TriangleSortData> m_TriangleSortData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        NativeParallelQueue<AreaBatchSystem.CullingAction>.Enumerator enumerator = this.m_CullingActions.GetEnumerator(index);
        while (enumerator.MoveNext())
        {
          // ISSUE: variable of a compiler-generated type
          AreaBatchSystem.CullingAction current = enumerator.Current;
          // ISSUE: reference to a compiler-generated field
          if (current.m_PassedCulling)
          {
            // ISSUE: reference to a compiler-generated method
            this.PassedCulling(current);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.FailedCulling(current);
          }
        }
        enumerator.Dispose();
      }

      private void PassedCulling(AreaBatchSystem.CullingAction cullingAction)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ref Game.Areas.Batch local1 = ref this.m_BatchData.GetRefRW(cullingAction.m_Item.m_Area).ValueRW;
        if (local1.m_AllocatedSize == 0)
          return;
        if (local1.m_VisibleCount == -1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.GenerateTriangleData(cullingAction.m_Item.m_Area, ref local1);
          local1.m_VisibleCount = 0;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ref AreaBatchSystem.TriangleMetaData local2 = ref this.m_TriangleMetaData.ElementAt((int) local1.m_BatchAllocation.Begin + cullingAction.m_Item.m_Triangle);
        // ISSUE: reference to a compiler-generated field
        if (local2.m_IsVisible)
          return;
        // ISSUE: reference to a compiler-generated field
        local2.m_IsVisible = true;
        ++local1.m_VisibleCount;
        // ISSUE: reference to a compiler-generated field
        ref AreaBatchSystem.NativeBatchData local3 = ref this.m_NativeBatchData.ElementAt(local1.m_BatchIndex);
        // ISSUE: reference to a compiler-generated field
        ref AreaBatchSystem.AreaMetaData local4 = ref local3.m_AreaMetaData.ElementAt(local1.m_MetaIndex);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (local2.m_Index < local4.m_VisibleCount)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        local4.m_VisibleCount = local2.m_Index + 1;
        // ISSUE: reference to a compiler-generated field
        if (local3.m_VisibleUpdated)
          return;
        // ISSUE: reference to a compiler-generated field
        local3.m_VisibleUpdated = true;
      }

      private void FailedCulling(AreaBatchSystem.CullingAction cullingAction)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ref Game.Areas.Batch local1 = ref this.m_BatchData.GetRefRW(cullingAction.m_Item.m_Area).ValueRW;
        // ISSUE: reference to a compiler-generated field
        if (local1.m_AllocatedSize == 0 || cullingAction.m_Item.m_Triangle == -1)
          return;
        // ISSUE: reference to a compiler-generated field
        ref AreaBatchSystem.NativeBatchData local2 = ref this.m_NativeBatchData.ElementAt(local1.m_BatchIndex);
        // ISSUE: reference to a compiler-generated field
        ref AreaBatchSystem.AreaMetaData local3 = ref local2.m_AreaMetaData.ElementAt(local1.m_MetaIndex);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        AreaBatchSystem.TriangleMetaData triangleMetaData1 = this.m_TriangleMetaData[(int) local1.m_BatchAllocation.Begin + cullingAction.m_Item.m_Triangle];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (triangleMetaData1.m_Index != local3.m_VisibleCount - 1)
          return;
        // ISSUE: reference to a compiler-generated field
        local3.m_VisibleCount = 0;
        for (int index = 0; index < local1.m_AllocatedSize; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          AreaBatchSystem.TriangleMetaData triangleMetaData2 = this.m_TriangleMetaData[(int) local1.m_BatchAllocation.Begin + index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          local3.m_VisibleCount = math.select(local3.m_VisibleCount, triangleMetaData2.m_Index + 1, triangleMetaData2.m_IsVisible && triangleMetaData2.m_Index >= local3.m_VisibleCount);
        }
        // ISSUE: reference to a compiler-generated field
        if (local2.m_VisibleUpdated)
          return;
        // ISSUE: reference to a compiler-generated field
        local2.m_VisibleUpdated = true;
      }

      private void GenerateTriangleData(Entity entity, ref Game.Areas.Batch batch)
      {
        // ISSUE: reference to a compiler-generated field
        Area area = this.m_AreaData[entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Areas.Node> node1 = this.m_Nodes[entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Triangle> triangle = this.m_Triangles[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        RenderedAreaData renderedAreaData = this.m_PrefabRenderedAreaData[this.m_PrefabRefData[entity].m_Prefab];
        float4 offsetDir = new float4(0.0f, 0.0f, 0.0f, 1f);
        bool flag = false;
        Owner componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.TryGetComponent(entity, out componentData1))
        {
          Game.Objects.Transform componentData2;
          Owner componentData3;
          // ISSUE: reference to a compiler-generated field
          for (; !this.m_TransformData.TryGetComponent(componentData1.m_Owner, out componentData2); componentData1 = componentData3)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_OwnerData.TryGetComponent(componentData1.m_Owner, out componentData3))
              goto label_5;
          }
          offsetDir.xy = componentData2.m_Position.xz;
          offsetDir.zw = math.forward(componentData2.m_Rotation).xz;
          flag = true;
        }
label_5:
        if (!flag)
        {
          Game.Areas.Node node2;
          if (node1.Length >= 1)
          {
            ref float4 local = ref offsetDir;
            node2 = node1[0];
            float2 xz = node2.m_Position.xz;
            local.xy = xz;
          }
          if (node1.Length >= 2)
          {
            node2 = node1[1];
            float2 forward = node2.m_Position.xz - offsetDir.xy;
            if (MathUtils.TryNormalize(ref forward))
              offsetDir.zw = MathUtils.Left(forward);
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ref AreaBatchSystem.AreaMetaData local1 = ref this.m_NativeBatchData.ElementAt(batch.m_BatchIndex).m_AreaMetaData.ElementAt(batch.m_MetaIndex);
        bool isCounterClockwise = (area.m_Flags & AreaFlags.CounterClockwise) != 0;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_BorderMap.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_BorderMap = new NativeParallelHashMap<AreaBatchSystem.TriangleUpdateJob.Border, int2>(node1.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_AdjacentNodes.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_AdjacentNodes = new NativeList<int2>(node1.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TriangleSortData.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TriangleSortData = new NativeList<AreaBatchSystem.TriangleSortData>(triangle.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        }
        // ISSUE: reference to a compiler-generated method
        this.SortTriangles(triangle, ref batch);
        DynamicBuffer<Expand> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Expands.TryGetBuffer(entity, out bufferData))
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_NodeList.IsCreated)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_NodeList = new NativeList<Game.Areas.Node>(node1.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          }
          // ISSUE: reference to a compiler-generated method
          this.FillExpandedNodes(node1, bufferData);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.AddBorders<NativeList<Game.Areas.Node>>(this.m_NodeList, isCounterClockwise);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.AddNodes<NativeList<Game.Areas.Node>>(this.m_NodeList, triangle, isCounterClockwise);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          local1.m_Bounds = this.AddTriangles<NativeList<Game.Areas.Node>>(this.m_NodeList, triangle, renderedAreaData, (int) batch.m_BatchAllocation.Begin, offsetDir, isCounterClockwise);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.AddBorders<DynamicBuffer<Game.Areas.Node>>(node1, isCounterClockwise);
          // ISSUE: reference to a compiler-generated method
          this.AddNodes<DynamicBuffer<Game.Areas.Node>>(node1, triangle, isCounterClockwise);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          local1.m_Bounds = this.AddTriangles<DynamicBuffer<Game.Areas.Node>>(node1, triangle, renderedAreaData, (int) batch.m_BatchAllocation.Begin, offsetDir, isCounterClockwise);
        }
      }

      private void SortTriangles(DynamicBuffer<Triangle> triangles, ref Game.Areas.Batch batch)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TriangleSortData.ResizeUninitialized(triangles.Length);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_TriangleSortData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_TriangleSortData[index] = new AreaBatchSystem.TriangleSortData()
          {
            m_Index = index,
            m_MinLod = triangles[index].m_MinLod
          };
        }
        // ISSUE: reference to a compiler-generated field
        this.m_TriangleSortData.Sort<AreaBatchSystem.TriangleSortData>();
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_TriangleSortData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          AreaBatchSystem.TriangleSortData triangleSortData = this.m_TriangleSortData[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_TriangleMetaData[(int) batch.m_BatchAllocation.Begin + triangleSortData.m_Index] = new AreaBatchSystem.TriangleMetaData()
          {
            m_Index = index
          };
        }
      }

      private void FillExpandedNodes(DynamicBuffer<Game.Areas.Node> nodes, DynamicBuffer<Expand> expands)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_NodeList.ResizeUninitialized(nodes.Length);
        for (int index = 0; index < nodes.Length; ++index)
        {
          Game.Areas.Node node = nodes[index];
          Expand expand = expands[index];
          node.m_Position.xz += expand.m_Offset;
          // ISSUE: reference to a compiler-generated field
          this.m_NodeList[index] = node;
        }
      }

      private void AddBorders<TNodeList>(TNodeList nodes, bool isCounterClockwise) where TNodeList : INativeList<Game.Areas.Node>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BorderMap.Clear();
        float3 float3 = nodes[0].m_Position;
        for (int index = 1; index < nodes.Length; ++index)
        {
          float3 position = nodes[index].m_Position;
          if (isCounterClockwise)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_BorderMap.Add(new AreaBatchSystem.TriangleUpdateJob.Border()
            {
              m_StartPos = position,
              m_EndPos = float3
            }, new int2(index, index - 1));
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_BorderMap.Add(new AreaBatchSystem.TriangleUpdateJob.Border()
            {
              m_StartPos = float3,
              m_EndPos = position
            }, new int2(index - 1, index));
          }
          float3 = position;
        }
        float3 position1 = nodes[0].m_Position;
        if (isCounterClockwise)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_BorderMap.Add(new AreaBatchSystem.TriangleUpdateJob.Border()
          {
            m_StartPos = position1,
            m_EndPos = float3
          }, new int2(0, nodes.Length - 1));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_BorderMap.Add(new AreaBatchSystem.TriangleUpdateJob.Border()
          {
            m_StartPos = float3,
            m_EndPos = position1
          }, new int2(nodes.Length - 1, 0));
        }
      }

      private void AddNodes<TNodeList>(
        TNodeList nodes,
        DynamicBuffer<Triangle> triangles,
        bool isCounterClockwise)
        where TNodeList : INativeList<Game.Areas.Node>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AdjacentNodes.ResizeUninitialized(nodes.Length);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_AdjacentNodes.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_AdjacentNodes[index] = (int2) index;
        }
        for (int index = 0; index < triangles.Length; ++index)
        {
          Triangle triangle = triangles[index];
          // ISSUE: reference to a compiler-generated field
          int2 adjacentNode1 = this.m_AdjacentNodes[triangle.m_Indices.x];
          // ISSUE: reference to a compiler-generated field
          int2 adjacentNode2 = this.m_AdjacentNodes[triangle.m_Indices.y];
          // ISSUE: reference to a compiler-generated field
          int2 adjacentNode3 = this.m_AdjacentNodes[triangle.m_Indices.z];
          // ISSUE: reference to a compiler-generated method
          this.CheckBorder<TNodeList>(ref adjacentNode1, ref adjacentNode2, nodes, triangle.m_Indices.x, triangle.m_Indices.y, isCounterClockwise);
          // ISSUE: reference to a compiler-generated method
          this.CheckBorder<TNodeList>(ref adjacentNode2, ref adjacentNode3, nodes, triangle.m_Indices.y, triangle.m_Indices.z, isCounterClockwise);
          // ISSUE: reference to a compiler-generated method
          this.CheckBorder<TNodeList>(ref adjacentNode3, ref adjacentNode1, nodes, triangle.m_Indices.z, triangle.m_Indices.x, isCounterClockwise);
          // ISSUE: reference to a compiler-generated field
          this.m_AdjacentNodes[triangle.m_Indices.x] = adjacentNode1;
          // ISSUE: reference to a compiler-generated field
          this.m_AdjacentNodes[triangle.m_Indices.y] = adjacentNode2;
          // ISSUE: reference to a compiler-generated field
          this.m_AdjacentNodes[triangle.m_Indices.z] = adjacentNode3;
        }
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_AdjacentNodes.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          int2 adjacentNode = this.m_AdjacentNodes[index1];
          bool2 x1 = adjacentNode != index1;
          if (math.any(x1))
          {
            if (x1.x)
            {
              for (int index2 = 0; index2 < nodes.Length; ++index2)
              {
                // ISSUE: reference to a compiler-generated field
                int x2 = this.m_AdjacentNodes[adjacentNode.x].x;
                if (x2 != adjacentNode.x)
                {
                  if (x2 == index1 || x2 == -1)
                  {
                    adjacentNode.x = -1;
                    break;
                  }
                  adjacentNode.x = x2;
                }
                else
                  break;
              }
            }
            if (x1.y)
            {
              for (int index3 = 0; index3 < nodes.Length; ++index3)
              {
                // ISSUE: reference to a compiler-generated field
                int y = this.m_AdjacentNodes[adjacentNode.y].y;
                if (y != adjacentNode.y)
                {
                  if (y == index1 || y == -1)
                  {
                    adjacentNode.y = -1;
                    break;
                  }
                  adjacentNode.y = y;
                }
                else
                  break;
              }
            }
            // ISSUE: reference to a compiler-generated field
            this.m_AdjacentNodes[index1] = adjacentNode;
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_AdjacentNodes.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          int2 adjacentNode = this.m_AdjacentNodes[index];
          // ISSUE: reference to a compiler-generated field
          this.m_AdjacentNodes[index] = math.select(math.select(adjacentNode + new int2(-1, 1), new int2(nodes.Length - 1, 0), adjacentNode == new int2(0, nodes.Length - 1)), (int2) index, adjacentNode == -1);
        }
      }

      private void CheckBorder<TNodeList>(
        ref int2 adjacentA,
        ref int2 adjacentB,
        TNodeList nodes,
        int nodeA,
        int nodeB,
        bool isCounterClockwise)
        where TNodeList : INativeList<Game.Areas.Node>
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AreaBatchSystem.TriangleUpdateJob.Border key = new AreaBatchSystem.TriangleUpdateJob.Border()
        {
          m_StartPos = nodes[nodeA].m_Position,
          m_EndPos = nodes[nodeB].m_Position
        };
        int2 int2;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_BorderMap.TryGetValue(key, out int2))
          return;
        if (isCounterClockwise)
        {
          adjacentB.x = int2.y;
          adjacentA.y = int2.x;
        }
        else
        {
          adjacentA.x = int2.x;
          adjacentB.y = int2.y;
        }
      }

      private Bounds3 AddTriangles<TNodeList>(
        TNodeList nodes,
        DynamicBuffer<Triangle> triangles,
        RenderedAreaData renderedAreaData,
        int triangleOffset,
        float4 offsetDir,
        bool isCounterClockwise)
        where TNodeList : INativeList<Game.Areas.Node>
      {
        Bounds3 bounds3_1 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        for (int index = 0; index < triangles.Length; ++index)
        {
          Triangle triangle = triangles[index];
          // ISSUE: reference to a compiler-generated field
          int2 adjacentNode1 = this.m_AdjacentNodes[triangle.m_Indices.x];
          // ISSUE: reference to a compiler-generated field
          int2 adjacentNode2 = this.m_AdjacentNodes[triangle.m_Indices.y];
          // ISSUE: reference to a compiler-generated field
          int2 adjacentNode3 = this.m_AdjacentNodes[triangle.m_Indices.z];
          // ISSUE: reference to a compiler-generated field
          int x1 = this.m_AdjacentNodes[adjacentNode1.x].x;
          // ISSUE: reference to a compiler-generated field
          int x2 = this.m_AdjacentNodes[adjacentNode2.x].x;
          // ISSUE: reference to a compiler-generated field
          int x3 = this.m_AdjacentNodes[adjacentNode3.x].x;
          // ISSUE: reference to a compiler-generated field
          int y1 = this.m_AdjacentNodes[adjacentNode1.y].y;
          // ISSUE: reference to a compiler-generated field
          int y2 = this.m_AdjacentNodes[adjacentNode2.y].y;
          // ISSUE: reference to a compiler-generated field
          int y3 = this.m_AdjacentNodes[adjacentNode3.y].y;
          AreaTriangleData areaTriangleData = new AreaTriangleData();
          areaTriangleData.m_APos = AreaUtils.GetExpandedNode<TNodeList>(nodes, triangle.m_Indices.x, adjacentNode1.x, adjacentNode1.y, renderedAreaData.m_ExpandAmount, isCounterClockwise);
          areaTriangleData.m_BPos = AreaUtils.GetExpandedNode<TNodeList>(nodes, triangle.m_Indices.y, adjacentNode2.x, adjacentNode2.y, renderedAreaData.m_ExpandAmount, isCounterClockwise);
          areaTriangleData.m_CPos = AreaUtils.GetExpandedNode<TNodeList>(nodes, triangle.m_Indices.z, adjacentNode3.x, adjacentNode3.y, renderedAreaData.m_ExpandAmount, isCounterClockwise);
          areaTriangleData.m_APrevXZ = AreaUtils.GetExpandedNode<TNodeList>(nodes, adjacentNode1.x, x1, triangle.m_Indices.x, renderedAreaData.m_ExpandAmount, isCounterClockwise).xz;
          areaTriangleData.m_BPrevXZ = AreaUtils.GetExpandedNode<TNodeList>(nodes, adjacentNode2.x, x2, triangle.m_Indices.y, renderedAreaData.m_ExpandAmount, isCounterClockwise).xz;
          areaTriangleData.m_CPrevXZ = AreaUtils.GetExpandedNode<TNodeList>(nodes, adjacentNode3.x, x3, triangle.m_Indices.z, renderedAreaData.m_ExpandAmount, isCounterClockwise).xz;
          areaTriangleData.m_ANextXZ = AreaUtils.GetExpandedNode<TNodeList>(nodes, adjacentNode1.y, triangle.m_Indices.x, y1, renderedAreaData.m_ExpandAmount, isCounterClockwise).xz;
          areaTriangleData.m_BNextXZ = AreaUtils.GetExpandedNode<TNodeList>(nodes, adjacentNode2.y, triangle.m_Indices.y, y2, renderedAreaData.m_ExpandAmount, isCounterClockwise).xz;
          areaTriangleData.m_CNextXZ = AreaUtils.GetExpandedNode<TNodeList>(nodes, adjacentNode3.y, triangle.m_Indices.z, y3, renderedAreaData.m_ExpandAmount, isCounterClockwise).xz;
          float3 x4 = new float3(areaTriangleData.m_APos.y, areaTriangleData.m_BPos.y, areaTriangleData.m_CPos.y);
          areaTriangleData.m_YMinMax.x = triangle.m_HeightRange.min - renderedAreaData.m_HeightOffset + math.cmin(x4);
          areaTriangleData.m_YMinMax.y = triangle.m_HeightRange.max + renderedAreaData.m_HeightOffset + math.cmax(x4);
          areaTriangleData.m_OffsetDir = offsetDir;
          areaTriangleData.m_LodDistanceFactor = RenderingUtils.CalculateDistanceFactor(triangle.m_MinLod);
          Bounds3 bounds3_2 = MathUtils.Bounds(new Triangle3(areaTriangleData.m_APos, areaTriangleData.m_BPos, areaTriangleData.m_CPos));
          bounds3_2.min.y = areaTriangleData.m_YMinMax.x;
          bounds3_2.max.y = areaTriangleData.m_YMinMax.y;
          bounds3_1 |= bounds3_2;
          // ISSUE: reference to a compiler-generated field
          ref AreaBatchSystem.TriangleMetaData local = ref this.m_TriangleMetaData.ElementAt(triangleOffset + index);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_AreaTriangleData[triangleOffset + local.m_Index] = areaTriangleData;
        }
        return bounds3_1;
      }

      private struct Border : IEquatable<AreaBatchSystem.TriangleUpdateJob.Border>
      {
        public float3 m_StartPos;
        public float3 m_EndPos;

        public bool Equals(AreaBatchSystem.TriangleUpdateJob.Border other)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return this.m_StartPos.Equals(other.m_StartPos) & this.m_EndPos.Equals(other.m_EndPos);
        }

        public override int GetHashCode() => this.m_StartPos.GetHashCode();
      }
    }

    [BurstCompile]
    private struct VisibleUpdateJob : IJobParallelFor
    {
      [NativeDisableParallelForRestriction]
      public NativeList<AreaBatchSystem.NativeBatchData> m_NativeBatchData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ref AreaBatchSystem.NativeBatchData local1 = ref this.m_NativeBatchData.ElementAt(index);
        // ISSUE: reference to a compiler-generated field
        if (local1.m_BoundsUpdated)
        {
          // ISSUE: reference to a compiler-generated field
          local1.m_Bounds = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
          // ISSUE: reference to a compiler-generated field
          local1.m_BoundsUpdated = false;
          // ISSUE: reference to a compiler-generated field
          for (int index1 = 0; index1 < local1.m_AreaMetaData.Length; ++index1)
          {
            // ISSUE: reference to a compiler-generated field
            ref AreaBatchSystem.AreaMetaData local2 = ref local1.m_AreaMetaData.ElementAt(index1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            local1.m_Bounds |= local2.m_Bounds;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!local1.m_VisibleUpdated)
          return;
        // ISSUE: reference to a compiler-generated field
        local1.m_VisibleIndices.Clear();
        // ISSUE: reference to a compiler-generated field
        local1.m_VisibleIndicesUpdated = true;
        // ISSUE: reference to a compiler-generated field
        local1.m_VisibleUpdated = false;
        // ISSUE: reference to a compiler-generated field
        if (!local1.m_IsEnabled)
          return;
        // ISSUE: reference to a compiler-generated field
        for (int index2 = 0; index2 < local1.m_AreaMetaData.Length; ++index2)
        {
          // ISSUE: reference to a compiler-generated field
          ref AreaBatchSystem.AreaMetaData local3 = ref local1.m_AreaMetaData.ElementAt(index2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          local1.m_Bounds |= (float3) local3.m_StartIndex;
          // ISSUE: reference to a compiler-generated field
          for (int index3 = 0; index3 < local3.m_VisibleCount; ++index3)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            local1.m_VisibleIndices.Add(local3.m_StartIndex + index3);
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AreaGeometryData>(true);
      }
    }
  }
}
