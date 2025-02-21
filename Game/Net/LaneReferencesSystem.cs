// Decompiled with JetBrains decompiler
// Type: Game.Net.LaneReferencesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Pathfind;
using Game.Prefabs;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class LaneReferencesSystem : GameSystemBase
  {
    private ModificationBarrier4B m_ModificationBarrier;
    private EntityQuery m_LanesQuery;
    private EntityQuery m_UpdatedOwnersQuery;
    private EntityQuery m_AllOwnersQuery;
    private bool m_Loaded;
    private LaneReferencesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4B>();
      // ISSUE: reference to a compiler-generated field
      this.m_LanesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Lane>(),
          ComponentType.ReadOnly<Owner>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<SecondaryLane>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedOwnersQuery = this.GetEntityQuery(ComponentType.ReadWrite<SubLane>(), ComponentType.ReadOnly<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_AllOwnersQuery = this.GetEntityQuery(ComponentType.ReadWrite<SubLane>());
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery query = this.GetLoaded() ? this.m_AllOwnersQuery : this.m_UpdatedOwnersQuery;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_LanesQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SubLane_RW_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_CarLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        LaneReferencesSystem.UpdateLaneReferencesJob jobData = new LaneReferencesSystem.UpdateLaneReferencesJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_PedestrianLaneType = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentTypeHandle,
          m_CarLaneType = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentTypeHandle,
          m_TrackLaneType = this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentTypeHandle,
          m_ParkingLaneType = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentTypeHandle,
          m_ConnectionLaneType = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentTypeHandle,
          m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
          m_Lanes = this.__TypeHandle.__Game_Net_SubLane_RW_BufferLookup
        };
        // ISSUE: reference to a compiler-generated field
        this.Dependency = jobData.Schedule<LaneReferencesSystem.UpdateLaneReferencesJob>(this.m_LanesQuery, this.Dependency);
      }
      if (query.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_MasterLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrainTrack_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TramTrack_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LaneReferencesSystem.UpdateLaneIndicesJob jobData1 = new LaneReferencesSystem.UpdateLaneIndicesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle,
        m_RoadType = this.__TypeHandle.__Game_Net_Road_RO_ComponentTypeHandle,
        m_TramTrackType = this.__TypeHandle.__Game_Net_TramTrack_RO_ComponentTypeHandle,
        m_TrainTrackType = this.__TypeHandle.__Game_Net_TrainTrack_RO_ComponentTypeHandle,
        m_SubLaneType = this.__TypeHandle.__Game_Net_SubLane_RW_BufferTypeHandle,
        m_SecondaryLaneData = this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabTrackLaneData = this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup,
        m_MasterLaneData = this.__TypeHandle.__Game_Net_MasterLane_RW_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RW_ComponentLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      this.Dependency = jobData1.ScheduleParallel<LaneReferencesSystem.UpdateLaneIndicesJob>(query, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(this.Dependency);
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
    public LaneReferencesSystem()
    {
    }

    [BurstCompile]
    private struct UpdateLaneReferencesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PedestrianLane> m_PedestrianLaneType;
      [ReadOnly]
      public ComponentTypeHandle<CarLane> m_CarLaneType;
      [ReadOnly]
      public ComponentTypeHandle<TrackLane> m_TrackLaneType;
      [ReadOnly]
      public ComponentTypeHandle<ParkingLane> m_ParkingLaneType;
      [ReadOnly]
      public ComponentTypeHandle<ConnectionLane> m_ConnectionLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      public BufferLookup<SubLane> m_Lanes;

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
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            DynamicBuffer<SubLane> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Lanes.TryGetBuffer(nativeArray2[index].m_Owner, out bufferData))
              CollectionUtils.RemoveValue<SubLane>(bufferData, new SubLane(nativeArray1[index], (PathMethod) 0));
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<ParkingLane> nativeArray3 = chunk.GetNativeArray<ParkingLane>(ref this.m_ParkingLaneType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<ConnectionLane> nativeArray4 = chunk.GetNativeArray<ConnectionLane>(ref this.m_ConnectionLaneType);
          PathMethod pathMethod = (PathMethod) 0;
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<PedestrianLane>(ref this.m_PedestrianLaneType))
            pathMethod |= PathMethod.Pedestrian;
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<CarLane>(ref this.m_CarLaneType))
            pathMethod |= PathMethod.Road;
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<TrackLane>(ref this.m_TrackLaneType))
            pathMethod |= PathMethod.Track;
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<SubLane> lane = this.m_Lanes[nativeArray2[index].m_Owner];
            PathMethod pathMethods = pathMethod;
            ParkingLane parkingLane;
            if (CollectionUtils.TryGet<ParkingLane>(nativeArray3, index, out parkingLane))
            {
              if ((parkingLane.m_Flags & ParkingLaneFlags.SpecialVehicles) != (ParkingLaneFlags) 0)
                pathMethods |= PathMethod.Boarding | PathMethod.SpecialParking;
              else
                pathMethods |= PathMethod.Parking | PathMethod.Boarding;
            }
            ConnectionLane connectionLane;
            if (CollectionUtils.TryGet<ConnectionLane>(nativeArray4, index, out connectionLane))
            {
              if ((connectionLane.m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0)
                pathMethods |= PathMethod.Pedestrian;
              if ((connectionLane.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0)
                pathMethods |= PathMethod.Road;
              if ((connectionLane.m_Flags & ConnectionLaneFlags.Track) != (ConnectionLaneFlags) 0)
                pathMethods |= PathMethod.Track;
              if ((connectionLane.m_Flags & ConnectionLaneFlags.Parking) != (ConnectionLaneFlags) 0)
                pathMethods |= PathMethod.Parking | PathMethod.Boarding;
            }
            SubLane subLane = new SubLane(nativeArray1[index], pathMethods);
            CollectionUtils.TryAddUniqueValue<SubLane>(lane, subLane);
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

    [BurstCompile]
    private struct UpdateLaneIndicesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Node> m_NodeType;
      [ReadOnly]
      public ComponentTypeHandle<Road> m_RoadType;
      [ReadOnly]
      public ComponentTypeHandle<TramTrack> m_TramTrackType;
      [ReadOnly]
      public ComponentTypeHandle<TrainTrack> m_TrainTrackType;
      public BufferTypeHandle<SubLane> m_SubLaneType;
      [ReadOnly]
      public ComponentLookup<SecondaryLane> m_SecondaryLaneData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> m_PrefabTrackLaneData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<MasterLane> m_MasterLaneData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeList<LaneReferencesSystem.SubLaneOrder> list = new NativeList<LaneReferencesSystem.SubLaneOrder>(256, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubLane> bufferAccessor = chunk.GetBufferAccessor<SubLane>(ref this.m_SubLaneType);
        bool flag1 = false;
        TrackTypes trackTypes1 = TrackTypes.None;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Node>(ref this.m_NodeType))
        {
          // ISSUE: reference to a compiler-generated field
          flag1 = chunk.Has<Road>(ref this.m_RoadType);
          if (flag1)
          {
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<TramTrack>(ref this.m_TramTrackType))
              trackTypes1 |= TrackTypes.Tram;
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<TrainTrack>(ref this.m_TrainTrackType))
              trackTypes1 |= TrackTypes.Train;
          }
        }
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<SubLane> dynamicBuffer = bufferAccessor[index1];
          TrackTypes trackTypes2 = TrackTypes.None;
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            LaneReferencesSystem.SubLaneOrder subLaneOrder = new LaneReferencesSystem.SubLaneOrder();
            // ISSUE: reference to a compiler-generated field
            subLaneOrder.m_SubLane = dynamicBuffer[index2];
            TrackLaneData componentData1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (flag1 && (subLaneOrder.m_SubLane.m_PathMethods & PathMethod.Track) != (PathMethod) 0 && this.m_PrefabTrackLaneData.TryGetComponent(this.m_PrefabRefData[subLaneOrder.m_SubLane.m_SubLane].m_Prefab, out componentData1))
              trackTypes2 |= componentData1.m_TrackTypes;
            MasterLane componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_MasterLaneData.TryGetComponent(subLaneOrder.m_SubLane.m_SubLane, out componentData2))
            {
              // ISSUE: reference to a compiler-generated field
              subLaneOrder.m_Group = componentData2.m_Group;
              // ISSUE: reference to a compiler-generated field
              subLaneOrder.m_Index = -1;
              // ISSUE: reference to a compiler-generated field
              subLaneOrder.m_FullLane = false;
            }
            else
            {
              SlaveLane componentData3;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_SlaveLaneData.TryGetComponent(subLaneOrder.m_SubLane.m_SubLane, out componentData3))
              {
                // ISSUE: reference to a compiler-generated field
                subLaneOrder.m_Group = componentData3.m_Group;
                // ISSUE: reference to a compiler-generated field
                subLaneOrder.m_Index = (int) componentData3.m_SubIndex << 16 | index2;
                // ISSUE: reference to a compiler-generated field
                subLaneOrder.m_FullLane = (componentData3.m_Flags & (SlaveLaneFlags.StartingLane | SlaveLaneFlags.EndingLane)) == (SlaveLaneFlags) 0;
                // ISSUE: reference to a compiler-generated field
                subLaneOrder.m_MergeLane = (componentData3.m_Flags & SlaveLaneFlags.MergingLane) != 0;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_SecondaryLaneData.HasComponent(subLaneOrder.m_SubLane.m_SubLane))
                {
                  // ISSUE: reference to a compiler-generated field
                  subLaneOrder.m_Group = uint.MaxValue;
                  // ISSUE: reference to a compiler-generated field
                  subLaneOrder.m_Index = index2;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  subLaneOrder.m_Group = 4294967294U;
                  // ISSUE: reference to a compiler-generated field
                  subLaneOrder.m_Index = index2;
                }
              }
            }
            list.Add(in subLaneOrder);
          }
          list.Sort<LaneReferencesSystem.SubLaneOrder>();
          int num1 = 0;
          while (num1 < list.Length)
          {
            // ISSUE: variable of a compiler-generated type
            LaneReferencesSystem.SubLaneOrder subLaneOrder1 = list[num1];
            // ISSUE: reference to a compiler-generated field
            if (subLaneOrder1.m_Group < 4294967294U)
            {
              int num2 = num1;
              int index3 = num1 + 1;
              // ISSUE: reference to a compiler-generated field
              int num3 = math.select(0, 1, subLaneOrder1.m_FullLane);
              for (; index3 < list.Length; ++index3)
              {
                // ISSUE: variable of a compiler-generated type
                LaneReferencesSystem.SubLaneOrder subLaneOrder2 = list[index3];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((int) subLaneOrder2.m_Group == (int) subLaneOrder1.m_Group)
                {
                  // ISSUE: reference to a compiler-generated field
                  num3 += math.select(0, 1, subLaneOrder2.m_FullLane);
                }
                else
                  break;
              }
              int a = -1;
              // ISSUE: reference to a compiler-generated field
              if (subLaneOrder1.m_Index == -1)
              {
                ++num2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                MasterLane masterLane = this.m_MasterLaneData[subLaneOrder1.m_SubLane.m_SubLane] with
                {
                  m_MinIndex = (ushort) num2,
                  m_MaxIndex = (ushort) (index3 - 1)
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_MasterLaneData[subLaneOrder1.m_SubLane.m_SubLane] = masterLane;
                a = num1;
                // ISSUE: reference to a compiler-generated field
                dynamicBuffer[num1++] = subLaneOrder1.m_SubLane;
              }
              Lane lane1 = new Lane();
              Lane lane2 = new Lane();
              bool flag2 = false;
              if (num1 < index3)
              {
                // ISSUE: variable of a compiler-generated type
                LaneReferencesSystem.SubLaneOrder subLaneOrder3 = list[num1];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                lane2 = this.m_LaneData[subLaneOrder3.m_SubLane.m_SubLane];
              }
              // ISSUE: variable of a compiler-generated type
              LaneReferencesSystem.SubLaneOrder subLaneOrder4;
              // ISSUE: reference to a compiler-generated field
              for (; num1 < index3; dynamicBuffer[num1++] = subLaneOrder4.m_SubLane)
              {
                subLaneOrder4 = list[num1];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                SlaveLane slaveLane = this.m_SlaveLaneData[subLaneOrder4.m_SubLane.m_SubLane];
                slaveLane.m_Flags &= ~(SlaveLaneFlags.MultipleLanes | SlaveLaneFlags.OpenStartLeft | SlaveLaneFlags.OpenStartRight | SlaveLaneFlags.OpenEndLeft | SlaveLaneFlags.OpenEndRight);
                Lane lane3 = lane2;
                // ISSUE: reference to a compiler-generated field
                if (num1 > num2 && subLaneOrder4.m_MergeLane == flag2)
                {
                  if (!lane1.m_StartNode.Equals(lane3.m_StartNode))
                    slaveLane.m_Flags |= SlaveLaneFlags.OpenStartLeft;
                  if (!lane1.m_EndNode.Equals(lane3.m_EndNode))
                    slaveLane.m_Flags |= SlaveLaneFlags.OpenEndLeft;
                }
                if (num1 + 1 < index3)
                {
                  // ISSUE: variable of a compiler-generated type
                  LaneReferencesSystem.SubLaneOrder subLaneOrder5 = list[num1 + 1];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  lane2 = this.m_LaneData[subLaneOrder5.m_SubLane.m_SubLane];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (subLaneOrder4.m_MergeLane == subLaneOrder5.m_MergeLane)
                  {
                    if (!lane2.m_StartNode.Equals(lane3.m_StartNode))
                      slaveLane.m_Flags |= SlaveLaneFlags.OpenStartRight;
                    if (!lane2.m_EndNode.Equals(lane3.m_EndNode))
                      slaveLane.m_Flags |= SlaveLaneFlags.OpenEndRight;
                  }
                }
                lane1 = lane3;
                // ISSUE: reference to a compiler-generated field
                flag2 = subLaneOrder4.m_MergeLane;
                slaveLane.m_MinIndex = (ushort) num2;
                slaveLane.m_MaxIndex = (ushort) (index3 - 1);
                slaveLane.m_SubIndex = (ushort) num1;
                slaveLane.m_MasterIndex = (ushort) math.select(a, num1, a == -1);
                if (num3 >= 2)
                  slaveLane.m_Flags |= SlaveLaneFlags.MultipleLanes;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_SlaveLaneData[subLaneOrder4.m_SubLane.m_SubLane] = slaveLane;
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              dynamicBuffer[num1++] = subLaneOrder1.m_SubLane;
            }
          }
          if (trackTypes1 != trackTypes2)
          {
            Entity e = nativeArray[index1];
            if ((trackTypes1 & ~trackTypes2 & TrackTypes.Tram) != TrackTypes.None)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<TramTrack>(unfilteredChunkIndex, e);
            }
            if ((trackTypes2 & ~trackTypes1 & TrackTypes.Tram) != TrackTypes.None)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<TramTrack>(unfilteredChunkIndex, e);
            }
            if ((trackTypes1 & ~trackTypes2 & TrackTypes.Train) != TrackTypes.None)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<TrainTrack>(unfilteredChunkIndex, e);
            }
            if ((trackTypes2 & ~trackTypes1 & TrackTypes.Train) != TrackTypes.None)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<TrainTrack>(unfilteredChunkIndex, e);
            }
          }
          list.Clear();
        }
        list.Dispose();
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

    private struct SubLaneOrder : IComparable<LaneReferencesSystem.SubLaneOrder>
    {
      public uint m_Group;
      public int m_Index;
      public SubLane m_SubLane;
      public bool m_FullLane;
      public bool m_MergeLane;

      public int CompareTo(LaneReferencesSystem.SubLaneOrder other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int a = math.select(0, math.select(1, -1, other.m_Group > this.m_Group), (int) other.m_Group != (int) this.m_Group);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.select(a, this.m_Index - other.m_Index, a == 0);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CarLane> __Game_Net_CarLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TrackLane> __Game_Net_TrackLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ParkingLane> __Game_Net_ParkingLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      public BufferLookup<SubLane> __Game_Net_SubLane_RW_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Node> __Game_Net_Node_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Road> __Game_Net_Road_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TramTrack> __Game_Net_TramTrack_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TrainTrack> __Game_Net_TrainTrack_RO_ComponentTypeHandle;
      public BufferTypeHandle<SubLane> __Game_Net_SubLane_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<SecondaryLane> __Game_Net_SecondaryLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> __Game_Prefabs_TrackLaneData_RO_ComponentLookup;
      public ComponentLookup<MasterLane> __Game_Net_MasterLane_RW_ComponentLookup;
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrackLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TrackLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RW_BufferLookup = state.GetBufferLookup<SubLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TramTrack_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TramTrack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrainTrack_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TrainTrack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SecondaryLane_RO_ComponentLookup = state.GetComponentLookup<SecondaryLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackLaneData_RO_ComponentLookup = state.GetComponentLookup<TrackLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_MasterLane_RW_ComponentLookup = state.GetComponentLookup<MasterLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RW_ComponentLookup = state.GetComponentLookup<SlaveLane>();
      }
    }
  }
}
