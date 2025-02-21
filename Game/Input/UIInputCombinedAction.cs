// Decompiled with JetBrains decompiler
// Type: Game.Input.UIInputCombinedAction
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.Input
{
  [CreateAssetMenu(menuName = "Colossal/UI/UIInputCombinedAction")]
  public class UIInputCombinedAction : UIBaseInputAction
  {
    public UIInputActionPart[] m_Parts;

    public override IProxyAction GetState(string source)
    {
      UIInputAction.State[] stateArray = new UIInputAction.State[this.m_Parts.Length];
      for (int index = 0; index < this.m_Parts.Length; ++index)
        stateArray[index] = new UIInputAction.State(source, this.m_Parts[index].GetProxyAction(), this.GetDisplayName(this.m_Parts[index], source), this.m_Parts[index].m_Mask);
      return (IProxyAction) new UIInputCombinedAction.State(stateArray);
    }

    public override IProxyAction GetState(
      string source,
      UIBaseInputAction.DisplayGetter displayNameGetter)
    {
      if (this.m_Parts.Length == 1)
      {
        ProxyAction proxyAction = this.m_Parts[0].GetProxyAction();
        DisplayNameOverride displayName = displayNameGetter(source, proxyAction, this.m_Parts[0].m_Mask, this.m_Parts[0].m_Transform);
        return (IProxyAction) new UIInputAction.State(source, proxyAction, displayName, this.m_Parts[0].m_Mask);
      }
      UIInputAction.State[] stateArray = new UIInputAction.State[this.m_Parts.Length];
      for (int index = 0; index < this.m_Parts.Length; ++index)
      {
        ProxyAction proxyAction = this.m_Parts[index].GetProxyAction();
        DisplayNameOverride displayName = displayNameGetter(source, proxyAction, this.m_Parts[0].m_Mask, this.m_Parts[index].m_Transform);
        stateArray[index] = new UIInputAction.State(source, proxyAction, displayName, this.m_Parts[index].m_Mask);
      }
      return (IProxyAction) new UIInputCombinedAction.State(stateArray);
    }

    public override IReadOnlyList<UIInputActionPart> actionParts
    {
      get => (IReadOnlyList<UIInputActionPart>) this.m_Parts;
    }

    public class State : IProxyAction, UIBaseInputAction.IState
    {
      private readonly UIInputAction.State[] m_States;

      public event Action<ProxyAction, InputActionPhase> onInteraction
      {
        add
        {
          foreach (UIInputAction.State state in this.m_States)
            state.onInteraction += value;
        }
        remove
        {
          foreach (UIInputAction.State state in this.m_States)
            state.onInteraction -= value;
        }
      }

      public IReadOnlyList<ProxyAction> actions
      {
        get
        {
          return (IReadOnlyList<ProxyAction>) ((IEnumerable<UIInputAction.State>) this.m_States).Select<UIInputAction.State, ProxyAction>((Func<UIInputAction.State, ProxyAction>) (s => s.action)).ToArray<ProxyAction>();
        }
      }

      public bool enabled
      {
        get
        {
          return ((IEnumerable<UIInputAction.State>) this.m_States).Any<UIInputAction.State>((Func<UIInputAction.State, bool>) (a => a.enabled));
        }
        set
        {
          foreach (UIInputAction.State state in this.m_States)
            state.enabled = value;
        }
      }

      bool IProxyAction.enabled
      {
        get => this.enabled;
        set => this.enabled = value;
      }

      public State(params UIInputAction.State[] states) => this.m_States = states;

      public bool WasPressedThisFrame()
      {
        return ((IEnumerable<UIInputAction.State>) this.m_States).Any<UIInputAction.State>((Func<UIInputAction.State, bool>) (s => s.WasPressedThisFrame()));
      }

      public bool WasReleasedThisFrame()
      {
        return ((IEnumerable<UIInputAction.State>) this.m_States).Any<UIInputAction.State>((Func<UIInputAction.State, bool>) (s => s.WasReleasedThisFrame()));
      }

      public bool IsPressed()
      {
        return ((IEnumerable<UIInputAction.State>) this.m_States).Any<UIInputAction.State>((Func<UIInputAction.State, bool>) (s => s.IsPressed()));
      }

      public float GetMagnitude()
      {
        float a = 0.0f;
        foreach (UIInputAction.State state in this.m_States)
          a = Mathf.Max(a, state.GetMagnitude());
        return a;
      }

      public T ReadValue<T>() where T : struct
      {
        if (this.m_States.Length == 0)
          return default (T);
        int index1 = 0;
        float num = 0.0f;
        for (int index2 = 0; index2 < this.m_States.Length; ++index2)
        {
          float magnitude = this.m_States[index2].GetMagnitude();
          if ((double) magnitude > (double) num)
          {
            num = magnitude;
            index1 = index2;
          }
        }
        return this.m_States[index1].ReadValue<T>();
      }

      public void Dispose()
      {
        foreach (UIInputAction.State state in this.m_States)
          state.Dispose();
      }
    }
  }
}
