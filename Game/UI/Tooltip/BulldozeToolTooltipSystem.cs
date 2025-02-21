// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.BulldozeToolTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Input;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  public class BulldozeToolTooltipSystem : TooltipSystemBase
  {
    private ToolSystem m_ToolSystem;
    private BulldozeToolSystem m_BulldozeTool;
    private EntityQuery m_TempQuery;
    private StringTooltip m_Tooltip;
    private CachedLocalizedStringBuilder<BulldozeToolSystem.Tooltip> m_StringBuilder;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      this.m_BulldozeTool = this.World.GetOrCreateSystemManaged<BulldozeToolSystem>();
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.Exclude<Deleted>());
      this.RequireForUpdate(this.m_TempQuery);
      StringTooltip stringTooltip = new StringTooltip();
      stringTooltip.path = (PathSegment) "bulldozeTool";
      this.m_Tooltip = stringTooltip;
      this.m_StringBuilder = CachedLocalizedStringBuilder<BulldozeToolSystem.Tooltip>.Id((Func<BulldozeToolSystem.Tooltip, string>) (t => string.Format("Tools.INFO[{0:G}]", (object) t)));
    }

    [Preserve]
    protected override void OnUpdate()
    {
      if (this.m_ToolSystem.activeTool != this.m_BulldozeTool || this.m_BulldozeTool.tooltip == BulldozeToolSystem.Tooltip.None || InputManager.instance.activeControlScheme != InputManager.ControlScheme.KeyboardAndMouse)
        return;
      this.m_Tooltip.value = this.m_StringBuilder[this.m_BulldozeTool.tooltip];
      this.AddMouseTooltip((IWidget) this.m_Tooltip);
    }

    [Preserve]
    public BulldozeToolTooltipSystem()
    {
    }
  }
}
