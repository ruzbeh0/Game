// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.HierarchyMenu`1
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
  public class HierarchyMenu<T> : HierarchyMenu
  {
    private HierarchyMenu.SelectionType m_SelectionType;
    private List<HierarchyItem<T>> m_Items = new List<HierarchyItem<T>>();
    private List<ViewportItem<T>> m_Viewport = new List<ViewportItem<T>>();
    private int m_ViewportStartIndex;
    private int m_ViewportEndIndex;
    private bool m_ViewportDirty;

    public override string propertiesTypeName => "Game.UI.Editor.HierarchyMenu";

    public FlexLayout flex { get; set; } = FlexLayout.Fill;

    public Action onSelectionChange { get; set; }

    public HierarchyMenu.SelectionType selectionType
    {
      get => this.m_SelectionType;
      set
      {
        if (this.m_SelectionType == value)
          return;
        this.m_SelectionType = value;
        this.ClearSelection();
        this.m_ViewportDirty = true;
        this.SetPropertiesChanged();
      }
    }

    public IEnumerable<HierarchyItem<T>> items
    {
      get
      {
        foreach (HierarchyItem<T> hierarchyItem in this.m_Items)
          yield return hierarchyItem;
      }
      set
      {
        this.m_Items.Clear();
        this.m_Items.AddRange(value);
        this.m_ViewportDirty = true;
        this.SetPropertiesChanged();
      }
    }

    public bool IsEmpty() => this.m_Items.Count == 0;

    public IEnumerable<T> GetSelectedItems()
    {
      for (int i = 0; i < this.m_Items.Count; ++i)
      {
        if (this.m_Items[i].m_Selected)
        {
          yield return this.m_Items[i].m_Data;
          switch (this.m_SelectionType)
          {
            case HierarchyMenu.SelectionType.singleSelection:
              yield break;
            case HierarchyMenu.SelectionType.inheritedMultiSelection:
              i += this.CountChildren(i);
              continue;
            default:
              continue;
          }
        }
      }
    }

    public bool GetSelectedItem(out T selection)
    {
      foreach (HierarchyItem<T> hierarchyItem in this.m_Items)
      {
        if (hierarchyItem.m_Selected)
        {
          selection = hierarchyItem.m_Data;
          return true;
        }
      }
      selection = default (T);
      return false;
    }

    protected override void OnSetRenderedRange(int start, int end)
    {
      this.m_ViewportStartIndex = start;
      this.m_ViewportEndIndex = end;
      this.m_ViewportDirty = true;
    }

    protected override void OnSetItemSelected(int viewportIndex, bool selected)
    {
      this.SetItemSelected(this.m_Viewport[viewportIndex].m_ItemIndex, selected);
    }

    public void SetItemSelected(int itemIndex, bool selected)
    {
      switch (this.m_SelectionType)
      {
        case HierarchyMenu.SelectionType.singleSelection:
          this.ClearSelection();
          this.SetItemSelectedImpl(itemIndex, true);
          break;
        case HierarchyMenu.SelectionType.multiSelection:
          this.SetItemSelectedImpl(itemIndex, selected);
          break;
        case HierarchyMenu.SelectionType.inheritedMultiSelection:
          this.SetItemSelectedImpl(itemIndex, selected);
          this.SetChildrenSelected(itemIndex, selected);
          this.PatchParentsSelected(itemIndex);
          break;
      }
      this.m_ViewportDirty = true;
      Action onSelectionChange = this.onSelectionChange;
      if (onSelectionChange == null)
        return;
      onSelectionChange();
    }

    protected override void OnSetItemExpanded(int viewportIndex, bool expanded)
    {
      this.SetItemExpanded(this.m_Viewport[viewportIndex].m_ItemIndex, expanded);
    }

    public void SetItemExpanded(int itemIndex, bool expanded)
    {
      HierarchyItem<T> hierarchyItem = this.m_Items[itemIndex] with
      {
        m_Expanded = expanded
      };
      this.m_Items[itemIndex] = hierarchyItem;
      this.m_ViewportDirty = true;
    }

    private void SetItemSelectedImpl(int itemIndex, bool selected)
    {
      HierarchyItem<T> hierarchyItem = this.m_Items[itemIndex] with
      {
        m_Selected = selected
      };
      this.m_Items[itemIndex] = hierarchyItem;
    }

    private void SetChildrenSelected(int itemIndex, bool selected)
    {
      int num1 = this.CountChildren(itemIndex);
      int num2 = itemIndex + 1;
      for (int index = num2; index < num2 + num1; ++index)
      {
        HierarchyItem<T> hierarchyItem = this.m_Items[index] with
        {
          m_Selected = selected
        };
        this.m_Items[index] = hierarchyItem;
      }
    }

    private void PatchParentsSelected(int itemIndex)
    {
      bool selected = true;
      int parentIndex;
      while (this.FindParent(itemIndex, out parentIndex))
      {
        if (selected)
        {
          int num1 = parentIndex + 1;
          int num2 = this.CountChildren(parentIndex);
          for (int index = num1; index < num1 + num2; index = index + this.CountChildren(index) + 1)
          {
            selected &= this.m_Items[index].m_Selected;
            if (!selected)
              break;
          }
        }
        this.SetItemSelectedImpl(parentIndex, selected);
      }
    }

    private void ClearSelection()
    {
      for (int itemIndex = 0; itemIndex < this.m_Items.Count; ++itemIndex)
        this.SetItemSelectedImpl(itemIndex, false);
    }

    private void RebuildViewport()
    {
      this.m_Viewport.Clear();
      int num = this.m_ViewportEndIndex - this.m_ViewportStartIndex;
      int itemIndex;
      if (!this.FindItemIndex(this.m_ViewportStartIndex, out itemIndex))
        return;
      for (int index = itemIndex; index < this.m_Items.Count && this.m_Viewport.Count <= num; ++index)
      {
        this.m_Viewport.Add(new ViewportItem<T>()
        {
          m_Item = this.m_Items[index],
          m_ItemIndex = index
        });
        if (!this.m_Items[index].m_Expanded)
          index += this.CountChildren(index);
      }
    }

    private bool FindItemIndex(int visibleIndex, out int itemIndex)
    {
      int num = 0;
      for (int index = 0; index < this.m_Items.Count; ++index)
      {
        if (num == visibleIndex)
        {
          itemIndex = index;
          return true;
        }
        ++num;
        if (!this.m_Items[index].m_Expanded)
          index += this.CountChildren(index);
      }
      itemIndex = -1;
      return false;
    }

    private int CountChildren(int itemIndex)
    {
      int level = this.m_Items[itemIndex].m_Level;
      int num = itemIndex + 1;
      for (int index = num; index < this.m_Items.Count; ++index)
      {
        if (this.m_Items[index].m_Level <= level)
          return index - num;
      }
      return this.m_Items.Count - num;
    }

    private bool FindParent(int itemIndex, out int parentIndex)
    {
      int level = this.m_Items[itemIndex].m_Level;
      for (int index = itemIndex; index >= 0; --index)
      {
        if (this.m_Items[index].m_Level < level)
        {
          parentIndex = index;
          return true;
        }
      }
      parentIndex = -1;
      return false;
    }

    private int CountVisibleItems()
    {
      int num = 0;
      for (int index = 0; index < this.m_Items.Count; ++index)
      {
        ++num;
        if (!this.m_Items[index].m_Expanded)
          index += this.CountChildren(index);
      }
      return num;
    }

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      if (this.m_ViewportDirty)
      {
        this.RebuildViewport();
        this.m_ViewportDirty = false;
        widgetChanges |= WidgetChanges.Properties;
      }
      return widgetChanges;
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("viewport");
      writer.Write<ViewportItem<T>>((IList<ViewportItem<T>>) this.m_Viewport);
      writer.PropertyName("singleSelection");
      writer.Write(this.m_SelectionType == HierarchyMenu.SelectionType.singleSelection);
      writer.PropertyName("visibleCount");
      int num = this.CountVisibleItems();
      writer.Write(num);
      writer.PropertyName("viewportStartIndex");
      writer.Write(this.m_ViewportStartIndex);
      writer.PropertyName("flex");
      writer.Write<FlexLayout>(this.flex);
    }
  }
}
