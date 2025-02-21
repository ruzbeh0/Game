// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialAutoActivation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Tutorials;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tutorials/Activation/", new System.Type[] {typeof (TutorialPrefab), typeof (TutorialListPrefab)})]
  public class TutorialAutoActivation : TutorialActivation
  {
    [CanBeNull]
    public PrefabBase m_RequiredUnlock;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (!((UnityEngine.Object) this.m_RequiredUnlock != (UnityEngine.Object) null))
        return;
      prefabs.Add(this.m_RequiredUnlock);
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<AutoActivationData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      Entity entity1 = Entity.Null;
      if ((UnityEngine.Object) this.m_RequiredUnlock != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        entity1 = existingSystemManaged.GetEntity(this.m_RequiredUnlock);
      }
      entityManager.SetComponentData<AutoActivationData>(entity, new AutoActivationData()
      {
        m_RequiredUnlock = entity1
      });
    }
  }
}
