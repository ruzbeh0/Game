// Decompiled with JetBrains decompiler
// Type: Game.Input.InputRecorder
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

#nullable disable
namespace Game.Input
{
  public class InputRecorder
  {
    private bool m_RecordFrames = true;
    private bool m_ReplayOnNewDevices;
    private bool m_SimulateOriginalTimingOnReplay;
    private bool m_RecordStateEventsOnly;
    [InputControl(layout = "InputDevice")]
    private string m_DevicePath;
    private string m_RecordButtonPath = "<Keyboard>/f9";
    private InputRecorder.ChangeEvent m_ChangeEvent;
    private Action<InputEventPtr, InputDevice> m_OnInputEventDelegate;
    private InputEventTrace m_EventTrace;
    private InputEventTrace.ReplayController m_ReplayController;

    public bool captureIsRunning => this.m_EventTrace != null && this.m_EventTrace.enabled;

    public bool replayIsRunning
    {
      get => this.m_ReplayController != null && !this.m_ReplayController.finished;
    }

    public long eventCount
    {
      get
      {
        InputEventTrace eventTrace = this.m_EventTrace;
        return eventTrace == null ? 0L : eventTrace.eventCount;
      }
    }

    public long totalEventSizeInBytes
    {
      get
      {
        InputEventTrace eventTrace = this.m_EventTrace;
        return eventTrace == null ? 0L : eventTrace.totalEventSizeInBytes;
      }
    }

    public long allocatedSizeInBytes
    {
      get
      {
        InputEventTrace eventTrace = this.m_EventTrace;
        return eventTrace == null ? 0L : eventTrace.allocatedSizeInBytes;
      }
    }

    public bool recordFrames
    {
      get => this.m_RecordFrames;
      set
      {
        if (this.m_RecordFrames == value)
          return;
        this.m_RecordFrames = value;
        if (this.m_EventTrace == null)
          return;
        this.m_EventTrace.recordFrameMarkers = this.m_RecordFrames;
      }
    }

    public bool recordStateEventsOnly
    {
      get => this.m_RecordStateEventsOnly;
      set => this.m_RecordStateEventsOnly = value;
    }

    public string devicePath
    {
      get => this.m_DevicePath;
      set => this.m_DevicePath = value;
    }

    public string recordButtonPath
    {
      get => this.m_RecordButtonPath;
      set
      {
        this.m_RecordButtonPath = value;
        this.HookOnInputEvent();
      }
    }

    public InputEventTrace capture => this.m_EventTrace;

    public InputEventTrace.ReplayController replay => this.m_ReplayController;

    public int replayPosition
    {
      get => this.m_ReplayController != null ? this.m_ReplayController.position : 0;
    }

    public bool replayOnNewDevices
    {
      get => this.m_ReplayOnNewDevices;
      set => this.m_ReplayOnNewDevices = value;
    }

    public bool simulateOriginalTimingOnReplay
    {
      get => this.m_SimulateOriginalTimingOnReplay;
      set => this.m_SimulateOriginalTimingOnReplay = value;
    }

    public InputRecorder.ChangeEvent changeEvent
    {
      get
      {
        if (this.m_ChangeEvent == null)
          this.m_ChangeEvent = new InputRecorder.ChangeEvent();
        return this.m_ChangeEvent;
      }
    }

    public void StartCapture()
    {
      if (this.m_EventTrace != null && this.m_EventTrace.enabled)
        return;
      this.CreateEventTrace();
      this.m_EventTrace.Enable();
      this.m_ChangeEvent?.Invoke(InputRecorder.Change.CaptureStarted);
    }

    public void StopCapture()
    {
      if (this.m_EventTrace == null || !this.m_EventTrace.enabled)
        return;
      this.m_EventTrace.Disable();
      this.m_ChangeEvent?.Invoke(InputRecorder.Change.CaptureStopped);
    }

    public void StartReplay()
    {
      if (this.m_EventTrace == null)
        return;
      if (this.replayIsRunning && this.replay.paused)
      {
        this.replay.paused = false;
      }
      else
      {
        this.StopCapture();
        this.m_ReplayController = this.m_EventTrace.Replay().OnFinished(new Action(this.StopReplay)).OnEvent((Action<InputEventPtr>) (_ => this.m_ChangeEvent?.Invoke(InputRecorder.Change.EventPlayed)));
        if (this.m_ReplayOnNewDevices)
          this.m_ReplayController.WithAllDevicesMappedToNewInstances();
        if (this.m_SimulateOriginalTimingOnReplay)
          this.m_ReplayController.PlayAllEventsAccordingToTimestamps();
        else
          this.m_ReplayController.PlayAllFramesOneByOne();
        this.m_ChangeEvent?.Invoke(InputRecorder.Change.ReplayStarted);
      }
    }

    public void StopReplay()
    {
      if (this.m_ReplayController == null)
        return;
      this.m_ReplayController.Dispose();
      this.m_ReplayController = (InputEventTrace.ReplayController) null;
      this.m_ChangeEvent?.Invoke(InputRecorder.Change.ReplayStopped);
    }

    public void PauseReplay()
    {
      if (this.m_ReplayController == null)
        return;
      this.m_ReplayController.paused = true;
    }

    public void ClearCapture() => this.m_EventTrace?.Clear();

    public void LoadCaptureFromFile(string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException(nameof (fileName));
      this.CreateEventTrace();
      this.m_EventTrace.ReadFrom(fileName);
    }

    public void SaveCaptureToFile(string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException(nameof (fileName));
      this.m_EventTrace?.WriteTo(fileName);
    }

    public void Dispose()
    {
      this.StopCapture();
      this.StopReplay();
      this.UnhookOnInputEvent();
      this.m_ReplayController?.Dispose();
      this.m_ReplayController = (InputEventTrace.ReplayController) null;
      this.m_EventTrace?.Dispose();
      this.m_EventTrace = (InputEventTrace) null;
    }

    private bool OnFilterInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
      if (this.m_RecordStateEventsOnly && !eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
        return false;
      return string.IsNullOrEmpty(this.m_DevicePath) || device == null || InputControlPath.MatchesPrefix(this.m_DevicePath, (InputControl) device);
    }

    private void OnEventRecorded(InputEventPtr eventPtr)
    {
      this.m_ChangeEvent?.Invoke(InputRecorder.Change.EventCaptured);
    }

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
      if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>() || string.IsNullOrEmpty(this.m_RecordButtonPath) || !(InputControlPath.TryFindControl((InputControl) device, this.m_RecordButtonPath) is InputControl<float> control) || (double) control.ReadValueFromEvent<float>(eventPtr) < (double) UnityEngine.InputSystem.InputSystem.settings.defaultButtonPressPoint)
        return;
      if (this.captureIsRunning)
        this.StopCapture();
      else
        this.StartCapture();
      eventPtr.handled = true;
    }

    public int captureMemoryDefaultSize { get; set; } = 2097152;

    public int captureMemoryMaxSize { get; set; } = 10485760;

    public InputRecorder() => this.HookOnInputEvent();

    private void CreateEventTrace()
    {
      if (this.m_EventTrace == null || this.m_EventTrace.maxSizeInBytes == 0L)
      {
        this.m_EventTrace?.Dispose();
        this.m_EventTrace = new InputEventTrace((long) this.captureMemoryDefaultSize, true, (long) this.captureMemoryMaxSize);
      }
      this.m_EventTrace.recordFrameMarkers = this.m_RecordFrames;
      this.m_EventTrace.onFilterEvent += new Func<InputEventPtr, InputDevice, bool>(this.OnFilterInputEvent);
      this.m_EventTrace.onEvent += new Action<InputEventPtr>(this.OnEventRecorded);
    }

    private void HookOnInputEvent()
    {
      if (string.IsNullOrEmpty(this.m_RecordButtonPath))
      {
        this.UnhookOnInputEvent();
      }
      else
      {
        this.UnhookOnInputEvent();
        if (this.m_OnInputEventDelegate == null)
          this.m_OnInputEventDelegate = new Action<InputEventPtr, InputDevice>(this.OnInputEvent);
        UnityEngine.InputSystem.InputSystem.onEvent += this.m_OnInputEventDelegate;
      }
    }

    private void UnhookOnInputEvent()
    {
      if (this.m_OnInputEventDelegate == null)
        return;
      UnityEngine.InputSystem.InputSystem.onEvent -= this.m_OnInputEventDelegate;
    }

    public enum Change
    {
      None,
      EventCaptured,
      EventPlayed,
      CaptureStarted,
      CaptureStopped,
      ReplayStarted,
      ReplayStopped,
    }

    [Serializable]
    public class ChangeEvent : UnityEvent<InputRecorder.Change>
    {
    }
  }
}
