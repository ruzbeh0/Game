// Decompiled with JetBrains decompiler
// Type: Game.Input.DeviceListener
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

#nullable disable
namespace Game.Input
{
  public class DeviceListener : IInputStateChangeMonitor, IDisposable
  {
    private List<InputControl> m_Controls;
    private bool m_Listening;
    private float m_RequiredDelta;
    private float m_Delta;
    private bool m_Activated;
    public DeviceListener.DeviceEvent EventDeviceActivated;

    public InputDevice device { get; private set; }

    public DeviceListener(InputDevice device, float requiredDelta)
    {
      this.EventDeviceActivated = new DeviceListener.DeviceEvent();
      this.device = device;
      this.m_Controls = new List<InputControl>();
      this.m_RequiredDelta = requiredDelta;
      foreach (InputControl allControl in device.allControls)
      {
        if (this.ValidateControl(allControl))
          this.m_Controls.Add(allControl);
      }
    }

    public void Tick()
    {
      this.m_Delta = Math.Max(this.m_Delta - Time.deltaTime, 0.0f);
      if (!this.m_Activated)
        return;
      this.m_Activated = false;
      this.EventDeviceActivated?.Invoke(this.device);
    }

    public void StartListening()
    {
      if (this.m_Listening)
        return;
      this.m_Activated = false;
      this.m_Listening = true;
      this.m_Delta = 0.0f;
      foreach (InputControl control in this.m_Controls)
        InputState.AddChangeMonitor(control, (IInputStateChangeMonitor) this);
    }

    public void StopListening()
    {
      if (!this.m_Listening)
        return;
      this.m_Activated = false;
      this.m_Listening = false;
      this.m_Delta = 0.0f;
      foreach (InputControl control in this.m_Controls)
        InputState.RemoveChangeMonitor(control, (IInputStateChangeMonitor) this);
    }

    private bool ValidateControl(InputControl control) => control is ButtonControl;

    public void NotifyControlStateChanged(
      InputControl control,
      double time,
      InputEventPtr eventPtr,
      long monitorIndex)
    {
      if (!(control is ButtonControl) || !((ButtonControl) control).wasPressedThisFrame)
        return;
      this.m_Activated = true;
    }

    public void NotifyTimerExpired(
      InputControl control,
      double time,
      long monitorIndex,
      int timerIndex)
    {
    }

    public void Dispose() => this.StopListening();

    public class DeviceEvent : UnityEvent<InputDevice>
    {
    }
  }
}
