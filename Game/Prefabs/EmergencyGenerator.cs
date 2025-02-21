// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.EmergencyGenerator
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Buildings;
using System.Collections.Generic;
using Unity.Assertions;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/CityServices/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class EmergencyGenerator : ComponentBase, IServiceUpgrade
  {
    public int m_ElectricityProduction;
    [Tooltip("The emergency generator is activated when the charge drops below Min. It it disabled again when the charge reaches Max.")]
    public Bounds1 m_ActivationThreshold = new Bounds1(0.05f, 0.1f);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      Assert.IsNotNull<ServiceUpgrade>(this.GetComponent<ServiceUpgrade>(), "Only battery building service upgrades can function as emergency generators");
      components.Add(ComponentType.ReadWrite<EmergencyGeneratorData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ServiceUsage>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.EmergencyGenerator>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<EmergencyGeneratorData>(entity, new EmergencyGeneratorData()
      {
        m_ElectricityProduction = this.m_ElectricityProduction,
        m_ActivationThreshold = this.m_ActivationThreshold
      });
    }
  }
}
