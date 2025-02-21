// Decompiled with JetBrains decompiler
// Type: Game.Input.UIInputActionPart
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.Input
{
  [Serializable]
  public class UIInputActionPart
  {
    public InputActionReference m_Action;
    public UIBaseInputAction.ProcessAs m_ProcessAs;
    public UIBaseInputAction.Transform m_Transform;
    public InputManager.DeviceType m_Mask = InputManager.DeviceType.All;

    public ProxyAction GetProxyAction() => InputManager.instance.FindAction(this.m_Action.action);

    public bool TryGetProxyAction(out ProxyAction action)
    {
      return InputManager.instance.TryFindAction(this.m_Action.action, out action);
    }
  }
}
