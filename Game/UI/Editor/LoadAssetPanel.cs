// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.LoadAssetPanel
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Editor
{
  public class LoadAssetPanel : EditorPanelBase
  {
    private LoadAssetPanel.LoadCallback m_ConfirmCallback;
    private AssetPickerAdapter m_Adapter;

    public LoadAssetPanel(
      LocalizedString panelTitle,
      IEnumerable<AssetItem> items,
      LoadAssetPanel.LoadCallback onConfirm,
      Action onClose)
    {
      this.m_ConfirmCallback = onConfirm;
      this.m_Adapter = new AssetPickerAdapter(items);
      this.title = panelTitle;
      IWidget[] widgetArray = new IWidget[4]
      {
        (IWidget) new SearchField()
        {
          adapter = (SearchField.IAdapter) this.m_Adapter
        },
        (IWidget) new ItemPicker<AssetItem>()
        {
          adapter = (ItemPicker<AssetItem>.IAdapter) this.m_Adapter,
          hasFavorites = true
        },
        (IWidget) new ItemPickerFooter()
        {
          adapter = (ItemPickerFooter.IAdapter) this.m_Adapter
        },
        null
      };
      Button[] children = new Button[2];
      Button button1 = new Button();
      button1.displayName = (LocalizedString) "Editor.LOAD";
      button1.disabled = (Func<bool>) (() => this.m_Adapter.selectedItem == null);
      button1.action = new Action(this.OnConfirm);
      children[0] = button1;
      Button button2 = new Button();
      button2.displayName = (LocalizedString) "Common.CANCEL";
      button2.action = onClose;
      children[1] = button2;
      widgetArray[3] = (IWidget) ButtonRow.WithChildren(children);
      this.children = (IList<IWidget>) widgetArray;
    }

    private void OnConfirm() => this.m_ConfirmCallback(this.m_Adapter.selectedItem.guid);

    public delegate void LoadCallback(Hash128 guid);
  }
}
