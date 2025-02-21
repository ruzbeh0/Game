// Decompiled with JetBrains decompiler
// Type: Game.Objects.ContainerClearSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Effects;
using Game.Prefabs;
using Game.Serialization;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  [CompilerGenerated]
  public class ContainerClearSystem : GameSystemBase
  {
    private LoadGameSystem m_LoadGameSystem;
    private EntityQuery m_EntityQuery;
    private ComponentTypeSet m_SubTypes;
    private ContainerClearSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadGameSystem = this.World.GetOrCreateSystemManaged<LoadGameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EntityQuery = this.GetEntityQuery(ComponentType.ReadOnly<SubObject>(), ComponentType.ReadOnly<Game.Net.SubNet>(), ComponentType.ReadOnly<Game.Areas.SubArea>(), ComponentType.ReadOnly<Object>(), ComponentType.Exclude<Building>());
      // ISSUE: reference to a compiler-generated field
      this.m_SubTypes = new ComponentTypeSet(ComponentType.ReadWrite<SubObject>(), ComponentType.ReadWrite<Game.Net.SubNet>(), ComponentType.ReadWrite<Game.Areas.SubArea>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EntityQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoadGameSystem.context.purpose != Colossal.Serialization.Entities.Purpose.NewGame)
        return;
      EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Effects_EnabledEffect_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      new ContainerClearSystem.ContainerClearJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_EffectOwnerType = this.__TypeHandle.__Game_Effects_EnabledEffect_RO_BufferTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_SubObjectType = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle,
        m_SubNetType = this.__TypeHandle.__Game_Net_SubNet_RO_BufferTypeHandle,
        m_SubAreaType = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferTypeHandle,
        m_PrefabEffects = this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup,
        m_PrefabSubObjects = this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup,
        m_PrefabSubNets = this.__TypeHandle.__Game_Prefabs_SubNet_RO_BufferLookup,
        m_PrefabSubAreas = this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup,
        m_SubTypes = this.m_SubTypes,
        m_CommandBuffer = entityCommandBuffer.AsParallelWriter()
      }.ScheduleParallel<ContainerClearSystem.ContainerClearJob>(this.m_EntityQuery, this.Dependency).Complete();
      entityCommandBuffer.Playback(this.EntityManager);
      entityCommandBuffer.Dispose();
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
    public ContainerClearSystem()
    {
    }

    [BurstCompile]
    private struct ContainerClearJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<EnabledEffect> m_EffectOwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<SubObject> m_SubObjectType;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubNet> m_SubNetType;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.SubArea> m_SubAreaType;
      [ReadOnly]
      public BufferLookup<Effect> m_PrefabEffects;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubObject> m_PrefabSubObjects;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubNet> m_PrefabSubNets;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> m_PrefabSubAreas;
      [ReadOnly]
      public ComponentTypeSet m_SubTypes;
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
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubObject> bufferAccessor1 = chunk.GetBufferAccessor<SubObject>(ref this.m_SubObjectType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Net.SubNet> bufferAccessor2 = chunk.GetBufferAccessor<Game.Net.SubNet>(ref this.m_SubNetType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Areas.SubArea> bufferAccessor3 = chunk.GetBufferAccessor<Game.Areas.SubArea>(ref this.m_SubAreaType);
        // ISSUE: reference to a compiler-generated field
        bool flag = chunk.Has<EnabledEffect>(ref this.m_EffectOwnerType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity e = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          DynamicBuffer<SubObject> dynamicBuffer1 = bufferAccessor1[index];
          DynamicBuffer<Game.Net.SubNet> dynamicBuffer2 = bufferAccessor2[index];
          DynamicBuffer<Game.Areas.SubArea> dynamicBuffer3 = bufferAccessor3[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool3 x = (bool3) false with
          {
            x = dynamicBuffer1.Length == 0 && !this.m_PrefabSubObjects.HasBuffer(prefabRef.m_Prefab),
            y = dynamicBuffer2.Length == 0 && !this.m_PrefabSubNets.HasBuffer(prefabRef.m_Prefab),
            z = dynamicBuffer3.Length == 0 && !this.m_PrefabSubAreas.HasBuffer(prefabRef.m_Prefab)
          };
          if (math.all(x))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent(unfilteredChunkIndex, e, in this.m_SubTypes);
          }
          else
          {
            if (x.x)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<SubObject>(unfilteredChunkIndex, e);
            }
            if (x.y)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Game.Net.SubNet>(unfilteredChunkIndex, e);
            }
            if (x.z)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Game.Areas.SubArea>(unfilteredChunkIndex, e);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (!flag && this.m_PrefabEffects.HasBuffer(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddBuffer<EnabledEffect>(unfilteredChunkIndex, e);
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
      public BufferTypeHandle<EnabledEffect> __Game_Effects_EnabledEffect_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubObject> __Game_Objects_SubObject_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferLookup<Effect> __Game_Prefabs_Effect_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubObject> __Game_Prefabs_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubNet> __Game_Prefabs_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> __Game_Prefabs_SubArea_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Effects_EnabledEffect_RO_BufferTypeHandle = state.GetBufferTypeHandle<EnabledEffect>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Effect_RO_BufferLookup = state.GetBufferLookup<Effect>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubArea>(true);
      }
    }
  }
}
