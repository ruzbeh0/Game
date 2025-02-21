// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.InputHintsTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Input;
using Game.Tools;
using Game.UI.Widgets;
using System.Collections.Generic;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  public class InputHintsTooltipSystem : TooltipSystemBase
  {
    private ToolSystem m_ToolSystem;
    private ToolBaseSystem m_LastActiveTool;
    private readonly Dictionary<ProxyAction, InputHintTooltip> m_Tooltips = new Dictionary<ProxyAction, InputHintTooltip>();

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: variable of a compiler-generated type
      ToolBaseSystem activeTool = this.m_ToolSystem.activeTool;
      if (this.m_LastActiveTool != activeTool)
      {
        this.m_LastActiveTool = activeTool;
        this.m_Tooltips.Clear();
        if (this.m_LastActiveTool != null)
        {
          foreach (IProxyAction action1 in activeTool.actions)
          {
            if (!(action1 is UIBaseInputAction.IState state))
            {
              if (action1 is ProxyAction proxyAction && !this.m_Tooltips.ContainsKey(proxyAction))
                this.m_Tooltips.Add(proxyAction, new InputHintTooltip(proxyAction));
            }
            else
            {
              foreach (ProxyAction action2 in (IEnumerable<ProxyAction>) state.actions)
              {
                if (!this.m_Tooltips.ContainsKey(action2))
                  this.m_Tooltips.Add(action2, new InputHintTooltip(action2));
              }
            }
          }
        }
      }
      foreach (KeyValuePair<ProxyAction, InputHintTooltip> tooltip1 in this.m_Tooltips)
      {
        ProxyAction proxyAction1;
        InputHintTooltip inputHintTooltip;
        tooltip1.Deconstruct(ref proxyAction1, ref inputHintTooltip);
        ProxyAction proxyAction2 = proxyAction1;
        InputHintTooltip tooltip2 = inputHintTooltip;
        if (proxyAction2.enabled && (proxyAction2.mask & InputManager.DeviceType.Gamepad) != InputManager.DeviceType.None && proxyAction2.displayOverride != null && proxyAction2.displayOverride.priority != -1)
        {
          tooltip2.Refresh();
          this.AddMouseTooltip((IWidget) tooltip2);
        }
      }
    }

    [Preserve]
    public InputHintsTooltipSystem()
    {
    }
  }
}
