// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.Widgets.ItemPickerPopupAdapter`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.UI.Editor.Widgets
{
  public class ItemPickerPopupAdapter<T> : 
    ItemPicker<ItemPickerPopup<T>.Item>.IAdapter,
    PopupSearchField.IAdapter,
    SearchField.IAdapter,
    ItemPickerFooter.IAdapter
    where T : IEquatable<T>
  {
    public Action<T> onSelectedItemChanged;
    private List<ItemPickerPopup<T>.Item> m_Items = new List<ItemPickerPopup<T>.Item>();
    private ItemPickerPopup<T>.Item m_SelectedItem;
    private string m_SearchQuery = string.Empty;
    private bool m_FilteredItemsDirty;

    public ItemPickerPopup<T>.Item selectedItem
    {
      get => this.m_SelectedItem;
      set
      {
        if (value == this.m_SelectedItem)
          return;
        this.m_SelectedItem = value;
        Action<T> selectedItemChanged = this.onSelectedItemChanged;
        if (selectedItemChanged == null)
          return;
        selectedItemChanged(this.m_SelectedItem != null ? this.m_SelectedItem.m_Value : default (T));
      }
    }

    public T selectedValue
    {
      get => this.m_SelectedItem == null ? default (T) : this.m_SelectedItem.m_Value;
      set
      {
        if (this.m_SelectedItem != null && object.Equals((object) this.m_SelectedItem.m_Value, (object) value))
          return;
        this.m_SelectedItem = this.m_Items.Find((Predicate<ItemPickerPopup<T>.Item>) (item => object.Equals((object) item.m_Value, (object) value)));
      }
    }

    public List<ItemPickerPopup<T>.Item> items => this.filteredItems;

    public List<ItemPickerPopup<T>.Item> filteredItems { get; } = new List<ItemPickerPopup<T>.Item>();

    public int length => this.items.Count;

    public int columnCount { get; set; } = 1;

    public bool Update()
    {
      if (!this.m_FilteredItemsDirty)
        return false;
      this.UpdateFilteredItems();
      return true;
    }

    public void SetItems(IEnumerable<ItemPickerPopup<T>.Item> newItems)
    {
      this.m_Items.Clear();
      this.m_Items.AddRange(newItems);
      this.m_FilteredItemsDirty = true;
      if (this.m_SelectedItem == null || this.m_Items.Contains(this.m_SelectedItem))
        return;
      this.m_SelectedItem = (ItemPickerPopup<T>.Item) null;
    }

    private void UpdateFilteredItems()
    {
      this.m_FilteredItemsDirty = false;
      this.filteredItems.Clear();
      string[] searchParts = ((IEnumerable<string>) this.m_SearchQuery.Split(' ', StringSplitOptions.None)).Where<string>((Func<string, bool>) (word => word.Length > 0)).ToArray<string>();
      this.filteredItems.AddRange(searchParts.Length != 0 ? this.m_Items.Where<ItemPickerPopup<T>.Item>((Func<ItemPickerPopup<T>.Item, bool>) (item => item.m_SearchTerms != null && item.m_SearchTerms.Length != 0 && ((IEnumerable<string>) item.m_SearchTerms).Any<string>((Func<string, bool>) (term => ((IEnumerable<string>) searchParts).All<string>((Func<string, bool>) (word => term.Contains(word, StringComparison.OrdinalIgnoreCase))))))) : (IEnumerable<ItemPickerPopup<T>.Item>) this.m_Items);
    }

    public string searchQuery
    {
      get => this.m_SearchQuery;
      set
      {
        this.m_SearchQuery = value;
        this.m_FilteredItemsDirty = true;
      }
    }

    public IEnumerable<PopupSearchField.Suggestion> searchSuggestions
    {
      get => Enumerable.Empty<PopupSearchField.Suggestion>();
    }

    public bool searchQueryIsFavorite => false;

    public void SetFavorite(int index, bool favorite)
    {
    }

    public void SetFavorite(string query, bool favorite)
    {
    }
  }
}
