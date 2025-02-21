// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WaterPumpingStation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/CityServices/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class WaterPumpingStation : ComponentBase, IServiceUpgrade
  {
    public int m_Capacity = 75;
    public float m_Purification;
    [EnumFlag]
    public AllowedWaterTypes m_AllowedWaterTypes;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<WaterPumpingStationData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<Game.Buildings.WaterPumpingStation>());
      if (!((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<Efficiency>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.WaterPumpingStation>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<WaterPumpingStationData>(entity, new WaterPumpingStationData()
      {
        m_Capacity = this.m_Capacity,
        m_Types = this.m_AllowedWaterTypes,
        m_Purification = this.m_Purification
      });
    }
  }
}
