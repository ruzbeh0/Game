// Decompiled with JetBrains decompiler
// Type: Game.Tools.SnapLine
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;

#nullable disable
namespace Game.Tools
{
  public struct SnapLine
  {
    public ControlPoint m_ControlPoint;
    public Bezier4x3 m_Curve;
    public SnapLineFlags m_Flags;

    public SnapLine(ControlPoint position, Bezier4x3 curve, SnapLineFlags flags)
    {
      this.m_ControlPoint = position;
      this.m_Curve = curve;
      this.m_Flags = flags;
    }
  }
}
