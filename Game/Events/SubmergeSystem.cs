// Decompiled with JetBrains decompiler
// Type: Game.Events.SubmergeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Common;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Events
{
  [CompilerGenerated]
  public class SubmergeSystem : GameSystemBase
  {
    private ModificationBarrier4 m_ModificationBarrier;
    private EntityQuery m_FaceWeatherQuery;
    private EntityArchetype m_JournalDataArchetype;
    private SubmergeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_FaceWeatherQuery = this.GetEntityQuery(ComponentType.ReadOnly<Submerge>(), ComponentType.ReadOnly<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_JournalDataArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<AddEventJournalData>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_FaceWeatherQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_FaceWeatherQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_TargetElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Flooded_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Submerge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle jobHandle = new SubmergeSystem.SubmergeJob()
      {
        m_Chunks = archetypeChunkListAsync,
        m_SubmergeType = this.__TypeHandle.__Game_Events_Submerge_RO_ComponentTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_FloodedData = this.__TypeHandle.__Game_Events_Flooded_RW_ComponentLookup,
        m_TargetElements = this.__TypeHandle.__Game_Events_TargetElement_RW_BufferLookup,
        m_JournalDataArchetype = this.m_JournalDataArchetype,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<SubmergeSystem.SubmergeJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      archetypeChunkListAsync.Dispose(jobHandle);
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
    public SubmergeSystem()
    {
    }

    [BurstCompile]
    private struct SubmergeJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<Submerge> m_SubmergeType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      public ComponentLookup<Flooded> m_FloodedData;
      public BufferLookup<TargetElement> m_TargetElements;
      public EntityArchetype m_JournalDataArchetype;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        int capacity = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          capacity += this.m_Chunks[index].Count;
        }
        NativeParallelHashMap<Entity, Flooded> nativeParallelHashMap = new NativeParallelHashMap<Entity, Flooded>(capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Submerge> nativeArray = this.m_Chunks[index1].GetNativeArray<Submerge>(ref this.m_SubmergeType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            Submerge submerge = nativeArray[index2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(submerge.m_Target))
            {
              Flooded flooded1 = new Flooded(submerge.m_Event, submerge.m_Depth);
              Flooded flooded2;
              if (nativeParallelHashMap.TryGetValue(submerge.m_Target, out flooded2))
              {
                if ((double) flooded1.m_Depth > (double) flooded2.m_Depth)
                  nativeParallelHashMap[submerge.m_Target] = flooded1;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_FloodedData.HasComponent(submerge.m_Target))
                {
                  // ISSUE: reference to a compiler-generated field
                  flooded2 = this.m_FloodedData[submerge.m_Target];
                  if ((double) flooded1.m_Depth > (double) flooded2.m_Depth)
                    nativeParallelHashMap.TryAdd(submerge.m_Target, flooded1);
                }
                else
                  nativeParallelHashMap.TryAdd(submerge.m_Target, flooded1);
              }
            }
          }
        }
        if (nativeParallelHashMap.Count() == 0)
          return;
        NativeArray<Entity> keyArray = nativeParallelHashMap.GetKeyArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < keyArray.Length; ++index)
        {
          Entity entity = keyArray[index];
          Flooded flooded = nativeParallelHashMap[entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_FloodedData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_FloodedData[entity].m_Event != flooded.m_Event)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_TargetElements.HasBuffer(flooded.m_Event))
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.TryAddUniqueValue<TargetElement>(this.m_TargetElements[flooded.m_Event], new TargetElement(entity));
              }
              // ISSUE: reference to a compiler-generated method
              this.AddJournalData(entity, flooded);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_FloodedData[entity] = flooded;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TargetElements.HasBuffer(flooded.m_Event))
            {
              // ISSUE: reference to a compiler-generated field
              CollectionUtils.TryAddUniqueValue<TargetElement>(this.m_TargetElements[flooded.m_Event], new TargetElement(entity));
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Flooded>(entity, flooded);
            // ISSUE: reference to a compiler-generated method
            this.AddJournalData(entity, flooded);
          }
        }
      }

      private void AddJournalData(Entity target, Flooded flooded)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_BuildingData.HasComponent(target))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<AddEventJournalData>(this.m_CommandBuffer.CreateEntity(this.m_JournalDataArchetype), new AddEventJournalData(flooded.m_Event, EventDataTrackingType.Damages));
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Submerge> __Game_Events_Submerge_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      public ComponentLookup<Flooded> __Game_Events_Flooded_RW_ComponentLookup;
      public BufferLookup<TargetElement> __Game_Events_TargetElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Submerge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Submerge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Flooded_RW_ComponentLookup = state.GetComponentLookup<Flooded>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_TargetElement_RW_BufferLookup = state.GetBufferLookup<TargetElement>();
      }
    }
  }
}
