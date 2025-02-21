// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.ItemPicker`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using Game.UI.Widgets;
using System.Collections.Generic;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.Editor
{
  public class ItemPicker<T> : Widget, IItemPicker where T : IItemPicker.Item
  {
    private int m_Length;
    private int m_SelectedIndex = -1;
    private int m_ColumnCount = 1;
    private int m_StartIndex;
    private List<T> m_VisibleItems = new List<T>();

    public ItemPicker<T>.IAdapter adapter { get; set; }

    public FlexLayout flex { get; set; } = FlexLayout.Fill;

    public int selectedIndex
    {
      get => this.m_SelectedIndex;
      set => this.adapter.selectedItem = this.adapter.items[value];
    }

    public bool hasImages { get; set; } = true;

    public bool hasFavorites { get; set; }

    public int visibleStartIndex { get; set; }

    public int visibleEndIndex { get; set; }

    public bool selectOnFocus { get; set; }

    public void SetFavorite(int index, bool favorite)
    {
      this.adapter.SetFavorite(index, favorite);
      this.SetPropertiesChanged();
    }

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      int num1 = this.adapter.Update() ? 1 : 0;
      int count = this.adapter.items.Count;
      if (count != this.m_Length)
      {
        widgetChanges |= WidgetChanges.Properties;
        this.m_Length = count;
      }
      if ((object) this.adapter.selectedItem != null)
      {
        if (this.m_SelectedIndex == -1 || this.m_SelectedIndex >= this.adapter.items.Count || (object) this.adapter.items[this.m_SelectedIndex] != (object) this.adapter.selectedItem)
        {
          widgetChanges |= WidgetChanges.Properties;
          this.m_SelectedIndex = this.adapter.items.IndexOf(this.adapter.selectedItem);
        }
      }
      else
      {
        if (this.m_SelectedIndex != -1)
          widgetChanges |= WidgetChanges.Properties;
        this.m_SelectedIndex = -1;
      }
      if (this.adapter.columnCount != this.m_ColumnCount)
      {
        widgetChanges |= WidgetChanges.Properties;
        this.m_ColumnCount = this.adapter.columnCount;
      }
      this.visibleStartIndex = math.clamp(this.visibleStartIndex, 0, this.m_Length);
      this.visibleEndIndex = math.clamp(this.visibleEndIndex, this.visibleStartIndex, this.m_Length);
      int num2 = this.visibleEndIndex - this.visibleStartIndex;
      if (num1 != 0 || this.visibleStartIndex != this.m_StartIndex || num2 != this.m_VisibleItems.Count)
      {
        widgetChanges |= WidgetChanges.Properties | WidgetChanges.Children;
        this.m_StartIndex = this.visibleStartIndex;
        this.m_VisibleItems.Clear();
        for (int visibleStartIndex = this.visibleStartIndex; visibleStartIndex < this.visibleEndIndex; ++visibleStartIndex)
          this.m_VisibleItems.Add(this.adapter.items[visibleStartIndex]);
      }
      return widgetChanges;
    }

    public override string propertiesTypeName => "Game.UI.Editor.ItemPicker";

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("flex");
      writer.Write<FlexLayout>(this.flex);
      writer.PropertyName("selectedIndex");
      writer.Write(this.selectedIndex);
      writer.PropertyName("hasImages");
      writer.Write(this.hasImages);
      writer.PropertyName("hasFavorites");
      writer.Write(this.hasFavorites);
      writer.PropertyName("columnCount");
      writer.Write(this.m_ColumnCount);
      writer.PropertyName("length");
      writer.Write(this.m_Length);
      writer.PropertyName("visibleStartIndex");
      writer.Write(this.m_StartIndex);
      writer.PropertyName("visibleItems");
      writer.ArrayBegin(this.m_VisibleItems.Count);
      foreach (T visibleItem in this.m_VisibleItems)
        writer.Write<T>(visibleItem);
      writer.ArrayEnd();
      writer.PropertyName("selectOnFocus");
      writer.Write(this.selectOnFocus);
    }

    public interface IAdapter
    {
      [CanBeNull]
      T selectedItem { get; set; }

      List<T> items { get; }

      int columnCount { get; }

      bool Update();

      void SetFavorite(int index, bool favorite);
    }
  }
}
