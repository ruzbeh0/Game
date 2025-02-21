// Decompiled with JetBrains decompiler
// Type: Game.Input.CameraZoomProcessor
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Settings;
using UnityEngine;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.Input
{
  public class CameraZoomProcessor : PlatformProcessor<float>
  {
    public float m_Scale = 1f;

    public override float Process(float value, InputControl control)
    {
      if (!this.needProcess)
        return value;
      Game.Settings.InputSettings input = SharedSettings.instance.input;
      value *= this.m_Scale;
      float num1 = value;
      float num2;
      switch (this.m_DeviceType)
      {
        case ProcessorDeviceType.Keyboard:
          num2 = input.keyboardZoomSensitivity;
          break;
        case ProcessorDeviceType.Mouse:
          num2 = input.mouseZoomSensitivity;
          break;
        case ProcessorDeviceType.Gamepad:
          num2 = input.gamepadZoomSensitivity;
          break;
        default:
          num2 = 1f;
          break;
      }
      value = num1 * num2;
      return value;
    }

    static CameraZoomProcessor() => CameraZoomProcessor.Initialize();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() => UnityEngine.InputSystem.InputSystem.RegisterProcessor<CameraZoomProcessor>();
  }
}
