// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.SaveAssetPanel
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Game.Reflection;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Editor
{
  public class SaveAssetPanel : EditorPanelBase
  {
    private SaveAssetPanel.SaveCallback m_ConfirmCallback;
    private AssetPickerAdapter m_Adapter;
    private string m_FileName;

    public SaveAssetPanel(
      LocalizedString panelTitle,
      IEnumerable<AssetItem> items,
      Hash128? initialSelected,
      SaveAssetPanel.SaveCallback onConfirm,
      Action onCancel,
      LocalizedString saveButtonLabel = default (LocalizedString))
    {
      this.m_ConfirmCallback = onConfirm;
      this.m_Adapter = new AssetPickerAdapter(items);
      this.m_Adapter.EventItemSelected += new Action<AssetItem>(this.OnMapSelected);
      if (initialSelected.HasValue)
        this.m_Adapter.SelectItemByGuid(initialSelected.Value);
      this.m_FileName = this.m_Adapter.selectedItem?.fileName ?? string.Empty;
      this.title = panelTitle;
      IWidget[] widgetArray = new IWidget[4]
      {
        (IWidget) new ItemPicker<AssetItem>()
        {
          adapter = (ItemPicker<AssetItem>.IAdapter) this.m_Adapter,
          hasFavorites = true
        },
        (IWidget) new ItemPickerFooter()
        {
          adapter = (ItemPickerFooter.IAdapter) this.m_Adapter
        },
        null,
        null
      };
      StringInputField stringInputField = new StringInputField();
      stringInputField.displayName = (LocalizedString) "Editor.FILE_NAME";
      stringInputField.accessor = (ITypedValueAccessor<string>) new DelegateAccessor<string>((Func<string>) (() => this.m_FileName), new Action<string>(this.OnNameChange));
      widgetArray[2] = (IWidget) stringInputField;
      Button[] children = new Button[2];
      Button button1 = new Button();
      button1.displayName = !saveButtonLabel.isEmpty ? saveButtonLabel : (LocalizedString) "Editor.SAVE";
      button1.disabled = (Func<bool>) (() => string.IsNullOrEmpty(this.m_FileName));
      button1.action = new Action(this.OnConfirm);
      children[0] = button1;
      Button button2 = new Button();
      button2.displayName = (LocalizedString) "Common.CANCEL";
      button2.action = onCancel;
      children[1] = button2;
      widgetArray[3] = (IWidget) ButtonRow.WithChildren(children);
      this.children = (IList<IWidget>) widgetArray;
    }

    private void OnMapSelected(AssetItem item)
    {
      if (item.fileName.Equals(this.m_FileName, StringComparison.OrdinalIgnoreCase))
        return;
      this.m_FileName = item.fileName;
    }

    private void OnNameChange(string value)
    {
      this.m_Adapter.SelectItemByName(value, StringComparison.OrdinalIgnoreCase);
      this.m_FileName = value;
    }

    private void OnConfirm()
    {
      this.m_ConfirmCallback(this.m_FileName, this.m_Adapter.selectedItem?.guid);
    }

    public delegate void SaveCallback(string name, Hash128? overwriteGuid);
  }
}
