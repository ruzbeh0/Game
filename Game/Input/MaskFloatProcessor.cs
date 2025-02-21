// Decompiled with JetBrains decompiler
// Type: Game.Input.MaskFloatProcessor
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine;

#nullable disable
namespace Game.Input
{
  public class MaskFloatProcessor : BaseMaskProcessor<float>
  {
    static MaskFloatProcessor() => MaskFloatProcessor.Initialize();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() => UnityEngine.InputSystem.InputSystem.RegisterProcessor<MaskFloatProcessor>();
  }
}
