// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.LanePoliciesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Net;
using Game.Policies;
using Game.Prefabs;
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
  public class LanePoliciesSystem : GameSystemBase
  {
    private ModificationBarrier5 m_ModificationBarrier;
    private EntityQuery m_PolicyModifyQuery;
    private EntityQuery m_LaneOwnerQuery;
    private EntityQuery m_CarLaneQuery;
    private EntityQuery m_ParkingLaneQuery;
    private LanePoliciesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_PolicyModifyQuery = this.GetEntityQuery(ComponentType.ReadOnly<Modify>());
      // ISSUE: reference to a compiler-generated field
      this.m_LaneOwnerQuery = this.GetEntityQuery(ComponentType.ReadOnly<BorderDistrict>(), ComponentType.ReadOnly<Game.Net.SubLane>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Updated>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_CarLaneQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.CarLane>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Updated>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ParkingLaneQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.ParkingLane>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Updated>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PolicyModifyQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Modify> componentDataArray = this.m_PolicyModifyQuery.ToComponentDataArray<Modify>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeParallelHashMap<Entity, LanePoliciesSystem.LaneCheckMask> nativeParallelHashMap = new NativeParallelHashMap<Entity, LanePoliciesSystem.LaneCheckMask>();
      NativeList<Entity> nativeList = new NativeList<Entity>();
      // ISSUE: variable of a compiler-generated type
      LanePoliciesSystem.LaneCheckMask laneCheckMask1 = (LanePoliciesSystem.LaneCheckMask) 0;
      for (int index1 = 0; index1 < componentDataArray.Length; ++index1)
      {
        Modify modify = componentDataArray[index1];
        // ISSUE: variable of a compiler-generated type
        LanePoliciesSystem.LaneCheckMask laneCheckMask2 = (LanePoliciesSystem.LaneCheckMask) 0;
        bool flag = false;
        EntityManager entityManager = this.EntityManager;
        if (entityManager.HasComponent<Game.City.City>(modify.m_Entity))
        {
          CityOptionData component;
          if (this.EntityManager.TryGetComponent<CityOptionData>(modify.m_Policy, out component))
          {
            if (CityUtils.HasOption(component, CityOption.UnlimitedHighwaySpeed))
              laneCheckMask1 |= LanePoliciesSystem.LaneCheckMask.CarUnknown;
            if (CityUtils.HasOption(component, CityOption.PaidTaxiStart))
              laneCheckMask1 |= LanePoliciesSystem.LaneCheckMask.ParkingUnknown;
          }
          DynamicBuffer<CityModifierData> buffer;
          if (this.EntityManager.TryGetBuffer<CityModifierData>(modify.m_Policy, true, out buffer))
          {
            for (int index2 = 0; index2 < buffer.Length; ++index2)
            {
              if (buffer[index2].m_Type == CityModifierType.TaxiStartingFee)
                laneCheckMask1 |= LanePoliciesSystem.LaneCheckMask.ParkingUnknown;
            }
          }
        }
        entityManager = this.EntityManager;
        if (entityManager.HasComponent<District>(modify.m_Entity))
        {
          DistrictOptionData component;
          if (this.EntityManager.TryGetComponent<DistrictOptionData>(modify.m_Policy, out component))
          {
            if (AreaUtils.HasOption(component, DistrictOption.PaidParking))
              laneCheckMask2 |= LanePoliciesSystem.LaneCheckMask.ParkingUnknown;
            if (AreaUtils.HasOption(component, DistrictOption.ForbidCombustionEngines))
              laneCheckMask2 |= LanePoliciesSystem.LaneCheckMask.CarUnknown;
            if (AreaUtils.HasOption(component, DistrictOption.ForbidTransitTraffic))
              laneCheckMask2 |= LanePoliciesSystem.LaneCheckMask.CarUnknown | LanePoliciesSystem.LaneCheckMask.PedestrianUnknown;
            if (AreaUtils.HasOption(component, DistrictOption.ForbidHeavyTraffic))
              laneCheckMask2 |= LanePoliciesSystem.LaneCheckMask.CarUnknown;
          }
          DynamicBuffer<DistrictModifierData> buffer;
          if (this.EntityManager.TryGetBuffer<DistrictModifierData>(modify.m_Policy, true, out buffer))
          {
            for (int index3 = 0; index3 < buffer.Length; ++index3)
            {
              switch (buffer[index3].m_Type)
              {
                case DistrictModifierType.ParkingFee:
                  laneCheckMask2 |= LanePoliciesSystem.LaneCheckMask.ParkingUnknown;
                  break;
                case DistrictModifierType.StreetSpeedLimit:
                  laneCheckMask2 |= LanePoliciesSystem.LaneCheckMask.CarUnknown;
                  break;
              }
            }
          }
        }
        entityManager = this.EntityManager;
        if (entityManager.HasComponent<Building>(modify.m_Entity))
        {
          BuildingOptionData component;
          if (this.EntityManager.TryGetComponent<BuildingOptionData>(modify.m_Policy, out component) && BuildingUtils.HasOption(component, BuildingOption.PaidParking))
            flag = true;
          DynamicBuffer<BuildingModifierData> buffer;
          if (this.EntityManager.TryGetBuffer<BuildingModifierData>(modify.m_Policy, true, out buffer))
          {
            for (int index4 = 0; index4 < buffer.Length; ++index4)
            {
              if (buffer[index4].m_Type == BuildingModifierType.ParkingFee)
              {
                flag = true;
                break;
              }
            }
          }
        }
        if (laneCheckMask2 != (LanePoliciesSystem.LaneCheckMask) 0)
        {
          if (!nativeParallelHashMap.IsCreated)
            nativeParallelHashMap = new NativeParallelHashMap<Entity, LanePoliciesSystem.LaneCheckMask>(componentDataArray.Length, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
          if (!nativeParallelHashMap.TryAdd(modify.m_Entity, laneCheckMask2))
            nativeParallelHashMap[modify.m_Entity] = nativeParallelHashMap[modify.m_Entity] | laneCheckMask2;
        }
        if (flag)
        {
          if (!nativeList.IsCreated)
            nativeList = new NativeList<Entity>(componentDataArray.Length, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
          nativeList.Add(in modify.m_Entity);
        }
      }
      componentDataArray.Dispose();
      JobHandle job0 = this.Dependency;
      if (laneCheckMask1 != (LanePoliciesSystem.LaneCheckMask) 0)
      {
        // ISSUE: reference to a compiler-generated field
        EntityCommandBuffer commandBuffer = this.m_ModificationBarrier.CreateCommandBuffer();
        if ((laneCheckMask1 & LanePoliciesSystem.LaneCheckMask.CarUnknown) != (LanePoliciesSystem.LaneCheckMask) 0)
        {
          // ISSUE: reference to a compiler-generated field
          commandBuffer.AddComponent<PathfindUpdated>(this.m_CarLaneQuery, EntityQueryCaptureMode.AtPlayback);
        }
        if ((laneCheckMask1 & LanePoliciesSystem.LaneCheckMask.ParkingUnknown) != (LanePoliciesSystem.LaneCheckMask) 0)
        {
          // ISSUE: reference to a compiler-generated field
          commandBuffer.AddComponent<PathfindUpdated>(this.m_ParkingLaneQuery, EntityQueryCaptureMode.AtPlayback);
        }
      }
      if (nativeParallelHashMap.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        JobHandle jobHandle = new LanePoliciesSystem.CheckDistrictLanesJob()
        {
          m_BorderDistrictType = this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentTypeHandle,
          m_SubLaneType = this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle,
          m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
          m_PedestrianLaneData = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup,
          m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
          m_CheckDistricts = nativeParallelHashMap,
          m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
        }.ScheduleParallel<LanePoliciesSystem.CheckDistrictLanesJob>(this.m_LaneOwnerQuery, this.Dependency);
        nativeParallelHashMap.Dispose(jobHandle);
        // ISSUE: reference to a compiler-generated field
        this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
        job0 = JobHandle.CombineDependencies(job0, jobHandle);
      }
      if (nativeList.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
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
        JobHandle jobHandle = new LanePoliciesSystem.CheckBuildingLanesJob()
        {
          m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
          m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
          m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
          m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
          m_GarageLaneData = this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup,
          m_CheckBuildings = nativeList.AsArray(),
          m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
        }.Schedule<LanePoliciesSystem.CheckBuildingLanesJob>(nativeList.Length, 1, this.Dependency);
        nativeList.Dispose(jobHandle);
        // ISSUE: reference to a compiler-generated field
        this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
        job0 = JobHandle.CombineDependencies(job0, jobHandle);
      }
      this.Dependency = job0;
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
    public LanePoliciesSystem()
    {
    }

    private enum LaneCheckMask
    {
      ParkingUnknown = 1,
      CarUnknown = 2,
      PedestrianUnknown = 4,
    }

    [BurstCompile]
    private struct CheckDistrictLanesJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<BorderDistrict> m_BorderDistrictType;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubLane> m_SubLaneType;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> m_PedestrianLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public NativeParallelHashMap<Entity, LanePoliciesSystem.LaneCheckMask> m_CheckDistricts;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<BorderDistrict> nativeArray = chunk.GetNativeArray<BorderDistrict>(ref this.m_BorderDistrictType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Net.SubLane> bufferAccessor = chunk.GetBufferAccessor<Game.Net.SubLane>(ref this.m_SubLaneType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          BorderDistrict borderDistrict = nativeArray[index1];
          // ISSUE: variable of a compiler-generated type
          LanePoliciesSystem.LaneCheckMask laneCheckMask1 = (LanePoliciesSystem.LaneCheckMask) 0;
          // ISSUE: variable of a compiler-generated type
          LanePoliciesSystem.LaneCheckMask laneCheckMask2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CheckDistricts.TryGetValue(borderDistrict.m_Left, out laneCheckMask2))
            laneCheckMask1 |= laneCheckMask2;
          // ISSUE: variable of a compiler-generated type
          LanePoliciesSystem.LaneCheckMask laneCheckMask3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CheckDistricts.TryGetValue(borderDistrict.m_Right, out laneCheckMask3))
            laneCheckMask1 |= laneCheckMask3;
          if (laneCheckMask1 != (LanePoliciesSystem.LaneCheckMask) 0)
          {
            DynamicBuffer<Game.Net.SubLane> dynamicBuffer = bufferAccessor[index1];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              Entity subLane = dynamicBuffer[index2].m_SubLane;
              // ISSUE: reference to a compiler-generated field
              if ((laneCheckMask1 & LanePoliciesSystem.LaneCheckMask.ParkingUnknown) != (LanePoliciesSystem.LaneCheckMask) 0 && this.m_ParkingLaneData.HasComponent(subLane))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<PathfindUpdated>(unfilteredChunkIndex, subLane);
              }
              // ISSUE: reference to a compiler-generated field
              if ((laneCheckMask1 & LanePoliciesSystem.LaneCheckMask.CarUnknown) != (LanePoliciesSystem.LaneCheckMask) 0 && this.m_CarLaneData.HasComponent(subLane))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<PathfindUpdated>(unfilteredChunkIndex, subLane);
              }
              // ISSUE: reference to a compiler-generated field
              if ((laneCheckMask1 & LanePoliciesSystem.LaneCheckMask.PedestrianUnknown) != (LanePoliciesSystem.LaneCheckMask) 0 && this.m_PedestrianLaneData.HasComponent(subLane))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<PathfindUpdated>(unfilteredChunkIndex, subLane);
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

    [BurstCompile]
    private struct CheckBuildingLanesJob : IJobParallelFor
    {
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> m_SubNets;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<GarageLane> m_GarageLaneData;
      [ReadOnly]
      public NativeArray<Entity> m_CheckBuildings;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity checkBuilding = this.m_CheckBuildings[index];
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubLanes.HasBuffer(checkBuilding))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckParkingLanes(index, this.m_SubLanes[checkBuilding]);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubNets.HasBuffer(checkBuilding))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckParkingLanes(index, this.m_SubNets[checkBuilding]);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.HasBuffer(checkBuilding))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.CheckParkingLanes(index, this.m_SubObjects[checkBuilding]);
      }

      private void CheckParkingLanes(int jobIndex, DynamicBuffer<Game.Objects.SubObject> subObjects)
      {
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubLanes.HasBuffer(subObject))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.CheckParkingLanes(jobIndex, this.m_SubLanes[subObject]);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubObjects.HasBuffer(subObject))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.CheckParkingLanes(jobIndex, this.m_SubObjects[subObject]);
          }
        }
      }

      private void CheckParkingLanes(int jobIndex, DynamicBuffer<Game.Net.SubNet> subNets)
      {
        for (int index = 0; index < subNets.Length; ++index)
        {
          Entity subNet = subNets[index].m_SubNet;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubLanes.HasBuffer(subNet))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.CheckParkingLanes(jobIndex, this.m_SubLanes[subNet]);
          }
        }
      }

      private void CheckParkingLanes(int jobIndex, DynamicBuffer<Game.Net.SubLane> subLanes)
      {
        for (int index = 0; index < subLanes.Length; ++index)
        {
          Entity subLane = subLanes[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ParkingLaneData.HasComponent(subLane) || this.m_GarageLaneData.HasComponent(subLane))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, subLane);
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<BorderDistrict> __Game_Areas_BorderDistrict_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<GarageLane> __Game_Net_GarageLane_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_BorderDistrict_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BorderDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_GarageLane_RO_ComponentLookup = state.GetComponentLookup<GarageLane>(true);
      }
    }
  }
}
