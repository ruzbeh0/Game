// Decompiled with JetBrains decompiler
// Type: Game.Rendering.PreCullingSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Creatures;
using Game.Effects;
using Game.Events;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Routes;
using Game.Serialization;
using Game.Simulation;
using Game.Tools;
using Game.Vehicles;
using Game.Zones;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class PreCullingSystem : GameSystemBase, IPostDeserialize
  {
    private RenderingSystem m_RenderingSystem;
    private UndergroundViewSystem m_UndergroundViewSystem;
    private BatchMeshSystem m_BatchMeshSystem;
    private BatchDataSystem m_BatchDataSystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private ToolSystem m_ToolSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private TerrainSystem m_TerrainSystem;
    private EntityQuery m_InitializeQuery;
    private EntityQuery m_EventQuery;
    private EntityQuery m_CullingInfoQuery;
    private EntityQuery m_TempQuery;
    private float3 m_PrevCameraPosition;
    private float3 m_PrevCameraDirection;
    private float4 m_PrevLodParameters;
    private BoundsMask m_PrevVisibleMask;
    private PreCullingSystem.QueryFlags m_PrevQueryFlags;
    private Dictionary<PreCullingSystem.QueryFlags, EntityQuery> m_CullingQueries;
    private Dictionary<PreCullingSystem.QueryFlags, EntityQuery> m_RelativeQueries;
    private Dictionary<PreCullingSystem.QueryFlags, EntityQuery> m_RemoveQueries;
    private NativeList<PreCullingData> m_CullingData;
    private NativeList<PreCullingData> m_UpdatedData;
    private Entity m_FadeContainer;
    private JobHandle m_WriteDependencies;
    private JobHandle m_ReadDependencies;
    private bool m_ResetPrevious;
    private bool m_Loaded;
    private PreCullingSystem.TypeHandle __TypeHandle;

    public BoundsMask visibleMask { get; private set; }

    public BoundsMask becameVisible { get; private set; }

    public BoundsMask becameHidden { get; private set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UndergroundViewSystem = this.World.GetOrCreateSystemManaged<UndergroundViewSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchMeshSystem = this.World.GetOrCreateSystemManaged<BatchMeshSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchDataSystem = this.World.GetOrCreateSystemManaged<BatchDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_InitializeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<CullingInfo>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<BatchesUpdated>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_EventQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Common.Event>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<RentersUpdated>(),
          ComponentType.ReadOnly<ColorUpdated>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_CullingInfoQuery = this.GetEntityQuery(ComponentType.ReadOnly<CullingInfo>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadWrite<CullingInfo>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_CullingQueries = new Dictionary<PreCullingSystem.QueryFlags, EntityQuery>();
      // ISSUE: reference to a compiler-generated field
      this.m_RelativeQueries = new Dictionary<PreCullingSystem.QueryFlags, EntityQuery>();
      // ISSUE: reference to a compiler-generated field
      this.m_RemoveQueries = new Dictionary<PreCullingSystem.QueryFlags, EntityQuery>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrevCameraDirection = math.forward();
      // ISSUE: reference to a compiler-generated field
      this.m_PrevLodParameters = (float4) 1f;
      // ISSUE: reference to a compiler-generated field
      this.m_CullingData = new NativeList<PreCullingData>(10000, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedData = new NativeList<PreCullingData>(10000, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_FadeContainer = this.EntityManager.CreateEntity(ComponentType.ReadWrite<MeshBatch>(), ComponentType.ReadWrite<FadeBatch>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_CullingData.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedData.Dispose();
      base.OnDestroy();
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.GetBuffer<MeshBatch>(this.m_FadeContainer).Clear();
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.GetBuffer<FadeBatch>(this.m_FadeContainer).Clear();
      // ISSUE: reference to a compiler-generated method
      this.InitializeCullingData();
      // ISSUE: reference to a compiler-generated method
      this.ResetCulling();
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    public void ResetCulling() => this.m_ResetPrevious = true;

    public Entity GetFadeContainer() => this.m_FadeContainer;

    public NativeList<PreCullingData> GetCullingData(bool readOnly, out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = readOnly ? this.m_WriteDependencies : JobHandle.CombineDependencies(this.m_WriteDependencies, this.m_ReadDependencies);
      // ISSUE: reference to a compiler-generated field
      return this.m_CullingData;
    }

    public NativeList<PreCullingData> GetUpdatedData(bool readOnly, out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = readOnly ? this.m_WriteDependencies : JobHandle.CombineDependencies(this.m_WriteDependencies, this.m_ReadDependencies);
      // ISSUE: reference to a compiler-generated field
      return this.m_UpdatedData;
    }

    public void AddCullingDataReader(JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies = JobHandle.CombineDependencies(this.m_ReadDependencies, dependencies);
    }

    public void AddCullingDataWriter(JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = dependencies;
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
      bool loaded = this.GetLoaded();
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies.Complete();
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
      BoundsMask boundsMask = BoundsMask.NormalLayers;
      // ISSUE: reference to a compiler-generated field
      if (this.m_UndergroundViewSystem.pipelinesOn)
        boundsMask |= BoundsMask.PipelineLayer;
      // ISSUE: reference to a compiler-generated field
      if (this.m_UndergroundViewSystem.subPipelinesOn)
        boundsMask |= BoundsMask.SubPipelineLayer;
      // ISSUE: reference to a compiler-generated field
      if (this.m_UndergroundViewSystem.waterwaysOn)
        boundsMask |= BoundsMask.WaterwayLayer;
      // ISSUE: reference to a compiler-generated field
      if (this.m_RenderingSystem.markersVisible)
        boundsMask |= BoundsMask.Debug;
      // ISSUE: reference to a compiler-generated field
      if (this.m_ResetPrevious)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PrevCameraPosition = float3_1;
        // ISSUE: reference to a compiler-generated field
        this.m_PrevCameraDirection = float3_2;
        // ISSUE: reference to a compiler-generated field
        this.m_PrevLodParameters = float4;
        // ISSUE: reference to a compiler-generated field
        this.m_PrevVisibleMask = (BoundsMask) 0;
        this.visibleMask = boundsMask;
        this.becameVisible = boundsMask;
        this.becameHidden = (BoundsMask) 0;
      }
      else
      {
        this.visibleMask = boundsMask;
        // ISSUE: reference to a compiler-generated field
        this.becameVisible = boundsMask & ~this.m_PrevVisibleMask;
        // ISSUE: reference to a compiler-generated field
        this.becameHidden = this.m_PrevVisibleMask & ~boundsMask;
      }
      // ISSUE: reference to a compiler-generated field
      int length1 = this.m_CullingData.Length;
      NativeParallelQueue<PreCullingSystem.CullingAction> nativeParallelQueue = new NativeParallelQueue<PreCullingSystem.CullingAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeReference<int> nativeReference = new NativeReference<int>(length1, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<PreCullingSystem.OverflowAction> nativeQueue = new NativeQueue<PreCullingSystem.OverflowAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeArray<int> nativeArray1 = new NativeArray<int>(1536, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
      NativeArray<int> nativeArray2 = new NativeArray<int>(1536, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
      JobHandle dependencies1;
      JobHandle dependencies2;
      JobHandle dependencies3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PreCullingSystem.TreeCullingJob1 jobData1 = new PreCullingSystem.TreeCullingJob1()
      {
        m_StaticObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies1),
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies2),
        m_LaneSearchTree = this.m_NetSearchSystem.GetLaneSearchTree(true, out dependencies3),
        m_LodParameters = float4,
        m_PrevLodParameters = this.m_PrevLodParameters,
        m_CameraPosition = float3_1,
        m_PrevCameraPosition = this.m_PrevCameraPosition,
        m_CameraDirection = float3_2,
        m_PrevCameraDirection = this.m_PrevCameraDirection,
        m_VisibleMask = boundsMask,
        m_PrevVisibleMask = this.m_PrevVisibleMask,
        m_NodeBuffer = nativeArray1,
        m_SubDataBuffer = nativeArray2,
        m_ActionQueue = nativeParallelQueue.AsWriter()
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
      PreCullingSystem.TreeCullingJob2 jobData2 = new PreCullingSystem.TreeCullingJob2()
      {
        m_StaticObjectSearchTree = jobData1.m_StaticObjectSearchTree,
        m_NetSearchTree = jobData1.m_NetSearchTree,
        m_LaneSearchTree = jobData1.m_LaneSearchTree,
        m_LodParameters = float4,
        m_PrevLodParameters = this.m_PrevLodParameters,
        m_CameraPosition = float3_1,
        m_PrevCameraPosition = this.m_PrevCameraPosition,
        m_CameraDirection = float3_2,
        m_PrevCameraDirection = this.m_PrevCameraDirection,
        m_VisibleMask = boundsMask,
        m_PrevVisibleMask = this.m_PrevVisibleMask,
        m_NodeBuffer = nativeArray1,
        m_SubDataBuffer = nativeArray2,
        m_ActionQueue = nativeParallelQueue.AsWriter()
      };
      JobHandle jobHandle1 = jobData1.Schedule<PreCullingSystem.TreeCullingJob1>(3, 1, JobHandle.CombineDependencies(dependencies1, dependencies2, dependencies3));
      int length2 = nativeArray1.Length;
      JobHandle dependsOn1 = jobHandle1;
      JobHandle jobHandle2 = jobData2.Schedule<PreCullingSystem.TreeCullingJob2>(length2, 1, dependsOn1);
      JobHandle.ScheduleBatchedJobs();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BatchMeshSystem.CompleteCaching();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      PreCullingSystem.QueryFlags queryFlags = this.GetQueryFlags();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Marker_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Orphan_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Marker_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stack_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Overridden_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_BatchesUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PreCullingSystem.InitializeCullingJob jobData3 = new PreCullingSystem.InitializeCullingJob()
      {
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UpdatedType = this.__TypeHandle.__Game_Common_Updated_RO_ComponentTypeHandle,
        m_BatchesUpdatedType = this.__TypeHandle.__Game_Common_BatchesUpdated_RO_ComponentTypeHandle,
        m_OverriddenType = this.__TypeHandle.__Game_Common_Overridden_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_StackType = this.__TypeHandle.__Game_Objects_Stack_RO_ComponentTypeHandle,
        m_ObjectMarkerType = this.__TypeHandle.__Game_Objects_Marker_RO_ComponentTypeHandle,
        m_OutsideConnectionType = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle,
        m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
        m_NodeGeometryType = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentTypeHandle,
        m_EdgeGeometryType = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle,
        m_StartNodeGeometryType = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle,
        m_EndNodeGeometryType = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle,
        m_CompositionType = this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle,
        m_OrphanType = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_UtilityLaneType = this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentTypeHandle,
        m_NetMarkerType = this.__TypeHandle.__Game_Net_Marker_RO_ComponentTypeHandle,
        m_ZoneBlockType = this.__TypeHandle.__Game_Zones_Block_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_TransformFrameType = this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferTypeHandle,
        m_CullingInfoType = this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabStackData = this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup,
        m_PrefabLaneGeometryData = this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup,
        m_PrefabUtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_PrefabCompositionMeshRef = this.__TypeHandle.__Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup,
        m_PrefabCompositionMeshData = this.__TypeHandle.__Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup,
        m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_UpdateAll = loaded,
        m_UnspawnedVisible = this.m_RenderingSystem.unspawnedVisible,
        m_DilatedUtilityTypes = this.m_UndergroundViewSystem.utilityTypes,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_CullingData = this.m_CullingData
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_ColorUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_RentersUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      PreCullingSystem.EventCullingJob jobData4 = new PreCullingSystem.EventCullingJob()
      {
        m_RentersUpdatedType = this.__TypeHandle.__Game_Buildings_RentersUpdated_RO_ComponentTypeHandle,
        m_ColorUpdatedType = this.__TypeHandle.__Game_Routes_ColorUpdated_RO_ComponentTypeHandle,
        m_CullingInfoData = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_RouteVehicles = this.__TypeHandle.__Game_Routes_RouteVehicle_RO_BufferLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_CullingData = this.m_CullingData
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      PreCullingSystem.QueryCullingJob jobData5 = new PreCullingSystem.QueryCullingJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_TransformFrameType = this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferTypeHandle,
        m_CullingInfoType = this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentTypeHandle,
        m_LodParameters = float4,
        m_CameraPosition = float3_1,
        m_CameraDirection = float3_2,
        m_FrameIndex = this.m_RenderingSystem.frameIndex,
        m_FrameTime = this.m_RenderingSystem.frameTime,
        m_VisibleMask = boundsMask,
        m_ActionQueue = nativeParallelQueue.AsWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Applied_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      PreCullingSystem.QueryRemoveJob jobData6 = new PreCullingSystem.QueryRemoveJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_AppliedType = this.__TypeHandle.__Game_Common_Applied_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_TransformFrameType = this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferTypeHandle,
        m_CullingInfoType = this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentTypeHandle,
        m_ActionQueue = nativeParallelQueue.AsWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PreCullingSystem.RelativeCullingJob jobData7 = new PreCullingSystem.RelativeCullingJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_CurrentVehicleType = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle,
        m_CullingInfoData = this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentLookup,
        m_LodParameters = float4,
        m_CameraPosition = float3_1,
        m_CameraDirection = float3_2,
        m_VisibleMask = boundsMask,
        m_ActionQueue = nativeParallelQueue.AsWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Static_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stack_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PreCullingSystem.TempCullingJob jobData8 = new PreCullingSystem.TempCullingJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_InterpolatedTransformType = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_StackType = this.__TypeHandle.__Game_Objects_Stack_RO_ComponentTypeHandle,
        m_StaticType = this.__TypeHandle.__Game_Objects_Static_RO_ComponentTypeHandle,
        m_StoppedType = this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabStackData = this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup,
        m_CullingInfoData = this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentLookup,
        m_LodParameters = float4,
        m_CameraPosition = float3_1,
        m_CameraDirection = float3_2,
        m_VisibleMask = boundsMask,
        m_TerrainHeightData = jobData3.m_TerrainHeightData,
        m_ActionQueue = nativeParallelQueue.AsWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery query = loaded ? this.m_CullingInfoQuery : this.m_InitializeQuery;
      // ISSUE: reference to a compiler-generated method
      EntityQuery cullingQuery = this.GetCullingQuery(queryFlags);
      // ISSUE: reference to a compiler-generated method
      EntityQuery relativeQuery = this.GetRelativeQuery(queryFlags);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      EntityQuery removeQuery = this.GetRemoveQuery(this.m_PrevQueryFlags & ~queryFlags);
      JobHandle dependsOn2 = jobData3.ScheduleParallel<PreCullingSystem.InitializeCullingJob>(query, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn3 = jobData4.Schedule<PreCullingSystem.EventCullingJob>(this.m_EventQuery, dependsOn2);
      JobHandle dependsOn4 = jobData5.ScheduleParallel<PreCullingSystem.QueryCullingJob>(cullingQuery, dependsOn3);
      JobHandle dependsOn5 = jobData6.ScheduleParallel<PreCullingSystem.QueryRemoveJob>(removeQuery, dependsOn4);
      JobHandle jobHandle3 = jobData7.ScheduleParallel<PreCullingSystem.RelativeCullingJob>(relativeQuery, dependsOn5);
      // ISSUE: reference to a compiler-generated field
      EntityQuery tempQuery = this.m_TempQuery;
      JobHandle dependsOn6 = jobHandle3;
      JobHandle jobHandle4 = jobData8.ScheduleParallel<PreCullingSystem.TempCullingJob>(tempQuery, dependsOn6);
      // ISSUE: reference to a compiler-generated field
      if (this.m_ResetPrevious || this.becameHidden != (BoundsMask) 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        JobHandle job1 = new PreCullingSystem.VerifyVisibleJob()
        {
          m_CullingInfoData = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup,
          m_LodParameters = float4,
          m_CameraPosition = float3_1,
          m_CameraDirection = float3_2,
          m_VisibleMask = boundsMask,
          m_CullingData = this.m_CullingData,
          m_ActionQueue = nativeParallelQueue.AsWriter()
        }.Schedule<PreCullingSystem.VerifyVisibleJob>(length1, 16, jobHandle4);
        // ISSUE: reference to a compiler-generated field
        this.m_WriteDependencies = JobHandle.CombineDependencies(jobHandle2, job1);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_WriteDependencies = JobHandle.CombineDependencies(jobHandle2, jobHandle4);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PreCullingSystem.CullingActionJob jobData9 = new PreCullingSystem.CullingActionJob()
      {
        m_CullingActions = nativeParallelQueue.AsReader(),
        m_OverflowActions = nativeQueue.AsParallelWriter(),
        m_CullingInfo = this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentLookup,
        m_CullingData = this.m_CullingData,
        m_CullingDataIndex = nativeReference
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PreCullingSystem.ResizeCullingDataJob jobData10 = new PreCullingSystem.ResizeCullingDataJob()
      {
        m_CullingDataIndex = nativeReference,
        m_CullingData = this.m_CullingData,
        m_UpdatedData = this.m_UpdatedData,
        m_OverflowActions = nativeQueue
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Effects_EnabledEffect_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshColor_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Emissive_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Skeleton_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Animated_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_OnFire_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneCondition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneColor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeColor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeColor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Extension_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Relative_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Plant_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Color_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_ObjectGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Object_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_BatchesUpdated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Applied_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      PreCullingSystem.FilterUpdatesJob jobData11 = new PreCullingSystem.FilterUpdatesJob()
      {
        m_CreatedData = this.__TypeHandle.__Game_Common_Created_RO_ComponentLookup,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_AppliedData = this.__TypeHandle.__Game_Common_Applied_RO_ComponentLookup,
        m_BatchesUpdatedData = this.__TypeHandle.__Game_Common_BatchesUpdated_RO_ComponentLookup,
        m_InterpolatedTransformData = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_ObjectData = this.__TypeHandle.__Game_Objects_Object_RO_ComponentLookup,
        m_ObjectGeometryData = this.__TypeHandle.__Game_Objects_ObjectGeometry_RO_ComponentLookup,
        m_ObjectColorData = this.__TypeHandle.__Game_Objects_Color_RO_ComponentLookup,
        m_PlantData = this.__TypeHandle.__Game_Objects_Plant_RO_ComponentLookup,
        m_TreeData = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup,
        m_RelativeData = this.__TypeHandle.__Game_Objects_Relative_RO_ComponentLookup,
        m_DamagedData = this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_ExtensionData = this.__TypeHandle.__Game_Buildings_Extension_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_NodeColorData = this.__TypeHandle.__Game_Net_NodeColor_RO_ComponentLookup,
        m_EdgeColorData = this.__TypeHandle.__Game_Net_EdgeColor_RO_ComponentLookup,
        m_LaneColorData = this.__TypeHandle.__Game_Net_LaneColor_RO_ComponentLookup,
        m_LaneConditionData = this.__TypeHandle.__Game_Net_LaneCondition_RO_ComponentLookup,
        m_ZoneData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
        m_OnFireData = this.__TypeHandle.__Game_Events_OnFire_RO_ComponentLookup,
        m_AnimatedData = this.__TypeHandle.__Game_Rendering_Animated_RO_BufferLookup,
        m_SkeletonData = this.__TypeHandle.__Game_Rendering_Skeleton_RO_BufferLookup,
        m_EmissiveData = this.__TypeHandle.__Game_Rendering_Emissive_RO_BufferLookup,
        m_MeshColorData = this.__TypeHandle.__Game_Rendering_MeshColor_RO_BufferLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_EffectInstances = this.__TypeHandle.__Game_Effects_EnabledEffect_RO_BufferLookup,
        m_TimerDelta = this.m_RenderingSystem.lodTimerDelta,
        m_CullingData = this.m_CullingData,
        m_UpdatedCullingData = this.m_UpdatedData.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle5 = jobData9.Schedule<PreCullingSystem.CullingActionJob>(nativeParallelQueue.HashRange, 1, this.m_WriteDependencies);
      JobHandle inputDeps = jobData10.Schedule<PreCullingSystem.ResizeCullingDataJob>(jobHandle5);
      // ISSUE: reference to a compiler-generated field
      NativeList<PreCullingData> cullingData = this.m_CullingData;
      JobHandle dependsOn7 = inputDeps;
      JobHandle jobHandle6 = jobData11.Schedule<PreCullingSystem.FilterUpdatesJob, PreCullingData>(cullingData, 16, dependsOn7);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddLaneSearchTreeReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle4);
      nativeParallelQueue.Dispose(jobHandle5);
      nativeReference.Dispose(inputDeps);
      nativeQueue.Dispose(inputDeps);
      nativeArray1.Dispose(jobHandle2);
      nativeArray2.Dispose(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_PrevCameraPosition = float3_1;
      // ISSUE: reference to a compiler-generated field
      this.m_PrevCameraDirection = float3_2;
      // ISSUE: reference to a compiler-generated field
      this.m_PrevLodParameters = float4;
      // ISSUE: reference to a compiler-generated field
      this.m_PrevVisibleMask = boundsMask;
      // ISSUE: reference to a compiler-generated field
      this.m_PrevQueryFlags = queryFlags;
      // ISSUE: reference to a compiler-generated field
      this.m_ResetPrevious = false;
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = jobHandle6;
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies = new JobHandle();
      this.Dependency = jobHandle6;
    }

    private void InitializeCullingData()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CullingData.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CullingData.Add(new PreCullingData()
      {
        m_Entity = this.m_FadeContainer,
        m_Flags = PreCullingFlags.PassedCulling | PreCullingFlags.NearCamera | PreCullingFlags.FadeContainer
      });
    }

    private EntityQuery GetCullingQuery(PreCullingSystem.QueryFlags flags)
    {
      EntityQuery entityQuery;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CullingQueries.TryGetValue(flags, out entityQuery))
      {
        List<ComponentType> componentTypeList1 = new List<ComponentType>()
        {
          ComponentType.ReadOnly<Moving>(),
          ComponentType.ReadOnly<Stopped>(),
          ComponentType.ReadOnly<Updated>()
        };
        List<ComponentType> componentTypeList2 = new List<ComponentType>()
        {
          ComponentType.ReadOnly<Relative>(),
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        };
        if ((flags & PreCullingSystem.QueryFlags.Unspawned) == (PreCullingSystem.QueryFlags) 0)
          componentTypeList2.Add(ComponentType.ReadOnly<Unspawned>());
        if ((flags & PreCullingSystem.QueryFlags.Zones) != (PreCullingSystem.QueryFlags) 0)
          componentTypeList1.Add(ComponentType.ReadOnly<Game.Zones.Block>());
        else
          componentTypeList2.Add(ComponentType.ReadOnly<Game.Zones.Block>());
        entityQuery = this.GetEntityQuery(new EntityQueryDesc()
        {
          All = new ComponentType[1]
          {
            ComponentType.ReadWrite<CullingInfo>()
          },
          Any = componentTypeList1.ToArray(),
          None = componentTypeList2.ToArray()
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CullingQueries.Add(flags, entityQuery);
      }
      return entityQuery;
    }

    private EntityQuery GetRelativeQuery(PreCullingSystem.QueryFlags flags)
    {
      flags &= PreCullingSystem.QueryFlags.Unspawned;
      EntityQuery entityQuery;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RelativeQueries.TryGetValue(flags, out entityQuery))
      {
        List<ComponentType> componentTypeList = new List<ComponentType>()
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        };
        if ((flags & PreCullingSystem.QueryFlags.Unspawned) == (PreCullingSystem.QueryFlags) 0)
          componentTypeList.Add(ComponentType.ReadOnly<Unspawned>());
        entityQuery = this.GetEntityQuery(new EntityQueryDesc()
        {
          All = new ComponentType[2]
          {
            ComponentType.ReadOnly<Relative>(),
            ComponentType.ReadWrite<CullingInfo>()
          },
          None = componentTypeList.ToArray()
        });
        // ISSUE: reference to a compiler-generated field
        this.m_RelativeQueries.Add(flags, entityQuery);
      }
      return entityQuery;
    }

    private EntityQuery GetRemoveQuery(PreCullingSystem.QueryFlags flags)
    {
      EntityQuery entityQuery;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RemoveQueries.TryGetValue(flags, out entityQuery))
      {
        List<ComponentType> componentTypeList = new List<ComponentType>()
        {
          ComponentType.ReadOnly<Deleted>()
        };
        if ((flags & PreCullingSystem.QueryFlags.Zones) != (PreCullingSystem.QueryFlags) 0)
          componentTypeList.Add(ComponentType.ReadOnly<Game.Zones.Block>());
        if ((flags & PreCullingSystem.QueryFlags.Unspawned) != (PreCullingSystem.QueryFlags) 0)
          componentTypeList.Add(ComponentType.ReadOnly<Unspawned>());
        entityQuery = this.GetEntityQuery(new EntityQueryDesc()
        {
          All = new ComponentType[1]
          {
            ComponentType.ReadWrite<CullingInfo>()
          },
          Any = componentTypeList.ToArray()
        });
        // ISSUE: reference to a compiler-generated field
        this.m_RemoveQueries.Add(flags, entityQuery);
      }
      return entityQuery;
    }

    private PreCullingSystem.QueryFlags GetQueryFlags()
    {
      // ISSUE: variable of a compiler-generated type
      PreCullingSystem.QueryFlags queryFlags = (PreCullingSystem.QueryFlags) 0;
      // ISSUE: reference to a compiler-generated field
      if (this.m_RenderingSystem.unspawnedVisible)
        queryFlags |= PreCullingSystem.QueryFlags.Unspawned;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool != null && this.m_ToolSystem.activeTool.requireZones)
        queryFlags |= PreCullingSystem.QueryFlags.Zones;
      return queryFlags;
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
    public PreCullingSystem()
    {
    }

    [Flags]
    private enum QueryFlags
    {
      Unspawned = 1,
      Zones = 2,
    }

    [BurstCompile]
    private struct TreeCullingJob1 : IJobParallelFor
    {
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_StaticObjectSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_LaneSearchTree;
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
      [NativeDisableParallelForRestriction]
      public NativeArray<int> m_NodeBuffer;
      [NativeDisableParallelForRestriction]
      public NativeArray<int> m_SubDataBuffer;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelQueue<PreCullingSystem.CullingAction>.Writer m_ActionQueue;

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
        PreCullingSystem.TreeCullingIterator iterator = new PreCullingSystem.TreeCullingIterator()
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
        int num = this.m_NodeBuffer.Length / 3;
        switch (index)
        {
          case 0:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_StaticObjectSearchTree.Iterate<PreCullingSystem.TreeCullingIterator, int>(ref iterator, 3, this.m_NodeBuffer.GetSubArray(0, num), this.m_SubDataBuffer.GetSubArray(0, num));
            break;
          case 1:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_NetSearchTree.Iterate<PreCullingSystem.TreeCullingIterator, int>(ref iterator, 3, this.m_NodeBuffer.GetSubArray(num, num), this.m_SubDataBuffer.GetSubArray(num, num));
            break;
          case 2:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_LaneSearchTree.Iterate<PreCullingSystem.TreeCullingIterator, int>(ref iterator, 3, this.m_NodeBuffer.GetSubArray(num * 2, num), this.m_SubDataBuffer.GetSubArray(num * 2, num));
            break;
        }
      }
    }

    [BurstCompile]
    private struct TreeCullingJob2 : IJobParallelFor
    {
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_StaticObjectSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_LaneSearchTree;
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
      public NativeParallelQueue<PreCullingSystem.CullingAction>.Writer m_ActionQueue;

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
        PreCullingSystem.TreeCullingIterator iterator = new PreCullingSystem.TreeCullingIterator()
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
        switch (index * 3 / this.m_NodeBuffer.Length)
        {
          case 0:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_StaticObjectSearchTree.Iterate<PreCullingSystem.TreeCullingIterator, int>(ref iterator, this.m_SubDataBuffer[index], this.m_NodeBuffer[index]);
            break;
          case 1:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_NetSearchTree.Iterate<PreCullingSystem.TreeCullingIterator, int>(ref iterator, this.m_SubDataBuffer[index], this.m_NodeBuffer[index]);
            break;
          case 2:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_LaneSearchTree.Iterate<PreCullingSystem.TreeCullingIterator, int>(ref iterator, this.m_SubDataBuffer[index], this.m_NodeBuffer[index]);
            break;
        }
      }
    }

    private struct TreeCullingIterator : 
      INativeQuadTreeIteratorWithSubData<Entity, QuadTreeBoundsXZ, int>,
      IUnsafeQuadTreeIteratorWithSubData<Entity, QuadTreeBoundsXZ, int>
    {
      public float4 m_LodParameters;
      public float3 m_CameraPosition;
      public float3 m_CameraDirection;
      public float3 m_PrevCameraPosition;
      public float4 m_PrevLodParameters;
      public float3 m_PrevCameraDirection;
      public BoundsMask m_VisibleMask;
      public BoundsMask m_PrevVisibleMask;
      public NativeParallelQueue<PreCullingSystem.CullingAction>.Writer m_ActionQueue;

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

      public void Iterate(QuadTreeBoundsXZ bounds, int subData, Entity entity)
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
            this.m_ActionQueue.Enqueue(new PreCullingSystem.CullingAction()
            {
              m_Entity = entity,
              m_Flags = PreCullingSystem.ActionFlags.PassedCulling,
              m_UpdateFrame = (sbyte) -1
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
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ActionQueue.Enqueue(new PreCullingSystem.CullingAction()
            {
              m_Entity = entity,
              m_Flags = (this.m_VisibleMask & bounds.m_Mask) != (BoundsMask) 0 ? PreCullingSystem.ActionFlags.CrossFade : (PreCullingSystem.ActionFlags) 0,
              m_UpdateFrame = (sbyte) -1
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
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            PreCullingSystem.CullingAction cullingAction = new PreCullingSystem.CullingAction()
            {
              m_Entity = entity,
              m_UpdateFrame = -1
            };
            if (flag1)
            {
              // ISSUE: reference to a compiler-generated field
              cullingAction.m_Flags = PreCullingSystem.ActionFlags.PassedCulling;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if ((this.m_VisibleMask & bounds.m_Mask) != (BoundsMask) 0)
              {
                // ISSUE: reference to a compiler-generated field
                cullingAction.m_Flags = PreCullingSystem.ActionFlags.CrossFade;
              }
            }
            // ISSUE: reference to a compiler-generated field
            this.m_ActionQueue.Enqueue(cullingAction);
            break;
        }
      }
    }

    [BurstCompile]
    public struct InitializeCullingJob : IJobChunk
    {
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Updated> m_UpdatedType;
      [ReadOnly]
      public ComponentTypeHandle<BatchesUpdated> m_BatchesUpdatedType;
      [ReadOnly]
      public ComponentTypeHandle<Overridden> m_OverriddenType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Stack> m_StackType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Marker> m_ObjectMarkerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Node> m_NodeType;
      [ReadOnly]
      public ComponentTypeHandle<Edge> m_EdgeType;
      [ReadOnly]
      public ComponentTypeHandle<NodeGeometry> m_NodeGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> m_EdgeGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<StartNodeGeometry> m_StartNodeGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<EndNodeGeometry> m_EndNodeGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<Composition> m_CompositionType;
      [ReadOnly]
      public ComponentTypeHandle<Orphan> m_OrphanType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.UtilityLane> m_UtilityLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Marker> m_NetMarkerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Zones.Block> m_ZoneBlockType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<TransformFrame> m_TransformFrameType;
      public ComponentTypeHandle<CullingInfo> m_CullingInfoType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<StackData> m_PrefabStackData;
      [ReadOnly]
      public ComponentLookup<NetLaneGeometryData> m_PrefabLaneGeometryData;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> m_PrefabUtilityLaneData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshRef> m_PrefabCompositionMeshRef;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshData> m_PrefabCompositionMeshData;
      [ReadOnly]
      public ComponentLookup<NetData> m_PrefabNetData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public bool m_UpdateAll;
      [ReadOnly]
      public bool m_UnspawnedVisible;
      [ReadOnly]
      public bool m_Loaded;
      [ReadOnly]
      public UtilityTypes m_DilatedUtilityTypes;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [NativeDisableParallelForRestriction]
      public NativeList<PreCullingData> m_CullingData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CullingInfo> nativeArray1 = chunk.GetNativeArray<CullingInfo>(ref this.m_CullingInfoType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TransformFrame> bufferAccessor = chunk.GetBufferAccessor<TransformFrame>(ref this.m_TransformFrameType);
        // ISSUE: reference to a compiler-generated field
        bool isUpdated = chunk.Has<Updated>(ref this.m_UpdatedType);
        // ISSUE: reference to a compiler-generated field
        bool batchesUpdated = chunk.Has<BatchesUpdated>(ref this.m_BatchesUpdatedType);
        if (bufferAccessor.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          // ISSUE: reference to a compiler-generated field
          uint index1 = chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool flag = chunk.Has<Game.Objects.Marker>(ref this.m_ObjectMarkerType) && !chunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool remove = !this.m_UnspawnedVisible && chunk.Has<Unspawned>(ref this.m_UnspawnedType);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            ref CullingInfo local = ref nativeArray1.ElementAt<CullingInfo>(index2);
            // ISSUE: reference to a compiler-generated field
            if (this.m_UpdateAll | isUpdated)
            {
              PrefabRef prefabRef = nativeArray2[index2];
              local.m_Bounds = new Bounds3();
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabObjectGeometryData.HasComponent(prefabRef.m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
                local.m_Radius = math.length(math.max(-objectGeometryData.m_Bounds.min, objectGeometryData.m_Bounds.max));
                local.m_Mask = BoundsMask.Debug;
                // ISSUE: reference to a compiler-generated field
                if (!flag || this.m_EditorMode)
                {
                  MeshLayer layers = objectGeometryData.m_Layers;
                  Owner owner;
                  CollectionUtils.TryGet<Owner>(nativeArray3, index2, out owner);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  local.m_Mask |= CommonUtils.GetBoundsMask(Game.Net.SearchSystem.GetLayers(owner, new Game.Net.UtilityLane(), layers, ref this.m_PrefabRefData, ref this.m_PrefabNetData, ref this.m_PrefabNetGeometryData));
                }
                local.m_MinLod = (byte) objectGeometryData.m_MinLod;
              }
              else
              {
                local.m_Radius = 1f;
                // ISSUE: reference to a compiler-generated field
                local.m_Mask = this.m_EditorMode ? BoundsMask.Debug | BoundsMask.NormalLayers : BoundsMask.Debug;
                local.m_MinLod = (byte) RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float3(2f)));
              }
            }
            // ISSUE: reference to a compiler-generated method
            this.SetFlags(ref local, (int) index1, isUpdated, batchesUpdated, remove);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Transform> nativeArray4 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Net.Node> nativeArray5 = chunk.GetNativeArray<Game.Net.Node>(ref this.m_NodeType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Edge> nativeArray6 = chunk.GetNativeArray<Edge>(ref this.m_EdgeType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Curve> nativeArray7 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Zones.Block> nativeArray8 = chunk.GetNativeArray<Game.Zones.Block>(ref this.m_ZoneBlockType);
          if (nativeArray4.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Owner> nativeArray9 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Stack> nativeArray10 = chunk.GetNativeArray<Stack>(ref this.m_StackType);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            bool flag1 = chunk.Has<Game.Objects.Marker>(ref this.m_ObjectMarkerType) && !chunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType);
            // ISSUE: reference to a compiler-generated field
            bool flag2 = chunk.Has<Overridden>(ref this.m_OverriddenType);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            bool remove = !this.m_UnspawnedVisible && chunk.Has<Unspawned>(ref this.m_UnspawnedType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              ref CullingInfo local = ref nativeArray1.ElementAt<CullingInfo>(index);
              // ISSUE: reference to a compiler-generated field
              if (this.m_UpdateAll | isUpdated)
              {
                Transform transform = nativeArray4[index];
                PrefabRef prefabRef = nativeArray2[index];
                ObjectGeometryData componentData1;
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
                {
                  StackData componentData2;
                  // ISSUE: reference to a compiler-generated field
                  if (nativeArray10.Length != 0 && this.m_PrefabStackData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
                  {
                    Stack stack = nativeArray10[index];
                    local.m_Bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, stack, componentData1, componentData2);
                    local.m_Radius = 0.0f;
                  }
                  else
                  {
                    local.m_Bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, componentData1);
                    local.m_Radius = 0.0f;
                  }
                  if ((componentData1.m_Flags & Game.Objects.GeometryFlags.HasBase) != Game.Objects.GeometryFlags.None)
                  {
                    // ISSUE: reference to a compiler-generated field
                    local.m_Bounds.min.y = math.min(local.m_Bounds.min.y, TerrainUtils.GetHeightRange(ref this.m_TerrainHeightData, local.m_Bounds).min);
                  }
                  local.m_Mask = BoundsMask.Debug;
                  // ISSUE: reference to a compiler-generated field
                  if (!flag2 && (!flag1 || this.m_EditorMode))
                  {
                    MeshLayer layers = componentData1.m_Layers;
                    Owner owner;
                    CollectionUtils.TryGet<Owner>(nativeArray9, index, out owner);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    local.m_Mask |= CommonUtils.GetBoundsMask(Game.Net.SearchSystem.GetLayers(owner, new Game.Net.UtilityLane(), layers, ref this.m_PrefabRefData, ref this.m_PrefabNetData, ref this.m_PrefabNetGeometryData));
                  }
                  local.m_MinLod = (byte) componentData1.m_MinLod;
                }
                else
                {
                  local.m_Bounds = new Bounds3(transform.m_Position - 1f, transform.m_Position + 1f);
                  local.m_Radius = 0.0f;
                  // ISSUE: reference to a compiler-generated field
                  local.m_Mask = this.m_EditorMode ? BoundsMask.Debug | BoundsMask.NormalLayers : BoundsMask.Debug;
                  local.m_MinLod = (byte) RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float3(2f)));
                }
              }
              // ISSUE: reference to a compiler-generated method
              this.SetFlags(ref local, -1, isUpdated, batchesUpdated, remove);
            }
          }
          else if (nativeArray5.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<NodeGeometry> nativeArray11 = chunk.GetNativeArray<NodeGeometry>(ref this.m_NodeGeometryType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Orphan> nativeArray12 = chunk.GetNativeArray<Orphan>(ref this.m_OrphanType);
            // ISSUE: reference to a compiler-generated field
            bool flag = chunk.Has<Game.Net.Marker>(ref this.m_NetMarkerType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              ref CullingInfo local = ref nativeArray1.ElementAt<CullingInfo>(index);
              // ISSUE: reference to a compiler-generated field
              if (this.m_UpdateAll | isUpdated)
              {
                if (nativeArray11.Length != 0)
                {
                  NodeGeometry nodeGeometry = nativeArray11[index];
                  local.m_Bounds = nodeGeometry.m_Bounds;
                  local.m_Radius = 0.0f;
                  local.m_Mask = BoundsMask.Debug;
                  if (nativeArray12.Length != 0)
                  {
                    Orphan orphan = nativeArray12[index];
                    // ISSUE: reference to a compiler-generated field
                    NetCompositionData netCompositionData = this.m_PrefabCompositionData[orphan.m_Composition];
                    local.m_MinLod = (byte) netCompositionData.m_MinLod;
                    NetCompositionMeshData componentData;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if ((!flag || this.m_EditorMode) && this.m_PrefabCompositionMeshData.TryGetComponent(this.m_PrefabCompositionMeshRef[orphan.m_Composition].m_Mesh, out componentData))
                      local.m_Mask |= componentData.m_DefaultLayers == (MeshLayer) 0 ? BoundsMask.NormalLayers : CommonUtils.GetBoundsMask(componentData.m_DefaultLayers);
                  }
                  else
                    local.m_MinLod = (byte) RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float2(2f)));
                }
                else
                {
                  Game.Net.Node node = nativeArray5[index];
                  local.m_Bounds = new Bounds3(node.m_Position - 1f, node.m_Position + 1f);
                  local.m_Radius = 0.0f;
                  // ISSUE: reference to a compiler-generated field
                  local.m_Mask = this.m_EditorMode ? BoundsMask.Debug | BoundsMask.NormalLayers : BoundsMask.Debug;
                  local.m_MinLod = (byte) RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float2(2f)));
                }
              }
              // ISSUE: reference to a compiler-generated method
              this.SetFlags(ref local, -1, isUpdated, batchesUpdated, false);
            }
          }
          else if (nativeArray6.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<EdgeGeometry> nativeArray13 = chunk.GetNativeArray<EdgeGeometry>(ref this.m_EdgeGeometryType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<StartNodeGeometry> nativeArray14 = chunk.GetNativeArray<StartNodeGeometry>(ref this.m_StartNodeGeometryType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<EndNodeGeometry> nativeArray15 = chunk.GetNativeArray<EndNodeGeometry>(ref this.m_EndNodeGeometryType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Composition> nativeArray16 = chunk.GetNativeArray<Composition>(ref this.m_CompositionType);
            // ISSUE: reference to a compiler-generated field
            bool flag = chunk.Has<Game.Net.Marker>(ref this.m_NetMarkerType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              ref CullingInfo local = ref nativeArray1.ElementAt<CullingInfo>(index);
              // ISSUE: reference to a compiler-generated field
              if (this.m_UpdateAll | isUpdated)
              {
                if (nativeArray13.Length != 0)
                {
                  EdgeGeometry edgeGeometry = nativeArray13[index];
                  StartNodeGeometry startNodeGeometry = nativeArray14[index];
                  EndNodeGeometry endNodeGeometry = nativeArray15[index];
                  Composition composition = nativeArray16[index];
                  local.m_Bounds = edgeGeometry.m_Bounds | startNodeGeometry.m_Geometry.m_Bounds | endNodeGeometry.m_Geometry.m_Bounds;
                  // ISSUE: reference to a compiler-generated field
                  NetCompositionData netCompositionData1 = this.m_PrefabCompositionData[composition.m_Edge];
                  // ISSUE: reference to a compiler-generated field
                  NetCompositionData netCompositionData2 = this.m_PrefabCompositionData[composition.m_StartNode];
                  // ISSUE: reference to a compiler-generated field
                  NetCompositionData netCompositionData3 = this.m_PrefabCompositionData[composition.m_EndNode];
                  local.m_Radius = 0.0f;
                  local.m_Mask = BoundsMask.Debug;
                  // ISSUE: reference to a compiler-generated field
                  if (!flag || this.m_EditorMode)
                  {
                    NetCompositionMeshData componentData3;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (math.any(edgeGeometry.m_Start.m_Length + edgeGeometry.m_End.m_Length > 0.1f) && this.m_PrefabCompositionMeshData.TryGetComponent(this.m_PrefabCompositionMeshRef[composition.m_Edge].m_Mesh, out componentData3))
                      local.m_Mask |= componentData3.m_DefaultLayers == (MeshLayer) 0 ? BoundsMask.NormalLayers : CommonUtils.GetBoundsMask(componentData3.m_DefaultLayers);
                    NetCompositionMeshData componentData4;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (math.any(startNodeGeometry.m_Geometry.m_Left.m_Length > 0.05f) | math.any(startNodeGeometry.m_Geometry.m_Right.m_Length > 0.05f) && this.m_PrefabCompositionMeshData.TryGetComponent(this.m_PrefabCompositionMeshRef[composition.m_StartNode].m_Mesh, out componentData4))
                      local.m_Mask |= componentData4.m_DefaultLayers == (MeshLayer) 0 ? BoundsMask.NormalLayers : CommonUtils.GetBoundsMask(componentData4.m_DefaultLayers);
                    NetCompositionMeshData componentData5;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (math.any(endNodeGeometry.m_Geometry.m_Left.m_Length > 0.05f) | math.any(endNodeGeometry.m_Geometry.m_Right.m_Length > 0.05f) && this.m_PrefabCompositionMeshData.TryGetComponent(this.m_PrefabCompositionMeshRef[composition.m_EndNode].m_Mesh, out componentData5))
                      local.m_Mask |= componentData5.m_DefaultLayers == (MeshLayer) 0 ? BoundsMask.NormalLayers : CommonUtils.GetBoundsMask(componentData5.m_DefaultLayers);
                  }
                  local.m_MinLod = (byte) math.min(netCompositionData1.m_MinLod, math.min(netCompositionData2.m_MinLod, netCompositionData3.m_MinLod));
                }
                else
                {
                  Curve curve = nativeArray7[index];
                  local.m_Bounds = MathUtils.Expand(MathUtils.Bounds(curve.m_Bezier), (float3) 0.5f);
                  local.m_Radius = 0.0f;
                  // ISSUE: reference to a compiler-generated field
                  local.m_Mask = this.m_EditorMode ? BoundsMask.Debug | BoundsMask.NormalLayers : BoundsMask.Debug;
                  local.m_MinLod = (byte) RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float2(2f)));
                }
              }
              // ISSUE: reference to a compiler-generated method
              this.SetFlags(ref local, -1, isUpdated, batchesUpdated, false);
            }
          }
          else if (nativeArray7.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Owner> nativeArray17 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Net.UtilityLane> nativeArray18 = chunk.GetNativeArray<Game.Net.UtilityLane>(ref this.m_UtilityLaneType);
            // ISSUE: reference to a compiler-generated field
            bool flag = chunk.Has<Overridden>(ref this.m_OverriddenType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              ref CullingInfo local = ref nativeArray1.ElementAt<CullingInfo>(index);
              // ISSUE: reference to a compiler-generated field
              if (this.m_UpdateAll | isUpdated)
              {
                Curve curve = nativeArray7[index];
                PrefabRef prefabRef = nativeArray2[index];
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabLaneGeometryData.HasComponent(prefabRef.m_Prefab))
                {
                  // ISSUE: reference to a compiler-generated field
                  NetLaneGeometryData laneGeometryData = this.m_PrefabLaneGeometryData[prefabRef.m_Prefab];
                  local.m_Bounds = MathUtils.Expand(MathUtils.Bounds(curve.m_Bezier), laneGeometryData.m_Size.xyx * 0.5f);
                  local.m_Radius = 0.0f;
                  local.m_Mask = BoundsMask.Debug;
                  if (!flag && (double) curve.m_Length > 0.10000000149011612)
                  {
                    // ISSUE: reference to a compiler-generated field
                    MeshLayer defaultLayers = this.m_EditorMode ? laneGeometryData.m_EditorLayers : laneGeometryData.m_GameLayers;
                    Owner owner;
                    CollectionUtils.TryGet<Owner>(nativeArray17, index, out owner);
                    Game.Net.UtilityLane utilityLane;
                    CollectionUtils.TryGet<Game.Net.UtilityLane>(nativeArray18, index, out utilityLane);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    local.m_Mask |= CommonUtils.GetBoundsMask(Game.Net.SearchSystem.GetLayers(owner, utilityLane, defaultLayers, ref this.m_PrefabRefData, ref this.m_PrefabNetData, ref this.m_PrefabNetGeometryData));
                  }
                  int x = laneGeometryData.m_MinLod;
                  UtilityLaneData componentData;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabUtilityLaneData.TryGetComponent(prefabRef.m_Prefab, out componentData) && (componentData.m_UtilityTypes & this.m_DilatedUtilityTypes) != UtilityTypes.None)
                  {
                    float renderingSize = RenderingUtils.GetRenderingSize(new float2(componentData.m_VisualCapacity));
                    x = math.min(x, RenderingUtils.CalculateLodLimit(renderingSize));
                  }
                  local.m_MinLod = (byte) x;
                }
                else
                {
                  local.m_Bounds = MathUtils.Expand(MathUtils.Bounds(curve.m_Bezier), (float3) 0.5f);
                  local.m_Radius = 0.0f;
                  local.m_Mask = BoundsMask.Debug;
                  local.m_MinLod = (byte) RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float2(1f)));
                }
              }
              // ISSUE: reference to a compiler-generated method
              this.SetFlags(ref local, -1, isUpdated, batchesUpdated, false);
            }
          }
          else
          {
            if (nativeArray8.Length == 0)
              return;
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              ref CullingInfo local = ref nativeArray1.ElementAt<CullingInfo>(index);
              // ISSUE: reference to a compiler-generated field
              if (this.m_UpdateAll | isUpdated)
              {
                Game.Zones.Block block = nativeArray8[index];
                float3 size = new float3((float) block.m_Size.x, (float) math.cmax(block.m_Size), (float) block.m_Size.y) * 8f;
                local.m_Bounds = new Bounds3(block.m_Position, block.m_Position);
                local.m_Bounds.xz = ZoneUtils.CalculateBounds(block);
                local.m_Radius = 0.0f;
                local.m_Mask = BoundsMask.Debug | BoundsMask.NormalLayers;
                local.m_MinLod = (byte) RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(size), 0.0f);
              }
              // ISSUE: reference to a compiler-generated method
              this.SetFlags(ref local, -1, isUpdated, batchesUpdated, false);
            }
          }
        }
      }

      private void SetFlags(
        ref CullingInfo cullingInfo,
        int updateFrame,
        bool isUpdated,
        bool batchesUpdated,
        bool remove)
      {
        if (cullingInfo.m_CullingIndex == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        ref PreCullingData local = ref this.m_CullingData.ElementAt(cullingInfo.m_CullingIndex);
        local.m_UpdateFrame = (sbyte) updateFrame;
        if (isUpdated)
          local.m_Flags |= PreCullingFlags.Updated;
        if (batchesUpdated)
          local.m_Flags |= PreCullingFlags.BatchesUpdated;
        if (!remove)
          return;
        cullingInfo.m_PassedCulling = (byte) 0;
        local.m_Flags &= ~PreCullingFlags.PassedCulling;
        local.m_Timer = byte.MaxValue;
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
    private struct EventCullingJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<RentersUpdated> m_RentersUpdatedType;
      [ReadOnly]
      public ComponentTypeHandle<ColorUpdated> m_ColorUpdatedType;
      [ReadOnly]
      public ComponentLookup<CullingInfo> m_CullingInfoData;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<RouteVehicle> m_RouteVehicles;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      public NativeList<PreCullingData> m_CullingData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<RentersUpdated> nativeArray1 = chunk.GetNativeArray<RentersUpdated>(ref this.m_RentersUpdatedType);
        if (nativeArray1.Length != 0)
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity property = nativeArray1[index].m_Property;
            // ISSUE: reference to a compiler-generated method
            this.SetFlags(property);
            // ISSUE: reference to a compiler-generated method
            this.AddSubObjects(property);
            // ISSUE: reference to a compiler-generated method
            this.AddSubLanes(property);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<ColorUpdated> nativeArray2 = chunk.GetNativeArray<ColorUpdated>(ref this.m_ColorUpdatedType);
          if (nativeArray2.Length == 0)
            return;
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            this.AddRouteVehicles(nativeArray2[index].m_Route);
          }
        }
      }

      private void AddSubObjects(Entity owner)
      {
        DynamicBuffer<Game.Objects.SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(owner, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity subObject = bufferData[index].m_SubObject;
          // ISSUE: reference to a compiler-generated method
          this.SetFlags(subObject);
          // ISSUE: reference to a compiler-generated method
          this.AddSubObjects(subObject);
          // ISSUE: reference to a compiler-generated method
          this.AddSubLanes(subObject);
        }
      }

      private void AddSubLanes(Entity owner)
      {
        DynamicBuffer<Game.Net.SubLane> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubLanes.TryGetBuffer(owner, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.SetFlags(bufferData[index].m_SubLane);
        }
      }

      private void AddRouteVehicles(Entity owner)
      {
        DynamicBuffer<RouteVehicle> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_RouteVehicles.TryGetBuffer(owner, out bufferData1))
          return;
        for (int index1 = 0; index1 < bufferData1.Length; ++index1)
        {
          Entity vehicle = bufferData1[index1].m_Vehicle;
          DynamicBuffer<LayoutElement> bufferData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LayoutElements.TryGetBuffer(vehicle, out bufferData2) && bufferData2.Length != 0)
          {
            for (int index2 = 0; index2 < bufferData2.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated method
              this.SetFlags(bufferData2[index2].m_Vehicle);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.SetFlags(vehicle);
          }
        }
      }

      private void SetFlags(Entity entity)
      {
        CullingInfo componentData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CullingInfoData.TryGetComponent(entity, out componentData) || componentData.m_CullingIndex == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CullingData.ElementAt(componentData.m_CullingIndex).m_Flags |= PreCullingFlags.ColorsUpdated;
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
    private struct QueryCullingJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public BufferTypeHandle<TransformFrame> m_TransformFrameType;
      public ComponentTypeHandle<CullingInfo> m_CullingInfoType;
      [ReadOnly]
      public float4 m_LodParameters;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public float3 m_CameraDirection;
      [ReadOnly]
      public uint m_FrameIndex;
      [ReadOnly]
      public float m_FrameTime;
      [ReadOnly]
      public BoundsMask m_VisibleMask;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelQueue<PreCullingSystem.CullingAction>.Writer m_ActionQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CullingInfo> nativeArray2 = chunk.GetNativeArray<CullingInfo>(ref this.m_CullingInfoType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TransformFrame> bufferAccessor = chunk.GetBufferAccessor<TransformFrame>(ref this.m_TransformFrameType);
        if (bufferAccessor.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Transform> nativeArray3 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
          // ISSUE: reference to a compiler-generated field
          uint index1 = chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index;
          uint updateFrame1;
          uint updateFrame2;
          float framePosition;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ObjectInterpolateSystem.CalculateUpdateFrames(this.m_FrameIndex, this.m_FrameTime, index1, out updateFrame1, out updateFrame2, out framePosition);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            ref CullingInfo local1 = ref nativeArray2.ElementAt<CullingInfo>(index2);
            // ISSUE: variable of a compiler-generated type
            PreCullingSystem.CullingAction cullingAction1;
            // ISSUE: reference to a compiler-generated field
            if ((this.m_VisibleMask & local1.m_Mask) == (BoundsMask) 0)
            {
              if (local1.m_CullingIndex != 0)
              {
                // ISSUE: reference to a compiler-generated field
                ref NativeParallelQueue<PreCullingSystem.CullingAction>.Writer local2 = ref this.m_ActionQueue;
                // ISSUE: object of a compiler-generated type is created
                cullingAction1 = new PreCullingSystem.CullingAction();
                // ISSUE: reference to a compiler-generated field
                cullingAction1.m_Entity = nativeArray1[index2];
                // ISSUE: reference to a compiler-generated field
                cullingAction1.m_UpdateFrame = (sbyte) index1;
                // ISSUE: variable of a compiler-generated type
                PreCullingSystem.CullingAction cullingAction2 = cullingAction1;
                local2.Enqueue(cullingAction2);
              }
            }
            else if (local1.m_PassedCulling != (byte) 0)
            {
              DynamicBuffer<TransformFrame> dynamicBuffer = bufferAccessor[index2];
              float3 float3 = math.lerp(dynamicBuffer[(int) updateFrame1].m_Position, dynamicBuffer[(int) updateFrame2].m_Position, framePosition);
              local1.m_Bounds = new Bounds3(float3 - local1.m_Radius, float3 + local1.m_Radius);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              double minDistance = (double) RenderingUtils.CalculateMinDistance(local1.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
              // ISSUE: reference to a compiler-generated field
              if (RenderingUtils.CalculateLod((float) (minDistance * minDistance), this.m_LodParameters) < (int) local1.m_MinLod)
              {
                // ISSUE: reference to a compiler-generated field
                ref NativeParallelQueue<PreCullingSystem.CullingAction>.Writer local3 = ref this.m_ActionQueue;
                // ISSUE: object of a compiler-generated type is created
                cullingAction1 = new PreCullingSystem.CullingAction();
                // ISSUE: reference to a compiler-generated field
                cullingAction1.m_Entity = nativeArray1[index2];
                // ISSUE: reference to a compiler-generated field
                cullingAction1.m_Flags = PreCullingSystem.ActionFlags.CrossFade;
                // ISSUE: reference to a compiler-generated field
                cullingAction1.m_UpdateFrame = (sbyte) index1;
                // ISSUE: variable of a compiler-generated type
                PreCullingSystem.CullingAction cullingAction3 = cullingAction1;
                local3.Enqueue(cullingAction3);
              }
            }
            else
            {
              float3 position = nativeArray3[index2].m_Position;
              local1.m_Bounds = new Bounds3(position - local1.m_Radius, position + local1.m_Radius);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              double num = (double) math.max(0.0f, RenderingUtils.CalculateMinDistance(local1.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters) - 277.777771f);
              // ISSUE: reference to a compiler-generated field
              if (RenderingUtils.CalculateLod((float) (num * num), this.m_LodParameters) >= (int) local1.m_MinLod)
              {
                DynamicBuffer<TransformFrame> dynamicBuffer = bufferAccessor[index2];
                float3 float3 = math.lerp(dynamicBuffer[(int) updateFrame1].m_Position, dynamicBuffer[(int) updateFrame2].m_Position, framePosition);
                local1.m_Bounds = new Bounds3(float3 - local1.m_Radius, float3 + local1.m_Radius);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                double minDistance = (double) RenderingUtils.CalculateMinDistance(local1.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
                // ISSUE: reference to a compiler-generated field
                if (RenderingUtils.CalculateLod((float) (minDistance * minDistance), this.m_LodParameters) >= (int) local1.m_MinLod)
                {
                  // ISSUE: reference to a compiler-generated field
                  ref NativeParallelQueue<PreCullingSystem.CullingAction>.Writer local4 = ref this.m_ActionQueue;
                  // ISSUE: object of a compiler-generated type is created
                  cullingAction1 = new PreCullingSystem.CullingAction();
                  // ISSUE: reference to a compiler-generated field
                  cullingAction1.m_Entity = nativeArray1[index2];
                  // ISSUE: reference to a compiler-generated field
                  cullingAction1.m_Flags = PreCullingSystem.ActionFlags.PassedCulling;
                  // ISSUE: reference to a compiler-generated field
                  cullingAction1.m_UpdateFrame = (sbyte) index1;
                  // ISSUE: variable of a compiler-generated type
                  PreCullingSystem.CullingAction cullingAction4 = cullingAction1;
                  local4.Enqueue(cullingAction4);
                }
              }
            }
          }
        }
        else
        {
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            ref CullingInfo local5 = ref nativeArray2.ElementAt<CullingInfo>(index);
            // ISSUE: variable of a compiler-generated type
            PreCullingSystem.CullingAction cullingAction5;
            // ISSUE: reference to a compiler-generated field
            if ((this.m_VisibleMask & local5.m_Mask) == (BoundsMask) 0)
            {
              if (local5.m_CullingIndex != 0)
              {
                // ISSUE: reference to a compiler-generated field
                ref NativeParallelQueue<PreCullingSystem.CullingAction>.Writer local6 = ref this.m_ActionQueue;
                // ISSUE: object of a compiler-generated type is created
                cullingAction5 = new PreCullingSystem.CullingAction();
                // ISSUE: reference to a compiler-generated field
                cullingAction5.m_Entity = nativeArray1[index];
                // ISSUE: reference to a compiler-generated field
                cullingAction5.m_UpdateFrame = (sbyte) -1;
                // ISSUE: variable of a compiler-generated type
                PreCullingSystem.CullingAction cullingAction6 = cullingAction5;
                local6.Enqueue(cullingAction6);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              double minDistance = (double) RenderingUtils.CalculateMinDistance(local5.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
              // ISSUE: reference to a compiler-generated field
              int lod = RenderingUtils.CalculateLod((float) (minDistance * minDistance), this.m_LodParameters);
              if (local5.m_PassedCulling != (byte) 0)
              {
                if (lod < (int) local5.m_MinLod)
                {
                  // ISSUE: reference to a compiler-generated field
                  ref NativeParallelQueue<PreCullingSystem.CullingAction>.Writer local7 = ref this.m_ActionQueue;
                  // ISSUE: object of a compiler-generated type is created
                  cullingAction5 = new PreCullingSystem.CullingAction();
                  // ISSUE: reference to a compiler-generated field
                  cullingAction5.m_Entity = nativeArray1[index];
                  // ISSUE: reference to a compiler-generated field
                  cullingAction5.m_Flags = PreCullingSystem.ActionFlags.CrossFade;
                  // ISSUE: reference to a compiler-generated field
                  cullingAction5.m_UpdateFrame = (sbyte) -1;
                  // ISSUE: variable of a compiler-generated type
                  PreCullingSystem.CullingAction cullingAction7 = cullingAction5;
                  local7.Enqueue(cullingAction7);
                }
              }
              else if (lod >= (int) local5.m_MinLod)
              {
                // ISSUE: reference to a compiler-generated field
                ref NativeParallelQueue<PreCullingSystem.CullingAction>.Writer local8 = ref this.m_ActionQueue;
                // ISSUE: object of a compiler-generated type is created
                cullingAction5 = new PreCullingSystem.CullingAction();
                // ISSUE: reference to a compiler-generated field
                cullingAction5.m_Entity = nativeArray1[index];
                // ISSUE: reference to a compiler-generated field
                cullingAction5.m_Flags = PreCullingSystem.ActionFlags.PassedCulling;
                // ISSUE: reference to a compiler-generated field
                cullingAction5.m_UpdateFrame = (sbyte) -1;
                // ISSUE: variable of a compiler-generated type
                PreCullingSystem.CullingAction cullingAction8 = cullingAction5;
                local8.Enqueue(cullingAction8);
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
    private struct QueryRemoveJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<Applied> m_AppliedType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public BufferTypeHandle<TransformFrame> m_TransformFrameType;
      public ComponentTypeHandle<CullingInfo> m_CullingInfoType;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelQueue<PreCullingSystem.CullingAction>.Writer m_ActionQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CullingInfo> nativeArray2 = chunk.GetNativeArray<CullingInfo>(ref this.m_CullingInfoType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<Deleted>(ref this.m_DeletedType);
        bool flag2 = false;
        if (flag1)
        {
          // ISSUE: reference to a compiler-generated field
          flag2 = chunk.Has<Applied>(ref this.m_AppliedType);
        }
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<TransformFrame>(ref this.m_TransformFrameType))
        {
          // ISSUE: reference to a compiler-generated field
          uint index1 = chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index;
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            if (nativeArray2.ElementAt<CullingInfo>(index2).m_CullingIndex != 0)
            {
              // ISSUE: variable of a compiler-generated type
              PreCullingSystem.ActionFlags actionFlags = (PreCullingSystem.ActionFlags) 0;
              if (flag1)
                actionFlags = flag2 ? PreCullingSystem.ActionFlags.Deleted | PreCullingSystem.ActionFlags.Applied : PreCullingSystem.ActionFlags.Deleted;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_ActionQueue.Enqueue(new PreCullingSystem.CullingAction()
              {
                m_Entity = nativeArray1[index2],
                m_Flags = actionFlags,
                m_UpdateFrame = (sbyte) index1
              });
            }
          }
        }
        else
        {
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            if (nativeArray2.ElementAt<CullingInfo>(index).m_CullingIndex != 0)
            {
              // ISSUE: variable of a compiler-generated type
              PreCullingSystem.ActionFlags actionFlags = (PreCullingSystem.ActionFlags) 0;
              if (flag1)
                actionFlags = flag2 ? PreCullingSystem.ActionFlags.Deleted | PreCullingSystem.ActionFlags.Applied : PreCullingSystem.ActionFlags.Deleted;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_ActionQueue.Enqueue(new PreCullingSystem.CullingAction()
              {
                m_Entity = nativeArray1[index],
                m_Flags = actionFlags,
                m_UpdateFrame = (sbyte) -1
              });
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
    private struct RelativeCullingJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> m_CurrentVehicleType;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<CullingInfo> m_CullingInfoData;
      [ReadOnly]
      public float4 m_LodParameters;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public float3 m_CameraDirection;
      [ReadOnly]
      public BoundsMask m_VisibleMask;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelQueue<PreCullingSystem.CullingAction>.Writer m_ActionQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentVehicle> nativeArray2 = chunk.GetNativeArray<CurrentVehicle>(ref this.m_CurrentVehicleType);
        if (nativeArray2.Length != 0)
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateCulling(nativeArray1[index], nativeArray2[index].m_Vehicle);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateCulling(nativeArray1[index], nativeArray3[index].m_Owner);
          }
        }
      }

      private void UpdateCulling(Entity entity, Entity parent)
      {
        // ISSUE: reference to a compiler-generated field
        ref CullingInfo local = ref this.m_CullingInfoData.GetRefRW(entity).ValueRW;
        // ISSUE: reference to a compiler-generated field
        CullingInfo cullingInfo = this.m_CullingInfoData[parent];
        local.m_Bounds = cullingInfo.m_Bounds;
        // ISSUE: reference to a compiler-generated field
        if ((this.m_VisibleMask & local.m_Mask) == (BoundsMask) 0)
        {
          if (local.m_CullingIndex == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ActionQueue.Enqueue(new PreCullingSystem.CullingAction()
          {
            m_Entity = entity,
            m_UpdateFrame = (sbyte) -1
          });
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          double minDistance = (double) RenderingUtils.CalculateMinDistance(local.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
          // ISSUE: reference to a compiler-generated field
          int lod = RenderingUtils.CalculateLod((float) (minDistance * minDistance), this.m_LodParameters);
          if (local.m_PassedCulling != (byte) 0)
          {
            if (lod >= (int) local.m_MinLod)
              return;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ActionQueue.Enqueue(new PreCullingSystem.CullingAction()
            {
              m_Entity = entity,
              m_Flags = PreCullingSystem.ActionFlags.CrossFade,
              m_UpdateFrame = (sbyte) -1
            });
          }
          else
          {
            if (lod < (int) local.m_MinLod)
              return;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ActionQueue.Enqueue(new PreCullingSystem.CullingAction()
            {
              m_Entity = entity,
              m_Flags = PreCullingSystem.ActionFlags.PassedCulling,
              m_UpdateFrame = (sbyte) -1
            });
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
    private struct TempCullingJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<InterpolatedTransform> m_InterpolatedTransformType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Stack> m_StackType;
      [ReadOnly]
      public ComponentTypeHandle<Static> m_StaticType;
      [ReadOnly]
      public ComponentTypeHandle<Stopped> m_StoppedType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<StackData> m_PrefabStackData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<CullingInfo> m_CullingInfoData;
      [ReadOnly]
      public float4 m_LodParameters;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public float3 m_CameraDirection;
      [ReadOnly]
      public BoundsMask m_VisibleMask;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelQueue<PreCullingSystem.CullingAction>.Writer m_ActionQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<InterpolatedTransform>(ref this.m_InterpolatedTransformType);
        bool flag2 = false;
        bool flag3 = false;
        NativeArray<Transform> nativeArray2 = new NativeArray<Transform>();
        NativeArray<Stack> nativeArray3 = new NativeArray<Stack>();
        NativeArray<Temp> nativeArray4 = new NativeArray<Temp>();
        NativeArray<PrefabRef> nativeArray5 = new NativeArray<PrefabRef>();
        if (flag1)
        {
          // ISSUE: reference to a compiler-generated field
          flag2 = chunk.Has<Static>(ref this.m_StaticType);
          // ISSUE: reference to a compiler-generated field
          flag3 = chunk.Has<Stopped>(ref this.m_StoppedType);
          // ISSUE: reference to a compiler-generated field
          nativeArray2 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
          // ISSUE: reference to a compiler-generated field
          nativeArray3 = chunk.GetNativeArray<Stack>(ref this.m_StackType);
          // ISSUE: reference to a compiler-generated field
          nativeArray4 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
          // ISSUE: reference to a compiler-generated field
          nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        }
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          ref CullingInfo local1 = ref this.m_CullingInfoData.GetRefRW(nativeArray1[index]).ValueRW;
          if (flag1)
          {
            Temp temp = nativeArray4[index];
            CullingInfo componentData1;
            // ISSUE: reference to a compiler-generated field
            if (temp.m_Original != Entity.Null && (temp.m_Flags & TempFlags.Dragging) == (TempFlags) 0 && (!flag2 && !flag3 || (temp.m_Flags & (TempFlags.Create | TempFlags.Modify)) == (TempFlags) 0) && this.m_CullingInfoData.TryGetComponent(temp.m_Original, out componentData1))
            {
              local1.m_Bounds = componentData1.m_Bounds;
            }
            else
            {
              Transform transform = nativeArray2[index];
              PrefabRef prefabRef = nativeArray5[index];
              ObjectGeometryData componentData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
              {
                StackData componentData3;
                // ISSUE: reference to a compiler-generated field
                if (nativeArray3.Length != 0 && this.m_PrefabStackData.TryGetComponent(prefabRef.m_Prefab, out componentData3))
                {
                  Stack stack = nativeArray3[index];
                  local1.m_Bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, stack, componentData2, componentData3);
                }
                else
                  local1.m_Bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, componentData2);
                if ((componentData2.m_Flags & Game.Objects.GeometryFlags.HasBase) != Game.Objects.GeometryFlags.None)
                {
                  // ISSUE: reference to a compiler-generated field
                  local1.m_Bounds.min.y = math.min(local1.m_Bounds.min.y, TerrainUtils.GetHeightRange(ref this.m_TerrainHeightData, local1.m_Bounds).min);
                }
              }
              else
                local1.m_Bounds = new Bounds3(transform.m_Position - 1f, transform.m_Position + 1f);
            }
          }
          // ISSUE: variable of a compiler-generated type
          PreCullingSystem.CullingAction cullingAction1;
          // ISSUE: reference to a compiler-generated field
          if ((this.m_VisibleMask & local1.m_Mask) == (BoundsMask) 0)
          {
            if (local1.m_CullingIndex != 0)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeParallelQueue<PreCullingSystem.CullingAction>.Writer local2 = ref this.m_ActionQueue;
              // ISSUE: object of a compiler-generated type is created
              cullingAction1 = new PreCullingSystem.CullingAction();
              // ISSUE: reference to a compiler-generated field
              cullingAction1.m_Entity = nativeArray1[index];
              // ISSUE: reference to a compiler-generated field
              cullingAction1.m_UpdateFrame = (sbyte) -1;
              // ISSUE: variable of a compiler-generated type
              PreCullingSystem.CullingAction cullingAction2 = cullingAction1;
              local2.Enqueue(cullingAction2);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double minDistance = (double) RenderingUtils.CalculateMinDistance(local1.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
            // ISSUE: reference to a compiler-generated field
            int lod = RenderingUtils.CalculateLod((float) (minDistance * minDistance), this.m_LodParameters);
            if (local1.m_PassedCulling != (byte) 0)
            {
              if (lod < (int) local1.m_MinLod)
              {
                // ISSUE: reference to a compiler-generated field
                ref NativeParallelQueue<PreCullingSystem.CullingAction>.Writer local3 = ref this.m_ActionQueue;
                // ISSUE: object of a compiler-generated type is created
                cullingAction1 = new PreCullingSystem.CullingAction();
                // ISSUE: reference to a compiler-generated field
                cullingAction1.m_Entity = nativeArray1[index];
                // ISSUE: reference to a compiler-generated field
                cullingAction1.m_Flags = PreCullingSystem.ActionFlags.CrossFade;
                // ISSUE: reference to a compiler-generated field
                cullingAction1.m_UpdateFrame = (sbyte) -1;
                // ISSUE: variable of a compiler-generated type
                PreCullingSystem.CullingAction cullingAction3 = cullingAction1;
                local3.Enqueue(cullingAction3);
              }
            }
            else if (lod >= (int) local1.m_MinLod)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeParallelQueue<PreCullingSystem.CullingAction>.Writer local4 = ref this.m_ActionQueue;
              // ISSUE: object of a compiler-generated type is created
              cullingAction1 = new PreCullingSystem.CullingAction();
              // ISSUE: reference to a compiler-generated field
              cullingAction1.m_Entity = nativeArray1[index];
              // ISSUE: reference to a compiler-generated field
              cullingAction1.m_Flags = PreCullingSystem.ActionFlags.PassedCulling;
              // ISSUE: reference to a compiler-generated field
              cullingAction1.m_UpdateFrame = (sbyte) -1;
              // ISSUE: variable of a compiler-generated type
              PreCullingSystem.CullingAction cullingAction4 = cullingAction1;
              local4.Enqueue(cullingAction4);
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
    private struct VerifyVisibleJob : IJobParallelFor
    {
      [ReadOnly]
      public ComponentLookup<CullingInfo> m_CullingInfoData;
      [ReadOnly]
      public float4 m_LodParameters;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public float3 m_CameraDirection;
      [ReadOnly]
      public BoundsMask m_VisibleMask;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelQueue<PreCullingSystem.CullingAction>.Writer m_ActionQueue;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        PreCullingData preCullingData = this.m_CullingData[index];
        if ((preCullingData.m_Flags & PreCullingFlags.FadeContainer) != (PreCullingFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        CullingInfo cullingInfo = this.m_CullingInfoData[preCullingData.m_Entity];
        // ISSUE: reference to a compiler-generated field
        if ((this.m_VisibleMask & cullingInfo.m_Mask) == (BoundsMask) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ActionQueue.Enqueue(new PreCullingSystem.CullingAction()
          {
            m_Entity = preCullingData.m_Entity,
            m_UpdateFrame = preCullingData.m_UpdateFrame
          });
        }
        else
        {
          if (cullingInfo.m_PassedCulling == (byte) 0)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          double minDistance = (double) RenderingUtils.CalculateMinDistance(cullingInfo.m_Bounds, this.m_CameraPosition, this.m_CameraDirection, this.m_LodParameters);
          // ISSUE: reference to a compiler-generated field
          if (RenderingUtils.CalculateLod((float) (minDistance * minDistance), this.m_LodParameters) >= (int) cullingInfo.m_MinLod)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ActionQueue.Enqueue(new PreCullingSystem.CullingAction()
          {
            m_Entity = preCullingData.m_Entity,
            m_UpdateFrame = preCullingData.m_UpdateFrame
          });
        }
      }
    }

    [Flags]
    public enum ActionFlags : byte
    {
      PassedCulling = 1,
      CrossFade = 2,
      Deleted = 4,
      Applied = 8,
    }

    private struct CullingAction
    {
      public Entity m_Entity;
      public PreCullingSystem.ActionFlags m_Flags;
      public sbyte m_UpdateFrame;

      public override int GetHashCode() => this.m_Entity.GetHashCode();
    }

    private struct OverflowAction
    {
      public int m_DataIndex;
      public Entity m_Entity;
      public sbyte m_UpdateFrame;
    }

    [BurstCompile]
    private struct CullingActionJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeParallelQueue<PreCullingSystem.CullingAction>.Reader m_CullingActions;
      public NativeQueue<PreCullingSystem.OverflowAction>.ParallelWriter m_OverflowActions;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<CullingInfo> m_CullingInfo;
      [NativeDisableParallelForRestriction]
      public NativeList<PreCullingData> m_CullingData;
      [NativeDisableParallelForRestriction]
      public NativeReference<int> m_CullingDataIndex;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        NativeParallelQueue<PreCullingSystem.CullingAction>.Enumerator enumerator = this.m_CullingActions.GetEnumerator(index);
        while (enumerator.MoveNext())
        {
          // ISSUE: variable of a compiler-generated type
          PreCullingSystem.CullingAction current = enumerator.Current;
          // ISSUE: reference to a compiler-generated field
          if ((current.m_Flags & PreCullingSystem.ActionFlags.PassedCulling) != (PreCullingSystem.ActionFlags) 0)
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

      private unsafe void PassedCulling(PreCullingSystem.CullingAction cullingAction)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ref CullingInfo local1 = ref this.m_CullingInfo.GetRefRW(cullingAction.m_Entity).ValueRW;
        local1.m_PassedCulling = (byte) 1;
        if (local1.m_CullingIndex == 0)
        {
          // ISSUE: reference to a compiler-generated field
          ref int local2 = ref UnsafeUtility.AsRef<int>((void*) this.m_CullingDataIndex.GetUnsafePtr<int>());
          local1.m_CullingIndex = Interlocked.Increment(ref local2) - 1;
          // ISSUE: reference to a compiler-generated field
          if (local1.m_CullingIndex >= this.m_CullingData.Capacity)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_OverflowActions.Enqueue(new PreCullingSystem.OverflowAction()
            {
              m_DataIndex = local1.m_CullingIndex,
              m_Entity = cullingAction.m_Entity,
              m_UpdateFrame = cullingAction.m_UpdateFrame
            });
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            ref PreCullingData local3 = ref UnsafeUtility.ArrayElementAsRef<PreCullingData>((void*) this.m_CullingData.GetUnsafePtr<PreCullingData>(), local1.m_CullingIndex);
            // ISSUE: reference to a compiler-generated field
            local3.m_Entity = cullingAction.m_Entity;
            // ISSUE: reference to a compiler-generated field
            local3.m_UpdateFrame = cullingAction.m_UpdateFrame;
            local3.m_Flags = PreCullingFlags.PassedCulling | PreCullingFlags.NearCamera | PreCullingFlags.NearCameraUpdated;
            local3.m_Timer = (byte) 0;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (local1.m_CullingIndex >= this.m_CullingData.Length)
            return;
          // ISSUE: reference to a compiler-generated field
          ref PreCullingData local4 = ref UnsafeUtility.ArrayElementAsRef<PreCullingData>((void*) this.m_CullingData.GetUnsafePtr<PreCullingData>(), local1.m_CullingIndex);
          // ISSUE: reference to a compiler-generated field
          local4.m_Entity = cullingAction.m_Entity;
          // ISSUE: reference to a compiler-generated field
          local4.m_UpdateFrame = cullingAction.m_UpdateFrame;
          local4.m_Flags |= PreCullingFlags.PassedCulling;
          local4.m_Timer = (byte) 0;
          if ((local4.m_Flags & PreCullingFlags.NearCamera) != (PreCullingFlags) 0)
            return;
          local4.m_Flags |= PreCullingFlags.NearCamera | PreCullingFlags.NearCameraUpdated;
        }
      }

      private unsafe void FailedCulling(PreCullingSystem.CullingAction cullingAction)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ref CullingInfo local1 = ref this.m_CullingInfo.GetRefRW(cullingAction.m_Entity).ValueRW;
        local1.m_PassedCulling = (byte) 0;
        // ISSUE: reference to a compiler-generated field
        if (local1.m_CullingIndex == 0 || local1.m_CullingIndex >= this.m_CullingData.Length)
          return;
        // ISSUE: reference to a compiler-generated field
        ref PreCullingData local2 = ref UnsafeUtility.ArrayElementAsRef<PreCullingData>((void*) this.m_CullingData.GetUnsafePtr<PreCullingData>(), local1.m_CullingIndex);
        // ISSUE: reference to a compiler-generated field
        local2.m_UpdateFrame = cullingAction.m_UpdateFrame;
        local2.m_Flags &= ~PreCullingFlags.PassedCulling;
        // ISSUE: reference to a compiler-generated field
        if ((cullingAction.m_Flags & PreCullingSystem.ActionFlags.Deleted) != (PreCullingSystem.ActionFlags) 0)
          local2.m_Flags |= PreCullingFlags.Deleted;
        // ISSUE: reference to a compiler-generated field
        if ((cullingAction.m_Flags & PreCullingSystem.ActionFlags.Applied) != (PreCullingSystem.ActionFlags) 0)
          local2.m_Flags |= PreCullingFlags.Applied;
        // ISSUE: reference to a compiler-generated field
        if ((cullingAction.m_Flags & PreCullingSystem.ActionFlags.CrossFade) != (PreCullingSystem.ActionFlags) 0)
          return;
        local2.m_Timer = byte.MaxValue;
      }
    }

    [BurstCompile]
    private struct ResizeCullingDataJob : IJob
    {
      [ReadOnly]
      public NativeReference<int> m_CullingDataIndex;
      public NativeList<PreCullingData> m_CullingData;
      public NativeList<PreCullingData> m_UpdatedData;
      public NativeQueue<PreCullingSystem.OverflowAction> m_OverflowActions;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CullingData.Resize(math.min(this.m_CullingDataIndex.Value, this.m_CullingData.Capacity), NativeArrayOptions.UninitializedMemory);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CullingData.Resize(this.m_CullingDataIndex.Value, NativeArrayOptions.UninitializedMemory);
        // ISSUE: reference to a compiler-generated field
        this.m_UpdatedData.Clear();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CullingData.Length > this.m_UpdatedData.Capacity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_UpdatedData.Capacity = this.m_CullingData.Length;
        }
        // ISSUE: variable of a compiler-generated type
        PreCullingSystem.OverflowAction overflowAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_OverflowActions.TryDequeue(out overflowAction))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ref PreCullingData local = ref this.m_CullingData.ElementAt(overflowAction.m_DataIndex);
          // ISSUE: reference to a compiler-generated field
          local.m_Entity = overflowAction.m_Entity;
          // ISSUE: reference to a compiler-generated field
          local.m_UpdateFrame = overflowAction.m_UpdateFrame;
          local.m_Flags = PreCullingFlags.PassedCulling | PreCullingFlags.NearCamera | PreCullingFlags.NearCameraUpdated;
          local.m_Timer = (byte) 0;
        }
      }
    }

    [BurstCompile]
    private struct FilterUpdatesJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Created> m_CreatedData;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Applied> m_AppliedData;
      [ReadOnly]
      public ComponentLookup<BatchesUpdated> m_BatchesUpdatedData;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> m_InterpolatedTransformData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Object> m_ObjectData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometry> m_ObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Color> m_ObjectColorData;
      [ReadOnly]
      public ComponentLookup<Plant> m_PlantData;
      [ReadOnly]
      public ComponentLookup<Tree> m_TreeData;
      [ReadOnly]
      public ComponentLookup<Relative> m_RelativeData;
      [ReadOnly]
      public ComponentLookup<Damaged> m_DamagedData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Extension> m_ExtensionData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<NodeColor> m_NodeColorData;
      [ReadOnly]
      public ComponentLookup<EdgeColor> m_EdgeColorData;
      [ReadOnly]
      public ComponentLookup<LaneColor> m_LaneColorData;
      [ReadOnly]
      public ComponentLookup<LaneCondition> m_LaneConditionData;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> m_ZoneData;
      [ReadOnly]
      public ComponentLookup<OnFire> m_OnFireData;
      [ReadOnly]
      public BufferLookup<Animated> m_AnimatedData;
      [ReadOnly]
      public BufferLookup<Skeleton> m_SkeletonData;
      [ReadOnly]
      public BufferLookup<Emissive> m_EmissiveData;
      [ReadOnly]
      public BufferLookup<MeshColor> m_MeshColorData;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [ReadOnly]
      public BufferLookup<EnabledEffect> m_EffectInstances;
      [ReadOnly]
      public int m_TimerDelta;
      [NativeDisableParallelForRestriction]
      public NativeList<PreCullingData> m_CullingData;
      public NativeList<PreCullingData>.ParallelWriter m_UpdatedCullingData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ref PreCullingData local = ref this.m_CullingData.ElementAt(index);
        if ((local.m_Flags & (PreCullingFlags.NearCameraUpdated | PreCullingFlags.Updated)) != (PreCullingFlags) 0)
        {
          local.m_Flags &= ~(PreCullingFlags.Updated | PreCullingFlags.Created | PreCullingFlags.Applied | PreCullingFlags.BatchesUpdated | PreCullingFlags.Temp | PreCullingFlags.Object | PreCullingFlags.Net | PreCullingFlags.Lane | PreCullingFlags.Zone | PreCullingFlags.InfoviewColor | PreCullingFlags.BuildingState | PreCullingFlags.TreeGrowth | PreCullingFlags.LaneCondition | PreCullingFlags.InterpolatedTransform | PreCullingFlags.Animated | PreCullingFlags.Skeleton | PreCullingFlags.Emissive | PreCullingFlags.VehicleLayout | PreCullingFlags.EffectInstances | PreCullingFlags.Relative | PreCullingFlags.SurfaceState | PreCullingFlags.SurfaceDamage | PreCullingFlags.SmoothColor);
          // ISSUE: reference to a compiler-generated field
          if (this.m_CreatedData.HasComponent(local.m_Entity))
            local.m_Flags |= PreCullingFlags.Updated | PreCullingFlags.Created;
          // ISSUE: reference to a compiler-generated field
          if ((local.m_Flags & PreCullingFlags.Updated) != (PreCullingFlags) 0 || this.m_UpdatedData.HasComponent(local.m_Entity))
            local.m_Flags |= PreCullingFlags.Updated;
          // ISSUE: reference to a compiler-generated field
          if (this.m_AppliedData.HasComponent(local.m_Entity))
            local.m_Flags |= PreCullingFlags.Applied;
          // ISSUE: reference to a compiler-generated field
          if (this.m_BatchesUpdatedData.HasComponent(local.m_Entity))
            local.m_Flags |= PreCullingFlags.BatchesUpdated;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TempData.HasComponent(local.m_Entity))
            local.m_Flags |= PreCullingFlags.Temp;
          // ISSUE: reference to a compiler-generated field
          if (this.m_EffectInstances.HasBuffer(local.m_Entity))
            local.m_Flags |= PreCullingFlags.EffectInstances;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ObjectData.HasComponent(local.m_Entity))
          {
            local.m_Flags |= PreCullingFlags.Object;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ObjectGeometryData.HasComponent(local.m_Entity))
              local.m_Flags |= PreCullingFlags.SurfaceState;
            // ISSUE: reference to a compiler-generated field
            if (this.m_InterpolatedTransformData.HasComponent(local.m_Entity))
              local.m_Flags |= PreCullingFlags.InterpolatedTransform;
            // ISSUE: reference to a compiler-generated field
            if (this.m_AnimatedData.HasBuffer(local.m_Entity))
              local.m_Flags |= PreCullingFlags.Animated;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ObjectColorData.HasComponent(local.m_Entity) || this.m_OwnerData.HasComponent(local.m_Entity))
              local.m_Flags |= PreCullingFlags.InfoviewColor;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingData.HasComponent(local.m_Entity) || this.m_ExtensionData.HasComponent(local.m_Entity))
              local.m_Flags |= PreCullingFlags.BuildingState;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PlantData.HasComponent(local.m_Entity))
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_TreeData.HasComponent(local.m_Entity))
                local.m_Flags |= PreCullingFlags.TreeGrowth;
              // ISSUE: reference to a compiler-generated field
              if (this.m_MeshColorData.HasBuffer(local.m_Entity))
                local.m_Flags |= PreCullingFlags.SmoothColor;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_SkeletonData.HasBuffer(local.m_Entity))
              local.m_Flags |= PreCullingFlags.Skeleton;
            // ISSUE: reference to a compiler-generated field
            if (this.m_EmissiveData.HasBuffer(local.m_Entity))
              local.m_Flags |= PreCullingFlags.Emissive;
            // ISSUE: reference to a compiler-generated field
            if (this.m_LayoutElements.HasBuffer(local.m_Entity))
              local.m_Flags |= PreCullingFlags.VehicleLayout;
            // ISSUE: reference to a compiler-generated field
            if (this.m_RelativeData.HasComponent(local.m_Entity))
              local.m_Flags |= PreCullingFlags.Relative;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_DamagedData.HasComponent(local.m_Entity) || this.m_OnFireData.HasComponent(local.m_Entity))
              local.m_Flags |= PreCullingFlags.SurfaceDamage;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_EdgeData.HasComponent(local.m_Entity))
            {
              local.m_Flags |= PreCullingFlags.Net;
              // ISSUE: reference to a compiler-generated field
              if (this.m_EdgeColorData.HasComponent(local.m_Entity))
                local.m_Flags |= PreCullingFlags.InfoviewColor;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_NodeData.HasComponent(local.m_Entity))
              {
                local.m_Flags |= PreCullingFlags.Net;
                // ISSUE: reference to a compiler-generated field
                if (this.m_NodeColorData.HasComponent(local.m_Entity))
                  local.m_Flags |= PreCullingFlags.InfoviewColor;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_LaneData.HasComponent(local.m_Entity))
                {
                  local.m_Flags |= PreCullingFlags.Lane;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PlantData.HasComponent(local.m_Entity) && this.m_MeshColorData.HasBuffer(local.m_Entity))
                    local.m_Flags |= PreCullingFlags.SmoothColor;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_LaneColorData.HasComponent(local.m_Entity) || this.m_OwnerData.HasComponent(local.m_Entity))
                    local.m_Flags |= PreCullingFlags.InfoviewColor;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_LaneConditionData.HasComponent(local.m_Entity))
                    local.m_Flags |= PreCullingFlags.LaneCondition;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ZoneData.HasComponent(local.m_Entity))
                    local.m_Flags |= PreCullingFlags.Zone;
                }
              }
            }
          }
        }
        if ((local.m_Flags & PreCullingFlags.PassedCulling) == (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          int num = (int) local.m_Timer + this.m_TimerDelta;
          if (num >= (int) byte.MaxValue)
          {
            local.m_Flags &= ~PreCullingFlags.NearCamera;
            local.m_Flags |= PreCullingFlags.NearCameraUpdated;
            local.m_Timer = byte.MaxValue;
          }
          else
            local.m_Timer = (byte) num;
        }
        if ((local.m_Flags & (PreCullingFlags.NearCameraUpdated | PreCullingFlags.Updated | PreCullingFlags.BatchesUpdated | PreCullingFlags.FadeContainer | PreCullingFlags.ColorsUpdated)) == (PreCullingFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_UpdatedCullingData.AddNoResize(local);
      }
    }

    private struct TypeHandle
    {
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Updated> __Game_Common_Updated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BatchesUpdated> __Game_Common_BatchesUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Overridden> __Game_Common_Overridden_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Stack> __Game_Objects_Stack_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Marker> __Game_Objects_Marker_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> __Game_Objects_Unspawned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Node> __Game_Net_Node_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<NodeGeometry> __Game_Net_NodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Composition> __Game_Net_Composition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Orphan> __Game_Net_Orphan_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.UtilityLane> __Game_Net_UtilityLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Marker> __Game_Net_Marker_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Zones.Block> __Game_Zones_Block_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<TransformFrame> __Game_Objects_TransformFrame_RO_BufferTypeHandle;
      public ComponentTypeHandle<CullingInfo> __Game_Rendering_CullingInfo_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StackData> __Game_Prefabs_StackData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneGeometryData> __Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> __Game_Prefabs_UtilityLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshRef> __Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshData> __Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<RentersUpdated> __Game_Buildings_RentersUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ColorUpdated> __Game_Routes_ColorUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<CullingInfo> __Game_Rendering_CullingInfo_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteVehicle> __Game_Routes_RouteVehicle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Applied> __Game_Common_Applied_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle;
      public ComponentLookup<CullingInfo> __Game_Rendering_CullingInfo_RW_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Static> __Game_Objects_Static_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Stopped> __Game_Objects_Stopped_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Created> __Game_Common_Created_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Applied> __Game_Common_Applied_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BatchesUpdated> __Game_Common_BatchesUpdated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Object> __Game_Objects_Object_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometry> __Game_Objects_ObjectGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Color> __Game_Objects_Color_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Plant> __Game_Objects_Plant_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Tree> __Game_Objects_Tree_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Relative> __Game_Objects_Relative_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Damaged> __Game_Objects_Damaged_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Extension> __Game_Buildings_Extension_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NodeColor> __Game_Net_NodeColor_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeColor> __Game_Net_EdgeColor_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LaneColor> __Game_Net_LaneColor_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LaneCondition> __Game_Net_LaneCondition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> __Game_Zones_Block_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OnFire> __Game_Events_OnFire_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Animated> __Game_Rendering_Animated_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Skeleton> __Game_Rendering_Skeleton_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Emissive> __Game_Rendering_Emissive_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshColor> __Game_Rendering_MeshColor_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<EnabledEffect> __Game_Effects_EnabledEffect_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_BatchesUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BatchesUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Overridden_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Overridden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stack_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Stack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Marker_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Marker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Orphan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_UtilityLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.UtilityLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Marker_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Marker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Block_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Zones.Block>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RO_BufferTypeHandle = state.GetBufferTypeHandle<TransformFrame>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CullingInfo>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StackData_RO_ComponentLookup = state.GetComponentLookup<StackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetLaneGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup = state.GetComponentLookup<UtilityLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup = state.GetComponentLookup<NetCompositionMeshRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionMeshData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_RentersUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RentersUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_ColorUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ColorUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RO_ComponentLookup = state.GetComponentLookup<CullingInfo>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteVehicle_RO_BufferLookup = state.GetBufferLookup<RouteVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Applied_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Applied>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RW_ComponentLookup = state.GetComponentLookup<CullingInfo>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InterpolatedTransform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Static_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Static>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stopped_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Stopped>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentLookup = state.GetComponentLookup<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Applied_RO_ComponentLookup = state.GetComponentLookup<Applied>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_BatchesUpdated_RO_ComponentLookup = state.GetComponentLookup<BatchesUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup = state.GetComponentLookup<InterpolatedTransform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Object_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Object>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_ObjectGeometry_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Color_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Color>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Plant_RO_ComponentLookup = state.GetComponentLookup<Plant>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentLookup = state.GetComponentLookup<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Relative_RO_ComponentLookup = state.GetComponentLookup<Relative>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RO_ComponentLookup = state.GetComponentLookup<Damaged>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Extension_RO_ComponentLookup = state.GetComponentLookup<Extension>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeColor_RO_ComponentLookup = state.GetComponentLookup<NodeColor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeColor_RO_ComponentLookup = state.GetComponentLookup<EdgeColor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneColor_RO_ComponentLookup = state.GetComponentLookup<LaneColor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneCondition_RO_ComponentLookup = state.GetComponentLookup<LaneCondition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Block_RO_ComponentLookup = state.GetComponentLookup<Game.Zones.Block>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_OnFire_RO_ComponentLookup = state.GetComponentLookup<OnFire>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Animated_RO_BufferLookup = state.GetBufferLookup<Animated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Skeleton_RO_BufferLookup = state.GetBufferLookup<Skeleton>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Emissive_RO_BufferLookup = state.GetBufferLookup<Emissive>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshColor_RO_BufferLookup = state.GetBufferLookup<MeshColor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Effects_EnabledEffect_RO_BufferLookup = state.GetBufferLookup<EnabledEffect>(true);
      }
    }
  }
}
