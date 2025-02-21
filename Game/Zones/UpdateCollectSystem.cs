// Decompiled with JetBrains decompiler
// Type: Game.Zones.UpdateCollectSystem
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
namespace Game.Zones
{
  [CompilerGenerated]
  public class UpdateCollectSystem : GameSystemBase
  {
    private EntityQuery m_BlockQuery;
    private SearchSystem m_SearchSystem;
    private NativeList<Bounds2> m_UpdatedBounds;
    private JobHandle m_WriteDependencies;
    private JobHandle m_ReadDependencies;
    private UpdateCollectSystem.TypeHandle __TypeHandle;

    public bool isUpdated { get; private set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BlockQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Block>()
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
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedBounds = new NativeList<Bounds2>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedBounds.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_BlockQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_WriteDependencies.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_ReadDependencies.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_UpdatedBounds.Clear();
        this.isUpdated = false;
      }
      else
      {
        this.isUpdated = true;
        NativeQueue<Bounds2> nativeQueue = new NativeQueue<Bounds2>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Zones_Block_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated method
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        UpdateCollectSystem.CollectUpdatedBlockBoundsJob jobData1 = new UpdateCollectSystem.CollectUpdatedBlockBoundsJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_BlockType = this.__TypeHandle.__Game_Zones_Block_RO_ComponentTypeHandle,
          m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
          m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
          m_SearchTree = this.m_SearchSystem.GetSearchTree(true, out dependencies),
          m_ResultQueue = nativeQueue.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        UpdateCollectSystem.DequeueBoundsJob jobData2 = new UpdateCollectSystem.DequeueBoundsJob()
        {
          m_Queue = nativeQueue,
          m_ResultList = this.m_UpdatedBounds
        };
        // ISSUE: reference to a compiler-generated field
        JobHandle jobHandle = jobData1.ScheduleParallel<UpdateCollectSystem.CollectUpdatedBlockBoundsJob>(this.m_BlockQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle, this.m_WriteDependencies, this.m_ReadDependencies);
        JobHandle inputDeps = jobData2.Schedule<UpdateCollectSystem.DequeueBoundsJob>(dependsOn);
        nativeQueue.Dispose(inputDeps);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_SearchSystem.AddSearchTreeReader(jobHandle);
        // ISSUE: reference to a compiler-generated field
        this.m_WriteDependencies = inputDeps;
        // ISSUE: reference to a compiler-generated field
        this.m_ReadDependencies = new JobHandle();
        this.Dependency = jobHandle;
      }
    }

    public NativeList<Bounds2> GetUpdatedBounds(bool readOnly, out JobHandle dependencies)
    {
      if (readOnly)
      {
        // ISSUE: reference to a compiler-generated field
        dependencies = this.m_WriteDependencies;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        dependencies = JobHandle.CombineDependencies(this.m_WriteDependencies, this.m_ReadDependencies);
        this.isUpdated = true;
      }
      // ISSUE: reference to a compiler-generated field
      return this.m_UpdatedBounds;
    }

    public void AddBoundsReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies = JobHandle.CombineDependencies(this.m_ReadDependencies, handle);
    }

    public void AddBoundsWriter(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = handle;
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies = new JobHandle();
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

    [BurstCompile]
    private struct CollectUpdatedBlockBoundsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Block> m_BlockType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public NativeQuadTree<Entity, Bounds2> m_SearchTree;
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
          NativeArray<Block> nativeArray = chunk.GetNativeArray<Block>(ref this.m_BlockType);
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ResultQueue.Enqueue(ZoneUtils.CalculateBounds(nativeArray[index]));
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
              Bounds2 bounds;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SearchTree.TryGet(nativeArray[index], out bounds))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_ResultQueue.Enqueue(bounds);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Block> nativeArray2 = chunk.GetNativeArray<Block>(ref this.m_BlockType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              Bounds2 bounds1 = ZoneUtils.CalculateBounds(nativeArray2[index]);
              Bounds2 bounds2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SearchTree.TryGet(entity, out bounds2))
              {
                Bounds2 bounds3 = bounds2 | bounds1;
                if ((double) math.length(MathUtils.Size(bounds3)) < (double) math.length(MathUtils.Size(bounds2)) + (double) math.length(MathUtils.Size(bounds1)))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ResultQueue.Enqueue(bounds3);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ResultQueue.Enqueue(bounds2);
                  // ISSUE: reference to a compiler-generated field
                  this.m_ResultQueue.Enqueue(bounds1);
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_ResultQueue.Enqueue(bounds1);
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
      public ComponentTypeHandle<Block> __Game_Zones_Block_RO_ComponentTypeHandle;
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
        this.__Game_Zones_Block_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Block>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
      }
    }
  }
}
