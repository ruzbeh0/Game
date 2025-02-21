// Decompiled with JetBrains decompiler
// Type: Game.Input.OptionGroupOverride
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.ComponentModel;

#nullable disable
namespace Game.Input
{
  public enum OptionGroupOverride
  {
    None,
    [Description("Navigation")] Navigation,
    [Description("Menu")] Menu,
    [Description("Camera")] Camera,
    [Description("Tool")] Tool,
    [Description("Shortcuts")] Shortcuts,
    [Description("Photo mode")] PhotoMode,
    [Description("Toolbar")] Toolbar,
    [Description("Tutorial")] Tutorial,
    [Description("Simulation")] Simulation,
    [Description("SIP")] SIP,
  }
}
