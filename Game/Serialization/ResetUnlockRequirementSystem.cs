// Decompiled with JetBrains decompiler
// Type: Game.Serialization.ResetUnlockRequirementSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class ResetUnlockRequirementSystem : GameSystemBase
  {
    private EntityQuery m_RequirementQuery;
    private ResetUnlockRequirementSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RequirementQuery = this.GetEntityQuery(ComponentType.ReadOnly<UnlockRequirementData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_RequirementQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ResetUnlockRequirementSystem.ResetUnlockRequirementJob jobData = new ResetUnlockRequirementSystem.ResetUnlockRequirementJob()
      {
        m_UnlockRequirementDataType = this.__TypeHandle.__Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<ResetUnlockRequirementSystem.ResetUnlockRequirementJob>(this.m_RequirementQuery, this.Dependency);
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
    public ResetUnlockRequirementSystem()
    {
    }

    [BurstCompile]
    private struct ResetUnlockRequirementJob : IJobChunk
    {
      public ComponentTypeHandle<UnlockRequirementData> m_UnlockRequirementDataType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<UnlockRequirementData> nativeArray = chunk.GetNativeArray<UnlockRequirementData>(ref this.m_UnlockRequirementDataType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          UnlockRequirementData unlockRequirementData = nativeArray[index] with
          {
            m_Progress = 0
          };
          nativeArray[index] = unlockRequirementData;
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
      public ComponentTypeHandle<UnlockRequirementData> __Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnlockRequirementData>();
      }
    }
  }
}
