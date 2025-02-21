// Decompiled with JetBrains decompiler
// Type: Game.Citizens.CitizenUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Agents;
using Game.Pathfind;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Citizens
{
  public static class CitizenUtils
  {
    public static bool IsDead(Entity citizen, ref ComponentLookup<HealthProblem> healthProblems)
    {
      HealthProblem componentData;
      return healthProblems.TryGetComponent(citizen, out componentData) && CitizenUtils.IsDead(componentData);
    }

    public static bool IsDead(EntityManager entityManager, Entity citizen)
    {
      HealthProblem component;
      return entityManager.TryGetComponent<HealthProblem>(citizen, out component) && CitizenUtils.IsDead(component);
    }

    public static bool IsDead(HealthProblem healthProblem)
    {
      return (healthProblem.m_Flags & HealthProblemFlags.Dead) != 0;
    }

    public static bool IsCorpsePickedByHearse(
      Entity citizen,
      ref ComponentLookup<HealthProblem> healthProblems,
      ref ComponentLookup<TravelPurpose> travelPurposes)
    {
      TravelPurpose componentData;
      return CitizenUtils.IsDead(citizen, ref healthProblems) && travelPurposes.TryGetComponent(citizen, out componentData) && (componentData.m_Purpose == Purpose.Deathcare || componentData.m_Purpose == Purpose.InDeathcare);
    }

    public static bool IsCorpsePickedByHearse(EntityManager entityManager, Entity citizen)
    {
      TravelPurpose component;
      return CitizenUtils.IsDead(entityManager, citizen) && entityManager.TryGetComponent<TravelPurpose>(citizen, out component) && (component.m_Purpose == Purpose.Deathcare || component.m_Purpose == Purpose.InDeathcare);
    }

    public static bool TryGetResident(
      Entity entity,
      ComponentLookup<Citizen> citizenFromEntity,
      out Citizen citizen)
    {
      return citizenFromEntity.TryGetComponent(entity, out citizen) && (citizen.m_State & (CitizenFlags.MovingAwayReachOC | CitizenFlags.Tourist | CitizenFlags.Commuter)) == CitizenFlags.None;
    }

    public static PathfindWeights GetPathfindWeights(
      Citizen citizen,
      Household household,
      int householdCitizens)
    {
      double time = 5.0 * (4.0 - 3.75 * (double) citizen.m_LeisureCounter / (double) byte.MaxValue);
      float num1 = 2f;
      float a = 2500f * math.max(1f, (float) householdCitizens) / (float) math.max(250, (int) household.m_ConsumptionPerDay);
      float num2 = (float) (1.0 + 2.0 * (double) citizen.GetPseudoRandom(CitizenPseudoRandom.TrafficComfort).NextFloat());
      float num3 = math.select(a, a * 0.1f, (household.m_Flags & HouseholdFlags.MovedIn) == HouseholdFlags.None && (citizen.m_State & (CitizenFlags.MovingAwayReachOC | CitizenFlags.Tourist | CitizenFlags.Commuter)) == CitizenFlags.None);
      double behaviour = (double) num1;
      double money = (double) num3;
      double comfort = (double) num2;
      return new PathfindWeights((float) time, (float) behaviour, (float) money, (float) comfort);
    }

    public static bool IsCommuter(Entity citizenEntity, ref ComponentLookup<Citizen> citizens)
    {
      return (citizens[citizenEntity].m_State & CitizenFlags.Commuter) != 0;
    }

    public static bool IsResident(EntityManager entityManager, Entity entity, out Citizen citizen)
    {
      HouseholdMember component;
      return entityManager.TryGetComponent<Citizen>(entity, out citizen) && entityManager.TryGetComponent<HouseholdMember>(entity, out component) && !entityManager.HasComponent<TouristHousehold>(component.m_Household) && !entityManager.HasComponent<CommuterHousehold>(component.m_Household) && !entityManager.HasComponent<MovingAway>(component.m_Household) && (citizen.m_State & (CitizenFlags.MovingAwayReachOC | CitizenFlags.Tourist | CitizenFlags.Commuter)) == CitizenFlags.None;
    }

    public static bool IsResident(
      Entity entity,
      Citizen citizen,
      ComponentLookup<HouseholdMember> householdMemberFromEntity,
      ComponentLookup<MovingAway> movingAwayFromEntity,
      ComponentLookup<TouristHousehold> touristHouseholdFromEntity,
      ComponentLookup<CommuterHousehold> commuterHouseholdFromEntity)
    {
      HouseholdMember componentData;
      return householdMemberFromEntity.TryGetComponent(entity, out componentData) && !touristHouseholdFromEntity.HasComponent(componentData.m_Household) && !commuterHouseholdFromEntity.HasComponent(componentData.m_Household) && !movingAwayFromEntity.HasComponent(componentData.m_Household) && (citizen.m_State & (CitizenFlags.MovingAwayReachOC | CitizenFlags.Tourist | CitizenFlags.Commuter)) == CitizenFlags.None;
    }

    public static bool HasMovedIn(Entity householdEntity, ComponentLookup<Household> householdDatas)
    {
      Household componentData;
      return householdDatas.TryGetComponent(householdEntity, out componentData) && (componentData.m_Flags & HouseholdFlags.MovedIn) != 0;
    }

    public static bool HasMovedIn(
      Entity citizen,
      ref ComponentLookup<HouseholdMember> householdMembers,
      ref ComponentLookup<Household> households,
      ref ComponentLookup<HomelessHousehold> homelessHouseholds)
    {
      HouseholdMember componentData1;
      Household componentData2;
      return householdMembers.TryGetComponent(citizen, out componentData1) && households.TryGetComponent(componentData1.m_Household, out componentData2) && (componentData2.m_Flags & HouseholdFlags.MovedIn) != HouseholdFlags.None && !homelessHouseholds.HasComponent(componentData1.m_Household);
    }

    public static CitizenHappiness GetHappinessKey(int happiness)
    {
      if (happiness > 70)
        return CitizenHappiness.Happy;
      if (happiness > 55)
        return CitizenHappiness.Content;
      if (happiness > 40)
        return CitizenHappiness.Neutral;
      return happiness > 25 ? CitizenHappiness.Sad : CitizenHappiness.Depressed;
    }

    public static Entity GetCitizenSelectedSound(
      EntityManager entityManager,
      Entity entity,
      Citizen citizen,
      Entity citizenPrefabRef)
    {
      if (!entityManager.HasComponent<CitizenSelectedSoundData>(citizenPrefabRef))
        return Entity.Null;
      CitizenHappiness happinessKey = CitizenUtils.GetHappinessKey(citizen.Happiness);
      bool isSickOrInjured = false;
      HealthProblem component;
      if (entityManager.TryGetComponent<HealthProblem>(entity, out component) && (component.m_Flags & (HealthProblemFlags.Sick | HealthProblemFlags.Injured)) != HealthProblemFlags.None)
        isSickOrInjured = true;
      DynamicBuffer<CitizenSelectedSoundData> buffer = entityManager.GetBuffer<CitizenSelectedSoundData>(citizenPrefabRef, true);
      for (int index = 0; index < buffer.Length; ++index)
      {
        if (buffer[index].Equals(new CitizenSelectedSoundData(isSickOrInjured, citizen.GetAge(), happinessKey, Entity.Null)))
          return buffer[index].m_SelectedSound;
      }
      return Entity.Null;
    }

    public static bool IsHouseholdNeedSupport(
      DynamicBuffer<HouseholdCitizen> householdCitizens,
      ref ComponentLookup<Citizen> citizens,
      ref ComponentLookup<Student> students)
    {
      bool flag = true;
      for (int index = 0; index < householdCitizens.Length; ++index)
      {
        Entity citizen = householdCitizens[index].m_Citizen;
        if (citizens[citizen].GetAge() == CitizenAge.Adult && !students.HasComponent(citizen))
        {
          flag = false;
          break;
        }
      }
      return flag;
    }

    public static bool IsWorkableCitizen(
      Entity citizenEntity,
      ref ComponentLookup<Citizen> citizens,
      ref ComponentLookup<Student> m_Students,
      ref ComponentLookup<HealthProblem> healthProblems)
    {
      if ((!healthProblems.HasComponent(citizenEntity) || !CitizenUtils.IsDead(healthProblems[citizenEntity])) && !m_Students.HasComponent(citizenEntity) && (citizens[citizenEntity].m_State & (CitizenFlags.Tourist | CitizenFlags.Commuter)) == CitizenFlags.None)
      {
        Citizen citizen = citizens[citizenEntity];
        if (citizen.GetAge() != CitizenAge.Teen)
        {
          citizen = citizens[citizenEntity];
          if (citizen.GetAge() != CitizenAge.Adult)
            goto label_4;
        }
        return true;
      }
label_4:
      return false;
    }
  }
}
