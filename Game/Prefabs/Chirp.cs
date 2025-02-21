// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Chirp
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Triggers/", new System.Type[] {typeof (TriggerPrefab), typeof (StatisticTriggerPrefab)})]
  public class Chirp : ComponentBase
  {
    [Tooltip("When the trigger happens, one of these chirps will be selected randomly")]
    public PrefabBase[] m_Chirps;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<TriggerChirpData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      DynamicBuffer<TriggerChirpData> buffer = entityManager.GetBuffer<TriggerChirpData>(entity);
      if (this.m_Chirps == null || this.m_Chirps.Length == 0)
        return;
      foreach (PrefabBase chirp in this.m_Chirps)
      {
        if ((UnityEngine.Object) chirp != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated method
          buffer.Add(new TriggerChirpData()
          {
            m_Chirp = existingSystemManaged.GetEntity(chirp)
          });
        }
      }
    }
  }
}
