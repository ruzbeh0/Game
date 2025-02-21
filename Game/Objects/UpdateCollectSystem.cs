// Decompiled with JetBrains decompiler
// Type: Game.Objects.UpdateCollectSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Prefabs;
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
  public class UpdateCollectSystem : GameSystemBase
  {
    private EntityQuery m_ObjectQuery;
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
      this.m_ObjectQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<Object>(),
          ComponentType.ReadOnly<Static>(),
          ComponentType.ReadOnly<Transform>(),
          ComponentType.ReadOnly<PrefabRef>()
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
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ObjectQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedBounds.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStopRunning()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedBounds.Clear();
      this.isUpdated = false;
      base.OnStopRunning();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      this.isUpdated = true;
      NativeQueue<Bounds2> nativeQueue = new NativeQueue<Bounds2>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UpdateCollectSystem.CollectUpdatedObjectBoundsJob jobData1 = new UpdateCollectSystem.CollectUpdatedObjectBoundsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_SearchTree = this.m_SearchSystem.GetStaticSearchTree(true, out dependencies),
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
      JobHandle jobHandle = jobData1.ScheduleParallel<UpdateCollectSystem.CollectUpdatedObjectBoundsJob>(this.m_ObjectQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle, this.m_ReadDependencies);
      JobHandle inputDeps = jobData2.Schedule<UpdateCollectSystem.DequeueBoundsJob>(dependsOn);
      nativeQueue.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SearchSystem.AddStaticSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = inputDeps;
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies = new JobHandle();
      this.Dependency = jobHandle;
    }

    public NativeList<Bounds2> GetUpdatedBounds(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_UpdatedBounds;
    }

    public void AddBoundsReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies = JobHandle.CombineDependencies(this.m_ReadDependencies, handle);
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
    private struct CollectUpdatedObjectBoundsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;
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
          NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Transform> nativeArray2 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            PrefabRef prefabRef = nativeArray1[index];
            Transform transform = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabObjectGeometryData.HasComponent(prefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              ObjectGeometryData geometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
              // ISSUE: reference to a compiler-generated field
              this.m_ResultQueue.Enqueue(ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, geometryData).xz);
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
              QuadTreeBoundsXZ bounds;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SearchTree.TryGet(nativeArray[index], out bounds))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_ResultQueue.Enqueue(bounds.m_Bounds.xz);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray3 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Transform> nativeArray5 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
            for (int index = 0; index < nativeArray3.Length; ++index)
            {
              Entity entity = nativeArray3[index];
              PrefabRef prefabRef = nativeArray4[index];
              Transform transform = nativeArray5[index];
              Bounds2 bounds1 = new Bounds2();
              Bounds2 bounds2 = new Bounds2();
              bool2 x = new bool2();
              QuadTreeBoundsXZ bounds3;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SearchTree.TryGet(entity, out bounds3))
              {
                bounds1 = bounds3.m_Bounds.xz;
                x.x = true;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabObjectGeometryData.HasComponent(prefabRef.m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                ObjectGeometryData geometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
                bounds2 = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, geometryData).xz;
                x.y = true;
              }
              if (math.all(x))
              {
                Bounds2 bounds4 = bounds1 | bounds2;
                if ((double) math.length(MathUtils.Size(bounds4)) < (double) math.length(MathUtils.Size(bounds1)) + (double) math.length(MathUtils.Size(bounds2)))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ResultQueue.Enqueue(bounds4);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ResultQueue.Enqueue(bounds1);
                  // ISSUE: reference to a compiler-generated field
                  this.m_ResultQueue.Enqueue(bounds2);
                }
              }
              else if (x.x)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_ResultQueue.Enqueue(bounds1);
              }
              else if (x.y)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_ResultQueue.Enqueue(bounds2);
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
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
      }
    }
  }
}
