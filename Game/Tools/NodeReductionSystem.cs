// Decompiled with JetBrains decompiler
// Type: Game.Tools.NodeReductionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class NodeReductionSystem : GameSystemBase
  {
    private ToolSystem m_ToolSystem;
    private EntityQuery m_TempNodeQuery;
    private NodeReductionSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TempNodeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Game.Net.Node>(), ComponentType.ReadOnly<Updated>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Fixed>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TempNodeQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<NodeReductionSystem.ReductionData> nativeQueue = new NativeQueue<NodeReductionSystem.ReductionData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Fixed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_OwnerDefinition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      NodeReductionSystem.FindCandidatesJob jobData = new NodeReductionSystem.FindCandidatesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_OwnerDefinitionData = this.__TypeHandle.__Game_Tools_OwnerDefinition_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_UpgradedData = this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup,
        m_FixedData = this.__TypeHandle.__Game_Net_Fixed_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_ReductionQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_BuildOrder_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      JobHandle inputDeps = new NodeReductionSystem.NodeReductionJob()
      {
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_LocalConnectData = this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RW_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RW_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RW_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RW_ComponentLookup,
        m_BuildOrderData = this.__TypeHandle.__Game_Net_BuildOrder_RW_ComponentLookup,
        m_RoadData = this.__TypeHandle.__Game_Net_Road_RW_ComponentLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RW_BufferLookup,
        m_ConnectedNodes = this.__TypeHandle.__Game_Net_ConnectedNode_RW_BufferLookup,
        m_ReductionQueue = nativeQueue
      }.Schedule<NodeReductionSystem.NodeReductionJob>(jobData.ScheduleParallel<NodeReductionSystem.FindCandidatesJob>(this.m_TempNodeQuery, this.Dependency));
      nativeQueue.Dispose(inputDeps);
      this.Dependency = inputDeps;
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
    public NodeReductionSystem()
    {
    }

    [BurstCompile]
    private struct FindCandidatesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<OwnerDefinition> m_OwnerDefinitionData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Upgraded> m_UpgradedData;
      [ReadOnly]
      public ComponentLookup<Fixed> m_FixedData;
      [ReadOnly]
      public ComponentLookup<Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public bool m_EditorMode;
      public NativeQueue<NodeReductionSystem.ReductionData>.ParallelWriter m_ReductionQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          NodeReductionSystem.ReductionData data;
          // ISSUE: reference to a compiler-generated method
          if (this.CanMove(nativeArray[index], out data))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ReductionQueue.Enqueue(data);
          }
        }
      }

      private bool CanMove(Entity node, out NodeReductionSystem.ReductionData data)
      {
        // ISSUE: object of a compiler-generated type is created
        data = new NodeReductionSystem.ReductionData()
        {
          m_Node = node
        };
        // ISSUE: reference to a compiler-generated field
        Temp temp = this.m_TempData[node];
        if (temp.m_Original == Entity.Null || (temp.m_Flags & (TempFlags.Delete | TempFlags.Replace | TempFlags.Upgrade)) != (TempFlags) 0)
          return false;
        Entity edge1 = Entity.Null;
        Entity edge2 = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Game.Net.EdgeIterator edgeIterator = new Game.Net.EdgeIterator(Entity.Null, node, this.m_ConnectedEdges, this.m_EdgeData, this.m_TempData, this.m_HiddenData, true);
        EdgeIteratorValue edgeIteratorValue;
        while (edgeIterator.GetNext(out edgeIteratorValue))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (edgeIteratorValue.m_Middle || !this.m_TempData.HasComponent(edgeIteratorValue.m_Edge) || this.m_FixedData.HasComponent(edgeIteratorValue.m_Edge))
            return false;
          if (edge1 == Entity.Null)
          {
            edge1 = edgeIteratorValue.m_Edge;
          }
          else
          {
            if (!(edge2 == Entity.Null))
              return false;
            edge2 = edgeIteratorValue.m_Edge;
          }
        }
        if (edge2 == Entity.Null)
          return false;
        // ISSUE: reference to a compiler-generated field
        Edge edge3 = this.m_EdgeData[edge1];
        // ISSUE: reference to a compiler-generated field
        Edge edge4 = this.m_EdgeData[edge2];
        bool2 bool2_1 = new bool2(edge3.m_Start == node, edge4.m_Start == node);
        bool2 bool2_2 = new bool2(edge3.m_End == node, edge4.m_End == node);
        if (math.any(bool2_1 == bool2_2))
          return false;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef1 = this.m_PrefabRefData[edge1];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef2 = this.m_PrefabRefData[edge2];
        if (prefabRef1.m_Prefab != prefabRef2.m_Prefab)
          return false;
        NetGeometryData prefabGeometryData = new NetGeometryData();
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabGeometryData.HasComponent(prefabRef1.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          prefabGeometryData = this.m_PrefabGeometryData[prefabRef1.m_Prefab];
          if (bool2_1.x == bool2_1.y && (prefabGeometryData.m_Flags & GeometryFlags.Asymmetric) != (GeometryFlags) 0)
            return false;
          // ISSUE: reference to a compiler-generated field
          data.m_EdgeLengthRange = prefabGeometryData.m_EdgeLengthRange;
          // ISSUE: reference to a compiler-generated field
          data.m_SnapCellSize = (prefabGeometryData.m_Flags & GeometryFlags.SnapCellSize) != 0;
          // ISSUE: reference to a compiler-generated field
          data.m_ForbidMove = (prefabGeometryData.m_Flags & GeometryFlags.StraightEdges) != 0;
          // ISSUE: reference to a compiler-generated field
          data.m_NoEdgeConnection = (prefabGeometryData.m_Flags & GeometryFlags.NoEdgeConnection) != 0;
        }
        // ISSUE: reference to a compiler-generated method
        CompositionFlags flags1 = this.GetElevationFlags(edge1, edge3.m_Start, edge3.m_End, prefabGeometryData);
        // ISSUE: reference to a compiler-generated method
        CompositionFlags flags2 = this.GetElevationFlags(edge2, edge4.m_Start, edge4.m_End, prefabGeometryData);
        if (bool2_1.x)
          flags1 = NetCompositionHelpers.InvertCompositionFlags(flags1);
        if (bool2_2.y)
          flags2 = NetCompositionHelpers.InvertCompositionFlags(flags2);
        CompositionFlags compositionFlags = new CompositionFlags(CompositionFlags.General.Elevated | CompositionFlags.General.Tunnel, CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered, CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered);
        if (((flags1 ^ flags2) & compositionFlags) != new CompositionFlags())
          return false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        data.m_EdgeLengthRange.max = math.select(data.m_EdgeLengthRange.max, prefabGeometryData.m_ElevatedLength, (flags1.m_General & CompositionFlags.General.Elevated) > (CompositionFlags.General) 0);
        // ISSUE: reference to a compiler-generated field
        data.m_ForbidMove |= (flags1.m_General & CompositionFlags.General.Elevated) > (CompositionFlags.General) 0;
        // ISSUE: reference to a compiler-generated field
        data.m_CheckHeight |= (flags1.m_General & compositionFlags.m_General) > (CompositionFlags.General) 0;
        // ISSUE: reference to a compiler-generated field
        ref bool local = ref data.m_CheckHeight;
        local = ((local ? 1 : 0) | ((flags1.m_Left & compositionFlags.m_Left) == (CompositionFlags.Side) 0 ? 0 : ((flags1.m_Right & compositionFlags.m_Right) > (CompositionFlags.Side) 0 ? 1 : 0))) != 0;
        Upgraded upgraded1 = new Upgraded();
        Upgraded upgraded2 = new Upgraded();
        // ISSUE: reference to a compiler-generated field
        if (this.m_UpgradedData.HasComponent(edge1))
        {
          // ISSUE: reference to a compiler-generated field
          upgraded1 = this.m_UpgradedData[edge1];
          if (bool2_1.x)
            upgraded1.m_Flags = NetCompositionHelpers.InvertCompositionFlags(upgraded1.m_Flags);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_UpgradedData.HasComponent(edge2))
        {
          // ISSUE: reference to a compiler-generated field
          upgraded2 = this.m_UpgradedData[edge2];
          if (bool2_2.y)
            upgraded2.m_Flags = NetCompositionHelpers.InvertCompositionFlags(upgraded2.m_Flags);
        }
        if (upgraded1.m_Flags != upgraded2.m_Flags)
          return false;
        Owner owner1 = new Owner();
        Owner owner2 = new Owner();
        Owner owner3 = new Owner();
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.HasComponent(node))
        {
          // ISSUE: reference to a compiler-generated field
          owner1 = this.m_OwnerData[node];
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.HasComponent(edge1))
        {
          // ISSUE: reference to a compiler-generated field
          owner2 = this.m_OwnerData[edge1];
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.HasComponent(edge2))
        {
          // ISSUE: reference to a compiler-generated field
          owner3 = this.m_OwnerData[edge2];
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_EditorMode)
        {
          if (owner2.m_Owner != owner1.m_Owner || owner3.m_Owner != owner1.m_Owner)
            return false;
        }
        else if (owner1.m_Owner != Entity.Null || owner2.m_Owner != Entity.Null || owner3.m_Owner != Entity.Null)
          return false;
        OwnerDefinition other = new OwnerDefinition();
        OwnerDefinition ownerDefinition1 = new OwnerDefinition();
        OwnerDefinition ownerDefinition2 = new OwnerDefinition();
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerDefinitionData.HasComponent(node))
        {
          // ISSUE: reference to a compiler-generated field
          other = this.m_OwnerDefinitionData[node];
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerDefinitionData.HasComponent(edge1))
        {
          // ISSUE: reference to a compiler-generated field
          ownerDefinition1 = this.m_OwnerDefinitionData[edge1];
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerDefinitionData.HasComponent(edge2))
        {
          // ISSUE: reference to a compiler-generated field
          ownerDefinition2 = this.m_OwnerDefinitionData[edge2];
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_EditorMode)
        {
          if (!ownerDefinition1.Equals(other) || !ownerDefinition2.Equals(other))
            return false;
        }
        else if (other.m_Prefab != Entity.Null || ownerDefinition1.m_Prefab != Entity.Null || ownerDefinition2.m_Prefab != Entity.Null)
          return false;
        // ISSUE: reference to a compiler-generated field
        Curve curve1 = this.m_CurveData[edge1];
        // ISSUE: reference to a compiler-generated field
        Curve curve2 = this.m_CurveData[edge2];
        if (bool2_1.x)
          curve1.m_Bezier = MathUtils.Invert(curve1.m_Bezier);
        if (bool2_2.y)
          curve2.m_Bezier = MathUtils.Invert(curve2.m_Bezier);
        // ISSUE: reference to a compiler-generated field
        return data.m_CheckHeight ? (double) math.dot(math.normalizesafe(MathUtils.EndTangent(curve1.m_Bezier)), math.normalizesafe(MathUtils.StartTangent(curve2.m_Bezier))) >= 0.99949997663497925 : (double) math.dot(math.normalizesafe(MathUtils.EndTangent(curve1.m_Bezier).xz), math.normalizesafe(MathUtils.StartTangent(curve2.m_Bezier).xz)) >= 0.99949997663497925;
      }

      private CompositionFlags GetElevationFlags(
        Entity edge,
        Entity startNode,
        Entity endNode,
        NetGeometryData prefabGeometryData)
      {
        Elevation startElevation = new Elevation();
        Elevation middleElevation = new Elevation();
        Elevation endElevation = new Elevation();
        // ISSUE: reference to a compiler-generated field
        if (this.m_ElevationData.HasComponent(startNode))
        {
          // ISSUE: reference to a compiler-generated field
          startElevation = this.m_ElevationData[startNode];
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ElevationData.HasComponent(edge))
        {
          // ISSUE: reference to a compiler-generated field
          middleElevation = this.m_ElevationData[edge];
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ElevationData.HasComponent(endNode))
        {
          // ISSUE: reference to a compiler-generated field
          endElevation = this.m_ElevationData[endNode];
        }
        return NetCompositionHelpers.GetElevationFlags(startElevation, middleElevation, endElevation, prefabGeometryData);
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

    private struct ReductionData
    {
      public Entity m_Node;
      public Bounds1 m_EdgeLengthRange;
      public bool m_SnapCellSize;
      public bool m_ForbidMove;
      public bool m_CheckHeight;
      public bool m_NoEdgeConnection;
    }

    [BurstCompile]
    private struct NodeReductionJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<LocalConnectData> m_LocalConnectData;
      public ComponentLookup<Game.Net.Node> m_NodeData;
      public ComponentLookup<Edge> m_EdgeData;
      public ComponentLookup<Curve> m_CurveData;
      public ComponentLookup<Temp> m_TempData;
      public ComponentLookup<BuildOrder> m_BuildOrderData;
      public ComponentLookup<Road> m_RoadData;
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      public BufferLookup<ConnectedNode> m_ConnectedNodes;
      public NativeQueue<NodeReductionSystem.ReductionData> m_ReductionQueue;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_ReductionQueue.Count;
        if (count == 0)
          return;
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          NodeReductionSystem.ReductionData data = this.m_ReductionQueue.Dequeue();
          // ISSUE: reference to a compiler-generated method
          this.TryReduceOrMove(data);
        }
      }

      private void TryReduceOrMove(NodeReductionSystem.ReductionData data)
      {
        Entity entity1 = Entity.Null;
        Entity entity2 = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[data.m_Node];
        for (int index = 0; index < connectedEdge.Length; ++index)
        {
          Entity edge = connectedEdge[index].m_Edge;
          // ISSUE: reference to a compiler-generated field
          if ((this.m_TempData[edge].m_Flags & TempFlags.Delete) == (TempFlags) 0)
          {
            if (entity1 == Entity.Null)
            {
              entity1 = edge;
            }
            else
            {
              if (!(entity2 == Entity.Null))
                return;
              entity2 = edge;
            }
          }
        }
        if (entity2 == Entity.Null)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Temp temp1 = this.m_TempData[data.m_Node];
        // ISSUE: reference to a compiler-generated field
        Temp temp2 = this.m_TempData[entity1];
        // ISSUE: reference to a compiler-generated field
        Temp temp3 = this.m_TempData[entity2];
        // ISSUE: reference to a compiler-generated field
        Edge edge1 = this.m_EdgeData[entity1];
        // ISSUE: reference to a compiler-generated field
        Edge edge2 = this.m_EdgeData[entity2];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool2 bool2_1 = new bool2(edge1.m_Start == data.m_Node, edge2.m_Start == data.m_Node);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool2 bool2_2 = new bool2(edge1.m_End == data.m_Node, edge2.m_End == data.m_Node);
        // ISSUE: reference to a compiler-generated field
        Curve curve1 = this.m_CurveData[entity1];
        // ISSUE: reference to a compiler-generated field
        Curve curve2 = this.m_CurveData[entity2];
        if (bool2_1.x)
          curve1.m_Bezier = MathUtils.Invert(curve1.m_Bezier);
        if (bool2_2.y)
          curve2.m_Bezier = MathUtils.Invert(curve2.m_Bezier);
        float num1 = MathUtils.Length(curve1.m_Bezier.xz);
        float num2 = MathUtils.Length(curve2.m_Bezier.xz);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((double) data.m_EdgeLengthRange.max == 0.0 || (double) num1 + (double) num2 <= (double) data.m_EdgeLengthRange.max)
        {
          bool flag = temp2.m_Original != Entity.Null == (temp3.m_Original != Entity.Null) ? (double) num1 < (double) num2 : temp2.m_Original != Entity.Null;
          Bezier4x3 curve3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (!this.TryJoinCurve(curve1.m_Bezier, curve2.m_Bezier, data.m_CheckHeight, out curve3))
            return;
          if (temp2.m_Original != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated method
            this.FixStartSlope(curve1.m_Bezier, ref curve3);
          }
          if (temp3.m_Original != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated method
            this.FixEndSlope(curve2.m_Bezier, ref curve3);
          }
          temp1.m_Flags = TempFlags.Delete | TempFlags.Hidden;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_TempData[data.m_Node] = temp1;
          if (flag)
          {
            if ((temp2.m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Replace)) != (TempFlags) 0)
              temp3.m_Flags |= TempFlags.Modify;
            temp3.m_Flags |= temp2.m_Flags & (TempFlags.Essential | TempFlags.Upgrade | TempFlags.Parent);
            if (temp3.m_Original != Entity.Null)
              temp3.m_Flags |= TempFlags.Combine;
            temp2.m_Flags = temp3.m_Flags & TempFlags.Essential | TempFlags.Delete | TempFlags.Hidden;
            if ((temp3.m_Flags & TempFlags.Essential) != (TempFlags) 0 && (temp3.m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Replace | TempFlags.Upgrade)) != (TempFlags) 0)
              temp2.m_Flags |= TempFlags.RemoveCost;
            curve2.m_Bezier = curve3;
            curve2.m_Length = MathUtils.Length(curve3);
            if (bool2_2.y)
            {
              curve2.m_Bezier = MathUtils.Invert(curve2.m_Bezier);
              // ISSUE: reference to a compiler-generated method
              this.ReplaceEdgeConnection(ref edge2.m_End, entity2, bool2_1.x ? edge1.m_End : edge1.m_Start);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.ReplaceEdgeConnection(ref edge2.m_Start, entity2, bool2_1.x ? edge1.m_End : edge1.m_Start);
            }
            // ISSUE: reference to a compiler-generated method
            this.ReplaceEdgeData(entity1, entity2, bool2_2.x, bool2_1.y);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.MoveConnectedNodes(entity1, entity2, curve2, data.m_NoEdgeConnection);
            // ISSUE: reference to a compiler-generated field
            this.m_CurveData[entity2] = curve2;
            // ISSUE: reference to a compiler-generated field
            this.m_EdgeData[entity2] = edge2;
          }
          else
          {
            if ((temp3.m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Replace)) != (TempFlags) 0)
              temp2.m_Flags |= TempFlags.Modify;
            temp2.m_Flags |= temp3.m_Flags & (TempFlags.Essential | TempFlags.Upgrade | TempFlags.Parent);
            if (temp2.m_Original != Entity.Null)
              temp2.m_Flags |= TempFlags.Combine;
            temp3.m_Flags = temp2.m_Flags & TempFlags.Essential | TempFlags.Delete | TempFlags.Hidden;
            if ((temp2.m_Flags & TempFlags.Essential) != (TempFlags) 0 && (temp2.m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Replace | TempFlags.Upgrade)) != (TempFlags) 0)
              temp3.m_Flags |= TempFlags.RemoveCost;
            curve1.m_Bezier = curve3;
            curve1.m_Length = MathUtils.Length(curve3);
            if (bool2_1.x)
            {
              curve1.m_Bezier = MathUtils.Invert(curve1.m_Bezier);
              // ISSUE: reference to a compiler-generated method
              this.ReplaceEdgeConnection(ref edge1.m_Start, entity1, bool2_2.y ? edge2.m_Start : edge2.m_End);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.ReplaceEdgeConnection(ref edge1.m_End, entity1, bool2_2.y ? edge2.m_Start : edge2.m_End);
            }
            // ISSUE: reference to a compiler-generated method
            this.ReplaceEdgeData(entity2, entity1, bool2_2.y, bool2_1.x);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.MoveConnectedNodes(entity2, entity1, curve1, data.m_NoEdgeConnection);
            // ISSUE: reference to a compiler-generated field
            this.m_CurveData[entity1] = curve1;
            // ISSUE: reference to a compiler-generated field
            this.m_EdgeData[entity1] = edge1;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_TempData[entity1] = temp2;
          // ISSUE: reference to a compiler-generated field
          this.m_TempData[entity2] = temp3;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (data.m_ForbidMove || (double) num1 >= (double) data.m_EdgeLengthRange.max * 0.5 && (double) num2 >= (double) data.m_EdgeLengthRange.max * 0.5)
            return;
          float length = math.abs(num1 - num2) * 0.5f;
          // ISSUE: reference to a compiler-generated field
          if (data.m_SnapCellSize)
            length = MathUtils.Snap(length - 1f, 8f);
          if ((double) length < 1.0)
            return;
          if ((double) num1 >= (double) num2)
          {
            Bounds1 t = new Bounds1(0.0f, 1f);
            MathUtils.ClampLengthInverse(curve1.m_Bezier.xz, ref t, length);
            Bezier4x3 output1;
            Bezier4x3 output2;
            MathUtils.Divide(curve1.m_Bezier, out output1, out output2, t.min);
            Bezier4x3 curve4;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (!this.TryJoinCurve(output2, curve2.m_Bezier, data.m_CheckHeight, out curve4))
              return;
            if (temp3.m_Original != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated method
              this.FixEndSlope(curve2.m_Bezier, ref curve4);
            }
            curve1.m_Bezier = output1;
            curve2.m_Bezier = curve4;
            if ((temp3.m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Replace | TempFlags.Parent)) == (TempFlags) 0 && (temp2.m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Replace | TempFlags.Parent)) != (TempFlags) 0)
              temp3.m_Flags |= (temp2.m_Flags & TempFlags.Parent) != (TempFlags) 0 ? TempFlags.Parent : TempFlags.Modify;
          }
          else
          {
            Bounds1 t = new Bounds1(0.0f, 1f);
            MathUtils.ClampLength(curve2.m_Bezier.xz, ref t, length);
            Bezier4x3 output1;
            Bezier4x3 output2;
            MathUtils.Divide(curve2.m_Bezier, out output1, out output2, t.max);
            Bezier4x3 curve5;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (!this.TryJoinCurve(curve1.m_Bezier, output1, data.m_CheckHeight, out curve5))
              return;
            if (temp2.m_Original != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated method
              this.FixStartSlope(curve1.m_Bezier, ref curve5);
            }
            curve2.m_Bezier = output2;
            curve1.m_Bezier = curve5;
            if ((temp2.m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Replace | TempFlags.Parent)) == (TempFlags) 0 && (temp3.m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Replace | TempFlags.Parent)) != (TempFlags) 0)
              temp2.m_Flags |= (temp3.m_Flags & TempFlags.Parent) != (TempFlags) 0 ? TempFlags.Parent : TempFlags.Modify;
          }
          bool flag1 = (temp2.m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Replace | TempFlags.Upgrade)) > (TempFlags) 0;
          bool flag2 = (temp3.m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Replace | TempFlags.Upgrade)) > (TempFlags) 0;
          if ((temp2.m_Flags & TempFlags.Essential) > (TempFlags) 0 & flag1 && !flag2)
            temp3.m_Flags |= TempFlags.RemoveCost;
          if ((temp3.m_Flags & TempFlags.Essential) > (TempFlags) 0 & flag2 && !flag1)
            temp2.m_Flags |= TempFlags.RemoveCost;
          if (bool2_1.x)
            curve1.m_Bezier = MathUtils.Invert(curve1.m_Bezier);
          if (bool2_2.y)
            curve2.m_Bezier = MathUtils.Invert(curve2.m_Bezier);
          curve1.m_Length = MathUtils.Length(curve1.m_Bezier);
          curve2.m_Length = MathUtils.Length(curve2.m_Bezier);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.MoveConnectedNodes(entity1, entity2, curve1, curve2, data.m_NoEdgeConnection);
          // ISSUE: reference to a compiler-generated field
          this.m_CurveData[entity1] = curve1;
          // ISSUE: reference to a compiler-generated field
          this.m_CurveData[entity2] = curve2;
          temp2.m_Flags |= temp3.m_Flags & TempFlags.Essential;
          temp3.m_Flags |= temp2.m_Flags & TempFlags.Essential;
          // ISSUE: reference to a compiler-generated field
          this.m_TempData[entity1] = temp2;
          // ISSUE: reference to a compiler-generated field
          this.m_TempData[entity2] = temp3;
        }
      }

      private bool TryJoinCurve(
        Bezier4x3 curve1,
        Bezier4x3 curve2,
        bool checkHeight,
        out Bezier4x3 curve)
      {
        curve = MathUtils.Join(curve1, curve2);
        float4 float4;
        if (checkHeight)
        {
          float t;
          float4.x = MathUtils.Distance(curve, MathUtils.Position(curve1, 0.5f), out t);
          float4.y = MathUtils.Distance(curve, curve1.d, out t);
          float4.z = MathUtils.Distance(curve, curve2.a, out t);
          float4.w = MathUtils.Distance(curve, MathUtils.Position(curve2, 0.5f), out t);
        }
        else
        {
          float t;
          float4.x = MathUtils.Distance(curve.xz, MathUtils.Position(curve1, 0.5f).xz, out t);
          float4.y = MathUtils.Distance(curve.xz, curve1.d.xz, out t);
          float4.z = MathUtils.Distance(curve.xz, curve2.a.xz, out t);
          float4.w = MathUtils.Distance(curve.xz, MathUtils.Position(curve2, 0.5f).xz, out t);
          // ISSUE: reference to a compiler-generated method
          float heightOffset1 = this.FindHeightOffset(curve1, curve2, MathUtils.Position(curve, 0.333333343f));
          // ISSUE: reference to a compiler-generated method
          float heightOffset2 = this.FindHeightOffset(curve1, curve2, MathUtils.Position(curve, 0.6666667f));
          curve.b.y += (float) ((double) heightOffset1 * 3.0 - (double) heightOffset2 * 1.5);
          curve.c.y += (float) ((double) heightOffset2 * 3.0 - (double) heightOffset1 * 1.5);
        }
        return math.all(float4 < 0.1f);
      }

      private void FixStartSlope(Bezier4x3 originalCurve, ref Bezier4x3 newCurve)
      {
        newCurve.b.y = originalCurve.b.y;
      }

      private void FixEndSlope(Bezier4x3 originalCurve, ref Bezier4x3 newCurve)
      {
        newCurve.c.y = originalCurve.c.y;
      }

      private float FindHeightOffset(Bezier4x3 curve1, Bezier4x3 curve2, float3 position)
      {
        float t1;
        float t2;
        return (double) MathUtils.Distance(curve1.xz, position.xz, out t1) < (double) MathUtils.Distance(curve2.xz, position.xz, out t2) ? MathUtils.Position(curve1, t1).y - position.y : MathUtils.Position(curve2, t2).y - position.y;
      }

      private void ReplaceEdgeConnection(ref Entity node, Entity edge, Entity newNode)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_TempData.HasComponent(node))
        {
          // ISSUE: reference to a compiler-generated field
          CollectionUtils.RemoveValue<ConnectedEdge>(this.m_ConnectedEdges[node], new ConnectedEdge(edge));
        }
        node = newNode;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TempData.HasComponent(node))
          return;
        // ISSUE: reference to a compiler-generated field
        CollectionUtils.TryAddUniqueValue<ConnectedEdge>(this.m_ConnectedEdges[node], new ConnectedEdge(edge));
      }

      private void ReplaceEdgeData(
        Entity source,
        Entity target,
        bool sourceStart,
        bool targetStart)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_RoadData.HasComponent(source) && this.m_RoadData.HasComponent(target))
        {
          // ISSUE: reference to a compiler-generated field
          Road road1 = this.m_RoadData[source];
          // ISSUE: reference to a compiler-generated field
          Road road2 = this.m_RoadData[target];
          if ((road1.m_Flags & (sourceStart ? Game.Net.RoadFlags.StartHalfAligned : Game.Net.RoadFlags.EndHalfAligned)) != (Game.Net.RoadFlags) 0)
            road2.m_Flags |= targetStart ? Game.Net.RoadFlags.StartHalfAligned : Game.Net.RoadFlags.EndHalfAligned;
          else
            road2.m_Flags &= (Game.Net.RoadFlags) ~(targetStart ? 1 : 2);
          // ISSUE: reference to a compiler-generated field
          this.m_RoadData[target] = road2;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_BuildOrderData.HasComponent(source) || !this.m_BuildOrderData.HasComponent(target))
          return;
        // ISSUE: reference to a compiler-generated field
        BuildOrder buildOrder1 = this.m_BuildOrderData[source];
        // ISSUE: reference to a compiler-generated field
        BuildOrder buildOrder2 = this.m_BuildOrderData[target];
        if (targetStart)
          buildOrder2.m_Start = sourceStart ? buildOrder1.m_Start : buildOrder1.m_End;
        else
          buildOrder2.m_End = sourceStart ? buildOrder1.m_Start : buildOrder1.m_End;
        // ISSUE: reference to a compiler-generated field
        this.m_BuildOrderData[target] = buildOrder2;
      }

      private void MoveConnectedNodes(
        Entity source,
        Entity target,
        Curve curve,
        bool noEdgeConnection)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedNode> connectedNode1 = this.m_ConnectedNodes[source];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedNode> connectedNode2 = this.m_ConnectedNodes[target];
        for (int index = 0; index < connectedNode2.Length; ++index)
        {
          ConnectedNode connectedNode3 = connectedNode2[index];
          // ISSUE: reference to a compiler-generated field
          Game.Net.Node node = this.m_NodeData[connectedNode3.m_Node];
          // ISSUE: reference to a compiler-generated method
          double distance = (double) this.GetDistance(curve, node, noEdgeConnection, out connectedNode3.m_CurvePosition);
          connectedNode2[index] = connectedNode3;
        }
        for (int index = 0; index < connectedNode1.Length; ++index)
        {
          ConnectedNode elem = connectedNode1[index];
          if (CollectionUtils.ContainsValue<ConnectedNode>(connectedNode2, elem))
          {
            // ISSUE: reference to a compiler-generated method
            this.RemoveConnectedEdge(elem.m_Node, source);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            Game.Net.Node node = this.m_NodeData[elem.m_Node];
            // ISSUE: reference to a compiler-generated method
            double distance = (double) this.GetDistance(curve, node, noEdgeConnection, out elem.m_CurvePosition);
            connectedNode2.Add(elem);
            // ISSUE: reference to a compiler-generated method
            this.SwitchConnectedEdge(elem.m_Node, source, target);
          }
        }
        connectedNode1.Clear();
      }

      private void MoveConnectedNodes(
        Entity edge1,
        Entity edge2,
        Curve curve1,
        Curve curve2,
        bool noEdgeConnection)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedNode> connectedNode1 = this.m_ConnectedNodes[edge1];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedNode> connectedNode2 = this.m_ConnectedNodes[edge2];
        int length1 = connectedNode1.Length;
        int length2 = connectedNode2.Length;
        for (int index = 0; index < length1; ++index)
        {
          ConnectedNode elem = connectedNode1[index];
          // ISSUE: reference to a compiler-generated field
          Game.Net.Node node = this.m_NodeData[elem.m_Node];
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef1 = this.m_PrefabRefData[elem.m_Node];
          LocalConnectData componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LocalConnectData.TryGetComponent(prefabRef1.m_Prefab, out componentData) && (componentData.m_Flags & LocalConnectFlags.ChooseSides) != (LocalConnectFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef2 = this.m_PrefabRefData[edge1];
            // ISSUE: reference to a compiler-generated field
            float num1 = (float) ((double) this.m_PrefabGeometryData[prefabRef1.m_Prefab].m_DefaultWidth * 0.5 + 0.10000000149011612);
            // ISSUE: reference to a compiler-generated field
            float num2 = this.m_PrefabGeometryData[prefabRef2.m_Prefab].m_DefaultWidth * 0.5f;
            float curvePosition1;
            // ISSUE: reference to a compiler-generated method
            float clampedDistance1 = this.GetClampedDistance(curve1, node, out curvePosition1);
            float curvePosition2;
            // ISSUE: reference to a compiler-generated method
            float clampedDistance2 = this.GetClampedDistance(curve2, node, out curvePosition2);
            if ((double) clampedDistance1 <= (double) clampedDistance2)
            {
              elem.m_CurvePosition = curvePosition1;
              connectedNode1[index] = elem;
              if ((double) (clampedDistance2 - (math.sqrt((float) ((double) num2 * (double) num2 + (double) num1 * (double) num1)) - num2)) <= (double) clampedDistance1 && !CollectionUtils.ContainsValue<ConnectedNode>(connectedNode2, elem))
              {
                elem.m_CurvePosition = curvePosition2;
                connectedNode2.Add(elem);
                // ISSUE: reference to a compiler-generated method
                this.AddConnectedEdge(elem.m_Node, edge2);
              }
            }
            else if ((double) (clampedDistance1 - (math.sqrt((float) ((double) num2 * (double) num2 + (double) num1 * (double) num1)) - num2)) <= (double) clampedDistance2)
            {
              elem.m_CurvePosition = curvePosition1;
              connectedNode1[index] = elem;
            }
            else
            {
              connectedNode1.RemoveAt(index--);
              --length1;
              if (CollectionUtils.ContainsValue<ConnectedNode>(connectedNode2, elem))
              {
                // ISSUE: reference to a compiler-generated method
                this.RemoveConnectedEdge(elem.m_Node, edge1);
              }
              else
              {
                elem.m_CurvePosition = curvePosition2;
                connectedNode2.Add(elem);
                // ISSUE: reference to a compiler-generated method
                this.SwitchConnectedEdge(elem.m_Node, edge1, edge2);
              }
            }
          }
          else
          {
            float curvePosition3;
            float curvePosition4;
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            if ((double) this.GetDistance(curve1, node, noEdgeConnection, out curvePosition3) <= (double) this.GetDistance(curve2, node, noEdgeConnection, out curvePosition4) || CollectionUtils.ContainsValue<ConnectedNode>(connectedNode2, elem))
            {
              elem.m_CurvePosition = curvePosition3;
              connectedNode1[index] = elem;
            }
            else
            {
              elem.m_CurvePosition = curvePosition4;
              connectedNode2.Add(elem);
              connectedNode1.RemoveAt(index--);
              // ISSUE: reference to a compiler-generated method
              this.SwitchConnectedEdge(elem.m_Node, edge1, edge2);
              --length1;
            }
          }
        }
        for (int index = 0; index < length2; ++index)
        {
          ConnectedNode elem = connectedNode2[index];
          // ISSUE: reference to a compiler-generated field
          Game.Net.Node node = this.m_NodeData[elem.m_Node];
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef3 = this.m_PrefabRefData[elem.m_Node];
          LocalConnectData componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LocalConnectData.TryGetComponent(prefabRef3.m_Prefab, out componentData) && (componentData.m_Flags & LocalConnectFlags.ChooseSides) != (LocalConnectFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef4 = this.m_PrefabRefData[edge1];
            // ISSUE: reference to a compiler-generated field
            float num3 = (float) ((double) this.m_PrefabGeometryData[prefabRef3.m_Prefab].m_DefaultWidth * 0.5 + 0.10000000149011612);
            // ISSUE: reference to a compiler-generated field
            float num4 = this.m_PrefabGeometryData[prefabRef4.m_Prefab].m_DefaultWidth * 0.5f;
            float curvePosition5;
            // ISSUE: reference to a compiler-generated method
            float clampedDistance3 = this.GetClampedDistance(curve1, node, out curvePosition5);
            float curvePosition6;
            // ISSUE: reference to a compiler-generated method
            float clampedDistance4 = this.GetClampedDistance(curve2, node, out curvePosition6);
            if ((double) clampedDistance4 <= (double) clampedDistance3)
            {
              elem.m_CurvePosition = curvePosition6;
              connectedNode2[index] = elem;
              if ((double) (clampedDistance3 - (math.sqrt((float) ((double) num4 * (double) num4 + (double) num3 * (double) num3)) - num4)) <= (double) clampedDistance4 && !CollectionUtils.ContainsValue<ConnectedNode>(connectedNode1, elem))
              {
                elem.m_CurvePosition = curvePosition5;
                connectedNode1.Add(elem);
                // ISSUE: reference to a compiler-generated method
                this.AddConnectedEdge(elem.m_Node, edge1);
              }
            }
            else if ((double) (clampedDistance4 - (math.sqrt((float) ((double) num4 * (double) num4 + (double) num3 * (double) num3)) - num4)) <= (double) clampedDistance3)
            {
              elem.m_CurvePosition = curvePosition6;
              connectedNode2[index] = elem;
            }
            else
            {
              connectedNode2.RemoveAt(index--);
              --length2;
              if (CollectionUtils.ContainsValue<ConnectedNode>(connectedNode1, elem))
              {
                // ISSUE: reference to a compiler-generated method
                this.RemoveConnectedEdge(elem.m_Node, edge2);
              }
              else
              {
                elem.m_CurvePosition = curvePosition5;
                connectedNode1.Add(elem);
                // ISSUE: reference to a compiler-generated method
                this.SwitchConnectedEdge(elem.m_Node, edge2, edge1);
              }
            }
          }
          else
          {
            float curvePosition7;
            // ISSUE: reference to a compiler-generated method
            float distance = this.GetDistance(curve1, node, noEdgeConnection, out curvePosition7);
            float curvePosition8;
            // ISSUE: reference to a compiler-generated method
            if ((double) this.GetDistance(curve2, node, noEdgeConnection, out curvePosition8) <= (double) distance || CollectionUtils.ContainsValue<ConnectedNode>(connectedNode1, elem))
            {
              elem.m_CurvePosition = curvePosition8;
              connectedNode2[index] = elem;
            }
            else
            {
              elem.m_CurvePosition = curvePosition7;
              connectedNode1.Add(elem);
              connectedNode2.RemoveAt(index--);
              // ISSUE: reference to a compiler-generated method
              this.SwitchConnectedEdge(elem.m_Node, edge2, edge1);
              --length2;
            }
          }
        }
      }

      private void RemoveConnectedEdge(Entity node, Entity source)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TempData.HasComponent(node))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[node];
        for (int index = 0; index < connectedEdge.Length; ++index)
        {
          if (connectedEdge[index].m_Edge == source)
          {
            connectedEdge.RemoveAt(index);
            break;
          }
        }
      }

      private void AddConnectedEdge(Entity node, Entity target)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TempData.HasComponent(node))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_ConnectedEdges[node].Add(new ConnectedEdge(target));
      }

      private void SwitchConnectedEdge(Entity node, Entity source, Entity target)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TempData.HasComponent(node))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> connectedEdge1 = this.m_ConnectedEdges[node];
        for (int index = 0; index < connectedEdge1.Length; ++index)
        {
          ConnectedEdge connectedEdge2 = connectedEdge1[index];
          if (connectedEdge2.m_Edge == source)
          {
            connectedEdge2.m_Edge = target;
            connectedEdge1[index] = connectedEdge2;
          }
        }
      }

      private float GetClampedDistance(Curve curve, Game.Net.Node node, out float curvePosition)
      {
        double num = (double) MathUtils.Distance(curve.m_Bezier.xz, node.m_Position.xz, out curvePosition);
        float t;
        if ((double) curve.m_Length >= 0.20000000298023224)
        {
          float a = 0.1f / curve.m_Length;
          t = math.clamp(curvePosition, a, 1f - a);
        }
        else
          t = 0.5f;
        return math.distance(MathUtils.Position(curve.m_Bezier, t), node.m_Position);
      }

      private float GetDistance(
        Curve curve,
        Game.Net.Node node,
        bool noEdgeConnection,
        out float curvePosition)
      {
        if (!noEdgeConnection)
          return MathUtils.Distance(curve.m_Bezier.xz, node.m_Position.xz, out curvePosition);
        float a = math.distance(curve.m_Bezier.a.xz, node.m_Position.xz);
        float b = math.distance(curve.m_Bezier.d.xz, node.m_Position.xz);
        curvePosition = math.select(0.0f, 1f, (double) b < (double) a);
        return math.select(a, b, (double) b < (double) a);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OwnerDefinition> __Game_Tools_OwnerDefinition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Upgraded> __Game_Net_Upgraded_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Fixed> __Game_Net_Fixed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<LocalConnectData> __Game_Prefabs_LocalConnectData_RO_ComponentLookup;
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RW_ComponentLookup;
      public ComponentLookup<Edge> __Game_Net_Edge_RW_ComponentLookup;
      public ComponentLookup<Curve> __Game_Net_Curve_RW_ComponentLookup;
      public ComponentLookup<Temp> __Game_Tools_Temp_RW_ComponentLookup;
      public ComponentLookup<BuildOrder> __Game_Net_BuildOrder_RW_ComponentLookup;
      public ComponentLookup<Road> __Game_Net_Road_RW_ComponentLookup;
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RW_BufferLookup;
      public BufferLookup<ConnectedNode> __Game_Net_ConnectedNode_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_OwnerDefinition_RO_ComponentLookup = state.GetComponentLookup<OwnerDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Upgraded_RO_ComponentLookup = state.GetComponentLookup<Upgraded>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Fixed_RO_ComponentLookup = state.GetComponentLookup<Fixed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LocalConnectData_RO_ComponentLookup = state.GetComponentLookup<LocalConnectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RW_ComponentLookup = state.GetComponentLookup<Game.Net.Node>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RW_ComponentLookup = state.GetComponentLookup<Edge>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RW_ComponentLookup = state.GetComponentLookup<Curve>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RW_ComponentLookup = state.GetComponentLookup<Temp>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_BuildOrder_RW_ComponentLookup = state.GetComponentLookup<BuildOrder>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RW_ComponentLookup = state.GetComponentLookup<Road>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RW_BufferLookup = state.GetBufferLookup<ConnectedEdge>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RW_BufferLookup = state.GetBufferLookup<ConnectedNode>();
      }
    }
  }
}
