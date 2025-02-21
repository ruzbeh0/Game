// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.EffectColorData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  public struct EffectColorData : IComponentData, IQueryTypeParameter
  {
    public Color m_Color;
    public EffectColorSource m_Source;
    public float3 m_VaritationRanges;
  }
}
