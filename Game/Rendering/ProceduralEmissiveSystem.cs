// Decompiled with JetBrains decompiler
// Type: Game.Rendering.ProceduralEmissiveSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Rendering;
using Game.Prefabs;
using Game.Serialization;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  public class ProceduralEmissiveSystem : GameSystemBase, IPreDeserialize
  {
    public const uint EMISSIVE_MEMORY_DEFAULT = 2097152;
    public const uint EMISSIVE_MEMORY_INCREMENT = 1048576;
    public const uint UPLOADER_CHUNK_SIZE = 131072;
    private RenderingSystem m_RenderingSystem;
    private NativeHeapAllocator m_HeapAllocator;
    private SparseUploader m_SparseUploader;
    private ThreadedSparseUploader m_ThreadedSparseUploader;
    private NativeReference<ProceduralEmissiveSystem.AllocationInfo> m_AllocationInfo;
    private NativeQueue<ProceduralEmissiveSystem.AllocationRemove> m_AllocationRemoves;
    private bool m_IsAllocating;
    private bool m_IsUploading;
    private GraphicsBuffer m_ComputeBuffer;
    private JobHandle m_HeapDeps;
    private JobHandle m_UploadDeps;
    private int m_HeapAllocatorByteSize;
    private int m_CurrentTime;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      this.m_HeapAllocator = new NativeHeapAllocator(2097152U / (uint) sizeof (float4), 1U, Allocator.Persistent);
      this.m_SparseUploader = new SparseUploader("Procedural emissive uploader", (GraphicsBuffer) null, 131072);
      this.m_AllocationInfo = new NativeReference<ProceduralEmissiveSystem.AllocationInfo>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_AllocationRemoves = new NativeQueue<ProceduralEmissiveSystem.AllocationRemove>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.AllocateIdentityEntry();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      this.CompleteUpload();
      this.m_HeapDeps.Complete();
      if (this.m_HeapAllocator.IsCreated)
      {
        this.m_HeapAllocator.Dispose();
        this.m_SparseUploader.Dispose();
        this.m_AllocationInfo.Dispose();
        this.m_AllocationRemoves.Dispose();
      }
      if (this.m_ComputeBuffer != null)
        this.m_ComputeBuffer.Release();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      this.CompleteUpload();
      this.m_HeapDeps.Complete();
      if (this.m_IsAllocating)
      {
        this.m_IsAllocating = false;
        this.m_HeapAllocatorByteSize = (int) this.m_HeapAllocator.Size * sizeof (float4);
        int allocatorByteSize = this.m_HeapAllocatorByteSize;
        int num = this.m_ComputeBuffer != null ? this.m_ComputeBuffer.count * this.m_ComputeBuffer.stride : 0;
        if (allocatorByteSize != num)
        {
          GraphicsBuffer buffer = new GraphicsBuffer(GraphicsBuffer.Target.Raw, allocatorByteSize / 4, 4);
          buffer.name = "Procedural emissive buffer";
          Shader.SetGlobalBuffer("_LightInfo", buffer);
          this.m_SparseUploader.ReplaceBuffer(buffer, true);
          if (this.m_ComputeBuffer != null)
            this.m_ComputeBuffer.Release();
          else
            buffer.SetData<float4>(new List<float4>()
            {
              float4.zero
            }, 0, 0, 1);
          this.m_ComputeBuffer = buffer;
        }
      }
      if (this.m_AllocationRemoves.IsEmpty())
        return;
      this.m_CurrentTime = this.m_CurrentTime + this.m_RenderingSystem.lodTimerDelta & (int) ushort.MaxValue;
      this.m_HeapDeps = new ProceduralEmissiveSystem.RemoveAllocationsJob()
      {
        m_HeapAllocator = this.m_HeapAllocator,
        m_AllocationInfo = this.m_AllocationInfo,
        m_AllocationRemoves = this.m_AllocationRemoves,
        m_CurrentTime = this.m_CurrentTime
      }.Schedule<ProceduralEmissiveSystem.RemoveAllocationsJob>();
    }

    public ThreadedSparseUploader BeginUpload(int opCount, uint dataSize, uint maxOpSize)
    {
      this.m_ThreadedSparseUploader = this.m_SparseUploader.Begin((int) dataSize, (int) maxOpSize, opCount);
      this.m_IsUploading = true;
      return this.m_ThreadedSparseUploader;
    }

    public void AddUploadWriter(JobHandle handle) => this.m_UploadDeps = handle;

    public void CompleteUpload()
    {
      if (!this.m_IsUploading)
        return;
      this.m_UploadDeps.Complete();
      this.m_IsUploading = false;
      this.m_SparseUploader.EndAndCommit(this.m_ThreadedSparseUploader);
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      this.m_HeapDeps.Complete();
      this.m_HeapAllocator.Clear();
      this.m_AllocationRemoves.Clear();
      this.AllocateIdentityEntry();
    }

    public NativeHeapAllocator GetHeapAllocator(
      out NativeReference<ProceduralEmissiveSystem.AllocationInfo> allocationInfo,
      out NativeQueue<ProceduralEmissiveSystem.AllocationRemove> allocationRemoves,
      out int currentTime,
      out JobHandle dependencies)
    {
      dependencies = this.m_HeapDeps;
      allocationInfo = this.m_AllocationInfo;
      allocationRemoves = this.m_AllocationRemoves;
      currentTime = this.m_CurrentTime;
      this.m_IsAllocating = true;
      return this.m_HeapAllocator;
    }

    public void AddHeapWriter(JobHandle handle) => this.m_HeapDeps = handle;

    public void GetMemoryStats(
      out uint allocatedSize,
      out uint bufferSize,
      out uint currentUpload,
      out uint uploadSize,
      out int allocationCount)
    {
      this.m_HeapDeps.Complete();
      allocatedSize = this.m_HeapAllocator.UsedSpace * (uint) sizeof (float4);
      bufferSize = this.m_HeapAllocator.Size * (uint) sizeof (float4);
      allocationCount = (int) this.m_AllocationInfo.Value.m_AllocationCount;
      SparseUploaderStats stats = this.m_SparseUploader.ComputeStats();
      currentUpload = (uint) stats.BytesGPUMemoryUploadedCurr;
      uploadSize = (uint) stats.BytesGPUMemoryUsed;
    }

    private void AllocateIdentityEntry()
    {
      this.m_IsAllocating = true;
      this.m_HeapAllocator.Allocate(1U);
      this.m_AllocationInfo.Value = new ProceduralEmissiveSystem.AllocationInfo()
      {
        m_AllocationCount = 0U
      };
    }

    public static void GetGpuLights(
      Emissive emissive,
      in DynamicBuffer<ProceduralLight> proceduralLights,
      in DynamicBuffer<LightState> lights,
      NativeList<float4> gpuLights)
    {
      gpuLights[0] = new float4();
      for (int index = 0; index < proceduralLights.Length; ++index)
      {
        ProceduralLight proceduralLight = proceduralLights[index];
        LightState lightState = lights[emissive.m_LightOffset + index];
        float4 float4 = math.lerp(proceduralLight.m_Color, proceduralLight.m_Color2, lightState.m_Color);
        float4.w *= lightState.m_Intensity;
        gpuLights[index + 1] = float4;
      }
    }

    [UnityEngine.Scripting.Preserve]
    public ProceduralEmissiveSystem()
    {
    }

    public struct AllocationInfo
    {
      public uint m_AllocationCount;
    }

    public struct AllocationRemove
    {
      public NativeHeapBlock m_Allocation;
      public int m_RemoveTime;
    }

    [BurstCompile]
    private struct RemoveAllocationsJob : IJob
    {
      public NativeHeapAllocator m_HeapAllocator;
      public NativeReference<ProceduralEmissiveSystem.AllocationInfo> m_AllocationInfo;
      public NativeQueue<ProceduralEmissiveSystem.AllocationRemove> m_AllocationRemoves;
      public int m_CurrentTime;

      public void Execute()
      {
        ref ProceduralEmissiveSystem.AllocationInfo local = ref this.m_AllocationInfo.ValueAsRef<ProceduralEmissiveSystem.AllocationInfo>();
        while (!this.m_AllocationRemoves.IsEmpty())
        {
          ProceduralEmissiveSystem.AllocationRemove allocationRemove = this.m_AllocationRemoves.Peek();
          int num = this.m_CurrentTime - allocationRemove.m_RemoveTime;
          if (num + math.select(0, 65536, num < 0) < (int) byte.MaxValue)
            break;
          this.m_AllocationRemoves.Dequeue();
          this.m_HeapAllocator.Release(allocationRemove.m_Allocation);
          --local.m_AllocationCount;
        }
      }
    }
  }
}
