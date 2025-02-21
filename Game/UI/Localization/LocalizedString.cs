// Decompiled with JetBrains decompiler
// Type: Game.UI.Localization.LocalizedString
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.UI.Localization
{
  public readonly struct LocalizedString : ILocElement, IJsonWritable, IEquatable<LocalizedString>
  {
    [CanBeNull]
    public string id { get; }

    [CanBeNull]
    public string value { get; }

    [CanBeNull]
    public IReadOnlyDictionary<string, ILocElement> args { get; }

    public LocalizedString([CanBeNull] string id, [CanBeNull] string value, [CanBeNull] IReadOnlyDictionary<string, ILocElement> args)
    {
      this.id = id;
      this.value = value;
      this.args = args;
    }

    public bool isEmpty => this.id == null && this.value == null;

    public static LocalizedString Id(string id)
    {
      return new LocalizedString(id, (string) null, (IReadOnlyDictionary<string, ILocElement>) null);
    }

    public static LocalizedString Value(string value)
    {
      return new LocalizedString((string) null, value, (IReadOnlyDictionary<string, ILocElement>) null);
    }

    public static LocalizedString IdWithFallback(string id, string value)
    {
      return new LocalizedString(id, value, (IReadOnlyDictionary<string, ILocElement>) null);
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("id");
      writer.Write(this.id);
      writer.PropertyName("value");
      writer.Write(this.value);
      writer.PropertyName("args");
      writer.Write<ILocElement>(this.args);
      writer.TypeEnd();
    }

    public bool Equals(LocalizedString other)
    {
      return this.id == other.id && this.value == other.value && LocalizedString.ArgsEqual(this.args, other.args);
    }

    private static bool ArgsEqual(
      [CanBeNull] IReadOnlyDictionary<string, ILocElement> a,
      [CanBeNull] IReadOnlyDictionary<string, ILocElement> b)
    {
      if (object.Equals((object) a, (object) b))
        return true;
      return a != null && b != null && a.SequenceEqual<KeyValuePair<string, ILocElement>>((IEnumerable<KeyValuePair<string, ILocElement>>) b);
    }

    public override bool Equals(object obj) => obj is LocalizedString other && this.Equals(other);

    public override int GetHashCode()
    {
      return HashCode.Combine<string, string, IReadOnlyDictionary<string, ILocElement>>(this.id, this.value, this.args);
    }

    public static implicit operator LocalizedString(string id) => LocalizedString.Id(id);
  }
}
