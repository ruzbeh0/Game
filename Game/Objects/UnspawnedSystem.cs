// Decompiled with JetBrains decompiler
// Type: Game.Objects.UnspawnedSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Creatures;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Objects
{
  [CompilerGenerated]
  public class UnspawnedSystem : GameSystemBase
  {
    private EntityQuery m_UpdateQuery;
    private ModificationEndBarrier m_ModificationEndBarrier;
    private UnspawnedSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationEndBarrier = this.World.GetOrCreateSystemManaged<ModificationEndBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadWrite<Vehicle>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<BatchesUpdated>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadWrite<Creature>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<BatchesUpdated>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UpdateQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Relative_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new UnspawnedSystem.UpdateUnspawnedJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_SubObjectType = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle,
        m_LayoutType = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle,
        m_PassengerType = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle,
        m_RelativeData = this.__TypeHandle.__Game_Objects_Relative_RO_ComponentLookup,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_Passengers = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferLookup,
        m_CommandBuffer = this.m_ModificationEndBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<UnspawnedSystem.UpdateUnspawnedJob>(this.m_UpdateQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationEndBarrier.AddJobHandleForProducer(producerJob);
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
    public UnspawnedSystem()
    {
    }

    [BurstCompile]
    private struct UpdateUnspawnedJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public BufferTypeHandle<SubObject> m_SubObjectType;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> m_LayoutType;
      [ReadOnly]
      public BufferTypeHandle<Passenger> m_PassengerType;
      [ReadOnly]
      public ComponentLookup<Relative> m_RelativeData;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<Passenger> m_Passengers;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubObject> bufferAccessor1 = chunk.GetBufferAccessor<SubObject>(ref this.m_SubObjectType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LayoutElement> bufferAccessor2 = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Passenger> bufferAccessor3 = chunk.GetBufferAccessor<Passenger>(ref this.m_PassengerType);
        // ISSUE: reference to a compiler-generated field
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        for (int index = 0; index < bufferAccessor1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckSubObjects(unfilteredChunkIndex, bufferAccessor1[index], isUnspawned);
        }
        for (int index = 0; index < bufferAccessor2.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckLayout(unfilteredChunkIndex, nativeArray[index], bufferAccessor2[index], isUnspawned);
        }
        for (int index = 0; index < bufferAccessor3.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckPassengers(unfilteredChunkIndex, bufferAccessor3[index], isUnspawned);
        }
      }

      private void CheckSubObjects(
        int jobIndex,
        DynamicBuffer<SubObject> subObjects,
        bool isUnspawned)
      {
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_RelativeData.HasComponent(subObject) && this.m_UnspawnedData.HasComponent(subObject) != isUnspawned)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateUnspawned(jobIndex, subObject, isUnspawned);
            DynamicBuffer<SubObject> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubObjects.TryGetBuffer(subObject, out bufferData))
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckSubObjects(jobIndex, bufferData, isUnspawned);
            }
          }
        }
      }

      private void CheckLayout(
        int jobIndex,
        Entity entity,
        DynamicBuffer<LayoutElement> layout,
        bool isUnspawned)
      {
        for (int index = 0; index < layout.Length; ++index)
        {
          Entity vehicle = layout[index].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          if (!(vehicle == entity) && this.m_UnspawnedData.HasComponent(vehicle) != isUnspawned)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateUnspawned(jobIndex, vehicle, isUnspawned);
            DynamicBuffer<SubObject> bufferData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubObjects.TryGetBuffer(vehicle, out bufferData1))
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckSubObjects(jobIndex, bufferData1, isUnspawned);
            }
            DynamicBuffer<Passenger> bufferData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Passengers.TryGetBuffer(vehicle, out bufferData2))
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckPassengers(jobIndex, bufferData2, isUnspawned);
            }
          }
        }
      }

      private void CheckPassengers(
        int jobIndex,
        DynamicBuffer<Passenger> passengers,
        bool isUnspawned)
      {
        for (int index = 0; index < passengers.Length; ++index)
        {
          Entity passenger = passengers[index].m_Passenger;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_RelativeData.HasComponent(passenger) && this.m_UnspawnedData.HasComponent(passenger) != isUnspawned)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateUnspawned(jobIndex, passenger, isUnspawned);
            DynamicBuffer<SubObject> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubObjects.TryGetBuffer(passenger, out bufferData))
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckSubObjects(jobIndex, bufferData, isUnspawned);
            }
          }
        }
      }

      private void UpdateUnspawned(int jobIndex, Entity entity, bool isUnspawned)
      {
        if (isUnspawned)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Unspawned>(jobIndex, entity, new Unspawned());
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Unspawned>(jobIndex, entity);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_UpdatedData.HasComponent(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, entity, new BatchesUpdated());
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
      public ComponentTypeHandle<Unspawned> __Game_Objects_Unspawned_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubObject> __Game_Objects_SubObject_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Passenger> __Game_Vehicles_Passenger_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Relative> __Game_Objects_Relative_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Passenger> __Game_Vehicles_Passenger_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RO_BufferTypeHandle = state.GetBufferTypeHandle<Passenger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Relative_RO_ComponentLookup = state.GetComponentLookup<Relative>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RO_BufferLookup = state.GetBufferLookup<Passenger>(true);
      }
    }
  }
}
