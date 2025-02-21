// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.StrictObjectBuiltRequirementSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class StrictObjectBuiltRequirementSystem : GameSystemBase
  {
    private InstanceCountSystem m_InstanceCountSystem;
    private ModificationEndBarrier m_ModificationEndBarrier;
    private EntityQuery m_ChangedQuery;
    private EntityQuery m_RequirementQuery;
    private EntityArchetype m_UnlockEventArchetype;
    private bool m_Loaded;
    private StrictObjectBuiltRequirementSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_InstanceCountSystem = this.World.GetOrCreateSystemManaged<InstanceCountSystem>();
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
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_RequirementQuery = this.GetEntityQuery(ComponentType.ReadOnly<StrictObjectBuiltRequirementData>(), ComponentType.ReadWrite<UnlockRequirementData>(), ComponentType.ReadOnly<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_RequirementQuery);
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
      if (!this.GetLoaded() && this.m_ChangedQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StrictObjectBuiltRequirementData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      StrictObjectBuiltRequirementSystem.TrackObjectsJob jobData = new StrictObjectBuiltRequirementSystem.TrackObjectsJob()
      {
        m_Buffer = this.m_ModificationEndBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_InstanceCounts = this.m_InstanceCountSystem.GetInstanceCounts(true, out dependencies),
        m_ObjectBuiltRequirementDataHandle = this.__TypeHandle.__Game_Prefabs_StrictObjectBuiltRequirementData_RO_ComponentTypeHandle,
        m_RequirementDataHandle = this.__TypeHandle.__Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle,
        m_EntityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UnlockEventArchetype = this.m_UnlockEventArchetype
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<StrictObjectBuiltRequirementSystem.TrackObjectsJob>(this.m_RequirementQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InstanceCountSystem.AddCountReader(this.Dependency);
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
    public StrictObjectBuiltRequirementSystem()
    {
    }

    [BurstCompile]
    private struct TrackObjectsJob : IJobChunk
    {
      public EntityCommandBuffer.ParallelWriter m_Buffer;
      [ReadOnly]
      public NativeParallelHashMap<Entity, int> m_InstanceCounts;
      [ReadOnly]
      public ComponentTypeHandle<StrictObjectBuiltRequirementData> m_ObjectBuiltRequirementDataHandle;
      public ComponentTypeHandle<UnlockRequirementData> m_RequirementDataHandle;
      [ReadOnly]
      public EntityTypeHandle m_EntityTypeHandle;
      public EntityArchetype m_UnlockEventArchetype;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<StrictObjectBuiltRequirementData> nativeArray1 = chunk.GetNativeArray<StrictObjectBuiltRequirementData>(ref this.m_ObjectBuiltRequirementDataHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<UnlockRequirementData> nativeArray2 = chunk.GetNativeArray<UnlockRequirementData>(ref this.m_RequirementDataHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray3 = chunk.GetNativeArray(this.m_EntityTypeHandle);
        ChunkEntityEnumerator entityEnumerator = new ChunkEntityEnumerator(useEnabledMask, chunkEnabledMask, chunk.Count);
        int nextIndex;
        while (entityEnumerator.NextEntityIndex(out nextIndex))
        {
          int y;
          // ISSUE: reference to a compiler-generated field
          if (this.m_InstanceCounts.TryGetValue(nativeArray1[nextIndex].m_Requirement, out y))
          {
            UnlockRequirementData unlockRequirementData = nativeArray2[nextIndex] with
            {
              m_Progress = math.min(nativeArray1[nextIndex].m_MinimumCount, y)
            };
            nativeArray2[nextIndex] = unlockRequirementData;
            if (nativeArray1[nextIndex].m_MinimumCount <= y)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_Buffer.CreateEntity(unfilteredChunkIndex, this.m_UnlockEventArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_Buffer.SetComponent<Unlock>(unfilteredChunkIndex, entity, new Unlock(nativeArray3[nextIndex]));
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
      public ComponentTypeHandle<StrictObjectBuiltRequirementData> __Game_Prefabs_StrictObjectBuiltRequirementData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<UnlockRequirementData> __Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StrictObjectBuiltRequirementData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<StrictObjectBuiltRequirementData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnlockRequirementData>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
      }
    }
  }
}
