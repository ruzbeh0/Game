// Decompiled with JetBrains decompiler
// Type: Game.Citizens.HouseholdPetRemoveSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Creatures;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Citizens
{
  [CompilerGenerated]
  public class HouseholdPetRemoveSystem : GameSystemBase
  {
    private EntityQuery m_HouseholdPetQuery;
    private ModificationBarrier4 m_ModificationBarrier;
    private HouseholdPetRemoveSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdPetQuery = this.GetEntityQuery(ComponentType.ReadOnly<HouseholdPet>(), ComponentType.ReadOnly<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HouseholdPetQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdPet_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new HouseholdPetRemoveSystem.RemovePetJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_HouseholdPetType = this.__TypeHandle.__Game_Citizens_HouseholdPet_RO_ComponentTypeHandle,
        m_CurrentTransportType = this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentTypeHandle,
        m_CreatureData = this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup,
        m_HouseholdAnimals = this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RW_BufferLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<HouseholdPetRemoveSystem.RemovePetJob>(this.m_HouseholdPetQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(producerJob);
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
    public HouseholdPetRemoveSystem()
    {
    }

    [BurstCompile]
    private struct RemovePetJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdPet> m_HouseholdPetType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentTransport> m_CurrentTransportType;
      [ReadOnly]
      public ComponentLookup<Creature> m_CreatureData;
      public BufferLookup<HouseholdAnimal> m_HouseholdAnimals;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HouseholdPet> nativeArray2 = chunk.GetNativeArray<HouseholdPet>(ref this.m_HouseholdPetType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          HouseholdPet householdPet = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_HouseholdAnimals.HasBuffer(householdPet.m_Household))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<HouseholdAnimal> householdAnimal = this.m_HouseholdAnimals[householdPet.m_Household];
            CollectionUtils.RemoveValue<HouseholdAnimal>(householdAnimal, new HouseholdAnimal(nativeArray1[index]));
            if (householdAnimal.Length == 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<HouseholdAnimal>(householdPet.m_Household);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentTransport> nativeArray3 = chunk.GetNativeArray<CurrentTransport>(ref this.m_CurrentTransportType);
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          CurrentTransport currentTransport = nativeArray3[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_CreatureData.HasComponent(currentTransport.m_CurrentTransport))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(currentTransport.m_CurrentTransport, new Deleted());
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
      public ComponentTypeHandle<HouseholdPet> __Game_Citizens_HouseholdPet_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentTransport> __Game_Citizens_CurrentTransport_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Creature> __Game_Creatures_Creature_RO_ComponentLookup;
      public BufferLookup<HouseholdAnimal> __Game_Citizens_HouseholdAnimal_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdPet_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HouseholdPet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentTransport_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Creature_RO_ComponentLookup = state.GetComponentLookup<Creature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdAnimal_RW_BufferLookup = state.GetBufferLookup<HouseholdAnimal>();
      }
    }
  }
}
