// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathTargetMoved
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Pathfind
{
  public struct PathTargetMoved : IComponentData, IQueryTypeParameter
  {
    public Entity m_Target;
    public float3 m_OldLocation;
    public float3 m_NewLocation;

    public PathTargetMoved(Entity target, float3 oldLocation, float3 newLocation)
    {
      this.m_Target = target;
      this.m_OldLocation = oldLocation;
      this.m_NewLocation = newLocation;
    }
  }
}
