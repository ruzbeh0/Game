// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ObjectBuiltRequirementSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Common;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class ObjectBuiltRequirementSystem : GameSystemBase
  {
    private ModificationEndBarrier m_ModificationEndBarrier;
    private EntityQuery m_ChangedQuery;
    private EntityQuery m_AllQuery;
    private EntityArchetype m_UnlockEventArchetype;
    private bool m_Loaded;
    private ObjectBuiltRequirementSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationEndBarrier = this.World.GetOrCreateSystemManaged<ModificationEndBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ChangedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Native>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AllQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Native>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery query = this.GetLoaded() ? this.m_AllQuery : this.m_ChangedQuery;
      if (query.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectBuiltRequirementData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UnlockRequirementData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UnlockOnBuildData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      ObjectBuiltRequirementSystem.UnlockOnBuildJob jobData = new ObjectBuiltRequirementSystem.UnlockOnBuildJob()
      {
        m_UnlockEventArchetype = this.m_UnlockEventArchetype,
        m_Buffer = this.m_ModificationEndBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PrefabRefTypeHandle = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_DeletedTypeHandle = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_LockedDataFromEntity = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup,
        m_UnlockOnBuildDataFromEntity = this.__TypeHandle.__Game_Prefabs_UnlockOnBuildData_RO_BufferLookup,
        m_UnlockRequirementDataFromEntity = this.__TypeHandle.__Game_Prefabs_UnlockRequirementData_RW_ComponentLookup,
        m_UnlockOnBuildRequirementDataFromEntity = this.__TypeHandle.__Game_Prefabs_ObjectBuiltRequirementData_RO_ComponentLookup
      };
      this.Dependency = jobData.ScheduleParallel<ObjectBuiltRequirementSystem.UnlockOnBuildJob>(query, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationEndBarrier.AddJobHandleForProducer(this.Dependency);
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
    public ObjectBuiltRequirementSystem()
    {
    }

    [BurstCompile]
    private struct UnlockOnBuildJob : IJobChunk
    {
      public EntityArchetype m_UnlockEventArchetype;
      public EntityCommandBuffer.ParallelWriter m_Buffer;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedTypeHandle;
      [ReadOnly]
      public ComponentLookup<Locked> m_LockedDataFromEntity;
      [ReadOnly]
      public BufferLookup<UnlockOnBuildData> m_UnlockOnBuildDataFromEntity;
      [ReadOnly]
      public ComponentLookup<UnlockRequirementData> m_UnlockRequirementDataFromEntity;
      [ReadOnly]
      public ComponentLookup<ObjectBuiltRequirementData> m_UnlockOnBuildRequirementDataFromEntity;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefTypeHandle);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          DynamicBuffer<UnlockOnBuildData> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_UnlockOnBuildDataFromEntity.TryGetBuffer(nativeArray[index1].m_Prefab, out bufferData))
          {
            // ISSUE: reference to a compiler-generated field
            bool flag = chunk.Has<Deleted>(ref this.m_DeletedTypeHandle);
            for (int index2 = 0; index2 < bufferData.Length; ++index2)
            {
              Entity entity1 = bufferData[index2].m_Entity;
              // ISSUE: reference to a compiler-generated field
              ObjectBuiltRequirementData builtRequirementData = this.m_UnlockOnBuildRequirementDataFromEntity[entity1];
              // ISSUE: reference to a compiler-generated field
              UnlockRequirementData component = this.m_UnlockRequirementDataFromEntity[entity1];
              int y = math.max(component.m_Progress + (flag ? -1 : 1), 0);
              component.m_Progress = math.min(builtRequirementData.m_MinimumCount, y);
              // ISSUE: reference to a compiler-generated field
              this.m_Buffer.SetComponent<UnlockRequirementData>(unfilteredChunkIndex, entity1, component);
              // ISSUE: reference to a compiler-generated field
              if (this.m_LockedDataFromEntity.HasEnabledComponent<Locked>(entity1) && builtRequirementData.m_MinimumCount <= y)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity entity2 = this.m_Buffer.CreateEntity(index1, this.m_UnlockEventArchetype);
                // ISSUE: reference to a compiler-generated field
                this.m_Buffer.SetComponent<Unlock>(index1, entity2, new Unlock(entity1));
              }
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Locked> __Game_Prefabs_Locked_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<UnlockOnBuildData> __Game_Prefabs_UnlockOnBuildData_RO_BufferLookup;
      public ComponentLookup<UnlockRequirementData> __Game_Prefabs_UnlockRequirementData_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectBuiltRequirementData> __Game_Prefabs_ObjectBuiltRequirementData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentLookup = state.GetComponentLookup<Locked>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UnlockOnBuildData_RO_BufferLookup = state.GetBufferLookup<UnlockOnBuildData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UnlockRequirementData_RW_ComponentLookup = state.GetComponentLookup<UnlockRequirementData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectBuiltRequirementData_RO_ComponentLookup = state.GetComponentLookup<ObjectBuiltRequirementData>(true);
      }
    }
  }
}
