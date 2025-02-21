// Decompiled with JetBrains decompiler
// Type: Game.Input.ProxyAction
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
  public class ProxyAction : IProxyAction
  {
    private static int counter;
    internal readonly int m_GlobalIndex;
    private readonly InputAction m_SourceAction;
    private readonly ProxyActionMap m_Map;
    internal readonly HashSet<InputBarrier> m_Barriers = new HashSet<InputBarrier>();
    internal readonly HashSet<InputActivator> m_Activators = new HashSet<InputActivator>();
    internal readonly HashSet<DisplayNameOverride> m_DisplayOverrides = new HashSet<DisplayNameOverride>();
    internal readonly HashSet<UIBaseInputAction> m_UIAliases = new HashSet<UIBaseInputAction>();
    internal readonly HashSet<ProxyAction.LinkInfo> m_LinkedActions = new HashSet<ProxyAction.LinkInfo>();
    private InputActivator m_DefaultActivator;
    private InputActivator m_DefaultBuiltInActivator;
    internal bool m_PreResolvedEnable;
    private InputManager.DeviceType m_AvailableMask;
    private InputManager.DeviceType m_PreResolvedMask;
    private InputManager.DeviceType m_Mask;
    private bool m_IsSystemAction;
    private readonly Dictionary<InputManager.DeviceType, ProxyComposite> m_Composites = new Dictionary<InputManager.DeviceType, ProxyComposite>();
    private readonly List<ProxyBinding> m_Bindings = new List<ProxyBinding>();
    private Action<ProxyAction, InputActionPhase> m_OnInteraction;
    internal static readonly ProxyAction.DeferActionUpdatingWrapper sDeferUpdatingWrapper = new ProxyAction.DeferActionUpdatingWrapper();
    internal static readonly ProxyAction.DeferActionStateUpdatingWrapper sDeferStateUpdatingWrapper = new ProxyAction.DeferActionStateUpdatingWrapper();

    public event Action<ProxyAction> onChanged;

    public ProxyActionMap map => this.m_Map;

    internal InputAction sourceAction => this.m_SourceAction;

    public IReadOnlyDictionary<InputManager.DeviceType, ProxyComposite> composites
    {
      get => (IReadOnlyDictionary<InputManager.DeviceType, ProxyComposite>) this.m_Composites;
    }

    public int compositesCount => this.m_Composites.Count;

    internal IReadOnlyCollection<InputBarrier> barriers
    {
      get => (IReadOnlyCollection<InputBarrier>) this.m_Barriers;
    }

    internal IReadOnlyCollection<InputActivator> activators
    {
      get => (IReadOnlyCollection<InputActivator>) this.m_Activators;
    }

    public IEnumerable<ProxyBinding> bindings
    {
      get
      {
        foreach (KeyValuePair<InputManager.DeviceType, ProxyComposite> composite in this.m_Composites)
        {
          InputManager.DeviceType deviceType;
          ProxyComposite proxyComposite;
          composite.Deconstruct(ref deviceType, ref proxyComposite);
          foreach (KeyValuePair<ActionComponent, ProxyBinding> binding1 in (IEnumerable<KeyValuePair<ActionComponent, ProxyBinding>>) proxyComposite.bindings)
          {
            ActionComponent actionComponent;
            ProxyBinding binding2;
            binding1.Deconstruct(ref actionComponent, ref binding2);
            yield return binding2;
          }
        }
      }
    }

    public bool isSet
    {
      get
      {
        return this.m_Composites.Any<KeyValuePair<InputManager.DeviceType, ProxyComposite>>((Func<KeyValuePair<InputManager.DeviceType, ProxyComposite>, bool>) (c => c.Value.isSet));
      }
    }

    public bool isBuiltIn
    {
      get
      {
        return this.m_Composites.Any<KeyValuePair<InputManager.DeviceType, ProxyComposite>>((Func<KeyValuePair<InputManager.DeviceType, ProxyComposite>, bool>) (c => c.Value.isBuildIn));
      }
    }

    internal bool isDummy
    {
      get
      {
        return this.m_Composites.Count == 0 || this.m_Composites.Any<KeyValuePair<InputManager.DeviceType, ProxyComposite>>((Func<KeyValuePair<InputManager.DeviceType, ProxyComposite>, bool>) (c => c.Value.isDummy));
      }
    }

    internal bool isSystemAction
    {
      get
      {
        if (!this.isBuiltIn)
          return false;
        return this.m_IsSystemAction || this.m_UIAliases.Count == 0;
      }
    }

    public InputManager.DeviceType availableDevices => this.m_AvailableMask;

    public bool isKeyboardAction => (this.m_AvailableMask & InputManager.DeviceType.Keyboard) != 0;

    public bool isMouseAction => (this.m_AvailableMask & InputManager.DeviceType.Mouse) != 0;

    public bool isGamepadAction => (this.m_AvailableMask & InputManager.DeviceType.Gamepad) != 0;

    public bool isOnlyKeyboardAction => this.m_AvailableMask == InputManager.DeviceType.Keyboard;

    public bool isOnlyMouseAction => this.m_AvailableMask == InputManager.DeviceType.Mouse;

    public bool isOnlyGamepadAction => this.m_AvailableMask == InputManager.DeviceType.Gamepad;

    public bool isMultiDeviceAction => this.compositesCount > 1;

    public string name => this.m_SourceAction.name;

    public string mapName => this.m_Map.name;

    public string title => this.mapName + "/" + this.name;

    public System.Type valueType
    {
      get
      {
        System.Type valueType;
        switch (this.m_SourceAction.expectedControlType)
        {
          case "Dpad":
            valueType = typeof (Vector2);
            break;
          case "Stick":
            valueType = typeof (Vector2);
            break;
          case "Vector2":
            valueType = typeof (Vector2);
            break;
          case "Axis":
            valueType = typeof (float);
            break;
          case "Button":
            valueType = typeof (float);
            break;
          default:
            valueType = typeof (float);
            break;
        }
        return valueType;
      }
    }

    bool IProxyAction.enabled
    {
      get => this.shouldBeEnabled;
      set => this.shouldBeEnabled = value;
    }

    public bool enabled
    {
      get => this.m_SourceAction.enabled;
      internal set
      {
        if (this.m_DefaultBuiltInActivator != null)
        {
          this.m_DefaultBuiltInActivator.enabled = value;
        }
        else
        {
          if (!value)
            return;
          this.m_DefaultBuiltInActivator = new InputActivator(true, "Default built-in (" + this.name + ")", this, enabled: true);
        }
      }
    }

    public bool shouldBeEnabled
    {
      get => this.m_DefaultActivator != null && this.m_DefaultActivator.enabled;
      set
      {
        if (this.isBuiltIn)
          throw new Exception("Built-in actions can not be enabled directly");
        if (this.m_DefaultActivator != null)
        {
          this.m_DefaultActivator.enabled = value;
        }
        else
        {
          if (!value)
            return;
          this.m_DefaultActivator = new InputActivator(false, "Default (" + this.name + ")", this, enabled: true);
        }
      }
    }

    internal bool preResolvedEnable => this.m_PreResolvedEnable;

    public InputManager.DeviceType mask => this.m_Mask;

    internal InputManager.DeviceType preResolvedMask => this.m_PreResolvedMask;

    public DisplayNameOverride displayOverride { get; private set; }

    public IEnumerable<string> usedKeys
    {
      get
      {
        return this.bindings.Where<ProxyBinding>((Func<ProxyBinding, bool>) (b => b.isSet)).Select<ProxyBinding, string>((Func<ProxyBinding, string>) (b => b.path)).Distinct<string>();
      }
    }

    internal ProxyAction(ProxyActionMap map, InputAction sourceAction)
    {
      this.m_GlobalIndex = ProxyAction.counter++;
      this.m_Map = map ?? throw new ArgumentNullException(nameof (map));
      this.m_SourceAction = sourceAction ?? throw new ArgumentNullException(nameof (sourceAction));
      InputManager.instance.actionIndex[this.m_GlobalIndex] = this;
      this.Update();
    }

    public unsafe T ReadValue<T>() where T : struct
    {
      InputActionState state = this.m_SourceAction.GetOrCreateActionMap().m_State;
      if (state == null)
        return default (T);
      InputActionState.TriggerState* triggerStatePtr = state.actionStates + this.m_SourceAction.m_ActionIndexInState;
      return state.ReadValue<T>(triggerStatePtr->bindingIndex, triggerStatePtr->controlIndex);
    }

    public object ReadValueAsObject() => this.m_SourceAction.ReadValueAsObject();

    internal unsafe T ReadRawValue<T>(bool disableAll = true) where T : struct
    {
      InputActionState state = this.m_SourceAction.GetOrCreateActionMap().m_State;
      if (state == null)
        return default (T);
      InputActionState.TriggerState* triggerStatePtr = state.actionStates + this.m_SourceAction.m_ActionIndexInState;
      int bindingIndex = triggerStatePtr->bindingIndex;
      // ISSUE: explicit reference operation
      ref InputActionState.BindingState local = @state.bindingStates[bindingIndex];
      if (local.isPartOfComposite)
      {
        // ISSUE: explicit reference operation
        local = @state.bindingStates[local.compositeOrCompositeBindingIndex];
      }
      int processorCount = local.processorCount;
      try
      {
        if (disableAll)
        {
          local.processorCount = 0;
        }
        else
        {
          for (int index = 0; index < processorCount; ++index)
          {
            if (state.processors[local.processorStartIndex + index] is IDisableableProcessor processor)
              processor.disabled = true;
          }
        }
        return state.ReadValue<T>(triggerStatePtr->bindingIndex, triggerStatePtr->controlIndex);
      }
      finally
      {
        if (disableAll)
        {
          local.processorCount = processorCount;
        }
        else
        {
          for (int index = 0; index < processorCount; ++index)
          {
            if (state.processors[local.processorStartIndex + index] is IDisableableProcessor processor)
              processor.disabled = false;
          }
        }
      }
    }

    public unsafe float GetMagnitude()
    {
      InputActionState state = this.m_SourceAction.GetOrCreateActionMap().m_State;
      if (state != null)
      {
        InputActionState.TriggerState* triggerStatePtr = state.actionStates + this.m_SourceAction.m_ActionIndexInState;
        if (triggerStatePtr->haveMagnitude)
          return triggerStatePtr->magnitude;
      }
      return 0.0f;
    }

    public bool IsPressed() => this.m_SourceAction.IsPressed();

    public bool IsInProgress() => this.m_SourceAction.IsInProgress();

    public bool WasPressedThisFrame() => this.m_SourceAction.WasPressedThisFrame();

    public bool WasReleasedThisFrame() => this.m_SourceAction.WasReleasedThisFrame();

    public bool WasPerformedThisFrame() => this.m_SourceAction.WasPerformedThisFrame();

    public event Action<ProxyAction, InputActionPhase> onInteraction
    {
      add
      {
        if (this.m_OnInteraction == null)
        {
          this.m_OnInteraction += value;
          this.m_SourceAction.started += new Action<InputAction.CallbackContext>(this.SourceOnStarted);
          this.m_SourceAction.performed += new Action<InputAction.CallbackContext>(this.SourceOnPerformed);
          this.m_SourceAction.canceled += new Action<InputAction.CallbackContext>(this.SourceOnCanceled);
        }
        else
          this.m_OnInteraction += value;
      }
      remove
      {
        this.m_OnInteraction -= value;
        if (this.m_OnInteraction != null)
          return;
        this.m_SourceAction.started -= new Action<InputAction.CallbackContext>(this.SourceOnStarted);
        this.m_SourceAction.performed -= new Action<InputAction.CallbackContext>(this.SourceOnPerformed);
        this.m_SourceAction.canceled -= new Action<InputAction.CallbackContext>(this.SourceOnCanceled);
      }
    }

    private void SourceOnStarted(InputAction.CallbackContext context)
    {
      Action<ProxyAction, InputActionPhase> onInteraction = this.m_OnInteraction;
      if (onInteraction == null)
        return;
      onInteraction(this, InputActionPhase.Started);
    }

    private void SourceOnPerformed(InputAction.CallbackContext context)
    {
      Action<ProxyAction, InputActionPhase> onInteraction = this.m_OnInteraction;
      if (onInteraction == null)
        return;
      onInteraction(this, InputActionPhase.Performed);
    }

    private void SourceOnCanceled(InputAction.CallbackContext context)
    {
      Action<ProxyAction, InputActionPhase> onInteraction = this.m_OnInteraction;
      if (onInteraction == null)
        return;
      onInteraction(this, InputActionPhase.Canceled);
    }

    internal void UpdateState(bool ignoreDefer = false)
    {
      if (this.m_SourceAction == null)
        return;
      if (ProxyAction.sDeferStateUpdatingWrapper.isDeferred && !ignoreDefer)
      {
        ProxyAction.sDeferStateUpdatingWrapper.AddToUpdateQueue(this);
      }
      else
      {
        this.m_PreResolvedMask = this.m_Map.enabled ? this.m_AvailableMask & this.map.mask : InputManager.DeviceType.None;
        if (this.m_PreResolvedMask != InputManager.DeviceType.None)
        {
          InputManager.DeviceType deviceType = InputManager.DeviceType.None;
          foreach (InputActivator activator in this.m_Activators)
          {
            if (activator.enabled)
            {
              deviceType |= activator.mask & this.m_PreResolvedMask;
              if ((deviceType & this.m_PreResolvedMask) == this.m_PreResolvedMask)
                break;
            }
          }
          if (deviceType != InputManager.DeviceType.None)
          {
            foreach (InputBarrier barrier in this.m_Barriers)
            {
              if (barrier.blocked)
              {
                deviceType &= ~barrier.mask;
                if (deviceType == InputManager.DeviceType.None)
                  break;
              }
            }
          }
          this.m_PreResolvedMask &= deviceType;
        }
        this.m_PreResolvedEnable = this.m_Map.enabled && this.m_PreResolvedMask != 0;
        if (InputManager.exists && (this.m_PreResolvedEnable != this.enabled || this.m_PreResolvedMask != this.mask))
          InputManager.instance.OnPreResolvedActionChanged();
        if (!this.isSystemAction)
          return;
        this.ApplyState(this.m_PreResolvedEnable, this.m_PreResolvedMask);
      }
    }

    internal void ApplyState(bool newEnable, InputManager.DeviceType newMask)
    {
      int num = newMask != this.m_Mask ? 1 : 0;
      bool flag = newEnable != this.m_SourceAction.enabled;
      if (num != 0)
      {
        this.m_Mask = newMask;
        if (InputManager.exists)
          InputManager.instance.OnActionMasksChanged();
      }
      if (!flag)
        return;
      if (newEnable)
        this.m_SourceAction.Enable();
      else
        this.m_SourceAction.Disable();
      this.UpdateDisplay();
      if (!InputManager.exists)
        return;
      InputManager.instance.OnEnabledActionsChanged();
    }

    internal void UpdateDisplay()
    {
      this.displayOverride = this.enabled ? this.m_DisplayOverrides.Where<DisplayNameOverride>((Func<DisplayNameOverride, bool>) (n => n.active)).OrderBy<DisplayNameOverride, int>((Func<DisplayNameOverride, int>) (n => n.priority)).FirstOrDefault<DisplayNameOverride>() : (DisplayNameOverride) null;
      if (!InputManager.exists)
        return;
      InputManager.instance.OnActionDisplayNamesChanged();
    }

    internal void Update(bool ignoreDefer = false)
    {
      if (ProxyAction.sDeferUpdatingWrapper.isDeferred && !ignoreDefer)
      {
        ProxyAction.sDeferUpdatingWrapper.AddToUpdateQueue(this);
      }
      else
      {
        this.m_Composites.Clear();
        this.m_AvailableMask = InputManager.DeviceType.None;
        foreach (ProxyComposite composite in InputManager.instance.GetComposites(this.sourceAction))
        {
          foreach (ProxyAction.LinkInfo linkedAction in this.m_LinkedActions)
          {
            if (linkedAction.m_Device == composite.m_Device)
              composite.m_LinkedActions.Add(linkedAction.m_Action);
          }
          this.m_Composites[composite.m_Device] = composite;
          this.m_AvailableMask |= composite.m_Device;
        }
        this.m_Bindings.Clear();
        foreach (KeyValuePair<InputManager.DeviceType, ProxyComposite> composite in this.m_Composites)
        {
          InputManager.DeviceType deviceType;
          ProxyComposite proxyComposite;
          composite.Deconstruct(ref deviceType, ref proxyComposite);
          foreach (KeyValuePair<ActionComponent, ProxyBinding> binding in (IEnumerable<KeyValuePair<ActionComponent, ProxyBinding>>) proxyComposite.bindings)
          {
            ActionComponent actionComponent;
            ProxyBinding proxyBinding;
            binding.Deconstruct(ref actionComponent, ref proxyBinding);
            this.m_Bindings.Add(proxyBinding);
          }
        }
        this.m_IsSystemAction = this.mapName == "Splash screen" || this.mapName == "Engagement" || this.mapName == "Camera" || this.mapName == "Tool" || this.mapName == "Editor";
        InputManager.instance.UpdateActionInKeyActionMap(this);
        InputManager.instance.OnActionChanged();
        Action<ProxyAction> onChanged = this.onChanged;
        if (onChanged == null)
          return;
        onChanged(this);
      }
    }

    public bool TryGetComposite(InputManager.DeviceType device, out ProxyComposite composite)
    {
      return this.m_Composites.TryGetValue(device, out composite);
    }

    public bool TryGetBinding(ProxyBinding sampleBinding, out ProxyBinding foundBinding)
    {
      ProxyComposite composite;
      if (this.TryGetComposite(sampleBinding.device, out composite))
        return composite.TryGetBinding(sampleBinding, out foundBinding);
      foundBinding = new ProxyBinding();
      return false;
    }

    public InputBarrier CreateBarrier(string barrierName = null, InputManager.DeviceType barrierMask = InputManager.DeviceType.All)
    {
      return new InputBarrier(barrierName, this, barrierMask);
    }

    public InputActivator CreateActivator(
      string activatorName = null,
      InputManager.DeviceType activatorMask = InputManager.DeviceType.All)
    {
      return new InputActivator(false, activatorName, this, activatorMask);
    }

    public bool ContainsComposite(InputManager.DeviceType device)
    {
      return (this.m_AvailableMask & device) != 0;
    }

    internal static void LinkActions(ProxyAction.LinkInfo action1, ProxyAction.LinkInfo action2)
    {
      ProxyAction.LinkActions(action1, action2, true);
    }

    private static void LinkActions(
      ProxyAction.LinkInfo link1,
      ProxyAction.LinkInfo link2,
      bool addToOther)
    {
      if (link1.m_Action == null || link2.m_Action == null || link1.m_Device != link2.m_Device || link1.m_Action == link2.m_Action)
        return;
      if (addToOther)
      {
        if (!link1.m_Action.m_LinkedActions.Contains(link2))
        {
          foreach (ProxyAction.LinkInfo linkedAction in link1.m_Action.m_LinkedActions)
            ProxyAction.LinkActions(link2, linkedAction, false);
        }
        if (!link2.m_Action.m_LinkedActions.Contains(link1))
        {
          foreach (ProxyAction.LinkInfo linkedAction in link2.m_Action.m_LinkedActions)
            ProxyAction.LinkActions(link1, linkedAction, false);
        }
      }
      link1.m_Action.m_LinkedActions.Add(link2);
      link2.m_Action.m_LinkedActions.Add(link1);
    }

    public override string ToString()
    {
      return this.mapName + "/" + this.name + " ( " + string.Join(" | ", this.bindings.Where<ProxyBinding>((Func<ProxyBinding, bool>) (b => !string.IsNullOrEmpty(b.path))).Select<ProxyBinding, string>((Func<ProxyBinding, string>) (b => string.Format("{0}: {1}{2}", (object) b.component, (object) string.Join("", b.modifiers.Select<ProxyModifier, string>((Func<ProxyModifier, string>) (m => m.m_Path + " + "))), (object) b.path)))) + " )";
    }

    internal static ProxyAction.DeferActionStateUpdatingWrapper DeferStateUpdating()
    {
      ProxyAction.sDeferStateUpdatingWrapper.Acquire();
      return ProxyAction.sDeferStateUpdatingWrapper;
    }

    public struct Info
    {
      public string m_Name;
      public string m_Map;
      public ActionType m_Type;
      public List<ProxyComposite.Info> m_Composites;
    }

    internal struct LinkInfo : IEquatable<ProxyAction.LinkInfo>
    {
      public ProxyAction m_Action;
      public InputManager.DeviceType m_Device;

      public bool Equals(ProxyAction.LinkInfo other)
      {
        return object.Equals((object) this.m_Action, (object) other.m_Action) && this.m_Device == other.m_Device;
      }

      public override bool Equals(object obj)
      {
        return obj is ProxyAction.LinkInfo other && this.Equals(other);
      }

      public override int GetHashCode()
      {
        return HashCode.Combine<ProxyAction, int>(this.m_Action, (int) this.m_Device);
      }
    }

    internal class DeferActionUpdatingWrapper : IDisposable
    {
      private static int sDeferUpdating;
      private static readonly HashSet<ProxyAction> sUpdateQueue = new HashSet<ProxyAction>();

      public bool isDeferred => ProxyAction.DeferActionUpdatingWrapper.sDeferUpdating != 0;

      public void AddToUpdateQueue(ProxyAction action)
      {
        ProxyAction.DeferActionUpdatingWrapper.sUpdateQueue.Add(action);
      }

      public void Acquire() => ++ProxyAction.DeferActionUpdatingWrapper.sDeferUpdating;

      public void Dispose()
      {
        if (ProxyAction.DeferActionUpdatingWrapper.sDeferUpdating > 0)
          --ProxyAction.DeferActionUpdatingWrapper.sDeferUpdating;
        if (ProxyAction.DeferActionUpdatingWrapper.sDeferUpdating != 0)
          return;
        try
        {
          ++ProxyAction.DeferActionUpdatingWrapper.sDeferUpdating;
          while (ProxyAction.DeferActionUpdatingWrapper.sUpdateQueue.Count != 0)
          {
            ProxyAction[] array = ProxyAction.DeferActionUpdatingWrapper.sUpdateQueue.ToArray<ProxyAction>();
            ProxyAction.DeferActionUpdatingWrapper.sUpdateQueue.Clear();
            foreach (ProxyAction proxyAction in array)
              proxyAction.Update(true);
          }
        }
        finally
        {
          --ProxyAction.DeferActionUpdatingWrapper.sDeferUpdating;
        }
      }
    }

    internal class DeferActionStateUpdatingWrapper : IDisposable
    {
      private static int sDeferUpdating;
      private static readonly HashSet<ProxyAction> sUpdateQueue = new HashSet<ProxyAction>();

      public bool isDeferred => ProxyAction.DeferActionStateUpdatingWrapper.sDeferUpdating != 0;

      public void AddToUpdateQueue(ProxyAction action)
      {
        ProxyAction.DeferActionStateUpdatingWrapper.sUpdateQueue.Add(action);
      }

      public void Acquire() => ++ProxyAction.DeferActionStateUpdatingWrapper.sDeferUpdating;

      public void Dispose()
      {
        if (ProxyAction.DeferActionStateUpdatingWrapper.sDeferUpdating > 0)
          --ProxyAction.DeferActionStateUpdatingWrapper.sDeferUpdating;
        if (ProxyAction.DeferActionStateUpdatingWrapper.sDeferUpdating != 0)
          return;
        try
        {
          ++ProxyAction.DeferActionStateUpdatingWrapper.sDeferUpdating;
          while (ProxyAction.DeferActionStateUpdatingWrapper.sUpdateQueue.Count != 0)
          {
            ProxyAction[] array = ProxyAction.DeferActionStateUpdatingWrapper.sUpdateQueue.ToArray<ProxyAction>();
            ProxyAction.DeferActionStateUpdatingWrapper.sUpdateQueue.Clear();
            foreach (ProxyAction proxyAction in array)
              proxyAction.UpdateState(true);
          }
        }
        finally
        {
          --ProxyAction.DeferActionStateUpdatingWrapper.sDeferUpdating;
        }
      }
    }
  }
}
