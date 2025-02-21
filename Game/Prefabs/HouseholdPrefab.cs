// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.HouseholdPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Citizens;
using Game.Simulation;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Citizens/", new System.Type[] {})]
  public class HouseholdPrefab : ArchetypePrefab
  {
    public int m_ResourceConsumption;
    public int m_InitialWealthRange;
    public int m_InitialWealthOffset;
    [Tooltip("Percentage chance of arriving with a car")]
    public int m_InitialCarProbability;
    public int m_ChildCount;
    public int m_AdultCount;
    public int m_ElderlyCount;
    [Tooltip("Guaranteed to be in college/uni age and have education level 2")]
    public int m_StudentCount;
    public int m_FirstPetProbability = 20;
    public int m_NextPetProbability = 10;
    [Tooltip("Is this prefab only for households that are created when kids move out of home or people divorce, which do not need random citizens")]
    public bool m_DynamicHousehold;
    public int m_Weight = 1;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<HouseholdData>());
      if (!this.m_DynamicHousehold)
        return;
      components.Add(ComponentType.ReadWrite<DynamicHousehold>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Household>());
      components.Add(ComponentType.ReadWrite<HouseholdNeed>());
      components.Add(ComponentType.ReadWrite<HouseholdCitizen>());
      components.Add(ComponentType.ReadWrite<TaxPayer>());
      components.Add(ComponentType.ReadWrite<Game.Economy.Resources>());
      components.Add(ComponentType.ReadWrite<UpdateFrame>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<HouseholdData>(entity, new HouseholdData()
      {
        m_InitialCarProbability = this.m_InitialCarProbability,
        m_InitialWealthOffset = this.m_InitialWealthOffset,
        m_InitialWealthRange = this.m_InitialWealthRange,
        m_ChildCount = this.m_ChildCount,
        m_AdultCount = this.m_AdultCount,
        m_ElderCount = this.m_ElderlyCount,
        m_StudentCount = this.m_StudentCount,
        m_FirstPetProbability = this.m_FirstPetProbability,
        m_NextPetProbability = this.m_NextPetProbability,
        m_Weight = this.m_Weight
      });
    }
  }
}
