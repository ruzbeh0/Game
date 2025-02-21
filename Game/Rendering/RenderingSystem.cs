// Decompiled with JetBrains decompiler
// Type: Game.Rendering.RenderingSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Serialization.Entities;
using Game.Common;
using Game.Prefabs;
using Game.Serialization;
using Game.Settings;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class RenderingSystem : GameSystemBase, IPostDeserialize
  {
    public const string kLoadingTask = "LoadMeshes";
    private SimulationSystem m_SimulationSystem;
    private PlanetarySystem m_PlanetarySystem;
    private UpdateSystem m_UpdateSystem;
    private BatchManagerSystem m_BatchManagerSystem;
    private ManagedBatchSystem m_ManagedBatchSystem;
    private BatchMeshSystem m_BatchMeshSystem;
    private AreaBatchSystem m_AreaBatchSystem;
    private TimeSystem m_TimeSystem;
    private Dictionary<Shader, bool> m_EnabledShaders;
    private EntityQuery m_TimeSettingGroup;
    private EntityQuery m_TimeDataQuery;
    private int m_TotalLoadingCount;
    private int m_EnabledShaderCount;
    private float m_LastFrameOffset;
    private float m_LodTimer;
    private bool m_IsLoading;
    private bool m_EnabledShadersUpdated;

    public uint frameIndex { get; private set; }

    public float frameTime { get; private set; }

    public float frameDelta { get; private set; }

    public float frameLod { get; private set; }

    public float timeOfDay { get; private set; }

    public int lodTimerDelta { get; private set; }

    public float frameOffset { get; set; }

    public bool hideOverlay { get; set; }

    public bool unspawnedVisible { get; set; }

    public bool markersVisible { get; set; }

    public float levelOfDetail { get; set; }

    public bool lodCrossFade { get; set; }

    public int maxLightCount { get; set; }

    public bool debugCrossFade { get; set; }

    public bool disableLodModels { get; set; }

    public float4 editorBuildingStateOverride { get; set; }

    public float loadingProgress
    {
      get => TaskManager.instance.GetTaskProgress("LoadMeshes");
      private set
      {
        TaskManager.instance.progress.Report(new ProgressTracker("LoadMeshes", ProgressTracker.Group.Group2)
        {
          progress = value
        });
      }
    }

    public bool motionVectors { get; private set; }

    public IReadOnlyDictionary<Shader, bool> enabledShaders
    {
      get => (IReadOnlyDictionary<Shader, bool>) this.m_EnabledShaders;
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PlanetarySystem = this.World.GetOrCreateSystemManaged<PlanetarySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem = this.World.GetOrCreateSystemManaged<UpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchManagerSystem = this.World.GetOrCreateSystemManaged<BatchManagerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ManagedBatchSystem = this.World.GetOrCreateSystemManaged<ManagedBatchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchMeshSystem = this.World.GetOrCreateSystemManaged<BatchMeshSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaBatchSystem = this.World.GetOrCreateSystemManaged<AreaBatchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledShaders = new Dictionary<Shader, bool>();
      this.levelOfDetail = 0.5f;
      this.maxLightCount = 2048;
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSettingGroup = this.GetEntityQuery(ComponentType.ReadOnly<TimeSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TimeDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
      // ISSUE: reference to a compiler-generated method
      this.motionVectors = this.GetMotionVectorsEnabled();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem.Update(SystemUpdatePhase.Rendering);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_IsLoading)
        return;
      if ((double) this.loadingProgress != 1.0)
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateLoadingProgress();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_IsLoading = false;
      }
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      // ISSUE: reference to a compiler-generated field
      this.m_TotalLoadingCount = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_IsLoading = true;
      this.frameLod = 0.0f;
    }

    private void UpdateLoadingProgress()
    {
      // ISSUE: reference to a compiler-generated field
      int x = this.m_BatchMeshSystem.loadingRemaining;
      if ((double) this.frameLod < (double) this.levelOfDetail)
        x = math.max(1, (int) ((double) x * (double) this.levelOfDetail / (double) math.max(this.frameLod, this.levelOfDetail * 0.01f)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TotalLoadingCount = math.max(x, this.m_TotalLoadingCount);
      if (x > 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.loadingProgress = math.clamp((float) (this.m_TotalLoadingCount - x) / (float) this.m_TotalLoadingCount, 0.0f, 0.99999f);
      }
      else
      {
        this.loadingProgress = 1f;
        // ISSUE: reference to a compiler-generated field
        this.m_IsLoading = false;
      }
    }

    public void PrepareRendering()
    {
      int b = 15;
      // ISSUE: reference to a compiler-generated field
      if ((double) this.m_LastFrameOffset != (double) this.frameOffset)
      {
        float x = this.frameOffset * (float) b;
        int num = (int) math.floor(x);
        uint frameIndex = this.frameIndex;
        float frameTime = this.frameTime;
        // ISSUE: reference to a compiler-generated field
        this.frameIndex = this.m_SimulationSystem.frameIndex + (uint) num;
        this.frameTime = x - (float) num;
        this.frameDelta = (float) ((int) this.frameIndex - (int) frameIndex) + (this.frameTime - frameTime);
        // ISSUE: reference to a compiler-generated field
        this.m_LastFrameOffset = this.frameOffset;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if ((double) this.m_SimulationSystem.selectedSpeed < 9.9999997473787516E-06)
        {
          this.frameDelta = 0.0f;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          int num1 = (int) this.frameIndex - (int) this.m_SimulationSystem.frameIndex;
          // ISSUE: reference to a compiler-generated field
          float frameTime1 = this.m_SimulationSystem.frameTime;
          // ISSUE: reference to a compiler-generated field
          float num2 = (float) ((double) num1 + (double) this.frameTime + (double) UnityEngine.Time.deltaTime * (double) this.m_SimulationSystem.smoothSpeed * 60.0) - frameTime1;
          float num3 = (float) b * 1.27323949f;
          float num4 = math.clamp(math.atan(num2 / num3) * num3, (float) -b, (float) b);
          float x = frameTime1 + num4;
          int num5 = (int) math.floor(x);
          uint frameIndex = this.frameIndex;
          float frameTime2 = this.frameTime;
          // ISSUE: reference to a compiler-generated field
          if (num5 < 0 && this.m_SimulationSystem.frameIndex < (uint) -num5)
          {
            this.frameIndex = 0U;
            this.frameTime = 0.0f;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.frameIndex = this.m_SimulationSystem.frameIndex + (uint) num5;
            this.frameTime = math.saturate(x - (float) num5);
          }
          this.frameDelta = (float) ((int) this.frameIndex - (int) frameIndex) + (this.frameTime - frameTime2);
          if ((double) this.frameDelta < 0.0)
          {
            this.frameIndex = frameIndex;
            this.frameTime = frameTime2;
            this.frameDelta = 0.0f;
          }
          this.frameOffset = math.clamp((x - frameTime1) / (float) b, -1f, 1f);
          // ISSUE: reference to a compiler-generated field
          this.m_LastFrameOffset = this.frameOffset;
        }
      }
      Shader.SetGlobalVector("colossal_SimulationTime", (Vector4) (((float2) (this.frameIndex % new uint2(60U, 3600U)) + new float2(this.frameTime)).xyxy * new float4(0.0166666675f, 0.000277777785f, (float) Math.PI / 30f, 0.00174532935f)));
      Shader.SetGlobalFloat("colossal_SimulationTime2", (float) (this.frameIndex % 216000U) + this.frameTime);
      TimeSettingsData settings;
      TimeData data;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.timeOfDay = !this.m_TimeSettingGroup.TryGetSingleton<TimeSettingsData>(out settings) || !this.m_TimeDataQuery.TryGetSingleton<TimeData>(out data) ? -1f : this.m_TimeSystem.GetTimeOfDay(settings, data, (double) (this.frameIndex - data.m_FirstFrame) + (double) this.frameTime);
      // ISSUE: reference to a compiler-generated method
      this.motionVectors = this.GetMotionVectorsEnabled();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (this.m_BatchManagerSystem.CheckPropertyUpdates())
      {
        this.frameLod = 0.0f;
        this.lodTimerDelta = (int) byte.MaxValue;
      }
      else if (this.lodCrossFade)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LodTimer += UnityEngine.Time.deltaTime * (this.debugCrossFade ? 102f : 1020f);
        // ISSUE: reference to a compiler-generated field
        this.lodTimerDelta = Mathf.FloorToInt(this.m_LodTimer);
        // ISSUE: reference to a compiler-generated field
        this.m_LodTimer -= (float) this.lodTimerDelta;
        this.lodTimerDelta = math.clamp(this.lodTimerDelta, 0, (int) byte.MaxValue);
      }
      else
        this.lodTimerDelta = (int) byte.MaxValue;
      this.frameLod = math.min(this.frameLod + this.levelOfDetail * 0.01f, this.levelOfDetail);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_EnabledShadersUpdated)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledShadersUpdated = false;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ManagedBatchSystem.EnabledShadersUpdated();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaBatchSystem.EnabledShadersUpdated();
    }

    public bool IsShaderEnabled(Shader shader)
    {
      bool flag1;
      // ISSUE: reference to a compiler-generated field
      if (this.m_EnabledShaders.TryGetValue(shader, out flag1))
        return flag1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      bool flag2 = this.m_EnabledShaderCount != 0 || this.m_EnabledShaders.Count == 0;
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledShaderCount += flag2 ? 1 : 0;
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledShaders.Add(shader, flag2);
      return flag2;
    }

    public void SetShaderEnabled(Shader shader, bool isEnabled)
    {
      // ISSUE: reference to a compiler-generated method
      if (this.IsShaderEnabled(shader) == isEnabled)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledShaderCount += isEnabled ? 1 : -1;
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledShaders[shader] = isEnabled;
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledShadersUpdated = true;
    }

    private bool GetMotionVectorsEnabled() => true;

    public void PostDeserialize(Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.frameIndex = this.m_SimulationSystem.frameIndex;
      // ISSUE: reference to a compiler-generated field
      this.frameTime = this.m_SimulationSystem.frameTime;
      this.frameDelta = 0.0f;
      this.frameOffset = 1f;
      // ISSUE: reference to a compiler-generated field
      this.m_LastFrameOffset = 1f;
    }

    public float3 GetShadowCullingData()
    {
      float3 shadowCullingData = new float3(2048f, 1f, 1f);
      SharedSettings instance = SharedSettings.instance;
      if (instance != null && instance.graphics != null)
      {
        ShadowsQualitySettings qualitySetting = instance.graphics.GetQualitySetting<ShadowsQualitySettings>();
        if (qualitySetting != null)
        {
          shadowCullingData.y = qualitySetting.shadowCullingThresholdHeight;
          shadowCullingData.z = qualitySetting.shadowCullingThresholdVolume;
        }
      }
      return shadowCullingData;
    }

    [Preserve]
    public RenderingSystem()
    {
    }
  }
}
