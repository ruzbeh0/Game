// Decompiled with JetBrains decompiler
// Type: Game.UI.InputBindings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Colossal.UI.Binding;
using Game.Input;
using Game.Settings;
using Game.Tools;
using System;
using UnityEngine;

#nullable disable
namespace Game.UI
{
  public class InputBindings : CompositeBinding, IDisposable
  {
    private const string kGroup = "input";
    private const float kCameraInputSensitivity = 0.2f;
    private const float kCameraInputSensitivitySqr = 0.0400000028f;
    private CameraController m_CameraController;
    private readonly ValueBinding<bool> m_CameraMovingBinding;
    private readonly EventBinding<bool> m_CameraBarrierBinding;
    private readonly EventBinding<bool> m_ToolBarrierBinding;
    private readonly EventBinding<bool> m_ToolActionPerformedBinding;
    private InputBarrier m_CameraInputBarrier;
    private InputBarrier m_ToolInputBarrier;

    public InputBindings()
    {
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("input", "mouseOverUI", (Func<bool>) (() => InputManager.instance.mouseOverUI)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("input", "hideCursor", (Func<bool>) (() => InputManager.instance.hideCursor)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<int>("input", "controlScheme", (Func<int>) (() => (int) InputManager.instance.activeControlScheme)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("input", "scrollSensitivity", (Func<float>) (() => SharedSettings.instance.input.finalScrollSensitivity)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<Vector2>("input", "gamepadPointerPosition", (Func<Vector2>) (() => InputManager.instance.gamepadPointerPosition)));
      this.AddBinding((IBinding) (this.m_CameraMovingBinding = new ValueBinding<bool>("input", "cameraMoving", false)));
      this.AddBinding((IBinding) (this.m_ToolActionPerformedBinding = new EventBinding<bool>("input", "toolActionPerformed")));
      this.AddBinding((IBinding) (this.m_CameraBarrierBinding = new EventBinding<bool>("input", "cameraBarrier")));
      this.AddBinding((IBinding) (this.m_ToolBarrierBinding = new EventBinding<bool>("input", "toolBarrier")));
      this.AddBinding((IBinding) new TriggerBinding<bool>("input", "onGamepadPointerEvent", new Action<bool>(this.OnGamepadPointerEvent)));
      this.AddBinding((IBinding) new TriggerBinding<int, int, int, int>("input", "setActiveTextFieldRect", new Action<int, int, int, int>(this.SetActiveTextfieldRect)));
      this.AddBinding((IBinding) new GetterValueBinding<bool>("input", "useTextFieldInputBarrier", (Func<bool>) (() => PlatformManager.instance.passThroughVKeyboard)));
      this.m_CameraInputBarrier = InputManager.instance.CreateMapBarrier("Camera", nameof (InputBindings));
      this.m_ToolInputBarrier = InputManager.instance.CreateMapBarrier("Tool", nameof (InputBindings));
      ToolBaseSystem.EventToolActionPerformed += new Action<ProxyAction>(this.OnToolActionPerformed);
    }

    public void Dispose()
    {
      this.m_CameraInputBarrier.Dispose();
      this.m_ToolInputBarrier.Dispose();
      ToolBaseSystem.EventToolActionPerformed -= new Action<ProxyAction>(this.OnToolActionPerformed);
    }

    public override bool Update()
    {
      bool newValue = false;
      if ((UnityEngine.Object) this.m_CameraController != (UnityEngine.Object) null || CameraController.TryGet(out this.m_CameraController))
      {
        foreach (ProxyAction inputAction in this.m_CameraController.inputActions)
        {
          if (inputAction.IsInProgress())
          {
            System.Type valueType = inputAction.valueType;
            if (valueType == typeof (float))
            {
              if ((double) Mathf.Abs(inputAction.ReadRawValue<float>()) >= 0.20000000298023224)
              {
                newValue = true;
                break;
              }
            }
            else if (valueType == typeof (Vector2) && (double) inputAction.ReadRawValue<Vector2>().sqrMagnitude >= 0.040000002831220627)
            {
              newValue = true;
              break;
            }
          }
        }
      }
      this.m_CameraMovingBinding.Update(newValue);
      this.m_CameraInputBarrier.blocked = this.m_CameraBarrierBinding.observerCount > 0;
      this.m_ToolInputBarrier.blocked = this.m_ToolBarrierBinding.observerCount > 0;
      return base.Update();
    }

    private void OnToolActionPerformed(ProxyAction action)
    {
      this.m_ToolActionPerformedBinding.Trigger(true);
    }

    private void OnGamepadPointerEvent(bool pointerOverUI)
    {
      InputManager.instance.mouseOverUI = pointerOverUI;
    }

    private void SetActiveTextfieldRect(int x, int y, int width, int height)
    {
      PlatformManager.instance?.SetActiveTextFieldRect(x, y, width, height);
    }
  }
}
