// Decompiled with JetBrains decompiler
// Type: Game.SceneFlow.SplashScreenSequence
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.SceneFlow
{
  public class SplashScreenSequence : IScreenState
  {
    private const string kSkipSplashKeyAction = "Skip";

    private Task WaitForCompletion(TimeSpan delay, InputAction anyKey, CancellationToken token)
    {
      return (Task) Task.WhenAny(Task.Delay(delay, token), (Task) IScreenState.WaitForInput(anyKey, (InputAction) null, (Action) null, token));
    }

    public async Task Execute(GameManager manager, CancellationToken token)
    {
      using (EnabledActionScoped anyKey = new EnabledActionScoped(manager, "Splash screen", "Skip", new Func<OverlayScreen, bool>(HandleScreenChange)))
      {
        using (manager.inputManager.CreateOverlayBarrier(nameof (SplashScreenSequence)))
        {
          OverlayBindings overlay = manager.userInterface.overlayBindings;
          OverlayScreen[] splashes = this.GetSplashSequence().ToArray<OverlayScreen>();
          int i;
          for (i = 0; i < splashes.Length; ++i)
          {
            if (i == 0)
              overlay.ActivateScreen(splashes[i]);
            else
              overlay.SwapScreen(splashes[i - 1], splashes[i]);
            await this.WaitForCompletion(TimeSpan.FromSeconds(4.0), (InputAction) anyKey, token);
            overlay.DeactivateScreen(splashes[i]);
            token.ThrowIfCancellationRequested();
          }
          for (i = 0; i < 60; ++i)
            await Task.Yield();
          overlay = (OverlayBindings) null;
          splashes = (OverlayScreen[]) null;
        }
      }

      static bool HandleScreenChange(OverlayScreen screen)
      {
        if (screen == OverlayScreen.Splash1 || screen == OverlayScreen.Splash2 || screen == OverlayScreen.Splash3 || screen == OverlayScreen.Splash4 || screen == OverlayScreen.PiracyDisclaimer || screen == OverlayScreen.PhotosensitivityDisclaimer)
          Game.Input.InputManager.instance.AssociateActionsWithUser(false);
        else if (screen == OverlayScreen.None || screen == OverlayScreen.Loading)
          Game.Input.InputManager.instance.AssociateActionsWithUser(true);
        return screen == OverlayScreen.Splash1 || screen == OverlayScreen.Splash2 || screen == OverlayScreen.Splash3 || screen == OverlayScreen.Splash4;
      }
    }

    private IEnumerable<OverlayScreen> GetSplashSequence()
    {
      yield return OverlayScreen.Splash1;
      yield return OverlayScreen.Splash2;
      yield return OverlayScreen.Splash4;
    }
  }
}
