// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.AvailabilityElement
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Pathfind
{
  [InternalBufferCapacity(0)]
  public struct AvailabilityElement : IBufferElementData
  {
    public Entity m_Edge;
    public float2 m_Availability;
  }
}
