// Decompiled with JetBrains decompiler
// Type: Game.Policies.Policy
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Policies
{
  [InternalBufferCapacity(0)]
  public struct Policy : IBufferElementData, ISerializable
  {
    public Entity m_Policy;
    public PolicyFlags m_Flags;
    public float m_Adjustment;

    public Policy(Entity policy, PolicyFlags flags, float adjustment)
    {
      this.m_Policy = policy;
      this.m_Flags = flags;
      this.m_Adjustment = adjustment;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Policy);
      writer.Write((byte) this.m_Flags);
      writer.Write(this.m_Adjustment);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Policy);
      byte num;
      reader.Read(out num);
      reader.Read(out this.m_Adjustment);
      this.m_Flags = (PolicyFlags) num;
    }
  }
}
