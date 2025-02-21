// Decompiled with JetBrains decompiler
// Type: Game.Input.SmoothVector2Processor
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine;

#nullable disable
namespace Game.Input
{
  public class SmoothVector2Processor : SmoothProcessor<Vector2>
  {
    private const float kDelta = 1E-12f;

    protected override Vector2 Smooth(Vector2 value, ref Vector2 lastValue, float delta)
    {
      if ((double) this.m_Smoothing > 0.0)
      {
        float t = Mathf.Pow(this.m_Smoothing, delta);
        value = Vector2.Lerp(value, lastValue, t);
        if ((double) value.sqrMagnitude < 9.999999960041972E-13)
          value = Vector2.zero;
      }
      lastValue = value;
      if (this.m_Time)
        value *= Time.deltaTime;
      return value;
    }

    static SmoothVector2Processor() => SmoothVector2Processor.Initialize();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() => UnityEngine.InputSystem.InputSystem.RegisterProcessor<SmoothVector2Processor>();
  }
}
