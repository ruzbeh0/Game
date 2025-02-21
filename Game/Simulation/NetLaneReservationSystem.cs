// Decompiled with JetBrains decompiler
// Type: Game.Simulation.NetLaneReservationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Net;
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
  public class NetLaneReservationSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_LaneQuery;
    private NetLaneReservationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Net.LaneReservation>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_LaneQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint index = this.m_SimulationSystem.frameIndex % 16U;
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(index));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneReservation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NetLaneReservationSystem.ResetLaneReservationsJob jobData = new NetLaneReservationSystem.ResetLaneReservationsJob()
      {
        m_LaneReservationType = this.__TypeHandle.__Game_Net_LaneReservation_RW_ComponentTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<NetLaneReservationSystem.ResetLaneReservationsJob>(this.m_LaneQuery, this.Dependency);
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
    public NetLaneReservationSystem()
    {
    }

    [BurstCompile]
    private struct ResetLaneReservationsJob : IJobChunk
    {
      public ComponentTypeHandle<Game.Net.LaneReservation> m_LaneReservationType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.LaneReservation> nativeArray = chunk.GetNativeArray<Game.Net.LaneReservation>(ref this.m_LaneReservationType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          ref Game.Net.LaneReservation local = ref nativeArray.ElementAt<Game.Net.LaneReservation>(index);
          if ((int) local.m_Next.m_Priority < (int) local.m_Prev.m_Priority)
            local.m_Blocker = Entity.Null;
          local.m_Prev = local.m_Next;
          local.m_Next = new ReservationData();
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
      public ComponentTypeHandle<Game.Net.LaneReservation> __Game_Net_LaneReservation_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneReservation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.LaneReservation>();
      }
    }
  }
}
