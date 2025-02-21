// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZonePreferencePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Zones/", new Type[] {})]
  public class ZonePreferencePrefab : PrefabBase
  {
    public float m_ResidentialSignificanceServices = 100f;
    public float m_ResidentialSignificanceWorkplaces = 50f;
    public float m_ResidentialSignificanceLandValue = -1f;
    public float m_ResidentialSignificancePollution = -100f;
    public float m_ResidentialNeutralLandValue = 10f;
    public float m_CommercialSignificanceConsumers = 100f;
    public float m_CommercialSignificanceCompetitors = 200f;
    public float m_CommercialSignificanceWorkplaces = 70f;
    public float m_CommercialSignificanceLandValue = -0.5f;
    public float m_CommercialNeutralLandValue = 20f;
    public float m_IndustrialSignificanceInput = 100f;
    public float m_IndustrialSignificanceOutside = 100f;
    public float m_IndustrialSignificanceLandValue = -1f;
    public float m_IndustrialNeutralLandValue = 5f;
    public float m_OfficeSignificanceEmployees = 100f;
    public float m_OfficeSignificanceServices = 100f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ZonePreferenceData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      entityManager.SetComponentData<ZonePreferenceData>(entity, new ZonePreferenceData()
      {
        m_ResidentialSignificanceServices = this.m_ResidentialSignificanceServices,
        m_ResidentialSignificanceWorkplaces = this.m_ResidentialSignificanceWorkplaces,
        m_ResidentialSignificanceLandValue = this.m_ResidentialSignificanceLandValue,
        m_ResidentialSignificancePollution = this.m_ResidentialSignificancePollution,
        m_ResidentialNeutralLandValue = this.m_ResidentialNeutralLandValue,
        m_CommercialSignificanceCompetitors = this.m_CommercialSignificanceCompetitors,
        m_CommercialSignificanceConsumers = this.m_CommercialSignificanceConsumers,
        m_CommercialSignificanceWorkplaces = this.m_CommercialSignificanceWorkplaces,
        m_CommercialSignificanceLandValue = this.m_CommercialSignificanceLandValue,
        m_CommercialNeutralLandValue = this.m_CommercialNeutralLandValue,
        m_IndustrialSignificanceInput = this.m_IndustrialSignificanceInput,
        m_IndustrialSignificanceLandValue = this.m_IndustrialSignificanceLandValue,
        m_IndustrialSignificanceOutside = this.m_IndustrialSignificanceOutside,
        m_IndustrialNeutralLandValue = this.m_IndustrialNeutralLandValue,
        m_OfficeSignificanceEmployees = this.m_OfficeSignificanceEmployees,
        m_OfficeSignificanceServices = this.m_OfficeSignificanceServices
      });
    }
  }
}
