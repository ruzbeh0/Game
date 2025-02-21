// Decompiled with JetBrains decompiler
// Type: Game.Rendering.Utilities.AdaptiveDynamicResolutionScale
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Settings;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#nullable disable
namespace Game.Rendering.Utilities
{
  public class AdaptiveDynamicResolutionScale
  {
    private static AdaptiveDynamicResolutionScale s_Instance;
    public float DefaultTargetFrameRate = 30f;
    public int EvaluationFrameCount = 15;
    public uint ScaleUpDuration = 4;
    public uint ScaleDownDuration = 2;
    public int ScaleUpStepCount = 5;
    public int ScaleDownStepCount = 2;
    private const uint InitialFramesToSkip = 1;
    private float m_AccumGPUFrameTime;
    private float m_GPULimitedFrames;
    private int m_CurrentFrameSlot;
    private uint m_ScaleUpCounter;
    private uint m_ScaleDownCounter;
    private static float s_CurrentScaleFraction = 1f;
    private bool m_Initialized;
    private uint m_InitialFrameCounter;
    private float m_AvgGPUTime;
    private float m_AvgGPULimited;

    public static AdaptiveDynamicResolutionScale instance
    {
      get
      {
        if (AdaptiveDynamicResolutionScale.s_Instance == null)
          AdaptiveDynamicResolutionScale.s_Instance = new AdaptiveDynamicResolutionScale();
        return AdaptiveDynamicResolutionScale.s_Instance;
      }
    }

    public AdaptiveDynamicResolutionScale.DynResUpscaleFilter upscaleFilter { get; private set; }

    public bool isEnabled { get; private set; }

    public bool isAdaptive { get; private set; }

    public float minScale { get; private set; } = 0.5f;

    public float currentScale => AdaptiveDynamicResolutionScale.s_CurrentScaleFraction;

    public string debugState
    {
      get
      {
        return !this.isAdaptive ? string.Format("Scale {0:F2}", (object) this.currentScale) : string.Format("Scale {0:F2} GPU lim {1:P1} dur {2:F1}ms", (object) this.currentScale, (object) this.m_AvgGPULimited, (object) this.m_AvgGPUTime);
      }
    }

    public void SetParams(
      bool enabled,
      bool adaptive,
      float minScale,
      AdaptiveDynamicResolutionScale.DynResUpscaleFilter filter,
      Camera camera)
    {
      this.isEnabled = enabled;
      this.isAdaptive = adaptive;
      this.minScale = minScale;
      this.upscaleFilter = filter;
      if (!((UnityEngine.Object) camera != (UnityEngine.Object) null))
        return;
      if (!SharedSettings.instance.graphics.isDlssActive && !SharedSettings.instance.graphics.isFsr2Active)
      {
        HDAdditionalCameraData component = camera.GetComponent<HDAdditionalCameraData>();
        component.allowDeepLearningSuperSampling = false;
        component.allowFidelityFX2SuperResolution = false;
        DynamicResolutionHandler.SetUpscaleFilter(camera, enabled ? AdaptiveDynamicResolutionScale.GetFilterFromUiEnum(filter) : DynamicResUpscaleFilter.CatmullRom);
      }
      else
        DynamicResolutionHandler.ClearSelectedCamera();
    }

    private static DynamicResUpscaleFilter GetFilterFromUiEnum(
      AdaptiveDynamicResolutionScale.DynResUpscaleFilter filter)
    {
      switch (filter)
      {
        case AdaptiveDynamicResolutionScale.DynResUpscaleFilter.CatmullRom:
          return DynamicResUpscaleFilter.CatmullRom;
        case AdaptiveDynamicResolutionScale.DynResUpscaleFilter.ContrastAdaptiveSharpen:
          return DynamicResUpscaleFilter.TAAU;
        case AdaptiveDynamicResolutionScale.DynResUpscaleFilter.EdgeAdaptiveScaling:
          return DynamicResUpscaleFilter.EdgeAdaptiveScalingUpres;
        case AdaptiveDynamicResolutionScale.DynResUpscaleFilter.TAAU:
          return DynamicResUpscaleFilter.ContrastAdaptiveSharpen;
        default:
          throw new NotSupportedException(string.Format("{0} is not a supported upscaler", (object) filter));
      }
    }

    private static bool IsGpuBottleneck(
      float fullFrameTime,
      float mainThreadCpuTime,
      float renderThreadCpuTime,
      float gpuTime)
    {
      if ((double) gpuTime == 0.0 || (double) mainThreadCpuTime == 0.0)
        return false;
      float num = fullFrameTime * 0.8f;
      return (double) gpuTime > (double) num && (double) mainThreadCpuTime < (double) num && (double) renderThreadCpuTime < (double) num;
    }

    public void UpdateDRS(
      float fullFrameTime,
      float mainThreadCpuTime,
      float renderThreadCpuTime,
      float gpuTime)
    {
      if (!FrameTimingManager.IsFeatureEnabled())
        return;
      if (!this.m_Initialized)
      {
        if (this.m_InitialFrameCounter >= 1U)
        {
          DynamicResolutionHandler.SetDynamicResScaler((PerformDynamicRes) (() => AdaptiveDynamicResolutionScale.s_CurrentScaleFraction * 100f), DynamicResScalePolicyType.ReturnsPercentage);
          this.m_Initialized = true;
        }
        else
          ++this.m_InitialFrameCounter;
      }
      if (!this.m_Initialized)
        return;
      if (!this.isEnabled)
        AdaptiveDynamicResolutionScale.s_CurrentScaleFraction = 1f;
      else if (!this.isAdaptive)
      {
        AdaptiveDynamicResolutionScale.s_CurrentScaleFraction = this.minScale;
      }
      else
      {
        this.m_AccumGPUFrameTime += gpuTime;
        this.m_GPULimitedFrames += AdaptiveDynamicResolutionScale.IsGpuBottleneck(fullFrameTime, mainThreadCpuTime, renderThreadCpuTime, gpuTime) ? 1f : 0.0f;
        ++this.m_CurrentFrameSlot;
        if (this.m_CurrentFrameSlot != this.EvaluationFrameCount)
          return;
        this.m_AvgGPUTime = this.m_AccumGPUFrameTime / (float) this.EvaluationFrameCount;
        this.m_AvgGPULimited = this.m_GPULimitedFrames / (float) this.EvaluationFrameCount;
        if (1000.0 / (double) this.DefaultTargetFrameRate - (double) this.m_AvgGPUTime < 0.0 && (double) this.m_AvgGPULimited > 0.30000001192092896)
        {
          this.m_ScaleUpCounter = 0U;
          ++this.m_ScaleDownCounter;
          if (this.m_ScaleDownCounter >= this.ScaleDownDuration)
          {
            this.m_ScaleDownCounter = 0U;
            AdaptiveDynamicResolutionScale.s_CurrentScaleFraction -= (1f - this.minScale) / (float) this.ScaleDownStepCount;
            AdaptiveDynamicResolutionScale.s_CurrentScaleFraction = math.clamp(AdaptiveDynamicResolutionScale.s_CurrentScaleFraction, this.minScale, 1f);
          }
        }
        else
        {
          this.m_ScaleDownCounter = 0U;
          ++this.m_ScaleUpCounter;
          if (this.m_ScaleUpCounter >= this.ScaleUpDuration)
          {
            this.m_ScaleUpCounter = 0U;
            AdaptiveDynamicResolutionScale.s_CurrentScaleFraction += (1f - this.minScale) / (float) this.ScaleUpStepCount;
            AdaptiveDynamicResolutionScale.s_CurrentScaleFraction = math.clamp(AdaptiveDynamicResolutionScale.s_CurrentScaleFraction, this.minScale, 1f);
          }
        }
        this.m_AccumGPUFrameTime = 0.0f;
        this.m_GPULimitedFrames = 0.0f;
        this.m_CurrentFrameSlot = 0;
      }
    }

    private static void ResetScale() => AdaptiveDynamicResolutionScale.s_CurrentScaleFraction = 1f;

    public static void Dispose()
    {
      AdaptiveDynamicResolutionScale.ResetScale();
      AdaptiveDynamicResolutionScale.s_Instance = (AdaptiveDynamicResolutionScale) null;
    }

    public enum DynResUpscaleFilter
    {
      CatmullRom,
      ContrastAdaptiveSharpen,
      EdgeAdaptiveScaling,
      TAAU,
    }
  }
}
