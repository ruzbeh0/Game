// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TimeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Common;
using Game.Prefabs;
using Game.Serialization;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TimeSystem : GameSystemBase, ITimeSystem, IPostDeserialize
  {
    private SimulationSystem m_SimulationSystem;
    public const int kTicksPerDay = 262144;
    private float m_Time;
    private float m_Date;
    private int m_Year = 1;
    private int m_DaysPerYear = 1;
    private uint m_InitialFrame;
    private EntityQuery m_TimeSettingGroup;
    private EntityQuery m_TimeDataQuery;

    public int startingYear { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSettingGroup = this.GetEntityQuery(ComponentType.ReadOnly<TimeSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TimeDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TimeSettingGroup);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TimeDataQuery);
    }

    public void PostDeserialize(Context context)
    {
      EntityManager entityManager;
      // ISSUE: reference to a compiler-generated field
      if (this.m_TimeDataQuery.IsEmpty)
      {
        entityManager = this.EntityManager;
        Entity entity = entityManager.CreateEntity();
        TimeData componentData = new TimeData();
        componentData.SetDefaults(context);
        entityManager = this.EntityManager;
        entityManager.AddComponentData<TimeData>(entity, componentData);
      }
      if (context.purpose == Colossal.Serialization.Entities.Purpose.NewGame)
      {
        // ISSUE: reference to a compiler-generated field
        TimeData singleton = this.m_TimeDataQuery.GetSingleton<TimeData>();
        // ISSUE: reference to a compiler-generated field
        Entity singletonEntity = this.m_TimeDataQuery.GetSingletonEntity();
        // ISSUE: reference to a compiler-generated field
        singleton.m_FirstFrame = this.m_SimulationSystem.frameIndex;
        singleton.m_StartingYear = this.startingYear;
        entityManager = this.EntityManager;
        entityManager.SetComponentData<TimeData>(singletonEntity, singleton);
      }
      // ISSUE: reference to a compiler-generated method
      this.UpdateTime();
    }

    protected int GetTicks(uint frameIndex, TimeSettingsData settings, TimeData data)
    {
      return (int) frameIndex - (int) data.m_FirstFrame + Mathf.RoundToInt(data.TimeOffset * 262144f) + Mathf.RoundToInt(data.GetDateOffset(settings.m_DaysPerYear) * 262144f * (float) settings.m_DaysPerYear);
    }

    protected int GetTicks(TimeSettingsData settings, TimeData data)
    {
      // ISSUE: reference to a compiler-generated field
      return (int) this.m_SimulationSystem.frameIndex - (int) data.m_FirstFrame + Mathf.RoundToInt(data.TimeOffset * 262144f) + Mathf.RoundToInt(data.GetDateOffset(settings.m_DaysPerYear) * 262144f * (float) settings.m_DaysPerYear);
    }

    protected double GetTimeWithOffset(
      TimeSettingsData settings,
      TimeData data,
      double renderingFrame)
    {
      return renderingFrame + (double) data.TimeOffset * 262144.0 + (double) data.GetDateOffset(settings.m_DaysPerYear) * 262144.0 * (double) settings.m_DaysPerYear;
    }

    public float GetTimeOfDay(TimeSettingsData settings, TimeData data, double renderingFrame)
    {
      // ISSUE: reference to a compiler-generated method
      return (float) (this.GetTimeWithOffset(settings, data, renderingFrame) % 262144.0 / 262144.0);
    }

    protected float GetTimeOfDay(TimeSettingsData settings, TimeData data)
    {
      // ISSUE: reference to a compiler-generated method
      return (float) (this.GetTicks(settings, data) % 262144) / 262144f;
    }

    public float GetTimeOfYear(TimeSettingsData settings, TimeData data, double renderingFrame)
    {
      int num = 262144 * settings.m_DaysPerYear;
      // ISSUE: reference to a compiler-generated method
      return (float) this.GetTimeWithOffset(settings, data, renderingFrame % (double) num) / (float) num;
    }

    protected float GetTimeOfYear(TimeSettingsData settings, TimeData data)
    {
      int num = 262144 * settings.m_DaysPerYear;
      // ISSUE: reference to a compiler-generated method
      return (float) (this.GetTicks(settings, data) % num) / (float) num;
    }

    public float GetElapsedYears(TimeSettingsData settings, TimeData data)
    {
      int num = 262144 * settings.m_DaysPerYear;
      // ISSUE: reference to a compiler-generated field
      return (float) (this.m_SimulationSystem.frameIndex - data.m_FirstFrame) / (float) num;
    }

    public float GetStartingDate(TimeSettingsData settings, TimeData data)
    {
      int num = 262144 * settings.m_DaysPerYear;
      // ISSUE: reference to a compiler-generated method
      return (float) (this.GetTicks(data.m_FirstFrame, settings, data) % num) / (float) num;
    }

    public int GetYear(TimeSettingsData settings, TimeData data)
    {
      int num = 262144 * settings.m_DaysPerYear;
      // ISSUE: reference to a compiler-generated method
      return data.m_StartingYear + Mathf.FloorToInt((float) (this.GetTicks(settings, data) / num));
    }

    public float normalizedTime => this.m_Time;

    public float normalizedDate => this.m_Date;

    public int year => this.m_Year;

    public int daysPerYear
    {
      get
      {
        if (this.m_DaysPerYear == 0 && !this.m_TimeSettingGroup.IsEmptyIgnoreFilter)
        {
          this.m_DaysPerYear = this.m_TimeSettingGroup.GetSingleton<TimeSettingsData>().m_DaysPerYear;
          if (this.m_DaysPerYear == 0)
            this.m_DaysPerYear = 1;
        }
        return this.m_DaysPerYear;
      }
    }

    public static int GetDay(uint frame, TimeData data)
    {
      return Mathf.FloorToInt((float) (frame - data.m_FirstFrame) / 262144f + data.TimeOffset);
    }

    public void DebugAdvanceTime(int minutes)
    {
      // ISSUE: reference to a compiler-generated field
      TimeData singleton = this.m_TimeDataQuery.GetSingleton<TimeData>();
      // ISSUE: reference to a compiler-generated field
      Entity singletonEntity = this.m_TimeDataQuery.GetSingletonEntity();
      singleton.m_FirstFrame -= (uint) (minutes * 262144) / 1440U;
      this.EntityManager.SetComponentData<TimeData>(singletonEntity, singleton);
    }

    private static DateTime CreateDateTime(int year, int day, int hour, int minute, float second)
    {
      DateTime dateTime = new DateTime(0L, DateTimeKind.Utc);
      dateTime = dateTime.AddYears(year - 1);
      dateTime = dateTime.AddDays((double) (day - 1));
      dateTime = dateTime.AddHours((double) hour);
      dateTime = dateTime.AddMinutes((double) minute);
      dateTime = dateTime.AddSeconds((double) second);
      if (dateTime.IsDaylightSavingTime())
        dateTime = dateTime.AddHours(1.0);
      return dateTime;
    }

    public DateTime GetDateTime(double renderingFrame)
    {
      // ISSUE: reference to a compiler-generated field
      TimeSettingsData singleton1 = this.m_TimeSettingGroup.GetSingleton<TimeSettingsData>();
      // ISSUE: reference to a compiler-generated field
      TimeData singleton2 = this.m_TimeDataQuery.GetSingleton<TimeData>();
      // ISSUE: reference to a compiler-generated method
      float timeOfDay = this.GetTimeOfDay(singleton1, singleton2, renderingFrame);
      // ISSUE: reference to a compiler-generated method
      float timeOfYear = this.GetTimeOfYear(singleton1, singleton2, renderingFrame);
      int hour = Mathf.FloorToInt(24f * timeOfDay);
      int minute = Mathf.FloorToInt((float) (60.0 * (24.0 * (double) timeOfDay - (double) hour)));
      // ISSUE: reference to a compiler-generated method
      return TimeSystem.CreateDateTime(this.year, 1 + Mathf.FloorToInt((float) this.daysPerYear * timeOfYear) % this.daysPerYear, hour, minute, Mathf.Repeat(timeOfDay, 1f));
    }

    public DateTime GetCurrentDateTime()
    {
      float normalizedTime = this.normalizedTime;
      float normalizedDate = this.normalizedDate;
      int hour = Mathf.FloorToInt(24f * normalizedTime);
      int minute = Mathf.FloorToInt((float) (60.0 * (24.0 * (double) normalizedTime - (double) hour)));
      // ISSUE: reference to a compiler-generated method
      return TimeSystem.CreateDateTime(this.year, 1 + Mathf.FloorToInt((float) this.daysPerYear * normalizedDate) % this.daysPerYear, hour, minute, Mathf.Repeat(normalizedTime, 1f));
    }

    [Preserve]
    protected override void OnUpdate() => this.UpdateTime();

    private void UpdateTime()
    {
      // ISSUE: reference to a compiler-generated field
      TimeSettingsData singleton1 = this.m_TimeSettingGroup.GetSingleton<TimeSettingsData>();
      // ISSUE: reference to a compiler-generated field
      TimeData singleton2 = this.m_TimeDataQuery.GetSingleton<TimeData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_Time = this.GetTimeOfDay(singleton1, singleton2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_Date = this.GetTimeOfYear(singleton1, singleton2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_Year = this.GetYear(singleton1, singleton2);
      // ISSUE: reference to a compiler-generated field
      this.m_DaysPerYear = singleton1.m_DaysPerYear;
    }

    [Preserve]
    public TimeSystem()
    {
    }
  }
}
