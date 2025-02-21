// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TriggerPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Triggers;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Triggers/", new System.Type[] {})]
  public class TriggerPrefab : PrefabBase
  {
    public TriggerType m_TriggerType;
    public PrefabBase[] m_TriggerPrefabs;
    [EnumFlag]
    public TargetType m_TargetTypes;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_TriggerPrefabs == null)
        return;
      foreach (PrefabBase triggerPrefab in this.m_TriggerPrefabs)
      {
        if ((UnityEngine.Object) triggerPrefab != (UnityEngine.Object) null)
          prefabs.Add(triggerPrefab);
      }
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<TriggerData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      DynamicBuffer<TriggerData> buffer = entityManager.GetBuffer<TriggerData>(entity);
      if (this.m_TriggerPrefabs != null && this.m_TriggerPrefabs.Length != 0)
      {
        foreach (PrefabBase triggerPrefab in this.m_TriggerPrefabs)
        {
          if ((UnityEngine.Object) triggerPrefab != (UnityEngine.Object) null)
          {
            // ISSUE: reference to a compiler-generated method
            buffer.Add(new TriggerData()
            {
              m_TriggerType = this.m_TriggerType,
              m_TargetTypes = this.m_TargetTypes,
              m_TriggerPrefab = existingSystemManaged.GetEntity(triggerPrefab)
            });
          }
        }
      }
      else
        buffer.Add(new TriggerData()
        {
          m_TriggerType = this.m_TriggerType,
          m_TargetTypes = this.m_TargetTypes
        });
    }
  }
}
