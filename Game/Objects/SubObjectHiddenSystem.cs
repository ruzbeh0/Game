// Decompiled with JetBrains decompiler
// Type: Game.Objects.SubObjectHiddenSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Common;
using Game.Creatures;
using Game.Tools;
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
  public class SubObjectHiddenSystem : GameSystemBase
  {
    private ModificationBarrier5 m_ModificationBarrier;
    private EntityQuery m_HiddenQuery;
    private EntityQuery m_TempQuery;
    private SubObjectHiddenSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_HiddenQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Hidden>(),
          ComponentType.ReadOnly<SubObject>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Hidden>(),
          ComponentType.ReadOnly<Object>(),
          ComponentType.ReadOnly<Owner>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Vehicle>(),
          ComponentType.ReadOnly<Creature>(),
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Object>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HiddenQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      NativeParallelHashMap<Entity, Entity> nativeParallelHashMap = new NativeParallelHashMap<Entity, Entity>(this.m_TempQuery.CalculateEntityCount() * 2, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SubObjectHiddenSystem.FillTempMapJob jobData1 = new SubObjectHiddenSystem.FillTempMapJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_TempMap = nativeParallelHashMap.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Object_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SubObjectHiddenSystem.HiddenSubObjectJob jobData2 = new SubObjectHiddenSystem.HiddenSubObjectJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_SubObjectType = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle,
        m_ObjectType = this.__TypeHandle.__Game_Objects_Object_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_VehicleType = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentTypeHandle,
        m_CreatureType = this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_TempMap = nativeParallelHashMap,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.ScheduleParallel<SubObjectHiddenSystem.FillTempMapJob>(this.m_TempQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      EntityQuery hiddenQuery = this.m_HiddenQuery;
      JobHandle dependsOn = jobHandle1;
      JobHandle jobHandle2 = jobData2.ScheduleParallel<SubObjectHiddenSystem.HiddenSubObjectJob>(hiddenQuery, dependsOn);
      nativeParallelHashMap.Dispose(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle2);
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
    public SubObjectHiddenSystem()
    {
    }

    [BurstCompile]
    private struct FillTempMapJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      public NativeParallelHashMap<Entity, Entity>.ParallelWriter m_TempMap;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray3 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Temp temp = nativeArray3[index];
          if (temp.m_Original != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_TempMap.TryAdd(temp.m_Original, entity);
          }
          if (nativeArray2.Length != 0)
          {
            Owner owner = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            if (owner.m_Owner != Entity.Null && !this.m_TempData.HasComponent(owner.m_Owner))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_TempMap.TryAdd(owner.m_Owner, entity);
            }
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

    [BurstCompile]
    private struct HiddenSubObjectJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<SubObject> m_SubObjectType;
      [ReadOnly]
      public ComponentTypeHandle<Object> m_ObjectType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Vehicle> m_VehicleType;
      [ReadOnly]
      public ComponentTypeHandle<Creature> m_CreatureType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjects;
      [ReadOnly]
      public NativeParallelHashMap<Entity, Entity> m_TempMap;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubObject> bufferAccessor = chunk.GetBufferAccessor<SubObject>(ref this.m_SubObjectType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Object>(ref this.m_ObjectType))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          if (nativeArray2.Length != 0)
          {
            if (bufferAccessor.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (chunk.Has<Vehicle>(ref this.m_VehicleType) || chunk.Has<Creature>(ref this.m_CreatureType) || chunk.Has<Building>(ref this.m_BuildingType))
              {
                for (int index = 0; index < bufferAccessor.Length; ++index)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.EnsureHidden(unfilteredChunkIndex, nativeArray1[index], bufferAccessor[index]);
                }
                return;
              }
              StackList<Entity> stackList = (StackList<Entity>) stackalloc Entity[nativeArray2.Length];
              for (int index = 0; index < nativeArray2.Length; ++index)
              {
                Entity entity = nativeArray1[index];
                Owner owner = nativeArray2[index];
                // ISSUE: reference to a compiler-generated field
                if (!this.m_HiddenData.HasComponent(owner.m_Owner))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_TempMap.ContainsKey(entity) && !this.m_TempMap.ContainsKey(owner.m_Owner))
                  {
                    stackList.AddNoResize(entity);
                    // ISSUE: reference to a compiler-generated method
                    this.EnsureVisible(unfilteredChunkIndex, entity, bufferAccessor[index]);
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated method
                  this.EnsureHidden(unfilteredChunkIndex, entity, bufferAccessor[index]);
                }
              }
              if (stackList.Length == 0)
                return;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<BatchesUpdated>(unfilteredChunkIndex, stackList.AsArray());
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Hidden>(unfilteredChunkIndex, stackList.AsArray());
              return;
            }
            StackList<Entity> stackList1 = (StackList<Entity>) stackalloc Entity[nativeArray2.Length];
            for (int index = 0; index < nativeArray2.Length; ++index)
            {
              Entity key = nativeArray1[index];
              Owner owner = nativeArray2[index];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!this.m_HiddenData.HasComponent(owner.m_Owner) && !this.m_TempMap.ContainsKey(key) && !this.m_TempMap.ContainsKey(owner.m_Owner))
                stackList1.AddNoResize(key);
            }
            if (stackList1.Length == 0)
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<BatchesUpdated>(unfilteredChunkIndex, stackList1.AsArray());
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Hidden>(unfilteredChunkIndex, stackList1.AsArray());
            return;
          }
        }
        for (int index = 0; index < bufferAccessor.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.EnsureHidden(unfilteredChunkIndex, nativeArray1[index], bufferAccessor[index]);
        }
      }

      private void EnsureHidden(int jobIndex, Entity parent, DynamicBuffer<SubObject> subObjects)
      {
        StackList<Entity> stackList = (StackList<Entity>) stackalloc Entity[subObjects.Length];
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          Owner componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_HiddenData.HasComponent(subObject) && !this.m_BuildingData.HasComponent(subObject) && this.m_OwnerData.TryGetComponent(subObject, out componentData) && !(componentData.m_Owner != parent))
          {
            stackList.AddNoResize(subObject);
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubObjects.HasBuffer(subObject))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.EnsureHidden(jobIndex, subObject, this.m_SubObjects[subObject]);
            }
          }
        }
        if (stackList.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Hidden>(jobIndex, stackList.AsArray());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, stackList.AsArray());
      }

      private void EnsureVisible(int jobIndex, Entity parent, DynamicBuffer<SubObject> subObjects)
      {
        StackList<Entity> stackList = (StackList<Entity>) stackalloc Entity[subObjects.Length];
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          Owner componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_HiddenData.HasComponent(subObject) && !this.m_BuildingData.HasComponent(subObject) && !this.m_TempMap.ContainsKey(subObject) && this.m_OwnerData.TryGetComponent(subObject, out componentData) && !(componentData.m_Owner != parent))
          {
            stackList.AddNoResize(subObject);
            DynamicBuffer<SubObject> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubObjects.TryGetBuffer(subObject, out bufferData))
            {
              // ISSUE: reference to a compiler-generated method
              this.EnsureVisible(jobIndex, subObject, bufferData);
            }
          }
        }
        if (stackList.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, stackList.AsArray());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Hidden>(jobIndex, stackList.AsArray());
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
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public BufferTypeHandle<SubObject> __Game_Objects_SubObject_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Object> __Game_Objects_Object_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Creature> __Game_Creatures_Creature_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubObject> __Game_Objects_SubObject_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Object_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Object>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Creature_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Creature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<SubObject>(true);
      }
    }
  }
}
