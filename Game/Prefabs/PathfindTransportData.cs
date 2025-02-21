// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PathfindTransportData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Pathfind;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PathfindTransportData : IComponentData, IQueryTypeParameter
  {
    public PathfindCosts m_OrderingCost;
    public PathfindCosts m_StartingCost;
    public PathfindCosts m_TravelCost;
  }
}
