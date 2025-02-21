// Decompiled with JetBrains decompiler
// Type: Game.Input.ProxyComposite
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Game.Input
{
  [DebuggerDisplay("{m_Device}")]
  public class ProxyComposite
  {
    private readonly CompositeInstance m_Source;
    public readonly InputManager.DeviceType m_Device;
    public readonly ActionType m_Type;
    internal readonly HashSet<ProxyAction> m_LinkedActions = new HashSet<ProxyAction>();
    private readonly Dictionary<ActionComponent, ProxyBinding> m_Bindings = new Dictionary<ActionComponent, ProxyBinding>();

    public IReadOnlyDictionary<ActionComponent, ProxyBinding> bindings
    {
      get => (IReadOnlyDictionary<ActionComponent, ProxyBinding>) this.m_Bindings;
    }

    public bool isSet
    {
      get
      {
        return this.m_Bindings.Any<KeyValuePair<ActionComponent, ProxyBinding>>((Func<KeyValuePair<ActionComponent, ProxyBinding>, bool>) (b => b.Value.isSet));
      }
    }

    public bool isBuildIn => this.m_Source.builtIn;

    internal bool isDummy => this.m_Source.isDummy;

    internal bool isHidden => this.m_Source.isHidden;

    public bool isRebindable => this.m_Source.isRebindable;

    public bool isModifiersRebindable => this.m_Source.isModifiersRebindable;

    public bool allowModifiers => this.m_Source.allowModifiers;

    public bool canBeEmpty => this.m_Source.canBeEmpty;

    public bool developerOnly => this.m_Source.developerOnly;

    public Usages usage => this.m_Source.usages;

    internal ProxyComposite(
      InputManager.DeviceType device,
      ActionType type,
      CompositeInstance source,
      IList<ProxyBinding> bindings)
    {
      this.m_Device = device;
      this.m_Type = type;
      this.m_Source = source;
      foreach (ProxyBinding binding in (IEnumerable<ProxyBinding>) bindings)
        this.m_Bindings[binding.component] = binding;
    }

    public bool TryGetBinding(ProxyBinding sampleBinding, out ProxyBinding foundBinding)
    {
      return this.m_Bindings.TryGetValue(sampleBinding.component, out foundBinding);
    }

    public bool TryGetBinding(ActionComponent component, out ProxyBinding foundBinding)
    {
      return this.m_Bindings.TryGetValue(component, out foundBinding);
    }

    public override string ToString()
    {
      return string.Format("{0} ({1})", (object) this.m_Device, (object) this.m_Type);
    }

    public struct Info
    {
      public InputManager.DeviceType m_Device;
      public CompositeInstance m_Source;
      public List<ProxyBinding> m_Bindings;
    }
  }
}
