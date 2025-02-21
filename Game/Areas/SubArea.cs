// Decompiled with JetBrains decompiler
// Type: Game.Areas.SubArea
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Areas
{
  [InternalBufferCapacity(0)]
  public struct SubArea : IBufferElementData, IEquatable<SubArea>, IEmptySerializable
  {
    public Entity m_Area;

    public SubArea(Entity area) => this.m_Area = area;

    public bool Equals(SubArea other) => this.m_Area.Equals(other.m_Area);

    public override int GetHashCode() => this.m_Area.GetHashCode();
  }
}
