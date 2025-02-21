// Decompiled with JetBrains decompiler
// Type: Game.Simulation.HouseholdPetSpawnSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Citizens;
using Game.Common;
using Game.Creatures;
using Game.Objects;
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
  public class HouseholdPetSpawnSystem : GameSystemBase
  {
    private EntityQuery m_HouseholdPetQuery;
    private EntityQuery m_AnimalPrefabQuery;
    private EntityArchetype m_ResetTripArchetype;
    private EndFrameBarrier m_EndFrameBarrier;
    private HouseholdPetSpawnSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdPetQuery = this.GetEntityQuery(ComponentType.ReadOnly<HouseholdPet>(), ComponentType.ReadOnly<Target>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_AnimalPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<AnimalData>(), ComponentType.ReadOnly<PetData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ResetTripArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<ResetTrip>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HouseholdPetQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_AnimalPrefabQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HouseholdPetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PetData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new HouseholdPetSpawnSystem.HouseholdPetSpawnJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CurrentTransportType = this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentTypeHandle,
        m_CurrentBuildingType = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PetDataType = this.__TypeHandle.__Game_Prefabs_PetData_RO_ComponentTypeHandle,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_HouseholdPetData = this.__TypeHandle.__Game_Prefabs_HouseholdPetData_RO_ComponentLookup,
        m_ObjectData = this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup,
        m_AnimalPrefabChunks = archetypeChunkListAsync,
        m_RandomSeed = RandomSeed.Next(),
        m_ResetTripArchetype = this.m_ResetTripArchetype,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<HouseholdPetSpawnSystem.HouseholdPetSpawnJob>(this.m_HouseholdPetQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      archetypeChunkListAsync.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = jobHandle;
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
    public HouseholdPetSpawnSystem()
    {
    }

    [BurstCompile]
    private struct HouseholdPetSpawnJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentTransport> m_CurrentTransportType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> m_CurrentBuildingType;
      [ReadOnly]
      public ComponentTypeHandle<Target> m_TargetType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<PetData> m_PetDataType;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<HouseholdPetData> m_HouseholdPetData;
      [ReadOnly]
      public ComponentLookup<ObjectData> m_ObjectData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_AnimalPrefabChunks;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityArchetype m_ResetTripArchetype;
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
        NativeArray<CurrentTransport> nativeArray2 = chunk.GetNativeArray<CurrentTransport>(ref this.m_CurrentTransportType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentBuilding> nativeArray3 = chunk.GetNativeArray<CurrentBuilding>(ref this.m_CurrentBuildingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Target> nativeArray4 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        if (nativeArray3.Length == 0)
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Target>(unfilteredChunkIndex, nativeArray1[index]);
          }
        }
        else if (nativeArray2.Length == 0)
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            CurrentBuilding currentBuilding = nativeArray3[index];
            Target target = nativeArray4[index];
            // ISSUE: reference to a compiler-generated field
            HouseholdPetData householdPetData = this.m_HouseholdPetData[nativeArray5[index].m_Prefab];
            // ISSUE: reference to a compiler-generated field
            Random random = this.m_RandomSeed.GetRandom(entity.Index);
            PseudoRandomSeed randomSeed;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            Entity prefab = ObjectEmergeSystem.SelectAnimalPrefab(ref random, householdPetData.m_Type, this.m_AnimalPrefabChunks, this.m_EntityType, this.m_PetDataType, out randomSeed);
            // ISSUE: reference to a compiler-generated field
            if (prefab != Entity.Null && this.m_TransformData.HasComponent(currentBuilding.m_CurrentBuilding))
            {
              // ISSUE: reference to a compiler-generated method
              Entity transport = this.SpawnPet(unfilteredChunkIndex, entity, currentBuilding.m_CurrentBuilding, target.m_Target, prefab, randomSeed);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<CurrentTransport>(unfilteredChunkIndex, entity, new CurrentTransport(transport));
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<CurrentBuilding>(unfilteredChunkIndex, entity);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Target>(unfilteredChunkIndex, entity);
          }
        }
        else
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity e = nativeArray1[index];
            CurrentBuilding currentBuilding = nativeArray3[index];
            CurrentTransport currentTransport = nativeArray2[index];
            Target target = nativeArray4[index];
            // ISSUE: reference to a compiler-generated field
            if (!this.m_DeletedData.HasComponent(currentTransport.m_CurrentTransport))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_ResetTripArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<ResetTrip>(unfilteredChunkIndex, entity, new ResetTrip()
              {
                m_Creature = currentTransport.m_CurrentTransport,
                m_Source = currentBuilding.m_CurrentBuilding,
                m_Target = target.m_Target,
                m_Delay = 512U
              });
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<CurrentBuilding>(unfilteredChunkIndex, e);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Target>(unfilteredChunkIndex, e);
            }
          }
        }
      }

      private Entity SpawnPet(
        int jobIndex,
        Entity householdPet,
        Entity source,
        Entity target,
        Entity prefab,
        PseudoRandomSeed randomSeed)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectData objectData = this.m_ObjectData[prefab];
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, objectData.m_Archetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity, new PrefabRef(prefab));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Transform>(jobIndex, entity, this.m_TransformData[source]);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Target>(jobIndex, entity, new Target(target));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Game.Creatures.Pet>(jobIndex, entity, new Game.Creatures.Pet(householdPet));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity, randomSeed);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<TripSource>(jobIndex, entity, new TripSource(source, 512U));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Unspawned>(jobIndex, entity, new Unspawned());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<AnimalCurrentLane>(jobIndex, entity, new AnimalCurrentLane());
        return entity;
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
      public ComponentTypeHandle<CurrentTransport> __Game_Citizens_CurrentTransport_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Target> __Game_Common_Target_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PetData> __Game_Prefabs_PetData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdPetData> __Game_Prefabs_HouseholdPetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectData> __Game_Prefabs_ObjectData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentTransport_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Target>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PetData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HouseholdPetData_RO_ComponentLookup = state.GetComponentLookup<HouseholdPetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectData_RO_ComponentLookup = state.GetComponentLookup<ObjectData>(true);
      }
    }
  }
}
