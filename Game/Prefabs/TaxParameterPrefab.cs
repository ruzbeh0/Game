// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TaxParameterPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new Type[] {})]
  public class TaxParameterPrefab : PrefabBase
  {
    public int2 m_TotalTaxLimits;
    public int2 m_ResidentialTaxLimits;
    public int2 m_CommercialTaxLimits;
    public int2 m_IndustrialTaxLimits;
    public int2 m_OfficeTaxLimits;
    public int2 m_JobLevelTaxLimits;
    public int2 m_ResourceTaxLimits;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<TaxParameterData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      entityManager.SetComponentData<TaxParameterData>(entity, new TaxParameterData()
      {
        m_TotalTaxLimits = this.m_TotalTaxLimits,
        m_ResidentialTaxLimits = this.m_ResidentialTaxLimits,
        m_CommercialTaxLimits = this.m_CommercialTaxLimits,
        m_IndustrialTaxLimits = this.m_IndustrialTaxLimits,
        m_OfficeTaxLimits = this.m_OfficeTaxLimits,
        m_JobLevelTaxLimits = this.m_JobLevelTaxLimits,
        m_ResourceTaxLimits = this.m_ResourceTaxLimits
      });
    }
  }
}
