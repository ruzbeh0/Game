// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PoliceStation
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
  public class PoliceStation : ComponentBase, IServiceUpgrade
  {
    public int m_PatrolCarCapacity = 10;
    public int m_PoliceHelicopterCapacity;
    public int m_JailCapacity = 15;
    [EnumFlag]
    public PolicePurpose m_Purposes = PolicePurpose.Patrol | PolicePurpose.Emergency;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PoliceStationData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.PoliceStation>());
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null)
        components.Add(ComponentType.ReadWrite<Efficiency>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      if ((UnityEngine.Object) this.GetComponent<UniqueObject>() == (UnityEngine.Object) null)
        components.Add(ComponentType.ReadWrite<ServiceDistrict>());
      if (this.m_JailCapacity == 0)
        return;
      components.Add(ComponentType.ReadWrite<Occupant>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.PoliceStation>());
      components.Add(ComponentType.ReadWrite<ServiceDispatch>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      if (this.m_JailCapacity == 0)
        return;
      components.Add(ComponentType.ReadWrite<Occupant>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      PoliceStationData componentData;
      componentData.m_PatrolCarCapacity = this.m_PatrolCarCapacity;
      componentData.m_PoliceHelicopterCapacity = this.m_PoliceHelicopterCapacity;
      componentData.m_JailCapacity = this.m_JailCapacity;
      componentData.m_PurposeMask = this.m_Purposes;
      entityManager.SetComponentData<PoliceStationData>(entity, componentData);
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(8));
    }
  }
}
