// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TransportDepot
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
  public class TransportDepot : ComponentBase, IServiceUpgrade
  {
    public TransportType m_TransportType;
    public EnergyTypes m_EnergyTypes = EnergyTypes.Fuel;
    public int m_VehicleCapacity = 10;
    public float m_ProductionDuration;
    public float m_MaintenanceDuration;
    public bool m_DispatchCenter;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<TransportDepotData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.TransportDepot>());
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null)
        components.Add(ComponentType.ReadWrite<Efficiency>());
      if (this.m_TransportType == TransportType.Taxi)
        components.Add(ComponentType.ReadWrite<ServiceDistrict>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.TransportDepot>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      TransportDepotData componentData;
      componentData.m_TransportType = this.m_TransportType;
      componentData.m_DispatchCenter = this.m_DispatchCenter;
      componentData.m_EnergyTypes = this.m_EnergyTypes;
      componentData.m_VehicleCapacity = this.m_VehicleCapacity;
      componentData.m_ProductionDuration = this.m_ProductionDuration;
      componentData.m_MaintenanceDuration = this.m_MaintenanceDuration;
      entityManager.SetComponentData<TransportDepotData>(entity, componentData);
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(2));
    }
  }
}
