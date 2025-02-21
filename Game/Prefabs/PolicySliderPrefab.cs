// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PolicySliderPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Policies/", new Type[] {})]
  public class PolicySliderPrefab : PolicyPrefab
  {
    public Bounds1 m_SliderRange = new Bounds1(0.0f, 1f);
    public float m_SliderDefault = 0.5f;
    public float m_SliderStep = 0.1f;
    public PolicySliderUnit m_Unit = PolicySliderUnit.integer;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<PolicySliderData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      PolicySliderData componentData;
      componentData.m_Range = this.m_SliderRange;
      componentData.m_Default = this.m_SliderDefault;
      componentData.m_Step = this.m_SliderStep;
      componentData.m_Unit = (int) this.m_Unit;
      entityManager.SetComponentData<PolicySliderData>(entity, componentData);
    }
  }
}
