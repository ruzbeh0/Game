// Decompiled with JetBrains decompiler
// Type: Game.Net.OverrideSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class OverrideSystem : GameSystemBase
  {
    private const float MIN_PARALLEL_FENCE_DISTANCE = 1.6f;
    private UpdateCollectSystem m_NetUpdateCollectSystem;
    private SearchSystem m_NetSearchSystem;
    private ModificationBarrier5 m_ModificationBarrier;
    private ToolSystem m_ToolSystem;
    private OverrideSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_NetUpdateCollectSystem = this.World.GetOrCreateSystemManaged<UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_NetUpdateCollectSystem.lanesUpdated)
        return;
      NativeList<Entity> nativeList = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<OverrideSystem.TreeAction> nativeQueue = new NativeQueue<OverrideSystem.TreeAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated method
      this.Dependency = JobHandle.CombineDependencies(this.Dependency, this.CollectUpdatedLanes(nativeList));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CutRange_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Overridden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      OverrideSystem.CheckLaneOverrideJob jobData1 = new OverrideSystem.CheckLaneOverrideJob()
      {
        m_OverriddenData = this.__TypeHandle.__Game_Common_Overridden_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_UtilityLaneData = this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabUtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
        m_PrefabNetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_PrefabLaneGeometryData = this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup,
        m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_CutRanges = this.__TypeHandle.__Game_Net_CutRange_RW_BufferLookup,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_LaneArray = nativeList.AsDeferredJobArray(),
        m_LaneSearchTree = this.m_NetSearchSystem.GetLaneSearchTree(true, out dependencies1),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_TreeActions = nativeQueue.AsParallelWriter()
      };
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      OverrideSystem.UpdateOverriddenLayersJob jobData2 = new OverrideSystem.UpdateOverriddenLayersJob()
      {
        m_LaneSearchTree = this.m_NetSearchSystem.GetLaneSearchTree(false, out dependencies2),
        m_Actions = nativeQueue
      };
      JobHandle jobHandle1 = jobData1.Schedule<OverrideSystem.CheckLaneOverrideJob, Entity>(nativeList, 1, JobHandle.CombineDependencies(this.Dependency, dependencies1));
      JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle1, dependencies2);
      JobHandle jobHandle2 = jobData2.Schedule<OverrideSystem.UpdateOverriddenLayersJob>(dependsOn);
      nativeList.Dispose(jobHandle1);
      nativeQueue.Dispose(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddLaneSearchTreeWriter(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle1);
      this.Dependency = jobHandle1;
    }

    private JobHandle CollectUpdatedLanes(NativeList<Entity> updateLanesList)
    {
      NativeQueue<Entity> nativeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeQuadTree<Entity, QuadTreeBoundsXZ> laneSearchTree = this.m_NetSearchSystem.GetLaneSearchTree(true, out dependencies1);
      JobHandle jobHandle1 = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (this.m_NetUpdateCollectSystem.lanesUpdated)
      {
        JobHandle dependencies2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedLaneBounds = this.m_NetUpdateCollectSystem.GetUpdatedLaneBounds(out dependencies2);
        // ISSUE: object of a compiler-generated type is created
        JobHandle jobHandle2 = new OverrideSystem.FindUpdatedLanesJob()
        {
          m_Bounds = updatedLaneBounds.AsDeferredJobArray(),
          m_SearchTree = laneSearchTree,
          m_ResultQueue = nativeQueue.AsParallelWriter()
        }.Schedule<OverrideSystem.FindUpdatedLanesJob, Bounds2>(updatedLaneBounds, 1, JobHandle.CombineDependencies(dependencies2, dependencies1));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NetUpdateCollectSystem.AddLaneBoundsReader(jobHandle2);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle2);
      }
      // ISSUE: object of a compiler-generated type is created
      JobHandle inputDeps = new OverrideSystem.CollectObjectsJob()
      {
        m_Queue = nativeQueue,
        m_ResultList = updateLanesList
      }.Schedule<OverrideSystem.CollectObjectsJob>(jobHandle1);
      nativeQueue.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddLaneSearchTreeReader(jobHandle1);
      return inputDeps;
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
    public OverrideSystem()
    {
    }

    private struct TreeAction
    {
      public Entity m_Entity;
      public BoundsMask m_Mask;
    }

    [BurstCompile]
    private struct UpdateOverriddenLayersJob : IJob
    {
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_LaneSearchTree;
      public NativeQueue<OverrideSystem.TreeAction> m_Actions;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        OverrideSystem.TreeAction treeAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_Actions.TryDequeue(out treeAction))
        {
          QuadTreeBoundsXZ bounds;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneSearchTree.TryGet(treeAction.m_Entity, out bounds))
          {
            // ISSUE: reference to a compiler-generated field
            bounds.m_Mask = bounds.m_Mask & ~(BoundsMask.AllLayers | BoundsMask.NotOverridden) | treeAction.m_Mask;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_LaneSearchTree.Update(treeAction.m_Entity, bounds);
          }
        }
      }
    }

    [BurstCompile]
    private struct FindUpdatedLanesJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Bounds2> m_Bounds;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;
      public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        OverrideSystem.FindUpdatedLanesJob.Iterator iterator = new OverrideSystem.FindUpdatedLanesJob.Iterator()
        {
          m_Bounds = MathUtils.Expand(this.m_Bounds[index], (float2) 1.6f),
          m_ResultQueue = this.m_ResultQueue
        };
        // ISSUE: reference to a compiler-generated field
        this.m_SearchTree.Iterate<OverrideSystem.FindUpdatedLanesJob.Iterator>(ref iterator);
      }

      private struct Iterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity objectEntity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_ResultQueue.Enqueue(objectEntity);
        }
      }
    }

    [BurstCompile]
    private struct CollectObjectsJob : IJob
    {
      public NativeQueue<Entity> m_Queue;
      public NativeList<Entity> m_ResultList;

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
        // ISSUE: reference to a compiler-generated field
        NativeList<Entity> resultList = this.m_ResultList;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        OverrideSystem.CollectObjectsJob.EntityComparer entityComparer = new OverrideSystem.CollectObjectsJob.EntityComparer();
        // ISSUE: variable of a compiler-generated type
        OverrideSystem.CollectObjectsJob.EntityComparer comp = entityComparer;
        resultList.Sort<Entity, OverrideSystem.CollectObjectsJob.EntityComparer>(comp);
        Entity entity = Entity.Null;
        int num = 0;
        int index1 = 0;
        // ISSUE: reference to a compiler-generated field
        while (num < this.m_ResultList.Length)
        {
          // ISSUE: reference to a compiler-generated field
          Entity result = this.m_ResultList[num++];
          if (result != entity)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ResultList[index1++] = result;
            entity = result;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (index1 >= this.m_ResultList.Length)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ResultList.RemoveRangeSwapBack(index1, this.m_ResultList.Length - index1);
      }

      [StructLayout(LayoutKind.Sequential, Size = 1)]
      private struct EntityComparer : IComparer<Entity>
      {
        public int Compare(Entity x, Entity y) => x.Index - y.Index;
      }
    }

    [BurstCompile]
    private struct CheckLaneOverrideJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Overridden> m_OverriddenData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<UtilityLane> m_UtilityLaneData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> m_PrefabUtilityLaneData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabNetLaneData;
      [ReadOnly]
      public ComponentLookup<NetLaneGeometryData> m_PrefabLaneGeometryData;
      [ReadOnly]
      public ComponentLookup<NetData> m_PrefabNetData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<CutRange> m_CutRanges;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public NativeArray<Entity> m_LaneArray;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_LaneSearchTree;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<OverrideSystem.TreeAction>.ParallelWriter m_TreeActions;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity lane = this.m_LaneArray[index];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[lane];
        UtilityLaneData componentData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabUtilityLaneData.TryGetComponent(prefabRef.m_Prefab, out componentData) || (componentData.m_UtilityTypes & UtilityTypes.Fence) == UtilityTypes.None)
          return;
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[lane];
        // ISSUE: reference to a compiler-generated field
        UtilityLane utilityLane = this.m_UtilityLaneData[lane];
        // ISSUE: reference to a compiler-generated field
        NetLaneGeometryData laneGeometryData = this.m_PrefabLaneGeometryData[prefabRef.m_Prefab];
        float range = (float) ((double) laneGeometryData.m_Size.x * 0.5 + 1.6000000238418579);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        OverrideSystem.CheckLaneOverrideJob.LaneIterator iterator = new OverrideSystem.CheckLaneOverrideJob.LaneIterator()
        {
          m_Range = range,
          m_SizeLimit = range * 4f,
          m_Priority = componentData.m_VisualCapacity,
          m_LaneEntity = lane,
          m_LaneCurve = curve,
          m_CurveData = this.m_CurveData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabUtilityLaneData = this.m_PrefabUtilityLaneData,
          m_PrefabNetLaneData = this.m_PrefabNetLaneData,
          m_PrefabLaneGeometryData = this.m_PrefabLaneGeometryData,
          m_CutForTraffic = (utilityLane.m_Flags & UtilityLaneFlags.CutForTraffic) != 0
        };
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        OverrideSystem.CheckLaneOverrideJob.LaneIteratorSubData subData = new OverrideSystem.CheckLaneOverrideJob.LaneIteratorSubData();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        MathUtils.Divide(curve.m_Bezier, out subData.m_Curve1, out subData.m_Curve2, 0.5f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        subData.m_Bounds1 = MathUtils.Expand(MathUtils.Bounds(subData.m_Curve1), (float3) range);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        subData.m_Bounds2 = MathUtils.Expand(MathUtils.Bounds(subData.m_Curve2), (float3) range);
        // ISSUE: reference to a compiler-generated field
        this.m_LaneSearchTree.Iterate<OverrideSystem.CheckLaneOverrideJob.LaneIterator, OverrideSystem.CheckLaneOverrideJob.LaneIteratorSubData>(ref iterator, subData);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (iterator.m_CutForTraffic && !iterator.m_FullOverride && (!iterator.m_CutRangeList.IsCreated || iterator.m_CutRangeList.Length == 0))
        {
          float num = math.min(0.25f, 3f / math.max(1f / 1000f, curve.m_Length));
          // ISSUE: reference to a compiler-generated method
          iterator.AddCutRange(new Bounds1(0.5f - num, 0.5f + num));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CurveData = iterator.m_CurveData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabRefData = iterator.m_PrefabRefData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabUtilityLaneData = iterator.m_PrefabUtilityLaneData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabNetLaneData = iterator.m_PrefabNetLaneData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabLaneGeometryData = iterator.m_PrefabLaneGeometryData;
        // ISSUE: reference to a compiler-generated field
        if (iterator.m_FullOverride)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_OverriddenData.HasComponent(lane))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_CutRanges.HasBuffer(lane))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<CutRange>(index, lane);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(index, lane, new Updated());
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Overridden>(index, lane, new Overridden());
            // ISSUE: reference to a compiler-generated method
            this.AddTreeAction(lane, curve, utilityLane, laneGeometryData, true);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (iterator.m_CutRangeList.IsCreated && iterator.m_CutRangeList.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_OverriddenData.HasComponent(lane))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(index, lane, new Updated());
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Overridden>(index, lane);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddBuffer<CutRange>(index, lane).AddRange(iterator.m_CutRangeList.AsArray());
              // ISSUE: reference to a compiler-generated method
              this.AddTreeAction(lane, curve, utilityLane, laneGeometryData, false);
            }
            else
            {
              DynamicBuffer<CutRange> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_CutRanges.TryGetBuffer(lane, out bufferData))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                if (!this.IsEqual(bufferData, iterator.m_CutRangeList))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Updated>(index, lane, new Updated());
                  // ISSUE: reference to a compiler-generated field
                  bufferData.CopyFrom(iterator.m_CutRangeList.AsArray());
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Updated>(index, lane, new Updated());
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddBuffer<CutRange>(index, lane).AddRange(iterator.m_CutRangeList.AsArray());
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_OverriddenData.HasComponent(lane))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(index, lane, new Updated());
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Overridden>(index, lane);
              // ISSUE: reference to a compiler-generated method
              this.AddTreeAction(lane, curve, utilityLane, laneGeometryData, false);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_CutRanges.HasBuffer(lane))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Updated>(index, lane, new Updated());
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<CutRange>(index, lane);
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!iterator.m_CutRangeList.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        iterator.m_CutRangeList.Dispose();
      }

      private void AddTreeAction(
        Entity entity,
        Curve curve,
        UtilityLane utilityLane,
        NetLaneGeometryData laneGeometryData,
        bool isOverridden)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        OverrideSystem.TreeAction treeAction = new OverrideSystem.TreeAction()
        {
          m_Entity = entity
        };
        if (!isOverridden)
        {
          // ISSUE: reference to a compiler-generated field
          treeAction.m_Mask |= BoundsMask.NotOverridden;
          if ((double) curve.m_Length > 0.10000000149011612)
          {
            // ISSUE: reference to a compiler-generated field
            MeshLayer defaultLayers = this.m_EditorMode ? laneGeometryData.m_EditorLayers : laneGeometryData.m_GameLayers;
            Owner componentData;
            // ISSUE: reference to a compiler-generated field
            this.m_OwnerData.TryGetComponent(entity, out componentData);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            treeAction.m_Mask |= CommonUtils.GetBoundsMask(SearchSystem.GetLayers(componentData, utilityLane, defaultLayers, ref this.m_PrefabRefData, ref this.m_PrefabNetData, ref this.m_PrefabNetGeometryData));
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_TreeActions.Enqueue(treeAction);
      }

      private bool IsEqual(DynamicBuffer<CutRange> cutRanges1, NativeList<CutRange> cutRanges2)
      {
        if (cutRanges1.Length != cutRanges2.Length)
          return false;
        for (int index = 0; index < cutRanges1.Length; ++index)
        {
          if (!cutRanges1[index].m_CurveDelta.Equals(cutRanges2[index].m_CurveDelta))
            return false;
        }
        return true;
      }

      private struct LaneIteratorSubData
      {
        public Bounds3 m_Bounds1;
        public Bounds3 m_Bounds2;
        public Bezier4x3 m_Curve1;
        public Bezier4x3 m_Curve2;
      }

      private struct LaneIterator : 
        INativeQuadTreeIteratorWithSubData<Entity, QuadTreeBoundsXZ, OverrideSystem.CheckLaneOverrideJob.LaneIteratorSubData>,
        IUnsafeQuadTreeIteratorWithSubData<Entity, QuadTreeBoundsXZ, OverrideSystem.CheckLaneOverrideJob.LaneIteratorSubData>
      {
        public float m_Range;
        public float m_SizeLimit;
        public float m_Priority;
        public Entity m_LaneEntity;
        public Curve m_LaneCurve;
        public NativeList<CutRange> m_CutRangeList;
        public ComponentLookup<Curve> m_CurveData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<UtilityLaneData> m_PrefabUtilityLaneData;
        public ComponentLookup<NetLaneData> m_PrefabNetLaneData;
        public ComponentLookup<NetLaneGeometryData> m_PrefabLaneGeometryData;
        public bool m_CutForTraffic;
        public bool m_FullOverride;

        public bool Intersect(
          QuadTreeBoundsXZ bounds,
          ref OverrideSystem.CheckLaneOverrideJob.LaneIteratorSubData subData)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_FullOverride)
            return false;
          bool2 x;
          // ISSUE: reference to a compiler-generated field
          x.x = MathUtils.Intersect(bounds.m_Bounds, subData.m_Bounds1);
          // ISSUE: reference to a compiler-generated field
          x.y = MathUtils.Intersect(bounds.m_Bounds, subData.m_Bounds2);
          if (!math.any(x))
            return false;
          if (math.all(x))
            return true;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          while (math.any(MathUtils.Size(subData.m_Bounds1) > this.m_SizeLimit))
          {
            if (x.x)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              MathUtils.Divide(subData.m_Curve1, out subData.m_Curve1, out subData.m_Curve2, 0.5f);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              MathUtils.Divide(subData.m_Curve2, out subData.m_Curve1, out subData.m_Curve2, 0.5f);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            subData.m_Bounds1 = MathUtils.Expand(MathUtils.Bounds(subData.m_Curve1), (float3) this.m_Range);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            subData.m_Bounds2 = MathUtils.Expand(MathUtils.Bounds(subData.m_Curve2), (float3) this.m_Range);
            // ISSUE: reference to a compiler-generated field
            x.x = MathUtils.Intersect(bounds.m_Bounds, subData.m_Bounds1);
            // ISSUE: reference to a compiler-generated field
            x.y = MathUtils.Intersect(bounds.m_Bounds, subData.m_Bounds2);
            if (!math.any(x))
              return false;
            if (math.all(x))
              return true;
          }
          return true;
        }

        public void Iterate(
          QuadTreeBoundsXZ bounds,
          OverrideSystem.CheckLaneOverrideJob.LaneIteratorSubData subData,
          Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_FullOverride || this.m_LaneEntity == entity)
            return;
          bool2 x;
          // ISSUE: reference to a compiler-generated field
          x.x = MathUtils.Intersect(bounds.m_Bounds, subData.m_Bounds1);
          // ISSUE: reference to a compiler-generated field
          x.y = MathUtils.Intersect(bounds.m_Bounds, subData.m_Bounds2);
          if (!math.any(x))
            return;
          Bounds1 bounds1 = new Bounds1(1f, 0.0f);
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[entity];
          UtilityLaneData componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabUtilityLaneData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
          {
            // ISSUE: reference to a compiler-generated field
            if ((componentData1.m_UtilityTypes & UtilityTypes.Fence) == UtilityTypes.None || (double) componentData1.m_VisualCapacity < (double) this.m_Priority)
              return;
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[entity];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float num1 = this.m_Range + this.m_PrefabLaneGeometryData[prefabRef.m_Prefab].m_Size.x * 0.5f;
            Bounds1 bounds2 = new Bounds1(1f, 0.0f);
            float t1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if ((double) MathUtils.Distance(this.m_LaneCurve.m_Bezier, curve.m_Bezier.a, out t1) < (double) num1 && this.IsParallel(MathUtils.Tangent(this.m_LaneCurve.m_Bezier, t1), MathUtils.StartTangent(curve.m_Bezier)))
            {
              bounds1 |= t1;
              bounds2 |= 0.0f;
            }
            float t2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if ((double) MathUtils.Distance(this.m_LaneCurve.m_Bezier, curve.m_Bezier.d, out t2) < (double) num1 && this.IsParallel(MathUtils.Tangent(this.m_LaneCurve.m_Bezier, t2), MathUtils.EndTangent(curve.m_Bezier)))
            {
              bounds1 |= t2;
              bounds2 |= 1f;
            }
            float t3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if ((double) MathUtils.Distance(curve.m_Bezier, this.m_LaneCurve.m_Bezier.a, out t3) < (double) num1 && this.IsParallel(MathUtils.Tangent(curve.m_Bezier, t3), MathUtils.StartTangent(this.m_LaneCurve.m_Bezier)))
            {
              bounds1 |= 0.0f;
              bounds2 |= t3;
            }
            float t4;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if ((double) MathUtils.Distance(curve.m_Bezier, this.m_LaneCurve.m_Bezier.d, out t4) < (double) num1 && this.IsParallel(MathUtils.Tangent(curve.m_Bezier, t4), MathUtils.EndTangent(this.m_LaneCurve.m_Bezier)))
            {
              bounds1 |= 1f;
              bounds2 |= t4;
            }
            float num2 = MathUtils.Size(bounds1);
            float num3 = MathUtils.Size(bounds2);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((double) num2 <= 0.0 || (double) num3 <= 0.0 || (double) this.m_Priority == (double) componentData1.m_VisualCapacity && ((double) num3 > (double) num2 || (double) num2 == (double) num3 && this.m_LaneEntity.Index > entity.Index))
              return;
          }
          else
          {
            NetLaneData componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_CutForTraffic || !this.m_PrefabNetLaneData.TryGetComponent(prefabRef.m_Prefab, out componentData2) || (componentData2.m_Flags & (LaneFlags.Road | LaneFlags.Pedestrian)) == (LaneFlags) 0)
              return;
            float2 t;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (MathUtils.Intersect(this.m_LaneCurve.m_Bezier.xz, this.m_CurveData[entity].m_Bezier.xz, out t, 3))
            {
              // ISSUE: reference to a compiler-generated field
              float num = componentData2.m_Width * 0.5f / math.max(1f / 1000f, this.m_LaneCurve.m_Length);
              bounds1.min = math.clamp(t.x - num, 0.0f, bounds1.min);
              bounds1.max = math.clamp(t.x + num, bounds1.max, 1f);
            }
            if ((double) MathUtils.Size(bounds1) <= 0.0)
              return;
          }
          // ISSUE: reference to a compiler-generated method
          this.AddCutRange(bounds1);
        }

        public void AddCutRange(Bounds1 range)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) range.min * (double) this.m_LaneCurve.m_Length < (double) this.m_Range)
            range.min = 0.0f;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((1.0 - (double) range.max) * (double) this.m_LaneCurve.m_Length < (double) this.m_Range)
            range.max = 1f;
          if ((double) range.min == 0.0 && (double) range.max == 1.0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_FullOverride = true;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_CutRangeList.IsCreated)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CutRangeList = new NativeList<CutRange>(4, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            }
            CutRange cutRange2 = new CutRange()
            {
              m_CurveDelta = range
            };
            // ISSUE: reference to a compiler-generated field
            for (int index1 = 0; index1 < this.m_CutRangeList.Length; ++index1)
            {
              // ISSUE: reference to a compiler-generated field
              CutRange cutRange1 = this.m_CutRangeList[index1];
              // ISSUE: reference to a compiler-generated method
              if (this.ShouldMerge(cutRange1, cutRange2))
              {
                cutRange1.m_CurveDelta |= cutRange2.m_CurveDelta;
                int count = 0;
                // ISSUE: reference to a compiler-generated field
                for (int index2 = index1 + 1; index2 < this.m_CutRangeList.Length; ++index2)
                {
                  // ISSUE: reference to a compiler-generated field
                  CutRange cutRange3 = this.m_CutRangeList[index2];
                  // ISSUE: reference to a compiler-generated method
                  if (this.ShouldMerge(cutRange1, cutRange3))
                  {
                    cutRange1.m_CurveDelta |= cutRange3.m_CurveDelta;
                    ++count;
                  }
                  else
                    break;
                }
                if (count != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CutRangeList.RemoveRange(index1 + 1, count);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CutRangeList[index1] = cutRange1;
                if ((double) cutRange1.m_CurveDelta.min != 0.0 || (double) cutRange1.m_CurveDelta.max != 1.0)
                  return;
                // ISSUE: reference to a compiler-generated field
                this.m_FullOverride = true;
                // ISSUE: reference to a compiler-generated field
                this.m_CutRangeList.Clear();
                return;
              }
              if ((double) cutRange1.m_CurveDelta.min > (double) cutRange2.m_CurveDelta.min)
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.Insert<CutRange>(this.m_CutRangeList, index1, cutRange2);
                return;
              }
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CutRangeList.Add(in cutRange2);
          }
        }

        private bool IsParallel(float3 tangent1, float3 tangent2)
        {
          return (double) math.abs(math.dot(math.normalizesafe(tangent1.xz), math.normalizesafe(tangent2.xz))) > 0.949999988079071;
        }

        private bool ShouldMerge(CutRange cutRange1, CutRange cutRange2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return ((double) cutRange1.m_CurveDelta.min - (double) cutRange2.m_CurveDelta.max) * (double) this.m_LaneCurve.m_Length < (double) this.m_Range && ((double) cutRange2.m_CurveDelta.min - (double) cutRange1.m_CurveDelta.max) * (double) this.m_LaneCurve.m_Length < (double) this.m_Range;
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Overridden> __Game_Common_Overridden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UtilityLane> __Game_Net_UtilityLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> __Game_Prefabs_UtilityLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneGeometryData> __Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      public BufferLookup<CutRange> __Game_Net_CutRange_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Overridden_RO_ComponentLookup = state.GetComponentLookup<Overridden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_UtilityLane_RO_ComponentLookup = state.GetComponentLookup<UtilityLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup = state.GetComponentLookup<UtilityLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetLaneGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CutRange_RW_BufferLookup = state.GetBufferLookup<CutRange>();
      }
    }
  }
}
