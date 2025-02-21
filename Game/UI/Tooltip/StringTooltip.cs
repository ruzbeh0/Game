// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.StringTooltip
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.UI.Localization;

#nullable disable
namespace Game.UI.Tooltip
{
  public class StringTooltip : IconTooltip
  {
    private LocalizedString m_Value;

    public LocalizedString value
    {
      get => this.m_Value;
      set
      {
        if (object.Equals((object) value, (object) this.m_Value))
          return;
        this.m_Value = value;
        this.SetPropertiesChanged();
      }
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("value");
      writer.Write<LocalizedString>(this.value);
    }
  }
}
