// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CompanyPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Citizens;
using Game.Companies;
using Game.Economy;
using Game.Simulation;
using Game.Zones;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Companies/", new Type[] {})]
  public class CompanyPrefab : ArchetypePrefab
  {
    public AreaType zone;
    public float profitability;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      if (this.zone == AreaType.Commercial)
        components.Add(ComponentType.ReadWrite<CommercialCompanyData>());
      else if (this.zone == AreaType.Industrial)
        components.Add(ComponentType.ReadWrite<IndustrialCompanyData>());
      components.Add(ComponentType.ReadWrite<CompanyBrandElement>());
      components.Add(ComponentType.ReadWrite<AffiliatedBrandElement>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<CompanyData>());
      components.Add(ComponentType.ReadWrite<UpdateFrame>());
      components.Add(ComponentType.ReadWrite<Resources>());
      components.Add(ComponentType.ReadWrite<PropertySeeker>());
      components.Add(ComponentType.ReadWrite<TripNeeded>());
      components.Add(ComponentType.ReadWrite<CompanyNotifications>());
      if (this.zone == AreaType.Commercial)
      {
        components.Add(ComponentType.ReadWrite<CommercialCompany>());
      }
      else
      {
        if (this.zone != AreaType.Industrial)
          return;
        components.Add(ComponentType.ReadWrite<IndustrialCompany>());
      }
    }
  }
}
