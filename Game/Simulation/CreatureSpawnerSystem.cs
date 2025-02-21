// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CreatureSpawnerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

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
  public class CreatureSpawnerSystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_SpawnerQuery;
    private ComponentTypeSet m_AnimalSpawnTypes;
    private CreatureSpawnerSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 512;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SpawnerQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Creatures.CreatureSpawner>(), ComponentType.ReadWrite<OwnedCreature>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_AnimalSpawnTypes = new ComponentTypeSet(ComponentType.ReadWrite<AnimalCurrentLane>(), ComponentType.ReadWrite<Owner>(), ComponentType.ReadWrite<TripSource>(), ComponentType.ReadWrite<Unspawned>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_SpawnerQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DomesticatedData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WildlifeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AnimalData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CreatureSpawnData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Domesticated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_OwnedCreature_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new CreatureSpawnerSystem.CreatureSpawnerJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_SpawnLocationType = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_OwnedCreatureType = this.__TypeHandle.__Game_Creatures_OwnedCreature_RW_BufferTypeHandle,
        m_CreatureData = this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup,
        m_GroupMemberData = this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentLookup,
        m_DomesticatedData = this.__TypeHandle.__Game_Creatures_Domesticated_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabCreatureSpawnData = this.__TypeHandle.__Game_Prefabs_CreatureSpawnData_RO_ComponentLookup,
        m_PrefabSpawnableObjectData = this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup,
        m_PrefabObjectData = this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup,
        m_PrefabAnimalData = this.__TypeHandle.__Game_Prefabs_AnimalData_RO_ComponentLookup,
        m_PrefabWildlifeData = this.__TypeHandle.__Game_Prefabs_WildlifeData_RO_ComponentLookup,
        m_PrefabDomesticatedData = this.__TypeHandle.__Game_Prefabs_DomesticatedData_RO_ComponentLookup,
        m_PrefabPlaceholderObjects = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_AnimalSpawnTypes = this.m_AnimalSpawnTypes,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<CreatureSpawnerSystem.CreatureSpawnerJob>(this.m_SpawnerQuery, this.Dependency);
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
    public CreatureSpawnerSystem()
    {
    }

    [BurstCompile]
    private struct CreatureSpawnerJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.SpawnLocation> m_SpawnLocationType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public BufferTypeHandle<OwnedCreature> m_OwnedCreatureType;
      [ReadOnly]
      public ComponentLookup<Creature> m_CreatureData;
      [ReadOnly]
      public ComponentLookup<GroupMember> m_GroupMemberData;
      [ReadOnly]
      public ComponentLookup<Game.Creatures.Domesticated> m_DomesticatedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<CreatureSpawnData> m_PrefabCreatureSpawnData;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> m_PrefabSpawnableObjectData;
      [ReadOnly]
      public ComponentLookup<ObjectData> m_PrefabObjectData;
      [ReadOnly]
      public ComponentLookup<AnimalData> m_PrefabAnimalData;
      [ReadOnly]
      public ComponentLookup<WildlifeData> m_PrefabWildlifeData;
      [ReadOnly]
      public ComponentLookup<DomesticatedData> m_PrefabDomesticatedData;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> m_PrefabPlaceholderObjects;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public ComponentTypeSet m_AnimalSpawnTypes;
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
        NativeArray<Transform> nativeArray2 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.SpawnLocation> nativeArray3 = chunk.GetNativeArray<Game.Objects.SpawnLocation>(ref this.m_SpawnLocationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<OwnedCreature> bufferAccessor = chunk.GetBufferAccessor<OwnedCreature>(ref this.m_OwnedCreatureType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          Entity spawner = nativeArray1[index1];
          Transform transform = nativeArray2[index1];
          PrefabRef prefabRef1 = nativeArray4[index1];
          DynamicBuffer<OwnedCreature> dynamicBuffer = bufferAccessor[index1];
          // ISSUE: reference to a compiler-generated field
          CreatureSpawnData creatureSpawnData = this.m_PrefabCreatureSpawnData[prefabRef1.m_Prefab];
          int num1 = 0;
          Entity group = Entity.Null;
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            Entity creature = dynamicBuffer[index2].m_Creature;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CreatureData.HasComponent(creature))
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_GroupMemberData.HasComponent(creature))
                ++num1;
              // ISSUE: reference to a compiler-generated field
              if (group == Entity.Null && this.m_DomesticatedData.HasComponent(creature))
              {
                // ISSUE: reference to a compiler-generated field
                PrefabRef prefabRef2 = this.m_PrefabRefData[creature];
                SpawnableObjectData componentData;
                // ISSUE: reference to a compiler-generated field
                group = !this.m_PrefabSpawnableObjectData.TryGetComponent(prefabRef2.m_Prefab, out componentData) ? prefabRef2.m_Prefab : componentData.m_RandomizationGroup;
              }
            }
            else
              dynamicBuffer.RemoveAtSwapBack(index2--);
          }
          if (num1 < random.NextInt(creatureSpawnData.m_MaxGroupCount + 1))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<PlaceholderObjectElement> placeholderObject = this.m_PrefabPlaceholderObjects[prefabRef1.m_Prefab];
            Game.Objects.SpawnLocation spawnLocation = new Game.Objects.SpawnLocation();
            if (nativeArray3.Length != 0)
              spawnLocation = nativeArray3[index1];
            // ISSUE: reference to a compiler-generated method
            Entity entity = this.SelectPrefab(placeholderObject, ref random, ref group);
            int num2 = 1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabWildlifeData.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              WildlifeData wildlifeData = this.m_PrefabWildlifeData[entity];
              num2 = random.NextInt(wildlifeData.m_GroupMemberCount.x, wildlifeData.m_GroupMemberCount.y + 1);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabDomesticatedData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                DomesticatedData domesticatedData = this.m_PrefabDomesticatedData[entity];
                num2 = random.NextInt(domesticatedData.m_GroupMemberCount.x, domesticatedData.m_GroupMemberCount.y + 1);
              }
            }
            for (int index3 = 0; index3 < num2; ++index3)
            {
              if (index3 != 0)
              {
                // ISSUE: reference to a compiler-generated method
                entity = this.SelectPrefab(placeholderObject, ref random, ref group);
              }
              if (!(entity == Entity.Null))
              {
                // ISSUE: reference to a compiler-generated method
                this.SpawnCreature(unfilteredChunkIndex, spawner, entity, transform, spawnLocation, new PseudoRandomSeed(ref random));
              }
            }
          }
        }
      }

      private Entity SelectPrefab(
        DynamicBuffer<PlaceholderObjectElement> placeholderObjects,
        ref Random random,
        ref Entity group)
      {
        int max = 0;
        Entity entity1 = Entity.Null;
        Entity entity2 = Entity.Null;
        for (int index = 0; index < placeholderObjects.Length; ++index)
        {
          PlaceholderObjectElement placeholderObject = placeholderObjects[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabSpawnableObjectData.HasComponent(placeholderObject.m_Object))
          {
            // ISSUE: reference to a compiler-generated field
            SpawnableObjectData spawnableObjectData = this.m_PrefabSpawnableObjectData[placeholderObject.m_Object];
            Entity entity3 = spawnableObjectData.m_RandomizationGroup != Entity.Null ? spawnableObjectData.m_RandomizationGroup : placeholderObject.m_Object;
            if (!(group != Entity.Null) || !(group != entity3))
            {
              max += spawnableObjectData.m_Probability;
              if (random.NextInt(max) < spawnableObjectData.m_Probability)
              {
                entity1 = placeholderObject.m_Object;
                entity2 = entity3;
              }
            }
          }
        }
        group = entity2;
        return entity1;
      }

      private void SpawnCreature(
        int jobIndex,
        Entity spawner,
        Entity prefab,
        Transform transform,
        Game.Objects.SpawnLocation spawnLocation,
        PseudoRandomSeed randomSeed)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectData objectData = this.m_PrefabObjectData[prefab];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabAnimalData.HasComponent(prefab))
          return;
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, objectData.m_Archetype);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(jobIndex, entity, in this.m_AnimalSpawnTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity, new PrefabRef(prefab));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Transform>(jobIndex, entity, transform);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity, randomSeed);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Owner>(jobIndex, entity, new Owner(spawner));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<TripSource>(jobIndex, entity, new TripSource(spawner));
        if (spawnLocation.m_ConnectedLane1 == Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Animal>(jobIndex, entity, new Animal(AnimalFlags.Roaming));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<AnimalNavigation>(jobIndex, entity, new AnimalNavigation(transform.m_Position));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<AnimalCurrentLane>(jobIndex, entity, new AnimalCurrentLane());
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<AnimalCurrentLane>(jobIndex, entity, new AnimalCurrentLane(spawnLocation.m_ConnectedLane1, spawnLocation.m_CurvePosition1, (CreatureLaneFlags) 0));
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
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public BufferTypeHandle<OwnedCreature> __Game_Creatures_OwnedCreature_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Creature> __Game_Creatures_Creature_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GroupMember> __Game_Creatures_GroupMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Creatures.Domesticated> __Game_Creatures_Domesticated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CreatureSpawnData> __Game_Prefabs_CreatureSpawnData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> __Game_Prefabs_SpawnableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectData> __Game_Prefabs_ObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AnimalData> __Game_Prefabs_AnimalData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WildlifeData> __Game_Prefabs_WildlifeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<DomesticatedData> __Game_Prefabs_DomesticatedData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_OwnedCreature_RW_BufferTypeHandle = state.GetBufferTypeHandle<OwnedCreature>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Creature_RO_ComponentLookup = state.GetComponentLookup<Creature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupMember_RO_ComponentLookup = state.GetComponentLookup<GroupMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Domesticated_RO_ComponentLookup = state.GetComponentLookup<Game.Creatures.Domesticated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CreatureSpawnData_RO_ComponentLookup = state.GetComponentLookup<CreatureSpawnData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup = state.GetComponentLookup<SpawnableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectData_RO_ComponentLookup = state.GetComponentLookup<ObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimalData_RO_ComponentLookup = state.GetComponentLookup<AnimalData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WildlifeData_RO_ComponentLookup = state.GetComponentLookup<WildlifeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DomesticatedData_RO_ComponentLookup = state.GetComponentLookup<DomesticatedData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup = state.GetBufferLookup<PlaceholderObjectElement>(true);
      }
    }
  }
}
