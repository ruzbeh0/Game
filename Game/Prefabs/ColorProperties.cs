// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ColorProperties
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Rendering;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Rendering/", new System.Type[] {typeof (RenderPrefab), typeof (CharacterOverlay)})]
  public class ColorProperties : ComponentBase
  {
    public List<ColorProperties.VariationSet> m_ColorVariations = new List<ColorProperties.VariationSet>();
    [FixedLength]
    public List<ColorProperties.ColorChannelBinding> m_ChannelsBinding = new List<ColorProperties.ColorChannelBinding>()
    {
      new ColorProperties.ColorChannelBinding()
      {
        m_ChannelId = (sbyte) 0
      },
      new ColorProperties.ColorChannelBinding()
      {
        m_ChannelId = (sbyte) 1
      },
      new ColorProperties.ColorChannelBinding()
      {
        m_ChannelId = (sbyte) 2
      }
    };
    public List<ColorProperties.VariationGroup> m_VariationGroups;
    public int3 m_VariationRanges = new int3(5, 5, 5);
    public int3 m_AlphaRanges = new int3(0, 0, 0);
    public ColorSourceType m_ExternalColorSource;

    public bool SanityCheck(sbyte channel)
    {
      return this.m_ChannelsBinding != null && channel >= (sbyte) 0 && (int) channel < this.m_ChannelsBinding.Count;
    }

    public bool CanBeModifiedByExternal(sbyte channel)
    {
      return !this.SanityCheck(channel) || this.m_ChannelsBinding[(int) channel].m_CanBeModifiedByExternal;
    }

    public Color GetColor(int index, sbyte channel)
    {
      if (!this.SanityCheck(channel) || this.m_ColorVariations.Count <= 0)
        return Color.white;
      index %= this.m_ColorVariations.Count;
      return this.m_ColorVariations[index].m_Colors[(int) this.m_ChannelsBinding[(int) channel].m_ChannelId];
    }

    public int GetAlpha(int3 alphas, sbyte channel, int def)
    {
      return this.SanityCheck(channel) ? alphas[(int) this.m_ChannelsBinding[(int) channel].m_ChannelId] : def;
    }

    public float GetAlpha(float3 alphas, sbyte channel, float def)
    {
      return this.SanityCheck(channel) ? alphas[(int) this.m_ChannelsBinding[(int) channel].m_ChannelId] : def;
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ColorVariation>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      MeshColorSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<MeshColorSystem>();
      // ISSUE: reference to a compiler-generated method
      ColorVariation colorVariation1 = new ColorVariation()
      {
        m_GroupID = systemManaged.GetColorGroupID((string) null),
        m_SyncFlags = ColorSyncFlags.None,
        m_ColorSourceType = this.m_ExternalColorSource,
        m_Probability = 100
      };
      for (int index = 0; index < 3; ++index)
      {
        if (this.CanBeModifiedByExternal((sbyte) index))
          colorVariation1.SetExternalChannelIndex(index, (int) this.m_ChannelsBinding[index].m_ChannelId);
        else
          colorVariation1.SetExternalChannelIndex(index, -1);
      }
      int3 int3_1 = math.clamp(this.m_VariationRanges, (int3) 0, (int3) 100);
      int3 alphas1 = math.clamp(this.m_AlphaRanges, (int3) 0, (int3) 100);
      colorVariation1.m_HueRange = (byte) int3_1.x;
      colorVariation1.m_SaturationRange = (byte) int3_1.y;
      colorVariation1.m_ValueRange = (byte) int3_1.z;
      colorVariation1.m_AlphaRange0 = (byte) this.GetAlpha(alphas1, (sbyte) 0, 0);
      colorVariation1.m_AlphaRange1 = (byte) this.GetAlpha(alphas1, (sbyte) 1, 0);
      colorVariation1.m_AlphaRange2 = (byte) this.GetAlpha(alphas1, (sbyte) 2, 0);
      DynamicBuffer<ColorVariation> buffer = entityManager.GetBuffer<ColorVariation>(entity);
      buffer.ResizeUninitialized(this.m_ColorVariations.Count);
      int num = 0;
      bool flag = false;
      if (this.m_VariationGroups != null)
      {
        for (int index1 = 0; index1 < this.m_VariationGroups.Count; ++index1)
        {
          ColorProperties.VariationGroup variationGroup = this.m_VariationGroups[index1];
          flag |= string.IsNullOrEmpty(variationGroup.m_Name);
          // ISSUE: reference to a compiler-generated method
          ColorVariation colorVariation2 = colorVariation1 with
          {
            m_GroupID = systemManaged.GetColorGroupID(variationGroup.m_Name),
            m_SyncFlags = variationGroup.m_MeshSyncMode,
            m_Probability = (byte) math.clamp(variationGroup.m_Probability, 0, 100)
          };
          if (variationGroup.m_OverrideRandomness)
          {
            int3 int3_2 = math.clamp(variationGroup.m_VariationRanges, (int3) 0, (int3) 100);
            int3 alphas2 = math.clamp(variationGroup.m_AlphaRanges, (int3) 0, (int3) 100);
            colorVariation2.m_HueRange = (byte) int3_2.x;
            colorVariation2.m_SaturationRange = (byte) int3_2.y;
            colorVariation2.m_ValueRange = (byte) int3_2.z;
            colorVariation2.m_AlphaRange0 = (byte) this.GetAlpha(alphas2, (sbyte) 0, 0);
            colorVariation2.m_AlphaRange1 = (byte) this.GetAlpha(alphas2, (sbyte) 1, 0);
            colorVariation2.m_AlphaRange2 = (byte) this.GetAlpha(alphas2, (sbyte) 2, 0);
          }
          for (int index2 = 0; index2 < this.m_ColorVariations.Count; ++index2)
          {
            if (this.m_ColorVariations[index2].m_VariationGroup == variationGroup.m_Name)
            {
              ColorVariation colorVariation3 = colorVariation2;
              for (int index3 = 0; index3 < 3; ++index3)
                colorVariation3.m_ColorSet[index3] = this.GetColor(index2, (sbyte) index3);
              buffer[num++] = colorVariation3;
            }
          }
        }
      }
      if (flag)
        return;
      for (int index4 = 0; index4 < this.m_ColorVariations.Count; ++index4)
      {
        if (string.IsNullOrEmpty(this.m_ColorVariations[index4].m_VariationGroup))
        {
          ColorVariation colorVariation4 = colorVariation1;
          for (int index5 = 0; index5 < 3; ++index5)
            colorVariation4.m_ColorSet[index5] = this.GetColor(index4, (sbyte) index5);
          buffer[num++] = colorVariation4;
        }
      }
    }

    [Serializable]
    public class VariationSet
    {
      [FixedLength]
      [ColorUsage(true)]
      public Color[] m_Colors = new Color[3];
      public string m_VariationGroup;
    }

    [Serializable]
    public class ColorChannelBinding
    {
      public sbyte m_ChannelId;
      public bool m_CanBeModifiedByExternal;
    }

    [Serializable]
    public class VariationGroup
    {
      public string m_Name;
      [Range(0.0f, 100f)]
      public int m_Probability = 100;
      public ColorSyncFlags m_MeshSyncMode;
      public bool m_OverrideRandomness;
      public int3 m_VariationRanges = new int3(5, 5, 5);
      public int3 m_AlphaRanges = new int3(0, 0, 0);
    }
  }
}
