// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CharacterGroup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Objects;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public class CharacterGroup : RenderPrefabBase
  {
    public CharacterGroup.Character[] m_Characters;
    public CharacterGroup.OverrideInfo[] m_Overrides;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      foreach (CharacterGroup.Character character in this.m_Characters)
      {
        prefabs.Add((PrefabBase) character.m_Style);
        foreach (RenderPrefab meshPrefab in character.m_MeshPrefabs)
          prefabs.Add((PrefabBase) meshPrefab);
      }
      if (this.m_Overrides == null)
        return;
      foreach (CharacterGroup.OverrideInfo overrideInfo in this.m_Overrides)
        prefabs.Add((PrefabBase) overrideInfo.m_Group);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<CharacterGroupData>());
    }

    [Serializable]
    public class Character
    {
      public CharacterStyle m_Style;
      public CharacterGroup.Meta m_Meta;
      public RenderPrefab[] m_MeshPrefabs;
    }

    [Serializable]
    public class OverrideInfo
    {
      public CharacterGroup m_Group;
      public ObjectState m_RequireState;
      public CharacterProperties.BodyPart m_OverrideBodyParts;
      public bool m_OverrideShapeWeights;
    }

    [Serializable]
    public struct IndexWeight
    {
      public int index;
      public float weight;
    }

    [Serializable]
    public struct IndexWeight8
    {
      public CharacterGroup.IndexWeight w0;
      public CharacterGroup.IndexWeight w1;
      public CharacterGroup.IndexWeight w2;
      public CharacterGroup.IndexWeight w3;
      public CharacterGroup.IndexWeight w4;
      public CharacterGroup.IndexWeight w5;
      public CharacterGroup.IndexWeight w6;
      public CharacterGroup.IndexWeight w7;
    }

    [Serializable]
    public struct Meta
    {
      public CharacterGroup.IndexWeight8 shapeWeights;
      public CharacterGroup.IndexWeight8 textureWeights;
      public CharacterGroup.IndexWeight8 overlayWeights;
      public CharacterGroup.IndexWeight8 maskWeights;
    }
  }
}
