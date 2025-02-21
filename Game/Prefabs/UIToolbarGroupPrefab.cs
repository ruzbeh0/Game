// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UIToolbarGroupPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("UI/", new Type[] {})]
  public class UIToolbarGroupPrefab : UIGroupPrefab
  {
    public int m_Priority;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<UIToolbarGroupData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      entityManager.SetComponentData<UIToolbarGroupData>(entity, new UIToolbarGroupData()
      {
        m_Priority = this.m_Priority
      });
    }
  }
}
