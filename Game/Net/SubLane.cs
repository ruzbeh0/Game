// Decompiled with JetBrains decompiler
// Type: Game.Net.SubLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Pathfind;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  [InternalBufferCapacity(0)]
  public struct SubLane : IBufferElementData, IEquatable<SubLane>, IEmptySerializable
  {
    public Entity m_SubLane;
    public PathMethod m_PathMethods;

    public SubLane(Entity lane, PathMethod pathMethods)
    {
      this.m_SubLane = lane;
      this.m_PathMethods = pathMethods;
    }

    public bool Equals(SubLane other) => this.m_SubLane.Equals(other.m_SubLane);

    public override int GetHashCode() => this.m_SubLane.GetHashCode();
  }
}
