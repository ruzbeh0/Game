// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MaintenanceDepot
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Simulation;
using Game.Vehicles;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/CityServices/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class MaintenanceDepot : ComponentBase, IServiceUpgrade
  {
    public MaintenanceType m_MaintenanceType = MaintenanceType.Park;
    public int m_VehicleCapacity = 10;
    public float m_VehicleEfficiency = 1f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<MaintenanceDepotData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.MaintenanceDepot>());
      if ((this.m_MaintenanceType & MaintenanceType.Park) != MaintenanceType.None)
        components.Add(ComponentType.ReadWrite<ParkMaintenance>());
      if ((this.m_MaintenanceType & (MaintenanceType.Road | MaintenanceType.Snow | MaintenanceType.Vehicle)) != MaintenanceType.None)
        components.Add(ComponentType.ReadWrite<RoadMaintenance>());
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null)
        components.Add(ComponentType.ReadWrite<Efficiency>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.MaintenanceDepot>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      MaintenanceDepotData componentData;
      componentData.m_MaintenanceType = this.m_MaintenanceType;
      componentData.m_VehicleCapacity = this.m_VehicleCapacity;
      componentData.m_VehicleEfficiency = this.m_VehicleEfficiency;
      entityManager.SetComponentData<MaintenanceDepotData>(entity, componentData);
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(10));
    }
  }
}
