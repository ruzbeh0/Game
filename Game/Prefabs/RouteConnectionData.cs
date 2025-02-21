// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.RouteConnectionData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct RouteConnectionData : IComponentData, IQueryTypeParameter
  {
    public RouteConnectionType m_AccessConnectionType;
    public RouteConnectionType m_RouteConnectionType;
    public TrackTypes m_AccessTrackType;
    public TrackTypes m_RouteTrackType;
    public RoadTypes m_AccessRoadType;
    public RoadTypes m_RouteRoadType;
    public float m_StartLaneOffset;
    public float m_EndMargin;
  }
}
