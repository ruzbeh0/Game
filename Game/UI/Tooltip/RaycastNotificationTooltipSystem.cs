// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.RaycastNotificationTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Common;
using Game.Notifications;
using Game.Prefabs;
using Game.Tools;
using Game.UI.Widgets;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  [CompilerGenerated]
  public class RaycastNotificationTooltipSystem : TooltipSystemBase
  {
    private ToolSystem m_ToolSystem;
    private DefaultToolSystem m_DefaultTool;
    private PrefabSystem m_PrefabSystem;
    private NameSystem m_NameSystem;
    private ImageSystem m_ImageSystem;
    private ToolRaycastSystem m_ToolRaycastSystem;
    private EntityQuery m_ConfigurationQuery;
    private NotificationTooltip m_Tooltip;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefaultTool = this.World.GetOrCreateSystemManaged<DefaultToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NameSystem = this.World.GetOrCreateSystemManaged<NameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ImageSystem = this.World.GetOrCreateSystemManaged<ImageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolRaycastSystem = this.World.GetOrCreateSystemManaged<ToolRaycastSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<IconConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ConfigurationQuery);
      NotificationTooltip notificationTooltip = new NotificationTooltip();
      notificationTooltip.path = (PathSegment) "raycastNotification";
      notificationTooltip.verbose = true;
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltip = notificationTooltip;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      RaycastResult result;
      Icon component1;
      PrefabRef component2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool != this.m_DefaultTool || !this.m_ToolRaycastSystem.GetRaycastResult(out result) || !this.EntityManager.TryGetComponent<Icon>(result.m_Owner, out component1) || !this.EntityManager.TryGetComponent<PrefabRef>(result.m_Owner, out component2))
        return;
      // ISSUE: reference to a compiler-generated field
      IconConfigurationData singleton = this.m_ConfigurationQuery.GetSingleton<IconConfigurationData>();
      if (component2.m_Prefab == singleton.m_SelectedMarker || component2.m_Prefab == singleton.m_FollowedMarker)
        return;
      NotificationIconPrefab prefab;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_Tooltip.name = this.m_PrefabSystem.TryGetPrefab<NotificationIconPrefab>(component2, out prefab) ? prefab.name : this.m_PrefabSystem.GetObsoleteID(component2.m_Prefab).GetName();
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltip.color = NotificationTooltip.GetColor(component1.m_Priority);
      // ISSUE: reference to a compiler-generated field
      this.AddMouseTooltip((IWidget) this.m_Tooltip);
    }

    [Preserve]
    public RaycastNotificationTooltipSystem()
    {
    }
  }
}
