// Decompiled with JetBrains decompiler
// Type: Game.Input.UIInputAction
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.Input
{
  [CreateAssetMenu(menuName = "Colossal/UI/UIInputAction")]
  public class UIInputAction : UIBaseInputAction
  {
    public InputActionReference m_Action;
    public UIBaseInputAction.ProcessAs m_ProcessAs;
    public UIBaseInputAction.Transform m_Transform;
    public InputManager.DeviceType m_Mask = InputManager.DeviceType.All;
    [NonSerialized]
    private UIInputActionPart[] m_ActionParts;

    public override IReadOnlyList<UIInputActionPart> actionParts
    {
      get
      {
        UIInputActionPart[] actionParts = this.m_ActionParts;
        if (actionParts != null)
          return (IReadOnlyList<UIInputActionPart>) actionParts;
        return (IReadOnlyList<UIInputActionPart>) (this.m_ActionParts = new UIInputActionPart[1]
        {
          new UIInputActionPart()
          {
            m_Action = this.m_Action,
            m_ProcessAs = this.m_ProcessAs,
            m_Transform = this.m_Transform,
            m_Mask = this.m_Mask
          }
        });
      }
    }

    public override IProxyAction GetState(string source)
    {
      ProxyAction action = InputManager.instance.FindAction(this.m_Action.action);
      DisplayNameOverride displayName = new DisplayNameOverride(source, action, this.m_AliasName, this.displayPriority, this.m_Transform);
      return (IProxyAction) new UIInputAction.State(source, action, displayName, this.m_Mask);
    }

    public override IProxyAction GetState(
      string source,
      UIBaseInputAction.DisplayGetter displayNameGetter)
    {
      ProxyAction action = InputManager.instance.FindAction(this.m_Action.action);
      DisplayNameOverride displayName = displayNameGetter(source, action, this.m_Mask, this.m_Transform);
      return (IProxyAction) new UIInputAction.State(source, action, displayName, this.m_Mask);
    }

    public class State : IProxyAction, UIBaseInputAction.IState
    {
      private readonly ProxyAction m_Action;
      private readonly InputActivator m_Activator;
      private readonly DisplayNameOverride m_DisplayName;

      public event Action<ProxyAction, InputActionPhase> onInteraction
      {
        add => this.m_Action.onInteraction += value;
        remove => this.m_Action.onInteraction -= value;
      }

      public ProxyAction action => this.m_Action;

      public DisplayNameOverride displayName => this.m_DisplayName;

      internal State(
        string source,
        ProxyAction action,
        DisplayNameOverride displayName,
        InputManager.DeviceType mask)
      {
        this.m_Action = action ?? throw new ArgumentNullException(nameof (action));
        this.m_Activator = new InputActivator(true, source, action, mask);
        this.m_DisplayName = displayName;
      }

      public IReadOnlyList<ProxyAction> actions
      {
        get
        {
          return (IReadOnlyList<ProxyAction>) new ProxyAction[1]
          {
            this.m_Action
          };
        }
      }

      public bool enabled
      {
        get => this.m_Activator.enabled;
        set
        {
          this.m_Activator.enabled = value;
          if (this.m_DisplayName == null)
            return;
          this.m_DisplayName.active = value;
        }
      }

      bool IProxyAction.enabled
      {
        get => this.enabled;
        set => this.enabled = value;
      }

      public bool WasPressedThisFrame() => this.m_Action.WasPressedThisFrame();

      public bool WasReleasedThisFrame() => this.m_Action.WasReleasedThisFrame();

      public bool IsPressed() => this.m_Action.IsPressed();

      public float GetMagnitude() => this.m_Action.GetMagnitude();

      public T ReadValue<T>() where T : struct => this.m_Action.ReadValue<T>();

      public void Dispose()
      {
        this.m_Activator?.Dispose();
        this.m_DisplayName?.Dispose();
      }
    }
  }
}
