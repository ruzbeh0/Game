// Decompiled with JetBrains decompiler
// Type: Game.Input.UIBaseInputAction
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Game.Input
{
  public abstract class UIBaseInputAction : ScriptableObject
  {
    public string m_AliasName;
    public UIBaseInputAction.Priority m_DisplayPriority = UIBaseInputAction.Priority.Disabled;
    public InputManager.DeviceType m_DisplayMask = InputManager.DeviceType.Gamepad;
    public bool m_ShowInOptions;
    public OptionGroupOverride m_OptionGroupOverride;

    public string aliasName => this.m_AliasName;

    public int displayPriority => (int) this.m_DisplayPriority;

    public bool showInOptions => this.m_ShowInOptions;

    public OptionGroupOverride optionGroupOverride => this.m_OptionGroupOverride;

    public abstract IReadOnlyList<UIInputActionPart> actionParts { get; }

    public DisplayNameOverride GetDisplayName(UIInputActionPart actionPart, string source)
    {
      return (actionPart.m_Mask & this.m_DisplayMask) != InputManager.DeviceType.None ? new DisplayNameOverride(source, actionPart.GetProxyAction(), this.m_AliasName, (int) this.m_DisplayPriority, actionPart.m_Transform) : (DisplayNameOverride) null;
    }

    public abstract IProxyAction GetState(string source);

    public abstract IProxyAction GetState(
      string source,
      UIBaseInputAction.DisplayGetter displayNameGetter);

    public delegate DisplayNameOverride DisplayGetter(
      string name,
      ProxyAction action,
      InputManager.DeviceType mask,
      UIBaseInputAction.Transform transform);

    public interface IState
    {
      IReadOnlyList<ProxyAction> actions { get; }
    }

    public enum Priority
    {
      Disabled = -1, // 0xFFFFFFFF
      Custom = 0,
      A = 20, // 0x00000014
      X = 30, // 0x0000001E
      Y = 40, // 0x00000028
      B = 50, // 0x00000032
      DPad = 55, // 0x00000037
      Bumper = 60, // 0x0000003C
      Trigger = 70, // 0x00000046
    }

    public enum ProcessAs
    {
      AutoDetect,
      Button,
      Axis,
      Vector2,
    }

    [Flags]
    public enum Transform
    {
      None = 0,
      Down = 1,
      Up = 2,
      Left = 4,
      Right = 8,
      Negative = Left | Down, // 0x00000005
      Positive = Right | Up, // 0x0000000A
      Vertical = Up | Down, // 0x00000003
      Horizontal = Right | Left, // 0x0000000C
      Press = Horizontal | Vertical, // 0x0000000F
    }
  }
}
