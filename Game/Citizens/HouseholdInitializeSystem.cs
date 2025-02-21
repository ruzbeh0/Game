// Decompiled with JetBrains decompiler
// Type: Game.Citizens.HouseholdInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Agents;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Economy;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

#nullable disable
namespace Game.Citizens
{
  [CompilerGenerated]
  public class HouseholdInitializeSystem : GameSystemBase
  {
    private EntityQuery m_CarPrefabGroup;
    private EntityQuery m_CitizenPrefabGroup;
    private EntityQuery m_HouseholdPetPrefabGroup;
    private EntityQuery m_Additions;
    private ModificationBarrier4 m_EndFrameBarrier;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private PersonalCarSelectData m_PersonalCarSelectData;
    private HouseholdInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PersonalCarSelectData = new PersonalCarSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_CarPrefabGroup = this.GetEntityQuery(PersonalCarSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenPrefabGroup = this.GetEntityQuery(ComponentType.ReadOnly<CitizenData>(), ComponentType.ReadOnly<Game.Prefabs.ArchetypeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdPetPrefabGroup = this.GetEntityQuery(ComponentType.ReadOnly<HouseholdPetData>(), ComponentType.ReadOnly<Game.Prefabs.ArchetypeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_Additions = this.GetEntityQuery(ComponentType.ReadWrite<Household>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadWrite<HouseholdCitizen>(), ComponentType.ReadOnly<CurrentBuilding>(), ComponentType.ReadWrite<Game.Economy.Resources>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Additions);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PersonalCarSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_CarPrefabGroup, Allocator.TempJob, out jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DynamicHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HouseholdData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CommuterHousehold_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TouristHousehold_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle1;
      JobHandle outJobHandle2;
      JobHandle outJobHandle3;
      JobHandle outJobHandle4;
      JobHandle deps;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = new HouseholdInitializeSystem.InitializeHouseholdJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CurrentBuildingType = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_ResourceType = this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle,
        m_TouristHouseholdType = this.__TypeHandle.__Game_Citizens_TouristHousehold_RW_ComponentTypeHandle,
        m_CommuterHouseholdType = this.__TypeHandle.__Game_Citizens_CommuterHousehold_RO_ComponentTypeHandle,
        m_HouseholdType = this.__TypeHandle.__Game_Citizens_Household_RW_ComponentTypeHandle,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_HouseholdDatas = this.__TypeHandle.__Game_Prefabs_HouseholdData_RO_ComponentLookup,
        m_DynamicHouseholds = this.__TypeHandle.__Game_Prefabs_DynamicHousehold_RO_ComponentLookup,
        m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_OutsideConnectionDatas = this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup,
        m_CitizenPrefabs = this.m_CitizenPrefabGroup.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle1),
        m_HouseholdPetPrefabs = this.m_HouseholdPetPrefabGroup.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle2),
        m_CitizenPrefabArchetypes = this.m_CitizenPrefabGroup.ToComponentDataListAsync<Game.Prefabs.ArchetypeData>((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle3),
        m_HouseholdPetArchetypes = this.m_HouseholdPetPrefabGroup.ToComponentDataListAsync<Game.Prefabs.ArchetypeData>((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle4),
        m_StatisticsQueue = this.m_CityStatisticsSystem.GetSafeStatisticsQueue(out deps),
        m_RandomSeed = RandomSeed.Next(),
        m_PersonalCarSelectData = this.m_PersonalCarSelectData
      }.ScheduleParallel<HouseholdInitializeSystem.InitializeHouseholdJob>(this.m_Additions, JobUtils.CombineDependencies(this.Dependency, outJobHandle1, outJobHandle2, outJobHandle3, outJobHandle4, deps, jobHandle1));
      // ISSUE: reference to a compiler-generated field
      this.m_PersonalCarSelectData.PostUpdate(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(jobHandle2);
      this.Dependency = jobHandle2;
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
    public HouseholdInitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeHouseholdJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      public BufferTypeHandle<Game.Economy.Resources> m_ResourceType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> m_CurrentBuildingType;
      [ReadOnly]
      public ComponentTypeHandle<TouristHousehold> m_TouristHouseholdType;
      [ReadOnly]
      public ComponentTypeHandle<CommuterHousehold> m_CommuterHouseholdType;
      public ComponentTypeHandle<Household> m_HouseholdType;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.HouseholdData> m_HouseholdDatas;
      [ReadOnly]
      public ComponentLookup<DynamicHousehold> m_DynamicHouseholds;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> m_OutsideConnectionDatas;
      [ReadOnly]
      public NativeList<Entity> m_CitizenPrefabs;
      [ReadOnly]
      public NativeList<Entity> m_HouseholdPetPrefabs;
      [ReadOnly]
      public NativeList<Game.Prefabs.ArchetypeData> m_CitizenPrefabArchetypes;
      [ReadOnly]
      public NativeList<Game.Prefabs.ArchetypeData> m_HouseholdPetArchetypes;
      [ReadOnly]
      public PersonalCarSelectData m_PersonalCarSelectData;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      public CityStatisticsSystem.SafeStatisticQueue m_StatisticsQueue;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      private void SpawnCitizen(
        int index,
        Entity household,
        ref Unity.Mathematics.Random random,
        CurrentBuilding building,
        int age,
        bool tourist,
        bool commuter)
      {
        // ISSUE: reference to a compiler-generated field
        int index1 = random.NextInt(this.m_CitizenPrefabs.Length);
        // ISSUE: reference to a compiler-generated field
        Entity citizenPrefab = this.m_CitizenPrefabs[index1];
        // ISSUE: reference to a compiler-generated field
        Game.Prefabs.ArchetypeData citizenPrefabArchetype = this.m_CitizenPrefabArchetypes[index1];
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(index, citizenPrefabArchetype.m_Archetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(index, entity, new PrefabRef()
        {
          m_Prefab = citizenPrefab
        });
        HouseholdMember component1 = new HouseholdMember()
        {
          m_Household = household
        };
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<HouseholdMember>(index, entity, component1);
        CitizenFlags citizenFlags = CitizenFlags.None;
        if (tourist)
          citizenFlags |= CitizenFlags.Tourist;
        if (commuter)
          citizenFlags |= CitizenFlags.Commuter;
        Citizen component2 = new Citizen()
        {
          m_BirthDay = (short) age,
          m_State = citizenFlags
        };
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Citizen>(index, entity, component2);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CurrentBuilding>(index, entity, building);
      }

      private void SpawnHouseholdPet(
        int index,
        Entity household,
        ref Unity.Mathematics.Random random,
        CurrentBuilding building)
      {
        // ISSUE: reference to a compiler-generated field
        int index1 = random.NextInt(this.m_HouseholdPetPrefabs.Length);
        // ISSUE: reference to a compiler-generated field
        Entity householdPetPrefab = this.m_HouseholdPetPrefabs[index1];
        // ISSUE: reference to a compiler-generated field
        Game.Prefabs.ArchetypeData householdPetArchetype = this.m_HouseholdPetArchetypes[index1];
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(index, householdPetArchetype.m_Archetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(index, entity, new PrefabRef()
        {
          m_Prefab = householdPetPrefab
        });
        HouseholdPet component = new HouseholdPet()
        {
          m_Household = household
        };
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<HouseholdPet>(index, entity, component);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CurrentBuilding>(index, entity, building);
      }

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Economy.Resources> bufferAccessor = chunk.GetBufferAccessor<Game.Economy.Resources>(ref this.m_ResourceType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentBuilding> nativeArray3 = chunk.GetNativeArray<CurrentBuilding>(ref this.m_CurrentBuildingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Household> nativeArray4 = chunk.GetNativeArray<Household>(ref this.m_HouseholdType);
        // ISSUE: reference to a compiler-generated field
        bool tourist = chunk.Has<TouristHousehold>(ref this.m_TouristHouseholdType);
        // ISSUE: reference to a compiler-generated field
        bool commuter = chunk.Has<CommuterHousehold>(ref this.m_CommuterHouseholdType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity entity1 = nativeArray1[index1];
          Entity prefab = nativeArray2[index1].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_DynamicHouseholds.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Prefabs.HouseholdData householdData = this.m_HouseholdDatas[prefab];
            DynamicBuffer<Game.Economy.Resources> resources = bufferAccessor[index1];
            // ISSUE: reference to a compiler-generated field
            int amount = !chunk.Has<TouristHousehold>(ref this.m_TouristHouseholdType) ? random.NextInt(householdData.m_InitialWealthRange) - householdData.m_InitialWealthRange / 2 + householdData.m_InitialWealthOffset : Mathf.RoundToInt((float) ((2.0 + 19.0 * (double) random.NextFloat() + 19.0 * (double) random.NextFloat()) * 50.0));
            EconomyUtils.AddResources(Resource.Money, amount, resources);
            CurrentBuilding building = nativeArray3[index1];
            for (int index2 = 0; index2 < householdData.m_StudentCount; ++index2)
            {
              // ISSUE: reference to a compiler-generated method
              this.SpawnCitizen(unfilteredChunkIndex, entity1, ref random, building, 4, tourist, commuter);
            }
            for (int index3 = 0; index3 < householdData.m_AdultCount; ++index3)
            {
              // ISSUE: reference to a compiler-generated method
              this.SpawnCitizen(unfilteredChunkIndex, entity1, ref random, building, 1, tourist, commuter);
            }
            int num1 = random.NextInt(householdData.m_ChildCount);
            for (int index4 = 0; index4 < num1; ++index4)
            {
              // ISSUE: reference to a compiler-generated method
              this.SpawnCitizen(unfilteredChunkIndex, entity1, ref random, building, 2, tourist, commuter);
            }
            for (int index5 = 0; index5 < householdData.m_ElderCount; ++index5)
            {
              // ISSUE: reference to a compiler-generated method
              this.SpawnCitizen(unfilteredChunkIndex, entity1, ref random, building, 3, tourist, commuter);
            }
            int num2 = 0;
            if (!commuter && random.NextInt(100) < householdData.m_FirstPetProbability)
            {
              do
              {
                // ISSUE: reference to a compiler-generated method
                this.SpawnHouseholdPet(unfilteredChunkIndex, entity1, ref random, building);
              }
              while (++num2 < 4 && random.NextInt(100) < householdData.m_NextPetProbability);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddBuffer<HouseholdAnimal>(unfilteredChunkIndex, entity1);
            }
            int num3 = householdData.m_AdultCount + num1 + householdData.m_ElderCount;
            bool flag = false;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((BuildingUtils.GetOutsideConnectionType(building.m_CurrentBuilding, ref this.m_PrefabRefs, ref this.m_OutsideConnectionDatas) & OutsideConnectionTransferType.Road) != OutsideConnectionTransferType.None)
              flag = true;
            if (flag)
            {
              Entity entity2 = entity1;
              Entity currentBuilding = nativeArray3[index1].m_CurrentBuilding;
              // ISSUE: reference to a compiler-generated field
              if (this.m_TransformData.HasComponent(currentBuilding))
              {
                // ISSUE: reference to a compiler-generated field
                Game.Objects.Transform transform = this.m_TransformData[currentBuilding];
                int passengerAmount = num3;
                int baggageAmount = 1 + num2;
                if (random.NextInt(20) == 0)
                {
                  passengerAmount += 5;
                  baggageAmount += 5;
                }
                else if (random.NextInt(10) == 0)
                {
                  baggageAmount += 5;
                  if (random.NextInt(10) == 0)
                    baggageAmount += 5;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity vehicle = this.m_PersonalCarSelectData.CreateVehicle(this.m_CommandBuffer, unfilteredChunkIndex, ref random, passengerAmount, baggageAmount, false, true, transform, currentBuilding, Entity.Null, (PersonalCarFlags) 0, true);
                if (vehicle != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Owner>(unfilteredChunkIndex, vehicle, new Owner(entity2));
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddBuffer<OwnedVehicle>(unfilteredChunkIndex, entity2);
                }
              }
            }
            Household household = nativeArray4[index1] with
            {
              m_Resources = random.NextInt(1000 * num3)
            };
            nativeArray4[index1] = household;
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<TouristHousehold>(ref this.m_TouristHouseholdType))
            {
              TouristHousehold component = new TouristHousehold()
              {
                m_LeavingTime = 0,
                m_Hotel = Entity.Null
              };
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<TouristHousehold>(unfilteredChunkIndex, entity1, component);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<LodgingSeeker>(unfilteredChunkIndex, entity1, new LodgingSeeker());
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_StatisticsQueue.Enqueue(new StatisticsEvent()
              {
                m_Statistic = StatisticType.TouristIncome,
                m_Change = (float) amount
              });
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (!chunk.Has<CommuterHousehold>(ref this.m_CommuterHouseholdType))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<PropertySeeker>(unfilteredChunkIndex, entity1, new PropertySeeker());
              }
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<CurrentBuilding>(unfilteredChunkIndex, entity1);
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
      public ComponentTypeHandle<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public BufferTypeHandle<Game.Economy.Resources> __Game_Economy_Resources_RW_BufferTypeHandle;
      public ComponentTypeHandle<TouristHousehold> __Game_Citizens_TouristHousehold_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CommuterHousehold> __Game_Citizens_CommuterHousehold_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Household> __Game_Citizens_Household_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.HouseholdData> __Game_Prefabs_HouseholdData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<DynamicHousehold> __Game_Prefabs_DynamicHousehold_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> __Game_Prefabs_OutsideConnectionData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferTypeHandle = state.GetBufferTypeHandle<Game.Economy.Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TouristHousehold_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TouristHousehold>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CommuterHousehold_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CommuterHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Household>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HouseholdData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.HouseholdData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DynamicHousehold_RO_ComponentLookup = state.GetComponentLookup<DynamicHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup = state.GetComponentLookup<OutsideConnectionData>(true);
      }
    }
  }
}
