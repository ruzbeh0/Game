// Decompiled with JetBrains decompiler
// Type: Game.Events.EventJournalUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Events
{
  public static class EventJournalUtils
  {
    public static bool IsValid(EventDataTrackingType type)
    {
      return type >= EventDataTrackingType.Damages && type < EventDataTrackingType.Count;
    }

    public static bool IsValid(EventCityEffectTrackingType type)
    {
      return type >= EventCityEffectTrackingType.Crime && type < EventCityEffectTrackingType.Count;
    }

    public static int GetValue(DynamicBuffer<EventJournalData> data, EventDataTrackingType type)
    {
      if (EventJournalUtils.IsValid(type))
      {
        for (int index = 0; index < data.Length; ++index)
        {
          if (data[index].m_Type == type)
            return data[index].m_Value;
        }
      }
      return 0;
    }

    public static int GetValue(
      DynamicBuffer<EventJournalCityEffect> effects,
      EventCityEffectTrackingType type)
    {
      if (EventJournalUtils.IsValid(type))
      {
        for (int index = 0; index < effects.Length; ++index)
        {
          EventJournalCityEffect effect = effects[index];
          if (effect.m_Type == type)
            return EventJournalUtils.GetPercentileChange(effect);
        }
      }
      return 0;
    }

    public static int GetPercentileChange(EventJournalCityEffect effect)
    {
      return effect.m_StartValue != 0 ? Mathf.RoundToInt((float) ((double) (effect.m_Value - effect.m_StartValue) / (double) effect.m_StartValue * 100.0)) : 0;
    }
  }
}
