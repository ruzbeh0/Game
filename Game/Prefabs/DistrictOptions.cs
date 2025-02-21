// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.DistrictOptions
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Policies/", new Type[] {typeof (PolicyPrefab)})]
  public class DistrictOptions : ComponentBase
  {
    public DistrictOption[] m_Options;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<DistrictOptionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      if (this.m_Options == null)
        return;
      DistrictOptionData componentData = new DistrictOptionData();
      for (int index = 0; index < this.m_Options.Length; ++index)
        componentData.m_OptionMask |= 1U << (int) (this.m_Options[index] & (DistrictOption) 31);
      entityManager.SetComponentData<DistrictOptionData>(entity, componentData);
    }
  }
}
