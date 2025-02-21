// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CompanyObject
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {typeof (ObjectPrefab)})]
  public class CompanyObject : ComponentBase
  {
    public bool m_SelectCompany;
    public CompanyPrefab[] m_Companies;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      for (int index = 0; index < this.m_Companies.Length; ++index)
        prefabs.Add((PrefabBase) this.m_Companies[index]);
    }

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
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      DynamicBuffer<ObjectRequirementElement> buffer = entityManager.GetBuffer<ObjectRequirementElement>(entity);
      int length = buffer.Length;
      ObjectRequirementType type = this.m_SelectCompany ? ObjectRequirementType.SelectOnly : (ObjectRequirementType) 0;
      for (int index = 0; index < this.m_Companies.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        buffer.Add(new ObjectRequirementElement(existingSystemManaged.GetEntity((PrefabBase) this.m_Companies[index]), length, type));
      }
    }
  }
}
