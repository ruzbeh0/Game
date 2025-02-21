// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TouristHouseholdBehaviorSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TouristHouseholdBehaviorSystem : GameSystemBase
  {
    private EntityQuery m_TouristHouseholdGroup;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private TouristHouseholdBehaviorSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_TouristHouseholdGroup = this.GetEntityQuery(ComponentType.ReadWrite<TouristHousehold>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<MovingAway>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TouristHouseholdGroup);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint frameWithInterval = SimulationUtils.GetUpdateFrameWithInterval(this.m_SimulationSystem.frameIndex, (uint) this.GetUpdateInterval(SystemUpdatePhase.GameSimulation), 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_LodgingSeeker_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TouristHousehold_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TouristHouseholdBehaviorSystem.TouristHouseholdTickJob jobData = new TouristHouseholdBehaviorSystem.TouristHouseholdTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TouristHouseholdType = this.__TypeHandle.__Game_Citizens_TouristHousehold_RW_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_LodgingSeekerType = this.__TypeHandle.__Game_Citizens_LodgingSeeker_RO_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle,
        m_Buildings = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_RenterBufs = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_UpdateFrameIndex = frameWithInterval
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<TouristHouseholdBehaviorSystem.TouristHouseholdTickJob>(this.m_TouristHouseholdGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
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
    public TouristHouseholdBehaviorSystem()
    {
    }

    [BurstCompile]
    private struct TouristHouseholdTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public ComponentTypeHandle<TouristHousehold> m_TouristHouseholdType;
      [ReadOnly]
      public ComponentTypeHandle<LodgingSeeker> m_LodgingSeekerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Common.Target> m_TargetType;
      [ReadOnly]
      public ComponentLookup<Building> m_Buildings;
      [ReadOnly]
      public BufferLookup<Renter> m_RenterBufs;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public uint m_UpdateFrameIndex;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((int) chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index != (int) this.m_UpdateFrameIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TouristHousehold> nativeArray2 = chunk.GetNativeArray<TouristHousehold>(ref this.m_TouristHouseholdType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Common.Target> nativeArray3 = chunk.GetNativeArray<Game.Common.Target>(ref this.m_TargetType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          if (nativeArray3.Length > 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (nativeArray3[index1].m_Target == Entity.Null || !this.m_Buildings.HasComponent(nativeArray3[index1].m_Target))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Game.Common.Target>(unfilteredChunkIndex, nativeArray1[index1]);
            }
            else
              continue;
          }
          Entity entity = nativeArray1[index1];
          // ISSUE: reference to a compiler-generated field
          if (!chunk.Has<LodgingSeeker>(ref this.m_LodgingSeekerType))
          {
            TouristHousehold touristHousehold = nativeArray2[index1];
            Entity hotel = touristHousehold.m_Hotel;
            // ISSUE: reference to a compiler-generated field
            if (hotel == Entity.Null || !this.m_RenterBufs.HasBuffer(hotel))
            {
              touristHousehold.m_Hotel = Entity.Null;
              nativeArray2[index1] = touristHousehold;
            }
            else
            {
              UnityEngine.Debug.Assert(hotel != Entity.Null);
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Renter> renterBuf = this.m_RenterBufs[hotel];
              bool flag = false;
              for (int index2 = 0; index2 < renterBuf.Length; ++index2)
              {
                if (renterBuf[index2].m_Renter.Equals(entity))
                  flag = true;
              }
              if (!flag)
              {
                touristHousehold.m_Hotel = Entity.Null;
                nativeArray2[index1] = touristHousehold;
              }
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<LodgingSeeker>(unfilteredChunkIndex, entity, new LodgingSeeker());
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
      public ComponentTypeHandle<TouristHousehold> __Game_Citizens_TouristHousehold_RW_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<LodgingSeeker> __Game_Citizens_LodgingSeeker_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Common.Target> __Game_Common_Target_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TouristHousehold_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TouristHousehold>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_LodgingSeeker_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LodgingSeeker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Common.Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
      }
    }
  }
}
