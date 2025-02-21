// Decompiled with JetBrains decompiler
// Type: Game.Creatures.GroupSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Citizens;
using Game.Common;
using Game.Objects;
using Game.Pathfind;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Creatures
{
  [CompilerGenerated]
  public class GroupSystem : GameSystemBase
  {
    private ModificationBarrier5 m_ModificationBarrier;
    private EntityQuery m_CreatureQuery;
    private GroupSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatureQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Creature>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<ResetTrip>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatureQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(100, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<GroupSystem.GroupData> nativeQueue = new NativeQueue<GroupSystem.GroupData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_ResetTrip_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GroupSystem.ResetTripSetJob jobData1 = new GroupSystem.ResetTripSetJob()
      {
        m_ResetTripType = this.__TypeHandle.__Game_Creatures_ResetTrip_RO_ComponentTypeHandle,
        m_ResetTripSet = nativeParallelHashSet
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_OwnedCreature_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_GroupCreature_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Domesticated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Wildlife_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Pet_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdPet_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TripSource_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_GroupCreature_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_ResetTrip_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Domesticated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Wildlife_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Pet_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TripSource_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GroupSystem.FillGroupQueueJob jobData2 = new GroupSystem.FillGroupQueueJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle,
        m_TripSourceType = this.__TypeHandle.__Game_Objects_TripSource_RO_ComponentTypeHandle,
        m_ResidentType = this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentTypeHandle,
        m_PetType = this.__TypeHandle.__Game_Creatures_Pet_RO_ComponentTypeHandle,
        m_WildlifeType = this.__TypeHandle.__Game_Creatures_Wildlife_RO_ComponentTypeHandle,
        m_DomesticatedType = this.__TypeHandle.__Game_Creatures_Domesticated_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_ResetTripType = this.__TypeHandle.__Game_Creatures_ResetTrip_RO_ComponentTypeHandle,
        m_GroupMemberType = this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentTypeHandle,
        m_GroupCreatureType = this.__TypeHandle.__Game_Creatures_GroupCreature_RO_BufferTypeHandle,
        m_TargetData = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
        m_TripSourceData = this.__TypeHandle.__Game_Objects_TripSource_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_HouseholdMemberData = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_CurrentTransportData = this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup,
        m_HouseholdPetData = this.__TypeHandle.__Game_Citizens_HouseholdPet_RO_ComponentLookup,
        m_GroupMemberData = this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentLookup,
        m_ResidentData = this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup,
        m_PetData = this.__TypeHandle.__Game_Creatures_Pet_RO_ComponentLookup,
        m_WildlifeData = this.__TypeHandle.__Game_Creatures_Wildlife_RO_ComponentLookup,
        m_DomesticatedData = this.__TypeHandle.__Game_Creatures_Domesticated_RO_ComponentLookup,
        m_GroupCreatures = this.__TypeHandle.__Game_Creatures_GroupCreature_RO_BufferLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_HouseholdAnimals = this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RO_BufferLookup,
        m_OwnedCreatures = this.__TypeHandle.__Game_Creatures_OwnedCreature_RO_BufferLookup,
        m_ResetTripSet = nativeParallelHashSet,
        m_GroupQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_GroupCreature_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_GroupMember_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CarKeeper_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Human_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      GroupSystem.GroupCreaturesJob jobData3 = new GroupSystem.GroupCreaturesJob()
      {
        m_HumanData = this.__TypeHandle.__Game_Creatures_Human_RO_ComponentLookup,
        m_ResidentData = this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_CarKeeperData = this.__TypeHandle.__Game_Citizens_CarKeeper_RO_ComponentLookup,
        m_GroupMemberData = this.__TypeHandle.__Game_Creatures_GroupMember_RW_ComponentLookup,
        m_PathOwnerData = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentLookup,
        m_GroupCreatures = this.__TypeHandle.__Game_Creatures_GroupCreature_RW_BufferLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_ResetTripSet = nativeParallelHashSet,
        m_GroupQueue = nativeQueue,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn1 = jobData1.Schedule<GroupSystem.ResetTripSetJob>(this.m_CreatureQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn2 = jobData2.ScheduleParallel<GroupSystem.FillGroupQueueJob>(this.m_CreatureQuery, dependsOn1);
      JobHandle jobHandle = jobData3.Schedule<GroupSystem.GroupCreaturesJob>(dependsOn2);
      nativeParallelHashSet.Dispose(jobHandle);
      nativeQueue.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
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
    public GroupSystem()
    {
    }

    private struct GroupData
    {
      public Entity m_Creature1;
      public Entity m_Creature2;

      public GroupData(Entity creature1, Entity creature2)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Creature1 = creature1;
        // ISSUE: reference to a compiler-generated field
        this.m_Creature2 = creature2;
      }
    }

    [BurstCompile]
    private struct ResetTripSetJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<ResetTrip> m_ResetTripType;
      public NativeParallelHashSet<Entity> m_ResetTripSet;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ResetTrip> nativeArray = chunk.GetNativeArray<ResetTrip>(ref this.m_ResetTripType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ResetTripSet.Add(nativeArray[index].m_Creature);
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
    private struct FillGroupQueueJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Target> m_TargetType;
      [ReadOnly]
      public ComponentTypeHandle<TripSource> m_TripSourceType;
      [ReadOnly]
      public ComponentTypeHandle<Resident> m_ResidentType;
      [ReadOnly]
      public ComponentTypeHandle<Pet> m_PetType;
      [ReadOnly]
      public ComponentTypeHandle<Wildlife> m_WildlifeType;
      [ReadOnly]
      public ComponentTypeHandle<Domesticated> m_DomesticatedType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<ResetTrip> m_ResetTripType;
      [ReadOnly]
      public ComponentTypeHandle<GroupMember> m_GroupMemberType;
      [ReadOnly]
      public BufferTypeHandle<GroupCreature> m_GroupCreatureType;
      [ReadOnly]
      public ComponentLookup<Target> m_TargetData;
      [ReadOnly]
      public ComponentLookup<TripSource> m_TripSourceData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMemberData;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> m_CurrentTransportData;
      [ReadOnly]
      public ComponentLookup<HouseholdPet> m_HouseholdPetData;
      [ReadOnly]
      public ComponentLookup<GroupMember> m_GroupMemberData;
      [ReadOnly]
      public ComponentLookup<Resident> m_ResidentData;
      [ReadOnly]
      public ComponentLookup<Pet> m_PetData;
      [ReadOnly]
      public ComponentLookup<Wildlife> m_WildlifeData;
      [ReadOnly]
      public ComponentLookup<Domesticated> m_DomesticatedData;
      [ReadOnly]
      public BufferLookup<GroupCreature> m_GroupCreatures;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public BufferLookup<HouseholdAnimal> m_HouseholdAnimals;
      [ReadOnly]
      public BufferLookup<OwnedCreature> m_OwnedCreatures;
      [ReadOnly]
      public NativeParallelHashSet<Entity> m_ResetTripSet;
      public NativeQueue<GroupSystem.GroupData>.ParallelWriter m_GroupQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ResetTrip> nativeArray1 = chunk.GetNativeArray<ResetTrip>(ref this.m_ResetTripType);
        if (nativeArray1.Length != 0)
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            ResetTrip resetTrip = nativeArray1[index];
            // ISSUE: reference to a compiler-generated field
            if (!this.m_DeletedData.HasComponent(resetTrip.m_Creature))
            {
              GroupMember componentData1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_GroupMemberData.TryGetComponent(resetTrip.m_Creature, out componentData1))
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckDeletedGroupMember(componentData1);
              }
              DynamicBuffer<GroupCreature> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_GroupCreatures.TryGetBuffer(resetTrip.m_Creature, out bufferData))
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckDeletedGroupLeader(bufferData);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!this.m_UpdatedData.HasComponent(resetTrip.m_Creature) || !this.m_TripSourceData.HasComponent(resetTrip.m_Creature))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_GroupQueue.Enqueue(new GroupSystem.GroupData(resetTrip.m_Creature, Entity.Null));
                TripSource componentData2;
                Target componentData3;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_TripSourceData.TryGetComponent(resetTrip.m_Creature, out componentData2) && this.m_TargetData.TryGetComponent(resetTrip.m_Creature, out componentData3))
                {
                  Resident componentData4;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ResidentData.TryGetComponent(resetTrip.m_Creature, out componentData4))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.CheckUpdatedResident(resetTrip.m_Creature, componentData4, componentData2, componentData3);
                  }
                  Pet componentData5;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PetData.TryGetComponent(resetTrip.m_Creature, out componentData5))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.CheckUpdatedPet(resetTrip.m_Creature, componentData5, componentData2, componentData3);
                  }
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_WildlifeData.HasComponent(resetTrip.m_Creature))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.CheckUpdatedWildlife(resetTrip.m_Creature, componentData2);
                  }
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_DomesticatedData.HasComponent(resetTrip.m_Creature))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.CheckUpdatedDomesticated(resetTrip.m_Creature, componentData2);
                  }
                }
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Deleted>(ref this.m_DeletedType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<GroupMember> nativeArray2 = chunk.GetNativeArray<GroupMember>(ref this.m_GroupMemberType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<GroupCreature> bufferAccessor = chunk.GetBufferAccessor<GroupCreature>(ref this.m_GroupCreatureType);
            for (int index = 0; index < nativeArray2.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckDeletedGroupMember(nativeArray2[index]);
            }
            for (int index = 0; index < bufferAccessor.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckDeletedGroupLeader(bufferAccessor[index]);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<TripSource> nativeArray3 = chunk.GetNativeArray<TripSource>(ref this.m_TripSourceType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Target> nativeArray4 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
            if (nativeArray3.Length == 0 || nativeArray4.Length == 0)
              return;
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray5 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Resident> nativeArray6 = chunk.GetNativeArray<Resident>(ref this.m_ResidentType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Pet> nativeArray7 = chunk.GetNativeArray<Pet>(ref this.m_PetType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Wildlife> nativeArray8 = chunk.GetNativeArray<Wildlife>(ref this.m_WildlifeType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Domesticated> nativeArray9 = chunk.GetNativeArray<Domesticated>(ref this.m_DomesticatedType);
            for (int index = 0; index < nativeArray6.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckUpdatedResident(nativeArray5[index], nativeArray6[index], nativeArray3[index], nativeArray4[index]);
            }
            for (int index = 0; index < nativeArray7.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckUpdatedPet(nativeArray5[index], nativeArray7[index], nativeArray3[index], nativeArray4[index]);
            }
            for (int index = 0; index < nativeArray8.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckUpdatedWildlife(nativeArray5[index], nativeArray3[index]);
            }
            for (int index = 0; index < nativeArray9.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckUpdatedDomesticated(nativeArray5[index], nativeArray3[index]);
            }
          }
        }
      }

      private void CheckDeletedGroupMember(GroupMember groupMember)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_DeletedData.HasComponent(groupMember.m_Leader) || this.m_ResetTripSet.Contains(groupMember.m_Leader))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_GroupQueue.Enqueue(new GroupSystem.GroupData(groupMember.m_Leader, Entity.Null));
      }

      private void CheckDeletedGroupLeader(DynamicBuffer<GroupCreature> groupCreatures)
      {
        Entity creature = Entity.Null;
        bool flag = false;
        for (int index = 0; index < groupCreatures.Length; ++index)
        {
          GroupCreature groupCreature = groupCreatures[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_DeletedData.HasComponent(groupCreature.m_Creature) && !this.m_ResetTripSet.Contains(groupCreature.m_Creature))
          {
            if (creature != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_GroupQueue.Enqueue(new GroupSystem.GroupData(creature, groupCreature.m_Creature));
              flag = true;
            }
            creature = groupCreature.m_Creature;
          }
        }
        if (!(creature != Entity.Null) || flag)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_GroupQueue.Enqueue(new GroupSystem.GroupData(creature, Entity.Null));
      }

      private void CheckUpdatedResident(
        Entity creature,
        Resident resident,
        TripSource tripSource,
        Target target)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_HouseholdMemberData.HasComponent(resident.m_Citizen))
          return;
        // ISSUE: reference to a compiler-generated field
        HouseholdMember householdMember = this.m_HouseholdMemberData[resident.m_Citizen];
        // ISSUE: reference to a compiler-generated method
        this.FindHumanPartners(creature, householdMember.m_Household, tripSource.m_Source, target.m_Target);
        // ISSUE: reference to a compiler-generated method
        this.FindAnimalPartners(creature, householdMember.m_Household, tripSource.m_Source, target.m_Target);
      }

      private void CheckUpdatedPet(Entity creature, Pet pet, TripSource tripSource, Target target)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_HouseholdPetData.HasComponent(pet.m_HouseholdPet))
          return;
        // ISSUE: reference to a compiler-generated field
        HouseholdPet householdPet = this.m_HouseholdPetData[pet.m_HouseholdPet];
        // ISSUE: reference to a compiler-generated method
        this.FindHumanPartners(creature, householdPet.m_Household, tripSource.m_Source, target.m_Target);
        // ISSUE: reference to a compiler-generated method
        this.FindAnimalPartners(creature, householdPet.m_Household, tripSource.m_Source, target.m_Target);
      }

      private void CheckUpdatedWildlife(Entity creature, TripSource tripSource)
      {
        // ISSUE: reference to a compiler-generated method
        this.FindCreaturePartners(creature, tripSource.m_Source);
      }

      private void CheckUpdatedDomesticated(Entity creature, TripSource tripSource)
      {
        // ISSUE: reference to a compiler-generated method
        this.FindCreaturePartners(creature, tripSource.m_Source);
      }

      private void FindCreaturePartners(Entity creature, Entity source)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_OwnedCreatures.HasBuffer(source))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<OwnedCreature> ownedCreature1 = this.m_OwnedCreatures[source];
        for (int index = 0; index < ownedCreature1.Length; ++index)
        {
          OwnedCreature ownedCreature2 = ownedCreature1[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!(ownedCreature2.m_Creature == creature) && !this.m_DeletedData.HasComponent(ownedCreature2.m_Creature) && this.m_TripSourceData.HasComponent(ownedCreature2.m_Creature))
          {
            // ISSUE: reference to a compiler-generated field
            TripSource tripSource = this.m_TripSourceData[ownedCreature2.m_Creature];
            if (source == tripSource.m_Source)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_GroupQueue.Enqueue(new GroupSystem.GroupData(creature, ownedCreature2.m_Creature));
            }
          }
        }
      }

      private void FindAnimalPartners(
        Entity creature,
        Entity household,
        Entity source,
        Entity target)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_HouseholdAnimals.HasBuffer(household))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<HouseholdAnimal> householdAnimal1 = this.m_HouseholdAnimals[household];
        for (int index = 0; index < householdAnimal1.Length; ++index)
        {
          HouseholdAnimal householdAnimal2 = householdAnimal1[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentTransportData.HasComponent(householdAnimal2.m_HouseholdPet))
          {
            // ISSUE: reference to a compiler-generated field
            CurrentTransport currentTransport = this.m_CurrentTransportData[householdAnimal2.m_HouseholdPet];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!(currentTransport.m_CurrentTransport == creature) && !this.m_DeletedData.HasComponent(currentTransport.m_CurrentTransport) && this.m_TripSourceData.HasComponent(currentTransport.m_CurrentTransport) && this.m_TargetData.HasComponent(currentTransport.m_CurrentTransport))
            {
              // ISSUE: reference to a compiler-generated field
              TripSource tripSource = this.m_TripSourceData[currentTransport.m_CurrentTransport];
              // ISSUE: reference to a compiler-generated field
              Target target1 = this.m_TargetData[currentTransport.m_CurrentTransport];
              if (source == tripSource.m_Source && target == target1.m_Target)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_GroupQueue.Enqueue(new GroupSystem.GroupData(creature, currentTransport.m_CurrentTransport));
              }
            }
          }
        }
      }

      private void FindHumanPartners(
        Entity creature,
        Entity household,
        Entity source,
        Entity target)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_HouseholdCitizens.HasBuffer(household))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<HouseholdCitizen> householdCitizen1 = this.m_HouseholdCitizens[household];
        for (int index = 0; index < householdCitizen1.Length; ++index)
        {
          HouseholdCitizen householdCitizen2 = householdCitizen1[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentTransportData.HasComponent(householdCitizen2.m_Citizen))
          {
            // ISSUE: reference to a compiler-generated field
            CurrentTransport currentTransport = this.m_CurrentTransportData[householdCitizen2.m_Citizen];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!(currentTransport.m_CurrentTransport == creature) && !this.m_DeletedData.HasComponent(currentTransport.m_CurrentTransport) && this.m_TripSourceData.HasComponent(currentTransport.m_CurrentTransport) && this.m_TargetData.HasComponent(currentTransport.m_CurrentTransport))
            {
              // ISSUE: reference to a compiler-generated field
              TripSource tripSource = this.m_TripSourceData[currentTransport.m_CurrentTransport];
              // ISSUE: reference to a compiler-generated field
              Target target1 = this.m_TargetData[currentTransport.m_CurrentTransport];
              if (source == tripSource.m_Source && target == target1.m_Target)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_GroupQueue.Enqueue(new GroupSystem.GroupData(creature, currentTransport.m_CurrentTransport));
              }
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
    private struct GroupCreaturesJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<Human> m_HumanData;
      [ReadOnly]
      public ComponentLookup<Resident> m_ResidentData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<CarKeeper> m_CarKeeperData;
      public ComponentLookup<GroupMember> m_GroupMemberData;
      public ComponentLookup<PathOwner> m_PathOwnerData;
      public BufferLookup<GroupCreature> m_GroupCreatures;
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public NativeParallelHashSet<Entity> m_ResetTripSet;
      public NativeQueue<GroupSystem.GroupData> m_GroupQueue;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_GroupQueue.Count == 0)
          return;
        GroupBuilder<Entity> groupBuffer = new GroupBuilder<Entity>(Allocator.Temp);
        // ISSUE: variable of a compiler-generated type
        GroupSystem.GroupData groupData;
        // ISSUE: reference to a compiler-generated field
        while (this.m_GroupQueue.TryDequeue(out groupData))
        {
          // ISSUE: reference to a compiler-generated field
          if (groupData.m_Creature1 != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.AddExistingGroupMembers(ref groupBuffer, groupData.m_Creature1);
            // ISSUE: reference to a compiler-generated field
            if (groupData.m_Creature2 != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.AddExistingGroupMembers(ref groupBuffer, groupData.m_Creature2);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              groupBuffer.AddPair(groupData.m_Creature1, groupData.m_Creature2);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              groupBuffer.AddSingle(groupData.m_Creature1);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (groupData.m_Creature2 != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.AddExistingGroupMembers(ref groupBuffer, groupData.m_Creature2);
              // ISSUE: reference to a compiler-generated field
              groupBuffer.AddSingle(groupData.m_Creature2);
            }
          }
        }
        NativeArray<GroupBuilder<Entity>.Result> group;
        GroupBuilder<Entity>.Iterator iterator;
        if (!groupBuffer.TryGetFirstGroup(out group, out iterator))
          return;
        do
        {
          // ISSUE: reference to a compiler-generated method
          this.ComposeGroup((NativeSlice<GroupBuilder<Entity>.Result>) group);
        }
        while (groupBuffer.TryGetNextGroup(out group, ref iterator));
      }

      private void AddExistingGroupMembers(ref GroupBuilder<Entity> groupBuffer, Entity creature)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ResetTripSet.Contains(creature))
          return;
        // ISSUE: reference to a compiler-generated field
        if (this.m_GroupMemberData.HasComponent(creature))
        {
          // ISSUE: reference to a compiler-generated field
          creature = this.m_GroupMemberData[creature].m_Leader;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_DeletedData.HasComponent(creature) || this.m_ResetTripSet.Contains(creature))
            return;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_GroupCreatures.HasBuffer(creature))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<GroupCreature> groupCreature = this.m_GroupCreatures[creature];
        for (int index = 0; index < groupCreature.Length; ++index)
        {
          Entity creature1 = groupCreature[index].m_Creature;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_DeletedData.HasComponent(creature1) && !this.m_ResetTripSet.Contains(creature1))
            groupBuffer.AddPair(creature, creature1);
        }
      }

      private void ComposeGroup(NativeSlice<GroupBuilder<Entity>.Result> group)
      {
        if (group.Length == 1)
        {
          Entity creature = group[0].m_Item;
          // ISSUE: reference to a compiler-generated method
          this.RemoveGroupMember(creature);
          // ISSUE: reference to a compiler-generated method
          this.RemoveGroupCreatures(creature);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          Entity bestLeader = this.FindBestLeader(group);
          // ISSUE: reference to a compiler-generated method
          this.RemoveGroupMember(bestLeader);
          // ISSUE: reference to a compiler-generated method
          DynamicBuffer<GroupCreature> dynamicBuffer = this.AddGroupCreatures(bestLeader);
          for (int index = 0; index < group.Length; ++index)
          {
            Entity creature = group[index].m_Item;
            if (creature != bestLeader)
            {
              // ISSUE: reference to a compiler-generated method
              this.RemoveGroupCreatures(creature);
              // ISSUE: reference to a compiler-generated method
              this.AddGroupMember(creature, bestLeader);
              dynamicBuffer.Add(new GroupCreature(creature));
            }
          }
        }
      }

      private void RemoveGroupMember(Entity creature)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_GroupMemberData.HasComponent(creature))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<GroupMember>(creature);
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PathOwnerData.HasComponent(creature))
          return;
        // ISSUE: reference to a compiler-generated field
        PathOwner pathOwner = this.m_PathOwnerData[creature] with
        {
          m_ElementIndex = 0
        };
        pathOwner.m_State |= PathFlags.Obsolete;
        // ISSUE: reference to a compiler-generated field
        this.m_PathOwnerData[creature] = pathOwner;
        // ISSUE: reference to a compiler-generated field
        this.m_PathElements[creature].Clear();
      }

      private void RemoveGroupCreatures(Entity creature)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_GroupCreatures.HasBuffer(creature))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<GroupCreature>(creature);
      }

      private void AddGroupMember(Entity creature, Entity leader)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_GroupMemberData.HasComponent(creature))
        {
          // ISSUE: reference to a compiler-generated field
          GroupMember groupMember = this.m_GroupMemberData[creature] with
          {
            m_Leader = leader
          };
          // ISSUE: reference to a compiler-generated field
          this.m_GroupMemberData[creature] = groupMember;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<GroupMember>(creature, new GroupMember(leader));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Divert>(creature);
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PathOwnerData.HasComponent(creature))
            return;
          // ISSUE: reference to a compiler-generated field
          PathOwner pathOwner = this.m_PathOwnerData[creature] with
          {
            m_ElementIndex = 0,
            m_State = (PathFlags) 0
          };
          // ISSUE: reference to a compiler-generated field
          this.m_PathOwnerData[creature] = pathOwner;
          // ISSUE: reference to a compiler-generated field
          this.m_PathElements[creature].Clear();
        }
      }

      private DynamicBuffer<GroupCreature> AddGroupCreatures(Entity creature)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_GroupCreatures.HasBuffer(creature))
        {
          // ISSUE: reference to a compiler-generated field
          return this.m_CommandBuffer.AddBuffer<GroupCreature>(creature);
        }
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<GroupCreature> groupCreature = this.m_GroupCreatures[creature];
        groupCreature.Clear();
        return groupCreature;
      }

      private Entity FindBestLeader(NativeSlice<GroupBuilder<Entity>.Result> group)
      {
        Entity bestLeader = Entity.Null;
        int num1 = -1;
        for (int index = 0; index < group.Length; ++index)
        {
          Entity entity = group[index].m_Item;
          int num2 = 0;
          // ISSUE: reference to a compiler-generated field
          if (this.m_HumanData.HasComponent(entity))
            num2 += 10;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ResidentData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            Resident resident = this.m_ResidentData[entity];
            if ((resident.m_Flags & ResidentFlags.PreferredLeader) != ResidentFlags.None)
              num2 += 2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CarKeeperData.HasEnabledComponent<CarKeeper>(resident.m_Citizen))
              ++num2;
          }
          if (num2 > num1)
          {
            bestLeader = entity;
            num1 = num2;
          }
        }
        return bestLeader;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<ResetTrip> __Game_Creatures_ResetTrip_RO_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Target> __Game_Common_Target_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TripSource> __Game_Objects_TripSource_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Resident> __Game_Creatures_Resident_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Pet> __Game_Creatures_Pet_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Wildlife> __Game_Creatures_Wildlife_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Domesticated> __Game_Creatures_Domesticated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<GroupMember> __Game_Creatures_GroupMember_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<GroupCreature> __Game_Creatures_GroupCreature_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Target> __Game_Common_Target_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TripSource> __Game_Objects_TripSource_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> __Game_Citizens_CurrentTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdPet> __Game_Citizens_HouseholdPet_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GroupMember> __Game_Creatures_GroupMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Resident> __Game_Creatures_Resident_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Pet> __Game_Creatures_Pet_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Wildlife> __Game_Creatures_Wildlife_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Domesticated> __Game_Creatures_Domesticated_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<GroupCreature> __Game_Creatures_GroupCreature_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdAnimal> __Game_Citizens_HouseholdAnimal_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<OwnedCreature> __Game_Creatures_OwnedCreature_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Human> __Game_Creatures_Human_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarKeeper> __Game_Citizens_CarKeeper_RO_ComponentLookup;
      public ComponentLookup<GroupMember> __Game_Creatures_GroupMember_RW_ComponentLookup;
      public ComponentLookup<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentLookup;
      public BufferLookup<GroupCreature> __Game_Creatures_GroupCreature_RW_BufferLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_ResetTrip_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ResetTrip>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TripSource_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TripSource>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Resident_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Resident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Pet_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Pet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Wildlife_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Wildlife>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Domesticated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Domesticated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupMember_RO_ComponentTypeHandle = state.GetComponentTypeHandle<GroupMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupCreature_RO_BufferTypeHandle = state.GetBufferTypeHandle<GroupCreature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentLookup = state.GetComponentLookup<Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TripSource_RO_ComponentLookup = state.GetComponentLookup<TripSource>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentTransport_RO_ComponentLookup = state.GetComponentLookup<CurrentTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdPet_RO_ComponentLookup = state.GetComponentLookup<HouseholdPet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupMember_RO_ComponentLookup = state.GetComponentLookup<GroupMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Resident_RO_ComponentLookup = state.GetComponentLookup<Resident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Pet_RO_ComponentLookup = state.GetComponentLookup<Pet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Wildlife_RO_ComponentLookup = state.GetComponentLookup<Wildlife>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Domesticated_RO_ComponentLookup = state.GetComponentLookup<Domesticated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupCreature_RO_BufferLookup = state.GetBufferLookup<GroupCreature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdAnimal_RO_BufferLookup = state.GetBufferLookup<HouseholdAnimal>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_OwnedCreature_RO_BufferLookup = state.GetBufferLookup<OwnedCreature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Human_RO_ComponentLookup = state.GetComponentLookup<Human>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CarKeeper_RO_ComponentLookup = state.GetComponentLookup<CarKeeper>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupMember_RW_ComponentLookup = state.GetComponentLookup<GroupMember>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentLookup = state.GetComponentLookup<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupCreature_RW_BufferLookup = state.GetBufferLookup<GroupCreature>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
      }
    }
  }
}
