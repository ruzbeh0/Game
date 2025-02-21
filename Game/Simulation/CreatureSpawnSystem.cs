// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CreatureSpawnSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Common;
using Game.Creatures;
using Game.Objects;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CreatureSpawnSystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_CreatureQuery;
    private ComponentTypeSet m_TripSourceRemoveTypes;
    private CreatureSpawnSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatureQuery = this.GetEntityQuery(ComponentType.ReadWrite<TripSource>(), ComponentType.ReadOnly<Creature>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatureQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeList<CreatureSpawnSystem.SpawnData> nativeList1 = new NativeList<CreatureSpawnSystem.SpawnData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<CreatureSpawnSystem.SpawnRange> nativeList2 = new NativeList<CreatureSpawnSystem.SpawnRange>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TripSource_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CreatureSpawnSystem.GroupSpawnSourcesJob jobData1 = new CreatureSpawnSystem.GroupSpawnSourcesJob()
      {
        m_Chunks = this.m_CreatureQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TripSourceType = this.__TypeHandle.__Game_Objects_TripSource_RW_ComponentTypeHandle,
        m_SpawnData = nativeList1,
        m_SpawnGroups = nativeList2
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Occupant_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Patient_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CreatureSpawnSystem.TrySpawnCreaturesJob jobData2 = new CreatureSpawnSystem.TrySpawnCreaturesJob()
      {
        m_SpawnData = nativeList1.AsDeferredJobArray(),
        m_SpawnGroups = nativeList2.AsDeferredJobArray(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_ResidentData = this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup,
        m_Patients = this.__TypeHandle.__Game_Buildings_Patient_RW_BufferLookup,
        m_Occupants = this.__TypeHandle.__Game_Buildings_Occupant_RW_BufferLookup
      };
      JobHandle jobHandle1 = jobData1.Schedule<CreatureSpawnSystem.GroupSpawnSourcesJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      NativeList<CreatureSpawnSystem.SpawnRange> list = nativeList2;
      JobHandle dependsOn = jobHandle1;
      JobHandle jobHandle2 = jobData2.Schedule<CreatureSpawnSystem.TrySpawnCreaturesJob, CreatureSpawnSystem.SpawnRange>(list, 1, dependsOn);
      nativeList1.Dispose(jobHandle2);
      nativeList2.Dispose(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle2);
      this.Dependency = jobHandle2;
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
    public CreatureSpawnSystem()
    {
    }

    private struct SpawnData : IComparable<CreatureSpawnSystem.SpawnData>
    {
      public Entity m_Source;
      public Entity m_Creature;
      public int m_Priority;

      public int CompareTo(CreatureSpawnSystem.SpawnData other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.select(this.m_Priority - other.m_Priority, this.m_Source.Index - other.m_Source.Index, this.m_Source.Index != other.m_Source.Index);
      }
    }

    private struct SpawnRange
    {
      public int m_Start;
      public int m_End;
    }

    [BurstCompile]
    private struct GroupSpawnSourcesJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<TripSource> m_TripSourceType;
      public NativeList<CreatureSpawnSystem.SpawnData> m_SpawnData;
      public NativeList<CreatureSpawnSystem.SpawnRange> m_SpawnGroups;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<TripSource> nativeArray2 = chunk.GetNativeArray<TripSource>(ref this.m_TripSourceType);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            TripSource tripSource = nativeArray2[index2];
            if (tripSource.m_Timer <= 0)
            {
              // ISSUE: variable of a compiler-generated type
              CreatureSpawnSystem.SpawnData spawnData;
              // ISSUE: reference to a compiler-generated field
              spawnData.m_Source = tripSource.m_Source;
              // ISSUE: reference to a compiler-generated field
              spawnData.m_Creature = nativeArray1[index2];
              // ISSUE: reference to a compiler-generated field
              spawnData.m_Priority = tripSource.m_Timer;
              // ISSUE: reference to a compiler-generated field
              this.m_SpawnData.Add(in spawnData);
            }
            tripSource.m_Timer -= 16;
            nativeArray2[index2] = tripSource;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_SpawnData.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_SpawnData.Sort<CreatureSpawnSystem.SpawnData>();
        // ISSUE: variable of a compiler-generated type
        CreatureSpawnSystem.SpawnRange spawnRange;
        // ISSUE: reference to a compiler-generated field
        spawnRange.m_Start = -1;
        Entity entity = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_SpawnData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity source = this.m_SpawnData[index].m_Source;
          if (source != entity)
          {
            // ISSUE: reference to a compiler-generated field
            if (spawnRange.m_Start != -1)
            {
              // ISSUE: reference to a compiler-generated field
              spawnRange.m_End = index;
              // ISSUE: reference to a compiler-generated field
              this.m_SpawnGroups.Add(in spawnRange);
            }
            // ISSUE: reference to a compiler-generated field
            spawnRange.m_Start = index;
            entity = source;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (spawnRange.m_Start == -1)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        spawnRange.m_End = this.m_SpawnData.Length;
        // ISSUE: reference to a compiler-generated field
        this.m_SpawnGroups.Add(in spawnRange);
      }
    }

    [BurstCompile]
    private struct TrySpawnCreaturesJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<CreatureSpawnSystem.SpawnData> m_SpawnData;
      [ReadOnly]
      public NativeArray<CreatureSpawnSystem.SpawnRange> m_SpawnGroups;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      [ReadOnly]
      public ComponentLookup<Resident> m_ResidentData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Patient> m_Patients;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Occupant> m_Occupants;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        CreatureSpawnSystem.SpawnRange spawnGroup = this.m_SpawnGroups[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity source = this.m_SpawnData[spawnGroup.m_Start].m_Source;
        DynamicBuffer<Patient> buffer1 = new DynamicBuffer<Patient>();
        DynamicBuffer<Occupant> buffer2 = new DynamicBuffer<Occupant>();
        // ISSUE: reference to a compiler-generated field
        if (this.m_Patients.HasBuffer(source))
        {
          // ISSUE: reference to a compiler-generated field
          buffer1 = this.m_Patients[source];
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_Occupants.HasBuffer(source))
        {
          // ISSUE: reference to a compiler-generated field
          buffer2 = this.m_Occupants[source];
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (int start = spawnGroup.m_Start; start < spawnGroup.m_End; ++start)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity creature = this.m_SpawnData[start].m_Creature;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<TripSource>(index, creature);
          // ISSUE: reference to a compiler-generated field
          if (this.m_ResidentData.HasComponent(creature))
          {
            // ISSUE: reference to a compiler-generated field
            Resident resident = this.m_ResidentData[creature];
            if (buffer1.IsCreated)
              CollectionUtils.RemoveValue<Patient>(buffer1, new Patient(resident.m_Citizen));
            if (buffer2.IsCreated)
              CollectionUtils.RemoveValue<Occupant>(buffer2, new Occupant(resident.m_Citizen));
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public ComponentTypeHandle<TripSource> __Game_Objects_TripSource_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Resident> __Game_Creatures_Resident_RO_ComponentLookup;
      public BufferLookup<Patient> __Game_Buildings_Patient_RW_BufferLookup;
      public BufferLookup<Occupant> __Game_Buildings_Occupant_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TripSource_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TripSource>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Resident_RO_ComponentLookup = state.GetComponentLookup<Resident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Patient_RW_BufferLookup = state.GetBufferLookup<Patient>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Occupant_RW_BufferLookup = state.GetBufferLookup<Occupant>();
      }
    }
  }
}
