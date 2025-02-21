// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.RoadsInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.City;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class RoadsInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "roadsInfo";
    private CityStatisticsSystem m_CityStatisticsSystem;
    private ValueBinding<float> m_ParkingCapacity;
    private ValueBinding<int> m_ParkedCars;
    private ValueBinding<int> m_ParkingIncome;
    private ValueBinding<IndicatorValue> m_ParkingAvailability;
    private EntityQuery m_ParkingFacilityQuery;
    private EntityQuery m_ParkingFacilityModifiedQuery;
    private NativeArray<int> m_Results;
    private RoadsInfoviewUISystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ParkingFacilityQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Buildings.ParkingFacility>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Net.SubLane>(),
          ComponentType.ReadOnly<Game.Net.SubNet>(),
          ComponentType.ReadOnly<Game.Objects.SubObject>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ParkingFacilityModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Buildings.ParkingFacility>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ParkingCapacity = new ValueBinding<float>("roadsInfo", "parkingCapacity", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ParkedCars = new ValueBinding<int>("roadsInfo", "parkedCars", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ParkingIncome = new ValueBinding<int>("roadsInfo", "parkingIncome", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ParkingAvailability = new ValueBinding<IndicatorValue>("roadsInfo", "parkingAvailability", new IndicatorValue(), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<int>(2, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Results.Dispose();
      base.OnDestroy();
    }

    protected override bool Active
    {
      get
      {
        return base.Active || this.m_ParkedCars.active || this.m_ParkingAvailability.active || this.m_ParkingCapacity.active || this.m_ParkingIncome.active;
      }
    }

    protected override bool Modified => !this.m_ParkingFacilityModifiedQuery.IsEmptyIgnoreFilter;

    protected override void PerformUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.UpdateCapacity();
      // ISSUE: reference to a compiler-generated method
      this.UpdateAvailability();
      // ISSUE: reference to a compiler-generated method
      this.UpdateIncome();
    }

    private void ResetResults()
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Results.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Results[index] = 0;
      }
    }

    private void UpdateCapacity()
    {
      // ISSUE: reference to a compiler-generated method
      this.ResetResults();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new RoadsInfoviewUISystem.UpdateParkingJob()
      {
        m_EntityHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_SubNetHandle = this.__TypeHandle.__Game_Net_SubNet_RO_BufferTypeHandle,
        m_SubLaneHandle = this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle,
        m_SubObjectHandle = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle,
        m_CurveFromEntity = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_ParkedCarFromEntity = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ParkingLaneFromEntity = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_GarageLaneFromEntity = this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup,
        m_PrefabRefFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ParkingLaneDataFromEntity = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_SubLaneFromEntity = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_SubObjectFromEntity = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_LaneObjectFromEntity = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_Results = this.m_Results
      }.Schedule<RoadsInfoviewUISystem.UpdateParkingJob>(this.m_ParkingFacilityQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ParkingCapacity.Update((float) this.m_Results[0]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ParkedCars.Update(this.m_Results[1]);
    }

    private void UpdateAvailability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ParkingAvailability.Update(IndicatorValue.Calculate(this.m_ParkingCapacity.value, (float) this.m_ParkedCars.value));
    }

    private void UpdateIncome()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ParkingIncome.Update(this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.Income, 9));
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
    public RoadsInfoviewUISystem()
    {
    }

    private enum Result
    {
      Slots,
      Parked,
      ResultCount,
    }

    [BurstCompile]
    private struct UpdateParkingJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubNet> m_SubNetHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubLane> m_SubLaneHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Objects.SubObject> m_SubObjectHandle;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveFromEntity;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarFromEntity;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneFromEntity;
      [ReadOnly]
      public ComponentLookup<GarageLane> m_GarageLaneFromEntity;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefFromEntity;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_ParkingLaneDataFromEntity;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLaneFromEntity;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjectFromEntity;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjectFromEntity;
      public NativeArray<int> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityHandle);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Net.SubLane> bufferAccessor1 = chunk.GetBufferAccessor<Game.Net.SubLane>(ref this.m_SubLaneHandle);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Net.SubNet> bufferAccessor2 = chunk.GetBufferAccessor<Game.Net.SubNet>(ref this.m_SubNetHandle);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Objects.SubObject> bufferAccessor3 = chunk.GetBufferAccessor<Game.Objects.SubObject>(ref this.m_SubObjectHandle);
        int num = 0;
        int parked = 0;
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          int slots = 0;
          if (bufferAccessor1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckParkingLanes(bufferAccessor1[index], ref slots, ref parked);
          }
          if (bufferAccessor2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckParkingLanes(bufferAccessor2[index], ref slots, ref parked);
          }
          if (bufferAccessor3.Length != 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckParkingLanes(bufferAccessor3[index], ref slots, ref parked);
          }
          num += math.select(0, slots, slots > 0);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Results[0] += num;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[1] += parked;
      }

      private void CheckParkingLanes(
        DynamicBuffer<Game.Objects.SubObject> subObjects,
        ref int slots,
        ref int parked)
      {
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          DynamicBuffer<Game.Net.SubLane> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubLaneFromEntity.TryGetBuffer(subObject, out bufferData1))
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckParkingLanes(bufferData1, ref slots, ref parked);
          }
          DynamicBuffer<Game.Objects.SubObject> bufferData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubObjectFromEntity.TryGetBuffer(subObject, out bufferData2))
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckParkingLanes(bufferData2, ref slots, ref parked);
          }
        }
      }

      private void CheckParkingLanes(DynamicBuffer<Game.Net.SubNet> subNets, ref int slots, ref int parked)
      {
        for (int index = 0; index < subNets.Length; ++index)
        {
          DynamicBuffer<Game.Net.SubLane> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubLaneFromEntity.TryGetBuffer(subNets[index].m_SubNet, out bufferData))
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckParkingLanes(bufferData, ref slots, ref parked);
          }
        }
      }

      private void CheckParkingLanes(
        DynamicBuffer<Game.Net.SubLane> subLanes,
        ref int slots,
        ref int parked)
      {
        for (int index1 = 0; index1 < subLanes.Length; ++index1)
        {
          Entity subLane = subLanes[index1].m_SubLane;
          Game.Net.ParkingLane componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ParkingLaneFromEntity.TryGetComponent(subLane, out componentData1))
          {
            if ((componentData1.m_Flags & ParkingLaneFlags.VirtualLane) == (ParkingLaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              Entity prefab = this.m_PrefabRefFromEntity[subLane].m_Prefab;
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveFromEntity[subLane];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<LaneObject> dynamicBuffer = this.m_LaneObjectFromEntity[subLane];
              // ISSUE: reference to a compiler-generated field
              ParkingLaneData prefabParkingLane = this.m_ParkingLaneDataFromEntity[prefab];
              if ((double) prefabParkingLane.m_SlotInterval != 0.0)
              {
                int parkingSlotCount = NetUtils.GetParkingSlotCount(curve, componentData1, prefabParkingLane);
                slots += parkingSlotCount;
              }
              else
                slots = -1000000;
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_ParkedCarFromEntity.HasComponent(dynamicBuffer[index2].m_LaneObject))
                  ++parked;
              }
            }
          }
          else
          {
            GarageLane componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_GarageLaneFromEntity.TryGetComponent(subLane, out componentData2))
            {
              slots += (int) componentData2.m_VehicleCapacity;
              parked += (int) componentData2.m_VehicleCount;
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
      public BufferTypeHandle<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarageLane> __Game_Net_GarageLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_GarageLane_RO_ComponentLookup = state.GetComponentLookup<GarageLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
      }
    }
  }
}
