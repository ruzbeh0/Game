// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.HappinessFactorParameterPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Simulation;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class HappinessFactorParameterPrefab : PrefabBase
  {
    [EnumValue(typeof (CitizenHappinessSystem.HappinessFactor))]
    public int[] m_BaseLevels = new int[25];
    [EnumValue(typeof (CitizenHappinessSystem.HappinessFactor))]
    public PrefabBase[] m_LockedEntities = new PrefabBase[25];

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      for (int index = 0; index < this.m_LockedEntities.Length; ++index)
      {
        if ((UnityEngine.Object) this.m_LockedEntities[index] != (UnityEngine.Object) null)
          prefabs.Add(this.m_LockedEntities[index]);
      }
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<HappinessFactorParameterData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>();
      DynamicBuffer<HappinessFactorParameterData> buffer = entityManager.GetBuffer<HappinessFactorParameterData>(entity);
      for (int index = 0; index < this.m_BaseLevels.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        Entity entity1 = (UnityEngine.Object) this.m_LockedEntities[index] != (UnityEngine.Object) null ? systemManaged.GetEntity(this.m_LockedEntities[index]) : Entity.Null;
        buffer.Add(new HappinessFactorParameterData()
        {
          m_BaseLevel = this.m_BaseLevels[index],
          m_LockedEntity = entity1
        });
      }
    }
  }
}
