// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.GroupAmbience
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Simulation;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Zones/", new Type[] {typeof (ZonePrefab)})]
  public class GroupAmbience : ComponentBase, IZoneBuildingComponent
  {
    public GroupAmbienceType m_AmbienceType;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<GroupAmbienceData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<GroupAmbienceData>(entity, new GroupAmbienceData()
      {
        m_AmbienceType = this.m_AmbienceType
      });
    }

    public void GetBuildingPrefabComponents(
      HashSet<ComponentType> components,
      BuildingPrefab buildingPrefab,
      byte level)
    {
      components.Add(ComponentType.ReadWrite<GroupAmbienceData>());
    }

    public void GetBuildingArchetypeComponents(
      HashSet<ComponentType> components,
      BuildingPrefab buildingPrefab,
      byte level)
    {
    }

    public void InitializeBuilding(
      EntityManager entityManager,
      Entity entity,
      BuildingPrefab buildingPrefab,
      byte level)
    {
      if (buildingPrefab.Has<GroupAmbience>())
        return;
      entityManager.SetComponentData<GroupAmbienceData>(entity, new GroupAmbienceData()
      {
        m_AmbienceType = this.m_AmbienceType
      });
    }
  }
}
