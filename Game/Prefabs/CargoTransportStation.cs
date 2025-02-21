// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CargoTransportStation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Companies;
using Game.Economy;
using Game.Vehicles;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/CityServices/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  [RequireComponent(typeof (StorageLimit))]
  public class CargoTransportStation : ComponentBase, IServiceUpgrade
  {
    public ResourceInEditor[] m_TradedResources;
    public int transports;
    public EnergyTypes m_CarRefuelTypes;
    public EnergyTypes m_TrainRefuelTypes;
    public EnergyTypes m_WatercraftRefuelTypes;
    public EnergyTypes m_AircraftRefuelTypes;
    public float m_LoadingFactor;
    public int2 m_TransportInterval;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<TransportStationData>());
      components.Add(ComponentType.ReadWrite<CargoTransportStationData>());
      components.Add(ComponentType.ReadWrite<StorageCompanyData>());
      components.Add(ComponentType.ReadWrite<TransportCompanyData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.TransportStation>());
      components.Add(ComponentType.ReadWrite<Game.Buildings.CargoTransportStation>());
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null)
        components.Add(ComponentType.ReadWrite<Efficiency>());
      components.Add(ComponentType.ReadWrite<Game.Companies.StorageCompany>());
      components.Add(ComponentType.ReadWrite<TradeCost>());
      components.Add(ComponentType.ReadWrite<StorageTransferRequest>());
      components.Add(ComponentType.ReadWrite<Game.Economy.Resources>());
      if (this.transports <= 0)
        return;
      components.Add(ComponentType.ReadWrite<TransportCompany>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.TransportStation>());
      components.Add(ComponentType.ReadWrite<Game.Buildings.CargoTransportStation>());
      components.Add(ComponentType.ReadWrite<Game.Companies.StorageCompany>());
      components.Add(ComponentType.ReadWrite<TradeCost>());
      components.Add(ComponentType.ReadWrite<StorageTransferRequest>());
      components.Add(ComponentType.ReadWrite<Game.Economy.Resources>());
      if (this.transports <= 0)
        return;
      components.Add(ComponentType.ReadWrite<TransportCompany>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      StorageCompanyData componentData1 = new StorageCompanyData()
      {
        m_StoredResources = Resource.NoResource
      };
      if (this.m_TradedResources != null && this.m_TradedResources.Length != 0)
      {
        for (int index = 0; index < this.m_TradedResources.Length; ++index)
        {
          componentData1.m_StoredResources |= EconomyUtils.GetResource(this.m_TradedResources[index]);
          componentData1.m_TransportInterval = this.m_TransportInterval;
        }
      }
      entityManager.SetComponentData<StorageCompanyData>(entity, componentData1);
      if (this.transports > 0)
        entityManager.SetComponentData<TransportCompanyData>(entity, new TransportCompanyData()
        {
          m_MaxTransports = this.transports
        });
      TransportStationData componentData2 = entityManager.GetComponentData<TransportStationData>(entity);
      componentData2.m_CarRefuelTypes |= this.m_CarRefuelTypes;
      componentData2.m_TrainRefuelTypes |= this.m_TrainRefuelTypes;
      componentData2.m_WatercraftRefuelTypes |= this.m_WatercraftRefuelTypes;
      componentData2.m_AircraftRefuelTypes |= this.m_AircraftRefuelTypes;
      componentData2.m_LoadingFactor = this.m_LoadingFactor;
      entityManager.SetComponentData<TransportStationData>(entity, componentData2);
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(0));
    }
  }
}
