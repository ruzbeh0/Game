// Decompiled with JetBrains decompiler
// Type: Game.Rendering.Utilities.StateMachine
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Rendering.Utilities
{
  public class StateMachine
  {
    public Action onStarted;
    public Action onStopped;
    public Action<State> onTransitioned;
    private string m_name;
    private State _currentState;

    public StateMachine(string name = null) => this.m_name = name;

    public string name => this.m_name;

    public bool isStarted => this._currentState != null;

    public virtual void Start(State initialState)
    {
      if (this.isStarted || initialState == null)
        throw new Exception("already started");
      this.TransitionTo(initialState);
      this.onStarted.Fire();
    }

    public virtual void Stop()
    {
      if (!this.isStarted)
        return;
      if (this._currentState != null)
      {
        this._currentState.TransitionOut();
        this._currentState = (State) null;
      }
      this.onStopped.Fire();
    }

    public void Update()
    {
      if (!this.isStarted)
        return;
      State.Result result = this._currentState.Update();
      switch (result.type)
      {
        case State.ResultType.Stop:
          this.Stop();
          break;
        case State.ResultType.Transition:
          this.TransitionTo(result.next);
          break;
      }
    }

    public void LateUpdate()
    {
      if (!this.isStarted)
        return;
      this._currentState.LateUpdate();
    }

    public string GetCurrentStateName()
    {
      if (this._currentState == null)
        return "none";
      return !string.IsNullOrEmpty(this._currentState.Name) ? this._currentState.Name : this._currentState.ToString();
    }

    public bool IsIn<T>() where T : State => this._currentState != null && this._currentState is T;

    private void TransitionTo(State state)
    {
      this._currentState?.TransitionOut();
      this._currentState = state;
      this._currentState.machine = this;
      this._currentState.TransitionIn();
      this.onTransitioned.Fire<State>(this._currentState);
      this.Update();
    }
  }
}
