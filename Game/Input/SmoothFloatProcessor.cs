// Decompiled with JetBrains decompiler
// Type: Game.Input.SmoothFloatProcessor
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine;

#nullable disable
namespace Game.Input
{
  public class SmoothFloatProcessor : SmoothProcessor<float>
  {
    private const float kDelta = 1E-06f;

    protected override float Smooth(float value, ref float lastValue, float delta)
    {
      if ((double) this.m_Smoothing > 0.0)
      {
        float t = Mathf.Pow(this.m_Smoothing, delta);
        value = Mathf.Lerp(value, lastValue, t);
        if ((double) Mathf.Abs(value) < 9.9999999747524271E-07)
          value = 0.0f;
      }
      lastValue = value;
      if (this.m_Time)
        value *= Time.deltaTime;
      return value;
    }

    static SmoothFloatProcessor() => SmoothFloatProcessor.Initialize();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() => UnityEngine.InputSystem.InputSystem.RegisterProcessor<SmoothFloatProcessor>();
  }
}
