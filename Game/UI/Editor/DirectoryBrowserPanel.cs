// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.DirectoryBrowserPanel
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Debug;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine.InputSystem.Utilities;

#nullable disable
namespace Game.UI.Editor
{
  public class DirectoryBrowserPanel : DirectoryPanelBase
  {
    private readonly DirectoryBrowserPanel.SelectCallback m_SelectCallback;
    private readonly Action m_OnCloseDirectoryBrowser;
    private string m_SelectedDirectory;
    private string m_RootDirectory;
    private bool m_LimitDepthToRoot;

    private bool ImportNotReady() => true;

    private void ImportAssets()
    {
    }

    public DirectoryBrowserPanel(
      string directory,
      string root,
      DirectoryBrowserPanel.SelectCallback onSelect,
      Action onCancel)
    {
      this.m_LimitDepthToRoot = !string.IsNullOrEmpty(root);
      this.m_RootDirectory = root + "/";
      this.m_SelectCallback = onSelect;
      this.m_OnCloseDirectoryBrowser = onCancel;
      this.ListDrives();
      try
      {
        if (directory != null)
        {
          while (!Directory.Exists(directory))
            directory = Path.GetDirectoryName(directory);
          this.TraverseToDirectory(directory);
        }
      }
      catch (ArgumentException ex)
      {
        DirectoryPanelBase.log.Error((Exception) ex);
      }
      this.m_SelectedDirectory = this.m_Stack.LastOrDefault<DirectoryAdapter>()?.directoryPath;
    }

    private void TraverseToDirectory(string directory)
    {
      DirectoryInfo directoryInfo1 = new DirectoryInfo(directory);
      List<string> stringList = new List<string>();
      for (DirectoryInfo directoryInfo2 = directoryInfo1; directoryInfo2 != null; directoryInfo2 = directoryInfo2.Parent)
      {
        string str = directoryInfo2.Parent == null ? directoryInfo2.Name.TrimEnd('\\') : directoryInfo2.Name;
        stringList.Add(str);
      }
      stringList.Reverse();
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string str in stringList)
      {
        stringBuilder.Append(str);
        stringBuilder.Append('/');
        this.ShowSubDir(stringBuilder.ToString());
      }
    }

    private void ListDrives()
    {
      DriveInfo[] drives = DriveInfo.GetDrives();
      List<Item> items = new List<Item>();
      foreach (DriveInfo driveInfo in drives)
      {
        DirectoryInfo directoryInfo1 = new DirectoryInfo(driveInfo.VolumeLabel);
        try
        {
          DirectoryInfo directoryInfo2 = directoryInfo1;
          foreach (DirectoryInfo directory in directoryInfo2.GetDirectories("*", new EnumerationOptions()
          {
            IgnoreInaccessible = true
          }))
          {
            Item obj = new Item();
            obj.name = directory.Name;
            obj.tooltip = new LocalizedString?((LocalizedString) directory.Name);
            obj.fullName = directory.FullName.Replace("\\", "/");
            obj.parentDir = driveInfo.VolumeLabel.Replace("\\", "");
            obj.directory = true;
            items.Add(obj);
          }
        }
        catch (IOException ex)
        {
        }
      }
      this.ListItems("Editor.DRIVES", items);
      this.ShowSubDir((string) null);
    }

    private void ListItems(string directory, List<Item> items)
    {
      if (items.Count == 0)
        return;
      this.m_Directories.Clear();
      this.m_Items = items;
      this.m_RootDirName = (LocalizedString) directory;
      foreach (Item obj1 in this.m_Items)
      {
        Assert.IsNotNull<string>(obj1.name);
        if (obj1.displayName.isEmpty)
          obj1.displayName = (LocalizedString) obj1.name;
        if (obj1.parentDir != null)
        {
          string[] strArray = obj1.parentDir.Split(new char[1]
          {
            '/'
          }, StringSplitOptions.RemoveEmptyEntries);
          string key1 = string.Empty;
          string str1 = (string) null;
          foreach (string str2 in strArray)
          {
            key1 = key1 + str2 + "/";
            if (!this.m_Directories.ContainsKey(key1))
            {
              Dictionary<string, Item> directories = this.m_Directories;
              string key2 = key1;
              Item obj2 = new Item();
              obj2.displayName = LocalizedString.Value(str2);
              obj2.directory = true;
              obj2.parentDir = str1;
              obj2.name = str2;
              obj2.tooltip = new LocalizedString?((LocalizedString) str1);
              directories.Add(key2, obj2);
            }
            str1 = key1;
          }
          obj1.parentDir = key1;
        }
      }
      this.m_Items.AddRange((IEnumerable<Item>) this.m_Directories.Values);
      this.m_Items.Sort();
      this.title = (LocalizedString) "Editor.OPEN_DIRECTORY";
      IWidget[] widgetArray = new IWidget[4]
      {
        (IWidget) new SearchField()
        {
          adapter = (SearchField.IAdapter) this
        },
        null,
        null,
        null
      };
      PageView pageView = new PageView();
      pageView.currentPage = 0;
      pageView.children = (IList<IWidget>) new IWidget[0];
      widgetArray[1] = (IWidget) (this.m_PageView = pageView);
      Button button1 = new Button();
      button1.displayName = (LocalizedString) "Editor.SELECT_DIRECTORY";
      button1.action = new Action(this.OnSelectDirectory);
      button1.disabled = new Func<bool>(this.DirectoryNotSelectedOrRootSelected);
      widgetArray[2] = (IWidget) button1;
      Button button2 = new Button();
      button2.displayName = (LocalizedString) "Common.CANCEL";
      button2.action = this.m_OnCloseDirectoryBrowser;
      widgetArray[3] = (IWidget) button2;
      this.children = (IList<IWidget>) widgetArray;
    }

    private bool DirectoryNotSelectedOrRootSelected()
    {
      return this.m_SelectedDirectory == null || this.m_SelectedDirectory == this.m_RootDirectory;
    }

    private void OnSelectDirectory() => this.m_SelectCallback(this.m_SelectedDirectory);

    public override void OnSelect(Item item)
    {
      if (item == null)
        return;
      this.ShowSubDir(item.fullName == null ? item.name + "/" : item.fullName + "/");
    }

    protected override void OnBack()
    {
      if (this.m_Stack.Count <= 1)
        return;
      this.m_Stack.RemoveAt(this.m_Stack.Count - 1);
      this.m_Stack.Last<DirectoryAdapter>().selectedItem = (Item) null;
      this.m_Pages.RemoveAt(this.m_Pages.Count - 1);
      this.m_SelectedDirectory = this.m_Stack.LastOrDefault<DirectoryAdapter>()?.directoryPath;
      if (this.m_LimitDepthToRoot && this.m_SelectedDirectory == this.m_RootDirectory)
        this.m_Pages.Last<IWidget>().As<PageLayout>().backAction = (Action) null;
      this.m_PageView.children = (IList<IWidget>) this.m_Pages.ToArray();
      this.m_PageView.currentPage = this.m_Stack.Count - 1;
    }

    protected override void ShowSubDir(string dir)
    {
      if (!string.IsNullOrEmpty(dir))
      {
        this.m_SelectedDirectory = dir;
        if (this.m_Items != null)
          this.m_Items.Clear();
        try
        {
          List<Item> items = new List<Item>();
          DirectoryInfo directoryInfo1 = new DirectoryInfo(dir);
          foreach (DirectoryInfo directory in directoryInfo1.GetDirectories("*", new EnumerationOptions()
          {
            IgnoreInaccessible = true
          }))
          {
            Item obj1 = new Item();
            obj1.name = directory.Name;
            obj1.parentDir = dir;
            obj1.fullName = directory.FullName.Replace("\\", "/");
            obj1.directory = true;
            obj1.tooltip = new LocalizedString?((LocalizedString) directory.FullName);
            items.Add(obj1);
            DirectoryInfo[] directories = directory.GetDirectories("*", new EnumerationOptions()
            {
              IgnoreInaccessible = true
            });
            obj1.directory = directories.Length != 0;
            foreach (DirectoryInfo directoryInfo2 in directories)
            {
              Item obj2 = new Item();
              obj2.name = directoryInfo2.Name;
              obj2.parentDir = directory.Name;
              obj2.fullName = directoryInfo2.FullName.Replace("\\", "/");
              obj2.tooltip = new LocalizedString?((LocalizedString) directoryInfo2.FullName);
              items.Add(obj2);
            }
          }
          if (items.Count == 0)
            return;
          this.ListItems(dir, items);
        }
        catch (Exception ex)
        {
          DirectoryPanelBase.log.Error(ex, (object) ("Error showing directory '" + dir + "'"));
        }
      }
      base.ShowSubDir(dir);
      if (!this.m_LimitDepthToRoot || !(this.m_SelectedDirectory == this.m_RootDirectory))
        return;
      this.m_Pages.Last<IWidget>().As<PageLayout>().backAction = (Action) null;
    }

    public delegate void SelectCallback(string directory);
  }
}
