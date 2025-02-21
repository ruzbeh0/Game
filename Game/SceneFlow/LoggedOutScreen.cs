// Decompiled with JetBrains decompiler
// Type: Game.SceneFlow.LoggedOutScreen
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
  public class LoggedOutScreen : FullScreenOverlay
  {
    protected override OverlayScreen overlayScreen => OverlayScreen.UserLoggedOut;

    protected override string actionA => "AnyKey";

    protected override string continueDisplayProperty => "Continue";

    public override async Task Execute(GameManager manager, CancellationToken token)
    {
      LoggedOutScreen loggedOutScreen = this;
      using (EnabledActionScoped continueAction = new EnabledActionScoped(manager, "Engagement", loggedOutScreen.actionA, new Func<OverlayScreen, bool>(((FullScreenOverlay) loggedOutScreen).HandleScreenChange), loggedOutScreen.continueDisplayProperty, loggedOutScreen.continueDisplayPriority))
      {
        using (manager.inputManager.CreateOverlayBarrier(nameof (LoggedOutScreen)))
        {
          OverlayBindings.ScopedScreen scope = manager.userInterface.overlayBindings.ActivateScreenScoped(OverlayScreen.UserLoggedOut);
          try
          {
            while (!loggedOutScreen.m_Done)
            {
              Task<(bool, InputDevice)> input = IScreenState.WaitForInput((InputAction) continueAction, (InputAction) null, loggedOutScreen.m_CompletedEvent, token);
              Task<UserChangedFlags> user = IScreenState.WaitForUser(loggedOutScreen.m_CompletedEvent, token);
              Task task = await Task.WhenAny((Task) input, (Task) user);
              Action completedEvent = loggedOutScreen.m_CompletedEvent;
              if (completedEvent != null)
                completedEvent();
              if (input.IsCompletedSuccessfully)
              {
                SignInFlags signInFlags = await PlatformManager.instance.SignIn(SignInOptions.None, new Action<Task>(UserChangingCallback));
                loggedOutScreen.m_Done = signInFlags.HasFlag((Enum) SignInFlags.Success);
                if (signInFlags.HasFlag((Enum) SignInFlags.UserChanged) && manager.gameMode.IsGameOrEditor())
                  manager.MainMenu();
              }
              else if (user.IsCompletedSuccessfully)
                loggedOutScreen.m_Done = true;
              else
                loggedOutScreen.m_Done = true;
              input = (Task<(bool, InputDevice)>) null;
              user = (Task<UserChangedFlags>) null;
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
