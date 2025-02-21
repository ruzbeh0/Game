// Decompiled with JetBrains decompiler
// Type: Game.Common.TimeData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Common
{
  public struct TimeData : IComponentData, IQueryTypeParameter, IDefaultSerializable, ISerializable
  {
    public uint m_FirstFrame;
    public int m_StartingYear;
    public byte m_StartingMonth;
    public byte m_StartingHour;
    public byte m_StartingMinutes;

    public float TimeOffset
    {
      get
      {
        return (float) ((double) this.m_StartingHour / 24.0 + (double) this.m_StartingMinutes / 1440.0 + 9.9999997473787516E-06);
      }
      set
      {
        this.m_StartingHour = (byte) Mathf.FloorToInt((float) (((double) value * 24.0 + 9.9999997473787516E-06) % 24.0));
        this.m_StartingMinutes = (byte) (Mathf.RoundToInt(value * 1440f) % 60);
      }
    }

    public float GetDateOffset(int daysPerYear)
    {
      return (float) this.m_StartingMonth / (float) daysPerYear;
    }

    public void SetDefaults(Context context)
    {
      this.m_FirstFrame = 0U;
      this.m_StartingYear = 2021;
      this.m_StartingMonth = (byte) 5;
      this.m_StartingHour = (byte) 7;
      this.m_StartingMinutes = (byte) 0;
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_FirstFrame);
      reader.Read(out this.m_StartingYear);
      reader.Read(out this.m_StartingMonth);
      reader.Read(out this.m_StartingHour);
      reader.Read(out this.m_StartingMinutes);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_FirstFrame);
      writer.Write(this.m_StartingYear);
      writer.Write(this.m_StartingMonth);
      writer.Write(this.m_StartingHour);
      writer.Write(this.m_StartingMinutes);
    }

    public static TimeData GetSingleton(EntityQuery query)
    {
      return !query.IsEmptyIgnoreFilter ? query.GetSingleton<TimeData>() : new TimeData();
    }
  }
}
