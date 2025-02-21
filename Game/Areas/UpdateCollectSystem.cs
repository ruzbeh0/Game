// Decompiled with JetBrains decompiler
// Type: Game.Areas.UpdateCollectSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
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
  public class UpdateCollectSystem : GameSystemBase
  {
    private SearchSystem m_SearchSystem;
    private UpdateCollectSystem.UpdateBufferData m_LotData;
    private UpdateCollectSystem.UpdateBufferData m_DistrictData;
    private UpdateCollectSystem.UpdateBufferData m_MapTileData;
    private UpdateCollectSystem.UpdateBufferData m_SpaceData;
    private UpdateCollectSystem.TypeHandle __TypeHandle;

    public bool lotsUpdated => this.m_LotData.m_IsUpdated;

    public bool districtsUpdated => this.m_DistrictData.m_IsUpdated;

    public bool mapTilesUpdated => this.m_MapTileData.m_IsUpdated;

    public bool spacesUpdated => this.m_SpaceData.m_IsUpdated;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.m_LotData.Create(this.GetQuery<Lot>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.m_DistrictData.Create(this.GetQuery<District>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.m_MapTileData.Create(this.GetQuery<MapTile>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.m_SpaceData.Create(this.GetQuery<Space>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LotData.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DistrictData.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_MapTileData.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SpaceData.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStopRunning()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LotData.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DistrictData.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_MapTileData.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SpaceData.Clear();
      base.OnStopRunning();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle dependency = this.Dependency;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_LotData.m_Query.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_LotData.Clear();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Dependency = JobHandle.CombineDependencies(this.Dependency, this.UpdateBounds(ref this.m_LotData, dependency));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_DistrictData.m_Query.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_DistrictData.Clear();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Dependency = JobHandle.CombineDependencies(this.Dependency, this.UpdateBounds(ref this.m_DistrictData, dependency));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_MapTileData.m_Query.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_MapTileData.Clear();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Dependency = JobHandle.CombineDependencies(this.Dependency, this.UpdateBounds(ref this.m_MapTileData, dependency));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_SpaceData.m_Query.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_SpaceData.Clear();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Dependency = JobHandle.CombineDependencies(this.Dependency, this.UpdateBounds(ref this.m_SpaceData, dependency));
      }
    }

    private JobHandle UpdateBounds(
      ref UpdateCollectSystem.UpdateBufferData data,
      JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      data.m_IsUpdated = true;
      NativeQueue<Bounds2> nativeQueue = new NativeQueue<Bounds2>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle dependencies;
      NativeParallelHashMap<Entity, int> triangleCount;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> searchTree = this.m_SearchSystem.GetSearchTree(true, out dependencies, out triangleCount);
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
      // ISSUE: reference to a compiler-generated field
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
      UpdateCollectSystem.CollectUpdatedAreaBoundsJob jobData1 = new UpdateCollectSystem.CollectUpdatedAreaBoundsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle,
        m_TriangleType = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_SearchTree = searchTree,
        m_TriangleCount = triangleCount,
        m_ResultQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UpdateCollectSystem.DequeueBoundsJob jobData2 = new UpdateCollectSystem.DequeueBoundsJob()
      {
        m_Queue = nativeQueue,
        m_ResultList = data.m_Bounds
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.ScheduleParallel<UpdateCollectSystem.CollectUpdatedAreaBoundsJob>(data.m_Query, JobHandle.CombineDependencies(inputDeps, dependencies));
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle, data.m_ReadDependencies);
      JobHandle inputDeps1 = jobData2.Schedule<UpdateCollectSystem.DequeueBoundsJob>(dependsOn);
      nativeQueue.Dispose(inputDeps1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SearchSystem.AddSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      data.m_WriteDependencies = inputDeps1;
      // ISSUE: reference to a compiler-generated field
      data.m_ReadDependencies = new JobHandle();
      return inputDeps1;
    }

    public NativeList<Bounds2> GetUpdatedLotBounds(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_LotData.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_LotData.m_Bounds;
    }

    public NativeList<Bounds2> GetUpdatedDistrictBounds(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_DistrictData.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_DistrictData.m_Bounds;
    }

    public NativeList<Bounds2> GetUpdatedMapTileBounds(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_MapTileData.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_MapTileData.m_Bounds;
    }

    public NativeList<Bounds2> GetUpdatedSpaceBounds(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_SpaceData.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_SpaceData.m_Bounds;
    }

    public void AddLotBoundsReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LotData.m_ReadDependencies = JobHandle.CombineDependencies(this.m_LotData.m_ReadDependencies, handle);
    }

    public void AddDistrictBoundsReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_DistrictData.m_ReadDependencies = JobHandle.CombineDependencies(this.m_DistrictData.m_ReadDependencies, handle);
    }

    public void AddMapTileBoundsReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileData.m_ReadDependencies = JobHandle.CombineDependencies(this.m_MapTileData.m_ReadDependencies, handle);
    }

    public void AddSpaceBoundsReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SpaceData.m_ReadDependencies = JobHandle.CombineDependencies(this.m_SpaceData.m_ReadDependencies, handle);
    }

    private EntityQuery GetQuery<T>()
    {
      return this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<Area>(),
          ComponentType.ReadOnly<Node>(),
          ComponentType.ReadOnly<Triangle>(),
          ComponentType.ReadOnly<T>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
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
    public UpdateCollectSystem()
    {
    }

    private struct UpdateBufferData
    {
      public NativeList<Bounds2> m_Bounds;
      public EntityQuery m_Query;
      public JobHandle m_WriteDependencies;
      public JobHandle m_ReadDependencies;
      public bool m_IsUpdated;

      public void Create(EntityQuery query)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Bounds = new NativeList<Bounds2>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
        // ISSUE: reference to a compiler-generated field
        this.m_Query = query;
      }

      public void Dispose() => this.m_Bounds.Dispose();

      public void Clear()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_WriteDependencies.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_ReadDependencies.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_Bounds.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_IsUpdated = false;
      }
    }

    [BurstCompile]
    private struct CollectUpdatedAreaBoundsJob : IJobChunk
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
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_SearchTree;
      [ReadOnly]
      public NativeParallelHashMap<Entity, int> m_TriangleCount;
      public NativeQueue<Bounds2>.ParallelWriter m_ResultQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Created>(ref this.m_CreatedType))
        {
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Node> bufferAccessor1 = chunk.GetBufferAccessor<Node>(ref this.m_NodeType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Triangle> bufferAccessor2 = chunk.GetBufferAccessor<Triangle>(ref this.m_TriangleType);
          for (int index1 = 0; index1 < bufferAccessor1.Length; ++index1)
          {
            DynamicBuffer<Node> nodes = bufferAccessor1[index1];
            DynamicBuffer<Triangle> dynamicBuffer = bufferAccessor2[index1];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ResultQueue.Enqueue(MathUtils.Bounds(AreaUtils.GetTriangle3(nodes, dynamicBuffer[index2])).xz);
            }
          }
        }
        else
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
                  QuadTreeBoundsXZ bounds;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_SearchTree.TryGet(new AreaSearchItem(entity, triangle), out bounds))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_ResultQueue.Enqueue(bounds.m_Bounds.xz);
                  }
                }
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Node> bufferAccessor3 = chunk.GetBufferAccessor<Node>(ref this.m_NodeType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Triangle> bufferAccessor4 = chunk.GetBufferAccessor<Triangle>(ref this.m_TriangleType);
            for (int index3 = 0; index3 < nativeArray.Length; ++index3)
            {
              Entity entity = nativeArray[index3];
              DynamicBuffer<Node> nodes = bufferAccessor3[index3];
              DynamicBuffer<Triangle> dynamicBuffer = bufferAccessor4[index3];
              int x;
              // ISSUE: reference to a compiler-generated field
              if (this.m_TriangleCount.TryGetValue(entity, out x))
              {
                int num = math.min(x, dynamicBuffer.Length);
                for (int index4 = 0; index4 < num; ++index4)
                {
                  Bounds2 xz1 = MathUtils.Bounds(AreaUtils.GetTriangle3(nodes, dynamicBuffer[index4])).xz;
                  QuadTreeBoundsXZ bounds1;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_SearchTree.TryGet(new AreaSearchItem(entity, index4), out bounds1))
                  {
                    Bounds2 xz2 = bounds1.m_Bounds.xz;
                    Bounds2 bounds2 = xz2 | xz1;
                    if ((double) math.length(MathUtils.Size(bounds2)) < (double) math.length(MathUtils.Size(xz2)) + (double) math.length(MathUtils.Size(xz1)))
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_ResultQueue.Enqueue(bounds2);
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_ResultQueue.Enqueue(xz2);
                      // ISSUE: reference to a compiler-generated field
                      this.m_ResultQueue.Enqueue(xz1);
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_ResultQueue.Enqueue(xz1);
                  }
                }
                for (int triangle = num; triangle < x; ++triangle)
                {
                  QuadTreeBoundsXZ bounds;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_SearchTree.TryGet(new AreaSearchItem(entity, triangle), out bounds))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_ResultQueue.Enqueue(bounds.m_Bounds.xz);
                  }
                }
                for (int index5 = num; index5 < dynamicBuffer.Length; ++index5)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ResultQueue.Enqueue(MathUtils.Bounds(AreaUtils.GetTriangle3(nodes, dynamicBuffer[index5])).xz);
                }
              }
              else
              {
                for (int index6 = 0; index6 < dynamicBuffer.Length; ++index6)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ResultQueue.Enqueue(MathUtils.Bounds(AreaUtils.GetTriangle3(nodes, dynamicBuffer[index6])).xz);
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
    private struct DequeueBoundsJob : IJob
    {
      public NativeQueue<Bounds2> m_Queue;
      public NativeList<Bounds2> m_ResultList;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_Queue.Count;
        // ISSUE: reference to a compiler-generated field
        this.m_ResultList.ResizeUninitialized(count);
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ResultList[index] = this.m_Queue.Dequeue();
        }
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
      }
    }
  }
}
