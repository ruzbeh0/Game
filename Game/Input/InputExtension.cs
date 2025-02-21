// Decompiled with JetBrains decompiler
// Type: Game.Input.InputExtension
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.Input
{
  public static class InputExtension
  {
    public static InputManager.DeviceType ToDeviceType(this InputBinding? mask)
    {
      if (!mask.HasValue)
        return InputManager.DeviceType.All;
      InputManager.DeviceType deviceType = InputManager.DeviceType.None;
      if (mask.Value.groups.Contains("Keyboard"))
        deviceType |= InputManager.DeviceType.Keyboard;
      if (mask.Value.groups.Contains("Mouse"))
        deviceType |= InputManager.DeviceType.Mouse;
      if (mask.Value.groups.Contains("Gamepad"))
        deviceType |= InputManager.DeviceType.Gamepad;
      return deviceType;
    }

    public static InputManager.DeviceType ToDeviceType(this string group)
    {
      switch (group)
      {
        case "Keyboard":
          return InputManager.DeviceType.Keyboard;
        case "Mouse":
          return InputManager.DeviceType.Mouse;
        case "Gamepad":
          return InputManager.DeviceType.Gamepad;
        case "DualShockGamepad":
          return InputManager.DeviceType.Gamepad;
        case "XInputController":
          return InputManager.DeviceType.Gamepad;
        default:
          throw new ArgumentException("Unsupported device type \"" + group + "\"");
      }
    }

    public static InputManager.DeviceType ToDeviceType(this InputManager.ControlScheme scheme)
    {
      InputManager.DeviceType deviceType;
      switch (scheme)
      {
        case InputManager.ControlScheme.KeyboardAndMouse:
          deviceType = InputManager.DeviceType.Keyboard | InputManager.DeviceType.Mouse;
          break;
        case InputManager.ControlScheme.Gamepad:
          deviceType = InputManager.DeviceType.Gamepad;
          break;
        default:
          deviceType = InputManager.DeviceType.None;
          break;
      }
      return deviceType;
    }

    public static InputBinding? ToInputBinding(this InputManager.DeviceType type)
    {
      if (type == InputManager.DeviceType.All)
        return new InputBinding?();
      if (type == InputManager.DeviceType.None)
        return new InputBinding?(InputBinding.MaskByGroup(string.Empty));
      return new InputBinding?(InputBinding.MaskByGroups((type & InputManager.DeviceType.Keyboard) != InputManager.DeviceType.None ? "Keyboard" : string.Empty, (type & InputManager.DeviceType.Mouse) != InputManager.DeviceType.None ? "Mouse" : string.Empty, (type & InputManager.DeviceType.Gamepad) != InputManager.DeviceType.None ? "Gamepad" : string.Empty));
    }

    public static UIBaseInputAction.Transform ToTransform(this ActionComponent component)
    {
      UIBaseInputAction.Transform transform;
      switch (component)
      {
        case ActionComponent.Press:
          transform = UIBaseInputAction.Transform.Press;
          break;
        case ActionComponent.Negative:
          transform = UIBaseInputAction.Transform.Negative;
          break;
        case ActionComponent.Positive:
          transform = UIBaseInputAction.Transform.Positive;
          break;
        case ActionComponent.Down:
          transform = UIBaseInputAction.Transform.Down;
          break;
        case ActionComponent.Up:
          transform = UIBaseInputAction.Transform.Up;
          break;
        case ActionComponent.Left:
          transform = UIBaseInputAction.Transform.Left;
          break;
        case ActionComponent.Right:
          transform = UIBaseInputAction.Transform.Right;
          break;
        default:
          transform = UIBaseInputAction.Transform.Press;
          break;
      }
      return transform;
    }

    public static string GetPath(this InputBinding binding, InputManager.PathType pathType)
    {
      string path;
      switch (pathType)
      {
        case InputManager.PathType.Effective:
          path = binding.effectivePath;
          break;
        case InputManager.PathType.Original:
          path = binding.path;
          break;
        case InputManager.PathType.Overridden:
          path = binding.overridePath;
          break;
        default:
          path = binding.effectivePath;
          break;
      }
      return path;
    }

    public static string GetProcessors(this InputBinding binding, InputManager.PathType pathType)
    {
      string processors;
      switch (pathType)
      {
        case InputManager.PathType.Effective:
          processors = binding.effectiveProcessors;
          break;
        case InputManager.PathType.Original:
          processors = binding.processors;
          break;
        case InputManager.PathType.Overridden:
          processors = binding.overrideProcessors;
          break;
        default:
          processors = binding.effectivePath;
          break;
      }
      return processors;
    }

    public static string GetInteractions(this InputBinding binding, InputManager.PathType pathType)
    {
      string interactions;
      switch (pathType)
      {
        case InputManager.PathType.Effective:
          interactions = binding.effectiveInteractions;
          break;
        case InputManager.PathType.Original:
          interactions = binding.interactions;
          break;
        case InputManager.PathType.Overridden:
          interactions = binding.overrideInteractions;
          break;
        default:
          interactions = binding.effectivePath;
          break;
      }
      return interactions;
    }
  }
}
