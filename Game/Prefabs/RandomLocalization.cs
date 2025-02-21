// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.RandomLocalization
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.SceneFlow;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Localization/", new System.Type[] {})]
  public class RandomLocalization : Localization
  {
    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<LocalizationCount>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<RandomLocalizationIndex>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      int localizationCount = this.GetLocalizationCount();
      entityManager.GetBuffer<LocalizationCount>(entity).Add(new LocalizationCount()
      {
        m_Count = localizationCount
      });
    }

    protected virtual int GetLocalizationCount()
    {
      return RandomLocalization.GetLocalizationIndexCount(this.prefab, this.m_LocalizationID);
    }

    public static int GetLocalizationIndexCount(PrefabBase prefab, string id)
    {
      int localizationIndexCount = -1;
      if (id != null)
      {
        Dictionary<string, int> indexCounts = GameManager.instance.localizationManager.activeDictionary.indexCounts;
        if (indexCounts.ContainsKey(id))
          localizationIndexCount = indexCounts[id];
      }
      if (localizationIndexCount < 1)
        ComponentBase.baseLog.WarnFormat((UnityEngine.Object) prefab, "Warning: localizationID {0} not found for {1}", (object) id, (object) prefab.name);
      return localizationIndexCount;
    }
  }
}
