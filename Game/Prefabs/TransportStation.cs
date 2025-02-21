// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TransportStation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Vehicles;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/CityServices/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class TransportStation : ComponentBase, IServiceUpgrade
  {
    public EnergyTypes m_CarRefuelTypes;
    public EnergyTypes m_TrainRefuelTypes;
    public EnergyTypes m_WatercraftRefuelTypes;
    public EnergyTypes m_AircraftRefuelTypes;
    public float m_ComfortFactor;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<TransportStationData>());
      components.Add(ComponentType.ReadWrite<PublicTransportStationData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.TransportStation>());
      components.Add(ComponentType.ReadWrite<PublicTransportStation>());
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null) || !((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<Efficiency>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.TransportStation>());
      components.Add(ComponentType.ReadWrite<PublicTransportStation>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      TransportStationData componentData = entityManager.GetComponentData<TransportStationData>(entity);
      componentData.m_CarRefuelTypes |= this.m_CarRefuelTypes;
      componentData.m_TrainRefuelTypes |= this.m_TrainRefuelTypes;
      componentData.m_WatercraftRefuelTypes |= this.m_WatercraftRefuelTypes;
      componentData.m_AircraftRefuelTypes |= this.m_AircraftRefuelTypes;
      componentData.m_ComfortFactor = this.m_ComfortFactor;
      entityManager.SetComponentData<TransportStationData>(entity, componentData);
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(0));
    }
  }
}
