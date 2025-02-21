// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrackPathfind
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Pathfind/", new Type[] {typeof (PathfindPrefab)})]
  public class TrackPathfind : ComponentBase
  {
    public PathfindCostInfo m_DrivingCost = new PathfindCostInfo(0.0f, 0.0f, 0.01f, 0.0f);
    public PathfindCostInfo m_TwowayCost = new PathfindCostInfo(0.0f, 0.0f, 0.0f, 5f);
    public PathfindCostInfo m_SwitchCost = new PathfindCostInfo(0.0f, 0.0f, 0.0f, 2f);
    public PathfindCostInfo m_DiamondCrossingCost = new PathfindCostInfo(0.0f, 0.0f, 0.0f, 2f);
    public PathfindCostInfo m_CurveAngleCost = new PathfindCostInfo(2f, 0.0f, 0.0f, 3f);
    public PathfindCostInfo m_SpawnCost = new PathfindCostInfo(5f, 0.0f, 0.0f, 0.0f);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PathfindTrackData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<PathfindTrackData>(entity, new PathfindTrackData()
      {
        m_DrivingCost = this.m_DrivingCost.ToPathfindCosts(),
        m_TwowayCost = this.m_TwowayCost.ToPathfindCosts(),
        m_SwitchCost = this.m_SwitchCost.ToPathfindCosts(),
        m_DiamondCrossingCost = this.m_DiamondCrossingCost.ToPathfindCosts(),
        m_CurveAngleCost = this.m_CurveAngleCost.ToPathfindCosts(),
        m_SpawnCost = this.m_SpawnCost.ToPathfindCosts()
      });
    }
  }
}
