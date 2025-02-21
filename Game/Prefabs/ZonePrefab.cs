// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZonePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Zones;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Zones/", new System.Type[] {})]
  public class ZonePrefab : PrefabBase
  {
    public AreaType m_AreaType;
    public Color m_Color = Color.white;
    public Color m_Edge = Color.white;
    public bool m_Office;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ZoneData>());
      components.Add(ComponentType.ReadWrite<PlaceableInfoviewItem>());
      components.Add(ComponentType.ReadWrite<ProcessEstimate>());
    }

    public void GetBuildingPrefabComponents(
      HashSet<ComponentType> components,
      BuildingPrefab buildingPrefab,
      byte level)
    {
      if (this.m_Office)
        components.Add(ComponentType.ReadWrite<OfficeBuilding>());
      List<IZoneBuildingComponent> result = new List<IZoneBuildingComponent>();
      if (!this.prefab.TryGet<IZoneBuildingComponent>(result))
        return;
      foreach (IZoneBuildingComponent buildingComponent in result)
        buildingComponent.GetBuildingPrefabComponents(components, buildingPrefab, level);
    }

    public void GetBuildingArchetypeComponents(
      HashSet<ComponentType> components,
      BuildingPrefab buildingPrefab,
      byte level)
    {
      List<IZoneBuildingComponent> result = new List<IZoneBuildingComponent>();
      if (!this.prefab.TryGet<IZoneBuildingComponent>(result))
        return;
      foreach (IZoneBuildingComponent buildingComponent in result)
        buildingComponent.GetBuildingArchetypeComponents(components, buildingPrefab, level);
    }

    public void InitializeBuilding(
      EntityManager entityManager,
      Entity entity,
      BuildingPrefab buildingPrefab,
      byte level)
    {
      List<IZoneBuildingComponent> result = new List<IZoneBuildingComponent>();
      if (!this.prefab.TryGet<IZoneBuildingComponent>(result))
        return;
      foreach (IZoneBuildingComponent buildingComponent in result)
        buildingComponent.InitializeBuilding(entityManager, entity, buildingPrefab, level);
    }

    public override IEnumerable<string> modTags
    {
      get
      {
        foreach (string modTag in base.modTags)
          yield return modTag;
        yield return "Zones";
        if (this.m_Office)
          yield return "ZonesOffice";
        else
          yield return string.Format("Zones{0}", (object) this.m_AreaType);
      }
    }
  }
}
