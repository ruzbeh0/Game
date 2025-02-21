// Decompiled with JetBrains decompiler
// Type: Game.Areas.SurfaceExpandSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Areas
{
  [CompilerGenerated]
  public class SurfaceExpandSystem : GameSystemBase
  {
    private EntityQuery m_UpdatedAreasQuery;
    private EntityQuery m_AllAreasQuery;
    private bool m_Loaded;
    private SurfaceExpandSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedAreasQuery = this.GetEntityQuery(ComponentType.ReadOnly<Surface>(), ComponentType.ReadOnly<Expand>(), ComponentType.ReadOnly<Updated>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_AllAreasQuery = this.GetEntityQuery(ComponentType.ReadOnly<Surface>(), ComponentType.ReadOnly<Expand>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
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
      EntityQuery query = this.GetLoaded() ? this.m_AllAreasQuery : this.m_UpdatedAreasQuery;
      if (query.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_AccessLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Extension_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Expand_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      this.Dependency = new SurfaceExpandSystem.ExpandAreasJob()
      {
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle,
        m_ExpandType = this.__TypeHandle.__Game_Areas_Expand_RW_BufferTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_ExtensionData = this.__TypeHandle.__Game_Buildings_Extension_RO_ComponentLookup,
        m_AccessLaneData = this.__TypeHandle.__Game_Routes_AccessLane_RO_ComponentLookup,
        m_RouteLaneData = this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_ConnectedNodes = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup
      }.ScheduleParallel<SurfaceExpandSystem.ExpandAreasJob>(query, this.Dependency);
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
    public SurfaceExpandSystem()
    {
    }

    [BurstCompile]
    private struct ExpandAreasJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public BufferTypeHandle<Node> m_NodeType;
      public BufferTypeHandle<Expand> m_ExpandType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Extension> m_ExtensionData;
      [ReadOnly]
      public ComponentLookup<AccessLane> m_AccessLaneData;
      [ReadOnly]
      public ComponentLookup<RouteLane> m_RouteLaneData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> m_SubNets;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<ConnectedNode> m_ConnectedNodes;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Node> bufferAccessor1 = chunk.GetBufferAccessor<Node>(ref this.m_NodeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Expand> bufferAccessor2 = chunk.GetBufferAccessor<Expand>(ref this.m_ExpandType);
        NativeList<float4> connections = new NativeList<float4>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < bufferAccessor2.Length; ++index)
        {
          DynamicBuffer<Node> nodes = bufferAccessor1[index];
          DynamicBuffer<Expand> expands = bufferAccessor2[index];
          expands.ResizeUninitialized(nodes.Length);
          Owner owner = new Owner();
          if (nativeArray.Length != 0)
          {
            owner = nativeArray[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            while (this.m_OwnerData.HasComponent(owner.m_Owner) && !this.m_BuildingData.HasComponent(owner.m_Owner))
            {
              // ISSUE: reference to a compiler-generated field
              owner = this.m_OwnerData[owner.m_Owner];
            }
          }
          PrefabRef componentData1;
          Transform componentData2;
          BuildingData componentData3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.TryGetComponent(owner.m_Owner, out componentData1) && this.m_TransformData.TryGetComponent(owner.m_Owner, out componentData2) && this.m_PrefabBuildingData.TryGetComponent(componentData1.m_Prefab, out componentData3))
          {
            // ISSUE: reference to a compiler-generated method
            this.Calculate(expands, nodes, connections, owner.m_Owner, componentData2, componentData3);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.Clear(expands);
          }
        }
        connections.Dispose();
      }

      private void Clear(DynamicBuffer<Expand> expands)
      {
        for (int index = 0; index < expands.Length; ++index)
          expands[index] = new Expand();
      }

      private void Calculate(
        DynamicBuffer<Expand> expands,
        DynamicBuffer<Node> nodes,
        NativeList<float4> connections,
        Entity building,
        Transform transform,
        BuildingData prefabBuildingData)
      {
        if (expands.Length == 0)
          return;
        Quad2 xz1 = BuildingUtils.CalculateCorners(transform, prefabBuildingData.m_LotSize).xz;
        float2 xz2 = math.mul(transform.m_Rotation, new float3(0.0f, 0.0f, 8f)).xz;
        float2 xz3 = math.mul(transform.m_Rotation, new float3(-8f, 0.0f, 0.0f)).xz;
        float borderDistance = AreaUtils.GetMinNodeDistance(AreaType.Surface) * 0.5f;
        bool flag = false;
        float2 xz4 = nodes[nodes.Length - 1].m_Position.xz;
        // ISSUE: reference to a compiler-generated method
        float4 border1 = this.CheckBorders(xz1, xz4, prefabBuildingData, borderDistance);
        float2 xz5 = nodes[0].m_Position.xz;
        // ISSUE: reference to a compiler-generated method
        float4 float4 = this.CheckBorders(xz1, xz5, prefabBuildingData, borderDistance);
        bool4 bool4_1 = (bool4) false;
        if (math.any(border1 != -1f & float4 != -1f))
        {
          if (!flag)
          {
            flag = true;
            // ISSUE: reference to a compiler-generated method
            this.FillConnections(connections, building, xz1, prefabBuildingData);
          }
          // ISSUE: reference to a compiler-generated method
          bool4_1 = this.CheckConnections(border1, float4, connections);
        }
        for (int index = 0; index < expands.Length; ++index)
        {
          Expand expand = new Expand();
          float2 xz6 = nodes[math.select(index + 1, 0, index == nodes.Length - 1)].m_Position.xz;
          // ISSUE: reference to a compiler-generated method
          float4 border2 = this.CheckBorders(xz1, xz6, prefabBuildingData, borderDistance);
          bool4 bool4_2 = (bool4) false;
          if (math.any(float4 != -1f & border2 != -1f))
          {
            if (!flag)
            {
              flag = true;
              // ISSUE: reference to a compiler-generated method
              this.FillConnections(connections, building, xz1, prefabBuildingData);
            }
            // ISSUE: reference to a compiler-generated method
            bool4_2 = this.CheckConnections(float4, border2, connections);
          }
          bool4 bool4_3 = bool4_1 | bool4_2;
          if (bool4_3.x)
            expand.m_Offset += xz2;
          if (bool4_3.y)
            expand.m_Offset += xz3;
          if (bool4_3.z)
            expand.m_Offset -= xz2;
          if (bool4_3.w)
            expand.m_Offset -= xz3;
          expands[index] = expand;
          float4 = border2;
          bool4_1 = bool4_2;
        }
      }

      private void FillConnections(
        NativeList<float4> connections,
        Entity building,
        Quad2 quad,
        BuildingData prefabBuildingData)
      {
        connections.Clear();
        // ISSUE: reference to a compiler-generated method
        this.AddConnections(connections, building, quad, prefabBuildingData);
      }

      private void AddConnections(
        NativeList<float4> connections,
        Entity owner,
        Quad2 quad,
        BuildingData prefabBuildingData)
      {
        DynamicBuffer<Game.Objects.SubObject> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubObjects.TryGetBuffer(owner, out bufferData1))
        {
          for (int index = 0; index < bufferData1.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            this.AddConnections(connections, bufferData1[index], quad, prefabBuildingData);
          }
        }
        DynamicBuffer<Game.Net.SubNet> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubNets.TryGetBuffer(owner, out bufferData2))
          return;
        for (int index = 0; index < bufferData2.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.AddConnections(connections, bufferData2[index], quad, prefabBuildingData);
        }
      }

      private void AddConnections(
        NativeList<float4> connections,
        Game.Objects.SubObject subObject,
        Quad2 quad,
        BuildingData prefabBuildingData)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ExtensionData.HasComponent(subObject.m_SubObject))
        {
          // ISSUE: reference to a compiler-generated method
          this.AddConnections(connections, subObject.m_SubObject, quad, prefabBuildingData);
        }
        AccessLane componentData1;
        RouteLane componentData2;
        Curve componentData3;
        Curve componentData4;
        Owner componentData5;
        Owner componentData6;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.m_AccessLaneData.TryGetComponent(subObject.m_SubObject, out componentData1) || !this.m_RouteLaneData.TryGetComponent(subObject.m_SubObject, out componentData2) || !this.m_CurveData.TryGetComponent(componentData1.m_Lane, out componentData3) || !this.m_CurveData.TryGetComponent(componentData2.m_EndLane, out componentData4) || (!this.m_OwnerData.TryGetComponent(componentData1.m_Lane, out componentData5) || !this.CheckConnectionOwner(componentData5.m_Owner)) && (!this.m_OwnerData.TryGetComponent(componentData2.m_EndLane, out componentData6) || !this.CheckConnectionOwner(componentData6.m_Owner)))
          return;
        Line2.Segment line;
        line.a = MathUtils.Position(componentData3.m_Bezier, componentData1.m_CurvePos).xz;
        line.b = MathUtils.Position(componentData4.m_Bezier, componentData2.m_EndCurvePos).xz;
        // ISSUE: reference to a compiler-generated method
        this.AddConnection(connections, line, quad, prefabBuildingData);
      }

      private void AddConnections(
        NativeList<float4> connections,
        Game.Net.SubNet subNet,
        Quad2 quad,
        BuildingData prefabBuildingData)
      {
        DynamicBuffer<ConnectedEdge> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ConnectedEdges.TryGetBuffer(subNet.m_SubNet, out bufferData))
          return;
        // ISSUE: reference to a compiler-generated field
        Game.Net.Node node = this.m_NodeData[subNet.m_SubNet];
        for (int index1 = 0; index1 < bufferData.Length; ++index1)
        {
          ConnectedEdge connectedEdge = bufferData[index1];
          // ISSUE: reference to a compiler-generated field
          Edge edge = this.m_EdgeData[connectedEdge.m_Edge];
          // ISSUE: reference to a compiler-generated method
          if (!(edge.m_Start == subNet.m_SubNet) && !(edge.m_End == subNet.m_SubNet) && this.CheckConnectionOwner(connectedEdge.m_Edge))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ConnectedNode> connectedNode1 = this.m_ConnectedNodes[connectedEdge.m_Edge];
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[connectedEdge.m_Edge];
            for (int index2 = 0; index2 < connectedNode1.Length; ++index2)
            {
              ConnectedNode connectedNode2 = connectedNode1[index2];
              if (connectedNode2.m_Node == subNet.m_SubNet)
              {
                Line2.Segment line = new Line2.Segment(node.m_Position.xz, MathUtils.Position(curve.m_Bezier, connectedNode2.m_CurvePosition).xz);
                // ISSUE: reference to a compiler-generated method
                this.AddConnection(connections, line, quad, prefabBuildingData);
                break;
              }
            }
          }
        }
      }

      private bool CheckConnectionOwner(Entity owner)
      {
        PrefabRef componentData1;
        NetGeometryData componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_PrefabRefData.TryGetComponent(owner, out componentData1) && this.m_PrefabNetGeometryData.TryGetComponent(componentData1.m_Prefab, out componentData2) && (componentData2.m_Flags & Game.Net.GeometryFlags.Marker) == (Game.Net.GeometryFlags) 0;
      }

      private void AddConnection(
        NativeList<float4> connections,
        Line2.Segment line,
        Quad2 quad,
        BuildingData prefabBuildingData)
      {
        float4 float4 = (float4) -1f;
        float2 t1;
        if ((prefabBuildingData.m_Flags & Game.Prefabs.BuildingFlags.BackAccess) != (Game.Prefabs.BuildingFlags) 0 && MathUtils.Intersect(quad.ab, line, out t1))
          float4.z = t1.x;
        float2 t2;
        if ((prefabBuildingData.m_Flags & Game.Prefabs.BuildingFlags.RightAccess) != (Game.Prefabs.BuildingFlags) 0 && MathUtils.Intersect(quad.bc, line, out t2))
          float4.y = t2.x;
        float2 t3;
        if (MathUtils.Intersect(quad.cd, line, out t3))
          float4.x = t3.x;
        float2 t4;
        if ((prefabBuildingData.m_Flags & Game.Prefabs.BuildingFlags.LeftAccess) != (Game.Prefabs.BuildingFlags) 0 && MathUtils.Intersect(quad.da, line, out t4))
          float4.w = t4.x;
        if (!math.any(float4 != -1f))
          return;
        connections.Add(in float4);
      }

      private bool4 CheckConnections(
        float4 border1,
        float4 border2,
        NativeList<float4> connections)
      {
        bool4 bool4 = (bool4) false;
        float4 float4_1 = math.min(math.select(border1, (float4) 2f, border1 == -1f), math.select(border2, (float4) 2f, border2 == -1f));
        float4 float4_2 = math.max(border1, border2);
        for (int index = 0; index < connections.Length; ++index)
        {
          float4 connection = connections[index];
          bool4 |= connection >= float4_1 & connection <= float4_2;
        }
        return bool4;
      }

      private float4 CheckBorders(
        Quad2 quad,
        float2 position,
        BuildingData prefabBuildingData,
        float borderDistance)
      {
        float4 float4 = (float4) -1f;
        float t1;
        if ((prefabBuildingData.m_Flags & Game.Prefabs.BuildingFlags.BackAccess) != (Game.Prefabs.BuildingFlags) 0 && (double) MathUtils.Distance(quad.ab, position, out t1) < (double) borderDistance)
          float4.z = t1;
        float t2;
        if ((prefabBuildingData.m_Flags & Game.Prefabs.BuildingFlags.RightAccess) != (Game.Prefabs.BuildingFlags) 0 && (double) MathUtils.Distance(quad.bc, position, out t2) < (double) borderDistance)
          float4.y = t2;
        float t3;
        if ((double) MathUtils.Distance(quad.cd, position, out t3) < (double) borderDistance)
          float4.x = t3;
        float t4;
        if ((prefabBuildingData.m_Flags & Game.Prefabs.BuildingFlags.LeftAccess) != (Game.Prefabs.BuildingFlags) 0 && (double) MathUtils.Distance(quad.da, position, out t4) < (double) borderDistance)
          float4.w = t4;
        return float4;
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
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Node> __Game_Areas_Node_RO_BufferTypeHandle;
      public BufferTypeHandle<Expand> __Game_Areas_Expand_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Extension> __Game_Buildings_Extension_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AccessLane> __Game_Routes_AccessLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteLane> __Game_Routes_RouteLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedNode> __Game_Net_ConnectedNode_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferTypeHandle = state.GetBufferTypeHandle<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Expand_RW_BufferTypeHandle = state.GetBufferTypeHandle<Expand>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Extension_RO_ComponentLookup = state.GetComponentLookup<Extension>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_AccessLane_RO_ComponentLookup = state.GetComponentLookup<AccessLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteLane_RO_ComponentLookup = state.GetComponentLookup<RouteLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RO_BufferLookup = state.GetBufferLookup<ConnectedNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
      }
    }
  }
}
