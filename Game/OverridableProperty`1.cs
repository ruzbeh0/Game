// Decompiled with JetBrains decompiler
// Type: Game.OverridableProperty`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Diagnostics;

#nullable disable
namespace Game
{
  [DebuggerDisplay("{m_Value} ({m_OverrideState})")]
  public class OverridableProperty<T>
  {
    public const string k_DebuggerDisplay = "{m_Value} ({m_OverrideState})";
    private readonly Func<T> m_SynchronizeFunc;
    private T m_Value;
    private T m_OverrideValue;
    private bool m_OverrideState;

    public T value
    {
      get => this.m_Value;
      set => this.m_Value = value;
    }

    public T overrideValue
    {
      get => this.m_OverrideValue;
      set
      {
        this.m_OverrideState = true;
        this.m_OverrideValue = value;
      }
    }

    public bool overrideState
    {
      get => this.m_OverrideState;
      set => this.m_OverrideState = value;
    }

    public OverridableProperty(Func<T> synchronizeFunc = null)
      : this(default (T), false)
    {
      this.m_SynchronizeFunc = synchronizeFunc;
    }

    private OverridableProperty(T value, bool overrideState)
    {
      this.m_Value = value;
      this.m_OverrideState = overrideState;
    }

    public static implicit operator T(OverridableProperty<T> prop)
    {
      if (prop.m_OverrideState)
        return prop.m_OverrideValue;
      return prop.m_SynchronizeFunc == null ? prop.m_Value : prop.m_SynchronizeFunc();
    }

    public override string ToString() => ((T) this).ToString();
  }
}
