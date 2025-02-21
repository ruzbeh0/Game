// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.EffectPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Effects;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public class EffectPrefab : TransformPrefab
  {
    public EffectCondition m_Conditions;
    public bool m_DisableDistanceCulling;

    public override void GetDependencies(List<PrefabBase> prefabs) => base.GetDependencies(prefabs);

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<EffectInstance>());
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<EffectData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      entityManager.SetComponentData<EffectData>(entity, new EffectData()
      {
        m_Archetype = this.GetArchetype(entityManager, entity),
        m_Flags = new EffectCondition()
        {
          m_RequiredFlags = this.m_Conditions.m_RequiredFlags,
          m_ForbiddenFlags = this.m_Conditions.m_ForbiddenFlags,
          m_IntensityFlags = this.m_Conditions.m_IntensityFlags
        },
        m_OwnerCulling = !this.m_DisableDistanceCulling
      });
    }

    private EntityArchetype GetArchetype(EntityManager entityManager, Entity entity)
    {
      List<ComponentBase> list = new List<ComponentBase>();
      this.GetComponents<ComponentBase>(list);
      HashSet<ComponentType> componentTypeSet = new HashSet<ComponentType>();
      for (int index = 0; index < list.Count; ++index)
        list[index].GetArchetypeComponents(componentTypeSet);
      componentTypeSet.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet.Add(ComponentType.ReadWrite<Updated>());
      return entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet));
    }
  }
}
