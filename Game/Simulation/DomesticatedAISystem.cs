// Decompiled with JetBrains decompiler
// Type: Game.Simulation.DomesticatedAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Creatures;
using Game.Net;
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
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class DomesticatedAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private EntityQuery m_CreatureQuery;
    private EntityQuery m_GroupCreatureQuery;
    private DomesticatedAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 9;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatureQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Creatures.Domesticated>(), ComponentType.ReadWrite<Animal>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<GroupMember>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Stumbling>());
      // ISSUE: reference to a compiler-generated field
      this.m_GroupCreatureQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Creatures.Domesticated>(), ComponentType.ReadWrite<Animal>(), ComponentType.ReadOnly<GroupMember>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Stumbling>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_CreatureQuery, this.m_GroupCreatureQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DomesticatedData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AnimalData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_AnimalNavigation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Domesticated_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Animal_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DomesticatedAISystem.DomesticatedTickJob jobData1 = new DomesticatedAISystem.DomesticatedTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_AnimalType = this.__TypeHandle.__Game_Creatures_Animal_RW_ComponentTypeHandle,
        m_DomesticatedType = this.__TypeHandle.__Game_Creatures_Domesticated_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle,
        m_NavigationType = this.__TypeHandle.__Game_Creatures_AnimalNavigation_RW_ComponentTypeHandle,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabAnimalData = this.__TypeHandle.__Game_Prefabs_AnimalData_RO_ComponentLookup,
        m_PrefabDomesticatedData = this.__TypeHandle.__Game_Prefabs_DomesticatedData_RO_ComponentLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Animal_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DomesticatedAISystem.DomesticatedGroupTickJob jobData2 = new DomesticatedAISystem.DomesticatedGroupTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_GroupMemberType = this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle,
        m_AnimalData = this.__TypeHandle.__Game_Creatures_Animal_RW_ComponentLookup,
        m_CommandBuffer = jobData1.m_CommandBuffer
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle handle = jobData1.ScheduleParallel<DomesticatedAISystem.DomesticatedTickJob>(this.m_CreatureQuery, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      EntityQuery groupCreatureQuery = this.m_GroupCreatureQuery;
      JobHandle dependsOn = handle;
      JobHandle producerJob = jobData2.ScheduleParallel<DomesticatedAISystem.DomesticatedGroupTickJob>(groupCreatureQuery, dependsOn);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(handle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(handle);
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
    public DomesticatedAISystem()
    {
    }

    [BurstCompile]
    private struct DomesticatedGroupTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<GroupMember> m_GroupMemberType;
      public ComponentTypeHandle<AnimalCurrentLane> m_CurrentLaneType;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Animal> m_AnimalData;
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
        NativeArray<GroupMember> nativeArray2 = chunk.GetNativeArray<GroupMember>(ref this.m_GroupMemberType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AnimalCurrentLane> nativeArray3 = chunk.GetNativeArray<AnimalCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          GroupMember groupMember = nativeArray2[index];
          AnimalCurrentLane currentLane;
          if (!CollectionUtils.TryGet<AnimalCurrentLane>(nativeArray3, index, out currentLane))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<AnimalCurrentLane>(unfilteredChunkIndex, entity, new AnimalCurrentLane());
          }
          // ISSUE: reference to a compiler-generated field
          Animal animal = this.m_AnimalData[entity];
          // ISSUE: reference to a compiler-generated field
          CreatureUtils.CheckUnspawned(unfilteredChunkIndex, entity, currentLane, animal, isUnspawned, this.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.TickGroupMemberWalking(unfilteredChunkIndex, entity, groupMember, ref animal, ref currentLane);
          // ISSUE: reference to a compiler-generated field
          this.m_AnimalData[entity] = animal;
          CollectionUtils.TrySet<AnimalCurrentLane>(nativeArray3, index, currentLane);
        }
      }

      private void TickGroupMemberWalking(
        int jobIndex,
        Entity entity,
        GroupMember groupMember,
        ref Animal animal,
        ref AnimalCurrentLane currentLane)
      {
        if (CreatureUtils.IsStuck(currentLane))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_AnimalData.HasComponent(groupMember.m_Leader))
            return;
          // ISSUE: reference to a compiler-generated field
          Animal animal1 = this.m_AnimalData[groupMember.m_Leader];
          animal.m_Flags = animal.m_Flags & ~(AnimalFlags.SwimmingTarget | AnimalFlags.FlyingTarget) | animal1.m_Flags & (AnimalFlags.SwimmingTarget | AnimalFlags.FlyingTarget);
          if (((animal.m_Flags ^ animal1.m_Flags) & AnimalFlags.Roaming) == (AnimalFlags) 0 || (currentLane.m_Flags & CreatureLaneFlags.EndReached) == (CreatureLaneFlags) 0)
            return;
          animal.m_Flags ^= AnimalFlags.Roaming;
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
    private struct DomesticatedTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Animal> m_AnimalType;
      public ComponentTypeHandle<Game.Creatures.Domesticated> m_DomesticatedType;
      public ComponentTypeHandle<AnimalCurrentLane> m_CurrentLaneType;
      public ComponentTypeHandle<AnimalNavigation> m_NavigationType;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<AnimalData> m_PrefabAnimalData;
      [ReadOnly]
      public ComponentLookup<DomesticatedData> m_PrefabDomesticatedData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
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
        NativeArray<Animal> nativeArray3 = chunk.GetNativeArray<Animal>(ref this.m_AnimalType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Creatures.Domesticated> nativeArray4 = chunk.GetNativeArray<Game.Creatures.Domesticated>(ref this.m_DomesticatedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AnimalCurrentLane> nativeArray5 = chunk.GetNativeArray<AnimalCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AnimalNavigation> nativeArray6 = chunk.GetNativeArray<AnimalNavigation>(ref this.m_NavigationType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          Animal animal = nativeArray3[index];
          Game.Creatures.Domesticated domesticated = nativeArray4[index];
          AnimalNavigation navigation = nativeArray6[index];
          AnimalCurrentLane currentLane;
          if (!CollectionUtils.TryGet<AnimalCurrentLane>(nativeArray5, index, out currentLane))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<AnimalCurrentLane>(unfilteredChunkIndex, entity, new AnimalCurrentLane());
          }
          // ISSUE: reference to a compiler-generated field
          CreatureUtils.CheckUnspawned(unfilteredChunkIndex, entity, currentLane, animal, isUnspawned, this.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.TickWalking(unfilteredChunkIndex, entity, prefabRef, ref random, ref animal, ref domesticated, ref currentLane, ref navigation);
          nativeArray3[index] = animal;
          nativeArray4[index] = domesticated;
          nativeArray6[index] = navigation;
          CollectionUtils.TrySet<AnimalCurrentLane>(nativeArray5, index, currentLane);
        }
      }

      private void TickWalking(
        int jobIndex,
        Entity entity,
        PrefabRef prefabRef,
        ref Unity.Mathematics.Random random,
        ref Animal animal,
        ref Game.Creatures.Domesticated domesticated,
        ref AnimalCurrentLane currentLane,
        ref AnimalNavigation navigation)
      {
        if (CreatureUtils.IsStuck(currentLane))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
        }
        else
        {
          if (currentLane.m_Lane != Entity.Null)
            animal.m_Flags &= ~AnimalFlags.Roaming;
          else
            animal.m_Flags |= AnimalFlags.Roaming;
          if (!CreatureUtils.PathEndReached(currentLane))
            return;
          // ISSUE: reference to a compiler-generated method
          this.PathEndReached(jobIndex, entity, prefabRef, ref random, ref animal, ref domesticated, ref currentLane, ref navigation);
        }
      }

      private bool PathEndReached(
        int jobIndex,
        Entity entity,
        PrefabRef prefabRef,
        ref Unity.Mathematics.Random random,
        ref Animal animal,
        ref Game.Creatures.Domesticated domesticated,
        ref AnimalCurrentLane currentLane,
        ref AnimalNavigation navigation)
      {
        // ISSUE: reference to a compiler-generated field
        AnimalData animalData = this.m_PrefabAnimalData[prefabRef.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        DomesticatedData domesticatedData = this.m_PrefabDomesticatedData[prefabRef.m_Prefab];
        if ((domesticated.m_Flags & DomesticatedFlags.Idling) != DomesticatedFlags.None)
        {
          if (--domesticated.m_StateTime > (ushort) 0)
            return false;
          domesticated.m_Flags &= ~DomesticatedFlags.Idling;
          domesticated.m_Flags |= DomesticatedFlags.Wandering;
        }
        else if ((domesticated.m_Flags & DomesticatedFlags.Wandering) != DomesticatedFlags.None)
        {
          if ((animal.m_Flags & AnimalFlags.FlyingTarget) == (AnimalFlags) 0)
          {
            float num1 = 3.75f;
            int min = Mathf.RoundToInt(domesticatedData.m_IdleTime.min * num1);
            int num2 = Mathf.RoundToInt(domesticatedData.m_IdleTime.max * num1);
            domesticated.m_StateTime = (ushort) math.clamp(random.NextInt(min, num2 + 1), 0, (int) ushort.MaxValue);
            if (domesticated.m_StateTime > (ushort) 0)
            {
              domesticated.m_Flags &= ~DomesticatedFlags.Wandering;
              domesticated.m_Flags |= DomesticatedFlags.Idling;
              return false;
            }
          }
        }
        else
          domesticated.m_Flags |= DomesticatedFlags.Wandering;
        Owner componentData1;
        Game.Objects.Transform componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.TryGetComponent(entity, out componentData1) && this.m_TransformData.TryGetComponent(componentData1.m_Owner, out componentData2))
        {
          if ((animal.m_Flags & AnimalFlags.Roaming) != (AnimalFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            Game.Objects.Transform transform = this.m_TransformData[entity];
            if ((animal.m_Flags & AnimalFlags.FlyingTarget) != (AnimalFlags) 0)
            {
              if (random.NextInt(5) == 0)
                animal.m_Flags &= ~AnimalFlags.FlyingTarget;
            }
            else if ((double) animalData.m_FlySpeed > 0.0 && ((double) animalData.m_MoveSpeed == 0.0 || random.NextInt(3) == 0))
              animal.m_Flags |= AnimalFlags.FlyingTarget;
            Entity owner = componentData1.m_Owner;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            while (this.m_OwnerData.HasComponent(owner) && !this.m_BuildingData.HasComponent(owner))
            {
              // ISSUE: reference to a compiler-generated field
              owner = this.m_OwnerData[owner].m_Owner;
            }
            float2 x = (float2) 16f;
            ObjectGeometryData componentData3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (owner != Entity.Null && this.m_PrefabObjectGeometryData.TryGetComponent(this.m_PrefabRefData[owner].m_Prefab, out componentData3))
              x = componentData3.m_Size.xz;
            float2 float2_1 = math.length(x) * new float2(0.2f, 0.9f);
            currentLane.m_Flags &= ~(CreatureLaneFlags.EndOfPath | CreatureLaneFlags.EndReached);
            navigation.m_TargetPosition = math.lerp(transform.m_Position, componentData2.m_Position, 0.5f);
            navigation.m_TargetPosition.xz += random.NextFloat2Direction() * random.NextFloat(float2_1.x, float2_1.y);
            if ((animal.m_Flags & AnimalFlags.SwimmingTarget) != (AnimalFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Bounds1 bounds1 = WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, navigation.m_TargetPosition) - MathUtils.Invert(animalData.m_SwimDepth);
              navigation.m_TargetPosition.y = random.NextFloat(bounds1.min, bounds1.max);
            }
            else if ((animal.m_Flags & AnimalFlags.FlyingTarget) != (AnimalFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Bounds1 bounds1 = WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, navigation.m_TargetPosition) + animalData.m_FlyHeight;
              navigation.m_TargetPosition.y = random.NextFloat(bounds1.min, bounds1.max);
            }
            else
            {
              if ((double) animalData.m_FlySpeed > 0.0)
              {
                float2 float2_2 = navigation.m_TargetPosition.xz - transform.m_Position.xz;
                navigation.m_TargetPosition.xz = transform.m_Position.xz + float2_2 * (animalData.m_MoveSpeed / animalData.m_FlySpeed);
              }
              // ISSUE: reference to a compiler-generated field
              navigation.m_TargetPosition.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, navigation.m_TargetPosition);
            }
            return false;
          }
          Lane componentData4;
          Owner componentData5;
          DynamicBuffer<Game.Net.SubLane> bufferData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((currentLane.m_Flags & CreatureLaneFlags.Area) != (CreatureLaneFlags) 0 && this.m_LaneData.TryGetComponent(currentLane.m_Lane, out componentData4) && this.m_OwnerData.TryGetComponent(currentLane.m_Lane, out componentData5) && this.m_SubLanes.TryGetBuffer(componentData5.m_Owner, out bufferData))
          {
            int max1 = 0;
            int max2 = 0;
            Entity a1 = Entity.Null;
            Entity subLane1 = Entity.Null;
            bool a2 = false;
            bool b = false;
            for (int index = 0; index < bufferData.Length; ++index)
            {
              Game.Net.SubLane subLane2 = bufferData[index];
              Lane componentData6;
              // ISSUE: reference to a compiler-generated field
              if (!(subLane2.m_SubLane == currentLane.m_Lane) && this.m_LaneData.TryGetComponent(subLane2.m_SubLane, out componentData6))
              {
                int num = 100;
                if (componentData6.m_StartNode.Equals(componentData4.m_EndNode) || componentData6.m_EndNode.Equals(componentData4.m_EndNode))
                {
                  max1 += num;
                  if (random.NextInt(max1) < num)
                  {
                    a1 = subLane2.m_SubLane;
                    a2 = componentData6.m_StartNode.Equals(componentData4.m_EndNode);
                  }
                }
                if (componentData6.m_StartNode.Equals(componentData4.m_StartNode) || componentData6.m_EndNode.Equals(componentData4.m_StartNode))
                {
                  max2 += num;
                  if (random.NextInt(max2) < num)
                  {
                    subLane1 = subLane2.m_SubLane;
                    b = componentData6.m_StartNode.Equals(componentData4.m_StartNode);
                  }
                }
              }
            }
            float num3;
            if ((currentLane.m_Flags & CreatureLaneFlags.Backward) != (CreatureLaneFlags) 0)
            {
              CommonUtils.Swap<Entity>(ref a1, ref subLane1);
              CommonUtils.Swap<bool>(ref a2, ref b);
              num3 = 0.0f;
            }
            else
              num3 = 1f;
            if (a1 == Entity.Null)
            {
              a1 = subLane1;
              a2 = b;
              num3 = math.select(0.0f, 1f, (double) num3 == 0.0);
            }
            currentLane.m_Flags &= ~(CreatureLaneFlags.EndOfPath | CreatureLaneFlags.EndReached);
            if (a1 != Entity.Null)
            {
              currentLane.m_NextLane = a1;
              currentLane.m_NextPosition.x = math.select(1f, 0.0f, a2);
              currentLane.m_NextPosition.y = random.NextFloat(0.0f, 1f);
              currentLane.m_NextFlags = currentLane.m_Flags;
              currentLane.m_CurvePosition.y = num3;
              if ((double) currentLane.m_NextPosition.y > (double) currentLane.m_NextPosition.x)
                currentLane.m_NextFlags &= ~CreatureLaneFlags.Backward;
              else if ((double) currentLane.m_NextPosition.y < (double) currentLane.m_NextPosition.x)
                currentLane.m_NextFlags |= CreatureLaneFlags.Backward;
            }
            else
            {
              currentLane.m_NextLane = Entity.Null;
              currentLane.m_CurvePosition.y = random.NextFloat(0.0f, 1f);
            }
            if ((double) currentLane.m_CurvePosition.y > (double) currentLane.m_CurvePosition.x)
              currentLane.m_Flags &= ~CreatureLaneFlags.Backward;
            else if ((double) currentLane.m_CurvePosition.y < (double) currentLane.m_CurvePosition.x)
              currentLane.m_Flags |= CreatureLaneFlags.Backward;
            return false;
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
        return true;
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
      public ComponentTypeHandle<Unspawned> __Game_Objects_Unspawned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Animal> __Game_Creatures_Animal_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Creatures.Domesticated> __Game_Creatures_Domesticated_RW_ComponentTypeHandle;
      public ComponentTypeHandle<AnimalCurrentLane> __Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<AnimalNavigation> __Game_Creatures_AnimalNavigation_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AnimalData> __Game_Prefabs_AnimalData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<DomesticatedData> __Game_Prefabs_DomesticatedData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<GroupMember> __Game_Creatures_GroupMember_RO_ComponentTypeHandle;
      public ComponentLookup<Animal> __Game_Creatures_Animal_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Animal_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Animal>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Domesticated_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Creatures.Domesticated>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AnimalCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalNavigation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AnimalNavigation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimalData_RO_ComponentLookup = state.GetComponentLookup<AnimalData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DomesticatedData_RO_ComponentLookup = state.GetComponentLookup<DomesticatedData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupMember_RO_ComponentTypeHandle = state.GetComponentTypeHandle<GroupMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Animal_RW_ComponentLookup = state.GetComponentLookup<Animal>();
      }
    }
  }
}
