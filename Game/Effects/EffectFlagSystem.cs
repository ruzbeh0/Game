// Decompiled with JetBrains decompiler
// Type: Game.Effects.EffectFlagSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Prefabs;
using Game.Simulation;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Effects
{
  public class EffectFlagSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    public static readonly uint kNightRandomTicks = 15000;
    public static readonly uint kDayRandomTicks = 10000;
    public static readonly float kNightBegin = 0.75f;
    public static readonly float kDayBegin = 0.25f;
    public static readonly uint kSpringRandomTicks = 20000;
    public static readonly uint kAutumnRandomTicks = 20000;
    public static readonly float kSpringTemperature = 10f;
    public static readonly float kAutumnTemperature = 5f;
    private Entity m_CurrentSeason;
    private uint m_LastSeasonChange;
    private bool m_IsColdSeason;
    private bool m_IsNightTime;
    private uint m_LastTimeChange;
    private SimulationSystem m_SimulationSystem;
    private TimeSystem m_TimeSystem;
    private ClimateSystem m_ClimateSystem;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 2048;

    public static bool IsEnabled(
      EffectConditionFlags flag,
      Random random,
      EffectFlagSystem.EffectFlagData data,
      uint frame)
    {
      if ((flag & EffectConditionFlags.Night) != EffectConditionFlags.None)
        return data.m_IsNightTime ? data.m_LastTimeChange + random.NextUInt(EffectFlagSystem.kNightRandomTicks) < frame : data.m_LastTimeChange + random.NextUInt(EffectFlagSystem.kDayRandomTicks) >= frame;
      if ((flag & EffectConditionFlags.Cold) == EffectConditionFlags.None)
        return true;
      return data.m_IsColdSeason ? data.m_LastSeasonChange + random.NextUInt(EffectFlagSystem.kAutumnRandomTicks) < frame : data.m_LastSeasonChange + random.NextUInt(EffectFlagSystem.kSpringRandomTicks) >= frame;
    }

    public EffectFlagSystem.EffectFlagData GetData()
    {
      return new EffectFlagSystem.EffectFlagData()
      {
        m_IsColdSeason = this.m_IsColdSeason,
        m_IsNightTime = this.m_IsNightTime,
        m_LastSeasonChange = this.m_LastSeasonChange,
        m_LastTimeChange = this.m_LastTimeChange
      };
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_LastSeasonChange);
      reader.Read(out this.m_IsColdSeason);
      reader.Read(out this.m_LastTimeChange);
      this.m_IsNightTime = false;
      if (!(reader.context.version >= Version.addNightForestAmbienceSound))
        return;
      reader.Read(out this.m_IsNightTime);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_LastSeasonChange);
      writer.Write(this.m_IsColdSeason);
      writer.Write(this.m_LastTimeChange);
      writer.Write(this.m_IsNightTime);
    }

    public void SetDefaults(Context context)
    {
      this.m_LastSeasonChange = 0U;
      this.m_IsColdSeason = false;
      this.m_IsNightTime = false;
      this.m_LastTimeChange = 0U;
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      uint frameIndex = this.m_SimulationSystem.frameIndex;
      if (this.m_CurrentSeason != this.m_ClimateSystem.currentSeason)
      {
        float temperature = (float) this.m_ClimateSystem.temperature;
        if (this.m_IsColdSeason && (double) temperature >= (double) EffectFlagSystem.kSpringTemperature)
        {
          this.m_IsColdSeason = false;
          this.m_LastSeasonChange = frameIndex;
        }
        else if (!this.m_IsColdSeason && (double) temperature < (double) EffectFlagSystem.kAutumnTemperature)
        {
          this.m_IsColdSeason = true;
          this.m_LastSeasonChange = frameIndex;
        }
      }
      this.m_IsNightTime = (double) this.m_TimeSystem.normalizedTime >= (double) EffectFlagSystem.kNightBegin || (double) this.m_TimeSystem.normalizedTime < (double) EffectFlagSystem.kDayBegin;
      this.m_CurrentSeason = this.m_ClimateSystem.currentSeason;
    }

    [Preserve]
    public EffectFlagSystem()
    {
    }

    public struct EffectFlagData
    {
      public bool m_IsNightTime;
      public uint m_LastTimeChange;
      public bool m_IsColdSeason;
      public uint m_LastSeasonChange;
    }
  }
}
