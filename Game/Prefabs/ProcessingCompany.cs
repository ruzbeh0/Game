// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ProcessingCompany
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Buildings;
using Game.Companies;
using Game.Economy;
using Game.Vehicles;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Companies/", new Type[] {typeof (CompanyPrefab)})]
  public class ProcessingCompany : ComponentBase
  {
    public IndustrialProcess process;
    public int transports;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<IndustrialProcessData>());
      if (this.transports <= 0)
        return;
      components.Add(ComponentType.ReadWrite<TransportCompanyData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Companies.ProcessingCompany>());
      components.Add(ComponentType.ReadWrite<TaxPayer>());
      if (this.process.m_Input1.m_Resource != ResourceInEditor.NoResource || this.process.m_Input2.m_Resource != ResourceInEditor.NoResource)
        components.Add(ComponentType.ReadWrite<BuyingCompany>());
      if (this.process.m_Output.m_Resource == ResourceInEditor.Lodging)
      {
        components.Add(ComponentType.ReadWrite<LodgingProvider>());
        components.Add(ComponentType.ReadWrite<Renter>());
      }
      components.Add(ComponentType.ReadWrite<ResourceSeller>());
      components.Add(ComponentType.ReadWrite<Profitability>());
      components.Add(ComponentType.ReadWrite<TradeCost>());
      if (this.transports <= 0)
        return;
      components.Add(ComponentType.ReadWrite<TransportCompany>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<IndustrialProcessData>(entity, new IndustrialProcessData()
      {
        m_Input1 = {
          m_Amount = this.process.m_Input1.m_Amount,
          m_Resource = EconomyUtils.GetResource(this.process.m_Input1.m_Resource)
        },
        m_Input2 = {
          m_Amount = this.process.m_Input2.m_Amount,
          m_Resource = EconomyUtils.GetResource(this.process.m_Input2.m_Resource)
        },
        m_Output = {
          m_Amount = this.process.m_Output.m_Amount,
          m_Resource = EconomyUtils.GetResource(this.process.m_Output.m_Resource)
        },
        m_MaxWorkersPerCell = this.process.m_MaxWorkersPerCell
      });
      if (this.transports <= 0)
        return;
      entityManager.SetComponentData<TransportCompanyData>(entity, new TransportCompanyData()
      {
        m_MaxTransports = this.transports
      });
    }
  }
}
