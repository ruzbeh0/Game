// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.FilterMenu
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Editor
{
  public class FilterMenu : Widget
  {
    private bool m_Dirty;
    private FilterMenu.IAdapter m_Adapter;

    public FilterMenu.IAdapter adapter
    {
      get => this.m_Adapter;
      set
      {
        if (this.m_Adapter != null)
          this.m_Adapter.onAvailableFiltersChanged -= new Action(this.OnAvailableFiltersChanged);
        this.m_Adapter = value;
        this.m_Adapter.onAvailableFiltersChanged += new Action(this.OnAvailableFiltersChanged);
      }
    }

    private void OnAvailableFiltersChanged() => this.m_Dirty = true;

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      if (this.m_Dirty)
        widgetChanges |= WidgetChanges.Properties;
      this.m_Dirty = false;
      return widgetChanges;
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("availableFilters");
      writer.Write((IList<string>) this.adapter.availableFilters);
      writer.PropertyName("activeFilters");
      writer.Write((IList<string>) this.adapter.activeFilters);
    }

    private void OnToggleFilter(string filter, bool active)
    {
      this.adapter.ToggleFilter(filter, active);
      this.m_Dirty = true;
    }

    private void OnClearFilters()
    {
      this.adapter.ClearFilters();
      this.m_Dirty = true;
    }

    public interface IAdapter
    {
      void ToggleFilter(string filter, bool active);

      void ClearFilters();

      List<string> availableFilters { get; }

      List<string> activeFilters { get; }

      Action onAvailableFiltersChanged { get; set; }
    }

    public class Bindings : IWidgetBindingFactory
    {
      public IEnumerable<IBinding> CreateBindings(
        string group,
        IReader<IWidget> pathResolver,
        ValueChangedCallback onValueChanged)
      {
        yield return (IBinding) new TriggerBinding<IWidget, string, bool>(group, "toggleFilter", (Action<IWidget, string, bool>) ((widget, filter, active) =>
        {
          if (!(widget is FilterMenu filterMenu2))
            return;
          filterMenu2.OnToggleFilter(filter, active);
          onValueChanged(widget);
        }), pathResolver);
        yield return (IBinding) new TriggerBinding<IWidget>(group, "clearFilters", (Action<IWidget>) (widget =>
        {
          if (!(widget is FilterMenu filterMenu4))
            return;
          filterMenu4.OnClearFilters();
          onValueChanged(widget);
        }), pathResolver);
      }
    }
  }
}
