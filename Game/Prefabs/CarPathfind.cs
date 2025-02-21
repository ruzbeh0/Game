// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CarPathfind
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
  public class CarPathfind : ComponentBase
  {
    public PathfindCostInfo m_DrivingCost = new PathfindCostInfo(0.0f, 0.0f, 0.01f, 0.0f);
    public PathfindCostInfo m_TurningCost = new PathfindCostInfo(0.0f, 0.0f, 0.0f, 1f);
    public PathfindCostInfo m_UTurnCost = new PathfindCostInfo(0.0f, 0.0f, 0.0f, 10f);
    public PathfindCostInfo m_UnsafeUTurnCost = new PathfindCostInfo(0.0f, 50f, 0.0f, 10f);
    public PathfindCostInfo m_CurveAngleCost = new PathfindCostInfo(2f, 0.0f, 0.0f, 3f);
    public PathfindCostInfo m_LaneCrossCost = new PathfindCostInfo(0.0f, 0.0f, 0.0f, 2f);
    public PathfindCostInfo m_ParkingCost = new PathfindCostInfo(10f, 0.0f, 0.0f, 0.0f);
    public PathfindCostInfo m_SpawnCost = new PathfindCostInfo(5f, 0.0f, 0.0f, 0.0f);
    public PathfindCostInfo m_ForbiddenCost = new PathfindCostInfo(10f, 100f, 20f, 50f);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PathfindCarData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<PathfindCarData>(entity, new PathfindCarData()
      {
        m_DrivingCost = this.m_DrivingCost.ToPathfindCosts(),
        m_TurningCost = this.m_TurningCost.ToPathfindCosts(),
        m_UTurnCost = this.m_UTurnCost.ToPathfindCosts(),
        m_UnsafeUTurnCost = this.m_UnsafeUTurnCost.ToPathfindCosts(),
        m_CurveAngleCost = this.m_CurveAngleCost.ToPathfindCosts(),
        m_LaneCrossCost = this.m_LaneCrossCost.ToPathfindCosts(),
        m_ParkingCost = this.m_ParkingCost.ToPathfindCosts(),
        m_SpawnCost = this.m_SpawnCost.ToPathfindCosts(),
        m_ForbiddenCost = this.m_ForbiddenCost.ToPathfindCosts()
      });
    }
  }
}
