// Decompiled with JetBrains decompiler
// Type: Game.Simulation.RideNeederSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class RideNeederSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 256;
    private EntityQuery m_NeederQuery;
    private EntityArchetype m_VehicleRequestArchetype;
    private EndFrameBarrier m_EndFrameBarrier;
    private RideNeederSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_NeederQuery = this.GetEntityQuery(ComponentType.ReadOnly<RideNeeder>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<TaxiRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_NeederQuery);
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_TaxiRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_RideNeeder_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new RideNeederSystem.RideNeederTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_RideNeederType = this.__TypeHandle.__Game_Creatures_RideNeeder_RO_ComponentTypeHandle,
        m_HumanCurrentLaneType = this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle,
        m_TaxiRequestData = this.__TypeHandle.__Game_Simulation_TaxiRequest_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_CurrentDistrictData = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup,
        m_BorderDistrictData = this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_TaxiRequestArchetype = this.m_VehicleRequestArchetype,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<RideNeederSystem.RideNeederTickJob>(this.m_NeederQuery, this.Dependency);
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
    public RideNeederSystem()
    {
    }

    [BurstCompile]
    private struct RideNeederTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<RideNeeder> m_RideNeederType;
      [ReadOnly]
      public ComponentTypeHandle<HumanCurrentLane> m_HumanCurrentLaneType;
      [ReadOnly]
      public ComponentLookup<TaxiRequest> m_TaxiRequestData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> m_BorderDistrictData;
      [ReadOnly]
      public ComponentLookup<ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public EntityArchetype m_TaxiRequestArchetype;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<RideNeeder> nativeArray2 = chunk.GetNativeArray<RideNeeder>(ref this.m_RideNeederType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HumanCurrentLane> nativeArray3 = chunk.GetNativeArray<HumanCurrentLane>(ref this.m_HumanCurrentLaneType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          RideNeeder rideNeeder = nativeArray2[index];
          if (nativeArray3.Length == 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<RideNeeder>(unfilteredChunkIndex, entity);
          }
          else
          {
            HumanCurrentLane humanCurrentLane = nativeArray3[index];
            if ((nativeArray3[index].m_Flags & (CreatureLaneFlags.ParkingSpace | CreatureLaneFlags.Taxi)) != (CreatureLaneFlags.ParkingSpace | CreatureLaneFlags.Taxi))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<RideNeeder>(unfilteredChunkIndex, entity);
            }
            else
            {
              TaxiRequestType type = TaxiRequestType.Customer;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConnectionLaneData.HasComponent(humanCurrentLane.m_Lane) && (this.m_ConnectionLaneData[humanCurrentLane.m_Lane].m_Flags & ConnectionLaneFlags.Outside) != (ConnectionLaneFlags) 0)
                type = TaxiRequestType.Outside;
              // ISSUE: reference to a compiler-generated method
              this.RequestNewVehicleIfNeeded(unfilteredChunkIndex, entity, humanCurrentLane.m_Lane, rideNeeder, type, 1);
            }
          }
        }
      }

      private void RequestNewVehicleIfNeeded(
        int jobIndex,
        Entity entity,
        Entity lane,
        RideNeeder rideNeeder,
        TaxiRequestType type,
        int priority)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_TaxiRequestData.HasComponent(rideNeeder.m_RideRequest))
          return;
        Entity district1;
        Entity district2;
        // ISSUE: reference to a compiler-generated method
        this.GetDistricts(lane, out district1, out district2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_TaxiRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<TaxiRequest>(jobIndex, entity1, new TaxiRequest(entity, district1, district2, type, priority));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(16U));
      }

      private void GetDistricts(Entity entity, out Entity district1, out Entity district2)
      {
        BorderDistrict componentData1;
        Owner componentData2;
        // ISSUE: reference to a compiler-generated field
        for (; !this.m_BorderDistrictData.TryGetComponent(entity, out componentData1); entity = componentData2.m_Owner)
        {
          CurrentDistrict componentData3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentDistrictData.TryGetComponent(entity, out componentData3))
          {
            district1 = componentData3.m_District;
            district2 = componentData3.m_District;
            return;
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_OwnerData.TryGetComponent(entity, out componentData2))
          {
            district1 = Entity.Null;
            district2 = Entity.Null;
            return;
          }
        }
        district1 = componentData1.m_Left;
        district2 = componentData1.m_Right;
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
      public ComponentTypeHandle<RideNeeder> __Game_Creatures_RideNeeder_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HumanCurrentLane> __Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<TaxiRequest> __Game_Simulation_TaxiRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> __Game_Areas_BorderDistrict_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_RideNeeder_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RideNeeder>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HumanCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_TaxiRequest_RO_ComponentLookup = state.GetComponentLookup<TaxiRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentLookup = state.GetComponentLookup<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_BorderDistrict_RO_ComponentLookup = state.GetComponentLookup<BorderDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<ConnectionLane>(true);
      }
    }
  }
}
