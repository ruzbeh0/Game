// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Climate.SeasonFilter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs.Climate
{
  [ComponentMenu("Themes/", new Type[] {typeof (WeatherPrefab)})]
  public class SeasonFilter : ComponentBase
  {
    public SeasonPrefab[] m_Seasons;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      for (int index = 0; index < this.m_Seasons.Length; ++index)
        prefabs.Add((PrefabBase) this.m_Seasons[index]);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ObjectRequirementElement>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      DynamicBuffer<ObjectRequirementElement> buffer = entityManager.GetBuffer<ObjectRequirementElement>(entity);
      int length = buffer.Length;
      for (int index = 0; index < this.m_Seasons.Length; ++index)
      {
        SeasonPrefab season = this.m_Seasons[index];
        // ISSUE: reference to a compiler-generated method
        Entity entity1 = existingSystemManaged.GetEntity((PrefabBase) season);
        buffer.Add(new ObjectRequirementElement(entity1, length));
      }
    }
  }
}
