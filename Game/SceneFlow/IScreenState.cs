// Decompiled with JetBrains decompiler
// Type: Game.SceneFlow.IScreenState
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Game.Input;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.SceneFlow
{
  public interface IScreenState
  {
    static Task WaitForWaitingState(InputAction inputAction)
    {
      if (inputAction.phase == InputActionPhase.Waiting)
        return Task.CompletedTask;
      TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
      System.Timers.Timer timer = new System.Timers.Timer(100.0 / 3.0);
      timer.Elapsed += (ElapsedEventHandler) ((sender, e) =>
      {
        if (inputAction.phase != InputActionPhase.Waiting)
          return;
        taskCompletionSource.SetResult(true);
        timer.Stop();
        timer.Dispose();
      });
      timer.AutoReset = true;
      timer.Start();
      return (Task) taskCompletionSource.Task;
    }

    static async Task<(bool ok, InputDevice device)> WaitForInput(
      InputAction inputContinue,
      InputAction inputCancel,
      Action cancel,
      CancellationToken token)
    {
      TaskCompletionSource<(bool, InputDevice)> performed = new TaskCompletionSource<(bool, InputDevice)>();
      (bool, InputDevice) task;
      using (token.Register((Action) (() => performed.TrySetCanceled())))
      {
        inputContinue.performed += new Action<InputAction.CallbackContext>(Handler);
        if (inputCancel != null)
          inputCancel.performed += new Action<InputAction.CallbackContext>(Handler);
        if (cancel != null)
          cancel += new Action(CancelHandler);
        GameManager.instance.userInterface.inputHintBindings.onInputHintPerformed += new Action<ProxyAction>(InputHintPerformedHandler);
        try
        {
          task = await performed.Task;
        }
        finally
        {
          inputContinue.performed -= new Action<InputAction.CallbackContext>(Handler);
          inputContinue.Reset();
          if (inputCancel != null)
          {
            inputCancel.performed -= new Action<InputAction.CallbackContext>(Handler);
            inputCancel.Reset();
          }
          if (cancel != null)
            cancel -= new Action(CancelHandler);
          GameManager.instance.userInterface.inputHintBindings.onInputHintPerformed -= new Action<ProxyAction>(InputHintPerformedHandler);
        }
      }
      return task;

      void Handler(InputAction.CallbackContext c)
      {
        performed.TrySetResult((inputContinue == c.action, c.action.activeControl?.device));
      }

      void CancelHandler() => performed.TrySetCanceled();

      void InputHintPerformedHandler(ProxyAction action)
      {
        if (action.sourceAction == inputContinue)
        {
          performed.TrySetResult((true, (InputDevice) Mouse.current));
        }
        else
        {
          if (action.sourceAction != inputCancel)
            return;
          performed.TrySetCanceled();
        }
      }
    }

    static async Task<object> WaitForDevice(
      Game.Input.InputManager manager,
      Action cancel,
      CancellationToken token)
    {
      TaskCompletionSource<object> devicePaired = new TaskCompletionSource<object>();
      object task;
      using (token.Register((Action) (() => devicePaired.TrySetCanceled())))
      {
        manager.EventDevicePaired += new Action(Handler);
        if (cancel != null)
          cancel += new Action(CancelHandler);
        try
        {
          task = await devicePaired.Task;
        }
        finally
        {
          manager.EventDevicePaired -= new Action(Handler);
          if (cancel != null)
            cancel -= new Action(CancelHandler);
        }
      }
      return task;

      void Handler() => devicePaired.TrySetResult((object) null);

      void CancelHandler() => devicePaired.TrySetCanceled();
    }

    static async Task<UserChangedFlags> WaitForUser(Action cancel, CancellationToken token)
    {
      TaskCompletionSource<UserChangedFlags> userSignedBackIn = new TaskCompletionSource<UserChangedFlags>();
      UserChangedFlags task;
      using (token.Register((Action) (() => userSignedBackIn.TrySetCanceled())))
      {
        PlatformManager.instance.onUserUpdated += new OnUserUpdatedEventHandler(Handler);
        if (cancel != null)
          cancel += new Action(CancelHandler);
        try
        {
          task = await userSignedBackIn.Task;
        }
        finally
        {
          PlatformManager.instance.onUserUpdated -= new OnUserUpdatedEventHandler(Handler);
          if (cancel != null)
            cancel -= new Action(CancelHandler);
        }
      }
      return task;

      void Handler(IPlatformServiceIntegration psi, UserChangedFlags flags)
      {
        if (!PlatformManager.instance.IsPrincipalUserIntegration(psi) || !flags.HasFlag((Enum) UserChangedFlags.UserSignedInAgain))
          return;
        userSignedBackIn.TrySetResult(flags);
      }

      void CancelHandler() => userSignedBackIn.TrySetCanceled();
    }

    Task Execute(GameManager manager, CancellationToken token);
  }
}
