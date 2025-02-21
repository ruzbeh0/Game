// Decompiled with JetBrains decompiler
// Type: Game.SceneFlow.ControllerDisconnectedScreen
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
  public class ControllerDisconnectedScreen : FullScreenOverlay
  {
    protected override OverlayScreen overlayScreen => OverlayScreen.ControllerDisconnected;

    protected override string actionA => "AnyKey";

    protected override string continueDisplayProperty => "Continue";

    public override async Task Execute(GameManager manager, CancellationToken token)
    {
      ControllerDisconnectedScreen disconnectedScreen = this;
      using (EnabledActionScoped continueAction = new EnabledActionScoped(manager, "Engagement", disconnectedScreen.actionA, new Func<OverlayScreen, bool>(((FullScreenOverlay) disconnectedScreen).HandleScreenChange), disconnectedScreen.continueDisplayProperty, disconnectedScreen.continueDisplayPriority))
      {
        using (manager.inputManager.CreateOverlayBarrier(nameof (ControllerDisconnectedScreen)))
        {
          OverlayBindings.ScopedScreen scope = manager.userInterface.overlayBindings.ActivateScreenScoped(disconnectedScreen.overlayScreen);
          try
          {
            while (!disconnectedScreen.m_Done)
            {
              Task<(bool, InputDevice)> input = IScreenState.WaitForInput((InputAction) continueAction, (InputAction) null, disconnectedScreen.m_CompletedEvent, token);
              Task<object> device = IScreenState.WaitForDevice(manager.inputManager, disconnectedScreen.m_CompletedEvent, token);
              Task task = await Task.WhenAny((Task) input, (Task) device);
              Action completedEvent = disconnectedScreen.m_CompletedEvent;
              if (completedEvent != null)
                completedEvent();
              if (input.IsCompletedSuccessfully)
              {
                bool flag = await PlatformManager.instance.AssociateDevice(input.Result.Item2);
                disconnectedScreen.m_Done = flag;
              }
              else if (device.IsCompletedSuccessfully)
                disconnectedScreen.m_Done = true;
              else
                disconnectedScreen.m_Done = true;
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
  }
}
