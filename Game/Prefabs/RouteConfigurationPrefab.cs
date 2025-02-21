// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.RouteConfigurationPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new Type[] {})]
  public class RouteConfigurationPrefab : PrefabBase
  {
    public NotificationIconPrefab m_PathfindNotification;
    public RoutePrefab m_CarPathVisualization;
    public RoutePrefab m_WatercraftPathVisualization;
    public RoutePrefab m_AircraftPathVisualization;
    public RoutePrefab m_TrainPathVisualization;
    public RoutePrefab m_HumanPathVisualization;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_PathfindNotification);
      prefabs.Add((PrefabBase) this.m_CarPathVisualization);
      prefabs.Add((PrefabBase) this.m_WatercraftPathVisualization);
      prefabs.Add((PrefabBase) this.m_AircraftPathVisualization);
      prefabs.Add((PrefabBase) this.m_TrainPathVisualization);
      prefabs.Add((PrefabBase) this.m_HumanPathVisualization);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<RouteConfigurationData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<RouteConfigurationData>(entity, new RouteConfigurationData()
      {
        m_PathfindNotification = systemManaged.GetEntity((PrefabBase) this.m_PathfindNotification),
        m_CarPathVisualization = systemManaged.GetEntity((PrefabBase) this.m_CarPathVisualization),
        m_WatercraftPathVisualization = systemManaged.GetEntity((PrefabBase) this.m_WatercraftPathVisualization),
        m_AircraftPathVisualization = systemManaged.GetEntity((PrefabBase) this.m_AircraftPathVisualization),
        m_TrainPathVisualization = systemManaged.GetEntity((PrefabBase) this.m_TrainPathVisualization),
        m_HumanPathVisualization = systemManaged.GetEntity((PrefabBase) this.m_HumanPathVisualization)
      });
    }
  }
}
