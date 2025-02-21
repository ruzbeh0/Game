// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.NotificationTooltip
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Notifications;
using Game.UI.Widgets;

#nullable disable
namespace Game.UI.Tooltip
{
  public class NotificationTooltip : Widget
  {
    private string m_Name;
    private TooltipColor m_Color;
    private bool m_Verbose;

    public string name
    {
      get => this.m_Name;
      set
      {
        if (!(value != this.m_Name))
          return;
        this.m_Name = value;
        this.SetPropertiesChanged();
      }
    }

    public TooltipColor color
    {
      get => this.m_Color;
      set
      {
        if (value == this.m_Color)
          return;
        this.m_Color = value;
        this.SetPropertiesChanged();
      }
    }

    public bool verbose
    {
      get => this.m_Verbose;
      set
      {
        if (value == this.m_Verbose)
          return;
        this.m_Verbose = value;
        this.SetPropertiesChanged();
      }
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("name");
      writer.Write(this.name);
      writer.PropertyName("color");
      writer.Write((int) this.color);
      writer.PropertyName("verbose");
      writer.Write(this.verbose);
    }

    public static TooltipColor GetColor(IconPriority iconPriority)
    {
      if (iconPriority >= IconPriority.Error)
        return TooltipColor.Error;
      return iconPriority >= IconPriority.Problem ? TooltipColor.Warning : TooltipColor.Info;
    }
  }
}
