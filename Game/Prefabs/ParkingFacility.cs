// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ParkingFacility
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
  public class ParkingFacility : ComponentBase, IServiceUpgrade
  {
    public float m_ComfortFactor = 0.5f;
    public int m_GarageMarkerCapacity;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ParkingFacilityData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.ParkingFacility>());
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null) || !((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<Efficiency>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.ParkingFacility>());
      components.Add(ComponentType.ReadWrite<Efficiency>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<ParkingFacilityData>(entity, new ParkingFacilityData()
      {
        m_ComfortFactor = this.m_ComfortFactor,
        m_GarageMarkerCapacity = this.m_GarageMarkerCapacity
      });
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(12));
    }
  }
}
