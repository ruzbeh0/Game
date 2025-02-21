// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MailBox
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Net;
using Game.Routes;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Routes/", new Type[] {typeof (ObjectPrefab)})]
  public class MailBox : ComponentBase
  {
    public int m_MailCapacity = 1000;
    public float m_ComfortFactor;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<MailBoxData>());
      components.Add(ComponentType.ReadWrite<TransportStopData>());
      components.Add(ComponentType.ReadWrite<RouteConnectionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Objects.Color>());
      components.Add(ComponentType.ReadWrite<Game.Routes.TransportStop>());
      components.Add(ComponentType.ReadWrite<Game.Routes.MailBox>());
      components.Add(ComponentType.ReadWrite<AccessLane>());
      components.Add(ComponentType.ReadWrite<RouteLane>());
      components.Add(ComponentType.ReadWrite<CurrentDistrict>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      RouteConnectionData componentData1;
      componentData1.m_AccessConnectionType = RouteConnectionType.Pedestrian;
      componentData1.m_RouteConnectionType = RouteConnectionType.Road;
      componentData1.m_AccessTrackType = TrackTypes.None;
      componentData1.m_RouteTrackType = TrackTypes.None;
      componentData1.m_AccessRoadType = RoadTypes.None;
      componentData1.m_RouteRoadType = RoadTypes.Car;
      componentData1.m_StartLaneOffset = 0.0f;
      componentData1.m_EndMargin = 0.0f;
      TransportStopData componentData2;
      componentData2.m_ComfortFactor = this.m_ComfortFactor;
      componentData2.m_LoadingFactor = 0.0f;
      componentData2.m_AccessDistance = 0.0f;
      componentData2.m_BoardingTime = 0.0f;
      componentData2.m_TransportType = TransportType.Post;
      componentData2.m_PassengerTransport = false;
      componentData2.m_CargoTransport = true;
      MailBoxData componentData3;
      componentData3.m_MailCapacity = this.m_MailCapacity;
      entityManager.SetComponentData<RouteConnectionData>(entity, componentData1);
      entityManager.SetComponentData<TransportStopData>(entity, componentData2);
      entityManager.SetComponentData<MailBoxData>(entity, componentData3);
    }
  }
}
