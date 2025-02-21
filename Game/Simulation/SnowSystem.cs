// Decompiled with JetBrains decompiler
// Type: Game.Simulation.SnowSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.AssetPipeline.Native;
using Colossal.Logging;
using Colossal.Serialization.Entities;
using Game.Prefabs;
using Game.Rendering;
using Game.Serialization;
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Simulation
{
  [FormerlySerializedAs("Colossal.Terrain.SnowSystem, Game")]
  public class SnowSystem : GameSystemBase, IDefaultSerializable, ISerializable, IPostDeserialize
  {
    private static ILog log = LogManager.GetLogger("SceneFlow");
    private const int kTexSize = 1024;
    private const int kGroupSizeAddSnow = 16;
    private const int kNumGroupAddSnow = 64;
    private const float kTimeStep = 0.2f;
    private const float kSnowHeightScale = 8f;
    private const float kSnowMeltScale = 1f;
    private const float m_SnowAddConstant = 1E-05f;
    private const float m_WaterAddConstant = 0.1f;
    private const int kSnowHeightBackdropTextureSize = 1024;
    private const float kSnowBackdropUpdateLerpFactor = 0.1f;
    private RenderTexture m_snowHeightBackdropTextureFinal;
    private ComputeBuffer m_snowBackdropBuffer;
    private ComputeBuffer m_MinHeights;
    private RenderTexture[] m_SnowHeights;
    private CommandBuffer m_CommandBuffer;
    private ComputeShader m_SnowUpdateShader;
    private int m_TransferKernel;
    private int m_AddKernel;
    private int m_ResetKernel;
    private int m_LoadKernel;
    private int m_LoadOldFormatKernel;
    private int m_UpdateBackdropSnowHeightTextureKernel;
    private int m_ClearBackdropSnowHeightTextureKernel;
    private int m_FinalizeBackdropSnowHeightTextureKernel;
    private int m_ID_SnowDepth;
    private int m_ID_OldSnowDepth;
    private int m_ID_Timestep;
    private int m_ID_AddMultiplier;
    private int m_ID_MeltMultiplier;
    private int m_ID_AddWaterMultiplier;
    private int m_ID_ElapseWaterMultiplier;
    private int m_ID_Temperature;
    private int m_ID_Rain;
    private int m_ID_Wind;
    private int m_ID_Time;
    private int m_ID_SnowScale;
    private int m_ID_MinHeights;
    private int m_ID_SnowHeightBackdropBuffer;
    private int m_ID_SnowHeightBackdropFinal;
    private int m_ID_SnowBackdropUpdateLerpFactor;
    private int m_ID_SnowHeightBackdropBufferSize;
    private TerrainSystem m_TerrainSystem;
    private SimulationSystem m_SimulationSystem;
    private TimeSystem m_TimeSystem;
    private ClimateSystem m_ClimateSystem;
    private WindSimulationSystem m_WindSimulationSystem;
    private WaterSystem m_WaterSystem;

    public RenderTexture SnowHeightBackdropTexture => this.m_snowHeightBackdropTextureFinal;

    public int SnowSimSpeed { get; set; }

    public int2 TextureSize => new int2(1024, 1024);

    public bool Loaded => (UnityEngine.Object) this.m_SnowUpdateShader != (UnityEngine.Object) null;

    public ComputeShader m_SnowTransferShader { private get; set; }

    public ComputeShader m_DynamicHeightShader { private get; set; }

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 4;

    private float4 SnowScaleVector => new float4(8f, 1f, 1f, 1f);

    private void InitShader()
    {
      this.m_SnowUpdateShader = Colossal.IO.AssetDatabase.AssetDatabase.global.resources.shaders.snowUpdate;
      this.m_ResetKernel = this.m_SnowUpdateShader.FindKernel("Reset");
      this.m_LoadKernel = this.m_SnowUpdateShader.FindKernel("Load");
      this.m_LoadOldFormatKernel = this.m_SnowUpdateShader.FindKernel("LoadOldFormat");
      this.m_AddKernel = this.m_SnowUpdateShader.FindKernel("Add");
      this.m_TransferKernel = this.m_SnowUpdateShader.FindKernel("Transfer");
      this.m_UpdateBackdropSnowHeightTextureKernel = this.m_SnowUpdateShader.FindKernel("UpdateBackdropSnowHeightTexture");
      this.m_ClearBackdropSnowHeightTextureKernel = this.m_SnowUpdateShader.FindKernel("ClearBackdropSnowHeightTexture");
      this.m_FinalizeBackdropSnowHeightTextureKernel = this.m_SnowUpdateShader.FindKernel("FinalizeBackdropSnowHeightTexture");
    }

    private int Write { get; set; }

    private int Read => 1 - this.Write;

    public RenderTexture SnowDepth
    {
      get => this.m_SnowHeights != null ? this.m_SnowHeights[this.Read] : (RenderTexture) null;
    }

    public bool IsAsync { get; set; }

    public unsafe void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(1024);
      writer.Write(1024);
      int dataStride = UnsafeUtility.SizeOf(typeof (SnowSystem.ushort2));
      NativeArray<byte> output = new NativeArray<byte>(1048576 * dataStride, Allocator.Persistent);
      AsyncGPUReadbackRequest gpuReadbackRequest = AsyncGPUReadback.RequestIntoNativeArray<byte>(ref output, (Texture) this.m_SnowHeights[this.Read]);
      gpuReadbackRequest.WaitForCompletion();
      if (!gpuReadbackRequest.done)
        SnowSystem.log.Warn((object) "Snow request not done after WaitForCompletion");
      if (gpuReadbackRequest.hasError)
        SnowSystem.log.Warn((object) "Snow request has error after WaitForCompletion");
      NativeArray<byte> nativeArray = new NativeArray<byte>(output.Length, Allocator.Temp);
      NativeCompression.FilterDataBeforeWrite((IntPtr) output.GetUnsafeReadOnlyPtr<byte>(), (IntPtr) nativeArray.GetUnsafePtr<byte>(), (long) nativeArray.Length, dataStride);
      output.Dispose();
      writer.Write(nativeArray);
      nativeArray.Dispose();
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      if (context.version < Game.Version.snow)
      {
        this.m_SnowUpdateShader.SetTexture(this.m_ResetKernel, "_Result", (Texture) this.m_SnowHeights[this.Write]);
        this.m_SnowUpdateShader.Dispatch(this.m_ResetKernel, 64, 64, 1);
        this.m_SnowUpdateShader.SetTexture(this.m_ResetKernel, "_Result", (Texture) this.m_SnowHeights[this.Read]);
        this.m_SnowUpdateShader.Dispatch(this.m_ResetKernel, 64, 64, 1);
      }
      Shader.SetGlobalTexture("_SnowMap", (Texture) this.SnowDepth);
    }

    public void DebugReset()
    {
      this.m_SnowUpdateShader.SetTexture(this.m_ResetKernel, "_Result", (Texture) this.m_SnowHeights[this.Write]);
      this.m_SnowUpdateShader.Dispatch(this.m_ResetKernel, 64, 64, 1);
      this.m_SnowUpdateShader.SetTexture(this.m_ResetKernel, "_Result", (Texture) this.m_SnowHeights[this.Read]);
      this.m_SnowUpdateShader.Dispatch(this.m_ResetKernel, 64, 64, 1);
    }

    public unsafe void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      int num1;
      reader.Read(out num1);
      int num2;
      reader.Read(out num2);
      bool flag = true;
      if (num1 != 1024)
      {
        Debug.LogWarning((object) ("Saved snow width = " + num1.ToString() + ", snow tex width = " + 1024.ToString()));
        flag = false;
      }
      if (num2 != 1024)
      {
        Debug.LogWarning((object) ("Saved snow height = " + num2.ToString() + ", snow tex height = " + 1024.ToString()));
        flag = false;
      }
      int num3 = num1 * num2;
      if (reader.context.version >= Game.Version.snow16bits)
      {
        int dataStride = UnsafeUtility.SizeOf(typeof (SnowSystem.ushort2));
        NativeArray<SnowSystem.ushort2> nativeArray1 = new NativeArray<SnowSystem.ushort2>(num3, Allocator.Temp);
        NativeArray<byte> nativeArray2 = new NativeArray<byte>(num3 * dataStride, Allocator.Temp);
        reader.Read(nativeArray2);
        NativeCompression.UnfilterDataAfterRead((IntPtr) nativeArray2.GetUnsafePtr<byte>(), (IntPtr) nativeArray1.GetUnsafePtr<SnowSystem.ushort2>(), (long) nativeArray2.Length, dataStride);
        nativeArray2.Dispose();
        if (flag)
        {
          NativeArray<float2> data = new NativeArray<float2>(num3, Allocator.Temp);
          int num4 = 0;
          foreach (SnowSystem.ushort2 ushort2 in nativeArray1)
            data[num4++] = new float2((float) ushort2.x / (float) ushort.MaxValue, (float) ushort2.y / (float) ushort.MaxValue);
          ComputeBuffer buffer = new ComputeBuffer(num3, UnsafeUtility.SizeOf<float2>(), ComputeBufferType.Default);
          buffer.SetData<float2>(data);
          this.m_CommandBuffer.SetComputeBufferParam(this.m_SnowUpdateShader, this.m_LoadKernel, "_LoadSource", buffer);
          this.m_CommandBuffer.SetComputeTextureParam(this.m_SnowUpdateShader, this.m_LoadKernel, "_Result", (RenderTargetIdentifier) (Texture) this.m_SnowHeights[this.Write]);
          this.m_CommandBuffer.DispatchCompute(this.m_SnowUpdateShader, this.m_LoadKernel, 64, 64, 1);
          this.m_CommandBuffer.SetComputeTextureParam(this.m_SnowUpdateShader, this.m_LoadKernel, "_Result", (RenderTargetIdentifier) (Texture) this.m_SnowHeights[this.Read]);
          this.m_CommandBuffer.DispatchCompute(this.m_SnowUpdateShader, this.m_LoadKernel, 64, 64, 1);
          this.m_CommandBuffer.SetComputeBufferParam(this.m_SnowUpdateShader, this.m_ClearBackdropSnowHeightTextureKernel, this.m_ID_SnowHeightBackdropBuffer, this.m_snowBackdropBuffer);
          this.m_CommandBuffer.DispatchCompute(this.m_SnowUpdateShader, this.m_ClearBackdropSnowHeightTextureKernel, 64, 1, 1);
          this.AddSnow(this.m_CommandBuffer);
          this.SnowTransfer(this.m_CommandBuffer);
          this.UpdateSnowBackdropTexture(this.m_CommandBuffer, 1f);
          Graphics.ExecuteCommandBuffer(this.m_CommandBuffer);
          buffer.Dispose();
          data.Dispose();
        }
        nativeArray1.Dispose();
      }
      else
      {
        NativeArray<float4> nativeArray3 = new NativeArray<float4>(num3, Allocator.Temp);
        if (reader.context.version >= Game.Version.terrainWaterSnowCompression)
        {
          NativeArray<byte> nativeArray4 = new NativeArray<byte>(num3 * 16, Allocator.Temp);
          reader.Read(nativeArray4);
          NativeCompression.UnfilterDataAfterRead((IntPtr) nativeArray4.GetUnsafePtr<byte>(), (IntPtr) nativeArray3.GetUnsafePtr<float4>(), (long) nativeArray4.Length, 16);
          nativeArray4.Dispose();
        }
        else
          reader.Read(nativeArray3);
        if (flag)
        {
          ComputeBuffer buffer = new ComputeBuffer(num3, UnsafeUtility.SizeOf<float4>(), ComputeBufferType.Default);
          buffer.SetData<float4>(nativeArray3);
          this.m_SnowUpdateShader.SetVector("_LoadScale", (Vector4) this.SnowScaleVector);
          this.m_SnowUpdateShader.SetBuffer(this.m_LoadOldFormatKernel, "_LoadSourceOldFormat", buffer);
          this.m_SnowUpdateShader.SetTexture(this.m_LoadOldFormatKernel, "_Result", (Texture) this.m_SnowHeights[this.Write]);
          this.m_SnowUpdateShader.Dispatch(this.m_LoadOldFormatKernel, 64, 64, 1);
          this.m_SnowUpdateShader.SetTexture(this.m_LoadOldFormatKernel, "_Result", (Texture) this.m_SnowHeights[this.Read]);
          this.m_SnowUpdateShader.Dispatch(this.m_LoadOldFormatKernel, 64, 64, 1);
          buffer.Dispose();
        }
        nativeArray3.Dispose();
      }
      Shader.SetGlobalVector("colossal_SnowScale", (Vector4) this.SnowScaleVector);
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      this.m_SnowUpdateShader.SetTexture(this.m_ResetKernel, "_Result", (Texture) this.m_SnowHeights[this.Write]);
      this.m_SnowUpdateShader.Dispatch(this.m_ResetKernel, 64, 64, 1);
      this.m_SnowUpdateShader.SetTexture(this.m_ResetKernel, "_Result", (Texture) this.m_SnowHeights[this.Read]);
      this.m_SnowUpdateShader.Dispatch(this.m_ResetKernel, 64, 64, 1);
      Shader.SetGlobalTexture("_SnowMap", (Texture) this.SnowDepth);
    }

    public void UpdateDynamicHeights()
    {
    }

    private RenderTexture CreateTexture(string name)
    {
      RenderTexture texture = new RenderTexture(1024, 1024, 0, GraphicsFormat.R16G16_UNorm);
      texture.name = name;
      texture.hideFlags = HideFlags.DontSave;
      texture.enableRandomWrite = true;
      texture.wrapMode = TextureWrapMode.Clamp;
      texture.filterMode = FilterMode.Bilinear;
      texture.Create();
      return texture;
    }

    private void InitTextures()
    {
      this.m_SnowHeights = new RenderTexture[2];
      this.m_SnowHeights[0] = this.CreateTexture("SnowRT0");
      this.m_SnowHeights[1] = this.CreateTexture("SnowRT1");
      this.m_MinHeights = new ComputeBuffer(4096, UnsafeUtility.SizeOf<float2>(), ComputeBufferType.Default);
      this.m_snowBackdropBuffer = new ComputeBuffer(1024, UnsafeUtility.SizeOf<uint2>(), ComputeBufferType.Default);
      RenderTexture renderTexture = new RenderTexture(1024, 1, 0, GraphicsFormat.R32_SFloat);
      renderTexture.name = "SnowBackdropHeightTextureFinal";
      renderTexture.hideFlags = HideFlags.DontSave;
      renderTexture.enableRandomWrite = true;
      renderTexture.wrapMode = TextureWrapMode.Clamp;
      renderTexture.filterMode = FilterMode.Bilinear;
      this.m_snowHeightBackdropTextureFinal = renderTexture;
      this.m_snowHeightBackdropTextureFinal.Create();
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.InitShader();
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      this.m_WindSimulationSystem = this.World.GetOrCreateSystemManaged<WindSimulationSystem>();
      this.InitTextures();
      this.m_ID_SnowDepth = Shader.PropertyToID("_Result");
      this.m_ID_OldSnowDepth = Shader.PropertyToID("_Previous");
      this.m_ID_Timestep = Shader.PropertyToID("_Timestep");
      this.m_ID_AddMultiplier = Shader.PropertyToID("_AddMultiplier");
      this.m_ID_MeltMultiplier = Shader.PropertyToID("_MeltMultiplier");
      this.m_ID_AddWaterMultiplier = Shader.PropertyToID("_AddWaterMultiplier");
      this.m_ID_ElapseWaterMultiplier = Shader.PropertyToID("_ElapseWaterMultiplier");
      this.m_ID_Temperature = Shader.PropertyToID("_Temperature");
      this.m_ID_Rain = Shader.PropertyToID("_Rain");
      this.m_ID_Time = Shader.PropertyToID("_SimTime");
      this.m_ID_Wind = Shader.PropertyToID("_Wind");
      this.m_ID_SnowScale = Shader.PropertyToID("_SnowScale");
      this.m_ID_MinHeights = Shader.PropertyToID("_MinHeights");
      this.m_ID_SnowHeightBackdropBuffer = Shader.PropertyToID("_SnowHeightBackdropBuffer");
      this.m_ID_SnowHeightBackdropFinal = Shader.PropertyToID("_SnowHeightBackdropTextureFinal");
      this.m_ID_SnowBackdropUpdateLerpFactor = Shader.PropertyToID("_SnowBackdropUpdateLerpFactor");
      this.m_ID_SnowHeightBackdropBufferSize = Shader.PropertyToID("_SnowHeightBackdropBufferSize");
      this.RequireForUpdate<TerrainPropertiesData>();
      this.m_CommandBuffer = new CommandBuffer();
      this.m_CommandBuffer.name = "Snowsystem";
      this.SnowSimSpeed = 1;
    }

    [Preserve]
    protected override void OnDestroy()
    {
      this.m_CommandBuffer.Dispose();
      CoreUtils.Destroy((UnityEngine.Object) this.m_SnowHeights[0]);
      CoreUtils.Destroy((UnityEngine.Object) this.m_SnowHeights[1]);
      this.m_MinHeights.Release();
      this.m_snowBackdropBuffer.Release();
      CoreUtils.Destroy((UnityEngine.Object) this.m_snowHeightBackdropTextureFinal);
    }

    private void FlipSnow() => this.Write = 1 - this.Write;

    private float GetSnowiness() => Mathf.Sin(125.663712f * this.m_TimeSystem.normalizedDate);

    private void AddSnow(CommandBuffer cmd)
    {
      using (new ProfilingScope(cmd, ProfilingSampler.Get<ProfileId>(ProfileId.AddSnow)))
      {
        cmd.SetComputeFloatParam(this.m_SnowUpdateShader, this.m_ID_Timestep, 0.2f);
        cmd.SetComputeFloatParam(this.m_SnowUpdateShader, this.m_ID_AddMultiplier, 1E-05f);
        cmd.SetComputeFloatParam(this.m_SnowUpdateShader, this.m_ID_MeltMultiplier, 0.00012f);
        cmd.SetComputeFloatParam(this.m_SnowUpdateShader, this.m_ID_AddWaterMultiplier, 0.1f);
        cmd.SetComputeFloatParam(this.m_SnowUpdateShader, this.m_ID_ElapseWaterMultiplier, 0.05f);
        cmd.SetComputeFloatParam(this.m_SnowUpdateShader, this.m_ID_Temperature, (float) this.m_ClimateSystem.temperature);
        cmd.SetComputeFloatParam(this.m_SnowUpdateShader, this.m_ID_Rain, (float) this.m_ClimateSystem.precipitation);
        cmd.SetComputeVectorParam(this.m_SnowUpdateShader, this.m_ID_Wind, (Vector4) new float4(this.m_WindSimulationSystem.constantWind, 0.0f, 0.0f));
        cmd.SetComputeVectorParam(this.m_SnowUpdateShader, this.m_ID_SnowScale, (Vector4) this.SnowScaleVector);
        cmd.SetComputeFloatParam(this.m_SnowUpdateShader, this.m_ID_Time, this.m_TimeSystem.normalizedTime);
        cmd.SetComputeTextureParam(this.m_SnowUpdateShader, this.m_AddKernel, "_Terrain", (RenderTargetIdentifier) this.m_TerrainSystem.heightmap);
        cmd.SetComputeVectorParam(this.m_SnowUpdateShader, "_HeightScale", (Vector4) new float4(this.m_TerrainSystem.heightScaleOffset, this.m_ClimateSystem.temperatureBaseHeight, this.m_ClimateSystem.snowTemperatureHeightScale));
        cmd.SetComputeTextureParam(this.m_SnowUpdateShader, this.m_AddKernel, this.m_ID_OldSnowDepth, (RenderTargetIdentifier) (Texture) this.m_SnowHeights[this.Read]);
        cmd.SetComputeTextureParam(this.m_SnowUpdateShader, this.m_AddKernel, this.m_ID_SnowDepth, (RenderTargetIdentifier) (Texture) this.m_SnowHeights[this.Write]);
        cmd.SetComputeTextureParam(this.m_SnowUpdateShader, this.m_AddKernel, "_Water", (RenderTargetIdentifier) (Texture) this.m_WaterSystem.WaterTexture);
        cmd.SetComputeBufferParam(this.m_SnowUpdateShader, this.m_AddKernel, this.m_ID_MinHeights, this.m_MinHeights);
        cmd.DispatchCompute(this.m_SnowUpdateShader, this.m_AddKernel, 64, 64, 1);
      }
      this.FlipSnow();
    }

    private void SnowTransfer(CommandBuffer cmd)
    {
      using (new ProfilingScope(cmd, ProfilingSampler.Get<ProfileId>(ProfileId.TransferSnow)))
      {
        if ((double) (float) this.m_ClimateSystem.precipitation < 0.10000000149011612 || (double) (float) this.m_ClimateSystem.temperature - 0.0099999997764825821 * ((double) this.m_TerrainSystem.heightScaleOffset.x - (double) this.m_ClimateSystem.temperatureBaseHeight) > 0.0)
          return;
        cmd.SetComputeVectorParam(this.m_SnowUpdateShader, this.m_ID_SnowScale, (Vector4) this.SnowScaleVector);
        cmd.SetComputeVectorParam(this.m_SnowUpdateShader, "_HeightScale", (Vector4) new float4(this.m_TerrainSystem.heightScaleOffset, this.m_ClimateSystem.temperatureBaseHeight, 0.0f));
        cmd.SetComputeTextureParam(this.m_SnowUpdateShader, this.m_TransferKernel, this.m_ID_OldSnowDepth, (RenderTargetIdentifier) (Texture) this.m_SnowHeights[this.Read]);
        cmd.SetComputeTextureParam(this.m_SnowUpdateShader, this.m_TransferKernel, this.m_ID_SnowDepth, (RenderTargetIdentifier) (Texture) this.m_SnowHeights[this.Write]);
        cmd.SetComputeTextureParam(this.m_SnowUpdateShader, this.m_TransferKernel, "_Terrain", (RenderTargetIdentifier) this.m_TerrainSystem.heightmap);
        cmd.SetComputeVectorParam(this.m_SnowUpdateShader, this.m_ID_Wind, (Vector4) new float4(this.m_WindSimulationSystem.constantWind, 0.0f, 0.0f));
        cmd.DispatchCompute(this.m_SnowUpdateShader, this.m_TransferKernel, 64, 64, 1);
      }
      this.FlipSnow();
    }

    private void UpdateSnowBackdropTexture(CommandBuffer cmd, float lerpFactor)
    {
      using (new ProfilingScope(this.m_CommandBuffer, ProfilingSampler.Get<ProfileId>(ProfileId.UpdateSnowHeightBackdrop)))
      {
        cmd.SetComputeBufferParam(this.m_SnowUpdateShader, this.m_ClearBackdropSnowHeightTextureKernel, this.m_ID_SnowHeightBackdropBuffer, this.m_snowBackdropBuffer);
        cmd.DispatchCompute(this.m_SnowUpdateShader, this.m_ClearBackdropSnowHeightTextureKernel, 64, 1, 1);
        cmd.SetComputeBufferParam(this.m_SnowUpdateShader, this.m_UpdateBackdropSnowHeightTextureKernel, this.m_ID_SnowHeightBackdropBuffer, this.m_snowBackdropBuffer);
        cmd.SetComputeBufferParam(this.m_SnowUpdateShader, this.m_UpdateBackdropSnowHeightTextureKernel, this.m_ID_MinHeights, this.m_MinHeights);
        cmd.SetComputeIntParam(this.m_SnowUpdateShader, this.m_ID_SnowHeightBackdropBufferSize, 1024);
        cmd.DispatchCompute(this.m_SnowUpdateShader, this.m_UpdateBackdropSnowHeightTextureKernel, 256, 1, 1);
        cmd.SetComputeBufferParam(this.m_SnowUpdateShader, this.m_FinalizeBackdropSnowHeightTextureKernel, this.m_ID_SnowHeightBackdropBuffer, this.m_snowBackdropBuffer);
        cmd.SetComputeTextureParam(this.m_SnowUpdateShader, this.m_FinalizeBackdropSnowHeightTextureKernel, this.m_ID_SnowHeightBackdropFinal, (RenderTargetIdentifier) (Texture) this.m_snowHeightBackdropTextureFinal);
        cmd.SetComputeFloatParam(this.m_SnowUpdateShader, this.m_ID_SnowBackdropUpdateLerpFactor, lerpFactor);
        cmd.DispatchCompute(this.m_SnowUpdateShader, this.m_FinalizeBackdropSnowHeightTextureKernel, 1, 1, 1);
      }
    }

    [Preserve]
    protected override void OnUpdate()
    {
      if (!this.m_WaterSystem.Loaded)
        return;
      this.m_CommandBuffer.Clear();
      for (int index = 0; index < this.SnowSimSpeed; ++index)
      {
        this.AddSnow(this.m_CommandBuffer);
        this.SnowTransfer(this.m_CommandBuffer);
      }
      this.UpdateSnowBackdropTexture(this.m_CommandBuffer, 0.1f);
      Shader.SetGlobalTexture("_SnowMap", (Texture) this.SnowDepth);
      Graphics.ExecuteCommandBuffer(this.m_CommandBuffer);
    }

    [Preserve]
    public SnowSystem()
    {
    }

    public struct ushort2
    {
      public ushort x;
      public ushort y;
    }
  }
}
