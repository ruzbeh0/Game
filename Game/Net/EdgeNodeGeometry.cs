// Decompiled with JetBrains decompiler
// Type: Game.Net.EdgeNodeGeometry
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  public struct EdgeNodeGeometry
  {
    public Segment m_Left;
    public Segment m_Right;
    public Bezier4x3 m_Middle;
    public Bounds3 m_Bounds;
    public float4 m_SyncVertexTargetsLeft;
    public float4 m_SyncVertexTargetsRight;
    public float m_MiddleRadius;
  }
}
