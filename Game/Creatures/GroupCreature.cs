// Decompiled with JetBrains decompiler
// Type: Game.Creatures.GroupCreature
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Creatures
{
  public struct GroupCreature : IBufferElementData, IEmptySerializable
  {
    public Entity m_Creature;

    public GroupCreature(Entity creature) => this.m_Creature = creature;
  }
}
