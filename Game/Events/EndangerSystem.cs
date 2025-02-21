// Decompiled with JetBrains decompiler
// Type: Game.Events.EndangerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

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
  public class EndangerSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private ModificationBarrier4 m_ModificationBarrier;
    private EntityQuery m_EndangerQuery;
    private EndangerSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndangerQuery = this.GetEntityQuery(ComponentType.ReadOnly<Endanger>(), ComponentType.ReadOnly<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EndangerQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_EndangerQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InDanger_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Hospital_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_School_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Endanger_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle jobHandle = new EndangerSystem.EndangerJob()
      {
        m_Chunks = archetypeChunkListAsync,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_EndangerType = this.__TypeHandle.__Game_Events_Endanger_RO_ComponentTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_SchoolData = this.__TypeHandle.__Game_Buildings_School_RO_ComponentLookup,
        m_HospitalData = this.__TypeHandle.__Game_Buildings_Hospital_RO_ComponentLookup,
        m_InDangerData = this.__TypeHandle.__Game_Events_InDanger_RW_ComponentLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<EndangerSystem.EndangerJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
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
    public EndangerSystem()
    {
    }

    [BurstCompile]
    private struct EndangerJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public ComponentTypeHandle<Endanger> m_EndangerType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.School> m_SchoolData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Hospital> m_HospitalData;
      public ComponentLookup<InDanger> m_InDangerData;
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
        NativeParallelHashMap<Entity, InDanger> nativeParallelHashMap = new NativeParallelHashMap<Entity, InDanger>(capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Endanger> nativeArray = this.m_Chunks[index1].GetNativeArray<Endanger>(ref this.m_EndangerType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            Endanger endanger = nativeArray[index2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(endanger.m_Target))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((endanger.m_Flags & DangerFlags.Evacuate) != (DangerFlags) 0 && (this.m_SchoolData.HasComponent(endanger.m_Target) || this.m_HospitalData.HasComponent(endanger.m_Target)))
                endanger.m_Flags |= DangerFlags.UseTransport;
              InDanger inDanger1 = new InDanger(endanger.m_Event, Entity.Null, endanger.m_Flags, endanger.m_EndFrame);
              InDanger inDanger2;
              if (nativeParallelHashMap.TryGetValue(endanger.m_Target, out inDanger2))
              {
                // ISSUE: reference to a compiler-generated field
                if (EventUtils.IsWorse(inDanger1.m_Flags, inDanger2.m_Flags) || inDanger2.m_EndFrame < this.m_SimulationFrame + 64U)
                  nativeParallelHashMap[endanger.m_Target] = inDanger1;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_InDangerData.HasComponent(endanger.m_Target))
                {
                  // ISSUE: reference to a compiler-generated field
                  inDanger2 = this.m_InDangerData[endanger.m_Target];
                  // ISSUE: reference to a compiler-generated field
                  if (EventUtils.IsWorse(inDanger1.m_Flags, inDanger2.m_Flags) || inDanger2.m_EndFrame < this.m_SimulationFrame + 64U)
                    nativeParallelHashMap.TryAdd(endanger.m_Target, inDanger1);
                }
                else
                  nativeParallelHashMap.TryAdd(endanger.m_Target, inDanger1);
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
          InDanger component = nativeParallelHashMap[entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_InDangerData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            InDanger inDanger = this.m_InDangerData[entity];
            // ISSUE: reference to a compiler-generated field
            this.m_InDangerData[entity] = component;
            if (component.m_Flags != inDanger.m_Flags)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<EffectsUpdated>(entity, new EffectsUpdated());
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<InDanger>(entity, component);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<EffectsUpdated>(entity, new EffectsUpdated());
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Endanger> __Game_Events_Endanger_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.School> __Game_Buildings_School_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Hospital> __Game_Buildings_Hospital_RO_ComponentLookup;
      public ComponentLookup<InDanger> __Game_Events_InDanger_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Endanger_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Endanger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_School_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.School>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Hospital_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Hospital>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InDanger_RW_ComponentLookup = state.GetComponentLookup<InDanger>();
      }
    }
  }
}
