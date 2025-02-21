// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UnlockSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Logging;
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
  public class UnlockSystem : GameSystemBase
  {
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_LockedQuery;
    private EntityQuery m_UpdatedQuery;
    private EntityQuery m_EventQuery;
    private EntityArchetype m_UnlockEventArchetype;
    private bool m_Loaded;
    private ILog m_Log;
    private UnlockSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LockedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Locked>(), ComponentType.ReadOnly<UnlockRequirement>());
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Locked>(), ComponentType.ReadOnly<UnlockRequirement>(), ComponentType.ReadOnly<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_EventQuery = this.GetEntityQuery(ComponentType.ReadOnly<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.m_Log = LogManager.GetLogger("Unlocking");
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      this.Enabled = mode.IsGame();
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context context)
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
      bool loaded = this.GetLoaded();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      if (!this.ProcessEvents() && !loaded && this.m_UpdatedQuery.IsEmptyIgnoreFilter)
        return;
      NativeQueue<Entity> nativeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
label_3:
      try
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_UnlockRequirement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        new UnlockSystem.CheckUnlockRequirementsJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_UnlockRequirementType = this.__TypeHandle.__Game_Prefabs_UnlockRequirement_RO_BufferTypeHandle,
          m_LockedData = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup,
          m_UnlockQueue = nativeQueue.AsParallelWriter()
        }.ScheduleParallel<UnlockSystem.CheckUnlockRequirementsJob>(this.m_LockedQuery, new JobHandle()).Complete();
        if (nativeQueue.Count == 0)
          return;
        while (true)
        {
          Entity unlock;
          if (nativeQueue.TryDequeue(out unlock))
          {
            // ISSUE: reference to a compiler-generated method
            this.UnlockPrefab(unlock, true);
          }
          else
            goto label_3;
        }
      }
      finally
      {
        nativeQueue.Dispose();
      }
    }

    private bool ProcessEvents()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_EventQuery.IsEmptyIgnoreFilter)
        return false;
      bool flag = false;
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<Unlock> componentDataArray = this.m_EventQuery.ToComponentDataArray<Unlock>((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        for (int index = 0; index < componentDataArray.Length; ++index)
        {
          Entity prefab = componentDataArray[index].m_Prefab;
          if (this.EntityManager.HasEnabledComponent<Locked>(prefab))
          {
            // ISSUE: reference to a compiler-generated method
            this.UnlockPrefab(prefab, false);
            flag = true;
          }
        }
      }
      return flag;
    }

    private void UnlockPrefab(Entity unlock, bool createEvent)
    {
      this.EntityManager.SetComponentEnabled<Locked>(unlock, false);
      if (createEvent)
      {
        // ISSUE: reference to a compiler-generated field
        this.EntityManager.SetComponentData<Unlock>(this.EntityManager.CreateEntity(this.m_UnlockEventArchetype), new Unlock(unlock));
      }
      if (!this.EntityManager.HasEnabledComponent<PrefabData>(unlock))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_Log.DebugFormat("Prefab unlocked: {0}", (object) this.m_PrefabSystem.GetObsoleteID(unlock));
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_Log.DebugFormat("Prefab unlocked: {0}", (object) this.m_PrefabSystem.GetPrefab<PrefabBase>(unlock));
      }
    }

    public bool IsLocked(PrefabBase prefab)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return this.m_PrefabSystem.HasEnabledComponent<Locked>(prefab);
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
    public UnlockSystem()
    {
    }

    [BurstCompile]
    private struct CheckUnlockRequirementsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<UnlockRequirement> m_UnlockRequirementType;
      [ReadOnly]
      public ComponentLookup<Locked> m_LockedData;
      public NativeQueue<Entity>.ParallelWriter m_UnlockQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<UnlockRequirement> bufferAccessor = chunk.GetBufferAccessor<UnlockRequirement>(ref this.m_UnlockRequirementType);
        ChunkEntityEnumerator entityEnumerator = new ChunkEntityEnumerator(useEnabledMask, chunkEnabledMask, chunk.Count);
        int nextIndex;
        while (entityEnumerator.NextEntityIndex(out nextIndex))
        {
          DynamicBuffer<UnlockRequirement> dynamicBuffer = bufferAccessor[nextIndex];
          bool flag1 = false;
          bool flag2 = false;
          bool flag3 = false;
          for (int index = 0; index < dynamicBuffer.Length && !flag1; ++index)
          {
            UnlockRequirement unlockRequirement = dynamicBuffer[index];
            // ISSUE: reference to a compiler-generated field
            bool flag4 = this.m_LockedData.HasEnabledComponent<Locked>(unlockRequirement.m_Prefab);
            bool flag5 = (unlockRequirement.m_Flags & UnlockFlags.RequireAll) > (UnlockFlags) 0;
            bool flag6 = (unlockRequirement.m_Flags & UnlockFlags.RequireAny) > (UnlockFlags) 0;
            flag1 |= flag4 & flag5;
            flag2 |= flag4 & flag6;
            flag3 |= !flag4 & flag6;
          }
          if (!flag1 && (flag3 || !flag2))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_UnlockQueue.Enqueue(nativeArray[nextIndex]);
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
      public BufferTypeHandle<UnlockRequirement> __Game_Prefabs_UnlockRequirement_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Locked> __Game_Prefabs_Locked_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UnlockRequirement_RO_BufferTypeHandle = state.GetBufferTypeHandle<UnlockRequirement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentLookup = state.GetComponentLookup<Locked>(true);
      }
    }
  }
}
