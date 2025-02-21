// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.LocalizedValueField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using Game.UI.Localization;
using System;

#nullable disable
namespace Game.UI.Widgets
{
  public class LocalizedValueField : ReadonlyField<LocalizedString>, IWarning
  {
    private bool m_Warning;

    [CanBeNull]
    public Func<bool> warningAction { get; set; }

    public bool warning
    {
      get => this.m_Warning;
      set
      {
        this.warningAction = (Func<bool>) null;
        this.m_Warning = value;
      }
    }

    public LocalizedValueField()
    {
      this.valueWriter = (IWriter<LocalizedString>) new ValueWriter<LocalizedString>();
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
      writer.PropertyName("warning");
      writer.Write(this.warning);
    }
  }
}
