// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.PagedBindings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Game.UI.Widgets
{
  public class PagedBindings : IWidgetBindingFactory
  {
    public IEnumerable<IBinding> CreateBindings(
      string group,
      IReader<IWidget> pathResolver,
      ValueChangedCallback onValueChanged)
    {
      yield return (IBinding) new TriggerBinding<IWidget, int>(group, "setCurrentPageIndex", (Action<IWidget, int>) ((widget, pageIndex) =>
      {
        if (widget is IPaged paged2)
          paged2.currentPageIndex = pageIndex;
        else
          Debug.LogError(widget != null ? (object) "Widget does not implement IPaged" : (object) "Invalid widget path");
      }), pathResolver);
    }
  }
}
