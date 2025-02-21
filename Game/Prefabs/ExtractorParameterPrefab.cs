// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ExtractorParameterPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class ExtractorParameterPrefab : PrefabBase
  {
    [Tooltip("How much of the fertility resource is consumed per produced agricultural unit")]
    public float m_FertilityConsumption = 0.1f;
    [Tooltip("How much ore can be extracted before efficiency drops to 1/2.71th of the original")]
    public float m_OreConsumption = 500000f;
    [Tooltip("How much of the forest resource is consumed per produced wood unit")]
    public float m_ForestConsumption = 1f;
    [Tooltip("How much oil can be extracted before efficiency drops to 1/2.71th of the original")]
    public float m_OilConsumption = 100000f;
    [Tooltip("If the resource concentration goes under this limit, the productivity starts to drop")]
    public float m_FullFertility = 0.5f;
    [Tooltip("If the resource concentration goes under this limit, the productivity starts to drop")]
    public float m_FullOre = 0.8f;
    [Tooltip("If the resource concentration goes under this limit, the productivity starts to drop")]
    public float m_FullOil = 0.8f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ExtractorParameterData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      entityManager.SetComponentData<ExtractorParameterData>(entity, new ExtractorParameterData()
      {
        m_FertilityConsumption = this.m_FertilityConsumption,
        m_ForestConsumption = this.m_ForestConsumption,
        m_OreConsumption = this.m_OreConsumption,
        m_OilConsumption = this.m_OilConsumption,
        m_FullFertility = this.m_FullFertility,
        m_FullOil = this.m_FullOil,
        m_FullOre = this.m_FullOre
      });
    }
  }
}
