// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.EmergencyShelter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Buildings;
using Game.Simulation;
using Game.Vehicles;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/CityServices/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class EmergencyShelter : ComponentBase, IServiceUpgrade
  {
    public int m_ShelterCapacity = 100;
    public int m_VehicleCapacity;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<EmergencyShelterData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.EmergencyShelter>());
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null)
      {
        components.Add(ComponentType.ReadWrite<Efficiency>());
        components.Add(ComponentType.ReadWrite<ServiceUsage>());
      }
      components.Add(ComponentType.ReadWrite<Occupant>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      components.Add(ComponentType.ReadWrite<Game.Economy.Resources>());
      if (!((UnityEngine.Object) this.GetComponent<UniqueObject>() == (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<ServiceDistrict>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.EmergencyShelter>());
      components.Add(ComponentType.ReadWrite<Occupant>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      components.Add(ComponentType.ReadWrite<Game.Economy.Resources>());
      components.Add(ComponentType.ReadWrite<ServiceUsage>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      EmergencyShelterData componentData;
      componentData.m_ShelterCapacity = this.m_ShelterCapacity;
      componentData.m_VehicleCapacity = this.m_VehicleCapacity;
      entityManager.SetComponentData<EmergencyShelterData>(entity, componentData);
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(15));
    }
  }
}
