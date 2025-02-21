// Decompiled with JetBrains decompiler
// Type: Game.CinemachineGameAxisProvider
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Cinemachine;
using Game.Input;
using UnityEngine;

#nullable disable
namespace Game
{
  public class CinemachineGameAxisProvider : MonoBehaviour, AxisState.IInputAxisProvider
  {
    private ProxyAction m_RotateAction;
    private ProxyAction m_ZoomAction;

    private void Awake()
    {
      this.m_RotateAction = InputManager.instance.FindAction("Camera", "Rotate");
      this.m_ZoomAction = InputManager.instance.FindAction("Camera", "Zoom");
    }

    public float GetAxisValue(int axis)
    {
      switch (axis)
      {
        case 0:
          return this.m_RotateAction.ReadRawValue<Vector2>(false).x;
        case 1:
          return this.m_RotateAction.ReadRawValue<Vector2>(false).y;
        case 2:
          return this.m_ZoomAction.ReadRawValue<float>(false);
        default:
          return 0.0f;
      }
    }
  }
}
