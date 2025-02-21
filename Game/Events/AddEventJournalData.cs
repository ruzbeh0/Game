// Decompiled with JetBrains decompiler
// Type: Game.Events.AddEventJournalData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Events
{
  public struct AddEventJournalData : IComponentData, IQueryTypeParameter
  {
    public EventDataTrackingType m_Type;
    public Entity m_Event;
    public int m_Count;

    public AddEventJournalData(Entity eventEntity, EventDataTrackingType type, int count = 1)
    {
      this.m_Event = eventEntity;
      this.m_Type = type;
      this.m_Count = count;
    }
  }
}
