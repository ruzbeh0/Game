// Decompiled with JetBrains decompiler
// Type: Game.Input.InputConflictResolution
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Game.Input
{
  public class InputConflictResolution : IDisposable
  {
    private bool m_ActionsDirty = true;
    private bool m_ConflictsDirty = true;
    private bool m_UpdateInProgress;
    private List<InputConflictResolution.State> m_SystemActions = new List<InputConflictResolution.State>();
    private List<InputConflictResolution.State> m_UIActions = new List<InputConflictResolution.State>();
    private List<InputConflictResolution.State> m_ModActions = new List<InputConflictResolution.State>();

    public event Action EventActionRefreshed;

    public event Action EventConflictResolved;

    public void Initialize()
    {
      InputManager.instance.EventActionsChanged += new Action(this.OnActionsChanged);
      InputManager.instance.EventPreResolvedActionChanged += new Action(this.OnPreResolvedActionChanged);
      InputManager.instance.EventControlSchemeChanged += new Action<InputManager.ControlScheme>(this.OnControlSchemeChanged);
    }

    public void Dispose()
    {
      InputManager.instance.EventActionsChanged -= new Action(this.OnActionsChanged);
      InputManager.instance.EventPreResolvedActionChanged -= new Action(this.OnPreResolvedActionChanged);
      InputManager.instance.EventControlSchemeChanged -= new Action<InputManager.ControlScheme>(this.OnControlSchemeChanged);
    }

    public void Update()
    {
      this.m_UpdateInProgress = true;
      if (this.m_ActionsDirty)
      {
        this.RefreshActions();
        this.m_ActionsDirty = false;
        Action eventActionRefreshed = this.EventActionRefreshed;
        if (eventActionRefreshed != null)
          eventActionRefreshed();
      }
      if (this.m_ConflictsDirty)
      {
        this.ResolveConflicts();
        this.m_ConflictsDirty = false;
        Action conflictResolved = this.EventConflictResolved;
        if (conflictResolved != null)
          conflictResolved();
      }
      this.m_UpdateInProgress = false;
    }

    private void OnActionsChanged()
    {
      if (this.m_UpdateInProgress)
        return;
      this.m_ActionsDirty = true;
      this.m_ConflictsDirty = true;
    }

    private void OnControlSchemeChanged(InputManager.ControlScheme scheme)
    {
      if (this.m_UpdateInProgress)
        return;
      this.m_ConflictsDirty = true;
    }

    private void OnPreResolvedActionChanged()
    {
      if (this.m_UpdateInProgress)
        return;
      this.m_ConflictsDirty = true;
    }

    private void RefreshActions()
    {
      this.m_SystemActions.Clear();
      this.m_UIActions.Clear();
      this.m_ModActions.Clear();
      foreach (ProxyAction action in InputManager.instance.actions)
      {
        if (!action.isBuiltIn)
          this.m_ModActions.Add(new InputConflictResolution.State(action));
        else if (action.isSystemAction)
          this.m_SystemActions.Add(new InputConflictResolution.State(action));
        else
          this.m_UIActions.Add(new InputConflictResolution.State(action));
      }
    }

    private void ResolveConflicts()
    {
      foreach (InputConflictResolution.State uiAction in this.m_UIActions)
        uiAction.Reset();
      foreach (InputConflictResolution.State modAction in this.m_ModActions)
        modAction.Reset();
      foreach (InputConflictResolution.State systemAction in this.m_SystemActions)
      {
        if (systemAction.enabled)
        {
          foreach (InputConflictResolution.State uiAction in this.m_UIActions)
          {
            if (uiAction.enabled)
              Resolve(systemAction, uiAction);
          }
          foreach (InputConflictResolution.State modAction in this.m_ModActions)
          {
            if (modAction.enabled)
              Resolve(systemAction, modAction);
          }
        }
      }
      foreach (InputConflictResolution.State uiAction in this.m_UIActions)
      {
        if (uiAction.enabled)
        {
          foreach (InputConflictResolution.State modAction in this.m_ModActions)
          {
            if (modAction.enabled)
              Resolve(uiAction, modAction);
          }
        }
      }
      foreach (InputConflictResolution.State uiAction in this.m_UIActions)
        uiAction.Apply();
      foreach (InputConflictResolution.State modAction in this.m_ModActions)
        modAction.Apply();

      static void Resolve(
        InputConflictResolution.State primary,
        InputConflictResolution.State secondary)
      {
        if (!InputManager.HasConflicts(primary.m_Action, secondary.m_Action, new InputManager.DeviceType?(primary.m_Action.preResolvedMask), new InputManager.DeviceType?(secondary.m_Action.preResolvedMask)))
          return;
        secondary.m_HasConflict = true;
      }
    }

    private class State
    {
      public readonly ProxyAction m_Action;
      public bool m_HasConflict;

      public bool enabled => this.m_Action.preResolvedEnable && !this.m_HasConflict;

      public State(ProxyAction action)
      {
        this.m_Action = action;
        this.Reset();
      }

      public void Reset() => this.m_HasConflict = false;

      public void Apply() => this.m_Action.ApplyState(this.enabled, this.m_Action.preResolvedMask);
    }
  }
}
