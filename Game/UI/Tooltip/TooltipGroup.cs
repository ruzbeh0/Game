// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.TooltipGroup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.Tooltip
{
  public class TooltipGroup : Widget
  {
    private float2 m_Position;
    private TooltipGroup.Category m_Category;
    private TooltipGroup.Alignment m_HorizontalAlignment;
    private TooltipGroup.Alignment m_VerticalAlignment;
    private List<IWidget> m_LastChildren = new List<IWidget>();

    public float2 position
    {
      get => this.m_Position;
      set
      {
        if (value.Equals(this.m_Position))
          return;
        this.m_Position = value;
        this.SetPropertiesChanged();
      }
    }

    public TooltipGroup.Alignment horizontalAlignment
    {
      get => this.m_HorizontalAlignment;
      set
      {
        if (value == this.m_HorizontalAlignment)
          return;
        this.m_HorizontalAlignment = value;
        this.SetPropertiesChanged();
      }
    }

    public TooltipGroup.Alignment verticalAlignment
    {
      get => this.m_VerticalAlignment;
      set
      {
        if (value == this.m_VerticalAlignment)
          return;
        this.m_VerticalAlignment = value;
        this.SetPropertiesChanged();
      }
    }

    public TooltipGroup.Category category
    {
      get => this.m_Category;
      set
      {
        if (value == this.m_Category)
          return;
        this.m_Category = value;
        this.SetPropertiesChanged();
      }
    }

    public IList<IWidget> children { get; set; } = (IList<IWidget>) new List<IWidget>();

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      if (!this.m_LastChildren.SequenceEqual<IWidget>((IEnumerable<IWidget>) this.children))
      {
        widgetChanges |= WidgetChanges.Children;
        this.m_LastChildren.Clear();
        this.m_LastChildren.AddRange((IEnumerable<IWidget>) this.children);
        ContainerExtensions.SetDefaults<IWidget>((IList<IWidget>) this.m_LastChildren);
      }
      return widgetChanges;
    }

    public override IList<IWidget> visibleChildren => (IList<IWidget>) this.m_LastChildren;

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("position");
      writer.Write(this.position);
      writer.PropertyName("horizontalAlignment");
      writer.Write((int) this.horizontalAlignment);
      writer.PropertyName("verticalAlignment");
      writer.Write((int) this.verticalAlignment);
      writer.PropertyName("category");
      writer.Write((int) this.category);
    }

    public enum Alignment
    {
      Start,
      Center,
      End,
    }

    [Flags]
    public enum Category
    {
      None = 0,
      Network = 1,
    }
  }
}
