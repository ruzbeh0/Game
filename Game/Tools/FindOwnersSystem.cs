// Decompiled with JetBrains decompiler
// Type: Game.Tools.FindOwnersSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class FindOwnersSystem : GameSystemBase
  {
    private ModificationBarrier3 m_ModificationBarrier;
    private EntityQuery m_OwnersQuery;
    private EntityQuery m_SubEntityQuery;
    private FindOwnersSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier3>();
      // ISSUE: reference to a compiler-generated field
      this.m_OwnersQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Net.SubNet>(),
          ComponentType.ReadOnly<Game.Areas.SubArea>(),
          ComponentType.ReadOnly<Game.Objects.SubObject>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_SubEntityQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<OwnerDefinition>(),
          ComponentType.ReadOnly<Owner>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Net.Node>(),
          ComponentType.ReadOnly<Edge>(),
          ComponentType.ReadOnly<Area>(),
          ComponentType.ReadOnly<Object>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_SubEntityQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_OwnersQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new FindOwnersSystem.SetSubEntityOwnerJob()
      {
        m_OwnerChunks = archetypeChunkListAsync,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_OwnerDefinitionType = this.__TypeHandle.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RW_ComponentTypeHandle,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<FindOwnersSystem.SetSubEntityOwnerJob>(this.m_SubEntityQuery, JobHandle.CombineDependencies(outJobHandle, this.Dependency));
      archetypeChunkListAsync.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
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
    public FindOwnersSystem()
    {
    }

    [BurstCompile]
    public struct SetSubEntityOwnerJob : IJobChunk
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_OwnerChunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<OwnerDefinition> m_OwnerDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      public ComponentTypeHandle<Owner> m_OwnerType;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<OwnerDefinition> nativeArray2 = chunk.GetNativeArray<OwnerDefinition>(ref this.m_OwnerDefinitionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        bool flag = chunk.Has<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<OwnerDefinition>(unfilteredChunkIndex, nativeArray1);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          OwnerDefinition ownerDefinition = nativeArray2[index1];
          Owner owner = nativeArray3[index1];
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_OwnerChunks.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            ArchetypeChunk ownerChunk = this.m_OwnerChunks[index2];
            // ISSUE: reference to a compiler-generated field
            if (ownerChunk.Has<Temp>(ref this.m_TempType) == flag)
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<Entity> nativeArray4 = ownerChunk.GetNativeArray(this.m_EntityType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<PrefabRef> nativeArray5 = ownerChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<Transform> nativeArray6 = ownerChunk.GetNativeArray<Transform>(ref this.m_TransformType);
              for (int index3 = 0; index3 < nativeArray5.Length; ++index3)
              {
                if (nativeArray5[index3].m_Prefab.Equals(ownerDefinition.m_Prefab))
                {
                  Transform transform = nativeArray6[index3];
                  if (transform.m_Position.Equals(ownerDefinition.m_Position) && transform.m_Rotation.Equals(ownerDefinition.m_Rotation))
                  {
                    owner.m_Owner = nativeArray4[index3];
                    goto label_11;
                  }
                }
              }
            }
          }
label_11:
          nativeArray3[index1] = owner;
        }
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<OwnerDefinition> __Game_Tools_OwnerDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<OwnerDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>();
      }
    }
  }
}
