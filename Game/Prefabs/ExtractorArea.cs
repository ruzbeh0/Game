// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ExtractorArea
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Vehicles;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Areas/", new System.Type[] {typeof (LotPrefab)})]
  public class ExtractorArea : ComponentBase
  {
    public MapFeature m_MapFeature;
    [Tooltip("Spawned object surface area per extracted resource amount")]
    public float m_ObjectSpawnFactor = 2f;
    [Tooltip("Maximum object surface area proportion of total extractor area")]
    public float m_MaxObjectArea = 0.25f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ExtractorAreaData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Extractor>());
      components.Add(ComponentType.ReadWrite<OwnedVehicle>());
      if (this.m_MapFeature != MapFeature.Forest)
        return;
      components.Add(ComponentType.ReadWrite<WoodResource>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<ExtractorAreaData>(entity, new ExtractorAreaData()
      {
        m_MapFeature = this.m_MapFeature,
        m_ObjectSpawnFactor = this.m_ObjectSpawnFactor,
        m_MaxObjectArea = this.m_MaxObjectArea
      });
    }
  }
}
