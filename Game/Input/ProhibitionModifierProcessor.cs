// Decompiled with JetBrains decompiler
// Type: Game.Input.ProhibitionModifierProcessor
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.Input
{
  public class ProhibitionModifierProcessor : InputProcessor<float>
  {
    public override float Process(float value, InputControl control)
    {
      value = (double) value != 0.0 ? float.NaN : 1f;
      return value;
    }

    static ProhibitionModifierProcessor() => ProhibitionModifierProcessor.Initialize();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
      UnityEngine.InputSystem.InputSystem.RegisterProcessor<ProhibitionModifierProcessor>();
    }
  }
}
