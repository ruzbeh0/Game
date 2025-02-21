// Decompiled with JetBrains decompiler
// Type: Game.Tools.ToolRaycastSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Areas;
using Game.Common;
using Game.Input;
using Game.Net;
using Game.Notifications;
using Game.Prefabs;
using Game.Rendering;
using Game.Routes;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tools
{
  public class ToolRaycastSystem : GameSystemBase
  {
    private ToolSystem m_ToolSystem;
    private RaycastSystem m_RaycastSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;

    public RaycastFlags raycastFlags { get; set; }

    public TypeMask typeMask { get; set; }

    public CollisionMask collisionMask { get; set; }

    public Layer netLayerMask { get; set; }

    public AreaTypeMask areaTypeMask { get; set; }

    public RouteType routeType { get; set; }

    public TransportType transportType { get; set; }

    public IconLayerMask iconLayerMask { get; set; }

    public UtilityTypes utilityTypeMask { get; set; }

    public float3 rayOffset { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      this.m_RaycastSystem = this.World.GetOrCreateSystemManaged<RaycastSystem>();
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
    }

    public bool GetRaycastResult(out RaycastResult result)
    {
      // ISSUE: reference to a compiler-generated method
      NativeArray<RaycastResult> result1 = this.m_RaycastSystem.GetResult((object) this);
      if (result1.Length != 0)
      {
        result = result1[0];
        return result.m_Owner != Entity.Null;
      }
      result = new RaycastResult();
      return false;
    }

    public static Line3.Segment CalculateRaycastLine(Camera mainCamera)
    {
      Ray ray = mainCamera.ScreenPointToRay(InputManager.instance.mousePosition);
      float3 direction = (float3) ray.direction;
      float3 forward = (float3) mainCamera.transform.forward;
      Line3.Segment raycastLine;
      raycastLine.a = (float3) ray.origin;
      raycastLine.b = raycastLine.a + direction * (mainCamera.farClipPlane / math.clamp(math.dot(direction, forward), 0.25f, 1f));
      return raycastLine;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      if (this.m_ToolSystem.activeTool != null)
      {
        // ISSUE: reference to a compiler-generated method
        this.m_ToolSystem.activeTool.InitializeRaycast();
      }
      Viewer viewer;
      // ISSUE: reference to a compiler-generated method
      if (!this.m_CameraUpdateSystem.TryGetViewer(out viewer))
        return;
      if (this.m_ToolSystem.fullUpdateRequired)
        this.raycastFlags |= RaycastFlags.ToolDisable;
      else
        this.raycastFlags &= ~RaycastFlags.ToolDisable;
      if (InputManager.instance.controlOverWorld)
        this.raycastFlags &= ~RaycastFlags.UIDisable;
      else
        this.raycastFlags |= RaycastFlags.UIDisable;
      // ISSUE: reference to a compiler-generated method
      this.m_RaycastSystem.AddInput((object) this, new RaycastInput()
      {
        m_Line = ToolRaycastSystem.CalculateRaycastLine(viewer.camera),
        m_Offset = this.rayOffset,
        m_TypeMask = this.typeMask,
        m_Flags = this.raycastFlags,
        m_CollisionMask = this.collisionMask,
        m_NetLayerMask = this.netLayerMask,
        m_AreaTypeMask = this.areaTypeMask,
        m_RouteType = this.routeType,
        m_TransportType = this.transportType,
        m_IconLayerMask = this.iconLayerMask,
        m_UtilityTypeMask = this.utilityTypeMask
      });
    }

    [Preserve]
    public ToolRaycastSystem()
    {
    }
  }
}
