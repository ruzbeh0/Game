// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathUpdated
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Pathfind
{
  public struct PathUpdated : IComponentData, IQueryTypeParameter
  {
    public Entity m_Owner;
    public PathEventData m_Data;

    public PathUpdated(Entity owner, PathEventData data)
    {
      this.m_Owner = owner;
      this.m_Data = data;
    }
  }
}
