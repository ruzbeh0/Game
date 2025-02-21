// Decompiled with JetBrains decompiler
// Type: Game.Events.IgniteSystem
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
  public class IgniteSystem : GameSystemBase
  {
    private ModificationBarrier4 m_ModificationBarrier;
    private EntityQuery m_IgniteQuery;
    private EntityArchetype m_JournalDataArchetype;
    private IgniteSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_IgniteQuery = this.GetEntityQuery(ComponentType.ReadOnly<Ignite>(), ComponentType.ReadOnly<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_JournalDataArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<AddEventJournalData>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_IgniteQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_IgniteQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_TargetElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_OnFire_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Ignite_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle jobHandle = new IgniteSystem.IgniteFireJob()
      {
        m_Chunks = archetypeChunkListAsync,
        m_IgniteType = this.__TypeHandle.__Game_Events_Ignite_RO_ComponentTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_OnFireData = this.__TypeHandle.__Game_Events_OnFire_RW_ComponentLookup,
        m_TargetElements = this.__TypeHandle.__Game_Events_TargetElement_RW_BufferLookup,
        m_JournalDataArchetype = this.m_JournalDataArchetype,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<IgniteSystem.IgniteFireJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
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
    public IgniteSystem()
    {
    }

    [BurstCompile]
    private struct IgniteFireJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<Ignite> m_IgniteType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      public ComponentLookup<OnFire> m_OnFireData;
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
        NativeParallelHashMap<Entity, OnFire> nativeParallelHashMap = new NativeParallelHashMap<Entity, OnFire>(capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Ignite> nativeArray = this.m_Chunks[index1].GetNativeArray<Ignite>(ref this.m_IgniteType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            Ignite ignite = nativeArray[index2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(ignite.m_Target))
            {
              OnFire onFire1 = new OnFire(ignite.m_Event, ignite.m_Intensity, ignite.m_RequestFrame);
              OnFire onFire2;
              if (nativeParallelHashMap.TryGetValue(ignite.m_Target, out onFire2))
              {
                if ((double) onFire1.m_Intensity > (double) onFire2.m_Intensity)
                  nativeParallelHashMap[ignite.m_Target] = onFire1;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_OnFireData.HasComponent(ignite.m_Target))
                {
                  // ISSUE: reference to a compiler-generated field
                  onFire2 = this.m_OnFireData[ignite.m_Target];
                  if ((double) onFire1.m_Intensity > (double) onFire2.m_Intensity)
                    nativeParallelHashMap.TryAdd(ignite.m_Target, onFire1);
                }
                else
                  nativeParallelHashMap.TryAdd(ignite.m_Target, onFire1);
              }
            }
          }
        }
        if (nativeParallelHashMap.Count() == 0)
          return;
        NativeArray<Entity> keyArray = nativeParallelHashMap.GetKeyArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index3 = 0; index3 < keyArray.Length; ++index3)
        {
          Entity entity = keyArray[index3];
          OnFire onFire3 = nativeParallelHashMap[entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_OnFireData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            OnFire onFire4 = this.m_OnFireData[entity];
            if (onFire4.m_Event != onFire3.m_Event)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_TargetElements.HasBuffer(onFire3.m_Event))
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.TryAddUniqueValue<TargetElement>(this.m_TargetElements[onFire3.m_Event], new TargetElement(entity));
              }
              // ISSUE: reference to a compiler-generated method
              this.AddJournalData(entity, onFire3);
            }
            if (onFire4.m_RequestFrame < onFire3.m_RequestFrame)
              onFire3.m_RequestFrame = onFire4.m_RequestFrame;
            onFire3.m_RescueRequest = onFire4.m_RescueRequest;
            // ISSUE: reference to a compiler-generated field
            this.m_OnFireData[entity] = onFire3;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TargetElements.HasBuffer(onFire3.m_Event))
            {
              // ISSUE: reference to a compiler-generated field
              CollectionUtils.TryAddUniqueValue<TargetElement>(this.m_TargetElements[onFire3.m_Event], new TargetElement(entity));
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<OnFire>(entity, onFire3);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<BatchesUpdated>(entity, new BatchesUpdated());
            // ISSUE: reference to a compiler-generated method
            this.AddJournalData(entity, onFire3);
            DynamicBuffer<InstalledUpgrade> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_InstalledUpgrades.TryGetBuffer(entity, out bufferData))
            {
              for (int index4 = 0; index4 < bufferData.Length; ++index4)
              {
                Entity upgrade = bufferData[index4].m_Upgrade;
                // ISSUE: reference to a compiler-generated field
                if (!this.m_BuildingData.HasComponent(upgrade))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<BatchesUpdated>(upgrade);
                }
              }
            }
          }
        }
      }

      private void AddJournalData(Entity target, OnFire onFire)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_BuildingData.HasComponent(target))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<AddEventJournalData>(this.m_CommandBuffer.CreateEntity(this.m_JournalDataArchetype), new AddEventJournalData(onFire.m_Event, EventDataTrackingType.Damages));
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Ignite> __Game_Events_Ignite_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      public ComponentLookup<OnFire> __Game_Events_OnFire_RW_ComponentLookup;
      public BufferLookup<TargetElement> __Game_Events_TargetElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Ignite_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Ignite>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_OnFire_RW_ComponentLookup = state.GetComponentLookup<OnFire>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_TargetElement_RW_BufferLookup = state.GetBufferLookup<TargetElement>();
      }
    }
  }
}
