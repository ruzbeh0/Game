// Decompiled with JetBrains decompiler
// Type: Game.Routes.SearchSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Pathfind;
using Game.Prefabs;
using Game.Serialization;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Routes
{
  [CompilerGenerated]
  public class SearchSystem : GameSystemBase, IPreDeserialize
  {
    private EntityQuery m_UpdatedRoutesQuery;
    private EntityQuery m_AllRoutesQuery;
    private NativeQuadTree<RouteSearchItem, QuadTreeBoundsXZ> m_SearchTree;
    private NativeParallelHashMap<Entity, int> m_ElementCount;
    private JobHandle m_ReadDependencies;
    private JobHandle m_WriteDependencies;
    private bool m_Loaded;
    private SearchSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedRoutesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Waypoint>(),
          ComponentType.ReadOnly<Position>()
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
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Segment>(),
          ComponentType.ReadOnly<PathElement>()
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
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Common.Event>(),
          ComponentType.ReadOnly<PathUpdated>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AllRoutesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Waypoint>(),
          ComponentType.ReadOnly<Position>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Segment>(),
          ComponentType.ReadOnly<PathElement>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_SearchTree = new NativeQuadTree<RouteSearchItem, QuadTreeBoundsXZ>(1f, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ElementCount = new NativeParallelHashMap<Entity, int>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SearchTree.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ElementCount.Dispose();
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
      EntityQuery query = loaded ? this.m_AllRoutesQuery : this.m_UpdatedRoutesQuery;
      if (query.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_CurveElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Segment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_CurveElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Position_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SearchSystem.UpdateSearchTreeJob jobData = new SearchSystem.UpdateSearchTreeJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PositionType = this.__TypeHandle.__Game_Routes_Position_RO_ComponentTypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PathUpdatedType = this.__TypeHandle.__Game_Pathfind_PathUpdated_RO_ComponentTypeHandle,
        m_CurveElementType = this.__TypeHandle.__Game_Routes_CurveElement_RO_BufferTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabRouteData = this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_SegmentData = this.__TypeHandle.__Game_Routes_Segment_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_CurveElements = this.__TypeHandle.__Game_Routes_CurveElement_RO_BufferLookup,
        m_Loaded = loaded,
        m_SearchTree = this.GetSearchTree(false, out dependencies),
        m_ElementCount = this.m_ElementCount
      };
      this.Dependency = jobData.Schedule<SearchSystem.UpdateSearchTreeJob>(query, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated method
      this.AddSearchTreeWriter(this.Dependency);
    }

    public NativeQuadTree<RouteSearchItem, QuadTreeBoundsXZ> GetSearchTree(
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

    public void AddSearchTreeReader(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies = JobHandle.CombineDependencies(this.m_ReadDependencies, jobHandle);
    }

    public void AddSearchTreeWriter(JobHandle jobHandle) => this.m_WriteDependencies = jobHandle;

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated method
      NativeQuadTree<RouteSearchItem, QuadTreeBoundsXZ> searchTree = this.GetSearchTree(false, out dependencies);
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
      public ComponentTypeHandle<Position> m_PositionType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<PathUpdated> m_PathUpdatedType;
      [ReadOnly]
      public BufferTypeHandle<CurveElement> m_CurveElementType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<RouteData> m_PrefabRouteData;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Segment> m_SegmentData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public BufferLookup<CurveElement> m_CurveElements;
      [ReadOnly]
      public bool m_Loaded;
      public NativeQuadTree<RouteSearchItem, QuadTreeBoundsXZ> m_SearchTree;
      public NativeParallelHashMap<Entity, int> m_ElementCount;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<PathUpdated>(ref this.m_PathUpdatedType))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<PathUpdated> nativeArray = chunk.GetNativeArray<PathUpdated>(ref this.m_PathUpdatedType);
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            Entity owner = nativeArray[index].m_Owner;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_SegmentData.HasComponent(owner) && !this.m_TempData.HasComponent(owner) && !this.m_UpdatedData.HasComponent(owner) && !this.m_DeletedData.HasComponent(owner))
            {
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef = this.m_PrefabRefData[owner];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<CurveElement> curveElement = this.m_CurveElements[owner];
              // ISSUE: reference to a compiler-generated method
              this.UpdateSegment(owner, prefabRef, curveElement);
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
            NativeArray<Position> nativeArray3 = chunk.GetNativeArray<Position>(ref this.m_PositionType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<CurveElement> bufferAccessor = chunk.GetBufferAccessor<CurveElement>(ref this.m_CurveElementType);
            for (int index = 0; index < nativeArray3.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              PrefabRef prefabRef = nativeArray2[index];
              // ISSUE: reference to a compiler-generated field
              Bounds3 bounds = RouteUtils.CalculateBounds(nativeArray3[index], this.m_PrefabRouteData[prefabRef.m_Prefab]);
              // ISSUE: reference to a compiler-generated field
              this.m_SearchTree.Add(new RouteSearchItem(entity, 0), new QuadTreeBoundsXZ(bounds));
            }
            for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
            {
              Entity entity = nativeArray1[index1];
              PrefabRef prefabRef = nativeArray2[index1];
              DynamicBuffer<CurveElement> dynamicBuffer = bufferAccessor[index1];
              // ISSUE: reference to a compiler-generated field
              RouteData routeData = this.m_PrefabRouteData[prefabRef.m_Prefab];
              int num = 0;
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                Bounds3 bounds = RouteUtils.CalculateBounds(dynamicBuffer[index2], routeData);
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Add(new RouteSearchItem(entity, num++), new QuadTreeBoundsXZ(bounds));
              }
              // ISSUE: reference to a compiler-generated field
              this.m_ElementCount.TryAdd(entity, num);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<Deleted>(ref this.m_DeletedType))
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<Entity> nativeArray4 = chunk.GetNativeArray(this.m_EntityType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<Position> nativeArray5 = chunk.GetNativeArray<Position>(ref this.m_PositionType);
              // ISSUE: reference to a compiler-generated field
              BufferAccessor<CurveElement> bufferAccessor = chunk.GetBufferAccessor<CurveElement>(ref this.m_CurveElementType);
              for (int index = 0; index < nativeArray5.Length; ++index)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Remove(new RouteSearchItem(nativeArray4[index], 0));
              }
              for (int index = 0; index < bufferAccessor.Length; ++index)
              {
                Entity entity = nativeArray4[index];
                int num;
                // ISSUE: reference to a compiler-generated field
                if (this.m_ElementCount.TryGetValue(entity, out num))
                {
                  for (int element = 0; element < num; ++element)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_SearchTree.Remove(new RouteSearchItem(entity, element));
                  }
                  // ISSUE: reference to a compiler-generated field
                  this.m_ElementCount.Remove(entity);
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<Entity> nativeArray6 = chunk.GetNativeArray(this.m_EntityType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<PrefabRef> nativeArray7 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<Position> nativeArray8 = chunk.GetNativeArray<Position>(ref this.m_PositionType);
              // ISSUE: reference to a compiler-generated field
              BufferAccessor<CurveElement> bufferAccessor = chunk.GetBufferAccessor<CurveElement>(ref this.m_CurveElementType);
              for (int index = 0; index < nativeArray8.Length; ++index)
              {
                Entity entity = nativeArray6[index];
                PrefabRef prefabRef = nativeArray7[index];
                // ISSUE: reference to a compiler-generated field
                Bounds3 bounds = RouteUtils.CalculateBounds(nativeArray8[index], this.m_PrefabRouteData[prefabRef.m_Prefab]);
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Update(new RouteSearchItem(entity, 0), new QuadTreeBoundsXZ(bounds));
              }
              for (int index = 0; index < bufferAccessor.Length; ++index)
              {
                // ISSUE: reference to a compiler-generated method
                this.UpdateSegment(nativeArray6[index], nativeArray7[index], bufferAccessor[index]);
              }
            }
          }
        }
      }

      private void UpdateSegment(
        Entity entity,
        PrefabRef prefabRef,
        DynamicBuffer<CurveElement> curveElements)
      {
        // ISSUE: reference to a compiler-generated field
        RouteData routeData = this.m_PrefabRouteData[prefabRef.m_Prefab];
        int num1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ElementCount.TryGetValue(entity, out num1))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ElementCount.Remove(entity);
        }
        else
          num1 = 0;
        int num2 = 0;
        for (int index = 0; index < curveElements.Length; ++index)
        {
          Bounds3 bounds = RouteUtils.CalculateBounds(curveElements[index], routeData);
          if (num2 < num1)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SearchTree.Update(new RouteSearchItem(entity, num2++), new QuadTreeBoundsXZ(bounds));
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SearchTree.Add(new RouteSearchItem(entity, num2++), new QuadTreeBoundsXZ(bounds));
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ElementCount.TryAdd(entity, num2);
        while (num2 < num1)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SearchTree.Remove(new RouteSearchItem(entity, num2++));
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
      public ComponentTypeHandle<Position> __Game_Routes_Position_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathUpdated> __Game_Pathfind_PathUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<CurveElement> __Game_Routes_CurveElement_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteData> __Game_Prefabs_RouteData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Segment> __Game_Routes_Segment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CurveElement> __Game_Routes_CurveElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_CurveElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<CurveElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteData_RO_ComponentLookup = state.GetComponentLookup<RouteData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Segment_RO_ComponentLookup = state.GetComponentLookup<Segment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_CurveElement_RO_BufferLookup = state.GetBufferLookup<CurveElement>(true);
      }
    }
  }
}
