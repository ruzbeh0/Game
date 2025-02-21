// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetSectionPiece
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct NetSectionPiece : IBufferElementData
  {
    public Entity m_Piece;
    public CompositionFlags m_CompositionAll;
    public CompositionFlags m_CompositionAny;
    public CompositionFlags m_CompositionNone;
    public NetSectionFlags m_SectionAll;
    public NetSectionFlags m_SectionAny;
    public NetSectionFlags m_SectionNone;
    public NetPieceFlags m_Flags;
    public float3 m_Offset;
  }
}
