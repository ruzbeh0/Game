// Decompiled with JetBrains decompiler
// Type: Game.SceneFlow.LoadingScreen
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Game.SceneFlow
{
  public class LoadingScreen : IScreenState
  {
    public async Task Execute(GameManager manager, CancellationToken token)
    {
      OverlayBindings overlay = manager.userInterface.overlayBindings;
      float[] progress = new float[3];
      using (manager.inputManager.CreateOverlayBarrier(nameof (LoadingScreen)))
      {
        OverlayBindings.ScopedScreen scopedScreen = overlay.ActivateScreenScoped(OverlayScreen.Loading);
        try
        {
          while (Poll(progress))
          {
            token.ThrowIfCancellationRequested();
            overlay.SetProgress(OverlayProgressType.Outer, progress[0]);
            overlay.SetProgress(OverlayProgressType.Middle, progress[1]);
            overlay.SetProgress(OverlayProgressType.Inner, progress[2]);
            await Task.Delay(100, token);
          }
          overlay.SetProgress(OverlayProgressType.Outer, 1f);
          overlay.SetProgress(OverlayProgressType.Middle, 1f);
          overlay.SetProgress(OverlayProgressType.Inner, 1f);
          for (int i = 0; i < 30; ++i)
          {
            token.ThrowIfCancellationRequested();
            await Task.Yield();
          }
        }
        finally
        {
          scopedScreen.Dispose();
        }
        scopedScreen = new OverlayBindings.ScopedScreen();
      }
      overlay = (OverlayBindings) null;
      progress = (float[]) null;

      static bool Poll(float[] progress)
      {
        progress[0] = TaskManager.instance.GetTaskProgress(ProgressTracker.Group.Group1);
        progress[1] = TaskManager.instance.GetTaskProgress(ProgressTracker.Group.Group2);
        progress[2] = TaskManager.instance.GetTaskProgress(ProgressTracker.Group.Group3);
        return ((IEnumerable<float>) progress).Any<float>((Func<float, bool>) (p => (double) p < 1.0));
      }
    }
  }
}
