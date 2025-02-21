// Decompiled with JetBrains decompiler
// Type: Game.Routes.WaypointDefinition
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Routes
{
  [InternalBufferCapacity(0)]
  public struct WaypointDefinition : IBufferElementData
  {
    public float3 m_Position;
    public Entity m_Connection;
    public Entity m_Original;

    public WaypointDefinition(float3 position)
    {
      this.m_Position = position;
      this.m_Connection = Entity.Null;
      this.m_Original = Entity.Null;
    }
  }
}
