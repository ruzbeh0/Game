// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.StringInputFieldWithError
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;

#nullable disable
namespace Game.UI.Editor
{
  public class StringInputFieldWithError : StringInputField
  {
    private bool m_Error;

    public Func<bool> error { get; set; }

    public LocalizedString errorMessage { get; set; }

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      if (this.error != null)
      {
        bool flag = this.error();
        if (this.m_Error != flag)
          widgetChanges |= WidgetChanges.Properties;
        this.m_Error = flag;
      }
      return widgetChanges;
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("error");
      writer.Write(this.m_Error);
      writer.PropertyName("errorMessage");
      writer.Write<LocalizedString>(this.errorMessage);
    }
  }
}
