// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.RouteData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Routes;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  public struct RouteData : IComponentData, IQueryTypeParameter
  {
    public EntityArchetype m_RouteArchetype;
    public EntityArchetype m_WaypointArchetype;
    public EntityArchetype m_ConnectedArchetype;
    public EntityArchetype m_SegmentArchetype;
    public float m_SnapDistance;
    public RouteType m_Type;
    public Color32 m_Color;
    public float m_Width;
    public float m_SegmentLength;
  }
}
