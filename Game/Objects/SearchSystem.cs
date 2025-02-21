// Decompiled with JetBrains decompiler
// Type: Game.Objects.SearchSystem
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
namespace Game.Objects
{
  [CompilerGenerated]
  public class SearchSystem : GameSystemBase, IPreDeserialize
  {
    private ToolSystem m_ToolSystem;
    private EntityQuery m_UpdatedStaticsQuery;
    private EntityQuery m_AllStaticsQuery;
    private NativeQuadTree<Entity, QuadTreeBoundsXZ> m_StaticSearchTree;
    private NativeQuadTree<Entity, QuadTreeBoundsXZ> m_MovingSearchTree;
    private JobHandle m_StaticReadDependencies;
    private JobHandle m_StaticWriteDependencies;
    private JobHandle m_MovingReadDependencies;
    private JobHandle m_MovingWriteDependencies;
    private bool m_Loaded;
    private SearchSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedStaticsQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Object>(),
          ComponentType.ReadOnly<Static>()
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
      this.m_AllStaticsQuery = this.GetEntityQuery(ComponentType.ReadOnly<Object>(), ComponentType.ReadOnly<Static>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_StaticSearchTree = new NativeQuadTree<Entity, QuadTreeBoundsXZ>(1f, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_MovingSearchTree = new NativeQuadTree<Entity, QuadTreeBoundsXZ>(1f, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_StaticReadDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_StaticWriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_StaticSearchTree.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_MovingReadDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_MovingWriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_MovingSearchTree.Dispose();
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
      EntityQuery query = loaded ? this.m_AllStaticsQuery : this.m_UpdatedStaticsQuery;
      if (query.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SearchSystem.UpdateSearchTreeJob jobData = new SearchSystem.UpdateSearchTreeJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_StackType = this.__TypeHandle.__Game_Objects_Stack_RO_ComponentTypeHandle,
        m_MarkerType = this.__TypeHandle.__Game_Objects_Marker_RO_ComponentTypeHandle,
        m_OutsideConnectionType = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle,
        m_TreeType = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_OverriddenType = this.__TypeHandle.__Game_Common_Overridden_RO_ComponentTypeHandle,
        m_CullingInfoType = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabStackData = this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup,
        m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_Loaded = loaded,
        m_SearchTree = this.GetStaticSearchTree(false, out dependencies)
      };
      this.Dependency = jobData.Schedule<SearchSystem.UpdateSearchTreeJob>(query, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated method
      this.AddStaticSearchTreeWriter(this.Dependency);
    }

    public NativeQuadTree<Entity, QuadTreeBoundsXZ> GetStaticSearchTree(
      bool readOnly,
      out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = readOnly ? this.m_StaticWriteDependencies : JobHandle.CombineDependencies(this.m_StaticReadDependencies, this.m_StaticWriteDependencies);
      // ISSUE: reference to a compiler-generated field
      return this.m_StaticSearchTree;
    }

    public NativeQuadTree<Entity, QuadTreeBoundsXZ> GetMovingSearchTree(
      bool readOnly,
      out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = readOnly ? this.m_MovingWriteDependencies : JobHandle.CombineDependencies(this.m_MovingReadDependencies, this.m_MovingWriteDependencies);
      // ISSUE: reference to a compiler-generated field
      return this.m_MovingSearchTree;
    }

    public void AddStaticSearchTreeReader(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_StaticReadDependencies = JobHandle.CombineDependencies(this.m_StaticReadDependencies, jobHandle);
    }

    public void AddStaticSearchTreeWriter(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_StaticWriteDependencies = jobHandle;
    }

    public void AddMovingSearchTreeReader(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MovingReadDependencies = JobHandle.CombineDependencies(this.m_MovingReadDependencies, jobHandle);
    }

    public void AddMovingSearchTreeWriter(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_MovingWriteDependencies = jobHandle;
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated method
      NativeQuadTree<Entity, QuadTreeBoundsXZ> staticSearchTree = this.GetStaticSearchTree(false, out dependencies1);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated method
      NativeQuadTree<Entity, QuadTreeBoundsXZ> movingSearchTree = this.GetMovingSearchTree(false, out dependencies2);
      dependencies1.Complete();
      dependencies2.Complete();
      staticSearchTree.Clear();
      movingSearchTree.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
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
    private struct UpdateSearchTreeJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Stack> m_StackType;
      [ReadOnly]
      public ComponentTypeHandle<Marker> m_MarkerType;
      [ReadOnly]
      public ComponentTypeHandle<OutsideConnection> m_OutsideConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<Tree> m_TreeType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<Overridden> m_OverriddenType;
      [ReadOnly]
      public ComponentTypeHandle<CullingInfo> m_CullingInfoType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<StackData> m_PrefabStackData;
      [ReadOnly]
      public ComponentLookup<NetData> m_PrefabNetData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
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
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SearchTree.TryRemove(nativeArray1[index]);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Loaded || chunk.Has<Created>(ref this.m_CreatedType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Transform> nativeArray3 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Stack> nativeArray4 = chunk.GetNativeArray<Stack>(ref this.m_StackType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Owner> nativeArray5 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            bool flag1 = chunk.Has<Marker>(ref this.m_MarkerType) && !chunk.Has<OutsideConnection>(ref this.m_OutsideConnectionType);
            // ISSUE: reference to a compiler-generated field
            bool flag2 = chunk.Has<Overridden>(ref this.m_OverriddenType);
            // ISSUE: reference to a compiler-generated field
            bool flag3 = chunk.Has<Tree>(ref this.m_TreeType);
            // ISSUE: reference to a compiler-generated field
            bool flag4 = chunk.Has<CullingInfo>(ref this.m_CullingInfoType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              PrefabRef prefabRef = nativeArray2[index];
              Transform transform = nativeArray3[index];
              ObjectGeometryData componentData1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
              {
                StackData componentData2;
                // ISSUE: reference to a compiler-generated field
                Bounds3 bounds = nativeArray4.Length == 0 || !this.m_PrefabStackData.TryGetComponent(prefabRef.m_Prefab, out componentData2) ? ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, componentData1) : ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, nativeArray4[index], componentData1, componentData2);
                BoundsMask mask = BoundsMask.Debug;
                if (!flag1)
                {
                  if (flag3)
                    mask |= BoundsMask.IsTree;
                  if ((componentData1.m_Flags & GeometryFlags.OccupyZone) != GeometryFlags.None)
                    mask |= BoundsMask.OccupyZone;
                  if ((componentData1.m_Flags & GeometryFlags.WalkThrough) == GeometryFlags.None)
                    mask |= BoundsMask.NotWalkThrough;
                }
                if (!flag2)
                {
                  mask |= BoundsMask.NotOverridden;
                  // ISSUE: reference to a compiler-generated field
                  if (!flag1 || this.m_EditorMode)
                  {
                    MeshLayer layers = componentData1.m_Layers;
                    Owner owner;
                    CollectionUtils.TryGet<Owner>(nativeArray5, index, out owner);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    mask |= CommonUtils.GetBoundsMask(Game.Net.SearchSystem.GetLayers(owner, new Game.Net.UtilityLane(), layers, ref this.m_PrefabRefData, ref this.m_PrefabNetData, ref this.m_PrefabNetGeometryData));
                  }
                }
                if (!flag4)
                  mask &= ~(BoundsMask.AllLayers | BoundsMask.Debug);
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Add(entity, new QuadTreeBoundsXZ(bounds, mask, componentData1.m_MinLod));
              }
              else
              {
                Bounds3 bounds = new Bounds3(transform.m_Position - 1f, transform.m_Position + 1f);
                int lodLimit = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float3(2f)));
                // ISSUE: reference to a compiler-generated field
                BoundsMask mask = this.m_EditorMode ? BoundsMask.Debug | BoundsMask.NormalLayers : BoundsMask.Debug;
                if (!flag4)
                  mask &= ~(BoundsMask.AllLayers | BoundsMask.Debug);
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Add(entity, new QuadTreeBoundsXZ(bounds, mask, lodLimit));
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray6 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Transform> nativeArray7 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Stack> nativeArray8 = chunk.GetNativeArray<Stack>(ref this.m_StackType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Owner> nativeArray9 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            bool flag5 = chunk.Has<Marker>(ref this.m_MarkerType) && !chunk.Has<OutsideConnection>(ref this.m_OutsideConnectionType);
            // ISSUE: reference to a compiler-generated field
            bool flag6 = chunk.Has<Overridden>(ref this.m_OverriddenType);
            // ISSUE: reference to a compiler-generated field
            bool flag7 = chunk.Has<Tree>(ref this.m_TreeType);
            // ISSUE: reference to a compiler-generated field
            bool flag8 = chunk.Has<CullingInfo>(ref this.m_CullingInfoType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              PrefabRef prefabRef = nativeArray6[index];
              Transform transform = nativeArray7[index];
              ObjectGeometryData componentData3;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData3))
              {
                StackData componentData4;
                // ISSUE: reference to a compiler-generated field
                Bounds3 bounds = nativeArray8.Length == 0 || !this.m_PrefabStackData.TryGetComponent(prefabRef.m_Prefab, out componentData4) ? ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, componentData3) : ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, nativeArray8[index], componentData3, componentData4);
                BoundsMask mask = BoundsMask.Debug;
                if (!flag5)
                {
                  if (flag7)
                    mask |= BoundsMask.IsTree;
                  if ((componentData3.m_Flags & GeometryFlags.OccupyZone) != GeometryFlags.None)
                    mask |= BoundsMask.OccupyZone;
                  if ((componentData3.m_Flags & GeometryFlags.WalkThrough) == GeometryFlags.None)
                    mask |= BoundsMask.NotWalkThrough;
                }
                if (!flag6)
                {
                  mask |= BoundsMask.NotOverridden;
                  // ISSUE: reference to a compiler-generated field
                  if (!flag5 || this.m_EditorMode)
                  {
                    MeshLayer layers = componentData3.m_Layers;
                    Owner owner;
                    CollectionUtils.TryGet<Owner>(nativeArray9, index, out owner);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    mask |= CommonUtils.GetBoundsMask(Game.Net.SearchSystem.GetLayers(owner, new Game.Net.UtilityLane(), layers, ref this.m_PrefabRefData, ref this.m_PrefabNetData, ref this.m_PrefabNetGeometryData));
                  }
                }
                if (!flag8)
                  mask &= ~(BoundsMask.AllLayers | BoundsMask.Debug);
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Update(entity, new QuadTreeBoundsXZ(bounds, mask, componentData3.m_MinLod));
              }
              else
              {
                Bounds3 bounds = new Bounds3(transform.m_Position - 1f, transform.m_Position + 1f);
                int lodLimit = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float3(2f)));
                // ISSUE: reference to a compiler-generated field
                BoundsMask mask = this.m_EditorMode ? BoundsMask.Debug | BoundsMask.NormalLayers : BoundsMask.Debug;
                if (!flag8)
                  mask &= ~(BoundsMask.AllLayers | BoundsMask.Debug);
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Update(entity, new QuadTreeBoundsXZ(bounds, mask, lodLimit));
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
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Stack> __Game_Objects_Stack_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Marker> __Game_Objects_Marker_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Tree> __Game_Objects_Tree_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Overridden> __Game_Common_Overridden_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CullingInfo> __Game_Rendering_CullingInfo_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StackData> __Game_Prefabs_StackData_RO_ComponentLookup;
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
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stack_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Stack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Marker_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Marker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Overridden_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Overridden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CullingInfo>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StackData_RO_ComponentLookup = state.GetComponentLookup<StackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
      }
    }
  }
}
