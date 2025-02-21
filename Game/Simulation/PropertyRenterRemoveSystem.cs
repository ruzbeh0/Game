// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PropertyRenterRemoveSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class PropertyRenterRemoveSystem : GameSystemBase
  {
    private EntityQuery m_RenterGroup;
    private EntityArchetype m_RentEventArchetype;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private PropertyRenterRemoveSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_RenterGroup = this.GetEntityQuery(ComponentType.ReadOnly<PropertyRenter>(), ComponentType.ReadOnly<UpdateFrame>());
      // ISSUE: reference to a compiler-generated field
      this.m_RentEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<RentersUpdated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_RenterGroup);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint frameWithInterval = SimulationUtils.GetUpdateFrameWithInterval(this.m_SimulationSystem.frameIndex, (uint) this.GetUpdateInterval(SystemUpdatePhase.GameSimulation), 16);
      NativeQueue<PropertyRenterRemoveSystem.RemoveData> nativeQueue = new NativeQueue<PropertyRenterRemoveSystem.RemoveData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PropertyRenterRemoveSystem.UpdateRentersJob jobData1 = new PropertyRenterRemoveSystem.UpdateRentersJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PropertyRenterType = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_Buildings = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PropertyDatas = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_Renters = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_UpdateFrameIndex = frameWithInterval,
        m_RemoveData = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PropertyRenterRemoveSystem.RemoveRentersJob jobData2 = new PropertyRenterRemoveSystem.RemoveRentersJob()
      {
        m_RentEventArchetype = this.m_RentEventArchetype,
        m_Renters = this.__TypeHandle.__Game_Buildings_Renter_RW_BufferLookup,
        m_RemoveData = nativeQueue,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<PropertyRenterRemoveSystem.UpdateRentersJob>(this.m_RenterGroup, this.Dependency);
      this.Dependency = jobData2.Schedule<PropertyRenterRemoveSystem.RemoveRentersJob>(this.Dependency);
      nativeQueue.Dispose(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
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
    public PropertyRenterRemoveSystem()
    {
    }

    [BurstCompile]
    private struct UpdateRentersJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_PropertyRenterType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentLookup<Building> m_Buildings;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_PropertyDatas;
      [ReadOnly]
      public BufferLookup<Renter> m_Renters;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      public uint m_UpdateFrameIndex;
      public NativeQueue<PropertyRenterRemoveSystem.RemoveData>.ParallelWriter m_RemoveData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((int) chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index != (int) this.m_UpdateFrameIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PropertyRenter> nativeArray2 = chunk.GetNativeArray<PropertyRenter>(ref this.m_PropertyRenterType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity property = nativeArray2[index].m_Property;
          // ISSUE: variable of a compiler-generated type
          PropertyRenterRemoveSystem.RemoveData removeData1;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Buildings.HasComponent(property))
          {
            // ISSUE: reference to a compiler-generated field
            ref NativeQueue<PropertyRenterRemoveSystem.RemoveData>.ParallelWriter local = ref this.m_RemoveData;
            // ISSUE: object of a compiler-generated type is created
            removeData1 = new PropertyRenterRemoveSystem.RemoveData();
            // ISSUE: reference to a compiler-generated field
            removeData1.m_Renter = nativeArray1[index];
            // ISSUE: variable of a compiler-generated type
            PropertyRenterRemoveSystem.RemoveData removeData2 = removeData1;
            local.Enqueue(removeData2);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            Entity prefab = this.m_Prefabs[property].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PropertyDatas.HasComponent(prefab) && this.m_Renters.HasBuffer(property))
            {
              // ISSUE: reference to a compiler-generated field
              BuildingPropertyData propertyData = this.m_PropertyDatas[prefab];
              // ISSUE: reference to a compiler-generated field
              if (this.m_Renters[property].Length > propertyData.CountProperties())
              {
                // ISSUE: reference to a compiler-generated field
                ref NativeQueue<PropertyRenterRemoveSystem.RemoveData>.ParallelWriter local = ref this.m_RemoveData;
                // ISSUE: object of a compiler-generated type is created
                removeData1 = new PropertyRenterRemoveSystem.RemoveData();
                // ISSUE: reference to a compiler-generated field
                removeData1.m_Property = property;
                // ISSUE: reference to a compiler-generated field
                removeData1.m_Renter = nativeArray1[index];
                // ISSUE: variable of a compiler-generated type
                PropertyRenterRemoveSystem.RemoveData removeData3 = removeData1;
                local.Enqueue(removeData3);
              }
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

    private struct RemoveData
    {
      public Entity m_Property;
      public Entity m_Renter;
    }

    [BurstCompile]
    private struct RemoveRentersJob : IJob
    {
      [ReadOnly]
      public EntityArchetype m_RentEventArchetype;
      public BufferLookup<Renter> m_Renters;
      public NativeQueue<PropertyRenterRemoveSystem.RemoveData> m_RemoveData;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        NativeHashSet<Entity> nativeHashSet = new NativeHashSet<Entity>();
        // ISSUE: variable of a compiler-generated type
        PropertyRenterRemoveSystem.RemoveData removeData;
        // ISSUE: reference to a compiler-generated field
        while (this.m_RemoveData.TryDequeue(out removeData))
        {
          // ISSUE: reference to a compiler-generated field
          if (removeData.m_Property != Entity.Null)
          {
            DynamicBuffer<Renter> bufferData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Renters.TryGetBuffer(removeData.m_Property, out bufferData))
            {
              for (int index = 0; index < bufferData.Length; ++index)
              {
                // ISSUE: reference to a compiler-generated field
                if (bufferData[index].m_Renter == removeData.m_Renter)
                {
                  bufferData.RemoveAt(index);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<PropertyRenter>(removeData.m_Renter);
                  break;
                }
              }
            }
            if (!nativeHashSet.IsCreated)
              nativeHashSet = new NativeHashSet<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            // ISSUE: reference to a compiler-generated field
            if (nativeHashSet.Add(removeData.m_Property))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<RentersUpdated>(this.m_CommandBuffer.CreateEntity(this.m_RentEventArchetype), new RentersUpdated(removeData.m_Property));
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            Debug.Log((object) string.Format("{0} removed renter since building does not exist", (object) removeData.m_Renter.Index));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<PropertyRenter>(removeData.m_Renter);
          }
        }
        if (!nativeHashSet.IsCreated)
          return;
        nativeHashSet.Dispose();
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      public BufferLookup<Renter> __Game_Buildings_Renter_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RW_BufferLookup = state.GetBufferLookup<Renter>();
      }
    }
  }
}
