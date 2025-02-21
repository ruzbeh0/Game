// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.CitizenUIUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Buildings;
using Game.Citizens;
using Game.Companies;
using Game.Creatures;
using Game.Economy;
using Game.Events;
using Game.Prefabs;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.InGame
{
  public static class CitizenUIUtils
  {
    public static HouseholdWealthKey GetHouseholdWealth(
      EntityManager entityManager,
      Entity householdEntity,
      CitizenHappinessParameterData happinessParameters)
    {
      if (!entityManager.Exists(householdEntity))
        return HouseholdWealthKey.Modest;
      int num = 0;
      DynamicBuffer<Game.Economy.Resources> buffer;
      Household component;
      if (entityManager.TryGetBuffer<Game.Economy.Resources>(householdEntity, true, out buffer) && entityManager.TryGetComponent<Household>(householdEntity, out component))
        num += EconomyUtils.GetHouseholdTotalWealth(component, buffer);
      if (num < happinessParameters.m_WealthyMoneyAmount.x)
        return HouseholdWealthKey.Wretched;
      if (num < happinessParameters.m_WealthyMoneyAmount.y)
        return HouseholdWealthKey.Poor;
      if (num < happinessParameters.m_WealthyMoneyAmount.z)
        return HouseholdWealthKey.Modest;
      return num < happinessParameters.m_WealthyMoneyAmount.w ? HouseholdWealthKey.Comfortable : HouseholdWealthKey.Wealthy;
    }

    public static HouseholdWealthKey GetAverageHouseholdWealth(
      EntityManager entityManager,
      NativeList<Entity> households,
      CitizenHappinessParameterData happinessParameters)
    {
      int num1 = 0;
      for (int index = 0; index < households.Length; ++index)
      {
        Entity household = households[index];
        DynamicBuffer<Game.Economy.Resources> buffer;
        Household component;
        if (entityManager.TryGetBuffer<Game.Economy.Resources>(household, true, out buffer) && entityManager.TryGetComponent<Household>(household, out component))
          num1 += EconomyUtils.GetHouseholdTotalWealth(component, buffer);
      }
      int num2 = num1 / math.select(households.Length, 1, households.Length == 0);
      if (num2 < happinessParameters.m_WealthyMoneyAmount.x)
        return HouseholdWealthKey.Wretched;
      if (num2 < happinessParameters.m_WealthyMoneyAmount.y)
        return HouseholdWealthKey.Poor;
      if (num2 < happinessParameters.m_WealthyMoneyAmount.z)
        return HouseholdWealthKey.Modest;
      return num2 < happinessParameters.m_WealthyMoneyAmount.w ? HouseholdWealthKey.Comfortable : HouseholdWealthKey.Wealthy;
    }

    public static CitizenJobLevelKey GetJobLevel(EntityManager entityManager, Entity entity)
    {
      Worker component;
      return entityManager.TryGetComponent<Worker>(entity, out component) ? (CitizenJobLevelKey) component.m_Level : CitizenJobLevelKey.Unknown;
    }

    public static CitizenAgeKey GetAge(EntityManager entityManager, Entity entity)
    {
      Citizen component;
      return entityManager.TryGetComponent<Citizen>(entity, out component) ? (CitizenAgeKey) component.GetAge() : CitizenAgeKey.Adult;
    }

    public static CitizenOccupationKey GetOccupation(EntityManager entityManager, Entity entity)
    {
      Entity household = entityManager.GetComponentData<HouseholdMember>(entity).m_Household;
      if (entityManager.Exists(household) && entityManager.HasComponent<TouristHousehold>(household))
        return CitizenOccupationKey.Tourist;
      Criminal component1;
      if (entityManager.TryGetComponent<Criminal>(entity, out component1))
        return (component1.m_Flags & CriminalFlags.Robber) == (CriminalFlags) 0 ? CitizenOccupationKey.Criminal : CitizenOccupationKey.Robber;
      if (entityManager.HasComponent<Worker>(entity))
        return CitizenOccupationKey.Worker;
      if (entityManager.HasComponent<Game.Citizens.Student>(entity))
        return CitizenOccupationKey.Student;
      Citizen component2;
      if (entityManager.TryGetComponent<Citizen>(entity, out component2))
      {
        switch (component2.GetAge())
        {
          case CitizenAge.Child:
          case CitizenAge.Teen:
            return CitizenOccupationKey.None;
          case CitizenAge.Adult:
            return CitizenOccupationKey.Unemployed;
          case CitizenAge.Elderly:
            return CitizenOccupationKey.Retired;
        }
      }
      return CitizenOccupationKey.Unknown;
    }

    public static CitizenEducationKey GetEducation(Citizen citizen)
    {
      switch (citizen.GetEducationLevel())
      {
        case 1:
          return CitizenEducationKey.PoorlyEducated;
        case 2:
          return CitizenEducationKey.Educated;
        case 3:
          return CitizenEducationKey.WellEducated;
        case 4:
          return CitizenEducationKey.HighlyEducated;
        default:
          return CitizenEducationKey.Uneducated;
      }
    }

    public static Entity GetResidenceEntity(EntityManager entityManager, Entity citizenEntity)
    {
      Entity household = entityManager.GetComponentData<HouseholdMember>(citizenEntity).m_Household;
      TouristHousehold component1;
      if (entityManager.TryGetComponent<TouristHousehold>(household, out component1))
        return component1.m_Hotel;
      PropertyRenter component2;
      return entityManager.TryGetComponent<PropertyRenter>(household, out component2) ? component2.m_Property : Entity.Null;
    }

    public static CitizenResidenceKey GetResidenceType(Citizen citizen)
    {
      return (citizen.m_State & CitizenFlags.Tourist) == 0 ? CitizenResidenceKey.Home : CitizenResidenceKey.Hotel;
    }

    public static Entity GetWorkplaceEntity(EntityManager entityManager, Entity citizenEntity)
    {
      Worker component1;
      if (!entityManager.TryGetComponent<Worker>(citizenEntity, out component1))
        return Entity.Null;
      PropertyRenter component2;
      return !entityManager.TryGetComponent<PropertyRenter>(component1.m_Workplace, out component2) ? component1.m_Workplace : component2.m_Property;
    }

    public static Entity GetCompanyEntity(EntityManager entityManager, Entity citizenEntity)
    {
      Worker component;
      return !entityManager.TryGetComponent<Worker>(citizenEntity, out component) ? Entity.Null : component.m_Workplace;
    }

    public static CitizenWorkplaceKey GetWorkplaceType(EntityManager entityManager, Entity entity)
    {
      Worker component;
      return (!entityManager.TryGetComponent<Worker>(entity, out component) ? 0 : (entityManager.HasComponent<CompanyData>(component.m_Workplace) ? 1 : 0)) == 0 ? CitizenWorkplaceKey.Building : CitizenWorkplaceKey.Company;
    }

    public static Entity GetSchoolEntity(
      EntityManager entityManager,
      Entity citizenEntity,
      out int level)
    {
      Game.Citizens.Student component;
      if (entityManager.TryGetComponent<Game.Citizens.Student>(citizenEntity, out component))
      {
        level = (int) component.m_Level;
        return component.m_School;
      }
      level = 0;
      return Entity.Null;
    }

    public static CitizenStateKey GetStateKey(EntityManager entityManager, Entity entity)
    {
      Citizen componentData1 = entityManager.GetComponentData<Citizen>(entity);
      HouseholdMember componentData2 = entityManager.GetComponentData<HouseholdMember>(entity);
      Household componentData3 = entityManager.GetComponentData<Household>(componentData2.m_Household);
      if (CitizenUtils.IsDead(entityManager, entity))
        return CitizenStateKey.Dead;
      CurrentTransport component;
      if (entityManager.TryGetComponent<CurrentTransport>(entity, out component) && entityManager.HasComponent<Creature>(component.m_CurrentTransport) && entityManager.HasComponent<InvolvedInAccident>(component.m_CurrentTransport))
        return CitizenStateKey.InvolvedInAccident;
      Game.Citizens.Purpose purpose;
      if (!CitizenUIUtils.TryGetTravelPurpose(entityManager, entity, out purpose))
        return CitizenStateKey.Idling;
      bool flag1 = (componentData1.m_State & CitizenFlags.Tourist) != 0;
      bool flag2 = (componentData1.m_State & CitizenFlags.Commuter) != 0;
      bool flag3 = (componentData3.m_Flags & HouseholdFlags.MovedIn) != 0;
      switch (purpose)
      {
        case Game.Citizens.Purpose.Shopping:
          return CitizenStateKey.Shopping;
        case Game.Citizens.Purpose.Leisure:
        case Game.Citizens.Purpose.VisitAttractions:
          return !flag1 ? CitizenStateKey.FreeTime : CitizenStateKey.Sightseeing;
        case Game.Citizens.Purpose.GoingHome:
          if (flag1)
            return CitizenStateKey.GoingBackToHotel;
          return !(flag3 | flag2) ? CitizenStateKey.MovingIn : CitizenStateKey.GoingHome;
        case Game.Citizens.Purpose.GoingToWork:
          return CitizenStateKey.GoingToWork;
        case Game.Citizens.Purpose.Working:
          return CitizenStateKey.Working;
        case Game.Citizens.Purpose.Sleeping:
          return CitizenStateKey.Sleeping;
        case Game.Citizens.Purpose.Exporting:
          return CitizenStateKey.Traveling;
        case Game.Citizens.Purpose.MovingAway:
          return !flag1 ? CitizenStateKey.MovingAway : CitizenStateKey.LeavingCity;
        case Game.Citizens.Purpose.Studying:
          return CitizenStateKey.Studying;
        case Game.Citizens.Purpose.GoingToSchool:
          return CitizenStateKey.GoingToSchool;
        case Game.Citizens.Purpose.Hospital:
          return CitizenStateKey.SeekingMedicalHelp;
        case Game.Citizens.Purpose.Safety:
          return !CitizenUIUtils.PathEndReached(entityManager, entity) ? CitizenStateKey.GettingToSafety : CitizenStateKey.Safe;
        case Game.Citizens.Purpose.EmergencyShelter:
          return CitizenStateKey.Evacuating;
        case Game.Citizens.Purpose.Crime:
          return CitizenStateKey.CommittingCrime;
        case Game.Citizens.Purpose.GoingToJail:
          return CitizenStateKey.GoingToJail;
        case Game.Citizens.Purpose.GoingToPrison:
          return CitizenStateKey.GoingToPrison;
        case Game.Citizens.Purpose.InJail:
          return CitizenStateKey.InJail;
        case Game.Citizens.Purpose.InPrison:
          return CitizenStateKey.InPrison;
        case Game.Citizens.Purpose.Escape:
          return CitizenStateKey.Escaping;
        case Game.Citizens.Purpose.InHospital:
          return CitizenStateKey.InHospital;
        case Game.Citizens.Purpose.SendMail:
          return CitizenStateKey.SendMail;
        case Game.Citizens.Purpose.InEmergencyShelter:
          return CitizenStateKey.InEmergencyShelter;
        default:
          return CitizenStateKey.Idling;
      }
    }

    private static bool TryGetTravelPurpose(
      EntityManager entityManager,
      Entity entity,
      out Game.Citizens.Purpose purpose)
    {
      CurrentTransport component1;
      Divert component2;
      if (entityManager.TryGetComponent<CurrentTransport>(entity, out component1) && entityManager.TryGetComponent<Divert>(component1.m_CurrentTransport, out component2))
      {
        switch (component2.m_Purpose)
        {
          case Game.Citizens.Purpose.Shopping:
          case Game.Citizens.Purpose.Safety:
          case Game.Citizens.Purpose.SendMail:
            purpose = component2.m_Purpose;
            return true;
        }
      }
      TravelPurpose component3;
      if (entityManager.TryGetComponent<TravelPurpose>(entity, out component3))
      {
        purpose = component3.m_Purpose;
        return true;
      }
      purpose = Game.Citizens.Purpose.None;
      return false;
    }

    private static bool PathEndReached(EntityManager entityManager, Entity citizen)
    {
      CurrentTransport component1;
      HumanCurrentLane component2;
      return entityManager.TryGetComponent<CurrentTransport>(citizen, out component1) && entityManager.TryGetComponent<HumanCurrentLane>(component1.m_CurrentTransport, out component2) && CreatureUtils.PathEndReached(component2);
    }

    public static NativeList<CitizenCondition> GetCitizenConditions(
      EntityManager entityManager,
      Entity entity,
      Citizen citizen,
      HouseholdMember householdMember,
      NativeList<CitizenCondition> conditions)
    {
      HealthProblem component1;
      CitizenCondition citizenCondition;
      if (entityManager.TryGetComponent<HealthProblem>(entity, out component1))
      {
        if ((component1.m_Flags & HealthProblemFlags.Sick) != HealthProblemFlags.None)
        {
          ref NativeList<CitizenCondition> local1 = ref conditions;
          citizenCondition = new CitizenCondition(CitizenConditionKey.Sick);
          ref CitizenCondition local2 = ref citizenCondition;
          local1.Add(in local2);
        }
        if ((component1.m_Flags & HealthProblemFlags.Injured) != HealthProblemFlags.None)
        {
          ref NativeList<CitizenCondition> local3 = ref conditions;
          citizenCondition = new CitizenCondition(CitizenConditionKey.Injured);
          ref CitizenCondition local4 = ref citizenCondition;
          local3.Add(in local4);
        }
        if ((component1.m_Flags & (HealthProblemFlags.InDanger | HealthProblemFlags.Trapped)) != HealthProblemFlags.None)
        {
          ref NativeList<CitizenCondition> local5 = ref conditions;
          citizenCondition = new CitizenCondition(CitizenConditionKey.InDistress);
          ref CitizenCondition local6 = ref citizenCondition;
          local5.Add(in local6);
        }
      }
      if (!entityManager.HasComponent<CommuterHousehold>(householdMember.m_Household) && !entityManager.HasComponent<TouristHousehold>(householdMember.m_Household) && BuildingUtils.IsHomelessHousehold(entityManager, householdMember.m_Household))
      {
        ref NativeList<CitizenCondition> local7 = ref conditions;
        citizenCondition = new CitizenCondition(CitizenConditionKey.Homeless);
        ref CitizenCondition local8 = ref citizenCondition;
        local7.Add(in local8);
      }
      CurrentBuilding component2;
      if (entityManager.TryGetComponent<CurrentBuilding>(entity, out component2) && entityManager.HasComponent<Game.Buildings.EmergencyShelter>(component2.m_CurrentBuilding))
      {
        ref NativeList<CitizenCondition> local9 = ref conditions;
        citizenCondition = new CitizenCondition(CitizenConditionKey.Evacuated);
        ref CitizenCondition local10 = ref citizenCondition;
        local9.Add(in local10);
      }
      if (citizen.m_Health <= (byte) 25)
      {
        ref NativeList<CitizenCondition> local11 = ref conditions;
        citizenCondition = new CitizenCondition(CitizenConditionKey.Weak);
        ref CitizenCondition local12 = ref citizenCondition;
        local11.Add(in local12);
      }
      if (citizen.m_WellBeing <= (byte) 25)
      {
        ref NativeList<CitizenCondition> local13 = ref conditions;
        citizenCondition = new CitizenCondition(CitizenConditionKey.Unwell);
        ref CitizenCondition local14 = ref citizenCondition;
        local13.Add(in local14);
      }
      conditions.Sort<CitizenCondition>();
      return conditions;
    }

    public static CitizenHappiness GetCitizenHappiness(Citizen citizen)
    {
      return new CitizenHappiness((CitizenHappinessKey) CitizenUtils.GetHappinessKey(citizen.Happiness));
    }

    public static CitizenHappiness GetCitizenHappiness(int happiness)
    {
      return new CitizenHappiness((CitizenHappinessKey) CitizenUtils.GetHappinessKey(happiness));
    }
  }
}
