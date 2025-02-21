// Decompiled with JetBrains decompiler
// Type: Game.Net.ReferencesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class ReferencesSystem : GameSystemBase
  {
    private EntityQuery m_EdgeQuery;
    private EntityQuery m_NodeQuery;
    private EntityQuery m_TempEdgeQuery;
    private ReferencesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Edge>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_NodeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Node>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_TempEdgeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Edge>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_NodeQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectedNode_RW_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectedEdge_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        this.Dependency = new ReferencesSystem.UpdateNodeReferencesJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
          m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
          m_ConnectedEdgeType = this.__TypeHandle.__Game_Net_ConnectedEdge_RW_BufferTypeHandle,
          m_Nodes = this.__TypeHandle.__Game_Net_ConnectedNode_RW_BufferLookup
        }.Schedule<ReferencesSystem.UpdateNodeReferencesJob>(this.m_NodeQuery, this.Dependency);
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TempEdgeQuery.IsEmptyIgnoreFilter)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Net_ConnectedNode_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          this.Dependency = new ReferencesSystem.ValidateConnectedNodesJob()
          {
            m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
            m_ConnectedNodeType = this.__TypeHandle.__Game_Net_ConnectedNode_RW_BufferTypeHandle
          }.ScheduleParallel<ReferencesSystem.ValidateConnectedNodesJob>(this.m_TempEdgeQuery, this.Dependency);
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_EdgeQuery.IsEmptyIgnoreFilter)
        return;
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_EdgeQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      NativeParallelMultiHashMap<Entity, ReferencesSystem.ConnectedNodeValue> parallelMultiHashMap = new NativeParallelMultiHashMap<Entity, ReferencesSystem.ConnectedNodeValue>(32, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ReferencesSystem.UpdateEdgeReferencesJob jobData1 = new ReferencesSystem.UpdateEdgeReferencesJob()
      {
        m_EdgeChunks = archetypeChunkListAsync,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_ConnectedNodeType = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferTypeHandle,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RW_BufferLookup,
        m_ConnectedNodes = parallelMultiHashMap
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Standalone_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ReferencesSystem.RationalizeConnectedNodesJob jobData2 = new ReferencesSystem.RationalizeConnectedNodesJob()
      {
        m_EdgeChunks = archetypeChunkListAsync.AsDeferredJobArray(),
        m_ConnectedNodes = parallelMultiHashMap,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_StandaloneData = this.__TypeHandle.__Game_Net_Standalone_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabLocalConnectData = this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_ConnectedNodeType = this.__TypeHandle.__Game_Net_ConnectedNode_RW_BufferTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ReferencesSystem.AddConnectedNodeReferencesJob jobData3 = new ReferencesSystem.AddConnectedNodeReferencesJob()
      {
        m_EdgeChunks = archetypeChunkListAsync,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_ConnectedNodeType = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferTypeHandle,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RW_BufferLookup
      };
      JobHandle dependsOn1 = jobData1.Schedule<ReferencesSystem.UpdateEdgeReferencesJob>(JobHandle.CombineDependencies(outJobHandle, this.Dependency));
      JobHandle inputDeps1 = jobData2.Schedule<ReferencesSystem.RationalizeConnectedNodesJob, ArchetypeChunk>(archetypeChunkListAsync, 1, dependsOn1);
      JobHandle dependsOn2 = inputDeps1;
      JobHandle inputDeps2 = jobData3.Schedule<ReferencesSystem.AddConnectedNodeReferencesJob>(dependsOn2);
      archetypeChunkListAsync.Dispose(inputDeps2);
      parallelMultiHashMap.Dispose(inputDeps1);
      this.Dependency = inputDeps2;
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
    public ReferencesSystem()
    {
    }

    [BurstCompile]
    private struct UpdateNodeReferencesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      public BufferTypeHandle<ConnectedEdge> m_ConnectedEdgeType;
      public BufferLookup<ConnectedNode> m_Nodes;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ConnectedEdge> bufferAccessor = chunk.GetBufferAccessor<ConnectedEdge>(ref this.m_ConnectedEdgeType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
          for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
          {
            Entity node = nativeArray[index1];
            DynamicBuffer<ConnectedEdge> dynamicBuffer = bufferAccessor[index1];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              CollectionUtils.RemoveValue<ConnectedNode>(this.m_Nodes[dynamicBuffer[index2].m_Edge], new ConnectedNode(node, 0.5f));
            }
          }
        }
        else
        {
          for (int index3 = 0; index3 < bufferAccessor.Length; ++index3)
          {
            DynamicBuffer<ConnectedEdge> dynamicBuffer = bufferAccessor[index3];
            for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_UpdatedData.HasComponent(dynamicBuffer[index4].m_Edge))
                dynamicBuffer.RemoveAt(index4--);
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
    private struct ValidateConnectedNodesJob : IJobChunk
    {
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      public BufferTypeHandle<ConnectedNode> m_ConnectedNodeType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ConnectedNode> bufferAccessor = chunk.GetBufferAccessor<ConnectedNode>(ref this.m_ConnectedNodeType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<ConnectedNode> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_DeletedData.HasComponent(dynamicBuffer[index2].m_Node))
              dynamicBuffer.RemoveAt(index2--);
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
    private struct UpdateEdgeReferencesJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_EdgeChunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Edge> m_EdgeType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public BufferTypeHandle<ConnectedNode> m_ConnectedNodeType;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      public BufferLookup<ConnectedEdge> m_Edges;
      public NativeParallelMultiHashMap<Entity, ReferencesSystem.ConnectedNodeValue> m_ConnectedNodes;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_EdgeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk edgeChunk = this.m_EdgeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = edgeChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Edge> nativeArray2 = edgeChunk.GetNativeArray<Edge>(ref this.m_EdgeType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<ConnectedNode> bufferAccessor = edgeChunk.GetBufferAccessor<ConnectedNode>(ref this.m_ConnectedNodeType);
          // ISSUE: reference to a compiler-generated field
          if (edgeChunk.Has<Deleted>(ref this.m_DeletedType))
          {
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              Entity edge1 = nativeArray1[index2];
              Edge edge2 = nativeArray2[index2];
              DynamicBuffer<ConnectedNode> dynamicBuffer = bufferAccessor[index2];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ConnectedEdge> edge3 = this.m_Edges[edge2.m_Start];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ConnectedEdge> edge4 = this.m_Edges[edge2.m_End];
              CollectionUtils.RemoveValue<ConnectedEdge>(edge3, new ConnectedEdge(edge1));
              ConnectedEdge connectedEdge = new ConnectedEdge(edge1);
              CollectionUtils.RemoveValue<ConnectedEdge>(edge4, connectedEdge);
              for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.RemoveValue<ConnectedEdge>(this.m_Edges[dynamicBuffer[index3].m_Node], new ConnectedEdge(edge1));
              }
            }
          }
          else
          {
            for (int index4 = 0; index4 < nativeArray1.Length; ++index4)
            {
              Entity edge5 = nativeArray1[index4];
              Edge edge6 = nativeArray2[index4];
              DynamicBuffer<ConnectedNode> dynamicBuffer = bufferAccessor[index4];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ConnectedEdge> edge7 = this.m_Edges[edge6.m_Start];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ConnectedEdge> edge8 = this.m_Edges[edge6.m_End];
              CollectionUtils.TryAddUniqueValue<ConnectedEdge>(edge7, new ConnectedEdge(edge5));
              ConnectedEdge connectedEdge = new ConnectedEdge(edge5);
              CollectionUtils.TryAddUniqueValue<ConnectedEdge>(edge8, connectedEdge);
              for (int index5 = 0; index5 < dynamicBuffer.Length; ++index5)
              {
                ConnectedNode connectedNode = dynamicBuffer[index5];
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.RemoveValue<ConnectedEdge>(this.m_Edges[connectedNode.m_Node], new ConnectedEdge(edge5));
                // ISSUE: reference to a compiler-generated field
                if (!this.m_DeletedData.HasComponent(connectedNode.m_Node))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: object of a compiler-generated type is created
                  this.m_ConnectedNodes.Add(connectedNode.m_Node, new ReferencesSystem.ConnectedNodeValue(edge5, connectedNode.m_CurvePosition));
                }
              }
            }
          }
        }
      }
    }

    private struct ConnectedNodeValue
    {
      public Entity m_Edge;
      public float m_CurvePosition;

      public ConnectedNodeValue(Entity edge, float curvePosition)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Edge = edge;
        // ISSUE: reference to a compiler-generated field
        this.m_CurvePosition = curvePosition;
      }
    }

    [BurstCompile]
    private struct RationalizeConnectedNodesJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_EdgeChunks;
      [ReadOnly]
      public NativeParallelMultiHashMap<Entity, ReferencesSystem.ConnectedNodeValue> m_ConnectedNodes;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Edge> m_EdgeType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Standalone> m_StandaloneData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<LocalConnectData> m_PrefabLocalConnectData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public ComponentLookup<BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_Edges;
      public BufferTypeHandle<ConnectedNode> m_ConnectedNodeType;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ArchetypeChunk edgeChunk = this.m_EdgeChunks[index];
        // ISSUE: reference to a compiler-generated field
        if (edgeChunk.Has<Deleted>(ref this.m_DeletedType))
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = edgeChunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Edge> nativeArray2 = edgeChunk.GetNativeArray<Edge>(ref this.m_EdgeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Curve> nativeArray3 = edgeChunk.GetNativeArray<Curve>(ref this.m_CurveType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray4 = edgeChunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray5 = edgeChunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray6 = edgeChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ConnectedNode> bufferAccessor = edgeChunk.GetBufferAccessor<ConnectedNode>(ref this.m_ConnectedNodeType);
        NativeParallelHashMap<Entity, Bounds1> curveBoundsMap = new NativeParallelHashMap<Entity, Bounds1>();
        NativeParallelHashMap<Entity, float2> nodePosMap = new NativeParallelHashMap<Entity, float2>();
        NativeList<Line2.Segment> lineBuffer = new NativeList<Line2.Segment>();
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          Entity entity = nativeArray1[index1];
          Edge edge1 = nativeArray2[index1];
          Curve curve1 = nativeArray3[index1];
          PrefabRef prefabRef1 = nativeArray6[index1];
          DynamicBuffer<ConnectedNode> dynamicBuffer = bufferAccessor[index1];
          Entity owner1 = Entity.Null;
          if (nativeArray4.Length != 0)
            owner1 = nativeArray4[index1].m_Owner;
          Entity original1 = Entity.Null;
          if (nativeArray5.Length != 0)
            original1 = nativeArray5[index1].m_Original;
          float size1 = 0.0f;
          NetGeometryData componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabNetGeometryData.TryGetComponent(prefabRef1.m_Prefab, out componentData1))
            size1 = componentData1.m_DefaultWidth * 0.5f;
          for (int index2 = dynamicBuffer.Length - 1; index2 >= 0; --index2)
          {
            ConnectedNode connectedNode = dynamicBuffer[index2];
            // ISSUE: reference to a compiler-generated field
            if (!this.m_DeletedData.HasComponent(connectedNode.m_Node))
            {
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef2 = this.m_PrefabRefData[connectedNode.m_Node];
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabLocalConnectData.HasComponent(prefabRef2.m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                LocalConnectData localConnectData = this.m_PrefabLocalConnectData[prefabRef2.m_Prefab];
                float num1 = 0.0f;
                if ((localConnectData.m_Flags & LocalConnectFlags.ChooseSides) != (LocalConnectFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  num1 = (float) ((double) this.m_PrefabNetGeometryData[prefabRef2.m_Prefab].m_DefaultWidth * 0.5 + 0.10000000149011612);
                }
                // ISSUE: reference to a compiler-generated method
                float t1 = this.ClampCurvePosition(entity, edge1, curve1, connectedNode.m_CurvePosition, ref curveBoundsMap, ref nodePosMap, ref lineBuffer);
                float3 float3_1 = MathUtils.Position(curve1.m_Bezier, t1);
                // ISSUE: reference to a compiler-generated field
                float3 position = this.m_NodeData[connectedNode.m_Node].m_Position;
                float nodeDistance1 = math.distance(float3_1, position);
                Entity owner2 = Entity.Null;
                Game.Prefabs.BuildingFlags allowDirections1 = (Game.Prefabs.BuildingFlags) 0;
                float3 allowForward1 = new float3();
                Owner componentData2;
                Game.Prefabs.BuildingFlags allowDirections2;
                float3 allowForward2;
                // ISSUE: reference to a compiler-generated field
                if (this.m_OwnerData.TryGetComponent(connectedNode.m_Node, out componentData2))
                {
                  owner2 = componentData2.m_Owner;
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated method
                  if (owner1 != owner2 && (this.GetElevationFlags(entity, edge1, componentData1).m_General & CompositionFlags.General.Tunnel) == (CompositionFlags.General) 0 && (!this.AllowConnection(owner2, new Line3.Segment(position, float3_1), out allowDirections1, out allowForward1) || owner1 != Entity.Null && !this.AllowConnection(owner1, new Line3.Segment(float3_1, position), out allowDirections2, out allowForward2)))
                    goto label_29;
                }
                if ((localConnectData.m_Flags & LocalConnectFlags.ChooseBest) != (LocalConnectFlags) 0)
                {
                  bool flag = false;
                  // ISSUE: variable of a compiler-generated type
                  ReferencesSystem.ConnectedNodeValue connectedNodeValue;
                  NativeParallelMultiHashMapIterator<Entity> it;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ConnectedNodes.TryGetFirstValue(connectedNode.m_Node, out connectedNodeValue, out it))
                  {
                    // ISSUE: reference to a compiler-generated field
                    do
                    {
                      Entity original2 = Entity.Null;
                      Temp componentData3;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_TempData.TryGetComponent(connectedNodeValue.m_Edge, out componentData3))
                        original2 = componentData3.m_Original;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (connectedNodeValue.m_Edge == entity || connectedNodeValue.m_Edge == original1 || original2 == entity)
                      {
                        flag = true;
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        Edge edge2 = this.m_EdgeData[connectedNodeValue.m_Edge];
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        Curve curve2 = this.m_CurveData[connectedNodeValue.m_Edge];
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        PrefabRef prefabRef3 = this.m_PrefabRefData[connectedNodeValue.m_Edge];
                        float size2 = 0.0f;
                        NetGeometryData componentData4;
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_PrefabNetGeometryData.TryGetComponent(prefabRef3.m_Prefab, out componentData4))
                          size2 = componentData4.m_DefaultWidth * 0.5f;
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        float t2 = this.ClampCurvePosition(connectedNodeValue.m_Edge, edge2, curve2, connectedNodeValue.m_CurvePosition, ref curveBoundsMap, ref nodePosMap, ref lineBuffer);
                        float3 float3_2 = MathUtils.Position(curve2.m_Bezier, t2);
                        float nodeDistance2 = math.distance(float3_2, position);
                        float num2 = (float) ((double) nodeDistance1 - (double) size1 - ((double) nodeDistance2 - (double) size2));
                        Edge originals1;
                        Edge originals2;
                        // ISSUE: reference to a compiler-generated method
                        // ISSUE: reference to a compiler-generated method
                        // ISSUE: reference to a compiler-generated method
                        // ISSUE: reference to a compiler-generated method
                        // ISSUE: reference to a compiler-generated method
                        if ((localConnectData.m_Flags & LocalConnectFlags.ChooseSides) != (LocalConnectFlags) 0 && (double) num2 >= 0.0 && (double) num2 - (double) num1 <= 0.0 && (this.AreNeighbors(edge1, edge2) || this.GetOriginals(edge1, out originals1) && this.AreNeighbors(originals1, edge2) || this.GetOriginals(edge2, out originals2) && this.AreNeighbors(edge1, originals2)))
                          num2 -= math.sqrt((float) ((double) size1 * (double) size1 + (double) num1 * (double) num1)) - size1;
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        // ISSUE: reference to a compiler-generated method
                        // ISSUE: reference to a compiler-generated method
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        if (((double) num2 > 0.0 || (double) num2 == 0.0 && !flag) && (!(owner2 != Entity.Null) || !(owner1 != owner2) || (this.GetElevationFlags(connectedNodeValue.m_Edge, edge2, componentData4).m_General & CompositionFlags.General.Tunnel) != (CompositionFlags.General) 0 || this.AllowConnection(owner2, new Line3.Segment(position, float3_2), out allowDirections2, out allowForward2) && (!(owner1 != Entity.Null) || this.AllowConnection(owner1, new Line3.Segment(float3_2, position), out allowDirections2, out allowForward2))) && this.ValidateConnection(connectedNodeValue.m_Edge, connectedNode.m_Node, original2, edge2, localConnectData, float3_2, nodeDistance2, size2, allowDirections1, allowForward1))
                          goto label_29;
                      }
                    }
                    while (this.m_ConnectedNodes.TryGetNextValue(out connectedNodeValue, ref it));
                  }
                }
                // ISSUE: reference to a compiler-generated method
                if (this.ValidateConnection(entity, connectedNode.m_Node, original1, edge1, localConnectData, float3_1, nodeDistance1, size1, allowDirections1, allowForward1))
                  continue;
              }
            }
label_29:
            dynamicBuffer.RemoveAt(index2);
          }
        }
        if (curveBoundsMap.IsCreated)
          curveBoundsMap.Dispose();
        if (nodePosMap.IsCreated)
          nodePosMap.Dispose();
        if (!lineBuffer.IsCreated)
          return;
        lineBuffer.Dispose();
      }

      private CompositionFlags GetElevationFlags(
        Entity entity,
        Edge edge,
        NetGeometryData prefabGeometryData)
      {
        Elevation componentData1;
        // ISSUE: reference to a compiler-generated field
        this.m_ElevationData.TryGetComponent(edge.m_Start, out componentData1);
        Elevation componentData2;
        // ISSUE: reference to a compiler-generated field
        this.m_ElevationData.TryGetComponent(entity, out componentData2);
        Elevation componentData3;
        // ISSUE: reference to a compiler-generated field
        this.m_ElevationData.TryGetComponent(edge.m_End, out componentData3);
        return NetCompositionHelpers.GetElevationFlags(componentData1, componentData2, componentData3, prefabGeometryData);
      }

      private float ClampCurvePosition(
        Entity entity,
        Edge edge,
        Curve curve,
        float curvePos,
        ref NativeParallelHashMap<Entity, Bounds1> curveBoundsMap,
        ref NativeParallelHashMap<Entity, float2> nodePosMap,
        ref NativeList<Line2.Segment> lineBuffer)
      {
        Bounds1 bounds;
        if (curveBoundsMap.IsCreated)
        {
          if (curveBoundsMap.TryGetValue(entity, out bounds))
            return MathUtils.Clamp(curvePos, bounds);
        }
        else
          curveBoundsMap = new NativeParallelHashMap<Entity, Bounds1>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        if ((double) curve.m_Length >= 0.20000000298023224)
        {
          // ISSUE: reference to a compiler-generated method
          float2 nodePosition1 = this.GetNodePosition(edge.m_Start, ref nodePosMap, ref lineBuffer);
          // ISSUE: reference to a compiler-generated method
          float2 nodePosition2 = this.GetNodePosition(edge.m_End, ref nodePosMap, ref lineBuffer);
          double num1 = (double) MathUtils.Distance(curve.m_Bezier.xz, nodePosition1, out bounds.min);
          double num2 = (double) MathUtils.Distance(curve.m_Bezier.xz, nodePosition2, out bounds.max);
          float num3 = 0.1f / curve.m_Length;
          bounds.min += num3;
          bounds.max -= num3;
          if ((double) bounds.max < (double) bounds.min)
            bounds.min = bounds.max = MathUtils.Center(bounds);
        }
        else
          bounds = new Bounds1(0.5f, 0.5f);
        curveBoundsMap.Add(entity, bounds);
        return MathUtils.Clamp(curvePos, bounds);
      }

      private float2 GetNodePosition(
        Entity entity,
        ref NativeParallelHashMap<Entity, float2> nodePosMap,
        ref NativeList<Line2.Segment> lineBuffer)
      {
        if (nodePosMap.IsCreated)
        {
          float2 nodePosition;
          if (nodePosMap.TryGetValue(entity, out nodePosition))
            return nodePosition;
        }
        else
          nodePosMap = new NativeParallelHashMap<Entity, float2>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        Node node = this.m_NodeData[entity];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_StandaloneData.HasComponent(entity))
        {
          float3 float3_1 = new float3();
          float3 position = node.m_Position;
          int num1 = 0;
          if (lineBuffer.IsCreated)
            lineBuffer.Clear();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          EdgeIterator edgeIterator = new EdgeIterator(Entity.Null, entity, this.m_Edges, this.m_EdgeData, this.m_TempData, this.m_HiddenData);
          EdgeIteratorValue edgeIteratorValue;
          while (edgeIterator.GetNext(out edgeIteratorValue))
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[edgeIteratorValue.m_Edge];
            float3 float3_2;
            float3 float3_3;
            float2 _b;
            if (edgeIteratorValue.m_End)
            {
              float3_2 = curve.m_Bezier.d - position;
              float3_3 = MathUtils.EndTangent(curve.m_Bezier);
              _b = -float3_3.xz;
            }
            else
            {
              float3_2 = curve.m_Bezier.a - position;
              float3_3 = MathUtils.StartTangent(curve.m_Bezier);
              _b = float3_3.xz;
            }
            float3_1 += float3_2;
            ++num1;
            if (MathUtils.TryNormalize(ref _b))
            {
              if (!lineBuffer.IsCreated)
                lineBuffer = new NativeList<Line2.Segment>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
              lineBuffer.Add(new Line2.Segment(float3_2.xz, _b));
            }
          }
          if (num1 > 0)
          {
            node.m_Position = position + float3_1 / (float) num1;
            float3_1 = new float3();
            num1 = 0;
          }
          if (lineBuffer.IsCreated && lineBuffer.Length >= 2)
          {
            NativeList<Line2.Segment> list = lineBuffer;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            ReferencesSystem.RationalizeConnectedNodesJob.LineComparer lineComparer = new ReferencesSystem.RationalizeConnectedNodesJob.LineComparer();
            // ISSUE: variable of a compiler-generated type
            ReferencesSystem.RationalizeConnectedNodesJob.LineComparer comp = lineComparer;
            list.Sort<Line2.Segment, ReferencesSystem.RationalizeConnectedNodesJob.LineComparer>(comp);
            for (int index1 = 1; index1 < lineBuffer.Length; ++index1)
            {
              Line2.Segment segment1 = lineBuffer[index1];
              for (int index2 = 0; index2 < index1; ++index2)
              {
                Line2.Segment segment2 = lineBuffer[index2];
                float x = math.dot(segment1.b, segment2.b);
                float3 float3_4 = new float3();
                if ((double) math.abs(x) > 0.99900001287460327)
                {
                  float3_4.xy = segment1.a + segment2.a;
                  float3_4.z = 2f;
                }
                else
                {
                  float2 float2 = math.distance(segment1.a, segment2.a) * new float2(math.abs(x) - 1f, 1f - math.abs(x));
                  Line2.Segment segment3 = new Line2.Segment(segment1.a - segment1.b * float2.x, segment1.a - segment1.b * float2.y);
                  Line2.Segment segment4 = new Line2.Segment(segment2.a - segment2.b * float2.x, segment2.a - segment2.b * float2.y);
                  float2 t;
                  double num2 = (double) MathUtils.Distance(segment3, segment4, out t);
                  float3_4.xy = MathUtils.Position(segment3, t.x) + MathUtils.Position(segment4, t.y);
                  float3_4.z = 2f;
                }
                float num3 = 1.01f - math.abs(x);
                float3_1 += float3_4 * num3;
                ++num1;
              }
            }
            if (num1 > 0)
              node.m_Position.xz = position.xz + float3_1.xy / float3_1.z;
          }
        }
        nodePosMap.Add(entity, node.m_Position.xz);
        return node.m_Position.xz;
      }

      private bool AreNeighbors(Edge edge1, Edge edge2)
      {
        return edge1.m_Start == edge2.m_Start || edge1.m_End == edge2.m_Start || edge1.m_Start == edge2.m_End || edge1.m_End == edge2.m_End;
      }

      private bool GetOriginals(Edge edge, out Edge originals)
      {
        Temp componentData1;
        Temp componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_TempData.TryGetComponent(edge.m_Start, out componentData1) && this.m_TempData.TryGetComponent(edge.m_End, out componentData2))
        {
          originals.m_Start = componentData1.m_Original;
          originals.m_End = componentData2.m_Original;
          return true;
        }
        originals = new Edge();
        return false;
      }

      private bool ValidateConnection(
        Entity entity,
        Entity node,
        Entity original,
        Edge edge,
        LocalConnectData localConnectData,
        float3 edgePosition,
        float nodeDistance,
        float size,
        Game.Prefabs.BuildingFlags allowDirections,
        float3 allowForward)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeIterator edgeIterator = new EdgeIterator(Entity.Null, node, this.m_Edges, this.m_EdgeData, this.m_TempData, this.m_HiddenData, (localConnectData.m_Flags & LocalConnectFlags.ChooseBest) > (LocalConnectFlags) 0);
        int num = 0;
        EdgeIteratorValue edgeIteratorValue;
        while (edgeIterator.GetNext(out edgeIteratorValue))
        {
          if (edgeIteratorValue.m_Middle)
          {
            if (edgeIteratorValue.m_Edge != entity && edgeIteratorValue.m_Edge != original)
            {
              if ((localConnectData.m_Flags & LocalConnectFlags.ChooseSides) == (LocalConnectFlags) 0)
                return false;
              // ISSUE: reference to a compiler-generated field
              Edge edge2 = this.m_EdgeData[edgeIteratorValue.m_Edge];
              Edge originals;
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              if (!this.AreNeighbors(edge, edge2) && (!this.GetOriginals(edge, out originals) || !this.AreNeighbors(originals, edge2)))
                return false;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            Edge edge1 = this.m_EdgeData[edgeIteratorValue.m_Edge];
            Entity entity1 = edgeIteratorValue.m_End ? edge1.m_Start : edge1.m_End;
            if (entity1 == edge.m_Start || entity1 == edge.m_End || ++num >= 2 && (localConnectData.m_Flags & LocalConnectFlags.RequireDeadend) != (LocalConnectFlags) 0)
              return false;
            // ISSUE: reference to a compiler-generated field
            float3 position = this.m_NodeData[entity1].m_Position;
            if ((double) math.distance(edgePosition, position) < (double) nodeDistance)
              return false;
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[edgeIteratorValue.m_Edge];
            float3 float3;
            float2 x1;
            if (!edgeIteratorValue.m_End)
            {
              float3 = MathUtils.StartTangent(curve.m_Bezier);
              x1 = -float3.xz;
            }
            else
            {
              float3 = MathUtils.EndTangent(curve.m_Bezier);
              x1 = float3.xz;
            }
            float2 defaultvalue = new float2();
            float2 x2 = math.normalizesafe(x1, defaultvalue);
            float2 float2_1 = (edgeIteratorValue.m_End ? curve.m_Bezier.d.xz : curve.m_Bezier.a.xz) - x2 * size;
            float2 y = math.normalizesafe(edgePosition.xz - float2_1);
            if ((double) math.dot(x2, y) < 0.70710676908493042)
              return false;
            ref float3 local = ref allowForward;
            float3 = new float3();
            float3 rhs = float3;
            if (!local.Equals(rhs))
            {
              float2 float2_2 = math.normalizesafe(allowForward.xz);
              float2 xy = new float2(math.dot(x2, float2_2), math.dot(x2, MathUtils.Left(float2_2)));
              if (!math.any(new float4(xy, -xy) >= 0.707106769f & new bool4(true, (allowDirections & Game.Prefabs.BuildingFlags.RightAccess) > (Game.Prefabs.BuildingFlags) 0, (allowDirections & Game.Prefabs.BuildingFlags.BackAccess) > (Game.Prefabs.BuildingFlags) 0, (allowDirections & Game.Prefabs.BuildingFlags.LeftAccess) > (Game.Prefabs.BuildingFlags) 0)))
                return false;
            }
          }
        }
        return true;
      }

      private bool AllowConnection(
        Entity owner,
        Line3.Segment line,
        out Game.Prefabs.BuildingFlags allowDirections,
        out float3 allowForward)
      {
        allowDirections = (Game.Prefabs.BuildingFlags) 0;
        allowForward = new float3();
        Owner componentData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.TryGetComponent(owner, out componentData1) && !this.m_BuildingData.HasComponent(owner))
          owner = componentData1.m_Owner;
        PrefabRef componentData2;
        Transform componentData3;
        BuildingData componentData4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabRefData.TryGetComponent(owner, out componentData2) || !this.m_TransformData.TryGetComponent(owner, out componentData3) || !this.m_PrefabBuildingData.TryGetComponent(componentData2.m_Prefab, out componentData4))
          return true;
        float2 size = (float2) componentData4.m_LotSize * 8f;
        Quad2 xz = ObjectUtils.CalculateBaseCorners(componentData3.m_Position, componentData3.m_Rotation, size).xz;
        float2 t;
        if ((componentData4.m_Flags & Game.Prefabs.BuildingFlags.LeftAccess) == (Game.Prefabs.BuildingFlags) 0 && MathUtils.Intersect(line.xz, xz.bc, out t) || (componentData4.m_Flags & Game.Prefabs.BuildingFlags.BackAccess) == (Game.Prefabs.BuildingFlags) 0 && MathUtils.Intersect(line.xz, xz.cd, out t) || (componentData4.m_Flags & Game.Prefabs.BuildingFlags.RightAccess) == (Game.Prefabs.BuildingFlags) 0 && MathUtils.Intersect(line.xz, xz.da, out t))
          return false;
        allowDirections = componentData4.m_Flags;
        allowForward = math.forward(componentData3.m_Rotation);
        return true;
      }

      [StructLayout(LayoutKind.Sequential, Size = 1)]
      private struct LineComparer : IComparer<Line2.Segment>
      {
        public int Compare(Line2.Segment x, Line2.Segment y)
        {
          return math.csum(math.select((int4) 0, math.select(new int4(-8, -4, -2, -1), new int4(8, 4, 2, 1), x.ab > y.ab), x.ab != y.ab));
        }
      }
    }

    [BurstCompile]
    private struct AddConnectedNodeReferencesJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_EdgeChunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public BufferTypeHandle<ConnectedNode> m_ConnectedNodeType;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      public BufferLookup<ConnectedEdge> m_Edges;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_EdgeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk edgeChunk = this.m_EdgeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          if (!edgeChunk.Has<Deleted>(ref this.m_DeletedType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray = edgeChunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<ConnectedNode> bufferAccessor = edgeChunk.GetBufferAccessor<ConnectedNode>(ref this.m_ConnectedNodeType);
            // ISSUE: reference to a compiler-generated field
            bool flag = edgeChunk.Has<Temp>(ref this.m_TempType);
            for (int index2 = 0; index2 < nativeArray.Length; ++index2)
            {
              Entity edge = nativeArray[index2];
              DynamicBuffer<ConnectedNode> dynamicBuffer = bufferAccessor[index2];
              for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
              {
                Entity node = dynamicBuffer[index3].m_Node;
                // ISSUE: reference to a compiler-generated field
                if (!flag || this.m_TempData.HasComponent(node))
                {
                  // ISSUE: reference to a compiler-generated field
                  CollectionUtils.TryAddUniqueValue<ConnectedEdge>(this.m_Edges[node], new ConnectedEdge(edge));
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
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      public BufferTypeHandle<ConnectedEdge> __Game_Net_ConnectedEdge_RW_BufferTypeHandle;
      public BufferLookup<ConnectedNode> __Game_Net_ConnectedNode_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      public BufferTypeHandle<ConnectedNode> __Game_Net_ConnectedNode_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ConnectedNode> __Game_Net_ConnectedNode_RO_BufferTypeHandle;
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RW_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Standalone> __Game_Net_Standalone_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LocalConnectData> __Game_Prefabs_LocalConnectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RW_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedEdge>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RW_BufferLookup = state.GetBufferLookup<ConnectedNode>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RW_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedNode>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RW_BufferLookup = state.GetBufferLookup<ConnectedEdge>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Standalone_RO_ComponentLookup = state.GetComponentLookup<Standalone>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LocalConnectData_RO_ComponentLookup = state.GetComponentLookup<LocalConnectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
      }
    }
  }
}
