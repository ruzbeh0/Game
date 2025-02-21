// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TaxiStandSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TaxiStandSystem : GameSystemBase
  {
    public const uint UPDATE_INTERVAL = 256;
    private EntityQuery m_StandQuery;
    private EntityArchetype m_VehicleRequestArchetype;
    private CitySystem m_CitySystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private TaxiStandSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_StandQuery = this.GetEntityQuery(ComponentType.ReadWrite<TaxiStand>(), ComponentType.ReadOnly<Game.Routes.TransportStop>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<TaxiRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_StandQuery);
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_City_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_TaxiRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_DispatchedRequest_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteVehicle_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TaxiStand_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_WaitingPassengers_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new TaxiStandSystem.TaxiStandTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_RouteLaneType = this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentTypeHandle,
        m_WaitingPassengersType = this.__TypeHandle.__Game_Routes_WaitingPassengers_RO_ComponentTypeHandle,
        m_TaxiStandType = this.__TypeHandle.__Game_Routes_TaxiStand_RW_ComponentTypeHandle,
        m_RouteVehicleType = this.__TypeHandle.__Game_Routes_RouteVehicle_RW_BufferTypeHandle,
        m_DispatchedRequestType = this.__TypeHandle.__Game_Routes_DispatchedRequest_RW_BufferTypeHandle,
        m_TaxiRequestData = this.__TypeHandle.__Game_Simulation_TaxiRequest_RO_ComponentLookup,
        m_CurrentRouteData = this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentLookup,
        m_DispatchedData = this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_CurrentDistrictData = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup,
        m_BorderDistrictData = this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentLookup,
        m_CityData = this.__TypeHandle.__Game_City_City_RO_ComponentLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_City = this.m_CitySystem.City,
        m_TaxiRequestArchetype = this.m_VehicleRequestArchetype,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<TaxiStandSystem.TaxiStandTickJob>(this.m_StandQuery, this.Dependency);
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
    public TaxiStandSystem()
    {
    }

    [BurstCompile]
    private struct TaxiStandTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<RouteLane> m_RouteLaneType;
      [ReadOnly]
      public ComponentTypeHandle<WaitingPassengers> m_WaitingPassengersType;
      public ComponentTypeHandle<TaxiStand> m_TaxiStandType;
      public BufferTypeHandle<RouteVehicle> m_RouteVehicleType;
      public BufferTypeHandle<DispatchedRequest> m_DispatchedRequestType;
      [ReadOnly]
      public ComponentLookup<TaxiRequest> m_TaxiRequestData;
      [ReadOnly]
      public ComponentLookup<CurrentRoute> m_CurrentRouteData;
      [ReadOnly]
      public ComponentLookup<Dispatched> m_DispatchedData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> m_BorderDistrictData;
      [ReadOnly]
      public ComponentLookup<Game.City.City> m_CityData;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public Entity m_City;
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
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TaxiStand> nativeArray3 = chunk.GetNativeArray<TaxiStand>(ref this.m_TaxiStandType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<RouteLane> nativeArray4 = chunk.GetNativeArray<RouteLane>(ref this.m_RouteLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaitingPassengers> nativeArray5 = chunk.GetNativeArray<WaitingPassengers>(ref this.m_WaitingPassengersType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<RouteVehicle> bufferAccessor1 = chunk.GetBufferAccessor<RouteVehicle>(ref this.m_RouteVehicleType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<DispatchedRequest> bufferAccessor2 = chunk.GetBufferAccessor<DispatchedRequest>(ref this.m_DispatchedRequestType);
        ushort num = 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (CityUtils.CheckOption(this.m_CityData[this.m_City], CityOption.PaidTaxiStart))
        {
          float f = 0.0f;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          CityUtils.ApplyModifier(ref f, this.m_CityModifiers[this.m_City], CityModifierType.TaxiStartingFee);
          num = (ushort) math.clamp(Mathf.RoundToInt(f), 0, (int) ushort.MaxValue);
        }
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          TaxiStand taxiStand = nativeArray3[index];
          WaitingPassengers waitingPassengers = nativeArray5[index];
          DynamicBuffer<RouteVehicle> vehicles = bufferAccessor1[index];
          DynamicBuffer<DispatchedRequest> requests = bufferAccessor2[index];
          int maxTaxiCount = RouteUtils.GetMaxTaxiCount(waitingPassengers);
          int count;
          // ISSUE: reference to a compiler-generated method
          this.CheckVehicles(entity, vehicles, out count);
          // ISSUE: reference to a compiler-generated method
          this.CheckRequests(ref taxiStand, requests);
          if (count < maxTaxiCount)
          {
            taxiStand.m_Flags |= TaxiStandFlags.RequireVehicles;
            count += requests.Length;
            if (count < maxTaxiCount)
            {
              Entity endLane = Entity.Null;
              if (nativeArray4.Length != 0)
                endLane = nativeArray4[index].m_EndLane;
              // ISSUE: reference to a compiler-generated method
              this.RequestNewVehicleIfNeeded(unfilteredChunkIndex, entity, endLane, taxiStand, maxTaxiCount - count);
            }
          }
          else
            taxiStand.m_Flags &= ~TaxiStandFlags.RequireVehicles;
          if ((int) taxiStand.m_StartingFee != (int) num)
          {
            taxiStand.m_StartingFee = num;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<PathfindUpdated>(unfilteredChunkIndex, entity, new PathfindUpdated());
          }
          nativeArray3[index] = taxiStand;
        }
      }

      private void CheckVehicles(Entity route, DynamicBuffer<RouteVehicle> vehicles, out int count)
      {
        count = 0;
        while (count < vehicles.Length)
        {
          Entity vehicle = vehicles[count].m_Vehicle;
          CurrentRoute currentRoute = new CurrentRoute();
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentRouteData.HasComponent(vehicle))
          {
            // ISSUE: reference to a compiler-generated field
            currentRoute = this.m_CurrentRouteData[vehicle];
          }
          if (currentRoute.m_Route == route)
            ++count;
          else
            vehicles.RemoveAt(count);
        }
      }

      private void CheckRequests(ref TaxiStand taxiStand, DynamicBuffer<DispatchedRequest> requests)
      {
        for (int index = 0; index < requests.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_TaxiRequestData.HasComponent(requests[index].m_VehicleRequest))
            requests.RemoveAtSwapBack(index--);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_DispatchedData.HasComponent(taxiStand.m_TaxiRequest))
          return;
        requests.Add(new DispatchedRequest()
        {
          m_VehicleRequest = taxiStand.m_TaxiRequest
        });
        taxiStand.m_TaxiRequest = Entity.Null;
      }

      private void RequestNewVehicleIfNeeded(
        int jobIndex,
        Entity entity,
        Entity lane,
        TaxiStand taxiStand,
        int priority)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_TaxiRequestData.HasComponent(taxiStand.m_TaxiRequest))
          return;
        Entity district1;
        Entity district2;
        // ISSUE: reference to a compiler-generated method
        this.GetDistricts(lane, out district1, out district2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_TaxiRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<TaxiRequest>(jobIndex, entity1, new TaxiRequest(entity, district1, district2, TaxiRequestType.Stand, priority));
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<RouteLane> __Game_Routes_RouteLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaitingPassengers> __Game_Routes_WaitingPassengers_RO_ComponentTypeHandle;
      public ComponentTypeHandle<TaxiStand> __Game_Routes_TaxiStand_RW_ComponentTypeHandle;
      public BufferTypeHandle<RouteVehicle> __Game_Routes_RouteVehicle_RW_BufferTypeHandle;
      public BufferTypeHandle<DispatchedRequest> __Game_Routes_DispatchedRequest_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<TaxiRequest> __Game_Simulation_TaxiRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentRoute> __Game_Routes_CurrentRoute_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Dispatched> __Game_Simulation_Dispatched_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> __Game_Areas_BorderDistrict_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.City.City> __Game_City_City_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RouteLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_WaitingPassengers_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaitingPassengers>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TaxiStand_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TaxiStand>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteVehicle_RW_BufferTypeHandle = state.GetBufferTypeHandle<RouteVehicle>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_DispatchedRequest_RW_BufferTypeHandle = state.GetBufferTypeHandle<DispatchedRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_TaxiRequest_RO_ComponentLookup = state.GetComponentLookup<TaxiRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_CurrentRoute_RO_ComponentLookup = state.GetComponentLookup<CurrentRoute>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Dispatched_RO_ComponentLookup = state.GetComponentLookup<Dispatched>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentLookup = state.GetComponentLookup<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_BorderDistrict_RO_ComponentLookup = state.GetComponentLookup<BorderDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_City_RO_ComponentLookup = state.GetComponentLookup<Game.City.City>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
