// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.DirectoryPanelBase
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Logging;
using Colossal.UI;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.UI.Editor
{
  public abstract class DirectoryPanelBase : EditorPanelBase, SearchField.IAdapter
  {
    protected static ILog log = UIManager.log;
    protected const char kDirSeparator = '/';
    protected List<Item> m_Items;
    protected Dictionary<string, Item> m_Directories = new Dictionary<string, Item>();
    protected LocalizedString m_RootDirName;
    protected PageView m_PageView;
    protected readonly List<DirectoryAdapter> m_Stack = new List<DirectoryAdapter>();
    protected readonly List<IWidget> m_Pages = new List<IWidget>();

    public abstract void OnSelect(Item item);

    protected virtual void ShowSubDir(string dir)
    {
      DirectoryAdapter adapter = this.BuildAdapter(dir);
      this.m_Stack.Add(adapter);
      this.m_Pages.Add(this.BuildPage(adapter));
      this.m_PageView.children = (IList<IWidget>) this.m_Pages.ToArray();
      this.m_PageView.currentPage = this.m_Stack.Count - 1;
    }

    private DirectoryAdapter BuildAdapter(string dir)
    {
      return new DirectoryAdapter(this)
      {
        directoryPath = dir,
        items = this.m_Items.ToList<Item>()
      };
    }

    private IWidget BuildPage(DirectoryAdapter adapter)
    {
      PageLayout pageLayout = new PageLayout();
      pageLayout.title = adapter.directoryPath != null ? this.m_Directories[adapter.directoryPath].displayName : this.m_RootDirName;
      pageLayout.backAction = adapter.directoryPath != null ? new Action(this.OnBack) : (Action) null;
      pageLayout.children = (IList<IWidget>) new IWidget[1]
      {
        (IWidget) new ItemPicker<Item>()
        {
          adapter = (ItemPicker<Item>.IAdapter) adapter,
          hasFavorites = true,
          hasImages = false
        }
      };
      return (IWidget) pageLayout;
    }

    protected virtual void OnBack()
    {
      if (this.m_Stack.Count <= 1)
        return;
      this.m_Stack.RemoveAt(this.m_Stack.Count - 1);
      this.m_Stack.Last<DirectoryAdapter>().selectedItem = (Item) null;
      this.m_Pages.RemoveAt(this.m_Pages.Count - 1);
      this.m_PageView.children = (IList<IWidget>) this.m_Pages.ToArray();
      this.m_PageView.currentPage = this.m_Stack.Count - 1;
    }

    string SearchField.IAdapter.searchQuery
    {
      get => this.m_Stack.Last<DirectoryAdapter>().searchQuery;
      set => this.m_Stack.Last<DirectoryAdapter>().searchQuery = value;
    }
  }
}
