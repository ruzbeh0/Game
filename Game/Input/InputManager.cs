// Decompiled with JetBrains decompiler
// Type: Game.Input.InputManager
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.PSI.Common;
using Game.Modding;
using Game.PSI;
using Game.SceneFlow;
using Game.UI.Localization;
using Game.UI.Menu;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

#nullable disable
namespace Game.Input
{
  public class InputManager : 
    IDisposable,
    IInputActionCollection,
    IEnumerable<InputAction>,
    IEnumerable
  {
    private bool m_NeedUpdate;
    private static System.Collections.Generic.Dictionary<string, InputManager.CompositeData> s_Composites = new System.Collections.Generic.Dictionary<string, InputManager.CompositeData>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private static InputControlLayout.Cache m_LayoutCache;
    private static StringBuilder m_PathBuilder;
    public const string kShiftName = "<Keyboard>/shift";
    public const string kCtrlName = "<Keyboard>/ctrl";
    public const string kAltName = "<Keyboard>/alt";
    public const string kLeftStick = "<Gamepad>/leftStickPress";
    public const string kRightStick = "<Gamepad>/rightStickPress";
    public const string kSplashScreenMap = "Splash screen";
    public const string kNavigationMap = "Navigation";
    public const string kMenuMap = "Menu";
    public const string kCameraMap = "Camera";
    public const string kToolMap = "Tool";
    public const string kShortcutsMap = "Shortcuts";
    public const string kPhotoModeMap = "Photo mode";
    public const string kEditorMap = "Editor";
    public const string kDebugMap = "Debug";
    public const string kEngagementMap = "Engagement";
    public const int kIdleDelay = 30;
    private static System.Collections.Generic.Dictionary<InputManager.DeviceType, HashSet<string>> kModifiers = new System.Collections.Generic.Dictionary<InputManager.DeviceType, HashSet<string>>()
    {
      {
        InputManager.DeviceType.Keyboard,
        new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
        {
          "<Keyboard>/shift",
          "<Keyboard>/ctrl",
          "<Keyboard>/alt"
        }
      },
      {
        InputManager.DeviceType.Mouse,
        new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
        {
          "<Keyboard>/shift",
          "<Keyboard>/ctrl",
          "<Keyboard>/alt"
        }
      },
      {
        InputManager.DeviceType.Gamepad,
        new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
        {
          "<Gamepad>/leftStickPress",
          "<Gamepad>/rightStickPress"
        }
      }
    };
    private InputRecorder m_InputRecorder;
    public static readonly ILog log = LogManager.GetLogger(nameof (InputManager));
    private readonly InputConflictResolution m_ConflictResolution = new InputConflictResolution();
    private readonly InputActionAsset m_ActionAsset;
    private readonly UIInputActionCollection m_UIActionCollection;
    private readonly UIInputActionCollection m_ToolActionCollection;
    private readonly System.Collections.Generic.Dictionary<string, ProxyActionMap> m_Maps = new System.Collections.Generic.Dictionary<string, ProxyActionMap>();
    internal InputBarrier m_MouseToolBarrier;
    private System.Collections.Generic.Dictionary<InputDevice, DeviceListener> m_DeviceListeners;
    private InputDevice m_LastActiveDevice;
    private bool m_MouseOverUI;
    private float m_AccumulatedIdleDelay;
    private bool m_Idle;
    private bool m_HasFocus;
    private bool m_HasInputFieldFocus;
    private bool m_OverlayActive;
    private bool m_HideCursor;
    private InputManager.ControlScheme m_ActiveControlScheme;
    private InputActionMap.DeviceArray m_Devices;
    private InputManager.DeviceType m_ConnectedDeviceTypes;
    private InputManager.DeviceType m_BlockedControlTypes;
    private InputManager.DeviceType m_Mask;
    private readonly System.Collections.Generic.Dictionary<ProxyBinding, ProxyBinding.Watcher> m_ProxyBindingWatchers = new System.Collections.Generic.Dictionary<ProxyBinding, ProxyBinding.Watcher>((IEqualityComparer<ProxyBinding>) new ProxyBinding.Comparer(ProxyBinding.Comparer.Options.MapName | ProxyBinding.Comparer.Options.ActionName | ProxyBinding.Comparer.Options.Name | ProxyBinding.Comparer.Options.Device | ProxyBinding.Comparer.Options.Component));
    private static string m_ProhibitionModifierProcessor;
    private const string kKeyBindingConflict = "KeyBindingConflict";
    private const string kKeyBindingConflictResolved = "KeyBindingConflictResolved";
    private static readonly InputManager.DeferManagerUpdatingWrapper sDeferUpdatingWrapper = new InputManager.DeferManagerUpdatingWrapper();

    public IEnumerable<ProxyAction> actions
    {
      get
      {
        foreach (KeyValuePair<string, ProxyActionMap> map in this.m_Maps)
        {
          string str;
          ProxyActionMap proxyActionMap;
          map.Deconstruct(ref str, ref proxyActionMap);
          foreach (KeyValuePair<string, ProxyAction> action1 in (IEnumerable<KeyValuePair<string, ProxyAction>>) proxyActionMap.actions)
          {
            ProxyAction action2;
            action1.Deconstruct(ref str, ref action2);
            yield return action2;
          }
        }
      }
    }

    public ProxyAction FindAction(string mapName, string actionName)
    {
      return this.FindActionMap(mapName)?.FindAction(actionName);
    }

    public bool TryFindAction(string mapName, string actionName, out ProxyAction action)
    {
      action = this.FindAction(mapName, actionName);
      return action != null;
    }

    public ProxyAction FindAction(ProxyBinding binding)
    {
      return this.FindActionMap(binding.mapName)?.FindAction(binding.actionName);
    }

    public bool TryFindAction(ProxyBinding binding, out ProxyAction action)
    {
      action = this.FindAction(binding.mapName, binding.actionName);
      return action != null;
    }

    public ProxyAction FindAction(InputAction action)
    {
      return this.FindActionMap(action?.actionMap)?.FindAction(action);
    }

    public bool TryFindAction(InputAction action, out ProxyAction proxyAction)
    {
      proxyAction = this.FindAction(action);
      return proxyAction != null;
    }

    public ProxyAction FindAction(Guid guid)
    {
      foreach (KeyValuePair<string, ProxyActionMap> map in this.m_Maps)
      {
        string str;
        ProxyActionMap proxyActionMap;
        map.Deconstruct(ref str, ref proxyActionMap);
        foreach (KeyValuePair<string, ProxyAction> action1 in (IEnumerable<KeyValuePair<string, ProxyAction>>) proxyActionMap.actions)
        {
          ProxyAction action2;
          action1.Deconstruct(ref str, ref action2);
          ProxyAction proxyAction = action2;
          if (proxyAction.sourceAction.id == guid)
          {
            action2 = proxyAction;
            return action2;
          }
        }
      }
      return (ProxyAction) null;
    }

    public bool TryFindAction(Guid guid, out ProxyAction proxyAction)
    {
      proxyAction = this.FindAction(guid);
      return proxyAction != null;
    }

    internal bool TryFindAction(int index, out ProxyAction action)
    {
      return this.actionIndex.TryGetValue(index, out action);
    }

    private void RefreshActiveControl()
    {
      this.mask = this.GetMaskForControlScheme();
      if (this.m_ActiveControlScheme != InputManager.ControlScheme.KeyboardAndMouse || !Keyboard.current.added)
        return;
      UnityEngine.Input.imeCompositionMode = this.hasInputFieldFocus ? IMECompositionMode.On : IMECompositionMode.Off;
      Keyboard.current.SetIMEEnabled(this.hasInputFieldFocus);
      Keyboard.current.SetIMECursorPosition(this.caretRect.Item1 + this.caretRect.Item2);
    }

    private InputManager.DeviceType GetMaskForControlScheme()
    {
      InputManager.DeviceType deviceType;
      switch (this.activeControlScheme)
      {
        case InputManager.ControlScheme.KeyboardAndMouse:
          deviceType = !this.overlayActive ? (this.hasInputFieldFocus ? InputManager.DeviceType.Mouse : InputManager.DeviceType.Keyboard | InputManager.DeviceType.Mouse) : InputManager.DeviceType.None;
          break;
        case InputManager.ControlScheme.Gamepad:
          deviceType = !this.overlayActive ? InputManager.DeviceType.Gamepad : InputManager.DeviceType.None;
          break;
        default:
          deviceType = InputManager.DeviceType.None;
          break;
      }
      return deviceType & ~this.blockedControlTypes;
    }

    private void RefreshBarriers()
    {
      this.m_MouseToolBarrier.blocked = this.activeControlScheme == InputManager.ControlScheme.KeyboardAndMouse && this.mouseOverUI;
    }

    internal void OnActionChanged()
    {
      if (InputManager.sDeferUpdatingWrapper.isDeferred)
        this.m_NeedUpdate = true;
      else
        this.ProcessActionsUpdate();
    }

    private void ProcessActionsUpdate(bool ignoreDefer = false)
    {
      if (InputManager.sDeferUpdatingWrapper.isDeferred && !ignoreDefer || !this.m_NeedUpdate)
        return;
      this.m_NeedUpdate = false;
      ++this.actionVersion;
      this.CheckConflicts();
      Action eventActionsChanged = this.EventActionsChanged;
      if (eventActionsChanged == null)
        return;
      eventActionsChanged();
    }

    internal void AddActions(ProxyAction.Info[] actionsToAdd)
    {
      ProxyAction[] source = new ProxyAction[actionsToAdd.Length];
      using (InputManager.DeferUpdating())
      {
        for (int index = 0; index < actionsToAdd.Length; ++index)
        {
          ProxyActionMap map = this.GetOrCreateMap(actionsToAdd[index].m_Map);
          source[index] = map.AddAction(actionsToAdd[index], true);
        }
      }
      foreach (ProxyActionMap proxyActionMap in ((IEnumerable<ProxyAction>) source).Select<ProxyAction, ProxyActionMap>((Func<ProxyAction, ProxyActionMap>) (a => a.map)).Distinct<ProxyActionMap>().ToArray<ProxyActionMap>())
        proxyActionMap.UpdateState();
    }

    internal void UpdateActionInKeyActionMap(ProxyAction action)
    {
      HashSet<string> stringSet;
      string[] array;
      string[] strArray;
      if (!this.actionKeyMap.TryGetValue(action, out stringSet))
      {
        array = action.usedKeys.ToArray<string>();
        strArray = Array.Empty<string>();
        this.actionKeyMap[action] = new HashSet<string>((IEnumerable<string>) array);
      }
      else
      {
        HashSet<string> hashSet = action.usedKeys.ToHashSet<string>();
        array = hashSet.Except<string>((IEnumerable<string>) stringSet).ToArray<string>();
        strArray = stringSet.Except<string>((IEnumerable<string>) hashSet).ToArray<string>();
        this.actionKeyMap[action] = hashSet;
      }
      foreach (string key in strArray)
      {
        HashSet<ProxyAction> proxyActionSet;
        if (this.keyActionMap.TryGetValue(key, out proxyActionSet))
          proxyActionSet.Remove(action);
      }
      foreach (string key in array)
      {
        HashSet<ProxyAction> proxyActionSet;
        if (!this.keyActionMap.TryGetValue(key, out proxyActionSet))
        {
          proxyActionSet = new HashSet<ProxyAction>();
          this.keyActionMap.Add(key, proxyActionSet);
        }
        proxyActionSet.Add(action);
      }
    }

    public static bool HasConflicts(
      ProxyAction action1,
      ProxyAction action2,
      InputManager.DeviceType? maskOverride1 = null,
      InputManager.DeviceType? maskOverride2 = null)
    {
      InputManager.DeviceType deviceType1 = (InputManager.DeviceType) ((int) maskOverride1 ?? (int) action1.mask);
      InputManager.DeviceType deviceType2 = (InputManager.DeviceType) ((int) maskOverride2 ?? (int) action2.mask);
      foreach (KeyValuePair<InputManager.DeviceType, ProxyComposite> composite1 in (IEnumerable<KeyValuePair<InputManager.DeviceType, ProxyComposite>>) action1.composites)
      {
        InputManager.DeviceType deviceType3;
        ProxyComposite proxyComposite1;
        composite1.Deconstruct(ref deviceType3, ref proxyComposite1);
        ProxyComposite proxyComposite2 = proxyComposite1;
        if ((proxyComposite2.m_Device & deviceType1) != InputManager.DeviceType.None)
        {
          foreach (KeyValuePair<InputManager.DeviceType, ProxyComposite> composite2 in (IEnumerable<KeyValuePair<InputManager.DeviceType, ProxyComposite>>) action2.composites)
          {
            composite2.Deconstruct(ref deviceType3, ref proxyComposite1);
            ProxyComposite proxyComposite3 = proxyComposite1;
            if ((proxyComposite3.m_Device & deviceType2) != InputManager.DeviceType.None)
            {
              foreach (KeyValuePair<ActionComponent, ProxyBinding> binding1 in (IEnumerable<KeyValuePair<ActionComponent, ProxyBinding>>) proxyComposite2.bindings)
              {
                ActionComponent actionComponent;
                ProxyBinding proxyBinding;
                binding1.Deconstruct(ref actionComponent, ref proxyBinding);
                ProxyBinding x = proxyBinding;
                foreach (KeyValuePair<ActionComponent, ProxyBinding> binding2 in (IEnumerable<KeyValuePair<ActionComponent, ProxyBinding>>) proxyComposite3.bindings)
                {
                  binding2.Deconstruct(ref actionComponent, ref proxyBinding);
                  ProxyBinding y = proxyBinding;
                  if ((action1 != action2 || x.component != y.component) && ProxyBinding.ConflictsWith(x, y, false))
                    return true;
                }
              }
            }
          }
        }
      }
      return false;
    }

    public static bool CanConflict(
      ProxyAction action1,
      ProxyAction action2,
      InputManager.DeviceType device)
    {
      if (action1 == action2)
        return false;
      HashSet<ProxyAction.LinkInfo> linkedActions1 = action1.m_LinkedActions;
      ProxyAction.LinkInfo linkInfo1 = new ProxyAction.LinkInfo();
      linkInfo1.m_Action = action2;
      linkInfo1.m_Device = device;
      ProxyAction.LinkInfo linkInfo2 = linkInfo1;
      if (linkedActions1.Contains(linkInfo2))
        return false;
      HashSet<ProxyAction.LinkInfo> linkedActions2 = action2.m_LinkedActions;
      linkInfo1 = new ProxyAction.LinkInfo();
      linkInfo1.m_Action = action1;
      linkInfo1.m_Device = device;
      ProxyAction.LinkInfo linkInfo3 = linkInfo1;
      return !linkedActions2.Contains(linkInfo3);
    }

    public List<ProxyBinding> GetBindings(
      InputManager.PathType pathType,
      InputManager.BindingOptions bindingOptions)
    {
      using (PerformanceCounter.Start((Action<TimeSpan>) (t => InputManager.log.InfoFormat("Get {1} bindings {2} in {0}ms", (object) t.TotalMilliseconds, (object) pathType, (object) bindingOptions))))
      {
        List<ProxyBinding> bindingsList = new List<ProxyBinding>();
        foreach (ProxyActionMap proxyActionMap in this.m_Maps.Values)
        {
          foreach (InputAction action1 in proxyActionMap.sourceMap.m_Actions)
          {
            InputAction action = action1;
            action.ForEachCompositeOfAction((Func<InputActionSetupExtensions.BindingSyntax, bool>) (iterator =>
            {
              ProxyComposite proxyComposite;
              if (this.TryGetComposite(action, iterator, pathType, bindingOptions, out proxyComposite))
              {
                foreach (KeyValuePair<ActionComponent, ProxyBinding> binding in (IEnumerable<KeyValuePair<ActionComponent, ProxyBinding>>) proxyComposite.bindings)
                {
                  ActionComponent actionComponent;
                  ProxyBinding proxyBinding;
                  binding.Deconstruct(ref actionComponent, ref proxyBinding);
                  bindingsList.Add(proxyBinding);
                }
              }
              return true;
            }));
          }
        }
        return bindingsList;
      }
    }

    public bool TryGetBinding(
      ProxyBinding bindingToGet,
      InputManager.PathType pathType,
      InputManager.BindingOptions bindingOptions,
      out ProxyBinding foundBinding)
    {
      foundBinding = new ProxyBinding();
      ProxyAction action;
      InputActionSetupExtensions.BindingSyntax compositeIterator;
      InputActionSetupExtensions.BindingSyntax bindingIterator;
      CompositeInstance compositeInstance;
      InputManager.CompositeComponentData componentData;
      return this.TryFindAction(bindingToGet, out action) && action.sourceAction != null && this.TryGetIterators(bindingToGet, action.sourceAction, out compositeIterator, out bindingIterator, out compositeInstance, out componentData) && this.TryGetBinding(action.sourceAction, compositeIterator, bindingIterator, compositeInstance, componentData, pathType, bindingOptions, out foundBinding);
    }

    private bool TryGetBinding(
      InputAction action,
      InputActionSetupExtensions.BindingSyntax compositeIterator,
      InputActionSetupExtensions.BindingSyntax bindingIterator,
      CompositeInstance compositeInstance,
      InputManager.CompositeComponentData componentData,
      InputManager.PathType pathType,
      InputManager.BindingOptions bindingOptions,
      out ProxyBinding foundBinding)
    {
      int num = (bindingOptions & InputManager.BindingOptions.OnlyRebound) != 0 ? 1 : 0;
      InputBinding binding = bindingIterator.binding;
      string currentPath;
      string originalPath;
      ProxyModifier[] currentModifiers;
      ProxyModifier[] originalModifiers;
      bool flag = this.TryGetMainBinding(bindingIterator, pathType, out currentPath, out originalPath) | this.TryGetModifierBindings(action, compositeInstance, compositeIterator, bindingIterator, pathType, componentData, out currentModifiers, out originalModifiers);
      if (num != 0 && !flag)
      {
        foundBinding = new ProxyBinding();
        return false;
      }
      foundBinding = new ProxyBinding(action, componentData.m_Component, binding.name, compositeInstance)
      {
        path = currentPath,
        modifiers = (IReadOnlyList<ProxyModifier>) currentModifiers,
        originalPath = originalPath,
        originalModifiers = (IReadOnlyList<ProxyModifier>) originalModifiers,
        device = compositeIterator.binding.name.ToDeviceType()
      };
      return true;
    }

    public bool TryGetModifierBindings(
      InputAction action,
      CompositeInstance compositeInstance,
      InputActionSetupExtensions.BindingSyntax compositeIterator,
      InputActionSetupExtensions.BindingSyntax iterator,
      InputManager.PathType pathType,
      InputManager.CompositeComponentData componentData,
      out ProxyModifier[] currentModifiers,
      out ProxyModifier[] originalModifiers)
    {
      currentModifiers = (ProxyModifier[]) null;
      originalModifiers = (ProxyModifier[]) null;
      HashSet<string> supportedModifiers;
      if (!compositeInstance.allowModifiers || !InputManager.kModifiers.TryGetValue(compositeIterator.binding.name.ToDeviceType(), out supportedModifiers))
        return false;
      bool isRebound = false;
      List<ProxyModifier> currentModifierList = new List<ProxyModifier>();
      List<ProxyModifier> originalModifierList = new List<ProxyModifier>();
      action.ForEachPartOfCompositeWithName(compositeIterator, componentData.m_ModifierName, (Func<InputActionSetupExtensions.BindingSyntax, bool>) (modifierIterator =>
      {
        InputBinding binding = modifierIterator.binding;
        if (string.IsNullOrEmpty(binding.path) || !supportedModifiers.Contains(binding.path))
          return true;
        isRebound |= binding.overrideProcessors != null;
        if (!binding.GetProcessors(pathType).Contains(InputManager.prohibitionModifierProcessor))
          currentModifierList.Add(new ProxyModifier()
          {
            m_Component = componentData.m_Component,
            m_Name = binding.name,
            m_Path = binding.path
          });
        if (!binding.processors.Contains(InputManager.prohibitionModifierProcessor))
          originalModifierList.Add(new ProxyModifier()
          {
            m_Component = componentData.m_Component,
            m_Name = binding.name,
            m_Path = binding.path
          });
        return true;
      }), out InputActionSetupExtensions.BindingSyntax _);
      currentModifiers = currentModifierList.ToArray();
      originalModifiers = originalModifierList.ToArray();
      return isRebound;
    }

    public bool TryGetMainBinding(
      InputActionSetupExtensions.BindingSyntax iterator,
      InputManager.PathType pathType,
      out string currentPath,
      out string originalPath)
    {
      InputBinding binding = iterator.binding;
      currentPath = binding.GetPath(pathType);
      originalPath = binding.path;
      return binding.overridePath != null;
    }

    public bool SetBindings(
      IEnumerable<ProxyBinding> newBindings,
      out List<ProxyBinding> resultBindings)
    {
      using (PerformanceCounter.Start((Action<TimeSpan>) (t => InputManager.log.InfoFormat("Set bindings in {0}ms", (object) t.TotalMilliseconds))))
      {
        resultBindings = new List<ProxyBinding>();
        using (InputManager.DeferUpdating())
        {
          foreach (ProxyBinding newBinding1 in newBindings)
          {
            ProxyBinding newBinding2;
            this.SetBindingImpl(newBinding1, out newBinding2);
            resultBindings.Add(newBinding2);
          }
        }
        return true;
      }
    }

    public bool SetBinding(ProxyBinding newBinding, out ProxyBinding result)
    {
      string bindingName = newBinding.ToString();
      using (PerformanceCounter.Start((Action<TimeSpan>) (t => InputManager.log.InfoFormat("Set binding {1} in {0}ms", (object) t.TotalMilliseconds, (object) bindingName))))
      {
        using (InputManager.DeferUpdating())
        {
          if (!this.SetBindingImpl(newBinding, out result))
            return false;
        }
        return true;
      }
    }

    internal bool SetBindingImpl(ProxyBinding bindingToSet, out ProxyBinding newBinding)
    {
      ProxyAction action;
      if (!this.TryFindAction(bindingToSet.mapName, bindingToSet.actionName, out action) || action.sourceAction == null)
      {
        newBinding = new ProxyBinding();
        return false;
      }
      InputActionSetupExtensions.BindingSyntax compositeIterator;
      InputActionSetupExtensions.BindingSyntax bindingIterator;
      CompositeInstance compositeInstance;
      InputManager.CompositeComponentData componentData;
      if (!this.TryGetIterators(bindingToSet, action.sourceAction, out compositeIterator, out bindingIterator, out compositeInstance, out componentData))
      {
        newBinding = new ProxyBinding();
        return false;
      }
      if (!compositeInstance.isRebindable)
      {
        newBinding = new ProxyBinding();
        return false;
      }
      ProxyModifier[] originalModifiers;
      if (!compositeInstance.isModifiersRebindable && this.TryGetModifierBindings(action.sourceAction, compositeInstance, compositeIterator, bindingIterator, InputManager.PathType.Original, componentData, out ProxyModifier[] _, out originalModifiers) && !ProxyBinding.ModifiersListComparer.defaultComparer.Equals((IReadOnlyCollection<ProxyModifier>) bindingToSet.modifiers, (IReadOnlyCollection<ProxyModifier>) originalModifiers))
      {
        newBinding = new ProxyBinding();
        return false;
      }
      if (string.IsNullOrEmpty(bindingToSet.path) && !compositeInstance.canBeEmpty)
      {
        newBinding = new ProxyBinding();
        return false;
      }
      bool changed1;
      bool changed2;
      if (!this.TrySetMainBinding(bindingToSet, action.sourceAction, bindingIterator, out changed1) || !this.TrySetModifierBindings(bindingToSet, action.sourceAction, compositeInstance, componentData, compositeIterator, bindingIterator, out changed2))
      {
        newBinding = new ProxyBinding();
        return false;
      }
      if (!changed1 && !changed2)
      {
        newBinding = new ProxyBinding();
        return false;
      }
      action.Update();
      return this.TryGetBinding(action.sourceAction, compositeIterator, bindingIterator, compositeInstance, componentData, InputManager.PathType.Effective, InputManager.BindingOptions.None, out newBinding);
    }

    private bool TrySetMainBinding(
      ProxyBinding bindingToSet,
      InputAction action,
      InputActionSetupExtensions.BindingSyntax bindingIterator,
      out bool changed)
    {
      InputBinding binding = bindingIterator.binding;
      if (bindingToSet.path == binding.path)
      {
        if (binding.overridePath != null)
        {
          binding.overridePath = (string) null;
          action.actionMap.ApplyBindingOverride(bindingIterator.m_BindingIndexInMap, binding);
          changed = true;
          return true;
        }
      }
      else if (bindingToSet.path != binding.overridePath)
      {
        binding.overridePath = bindingToSet.path;
        action.actionMap.ApplyBindingOverride(bindingIterator.m_BindingIndexInMap, binding);
        changed = true;
        return true;
      }
      changed = false;
      return true;
    }

    private bool TrySetModifierBindings(
      ProxyBinding bindingToSet,
      InputAction action,
      CompositeInstance compositeInstance,
      InputManager.CompositeComponentData componentData,
      InputActionSetupExtensions.BindingSyntax compositeIterator,
      InputActionSetupExtensions.BindingSyntax bindingIterator,
      out bool changed)
    {
      if (!compositeInstance.allowModifiers)
      {
        changed = false;
        return true;
      }
      HashSet<string> supportedModifiers;
      if (!InputManager.kModifiers.TryGetValue(compositeIterator.binding.name.ToDeviceType(), out supportedModifiers))
        supportedModifiers = new HashSet<string>();
      bool changedModifier = false;
      IReadOnlyList<ProxyModifier> modifiers = bindingToSet.modifiers;
      action.ForEachPartOfCompositeWithName(compositeIterator, componentData.m_ModifierName, (Func<InputActionSetupExtensions.BindingSyntax, bool>) (modifierIterator =>
      {
        InputBinding modifierBinding = modifierIterator.binding;
        if (string.IsNullOrEmpty(modifierBinding.path) || !supportedModifiers.Contains(modifierBinding.path))
          return true;
        bool allow = modifiers.Any<ProxyModifier>((Func<ProxyModifier, bool>) (m => StringComparer.OrdinalIgnoreCase.Equals(m.m_Path, modifierBinding.path)));
        changedModifier |= this.TrySetBindingModifierProcessor(action, modifierIterator, allow);
        return true;
      }), out InputActionSetupExtensions.BindingSyntax _);
      changed = changedModifier;
      return true;
    }

    private bool TrySetBindingModifierProcessor(
      InputAction action,
      InputActionSetupExtensions.BindingSyntax modifierIterator,
      bool allow)
    {
      InputBinding binding = modifierIterator.binding;
      string str;
      if (allow)
        str = string.IsNullOrEmpty(binding.effectiveProcessors) || binding.effectiveProcessors == InputManager.prohibitionModifierProcessor ? string.Empty : string.Join<bool>(";", ((IEnumerable<string>) binding.effectiveProcessors.Split(';', StringSplitOptions.RemoveEmptyEntries)).Select<string, bool>((Func<string, bool>) (p => p != InputManager.prohibitionModifierProcessor)));
      else if (string.IsNullOrEmpty(binding.effectiveProcessors) || binding.effectiveProcessors == InputManager.prohibitionModifierProcessor)
      {
        str = InputManager.prohibitionModifierProcessor;
      }
      else
      {
        string[] source = binding.effectiveProcessors.Split(';', StringSplitOptions.RemoveEmptyEntries);
        str = ((IEnumerable<string>) source).Any<string>((Func<string, bool>) (p => p == InputManager.prohibitionModifierProcessor)) ? binding.effectiveProcessors : string.Join(";", ((IEnumerable<string>) source).Append<string>(InputManager.prohibitionModifierProcessor));
      }
      if (str == binding.processors)
      {
        if (binding.overrideProcessors != null)
        {
          binding.overrideProcessors = (string) null;
          action.actionMap.ApplyBindingOverride(modifierIterator.m_BindingIndexInMap, binding);
          return true;
        }
      }
      else if (str != binding.overrideProcessors)
      {
        binding.overrideProcessors = str;
        action.actionMap.ApplyBindingOverride(modifierIterator.m_BindingIndexInMap, binding);
        return true;
      }
      return false;
    }

    public void ResetAllBindings(bool onlyBuiltIn = true)
    {
      this.SetBindings((IEnumerable<ProxyBinding>) this.GetBindings(InputManager.PathType.Original, (InputManager.BindingOptions) (4 | (onlyBuiltIn ? 8 : 0))), out List<ProxyBinding> _);
    }

    private bool TryGetIterators(
      ProxyBinding bindingSample,
      InputAction action,
      out InputActionSetupExtensions.BindingSyntax compositeIterator,
      out InputActionSetupExtensions.BindingSyntax bindingIterator,
      out CompositeInstance compositeInstance,
      out InputManager.CompositeComponentData componentData)
    {
      compositeIterator = new InputActionSetupExtensions.BindingSyntax();
      bindingIterator = new InputActionSetupExtensions.BindingSyntax();
      compositeInstance = (CompositeInstance) null;
      componentData = new InputManager.CompositeComponentData();
      if (!action.TryGetCompositeOfActionWithName(bindingSample.device.ToString(), out compositeIterator) || !this.TryGetCompositeInstance(compositeIterator, out compositeInstance))
        return false;
      if (bindingSample.component == ActionComponent.None)
      {
        if (!compositeInstance.compositeData.TryFindByBindingName(bindingSample.name, out componentData))
          return false;
      }
      else if (!compositeInstance.compositeData.TryGetData(bindingSample.component, out componentData))
        return false;
      bindingIterator = compositeIterator.NextPartBinding(componentData.m_BindingName);
      return bindingIterator.valid;
    }

    public void ResetGroupBindings(InputManager.DeviceType device, bool onlyBuiltIn = true)
    {
      this.SetBindings(this.GetBindings(InputManager.PathType.Original, (InputManager.BindingOptions) (4 | (onlyBuiltIn ? 8 : 0))).Where<ProxyBinding>((Func<ProxyBinding, bool>) (b => b.device == device)), out List<ProxyBinding> _);
    }

    internal ProxyBinding.Watcher GetOrCreateBindingWatcher(ProxyBinding binding)
    {
      ProxyBinding.Watcher bindingWatcher;
      if (!this.m_ProxyBindingWatchers.TryGetValue(binding, out bindingWatcher))
      {
        bindingWatcher = new ProxyBinding.Watcher(binding);
        if (bindingWatcher.isValid)
          this.m_ProxyBindingWatchers[binding] = bindingWatcher;
      }
      return bindingWatcher;
    }

    public List<ProxyComposite> GetComposites(InputAction action)
    {
      List<ProxyComposite> composites = new List<ProxyComposite>();
      action.ForEachCompositeOfAction((Func<InputActionSetupExtensions.BindingSyntax, bool>) (iterator =>
      {
        ProxyComposite proxyComposite;
        if (this.TryGetComposite(action, iterator, InputManager.PathType.Effective, InputManager.BindingOptions.None, out proxyComposite))
          composites.Add(proxyComposite);
        return true;
      }));
      return composites;
    }

    private bool TryGetComposite(
      InputAction action,
      InputActionSetupExtensions.BindingSyntax compositeIterator,
      InputManager.PathType pathType,
      InputManager.BindingOptions bindingOptions,
      out ProxyComposite proxyComposite)
    {
      List<ProxyBinding> bindingsList = new List<ProxyBinding>();
      proxyComposite = (ProxyComposite) null;
      CompositeInstance compositeInstance;
      if (!this.TryGetCompositeInstance(compositeIterator, out compositeInstance) || compositeInstance.developerOnly && !GameManager.instance.configuration.developerMode || !compositeInstance.platform.IsPlatformSet(Application.platform) || !compositeInstance.builtIn && (bindingOptions & InputManager.BindingOptions.OnlyBuiltIn) != InputManager.BindingOptions.None || !compositeInstance.isRebindable && (bindingOptions & InputManager.BindingOptions.OnlyRebindable) != InputManager.BindingOptions.None || compositeInstance.isDummy && (bindingOptions & InputManager.BindingOptions.ExcludeDummy) != InputManager.BindingOptions.None || compositeInstance.isHidden && (bindingOptions & InputManager.BindingOptions.ExcludeHidden) != InputManager.BindingOptions.None)
        return false;
      foreach (InputManager.CompositeComponentData compositeComponentData in compositeInstance.compositeData.m_Data.Values)
      {
        InputManager.CompositeComponentData componentData = compositeComponentData;
        action.ForEachPartOfCompositeWithName(compositeIterator, componentData.m_BindingName, (Func<InputActionSetupExtensions.BindingSyntax, bool>) (bindingIterator =>
        {
          ProxyBinding foundBinding;
          if (this.TryGetBinding(action, compositeIterator, bindingIterator, compositeInstance, componentData, pathType, bindingOptions, out foundBinding))
            bindingsList.Add(foundBinding);
          return true;
        }), out InputActionSetupExtensions.BindingSyntax _);
      }
      if (bindingsList.Count == 0)
        return false;
      proxyComposite = new ProxyComposite(compositeIterator.binding.name.ToDeviceType(), compositeInstance.compositeData.m_ActionType, compositeInstance, (IList<ProxyBinding>) bindingsList);
      return true;
    }

    private bool TryGetCompositeInstance(
      InputActionSetupExtensions.BindingSyntax compositeIterator,
      out CompositeInstance compositeInstance)
    {
      NameAndParameters[] array = NameAndParameters.ParseMultiple(compositeIterator.binding.effectivePath).ToArray<NameAndParameters>();
      compositeInstance = array.Length != 2 || !(array[1].name == "Usages") ? new CompositeInstance(array[0]) : new CompositeInstance(array[0], array[1]);
      return compositeInstance != null;
    }

    public static void RegisterBindingComposite<T>(
      string name,
      ActionType type,
      InputManager.CompositeComponentData[] data = null)
      where T : InputBindingComposite
    {
      UnityEngine.InputSystem.InputSystem.RegisterBindingComposite<T>(name);
      if (data == null || data.Length == 0)
        return;
      name = (string) InputBindingComposite.s_Composites.FindNameForType(typeof (T));
      InputManager.s_Composites.TryAdd(name, new InputManager.CompositeData(name, type, data));
    }

    public static string GeneratePathForControl(InputControl control)
    {
      InputDevice device = control.device;
      Debug.Assert(control != device, "Control must not be a device");
      InternedString introducesControl = InputControlLayout.s_Layouts.FindLayoutThatIntroducesControl(control, InputManager.m_LayoutCache);
      if (InputManager.m_PathBuilder == null)
        InputManager.m_PathBuilder = new StringBuilder();
      InputManager.m_PathBuilder.Length = 0;
      control.BuildPath((string) introducesControl, InputManager.m_PathBuilder);
      return InputManager.m_PathBuilder.ToString();
    }

    internal static bool TryGetCompositeData(string name, out InputManager.CompositeData data)
    {
      return InputManager.s_Composites.TryGetValue(name, out data);
    }

    internal static bool TryGetCompositeData(System.Type type, out InputManager.CompositeData data)
    {
      return InputManager.TryGetCompositeData((string) InputBindingComposite.s_Composites.FindNameForType(type), out data);
    }

    internal static bool TryGetCompositeData<T>(out InputManager.CompositeData data) where T : InputBindingComposite
    {
      return InputManager.TryGetCompositeData(typeof (T), out data);
    }

    internal static bool TryGetCompositeData(
      ActionType actionType,
      out InputManager.CompositeData data)
    {
      return InputManager.TryGetCompositeData(actionType.GetCompositeType(), out data);
    }

    public static string GetBindingName(ActionComponent component)
    {
      InputManager.CompositeData data1;
      InputManager.CompositeComponentData data2;
      return InputManager.TryGetCompositeData(component.GetActionType(), out data1) && data1.TryGetData(component, out data2) ? data2.m_BindingName : InputManager.CompositeComponentData.defaultData.m_BindingName;
    }

    public static string GetModifierName(ActionComponent component)
    {
      InputManager.CompositeData data1;
      InputManager.CompositeComponentData data2;
      return InputManager.TryGetCompositeData(component.GetActionType(), out data1) && data1.TryGetData(component, out data2) ? data2.m_ModifierName : InputManager.CompositeComponentData.defaultData.m_ModifierName;
    }

    public static InputManager instance => GameManager.instance.inputManager;

    public static bool exists => InputManager.instance != null;

    public InputRecorder inputRecorder
    {
      get
      {
        if (this.m_InputRecorder == null)
          this.m_InputRecorder = new InputRecorder();
        return this.m_InputRecorder;
      }
    }

    public event Action<InputManager.ControlScheme> EventControlSchemeChanged;

    public event InputManager.ActiveDeviceChanged EventActiveDeviceChanged;

    public event Action EventActiveDeviceDisconnected;

    public event Action EventActiveDeviceAssociationLost;

    public event Action EventDevicePaired;

    public event Action EventActionsChanged;

    public event Action EventEnabledActionsChanged;

    public event Action EventActionMasksChanged;

    public event Action EventActionDisplayNamesChanged;

    public event Action<bool> EventMouseOverUIChanged;

    internal event Action EventPreResolvedActionChanged;

    public bool mouseOverUI
    {
      get => this.m_MouseOverUI;
      set
      {
        if (value == this.m_MouseOverUI)
          return;
        this.m_MouseOverUI = value;
        this.RefreshBarriers();
        Action<bool> mouseOverUiChanged = this.EventMouseOverUIChanged;
        if (mouseOverUiChanged == null)
          return;
        mouseOverUiChanged(value);
      }
    }

    public bool hasInputFieldFocus
    {
      get => this.m_HasInputFieldFocus;
      set => this.m_HasInputFieldFocus = value;
    }

    public bool overlayActive => this.m_OverlayActive;

    public (Vector2, Vector2) caretRect { get; set; }

    public bool controlOverWorld
    {
      get
      {
        return (!this.mouseOverUI || this.activeControlScheme != InputManager.ControlScheme.KeyboardAndMouse) && this.mouseOnScreen;
      }
    }

    InputBinding? IInputActionCollection.bindingMask
    {
      get => this.mask.ToInputBinding();
      set => this.mask = value.ToDeviceType();
    }

    ReadOnlyArray<InputDevice>? IInputActionCollection.devices
    {
      get => this.m_Devices.Get();
      set
      {
        if (!this.m_Devices.Set(value))
          return;
        foreach (KeyValuePair<string, ProxyActionMap> map in this.m_Maps)
        {
          string str;
          ProxyActionMap proxyActionMap;
          map.Deconstruct(ref str, ref proxyActionMap);
          proxyActionMap.sourceMap.devices = value;
        }
      }
    }

    ReadOnlyArray<InputControlScheme> IInputActionCollection.controlSchemes
    {
      get => this.m_ActionAsset.controlSchemes;
    }

    public InputManager.ControlScheme activeControlScheme
    {
      get => this.m_ActiveControlScheme;
      private set
      {
        if (this.m_ActiveControlScheme == value)
          return;
        InputManager.log.InfoFormat("Active control scheme set: {0}", (object) value);
        this.m_ActiveControlScheme = value;
        this.UpdateCursorVisibility();
        this.RefreshActiveControl();
        this.RefreshBarriers();
        Action<InputManager.ControlScheme> controlSchemeChanged = this.EventControlSchemeChanged;
        if (controlSchemeChanged != null)
          controlSchemeChanged(value);
        Telemetry.ControlSchemeChanged(value);
      }
    }

    public InputManager.DeviceType connectedDeviceTypes => this.m_ConnectedDeviceTypes;

    internal InputManager.DeviceType mask
    {
      get => this.m_Mask;
      set
      {
        if (value == this.m_Mask)
          return;
        InputManager.log.InfoFormat("Set mask: {0}", (object) value);
        this.m_Mask = value;
        foreach (KeyValuePair<string, ProxyActionMap> map in this.m_Maps)
        {
          string str;
          ProxyActionMap proxyActionMap;
          map.Deconstruct(ref str, ref proxyActionMap);
          proxyActionMap.mask = value;
        }
      }
    }

    internal InputManager.DeviceType blockedControlTypes
    {
      get => this.m_BlockedControlTypes;
      set
      {
        if (value == this.m_BlockedControlTypes)
          return;
        InputManager.log.InfoFormat("Block control types: {0}", (object) value);
        this.m_BlockedControlTypes = value;
        this.RefreshActiveControl();
      }
    }

    public bool mouseOnScreen
    {
      get
      {
        return (double) this.mousePosition.x >= 0.0 && (double) this.mousePosition.x < (double) Screen.width && (double) this.mousePosition.y >= 0.0 && (double) this.mousePosition.y < (double) Screen.height && this.m_HasFocus;
      }
    }

    public Vector2 gamepadPointerPosition
    {
      get => new Vector2((float) Screen.width / 2f, (float) Screen.height / 2f);
    }

    public Vector3 mousePosition
    {
      get
      {
        Mouse current = Mouse.current;
        return this.activeControlScheme == InputManager.ControlScheme.KeyboardAndMouse && current != null ? (Vector3) current.position.ReadValue() : (Vector3) this.gamepadPointerPosition;
      }
    }

    public bool hideCursor
    {
      get => this.m_HideCursor;
      set
      {
        if (value == this.m_HideCursor)
          return;
        this.m_HideCursor = value;
        this.UpdateCursorVisibility();
      }
    }

    public CursorLockMode cursorLockMode
    {
      get => Cursor.lockState;
      set => Cursor.lockState = value;
    }

    public InputUser inputUser { get; private set; }

    public int actionVersion { get; private set; }

    internal System.Collections.Generic.Dictionary<string, HashSet<ProxyAction>> keyActionMap { get; } = new System.Collections.Generic.Dictionary<string, HashSet<ProxyAction>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    internal System.Collections.Generic.Dictionary<ProxyAction, HashSet<string>> actionKeyMap { get; } = new System.Collections.Generic.Dictionary<ProxyAction, HashSet<string>>();

    internal System.Collections.Generic.Dictionary<int, ProxyAction> actionIndex { get; } = new System.Collections.Generic.Dictionary<int, ProxyAction>();

    private static string prohibitionModifierProcessor
    {
      get
      {
        return InputManager.m_ProhibitionModifierProcessor ?? (InputManager.m_ProhibitionModifierProcessor = (string) InputProcessor.s_Processors.FindNameForType(typeof (ProhibitionModifierProcessor)));
      }
    }

    internal UIInputActionCollection uiActionCollection => this.m_UIActionCollection;

    internal UIInputActionCollection toolActionCollection => this.m_ToolActionCollection;

    public InputManager.DeviceType bindingConflicts { get; private set; }

    public InputManager()
    {
      InputManager.log.Debug((object) "Creating InputManager");
      this.OnFocusChanged(Application.isFocused);
      this.m_ActionAsset = UnityEngine.Resources.Load<InputActionAsset>("Input/InputActions");
      this.m_UIActionCollection = UnityEngine.Resources.Load<UIInputActionCollection>("Input/UI Input Actions");
      this.m_ToolActionCollection = UnityEngine.Resources.Load<UIInputActionCollection>("Input/Tool Input Actions");
      foreach (InputActionMap actionMap in this.m_ActionAsset.m_ActionMaps)
      {
        actionMap.m_Asset = (InputActionAsset) null;
        ProxyActionMap proxyActionMap = new ProxyActionMap(actionMap);
        this.m_Maps.Add(proxyActionMap.name, proxyActionMap);
      }
    }

    public void Dispose()
    {
      InputManager.log.Debug((object) "Disposing InputManager");
      if (this.inputUser.valid)
        this.inputUser.UnpairDevicesAndRemoveUser();
      UnityEngine.InputSystem.InputSystem.onDeviceChange -= new Action<InputDevice, InputDeviceChange>(this.OnDeviceChange);
      foreach (KeyValuePair<InputDevice, DeviceListener> deviceListener1 in this.m_DeviceListeners)
      {
        InputDevice inputDevice;
        DeviceListener deviceListener2;
        deviceListener1.Deconstruct(ref inputDevice, ref deviceListener2);
        deviceListener2.StopListening();
      }
      PlatformManager.instance.onDeviceAssociationChanged -= new OnDeviceAssociationChangedEventHandler(this.OnDeviceAssociationChanged);
      PlatformManager.instance.onOverlayStateChanged -= new Colossal.PSI.Common.OnOverlayStateChanged(this.OnOverlayStateChanged);
    }

    public void Update()
    {
      if (!this.m_OverlayActive)
      {
        foreach (KeyValuePair<InputDevice, DeviceListener> deviceListener1 in this.m_DeviceListeners)
        {
          InputDevice inputDevice;
          DeviceListener deviceListener2;
          deviceListener1.Deconstruct(ref inputDevice, ref deviceListener2);
          deviceListener2.Tick();
        }
      }
      if (this.m_ActiveControlScheme == InputManager.ControlScheme.KeyboardAndMouse)
      {
        Mouse current = Mouse.current;
        if ((current != null ? ((double) current.delta.value.magnitude > 0.20000000298023224 ? 1 : 0) : 0) != 0)
        {
          this.m_AccumulatedIdleDelay = 0.0f;
          if (this.m_Idle)
          {
            this.m_Idle = false;
            Telemetry.InputIdleEnd();
          }
        }
      }
      if (!this.m_Idle && !GameManager.instance.isGameLoading)
      {
        this.m_AccumulatedIdleDelay += Time.unscaledDeltaTime;
        if ((double) this.m_AccumulatedIdleDelay >= 30.0)
        {
          this.m_AccumulatedIdleDelay = 30f;
          this.m_Idle = true;
          InputManager.log.Debug((object) "Input idle");
          Telemetry.InputIdleStart();
        }
      }
      this.m_ConflictResolution.Update();
      this.RefreshActiveControl();
    }

    private void UpdateCursorVisibility()
    {
      Cursor.visible = this.activeControlScheme == InputManager.ControlScheme.KeyboardAndMouse && !this.hideCursor;
    }

    internal void CheckConflicts()
    {
      if (GameManager.instance.state < GameManager.State.UIReady)
        return;
      this.bindingConflicts = InputManager.DeviceType.None;
      foreach (KeyValuePair<string, ProxyActionMap> map1 in this.m_Maps)
      {
        string str;
        ProxyActionMap proxyActionMap;
        map1.Deconstruct(ref str, ref proxyActionMap);
        ProxyActionMap map2 = proxyActionMap;
        bool conflict = false;
        foreach (KeyValuePair<string, ProxyAction> action in (IEnumerable<KeyValuePair<string, ProxyAction>>) map2.actions)
        {
          ProxyAction proxyAction1;
          action.Deconstruct(ref str, ref proxyAction1);
          ProxyAction proxyAction2 = proxyAction1;
          if ((proxyAction2.availableDevices & ~this.bindingConflicts) != InputManager.DeviceType.None)
          {
            foreach (KeyValuePair<InputManager.DeviceType, ProxyComposite> composite in (IEnumerable<KeyValuePair<InputManager.DeviceType, ProxyComposite>>) proxyAction2.composites)
            {
              InputManager.DeviceType deviceType;
              ProxyComposite proxyComposite1;
              composite.Deconstruct(ref deviceType, ref proxyComposite1);
              ProxyComposite proxyComposite2 = proxyComposite1;
              if ((proxyComposite2.m_Device & ~this.bindingConflicts) != InputManager.DeviceType.None)
              {
                foreach (KeyValuePair<ActionComponent, ProxyBinding> binding in (IEnumerable<KeyValuePair<ActionComponent, ProxyBinding>>) proxyComposite2.bindings)
                {
                  ActionComponent actionComponent;
                  ProxyBinding proxyBinding1;
                  binding.Deconstruct(ref actionComponent, ref proxyBinding1);
                  ProxyBinding proxyBinding2 = proxyBinding1;
                  if ((proxyBinding2.hasConflicts & ProxyBinding.ConflictType.WithBuiltIn) != ProxyBinding.ConflictType.None)
                  {
                    if (proxyBinding2.isBuiltIn)
                      this.bindingConflicts |= proxyBinding2.device;
                    else
                      conflict = true;
                  }
                }
              }
            }
            if (this.bindingConflicts == InputManager.DeviceType.All & conflict)
              break;
          }
        }
        this.SetModConflictNotification(map2, conflict);
      }
      this.SetBuiltInConflictNotification(this.bindingConflicts != 0);
    }

    private void SetBuiltInConflictNotification(bool conflict)
    {
      if (conflict == NotificationSystem.Exist("KeyBindingConflict"))
        return;
      if (conflict)
      {
        // ISSUE: method pointer
        NotificationSystem.Push("KeyBindingConflict", titleId: "KeyBindingConflict", textId: "KeyBindingConflict", progressState: new ProgressState?(ProgressState.Warning), onClicked: (Action) (() => GameManager.instance.userInterface.appBindings.ShowMessageDialog(new Game.UI.MessageDialog(new LocalizedString?(LocalizedString.Id("Common.DIALOG_TITLE_INPUT")), LocalizedString.Id("Common.DIALOG_MESSAGE_INPUT"), LocalizedString.Id("Common.OK"), new LocalizedString[2]
        {
          LocalizedString.Id("Common.DIALOG_ACTION_INPUT[Reset]"),
          LocalizedString.Id("Common.DIALOG_ACTION_INPUT[OpenOptions]")
        }), new Action<int>((object) this, __methodptr(\u003CSetBuiltInConflictNotification\u003Eg__Callback\u007C212_1)))));
      }
      else
        NotificationSystem.Pop("KeyBindingConflict", 2f, textId: "KeyBindingConflictResolved", progressState: new ProgressState?(ProgressState.Complete));
    }

    private void SetModConflictNotification(ProxyActionMap map, bool conflict)
    {
      if (conflict == NotificationSystem.Exist(map.name))
        return;
      if (conflict)
      {
        string str1 = (string) null;
        Action action1 = (Action) null;
        LocalizedString localizedString = LocalizedString.IdWithFallback("Options.INPUT_MAP[" + map.name + "]", map.name);
        ModSetting modSetting;
        ExecutableAsset asset;
        if (ModSetting.instances.TryGetValue(map.name, out modSetting) && GameManager.instance.modManager.TryGetExecutableAsset(modSetting.mod, out asset))
        {
          localizedString = LocalizedString.Value(asset.mod.displayName);
          if (!string.IsNullOrEmpty(asset.mod.thumbnailPath))
            str1 = string.Format("{0}?width={1})", (object) asset.mod.thumbnailPath, (object) NotificationUISystem.width);
          // ISSUE: reference to a compiler-generated method
          action1 = (Action) (() => World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<OptionsUISystem>()?.OpenPage(map.name, (string) null, false));
        }
        string name = map.name;
        LocalizedString? title = new LocalizedString?(localizedString);
        string str2 = str1;
        ProgressState? nullable = new ProgressState?(ProgressState.Warning);
        Action action2 = action1;
        LocalizedString? text = new LocalizedString?();
        string thumbnail = str2;
        ProgressState? progressState = nullable;
        int? progress = new int?();
        Action onClicked = action2;
        NotificationSystem.Push(name, title, text, textId: "KeyBindingConflict", thumbnail: thumbnail, progressState: progressState, progress: progress, onClicked: onClicked);
      }
      else
      {
        string name = map.name;
        ProgressState? nullable = new ProgressState?(ProgressState.Complete);
        LocalizedString? title = new LocalizedString?();
        LocalizedString? text = new LocalizedString?();
        ProgressState? progressState = nullable;
        int? progress = new int?();
        NotificationSystem.Pop(name, 2f, title, text, textId: "KeyBindingConflictResolved", progressState: progressState, progress: progress);
      }
    }

    public void OnFocusChanged(bool hasFocus)
    {
      InputManager.log.DebugFormat("Has focus {0}", (object) hasFocus);
      this.m_HasFocus = hasFocus;
    }

    private void OnOverlayStateChanged(IOverlaySupport psi, bool active)
    {
      InputManager.log.DebugFormat("Overlay active {0}", (object) active);
      this.m_OverlayActive = active;
      if (!active)
        return;
      ReadOnlyArray<InputDevice>? source = this.m_Devices.Get();
      if (!source.HasValue)
        return;
      foreach (InputDevice device in ((IEnumerable) source).OfType<Keyboard>())
        UnityEngine.InputSystem.InputSystem.ResetDevice(device);
    }

    bool IInputActionCollection.Contains(InputAction action)
    {
      InputActionMap sourceMap = action?.actionMap;
      return sourceMap != null && this.m_Maps.Any<KeyValuePair<string, ProxyActionMap>>((Func<KeyValuePair<string, ProxyActionMap>, bool>) (m => m.Value.sourceMap == sourceMap));
    }

    void IInputActionCollection.Enable()
    {
      foreach (KeyValuePair<string, ProxyActionMap> map in this.m_Maps)
      {
        string str;
        ProxyActionMap proxyActionMap;
        map.Deconstruct(ref str, ref proxyActionMap);
        proxyActionMap.sourceMap.Enable();
      }
    }

    void IInputActionCollection.Disable()
    {
      foreach (KeyValuePair<string, ProxyActionMap> map in this.m_Maps)
      {
        string str;
        ProxyActionMap proxyActionMap;
        map.Deconstruct(ref str, ref proxyActionMap);
        proxyActionMap.sourceMap.Disable();
      }
    }

    IEnumerator<InputAction> IEnumerable<InputAction>.GetEnumerator()
    {
      return this.m_Maps.SelectMany<KeyValuePair<string, ProxyActionMap>, InputAction>((Func<KeyValuePair<string, ProxyActionMap>, IEnumerable<InputAction>>) (map => (IEnumerable<InputAction>) map.Value.sourceMap.actions)).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) ((IEnumerable<InputAction>) this).GetEnumerator();
    }

    internal void OnEnabledActionsChanged()
    {
      Action enabledActionsChanged = this.EventEnabledActionsChanged;
      if (enabledActionsChanged == null)
        return;
      enabledActionsChanged();
    }

    internal void OnActionMasksChanged()
    {
      Action actionMasksChanged = this.EventActionMasksChanged;
      if (actionMasksChanged == null)
        return;
      actionMasksChanged();
    }

    internal void OnActionDisplayNamesChanged()
    {
      Action displayNamesChanged = this.EventActionDisplayNamesChanged;
      if (displayNamesChanged == null)
        return;
      displayNamesChanged();
    }

    internal void OnPreResolvedActionChanged()
    {
      Action resolvedActionChanged = this.EventPreResolvedActionChanged;
      if (resolvedActionChanged == null)
        return;
      resolvedActionChanged();
    }

    internal static InputManager.DeferManagerUpdatingWrapper DeferUpdating()
    {
      InputManager.sDeferUpdatingWrapper.Acquire();
      return InputManager.sDeferUpdatingWrapper;
    }

    public void SetDefaultControlScheme()
    {
      this.activeControlScheme = InputManager.ControlScheme.KeyboardAndMouse;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
      if (change != InputDeviceChange.Added)
      {
        if (change != InputDeviceChange.Removed)
          return;
        this.OnRemoveDevice(device);
      }
      else
        this.OnAddDevice(device);
    }

    private void OnAddDevice(InputDevice device)
    {
      DeviceListener deviceListener;
      if (!this.m_DeviceListeners.TryGetValue(device, out deviceListener))
      {
        deviceListener = new DeviceListener(device, 50f);
        deviceListener.EventDeviceActivated.AddListener(new UnityAction<InputDevice>(this.OnDeviceActivated));
        this.m_DeviceListeners.Add(device, deviceListener);
        deviceListener.StartListening();
      }
      deviceListener.StartListening();
      this.TryPairDevice(device);
    }

    private void OnRemoveDevice(InputDevice device)
    {
      DeviceListener deviceListener;
      if (this.m_DeviceListeners.TryGetValue(device, out deviceListener))
        deviceListener.StopListening();
      if (!this.TryUnpairDevice(device) || (this.activeControlScheme != InputManager.ControlScheme.KeyboardAndMouse || !(device is Keyboard) && !(device is Mouse)) && (this.activeControlScheme != InputManager.ControlScheme.Gamepad || !(device is Gamepad)))
        return;
      Action deviceDisconnected = this.EventActiveDeviceDisconnected;
      if (deviceDisconnected == null)
        return;
      deviceDisconnected();
    }

    private void OnDeviceActivated(InputDevice newDevice)
    {
      if (newDevice != this.m_LastActiveDevice)
      {
        InputDevice lastActiveDevice = this.m_LastActiveDevice;
        InputManager.ControlScheme activeControlScheme = this.activeControlScheme;
        this.m_LastActiveDevice = newDevice;
        switch (newDevice)
        {
          case Mouse _:
          case Keyboard _:
            this.activeControlScheme = InputManager.ControlScheme.KeyboardAndMouse;
            break;
          case Gamepad _:
            this.activeControlScheme = InputManager.ControlScheme.Gamepad;
            break;
        }
        InputManager.ActiveDeviceChanged activeDeviceChanged = this.EventActiveDeviceChanged;
        if (activeDeviceChanged != null)
          activeDeviceChanged(newDevice, lastActiveDevice, this.activeControlScheme != activeControlScheme);
      }
      if (this.m_Idle)
      {
        this.m_Idle = false;
        Telemetry.InputIdleEnd();
      }
      this.m_AccumulatedIdleDelay = 0.0f;
      if (this.inputUser.pairedDevices.Contains<InputDevice>(newDevice))
        return;
      this.OnUnpairedDeviceUsed(newDevice);
    }

    private void OnUnpairedDeviceUsed(InputDevice device)
    {
      switch (device)
      {
        case Mouse _:
          this.UnpairAll<Mouse>();
          break;
        case Keyboard _:
          this.UnpairAll<Keyboard>();
          break;
        case Gamepad _:
          if (!PlatformManager.instance.IsDeviceAssociated(device))
            return;
          this.UnpairAll<Gamepad>();
          break;
      }
      this.PairDevice(device);
    }

    private void OnDeviceAssociationChanged(
      IPlatformServiceIntegration psi,
      DeviceAssociationChange change)
    {
      if (!PlatformManager.instance.IsPrincipalDeviceAssociationIntegration(psi))
        return;
      InputDevice device1 = UnityEngine.InputSystem.InputSystem.devices.FirstOrDefault<InputDevice>((Func<InputDevice, bool>) (device => device.deviceId == change.deviceId)) ?? UnityEngine.InputSystem.InputSystem.disconnectedDevices.FirstOrDefault<InputDevice>((Func<InputDevice, bool>) (device => device.deviceId == change.deviceId));
      if (device1 != null)
      {
        if (!change.associated)
        {
          if (!this.TryUnpairDevice(device1))
            return;
          Action deviceAssociationLost = this.EventActiveDeviceAssociationLost;
          if (deviceAssociationLost == null)
            return;
          deviceAssociationLost();
        }
        else
        {
          if (!change.associated)
            return;
          this.TryPairDevice(device1);
        }
      }
      else
        InputManager.log.Error((object) string.Format("No matching device found with ID: {0}.", (object) change.deviceId));
    }

    public void AddInitialDevices()
    {
      foreach (InputDevice device in UnityEngine.InputSystem.InputSystem.devices)
        this.OnAddDevice(device);
    }

    private bool TryPairDevice(InputDevice device)
    {
      if ((!(device is Mouse) || this.IsDeviceTypePaired<Mouse>()) && (!(device is Keyboard) || this.IsDeviceTypePaired<Keyboard>()) && (!(device is Gamepad) || this.IsDeviceTypePaired<Gamepad>() || !PlatformManager.instance.IsDeviceAssociated(device)))
        return false;
      this.PairDevice(device);
      return true;
    }

    private void PairDevice(InputDevice device)
    {
      InputManager.log.InfoFormat("Pair {0} [{1}]", (object) device.displayName, (object) device.deviceId);
      InputUser.PerformPairingWithDevice(device, this.inputUser);
      Action eventDevicePaired = this.EventDevicePaired;
      if (eventDevicePaired != null)
        eventDevicePaired();
      this.UpdateConnectedDeviceTypes();
    }

    private bool TryUnpairDevice(InputDevice device)
    {
      if (!this.inputUser.pairedDevices.Contains<InputDevice>(device) && !this.inputUser.lostDevices.Contains<InputDevice>(device))
        return false;
      this.UnpairDevice(device);
      return true;
    }

    private void UnpairDevice(InputDevice device)
    {
      InputManager.log.InfoFormat("Unpair {0} [{1}]", (object) device.displayName, (object) device.deviceId);
      this.inputUser.UnpairDevice(device);
      this.UpdateConnectedDeviceTypes();
    }

    private void UnpairAll<T>() where T : InputDevice
    {
      foreach (InputDevice device in this.inputUser.pairedDevices.Where<InputDevice>((Func<InputDevice, bool>) (x => x is T)))
        this.UnpairDevice(device);
      foreach (InputDevice device in this.inputUser.lostDevices.Where<InputDevice>((Func<InputDevice, bool>) (x => x is T)))
        this.UnpairDevice(device);
    }

    private bool IsDeviceTypePaired<T>() where T : InputDevice
    {
      InputUser inputUser = this.inputUser;
      if (inputUser.pairedDevices.Any<InputDevice>((Func<InputDevice, bool>) (d => d is T)))
        return true;
      inputUser = this.inputUser;
      return inputUser.lostDevices.Any<InputDevice>((Func<InputDevice, bool>) (d => d is T));
    }

    public static bool IsGamepadActive()
    {
      return InputManager.instance.activeControlScheme == InputManager.ControlScheme.Gamepad;
    }

    public static bool IsKeyboardConnected
    {
      get => (InputManager.instance.connectedDeviceTypes & InputManager.DeviceType.Keyboard) != 0;
    }

    public static bool IsMouseConnected
    {
      get => (InputManager.instance.connectedDeviceTypes & InputManager.DeviceType.Mouse) != 0;
    }

    public static bool IsGamepadConnected
    {
      get => (InputManager.instance.connectedDeviceTypes & InputManager.DeviceType.Gamepad) != 0;
    }

    private void UpdateConnectedDeviceTypes()
    {
      this.m_ConnectedDeviceTypes = this.inputUser.pairedDevices.Aggregate<InputDevice, InputManager.DeviceType>(InputManager.DeviceType.None, (Func<InputManager.DeviceType, InputDevice, InputManager.DeviceType>) ((result, device) =>
      {
        int num1 = (int) result;
        InputManager.DeviceType deviceType;
        switch (device)
        {
          case Keyboard _:
            deviceType = InputManager.DeviceType.Keyboard;
            break;
          case Mouse _:
            deviceType = InputManager.DeviceType.Mouse;
            break;
          case Gamepad _:
            deviceType = InputManager.DeviceType.Gamepad;
            break;
          default:
            deviceType = InputManager.DeviceType.None;
            break;
        }
        int num2 = (int) deviceType;
        return (InputManager.DeviceType) (num1 | num2);
      }));
    }

    public InputManager.GamepadType GetActiveGamepadType() => this.GetGamepadType(Gamepad.current);

    public InputManager.GamepadType GetGamepadType(Gamepad gamepad)
    {
      InputManager.GamepadType gamepadType;
      switch (gamepad)
      {
        case DualShockGamepad _:
          gamepadType = InputManager.GamepadType.PS;
          break;
        case XInputController _:
          gamepadType = InputManager.GamepadType.Xbox;
          break;
        default:
          gamepadType = InputManager.GamepadType.Xbox;
          break;
      }
      return gamepadType;
    }

    public void Initialize()
    {
      this.m_DeviceListeners = new System.Collections.Generic.Dictionary<InputDevice, DeviceListener>();
      using (PerformanceCounter.Start((Action<TimeSpan>) (t => InputManager.log.InfoFormat("Input initialized in {0}ms", (object) t.TotalMilliseconds))))
      {
        using (InputManager.DeferUpdating())
        {
          this.InitializeModifiers();
          foreach (KeyValuePair<string, ProxyActionMap> map in this.m_Maps)
          {
            string str;
            ProxyActionMap proxyActionMap;
            map.Deconstruct(ref str, ref proxyActionMap);
            proxyActionMap.InitActions();
          }
          this.InitializeMasks();
          this.InitializeAliases();
          this.InitializeLinkedActions();
        }
      }
      this.m_MouseToolBarrier = new InputBarrier("Mouse Tool", (IList<ProxyAction>) this.FindActionMap("Tool").actions.Values.Where<ProxyAction>((Func<ProxyAction, bool>) (a => a.isMouseAction)).ToArray<ProxyAction>(), InputManager.DeviceType.Mouse);
      this.inputUser = InputUser.CreateUserWithoutPairedDevices();
      this.AssociateActionsWithUser(true);
      UnityEngine.InputSystem.InputSystem.onDeviceChange += new Action<InputDevice, InputDeviceChange>(this.OnDeviceChange);
      this.AddInitialDevices();
      PlatformManager.instance.onDeviceAssociationChanged += new OnDeviceAssociationChangedEventHandler(this.OnDeviceAssociationChanged);
      PlatformManager.instance.onOverlayStateChanged += new Colossal.PSI.Common.OnOverlayStateChanged(this.OnOverlayStateChanged);
      this.m_ConflictResolution.Initialize();
    }

    private void InitializeModifiers()
    {
      foreach (InputAction inputAction in this.m_Maps.Values.SelectMany<ProxyActionMap, InputAction>((Func<ProxyActionMap, IEnumerable<InputAction>>) (map => (IEnumerable<InputAction>) map.sourceMap.actions)))
      {
        InputAction action = inputAction;
        action.ForEachCompositeOfAction((Func<InputActionSetupExtensions.BindingSyntax, bool>) (iterator =>
        {
          CompositeInstance compositeInstance = new CompositeInstance(NameAndParameters.Parse(iterator.binding.effectivePath));
          this.InitializeModifiers(iterator, action, compositeInstance);
          return true;
        }));
      }
    }

    private void InitializeModifiers(
      InputActionSetupExtensions.BindingSyntax compositeIterator,
      InputAction action,
      CompositeInstance compositeInstance)
    {
      if (!compositeInstance.allowModifiers)
        return;
      foreach (InputManager.CompositeComponentData compositeComponentData in compositeInstance.compositeData.m_Data.Values)
      {
        InputManager.CompositeComponentData componentData = compositeComponentData;
        action.ForEachPartOfCompositeWithName(compositeIterator, componentData.m_BindingName, (Func<InputActionSetupExtensions.BindingSyntax, bool>) (mainIterator =>
        {
          InputBinding binding1 = mainIterator.binding;
          HashSet<string> collection;
          if (!InputManager.kModifiers.TryGetValue(compositeIterator.binding.name.ToDeviceType(), out collection))
            return true;
          HashSet<string> missedModifiers = new HashSet<string>((IEnumerable<string>) collection, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
          InputActionSetupExtensions.BindingSyntax endIterator;
          action.ForEachPartOfCompositeWithName(mainIterator, componentData.m_ModifierName, (Func<InputActionSetupExtensions.BindingSyntax, bool>) (modifierIterator =>
          {
            InputBinding binding2 = modifierIterator.binding;
            if (!string.Equals(binding2.name, componentData.m_ModifierName, StringComparison.Ordinal) || string.IsNullOrEmpty(binding2.path))
              return true;
            missedModifiers.Remove(binding2.path);
            return true;
          }), out endIterator);
          foreach (string path in missedModifiers)
          {
            InputActionSetupExtensions.BindingSyntax bindingSyntax = endIterator.InsertPartBinding(componentData.m_ModifierName, path);
            bindingSyntax = bindingSyntax.WithGroups(binding1.groups);
            bindingSyntax = bindingSyntax.WithProcessor(InputManager.prohibitionModifierProcessor);
            endIterator = bindingSyntax.Triggering(action);
          }
          return true;
        }), out InputActionSetupExtensions.BindingSyntax _);
      }
    }

    private void InitializeMasks()
    {
      foreach (KeyValuePair<string, ProxyActionMap> map in this.m_Maps)
      {
        string str;
        ProxyActionMap proxyActionMap;
        map.Deconstruct(ref str, ref proxyActionMap);
        foreach (KeyValuePair<string, ProxyAction> action1 in (IEnumerable<KeyValuePair<string, ProxyAction>>) proxyActionMap.actions)
        {
          ProxyAction action2;
          action1.Deconstruct(ref str, ref action2);
          this.InitializeMasks(action2);
        }
      }
    }

    internal void InitializeMasks(ProxyAction action)
    {
      InputAction sourceAction = action.sourceAction;
      System.Type type;
      switch (sourceAction.expectedControlType)
      {
        case "Dpad":
          type = typeof (MaskVector2Processor);
          break;
        case "Stick":
          type = typeof (MaskVector2Processor);
          break;
        case "Vector2":
          type = typeof (MaskVector2Processor);
          break;
        case "Axis":
          type = typeof (MaskFloatProcessor);
          break;
        case "Button":
          type = typeof (MaskFloatProcessor);
          break;
        default:
          throw new ArgumentException("Unexpected type of control", "expectedControlType");
      }
      InternedString processorName = InputProcessor.s_Processors.FindNameForType(type);
      sourceAction.ForEachCompositeOfAction((Func<InputActionSetupExtensions.BindingSyntax, bool>) (iterator =>
      {
        NameAndParameters nameAndParameters = new NameAndParameters()
        {
          name = (string) processorName,
          parameters = new ReadOnlyArray<NamedValue>(new NamedValue[2]
          {
            NamedValue.From<int>("m_Index", action.m_GlobalIndex),
            NamedValue.From<InputManager.DeviceType>("m_Mask", iterator.binding.name.ToDeviceType())
          })
        };
        sourceAction.m_ActionMap.m_Bindings[iterator.m_BindingIndexInMap].processors = string.IsNullOrEmpty(iterator.binding.processors) ? nameAndParameters.ToString() : string.Format("{0}{1}{2}", (object) iterator.binding.processors, (object) ",", (object) nameAndParameters);
        sourceAction.m_ActionMap.OnBindingModified();
        return true;
      }));
    }

    private void InitializeAliases()
    {
      foreach (UIBaseInputAction inputAction in this.uiActionCollection.m_InputActions)
      {
        foreach (UIInputActionPart actionPart in (IEnumerable<UIInputActionPart>) inputAction.actionParts)
        {
          ProxyAction action;
          if (actionPart.TryGetProxyAction(out action))
            action.m_UIAliases.Add(inputAction);
        }
      }
      foreach (UIBaseInputAction inputAction in this.toolActionCollection.m_InputActions)
      {
        foreach (UIInputActionPart actionPart in (IEnumerable<UIInputActionPart>) inputAction.actionParts)
        {
          ProxyAction action;
          if (actionPart.TryGetProxyAction(out action))
            action.m_UIAliases.Add(inputAction);
        }
      }
    }

    private void InitializeLinkedActions()
    {
      foreach (KeyValuePair<string, ProxyActionMap> map in this.m_Maps)
      {
        string str;
        ProxyActionMap proxyActionMap;
        map.Deconstruct(ref str, ref proxyActionMap);
        foreach (KeyValuePair<string, ProxyAction> action1 in (IEnumerable<KeyValuePair<string, ProxyAction>>) proxyActionMap.actions)
        {
          ProxyAction proxyAction1;
          action1.Deconstruct(ref str, ref proxyAction1);
          ProxyAction action = proxyAction1;
          action.sourceAction.ForEachCompositeOfAction((Func<InputActionSetupExtensions.BindingSyntax, bool>) (iterator =>
          {
            InputManager.DeviceType deviceType = iterator.binding.name.ToDeviceType();
            if (deviceType == InputManager.DeviceType.None)
              return true;
            CompositeInstance compositeInstance = new CompositeInstance(NameAndParameters.Parse(iterator.binding.effectivePath));
            ProxyAction proxyAction2;
            if (compositeInstance.linkedGuid != Guid.Empty && this.TryFindAction(compositeInstance.linkedGuid, out proxyAction2))
            {
              ProxyAction.LinkInfo linkInfo = new ProxyAction.LinkInfo();
              linkInfo.m_Action = action;
              linkInfo.m_Device = deviceType;
              ProxyAction.LinkInfo action1_1 = linkInfo;
              linkInfo = new ProxyAction.LinkInfo();
              linkInfo.m_Action = proxyAction2;
              linkInfo.m_Device = deviceType;
              ProxyAction.LinkInfo action2 = linkInfo;
              ProxyAction.LinkActions(action1_1, action2);
            }
            return true;
          }));
        }
      }
    }

    internal void CreateCompositeBinding(InputAction action, ProxyComposite.Info info)
    {
      string composite = string.Format("{0}{1}{2}", (object) info.m_Source.parameters, (object) ';', (object) info.m_Source.usages.parameters);
      string interactions = string.Join<NameAndParameters>(";", (IEnumerable<NameAndParameters>) info.m_Source.interactions);
      string processors1 = string.Join<NameAndParameters>(";", (IEnumerable<NameAndParameters>) info.m_Source.processors);
      InputActionSetupExtensions.CompositeSyntax compositeSyntax = action.AddCompositeBinding(composite, interactions, processors1);
      new InputActionSetupExtensions.BindingSyntax(action.m_ActionMap, action.BindingIndexOnActionToBindingIndexOnMap(compositeSyntax.bindingIndex), action).WithName(info.m_Device.ToString());
      foreach (ProxyBinding binding in info.m_Bindings)
      {
        InputManager.CompositeComponentData data;
        if (info.m_Source.compositeData.TryGetData(binding.component, out data))
        {
          compositeSyntax.With(data.m_BindingName, binding.path, binding.device.ToString());
          HashSet<string> stringSet;
          if (info.m_Source.allowModifiers && InputManager.kModifiers.TryGetValue(binding.device, out stringSet))
          {
            foreach (string str in stringSet)
            {
              string supportedModifier = str;
              string processors2 = binding.modifiers.Any<ProxyModifier>((Func<ProxyModifier, bool>) (m => m.m_Path == supportedModifier)) ? string.Empty : InputManager.prohibitionModifierProcessor;
              compositeSyntax.With(data.m_ModifierName, supportedModifier, binding.device.ToString(), processors2);
            }
          }
        }
      }
    }

    public InputBarrier CreateGlobalBarrier(string barrierName)
    {
      return new InputBarrier(barrierName, (IList<ProxyActionMap>) this.m_Maps.Values.ToArray<ProxyActionMap>());
    }

    public InputBarrier CreateOverlayBarrier(string barrierName)
    {
      ProxyActionMap[] array = this.m_Maps.Values.Where<ProxyActionMap>((Func<ProxyActionMap, bool>) (actionMap => actionMap.name != "Engagement" && actionMap.name != "Splash screen")).ToArray<ProxyActionMap>();
      return new InputBarrier(barrierName, (IList<ProxyActionMap>) array, blocked: true);
    }

    public InputBarrier CreateMapBarrier(string map, string barrierName)
    {
      return new InputBarrier(barrierName, this.FindActionMap(map));
    }

    public InputBarrier CreateActionBarrier(string map, string name, string barrierName)
    {
      return new InputBarrier(barrierName, this.FindAction(map, name));
    }

    public ProxyActionMap FindActionMap(string name)
    {
      ProxyActionMap proxyActionMap;
      return !this.m_Maps.TryGetValue(name, out proxyActionMap) ? (ProxyActionMap) null : proxyActionMap;
    }

    public bool TryFindActionMap(string name, out ProxyActionMap map)
    {
      return this.m_Maps.TryGetValue(name, out map);
    }

    internal ProxyActionMap FindActionMap(InputActionMap map) => this.FindActionMap(map?.name);

    internal bool TryFindActionMap(InputActionMap map, out ProxyActionMap proxyMap)
    {
      return this.TryFindActionMap(map.name, out proxyMap);
    }

    private ProxyActionMap AddActionMap(string name)
    {
      using (InputManager.DeferUpdating())
      {
        InputActionMap sourceMap = new InputActionMap(name);
        sourceMap.GenerateId();
        ProxyActionMap proxyActionMap = new ProxyActionMap(sourceMap);
        this.m_Maps.Add(proxyActionMap.name, proxyActionMap);
        return proxyActionMap;
      }
    }

    private ProxyActionMap GetOrCreateMap(string name)
    {
      ProxyActionMap map;
      if (!this.TryFindActionMap(name, out map))
        map = this.AddActionMap(name);
      return map;
    }

    public void AssociateActionsWithUser(bool associate)
    {
      if (!this.inputUser.valid)
        return;
      if (associate)
        this.inputUser.AssociateActionsWithUser((IInputActionCollection) this);
      else
        this.inputUser.AssociateActionsWithUser((IInputActionCollection) null);
    }

    public readonly struct CompositeData
    {
      public readonly string m_Type;
      public readonly ActionType m_ActionType;
      public readonly IReadOnlyDictionary<ActionComponent, InputManager.CompositeComponentData> m_Data;

      public CompositeData(
        string type,
        ActionType actionType,
        InputManager.CompositeComponentData[] data)
      {
        this.m_Type = type;
        this.m_ActionType = actionType;
        this.m_Data = (IReadOnlyDictionary<ActionComponent, InputManager.CompositeComponentData>) new ReadOnlyDictionary<ActionComponent, InputManager.CompositeComponentData>((IDictionary<ActionComponent, InputManager.CompositeComponentData>) ((IEnumerable<InputManager.CompositeComponentData>) data).ToDictionary<InputManager.CompositeComponentData, ActionComponent>((Func<InputManager.CompositeComponentData, ActionComponent>) (d => d.m_Component)));
      }

      public bool TryGetData(
        ActionComponent component,
        out InputManager.CompositeComponentData data)
      {
        return this.m_Data.TryGetValue(component, out data);
      }

      public bool TryFindByBindingName(
        string bindingName,
        out InputManager.CompositeComponentData data)
      {
        foreach (InputManager.CompositeComponentData compositeComponentData in this.m_Data.Values)
        {
          if (compositeComponentData.m_BindingName == bindingName)
          {
            data = compositeComponentData;
            return true;
          }
        }
        data = new InputManager.CompositeComponentData();
        return false;
      }
    }

    public readonly struct CompositeComponentData
    {
      public static InputManager.CompositeComponentData defaultData = new InputManager.CompositeComponentData(ActionComponent.Press, "binding", "modifier");
      public readonly ActionComponent m_Component;
      public readonly string m_BindingName;
      public readonly string m_ModifierName;

      public CompositeComponentData(
        ActionComponent component,
        string bindingName,
        string modifierName)
      {
        this.m_Component = component;
        this.m_BindingName = bindingName;
        this.m_ModifierName = modifierName;
      }
    }

    public delegate void ActiveDeviceChanged(
      InputDevice newDevice,
      InputDevice oldDevice,
      bool schemeChanged);

    public enum PathType
    {
      Effective,
      Original,
      Overridden,
    }

    [Flags]
    public enum BindingOptions
    {
      None = 0,
      OnlyOriginal = 1,
      OnlyRebindable = 2,
      OnlyRebound = 4,
      OnlyBuiltIn = 8,
      ExcludeDummy = 16, // 0x00000010
      ExcludeHidden = 32, // 0x00000020
    }

    public enum ControlScheme : byte
    {
      KeyboardAndMouse,
      Gamepad,
    }

    [Flags]
    public enum DeviceType
    {
      None = 0,
      Keyboard = 1,
      Mouse = 2,
      Gamepad = 4,
      All = Gamepad | Mouse | Keyboard, // 0x00000007
    }

    public enum GamepadType
    {
      Xbox,
      PS,
    }

    internal class DeferManagerUpdatingWrapper : IDisposable
    {
      private static int sDeferUpdating;
      private readonly InputActionRebindingExtensions.DeferBindingResolutionWrapper m_BindingResolution;

      public bool isDeferred => InputManager.DeferManagerUpdatingWrapper.sDeferUpdating != 0;

      internal DeferManagerUpdatingWrapper()
      {
        this.m_BindingResolution = new InputActionRebindingExtensions.DeferBindingResolutionWrapper();
      }

      public void Acquire()
      {
        ++InputManager.DeferManagerUpdatingWrapper.sDeferUpdating;
        this.m_BindingResolution.Acquire();
        ProxyAction.sDeferUpdatingWrapper.Acquire();
      }

      public void Dispose()
      {
        this.m_BindingResolution.Dispose();
        if (InputActionMap.s_DeferBindingResolution == 0)
        {
          foreach (KeyValuePair<string, ProxyActionMap> map in InputManager.instance.m_Maps)
          {
            string str;
            ProxyActionMap proxyActionMap;
            map.Deconstruct(ref str, ref proxyActionMap);
            proxyActionMap.sourceMap.ResolveBindingsIfNecessary();
          }
        }
        ProxyAction.sDeferUpdatingWrapper.Dispose();
        if (InputManager.DeferManagerUpdatingWrapper.sDeferUpdating > 0)
          --InputManager.DeferManagerUpdatingWrapper.sDeferUpdating;
        if (InputManager.DeferManagerUpdatingWrapper.sDeferUpdating != 0)
          return;
        try
        {
          ++InputManager.DeferManagerUpdatingWrapper.sDeferUpdating;
          InputManager.instance.ProcessActionsUpdate(true);
        }
        finally
        {
          --InputManager.DeferManagerUpdatingWrapper.sDeferUpdating;
        }
      }
    }
  }
}
