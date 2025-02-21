// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetPieceInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public class NetPieceInfo
  {
    public NetPiecePrefab m_Piece;
    public NetPieceRequirements[] m_RequireAll;
    public NetPieceRequirements[] m_RequireAny;
    public NetPieceRequirements[] m_RequireNone;
    public float3 m_Offset;
  }
}
