// Decompiled with JetBrains decompiler
// Type: Game.Net.ValidationHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Common;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  public static class ValidationHelpers
  {
    public static void ValidateEdge(
      Entity entity,
      Temp temp,
      Owner owner,
      Fixed _fixed,
      Edge edge,
      EdgeGeometry edgeGeometry,
      StartNodeGeometry startNodeGeometry,
      EndNodeGeometry endNodeGeometry,
      Composition composition,
      PrefabRef prefabRef,
      bool editorMode,
      ValidationSystem.EntityData data,
      NativeList<ValidationSystem.BoundsData> edgeList,
      NativeQuadTree<Entity, QuadTreeBoundsXZ> objectSearchTree,
      NativeQuadTree<Entity, QuadTreeBoundsXZ> netSearchTree,
      NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> areaSearchTree,
      WaterSurfaceData waterSurfaceData,
      TerrainHeightData terrainHeightData,
      NativeQueue<ErrorData>.ParallelWriter errorQueue,
      NativeList<ConnectedNode> tempNodes)
    {
      Edge edge1;
      edge1.m_Start = ValidationHelpers.GetNetNode(edge.m_Start, data);
      edge1.m_End = ValidationHelpers.GetNetNode(edge.m_End, data);
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<ConnectedNode> connectedNode1 = data.m_ConnectedNodes[entity];
      tempNodes.Clear();
      for (int index = 0; index < connectedNode1.Length; ++index)
      {
        ConnectedNode connectedNode2 = connectedNode1[index];
        connectedNode2.m_Node = ValidationHelpers.GetNetNode(connectedNode2.m_Node, data);
        tempNodes.Add(in connectedNode2);
      }
      bool flag1 = owner.m_Owner != Entity.Null;
      Bounds3 bounds1 = edgeGeometry.m_Bounds | startNodeGeometry.m_Geometry.m_Bounds | endNodeGeometry.m_Geometry.m_Bounds;
      // ISSUE: reference to a compiler-generated field
      NetCompositionData compositionData1 = data.m_PrefabComposition[composition.m_Edge];
      // ISSUE: reference to a compiler-generated field
      NetCompositionData compositionData2 = data.m_PrefabComposition[composition.m_StartNode];
      // ISSUE: reference to a compiler-generated field
      NetCompositionData compositionData3 = data.m_PrefabComposition[composition.m_EndNode];
      CollisionMask collisionMask1 = NetUtils.GetCollisionMask(compositionData1, !editorMode | flag1);
      CollisionMask collisionMask2 = NetUtils.GetCollisionMask(compositionData2, !editorMode | flag1);
      CollisionMask collisionMask3 = NetUtils.GetCollisionMask(compositionData3, !editorMode | flag1);
      CollisionMask collisionMask4 = collisionMask1 | collisionMask2 | collisionMask3;
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<NetCompositionArea> prefabCompositionArea1 = data.m_PrefabCompositionAreas[composition.m_Edge];
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<NetCompositionArea> prefabCompositionArea2 = data.m_PrefabCompositionAreas[composition.m_StartNode];
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<NetCompositionArea> prefabCompositionArea3 = data.m_PrefabCompositionAreas[composition.m_EndNode];
      Entity entity1 = entity;
      Entity owner1;
      // ISSUE: reference to a compiler-generated field
      if (owner.m_Owner != Entity.Null && !data.m_AssetStamp.HasComponent(owner.m_Owner))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (entity1 = owner.m_Owner; data.m_Owner.HasComponent(entity1) && !data.m_Building.HasComponent(entity1); entity1 = owner1)
        {
          // ISSUE: reference to a compiler-generated field
          owner1 = data.m_Owner[entity1].m_Owner;
          // ISSUE: reference to a compiler-generated field
          if (data.m_AssetStamp.HasComponent(owner1))
            break;
        }
      }
      ValidationHelpers.NetIterator iterator1 = new ValidationHelpers.NetIterator();
      if ((temp.m_Flags & TempFlags.Delete) == (TempFlags) 0)
      {
        iterator1 = new ValidationHelpers.NetIterator()
        {
          m_Edge = edge,
          m_OriginalNodes = edge1,
          m_ConnectedNodes = connectedNode1.AsNativeArray(),
          m_OriginalConnectedNodes = tempNodes.AsArray(),
          m_TopLevelEntity = entity1,
          m_Essential = (temp.m_Flags & TempFlags.Essential) > (TempFlags) 0,
          m_EditorMode = editorMode,
          m_EdgeEntity = entity,
          m_Bounds = bounds1,
          m_EdgeGeometryData = edgeGeometry,
          m_StartNodeGeometryData = startNodeGeometry,
          m_EndNodeGeometryData = endNodeGeometry,
          m_EdgeCompositionData = compositionData1,
          m_StartCompositionData = compositionData2,
          m_EndCompositionData = compositionData3,
          m_EdgeCollisionMask = collisionMask1,
          m_StartCollisionMask = collisionMask2,
          m_EndCollisionMask = collisionMask3,
          m_CombinedCollisionMask = collisionMask4,
          m_Data = data,
          m_ErrorQueue = errorQueue
        };
        netSearchTree.Iterate<ValidationHelpers.NetIterator>(ref iterator1);
      }
      Edge edge2;
      Edge edge3;
      edge2.m_Start = ValidationHelpers.GetOwner(edge1.m_Start, data, out edge3.m_Start);
      edge2.m_End = ValidationHelpers.GetOwner(edge1.m_End, data, out edge3.m_End);
      ValidationHelpers.ObjectIterator objectIterator = new ValidationHelpers.ObjectIterator();
      if ((temp.m_Flags & TempFlags.Delete) == (TempFlags) 0)
      {
        Entity assetStamp;
        Entity owner2 = ValidationHelpers.GetOwner(entity, data, out assetStamp);
        ValidationHelpers.ObjectIterator iterator2 = new ValidationHelpers.ObjectIterator()
        {
          m_OriginalNodes = edge1,
          m_NodeOwners = edge2,
          m_NodeAssetStamps = edge3,
          m_EdgeEntity = entity,
          m_TopLevelEntity = owner2,
          m_AssetStampEntity = assetStamp,
          m_Bounds = bounds1,
          m_EdgeGeometryData = edgeGeometry,
          m_StartNodeGeometryData = startNodeGeometry,
          m_EndNodeGeometryData = endNodeGeometry,
          m_EdgeCompositionData = compositionData1,
          m_StartCompositionData = compositionData2,
          m_EndCompositionData = compositionData3,
          m_EdgeCollisionMask = collisionMask1,
          m_StartCollisionMask = collisionMask2,
          m_EndCollisionMask = collisionMask3,
          m_CombinedCollisionMask = collisionMask4,
          m_EdgeCompositionAreas = prefabCompositionArea1,
          m_StartCompositionAreas = prefabCompositionArea2,
          m_EndCompositionAreas = prefabCompositionArea3,
          m_Data = data,
          m_ErrorQueue = errorQueue,
          m_EditorMode = editorMode
        };
        objectSearchTree.Iterate<ValidationHelpers.ObjectIterator>(ref iterator2);
      }
      ValidationHelpers.AreaIterator iterator3 = new ValidationHelpers.AreaIterator()
      {
        m_NodeOwners = edge2,
        m_EdgeEntity = entity,
        m_Bounds = bounds1,
        m_IgnoreCollisions = (temp.m_Flags & TempFlags.Delete) > (TempFlags) 0,
        m_IgnoreProtectedAreas = (temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Modify | TempFlags.Replace | TempFlags.Upgrade)) == (TempFlags) 0 || (temp.m_Flags & TempFlags.Hidden) > (TempFlags) 0,
        m_EditorMode = editorMode,
        m_EdgeGeometryData = edgeGeometry,
        m_StartNodeGeometryData = startNodeGeometry,
        m_EndNodeGeometryData = endNodeGeometry,
        m_EdgeCompositionData = compositionData1,
        m_StartCompositionData = compositionData2,
        m_EndCompositionData = compositionData3,
        m_EdgeCollisionMask = collisionMask1,
        m_StartCollisionMask = collisionMask2,
        m_EndCollisionMask = collisionMask3,
        m_CombinedCollisionMask = collisionMask4,
        m_Data = data,
        m_ErrorQueue = errorQueue
      };
      areaSearchTree.Iterate<ValidationHelpers.AreaIterator>(ref iterator3);
      if ((temp.m_Flags & TempFlags.Delete) == (TempFlags) 0 && edgeList.Length != 0)
      {
        int num1 = 0;
        int num2 = edgeList.Length;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float3 float3 = edgeList[edgeList.Length - 1].m_Bounds.max - edgeList[0].m_Bounds.min;
        bool flag2 = (double) float3.z > (double) float3.x;
        while (num1 < num2)
        {
          int index = num1 + num2 >> 1;
          // ISSUE: variable of a compiler-generated type
          ValidationSystem.BoundsData edge4 = edgeList[index];
          // ISSUE: reference to a compiler-generated field
          bool2 bool2 = edge4.m_Bounds.min.xz < bounds1.min.xz;
          if ((flag2 ? (bool2.y ? 1 : 0) : (bool2.x ? 1 : 0)) != 0)
            num1 = index + 1;
          else
            num2 = index;
        }
        for (int index = 0; index < edgeList.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          ValidationSystem.BoundsData edge5 = edgeList[index];
          // ISSUE: reference to a compiler-generated field
          bool2 bool2 = edge5.m_Bounds.min.xz > bounds1.max.xz;
          if ((flag2 ? (bool2.y ? 1 : 0) : (bool2.x ? 1 : 0)) == 0)
          {
            if ((collisionMask4 & CollisionMask.OnGround) != (CollisionMask) 0)
            {
              // ISSUE: reference to a compiler-generated field
              if (!MathUtils.Intersect(bounds1.xz, edge5.m_Bounds.xz))
                continue;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (!MathUtils.Intersect(bounds1, edge5.m_Bounds))
                continue;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!(edge5.m_Entity == entity) && ((double) edge5.m_Bounds.min.x != (double) bounds1.min.x || edge5.m_Entity.Index >= entity.Index))
            {
              // ISSUE: reference to a compiler-generated field
              Entity entity2 = edge5.m_Entity;
              Owner componentData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (data.m_Owner.TryGetComponent(edge5.m_Entity, out componentData))
              {
                Entity owner3 = componentData.m_Owner;
                Entity owner4;
                // ISSUE: reference to a compiler-generated field
                if (!data.m_AssetStamp.HasComponent(owner3))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  for (entity2 = owner3; data.m_Owner.HasComponent(entity2) && !data.m_Building.HasComponent(entity2); entity2 = owner4)
                  {
                    // ISSUE: reference to a compiler-generated field
                    owner4 = data.m_Owner[entity2].m_Owner;
                    // ISSUE: reference to a compiler-generated field
                    if (data.m_AssetStamp.HasComponent(owner4))
                      break;
                  }
                }
              }
              if (!(entity1 == entity2))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Edge edgeData2 = data.m_Edge[edge5.m_Entity];
                if (!(edge.m_Start == edgeData2.m_Start) && !(edge.m_Start == edgeData2.m_End) && !(edge.m_End == edgeData2.m_Start) && !(edge.m_End == edgeData2.m_End))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  EdgeGeometry edgeGeometryData2 = data.m_EdgeGeometry[edge5.m_Entity];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  StartNodeGeometry startNodeGeometryData2 = data.m_StartNodeGeometry[edge5.m_Entity];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  EndNodeGeometry endNodeGeometryData2 = data.m_EndNodeGeometry[edge5.m_Entity];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Composition compositionData2_1 = data.m_Composition[edge5.m_Entity];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Temp temp1 = data.m_Temp[edge5.m_Entity];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  iterator1.CheckOverlap(entity2, edge5.m_Entity, edge5.m_Bounds, edgeData2, compositionData2_1, edgeGeometryData2, startNodeGeometryData2, endNodeGeometryData2, (temp1.m_Flags & TempFlags.Essential) > (TempFlags) 0, componentData.m_Owner != Entity.Null);
                }
              }
            }
          }
          else
            break;
        }
      }
      Bounds3 errorBounds;
      errorBounds.min = (float3) float.MaxValue;
      errorBounds.max = (float3) float.MinValue;
      bool flag3 = false;
      if ((temp.m_Flags & TempFlags.Essential) == (TempFlags) 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        flag3 = !data.m_NetElevation.HasComponent(entity) || !data.m_NetElevation.HasComponent(edge.m_Start) || !data.m_NetElevation.HasComponent(edge.m_End);
      }
      if ((temp.m_Flags & TempFlags.Delete) == (TempFlags) 0)
      {
        // ISSUE: reference to a compiler-generated field
        int num = !(entity1 != entity) ? 0 : (ValidationHelpers.IsInternal(entity1, edge.m_Start, data.m_ConnectedEdges[edge.m_Start], data) ? 1 : 0);
        // ISSUE: reference to a compiler-generated field
        bool flag4 = entity1 != entity && ValidationHelpers.IsInternal(entity1, edge.m_End, data.m_ConnectedEdges[edge.m_End], data);
        bool flag5 = false;
        if (num == 0 | !flag4)
          flag5 |= ValidationHelpers.CheckGeometryShape(edgeGeometry, ref errorBounds);
        if (num == 0)
          flag5 |= ValidationHelpers.CheckGeometryShape(startNodeGeometry.m_Geometry, ref errorBounds);
        if (!flag4)
          flag5 |= ValidationHelpers.CheckGeometryShape(endNodeGeometry.m_Geometry, ref errorBounds);
        if (flag5)
          errorQueue.Enqueue(new ErrorData()
          {
            m_ErrorType = ErrorType.InvalidShape,
            m_ErrorSeverity = ErrorSeverity.Error,
            m_Position = MathUtils.Center(errorBounds),
            m_TempEntity = entity
          });
        // ISSUE: reference to a compiler-generated field
        if (data.m_PrefabNetGeometry.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          Curve curve = data.m_Curve[entity];
          // ISSUE: reference to a compiler-generated field
          NetGeometryData netGeometryData = data.m_PrefabNetGeometry[prefabRef.m_Prefab];
          Bounds1 edgeLengthRange = netGeometryData.m_EdgeLengthRange;
          // ISSUE: reference to a compiler-generated field
          if (_fixed.m_Index >= 0 && data.m_PrefabFixedElements.HasBuffer(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<FixedNetElement> prefabFixedElement = data.m_PrefabFixedElements[prefabRef.m_Prefab];
            if (_fixed.m_Index < prefabFixedElement.Length)
            {
              Bounds1 lengthRange = prefabFixedElement[_fixed.m_Index].m_LengthRange;
              edgeLengthRange.min = math.select(lengthRange.min, lengthRange.min * 0.6f, (double) lengthRange.max == (double) lengthRange.min);
              edgeLengthRange.max = lengthRange.max;
            }
          }
          edgeLengthRange.max *= 1.1f;
          Bezier4x3 bezier4x3_1 = MathUtils.Lerp(edgeGeometry.m_Start.m_Left, edgeGeometry.m_Start.m_Right, 0.5f);
          Bezier4x3 bezier4x3_2 = MathUtils.Lerp(edgeGeometry.m_End.m_Left, edgeGeometry.m_End.m_Right, 0.5f);
          float y1 = MathUtils.Length(bezier4x3_1.xz);
          float y2 = MathUtils.Length(bezier4x3_2.xz);
          if ((double) y1 + (double) y2 < (double) edgeLengthRange.min)
            errorQueue.Enqueue(new ErrorData()
            {
              m_ErrorType = ErrorType.ShortDistance,
              m_ErrorSeverity = ErrorSeverity.Error,
              m_Position = math.lerp(edgeGeometry.m_Start.m_Left.d, edgeGeometry.m_Start.m_Right.d, 0.5f),
              m_TempEntity = entity
            });
          if ((double) y1 + (double) y2 > (double) edgeLengthRange.max)
            errorQueue.Enqueue(new ErrorData()
            {
              m_ErrorType = ErrorType.LongDistance,
              m_ErrorSeverity = ErrorSeverity.Error,
              m_Position = math.lerp(edgeGeometry.m_Start.m_Left.d, edgeGeometry.m_Start.m_Right.d, 0.5f),
              m_TempEntity = entity
            });
          if ((netGeometryData.m_Flags & GeometryFlags.FlattenTerrain) != (GeometryFlags) 0 && (temp.m_Flags & TempFlags.Essential) != (TempFlags) 0)
            flag3 = false;
          if ((double) netGeometryData.m_MaxSlopeSteepness != 0.0 && !flag3)
          {
            float3 float3;
            float3.x = math.abs(bezier4x3_1.d.y - bezier4x3_1.a.y) / math.max(0.1f, y1);
            float3.y = math.abs(bezier4x3_2.d.y - bezier4x3_2.a.y) / math.max(0.1f, y2);
            float3.z = math.abs(curve.m_Bezier.d.y - curve.m_Bezier.a.y) / math.max(0.1f, MathUtils.Length(curve.m_Bezier.xz));
            bool3 x = float3 >= new float3(netGeometryData.m_MaxSlopeSteepness * 2f, netGeometryData.m_MaxSlopeSteepness * 2f, netGeometryData.m_MaxSlopeSteepness + 0.0005f);
            if (math.any(x))
            {
              float4 float4 = new float4();
              if (x.x)
                float4 += new float4(math.lerp(MathUtils.Position(edgeGeometry.m_Start.m_Left, 0.5f), MathUtils.Position(edgeGeometry.m_Start.m_Right, 0.5f), 0.5f), 1f);
              if (x.y)
                float4 += new float4(math.lerp(MathUtils.Position(edgeGeometry.m_End.m_Left, 0.5f), MathUtils.Position(edgeGeometry.m_End.m_Right, 0.5f), 0.5f), 1f);
              if (x.z)
                float4 += new float4(math.lerp(edgeGeometry.m_Start.m_Left.d, edgeGeometry.m_Start.m_Right.d, 0.5f), 1f);
              errorQueue.Enqueue(new ErrorData()
              {
                m_ErrorType = ErrorType.SteepSlope,
                m_ErrorSeverity = ErrorSeverity.Error,
                m_Position = float4.xyz / float4.w,
                m_TempEntity = entity
              });
            }
          }
          if ((netGeometryData.m_Flags & GeometryFlags.RequireElevated) != (GeometryFlags) 0)
          {
            Elevation componentData1;
            // ISSUE: reference to a compiler-generated field
            data.m_NetElevation.TryGetComponent(edge.m_Start, out componentData1);
            Elevation componentData2;
            // ISSUE: reference to a compiler-generated field
            data.m_NetElevation.TryGetComponent(entity, out componentData2);
            Elevation componentData3;
            // ISSUE: reference to a compiler-generated field
            data.m_NetElevation.TryGetComponent(edge.m_End, out componentData3);
            if (!math.all(math.max((float2) math.max(math.cmin(componentData1.m_Elevation), math.cmin(componentData3.m_Elevation)), componentData2.m_Elevation) >= netGeometryData.m_ElevationLimit * 2f))
              errorQueue.Enqueue(new ErrorData()
              {
                m_ErrorType = ErrorType.LowElevation,
                m_ErrorSeverity = ErrorSeverity.Error,
                m_Position = math.lerp(edgeGeometry.m_Start.m_Left.d, edgeGeometry.m_Start.m_Right.d, 0.5f),
                m_TempEntity = entity
              });
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      if ((temp.m_Flags & (TempFlags.Create | TempFlags.Modify)) != (TempFlags) 0 && !flag3 && data.m_PlaceableNet.HasComponent(prefabRef.m_Prefab))
      {
        // ISSUE: reference to a compiler-generated field
        PlaceableNetData placeableNetData = data.m_PlaceableNet[prefabRef.m_Prefab];
        errorBounds.min = (float3) float.MaxValue;
        errorBounds.max = (float3) float.MinValue;
        if (ValidationHelpers.CheckSurface(waterSurfaceData, terrainHeightData, placeableNetData, compositionData1, edgeGeometry.m_Start, ref errorBounds) | ValidationHelpers.CheckSurface(waterSurfaceData, terrainHeightData, placeableNetData, compositionData1, edgeGeometry.m_End, ref errorBounds) | ValidationHelpers.CheckSurface(waterSurfaceData, terrainHeightData, placeableNetData, compositionData2, startNodeGeometry.m_Geometry.m_Left, ref errorBounds) | ValidationHelpers.CheckSurface(waterSurfaceData, terrainHeightData, placeableNetData, compositionData2, startNodeGeometry.m_Geometry.m_Right, ref errorBounds) | ValidationHelpers.CheckSurface(waterSurfaceData, terrainHeightData, placeableNetData, compositionData3, endNodeGeometry.m_Geometry.m_Left, ref errorBounds) | ValidationHelpers.CheckSurface(waterSurfaceData, terrainHeightData, placeableNetData, compositionData3, endNodeGeometry.m_Geometry.m_Right, ref errorBounds))
          errorQueue.Enqueue(new ErrorData()
          {
            m_ErrorType = (placeableNetData.m_PlacementFlags & PlacementFlags.Floating) == PlacementFlags.None ? ErrorType.InWater : ErrorType.NoWater,
            m_ErrorSeverity = ErrorSeverity.Error,
            m_Position = MathUtils.Center(errorBounds),
            m_TempEntity = entity
          });
      }
      if ((temp.m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Replace | TempFlags.Upgrade)) == (TempFlags) 0)
        return;
      Bounds3 bounds = edgeGeometry.m_Bounds;
      if (math.any(startNodeGeometry.m_Geometry.m_Left.m_Length > 0.05f) | math.any(startNodeGeometry.m_Geometry.m_Right.m_Length > 0.05f))
        bounds |= startNodeGeometry.m_Geometry.m_Bounds;
      if (math.any(endNodeGeometry.m_Geometry.m_Left.m_Length > 0.05f) | math.any(endNodeGeometry.m_Geometry.m_Right.m_Length > 0.05f))
        bounds |= endNodeGeometry.m_Geometry.m_Bounds;
      Game.Objects.ValidationHelpers.ValidateWorldBounds(entity, owner, bounds, data, terrainHeightData, errorQueue);
    }

    private static bool IsInternal(
      Entity topLevelEntity,
      Entity node,
      DynamicBuffer<ConnectedEdge> connectedEdges,
      ValidationSystem.EntityData data)
    {
      for (int index = 0; index < connectedEdges.Length; ++index)
      {
        Entity entity = connectedEdges[index].m_Edge;
        // ISSUE: reference to a compiler-generated field
        Edge edge = data.m_Edge[entity];
        if (edge.m_Start == node || edge.m_End == node)
        {
          Entity owner;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          for (; data.m_Owner.HasComponent(entity) && !data.m_Building.HasComponent(entity); entity = owner)
          {
            // ISSUE: reference to a compiler-generated field
            owner = data.m_Owner[entity].m_Owner;
            // ISSUE: reference to a compiler-generated field
            if (data.m_AssetStamp.HasComponent(owner))
              break;
          }
          if (topLevelEntity != entity)
            return false;
        }
      }
      return true;
    }

    private static bool CheckSurface(
      WaterSurfaceData waterSurfaceData,
      TerrainHeightData terrainHeightData,
      PlaceableNetData placeableNetData,
      NetCompositionData compositionData,
      Segment segment,
      ref Bounds3 errorBounds)
    {
      bool flag1 = false;
      bool flag2 = (placeableNetData.m_PlacementFlags & (PlacementFlags.OnGround | PlacementFlags.Floating)) == PlacementFlags.Floating;
      bool flag3 = (placeableNetData.m_PlacementFlags & (PlacementFlags.OnGround | PlacementFlags.Floating)) == PlacementFlags.OnGround & (compositionData.m_Flags.m_General & CompositionFlags.General.Tunnel) == (CompositionFlags.General) 0;
      if (flag2 | flag3)
      {
        float sampleInterval = WaterUtils.GetSampleInterval(ref waterSurfaceData);
        int num1 = (int) math.ceil(segment.middleLength / sampleInterval);
        for (int index1 = 0; index1 < num1; ++index1)
        {
          float t = ((float) index1 + 0.5f) / (float) num1;
          float3 x = MathUtils.Position(segment.m_Left, t);
          float3 y = MathUtils.Position(segment.m_Right, t);
          int num2 = (int) math.ceil(math.distance(x, y) / sampleInterval);
          for (int index2 = 0; index2 < num2; ++index2)
          {
            float s = ((float) index2 + 0.5f) / (float) num2;
            float3 worldPosition = math.lerp(x, y, s);
            float num3 = WaterUtils.SampleDepth(ref waterSurfaceData, worldPosition);
            if (flag3 && (double) num3 >= 0.20000000298023224 && (double) WaterUtils.SampleHeight(ref waterSurfaceData, ref terrainHeightData, worldPosition) > (double) worldPosition.y + (double) compositionData.m_HeightRange.min)
            {
              errorBounds |= worldPosition;
              flag1 = true;
            }
            if (flag2 && (double) num3 < 0.20000000298023224)
            {
              errorBounds |= worldPosition;
              flag1 = true;
            }
          }
        }
      }
      return flag1;
    }

    private static Entity GetNetNode(Entity entity, ValidationSystem.EntityData data)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return data.m_Temp.HasComponent(entity) ? data.m_Temp[entity].m_Original : entity;
    }

    private static Entity GetOwner(
      Entity entity,
      ValidationSystem.EntityData data,
      out Entity assetStamp)
    {
      assetStamp = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      while (data.m_Owner.HasComponent(entity) && !data.m_Building.HasComponent(entity))
      {
        // ISSUE: reference to a compiler-generated field
        Entity owner = data.m_Owner[entity].m_Owner;
        // ISSUE: reference to a compiler-generated field
        if (data.m_AssetStamp.HasComponent(owner))
        {
          assetStamp = owner;
          break;
        }
        entity = owner;
        // ISSUE: reference to a compiler-generated field
        if (data.m_Temp.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          entity = data.m_Temp[entity].m_Original;
        }
      }
      return entity;
    }

    public static void ValidateLane(
      Entity entity,
      Owner owner,
      Lane lane,
      TrackLane trackLane,
      Curve curve,
      EdgeLane edgeLane,
      PrefabRef prefabRef,
      ValidationSystem.EntityData data,
      NativeQueue<ErrorData>.ParallelWriter errorQueue)
    {
      // ISSUE: reference to a compiler-generated field
      if (!data.m_Edge.HasComponent(owner.m_Owner))
        return;
      // ISSUE: reference to a compiler-generated field
      TrackLaneData trackLaneData = data.m_TrackLaneData[prefabRef.m_Prefab];
      Temp componentData1;
      // ISSUE: reference to a compiler-generated field
      if ((double) trackLane.m_Curviness > (double) trackLaneData.m_MaxCurviness && data.m_Temp.TryGetComponent(owner.m_Owner, out componentData1) && (componentData1.m_Flags & TempFlags.Essential) != (TempFlags) 0)
        errorQueue.Enqueue(new ErrorData()
        {
          m_ErrorType = ErrorType.TightCurve,
          m_Position = MathUtils.Position(curve.m_Bezier, 0.5f),
          m_ErrorSeverity = ErrorSeverity.Error,
          m_TempEntity = owner.m_Owner
        });
      // ISSUE: reference to a compiler-generated field
      Edge edge = data.m_Edge[owner.m_Owner];
      bool flag1 = (trackLane.m_Flags & TrackLaneFlags.Twoway) != 0;
      bool flag2;
      Entity entity1;
      if ((double) edgeLane.m_EdgeDelta.x < 1.0 / 1000.0)
      {
        flag2 = ValidationHelpers.FindConnectedLane(edge.m_Start, lane.m_StartNode, data) || ValidationHelpers.IsIgnored(owner.m_Owner, edge.m_Start, data, trackLaneData.m_TrackTypes, flag1, true);
        entity1 = edge.m_Start;
      }
      else if ((double) edgeLane.m_EdgeDelta.x > 0.99900001287460327)
      {
        flag2 = ValidationHelpers.FindConnectedLane(edge.m_End, lane.m_StartNode, data) || ValidationHelpers.IsIgnored(owner.m_Owner, edge.m_End, data, trackLaneData.m_TrackTypes, flag1, true);
        entity1 = edge.m_End;
      }
      else
      {
        flag2 = true;
        entity1 = Entity.Null;
      }
      bool flag3;
      Entity entity2;
      if ((double) edgeLane.m_EdgeDelta.y < 1.0 / 1000.0)
      {
        flag3 = ValidationHelpers.FindConnectedLane(edge.m_Start, lane.m_EndNode, data) || ValidationHelpers.IsIgnored(owner.m_Owner, edge.m_Start, data, trackLaneData.m_TrackTypes, true, flag1);
        entity2 = edge.m_Start;
      }
      else if ((double) edgeLane.m_EdgeDelta.y > 0.99900001287460327)
      {
        flag3 = ValidationHelpers.FindConnectedLane(edge.m_End, lane.m_EndNode, data) || ValidationHelpers.IsIgnored(owner.m_Owner, edge.m_End, data, trackLaneData.m_TrackTypes, true, flag1);
        entity2 = edge.m_End;
      }
      else
      {
        flag3 = true;
        entity2 = Entity.Null;
      }
      Temp componentData2;
      // ISSUE: reference to a compiler-generated field
      if (!flag2 && data.m_Temp.TryGetComponent(entity1, out componentData2) && (componentData2.m_Flags & TempFlags.Essential) != (TempFlags) 0)
      {
        // ISSUE: reference to a compiler-generated field
        errorQueue.Enqueue(new ErrorData()
        {
          m_ErrorType = ErrorType.TightCurve,
          m_Position = (curve.m_Bezier.a + data.m_Node[entity1].m_Position) * 0.5f,
          m_ErrorSeverity = ErrorSeverity.Warning,
          m_TempEntity = entity1
        });
      }
      Temp componentData3;
      // ISSUE: reference to a compiler-generated field
      if (flag3 || !data.m_Temp.TryGetComponent(entity2, out componentData3) || (componentData3.m_Flags & TempFlags.Essential) == (TempFlags) 0)
        return;
      // ISSUE: reference to a compiler-generated field
      errorQueue.Enqueue(new ErrorData()
      {
        m_ErrorType = ErrorType.TightCurve,
        m_Position = (curve.m_Bezier.d + data.m_Node[entity2].m_Position) * 0.5f,
        m_ErrorSeverity = ErrorSeverity.Warning,
        m_TempEntity = entity2
      });
    }

    private static bool FindConnectedLane(
      Entity owner,
      PathNode node,
      ValidationSystem.EntityData data)
    {
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<SubLane> lane1 = data.m_Lanes[owner];
      for (int index = 0; index < lane1.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        Lane lane2 = data.m_Lane[lane1[index].m_SubLane];
        if (lane2.m_StartNode.Equals(node) || lane2.m_EndNode.Equals(node))
          return true;
      }
      return false;
    }

    private static bool IsIgnored(
      Entity edge,
      Entity node,
      ValidationSystem.EntityData data,
      TrackTypes trackTypes,
      bool isSource,
      bool isTarget)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EdgeIterator edgeIterator = new EdgeIterator(edge, node, data.m_ConnectedEdges, data.m_Edge, data.m_Temp, data.m_Hidden);
      EdgeIteratorValue edgeIteratorValue;
      while (edgeIterator.GetNext(out edgeIteratorValue))
      {
        DynamicBuffer<SubLane> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!(edgeIteratorValue.m_Edge == edge) && data.m_Lanes.TryGetBuffer(edgeIteratorValue.m_Edge, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            Entity subLane = bufferData[index].m_SubLane;
            TrackLane componentData;
            // ISSUE: reference to a compiler-generated field
            if (data.m_TrackLane.TryGetComponent(subLane, out componentData))
            {
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef = data.m_PrefabRef[subLane];
              // ISSUE: reference to a compiler-generated field
              if (data.m_TrackLaneData[prefabRef.m_Prefab].m_TrackTypes == trackTypes)
              {
                int num = (componentData.m_Flags & TrackLaneFlags.Twoway) != 0 ? 1 : 0;
                bool flag1 = (componentData.m_Flags & TrackLaneFlags.Invert) != 0;
                bool flag2 = (num | (edgeIteratorValue.m_End != flag1 ? 1 : 0)) != 0;
                bool flag3 = (num | (edgeIteratorValue.m_End == flag1 ? 1 : 0)) != 0;
                if (isSource & flag3 | isTarget & flag2)
                  return false;
              }
            }
          }
        }
      }
      return true;
    }

    private static bool CheckGeometryShape(EdgeGeometry geometry, ref Bounds3 errorBounds)
    {
      return math.any(geometry.m_Start.m_Length + geometry.m_End.m_Length > 0.1f) && ValidationHelpers.CheckSegmentShape(geometry.m_Start, ref errorBounds) | ValidationHelpers.CheckSegmentShape(geometry.m_End, ref errorBounds);
    }

    private static bool CheckGeometryShape(EdgeNodeGeometry geometry, ref Bounds3 errorBounds)
    {
      return math.any(geometry.m_Left.m_Length > 0.05f) | math.any(geometry.m_Right.m_Length > 0.05f) && ValidationHelpers.CheckSegmentShape(geometry.m_Left, ref errorBounds) | ValidationHelpers.CheckSegmentShape(geometry.m_Right, ref errorBounds);
    }

    private static bool CheckSegmentShape(Segment segment, ref Bounds3 errorBounds)
    {
      bool flag = false;
      Quad3 quad;
      quad.a = segment.m_Left.a;
      quad.b = segment.m_Right.a;
      float3 y1 = quad.b - quad.a;
      for (int index = 1; index <= 8; ++index)
      {
        float t = (float) index / 8f;
        quad.d = MathUtils.Position(segment.m_Left, t);
        quad.c = MathUtils.Position(segment.m_Right, t);
        float3 float3_1 = quad.d - quad.a;
        float3 float3_2 = quad.c - quad.b;
        float3 y2 = quad.c - quad.d;
        float3 x1 = math.select(float3_1, (float3) 0.0f, (double) math.lengthsq(float3_1) < 9.9999997473787516E-05);
        float3 x2 = math.select(float3_2, (float3) 0.0f, (double) math.lengthsq(float3_2) < 9.9999997473787516E-05);
        if ((double) math.cross(x1, y1).y < 0.0 | (double) math.cross(x2, y2).y < 0.0)
        {
          errorBounds |= MathUtils.Bounds(quad);
          flag = true;
        }
        quad.a = quad.d;
        quad.b = quad.c;
        y1 = y2;
      }
      return flag;
    }

    public static bool Intersect(
      Edge edge1,
      Entity node2,
      EdgeGeometry edgeGeometry1,
      float3 offset1,
      Box3 box2,
      Bounds3 bounds2,
      NetCompositionData prefabCompositionData1,
      DynamicBuffer<NetCompositionArea> areas1,
      ref Bounds3 intersection)
    {
      if (!MathUtils.Intersect(edgeGeometry1.m_Bounds, bounds2) || math.all(edgeGeometry1.m_Start.m_Length + edgeGeometry1.m_End.m_Length <= 0.1f))
        return false;
      bool flag = false;
      if (edge1.m_Start != node2 || (prefabCompositionData1.m_State & CompositionState.HasSurface) != (CompositionState) 0)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, new float2(0.0f, 1f), offset1, box2, bounds2, prefabCompositionData1, areas1, ref intersection);
      if (edge1.m_End != node2 || (prefabCompositionData1.m_State & CompositionState.HasSurface) != (CompositionState) 0)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, new float2(0.0f, 1f), offset1, box2, bounds2, prefabCompositionData1, areas1, ref intersection);
      return flag;
    }

    public static bool Intersect(
      Edge edge1,
      Entity node2,
      EdgeGeometry edgeGeometry1,
      float2 offset1,
      Quad2 quad2,
      Bounds2 bounds2,
      NetCompositionData prefabCompositionData1,
      DynamicBuffer<NetCompositionArea> areas1,
      ref Bounds2 intersection)
    {
      if (!MathUtils.Intersect(edgeGeometry1.m_Bounds.xz, bounds2) || math.all(edgeGeometry1.m_Start.m_Length + edgeGeometry1.m_End.m_Length <= 0.1f))
        return false;
      bool flag = false;
      if (edge1.m_Start != node2 || (prefabCompositionData1.m_State & CompositionState.HasSurface) != (CompositionState) 0)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, new float2(0.0f, 1f), offset1, quad2, bounds2, prefabCompositionData1, areas1, ref intersection);
      if (edge1.m_End != node2 || (prefabCompositionData1.m_State & CompositionState.HasSurface) != (CompositionState) 0)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, new float2(0.0f, 1f), offset1, quad2, bounds2, prefabCompositionData1, areas1, ref intersection);
      return flag;
    }

    public static bool Intersect(
      Entity node1,
      Entity node2,
      EdgeNodeGeometry nodeGeometry1,
      float3 offset1,
      Box3 box2,
      Bounds3 bounds2,
      NetCompositionData prefabCompositionData1,
      DynamicBuffer<NetCompositionArea> areas1,
      ref Bounds3 intersection)
    {
      if (!MathUtils.Intersect(nodeGeometry1.m_Bounds, bounds2) || node1 == node2 || math.all(nodeGeometry1.m_Left.m_Length <= 0.05f) && math.all(nodeGeometry1.m_Left.m_Length <= 0.05f))
        return false;
      if ((double) nodeGeometry1.m_MiddleRadius > 0.0)
      {
        Segment right1 = nodeGeometry1.m_Right;
        Segment right2 = nodeGeometry1.m_Right;
        right1.m_Right = MathUtils.Lerp(nodeGeometry1.m_Right.m_Left, nodeGeometry1.m_Right.m_Right, 0.5f);
        right1.m_Right.d = nodeGeometry1.m_Middle.d;
        right2.m_Left = right1.m_Right;
        return ValidationHelpers.Intersect(nodeGeometry1.m_Left, new float2(0.0f, 1f), offset1, box2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(right1, new float2(0.0f, 0.5f), offset1, box2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(right2, new float2(0.5f, 1f), offset1, box2, bounds2, prefabCompositionData1, areas1, ref intersection);
      }
      Segment left = nodeGeometry1.m_Left;
      Segment right = nodeGeometry1.m_Right;
      left.m_Right = nodeGeometry1.m_Middle;
      right.m_Left = nodeGeometry1.m_Middle;
      return ValidationHelpers.Intersect(nodeGeometry1.m_Left, new float2(0.0f, 0.5f), offset1, box2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(nodeGeometry1.m_Right, new float2(0.5f, 1f), offset1, box2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(left, new float2(0.0f, 0.5f), offset1, box2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(right, new float2(0.5f, 1f), offset1, box2, bounds2, prefabCompositionData1, areas1, ref intersection);
    }

    public static bool Intersect(
      Entity node1,
      Entity node2,
      EdgeNodeGeometry nodeGeometry1,
      float2 offset1,
      Quad2 quad2,
      Bounds2 bounds2,
      NetCompositionData prefabCompositionData1,
      DynamicBuffer<NetCompositionArea> areas1,
      ref Bounds2 intersection)
    {
      if (!MathUtils.Intersect(nodeGeometry1.m_Bounds.xz, bounds2) || node1 == node2 || math.all(nodeGeometry1.m_Left.m_Length <= 0.05f) && math.all(nodeGeometry1.m_Left.m_Length <= 0.05f))
        return false;
      if ((double) nodeGeometry1.m_MiddleRadius > 0.0)
      {
        Segment right1 = nodeGeometry1.m_Right;
        Segment right2 = nodeGeometry1.m_Right;
        right1.m_Right = MathUtils.Lerp(nodeGeometry1.m_Right.m_Left, nodeGeometry1.m_Right.m_Right, 0.5f);
        right1.m_Right.d = nodeGeometry1.m_Middle.d;
        right2.m_Left = right1.m_Right;
        return ValidationHelpers.Intersect(nodeGeometry1.m_Left, new float2(0.0f, 1f), offset1, quad2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(right1, new float2(0.0f, 0.5f), offset1, quad2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(right2, new float2(0.5f, 1f), offset1, quad2, bounds2, prefabCompositionData1, areas1, ref intersection);
      }
      Segment left = nodeGeometry1.m_Left;
      Segment right = nodeGeometry1.m_Right;
      left.m_Right = nodeGeometry1.m_Middle;
      right.m_Left = nodeGeometry1.m_Middle;
      return ValidationHelpers.Intersect(nodeGeometry1.m_Left, new float2(0.0f, 0.5f), offset1, quad2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(nodeGeometry1.m_Right, new float2(0.5f, 1f), offset1, quad2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(left, new float2(0.0f, 0.5f), offset1, quad2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(right, new float2(0.5f, 1f), offset1, quad2, bounds2, prefabCompositionData1, areas1, ref intersection);
    }

    public static bool Intersect(
      Segment segment1,
      float2 segmentSide1,
      float3 offset1,
      Box3 box2,
      Bounds3 bounds2,
      NetCompositionData prefabCompositionData1,
      DynamicBuffer<NetCompositionArea> areas1,
      ref Bounds3 intersection)
    {
      if (!MathUtils.Intersect(ValidationHelpers.SetHeightRange(MathUtils.Bounds(segment1.m_Left) | MathUtils.Bounds(segment1.m_Right), prefabCompositionData1.m_HeightRange), bounds2))
        return false;
      float3 float3_1;
      if (areas1.IsCreated)
      {
        for (int index = 0; index < areas1.Length; ++index)
        {
          NetCompositionArea netCompositionArea = areas1[index];
          if ((netCompositionArea.m_Flags & (NetAreaFlags.Buildable | NetAreaFlags.Hole)) != (NetAreaFlags) 0)
          {
            float num1 = netCompositionArea.m_Width * 0.51f;
            float3 x = MathUtils.Size(box2.bounds) * 0.5f;
            if ((double) math.cmin(x) < (double) num1)
            {
              float num2 = (float) ((double) netCompositionArea.m_Position.x / (double) prefabCompositionData1.m_Width + 0.5);
              if ((double) num2 >= (double) segmentSide1.x && (double) num2 <= (double) segmentSide1.y)
              {
                Bezier4x3 curve1 = MathUtils.Lerp(segment1.m_Left, segment1.m_Right, (float) (((double) num2 - (double) segmentSide1.x) / ((double) segmentSide1.y - (double) segmentSide1.x)));
                Bounds3 bounds = MathUtils.Bounds(curve1);
                bounds.min.xz -= num1;
                bounds.max.xz += num1;
                if (MathUtils.Intersect(ValidationHelpers.SetHeightRange(bounds, prefabCompositionData1.m_HeightRange), bounds2))
                {
                  Bezier4x3 curve2 = curve1 + offset1;
                  float3 float3_2 = math.mul(box2.rotation, MathUtils.Center(box2.bounds));
                  float3_1 = math.mul(box2.rotation, new float3(x.x, 0.0f, 0.0f));
                  float2 xz1 = float3_1.xz;
                  float3_1 = math.mul(box2.rotation, new float3(0.0f, x.y, 0.0f));
                  float2 xz2 = float3_1.xz;
                  float3_1 = math.mul(box2.rotation, new float3(0.0f, 0.0f, x.z));
                  float2 xz3 = float3_1.xz;
                  float t;
                  double num3 = (double) MathUtils.Distance(curve2.xz, float3_2.xz, out t);
                  float3 float3_3 = MathUtils.Position(curve2, t);
                  if ((netCompositionArea.m_Flags & NetAreaFlags.Hole) != (NetAreaFlags) 0 || (double) bounds2.min.y + (double) offset1.y > (double) float3_3.y + (double) prefabCompositionData1.m_HeightRange.min)
                  {
                    float3_1 = MathUtils.Tangent(curve2, t);
                    float2 y = MathUtils.Right(math.normalizesafe(float3_1.xz));
                    if ((double) math.abs(math.dot(float3_2.xz - float3_3.xz, y)) + (double) math.csum(math.abs(new float3(math.dot(xz1, y), math.dot(xz2, y), math.dot(xz3, y)))) < (double) num1)
                      return false;
                  }
                }
              }
            }
          }
        }
      }
      bool flag = false;
      Quad3 quad3_1;
      quad3_1.a = segment1.m_Left.a;
      quad3_1.b = segment1.m_Right.a;
      Bounds3 bounds3_1 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(quad3_1.a, quad3_1.b), prefabCompositionData1.m_HeightRange);
      for (int index = 1; index <= 8; ++index)
      {
        float t = (float) index / 8f;
        quad3_1.d = MathUtils.Position(segment1.m_Left, t);
        quad3_1.c = MathUtils.Position(segment1.m_Right, t);
        Bounds3 bounds3_2 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(quad3_1.d, quad3_1.c), prefabCompositionData1.m_HeightRange);
        if (MathUtils.Intersect(bounds3_1 | bounds3_2, bounds2))
        {
          Quad3 quad3_2 = quad3_1;
          float3 x1 = quad3_2.b - quad3_2.a;
          float3_1 = new float3();
          float3 defaultvalue1 = float3_1;
          float3 float3_4 = math.normalizesafe(x1, defaultvalue1) * 0.5f;
          float3 x2 = quad3_2.d - quad3_2.c;
          float3_1 = new float3();
          float3 defaultvalue2 = float3_1;
          float3 float3_5 = math.normalizesafe(x2, defaultvalue2) * 0.5f;
          quad3_2.a += float3_4;
          quad3_2.b -= float3_4;
          quad3_2.c += float3_5;
          quad3_2.d -= float3_5;
          TrigonalTrapezohedron3 trapezohedron = new TrigonalTrapezohedron3(quad3_2, quad3_2);
          float3 float3_6 = new float3(offset1.x, offset1.y + prefabCompositionData1.m_HeightRange.min, offset1.z);
          float3 float3_7 = new float3(offset1.x, offset1.y + prefabCompositionData1.m_HeightRange.max, offset1.z);
          trapezohedron.a.a += float3_6;
          trapezohedron.a.b += float3_6;
          trapezohedron.a.c += float3_6;
          trapezohedron.a.d += float3_6;
          trapezohedron.b.a += float3_7;
          trapezohedron.b.b += float3_7;
          trapezohedron.b.c += float3_7;
          trapezohedron.b.d += float3_7;
          Bounds3 intersection1;
          Bounds3 intersection2;
          if (MathUtils.Intersect(trapezohedron, box2, out intersection1, out intersection2))
          {
            Box3 box = new Box3(intersection2, box2.rotation);
            flag = true;
            intersection |= intersection1 | MathUtils.Bounds(box);
          }
        }
        quad3_1.a = quad3_1.d;
        quad3_1.b = quad3_1.c;
        bounds3_1 = bounds3_2;
      }
      return flag;
    }

    public static bool Intersect(
      Segment segment1,
      float2 segmentSide1,
      float2 offset1,
      Quad2 quad2,
      Bounds2 bounds2,
      NetCompositionData prefabCompositionData1,
      DynamicBuffer<NetCompositionArea> areas1,
      ref Bounds2 intersection)
    {
      if (!MathUtils.Intersect((MathUtils.Bounds(segment1.m_Left) | MathUtils.Bounds(segment1.m_Right)).xz, bounds2))
        return false;
      if (areas1.IsCreated)
      {
        for (int index = 0; index < areas1.Length; ++index)
        {
          NetCompositionArea netCompositionArea = areas1[index];
          if ((netCompositionArea.m_Flags & (NetAreaFlags.Buildable | NetAreaFlags.Hole)) != (NetAreaFlags) 0)
          {
            float2 x1 = new float2(math.max(math.distance(quad2.a, quad2.b), math.distance(quad2.c, quad2.d)), math.max(math.distance(quad2.b, quad2.c), math.distance(quad2.d, quad2.a)));
            float num1 = netCompositionArea.m_Width * 0.51f;
            if ((double) math.cmin(x1) * 0.5 < (double) num1)
            {
              float num2 = (float) ((double) netCompositionArea.m_Position.x / (double) prefabCompositionData1.m_Width + 0.5);
              if ((double) num2 >= (double) segmentSide1.x && (double) num2 <= (double) segmentSide1.y)
              {
                Bezier4x2 xz = MathUtils.Lerp(segment1.m_Left, segment1.m_Right, (float) (((double) num2 - (double) segmentSide1.x) / ((double) segmentSide1.y - (double) segmentSide1.x))).xz;
                Bounds2 bounds1 = MathUtils.Bounds(xz);
                bounds1.min -= num1;
                bounds1.max += num1;
                if (MathUtils.Intersect(bounds1, bounds2))
                {
                  Bezier4x2 curve = xz + offset1;
                  float2 position = MathUtils.Center(quad2);
                  float t;
                  double num3 = (double) MathUtils.Distance(curve, position, out t);
                  float2 float2 = MathUtils.Position(curve, t);
                  float2 x2 = MathUtils.Right(math.normalizesafe(MathUtils.Tangent(curve, t)));
                  if ((double) math.cmax(math.abs(new float4(math.dot(x2, quad2.a - float2), math.dot(x2, quad2.b - float2), math.dot(x2, quad2.c - float2), math.dot(x2, quad2.d - float2)))) < (double) num1)
                    return false;
                }
              }
            }
          }
        }
      }
      bool flag = false;
      Quad2 quad2_1;
      quad2_1.a = segment1.m_Left.a.xz;
      quad2_1.b = segment1.m_Right.a.xz;
      Bounds2 bounds2_1 = MathUtils.Bounds(quad2_1.a, quad2_1.b);
      for (int index = 1; index <= 8; ++index)
      {
        float t = (float) index / 8f;
        ref Quad2 local1 = ref quad2_1;
        float3 float3 = MathUtils.Position(segment1.m_Left, t);
        float2 xz1 = float3.xz;
        local1.d = xz1;
        ref Quad2 local2 = ref quad2_1;
        float3 = MathUtils.Position(segment1.m_Right, t);
        float2 xz2 = float3.xz;
        local2.c = xz2;
        Bounds2 bounds2_2 = MathUtils.Bounds(quad2_1.d, quad2_1.c);
        if (MathUtils.Intersect(bounds2_1 | bounds2_2, bounds2))
        {
          Quad2 quad1 = quad2_1;
          float2 float2_1 = math.normalizesafe(quad1.b - quad1.a) * 0.5f;
          float2 float2_2 = math.normalizesafe(quad1.d - quad1.c) * 0.5f;
          quad1.a += float2_1;
          quad1.b -= float2_1;
          quad1.c += float2_2;
          quad1.d -= float2_2;
          quad1.a += offset1;
          quad1.b += offset1;
          quad1.c += offset1;
          quad1.d += offset1;
          Bounds2 intersection1;
          if (MathUtils.Intersect(quad1, quad2, out intersection1))
          {
            flag = true;
            intersection |= intersection1;
          }
        }
        quad2_1.a = quad2_1.d;
        quad2_1.b = quad2_1.c;
        bounds2_1 = bounds2_2;
      }
      return flag;
    }

    public static bool Intersect(
      Edge edge1,
      Entity node2,
      EdgeGeometry edgeGeometry1,
      Triangle2 triangle2,
      Bounds3 bounds2,
      NetCompositionData prefabCompositionData1,
      ref Bounds3 intersection)
    {
      if (!MathUtils.Intersect(edgeGeometry1.m_Bounds.xz, bounds2.xz) || math.all(edgeGeometry1.m_Start.m_Length + edgeGeometry1.m_End.m_Length <= 0.1f))
        return false;
      bool flag = false;
      if (edge1.m_Start != node2)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, new float2(0.0f, 1f), triangle2, bounds2, prefabCompositionData1, ref intersection);
      if (edge1.m_End != node2)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, new float2(0.0f, 1f), triangle2, bounds2, prefabCompositionData1, ref intersection);
      return flag;
    }

    public static bool Intersect(
      Edge edge1,
      Entity node2,
      EdgeGeometry edgeGeometry1,
      Triangle3 triangle2,
      NetCompositionData prefabCompositionData1,
      Bounds1 heightRange2,
      ref Bounds3 intersection)
    {
      Bounds3 bounds2 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(triangle2), heightRange2);
      if (!MathUtils.Intersect(edgeGeometry1.m_Bounds, bounds2) || math.all(edgeGeometry1.m_Start.m_Length + edgeGeometry1.m_End.m_Length <= 0.1f))
        return false;
      bool flag = false;
      if (edge1.m_Start != node2)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, new float2(0.0f, 1f), triangle2, prefabCompositionData1, heightRange2, ref intersection);
      if (edge1.m_End != node2)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, new float2(0.0f, 1f), triangle2, prefabCompositionData1, heightRange2, ref intersection);
      return flag;
    }

    public static bool Intersect(
      Edge edge1,
      Entity node2,
      EdgeGeometry edgeGeometry1,
      Line2.Segment line2,
      Bounds3 bounds2,
      NetCompositionData prefabCompositionData1,
      ref Bounds3 intersection)
    {
      if (!MathUtils.Intersect(edgeGeometry1.m_Bounds.xz, bounds2.xz) || math.all(edgeGeometry1.m_Start.m_Length + edgeGeometry1.m_End.m_Length <= 0.1f))
        return false;
      bool flag = false;
      if (edge1.m_Start != node2)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, new float2(0.0f, 1f), line2, bounds2, prefabCompositionData1, ref intersection);
      if (edge1.m_End != node2)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, new float2(0.0f, 1f), line2, bounds2, prefabCompositionData1, ref intersection);
      return flag;
    }

    public static bool Intersect(
      Entity node1,
      Entity node2,
      EdgeNodeGeometry nodeGeometry1,
      Triangle2 triangle2,
      Bounds3 bounds2,
      NetCompositionData prefabCompositionData1,
      ref Bounds3 intersection)
    {
      if (!MathUtils.Intersect(nodeGeometry1.m_Bounds.xz, bounds2.xz) || node1 == node2 || math.all(nodeGeometry1.m_Left.m_Length <= 0.05f) && math.all(nodeGeometry1.m_Left.m_Length <= 0.05f))
        return false;
      if ((double) nodeGeometry1.m_MiddleRadius > 0.0)
      {
        Segment right1 = nodeGeometry1.m_Right;
        Segment right2 = nodeGeometry1.m_Right;
        right1.m_Right = MathUtils.Lerp(nodeGeometry1.m_Right.m_Left, nodeGeometry1.m_Right.m_Right, 0.5f);
        right1.m_Right.d = nodeGeometry1.m_Middle.d;
        right2.m_Left = right1.m_Right;
        return ValidationHelpers.Intersect(nodeGeometry1.m_Left, new float2(0.0f, 1f), triangle2, bounds2, prefabCompositionData1, ref intersection) | ValidationHelpers.Intersect(right1, new float2(0.0f, 0.5f), triangle2, bounds2, prefabCompositionData1, ref intersection) | ValidationHelpers.Intersect(right2, new float2(0.5f, 1f), triangle2, bounds2, prefabCompositionData1, ref intersection);
      }
      Segment left = nodeGeometry1.m_Left;
      Segment right = nodeGeometry1.m_Right;
      left.m_Right = nodeGeometry1.m_Middle;
      right.m_Left = nodeGeometry1.m_Middle;
      return ValidationHelpers.Intersect(nodeGeometry1.m_Left, new float2(0.0f, 0.5f), triangle2, bounds2, prefabCompositionData1, ref intersection) | ValidationHelpers.Intersect(nodeGeometry1.m_Right, new float2(0.5f, 1f), triangle2, bounds2, prefabCompositionData1, ref intersection) | ValidationHelpers.Intersect(left, new float2(0.0f, 0.5f), triangle2, bounds2, prefabCompositionData1, ref intersection) | ValidationHelpers.Intersect(right, new float2(0.5f, 1f), triangle2, bounds2, prefabCompositionData1, ref intersection);
    }

    public static bool Intersect(
      Entity node1,
      Entity node2,
      EdgeNodeGeometry nodeGeometry1,
      Triangle3 triangle2,
      NetCompositionData prefabCompositionData1,
      Bounds1 heightRange2,
      ref Bounds3 intersection)
    {
      Bounds3 bounds2 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(triangle2), heightRange2);
      if (!MathUtils.Intersect(nodeGeometry1.m_Bounds, bounds2) || node1 == node2 || math.all(nodeGeometry1.m_Left.m_Length <= 0.05f) && math.all(nodeGeometry1.m_Left.m_Length <= 0.05f))
        return false;
      if ((double) nodeGeometry1.m_MiddleRadius > 0.0)
      {
        Segment right1 = nodeGeometry1.m_Right;
        Segment right2 = nodeGeometry1.m_Right;
        right1.m_Right = MathUtils.Lerp(nodeGeometry1.m_Right.m_Left, nodeGeometry1.m_Right.m_Right, 0.5f);
        right1.m_Right.d = nodeGeometry1.m_Middle.d;
        right2.m_Left = right1.m_Right;
        return ValidationHelpers.Intersect(nodeGeometry1.m_Left, new float2(0.0f, 1f), triangle2, prefabCompositionData1, heightRange2, ref intersection) | ValidationHelpers.Intersect(right1, new float2(0.0f, 0.5f), triangle2, prefabCompositionData1, heightRange2, ref intersection) | ValidationHelpers.Intersect(right2, new float2(0.5f, 1f), triangle2, prefabCompositionData1, heightRange2, ref intersection);
      }
      Segment left = nodeGeometry1.m_Left;
      Segment right = nodeGeometry1.m_Right;
      left.m_Right = nodeGeometry1.m_Middle;
      right.m_Left = nodeGeometry1.m_Middle;
      return ValidationHelpers.Intersect(nodeGeometry1.m_Left, new float2(0.0f, 0.5f), triangle2, prefabCompositionData1, heightRange2, ref intersection) | ValidationHelpers.Intersect(nodeGeometry1.m_Right, new float2(0.5f, 1f), triangle2, prefabCompositionData1, heightRange2, ref intersection) | ValidationHelpers.Intersect(left, new float2(0.0f, 0.5f), triangle2, prefabCompositionData1, heightRange2, ref intersection) | ValidationHelpers.Intersect(right, new float2(0.5f, 1f), triangle2, prefabCompositionData1, heightRange2, ref intersection);
    }

    public static bool Intersect(
      Entity node1,
      Entity node2,
      EdgeNodeGeometry nodeGeometry1,
      Line2.Segment line2,
      Bounds3 bounds2,
      NetCompositionData prefabCompositionData1,
      ref Bounds3 intersection)
    {
      if (!MathUtils.Intersect(nodeGeometry1.m_Bounds.xz, bounds2.xz) || node1 == node2 || math.all(nodeGeometry1.m_Left.m_Length <= 0.05f) && math.all(nodeGeometry1.m_Left.m_Length <= 0.05f))
        return false;
      if ((double) nodeGeometry1.m_MiddleRadius > 0.0)
      {
        Segment right1 = nodeGeometry1.m_Right;
        Segment right2 = nodeGeometry1.m_Right;
        right1.m_Right = MathUtils.Lerp(nodeGeometry1.m_Right.m_Left, nodeGeometry1.m_Right.m_Right, 0.5f);
        right1.m_Right.d = nodeGeometry1.m_Middle.d;
        right2.m_Left = right1.m_Right;
        return ValidationHelpers.Intersect(nodeGeometry1.m_Left, new float2(0.0f, 1f), line2, bounds2, prefabCompositionData1, ref intersection) | ValidationHelpers.Intersect(right1, new float2(0.0f, 0.5f), line2, bounds2, prefabCompositionData1, ref intersection) | ValidationHelpers.Intersect(right2, new float2(0.5f, 1f), line2, bounds2, prefabCompositionData1, ref intersection);
      }
      Segment left = nodeGeometry1.m_Left;
      Segment right = nodeGeometry1.m_Right;
      left.m_Right = nodeGeometry1.m_Middle;
      right.m_Left = nodeGeometry1.m_Middle;
      return ValidationHelpers.Intersect(nodeGeometry1.m_Left, new float2(0.0f, 0.5f), line2, bounds2, prefabCompositionData1, ref intersection) | ValidationHelpers.Intersect(nodeGeometry1.m_Right, new float2(0.5f, 1f), line2, bounds2, prefabCompositionData1, ref intersection) | ValidationHelpers.Intersect(left, new float2(0.0f, 0.5f), line2, bounds2, prefabCompositionData1, ref intersection) | ValidationHelpers.Intersect(right, new float2(0.5f, 1f), line2, bounds2, prefabCompositionData1, ref intersection);
    }

    public static bool Intersect(
      Segment segment1,
      float2 segmentSide1,
      Triangle2 triangle2,
      Bounds3 bounds2,
      NetCompositionData prefabCompositionData1,
      ref Bounds3 intersection)
    {
      if (!MathUtils.Intersect(ValidationHelpers.SetHeightRange(MathUtils.Bounds(segment1.m_Left) | MathUtils.Bounds(segment1.m_Right), prefabCompositionData1.m_HeightRange).xz, bounds2.xz))
        return false;
      bool flag = false;
      Quad3 quad3;
      quad3.a = segment1.m_Left.a;
      quad3.b = segment1.m_Right.a;
      Bounds3 bounds3_1 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(quad3.a, quad3.b), prefabCompositionData1.m_HeightRange);
      for (int index = 1; index <= 8; ++index)
      {
        float t = (float) index / 8f;
        quad3.d = MathUtils.Position(segment1.m_Left, t);
        quad3.c = MathUtils.Position(segment1.m_Right, t);
        Bounds3 bounds3_2 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(quad3.d, quad3.c), prefabCompositionData1.m_HeightRange);
        Bounds3 bounds3_3 = bounds3_1 | bounds3_2;
        if (MathUtils.Intersect(bounds3_3.xz, bounds2.xz))
        {
          Quad2 xz = quad3.xz;
          float2 float2_1 = math.normalizesafe(xz.b - xz.a) * 0.5f;
          float2 float2_2 = math.normalizesafe(xz.d - xz.c) * 0.5f;
          xz.a += float2_1;
          xz.b -= float2_1;
          xz.c += float2_2;
          xz.d -= float2_2;
          if (MathUtils.Intersect(xz, triangle2))
          {
            flag = true;
            intersection |= bounds3_3 & bounds2;
          }
        }
        quad3.a = quad3.d;
        quad3.b = quad3.c;
        bounds3_1 = bounds3_2;
      }
      return flag;
    }

    public static bool Intersect(
      Segment segment1,
      float2 segmentSide1,
      Line2.Segment line2,
      Bounds3 bounds2,
      NetCompositionData prefabCompositionData1,
      ref Bounds3 intersection)
    {
      if (!MathUtils.Intersect(ValidationHelpers.SetHeightRange(MathUtils.Bounds(segment1.m_Left) | MathUtils.Bounds(segment1.m_Right), prefabCompositionData1.m_HeightRange).xz, bounds2.xz))
        return false;
      bool flag = false;
      Quad3 quad3;
      quad3.a = segment1.m_Left.a;
      quad3.b = segment1.m_Right.a;
      Bounds3 bounds3_1 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(quad3.a, quad3.b), prefabCompositionData1.m_HeightRange);
      for (int index = 1; index <= 8; ++index)
      {
        float t1 = (float) index / 8f;
        quad3.d = MathUtils.Position(segment1.m_Left, t1);
        quad3.c = MathUtils.Position(segment1.m_Right, t1);
        Bounds3 bounds3_2 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(quad3.d, quad3.c), prefabCompositionData1.m_HeightRange);
        Bounds3 bounds3_3 = bounds3_1 | bounds3_2;
        if (MathUtils.Intersect(bounds3_3.xz, bounds2.xz))
        {
          Quad2 xz = quad3.xz;
          float2 float2_1 = math.normalizesafe(xz.b - xz.a) * 0.5f;
          float2 x = xz.d - xz.c;
          float2 t2 = new float2();
          float2 defaultvalue = t2;
          float2 float2_2 = math.normalizesafe(x, defaultvalue) * 0.5f;
          xz.a += float2_1;
          xz.b -= float2_1;
          xz.c += float2_2;
          xz.d -= float2_2;
          if (MathUtils.Intersect(xz, line2, out t2))
          {
            flag = true;
            intersection |= bounds3_3 & bounds2;
          }
        }
        quad3.a = quad3.d;
        quad3.b = quad3.c;
        bounds3_1 = bounds3_2;
      }
      return flag;
    }

    public static bool Intersect(
      Edge edge1,
      Entity node2,
      EdgeGeometry edgeGeometry1,
      float3 offset1,
      Cylinder3 cylinder2,
      Bounds3 bounds2,
      NetCompositionData prefabCompositionData1,
      DynamicBuffer<NetCompositionArea> areas1,
      ref Bounds3 intersection)
    {
      if (!MathUtils.Intersect(edgeGeometry1.m_Bounds, bounds2) || math.all(edgeGeometry1.m_Start.m_Length + edgeGeometry1.m_End.m_Length <= 0.1f))
        return false;
      bool flag = false;
      if (edge1.m_Start != node2 || (prefabCompositionData1.m_State & CompositionState.HasSurface) != (CompositionState) 0)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, new float2(0.0f, 1f), offset1, cylinder2, bounds2, prefabCompositionData1, areas1, ref intersection);
      if (edge1.m_End != node2 || (prefabCompositionData1.m_State & CompositionState.HasSurface) != (CompositionState) 0)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, new float2(0.0f, 1f), offset1, cylinder2, bounds2, prefabCompositionData1, areas1, ref intersection);
      return flag;
    }

    public static bool Intersect(
      Edge edge1,
      Entity node2,
      EdgeGeometry edgeGeometry1,
      float2 offset1,
      Circle2 circle2,
      Bounds2 bounds2,
      NetCompositionData prefabCompositionData1,
      DynamicBuffer<NetCompositionArea> areas1,
      ref Bounds2 intersection)
    {
      if (!MathUtils.Intersect(edgeGeometry1.m_Bounds.xz, bounds2) || math.all(edgeGeometry1.m_Start.m_Length + edgeGeometry1.m_End.m_Length <= 0.1f))
        return false;
      bool flag = false;
      if (edge1.m_Start != node2 || (prefabCompositionData1.m_State & CompositionState.HasSurface) != (CompositionState) 0)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, new float2(0.0f, 1f), offset1, circle2, bounds2, prefabCompositionData1, areas1, ref intersection);
      if (edge1.m_End != node2 || (prefabCompositionData1.m_State & CompositionState.HasSurface) != (CompositionState) 0)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, new float2(0.0f, 1f), offset1, circle2, bounds2, prefabCompositionData1, areas1, ref intersection);
      return flag;
    }

    public static bool Intersect(
      Entity node1,
      Entity node2,
      EdgeNodeGeometry nodeGeometry1,
      float3 offset1,
      Cylinder3 cylinder2,
      Bounds3 bounds2,
      NetCompositionData prefabCompositionData1,
      DynamicBuffer<NetCompositionArea> areas1,
      ref Bounds3 intersection)
    {
      if (!MathUtils.Intersect(nodeGeometry1.m_Bounds, bounds2) || node1 == node2 || math.all(nodeGeometry1.m_Left.m_Length <= 0.05f) && math.all(nodeGeometry1.m_Left.m_Length <= 0.05f))
        return false;
      if ((double) nodeGeometry1.m_MiddleRadius > 0.0)
      {
        Segment right1 = nodeGeometry1.m_Right;
        Segment right2 = nodeGeometry1.m_Right;
        right1.m_Right = MathUtils.Lerp(nodeGeometry1.m_Right.m_Left, nodeGeometry1.m_Right.m_Right, 0.5f);
        right1.m_Right.d = nodeGeometry1.m_Middle.d;
        right2.m_Left = right1.m_Right;
        return ValidationHelpers.Intersect(nodeGeometry1.m_Left, new float2(0.0f, 1f), offset1, cylinder2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(right1, new float2(0.0f, 0.5f), offset1, cylinder2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(right2, new float2(0.5f, 1f), offset1, cylinder2, bounds2, prefabCompositionData1, areas1, ref intersection);
      }
      Segment left = nodeGeometry1.m_Left;
      Segment right = nodeGeometry1.m_Right;
      left.m_Right = nodeGeometry1.m_Middle;
      right.m_Left = nodeGeometry1.m_Middle;
      return ValidationHelpers.Intersect(nodeGeometry1.m_Left, new float2(0.0f, 0.5f), offset1, cylinder2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(nodeGeometry1.m_Right, new float2(0.5f, 1f), offset1, cylinder2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(left, new float2(0.0f, 0.5f), offset1, cylinder2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(right, new float2(0.5f, 1f), offset1, cylinder2, bounds2, prefabCompositionData1, areas1, ref intersection);
    }

    public static bool Intersect(
      Entity node1,
      Entity node2,
      EdgeNodeGeometry nodeGeometry1,
      float2 offset1,
      Circle2 circle2,
      Bounds2 bounds2,
      NetCompositionData prefabCompositionData1,
      DynamicBuffer<NetCompositionArea> areas1,
      ref Bounds2 intersection)
    {
      if (!MathUtils.Intersect(nodeGeometry1.m_Bounds.xz, bounds2) || node1 == node2 || math.all(nodeGeometry1.m_Left.m_Length <= 0.05f) && math.all(nodeGeometry1.m_Left.m_Length <= 0.05f))
        return false;
      if ((double) nodeGeometry1.m_MiddleRadius > 0.0)
      {
        Segment right1 = nodeGeometry1.m_Right;
        Segment right2 = nodeGeometry1.m_Right;
        right1.m_Right = MathUtils.Lerp(nodeGeometry1.m_Right.m_Left, nodeGeometry1.m_Right.m_Right, 0.5f);
        right1.m_Right.d = nodeGeometry1.m_Middle.d;
        right2.m_Left = right1.m_Right;
        return ValidationHelpers.Intersect(nodeGeometry1.m_Left, new float2(0.0f, 1f), offset1, circle2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(right1, new float2(0.0f, 0.5f), offset1, circle2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(right2, new float2(0.5f, 1f), offset1, circle2, bounds2, prefabCompositionData1, areas1, ref intersection);
      }
      Segment left = nodeGeometry1.m_Left;
      Segment right = nodeGeometry1.m_Right;
      left.m_Right = nodeGeometry1.m_Middle;
      right.m_Left = nodeGeometry1.m_Middle;
      return ValidationHelpers.Intersect(nodeGeometry1.m_Left, new float2(0.0f, 0.5f), offset1, circle2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(nodeGeometry1.m_Right, new float2(0.5f, 1f), offset1, circle2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(left, new float2(0.0f, 0.5f), offset1, circle2, bounds2, prefabCompositionData1, areas1, ref intersection) | ValidationHelpers.Intersect(right, new float2(0.5f, 1f), offset1, circle2, bounds2, prefabCompositionData1, areas1, ref intersection);
    }

    public static bool Intersect(
      Segment segment1,
      float2 segmentSide1,
      float3 offset1,
      Cylinder3 cylinder2,
      Bounds3 bounds2,
      NetCompositionData prefabCompositionData1,
      DynamicBuffer<NetCompositionArea> areas1,
      ref Bounds3 intersection)
    {
      if (!MathUtils.Intersect(ValidationHelpers.SetHeightRange(MathUtils.Bounds(segment1.m_Left) | MathUtils.Bounds(segment1.m_Right), prefabCompositionData1.m_HeightRange), bounds2))
        return false;
      if (areas1.IsCreated)
      {
        for (int index = 0; index < areas1.Length; ++index)
        {
          NetCompositionArea netCompositionArea = areas1[index];
          if ((netCompositionArea.m_Flags & (NetAreaFlags.Buildable | NetAreaFlags.Hole)) != (NetAreaFlags) 0)
          {
            float num1 = netCompositionArea.m_Width * 0.51f;
            if ((double) cylinder2.circle.radius < (double) num1)
            {
              float num2 = (float) ((double) netCompositionArea.m_Position.x / (double) prefabCompositionData1.m_Width + 0.5);
              if ((double) num2 >= (double) segmentSide1.x && (double) num2 <= (double) segmentSide1.y)
              {
                Bezier4x3 curve = MathUtils.Lerp(segment1.m_Left, segment1.m_Right, (float) (((double) num2 - (double) segmentSide1.x) / ((double) segmentSide1.y - (double) segmentSide1.x)));
                Bounds3 bounds = MathUtils.Bounds(curve);
                bounds.min.xz -= num1;
                bounds.max.xz += num1;
                if (MathUtils.Intersect(ValidationHelpers.SetHeightRange(bounds, prefabCompositionData1.m_HeightRange), bounds2))
                {
                  curve += offset1;
                  float3 float3_1 = math.mul(cylinder2.rotation, new float3(cylinder2.circle.position.x, cylinder2.height.min, cylinder2.circle.position.y));
                  float t;
                  float num3 = MathUtils.Distance(curve.xz, float3_1.xz, out t);
                  if ((netCompositionArea.m_Flags & NetAreaFlags.Hole) == (NetAreaFlags) 0)
                  {
                    float3 float3_2 = MathUtils.Position(curve, t);
                    if ((double) bounds2.min.y + (double) offset1.y <= (double) float3_2.y + (double) prefabCompositionData1.m_HeightRange.min)
                      continue;
                  }
                  if ((double) t == 0.0)
                  {
                    float num4 = math.dot(math.normalizesafe(MathUtils.StartTangent(curve).xz), curve.a.xz - float3_1.xz);
                    if ((double) num4 < (double) cylinder2.circle.radius && (double) num1 * (double) num1 - 2.0 * (double) num1 * (double) math.sqrt(math.max(0.0f, (float) ((double) num3 * (double) num3 - (double) num4 * (double) num4))) + (double) num3 * (double) num3 > (double) cylinder2.circle.radius * (double) cylinder2.circle.radius)
                      return false;
                  }
                  else if ((double) t == 1.0)
                  {
                    float num5 = math.dot(math.normalizesafe(MathUtils.EndTangent(curve).xz), float3_1.xz - curve.d.xz);
                    if ((double) num5 < (double) cylinder2.circle.radius && (double) num1 * (double) num1 - 2.0 * (double) num1 * (double) math.sqrt(math.max(0.0f, (float) ((double) num3 * (double) num3 - (double) num5 * (double) num5))) + (double) num3 * (double) num3 > (double) cylinder2.circle.radius * (double) cylinder2.circle.radius)
                      return false;
                  }
                  else if ((double) num3 < (double) num1 - (double) cylinder2.circle.radius)
                    return false;
                }
              }
            }
          }
        }
      }
      bool flag = false;
      Quad3 quad3;
      quad3.a = segment1.m_Left.a;
      quad3.b = segment1.m_Right.a;
      Bounds3 bounds3_1 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(quad3.a, quad3.b), prefabCompositionData1.m_HeightRange);
      for (int index = 1; index <= 8; ++index)
      {
        float t = (float) index / 8f;
        quad3.d = MathUtils.Position(segment1.m_Left, t);
        quad3.c = MathUtils.Position(segment1.m_Right, t);
        Bounds3 bounds3_2 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(quad3.d, quad3.c), prefabCompositionData1.m_HeightRange);
        if (MathUtils.Intersect(bounds3_1 | bounds3_2, bounds2))
        {
          Quad3 quad1 = quad3;
          quad1.a += offset1;
          quad1.b += offset1;
          quad1.c += offset1;
          quad1.d += offset1;
          Bounds3 intersection1;
          Bounds3 intersection2;
          if (ValidationHelpers.QuadCylinderIntersect(quad1, cylinder2, out intersection1, out intersection2))
          {
            intersection1 = ValidationHelpers.SetHeightRange(intersection1, prefabCompositionData1.m_HeightRange);
            Bounds3 intersection3;
            if (MathUtils.Intersect(intersection1, intersection2, out intersection3))
            {
              flag = true;
              intersection |= intersection3;
            }
          }
        }
        quad3.a = quad3.d;
        quad3.b = quad3.c;
        bounds3_1 = bounds3_2;
      }
      return flag;
    }

    public static bool Intersect(
      Segment segment1,
      float2 segmentSide1,
      float2 offset1,
      Circle2 circle2,
      Bounds2 bounds2,
      NetCompositionData prefabCompositionData1,
      DynamicBuffer<NetCompositionArea> areas1,
      ref Bounds2 intersection)
    {
      if (!MathUtils.Intersect((MathUtils.Bounds(segment1.m_Left) | MathUtils.Bounds(segment1.m_Right)).xz, bounds2))
        return false;
      if (areas1.IsCreated)
      {
        for (int index = 0; index < areas1.Length; ++index)
        {
          NetCompositionArea netCompositionArea = areas1[index];
          if ((netCompositionArea.m_Flags & (NetAreaFlags.Buildable | NetAreaFlags.Hole)) != (NetAreaFlags) 0)
          {
            float num1 = netCompositionArea.m_Width * 0.51f;
            if ((double) circle2.radius < (double) num1)
            {
              float num2 = (float) ((double) netCompositionArea.m_Position.x / (double) prefabCompositionData1.m_Width + 0.5);
              if ((double) num2 >= (double) segmentSide1.x && (double) num2 <= (double) segmentSide1.y)
              {
                Bezier4x2 xz = MathUtils.Lerp(segment1.m_Left, segment1.m_Right, (float) (((double) num2 - (double) segmentSide1.x) / ((double) segmentSide1.y - (double) segmentSide1.x))).xz;
                Bounds2 bounds1 = MathUtils.Bounds(xz);
                bounds1.min -= num1;
                bounds1.max += num1;
                if (MathUtils.Intersect(bounds1, bounds2))
                {
                  Bezier4x2 curve = xz + offset1;
                  float t;
                  float num3 = MathUtils.Distance(curve, circle2.position, out t);
                  if ((double) t == 0.0)
                  {
                    float num4 = math.dot(math.normalizesafe(MathUtils.StartTangent(curve)), curve.a - circle2.position);
                    if ((double) num4 < (double) circle2.radius && (double) num1 * (double) num1 - 2.0 * (double) num1 * (double) math.sqrt(math.max(0.0f, (float) ((double) num3 * (double) num3 - (double) num4 * (double) num4))) + (double) num3 * (double) num3 > (double) circle2.radius * (double) circle2.radius)
                      return false;
                  }
                  else if ((double) t == 1.0)
                  {
                    float num5 = math.dot(math.normalizesafe(MathUtils.EndTangent(curve)), circle2.position - curve.d);
                    if ((double) num5 < (double) circle2.radius && (double) num1 * (double) num1 - 2.0 * (double) num1 * (double) math.sqrt(math.max(0.0f, (float) ((double) num3 * (double) num3 - (double) num5 * (double) num5))) + (double) num3 * (double) num3 > (double) circle2.radius * (double) circle2.radius)
                      return false;
                  }
                  else if ((double) num3 < (double) num1 - (double) circle2.radius)
                    return false;
                }
              }
            }
          }
        }
      }
      bool flag = false;
      Quad2 quad2;
      quad2.a = segment1.m_Left.a.xz;
      quad2.b = segment1.m_Right.a.xz;
      Bounds2 bounds2_1 = MathUtils.Bounds(quad2.a, quad2.b);
      for (int index = 1; index <= 8; ++index)
      {
        float t = (float) index / 8f;
        ref Quad2 local1 = ref quad2;
        float3 float3 = MathUtils.Position(segment1.m_Left, t);
        float2 xz1 = float3.xz;
        local1.d = xz1;
        ref Quad2 local2 = ref quad2;
        float3 = MathUtils.Position(segment1.m_Right, t);
        float2 xz2 = float3.xz;
        local2.c = xz2;
        Bounds2 bounds2_2 = MathUtils.Bounds(quad2.d, quad2.c);
        if (MathUtils.Intersect(bounds2_1 | bounds2_2, bounds2))
        {
          Quad2 quad = quad2;
          quad.a += offset1;
          quad.b += offset1;
          quad.c += offset1;
          quad.d += offset1;
          Bounds2 intersection1;
          if (MathUtils.Intersect(quad, circle2, out intersection1))
          {
            flag = true;
            intersection |= intersection1;
          }
        }
        quad2.a = quad2.d;
        quad2.b = quad2.c;
        bounds2_1 = bounds2_2;
      }
      return flag;
    }

    public static bool QuadCylinderIntersect(
      Quad3 quad1,
      Cylinder3 cylinder2,
      out Bounds3 intersection1,
      out Bounds3 intersection2)
    {
      intersection1.min = (float3) float.MaxValue;
      intersection1.max = (float3) float.MinValue;
      intersection2.min = (float3) float.MaxValue;
      intersection2.max = (float3) float.MinValue;
      Line3.Segment line1_1 = new Line3.Segment(quad1.a, quad1.b);
      Line3.Segment line1_2 = new Line3.Segment(quad1.b, quad1.c);
      Line3.Segment line1_3 = new Line3.Segment(quad1.c, quad1.d);
      Line3.Segment line1_4 = new Line3.Segment(quad1.d, quad1.a);
      float3 float3_1 = math.mul(cylinder2.rotation, new float3(cylinder2.circle.position.x, cylinder2.height.min, cylinder2.circle.position.y));
      float3 float3_2 = math.mul(cylinder2.rotation, new float3(cylinder2.circle.position.x, cylinder2.height.max, cylinder2.circle.position.y));
      Circle2 circle = cylinder2.circle with
      {
        position = float3_1.xz
      };
      Bounds1 height2_1 = MathUtils.Bounds(float3_1.y, float3_2.y);
      Line3 line2;
      line2.a = new float3(circle.position.x, height2_1.min, circle.position.y);
      line2.b = new float3(circle.position.x, height2_1.max, circle.position.y);
      Circle2 circle2 = circle;
      Bounds1 height2_2 = height2_1;
      ref Bounds3 local1 = ref intersection1;
      ref Bounds3 local2 = ref intersection2;
      return ValidationHelpers.QuadCylinderIntersectHelper(line1_1, circle2, height2_2, ref local1, ref local2) | ValidationHelpers.QuadCylinderIntersectHelper(line1_2, circle, height2_1, ref intersection1, ref intersection2) | ValidationHelpers.QuadCylinderIntersectHelper(line1_3, circle, height2_1, ref intersection1, ref intersection2) | ValidationHelpers.QuadCylinderIntersectHelper(line1_4, circle, height2_1, ref intersection1, ref intersection2) | ValidationHelpers.QuadCylinderIntersectHelper(quad1, line2, ref intersection1, ref intersection2);
    }

    public static bool TriangleCylinderIntersect(
      Triangle3 triangle1,
      Cylinder3 cylinder2,
      out Bounds3 intersection1,
      out Bounds3 intersection2)
    {
      intersection1.min = (float3) float.MaxValue;
      intersection1.max = (float3) float.MinValue;
      intersection2.min = (float3) float.MaxValue;
      intersection2.max = (float3) float.MinValue;
      Line3.Segment line1_1 = new Line3.Segment(triangle1.a, triangle1.b);
      Line3.Segment line1_2 = new Line3.Segment(triangle1.b, triangle1.c);
      Line3.Segment line1_3 = new Line3.Segment(triangle1.c, triangle1.a);
      float3 float3_1 = math.mul(cylinder2.rotation, new float3(cylinder2.circle.position.x, cylinder2.height.min, cylinder2.circle.position.y));
      float3 float3_2 = math.mul(cylinder2.rotation, new float3(cylinder2.circle.position.x, cylinder2.height.max, cylinder2.circle.position.y));
      Circle2 circle = cylinder2.circle with
      {
        position = float3_1.xz
      };
      Bounds1 height2_1 = MathUtils.Bounds(float3_1.y, float3_2.y);
      Line3 line2;
      line2.a = new float3(circle.position.x, height2_1.min, circle.position.y);
      line2.b = new float3(circle.position.x, height2_1.max, circle.position.y);
      Circle2 circle2 = circle;
      Bounds1 height2_2 = height2_1;
      ref Bounds3 local1 = ref intersection1;
      ref Bounds3 local2 = ref intersection2;
      return ValidationHelpers.QuadCylinderIntersectHelper(line1_1, circle2, height2_2, ref local1, ref local2) | ValidationHelpers.QuadCylinderIntersectHelper(line1_2, circle, height2_1, ref intersection1, ref intersection2) | ValidationHelpers.QuadCylinderIntersectHelper(line1_3, circle, height2_1, ref intersection1, ref intersection2) | ValidationHelpers.TriangleCylinderIntersectHelper(triangle1, line2, ref intersection1, ref intersection2);
    }

    private static bool QuadCylinderIntersectHelper(
      Line3.Segment line1,
      Circle2 circle2,
      Bounds1 height2,
      ref Bounds3 intersection1,
      ref Bounds3 intersection2)
    {
      float2 t;
      if (!MathUtils.Intersect(circle2, line1.xz, out t))
        return false;
      float3 float3_1 = MathUtils.Position(line1, t.x);
      float3 float3_2 = MathUtils.Position(line1, t.y);
      intersection1 |= float3_1;
      intersection1 |= float3_2;
      float3_1.y = height2.min;
      float3_2.y = height2.max;
      intersection2 |= float3_1;
      intersection2 |= float3_2;
      return true;
    }

    private static bool QuadCylinderIntersectHelper(
      Quad3 quad1,
      Line3 line2,
      ref Bounds3 intersection1,
      ref Bounds3 intersection2)
    {
      float t;
      if (!MathUtils.Intersect(quad1, line2, out t))
        return false;
      intersection1 |= MathUtils.Position(line2, t);
      intersection2 |= MathUtils.Position(line2, math.saturate(t));
      return true;
    }

    private static bool TriangleCylinderIntersectHelper(
      Triangle3 triangle1,
      Line3 line2,
      ref Bounds3 intersection1,
      ref Bounds3 intersection2)
    {
      float3 t;
      if (!MathUtils.Intersect(triangle1, line2, out t))
        return false;
      intersection1 |= MathUtils.Position(line2, t.z);
      intersection2 |= MathUtils.Position(line2, math.saturate(t.z));
      return true;
    }

    public static bool Intersect(
      Edge edge1,
      Edge edge2,
      EdgeGeometry edgeGeometry1,
      EdgeGeometry edgeGeometry2,
      NetCompositionData prefabCompositionData1,
      NetCompositionData prefabCompositionData2,
      ref Bounds3 intersection)
    {
      if (!MathUtils.Intersect(edgeGeometry1.m_Bounds, edgeGeometry2.m_Bounds) || math.all(edgeGeometry1.m_Start.m_Length + edgeGeometry1.m_End.m_Length <= 0.1f) || math.all(edgeGeometry2.m_Start.m_Length + edgeGeometry2.m_End.m_Length <= 0.1f))
        return false;
      bool flag = false;
      if (edge1.m_Start != edge2.m_Start)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, edgeGeometry2.m_Start, prefabCompositionData1, prefabCompositionData2, ref intersection);
      if (edge1.m_Start != edge2.m_End)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, edgeGeometry2.m_End, prefabCompositionData1, prefabCompositionData2, ref intersection);
      if (edge1.m_End != edge2.m_Start)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, edgeGeometry2.m_Start, prefabCompositionData1, prefabCompositionData2, ref intersection);
      if (edge1.m_End != edge2.m_End)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, edgeGeometry2.m_End, prefabCompositionData1, prefabCompositionData2, ref intersection);
      return flag;
    }

    public static bool Intersect(
      Edge edge1,
      Edge edge2,
      EdgeGeometry edgeGeometry1,
      EdgeGeometry edgeGeometry2,
      NetCompositionData prefabCompositionData1,
      NetCompositionData prefabCompositionData2,
      ref Bounds2 intersection)
    {
      if (!MathUtils.Intersect(edgeGeometry1.m_Bounds.xz, edgeGeometry2.m_Bounds.xz) || math.all(edgeGeometry1.m_Start.m_Length + edgeGeometry1.m_End.m_Length <= 0.1f) || math.all(edgeGeometry2.m_Start.m_Length + edgeGeometry2.m_End.m_Length <= 0.1f))
        return false;
      bool flag = false;
      if (edge1.m_Start != edge2.m_Start)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, edgeGeometry2.m_Start, prefabCompositionData1, prefabCompositionData2, ref intersection);
      if (edge1.m_Start != edge2.m_End)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, edgeGeometry2.m_End, prefabCompositionData1, prefabCompositionData2, ref intersection);
      if (edge1.m_End != edge2.m_Start)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, edgeGeometry2.m_Start, prefabCompositionData1, prefabCompositionData2, ref intersection);
      if (edge1.m_End != edge2.m_End)
        flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, edgeGeometry2.m_End, prefabCompositionData1, prefabCompositionData2, ref intersection);
      return flag;
    }

    public static bool Intersect(
      Edge edge1,
      Edge originalEdge1,
      NativeArray<ConnectedNode> nodes1,
      NativeArray<ConnectedNode> originalNodes1,
      Entity node2,
      EdgeGeometry edgeGeometry1,
      EdgeNodeGeometry nodeGeometry2,
      NetCompositionData prefabCompositionData1,
      NetCompositionData prefabCompositionData2,
      ref Bounds3 intersection)
    {
      if (!MathUtils.Intersect(edgeGeometry1.m_Bounds, nodeGeometry2.m_Bounds) || math.all(edgeGeometry1.m_Start.m_Length + edgeGeometry1.m_End.m_Length <= 0.1f) || math.all(nodeGeometry2.m_Left.m_Length <= 0.05f) && math.all(nodeGeometry2.m_Left.m_Length <= 0.05f))
        return false;
      for (int index = 0; index < nodes1.Length; ++index)
      {
        if (nodes1[index].m_Node == node2)
          return false;
      }
      for (int index = 0; index < originalNodes1.Length; ++index)
      {
        if (originalNodes1[index].m_Node == node2)
          return false;
      }
      bool flag = false;
      if ((double) nodeGeometry2.m_MiddleRadius > 0.0)
      {
        Segment right1 = nodeGeometry2.m_Right;
        Segment right2 = nodeGeometry2.m_Right;
        right1.m_Right = MathUtils.Lerp(nodeGeometry2.m_Right.m_Left, nodeGeometry2.m_Right.m_Right, 0.5f);
        right1.m_Right.d = nodeGeometry2.m_Middle.d;
        right2.m_Left = right1.m_Right;
        if (edge1.m_Start != node2 && originalEdge1.m_Start != node2)
          flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, right1, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, right2, prefabCompositionData1, prefabCompositionData2, ref intersection);
        if (edge1.m_End != node2 && originalEdge1.m_End != node2)
          flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, right1, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, right2, prefabCompositionData1, prefabCompositionData2, ref intersection);
      }
      else
      {
        Segment left = nodeGeometry2.m_Left;
        Segment right = nodeGeometry2.m_Right;
        left.m_Right = nodeGeometry2.m_Middle;
        right.m_Left = nodeGeometry2.m_Middle;
        if (edge1.m_Start != node2 && originalEdge1.m_Start != node2)
          flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, nodeGeometry2.m_Right, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, right, prefabCompositionData1, prefabCompositionData2, ref intersection);
        if (edge1.m_End != node2 && originalEdge1.m_End != node2)
          flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, nodeGeometry2.m_Right, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, right, prefabCompositionData1, prefabCompositionData2, ref intersection);
      }
      return flag;
    }

    public static bool Intersect(
      Edge edge1,
      NativeArray<ConnectedNode> nodes1,
      Entity node2,
      Entity originalNode2,
      EdgeGeometry edgeGeometry1,
      EdgeNodeGeometry nodeGeometry2,
      NetCompositionData prefabCompositionData1,
      NetCompositionData prefabCompositionData2,
      ref Bounds3 intersection)
    {
      if (!MathUtils.Intersect(edgeGeometry1.m_Bounds, nodeGeometry2.m_Bounds) || math.all(edgeGeometry1.m_Start.m_Length + edgeGeometry1.m_End.m_Length <= 0.1f) || math.all(nodeGeometry2.m_Left.m_Length <= 0.05f) && math.all(nodeGeometry2.m_Left.m_Length <= 0.05f))
        return false;
      for (int index = 0; index < nodes1.Length; ++index)
      {
        Entity node = nodes1[index].m_Node;
        if (node == node2 || node == originalNode2)
          return false;
      }
      bool flag = false;
      if ((double) nodeGeometry2.m_MiddleRadius > 0.0)
      {
        Segment right1 = nodeGeometry2.m_Right;
        Segment right2 = nodeGeometry2.m_Right;
        right1.m_Right = MathUtils.Lerp(nodeGeometry2.m_Right.m_Left, nodeGeometry2.m_Right.m_Right, 0.5f);
        right1.m_Right.d = nodeGeometry2.m_Middle.d;
        right2.m_Left = right1.m_Right;
        if (edge1.m_Start != node2 && edge1.m_Start != originalNode2)
          flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, right1, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, right2, prefabCompositionData1, prefabCompositionData2, ref intersection);
        if (edge1.m_End != node2 && edge1.m_End != originalNode2)
          flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, right1, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, right2, prefabCompositionData1, prefabCompositionData2, ref intersection);
      }
      else
      {
        Segment left = nodeGeometry2.m_Left;
        Segment right = nodeGeometry2.m_Right;
        left.m_Right = nodeGeometry2.m_Middle;
        right.m_Left = nodeGeometry2.m_Middle;
        if (edge1.m_Start != node2 && edge1.m_Start != originalNode2)
          flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, nodeGeometry2.m_Right, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, right, prefabCompositionData1, prefabCompositionData2, ref intersection);
        if (edge1.m_End != node2 && edge1.m_End != originalNode2)
          flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, nodeGeometry2.m_Right, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, right, prefabCompositionData1, prefabCompositionData2, ref intersection);
      }
      return flag;
    }

    public static bool Intersect(
      Edge edge1,
      Edge originalEdge1,
      NativeArray<ConnectedNode> nodes1,
      NativeArray<ConnectedNode> originalNodes1,
      Entity node2,
      EdgeGeometry edgeGeometry1,
      EdgeNodeGeometry nodeGeometry2,
      NetCompositionData prefabCompositionData1,
      NetCompositionData prefabCompositionData2,
      ref Bounds2 intersection)
    {
      if (!MathUtils.Intersect(edgeGeometry1.m_Bounds.xz, nodeGeometry2.m_Bounds.xz) || math.all(edgeGeometry1.m_Start.m_Length + edgeGeometry1.m_End.m_Length <= 0.1f) || math.all(nodeGeometry2.m_Left.m_Length <= 0.05f) && math.all(nodeGeometry2.m_Left.m_Length <= 0.05f))
        return false;
      for (int index = 0; index < nodes1.Length; ++index)
      {
        if (nodes1[index].m_Node == node2)
          return false;
      }
      for (int index = 0; index < originalNodes1.Length; ++index)
      {
        if (originalNodes1[index].m_Node == node2)
          return false;
      }
      bool flag = false;
      if ((double) nodeGeometry2.m_MiddleRadius > 0.0)
      {
        Segment right1 = nodeGeometry2.m_Right;
        Segment right2 = nodeGeometry2.m_Right;
        right1.m_Right = MathUtils.Lerp(nodeGeometry2.m_Right.m_Left, nodeGeometry2.m_Right.m_Right, 0.5f);
        right1.m_Right.d = nodeGeometry2.m_Middle.d;
        right2.m_Left = right1.m_Right;
        if (edge1.m_Start != node2 && originalEdge1.m_Start != node2)
          flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, right1, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, right2, prefabCompositionData1, prefabCompositionData2, ref intersection);
        if (edge1.m_End != node2 && originalEdge1.m_End != node2)
          flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, right1, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, right2, prefabCompositionData1, prefabCompositionData2, ref intersection);
      }
      else
      {
        Segment left = nodeGeometry2.m_Left;
        Segment right = nodeGeometry2.m_Right;
        left.m_Right = nodeGeometry2.m_Middle;
        right.m_Left = nodeGeometry2.m_Middle;
        if (edge1.m_Start != node2 && originalEdge1.m_Start != node2)
          flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, nodeGeometry2.m_Right, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, right, prefabCompositionData1, prefabCompositionData2, ref intersection);
        if (edge1.m_End != node2 && originalEdge1.m_End != node2)
          flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, nodeGeometry2.m_Right, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, right, prefabCompositionData1, prefabCompositionData2, ref intersection);
      }
      return flag;
    }

    public static bool Intersect(
      Edge edge1,
      NativeArray<ConnectedNode> nodes1,
      Entity node2,
      Entity originalNode2,
      EdgeGeometry edgeGeometry1,
      EdgeNodeGeometry nodeGeometry2,
      NetCompositionData prefabCompositionData1,
      NetCompositionData prefabCompositionData2,
      ref Bounds2 intersection)
    {
      if (!MathUtils.Intersect(edgeGeometry1.m_Bounds.xz, nodeGeometry2.m_Bounds.xz) || math.all(edgeGeometry1.m_Start.m_Length + edgeGeometry1.m_End.m_Length <= 0.1f) || math.all(nodeGeometry2.m_Left.m_Length <= 0.05f) && math.all(nodeGeometry2.m_Left.m_Length <= 0.05f))
        return false;
      for (int index = 0; index < nodes1.Length; ++index)
      {
        Entity node = nodes1[index].m_Node;
        if (node == node2 || node == originalNode2)
          return false;
      }
      bool flag = false;
      if ((double) nodeGeometry2.m_MiddleRadius > 0.0)
      {
        Segment right1 = nodeGeometry2.m_Right;
        Segment right2 = nodeGeometry2.m_Right;
        right1.m_Right = MathUtils.Lerp(nodeGeometry2.m_Right.m_Left, nodeGeometry2.m_Right.m_Right, 0.5f);
        right1.m_Right.d = nodeGeometry2.m_Middle.d;
        right2.m_Left = right1.m_Right;
        if (edge1.m_Start != node2 && edge1.m_Start != originalNode2)
          flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, right1, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, right2, prefabCompositionData1, prefabCompositionData2, ref intersection);
        if (edge1.m_End != node2 && edge1.m_End != originalNode2)
          flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, right1, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, right2, prefabCompositionData1, prefabCompositionData2, ref intersection);
      }
      else
      {
        Segment left = nodeGeometry2.m_Left;
        Segment right = nodeGeometry2.m_Right;
        left.m_Right = nodeGeometry2.m_Middle;
        right.m_Left = nodeGeometry2.m_Middle;
        if (edge1.m_Start != node2 && edge1.m_Start != originalNode2)
          flag |= ValidationHelpers.Intersect(edgeGeometry1.m_Start, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, nodeGeometry2.m_Right, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_Start, right, prefabCompositionData1, prefabCompositionData2, ref intersection);
        if (edge1.m_End != node2 && edge1.m_End != originalNode2)
          flag |= ValidationHelpers.Intersect(edgeGeometry1.m_End, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, nodeGeometry2.m_Right, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(edgeGeometry1.m_End, right, prefabCompositionData1, prefabCompositionData2, ref intersection);
      }
      return flag;
    }

    public static bool Intersect(
      Entity node1,
      Entity originalNode1,
      NativeArray<ConnectedNode> nodes1,
      NativeArray<ConnectedNode> originalNodes1,
      Entity node2,
      NativeArray<ConnectedNode> nodes2,
      EdgeNodeGeometry nodeGeometry1,
      EdgeNodeGeometry nodeGeometry2,
      NetCompositionData prefabCompositionData1,
      NetCompositionData prefabCompositionData2,
      ref Bounds3 intersection)
    {
      if (!MathUtils.Intersect(nodeGeometry1.m_Bounds, nodeGeometry2.m_Bounds) || node1 == node2 || originalNode1 == node2 || math.all(nodeGeometry1.m_Left.m_Length <= 0.05f) && math.all(nodeGeometry1.m_Left.m_Length <= 0.05f) || math.all(nodeGeometry2.m_Left.m_Length <= 0.05f) && math.all(nodeGeometry2.m_Left.m_Length <= 0.05f))
        return false;
      for (int index = 0; index < nodes1.Length; ++index)
      {
        if (nodes1[index].m_Node == node2)
          return false;
      }
      for (int index = 0; index < originalNodes1.Length; ++index)
      {
        if (originalNodes1[index].m_Node == node2)
          return false;
      }
      for (int index = 0; index < nodes2.Length; ++index)
      {
        Entity node = nodes2[index].m_Node;
        if (node == node1 || node == originalNode1)
          return false;
      }
      Segment segment1;
      Segment right1;
      if ((double) nodeGeometry1.m_MiddleRadius > 0.0)
      {
        segment1 = nodeGeometry1.m_Right;
        right1 = nodeGeometry1.m_Right;
        segment1.m_Right = MathUtils.Lerp(nodeGeometry1.m_Right.m_Left, nodeGeometry1.m_Right.m_Right, 0.5f);
        segment1.m_Right.d = nodeGeometry1.m_Middle.d;
        right1.m_Left = segment1.m_Right;
      }
      else
      {
        segment1 = nodeGeometry1.m_Left;
        right1 = nodeGeometry1.m_Right;
        segment1.m_Right = nodeGeometry1.m_Middle;
        right1.m_Left = nodeGeometry1.m_Middle;
      }
      Segment segment2;
      Segment right2;
      if ((double) nodeGeometry2.m_MiddleRadius > 0.0)
      {
        segment2 = nodeGeometry2.m_Right;
        right2 = nodeGeometry2.m_Right;
        segment2.m_Right = MathUtils.Lerp(nodeGeometry2.m_Right.m_Left, nodeGeometry2.m_Right.m_Right, 0.5f);
        segment2.m_Right.d = nodeGeometry2.m_Middle.d;
        right2.m_Left = segment2.m_Right;
      }
      else
      {
        segment2 = nodeGeometry2.m_Left;
        right2 = nodeGeometry2.m_Right;
        segment2.m_Right = nodeGeometry2.m_Middle;
        right2.m_Left = nodeGeometry2.m_Middle;
      }
      bool flag = ValidationHelpers.Intersect(nodeGeometry1.m_Left, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(nodeGeometry1.m_Left, segment2, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(nodeGeometry1.m_Left, right2, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(segment1, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(segment1, segment2, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(segment1, right2, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(right1, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(right1, segment2, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(right1, right2, prefabCompositionData1, prefabCompositionData2, ref intersection);
      if ((double) nodeGeometry1.m_MiddleRadius <= 0.0)
        flag |= ValidationHelpers.Intersect(nodeGeometry1.m_Right, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(nodeGeometry1.m_Right, segment2, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(nodeGeometry1.m_Right, right2, prefabCompositionData1, prefabCompositionData2, ref intersection);
      if ((double) nodeGeometry2.m_MiddleRadius <= 0.0)
        flag |= ValidationHelpers.Intersect(nodeGeometry1.m_Left, nodeGeometry2.m_Right, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(segment1, nodeGeometry2.m_Right, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(right1, nodeGeometry2.m_Right, prefabCompositionData1, prefabCompositionData2, ref intersection);
      if ((double) nodeGeometry1.m_MiddleRadius <= 0.0 && (double) nodeGeometry2.m_MiddleRadius <= 0.0)
        flag |= ValidationHelpers.Intersect(nodeGeometry1.m_Right, nodeGeometry2.m_Right, prefabCompositionData1, prefabCompositionData2, ref intersection);
      return flag;
    }

    public static bool Intersect(
      Entity node1,
      Entity originalNode1,
      NativeArray<ConnectedNode> nodes1,
      NativeArray<ConnectedNode> originalNodes1,
      Entity node2,
      NativeArray<ConnectedNode> nodes2,
      EdgeNodeGeometry nodeGeometry1,
      EdgeNodeGeometry nodeGeometry2,
      NetCompositionData prefabCompositionData1,
      NetCompositionData prefabCompositionData2,
      ref Bounds2 intersection)
    {
      if (!MathUtils.Intersect(nodeGeometry1.m_Bounds.xz, nodeGeometry2.m_Bounds.xz) || node1 == node2 || originalNode1 == node2 || math.all(nodeGeometry1.m_Left.m_Length <= 0.05f) && math.all(nodeGeometry1.m_Left.m_Length <= 0.05f) || math.all(nodeGeometry2.m_Left.m_Length <= 0.05f) && math.all(nodeGeometry2.m_Left.m_Length <= 0.05f))
        return false;
      for (int index = 0; index < nodes1.Length; ++index)
      {
        if (nodes1[index].m_Node == node2)
          return false;
      }
      for (int index = 0; index < originalNodes1.Length; ++index)
      {
        if (originalNodes1[index].m_Node == node2)
          return false;
      }
      for (int index = 0; index < nodes2.Length; ++index)
      {
        Entity node = nodes2[index].m_Node;
        if (node == node1 || node == originalNode1)
          return false;
      }
      Segment segment1;
      Segment right1;
      if ((double) nodeGeometry1.m_MiddleRadius > 0.0)
      {
        segment1 = nodeGeometry1.m_Right;
        right1 = nodeGeometry1.m_Right;
        segment1.m_Right = MathUtils.Lerp(nodeGeometry1.m_Right.m_Left, nodeGeometry1.m_Right.m_Right, 0.5f);
        segment1.m_Right.d = nodeGeometry1.m_Middle.d;
        right1.m_Left = segment1.m_Right;
      }
      else
      {
        segment1 = nodeGeometry1.m_Left;
        right1 = nodeGeometry1.m_Right;
        segment1.m_Right = nodeGeometry1.m_Middle;
        right1.m_Left = nodeGeometry1.m_Middle;
      }
      Segment segment2;
      Segment right2;
      if ((double) nodeGeometry2.m_MiddleRadius > 0.0)
      {
        segment2 = nodeGeometry2.m_Right;
        right2 = nodeGeometry2.m_Right;
        segment2.m_Right = MathUtils.Lerp(nodeGeometry2.m_Right.m_Left, nodeGeometry2.m_Right.m_Right, 0.5f);
        segment2.m_Right.d = nodeGeometry2.m_Middle.d;
        right2.m_Left = segment2.m_Right;
      }
      else
      {
        segment2 = nodeGeometry2.m_Left;
        right2 = nodeGeometry2.m_Right;
        segment2.m_Right = nodeGeometry2.m_Middle;
        right2.m_Left = nodeGeometry2.m_Middle;
      }
      bool flag = ValidationHelpers.Intersect(nodeGeometry1.m_Left, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(nodeGeometry1.m_Left, segment2, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(nodeGeometry1.m_Left, right2, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(segment1, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(segment1, segment2, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(segment1, right2, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(right1, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(right1, segment2, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(right1, right2, prefabCompositionData1, prefabCompositionData2, ref intersection);
      if ((double) nodeGeometry1.m_MiddleRadius <= 0.0)
        flag |= ValidationHelpers.Intersect(nodeGeometry1.m_Right, nodeGeometry2.m_Left, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(nodeGeometry1.m_Right, segment2, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(nodeGeometry1.m_Right, right2, prefabCompositionData1, prefabCompositionData2, ref intersection);
      if ((double) nodeGeometry2.m_MiddleRadius <= 0.0)
        flag |= ValidationHelpers.Intersect(nodeGeometry1.m_Left, nodeGeometry2.m_Right, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(segment1, nodeGeometry2.m_Right, prefabCompositionData1, prefabCompositionData2, ref intersection) | ValidationHelpers.Intersect(right1, nodeGeometry2.m_Right, prefabCompositionData1, prefabCompositionData2, ref intersection);
      if ((double) nodeGeometry1.m_MiddleRadius <= 0.0 && (double) nodeGeometry2.m_MiddleRadius <= 0.0)
        flag |= ValidationHelpers.Intersect(nodeGeometry1.m_Right, nodeGeometry2.m_Right, prefabCompositionData1, prefabCompositionData2, ref intersection);
      return flag;
    }

    public static bool Intersect(
      Segment segment1,
      float2 segmentSide1,
      Triangle3 triangle2,
      NetCompositionData prefabCompositionData1,
      Bounds1 heightRange2,
      ref Bounds3 intersection)
    {
      Bounds3 bounds1 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(segment1.m_Left) | MathUtils.Bounds(segment1.m_Right), prefabCompositionData1.m_HeightRange);
      Bounds3 bounds2_1 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(triangle2), heightRange2);
      Bounds3 bounds2_2 = bounds2_1;
      if (!MathUtils.Intersect(bounds1, bounds2_2))
        return false;
      bool flag = false;
      Quad3 quad3;
      quad3.a = segment1.m_Left.a;
      quad3.b = segment1.m_Right.a;
      Bounds3 bounds3_1 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(quad3.a, quad3.b), prefabCompositionData1.m_HeightRange);
      for (int index = 1; index <= 8; ++index)
      {
        float t = (float) index / 8f;
        quad3.d = MathUtils.Position(segment1.m_Left, t);
        quad3.c = MathUtils.Position(segment1.m_Right, t);
        Bounds3 bounds3_2 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(quad3.d, quad3.c), prefabCompositionData1.m_HeightRange);
        if (MathUtils.Intersect(bounds3_1 | bounds3_2, bounds2_1))
        {
          Quad3 quad1 = quad3;
          float3 float3_1 = math.normalizesafe(quad1.b - quad1.a) * 0.5f;
          float3 float3_2 = math.normalizesafe(quad1.d - quad1.c) * 0.5f;
          quad1.a += float3_1;
          quad1.b -= float3_1;
          quad1.c += float3_2;
          quad1.d -= float3_2;
          Bounds3 intersection1;
          Bounds3 intersection2;
          if (ValidationHelpers.QuadTriangleIntersect(quad1, triangle2, out intersection1, out intersection2))
          {
            intersection1 = ValidationHelpers.SetHeightRange(intersection1, prefabCompositionData1.m_HeightRange);
            intersection2 = ValidationHelpers.SetHeightRange(intersection2, heightRange2);
            Bounds3 intersection3;
            if (MathUtils.Intersect(intersection1, intersection2, out intersection3))
            {
              flag = true;
              intersection |= intersection3;
            }
          }
        }
        quad3.a = quad3.d;
        quad3.b = quad3.c;
        bounds3_1 = bounds3_2;
      }
      return flag;
    }

    public static bool Intersect(
      Segment segment1,
      Segment segment2,
      NetCompositionData prefabCompositionData1,
      NetCompositionData prefabCompositionData2,
      ref Bounds3 intersection)
    {
      Bounds3 bounds1_1 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(segment1.m_Left) | MathUtils.Bounds(segment1.m_Right), prefabCompositionData1.m_HeightRange);
      Bounds3 bounds2_1 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(segment2.m_Left) | MathUtils.Bounds(segment2.m_Right), prefabCompositionData2.m_HeightRange);
      Bounds3 bounds2_2 = bounds2_1;
      if (!MathUtils.Intersect(bounds1_1, bounds2_2))
        return false;
      bool flag = false;
      Quad3 quad1;
      quad1.a = segment1.m_Left.a;
      quad1.b = segment1.m_Right.a;
      Bounds3 bounds3_1 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(quad1.a, quad1.b), prefabCompositionData1.m_HeightRange);
      for (int index1 = 1; index1 <= 8; ++index1)
      {
        float t1 = (float) index1 / 8f;
        quad1.d = MathUtils.Position(segment1.m_Left, t1);
        quad1.c = MathUtils.Position(segment1.m_Right, t1);
        Bounds3 bounds3_2 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(quad1.d, quad1.c), prefabCompositionData1.m_HeightRange);
        Bounds3 bounds1_2 = bounds3_1 | bounds3_2;
        if (MathUtils.Intersect(bounds1_2, bounds2_1))
        {
          Quad3 quad2;
          quad2.a = segment2.m_Left.a;
          quad2.b = segment2.m_Right.a;
          Bounds3 bounds3_3 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(quad2.a, quad2.b), prefabCompositionData2.m_HeightRange);
          for (int index2 = 1; index2 <= 8; ++index2)
          {
            float t2 = (float) index2 / 8f;
            quad2.d = MathUtils.Position(segment2.m_Left, t2);
            quad2.c = MathUtils.Position(segment2.m_Right, t2);
            Bounds3 bounds3_4 = ValidationHelpers.SetHeightRange(MathUtils.Bounds(quad2.d, quad2.c), prefabCompositionData2.m_HeightRange);
            Bounds3 bounds2_3 = bounds3_3 | bounds3_4;
            Bounds3 intersection1;
            Bounds3 intersection2;
            if (MathUtils.Intersect(bounds1_2, bounds2_3) && ValidationHelpers.QuadIntersect(quad1, quad2, out intersection1, out intersection2))
            {
              intersection1 = ValidationHelpers.SetHeightRange(intersection1, prefabCompositionData1.m_HeightRange);
              intersection2 = ValidationHelpers.SetHeightRange(intersection2, prefabCompositionData2.m_HeightRange);
              Bounds3 intersection3;
              if (MathUtils.Intersect(intersection1, intersection2, out intersection3))
              {
                flag = true;
                intersection |= intersection3;
              }
            }
            quad2.a = quad2.d;
            quad2.b = quad2.c;
            bounds3_3 = bounds3_4;
          }
        }
        quad1.a = quad1.d;
        quad1.b = quad1.c;
        bounds3_1 = bounds3_2;
      }
      return flag;
    }

    public static bool Intersect(
      Segment segment1,
      Segment segment2,
      NetCompositionData prefabCompositionData1,
      NetCompositionData prefabCompositionData2,
      ref Bounds2 intersection)
    {
      Bounds3 bounds3 = MathUtils.Bounds(segment1.m_Left) | MathUtils.Bounds(segment1.m_Right);
      Bounds2 xz1 = bounds3.xz;
      bounds3 = MathUtils.Bounds(segment2.m_Left) | MathUtils.Bounds(segment2.m_Right);
      Bounds2 xz2 = bounds3.xz;
      Bounds2 bounds2_1 = xz2;
      if (!MathUtils.Intersect(xz1, bounds2_1))
        return false;
      bool flag = false;
      Quad2 quad1;
      quad1.a = segment1.m_Left.a.xz;
      quad1.b = segment1.m_Right.a.xz;
      Bounds2 bounds2_2 = MathUtils.Bounds(quad1.a, quad1.b);
      for (int index1 = 1; index1 <= 8; ++index1)
      {
        float t1 = (float) index1 / 8f;
        quad1.d = MathUtils.Position(segment1.m_Left, t1).xz;
        quad1.c = MathUtils.Position(segment1.m_Right, t1).xz;
        Bounds2 bounds2_3 = MathUtils.Bounds(quad1.d, quad1.c);
        Bounds2 bounds1 = bounds2_2 | bounds2_3;
        if (MathUtils.Intersect(bounds1, xz2))
        {
          Quad2 quad2;
          quad2.a = segment2.m_Left.a.xz;
          quad2.b = segment2.m_Right.a.xz;
          Bounds2 bounds2_4 = MathUtils.Bounds(quad2.a, quad2.b);
          for (int index2 = 1; index2 <= 8; ++index2)
          {
            float t2 = (float) index2 / 8f;
            quad2.d = MathUtils.Position(segment2.m_Left, t2).xz;
            quad2.c = MathUtils.Position(segment2.m_Right, t2).xz;
            Bounds2 bounds2_5 = MathUtils.Bounds(quad2.d, quad2.c);
            Bounds2 bounds2_6 = bounds2_4 | bounds2_5;
            Bounds2 intersection1;
            if (MathUtils.Intersect(bounds1, bounds2_6) && MathUtils.Intersect(quad1, quad2, out intersection1))
            {
              flag = true;
              intersection |= intersection1;
            }
            quad2.a = quad2.d;
            quad2.b = quad2.c;
            bounds2_4 = bounds2_5;
          }
        }
        quad1.a = quad1.d;
        quad1.b = quad1.c;
        bounds2_2 = bounds2_3;
      }
      return flag;
    }

    public static Bounds3 SetHeightRange(Bounds3 bounds, Bounds1 heightRange)
    {
      bounds.min.y += heightRange.min;
      bounds.max.y += heightRange.max;
      return bounds;
    }

    public static bool QuadIntersect(
      Quad3 quad1,
      Quad3 quad2,
      out Bounds3 intersection1,
      out Bounds3 intersection2)
    {
      intersection1.min = (float3) float.MaxValue;
      intersection1.max = (float3) float.MinValue;
      intersection2.min = (float3) float.MaxValue;
      intersection2.max = (float3) float.MinValue;
      Triangle3 triangle1_1 = new Triangle3(quad1.a, quad1.d, quad1.c);
      Triangle3 triangle1_2 = new Triangle3(quad1.c, quad1.b, quad1.a);
      Triangle3 triangle1_3 = new Triangle3(quad2.a, quad2.d, quad2.c);
      Triangle3 triangle1_4 = new Triangle3(quad2.c, quad2.b, quad2.a);
      Line3.Segment line1_1 = new Line3.Segment(quad1.a, quad1.b);
      Line3.Segment line1_2 = new Line3.Segment(quad1.b, quad1.c);
      Line3.Segment line1_3 = new Line3.Segment(quad1.c, quad1.d);
      Line3.Segment line1_4 = new Line3.Segment(quad1.d, quad1.a);
      Quad3 quad2_1 = quad2;
      ref Bounds3 local1 = ref intersection1;
      ref Bounds3 local2 = ref intersection2;
      return ValidationHelpers.QuadIntersectHelper(triangle1_1, quad2_1, ref local1, ref local2) | ValidationHelpers.QuadIntersectHelper(triangle1_2, quad2, ref intersection1, ref intersection2) | ValidationHelpers.QuadIntersectHelper(triangle1_3, quad1, ref intersection2, ref intersection1) | ValidationHelpers.QuadIntersectHelper(triangle1_4, quad1, ref intersection2, ref intersection1) | ValidationHelpers.QuadIntersectHelper(line1_1, quad2, ref intersection1, ref intersection2) | ValidationHelpers.QuadIntersectHelper(line1_2, quad2, ref intersection1, ref intersection2) | ValidationHelpers.QuadIntersectHelper(line1_3, quad2, ref intersection1, ref intersection2) | ValidationHelpers.QuadIntersectHelper(line1_4, quad2, ref intersection1, ref intersection2);
    }

    public static bool QuadTriangleIntersect(
      Quad3 quad1,
      Triangle3 triangle2,
      out Bounds3 intersection1,
      out Bounds3 intersection2)
    {
      intersection1.min = (float3) float.MaxValue;
      intersection1.max = (float3) float.MinValue;
      intersection2.min = (float3) float.MaxValue;
      intersection2.max = (float3) float.MinValue;
      Triangle3 triangle1_1 = new Triangle3(quad1.a, quad1.d, quad1.c);
      Triangle3 triangle1_2 = new Triangle3(quad1.c, quad1.b, quad1.a);
      Line3.Segment line1_1 = new Line3.Segment(quad1.a, quad1.b);
      Line3.Segment line1_2 = new Line3.Segment(quad1.b, quad1.c);
      Line3.Segment line1_3 = new Line3.Segment(quad1.c, quad1.d);
      Line3.Segment line1_4 = new Line3.Segment(quad1.d, quad1.a);
      Triangle3 triangle2_1 = triangle2;
      ref Bounds3 local1 = ref intersection1;
      ref Bounds3 local2 = ref intersection2;
      return ValidationHelpers.QuadTriangleIntersectHelper(triangle1_1, triangle2_1, ref local1, ref local2) | ValidationHelpers.QuadTriangleIntersectHelper(triangle1_2, triangle2, ref intersection1, ref intersection2) | ValidationHelpers.QuadIntersectHelper(triangle2, quad1, ref intersection2, ref intersection1) | ValidationHelpers.QuadTriangleIntersectHelper(line1_1, triangle2, ref intersection1, ref intersection2) | ValidationHelpers.QuadTriangleIntersectHelper(line1_2, triangle2, ref intersection1, ref intersection2) | ValidationHelpers.QuadTriangleIntersectHelper(line1_3, triangle2, ref intersection1, ref intersection2) | ValidationHelpers.QuadTriangleIntersectHelper(line1_4, triangle2, ref intersection1, ref intersection2);
    }

    private static bool QuadIntersectHelper(
      Triangle3 triangle1,
      Quad3 quad2,
      ref Bounds3 intersection1,
      ref Bounds3 intersection2)
    {
      Triangle2 xz = triangle1.xz;
      bool flag = false;
      float2 t;
      if (MathUtils.Intersect(xz, quad2.a.xz, out t))
      {
        intersection1 |= MathUtils.Position(triangle1, t);
        intersection2 |= quad2.a;
        flag = true;
      }
      if (MathUtils.Intersect(xz, quad2.b.xz, out t))
      {
        intersection1 |= MathUtils.Position(triangle1, t);
        intersection2 |= quad2.b;
        flag = true;
      }
      if (MathUtils.Intersect(xz, quad2.c.xz, out t))
      {
        intersection1 |= MathUtils.Position(triangle1, t);
        intersection2 |= quad2.c;
        flag = true;
      }
      if (MathUtils.Intersect(xz, quad2.d.xz, out t))
      {
        intersection1 |= MathUtils.Position(triangle1, t);
        intersection2 |= quad2.d;
        flag = true;
      }
      return flag;
    }

    private static bool QuadTriangleIntersectHelper(
      Triangle3 triangle1,
      Triangle3 triangle2,
      ref Bounds3 intersection1,
      ref Bounds3 intersection2)
    {
      Triangle2 xz = triangle1.xz;
      bool flag = false;
      float2 t;
      if (MathUtils.Intersect(xz, triangle2.a.xz, out t))
      {
        intersection1 |= MathUtils.Position(triangle1, t);
        intersection2 |= triangle2.a;
        flag = true;
      }
      if (MathUtils.Intersect(xz, triangle2.b.xz, out t))
      {
        intersection1 |= MathUtils.Position(triangle1, t);
        intersection2 |= triangle2.b;
        flag = true;
      }
      if (MathUtils.Intersect(xz, triangle2.c.xz, out t))
      {
        intersection1 |= MathUtils.Position(triangle1, t);
        intersection2 |= triangle2.c;
        flag = true;
      }
      return flag;
    }

    private static bool QuadIntersectHelper(
      Line3.Segment line1,
      Quad3 quad2,
      ref Bounds3 intersection1,
      ref Bounds3 intersection2)
    {
      Line2.Segment xz = line1.xz;
      bool flag = false;
      float2 t;
      if (MathUtils.Intersect(xz, new Line2.Segment(quad2.a.xz, quad2.b.xz), out t))
      {
        intersection1 |= MathUtils.Position(line1, t.x);
        intersection2 |= math.lerp(quad2.a, quad2.b, t.y);
        flag = true;
      }
      if (MathUtils.Intersect(xz, new Line2.Segment(quad2.b.xz, quad2.c.xz), out t))
      {
        intersection1 |= MathUtils.Position(line1, t.x);
        intersection2 |= math.lerp(quad2.b, quad2.c, t.y);
        flag = true;
      }
      if (MathUtils.Intersect(xz, new Line2.Segment(quad2.c.xz, quad2.d.xz), out t))
      {
        intersection1 |= MathUtils.Position(line1, t.x);
        intersection2 |= math.lerp(quad2.c, quad2.d, t.y);
        flag = true;
      }
      if (MathUtils.Intersect(xz, new Line2.Segment(quad2.d.xz, quad2.a.xz), out t))
      {
        intersection1 |= MathUtils.Position(line1, t.x);
        intersection2 |= math.lerp(quad2.d, quad2.a, t.y);
        flag = true;
      }
      return flag;
    }

    private static bool QuadTriangleIntersectHelper(
      Line3.Segment line1,
      Triangle3 triangle2,
      ref Bounds3 intersection1,
      ref Bounds3 intersection2)
    {
      Line2.Segment xz = line1.xz;
      bool flag = false;
      float2 t;
      if (MathUtils.Intersect(xz, new Line2.Segment(triangle2.a.xz, triangle2.b.xz), out t))
      {
        intersection1 |= MathUtils.Position(line1, t.x);
        intersection2 |= math.lerp(triangle2.a, triangle2.b, t.y);
        flag = true;
      }
      if (MathUtils.Intersect(xz, new Line2.Segment(triangle2.b.xz, triangle2.c.xz), out t))
      {
        intersection1 |= MathUtils.Position(line1, t.x);
        intersection2 |= math.lerp(triangle2.b, triangle2.c, t.y);
        flag = true;
      }
      if (MathUtils.Intersect(xz, new Line2.Segment(triangle2.c.xz, triangle2.a.xz), out t))
      {
        intersection1 |= MathUtils.Position(line1, t.x);
        intersection2 |= math.lerp(triangle2.c, triangle2.a, t.y);
        flag = true;
      }
      return flag;
    }

    private struct NetIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public Entity m_TopLevelEntity;
      public Entity m_EdgeEntity;
      public Bounds3 m_Bounds;
      public bool m_Essential;
      public bool m_EditorMode;
      public Edge m_Edge;
      public Edge m_OriginalNodes;
      public NativeArray<ConnectedNode> m_ConnectedNodes;
      public NativeArray<ConnectedNode> m_OriginalConnectedNodes;
      public EdgeGeometry m_EdgeGeometryData;
      public StartNodeGeometry m_StartNodeGeometryData;
      public EndNodeGeometry m_EndNodeGeometryData;
      public NetCompositionData m_EdgeCompositionData;
      public NetCompositionData m_StartCompositionData;
      public NetCompositionData m_EndCompositionData;
      public CollisionMask m_EdgeCollisionMask;
      public CollisionMask m_StartCollisionMask;
      public CollisionMask m_EndCollisionMask;
      public CollisionMask m_CombinedCollisionMask;
      public ValidationSystem.EntityData m_Data;
      public NativeQueue<ErrorData>.ParallelWriter m_ErrorQueue;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        return (this.m_CombinedCollisionMask & CollisionMask.OnGround) != (CollisionMask) 0 ? MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds.xz) : MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity edgeEntity2)
      {
        if ((this.m_CombinedCollisionMask & CollisionMask.OnGround) != (CollisionMask) 0)
        {
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds.xz))
            return;
        }
        else if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Data.m_Hidden.HasComponent(edgeEntity2) || !this.m_Data.m_Composition.HasComponent(edgeEntity2))
          return;
        Entity entity = edgeEntity2;
        bool hasOwner = false;
        Owner componentData;
        Entity owner;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (; this.m_Data.m_Owner.TryGetComponent(entity, out componentData) && !this.m_Data.m_Building.HasComponent(entity); entity = owner)
        {
          owner = componentData.m_Owner;
          hasOwner = true;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Data.m_AssetStamp.HasComponent(owner))
            break;
        }
        // ISSUE: reference to a compiler-generated field
        Composition compositionData2 = this.m_Data.m_Composition[edgeEntity2];
        // ISSUE: reference to a compiler-generated field
        Edge edgeData2 = this.m_Data.m_Edge[edgeEntity2];
        // ISSUE: reference to a compiler-generated field
        EdgeGeometry edgeGeometryData2 = this.m_Data.m_EdgeGeometry[edgeEntity2];
        // ISSUE: reference to a compiler-generated field
        StartNodeGeometry startNodeGeometryData2 = this.m_Data.m_StartNodeGeometry[edgeEntity2];
        // ISSUE: reference to a compiler-generated field
        EndNodeGeometry endNodeGeometryData2 = this.m_Data.m_EndNodeGeometry[edgeEntity2];
        this.CheckOverlap(entity, edgeEntity2, bounds.m_Bounds, edgeData2, compositionData2, edgeGeometryData2, startNodeGeometryData2, endNodeGeometryData2, false, hasOwner);
      }

      public void CheckOverlap(
        Entity topLevelEntity2,
        Entity edgeEntity2,
        Bounds3 bounds2,
        Edge edgeData2,
        Composition compositionData2,
        EdgeGeometry edgeGeometryData2,
        StartNodeGeometry startNodeGeometryData2,
        EndNodeGeometry endNodeGeometryData2,
        bool essential,
        bool hasOwner)
      {
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData1 = this.m_Data.m_PrefabComposition[compositionData2.m_Edge];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData2 = this.m_Data.m_PrefabComposition[compositionData2.m_StartNode];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData3 = this.m_Data.m_PrefabComposition[compositionData2.m_EndNode];
        CollisionMask collisionMask1 = NetUtils.GetCollisionMask(netCompositionData1, !this.m_EditorMode | hasOwner);
        CollisionMask collisionMask2 = NetUtils.GetCollisionMask(netCompositionData2, !this.m_EditorMode | hasOwner);
        CollisionMask collisionMask3 = NetUtils.GetCollisionMask(netCompositionData3, !this.m_EditorMode | hasOwner);
        CollisionMask mask2 = collisionMask1 | collisionMask2 | collisionMask3;
        if ((this.m_CombinedCollisionMask & mask2) == (CollisionMask) 0 || (this.m_CombinedCollisionMask & CollisionMask.OnGround) != (CollisionMask) 0 && !CommonUtils.ExclusiveGroundCollision(this.m_CombinedCollisionMask, mask2) && !MathUtils.Intersect(bounds2, this.m_Bounds))
          return;
        ErrorData errorData = new ErrorData();
        Bounds3 intersection1;
        intersection1.min = (float3) float.MaxValue;
        intersection1.max = (float3) float.MinValue;
        Bounds2 intersection2;
        intersection2.min = (float2) float.MaxValue;
        intersection2.max = (float2) float.MinValue;
        Bounds1 bounds1;
        bounds1.min = float.MaxValue;
        bounds1.max = float.MinValue;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedNode> connectedNode = this.m_Data.m_ConnectedNodes[edgeEntity2];
        if ((this.m_EdgeCollisionMask & collisionMask1) != (CollisionMask) 0)
        {
          if (CommonUtils.ExclusiveGroundCollision(this.m_EdgeCollisionMask, collisionMask1))
          {
            if (ValidationHelpers.Intersect(this.m_OriginalNodes, edgeData2, this.m_EdgeGeometryData, edgeGeometryData2, this.m_EdgeCompositionData, netCompositionData1, ref intersection2))
            {
              errorData.m_ErrorType = ErrorType.OverlapExisting;
              bounds1 |= MathUtils.Center(this.m_EdgeGeometryData.m_Bounds.y & edgeGeometryData2.m_Bounds.y);
            }
          }
          else if (ValidationHelpers.Intersect(this.m_OriginalNodes, edgeData2, this.m_EdgeGeometryData, edgeGeometryData2, this.m_EdgeCompositionData, netCompositionData1, ref intersection1))
            errorData.m_ErrorType = ErrorType.OverlapExisting;
        }
        if ((this.m_EdgeCollisionMask & collisionMask2) != (CollisionMask) 0)
        {
          if (CommonUtils.ExclusiveGroundCollision(this.m_EdgeCollisionMask, collisionMask2))
          {
            if (ValidationHelpers.Intersect(this.m_Edge, this.m_OriginalNodes, this.m_ConnectedNodes, this.m_OriginalConnectedNodes, edgeData2.m_Start, this.m_EdgeGeometryData, startNodeGeometryData2.m_Geometry, this.m_EdgeCompositionData, netCompositionData2, ref intersection2))
            {
              if (!this.IgnoreCollision(edgeEntity2, edgeData2.m_Start, this.m_Edge))
                errorData.m_ErrorType = ErrorType.OverlapExisting;
              bounds1 |= MathUtils.Center(this.m_EdgeGeometryData.m_Bounds.y & startNodeGeometryData2.m_Geometry.m_Bounds.y);
            }
          }
          else if (ValidationHelpers.Intersect(this.m_Edge, this.m_OriginalNodes, this.m_ConnectedNodes, this.m_OriginalConnectedNodes, edgeData2.m_Start, this.m_EdgeGeometryData, startNodeGeometryData2.m_Geometry, this.m_EdgeCompositionData, netCompositionData2, ref intersection1) && !this.IgnoreCollision(edgeEntity2, edgeData2.m_Start, this.m_Edge))
            errorData.m_ErrorType = ErrorType.OverlapExisting;
        }
        if ((this.m_EdgeCollisionMask & collisionMask3) != (CollisionMask) 0)
        {
          if (CommonUtils.ExclusiveGroundCollision(this.m_EdgeCollisionMask, collisionMask3))
          {
            if (ValidationHelpers.Intersect(this.m_Edge, this.m_OriginalNodes, this.m_ConnectedNodes, this.m_OriginalConnectedNodes, edgeData2.m_End, this.m_EdgeGeometryData, endNodeGeometryData2.m_Geometry, this.m_EdgeCompositionData, netCompositionData3, ref intersection2))
            {
              if (!this.IgnoreCollision(edgeEntity2, edgeData2.m_End, this.m_Edge))
                errorData.m_ErrorType = ErrorType.OverlapExisting;
              bounds1 |= MathUtils.Center(this.m_EdgeGeometryData.m_Bounds.y & endNodeGeometryData2.m_Geometry.m_Bounds.y);
            }
          }
          else if (ValidationHelpers.Intersect(this.m_Edge, this.m_OriginalNodes, this.m_ConnectedNodes, this.m_OriginalConnectedNodes, edgeData2.m_End, this.m_EdgeGeometryData, endNodeGeometryData2.m_Geometry, this.m_EdgeCompositionData, netCompositionData3, ref intersection1) && !this.IgnoreCollision(edgeEntity2, edgeData2.m_End, this.m_Edge))
            errorData.m_ErrorType = ErrorType.OverlapExisting;
        }
        if ((collisionMask1 & this.m_StartCollisionMask) != (CollisionMask) 0)
        {
          if (CommonUtils.ExclusiveGroundCollision(collisionMask1, this.m_StartCollisionMask))
          {
            if (ValidationHelpers.Intersect(edgeData2, connectedNode.AsNativeArray(), this.m_Edge.m_Start, this.m_OriginalNodes.m_Start, edgeGeometryData2, this.m_StartNodeGeometryData.m_Geometry, netCompositionData1, this.m_StartCompositionData, ref intersection2))
            {
              if (!this.IgnoreCollision(this.m_EdgeEntity, this.m_Edge.m_Start, edgeData2))
                errorData.m_ErrorType = ErrorType.OverlapExisting;
              bounds1 |= MathUtils.Center(edgeGeometryData2.m_Bounds.y & this.m_StartNodeGeometryData.m_Geometry.m_Bounds.y);
            }
          }
          else if (ValidationHelpers.Intersect(edgeData2, connectedNode.AsNativeArray(), this.m_Edge.m_Start, this.m_OriginalNodes.m_Start, edgeGeometryData2, this.m_StartNodeGeometryData.m_Geometry, netCompositionData1, this.m_StartCompositionData, ref intersection1) && !this.IgnoreCollision(this.m_EdgeEntity, this.m_Edge.m_Start, edgeData2))
            errorData.m_ErrorType = ErrorType.OverlapExisting;
        }
        if ((collisionMask1 & this.m_EndCollisionMask) != (CollisionMask) 0)
        {
          if (CommonUtils.ExclusiveGroundCollision(collisionMask1, this.m_EndCollisionMask))
          {
            if (ValidationHelpers.Intersect(edgeData2, connectedNode.AsNativeArray(), this.m_Edge.m_End, this.m_OriginalNodes.m_End, edgeGeometryData2, this.m_EndNodeGeometryData.m_Geometry, netCompositionData1, this.m_EndCompositionData, ref intersection2))
            {
              if (!this.IgnoreCollision(this.m_EdgeEntity, this.m_Edge.m_End, edgeData2))
                errorData.m_ErrorType = ErrorType.OverlapExisting;
              bounds1 |= MathUtils.Center(edgeGeometryData2.m_Bounds.y & this.m_EndNodeGeometryData.m_Geometry.m_Bounds.y);
            }
          }
          else if (ValidationHelpers.Intersect(edgeData2, connectedNode.AsNativeArray(), this.m_Edge.m_End, this.m_OriginalNodes.m_End, edgeGeometryData2, this.m_EndNodeGeometryData.m_Geometry, netCompositionData1, this.m_EndCompositionData, ref intersection1) && !this.IgnoreCollision(this.m_EdgeEntity, this.m_Edge.m_End, edgeData2))
            errorData.m_ErrorType = ErrorType.OverlapExisting;
        }
        if ((this.m_StartCollisionMask & collisionMask2) != (CollisionMask) 0)
        {
          if (CommonUtils.ExclusiveGroundCollision(this.m_StartCollisionMask, collisionMask2))
          {
            if (ValidationHelpers.Intersect(this.m_Edge.m_Start, this.m_OriginalNodes.m_Start, this.m_ConnectedNodes, this.m_OriginalConnectedNodes, edgeData2.m_Start, connectedNode.AsNativeArray(), this.m_StartNodeGeometryData.m_Geometry, startNodeGeometryData2.m_Geometry, this.m_StartCompositionData, netCompositionData2, ref intersection2))
            {
              if (!this.IgnoreCollision(this.m_EdgeEntity, this.m_Edge.m_Start, edgeEntity2, edgeData2.m_Start))
                errorData.m_ErrorType = ErrorType.OverlapExisting;
              bounds1 |= MathUtils.Center(this.m_StartNodeGeometryData.m_Geometry.m_Bounds.y & startNodeGeometryData2.m_Geometry.m_Bounds.y);
            }
          }
          else if (ValidationHelpers.Intersect(this.m_Edge.m_Start, this.m_OriginalNodes.m_Start, this.m_ConnectedNodes, this.m_OriginalConnectedNodes, edgeData2.m_Start, connectedNode.AsNativeArray(), this.m_StartNodeGeometryData.m_Geometry, startNodeGeometryData2.m_Geometry, this.m_StartCompositionData, netCompositionData2, ref intersection1) && !this.IgnoreCollision(this.m_EdgeEntity, this.m_Edge.m_Start, edgeEntity2, edgeData2.m_Start))
            errorData.m_ErrorType = ErrorType.OverlapExisting;
        }
        if ((this.m_StartCollisionMask & collisionMask3) != (CollisionMask) 0)
        {
          if (CommonUtils.ExclusiveGroundCollision(this.m_StartCollisionMask, collisionMask3))
          {
            if (ValidationHelpers.Intersect(this.m_Edge.m_Start, this.m_OriginalNodes.m_Start, this.m_ConnectedNodes, this.m_OriginalConnectedNodes, edgeData2.m_End, connectedNode.AsNativeArray(), this.m_StartNodeGeometryData.m_Geometry, endNodeGeometryData2.m_Geometry, this.m_StartCompositionData, netCompositionData3, ref intersection2))
            {
              if (!this.IgnoreCollision(this.m_EdgeEntity, this.m_Edge.m_Start, edgeEntity2, edgeData2.m_End))
                errorData.m_ErrorType = ErrorType.OverlapExisting;
              bounds1 |= MathUtils.Center(this.m_StartNodeGeometryData.m_Geometry.m_Bounds.y & endNodeGeometryData2.m_Geometry.m_Bounds.y);
            }
          }
          else if (ValidationHelpers.Intersect(this.m_Edge.m_Start, this.m_OriginalNodes.m_Start, this.m_ConnectedNodes, this.m_OriginalConnectedNodes, edgeData2.m_End, connectedNode.AsNativeArray(), this.m_StartNodeGeometryData.m_Geometry, endNodeGeometryData2.m_Geometry, this.m_StartCompositionData, netCompositionData3, ref intersection1) && !this.IgnoreCollision(this.m_EdgeEntity, this.m_Edge.m_Start, edgeEntity2, edgeData2.m_End))
            errorData.m_ErrorType = ErrorType.OverlapExisting;
        }
        if ((this.m_EndCollisionMask & collisionMask2) != (CollisionMask) 0)
        {
          if (CommonUtils.ExclusiveGroundCollision(this.m_EndCollisionMask, collisionMask2))
          {
            if (ValidationHelpers.Intersect(this.m_Edge.m_End, this.m_OriginalNodes.m_End, this.m_ConnectedNodes, this.m_OriginalConnectedNodes, edgeData2.m_Start, connectedNode.AsNativeArray(), this.m_EndNodeGeometryData.m_Geometry, startNodeGeometryData2.m_Geometry, this.m_EndCompositionData, netCompositionData2, ref intersection2))
            {
              if (!this.IgnoreCollision(this.m_EdgeEntity, this.m_Edge.m_End, edgeEntity2, edgeData2.m_Start))
                errorData.m_ErrorType = ErrorType.OverlapExisting;
              bounds1 |= MathUtils.Center(this.m_EndNodeGeometryData.m_Geometry.m_Bounds.y & startNodeGeometryData2.m_Geometry.m_Bounds.y);
            }
          }
          else if (ValidationHelpers.Intersect(this.m_Edge.m_End, this.m_OriginalNodes.m_End, this.m_ConnectedNodes, this.m_OriginalConnectedNodes, edgeData2.m_Start, connectedNode.AsNativeArray(), this.m_EndNodeGeometryData.m_Geometry, startNodeGeometryData2.m_Geometry, this.m_EndCompositionData, netCompositionData2, ref intersection1) && !this.IgnoreCollision(this.m_EdgeEntity, this.m_Edge.m_End, edgeEntity2, edgeData2.m_Start))
            errorData.m_ErrorType = ErrorType.OverlapExisting;
        }
        if ((this.m_EndCollisionMask & collisionMask3) != (CollisionMask) 0)
        {
          if (CommonUtils.ExclusiveGroundCollision(this.m_EndCollisionMask, collisionMask3))
          {
            if (ValidationHelpers.Intersect(this.m_Edge.m_End, this.m_OriginalNodes.m_End, this.m_ConnectedNodes, this.m_OriginalConnectedNodes, edgeData2.m_End, connectedNode.AsNativeArray(), this.m_EndNodeGeometryData.m_Geometry, endNodeGeometryData2.m_Geometry, this.m_EndCompositionData, netCompositionData3, ref intersection2))
            {
              if (!this.IgnoreCollision(this.m_EdgeEntity, this.m_Edge.m_End, edgeEntity2, edgeData2.m_End))
                errorData.m_ErrorType = ErrorType.OverlapExisting;
              bounds1 |= MathUtils.Center(this.m_EndNodeGeometryData.m_Geometry.m_Bounds.y & endNodeGeometryData2.m_Geometry.m_Bounds.y);
            }
          }
          else if (ValidationHelpers.Intersect(this.m_Edge.m_End, this.m_OriginalNodes.m_End, this.m_ConnectedNodes, this.m_OriginalConnectedNodes, edgeData2.m_End, connectedNode.AsNativeArray(), this.m_EndNodeGeometryData.m_Geometry, endNodeGeometryData2.m_Geometry, this.m_EndCompositionData, netCompositionData3, ref intersection1) && !this.IgnoreCollision(this.m_EdgeEntity, this.m_Edge.m_End, edgeEntity2, edgeData2.m_End))
            errorData.m_ErrorType = ErrorType.OverlapExisting;
        }
        if (errorData.m_ErrorType == ErrorType.None)
          return;
        intersection1.xz |= intersection2;
        intersection1.y |= bounds1;
        errorData.m_ErrorSeverity = ErrorSeverity.Error;
        errorData.m_TempEntity = this.m_EdgeEntity;
        errorData.m_PermanentEntity = edgeEntity2;
        errorData.m_Position = MathUtils.Center(intersection1);
        if (!essential && topLevelEntity2 != edgeEntity2 && topLevelEntity2 != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_Data.m_PrefabRef[topLevelEntity2];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Data.m_PrefabObjectGeometry.HasComponent(prefabRef.m_Prefab) && (this.m_Data.m_PrefabObjectGeometry[prefabRef.m_Prefab].m_Flags & (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.DeleteOverridden)) == (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.DeleteOverridden) && !this.m_Data.m_Attached.HasComponent(topLevelEntity2) && (!this.m_Data.m_Temp.HasComponent(topLevelEntity2) || (this.m_Data.m_Temp[topLevelEntity2].m_Flags & TempFlags.Essential) == (TempFlags) 0))
          {
            errorData.m_ErrorSeverity = ErrorSeverity.Warning;
            errorData.m_PermanentEntity = topLevelEntity2;
          }
        }
        if (!this.m_Essential && this.m_TopLevelEntity != this.m_EdgeEntity && this.m_TopLevelEntity != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_Data.m_PrefabRef[this.m_TopLevelEntity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Data.m_PrefabObjectGeometry.HasComponent(prefabRef.m_Prefab) && (this.m_Data.m_PrefabObjectGeometry[prefabRef.m_Prefab].m_Flags & (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.DeleteOverridden)) == (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.DeleteOverridden) && !this.m_Data.m_Attached.HasComponent(this.m_TopLevelEntity) && (!this.m_Data.m_Temp.HasComponent(this.m_TopLevelEntity) || (this.m_Data.m_Temp[this.m_TopLevelEntity].m_Flags & TempFlags.Essential) == (TempFlags) 0))
          {
            errorData.m_ErrorSeverity = ErrorSeverity.Warning;
            errorData.m_TempEntity = edgeEntity2;
            errorData.m_PermanentEntity = this.m_TopLevelEntity;
          }
        }
        this.m_ErrorQueue.Enqueue(errorData);
      }

      private bool IgnoreCollision(Entity edge1, Entity node1, Edge edge2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeIterator edgeIterator = new EdgeIterator(edge1, node1, this.m_Data.m_ConnectedEdges, this.m_Data.m_Edge, this.m_Data.m_Temp, this.m_Data.m_Hidden, true);
        EdgeIteratorValue edgeIteratorValue;
        while (edgeIterator.GetNext(out edgeIteratorValue))
        {
          if (edgeIteratorValue.m_Middle)
          {
            // ISSUE: reference to a compiler-generated field
            Edge edge = this.m_Data.m_Edge[edgeIteratorValue.m_Edge];
            if (edge.m_Start == edge2.m_Start || edge.m_End == edge2.m_Start || edge.m_Start == edge2.m_End || edge.m_End == edge2.m_End)
              return true;
            Temp componentData1;
            Temp componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Data.m_Temp.TryGetComponent(edge.m_Start, out componentData1) && this.m_Data.m_Temp.TryGetComponent(edge.m_End, out componentData2))
            {
              if (componentData1.m_Original == edge2.m_Start || componentData2.m_Original == edge2.m_Start || componentData1.m_Original == edge2.m_End || componentData2.m_Original == edge2.m_End)
                return true;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_Data.m_Temp.TryGetComponent(edge2.m_Start, out componentData1) && this.m_Data.m_Temp.TryGetComponent(edge2.m_End, out componentData2) && (componentData1.m_Original == edge.m_Start || componentData2.m_Original == edge.m_Start || componentData1.m_Original == edge.m_End || componentData2.m_Original == edge.m_End))
                return true;
            }
          }
        }
        return false;
      }

      private bool IgnoreCollision(Entity edge1, Entity node1, Entity edge2, Entity node2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeIterator edgeIterator = new EdgeIterator(edge1, node1, this.m_Data.m_ConnectedEdges, this.m_Data.m_Edge, this.m_Data.m_Temp, this.m_Data.m_Hidden, true);
        EdgeIteratorValue edgeIteratorValue1;
        while (edgeIterator.GetNext(out edgeIteratorValue1))
        {
          if (edgeIteratorValue1.m_Middle)
          {
            // ISSUE: reference to a compiler-generated field
            Edge edge = this.m_Data.m_Edge[edgeIteratorValue1.m_Edge];
            if (edge.m_Start == node2 || edge.m_End == node2)
              return true;
            Temp componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Data.m_Temp.TryGetComponent(node2, out componentData))
            {
              if (edge.m_Start == componentData.m_Original || edge.m_End == componentData.m_Original)
                return true;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_Data.m_Temp.TryGetComponent(edge.m_Start, out componentData) && componentData.m_Original == node2 || this.m_Data.m_Temp.TryGetComponent(edge.m_End, out componentData) && componentData.m_Original == node2)
                return true;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        edgeIterator = new EdgeIterator(edge2, node2, this.m_Data.m_ConnectedEdges, this.m_Data.m_Edge, this.m_Data.m_Temp, this.m_Data.m_Hidden, true);
        EdgeIteratorValue edgeIteratorValue2;
        while (edgeIterator.GetNext(out edgeIteratorValue2))
        {
          if (edgeIteratorValue2.m_Middle)
          {
            // ISSUE: reference to a compiler-generated field
            Edge edge = this.m_Data.m_Edge[edgeIteratorValue2.m_Edge];
            if (edge.m_Start == node1 || edge.m_End == node1)
              return true;
            Temp componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Data.m_Temp.TryGetComponent(node1, out componentData))
            {
              if (edge.m_Start == componentData.m_Original || edge.m_End == componentData.m_Original)
                return true;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_Data.m_Temp.TryGetComponent(edge.m_Start, out componentData) && componentData.m_Original == node1 || this.m_Data.m_Temp.TryGetComponent(edge.m_End, out componentData) && componentData.m_Original == node1)
                return true;
            }
          }
        }
        return false;
      }
    }

    private struct ObjectIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public Entity m_EdgeEntity;
      public Entity m_TopLevelEntity;
      public Entity m_AssetStampEntity;
      public Bounds3 m_Bounds;
      public Edge m_OriginalNodes;
      public Edge m_NodeOwners;
      public Edge m_NodeAssetStamps;
      public EdgeGeometry m_EdgeGeometryData;
      public StartNodeGeometry m_StartNodeGeometryData;
      public EndNodeGeometry m_EndNodeGeometryData;
      public NetCompositionData m_EdgeCompositionData;
      public NetCompositionData m_StartCompositionData;
      public NetCompositionData m_EndCompositionData;
      public CollisionMask m_EdgeCollisionMask;
      public CollisionMask m_StartCollisionMask;
      public CollisionMask m_EndCollisionMask;
      public CollisionMask m_CombinedCollisionMask;
      public DynamicBuffer<NetCompositionArea> m_EdgeCompositionAreas;
      public DynamicBuffer<NetCompositionArea> m_StartCompositionAreas;
      public DynamicBuffer<NetCompositionArea> m_EndCompositionAreas;
      public ValidationSystem.EntityData m_Data;
      public NativeQueue<ErrorData>.ParallelWriter m_ErrorQueue;
      public bool m_EditorMode;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        if ((bounds.m_Mask & BoundsMask.NotOverridden) == (BoundsMask) 0)
          return false;
        return (this.m_CombinedCollisionMask & CollisionMask.OnGround) != (CollisionMask) 0 ? MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds.xz) : MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity objectEntity2)
      {
        if ((bounds.m_Mask & BoundsMask.NotOverridden) == (BoundsMask) 0)
          return;
        if ((this.m_CombinedCollisionMask & CollisionMask.OnGround) != (CollisionMask) 0)
        {
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds.xz))
            return;
        }
        else if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds))
          return;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Data.m_Hidden.HasComponent(objectEntity2) || this.m_AssetStampEntity == objectEntity2 || this.m_NodeAssetStamps.m_Start == objectEntity2 || this.m_NodeAssetStamps.m_End == objectEntity2)
          return;
        bool flag1 = true;
        bool flag2 = true;
        Entity entity = objectEntity2;
        bool hasOwner = false;
        Owner componentData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        while (this.m_Data.m_Owner.TryGetComponent(entity, out componentData1) && !this.m_Data.m_Building.HasComponent(entity))
        {
          Entity owner = componentData1.m_Owner;
          hasOwner = true;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Data.m_AssetStamp.HasComponent(owner))
          {
            entity = owner;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Data.m_Edge.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              Edge edge = this.m_Data.m_Edge[entity];
              flag1 = ((flag1 ? 1 : 0) & (!(edge.m_Start != this.m_OriginalNodes.m_Start) ? 0 : (edge.m_End != this.m_OriginalNodes.m_Start ? 1 : 0))) != 0;
              flag2 = ((flag2 ? 1 : 0) & (!(edge.m_Start != this.m_OriginalNodes.m_End) ? 0 : (edge.m_End != this.m_OriginalNodes.m_End ? 1 : 0))) != 0;
            }
          }
          else
            break;
        }
        if (this.m_TopLevelEntity == entity)
          return;
        bool checkStart = flag1 & this.m_NodeOwners.m_Start != entity;
        bool checkEnd = flag2 & this.m_NodeOwners.m_End != entity;
        Attached componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Data.m_Attached.TryGetComponent(objectEntity2, out componentData2))
        {
          if (componentData2.m_Parent == this.m_OriginalNodes.m_Start)
            checkStart &= (this.m_StartCompositionData.m_Flags.m_General & CompositionFlags.General.Roundabout) == (CompositionFlags.General) 0;
          if (componentData2.m_Parent == this.m_OriginalNodes.m_End)
            checkEnd &= (this.m_EndCompositionData.m_Flags.m_General & CompositionFlags.General.Roundabout) == (CompositionFlags.General) 0;
        }
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef2 = this.m_Data.m_PrefabRef[objectEntity2];
        // ISSUE: reference to a compiler-generated field
        Transform transform2 = this.m_Data.m_Transform[objectEntity2];
        this.CheckOverlap(objectEntity2, entity, bounds.m_Bounds, prefabRef2, transform2, checkStart, checkEnd, hasOwner);
      }

      public void CheckOverlap(
        Entity objectEntity2,
        Entity topLevelEntity2,
        Bounds3 bounds2,
        PrefabRef prefabRef2,
        Transform transform2,
        bool checkStart,
        bool checkEnd,
        bool hasOwner)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Data.m_PrefabObjectGeometry.HasComponent(prefabRef2.m_Prefab))
          return;
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData objectGeometryData = this.m_Data.m_PrefabObjectGeometry[prefabRef2.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.IgnoreSecondaryCollision) != Game.Objects.GeometryFlags.None && this.m_Data.m_Secondary.HasComponent(objectEntity2))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        CollisionMask collisionMask = !this.m_Data.m_ObjectElevation.HasComponent(objectEntity2) ? ObjectUtils.GetCollisionMask(objectGeometryData, !this.m_EditorMode | hasOwner) : ObjectUtils.GetCollisionMask(objectGeometryData, this.m_Data.m_ObjectElevation[objectEntity2], !this.m_EditorMode | hasOwner);
        if ((this.m_CombinedCollisionMask & collisionMask) == (CollisionMask) 0)
          return;
        ErrorData error = new ErrorData();
        error.m_ErrorSeverity = ErrorSeverity.Error;
        error.m_TempEntity = this.m_EdgeEntity;
        error.m_PermanentEntity = objectEntity2;
        if (topLevelEntity2 != objectEntity2)
        {
          if ((objectGeometryData.m_Flags & (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.DeleteOverridden)) == Game.Objects.GeometryFlags.Overridable)
          {
            error.m_ErrorSeverity = ErrorSeverity.Override;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_Data.m_PrefabRef[topLevelEntity2];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Data.m_PrefabObjectGeometry.HasComponent(prefabRef.m_Prefab) && (this.m_Data.m_PrefabObjectGeometry[prefabRef.m_Prefab].m_Flags & (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.DeleteOverridden)) == (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.DeleteOverridden) && !this.m_Data.m_Attached.HasComponent(topLevelEntity2))
            {
              error.m_ErrorSeverity = ErrorSeverity.Warning;
              error.m_PermanentEntity = topLevelEntity2;
            }
          }
        }
        else if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Overridable) != Game.Objects.GeometryFlags.None)
        {
          if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.DeleteOverridden) != Game.Objects.GeometryFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Data.m_Attached.HasComponent(objectEntity2))
              error.m_ErrorSeverity = ErrorSeverity.Warning;
          }
          else
            error.m_ErrorSeverity = ErrorSeverity.Override;
        }
        float3 origin = MathUtils.Center(bounds2);
        StackData componentData1 = new StackData();
        Stack componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Data.m_Stack.TryGetComponent(objectEntity2, out componentData2))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Data.m_PrefabStackData.TryGetComponent(prefabRef2.m_Prefab, out componentData1);
        }
        if ((this.m_CombinedCollisionMask & CollisionMask.OnGround) == (CollisionMask) 0 || MathUtils.Intersect(bounds2, this.m_Bounds))
          this.CheckOverlap3D(ref error, collisionMask, objectGeometryData, componentData1, bounds2, transform2, componentData2, topLevelEntity2, origin, checkStart, checkEnd);
        if (error.m_ErrorType == ErrorType.None && CommonUtils.ExclusiveGroundCollision(this.m_CombinedCollisionMask, collisionMask))
          this.CheckOverlap2D(ref error, collisionMask, objectGeometryData, bounds2, transform2, topLevelEntity2, origin, checkStart, checkEnd);
        if (error.m_ErrorType == ErrorType.None)
          return;
        // ISSUE: reference to a compiler-generated field
        if ((error.m_ErrorSeverity == ErrorSeverity.Override || error.m_ErrorSeverity == ErrorSeverity.Warning) && error.m_ErrorType == ErrorType.OverlapExisting && this.m_Data.m_OnFire.HasComponent(error.m_PermanentEntity))
        {
          error.m_ErrorType = ErrorType.OnFire;
          error.m_ErrorSeverity = ErrorSeverity.Error;
        }
        this.m_ErrorQueue.Enqueue(error);
      }

      private void CheckOverlap3D(
        ref ErrorData error,
        CollisionMask collisionMask2,
        ObjectGeometryData prefabObjectGeometryData2,
        StackData stackData2,
        Bounds3 bounds2,
        Transform transform2,
        Stack stack2,
        Entity topLevelEntity2,
        float3 origin,
        bool checkStart,
        bool checkEnd)
      {
        Bounds3 intersection;
        intersection.min = (float3) float.MaxValue;
        intersection.max = (float3) float.MinValue;
        float3 float3 = math.mul(math.inverse(transform2.m_Rotation), transform2.m_Position - origin);
        Bounds3 bounds = ObjectUtils.GetBounds(stack2, prefabObjectGeometryData2, stackData2);
        DynamicBuffer<NetCompositionArea> areas1_1 = this.m_EdgeCompositionAreas;
        DynamicBuffer<NetCompositionArea> areas1_2 = this.m_StartCompositionAreas;
        DynamicBuffer<NetCompositionArea> areas1_3 = this.m_EndCompositionAreas;
        if ((prefabObjectGeometryData2.m_Flags & (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.DeleteOverridden)) == Game.Objects.GeometryFlags.Overridable)
        {
          areas1_1 = new DynamicBuffer<NetCompositionArea>();
          areas1_2 = new DynamicBuffer<NetCompositionArea>();
          areas1_3 = new DynamicBuffer<NetCompositionArea>();
        }
        if ((prefabObjectGeometryData2.m_Flags & Game.Objects.GeometryFlags.IgnoreBottomCollision) != Game.Objects.GeometryFlags.None)
          bounds.min.y = math.max(bounds.min.y, 0.0f);
        if ((prefabObjectGeometryData2.m_Flags & Game.Objects.GeometryFlags.Standing) != Game.Objects.GeometryFlags.None)
        {
          if ((prefabObjectGeometryData2.m_Flags & (Game.Objects.GeometryFlags.CircularLeg | Game.Objects.GeometryFlags.IgnoreLegCollision)) == Game.Objects.GeometryFlags.CircularLeg)
          {
            Cylinder3 cylinder2 = new Cylinder3();
            cylinder2.circle = new Circle2(prefabObjectGeometryData2.m_LegSize.x * 0.5f, float3.xz);
            cylinder2.height = new Bounds1(bounds.min.y, prefabObjectGeometryData2.m_LegSize.y) + float3.y;
            cylinder2.rotation = transform2.m_Rotation;
            if ((this.m_EdgeCollisionMask & collisionMask2) != (CollisionMask) 0 && ValidationHelpers.Intersect(this.m_NodeOwners, topLevelEntity2, this.m_EdgeGeometryData, -origin, cylinder2, bounds2, this.m_EdgeCompositionData, areas1_1, ref intersection))
              error.m_ErrorType = ErrorType.OverlapExisting;
            if ((this.m_StartCollisionMask & collisionMask2) != 0 & checkStart && ValidationHelpers.Intersect(this.m_NodeOwners.m_Start, topLevelEntity2, this.m_StartNodeGeometryData.m_Geometry, -origin, cylinder2, bounds2, this.m_StartCompositionData, areas1_2, ref intersection))
              error.m_ErrorType = ErrorType.OverlapExisting;
            if ((this.m_EndCollisionMask & collisionMask2) != 0 & checkEnd && ValidationHelpers.Intersect(this.m_NodeOwners.m_End, topLevelEntity2, this.m_EndNodeGeometryData.m_Geometry, -origin, cylinder2, bounds2, this.m_EndCompositionData, areas1_3, ref intersection))
              error.m_ErrorType = ErrorType.OverlapExisting;
          }
          else if ((prefabObjectGeometryData2.m_Flags & Game.Objects.GeometryFlags.IgnoreLegCollision) == Game.Objects.GeometryFlags.None)
          {
            Box3 box2 = new Box3(new Bounds3()
            {
              min = {
                y = bounds.min.y,
                xz = prefabObjectGeometryData2.m_LegSize.xz * -0.5f
              },
              max = {
                y = prefabObjectGeometryData2.m_LegSize.y,
                xz = prefabObjectGeometryData2.m_LegSize.xz * 0.5f
              }
            } + float3, transform2.m_Rotation);
            if ((this.m_EdgeCollisionMask & collisionMask2) != (CollisionMask) 0 && ValidationHelpers.Intersect(this.m_NodeOwners, topLevelEntity2, this.m_EdgeGeometryData, -origin, box2, bounds2, this.m_EdgeCompositionData, areas1_1, ref intersection))
              error.m_ErrorType = ErrorType.OverlapExisting;
            if ((this.m_StartCollisionMask & collisionMask2) != 0 & checkStart && ValidationHelpers.Intersect(this.m_NodeOwners.m_Start, topLevelEntity2, this.m_StartNodeGeometryData.m_Geometry, -origin, box2, bounds2, this.m_StartCompositionData, areas1_2, ref intersection))
              error.m_ErrorType = ErrorType.OverlapExisting;
            if ((this.m_EndCollisionMask & collisionMask2) != 0 & checkEnd && ValidationHelpers.Intersect(this.m_NodeOwners.m_End, topLevelEntity2, this.m_EndNodeGeometryData.m_Geometry, -origin, box2, bounds2, this.m_EndCompositionData, areas1_3, ref intersection))
              error.m_ErrorType = ErrorType.OverlapExisting;
          }
          bounds.min.y = prefabObjectGeometryData2.m_LegSize.y;
        }
        if ((prefabObjectGeometryData2.m_Flags & Game.Objects.GeometryFlags.Circular) != Game.Objects.GeometryFlags.None)
        {
          Cylinder3 cylinder2 = new Cylinder3();
          cylinder2.circle = new Circle2(prefabObjectGeometryData2.m_Size.x * 0.5f, float3.xz);
          cylinder2.height = new Bounds1(bounds.min.y, bounds.max.y) + float3.y;
          cylinder2.rotation = transform2.m_Rotation;
          if ((this.m_EdgeCollisionMask & collisionMask2) != (CollisionMask) 0 && ValidationHelpers.Intersect(this.m_NodeOwners, topLevelEntity2, this.m_EdgeGeometryData, -origin, cylinder2, bounds2, this.m_EdgeCompositionData, areas1_1, ref intersection))
            error.m_ErrorType = ErrorType.OverlapExisting;
          if ((this.m_StartCollisionMask & collisionMask2) != 0 & checkStart && ValidationHelpers.Intersect(this.m_NodeOwners.m_Start, topLevelEntity2, this.m_StartNodeGeometryData.m_Geometry, -origin, cylinder2, bounds2, this.m_StartCompositionData, areas1_2, ref intersection))
            error.m_ErrorType = ErrorType.OverlapExisting;
          if ((this.m_EndCollisionMask & collisionMask2) != 0 & checkEnd && ValidationHelpers.Intersect(this.m_NodeOwners.m_End, topLevelEntity2, this.m_EndNodeGeometryData.m_Geometry, -origin, cylinder2, bounds2, this.m_EndCompositionData, areas1_3, ref intersection))
            error.m_ErrorType = ErrorType.OverlapExisting;
        }
        else
        {
          Box3 box2 = new Box3();
          box2.bounds = bounds + float3;
          box2.rotation = transform2.m_Rotation;
          if ((this.m_EdgeCollisionMask & collisionMask2) != (CollisionMask) 0 && ValidationHelpers.Intersect(this.m_NodeOwners, topLevelEntity2, this.m_EdgeGeometryData, -origin, box2, bounds2, this.m_EdgeCompositionData, areas1_1, ref intersection))
            error.m_ErrorType = ErrorType.OverlapExisting;
          if ((this.m_StartCollisionMask & collisionMask2) != 0 & checkStart && ValidationHelpers.Intersect(this.m_NodeOwners.m_Start, topLevelEntity2, this.m_StartNodeGeometryData.m_Geometry, -origin, box2, bounds2, this.m_StartCompositionData, areas1_2, ref intersection))
            error.m_ErrorType = ErrorType.OverlapExisting;
          if ((this.m_EndCollisionMask & collisionMask2) != 0 & checkEnd && ValidationHelpers.Intersect(this.m_NodeOwners.m_End, topLevelEntity2, this.m_EndNodeGeometryData.m_Geometry, -origin, box2, bounds2, this.m_EndCompositionData, areas1_3, ref intersection))
            error.m_ErrorType = ErrorType.OverlapExisting;
        }
        if (error.m_ErrorType == ErrorType.None)
          return;
        error.m_Position = origin + MathUtils.Center(intersection);
      }

      private void CheckOverlap2D(
        ref ErrorData error,
        CollisionMask collisionMask2,
        ObjectGeometryData prefabObjectGeometryData2,
        Bounds3 bounds2,
        Transform transformData2,
        Entity topLevelEntity2,
        float3 origin,
        bool checkStart,
        bool checkEnd)
      {
        Bounds2 intersection;
        intersection.min = (float2) float.MaxValue;
        intersection.max = (float2) float.MinValue;
        Bounds1 bounds;
        bounds.min = float.MaxValue;
        bounds.max = float.MinValue;
        DynamicBuffer<NetCompositionArea> areas1_1 = this.m_EdgeCompositionAreas;
        DynamicBuffer<NetCompositionArea> areas1_2 = this.m_StartCompositionAreas;
        DynamicBuffer<NetCompositionArea> areas1_3 = this.m_EndCompositionAreas;
        if ((prefabObjectGeometryData2.m_Flags & (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.DeleteOverridden)) == Game.Objects.GeometryFlags.Overridable)
        {
          areas1_1 = new DynamicBuffer<NetCompositionArea>();
          areas1_2 = new DynamicBuffer<NetCompositionArea>();
          areas1_3 = new DynamicBuffer<NetCompositionArea>();
        }
        if ((prefabObjectGeometryData2.m_Flags & Game.Objects.GeometryFlags.Standing) != Game.Objects.GeometryFlags.None)
        {
          if ((prefabObjectGeometryData2.m_Flags & (Game.Objects.GeometryFlags.CircularLeg | Game.Objects.GeometryFlags.IgnoreLegCollision)) == Game.Objects.GeometryFlags.CircularLeg)
          {
            Circle2 circle2 = new Circle2(prefabObjectGeometryData2.m_LegSize.x * 0.5f, (transformData2.m_Position - origin).xz);
            if (CommonUtils.ExclusiveGroundCollision(this.m_EdgeCollisionMask, collisionMask2) && ValidationHelpers.Intersect(this.m_NodeOwners, topLevelEntity2, this.m_EdgeGeometryData, -origin.xz, circle2, bounds2.xz, this.m_EdgeCompositionData, areas1_1, ref intersection))
            {
              error.m_ErrorType = ErrorType.OverlapExisting;
              bounds |= MathUtils.Center(this.m_EdgeGeometryData.m_Bounds.y & bounds2.y);
            }
            if (CommonUtils.ExclusiveGroundCollision(this.m_StartCollisionMask, collisionMask2) & checkStart && ValidationHelpers.Intersect(this.m_NodeOwners.m_Start, topLevelEntity2, this.m_StartNodeGeometryData.m_Geometry, -origin.xz, circle2, bounds2.xz, this.m_StartCompositionData, areas1_2, ref intersection))
            {
              error.m_ErrorType = ErrorType.OverlapExisting;
              bounds |= MathUtils.Center(this.m_StartNodeGeometryData.m_Geometry.m_Bounds.y & bounds2.y);
            }
            if (CommonUtils.ExclusiveGroundCollision(this.m_EndCollisionMask, collisionMask2) & checkEnd && ValidationHelpers.Intersect(this.m_NodeOwners.m_End, topLevelEntity2, this.m_EndNodeGeometryData.m_Geometry, -origin.xz, circle2, bounds2.xz, this.m_EndCompositionData, areas1_3, ref intersection))
            {
              error.m_ErrorType = ErrorType.OverlapExisting;
              bounds |= MathUtils.Center(this.m_EndNodeGeometryData.m_Geometry.m_Bounds.y & bounds2.y);
            }
          }
          else if ((prefabObjectGeometryData2.m_Flags & Game.Objects.GeometryFlags.IgnoreLegCollision) == Game.Objects.GeometryFlags.None)
          {
            Quad2 xz = ObjectUtils.CalculateBaseCorners(transformData2.m_Position - origin, transformData2.m_Rotation, new Bounds3()
            {
              min = {
                xz = prefabObjectGeometryData2.m_LegSize.xz * -0.5f
              },
              max = {
                xz = prefabObjectGeometryData2.m_LegSize.xz * 0.5f
              }
            }).xz;
            if (CommonUtils.ExclusiveGroundCollision(this.m_EdgeCollisionMask, collisionMask2) && ValidationHelpers.Intersect(this.m_NodeOwners, topLevelEntity2, this.m_EdgeGeometryData, -origin.xz, xz, bounds2.xz, this.m_EdgeCompositionData, areas1_1, ref intersection))
            {
              error.m_ErrorType = ErrorType.OverlapExisting;
              bounds |= MathUtils.Center(this.m_EdgeGeometryData.m_Bounds.y & bounds2.y);
            }
            if (CommonUtils.ExclusiveGroundCollision(this.m_StartCollisionMask, collisionMask2) & checkStart && ValidationHelpers.Intersect(this.m_NodeOwners.m_Start, topLevelEntity2, this.m_StartNodeGeometryData.m_Geometry, -origin.xz, xz, bounds2.xz, this.m_StartCompositionData, areas1_2, ref intersection))
            {
              error.m_ErrorType = ErrorType.OverlapExisting;
              bounds |= MathUtils.Center(this.m_StartNodeGeometryData.m_Geometry.m_Bounds.y & bounds2.y);
            }
            if (CommonUtils.ExclusiveGroundCollision(this.m_EndCollisionMask, collisionMask2) & checkEnd && ValidationHelpers.Intersect(this.m_NodeOwners.m_End, topLevelEntity2, this.m_EndNodeGeometryData.m_Geometry, -origin.xz, xz, bounds2.xz, this.m_EndCompositionData, areas1_3, ref intersection))
            {
              error.m_ErrorType = ErrorType.OverlapExisting;
              bounds |= MathUtils.Center(this.m_EndNodeGeometryData.m_Geometry.m_Bounds.y & bounds2.y);
            }
          }
        }
        else if ((prefabObjectGeometryData2.m_Flags & Game.Objects.GeometryFlags.Circular) != Game.Objects.GeometryFlags.None)
        {
          Circle2 circle2 = new Circle2(prefabObjectGeometryData2.m_Size.x * 0.5f, (transformData2.m_Position - origin).xz);
          if (CommonUtils.ExclusiveGroundCollision(this.m_EdgeCollisionMask, collisionMask2) && ValidationHelpers.Intersect(this.m_NodeOwners, topLevelEntity2, this.m_EdgeGeometryData, -origin.xz, circle2, bounds2.xz, this.m_EdgeCompositionData, areas1_1, ref intersection))
          {
            error.m_ErrorType = ErrorType.OverlapExisting;
            bounds |= MathUtils.Center(this.m_EdgeGeometryData.m_Bounds.y & bounds2.y);
          }
          if (CommonUtils.ExclusiveGroundCollision(this.m_StartCollisionMask, collisionMask2) & checkStart && ValidationHelpers.Intersect(this.m_NodeOwners.m_Start, topLevelEntity2, this.m_StartNodeGeometryData.m_Geometry, -origin.xz, circle2, bounds2.xz, this.m_StartCompositionData, areas1_2, ref intersection))
          {
            error.m_ErrorType = ErrorType.OverlapExisting;
            bounds |= MathUtils.Center(this.m_StartNodeGeometryData.m_Geometry.m_Bounds.y & bounds2.y);
          }
          if (CommonUtils.ExclusiveGroundCollision(this.m_EndCollisionMask, collisionMask2) & checkEnd && ValidationHelpers.Intersect(this.m_NodeOwners.m_End, topLevelEntity2, this.m_EndNodeGeometryData.m_Geometry, -origin.xz, circle2, bounds2.xz, this.m_EndCompositionData, areas1_3, ref intersection))
          {
            error.m_ErrorType = ErrorType.OverlapExisting;
            bounds |= MathUtils.Center(this.m_EndNodeGeometryData.m_Geometry.m_Bounds.y & bounds2.y);
          }
        }
        else
        {
          Quad2 xz = ObjectUtils.CalculateBaseCorners(transformData2.m_Position - origin, transformData2.m_Rotation, prefabObjectGeometryData2.m_Bounds).xz;
          if (CommonUtils.ExclusiveGroundCollision(this.m_EdgeCollisionMask, collisionMask2) && ValidationHelpers.Intersect(this.m_NodeOwners, topLevelEntity2, this.m_EdgeGeometryData, -origin.xz, xz, bounds2.xz, this.m_EdgeCompositionData, areas1_1, ref intersection))
          {
            error.m_ErrorType = ErrorType.OverlapExisting;
            bounds |= MathUtils.Center(this.m_EdgeGeometryData.m_Bounds.y & bounds2.y);
          }
          if (CommonUtils.ExclusiveGroundCollision(this.m_StartCollisionMask, collisionMask2) & checkStart && ValidationHelpers.Intersect(this.m_NodeOwners.m_Start, topLevelEntity2, this.m_StartNodeGeometryData.m_Geometry, -origin.xz, xz, bounds2.xz, this.m_StartCompositionData, areas1_2, ref intersection))
          {
            error.m_ErrorType = ErrorType.OverlapExisting;
            bounds |= MathUtils.Center(this.m_StartNodeGeometryData.m_Geometry.m_Bounds.y & bounds2.y);
          }
          if (CommonUtils.ExclusiveGroundCollision(this.m_EndCollisionMask, collisionMask2) & checkEnd && ValidationHelpers.Intersect(this.m_NodeOwners.m_End, topLevelEntity2, this.m_EndNodeGeometryData.m_Geometry, -origin.xz, xz, bounds2.xz, this.m_EndCompositionData, areas1_3, ref intersection))
          {
            error.m_ErrorType = ErrorType.OverlapExisting;
            bounds |= MathUtils.Center(this.m_EndNodeGeometryData.m_Geometry.m_Bounds.y & bounds2.y);
          }
        }
        if (error.m_ErrorType == ErrorType.None)
          return;
        error.m_Position.xz = origin.xz + MathUtils.Center(intersection);
        error.m_Position.y = MathUtils.Center(bounds);
      }
    }

    private struct AreaIterator : 
      INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
    {
      public Entity m_EdgeEntity;
      public Bounds3 m_Bounds;
      public Edge m_NodeOwners;
      public bool m_IgnoreCollisions;
      public bool m_IgnoreProtectedAreas;
      public bool m_EditorMode;
      public EdgeGeometry m_EdgeGeometryData;
      public StartNodeGeometry m_StartNodeGeometryData;
      public EndNodeGeometry m_EndNodeGeometryData;
      public NetCompositionData m_EdgeCompositionData;
      public NetCompositionData m_StartCompositionData;
      public NetCompositionData m_EndCompositionData;
      public CollisionMask m_EdgeCollisionMask;
      public CollisionMask m_StartCollisionMask;
      public CollisionMask m_EndCollisionMask;
      public CollisionMask m_CombinedCollisionMask;
      public ValidationSystem.EntityData m_Data;
      public NativeQueue<ErrorData>.ParallelWriter m_ErrorQueue;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds.xz);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem areaItem2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds.xz) || this.m_Data.m_Hidden.HasComponent(areaItem2.m_Area) || (this.m_Data.m_Area[areaItem2.m_Area].m_Flags & AreaFlags.Slave) != (AreaFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        AreaGeometryData areaGeometryData = this.m_Data.m_PrefabAreaGeometry[this.m_Data.m_PrefabRef[areaItem2.m_Area].m_Prefab];
        // ISSUE: reference to a compiler-generated field
        AreaUtils.SetCollisionFlags(ref areaGeometryData, !this.m_EditorMode || this.m_Data.m_Owner.HasComponent(areaItem2.m_Area));
        if ((areaGeometryData.m_Flags & (Game.Areas.GeometryFlags.PhysicalGeometry | Game.Areas.GeometryFlags.ProtectedArea)) == (Game.Areas.GeometryFlags) 0)
          return;
        if ((areaGeometryData.m_Flags & Game.Areas.GeometryFlags.ProtectedArea) != (Game.Areas.GeometryFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Data.m_Native.HasComponent(areaItem2.m_Area) || this.m_IgnoreProtectedAreas)
            return;
        }
        else if (this.m_IgnoreCollisions)
          return;
        CollisionMask collisionMask = AreaUtils.GetCollisionMask(areaGeometryData);
        if ((this.m_CombinedCollisionMask & collisionMask) == (CollisionMask) 0)
          return;
        ErrorType errorType = ErrorType.OverlapExisting;
        if (areaGeometryData.m_Type == AreaType.MapTile)
        {
          errorType = ErrorType.ExceedsCityLimits;
          if ((this.m_EdgeCompositionData.m_State & CompositionState.Airspace) != (CompositionState) 0)
            return;
        }
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Areas.Node> areaNode = this.m_Data.m_AreaNodes[areaItem2.m_Area];
        // ISSUE: reference to a compiler-generated field
        Triangle triangle1 = this.m_Data.m_AreaTriangles[areaItem2.m_Area][areaItem2.m_Triangle];
        Triangle triangle2 = triangle1;
        Triangle3 triangle3 = AreaUtils.GetTriangle3(areaNode, triangle2);
        ErrorData errorData = new ErrorData();
        Bounds3 intersection;
        intersection.min = (float3) float.MaxValue;
        intersection.max = (float3) float.MinValue;
        if (areaGeometryData.m_Type != AreaType.MapTile && ((this.m_CombinedCollisionMask & CollisionMask.OnGround) == (CollisionMask) 0 || MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds)))
        {
          Bounds1 heightRange = triangle1.m_HeightRange;
          heightRange.max += areaGeometryData.m_MaxHeight;
          if ((this.m_EdgeCollisionMask & collisionMask) != (CollisionMask) 0 && ValidationHelpers.Intersect(this.m_NodeOwners, areaItem2.m_Area, this.m_EdgeGeometryData, triangle3, this.m_EdgeCompositionData, heightRange, ref intersection))
            errorData.m_ErrorType = errorType;
          if ((this.m_StartCollisionMask & collisionMask) != (CollisionMask) 0 && ValidationHelpers.Intersect(this.m_NodeOwners.m_Start, areaItem2.m_Area, this.m_StartNodeGeometryData.m_Geometry, triangle3, this.m_StartCompositionData, heightRange, ref intersection))
            errorData.m_ErrorType = errorType;
          if ((this.m_EndCollisionMask & collisionMask) != (CollisionMask) 0 && ValidationHelpers.Intersect(this.m_NodeOwners.m_End, areaItem2.m_Area, this.m_EndNodeGeometryData.m_Geometry, triangle3, this.m_EndCompositionData, heightRange, ref intersection))
            errorData.m_ErrorType = errorType;
        }
        if (areaGeometryData.m_Type == AreaType.MapTile || errorData.m_ErrorType == ErrorType.None && CommonUtils.ExclusiveGroundCollision(this.m_CombinedCollisionMask, collisionMask))
        {
          if ((this.m_EdgeCollisionMask & collisionMask) != (CollisionMask) 0 && ValidationHelpers.Intersect(this.m_NodeOwners, areaItem2.m_Area, this.m_EdgeGeometryData, triangle3.xz, bounds.m_Bounds, this.m_EdgeCompositionData, ref intersection))
            errorData.m_ErrorType = errorType;
          if ((this.m_StartCollisionMask & collisionMask) != (CollisionMask) 0 && ValidationHelpers.Intersect(this.m_NodeOwners.m_Start, areaItem2.m_Area, this.m_StartNodeGeometryData.m_Geometry, triangle3.xz, bounds.m_Bounds, this.m_StartCompositionData, ref intersection))
            errorData.m_ErrorType = errorType;
          if ((this.m_EndCollisionMask & collisionMask) != (CollisionMask) 0 && ValidationHelpers.Intersect(this.m_NodeOwners.m_End, areaItem2.m_Area, this.m_EndNodeGeometryData.m_Geometry, triangle3.xz, bounds.m_Bounds, this.m_EndCompositionData, ref intersection))
            errorData.m_ErrorType = errorType;
        }
        if (errorData.m_ErrorType == ErrorType.None)
          return;
        errorData.m_ErrorSeverity = ErrorSeverity.Error;
        errorData.m_TempEntity = this.m_EdgeEntity;
        errorData.m_PermanentEntity = areaItem2.m_Area;
        errorData.m_Position = MathUtils.Center(intersection);
        errorData.m_Position.y = MathUtils.Clamp(errorData.m_Position.y, this.m_Bounds.y);
        this.m_ErrorQueue.Enqueue(errorData);
      }
    }
  }
}
