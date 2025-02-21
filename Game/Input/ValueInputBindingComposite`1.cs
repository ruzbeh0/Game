// Decompiled with JetBrains decompiler
// Type: Game.Input.ValueInputBindingComposite`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

#nullable disable
namespace Game.Input
{
  public abstract class ValueInputBindingComposite<T> : InputBindingComposite<T>, ICustomComposite where T : struct
  {
    public bool m_IsRebindable = true;
    public bool m_IsModifiersRebindable = true;
    public bool m_AllowModifiers = true;
    public bool m_CanBeEmpty = true;
    public bool m_DeveloperOnly;
    public Platform m_Platform = Platform.All;
    public bool m_BuiltIn = true;
    public bool m_IsDummy;
    public bool m_IsHidden;
    public OptionGroupOverride m_OptionGroupOverride;
    public BuiltInUsages m_Usages = BuiltInUsages.DefaultTool | BuiltInUsages.Overlay | BuiltInUsages.Tool | BuiltInUsages.CancelableTool;
    public long m_LinkGuid1;
    public long m_LinkGuid2;

    public bool isRebindable => this.m_IsRebindable;

    public bool isModifiersRebindable => this.m_IsModifiersRebindable;

    public bool allowModifiers => this.m_AllowModifiers;

    public bool canBeEmpty => this.m_CanBeEmpty;

    public bool developerOnly => this.m_DeveloperOnly;

    public Platform platform => this.m_Platform;

    public bool builtIn => this.m_BuiltIn;

    public bool isDummy => this.m_IsDummy;

    public bool isHidden => this.m_IsHidden;

    public OptionGroupOverride optionGroupOverride => this.m_OptionGroupOverride;

    public Guid linkedGuid => CompositeUtility.GetGuid(this.m_LinkGuid1, this.m_LinkGuid2);

    public abstract string typeName { get; }

    protected virtual IEnumerable<NamedValue> GetParameters()
    {
      yield return NamedValue.From<bool>("m_IsRebindable", this.m_IsRebindable);
      yield return NamedValue.From<bool>("m_IsModifiersRebindable", this.m_IsModifiersRebindable);
      yield return NamedValue.From<bool>("m_AllowModifiers", this.m_AllowModifiers);
      yield return NamedValue.From<bool>("m_CanBeEmpty", this.m_CanBeEmpty);
      yield return NamedValue.From<bool>("m_DeveloperOnly", this.m_DeveloperOnly);
      yield return NamedValue.From<bool>("m_BuiltIn", this.m_BuiltIn);
      yield return NamedValue.From<bool>("m_IsDummy", this.m_IsDummy);
      yield return NamedValue.From<bool>("m_IsHidden", this.m_IsHidden);
      yield return NamedValue.From<OptionGroupOverride>("m_OptionGroupOverride", this.m_OptionGroupOverride);
    }

    public virtual NameAndParameters parameters
    {
      get
      {
        return new NameAndParameters()
        {
          name = this.typeName,
          parameters = new ReadOnlyArray<NamedValue>(this.GetParameters().ToArray<NamedValue>())
        };
      }
    }

    public Usages usages => new Usages(this.m_Usages);
  }
}
