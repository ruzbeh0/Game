// Decompiled with JetBrains decompiler
// Type: Game.SceneFlow.ValidationScreen
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.SceneFlow
{
  public class ValidationScreen : FullScreenOverlay
  {
    protected override OverlayScreen overlayScreen => OverlayScreen.Validation;

    protected override string continueDisplayProperty => "Proceed";

    protected override string cancelDisplayProperty => "Back";

    protected override int cancelDisplayPriority => 30;

    public override async Task Execute(GameManager manager, CancellationToken token)
    {
      ValidationScreen validationScreen = this;
      using (EnabledActionScoped continueAction = new EnabledActionScoped(manager, "Engagement", validationScreen.actionA, new Func<OverlayScreen, bool>(((FullScreenOverlay) validationScreen).HandleScreenChange), validationScreen.continueDisplayProperty, validationScreen.continueDisplayPriority))
      {
        using (EnabledActionScoped cancelAction = new EnabledActionScoped(manager, "Engagement", validationScreen.actionB, new Func<OverlayScreen, bool>(((FullScreenOverlay) validationScreen).HandleScreenChange), validationScreen.cancelDisplayProperty, validationScreen.cancelDisplayPriority))
        {
          using (manager.inputManager.CreateOverlayBarrier(nameof (ValidationScreen)))
          {
            OverlayBindings.ScopedScreen scope = manager.userInterface.overlayBindings.ActivateScreenScoped(validationScreen.overlayScreen);
            try
            {
              Task<(bool, InputDevice)> input = IScreenState.WaitForInput((InputAction) continueAction, (InputAction) cancelAction, (Action) null, token);
              (bool, InputDevice) valueTuple = await input;
              if (input.IsCompletedSuccessfully)
              {
                if (input.Result.Item1)
                  Debug.Log((object) "OK");
                else
                  Debug.Log((object) "Cancel");
              }
              input = (Task<(bool, InputDevice)>) null;
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
}
