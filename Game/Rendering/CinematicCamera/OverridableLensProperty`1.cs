// Decompiled with JetBrains decompiler
// Type: Game.Rendering.CinematicCamera.OverridableLensProperty`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Rendering.CinematicCamera
{
  public class OverridableLensProperty<T>
  {
    private CameraUpdateSystem m_CameraUpdateSystem;
    private readonly Action<IGameCameraController, T> m_Setter;
    private readonly Func<IGameCameraController, T> m_Getter;
    private T m_Value;
    private bool m_OverrideState;

    public bool overrideState
    {
      get => this.m_OverrideState;
      set
      {
        if (!value)
          this.SetDefault();
        else
          this.Apply(this.m_Value);
        this.m_OverrideState = value;
      }
    }

    public OverridableLensProperty(
      CameraUpdateSystem cameraUpdateSystem,
      Action<IGameCameraController, T> setter,
      Func<IGameCameraController, T> getter)
    {
      this.m_CameraUpdateSystem = cameraUpdateSystem;
      this.m_Setter = setter;
      this.m_Getter = getter;
    }

    public void Override(T v)
    {
      this.value = v;
      this.overrideState = true;
    }

    private void Apply(T v)
    {
      if (!((UnityEngine.Object) this.m_CameraUpdateSystem.cinematicCameraController != (UnityEngine.Object) null))
        return;
      this.m_Setter((IGameCameraController) this.m_CameraUpdateSystem.cinematicCameraController, v);
    }

    public T currentValue
    {
      get
      {
        if (this.overrideState)
          return this.value;
        return (UnityEngine.Object) this.m_CameraUpdateSystem.cinematicCameraController != (UnityEngine.Object) null ? this.m_Getter((IGameCameraController) this.m_CameraUpdateSystem.cinematicCameraController) : default (T);
      }
    }

    private T GetDefault()
    {
      return (UnityEngine.Object) this.m_CameraUpdateSystem.gamePlayController != (UnityEngine.Object) null ? this.m_Getter((IGameCameraController) this.m_CameraUpdateSystem.gamePlayController) : default (T);
    }

    public T value
    {
      get => this.m_Value;
      set
      {
        this.m_Value = value;
        if (!this.overrideState)
          return;
        this.Apply(this.m_Value);
      }
    }

    public void Sync()
    {
      this.m_Value = this.GetDefault();
      this.Apply(this.m_Value);
    }

    private void SetDefault() => this.Apply(this.GetDefault());
  }
}
