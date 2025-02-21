// Decompiled with JetBrains decompiler
// Type: Game.Tools.ToolFeedbackSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Net;
using Game.Notifications;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Serialization;
using Game.Simulation;
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
namespace Game.Tools
{
  [CompilerGenerated]
  public class ToolFeedbackSystem : GameSystemBase, IPostDeserialize
  {
    private const float INFINITE_RANGE = 25000f;
    private IconCommandSystem m_IconCommandSystem;
    private PathfindQueueSystem m_PathfindQueueSystem;
    private AirwaySystem m_AirwaySystem;
    private SimulationSystem m_SimulationSystem;
    private TelecomCoverageSystem m_TelecomCoverageSystem;
    private List<Entity> m_FeedbackContainers;
    private List<Entity> m_PendingContainers;
    private NativeParallelHashMap<ToolFeedbackSystem.RecentKey, ToolFeedbackSystem.RecentValue> m_RecentMap;
    private PathfindTargetSeekerData m_TargetSeekerData;
    private EntityQuery m_ConfigurationQuery;
    private EntityQuery m_AppliedQuery;
    private EntityQuery m_TargetQuery;
    private EntityQuery m_EventQuery;
    private JobHandle m_RecentDeps;
    private ToolFeedbackSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindQueueSystem = this.World.GetOrCreateSystemManaged<PathfindQueueSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirwaySystem = this.World.GetOrCreateSystemManaged<AirwaySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomCoverageSystem = this.World.GetOrCreateSystemManaged<TelecomCoverageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_FeedbackContainers = new List<Entity>();
      // ISSUE: reference to a compiler-generated field
      this.m_PendingContainers = new List<Entity>();
      // ISSUE: reference to a compiler-generated field
      this.m_RecentMap = new NativeParallelHashMap<ToolFeedbackSystem.RecentKey, ToolFeedbackSystem.RecentValue>(1000, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_TargetSeekerData = new PathfindTargetSeekerData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<FeedbackConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Applied>(),
          ComponentType.ReadOnly<Created>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>(),
          ComponentType.ReadOnly<Game.Routes.TransportStop>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>(),
          ComponentType.ReadOnly<Game.Routes.TransportStop>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Abandoned>(),
          ComponentType.ReadOnly<Condemned>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_TargetQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.Exclude<Game.Buildings.ServiceUpgrade>(), ComponentType.Exclude<Abandoned>(), ComponentType.Exclude<Condemned>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Updated>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_EventQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Common.Event>(), ComponentType.ReadOnly<CoverageUpdated>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_RecentDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_RecentMap.Dispose();
      base.OnDestroy();
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_RecentDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_RecentMap.Clear();
      int num;
      // ISSUE: reference to a compiler-generated field
      for (int index1 = 0; index1 < this.m_PendingContainers.Count; index1 = num + 1)
      {
        // ISSUE: reference to a compiler-generated field
        Entity pendingContainer = this.m_PendingContainers[index1];
        // ISSUE: reference to a compiler-generated field
        List<Entity> pendingContainers = this.m_PendingContainers;
        int index2 = index1;
        num = index2 - 1;
        pendingContainers.RemoveAtSwapBack<Entity>(index2);
        // ISSUE: reference to a compiler-generated field
        this.m_FeedbackContainers.Add(pendingContainer);
      }
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      this.Enabled = mode.IsGame();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_AppliedQuery.IsEmptyIgnoreFilter && !this.m_ConfigurationQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated method
        this.ProcessModifications();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_PendingContainers.Count == 0 || this.m_EventQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated method
      this.UpdatePending();
    }

    private void ProcessModifications()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_AppliedQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      PathfindParameters pathfindParameters = new PathfindParameters();
      pathfindParameters.m_MaxSpeed = (float2) 111.111115f;
      pathfindParameters.m_WalkSpeed = (float2) 5.555556f;
      pathfindParameters.m_Weights = new PathfindWeights(1f, 1f, 1f, 1f);
      pathfindParameters.m_PathfindFlags = PathfindFlags.Stable | PathfindFlags.IgnoreFlow;
      pathfindParameters.m_IgnoredRules = RuleFlags.HasBlockage | RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic;
      SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
      EntityManager entityManager;
      // ISSUE: reference to a compiler-generated field
      while (this.m_FeedbackContainers.Count < entityArray.Length)
      {
        // ISSUE: reference to a compiler-generated field
        List<Entity> feedbackContainers = this.m_FeedbackContainers;
        entityManager = this.EntityManager;
        Entity entity = entityManager.CreateEntity(ComponentType.ReadWrite<Feedback>(), ComponentType.ReadWrite<ExtraFeedback>(), ComponentType.ReadWrite<Game.Pathfind.CoverageElement>());
        feedbackContainers.Add(entity);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TargetSeekerData.Update((SystemBase) this, this.m_AirwaySystem.GetAirwayData());
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
      ToolFeedbackSystem.SetupCoverageSearchJob jobData = new ToolFeedbackSystem.SetupCoverageSearchJob()
      {
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabCoverageData = this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentLookup
      };
      FeedbackConfigurationData configurationData = new FeedbackConfigurationData();
      IconCommandBuffer iconCommandBuffer = new IconCommandBuffer();
      JobHandle job0 = new JobHandle();
      bool flag1 = false;
      for (int index1 = 0; index1 < entityArray.Length; ++index1)
      {
        Entity entity1 = entityArray[index1];
        Entity entity2 = entity1;
        Owner component1;
        while (this.EntityManager.TryGetComponent<Owner>(entity2, out component1))
          entity2 = component1.m_Owner;
        entityManager = this.EntityManager;
        bool flag2 = entityManager.HasComponent<Deleted>(entity1);
        if (entity2 != entity1)
        {
          if (flag2)
          {
            entityManager = this.EntityManager;
            if (entityManager.HasComponent<Deleted>(entity2))
              continue;
          }
          else
          {
            entityManager = this.EntityManager;
            if (entityManager.HasComponent<Applied>(entity2))
              continue;
          }
        }
        entityManager = this.EntityManager;
        Transform componentData1 = entityManager.GetComponentData<Transform>(entity2);
        entityManager = this.EntityManager;
        PrefabRef componentData2 = entityManager.GetComponentData<PrefabRef>(entity1);
        entityManager = this.EntityManager;
        PrefabRef componentData3 = entityManager.GetComponentData<PrefabRef>(entity2);
        entityManager = this.EntityManager;
        if (entityManager.HasComponent<SpawnableBuildingData>(componentData2.m_Prefab))
        {
          if (flag2)
          {
            if (configurationData.m_HappyFaceNotification == Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              configurationData = this.m_ConfigurationQuery.GetSingleton<FeedbackConfigurationData>();
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              iconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer();
            }
            iconCommandBuffer.Add(entity1, configurationData.m_SadFaceNotification, clusterLayer: IconClusterLayer.Transaction);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity feedbackContainer = this.m_FeedbackContainers[this.m_FeedbackContainers.Count - 1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_FeedbackContainers.RemoveAt(this.m_FeedbackContainers.Count - 1);
          // ISSUE: reference to a compiler-generated field
          this.m_PendingContainers.Add(feedbackContainer);
          entityManager = this.EntityManager;
          entityManager.SetComponentData<Feedback>(feedbackContainer, new Feedback()
          {
            m_Position = componentData1.m_Position,
            m_MainEntity = entity2,
            m_Prefab = componentData2.m_Prefab,
            m_MainPrefab = componentData3.m_Prefab,
            m_IsDeleted = flag2
          });
          entityManager = this.EntityManager;
          DynamicBuffer<ExtraFeedback> buffer1 = entityManager.GetBuffer<ExtraFeedback>(feedbackContainer);
          buffer1.Clear();
          DynamicBuffer<InstalledUpgrade> buffer2;
          if (this.EntityManager.TryGetBuffer<InstalledUpgrade>(entity2, true, out buffer2))
          {
            for (int index2 = 0; index2 < buffer2.Length; ++index2)
            {
              InstalledUpgrade installedUpgrade = buffer2[index2];
              if (!BuildingUtils.CheckOption(installedUpgrade, BuildingOption.Inactive))
              {
                ref DynamicBuffer<ExtraFeedback> local1 = ref buffer1;
                ExtraFeedback extraFeedback = new ExtraFeedback();
                ref ExtraFeedback local2 = ref extraFeedback;
                entityManager = this.EntityManager;
                Entity prefab = entityManager.GetComponentData<PrefabRef>(installedUpgrade.m_Upgrade).m_Prefab;
                local2.m_Prefab = prefab;
                ExtraFeedback elem = extraFeedback;
                local1.Add(elem);
              }
            }
          }
          entityManager = this.EntityManager;
          entityManager.GetBuffer<Game.Pathfind.CoverageElement>(feedbackContainer).Clear();
          CoverageServiceType component2;
          if (!this.EntityManager.TryGetSharedComponent<CoverageServiceType>(entity2, out component2))
            component2.m_Service = CoverageService.Count;
          // ISSUE: reference to a compiler-generated method
          Game.Simulation.ServiceCoverageSystem.SetupPathfindMethods(component2.m_Service, ref pathfindParameters, ref setupQueueTarget);
          CoverageAction action = new CoverageAction(Allocator.Persistent);
          // ISSUE: reference to a compiler-generated field
          jobData.m_Entity = entity2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          jobData.m_TargetSeeker = new PathfindTargetSeeker<PathfindTargetBuffer>(this.m_TargetSeekerData, pathfindParameters, setupQueueTarget, (PathfindTargetBuffer) action.data.m_Sources.AsParallelWriter(), RandomSeed.Next());
          // ISSUE: reference to a compiler-generated field
          jobData.m_Action = action;
          JobHandle jobHandle = jobData.Schedule<ToolFeedbackSystem.SetupCoverageSearchJob>(this.Dependency);
          job0 = JobHandle.CombineDependencies(job0, jobHandle);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PathfindQueueSystem.Enqueue(action, feedbackContainer, jobHandle, uint.MaxValue, (object) this, new PathEventData(), true);
          flag1 = true;
        }
      }
      entityArray.Dispose();
      if (!flag1)
        return;
      this.Dependency = job0;
    }

    private void UpdatePending()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<CoverageUpdated> componentDataArray = this.m_EventQuery.ToComponentDataArray<CoverageUpdated>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      Entity singletonEntity = this.m_ConfigurationQuery.GetSingletonEntity();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CityModifierData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LocalModifierData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AttractionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PollutionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrisonData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PoliceStationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_FireStationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MaintenanceDepotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SchoolData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TelecomFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PostFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportStopData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CargoTransportStationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PublicTransportStationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportStationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WastewaterTreatmentPlantData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SewageOutletData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterPumpingStationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransformerData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SolarPoweredData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WindPoweredData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PowerPlantData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DeathcareFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HospitalData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ToolFeedbackSystem.TargetCheckJob jobData1 = new ToolFeedbackSystem.TargetCheckJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_GarbageProducerType = this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentTypeHandle,
        m_ElectricityConsumerType = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle,
        m_WaterConsumerType = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentTypeHandle,
        m_MailProducerType = this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentTypeHandle,
        m_CrimeProducerType = this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentTypeHandle,
        m_PrefabCoverageData = this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentLookup,
        m_PrefabGarbageFacilityData = this.__TypeHandle.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup,
        m_PrefabHospitalData = this.__TypeHandle.__Game_Prefabs_HospitalData_RO_ComponentLookup,
        m_PrefabDeathcareFacilityData = this.__TypeHandle.__Game_Prefabs_DeathcareFacilityData_RO_ComponentLookup,
        m_PrefabPowerPlantData = this.__TypeHandle.__Game_Prefabs_PowerPlantData_RO_ComponentLookup,
        m_PrefabWindPoweredData = this.__TypeHandle.__Game_Prefabs_WindPoweredData_RO_ComponentLookup,
        m_PrefabSolarPoweredData = this.__TypeHandle.__Game_Prefabs_SolarPoweredData_RO_ComponentLookup,
        m_PrefabTransformerData = this.__TypeHandle.__Game_Prefabs_TransformerData_RO_ComponentLookup,
        m_PrefabWaterPumpingStationData = this.__TypeHandle.__Game_Prefabs_WaterPumpingStationData_RO_ComponentLookup,
        m_PrefabSewageOutletData = this.__TypeHandle.__Game_Prefabs_SewageOutletData_RO_ComponentLookup,
        m_PrefabWastewaterTreatmentPlantData = this.__TypeHandle.__Game_Prefabs_WastewaterTreatmentPlantData_RO_ComponentLookup,
        m_PrefabTransportDepotData = this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentLookup,
        m_PrefabTransportStationData = this.__TypeHandle.__Game_Prefabs_TransportStationData_RO_ComponentLookup,
        m_PrefabPublicTransportStationData = this.__TypeHandle.__Game_Prefabs_PublicTransportStationData_RO_ComponentLookup,
        m_PrefabCargoTransportStationData = this.__TypeHandle.__Game_Prefabs_CargoTransportStationData_RO_ComponentLookup,
        m_PrefabTransportStopData = this.__TypeHandle.__Game_Prefabs_TransportStopData_RO_ComponentLookup,
        m_PrefabPostFacilityData = this.__TypeHandle.__Game_Prefabs_PostFacilityData_RO_ComponentLookup,
        m_PrefabTelecomFacilityData = this.__TypeHandle.__Game_Prefabs_TelecomFacilityData_RO_ComponentLookup,
        m_PrefabSchoolData = this.__TypeHandle.__Game_Prefabs_SchoolData_RO_ComponentLookup,
        m_PrefabParkingFacilityData = this.__TypeHandle.__Game_Prefabs_ParkingFacilityData_RO_ComponentLookup,
        m_PrefabMaintenanceDepotData = this.__TypeHandle.__Game_Prefabs_MaintenanceDepotData_RO_ComponentLookup,
        m_PrefabFireStationData = this.__TypeHandle.__Game_Prefabs_FireStationData_RO_ComponentLookup,
        m_PrefabPoliceStationData = this.__TypeHandle.__Game_Prefabs_PoliceStationData_RO_ComponentLookup,
        m_PrefabPrisonData = this.__TypeHandle.__Game_Prefabs_PrisonData_RO_ComponentLookup,
        m_PrefabPollutionData = this.__TypeHandle.__Game_Prefabs_PollutionData_RO_ComponentLookup,
        m_PrefabAttractionData = this.__TypeHandle.__Game_Prefabs_AttractionData_RO_ComponentLookup,
        m_ServiceCoverages = this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup,
        m_PrefabLocalModifierDatas = this.__TypeHandle.__Game_Prefabs_LocalModifierData_RO_BufferLookup,
        m_PrefabCityModifierDatas = this.__TypeHandle.__Game_Prefabs_CityModifierData_RO_BufferLookup,
        m_FeedbackConfigurationData = this.EntityManager.GetComponentData<FeedbackConfigurationData>(singletonEntity),
        m_FeedbackLocalEffectFactors = this.EntityManager.GetBuffer<FeedbackLocalEffectFactor>(singletonEntity, true),
        m_FeedbackCityEffectFactors = this.EntityManager.GetBuffer<FeedbackCityEffectFactor>(singletonEntity, true),
        m_RecentMap = this.m_RecentMap
      };
      NativeQueue<ToolFeedbackSystem.RecentUpdate> nativeQueue = new NativeQueue<ToolFeedbackSystem.RecentUpdate>();
      JobHandle jobHandle = new JobHandle();
      bool flag = false;
      EntityManager entityManager;
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_PendingContainers.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity pendingContainer = this.m_PendingContainers[index];
        entityManager = this.EntityManager;
        DynamicBuffer<Game.Pathfind.CoverageElement> buffer1 = entityManager.GetBuffer<Game.Pathfind.CoverageElement>(pendingContainer, true);
        if (buffer1.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_PendingContainers.RemoveAtSwapBack<Entity>(index--);
          // ISSUE: reference to a compiler-generated field
          this.m_FeedbackContainers.Add(pendingContainer);
          NativeParallelHashMap<Entity, float2> nativeParallelHashMap = new NativeParallelHashMap<Entity, float2>(buffer1.Length, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          ToolFeedbackSystem.FillCoverageMapJob jobData2 = new ToolFeedbackSystem.FillCoverageMapJob()
          {
            m_CoverageElements = buffer1.AsNativeArray(),
            m_CoverageMap = nativeParallelHashMap.AsParallelWriter()
          };
          ref ToolFeedbackSystem.TargetCheckJob local1 = ref jobData1;
          entityManager = this.EntityManager;
          Feedback componentData = entityManager.GetComponentData<Feedback>(pendingContainer);
          // ISSUE: reference to a compiler-generated field
          local1.m_FeedbackData = componentData;
          ref ToolFeedbackSystem.TargetCheckJob local2 = ref jobData1;
          entityManager = this.EntityManager;
          DynamicBuffer<ExtraFeedback> buffer2 = entityManager.GetBuffer<ExtraFeedback>(pendingContainer, true);
          // ISSUE: reference to a compiler-generated field
          local2.m_ExtraFeedbacks = buffer2;
          // ISSUE: reference to a compiler-generated field
          jobData1.m_RandomSeed = RandomSeed.Next();
          // ISSUE: reference to a compiler-generated field
          jobData1.m_CoverageMap = nativeParallelHashMap;
          if (!flag)
          {
            nativeQueue = new NativeQueue<ToolFeedbackSystem.RecentUpdate>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
            JobHandle dependencies;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            jobData1.m_TelecomCoverageData = this.m_TelecomCoverageSystem.GetData(true, out dependencies);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            jobData1.m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer();
            // ISSUE: reference to a compiler-generated field
            jobData1.m_RecentUpdates = nativeQueue.AsParallelWriter();
            // ISSUE: reference to a compiler-generated field
            jobHandle = JobHandle.CombineDependencies(this.Dependency, this.m_RecentDeps, dependencies);
          }
          int length = buffer1.Length;
          JobHandle dependsOn = new JobHandle();
          JobHandle job1 = jobData2.Schedule<ToolFeedbackSystem.FillCoverageMapJob>(length, 4, dependsOn);
          // ISSUE: reference to a compiler-generated field
          JobHandle inputDeps = jobData1.ScheduleParallel<ToolFeedbackSystem.TargetCheckJob>(this.m_TargetQuery, JobHandle.CombineDependencies(jobHandle, job1));
          nativeParallelHashMap.Dispose(inputDeps);
          jobHandle = inputDeps;
          flag = true;
        }
      }
      for (int index1 = 0; index1 < componentDataArray.Length; ++index1)
      {
        CoverageUpdated coverageUpdated = componentDataArray[index1];
        // ISSUE: reference to a compiler-generated field
        for (int index2 = 0; index2 < this.m_PendingContainers.Count; ++index2)
        {
          // ISSUE: reference to a compiler-generated field
          Entity pendingContainer = this.m_PendingContainers[index2];
          if (pendingContainer == coverageUpdated.m_Owner)
          {
            // ISSUE: reference to a compiler-generated field
            List<Entity> pendingContainers = this.m_PendingContainers;
            int index3 = index2;
            int num = index3 - 1;
            pendingContainers.RemoveAtSwapBack<Entity>(index3);
            // ISSUE: reference to a compiler-generated field
            this.m_FeedbackContainers.Add(pendingContainer);
            ref ToolFeedbackSystem.TargetCheckJob local3 = ref jobData1;
            entityManager = this.EntityManager;
            Feedback componentData = entityManager.GetComponentData<Feedback>(pendingContainer);
            // ISSUE: reference to a compiler-generated field
            local3.m_FeedbackData = componentData;
            ref ToolFeedbackSystem.TargetCheckJob local4 = ref jobData1;
            entityManager = this.EntityManager;
            DynamicBuffer<ExtraFeedback> buffer = entityManager.GetBuffer<ExtraFeedback>(pendingContainer, true);
            // ISSUE: reference to a compiler-generated field
            local4.m_ExtraFeedbacks = buffer;
            // ISSUE: reference to a compiler-generated field
            jobData1.m_RandomSeed = RandomSeed.Next();
            // ISSUE: reference to a compiler-generated field
            jobData1.m_CoverageMap = new NativeParallelHashMap<Entity, float2>();
            if (!flag)
            {
              nativeQueue = new NativeQueue<ToolFeedbackSystem.RecentUpdate>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
              JobHandle dependencies;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              jobData1.m_TelecomCoverageData = this.m_TelecomCoverageSystem.GetData(true, out dependencies);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              jobData1.m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer();
              // ISSUE: reference to a compiler-generated field
              jobData1.m_RecentUpdates = nativeQueue.AsParallelWriter();
              // ISSUE: reference to a compiler-generated field
              jobHandle = JobHandle.CombineDependencies(this.Dependency, this.m_RecentDeps, dependencies);
            }
            // ISSUE: reference to a compiler-generated field
            jobHandle = jobData1.ScheduleParallel<ToolFeedbackSystem.TargetCheckJob>(this.m_TargetQuery, jobHandle);
            flag = true;
            break;
          }
        }
      }
      componentDataArray.Dispose();
      if (!flag)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomCoverageSystem.AddReader(jobHandle);
      this.Dependency = jobHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ToolFeedbackSystem.UpdateRecentMapJob jobData3 = new ToolFeedbackSystem.UpdateRecentMapJob()
      {
        m_RecentMap = this.m_RecentMap,
        m_RecentUpdates = nativeQueue,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex
      };
      // ISSUE: reference to a compiler-generated field
      this.m_RecentDeps = jobData3.Schedule<ToolFeedbackSystem.UpdateRecentMapJob>(jobHandle);
      // ISSUE: reference to a compiler-generated field
      nativeQueue.Dispose(this.m_RecentDeps);
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
    public ToolFeedbackSystem()
    {
    }

    private struct RecentKey : IEquatable<ToolFeedbackSystem.RecentKey>
    {
      private Entity m_Entity;
      private int m_Type;

      public RecentKey(Entity entity, CoverageService coverageService)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Entity = entity;
        // ISSUE: reference to a compiler-generated field
        this.m_Type = (int) (coverageService | CoverageService.Healthcare);
      }

      public RecentKey(Entity entity, ToolFeedbackSystem.FeedbackType feedbackType)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Entity = entity;
        // ISSUE: reference to a compiler-generated field
        this.m_Type = (int) (feedbackType | ToolFeedbackSystem.FeedbackType.GarbageVehicles);
      }

      public RecentKey(Entity entity, LocalModifierType localModifierType)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Entity = entity;
        // ISSUE: reference to a compiler-generated field
        this.m_Type = (int) (localModifierType | (LocalModifierType) 768);
      }

      public RecentKey(Entity entity, CityModifierType cityModifierType)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Entity = entity;
        // ISSUE: reference to a compiler-generated field
        this.m_Type = (int) (cityModifierType | (CityModifierType) 1024);
      }

      public RecentKey(Entity entity, MaintenanceType maintenanceType)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Entity = entity;
        // ISSUE: reference to a compiler-generated field
        this.m_Type = (int) (maintenanceType | MaintenanceType.None);
      }

      public RecentKey(Entity entity, TransportType transportType)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Entity = entity;
        // ISSUE: reference to a compiler-generated field
        this.m_Type = (int) (transportType | (TransportType) 1536);
      }

      public bool Equals(ToolFeedbackSystem.RecentKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Entity == other.m_Entity && this.m_Type == other.m_Type;
      }

      public override int GetHashCode() => this.m_Entity.GetHashCode() * 1792 + this.m_Type;
    }

    private struct RecentValue
    {
      public uint m_UpdateFrame;
      public float m_FeedbackDelta;
    }

    private struct RecentUpdate
    {
      public ToolFeedbackSystem.RecentKey m_Key;
      public float m_Delta;
    }

    private enum FeedbackType : byte
    {
      GarbageVehicles,
      HospitalAmbulances,
      HospitalHelicopters,
      HospitalCapacity,
      DeathcareHearses,
      DeathcareCapacity,
      Electricity,
      Transformer,
      WaterCapacity,
      SewageCapacity,
      TransportDispatch,
      PublicTransport,
      CargoTransport,
      GroundPollution,
      AirPollution,
      NoisePollution,
      PostFacilityVehicles,
      PostFacilityCapacity,
      TelecomCoverage,
      ElementarySchoolCapacity,
      HighSchoolCapacity,
      CollegeCapacity,
      UniversityCapacity,
      ParkingSpaces,
      FireStationEngines,
      FireStationHelicopters,
      PoliceStationCars,
      PoliceStationHelicopters,
      PoliceStationCapacity,
      PrisonVehicles,
      PrisonCapacity,
      Attractiveness,
    }

    [BurstCompile]
    private struct SetupCoverageSearchJob : IJob
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
        else
          coverageData.m_Range = 25000f;
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

    [BurstCompile]
    private struct FillCoverageMapJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeArray<Game.Pathfind.CoverageElement> m_CoverageElements;
      public NativeParallelHashMap<Entity, float2>.ParallelWriter m_CoverageMap;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Game.Pathfind.CoverageElement coverageElement = this.m_CoverageElements[index];
        // ISSUE: reference to a compiler-generated field
        this.m_CoverageMap.TryAdd(coverageElement.m_Edge, coverageElement.m_Cost);
      }
    }

    [BurstCompile]
    private struct TargetCheckJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<GarbageProducer> m_GarbageProducerType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityConsumer> m_ElectricityConsumerType;
      [ReadOnly]
      public ComponentTypeHandle<WaterConsumer> m_WaterConsumerType;
      [ReadOnly]
      public ComponentTypeHandle<MailProducer> m_MailProducerType;
      [ReadOnly]
      public ComponentTypeHandle<CrimeProducer> m_CrimeProducerType;
      [ReadOnly]
      public ComponentLookup<CoverageData> m_PrefabCoverageData;
      [ReadOnly]
      public ComponentLookup<GarbageFacilityData> m_PrefabGarbageFacilityData;
      [ReadOnly]
      public ComponentLookup<HospitalData> m_PrefabHospitalData;
      [ReadOnly]
      public ComponentLookup<DeathcareFacilityData> m_PrefabDeathcareFacilityData;
      [ReadOnly]
      public ComponentLookup<PowerPlantData> m_PrefabPowerPlantData;
      [ReadOnly]
      public ComponentLookup<WindPoweredData> m_PrefabWindPoweredData;
      [ReadOnly]
      public ComponentLookup<SolarPoweredData> m_PrefabSolarPoweredData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.TransformerData> m_PrefabTransformerData;
      [ReadOnly]
      public ComponentLookup<WaterPumpingStationData> m_PrefabWaterPumpingStationData;
      [ReadOnly]
      public ComponentLookup<SewageOutletData> m_PrefabSewageOutletData;
      [ReadOnly]
      public ComponentLookup<WastewaterTreatmentPlantData> m_PrefabWastewaterTreatmentPlantData;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> m_PrefabTransportDepotData;
      [ReadOnly]
      public ComponentLookup<TransportStationData> m_PrefabTransportStationData;
      [ReadOnly]
      public ComponentLookup<PublicTransportStationData> m_PrefabPublicTransportStationData;
      [ReadOnly]
      public ComponentLookup<CargoTransportStationData> m_PrefabCargoTransportStationData;
      [ReadOnly]
      public ComponentLookup<TransportStopData> m_PrefabTransportStopData;
      [ReadOnly]
      public ComponentLookup<PostFacilityData> m_PrefabPostFacilityData;
      [ReadOnly]
      public ComponentLookup<TelecomFacilityData> m_PrefabTelecomFacilityData;
      [ReadOnly]
      public ComponentLookup<SchoolData> m_PrefabSchoolData;
      [ReadOnly]
      public ComponentLookup<ParkingFacilityData> m_PrefabParkingFacilityData;
      [ReadOnly]
      public ComponentLookup<MaintenanceDepotData> m_PrefabMaintenanceDepotData;
      [ReadOnly]
      public ComponentLookup<FireStationData> m_PrefabFireStationData;
      [ReadOnly]
      public ComponentLookup<PoliceStationData> m_PrefabPoliceStationData;
      [ReadOnly]
      public ComponentLookup<PrisonData> m_PrefabPrisonData;
      [ReadOnly]
      public ComponentLookup<PollutionData> m_PrefabPollutionData;
      [ReadOnly]
      public ComponentLookup<AttractionData> m_PrefabAttractionData;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> m_ServiceCoverages;
      [ReadOnly]
      public BufferLookup<LocalModifierData> m_PrefabLocalModifierDatas;
      [ReadOnly]
      public BufferLookup<CityModifierData> m_PrefabCityModifierDatas;
      [ReadOnly]
      public Feedback m_FeedbackData;
      [ReadOnly]
      public DynamicBuffer<ExtraFeedback> m_ExtraFeedbacks;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeParallelHashMap<Entity, float2> m_CoverageMap;
      [ReadOnly]
      public NativeParallelHashMap<ToolFeedbackSystem.RecentKey, ToolFeedbackSystem.RecentValue> m_RecentMap;
      [ReadOnly]
      public FeedbackConfigurationData m_FeedbackConfigurationData;
      [ReadOnly]
      public DynamicBuffer<FeedbackLocalEffectFactor> m_FeedbackLocalEffectFactors;
      [ReadOnly]
      public DynamicBuffer<FeedbackCityEffectFactor> m_FeedbackCityEffectFactors;
      [ReadOnly]
      public CellMapData<TelecomCoverage> m_TelecomCoverageData;
      public IconCommandBuffer m_IconCommandBuffer;
      public NativeQueue<ToolFeedbackSystem.RecentUpdate>.ParallelWriter m_RecentUpdates;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray2 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Building> nativeArray3 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<GarbageProducer> nativeArray4 = chunk.GetNativeArray<GarbageProducer>(ref this.m_GarbageProducerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityConsumer> nativeArray5 = chunk.GetNativeArray<ElectricityConsumer>(ref this.m_ElectricityConsumerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterConsumer> nativeArray6 = chunk.GetNativeArray<WaterConsumer>(ref this.m_WaterConsumerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<MailProducer> nativeArray7 = chunk.GetNativeArray<MailProducer>(ref this.m_MailProducerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CrimeProducer> nativeArray8 = chunk.GetNativeArray<CrimeProducer>(ref this.m_CrimeProducerType);
        CoverageData coverageData = new CoverageData();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabCoverageData.HasComponent(this.m_FeedbackData.m_MainPrefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          coverageData = this.m_PrefabCoverageData[this.m_FeedbackData.m_MainPrefab];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_FeedbackData.m_Prefab != this.m_FeedbackData.m_MainPrefab)
          {
            coverageData.m_Magnitude = 1f;
            coverageData.m_Service = CoverageService.Count;
          }
          else
            coverageData.m_Magnitude = 1f / math.max(1f / 1000f, coverageData.m_Magnitude);
        }
        else
        {
          coverageData.m_Range = 25000f;
          coverageData.m_Magnitude = 1f;
          coverageData.m_Service = CoverageService.Count;
        }
        GarbageFacilityData componentData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabGarbageFacilityData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData1);
        HospitalData componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabHospitalData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData2);
        DeathcareFacilityData componentData3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabDeathcareFacilityData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData3);
        PowerPlantData componentData4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabPowerPlantData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData4);
        WindPoweredData componentData5;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabWindPoweredData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData5);
        SolarPoweredData componentData6;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabSolarPoweredData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData6);
        WaterPumpingStationData componentData7;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabWaterPumpingStationData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData7);
        SewageOutletData componentData8;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabSewageOutletData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData8);
        WastewaterTreatmentPlantData componentData9;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabWastewaterTreatmentPlantData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData9);
        TransportDepotData componentData10;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabTransportDepotData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData10);
        TransportStationData componentData11;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabTransportStationData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData11);
        TransportStopData componentData12;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabTransportStopData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData12);
        PostFacilityData componentData13;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabPostFacilityData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData13);
        TelecomFacilityData componentData14;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabTelecomFacilityData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData14);
        SchoolData componentData15;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabSchoolData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData15);
        ParkingFacilityData componentData16;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag1 = this.m_PrefabParkingFacilityData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData16);
        MaintenanceDepotData componentData17;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabMaintenanceDepotData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData17);
        FireStationData componentData18;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabFireStationData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData18);
        PoliceStationData componentData19;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabPoliceStationData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData19);
        PrisonData componentData20;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabPrisonData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData20);
        PollutionData componentData21;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabPollutionData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData21);
        AttractionData componentData22;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabAttractionData.TryGetComponent(this.m_FeedbackData.m_Prefab, out componentData22);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag2 = this.m_PrefabTransformerData.HasComponent(this.m_FeedbackData.m_Prefab);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag3 = this.m_PrefabPublicTransportStationData.HasComponent(this.m_FeedbackData.m_Prefab);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag4 = this.m_PrefabCargoTransportStationData.HasComponent(this.m_FeedbackData.m_Prefab);
        float num1 = 0.0f;
        NativeList<LocalModifierData> tempModifierList1 = new NativeList<LocalModifierData>();
        NativeList<CityModifierData> tempModifierList2 = new NativeList<CityModifierData>();
        DynamicBuffer<LocalModifierData> bufferData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabLocalModifierDatas.TryGetBuffer(this.m_FeedbackData.m_Prefab, out bufferData1))
        {
          tempModifierList1 = new NativeList<LocalModifierData>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated method
          LocalEffectSystem.InitializeTempList(tempModifierList1, bufferData1);
        }
        DynamicBuffer<CityModifierData> bufferData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabCityModifierDatas.TryGetBuffer(this.m_FeedbackData.m_Prefab, out bufferData2))
        {
          tempModifierList2 = new NativeList<CityModifierData>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated method
          CityModifierUpdateSystem.InitializeTempList(tempModifierList2, bufferData2);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_FeedbackData.m_Prefab == this.m_FeedbackData.m_MainPrefab)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_ExtraFeedbacks.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            Entity prefab = this.m_ExtraFeedbacks[index].m_Prefab;
            GarbageFacilityData componentData23;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabGarbageFacilityData.TryGetComponent(prefab, out componentData23))
              componentData1.Combine(componentData23);
            HospitalData componentData24;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabHospitalData.TryGetComponent(prefab, out componentData24))
              componentData2.Combine(componentData24);
            DeathcareFacilityData componentData25;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabDeathcareFacilityData.TryGetComponent(prefab, out componentData25))
              componentData3.Combine(componentData25);
            PowerPlantData componentData26;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabPowerPlantData.TryGetComponent(prefab, out componentData26))
              componentData4.Combine(componentData26);
            WindPoweredData componentData27;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabWindPoweredData.TryGetComponent(prefab, out componentData27))
              componentData5.Combine(componentData27);
            SolarPoweredData componentData28;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabSolarPoweredData.TryGetComponent(prefab, out componentData28))
              componentData6.Combine(componentData28);
            WaterPumpingStationData componentData29;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabWaterPumpingStationData.TryGetComponent(prefab, out componentData29))
              componentData7.Combine(componentData29);
            SewageOutletData componentData30;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabSewageOutletData.TryGetComponent(prefab, out componentData30))
              componentData8.Combine(componentData30);
            WastewaterTreatmentPlantData componentData31;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabWastewaterTreatmentPlantData.TryGetComponent(prefab, out componentData31))
              componentData9.Combine(componentData31);
            TransportDepotData componentData32;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabTransportDepotData.TryGetComponent(prefab, out componentData32))
              componentData10.Combine(componentData32);
            TransportStationData componentData33;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabTransportStationData.TryGetComponent(prefab, out componentData33))
              componentData11.Combine(componentData33);
            // ISSUE: reference to a compiler-generated field
            flag3 |= this.m_PrefabPublicTransportStationData.HasComponent(prefab);
            // ISSUE: reference to a compiler-generated field
            flag4 |= this.m_PrefabCargoTransportStationData.HasComponent(prefab);
            PostFacilityData componentData34;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabPostFacilityData.TryGetComponent(prefab, out componentData34))
              componentData13.Combine(componentData34);
            TelecomFacilityData componentData35;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabTelecomFacilityData.TryGetComponent(prefab, out componentData35))
              componentData14.Combine(componentData35);
            SchoolData componentData36;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabSchoolData.TryGetComponent(prefab, out componentData36))
              componentData15.Combine(componentData36);
            ParkingFacilityData componentData37;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabParkingFacilityData.TryGetComponent(prefab, out componentData37))
            {
              componentData16.Combine(componentData37);
              flag1 = true;
            }
            MaintenanceDepotData componentData38;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabMaintenanceDepotData.TryGetComponent(prefab, out componentData38))
              componentData17.Combine(componentData38);
            FireStationData componentData39;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabFireStationData.TryGetComponent(prefab, out componentData39))
              componentData18.Combine(componentData39);
            PoliceStationData componentData40;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabPoliceStationData.TryGetComponent(prefab, out componentData40))
              componentData19.Combine(componentData40);
            PrisonData componentData41;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabPrisonData.TryGetComponent(prefab, out componentData41))
              componentData20.Combine(componentData41);
            PollutionData componentData42;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabPollutionData.TryGetComponent(prefab, out componentData42))
              componentData21.Combine(componentData42);
            AttractionData componentData43;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabAttractionData.TryGetComponent(prefab, out componentData43))
              componentData22.Combine(componentData43);
            DynamicBuffer<LocalModifierData> bufferData3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabLocalModifierDatas.TryGetBuffer(this.m_FeedbackData.m_Prefab, out bufferData3))
            {
              // ISSUE: reference to a compiler-generated method
              LocalEffectSystem.AddToTempList(tempModifierList1, bufferData3, false);
            }
            DynamicBuffer<CityModifierData> bufferData4;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabCityModifierDatas.TryGetBuffer(this.m_FeedbackData.m_Prefab, out bufferData4))
            {
              // ISSUE: reference to a compiler-generated method
              CityModifierUpdateSystem.AddToTempList(tempModifierList2, bufferData4);
            }
          }
        }
        else
        {
          bool flag5 = (double) componentData14.m_Range >= 1.0;
          bool flag6 = (double) componentData14.m_NetworkCapacity >= 1.0;
          if (flag5 | flag6)
          {
            TelecomFacilityData componentData44;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabTelecomFacilityData.TryGetComponent(this.m_FeedbackData.m_MainPrefab, out componentData44);
            // ISSUE: reference to a compiler-generated field
            for (int index = 0; index < this.m_ExtraFeedbacks.Length; ++index)
            {
              TelecomFacilityData componentData45;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabTelecomFacilityData.TryGetComponent(this.m_ExtraFeedbacks[index].m_Prefab, out componentData45))
                componentData44.Combine(componentData45);
            }
            if (flag5)
              componentData14.m_NetworkCapacity += componentData44.m_NetworkCapacity;
            componentData14.m_Range += componentData44.m_Range;
            if (flag5 && !flag6)
              num1 = componentData44.m_Range;
          }
          bool flag7 = componentData18.m_FireEngineCapacity != 0;
          bool flag8 = componentData18.m_FireHelicopterCapacity != 0;
          bool flag9 = componentData18.m_DisasterResponseCapacity != 0;
          bool flag10 = (double) componentData18.m_VehicleEfficiency != 0.0;
          if (flag7 | flag8 | flag9 | flag10)
          {
            FireStationData componentData46;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabFireStationData.TryGetComponent(this.m_FeedbackData.m_MainPrefab, out componentData46);
            // ISSUE: reference to a compiler-generated field
            for (int index = 0; index < this.m_ExtraFeedbacks.Length; ++index)
            {
              FireStationData componentData47;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabFireStationData.TryGetComponent(this.m_ExtraFeedbacks[index].m_Prefab, out componentData47))
                componentData46.Combine(componentData47);
            }
            if (flag7 | flag8)
              componentData18.m_VehicleEfficiency += componentData46.m_VehicleEfficiency;
            if (flag9 | flag10)
            {
              componentData18.m_FireEngineCapacity += componentData46.m_FireEngineCapacity;
              componentData18.m_FireHelicopterCapacity += componentData46.m_FireHelicopterCapacity;
            }
          }
          bool flag11 = (double) componentData17.m_VehicleCapacity != 0.0;
          bool flag12 = (double) componentData17.m_VehicleEfficiency != 0.0;
          if (flag11 | flag12)
          {
            MaintenanceDepotData componentData48;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabMaintenanceDepotData.TryGetComponent(this.m_FeedbackData.m_MainPrefab, out componentData48);
            // ISSUE: reference to a compiler-generated field
            for (int index = 0; index < this.m_ExtraFeedbacks.Length; ++index)
            {
              MaintenanceDepotData componentData49;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabMaintenanceDepotData.TryGetComponent(this.m_ExtraFeedbacks[index].m_Prefab, out componentData49))
                componentData48.Combine(componentData49);
            }
            if (flag11)
              componentData17.m_VehicleEfficiency += componentData48.m_VehicleEfficiency;
            if (flag12)
              componentData17.m_VehicleCapacity += componentData48.m_VehicleCapacity;
          }
          if (componentData10.m_DispatchCenter)
          {
            TransportDepotData componentData50;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabTransportDepotData.TryGetComponent(this.m_FeedbackData.m_MainPrefab, out componentData50);
            // ISSUE: reference to a compiler-generated field
            for (int index = 0; index < this.m_ExtraFeedbacks.Length; ++index)
            {
              TransportDepotData componentData51;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabTransportDepotData.TryGetComponent(this.m_ExtraFeedbacks[index].m_Prefab, out componentData51))
                componentData50.Combine(componentData51);
            }
            componentData10.m_VehicleCapacity += componentData50.m_VehicleCapacity;
          }
          if (tempModifierList1.IsCreated)
          {
            DynamicBuffer<LocalModifierData> bufferData5;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabLocalModifierDatas.TryGetBuffer(this.m_FeedbackData.m_MainPrefab, out bufferData5))
            {
              // ISSUE: reference to a compiler-generated method
              this.AddToTempListForUpgrade(tempModifierList1, bufferData5);
            }
            // ISSUE: reference to a compiler-generated field
            for (int index = 0; index < this.m_ExtraFeedbacks.Length; ++index)
            {
              DynamicBuffer<LocalModifierData> bufferData6;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabLocalModifierDatas.TryGetBuffer(this.m_ExtraFeedbacks[index].m_Prefab, out bufferData6))
              {
                // ISSUE: reference to a compiler-generated method
                this.AddToTempListForUpgrade(tempModifierList1, bufferData6);
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        NativeList<ToolFeedbackSystem.RecentUpdate> updateList = new NativeList<ToolFeedbackSystem.RecentUpdate>();
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity entity = nativeArray1[index1];
          Transform transform = nativeArray2[index1];
          Building building = nativeArray3[index1];
          // ISSUE: reference to a compiler-generated field
          if (!(entity == this.m_FeedbackData.m_MainEntity))
          {
            float3 total = (float3) 0.0f;
            float2 float2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_CoverageMap.IsCreated && this.m_CoverageMap.TryGetValue(building.m_RoadEdge, out float2))
            {
              float num2 = math.lerp(float2.x, float2.y, building.m_CurvePosition);
              float x = math.max(0.0f, (float) (1.0 - (double) num2 * (double) num2));
              float distance = num2 * coverageData.m_Range;
              if ((double) x != 0.0)
              {
                DynamicBuffer<Game.Net.ServiceCoverage> bufferData7;
                // ISSUE: reference to a compiler-generated field
                if (coverageData.m_Service != CoverageService.Count && this.m_ServiceCoverages.TryGetBuffer(building.m_RoadEdge, out bufferData7) && bufferData7.Length != 0)
                {
                  Game.Net.ServiceCoverage serviceCoverage = bufferData7[(int) coverageData.m_Service];
                  float num3 = (float) (1.0 + (double) math.lerp(serviceCoverage.m_Coverage.x, serviceCoverage.m_Coverage.y, building.m_CurvePosition) * (double) coverageData.m_Magnitude);
                  float delta = x / math.max(x, num3 * num3);
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, coverageData.m_Service), delta, distance);
                }
                if (componentData1.m_VehicleCapacity != 0 && nativeArray4.Length != 0)
                {
                  GarbageProducer garbageProducer = nativeArray4[index1];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate((float) garbageProducer.m_Garbage * this.m_FeedbackConfigurationData.m_GarbageProducerGarbageFactor) * math.saturate((float) componentData1.m_VehicleCapacity * this.m_FeedbackConfigurationData.m_GarbageVehicleFactor);
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.GarbageVehicles), delta, distance);
                }
                if (componentData2.m_AmbulanceCapacity != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate((float) componentData2.m_AmbulanceCapacity * this.m_FeedbackConfigurationData.m_HospitalAmbulanceFactor);
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.HospitalAmbulances), delta, distance);
                }
                if (componentData2.m_PatientCapacity != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate((float) componentData2.m_PatientCapacity * this.m_FeedbackConfigurationData.m_HospitalCapacityFactor);
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.HospitalCapacity), delta, distance);
                }
                if (componentData3.m_HearseCapacity != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate((float) componentData3.m_HearseCapacity * this.m_FeedbackConfigurationData.m_DeathcareHearseFactor);
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.DeathcareHearses), delta, distance);
                }
                if (componentData3.m_StorageCapacity != 0 || (double) componentData3.m_ProcessingRate != 0.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate((float) ((double) componentData3.m_StorageCapacity * (double) this.m_FeedbackConfigurationData.m_DeathcareCapacityFactor + (double) componentData3.m_ProcessingRate * (double) this.m_FeedbackConfigurationData.m_DeathcareProcessingFactor));
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.DeathcareCapacity), delta, distance);
                }
                if (flag2 && nativeArray5.Length != 0)
                {
                  ElectricityConsumer electricityConsumer = nativeArray5[index1];
                  // ISSUE: reference to a compiler-generated field
                  float a = math.saturate((float) electricityConsumer.m_WantedConsumption * this.m_FeedbackConfigurationData.m_ElectricityConsumptionFactor);
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.select(a, 1f, !electricityConsumer.electricityConnected) * math.saturate((float) (1.0 - (double) distance / (double) this.m_FeedbackConfigurationData.m_TransformerRadius));
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.Transformer), delta, distance);
                }
                if (componentData10.m_DispatchCenter)
                {
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate((float) componentData10.m_VehicleCapacity * this.m_FeedbackConfigurationData.m_TransportDispatchCenterFactor);
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.TransportDispatch), delta, distance);
                }
                if (flag3)
                {
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate((float) (0.5 + (double) componentData11.m_ComfortFactor * 0.5)) * math.saturate((float) (1.0 - (double) distance / (double) this.m_FeedbackConfigurationData.m_TransportStationRange));
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.PublicTransport), delta, distance);
                }
                if (flag4)
                {
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate((float) (0.5 + (double) componentData11.m_LoadingFactor * 0.5)) * math.saturate((float) (1.0 - (double) distance / (double) this.m_FeedbackConfigurationData.m_TransportStationRange));
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.CargoTransport), delta, distance);
                }
                if (componentData12.m_PassengerTransport)
                {
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate((float) (0.5 + (double) componentData12.m_ComfortFactor * 0.5)) * math.saturate((float) (1.0 - (double) distance / (double) this.m_FeedbackConfigurationData.m_TransportStopRange));
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.PublicTransport), delta, distance);
                }
                if ((componentData13.m_PostVanCapacity != 0 || componentData13.m_PostTruckCapacity != 0) && nativeArray7.Length != 0)
                {
                  MailProducer mailProducer = nativeArray7[index1];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate((float) (mailProducer.receivingMail + (int) mailProducer.m_SendingMail) * this.m_FeedbackConfigurationData.m_MailProducerMailFactor) * math.saturate((float) ((double) componentData13.m_PostVanCapacity * (double) this.m_FeedbackConfigurationData.m_PostFacilityVanFactor + (double) componentData13.m_PostTruckCapacity * (double) this.m_FeedbackConfigurationData.m_PostFacilityTruckFactor));
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.PostFacilityVehicles), delta, distance);
                }
                if ((componentData13.m_MailCapacity != 0 || componentData13.m_SortingRate != 0) && nativeArray7.Length != 0)
                {
                  MailProducer mailProducer = nativeArray7[index1];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate((float) (mailProducer.receivingMail + (int) mailProducer.m_SendingMail) * this.m_FeedbackConfigurationData.m_MailProducerMailFactor) * math.saturate((float) ((double) componentData13.m_MailCapacity * (double) this.m_FeedbackConfigurationData.m_PostFacilityCapacityFactor + (double) componentData13.m_SortingRate * (double) this.m_FeedbackConfigurationData.m_PostFacilityProcessingFactor));
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.PostFacilityCapacity), delta, distance);
                }
                if (componentData15.m_StudentCapacity != 0)
                {
                  float num4 = 0.0f;
                  // ISSUE: variable of a compiler-generated type
                  ToolFeedbackSystem.FeedbackType feedbackType = ToolFeedbackSystem.FeedbackType.GarbageVehicles;
                  switch (componentData15.m_EducationLevel)
                  {
                    case 1:
                      // ISSUE: reference to a compiler-generated field
                      num4 = this.m_FeedbackConfigurationData.m_ElementarySchoolCapacityFactor;
                      feedbackType = ToolFeedbackSystem.FeedbackType.ElementarySchoolCapacity;
                      break;
                    case 2:
                      // ISSUE: reference to a compiler-generated field
                      num4 = this.m_FeedbackConfigurationData.m_HighSchoolCapacityFactor;
                      feedbackType = ToolFeedbackSystem.FeedbackType.HighSchoolCapacity;
                      break;
                    case 3:
                      // ISSUE: reference to a compiler-generated field
                      num4 = this.m_FeedbackConfigurationData.m_CollegeCapacityFactor;
                      feedbackType = ToolFeedbackSystem.FeedbackType.CollegeCapacity;
                      break;
                    case 4:
                      // ISSUE: reference to a compiler-generated field
                      num4 = this.m_FeedbackConfigurationData.m_UniversityCapacityFactor;
                      feedbackType = ToolFeedbackSystem.FeedbackType.UniversityCapacity;
                      break;
                  }
                  if ((double) num4 != 0.0)
                  {
                    float delta = x * math.saturate((float) componentData15.m_StudentCapacity * num4);
                    // ISSUE: object of a compiler-generated type is created
                    // ISSUE: reference to a compiler-generated method
                    this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, feedbackType), delta, distance);
                  }
                }
                if (flag1)
                {
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate((float) (0.5 + (double) componentData16.m_ComfortFactor * 0.5)) * math.saturate((float) (1.0 - (double) distance / (double) this.m_FeedbackConfigurationData.m_ParkingFacilityRange));
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.ParkingSpaces), delta, distance);
                }
                if (componentData17.m_VehicleCapacity != 0)
                {
                  float num5 = (float) componentData17.m_VehicleCapacity * componentData17.m_VehicleEfficiency;
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate(num5 * this.m_FeedbackConfigurationData.m_MaintenanceVehicleFactor);
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, componentData17.m_MaintenanceType), delta, distance);
                }
                if (componentData18.m_FireEngineCapacity != 0)
                {
                  float num6 = (float) componentData18.m_FireEngineCapacity * componentData18.m_VehicleEfficiency + (float) math.min(componentData18.m_FireEngineCapacity, componentData18.m_DisasterResponseCapacity);
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate(num6 * this.m_FeedbackConfigurationData.m_FireStationEngineFactor);
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.FireStationEngines), delta, distance);
                }
                if (componentData19.m_PatrolCarCapacity != 0 && nativeArray8.Length != 0)
                {
                  CrimeProducer crimeProducer = nativeArray8[index1];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate(crimeProducer.m_Crime * this.m_FeedbackConfigurationData.m_CrimeProducerCrimeFactor) * math.saturate((float) componentData19.m_PatrolCarCapacity * this.m_FeedbackConfigurationData.m_PoliceStationCarFactor);
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.PoliceStationCars), delta, distance);
                }
                if (componentData19.m_JailCapacity != 0 && nativeArray8.Length != 0)
                {
                  CrimeProducer crimeProducer = nativeArray8[index1];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate(crimeProducer.m_Crime * this.m_FeedbackConfigurationData.m_CrimeProducerCrimeFactor) * math.saturate((float) componentData19.m_JailCapacity * this.m_FeedbackConfigurationData.m_PoliceStationCapacityFactor);
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.PoliceStationCapacity), delta, distance);
                }
                if (componentData20.m_PrisonVanCapacity != 0 && nativeArray8.Length != 0)
                {
                  CrimeProducer crimeProducer = nativeArray8[index1];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate(crimeProducer.m_Crime * this.m_FeedbackConfigurationData.m_CrimeProducerCrimeFactor) * math.saturate((float) componentData20.m_PrisonVanCapacity * this.m_FeedbackConfigurationData.m_PrisonVehicleFactor);
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.PrisonVehicles), delta, distance);
                }
                if (componentData20.m_PrisonerCapacity != 0 && nativeArray8.Length != 0)
                {
                  CrimeProducer crimeProducer = nativeArray8[index1];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float delta = x * math.saturate(crimeProducer.m_Crime * this.m_FeedbackConfigurationData.m_CrimeProducerCrimeFactor) * math.saturate((float) componentData20.m_PrisonerCapacity * this.m_FeedbackConfigurationData.m_PrisonCapacityFactor);
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: reference to a compiler-generated method
                  this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.PrisonCapacity), delta, distance);
                }
              }
            }
            float num7 = 1f;
            // ISSUE: reference to a compiler-generated field
            float distance1 = math.distance(transform.m_Position.xz, this.m_FeedbackData.m_Position.xz);
            if (componentData2.m_MedicalHelicopterCapacity != 0)
            {
              // ISSUE: reference to a compiler-generated field
              float delta = num7 * math.saturate((float) componentData2.m_MedicalHelicopterCapacity * this.m_FeedbackConfigurationData.m_HospitalHelicopterFactor);
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: reference to a compiler-generated method
              this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.HospitalHelicopters), delta, distance1);
            }
            if ((componentData4.m_ElectricityProduction != 0 || componentData5.m_Production != 0 || componentData6.m_Production != 0) && nativeArray5.Length != 0)
            {
              ElectricityConsumer electricityConsumer = nativeArray5[index1];
              // ISSUE: reference to a compiler-generated field
              float a = math.saturate((float) electricityConsumer.m_WantedConsumption * this.m_FeedbackConfigurationData.m_ElectricityConsumptionFactor);
              float num8 = (float) (componentData4.m_ElectricityProduction + componentData5.m_Production + componentData6.m_Production);
              // ISSUE: reference to a compiler-generated field
              float delta = num7 * math.select(a, 1f, !electricityConsumer.electricityConnected) * math.saturate(num8 * this.m_FeedbackConfigurationData.m_ElectricityProductionFactor);
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: reference to a compiler-generated method
              this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.Electricity), delta, distance1);
            }
            if (componentData7.m_Capacity != 0 && nativeArray6.Length != 0)
            {
              WaterConsumer waterConsumer = nativeArray6[index1];
              // ISSUE: reference to a compiler-generated field
              float a = math.saturate((float) waterConsumer.m_WantedConsumption * this.m_FeedbackConfigurationData.m_WaterConsumptionFactor);
              float capacity = (float) componentData7.m_Capacity;
              // ISSUE: reference to a compiler-generated field
              float delta = num7 * math.select(a, 1f, !waterConsumer.waterConnected) * math.saturate(capacity * this.m_FeedbackConfigurationData.m_WaterCapacityFactor);
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: reference to a compiler-generated method
              this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.WaterCapacity), delta, distance1);
            }
            if ((componentData8.m_Capacity != 0 || componentData9.m_Capacity != 0) && nativeArray6.Length != 0)
            {
              WaterConsumer waterConsumer = nativeArray6[index1];
              // ISSUE: reference to a compiler-generated field
              float a = math.saturate((float) waterConsumer.m_WantedConsumption * this.m_FeedbackConfigurationData.m_WaterConsumerSewageFactor);
              float num9 = (float) (componentData8.m_Capacity + componentData9.m_Capacity);
              // ISSUE: reference to a compiler-generated field
              float delta = num7 * math.select(a, 1f, !waterConsumer.sewageConnected) * math.saturate(num9 * this.m_FeedbackConfigurationData.m_SewageCapacityFactor);
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: reference to a compiler-generated method
              this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.SewageCapacity), delta, distance1);
            }
            if (componentData10.m_VehicleCapacity != 0)
            {
              // ISSUE: reference to a compiler-generated field
              float delta = num7 * math.saturate((float) componentData10.m_VehicleCapacity * this.m_FeedbackConfigurationData.m_TransportVehicleCapacityFactor);
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: reference to a compiler-generated method
              this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, componentData10.m_TransportType), delta, distance1);
            }
            if ((double) componentData14.m_NetworkCapacity >= 1.0 && (double) componentData14.m_Range >= 1.0)
            {
              // ISSUE: reference to a compiler-generated field
              float x = num7 * math.saturate(componentData14.m_NetworkCapacity * this.m_FeedbackConfigurationData.m_TelecomCapacityFactor) * math.saturate((float) (1.0 - (double) distance1 / (double) componentData14.m_Range));
              if ((double) num1 >= 1.0)
                x *= math.saturate(distance1 / num1);
              if ((double) x != 0.0)
              {
                // ISSUE: reference to a compiler-generated field
                float y = 1f + TelecomCoverage.SampleNetworkQuality(this.m_TelecomCoverageData, transform.m_Position);
                float delta = x / math.max(x, y);
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: reference to a compiler-generated method
                this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.TelecomCoverage), delta, distance1);
              }
            }
            if (componentData18.m_FireHelicopterCapacity != 0)
            {
              float num10 = (float) componentData18.m_FireHelicopterCapacity * componentData18.m_VehicleEfficiency + (float) math.min(componentData18.m_FireHelicopterCapacity, componentData18.m_DisasterResponseCapacity);
              // ISSUE: reference to a compiler-generated field
              float delta = num7 * math.saturate(num10 * this.m_FeedbackConfigurationData.m_FireStationHelicopterFactor);
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: reference to a compiler-generated method
              this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.FireStationHelicopters), delta, distance1);
            }
            if (componentData19.m_PoliceHelicopterCapacity != 0 && nativeArray8.Length != 0)
            {
              CrimeProducer crimeProducer = nativeArray8[index1];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float delta = num7 * math.saturate(crimeProducer.m_Crime * this.m_FeedbackConfigurationData.m_CrimeProducerCrimeFactor) * math.saturate((float) componentData19.m_PoliceHelicopterCapacity * this.m_FeedbackConfigurationData.m_PoliceStationHelicopterFactor);
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: reference to a compiler-generated method
              this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.PoliceStationHelicopters), delta, distance1);
            }
            if ((double) componentData21.m_GroundPollution != 0.0 || (double) componentData21.m_AirPollution != 0.0 || (double) componentData21.m_NoisePollution != 0.0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 float3 = -math.saturate(math.saturate(num7 * new float3(componentData21.m_GroundPollution, componentData21.m_AirPollution, componentData21.m_NoisePollution) * new float3(this.m_FeedbackConfigurationData.m_GroundPollutionFactor, this.m_FeedbackConfigurationData.m_AirPollutionFactor, this.m_FeedbackConfigurationData.m_NoisePollutionFactor)) * (1f - distance1 / new float3(this.m_FeedbackConfigurationData.m_GroundPollutionRadius, this.m_FeedbackConfigurationData.m_AirPollutionRadius, this.m_FeedbackConfigurationData.m_NoisePollutionRadius)));
              if ((double) float3.x != 0.0)
              {
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: reference to a compiler-generated method
                this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.GroundPollution), float3.x, distance1);
              }
              if ((double) float3.y != 0.0)
              {
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: reference to a compiler-generated method
                this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.AirPollution), float3.y, distance1);
              }
              if ((double) float3.z != 0.0)
              {
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: reference to a compiler-generated method
                this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.NoisePollution), float3.z, distance1);
              }
            }
            if (componentData22.m_Attractiveness != 0)
            {
              // ISSUE: reference to a compiler-generated field
              float delta = num7 * math.saturate((float) componentData22.m_Attractiveness * this.m_FeedbackConfigurationData.m_AttractivenessFactor);
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: reference to a compiler-generated method
              this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, ToolFeedbackSystem.FeedbackType.Attractiveness), delta, distance1);
            }
            if (tempModifierList1.IsCreated)
            {
              for (int index2 = 0; index2 < tempModifierList1.Length; ++index2)
              {
                LocalModifierData localModifierData = tempModifierList1[index2];
                // ISSUE: reference to a compiler-generated field
                if ((LocalModifierType) this.m_FeedbackLocalEffectFactors.Length > localModifierData.m_Type)
                {
                  // ISSUE: reference to a compiler-generated field
                  float factor = this.m_FeedbackLocalEffectFactors[(int) localModifierData.m_Type].m_Factor;
                  float num11 = math.select(math.sign(factor), factor, localModifierData.m_Mode == ModifierValueMode.Absolute);
                  float delta = num7 * math.clamp(localModifierData.m_Delta.max * num11, -1f, 1f) * math.saturate((float) (1.0 - (double) distance1 / (double) localModifierData.m_Radius.max));
                  if ((double) localModifierData.m_Radius.min != 0.0)
                    delta *= math.saturate(distance1 / localModifierData.m_Radius.min);
                  if ((double) delta != 0.0)
                  {
                    // ISSUE: object of a compiler-generated type is created
                    // ISSUE: reference to a compiler-generated method
                    this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, localModifierData.m_Type), delta, distance1);
                  }
                }
              }
            }
            if (tempModifierList2.IsCreated)
            {
              for (int index3 = 0; index3 < tempModifierList2.Length; ++index3)
              {
                CityModifierData cityModifierData = tempModifierList2[index3];
                // ISSUE: reference to a compiler-generated field
                if ((CityModifierType) this.m_FeedbackCityEffectFactors.Length > cityModifierData.m_Type)
                {
                  // ISSUE: reference to a compiler-generated field
                  float factor = this.m_FeedbackCityEffectFactors[(int) cityModifierData.m_Type].m_Factor;
                  float num12 = math.select(math.sign(factor), factor, cityModifierData.m_Mode == ModifierValueMode.Absolute);
                  float delta = num7 * math.clamp(cityModifierData.m_Range.max * num12, -1f, 1f);
                  if ((double) delta != 0.0)
                  {
                    // ISSUE: object of a compiler-generated type is created
                    // ISSUE: reference to a compiler-generated method
                    this.AddEffect(ref updateList, ref total, new ToolFeedbackSystem.RecentKey(entity, cityModifierData.m_Type), delta, distance1);
                  }
                }
              }
            }
            if ((double) random.NextFloat(1f) < (double) math.abs(total.x))
            {
              bool flag13 = (double) total.x > 0.0;
              float num13 = total.y / total.z;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity prefab = flag13 ? this.m_FeedbackConfigurationData.m_HappyFaceNotification : this.m_FeedbackConfigurationData.m_SadFaceNotification;
              float delay = num13 * (1f / 1000f) + random.NextFloat(0.1f);
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Add(entity, prefab, clusterLayer: IconClusterLayer.Transaction, target: Entity.Null, delay: delay);
              if (updateList.IsCreated)
              {
                for (int index4 = 0; index4 < updateList.Length; ++index4)
                {
                  // ISSUE: variable of a compiler-generated type
                  ToolFeedbackSystem.RecentUpdate recentUpdate = updateList[index4];
                  // ISSUE: reference to a compiler-generated field
                  if ((double) recentUpdate.m_Delta > 0.0 == flag13)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_RecentUpdates.Enqueue(recentUpdate);
                  }
                }
              }
            }
            if (updateList.IsCreated)
              updateList.Clear();
          }
        }
        if (updateList.IsCreated)
          updateList.Dispose();
        if (tempModifierList1.IsCreated)
          tempModifierList1.Dispose();
        if (!tempModifierList2.IsCreated)
          return;
        tempModifierList2.Dispose();
      }

      private void AddToTempListForUpgrade(
        NativeList<LocalModifierData> tempModifierList,
        DynamicBuffer<LocalModifierData> localModifiers)
      {
        for (int index1 = 0; index1 < localModifiers.Length; ++index1)
        {
          LocalModifierData localModifier = localModifiers[index1];
          for (int index2 = 0; index2 < tempModifierList.Length; ++index2)
          {
            LocalModifierData tempModifier = tempModifierList[index2];
            if (tempModifier.m_Type == localModifier.m_Type)
            {
              bool flag1 = (double) tempModifier.m_Radius.max > 0.0;
              bool flag2 = (double) tempModifier.m_Delta.max != 0.0;
              if (flag1)
                tempModifier.m_Delta.max += localModifier.m_Delta.max;
              switch (tempModifier.m_RadiusCombineMode)
              {
                case ModifierRadiusCombineMode.Maximal:
                  if (flag1 && !flag2)
                    tempModifier.m_Radius.min = math.max(tempModifier.m_Radius.min, localModifier.m_Radius.max);
                  tempModifier.m_Radius.max = math.max(tempModifier.m_Radius.max, localModifier.m_Radius.max);
                  break;
                case ModifierRadiusCombineMode.Additive:
                  if (flag1 && !flag2)
                    tempModifier.m_Radius.min += localModifier.m_Radius.max;
                  tempModifier.m_Radius.max += localModifier.m_Radius.max;
                  break;
              }
              tempModifierList[index2] = tempModifier;
              break;
            }
          }
        }
      }

      private void AddEffect(
        ref NativeList<ToolFeedbackSystem.RecentUpdate> updateList,
        ref float3 total,
        ToolFeedbackSystem.RecentKey recentKey,
        float delta,
        float distance)
      {
        // ISSUE: reference to a compiler-generated field
        delta = math.select(delta, -delta, this.m_FeedbackData.m_IsDeleted);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ToolFeedbackSystem.RecentUpdate recentUpdate = new ToolFeedbackSystem.RecentUpdate()
        {
          m_Key = recentKey,
          m_Delta = math.sign(delta)
        };
        // ISSUE: variable of a compiler-generated type
        ToolFeedbackSystem.RecentValue recentValue;
        // ISSUE: reference to a compiler-generated field
        if (this.m_RecentMap.TryGetValue(recentKey, out recentValue))
        {
          // ISSUE: reference to a compiler-generated field
          float x = delta - recentValue.m_FeedbackDelta;
          delta = math.select(math.clamp(x, delta, 0.0f), math.clamp(x, 0.0f, delta), (double) delta > 0.0);
        }
        if ((double) delta == 0.0)
          return;
        float z = math.abs(delta);
        total += new float3(delta, distance * z, z);
        if (!updateList.IsCreated)
          updateList = new NativeList<ToolFeedbackSystem.RecentUpdate>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        updateList.Add(in recentUpdate);
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
    private struct UpdateRecentMapJob : IJob
    {
      public NativeParallelHashMap<ToolFeedbackSystem.RecentKey, ToolFeedbackSystem.RecentValue> m_RecentMap;
      public NativeQueue<ToolFeedbackSystem.RecentUpdate> m_RecentUpdates;
      public uint m_SimulationFrame;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ToolFeedbackSystem.RecentKey> keyArray = this.m_RecentMap.GetKeyArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < keyArray.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          ToolFeedbackSystem.RecentKey key = keyArray[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          ToolFeedbackSystem.RecentValue recent = this.m_RecentMap[key];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num = (float) (this.m_SimulationFrame - recent.m_UpdateFrame) * 0.0001f;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          recent.m_UpdateFrame = this.m_SimulationFrame;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          recent.m_FeedbackDelta = math.select(math.min(0.0f, recent.m_FeedbackDelta + num), math.max(0.0f, recent.m_FeedbackDelta - num), (double) recent.m_FeedbackDelta > 0.0);
          // ISSUE: reference to a compiler-generated field
          if ((double) recent.m_FeedbackDelta != 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_RecentMap[key] = recent;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_RecentMap.Remove(key);
          }
        }
        keyArray.Dispose();
        // ISSUE: variable of a compiler-generated type
        ToolFeedbackSystem.RecentUpdate recentUpdate;
        // ISSUE: reference to a compiler-generated field
        while (this.m_RecentUpdates.TryDequeue(out recentUpdate))
        {
          // ISSUE: variable of a compiler-generated type
          ToolFeedbackSystem.RecentValue recentValue;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_RecentMap.TryGetValue(recentUpdate.m_Key, out recentValue))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            recentValue.m_FeedbackDelta += recentUpdate.m_Delta;
            // ISSUE: reference to a compiler-generated field
            if ((double) recentValue.m_FeedbackDelta != 0.0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_RecentMap[recentUpdate.m_Key] = recentValue;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_RecentMap.Remove(recentUpdate.m_Key);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_RecentMap.Add(recentUpdate.m_Key, new ToolFeedbackSystem.RecentValue()
            {
              m_UpdateFrame = this.m_SimulationFrame,
              m_FeedbackDelta = recentUpdate.m_Delta
            });
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CoverageData> __Game_Prefabs_CoverageData_RO_ComponentLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<GarbageProducer> __Game_Buildings_GarbageProducer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterConsumer> __Game_Buildings_WaterConsumer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<MailProducer> __Game_Buildings_MailProducer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CrimeProducer> __Game_Buildings_CrimeProducer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<GarbageFacilityData> __Game_Prefabs_GarbageFacilityData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HospitalData> __Game_Prefabs_HospitalData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<DeathcareFacilityData> __Game_Prefabs_DeathcareFacilityData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PowerPlantData> __Game_Prefabs_PowerPlantData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WindPoweredData> __Game_Prefabs_WindPoweredData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SolarPoweredData> __Game_Prefabs_SolarPoweredData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.TransformerData> __Game_Prefabs_TransformerData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterPumpingStationData> __Game_Prefabs_WaterPumpingStationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SewageOutletData> __Game_Prefabs_SewageOutletData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WastewaterTreatmentPlantData> __Game_Prefabs_WastewaterTreatmentPlantData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> __Game_Prefabs_TransportDepotData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportStationData> __Game_Prefabs_TransportStationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PublicTransportStationData> __Game_Prefabs_PublicTransportStationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CargoTransportStationData> __Game_Prefabs_CargoTransportStationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportStopData> __Game_Prefabs_TransportStopData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PostFacilityData> __Game_Prefabs_PostFacilityData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TelecomFacilityData> __Game_Prefabs_TelecomFacilityData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SchoolData> __Game_Prefabs_SchoolData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingFacilityData> __Game_Prefabs_ParkingFacilityData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MaintenanceDepotData> __Game_Prefabs_MaintenanceDepotData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<FireStationData> __Game_Prefabs_FireStationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PoliceStationData> __Game_Prefabs_PoliceStationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrisonData> __Game_Prefabs_PrisonData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PollutionData> __Game_Prefabs_PollutionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AttractionData> __Game_Prefabs_AttractionData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> __Game_Net_ServiceCoverage_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LocalModifierData> __Game_Prefabs_LocalModifierData_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CityModifierData> __Game_Prefabs_CityModifierData_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CoverageData_RO_ComponentLookup = state.GetComponentLookup<CoverageData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageProducer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<GarbageProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_MailProducer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MailProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CrimeProducer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CrimeProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup = state.GetComponentLookup<GarbageFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HospitalData_RO_ComponentLookup = state.GetComponentLookup<HospitalData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DeathcareFacilityData_RO_ComponentLookup = state.GetComponentLookup<DeathcareFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PowerPlantData_RO_ComponentLookup = state.GetComponentLookup<PowerPlantData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WindPoweredData_RO_ComponentLookup = state.GetComponentLookup<WindPoweredData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SolarPoweredData_RO_ComponentLookup = state.GetComponentLookup<SolarPoweredData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransformerData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.TransformerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterPumpingStationData_RO_ComponentLookup = state.GetComponentLookup<WaterPumpingStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SewageOutletData_RO_ComponentLookup = state.GetComponentLookup<SewageOutletData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WastewaterTreatmentPlantData_RO_ComponentLookup = state.GetComponentLookup<WastewaterTreatmentPlantData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportDepotData_RO_ComponentLookup = state.GetComponentLookup<TransportDepotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportStationData_RO_ComponentLookup = state.GetComponentLookup<TransportStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PublicTransportStationData_RO_ComponentLookup = state.GetComponentLookup<PublicTransportStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CargoTransportStationData_RO_ComponentLookup = state.GetComponentLookup<CargoTransportStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportStopData_RO_ComponentLookup = state.GetComponentLookup<TransportStopData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PostFacilityData_RO_ComponentLookup = state.GetComponentLookup<PostFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TelecomFacilityData_RO_ComponentLookup = state.GetComponentLookup<TelecomFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SchoolData_RO_ComponentLookup = state.GetComponentLookup<SchoolData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingFacilityData_RO_ComponentLookup = state.GetComponentLookup<ParkingFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MaintenanceDepotData_RO_ComponentLookup = state.GetComponentLookup<MaintenanceDepotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_FireStationData_RO_ComponentLookup = state.GetComponentLookup<FireStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PoliceStationData_RO_ComponentLookup = state.GetComponentLookup<PoliceStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrisonData_RO_ComponentLookup = state.GetComponentLookup<PrisonData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PollutionData_RO_ComponentLookup = state.GetComponentLookup<PollutionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AttractionData_RO_ComponentLookup = state.GetComponentLookup<AttractionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ServiceCoverage_RO_BufferLookup = state.GetBufferLookup<Game.Net.ServiceCoverage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LocalModifierData_RO_BufferLookup = state.GetBufferLookup<LocalModifierData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CityModifierData_RO_BufferLookup = state.GetBufferLookup<CityModifierData>(true);
      }
    }
  }
}
