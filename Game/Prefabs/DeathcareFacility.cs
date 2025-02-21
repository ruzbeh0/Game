// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.DeathcareFacility
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
  [ComponentMenu("Buildings/CityServices/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab), typeof (MarkerObjectPrefab)})]
  public class DeathcareFacility : ComponentBase, IServiceUpgrade
  {
    public int m_HearseCapacity = 5;
    public int m_StorageCapacity = 100;
    public float m_ProcessingRate = 10f;
    public bool m_LongTermStorage;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<DeathcareFacilityData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.DeathcareFacility>());
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null)
        components.Add(ComponentType.ReadWrite<Efficiency>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      components.Add(ComponentType.ReadWrite<ServiceDistrict>());
      if (this.m_StorageCapacity == 0)
        return;
      components.Add(ComponentType.ReadWrite<Patient>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.DeathcareFacility>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      if (this.m_StorageCapacity == 0)
        return;
      components.Add(ComponentType.ReadWrite<Patient>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<DeathcareFacilityData>(entity, new DeathcareFacilityData()
      {
        m_HearseCapacity = this.m_HearseCapacity,
        m_StorageCapacity = this.m_StorageCapacity,
        m_LongTermStorage = this.m_LongTermStorage,
        m_ProcessingRate = this.m_ProcessingRate
      });
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(2));
    }
  }
}
