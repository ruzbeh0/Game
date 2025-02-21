// Decompiled with JetBrains decompiler
// Type: Game.Settings.SettingsUIMouseBindingAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Input;
using System.Collections.Generic;

#nullable disable
namespace Game.Settings
{
  public class SettingsUIMouseBindingAttribute : SettingsUIKeybindingAttribute
  {
    public readonly BindingMouse defaultKey;
    public readonly bool alt;
    public readonly bool ctrl;
    public readonly bool shift;

    public override string control
    {
      get
      {
        string control;
        switch (this.defaultKey)
        {
          case BindingMouse.None:
            control = string.Empty;
            break;
          case BindingMouse.Left:
            control = "<Mouse>/leftButton";
            break;
          case BindingMouse.Right:
            control = "<Mouse>/rightButton";
            break;
          case BindingMouse.Middle:
            control = "<Mouse>/middleButton";
            break;
          case BindingMouse.Forward:
            control = "<Mouse>/forwardButton";
            break;
          case BindingMouse.Backward:
            control = "<Mouse>/backButton";
            break;
          default:
            control = string.Empty;
            break;
        }
        return control;
      }
    }

    public override IEnumerable<string> modifierControls
    {
      get
      {
        if (this.shift)
          yield return "<Keyboard>/shift";
        if (this.ctrl)
          yield return "<Keyboard>/ctrl";
        if (this.alt)
          yield return "<Keyboard>/alt";
      }
    }

    public SettingsUIMouseBindingAttribute(string actionName = null)
      : base(actionName, InputManager.DeviceType.Mouse, ActionType.Button, ActionComponent.Press)
    {
    }

    public SettingsUIMouseBindingAttribute(AxisComponent component, string actionName = null)
      : base(actionName, InputManager.DeviceType.Mouse, ActionType.Axis, (ActionComponent) component)
    {
    }

    public SettingsUIMouseBindingAttribute(Vector2Component component, string actionName = null)
      : base(actionName, InputManager.DeviceType.Mouse, ActionType.Vector2, (ActionComponent) component)
    {
    }

    public SettingsUIMouseBindingAttribute(
      BindingMouse defaultKey,
      string actionName = null,
      bool alt = false,
      bool ctrl = false,
      bool shift = false)
      : this(actionName)
    {
      this.alt = alt;
      this.ctrl = ctrl;
      this.shift = shift;
      this.defaultKey = defaultKey;
    }

    public SettingsUIMouseBindingAttribute(
      BindingMouse defaultKey,
      AxisComponent component,
      string actionName = null,
      bool alt = false,
      bool ctrl = false,
      bool shift = false)
      : this(component, actionName)
    {
      this.alt = alt;
      this.ctrl = ctrl;
      this.shift = shift;
      this.defaultKey = defaultKey;
    }

    public SettingsUIMouseBindingAttribute(
      BindingMouse defaultKey,
      Vector2Component component,
      string actionName = null,
      bool alt = false,
      bool ctrl = false,
      bool shift = false)
      : this(component, actionName)
    {
      this.alt = alt;
      this.ctrl = ctrl;
      this.shift = shift;
      this.defaultKey = defaultKey;
    }
  }
}
