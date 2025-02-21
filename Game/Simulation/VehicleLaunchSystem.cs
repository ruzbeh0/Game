// Decompiled with JetBrains decompiler
// Type: Game.Simulation.VehicleLaunchSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Events;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class VehicleLaunchSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 64;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_LaunchQuery;
    private VehicleLaunchSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_LaunchQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Events.VehicleLaunch>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_LaunchQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_VehicleLaunchData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Produced_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_SpectatorSite_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_VehicleLaunch_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_TargetElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Duration_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
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
      VehicleLaunchSystem.VehicleLaunchJob jobData = new VehicleLaunchSystem.VehicleLaunchJob()
      {
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_DurationType = this.__TypeHandle.__Game_Events_Duration_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_TargetElementType = this.__TypeHandle.__Game_Events_TargetElement_RO_BufferTypeHandle,
        m_VehicleLaunchType = this.__TypeHandle.__Game_Events_VehicleLaunch_RW_ComponentTypeHandle,
        m_SpectatorSiteData = this.__TypeHandle.__Game_Events_SpectatorSite_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_ProducedData = this.__TypeHandle.__Game_Vehicles_Produced_RO_ComponentLookup,
        m_VehicleLaunchData = this.__TypeHandle.__Game_Prefabs_VehicleLaunchData_RO_ComponentLookup,
        m_OwnedVehicles = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<VehicleLaunchSystem.VehicleLaunchJob>(this.m_LaunchQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(this.Dependency);
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
    public VehicleLaunchSystem()
    {
    }

    [BurstCompile]
    private struct VehicleLaunchJob : IJobChunk
    {
      [ReadOnly]
      public uint m_SimulationFrame;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Duration> m_DurationType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<TargetElement> m_TargetElementType;
      public ComponentTypeHandle<Game.Events.VehicleLaunch> m_VehicleLaunchType;
      [ReadOnly]
      public ComponentLookup<SpectatorSite> m_SpectatorSiteData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<Produced> m_ProducedData;
      [ReadOnly]
      public ComponentLookup<VehicleLaunchData> m_VehicleLaunchData;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> m_OwnedVehicles;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElements;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Duration> nativeArray2 = chunk.GetNativeArray<Duration>(ref this.m_DurationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Events.VehicleLaunch> nativeArray3 = chunk.GetNativeArray<Game.Events.VehicleLaunch>(ref this.m_VehicleLaunchType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TargetElement> bufferAccessor = chunk.GetBufferAccessor<TargetElement>(ref this.m_TargetElementType);
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Entity eventEntity = nativeArray1[index];
          Game.Events.VehicleLaunch vehicleLaunch = nativeArray3[index];
          PrefabRef prefabRef = nativeArray4[index];
          if ((vehicleLaunch.m_Flags & VehicleLaunchFlags.PathRequested) == (VehicleLaunchFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (nativeArray2.Length == 0 || nativeArray2[index].m_StartFrame <= this.m_SimulationFrame)
            {
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              Entity producedVehicle = this.FindProducedVehicle(this.FindSpectatorSite(bufferAccessor[index], eventEntity));
              // ISSUE: reference to a compiler-generated field
              VehicleLaunchData vehicleLaunchData = this.m_VehicleLaunchData[prefabRef.m_Prefab];
              // ISSUE: reference to a compiler-generated method
              this.FindPath(unfilteredChunkIndex, eventEntity, producedVehicle, vehicleLaunchData);
              vehicleLaunch.m_Flags |= VehicleLaunchFlags.PathRequested;
            }
            else
              continue;
          }
          else if ((vehicleLaunch.m_Flags & VehicleLaunchFlags.Launched) == (VehicleLaunchFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            Entity producedVehicle = this.FindProducedVehicle(this.FindSpectatorSite(bufferAccessor[index], eventEntity));
            // ISSUE: reference to a compiler-generated method
            this.LaunchVehicle(unfilteredChunkIndex, eventEntity, producedVehicle);
            vehicleLaunch.m_Flags |= VehicleLaunchFlags.Launched;
          }
          nativeArray3[index] = vehicleLaunch;
        }
      }

      private Entity FindSpectatorSite(
        DynamicBuffer<TargetElement> targetElements,
        Entity eventEntity)
      {
        for (int index = 0; index < targetElements.Length; ++index)
        {
          Entity entity = targetElements[index].m_Entity;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_SpectatorSiteData.HasComponent(entity) && this.m_SpectatorSiteData[entity].m_Event == eventEntity)
            return entity;
        }
        return Entity.Null;
      }

      private Entity FindProducedVehicle(Entity siteEntity)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnedVehicles.HasBuffer(siteEntity))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<OwnedVehicle> ownedVehicle = this.m_OwnedVehicles[siteEntity];
          for (int index = 0; index < ownedVehicle.Length; ++index)
          {
            Entity vehicle = ownedVehicle[index].m_Vehicle;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ProducedData.HasComponent(vehicle))
              return vehicle;
          }
        }
        return Entity.Null;
      }

      private void FindPath(
        int jobIndex,
        Entity eventEntity,
        Entity vehicleEntity,
        VehicleLaunchData vehicleLaunchData)
      {
        if (vehicleEntity != Entity.Null)
        {
          PathfindParameters parameters = new PathfindParameters()
          {
            m_MaxSpeed = (float2) 277.777771f,
            m_WalkSpeed = (float2) 5.555556f,
            m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
            m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic
          };
          SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
          setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
          setupQueueTarget.m_Entity = vehicleEntity;
          SetupQueueTarget origin = setupQueueTarget;
          setupQueueTarget = new SetupQueueTarget();
          setupQueueTarget.m_Type = SetupTargetType.OutsideConnection;
          setupQueueTarget.m_Value2 = 1000f;
          SetupQueueTarget destination = setupQueueTarget;
          if (vehicleLaunchData.m_TransportType == TransportType.Rocket)
          {
            parameters.m_Methods = PathMethod.Road | PathMethod.Flying;
            origin.m_Methods = PathMethod.Road;
            origin.m_RoadTypes = RoadTypes.Helicopter;
            destination.m_Methods = PathMethod.Flying;
            destination.m_FlyingTypes = RoadTypes.Helicopter;
            // ISSUE: reference to a compiler-generated field
            this.m_PathfindQueue.Enqueue(new SetupQueueItem(eventEntity, parameters, origin, destination));
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathInformation>(jobIndex, eventEntity, new PathInformation());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddBuffer<PathElement>(jobIndex, eventEntity);
      }

      private void LaunchVehicle(int jobIndex, Entity eventEntity, Entity vehicleEntity)
      {
        if (!(vehicleEntity != Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated field
        PathInformation pathInformation = this.m_PathInformationData[eventEntity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement = this.m_PathElements[eventEntity];
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Produced>(jobIndex, vehicleEntity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Target>(jobIndex, vehicleEntity, new Target(pathInformation.m_Destination));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PathOwner>(jobIndex, vehicleEntity, new PathOwner(PathFlags.Updated));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Game.Vehicles.PublicTransport>(jobIndex, vehicleEntity, new Game.Vehicles.PublicTransport()
        {
          m_State = PublicTransportFlags.Launched
        });
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> dynamicBuffer = this.m_CommandBuffer.SetBuffer<PathElement>(jobIndex, vehicleEntity);
        PathOwner sourceOwner = new PathOwner();
        DynamicBuffer<PathElement> targetElements = dynamicBuffer;
        PathUtils.CopyPath(pathElement, sourceOwner, 0, targetElements);
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
      public ComponentTypeHandle<Duration> __Game_Events_Duration_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<TargetElement> __Game_Events_TargetElement_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Events.VehicleLaunch> __Game_Events_VehicleLaunch_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<SpectatorSite> __Game_Events_SpectatorSite_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Produced> __Game_Vehicles_Produced_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<VehicleLaunchData> __Game_Prefabs_VehicleLaunchData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Duration_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Duration>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_TargetElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<TargetElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_VehicleLaunch_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Events.VehicleLaunch>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_SpectatorSite_RO_ComponentLookup = state.GetComponentLookup<SpectatorSite>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Produced_RO_ComponentLookup = state.GetComponentLookup<Produced>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_VehicleLaunchData_RO_ComponentLookup = state.GetComponentLookup<VehicleLaunchData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RO_BufferLookup = state.GetBufferLookup<OwnedVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferLookup = state.GetBufferLookup<PathElement>(true);
      }
    }
  }
}
