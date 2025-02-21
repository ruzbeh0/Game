// Decompiled with JetBrains decompiler
// Type: Game.Input.SmoothProcessor`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.Input
{
  public abstract class SmoothProcessor<TValue> : InputProcessor<TValue>, IDisableableProcessor where TValue : struct
  {
    public float m_Smoothing = 1E-06f;
    public bool m_CanBeDisabled = true;
    public bool m_Time;
    private TValue m_LastValue;
    private int m_LastFrame;
    private float m_LastTime;
    private float m_LastDelta;

    public bool canBeDisabled => this.m_CanBeDisabled;

    public bool disabled { get; set; }

    public override TValue Process(TValue value, InputControl control)
    {
      if (this.canBeDisabled && this.disabled)
        return value;
      if (this.m_LastFrame == 0)
      {
        this.m_LastValue = default (TValue);
        this.m_LastTime = Time.time;
        this.m_LastFrame = Time.frameCount;
        value = this.Smooth(value, ref this.m_LastValue, Time.deltaTime);
      }
      else if (this.m_LastFrame == Time.frameCount)
      {
        TValue lastValue = this.m_LastValue;
        value = this.Smooth(value, ref lastValue, this.m_LastDelta);
      }
      else
      {
        this.m_LastDelta = Time.time - this.m_LastTime;
        value = this.Smooth(value, ref this.m_LastValue, this.m_LastDelta);
        this.m_LastTime = Time.time;
        this.m_LastFrame = Time.frameCount;
      }
      return value;
    }

    protected abstract TValue Smooth(TValue value, ref TValue lastValue, float delta);
  }
}
