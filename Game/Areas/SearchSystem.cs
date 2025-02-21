// Decompiled with JetBrains decompiler
// Type: Game.Areas.SearchSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Prefabs;
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
namespace Game.Areas
{
  [CompilerGenerated]
  public class SearchSystem : GameSystemBase, IPreDeserialize
  {
    private EntityQuery m_UpdatedAreasQuery;
    private EntityQuery m_AllAreasQuery;
    private NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_SearchTree;
    private NativeParallelHashMap<Entity, int> m_TriangleCount;
    private JobHandle m_ReadDependencies;
    private JobHandle m_WriteDependencies;
    private bool m_Loaded;
    private SearchSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedAreasQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Area>(),
          ComponentType.ReadOnly<Node>(),
          ComponentType.ReadOnly<Triangle>()
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
      this.m_AllAreasQuery = this.GetEntityQuery(ComponentType.ReadOnly<Area>(), ComponentType.ReadOnly<Node>(), ComponentType.ReadOnly<Triangle>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_SearchTree = new NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ>(1f, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_TriangleCount = new NativeParallelHashMap<Entity, int>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SearchTree.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_TriangleCount.Dispose();
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
      EntityQuery query = loaded ? this.m_AllAreasQuery : this.m_UpdatedAreasQuery;
      if (query.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Batch_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      NativeParallelHashMap<Entity, int> triangleCount;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
        m_NodeType = this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle,
        m_TriangleType = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_BatchType = this.__TypeHandle.__Game_Areas_Batch_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PrefabAreaGeometryData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
        m_Loaded = loaded,
        m_SearchTree = this.GetSearchTree(false, out dependencies, out triangleCount),
        m_TriangleCount = triangleCount
      };
      this.Dependency = jobData.Schedule<SearchSystem.UpdateSearchTreeJob>(query, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated method
      this.AddSearchTreeWriter(this.Dependency);
    }

    public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> GetSearchTree(
      bool readOnly,
      out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = readOnly ? this.m_WriteDependencies : JobHandle.CombineDependencies(this.m_ReadDependencies, this.m_WriteDependencies);
      // ISSUE: reference to a compiler-generated field
      return this.m_SearchTree;
    }

    public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> GetSearchTree(
      bool readOnly,
      out JobHandle dependencies,
      out NativeParallelHashMap<Entity, int> triangleCount)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = readOnly ? this.m_WriteDependencies : JobHandle.CombineDependencies(this.m_ReadDependencies, this.m_WriteDependencies);
      // ISSUE: reference to a compiler-generated field
      triangleCount = this.m_TriangleCount;
      // ISSUE: reference to a compiler-generated field
      return this.m_SearchTree;
    }

    public void AddSearchTreeReader(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies = JobHandle.CombineDependencies(this.m_ReadDependencies, jobHandle);
    }

    public void AddSearchTreeWriter(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = JobHandle.CombineDependencies(this.m_WriteDependencies, jobHandle);
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated method
      NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> searchTree = this.GetSearchTree(false, out dependencies);
      dependencies.Complete();
      searchTree.Clear();
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
    public struct UpdateSearchTreeJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<Node> m_NodeType;
      [ReadOnly]
      public BufferTypeHandle<Triangle> m_TriangleType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<Batch> m_BatchType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_PrefabAreaGeometryData;
      [ReadOnly]
      public bool m_Loaded;
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_SearchTree;
      public NativeParallelHashMap<Entity, int> m_TriangleCount;

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
            Entity entity = nativeArray[index];
            int num;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TriangleCount.TryGetValue(entity, out num))
            {
              for (int triangle = 0; triangle < num; ++triangle)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Remove(new AreaSearchItem(entity, triangle));
              }
              // ISSUE: reference to a compiler-generated field
              this.m_TriangleCount.Remove(entity);
            }
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
            NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Node> bufferAccessor1 = chunk.GetBufferAccessor<Node>(ref this.m_NodeType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Triangle> bufferAccessor2 = chunk.GetBufferAccessor<Triangle>(ref this.m_TriangleType);
            BoundsMask mask = BoundsMask.Debug | BoundsMask.NotOverridden | BoundsMask.NotWalkThrough;
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<Batch>(ref this.m_BatchType))
              mask |= BoundsMask.NormalLayers;
            for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
            {
              Entity entity = nativeArray1[index1];
              PrefabRef prefabRef = nativeArray2[index1];
              DynamicBuffer<Node> nodes = bufferAccessor1[index1];
              DynamicBuffer<Triangle> dynamicBuffer = bufferAccessor2[index1];
              // ISSUE: reference to a compiler-generated field
              AreaGeometryData areaData = this.m_PrefabAreaGeometryData[prefabRef.m_Prefab];
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                Triangle triangle = dynamicBuffer[index2];
                Triangle3 triangle3 = AreaUtils.GetTriangle3(nodes, triangle);
                Bounds3 bounds = AreaUtils.GetBounds(triangle, triangle3, areaData);
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Add(new AreaSearchItem(entity, index2), new QuadTreeBoundsXZ(bounds, mask, triangle.m_MinLod));
              }
              // ISSUE: reference to a compiler-generated field
              this.m_TriangleCount.TryAdd(entity, dynamicBuffer.Length);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray3 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Node> bufferAccessor3 = chunk.GetBufferAccessor<Node>(ref this.m_NodeType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Triangle> bufferAccessor4 = chunk.GetBufferAccessor<Triangle>(ref this.m_TriangleType);
            BoundsMask mask = BoundsMask.Debug | BoundsMask.NotOverridden | BoundsMask.NotWalkThrough;
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<Batch>(ref this.m_BatchType))
              mask |= BoundsMask.NormalLayers;
            for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
            {
              Entity entity = nativeArray3[index3];
              PrefabRef prefabRef = nativeArray4[index3];
              DynamicBuffer<Node> nodes = bufferAccessor3[index3];
              DynamicBuffer<Triangle> dynamicBuffer = bufferAccessor4[index3];
              // ISSUE: reference to a compiler-generated field
              AreaGeometryData areaData = this.m_PrefabAreaGeometryData[prefabRef.m_Prefab];
              int x;
              // ISSUE: reference to a compiler-generated field
              if (this.m_TriangleCount.TryGetValue(entity, out x))
              {
                for (int length = dynamicBuffer.Length; length < x; ++length)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_SearchTree.Remove(new AreaSearchItem(entity, length));
                }
                // ISSUE: reference to a compiler-generated field
                this.m_TriangleCount.Remove(entity);
              }
              else
                x = 0;
              int num = math.min(x, dynamicBuffer.Length);
              for (int index4 = 0; index4 < num; ++index4)
              {
                Triangle triangle = dynamicBuffer[index4];
                Triangle3 triangle3 = AreaUtils.GetTriangle3(nodes, triangle);
                Bounds3 bounds = AreaUtils.GetBounds(triangle, triangle3, areaData);
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Update(new AreaSearchItem(entity, index4), new QuadTreeBoundsXZ(bounds, mask, triangle.m_MinLod));
              }
              for (int index5 = x; index5 < dynamicBuffer.Length; ++index5)
              {
                Triangle triangle = dynamicBuffer[index5];
                Triangle3 triangle3 = AreaUtils.GetTriangle3(nodes, triangle);
                Bounds3 bounds = AreaUtils.GetBounds(triangle, triangle3, areaData);
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Add(new AreaSearchItem(entity, index5), new QuadTreeBoundsXZ(bounds, mask, triangle.m_MinLod));
              }
              // ISSUE: reference to a compiler-generated field
              this.m_TriangleCount.TryAdd(entity, dynamicBuffer.Length);
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
      public BufferTypeHandle<Node> __Game_Areas_Node_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Triangle> __Game_Areas_Triangle_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Batch> __Game_Areas_Batch_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferTypeHandle = state.GetBufferTypeHandle<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferTypeHandle = state.GetBufferTypeHandle<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Batch_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Batch>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
      }
    }
  }
}
