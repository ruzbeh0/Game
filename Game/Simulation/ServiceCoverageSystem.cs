// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ServiceCoverageSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class ServiceCoverageSystem : GameSystemBase
  {
    public const uint COVERAGE_UPDATE_INTERVAL = 256;
    private SimulationSystem m_SimulationSystem;
    private PathfindQueueSystem m_PathfindQueueSystem;
    private AirwaySystem m_AirwaySystem;
    private EntityQuery m_EdgeQuery;
    private EntityQuery m_BuildingQuery;
    private PathfindTargetSeekerData m_TargetSeekerData;
    private NativeQueue<ServiceCoverageSystem.QueueItem> m_PendingCoverages;
    private CoverageService m_LastCoverageService;
    private ServiceCoverageSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindQueueSystem = this.World.GetOrCreateSystemManaged<PathfindQueueSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirwaySystem = this.World.GetOrCreateSystemManaged<AirwaySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.Edge>(), ComponentType.ReadWrite<Game.Net.ServiceCoverage>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<CoverageServiceType>(), ComponentType.ReadOnly<Game.Pathfind.CoverageElement>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_TargetSeekerData = new PathfindTargetSeekerData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_PendingCoverages = new NativeQueue<ServiceCoverageSystem.QueueItem>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LastCoverageService = CoverageService.Count;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PendingCoverages.Dispose();
      base.OnDestroy();
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      // ISSUE: reference to a compiler-generated field
      this.m_PendingCoverages.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_LastCoverageService = CoverageService.Count;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      CoverageService frameService1 = ServiceCoverageSystem.GetFrameService(this.m_SimulationSystem.frameIndex);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      CoverageService frameService2 = ServiceCoverageSystem.GetFrameService(this.m_SimulationSystem.frameIndex + 1U);
      if (frameService1 == frameService2)
      {
        JobHandle outputDeps;
        // ISSUE: reference to a compiler-generated method
        if (!this.EnqueuePendingCoverages(out outputDeps))
          return;
        this.Dependency = outputDeps;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_BuildingQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ServiceCoverage_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<Game.Net.ServiceCoverage> bufferTypeHandle = this.__TypeHandle.__Game_Net_ServiceCoverage_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_CoverageServiceType_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        SharedComponentTypeHandle<CoverageServiceType> componentTypeHandle = this.__TypeHandle.__Game_Net_CoverageServiceType_SharedComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        uint num1 = this.m_SimulationSystem.frameIndex + 192U;
        // ISSUE: reference to a compiler-generated field
        uint num2 = this.m_SimulationSystem.frameIndex + 256U;
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          if (archetypeChunk.GetSharedComponent<CoverageServiceType>(componentTypeHandle).m_Service == frameService2)
          {
            NativeArray<Entity> nativeArray = archetypeChunk.GetNativeArray(entityTypeHandle);
            for (int index2 = 0; index2 < nativeArray.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_PendingCoverages.Enqueue(new ServiceCoverageSystem.QueueItem()
              {
                m_Entity = nativeArray[index2],
                m_QueueFrame = num1,
                m_ResultFrame = num2
              });
            }
          }
        }
        JobHandle outputDeps;
        // ISSUE: reference to a compiler-generated method
        this.EnqueuePendingCoverages(out outputDeps);
        // ISSUE: reference to a compiler-generated field
        if (this.m_LastCoverageService != CoverageService.Count)
        {
          NativeList<ServiceCoverageSystem.BuildingData> list = new NativeList<ServiceCoverageSystem.BuildingData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
          NativeList<ServiceCoverageSystem.CoverageElement> nativeList = new NativeList<ServiceCoverageSystem.CoverageElement>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Pathfind_CoverageElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          ServiceCoverageSystem.PrepareCoverageJob jobData1 = new ServiceCoverageSystem.PrepareCoverageJob()
          {
            m_Service = frameService1,
            m_BuildingChunks = archetypeChunkArray,
            m_CoverageServiceType = componentTypeHandle,
            m_EntityType = entityTypeHandle,
            m_CoverageElementType = this.__TypeHandle.__Game_Pathfind_CoverageElement_RO_BufferTypeHandle,
            m_BuildingData = list,
            m_Elements = nativeList
          };
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          ServiceCoverageSystem.ClearCoverageJob jobData2 = new ServiceCoverageSystem.ClearCoverageJob()
          {
            m_CoverageIndex = (int) frameService1,
            m_ServiceCoverageType = bufferTypeHandle
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Net_ServiceCoverage_RW_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Areas_ServiceDistrict_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Pathfind_CoverageElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Buildings_ModifiedServiceCoverage_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Net_Density_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          ServiceCoverageSystem.ProcessCoverageJob jobData3 = new ServiceCoverageSystem.ProcessCoverageJob()
          {
            m_CoverageIndex = (int) frameService1,
            m_BuildingData = list,
            m_Elements = nativeList,
            m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
            m_DensityData = this.__TypeHandle.__Game_Net_Density_RO_ComponentLookup,
            m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
            m_ModifiedServiceCoverageData = this.__TypeHandle.__Game_Buildings_ModifiedServiceCoverage_RO_ComponentLookup,
            m_BorderDistrictData = this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentLookup,
            m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
            m_PrefabCoverageData = this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentLookup,
            m_CoverageElements = this.__TypeHandle.__Game_Pathfind_CoverageElement_RO_BufferLookup,
            m_ServiceDistricts = this.__TypeHandle.__Game_Areas_ServiceDistrict_RO_BufferLookup,
            m_Efficiencies = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup,
            m_CoverageData = this.__TypeHandle.__Game_Net_ServiceCoverage_RW_BufferLookup
          };
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          ServiceCoverageSystem.ApplyCoverageJob jobData4 = new ServiceCoverageSystem.ApplyCoverageJob()
          {
            m_BuildingData = list,
            m_Elements = nativeList
          };
          JobHandle jobHandle1 = jobData1.Schedule<ServiceCoverageSystem.PrepareCoverageJob>(this.Dependency);
          // ISSUE: reference to a compiler-generated field
          JobHandle job1 = jobData2.ScheduleParallel<ServiceCoverageSystem.ClearCoverageJob>(this.m_EdgeQuery, this.Dependency);
          JobHandle dependsOn = jobData3.Schedule<ServiceCoverageSystem.ProcessCoverageJob, ServiceCoverageSystem.BuildingData>(list, 1, JobHandle.CombineDependencies(jobHandle1, job1));
          JobHandle jobHandle2 = jobData4.Schedule<ServiceCoverageSystem.ApplyCoverageJob>(dependsOn);
          archetypeChunkArray.Dispose(jobHandle1);
          list.Dispose(jobHandle2);
          nativeList.Dispose(jobHandle2);
          outputDeps = JobHandle.CombineDependencies(outputDeps, jobHandle2);
        }
        else
          archetypeChunkArray.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_LastCoverageService = frameService2;
        this.Dependency = outputDeps;
      }
    }

    private bool EnqueuePendingCoverages(out JobHandle outputDeps)
    {
      outputDeps = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (this.m_PendingCoverages.IsEmpty())
        return false;
      // ISSUE: reference to a compiler-generated field
      int count = this.m_PendingCoverages.Count;
      int num1 = 192;
      int num2 = (count + num1 - 1) / num1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TargetSeekerData.Update((SystemBase) this, this.m_AirwaySystem.GetAirwayData());
      PathfindParameters pathfindParameters = new PathfindParameters()
      {
        m_MaxSpeed = (float2) 111.111115f,
        m_WalkSpeed = (float2) 5.555556f,
        m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
        m_PathfindFlags = PathfindFlags.Stable | PathfindFlags.IgnoreFlow,
        m_IgnoredRules = RuleFlags.HasBlockage | RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic
      };
      SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ServiceCoverageSystem.SetupCoverageSearchJob jobData = new ServiceCoverageSystem.SetupCoverageSearchJob()
      {
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabCoverageData = this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentLookup
      };
      for (int index = 0; index < count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        ServiceCoverageSystem.QueueItem queueItem = this.m_PendingCoverages.Peek();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (--num2 >= 0 || queueItem.m_QueueFrame <= this.m_SimulationSystem.frameIndex)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_PendingCoverages.Dequeue();
          CoverageServiceType component;
          // ISSUE: reference to a compiler-generated field
          if (this.EntityManager.TryGetSharedComponent<CoverageServiceType>(queueItem.m_Entity, out component))
          {
            // ISSUE: reference to a compiler-generated method
            ServiceCoverageSystem.SetupPathfindMethods(component.m_Service, ref pathfindParameters, ref setupQueueTarget);
            CoverageAction action = new CoverageAction(Allocator.Persistent);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            jobData.m_Entity = queueItem.m_Entity;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            jobData.m_TargetSeeker = new PathfindTargetSeeker<PathfindTargetBuffer>(this.m_TargetSeekerData, pathfindParameters, setupQueueTarget, (PathfindTargetBuffer) action.data.m_Sources.AsParallelWriter(), RandomSeed.Next());
            // ISSUE: reference to a compiler-generated field
            jobData.m_Action = action;
            JobHandle jobHandle = jobData.Schedule<ServiceCoverageSystem.SetupCoverageSearchJob>(this.Dependency);
            outputDeps = JobHandle.CombineDependencies(outputDeps, jobHandle);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_PathfindQueueSystem.Enqueue(action, queueItem.m_Entity, jobHandle, queueItem.m_ResultFrame, (object) this);
          }
        }
        else
          break;
      }
      return true;
    }

    public static void SetupPathfindMethods(
      CoverageService service,
      ref PathfindParameters pathfindParameters,
      ref SetupQueueTarget setupQueueTarget)
    {
      switch (service)
      {
        case CoverageService.Park:
          pathfindParameters.m_Methods = PathMethod.Pedestrian;
          setupQueueTarget.m_Methods = PathMethod.Pedestrian;
          setupQueueTarget.m_RoadTypes = RoadTypes.None;
          setupQueueTarget.m_ActivityMask.m_Mask |= new ActivityMask(ActivityType.BenchSitting).m_Mask;
          setupQueueTarget.m_ActivityMask.m_Mask |= new ActivityMask(ActivityType.PullUps).m_Mask;
          setupQueueTarget.m_ActivityMask.m_Mask |= new ActivityMask(ActivityType.Standing).m_Mask;
          setupQueueTarget.m_ActivityMask.m_Mask |= new ActivityMask(ActivityType.GroundLaying).m_Mask;
          setupQueueTarget.m_ActivityMask.m_Mask |= new ActivityMask(ActivityType.GroundSitting).m_Mask;
          setupQueueTarget.m_ActivityMask.m_Mask |= new ActivityMask(ActivityType.PushUps).m_Mask;
          setupQueueTarget.m_ActivityMask.m_Mask |= new ActivityMask(ActivityType.SitUps).m_Mask;
          setupQueueTarget.m_ActivityMask.m_Mask |= new ActivityMask(ActivityType.JumpingJacks).m_Mask;
          setupQueueTarget.m_ActivityMask.m_Mask |= new ActivityMask(ActivityType.JumpingLunges).m_Mask;
          setupQueueTarget.m_ActivityMask.m_Mask |= new ActivityMask(ActivityType.Squats).m_Mask;
          setupQueueTarget.m_ActivityMask.m_Mask |= new ActivityMask(ActivityType.Yoga).m_Mask;
          break;
        case CoverageService.PostService:
        case CoverageService.Education:
        case CoverageService.EmergencyShelter:
        case CoverageService.Welfare:
          pathfindParameters.m_Methods = PathMethod.Pedestrian;
          setupQueueTarget.m_Methods = PathMethod.Pedestrian;
          setupQueueTarget.m_RoadTypes = RoadTypes.None;
          break;
        default:
          pathfindParameters.m_Methods = PathMethod.Road;
          setupQueueTarget.m_Methods = PathMethod.Road;
          setupQueueTarget.m_RoadTypes = RoadTypes.Car;
          break;
      }
    }

    private static CoverageService GetFrameService(uint frame)
    {
      return (CoverageService) (frame % 256U * 8U / 256U);
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
    public ServiceCoverageSystem()
    {
    }

    [BurstCompile]
    public struct ClearCoverageJob : IJobChunk
    {
      [ReadOnly]
      public int m_CoverageIndex;
      public BufferTypeHandle<Game.Net.ServiceCoverage> m_ServiceCoverageType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Net.ServiceCoverage> bufferAccessor = chunk.GetBufferAccessor<Game.Net.ServiceCoverage>(ref this.m_ServiceCoverageType);
        for (int index = 0; index < bufferAccessor.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          bufferAccessor[index][this.m_CoverageIndex] = new Game.Net.ServiceCoverage();
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

    public struct CoverageElement : IComparable<ServiceCoverageSystem.CoverageElement>
    {
      [NativeDisableUnsafePtrRestriction]
      public unsafe void* m_CoveragePtr;
      public float2 m_Coverage;
      public float m_AverageCoverage;
      public float m_DensityFactor;
      public float m_LengthFactor;

      public int CompareTo(ServiceCoverageSystem.CoverageElement other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.select(0, math.select(-1, 1, (double) this.m_AverageCoverage < (double) other.m_AverageCoverage), (double) this.m_AverageCoverage != (double) other.m_AverageCoverage);
      }
    }

    private struct QueueItem
    {
      public Entity m_Entity;
      public uint m_QueueFrame;
      public uint m_ResultFrame;
    }

    public struct BuildingData
    {
      public Entity m_Entity;
      public int m_ElementIndex;
      public int m_ElementCount;
      public float m_Total;
      public float m_Remaining;
    }

    [BurstCompile]
    public struct PrepareCoverageJob : IJob
    {
      [ReadOnly]
      public CoverageService m_Service;
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_BuildingChunks;
      [ReadOnly]
      public SharedComponentTypeHandle<CoverageServiceType> m_CoverageServiceType;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<Game.Pathfind.CoverageElement> m_CoverageElementType;
      public NativeList<ServiceCoverageSystem.BuildingData> m_BuildingData;
      public NativeList<ServiceCoverageSystem.CoverageElement> m_Elements;

      public void Execute()
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ServiceCoverageSystem.BuildingData buildingData = new ServiceCoverageSystem.BuildingData();
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_BuildingChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk buildingChunk = this.m_BuildingChunks[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (buildingChunk.GetSharedComponent<CoverageServiceType>(this.m_CoverageServiceType).m_Service == this.m_Service)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray = buildingChunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Game.Pathfind.CoverageElement> bufferAccessor = buildingChunk.GetBufferAccessor<Game.Pathfind.CoverageElement>(ref this.m_CoverageElementType);
            for (int index2 = 0; index2 < buildingChunk.Count; ++index2)
            {
              DynamicBuffer<Game.Pathfind.CoverageElement> dynamicBuffer = bufferAccessor[index2];
              if (dynamicBuffer.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                buildingData.m_Entity = nativeArray[index2];
                // ISSUE: reference to a compiler-generated field
                buildingData.m_ElementCount = dynamicBuffer.Length;
                // ISSUE: reference to a compiler-generated field
                this.m_BuildingData.Add(in buildingData);
                // ISSUE: reference to a compiler-generated field
                buildingData.m_ElementIndex += dynamicBuffer.Length;
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Elements.ResizeUninitialized(buildingData.m_ElementIndex);
      }
    }

    [BurstCompile]
    public struct ProcessCoverageJob : IJobParallelForDefer
    {
      [ReadOnly]
      public int m_CoverageIndex;
      [NativeDisableParallelForRestriction]
      public NativeList<ServiceCoverageSystem.BuildingData> m_BuildingData;
      [NativeDisableParallelForRestriction]
      public NativeList<ServiceCoverageSystem.CoverageElement> m_Elements;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Density> m_DensityData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<ModifiedServiceCoverage> m_ModifiedServiceCoverageData;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> m_BorderDistrictData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<CoverageData> m_PrefabCoverageData;
      [ReadOnly]
      public BufferLookup<Game.Pathfind.CoverageElement> m_CoverageElements;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> m_ServiceDistricts;
      [ReadOnly]
      public BufferLookup<Efficiency> m_Efficiencies;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Game.Net.ServiceCoverage> m_CoverageData;

      public unsafe void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ref ServiceCoverageSystem.BuildingData local = ref this.m_BuildingData.ElementAt(index);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[local.m_Entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Pathfind.CoverageElement> coverageElement1 = this.m_CoverageElements[local.m_Entity];
        CoverageData componentData1;
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabCoverageData.TryGetComponent(prefabRef.m_Prefab, out componentData1);
        DynamicBuffer<ServiceDistrict> bufferData1 = new DynamicBuffer<ServiceDistrict>();
        Temp componentData2;
        float efficiency;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_TempData.TryGetComponent(local.m_Entity, out componentData2))
        {
          ModifiedServiceCoverage componentData3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ModifiedServiceCoverageData.TryGetComponent(componentData2.m_Original, out componentData3))
            componentData3.ReplaceData(ref componentData1);
          // ISSUE: reference to a compiler-generated field
          this.m_ServiceDistricts.TryGetBuffer(componentData2.m_Original, out bufferData1);
          // ISSUE: reference to a compiler-generated field
          efficiency = BuildingUtils.GetEfficiency(componentData2.m_Original, ref this.m_Efficiencies);
        }
        else
        {
          ModifiedServiceCoverage componentData4;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ModifiedServiceCoverageData.TryGetComponent(local.m_Entity, out componentData4))
            componentData4.ReplaceData(ref componentData1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ServiceDistricts.TryGetBuffer(local.m_Entity, out bufferData1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          efficiency = BuildingUtils.GetEfficiency(local.m_Entity, ref this.m_Efficiencies);
        }
        NativeHashSet<Entity> nativeHashSet = new NativeHashSet<Entity>();
        if (bufferData1.IsCreated && bufferData1.Length != 0)
        {
          nativeHashSet = new NativeHashSet<Entity>(bufferData1.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          for (int index1 = 0; index1 < bufferData1.Length; ++index1)
            nativeHashSet.Add(bufferData1[index1].m_District);
        }
        // ISSUE: reference to a compiler-generated field
        int elementIndex = local.m_ElementIndex;
        for (int index2 = 0; index2 < coverageElement1.Length; ++index2)
        {
          Game.Pathfind.CoverageElement coverageElement2 = coverageElement1[index2];
          DynamicBuffer<Game.Net.ServiceCoverage> bufferData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CoverageData.TryGetBuffer(coverageElement2.m_Edge, out bufferData2))
          {
            float num = 1f;
            BorderDistrict componentData5;
            // ISSUE: reference to a compiler-generated field
            if (nativeHashSet.IsCreated && this.m_BorderDistrictData.TryGetComponent(coverageElement2.m_Edge, out componentData5))
            {
              if (componentData5.m_Right == componentData5.m_Left)
              {
                if (!nativeHashSet.Contains(componentData5.m_Left))
                  continue;
              }
              else
              {
                bool2 x;
                x.x = nativeHashSet.Contains(componentData5.m_Left);
                x.y = nativeHashSet.Contains(componentData5.m_Right);
                if (math.any(x))
                  num = math.select(0.5f, 1f, math.all(x));
                else
                  continue;
              }
            }
            float x1 = 0.01f;
            Density componentData6;
            // ISSUE: reference to a compiler-generated field
            if (this.m_DensityData.TryGetComponent(coverageElement2.m_Edge, out componentData6))
              x1 = math.max(x1, componentData6.m_Density);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            ServiceCoverageSystem.CoverageElement coverageElement3 = new ServiceCoverageSystem.CoverageElement()
            {
              m_CoveragePtr = UnsafeUtility.AddressOf<Game.Net.ServiceCoverage>(ref bufferData2.ElementAt(this.m_CoverageIndex)),
              m_Coverage = math.max((float2) 0.0f, 1f - coverageElement2.m_Cost * coverageElement2.m_Cost) * componentData1.m_Magnitude * efficiency
            };
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            coverageElement3.m_AverageCoverage = math.csum(coverageElement3.m_Coverage) * 0.5f;
            // ISSUE: reference to a compiler-generated field
            coverageElement3.m_DensityFactor = num;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            coverageElement3.m_LengthFactor = this.m_CurveData[coverageElement2.m_Edge].m_Length * math.sqrt(x1);
            // ISSUE: reference to a compiler-generated field
            this.m_Elements[elementIndex++] = coverageElement3;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (elementIndex > local.m_ElementIndex + 1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Elements.AsArray().GetSubArray(local.m_ElementIndex, elementIndex - local.m_ElementIndex).Sort<ServiceCoverageSystem.CoverageElement>();
        }
        // ISSUE: reference to a compiler-generated field
        local.m_Total = componentData1.m_Capacity;
        // ISSUE: reference to a compiler-generated field
        local.m_Remaining = componentData1.m_Capacity;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        local.m_ElementCount = elementIndex - local.m_ElementIndex;
        if (!nativeHashSet.IsCreated)
          return;
        nativeHashSet.Dispose();
      }
    }

    private struct BuildingDataComparer : IComparer<ServiceCoverageSystem.BuildingData>
    {
      public NativeList<ServiceCoverageSystem.CoverageElement> m_Elements;

      public int Compare(ServiceCoverageSystem.BuildingData x, ServiceCoverageSystem.BuildingData y)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        ServiceCoverageSystem.CoverageElement element = this.m_Elements[x.m_ElementIndex];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        return element.CompareTo(this.m_Elements[y.m_ElementIndex]);
      }
    }

    [BurstCompile]
    public struct ApplyCoverageJob : IJob
    {
      public NativeList<ServiceCoverageSystem.BuildingData> m_BuildingData;
      public NativeList<ServiceCoverageSystem.CoverageElement> m_Elements;

      public unsafe void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_BuildingData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          ServiceCoverageSystem.BuildingData buildingData = this.m_BuildingData[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (buildingData.m_ElementCount == 0 || (double) buildingData.m_Remaining <= 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_BuildingData.RemoveAtSwapBack(index--);
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_BuildingData.Sort<ServiceCoverageSystem.BuildingData, ServiceCoverageSystem.BuildingDataComparer>(new ServiceCoverageSystem.BuildingDataComparer()
        {
          m_Elements = this.m_Elements
        });
        int index1 = 0;
        // ISSUE: reference to a compiler-generated field
        while (index1 < this.m_BuildingData.Length)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          ServiceCoverageSystem.BuildingData buildingData1 = this.m_BuildingData[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          ServiceCoverageSystem.CoverageElement element1 = this.m_Elements[buildingData1.m_ElementIndex++];
          // ISSUE: reference to a compiler-generated field
          ref Game.Net.ServiceCoverage local = ref UnsafeUtility.AsRef<Game.Net.ServiceCoverage>(element1.m_CoveragePtr);
          // ISSUE: reference to a compiler-generated field
          if (math.any(element1.m_Coverage > local.m_Coverage))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float num1 = (float) (0.99000000953674316 * (1.0 - (double) buildingData1.m_Remaining / (double) buildingData1.m_Total));
            float num2 = num1 * num1;
            float num3 = num2 * num2;
            float num4 = 1f - num3 * num3;
            // ISSUE: reference to a compiler-generated field
            float2 float2_1 = element1.m_Coverage * num4;
            // ISSUE: reference to a compiler-generated field
            float2 float2_2 = math.clamp(float2_1 - local.m_Coverage, (float2) 0.0f, float2_1 * element1.m_DensityFactor);
            local.m_Coverage += float2_2;
            // ISSUE: reference to a compiler-generated field
            float2 float2_3 = math.saturate(float2_2 / element1.m_Coverage);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            buildingData1.m_Remaining -= math.lerp(float2_3.x, float2_3.y, 0.5f) * element1.m_LengthFactor * element1.m_DensityFactor;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (--buildingData1.m_ElementCount == 0 || (double) buildingData1.m_Remaining <= 0.0)
          {
            ++index1;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            ServiceCoverageSystem.CoverageElement element2 = this.m_Elements[buildingData1.m_ElementIndex];
            // ISSUE: reference to a compiler-generated field
            this.m_BuildingData[index1] = buildingData1;
            // ISSUE: reference to a compiler-generated field
            for (int index2 = index1 + 1; index2 < this.m_BuildingData.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              ServiceCoverageSystem.BuildingData buildingData2 = this.m_BuildingData[index2];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              ServiceCoverageSystem.CoverageElement element3 = this.m_Elements[buildingData2.m_ElementIndex];
              // ISSUE: reference to a compiler-generated method
              if (element2.CompareTo(element3) > 0)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_BuildingData[index2] = buildingData1;
                // ISSUE: reference to a compiler-generated field
                this.m_BuildingData[index2 - 1] = buildingData2;
              }
              else
                break;
            }
          }
        }
      }
    }

    [BurstCompile]
    public struct SetupCoverageSearchJob : IJob
    {
      [ReadOnly]
      public Entity m_Entity;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<CoverageData> m_PrefabCoverageData;
      public CoverageAction m_Action;
      public PathfindTargetSeeker<PathfindTargetBuffer> m_TargetSeeker;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[this.m_Entity];
        CoverageData coverageData = new CoverageData();
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabCoverageData.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          coverageData = this.m_PrefabCoverageData[prefabRef.m_Prefab];
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TargetSeeker.FindTargets(this.m_Entity, 0.0f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Action.data.m_Parameters = new CoverageParameters()
        {
          m_Methods = this.m_TargetSeeker.m_PathfindParameters.m_Methods,
          m_Range = coverageData.m_Range
        };
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public BufferTypeHandle<Game.Net.ServiceCoverage> __Game_Net_ServiceCoverage_RW_BufferTypeHandle;
      public SharedComponentTypeHandle<CoverageServiceType> __Game_Net_CoverageServiceType_SharedComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Pathfind.CoverageElement> __Game_Pathfind_CoverageElement_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Density> __Game_Net_Density_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ModifiedServiceCoverage> __Game_Buildings_ModifiedServiceCoverage_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> __Game_Areas_BorderDistrict_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CoverageData> __Game_Prefabs_CoverageData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Pathfind.CoverageElement> __Game_Pathfind_CoverageElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> __Game_Areas_ServiceDistrict_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Efficiency> __Game_Buildings_Efficiency_RO_BufferLookup;
      public BufferLookup<Game.Net.ServiceCoverage> __Game_Net_ServiceCoverage_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ServiceCoverage_RW_BufferTypeHandle = state.GetBufferTypeHandle<Game.Net.ServiceCoverage>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CoverageServiceType_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<CoverageServiceType>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_CoverageElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Pathfind.CoverageElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Density_RO_ComponentLookup = state.GetComponentLookup<Density>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ModifiedServiceCoverage_RO_ComponentLookup = state.GetComponentLookup<ModifiedServiceCoverage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_BorderDistrict_RO_ComponentLookup = state.GetComponentLookup<BorderDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CoverageData_RO_ComponentLookup = state.GetComponentLookup<CoverageData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_CoverageElement_RO_BufferLookup = state.GetBufferLookup<Game.Pathfind.CoverageElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_ServiceDistrict_RO_BufferLookup = state.GetBufferLookup<ServiceDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferLookup = state.GetBufferLookup<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ServiceCoverage_RW_BufferLookup = state.GetBufferLookup<Game.Net.ServiceCoverage>();
      }
    }
  }
}
