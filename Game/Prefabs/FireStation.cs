// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.FireStation
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
  public class FireStation : ComponentBase, IServiceUpgrade
  {
    public int m_FireEngineCapacity = 3;
    public int m_FireHelicopterCapacity;
    public int m_DisasterResponseCapacity;
    public float m_VehicleEfficiency = 1f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<FireStationData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.FireStation>());
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null)
        components.Add(ComponentType.ReadWrite<Efficiency>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      if (!((UnityEngine.Object) this.GetComponent<UniqueObject>() == (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<ServiceDistrict>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.FireStation>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<FireStationData>(entity, new FireStationData()
      {
        m_FireEngineCapacity = this.m_FireEngineCapacity,
        m_FireHelicopterCapacity = this.m_FireHelicopterCapacity,
        m_DisasterResponseCapacity = this.m_DisasterResponseCapacity,
        m_VehicleEfficiency = this.m_VehicleEfficiency
      });
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(7));
    }
  }
}
