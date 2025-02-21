// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ResourceConsumption
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Citizens;
using Game.City;
using Game.Economy;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class ResourceConsumption : ComponentBase
  {
    public ResourceConsumptionItem[] m_Consumptions;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ResourceConsumptionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<CityServiceUpkeep>());
      components.Add(ComponentType.ReadWrite<Resources>());
      components.Add(ComponentType.ReadWrite<TripNeeded>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      DynamicBuffer<ResourceConsumptionData> dynamicBuffer = entityManager.AddBuffer<ResourceConsumptionData>(entity);
      if (this.m_Consumptions == null)
        return;
      foreach (ResourceConsumptionItem consumption in this.m_Consumptions)
        dynamicBuffer.Add(new ResourceConsumptionData()
        {
          m_Consumption = new ResourceStack()
          {
            m_Resource = EconomyUtils.GetResource(consumption.m_Consumption.m_Resource),
            m_Amount = consumption.m_Consumption.m_Amount
          },
          m_ScaleWithUsage = consumption.m_ScaleWithUsage
        });
    }
  }
}
