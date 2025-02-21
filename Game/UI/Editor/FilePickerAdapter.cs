// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.FilePickerAdapter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.UI.Editor
{
  public class FilePickerAdapter : 
    ItemPicker<FileItem>.IAdapter,
    SearchField.IAdapter,
    ItemPickerFooter.IAdapter
  {
    private string m_SearchQuery = string.Empty;
    private List<FileItem> m_Items;
    private List<FileItem> m_FilteredItems;
    private bool m_FilteredItemsChanged;
    [CanBeNull]
    private FileItem m_SelectedItem;
    private int m_ColumnCount;
    public Action<FileItem> EventItemSelected;

    [CanBeNull]
    public FileItem selectedItem
    {
      get => this.m_SelectedItem;
      set => this.m_SelectedItem = value;
    }

    public FilePickerAdapter(IEnumerable<FileItem> items)
    {
      this.m_Items = items.ToList<FileItem>();
      this.m_Items.Sort();
      this.m_FilteredItems = new List<FileItem>((IEnumerable<FileItem>) this.m_Items);
      EditorSettings editor = SharedSettings.instance?.editor;
      this.m_ColumnCount = editor != null ? editor.assetPickerColumnCount : 4;
    }

    public FileItem SelectItemByName(string name, StringComparison comparisonType)
    {
      this.m_SelectedItem = this.m_Items.FirstOrDefault<FileItem>((Func<FileItem, bool>) (item => item.path.Equals(name, comparisonType)));
      return this.m_SelectedItem;
    }

    private void UpdateFilteredItems()
    {
      this.m_FilteredItems.Clear();
      this.m_FilteredItems.AddRange(!string.IsNullOrEmpty(this.m_SearchQuery) ? this.m_Items.Where<FileItem>((Func<FileItem, bool>) (item => item.path.IndexOf(this.m_SearchQuery, StringComparison.OrdinalIgnoreCase) != -1)) : (IEnumerable<FileItem>) this.m_Items);
      this.m_FilteredItemsChanged = true;
    }

    FileItem ItemPicker<FileItem>.IAdapter.selectedItem
    {
      get => this.m_SelectedItem;
      set
      {
        this.m_SelectedItem = value;
        Action<FileItem> eventItemSelected = this.EventItemSelected;
        if (eventItemSelected == null)
          return;
        eventItemSelected(this.m_SelectedItem);
      }
    }

    List<FileItem> ItemPicker<FileItem>.IAdapter.items => this.m_FilteredItems;

    bool ItemPicker<FileItem>.IAdapter.Update()
    {
      int num = this.m_FilteredItemsChanged ? 1 : 0;
      this.m_FilteredItemsChanged = false;
      return num != 0;
    }

    void ItemPicker<FileItem>.IAdapter.SetFavorite(int index, bool favorite)
    {
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
        EditorSettings editor = SharedSettings.instance?.editor;
        if (editor == null)
          return;
        editor.assetPickerColumnCount = value;
      }
    }
  }
}
