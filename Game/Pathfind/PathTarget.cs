// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathTarget
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Pathfind
{
  public struct PathTarget
  {
    public Entity m_Target;
    public Entity m_Entity;
    public float m_Delta;
    public float m_Cost;
    public EdgeFlags m_Flags;

    public PathTarget(Entity target, Entity entity, float delta, float cost)
    {
      this.m_Target = target;
      this.m_Entity = entity;
      this.m_Delta = delta;
      this.m_Cost = cost;
      this.m_Flags = EdgeFlags.DefaultMask;
    }

    public PathTarget(Entity target, Entity entity, float delta, float cost, EdgeFlags flags)
    {
      this.m_Target = target;
      this.m_Entity = entity;
      this.m_Delta = delta;
      this.m_Cost = cost;
      this.m_Flags = flags;
    }
  }
}
