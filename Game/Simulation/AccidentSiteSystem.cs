// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AccidentSiteSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Events;
using Game.Net;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class AccidentSiteSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 64;
    private SimulationSystem m_SimulationSystem;
    private IconCommandSystem m_IconCommandSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_AccidentQuery;
    private EntityQuery m_ConfigQuery;
    private EntityArchetype m_PoliceRequestArchetype;
    private EntityArchetype m_EventImpactArchetype;
    private AccidentSiteSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_AccidentQuery = this.GetEntityQuery(ComponentType.ReadWrite<AccidentSite>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<PoliceConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PoliceRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<PoliceEmergencyRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_EventImpactArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Impact>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_AccidentQuery);
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_TargetElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CrimeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrafficAccidentData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_PoliceEmergencyRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Criminal_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_AccidentSite_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new AccidentSiteSystem.AccidentSiteJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_AccidentSiteType = this.__TypeHandle.__Game_Events_AccidentSite_RW_ComponentTypeHandle,
        m_InvolvedInAccidentData = this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentLookup,
        m_CriminalData = this.__TypeHandle.__Game_Citizens_Criminal_RO_ComponentLookup,
        m_PoliceEmergencyRequestData = this.__TypeHandle.__Game_Simulation_PoliceEmergencyRequest_RO_ComponentLookup,
        m_MovingData = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup,
        m_VehicleData = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup,
        m_CarData = this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabTrafficAccidentData = this.__TypeHandle.__Game_Prefabs_TrafficAccidentData_RO_ComponentLookup,
        m_PrefabCrimeData = this.__TypeHandle.__Game_Prefabs_CrimeData_RO_ComponentLookup,
        m_TargetElements = this.__TypeHandle.__Game_Events_TargetElement_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_PoliceRequestArchetype = this.m_PoliceRequestArchetype,
        m_EventImpactArchetype = this.m_EventImpactArchetype,
        m_PoliceConfigurationData = this.m_ConfigQuery.GetSingleton<PoliceConfigurationData>(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer()
      }.ScheduleParallel<AccidentSiteSystem.AccidentSiteJob>(this.m_AccidentQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(jobHandle);
      this.Dependency = jobHandle;
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
    public AccidentSiteSystem()
    {
    }

    [BurstCompile]
    private struct AccidentSiteJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      public ComponentTypeHandle<AccidentSite> m_AccidentSiteType;
      [ReadOnly]
      public ComponentLookup<InvolvedInAccident> m_InvolvedInAccidentData;
      [ReadOnly]
      public ComponentLookup<Criminal> m_CriminalData;
      [ReadOnly]
      public ComponentLookup<PoliceEmergencyRequest> m_PoliceEmergencyRequestData;
      [ReadOnly]
      public ComponentLookup<Moving> m_MovingData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [ReadOnly]
      public ComponentLookup<Car> m_CarData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<TrafficAccidentData> m_PrefabTrafficAccidentData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.CrimeData> m_PrefabCrimeData;
      [ReadOnly]
      public BufferLookup<TargetElement> m_TargetElements;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public EntityArchetype m_PoliceRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_EventImpactArchetype;
      [ReadOnly]
      public PoliceConfigurationData m_PoliceConfigurationData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public IconCommandBuffer m_IconCommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AccidentSite> nativeArray2 = chunk.GetNativeArray<AccidentSite>(ref this.m_AccidentSiteType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<Building>(ref this.m_BuildingType);
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          Entity entity1 = nativeArray1[index1];
          AccidentSite accidentSite = nativeArray2[index1];
          // ISSUE: reference to a compiler-generated field
          Random random = this.m_RandomSeed.GetRandom(entity1.Index);
          Entity target = Entity.Null;
          int num1 = 0;
          float severity = 0.0f;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SimulationFrame - accidentSite.m_CreationFrame >= 3600U)
            accidentSite.m_Flags &= ~AccidentSiteFlags.StageAccident;
          accidentSite.m_Flags &= ~AccidentSiteFlags.MovingVehicles;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TargetElements.HasBuffer(accidentSite.m_Event))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<TargetElement> targetElement = this.m_TargetElements[accidentSite.m_Event];
            for (int index2 = 0; index2 < targetElement.Length; ++index2)
            {
              Entity entity2 = targetElement[index2].m_Entity;
              InvolvedInAccident componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_InvolvedInAccidentData.TryGetComponent(entity2, out componentData))
              {
                if (componentData.m_Event == accidentSite.m_Event)
                {
                  ++num1;
                  // ISSUE: reference to a compiler-generated field
                  bool flag2 = this.m_MovingData.HasComponent(entity2);
                  // ISSUE: reference to a compiler-generated field
                  if (flag2 && (accidentSite.m_Flags & AccidentSiteFlags.MovingVehicles) == (AccidentSiteFlags) 0 && this.m_VehicleData.HasComponent(entity2))
                    accidentSite.m_Flags |= AccidentSiteFlags.MovingVehicles;
                  if ((double) componentData.m_Severity > (double) severity)
                  {
                    target = flag2 ? Entity.Null : entity2;
                    severity = componentData.m_Severity;
                    accidentSite.m_Flags &= ~AccidentSiteFlags.StageAccident;
                  }
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_CriminalData.HasComponent(entity2))
                {
                  // ISSUE: reference to a compiler-generated field
                  Criminal criminal = this.m_CriminalData[entity2];
                  if (criminal.m_Event == accidentSite.m_Event && (criminal.m_Flags & CriminalFlags.Arrested) == (CriminalFlags) 0)
                  {
                    ++num1;
                    if ((criminal.m_Flags & CriminalFlags.Monitored) != (CriminalFlags) 0)
                      accidentSite.m_Flags |= AccidentSiteFlags.CrimeMonitored;
                  }
                }
              }
            }
            if (num1 == 0 && (accidentSite.m_Flags & AccidentSiteFlags.StageAccident) != (AccidentSiteFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef = this.m_PrefabRefData[accidentSite.m_Event];
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabTrafficAccidentData.HasComponent(prefabRef.m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                TrafficAccidentData trafficAccidentData = this.m_PrefabTrafficAccidentData[prefabRef.m_Prefab];
                // ISSUE: reference to a compiler-generated method
                Entity subject = this.TryFindSubject(entity1, ref random, trafficAccidentData);
                if (subject != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddImpact(unfilteredChunkIndex, accidentSite.m_Event, ref random, subject, trafficAccidentData);
                }
              }
            }
          }
          if ((accidentSite.m_Flags & (AccidentSiteFlags.CrimeScene | AccidentSiteFlags.CrimeDetected)) == AccidentSiteFlags.CrimeScene)
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[accidentSite.m_Event];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabCrimeData.HasComponent(prefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Prefabs.CrimeData crimeData = this.m_PrefabCrimeData[prefabRef.m_Prefab];
              // ISSUE: reference to a compiler-generated field
              float num2 = (float) (this.m_SimulationFrame - accidentSite.m_CreationFrame) / 60f;
              if ((accidentSite.m_Flags & AccidentSiteFlags.CrimeMonitored) != (AccidentSiteFlags) 0 || (double) num2 >= (double) crimeData.m_AlarmDelay.max)
                accidentSite.m_Flags |= AccidentSiteFlags.CrimeDetected;
              else if ((double) num2 >= (double) crimeData.m_AlarmDelay.min)
              {
                float num3 = (float) (1.0666667222976685 / ((double) crimeData.m_AlarmDelay.max - (double) crimeData.m_AlarmDelay.min));
                if ((double) random.NextFloat(1f) <= (double) num3)
                  accidentSite.m_Flags |= AccidentSiteFlags.CrimeDetected;
              }
            }
            if ((accidentSite.m_Flags & AccidentSiteFlags.CrimeDetected) != (AccidentSiteFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Add(entity1, this.m_PoliceConfigurationData.m_CrimeSceneNotificationPrefab, IconPriority.MajorProblem, flags: IconFlags.IgnoreTarget, target: accidentSite.m_Event);
            }
          }
          else if ((accidentSite.m_Flags & (AccidentSiteFlags.CrimeScene | AccidentSiteFlags.CrimeFinished)) == AccidentSiteFlags.CrimeScene)
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[accidentSite.m_Event];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabCrimeData.HasComponent(prefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Prefabs.CrimeData crimeData = this.m_PrefabCrimeData[prefabRef.m_Prefab];
              // ISSUE: reference to a compiler-generated field
              float num4 = (float) (this.m_SimulationFrame - accidentSite.m_CreationFrame) / 60f;
              if ((double) num4 >= (double) crimeData.m_CrimeDuration.max)
                accidentSite.m_Flags |= AccidentSiteFlags.CrimeFinished;
              else if ((double) num4 >= (double) crimeData.m_CrimeDuration.min)
              {
                float num5 = (float) (1.0666667222976685 / ((double) crimeData.m_CrimeDuration.max - (double) crimeData.m_CrimeDuration.min));
                if ((double) random.NextFloat(1f) <= (double) num5)
                  accidentSite.m_Flags |= AccidentSiteFlags.CrimeFinished;
              }
            }
          }
          accidentSite.m_Flags &= ~AccidentSiteFlags.RequirePolice;
          if ((double) severity > 0.0 || (accidentSite.m_Flags & (AccidentSiteFlags.Secured | AccidentSiteFlags.CrimeScene)) == AccidentSiteFlags.CrimeScene)
          {
            if ((double) severity > 0.0 || (accidentSite.m_Flags & AccidentSiteFlags.CrimeDetected) != (AccidentSiteFlags) 0)
            {
              if (flag1)
                target = entity1;
              if (target != Entity.Null)
              {
                accidentSite.m_Flags |= AccidentSiteFlags.RequirePolice;
                // ISSUE: reference to a compiler-generated method
                this.RequestPoliceIfNeeded(unfilteredChunkIndex, entity1, ref accidentSite, target, severity);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (num1 == 0 && ((accidentSite.m_Flags & (AccidentSiteFlags.Secured | AccidentSiteFlags.CrimeScene)) != (AccidentSiteFlags.Secured | AccidentSiteFlags.CrimeScene) || this.m_SimulationFrame >= accidentSite.m_SecuredFrame + 1024U))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<AccidentSite>(unfilteredChunkIndex, entity1);
              if ((accidentSite.m_Flags & AccidentSiteFlags.CrimeScene) != (AccidentSiteFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Remove(entity1, this.m_PoliceConfigurationData.m_CrimeSceneNotificationPrefab);
              }
            }
          }
          nativeArray2[index1] = accidentSite;
        }
      }

      private Entity TryFindSubject(
        Entity entity,
        ref Random random,
        TrafficAccidentData trafficAccidentData)
      {
        Entity subject = Entity.Null;
        int max = 0;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubLanes.HasBuffer(entity))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[entity];
          for (int index1 = 0; index1 < subLane1.Length; ++index1)
          {
            Entity subLane2 = subLane1[index1].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneObjects.HasBuffer(subLane2))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<LaneObject> laneObject1 = this.m_LaneObjects[subLane2];
              for (int index2 = 0; index2 < laneObject1.Length; ++index2)
              {
                Entity laneObject2 = laneObject1[index2].m_LaneObject;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (trafficAccidentData.m_SubjectType == EventTargetType.MovingCar && this.m_CarData.HasComponent(laneObject2) && this.m_MovingData.HasComponent(laneObject2) && !this.m_InvolvedInAccidentData.HasComponent(laneObject2))
                {
                  ++max;
                  if (random.NextInt(max) == max - 1)
                    subject = laneObject2;
                }
              }
            }
          }
        }
        return subject;
      }

      private void RequestPoliceIfNeeded(
        int jobIndex,
        Entity entity,
        ref AccidentSite accidentSite,
        Entity target,
        float severity)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_PoliceEmergencyRequestData.HasComponent(accidentSite.m_PoliceRequest))
          return;
        PolicePurpose purpose = (accidentSite.m_Flags & AccidentSiteFlags.CrimeMonitored) == (AccidentSiteFlags) 0 ? PolicePurpose.Emergency : PolicePurpose.Intelligence;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_PoliceRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PoliceEmergencyRequest>(jobIndex, entity1, new PoliceEmergencyRequest(entity, target, severity, purpose));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(4U));
      }

      private void AddImpact(
        int jobIndex,
        Entity eventEntity,
        ref Random random,
        Entity target,
        TrafficAccidentData trafficAccidentData)
      {
        Impact component = new Impact()
        {
          m_Event = eventEntity,
          m_Target = target
        };
        // ISSUE: reference to a compiler-generated field
        if (trafficAccidentData.m_AccidentType == TrafficAccidentType.LoseControl && this.m_MovingData.HasComponent(target))
        {
          // ISSUE: reference to a compiler-generated field
          Moving moving = this.m_MovingData[target];
          component.m_Severity = 5f;
          if (random.NextBool())
          {
            component.m_AngularVelocityDelta.y = -2f;
            component.m_VelocityDelta.xz = component.m_Severity * MathUtils.Left(math.normalizesafe(moving.m_Velocity.xz));
          }
          else
          {
            component.m_AngularVelocityDelta.y = 2f;
            component.m_VelocityDelta.xz = component.m_Severity * MathUtils.Right(math.normalizesafe(moving.m_Velocity.xz));
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_EventImpactArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Impact>(jobIndex, entity, component);
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

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      public ComponentTypeHandle<AccidentSite> __Game_Events_AccidentSite_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<InvolvedInAccident> __Game_Events_InvolvedInAccident_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Criminal> __Game_Citizens_Criminal_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PoliceEmergencyRequest> __Game_Simulation_PoliceEmergencyRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Moving> __Game_Objects_Moving_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Car> __Game_Vehicles_Car_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrafficAccidentData> __Game_Prefabs_TrafficAccidentData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.CrimeData> __Game_Prefabs_CrimeData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<TargetElement> __Game_Events_TargetElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_AccidentSite_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AccidentSite>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InvolvedInAccident_RO_ComponentLookup = state.GetComponentLookup<InvolvedInAccident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Criminal_RO_ComponentLookup = state.GetComponentLookup<Criminal>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_PoliceEmergencyRequest_RO_ComponentLookup = state.GetComponentLookup<PoliceEmergencyRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentLookup = state.GetComponentLookup<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentLookup = state.GetComponentLookup<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RO_ComponentLookup = state.GetComponentLookup<Car>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrafficAccidentData_RO_ComponentLookup = state.GetComponentLookup<TrafficAccidentData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CrimeData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.CrimeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_TargetElement_RO_BufferLookup = state.GetBufferLookup<TargetElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
      }
    }
  }
}
