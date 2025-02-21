// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.FixedNetSegmentInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public class FixedNetSegmentInfo
  {
    public int2 m_CountRange;
    public float m_Length;
    public bool m_CanCurve;
    public NetPieceRequirements[] m_SetState;
    public NetPieceRequirements[] m_UnsetState;
  }
}
