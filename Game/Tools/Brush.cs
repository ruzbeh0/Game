// Decompiled with JetBrains decompiler
// Type: Game.Tools.Brush
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  public struct Brush : IComponentData, IQueryTypeParameter
  {
    public Entity m_Tool;
    public float3 m_Position;
    public float3 m_Target;
    public float3 m_Start;
    public float m_Angle;
    public float m_Size;
    public float m_Strength;
    public float m_Opacity;
  }
}
