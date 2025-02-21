// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.DispatchedVehiclesSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Citizens;
using Game.Common;
using Game.Events;
using Game.Simulation;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class DispatchedVehiclesSection : InfoSectionBase
  {
    private EntityQuery m_ServiceDispatchQuery;
    private NativeList<Entity> m_VehiclesResult;
    private DispatchedVehiclesSection.TypeHandle __TypeHandle;

    protected override string group => nameof (DispatchedVehiclesSection);

    protected override bool displayForDestroyedObjects => true;

    private NativeList<VehiclesSection.UIVehicle> vehicleList { get; set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceDispatchQuery = this.GetEntityQuery(ComponentType.ReadOnly<Vehicle>(), ComponentType.ReadOnly<ServiceDispatch>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_VehiclesResult = new NativeList<Entity>(5, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.vehicleList = new NativeList<VehiclesSection.UIVehicle>(5, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      this.vehicleList.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_VehiclesResult.Dispose();
      base.OnDestroy();
    }

    protected override void Reset()
    {
      this.vehicleList.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_VehiclesResult.Clear();
    }

    private bool Visible() => this.m_VehiclesResult.Length > 0;

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_MaintenanceVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Hearse_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PoliceCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Ambulance_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_FireEngine_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InDanger_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_AccidentSite_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_OnFire_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_GarbageCollectionRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_EvacuationRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_HealthcareRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_PoliceEmergencyRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_FireRescueRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new DispatchedVehiclesSection.CollectDispatchedVehiclesJob()
      {
        m_SelectedEntity = this.selectedEntity,
        m_EntityHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ServiceDispatchHandle = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferTypeHandle,
        m_CurrentBuildingFromEntity = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup,
        m_FireRequestFromEntity = this.__TypeHandle.__Game_Simulation_FireRescueRequest_RO_ComponentLookup,
        m_PoliceRequestFromEntity = this.__TypeHandle.__Game_Simulation_PoliceEmergencyRequest_RO_ComponentLookup,
        m_HealthcareRequestFromEntity = this.__TypeHandle.__Game_Simulation_HealthcareRequest_RO_ComponentLookup,
        m_EvacuationRequestFromEntity = this.__TypeHandle.__Game_Simulation_EvacuationRequest_RO_ComponentLookup,
        m_GarbageCollectionRequest = this.__TypeHandle.__Game_Simulation_GarbageCollectionRequest_RO_ComponentLookup,
        m_OnFireFromEntity = this.__TypeHandle.__Game_Events_OnFire_RO_ComponentLookup,
        m_DestroyedFromEntity = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_AccidentSiteFromEntity = this.__TypeHandle.__Game_Events_AccidentSite_RO_ComponentLookup,
        m_InDangerFromEntity = this.__TypeHandle.__Game_Events_InDanger_RO_ComponentLookup,
        m_FireEngineFromEntity = this.__TypeHandle.__Game_Vehicles_FireEngine_RO_ComponentLookup,
        m_AmbulanceFromEntity = this.__TypeHandle.__Game_Vehicles_Ambulance_RO_ComponentLookup,
        m_PoliceCarFromEntity = this.__TypeHandle.__Game_Vehicles_PoliceCar_RO_ComponentLookup,
        m_PublicTransportFromEntity = this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup,
        m_HearseFromEntity = this.__TypeHandle.__Game_Vehicles_Hearse_RO_ComponentLookup,
        m_MaintenanceVehicleFromEntity = this.__TypeHandle.__Game_Vehicles_MaintenanceVehicle_RO_ComponentLookup,
        m_MaintenanceRequest = this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup,
        m_VehiclesResult = this.m_VehiclesResult
      }.Schedule<DispatchedVehiclesSection.CollectDispatchedVehiclesJob>(this.m_ServiceDispatchQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated method
      this.visible = this.Visible();
    }

    protected override void OnProcess()
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_VehiclesResult.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        VehiclesSection.AddVehicle(this.EntityManager, this.m_VehiclesResult[index], this.vehicleList);
      }
      this.vehicleList.Sort<VehiclesSection.UIVehicle>();
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("vehicleList");
      writer.ArrayBegin(this.vehicleList.Length);
      for (int index = 0; index < this.vehicleList.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        VehiclesSection.BindVehicle(this.m_NameSystem, writer, this.vehicleList[index]);
      }
      writer.ArrayEnd();
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
    public DispatchedVehiclesSection()
    {
    }

    [BurstCompile]
    private struct CollectDispatchedVehiclesJob : IJobChunk
    {
      [ReadOnly]
      public Entity m_SelectedEntity;
      [ReadOnly]
      public EntityTypeHandle m_EntityHandle;
      [ReadOnly]
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchHandle;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> m_CurrentBuildingFromEntity;
      [ReadOnly]
      public ComponentLookup<FireRescueRequest> m_FireRequestFromEntity;
      [ReadOnly]
      public ComponentLookup<PoliceEmergencyRequest> m_PoliceRequestFromEntity;
      [ReadOnly]
      public ComponentLookup<HealthcareRequest> m_HealthcareRequestFromEntity;
      [ReadOnly]
      public ComponentLookup<EvacuationRequest> m_EvacuationRequestFromEntity;
      [ReadOnly]
      public ComponentLookup<GarbageCollectionRequest> m_GarbageCollectionRequest;
      [ReadOnly]
      public ComponentLookup<MaintenanceRequest> m_MaintenanceRequest;
      [ReadOnly]
      public ComponentLookup<OnFire> m_OnFireFromEntity;
      [ReadOnly]
      public ComponentLookup<Game.Common.Destroyed> m_DestroyedFromEntity;
      [ReadOnly]
      public ComponentLookup<AccidentSite> m_AccidentSiteFromEntity;
      [ReadOnly]
      public ComponentLookup<InDanger> m_InDangerFromEntity;
      [ReadOnly]
      public ComponentLookup<FireEngine> m_FireEngineFromEntity;
      [ReadOnly]
      public ComponentLookup<PoliceCar> m_PoliceCarFromEntity;
      [ReadOnly]
      public ComponentLookup<Ambulance> m_AmbulanceFromEntity;
      [ReadOnly]
      public ComponentLookup<PublicTransport> m_PublicTransportFromEntity;
      [ReadOnly]
      public ComponentLookup<Hearse> m_HearseFromEntity;
      [ReadOnly]
      public ComponentLookup<MaintenanceVehicle> m_MaintenanceVehicleFromEntity;
      public NativeList<Entity> m_VehiclesResult;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityHandle);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          Entity entity = nativeArray[index1];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_VehiclesResult.Contains<Entity, Entity>(entity))
          {
            int y = 0;
            FireEngine componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_FireEngineFromEntity.TryGetComponent(entity, out componentData1))
              y = componentData1.m_RequestCount;
            PoliceCar componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PoliceCarFromEntity.TryGetComponent(entity, out componentData2))
              y = componentData2.m_RequestCount;
            Ambulance componentData3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_AmbulanceFromEntity.TryGetComponent(entity, out componentData3) && (componentData3.m_State & AmbulanceFlags.Dispatched) != (AmbulanceFlags) 0)
              y = 1;
            PublicTransport componentData4;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PublicTransportFromEntity.TryGetComponent(entity, out componentData4))
              y = componentData4.m_RequestCount;
            Hearse componentData5;
            // ISSUE: reference to a compiler-generated field
            if (this.m_HearseFromEntity.TryGetComponent(entity, out componentData5) && (componentData5.m_State & HearseFlags.Dispatched) != (HearseFlags) 0)
              y = 1;
            MaintenanceVehicle componentData6;
            // ISSUE: reference to a compiler-generated field
            if (this.m_MaintenanceVehicleFromEntity.TryGetComponent(entity, out componentData6))
              y = componentData6.m_RequestCount;
            DynamicBuffer<ServiceDispatch> dynamicBuffer = bufferAccessor[index1];
            for (int index2 = 0; index2 < math.min(dynamicBuffer.Length, y); ++index2)
            {
              ServiceDispatch serviceDispatch = dynamicBuffer[index2];
              FireRescueRequest componentData7;
              OnFire componentData8;
              Game.Common.Destroyed componentData9;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_FireRequestFromEntity.TryGetComponent(serviceDispatch.m_Request, out componentData7) && (componentData7.m_Target == this.m_SelectedEntity || this.m_OnFireFromEntity.TryGetComponent(componentData7.m_Target, out componentData8) && componentData8.m_Event == this.m_SelectedEntity || this.m_DestroyedFromEntity.TryGetComponent(componentData7.m_Target, out componentData9) && componentData9.m_Event == this.m_SelectedEntity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_VehiclesResult.Add(in entity);
              }
              PoliceEmergencyRequest componentData10;
              AccidentSite componentData11;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PoliceRequestFromEntity.TryGetComponent(serviceDispatch.m_Request, out componentData10) && (componentData10.m_Target == this.m_SelectedEntity || this.m_AccidentSiteFromEntity.TryGetComponent(componentData10.m_Target, out componentData11) && componentData11.m_Event == this.m_SelectedEntity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_VehiclesResult.Add(in entity);
              }
              HealthcareRequest componentData12;
              CurrentBuilding componentData13;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_HealthcareRequestFromEntity.TryGetComponent(serviceDispatch.m_Request, out componentData12) && (componentData12.m_Citizen == this.m_SelectedEntity || this.m_CurrentBuildingFromEntity.TryGetComponent(componentData12.m_Citizen, out componentData13) && componentData13.m_CurrentBuilding == this.m_SelectedEntity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_VehiclesResult.Add(in entity);
              }
              EvacuationRequest componentData14;
              InDanger componentData15;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_EvacuationRequestFromEntity.TryGetComponent(serviceDispatch.m_Request, out componentData14) && (componentData14.m_Target == this.m_SelectedEntity || this.m_InDangerFromEntity.TryGetComponent(componentData14.m_Target, out componentData15) && componentData15.m_Event == this.m_SelectedEntity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_VehiclesResult.Add(in entity);
              }
              GarbageCollectionRequest componentData16;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_GarbageCollectionRequest.TryGetComponent(serviceDispatch.m_Request, out componentData16) && componentData16.m_Target == this.m_SelectedEntity)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_VehiclesResult.Add(in entity);
              }
              MaintenanceRequest componentData17;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_MaintenanceRequest.TryGetComponent(serviceDispatch.m_Request, out componentData17) && componentData17.m_Target == this.m_SelectedEntity)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_VehiclesResult.Add(in entity);
              }
            }
          }
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

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ServiceDispatch> __Game_Simulation_ServiceDispatch_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<FireRescueRequest> __Game_Simulation_FireRescueRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PoliceEmergencyRequest> __Game_Simulation_PoliceEmergencyRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthcareRequest> __Game_Simulation_HealthcareRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EvacuationRequest> __Game_Simulation_EvacuationRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageCollectionRequest> __Game_Simulation_GarbageCollectionRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OnFire> __Game_Events_OnFire_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Common.Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AccidentSite> __Game_Events_AccidentSite_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<InDanger> __Game_Events_InDanger_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<FireEngine> __Game_Vehicles_FireEngine_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Ambulance> __Game_Vehicles_Ambulance_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PoliceCar> __Game_Vehicles_PoliceCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PublicTransport> __Game_Vehicles_PublicTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Hearse> __Game_Vehicles_Hearse_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MaintenanceVehicle> __Game_Vehicles_MaintenanceVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MaintenanceRequest> __Game_Simulation_MaintenanceRequest_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RO_BufferTypeHandle = state.GetBufferTypeHandle<ServiceDispatch>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentLookup = state.GetComponentLookup<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_FireRescueRequest_RO_ComponentLookup = state.GetComponentLookup<FireRescueRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_PoliceEmergencyRequest_RO_ComponentLookup = state.GetComponentLookup<PoliceEmergencyRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_HealthcareRequest_RO_ComponentLookup = state.GetComponentLookup<HealthcareRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_EvacuationRequest_RO_ComponentLookup = state.GetComponentLookup<EvacuationRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_GarbageCollectionRequest_RO_ComponentLookup = state.GetComponentLookup<GarbageCollectionRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_OnFire_RO_ComponentLookup = state.GetComponentLookup<OnFire>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Game.Common.Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_AccidentSite_RO_ComponentLookup = state.GetComponentLookup<AccidentSite>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InDanger_RO_ComponentLookup = state.GetComponentLookup<InDanger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_FireEngine_RO_ComponentLookup = state.GetComponentLookup<FireEngine>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Ambulance_RO_ComponentLookup = state.GetComponentLookup<Ambulance>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PoliceCar_RO_ComponentLookup = state.GetComponentLookup<PoliceCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RO_ComponentLookup = state.GetComponentLookup<PublicTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Hearse_RO_ComponentLookup = state.GetComponentLookup<Hearse>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_MaintenanceVehicle_RO_ComponentLookup = state.GetComponentLookup<MaintenanceVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup = state.GetComponentLookup<MaintenanceRequest>(true);
      }
    }
  }
}
