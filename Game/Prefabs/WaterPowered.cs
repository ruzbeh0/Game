// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WaterPowered
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [RequireComponent(typeof (PowerPlant))]
  [ComponentMenu("Buildings/CityServices/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class WaterPowered : ComponentBase, IServiceUpgrade
  {
    public float m_ProductionFactor;
    public float m_CapacityFactor;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<WaterPoweredData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<Game.Buildings.WaterPowered>());
      components.Add(ComponentType.ReadWrite<Efficiency>());
      components.Add(ComponentType.ReadWrite<RenewableElectricityProduction>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.WaterPowered>());
      components.Add(ComponentType.ReadWrite<Efficiency>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      PowerPlant component;
      if (this.prefab.TryGet<PowerPlant>(out component) && component.m_ElectricityProduction != 0)
        Debug.LogErrorFormat((UnityEngine.Object) this.prefab, "WaterPowered has non-zero electricity production: {0}", (object) this.prefab.name);
      entityManager.SetComponentData<WaterPoweredData>(entity, new WaterPoweredData()
      {
        m_ProductionFactor = this.m_ProductionFactor,
        m_CapacityFactor = this.m_CapacityFactor
      });
    }
  }
}
