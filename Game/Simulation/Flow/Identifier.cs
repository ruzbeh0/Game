// Decompiled with JetBrains decompiler
// Type: Game.Simulation.Flow.Identifier
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Simulation.Flow
{
  public struct Identifier : IEquatable<Identifier>
  {
    public int m_Index;
    public int m_Version;

    public static Identifier Null => new Identifier();

    public Identifier(int index, int version)
    {
      this.m_Index = index;
      this.m_Version = version;
    }

    public static bool operator ==(Identifier left, Identifier right)
    {
      return left.m_Index == right.m_Index && left.m_Version == right.m_Version;
    }

    public static bool operator !=(Identifier left, Identifier right) => !(left == right);

    public bool Equals(Identifier other)
    {
      return this.m_Index == other.m_Index && this.m_Version == other.m_Version;
    }

    public override bool Equals(object obj) => obj is Identifier other && this.Equals(other);

    public override int GetHashCode() => this.m_Index;
  }
}
