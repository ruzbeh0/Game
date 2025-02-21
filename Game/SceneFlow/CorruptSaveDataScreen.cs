// Decompiled with JetBrains decompiler
// Type: Game.SceneFlow.CorruptSaveDataScreen
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.SceneFlow
{
  public class CorruptSaveDataScreen : FullScreenOverlay
  {
    protected override OverlayScreen overlayScreen => OverlayScreen.CorruptSaveData;

    protected override string actionA => "AnyKey";

    public override async Task Execute(GameManager manager, CancellationToken token)
    {
      CorruptSaveDataScreen corruptSaveDataScreen = this;
      using (EnabledActionScoped continueAction = new EnabledActionScoped(manager, "Engagement", corruptSaveDataScreen.actionA, new Func<OverlayScreen, bool>(((FullScreenOverlay) corruptSaveDataScreen).HandleScreenChange), corruptSaveDataScreen.continueDisplayProperty, corruptSaveDataScreen.continueDisplayPriority))
      {
        using (manager.inputManager.CreateOverlayBarrier(nameof (CorruptSaveDataScreen)))
        {
          OverlayBindings.ScopedScreen scope = manager.userInterface.overlayBindings.ActivateScreenScoped(corruptSaveDataScreen.overlayScreen);
          try
          {
            (bool, InputDevice) valueTuple = await IScreenState.WaitForInput((InputAction) continueAction, (InputAction) null, (Action) null, token);
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
