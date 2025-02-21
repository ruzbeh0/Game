// Decompiled with JetBrains decompiler
// Type: Game.Input.Usages
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem.Utilities;

#nullable disable
namespace Game.Input
{
  public struct Usages : IEnumerable<int>, IEnumerable, IEquatable<Usages>
  {
    public const string kMenuUsage = "Menu";
    public const string kDefaultUsage = "DefaultTool";
    public const string kOverlayUsage = "Overlay";
    public const string kToolUsage = "Tool";
    public const string kCancelableToolUsage = "CancelableTool";
    public const string kDebugUsage = "Debug";
    public const string kEditorUsage = "Editor";
    public const string kPhotoModeUsage = "PhotoMode";
    public const string kOptionsUsage = "Options";
    public const string kTutorialUsage = "Tutorial";
    public const string kDiscardableToolUsage = "DiscardableTool";
    private ulong[] m_Value;
    private bool m_ReadOnly;

    internal static Dictionary<string, int> usagesMap { get; } = new Dictionary<string, int>();

    public static Usages defaultUsages { get; } = new Usages(BuiltInUsages.DefaultSet);

    public static Usages empty => new Usages();

    public bool this[int index]
    {
      get
      {
        int index1 = index >> 6;
        return this.m_Value != null && this.m_Value.Length >= index1 && (this.m_Value[index1] & (ulong) (1L << index)) > 0UL;
      }
      set
      {
        if (this.m_ReadOnly)
          throw new InvalidOperationException("Value is readonly");
        int index1 = index >> 6;
        if (value)
        {
          if (this.m_Value == null)
            this.m_Value = new ulong[index1 + 1];
          else if (this.m_Value.Length <= index1)
            Array.Resize<ulong>(ref this.m_Value, index1 + 1);
          this.m_Value[index1] |= (ulong) (1L << index);
        }
        else
        {
          if (this.m_Value == null || this.m_Value.Length <= index1)
            return;
          this.m_Value[index1] &= (ulong) ~(1L << index);
        }
      }
    }

    public bool this[string usage]
    {
      get
      {
        int index;
        return Usages.usagesMap.TryGetValue(usage, out index) && this[index];
      }
      set => this[Usages.AddOrGetUsage(usage)] = value;
    }

    public bool isReadOnly => this.m_ReadOnly;

    public bool isNone
    {
      get
      {
        if (this.m_Value == null)
          return true;
        for (int index = 0; index < this.m_Value.Length; ++index)
        {
          if (this.m_Value[index] != 0UL)
            return false;
        }
        return true;
      }
    }

    public NameAndParameters parameters
    {
      get
      {
        return new NameAndParameters()
        {
          name = nameof (Usages),
          parameters = new ReadOnlyArray<NamedValue>(this.Select<int, NamedValue>((Func<int, NamedValue>) (u => NamedValue.From<bool>(u.ToString(), true))).ToArray<NamedValue>())
        };
      }
      set
      {
        foreach (NamedValue parameter in value.parameters)
        {
          int result;
          if (int.TryParse(parameter.name, out result))
            this[result] = parameter.value.ToBoolean();
        }
      }
    }

    public Usages(int length = 0, bool readOnly = true)
    {
      this.m_Value = length == 0 ? Array.Empty<ulong>() : new ulong[length];
      this.m_ReadOnly = readOnly;
    }

    public Usages(bool readOnly = true, params int[] values)
      : this(((IEnumerable<int>) values).Max() >> 6, false)
    {
      foreach (int index in values)
        this[index] = true;
      this.m_ReadOnly = readOnly;
    }

    public Usages(BuiltInUsages usages, bool readOnly = true)
      : this(0, false)
    {
      this["Menu"] = (usages & BuiltInUsages.Menu) != 0;
      this["DefaultTool"] = (usages & BuiltInUsages.DefaultTool) != 0;
      this["Overlay"] = (usages & BuiltInUsages.Overlay) != 0;
      this["Tool"] = (usages & BuiltInUsages.Tool) != 0;
      this["CancelableTool"] = (usages & BuiltInUsages.CancelableTool) != 0;
      this["Debug"] = (usages & BuiltInUsages.Debug) != 0;
      this["Editor"] = (usages & BuiltInUsages.Editor) != 0;
      this["PhotoMode"] = (usages & BuiltInUsages.PhotoMode) != 0;
      this["Options"] = (usages & BuiltInUsages.Options) != 0;
      this["Tutorial"] = (usages & BuiltInUsages.Tutorial) != 0;
      this["DiscardableTool"] = (usages & BuiltInUsages.DiscardableTool) != 0;
      this.m_ReadOnly = readOnly;
    }

    internal Usages(bool readOnly = true, params string[] customUsages)
      : this(0, false)
    {
      if (customUsages != null)
      {
        foreach (string customUsage in customUsages)
          this[customUsage] = true;
      }
      this.m_ReadOnly = readOnly;
    }

    public Usages Copy(bool readOnly = true)
    {
      if (this.m_ReadOnly & readOnly)
        return this;
      Usages usages = new Usages(this.m_Value.Length, readOnly);
      Array.Copy((Array) this.m_Value, (Array) usages.m_Value, this.m_Value.Length);
      return usages;
    }

    public void SetFrom(Usages source)
    {
      if (this.m_ReadOnly)
        throw new InvalidOperationException("Value is readonly");
      if (this.m_Value == null)
      {
        if (source.m_Value == null || source.m_Value.Length == 0)
        {
          this.m_Value = Array.Empty<ulong>();
          return;
        }
        Array.Resize<ulong>(ref this.m_Value, source.m_Value.Length);
      }
      Array.Copy((Array) source.m_Value, (Array) this.m_Value, this.m_Value.Length);
    }

    internal void MakeReadOnly() => this.m_ReadOnly = true;

    internal void MakeEditable() => this.m_ReadOnly = false;

    public static bool TestAny(Usages usages1, Usages usages2)
    {
      ulong[] numArray1 = usages1.m_Value ?? Array.Empty<ulong>();
      ulong[] numArray2 = usages2.m_Value ?? Array.Empty<ulong>();
      int num = Math.Max(numArray1.Length, numArray2.Length);
      for (int index = 0; index < num; ++index)
      {
        if (((index < numArray1.Length ? (long) numArray1[index] : 0L) & (index < numArray2.Length ? (long) numArray2[index] : 0L)) != 0L)
          return true;
      }
      return false;
    }

    public static bool TestAll(Usages usages1, Usages usages2)
    {
      ulong[] numArray1 = usages1.m_Value ?? Array.Empty<ulong>();
      ulong[] numArray2 = usages2.m_Value ?? Array.Empty<ulong>();
      int num = Math.Max(numArray1.Length, numArray2.Length);
      for (int index = 0; index < num; ++index)
      {
        if ((index < numArray1.Length ? (long) numArray1[index] : 0L) != (index < numArray2.Length ? (long) numArray2[index] : 0L))
          return false;
      }
      return true;
    }

    public static Usages Intersect(Usages usages1, Usages usages2, bool readOnly = true)
    {
      ulong[] numArray1 = usages1.m_Value ?? Array.Empty<ulong>();
      ulong[] numArray2 = usages2.m_Value ?? Array.Empty<ulong>();
      int length = Math.Max(numArray1.Length, numArray2.Length);
      Usages usages = new Usages(length, readOnly);
      for (int index = 0; index < length; ++index)
        usages.m_Value[index] = (ulong) ((index < numArray1.Length ? (long) numArray1[index] : 0L) & (index < numArray2.Length ? (long) numArray2[index] : 0L));
      return usages;
    }

    public static Usages Combine(Usages usages1, Usages usages2, bool readOnly = true)
    {
      ulong[] numArray1 = usages1.m_Value ?? Array.Empty<ulong>();
      ulong[] numArray2 = usages2.m_Value ?? Array.Empty<ulong>();
      int length = Math.Max(numArray1.Length, numArray2.Length);
      Usages usages = new Usages(length, readOnly);
      for (int index = 0; index < length; ++index)
        usages.m_Value[index] = (ulong) ((index < numArray1.Length ? (long) numArray1[index] : 0L) | (index < numArray2.Length ? (long) numArray2[index] : 0L));
      return usages;
    }

    internal static int AddOrGetUsage(string usageName)
    {
      int count;
      if (!Usages.usagesMap.TryGetValue(usageName, out count))
      {
        count = Usages.usagesMap.Count;
        Usages.usagesMap[usageName] = count;
      }
      return count;
    }

    IEnumerator<int> IEnumerable<int>.GetEnumerator() => this.Enumerate().GetEnumerator();

    public IEnumerator GetEnumerator() => (IEnumerator) this.Enumerate().GetEnumerator();

    private IEnumerable<int> Enumerate()
    {
      if (this.m_Value != null)
      {
        for (int i = 0; i < this.m_Value.Length; ++i)
        {
          for (int j = 0; j < 64; ++j)
          {
            if (((long) this.m_Value[i] & 1L << j) != 0L)
              yield return (i << 6) + j;
          }
        }
      }
    }

    public bool Equals(Usages other) => Usages.Comparer.defaultComparer.Equals(this, other);

    public override string ToString()
    {
      return this.m_Value != null ? string.Join<int>('|', (IEnumerable<int>) this) : "Empty";
    }

    public class Comparer : IEqualityComparer<Usages>
    {
      public static readonly Usages.Comparer defaultComparer = new Usages.Comparer();

      public bool Equals(Usages x, Usages y)
      {
        ulong[] numArray1 = x.m_Value ?? Array.Empty<ulong>();
        ulong[] numArray2 = y.m_Value ?? Array.Empty<ulong>();
        int num = Math.Max(numArray1.Length, numArray2.Length);
        for (int index = 0; index < num; ++index)
        {
          if ((index < numArray1.Length ? (long) numArray1[index] : 0L) != (index < numArray2.Length ? (long) numArray2[index] : 0L))
            return false;
        }
        return true;
      }

      public int GetHashCode(Usages usages)
      {
        HashCode hashCode = new HashCode();
        if (usages.m_Value != null)
        {
          for (int index = 0; index < usages.m_Value.Length; ++index)
            hashCode.Add<ulong>(usages.m_Value[index]);
        }
        return hashCode.ToHashCode();
      }
    }
  }
}
