// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.TooltipUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Input;
using Game.UI.Widgets;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  public class TooltipUISystem : UISystemBase
  {
    private const string kGroup = "tooltip";
    private static readonly float2 kTooltipPointerDistance = new float2(0.0f, 16f);
    private UpdateSystem m_UpdateSystem;
    private WidgetBindings m_WidgetBindings;

    public override GameMode gameMode => GameMode.GameOrEditor;

    public List<TooltipGroup> groups { get; private set; }

    public TooltipGroup mouseGroup { get; private set; }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_UpdateSystem = this.World.GetOrCreateSystemManaged<UpdateSystem>();
      this.AddUpdateBinding((IUpdateBinding) (this.m_WidgetBindings = new WidgetBindings("tooltip", "groups")));
      this.groups = new List<TooltipGroup>();
      TooltipGroup tooltipGroup = new TooltipGroup();
      tooltipGroup.path = (PathSegment) "mouse";
      tooltipGroup.position = new float2();
      tooltipGroup.horizontalAlignment = TooltipGroup.Alignment.Start;
      tooltipGroup.verticalAlignment = TooltipGroup.Alignment.Start;
      this.mouseGroup = tooltipGroup;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      if (this.m_WidgetBindings.active)
      {
        this.m_WidgetBindings.children.Clear();
        this.groups.Clear();
        this.mouseGroup.children.Clear();
        if (!InputManager.instance.mouseOverUI)
        {
          this.m_UpdateSystem.Update(SystemUpdatePhase.UITooltip);
          if (InputManager.instance.mouseOnScreen && this.mouseGroup.children.Count > 0)
          {
            Vector3 mousePosition = InputManager.instance.mousePosition;
            this.mouseGroup.position = math.round(new float2(mousePosition.x, (float) Screen.height - mousePosition.y) + TooltipUISystem.kTooltipPointerDistance);
            this.m_WidgetBindings.children.Add((IWidget) this.mouseGroup);
          }
          foreach (IWidget group in this.groups)
            this.m_WidgetBindings.children.Add(group);
        }
      }
      base.OnUpdate();
    }

    [Preserve]
    public TooltipUISystem()
    {
    }
  }
}
