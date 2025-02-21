// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.NamedWidgetWithTooltip
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using Game.UI.Localization;

#nullable disable
namespace Game.UI.Widgets
{
  public abstract class NamedWidgetWithTooltip : NamedWidget, ITooltipTarget, IUITagProvider
  {
    [CanBeNull]
    public LocalizedString? tooltip { get; set; }

    [CanBeNull]
    public string uiTag { get; set; }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("tooltip");
      writer.Write<LocalizedString>(this.tooltip);
      writer.PropertyName("uiTag");
      writer.Write(this.uiTag);
    }
  }
}
