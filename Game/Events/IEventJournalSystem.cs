// Decompiled with JetBrains decompiler
// Type: Game.Events.IEventJournalSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Events
{
  public interface IEventJournalSystem
  {
    NativeList<Entity> eventJournal { get; }

    EventJournalEntry GetInfo(Entity journalEntity);

    Entity GetPrefab(Entity journalEntity);

    bool TryGetData(Entity journalEntity, out DynamicBuffer<EventJournalData> data);

    bool TryGetCityEffects(Entity journalEntity, out DynamicBuffer<EventJournalCityEffect> data);

    IEnumerable<JournalEventComponent> eventPrefabs { get; }

    Action<Entity> eventEventDataChanged { get; set; }

    System.Action eventEntryAdded { get; set; }
  }
}
