// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.TooltipSystemBase
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.UI.Widgets;
using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  public abstract class TooltipSystemBase : GameSystemBase
  {
    private TooltipUISystem m_TooltipUISystem;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_TooltipUISystem = this.World.GetOrCreateSystemManaged<TooltipUISystem>();
    }

    protected void AddGroup(TooltipGroup group)
    {
      if (group.path != PathSegment.Empty && this.m_TooltipUISystem.groups.Any<TooltipGroup>((Func<TooltipGroup, bool>) (g => g.path == group.path)))
        Debug.LogError((object) string.Format("Trying to add tooltip group with duplicate path '{0}'", (object) group.path));
      else
        this.m_TooltipUISystem.groups.Add(group);
    }

    protected void AddMouseTooltip(IWidget tooltip)
    {
      if (tooltip.path != PathSegment.Empty && this.m_TooltipUISystem.mouseGroup.children.Any<IWidget>((Func<IWidget, bool>) (t => t.path == tooltip.path)))
        Debug.LogError((object) string.Format("Trying to add mouse tooltip with duplicate path '{0}'", (object) tooltip.path));
      else
        this.m_TooltipUISystem.mouseGroup.children.Add(tooltip);
    }

    protected static float2 WorldToTooltipPos(Vector3 worldPos, out bool onScreen)
    {
      float2 xy = ((float3) Camera.main.WorldToScreenPoint(worldPos)).xy;
      xy.y = (float) Screen.height - xy.y;
      onScreen = (double) xy.x >= 0.0 && (double) xy.y >= 0.0 && (double) xy.x <= (double) Screen.width && (double) xy.y <= (double) Screen.height;
      return xy;
    }

    [Preserve]
    protected TooltipSystemBase()
    {
    }
  }
}
