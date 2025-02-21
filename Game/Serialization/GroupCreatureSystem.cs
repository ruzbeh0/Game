// Decompiled with JetBrains decompiler
// Type: Game.Serialization.GroupCreatureSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Creatures;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class GroupCreatureSystem : GameSystemBase
  {
    private EntityQuery m_Query;
    private GroupCreatureSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(ComponentType.ReadOnly<GroupMember>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Query);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_GroupCreature_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      GroupCreatureSystem.GroupCreatureJob jobData = new GroupCreatureSystem.GroupCreatureJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_GroupMemberType = this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentTypeHandle,
        m_GroupCreatures = this.__TypeHandle.__Game_Creatures_GroupCreature_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<GroupCreatureSystem.GroupCreatureJob>(this.m_Query, this.Dependency);
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
    public GroupCreatureSystem()
    {
    }

    [BurstCompile]
    private struct GroupCreatureJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<GroupMember> m_GroupMemberType;
      public BufferLookup<GroupCreature> m_GroupCreatures;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<GroupMember> nativeArray2 = chunk.GetNativeArray<GroupMember>(ref this.m_GroupMemberType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity creature = nativeArray1[index];
          // ISSUE: reference to a compiler-generated field
          this.m_GroupCreatures[nativeArray2[index].m_Leader].Add(new GroupCreature(creature));
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
      public ComponentTypeHandle<GroupMember> __Game_Creatures_GroupMember_RO_ComponentTypeHandle;
      public BufferLookup<GroupCreature> __Game_Creatures_GroupCreature_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupMember_RO_ComponentTypeHandle = state.GetComponentTypeHandle<GroupMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupCreature_RW_BufferLookup = state.GetBufferLookup<GroupCreature>();
      }
    }
  }
}
