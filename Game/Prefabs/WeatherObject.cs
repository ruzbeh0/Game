// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WeatherObject
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
  public class WeatherObject : ComponentBase
  {
    public bool m_RequireSnow;
    public bool m_ForbidSnow;

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
      if (this.m_RequireSnow)
        require |= ObjectRequirementFlags.Snow;
      if (this.m_ForbidSnow)
        forbid |= ObjectRequirementFlags.Snow;
      buffer.Add(new ObjectRequirementElement(require, forbid, length));
    }
  }
}
