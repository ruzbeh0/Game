// Decompiled with JetBrains decompiler
// Type: Game.Tools.CoveragePreviewSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Simulation;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class CoveragePreviewSystem : GameSystemBase
  {
    private PathfindQueueSystem m_PathfindQueueSystem;
    private AirwaySystem m_AirwaySystem;
    private EntityQuery m_EdgeQuery;
    private EntityQuery m_ModifiedQuery;
    private EntityQuery m_UpdatedBuildingQuery;
    private EntityQuery m_ServiceBuildingQuery;
    private EntityQuery m_InfomodeQuery;
    private EntityQuery m_EventQuery;
    private CoverageService m_LastService;
    private PathfindTargetSeekerData m_TargetSeekerData;
    private HashSet<Entity> m_PendingCoverages;
    private CoveragePreviewSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindQueueSystem = this.World.GetOrCreateSystemManaged<PathfindQueueSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirwaySystem = this.World.GetOrCreateSystemManaged<AirwaySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.Edge>(), ComponentType.ReadWrite<Game.Net.ServiceCoverage>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadWrite<Game.Net.Edge>(),
          ComponentType.ReadWrite<District>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadWrite<Game.Net.Edge>(),
          ComponentType.ReadWrite<District>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<CoverageServiceType>(), ComponentType.ReadOnly<Game.Pathfind.CoverageElement>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Updated>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<CoverageServiceType>(), ComponentType.ReadOnly<Game.Pathfind.CoverageElement>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Hidden>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_InfomodeQuery = this.GetEntityQuery(ComponentType.ReadOnly<InfomodeActive>(), ComponentType.ReadOnly<InfoviewCoverageData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EventQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Common.Event>(), ComponentType.ReadOnly<CoverageUpdated>());
      // ISSUE: reference to a compiler-generated field
      this.m_LastService = CoverageService.Count;
      // ISSUE: reference to a compiler-generated field
      this.m_TargetSeekerData = new PathfindTargetSeekerData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_PendingCoverages = new HashSet<Entity>();
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_InfomodeQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStopRunning()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LastService = CoverageService.Count;
      base.OnStopRunning();
    }

    private bool GetInfoviewCoverageData(out InfoviewCoverageData coverageData)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_InfomodeQuery.IsEmptyIgnoreFilter)
      {
        coverageData = new InfoviewCoverageData();
        return false;
      }
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_InfomodeQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewCoverageData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<InfoviewCoverageData> componentTypeHandle = this.__TypeHandle.__Game_Prefabs_InfoviewCoverageData_RO_ComponentTypeHandle;
      coverageData = archetypeChunkArray[0].GetNativeArray<InfoviewCoverageData>(ref componentTypeHandle)[0];
      archetypeChunkArray.Dispose();
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      InfoviewCoverageData coverageData;
      // ISSUE: reference to a compiler-generated method
      if (!this.GetInfoviewCoverageData(out coverageData))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LastService = CoverageService.Count;
      }
      // ISSUE: reference to a compiler-generated field
      bool flag1 = this.m_LastService != coverageData.m_Service;
      // ISSUE: reference to a compiler-generated field
      bool flag2 = flag1 || !this.m_ModifiedQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      this.m_LastService = coverageData.m_Service;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      bool flag3 = flag2 ? !this.m_ServiceBuildingQuery.IsEmptyIgnoreFilter : !this.m_UpdatedBuildingQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      bool flag4 = !this.m_EventQuery.IsEmptyIgnoreFilter;
      if (!flag3 && !flag2 && !flag4)
        return;
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_ServiceBuildingQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CoverageServiceType_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      SharedComponentTypeHandle<CoverageServiceType> componentTypeHandle1 = this.__TypeHandle.__Game_Net_CoverageServiceType_SharedComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ServiceCoverage_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferTypeHandle<Game.Net.ServiceCoverage> bufferTypeHandle = this.__TypeHandle.__Game_Net_ServiceCoverage_RW_BufferTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<Created> componentTypeHandle2 = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<Temp> componentTypeHandle3 = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle;
      JobHandle job0 = new JobHandle();
      JobHandle jobHandle1 = new JobHandle();
      if (flag2)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PendingCoverages.Clear();
      }
      if (flag3)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_TargetSeekerData.Update((SystemBase) this, this.m_AirwaySystem.GetAirwayData());
        PathfindParameters pathfindParameters = new PathfindParameters();
        pathfindParameters.m_MaxSpeed = (float2) 111.111115f;
        pathfindParameters.m_WalkSpeed = (float2) 5.555556f;
        pathfindParameters.m_Weights = new PathfindWeights(1f, 1f, 1f, 1f);
        pathfindParameters.m_PathfindFlags = PathfindFlags.Stable | PathfindFlags.IgnoreFlow;
        pathfindParameters.m_IgnoredRules = RuleFlags.HasBlockage | RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic;
        SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
        // ISSUE: reference to a compiler-generated method
        ServiceCoverageSystem.SetupPathfindMethods(coverageData.m_Service, ref pathfindParameters, ref setupQueueTarget);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ArchetypeChunk> nativeArray1 = !flag2 ? this.m_UpdatedBuildingQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob) : archetypeChunkArray;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_BackSide_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CoveragePreviewSystem.SetupCoverageSearchJob jobData = new CoveragePreviewSystem.SetupCoverageSearchJob()
        {
          m_BackSideData = this.__TypeHandle.__Game_Buildings_BackSide_RO_ComponentLookup,
          m_PrefabCoverageData = this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentLookup
        };
        this.EntityManager.CompleteDependencyBeforeRO<Temp>();
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = nativeArray1[index1];
          if (archetypeChunk.GetSharedComponent<CoverageServiceType>(componentTypeHandle1).m_Service == coverageData.m_Service)
          {
            NativeArray<Entity> nativeArray2 = archetypeChunk.GetNativeArray(entityTypeHandle);
            NativeArray<Temp> nativeArray3 = archetypeChunk.GetNativeArray<Temp>(ref componentTypeHandle3);
            for (int index2 = 0; index2 < archetypeChunk.Count; ++index2)
            {
              Entity entity = nativeArray2[index2];
              Temp temp;
              DynamicBuffer<Game.Pathfind.CoverageElement> buffer;
              if (this.EntityManager.TryGetBuffer<Game.Pathfind.CoverageElement>(!CollectionUtils.TryGet<Temp>(nativeArray3, index2, out temp) || !(temp.m_Original != Entity.Null) ? entity : temp.m_Original, true, out buffer) && buffer.Length == 0)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_PendingCoverages.Add(entity);
              }
            }
          }
        }
        for (int index3 = 0; index3 < nativeArray1.Length; ++index3)
        {
          ArchetypeChunk archetypeChunk = nativeArray1[index3];
          if (archetypeChunk.GetSharedComponent<CoverageServiceType>(componentTypeHandle1).m_Service == coverageData.m_Service)
          {
            NativeArray<Entity> nativeArray4 = archetypeChunk.GetNativeArray(entityTypeHandle);
            NativeArray<Temp> nativeArray5 = archetypeChunk.GetNativeArray<Temp>(ref componentTypeHandle3);
            bool flag5 = archetypeChunk.Has<Created>(ref componentTypeHandle2);
            for (int index4 = 0; index4 < archetypeChunk.Count; ++index4)
            {
              Entity owner = nativeArray4[index4];
              Temp temp;
              CollectionUtils.TryGet<Temp>(nativeArray5, index4, out temp);
              CoverageAction action = new CoverageAction(Allocator.Persistent);
              // ISSUE: reference to a compiler-generated field
              jobData.m_Entity = !(temp.m_Original != Entity.Null) || (temp.m_Flags & TempFlags.Modify) != (TempFlags) 0 ? owner : temp.m_Original;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              jobData.m_TargetSeeker = new PathfindTargetSeeker<PathfindTargetBuffer>(this.m_TargetSeekerData, pathfindParameters, setupQueueTarget, (PathfindTargetBuffer) action.data.m_Sources.AsParallelWriter(), RandomSeed.Next());
              // ISSUE: reference to a compiler-generated field
              jobData.m_Action = action;
              JobHandle jobHandle2 = jobData.Schedule<CoveragePreviewSystem.SetupCoverageSearchJob>(this.Dependency);
              job0 = JobHandle.CombineDependencies(job0, jobHandle2);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_PathfindQueueSystem.Enqueue(action, owner, jobHandle2, uint.MaxValue, (object) this, new PathEventData(), nativeArray5.Length != 0);
              if (flag5 && temp.m_Original != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.__TypeHandle.__Game_Pathfind_CoverageElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                jobHandle1 = new CoveragePreviewSystem.CopyServiceCoverageJob()
                {
                  m_Source = temp.m_Original,
                  m_Target = owner,
                  m_CoverageElements = this.__TypeHandle.__Game_Pathfind_CoverageElement_RW_BufferLookup
                }.Schedule<CoveragePreviewSystem.CopyServiceCoverageJob>(jobHandle1);
              }
            }
          }
        }
        if (!flag2)
          nativeArray1.Dispose();
      }
      if (flag4)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CoverageUpdated> componentDataArray = this.m_EventQuery.ToComponentDataArray<CoverageUpdated>((AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < componentDataArray.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_PendingCoverages.Remove(componentDataArray[index].m_Owner);
        }
        componentDataArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_PendingCoverages.Count != 0)
      {
        JobHandle jobHandle3;
        if (flag1)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          JobHandle job2 = new CoveragePreviewSystem.InitializeCoverageJob()
          {
            m_SourceCoverageIndex = ((int) coverageData.m_Service),
            m_TargetCoverageIndex = 8,
            m_ServiceCoverageType = bufferTypeHandle
          }.ScheduleParallel<CoveragePreviewSystem.InitializeCoverageJob>(this.m_EdgeQuery, this.Dependency);
          jobHandle3 = JobHandle.CombineDependencies(job0, jobHandle1, job2);
        }
        else
          jobHandle3 = JobHandle.CombineDependencies(job0, jobHandle1);
        archetypeChunkArray.Dispose();
        this.Dependency = jobHandle3;
      }
      else
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
          m_Service = coverageData.m_Service,
          m_BuildingChunks = archetypeChunkArray,
          m_CoverageServiceType = componentTypeHandle1,
          m_EntityType = entityTypeHandle,
          m_CoverageElementType = this.__TypeHandle.__Game_Pathfind_CoverageElement_RO_BufferTypeHandle,
          m_BuildingData = list,
          m_Elements = nativeList
        };
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ServiceCoverageSystem.ClearCoverageJob jobData2 = new ServiceCoverageSystem.ClearCoverageJob()
        {
          m_CoverageIndex = 8,
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
          m_CoverageIndex = 8,
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
        JobHandle jobHandle4 = jobData1.Schedule<ServiceCoverageSystem.PrepareCoverageJob>(JobHandle.CombineDependencies(this.Dependency, jobHandle1));
        // ISSUE: reference to a compiler-generated field
        JobHandle job1 = jobData2.ScheduleParallel<ServiceCoverageSystem.ClearCoverageJob>(this.m_EdgeQuery, this.Dependency);
        JobHandle dependsOn = jobData3.Schedule<ServiceCoverageSystem.ProcessCoverageJob, ServiceCoverageSystem.BuildingData>(list, 1, JobHandle.CombineDependencies(jobHandle4, job1));
        JobHandle jobHandle5 = jobData4.Schedule<ServiceCoverageSystem.ApplyCoverageJob>(dependsOn);
        archetypeChunkArray.Dispose(jobHandle4);
        list.Dispose(jobHandle5);
        nativeList.Dispose(jobHandle5);
        this.Dependency = JobHandle.CombineDependencies(job0, jobHandle5);
      }
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
    public CoveragePreviewSystem()
    {
    }

    [BurstCompile]
    public struct InitializeCoverageJob : IJobChunk
    {
      [ReadOnly]
      public int m_SourceCoverageIndex;
      [ReadOnly]
      public int m_TargetCoverageIndex;
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
          DynamicBuffer<Game.Net.ServiceCoverage> dynamicBuffer = bufferAccessor[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          dynamicBuffer[this.m_TargetCoverageIndex] = dynamicBuffer[this.m_SourceCoverageIndex];
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
    private struct CopyServiceCoverageJob : IJob
    {
      [ReadOnly]
      public Entity m_Source;
      [ReadOnly]
      public Entity m_Target;
      public BufferLookup<Game.Pathfind.CoverageElement> m_CoverageElements;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CoverageElements.HasBuffer(this.m_Source) || !this.m_CoverageElements.HasBuffer(this.m_Target))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CoverageElements[this.m_Target].CopyFrom(this.m_CoverageElements[this.m_Source]);
      }
    }

    [BurstCompile]
    public struct SetupCoverageSearchJob : IJob
    {
      [ReadOnly]
      public Entity m_Entity;
      [ReadOnly]
      public ComponentLookup<BackSide> m_BackSideData;
      [ReadOnly]
      public ComponentLookup<CoverageData> m_PrefabCoverageData;
      public CoverageAction m_Action;
      public PathfindTargetSeeker<PathfindTargetBuffer> m_TargetSeeker;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_TargetSeeker.m_PrefabRef[this.m_Entity];
        CoverageData componentData1;
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabCoverageData.TryGetComponent(prefabRef.m_Prefab, out componentData1);
        Building componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_TargetSeeker.m_Building.TryGetComponent(this.m_Entity, out componentData2))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Transform transform = this.m_TargetSeeker.m_Transform[this.m_Entity];
          if (componentData2.m_RoadEdge != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            Game.Prefabs.BuildingData buildingData = this.m_TargetSeeker.m_BuildingData[prefabRef.m_Prefab];
            float3 comparePosition = transform.m_Position;
            Owner componentData3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_TargetSeeker.m_Owner.TryGetComponent(componentData2.m_RoadEdge, out componentData3) || componentData3.m_Owner != this.m_Entity)
              comparePosition = BuildingUtils.CalculateFrontPosition(transform, buildingData.m_LotSize.y);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Random random = this.m_TargetSeeker.m_RandomSeed.GetRandom(this.m_Entity.Index);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_TargetSeeker.AddEdgeTargets(ref random, this.m_Entity, 0.0f, EdgeFlags.DefaultMask, componentData2.m_RoadEdge, comparePosition, 0.0f, true, false);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_TargetSeeker.FindTargets(this.m_Entity, 0.0f);
        }
        BackSide componentData4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_BackSideData.TryGetComponent(this.m_Entity, out componentData4))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Transform transform = this.m_TargetSeeker.m_Transform[this.m_Entity];
          if (componentData4.m_RoadEdge != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            Game.Prefabs.BuildingData buildingData = this.m_TargetSeeker.m_BuildingData[prefabRef.m_Prefab];
            float3 comparePosition = transform.m_Position;
            Owner componentData5;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_TargetSeeker.m_Owner.TryGetComponent(componentData4.m_RoadEdge, out componentData5) || componentData5.m_Owner != this.m_Entity)
              comparePosition = BuildingUtils.CalculateFrontPosition(transform, -buildingData.m_LotSize.y);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Random random = this.m_TargetSeeker.m_RandomSeed.GetRandom(this.m_Entity.Index);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_TargetSeeker.AddEdgeTargets(ref random, this.m_Entity, 0.0f, EdgeFlags.DefaultMask, componentData4.m_RoadEdge, comparePosition, 0.0f, true, false);
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Action.data.m_Parameters = new CoverageParameters()
        {
          m_Methods = this.m_TargetSeeker.m_PathfindParameters.m_Methods,
          m_Range = componentData1.m_Range
        };
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<InfoviewCoverageData> __Game_Prefabs_InfoviewCoverageData_RO_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public SharedComponentTypeHandle<CoverageServiceType> __Game_Net_CoverageServiceType_SharedComponentTypeHandle;
      public BufferTypeHandle<Game.Net.ServiceCoverage> __Game_Net_ServiceCoverage_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<BackSide> __Game_Buildings_BackSide_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CoverageData> __Game_Prefabs_CoverageData_RO_ComponentLookup;
      public BufferLookup<Game.Pathfind.CoverageElement> __Game_Pathfind_CoverageElement_RW_BufferLookup;
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
        this.__Game_Prefabs_InfoviewCoverageData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewCoverageData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CoverageServiceType_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<CoverageServiceType>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ServiceCoverage_RW_BufferTypeHandle = state.GetBufferTypeHandle<Game.Net.ServiceCoverage>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_BackSide_RO_ComponentLookup = state.GetComponentLookup<BackSide>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CoverageData_RO_ComponentLookup = state.GetComponentLookup<CoverageData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_CoverageElement_RW_BufferLookup = state.GetBufferLookup<Game.Pathfind.CoverageElement>();
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
