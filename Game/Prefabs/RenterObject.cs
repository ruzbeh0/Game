// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.RenterObject
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {typeof (StaticObjectPrefab), typeof (MarkerObjectPrefab)})]
  public class RenterObject : ComponentBase
  {
    public bool m_RequireEmpty;
    public bool m_RequireRenter;
    public bool m_RequireGoodWealth;
    public bool m_RequireDogs;
    public bool m_RequireHomeless;
    public bool m_RequireChildren;
    public bool m_RequireTeens;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ObjectRequirementElement>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      DynamicBuffer<ObjectRequirementElement> buffer = entityManager.GetBuffer<ObjectRequirementElement>(entity);
      int length = buffer.Length;
      ObjectRequirementFlags require = (ObjectRequirementFlags) 0;
      ObjectRequirementFlags forbid = (ObjectRequirementFlags) 0;
      if (this.m_RequireEmpty)
        forbid |= ObjectRequirementFlags.Renter;
      if (this.m_RequireRenter)
        require |= ObjectRequirementFlags.Renter;
      if (this.m_RequireGoodWealth)
        require |= ObjectRequirementFlags.GoodWealth;
      if (this.m_RequireDogs)
        require |= ObjectRequirementFlags.Dogs;
      if (this.m_RequireHomeless)
        require |= ObjectRequirementFlags.Homeless;
      if (!this.m_RequireChildren && !this.m_RequireTeens)
        buffer.Add(new ObjectRequirementElement(require, forbid, length));
      if (this.m_RequireChildren)
        buffer.Add(new ObjectRequirementElement(require | ObjectRequirementFlags.Children, forbid, length));
      if (!this.m_RequireTeens)
        return;
      buffer.Add(new ObjectRequirementElement(require | ObjectRequirementFlags.Teens, forbid, length));
    }
  }
}
