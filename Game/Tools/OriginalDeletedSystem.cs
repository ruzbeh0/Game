// Decompiled with JetBrains decompiler
// Type: Game.Tools.OriginalDeletedSystem
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
namespace Game.Tools
{
  [CompilerGenerated]
  public class OriginalDeletedSystem : GameSystemBase
  {
    private EntityQuery m_TempQuery;
    private NativeArray<bool> m_OriginalDeleted;
    private JobHandle m_Dependency;
    private OriginalDeletedSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_OriginalDeleted = new NativeArray<bool>(2, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_OriginalDeleted.Dispose();
      base.OnDestroy();
    }

    public bool GetOriginalDeletedResult(int delay)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Dependency.Complete();
      for (int index = 1 - delay; index >= 0; --index)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_OriginalDeleted[index])
          return true;
      }
      return false;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_OriginalDeleted[0] = this.m_OriginalDeleted[1];
      // ISSUE: reference to a compiler-generated field
      this.m_OriginalDeleted[1] = false;
      // ISSUE: reference to a compiler-generated field
      if (this.m_TempQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new OriginalDeletedSystem.OriginalDeletedJob()
      {
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_OriginalDeleted = this.m_OriginalDeleted
      }.ScheduleParallel<OriginalDeletedSystem.OriginalDeletedJob>(this.m_TempQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_Dependency = jobHandle;
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
    public OriginalDeletedSystem()
    {
    }

    [BurstCompile]
    private struct OriginalDeletedJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [NativeDisableParallelForRestriction]
      public NativeArray<bool> m_OriginalDeleted;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          Temp temp = nativeArray[index];
          if (temp.m_Original != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_DeletedData.HasComponent(temp.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_OriginalDeleted[1] = true;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_EntityLookup.Exists(temp.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_OriginalDeleted[0] = true;
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
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
      }
    }
  }
}
