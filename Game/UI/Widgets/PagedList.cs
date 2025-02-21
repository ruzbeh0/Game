// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.PagedList
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;
using System.Collections.Generic;
using Unity.Assertions;
using UnityEngine;

#nullable disable
namespace Game.UI.Widgets
{
  public class PagedList : 
    NamedWidgetWithTooltip,
    IExpandable,
    IPaged,
    IListWidget,
    IWidget,
    IJsonWritable,
    IContainerWidget,
    IDisableCallback
  {
    private int m_Length;
    private int m_CurrentPageIndex;
    private int m_ChildStartIndex;
    private int m_ChildEndIndex;
    private List<IWidget> m_Children = new List<IWidget>();
    private bool m_Expanded;

    public bool expanded
    {
      get => this.m_Expanded;
      set
      {
        this.m_Expanded = value;
        this.SetPropertiesChanged();
        this.SetChildrenChanged();
      }
    }

    public IListAdapter adapter { get; set; }

    public int level { get; set; }

    public int pageSize { get; set; } = 10;

    public int pageCount => (this.m_Length + this.pageSize - 1) / this.pageSize;

    public IList<IWidget> children => (IList<IWidget>) this.m_Children;

    public int currentPageIndex
    {
      get => this.m_CurrentPageIndex;
      set
      {
        this.m_CurrentPageIndex = Mathf.Clamp(value, 0, this.pageCount - 1);
        this.CalculateChildIndices(this.m_CurrentPageIndex, this.m_Length, out this.m_ChildStartIndex, out this.m_ChildEndIndex);
        this.SetPropertiesChanged();
      }
    }

    public override IList<IWidget> visibleChildren
    {
      get
      {
        return !this.expanded ? (IList<IWidget>) Array.Empty<IWidget>() : (IList<IWidget>) this.m_Children;
      }
    }

    public int AddElement()
    {
      int elementIndex = this.adapter.AddElement();
      this.ShowElement(elementIndex);
      return elementIndex;
    }

    public void InsertElement(int index) => this.adapter.InsertElement(index);

    public int DuplicateElement(int index)
    {
      int elementIndex = this.adapter.DuplicateElement(index);
      this.ShowElement(elementIndex);
      return elementIndex;
    }

    public void MoveElement(int fromIndex, int toIndex)
    {
      this.adapter.MoveElement(fromIndex, toIndex);
    }

    public void DeleteElement(int index) => this.adapter.DeleteElement(index);

    public void Clear() => this.adapter.Clear();

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      int length = this.adapter.length;
      if (length != this.m_Length)
      {
        widgetChanges |= WidgetChanges.Properties;
        this.m_Length = length;
      }
      int pageIndex;
      int childStartIndex;
      int childEndIndex;
      this.CalculateIndices(this.m_ChildStartIndex, this.m_Length, out pageIndex, out childStartIndex, out childEndIndex);
      if (pageIndex != this.m_CurrentPageIndex || childStartIndex != this.m_ChildStartIndex || childEndIndex != this.m_ChildEndIndex)
      {
        widgetChanges |= WidgetChanges.Properties;
        this.m_CurrentPageIndex = pageIndex;
        this.m_ChildStartIndex = childStartIndex;
        this.m_ChildEndIndex = childEndIndex;
      }
      if (this.adapter.UpdateRange(this.m_ChildStartIndex, this.m_ChildEndIndex))
      {
        if (this.m_Expanded)
          widgetChanges |= WidgetChanges.Children;
        this.m_Children.Clear();
        this.m_Children.AddRange(this.adapter.BuildElementsInRange());
        Assert.AreEqual(this.m_ChildEndIndex - this.m_ChildStartIndex, this.m_Children.Count);
        if (this.m_Disabled)
        {
          foreach (IWidget child in this.m_Children)
            this.DisableChildren(child);
        }
      }
      return widgetChanges;
    }

    private void DisableChildren(IWidget child)
    {
      if (child is IDisableCallback disableCallback)
        disableCallback.disabled = (Func<bool>) (() => true);
      if (!(child is IContainerWidget containerWidget))
        return;
      foreach (IWidget child1 in (IEnumerable<IWidget>) containerWidget.children)
        this.DisableChildren(child1);
    }

    private void ShowElement(int elementIndex)
    {
      if (elementIndex == -1)
        return;
      this.CalculateIndices(elementIndex, this.adapter.length, out this.m_CurrentPageIndex, out this.m_ChildStartIndex, out this.m_ChildEndIndex);
    }

    private void CalculateIndices(
      int elementIndex,
      int length,
      out int pageIndex,
      out int childStartIndex,
      out int childEndIndex)
    {
      pageIndex = Mathf.Min(elementIndex / this.pageSize, Math.Max(0, (length + this.pageSize - 1) / this.pageSize - 1));
      this.CalculateChildIndices(pageIndex, length, out childStartIndex, out childEndIndex);
    }

    private void CalculateChildIndices(
      int pageIndex,
      int length,
      out int childStartIndex,
      out int childEndIndex)
    {
      childStartIndex = pageIndex * this.pageSize;
      childEndIndex = Mathf.Min((pageIndex + 1) * this.pageSize, length);
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("expanded");
      writer.Write(this.expanded);
      writer.PropertyName("resizable");
      writer.Write(this.adapter.resizable);
      writer.PropertyName("sortable");
      writer.Write(this.adapter.sortable);
      writer.PropertyName("length");
      writer.Write(this.m_Length);
      writer.PropertyName("currentPageIndex");
      writer.Write(this.currentPageIndex);
      writer.PropertyName("pageCount");
      writer.Write(this.pageCount);
      writer.PropertyName("childStartIndex");
      writer.Write(this.m_ChildStartIndex);
      writer.PropertyName("childEndIndex");
      writer.Write(this.m_ChildEndIndex);
    }
  }
}
