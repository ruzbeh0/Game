// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AccidentVehicleSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Events;
using Game.Net;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Tools;
using Game.Vehicles;
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
  public class AccidentVehicleSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 64;
    private SimulationSystem m_SimulationSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private IconCommandSystem m_IconCommandSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_VehicleQuery;
    private EntityQuery m_ConfigQuery;
    private EntityArchetype m_AddAccidentSiteArchetype;
    private EntityArchetype m_EventIgniteArchetype;
    private EntityArchetype m_AddImpactArchetype;
    private AccidentVehicleSystem.TypeHandle __TypeHandle;

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
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadWrite<InvolvedInAccident>(), ComponentType.ReadOnly<Vehicle>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<PoliceConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AddAccidentSiteArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<AddAccidentSite>());
      // ISSUE: reference to a compiler-generated field
      this.m_EventIgniteArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Ignite>());
      // ISSUE: reference to a compiler-generated field
      this.m_AddImpactArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Impact>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_VehicleQuery);
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
      this.__TypeHandle.__Game_Prefabs_FireData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_BlockedLane_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_OnFire_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle jobHandle = new AccidentVehicleSystem.AccidentVehicleJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_MovingType = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentTypeHandle,
        m_DamagedType = this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentTypeHandle,
        m_DestroyedType = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle,
        m_OnFireType = this.__TypeHandle.__Game_Events_OnFire_RO_ComponentTypeHandle,
        m_BlockedLaneType = this.__TypeHandle.__Game_Objects_BlockedLane_RO_BufferTypeHandle,
        m_PassengerType = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle,
        m_LayoutElementType = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle,
        m_InvolvedInAccidentType = this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentTypeHandle,
        m_ControllerType = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentTypeHandle,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_RoadData = this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_AccidentSiteData = this.__TypeHandle.__Game_Events_AccidentSite_RO_ComponentLookup,
        m_ResidentData = this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabFireData = this.__TypeHandle.__Game_Prefabs_FireData_RO_ComponentLookup,
        m_TargetElements = this.__TypeHandle.__Game_Events_TargetElement_RO_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_PoliceConfigurationData = singleton,
        m_AddAccidentSiteArchetype = this.m_AddAccidentSiteArchetype,
        m_EventIgniteArchetype = this.m_EventIgniteArchetype,
        m_AddImpactArchetype = this.m_AddImpactArchetype,
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer()
      }.ScheduleParallel<AccidentVehicleSystem.AccidentVehicleJob>(this.m_VehicleQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
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
    public AccidentVehicleSystem()
    {
    }

    [BurstCompile]
    private struct AccidentVehicleJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Moving> m_MovingType;
      [ReadOnly]
      public ComponentTypeHandle<Damaged> m_DamagedType;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> m_DestroyedType;
      [ReadOnly]
      public ComponentTypeHandle<OnFire> m_OnFireType;
      [ReadOnly]
      public ComponentTypeHandle<InvolvedInAccident> m_InvolvedInAccidentType;
      [ReadOnly]
      public ComponentTypeHandle<Controller> m_ControllerType;
      [ReadOnly]
      public BufferTypeHandle<BlockedLane> m_BlockedLaneType;
      [ReadOnly]
      public BufferTypeHandle<Passenger> m_PassengerType;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> m_LayoutElementType;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<Road> m_RoadData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<AccidentSite> m_AccidentSiteData;
      [ReadOnly]
      public ComponentLookup<Game.Creatures.Resident> m_ResidentData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<FireData> m_PrefabFireData;
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
      public EntityArchetype m_EventIgniteArchetype;
      [ReadOnly]
      public EntityArchetype m_AddImpactArchetype;
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
        NativeArray<Transform> nativeArray2 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Moving> nativeArray3 = chunk.GetNativeArray<Moving>(ref this.m_MovingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Damaged> nativeArray4 = chunk.GetNativeArray<Damaged>(ref this.m_DamagedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Destroyed> nativeArray5 = chunk.GetNativeArray<Destroyed>(ref this.m_DestroyedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<InvolvedInAccident> nativeArray6 = chunk.GetNativeArray<InvolvedInAccident>(ref this.m_InvolvedInAccidentType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Controller> nativeArray7 = chunk.GetNativeArray<Controller>(ref this.m_ControllerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<BlockedLane> bufferAccessor1 = chunk.GetBufferAccessor<BlockedLane>(ref this.m_BlockedLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LayoutElement> bufferAccessor2 = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutElementType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Passenger> bufferAccessor3 = chunk.GetBufferAccessor<Passenger>(ref this.m_PassengerType);
        // ISSUE: reference to a compiler-generated field
        bool flag = chunk.Has<OnFire>(ref this.m_OnFireType);
        if (nativeArray3.Length != 0)
        {
          for (int index = 0; index < nativeArray3.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Transform transform = nativeArray2[index];
            Moving moving = nativeArray3[index];
            InvolvedInAccident involvedInAccident = nativeArray6[index];
            if (bufferAccessor1.Length == 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.ClearAccident(unfilteredChunkIndex, entity);
            }
            else
            {
              DynamicBuffer<BlockedLane> blockedLanes = bufferAccessor1[index];
              DynamicBuffer<LayoutElement> layout = new DynamicBuffer<LayoutElement>();
              if (bufferAccessor2.Length != 0)
                layout = bufferAccessor2[index];
              // ISSUE: reference to a compiler-generated field
              float num1 = (float) (this.m_SimulationFrame - involvedInAccident.m_InvolvedFrame);
              float num2 = (float) (0.0099999997764825821 + (double) num1 * (double) num1 * 3.0000000261765081E-09);
              float num3 = num2 * num2;
              if ((double) transform.m_Position.y < -1000.0)
              {
                // ISSUE: reference to a compiler-generated field
                VehicleUtils.DeleteVehicle(this.m_CommandBuffer, unfilteredChunkIndex, entity, layout);
              }
              else if ((double) math.lengthsq(moving.m_Velocity) < (double) num3 && (double) math.lengthsq(moving.m_AngularVelocity) < (double) num3)
              {
                // ISSUE: reference to a compiler-generated method
                this.StopVehicle(unfilteredChunkIndex, entity, blockedLanes);
                // ISSUE: reference to a compiler-generated field
                Random random = this.m_RandomSeed.GetRandom(entity.Index);
                // ISSUE: reference to a compiler-generated field
                if (!flag && nativeArray4.Length != 0 && this.m_PrefabRefData.HasComponent(involvedInAccident.m_Event))
                {
                  Damaged damaged = nativeArray4[index];
                  // ISSUE: reference to a compiler-generated field
                  PrefabRef prefabRef = this.m_PrefabRefData[involvedInAccident.m_Event];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabFireData.HasComponent(prefabRef.m_Prefab))
                  {
                    // ISSUE: reference to a compiler-generated field
                    FireData fireData = this.m_PrefabFireData[prefabRef.m_Prefab];
                    float num4 = damaged.m_Damage.x * fireData.m_StartProbability;
                    if ((double) num4 > 0.0099999997764825821 && (double) random.NextFloat(100f) < (double) num4)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.IgniteFire(unfilteredChunkIndex, entity, involvedInAccident.m_Event, fireData);
                    }
                  }
                }
                if (nativeArray5.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Remove(entity, IconPriority.Problem);
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Remove(entity, IconPriority.FatalProblem);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Add(entity, this.m_PoliceConfigurationData.m_TrafficAccidentNotificationPrefab, IconPriority.FatalProblem, flags: IconFlags.IgnoreTarget, target: involvedInAccident.m_Event);
                  if (bufferAccessor3.Length != 0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddInjuries(unfilteredChunkIndex, involvedInAccident, bufferAccessor3[index], ref random);
                  }
                }
                else
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
            }
          }
        }
        else
        {
          for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
          {
            Entity entity = nativeArray1[index1];
            InvolvedInAccident involvedInAccident = nativeArray6[index1];
            DynamicBuffer<BlockedLane> blockedLanes = bufferAccessor1[index1];
            DynamicBuffer<LayoutElement> layout = new DynamicBuffer<LayoutElement>();
            if (bufferAccessor2.Length != 0)
              layout = bufferAccessor2[index1];
            // ISSUE: reference to a compiler-generated method
            if (this.IsSecured(involvedInAccident))
            {
              if (!flag)
              {
                if (nativeArray5.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  if ((double) nativeArray5[index1].m_Cleared >= 1.0 || this.m_SimulationFrame >= involvedInAccident.m_InvolvedFrame + 14400U)
                  {
                    // ISSUE: reference to a compiler-generated field
                    VehicleUtils.DeleteVehicle(this.m_CommandBuffer, unfilteredChunkIndex, entity, layout);
                    continue;
                  }
                }
                else
                {
                  if (nativeArray4.Length == 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (nativeArray7.Length == 0 || this.m_PrefabRefData.HasComponent(nativeArray7[index1].m_Controller))
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.StartVehicle(unfilteredChunkIndex, entity, blockedLanes);
                      // ISSUE: reference to a compiler-generated method
                      this.ClearAccident(unfilteredChunkIndex, entity);
                      continue;
                    }
                    // ISSUE: reference to a compiler-generated field
                    VehicleUtils.DeleteVehicle(this.m_CommandBuffer, unfilteredChunkIndex, entity, layout);
                    continue;
                  }
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_SimulationFrame >= involvedInAccident.m_InvolvedFrame + 14400U)
                  {
                    // ISSUE: reference to a compiler-generated field
                    VehicleUtils.DeleteVehicle(this.m_CommandBuffer, unfilteredChunkIndex, entity, layout);
                    continue;
                  }
                }
              }
              for (int index2 = 0; index2 < blockedLanes.Length; ++index2)
              {
                Entity lane = blockedLanes[index2].m_Lane;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_CarLaneData.HasComponent(lane) && (this.m_CarLaneData[lane].m_Flags & Game.Net.CarLaneFlags.IsSecured) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<PathfindUpdated>(unfilteredChunkIndex, lane, new PathfindUpdated());
                }
              }
            }
          }
        }
      }

      private void AddInjuries(
        int jobIndex,
        InvolvedInAccident involvedInAccident,
        DynamicBuffer<Passenger> passengers,
        ref Random random)
      {
        float severity = involvedInAccident.m_Severity;
        for (int index = 0; index < passengers.Length; ++index)
        {
          Entity passenger = passengers[index].m_Passenger;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ResidentData.HasComponent(passenger))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_AddImpactArchetype);
            Impact component = new Impact()
            {
              m_Event = involvedInAccident.m_Event,
              m_Target = passenger,
              m_Severity = random.NextFloat(severity)
            };
            severity *= random.NextFloat(0.8f, 0.9f);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Impact>(jobIndex, entity, component);
          }
        }
      }

      private bool IsSecured(InvolvedInAccident involvedInAccident)
      {
        // ISSUE: reference to a compiler-generated method
        Entity accidentSite1 = this.FindAccidentSite(involvedInAccident.m_Event);
        if (!(accidentSite1 != Entity.Null))
          return true;
        // ISSUE: reference to a compiler-generated field
        AccidentSite accidentSite2 = this.m_AccidentSiteData[accidentSite1];
        if ((accidentSite2.m_Flags & AccidentSiteFlags.MovingVehicles) != (AccidentSiteFlags) 0)
          return false;
        // ISSUE: reference to a compiler-generated field
        return (accidentSite2.m_Flags & AccidentSiteFlags.Secured) != (AccidentSiteFlags) 0 || this.m_SimulationFrame >= accidentSite2.m_CreationFrame + 14400U;
      }

      private void IgniteFire(int jobIndex, Entity entity, Entity _event, FireData fireData)
      {
        // ISSUE: reference to a compiler-generated field
        Ignite component = new Ignite()
        {
          m_Target = entity,
          m_Event = _event,
          m_Intensity = fireData.m_StartIntensity,
          m_RequestFrame = this.m_SimulationFrame
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_EventIgniteArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Ignite>(jobIndex, entity1, component);
      }

      private void StopVehicle(
        int jobIndex,
        Entity entity,
        DynamicBuffer<BlockedLane> blockedLanes)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Moving>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<TransformFrame>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<InterpolatedTransform>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Swaying>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Stopped>(jobIndex, entity, new Stopped());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
        for (int index = 0; index < blockedLanes.Length; ++index)
        {
          Entity lane = blockedLanes[index].m_Lane;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CarLaneData.HasComponent(lane))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, lane, new PathfindUpdated());
          }
        }
      }

      private void StartVehicle(
        int jobIndex,
        Entity entity,
        DynamicBuffer<BlockedLane> blockedLanes)
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
        this.m_CommandBuffer.AddComponent<Swaying>(jobIndex, entity, new Swaying());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
        for (int index = 0; index < blockedLanes.Length; ++index)
        {
          Entity lane = blockedLanes[index].m_Lane;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CarLaneData.HasComponent(lane))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, lane, new PathfindUpdated());
          }
        }
      }

      private void ClearAccident(int jobIndex, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<InvolvedInAccident>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<OutOfControl>(jobIndex, entity);
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
        AccidentVehicleSystem.AccidentVehicleJob.EdgeIterator iterator = new AccidentVehicleSystem.AccidentVehicleJob.EdgeIterator()
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
        this.m_NetSearchTree.Iterate<AccidentVehicleSystem.AccidentVehicleJob.EdgeIterator>(ref iterator);
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
      public ComponentTypeHandle<Damaged> __Game_Objects_Damaged_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> __Game_Common_Destroyed_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<OnFire> __Game_Events_OnFire_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<BlockedLane> __Game_Objects_BlockedLane_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Passenger> __Game_Vehicles_Passenger_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InvolvedInAccident> __Game_Events_InvolvedInAccident_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Controller> __Game_Vehicles_Controller_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Road> __Game_Net_Road_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AccidentSite> __Game_Events_AccidentSite_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Creatures.Resident> __Game_Creatures_Resident_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<FireData> __Game_Prefabs_FireData_RO_ComponentLookup;
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
        this.__Game_Objects_Damaged_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Damaged>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_OnFire_RO_ComponentTypeHandle = state.GetComponentTypeHandle<OnFire>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_BlockedLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<BlockedLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RO_BufferTypeHandle = state.GetBufferTypeHandle<Passenger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InvolvedInAccident_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InvolvedInAccident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentLookup = state.GetComponentLookup<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_AccidentSite_RO_ComponentLookup = state.GetComponentLookup<AccidentSite>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Resident_RO_ComponentLookup = state.GetComponentLookup<Game.Creatures.Resident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_FireData_RO_ComponentLookup = state.GetComponentLookup<FireData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_TargetElement_RO_BufferLookup = state.GetBufferLookup<TargetElement>(true);
      }
    }
  }
}
