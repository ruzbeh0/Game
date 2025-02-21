// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.IItemPicker
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Editor
{
  public interface IItemPicker
  {
    int selectedIndex { get; set; }

    int visibleStartIndex { get; set; }

    int visibleEndIndex { get; set; }

    void SetFavorite(int index, bool favorite);

    class Item : IJsonWritable
    {
      public LocalizedString displayName { get; set; }

      public LocalizedString? tooltip { get; set; }

      [CanBeNull]
      public string image { get; set; }

      [CanBeNull]
      public string badge { get; set; }

      public bool directory { get; set; }

      public bool favorite { get; set; }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("displayName");
        writer.Write<LocalizedString>(this.displayName);
        writer.PropertyName("tooltip");
        writer.Write<LocalizedString>(this.tooltip);
        writer.PropertyName("image");
        writer.Write(this.image);
        writer.PropertyName("directory");
        writer.Write(this.directory);
        writer.PropertyName("favorite");
        writer.Write(this.favorite);
        writer.PropertyName("badge");
        writer.Write(this.badge);
        writer.TypeEnd();
      }
    }

    class Bindings : IWidgetBindingFactory
    {
      public IEnumerable<IBinding> CreateBindings(
        string group,
        IReader<IWidget> pathResolver,
        ValueChangedCallback onValueChanged)
      {
        yield return (IBinding) new TriggerBinding<IWidget, int, int>(group, "setVisibleItemRange", (Action<IWidget, int, int>) ((widget, startIndex, endIndex) =>
        {
          if (!(widget is IItemPicker itemPicker2))
            return;
          itemPicker2.visibleStartIndex = startIndex;
          itemPicker2.visibleEndIndex = endIndex;
        }), pathResolver);
        yield return (IBinding) new TriggerBinding<IWidget, int>(group, "setItemSelected", (Action<IWidget, int>) ((widget, index) =>
        {
          if (!(widget is IItemPicker itemPicker4))
            return;
          itemPicker4.selectedIndex = index;
          onValueChanged(widget);
        }), pathResolver);
        yield return (IBinding) new TriggerBinding<IWidget, int, bool>(group, "setItemFavorite", (Action<IWidget, int, bool>) ((widget, index, favorite) =>
        {
          if (!(widget is IItemPicker itemPicker6))
            return;
          itemPicker6.SetFavorite(index, favorite);
        }), pathResolver);
      }
    }
  }
}
