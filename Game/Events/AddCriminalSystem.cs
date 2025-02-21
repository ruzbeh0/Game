// Decompiled with JetBrains decompiler
// Type: Game.Events.AddCriminalSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Citizens;
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
  public class AddCriminalSystem : GameSystemBase
  {
    private ModificationBarrier4 m_ModificationBarrier;
    private EntityQuery m_AddCriminalQuery;
    private AddCriminalSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_AddCriminalQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Common.Event>(), ComponentType.ReadOnly<AddCriminal>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_AddCriminalQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_AddCriminalQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_TargetElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Criminal_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_AddCriminal_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle jobHandle = new AddCriminalSystem.AddCriminalJob()
      {
        m_Chunks = archetypeChunkListAsync,
        m_AddCriminalType = this.__TypeHandle.__Game_Events_AddCriminal_RO_ComponentTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_CriminalData = this.__TypeHandle.__Game_Citizens_Criminal_RW_ComponentLookup,
        m_TargetElements = this.__TypeHandle.__Game_Events_TargetElement_RW_BufferLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<AddCriminalSystem.AddCriminalJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
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
    public AddCriminalSystem()
    {
    }

    [BurstCompile]
    private struct AddCriminalJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<AddCriminal> m_AddCriminalType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public ComponentLookup<Criminal> m_CriminalData;
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
        NativeParallelHashMap<Entity, Criminal> nativeParallelHashMap = new NativeParallelHashMap<Entity, Criminal>(capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<AddCriminal> nativeArray = this.m_Chunks[index1].GetNativeArray<AddCriminal>(ref this.m_AddCriminalType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            AddCriminal addCriminal = nativeArray[index2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(addCriminal.m_Target))
            {
              Criminal criminal2 = new Criminal(addCriminal.m_Event, addCriminal.m_Flags);
              Criminal criminal1;
              if (nativeParallelHashMap.TryGetValue(addCriminal.m_Target, out criminal1))
              {
                // ISSUE: reference to a compiler-generated method
                nativeParallelHashMap[addCriminal.m_Target] = this.MergeCriminals(criminal1, criminal2);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_CriminalData.HasComponent(addCriminal.m_Target))
                {
                  // ISSUE: reference to a compiler-generated field
                  criminal1 = this.m_CriminalData[addCriminal.m_Target];
                  // ISSUE: reference to a compiler-generated method
                  nativeParallelHashMap.TryAdd(addCriminal.m_Target, this.MergeCriminals(criminal1, criminal2));
                }
                else
                  nativeParallelHashMap.TryAdd(addCriminal.m_Target, criminal2);
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
          Criminal component = nativeParallelHashMap[entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_CriminalData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_CriminalData[entity].m_Event != component.m_Event && this.m_TargetElements.HasBuffer(component.m_Event))
            {
              // ISSUE: reference to a compiler-generated field
              CollectionUtils.TryAddUniqueValue<TargetElement>(this.m_TargetElements[component.m_Event], new TargetElement(entity));
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CriminalData[entity] = component;
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
            this.m_CommandBuffer.AddComponent<Criminal>(entity, component);
          }
        }
      }

      private Criminal MergeCriminals(Criminal criminal1, Criminal criminal2)
      {
        if (((criminal1.m_Flags ^ criminal2.m_Flags) & CriminalFlags.Prisoner) != (CriminalFlags) 0)
          return (criminal1.m_Flags & CriminalFlags.Prisoner) == (CriminalFlags) 0 ? criminal2 : criminal1;
        Criminal criminal;
        if (criminal1.m_Event != Entity.Null != (criminal2.m_Event != Entity.Null))
        {
          criminal = criminal1.m_Event != Entity.Null ? criminal1 : criminal2;
          criminal.m_Flags |= criminal1.m_Event != Entity.Null ? criminal2.m_Flags : criminal1.m_Flags;
        }
        else
        {
          criminal = criminal1;
          criminal.m_Flags |= criminal2.m_Flags;
        }
        return criminal;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<AddCriminal> __Game_Events_AddCriminal_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      public ComponentLookup<Criminal> __Game_Citizens_Criminal_RW_ComponentLookup;
      public BufferLookup<TargetElement> __Game_Events_TargetElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_AddCriminal_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AddCriminal>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Criminal_RW_ComponentLookup = state.GetComponentLookup<Criminal>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_TargetElement_RW_BufferLookup = state.GetBufferLookup<TargetElement>();
      }
    }
  }
}
