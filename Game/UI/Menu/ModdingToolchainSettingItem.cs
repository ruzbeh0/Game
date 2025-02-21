// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.ModdingToolchainSettingItem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Modding.Toolchain;
using Game.Reflection;
using Game.Settings;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.UI.Menu
{
  public class ModdingToolchainSettingItem : AutomaticSettings.SettingItemData
  {
    public List<AutomaticSettings.SettingItemData> children { get; } = new List<AutomaticSettings.SettingItemData>();

    public ModdingToolchainSettingItem(
      Setting setting,
      AutomaticSettings.IProxyProperty property,
      string prefix)
      : base(AutomaticSettings.WidgetType.None, setting, property, prefix)
    {
    }

    protected override IWidget GetWidget()
    {
      ModdingToolchainDependency widget = new ModdingToolchainDependency();
      widget.path = (PathSegment) this.path;
      widget.displayName = this.displayName;
      widget.description = this.description;
      widget.displayNameAction = this.dispayNameAction;
      widget.descriptionAction = this.descriptionAction;
      widget.accessor = (ITypedValueAccessor<IToolchainDependency>) new DelegateAccessor<IToolchainDependency>((Func<IToolchainDependency>) (() => (IToolchainDependency) this.property.GetValue((object) this.setting)));
      widget.valueVersion = this.valueVersionAction;
      widget.disabled = this.disableAction;
      widget.hidden = this.hideAction;
      widget.children = (IList<IWidget>) this.children.Select<AutomaticSettings.SettingItemData, IWidget>((Func<AutomaticSettings.SettingItemData, IWidget>) (c => c.widget)).Where<IWidget>((Func<IWidget, bool>) (w => w != null)).ToArray<IWidget>();
      return (IWidget) widget;
    }
  }
}
