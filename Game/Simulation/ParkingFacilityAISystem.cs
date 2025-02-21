// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ParkingFacilityAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
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
  public class ParkingFacilityAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_BuildingQuery;
    private ParkingFacilityAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 192;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.ParkingFacility>(), ComponentType.Exclude<Game.Buildings.ServiceUpgrade>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BuildingQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ParkingFacility_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new ParkingFacilityAISystem.ParkingFacilityTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_ParkingFacilityType = this.__TypeHandle.__Game_Buildings_ParkingFacility_RW_ComponentTypeHandle,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_GarageLaneData = this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabParkingFacilityData = this.__TypeHandle.__Game_Prefabs_ParkingFacilityData_RO_ComponentLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<ParkingFacilityAISystem.ParkingFacilityTickJob>(this.m_BuildingQuery, this.Dependency);
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
    public ParkingFacilityAISystem()
    {
    }

    [BurstCompile]
    private struct ParkingFacilityTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      public ComponentTypeHandle<Game.Buildings.ParkingFacility> m_ParkingFacilityType;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<GarageLane> m_GarageLaneData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ParkingFacilityData> m_PrefabParkingFacilityData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> m_SubNets;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
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
        NativeArray<Game.Buildings.ParkingFacility> nativeArray3 = chunk.GetNativeArray<Game.Buildings.ParkingFacility>(ref this.m_ParkingFacilityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor1 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          Game.Buildings.ParkingFacility parkingFacility = nativeArray3[index];
          // ISSUE: reference to a compiler-generated field
          ParkingFacilityData data = this.m_PrefabParkingFacilityData[prefabRef.m_Prefab];
          if (bufferAccessor2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UpgradeUtils.CombineStats<ParkingFacilityData>(ref data, bufferAccessor2[index], ref this.m_PrefabRefData, ref this.m_PrefabParkingFacilityData);
          }
          float efficiency = BuildingUtils.GetEfficiency(bufferAccessor1, index);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, ref parkingFacility, data, efficiency);
          nativeArray3[index] = parkingFacility;
        }
      }

      private void Tick(
        int jobIndex,
        Entity entity,
        ref Game.Buildings.ParkingFacility parkingFacility,
        ParkingFacilityData prefabParkingFacilityData,
        float efficiency)
      {
        float num = math.saturate(prefabParkingFacilityData.m_ComfortFactor * efficiency);
        bool flag1 = (double) efficiency > 0.0;
        bool flag2 = (parkingFacility.m_Flags & ParkingFacilityFlags.ParkingSpacesActive) != 0;
        if ((double) num == (double) parkingFacility.m_ComfortFactor && flag1 == flag2)
          return;
        parkingFacility.m_ComfortFactor = num;
        if (flag1)
          parkingFacility.m_Flags |= ParkingFacilityFlags.ParkingSpacesActive;
        else
          parkingFacility.m_Flags &= ~ParkingFacilityFlags.ParkingSpacesActive;
        DynamicBuffer<Game.Net.SubLane> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubLanes.TryGetBuffer(entity, out bufferData1))
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateParkingLanes(jobIndex, bufferData1);
        }
        DynamicBuffer<Game.Net.SubNet> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubNets.TryGetBuffer(entity, out bufferData2))
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateParkingLanes(jobIndex, bufferData2);
        }
        DynamicBuffer<Game.Objects.SubObject> bufferData3;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(entity, out bufferData3))
          return;
        // ISSUE: reference to a compiler-generated method
        this.UpdateParkingLanes(jobIndex, bufferData3);
      }

      private void UpdateParkingLanes(int jobIndex, DynamicBuffer<Game.Objects.SubObject> subObjects)
      {
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          DynamicBuffer<Game.Net.SubLane> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubLanes.TryGetBuffer(subObject, out bufferData1))
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateParkingLanes(jobIndex, bufferData1);
          }
          DynamicBuffer<Game.Objects.SubObject> bufferData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubObjects.TryGetBuffer(subObject, out bufferData2))
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateParkingLanes(jobIndex, bufferData2);
          }
        }
      }

      private void UpdateParkingLanes(int jobIndex, DynamicBuffer<Game.Net.SubNet> subNets)
      {
        for (int index = 0; index < subNets.Length; ++index)
        {
          DynamicBuffer<Game.Net.SubLane> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubLanes.TryGetBuffer(subNets[index].m_SubNet, out bufferData))
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateParkingLanes(jobIndex, bufferData);
          }
        }
      }

      private void UpdateParkingLanes(int jobIndex, DynamicBuffer<Game.Net.SubLane> subLanes)
      {
        for (int index = 0; index < subLanes.Length; ++index)
        {
          Entity subLane = subLanes[index].m_SubLane;
          Game.Net.ParkingLane componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ParkingLaneData.TryGetComponent(subLane, out componentData))
          {
            if ((componentData.m_Flags & ParkingLaneFlags.VirtualLane) == (ParkingLaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, subLane, new PathfindUpdated());
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_GarageLaneData.HasComponent(subLane))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, subLane, new PathfindUpdated());
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Buildings.ParkingFacility> __Game_Buildings_ParkingFacility_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarageLane> __Game_Net_GarageLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingFacilityData> __Game_Prefabs_ParkingFacilityData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ParkingFacility_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.ParkingFacility>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_GarageLane_RO_ComponentLookup = state.GetComponentLookup<GarageLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingFacilityData_RO_ComponentLookup = state.GetComponentLookup<ParkingFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
      }
    }
  }
}
