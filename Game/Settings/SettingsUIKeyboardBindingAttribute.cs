// Decompiled with JetBrains decompiler
// Type: Game.Settings.SettingsUIKeyboardBindingAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Input;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.Settings
{
  [AttributeUsage(AttributeTargets.Property)]
  public class SettingsUIKeyboardBindingAttribute : SettingsUIKeybindingAttribute
  {
    public readonly BindingKeyboard defaultKey;
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
          case BindingKeyboard.None:
            control = string.Empty;
            break;
          case BindingKeyboard.Space:
            control = "<Keyboard>/space";
            break;
          case BindingKeyboard.Enter:
            control = "<Keyboard>/enter";
            break;
          case BindingKeyboard.Tab:
            control = "<Keyboard>/tab";
            break;
          case BindingKeyboard.Backquote:
            control = "<Keyboard>/backquote";
            break;
          case BindingKeyboard.Quote:
            control = "<Keyboard>/quote";
            break;
          case BindingKeyboard.Semicolon:
            control = "<Keyboard>/semicolon";
            break;
          case BindingKeyboard.Comma:
            control = "<Keyboard>/comma";
            break;
          case BindingKeyboard.Period:
            control = "<Keyboard>/period";
            break;
          case BindingKeyboard.Slash:
            control = "<Keyboard>/slash";
            break;
          case BindingKeyboard.Backslash:
            control = "<Keyboard>/backslash";
            break;
          case BindingKeyboard.LeftBracket:
            control = "<Keyboard>/leftBracket";
            break;
          case BindingKeyboard.RightBracket:
            control = "<Keyboard>/rightBracket";
            break;
          case BindingKeyboard.Minus:
            control = "<Keyboard>/minus";
            break;
          case BindingKeyboard.Equals:
            control = "<Keyboard>/equals";
            break;
          case BindingKeyboard.A:
            control = "<Keyboard>/a";
            break;
          case BindingKeyboard.B:
            control = "<Keyboard>/b";
            break;
          case BindingKeyboard.C:
            control = "<Keyboard>/c";
            break;
          case BindingKeyboard.D:
            control = "<Keyboard>/d";
            break;
          case BindingKeyboard.E:
            control = "<Keyboard>/e";
            break;
          case BindingKeyboard.F:
            control = "<Keyboard>/f";
            break;
          case BindingKeyboard.G:
            control = "<Keyboard>/g";
            break;
          case BindingKeyboard.H:
            control = "<Keyboard>/h";
            break;
          case BindingKeyboard.I:
            control = "<Keyboard>/i";
            break;
          case BindingKeyboard.J:
            control = "<Keyboard>/j";
            break;
          case BindingKeyboard.K:
            control = "<Keyboard>/k";
            break;
          case BindingKeyboard.L:
            control = "<Keyboard>/l";
            break;
          case BindingKeyboard.M:
            control = "<Keyboard>/m";
            break;
          case BindingKeyboard.N:
            control = "<Keyboard>/n";
            break;
          case BindingKeyboard.O:
            control = "<Keyboard>/o";
            break;
          case BindingKeyboard.P:
            control = "<Keyboard>/p";
            break;
          case BindingKeyboard.Q:
            control = "<Keyboard>/q";
            break;
          case BindingKeyboard.R:
            control = "<Keyboard>/r";
            break;
          case BindingKeyboard.S:
            control = "<Keyboard>/s";
            break;
          case BindingKeyboard.T:
            control = "<Keyboard>/t";
            break;
          case BindingKeyboard.U:
            control = "<Keyboard>/u";
            break;
          case BindingKeyboard.V:
            control = "<Keyboard>/v";
            break;
          case BindingKeyboard.W:
            control = "<Keyboard>/w";
            break;
          case BindingKeyboard.X:
            control = "<Keyboard>/x";
            break;
          case BindingKeyboard.Y:
            control = "<Keyboard>/y";
            break;
          case BindingKeyboard.Z:
            control = "<Keyboard>/z";
            break;
          case BindingKeyboard.Digit1:
            control = "<Keyboard>/1";
            break;
          case BindingKeyboard.Digit2:
            control = "<Keyboard>/2";
            break;
          case BindingKeyboard.Digit3:
            control = "<Keyboard>/3";
            break;
          case BindingKeyboard.Digit4:
            control = "<Keyboard>/4";
            break;
          case BindingKeyboard.Digit5:
            control = "<Keyboard>/5";
            break;
          case BindingKeyboard.Digit6:
            control = "<Keyboard>/6";
            break;
          case BindingKeyboard.Digit7:
            control = "<Keyboard>/7";
            break;
          case BindingKeyboard.Digit8:
            control = "<Keyboard>/8";
            break;
          case BindingKeyboard.Digit9:
            control = "<Keyboard>/9";
            break;
          case BindingKeyboard.Digit0:
            control = "<Keyboard>/0";
            break;
          case BindingKeyboard.Escape:
            control = "<Keyboard>/escape";
            break;
          case BindingKeyboard.LeftArrow:
            control = "<Keyboard>/leftArrow";
            break;
          case BindingKeyboard.RightArrow:
            control = "<Keyboard>/rightArrow";
            break;
          case BindingKeyboard.UpArrow:
            control = "<Keyboard>/upArrow";
            break;
          case BindingKeyboard.DownArrow:
            control = "<Keyboard>/downArrow";
            break;
          case BindingKeyboard.Backspace:
            control = "<Keyboard>/backspace";
            break;
          case BindingKeyboard.PageDown:
            control = "<Keyboard>/pageDown";
            break;
          case BindingKeyboard.PageUp:
            control = "<Keyboard>/pageUp";
            break;
          case BindingKeyboard.Home:
            control = "<Keyboard>/home";
            break;
          case BindingKeyboard.End:
            control = "<Keyboard>/end";
            break;
          case BindingKeyboard.Delete:
            control = "<Keyboard>/delete";
            break;
          case BindingKeyboard.NumpadEnter:
            control = "<Keyboard>/numpadEnter";
            break;
          case BindingKeyboard.NumpadDivide:
            control = "<Keyboard>/numpadDivide";
            break;
          case BindingKeyboard.NumpadMultiply:
            control = "<Keyboard>/numpadMultiply";
            break;
          case BindingKeyboard.NumpadPlus:
            control = "<Keyboard>/numpadPlus";
            break;
          case BindingKeyboard.NumpadMinus:
            control = "<Keyboard>/numpadMinus";
            break;
          case BindingKeyboard.NumpadPeriod:
            control = "<Keyboard>/numpadPeriod";
            break;
          case BindingKeyboard.NumpadEquals:
            control = "<Keyboard>/numpadEquals";
            break;
          case BindingKeyboard.Numpad0:
            control = "<Keyboard>/numpad0";
            break;
          case BindingKeyboard.Numpad1:
            control = "<Keyboard>/numpad1";
            break;
          case BindingKeyboard.Numpad2:
            control = "<Keyboard>/numpad2";
            break;
          case BindingKeyboard.Numpad3:
            control = "<Keyboard>/numpad3";
            break;
          case BindingKeyboard.Numpad4:
            control = "<Keyboard>/numpad4";
            break;
          case BindingKeyboard.Numpad5:
            control = "<Keyboard>/numpad5";
            break;
          case BindingKeyboard.Numpad6:
            control = "<Keyboard>/numpad6";
            break;
          case BindingKeyboard.Numpad7:
            control = "<Keyboard>/numpad7";
            break;
          case BindingKeyboard.Numpad8:
            control = "<Keyboard>/numpad8";
            break;
          case BindingKeyboard.Numpad9:
            control = "<Keyboard>/numpad9";
            break;
          case BindingKeyboard.F1:
            control = "<Keyboard>/f1";
            break;
          case BindingKeyboard.F2:
            control = "<Keyboard>/f2";
            break;
          case BindingKeyboard.F3:
            control = "<Keyboard>/f3";
            break;
          case BindingKeyboard.F4:
            control = "<Keyboard>/f4";
            break;
          case BindingKeyboard.F5:
            control = "<Keyboard>/f5";
            break;
          case BindingKeyboard.F6:
            control = "<Keyboard>/f6";
            break;
          case BindingKeyboard.F7:
            control = "<Keyboard>/f7";
            break;
          case BindingKeyboard.F8:
            control = "<Keyboard>/f8";
            break;
          case BindingKeyboard.F9:
            control = "<Keyboard>/f9";
            break;
          case BindingKeyboard.F10:
            control = "<Keyboard>/f10";
            break;
          case BindingKeyboard.F11:
            control = "<Keyboard>/f11";
            break;
          case BindingKeyboard.F12:
            control = "<Keyboard>/f12";
            break;
          case BindingKeyboard.OEM1:
            control = "<Keyboard>/OEM1";
            break;
          case BindingKeyboard.OEM2:
            control = "<Keyboard>/OEM2";
            break;
          case BindingKeyboard.OEM3:
            control = "<Keyboard>/OEM3";
            break;
          case BindingKeyboard.OEM4:
            control = "<Keyboard>/OEM4";
            break;
          case BindingKeyboard.OEM5:
            control = "<Keyboard>/OEM5";
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

    public SettingsUIKeyboardBindingAttribute(string actionName = null)
      : base(actionName, Game.Input.InputManager.DeviceType.Keyboard, ActionType.Button, ActionComponent.Press)
    {
    }

    public SettingsUIKeyboardBindingAttribute(AxisComponent component, string actionName = null)
      : base(actionName, Game.Input.InputManager.DeviceType.Keyboard, ActionType.Axis, (ActionComponent) component)
    {
    }

    public SettingsUIKeyboardBindingAttribute(Vector2Component component, string actionName = null)
      : base(actionName, Game.Input.InputManager.DeviceType.Keyboard, ActionType.Vector2, (ActionComponent) component)
    {
    }

    public SettingsUIKeyboardBindingAttribute(
      BindingKeyboard defaultKey,
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

    public SettingsUIKeyboardBindingAttribute(
      BindingKeyboard defaultKey,
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

    public SettingsUIKeyboardBindingAttribute(
      BindingKeyboard defaultKey,
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

    [Obsolete("Use attribute constructor with BindingKeyboard instead of this, it will be removed eventually")]
    public SettingsUIKeyboardBindingAttribute(
      Key defaultKey,
      string actionName = null,
      bool alt = false,
      bool ctrl = false,
      bool shift = false)
      : this((BindingKeyboard) defaultKey, actionName, alt, ctrl, shift)
    {
    }

    [Obsolete("Use attribute constructor with BindingKeyboard instead of this, it will be removed eventually")]
    public SettingsUIKeyboardBindingAttribute(
      Key defaultKey,
      AxisComponent component,
      string actionName = null,
      bool alt = false,
      bool ctrl = false,
      bool shift = false)
      : this((BindingKeyboard) defaultKey, component, actionName, alt, ctrl, shift)
    {
    }

    [Obsolete("Use attribute constructor with BindingKeyboard instead of this, it will be removed eventually")]
    public SettingsUIKeyboardBindingAttribute(
      Key defaultKey,
      Vector2Component component,
      string actionName = null,
      bool alt = false,
      bool ctrl = false,
      bool shift = false)
      : this((BindingKeyboard) defaultKey, component, actionName, alt, ctrl, shift)
    {
    }
  }
}
