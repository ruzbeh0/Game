// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathfindResult
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Pathfind
{
  public struct PathfindResult
  {
    public Entity m_Origin;
    public Entity m_Destination;
    public float m_Distance;
    public float m_Duration;
    public float m_TotalCost;
    public PathMethod m_Methods;
    public int m_GraphTraversal;
    public int m_PathLength;
    public ErrorCode m_ErrorCode;
  }
}
