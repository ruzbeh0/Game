// Decompiled with JetBrains decompiler
// Type: Game.Simulation.HealthProblemSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Creatures;
using Game.Events;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using Game.Triggers;
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
  public class HealthProblemSystem : GameSystemBase
  {
    private const uint SYSTEM_UPDATE_INTERVAL = 16;
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private IconCommandSystem m_IconCommandSystem;
    private EntityArchetype m_HealthcareRequestArchetype;
    private EntityArchetype m_JournalDataArchetype;
    private EntityArchetype m_ResetTripArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private EntityQuery m_HealthProblemQuery;
    private EntityQuery m_HealthcareSettingsQuery;
    private EntityQuery m_FireSettingsQuery;
    private TriggerSystem m_TriggerSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private HealthProblemSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_HealthProblemQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadWrite<HealthProblem>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<HealthcareParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_FireSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<FireConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<HealthcareRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_JournalDataArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<AddEventJournalData>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_ResetTripArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<ResetTrip>());
      // ISSUE: reference to a compiler-generated field
      this.m_HandleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<HandleRequest>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HealthProblemQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HealthcareSettingsQuery);
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint num = this.m_SimulationSystem.frameIndex / 16U & 15U;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Divert_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Hearse_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Ambulance_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Static_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_OnFire_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_DeathcareFacility_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Hospital_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_HealthcareRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle deps;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      HealthProblemSystem.HealthProblemJob jobData = new HealthProblemSystem.HealthProblemJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CurrentBuildingType = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle,
        m_CurrentTransportType = this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentTypeHandle,
        m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle,
        m_TravelPurposeType = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_HealthProblemType = this.__TypeHandle.__Game_Citizens_HealthProblem_RW_ComponentTypeHandle,
        m_TripNeededType = this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle,
        m_HouseholdMemberType = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle,
        m_HealthcareRequestData = this.__TypeHandle.__Game_Simulation_HealthcareRequest_RO_ComponentLookup,
        m_HospitalData = this.__TypeHandle.__Game_Buildings_Hospital_RO_ComponentLookup,
        m_DeathcareFacilityData = this.__TypeHandle.__Game_Buildings_DeathcareFacility_RO_ComponentLookup,
        m_OnFireData = this.__TypeHandle.__Game_Events_OnFire_RO_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_DispatchedData = this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentLookup,
        m_TargetData = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
        m_StaticData = this.__TypeHandle.__Game_Objects_Static_RO_ComponentLookup,
        m_OutsideConnectionData = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_AmbulanceData = this.__TypeHandle.__Game_Vehicles_Ambulance_RO_ComponentLookup,
        m_HearseData = this.__TypeHandle.__Game_Vehicles_Hearse_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_DivertData = this.__TypeHandle.__Game_Creatures_Divert_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_TriggerBuffer = this.m_TriggerSystem.CreateActionBuffer().AsParallelWriter(),
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps).AsParallelWriter(),
        m_UpdateFrameIndex = num,
        m_RandomSeed = RandomSeed.Next(),
        m_HealthcareRequestArchetype = this.m_HealthcareRequestArchetype,
        m_JournalDataArchetype = this.m_JournalDataArchetype,
        m_ResetTripArchetype = this.m_ResetTripArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_HealthcareParameterData = this.m_HealthcareSettingsQuery.GetSingleton<HealthcareParameterData>(),
        m_FireConfigurationData = this.m_FireSettingsQuery.GetSingleton<FireConfigurationData>(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<HealthProblemSystem.HealthProblemJob>(this.m_HealthProblemQuery, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
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
    public HealthProblemSystem()
    {
    }

    [BurstCompile]
    private struct HealthProblemJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> m_CurrentBuildingType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentTransport> m_CurrentTransportType;
      [ReadOnly]
      public ComponentTypeHandle<Citizen> m_CitizenType;
      [ReadOnly]
      public ComponentTypeHandle<TravelPurpose> m_TravelPurposeType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public ComponentTypeHandle<HealthProblem> m_HealthProblemType;
      public BufferTypeHandle<TripNeeded> m_TripNeededType;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdMember> m_HouseholdMemberType;
      [ReadOnly]
      public ComponentLookup<HealthcareRequest> m_HealthcareRequestData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Hospital> m_HospitalData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.DeathcareFacility> m_DeathcareFacilityData;
      [ReadOnly]
      public ComponentLookup<OnFire> m_OnFireData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Dispatched> m_DispatchedData;
      [ReadOnly]
      public ComponentLookup<Target> m_TargetData;
      [ReadOnly]
      public ComponentLookup<Static> m_StaticData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnectionData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Ambulance> m_AmbulanceData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Hearse> m_HearseData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public ComponentLookup<Divert> m_DivertData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityArchetype m_HealthcareRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_JournalDataArchetype;
      [ReadOnly]
      public EntityArchetype m_ResetTripArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public HealthcareParameterData m_HealthcareParameterData;
      [ReadOnly]
      public FireConfigurationData m_FireConfigurationData;
      public IconCommandBuffer m_IconCommandBuffer;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;
      public NativeQueue<TriggerAction>.ParallelWriter m_TriggerBuffer;

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
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        int num1 = (int) ((double) this.m_HealthcareParameterData.m_TransportWarningTime * (15.0 / 64.0));
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Citizen> nativeArray2 = chunk.GetNativeArray<Citizen>(ref this.m_CitizenType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HealthProblem> nativeArray3 = chunk.GetNativeArray<HealthProblem>(ref this.m_HealthProblemType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentBuilding> nativeArray4 = chunk.GetNativeArray<CurrentBuilding>(ref this.m_CurrentBuildingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TravelPurpose> nativeArray5 = chunk.GetNativeArray<TravelPurpose>(ref this.m_TravelPurposeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentTransport> nativeArray6 = chunk.GetNativeArray<CurrentTransport>(ref this.m_CurrentTransportType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TripNeeded> bufferAccessor = chunk.GetBufferAccessor<TripNeeded>(ref this.m_TripNeededType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HouseholdMember> nativeArray7 = chunk.GetNativeArray<HouseholdMember>(ref this.m_HouseholdMemberType);
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          HealthProblem healthProblem = nativeArray3[index];
          CurrentBuilding currentBuilding = new CurrentBuilding();
          TravelPurpose travelPurpose = new TravelPurpose();
          CurrentTransport currentTransport = new CurrentTransport();
          if (nativeArray4.Length != 0)
            currentBuilding = nativeArray4[index];
          if (nativeArray5.Length != 0)
            travelPurpose = nativeArray5[index];
          if (nativeArray6.Length != 0)
            currentTransport = nativeArray6[index];
          if ((healthProblem.m_Flags & ~HealthProblemFlags.NoHealthcare) == HealthProblemFlags.None)
          {
            Entity e = nativeArray1[index];
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<HealthProblem>(unfilteredChunkIndex, e);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, e);
          }
          else
          {
            if ((healthProblem.m_Flags & (HealthProblemFlags.InDanger | HealthProblemFlags.Trapped)) != HealthProblemFlags.None)
            {
              if ((healthProblem.m_Flags & HealthProblemFlags.Dead) != HealthProblemFlags.None)
              {
                healthProblem.m_Flags &= ~(HealthProblemFlags.InDanger | HealthProblemFlags.Trapped);
                nativeArray3[index] = healthProblem;
              }
              else
              {
                Entity entity = nativeArray1[index];
                Citizen citizen = nativeArray2[index];
                DynamicBuffer<TripNeeded> tripNeededs = bufferAccessor[index];
                // ISSUE: reference to a compiler-generated field
                if (this.m_OnFireData.HasComponent(currentBuilding.m_CurrentBuilding))
                {
                  if ((healthProblem.m_Flags & HealthProblemFlags.InDanger) != HealthProblemFlags.None)
                  {
                    // ISSUE: reference to a compiler-generated field
                    OnFire onFire = this.m_OnFireData[currentBuilding.m_CurrentBuilding];
                    float num2 = (float) citizen.m_Health - onFire.m_Intensity * 0.5f;
                    if ((double) random.NextFloat(100f) < (double) num2)
                    {
                      if ((healthProblem.m_Flags & HealthProblemFlags.Trapped) == HealthProblemFlags.None)
                      {
                        healthProblem.m_Flags &= ~HealthProblemFlags.InDanger;
                        nativeArray3[index] = healthProblem;
                        // ISSUE: reference to a compiler-generated method
                        this.GoToSafety(unfilteredChunkIndex, entity, currentBuilding, travelPurpose, currentTransport, tripNeededs);
                      }
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      if ((healthProblem.m_Flags & HealthProblemFlags.Trapped) == HealthProblemFlags.None && (double) random.NextFloat() < (double) this.m_FireConfigurationData.m_DeathRateOfFireAccident)
                      {
                        if ((healthProblem.m_Flags & HealthProblemFlags.RequireTransport) != HealthProblemFlags.None)
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          this.m_IconCommandBuffer.Remove(entity, this.m_HealthcareParameterData.m_AmbulanceNotificationPrefab);
                          healthProblem.m_Timer = (byte) 0;
                        }
                        healthProblem.m_Flags &= ~(HealthProblemFlags.InDanger | HealthProblemFlags.Trapped);
                        healthProblem.m_Flags |= HealthProblemFlags.Dead | HealthProblemFlags.RequireTransport;
                        nativeArray3[index] = healthProblem;
                        // ISSUE: reference to a compiler-generated method
                        this.AddJournalData(unfilteredChunkIndex, healthProblem);
                        Entity household = nativeArray7.Length != 0 ? nativeArray7[index].m_Household : Entity.Null;
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        DeathCheckSystem.PerformAfterDeathActions(nativeArray1[index], household, this.m_TriggerBuffer, this.m_StatisticsEventQueue, ref this.m_HouseholdCitizens);
                      }
                      else
                      {
                        healthProblem.m_Flags |= HealthProblemFlags.Trapped;
                        nativeArray3[index] = healthProblem;
                      }
                    }
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_DestroyedData.HasComponent(currentBuilding.m_CurrentBuilding))
                  {
                    if ((healthProblem.m_Flags & HealthProblemFlags.InDanger) != HealthProblemFlags.None)
                    {
                      healthProblem.m_Flags &= ~HealthProblemFlags.InDanger;
                      nativeArray3[index] = healthProblem;
                    }
                    if ((healthProblem.m_Flags & HealthProblemFlags.Trapped) != HealthProblemFlags.None)
                    {
                      // ISSUE: reference to a compiler-generated field
                      Destroyed destroyed = this.m_DestroyedData[currentBuilding.m_CurrentBuilding];
                      if ((double) random.NextFloat(1f) < (double) destroyed.m_Cleared)
                      {
                        healthProblem.m_Flags &= ~HealthProblemFlags.Trapped;
                        nativeArray3[index] = healthProblem;
                        // ISSUE: reference to a compiler-generated method
                        this.GoToSafety(unfilteredChunkIndex, entity, currentBuilding, travelPurpose, currentTransport, tripNeededs);
                      }
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.GoToSafety(unfilteredChunkIndex, entity, currentBuilding, travelPurpose, currentTransport, tripNeededs);
                    }
                  }
                  else
                  {
                    healthProblem.m_Flags &= ~(HealthProblemFlags.InDanger | HealthProblemFlags.Trapped);
                    nativeArray3[index] = healthProblem;
                  }
                }
              }
            }
            if ((healthProblem.m_Flags & HealthProblemFlags.RequireTransport) != HealthProblemFlags.None)
            {
              if ((healthProblem.m_Flags & HealthProblemFlags.Dead) != HealthProblemFlags.None)
              {
                Entity entity1 = nativeArray1[index];
                DynamicBuffer<TripNeeded> tripNeededs = bufferAccessor[index];
                Entity entity2 = currentBuilding.m_CurrentBuilding;
                // ISSUE: reference to a compiler-generated field
                if (entity2 == Entity.Null && (travelPurpose.m_Purpose == Game.Citizens.Purpose.Deathcare || travelPurpose.m_Purpose == Game.Citizens.Purpose.Hospital) && this.m_TargetData.HasComponent(currentTransport.m_CurrentTransport))
                {
                  // ISSUE: reference to a compiler-generated field
                  entity2 = this.m_TargetData[currentTransport.m_CurrentTransport].m_Target;
                }
                // ISSUE: reference to a compiler-generated field
                if (this.m_DeathcareFacilityData.HasComponent(entity2))
                {
                  // ISSUE: reference to a compiler-generated field
                  if ((this.m_DeathcareFacilityData[entity2].m_Flags & (DeathcareFacilityFlags.CanProcessCorpses | DeathcareFacilityFlags.CanStoreCorpses)) != (DeathcareFacilityFlags) 0)
                  {
                    if (healthProblem.m_Timer > (byte) 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.m_IconCommandBuffer.Remove(entity1, this.m_HealthcareParameterData.m_HearseNotificationPrefab);
                      healthProblem.m_Timer = (byte) 0;
                      nativeArray3[index] = healthProblem;
                    }
                    // ISSUE: reference to a compiler-generated method
                    this.HandleRequest(unfilteredChunkIndex, healthProblem);
                    if (entity2 == currentBuilding.m_CurrentBuilding && travelPurpose.m_Purpose != Game.Citizens.Purpose.Deathcare && travelPurpose.m_Purpose != Game.Citizens.Purpose.InDeathcare)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.GoToDeathcare(unfilteredChunkIndex, entity1, currentBuilding, travelPurpose, currentTransport, tripNeededs, entity2);
                      continue;
                    }
                    continue;
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_HospitalData.HasComponent(entity2) && (this.m_HospitalData[entity2].m_Flags & HospitalFlags.CanProcessCorpses) != (HospitalFlags) 0)
                  {
                    if (healthProblem.m_Timer > (byte) 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.m_IconCommandBuffer.Remove(entity1, this.m_HealthcareParameterData.m_HearseNotificationPrefab);
                      healthProblem.m_Timer = (byte) 0;
                      nativeArray3[index] = healthProblem;
                    }
                    // ISSUE: reference to a compiler-generated method
                    this.HandleRequest(unfilteredChunkIndex, healthProblem);
                    if (entity2 == currentBuilding.m_CurrentBuilding && travelPurpose.m_Purpose != Game.Citizens.Purpose.Hospital && travelPurpose.m_Purpose != Game.Citizens.Purpose.InHospital)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.GoToHospital(unfilteredChunkIndex, entity1, currentBuilding, travelPurpose, currentTransport, tripNeededs, entity2, true);
                      continue;
                    }
                    continue;
                  }
                }
                // ISSUE: reference to a compiler-generated field
                if (!this.m_OutsideConnectionData.HasComponent(entity2))
                {
                  // ISSUE: reference to a compiler-generated method
                  if (this.RequestVehicleIfNeeded(unfilteredChunkIndex, entity1, currentBuilding, travelPurpose, currentTransport, tripNeededs, healthProblem))
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (currentTransport.m_CurrentTransport != Entity.Null || this.m_StaticData.HasComponent(currentBuilding.m_CurrentBuilding))
                    {
                      if ((int) healthProblem.m_Timer < num1)
                      {
                        if ((int) ++healthProblem.m_Timer == num1)
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          this.m_IconCommandBuffer.Add(entity1, this.m_HealthcareParameterData.m_HearseNotificationPrefab, IconPriority.MajorProblem);
                        }
                        nativeArray3[index] = healthProblem;
                      }
                    }
                    else if (healthProblem.m_Timer > (byte) 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.m_IconCommandBuffer.Remove(entity1, this.m_HealthcareParameterData.m_HearseNotificationPrefab);
                      healthProblem.m_Timer = (byte) 0;
                      nativeArray3[index] = healthProblem;
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_IconCommandBuffer.Remove(entity1, this.m_HealthcareParameterData.m_HearseNotificationPrefab);
                  }
                }
              }
              else if ((healthProblem.m_Flags & HealthProblemFlags.Injured) != HealthProblemFlags.None)
              {
                Entity entity3 = nativeArray1[index];
                DynamicBuffer<TripNeeded> tripNeededs = bufferAccessor[index];
                Entity entity4 = currentBuilding.m_CurrentBuilding;
                // ISSUE: reference to a compiler-generated field
                if (entity4 == Entity.Null && travelPurpose.m_Purpose == Game.Citizens.Purpose.Hospital && this.m_TargetData.HasComponent(currentTransport.m_CurrentTransport))
                {
                  // ISSUE: reference to a compiler-generated field
                  entity4 = this.m_TargetData[currentTransport.m_CurrentTransport].m_Target;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_HospitalData.HasComponent(entity4) && (this.m_HospitalData[entity4].m_Flags & HospitalFlags.CanCureInjury) != (HospitalFlags) 0)
                {
                  if (healthProblem.m_Timer > (byte) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_IconCommandBuffer.Remove(entity3, this.m_HealthcareParameterData.m_AmbulanceNotificationPrefab);
                    healthProblem.m_Timer = (byte) 0;
                    nativeArray3[index] = healthProblem;
                  }
                  // ISSUE: reference to a compiler-generated method
                  this.HandleRequest(unfilteredChunkIndex, healthProblem);
                  if (entity4 == currentBuilding.m_CurrentBuilding && travelPurpose.m_Purpose != Game.Citizens.Purpose.Hospital && travelPurpose.m_Purpose != Game.Citizens.Purpose.InHospital)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.GoToHospital(unfilteredChunkIndex, entity3, currentBuilding, travelPurpose, currentTransport, tripNeededs, entity4, true);
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_OutsideConnectionData.HasComponent(entity4))
                  {
                    // ISSUE: reference to a compiler-generated method
                    if (this.RequestVehicleIfNeeded(unfilteredChunkIndex, entity3, currentBuilding, travelPurpose, currentTransport, tripNeededs, healthProblem))
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (currentTransport.m_CurrentTransport != Entity.Null || this.m_StaticData.HasComponent(currentBuilding.m_CurrentBuilding))
                      {
                        if ((int) healthProblem.m_Timer < num1)
                        {
                          if ((int) ++healthProblem.m_Timer == num1)
                          {
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            this.m_IconCommandBuffer.Add(entity3, this.m_HealthcareParameterData.m_AmbulanceNotificationPrefab, IconPriority.MajorProblem);
                          }
                          nativeArray3[index] = healthProblem;
                        }
                      }
                      else if (healthProblem.m_Timer > (byte) 0)
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        this.m_IconCommandBuffer.Remove(entity3, this.m_HealthcareParameterData.m_AmbulanceNotificationPrefab);
                        healthProblem.m_Timer = (byte) 0;
                        nativeArray3[index] = healthProblem;
                      }
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.m_IconCommandBuffer.Remove(entity3, this.m_HealthcareParameterData.m_AmbulanceNotificationPrefab);
                    }
                  }
                }
              }
              else if ((healthProblem.m_Flags & (HealthProblemFlags.Sick | HealthProblemFlags.NoHealthcare)) == HealthProblemFlags.Sick)
              {
                Entity entity5 = nativeArray1[index];
                DynamicBuffer<TripNeeded> tripNeededs = bufferAccessor[index];
                Entity entity6 = currentBuilding.m_CurrentBuilding;
                // ISSUE: reference to a compiler-generated field
                if (entity6 == Entity.Null && travelPurpose.m_Purpose == Game.Citizens.Purpose.Hospital && this.m_TargetData.HasComponent(currentTransport.m_CurrentTransport))
                {
                  // ISSUE: reference to a compiler-generated field
                  entity6 = this.m_TargetData[currentTransport.m_CurrentTransport].m_Target;
                }
                if (entity6 == Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.HandleRequest(unfilteredChunkIndex, healthProblem);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_HospitalData.HasComponent(entity6) && (this.m_HospitalData[entity6].m_Flags & HospitalFlags.CanCureDisease) != (HospitalFlags) 0)
                  {
                    if (healthProblem.m_Timer > (byte) 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.m_IconCommandBuffer.Remove(entity5, this.m_HealthcareParameterData.m_AmbulanceNotificationPrefab);
                      healthProblem.m_Timer = (byte) 0;
                      nativeArray3[index] = healthProblem;
                    }
                    // ISSUE: reference to a compiler-generated method
                    this.HandleRequest(unfilteredChunkIndex, healthProblem);
                    if (entity6 == currentBuilding.m_CurrentBuilding && travelPurpose.m_Purpose != Game.Citizens.Purpose.Hospital && travelPurpose.m_Purpose != Game.Citizens.Purpose.InHospital)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.GoToHospital(unfilteredChunkIndex, entity5, currentBuilding, travelPurpose, currentTransport, tripNeededs, entity6, true);
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (!this.m_OutsideConnectionData.HasComponent(entity6))
                    {
                      // ISSUE: reference to a compiler-generated method
                      if (this.RequestVehicleIfNeeded(unfilteredChunkIndex, entity5, currentBuilding, travelPurpose, currentTransport, tripNeededs, healthProblem))
                      {
                        // ISSUE: reference to a compiler-generated field
                        if (currentTransport.m_CurrentTransport != Entity.Null || this.m_StaticData.HasComponent(currentBuilding.m_CurrentBuilding))
                        {
                          if ((int) healthProblem.m_Timer < num1)
                          {
                            if ((int) ++healthProblem.m_Timer == num1)
                            {
                              // ISSUE: reference to a compiler-generated field
                              // ISSUE: reference to a compiler-generated field
                              this.m_IconCommandBuffer.Add(entity5, this.m_HealthcareParameterData.m_AmbulanceNotificationPrefab, IconPriority.MajorProblem);
                            }
                            nativeArray3[index] = healthProblem;
                          }
                        }
                        else if (healthProblem.m_Timer > (byte) 0)
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          this.m_IconCommandBuffer.Remove(entity5, this.m_HealthcareParameterData.m_AmbulanceNotificationPrefab);
                          healthProblem.m_Timer = (byte) 0;
                          nativeArray3[index] = healthProblem;
                        }
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        this.m_IconCommandBuffer.Remove(entity5, this.m_HealthcareParameterData.m_AmbulanceNotificationPrefab);
                      }
                    }
                  }
                }
              }
            }
            else if ((healthProblem.m_Flags & (HealthProblemFlags.Dead | HealthProblemFlags.NoHealthcare)) == HealthProblemFlags.None)
            {
              if ((healthProblem.m_Flags & HealthProblemFlags.Sick) != HealthProblemFlags.None)
              {
                Entity entity7 = nativeArray1[index];
                DynamicBuffer<TripNeeded> tripNeededs = bufferAccessor[index];
                Entity entity8 = currentBuilding.m_CurrentBuilding;
                // ISSUE: reference to a compiler-generated field
                if (entity8 == Entity.Null && travelPurpose.m_Purpose == Game.Citizens.Purpose.Hospital && this.m_TargetData.HasComponent(currentTransport.m_CurrentTransport))
                {
                  // ISSUE: reference to a compiler-generated field
                  entity8 = this.m_TargetData[currentTransport.m_CurrentTransport].m_Target;
                }
                if (!(entity8 == Entity.Null))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_HospitalData.HasComponent(entity8) && (this.m_HospitalData[entity8].m_Flags & HospitalFlags.CanCureDisease) != (HospitalFlags) 0)
                  {
                    if (entity8 == currentBuilding.m_CurrentBuilding && travelPurpose.m_Purpose != Game.Citizens.Purpose.Hospital && travelPurpose.m_Purpose != Game.Citizens.Purpose.InHospital)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.GoToHospital(unfilteredChunkIndex, entity7, currentBuilding, travelPurpose, currentTransport, tripNeededs, entity8, true);
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (!this.m_OutsideConnectionData.HasComponent(entity8))
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.GoToHospital(unfilteredChunkIndex, entity7, currentBuilding, travelPurpose, currentTransport, tripNeededs, Entity.Null, false);
                    }
                  }
                }
              }
              else if ((healthProblem.m_Flags & HealthProblemFlags.Injured) != HealthProblemFlags.None)
              {
                Entity entity9 = nativeArray1[index];
                DynamicBuffer<TripNeeded> tripNeededs = bufferAccessor[index];
                Entity entity10 = currentBuilding.m_CurrentBuilding;
                // ISSUE: reference to a compiler-generated field
                if (entity10 == Entity.Null && travelPurpose.m_Purpose == Game.Citizens.Purpose.Hospital && this.m_TargetData.HasComponent(currentTransport.m_CurrentTransport))
                {
                  // ISSUE: reference to a compiler-generated field
                  entity10 = this.m_TargetData[currentTransport.m_CurrentTransport].m_Target;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_HospitalData.HasComponent(entity10) && (this.m_HospitalData[entity10].m_Flags & HospitalFlags.CanCureInjury) != (HospitalFlags) 0)
                {
                  if (entity10 == currentBuilding.m_CurrentBuilding && travelPurpose.m_Purpose != Game.Citizens.Purpose.Hospital && travelPurpose.m_Purpose != Game.Citizens.Purpose.InHospital)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.GoToHospital(unfilteredChunkIndex, entity9, currentBuilding, travelPurpose, currentTransport, tripNeededs, entity10, true);
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_OutsideConnectionData.HasComponent(entity10))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.GoToHospital(unfilteredChunkIndex, entity9, currentBuilding, travelPurpose, currentTransport, tripNeededs, Entity.Null, true);
                  }
                }
              }
            }
          }
        }
      }

      private void HandleRequest(int jobIndex, HealthProblem healthProblem)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_HealthcareRequestData.HasComponent(healthProblem.m_HealthcareRequest))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity, new HandleRequest(healthProblem.m_HealthcareRequest, Entity.Null, true));
      }

      private bool RequestVehicleIfNeeded(
        int jobIndex,
        Entity entity,
        CurrentBuilding currentBuilding,
        TravelPurpose travelPurpose,
        CurrentTransport currentTransport,
        DynamicBuffer<TripNeeded> tripNeededs,
        HealthProblem healthProblem)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_HealthcareRequestData.HasComponent(healthProblem.m_HealthcareRequest))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_DispatchedData.HasComponent(healthProblem.m_HealthcareRequest))
          {
            // ISSUE: reference to a compiler-generated field
            Dispatched dispatched = this.m_DispatchedData[healthProblem.m_HealthcareRequest];
            if ((healthProblem.m_Flags & HealthProblemFlags.Dead) != HealthProblemFlags.None)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_HearseData.HasComponent(dispatched.m_Handler))
              {
                // ISSUE: reference to a compiler-generated field
                Game.Vehicles.Hearse hearse = this.m_HearseData[dispatched.m_Handler];
                if (hearse.m_TargetCorpse == entity && (hearse.m_State & HearseFlags.AtTarget) != (HearseFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.GoToDeathcare(jobIndex, entity, currentBuilding, travelPurpose, currentTransport, tripNeededs, dispatched.m_Handler);
                  return false;
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_AmbulanceData.HasComponent(dispatched.m_Handler))
                {
                  // ISSUE: reference to a compiler-generated field
                  Game.Vehicles.Ambulance ambulance = this.m_AmbulanceData[dispatched.m_Handler];
                  if (ambulance.m_TargetPatient == entity && (ambulance.m_State & AmbulanceFlags.AtTarget) != (AmbulanceFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.GoToHospital(jobIndex, entity, currentBuilding, travelPurpose, currentTransport, tripNeededs, dispatched.m_Handler, true);
                    return false;
                  }
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_AmbulanceData.HasComponent(dispatched.m_Handler))
              {
                // ISSUE: reference to a compiler-generated field
                Game.Vehicles.Ambulance ambulance = this.m_AmbulanceData[dispatched.m_Handler];
                if (ambulance.m_TargetPatient == entity && (ambulance.m_State & AmbulanceFlags.AtTarget) != (AmbulanceFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.GoToHospital(jobIndex, entity, currentBuilding, travelPurpose, currentTransport, tripNeededs, dispatched.m_Handler, true);
                  return false;
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_TargetData.HasComponent(currentTransport.m_CurrentTransport) && !this.m_DeletedData.HasComponent(currentTransport.m_CurrentTransport) && (this.m_TargetData[currentTransport.m_CurrentTransport].m_Target != Entity.Null || this.m_DivertData.HasComponent(currentTransport.m_CurrentTransport)))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_ResetTripArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<ResetTrip>(jobIndex, entity1, new ResetTrip()
            {
              m_Creature = currentTransport.m_CurrentTransport,
              m_Target = Entity.Null
            });
          }
          return true;
        }
        HealthcareRequestType type = (healthProblem.m_Flags & HealthProblemFlags.Dead) == HealthProblemFlags.None ? HealthcareRequestType.Ambulance : HealthcareRequestType.Hearse;
        bool flag = true;
        if (type == HealthcareRequestType.Hearse)
        {
          PrefabRef componentData1;
          Game.Prefabs.BuildingData componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          flag = this.m_PrefabRefData.TryGetComponent(currentBuilding.m_CurrentBuilding, out componentData1) && this.m_PrefabBuildingData.TryGetComponent(componentData1.m_Prefab, out componentData2) && (componentData2.m_Flags & Game.Prefabs.BuildingFlags.HasInsideRoom) > (Game.Prefabs.BuildingFlags) 0;
        }
        if (flag)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HealthcareRequestArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<HealthcareRequest>(jobIndex, entity2, new HealthcareRequest(entity, type));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity2, new RequestGroup(16U));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_TargetData.HasComponent(currentTransport.m_CurrentTransport) && !this.m_DeletedData.HasComponent(currentTransport.m_CurrentTransport))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_TargetData[currentTransport.m_CurrentTransport].m_Target != Entity.Null || this.m_DivertData.HasComponent(currentTransport.m_CurrentTransport))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity3 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_ResetTripArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<ResetTrip>(jobIndex, entity3, new ResetTrip()
            {
              m_Creature = currentTransport.m_CurrentTransport,
              m_Target = Entity.Null
            });
          }
        }
        else if (!flag)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<CurrentBuilding>(jobIndex, entity);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<TravelPurpose>(jobIndex, entity, new TravelPurpose()
          {
            m_Purpose = Game.Citizens.Purpose.GoingHome
          });
        }
        return true;
      }

      private void GoToHospital(
        int jobIndex,
        Entity entity,
        CurrentBuilding currentBuilding,
        TravelPurpose travelPurpose,
        CurrentTransport currentTransport,
        DynamicBuffer<TripNeeded> tripNeededs,
        Entity ambulance,
        bool immediate)
      {
        if (currentBuilding.m_CurrentBuilding != Entity.Null)
        {
          if (immediate)
          {
            tripNeededs.Clear();
          }
          else
          {
            for (int index = 0; index < tripNeededs.Length; ++index)
            {
              if (tripNeededs[index].m_Purpose == Game.Citizens.Purpose.Hospital)
                return;
            }
          }
          tripNeededs.Add(new TripNeeded()
          {
            m_Purpose = Game.Citizens.Purpose.Hospital,
            m_TargetAgent = ambulance
          });
          if (!immediate)
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<ResourceBuyer>(jobIndex, entity);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<TravelPurpose>(jobIndex, entity);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<AttendingMeeting>(jobIndex, entity);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!immediate || !(ambulance != Entity.Null) || !this.m_TargetData.HasComponent(currentTransport.m_CurrentTransport) || this.m_DeletedData.HasComponent(currentTransport.m_CurrentTransport) || this.m_CurrentVehicleData.HasComponent(currentTransport.m_CurrentTransport) && this.m_CurrentVehicleData[currentTransport.m_CurrentTransport].m_Vehicle == ambulance || travelPurpose.m_Purpose == Game.Citizens.Purpose.Hospital && !(this.m_TargetData[currentTransport.m_CurrentTransport].m_Target != ambulance) && !this.m_DivertData.HasComponent(currentTransport.m_CurrentTransport))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_ResetTripArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<ResetTrip>(jobIndex, entity1, new ResetTrip()
          {
            m_Creature = currentTransport.m_CurrentTransport,
            m_Target = ambulance,
            m_TravelPurpose = Game.Citizens.Purpose.Hospital
          });
        }
      }

      private void GoToDeathcare(
        int jobIndex,
        Entity entity,
        CurrentBuilding currentBuilding,
        TravelPurpose travelPurpose,
        CurrentTransport currentTransport,
        DynamicBuffer<TripNeeded> tripNeededs,
        Entity hearse)
      {
        if (currentBuilding.m_CurrentBuilding != Entity.Null)
        {
          tripNeededs.Clear();
          tripNeededs.Add(new TripNeeded()
          {
            m_Purpose = Game.Citizens.Purpose.Deathcare,
            m_TargetAgent = hearse
          });
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<ResourceBuyer>(jobIndex, entity);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<TravelPurpose>(jobIndex, entity);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<AttendingMeeting>(jobIndex, entity);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!(hearse != Entity.Null) || !this.m_TargetData.HasComponent(currentTransport.m_CurrentTransport) || this.m_DeletedData.HasComponent(currentTransport.m_CurrentTransport) || this.m_CurrentVehicleData.HasComponent(currentTransport.m_CurrentTransport) && this.m_CurrentVehicleData[currentTransport.m_CurrentTransport].m_Vehicle == hearse || travelPurpose.m_Purpose == Game.Citizens.Purpose.Deathcare && !(this.m_TargetData[currentTransport.m_CurrentTransport].m_Target != hearse) && !this.m_DivertData.HasComponent(currentTransport.m_CurrentTransport))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_ResetTripArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<ResetTrip>(jobIndex, entity1, new ResetTrip()
          {
            m_Creature = currentTransport.m_CurrentTransport,
            m_Target = hearse,
            m_TravelPurpose = Game.Citizens.Purpose.Deathcare
          });
        }
      }

      private void GoToSafety(
        int jobIndex,
        Entity entity,
        CurrentBuilding currentBuilding,
        TravelPurpose travelPurpose,
        CurrentTransport currentTransport,
        DynamicBuffer<TripNeeded> tripNeededs)
      {
        if (!(currentBuilding.m_CurrentBuilding != Entity.Null))
          return;
        tripNeededs.Clear();
        tripNeededs.Add(new TripNeeded()
        {
          m_Purpose = Game.Citizens.Purpose.Safety
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<ResourceBuyer>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<TravelPurpose>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<AttendingMeeting>(jobIndex, entity);
      }

      private void AddJournalData(int chunkIndex, HealthProblem problem)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(chunkIndex, this.m_JournalDataArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<AddEventJournalData>(chunkIndex, entity, new AddEventJournalData(problem.m_Event, EventDataTrackingType.Casualties));
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
      public ComponentTypeHandle<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentTransport> __Game_Citizens_CurrentTransport_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Citizen> __Game_Citizens_Citizen_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TravelPurpose> __Game_Citizens_TravelPurpose_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      public ComponentTypeHandle<HealthProblem> __Game_Citizens_HealthProblem_RW_ComponentTypeHandle;
      public BufferTypeHandle<TripNeeded> __Game_Citizens_TripNeeded_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<HealthcareRequest> __Game_Simulation_HealthcareRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Hospital> __Game_Buildings_Hospital_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.DeathcareFacility> __Game_Buildings_DeathcareFacility_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OnFire> __Game_Events_OnFire_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Dispatched> __Game_Simulation_Dispatched_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Target> __Game_Common_Target_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Static> __Game_Objects_Static_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Ambulance> __Game_Vehicles_Ambulance_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Hearse> __Game_Vehicles_Hearse_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Divert> __Game_Creatures_Divert_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentTransport_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TravelPurpose_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TravelPurpose>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RW_ComponentTypeHandle = state.GetComponentTypeHandle<HealthProblem>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TripNeeded_RW_BufferTypeHandle = state.GetBufferTypeHandle<TripNeeded>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_HealthcareRequest_RO_ComponentLookup = state.GetComponentLookup<HealthcareRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Hospital_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Hospital>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_DeathcareFacility_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.DeathcareFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_OnFire_RO_ComponentLookup = state.GetComponentLookup<OnFire>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Dispatched_RO_ComponentLookup = state.GetComponentLookup<Dispatched>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentLookup = state.GetComponentLookup<Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Static_RO_ComponentLookup = state.GetComponentLookup<Static>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Ambulance_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.Ambulance>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Hearse_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.Hearse>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Divert_RO_ComponentLookup = state.GetComponentLookup<Divert>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
      }
    }
  }
}
