// Decompiled with JetBrains decompiler
// Type: Game.Buildings.Hospital
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct Hospital : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_TargetRequest;
    public HospitalFlags m_Flags;
    public byte m_TreatmentBonus;
    public byte m_MinHealth;
    public byte m_MaxHealth;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TargetRequest);
      writer.Write((byte) this.m_Flags);
      writer.Write(this.m_TreatmentBonus);
      writer.Write(this.m_MinHealth);
      writer.Write(this.m_MaxHealth);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.reverseServiceRequests2)
        reader.Read(out this.m_TargetRequest);
      byte num;
      reader.Read(out num);
      Context context = reader.context;
      if (context.version >= Version.healthcareImprovement)
        reader.Read(out this.m_TreatmentBonus);
      context = reader.context;
      if (context.version >= Version.healthcareImprovement2)
      {
        reader.Read(out this.m_MinHealth);
        reader.Read(out this.m_MaxHealth);
      }
      this.m_Flags = (HospitalFlags) num;
    }
  }
}
