// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialsConfigurationPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new Type[] {})]
  public class TutorialsConfigurationPrefab : PrefabBase
  {
    [NotNull]
    public TutorialListPrefab m_TutorialsIntroList;
    [NotNull]
    public FeaturePrefab m_MapTilesPrefab;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_TutorialsIntroList);
      prefabs.Add((PrefabBase) this.m_MapTilesPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<TutorialsConfigurationData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      Entity entity1 = existingSystemManaged.GetEntity((PrefabBase) this.m_TutorialsIntroList);
      // ISSUE: reference to a compiler-generated method
      Entity entity2 = existingSystemManaged.GetEntity((PrefabBase) this.m_MapTilesPrefab);
      entityManager.SetComponentData<TutorialsConfigurationData>(entity, new TutorialsConfigurationData()
      {
        m_TutorialsIntroList = entity1,
        m_MapTilesFeature = entity2
      });
    }
  }
}
