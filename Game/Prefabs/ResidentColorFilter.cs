// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ResidentColorFilter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Rendering;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Rendering/", new System.Type[] {typeof (RenderPrefab), typeof (CharacterOverlay)})]
  public class ResidentColorFilter : ComponentBase
  {
    public ResidentColorFilter.ColorFilter[] m_ColorFilters;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Prefabs.ColorFilter>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      MeshColorSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<MeshColorSystem>();
      DynamicBuffer<Game.Prefabs.ColorFilter> buffer = entityManager.GetBuffer<Game.Prefabs.ColorFilter>(entity);
      int length = 0;
      for (int index = 0; index < this.m_ColorFilters.Length; ++index)
        length += this.m_ColorFilters[index].m_VariationGroups.Length;
      buffer.ResizeUninitialized(length);
      int num = 0;
      ColorProperties component = this.GetComponent<ColorProperties>();
      for (int index1 = 0; index1 < this.m_ColorFilters.Length; ++index1)
      {
        ResidentColorFilter.ColorFilter colorFilter1 = this.m_ColorFilters[index1];
        Game.Prefabs.ColorFilter colorFilter2 = new Game.Prefabs.ColorFilter()
        {
          m_AgeFilter = colorFilter1.m_AgeFilter,
          m_GenderFilter = colorFilter1.m_GenderFilter,
          m_OverrideProbability = (sbyte) math.clamp(colorFilter1.m_OverrideProbability, -1, 100),
          m_OverrideAlpha = (float3) -1f
        };
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          float3 alphas = math.select(math.saturate(colorFilter1.m_OverrideAlpha), (float3) -1f, colorFilter1.m_OverrideAlpha < 0.0f);
          colorFilter2.m_OverrideAlpha.x = component.GetAlpha(alphas, (sbyte) 0, -1f);
          colorFilter2.m_OverrideAlpha.y = component.GetAlpha(alphas, (sbyte) 1, -1f);
          colorFilter2.m_OverrideAlpha.z = component.GetAlpha(alphas, (sbyte) 2, -1f);
        }
        for (int index2 = 0; index2 < colorFilter1.m_VariationGroups.Length; ++index2)
        {
          // ISSUE: reference to a compiler-generated method
          colorFilter2.m_GroupID = systemManaged.GetColorGroupID(colorFilter1.m_VariationGroups[index2]);
          buffer[num++] = colorFilter2;
        }
      }
    }

    [Serializable]
    public class ColorFilter
    {
      public AgeMask m_AgeFilter;
      public GenderMask m_GenderFilter;
      public string[] m_VariationGroups;
      public int m_OverrideProbability = -1;
      public float3 m_OverrideAlpha = (float3) -1f;
    }
  }
}
