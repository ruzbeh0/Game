// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Hospital
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Buildings;
using Game.Simulation;
using Game.Vehicles;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/CityServices/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab), typeof (MarkerObjectPrefab)})]
  public class Hospital : ComponentBase, IServiceUpgrade
  {
    public int m_AmbulanceCapacity = 10;
    public int m_MedicalHelicopterCapacity;
    public int m_PatientCapacity = 10;
    public int m_TreatmentBonus = 3;
    public int2 m_HealthRange = new int2(0, 100);
    public bool m_TreatDiseases = true;
    public bool m_TreatInjuries = true;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<HospitalData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<Game.Buildings.Hospital>());
      if ((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null)
      {
        components.Add(ComponentType.ReadWrite<Efficiency>());
        components.Add(ComponentType.ReadWrite<ServiceUsage>());
      }
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      if ((UnityEngine.Object) this.GetComponent<UniqueObject>() == (UnityEngine.Object) null)
        components.Add(ComponentType.ReadWrite<ServiceDistrict>());
      if (this.m_PatientCapacity == 0)
        return;
      components.Add(ComponentType.ReadWrite<Patient>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.Hospital>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      components.Add(ComponentType.ReadWrite<ServiceUsage>());
      if (this.m_PatientCapacity == 0)
        return;
      components.Add(ComponentType.ReadWrite<Patient>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<HospitalData>(entity, new HospitalData()
      {
        m_AmbulanceCapacity = this.m_AmbulanceCapacity,
        m_MedicalHelicopterCapacity = this.m_MedicalHelicopterCapacity,
        m_PatientCapacity = this.m_PatientCapacity,
        m_TreatmentBonus = this.m_TreatmentBonus,
        m_HealthRange = this.m_HealthRange,
        m_TreatDiseases = this.m_TreatDiseases,
        m_TreatInjuries = this.m_TreatInjuries
      });
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(1));
    }
  }
}
