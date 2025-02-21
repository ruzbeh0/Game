// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TaxParameterData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct TaxParameterData : IComponentData, IQueryTypeParameter, IEquatable<TaxParameterData>
  {
    public int2 m_TotalTaxLimits;
    public int2 m_ResidentialTaxLimits;
    public int2 m_CommercialTaxLimits;
    public int2 m_IndustrialTaxLimits;
    public int2 m_OfficeTaxLimits;
    public int2 m_JobLevelTaxLimits;
    public int2 m_ResourceTaxLimits;

    public bool Equals(TaxParameterData other)
    {
      return this.m_CommercialTaxLimits.Equals(other.m_CommercialTaxLimits) && this.m_IndustrialTaxLimits.Equals(other.m_IndustrialTaxLimits) && this.m_JobLevelTaxLimits.Equals(other.m_JobLevelTaxLimits) && this.m_OfficeTaxLimits.Equals(other.m_OfficeTaxLimits) && this.m_ResidentialTaxLimits.Equals(other.m_ResidentialTaxLimits) && this.m_ResourceTaxLimits.Equals(other.m_ResourceTaxLimits) && this.m_TotalTaxLimits.Equals(other.m_TotalTaxLimits);
    }
  }
}
