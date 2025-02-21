// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TakeoffLocation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using Game.Routes;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Routes/", new Type[] {typeof (MarkerObjectPrefab)})]
  public class TakeoffLocation : ComponentBase
  {
    public RouteConnectionType m_ConnectionType1 = RouteConnectionType.Road;
    public RouteConnectionType m_ConnectionType2 = RouteConnectionType.Air;
    public RoadTypes m_RoadType;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<RouteConnectionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Routes.TakeoffLocation>());
      components.Add(ComponentType.ReadWrite<AccessLane>());
      components.Add(ComponentType.ReadWrite<RouteLane>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      RouteConnectionData componentData;
      componentData.m_AccessConnectionType = this.m_ConnectionType1;
      componentData.m_RouteConnectionType = this.m_ConnectionType2;
      componentData.m_AccessTrackType = TrackTypes.None;
      componentData.m_RouteTrackType = TrackTypes.None;
      componentData.m_AccessRoadType = this.m_RoadType;
      componentData.m_RouteRoadType = this.m_RoadType;
      componentData.m_StartLaneOffset = 0.0f;
      componentData.m_EndMargin = 0.0f;
      entityManager.SetComponentData<RouteConnectionData>(entity, componentData);
    }
  }
}
