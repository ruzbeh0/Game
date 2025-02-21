// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.HealthcareInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.UI.Binding;
using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System;
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
  public class HealthcareInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "healthcareInfo";
    private CityStatisticsSystem m_CityStatisticsSystem;
    private ValueBinding<float> m_AverageHealth;
    private ValueBinding<int> m_PatientCount;
    private ValueBinding<int> m_SickCount;
    private ValueBinding<int> m_PatientCapacity;
    private ValueBinding<float> m_DeathRate;
    private ValueBinding<float> m_ProcessingRate;
    private ValueBinding<int> m_CemeteryUse;
    private ValueBinding<int> m_CemeteryCapacity;
    private GetterValueBinding<IndicatorValue> m_HealthcareAvailability;
    private GetterValueBinding<IndicatorValue> m_DeathcareAvailability;
    private GetterValueBinding<IndicatorValue> m_CemeteryAvailability;
    private EntityQuery m_HouseholdQuery;
    private EntityQuery m_DeathcareFacilityQuery;
    private EntityQuery m_HealthcareFacilityQuery;
    private EntityQuery m_DeathcareFacilityModifiedQuery;
    private EntityQuery m_HealthcareFacilityModifiedQuery;
    private NativeArray<float> m_Results;
    private HealthcareInfoviewUISystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdQuery = this.GetEntityQuery(ComponentType.ReadOnly<Household>(), ComponentType.ReadOnly<HouseholdCitizen>(), ComponentType.ReadOnly<PropertyRenter>(), ComponentType.Exclude<CommuterHousehold>(), ComponentType.Exclude<TouristHousehold>(), ComponentType.Exclude<MovingAway>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeathcareFacilityQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.DeathcareFacility>(), ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<ServiceDispatch>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Patient>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeathcareFacilityModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[5]
        {
          ComponentType.ReadOnly<Game.Buildings.DeathcareFacility>(),
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<ServiceDispatch>(),
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadOnly<Patient>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareFacilityQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.Hospital>(), ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<ServiceDispatch>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Patient>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareFacilityModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[5]
        {
          ComponentType.ReadOnly<Game.Buildings.Hospital>(),
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<ServiceDispatch>(),
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadOnly<Patient>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AverageHealth = new ValueBinding<float>("healthcareInfo", "averageHealth", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_DeathRate = new ValueBinding<float>("healthcareInfo", "deathRate", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ProcessingRate = new ValueBinding<float>("healthcareInfo", "processingRate", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CemeteryUse = new ValueBinding<int>("healthcareInfo", "cemeteryUse", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CemeteryCapacity = new ValueBinding<int>("healthcareInfo", "cemeteryCapacity", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SickCount = new ValueBinding<int>("healthcareInfo", "sickCount", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_PatientCount = new ValueBinding<int>("healthcareInfo", "patientCount", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_PatientCapacity = new ValueBinding<int>("healthcareInfo", "patientCapacity", 0)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_HealthcareAvailability = new GetterValueBinding<IndicatorValue>("healthcareInfo", "healthcareAvailability", (Func<IndicatorValue>) (() => IndicatorValue.Calculate((float) this.m_PatientCapacity.value, (float) this.m_SickCount.value)), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_DeathcareAvailability = new GetterValueBinding<IndicatorValue>("healthcareInfo", "deathcareAvailability", (Func<IndicatorValue>) (() => IndicatorValue.Calculate(this.m_ProcessingRate.value, this.m_DeathRate.value)), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CemeteryAvailability = new GetterValueBinding<IndicatorValue>("healthcareInfo", "cemeteryAvailability", (Func<IndicatorValue>) (() => IndicatorValue.Calculate((float) this.m_CemeteryCapacity.value, (float) this.m_CemeteryUse.value)), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<float>(8, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Results.Dispose();
      base.OnDestroy();
    }

    protected override bool Active
    {
      get
      {
        return base.Active || this.m_AverageHealth.active || this.m_PatientCount.active || this.m_SickCount.active || this.m_PatientCapacity.active || this.m_HealthcareAvailability.active || this.m_DeathRate.active || this.m_ProcessingRate.active || this.m_CemeteryUse.active || this.m_CemeteryCapacity.active || this.m_DeathcareAvailability.active || this.m_CemeteryAvailability.active;
      }
    }

    protected override bool Modified
    {
      get
      {
        return !this.m_DeathcareFacilityModifiedQuery.IsEmptyIgnoreFilter || !this.m_HealthcareFacilityModifiedQuery.IsEmptyIgnoreFilter;
      }
    }

    protected override void PerformUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Results.Fill<float>(0.0f);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      new HealthcareInfoviewUISystem.CalculateAverageHealthJob()
      {
        m_HouseholdType = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentTypeHandle,
        m_HouseholdCitizenType = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_HealthProblems = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
        m_Results = this.m_Results
      }.Schedule<HealthcareInfoviewUISystem.CalculateAverageHealthJob>(this.m_HouseholdQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HospitalData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Patient_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      new HealthcareInfoviewUISystem.UpdateHealthcareJob()
      {
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PatientType = this.__TypeHandle.__Game_Buildings_Patient_RO_BufferTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_HospitalDatas = this.__TypeHandle.__Game_Prefabs_HospitalData_RO_ComponentLookup,
        m_Result = this.m_Results
      }.Schedule<HealthcareInfoviewUISystem.UpdateHealthcareJob>(this.m_HealthcareFacilityQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DeathcareFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_DeathcareFacility_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      new HealthcareInfoviewUISystem.UpdateDeathcareJob()
      {
        m_DeathcareFacilityType = this.__TypeHandle.__Game_Buildings_DeathcareFacility_RO_ComponentTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_DeathcareFacilities = this.__TypeHandle.__Game_Prefabs_DeathcareFacilityData_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_Results = this.m_Results
      }.Schedule<HealthcareInfoviewUISystem.UpdateDeathcareJob>(this.m_DeathcareFacilityQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AverageHealth.Update(math.round(this.m_Results[0] / math.max(this.m_Results[1], 1f)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PatientCount.Update((int) this.m_Results[3]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SickCount.Update((int) this.m_Results[2]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PatientCapacity.Update((int) this.m_Results[4]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DeathRate.Update((float) this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.DeathRate, 0));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessingRate.Update(this.m_Results[5]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CemeteryUse.Update((int) this.m_Results[6]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CemeteryCapacity.Update((int) this.m_Results[7]);
      // ISSUE: reference to a compiler-generated field
      this.m_DeathcareAvailability.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_CemeteryAvailability.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareAvailability.Update();
    }

    private IndicatorValue GetHealthcareAvailability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return IndicatorValue.Calculate((float) this.m_PatientCapacity.value, (float) this.m_SickCount.value);
    }

    private IndicatorValue GetDeathcareAvailability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return IndicatorValue.Calculate(this.m_ProcessingRate.value, this.m_DeathRate.value);
    }

    private IndicatorValue GetCemeteryAvailability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return IndicatorValue.Calculate((float) this.m_CemeteryCapacity.value, (float) this.m_CemeteryUse.value);
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
    public HealthcareInfoviewUISystem()
    {
    }

    private enum Result
    {
      CitizenHealth,
      CitizenCount,
      SickCitizens,
      PatientCount,
      PatientCapacity,
      ProcessingRate,
      CemeteryUse,
      CemeteryCapacity,
      ResultCount,
    }

    [BurstCompile]
    public struct CalculateAverageHealthJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Household> m_HouseholdType;
      [ReadOnly]
      public BufferTypeHandle<HouseholdCitizen> m_HouseholdCitizenType;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblems;
      public NativeArray<float> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Household> nativeArray = chunk.GetNativeArray<Household>(ref this.m_HouseholdType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<HouseholdCitizen> bufferAccessor = chunk.GetBufferAccessor<HouseholdCitizen>(ref this.m_HouseholdCitizenType);
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          if ((nativeArray[index1].m_Flags & HouseholdFlags.MovedIn) != HouseholdFlags.None)
          {
            DynamicBuffer<HouseholdCitizen> dynamicBuffer = bufferAccessor[index1];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              Entity citizen1 = dynamicBuffer[index2].m_Citizen;
              Citizen citizen2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!CitizenUtils.IsDead(citizen1, ref this.m_HealthProblems) && CitizenUtils.TryGetResident(citizen1, this.m_Citizens, out citizen2))
              {
                HealthProblem componentData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_HealthProblems.TryGetComponent(citizen1, out componentData) && (componentData.m_Flags & (HealthProblemFlags.Sick | HealthProblemFlags.Injured)) != HealthProblemFlags.None)
                  ++num3;
                num2 += (int) citizen2.m_Health;
                ++num1;
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Results[0] += (float) num2;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[1] += (float) num1;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[2] += (float) num3;
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
    private struct UpdateHealthcareJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<Patient> m_PatientType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<HospitalData> m_HospitalDatas;
      public NativeArray<float> m_Result;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Patient> bufferAccessor1 = chunk.GetBufferAccessor<Patient>(ref this.m_PatientType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor3 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        int num1 = 0;
        int num2 = 0;
        for (int index = 0; index < chunk.Count; ++index)
        {
          PrefabRef prefabRef = nativeArray[index];
          DynamicBuffer<Patient> dynamicBuffer = bufferAccessor1[index];
          if ((double) BuildingUtils.GetEfficiency(bufferAccessor3, index) != 0.0)
          {
            HospitalData data = new HospitalData();
            // ISSUE: reference to a compiler-generated field
            if (this.m_HospitalDatas.HasComponent(prefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              data = this.m_HospitalDatas[prefabRef.m_Prefab];
            }
            if (bufferAccessor2.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UpgradeUtils.CombineStats<HospitalData>(ref data, bufferAccessor2[index], ref this.m_Prefabs, ref this.m_HospitalDatas);
            }
            num1 += dynamicBuffer.Length;
            num2 += data.m_PatientCapacity;
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Result[3] += (float) num1;
        // ISSUE: reference to a compiler-generated field
        this.m_Result[4] += (float) num2;
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
    private struct UpdateDeathcareJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.DeathcareFacility> m_DeathcareFacilityType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentLookup<DeathcareFacilityData> m_DeathcareFacilities;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      public NativeArray<float> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.DeathcareFacility> nativeArray1 = chunk.GetNativeArray<Game.Buildings.DeathcareFacility>(ref this.m_DeathcareFacilityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor1 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        float num1 = 0.0f;
        float num2 = 0.0f;
        float num3 = 0.0f;
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity prefab = nativeArray2[index].m_Prefab;
          float efficiency = BuildingUtils.GetEfficiency(bufferAccessor1, index);
          if ((double) efficiency != 0.0)
          {
            DeathcareFacilityData data = new DeathcareFacilityData();
            // ISSUE: reference to a compiler-generated field
            if (this.m_DeathcareFacilities.HasComponent(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              data = this.m_DeathcareFacilities[prefab];
            }
            if (bufferAccessor2.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UpgradeUtils.CombineStats<DeathcareFacilityData>(ref data, bufferAccessor2[index], ref this.m_Prefabs, ref this.m_DeathcareFacilities);
            }
            if (data.m_LongTermStorage)
            {
              Game.Buildings.DeathcareFacility deathcareFacility = nativeArray1[index];
              num2 += (float) deathcareFacility.m_LongTermStoredCount;
              num3 += (float) data.m_StorageCapacity;
            }
            num1 += efficiency * data.m_ProcessingRate;
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Results[5] += num1;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[6] += num2;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[7] += num3;
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
      public ComponentTypeHandle<Household> __Game_Citizens_Household_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Patient> __Game_Buildings_Patient_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HospitalData> __Game_Prefabs_HospitalData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.DeathcareFacility> __Game_Buildings_DeathcareFacility_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<DeathcareFacilityData> __Game_Prefabs_DeathcareFacilityData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle = state.GetBufferTypeHandle<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentLookup = state.GetComponentLookup<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Patient_RO_BufferTypeHandle = state.GetBufferTypeHandle<Patient>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HospitalData_RO_ComponentLookup = state.GetComponentLookup<HospitalData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_DeathcareFacility_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.DeathcareFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DeathcareFacilityData_RO_ComponentLookup = state.GetComponentLookup<DeathcareFacilityData>(true);
      }
    }
  }
}
