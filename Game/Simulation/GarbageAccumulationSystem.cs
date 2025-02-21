// Decompiled with JetBrains decompiler
// Type: Game.Simulation.GarbageAccumulationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Serialization;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class GarbageAccumulationSystem : 
    GameSystemBase,
    IDefaultSerializable,
    ISerializable,
    IPreDeserialize
  {
    public static readonly int kUpdatesPerDay = 16;
    private SimulationSystem m_SimulationSystem;
    private IconCommandSystem m_IconCommandSystem;
    private CitySystem m_CitySystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_GarbageProducerQuery;
    private EntityArchetype m_CollectionRequestArchetype;
    private NativeArray<long> m_GarbageAccumulation;
    private JobHandle m_AccumulationDeps;
    private long m_Accumulation;
    private GarbageAccumulationSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_2138252455_0;
    private EntityQuery __query_2138252455_1;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (GarbageAccumulationSystem.kUpdatesPerDay * 16);
    }

    public long garbageAccumulation
    {
      get => this.m_Accumulation * (long) GarbageAccumulationSystem.kUpdatesPerDay;
    }

    public static void GetGarbage(
      ref ConsumptionData consumption,
      Entity building,
      Entity prefab,
      BufferLookup<Renter> renters,
      BufferLookup<Game.Buildings.Student> students,
      BufferLookup<Occupant> occupants,
      ComponentLookup<HomelessHousehold> homelessHouseholds,
      BufferLookup<HouseholdCitizen> householdCitizens,
      ComponentLookup<Citizen> citizens,
      BufferLookup<Employee> employees,
      BufferLookup<Patient> patients,
      ComponentLookup<SpawnableBuildingData> spawnableDatas,
      ComponentLookup<CurrentDistrict> currentDistricts,
      BufferLookup<DistrictModifier> districtModifiers,
      ComponentLookup<ZoneData> zoneDatas,
      DynamicBuffer<CityModifier> cityModifiers,
      ref GarbageParameterData garbageParameter)
    {
      CurrentDistrict currentDistrict = currentDistricts[building];
      // ISSUE: reference to a compiler-generated method
      GarbageAccumulationSystem.GetGarbageAccumulation(building, prefab, ref consumption, currentDistrict, cityModifiers, citizens, spawnableDatas, zoneDatas, homelessHouseholds, householdCitizens, renters, employees, students, occupants, patients, districtModifiers, ref garbageParameter);
    }

    public static void GetGarbageAccumulation(
      Entity building,
      Entity prefab,
      ref ConsumptionData consumption,
      CurrentDistrict currentDistrict,
      DynamicBuffer<CityModifier> cityModifiers,
      ComponentLookup<Citizen> citizens,
      ComponentLookup<SpawnableBuildingData> spawnableDatas,
      ComponentLookup<ZoneData> zoneDatas,
      ComponentLookup<HomelessHousehold> homelessHousehold,
      BufferLookup<HouseholdCitizen> householdCitizens,
      BufferLookup<Renter> renters,
      BufferLookup<Employee> employees,
      BufferLookup<Game.Buildings.Student> students,
      BufferLookup<Occupant> occupants,
      BufferLookup<Patient> patients,
      BufferLookup<DistrictModifier> districtModifiers,
      ref GarbageParameterData garbageParameter)
    {
      float num1 = 0.0f;
      int num2 = 0;
      float num3 = 0.0f;
      if (renters.HasBuffer(building))
      {
        DynamicBuffer<Renter> renter1 = renters[building];
        for (int index1 = 0; index1 < renter1.Length; ++index1)
        {
          Entity renter2 = renter1[index1].m_Renter;
          Citizen citizen1;
          if (householdCitizens.HasBuffer(renter2))
          {
            DynamicBuffer<HouseholdCitizen> householdCitizen = householdCitizens[renter2];
            if (homelessHousehold.HasComponent(renter2))
            {
              num2 += householdCitizen.Length;
            }
            else
            {
              for (int index2 = 0; index2 < householdCitizen.Length; ++index2)
              {
                Entity citizen2 = householdCitizen[index2].m_Citizen;
                if (citizens.HasComponent(citizen2))
                {
                  double num4 = (double) num3;
                  citizen1 = citizens[citizen2];
                  double educationLevel = (double) citizen1.GetEducationLevel();
                  num3 = (float) (num4 + educationLevel);
                  ++num1;
                }
              }
            }
          }
          else if (employees.HasBuffer(renter2))
          {
            DynamicBuffer<Employee> employee = employees[renter2];
            for (int index3 = 0; index3 < employee.Length; ++index3)
            {
              Entity worker = employee[index3].m_Worker;
              if (citizens.HasComponent(worker))
              {
                double num5 = (double) num3;
                citizen1 = citizens[worker];
                double educationLevel = (double) citizen1.GetEducationLevel();
                num3 = (float) (num5 + educationLevel);
                ++num1;
              }
            }
          }
        }
      }
      else
      {
        Citizen citizen;
        if (employees.HasBuffer(building))
        {
          DynamicBuffer<Employee> employee = employees[building];
          for (int index = 0; index < employee.Length; ++index)
          {
            Entity worker = employee[index].m_Worker;
            if (citizens.HasComponent(worker))
            {
              double num6 = (double) num3;
              citizen = citizens[worker];
              double educationLevel = (double) citizen.GetEducationLevel();
              num3 = (float) (num6 + educationLevel);
              ++num1;
            }
          }
        }
        if (students.HasBuffer(building))
        {
          DynamicBuffer<Game.Buildings.Student> student = students[building];
          for (int index = 0; index < student.Length; ++index)
          {
            Entity entity = (Entity) student[index];
            if (citizens.HasComponent(entity))
            {
              double num7 = (double) num3;
              citizen = citizens[entity];
              double educationLevel = (double) citizen.GetEducationLevel();
              num3 = (float) (num7 + educationLevel);
              ++num1;
            }
          }
        }
        if (occupants.HasBuffer(building))
        {
          DynamicBuffer<Occupant> occupant = occupants[building];
          for (int index = 0; index < occupant.Length; ++index)
          {
            Entity entity = (Entity) occupant[index];
            if (citizens.HasComponent(entity))
            {
              double num8 = (double) num3;
              citizen = citizens[entity];
              double educationLevel = (double) citizen.GetEducationLevel();
              num3 = (float) (num8 + educationLevel);
              ++num1;
            }
          }
        }
        if (patients.HasBuffer(building))
        {
          DynamicBuffer<Patient> patient1 = patients[building];
          for (int index = 0; index < patient1.Length; ++index)
          {
            Entity patient2 = patient1[index].m_Patient;
            if (citizens.HasComponent(patient2))
            {
              double num9 = (double) num3;
              citizen = citizens[patient2];
              double educationLevel = (double) citizen.GetEducationLevel();
              num3 = (float) (num9 + educationLevel);
              ++num1;
            }
          }
        }
      }
      float num10 = 1f;
      if (spawnableDatas.HasComponent(prefab))
        num10 = (float) spawnableDatas[prefab].m_Level;
      float num11 = (double) num1 <= 0.0 ? consumption.m_GarbageAccumulation / num10 * garbageParameter.m_BuildingLevelBalance : math.max(0.0f, consumption.m_GarbageAccumulation - (float) ((double) num10 * (double) garbageParameter.m_BuildingLevelBalance + (double) num3 / (double) num1 * (double) garbageParameter.m_EducationBalance)) * num1;
      if (num2 > 0)
        num11 += (float) (garbageParameter.m_HomelessGarbageProduce * num2);
      if (districtModifiers.HasBuffer(currentDistrict.m_District))
      {
        DynamicBuffer<DistrictModifier> districtModifier = districtModifiers[currentDistrict.m_District];
        AreaUtils.ApplyModifier(ref num11, districtModifier, DistrictModifierType.GarbageProduction);
      }
      if (spawnableDatas.HasComponent(prefab))
      {
        SpawnableBuildingData spawnableData = spawnableDatas[prefab];
        if (zoneDatas.HasComponent(spawnableData.m_ZonePrefab) && zoneDatas[spawnableData.m_ZonePrefab].m_AreaType == Game.Zones.AreaType.Industrial && (zoneDatas[spawnableData.m_ZonePrefab].m_ZoneFlags & ZoneFlags.Office) == (ZoneFlags) 0)
          CityUtils.ApplyModifier(ref num11, cityModifiers, CityModifierType.IndustrialGarbage);
      }
      consumption.m_GarbageAccumulation = num11;
    }

    public static float GetGarbageEfficiencyFactor(
      int garbage,
      GarbageParameterData garbageParameters,
      float maxPenalty)
    {
      float num = math.saturate((float) (garbage - garbageParameters.m_WarningGarbageLimit) / (float) (garbageParameters.m_MaxGarbageAccumulation - garbageParameters.m_WarningGarbageLimit));
      return (float) (1.0 - (double) maxPenalty * (double) num);
    }

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
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageAccumulation = new NativeArray<long>(16, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageProducerQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<GarbageProducer>(),
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadOnly<UpdateFrame>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_CollectionRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<GarbageCollectionRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_GarbageProducerQuery);
      this.RequireForUpdate<GarbageParameterData>();
      this.RequireForUpdate<BuildingEfficiencyParameterData>();
      // ISSUE: reference to a compiler-generated field
      Assert.IsTrue(262144 / GarbageAccumulationSystem.kUpdatesPerDay >= 512);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageAccumulation.Dispose();
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Accumulation = 0L;
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_GarbageAccumulation.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_GarbageAccumulation[index] = 0L;
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, GarbageAccumulationSystem.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      this.m_AccumulationDeps.Complete();
      long num = 0;
      for (int index = 0; index < 16; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        num += this.m_GarbageAccumulation[index];
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Accumulation = num;
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageAccumulation[(int) updateFrame] = 0L;
      // ISSUE: reference to a compiler-generated field
      GarbageParameterData singleton = this.__query_2138252455_0.GetSingleton<GarbageParameterData>();
      if (this.EntityManager.HasEnabledComponent<Locked>(singleton.m_GarbageServicePrefab))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageProducerQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageProducerQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(updateFrame));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Patient_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Occupant_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Student_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HomelessHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_GarbageCollectionRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageProducer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new GarbageAccumulationSystem.GarbageAccumulationJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CurrentDistrictType = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle,
        m_GarbageProducerType = this.__TypeHandle.__Game_Buildings_GarbageProducer_RW_ComponentTypeHandle,
        m_GarbageCollectionRequestData = this.__TypeHandle.__Game_Simulation_GarbageCollectionRequest_RO_ComponentLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_QuantityData = this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ConsumptionDatas = this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup,
        m_SpawnableDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_ZoneDatas = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup,
        m_HomelessHousehold = this.__TypeHandle.__Game_Citizens_HomelessHousehold_RO_ComponentLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_Employees = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup,
        m_Renters = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_Students = this.__TypeHandle.__Game_Buildings_Student_RO_BufferLookup,
        m_Occupants = this.__TypeHandle.__Game_Buildings_Occupant_RO_BufferLookup,
        m_Patients = this.__TypeHandle.__Game_Buildings_Patient_RO_BufferLookup,
        m_DistrictModifiers = this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_City = this.m_CitySystem.City,
        m_UpdateFrame = ((int) updateFrame),
        m_RandomSeed = RandomSeed.Next(),
        m_CollectionRequestArchetype = this.m_CollectionRequestArchetype,
        m_GarbageParameters = singleton,
        m_GarbageEfficiencyPenalty = this.__query_2138252455_1.GetSingleton<BuildingEfficiencyParameterData>().m_GarbagePenalty,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_GarbageAccumulation = this.m_GarbageAccumulation
      }.ScheduleParallel<GarbageAccumulationSystem.GarbageAccumulationJob>(this.m_GarbageProducerQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(jobHandle);
      this.Dependency = jobHandle;
      // ISSUE: reference to a compiler-generated field
      this.m_AccumulationDeps = jobHandle;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write((byte) this.m_GarbageAccumulation.Length);
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_GarbageAccumulation.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_GarbageAccumulation[index]);
      }
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Accumulation = 0L;
      byte num1;
      reader.Read(out num1);
      for (int index = 0; index < (int) num1; ++index)
      {
        long num2;
        reader.Read(out num2);
        // ISSUE: reference to a compiler-generated field
        if (index < this.m_GarbageAccumulation.Length)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_GarbageAccumulation[index] = num2;
          // ISSUE: reference to a compiler-generated field
          this.m_Accumulation += num2;
        }
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = (int) num1; index < this.m_GarbageAccumulation.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_GarbageAccumulation[index] = 0L;
      }
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Accumulation = 0L;
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_GarbageAccumulation.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_GarbageAccumulation[index] = 0L;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_2138252455_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<GarbageParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_2138252455_1 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<BuildingEfficiencyParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
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
    public GarbageAccumulationSystem()
    {
    }

    [BurstCompile]
    private struct GarbageAccumulationJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> m_CurrentDistrictType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      public ComponentTypeHandle<GarbageProducer> m_GarbageProducerType;
      [ReadOnly]
      public ComponentLookup<GarbageCollectionRequest> m_GarbageCollectionRequestData;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public ComponentLookup<Quantity> m_QuantityData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<ConsumptionData> m_ConsumptionDatas;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableDatas;
      [ReadOnly]
      public ComponentLookup<ZoneData> m_ZoneDatas;
      [ReadOnly]
      public ComponentLookup<HomelessHousehold> m_HomelessHousehold;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public BufferLookup<Employee> m_Employees;
      [ReadOnly]
      public BufferLookup<Renter> m_Renters;
      [ReadOnly]
      public BufferLookup<Game.Buildings.Student> m_Students;
      [ReadOnly]
      public BufferLookup<Occupant> m_Occupants;
      [ReadOnly]
      public BufferLookup<Patient> m_Patients;
      [ReadOnly]
      public BufferLookup<DistrictModifier> m_DistrictModifiers;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public int m_UpdateFrame;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityArchetype m_CollectionRequestArchetype;
      [ReadOnly]
      public GarbageParameterData m_GarbageParameters;
      [ReadOnly]
      public float m_GarbageEfficiencyPenalty;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public IconCommandBuffer m_IconCommandBuffer;
      public NativeArray<long> m_GarbageAccumulation;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<GarbageProducer> nativeArray2 = chunk.GetNativeArray<GarbageProducer>(ref this.m_GarbageProducerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentDistrict> nativeArray3 = chunk.GetNativeArray<CurrentDistrict>(ref this.m_CurrentDistrictType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor1 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor2 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
        long accumulation = 0;
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity = nativeArray1[index];
          ref GarbageProducer local = ref nativeArray2.ElementAt<GarbageProducer>(index);
          CurrentDistrict currentDistrict = nativeArray3[index];
          // ISSUE: reference to a compiler-generated field
          Entity prefab = this.m_Prefabs[entity].m_Prefab;
          ConsumptionData componentData;
          // ISSUE: reference to a compiler-generated field
          this.m_ConsumptionDatas.TryGetComponent(prefab, out componentData);
          if (bufferAccessor1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UpgradeUtils.CombineStats<ConsumptionData>(ref componentData, bufferAccessor1[index], ref this.m_Prefabs, ref this.m_ConsumptionDatas);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
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
          GarbageAccumulationSystem.GetGarbageAccumulation(entity, prefab, ref componentData, currentDistrict, cityModifier, this.m_Citizens, this.m_SpawnableDatas, this.m_ZoneDatas, this.m_HomelessHousehold, this.m_HouseholdCitizens, this.m_Renters, this.m_Employees, this.m_Students, this.m_Occupants, this.m_Patients, this.m_DistrictModifiers, ref this.m_GarbageParameters);
          GarbageCollectionRequestFlags flags = (GarbageCollectionRequestFlags) 0;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SpawnableDatas.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            SpawnableBuildingData spawnableData = this.m_SpawnableDatas[prefab];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ZoneDatas.HasComponent(spawnableData.m_ZonePrefab) && this.m_ZoneDatas[spawnableData.m_ZonePrefab].m_AreaType == Game.Zones.AreaType.Industrial)
              flags |= GarbageCollectionRequestFlags.IndustrialWaste;
          }
          int garbage = local.m_Garbage;
          // ISSUE: reference to a compiler-generated field
          int intRandom = MathUtils.RoundToIntRandom(ref random, componentData.m_GarbageAccumulation / (float) GarbageAccumulationSystem.kUpdatesPerDay);
          local.m_Garbage += intRandom;
          // ISSUE: reference to a compiler-generated field
          local.m_Garbage = math.min(local.m_Garbage, this.m_GarbageParameters.m_MaxGarbageAccumulation);
          accumulation += (long) intRandom;
          // ISSUE: reference to a compiler-generated method
          this.RequestCollectionIfNeeded(unfilteredChunkIndex, entity, ref local, flags);
          // ISSUE: reference to a compiler-generated method
          this.AddWarningIfNeeded(entity, ref local, garbage);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (garbage >= this.m_GarbageParameters.m_RequestGarbageLimit != local.m_Garbage >= this.m_GarbageParameters.m_RequestGarbageLimit || garbage >= this.m_GarbageParameters.m_WarningGarbageLimit != local.m_Garbage >= this.m_GarbageParameters.m_WarningGarbageLimit)
          {
            // ISSUE: reference to a compiler-generated method
            this.QuantityUpdated(unfilteredChunkIndex, entity);
          }
          if (bufferAccessor2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            float efficiencyFactor = GarbageAccumulationSystem.GetGarbageEfficiencyFactor(local.m_Garbage, this.m_GarbageParameters, this.m_GarbageEfficiencyPenalty);
            BuildingUtils.SetEfficiencyFactor(bufferAccessor2[index], EfficiencyFactor.Garbage, efficiencyFactor);
          }
        }
        // ISSUE: reference to a compiler-generated method
        this.AddGarbageAccumulation(accumulation);
      }

      private void QuantityUpdated(int jobIndex, Entity buildingEntity, bool updateAll = false)
      {
        DynamicBuffer<Game.Objects.SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(buildingEntity, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity subObject = bufferData[index].m_SubObject;
          bool updateAll1 = false;
          // ISSUE: reference to a compiler-generated field
          if (updateAll || this.m_QuantityData.HasComponent(subObject))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, subObject, new BatchesUpdated());
            updateAll1 = true;
          }
          // ISSUE: reference to a compiler-generated method
          this.QuantityUpdated(jobIndex, subObject, updateAll1);
        }
      }

      private unsafe void AddGarbageAccumulation(long accumulation)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: cast to a reference type
        Interlocked.Add((long&) ((IntPtr) this.m_GarbageAccumulation.GetUnsafePtr<long>() + (IntPtr) this.m_UpdateFrame * 8), accumulation);
      }

      private void RequestCollectionIfNeeded(
        int jobIndex,
        Entity entity,
        ref GarbageProducer garbage,
        GarbageCollectionRequestFlags flags)
      {
        GarbageCollectionRequest componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (garbage.m_Garbage <= this.m_GarbageParameters.m_RequestGarbageLimit || this.m_GarbageCollectionRequestData.TryGetComponent(garbage.m_CollectionRequest, out componentData) && (componentData.m_Target == entity || (int) componentData.m_DispatchIndex == (int) garbage.m_DispatchIndex))
          return;
        garbage.m_CollectionRequest = Entity.Null;
        garbage.m_DispatchIndex = (byte) 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_CollectionRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<GarbageCollectionRequest>(jobIndex, entity1, new GarbageCollectionRequest(entity, garbage.m_Garbage, flags));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(32U));
      }

      private void AddWarningIfNeeded(Entity entity, ref GarbageProducer garbage, int oldGarbage)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (garbage.m_Garbage <= this.m_GarbageParameters.m_WarningGarbageLimit || oldGarbage > this.m_GarbageParameters.m_WarningGarbageLimit)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IconCommandBuffer.Add(entity, this.m_GarbageParameters.m_GarbageNotificationPrefab, IconPriority.Problem);
        garbage.m_Flags |= GarbageProducerFlags.GarbagePilingUpWarning;
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
      public ComponentTypeHandle<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RW_BufferTypeHandle;
      public ComponentTypeHandle<GarbageProducer> __Game_Buildings_GarbageProducer_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<GarbageCollectionRequest> __Game_Simulation_GarbageCollectionRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Quantity> __Game_Objects_Quantity_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ConsumptionData> __Game_Prefabs_ConsumptionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HomelessHousehold> __Game_Citizens_HomelessHousehold_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Employee> __Game_Companies_Employee_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Buildings.Student> __Game_Buildings_Student_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Occupant> __Game_Buildings_Occupant_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Patient> __Game_Buildings_Patient_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<DistrictModifier> __Game_Areas_DistrictModifier_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RW_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageProducer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<GarbageProducer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_GarbageCollectionRequest_RO_ComponentLookup = state.GetComponentLookup<GarbageCollectionRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Quantity_RO_ComponentLookup = state.GetComponentLookup<Quantity>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ConsumptionData_RO_ComponentLookup = state.GetComponentLookup<ConsumptionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentLookup = state.GetComponentLookup<ZoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HomelessHousehold_RO_ComponentLookup = state.GetComponentLookup<HomelessHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferLookup = state.GetBufferLookup<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Student_RO_BufferLookup = state.GetBufferLookup<Game.Buildings.Student>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Occupant_RO_BufferLookup = state.GetBufferLookup<Occupant>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Patient_RO_BufferLookup = state.GetBufferLookup<Patient>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_DistrictModifier_RO_BufferLookup = state.GetBufferLookup<DistrictModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
      }
    }
  }
}
