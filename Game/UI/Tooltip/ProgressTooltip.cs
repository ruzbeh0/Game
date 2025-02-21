// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.ProgressTooltip
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;

#nullable disable
namespace Game.UI.Tooltip
{
  public class ProgressTooltip : LabelIconTooltip
  {
    public const float kCapacityWarningThreshold = 0.75f;
    private float m_Value;
    private float m_Max;
    private string m_Unit = "integer";
    private bool m_OmitMax;

    public float value
    {
      get => this.m_Value;
      set
      {
        if ((double) value == (double) this.m_Value)
          return;
        this.m_Value = value;
        this.SetPropertiesChanged();
      }
    }

    public float max
    {
      get => this.m_Max;
      set
      {
        if (object.Equals((object) value, (object) this.m_Max))
          return;
        this.m_Max = value;
        this.SetPropertiesChanged();
      }
    }

    public string unit
    {
      get => this.m_Unit;
      set
      {
        if (!(value != this.m_Unit))
          return;
        this.m_Unit = value;
        this.SetPropertiesChanged();
      }
    }

    public bool omitMax
    {
      get => this.m_OmitMax;
      set
      {
        if (value == this.m_OmitMax)
          return;
        this.m_OmitMax = value;
        this.SetPropertiesChanged();
      }
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("value");
      writer.Write(this.value);
      writer.PropertyName("max");
      writer.Write(this.max);
      writer.PropertyName("unit");
      writer.Write(this.unit);
      writer.PropertyName("omitMax");
      writer.Write(this.omitMax);
    }

    public static void SetCapacityColor(ProgressTooltip tooltip)
    {
      if ((double) tooltip.value >= (double) tooltip.max * 0.75)
        tooltip.color = TooltipColor.Info;
      else if ((double) tooltip.value > 0.0)
        tooltip.color = TooltipColor.Warning;
      else
        tooltip.color = TooltipColor.Error;
    }
  }
}
