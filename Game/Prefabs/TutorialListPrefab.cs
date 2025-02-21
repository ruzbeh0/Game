// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialListPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Tutorials;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tutorials/", new Type[] {})]
  public class TutorialListPrefab : PrefabBase
  {
    public int m_Priority;
    [NotNull]
    public TutorialPrefab[] m_Tutorials;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      foreach (TutorialPrefab tutorial in this.m_Tutorials)
        prefabs.Add((PrefabBase) tutorial);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<TutorialListData>());
      components.Add(ComponentType.ReadWrite<TutorialRef>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<TutorialListData>(entity, new TutorialListData(this.m_Priority));
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PrefabSystem>();
      DynamicBuffer<TutorialRef> buffer = entityManager.GetBuffer<TutorialRef>(entity);
      foreach (TutorialPrefab tutorial in this.m_Tutorials)
      {
        // ISSUE: reference to a compiler-generated method
        Entity entity1 = existingSystemManaged.GetEntity((PrefabBase) tutorial);
        TutorialRef elem = new TutorialRef()
        {
          m_Tutorial = entity1
        };
        buffer.Add(elem);
      }
    }
  }
}
