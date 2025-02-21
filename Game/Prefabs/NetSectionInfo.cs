// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetSectionInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public class NetSectionInfo
  {
    public NetSectionPrefab m_Section;
    public NetPieceRequirements[] m_RequireAll;
    public NetPieceRequirements[] m_RequireAny;
    public NetPieceRequirements[] m_RequireNone;
    public NetPieceLayerMask m_HiddenLayers;
    public bool m_Invert;
    public bool m_Flip;
    public bool m_Median;
    public float3 m_Offset;
  }
}
