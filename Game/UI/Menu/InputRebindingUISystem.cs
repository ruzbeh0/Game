// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.InputRebindingUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

#nullable enable
namespace Game.UI.Menu
{
  public class InputRebindingUISystem : UISystemBase
  {
    private const 
    #nullable disable
    string kGroup = "inputRebinding";
    private ValueBinding<ProxyBinding?> m_ActiveRebindingBinding;
    private ValueBinding<InputRebindingUISystem.ConflictInfo?> m_ActiveConflictBinding;
    private InputActionRebindingExtensions.RebindingOperation m_Operation;
    private InputActionRebindingExtensions.RebindingOperation m_ModifierOperation;
    private ProxyBinding? m_ActiveRebinding;
    private Action<ProxyBinding> m_OnSetBinding;
    private ProxyBinding? m_PendingRebinding;
    private Dictionary<string, InputRebindingUISystem.ConflictInfoItem> m_Conflicts = new Dictionary<string, InputRebindingUISystem.ConflictInfoItem>();

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.AddBinding((IBinding) (this.m_ActiveRebindingBinding = new ValueBinding<ProxyBinding?>("inputRebinding", "activeRebinding", new ProxyBinding?(), (IWriter<ProxyBinding?>) new ValueWriter<ProxyBinding>().Nullable<ProxyBinding>())));
      this.AddBinding((IBinding) (this.m_ActiveConflictBinding = new ValueBinding<InputRebindingUISystem.ConflictInfo?>("inputRebinding", "activeConflict", new InputRebindingUISystem.ConflictInfo?(), (IWriter<InputRebindingUISystem.ConflictInfo?>) new ValueWriter<InputRebindingUISystem.ConflictInfo>().Nullable<InputRebindingUISystem.ConflictInfo>())));
      this.AddBinding((IBinding) new TriggerBinding("inputRebinding", "cancelRebinding", new Action(this.Cancel)));
      this.AddBinding((IBinding) new TriggerBinding("inputRebinding", "completeAndSwapConflicts", new Action(this.CompleteAndSwapConflicts)));
      this.AddBinding((IBinding) new TriggerBinding("inputRebinding", "completeAndUnsetConflicts", new Action(this.CompleteAndUnsetConflicts)));
      this.m_Operation = new InputActionRebindingExtensions.RebindingOperation();
      this.m_Operation.OnApplyBinding(new Action<InputActionRebindingExtensions.RebindingOperation, string>(this.OnApplyBinding));
      this.m_Operation.OnComplete(new Action<InputActionRebindingExtensions.RebindingOperation>(this.OnComplete));
      this.m_Operation.OnCancel(new Action<InputActionRebindingExtensions.RebindingOperation>(this.OnCancel));
      this.m_ModifierOperation = new InputActionRebindingExtensions.RebindingOperation();
      this.m_ModifierOperation.OnPotentialMatch(new Action<InputActionRebindingExtensions.RebindingOperation>(this.OnModifierPotentialMatch));
      this.m_ModifierOperation.OnApplyBinding(new Action<InputActionRebindingExtensions.RebindingOperation, string>(this.OnModifierApplyBinding));
      UnityEngine.InputSystem.InputSystem.onDeviceChange += new Action<InputDevice, InputDeviceChange>(this.OnDeviceChange);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      this.m_Operation.Dispose();
      this.m_ModifierOperation.Dispose();
      UnityEngine.InputSystem.InputSystem.onDeviceChange -= new Action<InputDevice, InputDeviceChange>(this.OnDeviceChange);
      base.OnDestroy();
    }

    private void OnDeviceChange(InputDevice changedDevice, InputDeviceChange change)
    {
      if (change != InputDeviceChange.Added && change != InputDeviceChange.Removed || !this.m_ActiveRebinding.HasValue)
        return;
      foreach (InputDevice device in UnityEngine.InputSystem.InputSystem.devices)
      {
        if (device.added)
        {
          ProxyBinding proxyBinding;
          if (device is Keyboard)
          {
            proxyBinding = this.m_ActiveRebinding.Value;
            if (proxyBinding.isKeyboard)
              return;
          }
          if (device is Mouse)
          {
            proxyBinding = this.m_ActiveRebinding.Value;
            if (proxyBinding.isMouse)
              return;
          }
          if (device is Gamepad)
          {
            proxyBinding = this.m_ActiveRebinding.Value;
            if (proxyBinding.isGamepad)
              return;
          }
        }
      }
      this.Cancel();
    }

    public void Start(ProxyBinding binding, Action<ProxyBinding> onSetBinding)
    {
      ProxyBinding? activeRebinding = this.m_ActiveRebinding;
      ProxyBinding proxyBinding1 = binding;
      if ((activeRebinding.HasValue ? (activeRebinding.HasValue ? (activeRebinding.GetValueOrDefault() == proxyBinding1 ? 1 : 0) : 1) : 0) != 0 || onSetBinding == null)
        return;
      this.m_ActiveRebinding = new ProxyBinding?(binding);
      this.m_OnSetBinding = onSetBinding;
      this.m_Conflicts.Clear();
      ProxyBinding proxyBinding2 = this.m_ActiveRebinding.Value;
      if (proxyBinding2.isKeyboard)
      {
        Game.Input.InputManager.instance.blockedControlTypes = Game.Input.InputManager.DeviceType.Keyboard;
      }
      else
      {
        proxyBinding2 = this.m_ActiveRebinding.Value;
        if (proxyBinding2.isMouse)
        {
          Game.Input.InputManager.instance.blockedControlTypes = Game.Input.InputManager.DeviceType.Mouse;
        }
        else
        {
          proxyBinding2 = this.m_ActiveRebinding.Value;
          Game.Input.InputManager.instance.blockedControlTypes = !proxyBinding2.isGamepad ? Game.Input.InputManager.DeviceType.None : Game.Input.InputManager.DeviceType.Gamepad;
        }
      }
      this.m_ActiveRebindingBinding.Update(new ProxyBinding?(binding));
      this.m_ActiveConflictBinding.Update(new InputRebindingUISystem.ConflictInfo?());
      this.m_Operation.Reset().WithMagnitudeHavingToBeGreaterThan(0.6f).OnMatchWaitForAnother(0.1f);
      this.m_ModifierOperation.Reset().WithMagnitudeHavingToBeGreaterThan(0.6f);
      if (binding.isKeyboard)
      {
        this.m_Operation.WithControlsHavingToMatchPath("<Keyboard>/<Key>").WithControlsExcluding("<Keyboard>/leftShift").WithControlsExcluding("<Keyboard>/rightShift").WithControlsExcluding("<Keyboard>/leftCtrl").WithControlsExcluding("<Keyboard>/rightCtrl").WithControlsExcluding("<Keyboard>/leftAlt").WithControlsExcluding("<Keyboard>/rightAlt").WithControlsExcluding("<Keyboard>/capsLock").WithControlsExcluding("<Keyboard>/leftWindows").WithControlsExcluding("<Keyboard>/rightWindow").WithControlsExcluding("<Keyboard>/leftMeta").WithControlsExcluding("<Keyboard>/rightMeta").WithControlsExcluding("<Keyboard>/numLock").WithControlsExcluding("<Keyboard>/printScreen").WithControlsExcluding("<Keyboard>/scrollLock").WithControlsExcluding("<Keyboard>/insert").WithControlsExcluding("<Keyboard>/contextMenu").WithControlsExcluding("<Keyboard>/pause").Start();
        if (!binding.allowModifiers || !binding.isModifiersRebindable)
          return;
        this.m_ModifierOperation.WithControlsHavingToMatchPath("<Keyboard>/shift").WithControlsHavingToMatchPath("<Keyboard>/ctrl").WithControlsHavingToMatchPath("<Keyboard>/alt").Start();
      }
      else if (binding.isMouse)
      {
        this.m_Operation.WithControlsHavingToMatchPath("<Mouse>/<Button>").Start();
        if (!binding.allowModifiers || !binding.isModifiersRebindable)
          return;
        this.m_ModifierOperation.WithControlsHavingToMatchPath("<Keyboard>/shift").WithControlsHavingToMatchPath("<Keyboard>/ctrl").WithControlsHavingToMatchPath("<Keyboard>/alt").Start();
      }
      else
      {
        if (!binding.isGamepad)
          return;
        this.m_Operation.WithControlsHavingToMatchPath("<Gamepad>/<Button>").WithControlsHavingToMatchPath("<Gamepad>/*/<Button>").WithControlsExcluding("<Gamepad>/leftStickPress").WithControlsExcluding("<Gamepad>/rightStickPress").WithControlsExcluding("<DualSenseGamepadHID>/leftTriggerButton").WithControlsExcluding("<DualSenseGamepadHID>/rightTriggerButton").WithControlsExcluding("<DualSenseGamepadHID>/systemButton").WithControlsExcluding("<DualSenseGamepadHID>/micButton").Start();
        if (!binding.allowModifiers || !binding.isModifiersRebindable)
          return;
        this.m_ModifierOperation.WithControlsHavingToMatchPath("<Gamepad>/leftStickPress").WithControlsHavingToMatchPath("<Gamepad>/rightStickPress").Start();
      }
    }

    public void Start(
      ProxyBinding binding,
      ProxyBinding newBinding,
      Action<ProxyBinding> onSetBinding)
    {
      ProxyBinding? activeRebinding = this.m_ActiveRebinding;
      ProxyBinding proxyBinding = binding;
      if ((activeRebinding.HasValue ? (activeRebinding.HasValue ? (activeRebinding.GetValueOrDefault() == proxyBinding ? 1 : 0) : 1) : 0) != 0 || onSetBinding == null)
        return;
      this.m_ActiveRebinding = new ProxyBinding?(binding);
      this.m_OnSetBinding = onSetBinding;
      this.m_ActiveRebindingBinding.Update(new ProxyBinding?(binding));
      this.m_ActiveConflictBinding.Update(new InputRebindingUISystem.ConflictInfo?());
      this.Process(binding, newBinding);
    }

    public void Cancel()
    {
      this.m_Operation.Reset();
      this.Reset();
    }

    private void CompleteAndSwapConflicts()
    {
      if (this.m_PendingRebinding.HasValue)
      {
        using (Game.Input.InputManager.DeferUpdating())
        {
          Game.Input.InputManager.instance.SetBindings(this.m_Conflicts.Values.Where<InputRebindingUISystem.ConflictInfoItem>((Func<InputRebindingUISystem.ConflictInfoItem, bool>) (c => !c.isAlias)).Select<InputRebindingUISystem.ConflictInfoItem, ProxyBinding>((Func<InputRebindingUISystem.ConflictInfoItem, ProxyBinding>) (c => c.resolution)), out List<ProxyBinding> _);
          this.Apply(this.m_PendingRebinding.Value);
        }
      }
      this.Reset();
    }

    private void CompleteAndUnsetConflicts()
    {
      if (this.m_PendingRebinding.HasValue)
      {
        using (Game.Input.InputManager.DeferUpdating())
        {
          Game.Input.InputManager.instance.SetBindings(this.m_Conflicts.Values.Where<InputRebindingUISystem.ConflictInfoItem>((Func<InputRebindingUISystem.ConflictInfoItem, bool>) (c => !c.isAlias)).Select<InputRebindingUISystem.ConflictInfoItem, ProxyBinding>((Func<InputRebindingUISystem.ConflictInfoItem, ProxyBinding>) (c => c.binding.WithPath(string.Empty).WithModifiers((IReadOnlyList<ProxyModifier>) Array.Empty<ProxyModifier>()))), out List<ProxyBinding> _);
          this.Apply(this.m_PendingRebinding.Value);
        }
      }
      this.Reset();
    }

    private void OnApplyBinding(
      InputActionRebindingExtensions.RebindingOperation operation,
      string path)
    {
      if (!this.m_ActiveRebinding.HasValue)
        return;
      Game.Input.InputManager.instance.blockedControlTypes = Game.Input.InputManager.DeviceType.None;
      if (path != null && path.StartsWith("<DualShockGamepad>"))
        path = path.Replace("<DualShockGamepad>", "<Gamepad>");
      ProxyBinding oldBinding = this.m_ActiveRebinding.Value;
      ProxyBinding newBinding = oldBinding.Copy() with
      {
        path = path
      };
      if (newBinding.isModifiersRebindable)
        newBinding.modifiers = (IReadOnlyList<ProxyModifier>) this.m_ModifierOperation.candidates.Where<InputControl>((Func<InputControl, bool>) (c => c.IsPressed())).Select<InputControl, ProxyModifier>((Func<InputControl, ProxyModifier>) (c => new ProxyModifier()
        {
          m_Component = oldBinding.component,
          m_Name = Game.Input.InputManager.GetModifierName(oldBinding.component),
          m_Path = Game.Input.InputManager.GeneratePathForControl(c)
        })).ToList<ProxyModifier>();
      this.m_ModifierOperation.Reset();
      this.Process(oldBinding, newBinding);
    }

    private void OnComplete(
      InputActionRebindingExtensions.RebindingOperation operation)
    {
      Game.Input.InputManager.instance.blockedControlTypes = Game.Input.InputManager.DeviceType.None;
      if (this.m_PendingRebinding.HasValue)
        return;
      this.Reset();
    }

    private void OnCancel(
      InputActionRebindingExtensions.RebindingOperation operation)
    {
      Game.Input.InputManager.instance.blockedControlTypes = Game.Input.InputManager.DeviceType.None;
      this.Reset();
    }

    private void Process(ProxyBinding oldBinding, ProxyBinding newBinding)
    {
      UISystemBase.log.InfoFormat("Rebinding from {0} to {1}", (object) oldBinding, (object) newBinding);
      if (newBinding.action == null)
        this.Reset();
      else if (!NeedAskUser(newBinding))
      {
        this.Apply(newBinding);
        this.Reset();
      }
      else
      {
        this.m_Conflicts.Clear();
        bool unsolved;
        bool batchSwap;
        bool swap;
        bool unset;
        this.GetRebindOptions(this.m_Conflicts, oldBinding, newBinding, out unsolved, out batchSwap, out swap, out unset);
        if (this.m_Conflicts.Count == 0)
        {
          this.Apply(newBinding);
          this.Reset();
        }
        else
        {
          this.m_PendingRebinding = new ProxyBinding?(newBinding);
          this.m_ActiveConflictBinding.Update(new InputRebindingUISystem.ConflictInfo?(new InputRebindingUISystem.ConflictInfo()
          {
            binding = newBinding,
            conflicts = this.m_Conflicts.Values.OrderBy<InputRebindingUISystem.ConflictInfoItem, string>((Func<InputRebindingUISystem.ConflictInfoItem, string>) (b => b.binding.mapName)).ToArray<InputRebindingUISystem.ConflictInfoItem>(),
            unsolved = unsolved,
            swap = swap,
            unset = unset,
            batchSwap = batchSwap
          }));
        }
      }

      static bool NeedAskUser(ProxyBinding binding)
      {
        ProxyAction action = binding.action;
        if (action.m_LinkedActions.Count != 0)
          return true;
        foreach (UIBaseInputAction uiAlias in action.m_UIAliases)
        {
          if (uiAlias.showInOptions)
          {
            foreach (UIInputActionPart actionPart in (IEnumerable<UIInputActionPart>) uiAlias.actionParts)
            {
              if ((actionPart.m_Mask & binding.device) != Game.Input.InputManager.DeviceType.None)
                return true;
            }
          }
          else
            break;
        }
        return binding.hasConflicts != 0;
      }
    }

    private void GetRebindOptions(
      Dictionary<string, InputRebindingUISystem.ConflictInfoItem> conflictInfos,
      ProxyBinding oldBinding,
      ProxyBinding newBinding,
      out bool unsolved,
      out bool batchSwap,
      out bool swap,
      out bool unset)
    {
      unsolved = false;
      batchSwap = false;
      swap = false;
      unset = false;
      Dictionary<ProxyBinding, List<InputRebindingUISystem.BindingPair>> rebindingMap;
      if (!this.CollectLinkedBindings(oldBinding, newBinding, out rebindingMap))
        return;
      ProxyBinding oldBinding2;
      List<InputRebindingUISystem.BindingPair> bindingPairList1;
      foreach (KeyValuePair<ProxyBinding, List<InputRebindingUISystem.BindingPair>> keyValuePair in rebindingMap)
      {
        keyValuePair.Deconstruct(ref oldBinding2, ref bindingPairList1);
        List<InputRebindingUISystem.BindingPair> list = bindingPairList1;
        this.GetRebindOptions(conflictInfos, list);
      }
      unsolved = conflictInfos.Values.Any<InputRebindingUISystem.ConflictInfoItem>((Func<InputRebindingUISystem.ConflictInfoItem, bool>) (c => (c.options & InputRebindingUISystem.Options.Unsolved) != 0));
      swap = conflictInfos.Values.All<InputRebindingUISystem.ConflictInfoItem>((Func<InputRebindingUISystem.ConflictInfoItem, bool>) (c => (c.options & InputRebindingUISystem.Options.Swap) != InputRebindingUISystem.Options.None && (c.options & InputRebindingUISystem.Options.Backward) == InputRebindingUISystem.Options.None));
      unset = conflictInfos.Values.All<InputRebindingUISystem.ConflictInfoItem>((Func<InputRebindingUISystem.ConflictInfoItem, bool>) (c => (c.options & InputRebindingUISystem.Options.Unset) != InputRebindingUISystem.Options.None && (c.options & InputRebindingUISystem.Options.Backward) == InputRebindingUISystem.Options.None));
      batchSwap = conflictInfos.Values.Any<InputRebindingUISystem.ConflictInfoItem>((Func<InputRebindingUISystem.ConflictInfoItem, bool>) (c => (c.options & InputRebindingUISystem.Options.Backward) != 0));
      int count = conflictInfos.Count;
      foreach (KeyValuePair<ProxyBinding, List<InputRebindingUISystem.BindingPair>> keyValuePair in rebindingMap)
      {
        keyValuePair.Deconstruct(ref oldBinding2, ref bindingPairList1);
        List<InputRebindingUISystem.BindingPair> bindingPairList2 = bindingPairList1;
        ProxyBinding newBinding2;
        foreach ((oldBinding2, newBinding2) in bindingPairList2)
        {
          ProxyBinding proxyBinding1 = oldBinding2;
          ProxyBinding proxyBinding2 = newBinding2;
          InputRebindingUISystem.ConflictInfoItem mainInfo = new InputRebindingUISystem.ConflictInfoItem()
          {
            binding = proxyBinding1,
            resolution = proxyBinding2
          };
          if (!conflictInfos.ContainsKey(mainInfo.binding.title))
          {
            this.CollectAliases(conflictInfos, mainInfo);
            if (((rebindingMap.Count > 1 || bindingPairList2.Count > 1 ? 1 : (conflictInfos.Count != count ? 1 : 0)) | (batchSwap ? 1 : 0)) != 0)
              conflictInfos.TryAdd(mainInfo.binding.title, mainInfo);
          }
        }
      }
      batchSwap |= conflictInfos.Count != count;
      swap &= !batchSwap;
      unset &= !batchSwap;
    }

    private void GetRebindOptions(
      Dictionary<string, InputRebindingUISystem.ConflictInfoItem> conflictInfos,
      List<InputRebindingUISystem.BindingPair> list)
    {
      List<InputRebindingUISystem.BindingPair> bindingPairList1 = new List<InputRebindingUISystem.BindingPair>();
      List<InputRebindingUISystem.BindingPair> bindingPairList2 = new List<InputRebindingUISystem.BindingPair>();
      Usages usages = new Usages(0, false);
      Usages usages1 = new Usages(0, false);
      foreach ((ProxyBinding proxyBinding6, ProxyBinding proxyBinding5) in list)
      {
        ProxyBinding proxyBinding3 = proxyBinding6;
        ProxyBinding proxyBinding4 = proxyBinding5;
        this.CollectBindingConflicts(bindingPairList1, proxyBinding3, proxyBinding4);
        this.CollectBindingConflicts(bindingPairList2, proxyBinding4, proxyBinding3);
        usages1 = Usages.Combine(usages1, proxyBinding4.usages);
      }
      foreach ((proxyBinding5, proxyBinding6) in list)
      {
        if (ProxyBinding.PathEquals(proxyBinding5, proxyBinding6))
          this.ProcessConflict(conflictInfos, bindingPairList2, ref usages1, ref usages1, InputRebindingUISystem.Options.None, out bool _);
      }
      bool changed = true;
      while (changed)
      {
        this.ProcessConflict(conflictInfos, bindingPairList2, ref usages1, ref usages, InputRebindingUISystem.Options.Forward, out changed);
        if (!changed)
          break;
        this.ProcessConflict(conflictInfos, bindingPairList1, ref usages, ref usages1, InputRebindingUISystem.Options.Backward, out changed);
        if (!changed)
          break;
      }
    }

    private void CollectBindingConflicts(
      List<InputRebindingUISystem.BindingPair> conflicts,
      ProxyBinding toCheck,
      ProxyBinding resolution)
    {
      HashSet<ProxyAction> proxyActionSet;
      if (!Game.Input.InputManager.instance.keyActionMap.TryGetValue(toCheck.path, out proxyActionSet))
        return;
      ProxyAction action = toCheck.action;
      foreach (ProxyAction action2 in proxyActionSet)
      {
        foreach (KeyValuePair<Game.Input.InputManager.DeviceType, ProxyComposite> composite1 in (IEnumerable<KeyValuePair<Game.Input.InputManager.DeviceType, ProxyComposite>>) action2.composites)
        {
          Game.Input.InputManager.DeviceType deviceType;
          ProxyComposite proxyComposite1;
          composite1.Deconstruct(ref deviceType, ref proxyComposite1);
          ProxyComposite proxyComposite2 = proxyComposite1;
          if (!proxyComposite2.isDummy && proxyComposite2.m_Device == toCheck.device)
          {
            bool flag = Game.Input.InputManager.CanConflict(action, action2, proxyComposite2.m_Device);
            foreach (KeyValuePair<ActionComponent, ProxyBinding> binding in (IEnumerable<KeyValuePair<ActionComponent, ProxyBinding>>) proxyComposite2.bindings)
            {
              ActionComponent actionComponent;
              ProxyBinding proxyBinding1;
              binding.Deconstruct(ref actionComponent, ref proxyBinding1);
              ProxyBinding proxyBinding2 = proxyBinding1;
              if ((flag || !ProxyBinding.componentComparer.Equals(proxyBinding2, toCheck)) && ProxyBinding.PathEquals(proxyBinding2, toCheck) && !proxyBinding2.usages.isNone)
              {
                InputRebindingUISystem.BindingPair bindingPair1 = new InputRebindingUISystem.BindingPair(proxyBinding2, resolution);
                if (!conflicts.Contains(bindingPair1))
                  conflicts.Add(bindingPair1);
                foreach (ProxyAction.LinkInfo linkedAction in action2.m_LinkedActions)
                {
                  ProxyComposite composite2;
                  ProxyBinding foundBinding;
                  if (linkedAction.m_Device == proxyBinding2.device && linkedAction.m_Action.TryGetComposite(proxyBinding2.device, out composite2) && composite2.TryGetBinding(proxyBinding2.component, out foundBinding) && !foundBinding.usages.isNone)
                  {
                    ProxyBinding newBinding = resolution.Copy();
                    if (!foundBinding.isModifiersRebindable)
                      newBinding.modifiers = foundBinding.modifiers;
                    InputRebindingUISystem.BindingPair bindingPair2 = new InputRebindingUISystem.BindingPair(foundBinding, newBinding);
                    if (!conflicts.Contains(bindingPair2))
                      conflicts.Add(bindingPair2);
                  }
                }
              }
            }
          }
        }
      }
    }

    private bool CollectLinkedBindings(
      ProxyBinding oldBinding,
      ProxyBinding newBinding,
      out Dictionary<ProxyBinding, List<InputRebindingUISystem.BindingPair>> rebindingMap)
    {
      rebindingMap = new Dictionary<ProxyBinding, List<InputRebindingUISystem.BindingPair>>((IEqualityComparer<ProxyBinding>) ProxyBinding.pathAndModifiersComparer)
      {
        {
          oldBinding,
          new List<InputRebindingUISystem.BindingPair>()
          {
            new InputRebindingUISystem.BindingPair(oldBinding, newBinding)
          }
        }
      };
      ProxyAction action = oldBinding.action;
      if (action == null)
        return true;
      foreach (ProxyAction.LinkInfo linkedAction in action.m_LinkedActions)
      {
        if (linkedAction.m_Device == oldBinding.device)
        {
          foreach (KeyValuePair<Game.Input.InputManager.DeviceType, ProxyComposite> composite in (IEnumerable<KeyValuePair<Game.Input.InputManager.DeviceType, ProxyComposite>>) linkedAction.m_Action.composites)
          {
            Game.Input.InputManager.DeviceType deviceType;
            ProxyComposite proxyComposite1;
            composite.Deconstruct(ref deviceType, ref proxyComposite1);
            ProxyComposite proxyComposite2 = proxyComposite1;
            if (!proxyComposite2.isDummy)
            {
              foreach (KeyValuePair<ActionComponent, ProxyBinding> binding in (IEnumerable<KeyValuePair<ActionComponent, ProxyBinding>>) proxyComposite2.bindings)
              {
                ActionComponent actionComponent;
                ProxyBinding proxyBinding1;
                binding.Deconstruct(ref actionComponent, ref proxyBinding1);
                ProxyBinding proxyBinding2 = proxyBinding1;
                if (ProxyBinding.componentComparer.Equals(oldBinding, proxyBinding2))
                {
                  if (!proxyBinding2.isRebindable)
                    return false;
                  ProxyBinding newBinding1 = proxyBinding2.Copy() with
                  {
                    path = newBinding.path
                  };
                  if (newBinding1.allowModifiers && newBinding1.isModifiersRebindable)
                    newBinding1.modifiers = newBinding.modifiers;
                  List<InputRebindingUISystem.BindingPair> bindingPairList;
                  if (!rebindingMap.TryGetValue(proxyBinding2, out bindingPairList))
                  {
                    bindingPairList = new List<InputRebindingUISystem.BindingPair>();
                    rebindingMap[proxyBinding2] = bindingPairList;
                  }
                  bindingPairList.Add(new InputRebindingUISystem.BindingPair(proxyBinding2, newBinding1));
                }
              }
            }
          }
        }
      }
      return true;
    }

    private void CollectAliases(
      Dictionary<string, InputRebindingUISystem.ConflictInfoItem> conflictInfos,
      InputRebindingUISystem.ConflictInfoItem mainInfo)
    {
      foreach (UIBaseInputAction uiAlias in mainInfo.binding.action.m_UIAliases)
      {
        if (!((UnityEngine.Object) mainInfo.binding.alies == (UnityEngine.Object) uiAlias) && uiAlias.showInOptions)
        {
          foreach (UIInputActionPart actionPart in (IEnumerable<UIInputActionPart>) uiAlias.actionParts)
          {
            if ((actionPart.m_Transform == UIBaseInputAction.Transform.None || (mainInfo.binding.component.ToTransform() & actionPart.m_Transform) != UIBaseInputAction.Transform.None) && (actionPart.m_Mask & mainInfo.binding.device) != Game.Input.InputManager.DeviceType.None)
            {
              ProxyBinding proxyBinding1 = mainInfo.binding.Copy() with
              {
                alies = uiAlias
              };
              ProxyBinding proxyBinding2 = mainInfo.resolution.Copy() with
              {
                alies = uiAlias
              };
              conflictInfos.TryAdd(proxyBinding1.title, new InputRebindingUISystem.ConflictInfoItem()
              {
                binding = proxyBinding1,
                resolution = proxyBinding2,
                options = mainInfo.options,
                isAlias = true
              });
            }
          }
        }
      }
      if (!mainInfo.binding.isAlias)
        return;
      ProxyBinding proxyBinding3 = mainInfo.binding.Copy() with
      {
        alies = (UIBaseInputAction) null
      };
      ProxyBinding proxyBinding4 = mainInfo.resolution.Copy() with
      {
        alies = (UIBaseInputAction) null
      };
      conflictInfos.TryAdd(proxyBinding3.title, new InputRebindingUISystem.ConflictInfoItem()
      {
        binding = proxyBinding3,
        resolution = proxyBinding4,
        options = mainInfo.options,
        isAlias = true
      });
    }

    private void ProcessConflict(
      Dictionary<string, InputRebindingUISystem.ConflictInfoItem> conflictInfos,
      List<InputRebindingUISystem.BindingPair> bindingConflicts,
      ref Usages usages,
      ref Usages otherUsages,
      InputRebindingUISystem.Options direction,
      out bool changed)
    {
      changed = false;
      for (int index = 0; index < bindingConflicts.Count; ++index)
      {
        (ProxyBinding proxyBinding1, ProxyBinding proxyBinding2) = bindingConflicts[index];
        if (Usages.TestAny(usages, proxyBinding1.usages))
        {
          bool flag = InputRebindingUISystem.CanSwap(proxyBinding2, proxyBinding1, direction == InputRebindingUISystem.Options.Backward);
          bool canBeEmpty = proxyBinding1.canBeEmpty;
          InputRebindingUISystem.ConflictInfoItem info;
          if (!flag && !canBeEmpty)
          {
            info = new InputRebindingUISystem.ConflictInfoItem();
            info.binding = proxyBinding1.Copy();
            info.resolution = proxyBinding1.Copy();
            info.options = direction | InputRebindingUISystem.Options.Unsolved;
            AddToConflictInfos(info);
            changed = true;
          }
          else if (flag)
          {
            ProxyBinding proxyBinding3 = proxyBinding1.Copy() with
            {
              path = proxyBinding2.path
            };
            if (proxyBinding1.allowModifiers && proxyBinding1.isModifiersRebindable)
              proxyBinding3.modifiers = proxyBinding2.modifiers;
            info = new InputRebindingUISystem.ConflictInfoItem();
            info.binding = proxyBinding1.Copy();
            info.resolution = proxyBinding3;
            info.options = direction | InputRebindingUISystem.Options.Swap;
            AddToConflictInfos(info);
            changed = true;
            otherUsages = Usages.Combine(otherUsages, proxyBinding1.usages);
            foreach (ProxyAction.LinkInfo linkedAction in proxyBinding1.action.m_LinkedActions)
            {
              ProxyComposite composite;
              ProxyBinding foundBinding;
              if (linkedAction.m_Device == proxyBinding2.device && linkedAction.m_Action.TryGetComposite(proxyBinding2.device, out composite) && composite.TryGetBinding(proxyBinding2.component, out foundBinding))
                usages = Usages.Combine(usages, foundBinding.usages);
            }
          }
          else if (canBeEmpty)
          {
            ProxyBinding proxyBinding4 = proxyBinding1.Copy() with
            {
              path = string.Empty,
              modifiers = (IReadOnlyList<ProxyModifier>) Array.Empty<ProxyModifier>()
            };
            info = new InputRebindingUISystem.ConflictInfoItem();
            info.binding = proxyBinding1.Copy();
            info.resolution = proxyBinding4;
            info.options = direction | InputRebindingUISystem.Options.Unset;
            AddToConflictInfos(info);
            changed = true;
          }
          bindingConflicts.RemoveAt(index);
          --index;
        }
      }

      void AddToConflictInfos(InputRebindingUISystem.ConflictInfoItem info)
      {
        if (!conflictInfos.TryAdd(info.binding.title, info))
          return;
        this.CollectAliases(conflictInfos, info);
      }
    }

    private static bool CanSwap(ProxyBinding x, ProxyBinding y, bool checkUsage)
    {
      return x.isSet && y.isSet && x.isRebindable && y.isRebindable && !ProxyBinding.PathEquals(x, y) && (!checkUsage || !Usages.TestAny(x.usages, y.usages)) && (ProxyBinding.defaultModifiersComparer.Equals((IReadOnlyCollection<ProxyModifier>) x.modifiers, (IReadOnlyCollection<ProxyModifier>) y.modifiers) || x.allowModifiers && y.allowModifiers && x.isModifiersRebindable && y.isModifiersRebindable);
    }

    private void Apply(ProxyBinding newBinding)
    {
      using (Game.Input.InputManager.DeferUpdating())
      {
        Action<ProxyBinding> onSetBinding = this.m_OnSetBinding;
        if (onSetBinding == null)
          return;
        onSetBinding(newBinding);
      }
    }

    private void Reset()
    {
      this.m_ActiveRebindingBinding.Update(new ProxyBinding?());
      this.m_ActiveConflictBinding.Update(new InputRebindingUISystem.ConflictInfo?());
      this.m_ModifierOperation.Reset();
      this.m_ActiveRebinding = new ProxyBinding?();
      this.m_OnSetBinding = (Action<ProxyBinding>) null;
      this.m_PendingRebinding = new ProxyBinding?();
      this.m_Conflicts.Clear();
    }

    private void OnModifierPotentialMatch(
      InputActionRebindingExtensions.RebindingOperation operation)
    {
    }

    private void OnModifierApplyBinding(
      InputActionRebindingExtensions.RebindingOperation operation,
      string path)
    {
    }

    [Preserve]
    public InputRebindingUISystem()
    {
    }

    [Flags]
    private enum Options
    {
      None = 0,
      Unsolved = 1,
      Swap = 2,
      Unset = 4,
      Forward = 8,
      Backward = 16, // 0x00000010
    }

    private record BindingPair(ProxyBinding oldBinding, ProxyBinding newBinding);

    private struct ConflictInfo : IJsonWritable
    {
      public ProxyBinding binding;
      public InputRebindingUISystem.ConflictInfoItem[] conflicts;

      public bool unsolved { get; set; }

      public bool swap { get; set; }

      public bool unset { get; set; }

      public bool batchSwap { get; set; }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(typeof (InputRebindingUISystem.ConflictInfo).FullName);
        writer.PropertyName("binding");
        writer.Write<ProxyBinding>(this.binding);
        writer.PropertyName("conflicts");
        writer.Write<InputRebindingUISystem.ConflictInfoItem>((IList<InputRebindingUISystem.ConflictInfoItem>) ((IEnumerable<InputRebindingUISystem.ConflictInfoItem>) this.conflicts).Where<InputRebindingUISystem.ConflictInfoItem>((Func<InputRebindingUISystem.ConflictInfoItem, bool>) (c => !c.isHidden)).ToArray<InputRebindingUISystem.ConflictInfoItem>());
        writer.PropertyName("unsolved");
        writer.Write(this.unsolved);
        writer.PropertyName("swap");
        writer.Write(this.swap);
        writer.PropertyName("unset");
        writer.Write(this.unset);
        writer.PropertyName("batchSwap");
        writer.Write(this.batchSwap);
        writer.TypeEnd();
      }
    }

    private struct ConflictInfoItem : IJsonWritable
    {
      public ProxyBinding binding;
      public ProxyBinding resolution;
      public InputRebindingUISystem.Options options;
      public bool isAlias;
      public bool isHidden;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(typeof (InputRebindingUISystem.ConflictInfoItem).FullName);
        writer.PropertyName("binding");
        writer.Write<ProxyBinding>(this.binding);
        writer.PropertyName("resolution");
        writer.Write<ProxyBinding>(this.resolution);
        writer.PropertyName("isHidden");
        writer.Write(this.isHidden);
        writer.TypeEnd();
      }
    }
  }
}
