// Decompiled with JetBrains decompiler
// Type: Game.Tools.BrushDefinition
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  public struct BrushDefinition : IComponentData, IQueryTypeParameter
  {
    public Entity m_Tool;
    public Line3.Segment m_Line;
    public float m_Angle;
    public float m_Size;
    public float m_Strength;
    public float m_Time;
    public float3 m_Target;
    public float3 m_Start;
  }
}
