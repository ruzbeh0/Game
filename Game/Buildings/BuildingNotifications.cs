// Decompiled with JetBrains decompiler
// Type: Game.Buildings.BuildingNotifications
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct BuildingNotifications : IComponentData, IQueryTypeParameter, ISerializable
  {
    public BuildingNotification m_Notifications;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((byte) this.m_Notifications);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      byte num;
      reader.Read(out num);
      this.m_Notifications = (BuildingNotification) num;
    }

    public bool HasNotification(BuildingNotification notification)
    {
      return (this.m_Notifications & notification) != 0;
    }
  }
}
