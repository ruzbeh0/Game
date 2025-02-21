// Decompiled with JetBrains decompiler
// Type: Game.Input.ProxyBinding
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Json;
using Colossal.UI.Binding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.Input
{
  public struct ProxyBinding : IEquatable<ProxyBinding>, IJsonWritable
  {
    public static readonly ProxyBinding.Comparer pathAndModifiersComparer = new ProxyBinding.Comparer(ProxyBinding.Comparer.Options.Path | ProxyBinding.Comparer.Options.Modifiers, new ProxyBinding.ModifiersListComparer(ProxyModifier.pathComparer));
    public static readonly ProxyBinding.Comparer onlyPathComparer = new ProxyBinding.Comparer(ProxyBinding.Comparer.Options.Path);
    internal static readonly ProxyBinding.Comparer componentComparer = new ProxyBinding.Comparer(ProxyBinding.Comparer.Options.Device | ProxyBinding.Comparer.Options.Component);
    [Include]
    private string m_MapName;
    [Include]
    private string m_ActionName;
    [Include]
    private ActionComponent m_Component;
    [Include]
    private string m_Name;
    [Include]
    [DecodeAlias(new string[] {"group", "m_Group"})]
    private InputManager.DeviceType m_Device;
    [Include]
    private string m_Path;
    [Include]
    [DiscardDefaultArrayExtraItems]
    private ProxyModifier[] m_Modifiers;
    [Exclude]
    private string m_OriginalPath;
    [Exclude]
    private ProxyModifier[] m_OriginalModifiers;
    [Exclude]
    private CompositeInstance m_Source;
    [Exclude]
    private UIBaseInputAction m_Alies;
    private int m_HasConflictVersion;
    private ProxyBinding.ConflictType m_HasConflicts;
    private int m_ConflictVersion;
    private IList<ProxyBinding> m_Conflicts;

    private static void SupportValueTypesForAOT() => JSON.SupportTypeForAOT<ProxyBinding>();

    public static ProxyBinding.Comparer defaultComparer => ProxyBinding.Comparer.defaultComparer;

    public static ProxyBinding.ModifiersListComparer defaultModifiersComparer
    {
      get => ProxyBinding.ModifiersListComparer.defaultComparer;
    }

    public string mapName => this.m_MapName;

    public string actionName => this.m_ActionName;

    public ActionComponent component => this.m_Component;

    public string name => this.m_Name;

    internal ProxyAction action
    {
      get => InputManager.instance.FindAction(this.m_MapName, this.m_ActionName);
    }

    public bool isBuiltIn => this.m_Source != null && this.m_Source.builtIn;

    public bool isRebindable => this.m_Source != null && this.m_Source.isRebindable;

    public bool isModifiersRebindable
    {
      get => this.m_Source != null && this.m_Source.isModifiersRebindable;
    }

    public bool allowModifiers => this.m_Source != null && this.m_Source.allowModifiers;

    public bool canBeEmpty => this.m_Source != null && this.m_Source.canBeEmpty;

    public bool developerOnly => this.m_Source != null && this.m_Source.developerOnly;

    internal bool isDummy => this.m_Source != null && this.m_Source.isDummy;

    internal bool isHidden
    {
      get
      {
        if (this.m_Alies != null)
          return !this.m_Alies.showInOptions;
        return this.m_Source != null && this.m_Source.isHidden;
      }
    }

    internal OptionGroupOverride optionGroupOverride
    {
      get
      {
        OptionGroupOverride optionGroupOverride = this.m_Source != null ? this.m_Source.optionGroupOverride : OptionGroupOverride.None;
        if (this.m_Alies != null && optionGroupOverride == OptionGroupOverride.None)
          optionGroupOverride = this.m_Alies.optionGroupOverride;
        return optionGroupOverride;
      }
    }

    public Usages usages
    {
      get
      {
        CompositeInstance source = this.m_Source;
        return source == null ? Usages.empty : __nonvirtual (source.usages);
      }
    }

    public ProxyBinding original
    {
      get
      {
        return this.Copy() with
        {
          m_Path = this.m_OriginalPath ?? this.m_Path,
          m_Modifiers = this.m_OriginalModifiers ?? this.m_Modifiers
        };
      }
    }

    public bool isOriginal
    {
      get
      {
        return this.m_Path == this.m_OriginalPath && ProxyBinding.defaultModifiersComparer.Equals((IReadOnlyCollection<ProxyModifier>) this.m_Modifiers, (IReadOnlyCollection<ProxyModifier>) this.m_OriginalModifiers);
      }
    }

    public ProxyBinding.ConflictType hasConflicts
    {
      get
      {
        if (this.m_HasConflictVersion == InputManager.instance.actionVersion)
          return this.m_HasConflicts;
        this.m_HasConflictVersion = InputManager.instance.actionVersion;
        this.m_HasConflicts = ProxyBinding.ConflictType.None;
        HashSet<ProxyAction> proxyActionSet;
        ProxyAction action;
        if (!this.isSet || !InputManager.instance.keyActionMap.TryGetValue(this.path, out proxyActionSet) || !InputManager.instance.TryFindAction(this.m_MapName, this.m_ActionName, out action))
          return this.m_HasConflicts;
        foreach (ProxyAction action2 in proxyActionSet)
        {
          foreach (KeyValuePair<InputManager.DeviceType, ProxyComposite> composite in (IEnumerable<KeyValuePair<InputManager.DeviceType, ProxyComposite>>) action2.composites)
          {
            InputManager.DeviceType deviceType;
            ProxyComposite proxyComposite1;
            composite.Deconstruct(ref deviceType, ref proxyComposite1);
            ProxyComposite proxyComposite2 = proxyComposite1;
            if (!proxyComposite2.isDummy)
            {
              bool flag = InputManager.CanConflict(action, action2, proxyComposite2.m_Device);
              foreach (KeyValuePair<ActionComponent, ProxyBinding> binding in (IEnumerable<KeyValuePair<ActionComponent, ProxyBinding>>) proxyComposite2.bindings)
              {
                ActionComponent actionComponent;
                ProxyBinding proxyBinding;
                binding.Deconstruct(ref actionComponent, ref proxyBinding);
                ProxyBinding y = proxyBinding;
                if ((flag || !ProxyBinding.componentComparer.Equals(this, y)) && ProxyBinding.ConflictsWith(this, y, true))
                  this.m_HasConflicts |= action2.isBuiltIn ? ProxyBinding.ConflictType.WithBuiltIn : ProxyBinding.ConflictType.WithNotBuiltIn;
              }
            }
          }
        }
        return this.m_HasConflicts;
      }
    }

    public IList<ProxyBinding> conflicts
    {
      get
      {
        if (this.m_ConflictVersion == InputManager.instance.actionVersion)
          return this.m_Conflicts;
        this.m_ConflictVersion = InputManager.instance.actionVersion;
        this.m_Conflicts = (IList<ProxyBinding>) Array.Empty<ProxyBinding>();
        HashSet<ProxyAction> proxyActionSet;
        ProxyAction action;
        if (!this.isSet || !InputManager.instance.keyActionMap.TryGetValue(this.path, out proxyActionSet) || !InputManager.instance.TryFindAction(this.m_MapName, this.m_ActionName, out action))
          return this.m_Conflicts;
        this.m_Conflicts = (IList<ProxyBinding>) new List<ProxyBinding>();
        foreach (ProxyAction action2 in proxyActionSet)
        {
          foreach (KeyValuePair<InputManager.DeviceType, ProxyComposite> composite in (IEnumerable<KeyValuePair<InputManager.DeviceType, ProxyComposite>>) action2.composites)
          {
            InputManager.DeviceType deviceType;
            ProxyComposite proxyComposite1;
            composite.Deconstruct(ref deviceType, ref proxyComposite1);
            ProxyComposite proxyComposite2 = proxyComposite1;
            if (!proxyComposite2.isDummy)
            {
              bool flag = InputManager.CanConflict(action, action2, proxyComposite2.m_Device);
              foreach (KeyValuePair<ActionComponent, ProxyBinding> binding in (IEnumerable<KeyValuePair<ActionComponent, ProxyBinding>>) proxyComposite2.bindings)
              {
                ActionComponent actionComponent;
                ProxyBinding proxyBinding;
                binding.Deconstruct(ref actionComponent, ref proxyBinding);
                ProxyBinding y = proxyBinding;
                if ((flag || !ProxyBinding.componentComparer.Equals(this, y)) && ProxyBinding.ConflictsWith(this, y, true))
                  this.m_Conflicts.Add(y);
              }
            }
          }
        }
        return this.m_Conflicts;
      }
    }

    [Exclude]
    public string path
    {
      get => this.m_Path;
      set
      {
        this.m_Path = value;
        this.ResetConflictCache();
      }
    }

    [Exclude]
    public IReadOnlyList<ProxyModifier> modifiers
    {
      get
      {
        return (IReadOnlyList<ProxyModifier>) this.m_Modifiers ?? (IReadOnlyList<ProxyModifier>) (this.m_Modifiers = Array.Empty<ProxyModifier>());
      }
      set
      {
        this.SetModifiers(out this.m_Modifiers, value);
        this.ResetConflictCache();
      }
    }

    [Exclude]
    public string originalPath
    {
      get => this.m_OriginalPath;
      set
      {
        this.m_OriginalPath = value;
        this.ResetConflictCache();
      }
    }

    [Exclude]
    public IReadOnlyList<ProxyModifier> originalModifiers
    {
      get
      {
        return (IReadOnlyList<ProxyModifier>) this.m_OriginalModifiers ?? (IReadOnlyList<ProxyModifier>) (this.m_OriginalModifiers = this.m_Modifiers ?? Array.Empty<ProxyModifier>());
      }
      set
      {
        this.SetModifiers(out this.m_OriginalModifiers, value);
        this.ResetConflictCache();
      }
    }

    [Exclude]
    [Obsolete("Use device instead. It will be removed eventually")]
    public string group
    {
      get => this.m_Device.ToString();
      set => this.m_Device = value.ToDeviceType();
    }

    [Exclude]
    public InputManager.DeviceType device
    {
      get => this.m_Device;
      set => this.m_Device = value;
    }

    public bool isKeyboard => (this.device & InputManager.DeviceType.Keyboard) != 0;

    public bool isMouse => (this.device & InputManager.DeviceType.Mouse) != 0;

    public bool isGamepad => (this.device & InputManager.DeviceType.Gamepad) != 0;

    public bool isSet => !string.IsNullOrEmpty(this.m_Path);

    [Exclude]
    internal UIBaseInputAction alies
    {
      get => this.m_Alies;
      set => this.m_Alies = value;
    }

    internal bool isAlias => this.m_Alies != null;

    public string title
    {
      get
      {
        if (this.isAlias)
          return this.m_Alies.aliasName + "/" + this.m_Name?.ToLower();
        return this.m_MapName + "/" + this.m_ActionName + "/" + this.m_Name?.ToLower();
      }
    }

    public ProxyBinding(
      string mapName,
      string actionName,
      ActionComponent component,
      string name,
      CompositeInstance source)
    {
      this.m_MapName = mapName;
      this.m_ActionName = actionName;
      this.m_Component = component;
      this.m_Name = name;
      this.m_Source = source;
      this.m_Device = InputManager.DeviceType.None;
      this.m_Path = string.Empty;
      this.m_Modifiers = Array.Empty<ProxyModifier>();
      this.m_OriginalPath = (string) null;
      this.m_OriginalModifiers = (ProxyModifier[]) null;
      this.m_Alies = (UIBaseInputAction) null;
      this.m_HasConflicts = ProxyBinding.ConflictType.None;
      this.m_Conflicts = (IList<ProxyBinding>) Array.Empty<ProxyBinding>();
      this.m_ConflictVersion = -1;
      this.m_HasConflictVersion = -1;
    }

    public ProxyBinding(
      InputAction action,
      ActionComponent component,
      string name,
      CompositeInstance source)
      : this(action.actionMap.name, action.name, component, name, source)
    {
    }

    public ProxyBinding Copy()
    {
      return new ProxyBinding()
      {
        m_Source = this.m_Source,
        m_Component = this.m_Component,
        m_MapName = this.m_MapName,
        m_ActionName = this.m_ActionName,
        m_Name = this.m_Name,
        m_Device = this.m_Device,
        m_Path = this.m_Path,
        modifiers = this.modifiers,
        m_OriginalPath = this.m_OriginalPath,
        m_OriginalModifiers = this.m_OriginalModifiers,
        m_Alies = this.m_Alies,
        m_HasConflicts = this.m_HasConflicts,
        m_Conflicts = this.m_Conflicts.Count == 0 ? (IList<ProxyBinding>) Array.Empty<ProxyBinding>() : (IList<ProxyBinding>) this.m_Conflicts.ToArray<ProxyBinding>(),
        m_ConflictVersion = this.m_ConflictVersion,
        m_HasConflictVersion = this.m_HasConflictVersion
      };
    }

    public static bool ConflictsWith(ProxyBinding x, ProxyBinding y, bool checkUsage)
    {
      return x.isSet && y.isSet && x.m_Device == y.m_Device && (!x.allowModifiers || !y.allowModifiers ? ProxyBinding.onlyPathComparer : ProxyBinding.pathAndModifiersComparer).Equals(x, y) && (!checkUsage || Usages.TestAny(x.usages, y.usages));
    }

    public IEnumerable<string> ToHumanReadablePath()
    {
      if (!string.IsNullOrEmpty(this.m_Path))
      {
        ProxyModifier[] proxyModifierArray = this.m_Modifiers;
        for (int index = 0; index < proxyModifierArray.Length; ++index)
          yield return ControlPath.ToHumanReadablePath(proxyModifierArray[index].m_Path);
        proxyModifierArray = (ProxyModifier[]) null;
        yield return ControlPath.ToHumanReadablePath(this.m_Path);
      }
    }

    public ProxyBinding WithPath(string newPath)
    {
      this.path = newPath;
      return this;
    }

    public ProxyBinding WithModifiers(IReadOnlyList<ProxyModifier> newModifiers)
    {
      this.modifiers = newModifiers;
      return this;
    }

    internal ProxyBinding.Watcher CreateWatcher(Action<ProxyBinding> onChange = null)
    {
      return new ProxyBinding.Watcher(this, onChange);
    }

    private void SetModifiers(out ProxyModifier[] field, IReadOnlyList<ProxyModifier> value)
    {
      if (value == null || value.Count == 0)
        field = Array.Empty<ProxyModifier>();
      else if (value.Count == 1)
      {
        field = value.ToArray<ProxyModifier>();
      }
      else
      {
        field = value.Distinct<ProxyModifier>((IEqualityComparer<ProxyModifier>) ProxyModifier.pathComparer).ToArray<ProxyModifier>();
        Array.Sort<ProxyModifier>(field, (IComparer<ProxyModifier>) ProxyModifier.pathComparer);
      }
    }

    public void ResetConflictCache()
    {
      this.m_ConflictVersion = -1;
      this.m_HasConflictVersion = -1;
    }

    internal string GetOptionsGroup()
    {
      if (this.optionGroupOverride == OptionGroupOverride.None)
        return this.mapName;
      FieldInfo field = typeof (OptionGroupOverride).GetField(this.optionGroupOverride.ToString());
      if (field == (FieldInfo) null)
        return this.mapName;
      DescriptionAttribute descriptionAttribute = field.GetCustomAttributes(false).OfType<DescriptionAttribute>().FirstOrDefault<DescriptionAttribute>();
      return descriptionAttribute == null ? this.mapName : descriptionAttribute.Description;
    }

    public override string ToString()
    {
      return string.Format("{0}/{1}/{2} - [{3}] ({4})", (object) this.m_MapName, (object) this.m_ActionName, (object) this.m_Name, string.IsNullOrEmpty(this.m_Path) ? (object) "Not set" : (object) string.Join(" + ", ((IEnumerable<ProxyModifier>) this.m_Modifiers).Select<ProxyModifier, string>((Func<ProxyModifier, string>) (m => m.m_Path)).Append<string>(this.m_Path)), (object) this.usages);
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(typeof (ProxyBinding).FullName);
      writer.PropertyName("binding");
      writer.Write<ControlPath>(ControlPath.Get(this.m_Path));
      writer.PropertyName("modifiers");
      writer.Write<ControlPath>((IList<ControlPath>) ((IEnumerable<ProxyModifier>) this.m_Modifiers).Select<ProxyModifier, ControlPath>((Func<ProxyModifier, ControlPath>) (m => ControlPath.Get(m.m_Path))).ToArray<ControlPath>());
      writer.PropertyName("name");
      writer.Write(this.m_Name);
      writer.PropertyName("map");
      writer.Write(this.m_MapName);
      writer.PropertyName("action");
      writer.Write(this.m_ActionName);
      writer.PropertyName("title");
      writer.Write(this.title);
      writer.PropertyName("optionGroup");
      writer.Write(this.GetOptionsGroup());
      writer.PropertyName("device");
      writer.Write(this.device.ToString());
      writer.PropertyName("isBuiltIn");
      writer.Write(this.isBuiltIn);
      writer.PropertyName("canBeEmpty");
      writer.Write(this.canBeEmpty);
      writer.PropertyName("isRebindable");
      writer.Write(this.isRebindable);
      writer.PropertyName("isOriginal");
      writer.Write(this.isOriginal);
      writer.PropertyName("allowModifiers");
      writer.Write(this.allowModifiers && this.isModifiersRebindable);
      writer.PropertyName("hasConflicts");
      writer.Write((int) this.hasConflicts);
      writer.TypeEnd();
    }

    public bool Equals(ProxyBinding other)
    {
      return ProxyBinding.Comparer.defaultComparer.Equals(this, other);
    }

    public override int GetHashCode() => ProxyBinding.Comparer.defaultComparer.GetHashCode(this);

    public static bool PathEquals(ProxyBinding x, ProxyBinding y)
    {
      return (!x.allowModifiers || !y.allowModifiers ? ProxyBinding.onlyPathComparer : ProxyBinding.pathAndModifiersComparer).Equals(x, y);
    }

    public override bool Equals(object obj)
    {
      return obj is ProxyBinding y && ProxyBinding.Comparer.defaultComparer.Equals(this, y);
    }

    public static bool operator ==(ProxyBinding left, ProxyBinding right)
    {
      return ProxyBinding.Comparer.defaultComparer.Equals(left, right);
    }

    public static bool operator !=(ProxyBinding left, ProxyBinding right)
    {
      return !ProxyBinding.Comparer.defaultComparer.Equals(left, right);
    }

    public class Comparer : IEqualityComparer<ProxyBinding>
    {
      public static readonly ProxyBinding.Comparer defaultComparer = new ProxyBinding.Comparer();
      private readonly ProxyBinding.ModifiersListComparer m_ModifiersListComparer;
      public readonly ProxyBinding.Comparer.Options m_Options;

      public Comparer(
        ProxyBinding.Comparer.Options options = ProxyBinding.Comparer.Options.MapName | ProxyBinding.Comparer.Options.ActionName | ProxyBinding.Comparer.Options.Name | ProxyBinding.Comparer.Options.Device,
        ProxyBinding.ModifiersListComparer modifiersListComparer = null)
      {
        this.m_Options = options;
        this.m_ModifiersListComparer = modifiersListComparer ?? ProxyBinding.ModifiersListComparer.defaultComparer;
      }

      public bool Equals(ProxyBinding x, ProxyBinding y)
      {
        return ((this.m_Options & ProxyBinding.Comparer.Options.MapName) == (ProxyBinding.Comparer.Options) 0 || !(x.m_MapName != y.m_MapName)) && ((this.m_Options & ProxyBinding.Comparer.Options.ActionName) == (ProxyBinding.Comparer.Options) 0 || !(x.m_ActionName != y.m_ActionName)) && ((this.m_Options & ProxyBinding.Comparer.Options.Name) == (ProxyBinding.Comparer.Options) 0 || !(x.m_Name != y.m_Name)) && ((this.m_Options & ProxyBinding.Comparer.Options.Path) == (ProxyBinding.Comparer.Options) 0 || !(x.m_Path != y.m_Path)) && ((this.m_Options & ProxyBinding.Comparer.Options.Modifiers) == (ProxyBinding.Comparer.Options) 0 || this.m_ModifiersListComparer.Equals((IReadOnlyCollection<ProxyModifier>) x.m_Modifiers, (IReadOnlyCollection<ProxyModifier>) y.m_Modifiers)) && ((this.m_Options & ProxyBinding.Comparer.Options.Usages) == (ProxyBinding.Comparer.Options) 0 || Usages.Comparer.defaultComparer.Equals(x.usages, y.usages)) && ((this.m_Options & ProxyBinding.Comparer.Options.Device) == (ProxyBinding.Comparer.Options) 0 || x.m_Device == y.m_Device) && ((this.m_Options & ProxyBinding.Comparer.Options.Component) == (ProxyBinding.Comparer.Options) 0 || x.m_Component == y.m_Component);
      }

      public int GetHashCode(ProxyBinding obj)
      {
        HashCode hashCode = new HashCode();
        if ((this.m_Options & ProxyBinding.Comparer.Options.MapName) != (ProxyBinding.Comparer.Options) 0)
          hashCode.Add<string>(obj.m_MapName);
        if ((this.m_Options & ProxyBinding.Comparer.Options.ActionName) != (ProxyBinding.Comparer.Options) 0)
          hashCode.Add<string>(obj.m_ActionName);
        if ((this.m_Options & ProxyBinding.Comparer.Options.Name) != (ProxyBinding.Comparer.Options) 0)
          hashCode.Add<string>(obj.m_Name);
        if ((this.m_Options & ProxyBinding.Comparer.Options.Path) != (ProxyBinding.Comparer.Options) 0)
          hashCode.Add<string>(obj.m_Path);
        if ((this.m_Options & ProxyBinding.Comparer.Options.Modifiers) != (ProxyBinding.Comparer.Options) 0)
          hashCode.Add<IReadOnlyCollection<ProxyModifier>>((IReadOnlyCollection<ProxyModifier>) obj.m_Modifiers, (IEqualityComparer<IReadOnlyCollection<ProxyModifier>>) this.m_ModifiersListComparer);
        if ((this.m_Options & ProxyBinding.Comparer.Options.Usages) != (ProxyBinding.Comparer.Options) 0)
          hashCode.Add<Usages>(obj.usages, (IEqualityComparer<Usages>) Usages.Comparer.defaultComparer);
        if ((this.m_Options & ProxyBinding.Comparer.Options.Device) != (ProxyBinding.Comparer.Options) 0)
          hashCode.Add<InputManager.DeviceType>(obj.m_Device);
        if ((this.m_Options & ProxyBinding.Comparer.Options.Component) != (ProxyBinding.Comparer.Options) 0)
          hashCode.Add<ActionComponent>(obj.m_Component);
        return hashCode.ToHashCode();
      }

      [Flags]
      public enum Options
      {
        MapName = 1,
        ActionName = 2,
        Name = 4,
        Path = 8,
        Modifiers = 16, // 0x00000010
        Usages = 32, // 0x00000020
        Device = 64, // 0x00000040
        Component = 128, // 0x00000080
      }
    }

    public class ModifiersListComparer : IEqualityComparer<IReadOnlyCollection<ProxyModifier>>
    {
      public static readonly ProxyBinding.ModifiersListComparer defaultComparer = new ProxyBinding.ModifiersListComparer();
      private readonly ProxyModifier.Comparer m_ModifierComparer;

      public ModifiersListComparer(ProxyModifier.Comparer modifierComparer = null)
      {
        this.m_ModifierComparer = modifierComparer ?? ProxyModifier.Comparer.defaultComparer;
      }

      public bool Equals(IReadOnlyCollection<ProxyModifier> x, IReadOnlyCollection<ProxyModifier> y)
      {
        if (x == null)
          return y == null;
        if (y == null || x.Count != y.Count)
          return false;
        if (x.Count == 0 && y.Count == 0)
          return true;
        foreach (ProxyModifier proxyModifier in (IEnumerable<ProxyModifier>) x)
        {
          if (!y.Contains<ProxyModifier>(proxyModifier, (IEqualityComparer<ProxyModifier>) this.m_ModifierComparer))
            return false;
        }
        foreach (ProxyModifier proxyModifier in (IEnumerable<ProxyModifier>) y)
        {
          if (!x.Contains<ProxyModifier>(proxyModifier, (IEqualityComparer<ProxyModifier>) this.m_ModifierComparer))
            return false;
        }
        return true;
      }

      public int GetHashCode(IReadOnlyCollection<ProxyModifier> list) => list.Count.GetHashCode();
    }

    public class Watcher : IDisposable
    {
      private static readonly ProxyBinding.Comparer comparer = new ProxyBinding.Comparer(ProxyBinding.Comparer.Options.Path | ProxyBinding.Comparer.Options.Modifiers | ProxyBinding.Comparer.Options.Usages, new ProxyBinding.ModifiersListComparer(ProxyModifier.pathComparer));
      private bool m_Disposed;
      private ProxyBinding m_Binding;
      private readonly ProxyAction m_Action;
      private readonly Action<ProxyBinding> m_OnChange;

      public ProxyBinding binding => this.m_Binding;

      public bool isValid => this.m_Action != null;

      public Watcher(ProxyBinding binding, Action<ProxyBinding> onChange = null)
      {
        this.m_Binding = binding;
        this.m_Action = binding.action;
        this.m_OnChange = onChange;
        if (this.m_Action == null)
          return;
        this.m_Action.onChanged += new Action<ProxyAction>(this.OnChanged);
        this.OnChanged(this.m_Action);
      }

      private void OnChanged(ProxyAction action)
      {
        ProxyBinding foundBinding;
        if (!this.m_Action.TryGetBinding(this.m_Binding, out foundBinding) || ProxyBinding.Watcher.comparer.Equals(this.m_Binding, foundBinding))
          return;
        this.m_Binding = foundBinding;
        Action<ProxyBinding> onChange = this.m_OnChange;
        if (onChange == null)
          return;
        onChange(foundBinding);
      }

      public void Dispose()
      {
        if (this.m_Disposed)
          return;
        this.m_Disposed = true;
        if (this.m_Action == null)
          return;
        this.m_Action.onChanged -= new Action<ProxyAction>(this.OnChanged);
      }
    }

    [Flags]
    public enum ConflictType
    {
      None = 0,
      WithBuiltIn = 1,
      WithNotBuiltIn = 2,
      All = WithNotBuiltIn | WithBuiltIn, // 0x00000003
    }
  }
}
