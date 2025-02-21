// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.Button
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;

#nullable disable
namespace Game.UI.Widgets
{
  public class Button : NamedWidgetWithTooltip, IInvokable, IWidget, IJsonWritable, IDisableCallback
  {
    public Action action { get; set; }

    public void Invoke() => this.action();
  }
}
