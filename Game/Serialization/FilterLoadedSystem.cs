// Decompiled with JetBrains decompiler
// Type: Game.Serialization.FilterLoadedSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Common;
using Game.Net;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class FilterLoadedSystem : GameSystemBase
  {
    private LoadGameSystem m_LoadGameSystem;
    private EntityQuery m_NetLaneQuery;
    private EntityQuery m_EditorContainerQuery;
    private FilterLoadedSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadGameSystem = this.World.GetOrCreateSystemManaged<LoadGameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetLaneQuery = this.GetEntityQuery(ComponentType.ReadOnly<Lane>(), ComponentType.ReadOnly<Owner>());
      // ISSUE: reference to a compiler-generated field
      this.m_EditorContainerQuery = this.GetEntityQuery(ComponentType.ReadOnly<EditorContainer>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoadGameSystem.context.purpose != Purpose.NewGame || this.m_EditorContainerQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_NetLaneQuery.IsEmptyIgnoreFilter)
      {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated field
        new FilterLoadedSystem.CheckLanesJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_EditorContainerData = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup,
          m_CommandBuffer = entityCommandBuffer.AsParallelWriter()
        }.ScheduleParallel<FilterLoadedSystem.CheckLanesJob>(this.m_NetLaneQuery, this.Dependency).Complete();
        entityCommandBuffer.Playback(this.EntityManager);
        entityCommandBuffer.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.DestroyEntity(this.m_EditorContainerQuery);
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
    public FilterLoadedSystem()
    {
    }

    [BurstCompile]
    private struct CheckLanesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<EditorContainer> m_EditorContainerData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

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
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Owner owner = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_EditorContainerData.HasComponent(owner.m_Owner))
          {
            Entity e = nativeArray1[index];
            Owner componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_OwnerData.TryGetComponent(owner.m_Owner, out componentData))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Owner>(unfilteredChunkIndex, e, componentData);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Owner>(unfilteredChunkIndex, e);
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
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EditorContainer> __Game_Tools_EditorContainer_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentLookup = state.GetComponentLookup<EditorContainer>(true);
      }
    }
  }
}
