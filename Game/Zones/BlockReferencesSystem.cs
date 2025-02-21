// Decompiled with JetBrains decompiler
// Type: Game.Zones.BlockReferencesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Zones
{
  [CompilerGenerated]
  public class BlockReferencesSystem : GameSystemBase
  {
    private EntityQuery m_BlockQuery;
    private BlockReferencesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_BlockQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Block>(),
          ComponentType.ReadOnly<Owner>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BlockQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_SubBlock_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BlockReferencesSystem.UpdateBlockReferencesJob jobData = new BlockReferencesSystem.UpdateBlockReferencesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_Blocks = this.__TypeHandle.__Game_Zones_SubBlock_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<BlockReferencesSystem.UpdateBlockReferencesJob>(this.m_BlockQuery, this.Dependency);
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
    public BlockReferencesSystem()
    {
    }

    [BurstCompile]
    private struct UpdateBlockReferencesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      public BufferLookup<SubBlock> m_Blocks;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Created>(ref this.m_CreatedType))
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity block = nativeArray1[index];
            // ISSUE: reference to a compiler-generated field
            CollectionUtils.TryAddUniqueValue<SubBlock>(this.m_Blocks[nativeArray2[index].m_Owner], new SubBlock(block));
          }
        }
        else
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity block = nativeArray1[index];
            // ISSUE: reference to a compiler-generated field
            CollectionUtils.RemoveValue<SubBlock>(this.m_Blocks[nativeArray2[index].m_Owner], new SubBlock(block));
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
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      public BufferLookup<SubBlock> __Game_Zones_SubBlock_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_SubBlock_RW_BufferLookup = state.GetBufferLookup<SubBlock>();
      }
    }
  }
}
