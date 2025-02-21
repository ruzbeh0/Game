// Decompiled with JetBrains decompiler
// Type: Game.Rendering.InitializeBonesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class InitializeBonesSystem : GameSystemBase
  {
    private ProceduralSkeletonSystem m_ProceduralSkeletonSystem;
    private PreCullingSystem m_PreCullingSystem;
    private InitializeBonesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ProceduralSkeletonSystem = this.World.GetOrCreateSystemManaged<ProceduralSkeletonSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PreCullingSystem = this.World.GetOrCreateSystemManaged<PreCullingSystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeReference<ProceduralSkeletonSystem.AllocationInfo> allocationInfo;
      NativeQueue<ProceduralSkeletonSystem.AllocationRemove> allocationRemoves;
      int currentTime;
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      NativeHeapAllocator heapAllocator = this.m_ProceduralSkeletonSystem.GetHeapAllocator(out allocationInfo, out allocationRemoves, out currentTime, out dependencies1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Momentum_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Bone_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Skeleton_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies2;
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
      JobHandle jobHandle = new InitializeBonesSystem.InitializeBonesJob()
      {
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ProceduralBones = this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup,
        m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_Skeletons = this.__TypeHandle.__Game_Rendering_Skeleton_RW_BufferLookup,
        m_Bones = this.__TypeHandle.__Game_Rendering_Bone_RW_BufferLookup,
        m_Momentums = this.__TypeHandle.__Game_Rendering_Momentum_RW_BufferLookup,
        m_CurrentTime = currentTime,
        m_CullingData = this.m_PreCullingSystem.GetUpdatedData(true, out dependencies2),
        m_HeapAllocator = heapAllocator,
        m_AllocationInfo = allocationInfo,
        m_AllocationRemoves = allocationRemoves
      }.Schedule<InitializeBonesSystem.InitializeBonesJob>(JobHandle.CombineDependencies(this.Dependency, dependencies2, dependencies1));
      // ISSUE: reference to a compiler-generated field
      this.m_ProceduralSkeletonSystem.AddHeapWriter(jobHandle);
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
    public InitializeBonesSystem()
    {
    }

    [BurstCompile]
    private struct InitializeBonesJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public BufferLookup<ProceduralBone> m_ProceduralBones;
      [ReadOnly]
      public BufferLookup<SubMesh> m_SubMeshes;
      public BufferLookup<Skeleton> m_Skeletons;
      public BufferLookup<Bone> m_Bones;
      public BufferLookup<Momentum> m_Momentums;
      [ReadOnly]
      public int m_CurrentTime;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;
      public NativeHeapAllocator m_HeapAllocator;
      public NativeReference<ProceduralSkeletonSystem.AllocationInfo> m_AllocationInfo;
      public NativeQueue<ProceduralSkeletonSystem.AllocationRemove> m_AllocationRemoves;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        ref ProceduralSkeletonSystem.AllocationInfo local = ref this.m_AllocationInfo.ValueAsRef<ProceduralSkeletonSystem.AllocationInfo>();
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_CullingData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          PreCullingData cullingData = this.m_CullingData[index];
          if ((cullingData.m_Flags & (PreCullingFlags.NearCameraUpdated | PreCullingFlags.Updated)) != (PreCullingFlags) 0 && (cullingData.m_Flags & PreCullingFlags.Skeleton) != (PreCullingFlags) 0)
          {
            if ((cullingData.m_Flags & PreCullingFlags.NearCamera) == (PreCullingFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.Remove(cullingData);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.Update(cullingData, ref local);
            }
          }
        }
      }

      private void Remove(PreCullingData cullingData)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Skeleton> skeleton = this.m_Skeletons[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Bone> bone = this.m_Bones[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated method
        this.Deallocate(skeleton);
        skeleton.Clear();
        bone.Clear();
        DynamicBuffer<Momentum> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Momentums.TryGetBuffer(cullingData.m_Entity, out bufferData))
          return;
        bufferData.Clear();
      }

      private void Update(
        PreCullingData cullingData,
        ref ProceduralSkeletonSystem.AllocationInfo allocationInfo)
      {
        DynamicBuffer<SubMesh> bufferData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubMeshes.TryGetBuffer(this.m_PrefabRefData[cullingData.m_Entity].m_Prefab, out bufferData1))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Skeleton> skeleton1 = this.m_Skeletons[cullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Bone> bone = this.m_Bones[cullingData.m_Entity];
          DynamicBuffer<Momentum> bufferData2;
          // ISSUE: reference to a compiler-generated field
          this.m_Momentums.TryGetBuffer(cullingData.m_Entity, out bufferData2);
          int length = 0;
          for (int index = 0; index < bufferData1.Length; ++index)
          {
            SubMesh subMesh = bufferData1[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_ProceduralBones.HasBuffer(subMesh.m_SubMesh))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ProceduralBone> proceduralBone = this.m_ProceduralBones[subMesh.m_SubMesh];
              length += proceduralBone.Length;
            }
          }
          if (skeleton1.Length == bufferData1.Length && bone.Length == length)
            return;
          // ISSUE: reference to a compiler-generated method
          this.Deallocate(skeleton1);
          skeleton1.ResizeUninitialized(bufferData1.Length);
          bone.ResizeUninitialized(length);
          if (bufferData2.IsCreated)
          {
            bufferData2.ResizeUninitialized(length);
            for (int index = 0; index < bufferData2.Length; ++index)
              bufferData2[index] = new Momentum();
          }
          int num = 0;
          Skeleton skeleton2;
          for (int index1 = 0; index1 < bufferData1.Length; ++index1)
          {
            DynamicBuffer<ProceduralBone> bufferData3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ProceduralBones.TryGetBuffer(bufferData1[index1].m_SubMesh, out bufferData3))
            {
              // ISSUE: reference to a compiler-generated field
              NativeHeapBlock nativeHeapBlock = this.m_HeapAllocator.Allocate((uint) bufferData3.Length);
              if (nativeHeapBlock.Empty)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_HeapAllocator.Resize(this.m_HeapAllocator.Size + 1048576U / (uint) sizeof (float4x4));
                // ISSUE: reference to a compiler-generated field
                nativeHeapBlock = this.m_HeapAllocator.Allocate((uint) bufferData3.Length);
              }
              ++allocationInfo.m_AllocationCount;
              skeleton2 = new Skeleton();
              skeleton2.m_BufferAllocation = nativeHeapBlock;
              skeleton2.m_BoneOffset = num;
              skeleton2.m_CurrentUpdated = true;
              skeleton2.m_HistoryUpdated = true;
              Skeleton skeleton3 = skeleton2;
              for (int index2 = 0; index2 < bufferData3.Length; ++index2)
              {
                ProceduralBone proceduralBone = bufferData3[index2];
                skeleton3.m_RequireHistory |= proceduralBone.m_ConnectionID != 0;
                bone[num++] = new Bone()
                {
                  m_Position = proceduralBone.m_Position,
                  m_Rotation = proceduralBone.m_Rotation,
                  m_Scale = proceduralBone.m_Scale
                };
              }
              skeleton1[index1] = skeleton3;
            }
            else
            {
              ref DynamicBuffer<Skeleton> local = ref skeleton1;
              int index3 = index1;
              skeleton2 = new Skeleton();
              skeleton2.m_BoneOffset = -1;
              Skeleton skeleton4 = skeleton2;
              local[index3] = skeleton4;
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.Remove(cullingData);
        }
      }

      private void Deallocate(DynamicBuffer<Skeleton> skeletons)
      {
        for (int index = 0; index < skeletons.Length; ++index)
        {
          Skeleton skeleton = skeletons[index];
          if (!skeleton.m_BufferAllocation.Empty)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_AllocationRemoves.Enqueue(new ProceduralSkeletonSystem.AllocationRemove()
            {
              m_Allocation = skeleton.m_BufferAllocation,
              m_RemoveTime = this.m_CurrentTime
            });
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ProceduralBone> __Game_Prefabs_ProceduralBone_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      public BufferLookup<Skeleton> __Game_Rendering_Skeleton_RW_BufferLookup;
      public BufferLookup<Bone> __Game_Rendering_Bone_RW_BufferLookup;
      public BufferLookup<Momentum> __Game_Rendering_Momentum_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ProceduralBone_RO_BufferLookup = state.GetBufferLookup<ProceduralBone>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Skeleton_RW_BufferLookup = state.GetBufferLookup<Skeleton>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Bone_RW_BufferLookup = state.GetBufferLookup<Bone>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Momentum_RW_BufferLookup = state.GetBufferLookup<Momentum>();
      }
    }
  }
}
