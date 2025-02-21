// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaitingPassengersSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Creatures;
using Game.Pathfind;
using Game.Routes;
using Game.Tools;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class WaitingPassengersSystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_StopQuery;
    private EntityQuery m_ResidentQuery;
    private WaitingPassengersSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetExistingSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_StopQuery = this.GetEntityQuery(ComponentType.ReadWrite<WaitingPassengers>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentQuery = this.GetEntityQuery(ComponentType.ReadOnly<HumanCurrentLane>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<GroupMember>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_StopQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_WaitingPassengers_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WaitingPassengersSystem.ClearWaitingPassengersJob jobData1 = new WaitingPassengersSystem.ClearWaitingPassengersJob()
      {
        m_WaitingPassengersType = this.__TypeHandle.__Game_Routes_WaitingPassengers_RW_ComponentTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_WaitingPassengers_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Queue_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_GroupCreature_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      WaitingPassengersSystem.CountWaitingPassengersJob jobData2 = new WaitingPassengersSystem.CountWaitingPassengersJob()
      {
        m_HumanCurrentLaneType = this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle,
        m_ResidentType = this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RO_ComponentTypeHandle,
        m_PathElementType = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferTypeHandle,
        m_GroupCreatureType = this.__TypeHandle.__Game_Creatures_GroupCreature_RO_BufferTypeHandle,
        m_QueueType = this.__TypeHandle.__Game_Creatures_Queue_RO_BufferTypeHandle,
        m_WaitingPassengersData = this.__TypeHandle.__Game_Routes_WaitingPassengers_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_WaitingPassengers_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WaitingPassengersSystem.TickWaitingPassengersJob jobData3 = new WaitingPassengersSystem.TickWaitingPassengersJob()
      {
        m_RandomSeed = RandomSeed.Next(),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_WaitingPassengersType = this.__TypeHandle.__Game_Routes_WaitingPassengers_RW_ComponentTypeHandle,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn1 = jobData1.ScheduleParallel<WaitingPassengersSystem.ClearWaitingPassengersJob>(this.m_StopQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData2.ScheduleParallel<WaitingPassengersSystem.CountWaitingPassengersJob>(this.m_ResidentQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      EntityQuery stopQuery = this.m_StopQuery;
      JobHandle dependsOn2 = jobHandle;
      JobHandle producerJob = jobData3.ScheduleParallel<WaitingPassengersSystem.TickWaitingPassengersJob>(stopQuery, dependsOn2);
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
    public WaitingPassengersSystem()
    {
    }

    [BurstCompile]
    private struct ClearWaitingPassengersJob : IJobChunk
    {
      public ComponentTypeHandle<WaitingPassengers> m_WaitingPassengersType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaitingPassengers> nativeArray = chunk.GetNativeArray<WaitingPassengers>(ref this.m_WaitingPassengersType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          ref WaitingPassengers local = ref nativeArray.ElementAt<WaitingPassengers>(index);
          local.m_Count = 0;
          local.m_OngoingAccumulation = 0;
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

    [BurstCompile]
    private struct CountWaitingPassengersJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<HumanCurrentLane> m_HumanCurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Resident> m_ResidentType;
      [ReadOnly]
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      [ReadOnly]
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public BufferTypeHandle<GroupCreature> m_GroupCreatureType;
      [ReadOnly]
      public BufferTypeHandle<Queue> m_QueueType;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<WaitingPassengers> m_WaitingPassengersData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<HumanCurrentLane> nativeArray1 = chunk.GetNativeArray<HumanCurrentLane>(ref this.m_HumanCurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Resident> nativeArray2 = chunk.GetNativeArray<Resident>(ref this.m_ResidentType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray3 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<PathElement> bufferAccessor1 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<GroupCreature> bufferAccessor2 = chunk.GetBufferAccessor<GroupCreature>(ref this.m_GroupCreatureType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Queue> bufferAccessor3 = chunk.GetBufferAccessor<Queue>(ref this.m_QueueType);
        Entity lastStop = Entity.Null;
        int2 accumulation = (int2) 0;
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          HumanCurrentLane currentLane = nativeArray1[index1];
          DynamicBuffer<GroupCreature> groupCreatures = new DynamicBuffer<GroupCreature>();
          if (bufferAccessor2.Length != 0)
            groupCreatures = bufferAccessor2[index1];
          Resident resident = new Resident();
          if (nativeArray2.Length != 0)
            resident = nativeArray2[index1];
          if (CreatureUtils.TransportStopReached(currentLane))
          {
            PathOwner pathOwner = nativeArray3[index1];
            DynamicBuffer<PathElement> dynamicBuffer = bufferAccessor1[index1];
            if (dynamicBuffer.Length >= pathOwner.m_ElementIndex + 2)
            {
              Entity target = dynamicBuffer[pathOwner.m_ElementIndex].m_Target;
              if (target != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated method
                this.AddPassengers(target, resident, groupCreatures, ref lastStop, ref accumulation);
              }
            }
          }
          else
          {
            DynamicBuffer<Queue> dynamicBuffer = bufferAccessor3[index1];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              Queue queue = dynamicBuffer[index2];
              if ((double) queue.m_TargetArea.radius > 0.0)
              {
                Entity targetEntity = queue.m_TargetEntity;
                if (targetEntity != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddPassengers(targetEntity, resident, groupCreatures, ref lastStop, ref accumulation);
                }
              }
            }
          }
        }
        if (!(lastStop != Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated method
        this.AddPassengers(Entity.Null, new Resident(), new DynamicBuffer<GroupCreature>(), ref lastStop, ref accumulation);
      }

      private void AddPassengers(
        Entity stop,
        Resident resident,
        DynamicBuffer<GroupCreature> groupCreatures,
        ref Entity lastStop,
        ref int2 accumulation)
      {
        if (stop != lastStop)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_WaitingPassengersData.HasComponent(lastStop))
          {
            // ISSUE: reference to a compiler-generated field
            ref WaitingPassengers local = ref this.m_WaitingPassengersData.GetRefRW(lastStop).ValueRW;
            Interlocked.Add(ref local.m_Count, accumulation.x);
            Interlocked.Add(ref local.m_OngoingAccumulation, accumulation.y);
          }
          lastStop = stop;
          accumulation = (int2) 0;
        }
        int2 int2;
        int2.x = 1;
        if (groupCreatures.IsCreated)
          int2.x += groupCreatures.Length;
        int2.y = (int) ((double) (resident.m_Timer * int2.x) * 0.13333334028720856);
        accumulation += int2;
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

    [BurstCompile]
    private struct TickWaitingPassengersJob : IJobChunk
    {
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<WaitingPassengers> m_WaitingPassengersType;
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
        NativeArray<WaitingPassengers> nativeArray2 = chunk.GetNativeArray<WaitingPassengers>(ref this.m_WaitingPassengersType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          ref WaitingPassengers local = ref nativeArray2.ElementAt<WaitingPassengers>(index);
          if (random.NextInt(64) == 0 && local.m_SuccessAccumulation < ushort.MaxValue)
            ++local.m_SuccessAccumulation;
          int2 int2_1 = new int2(local.m_OngoingAccumulation, local.m_ConcludedAccumulation);
          int2 y = new int2(local.m_Count, (int) local.m_SuccessAccumulation);
          y = math.max((int2) 1, y);
          int2 int2_2 = y;
          int2 x = (int2_1 + int2_2 - 1) / y;
          int num1 = math.cmax(x);
          int num2 = math.min((int) ushort.MaxValue, num1 - num1 % 5);
          if (num2 != (int) local.m_AverageWaitingTime)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<PathfindUpdated>(unfilteredChunkIndex, nativeArray1[index], new PathfindUpdated());
          }
          int num3 = (int) local.m_SuccessAccumulation + random.NextInt(256) >> 8;
          local.m_ConcludedAccumulation = math.max(0, local.m_ConcludedAccumulation - num3 * x.y);
          local.m_SuccessAccumulation -= (ushort) num3;
          local.m_AverageWaitingTime = (ushort) num2;
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
      public ComponentTypeHandle<WaitingPassengers> __Game_Routes_WaitingPassengers_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HumanCurrentLane> __Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Resident> __Game_Creatures_Resident_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<PathElement> __Game_Pathfind_PathElement_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<GroupCreature> __Game_Creatures_GroupCreature_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Queue> __Game_Creatures_Queue_RO_BufferTypeHandle;
      public ComponentLookup<WaitingPassengers> __Game_Routes_WaitingPassengers_RW_ComponentLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_WaitingPassengers_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WaitingPassengers>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HumanCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Resident_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Resident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupCreature_RO_BufferTypeHandle = state.GetBufferTypeHandle<GroupCreature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Queue_RO_BufferTypeHandle = state.GetBufferTypeHandle<Queue>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_WaitingPassengers_RW_ComponentLookup = state.GetComponentLookup<WaitingPassengers>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
      }
    }
  }
}
