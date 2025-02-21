// Decompiled with JetBrains decompiler
// Type: Game.Input.CameraRotateProcessor
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Settings;
using UnityEngine;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.Input
{
  public class CameraRotateProcessor : PlatformProcessor<Vector2>
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
      ref float local1 = ref value.x;
      float num1 = local1;
      float num2;
      switch (this.m_DeviceType)
      {
        case ProcessorDeviceType.Keyboard:
          num2 = input.keyboardRotateSensitivity;
          break;
        case ProcessorDeviceType.Mouse:
          num2 = input.mouseInvertX ? -input.mouseRotateSensitivity : input.mouseRotateSensitivity;
          break;
        case ProcessorDeviceType.Gamepad:
          num2 = input.gamepadInvertX ? -input.gamepadRotateSensitivity : input.gamepadRotateSensitivity;
          break;
        default:
          num2 = 1f;
          break;
      }
      local1 = num1 * num2;
      ref float local2 = ref value.y;
      float num3 = local2;
      float num4;
      switch (this.m_DeviceType)
      {
        case ProcessorDeviceType.Keyboard:
          num4 = input.keyboardRotateSensitivity;
          break;
        case ProcessorDeviceType.Mouse:
          num4 = input.mouseInvertY ? -input.mouseRotateSensitivity : input.mouseRotateSensitivity;
          break;
        case ProcessorDeviceType.Gamepad:
          num4 = input.gamepadInvertY ? -1f : 1f;
          break;
        default:
          num4 = 1f;
          break;
      }
      local2 = num3 * num4;
      return value;
    }

    static CameraRotateProcessor() => CameraRotateProcessor.Initialize();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() => UnityEngine.InputSystem.InputSystem.RegisterProcessor<CameraRotateProcessor>();
  }
}
