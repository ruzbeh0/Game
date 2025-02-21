// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrafficConfigurationPrefab
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
  public class TrafficConfigurationPrefab : PrefabBase
  {
    public NotificationIconPrefab m_BottleneckNotification;
    public NotificationIconPrefab m_DeadEndNotification;
    public NotificationIconPrefab m_RoadConnectionNotification;
    public NotificationIconPrefab m_TrackConnectionNotification;
    public NotificationIconPrefab m_CarConnectionNotification;
    public NotificationIconPrefab m_ShipConnectionNotification;
    public NotificationIconPrefab m_TrainConnectionNotification;
    public NotificationIconPrefab m_PedestrianConnectionNotification;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_BottleneckNotification);
      prefabs.Add((PrefabBase) this.m_DeadEndNotification);
      prefabs.Add((PrefabBase) this.m_RoadConnectionNotification);
      prefabs.Add((PrefabBase) this.m_TrackConnectionNotification);
      prefabs.Add((PrefabBase) this.m_CarConnectionNotification);
      prefabs.Add((PrefabBase) this.m_ShipConnectionNotification);
      prefabs.Add((PrefabBase) this.m_TrainConnectionNotification);
      prefabs.Add((PrefabBase) this.m_PedestrianConnectionNotification);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<TrafficConfigurationData>());
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<TrafficConfigurationData>(entity, new TrafficConfigurationData()
      {
        m_BottleneckNotification = systemManaged.GetEntity((PrefabBase) this.m_BottleneckNotification),
        m_DeadEndNotification = systemManaged.GetEntity((PrefabBase) this.m_DeadEndNotification),
        m_RoadConnectionNotification = systemManaged.GetEntity((PrefabBase) this.m_RoadConnectionNotification),
        m_TrackConnectionNotification = systemManaged.GetEntity((PrefabBase) this.m_TrackConnectionNotification),
        m_CarConnectionNotification = systemManaged.GetEntity((PrefabBase) this.m_CarConnectionNotification),
        m_ShipConnectionNotification = systemManaged.GetEntity((PrefabBase) this.m_ShipConnectionNotification),
        m_TrainConnectionNotification = systemManaged.GetEntity((PrefabBase) this.m_TrainConnectionNotification),
        m_PedestrianConnectionNotification = systemManaged.GetEntity((PrefabBase) this.m_PedestrianConnectionNotification)
      });
    }
  }
}
