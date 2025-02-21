// Decompiled with JetBrains decompiler
// Type: Game.Rendering.AnimatedSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Animations;
using Colossal.Collections;
using Colossal.Entities;
using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.Mathematics;
using Game.Prefabs;
using Game.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class AnimatedSystem : GameSystemBase, IPreDeserialize
  {
    public const uint BONEBUFFER_MEMORY_DEFAULT = 8388608;
    public const uint BONEBUFFER_MEMORY_INCREMENT = 2097152;
    public const uint ANIMBUFFER_MEMORY_DEFAULT = 33554432;
    public const uint ANIMBUFFER_MEMORY_INCREMENT = 8388608;
    public const uint METABUFFER_MEMORY_DEFAULT = 1048576;
    public const uint METABUFFER_MEMORY_INCREMENT = 262144;
    public const uint INDEXBUFFER_MEMORY_DEFAULT = 65536;
    public const uint INDEXBUFFER_MEMORY_INCREMENT = 16384;
    public const uint MAX_ASYNC_LOADING_COUNT = 10;
    private const string ANIMATION_COMPUTE_SHADER_RESOURCE = "Didimo/AnimationBlendCompute";
    private const string SHADER_BLEND_ANIMATION_LAYER0_KERNEL_NAME = "BlendAnimationLayer0";
    private const string SHADER_BLEND_ANIMATION_LAYER1_KERNEL_NAME = "BlendAnimationLayer1";
    private const string SHADER_BLEND_ANIMATION_LAYER2_KERNEL_NAME = "BlendAnimationLayer2";
    private const string SHADER_BLEND_TRANSITION_LAYER0_KERNEL_NAME = "BlendTransitionLayer0";
    private const string SHADER_BLEND_TRANSITION2_LAYER0_KERNEL_NAME = "BlendTransition2Layer0";
    private const string SHADER_BLEND_TRANSITION_LAYER1_KERNEL_NAME = "BlendTransitionLayer1";
    private const string SHADER_BLEND_REST_POSE_KERNEL_NAME = "BlendRestPose";
    private const string SHADER_CONVERT_COORDINATES_KERNEL_NAME = "ConvertLocalCoordinates";
    private const string SHADER_CONVERT_COORDINATES_WITH_HISTORY_KERNEL_NAME = "ConvertLocalCoordinatesWithHistory";
    private static ILog log;
    private RenderingSystem m_RenderingSystem;
    private PrefabSystem m_PrefabSystem;
    private NativeHeapAllocator m_BoneAllocator;
    private NativeHeapAllocator m_AnimAllocator;
    private NativeHeapAllocator m_IndexAllocator;
    private NativeList<MetaBufferData> m_MetaBufferData;
    private NativeList<int> m_FreeMetaIndices;
    private NativeList<int> m_UpdatedMetaIndices;
    private NativeList<RestPoseInstance> m_InstanceIndices;
    private NativeList<AnimatedInstance> m_BodyInstances;
    private NativeList<AnimatedInstance> m_FaceInstances;
    private NativeList<AnimatedInstance> m_CorrectiveInstances;
    private NativeList<AnimatedTransition> m_BodyTransitions;
    private NativeList<AnimatedTransition2> m_BodyTransitions2;
    private NativeList<AnimatedTransition> m_FaceTransitions;
    private NativeList<AnimatedSystem.ClipPriorityData> m_ClipPriorities;
    private NativeList<AnimatedSystem.AnimationClipData> m_AnimationClipData;
    private NativeList<int> m_FreeAnimIndices;
    private NativeQueue<AnimatedSystem.AllocationRemove> m_BoneAllocationRemoves;
    private NativeQueue<AnimatedSystem.IndexRemove> m_MetaBufferRemoves;
    private ComputeBuffer m_BoneBuffer;
    private ComputeBuffer m_BoneHistoryBuffer;
    private ComputeBuffer m_LocalTRSBlendPoseBuffer;
    private ComputeBuffer m_LocalTRSBoneBuffer;
    private ComputeBuffer m_AnimInfoBuffer;
    private ComputeBuffer m_AnimBuffer;
    private ComputeBuffer m_MetaBuffer;
    private ComputeBuffer m_IndexBuffer;
    private ComputeBuffer m_InstanceBuffer;
    private ComputeBuffer m_BodyInstanceBuffer;
    private ComputeBuffer m_FaceInstanceBuffer;
    private ComputeBuffer m_CorrectiveInstanceBuffer;
    private ComputeBuffer m_BodyTransitionBuffer;
    private ComputeBuffer m_BodyTransition2Buffer;
    private ComputeBuffer m_FaceTransitionBuffer;
    private int m_AnimationCount;
    private int m_MaxBoneCount;
    private int m_MaxActiveBoneCount;
    private int m_CurrentTime;
    private bool m_IsAllocating;
    private System.Collections.Generic.Dictionary<string, int> m_PropIDs;
    private ComputeShader m_AnimationComputeShader;
    private NativeQueue<AnimatedSystem.AnimationFrameData> m_TempAnimationQueue;
    private NativeQueue<AnimatedSystem.ClipPriorityData> m_TempPriorityQueue;
    private JobHandle m_AllocateDeps;
    private int m_BlendAnimationLayer0KernelIx;
    private int m_BlendAnimationLayer1KernelIx;
    private int m_BlendAnimationLayer2KernelIx;
    private int m_BlendTransitionLayer0KernelIx;
    private int m_BlendTransition2Layer0KernelIx;
    private int m_BlendTransitionLayer1KernelIx;
    private int m_BlendRestPoseKernelIx;
    private int m_ConvertLocalCoordinatesKernelIx;
    private int m_ConvertLocalCoordinatesWithHistoryKernelIx;
    private int m_IndexBufferID;
    private int m_MetadataBufferID;
    private int m_MetaIndexBufferID;
    private int m_AnimatedInstanceBufferID;
    private int m_AnimatedTransitionBufferID;
    private int m_AnimatedTransition2BufferID;
    private int m_AnimationInfoBufferID;
    private int m_AnimationBoneBufferID;
    private int m_InstanceCountID;
    private int m_BodyInstanceCountID;
    private int m_BodyTransitionCountID;
    private int m_BodyTransition2CountID;
    private int m_FaceInstanceCountID;
    private int m_FaceTransitionCountID;
    private int m_CorrectiveInstanceCountID;
    private int m_LocalTRSBlendPoseBufferID;
    private int m_LocalTRSBoneBufferID;
    private int m_BoneBufferID;
    private int m_BoneHistoryBufferID;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated field
      AnimatedSystem.log = LogManager.GetLogger("Rendering");
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BoneAllocator = new NativeHeapAllocator(8388608U / (uint) sizeof (BoneElement), 1U, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_AnimAllocator = new NativeHeapAllocator(33554432U / (uint) sizeof (Colossal.Animations.Animation.Element), 1U, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_IndexAllocator = new NativeHeapAllocator(16384U, 1U, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_MetaBufferData = new NativeList<MetaBufferData>(1000, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_FreeMetaIndices = new NativeList<int>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedMetaIndices = new NativeList<int>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_InstanceIndices = new NativeList<RestPoseInstance>(1000, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BodyInstances = new NativeList<AnimatedInstance>(1000, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_FaceInstances = new NativeList<AnimatedInstance>(1000, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_CorrectiveInstances = new NativeList<AnimatedInstance>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BodyTransitions = new NativeList<AnimatedTransition>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BodyTransitions2 = new NativeList<AnimatedTransition2>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_FaceTransitions = new NativeList<AnimatedTransition>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ClipPriorities = new NativeList<AnimatedSystem.ClipPriorityData>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationClipData = new NativeList<AnimatedSystem.AnimationClipData>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_FreeAnimIndices = new NativeList<int>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BoneAllocationRemoves = new NativeQueue<AnimatedSystem.AllocationRemove>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_MetaBufferRemoves = new NativeQueue<AnimatedSystem.IndexRemove>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_PropIDs = new System.Collections.Generic.Dictionary<string, int>();
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader = UnityEngine.Object.Instantiate<ComputeShader>(Resources.Load<ComputeShader>("Didimo/AnimationBlendCompute"));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_BlendAnimationLayer0KernelIx = this.m_AnimationComputeShader.FindKernel("BlendAnimationLayer0");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_BlendAnimationLayer1KernelIx = this.m_AnimationComputeShader.FindKernel("BlendAnimationLayer1");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_BlendAnimationLayer2KernelIx = this.m_AnimationComputeShader.FindKernel("BlendAnimationLayer2");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_BlendTransitionLayer0KernelIx = this.m_AnimationComputeShader.FindKernel("BlendTransitionLayer0");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_BlendTransition2Layer0KernelIx = this.m_AnimationComputeShader.FindKernel("BlendTransition2Layer0");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_BlendTransitionLayer1KernelIx = this.m_AnimationComputeShader.FindKernel("BlendTransitionLayer1");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_BlendRestPoseKernelIx = this.m_AnimationComputeShader.FindKernel("BlendRestPose");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ConvertLocalCoordinatesKernelIx = this.m_AnimationComputeShader.FindKernel("ConvertLocalCoordinates");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ConvertLocalCoordinatesWithHistoryKernelIx = this.m_AnimationComputeShader.FindKernel("ConvertLocalCoordinatesWithHistory");
      // ISSUE: reference to a compiler-generated field
      this.m_IndexBufferID = Shader.PropertyToID("IndexDataBuffer");
      // ISSUE: reference to a compiler-generated field
      this.m_MetadataBufferID = Shader.PropertyToID("MetaDataBuffer");
      // ISSUE: reference to a compiler-generated field
      this.m_MetaIndexBufferID = Shader.PropertyToID("MetaIndexBuffer");
      // ISSUE: reference to a compiler-generated field
      this.m_AnimatedInstanceBufferID = Shader.PropertyToID("AnimatedInstanceBuffer");
      // ISSUE: reference to a compiler-generated field
      this.m_AnimatedTransitionBufferID = Shader.PropertyToID("AnimatedTransitionBuffer");
      // ISSUE: reference to a compiler-generated field
      this.m_AnimatedTransition2BufferID = Shader.PropertyToID("AnimatedTransition2Buffer");
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationInfoBufferID = Shader.PropertyToID("AnimationInfoBuffer");
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationBoneBufferID = Shader.PropertyToID("AnimationBoneBuffer");
      // ISSUE: reference to a compiler-generated field
      this.m_InstanceCountID = Shader.PropertyToID("instanceCount");
      // ISSUE: reference to a compiler-generated field
      this.m_BodyInstanceCountID = Shader.PropertyToID("bodyInstanceCount");
      // ISSUE: reference to a compiler-generated field
      this.m_BodyTransitionCountID = Shader.PropertyToID("bodyTransitionCount");
      // ISSUE: reference to a compiler-generated field
      this.m_BodyTransition2CountID = Shader.PropertyToID("bodyTransition2Count");
      // ISSUE: reference to a compiler-generated field
      this.m_FaceInstanceCountID = Shader.PropertyToID("faceInstanceCount");
      // ISSUE: reference to a compiler-generated field
      this.m_FaceTransitionCountID = Shader.PropertyToID("faceTransitionCount");
      // ISSUE: reference to a compiler-generated field
      this.m_CorrectiveInstanceCountID = Shader.PropertyToID("correctiveInstanceCount");
      // ISSUE: reference to a compiler-generated field
      this.m_LocalTRSBlendPoseBufferID = Shader.PropertyToID("LocalTRSBlendPoseBuffer");
      // ISSUE: reference to a compiler-generated field
      this.m_LocalTRSBoneBufferID = Shader.PropertyToID("LocalTRSBoneBuffer");
      // ISSUE: reference to a compiler-generated field
      this.m_BoneBufferID = Shader.PropertyToID("BoneBuffer");
      // ISSUE: reference to a compiler-generated field
      this.m_BoneHistoryBufferID = Shader.PropertyToID("BoneHistoryBuffer");
      // ISSUE: reference to a compiler-generated field
      this.m_BoneAllocator.Allocate(1U);
      // ISSUE: reference to a compiler-generated field
      this.m_MetaBufferData.Add(new MetaBufferData());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_IsAllocating)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AllocateDeps.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_IsAllocating = false;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_BoneAllocator.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_AnimAllocator.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_IndexAllocator.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_MetaBufferData.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_FreeMetaIndices.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedMetaIndices.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_InstanceIndices.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_BodyInstances.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_FaceInstances.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_CorrectiveInstances.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_BodyTransitions.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_BodyTransitions2.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_FaceTransitions.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ClipPriorities.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationClipData.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_FreeAnimIndices.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_BoneAllocationRemoves.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_MetaBufferRemoves.Dispose();
      // ISSUE: reference to a compiler-generated field
      if (this.m_BoneBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BoneBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_BoneHistoryBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BoneHistoryBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_LocalTRSBlendPoseBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LocalTRSBlendPoseBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_LocalTRSBoneBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LocalTRSBoneBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_AnimInfoBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AnimInfoBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_AnimBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AnimBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_MetaBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_MetaBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_IndexBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_IndexBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_InstanceBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_InstanceBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_BodyInstanceBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BodyInstanceBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_FaceInstanceBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_FaceInstanceBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_CorrectiveInstanceBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CorrectiveInstanceBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_BodyTransitionBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BodyTransitionBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_BodyTransition2Buffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BodyTransition2Buffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_FaceTransitionBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_FaceTransitionBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_AnimationComputeShader);
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_IsAllocating)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AllocateDeps.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_IsAllocating = false;
        // ISSUE: reference to a compiler-generated method
        this.ResizeBoneBuffer();
        // ISSUE: reference to a compiler-generated method
        this.ResizeMetaBuffer();
        // ISSUE: reference to a compiler-generated method
        this.UpdateAnimations();
        // ISSUE: reference to a compiler-generated method
        this.UpdateMetaData();
        // ISSUE: reference to a compiler-generated method
        this.UpdateInstances();
      }
      // ISSUE: reference to a compiler-generated method
      this.PlayAnimations();
    }

    private void PlayAnimations()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_InstanceIndices.Length == 0 || this.m_MaxBoneCount == 0)
        return;
      // ISSUE: reference to a compiler-generated method
      this.ResizeBoneHistoryBuffer();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetInt(this.m_InstanceCountID, this.m_InstanceIndices.Length);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetInt(this.m_BodyInstanceCountID, this.m_BodyInstances.Length);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetInt(this.m_BodyTransitionCountID, this.m_BodyTransitions.Length);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetInt(this.m_BodyTransition2CountID, this.m_BodyTransitions2.Length);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetInt(this.m_FaceInstanceCountID, this.m_FaceInstances.Length);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetInt(this.m_FaceTransitionCountID, this.m_FaceTransitions.Length);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetInt(this.m_CorrectiveInstanceCountID, this.m_CorrectiveInstances.Length);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetBuffer(this.m_BlendRestPoseKernelIx, this.m_MetadataBufferID, this.m_MetaBuffer);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetBuffer(this.m_BlendRestPoseKernelIx, this.m_MetaIndexBufferID, this.m_InstanceBuffer);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetBuffer(this.m_BlendRestPoseKernelIx, this.m_LocalTRSBoneBufferID, this.m_LocalTRSBoneBuffer);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetBuffer(this.m_BlendRestPoseKernelIx, this.m_LocalTRSBlendPoseBufferID, this.m_LocalTRSBlendPoseBuffer);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetBuffer(this.m_BlendRestPoseKernelIx, this.m_AnimationInfoBufferID, this.m_AnimInfoBuffer);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetBuffer(this.m_BlendRestPoseKernelIx, this.m_AnimationBoneBufferID, this.m_AnimBuffer);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetBuffer(this.m_BlendRestPoseKernelIx, this.m_IndexBufferID, this.m_IndexBuffer);
      // ISSUE: reference to a compiler-generated field
      if (this.m_BodyInstances.Length != 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer0KernelIx, this.m_AnimationInfoBufferID, this.m_AnimInfoBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer0KernelIx, this.m_AnimationBoneBufferID, this.m_AnimBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer0KernelIx, this.m_MetadataBufferID, this.m_MetaBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer0KernelIx, this.m_AnimatedInstanceBufferID, this.m_BodyInstanceBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer0KernelIx, this.m_LocalTRSBoneBufferID, this.m_LocalTRSBoneBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer0KernelIx, this.m_LocalTRSBlendPoseBufferID, this.m_LocalTRSBlendPoseBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer0KernelIx, this.m_IndexBufferID, this.m_IndexBuffer);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_BodyTransitions.Length != 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransitionLayer0KernelIx, this.m_AnimationInfoBufferID, this.m_AnimInfoBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransitionLayer0KernelIx, this.m_AnimationBoneBufferID, this.m_AnimBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransitionLayer0KernelIx, this.m_MetadataBufferID, this.m_MetaBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransitionLayer0KernelIx, this.m_AnimatedTransitionBufferID, this.m_BodyTransitionBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransitionLayer0KernelIx, this.m_LocalTRSBoneBufferID, this.m_LocalTRSBoneBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransitionLayer0KernelIx, this.m_LocalTRSBlendPoseBufferID, this.m_LocalTRSBlendPoseBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransitionLayer0KernelIx, this.m_IndexBufferID, this.m_IndexBuffer);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_BodyTransitions2.Length != 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransition2Layer0KernelIx, this.m_AnimationInfoBufferID, this.m_AnimInfoBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransition2Layer0KernelIx, this.m_AnimationBoneBufferID, this.m_AnimBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransition2Layer0KernelIx, this.m_MetadataBufferID, this.m_MetaBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransition2Layer0KernelIx, this.m_AnimatedTransition2BufferID, this.m_BodyTransition2Buffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransition2Layer0KernelIx, this.m_LocalTRSBoneBufferID, this.m_LocalTRSBoneBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransition2Layer0KernelIx, this.m_LocalTRSBlendPoseBufferID, this.m_LocalTRSBlendPoseBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransition2Layer0KernelIx, this.m_IndexBufferID, this.m_IndexBuffer);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_FaceInstances.Length != 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer1KernelIx, this.m_AnimationInfoBufferID, this.m_AnimInfoBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer1KernelIx, this.m_AnimationBoneBufferID, this.m_AnimBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer1KernelIx, this.m_MetadataBufferID, this.m_MetaBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer1KernelIx, this.m_AnimatedInstanceBufferID, this.m_FaceInstanceBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer1KernelIx, this.m_LocalTRSBoneBufferID, this.m_LocalTRSBoneBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer1KernelIx, this.m_LocalTRSBlendPoseBufferID, this.m_LocalTRSBlendPoseBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer1KernelIx, this.m_IndexBufferID, this.m_IndexBuffer);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_FaceTransitions.Length != 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransitionLayer1KernelIx, this.m_AnimationInfoBufferID, this.m_AnimInfoBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransitionLayer1KernelIx, this.m_AnimationBoneBufferID, this.m_AnimBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransitionLayer1KernelIx, this.m_MetadataBufferID, this.m_MetaBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransitionLayer1KernelIx, this.m_AnimatedTransitionBufferID, this.m_FaceTransitionBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransitionLayer1KernelIx, this.m_LocalTRSBoneBufferID, this.m_LocalTRSBoneBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransitionLayer1KernelIx, this.m_LocalTRSBlendPoseBufferID, this.m_LocalTRSBlendPoseBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendTransitionLayer1KernelIx, this.m_IndexBufferID, this.m_IndexBuffer);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_CorrectiveInstances.Length != 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer2KernelIx, this.m_AnimationInfoBufferID, this.m_AnimInfoBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer2KernelIx, this.m_AnimationBoneBufferID, this.m_AnimBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer2KernelIx, this.m_MetadataBufferID, this.m_MetaBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer2KernelIx, this.m_AnimatedInstanceBufferID, this.m_CorrectiveInstanceBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer2KernelIx, this.m_LocalTRSBoneBufferID, this.m_LocalTRSBoneBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer2KernelIx, this.m_LocalTRSBlendPoseBufferID, this.m_LocalTRSBlendPoseBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(this.m_BlendAnimationLayer2KernelIx, this.m_IndexBufferID, this.m_IndexBuffer);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int kernelIndex = this.m_RenderingSystem.motionVectors ? this.m_ConvertLocalCoordinatesWithHistoryKernelIx : this.m_ConvertLocalCoordinatesKernelIx;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetBuffer(kernelIndex, this.m_MetadataBufferID, this.m_MetaBuffer);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetBuffer(kernelIndex, this.m_MetaIndexBufferID, this.m_InstanceBuffer);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetBuffer(kernelIndex, this.m_LocalTRSBoneBufferID, this.m_LocalTRSBoneBuffer);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetBuffer(kernelIndex, this.m_LocalTRSBlendPoseBufferID, this.m_LocalTRSBlendPoseBuffer);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetBuffer(kernelIndex, this.m_BoneBufferID, this.m_BoneBuffer);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetBuffer(kernelIndex, this.m_IndexBufferID, this.m_IndexBuffer);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.SetBuffer(kernelIndex, this.m_AnimationInfoBufferID, this.m_AnimInfoBuffer);
      // ISSUE: reference to a compiler-generated field
      if (this.m_RenderingSystem.motionVectors)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(kernelIndex, this.m_BoneHistoryBufferID, this.m_BoneHistoryBuffer);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.SetBuffer(kernelIndex, this.m_BoneHistoryBufferID, this.m_BoneBuffer);
      }
      uint x;
      uint y;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.GetKernelThreadGroupSizes(this.m_BlendRestPoseKernelIx, out x, out y, out uint _);
      // ISSUE: reference to a compiler-generated field
      int threadGroupsX = (this.m_InstanceIndices.Length + (int) x - 1) / (int) x;
      // ISSUE: reference to a compiler-generated field
      int threadGroupsY1 = (this.m_MaxBoneCount + (int) y - 1) / (int) y;
      // ISSUE: reference to a compiler-generated field
      int threadGroupsY2 = (this.m_MaxActiveBoneCount + (int) y - 1) / (int) y;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.Dispatch(this.m_BlendRestPoseKernelIx, threadGroupsX, threadGroupsY1, 1);
      // ISSUE: reference to a compiler-generated field
      if (this.m_BodyInstances.Length != 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.Dispatch(this.m_BlendAnimationLayer0KernelIx, (this.m_BodyInstances.Length + (int) x - 1) / (int) x, threadGroupsY2, 1);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_BodyTransitions.Length != 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.Dispatch(this.m_BlendTransitionLayer0KernelIx, (this.m_BodyTransitions.Length + (int) x - 1) / (int) x, threadGroupsY1, 1);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_BodyTransitions2.Length != 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.Dispatch(this.m_BlendTransition2Layer0KernelIx, (this.m_BodyTransitions2.Length + (int) x - 1) / (int) x, threadGroupsY1, 1);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_FaceInstances.Length != 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.Dispatch(this.m_BlendAnimationLayer1KernelIx, (this.m_FaceInstances.Length + (int) x - 1) / (int) x, threadGroupsY2, 1);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_FaceTransitions.Length != 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.Dispatch(this.m_BlendTransitionLayer1KernelIx, (this.m_FaceTransitions.Length + (int) x - 1) / (int) x, threadGroupsY1, 1);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_CorrectiveInstances.Length != 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationComputeShader.Dispatch(this.m_BlendAnimationLayer2KernelIx, (this.m_CorrectiveInstances.Length + (int) x - 1) / (int) x, threadGroupsY2, 1);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_AnimationComputeShader.Dispatch(kernelIndex, threadGroupsX, threadGroupsY1, 1);
    }

    private void UpdateInstances()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.UpdateInstanceBuffer<RestPoseInstance>(ref this.m_InstanceBuffer, this.m_InstanceIndices, "InstanceBuffer");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.UpdateInstanceBuffer<AnimatedInstance>(ref this.m_BodyInstanceBuffer, this.m_BodyInstances, "BodyInstanceBuffer");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.UpdateInstanceBuffer<AnimatedInstance>(ref this.m_FaceInstanceBuffer, this.m_FaceInstances, "FaceInstanceBuffer");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.UpdateInstanceBuffer<AnimatedInstance>(ref this.m_CorrectiveInstanceBuffer, this.m_CorrectiveInstances, "CorrectiveInstanceBuffer");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.UpdateInstanceBuffer<AnimatedTransition>(ref this.m_BodyTransitionBuffer, this.m_BodyTransitions, "BodyTransitionBuffer");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.UpdateInstanceBuffer<AnimatedTransition2>(ref this.m_BodyTransition2Buffer, this.m_BodyTransitions2, "BodyTransitionBuffer2");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.UpdateInstanceBuffer<AnimatedTransition>(ref this.m_FaceTransitionBuffer, this.m_FaceTransitions, "FaceTransitionBuffer");
    }

    private void UpdateInstanceBuffer<T>(ref ComputeBuffer buffer, NativeList<T> data, string name) where T : unmanaged
    {
      if (buffer == null || buffer.count != data.Capacity)
      {
        if (buffer != null)
          buffer.Release();
        buffer = new ComputeBuffer(data.Capacity, sizeof (T), ComputeBufferType.Structured);
        buffer.name = name;
      }
      if (data.Length == 0)
        return;
      buffer.SetData<T>(data.AsArray(), 0, 0, data.Length);
    }

    private void UpdateAnimations()
    {
      int num = 0;
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_ClipPriorities.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        ref AnimatedSystem.ClipPriorityData local = ref this.m_ClipPriorities.ElementAt(index);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (local.m_Priority < 0 && !local.m_IsLoading)
          break;
        // ISSUE: reference to a compiler-generated field
        if (!local.m_IsLoaded)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          CharacterStyle prefab = this.m_PrefabSystem.GetPrefab<CharacterStyle>(local.m_ClipIndex.m_Style);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          AnimationAsset animation = prefab.GetAnimation(local.m_ClipIndex.m_Index);
          try
          {
            // ISSUE: reference to a compiler-generated field
            local.m_IsLoading = true;
            Colossal.Animations.AnimationClip clip;
            if (animation.AsyncLoad(out clip))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (this.LoadAnimation(local.m_ClipIndex, clip))
              {
                // ISSUE: reference to a compiler-generated field
                local.m_IsLoading = false;
                // ISSUE: reference to a compiler-generated field
                local.m_IsLoaded = true;
                animation.Unload(false);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (local.m_Priority < 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  local.m_IsLoading = false;
                  animation.Unload(false);
                }
              }
            }
            else if (++num == 10)
              break;
          }
          catch (Exception ex)
          {
            // ISSUE: reference to a compiler-generated field
            AnimatedSystem.log.ErrorFormat(ex, "Error when loading animation: {0}->{1}", (object) prefab.name, (object) animation.name);
            // ISSUE: reference to a compiler-generated field
            local.m_IsLoading = false;
            // ISSUE: reference to a compiler-generated field
            local.m_IsLoaded = true;
            animation.Unload(false);
          }
        }
      }
    }

    private bool LoadAnimation(AnimatedSystem.ClipIndex clipIndex, Colossal.Animations.AnimationClip animation)
    {
      // ISSUE: reference to a compiler-generated field
      CharacterStyleData componentData = this.EntityManager.GetComponentData<CharacterStyleData>(clipIndex.m_Style);
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<Game.Prefabs.AnimationClip> buffer1 = this.EntityManager.GetBuffer<Game.Prefabs.AnimationClip>(clipIndex.m_Style);
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<AnimationMotion> buffer2 = this.EntityManager.GetBuffer<AnimationMotion>(clipIndex.m_Style, true);
      // ISSUE: reference to a compiler-generated field
      ref Game.Prefabs.AnimationClip local1 = ref buffer1.ElementAt(clipIndex.m_Index);
      DynamicBuffer<RestPoseElement> buffer3 = new DynamicBuffer<RestPoseElement>();
      // ISSUE: reference to a compiler-generated field
      if (local1.m_RootMotionBone != -1 && (!this.EntityManager.TryGetBuffer<RestPoseElement>(clipIndex.m_Style, true, out buffer3) || buffer3.Length == 0))
        return false;
      uint length = (uint) animation.m_Animation.elements.Length;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnimatedSystem.AnimationClipData animationClipData = new AnimatedSystem.AnimationClipData();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      animationClipData.m_AnimAllocation = this.m_AnimAllocator.Allocate(length);
      // ISSUE: reference to a compiler-generated field
      ++this.m_AnimationCount;
      // ISSUE: reference to a compiler-generated field
      int num1 = this.m_ClipPriorities.Length - 1;
      // ISSUE: reference to a compiler-generated field
      while (animationClipData.m_AnimAllocation.Empty)
      {
        if (num1 >= 0)
        {
          // ISSUE: reference to a compiler-generated field
          ref AnimatedSystem.ClipPriorityData local2 = ref this.m_ClipPriorities.ElementAt(num1--);
          // ISSUE: reference to a compiler-generated field
          if (local2.m_Priority >= 0)
          {
            num1 = -1;
            continue;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!local2.m_ClipIndex.Equals(clipIndex) && !local2.m_IsLoading && local2.m_IsLoaded)
          {
            // ISSUE: reference to a compiler-generated field
            local2.m_IsLoaded = false;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.UnloadAnimation(local2.m_ClipIndex);
          }
          else
            continue;
        }
        else
        {
          uint num2 = 8388608U / (uint) sizeof (Colossal.Animations.Animation.Element);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_AnimAllocator.Resize(this.m_AnimAllocator.Size + (uint) ((int) num2 + (int) length - 1) / num2 * num2);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        animationClipData.m_AnimAllocation = this.m_AnimAllocator.Allocate(length);
      }
      // ISSUE: reference to a compiler-generated field
      if (componentData.m_RestPoseClipIndex == clipIndex.m_Index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        animationClipData.m_HierarchyAllocation = this.AllocateIndexData((uint) animation.m_BoneHierarchy.hierarchyParentIndices.Length);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.CacheRestPose(clipIndex.m_Style, animation);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      animationClipData.m_ShapeAllocation = this.AllocateIndexData((uint) componentData.m_ShapeCount);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      animationClipData.m_BoneAllocation = this.AllocateIndexData((uint) animation.m_Animation.boneIndices.Length);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      animationClipData.m_InverseBoneAllocation = this.AllocateIndexData((uint) componentData.m_BoneCount);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MaxBoneCount = math.max(this.m_MaxBoneCount, animation.m_BoneHierarchy.hierarchyParentIndices.Length);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MaxActiveBoneCount = math.max(this.m_MaxActiveBoneCount, animation.m_Animation.boneIndices.Length);
      // ISSUE: reference to a compiler-generated field
      if (this.m_FreeAnimIndices.IsEmpty)
      {
        // ISSUE: reference to a compiler-generated field
        local1.m_InfoIndex = this.m_AnimationClipData.Length;
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationClipData.Add(in animationClipData);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        local1.m_InfoIndex = this.m_FreeAnimIndices[this.m_FreeAnimIndices.Length - 1];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_FreeAnimIndices.RemoveAt(this.m_FreeAnimIndices.Length - 1);
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationClipData[local1.m_InfoIndex] = animationClipData;
      }
      // ISSUE: reference to a compiler-generated method
      this.ResizeAnimInfoBuffer();
      // ISSUE: reference to a compiler-generated method
      this.ResizeAnimBuffer();
      // ISSUE: reference to a compiler-generated method
      this.ResizeIndexBuffer();
      NativeArray<AnimationInfoData> data1 = new NativeArray<AnimationInfoData>(1, Allocator.Temp);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      data1[0] = new AnimationInfoData()
      {
        m_Offset = (int) animationClipData.m_AnimAllocation.Begin,
        m_Hierarchy = animationClipData.m_HierarchyAllocation.Empty ? -1 : (int) animationClipData.m_HierarchyAllocation.Begin,
        m_Shapes = (int) animationClipData.m_ShapeAllocation.Begin,
        m_Bones = (int) animationClipData.m_BoneAllocation.Begin,
        m_InverseBones = (int) animationClipData.m_InverseBoneAllocation.Begin,
        m_ShapeCount = animation.m_Animation.shapeIndices.Length,
        m_BoneCount = animation.m_Animation.boneIndices.Length,
        m_Type = (int) animation.m_Animation.type,
        m_PositionMin = animation.m_Animation.positionMin,
        m_PositionRange = animation.m_Animation.positionRange
      };
      // ISSUE: reference to a compiler-generated field
      this.m_AnimInfoBuffer.SetData<AnimationInfoData>(data1, 0, local1.m_InfoIndex, 1);
      data1.Dispose();
      NativeArray<Colossal.Animations.Animation.Element> nativeArray = new NativeArray<Colossal.Animations.Animation.Element>(animation.m_Animation.elements, Allocator.Temp);
      if (local1.m_RootMotionBone != -1)
      {
        NativeArray<AnimationMotion> subArray = buffer2.AsNativeArray().GetSubArray(local1.m_MotionRange.x, local1.m_MotionRange.y - local1.m_MotionRange.x);
        // ISSUE: reference to a compiler-generated method
        this.RemoveRootMotion(animation, local1, buffer3, subArray, nativeArray);
      }
      if (local1.m_Layer == Game.Prefabs.AnimationLayer.Prop && (double) local1.m_TargetValue == -3.4028234663852886E+38)
      {
        // ISSUE: reference to a compiler-generated method
        local1.m_TargetValue = this.FindTargetValue(animation, local1, nativeArray);
        for (int index = 0; index < buffer1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (index != clipIndex.m_Index)
          {
            ref Game.Prefabs.AnimationClip local3 = ref buffer1.ElementAt(index);
            // ISSUE: reference to a compiler-generated field
            if (local3.m_PropClipIndex == clipIndex.m_Index)
              local3.m_TargetValue = local1.m_TargetValue;
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AnimBuffer.SetData<Colossal.Animations.Animation.Element>(nativeArray, 0, (int) animationClipData.m_AnimAllocation.Begin, (int) length);
      nativeArray.Dispose();
      // ISSUE: reference to a compiler-generated field
      if (!animationClipData.m_HierarchyAllocation.Empty)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IndexBuffer.SetData((System.Array) animation.m_BoneHierarchy.hierarchyParentIndices, 0, (int) animationClipData.m_HierarchyAllocation.Begin, animation.m_BoneHierarchy.hierarchyParentIndices.Length);
      }
      // ISSUE: reference to a compiler-generated field
      if (!animationClipData.m_ShapeAllocation.Empty)
      {
        NativeArray<int> data2 = new NativeArray<int>(componentData.m_ShapeCount, Allocator.Temp);
        for (int index = 0; index < data2.Length; ++index)
          data2[index] = -1;
        for (int index = 0; index < animation.m_Animation.shapeIndices.Length; ++index)
          data2[animation.m_Animation.shapeIndices[index]] = index;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IndexBuffer.SetData<int>(data2, 0, (int) animationClipData.m_ShapeAllocation.Begin, data2.Length);
        data2.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!animationClipData.m_BoneAllocation.Empty)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IndexBuffer.SetData((System.Array) animation.m_Animation.boneIndices, 0, (int) animationClipData.m_BoneAllocation.Begin, animation.m_Animation.boneIndices.Length);
      }
      // ISSUE: reference to a compiler-generated field
      if (!animationClipData.m_InverseBoneAllocation.Empty && local1.m_Layer != Game.Prefabs.AnimationLayer.Prop)
      {
        NativeArray<int> data3 = new NativeArray<int>(componentData.m_BoneCount, Allocator.Temp);
        for (int index = 0; index < data3.Length; ++index)
          data3[index] = -1;
        for (int index = 0; index < animation.m_Animation.boneIndices.Length; ++index)
          data3[animation.m_Animation.boneIndices[index]] = index;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IndexBuffer.SetData<int>(data3, 0, (int) animationClipData.m_InverseBoneAllocation.Begin, data3.Length);
        data3.Dispose();
      }
      return true;
    }

    private NativeHeapBlock AllocateIndexData(uint size)
    {
      NativeHeapBlock nativeHeapBlock;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (nativeHeapBlock = this.m_IndexAllocator.Allocate(size); nativeHeapBlock.Empty; nativeHeapBlock = this.m_IndexAllocator.Allocate(size))
      {
        uint num = 4096;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IndexAllocator.Resize(this.m_IndexAllocator.Size + (uint) ((int) num + (int) size - 1) / num * num);
      }
      return nativeHeapBlock;
    }

    private void UnloadAnimation(AnimatedSystem.ClipIndex clipIndex)
    {
      // ISSUE: reference to a compiler-generated field
      CharacterStyleData componentData = this.EntityManager.GetComponentData<CharacterStyleData>(clipIndex.m_Style);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ref Game.Prefabs.AnimationClip local = ref this.EntityManager.GetBuffer<Game.Prefabs.AnimationClip>(clipIndex.m_Style).ElementAt(clipIndex.m_Index);
      if (local.m_InfoIndex < 0)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      AnimatedSystem.AnimationClipData animationClipData = this.m_AnimationClipData[local.m_InfoIndex];
      // ISSUE: reference to a compiler-generated field
      if (componentData.m_RestPoseClipIndex == clipIndex.m_Index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UnCacheRestPose(clipIndex.m_Style);
      }
      // ISSUE: reference to a compiler-generated field
      if (!animationClipData.m_AnimAllocation.Empty)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimAllocator.Release(animationClipData.m_AnimAllocation);
      }
      // ISSUE: reference to a compiler-generated field
      if (!animationClipData.m_HierarchyAllocation.Empty)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IndexAllocator.Release(animationClipData.m_HierarchyAllocation);
      }
      // ISSUE: reference to a compiler-generated field
      if (!animationClipData.m_ShapeAllocation.Empty)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IndexAllocator.Release(animationClipData.m_ShapeAllocation);
      }
      // ISSUE: reference to a compiler-generated field
      if (!animationClipData.m_BoneAllocation.Empty)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IndexAllocator.Release(animationClipData.m_BoneAllocation);
      }
      // ISSUE: reference to a compiler-generated field
      if (!animationClipData.m_InverseBoneAllocation.Empty)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IndexAllocator.Release(animationClipData.m_InverseBoneAllocation);
      }
      // ISSUE: reference to a compiler-generated field
      if (local.m_InfoIndex == this.m_AnimationClipData.Length - 1)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationClipData.RemoveAt(local.m_InfoIndex);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_FreeAnimIndices.Add(in local.m_InfoIndex);
      }
      local.m_InfoIndex = -1;
      // ISSUE: reference to a compiler-generated field
      --this.m_AnimationCount;
    }

    private void CacheRestPose(Entity style, Colossal.Animations.AnimationClip restPose)
    {
      DynamicBuffer<RestPoseElement> buffer = this.EntityManager.GetBuffer<RestPoseElement>(style);
      buffer.ResizeUninitialized(restPose.m_Animation.elements.Length);
      for (int index = 0; index < buffer.Length; ++index)
      {
        Colossal.Animations.Animation.ElementRaw elementRaw = AnimationEncoding.DecodeElement(restPose.m_Animation.elements[index], restPose.m_Animation.positionMin, restPose.m_Animation.positionRange);
        buffer[index] = new RestPoseElement()
        {
          m_Position = elementRaw.position,
          m_Rotation = (quaternion) elementRaw.rotation
        };
      }
    }

    private void UnCacheRestPose(Entity style)
    {
      DynamicBuffer<RestPoseElement> buffer = this.EntityManager.GetBuffer<RestPoseElement>(style);
      buffer.Clear();
      buffer.TrimExcess();
    }

    private float FindTargetValue(
      Colossal.Animations.AnimationClip animation,
      Game.Prefabs.AnimationClip animationClip,
      NativeArray<Colossal.Animations.Animation.Element> elements)
    {
      if (animationClip.m_Activity == ActivityType.Driving)
      {
        switch (animationClip.m_Type)
        {
          case Game.Prefabs.AnimationType.LeftMin:
          case Game.Prefabs.AnimationType.LeftMax:
          case Game.Prefabs.AnimationType.RightMin:
          case Game.Prefabs.AnimationType.RightMax:
            // ISSUE: reference to a compiler-generated method
            return this.FindTargetRotation(animation, elements);
        }
      }
      return 0.0f;
    }

    private float FindTargetRotation(
      Colossal.Animations.AnimationClip animation,
      NativeArray<Colossal.Animations.Animation.Element> elements)
    {
      int length1 = animation.m_Animation.shapeIndices.Length;
      int length2 = animation.m_Animation.boneIndices.Length;
      float x = 0.0f;
      for (int index = 0; index < length2; ++index)
      {
        Colossal.Animations.Animation.ElementRaw elementRaw = AnimationEncoding.DecodeElement(elements.ElementAt<Colossal.Animations.Animation.Element>(index * length1), animation.m_Animation.positionMin, animation.m_Animation.positionRange);
        float y = MathUtils.RotationAngle(quaternion.identity, (quaternion) elementRaw.rotation);
        x = math.max(x, y);
      }
      return x;
    }

    private void RemoveRootMotion(
      Colossal.Animations.AnimationClip animation,
      Game.Prefabs.AnimationClip animationClip,
      DynamicBuffer<RestPoseElement> restPose,
      NativeArray<AnimationMotion> motions,
      NativeArray<Colossal.Animations.Animation.Element> elements)
    {
      int[] inverseBoneIndices = animation.GetInverseBoneIndices();
      int length1 = animation.m_Animation.shapeIndices.Length;
      int length2 = animation.m_Animation.boneIndices.Length;
      int num1 = length1 * length2;
      int num2 = elements.Length / num1 - 1;
      int length3 = inverseBoneIndices.Length;
      int num3 = restPose.Length / length3;
      for (int index1 = 0; index1 <= num2; ++index1)
      {
        float num4 = math.select((float) index1 / (float) (num2 - 1), 0.0f, index1 >= num2);
        for (int index2 = 0; index2 < length1; ++index2)
        {
          int shapeIndex = animation.m_Animation.shapeIndices[index2];
          AnimationMotion motion1 = motions[shapeIndex];
          int num5 = inverseBoneIndices[animationClip.m_RootMotionBone];
          if (num5 >= 0)
          {
            int num6 = index1 * num1 + num5 * length1;
            ref Colossal.Animations.Animation.Element local1 = ref elements.ElementAt<Colossal.Animations.Animation.Element>(num6 + index2);
            Colossal.Animations.Animation.ElementRaw input1 = AnimationEncoding.DecodeElement(local1, animation.m_Animation.positionMin, animation.m_Animation.positionRange);
            quaternion quaternion1 = math.slerp(motion1.m_StartRotation, motion1.m_EndRotation, num4);
            float3 float3_1 = animationClip.m_Playback == AnimationPlayback.Once ? MathUtils.Position(new Bezier4x3(motion1.m_StartOffset, motion1.m_StartOffset, motion1.m_EndOffset, motion1.m_EndOffset), num4) : math.lerp(motion1.m_StartOffset, motion1.m_EndOffset, num4);
            if (shapeIndex != 0)
            {
              AnimationMotion motion2 = motions[0];
              quaternion b = math.slerp(motion2.m_StartRotation, motion2.m_EndRotation, num4);
              float3 float3_2 = animationClip.m_Playback == AnimationPlayback.Once ? MathUtils.Position(new Bezier4x3(motion2.m_StartOffset, motion2.m_StartOffset, motion2.m_EndOffset, motion2.m_EndOffset), num4) : math.lerp(motion2.m_StartOffset, motion2.m_EndOffset, num4);
              float3_1 += float3_2;
              quaternion1 = math.mul(quaternion1, b);
            }
            for (int hierarchyParentIndex = animation.m_BoneHierarchy.hierarchyParentIndices[animationClip.m_RootMotionBone]; hierarchyParentIndex != -1; hierarchyParentIndex = animation.m_BoneHierarchy.hierarchyParentIndices[hierarchyParentIndex])
            {
              int num7 = inverseBoneIndices[hierarchyParentIndex];
              if (num7 >= 0)
              {
                int num8 = index1 * num1 + num7 * length1;
                ref Colossal.Animations.Animation.Element local2 = ref elements.ElementAt<Colossal.Animations.Animation.Element>(num8 + index2);
                Colossal.Animations.Animation.ElementRaw input2 = AnimationEncoding.DecodeElement(local2, animation.m_Animation.positionMin, animation.m_Animation.positionRange);
                input1.position = input2.position + math.mul((quaternion) input2.rotation, input1.position);
                input1.rotation = math.mul((quaternion) input2.rotation, (quaternion) input1.rotation).value;
                local1 = AnimationEncoding.EncodeElement(input1, animation.m_Animation.positionMin, animation.m_Animation.positionRange);
                input2.position = float3.zero;
                input2.rotation = quaternion.identity.value;
                local2 = AnimationEncoding.EncodeElement(input2, animation.m_Animation.positionMin, animation.m_Animation.positionRange);
              }
              else
              {
                int num9 = hierarchyParentIndex * num3;
                RestPoseElement restPoseElement = restPose[num9 + shapeIndex];
                restPoseElement.m_Rotation = math.inverse(restPoseElement.m_Rotation);
                float3_1 = math.mul(restPoseElement.m_Rotation, float3_1 - restPoseElement.m_Position);
                quaternion1 = math.normalize(math.mul(restPoseElement.m_Rotation, quaternion1));
              }
            }
            quaternion quaternion2 = math.inverse(quaternion1);
            input1.position = math.mul(quaternion2, input1.position - float3_1);
            input1.rotation = math.normalize(math.mul(quaternion2, (quaternion) input1.rotation)).value;
            local1 = AnimationEncoding.EncodeElement(input1, animation.m_Animation.positionMin, animation.m_Animation.positionRange);
          }
        }
      }
    }

    private void UpdateMetaData()
    {
      int index = 0;
      // ISSUE: reference to a compiler-generated field
      while (index < this.m_UpdatedMetaIndices.Length)
      {
        // ISSUE: reference to a compiler-generated field
        int updatedMetaIndex1 = this.m_UpdatedMetaIndices[index++];
        int num = updatedMetaIndex1 + 1;
        // ISSUE: reference to a compiler-generated field
        while (index < this.m_UpdatedMetaIndices.Length)
        {
          // ISSUE: reference to a compiler-generated field
          int updatedMetaIndex2 = this.m_UpdatedMetaIndices[index];
          if (updatedMetaIndex2 == num)
          {
            ++index;
            num = updatedMetaIndex2 + 1;
          }
          else
            break;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_MetaBuffer.SetData<MetaBufferData>(this.m_MetaBufferData.AsArray(), updatedMetaIndex1, updatedMetaIndex1, num - updatedMetaIndex1);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedMetaIndices.Clear();
    }

    private void ResizeBoneBuffer()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int count = this.m_BoneBuffer != null ? this.m_BoneBuffer.count : 0;
      // ISSUE: reference to a compiler-generated field
      int size = (int) this.m_BoneAllocator.Size;
      int num = size;
      if (count == num)
        return;
      ComputeBuffer computeBuffer = new ComputeBuffer(size, sizeof (BoneElement), ComputeBufferType.Structured);
      computeBuffer.name = "Bone buffer";
      Shader.SetGlobalBuffer("boneBuffer", computeBuffer);
      // ISSUE: reference to a compiler-generated field
      if (this.m_BoneHistoryBuffer == null)
        Shader.SetGlobalBuffer("boneHistoryBuffer", computeBuffer);
      // ISSUE: reference to a compiler-generated field
      if (this.m_BoneBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BoneBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_LocalTRSBlendPoseBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LocalTRSBlendPoseBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_LocalTRSBoneBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LocalTRSBoneBuffer.Release();
      }
      BoneElement[] data = new BoneElement[computeBuffer.count];
      for (int index = 0; index < data.Length; ++index)
        data[index] = new BoneElement()
        {
          m_Matrix = float4x4.identity
        };
      computeBuffer.SetData((System.Array) data);
      // ISSUE: reference to a compiler-generated field
      this.m_BoneBuffer = computeBuffer;
      // ISSUE: reference to a compiler-generated field
      this.m_LocalTRSBlendPoseBuffer = new ComputeBuffer(size, sizeof (BoneElement), ComputeBufferType.Structured);
      // ISSUE: reference to a compiler-generated field
      this.m_LocalTRSBoneBuffer = new ComputeBuffer(size, sizeof (BoneElement), ComputeBufferType.Structured);
      // ISSUE: reference to a compiler-generated field
      this.m_LocalTRSBlendPoseBuffer.name = "LocalTRSBlendPoseBuffer";
      // ISSUE: reference to a compiler-generated field
      this.m_LocalTRSBoneBuffer.name = "LocalTRSBoneBuffer";
    }

    private void ResizeBoneHistoryBuffer()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int count = this.m_BoneHistoryBuffer != null ? this.m_BoneHistoryBuffer.count : 0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int size = this.m_RenderingSystem.motionVectors ? (int) this.m_BoneAllocator.Size : 0;
      int num = size;
      if (count == num)
        return;
      if (size == 0)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_BoneHistoryBuffer != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_BoneHistoryBuffer != null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_BoneHistoryBuffer.Release();
          }
          // ISSUE: reference to a compiler-generated field
          this.m_BoneHistoryBuffer = (ComputeBuffer) null;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_BoneBuffer == null)
          return;
        // ISSUE: reference to a compiler-generated field
        Shader.SetGlobalBuffer("boneHistoryBuffer", this.m_BoneBuffer);
      }
      else
      {
        ComputeBuffer computeBuffer = new ComputeBuffer(size, sizeof (BoneElement), ComputeBufferType.Structured);
        computeBuffer.name = "Bone history buffer";
        Shader.SetGlobalBuffer("boneHistoryBuffer", computeBuffer);
        // ISSUE: reference to a compiler-generated field
        if (this.m_BoneHistoryBuffer != null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_BoneHistoryBuffer.Release();
        }
        BoneElement[] data = new BoneElement[computeBuffer.count];
        for (int index = 0; index < data.Length; ++index)
          data[index] = new BoneElement()
          {
            m_Matrix = float4x4.identity
          };
        computeBuffer.SetData((System.Array) data);
        // ISSUE: reference to a compiler-generated field
        this.m_BoneHistoryBuffer = computeBuffer;
      }
    }

    private void ResizeAnimInfoBuffer()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int count1 = this.m_AnimInfoBuffer != null ? this.m_AnimInfoBuffer.count : 0;
      // ISSUE: reference to a compiler-generated field
      int capacity = this.m_AnimationClipData.Capacity;
      if (count1 == capacity)
        return;
      ComputeBuffer computeBuffer = new ComputeBuffer(capacity, sizeof (AnimationInfoData), ComputeBufferType.Structured);
      computeBuffer.name = "Animation info buffer";
      int count2 = math.min(count1, capacity);
      if (count2 > 0)
      {
        AnimationInfoData[] data = new AnimationInfoData[count2];
        // ISSUE: reference to a compiler-generated field
        this.m_AnimInfoBuffer.GetData((System.Array) data, 0, 0, count2);
        computeBuffer.SetData((System.Array) data);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_AnimInfoBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AnimInfoBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_AnimInfoBuffer = computeBuffer;
    }

    private void ResizeAnimBuffer()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int count1 = this.m_AnimBuffer != null ? this.m_AnimBuffer.count : 0;
      // ISSUE: reference to a compiler-generated field
      int size = (int) this.m_AnimAllocator.Size;
      if (count1 == size)
        return;
      ComputeBuffer computeBuffer = new ComputeBuffer(size, sizeof (Colossal.Animations.Animation.Element), ComputeBufferType.Structured);
      computeBuffer.name = "Animation buffer";
      int count2 = math.min(count1, size);
      if (count2 > 0)
      {
        Colossal.Animations.Animation.Element[] data = new Colossal.Animations.Animation.Element[count2];
        // ISSUE: reference to a compiler-generated field
        this.m_AnimBuffer.GetData((System.Array) data, 0, 0, count2);
        computeBuffer.SetData((System.Array) data);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_AnimBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AnimBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_AnimBuffer = computeBuffer;
    }

    private void ResizeMetaBuffer()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int count1 = this.m_MetaBuffer != null ? this.m_MetaBuffer.count : 0;
      int count2 = 1048576 / sizeof (MetaBufferData);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_MetaBufferData.Length > count1 && this.m_MetaBufferData.Length > count2)
      {
        // ISSUE: reference to a compiler-generated field
        count2 += ((this.m_MetaBufferData.Length - count2) * sizeof (MetaBufferData) + 262144 - 1) / 262144 * 262144 / sizeof (MetaBufferData);
      }
      else if (count1 > count2)
        count2 = count1;
      if (count1 == count2)
        return;
      ComputeBuffer computeBuffer = new ComputeBuffer(count2, sizeof (MetaBufferData), ComputeBufferType.Structured);
      computeBuffer.name = "Meta buffer";
      Shader.SetGlobalBuffer("metaBuffer", computeBuffer);
      // ISSUE: reference to a compiler-generated field
      if (this.m_MetaBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        computeBuffer.SetData<MetaBufferData>(this.m_MetaBufferData.AsArray(), 0, 0, count1);
        // ISSUE: reference to a compiler-generated field
        this.m_MetaBuffer.Release();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        computeBuffer.SetData<MetaBufferData>(this.m_MetaBufferData.AsArray(), 0, 0, 1);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_MetaBuffer = computeBuffer;
    }

    private void ResizeIndexBuffer()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int count1 = this.m_IndexBuffer != null ? this.m_IndexBuffer.count : 0;
      // ISSUE: reference to a compiler-generated field
      int size = (int) this.m_IndexAllocator.Size;
      if (count1 == size)
        return;
      ComputeBuffer computeBuffer = new ComputeBuffer(size, 4, ComputeBufferType.Structured);
      computeBuffer.name = "Index buffer";
      int count2 = math.min(count1, size);
      if (count2 > 0)
      {
        int[] data = new int[count2];
        // ISSUE: reference to a compiler-generated field
        this.m_IndexBuffer.GetData((System.Array) data, 0, 0, count2);
        computeBuffer.SetData((System.Array) data);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_IndexBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_IndexBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_IndexBuffer = computeBuffer;
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_IsAllocating)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AllocateDeps.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_IsAllocating = false;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_BoneAllocator.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_MetaBufferData.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_FreeMetaIndices.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedMetaIndices.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_InstanceIndices.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_BodyInstances.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_FaceInstances.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_CorrectiveInstances.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_BodyTransitions.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_FaceTransitions.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_BoneAllocationRemoves.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_MetaBufferRemoves.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_BoneAllocator.Allocate(1U);
      // ISSUE: reference to a compiler-generated field
      this.m_MetaBufferData.Add(new MetaBufferData());
    }

    public AnimatedSystem.AllocationData GetAllocationData(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_AllocateDeps;
      // ISSUE: reference to a compiler-generated field
      this.m_IsAllocating = true;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      return new AnimatedSystem.AllocationData(this.m_BoneAllocator, this.m_MetaBufferData, this.m_FreeMetaIndices, this.m_UpdatedMetaIndices, this.m_BoneAllocationRemoves, this.m_MetaBufferRemoves, this.m_CurrentTime);
    }

    public AnimatedSystem.AnimationData GetAnimationData(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_AllocateDeps;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_TempAnimationQueue.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TempAnimationQueue = new NativeQueue<AnimatedSystem.AnimationFrameData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_TempPriorityQueue.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TempPriorityQueue = new NativeQueue<AnimatedSystem.ClipPriorityData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_IsAllocating = true;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      return new AnimatedSystem.AnimationData(this.m_TempAnimationQueue, this.m_TempPriorityQueue);
    }

    public void AddAllocationWriter(JobHandle handle) => this.m_AllocateDeps = handle;

    public void AddAnimationWriter(JobHandle handle) => this.m_AllocateDeps = handle;

    public AnimatedPropID GetPropID(string name)
    {
      int index = -1;
      // ISSUE: reference to a compiler-generated field
      if (!string.IsNullOrEmpty(name) && !this.m_PropIDs.TryGetValue(name, out index))
      {
        // ISSUE: reference to a compiler-generated field
        index = this.m_PropIDs.Count;
        // ISSUE: reference to a compiler-generated field
        this.m_PropIDs.Add(name, index);
      }
      return new AnimatedPropID(index);
    }

    public void GetBoneStats(out uint allocatedSize, out uint bufferSize, out uint count)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_AllocateDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      allocatedSize = this.m_BoneAllocator.UsedSpace * (uint) sizeof (BoneElement);
      // ISSUE: reference to a compiler-generated field
      bufferSize = this.m_BoneAllocator.Size * (uint) sizeof (BoneElement);
      // ISSUE: reference to a compiler-generated field
      if (this.m_RenderingSystem.motionVectors)
      {
        allocatedSize <<= 1;
        bufferSize <<= 1;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      count = (uint) (this.m_MetaBufferData.Length - this.m_FreeMetaIndices.Length - 1);
    }

    public void GetAnimStats(out uint allocatedSize, out uint bufferSize, out uint count)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_AllocateDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      allocatedSize = this.m_AnimAllocator.UsedSpace * (uint) sizeof (Colossal.Animations.Animation.Element);
      // ISSUE: reference to a compiler-generated field
      bufferSize = this.m_AnimAllocator.Size * (uint) sizeof (Colossal.Animations.Animation.Element);
      // ISSUE: reference to a compiler-generated field
      count = (uint) this.m_AnimationCount;
    }

    public void GetIndexStats(out uint allocatedSize, out uint bufferSize, out uint count)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_AllocateDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      allocatedSize = this.m_IndexAllocator.UsedSpace * 4U;
      // ISSUE: reference to a compiler-generated field
      bufferSize = this.m_IndexAllocator.Size * 4U;
      // ISSUE: reference to a compiler-generated field
      count = (uint) this.m_AnimationCount;
    }

    public void GetMetaStats(out uint allocatedSize, out uint bufferSize, out uint count)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_AllocateDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      count = (uint) (this.m_MetaBufferData.Length - this.m_FreeMetaIndices.Length - 1);
      // ISSUE: reference to a compiler-generated field
      if (this.m_MetaBuffer != null)
      {
        allocatedSize = (uint) (((int) count + 1) * sizeof (MetaBufferData));
        // ISSUE: reference to a compiler-generated field
        bufferSize = (uint) (this.m_MetaBuffer.count * sizeof (MetaBufferData));
      }
      else
      {
        allocatedSize = 0U;
        bufferSize = 0U;
      }
    }

    [UnityEngine.Scripting.Preserve]
    public AnimatedSystem()
    {
    }

    public class Prepare : GameSystemBase
    {
      private AnimatedSystem m_AnimatedSystem;

      [UnityEngine.Scripting.Preserve]
      protected override void OnCreate()
      {
        base.OnCreate();
        // ISSUE: reference to a compiler-generated field
        this.m_AnimatedSystem = this.World.GetOrCreateSystemManaged<AnimatedSystem>();
      }

      [UnityEngine.Scripting.Preserve]
      protected override void OnUpdate()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimatedSystem.m_CurrentTime = this.m_AnimatedSystem.m_CurrentTime + this.m_AnimatedSystem.m_RenderingSystem.lodTimerDelta & (int) ushort.MaxValue;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        JobHandle allocateDeps = this.m_AnimatedSystem.m_AllocateDeps;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_AnimatedSystem.m_IsAllocating)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_AnimatedSystem.m_AllocateDeps = new AnimatedSystem.EndAllocationJob()
          {
            m_BoneAllocator = this.m_AnimatedSystem.m_BoneAllocator,
            m_MetaBufferData = this.m_AnimatedSystem.m_MetaBufferData,
            m_FreeMetaIndices = this.m_AnimatedSystem.m_FreeMetaIndices,
            m_UpdatedMetaIndices = this.m_AnimatedSystem.m_UpdatedMetaIndices,
            m_BoneAllocationRemoves = this.m_AnimatedSystem.m_BoneAllocationRemoves,
            m_MetaBufferRemoves = this.m_AnimatedSystem.m_MetaBufferRemoves,
            m_CurrentTime = this.m_AnimatedSystem.m_CurrentTime
          }.Schedule<AnimatedSystem.EndAllocationJob>(allocateDeps);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_AnimatedSystem.m_TempAnimationQueue.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          JobHandle jobHandle = new AnimatedSystem.AddAnimationInstancesJob()
          {
            m_AnimationFrameData = this.m_AnimatedSystem.m_TempAnimationQueue,
            m_InstanceIndices = this.m_AnimatedSystem.m_InstanceIndices,
            m_BodyInstances = this.m_AnimatedSystem.m_BodyInstances,
            m_FaceInstances = this.m_AnimatedSystem.m_FaceInstances,
            m_CorrectiveIndices = this.m_AnimatedSystem.m_CorrectiveInstances,
            m_BodyTransitions = this.m_AnimatedSystem.m_BodyTransitions,
            m_BodyTransitions2 = this.m_AnimatedSystem.m_BodyTransitions2,
            m_FaceTransitions = this.m_AnimatedSystem.m_FaceTransitions
          }.Schedule<AnimatedSystem.AddAnimationInstancesJob>(allocateDeps);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_AnimatedSystem.m_AllocateDeps = JobHandle.CombineDependencies(this.m_AnimatedSystem.m_AllocateDeps, jobHandle);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_AnimatedSystem.m_TempAnimationQueue.Dispose(jobHandle);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_AnimatedSystem.m_TempPriorityQueue.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        JobHandle jobHandle1 = new AnimatedSystem.UpdateAnimationPriorityJob()
        {
          m_ClipPriorityData = this.m_AnimatedSystem.m_TempPriorityQueue,
          m_ClipPriorities = this.m_AnimatedSystem.m_ClipPriorities
        }.Schedule<AnimatedSystem.UpdateAnimationPriorityJob>(allocateDeps);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimatedSystem.m_AllocateDeps = JobHandle.CombineDependencies(this.m_AnimatedSystem.m_AllocateDeps, jobHandle1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AnimatedSystem.m_TempPriorityQueue.Dispose(jobHandle1);
      }

      [UnityEngine.Scripting.Preserve]
      public Prepare()
      {
      }
    }

    [BurstCompile]
    private struct AddAnimationInstancesJob : IJob
    {
      public NativeQueue<AnimatedSystem.AnimationFrameData> m_AnimationFrameData;
      public NativeList<RestPoseInstance> m_InstanceIndices;
      public NativeList<AnimatedInstance> m_BodyInstances;
      public NativeList<AnimatedInstance> m_FaceInstances;
      public NativeList<AnimatedInstance> m_CorrectiveIndices;
      public NativeList<AnimatedTransition> m_BodyTransitions;
      public NativeList<AnimatedTransition2> m_BodyTransitions2;
      public NativeList<AnimatedTransition> m_FaceTransitions;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_InstanceIndices.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_CorrectiveIndices.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_BodyInstances.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_FaceInstances.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_BodyTransitions.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_BodyTransitions2.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_FaceTransitions.Clear();
        // ISSUE: variable of a compiler-generated type
        AnimatedSystem.AnimationFrameData animationFrameData;
        // ISSUE: reference to a compiler-generated field
        while (this.m_AnimationFrameData.TryDequeue(out animationFrameData))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_InstanceIndices.Add(new RestPoseInstance()
          {
            m_MetaIndex = animationFrameData.m_MetaIndex,
            m_RestPoseIndex = animationFrameData.m_RestPoseIndex,
            m_ResetHistory = animationFrameData.m_ResetHistory
          });
          AnimatedInstance animatedInstance;
          // ISSUE: reference to a compiler-generated field
          if (animationFrameData.m_CorrectiveIndex >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            ref NativeList<AnimatedInstance> local1 = ref this.m_CorrectiveIndices;
            animatedInstance = new AnimatedInstance();
            // ISSUE: reference to a compiler-generated field
            animatedInstance.m_MetaIndex = animationFrameData.m_MetaIndex;
            // ISSUE: reference to a compiler-generated field
            animatedInstance.m_CurrentIndex = animationFrameData.m_CorrectiveIndex;
            ref AnimatedInstance local2 = ref animatedInstance;
            local1.Add(in local2);
          }
          AnimatedTransition animatedTransition;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (animationFrameData.m_BodyData.m_CurrentIndex >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (animationFrameData.m_BodyData.m_TransitionIndex.x >= 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (animationFrameData.m_BodyData.m_TransitionIndex.y >= 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_BodyTransitions2.Add(new AnimatedTransition2()
                {
                  m_MetaIndex = animationFrameData.m_MetaIndex,
                  m_CurrentIndex = animationFrameData.m_BodyData.m_CurrentIndex,
                  m_CurrentFrame = animationFrameData.m_BodyData.m_CurrentFrame,
                  m_TransitionIndex = animationFrameData.m_BodyData.m_TransitionIndex,
                  m_TransitionFrame = animationFrameData.m_BodyData.m_TransitionFrame,
                  m_TransitionWeight = animationFrameData.m_BodyData.m_TransitionWeight
                });
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                ref NativeList<AnimatedTransition> local3 = ref this.m_BodyTransitions;
                animatedTransition = new AnimatedTransition();
                // ISSUE: reference to a compiler-generated field
                animatedTransition.m_MetaIndex = animationFrameData.m_MetaIndex;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                animatedTransition.m_CurrentIndex = animationFrameData.m_BodyData.m_CurrentIndex;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                animatedTransition.m_TransitionIndex = animationFrameData.m_BodyData.m_TransitionIndex.x;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                animatedTransition.m_CurrentFrame = animationFrameData.m_BodyData.m_CurrentFrame;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                animatedTransition.m_TransitionFrame = animationFrameData.m_BodyData.m_TransitionFrame.x;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                animatedTransition.m_TransitionWeight = animationFrameData.m_BodyData.m_TransitionWeight.x;
                ref AnimatedTransition local4 = ref animatedTransition;
                local3.Add(in local4);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeList<AnimatedInstance> local5 = ref this.m_BodyInstances;
              animatedInstance = new AnimatedInstance();
              // ISSUE: reference to a compiler-generated field
              animatedInstance.m_MetaIndex = animationFrameData.m_MetaIndex;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              animatedInstance.m_CurrentIndex = animationFrameData.m_BodyData.m_CurrentIndex;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              animatedInstance.m_CurrentFrame = animationFrameData.m_BodyData.m_CurrentFrame;
              ref AnimatedInstance local6 = ref animatedInstance;
              local5.Add(in local6);
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (animationFrameData.m_FaceData.m_CurrentIndex >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (animationFrameData.m_FaceData.m_TransitionIndex >= 0)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeList<AnimatedTransition> local7 = ref this.m_FaceTransitions;
              animatedTransition = new AnimatedTransition();
              // ISSUE: reference to a compiler-generated field
              animatedTransition.m_MetaIndex = animationFrameData.m_MetaIndex;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              animatedTransition.m_CurrentIndex = animationFrameData.m_FaceData.m_CurrentIndex;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              animatedTransition.m_TransitionIndex = animationFrameData.m_FaceData.m_TransitionIndex;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              animatedTransition.m_CurrentFrame = animationFrameData.m_FaceData.m_CurrentFrame;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              animatedTransition.m_TransitionFrame = animationFrameData.m_FaceData.m_TransitionFrame;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              animatedTransition.m_TransitionWeight = animationFrameData.m_FaceData.m_TransitionWeight;
              ref AnimatedTransition local8 = ref animatedTransition;
              local7.Add(in local8);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeList<AnimatedInstance> local9 = ref this.m_FaceInstances;
              animatedInstance = new AnimatedInstance();
              // ISSUE: reference to a compiler-generated field
              animatedInstance.m_MetaIndex = animationFrameData.m_MetaIndex;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              animatedInstance.m_CurrentIndex = animationFrameData.m_FaceData.m_CurrentIndex;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              animatedInstance.m_CurrentFrame = animationFrameData.m_FaceData.m_CurrentFrame;
              ref AnimatedInstance local10 = ref animatedInstance;
              local9.Add(in local10);
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct UpdateAnimationPriorityJob : IJob
    {
      public NativeQueue<AnimatedSystem.ClipPriorityData> m_ClipPriorityData;
      public NativeList<AnimatedSystem.ClipPriorityData> m_ClipPriorities;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeHashMap<AnimatedSystem.ClipIndex, int> nativeHashMap = new NativeHashMap<AnimatedSystem.ClipIndex, int>(this.m_ClipPriorities.Length + 10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_ClipPriorities.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          ref AnimatedSystem.ClipPriorityData local = ref this.m_ClipPriorities.ElementAt(index);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          local.m_Priority = math.max(local.m_Priority - 1, -1000000);
          // ISSUE: reference to a compiler-generated field
          nativeHashMap.Add(local.m_ClipIndex, index);
        }
        // ISSUE: variable of a compiler-generated type
        AnimatedSystem.ClipPriorityData clipPriorityData;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ClipPriorityData.TryDequeue(out clipPriorityData))
        {
          int index;
          // ISSUE: reference to a compiler-generated field
          if (nativeHashMap.TryGetValue(clipPriorityData.m_ClipIndex, out index))
          {
            // ISSUE: reference to a compiler-generated field
            ref AnimatedSystem.ClipPriorityData local = ref this.m_ClipPriorities.ElementAt(index);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            local.m_Priority = math.max(local.m_Priority, clipPriorityData.m_Priority);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            nativeHashMap.Add(clipPriorityData.m_ClipIndex, this.m_ClipPriorities.Length);
            // ISSUE: reference to a compiler-generated field
            this.m_ClipPriorities.Add(in clipPriorityData);
          }
        }
        nativeHashMap.Dispose();
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_ClipPriorities.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          AnimatedSystem.ClipPriorityData clipPriority = this.m_ClipPriorities[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (clipPriority.m_Priority < 0 && !clipPriority.m_IsLoading && !clipPriority.m_IsLoaded)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ClipPriorities.RemoveAtSwapBack(index--);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ClipPriorities.Sort<AnimatedSystem.ClipPriorityData>();
      }
    }

    public struct AnimationData
    {
      private NativeQueue<AnimatedSystem.AnimationFrameData>.ParallelWriter m_AnimationFrameData;
      private NativeQueue<AnimatedSystem.ClipPriorityData>.ParallelWriter m_ClipPriorityData;

      public AnimationData(
        NativeQueue<AnimatedSystem.AnimationFrameData> animationFrameData,
        NativeQueue<AnimatedSystem.ClipPriorityData> clipPriorityData)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationFrameData = animationFrameData.AsParallelWriter();
        // ISSUE: reference to a compiler-generated field
        this.m_ClipPriorityData = clipPriorityData.AsParallelWriter();
      }

      public void SetAnimationFrame(
        in CharacterElement characterElement,
        DynamicBuffer<Game.Prefabs.AnimationClip> clips,
        in Animated animated,
        float2 transition,
        int priority,
        bool reset)
      {
        // ISSUE: reference to a compiler-generated method
        Game.Prefabs.AnimationClip clip1 = this.GetClip(characterElement.m_Style, clips, characterElement.m_RestPoseClipIndex, priority + 2);
        // ISSUE: reference to a compiler-generated method
        Game.Prefabs.AnimationClip clip2 = this.GetClip(characterElement.m_Style, clips, (int) animated.m_ClipIndexBody0, priority + 1);
        // ISSUE: reference to a compiler-generated method
        Game.Prefabs.AnimationClip clip3 = this.GetClip(characterElement.m_Style, clips, (int) animated.m_ClipIndexBody0I, priority + 1);
        // ISSUE: reference to a compiler-generated method
        Game.Prefabs.AnimationClip clip4 = this.GetClip(characterElement.m_Style, clips, (int) animated.m_ClipIndexBody1, priority + 1);
        // ISSUE: reference to a compiler-generated method
        Game.Prefabs.AnimationClip clip5 = this.GetClip(characterElement.m_Style, clips, (int) animated.m_ClipIndexBody1I, priority + 1);
        // ISSUE: reference to a compiler-generated method
        Game.Prefabs.AnimationClip clip6 = this.GetClip(characterElement.m_Style, clips, (int) animated.m_ClipIndexFace0, priority);
        // ISSUE: reference to a compiler-generated method
        Game.Prefabs.AnimationClip clip7 = this.GetClip(characterElement.m_Style, clips, (int) animated.m_ClipIndexFace1, priority);
        // ISSUE: reference to a compiler-generated method
        Game.Prefabs.AnimationClip clip8 = this.GetClip(characterElement.m_Style, clips, characterElement.m_CorrectiveClipIndex, priority);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AnimatedSystem.AnimationFrameData animationFrameData = new AnimatedSystem.AnimationFrameData()
        {
          m_MetaIndex = animated.m_MetaIndex,
          m_RestPoseIndex = clip1.m_InfoIndex,
          m_ResetHistory = math.select(0, 1, reset),
          m_CorrectiveIndex = clip8.m_InfoIndex,
          m_BodyData = new AnimatedSystem.AnimationLayerData2()
          {
            m_CurrentIndex = -1,
            m_TransitionIndex = (int2) -1
          },
          m_FaceData = new AnimatedSystem.AnimationLayerData()
          {
            m_CurrentIndex = -1,
            m_TransitionIndex = -1
          }
        };
        if (clip2.m_InfoIndex < 0)
        {
          if (!reset)
            return;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          ref AnimatedSystem.AnimationLayerData2 local1 = ref animationFrameData.m_BodyData;
          ref Game.Prefabs.AnimationClip local2 = ref clip2;
          ref Game.Prefabs.AnimationClip local3 = ref clip3;
          ref Game.Prefabs.AnimationClip local4 = ref clip4;
          ref Game.Prefabs.AnimationClip local5 = ref clip5;
          float4 time = animated.m_Time;
          float2 xy = time.xy;
          float2 interpolation = (float2) animated.m_Interpolation;
          double x = (double) transition.x;
          // ISSUE: reference to a compiler-generated method
          this.SetLayerData(ref local1, in local2, in local3, in local4, in local5, xy, interpolation, (float) x);
          if (clip6.m_InfoIndex >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            ref AnimatedSystem.AnimationLayerData local6 = ref animationFrameData.m_FaceData;
            ref Game.Prefabs.AnimationClip local7 = ref clip6;
            ref Game.Prefabs.AnimationClip local8 = ref clip7;
            time = animated.m_Time;
            float2 zw = time.zw;
            double y = (double) transition.y;
            // ISSUE: reference to a compiler-generated method
            this.SetLayerData(ref local6, in local7, in local8, zw, (float) y);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_AnimationFrameData.Enqueue(animationFrameData);
      }

      private Game.Prefabs.AnimationClip GetClip(
        Entity style,
        DynamicBuffer<Game.Prefabs.AnimationClip> clips,
        int index,
        int priority)
      {
        if (index != -1)
        {
          Game.Prefabs.AnimationClip clip = clips[index];
          if (priority >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            ref NativeQueue<AnimatedSystem.ClipPriorityData>.ParallelWriter local1 = ref this.m_ClipPriorityData;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            AnimatedSystem.ClipPriorityData clipPriorityData1 = new AnimatedSystem.ClipPriorityData();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            clipPriorityData1.m_ClipIndex = new AnimatedSystem.ClipIndex(style, index);
            // ISSUE: reference to a compiler-generated field
            clipPriorityData1.m_Priority = priority;
            // ISSUE: variable of a compiler-generated type
            AnimatedSystem.ClipPriorityData clipPriorityData2 = clipPriorityData1;
            local1.Enqueue(clipPriorityData2);
            if (clip.m_PropClipIndex >= 0 && (double) clip.m_TargetValue == -3.4028234663852886E+38)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeQueue<AnimatedSystem.ClipPriorityData>.ParallelWriter local2 = ref this.m_ClipPriorityData;
              // ISSUE: object of a compiler-generated type is created
              clipPriorityData1 = new AnimatedSystem.ClipPriorityData();
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              clipPriorityData1.m_ClipIndex = new AnimatedSystem.ClipIndex(style, clip.m_PropClipIndex);
              // ISSUE: reference to a compiler-generated field
              clipPriorityData1.m_Priority = priority;
              // ISSUE: variable of a compiler-generated type
              AnimatedSystem.ClipPriorityData clipPriorityData3 = clipPriorityData1;
              local2.Enqueue(clipPriorityData3);
            }
          }
          return clip;
        }
        return new Game.Prefabs.AnimationClip() { m_InfoIndex = -1 };
      }

      private void SetLayerData(
        ref AnimatedSystem.AnimationLayerData2 layerData,
        in Game.Prefabs.AnimationClip clipData0,
        in Game.Prefabs.AnimationClip clipData0I,
        in Game.Prefabs.AnimationClip clipData1,
        in Game.Prefabs.AnimationClip clipData1I,
        float2 time,
        float2 interpolation,
        float transition)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.SetLayerData(out layerData.m_CurrentIndex, out layerData.m_CurrentFrame, in clipData0, time.x);
        if (clipData1.m_InfoIndex >= 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.SetLayerData(out layerData.m_TransitionIndex.x, out layerData.m_TransitionFrame.x, in clipData1, time.y);
          // ISSUE: reference to a compiler-generated field
          layerData.m_TransitionWeight.x = transition;
          if (clipData0I.m_InfoIndex >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.SetLayerData(out layerData.m_TransitionIndex.y, out layerData.m_TransitionFrame.y, in clipData0I, time.x);
            if (clipData1I.m_InfoIndex == clipData0I.m_InfoIndex)
            {
              // ISSUE: reference to a compiler-generated field
              layerData.m_TransitionWeight.y = math.csum(interpolation) * 0.5f;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              layerData.m_TransitionWeight.y = interpolation.x * (1f - transition);
            }
          }
          else
          {
            if (clipData1I.m_InfoIndex < 0)
              return;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.SetLayerData(out layerData.m_TransitionIndex.y, out layerData.m_TransitionFrame.y, in clipData1I, time.y);
            // ISSUE: reference to a compiler-generated field
            layerData.m_TransitionWeight.y = interpolation.y * transition;
          }
        }
        else
        {
          if (clipData0I.m_InfoIndex < 0)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.SetLayerData(out layerData.m_TransitionIndex.x, out layerData.m_TransitionFrame.x, in clipData0I, time.x);
          // ISSUE: reference to a compiler-generated field
          layerData.m_TransitionWeight.x = interpolation.x;
        }
      }

      private void SetLayerData(
        ref AnimatedSystem.AnimationLayerData layerData,
        in Game.Prefabs.AnimationClip clipData0,
        in Game.Prefabs.AnimationClip clipData1,
        float2 time,
        float transition)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.SetLayerData(out layerData.m_CurrentIndex, out layerData.m_CurrentFrame, in clipData0, time.x);
        if (clipData1.m_InfoIndex < 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.SetLayerData(out layerData.m_TransitionIndex, out layerData.m_TransitionFrame, in clipData1, time.y);
        // ISSUE: reference to a compiler-generated field
        layerData.m_TransitionWeight = transition;
      }

      private void SetLayerData(
        out int index,
        out float frame,
        in Game.Prefabs.AnimationClip clipData,
        float time)
      {
        if (clipData.m_Playback != AnimationPlayback.Once && clipData.m_Playback != AnimationPlayback.OptionalOnce)
        {
          frame = math.fmod(time, clipData.m_AnimationLength);
          frame += math.select(0.0f, clipData.m_AnimationLength, (double) frame < 0.0);
        }
        else
          frame = math.clamp(time, 0.0f, clipData.m_AnimationLength);
        index = clipData.m_InfoIndex;
        frame *= clipData.m_FrameRate;
      }
    }

    [BurstCompile]
    private struct EndAllocationJob : IJob
    {
      public NativeHeapAllocator m_BoneAllocator;
      public NativeList<MetaBufferData> m_MetaBufferData;
      public NativeList<int> m_UpdatedMetaIndices;
      public NativeList<int> m_FreeMetaIndices;
      public NativeQueue<AnimatedSystem.AllocationRemove> m_BoneAllocationRemoves;
      public NativeQueue<AnimatedSystem.IndexRemove> m_MetaBufferRemoves;
      public int m_CurrentTime;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_UpdatedMetaIndices.Length >= 2)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UpdatedMetaIndices.Sort<int>();
        }
        // ISSUE: reference to a compiler-generated field
        while (!this.m_BoneAllocationRemoves.IsEmpty())
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          AnimatedSystem.AllocationRemove allocationRemove = this.m_BoneAllocationRemoves.Peek();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (!this.CheckTimeOffset(allocationRemove.m_RemoveTime))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_BoneAllocationRemoves.Dequeue();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_BoneAllocator.Release(allocationRemove.m_Allocation);
          }
          else
            break;
        }
        bool flag = false;
        // ISSUE: reference to a compiler-generated field
        while (!this.m_MetaBufferRemoves.IsEmpty())
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          AnimatedSystem.IndexRemove indexRemove = this.m_MetaBufferRemoves.Peek();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (!this.CheckTimeOffset(indexRemove.m_RemoveTime))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_MetaBufferRemoves.Dequeue();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (indexRemove.m_Index == this.m_MetaBufferData.Length - 1)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_MetaBufferData.RemoveAt(indexRemove.m_Index);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_FreeMetaIndices.Add(in indexRemove.m_Index);
              flag = true;
            }
          }
          else
            break;
        }
        if (!flag)
          return;
        // ISSUE: reference to a compiler-generated field
        if (this.m_FreeMetaIndices.Length >= 2)
        {
          // ISSUE: reference to a compiler-generated field
          NativeList<int> freeMetaIndices = this.m_FreeMetaIndices;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          AnimatedSystem.ReverseIntComparer reverseIntComparer = new AnimatedSystem.ReverseIntComparer();
          // ISSUE: variable of a compiler-generated type
          AnimatedSystem.ReverseIntComparer comp = reverseIntComparer;
          freeMetaIndices.Sort<int, AnimatedSystem.ReverseIntComparer>(comp);
        }
        // ISSUE: reference to a compiler-generated field
        int num = this.m_MetaBufferData.Length - 1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_FreeMetaIndices.Length && this.m_FreeMetaIndices[index] == num; ++index)
          --num;
        // ISSUE: reference to a compiler-generated field
        int count = this.m_MetaBufferData.Length - 1 - num;
        // ISSUE: reference to a compiler-generated field
        this.m_MetaBufferData.RemoveRange(num + 1, count);
        // ISSUE: reference to a compiler-generated field
        this.m_FreeMetaIndices.RemoveRange(0, count);
      }

      private bool CheckTimeOffset(int removeTime)
      {
        // ISSUE: reference to a compiler-generated field
        int num = this.m_CurrentTime - removeTime;
        return num + math.select(0, 65536, num < 0) < (int) byte.MaxValue;
      }
    }

    public struct IndexRemove
    {
      public int m_Index;
      public int m_RemoveTime;
    }

    public struct AllocationRemove
    {
      public NativeHeapBlock m_Allocation;
      public int m_RemoveTime;
    }

    public struct AllocationData
    {
      private NativeHeapAllocator m_BoneAllocator;
      private NativeList<MetaBufferData> m_MetaBufferData;
      private NativeList<int> m_FreeMetaIndices;
      private NativeList<int> m_UpdatedMetaIndices;
      private NativeQueue<AnimatedSystem.AllocationRemove> m_BoneAllocationRemoves;
      private NativeQueue<AnimatedSystem.IndexRemove> m_MetaBufferRemoves;
      private int m_CurrentTime;

      public AllocationData(
        NativeHeapAllocator boneAllocator,
        NativeList<MetaBufferData> metaBufferData,
        NativeList<int> freeMetaIndices,
        NativeList<int> updatedMetaIndices,
        NativeQueue<AnimatedSystem.AllocationRemove> boneAllocationRemoves,
        NativeQueue<AnimatedSystem.IndexRemove> metaBufferRemoves,
        int currentTime)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BoneAllocator = boneAllocator;
        // ISSUE: reference to a compiler-generated field
        this.m_MetaBufferData = metaBufferData;
        // ISSUE: reference to a compiler-generated field
        this.m_FreeMetaIndices = freeMetaIndices;
        // ISSUE: reference to a compiler-generated field
        this.m_UpdatedMetaIndices = updatedMetaIndices;
        // ISSUE: reference to a compiler-generated field
        this.m_BoneAllocationRemoves = boneAllocationRemoves;
        // ISSUE: reference to a compiler-generated field
        this.m_MetaBufferRemoves = metaBufferRemoves;
        // ISSUE: reference to a compiler-generated field
        this.m_CurrentTime = currentTime;
      }

      public NativeHeapBlock AllocateBones(int boneCount)
      {
        // ISSUE: reference to a compiler-generated field
        NativeHeapBlock nativeHeapBlock = this.m_BoneAllocator.Allocate((uint) boneCount);
        if (nativeHeapBlock.Empty)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_BoneAllocator.Resize(this.m_BoneAllocator.Size + 2097152U / (uint) sizeof (BoneElement));
          // ISSUE: reference to a compiler-generated field
          nativeHeapBlock = this.m_BoneAllocator.Allocate((uint) boneCount);
        }
        return nativeHeapBlock;
      }

      public void ReleaseBones(NativeHeapBlock allocation)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_BoneAllocationRemoves.Enqueue(new AnimatedSystem.AllocationRemove()
        {
          m_Allocation = allocation,
          m_RemoveTime = this.m_CurrentTime
        });
      }

      public int AddMetaBufferData(MetaBufferData metaBufferData)
      {
        int index;
        // ISSUE: reference to a compiler-generated field
        if (this.m_FreeMetaIndices.IsEmpty)
        {
          // ISSUE: reference to a compiler-generated field
          index = this.m_MetaBufferData.Length;
          // ISSUE: reference to a compiler-generated field
          this.m_MetaBufferData.Add(in metaBufferData);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          index = this.m_FreeMetaIndices[this.m_FreeMetaIndices.Length - 1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_FreeMetaIndices.RemoveAt(this.m_FreeMetaIndices.Length - 1);
          // ISSUE: reference to a compiler-generated field
          this.m_MetaBufferData[index] = metaBufferData;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_UpdatedMetaIndices.Add(in index);
        return index;
      }

      public void RemoveMetaBufferData(int metaIndex)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_MetaBufferRemoves.Enqueue(new AnimatedSystem.IndexRemove()
        {
          m_Index = metaIndex,
          m_RemoveTime = this.m_CurrentTime
        });
      }
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct ReverseIntComparer : IComparer<int>
    {
      public int Compare(int x, int y) => y - x;
    }

    public struct AnimationLayerData
    {
      public int m_CurrentIndex;
      public float m_CurrentFrame;
      public int m_TransitionIndex;
      public float m_TransitionFrame;
      public float m_TransitionWeight;
    }

    public struct AnimationLayerData2
    {
      public int m_CurrentIndex;
      public float m_CurrentFrame;
      public int2 m_TransitionIndex;
      public float2 m_TransitionFrame;
      public float2 m_TransitionWeight;
    }

    public struct AnimationFrameData
    {
      public int m_MetaIndex;
      public int m_RestPoseIndex;
      public int m_ResetHistory;
      public int m_CorrectiveIndex;
      public AnimatedSystem.AnimationLayerData2 m_BodyData;
      public AnimatedSystem.AnimationLayerData m_FaceData;
    }

    public struct ClipPriorityData : IComparable<AnimatedSystem.ClipPriorityData>
    {
      public AnimatedSystem.ClipIndex m_ClipIndex;
      public int m_Priority;
      public bool m_IsLoading;
      public bool m_IsLoaded;

      public int CompareTo(AnimatedSystem.ClipPriorityData other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.select(other.m_Priority - this.m_Priority, math.select(1, -1, this.m_IsLoading), this.m_IsLoading != other.m_IsLoading);
      }
    }

    public struct ClipIndex : IEquatable<AnimatedSystem.ClipIndex>
    {
      public Entity m_Style;
      public int m_Index;

      public ClipIndex(Entity style, int clipIndex)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Style = style;
        // ISSUE: reference to a compiler-generated field
        this.m_Index = clipIndex;
      }

      public bool Equals(AnimatedSystem.ClipIndex other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Style.Equals(other.m_Style) && this.m_Index == other.m_Index;
      }

      public override int GetHashCode() => this.m_Style.GetHashCode() * 5039 + this.m_Index;
    }

    public struct AnimationClipData
    {
      public NativeHeapBlock m_AnimAllocation;
      public NativeHeapBlock m_HierarchyAllocation;
      public NativeHeapBlock m_ShapeAllocation;
      public NativeHeapBlock m_BoneAllocation;
      public NativeHeapBlock m_InverseBoneAllocation;
    }
  }
}
