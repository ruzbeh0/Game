// Decompiled with JetBrains decompiler
// Type: Game.SceneFlow.EngagementScreen
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
  public class EngagementScreen : FullScreenOverlay
  {
    protected override OverlayScreen overlayScreen => OverlayScreen.Engagement;

    protected override string actionA => "AnyKey";

    protected override string continueDisplayProperty => "Start Game";

    public override async Task Execute(GameManager manager, CancellationToken token)
    {
      EngagementScreen engagementScreen = this;
      using (EnabledActionScoped continueAction = new EnabledActionScoped(manager, "Engagement", engagementScreen.actionA, new Func<OverlayScreen, bool>(((FullScreenOverlay) engagementScreen).HandleScreenChange), engagementScreen.continueDisplayProperty, engagementScreen.continueDisplayPriority))
      {
        using (manager.inputManager.CreateOverlayBarrier(nameof (EngagementScreen)))
        {
          OverlayBindings.ScopedScreen scope = manager.userInterface.overlayBindings.ActivateScreenScoped(engagementScreen.overlayScreen);
          try
          {
            while (!engagementScreen.m_Done)
            {
              Task<(bool, InputDevice)> input = IScreenState.WaitForInput((InputAction) continueAction, (InputAction) null, engagementScreen.m_CompletedEvent, token);
              (bool, InputDevice) valueTuple = await input;
              if (input.IsCompletedSuccessfully)
              {
                if (!PlatformManager.instance.isUserSignedIn)
                {
                  SignInFlags signInFlags = await PlatformManager.instance.SignIn(SignInOptions.WithUI, new Action<Task>(UserChangingCallback));
                  engagementScreen.m_Done = signInFlags.HasFlag((Enum) SignInFlags.Success);
                }
                else
                {
                  bool flag = await PlatformManager.instance.AssociateDevice(input.Result.Item2);
                  engagementScreen.m_Done = flag;
                }
              }
              else
                engagementScreen.m_Done = true;
              input = (Task<(bool, InputDevice)>) null;
            }
          }
          finally
          {
            scope.Dispose();
          }
          scope = new OverlayBindings.ScopedScreen();
        }
      }

      void UserChangingCallback(Task signInTask)
      {
        new WaitScreen().Execute(manager, token, signInTask);
      }
    }
  }
}
