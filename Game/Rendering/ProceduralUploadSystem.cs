// Decompiled with JetBrains decompiler
// Type: Game.Rendering.ProceduralUploadSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Rendering;
using Game.Prefabs;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class ProceduralUploadSystem : GameSystemBase
  {
    private ProceduralSkeletonSystem m_ProceduralSkeletonSystem;
    private ProceduralEmissiveSystem m_ProceduralEmissiveSystem;
    private PreCullingSystem m_PreCullingSystem;
    private PrefabSystem m_PrefabSystem;
    private RenderPrefabBase m_OverridePrefab;
    private NativeAccumulator<ProceduralUploadSystem.UploadData> m_UploadData;
    private JobHandle m_PrepareDeps;
    private Entity m_OverrideEntity;
    private LightState m_OverrideLightState;
    private int m_OverrideSingleLightIndex;
    private int m_OverrideMultiLightIndex;
    private float m_OverrideTime;
    private ProceduralUploadSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ProceduralSkeletonSystem = this.World.GetOrCreateSystemManaged<ProceduralSkeletonSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ProceduralEmissiveSystem = this.World.GetOrCreateSystemManaged<ProceduralEmissiveSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PreCullingSystem = this.World.GetOrCreateSystemManaged<PreCullingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
    }

    public void SetOverride(
      Entity entity,
      RenderPrefabBase prefab,
      int singleLightIndex,
      int multiLightIndex)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (entity != this.m_OverrideEntity || singleLightIndex != this.m_OverrideSingleLightIndex || multiLightIndex != this.m_OverrideMultiLightIndex)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_OverrideLightState.m_Intensity = -1f;
        // ISSUE: reference to a compiler-generated field
        this.m_OverrideTime = 0.0f;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_OverrideEntity = entity;
      // ISSUE: reference to a compiler-generated field
      this.m_OverridePrefab = prefab;
      // ISSUE: reference to a compiler-generated field
      this.m_OverrideSingleLightIndex = singleLightIndex;
      // ISSUE: reference to a compiler-generated field
      this.m_OverrideMultiLightIndex = multiLightIndex;
    }

    private void UpdateOverride(ref ProceduralUploadSystem.UploadData emissiveData)
    {
      DynamicBuffer<Emissive> buffer1;
      DynamicBuffer<LightState> buffer2;
      PrefabRef component1;
      DynamicBuffer<SubMesh> buffer3;
      EmissiveProperties component2;
      Entity entity;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.EntityManager.TryGetBuffer<Emissive>(this.m_OverrideEntity, false, out buffer1) || !this.EntityManager.TryGetBuffer<LightState>(this.m_OverrideEntity, false, out buffer2) || !this.EntityManager.TryGetComponent<PrefabRef>(this.m_OverrideEntity, out component1) || !this.EntityManager.TryGetBuffer<SubMesh>(component1.m_Prefab, true, out buffer3) || !this.m_OverridePrefab.TryGet<EmissiveProperties>(out component2) || !this.m_PrefabSystem.TryGetEntity((PrefabBase) this.m_OverridePrefab, out entity))
        return;
      int index1;
      // ISSUE: reference to a compiler-generated field
      if (this.m_OverrideMultiLightIndex >= 0)
      {
        // ISSUE: reference to a compiler-generated field
        index1 = this.m_OverrideMultiLightIndex;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_OverrideSingleLightIndex < 0)
          return;
        // ISSUE: reference to a compiler-generated field
        index1 = this.m_OverrideSingleLightIndex;
        if (component2.hasMultiLights)
          index1 += component2.m_MultiLights.Count;
      }
      float deltaTime = UnityEngine.Time.deltaTime;
      for (int index2 = 0; index2 < buffer1.Length; ++index2)
      {
        ref Emissive local1 = ref buffer1.ElementAt(index2);
        if (!local1.m_BufferAllocation.Empty)
        {
          SubMesh subMesh = buffer3[index2];
          DynamicBuffer<ProceduralLight> buffer4;
          if (!(subMesh.m_SubMesh != entity) && this.EntityManager.TryGetBuffer<ProceduralLight>(subMesh.m_SubMesh, true, out buffer4) && index1 < buffer4.Length)
          {
            if (!local1.m_Updated)
            {
              uint num = local1.m_BufferAllocation.Length * (uint) sizeof (float4);
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: reference to a compiler-generated method
              emissiveData.Accumulate(new ProceduralUploadSystem.UploadData()
              {
                m_OpCount = 1,
                m_DataSize = num,
                m_MaxOpSize = num
              });
            }
            ProceduralLight proceduralLight = buffer4[index1];
            ref LightState local2 = ref buffer2.ElementAt(local1.m_LightOffset + index1);
            // ISSUE: reference to a compiler-generated field
            if ((double) this.m_OverrideLightState.m_Intensity < 0.0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_OverrideLightState = local2;
            }
            float2 target = new float2(1f, 0.0f);
            DynamicBuffer<LightAnimation> buffer5;
            if (proceduralLight.m_AnimationIndex >= 0 && this.EntityManager.TryGetBuffer<LightAnimation>(subMesh.m_SubMesh, true, out buffer5))
            {
              LightAnimation lightAnimation = buffer5[proceduralLight.m_AnimationIndex];
              // ISSUE: reference to a compiler-generated field
              this.m_OverrideTime += deltaTime * 60f;
              // ISSUE: reference to a compiler-generated field
              this.m_OverrideTime %= (float) lightAnimation.m_DurationFrames;
              // ISSUE: reference to a compiler-generated field
              target.x *= lightAnimation.m_AnimationCurve.Evaluate(this.m_OverrideTime / (float) lightAnimation.m_DurationFrames);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.AnimateLight(proceduralLight, ref local1, ref this.m_OverrideLightState, deltaTime, target, false);
            // ISSUE: reference to a compiler-generated field
            local2 = this.m_OverrideLightState;
            local1.m_Updated = true;
          }
        }
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PrepareDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      ProceduralUploadSystem.UploadData result1 = this.m_UploadData.GetResult();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      ProceduralUploadSystem.UploadData result2 = this.m_UploadData.GetResult(1);
      // ISSUE: reference to a compiler-generated field
      this.m_UploadData.Dispose();
      // ISSUE: reference to a compiler-generated field
      if (this.m_OverrideEntity != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateOverride(ref result2);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Emissive_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_BoneHistory_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Skeleton_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProceduralLight_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_LightState_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Bone_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      int historyByteOffset;
      JobHandle dependencies;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ProceduralUploadSystem.ProceduralUploadJob jobData = new ProceduralUploadSystem.ProceduralUploadJob()
      {
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_Bones = this.__TypeHandle.__Game_Rendering_Bone_RO_BufferLookup,
        m_Lights = this.__TypeHandle.__Game_Rendering_LightState_RO_BufferLookup,
        m_ProceduralBones = this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup,
        m_ProceduralLights = this.__TypeHandle.__Game_Prefabs_ProceduralLight_RO_BufferLookup,
        m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_Skeletons = this.__TypeHandle.__Game_Rendering_Skeleton_RW_BufferLookup,
        m_BoneHistories = this.__TypeHandle.__Game_Rendering_BoneHistory_RW_BufferLookup,
        m_Emissives = this.__TypeHandle.__Game_Rendering_Emissive_RW_BufferLookup,
        m_BoneUploader = this.m_ProceduralSkeletonSystem.BeginUpload(result1.m_OpCount, result1.m_DataSize, result1.m_MaxOpSize, out historyByteOffset),
        m_LightUploader = this.m_ProceduralEmissiveSystem.BeginUpload(result2.m_OpCount, result2.m_DataSize, result2.m_MaxOpSize),
        m_HistoryByteOffset = historyByteOffset,
        m_MotionBlurEnabled = this.m_ProceduralSkeletonSystem.isMotionBlurEnabled,
        m_ForceHistoryUpdate = this.m_ProceduralSkeletonSystem.forceHistoryUpdate,
        m_CullingData = this.m_PreCullingSystem.GetCullingData(true, out dependencies)
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData.Schedule<ProceduralUploadSystem.ProceduralUploadJob, PreCullingData>(jobData.m_CullingData, 16, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      this.m_ProceduralSkeletonSystem.AddUploadWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ProceduralEmissiveSystem.AddUploadWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PreCullingSystem.AddCullingDataReader(jobHandle);
      this.Dependency = jobHandle;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
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

    [UnityEngine.Scripting.Preserve]
    public ProceduralUploadSystem()
    {
    }

    internal struct UploadData : IAccumulable<ProceduralUploadSystem.UploadData>
    {
      public int m_OpCount;
      public uint m_DataSize;
      public uint m_MaxOpSize;

      public void Accumulate(ProceduralUploadSystem.UploadData other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_OpCount += other.m_OpCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_DataSize += other.m_DataSize;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_MaxOpSize = math.max(this.m_MaxOpSize, other.m_MaxOpSize);
      }
    }

    [BurstCompile]
    private struct ProceduralPrepareJob : IJobParallelForDefer
    {
      [ReadOnly]
      public BufferLookup<Skeleton> m_Skeletons;
      [ReadOnly]
      public BufferLookup<Emissive> m_Emissives;
      [ReadOnly]
      public bool m_MotionBlurEnabled;
      [ReadOnly]
      public bool m_ForceHistoryUpdate;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;
      public NativeAccumulator<ProceduralUploadSystem.UploadData>.ParallelWriter m_UploadData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        PreCullingData preCullingData = this.m_CullingData[index];
        if ((preCullingData.m_Flags & (PreCullingFlags.Skeleton | PreCullingFlags.Emissive)) == (PreCullingFlags) 0 || (preCullingData.m_Flags & PreCullingFlags.NearCamera) == (PreCullingFlags) 0)
          return;
        // ISSUE: variable of a compiler-generated type
        ProceduralUploadSystem.UploadData uploadData1;
        if ((preCullingData.m_Flags & PreCullingFlags.Skeleton) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Skeleton> skeleton1 = this.m_Skeletons[preCullingData.m_Entity];
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          ProceduralUploadSystem.UploadData uploadData2 = new ProceduralUploadSystem.UploadData();
          // ISSUE: reference to a compiler-generated field
          if (this.m_MotionBlurEnabled)
          {
            for (int index1 = 0; index1 < skeleton1.Length; ++index1)
            {
              Skeleton skeleton2 = skeleton1[index1];
              // ISSUE: reference to a compiler-generated field
              if ((skeleton2.m_CurrentUpdated || skeleton2.m_HistoryUpdated || this.m_ForceHistoryUpdate) && !skeleton2.m_BufferAllocation.Empty)
              {
                uint num = skeleton2.m_BufferAllocation.Length * (uint) sizeof (float4x4);
                if (skeleton2.m_CurrentUpdated)
                {
                  ref ProceduralUploadSystem.UploadData local = ref uploadData2;
                  // ISSUE: object of a compiler-generated type is created
                  uploadData1 = new ProceduralUploadSystem.UploadData();
                  // ISSUE: reference to a compiler-generated field
                  uploadData1.m_OpCount = 1;
                  // ISSUE: reference to a compiler-generated field
                  uploadData1.m_DataSize = num;
                  // ISSUE: reference to a compiler-generated field
                  uploadData1.m_MaxOpSize = num;
                  // ISSUE: variable of a compiler-generated type
                  ProceduralUploadSystem.UploadData other = uploadData1;
                  // ISSUE: reference to a compiler-generated method
                  local.Accumulate(other);
                }
                // ISSUE: reference to a compiler-generated field
                if (skeleton2.m_HistoryUpdated || this.m_ForceHistoryUpdate)
                {
                  ref ProceduralUploadSystem.UploadData local = ref uploadData2;
                  // ISSUE: object of a compiler-generated type is created
                  uploadData1 = new ProceduralUploadSystem.UploadData();
                  // ISSUE: reference to a compiler-generated field
                  uploadData1.m_OpCount = 1;
                  // ISSUE: reference to a compiler-generated field
                  uploadData1.m_DataSize = num;
                  // ISSUE: reference to a compiler-generated field
                  uploadData1.m_MaxOpSize = num;
                  // ISSUE: variable of a compiler-generated type
                  ProceduralUploadSystem.UploadData other = uploadData1;
                  // ISSUE: reference to a compiler-generated method
                  local.Accumulate(other);
                }
              }
            }
          }
          else
          {
            for (int index2 = 0; index2 < skeleton1.Length; ++index2)
            {
              Skeleton skeleton3 = skeleton1[index2];
              if (skeleton3.m_CurrentUpdated && !skeleton3.m_BufferAllocation.Empty)
              {
                uint num = skeleton3.m_BufferAllocation.Length * (uint) sizeof (float4x4);
                ref ProceduralUploadSystem.UploadData local = ref uploadData2;
                // ISSUE: object of a compiler-generated type is created
                uploadData1 = new ProceduralUploadSystem.UploadData();
                // ISSUE: reference to a compiler-generated field
                uploadData1.m_OpCount = 1;
                // ISSUE: reference to a compiler-generated field
                uploadData1.m_DataSize = num;
                // ISSUE: reference to a compiler-generated field
                uploadData1.m_MaxOpSize = num;
                // ISSUE: variable of a compiler-generated type
                ProceduralUploadSystem.UploadData other = uploadData1;
                // ISSUE: reference to a compiler-generated method
                local.Accumulate(other);
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_UploadData.Accumulate(0, uploadData2);
        }
        if ((preCullingData.m_Flags & PreCullingFlags.Emissive) == (PreCullingFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Emissive> emissive1 = this.m_Emissives[preCullingData.m_Entity];
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ProceduralUploadSystem.UploadData uploadData3 = new ProceduralUploadSystem.UploadData();
        for (int index3 = 0; index3 < emissive1.Length; ++index3)
        {
          Emissive emissive2 = emissive1[index3];
          if (emissive2.m_Updated && !emissive2.m_BufferAllocation.Empty)
          {
            uint num = emissive2.m_BufferAllocation.Length * (uint) sizeof (float4);
            ref ProceduralUploadSystem.UploadData local = ref uploadData3;
            // ISSUE: object of a compiler-generated type is created
            uploadData1 = new ProceduralUploadSystem.UploadData();
            // ISSUE: reference to a compiler-generated field
            uploadData1.m_OpCount = 1;
            // ISSUE: reference to a compiler-generated field
            uploadData1.m_DataSize = num;
            // ISSUE: reference to a compiler-generated field
            uploadData1.m_MaxOpSize = num;
            // ISSUE: variable of a compiler-generated type
            ProceduralUploadSystem.UploadData other = uploadData1;
            // ISSUE: reference to a compiler-generated method
            local.Accumulate(other);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_UploadData.Accumulate(1, uploadData3);
      }
    }

    [BurstCompile]
    private struct ProceduralUploadJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public BufferLookup<Bone> m_Bones;
      [ReadOnly]
      public BufferLookup<LightState> m_Lights;
      [ReadOnly]
      public BufferLookup<ProceduralBone> m_ProceduralBones;
      [ReadOnly]
      public BufferLookup<ProceduralLight> m_ProceduralLights;
      [ReadOnly]
      public BufferLookup<SubMesh> m_SubMeshes;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Skeleton> m_Skeletons;
      [NativeDisableParallelForRestriction]
      public BufferLookup<BoneHistory> m_BoneHistories;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Emissive> m_Emissives;
      [ReadOnly]
      public ThreadedSparseUploader m_BoneUploader;
      [ReadOnly]
      public ThreadedSparseUploader m_LightUploader;
      [ReadOnly]
      public int m_HistoryByteOffset;
      [ReadOnly]
      public bool m_MotionBlurEnabled;
      [ReadOnly]
      public bool m_ForceHistoryUpdate;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;

      public unsafe void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        PreCullingData preCullingData = this.m_CullingData[index];
        if ((preCullingData.m_Flags & (PreCullingFlags.Skeleton | PreCullingFlags.Emissive)) == (PreCullingFlags) 0 || (preCullingData.m_Flags & PreCullingFlags.NearCamera) == (PreCullingFlags) 0)
          return;
        DynamicBuffer<Skeleton> dynamicBuffer1 = new DynamicBuffer<Skeleton>();
        DynamicBuffer<Emissive> dynamicBuffer2 = new DynamicBuffer<Emissive>();
        bool flag1 = false;
        bool flag2 = false;
        if ((preCullingData.m_Flags & PreCullingFlags.Skeleton) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          dynamicBuffer1 = this.m_Skeletons[preCullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_MotionBlurEnabled)
          {
            for (int index1 = 0; index1 < dynamicBuffer1.Length; ++index1)
            {
              ref Skeleton local = ref dynamicBuffer1.ElementAt(index1);
              // ISSUE: reference to a compiler-generated field
              if ((local.m_CurrentUpdated || local.m_HistoryUpdated || this.m_ForceHistoryUpdate) && !local.m_BufferAllocation.Empty)
                flag1 = true;
            }
          }
          else
          {
            for (int index2 = 0; index2 < dynamicBuffer1.Length; ++index2)
            {
              ref Skeleton local = ref dynamicBuffer1.ElementAt(index2);
              if (local.m_CurrentUpdated && !local.m_BufferAllocation.Empty)
                flag1 = true;
            }
          }
        }
        if ((preCullingData.m_Flags & PreCullingFlags.Emissive) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          dynamicBuffer2 = this.m_Emissives[preCullingData.m_Entity];
          for (int index3 = 0; index3 < dynamicBuffer2.Length; ++index3)
          {
            ref Emissive local = ref dynamicBuffer2.ElementAt(index3);
            if (local.m_Updated && !local.m_BufferAllocation.Empty)
              flag2 = true;
          }
        }
        if (!flag1 && !flag2)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<SubMesh> subMesh = this.m_SubMeshes[this.m_PrefabRefData[preCullingData.m_Entity].m_Prefab];
        if (flag1)
        {
          NativeList<float4x4> nativeList = new NativeList<float4x4>();
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Bone> bones = this.m_Bones[preCullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<BoneHistory> boneHistory = this.m_BoneHistories[preCullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_MotionBlurEnabled)
          {
            for (int index4 = 0; index4 < dynamicBuffer1.Length; ++index4)
            {
              ref Skeleton local = ref dynamicBuffer1.ElementAt(index4);
              // ISSUE: reference to a compiler-generated field
              if ((local.m_CurrentUpdated || local.m_HistoryUpdated || this.m_ForceHistoryUpdate) && !local.m_BufferAllocation.Empty)
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<ProceduralBone> proceduralBones = this.m_ProceduralBones[subMesh[index4].m_SubMesh];
                if (!nativeList.IsCreated)
                  nativeList = new NativeList<float4x4>(proceduralBones.Length * 3, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                nativeList.ResizeUninitialized(proceduralBones.Length * 3);
                ProceduralSkeletonSystem.GetSkinMatrices(local, in proceduralBones, in bones, nativeList);
                // ISSUE: reference to a compiler-generated field
                if (this.m_ForceHistoryUpdate)
                {
                  for (int index5 = 0; index5 < proceduralBones.Length; ++index5)
                  {
                    ProceduralBone proceduralBone = proceduralBones[index5];
                    int index6 = local.m_BoneOffset + index5;
                    float4x4 float4x4 = nativeList[proceduralBones.Length + proceduralBone.m_BindIndex];
                    nativeList[proceduralBones.Length * 2 + proceduralBone.m_BindIndex] = float4x4;
                    boneHistory[index6] = new BoneHistory()
                    {
                      m_Matrix = float4x4
                    };
                  }
                }
                else
                {
                  for (int index7 = 0; index7 < proceduralBones.Length; ++index7)
                  {
                    ProceduralBone proceduralBone = proceduralBones[index7];
                    int index8 = local.m_BoneOffset + index7;
                    float4x4 matrix = boneHistory[index8].m_Matrix;
                    float4x4 float4x4 = nativeList[proceduralBones.Length + proceduralBone.m_BindIndex];
                    nativeList[proceduralBones.Length * 2 + proceduralBone.m_BindIndex] = matrix;
                    boneHistory[index8] = new BoneHistory()
                    {
                      m_Matrix = float4x4
                    };
                  }
                }
                int size = proceduralBones.Length * sizeof (float4x4);
                int offsetInBytes = (int) local.m_BufferAllocation.Begin * sizeof (float4x4);
                if (local.m_CurrentUpdated)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_BoneUploader.AddUpload((void*) ((IntPtr) nativeList.GetUnsafePtr<float4x4>() + size), size, offsetInBytes);
                }
                // ISSUE: reference to a compiler-generated field
                if (local.m_HistoryUpdated || this.m_ForceHistoryUpdate)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_BoneUploader.AddUpload((void*) ((IntPtr) nativeList.GetUnsafePtr<float4x4>() + size * 2), size, offsetInBytes + this.m_HistoryByteOffset);
                }
                local.m_HistoryUpdated = local.m_CurrentUpdated;
                local.m_CurrentUpdated = false;
              }
            }
          }
          else
          {
            for (int index9 = 0; index9 < dynamicBuffer1.Length; ++index9)
            {
              ref Skeleton local = ref dynamicBuffer1.ElementAt(index9);
              if (local.m_CurrentUpdated && !local.m_BufferAllocation.Empty)
              {
                local.m_CurrentUpdated = false;
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<ProceduralBone> proceduralBones = this.m_ProceduralBones[subMesh[index9].m_SubMesh];
                if (!nativeList.IsCreated)
                  nativeList = new NativeList<float4x4>(proceduralBones.Length * 2, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                nativeList.ResizeUninitialized(proceduralBones.Length * 2);
                ProceduralSkeletonSystem.GetSkinMatrices(local, in proceduralBones, in bones, nativeList);
                // ISSUE: reference to a compiler-generated field
                this.m_BoneUploader.AddUpload((void*) (nativeList.GetUnsafePtr<float4x4>() + proceduralBones.Length), proceduralBones.Length * sizeof (float4x4), (int) local.m_BufferAllocation.Begin * sizeof (float4x4));
                if (local.m_RequireHistory)
                {
                  for (int index10 = 0; index10 < proceduralBones.Length; ++index10)
                  {
                    ProceduralBone proceduralBone = proceduralBones[index10];
                    float4x4 float4x4 = nativeList[proceduralBones.Length + proceduralBone.m_BindIndex];
                    boneHistory[local.m_BoneOffset + index10] = new BoneHistory()
                    {
                      m_Matrix = float4x4
                    };
                  }
                }
              }
            }
          }
          if (nativeList.IsCreated)
            nativeList.Dispose();
        }
        if (!flag2)
          return;
        NativeList<float4> nativeList1 = new NativeList<float4>();
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<LightState> lights = this.m_Lights[preCullingData.m_Entity];
        for (int index11 = 0; index11 < dynamicBuffer2.Length; ++index11)
        {
          ref Emissive local = ref dynamicBuffer2.ElementAt(index11);
          if (local.m_Updated && !local.m_BufferAllocation.Empty)
          {
            local.m_Updated = false;
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ProceduralLight> proceduralLights = this.m_ProceduralLights[subMesh[index11].m_SubMesh];
            if (!nativeList1.IsCreated)
              nativeList1 = new NativeList<float4>(proceduralLights.Length + 1, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            nativeList1.ResizeUninitialized(proceduralLights.Length + 1);
            ProceduralEmissiveSystem.GetGpuLights(local, in proceduralLights, in lights, nativeList1);
            // ISSUE: reference to a compiler-generated field
            this.m_LightUploader.AddUpload((void*) nativeList1.GetUnsafePtr<float4>(), nativeList1.Length * sizeof (float4), (int) local.m_BufferAllocation.Begin * sizeof (float4));
          }
        }
        if (!nativeList1.IsCreated)
          return;
        nativeList1.Dispose();
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Bone> __Game_Rendering_Bone_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LightState> __Game_Rendering_LightState_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ProceduralBone> __Game_Prefabs_ProceduralBone_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ProceduralLight> __Game_Prefabs_ProceduralLight_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      public BufferLookup<Skeleton> __Game_Rendering_Skeleton_RW_BufferLookup;
      public BufferLookup<BoneHistory> __Game_Rendering_BoneHistory_RW_BufferLookup;
      public BufferLookup<Emissive> __Game_Rendering_Emissive_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Bone_RO_BufferLookup = state.GetBufferLookup<Bone>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_LightState_RO_BufferLookup = state.GetBufferLookup<LightState>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ProceduralBone_RO_BufferLookup = state.GetBufferLookup<ProceduralBone>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ProceduralLight_RO_BufferLookup = state.GetBufferLookup<ProceduralLight>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Skeleton_RW_BufferLookup = state.GetBufferLookup<Skeleton>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_BoneHistory_RW_BufferLookup = state.GetBufferLookup<BoneHistory>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Emissive_RW_BufferLookup = state.GetBufferLookup<Emissive>();
      }
    }
  }
}
