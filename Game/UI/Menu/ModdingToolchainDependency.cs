// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.ModdingToolchainDependency
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Modding.Toolchain;
using Game.Reflection;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.UI.Menu
{
  public class ModdingToolchainDependency : ReadonlyField<IToolchainDependency>, IExpandable
  {
    private bool m_Expanded;
    private IList<IWidget> m_Children = (IList<IWidget>) Array.Empty<IWidget>();

    public ModdingToolchainDependency()
    {
      this.valueWriter = (IWriter<IToolchainDependency>) new ModdingToolchainDependencyWriter();
    }

    public ITypedValueAccessor<bool> expandedAccessor { get; set; }

    public bool expanded
    {
      get
      {
        ITypedValueAccessor<bool> expandedAccessor = this.expandedAccessor;
        return expandedAccessor == null || expandedAccessor.GetTypedValue();
      }
      set => this.expandedAccessor?.SetTypedValue(value);
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

    public override WidgetChanges UpdateVisibility()
    {
      foreach (Widget widget in this.visibleChildren.OfType<Widget>())
      {
        int num = (int) widget.UpdateVisibility();
      }
      return base.UpdateVisibility();
    }

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      ITypedValueAccessor<bool> expandedAccessor = this.expandedAccessor;
      bool flag = expandedAccessor == null || expandedAccessor.GetTypedValue();
      if (flag != this.m_Expanded)
      {
        widgetChanges |= WidgetChanges.Properties | WidgetChanges.Children;
        this.m_Expanded = flag;
      }
      return widgetChanges;
    }
  }
}
