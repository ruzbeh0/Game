// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CompanyNotificationParameterPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class CompanyNotificationParameterPrefab : PrefabBase
  {
    public NotificationIconPrefab m_NoInputsNotificationPrefab;
    public NotificationIconPrefab m_NoCustomersNotificationPrefab;
    public float m_NoInputCostLimit = 5f;
    public float m_NoCustomersServiceLimit = 0.9f;
    [Tooltip("The limit of empty rooms percentage of total room amount, 0.9 means 90% rooms are empty")]
    public float m_NoCustomersHotelLimit = 0.9f;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_NoInputsNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_NoCustomersNotificationPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<CompanyNotificationParameterData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      CompanyNotificationParameterData componentData;
      // ISSUE: reference to a compiler-generated method
      componentData.m_NoCustomersNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_NoCustomersNotificationPrefab);
      // ISSUE: reference to a compiler-generated method
      componentData.m_NoInputsNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_NoInputsNotificationPrefab);
      componentData.m_NoCustomersServiceLimit = this.m_NoCustomersServiceLimit;
      componentData.m_NoInputCostLimit = this.m_NoInputCostLimit;
      componentData.m_NoCustomersHotelLimit = this.m_NoCustomersHotelLimit;
      entityManager.SetComponentData<CompanyNotificationParameterData>(entity, componentData);
    }
  }
}
