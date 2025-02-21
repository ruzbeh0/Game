// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.EventJournalUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Common;
using Game.Events;
using Game.Prefabs;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class EventJournalUISystem : UISystemBase
  {
    private const string kGroup = "eventJournal";
    private const int kMaxMessages = 100;
    private IEventJournalSystem m_EventJournalSystem;
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_TimeDataQuery;
    private RawMapBinding<Entity> m_EventMap;
    private RawValueBinding m_Events;

    public Action eventJournalOpened { get; set; }

    public Action eventJournalClosed { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EventJournalSystem = (IEventJournalSystem) this.World.GetOrCreateSystemManaged<EventJournalSystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_EventJournalSystem.eventEventDataChanged += (Action<Entity>) (entity => this.m_EventMap.Update(entity));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_EventJournalSystem.eventEntryAdded += (Action) (() => this.m_Events.Update());
      // ISSUE: reference to a compiler-generated field
      this.m_TimeDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
      this.AddBinding((IBinding) new TriggerBinding("eventJournal", "openJournal", (Action) (() =>
      {
        Action eventJournalOpened = this.eventJournalOpened;
        if (eventJournalOpened == null)
          return;
        eventJournalOpened();
      })));
      this.AddBinding((IBinding) new TriggerBinding("eventJournal", "closeJournal", (Action) (() =>
      {
        Action eventJournalClosed = this.eventJournalClosed;
        if (eventJournalClosed == null)
          return;
        eventJournalClosed();
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_Events = new RawValueBinding("eventJournal", "events", (Action<IJsonWriter>) (binder =>
      {
        // ISSUE: reference to a compiler-generated field
        binder.ArrayBegin(Math.Min(this.m_EventJournalSystem.eventJournal.Length, 100));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (int index = this.m_EventJournalSystem.eventJournal.Length - 1; index >= 0 && index >= this.m_EventJournalSystem.eventJournal.Length - 100; --index)
        {
          // ISSUE: reference to a compiler-generated field
          binder.Write(this.m_EventJournalSystem.eventJournal[index]);
        }
        binder.ArrayEnd();
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_EventMap = new RawMapBinding<Entity>("eventJournal", "eventMap", (Action<IJsonWriter, Entity>) ((binder, entity) => this.BindJournalEntry(entity, binder)))));
    }

    [Preserve]
    protected override void OnUpdate()
    {
    }

    private void OnEventDataChanged(Entity entity) => this.m_EventMap.Update(entity);

    private void OnEntryAdded() => this.m_Events.Update();

    private void BindEvents(IJsonWriter binder)
    {
      // ISSUE: reference to a compiler-generated field
      binder.ArrayBegin(Math.Min(this.m_EventJournalSystem.eventJournal.Length, 100));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (int index = this.m_EventJournalSystem.eventJournal.Length - 1; index >= 0 && index >= this.m_EventJournalSystem.eventJournal.Length - 100; --index)
      {
        // ISSUE: reference to a compiler-generated field
        binder.Write(this.m_EventJournalSystem.eventJournal[index]);
      }
      binder.ArrayEnd();
    }

    private void BindJournalEntry(Entity entity, IJsonWriter binder)
    {
      // ISSUE: reference to a compiler-generated field
      EventJournalEntry info = this.m_EventJournalSystem.GetInfo(entity);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      EventPrefab prefab = this.m_PrefabSystem.GetPrefab<EventPrefab>(this.m_EventJournalSystem.GetPrefab(entity));
      JournalEventComponent component = prefab.GetComponent<JournalEventComponent>();
      binder.TypeBegin("eventJournal.EventInfo");
      binder.PropertyName("id");
      binder.Write(prefab.name);
      binder.PropertyName("icon");
      binder.Write(component.m_Icon);
      binder.PropertyName("date");
      // ISSUE: reference to a compiler-generated field
      binder.Write(info.m_StartFrame - TimeData.GetSingleton(this.m_TimeDataQuery).m_FirstFrame);
      binder.PropertyName("data");
      DynamicBuffer<EventJournalData> data1;
      // ISSUE: reference to a compiler-generated field
      if (this.m_EventJournalSystem.TryGetData(entity, out data1))
      {
        binder.ArrayBegin(data1.Length);
        for (int index = 0; index < data1.Length; ++index)
        {
          binder.TypeBegin("eventJournal.UIEventData");
          binder.PropertyName("type");
          binder.Write(Enum.GetName(typeof (EventDataTrackingType), (object) data1[index].m_Type));
          binder.PropertyName("value");
          binder.Write(data1[index].m_Value);
          binder.TypeEnd();
        }
        binder.ArrayEnd();
      }
      else
        binder.WriteNull();
      binder.PropertyName("effects");
      DynamicBuffer<EventJournalCityEffect> data2;
      // ISSUE: reference to a compiler-generated field
      if (this.m_EventJournalSystem.TryGetCityEffects(entity, out data2))
      {
        binder.ArrayBegin(data2.Length);
        for (int index = 0; index < data2.Length; ++index)
        {
          binder.TypeBegin("eventJournal.UIEventData");
          binder.PropertyName("type");
          binder.Write(Enum.GetName(typeof (EventCityEffectTrackingType), (object) data2[index].m_Type));
          binder.PropertyName("value");
          binder.Write(EventJournalUtils.GetPercentileChange(data2[index]));
          binder.TypeEnd();
        }
        binder.ArrayEnd();
      }
      else
        binder.WriteNull();
      binder.TypeEnd();
    }

    [Preserve]
    public EventJournalUISystem()
    {
    }
  }
}
