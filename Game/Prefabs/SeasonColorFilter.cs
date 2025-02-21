// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SeasonColorFilter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs.Climate;
using Game.Rendering;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Rendering/", new System.Type[] {typeof (RenderPrefab)})]
  public class SeasonColorFilter : ComponentBase
  {
    public SeasonColorFilter.ColorFilter[] m_ColorFilters;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      for (int index = 0; index < this.m_ColorFilters.Length; ++index)
      {
        SeasonPrefab seasonFilter = this.m_ColorFilters[index].m_SeasonFilter;
        if ((UnityEngine.Object) seasonFilter != (UnityEngine.Object) null)
          prefabs.Add((PrefabBase) seasonFilter);
      }
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Prefabs.ColorFilter>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged1 = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: variable of a compiler-generated type
      MeshColorSystem systemManaged2 = entityManager.World.GetOrCreateSystemManaged<MeshColorSystem>();
      DynamicBuffer<Game.Prefabs.ColorFilter> buffer = entityManager.GetBuffer<Game.Prefabs.ColorFilter>(entity);
      int length = 0;
      for (int index = 0; index < this.m_ColorFilters.Length; ++index)
        length += this.m_ColorFilters[index].m_VariationGroups.Length;
      buffer.ResizeUninitialized(length);
      int num = 0;
      for (int index1 = 0; index1 < this.m_ColorFilters.Length; ++index1)
      {
        SeasonColorFilter.ColorFilter colorFilter1 = this.m_ColorFilters[index1];
        Game.Prefabs.ColorFilter colorFilter2 = new Game.Prefabs.ColorFilter()
        {
          m_AgeFilter = AgeMask.Any,
          m_GenderFilter = GenderMask.Any,
          m_OverrideProbability = (sbyte) math.clamp(colorFilter1.m_OverrideProbability, -1, 100),
          m_OverrideAlpha = (float3) -1f
        };
        if ((UnityEngine.Object) colorFilter1.m_SeasonFilter != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated method
          colorFilter2.m_EntityFilter = systemManaged1.GetEntity((PrefabBase) colorFilter1.m_SeasonFilter);
          colorFilter2.m_Flags |= ColorFilterFlags.SeasonFilter;
        }
        for (int index2 = 0; index2 < colorFilter1.m_VariationGroups.Length; ++index2)
        {
          // ISSUE: reference to a compiler-generated method
          colorFilter2.m_GroupID = systemManaged2.GetColorGroupID(colorFilter1.m_VariationGroups[index2]);
          buffer[num++] = colorFilter2;
        }
      }
    }

    [Serializable]
    public class ColorFilter
    {
      public SeasonPrefab m_SeasonFilter;
      public string[] m_VariationGroups;
      public int m_OverrideProbability = -1;
    }
  }
}
