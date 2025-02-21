// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PathfindPedestrianData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Pathfind;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PathfindPedestrianData : IComponentData, IQueryTypeParameter
  {
    public PathfindCosts m_WalkingCost;
    public PathfindCosts m_CrosswalkCost;
    public PathfindCosts m_UnsafeCrosswalkCost;
    public PathfindCosts m_SpawnCost;
  }
}
