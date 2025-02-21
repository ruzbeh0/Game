// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.IZoneBuildingComponent
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public interface IZoneBuildingComponent
  {
    void GetBuildingPrefabComponents(
      HashSet<ComponentType> components,
      BuildingPrefab buildingPrefab,
      byte level);

    void GetBuildingArchetypeComponents(
      HashSet<ComponentType> components,
      BuildingPrefab buildingPrefab,
      byte level);

    void InitializeBuilding(
      EntityManager entityManager,
      Entity entity,
      BuildingPrefab buildingPrefab,
      byte level);
  }
}
