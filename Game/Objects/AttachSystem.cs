// Decompiled with JetBrains decompiler
// Type: Game.Objects.AttachSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
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
  public class AttachSystem : GameSystemBase
  {
    private ModificationBarrier3 m_ModificationBarrier;
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_ObjectQuery;
    private EntityQuery m_TempQuery;
    private EntityQuery m_EconomyParameterQuery;
    private AttachSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier3>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Object>(),
          ComponentType.ReadOnly<Attached>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<SubObject>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Edge>(),
          ComponentType.ReadOnly<Game.Net.Node>(),
          ComponentType.ReadOnly<Placeholder>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ObjectQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle1;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync1 = this.m_ObjectQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle1);
      JobHandle jobHandle1 = JobHandle.CombineDependencies(outJobHandle1, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_TempQuery.IsEmptyIgnoreFilter)
      {
        JobHandle outJobHandle2;
        // ISSUE: reference to a compiler-generated field
        NativeList<ArchetypeChunk> archetypeChunkListAsync2 = this.m_TempQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Attached_RW_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Elevation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        JobHandle inputDeps = new AttachSystem.FindAttachedParentsJob()
        {
          m_AttachedChunks = archetypeChunkListAsync1.AsDeferredJobArray(),
          m_ParentChunks = archetypeChunkListAsync2,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
          m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_NodeType = this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle,
          m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
          m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
          m_ElevationType = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentTypeHandle,
          m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_PlaceableObjectData = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup,
          m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
          m_NetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
          m_PlaceholderBuildingData = this.__TypeHandle.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup,
          m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
          m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
          m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
          m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
          m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RW_ComponentLookup
        }.Schedule<AttachSystem.FindAttachedParentsJob, ArchetypeChunk>(archetypeChunkListAsync1, 1, JobHandle.CombineDependencies(jobHandle1, outJobHandle2));
        archetypeChunkListAsync2.Dispose(inputDeps);
        jobHandle1 = inputDeps;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attachment_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Placeholder_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Recent_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle2 = new AttachSystem.UpdateAttachedReferencesJob()
      {
        m_AttachedChunks = archetypeChunkListAsync1,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_RecentData = this.__TypeHandle.__Game_Tools_Recent_RO_ComponentLookup,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PlaceholderData = this.__TypeHandle.__Game_Objects_Placeholder_RO_ComponentLookup,
        m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_AttachedType = this.__TypeHandle.__Game_Objects_Attached_RW_ComponentTypeHandle,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RW_BufferLookup,
        m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RW_ComponentLookup,
        m_EconomyParameterData = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<AttachSystem.UpdateAttachedReferencesJob>(jobHandle1);
      archetypeChunkListAsync1.Dispose(jobHandle2);
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
    public AttachSystem()
    {
    }

    public struct RemovedAttached
    {
      public Entity m_Entity;
      public Entity m_Parent;
    }

    [BurstCompile]
    private struct FindAttachedParentsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_AttachedChunks;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_ParentChunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Node> m_NodeType;
      [ReadOnly]
      public ComponentTypeHandle<Edge> m_EdgeType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Elevation> m_ElevationType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> m_PlaceableObjectData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_NetGeometryData;
      [ReadOnly]
      public ComponentLookup<PlaceholderBuildingData> m_PlaceholderBuildingData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_Edges;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Attached> m_AttachedData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ArchetypeChunk attachedChunk = this.m_AttachedChunks[index];
        // ISSUE: reference to a compiler-generated field
        if (attachedChunk.Has<Deleted>(ref this.m_DeletedType))
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = attachedChunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray2 = attachedChunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray3 = attachedChunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = attachedChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray5 = attachedChunk.GetNativeArray<Temp>(ref this.m_TempType);
        bool isTemp = nativeArray5.Length != 0;
        bool flag = nativeArray3.Length != 0;
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity entity1 = nativeArray1[index1];
          // ISSUE: reference to a compiler-generated field
          Attached attached = this.m_AttachedData[entity1];
          if (attached.m_Parent != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PlaceholderBuildingData.HasComponent(attached.m_Parent))
            {
              // ISSUE: reference to a compiler-generated method
              attached.m_Parent = this.FindParent(attached.m_Parent, nativeArray2[index1], isTemp);
              // ISSUE: reference to a compiler-generated field
              this.m_AttachedData[entity1] = attached;
            }
          }
          else
          {
            Attached componentData1 = new Attached();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (isTemp && this.m_AttachedData.TryGetComponent(nativeArray5[index1].m_Original, out componentData1) && !this.m_HiddenData.HasComponent(componentData1.m_Parent))
            {
              attached.m_Parent = componentData1.m_Parent;
              attached.m_CurvePosition = componentData1.m_CurvePosition;
              // ISSUE: reference to a compiler-generated field
              this.m_AttachedData[entity1] = attached;
            }
            else
            {
              PrefabRef prefabRef = nativeArray4[index1];
              PlaceableObjectData placeableObjectData = new PlaceableObjectData();
              // ISSUE: reference to a compiler-generated field
              if (this.m_PlaceableObjectData.HasComponent(prefabRef.m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                placeableObjectData = this.m_PlaceableObjectData[prefabRef.m_Prefab];
                if ((placeableObjectData.m_Flags & (PlacementFlags.RoadSide | PlacementFlags.OwnerSide | PlacementFlags.Shoreline | PlacementFlags.Floating)) != PlacementFlags.None)
                  continue;
              }
              ObjectGeometryData componentData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
              {
                if ((componentData2.m_Flags & GeometryFlags.OptionalAttach) != GeometryFlags.None)
                  componentData1 = new Attached();
                Entity entity2 = entity1;
                Entity owner = Entity.Null;
                if (flag)
                {
                  owner = nativeArray3[index1].m_Owner;
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_BuildingData.HasComponent(entity1) && (componentData2.m_Flags & GeometryFlags.OptionalAttach) == GeometryFlags.None)
                  {
                    entity2 = nativeArray3[index1].m_Owner;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    while (this.m_OwnerData.HasComponent(entity2) && !this.m_BuildingData.HasComponent(entity2))
                    {
                      // ISSUE: reference to a compiler-generated field
                      entity2 = this.m_OwnerData[entity2].m_Owner;
                    }
                  }
                }
                Transform transform = nativeArray2[index1];
                // ISSUE: reference to a compiler-generated method
                if (this.FindParent(ref attached, componentData1, transform, isTemp, entity2, owner, componentData2, placeableObjectData.m_Flags))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_AttachedData[entity1] = attached;
                }
              }
            }
          }
        }
      }

      private Entity FindParent(Entity prefab, Transform transform, bool isTemp)
      {
        float num1 = float.MaxValue;
        Entity parent = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_ParentChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk parentChunk = this.m_ParentChunks[index1];
          // ISSUE: reference to a compiler-generated field
          if (isTemp == parentChunk.Has<Temp>(ref this.m_TempType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray1 = parentChunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Transform> nativeArray2 = parentChunk.GetNativeArray<Transform>(ref this.m_TransformType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray3 = parentChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
            {
              if (!(nativeArray3[index2].m_Prefab != prefab))
              {
                Transform transform1 = nativeArray2[index2];
                float num2 = math.distance(transform.m_Position, transform1.m_Position);
                if ((double) num2 < (double) num1)
                {
                  num1 = num2;
                  parent = nativeArray1[index2];
                }
              }
            }
          }
        }
        return parent;
      }

      private bool FindParent(
        ref Attached attached,
        Attached originalAttached,
        Transform transform,
        bool isTemp,
        Entity topLevelOwner,
        Entity ignoreParent,
        ObjectGeometryData objectGeometryData,
        PlacementFlags placementFlags)
      {
        Bounds3 bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, objectGeometryData);
        float2 x1 = new float2(0.0f, 10f);
        bool parent = false;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_ParentChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk parentChunk = this.m_ParentChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Temp> nativeArray1 = parentChunk.GetNativeArray<Temp>(ref this.m_TempType);
          bool flag = nativeArray1.Length != 0;
          if (isTemp == flag)
          {
            if ((placementFlags & PlacementFlags.RoadNode) != PlacementFlags.None)
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<Game.Net.Node> nativeArray2 = parentChunk.GetNativeArray<Game.Net.Node>(ref this.m_NodeType);
              if (nativeArray2.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                NativeArray<Entity> nativeArray3 = parentChunk.GetNativeArray(this.m_EntityType);
                // ISSUE: reference to a compiler-generated field
                NativeArray<Game.Net.Elevation> nativeArray4 = parentChunk.GetNativeArray<Game.Net.Elevation>(ref this.m_ElevationType);
                // ISSUE: reference to a compiler-generated field
                NativeArray<PrefabRef> nativeArray5 = parentChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
label_28:
                for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
                {
                  Entity node1 = nativeArray3[index2];
                  Temp temp;
                  if (!(node1 == ignoreParent) && (!CollectionUtils.TryGet<Temp>(nativeArray1, index2, out temp) || (temp.m_Flags & TempFlags.Delete) == (TempFlags) 0))
                  {
                    PrefabRef prefabRef = nativeArray5[index2];
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_NetGeometryData.HasComponent(prefabRef.m_Prefab))
                    {
                      // ISSUE: reference to a compiler-generated field
                      NetGeometryData netGeometryData = this.m_NetGeometryData[prefabRef.m_Prefab];
                      int num1 = 0;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      Game.Net.EdgeIterator edgeIterator = new Game.Net.EdgeIterator(Entity.Null, node1, this.m_Edges, this.m_EdgeData, this.m_TempData, this.m_HiddenData);
                      EdgeIteratorValue edgeIteratorValue;
                      while (edgeIterator.GetNext(out edgeIteratorValue))
                      {
                        Entity edge1 = edgeIteratorValue.m_Edge;
                        if (edge1 == ignoreParent)
                        {
                          // ISSUE: reference to a compiler-generated field
                          Edge edge2 = this.m_EdgeData[edge1];
                          if (edge2.m_Start == node1 || edge2.m_End == node1)
                            goto label_28;
                        }
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        if ((this.m_NetGeometryData[this.m_PrefabRefData[edge1].m_Prefab].m_MergeLayers & netGeometryData.m_MergeLayers) != Layer.None)
                          ++num1;
                      }
                      if ((placementFlags & (PlacementFlags.RoadNode | PlacementFlags.RoadEdge)) == PlacementFlags.RoadNode || num1 >= 3)
                      {
                        Game.Net.Node node2 = nativeArray2[index2];
                        Entity entity = node1;
                        if ((objectGeometryData.m_Flags & GeometryFlags.OptionalAttach) == GeometryFlags.None)
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          while (this.m_OwnerData.HasComponent(entity) && !this.m_BuildingData.HasComponent(entity))
                          {
                            // ISSUE: reference to a compiler-generated field
                            entity = this.m_OwnerData[entity].m_Owner;
                          }
                        }
                        float num2 = math.select(x1.x, x1.y, topLevelOwner == entity);
                        float num3 = netGeometryData.m_DefaultWidth * 0.5f;
                        Bounds3 bounds1;
                        bounds1.min = node2.m_Position - new float3(num3, -netGeometryData.m_DefaultHeightRange.min, num3);
                        bounds1.max = node2.m_Position + new float3(num3, netGeometryData.m_DefaultHeightRange.max, num3);
                        if ((double) num2 > 0.0)
                        {
                          bounds1.min -= num2;
                          bounds1.max += num2;
                        }
                        if ((objectGeometryData.m_Flags & GeometryFlags.BaseCollision) != GeometryFlags.None)
                        {
                          bounds.y = bounds1.y;
                          transform.m_Position.y = node2.m_Position.y;
                          if (nativeArray4.Length != 0)
                          {
                            float num4 = math.csum(nativeArray4[index2].m_Elevation) * 0.5f;
                            bounds.y -= num4;
                            transform.m_Position.y -= num4;
                          }
                        }
                        if (MathUtils.Intersect(bounds1, bounds))
                        {
                          float x2 = math.distance(node2.m_Position, transform.m_Position) - num3;
                          float y = x2 + math.clamp(x2, -num3, 0.0f);
                          if ((double) y < (double) num2)
                          {
                            x1 = math.min(x1, (float2) y);
                            attached.m_Parent = node1;
                            attached.m_CurvePosition = 0.0f;
                            parent = true;
                          }
                        }
                        if (flag && originalAttached.m_Parent != Entity.Null && attached.m_Parent == Entity.Null && temp.m_Original == originalAttached.m_Parent)
                        {
                          attached.m_Parent = node1;
                          attached.m_CurvePosition = 0.0f;
                          parent = true;
                        }
                      }
                    }
                  }
                }
              }
            }
            if ((placementFlags & (PlacementFlags.RoadNode | PlacementFlags.RoadEdge)) != PlacementFlags.RoadNode)
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<Edge> nativeArray6 = parentChunk.GetNativeArray<Edge>(ref this.m_EdgeType);
              if (nativeArray6.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                NativeArray<Entity> nativeArray7 = parentChunk.GetNativeArray(this.m_EntityType);
                // ISSUE: reference to a compiler-generated field
                NativeArray<Curve> nativeArray8 = parentChunk.GetNativeArray<Curve>(ref this.m_CurveType);
                // ISSUE: reference to a compiler-generated field
                NativeArray<Game.Net.Elevation> nativeArray9 = parentChunk.GetNativeArray<Game.Net.Elevation>(ref this.m_ElevationType);
                // ISSUE: reference to a compiler-generated field
                NativeArray<PrefabRef> nativeArray10 = parentChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
                for (int index3 = 0; index3 < nativeArray6.Length; ++index3)
                {
                  Entity entity1 = nativeArray7[index3];
                  if (!(entity1 == ignoreParent))
                  {
                    Edge edge = nativeArray6[index3];
                    Temp temp;
                    if (!(edge.m_Start == ignoreParent) && !(edge.m_End == ignoreParent) && (!CollectionUtils.TryGet<Temp>(nativeArray1, index3, out temp) || (temp.m_Flags & TempFlags.Delete) == (TempFlags) 0))
                    {
                      PrefabRef prefabRef = nativeArray10[index3];
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_NetGeometryData.HasComponent(prefabRef.m_Prefab))
                      {
                        Curve curve = nativeArray8[index3];
                        // ISSUE: reference to a compiler-generated field
                        NetGeometryData netGeometryData = this.m_NetGeometryData[prefabRef.m_Prefab];
                        Entity entity2 = entity1;
                        if ((objectGeometryData.m_Flags & GeometryFlags.OptionalAttach) == GeometryFlags.None)
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          while (this.m_OwnerData.HasComponent(entity2) && !this.m_BuildingData.HasComponent(entity2))
                          {
                            // ISSUE: reference to a compiler-generated field
                            entity2 = this.m_OwnerData[entity2].m_Owner;
                          }
                        }
                        float num5 = math.select(x1.x, x1.y, topLevelOwner == entity2);
                        float num6 = netGeometryData.m_DefaultWidth * 0.5f;
                        Bounds3 bounds1 = MathUtils.Bounds(curve.m_Bezier);
                        bounds1.min -= new float3(num6, -netGeometryData.m_DefaultHeightRange.min, num6);
                        bounds1.max += new float3(num6, netGeometryData.m_DefaultHeightRange.max, num6);
                        if ((double) num5 > 0.0)
                        {
                          bounds1.min -= num5;
                          bounds1.max += num5;
                        }
                        if ((placementFlags & PlacementFlags.Waterway) != PlacementFlags.None)
                        {
                          if ((netGeometryData.m_Flags & Game.Net.GeometryFlags.OnWater) != (Game.Net.GeometryFlags) 0)
                          {
                            if (MathUtils.Intersect(bounds1, bounds))
                            {
                              float t;
                              float y = MathUtils.Distance(curve.m_Bezier.xz, transform.m_Position.xz, out t) - num6;
                              if ((double) y < (double) num5)
                              {
                                x1 = math.min(x1, (float2) y);
                                attached.m_Parent = entity1;
                                attached.m_CurvePosition = t;
                                parent = true;
                              }
                            }
                          }
                          else
                            continue;
                        }
                        else if ((netGeometryData.m_Flags & Game.Net.GeometryFlags.OnWater) == (Game.Net.GeometryFlags) 0)
                        {
                          if ((objectGeometryData.m_Flags & GeometryFlags.BaseCollision) != GeometryFlags.None)
                          {
                            bounds.y = bounds1.y;
                            float x3 = 0.0f;
                            if (nativeArray9.Length != 0)
                            {
                              x3 = math.csum(nativeArray9[index3].m_Elevation) * 0.5f;
                              bounds.y -= x3;
                            }
                            if (MathUtils.Intersect(bounds1, bounds))
                            {
                              float t;
                              float y1 = MathUtils.Distance(curve.m_Bezier.xz, transform.m_Position.xz, out t);
                              float y2 = math.length(new float2(x3, y1)) - num6;
                              if ((double) y2 < (double) num5)
                              {
                                x1 = math.min(x1, (float2) y2);
                                attached.m_Parent = entity1;
                                attached.m_CurvePosition = t;
                                parent = true;
                              }
                            }
                          }
                          else if (MathUtils.Intersect(bounds1, bounds))
                          {
                            float t;
                            float y = MathUtils.Distance(curve.m_Bezier, transform.m_Position, out t) - num6;
                            if ((double) y < (double) num5)
                            {
                              x1 = math.min(x1, (float2) y);
                              attached.m_Parent = entity1;
                              attached.m_CurvePosition = t;
                              parent = true;
                            }
                          }
                        }
                        else
                          continue;
                        if (flag && originalAttached.m_Parent != Entity.Null && attached.m_Parent == Entity.Null && temp.m_Original == originalAttached.m_Parent)
                        {
                          float t;
                          double num7 = (double) MathUtils.Distance(curve.m_Bezier.xz, transform.m_Position.xz, out t);
                          attached.m_Parent = entity1;
                          attached.m_CurvePosition = t;
                          parent = true;
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
        return parent;
      }
    }

    [BurstCompile]
    private struct UpdateAttachedReferencesJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_AttachedChunks;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Recent> m_RecentData;
      [ReadOnly]
      public ComponentLookup<Placeholder> m_PlaceholderData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      public ComponentTypeHandle<Attached> m_AttachedType;
      [ReadOnly]
      public EconomyParameterData m_EconomyParameterData;
      public BufferLookup<SubObject> m_SubObjects;
      public ComponentLookup<Attachment> m_AttachmentData;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        NativeHashMap<Entity, Entity> newAttachments = new NativeHashMap<Entity, Entity>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_AttachedChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.Execute(this.m_AttachedChunks[index], newAttachments);
        }
        if (newAttachments.Count != 0)
        {
          NativeArray<Entity> keyArray = newAttachments.GetKeyArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
          for (int index = 0; index < keyArray.Length; ++index)
          {
            Entity entity = keyArray[index];
            Entity attached = newAttachments[entity];
            if (attached != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_AttachmentData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_AttachmentData[entity] = new Attachment(attached);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Attachment>(entity, new Attachment(attached));
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_AttachmentData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<Attachment>(entity);
              }
            }
          }
          keyArray.Dispose();
        }
        newAttachments.Dispose();
      }

      private void Execute(ArchetypeChunk chunk, NativeHashMap<Entity, Entity> newAttachments)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Attached> nativeArray2 = chunk.GetNativeArray<Attached>(ref this.m_AttachedType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            ref Attached local = ref nativeArray2.ElementAt<Attached>(index);
            DynamicBuffer<SubObject> bufferData1;
            // ISSUE: reference to a compiler-generated field
            if (local.m_OldParent != Entity.Null && local.m_OldParent != local.m_Parent && this.m_SubObjects.TryGetBuffer(local.m_OldParent, out bufferData1))
              CollectionUtils.RemoveValue<SubObject>(bufferData1, new SubObject(entity));
            local.m_OldParent = Entity.Null;
            if (local.m_Parent == entity)
            {
              local.m_Parent = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Attached>(entity);
            }
            else
            {
              DynamicBuffer<SubObject> bufferData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SubObjects.TryGetBuffer(local.m_Parent, out bufferData2))
                CollectionUtils.RemoveValue<SubObject>(bufferData2, new SubObject(entity));
              // ISSUE: reference to a compiler-generated field
              if (this.m_PlaceholderData.HasComponent(local.m_Parent))
              {
                Entity attached;
                if (newAttachments.TryGetValue(local.m_Parent, out attached))
                {
                  if (attached == entity)
                  {
                    newAttachments.Remove(local.m_Parent);
                    newAttachments.TryAdd(local.m_Parent, Entity.Null);
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_AttachmentData.HasComponent(local.m_Parent))
                  {
                    // ISSUE: reference to a compiler-generated field
                    attached = this.m_AttachmentData[local.m_Parent].m_Attached;
                    if (attached == entity)
                      newAttachments.TryAdd(local.m_Parent, Entity.Null);
                  }
                }
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Temp> nativeArray3 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
          if (nativeArray3.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            bool flag1 = chunk.Has<Owner>(ref this.m_OwnerType);
            for (int index = 0; index < nativeArray2.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              ref Attached local = ref nativeArray2.ElementAt<Attached>(index);
              Temp component = nativeArray3[index];
              PrefabRef prefabRef = nativeArray4[index];
              DynamicBuffer<SubObject> bufferData3;
              // ISSUE: reference to a compiler-generated field
              if (local.m_OldParent != Entity.Null && local.m_OldParent != local.m_Parent && this.m_SubObjects.TryGetBuffer(local.m_OldParent, out bufferData3))
                CollectionUtils.RemoveValue<SubObject>(bufferData3, new SubObject(entity));
              local.m_OldParent = Entity.Null;
              if (local.m_Parent == entity)
              {
                local.m_Parent = Entity.Null;
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<Attached>(entity);
              }
              else
              {
                bool flag2 = local.m_Parent != Entity.Null;
                Temp componentData1;
                // ISSUE: reference to a compiler-generated field
                if (this.m_TempData.TryGetComponent(local.m_Parent, out componentData1))
                {
                  flag2 = (componentData1.m_Flags & TempFlags.Delete) == (TempFlags) 0;
                  DynamicBuffer<SubObject> bufferData4;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_SubObjects.TryGetBuffer(local.m_Parent, out bufferData4))
                    CollectionUtils.TryAddUniqueValue<SubObject>(bufferData4, new SubObject(entity));
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PlaceholderData.HasComponent(local.m_Parent))
                  {
                    newAttachments.Remove(local.m_Parent);
                    newAttachments.TryAdd(local.m_Parent, entity);
                  }
                }
                if (!flag1 && !flag2 && (component.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Upgrade | TempFlags.Parent | TempFlags.Duplicate)) == (TempFlags) 0)
                {
                  ObjectGeometryData objectGeometryData = new ObjectGeometryData();
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ObjectGeometryData.HasComponent(prefabRef.m_Prefab))
                  {
                    // ISSUE: reference to a compiler-generated field
                    objectGeometryData = this.m_ObjectGeometryData[prefabRef.m_Prefab];
                  }
                  if ((objectGeometryData.m_Flags & GeometryFlags.OptionalAttach) == GeometryFlags.None)
                  {
                    component.m_Flags |= TempFlags.Delete;
                    Recent componentData2;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_RecentData.TryGetComponent(component.m_Original, out componentData2))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      component.m_Cost = -ObjectUtils.GetRefundAmount(componentData2, this.m_SimulationFrame, this.m_EconomyParameterData);
                    }
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetComponent<Temp>(entity, component);
                  }
                }
              }
            }
          }
          else
          {
            for (int index = 0; index < nativeArray2.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              ref Attached local = ref nativeArray2.ElementAt<Attached>(index);
              DynamicBuffer<SubObject> bufferData5;
              // ISSUE: reference to a compiler-generated field
              if (local.m_OldParent != Entity.Null && local.m_OldParent != local.m_Parent && this.m_SubObjects.TryGetBuffer(local.m_OldParent, out bufferData5))
                CollectionUtils.RemoveValue<SubObject>(bufferData5, new SubObject(entity));
              local.m_OldParent = Entity.Null;
              if (local.m_Parent == entity)
              {
                local.m_Parent = Entity.Null;
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<Attached>(entity);
              }
              else
              {
                DynamicBuffer<SubObject> bufferData6;
                // ISSUE: reference to a compiler-generated field
                if (this.m_SubObjects.TryGetBuffer(local.m_Parent, out bufferData6))
                  CollectionUtils.TryAddUniqueValue<SubObject>(bufferData6, new SubObject(entity));
                // ISSUE: reference to a compiler-generated field
                if (this.m_PlaceholderData.HasComponent(local.m_Parent))
                {
                  newAttachments.Remove(local.m_Parent);
                  newAttachments.TryAdd(local.m_Parent, entity);
                }
              }
            }
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Node> __Game_Net_Node_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Elevation> __Game_Net_Elevation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceholderBuildingData> __Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      public ComponentLookup<Attached> __Game_Objects_Attached_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Recent> __Game_Tools_Recent_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Placeholder> __Game_Objects_Placeholder_RO_ComponentLookup;
      public ComponentTypeHandle<Attached> __Game_Objects_Attached_RW_ComponentTypeHandle;
      public BufferLookup<SubObject> __Game_Objects_SubObject_RW_BufferLookup;
      public ComponentLookup<Attachment> __Game_Objects_Attachment_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup = state.GetComponentLookup<PlaceholderBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RW_ComponentLookup = state.GetComponentLookup<Attached>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Recent_RO_ComponentLookup = state.GetComponentLookup<Recent>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Placeholder_RO_ComponentLookup = state.GetComponentLookup<Placeholder>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Attached>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RW_BufferLookup = state.GetBufferLookup<SubObject>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attachment_RW_ComponentLookup = state.GetComponentLookup<Attachment>();
      }
    }
  }
}
