// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.ListField
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
  public class ListField : Widget
  {
    public List<ListField.Item> m_Items;
    public Action<int> onItemRemoved;

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("items");
      writer.Write<ListField.Item>((IList<ListField.Item>) this.m_Items);
    }

    protected void RemoveItem(int index)
    {
      Action<int> onItemRemoved = this.onItemRemoved;
      if (onItemRemoved == null)
        return;
      onItemRemoved(index);
    }

    public struct Item : IJsonWritable
    {
      public string m_Label;
      public bool m_Removable;
      public object m_Data;
      public string[] m_SubItems;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().Name);
        writer.PropertyName("label");
        writer.Write(this.m_Label);
        writer.PropertyName("removable");
        writer.Write(this.m_Removable);
        writer.PropertyName("subItems");
        writer.Write(this.m_SubItems);
        writer.TypeEnd();
      }
    }

    public class Bindings : IWidgetBindingFactory
    {
      public IEnumerable<IBinding> CreateBindings(
        string group,
        IReader<IWidget> pathResolver,
        ValueChangedCallback onValueChanged)
      {
        yield return (IBinding) new TriggerBinding<IWidget, int>(group, "removeListItem", (Action<IWidget, int>) ((widget, index) =>
        {
          if (!(widget is ListField listField2))
            return;
          listField2.RemoveItem(index);
          onValueChanged(widget);
        }), pathResolver);
      }
    }
  }
}
