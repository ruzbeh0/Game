// Decompiled with JetBrains decompiler
// Type: Game.Input.MaskVector2Processor
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine;

#nullable disable
namespace Game.Input
{
  public class MaskVector2Processor : BaseMaskProcessor<Vector2>
  {
    static MaskVector2Processor() => MaskVector2Processor.Initialize();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() => UnityEngine.InputSystem.InputSystem.RegisterProcessor<MaskVector2Processor>();
  }
}
