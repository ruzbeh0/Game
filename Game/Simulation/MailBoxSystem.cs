// Decompiled with JetBrains decompiler
// Type: Game.Simulation.MailBoxSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class MailBoxSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 512;
    private EntityQuery m_MailBoxQuery;
    private EntityQuery m_PostConfigurationQuery;
    private EntityArchetype m_PostVanRequestArchetype;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private MailBoxSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 512;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_MailBoxQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Routes.MailBox>(), ComponentType.ReadOnly<Game.Routes.TransportStop>(), ComponentType.Exclude<Game.Buildings.PostFacility>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>());
      // ISSUE: reference to a compiler-generated field
      this.m_PostConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<PostConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PostVanRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<PostVanRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_MailBoxQuery);
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_PostVanRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_MailBox_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new MailBoxSystem.MailBoxTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_MailBoxType = this.__TypeHandle.__Game_Routes_MailBox_RO_ComponentTypeHandle,
        m_PostVanRequestData = this.__TypeHandle.__Game_Simulation_PostVanRequest_RO_ComponentLookup,
        m_PostVanRequestArchetype = this.m_PostVanRequestArchetype,
        m_PostConfigurationData = this.m_PostConfigurationQuery.GetSingleton<PostConfigurationData>(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<MailBoxSystem.MailBoxTickJob>(this.m_MailBoxQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(producerJob);
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
    public MailBoxSystem()
    {
    }

    [BurstCompile]
    private struct MailBoxTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.MailBox> m_MailBoxType;
      [ReadOnly]
      public ComponentLookup<PostVanRequest> m_PostVanRequestData;
      [ReadOnly]
      public EntityArchetype m_PostVanRequestArchetype;
      [ReadOnly]
      public PostConfigurationData m_PostConfigurationData;
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
        NativeArray<Game.Routes.MailBox> nativeArray2 = chunk.GetNativeArray<Game.Routes.MailBox>(ref this.m_MailBoxType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Game.Routes.MailBox mailBox = nativeArray2[index];
          // ISSUE: reference to a compiler-generated method
          this.RequestPostVanIfNeeded(unfilteredChunkIndex, entity, mailBox);
        }
      }

      private void RequestPostVanIfNeeded(int jobIndex, Entity entity, Game.Routes.MailBox mailBox)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (mailBox.m_MailAmount < this.m_PostConfigurationData.m_MailAccumulationTolerance || this.m_PostVanRequestData.HasComponent(mailBox.m_CollectRequest))
          return;
        PostVanRequestFlags flags = PostVanRequestFlags.Collect | PostVanRequestFlags.MailBoxTarget;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_PostVanRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PostVanRequest>(jobIndex, entity1, new PostVanRequest(entity, flags, (ushort) math.min((int) ushort.MaxValue, mailBox.m_MailAmount)));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(32U));
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
      public ComponentTypeHandle<Game.Routes.MailBox> __Game_Routes_MailBox_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PostVanRequest> __Game_Simulation_PostVanRequest_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_MailBox_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Routes.MailBox>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_PostVanRequest_RO_ComponentLookup = state.GetComponentLookup<PostVanRequest>(true);
      }
    }
  }
}
