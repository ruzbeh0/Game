// Decompiled with JetBrains decompiler
// Type: Game.Settings.SettingsUIGamepadBindingAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Input;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;

#nullable disable
namespace Game.Settings
{
  [AttributeUsage(AttributeTargets.Property)]
  public class SettingsUIGamepadBindingAttribute : SettingsUIKeybindingAttribute
  {
    public readonly BindingGamepad defaultKey;
    public readonly bool leftStick;
    public readonly bool rightStick;

    public override string control
    {
      get
      {
        string control;
        switch (this.defaultKey)
        {
          case BindingGamepad.None:
            control = string.Empty;
            break;
          case BindingGamepad.DpadUp:
            control = "<Gamepad>/dpad/up";
            break;
          case BindingGamepad.DpadDown:
            control = "<Gamepad>/dpad/down";
            break;
          case BindingGamepad.DpadLeft:
            control = "<Gamepad>/dpad/left";
            break;
          case BindingGamepad.DpadRight:
            control = "<Gamepad>/dpad/right";
            break;
          case BindingGamepad.North:
            control = "<Gamepad>/buttonNorth";
            break;
          case BindingGamepad.East:
            control = "<Gamepad>/buttonEast";
            break;
          case BindingGamepad.South:
            control = "<Gamepad>/buttonSouth";
            break;
          case BindingGamepad.West:
            control = "<Gamepad>/buttonWest";
            break;
          case BindingGamepad.LeftShoulder:
            control = "<Gamepad>/leftShoulder";
            break;
          case BindingGamepad.RightShoulder:
            control = "<Gamepad>/rightShoulder";
            break;
          case BindingGamepad.Start:
            control = "<Gamepad>/start";
            break;
          case BindingGamepad.Select:
            control = "<Gamepad>/select";
            break;
          case BindingGamepad.LeftTrigger:
            control = "<Gamepad>/leftTrigger";
            break;
          case BindingGamepad.RightTrigger:
            control = "<Gamepad>/rightTrigger";
            break;
          case BindingGamepad.LeftStickUp:
            control = "<Gamepad>/leftStick/up";
            break;
          case BindingGamepad.LeftStickDown:
            control = "<Gamepad>/leftStick/down";
            break;
          case BindingGamepad.LeftStickLeft:
            control = "<Gamepad>/leftStick/left";
            break;
          case BindingGamepad.LeftStickRight:
            control = "<Gamepad>/leftStick/right";
            break;
          case BindingGamepad.RightStickUp:
            control = "<Gamepad>/rightStick/up";
            break;
          case BindingGamepad.RightStickDown:
            control = "<Gamepad>/rightStick/down";
            break;
          case BindingGamepad.RightStickLeft:
            control = "<Gamepad>/rightStick/left";
            break;
          case BindingGamepad.RightStickRight:
            control = "<Gamepad>/rightStick/right";
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
        if (this.leftStick)
          yield return "<Gamepad>/leftStickPress";
        if (this.rightStick)
          yield return "<Gamepad>/rightStickPress";
      }
    }

    public SettingsUIGamepadBindingAttribute(string actionName = null)
      : base(actionName, InputManager.DeviceType.Gamepad, ActionType.Button, ActionComponent.Press)
    {
    }

    public SettingsUIGamepadBindingAttribute(AxisComponent component, string actionName = null)
      : base(actionName, InputManager.DeviceType.Gamepad, ActionType.Axis, (ActionComponent) component)
    {
    }

    public SettingsUIGamepadBindingAttribute(Vector2Component component, string actionName = null)
      : base(actionName, InputManager.DeviceType.Gamepad, ActionType.Vector2, (ActionComponent) component)
    {
    }

    public SettingsUIGamepadBindingAttribute(
      BindingGamepad defaultKey,
      string actionName = null,
      bool leftStick = false,
      bool rightStick = false)
      : this(actionName)
    {
      this.leftStick = leftStick;
      this.rightStick = rightStick;
      this.defaultKey = defaultKey;
    }

    public SettingsUIGamepadBindingAttribute(
      BindingGamepad defaultKey,
      AxisComponent component,
      string actionName = null,
      bool leftStick = false,
      bool rightStick = false)
      : this(component, actionName)
    {
      this.leftStick = leftStick;
      this.rightStick = rightStick;
      this.defaultKey = defaultKey;
    }

    public SettingsUIGamepadBindingAttribute(
      BindingGamepad defaultKey,
      Vector2Component component,
      string actionName = null,
      bool leftStick = false,
      bool rightStick = false)
      : this(component, actionName)
    {
      this.leftStick = leftStick;
      this.rightStick = rightStick;
      this.defaultKey = defaultKey;
    }

    [Obsolete("Use attribute constructor with BindingGamepad instead of this, it will be removed eventually")]
    public SettingsUIGamepadBindingAttribute(
      GamepadButton defaultKey,
      string actionName = null,
      bool leftStick = false)
      : this((BindingGamepad) (defaultKey + 1), actionName, leftStick)
    {
    }

    [Obsolete("Use attribute constructor with BindingGamepad instead of this, it will be removed eventually")]
    public SettingsUIGamepadBindingAttribute(
      GamepadButton defaultKey,
      AxisComponent component,
      string actionName = null,
      bool leftStick = false)
      : this((BindingGamepad) (defaultKey + 1), component, actionName, leftStick)
    {
    }

    [Obsolete("Use attribute constructor with BindingGamepad instead of this, it will be removed eventually")]
    public SettingsUIGamepadBindingAttribute(
      GamepadButton defaultKey,
      Vector2Component component,
      string actionName = null,
      bool leftStick = false)
      : this((BindingGamepad) (defaultKey + 1), component, actionName, leftStick)
    {
    }
  }
}
