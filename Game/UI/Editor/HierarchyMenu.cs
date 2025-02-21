// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.HierarchyMenu
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
  public abstract class HierarchyMenu : Widget
  {
    protected abstract void OnSetRenderedRange(int startVisibleIndex, int endVisibleIndex);

    protected abstract void OnSetItemSelected(int viewportIndex, bool selected);

    protected abstract void OnSetItemExpanded(int viewportIndex, bool expanded);

    public enum SelectionType
    {
      singleSelection,
      multiSelection,
      inheritedMultiSelection,
    }

    public class Bindings : IWidgetBindingFactory
    {
      public IEnumerable<IBinding> CreateBindings(
        string group,
        IReader<IWidget> pathResolver,
        ValueChangedCallback onValueChanged)
      {
        yield return (IBinding) new TriggerBinding<IWidget, int, int>(group, "setHierarchyRenderedRange", (Action<IWidget, int, int>) ((widget, startVisibleIndex, endVisibleIndex) =>
        {
          if (!(widget is HierarchyMenu hierarchyMenu2))
            return;
          hierarchyMenu2.OnSetRenderedRange(startVisibleIndex, endVisibleIndex);
          onValueChanged(widget);
        }), pathResolver);
        yield return (IBinding) new TriggerBinding<IWidget, int, bool>(group, "setHierarchyItemSelected", (Action<IWidget, int, bool>) ((widget, viewportIndex, selected) =>
        {
          if (!(widget is HierarchyMenu hierarchyMenu4))
            return;
          hierarchyMenu4.OnSetItemSelected(viewportIndex, selected);
          onValueChanged(widget);
        }), pathResolver);
        yield return (IBinding) new TriggerBinding<IWidget, int, bool>(group, "setHierarchyItemExpanded", (Action<IWidget, int, bool>) ((widget, viewportIndex, expanded) =>
        {
          if (!(widget is HierarchyMenu hierarchyMenu6))
            return;
          hierarchyMenu6.OnSetItemExpanded(viewportIndex, expanded);
          onValueChanged(widget);
        }), pathResolver);
      }
    }
  }
}
