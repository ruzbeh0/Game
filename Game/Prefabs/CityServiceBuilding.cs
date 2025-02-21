// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CityServiceBuilding
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Citizens;
using Game.City;
using Game.Economy;
using Game.Simulation;
using Game.Vehicles;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class CityServiceBuilding : ComponentBase, IServiceUpgrade
  {
    public ServiceUpkeepItem[] m_Upkeeps;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      ServiceConsumption component;
      if (this.m_Upkeeps != null && this.m_Upkeeps.Length != 0 || this.prefab.TryGet<ServiceConsumption>(out component) && component.m_Upkeep > 0)
        components.Add(ComponentType.ReadWrite<ServiceUpkeepData>());
      components.Add(ComponentType.ReadWrite<CollectedServiceBuildingBudgetData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<CityServiceUpkeep>());
      components.Add(ComponentType.ReadWrite<Game.Economy.Resources>());
      components.Add(ComponentType.ReadWrite<TripNeeded>());
      components.Add(ComponentType.ReadWrite<GuestVehicle>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<CityServiceUpkeep>());
      components.Add(ComponentType.ReadWrite<Game.Economy.Resources>());
      components.Add(ComponentType.ReadWrite<TripNeeded>());
      components.Add(ComponentType.ReadWrite<GuestVehicle>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      DynamicBuffer<ServiceUpkeepData> dynamicBuffer = entityManager.AddBuffer<ServiceUpkeepData>(entity);
      if (this.m_Upkeeps == null)
        return;
      foreach (ServiceUpkeepItem upkeep in this.m_Upkeeps)
        dynamicBuffer.Add(new ServiceUpkeepData()
        {
          m_Upkeep = new ResourceStack()
          {
            m_Resource = EconomyUtils.GetResource(upkeep.m_Resources.m_Resource),
            m_Amount = upkeep.m_Resources.m_Amount
          },
          m_ScaleWithUsage = upkeep.m_ScaleWithUsage
        });
    }
  }
}
