// Decompiled with JetBrains decompiler
// Type: Game.UI.InputHintBindings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.UI
{
  public class InputHintBindings : CompositeBinding, IDisposable
  {
    private const string kGroup = "input";
    private static readonly string[] axisControls = new string[3]
    {
      "<Gamepad>/leftStick",
      "<Gamepad>/rightStick",
      "<Gamepad>/dpad"
    };
    private static readonly string[] allDirs = new string[4]
    {
      "/up",
      "/down",
      "/left",
      "/right"
    };
    private static readonly string[] horizontal = new string[2]
    {
      "/left",
      "/right"
    };
    private static readonly string[] vertical = new string[2]
    {
      "/up",
      "/down"
    };
    private static readonly string[] axes = new string[2]
    {
      "/x",
      "/y"
    };
    private readonly ValueBinding<InputHintBindings.InputHint[]> m_ActiveHintsBinding;
    private readonly GetterMapBinding<InputHintBindings.InputHintQuery, InputHintBindings.InputHint> m_HintsMapBinding;
    private readonly ValueBinding<int> m_GamepadTypeBinding;
    private readonly GetterMapBinding<InputHintBindings.TutorialInputHintQuery, InputHintBindings.InputHint[]> m_TutorialHints;
    private Dictionary<(string name, int priority), InputHintBindings.InputHint> m_Hints = new Dictionary<(string, int), InputHintBindings.InputHint>();
    private bool m_HintsDirty = true;
    private bool m_TutorialHintsDirty = true;
    private static Dictionary<IReadOnlyList<ProxyModifier>, List<ProxyBinding>> modifiersGroups = new Dictionary<IReadOnlyList<ProxyModifier>, List<ProxyBinding>>((IEqualityComparer<IReadOnlyList<ProxyModifier>>) new ProxyBinding.ModifiersListComparer(ProxyModifier.pathComparer));

    public event Action<ProxyAction> onInputHintPerformed;

    public InputHintBindings()
    {
      this.AddBinding((IBinding) (this.m_HintsMapBinding = new GetterMapBinding<InputHintBindings.InputHintQuery, InputHintBindings.InputHint>("input", "hints", new Func<InputHintBindings.InputHintQuery, InputHintBindings.InputHint>(this.GetInputHint), (IReader<InputHintBindings.InputHintQuery>) new ValueReader<InputHintBindings.InputHintQuery>(), (IWriter<InputHintBindings.InputHintQuery>) new ValueWriter<InputHintBindings.InputHintQuery>(), (IWriter<InputHintBindings.InputHint>) new ValueWriter<InputHintBindings.InputHint>())));
      this.AddBinding((IBinding) (this.m_ActiveHintsBinding = new ValueBinding<InputHintBindings.InputHint[]>("input", "activeHints", Array.Empty<InputHintBindings.InputHint>(), (IWriter<InputHintBindings.InputHint[]>) new ArrayWriter<InputHintBindings.InputHint>((IWriter<InputHintBindings.InputHint>) new ValueWriter<InputHintBindings.InputHint>()))));
      this.AddBinding((IBinding) (this.m_GamepadTypeBinding = new ValueBinding<int>("input", "gamepadType", (int) Game.Input.InputManager.instance.GetActiveGamepadType())));
      this.AddBinding((IBinding) (this.m_TutorialHints = new GetterMapBinding<InputHintBindings.TutorialInputHintQuery, InputHintBindings.InputHint[]>("input", "tutorialHints", new Func<InputHintBindings.TutorialInputHintQuery, InputHintBindings.InputHint[]>(InputHintBindings.GetTutorialHints), (IReader<InputHintBindings.TutorialInputHintQuery>) new ValueReader<InputHintBindings.TutorialInputHintQuery>(), (IWriter<InputHintBindings.TutorialInputHintQuery>) new ValueWriter<InputHintBindings.TutorialInputHintQuery>(), (IWriter<InputHintBindings.InputHint[]>) new ArrayWriter<InputHintBindings.InputHint>((IWriter<InputHintBindings.InputHint>) new ValueWriter<InputHintBindings.InputHint>()))));
      this.AddBinding((IBinding) new TriggerBinding<string>("input", "onInputHintPerformed", new Action<string>(this.HandleInputHintPerformed)));
      Game.Input.InputManager.instance.EventActionsChanged += new Action(this.OnActionsChanged);
      Game.Input.InputManager.instance.EventEnabledActionsChanged += new Action(this.OnEnabledActionsChanged);
      Game.Input.InputManager.instance.EventActionDisplayNamesChanged += new Action(this.OnActionDisplayNamesChanged);
      Game.Input.InputManager.instance.EventControlSchemeChanged += new Action<Game.Input.InputManager.ControlScheme>(this.OnControlSchemeChanged);
      Game.Input.InputManager.instance.EventActiveDeviceChanged += new Game.Input.InputManager.ActiveDeviceChanged(this.OnActiveDeviceChanged);
    }

    public void Dispose()
    {
      Game.Input.InputManager.instance.EventActionsChanged -= new Action(this.OnActionsChanged);
      Game.Input.InputManager.instance.EventEnabledActionsChanged -= new Action(this.OnEnabledActionsChanged);
      Game.Input.InputManager.instance.EventActionDisplayNamesChanged -= new Action(this.OnActionDisplayNamesChanged);
      Game.Input.InputManager.instance.EventControlSchemeChanged -= new Action<Game.Input.InputManager.ControlScheme>(this.OnControlSchemeChanged);
      Game.Input.InputManager.instance.EventActiveDeviceChanged -= new Game.Input.InputManager.ActiveDeviceChanged(this.OnActiveDeviceChanged);
    }

    private void OnActionsChanged()
    {
      this.m_HintsDirty = true;
      this.m_TutorialHintsDirty = true;
      this.m_HintsMapBinding.UpdateAll();
    }

    private void OnEnabledActionsChanged() => this.m_HintsDirty = true;

    private void OnActionDisplayNamesChanged() => this.m_HintsDirty = true;

    private void OnControlSchemeChanged(Game.Input.InputManager.ControlScheme controlScheme)
    {
      this.m_HintsDirty = true;
    }

    private void OnActiveDeviceChanged(
      InputDevice newDevice,
      InputDevice oldDevice,
      bool schemeChanged)
    {
      if (Game.Input.InputManager.instance.activeControlScheme != Game.Input.InputManager.ControlScheme.Gamepad)
        return;
      this.m_GamepadTypeBinding.Update((int) Game.Input.InputManager.instance.GetActiveGamepadType());
    }

    public override bool Update()
    {
      if (this.m_TutorialHintsDirty)
      {
        this.m_TutorialHints.Update();
        this.m_TutorialHintsDirty = false;
      }
      if (this.m_HintsDirty)
      {
        this.m_HintsDirty = false;
        this.RebuildHints();
        this.m_ActiveHintsBinding.Update(this.m_Hints.Values.OrderBy<InputHintBindings.InputHint, int>((Func<InputHintBindings.InputHint, int>) (h => h.priority)).ToArray<InputHintBindings.InputHint>());
      }
      return base.Update();
    }

    private void HandleInputHintPerformed(string action)
    {
      foreach (InputHintBindings.InputHint inputHint in this.m_Hints.Values)
      {
        if (inputHint.name == action)
        {
          Action<ProxyAction> inputHintPerformed = this.onInputHintPerformed;
          if (inputHintPerformed == null)
            break;
          inputHintPerformed(inputHint.action);
          break;
        }
      }
    }

    private void RebuildHints()
    {
      this.m_Hints.Clear();
      foreach (ProxyAction action in Game.Input.InputManager.instance.actions)
      {
        if (action.displayOverride != null)
          InputHintBindings.CollectHints(this.m_Hints, action, Game.Input.InputManager.instance.activeControlScheme);
      }
    }

    private static void CollectHints(
      Dictionary<(string name, int priority), InputHintBindings.InputHint> hints,
      ProxyAction action,
      Game.Input.InputManager.ControlScheme controlScheme,
      bool ignoreMask = false)
    {
      string str = action.displayOverride?.displayName ?? action.title;
      DisplayNameOverride displayOverride1 = action.displayOverride;
      int num1 = displayOverride1 != null ? displayOverride1.priority : -1;
      InputHintBindings.InputHint inputHint;
      if (!hints.TryGetValue((str, num1), out inputHint))
      {
        inputHint = InputHintBindings.InputHint.Create(action);
        hints[(str, num1)] = inputHint;
      }
      InputHintBindings.InputHint hint = inputHint;
      ProxyAction action1 = action;
      int num2 = (int) controlScheme;
      DisplayNameOverride displayOverride2 = action.displayOverride;
      int transform = displayOverride2 != null ? (int) displayOverride2.transform : 0;
      int num3 = ignoreMask ? 1 : 0;
      InputHintBindings.CollectHintItems(hint, action1, (Game.Input.InputManager.ControlScheme) num2, (UIBaseInputAction.Transform) transform, num3 != 0);
    }

    internal static void CollectHintItems(
      InputHintBindings.InputHint hint,
      ProxyAction action,
      Game.Input.InputManager.ControlScheme controlScheme,
      UIBaseInputAction.Transform transform,
      bool ignoreMask = true)
    {
      foreach (KeyValuePair<Game.Input.InputManager.DeviceType, ProxyComposite> composite in (IEnumerable<KeyValuePair<Game.Input.InputManager.DeviceType, ProxyComposite>>) action.composites)
      {
        Game.Input.InputManager.DeviceType deviceType;
        ProxyComposite proxyComposite1;
        composite.Deconstruct(ref deviceType, ref proxyComposite1);
        ProxyComposite proxyComposite2 = proxyComposite1;
        if (ignoreMask || (proxyComposite2.m_Device & action.mask) != Game.Input.InputManager.DeviceType.None)
        {
          InputHintBindings.modifiersGroups.Clear();
          ProxyBinding proxyBinding;
          foreach (KeyValuePair<ActionComponent, ProxyBinding> binding1 in (IEnumerable<KeyValuePair<ActionComponent, ProxyBinding>>) proxyComposite2.bindings)
          {
            ActionComponent actionComponent;
            binding1.Deconstruct(ref actionComponent, ref proxyBinding);
            ProxyBinding binding2 = proxyBinding;
            if (binding2.isSet && !binding2.isDummy && InputHintBindings.MatchesControlScheme(binding2, controlScheme) && (transform == UIBaseInputAction.Transform.None || (binding2.component.ToTransform() & transform) != UIBaseInputAction.Transform.None))
            {
              List<ProxyBinding> proxyBindingList;
              if (!InputHintBindings.modifiersGroups.TryGetValue(binding2.modifiers, out proxyBindingList))
              {
                proxyBindingList = new List<ProxyBinding>();
                InputHintBindings.modifiersGroups[binding2.modifiers] = proxyBindingList;
              }
              proxyBindingList.Add(binding2);
            }
          }
          foreach (KeyValuePair<IReadOnlyList<ProxyModifier>, List<ProxyBinding>> modifiersGroup in InputHintBindings.modifiersGroups)
          {
            IReadOnlyList<ProxyModifier> proxyModifierList1;
            List<ProxyBinding> proxyBindingList1;
            modifiersGroup.Deconstruct(ref proxyModifierList1, ref proxyBindingList1);
            IReadOnlyList<ProxyModifier> proxyModifierList2 = proxyModifierList1;
            List<ProxyBinding> proxyBindingList2 = proxyBindingList1;
            string[] paths = new string[proxyBindingList2.Count];
            for (int index1 = 0; index1 < proxyBindingList2.Count; ++index1)
            {
              string[] strArray = paths;
              int index2 = index1;
              proxyBinding = proxyBindingList2[index1];
              string path = proxyBinding.path;
              strArray[index2] = path;
            }
            InputHintBindings.SimplifyPaths(ref paths);
            ControlPath[] controlPathArray1 = new ControlPath[paths.Length];
            for (int index = 0; index < controlPathArray1.Length; ++index)
              controlPathArray1[index] = ControlPath.Get(paths[index]);
            ControlPath[] controlPathArray2 = new ControlPath[proxyModifierList2.Count];
            for (int index = 0; index < controlPathArray2.Length; ++index)
              controlPathArray2[index] = ControlPath.Get(proxyModifierList2[index].m_Path);
            hint.items.Add(new InputHintBindings.InputHintItem()
            {
              bindings = controlPathArray1,
              modifiers = controlPathArray2
            });
          }
        }
      }
    }

    private static InputHintBindings.InputHint[] GetTutorialHints(
      InputHintBindings.TutorialInputHintQuery query)
    {
      switch (query.action)
      {
        case "Rotate Mouse":
          query.action = "Rotate";
          break;
        case "Zoom Mouse":
          query.action = "Zoom";
          break;
        case "Tool Options":
          query.map = "Navigation";
          query.action = "Secondary Action";
          break;
        case "Cancel":
          if (query.map == "Tool")
          {
            query.map = "Navigation";
            query.action = "Back";
            break;
          }
          break;
      }
      ProxyAction action = Game.Input.InputManager.instance.FindAction(query.map, query.action);
      if (action == null)
        return Array.Empty<InputHintBindings.InputHint>();
      if (query.controlScheme == Game.Input.InputManager.ControlScheme.Gamepad)
      {
        Dictionary<(string, int), InputHintBindings.InputHint> hints = new Dictionary<(string, int), InputHintBindings.InputHint>();
        InputHintBindings.CollectHints(hints, action, query.controlScheme, true);
        return hints.Values.OrderBy<InputHintBindings.InputHint, int>((Func<InputHintBindings.InputHint, int>) (h => h.priority)).ToArray<InputHintBindings.InputHint>();
      }
      if (query.index < 0)
        return action.bindings.Where<ProxyBinding>((Func<ProxyBinding, bool>) (b => b.isSet && InputHintBindings.MatchesControlScheme(b, query.controlScheme))).Select<ProxyBinding, InputHintBindings.InputHint>((Func<ProxyBinding, InputHintBindings.InputHint>) (b => new InputHintBindings.InputHint(action)
        {
          name = action.title,
          items = {
            new InputHintBindings.InputHintItem()
            {
              bindings = new ControlPath[1]
              {
                ControlPath.Get(b.path)
              },
              modifiers = b.modifiers.Select<ProxyModifier, ControlPath>((Func<ProxyModifier, ControlPath>) (m => ControlPath.Get(m.m_Path))).ToArray<ControlPath>()
            }
          }
        })).ToArray<InputHintBindings.InputHint>();
      ProxyBinding proxyBinding = action.bindings.Where<ProxyBinding>((Func<ProxyBinding, bool>) (b => InputHintBindings.MatchesControlScheme(b, query.controlScheme))).Skip<ProxyBinding>(query.index).FirstOrDefault<ProxyBinding>();
      return new InputHintBindings.InputHint[1]
      {
        new InputHintBindings.InputHint(action)
        {
          name = action.title,
          items = {
            new InputHintBindings.InputHintItem()
            {
              bindings = new ControlPath[1]
              {
                ControlPath.Get(proxyBinding.path)
              },
              modifiers = proxyBinding.modifiers.Select<ProxyModifier, ControlPath>((Func<ProxyModifier, ControlPath>) (m => ControlPath.Get(m.m_Path))).ToArray<ControlPath>()
            }
          }
        }
      };
    }

    private InputHintBindings.InputHint GetInputHint(InputHintBindings.InputHintQuery query)
    {
      InputHintBindings.InputHint inputHint;
      if (this.m_HintsMapBinding.values.TryGetValue(query, out inputHint) && inputHint.version == Game.Input.InputManager.instance.actionVersion)
        return inputHint;
      UIBaseInputAction[] inputActions = Game.Input.InputManager.instance.uiActionCollection.m_InputActions;
      UIBaseInputAction uiBaseInputAction = (UIBaseInputAction) null;
      for (int index = 0; index < inputActions.Length; ++index)
      {
        if (inputActions[index].aliasName == query.action)
        {
          uiBaseInputAction = inputActions[index];
          break;
        }
      }
      if ((UnityEngine.Object) uiBaseInputAction == (UnityEngine.Object) null)
        return (InputHintBindings.InputHint) null;
      InputHintBindings.InputHint hint = new InputHintBindings.InputHint((ProxyAction) null)
      {
        name = uiBaseInputAction.aliasName,
        priority = uiBaseInputAction.displayPriority,
        show = true
      };
      foreach (UIInputActionPart actionPart in (IEnumerable<UIInputActionPart>) uiBaseInputAction.actionParts)
      {
        ProxyAction proxyAction;
        if (Game.Input.InputManager.instance.TryFindAction((InputAction) actionPart.m_Action, out proxyAction))
          InputHintBindings.CollectHintItems(hint, proxyAction, query.controlScheme, actionPart.m_Transform);
      }
      return hint;
    }

    private static bool MatchesControlScheme(
      ProxyBinding binding,
      Game.Input.InputManager.ControlScheme controlScheme)
    {
      if (controlScheme == Game.Input.InputManager.ControlScheme.Gamepad && binding.isGamepad)
        return true;
      if (controlScheme != Game.Input.InputManager.ControlScheme.KeyboardAndMouse)
        return false;
      return binding.isKeyboard || binding.isMouse;
    }

    private static void SimplifyPaths(ref string[] paths)
    {
      for (int index = 0; index < InputHintBindings.axisControls.Length; ++index)
      {
        string axisControl = InputHintBindings.axisControls[index];
        if (InputHintBindings.MatchesDirections(paths, axisControl, InputHintBindings.allDirs) || InputHintBindings.MatchesDirections(paths, axisControl, InputHintBindings.axes))
        {
          paths = new string[1]{ axisControl };
          break;
        }
        if (InputHintBindings.MatchesDirections(paths, axisControl, InputHintBindings.horizontal))
        {
          paths = new string[1]{ axisControl + "/x" };
          break;
        }
        if (InputHintBindings.MatchesDirections(paths, axisControl, InputHintBindings.vertical))
        {
          paths = new string[1]{ axisControl + "/y" };
          break;
        }
      }
    }

    private static bool MatchesDirections(string[] bindings, string basePath, string[] dirs)
    {
      if (bindings.Length != dirs.Length)
        return false;
      for (int index1 = 0; index1 < dirs.Length; ++index1)
      {
        string dir = dirs[index1];
        bool flag = false;
        for (int index2 = 0; index2 < bindings.Length; ++index2)
        {
          string binding = bindings[index2];
          if (binding.Length == basePath.Length + dir.Length && binding.StartsWith(basePath) && binding.EndsWith(dir))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
      }
      return true;
    }

    internal class InputHint : IJsonWritable
    {
      public readonly ProxyAction action;
      public readonly int version = Game.Input.InputManager.instance.actionVersion;
      public string name;
      public int priority;
      public bool show;
      public readonly List<InputHintBindings.InputHintItem> items = new List<InputHintBindings.InputHintItem>();

      public InputHint(ProxyAction action) => this.action = action;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().Name);
        writer.PropertyName("name");
        writer.Write(this.name);
        writer.PropertyName("items");
        writer.Write<InputHintBindings.InputHintItem>((IList<InputHintBindings.InputHintItem>) this.items);
        writer.PropertyName("show");
        writer.Write(this.show);
        writer.TypeEnd();
      }

      public static InputHintBindings.InputHint Create(ProxyAction action)
      {
        DisplayNameOverride displayOverride = action.displayOverride;
        if (displayOverride != null)
          return new InputHintBindings.InputHint(action)
          {
            name = displayOverride.displayName,
            priority = displayOverride.priority,
            show = displayOverride.active
          };
        return new InputHintBindings.InputHint(action)
        {
          name = action.title,
          priority = -1,
          show = false
        };
      }
    }

    internal class InputHintItem : IJsonWritable
    {
      public ControlPath[] bindings;
      public ControlPath[] modifiers;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().Name);
        writer.PropertyName("bindings");
        writer.Write<ControlPath>((IList<ControlPath>) this.bindings);
        writer.PropertyName("modifiers");
        writer.Write<ControlPath>((IList<ControlPath>) this.modifiers);
        writer.TypeEnd();
      }
    }

    private struct TutorialInputHintQuery : IJsonReadable, IJsonWritable
    {
      public string map;
      public string action;
      public int index;
      public Game.Input.InputManager.ControlScheme controlScheme;

      public void Read(IJsonReader reader)
      {
        long num1 = (long) reader.ReadMapBegin();
        reader.ReadProperty("map");
        reader.Read(out this.map);
        reader.ReadProperty("action");
        reader.Read(out this.action);
        reader.ReadProperty("controlScheme");
        int num2;
        reader.Read(out num2);
        this.controlScheme = (Game.Input.InputManager.ControlScheme) num2;
        reader.ReadProperty("index");
        reader.Read(out this.index);
        reader.ReadMapEnd();
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(typeof (InputHintBindings.TutorialInputHintQuery).FullName);
        writer.PropertyName("map");
        writer.Write(this.map);
        writer.PropertyName("action");
        writer.Write(this.action);
        writer.PropertyName("controlScheme");
        writer.Write((int) this.controlScheme);
        writer.PropertyName("index");
        writer.Write(this.index);
        writer.TypeEnd();
      }
    }

    private struct InputHintQuery : 
      IJsonReadable,
      IJsonWritable,
      IEquatable<InputHintBindings.InputHintQuery>
    {
      public string action;
      public Game.Input.InputManager.ControlScheme controlScheme;

      public void Read(IJsonReader reader)
      {
        long num1 = (long) reader.ReadMapBegin();
        reader.ReadProperty("action");
        reader.Read(out this.action);
        reader.ReadProperty("controlScheme");
        int num2;
        reader.Read(out num2);
        this.controlScheme = (Game.Input.InputManager.ControlScheme) num2;
        reader.ReadMapEnd();
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(typeof (InputHintBindings.InputHintQuery).FullName);
        writer.PropertyName("action");
        writer.Write(this.action);
        writer.PropertyName("controlScheme");
        writer.Write((int) this.controlScheme);
        writer.TypeEnd();
      }

      public bool Equals(InputHintBindings.InputHintQuery other)
      {
        return other.action == this.action && other.controlScheme == this.controlScheme;
      }
    }
  }
}
