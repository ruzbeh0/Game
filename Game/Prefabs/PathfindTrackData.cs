// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PathfindTrackData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Pathfind;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PathfindTrackData : IComponentData, IQueryTypeParameter
  {
    public PathfindCosts m_DrivingCost;
    public PathfindCosts m_TwowayCost;
    public PathfindCosts m_SwitchCost;
    public PathfindCosts m_DiamondCrossingCost;
    public PathfindCosts m_CurveAngleCost;
    public PathfindCosts m_SpawnCost;
  }
}
