// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetCompositionPiece
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct NetCompositionPiece : IBufferElementData
  {
    public Entity m_Piece;
    public float3 m_Offset;
    public float3 m_Size;
    public NetSectionFlags m_SectionFlags;
    public NetPieceFlags m_PieceFlags;
    public int m_SectionIndex;
  }
}
