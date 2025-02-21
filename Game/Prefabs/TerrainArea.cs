// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TerrainArea
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Areas/", new Type[] {typeof (LotPrefab)})]
  public class TerrainArea : ComponentBase
  {
    public float m_HeightOffset = 20f;
    public float m_SlopeWidth = 20f;
    public float m_NoiseScale = 100f;
    public float m_NoiseFactor = 1f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<TerrainAreaData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Terrain>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      TerrainAreaData componentData;
      componentData.m_HeightOffset = this.m_HeightOffset;
      componentData.m_SlopeWidth = this.m_SlopeWidth;
      componentData.m_NoiseScale = 1f / math.max(1f / 1000f, this.m_NoiseScale);
      componentData.m_NoiseFactor = this.m_NoiseFactor;
      entityManager.SetComponentData<TerrainAreaData>(entity, componentData);
    }
  }
}
