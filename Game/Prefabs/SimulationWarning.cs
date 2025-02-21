// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SimulationWarning
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Notifications/", new Type[] {typeof (NotificationIconPrefab)})]
  public class SimulationWarning : ComponentBase
  {
    public IconCategory[] m_Categories;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      if (this.m_Categories == null)
        return;
      NotificationIconDisplayData componentData = entityManager.GetComponentData<NotificationIconDisplayData>(entity);
      for (int index = 0; index < this.m_Categories.Length; ++index)
        componentData.m_CategoryMask |= 1U << (int) (this.m_Categories[index] & (IconCategory) 31);
      entityManager.SetComponentData<NotificationIconDisplayData>(entity, componentData);
    }
  }
}
