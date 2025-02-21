// Decompiled with JetBrains decompiler
// Type: Game.Net.LaneObjectUpdater
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Net
{
  public struct LaneObjectUpdater
  {
    private Game.Objects.SearchSystem m_SearchSystem;
    private BufferLookup<LaneObject> m_LaneObjects;
    private NativeParallelQueue<LaneObjectAction> m_LaneActionQueue;
    private NativeQueue<TreeObjectAction> m_TreeActionQueue;

    public LaneObjectUpdater(SystemBase system)
    {
      this.m_SearchSystem = system.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      this.m_LaneObjects = system.GetBufferLookup<LaneObject>(false);
      this.m_LaneActionQueue = new NativeParallelQueue<LaneObjectAction>();
      this.m_TreeActionQueue = new NativeQueue<TreeObjectAction>();
    }

    public LaneObjectCommandBuffer Begin(Allocator allocator)
    {
      this.m_LaneActionQueue = new NativeParallelQueue<LaneObjectAction>((AllocatorManager.AllocatorHandle) allocator);
      this.m_TreeActionQueue = new NativeQueue<TreeObjectAction>((AllocatorManager.AllocatorHandle) allocator);
      return new LaneObjectCommandBuffer(this.m_LaneActionQueue.AsWriter(), this.m_TreeActionQueue.AsParallelWriter());
    }

    public JobHandle Apply(SystemBase system, JobHandle dependencies)
    {
      this.m_LaneObjects.Update(system);
      LaneObjectUpdater.UpdateLaneObjectsJob jobData1 = new LaneObjectUpdater.UpdateLaneObjectsJob()
      {
        m_LaneActions = this.m_LaneActionQueue.AsReader(),
        m_LaneObjects = this.m_LaneObjects
      };
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated method
      LaneObjectUpdater.UpdateTreeObjectsJob jobData2 = new LaneObjectUpdater.UpdateTreeObjectsJob()
      {
        m_TreeActions = this.m_TreeActionQueue,
        m_SearchTree = this.m_SearchSystem.GetMovingSearchTree(false, out dependencies1)
      };
      JobHandle inputDeps = jobData1.Schedule<LaneObjectUpdater.UpdateLaneObjectsJob>(this.m_LaneActionQueue.HashRange, 1, dependencies);
      JobHandle dependsOn = JobHandle.CombineDependencies(dependencies, dependencies1);
      JobHandle jobHandle = jobData2.Schedule<LaneObjectUpdater.UpdateTreeObjectsJob>(dependsOn);
      this.m_LaneActionQueue.Dispose(inputDeps);
      this.m_TreeActionQueue.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated method
      this.m_SearchSystem.AddMovingSearchTreeWriter(jobHandle);
      return inputDeps;
    }

    [BurstCompile]
    private struct UpdateLaneObjectsJob : IJobParallelFor
    {
      public NativeParallelQueue<LaneObjectAction>.Reader m_LaneActions;
      [NativeDisableParallelForRestriction]
      public BufferLookup<LaneObject> m_LaneObjects;

      public void Execute(int index)
      {
        NativeParallelQueue<LaneObjectAction>.Enumerator enumerator = this.m_LaneActions.GetEnumerator(index);
        while (enumerator.MoveNext())
        {
          LaneObjectAction current = enumerator.Current;
          DynamicBuffer<LaneObject> bufferData;
          if (this.m_LaneObjects.TryGetBuffer(current.m_Lane, out bufferData))
          {
            if (current.m_Add == current.m_Remove)
            {
              if (current.m_Add != Entity.Null)
                NetUtils.UpdateLaneObject(bufferData, current.m_Add, current.m_CurvePosition);
            }
            else
            {
              if (current.m_Remove != Entity.Null)
                NetUtils.RemoveLaneObject(bufferData, current.m_Remove);
              if (current.m_Add != Entity.Null)
                NetUtils.AddLaneObject(bufferData, current.m_Add, current.m_CurvePosition);
            }
          }
        }
        enumerator.Dispose();
      }
    }

    [BurstCompile]
    private struct UpdateTreeObjectsJob : IJob
    {
      public NativeQueue<TreeObjectAction> m_TreeActions;
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;

      public void Execute()
      {
        TreeObjectAction treeObjectAction;
        while (this.m_TreeActions.TryDequeue(out treeObjectAction))
        {
          if (treeObjectAction.m_Add == treeObjectAction.m_Remove)
          {
            if (treeObjectAction.m_Add != Entity.Null)
              this.m_SearchTree.Update(treeObjectAction.m_Add, new QuadTreeBoundsXZ(treeObjectAction.m_Bounds));
          }
          else
          {
            if (treeObjectAction.m_Remove != Entity.Null)
              this.m_SearchTree.TryRemove(treeObjectAction.m_Remove);
            if (treeObjectAction.m_Add != Entity.Null && !this.m_SearchTree.TryAdd(treeObjectAction.m_Add, new QuadTreeBoundsXZ(treeObjectAction.m_Bounds)))
            {
              float3 float3 = MathUtils.Center(treeObjectAction.m_Bounds);
              Debug.Log((object) string.Format("Entity already added to search tree ({0}: {1}, {2}, {3})", (object) treeObjectAction.m_Add.Index, (object) float3.x, (object) float3.y, (object) float3.z));
            }
          }
        }
      }
    }
  }
}
