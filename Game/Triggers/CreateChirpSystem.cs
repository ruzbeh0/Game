// Decompiled with JetBrains decompiler
// Type: Game.Triggers.CreateChirpSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Assertions;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Triggers
{
  [CompilerGenerated]
  public class CreateChirpSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private ModificationEndBarrier m_ModificationBarrier;
    private JobHandle m_WriteDependencies;
    private EntityQuery m_PrefabQuery;
    private EntityQuery m_ChirpQuery;
    private EntityQuery m_CitizenQuery;
    private NativeQueue<ChirpCreationData> m_Queue;
    private CreateChirpSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationEndBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<ChirpData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ChirpQuery = this.GetEntityQuery(ComponentType.ReadOnly<Chirp>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.ReadOnly<HouseholdMember>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_Queue = new NativeQueue<ChirpCreationData>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.Enabled = false;
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      this.Enabled = mode.IsGame();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_Queue.Dispose();
      base.OnDestroy();
    }

    public NativeQueue<ChirpCreationData> GetQueue(out JobHandle deps)
    {
      Assert.IsTrue(this.Enabled, "Can not write to queue when system isn't running");
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_Queue;
    }

    public void AddQueueWriter(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = JobHandle.CombineDependencies(this.m_WriteDependencies, handle);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      NativeParallelHashMap<Entity, Entity> nativeParallelHashMap = new NativeParallelHashMap<Entity, Entity>(this.m_PrefabQuery.CalculateEntityCount(), (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle job3 = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ChirpQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ChirpData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Triggers_Chirp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        job3 = new CreateChirpSystem.CollectRecentChirpsJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_ChirpType = this.__TypeHandle.__Game_Triggers_Chirp_RO_ComponentTypeHandle,
          m_ChirpDatas = this.__TypeHandle.__Game_Prefabs_ChirpData_RO_ComponentLookup,
          m_RecentChirps = nativeParallelHashMap.AsParallelWriter(),
          m_SimulationFrame = this.m_SimulationSystem.frameIndex
        }.ScheduleParallel<CreateChirpSystem.CollectRecentChirpsJob>(this.m_ChirpQuery, this.Dependency);
      }
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_CitizenQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Triggers_LifePathEntry_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HomelessHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceChirpData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RandomLikeCountData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BrandChirpData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LifePathEventData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ChirpData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TriggerChirpData_RO_BufferLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CreateChirpSystem.CreateChirpJob jobData = new CreateChirpSystem.CreateChirpJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TriggerChirpData = this.__TypeHandle.__Game_Prefabs_TriggerChirpData_RO_BufferLookup,
        m_ChirpData = this.__TypeHandle.__Game_Prefabs_ChirpData_RO_ComponentLookup,
        m_LifepathEventData = this.__TypeHandle.__Game_Prefabs_LifePathEventData_RO_ComponentLookup,
        m_BrandChirpData = this.__TypeHandle.__Game_Prefabs_BrandChirpData_RO_ComponentLookup,
        m_RandomLikeCountData = this.__TypeHandle.__Game_Prefabs_RandomLikeCountData_RO_ComponentLookup,
        m_ServiceChirpDatas = this.__TypeHandle.__Game_Prefabs_ServiceChirpData_RO_ComponentLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_HealthProblems = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
        m_HouseholdMembers = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_Households = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_HomelessHouseholds = this.__TypeHandle.__Game_Citizens_HomelessHousehold_RO_ComponentLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_Employees = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup,
        m_LifepathEntries = this.__TypeHandle.__Game_Triggers_LifePathEntry_RO_BufferLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer(),
        m_Queue = this.m_Queue,
        m_RecentChirps = nativeParallelHashMap,
        m_RandomCitizenChunks = archetypeChunkListAsync,
        m_RandomSeed = RandomSeed.Next(),
        m_UneducatedPopulation = this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.EducationCount, 0) + this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.EducationCount, 1),
        m_EducatedPopulation = this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.EducationCount, 2) + this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.EducationCount, 3) + this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.EducationCount, 4),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<CreateChirpSystem.CreateChirpJob>(JobUtils.CombineDependencies(this.Dependency, this.m_WriteDependencies, outJobHandle, job3));
      nativeParallelHashMap.Dispose(this.Dependency);
      archetypeChunkListAsync.Dispose(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = this.Dependency;
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
    public CreateChirpSystem()
    {
    }

    [BurstCompile]
    public struct CollectRecentChirpsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<Chirp> m_ChirpType;
      [ReadOnly]
      public ComponentLookup<ChirpData> m_ChirpDatas;
      public NativeParallelHashMap<Entity, Entity>.ParallelWriter m_RecentChirps;
      public uint m_SimulationFrame;

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
        NativeArray<Chirp> nativeArray3 = chunk.GetNativeArray<Chirp>(ref this.m_ChirpType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (nativeArray3[index].m_CreationFrame + 18000U >= this.m_SimulationFrame)
          {
            Entity prefab = nativeArray2[index].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ChirpDatas.HasComponent(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_RecentChirps.TryAdd(prefab, nativeArray1[index]);
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
    private struct CreateChirpJob : IJob
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferLookup<TriggerChirpData> m_TriggerChirpData;
      [ReadOnly]
      public ComponentLookup<ChirpData> m_ChirpData;
      [ReadOnly]
      public ComponentLookup<LifePathEventData> m_LifepathEventData;
      [ReadOnly]
      public ComponentLookup<BrandChirpData> m_BrandChirpData;
      [ReadOnly]
      public ComponentLookup<RandomLikeCountData> m_RandomLikeCountData;
      [ReadOnly]
      public ComponentLookup<ServiceChirpData> m_ServiceChirpDatas;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblems;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMembers;
      [ReadOnly]
      public ComponentLookup<Household> m_Households;
      [ReadOnly]
      public ComponentLookup<HomelessHousehold> m_HomelessHouseholds;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public BufferLookup<Employee> m_Employees;
      [ReadOnly]
      public BufferLookup<LifePathEntry> m_LifepathEntries;
      public EntityCommandBuffer m_CommandBuffer;
      public NativeQueue<ChirpCreationData> m_Queue;
      public NativeParallelHashMap<Entity, Entity> m_RecentChirps;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_RandomCitizenChunks;
      public RandomSeed m_RandomSeed;
      public int m_UneducatedPopulation;
      public int m_EducatedPopulation;
      public uint m_SimulationFrame;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_Queue.IsEmpty())
          return;
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(0);
        ChirpCreationData chirpCreationData;
        // ISSUE: reference to a compiler-generated field
        while (this.m_Queue.TryDequeue(out chirpCreationData))
        {
          bool isChirperChirp;
          // ISSUE: reference to a compiler-generated method
          Entity chirpPrefab = this.GetChirpPrefab(chirpCreationData.m_TriggerPrefab, ref random, out isChirperChirp);
          // ISSUE: reference to a compiler-generated field
          if (!(chirpPrefab == Entity.Null) && (!isChirperChirp || !this.m_RecentChirps.ContainsKey(chirpPrefab)))
          {
            // ISSUE: reference to a compiler-generated method
            EntityArchetype archetype = this.GetArchetype(chirpPrefab);
            if (archetype.Valid)
            {
              // ISSUE: reference to a compiler-generated method
              Entity entity1 = isChirperChirp ? this.FindSender(chirpCreationData.m_Sender, chirpCreationData.m_Target, chirpPrefab, ref random) : chirpCreationData.m_Sender;
              if (!(entity1 == Entity.Null))
              {
                float num1 = random.NextFloat(0.2f, 1f);
                float num2 = random.NextFloat(1f / 1000f, 0.03f);
                int num3 = random.NextInt(5, 100);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                int x = this.m_EducatedPopulation + this.m_UneducatedPopulation;
                float num4 = 0.2f;
                RandomLikeCountData componentData1;
                // ISSUE: reference to a compiler-generated field
                if (this.m_RandomLikeCountData.TryGetComponent(chirpPrefab, out componentData1))
                {
                  num2 = random.NextFloat(componentData1.m_RandomAmountFactor.x, componentData1.m_RandomAmountFactor.y);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  x = (int) ((double) this.m_EducatedPopulation * (double) componentData1.m_EducatedPercentage + (double) this.m_UneducatedPopulation * (double) componentData1.m_UneducatedPercentage);
                  num3 = random.NextInt(componentData1.m_GoViralFactor.x, componentData1.m_GoViralFactor.y + 1);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  num1 = random.NextFloat(this.m_RandomLikeCountData[chirpPrefab].m_ActiveDays.x, this.m_RandomLikeCountData[chirpPrefab].m_ActiveDays.y);
                  num4 = componentData1.m_ContinuousFactor;
                }
                int num5 = (int) ((double) x * (double) num2);
                // ISSUE: reference to a compiler-generated field
                Entity entity2 = this.m_CommandBuffer.CreateEntity(archetype);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<Chirp>(entity2, new Chirp(entity1, this.m_SimulationFrame)
                {
                  m_TargetLikes = (uint) num5,
                  m_InactiveFrame = (uint) ((double) this.m_SimulationFrame + (double) num1 * 262144.0),
                  m_ViralFactor = num3,
                  m_ContinuousFactor = num4,
                  m_Likes = (uint) math.min(x, random.NextInt(5))
                });
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<PrefabRef>(entity2, new PrefabRef(chirpPrefab));
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<ChirpEntity> dynamicBuffer = this.m_CommandBuffer.AddBuffer<ChirpEntity>(entity2);
                if (entity1 != Entity.Null)
                  dynamicBuffer.Add(new ChirpEntity(entity1));
                LifePathEventData componentData2;
                HouseholdMember componentData3;
                DynamicBuffer<HouseholdCitizen> bufferData;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_LifepathEventData.TryGetComponent(chirpPrefab, out componentData2) && componentData2.m_EventType == LifePathEventType.CitizenCoupleMadeBaby && this.m_HouseholdMembers.TryGetComponent(chirpCreationData.m_Sender, out componentData3) && this.m_HouseholdCitizens.TryGetBuffer(componentData3.m_Household, out bufferData))
                {
                  for (int index = 0; index < bufferData.Length; ++index)
                  {
                    Entity citizen = bufferData[index].m_Citizen;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_Citizens.HasComponent(citizen) && this.m_Citizens[citizen].GetAge() == CitizenAge.Adult && bufferData[index].m_Citizen != entity1)
                    {
                      dynamicBuffer.Add(new ChirpEntity(bufferData[index].m_Citizen));
                      break;
                    }
                  }
                }
                if (chirpCreationData.m_Target != Entity.Null)
                  dynamicBuffer.Add(new ChirpEntity(chirpCreationData.m_Target));
                if (isChirperChirp)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_RecentChirps.Add(chirpPrefab, entity2);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_LifepathEntries.HasBuffer(entity1))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AppendToBuffer<LifePathEntry>(entity1, new LifePathEntry(entity2));
                  }
                }
              }
            }
          }
        }
      }

      private Entity GetChirpPrefab(
        Entity triggerPrefab,
        ref Random random,
        out bool isChirperChirp)
      {
        DynamicBuffer<TriggerChirpData> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TriggerChirpData.TryGetBuffer(triggerPrefab, out bufferData))
        {
          isChirperChirp = true;
          return bufferData.Length <= 0 ? Entity.Null : bufferData[random.NextInt(bufferData.Length)].m_Chirp;
        }
        isChirperChirp = false;
        return triggerPrefab;
      }

      private EntityArchetype GetArchetype(Entity prefab)
      {
        ChirpData componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ChirpData.TryGetComponent(prefab, out componentData1))
          return componentData1.m_Archetype;
        LifePathEventData componentData2;
        // ISSUE: reference to a compiler-generated field
        return this.m_LifepathEventData.TryGetComponent(prefab, out componentData2) ? componentData2.m_ChirpArchetype : new EntityArchetype();
      }

      private Entity FindSender(Entity sender, Entity target, Entity prefab, ref Random random)
      {
        ServiceChirpData componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceChirpDatas.TryGetComponent(prefab, out componentData))
          return componentData.m_Account;
        // ISSUE: reference to a compiler-generated field
        if (this.m_BrandChirpData.HasComponent(prefab))
          return sender;
        DynamicBuffer<Employee> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Employees.TryGetBuffer(sender, out bufferData1))
        {
          // ISSUE: reference to a compiler-generated method
          return this.SelectRandomSender(bufferData1, ref random);
        }
        DynamicBuffer<HouseholdCitizen> bufferData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        return this.m_HouseholdCitizens.TryGetBuffer(sender, out bufferData2) ? this.SelectRandomSender(bufferData2, ref random) : this.SelectRandomSender(ref random);
      }

      private Entity SelectRandomSender(DynamicBuffer<Employee> employees, ref Random random)
      {
        Entity entity = Entity.Null;
        int num = 0;
        for (int index = 0; index < employees.Length; ++index)
        {
          Entity worker = employees[index].m_Worker;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Citizens.HasComponent(worker) && !CitizenUtils.IsDead(worker, ref this.m_HealthProblems) && random.NextInt(++num) == 0)
            entity = worker;
        }
        return entity;
      }

      private Entity SelectRandomSender(DynamicBuffer<HouseholdCitizen> citizens, ref Random random)
      {
        Entity entity = Entity.Null;
        int num = 0;
        for (int index = 0; index < citizens.Length; ++index)
        {
          Entity citizen = citizens[index].m_Citizen;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Citizens.HasComponent(citizen) && !CitizenUtils.IsDead(citizen, ref this.m_HealthProblems) && random.NextInt(++num) == 0)
            entity = citizen;
        }
        return entity;
      }

      private Entity SelectRandomSender(ref Random random)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ArchetypeChunk> nativeArray1 = this.m_RandomCitizenChunks.AsArray();
        int length1 = nativeArray1.Length;
        if (length1 != 0)
        {
          for (int index = 0; index < 100; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray2 = nativeArray1[random.NextInt(length1)].GetNativeArray(this.m_EntityType);
            Entity entity = nativeArray2[random.NextInt(nativeArray2.Length)];
            Citizen componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!CitizenUtils.IsDead(entity, ref this.m_HealthProblems) && CitizenUtils.HasMovedIn(entity, ref this.m_HouseholdMembers, ref this.m_Households, ref this.m_HomelessHouseholds) && this.m_Citizens.TryGetComponent(entity, out componentData) && componentData.GetAge() == CitizenAge.Adult)
              return entity;
          }
          int num1 = random.NextInt(length1);
          for (int index1 = 0; index1 < length1; ++index1)
          {
            ref NativeArray<ArchetypeChunk> local1 = ref nativeArray1;
            int index2 = num1;
            int a1 = index2 + 1;
            ArchetypeChunk archetypeChunk = local1[index2];
            num1 = math.select(a1, 0, a1 == length1);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray3 = archetypeChunk.GetNativeArray(this.m_EntityType);
            int length2 = nativeArray3.Length;
            int num2 = random.NextInt(length2);
            for (int index3 = 0; index3 < length2; ++index3)
            {
              ref NativeArray<Entity> local2 = ref nativeArray3;
              int index4 = num2;
              int a2 = index4 + 1;
              Entity entity = local2[index4];
              num2 = math.select(a2, 0, a2 == length2);
              Citizen componentData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!CitizenUtils.IsDead(entity, ref this.m_HealthProblems) && CitizenUtils.HasMovedIn(entity, ref this.m_HouseholdMembers, ref this.m_Households, ref this.m_HomelessHouseholds) && this.m_Citizens.TryGetComponent(entity, out componentData) && componentData.GetAge() == CitizenAge.Adult)
                return entity;
            }
          }
        }
        return Entity.Null;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Chirp> __Game_Triggers_Chirp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ChirpData> __Game_Prefabs_ChirpData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<TriggerChirpData> __Game_Prefabs_TriggerChirpData_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<LifePathEventData> __Game_Prefabs_LifePathEventData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BrandChirpData> __Game_Prefabs_BrandChirpData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RandomLikeCountData> __Game_Prefabs_RandomLikeCountData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceChirpData> __Game_Prefabs_ServiceChirpData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HomelessHousehold> __Game_Citizens_HomelessHousehold_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Employee> __Game_Companies_Employee_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LifePathEntry> __Game_Triggers_LifePathEntry_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Triggers_Chirp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Chirp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ChirpData_RO_ComponentLookup = state.GetComponentLookup<ChirpData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TriggerChirpData_RO_BufferLookup = state.GetBufferLookup<TriggerChirpData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LifePathEventData_RO_ComponentLookup = state.GetComponentLookup<LifePathEventData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BrandChirpData_RO_ComponentLookup = state.GetComponentLookup<BrandChirpData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RandomLikeCountData_RO_ComponentLookup = state.GetComponentLookup<RandomLikeCountData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceChirpData_RO_ComponentLookup = state.GetComponentLookup<ServiceChirpData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentLookup = state.GetComponentLookup<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HomelessHousehold_RO_ComponentLookup = state.GetComponentLookup<HomelessHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferLookup = state.GetBufferLookup<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Triggers_LifePathEntry_RO_BufferLookup = state.GetBufferLookup<LifePathEntry>(true);
      }
    }
  }
}
