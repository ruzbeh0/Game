// Decompiled with JetBrains decompiler
// Type: Game.Routes.InitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Common;
using Game.Economy;
using Game.Prefabs;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Routes
{
  [CompilerGenerated]
  public class InitializeSystem : GameSystemBase
  {
    private CityConfigurationSystem m_CityConfigurationSystem;
    private EntityQuery m_CreatedQuery;
    private EntityQuery m_RouteQuery;
    private EntityQuery m_VehiclePrefabQuery;
    private TransportVehicleSelectData m_TransportVehicleSelectData;
    private InitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TransportVehicleSelectData = new TransportVehicleSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Route>(), ComponentType.ReadOnly<RouteNumber>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_RouteQuery = this.GetEntityQuery(ComponentType.ReadOnly<Route>(), ComponentType.ReadOnly<RouteNumber>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_VehiclePrefabQuery = this.GetEntityQuery(TransportVehicleSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatedQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_RouteQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TransportVehicleSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_VehiclePrefabQuery, Allocator.TempJob, out jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteNumber_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      InitializeSystem.AssignRouteNumbersJob jobData1 = new InitializeSystem.AssignRouteNumbersJob()
      {
        m_RouteChunks = archetypeChunkListAsync,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_RouteNumberType = this.__TypeHandle.__Game_Routes_RouteNumber_RW_ComponentTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_VehicleModel_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      InitializeSystem.SelectVehicleJob jobData2 = new InitializeSystem.SelectVehicleJob()
      {
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_VehicleModelType = this.__TypeHandle.__Game_Routes_VehicleModel_RW_ComponentTypeHandle,
        m_PrefabTransportLineData = this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_TransportVehicleSelectData = this.m_TransportVehicleSelectData
      };
      JobHandle jobHandle2 = jobData1.Schedule<InitializeSystem.AssignRouteNumbersJob>(JobHandle.CombineDependencies(outJobHandle, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      EntityQuery createdQuery = this.m_CreatedQuery;
      JobHandle dependsOn = JobHandle.CombineDependencies(this.Dependency, jobHandle1);
      JobHandle jobHandle3 = jobData2.ScheduleParallel<InitializeSystem.SelectVehicleJob>(createdQuery, dependsOn);
      // ISSUE: reference to a compiler-generated field
      this.m_TransportVehicleSelectData.PostUpdate(jobHandle3);
      archetypeChunkListAsync.Dispose(jobHandle2);
      this.Dependency = JobHandle.CombineDependencies(jobHandle2, jobHandle3);
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
    public InitializeSystem()
    {
    }

    [BurstCompile]
    private struct AssignRouteNumbersJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_RouteChunks;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<RouteNumber> m_RouteNumberType;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_RouteChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk routeChunk = this.m_RouteChunks[index1];
          // ISSUE: reference to a compiler-generated field
          if (routeChunk.Has<Created>(ref this.m_CreatedType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<RouteNumber> nativeArray1 = routeChunk.GetNativeArray<RouteNumber>(ref this.m_RouteNumberType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray2 = routeChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              RouteNumber routeNumber = nativeArray1[index2];
              PrefabRef prefabRef = nativeArray2[index2];
              // ISSUE: reference to a compiler-generated method
              routeNumber.m_Number = this.FindFreeRouteNumber(prefabRef.m_Prefab);
              nativeArray1[index2] = routeNumber;
            }
          }
        }
      }

      private int FindFreeRouteNumber(Entity prefab)
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_RouteChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk routeChunk = this.m_RouteChunks[index1];
          // ISSUE: reference to a compiler-generated field
          if (!routeChunk.Has<Created>(ref this.m_CreatedType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray = routeChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            for (int index2 = 0; index2 < nativeArray.Length; ++index2)
              num += math.select(0, 1, nativeArray[index2].m_Prefab == prefab);
          }
        }
        if (num > 0)
        {
          NativeBitArray nativeBitArray = new NativeBitArray(num + 1, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          for (int index3 = 0; index3 < this.m_RouteChunks.Length; ++index3)
          {
            // ISSUE: reference to a compiler-generated field
            ArchetypeChunk routeChunk = this.m_RouteChunks[index3];
            // ISSUE: reference to a compiler-generated field
            if (!routeChunk.Has<Created>(ref this.m_CreatedType))
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<RouteNumber> nativeArray1 = routeChunk.GetNativeArray<RouteNumber>(ref this.m_RouteNumberType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<PrefabRef> nativeArray2 = routeChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
              for (int index4 = 0; index4 < nativeArray2.Length; ++index4)
              {
                if (nativeArray2[index4].m_Prefab == prefab)
                {
                  RouteNumber routeNumber = nativeArray1[index4];
                  if (routeNumber.m_Number <= num)
                    nativeBitArray.Set(routeNumber.m_Number, true);
                }
              }
            }
          }
          for (int pos = 1; pos <= num; ++pos)
          {
            if (!nativeBitArray.IsSet(pos))
              return pos;
          }
          nativeBitArray.Dispose();
        }
        return num + 1;
      }
    }

    [BurstCompile]
    private struct SelectVehicleJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<VehicleModel> m_VehicleModelType;
      [ReadOnly]
      public ComponentLookup<TransportLineData> m_PrefabTransportLineData;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public TransportVehicleSelectData m_TransportVehicleSelectData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<VehicleModel> nativeArray1 = chunk.GetNativeArray<VehicleModel>(ref this.m_VehicleModelType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          VehicleModel vehicleModel = nativeArray1[index];
          TransportLineData componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabTransportLineData.TryGetComponent(nativeArray2[index].m_Prefab, out componentData))
          {
            PublicTransportPurpose publicTransportPurpose = componentData.m_PassengerTransport ? PublicTransportPurpose.TransportLine : (PublicTransportPurpose) 0;
            Resource cargoResources = componentData.m_CargoTransport ? Resource.Food : Resource.NoResource;
            int2 passengerCapacity = componentData.m_PassengerTransport ? new int2(1, int.MaxValue) : (int2) 0;
            int2 cargoCapacity = componentData.m_CargoTransport ? new int2(1, int.MaxValue) : (int2) 0;
            // ISSUE: reference to a compiler-generated field
            this.m_TransportVehicleSelectData.SelectVehicle(ref random, componentData.m_TransportType, EnergyTypes.FuelAndElectricity, publicTransportPurpose, cargoResources, out vehicleModel.m_PrimaryPrefab, out vehicleModel.m_SecondaryPrefab, ref passengerCapacity, ref cargoCapacity);
          }
          nativeArray1[index] = vehicleModel;
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
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<RouteNumber> __Game_Routes_RouteNumber_RW_ComponentTypeHandle;
      public ComponentTypeHandle<VehicleModel> __Game_Routes_VehicleModel_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<TransportLineData> __Game_Prefabs_TransportLineData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteNumber_RW_ComponentTypeHandle = state.GetComponentTypeHandle<RouteNumber>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_VehicleModel_RW_ComponentTypeHandle = state.GetComponentTypeHandle<VehicleModel>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportLineData_RO_ComponentLookup = state.GetComponentLookup<TransportLineData>(true);
      }
    }
  }
}
