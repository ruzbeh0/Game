// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.Widgets.ItemPickerPopup`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Reflection;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Editor.Widgets
{
  public class ItemPickerPopup<T> : IValueFieldPopup<T> where T : IEquatable<T>
  {
    private static readonly LocalizedString kNoneValue = LocalizedString.Id("Editor.NONE_VALUE");
    private ITypedValueAccessor<T> m_Accessor;
    private ItemPickerPopupAdapter<T> m_Adapter;

    public IList<IWidget> children { get; }

    public ItemPickerPopup(bool hasFooter = true, bool hasImages = true)
    {
      this.m_Adapter = new ItemPickerPopupAdapter<T>();
      this.m_Adapter.onSelectedItemChanged += new Action<T>(this.OnSelectedItemChanged);
      List<IWidget> widgetList = new List<IWidget>()
      {
        (IWidget) new PopupSearchField()
        {
          adapter = (PopupSearchField.IAdapter) this.m_Adapter,
          hasFavorites = false
        },
        (IWidget) new ItemPicker<ItemPickerPopup<T>.Item>()
        {
          adapter = (ItemPicker<ItemPickerPopup<T>.Item>.IAdapter) this.m_Adapter,
          hasFavorites = false,
          hasImages = hasImages
        }
      };
      if (hasFooter)
        widgetList.Add((IWidget) new ItemPickerFooter()
        {
          adapter = (ItemPickerFooter.IAdapter) this.m_Adapter
        });
      this.children = (IList<IWidget>) widgetList;
      ContainerExtensions.SetDefaults<IWidget>(this.children);
    }

    public void SetItems(IEnumerable<ItemPickerPopup<T>.Item> items)
    {
      this.m_Adapter.SetItems(items);
    }

    public void Attach(ITypedValueAccessor<T> accessor) => this.m_Accessor = accessor;

    public void Detach() => this.m_Adapter.searchQuery = string.Empty;

    private void OnSelectedItemChanged(T value) => this.m_Accessor.SetTypedValue(value);

    public bool Update()
    {
      this.m_Adapter.selectedValue = this.m_Accessor.GetTypedValue();
      return false;
    }

    public LocalizedString GetDisplayValue(T value)
    {
      return this.m_Adapter.selectedItem != null ? this.m_Adapter.selectedItem.displayName : ItemPickerPopup<T>.kNoneValue;
    }

    public class Item : IItemPicker.Item
    {
      public T m_Value;
      public string[] m_SearchTerms;
    }
  }
}
