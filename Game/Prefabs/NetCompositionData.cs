// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetCompositionData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct NetCompositionData : IComponentData, IQueryTypeParameter
  {
    public float4 m_SyncVertexOffsetsLeft;
    public float4 m_SyncVertexOffsetsRight;
    public Bounds1 m_HeightRange;
    public Bounds1 m_SurfaceHeight;
    public float4 m_EdgeHeights;
    public float2 m_RoundaboutSize;
    public CompositionFlags m_Flags;
    public CompositionState m_State;
    public float m_Width;
    public float m_MiddleOffset;
    public float m_WidthOffset;
    public float m_NodeOffset;
    public int m_MinLod;
  }
}
