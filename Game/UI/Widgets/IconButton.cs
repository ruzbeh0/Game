// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.IconButton
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
  public class IconButton : Widget, ITooltipTarget, IInvokable, IWidget, IJsonWritable
  {
    private bool m_Selected;
    private string m_Icon;

    public string icon
    {
      get => this.m_Icon;
      set
      {
        if (!(this.m_Icon != value))
          return;
        this.m_Icon = value;
        this.SetPropertiesChanged();
      }
    }

    public Action action { get; set; }

    [CanBeNull]
    public Func<bool> selected { get; set; }

    [CanBeNull]
    public LocalizedString? tooltip { get; set; }

    public void Invoke() => this.action();

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      bool flag = this.selected != null && this.selected();
      if (flag != this.m_Selected)
      {
        widgetChanges |= WidgetChanges.Properties;
        this.m_Selected = flag;
      }
      return widgetChanges;
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("icon");
      writer.Write(this.icon);
      writer.PropertyName("selected");
      writer.Write(this.m_Selected);
      writer.PropertyName("tooltip");
      writer.Write<LocalizedString>(this.tooltip);
    }
  }
}
