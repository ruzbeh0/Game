// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.LaneDataUnknownEscalateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Pathfind
{
  [CompilerGenerated]
  public class LaneDataUnknownEscalateSystem : GameSystemBase
  {
    private ModificationBarrier5 m_ModificationBarrier;
    private EntityQuery m_LaneQuery;
    private LaneDataUnknownEscalateSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<PathfindUpdated>(),
          ComponentType.ReadOnly<Lane>(),
          ComponentType.ReadOnly<Owner>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<CarLane>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_LaneQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle producerJob = new LaneDataUnknownEscalateSystem.LaneDataUnknownEscalateJob()
      {
        m_LaneType = this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle,
        m_SlaveLaneType = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentTypeHandle,
        m_CarLaneType = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_MasterLaneData = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<LaneDataUnknownEscalateSystem.LaneDataUnknownEscalateJob>(this.m_LaneQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(producerJob);
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
    public LaneDataUnknownEscalateSystem()
    {
    }

    [BurstCompile]
    private struct LaneDataUnknownEscalateJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Lane> m_LaneType;
      [ReadOnly]
      public ComponentTypeHandle<SlaveLane> m_SlaveLaneType;
      [ReadOnly]
      public ComponentTypeHandle<CarLane> m_CarLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<MasterLane> m_MasterLaneData;
      [ReadOnly]
      public ComponentLookup<ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<CarLane> m_CarLaneData;
      [ReadOnly]
      public BufferLookup<SubLane> m_SubLanes;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Lane> nativeArray1 = chunk.GetNativeArray<Lane>(ref this.m_LaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<SlaveLane> nativeArray2 = chunk.GetNativeArray<SlaveLane>(ref this.m_SlaveLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarLane> nativeArray3 = chunk.GetNativeArray<CarLane>(ref this.m_CarLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray4 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        if (nativeArray2.Length != 0)
        {
          for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
          {
            SlaveLane slaveLane = nativeArray2[index1];
            CarLane carLane = nativeArray3[index1];
            Owner owner = nativeArray4[index1];
            Lane lane = new Lane();
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<SubLane> subLane1 = this.m_SubLanes[owner.m_Owner];
            for (int index2 = 0; index2 < subLane1.Length; ++index2)
            {
              Entity subLane2 = subLane1[index2].m_SubLane;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CarLaneData.HasComponent(subLane2) && (int) this.m_CarLaneData[subLane2].m_CarriagewayGroup == (int) carLane.m_CarriagewayGroup)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<PathfindUpdated>(unfilteredChunkIndex, subLane2, new PathfindUpdated());
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_MasterLaneData.HasComponent(subLane2) && (int) this.m_MasterLaneData[subLane2].m_Group == (int) slaveLane.m_Group)
                {
                  // ISSUE: reference to a compiler-generated field
                  lane = this.m_LaneData[subLane2];
                }
              }
            }
            for (int index3 = 0; index3 < subLane1.Length; ++index3)
            {
              Entity subLane3 = subLane1[index3].m_SubLane;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_LaneData[subLane3].m_StartNode.EqualsIgnoreCurvePos(lane.m_MiddleNode) && this.m_ParkingLaneData.HasComponent(subLane3))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<PathfindUpdated>(unfilteredChunkIndex, subLane3, new PathfindUpdated());
              }
            }
          }
        }
        else
        {
          for (int index4 = 0; index4 < nativeArray1.Length; ++index4)
          {
            CarLane carLane = nativeArray3[index4];
            Lane lane1 = nativeArray1[index4];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<SubLane> subLane4 = this.m_SubLanes[nativeArray4[index4].m_Owner];
            for (int index5 = 0; index5 < subLane4.Length; ++index5)
            {
              Entity subLane5 = subLane4[index5].m_SubLane;
              // ISSUE: reference to a compiler-generated field
              Lane lane2 = this.m_LaneData[subLane5];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CarLaneData.HasComponent(subLane5) && (int) this.m_CarLaneData[subLane5].m_CarriagewayGroup == (int) carLane.m_CarriagewayGroup)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<PathfindUpdated>(unfilteredChunkIndex, subLane5, new PathfindUpdated());
              }
              // ISSUE: reference to a compiler-generated field
              if (lane2.m_StartNode.EqualsIgnoreCurvePos(lane1.m_MiddleNode) && this.m_ParkingLaneData.HasComponent(subLane5))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<PathfindUpdated>(unfilteredChunkIndex, subLane5, new PathfindUpdated());
              }
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
      public ComponentTypeHandle<Lane> __Game_Net_Lane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SlaveLane> __Game_Net_SlaveLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CarLane> __Game_Net_CarLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MasterLane> __Game_Net_MasterLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubLane> __Game_Net_SubLane_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_MasterLane_RO_ComponentLookup = state.GetComponentLookup<MasterLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<SubLane>(true);
      }
    }
  }
}
