// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UIAssetCategoryPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("UI/", new System.Type[] {})]
  public class UIAssetCategoryPrefab : UIGroupPrefab
  {
    public UIAssetMenuPrefab m_Menu;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      if (!((UnityEngine.Object) this.m_Menu != (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<UIAssetCategoryData>());
    }

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (!((UnityEngine.Object) this.m_Menu != (UnityEngine.Object) null))
        return;
      prefabs.Add((PrefabBase) this.m_Menu);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      if (!((UnityEngine.Object) this.m_Menu != (UnityEngine.Object) null))
        return;
      // ISSUE: reference to a compiler-generated method
      Entity entity1 = entityManager.World.GetExistingSystemManaged<PrefabSystem>().GetEntity((PrefabBase) this.m_Menu);
      entityManager.SetComponentData<UIAssetCategoryData>(entity, new UIAssetCategoryData(entity1));
      this.m_Menu.AddElement(entityManager, entity);
    }
  }
}
