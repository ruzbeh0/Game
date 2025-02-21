// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialActivation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Tutorials;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public abstract class TutorialActivation : ComponentBase
  {
    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<TutorialActivationData>());
    }

    public virtual void GenerateTutorialLinks(
      EntityManager entityManager,
      NativeParallelHashSet<Entity> linkedPrefabs)
    {
    }

    public override bool ignoreUnlockDependencies => true;
  }
}
