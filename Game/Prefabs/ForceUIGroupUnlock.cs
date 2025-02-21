// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ForceUIGroupUnlock
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [RequireComponent(typeof (UnlockableBase))]
  [ComponentMenu("Prefabs/Unlocking/", new System.Type[] {typeof (TutorialPrefab), typeof (TutorialPhasePrefab), typeof (TutorialTriggerPrefabBase), typeof (TutorialListPrefab)})]
  public class ForceUIGroupUnlock : ComponentBase
  {
    [Tooltip("UIGroups listed here will unlock whenever this prefab unlocks, regardless if their own unlock requirements have been met.")]
    public UIGroupPrefab[] m_Unlocks;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ForceUIGroupUnlockData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      DynamicBuffer<ForceUIGroupUnlockData> buffer = entityManager.GetBuffer<ForceUIGroupUnlockData>(entity);
      for (int index = 0; index < this.m_Unlocks.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        Entity entity1 = existingSystemManaged.GetEntity((PrefabBase) this.m_Unlocks[index]);
        buffer.Add(new ForceUIGroupUnlockData()
        {
          m_Entity = entity1
        });
      }
    }
  }
}
