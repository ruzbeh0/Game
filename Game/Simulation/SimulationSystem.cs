// Decompiled with JetBrains decompiler
// Type: Game.Simulation.SimulationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Serialization.Entities;
using Game.Pathfind;
using Game.SceneFlow;
using Game.Settings;
using Game.Tools;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class SimulationSystem : 
    GameSystemBase,
    ISimulationSystem,
    IDefaultSerializable,
    ISerializable
  {
    public const float PENDING_FRAMES_SPEED_FACTOR = 0.020833334f;
    private const int LOADING_COUNT = 1024;
    public const string kLoadingTask = "LoadSimulation";
    private UpdateSystem m_UpdateSystem;
    private ToolSystem m_ToolSystem;
    private PathfindResultSystem m_PathfindResultSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private float m_Timer;
    private float m_LastSpeed;
    private float m_SelectedSpeed;
    private int m_LoadingCount;
    private int m_StepCount;
    private bool m_IsLoading;
    private JobHandle m_WatchDeps;
    private Stopwatch m_Stopwatch;

    public uint frameIndex { get; private set; }

    public float frameTime { get; private set; }

    public float selectedSpeed
    {
      get => this.m_SelectedSpeed;
      set
      {
        if (this.m_IsLoading)
          return;
        this.m_SelectedSpeed = value;
      }
    }

    public float smoothSpeed { get; private set; }

    public float loadingProgress
    {
      get => !this.m_IsLoading ? 1f : TaskManager.instance.GetTaskProgress("LoadSimulation");
      private set
      {
        if (!this.m_IsLoading)
          return;
        TaskManager.instance.progress.Report(new ProgressTracker("LoadSimulation", ProgressTracker.Group.Group3)
        {
          progress = value
        });
      }
    }

    public float frameDuration { get; private set; }

    public SimulationSystem.PerformancePreference performancePreference { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.selectedSpeed = 1f;
      SharedSettings instance = SharedSettings.instance;
      this.performancePreference = instance != null ? instance.general.performancePreference : SimulationSystem.PerformancePreference.Balanced;
      // ISSUE: reference to a compiler-generated field
      this.m_Stopwatch = new Stopwatch();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem = this.World.GetOrCreateSystemManaged<UpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindResultSystem = this.World.GetOrCreateSystemManaged<PathfindResultSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_WatchDeps.Complete();
      base.OnDestroy();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.frameIndex);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      uint num;
      reader.Read(out num);
      this.frameIndex = num;
      this.frameTime = 0.0f;
      // ISSUE: reference to a compiler-generated field
      this.m_Timer = 0.0f;
    }

    public void SetDefaults(Context context)
    {
      this.frameIndex = 0U;
      this.frameTime = 0.0f;
      // ISSUE: reference to a compiler-generated field
      this.m_Timer = 0.0f;
    }

    protected override void OnGamePreload(Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      this.selectedSpeed = 0.0f;
      this.loadingProgress = 0.0f;
      // ISSUE: reference to a compiler-generated field
      this.m_LoadingCount = purpose == Purpose.NewGame ? 1024 : 0;
      // ISSUE: reference to a compiler-generated field
      this.m_IsLoading = true;
    }

    private void UpdateLoadingProgress()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoadingCount > 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateSystem.Update(SystemUpdatePhase.PreSimulation);
        int num = 8;
        for (int iterationIndex = 0; iterationIndex < num; ++iterationIndex)
        {
          ++this.frameIndex;
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateSystem.Update(SystemUpdatePhase.LoadSimulation, this.frameIndex, iterationIndex);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateSystem.Update(SystemUpdatePhase.PostSimulation);
        // ISSUE: reference to a compiler-generated field
        this.m_LoadingCount -= num;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoadingCount > 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.loadingProgress = math.clamp((float) (1.0 - (double) this.m_LoadingCount / 1024.0), 0.0f, 0.99999f);
      }
      else
        this.loadingProgress = 1f;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_StepCount != 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_WatchDeps.Complete();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.frameDuration = (float) this.m_Stopwatch.ElapsedTicks / (float) (Stopwatch.Frequency * (long) this.m_StepCount);
        // ISSUE: reference to a compiler-generated field
        this.m_Stopwatch.Reset();
        // ISSUE: reference to a compiler-generated field
        this.m_StepCount = 0;
      }
      else
        this.frameDuration = 0.0f;
      // ISSUE: reference to a compiler-generated field
      if (this.m_IsLoading)
      {
        if ((double) this.loadingProgress != 1.0)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateLoadingProgress();
          return;
        }
        if (!GameManager.instance.isGameLoading)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_IsLoading = false;
          GameplaySettings gameplay = SharedSettings.instance?.gameplay;
          this.selectedSpeed = gameplay == null || !gameplay.pausedAfterLoading ? 1f : 0.0f;
        }
      }
      else if (GameManager.instance.isGameLoading)
        this.selectedSpeed = 0.0f;
      int num1;
      if ((double) this.selectedSpeed == 0.0)
      {
        num1 = 0;
        this.smoothSpeed = 0.0f;
      }
      else
      {
        float deltaTime = UnityEngine.Time.deltaTime;
        float num2 = deltaTime * this.selectedSpeed;
        float num3 = 1f;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PathfindResultSystem.pendingSimulationFrame < uint.MaxValue)
        {
          // ISSUE: reference to a compiler-generated field
          num3 = math.min(1f, (float) (int) math.max(0U, (uint) ((int) this.m_PathfindResultSystem.pendingSimulationFrame - (int) this.frameIndex - 1)) * 0.020833334f);
          num2 *= num3;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Timer += num2;
        // ISSUE: reference to a compiler-generated field
        int x1 = (int) math.floor(this.m_Timer * 60f);
        float x2 = num2 * 60f;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PathfindResultSystem.pendingSimulationFrame < uint.MaxValue)
        {
          // ISSUE: reference to a compiler-generated field
          int y = (int) math.max(0U, (uint) ((int) this.m_PathfindResultSystem.pendingSimulationFrame - (int) this.frameIndex - 1));
          x1 = math.min(x1, y);
          x2 = math.min(x2, (float) y);
        }
        if (this.performancePreference != SimulationSystem.PerformancePreference.SimulationSpeed)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float f = (this.m_EndFrameBarrier.lastElapsedTime - this.m_EndFrameBarrier.currentElapsedTime) / math.max(1f / 1000f, this.frameDuration);
          int y = math.max(1, this.performancePreference == SimulationSystem.PerformancePreference.FrameRate ? Mathf.FloorToInt(f) : Mathf.CeilToInt(f));
          x1 = math.min(x1, y);
          x2 = math.min(x2, (float) y);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Timer = math.clamp(this.m_Timer - (float) x1 / 60f, 0.0f, 0.0166666675f);
        int b = math.max(1, math.min(8, Mathf.RoundToInt((float) ((double) this.selectedSpeed * (double) num3 * 2.0))));
        num1 = math.clamp(x1, 0, b);
        float x3 = math.clamp(x2, 0.0f, (float) b) / math.max(1E-06f, 60f * deltaTime);
        float y1 = math.lerp(x3, this.smoothSpeed, math.pow(0.5f, deltaTime));
        // ISSUE: reference to a compiler-generated field
        float y2 = this.smoothSpeed + this.selectedSpeed - this.m_LastSpeed;
        this.smoothSpeed = (double) x3 <= (double) this.smoothSpeed ? math.min(math.max(x3, y2), y1) : math.max(math.min(x3, y2), y1);
      }
      // ISSUE: reference to a compiler-generated field
      this.frameTime = this.m_Timer * 60f;
      // ISSUE: reference to a compiler-generated field
      this.m_LastSpeed = this.selectedSpeed;
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem.Update(SystemUpdatePhase.PreSimulation);
      if (num1 != 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_StepCount = num1;
        // ISSUE: reference to a compiler-generated field
        this.m_Stopwatch.Start();
        for (int iterationIndex = 0; iterationIndex < num1; ++iterationIndex)
        {
          ++this.frameIndex;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ToolSystem.actionMode.IsEditor())
          {
            // ISSUE: reference to a compiler-generated field
            this.m_UpdateSystem.Update(SystemUpdatePhase.EditorSimulation, this.frameIndex, iterationIndex);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_ToolSystem.actionMode.IsGame())
          {
            // ISSUE: reference to a compiler-generated field
            this.m_UpdateSystem.Update(SystemUpdatePhase.GameSimulation, this.frameIndex, iterationIndex);
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SimulationSystem.SimulationEndTimeJob jobData = new SimulationSystem.SimulationEndTimeJob()
        {
          m_Stopwatch = GCHandle.Alloc((object) this.m_Stopwatch)
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_WatchDeps = jobData.Schedule<SimulationSystem.SimulationEndTimeJob>(this.m_EndFrameBarrier.producerHandle);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem.Update(SystemUpdatePhase.PostSimulation);
    }

    [Preserve]
    public SimulationSystem()
    {
    }

    public enum PerformancePreference
    {
      FrameRate,
      Balanced,
      SimulationSpeed,
    }

    private struct SimulationEndTimeJob : IJob
    {
      public GCHandle m_Stopwatch;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        ((Stopwatch) this.m_Stopwatch.Target).Stop();
        // ISSUE: reference to a compiler-generated field
        this.m_Stopwatch.Free();
      }
    }
  }
}
