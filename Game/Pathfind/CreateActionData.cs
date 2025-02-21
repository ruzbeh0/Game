// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.CreateActionData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Pathfind
{
  public struct CreateActionData
  {
    public Entity m_Owner;
    public PathNode m_StartNode;
    public PathNode m_MiddleNode;
    public PathNode m_EndNode;
    public PathNode m_SecondaryStartNode;
    public PathNode m_SecondaryEndNode;
    public PathSpecification m_Specification;
    public PathSpecification m_SecondarySpecification;
    public LocationSpecification m_Location;
  }
}
