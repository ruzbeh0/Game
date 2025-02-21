// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.StringInputField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using System;

#nullable disable
namespace Game.UI.Widgets
{
  public class StringInputField : Field<string>, IWarning
  {
    public static readonly int kDefaultMultilines = 5;
    public static readonly int kSingleLine = 0;
    private bool m_Warning;
    private int m_Multiline = StringInputField.kSingleLine;
    private int m_MaxLength;

    [CanBeNull]
    public Func<bool> warningAction { get; set; }

    public int multiline
    {
      get => this.m_Multiline;
      set
      {
        if (value == this.m_Multiline)
          return;
        this.m_Multiline = value;
        this.SetPropertiesChanged();
      }
    }

    public int maxLength
    {
      get => this.m_MaxLength;
      set
      {
        if (value == this.m_MaxLength)
          return;
        this.m_MaxLength = value;
        this.SetPropertiesChanged();
      }
    }

    public bool warning
    {
      get => this.m_Warning;
      set
      {
        this.warningAction = (Func<bool>) null;
        this.m_Warning = value;
      }
    }

    public override string GetValue() => base.GetValue() ?? string.Empty;

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
      writer.PropertyName("multiline");
      writer.Write(this.m_Multiline);
      writer.PropertyName("maxLength");
      writer.Write(this.m_MaxLength);
      writer.PropertyName("warning");
      writer.Write(this.warning);
    }
  }
}
