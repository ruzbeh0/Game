// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ConnectionPathfind
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
  public class ConnectionPathfind : ComponentBase
  {
    public PathfindCostInfo m_BorderCost = new PathfindCostInfo(10f, 0.0f, 10f, 0.0f);
    public PathfindCostInfo m_PedestrianBorderCost = new PathfindCostInfo(10f, 0.0f, 10f, 0.0f);
    public PathfindCostInfo m_DistanceCost = new PathfindCostInfo(0.01f, 0.0f, 0.01f, 0.0f);
    public PathfindCostInfo m_AirwayCost = new PathfindCostInfo(0.0f, 0.0f, 0.02f, 0.0f);
    public PathfindCostInfo m_InsideCost = new PathfindCostInfo(0.01f, 0.0f, 0.01f, 0.0f);
    public PathfindCostInfo m_AreaCost = new PathfindCostInfo(0.0f, 0.0f, 0.0f, 0.0f);
    public PathfindCostInfo m_CarSpawnCost = new PathfindCostInfo(5f, 0.0f, 0.0f, 0.0f);
    public PathfindCostInfo m_PedestrianSpawnCost = new PathfindCostInfo(5f, 0.0f, 0.0f, 0.0f);
    public PathfindCostInfo m_HelicopterTakeoffCost = new PathfindCostInfo(5f, 0.0f, 0.0f, 0.0f);
    public PathfindCostInfo m_AirplaneTakeoffCost = new PathfindCostInfo(5f, 0.0f, 0.0f, 0.0f);
    public PathfindCostInfo m_TaxiStartCost = new PathfindCostInfo(5f, 0.0f, 0.0f, 0.0f);
    public PathfindCostInfo m_ParkingCost = new PathfindCostInfo(10f, 0.0f, 0.0f, 0.0f);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PathfindConnectionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<PathfindConnectionData>(entity, new PathfindConnectionData()
      {
        m_BorderCost = this.m_BorderCost.ToPathfindCosts(),
        m_PedestrianBorderCost = this.m_PedestrianBorderCost.ToPathfindCosts(),
        m_DistanceCost = this.m_DistanceCost.ToPathfindCosts(),
        m_AirwayCost = this.m_AirwayCost.ToPathfindCosts(),
        m_InsideCost = this.m_InsideCost.ToPathfindCosts(),
        m_AreaCost = this.m_AreaCost.ToPathfindCosts(),
        m_CarSpawnCost = this.m_CarSpawnCost.ToPathfindCosts(),
        m_PedestrianSpawnCost = this.m_PedestrianSpawnCost.ToPathfindCosts(),
        m_HelicopterTakeoffCost = this.m_HelicopterTakeoffCost.ToPathfindCosts(),
        m_AirplaneTakeoffCost = this.m_AirplaneTakeoffCost.ToPathfindCosts(),
        m_TaxiStartCost = this.m_TaxiStartCost.ToPathfindCosts(),
        m_ParkingCost = this.m_ParkingCost.ToPathfindCosts()
      });
    }
  }
}
