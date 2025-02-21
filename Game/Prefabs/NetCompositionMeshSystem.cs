// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetCompositionMeshSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class NetCompositionMeshSystem : GameSystemBase
  {
    private EntityQuery m_MeshQuery;
    private NativeParallelMultiHashMap<int, Entity> m_MeshEntities;
    private JobHandle m_Dependencies;
    private NetCompositionMeshSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_MeshQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<NetCompositionMeshData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_MeshEntities = new NativeParallelMultiHashMap<int, Entity>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_MeshQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Dependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_MeshEntities.Dispose();
      base.OnDestroy();
    }

    public NativeParallelMultiHashMap<int, Entity> GetMeshEntities(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_Dependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_MeshEntities;
    }

    public void AddMeshEntityReader(JobHandle dependencies) => this.m_Dependencies = dependencies;

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionMeshData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new NetCompositionMeshSystem.CompositionMeshJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_NetCompositionMeshDataType = this.__TypeHandle.__Game_Prefabs_NetCompositionMeshData_RO_ComponentTypeHandle,
        m_MeshEntities = this.m_MeshEntities
      }.Schedule<NetCompositionMeshSystem.CompositionMeshJob>(this.m_MeshQuery, JobHandle.CombineDependencies(this.Dependency, this.m_Dependencies));
      // ISSUE: reference to a compiler-generated field
      this.m_Dependencies = jobHandle;
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
    public NetCompositionMeshSystem()
    {
    }

    [BurstCompile]
    private struct CompositionMeshJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<NetCompositionMeshData> m_NetCompositionMeshDataType;
      public NativeParallelMultiHashMap<int, Entity> m_MeshEntities;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetCompositionMeshData> nativeArray2 = chunk.GetNativeArray<NetCompositionMeshData>(ref this.m_NetCompositionMeshDataType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
label_7:
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity entity1 = nativeArray1[index];
            Entity entity2;
            NativeParallelMultiHashMapIterator<int> it;
            // ISSUE: reference to a compiler-generated field
            if (this.m_MeshEntities.TryGetFirstValue(nativeArray2[index].m_Hash, out entity2, out it))
            {
              while (!(entity2 == entity1))
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.m_MeshEntities.TryGetNextValue(out entity2, ref it))
                  goto label_7;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_MeshEntities.Remove(it);
            }
          }
        }
        else
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_MeshEntities.Add(nativeArray2[index].m_Hash, nativeArray1[index]);
          }
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
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<NetCompositionMeshData> __Game_Prefabs_NetCompositionMeshData_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionMeshData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NetCompositionMeshData>(true);
      }
    }
  }
}
