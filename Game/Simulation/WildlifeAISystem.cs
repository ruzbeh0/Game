// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WildlifeAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
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
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class WildlifeAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private EntityQuery m_CreatureQuery;
    private EntityQuery m_GroupCreatureQuery;
    private WildlifeAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 13;

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
      this.m_CreatureQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Creatures.Wildlife>(), ComponentType.ReadWrite<Animal>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<GroupMember>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Stumbling>());
      // ISSUE: reference to a compiler-generated field
      this.m_GroupCreatureQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Creatures.Wildlife>(), ComponentType.ReadWrite<Animal>(), ComponentType.ReadOnly<GroupMember>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Stumbling>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_CreatureQuery, this.m_GroupCreatureQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WildlifeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AnimalData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Creatures_Wildlife_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WildlifeAISystem.WildlifeTickJob jobData1 = new WildlifeAISystem.WildlifeTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_AnimalType = this.__TypeHandle.__Game_Creatures_Animal_RW_ComponentTypeHandle,
        m_WildlifeType = this.__TypeHandle.__Game_Creatures_Wildlife_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle,
        m_NavigationType = this.__TypeHandle.__Game_Creatures_AnimalNavigation_RW_ComponentTypeHandle,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PrefabAnimalData = this.__TypeHandle.__Game_Prefabs_AnimalData_RO_ComponentLookup,
        m_PrefabWildlifeData = this.__TypeHandle.__Game_Prefabs_WildlifeData_RO_ComponentLookup,
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
      WildlifeAISystem.WildlifeGroupTickJob jobData2 = new WildlifeAISystem.WildlifeGroupTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_GroupMemberType = this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle,
        m_AnimalData = this.__TypeHandle.__Game_Creatures_Animal_RW_ComponentLookup,
        m_CommandBuffer = jobData1.m_CommandBuffer
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle handle = jobData1.ScheduleParallel<WildlifeAISystem.WildlifeTickJob>(this.m_CreatureQuery, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      EntityQuery groupCreatureQuery = this.m_GroupCreatureQuery;
      JobHandle dependsOn = handle;
      JobHandle producerJob = jobData2.ScheduleParallel<WildlifeAISystem.WildlifeGroupTickJob>(groupCreatureQuery, dependsOn);
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
    public WildlifeAISystem()
    {
    }

    [BurstCompile]
    private struct WildlifeGroupTickJob : IJobChunk
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
          animal.m_Flags = animal.m_Flags & ~AnimalFlags.Roaming | animal1.m_Flags & AnimalFlags.Roaming;
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
    private struct WildlifeTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Animal> m_AnimalType;
      public ComponentTypeHandle<Game.Creatures.Wildlife> m_WildlifeType;
      public ComponentTypeHandle<AnimalCurrentLane> m_CurrentLaneType;
      public ComponentTypeHandle<AnimalNavigation> m_NavigationType;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<AnimalData> m_PrefabAnimalData;
      [ReadOnly]
      public ComponentLookup<WildlifeData> m_PrefabWildlifeData;
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
        NativeArray<Game.Creatures.Wildlife> nativeArray4 = chunk.GetNativeArray<Game.Creatures.Wildlife>(ref this.m_WildlifeType);
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
          Game.Creatures.Wildlife wildlife = nativeArray4[index];
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
          this.TickWalking(unfilteredChunkIndex, entity, prefabRef, ref random, ref animal, ref wildlife, ref currentLane, ref navigation);
          nativeArray3[index] = animal;
          nativeArray4[index] = wildlife;
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
        ref Game.Creatures.Wildlife wildlife,
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
          this.PathEndReached(jobIndex, entity, prefabRef, ref random, ref animal, ref wildlife, ref currentLane, ref navigation);
        }
      }

      private bool PathEndReached(
        int jobIndex,
        Entity entity,
        PrefabRef prefabRef,
        ref Unity.Mathematics.Random random,
        ref Animal animal,
        ref Game.Creatures.Wildlife wildlife,
        ref AnimalCurrentLane currentLane,
        ref AnimalNavigation navigation)
      {
        // ISSUE: reference to a compiler-generated field
        AnimalData animalData = this.m_PrefabAnimalData[prefabRef.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        WildlifeData wildlifeData = this.m_PrefabWildlifeData[prefabRef.m_Prefab];
        if ((wildlife.m_Flags & WildlifeFlags.Idling) != WildlifeFlags.None)
        {
          if (--wildlife.m_StateTime > (ushort) 0)
            return false;
          wildlife.m_Flags &= ~WildlifeFlags.Idling;
          wildlife.m_Flags |= WildlifeFlags.Wandering;
        }
        else if ((wildlife.m_Flags & WildlifeFlags.Wandering) != WildlifeFlags.None)
        {
          if ((animal.m_Flags & AnimalFlags.FlyingTarget) == (AnimalFlags) 0)
          {
            float num1 = 3.75f;
            int min = Mathf.RoundToInt(wildlifeData.m_IdleTime.min * num1);
            int num2 = Mathf.RoundToInt(wildlifeData.m_IdleTime.max * num1);
            wildlife.m_StateTime = (ushort) math.clamp(random.NextInt(min, num2 + 1), 0, (int) ushort.MaxValue);
            if (wildlife.m_StateTime > (ushort) 0)
            {
              wildlife.m_Flags &= ~WildlifeFlags.Wandering;
              wildlife.m_Flags |= WildlifeFlags.Idling;
              return false;
            }
          }
        }
        else
          wildlife.m_Flags |= WildlifeFlags.Wandering;
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          Owner owner = this.m_OwnerData[entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransformData.HasComponent(owner.m_Owner))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Objects.Transform transform1 = this.m_TransformData[entity];
            // ISSUE: reference to a compiler-generated field
            Game.Objects.Transform transform2 = this.m_TransformData[owner.m_Owner];
            if ((animal.m_Flags & AnimalFlags.FlyingTarget) != (AnimalFlags) 0)
            {
              if (random.NextInt(5) == 0)
                animal.m_Flags &= ~AnimalFlags.FlyingTarget;
            }
            else if ((double) animalData.m_FlySpeed > 0.0 && ((double) animalData.m_MoveSpeed == 0.0 || random.NextInt(3) == 0))
              animal.m_Flags |= AnimalFlags.FlyingTarget;
            currentLane.m_Flags &= ~(CreatureLaneFlags.EndOfPath | CreatureLaneFlags.EndReached);
            navigation.m_TargetPosition = math.lerp(transform1.m_Position, transform2.m_Position, 0.25f);
            navigation.m_TargetPosition.xz += random.NextFloat2Direction() * random.NextFloat(wildlifeData.m_TripLength.min, wildlifeData.m_TripLength.max);
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
                float2 float2 = navigation.m_TargetPosition.xz - transform1.m_Position.xz;
                navigation.m_TargetPosition.xz = transform1.m_Position.xz + float2 * (animalData.m_MoveSpeed / animalData.m_FlySpeed);
              }
              // ISSUE: reference to a compiler-generated field
              navigation.m_TargetPosition.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, navigation.m_TargetPosition);
            }
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
      public ComponentTypeHandle<Game.Creatures.Wildlife> __Game_Creatures_Wildlife_RW_ComponentTypeHandle;
      public ComponentTypeHandle<AnimalCurrentLane> __Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<AnimalNavigation> __Game_Creatures_AnimalNavigation_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AnimalData> __Game_Prefabs_AnimalData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WildlifeData> __Game_Prefabs_WildlifeData_RO_ComponentLookup;
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
        this.__Game_Creatures_Wildlife_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Creatures.Wildlife>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AnimalCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalNavigation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AnimalNavigation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimalData_RO_ComponentLookup = state.GetComponentLookup<AnimalData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WildlifeData_RO_ComponentLookup = state.GetComponentLookup<WildlifeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupMember_RO_ComponentTypeHandle = state.GetComponentTypeHandle<GroupMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Animal_RW_ComponentLookup = state.GetComponentLookup<Animal>();
      }
    }
  }
}
