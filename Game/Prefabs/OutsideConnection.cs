// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.OutsideConnection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Citizens;
using Game.Companies;
using Game.Economy;
using Game.Vehicles;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {typeof (StaticObjectPrefab), typeof (MarkerObjectPrefab)})]
  public class OutsideConnection : ComponentBase
  {
    public ResourceInEditor[] m_TradedResources;
    public bool m_Commuting;
    public OutsideConnectionTransferType m_TransferType;
    public float m_Remoteness;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<OutsideConnectionData>());
      components.Add(ComponentType.ReadWrite<StorageCompanyData>());
      components.Add(ComponentType.ReadWrite<TransportCompanyData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Objects.OutsideConnection>());
      components.Add(ComponentType.ReadWrite<Resources>());
      components.Add(ComponentType.ReadWrite<Game.Companies.StorageCompany>());
      components.Add(ComponentType.ReadWrite<TradeCost>());
      components.Add(ComponentType.ReadWrite<StorageTransferRequest>());
      components.Add(ComponentType.ReadWrite<TripNeeded>());
      components.Add(ComponentType.ReadWrite<ResourceSeller>());
      components.Add(ComponentType.ReadWrite<TransportCompany>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<OutsideConnectionData>(entity, new OutsideConnectionData()
      {
        m_Type = this.m_TransferType,
        m_Remoteness = this.m_Remoteness
      });
      StorageCompanyData componentData = new StorageCompanyData()
      {
        m_StoredResources = Resource.NoResource
      };
      if (this.m_TradedResources != null && this.m_TradedResources.Length != 0)
      {
        for (int index = 0; index < this.m_TradedResources.Length; ++index)
          componentData.m_StoredResources |= EconomyUtils.GetResource(this.m_TradedResources[index]);
      }
      entityManager.SetComponentData<StorageCompanyData>(entity, componentData);
      entityManager.SetComponentData<TransportCompanyData>(entity, new TransportCompanyData()
      {
        m_MaxTransports = int.MaxValue
      });
    }
  }
}
