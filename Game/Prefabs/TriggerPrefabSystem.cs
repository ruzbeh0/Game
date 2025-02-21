// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TriggerPrefabSystem
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
  public class TriggerPrefabSystem : GameSystemBase
  {
    private TriggerPrefabData m_PrefabData;
    private EntityQuery m_PrefabQuery;
    private JobHandle m_ReadDependencies;
    private JobHandle m_WriteDependencies;
    private TriggerPrefabSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<TriggerData>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabData = new TriggerPrefabData(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PrefabQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabData.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TriggerData_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle jobHandle = new TriggerPrefabSystem.UpdateTriggerPrefabDataJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_TriggerType = this.__TypeHandle.__Game_Prefabs_TriggerData_RO_BufferTypeHandle,
        m_TriggerPrefabData = this.m_PrefabData
      }.Schedule<TriggerPrefabSystem.UpdateTriggerPrefabDataJob>(this.m_PrefabQuery, JobHandle.CombineDependencies(this.Dependency, this.m_ReadDependencies));
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = jobHandle;
      this.Dependency = jobHandle;
    }

    public TriggerPrefabData ReadTriggerPrefabData(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_PrefabData;
    }

    public void AddReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies = JobHandle.CombineDependencies(this.m_ReadDependencies, handle);
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
    public TriggerPrefabSystem()
    {
    }

    [BurstCompile]
    private struct UpdateTriggerPrefabDataJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public BufferTypeHandle<TriggerData> m_TriggerType;
      public TriggerPrefabData m_TriggerPrefabData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TriggerData> bufferAccessor = chunk.GetBufferAccessor<TriggerData>(ref this.m_TriggerType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
          for (int index1 = 0; index1 < nativeArray.Length; ++index1)
          {
            Entity prefab = nativeArray[index1];
            DynamicBuffer<TriggerData> dynamicBuffer = bufferAccessor[index1];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_TriggerPrefabData.RemovePrefab(prefab, dynamicBuffer[index2]);
            }
          }
        }
        else
        {
          for (int index3 = 0; index3 < nativeArray.Length; ++index3)
          {
            Entity prefab = nativeArray[index3];
            DynamicBuffer<TriggerData> dynamicBuffer = bufferAccessor[index3];
            for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_TriggerPrefabData.AddPrefab(prefab, dynamicBuffer[index4]);
            }
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
      public BufferTypeHandle<TriggerData> __Game_Prefabs_TriggerData_RO_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TriggerData_RO_BufferTypeHandle = state.GetBufferTypeHandle<TriggerData>(true);
      }
    }
  }
}
