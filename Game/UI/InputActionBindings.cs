// Decompiled with JetBrains decompiler
// Type: Game.UI.InputActionBindings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.UI
{
  public class InputActionBindings : CompositeBinding, IDisposable
  {
    private const int kDisabledPriority = -1;
    private const string kGroup = "input";
    private RawEventBinding m_ActionPerformedBinding;
    private RawEventBinding m_ActionReleasedBinding;
    private EventBinding m_ActionRefreshedBinding;
    private readonly List<InputActionBindings.ActionState> m_UIActionStates = new List<InputActionBindings.ActionState>();
    private readonly Dictionary<(ProxyAction, UIBaseInputAction.ProcessAs), InputActionBindings.IEventTrigger> m_Triggers = new Dictionary<(ProxyAction, UIBaseInputAction.ProcessAs), InputActionBindings.IEventTrigger>();
    private readonly Dictionary<string, int> m_ActionOrder = new Dictionary<string, int>();
    private bool m_ActionsDirty = true;
    private bool m_ConflictsDirty = true;
    private bool m_UpdateInProgress;

    public InputActionBindings()
    {
      this.AddBinding((IBinding) (this.m_ActionPerformedBinding = new RawEventBinding("input", "onActionPerformed")));
      this.AddBinding((IBinding) (this.m_ActionReleasedBinding = new RawEventBinding("input", "onActionReleased")));
      this.AddBinding((IBinding) new TriggerBinding<string, int>("input", "setActionPriority", new Action<string, int>(this.SetActionPriority)));
      this.AddBinding((IBinding) new ValueBinding<string[]>("input", "actionNames", ((IEnumerable<UIBaseInputAction>) Game.Input.InputManager.instance.uiActionCollection.m_InputActions).Select<UIBaseInputAction, string>((Func<UIBaseInputAction, string>) (a => a.aliasName)).ToArray<string>(), (IWriter<string[]>) new ArrayWriter<string>()));
      this.AddBinding((IBinding) (this.m_ActionRefreshedBinding = new EventBinding("input", "onActionsRefreshed")));
      Game.Input.InputManager.instance.EventActionsChanged += new Action(this.OnActionsChanged);
      Game.Input.InputManager.instance.EventControlSchemeChanged += new Action<Game.Input.InputManager.ControlScheme>(this.OnControlSchemeChanged);
    }

    public void Dispose()
    {
      Game.Input.InputManager.instance.EventActionsChanged -= new Action(this.OnActionsChanged);
      Game.Input.InputManager.instance.EventControlSchemeChanged -= new Action<Game.Input.InputManager.ControlScheme>(this.OnControlSchemeChanged);
      foreach (InputActionBindings.ActionState uiActionState in this.m_UIActionStates)
        uiActionState.Dispose();
      foreach (IDisposable disposable in this.m_Triggers.Values)
        disposable.Dispose();
      this.m_Triggers.Clear();
    }

    private void SetActionPriority(string action, int priority)
    {
      int index;
      if (!this.m_ActionOrder.TryGetValue(action, out index))
        return;
      for (; index < this.m_UIActionStates.Count && this.m_UIActionStates[index].name == action; ++index)
        this.m_UIActionStates[index].priority = priority;
    }

    private void SetConflictsDirty()
    {
      if (this.m_UpdateInProgress)
        return;
      this.m_ConflictsDirty = true;
    }

    private void OnActionsChanged()
    {
      if (this.m_UpdateInProgress)
        return;
      this.m_ActionsDirty = true;
      this.m_ConflictsDirty = true;
    }

    private void OnControlSchemeChanged(Game.Input.InputManager.ControlScheme scheme)
    {
      if (this.m_UpdateInProgress)
        return;
      this.m_ConflictsDirty = true;
    }

    public override bool Update()
    {
      this.m_UpdateInProgress = true;
      if (this.m_ActionsDirty)
      {
        this.RefreshActions();
        this.m_ActionsDirty = false;
        this.m_ActionRefreshedBinding.Trigger();
      }
      if (this.m_ConflictsDirty)
      {
        this.ResolveConflicts();
        this.m_ConflictsDirty = false;
      }
      this.m_UpdateInProgress = false;
      return base.Update();
    }

    private void RefreshActions()
    {
      for (int index = 0; index < this.m_UIActionStates.Count; ++index)
      {
        this.m_UIActionStates[index].Dispose();
        InputActionBindings.IEventTrigger eventTrigger;
        if (this.m_Triggers.TryGetValue((this.m_UIActionStates[index].action, this.m_UIActionStates[index].processAs), out eventTrigger))
        {
          eventTrigger.states.Remove(this.m_UIActionStates[index]);
          if (eventTrigger.states.Count == 0)
          {
            eventTrigger.Dispose();
            this.m_Triggers.Remove((this.m_UIActionStates[index].action, this.m_UIActionStates[index].processAs));
          }
        }
      }
      this.m_UIActionStates.Clear();
      this.m_ActionOrder.Clear();
      for (int index1 = 0; index1 < Game.Input.InputManager.instance.uiActionCollection.m_InputActions.Length; ++index1)
      {
        UIBaseInputAction inputAction = Game.Input.InputManager.instance.uiActionCollection.m_InputActions[index1];
        int count = this.m_UIActionStates.Count;
        for (int index2 = 0; index2 < inputAction.actionParts.Count; ++index2)
        {
          UIInputActionPart actionPart = inputAction.actionParts[index2];
          ProxyAction proxyAction = actionPart.GetProxyAction();
          if (proxyAction.isSet)
          {
            DisplayNameOverride displayName = inputAction.GetDisplayName(actionPart, nameof (InputActionBindings));
            InputActionBindings.ActionState actionState = new InputActionBindings.ActionState(proxyAction, inputAction.aliasName, displayName, actionPart.m_ProcessAs, actionPart.m_Transform, actionPart.m_Mask);
            actionState.onChanged += new Action(this.SetConflictsDirty);
            InputActionBindings.IEventTrigger trigger;
            if (!this.m_Triggers.TryGetValue((actionState.action, actionState.processAs), out trigger))
            {
              trigger = InputActionBindings.IEventTrigger.GetTrigger(this, actionState.action, actionState.processAs);
              this.m_Triggers.Add((actionState.action, actionState.processAs), trigger);
            }
            trigger.states.Add(actionState);
            this.m_UIActionStates.Add(actionState);
          }
        }
        if (count != this.m_UIActionStates.Count)
          this.m_ActionOrder[inputAction.aliasName] = count;
      }
    }

    private void ResolveConflicts()
    {
      InputActionBindings.ActionState[] array = this.m_UIActionStates.OrderBy<InputActionBindings.ActionState, InputActionBindings.ActionState>((Func<InputActionBindings.ActionState, InputActionBindings.ActionState>) (a => a)).ToArray<InputActionBindings.ActionState>();
      Game.Input.InputManager.DeviceType mask = Game.Input.InputManager.instance.mask;
      for (int index = 0; index < this.m_UIActionStates.Count; ++index)
        this.m_UIActionStates[index].UpdateState();
      for (int index1 = 0; index1 < array.Length; ++index1)
      {
        InputActionBindings.ActionState actionState1 = array[index1];
        if (actionState1.state == InputActionBindings.ActionState.State.Enabled)
        {
          for (int index2 = index1 + 1; index2 < array.Length; ++index2)
          {
            InputActionBindings.ActionState actionState2 = array[index2];
            if (actionState2.state == InputActionBindings.ActionState.State.Enabled)
            {
              if (actionState2.action == actionState1.action)
              {
                if (actionState2.transform == actionState1.transform || (actionState2.transform & actionState1.transform) != UIBaseInputAction.Transform.None)
                  actionState2.state = InputActionBindings.ActionState.State.DisabledDuplicate;
              }
              else if (Game.Input.InputManager.HasConflicts(actionState2.action, actionState1.action, new Game.Input.InputManager.DeviceType?(actionState1.mask & mask), new Game.Input.InputManager.DeviceType?(actionState2.mask & mask)))
                actionState2.state = InputActionBindings.ActionState.State.DisabledConflict;
            }
          }
        }
      }
      using (ProxyAction.DeferStateUpdating())
      {
        for (int index = 0; index < this.m_UIActionStates.Count; ++index)
          this.m_UIActionStates[index].Apply();
      }
    }

    private class ActionState : IComparable<InputActionBindings.ActionState>, IDisposable
    {
      private bool m_Disposed;
      private readonly string m_Name;
      private readonly ProxyAction m_Action;
      private readonly UIBaseInputAction.ProcessAs m_ProcessAs;
      private readonly InputActivator m_Activator;
      private readonly DisplayNameOverride m_NameOverride;
      private readonly Game.Input.InputManager.DeviceType m_Mask;
      private int m_Priority;
      private InputActionBindings.ActionState.State m_State;
      private readonly UIBaseInputAction.Transform m_Transform;

      public event Action onChanged;

      public bool isDisposed => this.m_Disposed;

      public ProxyAction action => this.m_Action;

      public string name => this.m_Name;

      public Game.Input.InputManager.DeviceType mask => this.m_Mask;

      public int priority
      {
        get => this.m_Priority;
        set
        {
          if (this.m_Disposed || value == this.m_Priority)
            return;
          this.m_Priority = value;
          Action onChanged = this.onChanged;
          if (onChanged == null)
            return;
          onChanged();
        }
      }

      public InputActionBindings.ActionState.State state
      {
        get => this.m_State;
        set
        {
          if (this.m_Disposed || value == this.m_State)
            return;
          this.m_State = value;
          Action onChanged = this.onChanged;
          if (onChanged == null)
            return;
          onChanged();
        }
      }

      public UIBaseInputAction.Transform transform => this.m_Transform;

      public UIBaseInputAction.ProcessAs processAs => this.m_ProcessAs;

      public ActionState(
        ProxyAction action,
        string name,
        DisplayNameOverride displayOverride,
        UIBaseInputAction.ProcessAs processAs = UIBaseInputAction.ProcessAs.AutoDetect,
        UIBaseInputAction.Transform transform = UIBaseInputAction.Transform.None,
        Game.Input.InputManager.DeviceType mask = Game.Input.InputManager.DeviceType.All)
      {
        this.m_Action = action ?? throw new ArgumentNullException(nameof (action));
        this.m_Name = name ?? throw new ArgumentNullException(nameof (name));
        this.m_Mask = mask;
        this.m_Activator = new InputActivator(true, this.m_Name, action, mask);
        this.m_Priority = -1;
        this.m_NameOverride = displayOverride;
        this.m_ProcessAs = processAs;
        this.m_Transform = transform;
        this.UpdateState();
      }

      public void Dispose()
      {
        if (this.m_Disposed)
          return;
        this.m_Disposed = true;
        this.m_Activator.Dispose();
        this.m_NameOverride.Dispose();
        this.onChanged = (Action) null;
      }

      public void UpdateState()
      {
        if (!this.m_Action.isSet)
          this.state = InputActionBindings.ActionState.State.DisabledNotSet;
        else if ((this.m_Action.availableDevices & Game.Input.InputManager.instance.mask & this.m_Mask) == Game.Input.InputManager.DeviceType.None)
          this.state = InputActionBindings.ActionState.State.DisabledMaskMismatch;
        else if (this.m_Priority == -1)
          this.state = InputActionBindings.ActionState.State.DisabledNoConsumer;
        else
          this.state = InputActionBindings.ActionState.State.Enabled;
      }

      public int CompareTo(InputActionBindings.ActionState other)
      {
        return -this.m_Priority.CompareTo(other.m_Priority);
      }

      public void Apply()
      {
        if (this.m_Disposed)
          return;
        this.m_Activator.enabled = this.state == InputActionBindings.ActionState.State.Enabled;
        this.m_NameOverride.active = this.state == InputActionBindings.ActionState.State.Enabled;
      }

      public override string ToString()
      {
        return string.Format("{0} ({1})", (object) this.m_Name, (object) this.m_Action);
      }

      public enum State
      {
        Enabled,
        Disabled,
        DisabledNoConsumer,
        DisabledNotSet,
        DisabledMaskMismatch,
        DisabledConflict,
        DisabledDuplicate,
      }
    }

    private interface IEventTrigger : IDisposable
    {
      HashSet<InputActionBindings.ActionState> states { get; }

      static InputActionBindings.IEventTrigger GetTrigger(
        InputActionBindings parent,
        ProxyAction action,
        UIBaseInputAction.ProcessAs processAs)
      {
        switch (action.sourceAction.expectedControlType)
        {
          case "Dpad":
          case "Stick":
          case "Vector2":
            InputActionBindings.IEventTrigger trigger1;
            switch (processAs)
            {
              case UIBaseInputAction.ProcessAs.Button:
                trigger1 = (InputActionBindings.IEventTrigger) new InputActionBindings.Vector2ToButtonEventTrigger(parent, action);
                break;
              case UIBaseInputAction.ProcessAs.Axis:
                trigger1 = (InputActionBindings.IEventTrigger) new InputActionBindings.Vector2ToAxisEventTrigger(parent, action);
                break;
              case UIBaseInputAction.ProcessAs.Vector2:
                trigger1 = (InputActionBindings.IEventTrigger) new InputActionBindings.Vector2EventTrigger(parent, action);
                break;
              default:
                trigger1 = (InputActionBindings.IEventTrigger) new InputActionBindings.Vector2EventTrigger(parent, action);
                break;
            }
            return trigger1;
          case "Axis":
            InputActionBindings.IEventTrigger trigger2;
            switch (processAs)
            {
              case UIBaseInputAction.ProcessAs.Button:
                trigger2 = (InputActionBindings.IEventTrigger) new InputActionBindings.AxisToButtonEventTrigger(parent, action);
                break;
              case UIBaseInputAction.ProcessAs.Axis:
                trigger2 = (InputActionBindings.IEventTrigger) new InputActionBindings.AxisEventTrigger(parent, action);
                break;
              case UIBaseInputAction.ProcessAs.Vector2:
                trigger2 = (InputActionBindings.IEventTrigger) new InputActionBindings.AxisToVector2EventTrigger(parent, action);
                break;
              default:
                trigger2 = (InputActionBindings.IEventTrigger) new InputActionBindings.AxisEventTrigger(parent, action);
                break;
            }
            return trigger2;
          case "Button":
            InputActionBindings.IEventTrigger trigger3;
            switch (processAs)
            {
              case UIBaseInputAction.ProcessAs.Button:
                trigger3 = (InputActionBindings.IEventTrigger) new InputActionBindings.ButtonEventTrigger(parent, action);
                break;
              case UIBaseInputAction.ProcessAs.Axis:
                trigger3 = (InputActionBindings.IEventTrigger) new InputActionBindings.ButtonToAxisEventTrigger(parent, action);
                break;
              case UIBaseInputAction.ProcessAs.Vector2:
                trigger3 = (InputActionBindings.IEventTrigger) new InputActionBindings.ButtonToVector2EventTrigger(parent, action);
                break;
              default:
                trigger3 = (InputActionBindings.IEventTrigger) new InputActionBindings.ButtonEventTrigger(parent, action);
                break;
            }
            return trigger3;
          default:
            return (InputActionBindings.IEventTrigger) new InputActionBindings.DefaultEventTrigger(parent, action);
        }
      }
    }

    private abstract class EventTrigger<TRawValue, TValue> : 
      InputActionBindings.IEventTrigger,
      IDisposable
      where TRawValue : struct
      where TValue : struct
    {
      private bool m_Disposed;
      private readonly InputActionBindings m_Parent;
      private readonly ProxyAction m_Action;
      private readonly IWriter<TValue> m_ValueWriter;

      public HashSet<InputActionBindings.ActionState> states { get; } = new HashSet<InputActionBindings.ActionState>();

      public EventTrigger(
        InputActionBindings parent,
        ProxyAction action,
        IWriter<TValue> valueWriter = null)
      {
        this.m_Parent = parent;
        this.m_Action = action ?? throw new ArgumentNullException(nameof (action));
        this.m_ValueWriter = valueWriter ?? ValueWriters.Create<TValue>();
        this.m_Action.onInteraction += new Action<ProxyAction, InputActionPhase>(this.OnInteraction);
      }

      private void OnInteraction(ProxyAction _, InputActionPhase phase)
      {
        if (this.m_Disposed)
          return;
        bool flag;
        switch (phase)
        {
          case InputActionPhase.Performed:
            flag = true;
            break;
          case InputActionPhase.Canceled:
            flag = this.m_Action.sourceAction.type == InputActionType.PassThrough;
            break;
          default:
            flag = false;
            break;
        }
        if (!flag)
          return;
        TRawValue rawValue = this.m_Action.ReadValue<TRawValue>();
        foreach (InputActionBindings.ActionState state in this.states)
        {
          if (state.state == InputActionBindings.ActionState.State.Enabled)
          {
            switch (phase)
            {
              case InputActionPhase.Performed:
                TValue obj = this.TransformValue(rawValue, state.transform);
                if ((double) this.GetMagnitude(obj) != 0.0)
                {
                  this.TriggerEvent(this.m_Parent.m_ActionPerformedBinding, state.name, obj);
                  continue;
                }
                if (this.m_Action.sourceAction.type == InputActionType.PassThrough)
                {
                  this.TriggerEvent(this.m_Parent.m_ActionReleasedBinding, state.name, obj);
                  continue;
                }
                continue;
              case InputActionPhase.Canceled:
                if (this.m_Action.sourceAction.type == InputActionType.PassThrough)
                {
                  this.TriggerEvent(this.m_Parent.m_ActionReleasedBinding, state.name, default (TValue));
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
      }

      private void TriggerEvent(RawEventBinding binding, string action, TValue value)
      {
        IJsonWriter writer = binding.EventBegin();
        writer.TypeBegin("input.InputActionEvent");
        writer.PropertyName(nameof (action));
        writer.Write(action);
        writer.PropertyName(nameof (value));
        this.m_ValueWriter.Write(writer, value);
        writer.TypeEnd();
        binding.EventEnd();
      }

      protected abstract TValue TransformValue(
        TRawValue value,
        UIBaseInputAction.Transform transform);

      protected abstract float GetMagnitude(TValue value);

      public void Dispose()
      {
        if (this.m_Disposed)
          return;
        this.m_Disposed = true;
        this.states.Clear();
        this.m_Action.onInteraction -= new Action<ProxyAction, InputActionPhase>(this.OnInteraction);
      }
    }

    private class DefaultEventTrigger : InputActionBindings.EventTrigger<float, float>
    {
      public DefaultEventTrigger(
        InputActionBindings parent,
        ProxyAction action,
        IWriter<float> valueWriter = null)
        : base(parent, action, valueWriter)
      {
      }

      protected override float TransformValue(float value, UIBaseInputAction.Transform transform)
      {
        return value;
      }

      protected override float GetMagnitude(float value) => Mathf.Abs(value);
    }

    private class ButtonEventTrigger : InputActionBindings.EventTrigger<float, float>
    {
      public ButtonEventTrigger(
        InputActionBindings parent,
        ProxyAction action,
        IWriter<float> valueWriter = null)
        : base(parent, action, valueWriter)
      {
      }

      protected override float TransformValue(float value, UIBaseInputAction.Transform transform)
      {
        return Mathf.Clamp(value, 0.0f, 1f);
      }

      protected override float GetMagnitude(float value) => Mathf.Abs(value);
    }

    private class AxisEventTrigger : InputActionBindings.EventTrigger<float, float>
    {
      public AxisEventTrigger(
        InputActionBindings parent,
        ProxyAction action,
        IWriter<float> valueWriter = null)
        : base(parent, action, valueWriter)
      {
      }

      protected override float TransformValue(float value, UIBaseInputAction.Transform transform)
      {
        return value;
      }

      protected override float GetMagnitude(float value) => Mathf.Abs(value);
    }

    private class Vector2EventTrigger : InputActionBindings.EventTrigger<Vector2, Vector2>
    {
      public Vector2EventTrigger(
        InputActionBindings parent,
        ProxyAction action,
        IWriter<Vector2> valueWriter = null)
        : base(parent, action, valueWriter)
      {
      }

      protected override Vector2 TransformValue(
        Vector2 value,
        UIBaseInputAction.Transform transform)
      {
        return value;
      }

      protected override float GetMagnitude(Vector2 value) => value.magnitude;
    }

    private class AxisToButtonEventTrigger : InputActionBindings.EventTrigger<float, float>
    {
      public AxisToButtonEventTrigger(
        InputActionBindings parent,
        ProxyAction action,
        IWriter<float> valueWriter = null)
        : base(parent, action, valueWriter)
      {
      }

      protected override float TransformValue(float value, UIBaseInputAction.Transform transform)
      {
        float num;
        switch (transform)
        {
          case UIBaseInputAction.Transform.Negative:
            num = Mathf.Clamp(-value, 0.0f, 1f);
            break;
          case UIBaseInputAction.Transform.Positive:
            num = Mathf.Clamp(value, 0.0f, 1f);
            break;
          default:
            num = Mathf.Abs(value);
            break;
        }
        return num;
      }

      protected override float GetMagnitude(float value) => Mathf.Abs(value);
    }

    private class Vector2ToButtonEventTrigger : InputActionBindings.EventTrigger<Vector2, float>
    {
      public Vector2ToButtonEventTrigger(
        InputActionBindings parent,
        ProxyAction action,
        IWriter<float> valueWriter = null)
        : base(parent, action, valueWriter)
      {
      }

      protected override float TransformValue(Vector2 value, UIBaseInputAction.Transform transform)
      {
        float num;
        switch (transform)
        {
          case UIBaseInputAction.Transform.Down:
            num = Mathf.Clamp(-value.y, 0.0f, 1f);
            break;
          case UIBaseInputAction.Transform.Up:
            num = Mathf.Clamp(value.y, 0.0f, 1f);
            break;
          case UIBaseInputAction.Transform.Vertical:
            num = Mathf.Abs(value.y);
            break;
          case UIBaseInputAction.Transform.Left:
            num = Mathf.Clamp(-value.x, 0.0f, 1f);
            break;
          case UIBaseInputAction.Transform.Right:
            num = Mathf.Clamp(value.x, 0.0f, 1f);
            break;
          case UIBaseInputAction.Transform.Horizontal:
            num = Mathf.Abs(value.x);
            break;
          default:
            num = value.magnitude;
            break;
        }
        return num;
      }

      protected override float GetMagnitude(float value) => Mathf.Abs(value);
    }

    private class ButtonToAxisEventTrigger : InputActionBindings.EventTrigger<float, float>
    {
      public ButtonToAxisEventTrigger(
        InputActionBindings parent,
        ProxyAction action,
        IWriter<float> valueWriter = null)
        : base(parent, action, valueWriter)
      {
      }

      protected override float TransformValue(float value, UIBaseInputAction.Transform transform)
      {
        float num;
        switch (transform)
        {
          case UIBaseInputAction.Transform.Down:
            num = -value;
            break;
          case UIBaseInputAction.Transform.Up:
            num = value;
            break;
          case UIBaseInputAction.Transform.Left:
            num = -value;
            break;
          case UIBaseInputAction.Transform.Negative:
            num = -value;
            break;
          case UIBaseInputAction.Transform.Right:
            num = value;
            break;
          case UIBaseInputAction.Transform.Positive:
            num = value;
            break;
          default:
            num = value;
            break;
        }
        return num;
      }

      protected override float GetMagnitude(float value) => Mathf.Abs(value);
    }

    private class Vector2ToAxisEventTrigger : InputActionBindings.EventTrigger<Vector2, float>
    {
      public Vector2ToAxisEventTrigger(
        InputActionBindings parent,
        ProxyAction action,
        IWriter<float> valueWriter = null)
        : base(parent, action, valueWriter)
      {
      }

      protected override float TransformValue(Vector2 value, UIBaseInputAction.Transform transform)
      {
        float num;
        switch (transform)
        {
          case UIBaseInputAction.Transform.Vertical:
            num = value.y;
            break;
          case UIBaseInputAction.Transform.Horizontal:
            num = value.x;
            break;
          default:
            num = value.magnitude;
            break;
        }
        return num;
      }

      protected override float GetMagnitude(float value) => Mathf.Abs(value);
    }

    private class ButtonToVector2EventTrigger : InputActionBindings.EventTrigger<float, Vector2>
    {
      public ButtonToVector2EventTrigger(
        InputActionBindings parent,
        ProxyAction action,
        IWriter<Vector2> valueWriter = null)
        : base(parent, action, valueWriter)
      {
      }

      protected override Vector2 TransformValue(float value, UIBaseInputAction.Transform transform)
      {
        Vector2 vector2;
        switch (transform)
        {
          case UIBaseInputAction.Transform.Down:
            vector2 = new Vector2(0.0f, -value);
            break;
          case UIBaseInputAction.Transform.Up:
            vector2 = new Vector2(0.0f, value);
            break;
          case UIBaseInputAction.Transform.Left:
            vector2 = new Vector2(-value, 0.0f);
            break;
          case UIBaseInputAction.Transform.Right:
            vector2 = new Vector2(value, 0.0f);
            break;
          default:
            vector2 = new Vector2(value, value);
            break;
        }
        return vector2;
      }

      protected override float GetMagnitude(Vector2 value) => value.magnitude;
    }

    private class AxisToVector2EventTrigger : InputActionBindings.EventTrigger<float, Vector2>
    {
      public AxisToVector2EventTrigger(
        InputActionBindings parent,
        ProxyAction action,
        IWriter<Vector2> valueWriter = null)
        : base(parent, action, valueWriter)
      {
      }

      protected override Vector2 TransformValue(float value, UIBaseInputAction.Transform transform)
      {
        Vector2 vector2;
        switch (transform)
        {
          case UIBaseInputAction.Transform.Vertical:
            vector2 = new Vector2(0.0f, value);
            break;
          case UIBaseInputAction.Transform.Horizontal:
            vector2 = new Vector2(value, 0.0f);
            break;
          default:
            vector2 = Vector2.zero;
            break;
        }
        return vector2;
      }

      protected override float GetMagnitude(Vector2 value) => value.magnitude;
    }
  }
}
