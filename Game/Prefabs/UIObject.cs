// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UIObject
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.UI.Editor;
using Game.UI.Widgets;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("UI/", new System.Type[] {})]
  public class UIObject : ComponentBase
  {
    public UIGroupPrefab m_Group;
    public int m_Priority;
    [CustomField(typeof (UIIconField))]
    public string m_Icon;
    public bool m_IsDebugObject;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (!((UnityEngine.Object) this.m_Group != (UnityEngine.Object) null))
        return;
      prefabs.Add((PrefabBase) this.m_Group);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      if (this.m_IsDebugObject && !Debug.isDebugBuild)
        return;
      components.Add(ComponentType.ReadWrite<UIObjectData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      if (this.m_IsDebugObject && !Debug.isDebugBuild)
        return;
      Entity entity1 = Entity.Null;
      if ((UnityEngine.Object) this.m_Group != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        entity1 = entityManager.World.GetExistingSystemManaged<PrefabSystem>().GetEntity((PrefabBase) this.m_Group);
        this.m_Group.AddElement(entityManager, entity);
      }
      entityManager.SetComponentData<UIObjectData>(entity, new UIObjectData()
      {
        m_Group = entity1,
        m_Priority = this.m_Priority
      });
    }

    public override IEnumerable<string> modTags
    {
      get
      {
        foreach (string modTag in base.modTags)
          yield return modTag;
        if (this.m_Group is UIAssetCategoryPrefab category)
        {
          yield return "UI" + category.name;
          if (category.name.StartsWith("Props"))
            yield return "UIProps";
          if ((UnityEngine.Object) category.m_Menu != (UnityEngine.Object) null)
            yield return "UI" + category.m_Menu.name;
        }
      }
    }
  }
}
