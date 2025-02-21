// Decompiled with JetBrains decompiler
// Type: Game.Rendering.CinematicCamera.PhotoModeProperty
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Rendering.CinematicCamera
{
  public class PhotoModeProperty
  {
    public string id { get; set; }

    public string group { get; set; }

    public Action<float> setValue { get; set; }

    public Func<float> getValue { get; set; }

    public Func<float> min { get; set; }

    public Func<float> max { get; set; }

    public Func<bool> isAvailable { get; set; }

    public Func<bool> isEnabled { get; set; }

    public Action<bool> setEnabled { get; set; }

    public Action reset { get; set; }

    public int fractionDigits { get; set; } = 3;

    public Type enumType { get; set; }

    public PhotoModeProperty.OverrideControl overrideControl { get; set; }

    public enum OverrideControl
    {
      None,
      Checkbox,
      ColorField,
    }
  }
}
