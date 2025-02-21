// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.AssetPickerAdapter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Annotations;
using Game.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.UI.Editor
{
  public class AssetPickerAdapter : 
    ItemPicker<AssetItem>.IAdapter,
    SearchField.IAdapter,
    ItemPickerFooter.IAdapter
  {
    private string m_SearchQuery = string.Empty;
    private List<AssetItem> m_Items;
    private List<AssetItem> m_FilteredItems;
    private bool m_FilteredItemsChanged;
    [CanBeNull]
    private AssetItem m_SelectedItem;
    private int m_ColumnCount;
    private bool m_UseGlobalColumnCount = true;
    private HashSet<string> m_FavoriteIds = new HashSet<string>();
    public Action<AssetItem> EventItemSelected;

    [CanBeNull]
    public AssetItem selectedItem
    {
      get => this.m_SelectedItem;
      set => this.m_SelectedItem = value;
    }

    public AssetPickerAdapter(IEnumerable<AssetItem> items, int columnCount = 0)
    {
      this.m_FavoriteIds.Clear();
      EditorSettings editor = SharedSettings.instance?.editor;
      if (editor.assetPickerFavorites != null)
      {
        foreach (string assetPickerFavorite in editor.assetPickerFavorites)
          this.m_FavoriteIds.Add(assetPickerFavorite);
      }
      this.SetItems(items);
      this.m_UseGlobalColumnCount = columnCount <= 0;
      if (this.m_UseGlobalColumnCount)
        this.m_ColumnCount = editor != null ? editor.assetPickerColumnCount : 4;
      else
        this.m_ColumnCount = columnCount;
    }

    public void SetItems(IEnumerable<AssetItem> items)
    {
      this.m_Items = items.ToList<AssetItem>();
      foreach (AssetItem assetItem in this.m_Items)
        assetItem.favorite = this.m_FavoriteIds.Contains(assetItem.guid.ToString());
      this.m_Items.Sort();
      this.m_FilteredItems = new List<AssetItem>((IEnumerable<AssetItem>) this.m_Items);
    }

    public AssetItem SelectItemByName(string name, StringComparison comparisonType)
    {
      this.m_SelectedItem = this.m_Items.FirstOrDefault<AssetItem>((Func<AssetItem, bool>) (item => item.fileName.Equals(name, comparisonType)));
      return this.m_SelectedItem;
    }

    public AssetItem SelectItemByGuid(Hash128 guid)
    {
      this.m_SelectedItem = this.m_Items.FirstOrDefault<AssetItem>((Func<AssetItem, bool>) (item => item.guid == guid));
      return this.m_SelectedItem;
    }

    private void UpdateFilteredItems()
    {
      this.m_FilteredItems.Clear();
      this.m_FilteredItems.AddRange(!string.IsNullOrEmpty(this.m_SearchQuery) ? this.m_Items.Where<AssetItem>((Func<AssetItem, bool>) (item => !string.IsNullOrEmpty(item.fileName) && item.fileName.IndexOf(this.m_SearchQuery, StringComparison.OrdinalIgnoreCase) != -1)) : (IEnumerable<AssetItem>) this.m_Items);
      this.m_FilteredItemsChanged = true;
    }

    AssetItem ItemPicker<AssetItem>.IAdapter.selectedItem
    {
      get => this.m_SelectedItem;
      set
      {
        this.m_SelectedItem = value;
        Action<AssetItem> eventItemSelected = this.EventItemSelected;
        if (eventItemSelected == null)
          return;
        eventItemSelected(this.m_SelectedItem);
      }
    }

    List<AssetItem> ItemPicker<AssetItem>.IAdapter.items => this.m_FilteredItems;

    bool ItemPicker<AssetItem>.IAdapter.Update()
    {
      int num = this.m_FilteredItemsChanged ? 1 : 0;
      this.m_FilteredItemsChanged = false;
      return num != 0;
    }

    void ItemPicker<AssetItem>.IAdapter.SetFavorite(int index, bool favorite)
    {
      AssetItem filteredItem = this.m_FilteredItems[index];
      string str = filteredItem.guid.ToString();
      if (favorite)
        this.m_FavoriteIds.Add(str);
      else
        this.m_FavoriteIds.Remove(str);
      EditorSettings editor = SharedSettings.instance?.editor;
      if (editor != null)
        editor.assetPickerFavorites = this.m_FavoriteIds.ToArray<string>();
      filteredItem.favorite = favorite;
      this.m_Items.Sort();
      this.m_FilteredItems.Sort();
      this.m_FilteredItemsChanged = true;
    }

    public string searchQuery
    {
      get => this.m_SearchQuery;
      set
      {
        if (!(value != this.m_SearchQuery))
          return;
        this.m_SearchQuery = value;
        this.UpdateFilteredItems();
      }
    }

    int ItemPickerFooter.IAdapter.length => this.m_FilteredItems.Count;

    public int columnCount
    {
      get => this.m_ColumnCount;
      set
      {
        if (value == this.m_ColumnCount)
          return;
        this.m_ColumnCount = value;
        if (!this.m_UseGlobalColumnCount)
          return;
        EditorSettings editor = SharedSettings.instance?.editor;
        if (editor == null)
          return;
        editor.assetPickerColumnCount = value;
      }
    }
  }
}
