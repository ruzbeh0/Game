// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.ListBindings
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
  public class ListBindings : IWidgetBindingFactory
  {
    public IEnumerable<IBinding> CreateBindings(
      string group,
      IReader<IWidget> pathResolver,
      ValueChangedCallback onValueChanged)
    {
      yield return (IBinding) new TriggerBinding<IWidget>(group, "addListElement", (Action<IWidget>) (widget =>
      {
        if (widget is IListWidget listWidget2)
        {
          listWidget2.AddElement();
          onValueChanged(widget);
        }
        else
          Debug.LogError(widget != null ? (object) "Widget does not implement IListContainer" : (object) "Invalid widget path");
      }), pathResolver);
      yield return (IBinding) new TriggerBinding<IWidget, int>(group, "duplicateListElement", (Action<IWidget, int>) ((widget, index) =>
      {
        if (widget is IListWidget listWidget4)
        {
          listWidget4.DuplicateElement(index);
          onValueChanged(widget);
        }
        else
          Debug.LogError(widget != null ? (object) "Widget does not implement IListContainer" : (object) "Invalid widget path");
      }), pathResolver);
      yield return (IBinding) new TriggerBinding<IWidget, int, int>(group, "moveListElement", (Action<IWidget, int, int>) ((widget, fromIndex, toIndex) =>
      {
        if (widget is IListWidget listWidget6)
        {
          listWidget6.MoveElement(fromIndex, toIndex);
          onValueChanged(widget);
        }
        else
          Debug.LogError(widget != null ? (object) "Widget does not implement IListContainer" : (object) "Invalid widget path");
      }), pathResolver);
      yield return (IBinding) new TriggerBinding<IWidget, int>(group, "deleteListElement", (Action<IWidget, int>) ((widget, index) =>
      {
        if (widget is IListWidget listWidget8)
        {
          listWidget8.DeleteElement(index);
          onValueChanged(widget);
        }
        else
          Debug.LogError(widget != null ? (object) "Widget does not implement IListContainer" : (object) "Invalid widget path");
      }), pathResolver);
      yield return (IBinding) new TriggerBinding<IWidget>(group, "clearList", (Action<IWidget>) (widget =>
      {
        if (widget is IListWidget listWidget10)
        {
          listWidget10.Clear();
          onValueChanged(widget);
        }
        else
          Debug.LogError(widget != null ? (object) "Widget does not implement IListContainer" : (object) "Invalid widget path");
      }), pathResolver);
    }
  }
}
