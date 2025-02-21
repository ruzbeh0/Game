// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.MultilineTextSettingItemData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Settings;
using Game.UI.Menu;

#nullable disable
namespace Game.UI.Widgets
{
  public class MultilineTextSettingItemData : AutomaticSettings.SettingItemData
  {
    public string icon { get; set; }

    public MultilineTextSettingItemData(
      Setting setting,
      AutomaticSettings.IProxyProperty property,
      string prefix)
      : base(AutomaticSettings.WidgetType.MultilineText, setting, property, prefix)
    {
      SettingsUIMultilineTextAttribute attribute = property.GetAttribute<SettingsUIMultilineTextAttribute>();
      this.icon = attribute != null ? attribute.icon : string.Empty;
    }

    protected override IWidget GetWidget()
    {
      MultilineText widget = new MultilineText();
      widget.path = (PathSegment) this.path;
      widget.displayName = this.displayName;
      widget.displayNameAction = this.dispayNameAction;
      widget.icon = this.icon;
      widget.hidden = this.hideAction;
      widget.disabled = this.disableAction;
      return (IWidget) widget;
    }
  }
}
