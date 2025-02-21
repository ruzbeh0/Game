// Decompiled with JetBrains decompiler
// Type: Game.Net.EdgeMappingSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Pathfind;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class EdgeMappingSystem : GameSystemBase
  {
    private EntityQuery m_UpdatedLanesQuery;
    private EntityQuery m_AllLanesQuery;
    private bool m_Loaded;
    private EdgeMappingSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedLanesQuery = this.GetEntityQuery(ComponentType.ReadOnly<EdgeMapping>(), ComponentType.ReadOnly<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_AllLanesQuery = this.GetEntityQuery(ComponentType.ReadOnly<EdgeMapping>());
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery query = this.GetLoaded() ? this.m_AllLanesQuery : this.m_UpdatedLanesQuery;
      if (query.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeMapping_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      EdgeMappingSystem.UpdateMappingJob jobData = new EdgeMappingSystem.UpdateMappingJob()
      {
        m_LaneType = this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle,
        m_EdgeLaneType = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentTypeHandle,
        m_NodeLaneType = this.__TypeHandle.__Game_Net_NodeLane_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_EdgeMappingType = this.__TypeHandle.__Game_Net_EdgeMapping_RW_ComponentTypeHandle,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_ConnectedNodes = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup
      };
      this.Dependency = jobData.ScheduleParallel<EdgeMappingSystem.UpdateMappingJob>(query, this.Dependency);
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
    public EdgeMappingSystem()
    {
    }

    [BurstCompile]
    private struct UpdateMappingJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Lane> m_LaneType;
      [ReadOnly]
      public ComponentTypeHandle<EdgeLane> m_EdgeLaneType;
      [ReadOnly]
      public ComponentTypeHandle<NodeLane> m_NodeLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      public ComponentTypeHandle<EdgeMapping> m_EdgeMappingType;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<ConnectedNode> m_ConnectedNodes;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Lane> nativeArray1 = chunk.GetNativeArray<Lane>(ref this.m_LaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Curve> nativeArray2 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<EdgeMapping> nativeArray4 = chunk.GetNativeArray<EdgeMapping>(ref this.m_EdgeMappingType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<EdgeLane>(ref this.m_EdgeLaneType);
        // ISSUE: reference to a compiler-generated field
        bool flag2 = chunk.Has<NodeLane>(ref this.m_NodeLaneType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Lane lane = nativeArray1[index];
          Curve laneCurve = nativeArray2[index];
          EdgeMapping edgeMapping = new EdgeMapping();
          if (flag1)
          {
            edgeMapping.m_Parent1 = nativeArray3[index].m_Owner;
            // ISSUE: reference to a compiler-generated method
            edgeMapping.m_CurveDelta1 = this.GetCurveDelta(laneCurve.m_Bezier, edgeMapping.m_Parent1);
          }
          else if (flag2)
          {
            Owner owner = nativeArray3[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectedEdges.HasBuffer(owner.m_Owner))
            {
              // ISSUE: reference to a compiler-generated method
              edgeMapping = this.GetNodeEdgeMapping(lane, laneCurve, owner.m_Owner);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConnectedNodes.HasBuffer(owner.m_Owner))
              {
                // ISSUE: reference to a compiler-generated method
                edgeMapping = this.GetEdgeNodeMapping(lane, owner.m_Owner);
              }
            }
          }
          nativeArray4[index] = edgeMapping;
        }
      }

      private EdgeMapping GetNodeEdgeMapping(Lane lane, Curve laneCurve, Entity node)
      {
        EdgeMapping nodeEdgeMapping = new EdgeMapping();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeIterator edgeIterator = new EdgeIterator(Entity.Null, node, this.m_ConnectedEdges, this.m_EdgeData, this.m_TempData, this.m_HiddenData);
        bool2 bool2 = (bool2) false;
        EdgeIteratorValue edgeIteratorValue;
        while (edgeIterator.GetNext(out edgeIteratorValue))
        {
          PathNode other1 = new PathNode(edgeIteratorValue.m_Edge, (ushort) 0);
          if (lane.m_StartNode.OwnerEquals(other1))
          {
            nodeEdgeMapping.m_Parent1 = edgeIteratorValue.m_Edge;
            bool2.x = edgeIteratorValue.m_End;
          }
          if (lane.m_EndNode.OwnerEquals(other1))
          {
            nodeEdgeMapping.m_Parent2 = edgeIteratorValue.m_Edge;
            bool2.y = edgeIteratorValue.m_End;
          }
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<ConnectedNode> connectedNode = this.m_ConnectedNodes[edgeIteratorValue.m_Edge];
          for (int index = 0; index < connectedNode.Length; ++index)
          {
            PathNode other2 = new PathNode(connectedNode[index].m_Node, (ushort) 0);
            if (lane.m_StartNode.OwnerEquals(other2))
            {
              nodeEdgeMapping.m_Parent2 = edgeIteratorValue.m_Edge;
              bool2.y = edgeIteratorValue.m_End;
            }
            if (lane.m_EndNode.OwnerEquals(other2))
            {
              nodeEdgeMapping.m_Parent1 = edgeIteratorValue.m_Edge;
              bool2.x = edgeIteratorValue.m_End;
            }
          }
        }
        if (nodeEdgeMapping.m_Parent1 != Entity.Null && nodeEdgeMapping.m_Parent2 != Entity.Null)
        {
          Bezier4x3 output1;
          Bezier4x3 output2;
          MathUtils.Divide(laneCurve.m_Bezier, out output1, out output2, 0.5f);
          // ISSUE: reference to a compiler-generated method
          nodeEdgeMapping.m_CurveDelta1 = this.GetCurveDelta(output1, nodeEdgeMapping.m_Parent1);
          // ISSUE: reference to a compiler-generated method
          nodeEdgeMapping.m_CurveDelta2 = this.GetCurveDelta(output2, nodeEdgeMapping.m_Parent2);
        }
        else if (nodeEdgeMapping.m_Parent1 != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          nodeEdgeMapping.m_CurveDelta1 = this.GetCurveDelta(laneCurve.m_Bezier, nodeEdgeMapping.m_Parent1);
        }
        else if (nodeEdgeMapping.m_Parent2 != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          nodeEdgeMapping.m_CurveDelta2 = this.GetCurveDelta(laneCurve.m_Bezier, nodeEdgeMapping.m_Parent2);
        }
        nodeEdgeMapping.m_CurveDelta1.y = !bool2.x ? math.cmin(nodeEdgeMapping.m_CurveDelta1) : math.cmax(nodeEdgeMapping.m_CurveDelta1);
        nodeEdgeMapping.m_CurveDelta2.x = !bool2.y ? math.cmin(nodeEdgeMapping.m_CurveDelta2) : math.cmax(nodeEdgeMapping.m_CurveDelta2);
        if (nodeEdgeMapping.m_Parent1 == Entity.Null)
        {
          nodeEdgeMapping.m_Parent1 = nodeEdgeMapping.m_Parent2;
          nodeEdgeMapping.m_CurveDelta1 = nodeEdgeMapping.m_CurveDelta2;
          nodeEdgeMapping.m_Parent2 = Entity.Null;
          nodeEdgeMapping.m_CurveDelta2 = new float2();
        }
        return nodeEdgeMapping;
      }

      private EdgeMapping GetEdgeNodeMapping(Lane lane, Entity edge)
      {
        EdgeMapping edgeNodeMapping = new EdgeMapping();
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedNode> connectedNode1 = this.m_ConnectedNodes[edge];
        float2 float2 = new float2();
        for (int index = 0; index < connectedNode1.Length; ++index)
        {
          ConnectedNode connectedNode2 = connectedNode1[index];
          PathNode other1 = new PathNode(connectedNode2.m_Node, (ushort) 0);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          EdgeIterator edgeIterator = new EdgeIterator(Entity.Null, connectedNode2.m_Node, this.m_ConnectedEdges, this.m_EdgeData, this.m_TempData, this.m_HiddenData);
          if (lane.m_StartNode.OwnerEquals(other1))
          {
            edgeNodeMapping.m_Parent1 = connectedNode2.m_Node;
            float2.x = connectedNode2.m_CurvePosition;
            break;
          }
          if (lane.m_EndNode.OwnerEquals(other1))
          {
            edgeNodeMapping.m_Parent2 = connectedNode2.m_Node;
            float2.y = connectedNode2.m_CurvePosition;
            break;
          }
          EdgeIteratorValue edgeIteratorValue;
          while (edgeIterator.GetNext(out edgeIteratorValue))
          {
            PathNode other2 = new PathNode(edgeIteratorValue.m_Edge, (ushort) 0);
            if (lane.m_StartNode.OwnerEquals(other2))
            {
              edgeNodeMapping.m_Parent1 = connectedNode2.m_Node;
              float2.x = connectedNode2.m_CurvePosition;
              goto label_12;
            }
            else if (lane.m_EndNode.OwnerEquals(other2))
            {
              edgeNodeMapping.m_Parent2 = connectedNode2.m_Node;
              float2.y = connectedNode2.m_CurvePosition;
              goto label_12;
            }
          }
        }
label_12:
        if (edgeNodeMapping.m_Parent1 != Entity.Null)
        {
          edgeNodeMapping.m_Parent2 = edge;
          edgeNodeMapping.m_CurveDelta1 = new float2(0.0f, 1f);
          edgeNodeMapping.m_CurveDelta2 = (float2) float2.x;
        }
        else if (edgeNodeMapping.m_Parent2 != Entity.Null)
        {
          edgeNodeMapping.m_Parent1 = edgeNodeMapping.m_Parent2;
          edgeNodeMapping.m_Parent2 = edge;
          edgeNodeMapping.m_CurveDelta1 = new float2(1f, 0.0f);
          edgeNodeMapping.m_CurveDelta2 = (float2) float2.y;
        }
        return edgeNodeMapping;
      }

      private float2 GetCurveDelta(Bezier4x3 laneCurve, Entity edge)
      {
        float2 curveDelta = new float2();
        Curve componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurveData.TryGetComponent(edge, out componentData))
        {
          double num1 = (double) MathUtils.Distance(componentData.m_Bezier.xz, laneCurve.a.xz, out curveDelta.x);
          double num2 = (double) MathUtils.Distance(componentData.m_Bezier.xz, laneCurve.d.xz, out curveDelta.y);
        }
        return curveDelta;
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
      public ComponentTypeHandle<Lane> __Game_Net_Lane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EdgeLane> __Game_Net_EdgeLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<NodeLane> __Game_Net_NodeLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      public ComponentTypeHandle<EdgeMapping> __Game_Net_EdgeMapping_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedNode> __Game_Net_ConnectedNode_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NodeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeMapping_RW_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeMapping>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RO_BufferLookup = state.GetBufferLookup<ConnectedNode>(true);
      }
    }
  }
}
