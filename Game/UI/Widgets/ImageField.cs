// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.ImageField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.UI.Localization;

#nullable disable
namespace Game.UI.Widgets
{
  public class ImageField : Widget, ITooltipTarget
  {
    public string m_URI;
    public LocalizedString m_Label;

    public LocalizedString? tooltip { get; set; }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("uri");
      writer.Write(this.m_URI);
      writer.PropertyName("label");
      writer.Write<LocalizedString>(this.m_Label);
      writer.PropertyName("tooltip");
      writer.Write<LocalizedString>(this.tooltip);
    }
  }
}
