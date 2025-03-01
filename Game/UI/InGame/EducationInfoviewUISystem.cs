﻿// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.EducationInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

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
using UnityEngine;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class EducationInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "educationInfo";
    private SimulationSystem m_SimulationSystem;
    private CitySystem m_CitySystem;
    private RawValueBinding m_EducationData;
    private ValueBinding<int> m_ElementaryStudents;
    private ValueBinding<int> m_HighSchoolStudents;
    private ValueBinding<int> m_CollegeStudents;
    private ValueBinding<int> m_UniversityStudents;
    private ValueBinding<int> m_ElementaryEligible;
    private ValueBinding<int> m_HighSchoolEligible;
    private ValueBinding<int> m_CollegeEligible;
    private ValueBinding<int> m_UniversityEligible;
    private ValueBinding<int> m_ElementaryCapacity;
    private ValueBinding<int> m_HighSchoolCapacity;
    private ValueBinding<int> m_CollegeCapacity;
    private ValueBinding<int> m_UniversityCapacity;
    private GetterValueBinding<IndicatorValue> m_ElementaryAvailability;
    private GetterValueBinding<IndicatorValue> m_HighSchoolAvailability;
    private GetterValueBinding<IndicatorValue> m_CollegeAvailability;
    private GetterValueBinding<IndicatorValue> m_UniversityAvailability;
    private EntityQuery m_HouseholdQuery;
    private EntityQuery m_SchoolQuery;
    private EntityQuery m_SchoolModifiedQuery;
    private EntityQuery m_EligibleQuery;
    private EntityQuery m_TimeDataQuery;
    private NativeArray<int> m_Results;
    private EducationInfoviewUISystem.TypeHandle __TypeHandle;
    private EntityQuery __query_607537787_0;
    private EntityQuery __query_607537787_1;
    private EntityQuery __query_607537787_2;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      this.RequireForUpdate<EconomyParameterData>();
      this.RequireForUpdate<TimeData>();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdQuery = this.GetEntityQuery(ComponentType.ReadOnly<Household>(), ComponentType.ReadOnly<PropertyRenter>(), ComponentType.ReadOnly<HouseholdCitizen>(), ComponentType.Exclude<TouristHousehold>(), ComponentType.Exclude<CommuterHousehold>(), ComponentType.Exclude<MovingAway>());
      // ISSUE: reference to a compiler-generated field
      this.m_SchoolQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<Game.Buildings.School>(), ComponentType.ReadOnly<Game.Buildings.Student>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_SchoolModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Game.Buildings.School>()
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
      this.m_EligibleQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadWrite<Citizen>(),
          ComponentType.ReadOnly<UpdateFrame>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<HasJobSeeker>(),
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_EducationData = new RawValueBinding("educationInfo", "educationData", (Action<IJsonWriter>) (binder =>
      {
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
        new EducationInfoviewUISystem.UpdateEducationDataJob()
        {
          m_HouseholdHandle = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentTypeHandle,
          m_HouseholdCitizenHandle = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle,
          m_CitizenFromEntity = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
          m_HealthProblemFromEntity = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
          m_Results = this.m_Results
        }.Schedule<EducationInfoviewUISystem.UpdateEducationDataJob>(this.m_HouseholdQuery, this.Dependency).Complete();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InfoviewsUIUtils.UpdateFiveSlicePieChartData(binder, this.m_Results[0], this.m_Results[1], this.m_Results[2], this.m_Results[3], this.m_Results[4]);
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ElementaryStudents = new ValueBinding<int>("educationInfo", "elementaryStudentCount", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_HighSchoolStudents = new ValueBinding<int>("educationInfo", "highSchoolStudentCount", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CollegeStudents = new ValueBinding<int>("educationInfo", "collegeStudentCount", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_UniversityStudents = new ValueBinding<int>("educationInfo", "universityStudentCount", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ElementaryEligible = new ValueBinding<int>("educationInfo", "elementaryEligible", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_HighSchoolEligible = new ValueBinding<int>("educationInfo", "highSchoolEligible", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CollegeEligible = new ValueBinding<int>("educationInfo", "collegeEligible", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_UniversityEligible = new ValueBinding<int>("educationInfo", "universityEligible", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ElementaryCapacity = new ValueBinding<int>("educationInfo", "elementaryCapacity", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_HighSchoolCapacity = new ValueBinding<int>("educationInfo", "highSchoolCapacity", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CollegeCapacity = new ValueBinding<int>("educationInfo", "collegeCapacity", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_UniversityCapacity = new ValueBinding<int>("educationInfo", "universityCapacity", 0)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ElementaryAvailability = new GetterValueBinding<IndicatorValue>("educationInfo", "elementaryAvailability", (Func<IndicatorValue>) (() => IndicatorValue.Calculate((float) this.m_ElementaryCapacity.value, (float) this.m_ElementaryEligible.value)), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_HighSchoolAvailability = new GetterValueBinding<IndicatorValue>("educationInfo", "highSchoolAvailability", (Func<IndicatorValue>) (() => IndicatorValue.Calculate((float) this.m_HighSchoolCapacity.value, (float) this.m_HighSchoolEligible.value)), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CollegeAvailability = new GetterValueBinding<IndicatorValue>("educationInfo", "collegeAvailability", (Func<IndicatorValue>) (() => IndicatorValue.Calculate((float) this.m_CollegeCapacity.value, (float) this.m_CollegeEligible.value)), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_UniversityAvailability = new GetterValueBinding<IndicatorValue>("educationInfo", "universityAvailability", (Func<IndicatorValue>) (() => IndicatorValue.Calculate((float) this.m_UniversityCapacity.value, (float) this.m_UniversityEligible.value)), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<int>(17, Allocator.Persistent);
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
        return base.Active || this.m_EducationData.active || this.m_ElementaryStudents.active || this.m_ElementaryCapacity.active || this.m_ElementaryEligible.active || this.m_ElementaryAvailability.active || this.m_HighSchoolStudents.active || this.m_HighSchoolCapacity.active || this.m_HighSchoolEligible.active || this.m_HighSchoolAvailability.active || this.m_CollegeStudents.active || this.m_CollegeCapacity.active || this.m_CollegeEligible.active || this.m_CollegeAvailability.active || this.m_UniversityStudents.active || this.m_UniversityCapacity.active || this.m_UniversityEligible.active || this.m_UniversityAvailability.active;
      }
    }

    protected override bool Modified => !this.m_SchoolModifiedQuery.IsEmptyIgnoreFilter;

    protected override void PerformUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.ResetResults();
      // ISSUE: reference to a compiler-generated method
      this.UpdateStudentCounts();
      // ISSUE: reference to a compiler-generated method
      this.UpdateEligibility();
      // ISSUE: reference to a compiler-generated field
      this.m_EducationData.Update();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ElementaryStudents.Update(this.m_Results[9]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ElementaryCapacity.Update(this.m_Results[13]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ElementaryEligible.Update(this.m_Results[5]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_HighSchoolStudents.Update(this.m_Results[10]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_HighSchoolCapacity.Update(this.m_Results[14]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_HighSchoolEligible.Update(this.m_Results[6]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CollegeStudents.Update(this.m_Results[11]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CollegeCapacity.Update(this.m_Results[15]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CollegeEligible.Update(this.m_Results[7]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UniversityStudents.Update(this.m_Results[12]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UniversityCapacity.Update(this.m_Results[16]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UniversityEligible.Update(this.m_Results[8]);
      // ISSUE: reference to a compiler-generated field
      this.m_ElementaryAvailability.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_HighSchoolAvailability.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_CollegeAvailability.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_UniversityAvailability.Update();
    }

    private void ResetResults()
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Results.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Results[index] = 0;
      }
    }

    private void UpdateEducationData(IJsonWriter binder)
    {
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
      new EducationInfoviewUISystem.UpdateEducationDataJob()
      {
        m_HouseholdHandle = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentTypeHandle,
        m_HouseholdCitizenHandle = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle,
        m_CitizenFromEntity = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_HealthProblemFromEntity = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
        m_Results = this.m_Results
      }.Schedule<EducationInfoviewUISystem.UpdateEducationDataJob>(this.m_HouseholdQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      InfoviewsUIUtils.UpdateFiveSlicePieChartData(binder, this.m_Results[0], this.m_Results[1], this.m_Results[2], this.m_Results[3], this.m_Results[4]);
    }

    private void UpdateStudentCounts()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SchoolData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Student_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new EducationInfoviewUISystem.UpdateStudentCountsJob()
      {
        m_EntityHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_StudentHandle = this.__TypeHandle.__Game_Buildings_Student_RO_BufferTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_PrefabRefTypeHandle = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PrefabRefFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_SchoolDataFromEntity = this.__TypeHandle.__Game_Prefabs_SchoolData_RO_ComponentLookup,
        m_InstalledUpgradeFromEntity = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_Results = this.m_Results
      }.Schedule<EducationInfoviewUISystem.UpdateStudentCountsJob>(this.m_SchoolQuery, this.Dependency).Complete();
    }

    private void UpdateEligibility()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_ServiceFee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Student_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new EducationInfoviewUISystem.UpdateEligibilityJob()
      {
        m_EntityHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CitizenHandle = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle,
        m_StudentHandle = this.__TypeHandle.__Game_Citizens_Student_RO_ComponentTypeHandle,
        m_WorkerHandle = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentTypeHandle,
        m_HouseholdMemberFromEntity = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_HouseholdFromEntity = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_HealthProblemFromEntity = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_MovingAways = this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup,
        m_ServiceFeeFromEntity = this.__TypeHandle.__Game_City_ServiceFee_RO_BufferLookup,
        m_CityModifierFromEntity = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_City = this.m_CitySystem.City,
        m_EconomyParameterData = this.__query_607537787_0.GetSingleton<EconomyParameterData>(),
        m_EducationParameterData = this.__query_607537787_1.GetSingleton<EducationParameterData>(),
        m_TimeData = this.__query_607537787_2.GetSingleton<TimeData>(),
        m_Results = this.m_Results
      }.Schedule<EducationInfoviewUISystem.UpdateEligibilityJob>(this.m_EligibleQuery, this.Dependency).Complete();
    }

    private IndicatorValue UpdateElementaryAvailability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return IndicatorValue.Calculate((float) this.m_ElementaryCapacity.value, (float) this.m_ElementaryEligible.value);
    }

    private IndicatorValue UpdateHighSchoolAvailability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return IndicatorValue.Calculate((float) this.m_HighSchoolCapacity.value, (float) this.m_HighSchoolEligible.value);
    }

    private IndicatorValue UpdateCollegeAvailability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return IndicatorValue.Calculate((float) this.m_CollegeCapacity.value, (float) this.m_CollegeEligible.value);
    }

    private IndicatorValue UpdateUniversityAvailability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return IndicatorValue.Calculate((float) this.m_UniversityCapacity.value, (float) this.m_UniversityEligible.value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_607537787_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<EconomyParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_607537787_1 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<EducationParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_607537787_2 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<TimeData>()
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
    public EducationInfoviewUISystem()
    {
    }

    private enum Result
    {
      Uneducated,
      ElementaryEducated,
      HighSchoolEducated,
      CollegeEducated,
      UniversityEducated,
      ElementaryEligible,
      HighSchoolEligible,
      CollegeEligible,
      UniversityEligible,
      ElementaryStudents,
      HighSchoolStudents,
      CollegeStudents,
      UniversityStudents,
      ElementaryCapacity,
      HighSchoolCapacity,
      CollegeCapacity,
      UniversityCapacity,
      Count,
    }

    [BurstCompile]
    private struct UpdateEducationDataJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Household> m_HouseholdHandle;
      [ReadOnly]
      public BufferTypeHandle<HouseholdCitizen> m_HouseholdCitizenHandle;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenFromEntity;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblemFromEntity;
      public NativeArray<int> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Household> nativeArray = chunk.GetNativeArray<Household>(ref this.m_HouseholdHandle);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<HouseholdCitizen> bufferAccessor = chunk.GetBufferAccessor<HouseholdCitizen>(ref this.m_HouseholdCitizenHandle);
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        int num5 = 0;
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          Household household = nativeArray[index1];
          DynamicBuffer<HouseholdCitizen> dynamicBuffer = bufferAccessor[index1];
          if ((household.m_Flags & HouseholdFlags.MovedIn) != HouseholdFlags.None)
          {
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              Entity citizen = dynamicBuffer[index2].m_Citizen;
              Citizen componentData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!CitizenUtils.IsDead(citizen, ref this.m_HealthProblemFromEntity) && this.m_CitizenFromEntity.TryGetComponent(citizen, out componentData))
              {
                switch (componentData.GetEducationLevel())
                {
                  case 0:
                    ++num1;
                    continue;
                  case 1:
                    ++num2;
                    continue;
                  case 2:
                    ++num3;
                    continue;
                  case 3:
                    ++num4;
                    continue;
                  case 4:
                    ++num5;
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Results[0] += num1;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[1] += num2;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[2] += num3;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[3] += num4;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[4] += num5;
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
    private struct UpdateStudentCountsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Buildings.Student> m_StudentHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefFromEntity;
      [ReadOnly]
      public ComponentLookup<SchoolData> m_SchoolDataFromEntity;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgradeFromEntity;
      public NativeArray<int> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefTypeHandle);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Buildings.Student> bufferAccessor1 = chunk.GetBufferAccessor<Game.Buildings.Student>(ref this.m_StudentHandle);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor2 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Entity prefab = nativeArray2[index].m_Prefab;
          if ((double) BuildingUtils.GetEfficiency(bufferAccessor2, index) != 0.0)
          {
            DynamicBuffer<Game.Buildings.Student> dynamicBuffer = bufferAccessor1[index];
            SchoolData componentData;
            // ISSUE: reference to a compiler-generated field
            this.m_SchoolDataFromEntity.TryGetComponent(prefab, out componentData);
            DynamicBuffer<InstalledUpgrade> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_InstalledUpgradeFromEntity.TryGetBuffer(entity, out bufferData))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UpgradeUtils.CombineStats<SchoolData>(ref componentData, bufferData, ref this.m_PrefabRefFromEntity, ref this.m_SchoolDataFromEntity);
            }
            switch (componentData.m_EducationLevel)
            {
              case 1:
                // ISSUE: reference to a compiler-generated field
                this.m_Results[9] += dynamicBuffer.Length;
                // ISSUE: reference to a compiler-generated field
                this.m_Results[13] += componentData.m_StudentCapacity;
                continue;
              case 2:
                // ISSUE: reference to a compiler-generated field
                this.m_Results[10] += dynamicBuffer.Length;
                // ISSUE: reference to a compiler-generated field
                this.m_Results[14] += componentData.m_StudentCapacity;
                continue;
              case 3:
                // ISSUE: reference to a compiler-generated field
                this.m_Results[11] += dynamicBuffer.Length;
                // ISSUE: reference to a compiler-generated field
                this.m_Results[15] += componentData.m_StudentCapacity;
                continue;
              case 4:
                // ISSUE: reference to a compiler-generated field
                this.m_Results[12] += dynamicBuffer.Length;
                // ISSUE: reference to a compiler-generated field
                this.m_Results[16] += componentData.m_StudentCapacity;
                continue;
              default:
                continue;
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

    [BurstCompile]
    private struct UpdateEligibilityJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityHandle;
      [ReadOnly]
      public ComponentTypeHandle<Citizen> m_CitizenHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Citizens.Student> m_StudentHandle;
      [ReadOnly]
      public ComponentTypeHandle<Worker> m_WorkerHandle;
      [ReadOnly]
      public ComponentLookup<Household> m_HouseholdFromEntity;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMemberFromEntity;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblemFromEntity;
      [ReadOnly]
      public ComponentLookup<MovingAway> m_MovingAways;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public BufferLookup<ServiceFee> m_ServiceFeeFromEntity;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifierFromEntity;
      public uint m_SimulationFrame;
      public Entity m_City;
      public EconomyParameterData m_EconomyParameterData;
      public EducationParameterData m_EducationParameterData;
      public TimeData m_TimeData;
      public NativeArray<int> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Citizen> nativeArray2 = chunk.GetNativeArray<Citizen>(ref this.m_CitizenHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Citizens.Student> nativeArray3 = chunk.GetNativeArray<Game.Citizens.Student>(ref this.m_StudentHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Worker> nativeArray4 = chunk.GetNativeArray<Worker>(ref this.m_WorkerHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ServiceFee> dynamicBuffer = this.m_ServiceFeeFromEntity[this.m_City];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifiers = this.m_CityModifierFromEntity[this.m_City];
        float f1 = 0.0f;
        float f2 = 0.0f;
        float f3 = 0.0f;
        float f4 = 0.0f;
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          HouseholdMember componentData1;
          Household componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!CitizenUtils.IsDead(entity, ref this.m_HealthProblemFromEntity) && this.m_HouseholdMemberFromEntity.TryGetComponent(entity, out componentData1) && this.m_HouseholdFromEntity.TryGetComponent(componentData1.m_Household, out componentData2) && (componentData2.m_Flags & HouseholdFlags.MovedIn) != HouseholdFlags.None && (componentData2.m_Flags & HouseholdFlags.Tourist) == HouseholdFlags.None && !this.m_MovingAways.HasComponent(componentData1.m_Household) && this.m_PropertyRenters.HasComponent(componentData1.m_Household))
          {
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<Game.Citizens.Student>(ref this.m_StudentHandle))
            {
              switch (nativeArray3[index].m_Level)
              {
                case 1:
                  ++f1;
                  continue;
                case 2:
                  ++f2;
                  continue;
                case 3:
                  ++f3;
                  continue;
                case 4:
                  ++f4;
                  continue;
                default:
                  continue;
              }
            }
            else
            {
              Citizen citizen = nativeArray2[index];
              CitizenAge age = citizen.GetAge();
              float willingness = citizen.GetPseudoRandom(CitizenPseudoRandom.StudyWillingness).NextFloat();
              if (age == CitizenAge.Child)
                ++f1;
              else if (citizen.GetEducationLevel() == 1 && age <= CitizenAge.Adult)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                f2 += ApplyToSchoolSystem.GetEnteringProbability(age, nativeArray4.IsCreated, 2, (int) citizen.m_WellBeing, willingness, cityModifiers, ref this.m_EducationParameterData);
              }
              else
              {
                int failedEducationCount = citizen.GetFailedEducationCount();
                if (citizen.GetEducationLevel() == 2 && failedEducationCount < 3)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  float enteringProbability = ApplyToSchoolSystem.GetEnteringProbability(age, nativeArray4.IsCreated, 4, (int) citizen.m_WellBeing, willingness, cityModifiers, ref this.m_EducationParameterData);
                  f4 += enteringProbability;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  f3 += (1f - enteringProbability) * ApplyToSchoolSystem.GetEnteringProbability(age, nativeArray4.IsCreated, 3, (int) citizen.m_WellBeing, willingness, cityModifiers, ref this.m_EducationParameterData);
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Results[5] += Mathf.CeilToInt(f1);
        // ISSUE: reference to a compiler-generated field
        this.m_Results[6] += Mathf.CeilToInt(f2);
        // ISSUE: reference to a compiler-generated field
        this.m_Results[7] += Mathf.CeilToInt(f3);
        // ISSUE: reference to a compiler-generated field
        this.m_Results[8] += Mathf.CeilToInt(f4);
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
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Buildings.Student> __Game_Buildings_Student_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SchoolData> __Game_Prefabs_SchoolData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Citizen> __Game_Citizens_Citizen_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Citizens.Student> __Game_Citizens_Student_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Worker> __Game_Citizens_Worker_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MovingAway> __Game_Agents_MovingAway_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ServiceFee> __Game_City_ServiceFee_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

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
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Student_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Buildings.Student>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SchoolData_RO_ComponentLookup = state.GetComponentLookup<SchoolData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Student_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Citizens.Student>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Worker_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Worker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_MovingAway_RO_ComponentLookup = state.GetComponentLookup<MovingAway>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_ServiceFee_RO_BufferLookup = state.GetBufferLookup<ServiceFee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
