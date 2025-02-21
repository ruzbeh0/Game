// Decompiled with JetBrains decompiler
// Type: Game.Routes.RouteSegment
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Routes
{
  [InternalBufferCapacity(0)]
  public struct RouteSegment : IBufferElementData, IEmptySerializable
  {
    public Entity m_Segment;

    public RouteSegment(Entity segment) => this.m_Segment = segment;
  }
}
