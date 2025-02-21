// Decompiled with JetBrains decompiler
// Type: Game.Rendering.ClimateRenderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Mathematics;
using Game.Audio;
using Game.Common;
using Game.Prefabs;
using Game.Prefabs.Climate;
using Game.Rendering.Climate;
using Game.Rendering.Utilities;
using Game.Simulation;
using Game.Tools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Scripting;
using UnityEngine.VFX;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class ClimateRenderSystem : GameSystemBase
  {
    private RenderingSystem m_RenderingSystem;
    private ClimateSystem m_ClimateSystem;
    private SimulationSystem m_SimulationSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private PrefabSystem m_PrefabSystem;
    private WindTextureSystem m_WindTextureSystem;
    private TerrainSystem m_TerrainSystem;
    private TimeSystem m_TimeSystem;
    private AudioManager m_AudioManager;
    public bool globalEffectTimeStepFromSimulation;
    public bool weatherEffectTimeStepFromSimulation = true;
    private static VisualEffectAsset s_PrecipitationVFXAsset;
    private VisualEffect m_PrecipitationVFX;
    private static VisualEffectAsset s_LightningVFXAsset;
    private VisualEffect m_LightningVFX;
    private Volume m_ClimateControlVolume;
    private VolumetricClouds m_VolumetricClouds;
    private WindVolumeComponent m_Wind;
    private WindControl m_WindControl;
    private bool m_IsRaining;
    private bool m_IsSnowing;
    private bool m_HailStorm;
    private EntityQuery m_EventQuery;
    private NativeQueue<Game.Events.LightningStrike> m_LightningStrikeQueue;
    private JobHandle m_LightningStrikeDeps;
    private WeatherPropertiesStack m_PropertiesStack;
    private readonly List<WeatherPrefab> m_FromWeatherPrefabs = new List<WeatherPrefab>();
    private readonly List<WeatherPrefab> m_ToWeatherPrefabs = new List<WeatherPrefab>();
    private bool m_PropertiesChanged;
    private ClimateRenderSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_100321765_0;
    private EntityQuery __query_100321765_1;

    public float precipitationVolumeScale { get; set; } = 30f;

    public bool editMode { get; set; }

    public bool pauseSimulationOnLightning { get; set; }

    internal WeatherPropertiesStack propertiesStack => this.m_PropertiesStack;

    public IReadOnlyList<WeatherPrefab> fromWeatherPrefabs
    {
      get => (IReadOnlyList<WeatherPrefab>) this.m_FromWeatherPrefabs;
    }

    public IReadOnlyList<WeatherPrefab> toWeatherPrefabs
    {
      get => (IReadOnlyList<WeatherPrefab>) this.m_ToWeatherPrefabs;
    }

    private void SetData(
      WeatherPropertiesStack stack,
      IReadOnlyList<WeatherPrefab> fromPrefab,
      IReadOnlyList<WeatherPrefab> toPrefab)
    {
      for (int index = 0; index < fromPrefab.Count; ++index)
      {
        foreach (OverrideablePropertiesComponent overrideableProperty in (IEnumerable<OverrideablePropertiesComponent>) fromPrefab[index].overrideableProperties)
        {
          if (overrideableProperty.active && !overrideableProperty.hasTimeBasedInterpolation)
            stack.SetFrom(overrideableProperty.GetType(), overrideableProperty);
        }
      }
      for (int index = 0; index < toPrefab.Count; ++index)
      {
        WeatherPrefab weatherPrefab = toPrefab[index];
        foreach (OverrideablePropertiesComponent overrideableProperty in (IEnumerable<OverrideablePropertiesComponent>) weatherPrefab.overrideableProperties)
        {
          if (overrideableProperty.active)
          {
            if (overrideableProperty.hasTimeBasedInterpolation)
              stack.SetTarget(overrideableProperty.GetType(), overrideableProperty);
            else
              stack.SetTo(overrideableProperty.GetType(), overrideableProperty, index == 1, new Bounds1(weatherPrefab.m_CloudinessRange));
          }
        }
      }
    }

    public void Clear()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PropertiesChanged = true;
      // ISSUE: reference to a compiler-generated field
      this.m_FromWeatherPrefabs.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_ToWeatherPrefabs.Clear();
    }

    public void ScheduleFrom(WeatherPrefab prefab) => this.m_FromWeatherPrefabs.Add(prefab);

    public void ScheduleTo(WeatherPrefab prefab) => this.m_ToWeatherPrefabs.Add(prefab);

    private float GetTimeOfYear()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ClimateSystem.currentDate.overrideState)
      {
        // ISSUE: reference to a compiler-generated field
        return this.m_ClimateSystem.currentDate.overrideValue;
      }
      TimeSettingsData settings;
      TimeData data;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.__query_100321765_0.TryGetSingleton<TimeSettingsData>(out settings) || !this.__query_100321765_1.TryGetSingleton<TimeData>(out data))
        return 0.5f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      double renderingFrame = (double) (this.m_RenderingSystem.frameIndex - data.m_FirstFrame) + (double) this.m_RenderingSystem.frameTime;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return this.m_TimeSystem.GetTimeOfYear(settings, data, renderingFrame);
    }

    private void UpdateWeather()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ClimateSystem.ClimateSample sample = this.m_ClimateSystem.SampleClimate(this.GetTimeOfYear());
      // ISSUE: reference to a compiler-generated field
      if (this.m_PropertiesChanged)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.SetData(this.m_PropertiesStack, (IReadOnlyList<WeatherPrefab>) this.m_FromWeatherPrefabs, (IReadOnlyList<WeatherPrefab>) this.m_ToWeatherPrefabs);
        // ISSUE: reference to a compiler-generated field
        this.m_PropertiesChanged = false;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PropertiesStack.InterpolateOverrideData(this.CheckedStateRef.WorldUnmanaged.Time.DeltaTime, this.m_RenderingSystem.frameDelta / 60f, sample, this.editMode);
    }

    private void UpdateEffectsState()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((!this.m_ClimateSystem.isPrecipitating ? 0 : ((double) this.m_ClimateSystem.hail < 1.0 / 1000.0 ? 1 : 0)) != 0)
      {
        // ISSUE: reference to a compiler-generated field
        if ((double) (float) this.m_ClimateSystem.temperature > 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_IsSnowing)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateEffectState(ClimateRenderSystem.PrecipitationType.Snow, false);
            // ISSUE: reference to a compiler-generated field
            this.m_IsSnowing = false;
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_IsRaining)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateEffectState(ClimateRenderSystem.PrecipitationType.Rain, true);
            // ISSUE: reference to a compiler-generated field
            this.m_IsRaining = true;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_IsSnowing)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateEffectState(ClimateRenderSystem.PrecipitationType.Snow, true);
            // ISSUE: reference to a compiler-generated field
            this.m_IsSnowing = true;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_IsRaining)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateEffectState(ClimateRenderSystem.PrecipitationType.Rain, false);
            // ISSUE: reference to a compiler-generated field
            this.m_IsRaining = false;
          }
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_IsRaining)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateEffectState(ClimateRenderSystem.PrecipitationType.Rain, false);
          // ISSUE: reference to a compiler-generated field
          this.m_IsRaining = false;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_IsSnowing)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateEffectState(ClimateRenderSystem.PrecipitationType.Snow, false);
          // ISSUE: reference to a compiler-generated field
          this.m_IsSnowing = false;
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_HailStorm && (double) this.m_ClimateSystem.hail <= 1.0 / 1000.0)
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateEffectState(ClimateRenderSystem.PrecipitationType.Hail, false);
        // ISSUE: reference to a compiler-generated field
        this.m_HailStorm = false;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_HailStorm || (double) this.m_ClimateSystem.hail <= 1.0 / 1000.0)
          return;
        // ISSUE: reference to a compiler-generated method
        this.UpdateEffectState(ClimateRenderSystem.PrecipitationType.Hail, true);
        // ISSUE: reference to a compiler-generated field
        this.m_HailStorm = true;
      }
    }

    public bool IsAsync { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WindTextureSystem = this.World.GetOrCreateSystemManaged<WindTextureSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioManager = this.World.GetOrCreateSystemManaged<AudioManager>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateControlVolume = VolumeHelper.CreateVolume("ClimateControlVolume", 50);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PropertiesStack = new WeatherPropertiesStack(this.m_ClimateControlVolume);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      VolumeHelper.GetOrCreateVolumeComponent<VolumetricClouds>(this.m_ClimateControlVolume, ref this.m_VolumetricClouds);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      VolumeHelper.GetOrCreateVolumeComponent<WindVolumeComponent>(this.m_ClimateControlVolume, ref this.m_Wind);
      // ISSUE: reference to a compiler-generated field
      this.m_WindControl = WindControl.instance;
      // ISSUE: reference to a compiler-generated method
      this.ResetOverrides();
      // ISSUE: reference to a compiler-generated field
      ClimateRenderSystem.s_PrecipitationVFXAsset = Resources.Load<VisualEffectAsset>("Precipitation/PrecipitationVFX");
      // ISSUE: reference to a compiler-generated field
      ClimateRenderSystem.s_LightningVFXAsset = Resources.Load<VisualEffectAsset>("Lightning/LightningBolt");
      // ISSUE: reference to a compiler-generated field
      this.m_EventQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Events.WeatherPhenomenon>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_LightningStrikeQueue = new NativeQueue<Game.Events.LightningStrike>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    private void ResetOverrides() => this.m_VolumetricClouds.SetAllOverridesTo(false);

    public NativeQueue<Game.Events.LightningStrike> GetLightningStrikeQueue(
      out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_LightningStrikeDeps;
      // ISSUE: reference to a compiler-generated field
      return this.m_LightningStrikeQueue;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_CameraUpdateSystem.activeViewer != null)
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateWeather();
        // ISSUE: reference to a compiler-generated method
        this.UpdateVolumetricClouds();
        // ISSUE: reference to a compiler-generated method
        this.CreateDynamicVFXIfNeeded();
        // ISSUE: reference to a compiler-generated method
        this.UpdateEffectsState();
        // ISSUE: reference to a compiler-generated method
        this.UpdateEffectsProperties();
        // ISSUE: reference to a compiler-generated method
        this.UpdateVFXSpeed();
      }
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<Entity> entityArray = this.m_EventQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        for (int index = 0; index < entityArray.Length; ++index)
        {
          Entity entity = entityArray[index];
          EntityManager entityManager = this.EntityManager;
          if ((double) entityManager.GetComponentData<Game.Events.WeatherPhenomenon>(entity).m_Intensity != 0.0)
          {
            entityManager = this.EntityManager;
            entityManager.GetComponentData<InterpolatedTransform>(entity);
            entityManager = this.EntityManager;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_PrefabSystem.GetPrefab<EventPrefab>(entityManager.GetComponentData<PrefabRef>(entity)).GetComponent<Game.Prefabs.WeatherPhenomenon>();
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_LightningStrikeDeps.Complete();
      Game.Events.LightningStrike lightningStrike;
      // ISSUE: reference to a compiler-generated field
      while (this.m_LightningStrikeQueue.TryDequeue(out lightningStrike))
      {
        // ISSUE: reference to a compiler-generated method
        this.LightningStrike(lightningStrike.m_Position, lightningStrike.m_Position);
      }
    }

    public void AddLightningStrikeWriter(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LightningStrikeDeps = jobHandle;
    }

    private void UpdateVolumetricClouds()
    {
      // ISSUE: reference to a compiler-generated field
      float num = (float) (1.0 + (1.0 - (double) math.abs(math.dot(this.m_CameraUpdateSystem.direction, new float3(0.0f, 1f, 0.0f)))));
      // ISSUE: reference to a compiler-generated field
      this.m_VolumetricClouds.fadeInMode.Override(VolumetricClouds.CloudFadeInMode.Manual);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_VolumetricClouds.fadeInStart.Override(math.max((this.m_CameraUpdateSystem.position.y - this.m_VolumetricClouds.bottomAltitude.value) * num, this.m_CameraUpdateSystem.nearClipPlane));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_VolumetricClouds.fadeInDistance.Override(this.m_VolumetricClouds.altitudeRange.value * 0.3f);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_VolumetricClouds.renderHook.Override((double) this.m_CameraUpdateSystem.position.y < (double) this.m_VolumetricClouds.bottomAltitude.value ? VolumetricClouds.CloudHook.PreTransparent : VolumetricClouds.CloudHook.PostTransparent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LightningStrikeDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_LightningStrikeQueue.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_WindControl.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_PropertiesStack.Dispose();
      // ISSUE: reference to a compiler-generated field
      VolumeHelper.DestroyVolume(this.m_ClimateControlVolume);
      base.OnDestroy();
    }

    private void CreateDynamicVFXIfNeeded()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!((Object) ClimateRenderSystem.s_PrecipitationVFXAsset != (Object) null) || !((Object) this.m_PrecipitationVFX == (Object) null))
        return;
      COSystemBase.baseLog.DebugFormat("Creating VFXs pool");
      // ISSUE: reference to a compiler-generated field
      this.m_PrecipitationVFX = new GameObject("PrecipitationVFX").AddComponent<VisualEffect>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PrecipitationVFX.visualEffectAsset = ClimateRenderSystem.s_PrecipitationVFXAsset;
      // ISSUE: reference to a compiler-generated field
      this.m_LightningVFX = new GameObject("LightningVFX").AddComponent<VisualEffect>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LightningVFX.visualEffectAsset = ClimateRenderSystem.s_LightningVFXAsset;
    }

    public void LightningStrike(float3 start, float3 target, bool useCloudsAltitude = true)
    {
      if (this.pauseSimulationOnLightning)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SimulationSystem.selectedSpeed = 0.0f;
      }
      if (useCloudsAltitude)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        start.y = this.m_VolumetricClouds.bottomAltitude.value + this.m_VolumetricClouds.altitudeRange.value * 0.1f;
      }
      COSystemBase.baseLog.DebugFormat("Lightning strike {0}->{1}", (object) start, (object) target);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LightningVFX.SetVector3(ClimateRenderSystem.VFXIDs.LightningOrigin, (Vector3) start);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LightningVFX.SetVector3(ClimateRenderSystem.VFXIDs.LightningTarget, (Vector3) target);
      // ISSUE: reference to a compiler-generated field
      this.m_LightningVFX.SendEvent("OnPlay");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AudioManager.PlayLightningSFX((start - target) / 2f);
    }

    private bool GetEventName(
      ClimateRenderSystem.PrecipitationType type,
      bool start,
      out string name)
    {
      switch (type)
      {
        case ClimateRenderSystem.PrecipitationType.Rain:
          name = start ? "OnRainStart" : "OnRainStop";
          return true;
        case ClimateRenderSystem.PrecipitationType.Snow:
          name = start ? "OnSnowStart" : "OnSnowStop";
          return true;
        case ClimateRenderSystem.PrecipitationType.Hail:
          name = start ? "OnHailStart" : "OnHailStop";
          return true;
        default:
          name = (string) null;
          return false;
      }
    }

    private void UpdateEffectState(ClimateRenderSystem.PrecipitationType type, bool start)
    {
      string name;
      // ISSUE: reference to a compiler-generated method
      if (!this.GetEventName(type, start, out name))
        return;
      COSystemBase.baseLog.DebugFormat("PrecipitationVFX event {0}", (object) name);
      // ISSUE: reference to a compiler-generated field
      this.m_PrecipitationVFX.SendEvent(name);
    }

    private void UpdateEffectsProperties()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PrecipitationVFX.SetCheckedVector3(ClimateRenderSystem.VFXIDs.CameraPosition, (Vector3) this.m_CameraUpdateSystem.position);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PrecipitationVFX.SetCheckedVector3(ClimateRenderSystem.VFXIDs.CameraDirection, (Vector3) this.m_CameraUpdateSystem.direction);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PrecipitationVFX.SetCheckedVector3(ClimateRenderSystem.VFXIDs.VolumeScale, new Vector3(this.precipitationVolumeScale, this.precipitationVolumeScale, this.precipitationVolumeScale));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PrecipitationVFX.SetCheckedTexture(ClimateRenderSystem.VFXIDs.WindTexture, (Texture) this.m_WindTextureSystem.WindTexture);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PrecipitationVFX.SetCheckedFloat(ClimateRenderSystem.VFXIDs.CloudsAltitude, this.m_VolumetricClouds.bottomAltitude.value);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PrecipitationVFX.SetCheckedVector4(ClimateRenderSystem.VFXIDs.MapOffsetScale, this.m_TerrainSystem.mapOffsetScale);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PrecipitationVFX.SetCheckedFloat(ClimateRenderSystem.VFXIDs.RainStrength, (float) this.m_ClimateSystem.precipitation);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PrecipitationVFX.SetCheckedFloat(ClimateRenderSystem.VFXIDs.SnowStrength, (float) this.m_ClimateSystem.precipitation);
    }

    private void UpdateVFXSpeed()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.globalEffectTimeStepFromSimulation)
      {
        // ISSUE: reference to a compiler-generated field
        float num = this.m_RenderingSystem.frameDelta / 60f;
        // ISSUE: reference to a compiler-generated field
        float smoothSpeed = this.m_SimulationSystem.smoothSpeed;
        VFXManager.fixedTimeStep = num * smoothSpeed;
        UnityEngine.Debug.Log((object) ("smoothedRenderTimeStep: " + num.ToString() + " simulationSpeedMultiplier: " + smoothSpeed.ToString()));
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        float num = this.m_RenderingSystem.frameDelta / math.max(1E-06f, this.CheckedStateRef.WorldUnmanaged.Time.DeltaTime * 60f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrecipitationVFX.playRate = this.weatherEffectTimeStepFromSimulation ? num : 1f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_LightningVFX.playRate = this.weatherEffectTimeStepFromSimulation ? num : 1f;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_100321765_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<TimeSettingsData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_100321765_1 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<TimeData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [Preserve]
    public ClimateRenderSystem()
    {
    }

    private static class VFXIDs
    {
      public static readonly int CameraPosition = Shader.PropertyToID(nameof (CameraPosition));
      public static readonly int CameraDirection = Shader.PropertyToID(nameof (CameraDirection));
      public static readonly int VolumeScale = Shader.PropertyToID(nameof (VolumeScale));
      public static readonly int WindTexture = Shader.PropertyToID(nameof (WindTexture));
      public static readonly int CloudsAltitude = Shader.PropertyToID(nameof (CloudsAltitude));
      public static readonly int MapOffsetScale = Shader.PropertyToID(nameof (MapOffsetScale));
      public static readonly int RainStrength = Shader.PropertyToID(nameof (RainStrength));
      public static readonly int SnowStrength = Shader.PropertyToID(nameof (SnowStrength));
      public static readonly int LightningOrigin = Shader.PropertyToID(nameof (LightningOrigin));
      public static readonly int LightningTarget = Shader.PropertyToID(nameof (LightningTarget));
    }

    private enum PrecipitationType
    {
      Rain,
      Snow,
      Hail,
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct TypeHandle
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
      }
    }
  }
}
