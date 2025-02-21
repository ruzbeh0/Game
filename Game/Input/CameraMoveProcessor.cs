// Decompiled with JetBrains decompiler
// Type: Game.Input.CameraMoveProcessor
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Settings;
using UnityEngine;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.Input
{
  public class CameraMoveProcessor : PlatformProcessor<Vector2>
  {
    public float m_ScaleX = 1f;
    public float m_ScaleY = 1f;

    public override Vector2 Process(Vector2 value, InputControl control)
    {
      if (!this.needProcess)
        return value;
      Game.Settings.InputSettings input = SharedSettings.instance.input;
      value.x *= this.m_ScaleX;
      value.y *= this.m_ScaleY;
      Vector2 vector2 = value;
      float num;
      switch (this.m_DeviceType)
      {
        case ProcessorDeviceType.Keyboard:
          num = input.keyboardMoveSensitivity;
          break;
        case ProcessorDeviceType.Mouse:
          num = input.mouseMoveSensitivity;
          break;
        case ProcessorDeviceType.Gamepad:
          num = input.gamepadMoveSensitivity;
          break;
        default:
          num = 1f;
          break;
      }
      value = vector2 * num;
      return value;
    }

    static CameraMoveProcessor() => CameraMoveProcessor.Initialize();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() => UnityEngine.InputSystem.InputSystem.RegisterProcessor<CameraMoveProcessor>();
  }
}
