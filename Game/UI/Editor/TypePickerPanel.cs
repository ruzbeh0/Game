// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.TypePickerPanel
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Debug;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Game.UI.Editor
{
  public class TypePickerPanel : DirectoryPanelBase
  {
    private readonly TypePickerPanel.SelectCallback m_SelectCallback;

    public TypePickerPanel(
      LocalizedString panelTitle,
      LocalizedString rootDirName,
      IEnumerable<Item> items,
      TypePickerPanel.SelectCallback onSelect,
      Action onCancel)
    {
      this.m_RootDirName = rootDirName;
      this.m_SelectCallback = onSelect;
      this.m_Items = new List<Item>(items);
      foreach (Item obj1 in this.m_Items)
      {
        Assert.IsNotNull<Type>(obj1.type);
        Assert.IsNotNull<string>(obj1.name);
        Assert.IsFalse(obj1.directory);
        if (obj1.displayName.isEmpty)
          obj1.displayName = (LocalizedString) obj1.name;
        if (obj1.parentDir != null)
        {
          string[] strArray = obj1.parentDir.Split(new char[1]
          {
            '/'
          }, StringSplitOptions.RemoveEmptyEntries);
          string key1 = (string) null;
          foreach (string str1 in strArray)
          {
            string str2 = key1;
            if (key1 == null)
              key1 = string.Empty;
            key1 = key1 + str1 + "/";
            if (!this.m_Directories.ContainsKey(key1))
            {
              Dictionary<string, Item> directories = this.m_Directories;
              string key2 = key1;
              Item obj2 = new Item();
              obj2.displayName = LocalizedString.Value(str1);
              obj2.directory = true;
              obj2.parentDir = str2;
              obj2.name = str1;
              obj2.fullName = obj1.relativePath;
              directories.Add(key2, obj2);
            }
          }
          if (obj1.fullName == null)
            obj1.fullName = key1;
          obj1.parentDir = key1;
        }
      }
      this.m_Items.AddRange((IEnumerable<Item>) this.m_Directories.Values);
      this.title = panelTitle;
      IWidget[] widgetArray = new IWidget[3]
      {
        (IWidget) new SearchField()
        {
          adapter = (SearchField.IAdapter) this
        },
        null,
        null
      };
      PageView pageView = new PageView();
      pageView.currentPage = 0;
      pageView.children = (IList<IWidget>) new IWidget[0];
      widgetArray[1] = (IWidget) (this.m_PageView = pageView);
      Button button = new Button();
      button.displayName = (LocalizedString) "Common.CANCEL";
      button.action = onCancel;
      widgetArray[2] = (IWidget) button;
      this.children = (IList<IWidget>) widgetArray;
      this.ShowSubDir((string) null);
    }

    public override void OnSelect(Item item)
    {
      if (item == null)
        return;
      if (item.directory)
        this.ShowSubDir(item.relativePath + "/");
      else
        this.m_SelectCallback(item.type);
    }

    public static IEnumerable<Type> GetAllConcreteTypesDerivedFrom<T>()
    {
      return ((IEnumerable<Type>) Assembly.GetExecutingAssembly().GetTypes()).Where<Type>((Func<Type, bool>) (t => !t.IsAbstract && t.IsSubclassOf(typeof (T))));
    }

    public delegate void SelectCallback(Type type);
  }
}
