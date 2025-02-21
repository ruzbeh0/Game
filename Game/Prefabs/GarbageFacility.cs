// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.GarbageFacility
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
  public class GarbageFacility : ComponentBase, IServiceUpgrade
  {
    public int m_GarbageCapacity = 100000;
    public int m_VehicleCapacity = 10;
    public int m_TransportCapacity;
    public int m_ProcessingSpeed;
    public bool m_IndustrialWasteOnly;
    public bool m_LongTermStorage;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<GarbageFacilityData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.GarbageFacility>());
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<Game.Economy.Resources>());
      if ((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null)
      {
        components.Add(ComponentType.ReadWrite<GuestVehicle>());
        components.Add(ComponentType.ReadWrite<Efficiency>());
      }
      if (this.m_VehicleCapacity > 0 || this.m_TransportCapacity > 0)
      {
        components.Add(ComponentType.ReadWrite<ServiceDispatch>());
        components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      }
      if (!((UnityEngine.Object) this.GetComponent<UniqueObject>() == (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<ServiceDistrict>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.GarbageFacility>());
      components.Add(ComponentType.ReadWrite<Game.Economy.Resources>());
      if (this.m_VehicleCapacity <= 0 && this.m_TransportCapacity <= 0)
        return;
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      GarbageFacilityData componentData;
      componentData.m_GarbageCapacity = this.m_GarbageCapacity;
      componentData.m_VehicleCapacity = this.m_VehicleCapacity;
      componentData.m_TransportCapacity = this.m_TransportCapacity;
      componentData.m_ProcessingSpeed = this.m_ProcessingSpeed;
      componentData.m_IndustrialWasteOnly = this.m_IndustrialWasteOnly;
      componentData.m_LongTermStorage = this.m_LongTermStorage;
      entityManager.SetComponentData<GarbageFacilityData>(entity, componentData);
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(5));
    }
  }
}
