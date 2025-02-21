// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathfindQueueSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Prefabs;
using Game.Serialization;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;

#nullable disable
namespace Game.Pathfind
{
  [CompilerGenerated]
  public class PathfindQueueSystem : GameSystemBase, IPreDeserialize
  {
    private const int WORKER_DATA_COUNT = 2;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private TransportLineSystem m_TransportLineSystem;
    private NetInitializeSystem m_NetInitializeSystem;
    private PathfindQueueSystem.ActionList<CreateAction> m_CreateActions;
    private PathfindQueueSystem.ActionList<UpdateAction> m_UpdateActions;
    private PathfindQueueSystem.ActionList<DeleteAction> m_DeleteActions;
    private PathfindQueueSystem.ActionList<PathfindAction> m_PathfindActions;
    private PathfindQueueSystem.ActionList<CoverageAction> m_CoverageActions;
    private PathfindQueueSystem.ActionList<AvailabilityAction> m_AvailabilityActions;
    private PathfindQueueSystem.ActionList<DensityAction> m_DensityActions;
    private PathfindQueueSystem.ActionList<TimeAction> m_TimeActions;
    private PathfindQueueSystem.ActionList<FlowAction> m_FlowActions;
    private Queue<PathfindQueueSystem.ActionType> m_ActionTypes;
    private Queue<PathfindQueueSystem.ActionType> m_HighPriorityTypes;
    private Queue<PathfindQueueSystem.ActionType> m_ModificationTypes;
    private Queue<PathfindQueueSystem.WorkerActions> m_WorkerActions;
    private Queue<PathfindQueueSystem.WorkerActions> m_WorkerActionPool;
    private List<PathfindQueueSystem.WorkerData> m_WorkerData;
    private List<PathfindQueueSystem.ThreadData> m_ThreadData;
    private List<AllocatorHelper<UnsafeLinearAllocator>> m_AllocatorPool;
    private int m_MaxThreadCount;
    private int m_NextWorkerIndex;
    private int m_LastWorkerIndex;
    private int m_DependencyIndex;
    private bool m_RequireDebug;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TransportLineSystem = this.World.GetOrCreateSystemManaged<TransportLineSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetInitializeSystem = this.World.GetOrCreateSystemManaged<NetInitializeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MaxThreadCount = math.max(1, JobsUtility.JobWorkerCount / 2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_CreateActions = new PathfindQueueSystem.ActionList<CreateAction>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_UpdateActions = new PathfindQueueSystem.ActionList<UpdateAction>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_DeleteActions = new PathfindQueueSystem.ActionList<DeleteAction>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_PathfindActions = new PathfindQueueSystem.ActionList<PathfindAction>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_CoverageActions = new PathfindQueueSystem.ActionList<CoverageAction>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_AvailabilityActions = new PathfindQueueSystem.ActionList<AvailabilityAction>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_DensityActions = new PathfindQueueSystem.ActionList<DensityAction>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_TimeActions = new PathfindQueueSystem.ActionList<TimeAction>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_FlowActions = new PathfindQueueSystem.ActionList<FlowAction>();
      // ISSUE: reference to a compiler-generated field
      this.m_ActionTypes = new Queue<PathfindQueueSystem.ActionType>();
      // ISSUE: reference to a compiler-generated field
      this.m_HighPriorityTypes = new Queue<PathfindQueueSystem.ActionType>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationTypes = new Queue<PathfindQueueSystem.ActionType>();
      // ISSUE: reference to a compiler-generated field
      this.m_WorkerActions = new Queue<PathfindQueueSystem.WorkerActions>();
      // ISSUE: reference to a compiler-generated field
      this.m_WorkerActionPool = new Queue<PathfindQueueSystem.WorkerActions>();
      // ISSUE: reference to a compiler-generated field
      this.m_WorkerData = new List<PathfindQueueSystem.WorkerData>(2);
      for (int index = 0; index < 2; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_WorkerData.Add(new PathfindQueueSystem.WorkerData(Allocator.Persistent));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ThreadData = new List<PathfindQueueSystem.ThreadData>(this.m_MaxThreadCount);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AllocatorPool = new List<AllocatorHelper<UnsafeLinearAllocator>>(this.m_MaxThreadCount);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_ThreadData.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindQueueSystem.ThreadData threadData = this.m_ThreadData[index];
        // ISSUE: reference to a compiler-generated field
        threadData.m_JobHandle.Complete();
        // ISSUE: reference to a compiler-generated field
        UnsafeLinearAllocator unsafeLinearAllocator = threadData.m_Allocator.Allocator;
        // ISSUE: reference to a compiler-generated field
        threadData.m_Allocator.Dispose();
        unsafeLinearAllocator.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_AllocatorPool.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        UnsafeLinearAllocator unsafeLinearAllocator = this.m_AllocatorPool[index].Allocator;
        // ISSUE: reference to a compiler-generated field
        this.m_AllocatorPool[index].Dispose();
        unsafeLinearAllocator.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CreateActions.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_UpdateActions.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DeleteActions.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindActions.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CoverageActions.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AvailabilityActions.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DensityActions.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TimeActions.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_FlowActions.Dispose();
      // ISSUE: variable of a compiler-generated type
      PathfindQueueSystem.WorkerActions workerActions1;
      // ISSUE: reference to a compiler-generated field
      while (this.m_WorkerActions.TryDequeue(ref workerActions1))
      {
        // ISSUE: reference to a compiler-generated method
        workerActions1.Dispose();
      }
      // ISSUE: variable of a compiler-generated type
      PathfindQueueSystem.WorkerActions workerActions2;
      // ISSUE: reference to a compiler-generated field
      while (this.m_WorkerActionPool.TryDequeue(ref workerActions2))
      {
        // ISSUE: reference to a compiler-generated method
        workerActions2.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_WorkerData.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_WorkerData[index].Dispose();
      }
      base.OnDestroy();
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_ThreadData.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindQueueSystem.ThreadData threadData = this.m_ThreadData[index];
        // ISSUE: reference to a compiler-generated field
        threadData.m_JobHandle.Complete();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AllocatorPool.Add(threadData.m_Allocator);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_ThreadData.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_DependencyIndex = 0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CreateActions.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_UpdateActions.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DeleteActions.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindActions.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CoverageActions.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AvailabilityActions.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DensityActions.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TimeActions.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_FlowActions.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_ActionTypes.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_HighPriorityTypes.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationTypes.Clear();
      // ISSUE: variable of a compiler-generated type
      PathfindQueueSystem.WorkerActions workerActions;
      // ISSUE: reference to a compiler-generated field
      while (this.m_WorkerActions.TryDequeue(ref workerActions))
      {
        // ISSUE: reference to a compiler-generated method
        workerActions.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_WorkerActionPool.Enqueue(workerActions);
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_WorkerData.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_WorkerData[index].Clear();
      }
    }

    public NativePathfindData GetDataContainer(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      PathfindQueueSystem.WorkerData workerData = this.m_WorkerData[this.m_NextWorkerIndex];
      // ISSUE: reference to a compiler-generated field
      dependencies = workerData.m_WriteHandle;
      // ISSUE: reference to a compiler-generated field
      return workerData.m_PathfindData;
    }

    public void AddDataReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      PathfindQueueSystem.WorkerData workerData = this.m_WorkerData[this.m_NextWorkerIndex];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      workerData.m_ReadHandle = JobHandle.CombineDependencies(workerData.m_ReadHandle, handle);
    }

    public void RequireDebug() => this.m_RequireDebug = true;

    public int GetGraphSize() => this.m_WorkerData[this.m_NextWorkerIndex].m_PathfindData.Size;

    public void GetGraphMemory(out uint usedMemory, out uint allocatedMemory)
    {
      usedMemory = 0U;
      allocatedMemory = 0U;
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_WorkerData.Count; ++index)
      {
        uint used;
        uint allocated;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_WorkerData[index].m_PathfindData.GetMemoryStats(out used, out allocated);
        usedMemory += used;
        allocatedMemory += allocated;
      }
    }

    public void GetQueryMemory(out uint usedMemory, out uint allocatedMemory)
    {
      usedMemory = 0U;
      allocatedMemory = 0U;
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_ThreadData.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindQueueSystem.ThreadData threadData = this.m_ThreadData[index];
        // ISSUE: reference to a compiler-generated field
        ref UnsafeLinearAllocator local = ref threadData.m_Allocator.Allocator;
        usedMemory += local.Used;
        allocatedMemory += local.Size;
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_AllocatorPool.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        ref UnsafeLinearAllocator local = ref this.m_AllocatorPool[index].Allocator;
        usedMemory += local.Used;
        allocatedMemory += local.Size;
      }
    }

    public void Enqueue(CreateAction action, JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.Enqueue<CreateAction>(action, Entity.Null, dependencies, uint.MaxValue, this.m_CreateActions, PathfindQueueSystem.ActionType.Create, (object) null, false, true);
    }

    public void Enqueue(UpdateAction action, JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.Enqueue<UpdateAction>(action, Entity.Null, dependencies, uint.MaxValue, this.m_UpdateActions, PathfindQueueSystem.ActionType.Update, (object) null, false, true);
    }

    public void Enqueue(DeleteAction action, JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.Enqueue<DeleteAction>(action, Entity.Null, dependencies, uint.MaxValue, this.m_DeleteActions, PathfindQueueSystem.ActionType.Delete, (object) null, false, true);
    }

    public void Enqueue(
      PathfindAction action,
      Entity owner,
      JobHandle dependencies,
      uint resultFrame,
      object system,
      bool highPriority = false)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.Enqueue<PathfindAction>(action, owner, dependencies, resultFrame, this.m_PathfindActions, PathfindQueueSystem.ActionType.Pathfind, system, highPriority, false);
    }

    public void Enqueue(
      PathfindAction action,
      Entity owner,
      JobHandle dependencies,
      uint resultFrame,
      object system,
      PathEventData eventData,
      bool highPriority = false)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.Enqueue<PathfindAction>(action, owner, dependencies, resultFrame, this.m_PathfindActions, PathfindQueueSystem.ActionType.Pathfind, system, highPriority, eventData);
    }

    public void Enqueue(
      CoverageAction action,
      Entity owner,
      JobHandle dependencies,
      uint resultFrame,
      object system,
      bool highPriority = false)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.Enqueue<CoverageAction>(action, owner, dependencies, resultFrame, this.m_CoverageActions, PathfindQueueSystem.ActionType.Coverage, system, highPriority, false);
    }

    public void Enqueue(
      CoverageAction action,
      Entity owner,
      JobHandle dependencies,
      uint resultFrame,
      object system,
      PathEventData eventData,
      bool highPriority = false)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.Enqueue<CoverageAction>(action, owner, dependencies, resultFrame, this.m_CoverageActions, PathfindQueueSystem.ActionType.Coverage, system, highPriority, eventData);
    }

    public void Enqueue(
      AvailabilityAction action,
      Entity owner,
      JobHandle dependencies,
      uint resultFrame,
      object system)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.Enqueue<AvailabilityAction>(action, owner, dependencies, resultFrame, this.m_AvailabilityActions, PathfindQueueSystem.ActionType.Availability, system, false, false);
    }

    public void Enqueue(DensityAction action, JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.Enqueue<DensityAction>(action, Entity.Null, dependencies, uint.MaxValue, this.m_DensityActions, PathfindQueueSystem.ActionType.Density, (object) null, false, true);
    }

    public void Enqueue(TimeAction action, JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.Enqueue<TimeAction>(action, Entity.Null, dependencies, uint.MaxValue, this.m_TimeActions, PathfindQueueSystem.ActionType.Time, (object) null, false, true);
    }

    public void Enqueue(FlowAction action, JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.Enqueue<FlowAction>(action, Entity.Null, dependencies, uint.MaxValue, this.m_FlowActions, PathfindQueueSystem.ActionType.Flow, (object) null, false, true);
    }

    public PathfindQueueSystem.ActionList<CreateAction> GetCreateActions() => this.m_CreateActions;

    public PathfindQueueSystem.ActionList<UpdateAction> GetUpdateActions() => this.m_UpdateActions;

    public PathfindQueueSystem.ActionList<DeleteAction> GetDeleteActions() => this.m_DeleteActions;

    public PathfindQueueSystem.ActionList<PathfindAction> GetPathfindActions()
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_PathfindActions;
    }

    public PathfindQueueSystem.ActionList<CoverageAction> GetCoverageActions()
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_CoverageActions;
    }

    public PathfindQueueSystem.ActionList<AvailabilityAction> GetAvailabilityActions()
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_AvailabilityActions;
    }

    public PathfindQueueSystem.ActionList<DensityAction> GetDensityActions()
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_DensityActions;
    }

    public PathfindQueueSystem.ActionList<TimeAction> GetTimeActions() => this.m_TimeActions;

    public PathfindQueueSystem.ActionList<FlowAction> GetFlowActions() => this.m_FlowActions;

    private void Enqueue<T>(
      T action,
      Entity owner,
      JobHandle dependencies,
      uint resultFrame,
      PathfindQueueSystem.ActionList<T> list,
      PathfindQueueSystem.ActionType type,
      object system,
      bool highPriority,
      bool modification)
      where T : struct, IDisposable
    {
      PathFlags flags = PathFlags.Pending;
      if (highPriority)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        list.m_Items.Insert(list.m_NextIndex + list.m_PriorityCount++, new PathfindQueueSystem.ActionListItem<T>(action, owner, dependencies, flags, resultFrame, new PathEventData(), system));
        // ISSUE: reference to a compiler-generated field
        this.m_HighPriorityTypes.Enqueue(type);
      }
      else if (modification)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        list.m_Items.Add(new PathfindQueueSystem.ActionListItem<T>(action, owner, dependencies, flags, resultFrame, new PathEventData(), system));
        // ISSUE: reference to a compiler-generated field
        this.m_ModificationTypes.Enqueue(type);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        list.m_Items.Add(new PathfindQueueSystem.ActionListItem<T>(action, owner, dependencies, flags, resultFrame, new PathEventData(), system));
        // ISSUE: reference to a compiler-generated field
        this.m_ActionTypes.Enqueue(type);
      }
    }

    private void Enqueue<T>(
      T action,
      Entity owner,
      JobHandle dependencies,
      uint resultFrame,
      PathfindQueueSystem.ActionList<T> list,
      PathfindQueueSystem.ActionType type,
      object system,
      bool highPriority,
      PathEventData eventData)
      where T : struct, IDisposable
    {
      PathFlags flags = PathFlags.Pending | PathFlags.WantsEvent;
      if (highPriority)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        list.m_Items.Insert(list.m_NextIndex + list.m_PriorityCount++, new PathfindQueueSystem.ActionListItem<T>(action, owner, dependencies, flags, resultFrame, eventData, system));
        // ISSUE: reference to a compiler-generated field
        this.m_HighPriorityTypes.Enqueue(type);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        list.m_Items.Add(new PathfindQueueSystem.ActionListItem<T>(action, owner, dependencies, flags, resultFrame, eventData, system));
        // ISSUE: reference to a compiler-generated field
        this.m_ActionTypes.Enqueue(type);
      }
    }

    private JobHandle ScheduleModificationJob<T>(T job) where T : struct, IJob, ModificationJobs.IPathfindModificationJob
    {
      JobHandle job0 = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_WorkerData.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindQueueSystem.WorkerData workerData = this.m_WorkerData[index];
        // ISSUE: reference to a compiler-generated field
        job.SetPathfindData(workerData.m_PathfindData);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        workerData.m_WriteHandle = IJobExtensions.Schedule<T>(job, JobHandle.CombineDependencies(workerData.m_WriteHandle, workerData.m_ReadHandle));
        // ISSUE: reference to a compiler-generated field
        workerData.m_ReadHandle = new JobHandle();
        // ISSUE: reference to a compiler-generated field
        job0 = JobHandle.CombineDependencies(job0, workerData.m_WriteHandle);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_LastWorkerIndex == this.m_NextWorkerIndex && ++this.m_NextWorkerIndex >= this.m_WorkerData.Count)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_NextWorkerIndex = 0;
      }
      return job0;
    }

    private void ScheduleWorkerJobs(
      ref PathfindQueueSystem.WorkerActions currentActions)
    {
      if (currentActions == null)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      PathfindQueueSystem.WorkerData workerData = this.m_WorkerData[this.m_NextWorkerIndex];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int num1 = math.min(currentActions.m_Actions.Length, math.max(this.m_MaxThreadCount, currentActions.m_HighPriorityCount));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int num2 = this.m_MaxThreadCount + currentActions.m_HighPriorityCount;
      // ISSUE: reference to a compiler-generated field
      int count = this.m_ThreadData.Count;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PathfindQueueSystem.PathfindWorkerJob jobData = new PathfindQueueSystem.PathfindWorkerJob()
      {
        m_RandomSeed = RandomSeed.Next(),
        m_PathfindData = workerData.m_PathfindData,
        m_PathfindHeuristicData = this.m_NetInitializeSystem.GetHeuristicData(),
        m_Actions = currentActions.m_Actions.AsArray(),
        m_ActionIndex = currentActions.m_ActionIndex
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TransportLineSystem.GetMaxTransportSpeed(out jobData.m_MaxPassengerTransportSpeed, out jobData.m_MaxCargoTransportSpeed);
      for (int index = 0; index < num1; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        JobHandle jobHandle = workerData.m_WriteHandle;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        PathfindQueueSystem.ThreadData threadData = new PathfindQueueSystem.ThreadData();
        // ISSUE: reference to a compiler-generated field
        if (this.m_ThreadData.Count >= num2)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_DependencyIndex >= count)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_DependencyIndex = 0;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          threadData = this.m_ThreadData[this.m_DependencyIndex];
          // ISSUE: reference to a compiler-generated field
          jobHandle = JobHandle.CombineDependencies(jobHandle, threadData.m_JobHandle);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_AllocatorPool.Count != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            threadData.m_Allocator = this.m_AllocatorPool[this.m_AllocatorPool.Count - 1];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_AllocatorPool.RemoveAt(this.m_AllocatorPool.Count - 1);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            threadData.m_Allocator = new AllocatorHelper<UnsafeLinearAllocator>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
            // ISSUE: reference to a compiler-generated field
            threadData.m_Allocator.Allocator.Initialize(1048576U);
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_Allocator = threadData.m_Allocator;
        // ISSUE: reference to a compiler-generated field
        threadData.m_JobHandle = jobData.Schedule<PathfindQueueSystem.PathfindWorkerJob>(jobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        currentActions.m_ReadHandle = JobHandle.CombineDependencies(currentActions.m_ReadHandle, threadData.m_JobHandle);
        // ISSUE: reference to a compiler-generated field
        if (this.m_ThreadData.Count >= num2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ThreadData[this.m_DependencyIndex++] = threadData;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ThreadData.Add(threadData);
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      workerData.m_ReadHandle = JobHandle.CombineDependencies(workerData.m_ReadHandle, currentActions.m_ReadHandle);
      currentActions = (PathfindQueueSystem.WorkerActions) null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastWorkerIndex = this.m_NextWorkerIndex;
    }

    private void RequireWorkerActions(
      ref PathfindQueueSystem.WorkerActions currentActions)
    {
      if (currentActions != null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_WorkerActionPool.TryDequeue(ref currentActions))
      {
        // ISSUE: object of a compiler-generated type is created
        currentActions = new PathfindQueueSystem.WorkerActions(Allocator.Persistent);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_WorkerActions.Enqueue(currentActions);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      bool requireDebug = this.m_RequireDebug;
      // ISSUE: reference to a compiler-generated field
      this.m_RequireDebug = false;
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_AllocatorPool.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AllocatorPool[index].Allocator.Rewind(true);
      }
      int index1 = 0;
      // ISSUE: reference to a compiler-generated field
      for (int index2 = 0; index2 < this.m_ThreadData.Count; ++index2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindQueueSystem.ThreadData threadData = this.m_ThreadData[index2];
        // ISSUE: reference to a compiler-generated field
        if (threadData.m_JobHandle.IsCompleted)
        {
          // ISSUE: reference to a compiler-generated field
          threadData.m_JobHandle.Complete();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_AllocatorPool.Add(threadData.m_Allocator);
          // ISSUE: reference to a compiler-generated field
          if (index1 < this.m_DependencyIndex)
          {
            // ISSUE: reference to a compiler-generated field
            --this.m_DependencyIndex;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ThreadData[index1++] = threadData;
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (index1 < this.m_ThreadData.Count)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ThreadData.RemoveRange(index1, this.m_ThreadData.Count - index1);
      }
      // ISSUE: reference to a compiler-generated field
      for (int index3 = 0; index3 < this.m_WorkerData.Count; ++index3)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindQueueSystem.WorkerData workerData = this.m_WorkerData[index3];
        // ISSUE: reference to a compiler-generated field
        if (workerData.m_WriteHandle.IsCompleted)
        {
          // ISSUE: reference to a compiler-generated field
          workerData.m_WriteHandle.Complete();
        }
        // ISSUE: reference to a compiler-generated field
        if (workerData.m_ReadHandle.IsCompleted)
        {
          // ISSUE: reference to a compiler-generated field
          workerData.m_ReadHandle.Complete();
        }
      }
      // ISSUE: variable of a compiler-generated type
      PathfindQueueSystem.WorkerActions workerActions;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      while (this.m_WorkerActions.TryPeek(ref workerActions) && workerActions.m_ReadHandle.IsCompleted)
      {
        // ISSUE: reference to a compiler-generated method
        workerActions.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_WorkerActions.Dequeue();
        // ISSUE: reference to a compiler-generated field
        this.m_WorkerActionPool.Enqueue(workerActions);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.CompleteSetup();
      // ISSUE: variable of a compiler-generated type
      PathfindQueueSystem.WorkerActions currentActions = (PathfindQueueSystem.WorkerActions) null;
      try
      {
        while (true)
        {
          // ISSUE: variable of a compiler-generated type
          PathfindQueueSystem.ActionType type;
          bool isHighPriority;
          bool flag;
          // ISSUE: reference to a compiler-generated field
          if (this.m_HighPriorityTypes.Count != 0)
          {
            // ISSUE: reference to a compiler-generated field
            type = this.m_HighPriorityTypes.Peek();
            isHighPriority = true;
            flag = false;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_ModificationTypes.Count != 0)
            {
              // ISSUE: reference to a compiler-generated field
              type = this.m_ModificationTypes.Peek();
              isHighPriority = false;
              flag = true;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_ActionTypes.Count != 0)
              {
                // ISSUE: reference to a compiler-generated field
                type = this.m_ActionTypes.Peek();
                isHighPriority = false;
                flag = false;
              }
              else
                break;
            }
          }
          switch (type)
          {
            case PathfindQueueSystem.ActionType.Create:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              PathfindQueueSystem.ActionListItem<CreateAction> actionListItem1 = this.m_CreateActions.m_Items[this.m_CreateActions.m_NextIndex];
              // ISSUE: reference to a compiler-generated field
              if (actionListItem1.m_Dependencies.IsCompleted)
              {
                // ISSUE: reference to a compiler-generated field
                actionListItem1.m_Dependencies.Complete();
                // ISSUE: reference to a compiler-generated method
                this.ScheduleWorkerJobs(ref currentActions);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                actionListItem1.m_Dependencies = this.ScheduleModificationJob<ModificationJobs.CreateEdgesJob>(new ModificationJobs.CreateEdgesJob()
                {
                  m_Action = actionListItem1.m_Action
                });
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                actionListItem1.m_Flags = actionListItem1.m_Flags & ~PathFlags.Pending | PathFlags.Scheduled;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CreateActions.m_Items[this.m_CreateActions.m_NextIndex++] = actionListItem1;
                break;
              }
              goto label_64;
            case PathfindQueueSystem.ActionType.Update:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              PathfindQueueSystem.ActionListItem<UpdateAction> actionListItem2 = this.m_UpdateActions.m_Items[this.m_UpdateActions.m_NextIndex];
              // ISSUE: reference to a compiler-generated field
              if (actionListItem2.m_Dependencies.IsCompleted)
              {
                // ISSUE: reference to a compiler-generated field
                actionListItem2.m_Dependencies.Complete();
                // ISSUE: reference to a compiler-generated method
                this.ScheduleWorkerJobs(ref currentActions);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                actionListItem2.m_Dependencies = this.ScheduleModificationJob<ModificationJobs.UpdateEdgesJob>(new ModificationJobs.UpdateEdgesJob()
                {
                  m_Action = actionListItem2.m_Action
                });
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                actionListItem2.m_Flags = actionListItem2.m_Flags & ~PathFlags.Pending | PathFlags.Scheduled;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_UpdateActions.m_Items[this.m_UpdateActions.m_NextIndex++] = actionListItem2;
                break;
              }
              goto label_57;
            case PathfindQueueSystem.ActionType.Delete:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              PathfindQueueSystem.ActionListItem<DeleteAction> actionListItem3 = this.m_DeleteActions.m_Items[this.m_DeleteActions.m_NextIndex];
              // ISSUE: reference to a compiler-generated field
              if (actionListItem3.m_Dependencies.IsCompleted)
              {
                // ISSUE: reference to a compiler-generated field
                actionListItem3.m_Dependencies.Complete();
                // ISSUE: reference to a compiler-generated method
                this.ScheduleWorkerJobs(ref currentActions);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                actionListItem3.m_Dependencies = this.ScheduleModificationJob<ModificationJobs.DeleteEdgesJob>(new ModificationJobs.DeleteEdgesJob()
                {
                  m_Action = actionListItem3.m_Action
                });
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                actionListItem3.m_Flags = actionListItem3.m_Flags & ~PathFlags.Pending | PathFlags.Scheduled;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_DeleteActions.m_Items[this.m_DeleteActions.m_NextIndex++] = actionListItem3;
                break;
              }
              goto label_54;
            case PathfindQueueSystem.ActionType.Pathfind:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              PathfindQueueSystem.ActionListItem<PathfindAction> actionListItem4 = this.m_PathfindActions.m_Items[this.m_PathfindActions.m_NextIndex];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (actionListItem4.m_Dependencies.IsCompleted && (!requireDebug || (actionListItem4.m_Flags & PathFlags.Debug) != (PathFlags) 0))
              {
                // ISSUE: reference to a compiler-generated field
                actionListItem4.m_Dependencies.Complete();
                // ISSUE: reference to a compiler-generated method
                this.RequireWorkerActions(ref currentActions);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                currentActions.Add<PathfindActionData>(type, isHighPriority, ref actionListItem4.m_Action.data);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                actionListItem4.m_Flags = actionListItem4.m_Flags & ~PathFlags.Pending | PathFlags.Scheduled;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_PathfindActions.m_Items[this.m_PathfindActions.m_NextIndex++] = actionListItem4;
                if (isHighPriority)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  --this.m_PathfindActions.m_PriorityCount;
                  break;
                }
                break;
              }
              goto label_51;
            case PathfindQueueSystem.ActionType.Coverage:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              PathfindQueueSystem.ActionListItem<CoverageAction> actionListItem5 = this.m_CoverageActions.m_Items[this.m_CoverageActions.m_NextIndex];
              // ISSUE: reference to a compiler-generated field
              if (actionListItem5.m_Dependencies.IsCompleted)
              {
                // ISSUE: reference to a compiler-generated field
                actionListItem5.m_Dependencies.Complete();
                // ISSUE: reference to a compiler-generated method
                this.RequireWorkerActions(ref currentActions);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                currentActions.Add<CoverageActionData>(type, isHighPriority, ref actionListItem5.m_Action.data);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                actionListItem5.m_Flags = actionListItem5.m_Flags & ~PathFlags.Pending | PathFlags.Scheduled;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CoverageActions.m_Items[this.m_CoverageActions.m_NextIndex++] = actionListItem5;
                if (isHighPriority)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  --this.m_CoverageActions.m_PriorityCount;
                  break;
                }
                break;
              }
              goto label_44;
            case PathfindQueueSystem.ActionType.Availability:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              PathfindQueueSystem.ActionListItem<AvailabilityAction> actionListItem6 = this.m_AvailabilityActions.m_Items[this.m_AvailabilityActions.m_NextIndex];
              // ISSUE: reference to a compiler-generated field
              if (actionListItem6.m_Dependencies.IsCompleted)
              {
                // ISSUE: reference to a compiler-generated field
                actionListItem6.m_Dependencies.Complete();
                // ISSUE: reference to a compiler-generated method
                this.RequireWorkerActions(ref currentActions);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                currentActions.Add<AvailabilityActionData>(type, isHighPriority, ref actionListItem6.m_Action.data);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                actionListItem6.m_Flags = actionListItem6.m_Flags & ~PathFlags.Pending | PathFlags.Scheduled;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_AvailabilityActions.m_Items[this.m_AvailabilityActions.m_NextIndex++] = actionListItem6;
                if (isHighPriority)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  --this.m_CoverageActions.m_PriorityCount;
                  break;
                }
                break;
              }
              goto label_40;
            case PathfindQueueSystem.ActionType.Density:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              PathfindQueueSystem.ActionListItem<DensityAction> actionListItem7 = this.m_DensityActions.m_Items[this.m_DensityActions.m_NextIndex];
              // ISSUE: reference to a compiler-generated field
              if (actionListItem7.m_Dependencies.IsCompleted)
              {
                // ISSUE: reference to a compiler-generated field
                actionListItem7.m_Dependencies.Complete();
                // ISSUE: reference to a compiler-generated method
                this.ScheduleWorkerJobs(ref currentActions);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                actionListItem7.m_Dependencies = this.ScheduleModificationJob<ModificationJobs.SetDensityJob>(new ModificationJobs.SetDensityJob()
                {
                  m_Action = actionListItem7.m_Action
                });
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                actionListItem7.m_Flags = actionListItem7.m_Flags & ~PathFlags.Pending | PathFlags.Scheduled;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_DensityActions.m_Items[this.m_DensityActions.m_NextIndex++] = actionListItem7;
                break;
              }
              goto label_39;
            case PathfindQueueSystem.ActionType.Time:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              PathfindQueueSystem.ActionListItem<TimeAction> actionListItem8 = this.m_TimeActions.m_Items[this.m_TimeActions.m_NextIndex];
              // ISSUE: reference to a compiler-generated field
              if (actionListItem8.m_Dependencies.IsCompleted)
              {
                // ISSUE: reference to a compiler-generated field
                actionListItem8.m_Dependencies.Complete();
                // ISSUE: reference to a compiler-generated method
                this.ScheduleWorkerJobs(ref currentActions);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                actionListItem8.m_Dependencies = this.ScheduleModificationJob<ModificationJobs.SetTimeJob>(new ModificationJobs.SetTimeJob()
                {
                  m_Action = actionListItem8.m_Action
                });
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                actionListItem8.m_Flags = actionListItem8.m_Flags & ~PathFlags.Pending | PathFlags.Scheduled;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_TimeActions.m_Items[this.m_TimeActions.m_NextIndex++] = actionListItem8;
                break;
              }
              goto label_36;
            case PathfindQueueSystem.ActionType.Flow:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              PathfindQueueSystem.ActionListItem<FlowAction> actionListItem9 = this.m_FlowActions.m_Items[this.m_FlowActions.m_NextIndex];
              // ISSUE: reference to a compiler-generated field
              if (actionListItem9.m_Dependencies.IsCompleted)
              {
                // ISSUE: reference to a compiler-generated field
                actionListItem9.m_Dependencies.Complete();
                // ISSUE: reference to a compiler-generated method
                this.ScheduleWorkerJobs(ref currentActions);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                actionListItem9.m_Dependencies = this.ScheduleModificationJob<ModificationJobs.SetFlowJob>(new ModificationJobs.SetFlowJob()
                {
                  m_Action = actionListItem9.m_Action
                });
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                actionListItem9.m_Flags = actionListItem9.m_Flags & ~PathFlags.Pending | PathFlags.Scheduled;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_FlowActions.m_Items[this.m_FlowActions.m_NextIndex++] = actionListItem9;
                break;
              }
              goto label_33;
          }
          if (isHighPriority)
          {
            // ISSUE: reference to a compiler-generated field
            int num1 = (int) this.m_HighPriorityTypes.Dequeue();
          }
          else if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            int num2 = (int) this.m_ModificationTypes.Dequeue();
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            int num3 = (int) this.m_ActionTypes.Dequeue();
          }
        }
        return;
label_64:
        return;
label_57:
        return;
label_54:
        return;
label_51:
        return;
label_44:
        return;
label_40:
        return;
label_39:
        return;
label_36:
        return;
label_33:;
      }
      finally
      {
        // ISSUE: reference to a compiler-generated method
        this.ScheduleWorkerJobs(ref currentActions);
      }
    }

    [UnityEngine.Scripting.Preserve]
    public PathfindQueueSystem()
    {
    }

    public struct ActionListItem<T> : IDisposable where T : struct, IDisposable
    {
      public T m_Action;
      public Entity m_Owner;
      public JobHandle m_Dependencies;
      public PathFlags m_Flags;
      public uint m_ResultFrame;
      public PathEventData m_EventData;
      public object m_System;

      public ActionListItem(
        T action,
        Entity owner,
        JobHandle dependencies,
        PathFlags flags,
        uint frameIndex,
        PathEventData eventData,
        object system)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Action = action;
        // ISSUE: reference to a compiler-generated field
        this.m_Owner = owner;
        // ISSUE: reference to a compiler-generated field
        this.m_Dependencies = dependencies;
        // ISSUE: reference to a compiler-generated field
        this.m_Flags = flags;
        // ISSUE: reference to a compiler-generated field
        this.m_ResultFrame = frameIndex;
        // ISSUE: reference to a compiler-generated field
        this.m_EventData = eventData;
        // ISSUE: reference to a compiler-generated field
        this.m_System = system;
      }

      public void Dispose()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Dependencies.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_Action.Dispose();
      }
    }

    public class ActionList<T> : IDisposable where T : struct, IDisposable
    {
      public List<PathfindQueueSystem.ActionListItem<T>> m_Items;
      public int m_NextIndex;
      public int m_PriorityCount;

      public ActionList() => this.m_Items = new List<PathfindQueueSystem.ActionListItem<T>>();

      public void Clear()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Items.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          PathfindQueueSystem.ActionListItem<T> actionListItem = this.m_Items[index];
          // ISSUE: reference to a compiler-generated method
          actionListItem.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Items.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_NextIndex = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_PriorityCount = 0;
      }

      public void Dispose()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Items.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          PathfindQueueSystem.ActionListItem<T> actionListItem = this.m_Items[index];
          // ISSUE: reference to a compiler-generated method
          actionListItem.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Items = (List<PathfindQueueSystem.ActionListItem<T>>) null;
      }
    }

    public enum ActionType
    {
      Create,
      Update,
      Delete,
      Pathfind,
      Coverage,
      Availability,
      Density,
      Time,
      Flow,
    }

    private class WorkerData : IDisposable
    {
      public NativePathfindData m_PathfindData;
      public JobHandle m_WriteHandle;
      public JobHandle m_ReadHandle;

      public WorkerData(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PathfindData = new NativePathfindData(allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_WriteHandle = new JobHandle();
        // ISSUE: reference to a compiler-generated field
        this.m_ReadHandle = new JobHandle();
      }

      public void Clear()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_WriteHandle.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_ReadHandle.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_PathfindData.Clear();
      }

      public void Dispose()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_WriteHandle.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_ReadHandle.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_PathfindData.Dispose();
      }
    }

    private class WorkerActions : IDisposable
    {
      public NativeList<PathfindQueueSystem.WorkerAction> m_Actions;
      public NativeReference<int> m_ActionIndex;
      public JobHandle m_ReadHandle;
      public int m_HighPriorityCount;

      public WorkerActions(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Actions = new NativeList<PathfindQueueSystem.WorkerAction>(100, (AllocatorManager.AllocatorHandle) allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_ActionIndex = new NativeReference<int>(0, (AllocatorManager.AllocatorHandle) allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_ReadHandle = new JobHandle();
        // ISSUE: reference to a compiler-generated field
        this.m_HighPriorityCount = 0;
      }

      public unsafe void Add<T>(
        PathfindQueueSystem.ActionType type,
        bool isHighPriority,
        ref T data)
        where T : struct
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_Actions.Add(new PathfindQueueSystem.WorkerAction()
        {
          m_Type = type,
          m_ActionData = UnsafeUtility.AddressOf<T>(ref data)
        });
        if (!isHighPriority)
          return;
        // ISSUE: reference to a compiler-generated field
        ++this.m_HighPriorityCount;
      }

      public void Clear()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ReadHandle.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_Actions.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_ActionIndex.Value = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_HighPriorityCount = 0;
      }

      public void Dispose()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ReadHandle.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_Actions.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_ActionIndex.Dispose();
      }
    }

    private struct ThreadData
    {
      public JobHandle m_JobHandle;
      public AllocatorHelper<UnsafeLinearAllocator> m_Allocator;
    }

    public struct WorkerAction
    {
      public PathfindQueueSystem.ActionType m_Type;
      public unsafe void* m_ActionData;
    }

    [BurstCompile]
    public struct PathfindWorkerJob : IJob
    {
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public NativePathfindData m_PathfindData;
      [ReadOnly]
      public PathfindHeuristicData m_PathfindHeuristicData;
      [ReadOnly]
      public float m_MaxPassengerTransportSpeed;
      [ReadOnly]
      public float m_MaxCargoTransportSpeed;
      [ReadOnly]
      public NativeArray<PathfindQueueSystem.WorkerAction> m_Actions;
      [NativeDisableContainerSafetyRestriction]
      public NativeReference<int> m_ActionIndex;
      [NativeDisableUnsafePtrRestriction]
      public AllocatorHelper<UnsafeLinearAllocator> m_Allocator;

      public unsafe void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        ref int local1 = ref this.m_ActionIndex.ValueAsRef<int>();
        // ISSUE: reference to a compiler-generated field
        ref UnsafeLinearAllocator local2 = ref this.m_Allocator.Allocator;
        Allocator toAllocator = local2.Handle.ToAllocator;
        while (true)
        {
          int index = Interlocked.Increment(ref local1) - 1;
          // ISSUE: reference to a compiler-generated field
          if (index < this.m_Actions.Length)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            PathfindQueueSystem.WorkerAction action = this.m_Actions[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            PathfindQueueSystem.ActionType type = action.m_Type;
            switch (type)
            {
              case PathfindQueueSystem.ActionType.Pathfind:
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.Execute(ref UnsafeUtility.AsRef<PathfindActionData>(action.m_ActionData), index, toAllocator);
                break;
              case PathfindQueueSystem.ActionType.Coverage:
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.Execute(ref UnsafeUtility.AsRef<CoverageActionData>(action.m_ActionData), toAllocator);
                break;
              case PathfindQueueSystem.ActionType.Availability:
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.Execute(ref UnsafeUtility.AsRef<AvailabilityActionData>(action.m_ActionData), toAllocator);
                break;
            }
            local2.Rewind();
          }
          else
            break;
        }
        local2.Rewind(true);
      }

      private void Execute(ref PathfindActionData actionData, int index, Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PathfindJobs.PathfindJob.Execute(this.m_PathfindData, allocator, this.m_RandomSeed.GetRandom(index), this.m_PathfindHeuristicData, this.m_MaxPassengerTransportSpeed, this.m_MaxCargoTransportSpeed, ref actionData);
        Interlocked.MemoryBarrier();
        actionData.m_State = PathfindActionState.Completed;
      }

      private void Execute(ref CoverageActionData actionData, Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        CoverageJobs.CoverageJob.Execute(this.m_PathfindData, allocator, ref actionData);
        Interlocked.MemoryBarrier();
        actionData.m_State = PathfindActionState.Completed;
      }

      private void Execute(ref AvailabilityActionData actionData, Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        AvailabilityJobs.AvailabilityJob.Execute(this.m_PathfindData, allocator, ref actionData);
        Interlocked.MemoryBarrier();
        actionData.m_State = PathfindActionState.Completed;
      }
    }
  }
}
