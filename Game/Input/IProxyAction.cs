// Decompiled with JetBrains decompiler
// Type: Game.Input.IProxyAction
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.Input
{
  public interface IProxyAction
  {
    event Action<ProxyAction, InputActionPhase> onInteraction;

    bool enabled { get; set; }

    bool WasPressedThisFrame();

    bool WasReleasedThisFrame();

    bool IsPressed();

    float GetMagnitude();

    T ReadValue<T>() where T : struct;
  }
}
