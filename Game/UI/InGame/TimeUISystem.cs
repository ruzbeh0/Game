// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.TimeUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Colossal.Serialization.Entities;
using Colossal.UI.Binding;
using Game.Common;
using Game.Prefabs;
using Game.Rendering;
using Game.Simulation;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class TimeUISystem : UISystemBase
  {
    private const string kGroup = "time";
    private SimulationSystem m_SimulationSystem;
    private TimeSystem m_TimeSystem;
    private LightingSystem m_LightingSystem;
    private EntityQuery m_TimeSettingsQuery;
    private EntityQuery m_TimeDataQuery;
    private EventBinding<bool> m_SimulationPausedBarrierBinding;
    private float m_SpeedBeforePause = 1f;
    private bool m_UnpausedBeforeForcedPause;
    private bool m_HasFocus = true;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LightingSystem = this.World.GetOrCreateSystemManaged<LightingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TimeDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<TimeUISystem.TimeSettings>("time", "timeSettings", (Func<TimeUISystem.TimeSettings>) (() =>
      {
        // ISSUE: reference to a compiler-generated method
        TimeSettingsData timeSettingsData = this.GetTimeSettingsData();
        // ISSUE: reference to a compiler-generated field
        TimeData singleton = TimeData.GetSingleton(this.m_TimeDataQuery);
        // ISSUE: object of a compiler-generated type is created
        return new TimeUISystem.TimeSettings()
        {
          ticksPerDay = 262144,
          daysPerYear = timeSettingsData.m_DaysPerYear,
          epochTicks = Mathf.RoundToInt(singleton.TimeOffset * 262144f) + Mathf.RoundToInt(singleton.GetDateOffset(timeSettingsData.m_DaysPerYear) * 262144f * (float) timeSettingsData.m_DaysPerYear),
          epochYear = singleton.m_StartingYear
        };
      }), (IWriter<TimeUISystem.TimeSettings>) new ValueWriter<TimeUISystem.TimeSettings>()));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<int>("time", "ticks", (Func<int>) (() =>
      {
        float num = 182.044449f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return Mathf.FloorToInt(Mathf.Floor((float) (this.m_SimulationSystem.frameIndex - TimeData.GetSingleton(this.m_TimeDataQuery).m_FirstFrame) / num) * num);
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<int>("time", "day", (Func<int>) (() => TimeSystem.GetDay(this.m_SimulationSystem.frameIndex, TimeData.GetSingleton(this.m_TimeDataQuery)))));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<LightingSystem.State>("time", "lightingState", (Func<LightingSystem.State>) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        LightingSystem.State state = this.m_LightingSystem.state;
        if (state != LightingSystem.State.Invalid)
          return state;
        // ISSUE: reference to a compiler-generated field
        float normalizedTime = this.m_TimeSystem.normalizedTime;
        return (double) normalizedTime >= 0.2916666567325592 && (double) normalizedTime <= 0.875 ? LightingSystem.State.Day : LightingSystem.State.Night;
      }), (IWriter<LightingSystem.State>) new DelegateWriter<LightingSystem.State>((WriterDelegate<LightingSystem.State>) ((writer, value) => writer.Write((int) value)))));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("time", "simulationPaused", (Func<bool>) (() => (double) this.m_SimulationSystem.selectedSpeed == 0.0)));
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<int>("time", "simulationSpeed", (Func<int>) (() => TimeUISystem.SpeedToIndex(this.IsPaused() ? this.m_SpeedBeforePause : this.m_SimulationSystem.selectedSpeed))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SimulationPausedBarrierBinding = new EventBinding<bool>("time", "simulationPausedBarrier")));
      this.AddBinding((IBinding) new TriggerBinding<bool>("time", "setSimulationPaused", (Action<bool>) (paused =>
      {
        if (!this.pausedBarrierActive)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_SimulationSystem.selectedSpeed = paused ? 0.0f : this.m_SpeedBeforePause;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UnpausedBeforeForcedPause = !paused;
        }
      })));
      this.AddBinding((IBinding) new TriggerBinding<int>("time", "setSimulationSpeed", (Action<int>) (speedIndex =>
      {
        if (!this.pausedBarrierActive)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_SimulationSystem.selectedSpeed = TimeUISystem.IndexToSpeed(speedIndex);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_SpeedBeforePause = TimeUISystem.IndexToSpeed(speedIndex);
          // ISSUE: reference to a compiler-generated field
          this.m_UnpausedBeforeForcedPause = true;
        }
      })));
      PlatformManager.instance.onAppStateChanged += (OnAppStateChanged) ((psi, state) =>
      {
        if (state == AppState.Default)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_HasFocus = true;
        }
        else
        {
          if (state != AppState.Constrained)
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_HasFocus = false;
        }
      });
    }

    private void HandleAppStateChanged(IPlatformServiceIntegration psi, AppState state)
    {
      if (state == AppState.Default)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_HasFocus = true;
      }
      else
      {
        if (state != AppState.Constrained)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_HasFocus = false;
      }
    }

    protected override void OnGameLoaded(Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      // ISSUE: reference to a compiler-generated field
      this.m_SpeedBeforePause = 1f;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      // ISSUE: reference to a compiler-generated field
      if ((double) this.m_SimulationSystem.selectedSpeed > 0.0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SpeedBeforePause = this.m_SimulationSystem.selectedSpeed;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((!this.m_HasFocus ? 1 : (this.m_SimulationPausedBarrierBinding.observerCount > 0 ? 1 : 0)) != 0)
      {
        // ISSUE: reference to a compiler-generated method
        if (!this.IsPaused())
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UnpausedBeforeForcedPause = true;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_SimulationSystem.selectedSpeed = 0.0f;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_UnpausedBeforeForcedPause)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_SimulationSystem.selectedSpeed = this.m_SpeedBeforePause;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_UnpausedBeforeForcedPause = false;
      }
    }

    private TimeUISystem.TimeSettings GetTimeSettings()
    {
      // ISSUE: reference to a compiler-generated method
      TimeSettingsData timeSettingsData = this.GetTimeSettingsData();
      // ISSUE: reference to a compiler-generated field
      TimeData singleton = TimeData.GetSingleton(this.m_TimeDataQuery);
      // ISSUE: object of a compiler-generated type is created
      return new TimeUISystem.TimeSettings()
      {
        ticksPerDay = 262144,
        daysPerYear = timeSettingsData.m_DaysPerYear,
        epochTicks = Mathf.RoundToInt(singleton.TimeOffset * 262144f) + Mathf.RoundToInt(singleton.GetDateOffset(timeSettingsData.m_DaysPerYear) * 262144f * (float) timeSettingsData.m_DaysPerYear),
        epochYear = singleton.m_StartingYear
      };
    }

    public int GetTicks()
    {
      float num = 182.044449f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return Mathf.FloorToInt(Mathf.Floor((float) (this.m_SimulationSystem.frameIndex - TimeData.GetSingleton(this.m_TimeDataQuery).m_FirstFrame) / num) * num);
    }

    public int GetDay()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return TimeSystem.GetDay(this.m_SimulationSystem.frameIndex, TimeData.GetSingleton(this.m_TimeDataQuery));
    }

    public LightingSystem.State GetLightingState()
    {
      // ISSUE: reference to a compiler-generated field
      LightingSystem.State state = this.m_LightingSystem.state;
      if (state != LightingSystem.State.Invalid)
        return state;
      // ISSUE: reference to a compiler-generated field
      float normalizedTime = this.m_TimeSystem.normalizedTime;
      return (double) normalizedTime >= 0.2916666567325592 && (double) normalizedTime <= 0.875 ? LightingSystem.State.Day : LightingSystem.State.Night;
    }

    public bool IsPaused() => (double) this.m_SimulationSystem.selectedSpeed == 0.0;

    public int GetSimulationSpeed()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return TimeUISystem.SpeedToIndex(this.IsPaused() ? this.m_SpeedBeforePause : this.m_SimulationSystem.selectedSpeed);
    }

    private TimeSettingsData GetTimeSettingsData()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_TimeSettingsQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        return this.m_TimeSettingsQuery.GetSingleton<TimeSettingsData>();
      }
      return new TimeSettingsData() { m_DaysPerYear = 12 };
    }

    private void SetSimulationPaused(bool paused)
    {
      if (!this.pausedBarrierActive)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SimulationSystem.selectedSpeed = paused ? 0.0f : this.m_SpeedBeforePause;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_UnpausedBeforeForcedPause = !paused;
      }
    }

    private void SetSimulationSpeed(int speedIndex)
    {
      if (!this.pausedBarrierActive)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_SimulationSystem.selectedSpeed = TimeUISystem.IndexToSpeed(speedIndex);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_SpeedBeforePause = TimeUISystem.IndexToSpeed(speedIndex);
        // ISSUE: reference to a compiler-generated field
        this.m_UnpausedBeforeForcedPause = true;
      }
    }

    private bool pausedBarrierActive => this.m_SimulationPausedBarrierBinding.observerCount > 0;

    private static float IndexToSpeed(int index) => Mathf.Pow(2f, (float) Mathf.Clamp(index, 0, 2));

    private static int SpeedToIndex(float speed)
    {
      return (double) speed <= 0.0 ? 0 : Mathf.Clamp((int) Mathf.Log(speed, 2f), 0, 2);
    }

    [Preserve]
    public TimeUISystem()
    {
    }

    private struct TimeSettings : IJsonWritable, IEquatable<TimeUISystem.TimeSettings>
    {
      public int ticksPerDay;
      public int daysPerYear;
      public int epochTicks;
      public int epochYear;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("ticksPerDay");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.ticksPerDay);
        writer.PropertyName("daysPerYear");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.daysPerYear);
        writer.PropertyName("epochTicks");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.epochTicks);
        writer.PropertyName("epochYear");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.epochYear);
        writer.TypeEnd();
      }

      public bool Equals(TimeUISystem.TimeSettings other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.ticksPerDay == other.ticksPerDay && this.daysPerYear == other.daysPerYear && this.epochTicks == other.epochTicks && this.epochYear == other.epochYear;
      }
    }
  }
}
