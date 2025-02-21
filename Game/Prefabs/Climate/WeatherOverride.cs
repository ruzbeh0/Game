// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Climate.WeatherOverride
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs.Climate
{
  [ComponentMenu("Weather/", new Type[] {typeof (WeatherPrefab)})]
  public class WeatherOverride : ComponentBase
  {
    public WeatherPrefab[] m_Placeholders;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      for (int index = 0; index < this.m_Placeholders.Length; ++index)
        prefabs.Add((PrefabBase) this.m_Placeholders[index]);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<SpawnableObjectData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      for (int index = 0; index < this.m_Placeholders.Length; ++index)
      {
        WeatherPrefab placeholder = this.m_Placeholders[index];
        // ISSUE: reference to a compiler-generated method
        Entity entity1 = existingSystemManaged.GetEntity((PrefabBase) placeholder);
        entityManager.GetBuffer<PlaceholderObjectElement>(entity1).Add(new PlaceholderObjectElement(entity));
      }
    }
  }
}
