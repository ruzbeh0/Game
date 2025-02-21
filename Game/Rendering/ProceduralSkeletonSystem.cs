// Decompiled with JetBrains decompiler
// Type: Game.Rendering.ProceduralSkeletonSystem
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
  public class ProceduralSkeletonSystem : GameSystemBase, IPreDeserialize
  {
    public const uint SKELETON_MEMORY_DEFAULT = 4194304;
    public const uint SKELETON_MEMORY_INCREMENT = 1048576;
    public const uint UPLOADER_CHUNK_SIZE = 524288;
    private RenderingSystem m_RenderingSystem;
    private NativeHeapAllocator m_HeapAllocator;
    private SparseUploader m_SparseUploader;
    private ThreadedSparseUploader m_ThreadedSparseUploader;
    private NativeReference<ProceduralSkeletonSystem.AllocationInfo> m_AllocationInfo;
    private NativeQueue<ProceduralSkeletonSystem.AllocationRemove> m_AllocationRemoves;
    private bool m_IsAllocating;
    private bool m_IsUploading;
    private GraphicsBuffer m_ComputeBuffer;
    private JobHandle m_HeapDeps;
    private JobHandle m_UploadDeps;
    private int m_HeapAllocatorByteSize;
    private int m_CurrentTime;
    private bool m_AreMotionVectorsEnabled;
    private bool m_ForceHistoryUpdate;

    public bool isMotionBlurEnabled => this.m_AreMotionVectorsEnabled;

    public bool forceHistoryUpdate => this.m_ForceHistoryUpdate;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      this.m_HeapAllocator = new NativeHeapAllocator(4194304U / (uint) sizeof (float4x4), 1U, Allocator.Persistent);
      this.m_SparseUploader = new SparseUploader("Procedural skeleton uploader", (GraphicsBuffer) null, 524288);
      this.m_AllocationInfo = new NativeReference<ProceduralSkeletonSystem.AllocationInfo>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_AllocationRemoves = new NativeQueue<ProceduralSkeletonSystem.AllocationRemove>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
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
      bool motionVectors = this.m_RenderingSystem.motionVectors;
      int num1 = motionVectors ? 2 : 1;
      this.m_ForceHistoryUpdate = this.m_AreMotionVectorsEnabled != motionVectors;
      this.CompleteUpload();
      this.m_HeapDeps.Complete();
      if (this.m_IsAllocating || this.m_ForceHistoryUpdate)
      {
        this.m_IsAllocating = false;
        this.m_AreMotionVectorsEnabled = motionVectors;
        this.m_HeapAllocatorByteSize = (int) this.m_HeapAllocator.Size * sizeof (float4x4);
        int num2 = this.m_HeapAllocatorByteSize * num1;
        int num3 = this.m_ComputeBuffer != null ? this.m_ComputeBuffer.count * this.m_ComputeBuffer.stride : 0;
        if (num2 != num3)
        {
          GraphicsBuffer buffer = new GraphicsBuffer(GraphicsBuffer.Target.Raw, num2 / 4, 4);
          buffer.name = "Procedural bone buffer";
          Shader.SetGlobalBuffer("_BoneTransforms", buffer);
          if (motionVectors && !this.m_ForceHistoryUpdate)
            this.m_SparseUploader.ReplaceBuffer(buffer, true, num3 / 2);
          else
            this.m_SparseUploader.ReplaceBuffer(buffer, true);
          if (this.m_ComputeBuffer == null)
            buffer.SetData<float4x4>(new List<float4x4>()
            {
              float4x4.identity
            }, 0, 0, 1);
          if (motionVectors && (this.m_ComputeBuffer == null || this.m_ForceHistoryUpdate))
          {
            GraphicsBuffer graphicsBuffer = buffer;
            List<float4x4> data = new List<float4x4>();
            data.Add(float4x4.identity);
            int size = (int) this.m_HeapAllocator.Size;
            graphicsBuffer.SetData<float4x4>(data, 0, size, 1);
          }
          if (this.m_ComputeBuffer != null)
            this.m_ComputeBuffer.Release();
          this.m_ComputeBuffer = buffer;
        }
        Shader.SetGlobalInt("_BonePreviousTransformsByteOffset", this.m_HeapAllocatorByteSize);
      }
      if (this.m_AllocationRemoves.IsEmpty())
        return;
      this.m_CurrentTime = this.m_CurrentTime + this.m_RenderingSystem.lodTimerDelta & (int) ushort.MaxValue;
      this.m_HeapDeps = new ProceduralSkeletonSystem.RemoveAllocationsJob()
      {
        m_HeapAllocator = this.m_HeapAllocator,
        m_AllocationInfo = this.m_AllocationInfo,
        m_AllocationRemoves = this.m_AllocationRemoves,
        m_CurrentTime = this.m_CurrentTime
      }.Schedule<ProceduralSkeletonSystem.RemoveAllocationsJob>();
    }

    public ThreadedSparseUploader BeginUpload(
      int opCount,
      uint dataSize,
      uint maxOpSize,
      out int historyByteOffset)
    {
      this.m_ThreadedSparseUploader = this.m_SparseUploader.Begin((int) dataSize, (int) maxOpSize, opCount);
      this.m_IsUploading = true;
      historyByteOffset = this.m_HeapAllocatorByteSize;
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
      out NativeReference<ProceduralSkeletonSystem.AllocationInfo> allocationInfo,
      out NativeQueue<ProceduralSkeletonSystem.AllocationRemove> allocationRemoves,
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
      int num = this.m_RenderingSystem.motionVectors ? 2 : 1;
      allocatedSize = (uint) ((int) this.m_HeapAllocator.UsedSpace * sizeof (float4x4) * num);
      bufferSize = (uint) ((int) this.m_HeapAllocator.Size * sizeof (float4x4) * num);
      allocationCount = (int) this.m_AllocationInfo.Value.m_AllocationCount;
      SparseUploaderStats stats = this.m_SparseUploader.ComputeStats();
      currentUpload = (uint) stats.BytesGPUMemoryUploadedCurr;
      uploadSize = (uint) stats.BytesGPUMemoryUsed;
    }

    private void AllocateIdentityEntry()
    {
      this.m_IsAllocating = true;
      this.m_HeapAllocator.Allocate(1U);
      this.m_AllocationInfo.Value = new ProceduralSkeletonSystem.AllocationInfo()
      {
        m_AllocationCount = 0U
      };
    }

    public static void GetSkinMatrices(
      Skeleton skeleton,
      in DynamicBuffer<ProceduralBone> proceduralBones,
      in DynamicBuffer<Bone> bones,
      NativeList<float4x4> tempMatrices)
    {
      for (int index = 0; index < proceduralBones.Length; ++index)
      {
        ProceduralBone proceduralBone = proceduralBones[index];
        Bone bone = bones[skeleton.m_BoneOffset + index];
        float4x4 float4x4 = float4x4.TRS(bone.m_Position, bone.m_Rotation, bone.m_Scale);
        if (proceduralBone.m_ParentIndex >= 0)
          float4x4 = math.mul(tempMatrices[proceduralBone.m_ParentIndex], float4x4);
        tempMatrices[index] = float4x4;
        tempMatrices[proceduralBones.Length + proceduralBone.m_BindIndex] = math.mul(float4x4, proceduralBone.m_BindPose);
      }
    }

    [UnityEngine.Scripting.Preserve]
    public ProceduralSkeletonSystem()
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
      public NativeReference<ProceduralSkeletonSystem.AllocationInfo> m_AllocationInfo;
      public NativeQueue<ProceduralSkeletonSystem.AllocationRemove> m_AllocationRemoves;
      public int m_CurrentTime;

      public void Execute()
      {
        ref ProceduralSkeletonSystem.AllocationInfo local = ref this.m_AllocationInfo.ValueAsRef<ProceduralSkeletonSystem.AllocationInfo>();
        while (!this.m_AllocationRemoves.IsEmpty())
        {
          ProceduralSkeletonSystem.AllocationRemove allocationRemove = this.m_AllocationRemoves.Peek();
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
