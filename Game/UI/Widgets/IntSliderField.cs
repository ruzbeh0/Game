// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.IntSliderField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using System;

#nullable disable
namespace Game.UI.Widgets
{
  public class IntSliderField : IntField<int>, IWarning
  {
    private bool m_Warning;

    [CanBeNull]
    public Func<bool> warningAction { get; set; }

    [CanBeNull]
    public string unit { get; set; }

    public bool signed { get; set; }

    public bool separateThousands { get; set; }

    public bool scaleDragVolume { get; set; }

    public bool updateOnDragEnd { get; set; }

    public bool warning
    {
      get => this.m_Warning;
      set
      {
        this.warningAction = (Func<bool>) null;
        this.m_Warning = value;
      }
    }

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      if (this.warningAction != null)
      {
        bool flag = this.warningAction();
        if (flag != this.m_Warning)
        {
          this.m_Warning = flag;
          widgetChanges |= WidgetChanges.Properties;
        }
      }
      return widgetChanges;
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("unit");
      writer.Write(this.unit);
      writer.PropertyName("signed");
      writer.Write(this.signed);
      writer.PropertyName("separateThousands");
      writer.Write(this.separateThousands);
      writer.PropertyName("scaleDragVolume");
      writer.Write(this.scaleDragVolume);
      writer.PropertyName("updateOnDragEnd");
      writer.Write(this.updateOnDragEnd);
      writer.PropertyName("warning");
      writer.Write(this.warning);
    }
  }
}
