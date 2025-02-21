// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ObjectRequirementElement
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct ObjectRequirementElement : IBufferElementData
  {
    public Entity m_Requirement;
    public ushort m_Group;
    public ObjectRequirementType m_Type;
    public ObjectRequirementFlags m_RequireFlags;
    public ObjectRequirementFlags m_ForbidFlags;

    public ObjectRequirementElement(Entity requirement, int group, ObjectRequirementType type = (ObjectRequirementType) 0)
    {
      this.m_Requirement = requirement;
      this.m_Group = (ushort) group;
      this.m_Type = type;
      this.m_RequireFlags = (ObjectRequirementFlags) 0;
      this.m_ForbidFlags = (ObjectRequirementFlags) 0;
    }

    public ObjectRequirementElement(
      ObjectRequirementFlags require,
      ObjectRequirementFlags forbid,
      int group,
      ObjectRequirementType type = (ObjectRequirementType) 0)
    {
      this.m_Requirement = Entity.Null;
      this.m_RequireFlags = require;
      this.m_ForbidFlags = forbid;
      this.m_Group = (ushort) group;
      this.m_Type = type;
    }
  }
}
