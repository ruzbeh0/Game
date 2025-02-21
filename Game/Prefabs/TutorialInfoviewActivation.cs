// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialInfoviewActivation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Tutorials;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tutorials/Activation/", new Type[] {typeof (TutorialPrefab)})]
  public class TutorialInfoviewActivation : TutorialActivation
  {
    [NotNull]
    public InfoviewPrefab m_Infoview;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_Infoview);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<InfoviewActivationData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      Entity entity1;
      // ISSUE: reference to a compiler-generated method
      if (!entityManager.World.GetExistingSystemManaged<PrefabSystem>().TryGetEntity((PrefabBase) this.m_Infoview, out entity1))
        return;
      entityManager.SetComponentData<InfoviewActivationData>(entity, new InfoviewActivationData(entity1));
    }

    public override void GenerateTutorialLinks(
      EntityManager entityManager,
      NativeParallelHashSet<Entity> linkedPrefabs)
    {
      base.GenerateTutorialLinks(entityManager, linkedPrefabs);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      linkedPrefabs.Add(existingSystemManaged.GetEntity((PrefabBase) this.m_Infoview));
    }
  }
}
