// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.ExpandableGroup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Reflection;
using System;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Widgets
{
  public class ExpandableGroup : NamedWidgetWithTooltip, IExpandable, IContainerWidget
  {
    private ITypedValueAccessor<bool> m_ExpandedAccessor;
    private bool m_Expanded;
    private IList<IWidget> m_Children = (IList<IWidget>) Array.Empty<IWidget>();

    public virtual bool expanded
    {
      get => this.m_ExpandedAccessor.GetTypedValue();
      set => this.m_ExpandedAccessor.SetTypedValue(value);
    }

    public IList<IWidget> children
    {
      get => this.m_Children;
      set
      {
        if (object.Equals((object) value, (object) this.m_Children))
          return;
        ContainerExtensions.SetDefaults<IWidget>(value);
        this.m_Children = value;
        if (!this.expanded)
          return;
        this.SetChildrenChanged();
      }
    }

    public override IList<IWidget> visibleChildren
    {
      get => !this.expanded ? (IList<IWidget>) Array.Empty<IWidget>() : this.children;
    }

    public ExpandableGroup(ITypedValueAccessor<bool> expandedAccessor)
    {
      this.m_ExpandedAccessor = expandedAccessor;
      this.m_Expanded = expandedAccessor.GetTypedValue();
    }

    public ExpandableGroup(bool expanded = false)
    {
      this.m_ExpandedAccessor = (ITypedValueAccessor<bool>) new ObjectAccessor<bool>(expanded, false);
      this.m_Expanded = expanded;
    }

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      bool typedValue = this.m_ExpandedAccessor.GetTypedValue();
      if (typedValue != this.m_Expanded)
      {
        widgetChanges |= WidgetChanges.Properties | WidgetChanges.Children;
        this.m_Expanded = typedValue;
      }
      return widgetChanges;
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("expanded");
      writer.Write(this.expanded);
    }
  }
}
