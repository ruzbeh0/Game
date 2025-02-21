// Decompiled with JetBrains decompiler
// Type: Game.Achievements.EventAchievementTrackingData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Achievements
{
  public struct EventAchievementTrackingData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public uint m_StartFrame;
    public AchievementId m_ID;

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_StartFrame);
      int id;
      reader.Read(out id);
      this.m_ID = new AchievementId(id);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_StartFrame);
      writer.Write(this.m_ID.id);
    }
  }
}
