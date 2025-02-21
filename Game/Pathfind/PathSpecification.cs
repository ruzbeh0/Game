// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathSpecification
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.Pathfind
{
  public struct PathSpecification
  {
    public PathfindCosts m_Costs;
    public EdgeFlags m_Flags;
    public PathMethod m_Methods;
    public int m_AccessRequirement;
    public float m_Length;
    public float m_MaxSpeed;
    public float m_Density;
    public RuleFlags m_Rules;
    public byte m_BlockageStart;
    public byte m_BlockageEnd;
    public byte m_FlowOffset;
  }
}
