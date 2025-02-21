// Decompiled with JetBrains decompiler
// Type: Game.SceneFlow.ControllerPairingScreen
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.SceneFlow
{
  public class ControllerPairingScreen : FullScreenOverlay
  {
    protected override OverlayScreen overlayScreen => OverlayScreen.ControllerPairingChanged;

    protected override string continueDisplayProperty => "Switch User";

    protected override string cancelDisplayProperty => "Cancel";

    protected override int cancelDisplayPriority => 30;

    public override async Task Execute(GameManager manager, CancellationToken token)
    {
      ControllerPairingScreen controllerPairingScreen = this;
      using (EnabledActionScoped continueAction = new EnabledActionScoped(manager, "Engagement", controllerPairingScreen.actionA, new Func<OverlayScreen, bool>(((FullScreenOverlay) controllerPairingScreen).HandleScreenChange), controllerPairingScreen.continueDisplayProperty, controllerPairingScreen.continueDisplayPriority))
      {
        using (EnabledActionScoped cancelAction = new EnabledActionScoped(manager, "Engagement", controllerPairingScreen.actionB, new Func<OverlayScreen, bool>(((FullScreenOverlay) controllerPairingScreen).HandleScreenChange), controllerPairingScreen.cancelDisplayProperty, controllerPairingScreen.cancelDisplayPriority))
        {
          using (manager.inputManager.CreateOverlayBarrier(nameof (ControllerPairingScreen)))
          {
            OverlayBindings.ScopedScreen scope = manager.userInterface.overlayBindings.ActivateScreenScoped(controllerPairingScreen.overlayScreen);
            try
            {
              while (!controllerPairingScreen.m_Done)
              {
                Task<(bool, InputDevice)> input = IScreenState.WaitForInput((InputAction) continueAction, (InputAction) cancelAction, controllerPairingScreen.m_CompletedEvent, token);
                Task<object> device = IScreenState.WaitForDevice(manager.inputManager, controllerPairingScreen.m_CompletedEvent, token);
                Task task = await Task.WhenAny((Task) input, (Task) device);
                Action completedEvent = controllerPairingScreen.m_CompletedEvent;
                if (completedEvent != null)
                  completedEvent();
                if (input.IsCompletedSuccessfully)
                {
                  if (input.Result.Item1)
                  {
                    SignInFlags signInFlags = await PlatformManager.instance.SignIn(SignInOptions.None, new Action<Task>(UserChangingCallback));
                    controllerPairingScreen.m_Done = signInFlags.HasFlag((Enum) SignInFlags.Success);
                    if (signInFlags.HasFlag((Enum) SignInFlags.UserChanged) && manager.gameMode.IsGameOrEditor())
                      manager.MainMenu();
                  }
                  else
                  {
                    bool flag = await PlatformManager.instance.AssociateDevice(input.Result.Item2);
                    controllerPairingScreen.m_Done = flag;
                  }
                }
                else if (device.IsCompletedSuccessfully)
                  controllerPairingScreen.m_Done = true;
                else
                  controllerPairingScreen.m_Done = true;
                input = (Task<(bool, InputDevice)>) null;
                device = (Task<object>) null;
              }
            }
            finally
            {
              scope.Dispose();
            }
            scope = new OverlayBindings.ScopedScreen();
          }
        }
      }

      void UserChangingCallback(Task signInTask)
      {
        new WaitScreen().Execute(manager, token, signInTask);
      }
    }
  }
}
