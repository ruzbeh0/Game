// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AccidentCreatureSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Citizens;
using Game.Common;
using Game.Creatures;
using Game.Events;
using Game.Net;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class AccidentCreatureSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 64;
    private SimulationSystem m_SimulationSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private IconCommandSystem m_IconCommandSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_CreatureQuery;
    private EntityQuery m_ConfigQuery;
    private EntityArchetype m_AddAccidentSiteArchetype;
    private EntityArchetype m_AddProblemArchetype;
    private AccidentCreatureSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatureQuery = this.GetEntityQuery(ComponentType.ReadWrite<InvolvedInAccident>(), ComponentType.ReadOnly<Creature>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<PoliceConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AddAccidentSiteArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<AddAccidentSite>());
      // ISSUE: reference to a compiler-generated field
      this.m_AddProblemArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<AddHealthProblem>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatureQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      PoliceConfigurationData singleton = this.m_ConfigQuery.GetSingleton<PoliceConfigurationData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_TargetElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Ambulance_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Hearse_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_AccidentSite_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Creature_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Stumbling_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new AccidentCreatureSystem.AccidentCreatureJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_MovingType = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentTypeHandle,
        m_ResidentType = this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentTypeHandle,
        m_InvolvedInAccidentType = this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentTypeHandle,
        m_StumblingType = this.__TypeHandle.__Game_Creatures_Stumbling_RO_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle,
        m_CurrentVehicleType = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle,
        m_CreatureType = this.__TypeHandle.__Game_Creatures_Creature_RW_ComponentTypeHandle,
        m_RoadData = this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_AccidentSiteData = this.__TypeHandle.__Game_Events_AccidentSite_RO_ComponentLookup,
        m_HearseData = this.__TypeHandle.__Game_Vehicles_Hearse_RO_ComponentLookup,
        m_AmbulanceData = this.__TypeHandle.__Game_Vehicles_Ambulance_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_TargetElements = this.__TypeHandle.__Game_Events_TargetElement_RO_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_PoliceConfigurationData = singleton,
        m_AddAccidentSiteArchetype = this.m_AddAccidentSiteArchetype,
        m_AddProblemArchetype = this.m_AddProblemArchetype,
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer()
      }.ScheduleParallel<AccidentCreatureSystem.AccidentCreatureJob>(this.m_CreatureQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
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
    public AccidentCreatureSystem()
    {
    }

    [BurstCompile]
    private struct AccidentCreatureJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Moving> m_MovingType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Creatures.Resident> m_ResidentType;
      [ReadOnly]
      public ComponentTypeHandle<InvolvedInAccident> m_InvolvedInAccidentType;
      [ReadOnly]
      public ComponentTypeHandle<Stumbling> m_StumblingType;
      [ReadOnly]
      public ComponentTypeHandle<Target> m_TargetType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> m_CurrentVehicleType;
      public ComponentTypeHandle<Creature> m_CreatureType;
      [ReadOnly]
      public ComponentLookup<Road> m_RoadData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<AccidentSite> m_AccidentSiteData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Hearse> m_HearseData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Ambulance> m_AmbulanceData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public BufferLookup<TargetElement> m_TargetElements;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public PoliceConfigurationData m_PoliceConfigurationData;
      [ReadOnly]
      public EntityArchetype m_AddAccidentSiteArchetype;
      [ReadOnly]
      public EntityArchetype m_AddProblemArchetype;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
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
        NativeArray<InvolvedInAccident> nativeArray2 = chunk.GetNativeArray<InvolvedInAccident>(ref this.m_InvolvedInAccidentType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Stumbling>(ref this.m_StumblingType))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Transform> nativeArray3 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Moving> nativeArray4 = chunk.GetNativeArray<Moving>(ref this.m_MovingType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Creatures.Resident> nativeArray5 = chunk.GetNativeArray<Game.Creatures.Resident>(ref this.m_ResidentType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Creature> nativeArray6 = chunk.GetNativeArray<Creature>(ref this.m_CreatureType);
          for (int index = 0; index < nativeArray6.Length; ++index)
          {
            ref Creature local = ref nativeArray6.ElementAt<Creature>(index);
            local.m_QueueEntity = Entity.Null;
            local.m_QueueArea = new Sphere3();
          }
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<CurrentVehicle>(ref this.m_CurrentVehicleType))
          {
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              Transform transform = nativeArray3[index];
              InvolvedInAccident involvedInAccident = nativeArray2[index];
              if (nativeArray5.Length != 0)
              {
                Game.Creatures.Resident resident = nativeArray5[index];
                // ISSUE: reference to a compiler-generated method
                int num = (int) this.AddInjury(unfilteredChunkIndex, involvedInAccident, resident, ref random);
                // ISSUE: reference to a compiler-generated method
                this.StopStumbling(unfilteredChunkIndex, entity);
                if ((num & 8) == 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Add(entity, this.m_PoliceConfigurationData.m_TrafficAccidentNotificationPrefab, IconPriority.MajorProblem, flags: IconFlags.IgnoreTarget, target: involvedInAccident.m_Event);
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                if (this.m_TargetElements.HasBuffer(involvedInAccident.m_Event) && this.FindAccidentSite(involvedInAccident.m_Event) == Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated method
                  Entity suitableAccidentSite = this.FindSuitableAccidentSite(transform.m_Position);
                  if (suitableAccidentSite != Entity.Null)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddAccidentSite(unfilteredChunkIndex, ref involvedInAccident, suitableAccidentSite);
                  }
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.ClearAccident(unfilteredChunkIndex, entity);
              }
            }
          }
          else if (nativeArray4.Length != 0)
          {
            for (int index = 0; index < nativeArray4.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              Transform transform = nativeArray3[index];
              Moving moving = nativeArray4[index];
              InvolvedInAccident involvedInAccident = nativeArray2[index];
              if ((double) transform.m_Position.y < -1000.0)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, entity, new Deleted());
              }
              else if ((double) math.lengthsq(moving.m_Velocity) < 9.9999997473787516E-05 && (double) math.lengthsq(moving.m_AngularVelocity) < 9.9999997473787516E-05)
              {
                if (nativeArray5.Length != 0)
                {
                  Game.Creatures.Resident resident = nativeArray5[index];
                  // ISSUE: reference to a compiler-generated method
                  if ((this.AddInjury(unfilteredChunkIndex, involvedInAccident, resident, ref random) & HealthProblemFlags.RequireTransport) != HealthProblemFlags.None)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.StopMoving(unfilteredChunkIndex, entity);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.StopStumbling(unfilteredChunkIndex, entity);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_IconCommandBuffer.Add(entity, this.m_PoliceConfigurationData.m_TrafficAccidentNotificationPrefab, IconPriority.MajorProblem, flags: IconFlags.IgnoreTarget, target: involvedInAccident.m_Event);
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  if (this.m_TargetElements.HasBuffer(involvedInAccident.m_Event) && this.FindAccidentSite(involvedInAccident.m_Event) == Entity.Null)
                  {
                    // ISSUE: reference to a compiler-generated method
                    Entity suitableAccidentSite = this.FindSuitableAccidentSite(transform.m_Position);
                    if (suitableAccidentSite != Entity.Null)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.AddAccidentSite(unfilteredChunkIndex, ref involvedInAccident, suitableAccidentSite);
                    }
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated method
                  this.ClearAccident(unfilteredChunkIndex, entity);
                }
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Target> nativeArray7 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              InvolvedInAccident involvedInAccident = nativeArray2[index];
              Target target = nativeArray7[index];
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.IsSecured(involvedInAccident) || this.m_HearseData.HasComponent(target.m_Target) || this.m_AmbulanceData.HasComponent(target.m_Target))
              {
                // ISSUE: reference to a compiler-generated method
                this.StartMoving(unfilteredChunkIndex, entity);
                // ISSUE: reference to a compiler-generated method
                this.ClearAccident(unfilteredChunkIndex, entity);
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Target> nativeArray8 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            InvolvedInAccident involvedInAccident = nativeArray2[index];
            Target target = nativeArray8[index];
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.IsSecured(involvedInAccident) || this.m_HearseData.HasComponent(target.m_Target) || this.m_AmbulanceData.HasComponent(target.m_Target))
            {
              // ISSUE: reference to a compiler-generated method
              this.ClearAccident(unfilteredChunkIndex, entity);
            }
          }
        }
      }

      private HealthProblemFlags AddInjury(
        int jobIndex,
        InvolvedInAccident involvedInAccident,
        Game.Creatures.Resident resident,
        ref Random random)
      {
        int num = 50;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabRefData.HasComponent(resident.m_Citizen) || random.NextInt(100) >= num)
          return HealthProblemFlags.None;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_AddProblemArchetype);
        AddHealthProblem component = new AddHealthProblem()
        {
          m_Event = involvedInAccident.m_Event,
          m_Target = resident.m_Citizen,
          m_Flags = HealthProblemFlags.RequireTransport
        };
        component.m_Flags |= random.NextInt(100) < 20 ? HealthProblemFlags.Dead : HealthProblemFlags.Injured;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<AddHealthProblem>(jobIndex, entity, component);
        return component.m_Flags;
      }

      private bool IsSecured(InvolvedInAccident involvedInAccident)
      {
        // ISSUE: reference to a compiler-generated method
        Entity accidentSite1 = this.FindAccidentSite(involvedInAccident.m_Event);
        if (!(accidentSite1 != Entity.Null))
          return true;
        // ISSUE: reference to a compiler-generated field
        AccidentSite accidentSite2 = this.m_AccidentSiteData[accidentSite1];
        // ISSUE: reference to a compiler-generated field
        return (accidentSite2.m_Flags & AccidentSiteFlags.Secured) != (AccidentSiteFlags) 0 || this.m_SimulationFrame >= accidentSite2.m_CreationFrame + 14400U;
      }

      private void StopMoving(int jobIndex, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Moving>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<TransformFrame>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<InterpolatedTransform>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Stopped>(jobIndex, entity, new Stopped());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
      }

      private void StartMoving(int jobIndex, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Stopped>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddBuffer<TransformFrame>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<InterpolatedTransform>(jobIndex, entity, new InterpolatedTransform());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Moving>(jobIndex, entity, new Moving());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
      }

      private void StopStumbling(int jobIndex, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Stumbling>(jobIndex, entity);
      }

      private void ClearAccident(int jobIndex, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<InvolvedInAccident>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Stumbling>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IconCommandBuffer.Remove(entity, this.m_PoliceConfigurationData.m_TrafficAccidentNotificationPrefab);
      }

      private Entity FindAccidentSite(Entity _event)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_TargetElements.HasBuffer(_event))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<TargetElement> targetElement = this.m_TargetElements[_event];
          for (int index = 0; index < targetElement.Length; ++index)
          {
            Entity entity = targetElement[index].m_Entity;
            // ISSUE: reference to a compiler-generated field
            if (this.m_AccidentSiteData.HasComponent(entity))
              return entity;
          }
        }
        return Entity.Null;
      }

      private Entity FindSuitableAccidentSite(float3 position)
      {
        float num = 30f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AccidentCreatureSystem.AccidentCreatureJob.EdgeIterator iterator = new AccidentCreatureSystem.AccidentCreatureJob.EdgeIterator()
        {
          m_Bounds = new Bounds3(position - num, position + num),
          m_Position = position,
          m_MaxDistance = num,
          m_AccidentSiteData = this.m_AccidentSiteData,
          m_EdgeGeometryData = this.m_EdgeGeometryData,
          m_RoadData = this.m_RoadData,
          m_CurveData = this.m_CurveData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<AccidentCreatureSystem.AccidentCreatureJob.EdgeIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        return iterator.m_Result;
      }

      private void AddAccidentSite(
        int jobIndex,
        ref InvolvedInAccident involvedInAccident,
        Entity target)
      {
        AddAccidentSite component = new AddAccidentSite()
        {
          m_Event = involvedInAccident.m_Event,
          m_Target = target,
          m_Flags = AccidentSiteFlags.TrafficAccident
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_AddAccidentSiteArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<AddAccidentSite>(jobIndex, entity, component);
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

      private struct EdgeIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public float3 m_Position;
        public float m_MaxDistance;
        public Entity m_Result;
        public ComponentLookup<AccidentSite> m_AccidentSiteData;
        public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
        public ComponentLookup<Road> m_RoadData;
        public ComponentLookup<Curve> m_CurveData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(this.m_Bounds, bounds.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity edge)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(this.m_Bounds, bounds.m_Bounds) || !this.m_RoadData.HasComponent(edge) || this.m_AccidentSiteData.HasComponent(edge) || !this.m_CurveData.HasComponent(edge))
            return;
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[edge];
          // ISSUE: reference to a compiler-generated field
          EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[edge];
          float num1 = math.distance(edgeGeometry.m_Start.m_Left.d, edgeGeometry.m_Start.m_Right.d);
          // ISSUE: reference to a compiler-generated field
          float num2 = MathUtils.Distance(curve.m_Bezier, this.m_Position, out float _) - num1 * 0.5f;
          // ISSUE: reference to a compiler-generated field
          if ((double) num2 >= (double) this.m_MaxDistance)
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_MaxDistance = num2;
          // ISSUE: reference to a compiler-generated field
          this.m_Result = edge;
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Moving> __Game_Objects_Moving_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Creatures.Resident> __Game_Creatures_Resident_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InvolvedInAccident> __Game_Events_InvolvedInAccident_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Stumbling> __Game_Creatures_Stumbling_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Target> __Game_Common_Target_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Creature> __Game_Creatures_Creature_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Road> __Game_Net_Road_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AccidentSite> __Game_Events_AccidentSite_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Hearse> __Game_Vehicles_Hearse_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Ambulance> __Game_Vehicles_Ambulance_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<TargetElement> __Game_Events_TargetElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Resident_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Creatures.Resident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InvolvedInAccident_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InvolvedInAccident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Stumbling_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Stumbling>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Creature_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Creature>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentLookup = state.GetComponentLookup<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_AccidentSite_RO_ComponentLookup = state.GetComponentLookup<AccidentSite>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Hearse_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.Hearse>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Ambulance_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.Ambulance>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_TargetElement_RO_BufferLookup = state.GetBufferLookup<TargetElement>(true);
      }
    }
  }
}
