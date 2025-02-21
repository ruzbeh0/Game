// Decompiled with JetBrains decompiler
// Type: Game.Common.RaycastInput
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Areas;
using Game.Net;
using Game.Notifications;
using Game.Prefabs;
using Game.Routes;
using Unity.Mathematics;

#nullable disable
namespace Game.Common
{
  public struct RaycastInput
  {
    public Line3.Segment m_Line;
    public float3 m_Offset;
    public TypeMask m_TypeMask;
    public RaycastFlags m_Flags;
    public CollisionMask m_CollisionMask;
    public Layer m_NetLayerMask;
    public AreaTypeMask m_AreaTypeMask;
    public RouteType m_RouteType;
    public TransportType m_TransportType;
    public IconLayerMask m_IconLayerMask;
    public UtilityTypes m_UtilityTypeMask;

    public bool IsDisabled()
    {
      return (this.m_Flags & (RaycastFlags.DebugDisable | RaycastFlags.UIDisable | RaycastFlags.ToolDisable | RaycastFlags.FreeCameraDisable)) > (RaycastFlags) 0;
    }
  }
}
