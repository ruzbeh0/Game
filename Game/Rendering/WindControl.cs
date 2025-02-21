// Decompiled with JetBrains decompiler
// Type: Game.Rendering.WindControl
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#nullable disable
namespace Game.Rendering
{
  public class WindControl
  {
    private static WindControl s_Instance;
    private ShaderVariablesWind m_ShaderVariablesWindCB;
    private static readonly int m_ShaderVariablesWind = Shader.PropertyToID("ShaderVariablesWind");
    private WindControl.SampledParameter<float> _WindBaseStrengthPhase;
    private WindControl.SampledParameter<float> _WindBaseStrengthPhase2;
    private WindControl.SampledParameter<float> _WindTreeBaseStrengthPhase;
    private WindControl.SampledParameter<float> _WindTreeBaseStrengthPhase2;
    private WindControl.SampledParameter<float> _WindBaseStrengthVariancePeriod;
    private WindControl.SampledParameter<float> _WindTreeBaseStrengthVariancePeriod;
    private WindControl.SampledParameter<float> _WindGustStrengthPhase;
    private WindControl.SampledParameter<float> _WindGustStrengthPhase2;
    private WindControl.SampledParameter<float> _WindTreeGustStrengthPhase;
    private WindControl.SampledParameter<float> _WindTreeGustStrengthPhase2;
    private WindControl.SampledParameter<float> _WindGustStrengthVariancePeriod;
    private WindControl.SampledParameter<float> _WindTreeGustStrengthVariancePeriod;
    private WindControl.SampledParameter<float> _WindFlutterGustVariancePeriod;
    private WindControl.SampledParameter<float> _WindTreeFlutterGustVariancePeriod;
    private float _LastParametersSamplingTime;
    private static readonly float3 kForward = new float3(0.0f, 0.0f, 1f);

    public static WindControl instance
    {
      get
      {
        if (WindControl.s_Instance == null)
          WindControl.s_Instance = new WindControl();
        return WindControl.s_Instance;
      }
    }

    private WindControl()
    {
      RenderPipelineManager.beginCameraRendering += new Action<ScriptableRenderContext, Camera>(this.SetupGPUData);
    }

    public void Dispose()
    {
      RenderPipelineManager.beginCameraRendering -= new Action<ScriptableRenderContext, Camera>(this.SetupGPUData);
      WindControl.s_Instance = (WindControl) null;
    }

    private bool GetWindComponent(Camera camera, out WindVolumeComponent component)
    {
      if (camera.cameraType == CameraType.SceneView)
      {
        Camera main = Camera.main;
        if ((UnityEngine.Object) main == (UnityEngine.Object) null)
        {
          component = (WindVolumeComponent) null;
          return false;
        }
        camera = main;
      }
      HDCamera hdCamera = HDCamera.GetOrCreate(camera);
      component = hdCamera.volumeStack.GetComponent<WindVolumeComponent>();
      return true;
    }

    private void SetupGPUData(ScriptableRenderContext context, Camera camera)
    {
      WindVolumeComponent component;
      if (!this.GetWindComponent(camera, out component))
        return;
      CommandBuffer commandBuffer = CommandBufferPool.Get("");
      this.UpdateCPUData(component);
      this.SetGlobalProperties(commandBuffer, component);
      context.ExecuteCommandBuffer(commandBuffer);
      context.Submit();
      commandBuffer.Clear();
      CommandBufferPool.Release(commandBuffer);
    }

    private void UpdateCPUData(WindVolumeComponent wind)
    {
      if ((double) Time.time - (double) this._LastParametersSamplingTime <= (double) wind.windParameterInterpolationDuration.value)
        return;
      this._LastParametersSamplingTime = Time.time;
      this._WindBaseStrengthPhase.Update(wind.windBaseStrengthPhase.value);
      this._WindBaseStrengthPhase2.Update(wind.windBaseStrengthPhase2.value);
      this._WindTreeBaseStrengthPhase.Update(wind.windTreeBaseStrengthPhase.value);
      this._WindTreeBaseStrengthPhase2.Update(wind.windTreeBaseStrengthPhase2.value);
      this._WindBaseStrengthVariancePeriod.Update(wind.windBaseStrengthVariancePeriod.value);
      this._WindTreeBaseStrengthVariancePeriod.Update(wind.windTreeBaseStrengthVariancePeriod.value);
      this._WindGustStrengthPhase.Update(wind.windGustStrengthPhase.value);
      this._WindGustStrengthPhase2.Update(wind.windGustStrengthPhase2.value);
      this._WindTreeGustStrengthPhase.Update(wind.windTreeGustStrengthPhase.value);
      this._WindTreeGustStrengthPhase2.Update(wind.windTreeGustStrengthPhase2.value);
      this._WindGustStrengthVariancePeriod.Update(wind.windGustStrengthVariancePeriod.value);
      this._WindTreeGustStrengthVariancePeriod.Update(wind.windTreeGustStrengthVariancePeriod.value);
      this._WindFlutterGustVariancePeriod.Update(wind.windFlutterGustVariancePeriod.value);
      this._WindTreeFlutterGustVariancePeriod.Update(wind.windTreeFlutterGustVariancePeriod.value);
    }

    private void SetGlobalProperties(CommandBuffer cmd, WindVolumeComponent wind)
    {
      using (new ProfilingScope(cmd, ProfilingSampler.Get<ProfileId>(ProfileId.WindGlobalProperties)))
      {
        float num1 = Application.isPlaying ? Time.time : Time.realtimeSinceStartup;
        float3 xyz1 = math.mul(quaternion.Euler(0.0f, math.radians(wind.windDirection.value) + math.cos(6.28318548f * num1 / wind.windDirectionVariancePeriod.value) * math.radians(wind.windDirectionVariance.value), 0.0f), WindControl.kForward);
        float3 xyz2 = math.mul(quaternion.Euler(0.0f, wind.windDirection.value, 0.0f), WindControl.kForward);
        float num2 = wind.windGustStrengthControl.value.Evaluate(num1);
        float num3 = wind.windTreeGustStrengthControl.value.Evaluate(num1);
        this.m_ShaderVariablesWindCB._WindData_0 = (Matrix4x4) math.transpose(new float4x4(new float4(xyz1, 1f), new float4(xyz2, num1), float4.zero with
        {
          w = math.min(1f, (Time.time - this._LastParametersSamplingTime) / wind.windParameterInterpolationDuration.value)
        }, new float4(this._WindBaseStrengthPhase.previous, this._WindBaseStrengthPhase2.previous, this._WindBaseStrengthPhase.current, this._WindBaseStrengthPhase2.current)));
        this.m_ShaderVariablesWindCB._WindData_1 = (Matrix4x4) math.transpose(new float4x4(new float4(wind.windBaseStrength.value * wind.windGlobalStrengthScale.value * wind.windGlobalStrengthScale2.value, wind.windBaseStrengthOffset.value, wind.windTreeBaseStrength.value * wind.windGlobalStrengthScale.value * wind.windGlobalStrengthScale2.value, wind.windTreeBaseStrengthOffset.value), new float4(0.0f, wind.windGustStrength.value * num2 * wind.windGlobalStrengthScale.value * wind.windGlobalStrengthScale2.value, wind.windGustStrengthOffset.value, this._WindFlutterGustVariancePeriod.current), new float4(this._WindGustStrengthVariancePeriod.current, this._WindGustStrengthVariancePeriod.previous, wind.windGustInnerCosScale.value, wind.windFlutterStrength.value * wind.windGlobalStrengthScale.value * wind.windGlobalStrengthScale2.value), new float4(wind.windFlutterGustStrength.value * wind.windGlobalStrengthScale.value * wind.windGlobalStrengthScale2.value, wind.windFlutterGustStrengthOffset.value, wind.windFlutterGustStrengthScale.value, this._WindFlutterGustVariancePeriod.previous)));
        this.m_ShaderVariablesWindCB._WindData_2 = (Matrix4x4) math.transpose(new float4x4(new float4(this._WindTreeBaseStrengthPhase.previous, this._WindTreeBaseStrengthPhase2.previous, this._WindTreeBaseStrengthPhase.current, this._WindTreeBaseStrengthPhase2.current), new float4(0.0f, wind.windTreeGustStrength.value * num3 * wind.windGlobalStrengthScale.value * wind.windGlobalStrengthScale2.value, wind.windTreeGustStrengthOffset.value, this._WindTreeFlutterGustVariancePeriod.current), new float4(this._WindTreeGustStrengthVariancePeriod.current, this._WindTreeGustStrengthVariancePeriod.previous, wind.windTreeGustInnerCosScale.value, wind.windTreeFlutterStrength.value * wind.windGlobalStrengthScale.value * wind.windGlobalStrengthScale2.value), new float4(wind.windTreeFlutterGustStrength.value * wind.windGlobalStrengthScale.value * wind.windGlobalStrengthScale2.value, wind.windTreeFlutterGustStrengthOffset.value, wind.windTreeFlutterGustStrengthScale.value, this._WindTreeFlutterGustVariancePeriod.previous)));
        this.m_ShaderVariablesWindCB._WindData_3 = (Matrix4x4) math.transpose(new float4x4(new float4(this._WindBaseStrengthVariancePeriod.previous, this._WindTreeBaseStrengthVariancePeriod.previous, this._WindBaseStrengthVariancePeriod.previous, this._WindTreeBaseStrengthVariancePeriod.current), new float4(this._WindGustStrengthPhase.previous, this._WindGustStrengthPhase2.previous, this._WindGustStrengthPhase.current, this._WindGustStrengthPhase2.current), new float4(this._WindTreeGustStrengthPhase.previous, this._WindTreeGustStrengthPhase2.previous, this._WindTreeGustStrengthPhase.current, this._WindTreeGustStrengthPhase2.current), new float4(0.0f, 0.0f, 0.0f, 0.0f)));
        ConstantBuffer.PushGlobal<ShaderVariablesWind>(cmd, in this.m_ShaderVariablesWindCB, WindControl.m_ShaderVariablesWind);
      }
    }

    private struct SampledParameter<T>
    {
      public T current;
      public T previous;

      public void Reset(T value)
      {
        this.previous = value;
        this.current = value;
      }

      public void Update(T value)
      {
        this.previous = this.current;
        this.current = value;
      }
    }
  }
}
