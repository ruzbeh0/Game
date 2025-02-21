// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SpawnLocation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {typeof (MarkerObjectPrefab)})]
  public class SpawnLocation : ComponentBase
  {
    public RouteConnectionType m_ConnectionType = RouteConnectionType.Pedestrian;
    public TrackTypes m_TrackTypes;
    public RoadTypes m_RoadTypes;
    public bool m_RequireAuthorization;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<SpawnLocationData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Objects.SpawnLocation>());
      if (this.m_ConnectionType != RouteConnectionType.Air)
        return;
      components.Add(ComponentType.ReadWrite<Game.Routes.TakeoffLocation>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      SpawnLocationData componentData;
      componentData.m_ConnectionType = this.m_ConnectionType;
      componentData.m_ActivityMask = new ActivityMask();
      componentData.m_TrackTypes = this.m_TrackTypes;
      componentData.m_RoadTypes = this.m_RoadTypes;
      componentData.m_RequireAuthorization = this.m_RequireAuthorization;
      entityManager.SetComponentData<SpawnLocationData>(entity, componentData);
    }
  }
}
