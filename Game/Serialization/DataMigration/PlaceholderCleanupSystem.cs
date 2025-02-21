// Decompiled with JetBrains decompiler
// Type: Game.Serialization.DataMigration.PlaceholderCleanupSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Common;
using Game.Notifications;
using Game.Objects;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Serialization.DataMigration
{
  [CompilerGenerated]
  public class PlaceholderCleanupSystem : GameSystemBase
  {
    private LoadGameSystem m_LoadGameSystem;
    private DeserializationBarrier m_DeserializationBarrier;
    private EntityQuery m_Query;
    private ComponentTypeSet m_ComponentSet;
    private PlaceholderCleanupSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadGameSystem = this.World.GetOrCreateSystemManaged<LoadGameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DeserializationBarrier = this.World.GetOrCreateSystemManaged<DeserializationBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(ComponentType.ReadOnly<Placeholder>(), ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<Renter>());
      // ISSUE: reference to a compiler-generated field
      this.m_ComponentSet = new ComponentTypeSet(new ComponentType[7]
      {
        ComponentType.ReadWrite<Renter>(),
        ComponentType.ReadWrite<PropertyToBeOnMarket>(),
        ComponentType.ReadWrite<PropertyOnMarket>(),
        ComponentType.ReadWrite<ElectricityConsumer>(),
        ComponentType.ReadWrite<WaterConsumer>(),
        ComponentType.ReadWrite<GarbageProducer>(),
        ComponentType.ReadWrite<TelecomConsumer>()
      });
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoadGameSystem.context.version >= Version.placeholderCleanup || this.m_Query.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle producerJob = new PlaceholderCleanupSystem.PlaceholderCleanupJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_IconElementType = this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferTypeHandle,
        m_ComponentSet = this.m_ComponentSet,
        m_CommandBuffer = this.m_DeserializationBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<PlaceholderCleanupSystem.PlaceholderCleanupJob>(this.m_Query, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_DeserializationBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
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
    public PlaceholderCleanupSystem()
    {
    }

    [BurstCompile]
    private struct PlaceholderCleanupJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<IconElement> m_IconElementType;
      [ReadOnly]
      public ComponentTypeSet m_ComponentSet;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<IconElement> bufferAccessor = chunk.GetBufferAccessor<IconElement>(ref this.m_IconElementType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent(unfilteredChunkIndex, nativeArray[index1], in this.m_ComponentSet);
          DynamicBuffer<IconElement> dynamicBuffer;
          if (CollectionUtils.TryGet<IconElement>(bufferAccessor, index1, out dynamicBuffer))
          {
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, dynamicBuffer[index2].m_Icon, new Deleted());
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
      public BufferTypeHandle<IconElement> __Game_Notifications_IconElement_RO_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_IconElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<IconElement>(true);
      }
    }
  }
}
