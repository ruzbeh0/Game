// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AreasConfigurationPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.Mathematics;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class AreasConfigurationPrefab : PrefabBase
  {
    [NotNull]
    public AreaPrefab m_DefaultDistrictPrefab;
    [Tooltip("Maximum slope that is considered buildable land, for display in the map selection screen.\nMin and below: Fully buildable, Max and above: Not buildable")]
    public Bounds1 m_BuildableLandMaxSlope = new Bounds1(0.1f, 0.3f);

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_DefaultDistrictPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<AreasConfigurationData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<AreasConfigurationData>(entity, new AreasConfigurationData()
      {
        m_DefaultDistrictPrefab = existingSystemManaged.GetEntity((PrefabBase) this.m_DefaultDistrictPrefab),
        m_BuildableLandMaxSlope = this.m_BuildableLandMaxSlope
      });
    }
  }
}
