// Decompiled with JetBrains decompiler
// Type: Game.Objects.ObjectEmergeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Creatures;
using Game.Pathfind;
using Game.Prefabs;
using Game.Rendering;
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
namespace Game.Objects
{
  [CompilerGenerated]
  public class ObjectEmergeSystem : GameSystemBase
  {
    private ModificationBarrier4B m_ModificationBarrier;
    private EntityQuery m_DeletedBuildingQuery;
    private EntityQuery m_DeletedVehicleQuery;
    private EntityQuery m_EmergeObjectQuery;
    private EntityQuery m_CreaturePrefabQuery;
    private ComponentTypeSet m_TripSourceRemoveTypes;
    private ComponentTypeSet m_CurrentVehicleRemoveTypes;
    private ComponentTypeSet m_CurrentVehicleHumanAddTypes;
    private ComponentTypeSet m_CurrentVehicleAnimalAddTypes;
    private ComponentTypeSet m_HumanSpawnTypes;
    private ComponentTypeSet m_AnimalSpawnTypes;
    private ObjectEmergeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4B>();
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Deleted>(), ComponentType.ReadOnly<Building>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedVehicleQuery = this.GetEntityQuery(ComponentType.ReadOnly<Deleted>(), ComponentType.ReadOnly<Passenger>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_EmergeObjectQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<CurrentBuilding>(),
          ComponentType.ReadOnly<TripSource>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_CreaturePrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<CreatureData>(), ComponentType.ReadOnly<PrefabData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TripSourceRemoveTypes = new ComponentTypeSet(ComponentType.ReadWrite<TripSource>(), ComponentType.ReadWrite<Unspawned>());
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentVehicleRemoveTypes = new ComponentTypeSet(ComponentType.ReadWrite<CurrentVehicle>(), ComponentType.ReadWrite<Relative>(), ComponentType.ReadWrite<Unspawned>());
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentVehicleHumanAddTypes = new ComponentTypeSet(new ComponentType[7]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<HumanNavigation>(),
        ComponentType.ReadWrite<HumanCurrentLane>(),
        ComponentType.ReadWrite<Blocker>(),
        ComponentType.ReadWrite<Updated>()
      });
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentVehicleAnimalAddTypes = new ComponentTypeSet(new ComponentType[7]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<AnimalNavigation>(),
        ComponentType.ReadWrite<AnimalCurrentLane>(),
        ComponentType.ReadWrite<Blocker>(),
        ComponentType.ReadWrite<Updated>()
      });
      // ISSUE: reference to a compiler-generated field
      this.m_HumanSpawnTypes = new ComponentTypeSet(ComponentType.ReadWrite<HumanCurrentLane>(), ComponentType.ReadWrite<TripSource>(), ComponentType.ReadWrite<Unspawned>(), ComponentType.ReadWrite<Divert>());
      // ISSUE: reference to a compiler-generated field
      this.m_AnimalSpawnTypes = new ComponentTypeSet(ComponentType.ReadWrite<AnimalCurrentLane>(), ComponentType.ReadWrite<TripSource>(), ComponentType.ReadWrite<Unspawned>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      bool flag1 = !this.m_DeletedBuildingQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      bool flag2 = !this.m_DeletedVehicleQuery.IsEmptyIgnoreFilter;
      if (!flag1 && !flag2)
        return;
      NativeQueue<Entity> nativeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<Entity>.ParallelWriter parallelWriter = nativeQueue.AsParallelWriter();
      if (flag1)
      {
        // ISSUE: reference to a compiler-generated field
        using (NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_DeletedBuildingQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
          for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
          {
            NativeArray<Entity> nativeArray = archetypeChunkArray[index1].GetNativeArray(entityTypeHandle);
            for (int index2 = 0; index2 < nativeArray.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Objects_TripSource_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              ObjectEmergeSystem.FindObjectsInBuildingJob jobData = new ObjectEmergeSystem.FindObjectsInBuildingJob()
              {
                m_Building = nativeArray[index2],
                m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
                m_CurrentBuildingType = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle,
                m_TripSourceType = this.__TypeHandle.__Game_Objects_TripSource_RO_ComponentTypeHandle,
                m_EmergeQueue = parallelWriter
              };
              // ISSUE: reference to a compiler-generated field
              this.Dependency = jobData.ScheduleParallel<ObjectEmergeSystem.FindObjectsInBuildingJob>(this.m_EmergeObjectQuery, this.Dependency);
            }
          }
        }
      }
      if (flag2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ObjectEmergeSystem.FindObjectsInVehiclesJob jobData = new ObjectEmergeSystem.FindObjectsInVehiclesJob()
        {
          m_PassengerType = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle,
          m_EmergeQueue = parallelWriter
        };
        // ISSUE: reference to a compiler-generated field
        this.Dependency = jobData.ScheduleParallel<ObjectEmergeSystem.FindObjectsInVehiclesJob>(this.m_DeletedVehicleQuery, this.Dependency);
      }
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_CreaturePrefabQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HouseholdPetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Animal_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Human_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdPet_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TripSource_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResidentData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PetData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CreatureData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle jobHandle = new ObjectEmergeSystem.EmergeObjectsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CreatureDataType = this.__TypeHandle.__Game_Prefabs_CreatureData_RO_ComponentTypeHandle,
        m_PetDataType = this.__TypeHandle.__Game_Prefabs_PetData_RO_ComponentTypeHandle,
        m_ResidentDataType = this.__TypeHandle.__Game_Prefabs_ResidentData_RO_ComponentTypeHandle,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_TripSourceData = this.__TypeHandle.__Game_Objects_TripSource_RO_ComponentLookup,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_CitizenData = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_HouseholdPetData = this.__TypeHandle.__Game_Citizens_HouseholdPet_RO_ComponentLookup,
        m_CurrentBuildingData = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup,
        m_CurrentTransportData = this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup,
        m_HealthProblemData = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_HumanData = this.__TypeHandle.__Game_Creatures_Human_RO_ComponentLookup,
        m_AnimalData = this.__TypeHandle.__Game_Creatures_Animal_RO_ComponentLookup,
        m_ResidentData = this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup,
        m_HumanCurrentLaneData = this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentLookup,
        m_AnimalCurrentLane = this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectData = this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup,
        m_PrefabHouseholdPetData = this.__TypeHandle.__Game_Prefabs_HouseholdPetData_RO_ComponentLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_TripSourceRemoveTypes = this.m_TripSourceRemoveTypes,
        m_CurrentVehicleRemoveTypes = this.m_CurrentVehicleRemoveTypes,
        m_CurrentVehicleHumanAddTypes = this.m_CurrentVehicleHumanAddTypes,
        m_CurrentVehicleAnimalAddTypes = this.m_CurrentVehicleAnimalAddTypes,
        m_HumanSpawnTypes = this.m_HumanSpawnTypes,
        m_AnimalSpawnTypes = this.m_AnimalSpawnTypes,
        m_CreaturePrefabChunks = archetypeChunkListAsync,
        m_EmergeQueue = nativeQueue,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<ObjectEmergeSystem.EmergeObjectsJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      nativeQueue.Dispose(jobHandle);
      archetypeChunkListAsync.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = jobHandle;
    }

    public static Entity SelectResidentPrefab(
      Citizen citizenData,
      NativeList<ArchetypeChunk> chunks,
      EntityTypeHandle entityType,
      ref ComponentTypeHandle<CreatureData> creatureType,
      ref ComponentTypeHandle<ResidentData> residentType,
      out CreatureData creatureData,
      out PseudoRandomSeed randomSeed)
    {
      Random pseudoRandom = citizenData.GetPseudoRandom(CitizenPseudoRandom.SpawnResident);
      GenderMask genderMask = (citizenData.m_State & CitizenFlags.Male) != CitizenFlags.None ? GenderMask.Male : GenderMask.Female;
      Game.Prefabs.AgeMask ageMask;
      switch (citizenData.GetAge())
      {
        case CitizenAge.Child:
          ageMask = Game.Prefabs.AgeMask.Child;
          break;
        case CitizenAge.Teen:
          ageMask = Game.Prefabs.AgeMask.Teen;
          break;
        case CitizenAge.Adult:
          ageMask = Game.Prefabs.AgeMask.Adult;
          break;
        case CitizenAge.Elderly:
          ageMask = Game.Prefabs.AgeMask.Elderly;
          break;
        default:
          ageMask = (Game.Prefabs.AgeMask) 0;
          break;
      }
      Entity entity = Entity.Null;
      int totalProbability = 0;
      creatureData = new CreatureData();
      randomSeed = new PseudoRandomSeed(ref pseudoRandom);
      for (int index1 = 0; index1 < chunks.Length; ++index1)
      {
        ArchetypeChunk chunk = chunks[index1];
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(entityType);
        NativeArray<CreatureData> nativeArray2 = chunk.GetNativeArray<CreatureData>(ref creatureType);
        NativeArray<ResidentData> nativeArray3 = chunk.GetNativeArray<ResidentData>(ref residentType);
        for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
        {
          CreatureData creatureData1 = nativeArray2[index2];
          ResidentData residentData = nativeArray3[index2];
          if ((creatureData1.m_Gender & genderMask) == genderMask && (residentData.m_Age & ageMask) == ageMask)
          {
            int probability = 100;
            // ISSUE: reference to a compiler-generated method
            if (ObjectEmergeSystem.SelectItem(ref pseudoRandom, probability, ref totalProbability))
            {
              entity = nativeArray1[index2];
              creatureData = creatureData1;
            }
          }
        }
      }
      return entity;
    }

    public static Entity SelectAnimalPrefab(
      ref Random random,
      PetType petType,
      NativeList<ArchetypeChunk> chunks,
      EntityTypeHandle entityType,
      ComponentTypeHandle<PetData> petDataType,
      out PseudoRandomSeed randomSeed)
    {
      int totalProbability = 0;
      Entity entity = Entity.Null;
      for (int index1 = 0; index1 < chunks.Length; ++index1)
      {
        ArchetypeChunk chunk = chunks[index1];
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(entityType);
        NativeArray<PetData> nativeArray2 = chunk.GetNativeArray<PetData>(ref petDataType);
        for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
        {
          // ISSUE: reference to a compiler-generated method
          if (nativeArray2[index2].m_Type == petType && ObjectEmergeSystem.SelectItem(ref random, 100, ref totalProbability))
            entity = nativeArray1[index2];
        }
      }
      randomSeed = new PseudoRandomSeed(ref random);
      return entity;
    }

    private static bool SelectItem(ref Random random, int probability, ref int totalProbability)
    {
      totalProbability += probability;
      return random.NextInt(totalProbability) < probability;
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
    public ObjectEmergeSystem()
    {
    }

    [BurstCompile]
    private struct FindObjectsInBuildingJob : IJobChunk
    {
      [ReadOnly]
      public Entity m_Building;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> m_CurrentBuildingType;
      [ReadOnly]
      public ComponentTypeHandle<TripSource> m_TripSourceType;
      public NativeQueue<Entity>.ParallelWriter m_EmergeQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentBuilding> nativeArray2 = chunk.GetNativeArray<CurrentBuilding>(ref this.m_CurrentBuildingType);
        if (nativeArray2.Length != 0)
        {
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            if (nativeArray2[index].m_CurrentBuilding == this.m_Building)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_EmergeQueue.Enqueue(nativeArray1[index]);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<TripSource> nativeArray3 = chunk.GetNativeArray<TripSource>(ref this.m_TripSourceType);
          if (nativeArray3.Length == 0)
            return;
          for (int index = 0; index < nativeArray3.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            if (nativeArray3[index].m_Source == this.m_Building)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_EmergeQueue.Enqueue(nativeArray1[index]);
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
    private struct FindObjectsInVehiclesJob : IJobChunk
    {
      [ReadOnly]
      public BufferTypeHandle<Passenger> m_PassengerType;
      public NativeQueue<Entity>.ParallelWriter m_EmergeQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Passenger> bufferAccessor = chunk.GetBufferAccessor<Passenger>(ref this.m_PassengerType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<Passenger> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_EmergeQueue.Enqueue(dynamicBuffer[index2].m_Passenger);
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
    private struct EmergeObjectsJob : IJob
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CreatureData> m_CreatureDataType;
      [ReadOnly]
      public ComponentTypeHandle<PetData> m_PetDataType;
      [ReadOnly]
      public ComponentTypeHandle<ResidentData> m_ResidentDataType;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<TripSource> m_TripSourceData;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenData;
      [ReadOnly]
      public ComponentLookup<HouseholdPet> m_HouseholdPetData;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> m_CurrentBuildingData;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> m_CurrentTransportData;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblemData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public ComponentLookup<Human> m_HumanData;
      [ReadOnly]
      public ComponentLookup<Animal> m_AnimalData;
      [ReadOnly]
      public ComponentLookup<Game.Creatures.Resident> m_ResidentData;
      [ReadOnly]
      public ComponentLookup<HumanCurrentLane> m_HumanCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<AnimalCurrentLane> m_AnimalCurrentLane;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectData> m_PrefabObjectData;
      [ReadOnly]
      public ComponentLookup<HouseholdPetData> m_PrefabHouseholdPetData;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public ComponentTypeSet m_TripSourceRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_CurrentVehicleRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_CurrentVehicleHumanAddTypes;
      [ReadOnly]
      public ComponentTypeSet m_CurrentVehicleAnimalAddTypes;
      [ReadOnly]
      public ComponentTypeSet m_HumanSpawnTypes;
      [ReadOnly]
      public ComponentTypeSet m_AnimalSpawnTypes;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_CreaturePrefabChunks;
      public NativeQueue<Entity> m_EmergeQueue;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_EmergeQueue.Count;
        if (count == 0)
          return;
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_EmergeQueue.Dequeue();
          // ISSUE: reference to a compiler-generated field
          if (this.m_TripSourceData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_UpdatedData.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent(entity, in this.m_TripSourceRemoveTypes);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<BatchesUpdated>(entity, new BatchesUpdated());
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurrentVehicleData.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              CurrentVehicle currentVehicle = this.m_CurrentVehicleData[entity];
              // ISSUE: reference to a compiler-generated method
              this.ExitVehicle(entity, currentVehicle.m_Vehicle);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_CurrentBuildingData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                CurrentBuilding currentBuilding = this.m_CurrentBuildingData[entity];
                // ISSUE: reference to a compiler-generated method
                this.ExitBuilding(entity, currentBuilding.m_CurrentBuilding);
              }
            }
          }
        }
      }

      private void ExitVehicle(Entity creature, Entity vehicle)
      {
        // ISSUE: reference to a compiler-generated field
        Transform component1 = this.m_TransformData[creature];
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.HasComponent(vehicle))
        {
          // ISSUE: reference to a compiler-generated field
          component1 = this.m_TransformData[vehicle];
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent(creature, in this.m_CurrentVehicleRemoveTypes);
        // ISSUE: reference to a compiler-generated field
        int num = this.m_HumanData.HasComponent(creature) ? 1 : 0;
        // ISSUE: reference to a compiler-generated field
        bool flag = this.m_AnimalData.HasComponent(creature);
        CreatureLaneFlags flags = CreatureLaneFlags.Obsolete;
        // ISSUE: reference to a compiler-generated field
        if (this.m_UnspawnedData.HasComponent(vehicle))
        {
          flags |= CreatureLaneFlags.EmergeUnspawned;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Unspawned>(creature, new Unspawned());
        }
        // ISSUE: reference to a compiler-generated field
        if (num != 0 && !this.m_HumanCurrentLaneData.HasComponent(creature))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent(creature, in this.m_CurrentVehicleHumanAddTypes);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Transform>(creature, component1);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<HumanCurrentLane>(creature, new HumanCurrentLane(flags));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (flag && !this.m_AnimalCurrentLane.HasComponent(creature))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent(creature, in this.m_CurrentVehicleAnimalAddTypes);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Transform>(creature, component1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<AnimalCurrentLane>(creature, new AnimalCurrentLane(flags));
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ResidentData.HasComponent(creature))
          return;
        // ISSUE: reference to a compiler-generated field
        Game.Creatures.Resident component2 = this.m_ResidentData[creature];
        component2.m_Flags &= ~ResidentFlags.InVehicle;
        component2.m_Timer = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Game.Creatures.Resident>(creature, component2);
      }

      private void ExitBuilding(Entity entity, Entity building)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<CurrentBuilding>(entity);
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurrentTransportData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          CurrentTransport currentTransport = this.m_CurrentTransportData[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_TripSourceData.HasComponent(currentTransport.m_CurrentTransport) || this.m_UpdatedData.HasComponent(currentTransport.m_CurrentTransport))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent(currentTransport.m_CurrentTransport, in this.m_TripSourceRemoveTypes);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_CitizenData.HasComponent(entity))
          {
            bool isDead = false;
            HealthProblem componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_HealthProblemData.TryGetComponent(entity, out componentData))
              isDead = (componentData.m_Flags & HealthProblemFlags.Dead) != 0;
            // ISSUE: reference to a compiler-generated method
            this.SpawnResident(entity, building, isDead);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_HouseholdPetData.HasComponent(entity))
              return;
            // ISSUE: reference to a compiler-generated method
            this.SpawnPet(entity, building);
          }
        }
      }

      private void SpawnResident(Entity citizen, Entity building, bool isDead)
      {
        if (!isDead)
        {
          PseudoRandomSeed randomSeed;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          Entity entity1 = ObjectEmergeSystem.SelectResidentPrefab(this.m_CitizenData[citizen], this.m_CreaturePrefabChunks, this.m_EntityType, ref this.m_CreatureDataType, ref this.m_ResidentDataType, out CreatureData _, out randomSeed);
          // ISSUE: reference to a compiler-generated field
          ObjectData objectData = this.m_PrefabObjectData[entity1];
          PrefabRef component1 = new PrefabRef()
          {
            m_Prefab = entity1
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Transform component2 = !this.m_TransformData.HasComponent(building) ? new Transform(new float3(), quaternion.identity) : this.m_TransformData[building];
          Game.Creatures.Resident component3 = new Game.Creatures.Resident();
          component3.m_Citizen = citizen;
          PathOwner component4 = new PathOwner(PathFlags.Obsolete);
          TripSource component5 = new TripSource(building);
          HumanCurrentLane component6 = new HumanCurrentLane(CreatureLaneFlags.Obsolete);
          Divert component7 = new Divert()
          {
            m_Purpose = Game.Citizens.Purpose.Safety
          };
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(objectData.m_Archetype);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent(entity2, in this.m_HumanSpawnTypes);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Transform>(entity2, component2);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(entity2, component1);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Game.Creatures.Resident>(entity2, component3);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PathOwner>(entity2, component4);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(entity2, randomSeed);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<HumanCurrentLane>(entity2, component6);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<TripSource>(entity2, component5);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Divert>(entity2, component7);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CurrentTransport>(citizen, new CurrentTransport(entity2));
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<TravelPurpose>(citizen, new TravelPurpose()
        {
          m_Purpose = Game.Citizens.Purpose.GoingHome
        });
      }

      private void SpawnPet(Entity householdPet, Entity building)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        HouseholdPetData householdPetData = this.m_PrefabHouseholdPetData[this.m_PrefabRefData[householdPet].m_Prefab];
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(householdPet.Index);
        PseudoRandomSeed randomSeed;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Entity entity1 = ObjectEmergeSystem.SelectAnimalPrefab(ref random, householdPetData.m_Type, this.m_CreaturePrefabChunks, this.m_EntityType, this.m_PetDataType, out randomSeed);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity2 = this.m_CommandBuffer.CreateEntity(this.m_PrefabObjectData[entity1].m_Archetype);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(entity2, in this.m_AnimalSpawnTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(entity2, new PrefabRef(entity1));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Transform>(entity2, this.m_TransformData[building]);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Game.Creatures.Pet>(entity2, new Game.Creatures.Pet(householdPet));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(entity2, randomSeed);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<TripSource>(entity2, new TripSource(building));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<AnimalCurrentLane>(entity2, new AnimalCurrentLane(CreatureLaneFlags.Obsolete));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CurrentTransport>(householdPet, new CurrentTransport(entity2));
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TripSource> __Game_Objects_TripSource_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Passenger> __Game_Vehicles_Passenger_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CreatureData> __Game_Prefabs_CreatureData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PetData> __Game_Prefabs_PetData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ResidentData> __Game_Prefabs_ResidentData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TripSource> __Game_Objects_TripSource_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdPet> __Game_Citizens_HouseholdPet_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> __Game_Citizens_CurrentTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Human> __Game_Creatures_Human_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Animal> __Game_Creatures_Animal_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Creatures.Resident> __Game_Creatures_Resident_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HumanCurrentLane> __Game_Creatures_HumanCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AnimalCurrentLane> __Game_Creatures_AnimalCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectData> __Game_Prefabs_ObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdPetData> __Game_Prefabs_HouseholdPetData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TripSource_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TripSource>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RO_BufferTypeHandle = state.GetBufferTypeHandle<Passenger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CreatureData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CreatureData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PetData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResidentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ResidentData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TripSource_RO_ComponentLookup = state.GetComponentLookup<TripSource>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdPet_RO_ComponentLookup = state.GetComponentLookup<HouseholdPet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentLookup = state.GetComponentLookup<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentTransport_RO_ComponentLookup = state.GetComponentLookup<CurrentTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentLookup = state.GetComponentLookup<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Human_RO_ComponentLookup = state.GetComponentLookup<Human>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Animal_RO_ComponentLookup = state.GetComponentLookup<Animal>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Resident_RO_ComponentLookup = state.GetComponentLookup<Game.Creatures.Resident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanCurrentLane_RO_ComponentLookup = state.GetComponentLookup<HumanCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalCurrentLane_RO_ComponentLookup = state.GetComponentLookup<AnimalCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectData_RO_ComponentLookup = state.GetComponentLookup<ObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HouseholdPetData_RO_ComponentLookup = state.GetComponentLookup<HouseholdPetData>(true);
      }
    }
  }
}
