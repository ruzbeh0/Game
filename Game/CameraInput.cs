// Decompiled with JetBrains decompiler
// Type: Game.CameraInput
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Input;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game
{
  public class CameraInput : MonoBehaviour
  {
    public float m_MoveSmoothing = 1E-06f;
    public float m_RotateSmoothing = 1E-06f;
    public float m_ZoomSmoothing = 1E-06f;
    private ProxyAction m_MoveAction;
    private ProxyAction m_FastMoveAction;
    private ProxyAction m_RotateAction;
    private ProxyAction m_ZoomAction;

    public Vector2 move { get; private set; }

    public Vector2 rotate { get; private set; }

    public float zoom { get; private set; }

    public void Initialize()
    {
      this.m_MoveAction = InputManager.instance.FindAction("Camera", "Move");
      this.m_FastMoveAction = InputManager.instance.FindAction("Camera", "Move Fast");
      this.m_RotateAction = InputManager.instance.FindAction("Camera", "Rotate");
      this.m_ZoomAction = InputManager.instance.FindAction("Camera", "Zoom");
    }

    public bool isMoving => this.m_MoveAction.IsPressed();

    public bool any
    {
      get
      {
        return this.m_MoveAction.IsPressed() || this.m_FastMoveAction.IsPressed() || this.m_RotateAction.IsPressed() || this.m_ZoomAction.IsPressed();
      }
    }

    public void Refresh()
    {
      this.move = (Vector2) MathUtils.MaxAbs((float2) this.m_MoveAction.ReadValue<Vector2>(), (float2) this.m_FastMoveAction.ReadValue<Vector2>());
      this.rotate = this.m_RotateAction.ReadValue<Vector2>();
      this.zoom = this.m_ZoomAction.ReadValue<float>();
    }
  }
}
