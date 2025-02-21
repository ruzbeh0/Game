// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SecondaryLaneData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct SecondaryLaneData : IComponentData, IQueryTypeParameter
  {
    public SecondaryLaneDataFlags m_Flags;
    public float3 m_PositionOffset;
    public float2 m_LengthOffset;
    public float m_CutMargin;
    public float m_CutOffset;
    public float m_CutOverlap;
    public float m_Spacing;
  }
}
