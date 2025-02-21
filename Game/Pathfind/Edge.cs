// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.Edge
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Pathfind
{
  public struct Edge
  {
    public Entity m_Owner;
    public NodeID m_StartID;
    public NodeID m_MiddleID;
    public NodeID m_EndID;
    public float m_StartCurvePos;
    public float m_EndCurvePos;
    public PathSpecification m_Specification;
    public LocationSpecification m_Location;
  }
}
