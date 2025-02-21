// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SpawnableBuilding
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new System.Type[] {typeof (BuildingPrefab)})]
  public class SpawnableBuilding : ComponentBase
  {
    public ZonePrefab m_ZoneType;
    [Range(1f, 5f)]
    public byte m_Level;

    public override bool ignoreUnlockDependencies => true;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_ZoneType);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<SpawnableBuildingData>());
      components.Add(ComponentType.ReadWrite<BuildingSpawnGroupData>());
      if (!((UnityEngine.Object) this.m_ZoneType != (UnityEngine.Object) null))
        return;
      this.m_ZoneType.GetBuildingPrefabComponents(components, (BuildingPrefab) this.prefab, this.m_Level);
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<BuildingCondition>());
      if (!((UnityEngine.Object) this.m_ZoneType != (UnityEngine.Object) null))
        return;
      if (this.m_ZoneType.Has<RandomLocalization>())
        components.Add(ComponentType.ReadWrite<RandomLocalizationIndex>());
      this.m_ZoneType.GetBuildingArchetypeComponents(components, (BuildingPrefab) this.prefab, this.m_Level);
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      if (!((UnityEngine.Object) this.m_ZoneType != (UnityEngine.Object) null))
        return;
      this.m_ZoneType.InitializeBuilding(entityManager, entity, (BuildingPrefab) this.prefab, this.m_Level);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      SpawnableBuildingData componentData = new SpawnableBuildingData()
      {
        m_Level = this.m_Level
      };
      if ((UnityEngine.Object) this.m_ZoneType != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        componentData.m_ZonePrefab = existingSystemManaged.GetEntity((PrefabBase) this.m_ZoneType);
      }
      entityManager.SetComponentData<SpawnableBuildingData>(entity, componentData);
    }

    public override IEnumerable<string> modTags
    {
      get
      {
        foreach (string modTag in base.modTags)
          yield return modTag;
        UIObject component;
        if ((UnityEngine.Object) this.m_ZoneType != (UnityEngine.Object) null && this.m_ZoneType.TryGet<UIObject>(out component) && (UnityEngine.Object) component.m_Group != (UnityEngine.Object) null && component.m_Group is UIAssetCategoryPrefab group)
          yield return nameof (SpawnableBuilding) + group.name;
      }
    }
  }
}
