// Decompiled with JetBrains decompiler
// Type: Game.Objects.DamageSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Simulation;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  [CompilerGenerated]
  public class DamageSystem : GameSystemBase
  {
    private ModificationBarrier2 m_ModificationBarrier;
    private EntityQuery m_EventQuery;
    private DamageSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier2>();
      // ISSUE: reference to a compiler-generated field
      this.m_EventQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Common.Event>(), ComponentType.ReadOnly<Damage>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EventQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_EventQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damage_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new DamageSystem.DamageObjectsJob()
      {
        m_Chunks = archetypeChunkListAsync,
        m_DamageType = this.__TypeHandle.__Game_Objects_Damage_RO_ComponentTypeHandle,
        m_VehicleData = this.__TypeHandle.__Game_Vehicles_Vehicle_RW_ComponentLookup,
        m_DamagedData = this.__TypeHandle.__Game_Objects_Damaged_RW_ComponentLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<DamageSystem.DamageObjectsJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
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
    public DamageSystem()
    {
    }

    [BurstCompile]
    private struct DamageObjectsJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<Damage> m_DamageType;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      public ComponentLookup<Damaged> m_DamagedData;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        NativeParallelHashMap<Entity, Damaged> nativeParallelHashMap = new NativeParallelHashMap<Entity, Damaged>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Damage> nativeArray = this.m_Chunks[index1].GetNativeArray<Damage>(ref this.m_DamageType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            Damage damage = nativeArray[index2];
            Damaged damaged;
            if (nativeParallelHashMap.TryGetValue(damage.m_Object, out damaged))
            {
              damaged.m_Damage += damage.m_Delta;
              nativeParallelHashMap[damage.m_Object] = damaged;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_DamagedData.HasComponent(damage.m_Object))
              {
                // ISSUE: reference to a compiler-generated field
                damaged = this.m_DamagedData[damage.m_Object];
                damaged.m_Damage += damage.m_Delta;
                nativeParallelHashMap.TryAdd(damage.m_Object, damaged);
              }
              else
                nativeParallelHashMap.TryAdd(damage.m_Object, new Damaged(damage.m_Delta));
            }
          }
        }
        if (nativeParallelHashMap.Count() == 0)
          return;
        NativeArray<Entity> keyArray = nativeParallelHashMap.GetKeyArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < keyArray.Length; ++index)
        {
          Entity entity = keyArray[index];
          Damaged component = nativeParallelHashMap[entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_DamagedData.HasComponent(entity))
          {
            if (math.any(component.m_Damage > 0.0f))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_DamagedData[entity] = component;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<BatchesUpdated>(entity, new BatchesUpdated());
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Damaged>(entity);
              // ISSUE: reference to a compiler-generated field
              if (this.m_VehicleData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<MaintenanceConsumer>(entity);
              }
            }
          }
          else if (math.any(component.m_Damage > 0.0f))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Damaged>(entity, component);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<BatchesUpdated>(entity, new BatchesUpdated());
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Damage> __Game_Objects_Damage_RO_ComponentTypeHandle;
      public ComponentLookup<Vehicle> __Game_Vehicles_Vehicle_RW_ComponentLookup;
      public ComponentLookup<Damaged> __Game_Objects_Damaged_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damage_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Damage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RW_ComponentLookup = state.GetComponentLookup<Vehicle>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RW_ComponentLookup = state.GetComponentLookup<Damaged>();
      }
    }
  }
}
