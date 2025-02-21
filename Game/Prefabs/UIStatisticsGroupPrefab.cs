// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UIStatisticsGroupPrefab
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
  [ComponentMenu("UI/", new System.Type[] {})]
  public class UIStatisticsGroupPrefab : UIGroupPrefab
  {
    public Color m_Color = Color.black;
    public UIStatisticsCategoryPrefab m_Category;
    public StatisticUnitType m_UnitType;
    public bool m_Stacked;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<UIStatisticsGroupData>());
    }

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (!((UnityEngine.Object) this.m_Category != (UnityEngine.Object) null))
        return;
      prefabs.Add((PrefabBase) this.m_Category);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      Entity entity1 = (UnityEngine.Object) this.m_Category != (UnityEngine.Object) null ? existingSystemManaged.GetEntity((PrefabBase) this.m_Category) : Entity.Null;
      entityManager.SetComponentData<UIStatisticsGroupData>(entity, new UIStatisticsGroupData()
      {
        m_Category = entity1,
        m_Color = this.m_Color,
        m_UnitType = this.m_UnitType,
        m_Stacked = this.m_Stacked
      });
    }
  }
}
