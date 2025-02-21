// Decompiled with JetBrains decompiler
// Type: Game.Rendering.LightingSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Game.Settings;
using Game.Simulation;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Rendering
{
  public class LightingSystem : GameSystemBase
  {
    private PlanetarySystem m_PlanetarySystem;
    protected EntityQuery m_TimeSettingGroup;
    private Exposure m_Exposure;
    private PhysicallyBasedSky m_PhysicallyBasedSky;
    private ColorAdjustments m_ColorAdjustments;
    private IndirectLightingController m_Indirect;
    private Tonemapping m_Tonemap;
    private bool m_PostProcessingSetup;
    private DayNightCycleData m_NightDayCycleData;
    private Volume m_Volume;
    private VolumeProfile m_Profile;
    private RenderTexture m_BlendResult;
    private ComputeShader m_LUTBlend;
    private int m_KernalBlend = -1;
    private LightingSystem.State m_LastState = LightingSystem.State.Invalid;
    private float m_LastDelta = -1f;

    private bool shadowDisabled { get; set; }

    public float dayLightBrightness { get; private set; }

    public LightingSystem.State state
    {
      get
      {
        PlanetarySystem.LightData sunLight = this.m_PlanetarySystem.SunLight;
        return !sunLight.isValid ? LightingSystem.State.Invalid : this.CalculateState((float3) sunLight.transform.position, (float3) sunLight.transform.forward, out float _);
      }
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_PlanetarySystem = this.World.GetOrCreateSystemManaged<PlanetarySystem>();
      this.m_NightDayCycleData = Resources.Load<DayNightCycleData>("DayNight/Default");
      this.SetupPostprocessing();
      this.m_LUTBlend = Resources.Load<ComputeShader>("DayNight/LUTBlend");
      if ((bool) (Object) this.m_LUTBlend)
      {
        this.m_KernalBlend = this.m_LUTBlend.FindKernel("CSBlend");
        this.m_BlendResult = new RenderTexture(new RenderTextureDescriptor()
        {
          autoGenerateMips = false,
          bindMS = false,
          depthBufferBits = 0,
          dimension = TextureDimension.Tex3D,
          enableRandomWrite = true,
          graphicsFormat = GraphicsFormat.R16G16B16A16_SFloat,
          memoryless = RenderTextureMemoryless.None,
          height = 32,
          width = 32,
          volumeDepth = 32,
          mipCount = 1,
          msaaSamples = 1,
          sRGB = false,
          useDynamicScale = false,
          useMipMap = false
        });
        this.m_BlendResult.Create();
      }
      this.m_TimeSettingGroup = this.GetEntityQuery(ComponentType.ReadOnly<TimeSettingsData>());
      this.RequireForUpdate(this.m_TimeSettingGroup);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      if ((Object) this.m_Volume != (Object) null)
        Object.Destroy((Object) this.m_Volume.gameObject);
      if (!((Object) this.m_BlendResult != (Object) null))
        return;
      Object.Destroy((Object) this.m_BlendResult);
    }

    private float CalcObscured(
      PlanetarySystem.LightData moon,
      PlanetarySystem.LightData night,
      float range = 0.3f)
    {
      float y1 = moon.transform.position.y;
      float y2 = night.transform.position.y;
      return (double) y1 != (double) y2 ? math.clamp((y2 - y1) / range, 0.0f, 1f) : 0.0f;
    }

    private void EnableShadows(PlanetarySystem.LightData lightData, bool enabled)
    {
      lightData.additionalData.EnableShadows(enabled && !this.shadowDisabled);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      ShadowsQualitySettings qualitySetting = SharedSettings.instance?.graphics?.GetQualitySetting<ShadowsQualitySettings>();
      if (qualitySetting != null)
        this.shadowDisabled = !qualitySetting.enabled;
      // ISSUE: variable of a compiler-generated type
      PlanetarySystem.LightData moonLight = this.m_PlanetarySystem.MoonLight;
      // ISSUE: variable of a compiler-generated type
      PlanetarySystem.LightData sunLight = this.m_PlanetarySystem.SunLight;
      // ISSUE: variable of a compiler-generated type
      PlanetarySystem.LightData nightLight = this.m_PlanetarySystem.NightLight;
      if (!sunLight.isValid || !moonLight.isValid || !nightLight.isValid || (Object) this.m_NightDayCycleData == (Object) null)
        return;
      this.dayLightBrightness = math.saturate(sunLight.additionalData.intensity / 110000f);
      float delta;
      LightingSystem.State state = this.CalculateState((float3) sunLight.transform.position, (float3) sunLight.transform.forward, out delta);
      if (this.m_PlanetarySystem.overrideTime)
      {
        this.m_LastState = LightingSystem.State.Invalid;
      }
      else
      {
        if (state == this.m_LastState && (double) delta < (double) this.m_LastDelta || this.NextState(state) == this.m_LastState)
        {
          state = this.m_LastState;
          delta = this.m_LastDelta;
        }
        this.m_LastState = state;
        this.m_LastDelta = delta;
      }
      float s = this.CalcObscured(moonLight, nightLight, this.m_NightDayCycleData.NightLightObscuredRange);
      float num1 = math.lerp(this.m_NightDayCycleData.NightLightIntensity, this.m_NightDayCycleData.NightLightObscuredIntensity, s);
      float num2 = this.m_NightDayCycleData.MoonIntensity - (num1 - this.m_NightDayCycleData.NightLightIntensity);
      float num3 = math.lerp(this.m_NightDayCycleData.NightIndirectReflectiveMultiplier, this.m_NightDayCycleData.NightObscuredIndirectReflectiveMultiplier, s);
      float num4 = math.lerp(this.m_NightDayCycleData.NightIndirectDiffuseMultiplier, this.m_NightDayCycleData.NightObscuredIndirectDiffuseMultiplier, s);
      if (!this.m_NightDayCycleData.UseLUT)
      {
        this.m_Tonemap.lutTexture.value = (Texture) null;
        this.m_Tonemap.lutContribution.value = this.m_NightDayCycleData.LutContribution;
      }
      this.m_Tonemap.mode.overrideState = this.m_NightDayCycleData.UseLUT;
      this.m_ColorAdjustments.colorFilter.overrideState = this.m_NightDayCycleData.UseFilters;
      switch (state)
      {
        case LightingSystem.State.Dawn:
          this.m_Exposure.limitMax.value = this.m_NightDayCycleData.NightExposureMax;
          this.m_Exposure.limitMin.value = math.lerp(this.m_NightDayCycleData.NightExposureLowMin, this.m_NightDayCycleData.DayExposureMin, delta);
          moonLight.additionalData.intensity = num2;
          moonLight.additionalData.color = this.m_NightDayCycleData.MoonLightColor;
          bool enabled1 = (double) sunLight.additionalData.intensity > (double) num2;
          this.EnableShadows(moonLight, !enabled1);
          this.EnableShadows(sunLight, enabled1);
          nightLight.additionalData.intensity = num1;
          nightLight.additionalData.color = this.m_NightDayCycleData.MoonLightColor;
          this.EnableShadows(nightLight, false);
          this.m_PhysicallyBasedSky.exposure.value = math.lerp(this.m_NightDayCycleData.NightSkyExposure, this.m_NightDayCycleData.DaySkyExposure, delta);
          this.m_PhysicallyBasedSky.zenithTint.value = Color.Lerp(this.m_NightDayCycleData.NightZenithTint, this.m_NightDayCycleData.DayZenithTint, delta);
          this.m_PhysicallyBasedSky.horizonTint.value = Color.Lerp(this.m_NightDayCycleData.NightHorizonTint, this.m_NightDayCycleData.DayHorizonTint, delta);
          this.m_ColorAdjustments.colorFilter.value = Color.Lerp(this.m_NightDayCycleData.NightColorFilter, this.m_NightDayCycleData.SunriseColorFilter, delta);
          this.m_ColorAdjustments.contrast.value = math.lerp(this.m_NightDayCycleData.NightContrast, this.m_NightDayCycleData.SunriseAndSunsetContrast, delta);
          this.m_Indirect.reflectionLightingMultiplier.value = math.lerp(num3, 1f, delta);
          this.m_Indirect.indirectDiffuseLightingMultiplier.value = math.lerp(num4, 1f, delta);
          if (this.m_NightDayCycleData.UseLUT)
          {
            this.BlendLUT(this.m_NightDayCycleData.NightLUT, this.m_NightDayCycleData.SunriseAndSunsetLUT, delta, this.m_NightDayCycleData.LutContribution);
            break;
          }
          break;
        case LightingSystem.State.Sunrise:
          this.m_Exposure.limitMax.value = this.m_NightDayCycleData.DayExposureMax;
          this.m_Exposure.limitMin.value = this.m_NightDayCycleData.DayExposureMin;
          moonLight.additionalData.intensity = math.lerp(num2, 0.5f, delta);
          moonLight.additionalData.color = this.m_NightDayCycleData.MoonLightColor;
          this.EnableShadows(moonLight, false);
          nightLight.additionalData.intensity = math.lerp(num1, 0.5f, delta);
          nightLight.additionalData.color = this.m_NightDayCycleData.MoonLightColor;
          this.EnableShadows(nightLight, false);
          this.EnableShadows(sunLight, true);
          this.m_PhysicallyBasedSky.exposure.value = this.m_NightDayCycleData.DaySkyExposure;
          this.m_PhysicallyBasedSky.zenithTint.value = this.m_NightDayCycleData.DayZenithTint;
          this.m_PhysicallyBasedSky.horizonTint.value = this.m_NightDayCycleData.DayHorizonTint;
          this.m_ColorAdjustments.colorFilter.value = Color.Lerp(this.m_NightDayCycleData.SunriseColorFilter, this.m_NightDayCycleData.DayColorFilter, delta);
          this.m_ColorAdjustments.contrast.value = math.lerp(this.m_NightDayCycleData.SunriseAndSunsetContrast, this.m_NightDayCycleData.DayContrast, delta);
          this.m_Indirect.reflectionLightingMultiplier.value = 1f;
          this.m_Indirect.indirectDiffuseLightingMultiplier.value = 1f;
          if (this.m_NightDayCycleData.UseLUT)
          {
            this.BlendLUT(this.m_NightDayCycleData.SunriseAndSunsetLUT, this.m_NightDayCycleData.DayLUT, delta, this.m_NightDayCycleData.LutContribution);
            break;
          }
          break;
        case LightingSystem.State.Day:
          this.m_Exposure.limitMax.value = this.m_NightDayCycleData.DayExposureMax;
          this.m_Exposure.limitMin.value = this.m_NightDayCycleData.DayExposureMin;
          this.m_PhysicallyBasedSky.exposure.value = this.m_NightDayCycleData.DaySkyExposure;
          this.m_PhysicallyBasedSky.zenithTint.value = this.m_NightDayCycleData.DayZenithTint;
          this.m_PhysicallyBasedSky.horizonTint.value = this.m_NightDayCycleData.DayHorizonTint;
          moonLight.additionalData.intensity = 0.0f;
          moonLight.additionalData.color = this.m_NightDayCycleData.MoonLightColor;
          this.EnableShadows(moonLight, false);
          nightLight.additionalData.intensity = 0.0f;
          nightLight.additionalData.color = this.m_NightDayCycleData.MoonLightColor;
          this.EnableShadows(nightLight, false);
          this.EnableShadows(sunLight, true);
          this.m_ColorAdjustments.colorFilter.value = this.m_NightDayCycleData.DayColorFilter;
          this.m_ColorAdjustments.contrast.value = this.m_NightDayCycleData.DayContrast;
          this.m_Indirect.reflectionLightingMultiplier.value = 1f;
          this.m_Indirect.indirectDiffuseLightingMultiplier.value = 1f;
          if (this.m_NightDayCycleData.UseLUT)
          {
            this.m_Tonemap.lutTexture.value = (Texture) this.m_NightDayCycleData.DayLUT;
            this.m_Tonemap.lutContribution.value = this.m_NightDayCycleData.LutContribution;
            break;
          }
          break;
        case LightingSystem.State.Sunset:
          this.m_Exposure.limitMax.value = this.m_NightDayCycleData.DayExposureMax;
          this.m_Exposure.limitMin.value = this.m_NightDayCycleData.DayExposureMin;
          moonLight.additionalData.intensity = math.lerp(0.5f, num2, delta);
          moonLight.additionalData.color = this.m_NightDayCycleData.MoonLightColor;
          this.EnableShadows(moonLight, false);
          nightLight.additionalData.intensity = math.lerp(0.0f, num1, delta);
          nightLight.additionalData.color = this.m_NightDayCycleData.MoonLightColor;
          this.EnableShadows(nightLight, false);
          this.EnableShadows(sunLight, true);
          this.m_PhysicallyBasedSky.exposure.value = this.m_NightDayCycleData.DaySkyExposure;
          this.m_PhysicallyBasedSky.zenithTint.value = this.m_NightDayCycleData.DayZenithTint;
          this.m_PhysicallyBasedSky.horizonTint.value = this.m_NightDayCycleData.DayHorizonTint;
          this.m_ColorAdjustments.colorFilter.value = Color.Lerp(this.m_NightDayCycleData.DayColorFilter, this.m_NightDayCycleData.SunsetColorFilter, delta);
          this.m_ColorAdjustments.contrast.value = math.lerp(this.m_NightDayCycleData.DayContrast, this.m_NightDayCycleData.SunriseAndSunsetContrast, delta);
          this.m_Indirect.reflectionLightingMultiplier.value = 1f;
          this.m_Indirect.indirectDiffuseLightingMultiplier.value = 1f;
          if (this.m_NightDayCycleData.UseLUT)
          {
            this.BlendLUT(this.m_NightDayCycleData.DayLUT, this.m_NightDayCycleData.SunriseAndSunsetLUT, delta, this.m_NightDayCycleData.LutContribution);
            break;
          }
          break;
        case LightingSystem.State.Dusk:
          this.m_Exposure.limitMax.value = this.m_NightDayCycleData.NightExposureMax;
          this.m_Exposure.limitMin.value = this.m_NightDayCycleData.NightExposureLowMin;
          moonLight.additionalData.intensity = num2;
          moonLight.additionalData.color = this.m_NightDayCycleData.MoonLightColor;
          bool enabled2 = (double) sunLight.additionalData.intensity > (double) num2;
          this.EnableShadows(moonLight, !enabled2);
          this.EnableShadows(sunLight, enabled2);
          nightLight.additionalData.intensity = num1;
          nightLight.additionalData.color = this.m_NightDayCycleData.MoonLightColor;
          this.EnableShadows(nightLight, false);
          this.m_PhysicallyBasedSky.exposure.value = math.lerp(this.m_NightDayCycleData.DaySkyExposure, this.m_NightDayCycleData.NightSkyExposure, delta);
          this.m_PhysicallyBasedSky.zenithTint.value = Color.Lerp(this.m_NightDayCycleData.DayZenithTint, this.m_NightDayCycleData.NightZenithTint, delta);
          this.m_PhysicallyBasedSky.horizonTint.value = Color.Lerp(this.m_NightDayCycleData.DayHorizonTint, this.m_NightDayCycleData.NightHorizonTint, delta);
          this.m_ColorAdjustments.colorFilter.value = Color.Lerp(this.m_NightDayCycleData.SunsetColorFilter, this.m_NightDayCycleData.NightColorFilter, delta);
          this.m_ColorAdjustments.contrast.value = math.lerp(this.m_NightDayCycleData.SunriseAndSunsetContrast, this.m_NightDayCycleData.NightContrast, delta);
          this.m_Indirect.reflectionLightingMultiplier.value = math.lerp(1f, num3, delta);
          this.m_Indirect.indirectDiffuseLightingMultiplier.value = math.lerp(1f, num4, delta);
          if (this.m_NightDayCycleData.UseLUT)
          {
            this.BlendLUT(this.m_NightDayCycleData.SunriseAndSunsetLUT, this.m_NightDayCycleData.NightLUT, delta, this.m_NightDayCycleData.LutContribution);
            break;
          }
          break;
        case LightingSystem.State.Night:
          this.m_Exposure.limitMax.value = this.m_NightDayCycleData.NightExposureMax;
          this.m_Exposure.limitMin.value = this.m_NightDayCycleData.NightExposureLowMin;
          this.m_PhysicallyBasedSky.exposure.value = this.m_NightDayCycleData.NightSkyExposure;
          this.m_PhysicallyBasedSky.zenithTint.value = this.m_NightDayCycleData.NightZenithTint;
          this.m_PhysicallyBasedSky.horizonTint.value = this.m_NightDayCycleData.NightHorizonTint;
          moonLight.additionalData.intensity = num2;
          moonLight.additionalData.color = this.m_NightDayCycleData.MoonLightColor;
          this.EnableShadows(moonLight, true);
          this.EnableShadows(sunLight, false);
          moonLight.additionalData.shadowTint = this.m_NightDayCycleData.MoonShadowTint;
          nightLight.additionalData.intensity = num1;
          nightLight.additionalData.color = this.m_NightDayCycleData.MoonLightColor;
          this.EnableShadows(nightLight, false);
          this.m_ColorAdjustments.colorFilter.value = this.m_NightDayCycleData.NightColorFilter;
          this.m_ColorAdjustments.contrast.value = this.m_NightDayCycleData.NightContrast;
          this.m_Indirect.reflectionLightingMultiplier.value = num3;
          this.m_Indirect.indirectDiffuseLightingMultiplier.value = num4;
          if (this.m_NightDayCycleData.UseLUT)
          {
            this.m_Tonemap.lutTexture.value = (Texture) this.m_NightDayCycleData.NightLUT;
            this.m_Tonemap.lutContribution.value = this.m_NightDayCycleData.LutContribution;
            break;
          }
          break;
      }
      this.m_Profile.Reset();
    }

    private LightingSystem.State CalculateState(
      float3 sunPosition,
      float3 sunDirection,
      out float delta)
    {
      if ((Object) this.m_NightDayCycleData == (Object) null)
      {
        delta = 1f;
        return LightingSystem.State.Day;
      }
      float3 float3 = new float3(sunPosition.x, 0.0f, sunPosition.z);
      float3 = (double) math.dot(float3, float3) >= 9.9999997473787516E-05 ? math.normalize(float3) : new float3(-1f, 0.0f, 0.0f);
      float num1 = math.acos(math.dot(-sunDirection, float3)) * 57.2957764f;
      int num2 = (double) float3.x > 0.0 ? 1 : 0;
      if ((double) sunDirection.y > 0.0)
        num1 = -num1;
      if (num2 != 0)
      {
        delta = 1f;
        if ((double) num1 < (double) this.m_NightDayCycleData.DawnStartAngle)
          return LightingSystem.State.Night;
        if ((double) num1 >= (double) this.m_NightDayCycleData.DawnStartAngle && (double) num1 < (double) this.m_NightDayCycleData.SunriseMidpointAngle)
        {
          delta = (float) (((double) num1 - (double) this.m_NightDayCycleData.DawnStartAngle) / ((double) this.m_NightDayCycleData.SunriseMidpointAngle - (double) this.m_NightDayCycleData.DawnStartAngle));
          return LightingSystem.State.Dawn;
        }
        if ((double) num1 < (double) this.m_NightDayCycleData.SunriseMidpointAngle || (double) num1 >= (double) this.m_NightDayCycleData.SunriseEndAngle)
          return LightingSystem.State.Day;
        delta = (float) (((double) num1 - (double) this.m_NightDayCycleData.SunriseMidpointAngle) / ((double) this.m_NightDayCycleData.SunriseEndAngle - (double) this.m_NightDayCycleData.SunriseMidpointAngle));
        return LightingSystem.State.Sunrise;
      }
      delta = 1f;
      if ((double) num1 > (double) this.m_NightDayCycleData.SunsetStartAngle)
        return LightingSystem.State.Day;
      if ((double) num1 <= (double) this.m_NightDayCycleData.SunsetStartAngle && (double) num1 > (double) this.m_NightDayCycleData.SunsetMidpointAngle)
      {
        delta = (float) (1.0 - ((double) num1 - (double) this.m_NightDayCycleData.SunsetMidpointAngle) / ((double) this.m_NightDayCycleData.SunsetStartAngle - (double) this.m_NightDayCycleData.SunsetMidpointAngle));
        return LightingSystem.State.Sunset;
      }
      if ((double) num1 > (double) this.m_NightDayCycleData.SunsetMidpointAngle || (double) num1 <= (double) this.m_NightDayCycleData.DuskEndAngle)
        return LightingSystem.State.Night;
      delta = (float) (1.0 - ((double) num1 - (double) this.m_NightDayCycleData.DuskEndAngle) / ((double) this.m_NightDayCycleData.SunsetMidpointAngle - (double) this.m_NightDayCycleData.DuskEndAngle));
      return LightingSystem.State.Dusk;
    }

    private LightingSystem.State NextState(LightingSystem.State value)
    {
      switch (value)
      {
        case LightingSystem.State.Dawn:
          return LightingSystem.State.Sunrise;
        case LightingSystem.State.Sunrise:
          return LightingSystem.State.Day;
        case LightingSystem.State.Day:
          return LightingSystem.State.Sunset;
        case LightingSystem.State.Sunset:
          return LightingSystem.State.Dusk;
        case LightingSystem.State.Dusk:
          return LightingSystem.State.Night;
        case LightingSystem.State.Night:
          return LightingSystem.State.Dawn;
        default:
          return LightingSystem.State.Invalid;
      }
    }

    private void SetupPostprocessing()
    {
      if (this.m_PostProcessingSetup)
        return;
      GameObject target = new GameObject("LightingPostProcessVolume");
      Object.DontDestroyOnLoad((Object) target);
      this.m_Volume = target.AddComponent<Volume>();
      this.m_Volume.priority = 1000f;
      this.m_Profile = this.m_Volume.profile;
      this.m_Exposure = this.m_Profile.Add<Exposure>();
      this.m_Exposure.active = true;
      this.m_Exposure.mode.value = ExposureMode.Automatic;
      this.m_Exposure.limitMin.overrideState = true;
      this.m_Exposure.limitMin.value = -5f;
      this.m_Exposure.limitMax.overrideState = true;
      this.m_Exposure.limitMax.value = 14f;
      this.m_PhysicallyBasedSky = this.m_Profile.Add<PhysicallyBasedSky>();
      this.m_PhysicallyBasedSky.zenithTint.overrideState = true;
      this.m_PhysicallyBasedSky.horizonTint.overrideState = true;
      this.m_PhysicallyBasedSky.exposure.overrideState = true;
      this.m_PhysicallyBasedSky.exposure.value = 0.0f;
      this.m_ColorAdjustments = this.m_Profile.Add<ColorAdjustments>();
      this.m_ColorAdjustments.colorFilter.overrideState = true;
      this.m_ColorAdjustments.colorFilter.value = new Color(1f, 1f, 1f);
      this.m_ColorAdjustments.contrast.overrideState = true;
      this.m_ColorAdjustments.contrast.value = 0.0f;
      this.m_Indirect = this.m_Profile.Add<IndirectLightingController>();
      this.m_Indirect.reflectionLightingMultiplier.overrideState = true;
      this.m_Indirect.indirectDiffuseLightingMultiplier.overrideState = true;
      this.m_Indirect.reflectionLightingMultiplier.value = 1f;
      this.m_Indirect.indirectDiffuseLightingMultiplier.value = 1f;
      this.m_Tonemap = this.m_Profile.Add<Tonemapping>();
      this.m_Tonemap.mode.overrideState = true;
      this.m_Tonemap.mode.value = TonemappingMode.External;
      this.m_Tonemap.lutContribution.overrideState = true;
      this.m_Tonemap.lutContribution.value = 0.5f;
      this.m_Tonemap.lutTexture.overrideState = true;
      this.m_Tonemap.lutTexture.value = (Texture) null;
      this.m_PostProcessingSetup = true;
    }

    private void BlendLUT(
      Texture3D source,
      Texture3D destination,
      float delta,
      float lutContribution)
    {
      if ((Object) source != (Object) null && (Object) destination != (Object) null)
      {
        if ((Object) this.m_LUTBlend != (Object) null)
        {
          this.m_LUTBlend.SetTexture(this.m_KernalBlend, LightingSystem.ShaderID._TargetLUT, (Texture) this.m_BlendResult);
          this.m_LUTBlend.SetTexture(this.m_KernalBlend, LightingSystem.ShaderID._SourceLUT, (Texture) source);
          this.m_LUTBlend.SetTexture(this.m_KernalBlend, LightingSystem.ShaderID._DestinationLUT, (Texture) destination);
          this.m_LUTBlend.SetFloat(LightingSystem.ShaderID._BlendLUT, math.clamp(delta, 0.0f, 1f));
          this.m_LUTBlend.Dispatch(this.m_KernalBlend, 32, 32, 32);
          this.m_Tonemap.lutTexture.value = (Texture) this.m_BlendResult;
          this.m_Tonemap.lutContribution.value = lutContribution;
        }
        else
        {
          this.m_Tonemap.lutTexture.value = (double) delta < 0.5 ? (Texture) source : (Texture) destination;
          this.m_Tonemap.lutContribution.value = lutContribution;
        }
      }
      else if ((Object) source != (Object) null && (Object) destination == (Object) null)
      {
        this.m_Tonemap.lutTexture.value = (Texture) source;
        this.m_Tonemap.lutContribution.value = math.lerp(lutContribution, 0.0f, delta);
      }
      else if ((Object) destination != (Object) null && (Object) source == (Object) null)
      {
        this.m_Tonemap.lutTexture.value = (Texture) destination;
        this.m_Tonemap.lutContribution.value = math.lerp(0.0f, lutContribution, delta);
      }
      else
      {
        this.m_Tonemap.lutTexture.value = (Texture) null;
        this.m_Tonemap.lutContribution.value = lutContribution;
      }
    }

    [Preserve]
    public LightingSystem()
    {
    }

    private static class ShaderID
    {
      public static readonly int _TargetLUT = Shader.PropertyToID("_ResultLUT");
      public static readonly int _SourceLUT = Shader.PropertyToID(nameof (_SourceLUT));
      public static readonly int _DestinationLUT = Shader.PropertyToID(nameof (_DestinationLUT));
      public static readonly int _BlendLUT = Shader.PropertyToID("_LUTBlend");
    }

    public enum State
    {
      Dawn,
      Sunrise,
      Day,
      Sunset,
      Dusk,
      Night,
      Invalid,
    }
  }
}
