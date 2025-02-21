// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CriminalSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Events;
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

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CriminalSystem : GameSystemBase
  {
    public const uint SYSTEM_UPDATE_INTERVAL = 16;
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private CitySystem m_CitySystem;
    private EntityQuery m_CriminalQuery;
    private EntityQuery m_PoliceConfigQuery;
    private EntityArchetype m_AddAccidentSiteArchetype;
    private TriggerSystem m_TriggerSystem;
    private CriminalSystem.TypeHandle __TypeHandle;

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
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CriminalQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadWrite<Criminal>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_PoliceConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<PoliceConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AddAccidentSiteArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<AddAccidentSite>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CriminalQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PoliceConfigQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint num = this.m_SimulationSystem.frameIndex / 16U & 15U;
      NativeQueue<CriminalSystem.CrimeData> nativeQueue = new NativeQueue<CriminalSystem.CrimeData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Occupant_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CrimeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PoliceCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Prison_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PoliceStation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_AccidentSite_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Criminal_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CriminalSystem.CriminalJob jobData1 = new CriminalSystem.CriminalJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TravelPurposeType = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentTypeHandle,
        m_CurrentBuildingType = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle,
        m_HealthProblemType = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_CriminalType = this.__TypeHandle.__Game_Citizens_Criminal_RW_ComponentTypeHandle,
        m_TripNeededType = this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle,
        m_AccidentSiteData = this.__TypeHandle.__Game_Events_AccidentSite_RO_ComponentLookup,
        m_HouseholdMemberData = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_DispatchedData = this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentLookup,
        m_PoliceStationData = this.__TypeHandle.__Game_Buildings_PoliceStation_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PrisonData = this.__TypeHandle.__Game_Buildings_Prison_RO_ComponentLookup,
        m_PublicTransportData = this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup,
        m_PoliceCarData = this.__TypeHandle.__Game_Vehicles_PoliceCar_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabCrimeData = this.__TypeHandle.__Game_Prefabs_CrimeData_RO_ComponentLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_ServiceDispatches = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferLookup,
        m_Employees = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup,
        m_Renters = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_Occupants = this.__TypeHandle.__Game_Buildings_Occupant_RW_BufferLookup,
        m_TriggerBuffer = this.m_TriggerSystem.CreateActionBuffer().AsParallelWriter(),
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps).AsParallelWriter(),
        m_UpdateFrameIndex = num,
        m_RandomSeed = RandomSeed.Next(),
        m_PoliceConfigurationData = this.m_PoliceConfigQuery.GetSingleton<PoliceConfigurationData>(),
        m_AddAccidentSiteArchetype = this.m_AddAccidentSiteArchetype,
        m_City = this.m_CitySystem.City,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_CrimeQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CrimeVictim_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CriminalSystem.CrimeJob jobData2 = new CriminalSystem.CrimeJob()
      {
        m_CrimeVictimData = this.__TypeHandle.__Game_Citizens_CrimeVictim_RW_ComponentLookup,
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup,
        m_CrimeQueue = nativeQueue,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle writer = jobData1.Schedule<CriminalSystem.CriminalJob>(this.m_CriminalQuery, JobHandle.CombineDependencies(this.Dependency, deps));
      JobHandle dependsOn = writer;
      JobHandle jobHandle = jobData2.Schedule<CriminalSystem.CrimeJob>(dependsOn);
      nativeQueue.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(writer);
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
    public CriminalSystem()
    {
    }

    private struct CrimeData
    {
      public Entity m_Source;
      public Entity m_Target;
      public int m_StealAmount;
      public int m_EffectAmount;
    }

    [BurstCompile]
    private struct CriminalJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<TravelPurpose> m_TravelPurposeType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> m_CurrentBuildingType;
      [ReadOnly]
      public ComponentTypeHandle<HealthProblem> m_HealthProblemType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public ComponentTypeHandle<Criminal> m_CriminalType;
      public BufferTypeHandle<TripNeeded> m_TripNeededType;
      [ReadOnly]
      public ComponentLookup<AccidentSite> m_AccidentSiteData;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMemberData;
      [ReadOnly]
      public ComponentLookup<Dispatched> m_DispatchedData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.PoliceStation> m_PoliceStationData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Prison> m_PrisonData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PoliceCar> m_PoliceCarData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.CrimeData> m_PrefabCrimeData;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public BufferLookup<ServiceDispatch> m_ServiceDispatches;
      [ReadOnly]
      public BufferLookup<Employee> m_Employees;
      [ReadOnly]
      public BufferLookup<Renter> m_Renters;
      [ReadOnly]
      public BufferLookup<Resources> m_Resources;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      public BufferLookup<Occupant> m_Occupants;
      [ReadOnly]
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public PoliceConfigurationData m_PoliceConfigurationData;
      [ReadOnly]
      public EntityArchetype m_AddAccidentSiteArchetype;
      [ReadOnly]
      public Entity m_City;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<CriminalSystem.CrimeData>.ParallelWriter m_CrimeQueue;
      public NativeQueue<TriggerAction>.ParallelWriter m_TriggerBuffer;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;

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
        NativeArray<TravelPurpose> nativeArray2 = chunk.GetNativeArray<TravelPurpose>(ref this.m_TravelPurposeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentBuilding> nativeArray3 = chunk.GetNativeArray<CurrentBuilding>(ref this.m_CurrentBuildingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HealthProblem> nativeArray4 = chunk.GetNativeArray<HealthProblem>(ref this.m_HealthProblemType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Criminal> nativeArray5 = chunk.GetNativeArray<Criminal>(ref this.m_CriminalType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TripNeeded> bufferAccessor = chunk.GetBufferAccessor<TripNeeded>(ref this.m_TripNeededType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
        for (int index = 0; index < nativeArray5.Length; ++index)
        {
          Criminal criminal = nativeArray5[index];
          if (criminal.m_Flags == (CriminalFlags) 0)
          {
            Entity e = nativeArray1[index];
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Criminal>(unfilteredChunkIndex, e);
          }
          else if ((criminal.m_Flags & CriminalFlags.Prisoner) != (CriminalFlags) 0)
          {
            if (nativeArray3.Length != 0)
            {
              CurrentBuilding currentBuilding = nativeArray3[index];
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrisonData.HasComponent(currentBuilding.m_CurrentBuilding))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_BuildingData.HasComponent(currentBuilding.m_CurrentBuilding) && BuildingUtils.CheckOption(this.m_BuildingData[currentBuilding.m_CurrentBuilding], BuildingOption.Inactive))
                {
                  Entity entity = nativeArray1[index];
                  // ISSUE: reference to a compiler-generated method
                  this.RemoveTravelPurpose(unfilteredChunkIndex, entity, nativeArray2, index);
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_Occupants.HasBuffer(currentBuilding.m_CurrentBuilding))
                  {
                    // ISSUE: reference to a compiler-generated field
                    CollectionUtils.RemoveValue<Occupant>(this.m_Occupants[currentBuilding.m_CurrentBuilding], new Occupant(entity));
                    continue;
                  }
                }
                criminal.m_JailTime = (ushort) math.max(0, (int) criminal.m_JailTime - 1);
                if (criminal.m_JailTime == (ushort) 0)
                {
                  Entity entity = nativeArray1[index];
                  criminal.m_Flags = (CriminalFlags) 0;
                  criminal.m_Event = Entity.Null;
                  // ISSUE: reference to a compiler-generated method
                  this.RemoveTravelPurpose(unfilteredChunkIndex, entity, nativeArray2, index);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<Criminal>(unfilteredChunkIndex, entity);
                }
                nativeArray5[index] = criminal;
              }
              else
              {
                Entity entity = nativeArray1[index];
                criminal.m_Flags &= ~(CriminalFlags.Prisoner | CriminalFlags.Arrested | CriminalFlags.Sentenced);
                criminal.m_Event = Entity.Null;
                nativeArray5[index] = criminal;
                // ISSUE: reference to a compiler-generated method
                this.RemoveTravelPurpose(unfilteredChunkIndex, entity, nativeArray2, index);
              }
            }
          }
          else if ((criminal.m_Flags & CriminalFlags.Arrested) != (CriminalFlags) 0)
          {
            if (nativeArray3.Length != 0)
            {
              CurrentBuilding currentBuilding = nativeArray3[index];
              // ISSUE: reference to a compiler-generated field
              if (this.m_PoliceStationData.HasComponent(currentBuilding.m_CurrentBuilding))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_BuildingData.HasComponent(currentBuilding.m_CurrentBuilding) && BuildingUtils.CheckOption(this.m_BuildingData[currentBuilding.m_CurrentBuilding], BuildingOption.Inactive))
                {
                  Entity entity = nativeArray1[index];
                  // ISSUE: reference to a compiler-generated method
                  this.RemoveTravelPurpose(unfilteredChunkIndex, entity, nativeArray2, index);
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_Occupants.HasBuffer(currentBuilding.m_CurrentBuilding))
                  {
                    // ISSUE: reference to a compiler-generated field
                    CollectionUtils.RemoveValue<Occupant>(this.m_Occupants[currentBuilding.m_CurrentBuilding], new Occupant(entity));
                    continue;
                  }
                }
                if ((criminal.m_Flags & CriminalFlags.Sentenced) != (CriminalFlags) 0)
                {
                  Entity vehicle;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated method
                  if (this.GetTransportVehicle(this.m_PoliceStationData[currentBuilding.m_CurrentBuilding], out vehicle) && this.CheckHealth(nativeArray4, index))
                  {
                    Entity entity = nativeArray1[index];
                    DynamicBuffer<TripNeeded> tripNeededs = bufferAccessor[index];
                    criminal.m_Flags |= CriminalFlags.Prisoner;
                    criminal.m_Event = Entity.Null;
                    nativeArray5[index] = criminal;
                    // ISSUE: reference to a compiler-generated method
                    this.GoToPrison(unfilteredChunkIndex, entity, tripNeededs, vehicle);
                  }
                  else
                  {
                    criminal.m_JailTime = (ushort) math.max(0, (int) criminal.m_JailTime - 1);
                    if (criminal.m_JailTime == (ushort) 0)
                    {
                      Entity entity = nativeArray1[index];
                      criminal.m_Flags = (CriminalFlags) 0;
                      criminal.m_Event = Entity.Null;
                      // ISSUE: reference to a compiler-generated method
                      this.RemoveTravelPurpose(unfilteredChunkIndex, entity, nativeArray2, index);
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.RemoveComponent<Criminal>(unfilteredChunkIndex, entity);
                    }
                    nativeArray5[index] = criminal;
                  }
                }
                else
                {
                  criminal.m_JailTime = (ushort) math.max(0, (int) criminal.m_JailTime - 1);
                  if (criminal.m_JailTime == (ushort) 0)
                  {
                    Entity entity = nativeArray1[index];
                    // ISSUE: reference to a compiler-generated field
                    Random random = this.m_RandomSeed.GetRandom(entity.Index);
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_PrefabRefData.HasComponent(criminal.m_Event))
                    {
                      // ISSUE: reference to a compiler-generated field
                      PrefabRef prefabRef = this.m_PrefabRefData[criminal.m_Event];
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_PrefabCrimeData.HasComponent(prefabRef.m_Prefab))
                      {
                        // ISSUE: reference to a compiler-generated field
                        Game.Prefabs.CrimeData crimeData = this.m_PrefabCrimeData[prefabRef.m_Prefab];
                        if ((double) random.NextFloat(100f) < (double) crimeData.m_PrisonProbability)
                        {
                          float num = math.lerp(crimeData.m_PrisonTimeRange.min, crimeData.m_PrisonTimeRange.max, random.NextFloat(1f));
                          CityUtils.ApplyModifier(ref num, cityModifier, CityModifierType.PrisonTime);
                          criminal.m_Flags |= CriminalFlags.Sentenced;
                          criminal.m_JailTime = (ushort) math.min((float) ushort.MaxValue, (float) ((double) num * 262144.0 / 256.0));
                          criminal.m_Event = Entity.Null;
                          // ISSUE: reference to a compiler-generated field
                          this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.CitizenGotSentencedToPrison, Entity.Null, entity, Entity.Null));
                        }
                      }
                    }
                    if ((criminal.m_Flags & CriminalFlags.Sentenced) == (CriminalFlags) 0)
                    {
                      criminal.m_Flags &= ~CriminalFlags.Arrested;
                      criminal.m_Event = Entity.Null;
                      // ISSUE: reference to a compiler-generated method
                      this.RemoveTravelPurpose(unfilteredChunkIndex, entity, nativeArray2, index);
                    }
                  }
                  nativeArray5[index] = criminal;
                }
              }
              else
              {
                Entity entity = nativeArray1[index];
                criminal.m_Flags &= ~(CriminalFlags.Arrested | CriminalFlags.Sentenced);
                criminal.m_Event = Entity.Null;
                nativeArray5[index] = criminal;
                // ISSUE: reference to a compiler-generated method
                this.RemoveTravelPurpose(unfilteredChunkIndex, entity, nativeArray2, index);
              }
            }
          }
          else if ((criminal.m_Flags & CriminalFlags.Planning) != (CriminalFlags) 0)
          {
            Entity entity = nativeArray1[index];
            DynamicBuffer<TripNeeded> tripNeededs = bufferAccessor[index];
            // ISSUE: reference to a compiler-generated field
            Random random = this.m_RandomSeed.GetRandom(entity.Index);
            // ISSUE: reference to a compiler-generated method
            if (!this.IsPreparingCrime(tripNeededs))
              tripNeededs.Add(new TripNeeded()
              {
                m_Purpose = Game.Citizens.Purpose.Crime
              });
            float num = 0.0f;
            CityUtils.ApplyModifier(ref num, cityModifier, CityModifierType.CriminalMonitorProbability);
            if ((double) random.NextFloat(100f) < (double) num)
              criminal.m_Flags |= CriminalFlags.Monitored;
            criminal.m_Flags &= ~CriminalFlags.Planning;
            criminal.m_Flags |= CriminalFlags.Preparing;
            nativeArray5[index] = criminal;
          }
          else if ((criminal.m_Flags & CriminalFlags.Preparing) != (CriminalFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            if (!this.IsPreparingCrime(bufferAccessor[index]))
            {
              criminal.m_Flags &= ~CriminalFlags.Preparing;
              nativeArray5[index] = criminal;
            }
          }
          else if (criminal.m_Event != Entity.Null)
          {
            TravelPurpose travelPurpose = new TravelPurpose();
            CurrentBuilding currentBuilding = new CurrentBuilding();
            if (nativeArray2.Length != 0)
              travelPurpose = nativeArray2[index];
            if (nativeArray3.Length != 0)
              currentBuilding = nativeArray3[index];
            if (travelPurpose.m_Purpose != Game.Citizens.Purpose.Crime)
            {
              criminal.m_Event = Entity.Null;
              criminal.m_Flags &= ~CriminalFlags.Monitored;
              nativeArray5[index] = criminal;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_AccidentSiteData.HasComponent(currentBuilding.m_CurrentBuilding))
              {
                // ISSUE: reference to a compiler-generated field
                AccidentSite accidentSite = this.m_AccidentSiteData[currentBuilding.m_CurrentBuilding];
                if ((accidentSite.m_Flags & (AccidentSiteFlags.Secured | AccidentSiteFlags.CrimeScene | AccidentSiteFlags.CrimeFinished)) != AccidentSiteFlags.CrimeScene || accidentSite.m_Event != criminal.m_Event)
                {
                  Entity entity = nativeArray1[index];
                  DynamicBuffer<TripNeeded> tripNeededs = bufferAccessor[index];
                  // ISSUE: reference to a compiler-generated field
                  Random random = this.m_RandomSeed.GetRandom(entity.Index);
                  // ISSUE: reference to a compiler-generated method
                  if (this.CheckHealth(nativeArray4, index))
                  {
                    Entity vehicle;
                    // ISSUE: reference to a compiler-generated method
                    if ((accidentSite.m_Flags & AccidentSiteFlags.Secured) != (AccidentSiteFlags) 0 && this.GetPoliceCar(accidentSite, out vehicle))
                    {
                      float num = 0.0f;
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_PrefabRefData.HasComponent(criminal.m_Event))
                      {
                        // ISSUE: reference to a compiler-generated field
                        PrefabRef prefabRef = this.m_PrefabRefData[criminal.m_Event];
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_PrefabCrimeData.HasComponent(prefabRef.m_Prefab))
                        {
                          // ISSUE: reference to a compiler-generated field
                          Game.Prefabs.CrimeData crimeData = this.m_PrefabCrimeData[prefabRef.m_Prefab];
                          num = math.lerp(crimeData.m_JailTimeRange.min, crimeData.m_JailTimeRange.max, random.NextFloat(1f));
                        }
                      }
                      criminal.m_Flags &= ~CriminalFlags.Monitored;
                      criminal.m_Flags |= CriminalFlags.Arrested;
                      criminal.m_JailTime = (ushort) math.min((float) ushort.MaxValue, (float) ((double) num * 262144.0 / 256.0));
                      nativeArray5[index] = criminal;
                      // ISSUE: reference to a compiler-generated method
                      this.GoToJail(unfilteredChunkIndex, entity, tripNeededs, vehicle);
                      // ISSUE: reference to a compiler-generated field
                      this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.CitizenGotArrested, Entity.Null, entity, criminal.m_Event));
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated method
                      Entity crimeSource = this.GetCrimeSource(ref random, currentBuilding.m_CurrentBuilding);
                      // ISSUE: reference to a compiler-generated method
                      Entity crimeTarget = this.GetCrimeTarget(entity);
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_PrefabRefData.HasComponent(criminal.m_Event))
                      {
                        // ISSUE: reference to a compiler-generated field
                        PrefabRef prefabRef = this.m_PrefabRefData[criminal.m_Event];
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_PrefabCrimeData.HasComponent(prefabRef.m_Prefab))
                        {
                          // ISSUE: reference to a compiler-generated field
                          Game.Prefabs.CrimeData crimeData = this.m_PrefabCrimeData[prefabRef.m_Prefab];
                          if (crimeData.m_CrimeType == CrimeType.Robbery)
                          {
                            // ISSUE: reference to a compiler-generated method
                            int stealAmount = this.GetStealAmount(ref random, crimeSource, crimeData);
                            if (stealAmount > 0)
                            {
                              // ISSUE: reference to a compiler-generated field
                              // ISSUE: object of a compiler-generated type is created
                              this.m_CrimeQueue.Enqueue(new CriminalSystem.CrimeData()
                              {
                                m_Source = crimeSource,
                                m_Target = crimeTarget,
                                m_StealAmount = stealAmount
                              });
                            }
                          }
                        }
                      }
                      // ISSUE: reference to a compiler-generated method
                      this.AddCrimeEffects(crimeSource);
                      criminal.m_Event = Entity.Null;
                      criminal.m_Flags &= ~CriminalFlags.Monitored;
                      nativeArray5[index] = criminal;
                      // ISSUE: reference to a compiler-generated method
                      this.TryEscape(unfilteredChunkIndex, entity, tripNeededs);
                      // ISSUE: reference to a compiler-generated field
                      this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
                      {
                        m_Statistic = StatisticType.EscapedArrestCount,
                        m_Change = 1f
                      });
                    }
                  }
                }
              }
              else if (currentBuilding.m_CurrentBuilding != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated method
                this.AddCrimeScene(unfilteredChunkIndex, criminal.m_Event, currentBuilding.m_CurrentBuilding);
              }
            }
          }
        }
      }

      private void RemoveTravelPurpose(
        int jobIndex,
        Entity entity,
        NativeArray<TravelPurpose> travelPurposes,
        int index)
      {
        TravelPurpose travelPurpose;
        if (!CollectionUtils.TryGet<TravelPurpose>(travelPurposes, index, out travelPurpose) || travelPurpose.m_Purpose != Game.Citizens.Purpose.GoingToPrison && travelPurpose.m_Purpose != Game.Citizens.Purpose.InPrison && travelPurpose.m_Purpose != Game.Citizens.Purpose.GoingToJail && travelPurpose.m_Purpose != Game.Citizens.Purpose.InJail)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<TravelPurpose>(jobIndex, entity);
      }

      private bool CheckHealth(NativeArray<HealthProblem> healthProblems, int index)
      {
        HealthProblem healthProblem;
        return !CollectionUtils.TryGet<HealthProblem>(healthProblems, index, out healthProblem) || (healthProblem.m_Flags & HealthProblemFlags.RequireTransport) == HealthProblemFlags.None;
      }

      private void GoToPrison(
        int jobIndex,
        Entity entity,
        DynamicBuffer<TripNeeded> tripNeededs,
        Entity vehicle)
      {
        for (int index = 0; index < tripNeededs.Length; ++index)
        {
          if (tripNeededs[index].m_Purpose == Game.Citizens.Purpose.GoingToPrison)
            return;
        }
        tripNeededs.Add(new TripNeeded()
        {
          m_Purpose = Game.Citizens.Purpose.GoingToPrison,
          m_TargetAgent = vehicle
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<TravelPurpose>(jobIndex, entity);
      }

      private void GoToJail(
        int jobIndex,
        Entity entity,
        DynamicBuffer<TripNeeded> tripNeededs,
        Entity vehicle)
      {
        for (int index = 0; index < tripNeededs.Length; ++index)
        {
          if (tripNeededs[index].m_Purpose == Game.Citizens.Purpose.GoingToJail)
            return;
        }
        tripNeededs.Add(new TripNeeded()
        {
          m_Purpose = Game.Citizens.Purpose.GoingToJail,
          m_TargetAgent = vehicle
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<TravelPurpose>(jobIndex, entity);
      }

      private bool GetTransportVehicle(Game.Buildings.PoliceStation policeStation, out Entity vehicle)
      {
        vehicle = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_DispatchedData.HasComponent(policeStation.m_PrisonerTransportRequest))
          return false;
        // ISSUE: reference to a compiler-generated field
        Dispatched dispatched = this.m_DispatchedData[policeStation.m_PrisonerTransportRequest];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PublicTransportData.HasComponent(dispatched.m_Handler) || (this.m_PublicTransportData[dispatched.m_Handler].m_State & PublicTransportFlags.Boarding) == (PublicTransportFlags) 0 || !this.m_ServiceDispatches.HasBuffer(dispatched.m_Handler))
          return false;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ServiceDispatch> serviceDispatch = this.m_ServiceDispatches[dispatched.m_Handler];
        if (serviceDispatch.Length == 0 || serviceDispatch[0].m_Request != policeStation.m_PrisonerTransportRequest)
          return false;
        vehicle = dispatched.m_Handler;
        return true;
      }

      private bool GetPoliceCar(AccidentSite accidentSite, out Entity vehicle)
      {
        vehicle = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_DispatchedData.HasComponent(accidentSite.m_PoliceRequest))
          return false;
        // ISSUE: reference to a compiler-generated field
        Dispatched dispatched = this.m_DispatchedData[accidentSite.m_PoliceRequest];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PoliceCarData.HasComponent(dispatched.m_Handler) || (this.m_PoliceCarData[dispatched.m_Handler].m_State & PoliceCarFlags.AtTarget) == (PoliceCarFlags) 0 || !this.m_ServiceDispatches.HasBuffer(dispatched.m_Handler))
          return false;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ServiceDispatch> serviceDispatch = this.m_ServiceDispatches[dispatched.m_Handler];
        if (serviceDispatch.Length == 0 || serviceDispatch[0].m_Request != accidentSite.m_PoliceRequest)
          return false;
        vehicle = dispatched.m_Handler;
        return true;
      }

      private void AddCrimeEffects(Entity source)
      {
        // ISSUE: variable of a compiler-generated type
        CriminalSystem.CrimeData crimeData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_HouseholdCitizens.HasBuffer(source))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<HouseholdCitizen> householdCitizen = this.m_HouseholdCitizens[source];
          for (int index = 0; index < householdCitizen.Length; ++index)
          {
            Entity citizen = householdCitizen[index].m_Citizen;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(citizen))
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeQueue<CriminalSystem.CrimeData>.ParallelWriter local = ref this.m_CrimeQueue;
              // ISSUE: object of a compiler-generated type is created
              crimeData1 = new CriminalSystem.CrimeData();
              // ISSUE: reference to a compiler-generated field
              crimeData1.m_Source = citizen;
              // ISSUE: reference to a compiler-generated field
              crimeData1.m_Target = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              crimeData1.m_EffectAmount = this.m_PoliceConfigurationData.m_HomeCrimeEffect;
              // ISSUE: variable of a compiler-generated type
              CriminalSystem.CrimeData crimeData2 = crimeData1;
              local.Enqueue(crimeData2);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Employees.HasBuffer(source))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Employee> employee = this.m_Employees[source];
        for (int index = 0; index < employee.Length; ++index)
        {
          Entity worker = employee[index].m_Worker;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.HasComponent(worker))
          {
            // ISSUE: reference to a compiler-generated field
            ref NativeQueue<CriminalSystem.CrimeData>.ParallelWriter local = ref this.m_CrimeQueue;
            // ISSUE: object of a compiler-generated type is created
            crimeData1 = new CriminalSystem.CrimeData();
            // ISSUE: reference to a compiler-generated field
            crimeData1.m_Source = worker;
            // ISSUE: reference to a compiler-generated field
            crimeData1.m_Target = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            crimeData1.m_EffectAmount = this.m_PoliceConfigurationData.m_WorkplaceCrimeEffect;
            // ISSUE: variable of a compiler-generated type
            CriminalSystem.CrimeData crimeData3 = crimeData1;
            local.Enqueue(crimeData3);
          }
        }
      }

      private Entity GetCrimeSource(ref Random random, Entity building)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_Renters.HasBuffer(building))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Renter> renter = this.m_Renters[building];
          if (renter.Length > 0)
            return renter[random.NextInt(renter.Length)].m_Renter;
        }
        return building;
      }

      private Entity GetCrimeTarget(Entity criminal)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_HouseholdMemberData.HasComponent(criminal) ? this.m_HouseholdMemberData[criminal].m_Household : criminal;
      }

      private int GetStealAmount(ref Random random, Entity source, Game.Prefabs.CrimeData crimeData)
      {
        float stealAmount = 0.0f;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Resources.HasBuffer(source))
        {
          // ISSUE: reference to a compiler-generated field
          int resources = EconomyUtils.GetResources(Resource.Money, this.m_Resources[source]);
          if (resources > 0)
            stealAmount += math.lerp(crimeData.m_CrimeIncomeRelative.min, crimeData.m_CrimeIncomeRelative.max, random.NextFloat(1f)) * (float) resources;
          stealAmount += math.lerp(crimeData.m_CrimeIncomeAbsolute.min, crimeData.m_CrimeIncomeAbsolute.max, random.NextFloat(1f));
        }
        return (int) stealAmount;
      }

      private void TryEscape(int jobIndex, Entity entity, DynamicBuffer<TripNeeded> tripNeededs)
      {
        tripNeededs.Clear();
        tripNeededs.Add(new TripNeeded()
        {
          m_Purpose = Game.Citizens.Purpose.Escape
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<ResourceBuyer>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<TravelPurpose>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<AttendingMeeting>(jobIndex, entity);
      }

      private bool IsPreparingCrime(DynamicBuffer<TripNeeded> tripNeededs)
      {
        for (int index = 0; index < tripNeededs.Length; ++index)
        {
          if (tripNeededs[index].m_Purpose == Game.Citizens.Purpose.Crime)
            return true;
        }
        return false;
      }

      private void AddCrimeScene(int jobIndex, Entity _event, Entity building)
      {
        AddAccidentSite component = new AddAccidentSite()
        {
          m_Event = _event,
          m_Target = building,
          m_Flags = AccidentSiteFlags.CrimeScene
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
    }

    [BurstCompile]
    private struct CrimeJob : IJob
    {
      public ComponentLookup<CrimeVictim> m_CrimeVictimData;
      public BufferLookup<Resources> m_Resources;
      public NativeQueue<CriminalSystem.CrimeData> m_CrimeQueue;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_CrimeQueue.Count;
        if (count == 0)
          return;
        NativeParallelHashMap<Entity, CrimeVictim> nativeParallelHashMap = new NativeParallelHashMap<Entity, CrimeVictim>(count, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CriminalSystem.CrimeData crimeData = this.m_CrimeQueue.Dequeue();
          // ISSUE: reference to a compiler-generated field
          if (crimeData.m_StealAmount > 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Resources.HasBuffer(crimeData.m_Source) && this.m_Resources.HasBuffer(crimeData.m_Target))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Resources> resource1 = this.m_Resources[crimeData.m_Source];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Resources> resource2 = this.m_Resources[crimeData.m_Target];
              // ISSUE: reference to a compiler-generated field
              EconomyUtils.AddResources(Resource.Money, -crimeData.m_StealAmount, resource1);
              // ISSUE: reference to a compiler-generated field
              EconomyUtils.AddResources(Resource.Money, crimeData.m_StealAmount, resource2);
            }
            else
              continue;
          }
          // ISSUE: reference to a compiler-generated field
          if (crimeData.m_EffectAmount > 0)
          {
            CrimeVictim crimeVictim;
            // ISSUE: reference to a compiler-generated field
            if (nativeParallelHashMap.TryGetValue(crimeData.m_Source, out crimeVictim))
            {
              // ISSUE: reference to a compiler-generated field
              crimeVictim.m_Effect = (byte) math.min((int) crimeVictim.m_Effect + crimeData.m_EffectAmount, (int) byte.MaxValue);
              // ISSUE: reference to a compiler-generated field
              nativeParallelHashMap[crimeData.m_Source] = crimeVictim;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CrimeVictimData.HasComponent(crimeData.m_Source) && this.m_CrimeVictimData.IsComponentEnabled(crimeData.m_Source))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                crimeVictim = this.m_CrimeVictimData[crimeData.m_Source];
                // ISSUE: reference to a compiler-generated field
                crimeVictim.m_Effect = (byte) math.min((int) crimeVictim.m_Effect + crimeData.m_EffectAmount, (int) byte.MaxValue);
                // ISSUE: reference to a compiler-generated field
                nativeParallelHashMap.Add(crimeData.m_Source, crimeVictim);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                crimeVictim.m_Effect = (byte) math.min(crimeData.m_EffectAmount, (int) byte.MaxValue);
                // ISSUE: reference to a compiler-generated field
                nativeParallelHashMap.Add(crimeData.m_Source, crimeVictim);
              }
            }
          }
        }
        if (nativeParallelHashMap.Count() <= 0)
          return;
        NativeArray<Entity> keyArray = nativeParallelHashMap.GetKeyArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < keyArray.Length; ++index)
        {
          Entity entity = keyArray[index];
          CrimeVictim crimeVictim = nativeParallelHashMap[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_CrimeVictimData.HasComponent(entity) && !this.m_CrimeVictimData.IsComponentEnabled(entity))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CrimeVictimData.SetComponentEnabled(entity, true);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CrimeVictimData[entity] = crimeVictim;
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TravelPurpose> __Game_Citizens_TravelPurpose_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      public ComponentTypeHandle<Criminal> __Game_Citizens_Criminal_RW_ComponentTypeHandle;
      public BufferTypeHandle<TripNeeded> __Game_Citizens_TripNeeded_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<AccidentSite> __Game_Events_AccidentSite_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Dispatched> __Game_Simulation_Dispatched_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.PoliceStation> __Game_Buildings_PoliceStation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Prison> __Game_Buildings_Prison_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> __Game_Vehicles_PublicTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PoliceCar> __Game_Vehicles_PoliceCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.CrimeData> __Game_Prefabs_CrimeData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ServiceDispatch> __Game_Simulation_ServiceDispatch_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Employee> __Game_Companies_Employee_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Resources> __Game_Economy_Resources_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;
      public BufferLookup<Occupant> __Game_Buildings_Occupant_RW_BufferLookup;
      public ComponentLookup<CrimeVictim> __Game_Citizens_CrimeVictim_RW_ComponentLookup;
      public BufferLookup<Resources> __Game_Economy_Resources_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TravelPurpose_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TravelPurpose>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Criminal_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Criminal>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TripNeeded_RW_BufferTypeHandle = state.GetBufferTypeHandle<TripNeeded>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_AccidentSite_RO_ComponentLookup = state.GetComponentLookup<AccidentSite>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Dispatched_RO_ComponentLookup = state.GetComponentLookup<Dispatched>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PoliceStation_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.PoliceStation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Prison_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Prison>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PublicTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PoliceCar_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PoliceCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CrimeData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.CrimeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RO_BufferLookup = state.GetBufferLookup<ServiceDispatch>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferLookup = state.GetBufferLookup<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferLookup = state.GetBufferLookup<Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Occupant_RW_BufferLookup = state.GetBufferLookup<Occupant>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CrimeVictim_RW_ComponentLookup = state.GetComponentLookup<CrimeVictim>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferLookup = state.GetBufferLookup<Resources>();
      }
    }
  }
}
