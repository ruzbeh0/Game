// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.PathSegment
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using System;

#nullable disable
namespace Game.UI.Widgets
{
  public struct PathSegment : IJsonWritable, IJsonReadable, IEquatable<PathSegment>
  {
    [CanBeNull]
    public string m_Key;
    public int m_Index;

    public static PathSegment Empty
    {
      get => new PathSegment() { m_Index = -1 };
    }

    public PathSegment([NotNull] string key)
    {
      this.m_Key = key;
      this.m_Index = -1;
    }

    public PathSegment(int index)
    {
      this.m_Key = (string) null;
      this.m_Index = index;
    }

    public void Write(IJsonWriter writer)
    {
      if (this.m_Key != null)
        writer.Write(this.m_Key);
      else if (this.m_Index != -1)
        writer.Write(this.m_Index);
      else
        writer.WriteNull();
    }

    public void Read(IJsonReader reader)
    {
      switch (reader.PeekValueType())
      {
        case cohtml.Net.ValueType.Number:
          this.m_Key = (string) null;
          reader.Read(out this.m_Index);
          break;
        case cohtml.Net.ValueType.String:
          reader.Read(out this.m_Key);
          this.m_Index = -1;
          break;
        default:
          reader.SkipValue();
          this.m_Key = (string) null;
          this.m_Index = -1;
          break;
      }
    }

    public bool Equals(PathSegment other)
    {
      return this.m_Key == other.m_Key && this.m_Index == other.m_Index;
    }

    public override bool Equals(object obj) => obj is PathSegment other && this.Equals(other);

    public override int GetHashCode()
    {
      return (this.m_Key != null ? this.m_Key.GetHashCode() : 0) * 397 ^ this.m_Index;
    }

    public static bool operator ==(PathSegment left, PathSegment right) => left.Equals(right);

    public static bool operator !=(PathSegment left, PathSegment right) => !left.Equals(right);

    public override string ToString() => this.m_Key ?? this.m_Index.ToString();

    public static implicit operator PathSegment(string key) => new PathSegment(key);

    public static implicit operator PathSegment(int index) => new PathSegment(index);
  }
}
