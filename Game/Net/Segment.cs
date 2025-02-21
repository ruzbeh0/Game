// Decompiled with JetBrains decompiler
// Type: Game.Net.Segment
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  public struct Segment
  {
    public Bezier4x3 m_Left;
    public Bezier4x3 m_Right;
    public float2 m_Length;

    public float middleLength => math.lerp(this.m_Length.x, this.m_Length.y, 0.5f);
  }
}
