// Decompiled with JetBrains decompiler
// Type: Game.Citizens.HouseholdPetInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Citizens
{
  [CompilerGenerated]
  public class HouseholdPetInitializeSystem : GameSystemBase
  {
    private EntityQuery m_HouseholdPetQuery;
    private ModificationBarrier5 m_ModificationBarrier;
    private HouseholdPetInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdPetQuery = this.GetEntityQuery(ComponentType.ReadWrite<HouseholdPet>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Temp>());
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
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdPet_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
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
      JobHandle producerJob = new HouseholdPetInitializeSystem.InitializeHouseholdPetJob()
      {
        m_Chunks = this.m_HouseholdPetQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_HouseholdPetType = this.__TypeHandle.__Game_Citizens_HouseholdPet_RO_ComponentTypeHandle,
        m_HouseholdData = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_HouseholdAnimals = this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RW_BufferLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<HouseholdPetInitializeSystem.InitializeHouseholdPetJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
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
    public HouseholdPetInitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeHouseholdPetJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdPet> m_HouseholdPetType;
      [ReadOnly]
      public ComponentLookup<Household> m_HouseholdData;
      public BufferLookup<HouseholdAnimal> m_HouseholdAnimals;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        NativeParallelMultiHashMap<Entity, Entity> parallelMultiHashMap = new NativeParallelMultiHashMap<Entity, Entity>();
        NativeList<Entity> nativeList = new NativeList<Entity>();
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<HouseholdPet> nativeArray2 = chunk.GetNativeArray<HouseholdPet>(ref this.m_HouseholdPetType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Entity household = nativeArray2[index2].m_Household;
            Entity householdPet = nativeArray1[index2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_HouseholdAnimals.HasBuffer(household))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_HouseholdAnimals[household].Add(new HouseholdAnimal(householdPet));
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_HouseholdData.HasComponent(household))
              {
                if (!parallelMultiHashMap.IsCreated)
                {
                  parallelMultiHashMap = new NativeParallelMultiHashMap<Entity, Entity>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                  nativeList = new NativeList<Entity>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                }
                if (!parallelMultiHashMap.ContainsKey(household))
                  nativeList.Add(in household);
                parallelMultiHashMap.Add(household, householdPet);
              }
            }
          }
        }
        if (!parallelMultiHashMap.IsCreated)
          return;
        for (int index = 0; index < nativeList.Length; ++index)
        {
          Entity entity = nativeList[index];
          Entity householdPet;
          NativeParallelMultiHashMapIterator<Entity> it;
          if (parallelMultiHashMap.TryGetFirstValue(entity, out householdPet, out it))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<HouseholdAnimal> dynamicBuffer = this.m_CommandBuffer.AddBuffer<HouseholdAnimal>(entity);
            do
            {
              dynamicBuffer.Add(new HouseholdAnimal(householdPet));
            }
            while (parallelMultiHashMap.TryGetNextValue(out householdPet, ref it));
          }
        }
        nativeList.Dispose();
        parallelMultiHashMap.Dispose();
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdPet> __Game_Citizens_HouseholdPet_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      public BufferLookup<HouseholdAnimal> __Game_Citizens_HouseholdAnimal_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdPet_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HouseholdPet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdAnimal_RW_BufferLookup = state.GetBufferLookup<HouseholdAnimal>();
      }
    }
  }
}
