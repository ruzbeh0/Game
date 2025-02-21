// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PedestrianPathfind
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
  public class PedestrianPathfind : ComponentBase
  {
    public PathfindCostInfo m_WalkingCost = new PathfindCostInfo(0.0f, 0.0f, 0.0f, 0.01f);
    public PathfindCostInfo m_CrosswalkCost = new PathfindCostInfo(0.0f, 0.0f, 0.0f, 5f);
    public PathfindCostInfo m_UnsafeCrosswalkCost = new PathfindCostInfo(0.0f, 100f, 0.0f, 5f);
    public PathfindCostInfo m_SpawnCost = new PathfindCostInfo(5f, 0.0f, 0.0f, 0.0f);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PathfindPedestrianData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<PathfindPedestrianData>(entity, new PathfindPedestrianData()
      {
        m_WalkingCost = this.m_WalkingCost.ToPathfindCosts(),
        m_CrosswalkCost = this.m_CrosswalkCost.ToPathfindCosts(),
        m_UnsafeCrosswalkCost = this.m_UnsafeCrosswalkCost.ToPathfindCosts(),
        m_SpawnCost = this.m_SpawnCost.ToPathfindCosts()
      });
    }
  }
}
