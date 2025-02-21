// Decompiled with JetBrains decompiler
// Type: Game.Companies.WorkProvider
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Companies
{
  public struct WorkProvider : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_MaxWorkers;
    public short m_UneducatedCooldown;
    public short m_EducatedCooldown;
    public Entity m_UneducatedNotificationEntity;
    public Entity m_EducatedNotificationEntity;
    public short m_EfficiencyCooldown;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_MaxWorkers);
      writer.Write(this.m_UneducatedCooldown);
      writer.Write(this.m_EducatedCooldown);
      writer.Write(this.m_UneducatedNotificationEntity);
      writer.Write(this.m_EducatedNotificationEntity);
      writer.Write(this.m_EfficiencyCooldown);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_MaxWorkers);
      if (reader.context.version >= Version.companyNotifications)
      {
        reader.Read(out this.m_UneducatedCooldown);
        reader.Read(out this.m_EducatedCooldown);
        reader.Read(out this.m_UneducatedNotificationEntity);
        reader.Read(out this.m_EducatedNotificationEntity);
      }
      if (!(reader.context.version >= Version.buildingEfficiencyRework))
        return;
      reader.Read(out this.m_EfficiencyCooldown);
    }
  }
}
