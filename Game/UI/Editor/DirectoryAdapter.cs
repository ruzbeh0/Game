// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.DirectoryAdapter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.UI.Editor
{
  public class DirectoryAdapter : ItemPicker<Item>.IAdapter, SearchField.IAdapter
  {
    private DirectoryPanelBase m_Panel;
    private Item m_SelectedItem;
    private List<Item> m_Items;
    private bool m_Dirty;
    private HashSet<string> m_FavoriteIds = new HashSet<string>();
    public string searchQuery = string.Empty;
    private string m_SearchQuery = string.Empty;

    public DirectoryAdapter(DirectoryPanelBase panel) => this.m_Panel = panel;

    public string directoryPath { get; set; }

    public Item selectedItem
    {
      get => this.m_SelectedItem;
      set
      {
        if (value == this.m_SelectedItem)
          return;
        this.m_SelectedItem = value;
        this.m_Panel.OnSelect(value);
      }
    }

    public List<Item> items
    {
      get
      {
        return this.m_Items.Where<Item>((Func<Item, bool>) (item => ((string.IsNullOrEmpty(this.searchQuery) ? (item.parentDir == this.directoryPath ? 1 : 0) : (this.directoryPath == null ? 1 : (item.fullName == null || !item.fullName.StartsWith(this.directoryPath) ? 0 : (item.name + "/" != this.directoryPath ? 1 : 0)))) & (item.name.IndexOf(this.searchQuery, StringComparison.OrdinalIgnoreCase) != -1 ? 1 : 0)) != 0)).ToList<Item>();
      }
      set
      {
        if (value == this.m_Items)
          return;
        this.m_Items = value;
        this.m_FavoriteIds.Clear();
        EditorSettings editor = SharedSettings.instance?.editor;
        if (editor?.directoryPickerFavorites != null)
          this.m_FavoriteIds.UnionWith((IEnumerable<string>) editor.directoryPickerFavorites);
        foreach (Item obj in this.m_Items)
          obj.favorite = this.m_FavoriteIds.Contains(obj.relativePath);
        this.m_Items.Sort();
        this.m_Dirty = true;
      }
    }

    int ItemPicker<Item>.IAdapter.columnCount => 1;

    bool ItemPicker<Item>.IAdapter.Update()
    {
      int num = this.m_Dirty ? 1 : 0;
      this.m_Dirty = false;
      return num != 0;
    }

    void ItemPicker<Item>.IAdapter.SetFavorite(int index, bool favorite)
    {
      Item obj = this.items[index];
      string relativePath = obj.relativePath;
      if (favorite)
        this.m_FavoriteIds.Add(relativePath);
      else
        this.m_FavoriteIds.Remove(relativePath);
      EditorSettings editor = SharedSettings.instance?.editor;
      if (editor != null)
        editor.directoryPickerFavorites = this.m_FavoriteIds.ToArray<string>();
      obj.favorite = favorite;
      this.m_Items.Sort();
      this.items = this.m_Items;
      this.m_Dirty = true;
    }

    string SearchField.IAdapter.searchQuery
    {
      get => this.m_SearchQuery;
      set
      {
        if (!(value != this.m_SearchQuery))
          return;
        this.m_SearchQuery = value;
      }
    }
  }
}
