// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AffiliatedBrandElement
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct AffiliatedBrandElement : IBufferElementData, IComparable<AffiliatedBrandElement>
  {
    public Entity m_Brand;

    public int CompareTo(AffiliatedBrandElement other) => this.m_Brand.Index - other.m_Brand.Index;
  }
}
