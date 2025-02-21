// Decompiled with JetBrains decompiler
// Type: Game.Objects.ResetOverriddenSystem
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

#nullable disable
namespace Game.Objects
{
  [CompilerGenerated]
  public class ResetOverriddenSystem : GameSystemBase
  {
    private EntityQuery m_OverriddenQuery;
    private ResetOverriddenSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_OverriddenQuery = this.GetEntityQuery(ComponentType.ReadOnly<Updated>(), ComponentType.ReadOnly<Overridden>(), ComponentType.ReadWrite<Tree>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_OverriddenQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      this.Dependency = new ResetOverriddenSystem.ResetOverriddenJob()
      {
        m_TreeType = this.__TypeHandle.__Game_Objects_Tree_RW_ComponentTypeHandle
      }.ScheduleParallel<ResetOverriddenSystem.ResetOverriddenJob>(this.m_OverriddenQuery, this.Dependency);
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
    public ResetOverriddenSystem()
    {
    }

    [BurstCompile]
    private struct ResetOverriddenJob : IJobChunk
    {
      public ComponentTypeHandle<Tree> m_TreeType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Tree> nativeArray = chunk.GetNativeArray<Tree>(ref this.m_TreeType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          Tree tree = nativeArray[index];
          tree.m_State &= ~(TreeState.Teen | TreeState.Adult | TreeState.Elderly | TreeState.Dead | TreeState.Stump);
          tree.m_Growth = (byte) 0;
          nativeArray[index] = tree;
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
      public ComponentTypeHandle<Tree> __Game_Objects_Tree_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Tree>();
      }
    }
  }
}
