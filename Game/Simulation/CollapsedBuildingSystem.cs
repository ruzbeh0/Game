// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CollapsedBuildingSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CollapsedBuildingSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 64;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_CollapsedQuery;
    private EntityArchetype m_RescueRequestArchetype;
    private CollapsedBuildingSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CollapsedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Destroyed>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Extension>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_RescueRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<FireRescueRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CollapsedQuery);
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Area_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_FireRescueRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Extension_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_RescueTarget_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CollapsedBuildingSystem.CollapsedBuildingJob jobData = new CollapsedBuildingSystem.CollapsedBuildingJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_RescueTargetType = this.__TypeHandle.__Game_Buildings_RescueTarget_RO_ComponentTypeHandle,
        m_ServiceUpgradeType = this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentTypeHandle,
        m_ExtensionType = this.__TypeHandle.__Game_Buildings_Extension_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_AttachedType = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_DestroyedType = this.__TypeHandle.__Game_Common_Destroyed_RW_ComponentTypeHandle,
        m_FireRescueRequestData = this.__TypeHandle.__Game_Simulation_FireRescueRequest_RO_ComponentLookup,
        m_AreaData = this.__TypeHandle.__Game_Areas_Area_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_RescueRequestArchetype = this.m_RescueRequestArchetype,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<CollapsedBuildingSystem.CollapsedBuildingJob>(this.m_CollapsedQuery, this.Dependency);
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
    public CollapsedBuildingSystem()
    {
    }

    [BurstCompile]
    private struct CollapsedBuildingJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<RescueTarget> m_RescueTargetType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.ServiceUpgrade> m_ServiceUpgradeType;
      [ReadOnly]
      public ComponentTypeHandle<Extension> m_ExtensionType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Attached> m_AttachedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Destroyed> m_DestroyedType;
      [ReadOnly]
      public ComponentLookup<FireRescueRequest> m_FireRescueRequestData;
      [ReadOnly]
      public ComponentLookup<Area> m_AreaData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public EntityArchetype m_RescueRequestArchetype;
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
        NativeArray<Destroyed> nativeArray2 = chunk.GetNativeArray<Destroyed>(ref this.m_DestroyedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<RescueTarget> nativeArray3 = chunk.GetNativeArray<RescueTarget>(ref this.m_RescueTargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray4 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<Attached>(ref this.m_AttachedType);
        // ISSUE: reference to a compiler-generated field
        bool flag2 = chunk.Has<Game.Buildings.ServiceUpgrade>(ref this.m_ServiceUpgradeType);
        // ISSUE: reference to a compiler-generated field
        bool flag3 = chunk.Has<Extension>(ref this.m_ExtensionType);
        if (nativeArray3.Length != 0)
        {
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            Destroyed destroyed = nativeArray2[index];
            Entity entity = nativeArray1[index];
            if ((double) destroyed.m_Cleared < 1.0)
            {
              RescueTarget rescueTarget = nativeArray3[index];
              // ISSUE: reference to a compiler-generated method
              this.RequestRescueIfNeeded(unfilteredChunkIndex, entity, rescueTarget);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<RescueTarget>(unfilteredChunkIndex, entity);
            }
          }
        }
        else
        {
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            ref Destroyed local = ref nativeArray2.ElementAt<Destroyed>(index);
            PrefabRef prefabRef = nativeArray5[index];
            bool c = false;
            Game.Prefabs.BuildingData componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabBuildingData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
            {
              c = (componentData1.m_Flags & Game.Prefabs.BuildingFlags.RequireRoad) > (Game.Prefabs.BuildingFlags) 0;
              Owner owner;
              PrefabRef componentData2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (CollectionUtils.TryGet<Owner>(nativeArray4, index, out owner) && this.m_PrefabRefData.TryGetComponent(owner.m_Owner, out componentData2) && this.m_PrefabBuildingData.TryGetComponent(componentData2.m_Prefab, out componentData1))
                c |= (componentData1.m_Flags & Game.Prefabs.BuildingFlags.RequireRoad) > (Game.Prefabs.BuildingFlags) 0;
            }
            if ((double) local.m_Cleared < 0.0)
            {
              Entity e = nativeArray1[index];
              local.m_Cleared += 1.06666672f;
              if ((double) local.m_Cleared >= 0.0)
              {
                local.m_Cleared = math.select(1f, 0.0f, c);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<InterpolatedTransform>(unfilteredChunkIndex, e);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Updated>(unfilteredChunkIndex, e, new Updated());
              }
            }
            else if ((double) local.m_Cleared < 1.0 && !flag3)
            {
              if (c)
              {
                Entity entity = nativeArray1[index];
                RescueTarget rescueTarget = new RescueTarget();
                // ISSUE: reference to a compiler-generated method
                this.RequestRescueIfNeeded(unfilteredChunkIndex, entity, rescueTarget);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<RescueTarget>(unfilteredChunkIndex, entity, rescueTarget);
              }
              else
                local.m_Cleared = 1f;
            }
          }
        }
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          if ((double) nativeArray2[index1].m_Cleared >= 1.0 && !flag2 && !flag3)
          {
            Entity entity = nativeArray1[index1];
            ObjectGeometryData componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabObjectGeometryData.TryGetComponent(nativeArray5[index1].m_Prefab, out componentData))
            {
              if ((componentData.m_Flags & Game.Objects.GeometryFlags.Overridable) != Game.Objects.GeometryFlags.None && !flag1)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, entity, new Deleted());
              }
              else
              {
                Owner owner;
                // ISSUE: reference to a compiler-generated field
                if (CollectionUtils.TryGet<Owner>(nativeArray4, index1, out owner) && this.m_AreaData.HasComponent(owner.m_Owner))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, entity, new Deleted());
                  DynamicBuffer<Game.Areas.SubArea> bufferData;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_SubAreas.TryGetBuffer(entity, out bufferData))
                  {
                    for (int index2 = 0; index2 < bufferData.Length; ++index2)
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddComponent<Updated>(unfilteredChunkIndex, bufferData[index2].m_Area, new Updated());
                    }
                  }
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, entity, new Deleted());
            }
          }
        }
      }

      private void RequestRescueIfNeeded(int jobIndex, Entity entity, RescueTarget rescueTarget)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_FireRescueRequestData.HasComponent(rescueTarget.m_Request))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_RescueRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<FireRescueRequest>(jobIndex, entity1, new FireRescueRequest(entity, 10f, FireRescueRequestType.Disaster));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(4U));
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
      public ComponentTypeHandle<RescueTarget> __Game_Buildings_RescueTarget_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.ServiceUpgrade> __Game_Buildings_ServiceUpgrade_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Extension> __Game_Buildings_Extension_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Attached> __Game_Objects_Attached_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Destroyed> __Game_Common_Destroyed_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<FireRescueRequest> __Game_Simulation_FireRescueRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Area> __Game_Areas_Area_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_RescueTarget_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RescueTarget>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ServiceUpgrade_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.ServiceUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Extension_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Extension>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Destroyed>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_FireRescueRequest_RO_ComponentLookup = state.GetComponentLookup<FireRescueRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Area_RO_ComponentLookup = state.GetComponentLookup<Area>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
      }
    }
  }
}
