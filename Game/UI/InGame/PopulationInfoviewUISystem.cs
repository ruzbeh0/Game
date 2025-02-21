// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.PopulationInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Simulation;
using Game.Tools;
using System;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class PopulationInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "populationInfo";
    private CityStatisticsSystem m_CityStatisticsSystem;
    private CitySystem m_CitySystem;
    private CountWorkplacesSystem m_CountWorkplacesSystem;
    private CountHouseholdDataSystem m_CountHouseholdDataSystem;
    private ValueBinding<int> m_Population;
    private ValueBinding<int> m_Employed;
    private ValueBinding<int> m_Jobs;
    private ValueBinding<float> m_Unemployment;
    private ValueBinding<int> m_BirthRate;
    private ValueBinding<int> m_DeathRate;
    private ValueBinding<int> m_MovedIn;
    private ValueBinding<int> m_MovedAway;
    private RawValueBinding m_AgeData;
    private EntityQuery m_WorkProviderModifiedQuery;
    private EntityQuery m_PopulationModifiedQuery;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      this.m_CountWorkplacesSystem = this.World.GetOrCreateSystemManaged<CountWorkplacesSystem>();
      this.m_CountHouseholdDataSystem = this.World.GetOrCreateSystemManaged<CountHouseholdDataSystem>();
      this.m_WorkProviderModifiedQuery = this.GetEntityQuery(ComponentType.ReadOnly<WorkProvider>(), ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<Deleted>(), ComponentType.ReadOnly<Updated>(), ComponentType.Exclude<Temp>());
      this.m_PopulationModifiedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Population>(), ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<Deleted>(), ComponentType.ReadOnly<Updated>(), ComponentType.Exclude<Temp>());
      this.AddBinding((IBinding) (this.m_Population = new ValueBinding<int>("populationInfo", "population", 0)));
      this.AddBinding((IBinding) (this.m_Employed = new ValueBinding<int>("populationInfo", "employed", 0)));
      this.AddBinding((IBinding) (this.m_Jobs = new ValueBinding<int>("populationInfo", "jobs", 0)));
      this.AddBinding((IBinding) (this.m_Unemployment = new ValueBinding<float>("populationInfo", "unemployment", 0.0f)));
      this.AddBinding((IBinding) (this.m_BirthRate = new ValueBinding<int>("populationInfo", "birthRate", 0)));
      this.AddBinding((IBinding) (this.m_DeathRate = new ValueBinding<int>("populationInfo", "deathRate", 0)));
      this.AddBinding((IBinding) (this.m_MovedIn = new ValueBinding<int>("populationInfo", "movedIn", 0)));
      this.AddBinding((IBinding) (this.m_MovedAway = new ValueBinding<int>("populationInfo", "movedAway", 0)));
      this.AddBinding((IBinding) (this.m_AgeData = new RawValueBinding("populationInfo", "ageData", new Action<IJsonWriter>(this.UpdateAgeData))));
    }

    protected override bool Active
    {
      get
      {
        return base.Active || this.m_Population.active || this.m_Employed.active || this.m_Jobs.active || this.m_Unemployment.active || this.m_BirthRate.active || this.m_DeathRate.active || this.m_MovedIn.active || this.m_MovedAway.active || this.m_AgeData.active;
      }
    }

    protected override bool Modified
    {
      get
      {
        return !this.m_WorkProviderModifiedQuery.IsEmptyIgnoreFilter || this.m_PopulationModifiedQuery.IsEmptyIgnoreFilter;
      }
    }

    protected override void PerformUpdate() => this.UpdateBindings();

    private void UpdateBindings()
    {
      // ISSUE: reference to a compiler-generated method
      this.m_Jobs.Update(this.m_CountWorkplacesSystem.GetTotalWorkplaces().TotalCount);
      this.m_Employed.Update(this.m_CountHouseholdDataSystem.CityWorkerCount);
      this.m_Unemployment.Update(this.m_CountHouseholdDataSystem.UnemploymentRate);
      this.m_Population.Update(this.EntityManager.GetComponentData<Population>(this.m_CitySystem.City).m_Population);
      this.m_AgeData.Update();
      this.UpdateStatistics();
    }

    private void UpdateAgeData(IJsonWriter binder)
    {
      binder.TypeBegin("infoviews.ChartData");
      binder.PropertyName("values");
      binder.ArrayBegin(4U);
      binder.Write(this.m_CountHouseholdDataSystem.ChildrenCount);
      binder.Write(this.m_CountHouseholdDataSystem.TeenCount);
      binder.Write(this.m_CountHouseholdDataSystem.AdultCount);
      binder.Write(this.m_CountHouseholdDataSystem.SeniorCount);
      binder.ArrayEnd();
      binder.PropertyName("total");
      binder.Write(this.m_CountHouseholdDataSystem.MovedInCitizenCount);
      binder.TypeEnd();
    }

    private void UpdateStatistics()
    {
      // ISSUE: reference to a compiler-generated method
      this.m_BirthRate.Update(this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.BirthRate, 0));
      // ISSUE: reference to a compiler-generated method
      this.m_DeathRate.Update(this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.DeathRate, 0));
      // ISSUE: reference to a compiler-generated method
      this.m_MovedIn.Update(this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.CitizensMovedIn, 0));
      // ISSUE: reference to a compiler-generated method
      this.m_MovedAway.Update(this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.CitizensMovedAway, 0));
    }

    [Preserve]
    public PopulationInfoviewUISystem()
    {
    }
  }
}
