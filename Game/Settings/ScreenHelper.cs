// Decompiled with JetBrains decompiler
// Type: Game.Settings.ScreenHelper
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Game.Settings
{
  public static class ScreenHelper
  {
    public static ScreenResolution currentResolution
    {
      get
      {
        Resolution currentResolution = Screen.currentResolution;
        if (Screen.fullScreen)
          return new ScreenResolution(currentResolution);
        return new ScreenResolution()
        {
          width = Screen.width,
          height = Screen.height,
          refreshRate = currentResolution.refreshRateRatio
        };
      }
    }

    private static ScreenResolution[] resolutions { get; } = new List<ScreenResolution>(((IEnumerable<Resolution>) Screen.resolutions).Select<Resolution, ScreenResolution>((Func<Resolution, ScreenResolution>) (r => new ScreenResolution(r)))).OrderByDescending<ScreenResolution, int>((Func<ScreenResolution, int>) (x => x.width)).ThenByDescending<ScreenResolution, int>((Func<ScreenResolution, int>) (x => x.height)).ThenByDescending<ScreenResolution, double>((Func<ScreenResolution, double>) (x => x.refreshRate.value)).ToArray<ScreenResolution>();

    public static ScreenResolution[] simpleResolutions { get; } = ((IEnumerable<ScreenResolution>) ScreenHelper.resolutions).GroupBy<ScreenResolution, (int, int, int)>((Func<ScreenResolution, (int, int, int)>) (r => (r.width, r.height, (int) Math.Round(r.refreshRate.value)))).Select<IGrouping<(int, int, int), ScreenResolution>, ScreenResolution>((Func<IGrouping<(int, int, int), ScreenResolution>, ScreenResolution>) (g => g.Aggregate<ScreenResolution, ScreenResolution>(g.First<ScreenResolution>(), (Func<ScreenResolution, ScreenResolution, ScreenResolution>) ((a, b) => b.refreshRateDelta >= a.refreshRateDelta ? a : b)))).ToArray<ScreenResolution>();

    public static ScreenResolution[] GetAvailableResolutions(bool all)
    {
      return !all ? ScreenHelper.simpleResolutions : ScreenHelper.resolutions;
    }

    public static ScreenResolution GetClosestAvailable(ScreenResolution sample, bool all)
    {
      ScreenResolution screenResolution = new ScreenResolution();
      foreach (ScreenResolution availableResolution in ScreenHelper.GetAvailableResolutions(all))
      {
        if (Math.Abs(availableResolution.width - sample.width) + Math.Abs(availableResolution.height - sample.height) < Math.Abs(screenResolution.width - sample.width) + Math.Abs(screenResolution.height - sample.height))
          screenResolution = availableResolution;
        else if (availableResolution.width == screenResolution.width && availableResolution.height == screenResolution.height && Math.Abs(availableResolution.refreshRate.value - sample.refreshRate.value) < Math.Abs(screenResolution.refreshRate.value - sample.refreshRate.value))
          screenResolution = availableResolution;
      }
      return screenResolution.width <= 0 || screenResolution.height <= 0 ? sample : screenResolution;
    }

    public static bool HideAdditionalResolutionOption()
    {
      return ScreenHelper.simpleResolutions.Length == ScreenHelper.resolutions.Length;
    }

    public static DisplayMode currentDisplayMode
    {
      get
      {
        DisplayMode currentDisplayMode;
        switch (Screen.fullScreenMode)
        {
          case FullScreenMode.ExclusiveFullScreen:
            currentDisplayMode = DisplayMode.Fullscreen;
            break;
          case FullScreenMode.FullScreenWindow:
            currentDisplayMode = DisplayMode.FullscreenWindow;
            break;
          case FullScreenMode.Windowed:
            currentDisplayMode = DisplayMode.Window;
            break;
          default:
            currentDisplayMode = DisplayMode.Window;
            break;
        }
        return currentDisplayMode;
      }
    }

    public static FullScreenMode GetFullscreenMode(DisplayMode displayMode)
    {
      FullScreenMode fullscreenMode;
      switch (displayMode)
      {
        case DisplayMode.Fullscreen:
          fullscreenMode = FullScreenMode.ExclusiveFullScreen;
          break;
        case DisplayMode.FullscreenWindow:
          fullscreenMode = FullScreenMode.FullScreenWindow;
          break;
        case DisplayMode.Window:
          fullscreenMode = FullScreenMode.Windowed;
          break;
        default:
          fullscreenMode = FullScreenMode.Windowed;
          break;
      }
      return fullscreenMode;
    }
  }
}
