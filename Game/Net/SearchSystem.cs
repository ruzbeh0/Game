// Decompiled with JetBrains decompiler
// Type: Game.Net.SearchSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Prefabs;
using Game.Rendering;
using Game.Serialization;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class SearchSystem : GameSystemBase, IPreDeserialize
  {
    private ToolSystem m_ToolSystem;
    private UndergroundViewSystem m_UndergroundViewSystem;
    private EntityQuery m_UpdatedNetsQuery;
    private EntityQuery m_UpdatedLanesQuery;
    private EntityQuery m_AllNetsQuery;
    private EntityQuery m_AllLanesQuery;
    private NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
    private NativeQuadTree<Entity, QuadTreeBoundsXZ> m_LaneSearchTree;
    private JobHandle m_NetReadDependencies;
    private JobHandle m_NetWriteDependencies;
    private JobHandle m_LaneReadDependencies;
    private JobHandle m_LaneWriteDependencies;
    private bool m_Loaded;
    private SearchSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UndergroundViewSystem = this.World.GetOrCreateSystemManaged<UndergroundViewSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedNetsQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Edge>()
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
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Node>()
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
      this.m_UpdatedLanesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<LaneGeometry>(),
          ComponentType.ReadOnly<ParkingLane>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<LaneGeometry>(),
          ComponentType.ReadOnly<ParkingLane>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AllNetsQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Edge>(),
          ComponentType.ReadOnly<Node>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AllLanesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<LaneGeometry>(),
          ComponentType.ReadOnly<ParkingLane>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchTree = new NativeQuadTree<Entity, QuadTreeBoundsXZ>(1f, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LaneSearchTree = new NativeQuadTree<Entity, QuadTreeBoundsXZ>(1f, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchTree.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_LaneSearchTree.Dispose();
      base.OnDestroy();
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
      // ISSUE: reference to a compiler-generated field
      EntityQuery query1 = loaded ? this.m_AllNetsQuery : this.m_UpdatedNetsQuery;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery query2 = loaded ? this.m_AllLanesQuery : this.m_UpdatedLanesQuery;
      bool flag1 = !query1.IsEmptyIgnoreFilter;
      bool flag2 = !query2.IsEmptyIgnoreFilter;
      if (!flag1 && !flag2)
        return;
      JobHandle job0 = new JobHandle();
      if (flag1)
      {
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
        this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Marker_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Orphan_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        JobHandle jobHandle = new SearchSystem.UpdateNetSearchTreeJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_EdgeGeometryType = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle,
          m_StartGeometryType = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle,
          m_EndGeometryType = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle,
          m_NodeGeometryType = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentTypeHandle,
          m_CompositionType = this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle,
          m_OrphanType = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentTypeHandle,
          m_NodeType = this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle,
          m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
          m_MarkerType = this.__TypeHandle.__Game_Net_Marker_RO_ComponentTypeHandle,
          m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
          m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
          m_CullingInfoType = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle,
          m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
          m_PrefabCompositionMeshRef = this.__TypeHandle.__Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup,
          m_PrefabCompositionMeshData = this.__TypeHandle.__Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup,
          m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
          m_Loaded = loaded,
          m_SearchTree = this.GetNetSearchTree(false, out dependencies)
        }.Schedule<SearchSystem.UpdateNetSearchTreeJob>(query1, JobHandle.CombineDependencies(this.Dependency, dependencies));
        // ISSUE: reference to a compiler-generated method
        this.AddNetSearchTreeWriter(jobHandle);
        job0 = JobHandle.CombineDependencies(job0, jobHandle);
      }
      if (flag2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Overridden_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        JobHandle jobHandle = new SearchSystem.UpdateLaneSearchTreeJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
          m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
          m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
          m_OverriddenType = this.__TypeHandle.__Game_Common_Overridden_RO_ComponentTypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_UtilityLaneType = this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_PrefabLaneGeometryData = this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup,
          m_PrefabUtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
          m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
          m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
          m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
          m_Loaded = loaded,
          m_DilatedUtilityTypes = this.m_UndergroundViewSystem.utilityTypes,
          m_SearchTree = this.GetLaneSearchTree(false, out dependencies)
        }.Schedule<SearchSystem.UpdateLaneSearchTreeJob>(query2, JobHandle.CombineDependencies(this.Dependency, dependencies));
        // ISSUE: reference to a compiler-generated method
        this.AddLaneSearchTreeWriter(jobHandle);
        job0 = JobHandle.CombineDependencies(job0, jobHandle);
      }
      this.Dependency = job0;
    }

    public NativeQuadTree<Entity, QuadTreeBoundsXZ> GetNetSearchTree(
      bool readOnly,
      out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = readOnly ? this.m_NetWriteDependencies : JobHandle.CombineDependencies(this.m_NetReadDependencies, this.m_NetWriteDependencies);
      // ISSUE: reference to a compiler-generated field
      return this.m_NetSearchTree;
    }

    public NativeQuadTree<Entity, QuadTreeBoundsXZ> GetLaneSearchTree(
      bool readOnly,
      out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = readOnly ? this.m_LaneWriteDependencies : JobHandle.CombineDependencies(this.m_LaneReadDependencies, this.m_LaneWriteDependencies);
      // ISSUE: reference to a compiler-generated field
      return this.m_LaneSearchTree;
    }

    public void AddNetSearchTreeReader(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_NetReadDependencies = JobHandle.CombineDependencies(this.m_NetReadDependencies, jobHandle);
    }

    public void AddNetSearchTreeWriter(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_NetWriteDependencies = jobHandle;
    }

    public void AddLaneSearchTreeReader(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LaneReadDependencies = JobHandle.CombineDependencies(this.m_LaneReadDependencies, jobHandle);
    }

    public void AddLaneSearchTreeWriter(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LaneWriteDependencies = jobHandle;
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated method
      NativeQuadTree<Entity, QuadTreeBoundsXZ> netSearchTree = this.GetNetSearchTree(false, out dependencies1);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated method
      NativeQuadTree<Entity, QuadTreeBoundsXZ> laneSearchTree = this.GetLaneSearchTree(false, out dependencies2);
      dependencies1.Complete();
      dependencies2.Complete();
      netSearchTree.Clear();
      laneSearchTree.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    public static MeshLayer GetLayers(
      Owner owner,
      UtilityLane utilityLane,
      MeshLayer defaultLayers,
      ref ComponentLookup<PrefabRef> prefabRefs,
      ref ComponentLookup<NetData> netDatas,
      ref ComponentLookup<NetGeometryData> netGeometryDatas)
    {
      if (defaultLayers != (MeshLayer.Pipeline | MeshLayer.SubPipeline))
        return defaultLayers;
      // ISSUE: reference to a compiler-generated method
      return owner.m_Owner != Entity.Null && SearchSystem.IsNetOwnerPipeline(owner, ref prefabRefs, ref netDatas, ref netGeometryDatas) || (utilityLane.m_Flags & UtilityLaneFlags.PipelineConnection) != (UtilityLaneFlags) 0 ? MeshLayer.Pipeline : MeshLayer.SubPipeline;
    }

    public static bool IsNetOwnerPipeline(
      Owner owner,
      ref ComponentLookup<PrefabRef> prefabRefs,
      ref ComponentLookup<NetData> netDatas,
      ref ComponentLookup<NetGeometryData> netGeometryDatas)
    {
      PrefabRef componentData1;
      NetData componentData2;
      NetGeometryData componentData3;
      return prefabRefs.TryGetComponent(owner.m_Owner, out componentData1) && netDatas.TryGetComponent(componentData1.m_Prefab, out componentData2) && netGeometryDatas.TryGetComponent(componentData1.m_Prefab, out componentData3) && (componentData2.m_RequiredLayers & (Layer.PowerlineLow | Layer.PowerlineHigh | Layer.WaterPipe | Layer.SewagePipe)) != Layer.None && (componentData3.m_Flags & GeometryFlags.Marker) == (GeometryFlags) 0;
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
    public SearchSystem()
    {
    }

    [BurstCompile]
    private struct UpdateNetSearchTreeJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> m_EdgeGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<StartNodeGeometry> m_StartGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<EndNodeGeometry> m_EndGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<NodeGeometry> m_NodeGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<Composition> m_CompositionType;
      [ReadOnly]
      public ComponentTypeHandle<Orphan> m_OrphanType;
      [ReadOnly]
      public ComponentTypeHandle<Node> m_NodeType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<Marker> m_MarkerType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<CullingInfo> m_CullingInfoType;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshRef> m_PrefabCompositionMeshRef;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshData> m_PrefabCompositionMeshData;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public bool m_Loaded;
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SearchTree.TryRemove(nativeArray[index]);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Loaded || chunk.Has<Created>(ref this.m_CreatedType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            bool flag1 = chunk.Has<CullingInfo>(ref this.m_CullingInfoType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<EdgeGeometry> nativeArray2 = chunk.GetNativeArray<EdgeGeometry>(ref this.m_EdgeGeometryType);
            if (nativeArray2.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<StartNodeGeometry> nativeArray3 = chunk.GetNativeArray<StartNodeGeometry>(ref this.m_StartGeometryType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<EndNodeGeometry> nativeArray4 = chunk.GetNativeArray<EndNodeGeometry>(ref this.m_EndGeometryType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<Composition> nativeArray5 = chunk.GetNativeArray<Composition>(ref this.m_CompositionType);
              // ISSUE: reference to a compiler-generated field
              bool flag2 = chunk.Has<Marker>(ref this.m_MarkerType);
              for (int index = 0; index < nativeArray1.Length; ++index)
              {
                Entity entity = nativeArray1[index];
                EdgeGeometry edgeGeometry = nativeArray2[index];
                EdgeNodeGeometry geometry1 = nativeArray3[index].m_Geometry;
                EdgeNodeGeometry geometry2 = nativeArray4[index].m_Geometry;
                Bounds3 bounds = edgeGeometry.m_Bounds | geometry1.m_Bounds | geometry2.m_Bounds;
                Composition composition = nativeArray5[index];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                int lod = math.min(this.m_PrefabCompositionData[composition.m_Edge].m_MinLod, math.min(this.m_PrefabCompositionData[composition.m_StartNode].m_MinLod, this.m_PrefabCompositionData[composition.m_EndNode].m_MinLod));
                BoundsMask mask = BoundsMask.Debug;
                // ISSUE: reference to a compiler-generated field
                if (!flag2 || this.m_EditorMode)
                {
                  NetCompositionMeshData componentData1;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (math.any(edgeGeometry.m_Start.m_Length + edgeGeometry.m_End.m_Length > 0.1f) && this.m_PrefabCompositionMeshData.TryGetComponent(this.m_PrefabCompositionMeshRef[composition.m_Edge].m_Mesh, out componentData1))
                    mask |= componentData1.m_DefaultLayers == (MeshLayer) 0 ? BoundsMask.NormalLayers : CommonUtils.GetBoundsMask(componentData1.m_DefaultLayers);
                  NetCompositionMeshData componentData2;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (math.any(geometry1.m_Left.m_Length > 0.05f) | math.any(geometry1.m_Right.m_Length > 0.05f) && this.m_PrefabCompositionMeshData.TryGetComponent(this.m_PrefabCompositionMeshRef[composition.m_StartNode].m_Mesh, out componentData2))
                    mask |= componentData2.m_DefaultLayers == (MeshLayer) 0 ? BoundsMask.NormalLayers : CommonUtils.GetBoundsMask(componentData2.m_DefaultLayers);
                  NetCompositionMeshData componentData3;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (math.any(geometry2.m_Left.m_Length > 0.05f) | math.any(geometry2.m_Right.m_Length > 0.05f) && this.m_PrefabCompositionMeshData.TryGetComponent(this.m_PrefabCompositionMeshRef[composition.m_EndNode].m_Mesh, out componentData3))
                    mask |= componentData3.m_DefaultLayers == (MeshLayer) 0 ? BoundsMask.NormalLayers : CommonUtils.GetBoundsMask(componentData3.m_DefaultLayers);
                }
                if (!flag1)
                  mask &= ~(BoundsMask.AllLayers | BoundsMask.Debug);
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Add(entity, new QuadTreeBoundsXZ(bounds, mask, lod));
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<NodeGeometry> nativeArray6 = chunk.GetNativeArray<NodeGeometry>(ref this.m_NodeGeometryType);
              if (nativeArray6.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                NativeArray<Orphan> nativeArray7 = chunk.GetNativeArray<Orphan>(ref this.m_OrphanType);
                // ISSUE: reference to a compiler-generated field
                bool flag3 = chunk.Has<Marker>(ref this.m_MarkerType);
                for (int index = 0; index < nativeArray1.Length; ++index)
                {
                  Entity entity = nativeArray1[index];
                  Bounds3 bounds = nativeArray6[index].m_Bounds;
                  BoundsMask mask = BoundsMask.Debug;
                  int lod;
                  if (nativeArray7.Length != 0)
                  {
                    Orphan orphan = nativeArray7[index];
                    // ISSUE: reference to a compiler-generated field
                    lod = this.m_PrefabCompositionData[orphan.m_Composition].m_MinLod;
                    NetCompositionMeshData componentData;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if ((!flag3 || this.m_EditorMode) && this.m_PrefabCompositionMeshData.TryGetComponent(this.m_PrefabCompositionMeshRef[orphan.m_Composition].m_Mesh, out componentData))
                      mask |= componentData.m_DefaultLayers == (MeshLayer) 0 ? BoundsMask.NormalLayers : CommonUtils.GetBoundsMask(componentData.m_DefaultLayers);
                  }
                  else
                    lod = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float2(2f)));
                  if (!flag1)
                    mask &= ~(BoundsMask.AllLayers | BoundsMask.Debug);
                  // ISSUE: reference to a compiler-generated field
                  this.m_SearchTree.Add(entity, new QuadTreeBoundsXZ(bounds, mask, lod));
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                NativeArray<Node> nativeArray8 = chunk.GetNativeArray<Node>(ref this.m_NodeType);
                if (nativeArray8.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  BoundsMask mask = this.m_EditorMode ? BoundsMask.Debug | BoundsMask.NormalLayers : BoundsMask.Debug;
                  if (!flag1)
                    mask &= ~(BoundsMask.AllLayers | BoundsMask.Debug);
                  for (int index = 0; index < nativeArray1.Length; ++index)
                  {
                    Entity entity = nativeArray1[index];
                    Node node = nativeArray8[index];
                    Bounds3 bounds = new Bounds3(node.m_Position - 1f, node.m_Position + 1f);
                    int lodLimit = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float2(2f)));
                    // ISSUE: reference to a compiler-generated field
                    this.m_SearchTree.Add(entity, new QuadTreeBoundsXZ(bounds, mask, lodLimit));
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  NativeArray<Curve> nativeArray9 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
                  if (nativeArray9.Length == 0)
                    return;
                  // ISSUE: reference to a compiler-generated field
                  BoundsMask mask = this.m_EditorMode ? BoundsMask.Debug | BoundsMask.NormalLayers : BoundsMask.Debug;
                  if (!flag1)
                    mask &= ~(BoundsMask.AllLayers | BoundsMask.Debug);
                  for (int index = 0; index < nativeArray1.Length; ++index)
                  {
                    Entity entity = nativeArray1[index];
                    Bounds3 bounds = MathUtils.Expand(MathUtils.Bounds(nativeArray9[index].m_Bezier), (float3) 0.5f);
                    int lodLimit = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float3(2f)));
                    // ISSUE: reference to a compiler-generated field
                    this.m_SearchTree.Add(entity, new QuadTreeBoundsXZ(bounds, mask, lodLimit));
                  }
                }
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray10 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            bool flag4 = chunk.Has<CullingInfo>(ref this.m_CullingInfoType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<EdgeGeometry> nativeArray11 = chunk.GetNativeArray<EdgeGeometry>(ref this.m_EdgeGeometryType);
            if (nativeArray11.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<StartNodeGeometry> nativeArray12 = chunk.GetNativeArray<StartNodeGeometry>(ref this.m_StartGeometryType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<EndNodeGeometry> nativeArray13 = chunk.GetNativeArray<EndNodeGeometry>(ref this.m_EndGeometryType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<Composition> nativeArray14 = chunk.GetNativeArray<Composition>(ref this.m_CompositionType);
              // ISSUE: reference to a compiler-generated field
              bool flag5 = chunk.Has<Marker>(ref this.m_MarkerType);
              for (int index = 0; index < nativeArray10.Length; ++index)
              {
                Entity entity = nativeArray10[index];
                EdgeGeometry edgeGeometry = nativeArray11[index];
                EdgeNodeGeometry geometry3 = nativeArray12[index].m_Geometry;
                EdgeNodeGeometry geometry4 = nativeArray13[index].m_Geometry;
                Bounds3 bounds = edgeGeometry.m_Bounds | geometry3.m_Bounds | geometry4.m_Bounds;
                Composition composition = nativeArray14[index];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                int lod = math.min(this.m_PrefabCompositionData[composition.m_Edge].m_MinLod, math.min(this.m_PrefabCompositionData[composition.m_StartNode].m_MinLod, this.m_PrefabCompositionData[composition.m_EndNode].m_MinLod));
                BoundsMask mask = BoundsMask.Debug;
                // ISSUE: reference to a compiler-generated field
                if (!flag5 || this.m_EditorMode)
                {
                  NetCompositionMeshData componentData4;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (math.any(edgeGeometry.m_Start.m_Length + edgeGeometry.m_End.m_Length > 0.1f) && this.m_PrefabCompositionMeshData.TryGetComponent(this.m_PrefabCompositionMeshRef[composition.m_Edge].m_Mesh, out componentData4))
                    mask |= componentData4.m_DefaultLayers == (MeshLayer) 0 ? BoundsMask.NormalLayers : CommonUtils.GetBoundsMask(componentData4.m_DefaultLayers);
                  NetCompositionMeshData componentData5;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (math.any(geometry3.m_Left.m_Length > 0.05f) | math.any(geometry3.m_Right.m_Length > 0.05f) && this.m_PrefabCompositionMeshData.TryGetComponent(this.m_PrefabCompositionMeshRef[composition.m_StartNode].m_Mesh, out componentData5))
                    mask |= componentData5.m_DefaultLayers == (MeshLayer) 0 ? BoundsMask.NormalLayers : CommonUtils.GetBoundsMask(componentData5.m_DefaultLayers);
                  NetCompositionMeshData componentData6;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (math.any(geometry4.m_Left.m_Length > 0.05f) | math.any(geometry4.m_Right.m_Length > 0.05f) && this.m_PrefabCompositionMeshData.TryGetComponent(this.m_PrefabCompositionMeshRef[composition.m_EndNode].m_Mesh, out componentData6))
                    mask |= componentData6.m_DefaultLayers == (MeshLayer) 0 ? BoundsMask.NormalLayers : CommonUtils.GetBoundsMask(componentData6.m_DefaultLayers);
                }
                if (!flag4)
                  mask &= ~(BoundsMask.AllLayers | BoundsMask.Debug);
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Update(entity, new QuadTreeBoundsXZ(bounds, mask, lod));
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<NodeGeometry> nativeArray15 = chunk.GetNativeArray<NodeGeometry>(ref this.m_NodeGeometryType);
              if (nativeArray15.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                NativeArray<Orphan> nativeArray16 = chunk.GetNativeArray<Orphan>(ref this.m_OrphanType);
                // ISSUE: reference to a compiler-generated field
                bool flag6 = chunk.Has<Marker>(ref this.m_MarkerType);
                for (int index = 0; index < nativeArray10.Length; ++index)
                {
                  Entity entity = nativeArray10[index];
                  Bounds3 bounds = nativeArray15[index].m_Bounds;
                  BoundsMask mask = BoundsMask.Debug;
                  int lod;
                  if (nativeArray16.Length != 0)
                  {
                    Orphan orphan = nativeArray16[index];
                    // ISSUE: reference to a compiler-generated field
                    lod = this.m_PrefabCompositionData[orphan.m_Composition].m_MinLod;
                    NetCompositionMeshData componentData;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if ((!flag6 || this.m_EditorMode) && this.m_PrefabCompositionMeshData.TryGetComponent(this.m_PrefabCompositionMeshRef[orphan.m_Composition].m_Mesh, out componentData))
                      mask |= componentData.m_DefaultLayers == (MeshLayer) 0 ? BoundsMask.NormalLayers : CommonUtils.GetBoundsMask(componentData.m_DefaultLayers);
                  }
                  else
                    lod = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float2(2f)));
                  if (!flag4)
                    mask &= ~(BoundsMask.AllLayers | BoundsMask.Debug);
                  // ISSUE: reference to a compiler-generated field
                  this.m_SearchTree.Update(entity, new QuadTreeBoundsXZ(bounds, mask, lod));
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                NativeArray<Node> nativeArray17 = chunk.GetNativeArray<Node>(ref this.m_NodeType);
                if (nativeArray17.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  BoundsMask mask = this.m_EditorMode ? BoundsMask.Debug | BoundsMask.NormalLayers : BoundsMask.Debug;
                  if (!flag4)
                    mask &= ~(BoundsMask.AllLayers | BoundsMask.Debug);
                  for (int index = 0; index < nativeArray10.Length; ++index)
                  {
                    Entity entity = nativeArray10[index];
                    Node node = nativeArray17[index];
                    Bounds3 bounds = new Bounds3(node.m_Position - 1f, node.m_Position + 1f);
                    int lodLimit = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float2(2f)));
                    // ISSUE: reference to a compiler-generated field
                    this.m_SearchTree.Update(entity, new QuadTreeBoundsXZ(bounds, mask, lodLimit));
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  NativeArray<Curve> nativeArray18 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
                  if (nativeArray18.Length == 0)
                    return;
                  // ISSUE: reference to a compiler-generated field
                  BoundsMask mask = this.m_EditorMode ? BoundsMask.Debug | BoundsMask.NormalLayers : BoundsMask.Debug;
                  if (!flag4)
                    mask &= ~(BoundsMask.AllLayers | BoundsMask.Debug);
                  for (int index = 0; index < nativeArray10.Length; ++index)
                  {
                    Entity entity = nativeArray10[index];
                    Bounds3 bounds = MathUtils.Expand(MathUtils.Bounds(nativeArray18[index].m_Bezier), (float3) 0.5f);
                    int lodLimit = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float2(2f)));
                    // ISSUE: reference to a compiler-generated field
                    this.m_SearchTree.Update(entity, new QuadTreeBoundsXZ(bounds, mask, lodLimit));
                  }
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
    public struct UpdateLaneSearchTreeJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<Overridden> m_OverriddenType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<UtilityLane> m_UtilityLaneType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetLaneGeometryData> m_PrefabLaneGeometryData;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> m_PrefabUtilityLaneData;
      [ReadOnly]
      public ComponentLookup<NetData> m_PrefabNetData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public bool m_Loaded;
      [ReadOnly]
      public UtilityTypes m_DilatedUtilityTypes;
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SearchTree.TryRemove(nativeArray[index]);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Loaded || chunk.Has<Created>(ref this.m_CreatedType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Curve> nativeArray2 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<UtilityLane> nativeArray4 = chunk.GetNativeArray<UtilityLane>(ref this.m_UtilityLaneType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            bool flag1 = chunk.Has<Overridden>(ref this.m_OverriddenType);
            bool flag2 = chunk.Has<CullingInfo>();
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              Curve curve = nativeArray2[index];
              PrefabRef prefabRef = nativeArray5[index];
              Bounds3 bounds1 = MathUtils.Bounds(curve.m_Bezier);
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabLaneGeometryData.HasComponent(prefabRef.m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                NetLaneGeometryData laneGeometryData = this.m_PrefabLaneGeometryData[prefabRef.m_Prefab];
                Bounds3 bounds2 = MathUtils.Expand(bounds1, laneGeometryData.m_Size.xyx * 0.5f);
                BoundsMask mask = BoundsMask.Debug;
                if (!flag1)
                {
                  mask |= BoundsMask.NotOverridden;
                  if ((double) curve.m_Length > 0.10000000149011612)
                  {
                    // ISSUE: reference to a compiler-generated field
                    MeshLayer defaultLayers = this.m_EditorMode ? laneGeometryData.m_EditorLayers : laneGeometryData.m_GameLayers;
                    Owner owner;
                    CollectionUtils.TryGet<Owner>(nativeArray3, index, out owner);
                    UtilityLane utilityLane;
                    CollectionUtils.TryGet<UtilityLane>(nativeArray4, index, out utilityLane);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    mask |= CommonUtils.GetBoundsMask(SearchSystem.GetLayers(owner, utilityLane, defaultLayers, ref this.m_PrefabRefData, ref this.m_PrefabNetData, ref this.m_PrefabNetGeometryData));
                  }
                }
                int num = laneGeometryData.m_MinLod;
                UtilityLaneData componentData;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabUtilityLaneData.TryGetComponent(prefabRef.m_Prefab, out componentData) && (componentData.m_UtilityTypes & this.m_DilatedUtilityTypes) != UtilityTypes.None)
                {
                  float renderingSize = RenderingUtils.GetRenderingSize(new float2(componentData.m_VisualCapacity));
                  num = math.min(num, RenderingUtils.CalculateLodLimit(renderingSize));
                }
                if (!flag2)
                  mask &= ~(BoundsMask.AllLayers | BoundsMask.Debug);
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Add(entity, new QuadTreeBoundsXZ(bounds2, mask, num));
              }
              else
              {
                Bounds3 bounds3 = MathUtils.Expand(bounds1, (float3) 0.5f);
                int lodLimit = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float2(1f)));
                BoundsMask mask = BoundsMask.Debug;
                if (!flag2)
                  mask &= ~(BoundsMask.AllLayers | BoundsMask.Debug);
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Add(entity, new QuadTreeBoundsXZ(bounds3, mask, lodLimit));
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray6 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Curve> nativeArray7 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Owner> nativeArray8 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<UtilityLane> nativeArray9 = chunk.GetNativeArray<UtilityLane>(ref this.m_UtilityLaneType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray10 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            bool flag3 = chunk.Has<Overridden>(ref this.m_OverriddenType);
            bool flag4 = chunk.Has<CullingInfo>();
            for (int index = 0; index < nativeArray6.Length; ++index)
            {
              Entity entity = nativeArray6[index];
              Curve curve = nativeArray7[index];
              PrefabRef prefabRef = nativeArray10[index];
              Bounds3 bounds4 = MathUtils.Bounds(curve.m_Bezier);
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabLaneGeometryData.HasComponent(prefabRef.m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                NetLaneGeometryData laneGeometryData = this.m_PrefabLaneGeometryData[prefabRef.m_Prefab];
                Bounds3 bounds5 = MathUtils.Expand(bounds4, laneGeometryData.m_Size.xyx * 0.5f);
                BoundsMask mask = BoundsMask.Debug;
                if (!flag3)
                {
                  mask |= BoundsMask.NotOverridden;
                  if ((double) curve.m_Length > 0.10000000149011612)
                  {
                    // ISSUE: reference to a compiler-generated field
                    MeshLayer defaultLayers = this.m_EditorMode ? laneGeometryData.m_EditorLayers : laneGeometryData.m_GameLayers;
                    Owner owner;
                    CollectionUtils.TryGet<Owner>(nativeArray8, index, out owner);
                    UtilityLane utilityLane;
                    CollectionUtils.TryGet<UtilityLane>(nativeArray9, index, out utilityLane);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    mask |= CommonUtils.GetBoundsMask(SearchSystem.GetLayers(owner, utilityLane, defaultLayers, ref this.m_PrefabRefData, ref this.m_PrefabNetData, ref this.m_PrefabNetGeometryData));
                  }
                }
                int num = laneGeometryData.m_MinLod;
                UtilityLaneData componentData;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabUtilityLaneData.TryGetComponent(prefabRef.m_Prefab, out componentData) && (componentData.m_UtilityTypes & this.m_DilatedUtilityTypes) != UtilityTypes.None)
                {
                  float renderingSize = RenderingUtils.GetRenderingSize(new float2(componentData.m_VisualCapacity));
                  num = math.min(num, RenderingUtils.CalculateLodLimit(renderingSize));
                }
                if (!flag4)
                  mask &= ~(BoundsMask.AllLayers | BoundsMask.Debug);
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Update(entity, new QuadTreeBoundsXZ(bounds5, mask, num));
              }
              else
              {
                Bounds3 bounds6 = MathUtils.Expand(bounds4, (float3) 0.5f);
                int lodLimit = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float2(1f)));
                BoundsMask mask = BoundsMask.Debug;
                if (!flag4)
                  mask &= ~(BoundsMask.AllLayers | BoundsMask.Debug);
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Update(entity, new QuadTreeBoundsXZ(bounds6, mask, lodLimit));
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

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<NodeGeometry> __Game_Net_NodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Composition> __Game_Net_Composition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Orphan> __Game_Net_Orphan_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Node> __Game_Net_Node_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Marker> __Game_Net_Marker_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CullingInfo> __Game_Rendering_CullingInfo_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshRef> __Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshData> __Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Overridden> __Game_Common_Overridden_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<UtilityLane> __Game_Net_UtilityLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneGeometryData> __Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> __Game_Prefabs_UtilityLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Orphan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Marker_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Marker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CullingInfo>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup = state.GetComponentLookup<NetCompositionMeshRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionMeshData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Overridden_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Overridden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_UtilityLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<UtilityLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetLaneGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup = state.GetComponentLookup<UtilityLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
      }
    }
  }
}
