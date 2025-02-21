// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UpkeepModifier
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Economy;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new Type[] {typeof (BuildingExtensionPrefab)})]
  public class UpkeepModifier : ComponentBase, IServiceUpgrade
  {
    public UpkeepModifierInfo[] m_Modifiers;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<UpkeepModifierData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<UpkeepModifier>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      if (this.m_Modifiers == null)
        return;
      DynamicBuffer<UpkeepModifierData> buffer = entityManager.GetBuffer<UpkeepModifierData>(entity);
      for (int index = 0; index < this.m_Modifiers.Length; ++index)
      {
        UpkeepModifierInfo modifier = this.m_Modifiers[index];
        buffer.Add(new UpkeepModifierData()
        {
          m_Resource = EconomyUtils.GetResource(modifier.m_Resource),
          m_Multiplier = modifier.m_Multiplier
        });
      }
    }
  }
}
