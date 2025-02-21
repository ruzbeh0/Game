// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ServicePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Simulation;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Services/", new System.Type[] {})]
  public class ServicePrefab : PrefabBase
  {
    [SerializeField]
    private PlayerResource[] m_CityResources;
    [SerializeField]
    private CityService m_Service;
    [SerializeField]
    private bool m_BudgetAdjustable = true;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ServiceData>());
      components.Add(ComponentType.ReadWrite<CollectedCityServiceBudgetData>());
      components.Add(ComponentType.ReadWrite<CollectedCityServiceUpkeepData>());
      if (this.m_CityResources == null || this.m_CityResources.Length == 0)
        return;
      components.Add(ComponentType.ReadWrite<CollectedCityServiceFeeData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      if (this.m_CityResources != null && this.m_CityResources.Length != 0)
      {
        DynamicBuffer<CollectedCityServiceFeeData> buffer = entityManager.GetBuffer<CollectedCityServiceFeeData>(entity);
        for (int index = 0; index < this.m_CityResources.Length; ++index)
          buffer.Add(new CollectedCityServiceFeeData()
          {
            m_PlayerResource = (int) this.m_CityResources[index]
          });
      }
      entityManager.SetComponentData<ServiceData>(entity, new ServiceData()
      {
        m_Service = this.m_Service,
        m_BudgetAdjustable = this.m_BudgetAdjustable
      });
    }
  }
}
