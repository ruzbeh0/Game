// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MapTilePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Areas;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Areas/", new Type[] {})]
  public class MapTilePrefab : AreaPrefab
  {
    public float m_PurchaseCostFactor = 2500f;
    public MapTilePrefab.FeatureInfo[] m_MapFeatures;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<MapTileData>());
      components.Add(ComponentType.ReadWrite<MapFeatureData>());
      components.Add(ComponentType.ReadWrite<AreaGeometryData>());
      components.Add(ComponentType.ReadWrite<TilePurchaseCostFactor>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<MapTile>());
      components.Add(ComponentType.ReadWrite<MapFeatureElement>());
      components.Add(ComponentType.ReadWrite<Geometry>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      DynamicBuffer<MapFeatureData> buffer = entityManager.GetBuffer<MapFeatureData>(entity);
      CollectionUtils.ResizeInitialized<MapFeatureData>(buffer, 8);
      for (int index = 0; index < this.m_MapFeatures.Length; ++index)
      {
        MapTilePrefab.FeatureInfo mapFeature = this.m_MapFeatures[index];
        buffer[(int) mapFeature.m_MapFeature] = new MapFeatureData(mapFeature.m_Cost);
      }
      TilePurchaseCostFactor componentData = new TilePurchaseCostFactor(this.m_PurchaseCostFactor);
      entityManager.SetComponentData<TilePurchaseCostFactor>(entity, componentData);
    }

    [Serializable]
    public class FeatureInfo
    {
      public MapFeature m_MapFeature;
      public float m_Cost;
    }
  }
}
