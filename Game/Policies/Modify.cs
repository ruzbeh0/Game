// Decompiled with JetBrains decompiler
// Type: Game.Policies.Modify
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Policies
{
  public struct Modify : IComponentData, IQueryTypeParameter
  {
    public Entity m_Entity;
    public Entity m_Policy;
    public PolicyFlags m_Flags;
    public float m_Adjustment;

    public Modify(Entity entity, Entity policy, bool active, float adjustment)
    {
      this.m_Entity = entity;
      this.m_Policy = policy;
      this.m_Flags = active ? PolicyFlags.Active : (PolicyFlags) 0;
      this.m_Adjustment = adjustment;
    }
  }
}
