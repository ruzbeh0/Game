// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.DefaultPolicies
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Policies;
using Game.Routes;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Policies/", new Type[] {typeof (DistrictPrefab), typeof (BuildingPrefab), typeof (RoutePrefab), typeof (ServiceFeeParameterPrefab)})]
  public class DefaultPolicies : ComponentBase
  {
    public DefaultPolicyInfo[] m_Policies;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_Policies == null)
        return;
      for (int index = 0; index < this.m_Policies.Length; ++index)
        prefabs.Add((PrefabBase) this.m_Policies[index].m_Policy);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<DefaultPolicyData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (components.Contains(ComponentType.ReadWrite<Waypoint>()) || components.Contains(ComponentType.ReadWrite<Segment>()))
        return;
      components.Add(ComponentType.ReadWrite<Policy>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      if (this.m_Policies == null)
        return;
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      DynamicBuffer<DefaultPolicyData> buffer = entityManager.GetBuffer<DefaultPolicyData>(entity);
      for (int index = 0; index < this.m_Policies.Length; ++index)
      {
        DefaultPolicyInfo policy = this.m_Policies[index];
        // ISSUE: reference to a compiler-generated method
        buffer.Add(new DefaultPolicyData(existingSystemManaged.GetEntity((PrefabBase) policy.m_Policy)));
      }
    }
  }
}
