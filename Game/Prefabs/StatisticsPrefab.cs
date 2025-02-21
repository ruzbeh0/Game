// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.StatisticsPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Statistics/", new System.Type[] {})]
  public class StatisticsPrefab : ArchetypePrefab
  {
    public UIStatisticsCategoryPrefab m_Category;
    public UIStatisticsGroupPrefab m_Group;
    public StatisticType m_StatisticsType;
    public StatisticCollectionType m_CollectionType;
    public StatisticUnitType m_UnitType;
    public Color m_Color = Color.grey;
    public bool m_Stacked = true;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<StatisticsData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<CityStatistic>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      Entity entity1 = (UnityEngine.Object) this.m_Category != (UnityEngine.Object) null ? existingSystemManaged.GetEntity((PrefabBase) this.m_Category) : Entity.Null;
      // ISSUE: reference to a compiler-generated method
      Entity entity2 = (UnityEngine.Object) this.m_Group != (UnityEngine.Object) null ? existingSystemManaged.GetEntity((PrefabBase) this.m_Group) : Entity.Null;
      StatisticsData componentData = new StatisticsData()
      {
        m_Group = entity2,
        m_Category = entity1,
        m_CollectionType = this.m_CollectionType,
        m_StatisticType = this.m_StatisticsType,
        m_UnitType = this.m_UnitType,
        m_Color = this.m_Color,
        m_Stacked = this.m_Stacked
      };
      entityManager.SetComponentData<StatisticsData>(entity, componentData);
    }

    public static Entity CreateInstance(
      EntityManager entityManager,
      Entity entity,
      ArchetypeData archetypeData,
      int parameter = 0)
    {
      Entity entity1 = entityManager.CreateEntity(archetypeData.m_Archetype);
      PrefabRef componentData = new PrefabRef()
      {
        m_Prefab = entity
      };
      entityManager.AddComponentData<PrefabRef>(entity1, componentData);
      if (entityManager.HasComponent<StatisticParameter>(entity1))
        entityManager.SetComponentData<StatisticParameter>(entity1, new StatisticParameter()
        {
          m_Value = parameter
        });
      return entity1;
    }
  }
}
