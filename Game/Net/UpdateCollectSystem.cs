// Decompiled with JetBrains decompiler
// Type: Game.Net.UpdateCollectSystem
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
namespace Game.Net
{
  [CompilerGenerated]
  public class UpdateCollectSystem : GameSystemBase
  {
    private EntityQuery m_NetGeometryQuery;
    private EntityQuery m_LaneGeometryQuery;
    private SearchSystem m_SearchSystem;
    private NativeList<Bounds2> m_UpdatedNetBounds;
    private JobHandle m_NetWriteDependencies;
    private JobHandle m_NetReadDependencies;
    private JobHandle m_LaneWriteDependencies;
    private JobHandle m_LaneReadDependencies;
    private NativeList<Bounds2> m_UpdatedLaneBounds;
    private UpdateCollectSystem.TypeHandle __TypeHandle;

    public bool netsUpdated { get; private set; }

    public bool lanesUpdated { get; private set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetGeometryQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<EdgeGeometry>()
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
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<NodeGeometry>()
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
      this.m_LaneGeometryQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<LaneGeometry>(),
          ComponentType.ReadOnly<UtilityLane>()
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
      this.m_UpdatedNetBounds = new NativeList<Bounds2>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedLaneBounds = new NativeList<Bounds2>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_NetWriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_NetReadDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_LaneWriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_LaneReadDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedNetBounds.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedLaneBounds.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      bool flag1 = !this.m_NetGeometryQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      bool flag2 = !this.m_LaneGeometryQuery.IsEmptyIgnoreFilter;
      if (!flag1 && this.netsUpdated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_NetWriteDependencies.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_NetReadDependencies.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_UpdatedNetBounds.Clear();
        this.netsUpdated = false;
      }
      if (!flag2 && this.lanesUpdated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LaneWriteDependencies.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_LaneReadDependencies.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_UpdatedLaneBounds.Clear();
        this.lanesUpdated = false;
      }
      if (!flag1 && !flag2)
        return;
      JobHandle job0 = new JobHandle();
      if (flag1)
      {
        this.netsUpdated = true;
        NativeQueue<Bounds2> nativeQueue = new NativeQueue<Bounds2>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated method
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        UpdateCollectSystem.CollectUpdatedNetBoundsJob jobData1 = new UpdateCollectSystem.CollectUpdatedNetBoundsJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_EdgeGeometryType = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle,
          m_StartGeometryType = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle,
          m_EndGeometryType = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle,
          m_NodeGeometryType = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentTypeHandle,
          m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
          m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
          m_SearchTree = this.m_SearchSystem.GetNetSearchTree(true, out dependencies),
          m_ResultQueue = nativeQueue.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        UpdateCollectSystem.DequeueBoundsJob jobData2 = new UpdateCollectSystem.DequeueBoundsJob()
        {
          m_Queue = nativeQueue,
          m_ResultList = this.m_UpdatedNetBounds
        };
        // ISSUE: reference to a compiler-generated field
        JobHandle jobHandle = jobData1.ScheduleParallel<UpdateCollectSystem.CollectUpdatedNetBoundsJob>(this.m_NetGeometryQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
        // ISSUE: reference to a compiler-generated field
        JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle, this.m_NetReadDependencies);
        JobHandle inputDeps = jobData2.Schedule<UpdateCollectSystem.DequeueBoundsJob>(dependsOn);
        nativeQueue.Dispose(inputDeps);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_SearchSystem.AddNetSearchTreeReader(jobHandle);
        // ISSUE: reference to a compiler-generated field
        this.m_NetWriteDependencies = inputDeps;
        // ISSUE: reference to a compiler-generated field
        this.m_NetReadDependencies = new JobHandle();
        job0 = JobHandle.CombineDependencies(job0, jobHandle);
      }
      if (flag2)
      {
        this.lanesUpdated = true;
        NativeQueue<Bounds2> nativeQueue = new NativeQueue<Bounds2>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated method
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        UpdateCollectSystem.CollectUpdatedLaneBoundsJob jobData3 = new UpdateCollectSystem.CollectUpdatedLaneBoundsJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
          m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
          m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_PrefabLaneGeometryData = this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup,
          m_SearchTree = this.m_SearchSystem.GetLaneSearchTree(true, out dependencies),
          m_ResultQueue = nativeQueue.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        UpdateCollectSystem.DequeueBoundsJob jobData4 = new UpdateCollectSystem.DequeueBoundsJob()
        {
          m_Queue = nativeQueue,
          m_ResultList = this.m_UpdatedLaneBounds
        };
        // ISSUE: reference to a compiler-generated field
        JobHandle jobHandle = jobData3.ScheduleParallel<UpdateCollectSystem.CollectUpdatedLaneBoundsJob>(this.m_LaneGeometryQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
        // ISSUE: reference to a compiler-generated field
        JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle, this.m_LaneReadDependencies);
        JobHandle inputDeps = jobData4.Schedule<UpdateCollectSystem.DequeueBoundsJob>(dependsOn);
        nativeQueue.Dispose(inputDeps);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_SearchSystem.AddLaneSearchTreeReader(jobHandle);
        // ISSUE: reference to a compiler-generated field
        this.m_LaneWriteDependencies = inputDeps;
        // ISSUE: reference to a compiler-generated field
        this.m_LaneReadDependencies = new JobHandle();
        job0 = JobHandle.CombineDependencies(job0, jobHandle);
      }
      this.Dependency = job0;
    }

    public NativeList<Bounds2> GetUpdatedNetBounds(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_NetWriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_UpdatedNetBounds;
    }

    public void AddNetBoundsReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_NetReadDependencies = JobHandle.CombineDependencies(this.m_NetReadDependencies, handle);
    }

    public NativeList<Bounds2> GetUpdatedLaneBounds(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_LaneWriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_UpdatedLaneBounds;
    }

    public void AddLaneBoundsReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LaneReadDependencies = JobHandle.CombineDependencies(this.m_LaneReadDependencies, handle);
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
    private struct CollectUpdatedNetBoundsJob : IJobChunk
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
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
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
          NativeArray<EdgeGeometry> nativeArray1 = chunk.GetNativeArray<EdgeGeometry>(ref this.m_EdgeGeometryType);
          if (nativeArray1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<StartNodeGeometry> nativeArray2 = chunk.GetNativeArray<StartNodeGeometry>(ref this.m_StartGeometryType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<EndNodeGeometry> nativeArray3 = chunk.GetNativeArray<EndNodeGeometry>(ref this.m_EndGeometryType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ResultQueue.Enqueue((nativeArray1[index].m_Bounds | nativeArray2[index].m_Geometry.m_Bounds | nativeArray3[index].m_Geometry.m_Bounds).xz);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<NodeGeometry> nativeArray4 = chunk.GetNativeArray<NodeGeometry>(ref this.m_NodeGeometryType);
            for (int index = 0; index < nativeArray4.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ResultQueue.Enqueue(nativeArray4[index].m_Bounds.xz);
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
            NativeArray<Entity> nativeArray5 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<EdgeGeometry> nativeArray6 = chunk.GetNativeArray<EdgeGeometry>(ref this.m_EdgeGeometryType);
            if (nativeArray6.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<StartNodeGeometry> nativeArray7 = chunk.GetNativeArray<StartNodeGeometry>(ref this.m_StartGeometryType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<EndNodeGeometry> nativeArray8 = chunk.GetNativeArray<EndNodeGeometry>(ref this.m_EndGeometryType);
              for (int index = 0; index < nativeArray5.Length; ++index)
              {
                Entity entity = nativeArray5[index];
                Bounds2 xz1 = (nativeArray6[index].m_Bounds | nativeArray7[index].m_Geometry.m_Bounds | nativeArray8[index].m_Geometry.m_Bounds).xz;
                QuadTreeBoundsXZ bounds1;
                // ISSUE: reference to a compiler-generated field
                if (this.m_SearchTree.TryGet(entity, out bounds1))
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
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<NodeGeometry> nativeArray9 = chunk.GetNativeArray<NodeGeometry>(ref this.m_NodeGeometryType);
              for (int index = 0; index < nativeArray5.Length; ++index)
              {
                Entity entity = nativeArray5[index];
                Bounds2 xz3 = nativeArray9[index].m_Bounds.xz;
                QuadTreeBoundsXZ bounds3;
                // ISSUE: reference to a compiler-generated field
                if (this.m_SearchTree.TryGet(entity, out bounds3))
                {
                  Bounds2 xz4 = bounds3.m_Bounds.xz;
                  Bounds2 bounds4 = xz4 | xz3;
                  if ((double) math.length(MathUtils.Size(bounds4)) < (double) math.length(MathUtils.Size(xz4)) + (double) math.length(MathUtils.Size(xz3)))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_ResultQueue.Enqueue(bounds4);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_ResultQueue.Enqueue(xz4);
                    // ISSUE: reference to a compiler-generated field
                    this.m_ResultQueue.Enqueue(xz3);
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ResultQueue.Enqueue(xz3);
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
    private struct CollectUpdatedLaneBoundsJob : IJobChunk
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
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<NetLaneGeometryData> m_PrefabLaneGeometryData;
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
          NativeArray<Curve> nativeArray1 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Bounds3 bounds = MathUtils.Bounds(nativeArray1[index].m_Bezier);
            NetLaneGeometryData componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabLaneGeometryData.TryGetComponent(nativeArray2[index].m_Prefab, out componentData))
              bounds = MathUtils.Expand(bounds, (float3) (componentData.m_Size.x * 0.5f));
            // ISSUE: reference to a compiler-generated field
            this.m_ResultQueue.Enqueue(bounds.xz);
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
            NativeArray<Curve> nativeArray4 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            for (int index = 0; index < nativeArray3.Length; ++index)
            {
              Entity entity = nativeArray3[index];
              Bounds2 bounds1 = MathUtils.Bounds(nativeArray4[index].m_Bezier).xz;
              NetLaneGeometryData componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabLaneGeometryData.TryGetComponent(nativeArray5[index].m_Prefab, out componentData))
                bounds1 = MathUtils.Expand(bounds1, (float2) (componentData.m_Size.x * 0.5f));
              QuadTreeBoundsXZ bounds2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SearchTree.TryGet(entity, out bounds2))
              {
                Bounds2 xz = bounds2.m_Bounds.xz;
                Bounds2 bounds3 = xz | bounds1;
                if ((double) math.length(MathUtils.Size(bounds3)) < (double) math.length(MathUtils.Size(xz)) + (double) math.length(MathUtils.Size(bounds1)))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ResultQueue.Enqueue(bounds3);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ResultQueue.Enqueue(xz);
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
      public ComponentTypeHandle<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<NodeGeometry> __Game_Net_NodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<NetLaneGeometryData> __Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup;

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
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetLaneGeometryData>(true);
      }
    }
  }
}
