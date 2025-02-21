// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.PrefabPickerAdapter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Prefabs;
using Game.Settings;
using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.UI.Editor
{
  public class PrefabPickerAdapter : 
    ItemPicker<PrefabItem>.IAdapter,
    PopupSearchField.IAdapter,
    SearchField.IAdapter,
    ItemPickerFooter.IAdapter,
    FilterMenu.IAdapter
  {
    public const int kMaxHistoryLength = 100;
    public const int kMaxHistoryDisplayLength = 20;
    private ImageSystem m_ImageSystem;
    private string m_SearchQuery = string.Empty;
    private bool m_SearchQueryIsFavorite;
    private bool m_SearchQueryChanged;
    private List<PrefabItem> m_Items = new List<PrefabItem>();
    private bool m_ItemsChanged;
    private List<string> m_AvailableFilters = new List<string>();
    private List<string> m_ActiveFilters = new List<string>();
    private bool m_ActiveFiltersChanged;
    private List<PrefabItem> m_FilteredItems = new List<PrefabItem>();
    private bool m_FilteredItemsChanged;
    private PrefabItem m_SelectedItem;
    private PrefabBase m_SelectedPrefab;
    private List<string> m_SearchHistory = new List<string>();
    private HashSet<string> m_SearchFavorites = new HashSet<string>();
    private List<PopupSearchField.Suggestion> m_SearchSuggestions = new List<PopupSearchField.Suggestion>();
    private HashSet<string> m_FavoriteIds = new HashSet<string>();
    private int m_ColumnCount;
    public Action<PrefabBase> EventPrefabSelected;

    public bool displayPrefabTypeTooltip { get; set; }

    [CanBeNull]
    public PrefabBase selectedPrefab
    {
      get => this.m_SelectedPrefab;
      set => this.m_SelectedPrefab = value;
    }

    public List<string> availableFilters => this.m_AvailableFilters;

    public List<string> activeFilters => this.m_ActiveFilters;

    public Action onAvailableFiltersChanged { get; set; }

    public void LoadSettings()
    {
      this.m_SearchHistory.Clear();
      this.m_SearchFavorites.Clear();
      this.m_ColumnCount = 1;
      this.m_FavoriteIds.Clear();
      EditorSettings editor = SharedSettings.instance?.editor;
      if (editor == null)
        return;
      if (editor.prefabPickerSearchHistory != null)
        this.m_SearchHistory.AddRange((IEnumerable<string>) editor.prefabPickerSearchHistory);
      if (editor.prefabPickerSearchFavorites != null)
      {
        foreach (string pickerSearchFavorite in editor.prefabPickerSearchFavorites)
          this.m_SearchFavorites.Add(pickerSearchFavorite);
      }
      this.m_ColumnCount = editor.prefabPickerColumnCount;
      if (editor.prefabPickerFavorites == null)
        return;
      foreach (string prefabPickerFavorite in editor.prefabPickerFavorites)
        this.m_FavoriteIds.Add(prefabPickerFavorite);
    }

    public void SetPrefabs([ItemCanBeNull] ICollection<PrefabBase> prefabs)
    {
      this.m_Items.Clear();
      this.m_Items.Capacity = prefabs.Count;
      this.m_ItemsChanged = true;
      this.m_SelectedItem = (PrefabItem) null;
      this.m_AvailableFilters.Clear();
      this.m_ActiveFilters.Clear();
      HashSet<string> collection = new HashSet<string>();
      foreach (PrefabBase prefab in (IEnumerable<PrefabBase>) prefabs)
      {
        string prefabId = EditorPrefabUtils.GetPrefabID(prefab);
        PrefabItem prefabItem1 = new PrefabItem();
        prefabItem1.prefab = prefab;
        prefabItem1.displayName = EditorPrefabUtils.GetPrefabLabel(prefab);
        PrefabItem prefabItem2 = prefabItem1;
        if ((UnityEngine.Object) prefab != (UnityEngine.Object) null && this.displayPrefabTypeTooltip)
          prefabItem2.tooltip = new LocalizedString?(LocalizedString.Value(prefab.GetType().Name));
        if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
        {
          prefabItem2.tags.AddRange((IEnumerable<string>) EditorPrefabUtils.GetPrefabTags(prefab.GetType()));
          foreach (ComponentBase component in prefab.components)
            prefabItem2.tags.Add(component.GetType().Name.ToLowerInvariant());
          foreach (string tag in prefabItem2.tags)
            collection.Add(tag);
          // ISSUE: reference to a compiler-generated method
          prefabItem2.image = ImageSystem.GetThumbnail(prefab);
        }
        if (prefabId != null)
          prefabItem2.favorite = this.m_FavoriteIds.Contains(prefabId);
        this.m_Items.Add(prefabItem2);
      }
      this.m_AvailableFilters.AddRange((IEnumerable<string>) collection);
      this.m_AvailableFilters.Sort();
      Action availableFiltersChanged = this.onAvailableFiltersChanged;
      if (availableFiltersChanged != null)
        availableFiltersChanged();
      this.m_Items.Sort();
    }

    public PrefabBase SelectPrefabByName(string name, StringComparison comparisonType)
    {
      this.m_SelectedPrefab = this.m_Items.Select<PrefabItem, PrefabBase>((Func<PrefabItem, PrefabBase>) (item => item.prefab)).FirstOrDefault<PrefabBase>((Func<PrefabBase, bool>) (prefab => prefab.name.Equals(name, comparisonType)));
      return this.m_SelectedPrefab;
    }

    public void Update()
    {
      if ((UnityEngine.Object) this.m_SelectedPrefab != (UnityEngine.Object) this.m_SelectedItem?.prefab)
        this.m_SelectedItem = this.m_Items.FirstOrDefault<PrefabItem>((Func<PrefabItem, bool>) (item => (UnityEngine.Object) item.prefab == (UnityEngine.Object) this.m_SelectedPrefab));
      if (!this.m_ItemsChanged && !this.m_SearchQueryChanged && !this.m_ActiveFiltersChanged)
        return;
      this.m_ItemsChanged = false;
      this.m_SearchQueryChanged = false;
      this.m_ActiveFiltersChanged = false;
      this.UpdateFilteredItems();
    }

    private void UpdateFilteredItems()
    {
      this.m_SearchSuggestions.Clear();
      this.m_FilteredItems.Clear();
      this.m_FilteredItemsChanged = true;
      string[] strArray = this.m_SearchQuery.Split(' ', StringSplitOptions.None);
      string[] words = ((IEnumerable<string>) strArray).Where<string>((Func<string, bool>) (p => p.Length > 0 && !p.StartsWith("#"))).ToArray<string>();
      string[] tags = ((IEnumerable<string>) strArray).Take<string>(strArray.Length - 1).Where<string>((Func<string, bool>) (p => p.Length > 1 && p.StartsWith("#"))).Select<string, string>((Func<string, string>) (p => p.Substring(1))).Concat<string>((IEnumerable<string>) this.m_ActiveFilters).ToArray<string>();
      string incompleteTag = PrefabPickerAdapter.GetIncompleteTag(strArray);
      if (words.Length != 0 || tags.Length != 0 || incompleteTag != null)
      {
        this.m_SearchSuggestions.AddRange(this.m_SearchHistory.Where<string>((Func<string, bool>) (s => !this.m_SearchFavorites.Contains(s) && s.StartsWith(this.m_SearchQuery, StringComparison.OrdinalIgnoreCase))).Take<string>(20).Select<string, PopupSearchField.Suggestion>(new Func<string, PopupSearchField.Suggestion>(PopupSearchField.Suggestion.NonFavorite)));
        this.m_SearchSuggestions.AddRange(this.m_SearchFavorites.Where<string>((Func<string, bool>) (s => s.StartsWith(this.m_SearchQuery, StringComparison.OrdinalIgnoreCase))).Select<string, PopupSearchField.Suggestion>(new Func<string, PopupSearchField.Suggestion>(PopupSearchField.Suggestion.Favorite)));
        this.m_FilteredItems.AddRange(this.m_Items.Where<PrefabItem>((Func<PrefabItem, bool>) (item =>
        {
          if ((UnityEngine.Object) item.prefab == (UnityEngine.Object) null)
            return false;
          int num1 = words.Length == 0 ? 1 : (((IEnumerable<string>) words).All<string>((Func<string, bool>) (word => item.prefab.name.IndexOf(word, StringComparison.OrdinalIgnoreCase) != -1)) ? 1 : 0);
          string typeName = item.prefab.GetType().Name;
          bool flag1 = ((IEnumerable<string>) words).Any<string>((Func<string, bool>) (word => word.IndexOf(typeName, StringComparison.OrdinalIgnoreCase) != -1));
          bool flag2 = tags.Length == 0 || ((IEnumerable<string>) tags).Any<string>((Func<string, bool>) (tag => item.tags.Contains<string>(tag, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)));
          bool flag3 = incompleteTag == null || item.tags.Any<string>((Func<string, bool>) (tag => tag.StartsWith(incompleteTag, StringComparison.OrdinalIgnoreCase)));
          int num2 = flag1 ? 1 : 0;
          return ((num1 | num2) & (flag2 ? 1 : 0) & (flag3 ? 1 : 0)) != 0;
        })));
      }
      else
      {
        this.m_SearchSuggestions.AddRange(this.m_SearchHistory.Where<string>((Func<string, bool>) (s => !this.m_SearchFavorites.Contains(s))).Take<string>(20).Select<string, PopupSearchField.Suggestion>(new Func<string, PopupSearchField.Suggestion>(PopupSearchField.Suggestion.NonFavorite)));
        this.m_SearchSuggestions.AddRange(this.m_SearchFavorites.Select<string, PopupSearchField.Suggestion>(new Func<string, PopupSearchField.Suggestion>(PopupSearchField.Suggestion.Favorite)));
        this.m_FilteredItems.AddRange((IEnumerable<PrefabItem>) this.m_Items);
      }
      this.m_SearchSuggestions.Sort();
    }

    [CanBeNull]
    private static string GetIncompleteTag(string[] searchParts)
    {
      if (searchParts.Length == 0)
        return (string) null;
      string searchPart = searchParts[searchParts.Length - 1];
      return searchPart.Length <= 1 || !searchPart.StartsWith("#") ? (string) null : searchPart.Substring(1);
    }

    PrefabItem ItemPicker<PrefabItem>.IAdapter.selectedItem
    {
      get => this.m_SelectedItem;
      set
      {
        this.m_SelectedItem = value;
        this.m_SelectedPrefab = value?.prefab;
        Action<PrefabBase> eventPrefabSelected = this.EventPrefabSelected;
        if (eventPrefabSelected != null)
          eventPrefabSelected(this.m_SelectedPrefab);
        if (string.IsNullOrEmpty(this.m_SearchQuery.Trim()) || !this.m_FilteredItems.Contains(this.m_SelectedItem))
          return;
        this.m_SearchHistory.Remove(this.m_SearchQuery);
        this.m_SearchHistory.Insert(0, this.m_SearchQuery);
        if (this.m_SearchHistory.Count > 100)
          this.m_SearchHistory.RemoveRange(100, this.m_SearchHistory.Count - 100);
        EditorSettings editor = SharedSettings.instance?.editor;
        if (editor == null)
          return;
        editor.prefabPickerSearchHistory = this.m_SearchHistory.ToArray();
      }
    }

    List<PrefabItem> ItemPicker<PrefabItem>.IAdapter.items => this.m_FilteredItems;

    bool ItemPicker<PrefabItem>.IAdapter.Update()
    {
      int num = this.m_FilteredItemsChanged ? 1 : 0;
      this.m_FilteredItemsChanged = false;
      return num != 0;
    }

    void ItemPicker<PrefabItem>.IAdapter.SetFavorite(int index, bool favorite)
    {
      PrefabItem filteredItem = this.m_FilteredItems[index];
      string prefabId = EditorPrefabUtils.GetPrefabID(filteredItem.prefab);
      if (prefabId == null)
        return;
      if (favorite)
        this.m_FavoriteIds.Add(prefabId);
      else
        this.m_FavoriteIds.Remove(prefabId);
      EditorSettings editor = SharedSettings.instance?.editor;
      if (editor != null)
        editor.prefabPickerFavorites = this.m_FavoriteIds.ToArray<string>();
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
        this.m_SearchQueryIsFavorite = this.m_SearchFavorites.Contains(value);
        this.m_SearchQueryChanged = true;
      }
    }

    public bool searchQueryIsFavorite => this.m_SearchQueryIsFavorite;

    IEnumerable<PopupSearchField.Suggestion> PopupSearchField.IAdapter.searchSuggestions
    {
      get => (IEnumerable<PopupSearchField.Suggestion>) this.m_SearchSuggestions;
    }

    void PopupSearchField.IAdapter.SetFavorite(string query, bool favorite)
    {
      if (favorite)
        this.m_SearchFavorites.Add(query);
      else
        this.m_SearchFavorites.Remove(query);
      if (query == this.m_SearchQuery)
        this.m_SearchQueryIsFavorite = favorite;
      EditorSettings editor = SharedSettings.instance?.editor;
      if (editor != null)
      {
        editor.prefabPickerSearchFavorites = this.m_SearchFavorites.ToArray<string>();
        editor.prefabPickerSearchHistory = this.m_SearchHistory.ToArray();
      }
      this.UpdateFilteredItems();
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
        editor.prefabPickerColumnCount = value;
      }
    }

    public void ToggleFilter(string filter, bool active)
    {
      if (active)
      {
        if (!this.m_ActiveFilters.Contains(filter))
          this.m_ActiveFilters.Add(filter);
      }
      else
        this.m_ActiveFilters.Remove(filter);
      this.m_ActiveFiltersChanged = true;
    }

    public void ClearFilters()
    {
      this.m_ActiveFilters.Clear();
      this.m_ActiveFiltersChanged = true;
    }
  }
}
