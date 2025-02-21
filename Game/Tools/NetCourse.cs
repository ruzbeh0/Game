// Decompiled with JetBrains decompiler
// Type: Game.Tools.NetCourse
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  public struct NetCourse : IComponentData, IQueryTypeParameter
  {
    public CoursePos m_StartPosition;
    public CoursePos m_EndPosition;
    public Bezier4x3 m_Curve;
    public float2 m_Elevation;
    public float m_Length;
    public int m_FixedIndex;
  }
}
