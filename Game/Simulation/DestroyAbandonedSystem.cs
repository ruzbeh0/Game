// Decompiled with JetBrains decompiler
// Type: Game.Simulation.DestroyAbandonedSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class DestroyAbandonedSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private IconCommandSystem m_IconCommandSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_AbandonedQuery;
    private EntityArchetype m_DamageEventArchetype;
    private EntityArchetype m_DestroyEventArchetype;
    private EntityQuery m_BuildingSettingsQuery;
    private DestroyAbandonedSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 4096;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_AbandonedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<Abandoned>(), ComponentType.Exclude<Destroyed>());
      // ISSUE: reference to a compiler-generated field
      this.m_DamageEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Damage>());
      // ISSUE: reference to a compiler-generated field
      this.m_DestroyEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Destroy>());
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<BuildingConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_AbandonedQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DestroyAbandonedSystem.DestroyAbandonedJob jobData = new DestroyAbandonedSystem.DestroyAbandonedJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_AbandonedType = this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentTypeHandle,
        m_BuildingConfigurationData = this.m_BuildingSettingsQuery.GetSingleton<BuildingConfigurationData>(),
        m_DamageEventArchetype = this.m_DamageEventArchetype,
        m_DestroyEventArchetype = this.m_DestroyEventArchetype,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<DestroyAbandonedSystem.DestroyAbandonedJob>(this.m_AbandonedQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(this.Dependency);
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
    public DestroyAbandonedSystem()
    {
    }

    [BurstCompile]
    private struct DestroyAbandonedJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Abandoned> m_AbandonedType;
      [ReadOnly]
      public EntityArchetype m_DamageEventArchetype;
      [ReadOnly]
      public EntityArchetype m_DestroyEventArchetype;
      [ReadOnly]
      public BuildingConfigurationData m_BuildingConfigurationData;
      [ReadOnly]
      public uint m_SimulationFrame;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public IconCommandBuffer m_IconCommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Abandoned> nativeArray2 = chunk.GetNativeArray<Abandoned>(ref this.m_AbandonedType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (nativeArray2[index].m_AbandonmentTime + this.m_BuildingConfigurationData.m_AbandonedDestroyDelay <= this.m_SimulationFrame)
          {
            Entity entity1 = nativeArray1[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity2 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_DamageEventArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Damage>(unfilteredChunkIndex, entity2, new Damage(entity1, new float3(1f, 0.0f, 0.0f)));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity3 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_DestroyEventArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Destroy>(unfilteredChunkIndex, entity3, new Destroy(entity1, Entity.Null));
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Remove(entity1, IconPriority.Problem);
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Remove(entity1, IconPriority.FatalProblem);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Add(entity1, this.m_BuildingConfigurationData.m_AbandonedCollapsedNotification, IconPriority.FatalProblem);
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
      public ComponentTypeHandle<Abandoned> __Game_Buildings_Abandoned_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Abandoned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Abandoned>(true);
      }
    }
  }
}
