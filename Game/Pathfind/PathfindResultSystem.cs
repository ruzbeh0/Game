// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathfindResultSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Common;
using Game.Net;
using Game.Serialization;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Pathfind
{
  [CompilerGenerated]
  public class PathfindResultSystem : GameSystemBase, IPreDeserialize
  {
    private PathfindQueueSystem m_PathfindQueueSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityCommandBuffer m_CommandBuffer;
    private EntityArchetype m_PathEventArchetype;
    private EntityArchetype m_CoverageEventArchetype;
    private uint m_PendingSimulationFrameIndex;
    private int m_PendingRequestCount;
    private Dictionary<Entity, int> m_ResultListIndex;
    private Dictionary<PathfindResultSystem.ResultKey, PathfindResultSystem.ResultValue> m_QueryStats;
    private NativeList<PathfindJobs.ResultItem> m_PathfindResultBuffer;
    private NativeList<CoverageJobs.ResultItem> m_CoverageResultBuffer;
    private NativeList<AvailabilityJobs.ResultItem> m_AvailabilityResultBuffer;
    private PathfindResultSystem.TypeHandle __TypeHandle;

    public uint pendingSimulationFrame
    {
      get
      {
        return math.min(this.m_PendingSimulationFrameIndex, this.m_PathfindSetupSystem.pendingSimulationFrame);
      }
    }

    public int pendingRequestCount
    {
      get => this.m_PendingRequestCount + this.m_PathfindSetupSystem.pendingRequestCount;
    }

    public Dictionary<PathfindResultSystem.ResultKey, PathfindResultSystem.ResultValue> queryStats
    {
      get => this.m_QueryStats;
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindQueueSystem = this.World.GetOrCreateSystemManaged<PathfindQueueSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<PathUpdated>());
      // ISSUE: reference to a compiler-generated field
      this.m_CoverageEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<CoverageUpdated>());
      // ISSUE: reference to a compiler-generated field
      this.m_ResultListIndex = new Dictionary<Entity, int>(10);
      // ISSUE: reference to a compiler-generated field
      this.m_QueryStats = new Dictionary<PathfindResultSystem.ResultKey, PathfindResultSystem.ResultValue>(10);
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindResultBuffer = new NativeList<PathfindJobs.ResultItem>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_CoverageResultBuffer = new NativeList<CoverageJobs.ResultItem>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_AvailabilityResultBuffer = new NativeList<AvailabilityJobs.ResultItem>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindResultBuffer.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_CoverageResultBuffer.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_AvailabilityResultBuffer.Dispose();
      base.OnDestroy();
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PendingSimulationFrameIndex = uint.MaxValue;
      // ISSUE: reference to a compiler-generated field
      this.m_PendingRequestCount = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_QueryStats.Clear();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      JobHandle dependency = this.Dependency;
      // ISSUE: reference to a compiler-generated field
      this.m_PendingSimulationFrameIndex = uint.MaxValue;
      // ISSUE: reference to a compiler-generated field
      this.m_PendingRequestCount = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_CommandBuffer = new EntityCommandBuffer();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.ProcessResults(this.m_PathfindQueueSystem.GetPathfindActions(), ref dependency, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.ProcessResults(this.m_PathfindQueueSystem.GetCoverageActions(), ref dependency, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.ProcessResults(this.m_PathfindQueueSystem.GetAvailabilityActions(), ref dependency, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.ProcessResults<CreateAction>(this.m_PathfindQueueSystem.GetCreateActions());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.ProcessResults<UpdateAction>(this.m_PathfindQueueSystem.GetUpdateActions());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.ProcessResults<DeleteAction>(this.m_PathfindQueueSystem.GetDeleteActions());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.ProcessResults<DensityAction>(this.m_PathfindQueueSystem.GetDensityActions());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.ProcessResults<TimeAction>(this.m_PathfindQueueSystem.GetTimeActions());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.ProcessResults<FlowAction>(this.m_PathfindQueueSystem.GetFlowActions());
      this.Dependency = dependency;
    }

    private void AddQueryStats(
      object system,
      PathfindResultSystem.QueryType queryType,
      SetupTargetType originType,
      SetupTargetType destinationType,
      int resultLength,
      int graphTraversal)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PathfindResultSystem.ResultKey key = new PathfindResultSystem.ResultKey()
      {
        m_System = system,
        m_QueryType = queryType,
        m_OriginType = originType,
        m_DestinationType = destinationType
      };
      // ISSUE: variable of a compiler-generated type
      PathfindResultSystem.ResultValue resultValue;
      // ISSUE: reference to a compiler-generated field
      if (this.m_QueryStats.TryGetValue(key, out resultValue))
      {
        // ISSUE: reference to a compiler-generated field
        ++resultValue.m_QueryCount;
        // ISSUE: reference to a compiler-generated field
        resultValue.m_SuccessCount += math.min(1, resultLength);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        resultValue.m_GraphTraversal += (float) graphTraversal / math.max(1f, (float) this.m_PathfindQueueSystem.GetGraphSize());
        // ISSUE: reference to a compiler-generated field
        resultValue.m_Efficiency += (float) resultLength / math.max(1f, (float) graphTraversal);
        // ISSUE: reference to a compiler-generated field
        this.m_QueryStats[key] = resultValue;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: object of a compiler-generated type is created
        this.m_QueryStats.Add(key, new PathfindResultSystem.ResultValue()
        {
          m_QueryCount = 1,
          m_SuccessCount = math.min(1, resultLength),
          m_GraphTraversal = (float) graphTraversal / math.max(1f, (float) this.m_PathfindQueueSystem.GetGraphSize()),
          m_Efficiency = (float) resultLength / math.max(1f, (float) graphTraversal)
        });
      }
    }

    private void ProcessResults(
      PathfindQueueSystem.ActionList<PathfindAction> list,
      ref JobHandle outputDeps,
      JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ResultListIndex.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindResultBuffer.Clear();
      int index1 = 0;
      // ISSUE: reference to a compiler-generated field
      for (int index2 = 0; index2 < list.m_Items.Count; ++index2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindQueueSystem.ActionListItem<PathfindAction> actionListItem = list.m_Items[index2];
        // ISSUE: reference to a compiler-generated field
        if ((actionListItem.m_Flags & PathFlags.Scheduled) != (PathFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (actionListItem.m_Action.data.m_State == PathfindActionState.Completed)
          {
            // ISSUE: reference to a compiler-generated field
            actionListItem.m_Flags &= ~PathFlags.Scheduled;
            // ISSUE: reference to a compiler-generated field
            ErrorCode errorCode = actionListItem.m_Action.data.m_Result[0].m_ErrorCode;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int graphTraversal = actionListItem.m_Action.data.m_Result[actionListItem.m_Action.data.m_Result.Length - 1].m_GraphTraversal;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int pathLength = actionListItem.m_Action.data.m_Result[actionListItem.m_Action.data.m_Result.Length - 1].m_PathLength;
            PathfindJobs.ResultItem resultItem;
            // ISSUE: reference to a compiler-generated field
            resultItem.m_Owner = actionListItem.m_Owner;
            // ISSUE: reference to a compiler-generated field
            resultItem.m_Result = actionListItem.m_Action.data.m_Result;
            // ISSUE: reference to a compiler-generated field
            resultItem.m_Path = actionListItem.m_Action.data.m_Path;
            int index3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ResultListIndex.TryGetValue(actionListItem.m_Owner, out index3))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_PathfindResultBuffer[index3] = resultItem;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ResultListIndex.Add(actionListItem.m_Owner, this.m_PathfindResultBuffer.Length);
              // ISSUE: reference to a compiler-generated field
              this.m_PathfindResultBuffer.Add(in resultItem);
            }
            if (errorCode != ErrorCode.None)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              COSystemBase.baseLog.ErrorFormat("Pathfind error ({0}: {1} -> {2}): {3} (Request: {4})", (object) actionListItem.m_System.GetType().Name, (object) actionListItem.m_Action.data.m_OriginType, (object) actionListItem.m_Action.data.m_DestinationType, (object) errorCode, (object) actionListItem.m_Owner);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.AddQueryStats(actionListItem.m_System, PathfindResultSystem.QueryType.Pathfind, actionListItem.m_Action.data.m_OriginType, actionListItem.m_Action.data.m_DestinationType, pathLength, graphTraversal);
            // ISSUE: reference to a compiler-generated field
            if ((actionListItem.m_Flags & PathFlags.WantsEvent) != (PathFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_CommandBuffer.IsCreated)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PathUpdated>(this.m_CommandBuffer.CreateEntity(this.m_PathEventArchetype), new PathUpdated(actionListItem.m_Owner, actionListItem.m_EventData));
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_PendingSimulationFrameIndex = math.min(this.m_PendingSimulationFrameIndex, actionListItem.m_ResultFrame);
            // ISSUE: reference to a compiler-generated field
            ++this.m_PendingRequestCount;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if ((actionListItem.m_Flags & PathFlags.Pending) != (PathFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_PendingSimulationFrameIndex = math.min(this.m_PendingSimulationFrameIndex, actionListItem.m_ResultFrame);
            // ISSUE: reference to a compiler-generated field
            ++this.m_PendingRequestCount;
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            actionListItem.Dispose();
            // ISSUE: reference to a compiler-generated field
            --list.m_NextIndex;
            continue;
          }
        }
        // ISSUE: reference to a compiler-generated field
        list.m_Items[index1++] = actionListItem;
      }
      // ISSUE: reference to a compiler-generated field
      if (index1 < list.m_Items.Count)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        list.m_Items.RemoveRange(index1, list.m_Items.Count - index1);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_PathfindResultBuffer.Length <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformations_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentLookup.Update(ref this.CheckedStateRef);
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
      JobHandle job1 = new PathfindJobs.ProcessResultsJob()
      {
        m_ResultItems = this.m_PathfindResultBuffer,
        m_PathOwner = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentLookup,
        m_PathInformation = this.__TypeHandle.__Game_Pathfind_PathInformation_RW_ComponentLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_PathInformations = this.__TypeHandle.__Game_Pathfind_PathInformations_RW_BufferLookup
      }.Schedule<PathfindJobs.ProcessResultsJob>(this.m_PathfindResultBuffer.Length, 1, inputDeps);
      outputDeps = JobHandle.CombineDependencies(outputDeps, job1);
    }

    private void ProcessResults(
      PathfindQueueSystem.ActionList<CoverageAction> list,
      ref JobHandle outputDeps,
      JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ResultListIndex.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_CoverageResultBuffer.Clear();
      int index1 = 0;
      // ISSUE: reference to a compiler-generated field
      for (int index2 = 0; index2 < list.m_Items.Count; ++index2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindQueueSystem.ActionListItem<CoverageAction> actionListItem = list.m_Items[index2];
        // ISSUE: reference to a compiler-generated field
        if ((actionListItem.m_Flags & PathFlags.Scheduled) != (PathFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (actionListItem.m_Action.data.m_State == PathfindActionState.Completed)
          {
            // ISSUE: reference to a compiler-generated field
            actionListItem.m_Flags &= ~PathFlags.Scheduled;
            CoverageJobs.ResultItem resultItem;
            // ISSUE: reference to a compiler-generated field
            resultItem.m_Owner = actionListItem.m_Owner;
            // ISSUE: reference to a compiler-generated field
            resultItem.m_Results = actionListItem.m_Action.data.m_Results;
            int index3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ResultListIndex.TryGetValue(actionListItem.m_Owner, out index3))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CoverageResultBuffer[index3] = resultItem;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ResultListIndex.Add(actionListItem.m_Owner, this.m_CoverageResultBuffer.Length);
              // ISSUE: reference to a compiler-generated field
              this.m_CoverageResultBuffer.Add(in resultItem);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.AddQueryStats(actionListItem.m_System, PathfindResultSystem.QueryType.Coverage, SetupTargetType.None, SetupTargetType.None, resultItem.m_Results.Length, resultItem.m_Results.Length);
            // ISSUE: reference to a compiler-generated field
            if ((actionListItem.m_Flags & PathFlags.WantsEvent) != (PathFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_CommandBuffer.IsCreated)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<CoverageUpdated>(this.m_CommandBuffer.CreateEntity(this.m_CoverageEventArchetype), new CoverageUpdated(actionListItem.m_Owner, actionListItem.m_EventData));
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_PendingSimulationFrameIndex = math.min(this.m_PendingSimulationFrameIndex, actionListItem.m_ResultFrame);
            // ISSUE: reference to a compiler-generated field
            ++this.m_PendingRequestCount;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if ((actionListItem.m_Flags & PathFlags.Pending) != (PathFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_PendingSimulationFrameIndex = math.min(this.m_PendingSimulationFrameIndex, actionListItem.m_ResultFrame);
            // ISSUE: reference to a compiler-generated field
            ++this.m_PendingRequestCount;
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            actionListItem.Dispose();
            // ISSUE: reference to a compiler-generated field
            --list.m_NextIndex;
            continue;
          }
        }
        // ISSUE: reference to a compiler-generated field
        list.m_Items[index1++] = actionListItem;
      }
      // ISSUE: reference to a compiler-generated field
      if (index1 < list.m_Items.Count)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        list.m_Items.RemoveRange(index1, list.m_Items.Count - index1);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_CoverageResultBuffer.Length <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_CoverageElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JobHandle job1 = new CoverageJobs.ProcessResultsJob()
      {
        m_ResultItems = this.m_CoverageResultBuffer,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_CoverageElements = this.__TypeHandle.__Game_Pathfind_CoverageElement_RW_BufferLookup
      }.Schedule<CoverageJobs.ProcessResultsJob>(this.m_CoverageResultBuffer.Length, 1, inputDeps);
      outputDeps = JobHandle.CombineDependencies(outputDeps, job1);
    }

    private void ProcessResults(
      PathfindQueueSystem.ActionList<AvailabilityAction> list,
      ref JobHandle outputDeps,
      JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ResultListIndex.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_AvailabilityResultBuffer.Clear();
      int index1 = 0;
      // ISSUE: reference to a compiler-generated field
      for (int index2 = 0; index2 < list.m_Items.Count; ++index2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindQueueSystem.ActionListItem<AvailabilityAction> actionListItem = list.m_Items[index2];
        // ISSUE: reference to a compiler-generated field
        if ((actionListItem.m_Flags & PathFlags.Scheduled) != (PathFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (actionListItem.m_Action.data.m_State == PathfindActionState.Completed)
          {
            // ISSUE: reference to a compiler-generated field
            actionListItem.m_Flags &= ~PathFlags.Scheduled;
            AvailabilityJobs.ResultItem resultItem;
            // ISSUE: reference to a compiler-generated field
            resultItem.m_Owner = actionListItem.m_Owner;
            // ISSUE: reference to a compiler-generated field
            resultItem.m_Results = actionListItem.m_Action.data.m_Results;
            int index3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ResultListIndex.TryGetValue(actionListItem.m_Owner, out index3))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_AvailabilityResultBuffer[index3] = resultItem;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ResultListIndex.Add(actionListItem.m_Owner, this.m_AvailabilityResultBuffer.Length);
              // ISSUE: reference to a compiler-generated field
              this.m_AvailabilityResultBuffer.Add(in resultItem);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.AddQueryStats(actionListItem.m_System, PathfindResultSystem.QueryType.Availability, SetupTargetType.None, SetupTargetType.None, resultItem.m_Results.Length, resultItem.m_Results.Length);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_PendingSimulationFrameIndex = math.min(this.m_PendingSimulationFrameIndex, actionListItem.m_ResultFrame);
            // ISSUE: reference to a compiler-generated field
            ++this.m_PendingRequestCount;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if ((actionListItem.m_Flags & PathFlags.Pending) != (PathFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_PendingSimulationFrameIndex = math.min(this.m_PendingSimulationFrameIndex, actionListItem.m_ResultFrame);
            // ISSUE: reference to a compiler-generated field
            ++this.m_PendingRequestCount;
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            actionListItem.Dispose();
            // ISSUE: reference to a compiler-generated field
            --list.m_NextIndex;
            continue;
          }
        }
        // ISSUE: reference to a compiler-generated field
        list.m_Items[index1++] = actionListItem;
      }
      // ISSUE: reference to a compiler-generated field
      if (index1 < list.m_Items.Count)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        list.m_Items.RemoveRange(index1, list.m_Items.Count - index1);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_AvailabilityResultBuffer.Length <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_AvailabilityElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JobHandle job1 = new AvailabilityJobs.ProcessResultsJob()
      {
        m_ResultItems = this.m_AvailabilityResultBuffer,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_AvailabilityElements = this.__TypeHandle.__Game_Pathfind_AvailabilityElement_RW_BufferLookup
      }.Schedule<AvailabilityJobs.ProcessResultsJob>(this.m_AvailabilityResultBuffer.Length, 1, inputDeps);
      outputDeps = JobHandle.CombineDependencies(outputDeps, job1);
    }

    private void ProcessResults<T>(PathfindQueueSystem.ActionList<T> list) where T : struct, IDisposable
    {
      int index1 = 0;
      // ISSUE: reference to a compiler-generated field
      for (int index2 = 0; index2 < list.m_Items.Count; ++index2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindQueueSystem.ActionListItem<T> actionListItem = list.m_Items[index2];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((actionListItem.m_Flags & PathFlags.Pending) == (PathFlags) 0 && actionListItem.m_Dependencies.IsCompleted)
        {
          // ISSUE: reference to a compiler-generated field
          actionListItem.m_Dependencies.Complete();
          // ISSUE: reference to a compiler-generated method
          actionListItem.Dispose();
          // ISSUE: reference to a compiler-generated field
          --list.m_NextIndex;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          list.m_Items[index1++] = actionListItem;
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (index1 >= list.m_Items.Count)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      list.m_Items.RemoveRange(index1, list.m_Items.Count - index1);
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

    [Preserve]
    public PathfindResultSystem()
    {
    }

    public enum QueryType
    {
      Pathfind,
      Coverage,
      Availability,
    }

    public struct ResultKey : IEquatable<PathfindResultSystem.ResultKey>
    {
      public object m_System;
      public PathfindResultSystem.QueryType m_QueryType;
      public SetupTargetType m_OriginType;
      public SetupTargetType m_DestinationType;

      public bool Equals(PathfindResultSystem.ResultKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_System == other.m_System & this.m_QueryType == other.m_QueryType & this.m_OriginType == other.m_OriginType & this.m_DestinationType == other.m_DestinationType;
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return ((this.m_System.GetHashCode() * 31 + this.m_QueryType.GetHashCode()) * 31 + this.m_OriginType.GetHashCode()) * 31 + this.m_DestinationType.GetHashCode();
      }
    }

    public struct ResultValue
    {
      public int m_QueryCount;
      public int m_SuccessCount;
      public float m_GraphTraversal;
      public float m_Efficiency;
    }

    private struct TypeHandle
    {
      public ComponentLookup<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentLookup;
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RW_ComponentLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;
      public BufferLookup<PathInformations> __Game_Pathfind_PathInformations_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
      public BufferLookup<CoverageElement> __Game_Pathfind_CoverageElement_RW_BufferLookup;
      public BufferLookup<AvailabilityElement> __Game_Pathfind_AvailabilityElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentLookup = state.GetComponentLookup<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RW_ComponentLookup = state.GetComponentLookup<PathInformation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformations_RW_BufferLookup = state.GetBufferLookup<PathInformations>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_CoverageElement_RW_BufferLookup = state.GetBufferLookup<CoverageElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_AvailabilityElement_RW_BufferLookup = state.GetBufferLookup<AvailabilityElement>();
      }
    }
  }
}
