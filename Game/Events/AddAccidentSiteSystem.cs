// Decompiled with JetBrains decompiler
// Type: Game.Events.AddAccidentSiteSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Events
{
  [CompilerGenerated]
  public class AddAccidentSiteSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private ModificationBarrier4 m_ModificationBarrier;
    private EntityQuery m_ImpactQuery;
    private AddAccidentSiteSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_ImpactQuery = this.GetEntityQuery(ComponentType.ReadOnly<AddAccidentSite>(), ComponentType.ReadOnly<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ImpactQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_ImpactQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_TargetElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CrimeProducer_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_AccidentSite_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_AddAccidentSite_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle jobHandle = new AddAccidentSiteSystem.AddAccidentSiteJob()
      {
        m_Chunks = archetypeChunkListAsync,
        m_AddAccidentSiteType = this.__TypeHandle.__Game_Events_AddAccidentSite_RO_ComponentTypeHandle,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_AccidentSiteData = this.__TypeHandle.__Game_Events_AccidentSite_RW_ComponentLookup,
        m_CrimeProducerData = this.__TypeHandle.__Game_Buildings_CrimeProducer_RW_ComponentLookup,
        m_TargetElements = this.__TypeHandle.__Game_Events_TargetElement_RW_BufferLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<AddAccidentSiteSystem.AddAccidentSiteJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
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
    public AddAccidentSiteSystem()
    {
    }

    [BurstCompile]
    private struct AddAccidentSiteJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<AddAccidentSite> m_AddAccidentSiteType;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public ComponentLookup<AccidentSite> m_AccidentSiteData;
      public ComponentLookup<CrimeProducer> m_CrimeProducerData;
      public BufferLookup<TargetElement> m_TargetElements;
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
        NativeParallelHashMap<Entity, AccidentSite> nativeParallelHashMap = new NativeParallelHashMap<Entity, AccidentSite>(capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<AddAccidentSite> nativeArray = this.m_Chunks[index1].GetNativeArray<AddAccidentSite>(ref this.m_AddAccidentSiteType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            AddAccidentSite addAccidentSite = nativeArray[index2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(addAccidentSite.m_Target))
            {
              // ISSUE: reference to a compiler-generated field
              AccidentSite accidentSite2 = new AccidentSite(addAccidentSite.m_Event, addAccidentSite.m_Flags, this.m_SimulationFrame);
              AccidentSite accidentSite1;
              if (nativeParallelHashMap.TryGetValue(addAccidentSite.m_Target, out accidentSite1))
              {
                // ISSUE: reference to a compiler-generated method
                nativeParallelHashMap[addAccidentSite.m_Target] = this.MergeAccidentSites(accidentSite1, accidentSite2);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_AccidentSiteData.HasComponent(addAccidentSite.m_Target))
                {
                  // ISSUE: reference to a compiler-generated field
                  accidentSite1 = this.m_AccidentSiteData[addAccidentSite.m_Target];
                  // ISSUE: reference to a compiler-generated method
                  nativeParallelHashMap.TryAdd(addAccidentSite.m_Target, this.MergeAccidentSites(accidentSite1, accidentSite2));
                }
                else
                  nativeParallelHashMap.TryAdd(addAccidentSite.m_Target, accidentSite2);
              }
              // ISSUE: reference to a compiler-generated field
              if ((accidentSite2.m_Flags & AccidentSiteFlags.CrimeScene) != (AccidentSiteFlags) 0 && this.m_CrimeProducerData.HasComponent(addAccidentSite.m_Target))
              {
                // ISSUE: reference to a compiler-generated field
                CrimeProducer crimeProducer = this.m_CrimeProducerData[addAccidentSite.m_Target];
                crimeProducer.m_Crime *= 0.3f;
                // ISSUE: reference to a compiler-generated field
                this.m_CrimeProducerData[addAccidentSite.m_Target] = crimeProducer;
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
          AccidentSite component = nativeParallelHashMap[entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_AccidentSiteData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_AccidentSiteData[entity].m_Event != component.m_Event && this.m_TargetElements.HasBuffer(component.m_Event))
            {
              // ISSUE: reference to a compiler-generated field
              CollectionUtils.TryAddUniqueValue<TargetElement>(this.m_TargetElements[component.m_Event], new TargetElement(entity));
            }
            // ISSUE: reference to a compiler-generated field
            this.m_AccidentSiteData[entity] = component;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TargetElements.HasBuffer(component.m_Event))
            {
              // ISSUE: reference to a compiler-generated field
              CollectionUtils.TryAddUniqueValue<TargetElement>(this.m_TargetElements[component.m_Event], new TargetElement(entity));
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<AccidentSite>(entity, component);
          }
        }
      }

      private AccidentSite MergeAccidentSites(
        AccidentSite accidentSite1,
        AccidentSite accidentSite2)
      {
        AccidentSite accidentSite;
        if (accidentSite1.m_Event != Entity.Null != (accidentSite2.m_Event != Entity.Null))
        {
          accidentSite = accidentSite1.m_Event != Entity.Null ? accidentSite1 : accidentSite2;
          accidentSite.m_Flags |= accidentSite1.m_Event != Entity.Null ? accidentSite2.m_Flags : accidentSite1.m_Flags;
        }
        else
        {
          accidentSite = accidentSite1;
          accidentSite.m_Flags |= accidentSite2.m_Flags;
        }
        return accidentSite;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<AddAccidentSite> __Game_Events_AddAccidentSite_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      public ComponentLookup<AccidentSite> __Game_Events_AccidentSite_RW_ComponentLookup;
      public ComponentLookup<CrimeProducer> __Game_Buildings_CrimeProducer_RW_ComponentLookup;
      public BufferLookup<TargetElement> __Game_Events_TargetElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_AddAccidentSite_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AddAccidentSite>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_AccidentSite_RW_ComponentLookup = state.GetComponentLookup<AccidentSite>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CrimeProducer_RW_ComponentLookup = state.GetComponentLookup<CrimeProducer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_TargetElement_RW_BufferLookup = state.GetBufferLookup<TargetElement>();
      }
    }
  }
}
