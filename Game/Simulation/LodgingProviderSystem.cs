// Decompiled with JetBrains decompiler
// Type: Game.Simulation.LodgingProviderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class LodgingProviderSystem : GameSystemBase
  {
    private static readonly int kUpdatesPerDay = 32;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private ResourceSystem m_ResourceSystem;
    private EntityQuery m_ProviderQuery;
    private EntityQuery m_LeisureParameterQuery;
    private LodgingProviderSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (LodgingProviderSystem.kUpdatesPerDay * 16);
    }

    public static int GetRoomCount(
      int2 lotSize,
      int level,
      BuildingPropertyData buildingPropertyData)
    {
      return (int) ((double) (lotSize.x * lotSize.y * level) * (double) buildingPropertyData.m_SpaceMultiplier);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ProviderQuery = this.GetEntityQuery(ComponentType.ReadWrite<LodgingProvider>(), ComponentType.ReadWrite<PropertyRenter>(), ComponentType.ReadWrite<ServiceAvailable>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Game.Companies.ProcessingCompany>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_LeisureParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<LeisureParametersData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ProviderQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_LeisureParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, LodgingProviderSystem.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_LodgingProvider_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ServiceAvailable_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LodgingProviderSystem.LodgingProviderJob jobData = new LodgingProviderSystem.LodgingProviderJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ServiceAvailableType = this.__TypeHandle.__Game_Companies_ServiceAvailable_RW_ComponentTypeHandle,
        m_LodgingProviderType = this.__TypeHandle.__Game_Companies_LodgingProvider_RW_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_RenterType = this.__TypeHandle.__Game_Buildings_Renter_RW_BufferTypeHandle,
        m_SpawnableBuildingDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_BuildingDatas = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_BuildingPropertyDatas = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_TouristHouseholds = this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup,
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_LeisureParameters = this.m_LeisureParameterQuery.GetSingleton<LeisureParametersData>(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_UpdateFrameIndex = updateFrame
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<LodgingProviderSystem.LodgingProviderJob>(this.m_ProviderQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
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
    public LodgingProviderSystem()
    {
    }

    [BurstCompile]
    private struct LodgingProviderJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<ServiceAvailable> m_ServiceAvailableType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public ComponentTypeHandle<LodgingProvider> m_LodgingProviderType;
      public BufferTypeHandle<Renter> m_RenterType;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDatas;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyDatas;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDatas;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> m_TouristHouseholds;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Game.Economy.Resources> m_Resources;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public LeisureParametersData m_LeisureParameters;
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
        NativeArray<LodgingProvider> nativeArray1 = chunk.GetNativeArray<LodgingProvider>(ref this.m_LodgingProviderType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ServiceAvailable> nativeArray3 = chunk.GetNativeArray<ServiceAvailable>(ref this.m_ServiceAvailableType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Renter> bufferAccessor = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity entity = nativeArray2[index1];
          // ISSUE: reference to a compiler-generated field
          if (this.m_PropertyRenters.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity prefab = this.m_Prefabs[this.m_PropertyRenters[entity].m_Property].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            Game.Prefabs.BuildingData buildingData = this.m_BuildingDatas[prefab];
            // ISSUE: reference to a compiler-generated field
            BuildingPropertyData buildingPropertyData = this.m_BuildingPropertyDatas[prefab];
            // ISSUE: reference to a compiler-generated field
            SpawnableBuildingData spawnableBuildingData = this.m_SpawnableBuildingDatas[prefab];
            // ISSUE: reference to a compiler-generated method
            int roomCount = LodgingProviderSystem.GetRoomCount(buildingData.m_LotSize, (int) spawnableBuildingData.m_Level, buildingPropertyData);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float marketPrice = EconomyUtils.GetMarketPrice(Resource.Lodging, this.m_ResourcePrefabs, ref this.m_ResourceDatas);
            // ISSUE: reference to a compiler-generated field
            int num1 = Mathf.RoundToInt(2f * (float) this.m_PropertyRenters[entity].m_Rent / (float) roomCount + marketPrice);
            // ISSUE: reference to a compiler-generated field
            int num2 = Mathf.CeilToInt((float) num1 / (float) LodgingProviderSystem.kUpdatesPerDay);
            DynamicBuffer<Renter> dynamicBuffer = bufferAccessor[index1];
            for (int index2 = dynamicBuffer.Length - 1; index2 >= 0; --index2)
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_TouristHouseholds.HasComponent(dynamicBuffer[index2].m_Renter))
                dynamicBuffer.RemoveAt(index2);
            }
            if (roomCount < dynamicBuffer.Length)
            {
              int num3 = dynamicBuffer.Length - roomCount;
              for (int index3 = dynamicBuffer.Length - 1; index3 >= 0 && num3 > 0; --index3)
              {
                // ISSUE: reference to a compiler-generated field
                TouristHousehold touristHousehold = this.m_TouristHouseholds[dynamicBuffer[index3].m_Renter] with
                {
                  m_Hotel = Entity.Null
                };
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<TouristHousehold>(unfilteredChunkIndex, dynamicBuffer[index3].m_Renter, touristHousehold);
                dynamicBuffer.RemoveAt(index3);
                --num3;
              }
            }
            int num4 = 0;
            for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
            {
              Entity renter = dynamicBuffer[index4].m_Renter;
              // ISSUE: reference to a compiler-generated field
              EconomyUtils.AddResources(Resource.Money, -num2, this.m_Resources[renter]);
              ++num4;
            }
            // ISSUE: reference to a compiler-generated field
            EconomyUtils.AddResources(Resource.Money, num4 * num2, this.m_Resources[entity]);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            EconomyUtils.AddResources(Resource.Lodging, -num4 * this.m_LeisureParameters.m_TouristLodgingConsumePerDay / LodgingProviderSystem.kUpdatesPerDay, this.m_Resources[entity]);
            ServiceAvailable serviceAvailable = nativeArray3[index1];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            serviceAvailable.m_ServiceAvailable = math.max(0, serviceAvailable.m_ServiceAvailable - num4 * this.m_LeisureParameters.m_TouristServiceConsumePerDay / LodgingProviderSystem.kUpdatesPerDay);
            nativeArray3[index1] = serviceAvailable;
            LodgingProvider lodgingProvider = nativeArray1[index1] with
            {
              m_Price = num1,
              m_FreeRooms = roomCount - dynamicBuffer.Length
            };
            nativeArray1[index1] = lodgingProvider;
          }
          else
          {
            DynamicBuffer<Renter> dynamicBuffer = bufferAccessor[index1];
            for (int index5 = dynamicBuffer.Length - 1; index5 >= 0; --index5)
              dynamicBuffer.RemoveAt(index5);
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
      public ComponentTypeHandle<ServiceAvailable> __Game_Companies_ServiceAvailable_RW_ComponentTypeHandle;
      public ComponentTypeHandle<LodgingProvider> __Game_Companies_LodgingProvider_RW_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      public BufferTypeHandle<Renter> __Game_Buildings_Renter_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> __Game_Citizens_TouristHousehold_RO_ComponentLookup;
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceAvailable_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceAvailable>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_LodgingProvider_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LodgingProvider>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RW_BufferTypeHandle = state.GetBufferTypeHandle<Renter>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TouristHousehold_RO_ComponentLookup = state.GetComponentLookup<TouristHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>();
      }
    }
  }
}
