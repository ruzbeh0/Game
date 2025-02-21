// Decompiled with JetBrains decompiler
// Type: Game.SceneFlow.SwitchUserScreen
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Game.SceneFlow
{
  public class SwitchUserScreen : FullScreenOverlay
  {
    protected override OverlayScreen overlayScreen => OverlayScreen.Wait;

    public override async Task Execute(GameManager manager, CancellationToken token)
    {
      SwitchUserScreen switchUserScreen = this;
      using (manager.inputManager.CreateOverlayBarrier(nameof (SwitchUserScreen)))
      {
        OverlayBindings.ScopedScreen scope = manager.userInterface.overlayBindings.ActivateScreenScoped(switchUserScreen.overlayScreen);
        try
        {
          int num = (int) await PlatformManager.instance.SignIn(SignInOptions.None, (Action<Task>) null);
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
