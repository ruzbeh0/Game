// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.RouteToolTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Routes;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  public class RouteToolTooltipSystem : TooltipSystemBase
  {
    private ToolSystem m_ToolSystem;
    private RouteToolSystem m_RouteTool;
    private ImageSystem m_ImageSystem;
    private NameSystem m_NameSystem;
    private EntityQuery m_TempRouteQuery;
    private EntityQuery m_TempStopQuery;
    private NameTooltip m_StopName;
    private StringTooltip m_StopTooltip;
    private NameTooltip m_RouteName;
    private StringTooltip m_RouteTooltip;
    private CachedLocalizedStringBuilder<RouteToolSystem.Tooltip> m_StringBuilder;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      this.m_RouteTool = this.World.GetOrCreateSystemManaged<RouteToolSystem>();
      this.m_ImageSystem = this.World.GetOrCreateSystemManaged<ImageSystem>();
      this.m_NameSystem = this.World.GetOrCreateSystemManaged<NameSystem>();
      this.m_TempRouteQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Route>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
      this.m_TempStopQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<TransportStop>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
      NameTooltip nameTooltip1 = new NameTooltip();
      nameTooltip1.path = (PathSegment) "routeToolStopName";
      nameTooltip1.nameBinder = this.m_NameSystem;
      this.m_StopName = nameTooltip1;
      StringTooltip stringTooltip1 = new StringTooltip();
      stringTooltip1.path = (PathSegment) "routeToolStop";
      this.m_StopTooltip = stringTooltip1;
      NameTooltip nameTooltip2 = new NameTooltip();
      nameTooltip2.path = (PathSegment) "routeToolRouteName";
      nameTooltip2.nameBinder = this.m_NameSystem;
      this.m_RouteName = nameTooltip2;
      StringTooltip stringTooltip2 = new StringTooltip();
      stringTooltip2.path = (PathSegment) "routeToolRoute";
      this.m_RouteTooltip = stringTooltip2;
      this.m_StringBuilder = CachedLocalizedStringBuilder<RouteToolSystem.Tooltip>.Id((Func<RouteToolSystem.Tooltip, string>) (t => string.Format("Tools.INFO[{0:G}]", (object) t)));
    }

    [Preserve]
    protected override void OnUpdate()
    {
      if (this.m_ToolSystem.activeTool != this.m_RouteTool || this.m_RouteTool.tooltip == RouteToolSystem.Tooltip.None)
        return;
      // ISSUE: variable of a compiler-generated type
      RouteToolSystem.Tooltip tooltip = this.m_RouteTool.tooltip;
      switch (tooltip)
      {
        case RouteToolSystem.Tooltip.CreateRoute:
        case RouteToolSystem.Tooltip.AddWaypoint:
        case RouteToolSystem.Tooltip.CompleteRoute:
          this.m_StopTooltip.value = this.m_StringBuilder[this.m_RouteTool.tooltip];
          this.AddMouseTooltip((IWidget) this.m_StopTooltip);
          this.TryAddStopName();
          break;
        case RouteToolSystem.Tooltip.CreateOrModify:
          this.m_StopTooltip.value = this.m_StringBuilder[RouteToolSystem.Tooltip.CreateRoute];
          this.AddMouseTooltip((IWidget) this.m_StopTooltip);
          this.m_RouteTooltip.value = this.m_StringBuilder[RouteToolSystem.Tooltip.ModifyWaypoint];
          this.AddMouseTooltip((IWidget) this.m_RouteTooltip);
          this.TryAddStopName();
          this.TryAddRouteName();
          break;
        case RouteToolSystem.Tooltip.InsertWaypoint:
        case RouteToolSystem.Tooltip.MoveWaypoint:
        case RouteToolSystem.Tooltip.MergeWaypoints:
        case RouteToolSystem.Tooltip.RemoveWaypoint:
          this.m_RouteTooltip.value = this.m_StringBuilder[this.m_RouteTool.tooltip];
          this.AddMouseTooltip((IWidget) this.m_RouteTooltip);
          this.TryAddStopName();
          this.TryAddRouteName();
          break;
        default:
          this.m_RouteTooltip.value = this.m_StringBuilder[this.m_RouteTool.tooltip];
          this.AddMouseTooltip((IWidget) this.m_RouteTooltip);
          this.TryAddRouteName();
          break;
      }
    }

    public void TryAddStopName()
    {
      if (this.m_TempStopQuery.IsEmptyIgnoreFilter)
        return;
      using (NativeArray<Temp> componentDataArray = this.m_TempStopQuery.ToComponentDataArray<Temp>((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        for (int index = 0; index < componentDataArray.Length; ++index)
        {
          Temp temp = componentDataArray[index];
          if (temp.m_Original != Entity.Null)
          {
            this.AddMouseTooltip((IWidget) this.m_StopName);
            // ISSUE: reference to a compiler-generated method
            this.m_StopName.icon = this.m_ImageSystem.GetInstanceIcon(temp.m_Original);
            this.m_StopName.entity = temp.m_Original;
            break;
          }
        }
      }
    }

    public void TryAddRouteName()
    {
      if (this.m_TempRouteQuery.IsEmptyIgnoreFilter)
        return;
      using (NativeArray<Temp> componentDataArray = this.m_TempRouteQuery.ToComponentDataArray<Temp>((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        for (int index = 0; index < componentDataArray.Length; ++index)
        {
          Temp temp = componentDataArray[index];
          if (temp.m_Original != Entity.Null)
          {
            this.AddMouseTooltip((IWidget) this.m_RouteName);
            // ISSUE: reference to a compiler-generated method
            this.m_RouteName.icon = this.m_ImageSystem.GetInstanceIcon(temp.m_Original);
            this.m_RouteName.entity = temp.m_Original;
            break;
          }
        }
      }
    }

    [Preserve]
    public RouteToolTooltipSystem()
    {
    }
  }
}
