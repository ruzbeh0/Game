// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildingModifiers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Policies/", new Type[] {typeof (PolicyPrefab)})]
  public class BuildingModifiers : ComponentBase
  {
    public BuildingModifierInfo[] m_Modifiers;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<BuildingModifierData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      if (this.m_Modifiers == null)
        return;
      DynamicBuffer<BuildingModifierData> buffer = entityManager.GetBuffer<BuildingModifierData>(entity);
      for (int index = 0; index < this.m_Modifiers.Length; ++index)
      {
        BuildingModifierInfo modifier = this.m_Modifiers[index];
        buffer.Add(new BuildingModifierData(modifier.m_Type, modifier.m_Mode, modifier.m_Range));
      }
    }
  }
}
