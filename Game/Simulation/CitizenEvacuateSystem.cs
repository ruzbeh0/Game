// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CitizenEvacuateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Events;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CitizenEvacuateSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_InDangerQuery;
    private EntityQuery m_CitizenQuery;
    private CitizenEvacuateSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_InDangerQuery = this.GetEntityQuery(ComponentType.ReadWrite<InDanger>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.ReadOnly<CurrentBuilding>(), ComponentType.Exclude<HealthProblem>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_InDangerQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CitizenQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint num = this.m_SimulationSystem.frameIndex >> 4 & 15U;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InDanger_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new CitizenEvacuateSystem.CitizenEvacuateJob()
      {
        m_UpdateFrameIndex = num,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_CurrentBuildingType = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle,
        m_TripNeededType = this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle,
        m_DispatchedData = this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentLookup,
        m_PublicTransportData = this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup,
        m_ServiceDispatches = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferLookup,
        m_InDangerData = this.__TypeHandle.__Game_Events_InDanger_RW_ComponentLookup
      }.ScheduleParallel<CitizenEvacuateSystem.CitizenEvacuateJob>(this.m_CitizenQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
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
    public CitizenEvacuateSystem()
    {
    }

    [BurstCompile]
    private struct CitizenEvacuateJob : IJobChunk
    {
      [ReadOnly]
      public uint m_UpdateFrameIndex;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> m_CurrentBuildingType;
      public BufferTypeHandle<TripNeeded> m_TripNeededType;
      [ReadOnly]
      public ComponentLookup<Dispatched> m_DispatchedData;
      [ReadOnly]
      public ComponentLookup<PublicTransport> m_PublicTransportData;
      [ReadOnly]
      public BufferLookup<ServiceDispatch> m_ServiceDispatches;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<InDanger> m_InDangerData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((int) chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index != (int) this.m_UpdateFrameIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentBuilding> nativeArray2 = chunk.GetNativeArray<CurrentBuilding>(ref this.m_CurrentBuildingType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TripNeeded> bufferAccessor = chunk.GetBufferAccessor<TripNeeded>(ref this.m_TripNeededType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          CurrentBuilding currentBuilding = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_InDangerData.HasComponent(currentBuilding.m_CurrentBuilding))
          {
            // ISSUE: reference to a compiler-generated field
            InDanger inDanger = this.m_InDangerData[currentBuilding.m_CurrentBuilding];
            if ((inDanger.m_Flags & DangerFlags.Evacuate) != (DangerFlags) 0)
            {
              if ((inDanger.m_Flags & DangerFlags.UseTransport) != (DangerFlags) 0)
              {
                Entity vehicle;
                // ISSUE: reference to a compiler-generated method
                if (this.GetBoardingVehicle(inDanger, out vehicle))
                {
                  DynamicBuffer<TripNeeded> tripNeededs = bufferAccessor[index];
                  // ISSUE: reference to a compiler-generated method
                  this.GoToVehicle(unfilteredChunkIndex, nativeArray1[index], tripNeededs, vehicle);
                  if ((inDanger.m_Flags & DangerFlags.WaitingCitizens) != (DangerFlags) 0)
                  {
                    inDanger.m_Flags &= ~DangerFlags.WaitingCitizens;
                    // ISSUE: reference to a compiler-generated field
                    this.m_InDangerData[currentBuilding.m_CurrentBuilding] = inDanger;
                  }
                }
                else if ((inDanger.m_Flags & DangerFlags.WaitingCitizens) == (DangerFlags) 0)
                {
                  inDanger.m_Flags |= DangerFlags.WaitingCitizens;
                  // ISSUE: reference to a compiler-generated field
                  this.m_InDangerData[currentBuilding.m_CurrentBuilding] = inDanger;
                }
              }
              else
              {
                DynamicBuffer<TripNeeded> tripNeededs = bufferAccessor[index];
                // ISSUE: reference to a compiler-generated method
                this.GoToShelter(unfilteredChunkIndex, nativeArray1[index], tripNeededs);
              }
            }
          }
        }
      }

      private bool GetBoardingVehicle(InDanger inDanger, out Entity vehicle)
      {
        vehicle = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_DispatchedData.HasComponent(inDanger.m_EvacuationRequest))
          return false;
        // ISSUE: reference to a compiler-generated field
        Dispatched dispatched = this.m_DispatchedData[inDanger.m_EvacuationRequest];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PublicTransportData.HasComponent(dispatched.m_Handler) || (this.m_PublicTransportData[dispatched.m_Handler].m_State & PublicTransportFlags.Boarding) == (PublicTransportFlags) 0 || !this.m_ServiceDispatches.HasBuffer(dispatched.m_Handler))
          return false;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ServiceDispatch> serviceDispatch = this.m_ServiceDispatches[dispatched.m_Handler];
        if (serviceDispatch.Length == 0 || serviceDispatch[0].m_Request != inDanger.m_EvacuationRequest)
          return false;
        vehicle = dispatched.m_Handler;
        return true;
      }

      private void GoToShelter(int jobIndex, Entity entity, DynamicBuffer<TripNeeded> tripNeededs)
      {
        tripNeededs.Clear();
        tripNeededs.Add(new TripNeeded()
        {
          m_Purpose = Purpose.EmergencyShelter
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<ResourceBuyer>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<TravelPurpose>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<AttendingMeeting>(jobIndex, entity);
      }

      private void GoToVehicle(
        int jobIndex,
        Entity entity,
        DynamicBuffer<TripNeeded> tripNeededs,
        Entity vehicle)
      {
        tripNeededs.Clear();
        tripNeededs.Add(new TripNeeded()
        {
          m_Purpose = Purpose.EmergencyShelter,
          m_TargetAgent = vehicle
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<ResourceBuyer>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<TravelPurpose>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<AttendingMeeting>(jobIndex, entity);
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
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
      public BufferTypeHandle<TripNeeded> __Game_Citizens_TripNeeded_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Dispatched> __Game_Simulation_Dispatched_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PublicTransport> __Game_Vehicles_PublicTransport_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ServiceDispatch> __Game_Simulation_ServiceDispatch_RO_BufferLookup;
      public ComponentLookup<InDanger> __Game_Events_InDanger_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TripNeeded_RW_BufferTypeHandle = state.GetBufferTypeHandle<TripNeeded>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Dispatched_RO_ComponentLookup = state.GetComponentLookup<Dispatched>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RO_ComponentLookup = state.GetComponentLookup<PublicTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RO_BufferLookup = state.GetBufferLookup<ServiceDispatch>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InDanger_RW_ComponentLookup = state.GetComponentLookup<InDanger>();
      }
    }
  }
}
