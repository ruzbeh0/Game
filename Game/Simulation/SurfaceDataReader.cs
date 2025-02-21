// Decompiled with JetBrains decompiler
// Type: Game.Simulation.SurfaceDataReader
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Simulation
{
  [BurstCompile]
  internal class SurfaceDataReader
  {
    private int m_ReadbackDistribution = 8;
    private int m_ReadbackIndex;
    private NativeArray<float4> m_CPUTemp;
    private NativeArray<SurfaceWater> m_CPU;
    private JobHandle m_Writers;
    private JobHandle m_Readers;
    private int2 m_TexSize;
    private int m_mapSize;
    private AsyncGPUReadbackRequest m_AsyncReadback;
    private bool m_PendingReadback;
    private RenderTexture m_sourceTexture;

    public JobHandle JobWriters => this.m_Writers;

    public JobHandle JobReaders
    {
      get => this.m_Readers;
      set => this.m_Readers = value;
    }

    public SurfaceDataReader(RenderTexture sourceTexture, int mapSize)
    {
      this.m_sourceTexture = sourceTexture;
      this.m_TexSize = new int2(sourceTexture.width, sourceTexture.height);
      this.m_mapSize = mapSize;
      this.m_CPU = new NativeArray<SurfaceWater>(this.m_TexSize.x * this.m_TexSize.y, Allocator.Persistent);
      int2 size;
      this.GetReadbackBounds(out int2 _, out size);
      this.m_CPUTemp = new NativeArray<float4>(size.x * size.y, Allocator.Persistent);
    }

    public void LoadData(NativeArray<float4> buffer)
    {
      for (int index = 0; index < this.m_CPU.Length; ++index)
      {
        float4 float4 = buffer[index];
        this.m_CPU[index] = new SurfaceWater()
        {
          m_Depth = math.max(float4.x, 0.0f),
          m_Polluted = float4.w,
          m_Velocity = float4.yz
        };
      }
    }

    public void ExecuteReadBack()
    {
      if (this.m_PendingReadback)
        return;
      this.m_Writers.Complete();
      this.m_ReadbackIndex = (this.m_ReadbackIndex + 1) % (this.m_ReadbackDistribution * this.m_ReadbackDistribution);
      int2 pos;
      int2 size;
      this.GetReadbackBounds(out pos, out size);
      this.m_AsyncReadback = AsyncGPUReadback.RequestIntoNativeArray<float4>(ref this.m_CPUTemp, (Texture) this.m_sourceTexture, 0, pos.x, size.x, pos.y, size.y, 0, 1, GraphicsFormat.R32G32B32A32_SFloat, new Action<AsyncGPUReadbackRequest>(this.CopyWaterValues));
      this.m_PendingReadback = true;
    }

    private void CopyWaterValues(AsyncGPUReadbackRequest request)
    {
      SurfaceDataReader.CopyWaterValues(ref this.m_AsyncReadback, ref this.m_CPU, ref this.m_CPUTemp, ref this.m_Readers, ref this.m_TexSize, ref this.m_PendingReadback, this.m_ReadbackDistribution, this.m_ReadbackIndex);
    }

    [BurstCompile]
    private static void CopyWaterValues(
      ref AsyncGPUReadbackRequest asyncReadback,
      ref NativeArray<SurfaceWater> cpu,
      ref NativeArray<float4> cpuTemp,
      ref JobHandle readers,
      ref int2 texSize,
      ref bool pendingReadback,
      int readbackDistribution,
      int readbackIndex)
    {
      SurfaceDataReader.CopyWaterValues_00005969\u0024BurstDirectCall.Invoke(ref asyncReadback, ref cpu, ref cpuTemp, ref readers, ref texSize, ref pendingReadback, readbackDistribution, readbackIndex);
    }

    public WaterSurfaceData GetSurfaceData(out JobHandle deps)
    {
      deps = this.m_Writers;
      int3 _resolution = this.m_CPU.Length == this.m_TexSize.x * this.m_TexSize.y ? new int3(this.m_TexSize.x, 2, this.m_TexSize.y) : new int3(2, 2, 2);
      float3 float3 = new float3((float) this.m_mapSize, 1f, (float) this.m_mapSize);
      float3 _scale = new float3((float) _resolution.x, (float) (_resolution.y - 1), (float) _resolution.z) / float3;
      float3 _offset = -new float3((float) this.m_mapSize * -0.5f, 0.0f, (float) this.m_mapSize * -0.5f);
      return new WaterSurfaceData(this.m_CPU, _resolution, _scale, _offset);
    }

    public SurfaceWater GetSurface(int2 cell) => this.m_CPU[cell.x + 1 + this.m_TexSize.x * cell.y];

    private void GetReadbackBounds(out int2 pos, out int2 size)
    {
      SurfaceDataReader.GetReadbackBounds(this.m_TexSize, this.m_ReadbackDistribution, this.m_ReadbackIndex, out pos, out size);
    }

    private static void GetReadbackBounds(
      int2 texSize,
      int readbackDistribution,
      int readbackIndex,
      out int2 pos,
      out int2 size)
    {
      size.x = texSize.x / readbackDistribution;
      size.y = texSize.y / readbackDistribution;
      pos.x = readbackIndex % readbackDistribution * size.x;
      pos.y = readbackIndex / readbackDistribution * size.y;
    }

    public void Dispose()
    {
      if (!this.m_AsyncReadback.done)
        this.m_AsyncReadback.WaitForCompletion();
      if (this.m_CPU.IsCreated)
        this.m_CPU.Dispose();
      if (this.m_CPUTemp.IsCreated)
        this.m_CPUTemp.Dispose();
      this.m_Readers.Complete();
    }

    public bool PendingReadback { get; set; }

    public NativeArray<SurfaceWater> WaterSurfaceCPUArray => this.m_CPU;

    [BurstCompile]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyWaterValues\u0024BurstManaged(
      ref AsyncGPUReadbackRequest asyncReadback,
      ref NativeArray<SurfaceWater> cpu,
      ref NativeArray<float4> cpuTemp,
      ref JobHandle readers,
      ref int2 texSize,
      ref bool pendingReadback,
      int readbackDistribution,
      int readbackIndex)
    {
      if (!asyncReadback.hasError && cpu.IsCreated)
      {
        readers.Complete();
        int2 pos;
        int2 size;
        SurfaceDataReader.GetReadbackBounds(texSize, readbackDistribution, readbackIndex, out pos, out size);
        for (int index1 = 0; index1 < size.y; ++index1)
        {
          for (int index2 = 0; index2 < size.x; ++index2)
          {
            int index3 = pos.x + index2 + (pos.y + index1) * texSize.x;
            float4 float4 = cpuTemp[index2 + index1 * size.x];
            float w = float4.w;
            SurfaceWater surfaceWater = new SurfaceWater()
            {
              m_Depth = math.max(float4.x, 0.0f),
              m_Polluted = w,
              m_Velocity = float4.yz
            };
            cpu[index3] = surfaceWater;
          }
        }
        pendingReadback = false;
      }
      else
        Debug.LogWarning((object) "Error in readback");
    }

    public delegate void CopyWaterValues_00005969\u0024PostfixBurstDelegate(
      ref AsyncGPUReadbackRequest asyncReadback,
      ref NativeArray<SurfaceWater> cpu,
      ref NativeArray<float4> cpuTemp,
      ref JobHandle readers,
      ref int2 texSize,
      ref bool pendingReadback,
      int readbackDistribution,
      int readbackIndex);

    internal static class CopyWaterValues_00005969\u0024BurstDirectCall
    {
      private static IntPtr Pointer;
      private static IntPtr DeferredCompilation;

      [BurstDiscard]
      private static unsafe void GetFunctionPointerDiscard([In] ref IntPtr obj0)
      {
        if (SurfaceDataReader.CopyWaterValues_00005969\u0024BurstDirectCall.Pointer == IntPtr.Zero)
        {
          // ISSUE: method reference
          // ISSUE: type reference
          SurfaceDataReader.CopyWaterValues_00005969\u0024BurstDirectCall.Pointer = (IntPtr) BurstCompiler.GetILPPMethodFunctionPointer2(SurfaceDataReader.CopyWaterValues_00005969\u0024BurstDirectCall.DeferredCompilation, __methodref (SurfaceDataReader.CopyWaterValues\u0024BurstManaged), __typeref (SurfaceDataReader.CopyWaterValues_00005969\u0024PostfixBurstDelegate));
        }
        obj0 = SurfaceDataReader.CopyWaterValues_00005969\u0024BurstDirectCall.Pointer;
      }

      private static IntPtr GetFunctionPointer()
      {
        IntPtr zero = IntPtr.Zero;
        SurfaceDataReader.CopyWaterValues_00005969\u0024BurstDirectCall.GetFunctionPointerDiscard(ref zero);
        return zero;
      }

      public static void Constructor()
      {
        // ISSUE: method reference
        SurfaceDataReader.CopyWaterValues_00005969\u0024BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(__methodref (SurfaceDataReader.CopyWaterValues));
      }

      public static void Initialize()
      {
      }

      static CopyWaterValues_00005969\u0024BurstDirectCall()
      {
        SurfaceDataReader.CopyWaterValues_00005969\u0024BurstDirectCall.Constructor();
      }

      public static void Invoke(
        ref AsyncGPUReadbackRequest asyncReadback,
        ref NativeArray<SurfaceWater> cpu,
        ref NativeArray<float4> cpuTemp,
        ref JobHandle readers,
        ref int2 texSize,
        ref bool pendingReadback,
        int readbackDistribution,
        int readbackIndex)
      {
        if (BurstCompiler.IsEnabled)
        {
          IntPtr functionPointer = SurfaceDataReader.CopyWaterValues_00005969\u0024BurstDirectCall.GetFunctionPointer();
          if (functionPointer != IntPtr.Zero)
          {
            ref AsyncGPUReadbackRequest local1 = ref asyncReadback;
            ref NativeArray<SurfaceWater> local2 = ref cpu;
            ref NativeArray<float4> local3 = ref cpuTemp;
            ref JobHandle local4 = ref readers;
            ref int2 local5 = ref texSize;
            ref bool local6 = ref pendingReadback;
            int num1 = readbackDistribution;
            int num2 = readbackIndex;
            // ISSUE: cast to a function pointer type
            // ISSUE: cast to a reference type
            // ISSUE: cast to a reference type
            // ISSUE: cast to a reference type
            // ISSUE: cast to a reference type
            // ISSUE: cast to a reference type
            // ISSUE: cast to a reference type
            // ISSUE: function pointer call
            __calli((__FnPtr<void (AsyncGPUReadbackRequest&, NativeArray<SurfaceWater>&, NativeArray<float4>&, JobHandle&, int2&, bool&, int, int)>) functionPointer)((int) ref local1, (int) ref local2, (bool&) ref local3, (int2&) ref local4, (JobHandle&) ref local5, (NativeArray<float4>&) ref local6, (NativeArray<SurfaceWater>&) num1, (AsyncGPUReadbackRequest&) num2);
            return;
          }
        }
        SurfaceDataReader.CopyWaterValues\u0024BurstManaged(ref asyncReadback, ref cpu, ref cpuTemp, ref readers, ref texSize, ref pendingReadback, readbackDistribution, readbackIndex);
      }
    }
  }
}
