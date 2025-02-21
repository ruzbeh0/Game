// Decompiled with JetBrains decompiler
// Type: Game.Rendering.InitializeBoneHistoriesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Game.Tools;
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
  public class InitializeBoneHistoriesSystem : GameSystemBase
  {
    private PreCullingSystem m_PreCullingSystem;
    private InitializeBoneHistoriesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PreCullingSystem = this.World.GetOrCreateSystemManaged<PreCullingSystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_BoneHistory_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Bone_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Skeleton_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
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
      InitializeBoneHistoriesSystem.InitializeBoneHistoriesJob jobData = new InitializeBoneHistoriesSystem.InitializeBoneHistoriesJob()
      {
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_Skeletons = this.__TypeHandle.__Game_Rendering_Skeleton_RO_BufferLookup,
        m_Bones = this.__TypeHandle.__Game_Rendering_Bone_RO_BufferLookup,
        m_ProceduralBones = this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup,
        m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_BoneHistories = this.__TypeHandle.__Game_Rendering_BoneHistory_RW_BufferLookup,
        m_CullingData = this.m_PreCullingSystem.GetUpdatedData(true, out dependencies1)
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle dependencies2 = jobData.Schedule<InitializeBoneHistoriesSystem.InitializeBoneHistoriesJob, PreCullingData>(jobData.m_CullingData, 4, JobHandle.CombineDependencies(this.Dependency, dependencies1));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PreCullingSystem.AddCullingDataReader(dependencies2);
      this.Dependency = dependencies2;
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
    public InitializeBoneHistoriesSystem()
    {
    }

    [BurstCompile]
    private struct InitializeBoneHistoriesJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public BufferLookup<Skeleton> m_Skeletons;
      [ReadOnly]
      public BufferLookup<Bone> m_Bones;
      [ReadOnly]
      public BufferLookup<ProceduralBone> m_ProceduralBones;
      [ReadOnly]
      public BufferLookup<SubMesh> m_SubMeshes;
      [NativeDisableParallelForRestriction]
      public BufferLookup<BoneHistory> m_BoneHistories;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        PreCullingData cullingData = this.m_CullingData[index];
        if ((cullingData.m_Flags & (PreCullingFlags.NearCameraUpdated | PreCullingFlags.Updated)) == (PreCullingFlags) 0 || (cullingData.m_Flags & PreCullingFlags.Skeleton) == (PreCullingFlags) 0)
          return;
        if ((cullingData.m_Flags & PreCullingFlags.NearCamera) == (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.Remove(cullingData);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.Update(cullingData);
        }
      }

      private void Remove(PreCullingData cullingData)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BoneHistories[cullingData.m_Entity].Clear();
      }

      private void Update(PreCullingData cullingData)
      {
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[cullingData.m_Entity];
        DynamicBuffer<SubMesh> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubMeshes.TryGetBuffer(prefabRef.m_Prefab, out bufferData1))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Skeleton> skeleton1 = this.m_Skeletons[cullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Bone> bones1 = this.m_Bones[cullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<BoneHistory> boneHistory = this.m_BoneHistories[cullingData.m_Entity];
          if (bones1.Length == boneHistory.Length)
            return;
          boneHistory.ResizeUninitialized(bones1.Length);
          DynamicBuffer<Skeleton> bufferData2 = new DynamicBuffer<Skeleton>();
          DynamicBuffer<Bone> bones2 = new DynamicBuffer<Bone>();
          bool flag = false;
          if ((cullingData.m_Flags & PreCullingFlags.Temp) != (PreCullingFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            Temp temp = this.m_TempData[cullingData.m_Entity];
            PrefabRef componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            flag = this.m_PrefabRefData.TryGetComponent(temp.m_Original, out componentData) && this.m_Skeletons.TryGetBuffer(temp.m_Original, out bufferData2) && this.m_Bones.TryGetBuffer(temp.m_Original, out bones2) && componentData.m_Prefab == prefabRef.m_Prefab && bufferData2.Length == skeleton1.Length && bones2.Length == bones1.Length;
          }
          NativeList<float4x4> tempMatrices = new NativeList<float4x4>();
          for (int index1 = 0; index1 < skeleton1.Length; ++index1)
          {
            Skeleton skeleton2 = skeleton1[index1];
            if (skeleton2.m_BoneOffset >= 0)
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ProceduralBone> proceduralBones = this.m_ProceduralBones[bufferData1[index1].m_SubMesh];
              if (!tempMatrices.IsCreated)
                tempMatrices = new NativeList<float4x4>(proceduralBones.Length * 2, (AllocatorManager.AllocatorHandle) Allocator.Temp);
              tempMatrices.ResizeUninitialized(proceduralBones.Length * 2);
              if (flag)
                ProceduralSkeletonSystem.GetSkinMatrices(bufferData2[index1], in proceduralBones, in bones2, tempMatrices);
              else
                ProceduralSkeletonSystem.GetSkinMatrices(skeleton2, in proceduralBones, in bones1, tempMatrices);
              for (int index2 = 0; index2 < proceduralBones.Length; ++index2)
              {
                ProceduralBone proceduralBone = proceduralBones[index2];
                boneHistory[skeleton2.m_BoneOffset + index2] = new BoneHistory()
                {
                  m_Matrix = tempMatrices[proceduralBones.Length + proceduralBone.m_BindIndex]
                };
              }
            }
          }
          if (!tempMatrices.IsCreated)
            return;
          tempMatrices.Dispose();
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.Remove(cullingData);
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Skeleton> __Game_Rendering_Skeleton_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Bone> __Game_Rendering_Bone_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ProceduralBone> __Game_Prefabs_ProceduralBone_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      public BufferLookup<BoneHistory> __Game_Rendering_BoneHistory_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Skeleton_RO_BufferLookup = state.GetBufferLookup<Skeleton>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Bone_RO_BufferLookup = state.GetBufferLookup<Bone>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ProceduralBone_RO_BufferLookup = state.GetBufferLookup<ProceduralBone>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_BoneHistory_RW_BufferLookup = state.GetBufferLookup<BoneHistory>();
      }
    }
  }
}
