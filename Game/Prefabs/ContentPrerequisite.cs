// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ContentPrerequisite
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.UI.Editor;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [HideInEditor]
  [ComponentMenu("Prefabs/Content/", new System.Type[] {})]
  public class ContentPrerequisite : ComponentBase
  {
    public ContentPrefab m_ContentPrerequisite;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_ContentPrerequisite);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ContentPrerequisiteData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      if (!((UnityEngine.Object) this.m_ContentPrerequisite != (UnityEngine.Object) null))
        return;
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<ContentPrerequisiteData>(entity, new ContentPrerequisiteData()
      {
        m_ContentPrerequisite = existingSystemManaged.GetEntity((PrefabBase) this.m_ContentPrerequisite)
      });
    }
  }
}
