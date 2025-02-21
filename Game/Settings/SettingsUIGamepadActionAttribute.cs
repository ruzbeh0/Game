// Decompiled with JetBrains decompiler
// Type: Game.Settings.SettingsUIGamepadActionAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Input;
using System;

#nullable disable
namespace Game.Settings
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public class SettingsUIGamepadActionAttribute : SettingsUIInputActionAttribute
  {
    public SettingsUIGamepadActionAttribute(
      string name,
      ActionType type = ActionType.Button,
      bool allowModifiers = true,
      bool developerOnly = false,
      Game.Input.Mode mode = Game.Input.Mode.Analog,
      string[] usages = null,
      string[] interactions = null,
      string[] processors = null)
      : base(name, InputManager.DeviceType.Gamepad, type, allowModifiers, developerOnly, mode, usages, interactions, processors)
    {
    }

    public SettingsUIGamepadActionAttribute(
      string name,
      ActionType type,
      Game.Input.Mode mode,
      params string[] customUsages)
      : base(name, InputManager.DeviceType.Gamepad, type, mode, customUsages)
    {
    }

    public SettingsUIGamepadActionAttribute(
      string name,
      ActionType type,
      params string[] customUsages)
      : base(name, InputManager.DeviceType.Gamepad, type, Game.Input.Mode.Analog, customUsages)
    {
    }

    public SettingsUIGamepadActionAttribute(string name, Game.Input.Mode mode, params string[] customUsages)
      : base(name, InputManager.DeviceType.Gamepad, ActionType.Button, mode, customUsages)
    {
    }

    public SettingsUIGamepadActionAttribute(string name, params string[] customUsages)
      : base(name, InputManager.DeviceType.Gamepad, ActionType.Button, Game.Input.Mode.Analog, customUsages)
    {
    }
  }
}
