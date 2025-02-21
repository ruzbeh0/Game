// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PathfindSetupSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using Game.Pathfind;
using Game.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class PathfindSetupSystem : GameSystemBase, IPreDeserialize
  {
    private PathfindTargetSeekerData m_TargetSeekerData;
    private CommonPathfindSetup m_CommonPathfindSetup;
    private PostServicePathfindSetup m_PostServicePathfindSetup;
    private GarbagePathfindSetup m_GarbagePathfindSetup;
    private TransportPathfindSetup m_TransportPathfindSetup;
    private PolicePathfindSetup m_PolicePathfindSetup;
    private FirePathfindSetup m_FirePathfindSetup;
    private HealthcarePathfindSetup m_HealthcarePathfindSetup;
    private AreaPathfindSetup m_AreaPathfindSetup;
    private RoadPathfindSetup m_RoadPathfindSetup;
    private CitizenPathfindSetup m_CitizenPathfindSetup;
    private ResourcePathfindSetup m_ResourcePathfindSetup;
    private SimulationSystem m_SimulationSystem;
    private PathfindQueueSystem m_PathfindQueueSystem;
    private AirwaySystem m_AirwaySystem;
    private NativeList<PathfindSetupSystem.SetupListItem> m_SetupList;
    private List<PathfindSetupSystem.SetupQueue> m_ActiveQueues;
    private List<PathfindSetupSystem.SetupQueue> m_FreeQueues;
    private List<PathfindSetupSystem.ActionListItem> m_ActionList;
    private JobHandle m_QueueDependencies;
    private JobHandle m_SetupDependencies;
    private uint m_QueueSimulationFrameIndex;
    private uint m_SetupSimulationFrameIndex;
    private int m_PendingRequestCount;

    public uint pendingSimulationFrame
    {
      get => math.min(this.m_QueueSimulationFrameIndex, this.m_SetupSimulationFrameIndex);
    }

    public int pendingRequestCount => this.m_PendingRequestCount;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TargetSeekerData = new PathfindTargetSeekerData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_CommonPathfindSetup = new CommonPathfindSetup(this);
      // ISSUE: reference to a compiler-generated field
      this.m_PostServicePathfindSetup = new PostServicePathfindSetup(this);
      // ISSUE: reference to a compiler-generated field
      this.m_GarbagePathfindSetup = new GarbagePathfindSetup(this);
      // ISSUE: reference to a compiler-generated field
      this.m_TransportPathfindSetup = new TransportPathfindSetup(this);
      // ISSUE: reference to a compiler-generated field
      this.m_PolicePathfindSetup = new PolicePathfindSetup(this);
      // ISSUE: reference to a compiler-generated field
      this.m_FirePathfindSetup = new FirePathfindSetup(this);
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcarePathfindSetup = new HealthcarePathfindSetup(this);
      // ISSUE: reference to a compiler-generated field
      this.m_AreaPathfindSetup = new AreaPathfindSetup(this);
      // ISSUE: reference to a compiler-generated field
      this.m_RoadPathfindSetup = new RoadPathfindSetup(this);
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenPathfindSetup = new CitizenPathfindSetup(this);
      // ISSUE: reference to a compiler-generated field
      this.m_ResourcePathfindSetup = new ResourcePathfindSetup(this);
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindQueueSystem = this.World.GetOrCreateSystemManaged<PathfindQueueSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirwaySystem = this.World.GetOrCreateSystemManaged<AirwaySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SetupList = new NativeList<PathfindSetupSystem.SetupListItem>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveQueues = new List<PathfindSetupSystem.SetupQueue>(10);
      // ISSUE: reference to a compiler-generated field
      this.m_FreeQueues = new List<PathfindSetupSystem.SetupQueue>(10);
      // ISSUE: reference to a compiler-generated field
      this.m_ActionList = new List<PathfindSetupSystem.ActionListItem>(50);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_QueueDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_ActiveQueues.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindSetupSystem.SetupQueue activeQueue = this.m_ActiveQueues[index];
        // ISSUE: reference to a compiler-generated field
        activeQueue.m_Queue.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_FreeQueues.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindSetupSystem.SetupQueue freeQueue = this.m_FreeQueues[index];
        // ISSUE: reference to a compiler-generated field
        freeQueue.m_Queue.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveQueues.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_FreeQueues.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_SetupDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_SetupList.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindSetupSystem.SetupListItem setup = this.m_SetupList[index];
        // ISSUE: reference to a compiler-generated field
        setup.m_Buffer.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_ActionList.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindSetupSystem.ActionListItem action = this.m_ActionList[index];
        // ISSUE: reference to a compiler-generated field
        action.m_Action.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_SetupList.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ActionList.Clear();
      base.OnDestroy();
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_QueueDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_ActiveQueues.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindSetupSystem.SetupQueue activeQueue = this.m_ActiveQueues[index];
        // ISSUE: reference to a compiler-generated field
        activeQueue.m_Queue.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_FreeQueues.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindSetupSystem.SetupQueue freeQueue = this.m_FreeQueues[index];
        // ISSUE: reference to a compiler-generated field
        freeQueue.m_Queue.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveQueues.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_FreeQueues.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_SetupDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_SetupList.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindSetupSystem.SetupListItem setup = this.m_SetupList[index];
        // ISSUE: reference to a compiler-generated field
        setup.m_Buffer.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_ActionList.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindSetupSystem.ActionListItem action = this.m_ActionList[index];
        // ISSUE: reference to a compiler-generated field
        action.m_Action.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_SetupList.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_ActionList.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_QueueSimulationFrameIndex = uint.MaxValue;
      // ISSUE: reference to a compiler-generated field
      this.m_SetupSimulationFrameIndex = uint.MaxValue;
      // ISSUE: reference to a compiler-generated field
      this.m_PendingRequestCount = 0;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ActiveQueues.Count == 0)
        return;
      // ISSUE: reference to a compiler-generated method
      this.CompleteSetup();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TargetSeekerData.Update((SystemBase) this, this.m_AirwaySystem.GetAirwayData());
      // ISSUE: reference to a compiler-generated field
      this.m_QueueDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_QueueDependencies = new JobHandle();
      int index1 = 0;
      // ISSUE: reference to a compiler-generated field
      for (int index2 = 0; index2 < this.m_ActiveQueues.Count; ++index2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindSetupSystem.SetupQueue activeQueue = this.m_ActiveQueues[index2];
        int num1 = int.MaxValue;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (activeQueue.m_SpreadFrame > this.m_SimulationSystem.frameIndex)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num2 = (double) this.m_SimulationSystem.smoothSpeed == 0.0 ? 1f : (float) ((double) UnityEngine.Time.deltaTime * (double) this.m_SimulationSystem.smoothSpeed * 60.0);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num3 = (float) (activeQueue.m_SpreadFrame - this.m_SimulationSystem.frameIndex) + num2;
          // ISSUE: reference to a compiler-generated field
          num1 = (int) math.ceil((float) activeQueue.m_Queue.Count * (num2 / num3));
        }
        SetupQueueItem setupQueueItem;
        // ISSUE: reference to a compiler-generated field
        while (num1-- != 0 && activeQueue.m_Queue.TryDequeue(out setupQueueItem))
        {
          if (setupQueueItem.m_Parameters.m_ParkingTarget != Entity.Null && this.EntityManager.HasComponent<ConnectionLane>(setupQueueItem.m_Parameters.m_ParkingTarget))
            setupQueueItem.m_Parameters.m_ParkingDelta = -1f;
          PathfindAction action = new PathfindAction(0, 0, Allocator.Persistent, setupQueueItem.m_Parameters, setupQueueItem.m_Origin.m_Type, setupQueueItem.m_Destination.m_Type);
          // ISSUE: reference to a compiler-generated field
          ref NativeList<PathfindSetupSystem.SetupListItem> local1 = ref this.m_SetupList;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          PathfindSetupSystem.SetupListItem setupListItem1 = new PathfindSetupSystem.SetupListItem(setupQueueItem.m_Origin, setupQueueItem.m_Parameters, setupQueueItem.m_Owner, RandomSeed.Next(), this.m_ActionList.Count, true);
          ref PathfindSetupSystem.SetupListItem local2 = ref setupListItem1;
          local1.Add(in local2);
          // ISSUE: reference to a compiler-generated field
          ref NativeList<PathfindSetupSystem.SetupListItem> local3 = ref this.m_SetupList;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          PathfindSetupSystem.SetupListItem setupListItem2 = new PathfindSetupSystem.SetupListItem(setupQueueItem.m_Destination, setupQueueItem.m_Parameters, setupQueueItem.m_Owner, RandomSeed.Next(), this.m_ActionList.Count, false);
          ref PathfindSetupSystem.SetupListItem local4 = ref setupListItem2;
          local3.Add(in local4);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ActionList.Add(new PathfindSetupSystem.ActionListItem(action, setupQueueItem.m_Owner, activeQueue.m_ResultFrame, activeQueue.m_System));
        }
        // ISSUE: reference to a compiler-generated field
        if (activeQueue.m_Queue.IsEmpty())
        {
          // ISSUE: reference to a compiler-generated field
          this.m_FreeQueues.Add(activeQueue);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ActiveQueues[index1++] = activeQueue;
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ActiveQueues.Count > index1)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveQueues.RemoveRange(index1, this.m_ActiveQueues.Count - index1);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_SetupList.Length == 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_QueueSimulationFrameIndex = uint.MaxValue;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SetupList.Sort<PathfindSetupSystem.SetupListItem>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SetupSimulationFrameIndex = this.m_QueueSimulationFrameIndex;
        // ISSUE: reference to a compiler-generated field
        this.m_QueueSimulationFrameIndex = uint.MaxValue;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PendingRequestCount = this.m_ActionList.Count;
        int num4 = 0;
        int num5 = 1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        SetupTargetType setupTargetType = this.m_SetupList[num4].m_Target.m_Type;
        // ISSUE: reference to a compiler-generated field
        for (; num5 < this.m_SetupList.Length; ++num5)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          SetupTargetType type = this.m_SetupList[num5].m_Target.m_Type;
          if (setupTargetType != type)
          {
            // ISSUE: reference to a compiler-generated method
            this.FindTargets(num4, num5);
            num4 = num5;
            setupTargetType = type;
          }
        }
        // ISSUE: reference to a compiler-generated method
        this.FindTargets(num4, num5);
      }
    }

    public void CompleteSetup()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SetupSimulationFrameIndex = uint.MaxValue;
      // ISSUE: reference to a compiler-generated field
      this.m_PendingRequestCount = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_SetupDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_SetupDependencies = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_SetupList.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindSetupSystem.SetupListItem setup = this.m_SetupList[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindSetupSystem.ActionListItem action = this.m_ActionList[setup.m_ActionIndex];
        // ISSUE: reference to a compiler-generated field
        if (setup.m_ActionStart)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          action.m_Action.data.m_StartTargets = setup.m_Buffer;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          action.m_Action.data.m_EndTargets = setup.m_Buffer;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ActionList[setup.m_ActionIndex] = action;
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_ActionList.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindSetupSystem.ActionListItem action = this.m_ActionList[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PathfindQueueSystem.Enqueue(action.m_Action, action.m_Owner, this.m_SetupDependencies, action.m_ResultFrame, action.m_System);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_SetupList.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_ActionList.Clear();
    }

    public NativeQueue<SetupQueueItem> GetQueue(
      object system,
      int maxDelayFrames,
      int spreadFrames = 0)
    {
      // ISSUE: variable of a compiler-generated type
      PathfindSetupSystem.SetupQueue setupQueue;
      // ISSUE: reference to a compiler-generated field
      if (this.m_FreeQueues.Count != 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        setupQueue = this.m_FreeQueues[this.m_FreeQueues.Count - 1];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_FreeQueues.RemoveAt(this.m_FreeQueues.Count - 1);
      }
      else
      {
        // ISSUE: object of a compiler-generated type is created
        setupQueue = new PathfindSetupSystem.SetupQueue();
        // ISSUE: reference to a compiler-generated field
        setupQueue.m_Queue = new NativeQueue<SetupQueueItem>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      setupQueue.m_ResultFrame = this.m_SimulationSystem.frameIndex + (uint) maxDelayFrames;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      setupQueue.m_SpreadFrame = this.m_SimulationSystem.frameIndex + (uint) spreadFrames;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (setupQueue.m_ResultFrame < this.m_SimulationSystem.frameIndex)
      {
        // ISSUE: reference to a compiler-generated field
        setupQueue.m_ResultFrame = uint.MaxValue;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_QueueSimulationFrameIndex = math.min(this.m_QueueSimulationFrameIndex, setupQueue.m_ResultFrame);
      // ISSUE: reference to a compiler-generated field
      setupQueue.m_System = system;
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveQueues.Add(setupQueue);
      // ISSUE: reference to a compiler-generated field
      return setupQueue.m_Queue;
    }

    public void AddQueueWriter(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_QueueDependencies = JobHandle.CombineDependencies(this.m_QueueDependencies, handle);
    }

    public EntityQuery GetSetupQuery(params EntityQueryDesc[] entityQueryDesc)
    {
      return this.GetEntityQuery(entityQueryDesc);
    }

    public EntityQuery GetSetupQuery(params ComponentType[] componentTypes)
    {
      return this.GetEntityQuery(componentTypes);
    }

    private unsafe void FindTargets(int startIndex, int endIndex)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      PathfindSetupSystem.SetupListItem setup = this.m_SetupList[startIndex];
      NativeQueue<PathfindSetupTarget> nativeQueue = new NativeQueue<PathfindSetupTarget>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PathfindSetupSystem.SetupData setupData = new PathfindSetupSystem.SetupData(startIndex, endIndex, this.m_SetupList, this.m_TargetSeekerData, nativeQueue.AsParallelWriter());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PathfindSetupSystem.DequePathTargetsJob jobData = new PathfindSetupSystem.DequePathTargetsJob()
      {
        m_SetupItems = this.m_SetupList.GetUnsafeReadOnlyPtr<PathfindSetupSystem.SetupListItem>() + startIndex,
        m_TargetQueue = nativeQueue
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      JobHandle targets = this.FindTargets(setup.m_Target.m_Type, in setupData);
      JobHandle dependsOn = targets;
      JobHandle jobHandle = jobData.Schedule<PathfindSetupSystem.DequePathTargetsJob>(dependsOn);
      this.Dependency = JobHandle.CombineDependencies(this.Dependency, targets);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SetupDependencies = JobHandle.CombineDependencies(this.m_SetupDependencies, jobHandle);
      nativeQueue.Dispose(jobHandle);
    }

    private JobHandle FindTargets(
      SetupTargetType targetType,
      in PathfindSetupSystem.SetupData setupData)
    {
      switch (targetType)
      {
        case SetupTargetType.CurrentLocation:
          // ISSUE: reference to a compiler-generated field
          return this.m_CommonPathfindSetup.SetupCurrentLocation(this, setupData, this.Dependency);
        case SetupTargetType.ResourceSeller:
          // ISSUE: reference to a compiler-generated field
          return this.m_ResourcePathfindSetup.SetupResourceSeller(this, setupData, this.Dependency);
        case SetupTargetType.RouteWaypoints:
          // ISSUE: reference to a compiler-generated field
          return this.m_TransportPathfindSetup.SetupRouteWaypoints(this, setupData, this.Dependency);
        case SetupTargetType.TransportVehicle:
          // ISSUE: reference to a compiler-generated field
          return this.m_TransportPathfindSetup.SetupTransportVehicle(this, setupData, this.Dependency);
        case SetupTargetType.GarbageCollector:
          // ISSUE: reference to a compiler-generated field
          return this.m_GarbagePathfindSetup.SetupGarbageCollector(this, setupData, this.Dependency);
        case SetupTargetType.RandomTraffic:
          // ISSUE: reference to a compiler-generated field
          return this.m_RoadPathfindSetup.SetupRandomTraffic(this, setupData, this.Dependency);
        case SetupTargetType.JobSeekerTo:
          // ISSUE: reference to a compiler-generated field
          return this.m_CitizenPathfindSetup.SetupJobSeekerTo(this, setupData, this.Dependency);
        case SetupTargetType.SchoolSeekerTo:
          // ISSUE: reference to a compiler-generated field
          return this.m_CitizenPathfindSetup.SetupSchoolSeekerTo(this, setupData, this.Dependency);
        case SetupTargetType.FireEngine:
          // ISSUE: reference to a compiler-generated field
          return this.m_FirePathfindSetup.SetupFireEngines(this, setupData, this.Dependency);
        case SetupTargetType.PolicePatrol:
          // ISSUE: reference to a compiler-generated field
          return this.m_PolicePathfindSetup.SetupPolicePatrols(this, setupData, this.Dependency);
        case SetupTargetType.Leisure:
          // ISSUE: reference to a compiler-generated field
          return this.m_CitizenPathfindSetup.SetupLeisureTarget(this, setupData, this.Dependency);
        case SetupTargetType.Taxi:
          // ISSUE: reference to a compiler-generated field
          return this.m_TransportPathfindSetup.SetupTaxi(this, setupData, this.Dependency);
        case SetupTargetType.ResourceExport:
          // ISSUE: reference to a compiler-generated field
          return this.m_ResourcePathfindSetup.SetupResourceExport(this, setupData, this.Dependency);
        case SetupTargetType.Ambulance:
          // ISSUE: reference to a compiler-generated field
          return this.m_HealthcarePathfindSetup.SetupAmbulances(this, setupData, this.Dependency);
        case SetupTargetType.StorageTransfer:
          // ISSUE: reference to a compiler-generated field
          return this.m_ResourcePathfindSetup.SetupStorageTransfer(this, setupData, this.Dependency);
        case SetupTargetType.Maintenance:
          // ISSUE: reference to a compiler-generated field
          return this.m_RoadPathfindSetup.SetupMaintenanceProviders(this, setupData, this.Dependency);
        case SetupTargetType.PostVan:
          // ISSUE: reference to a compiler-generated field
          return this.m_PostServicePathfindSetup.SetupPostVans(this, setupData, this.Dependency);
        case SetupTargetType.MailTransfer:
          // ISSUE: reference to a compiler-generated field
          return this.m_PostServicePathfindSetup.SetupMailTransfer(this, setupData, this.Dependency);
        case SetupTargetType.MailBox:
          // ISSUE: reference to a compiler-generated field
          return this.m_PostServicePathfindSetup.SetupMailBoxes(this, setupData, this.Dependency);
        case SetupTargetType.OutsideConnection:
          // ISSUE: reference to a compiler-generated field
          return this.m_RoadPathfindSetup.SetupOutsideConnections(this, setupData, this.Dependency);
        case SetupTargetType.AccidentLocation:
          // ISSUE: reference to a compiler-generated field
          return this.m_CommonPathfindSetup.SetupAccidentLocation(this, setupData, this.Dependency);
        case SetupTargetType.Hospital:
          // ISSUE: reference to a compiler-generated field
          return this.m_HealthcarePathfindSetup.SetupHospitals(this, setupData, this.Dependency);
        case SetupTargetType.Safety:
          // ISSUE: reference to a compiler-generated field
          return this.m_CommonPathfindSetup.SetupSafety(this, setupData, this.Dependency);
        case SetupTargetType.EmergencyShelter:
          // ISSUE: reference to a compiler-generated field
          return this.m_FirePathfindSetup.SetupEmergencyShelters(this, setupData, this.Dependency);
        case SetupTargetType.EvacuationTransport:
          // ISSUE: reference to a compiler-generated field
          return this.m_FirePathfindSetup.SetupEvacuationTransport(this, setupData, this.Dependency);
        case SetupTargetType.Hearse:
          // ISSUE: reference to a compiler-generated field
          return this.m_HealthcarePathfindSetup.SetupHearses(this, setupData, this.Dependency);
        case SetupTargetType.CrimeProducer:
          // ISSUE: reference to a compiler-generated field
          return this.m_PolicePathfindSetup.SetupCrimeProducer(this, setupData, this.Dependency);
        case SetupTargetType.PrisonerTransport:
          // ISSUE: reference to a compiler-generated field
          return this.m_PolicePathfindSetup.SetupPrisonerTransport(this, setupData, this.Dependency);
        case SetupTargetType.WoodResource:
          // ISSUE: reference to a compiler-generated field
          return this.m_AreaPathfindSetup.SetupWoodResource(this, setupData, this.Dependency);
        case SetupTargetType.AreaLocation:
          // ISSUE: reference to a compiler-generated field
          return this.m_AreaPathfindSetup.SetupAreaLocation(this, setupData, this.Dependency);
        case SetupTargetType.Sightseeing:
          return new JobHandle();
        case SetupTargetType.Attraction:
          // ISSUE: reference to a compiler-generated field
          return this.m_CitizenPathfindSetup.SetupAttraction(this, setupData, this.Dependency);
        case SetupTargetType.GarbageTransfer:
          // ISSUE: reference to a compiler-generated field
          return this.m_GarbagePathfindSetup.SetupGarbageTransfer(this, setupData, this.Dependency);
        case SetupTargetType.HomelessShelter:
          // ISSUE: reference to a compiler-generated field
          return this.m_CitizenPathfindSetup.SetupHomeless(this, setupData, this.Dependency);
        case SetupTargetType.FindHome:
          // ISSUE: reference to a compiler-generated field
          return this.m_CitizenPathfindSetup.SetupFindHome(this, setupData, this.Dependency);
        case SetupTargetType.TransportVehicleRequest:
          // ISSUE: reference to a compiler-generated field
          return this.m_TransportPathfindSetup.SetupTransportVehicleRequest(this, setupData, this.Dependency);
        case SetupTargetType.TaxiRequest:
          // ISSUE: reference to a compiler-generated field
          return this.m_TransportPathfindSetup.SetupTaxiRequest(this, setupData, this.Dependency);
        case SetupTargetType.PrisonerTransportRequest:
          // ISSUE: reference to a compiler-generated field
          return this.m_PolicePathfindSetup.SetupPrisonerTransportRequest(this, setupData, this.Dependency);
        case SetupTargetType.EvacuationRequest:
          // ISSUE: reference to a compiler-generated field
          return this.m_FirePathfindSetup.SetupEvacuationRequest(this, setupData, this.Dependency);
        case SetupTargetType.GarbageCollectorRequest:
          // ISSUE: reference to a compiler-generated field
          return this.m_GarbagePathfindSetup.SetupGarbageCollectorRequest(this, setupData, this.Dependency);
        case SetupTargetType.PoliceRequest:
          // ISSUE: reference to a compiler-generated field
          return this.m_PolicePathfindSetup.SetupPoliceRequest(this, setupData, this.Dependency);
        case SetupTargetType.FireRescueRequest:
          // ISSUE: reference to a compiler-generated field
          return this.m_FirePathfindSetup.SetupFireRescueRequest(this, setupData, this.Dependency);
        case SetupTargetType.PostVanRequest:
          // ISSUE: reference to a compiler-generated field
          return this.m_PostServicePathfindSetup.SetupPostVanRequest(this, setupData, this.Dependency);
        case SetupTargetType.MaintenanceRequest:
          // ISSUE: reference to a compiler-generated field
          return this.m_RoadPathfindSetup.SetupMaintenanceRequest(this, setupData, this.Dependency);
        case SetupTargetType.HealthcareRequest:
          // ISSUE: reference to a compiler-generated field
          return this.m_HealthcarePathfindSetup.SetupHealthcareRequest(this, setupData, this.Dependency);
        case SetupTargetType.TouristFindTarget:
          // ISSUE: reference to a compiler-generated field
          return this.m_CitizenPathfindSetup.SetupTouristTarget(this, setupData, this.Dependency);
        default:
          Debug.LogWarning((object) ("Invalid target type in Pathfind setup " + targetType.ToString()));
          return new JobHandle();
      }
    }

    [UnityEngine.Scripting.Preserve]
    public PathfindSetupSystem()
    {
    }

    public struct SetupData
    {
      [ReadOnly]
      private int m_StartIndex;
      [ReadOnly]
      private int m_Length;
      [ReadOnly]
      private NativeList<PathfindSetupSystem.SetupListItem> m_SetupItems;
      [ReadOnly]
      private PathfindTargetSeekerData m_SeekerData;
      private NativeQueue<PathfindSetupTarget>.ParallelWriter m_TargetQueue;

      public SetupData(
        int startIndex,
        int endIndex,
        NativeList<PathfindSetupSystem.SetupListItem> setupItems,
        PathfindTargetSeekerData seekerData,
        NativeQueue<PathfindSetupTarget>.ParallelWriter targetQueue)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_StartIndex = startIndex;
        // ISSUE: reference to a compiler-generated field
        this.m_Length = endIndex - startIndex;
        // ISSUE: reference to a compiler-generated field
        this.m_SetupItems = setupItems;
        // ISSUE: reference to a compiler-generated field
        this.m_SeekerData = seekerData;
        // ISSUE: reference to a compiler-generated field
        this.m_TargetQueue = targetQueue;
      }

      public int Length => this.m_Length;

      public void GetItem(
        int index,
        out Entity entity,
        out PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindSetupSystem.SetupListItem setupItem = this.m_SetupItems[this.m_StartIndex + index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        entity = setupItem.m_Target.m_Entity != Entity.Null ? setupItem.m_Target.m_Entity : setupItem.m_Owner;
        // ISSUE: reference to a compiler-generated field
        PathfindSetupBuffer buffer = new PathfindSetupBuffer()
        {
          m_Queue = this.m_TargetQueue,
          m_SetupIndex = index
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        targetSeeker = new PathfindTargetSeeker<PathfindSetupBuffer>(this.m_SeekerData, setupItem.m_Parameters, setupItem.m_Target, buffer, setupItem.m_RandomSeed);
      }

      public void GetItem(
        int index,
        out Entity entity,
        out Entity owner,
        out PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PathfindSetupSystem.SetupListItem setupItem = this.m_SetupItems[this.m_StartIndex + index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        entity = setupItem.m_Target.m_Entity != Entity.Null ? setupItem.m_Target.m_Entity : setupItem.m_Owner;
        // ISSUE: reference to a compiler-generated field
        owner = setupItem.m_Owner;
        // ISSUE: reference to a compiler-generated field
        PathfindSetupBuffer buffer = new PathfindSetupBuffer()
        {
          m_Queue = this.m_TargetQueue,
          m_SetupIndex = index
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        targetSeeker = new PathfindTargetSeeker<PathfindSetupBuffer>(this.m_SeekerData, setupItem.m_Parameters, setupItem.m_Target, buffer, setupItem.m_RandomSeed);
      }
    }

    public struct SetupListItem : IComparable<PathfindSetupSystem.SetupListItem>
    {
      public SetupQueueTarget m_Target;
      public PathfindParameters m_Parameters;
      public UnsafeList<PathTarget> m_Buffer;
      public Entity m_Owner;
      public RandomSeed m_RandomSeed;
      public int m_ActionIndex;
      public bool m_ActionStart;

      public int CompareTo(PathfindSetupSystem.SetupListItem other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Target.m_Type - other.m_Target.m_Type;
      }

      public SetupListItem(
        SetupQueueTarget target,
        PathfindParameters parameters,
        Entity owner,
        RandomSeed randomSeed,
        int actionIndex,
        bool actionStart)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Target = target;
        // ISSUE: reference to a compiler-generated field
        this.m_Parameters = parameters;
        // ISSUE: reference to a compiler-generated field
        this.m_Buffer = new UnsafeList<PathTarget>(0, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
        // ISSUE: reference to a compiler-generated field
        this.m_Owner = owner;
        // ISSUE: reference to a compiler-generated field
        this.m_RandomSeed = randomSeed;
        // ISSUE: reference to a compiler-generated field
        this.m_ActionIndex = actionIndex;
        // ISSUE: reference to a compiler-generated field
        this.m_ActionStart = actionStart;
      }
    }

    private struct ActionListItem
    {
      public PathfindAction m_Action;
      public Entity m_Owner;
      public uint m_ResultFrame;
      public object m_System;

      public ActionListItem(PathfindAction action, Entity owner, uint resultFrame, object system)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Action = action;
        // ISSUE: reference to a compiler-generated field
        this.m_Owner = owner;
        // ISSUE: reference to a compiler-generated field
        this.m_ResultFrame = resultFrame;
        // ISSUE: reference to a compiler-generated field
        this.m_System = system;
      }
    }

    private struct SetupQueue
    {
      public NativeQueue<SetupQueueItem> m_Queue;
      public uint m_ResultFrame;
      public uint m_SpreadFrame;
      public object m_System;
    }

    [BurstCompile]
    private struct DequePathTargetsJob : IJob
    {
      [NativeDisableUnsafePtrRestriction]
      public unsafe PathfindSetupSystem.SetupListItem* m_SetupItems;
      public NativeQueue<PathfindSetupTarget> m_TargetQueue;

      public unsafe void Execute()
      {
        PathfindSetupTarget pathfindSetupTarget;
        // ISSUE: reference to a compiler-generated field
        while (this.m_TargetQueue.TryDequeue(out pathfindSetupTarget))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: explicit reference operation
          ref PathfindSetupSystem.SetupListItem local1 = @this.m_SetupItems[pathfindSetupTarget.m_SetupIndex];
          // ISSUE: reference to a compiler-generated field
          if ((local1.m_Parameters.m_PathfindFlags & PathfindFlags.SkipPathfind) != (PathfindFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (local1.m_Buffer.Length == 0)
            {
              // ISSUE: reference to a compiler-generated field
              local1.m_Buffer.Add(in pathfindSetupTarget.m_PathTarget);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              ref PathTarget local2 = ref local1.m_Buffer.ElementAt(0);
              if ((double) pathfindSetupTarget.m_PathTarget.m_Cost < (double) local2.m_Cost)
                local2 = pathfindSetupTarget.m_PathTarget;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            local1.m_Buffer.Add(in pathfindSetupTarget.m_PathTarget);
          }
        }
      }
    }
  }
}
