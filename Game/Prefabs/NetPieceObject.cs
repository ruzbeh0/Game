// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetPieceObject
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct NetPieceObject : IBufferElementData
  {
    public Entity m_Prefab;
    public float3 m_Position;
    public float3 m_Offset;
    public float3 m_Spacing;
    public float2 m_UseCurveRotation;
    public float m_MinLength;
    public int m_Probability;
    public float2 m_CurveOffsetRange;
    public quaternion m_Rotation;
    public CompositionFlags m_CompositionAll;
    public CompositionFlags m_CompositionAny;
    public CompositionFlags m_CompositionNone;
    public NetSectionFlags m_SectionAll;
    public NetSectionFlags m_SectionAny;
    public NetSectionFlags m_SectionNone;
    public SubObjectFlags m_Flags;
  }
}
