// Decompiled with JetBrains decompiler
// Type: Game.Areas.AreaSearchItem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Entities;

#nullable disable
namespace Game.Areas
{
  public struct AreaSearchItem : IEquatable<AreaSearchItem>
  {
    public Entity m_Area;
    public int m_Triangle;

    public AreaSearchItem(Entity area, int triangle)
    {
      this.m_Area = area;
      this.m_Triangle = triangle;
    }

    public bool Equals(AreaSearchItem other)
    {
      return this.m_Area.Equals(other.m_Area) & this.m_Triangle.Equals(other.m_Triangle);
    }

    public override int GetHashCode()
    {
      return (17 * 31 + this.m_Area.GetHashCode()) * 31 + this.m_Triangle.GetHashCode();
    }
  }
}
