// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZoneBuiltDataKey
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct ZoneBuiltDataKey : IEquatable<ZoneBuiltDataKey>
  {
    public Entity m_Zone;
    public int m_Level;

    public bool Equals(ZoneBuiltDataKey other)
    {
      return this.m_Zone.Equals(other.m_Zone) && this.m_Level.Equals(other.m_Level);
    }

    public override int GetHashCode()
    {
      return (17 * 31 + this.m_Zone.GetHashCode()) * 31 + this.m_Level.GetHashCode();
    }
  }
}
