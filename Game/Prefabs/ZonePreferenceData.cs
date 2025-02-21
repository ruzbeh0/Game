// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZonePreferenceData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct ZonePreferenceData : IComponentData, IQueryTypeParameter
  {
    public float m_ResidentialSignificanceServices;
    public float m_ResidentialSignificanceWorkplaces;
    public float m_ResidentialSignificanceLandValue;
    public float m_ResidentialSignificancePollution;
    public float m_ResidentialNeutralLandValue;
    public float m_CommercialSignificanceConsumers;
    public float m_CommercialSignificanceCompetitors;
    public float m_CommercialSignificanceWorkplaces;
    public float m_CommercialSignificanceLandValue;
    public float m_CommercialNeutralLandValue;
    public float m_IndustrialSignificanceInput;
    public float m_IndustrialSignificanceOutside;
    public float m_IndustrialSignificanceLandValue;
    public float m_IndustrialNeutralLandValue;
    public float m_OfficeSignificanceEmployees;
    public float m_OfficeSignificanceServices;
  }
}
