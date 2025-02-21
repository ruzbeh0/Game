// Decompiled with JetBrains decompiler
// Type: Game.Input.ControlPath
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

#nullable disable
namespace Game.Input
{
  [DebuggerDisplay("{name} ({displayName})")]
  public struct ControlPath : IJsonWritable
  {
    private static Dictionary<string, bool> m_IsLatinLayout = new Dictionary<string, bool>();
    public string name;
    public InputManager.DeviceType device;
    public string displayName;

    public static ControlPath Get(string path)
    {
      if (string.IsNullOrEmpty(path))
        return new ControlPath()
        {
          name = string.Empty,
          device = InputManager.DeviceType.None,
          displayName = string.Empty
        };
      InputControlPath.ParsedPathComponent[] array = InputControlPath.Parse(path).ToArray<InputControlPath.ParsedPathComponent>();
      string str = string.Join("/", ((IEnumerable<InputControlPath.ParsedPathComponent>) array).Where<InputControlPath.ParsedPathComponent>((Func<InputControlPath.ParsedPathComponent, bool>) (p => string.IsNullOrEmpty(p.layout))).Select<InputControlPath.ParsedPathComponent, string>((Func<InputControlPath.ParsedPathComponent, string>) (p => p.name)));
      string layout = ((IEnumerable<InputControlPath.ParsedPathComponent>) array).FirstOrDefault<InputControlPath.ParsedPathComponent>((Func<InputControlPath.ParsedPathComponent, bool>) (p => !string.IsNullOrEmpty(p.layout))).layout;
      return new ControlPath()
      {
        name = str,
        device = layout.ToDeviceType(),
        displayName = str.Length != 1 || !char.IsLetter(str[0]) ? str : str.ToUpper()
      };
    }

    public static bool IsLatinLikeLayout(Keyboard keyboard)
    {
      bool flag;
      if (!ControlPath.m_IsLatinLayout.TryGetValue(keyboard.keyboardLayout, out flag))
      {
        flag = Enumerable.Range(15, 26).All<int>((Func<int, bool>) (k => ControlPath.IsLatinOrPunctuation(keyboard[(Key) k].displayName)));
        ControlPath.m_IsLatinLayout[keyboard.keyboardLayout] = flag;
      }
      return flag;
    }

    public static bool NeedLocalName(Keyboard keyboard, KeyControl control)
    {
      switch (control.keyCode)
      {
        case Key.Space:
        case Key.Enter:
        case Key.Tab:
        case Key.Digit1:
        case Key.Digit2:
        case Key.Digit3:
        case Key.Digit4:
        case Key.Digit5:
        case Key.Digit6:
        case Key.Digit7:
        case Key.Digit8:
        case Key.Digit9:
        case Key.Digit0:
        case Key.LeftShift:
        case Key.RightShift:
        case Key.LeftAlt:
        case Key.RightAlt:
        case Key.LeftCtrl:
        case Key.RightCtrl:
        case Key.LeftMeta:
        case Key.RightMeta:
        case Key.Escape:
        case Key.LeftArrow:
        case Key.RightArrow:
        case Key.UpArrow:
        case Key.DownArrow:
        case Key.Backspace:
        case Key.PageDown:
        case Key.PageUp:
        case Key.Home:
        case Key.End:
        case Key.Delete:
        case Key.Numpad0:
        case Key.Numpad1:
        case Key.Numpad2:
        case Key.Numpad3:
        case Key.Numpad4:
        case Key.Numpad5:
        case Key.Numpad6:
        case Key.Numpad7:
        case Key.Numpad8:
        case Key.Numpad9:
          return false;
        case Key.OEM1:
        case Key.OEM2:
        case Key.OEM3:
        case Key.OEM4:
        case Key.OEM5:
          return true;
        default:
          return ControlPath.IsLatinLikeLayout(keyboard);
      }
    }

    private static bool IsLatinOrPunctuation(string displayName)
    {
      if (string.IsNullOrEmpty(displayName))
        return false;
      return ControlPath.IsLatinLater(displayName) || char.IsPunctuation(displayName[0]);
    }

    private static bool IsLatinLater(string displayName)
    {
      return !string.IsNullOrEmpty(displayName) && char.IsLetterOrDigit(displayName[0]) && displayName[0] <= 'ÿ';
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(typeof (ControlPath).FullName);
      writer.PropertyName("name");
      writer.Write(this.name);
      writer.PropertyName("device");
      writer.Write(this.device.ToString());
      writer.PropertyName("displayName");
      writer.Write(this.displayName);
      writer.TypeEnd();
    }

    public static string ToHumanReadablePath(
      string path,
      InputControlPath.HumanReadableStringOptions options = InputControlPath.HumanReadableStringOptions.OmitDevice)
    {
      return InputControlPath.ToHumanReadableString(path, options);
    }
  }
}
