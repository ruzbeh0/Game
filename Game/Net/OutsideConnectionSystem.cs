// Decompiled with JetBrains decompiler
// Type: Game.Net.OutsideConnectionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class OutsideConnectionSystem : GameSystemBase
  {
    private ModificationBarrier4 m_ModificationBarrier;
    private EntityQuery m_UpdatedQuery;
    private EntityQuery m_ConnectionQuery;
    private EntityQuery m_PrefabQuery;
    private bool m_Regenerate;
    private OutsideConnectionSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Objects.OutsideConnection>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Game.Objects.ElectricityOutsideConnection>(),
          ComponentType.ReadOnly<Game.Objects.WaterPipeOutsideConnection>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ConnectionQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Objects.OutsideConnection>(),
          ComponentType.ReadOnly<Transform>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Game.Objects.ElectricityOutsideConnection>(),
          ComponentType.ReadOnly<Game.Objects.WaterPipeOutsideConnection>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<OutsideConnection>(),
          ComponentType.ReadOnly<Lane>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<ConnectionLaneData>(), ComponentType.ReadOnly<PrefabData>());
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context context)
    {
      base.OnGameLoaded(context);
      if (!(context.version < Game.Version.outsideConnectionRemoteness))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_Regenerate = true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Regenerate && this.m_UpdatedQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_Regenerate = false;
      JobHandle outJobHandle1;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_ConnectionQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle1);
      JobHandle outJobHandle2;
      // ISSUE: reference to a compiler-generated field
      NativeList<Entity> entityListAsync = this.m_PrefabQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_ConnectedRoute_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle jobHandle = new OutsideConnectionSystem.UpdateOutsideConnectionsJob()
      {
        m_ConnectionChunks = archetypeChunkListAsync,
        m_PrefabEntities = entityListAsync,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_LaneType = this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_OutsideConnectionType = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_SubLaneType = this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle,
        m_ConnectedRouteType = this.__TypeHandle.__Game_Routes_ConnectedRoute_RO_BufferTypeHandle,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_SecondaryLaneData = this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_MasterLaneData = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_PrefabLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_PrefabCarLaneData = this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup,
        m_PrefabTrackLaneData = this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup,
        m_PrefabRouteConnectionData = this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup,
        m_PrefabLaneArchetypeData = this.__TypeHandle.__Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup,
        m_PrefabOutsideConnectionData = this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_PrefabCompositionLanes = this.__TypeHandle.__Game_Prefabs_NetCompositionLane_RO_BufferLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<OutsideConnectionSystem.UpdateOutsideConnectionsJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle1, outJobHandle2));
      archetypeChunkListAsync.Dispose(jobHandle);
      entityListAsync.Dispose(jobHandle);
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
    public OutsideConnectionSystem()
    {
    }

    private enum ConnectionType
    {
      Pedestrian,
      Road,
      Track,
      Parking,
    }

    private struct NodeData : IComparable<OutsideConnectionSystem.NodeData>
    {
      public float m_Order;
      public float m_Remoteness;
      public float3 m_Position1;
      public float3 m_Position2;
      public PathNode m_Node1;
      public PathNode m_Node2;
      public PathNode m_Node3;
      public OutsideConnectionSystem.ConnectionType m_ConnectionType;
      public TrackTypes m_TrackType;
      public RoadTypes m_RoadType;
      public Entity m_Owner;

      public int CompareTo(OutsideConnectionSystem.NodeData other)
      {
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
        return math.select(math.select(math.select(math.select(0, math.select(-1, 1, (double) this.m_Order > (double) other.m_Order), (double) this.m_Order != (double) other.m_Order), (int) (this.m_TrackType - other.m_TrackType), this.m_TrackType != other.m_TrackType), (int) (this.m_RoadType - other.m_RoadType), this.m_RoadType != other.m_RoadType), this.m_ConnectionType - other.m_ConnectionType, this.m_ConnectionType != other.m_ConnectionType);
      }
    }

    private struct LaneData : IEquatable<OutsideConnectionSystem.LaneData>
    {
      public PathNode m_Start;
      public PathNode m_End;

      public LaneData(Lane lane)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Start = lane.m_StartNode;
        // ISSUE: reference to a compiler-generated field
        this.m_End = lane.m_EndNode;
      }

      public LaneData(OutsideConnectionSystem.NodeData nodeData)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Start = nodeData.m_Node1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_End = nodeData.m_Node3;
      }

      public LaneData(
        OutsideConnectionSystem.NodeData nodeData1,
        OutsideConnectionSystem.NodeData nodeData2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Start = nodeData1.m_Node3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_End = nodeData2.m_Node3;
      }

      public bool Equals(OutsideConnectionSystem.LaneData other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Start.Equals(other.m_Start) && this.m_End.Equals(other.m_End);
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (17 * 31 + this.m_Start.GetHashCode()) * 31 + this.m_End.GetHashCode();
      }
    }

    [BurstCompile]
    private struct UpdateOutsideConnectionsJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_ConnectionChunks;
      [ReadOnly]
      public NativeList<Entity> m_PrefabEntities;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Lane> m_LaneType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<SubLane> m_SubLaneType;
      [ReadOnly]
      public BufferTypeHandle<ConnectedRoute> m_ConnectedRouteType;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public ComponentLookup<SecondaryLane> m_SecondaryLaneData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<MasterLane> m_MasterLaneData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabLaneData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<CarLaneData> m_PrefabCarLaneData;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> m_PrefabTrackLaneData;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> m_PrefabRouteConnectionData;
      [ReadOnly]
      public ComponentLookup<NetLaneArchetypeData> m_PrefabLaneArchetypeData;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> m_PrefabOutsideConnectionData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<NetCompositionLane> m_PrefabCompositionLanes;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        NativeList<OutsideConnectionSystem.NodeData> nativeList = new NativeList<OutsideConnectionSystem.NodeData>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeParallelHashMap<OutsideConnectionSystem.LaneData, Entity> laneMap = new NativeParallelHashMap<OutsideConnectionSystem.LaneData, Entity>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_ConnectionChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk connectionChunk = this.m_ConnectionChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = connectionChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          if (connectionChunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Owner> nativeArray2 = connectionChunk.GetNativeArray<Owner>(ref this.m_OwnerType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Transform> nativeArray3 = connectionChunk.GetNativeArray<Transform>(ref this.m_TransformType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray4 = connectionChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            bool flag = connectionChunk.Has<SubLane>(ref this.m_SubLaneType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              PrefabRef prefabRef = nativeArray4[index2];
              // ISSUE: reference to a compiler-generated field
              OutsideConnectionData outsideConnectionData = this.m_PrefabOutsideConnectionData[prefabRef.m_Prefab];
              if (nativeArray2.Length != 0)
              {
                Owner owner = nativeArray2[index2];
                // ISSUE: reference to a compiler-generated field
                if (this.m_ConnectedEdges.HasBuffer(owner.m_Owner))
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[owner.m_Owner];
                  // ISSUE: reference to a compiler-generated method
                  this.FillNodeData(owner.m_Owner, connectedEdge, outsideConnectionData, nativeList);
                  continue;
                }
              }
              if (flag)
              {
                Entity owner = nativeArray1[index2];
                Transform transform = nativeArray3[index2];
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabRouteConnectionData.HasComponent(prefabRef.m_Prefab))
                {
                  // ISSUE: reference to a compiler-generated field
                  RouteConnectionData routeConnectionData = this.m_PrefabRouteConnectionData[prefabRef.m_Prefab];
                  // ISSUE: reference to a compiler-generated method
                  this.FillNodeData(owner, transform, outsideConnectionData, routeConnectionData, nativeList);
                }
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Lane> nativeArray5 = connectionChunk.GetNativeArray<Lane>(ref this.m_LaneType);
            for (int index3 = 0; index3 < nativeArray5.Length; ++index3)
            {
              Entity entity = nativeArray1[index3];
              Lane lane = nativeArray5[index3];
              // ISSUE: object of a compiler-generated type is created
              laneMap.TryAdd(new OutsideConnectionSystem.LaneData(lane), entity);
            }
          }
        }
        nativeList.Sort<OutsideConnectionSystem.NodeData>();
        for (int index = 0; index < nativeList.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.TryCreateLane(nativeList[index], laneMap);
        }
        int index4;
        if (nativeList.Length >= 2)
        {
          for (int index5 = 0; index5 < nativeList.Length; index5 = index4)
          {
            index4 = index5;
            // ISSUE: variable of a compiler-generated type
            OutsideConnectionSystem.NodeData nodeData1 = nativeList[index5];
            while (++index4 < nativeList.Length)
            {
              // ISSUE: variable of a compiler-generated type
              OutsideConnectionSystem.NodeData nodeData2 = nativeList[index4];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (nodeData2.m_ConnectionType != nodeData1.m_ConnectionType || nodeData2.m_TrackType != nodeData1.m_TrackType || nodeData2.m_RoadType != nodeData1.m_RoadType)
                break;
            }
            // ISSUE: reference to a compiler-generated field
            if (nodeData1.m_ConnectionType != OutsideConnectionSystem.ConnectionType.Parking)
            {
              int b1 = index4 - index5;
              int num1 = b1 - 2;
              for (int index6 = index5; index6 < index4; ++index6)
              {
                int index7 = index6 - 1;
                int index8 = index6;
                if (index6 == index5)
                {
                  if (b1 > 2)
                    index7 += b1;
                  else
                    continue;
                }
                // ISSUE: variable of a compiler-generated type
                OutsideConnectionSystem.NodeData nodeData1_1 = nativeList[index7];
                // ISSUE: variable of a compiler-generated type
                OutsideConnectionSystem.NodeData nodeData2_1 = nativeList[index8];
                // ISSUE: reference to a compiler-generated method
                this.TryCreateLane(nodeData1_1, nodeData2_1, laneMap);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float b2 = nodeData2_1.m_Remoteness - nodeData1_1.m_Remoteness;
                bool2 bool2 = new bool2((double) b2 <= 0.0, (double) b2 >= 0.0);
                float2 a = math.select(new float2(float.MinValue, float.MaxValue), (float2) b2, bool2);
                for (int index9 = 1; index9 < num1 && (double) b2 != 0.0; ++index9)
                {
                  int num2 = index6 + index9;
                  int index10 = num2 - math.select(0, b1, num2 >= index4);
                  // ISSUE: variable of a compiler-generated type
                  OutsideConnectionSystem.NodeData nodeData2_2 = nativeList[index10];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  b2 = nodeData2_2.m_Remoteness - nodeData1_1.m_Remoteness;
                  bool2 = new bool2((double) b2 <= 0.0, (double) b2 >= 0.0) & new bool2((double) b2 > (double) a.x, (double) b2 < (double) a.y);
                  if (math.any(bool2))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.TryCreateLane(nodeData1_1, nodeData2_2, laneMap);
                    a = math.select(a, (float2) b2, bool2);
                  }
                }
              }
            }
          }
        }
        if (laneMap.Count() != 0)
        {
          NativeArray<Entity> valueArray = laneMap.GetValueArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
          for (int index11 = 0; index11 < valueArray.Length; ++index11)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(valueArray[index11], new Deleted());
          }
          valueArray.Dispose();
          // ISSUE: reference to a compiler-generated field
          for (int index12 = 0; index12 < this.m_ConnectionChunks.Length; ++index12)
          {
            // ISSUE: reference to a compiler-generated field
            ArchetypeChunk connectionChunk = this.m_ConnectionChunks[index12];
            // ISSUE: reference to a compiler-generated field
            if (connectionChunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType))
            {
              // ISSUE: reference to a compiler-generated field
              BufferAccessor<ConnectedRoute> bufferAccessor = connectionChunk.GetBufferAccessor<ConnectedRoute>(ref this.m_ConnectedRouteType);
              for (int index13 = 0; index13 < bufferAccessor.Length; ++index13)
              {
                DynamicBuffer<ConnectedRoute> dynamicBuffer = bufferAccessor[index13];
                for (int index14 = 0; index14 < dynamicBuffer.Length; ++index14)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Updated>(dynamicBuffer[index14].m_Waypoint, new Updated());
                }
              }
            }
          }
        }
        laneMap.Dispose();
        nativeList.Dispose();
      }

      private void FillNodeData(
        Entity owner,
        Transform transform,
        OutsideConnectionData outsideConnectionData,
        RouteConnectionData routeConnectionData,
        NativeList<OutsideConnectionSystem.NodeData> buffer)
      {
        int num1 = 0;
        if (routeConnectionData.m_AccessConnectionType == RouteConnectionType.Road)
        {
          // ISSUE: variable of a compiler-generated type
          OutsideConnectionSystem.NodeData nodeData;
          // ISSUE: reference to a compiler-generated field
          nodeData.m_Position1 = transform.m_Position;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          nodeData.m_Position2 = this.CalculateEndPos(nodeData.m_Position1);
          ref OutsideConnectionSystem.NodeData local1 = ref nodeData;
          Entity owner1 = owner;
          int num2 = num1;
          int num3 = num2 + 1;
          int laneIndex1 = (int) (ushort) num2;
          PathNode pathNode1 = new PathNode(owner1, (ushort) laneIndex1);
          // ISSUE: reference to a compiler-generated field
          local1.m_Node1 = pathNode1;
          ref OutsideConnectionSystem.NodeData local2 = ref nodeData;
          Entity owner2 = owner;
          int num4 = num3;
          int num5 = num4 + 1;
          int laneIndex2 = (int) (ushort) num4;
          PathNode pathNode2 = new PathNode(owner2, (ushort) laneIndex2);
          // ISSUE: reference to a compiler-generated field
          local2.m_Node2 = pathNode2;
          ref OutsideConnectionSystem.NodeData local3 = ref nodeData;
          Entity owner3 = owner;
          int num6 = num5;
          num1 = num6 + 1;
          int laneIndex3 = (int) (ushort) num6;
          PathNode pathNode3 = new PathNode(owner3, (ushort) laneIndex3);
          // ISSUE: reference to a compiler-generated field
          local3.m_Node3 = pathNode3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          nodeData.m_Order = math.atan2(nodeData.m_Position1.z, nodeData.m_Position1.x);
          // ISSUE: reference to a compiler-generated field
          nodeData.m_Remoteness = outsideConnectionData.m_Remoteness;
          // ISSUE: reference to a compiler-generated field
          nodeData.m_Owner = owner;
          // ISSUE: reference to a compiler-generated field
          nodeData.m_ConnectionType = OutsideConnectionSystem.ConnectionType.Road;
          // ISSUE: reference to a compiler-generated field
          nodeData.m_TrackType = TrackTypes.None;
          // ISSUE: reference to a compiler-generated field
          nodeData.m_RoadType = routeConnectionData.m_AccessRoadType;
          buffer.Add(in nodeData);
        }
        if (routeConnectionData.m_AccessConnectionType != RouteConnectionType.Road)
          return;
        // ISSUE: variable of a compiler-generated type
        OutsideConnectionSystem.NodeData nodeData1;
        // ISSUE: reference to a compiler-generated field
        nodeData1.m_Position1 = transform.m_Position;
        // ISSUE: reference to a compiler-generated field
        nodeData1.m_Position1.y += 2f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        nodeData1.m_Position2 = this.CalculateEndPos(nodeData1.m_Position1);
        ref OutsideConnectionSystem.NodeData local4 = ref nodeData1;
        Entity owner4 = owner;
        int num7 = num1;
        int num8 = num7 + 1;
        int laneIndex4 = (int) (ushort) num7;
        PathNode pathNode4 = new PathNode(owner4, (ushort) laneIndex4);
        // ISSUE: reference to a compiler-generated field
        local4.m_Node1 = pathNode4;
        ref OutsideConnectionSystem.NodeData local5 = ref nodeData1;
        Entity owner5 = owner;
        int num9 = num8;
        int num10 = num9 + 1;
        int laneIndex5 = (int) (ushort) num9;
        PathNode pathNode5 = new PathNode(owner5, (ushort) laneIndex5);
        // ISSUE: reference to a compiler-generated field
        local5.m_Node2 = pathNode5;
        ref OutsideConnectionSystem.NodeData local6 = ref nodeData1;
        Entity owner6 = owner;
        int num11 = num10;
        int num12 = num11 + 1;
        int laneIndex6 = (int) (ushort) num11;
        PathNode pathNode6 = new PathNode(owner6, (ushort) laneIndex6);
        // ISSUE: reference to a compiler-generated field
        local6.m_Node3 = pathNode6;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        nodeData1.m_Order = math.atan2(nodeData1.m_Position1.z, nodeData1.m_Position1.x);
        // ISSUE: reference to a compiler-generated field
        nodeData1.m_Remoteness = outsideConnectionData.m_Remoteness;
        // ISSUE: reference to a compiler-generated field
        nodeData1.m_Owner = owner;
        // ISSUE: reference to a compiler-generated field
        nodeData1.m_ConnectionType = OutsideConnectionSystem.ConnectionType.Pedestrian;
        // ISSUE: reference to a compiler-generated field
        nodeData1.m_TrackType = TrackTypes.None;
        // ISSUE: reference to a compiler-generated field
        nodeData1.m_RoadType = RoadTypes.None;
        buffer.Add(in nodeData1);
      }

      private void FillNodeData(
        Entity node,
        DynamicBuffer<ConnectedEdge> connectedEdges,
        OutsideConnectionData outsideConnectionData,
        NativeList<OutsideConnectionSystem.NodeData> buffer)
      {
        int length1 = buffer.Length;
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        float3 float3_1 = new float3();
        for (int index1 = 0; index1 < connectedEdges.Length; ++index1)
        {
          ConnectedEdge connectedEdge = connectedEdges[index1];
          // ISSUE: reference to a compiler-generated field
          bool c = this.m_EdgeData[connectedEdge.m_Edge].m_End == node;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_UpdatedData.HasComponent(connectedEdge.m_Edge) && this.m_SubLanes.HasBuffer(connectedEdge.m_Edge))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<SubLane> subLane1 = this.m_SubLanes[connectedEdge.m_Edge];
            float num5 = math.select(0.0f, 1f, c);
            for (int index2 = 0; index2 < subLane1.Length; ++index2)
            {
              Entity subLane2 = subLane1[index2].m_SubLane;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_EdgeLaneData.HasComponent(subLane2) && !this.m_SecondaryLaneData.HasComponent(subLane2) && !this.m_SlaveLaneData.HasComponent(subLane2))
              {
                // ISSUE: reference to a compiler-generated field
                bool2 x = this.m_EdgeLaneData[subLane2].m_EdgeDelta == num5;
                if (math.any(x))
                {
                  bool y = x.y;
                  // ISSUE: reference to a compiler-generated field
                  Curve curve = this.m_CurveData[subLane2];
                  if (y)
                    curve.m_Bezier = MathUtils.Invert(curve.m_Bezier);
                  // ISSUE: reference to a compiler-generated field
                  PrefabRef prefabRef = this.m_PrefabRefData[subLane2];
                  // ISSUE: reference to a compiler-generated field
                  NetLaneData netLaneData = this.m_PrefabLaneData[prefabRef.m_Prefab];
                  // ISSUE: reference to a compiler-generated field
                  if ((this.m_MasterLaneData.HasComponent(subLane2) || (netLaneData.m_Flags & (y ? LaneFlags.DisconnectedEnd : LaneFlags.DisconnectedStart)) == (LaneFlags) 0) && (netLaneData.m_Flags & (LaneFlags.Parking | LaneFlags.Utility | LaneFlags.FindAnchor)) == (LaneFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    Lane lane = this.m_LaneData[subLane2];
                    byte laneIndex1 = !y ? (byte) ((uint) lane.m_StartNode.GetLaneIndex() & (uint) byte.MaxValue) : (byte) ((uint) lane.m_EndNode.GetLaneIndex() & (uint) byte.MaxValue);
                    // ISSUE: variable of a compiler-generated type
                    OutsideConnectionSystem.NodeData nodeData;
                    // ISSUE: reference to a compiler-generated field
                    nodeData.m_Position1 = curve.m_Bezier.a;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    nodeData.m_Position2 = this.CalculateEndPos(nodeData.m_Position1);
                    // ISSUE: reference to a compiler-generated field
                    nodeData.m_Node1 = new PathNode(connectedEdge.m_Edge, laneIndex1, (byte) math.select(0, 4, c));
                    ref OutsideConnectionSystem.NodeData local1 = ref nodeData;
                    Entity owner1 = node;
                    int num6 = num1;
                    int num7 = num6 + 1;
                    int laneIndex2 = (int) (ushort) num6;
                    PathNode pathNode1 = new PathNode(owner1, (ushort) laneIndex2);
                    // ISSUE: reference to a compiler-generated field
                    local1.m_Node2 = pathNode1;
                    ref OutsideConnectionSystem.NodeData local2 = ref nodeData;
                    Entity owner2 = node;
                    int num8 = num7;
                    num1 = num8 + 1;
                    int laneIndex3 = (int) (ushort) num8;
                    PathNode pathNode2 = new PathNode(owner2, (ushort) laneIndex3);
                    // ISSUE: reference to a compiler-generated field
                    local2.m_Node3 = pathNode2;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    nodeData.m_Order = math.atan2(nodeData.m_Position1.z, nodeData.m_Position1.x);
                    // ISSUE: reference to a compiler-generated field
                    nodeData.m_Remoteness = outsideConnectionData.m_Remoteness;
                    // ISSUE: reference to a compiler-generated field
                    nodeData.m_Owner = node;
                    if ((netLaneData.m_Flags & LaneFlags.Track) != (LaneFlags) 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      TrackLaneData trackLaneData = this.m_PrefabTrackLaneData[prefabRef.m_Prefab];
                      // ISSUE: reference to a compiler-generated field
                      nodeData.m_ConnectionType = OutsideConnectionSystem.ConnectionType.Track;
                      // ISSUE: reference to a compiler-generated field
                      nodeData.m_TrackType = trackLaneData.m_TrackTypes;
                      // ISSUE: reference to a compiler-generated field
                      nodeData.m_RoadType = RoadTypes.None;
                      ++num2;
                      // ISSUE: reference to a compiler-generated field
                      float3_1 += nodeData.m_Position1;
                    }
                    else if ((netLaneData.m_Flags & LaneFlags.Road) != (LaneFlags) 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      CarLaneData carLaneData = this.m_PrefabCarLaneData[prefabRef.m_Prefab];
                      // ISSUE: reference to a compiler-generated field
                      nodeData.m_ConnectionType = OutsideConnectionSystem.ConnectionType.Road;
                      // ISSUE: reference to a compiler-generated field
                      nodeData.m_TrackType = TrackTypes.None;
                      // ISSUE: reference to a compiler-generated field
                      nodeData.m_RoadType = carLaneData.m_RoadTypes;
                      ++num3;
                      // ISSUE: reference to a compiler-generated field
                      float3_1 += nodeData.m_Position1;
                    }
                    else if ((netLaneData.m_Flags & LaneFlags.Pedestrian) != (LaneFlags) 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      nodeData.m_ConnectionType = OutsideConnectionSystem.ConnectionType.Pedestrian;
                      // ISSUE: reference to a compiler-generated field
                      nodeData.m_TrackType = TrackTypes.None;
                      // ISSUE: reference to a compiler-generated field
                      nodeData.m_RoadType = RoadTypes.None;
                      ++num4;
                      // ISSUE: reference to a compiler-generated field
                      float3_1 += nodeData.m_Position1;
                    }
                    else
                      continue;
                    buffer.Add(in nodeData);
                  }
                }
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            Composition composition = this.m_CompositionData[connectedEdge.m_Edge];
            // ISSUE: reference to a compiler-generated field
            EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[connectedEdge.m_Edge];
            // ISSUE: reference to a compiler-generated field
            NetCompositionData netCompositionData = this.m_PrefabCompositionData[composition.m_Edge];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<NetCompositionLane> prefabCompositionLane = this.m_PrefabCompositionLanes[composition.m_Edge];
            if (c)
            {
              edgeGeometry.m_Start.m_Left = MathUtils.Invert(edgeGeometry.m_End.m_Right);
              edgeGeometry.m_Start.m_Right = MathUtils.Invert(edgeGeometry.m_End.m_Left);
            }
            for (int index3 = 0; index3 < prefabCompositionLane.Length; ++index3)
            {
              NetCompositionLane netCompositionLane = prefabCompositionLane[index3];
              bool flag = c == ((netCompositionLane.m_Flags & LaneFlags.Invert) == (LaneFlags) 0);
              if ((netCompositionLane.m_Flags & (LaneFlags.Slave | LaneFlags.Parking | LaneFlags.Utility)) == (LaneFlags) 0 && (netCompositionLane.m_Flags & (flag ? LaneFlags.DisconnectedEnd : LaneFlags.DisconnectedStart)) == (LaneFlags) 0)
              {
                float t = (float) ((double) netCompositionLane.m_Position.x / (double) math.max(1f, netCompositionData.m_Width) + 0.5);
                if (!c)
                  t = 1f - t;
                Bezier4x3 bezier4x3 = MathUtils.Lerp(edgeGeometry.m_Start.m_Right, edgeGeometry.m_Start.m_Left, t);
                // ISSUE: variable of a compiler-generated type
                OutsideConnectionSystem.NodeData nodeData;
                // ISSUE: reference to a compiler-generated field
                nodeData.m_Position1 = bezier4x3.a;
                // ISSUE: reference to a compiler-generated field
                nodeData.m_Position1.y += netCompositionLane.m_Position.y;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                nodeData.m_Position2 = this.CalculateEndPos(nodeData.m_Position1);
                // ISSUE: reference to a compiler-generated field
                nodeData.m_Node1 = new PathNode(connectedEdge.m_Edge, netCompositionLane.m_Index, (byte) math.select(0, 4, c));
                ref OutsideConnectionSystem.NodeData local3 = ref nodeData;
                Entity owner3 = node;
                int num9 = num1;
                int num10 = num9 + 1;
                int laneIndex4 = (int) (ushort) num9;
                PathNode pathNode3 = new PathNode(owner3, (ushort) laneIndex4);
                // ISSUE: reference to a compiler-generated field
                local3.m_Node2 = pathNode3;
                ref OutsideConnectionSystem.NodeData local4 = ref nodeData;
                Entity owner4 = node;
                int num11 = num10;
                num1 = num11 + 1;
                int laneIndex5 = (int) (ushort) num11;
                PathNode pathNode4 = new PathNode(owner4, (ushort) laneIndex5);
                // ISSUE: reference to a compiler-generated field
                local4.m_Node3 = pathNode4;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                nodeData.m_Order = math.atan2(nodeData.m_Position1.z, nodeData.m_Position1.x);
                // ISSUE: reference to a compiler-generated field
                nodeData.m_Remoteness = outsideConnectionData.m_Remoteness;
                // ISSUE: reference to a compiler-generated field
                nodeData.m_Owner = node;
                if ((netCompositionLane.m_Flags & LaneFlags.Track) != (LaneFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  TrackLaneData trackLaneData = this.m_PrefabTrackLaneData[netCompositionLane.m_Lane];
                  // ISSUE: reference to a compiler-generated field
                  nodeData.m_ConnectionType = OutsideConnectionSystem.ConnectionType.Track;
                  // ISSUE: reference to a compiler-generated field
                  nodeData.m_TrackType = trackLaneData.m_TrackTypes;
                  // ISSUE: reference to a compiler-generated field
                  nodeData.m_RoadType = RoadTypes.None;
                  ++num2;
                  // ISSUE: reference to a compiler-generated field
                  float3_1 += nodeData.m_Position1;
                }
                else if ((netCompositionLane.m_Flags & LaneFlags.Road) != (LaneFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  CarLaneData carLaneData = this.m_PrefabCarLaneData[netCompositionLane.m_Lane];
                  // ISSUE: reference to a compiler-generated field
                  nodeData.m_ConnectionType = OutsideConnectionSystem.ConnectionType.Road;
                  // ISSUE: reference to a compiler-generated field
                  nodeData.m_TrackType = TrackTypes.None;
                  // ISSUE: reference to a compiler-generated field
                  nodeData.m_RoadType = carLaneData.m_RoadTypes;
                  ++num3;
                  // ISSUE: reference to a compiler-generated field
                  float3_1 += nodeData.m_Position1;
                }
                else if ((netCompositionLane.m_Flags & LaneFlags.Pedestrian) != (LaneFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  nodeData.m_ConnectionType = OutsideConnectionSystem.ConnectionType.Pedestrian;
                  // ISSUE: reference to a compiler-generated field
                  nodeData.m_TrackType = TrackTypes.None;
                  // ISSUE: reference to a compiler-generated field
                  nodeData.m_RoadType = RoadTypes.None;
                  ++num4;
                  // ISSUE: reference to a compiler-generated field
                  float3_1 += nodeData.m_Position1;
                }
                else
                  continue;
                buffer.Add(in nodeData);
              }
            }
          }
        }
        if (num4 == 0 && (num2 != 0 || num3 != 0))
        {
          float3 float3_2 = float3_1 / (float) (num2 + num3);
          // ISSUE: variable of a compiler-generated type
          OutsideConnectionSystem.NodeData nodeData;
          // ISSUE: reference to a compiler-generated field
          nodeData.m_Position1 = float3_2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          nodeData.m_Position2 = this.CalculateEndPos(nodeData.m_Position1);
          ref OutsideConnectionSystem.NodeData local5 = ref nodeData;
          Entity owner5 = node;
          int num12 = num1;
          int num13 = num12 + 1;
          int laneIndex6 = (int) (ushort) num12;
          PathNode pathNode5 = new PathNode(owner5, (ushort) laneIndex6);
          // ISSUE: reference to a compiler-generated field
          local5.m_Node1 = pathNode5;
          ref OutsideConnectionSystem.NodeData local6 = ref nodeData;
          Entity owner6 = node;
          int num14 = num13;
          int num15 = num14 + 1;
          int laneIndex7 = (int) (ushort) num14;
          PathNode pathNode6 = new PathNode(owner6, (ushort) laneIndex7);
          // ISSUE: reference to a compiler-generated field
          local6.m_Node2 = pathNode6;
          ref OutsideConnectionSystem.NodeData local7 = ref nodeData;
          Entity owner7 = node;
          int num16 = num15;
          num1 = num16 + 1;
          int laneIndex8 = (int) (ushort) num16;
          PathNode pathNode7 = new PathNode(owner7, (ushort) laneIndex8);
          // ISSUE: reference to a compiler-generated field
          local7.m_Node3 = pathNode7;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          nodeData.m_Order = math.atan2(nodeData.m_Position1.z, nodeData.m_Position1.x);
          // ISSUE: reference to a compiler-generated field
          nodeData.m_Remoteness = outsideConnectionData.m_Remoteness;
          // ISSUE: reference to a compiler-generated field
          nodeData.m_Owner = node;
          // ISSUE: reference to a compiler-generated field
          nodeData.m_ConnectionType = OutsideConnectionSystem.ConnectionType.Pedestrian;
          // ISSUE: reference to a compiler-generated field
          nodeData.m_TrackType = TrackTypes.None;
          // ISSUE: reference to a compiler-generated field
          nodeData.m_RoadType = RoadTypes.None;
          buffer.Add(in nodeData);
        }
        if (num3 == 0)
          return;
        int length2 = buffer.Length;
        for (int index4 = length1; index4 < length2; ++index4)
        {
          // ISSUE: variable of a compiler-generated type
          OutsideConnectionSystem.NodeData nodeData1 = buffer[index4];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (nodeData1.m_ConnectionType == OutsideConnectionSystem.ConnectionType.Road && nodeData1.m_RoadType == RoadTypes.Car)
          {
            float num17 = float.MaxValue;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            OutsideConnectionSystem.NodeData nodeData2 = new OutsideConnectionSystem.NodeData();
            bool flag = false;
            for (int index5 = length1; index5 < length2; ++index5)
            {
              // ISSUE: variable of a compiler-generated type
              OutsideConnectionSystem.NodeData nodeData3 = buffer[index5];
              // ISSUE: reference to a compiler-generated field
              if (nodeData3.m_ConnectionType == OutsideConnectionSystem.ConnectionType.Pedestrian)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float num18 = math.distance(nodeData1.m_Position1, nodeData3.m_Position1);
                if ((double) num18 < (double) num17)
                {
                  num17 = num18;
                  nodeData2 = nodeData3;
                  flag = true;
                }
              }
            }
            if (flag)
            {
              // ISSUE: variable of a compiler-generated type
              OutsideConnectionSystem.NodeData nodeData4;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              nodeData4.m_Position1 = nodeData1.m_Position2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              nodeData4.m_Position2 = nodeData2.m_Position2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              nodeData4.m_Node1 = new PathNode(nodeData1.m_Node2, 1f);
              // ISSUE: reference to a compiler-generated field
              nodeData4.m_Node2 = new PathNode(node, (ushort) num1++);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              nodeData4.m_Node3 = new PathNode(nodeData2.m_Node2, 1f);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              nodeData4.m_Order = math.atan2(nodeData4.m_Position1.z, nodeData4.m_Position1.x);
              // ISSUE: reference to a compiler-generated field
              nodeData4.m_Remoteness = outsideConnectionData.m_Remoteness;
              // ISSUE: reference to a compiler-generated field
              nodeData4.m_Owner = node;
              // ISSUE: reference to a compiler-generated field
              nodeData4.m_ConnectionType = OutsideConnectionSystem.ConnectionType.Parking;
              // ISSUE: reference to a compiler-generated field
              nodeData4.m_TrackType = TrackTypes.None;
              // ISSUE: reference to a compiler-generated field
              nodeData4.m_RoadType = RoadTypes.Car;
              buffer.Add(in nodeData4);
            }
          }
        }
      }

      private void TryCreateLane(
        OutsideConnectionSystem.NodeData nodeData,
        NativeParallelHashMap<OutsideConnectionSystem.LaneData, Entity> laneMap)
      {
        ConnectionLane connectionLane;
        connectionLane.m_AccessRestriction = Entity.Null;
        connectionLane.m_Flags = ConnectionLaneFlags.Start | ConnectionLaneFlags.Outside;
        // ISSUE: reference to a compiler-generated field
        connectionLane.m_TrackTypes = nodeData.m_TrackType;
        // ISSUE: reference to a compiler-generated field
        connectionLane.m_RoadTypes = nodeData.m_RoadType;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        OutsideConnectionSystem.ConnectionType connectionType = nodeData.m_ConnectionType;
        switch (connectionType)
        {
          case OutsideConnectionSystem.ConnectionType.Pedestrian:
            connectionLane.m_Flags |= ConnectionLaneFlags.Pedestrian | ConnectionLaneFlags.AllowMiddle | ConnectionLaneFlags.AllowCargo;
            break;
          case OutsideConnectionSystem.ConnectionType.Road:
            // ISSUE: reference to a compiler-generated field
            if (nodeData.m_RoadType == RoadTypes.Car)
            {
              connectionLane.m_Flags |= ConnectionLaneFlags.SecondaryStart | ConnectionLaneFlags.SecondaryEnd | ConnectionLaneFlags.Road | ConnectionLaneFlags.AllowMiddle;
              break;
            }
            connectionLane.m_Flags |= ConnectionLaneFlags.Road | ConnectionLaneFlags.AllowMiddle;
            break;
          case OutsideConnectionSystem.ConnectionType.Track:
            connectionLane.m_Flags |= ConnectionLaneFlags.Track | ConnectionLaneFlags.AllowMiddle;
            break;
          case OutsideConnectionSystem.ConnectionType.Parking:
            connectionLane.m_Flags |= ConnectionLaneFlags.SecondaryStart | ConnectionLaneFlags.Parking;
            break;
        }
        Entity entity1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        if (laneMap.TryGetValue(new OutsideConnectionSystem.LaneData(nodeData), out entity1) && this.m_ConnectionLaneData[entity1].Equals(connectionLane))
        {
          // ISSUE: reference to a compiler-generated method
          Curve curve1 = this.CalculateCurve(nodeData);
          // ISSUE: reference to a compiler-generated field
          Curve curve2 = this.m_CurveData[entity1];
          if (!curve1.m_Bezier.Equals(curve2.m_Bezier))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Curve>(entity1, curve1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(entity1, new Updated());
          }
          // ISSUE: object of a compiler-generated type is created
          laneMap.Remove(new OutsideConnectionSystem.LaneData(nodeData));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabEntities.Length == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          Entity prefabEntity = this.m_PrefabEntities[0];
          // ISSUE: reference to a compiler-generated field
          NetLaneArchetypeData laneArchetypeData = this.m_PrefabLaneArchetypeData[prefabEntity];
          Lane component;
          // ISSUE: reference to a compiler-generated field
          component.m_StartNode = nodeData.m_Node1;
          // ISSUE: reference to a compiler-generated field
          component.m_MiddleNode = nodeData.m_Node2;
          // ISSUE: reference to a compiler-generated field
          component.m_EndNode = nodeData.m_Node3;
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(laneArchetypeData.m_LaneArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(entity2, new PrefabRef(prefabEntity));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Lane>(entity2, component);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_CommandBuffer.SetComponent<Curve>(entity2, this.CalculateCurve(nodeData));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<ConnectionLane>(entity2, connectionLane);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OutsideConnection>(entity2, new OutsideConnection());
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Owner>(entity2, new Owner(nodeData.m_Owner));
        }
      }

      private void TryCreateLane(
        OutsideConnectionSystem.NodeData nodeData1,
        OutsideConnectionSystem.NodeData nodeData2,
        NativeParallelHashMap<OutsideConnectionSystem.LaneData, Entity> laneMap)
      {
        ConnectionLane connectionLane;
        connectionLane.m_AccessRestriction = Entity.Null;
        connectionLane.m_Flags = ConnectionLaneFlags.Distance | ConnectionLaneFlags.Outside;
        // ISSUE: reference to a compiler-generated field
        connectionLane.m_TrackTypes = nodeData1.m_TrackType;
        // ISSUE: reference to a compiler-generated field
        connectionLane.m_RoadTypes = nodeData1.m_RoadType;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        OutsideConnectionSystem.ConnectionType connectionType = nodeData1.m_ConnectionType;
        switch (connectionType)
        {
          case OutsideConnectionSystem.ConnectionType.Pedestrian:
            connectionLane.m_Flags |= ConnectionLaneFlags.Pedestrian;
            break;
          case OutsideConnectionSystem.ConnectionType.Road:
            // ISSUE: reference to a compiler-generated field
            if (nodeData1.m_RoadType == RoadTypes.Car)
            {
              connectionLane.m_Flags |= ConnectionLaneFlags.SecondaryStart | ConnectionLaneFlags.SecondaryEnd | ConnectionLaneFlags.Road;
              break;
            }
            connectionLane.m_Flags |= ConnectionLaneFlags.Road;
            break;
          case OutsideConnectionSystem.ConnectionType.Track:
            connectionLane.m_Flags |= ConnectionLaneFlags.Track;
            break;
        }
        Entity entity1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        if (laneMap.TryGetValue(new OutsideConnectionSystem.LaneData(nodeData1, nodeData2), out entity1) && this.m_ConnectionLaneData[entity1].Equals(connectionLane))
        {
          // ISSUE: reference to a compiler-generated method
          Curve curve1 = this.CalculateCurve(nodeData1, nodeData2);
          // ISSUE: reference to a compiler-generated field
          Curve curve2 = this.m_CurveData[entity1];
          if (!curve1.m_Bezier.Equals(curve2.m_Bezier))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Curve>(entity1, curve1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(entity1, new Updated());
          }
          // ISSUE: object of a compiler-generated type is created
          laneMap.Remove(new OutsideConnectionSystem.LaneData(nodeData1, nodeData2));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabEntities.Length == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          Entity prefabEntity = this.m_PrefabEntities[0];
          // ISSUE: reference to a compiler-generated field
          NetLaneArchetypeData laneArchetypeData = this.m_PrefabLaneArchetypeData[prefabEntity];
          Lane component;
          // ISSUE: reference to a compiler-generated field
          component.m_StartNode = nodeData1.m_Node3;
          component.m_MiddleNode = new PathNode();
          // ISSUE: reference to a compiler-generated field
          component.m_EndNode = nodeData2.m_Node3;
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(laneArchetypeData.m_LaneArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(entity2, new PrefabRef(prefabEntity));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Lane>(entity2, component);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_CommandBuffer.SetComponent<Curve>(entity2, this.CalculateCurve(nodeData1, nodeData2));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<ConnectionLane>(entity2, connectionLane);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OutsideConnection>(entity2, new OutsideConnection());
        }
      }

      private float3 CalculateEndPos(float3 startPos)
      {
        float3 endPos = startPos;
        float2 xz = startPos.xz;
        if (MathUtils.TryNormalize(ref xz, 10f))
          endPos.xz += xz;
        return endPos;
      }

      private Curve CalculateCurve(OutsideConnectionSystem.NodeData nodeData)
      {
        Curve curve;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        curve.m_Bezier = NetUtils.StraightCurve(nodeData.m_Position1, nodeData.m_Position2);
        curve.m_Length = 10f;
        return curve;
      }

      private Curve CalculateCurve(
        OutsideConnectionSystem.NodeData nodeData1,
        OutsideConnectionSystem.NodeData nodeData2)
      {
        // ISSUE: reference to a compiler-generated field
        float3 position2_1 = nodeData1.m_Position2;
        // ISSUE: reference to a compiler-generated field
        float3 position2_2 = nodeData2.m_Position2;
        float3 _b = math.lerp(position2_1, position2_2, 0.333333343f);
        float3 _c = math.lerp(position2_1, position2_2, 0.6666667f);
        float2 float2_1 = _b.xz;
        float2 float2_2 = _c.xz;
        float num = math.cmax(math.abs(position2_1.xz - position2_2.xz));
        float2 float2_3 = new float2(math.length(position2_1.xz), math.length(position2_2.xz));
        float2 float2_4 = math.lerp((float2) float2_3.x, (float2) float2_3.y, new float2(0.333333343f, 0.6666667f)) + num;
        if (!MathUtils.TryNormalize(ref float2_1, float2_4.x))
          float2_1 = new float2(0.0f, float2_4.x);
        if (!MathUtils.TryNormalize(ref float2_2, float2_4.y))
          float2_2 = new float2(0.0f, float2_4.y);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float newLength = (float) (50.0 + (double) math.abs(nodeData2.m_Remoteness - nodeData1.m_Remoteness) * 0.5);
        float2 float2_5 = position2_2.xz - position2_1.xz;
        float2 float2_6 = float2_1 + float2_1 * (newLength / math.max(1f, float2_4.x));
        float2 float2_7 = float2_2 + float2_2 * (newLength / math.max(1f, float2_4.y));
        if (MathUtils.TryNormalize(ref float2_5, newLength))
        {
          float2_6 -= float2_5;
          float2_7 += float2_5;
        }
        _b.xz = float2_6;
        _c.xz = float2_7;
        Curve curve;
        curve.m_Bezier = new Bezier4x3(position2_1, _b, _c, position2_2);
        curve.m_Length = MathUtils.Length(curve.m_Bezier);
        return curve;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Lane> __Game_Net_Lane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubLane> __Game_Net_SubLane_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ConnectedRoute> __Game_Routes_ConnectedRoute_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SecondaryLane> __Game_Net_SecondaryLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MasterLane> __Game_Net_MasterLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLaneData> __Game_Prefabs_CarLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> __Game_Prefabs_TrackLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> __Game_Prefabs_RouteConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneArchetypeData> __Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> __Game_Prefabs_OutsideConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetCompositionLane> __Game_Prefabs_NetCompositionLane_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_ConnectedRoute_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedRoute>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SecondaryLane_RO_ComponentLookup = state.GetComponentLookup<SecondaryLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_MasterLane_RO_ComponentLookup = state.GetComponentLookup<MasterLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarLaneData_RO_ComponentLookup = state.GetComponentLookup<CarLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackLaneData_RO_ComponentLookup = state.GetComponentLookup<TrackLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup = state.GetComponentLookup<RouteConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup = state.GetComponentLookup<NetLaneArchetypeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup = state.GetComponentLookup<OutsideConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionLane_RO_BufferLookup = state.GetBufferLookup<NetCompositionLane>(true);
      }
    }
  }
}
