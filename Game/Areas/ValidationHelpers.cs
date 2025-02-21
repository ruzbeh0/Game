// Decompiled with JetBrains decompiler
// Type: Game.Areas.ValidationHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Areas
{
  public static class ValidationHelpers
  {
    public static void ValidateArea(
      bool editorMode,
      Entity entity,
      Temp temp,
      Owner owner,
      Area area,
      Geometry geometry,
      Storage storage,
      DynamicBuffer<Node> nodes,
      PrefabRef prefabRef,
      ValidationSystem.EntityData data,
      NativeQuadTree<Entity, QuadTreeBoundsXZ> objectSearchTree,
      NativeQuadTree<Entity, QuadTreeBoundsXZ> netSearchTree,
      NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> areaSearchTree,
      WaterSurfaceData waterSurfaceData,
      TerrainHeightData terrainHeightData,
      NativeQueue<ErrorData>.ParallelWriter errorQueue)
    {
      if ((area.m_Flags & AreaFlags.Slave) != (AreaFlags) 0)
        return;
      // ISSUE: reference to a compiler-generated field
      float minNodeDistance = AreaUtils.GetMinNodeDistance(data.m_PrefabAreaGeometry[prefabRef.m_Prefab]);
      bool flag1 = true;
      bool flag2 = (area.m_Flags & AreaFlags.Complete) != 0;
      bool isCounterClockwise = (area.m_Flags & AreaFlags.CounterClockwise) != 0;
      Node node;
      if (nodes.Length == 2)
        ValidationHelpers.ValidateTriangle(editorMode, false, entity, temp, owner, new Triangle(0, 1, 1), data, objectSearchTree, netSearchTree, areaSearchTree, waterSurfaceData, terrainHeightData, errorQueue);
      else if (nodes.Length == 3)
      {
        if ((temp.m_Flags & TempFlags.Delete) == (TempFlags) 0)
        {
          Line3.Segment line1_1 = new Line3.Segment(nodes[0].m_Position, nodes[1].m_Position);
          Line3.Segment line1_2 = new Line3.Segment(nodes[1].m_Position, nodes[2].m_Position);
          flag1 = flag1 & ValidationHelpers.CheckShape(line1_1, nodes[2].m_Position, entity, minNodeDistance, errorQueue) & ValidationHelpers.CheckShape(line1_2, nodes[0].m_Position, entity, minNodeDistance, errorQueue);
          if (flag2)
          {
            Line3.Segment line1_3 = new Line3.Segment(nodes[2].m_Position, nodes[0].m_Position);
            flag1 &= ValidationHelpers.CheckShape(line1_3, nodes[1].m_Position, entity, minNodeDistance, errorQueue);
          }
        }
      }
      else if (nodes.Length > 3 && (temp.m_Flags & TempFlags.Delete) == (TempFlags) 0)
      {
        int num1 = 0;
        int num2 = math.select(nodes.Length - 1, nodes.Length, flag2);
        NativeArray<Bounds2> nativeArray = new NativeArray<Bounds2>();
        int num3 = num2 - num1 - 2;
        int num4 = 0;
        float2 t;
        if (num3 > 10)
        {
          int length = -1;
          int num5 = 0;
          for (; num3 >= 2; num3 >>= 1)
            length += 1 << num5++;
          nativeArray = new NativeArray<Bounds2>(length, Allocator.Temp);
          int num6;
          num4 = num6 = num5 - 1;
          int num7 = 1 << num6;
          int num8 = length - num7;
          num3 = num2 - num1 - 2;
          for (int index1 = 0; index1 < num7; ++index1)
          {
            int num9 = index1 * num3 >> num6;
            int num10 = (index1 + 1) * num3 >> num6;
            Bounds2 bounds;
            ref Bounds2 local1 = ref bounds;
            ref Bounds2 local2 = ref bounds;
            ref DynamicBuffer<Node> local3 = ref nodes;
            int index2 = num9;
            int num11 = index2 + 1;
            node = local3[index2];
            float2 xz1;
            t = xz1 = node.m_Position.xz;
            local2.max = xz1;
            float2 float2 = t;
            local1.min = float2;
            for (int index3 = num11; index3 <= num10; ++index3)
            {
              Bounds2 bounds2 = bounds;
              node = nodes[index3];
              float2 xz2 = node.m_Position.xz;
              bounds = bounds2 | xz2;
            }
            nativeArray[num8 + index1] = MathUtils.Expand(bounds, (float2) minNodeDistance);
          }
          while (--num6 > 0)
          {
            int num12 = num8;
            int num13 = 1 << num6;
            num8 -= num13;
            for (int index = 0; index < num13; ++index)
              nativeArray[num8 + index] = nativeArray[num12 + (index << 1)] | nativeArray[num12 + (index << 1) + 1];
          }
        }
        Line3.Segment line1 = new Line3.Segment();
        ref Line3.Segment local4 = ref line1;
        ref DynamicBuffer<Node> local5 = ref nodes;
        int index4 = num1;
        int num14 = index4 + 1;
        float3 position1 = local5[index4].m_Position;
        local4.a = position1;
        for (int a = num14; a <= num2; ++a)
        {
          int num15 = math.select(a, 0, a == nodes.Length);
          line1.b = nodes[num15].m_Position;
          int x1 = math.select(0, 1, a == nodes.Length);
          int x2 = a - 2;
          if (nativeArray.IsCreated)
          {
            int num16 = 0;
            int num17 = 1;
            int num18 = 0;
            while (num17 > 0)
            {
              if (MathUtils.Intersect(nativeArray[num16 + num18], line1.xz, out t))
              {
                if (num17 == num4)
                {
                  int num19 = math.max(x1, num18 * num3 >> num17);
                  int num20 = math.min(x2, (num18 + 1) * num3 >> num17);
                  if (num20 > num19)
                  {
                    Line3.Segment line2 = new Line3.Segment();
                    ref Line3.Segment local6 = ref line2;
                    ref DynamicBuffer<Node> local7 = ref nodes;
                    int index5 = num19;
                    int num21 = index5 + 1;
                    float3 position2 = local7[index5].m_Position;
                    local6.a = position2;
                    for (int index6 = num21; index6 <= num20; ++index6)
                    {
                      line2.b = nodes[index6].m_Position;
                      flag1 &= ValidationHelpers.CheckShape(line1, line2, entity, minNodeDistance, errorQueue, nodes, num15, index6, flag2, isCounterClockwise);
                      line2.a = line2.b;
                    }
                  }
                }
                else
                {
                  num18 <<= 1;
                  num16 += 1 << num17++;
                  continue;
                }
              }
              while ((num18 & 1) != 0)
              {
                num18 >>= 1;
                num16 -= 1 << --num17;
              }
              ++num18;
            }
          }
          else
          {
            Line3.Segment line2 = new Line3.Segment();
            ref Line3.Segment local8 = ref line2;
            ref DynamicBuffer<Node> local9 = ref nodes;
            int index7 = x1;
            int num22 = index7 + 1;
            float3 position3 = local9[index7].m_Position;
            local8.a = position3;
            for (int index8 = num22; index8 <= x2; ++index8)
            {
              line2.b = nodes[index8].m_Position;
              flag1 &= ValidationHelpers.CheckShape(line1, line2, entity, minNodeDistance, errorQueue, nodes, num15, index8, flag2, isCounterClockwise);
              line2.a = line2.b;
            }
          }
          if (a > num14 | flag2)
          {
            int num23 = a - 2;
            int index9 = num23 + math.select(0, nodes.Length, num23 < 0);
            flag1 &= ValidationHelpers.CheckShape(line1, nodes[index9].m_Position, entity, minNodeDistance, errorQueue);
          }
          if (a < num2 | flag2)
          {
            int num24 = a + 1;
            int index10 = num24 - math.select(0, nodes.Length, num24 >= nodes.Length);
            flag1 &= ValidationHelpers.CheckShape(line1, nodes[index10].m_Position, entity, minNodeDistance, errorQueue);
          }
          if (!flag2)
          {
            if (a > num14)
              flag1 &= ValidationHelpers.CheckShape(line1, nodes[0].m_Position, entity, minNodeDistance, errorQueue, nodes, num15, 0, flag2, isCounterClockwise);
            if (a < num2)
              flag1 &= ValidationHelpers.CheckShape(line1, nodes[nodes.Length - 1].m_Position, entity, minNodeDistance, errorQueue, nodes, num15, nodes.Length - 1, flag2, isCounterClockwise);
          }
          line1.a = line1.b;
        }
        if (nativeArray.IsCreated)
          nativeArray.Dispose();
      }
      if (!flag2 && nodes.Length >= 3)
        ValidationHelpers.ValidateTriangle(editorMode, false, entity, temp, owner, new Triangle(nodes.Length - 2, nodes.Length - 1, nodes.Length - 1), data, objectSearchTree, netSearchTree, areaSearchTree, waterSurfaceData, terrainHeightData, errorQueue);
      if (flag1 & flag2 && (area.m_Flags & AreaFlags.NoTriangles) != (AreaFlags) 0 && nodes.Length >= 3)
      {
        float3 float3_1 = (float3) 0;
        for (int index = 0; index < nodes.Length; ++index)
          float3_1 += nodes[index].m_Position;
        float3 float3_2 = float3_1 / (float) nodes.Length;
        errorQueue.Enqueue(new ErrorData()
        {
          m_ErrorSeverity = ErrorSeverity.Error,
          m_ErrorType = ErrorType.InvalidShape,
          m_TempEntity = entity,
          m_Position = float3_2
        });
        flag1 = false;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((temp.m_Flags & TempFlags.Delete) == (TempFlags) 0 && data.m_Transform.HasComponent(owner.m_Owner) && data.m_PrefabLotData.HasComponent(prefabRef.m_Prefab))
      {
        // ISSUE: reference to a compiler-generated field
        float2 xz3 = data.m_Transform[owner.m_Owner].m_Position.xz;
        // ISSUE: reference to a compiler-generated field
        float maxRadius = data.m_PrefabLotData[prefabRef.m_Prefab].m_MaxRadius;
        for (int index = 0; index < nodes.Length; ++index)
        {
          float2 x = xz3;
          node = nodes[index];
          float2 xz4 = node.m_Position.xz;
          if ((double) math.distance(x, xz4) > (double) maxRadius)
            errorQueue.Enqueue(new ErrorData()
            {
              m_ErrorSeverity = ErrorSeverity.Error,
              m_ErrorType = ErrorType.LongDistance,
              m_TempEntity = entity,
              m_PermanentEntity = owner.m_Owner,
              m_Position = nodes[index].m_Position
            });
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (!((temp.m_Flags & TempFlags.Delete) == (TempFlags) 0 & flag1 & flag2) || !data.m_PrefabStorageArea.HasComponent(prefabRef.m_Prefab))
        return;
      // ISSUE: reference to a compiler-generated field
      StorageAreaData prefabStorageData = data.m_PrefabStorageArea[prefabRef.m_Prefab];
      int storageCapacity = AreaUtils.CalculateStorageCapacity(geometry, prefabStorageData);
      if (storage.m_Amount <= storageCapacity)
        return;
      errorQueue.Enqueue(new ErrorData()
      {
        m_ErrorSeverity = ErrorSeverity.Error,
        m_ErrorType = ErrorType.SmallArea,
        m_TempEntity = entity,
        m_Position = geometry.m_CenterPosition
      });
    }

    private static bool CheckShape(
      Line3.Segment line1,
      float3 node2,
      Entity entity,
      float minNodeDistance,
      NativeQueue<ErrorData>.ParallelWriter errorQueue)
    {
      float t;
      if ((double) MathUtils.Distance(line1.xz, node2.xz, out t) >= (double) minNodeDistance)
        return true;
      errorQueue.Enqueue(new ErrorData()
      {
        m_ErrorSeverity = ErrorSeverity.Error,
        m_ErrorType = ErrorType.InvalidShape,
        m_TempEntity = entity,
        m_Position = math.lerp(MathUtils.Position(line1, t), node2, 0.5f)
      });
      return false;
    }

    private static bool CheckShape(
      Line3.Segment line1,
      float3 node2,
      Entity entity,
      float minNodeDistance,
      NativeQueue<ErrorData>.ParallelWriter errorQueue,
      DynamicBuffer<Node> nodes,
      int index1,
      int index2,
      bool isComplete,
      bool isCounterClockwise)
    {
      float t;
      if ((double) MathUtils.Distance(line1.xz, node2.xz, out t) < (double) minNodeDistance)
      {
        Quad2 edgeQuad = ValidationHelpers.GetEdgeQuad(minNodeDistance, nodes, index1, isComplete, isCounterClockwise);
        Line2.Segment edgeLine = ValidationHelpers.GetEdgeLine(minNodeDistance, nodes, index2, isComplete, isCounterClockwise);
        float2 xz = node2.xz;
        if (MathUtils.Intersect(edgeQuad, xz) || MathUtils.Intersect(edgeLine, line1.xz, out float2 _))
        {
          errorQueue.Enqueue(new ErrorData()
          {
            m_ErrorSeverity = ErrorSeverity.Error,
            m_ErrorType = ErrorType.InvalidShape,
            m_TempEntity = entity,
            m_Position = math.lerp(MathUtils.Position(line1, t), node2, 0.5f)
          });
          return false;
        }
      }
      return true;
    }

    private static bool CheckShape(
      Line3.Segment line1,
      Line3.Segment line2,
      Entity entity,
      float minNodeDistance,
      NativeQueue<ErrorData>.ParallelWriter errorQueue,
      DynamicBuffer<Node> nodes,
      int index1,
      int index2,
      bool isComplete,
      bool isCounterClockwise)
    {
      float2 t1;
      if ((double) MathUtils.Distance(line1.xz, line2.xz, out t1) < (double) minNodeDistance)
      {
        Quad2 edgeQuad1 = ValidationHelpers.GetEdgeQuad(minNodeDistance, nodes, index1, isComplete, isCounterClockwise);
        Quad2 edgeQuad2 = ValidationHelpers.GetEdgeQuad(minNodeDistance, nodes, index2, isComplete, isCounterClockwise);
        Line2.Segment xz = line2.xz;
        float2 t2;
        ref float2 local = ref t2;
        if (MathUtils.Intersect(edgeQuad1, xz, out local) || MathUtils.Intersect(edgeQuad2, line1.xz, out t2))
        {
          errorQueue.Enqueue(new ErrorData()
          {
            m_ErrorSeverity = ErrorSeverity.Error,
            m_ErrorType = ErrorType.InvalidShape,
            m_TempEntity = entity,
            m_Position = math.lerp(MathUtils.Position(line1, t1.x), MathUtils.Position(line2, t1.y), 0.5f)
          });
          return false;
        }
      }
      return true;
    }

    private static Line2.Segment GetEdgeLine(
      float minNodeDistance,
      DynamicBuffer<Node> nodes,
      int index,
      bool isComplete,
      bool isCounterClockwise)
    {
      Line2.Segment edgeLine;
      edgeLine.a = AreaUtils.GetExpandedNode(nodes, index, -0.1f, isComplete, isCounterClockwise).xz;
      edgeLine.b = AreaUtils.GetExpandedNode(nodes, index, -minNodeDistance, isComplete, isCounterClockwise).xz;
      return edgeLine;
    }

    private static Quad2 GetEdgeQuad(
      float minNodeDistance,
      DynamicBuffer<Node> nodes,
      int index,
      bool isComplete,
      bool isCounterClockwise)
    {
      int index1 = math.select(index - 1, index + nodes.Length - 1, index == 0);
      Quad2 edgeQuad;
      edgeQuad.a = AreaUtils.GetExpandedNode(nodes, index1, -minNodeDistance, isComplete, isCounterClockwise).xz;
      edgeQuad.b = AreaUtils.GetExpandedNode(nodes, index1, -0.1f, isComplete, isCounterClockwise).xz;
      edgeQuad.c = AreaUtils.GetExpandedNode(nodes, index, -0.1f, isComplete, isCounterClockwise).xz;
      edgeQuad.d = AreaUtils.GetExpandedNode(nodes, index, -minNodeDistance, isComplete, isCounterClockwise).xz;
      return edgeQuad;
    }

    public static void ValidateTriangle(
      bool editorMode,
      bool noErrors,
      Entity entity,
      Temp temp,
      Owner owner,
      Triangle triangle,
      ValidationSystem.EntityData data,
      NativeQuadTree<Entity, QuadTreeBoundsXZ> objectSearchTree,
      NativeQuadTree<Entity, QuadTreeBoundsXZ> netSearchTree,
      NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> areaSearchTree,
      WaterSurfaceData waterSurfaceData,
      TerrainHeightData terrainHeightData,
      NativeQueue<ErrorData>.ParallelWriter errorQueue)
    {
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<Node> areaNode = data.m_AreaNodes[entity];
      // ISSUE: reference to a compiler-generated field
      PrefabRef prefabRef = data.m_PrefabRef[entity];
      // ISSUE: reference to a compiler-generated field
      AreaGeometryData areaGeometryData = data.m_PrefabAreaGeometry[prefabRef.m_Prefab];
      Triangle3 triangle3 = AreaUtils.GetTriangle3(areaNode, triangle);
      Bounds3 bounds1 = AreaUtils.GetBounds(triangle, triangle3, areaGeometryData);
      Bounds1 heightRange = triangle.m_HeightRange;
      heightRange.max += areaGeometryData.m_MaxHeight;
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
      Entity entity2 = Entity.Null;
      Entity attached = Entity.Null;
      ErrorSeverity errorSeverity = ErrorSeverity.Error;
      if (noErrors)
      {
        entity2 = entity1;
        // ISSUE: reference to a compiler-generated field
        while (data.m_Owner.HasComponent(entity2))
        {
          // ISSUE: reference to a compiler-generated field
          entity2 = data.m_Owner[entity2].m_Owner;
        }
        Attachment componentData;
        // ISSUE: reference to a compiler-generated field
        if (data.m_Attachment.TryGetComponent(entity2, out componentData))
          attached = componentData.m_Attached;
        errorSeverity = ErrorSeverity.Warning;
      }
      AreaUtils.SetCollisionFlags(ref areaGeometryData, !editorMode || owner.m_Owner != Entity.Null);
      if ((areaGeometryData.m_Flags & GeometryFlags.PhysicalGeometry) != (GeometryFlags) 0)
      {
        CollisionMask collisionMask = AreaUtils.GetCollisionMask(areaGeometryData);
        if ((temp.m_Flags & TempFlags.Delete) == (TempFlags) 0)
        {
          ValidationHelpers.ObjectIterator iterator = new ValidationHelpers.ObjectIterator()
          {
            m_AreaEntity = entity,
            m_OriginalAreaEntity = temp.m_Original,
            m_IgnoreEntity = entity2,
            m_IgnoreEntity2 = attached,
            m_TriangleBounds = bounds1,
            m_HeightRange = heightRange,
            m_Triangle = triangle3,
            m_ErrorSeverity = errorSeverity,
            m_CollisionMask = collisionMask,
            m_PrefabAreaData = areaGeometryData,
            m_Data = data,
            m_ErrorQueue = errorQueue,
            m_EditorMode = editorMode
          };
          objectSearchTree.Iterate<ValidationHelpers.ObjectIterator>(ref iterator);
        }
        if ((temp.m_Flags & TempFlags.Delete) == (TempFlags) 0)
        {
          ValidationHelpers.NetIterator iterator = new ValidationHelpers.NetIterator()
          {
            m_AreaEntity = entity,
            m_OriginalAreaEntity = temp.m_Original,
            m_IgnoreEntity = entity2,
            m_IgnoreEntity2 = attached,
            m_TriangleBounds = bounds1,
            m_HeightRange = heightRange,
            m_Triangle = triangle3,
            m_ErrorSeverity = errorSeverity,
            m_CollisionMask = collisionMask,
            m_Data = data,
            m_ErrorQueue = errorQueue,
            m_EditorMode = editorMode
          };
          netSearchTree.Iterate<ValidationHelpers.NetIterator>(ref iterator);
        }
      }
      if ((areaGeometryData.m_Flags & (GeometryFlags.PhysicalGeometry | GeometryFlags.ProtectedArea)) != (GeometryFlags) 0 || entity1 == entity || !editorMode && ((temp.m_Flags & (TempFlags.Delete | TempFlags.Essential)) == TempFlags.Essential || (temp.m_Flags & TempFlags.Create) != (TempFlags) 0))
      {
        ValidationHelpers.AreaIterator iterator = new ValidationHelpers.AreaIterator()
        {
          m_AreaEntity = entity,
          m_IgnoreEntity = entity2,
          m_IgnoreEntity2 = attached,
          m_TopLevelEntity = entity1,
          m_TriangleBounds = bounds1,
          m_Triangle = AreaUtils.GetTriangle2(areaNode, triangle, areaGeometryData.m_SnapDistance * 0.01f),
          m_IgnoreCollisions = (temp.m_Flags & TempFlags.Delete) > (TempFlags) 0,
          m_EditorMode = editorMode,
          m_Essential = (temp.m_Flags & TempFlags.Essential) > (TempFlags) 0,
          m_PrefabAreaData = areaGeometryData,
          m_ErrorSeverity = errorSeverity,
          m_Data = data,
          m_ErrorQueue = errorQueue
        };
        areaSearchTree.Iterate<ValidationHelpers.AreaIterator>(ref iterator);
      }
      if ((areaGeometryData.m_Flags & (GeometryFlags.PhysicalGeometry | GeometryFlags.OnWaterSurface)) != GeometryFlags.PhysicalGeometry)
        return;
      float sampleInterval = WaterUtils.GetSampleInterval(ref waterSurfaceData);
      int2 int2 = (int2) math.ceil(new float2(math.distance(triangle3.a.xz, triangle3.b.xz), math.distance(triangle3.a.xz, triangle3.c.xz)) / sampleInterval);
      float num1 = 1f / (float) math.max(1, int2.x);
      float num2 = areaGeometryData.m_SnapDistance * 0.01f;
      Bounds3 bounds2;
      bounds2.min = (float3) float.MaxValue;
      bounds2.max = (float3) float.MinValue;
      bool flag = false;
      ValidationHelpers.OriginalAreaIterator iterator1 = new ValidationHelpers.OriginalAreaIterator();
      DynamicBuffer<Node> bufferData;
      // ISSUE: reference to a compiler-generated field
      if (data.m_AreaNodes.TryGetBuffer(temp.m_Original, out bufferData))
      {
        // ISSUE: reference to a compiler-generated field
        iterator1 = new ValidationHelpers.OriginalAreaIterator()
        {
          m_AreaEntity = temp.m_Original,
          m_Offset = num2,
          m_Nodes = bufferData,
          m_Triangles = data.m_AreaTriangles[temp.m_Original]
        };
      }
      for (int index1 = 0; index1 <= int2.x; ++index1)
      {
        float2 t = new float2()
        {
          x = (float) index1 * num1
        };
        int y = ((int2.x - index1) * int2.y + (int2.x >> 1)) / int2.x;
        float num3 = (1f - t.x) / (float) math.max(1, y);
        for (int index2 = 0; index2 <= y; ++index2)
        {
          t.y = (float) index2 * num3;
          float3 worldPosition = MathUtils.Position(triangle3, t);
          if ((double) WaterUtils.SampleDepth(ref waterSurfaceData, worldPosition) >= 0.20000000298023224)
          {
            if (iterator1.m_Nodes.IsCreated)
            {
              iterator1.m_Bounds = new Bounds2(worldPosition.xz - num2, worldPosition.xz + num2);
              iterator1.m_Position = worldPosition.xz;
              iterator1.m_Result = false;
              areaSearchTree.Iterate<ValidationHelpers.OriginalAreaIterator>(ref iterator1);
              if (iterator1.m_Result)
                continue;
            }
            bounds2 |= worldPosition;
            flag = true;
          }
        }
      }
      if (!flag)
        return;
      errorQueue.Enqueue(new ErrorData()
      {
        m_ErrorType = ErrorType.InWater,
        m_ErrorSeverity = ErrorSeverity.Error,
        m_TempEntity = entity,
        m_Position = MathUtils.Center(bounds2)
      });
    }

    private struct OriginalAreaIterator : 
      INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
    {
      public Entity m_AreaEntity;
      public Bounds2 m_Bounds;
      public float2 m_Position;
      public float m_Offset;
      public bool m_Result;
      public DynamicBuffer<Node> m_Nodes;
      public DynamicBuffer<Triangle> m_Triangles;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        return !this.m_Result && MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem areaItem2)
      {
        if (this.m_Result || areaItem2.m_Area != this.m_AreaEntity || !MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds))
          return;
        this.m_Result = MathUtils.Intersect(AreaUtils.GetTriangle2(this.m_Nodes, this.m_Triangles[areaItem2.m_Triangle]), new Circle2(this.m_Offset, this.m_Position));
      }
    }

    private struct ObjectIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public Entity m_AreaEntity;
      public Entity m_OriginalAreaEntity;
      public Entity m_IgnoreEntity;
      public Entity m_IgnoreEntity2;
      public Bounds3 m_TriangleBounds;
      public Bounds1 m_HeightRange;
      public Triangle3 m_Triangle;
      public ErrorSeverity m_ErrorSeverity;
      public CollisionMask m_CollisionMask;
      public AreaGeometryData m_PrefabAreaData;
      public ValidationSystem.EntityData m_Data;
      public NativeQueue<ErrorData>.ParallelWriter m_ErrorQueue;
      public bool m_EditorMode;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        return (bounds.m_Mask & BoundsMask.NotOverridden) != (BoundsMask) 0 && MathUtils.Intersect(bounds.m_Bounds.xz, this.m_TriangleBounds.xz);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity objectEntity2)
      {
        // ISSUE: reference to a compiler-generated field
        if ((bounds.m_Mask & BoundsMask.NotOverridden) == (BoundsMask) 0 || !MathUtils.Intersect(bounds.m_Bounds.xz, this.m_TriangleBounds.xz) || this.m_Data.m_Hidden.HasComponent(objectEntity2) || objectEntity2 == this.m_IgnoreEntity)
          return;
        Entity entity = objectEntity2;
        bool flag = false;
        Owner componentData;
        // ISSUE: reference to a compiler-generated field
        while (this.m_Data.m_Owner.TryGetComponent(entity, out componentData))
        {
          entity = componentData.m_Owner;
          flag = true;
          if (entity == this.m_AreaEntity || entity == this.m_OriginalAreaEntity)
            return;
        }
        if (entity == this.m_IgnoreEntity || entity == this.m_IgnoreEntity2)
          return;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef1 = this.m_Data.m_PrefabRef[objectEntity2];
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_Data.m_Transform[objectEntity2];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Data.m_PrefabObjectGeometry.HasComponent(prefabRef1.m_Prefab))
          return;
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData geometryData = this.m_Data.m_PrefabObjectGeometry[prefabRef1.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        CollisionMask mask1 = !this.m_Data.m_ObjectElevation.HasComponent(objectEntity2) ? ObjectUtils.GetCollisionMask(geometryData, !this.m_EditorMode | flag) : ObjectUtils.GetCollisionMask(geometryData, this.m_Data.m_ObjectElevation[objectEntity2], !this.m_EditorMode | flag);
        if ((this.m_CollisionMask & mask1) == (CollisionMask) 0)
          return;
        ErrorData errorData = new ErrorData();
        errorData.m_ErrorSeverity = this.m_ErrorSeverity;
        errorData.m_TempEntity = this.m_AreaEntity;
        errorData.m_PermanentEntity = objectEntity2;
        if (entity != objectEntity2)
        {
          if ((geometryData.m_Flags & (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.DeleteOverridden)) == Game.Objects.GeometryFlags.Overridable)
          {
            if ((this.m_PrefabAreaData.m_Flags & GeometryFlags.CanOverrideObjects) == (GeometryFlags) 0)
              return;
            errorData.m_ErrorSeverity = ErrorSeverity.Override;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef2 = this.m_Data.m_PrefabRef[entity];
            // ISSUE: reference to a compiler-generated field
            if (this.m_Data.m_PrefabObjectGeometry.HasComponent(prefabRef2.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              ObjectGeometryData objectGeometryData = this.m_Data.m_PrefabObjectGeometry[prefabRef2.m_Prefab];
              if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Overridable) != Game.Objects.GeometryFlags.None)
              {
                if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.DeleteOverridden) == Game.Objects.GeometryFlags.None)
                  return;
                // ISSUE: reference to a compiler-generated field
                if (!this.m_Data.m_Attached.HasComponent(entity))
                {
                  errorData.m_ErrorSeverity = ErrorSeverity.Warning;
                  errorData.m_PermanentEntity = entity;
                }
              }
            }
          }
        }
        else if ((geometryData.m_Flags & Game.Objects.GeometryFlags.Overridable) != Game.Objects.GeometryFlags.None)
        {
          if ((geometryData.m_Flags & Game.Objects.GeometryFlags.DeleteOverridden) != Game.Objects.GeometryFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Data.m_Attached.HasComponent(objectEntity2))
              errorData.m_ErrorSeverity = ErrorSeverity.Warning;
          }
          else
          {
            if ((this.m_PrefabAreaData.m_Flags & GeometryFlags.CanOverrideObjects) == (GeometryFlags) 0)
              return;
            errorData.m_ErrorSeverity = ErrorSeverity.Override;
          }
        }
        if ((mask1 & CollisionMask.OnGround) == (CollisionMask) 0 || MathUtils.Intersect(this.m_TriangleBounds, bounds.m_Bounds))
        {
          float3 float3 = math.mul(math.inverse(transform.m_Rotation), transform.m_Position);
          Bounds3 bounds1 = geometryData.m_Bounds;
          if ((geometryData.m_Flags & Game.Objects.GeometryFlags.IgnoreBottomCollision) != Game.Objects.GeometryFlags.None)
            bounds1.min.y = math.max(bounds1.min.y, 0.0f);
          if ((geometryData.m_Flags & Game.Objects.GeometryFlags.Standing) != Game.Objects.GeometryFlags.None)
          {
            if ((geometryData.m_Flags & (Game.Objects.GeometryFlags.CircularLeg | Game.Objects.GeometryFlags.IgnoreLegCollision)) == Game.Objects.GeometryFlags.CircularLeg)
            {
              Bounds3 intersection1;
              Bounds3 intersection2;
              if (Game.Net.ValidationHelpers.TriangleCylinderIntersect(this.m_Triangle, new Cylinder3()
              {
                circle = new Circle2(geometryData.m_LegSize.x * 0.5f, float3.xz),
                height = new Bounds1(bounds1.min.y, geometryData.m_LegSize.y) + float3.y,
                rotation = transform.m_Rotation
              }, out intersection1, out intersection2))
              {
                intersection1 = Game.Net.ValidationHelpers.SetHeightRange(intersection1, this.m_HeightRange);
                Bounds3 intersection;
                if (MathUtils.Intersect(intersection1, intersection2, out intersection))
                {
                  errorData.m_Position = MathUtils.Center(intersection);
                  errorData.m_ErrorType = ErrorType.OverlapExisting;
                }
              }
            }
            else if ((geometryData.m_Flags & Game.Objects.GeometryFlags.IgnoreLegCollision) == Game.Objects.GeometryFlags.None)
            {
              bounds1.min.xz = geometryData.m_LegSize.xz * -0.5f;
              bounds1.max.xz = geometryData.m_LegSize.xz * 0.5f;
              Bounds3 intersection1;
              Bounds3 intersection2;
              Bounds3 intersection;
              if (Game.Net.ValidationHelpers.QuadTriangleIntersect(ObjectUtils.CalculateBaseCorners(transform.m_Position, transform.m_Rotation, bounds1), this.m_Triangle, out intersection1, out intersection2) && MathUtils.Intersect(Game.Net.ValidationHelpers.SetHeightRange(intersection2, this.m_HeightRange), Game.Net.ValidationHelpers.SetHeightRange(intersection1, bounds1.y), out intersection))
              {
                errorData.m_Position = MathUtils.Center(intersection);
                errorData.m_ErrorType = ErrorType.OverlapExisting;
              }
            }
            bounds1.min.y = geometryData.m_LegSize.y;
          }
          if ((geometryData.m_Flags & Game.Objects.GeometryFlags.Circular) != Game.Objects.GeometryFlags.None)
          {
            Bounds3 intersection1;
            Bounds3 intersection2;
            if (Game.Net.ValidationHelpers.TriangleCylinderIntersect(this.m_Triangle, new Cylinder3()
            {
              circle = new Circle2(geometryData.m_Size.x * 0.5f, float3.xz),
              height = new Bounds1(bounds1.min.y, bounds1.max.y) + float3.y,
              rotation = transform.m_Rotation
            }, out intersection1, out intersection2))
            {
              intersection1 = Game.Net.ValidationHelpers.SetHeightRange(intersection1, this.m_HeightRange);
              Bounds3 intersection;
              if (MathUtils.Intersect(intersection1, intersection2, out intersection))
              {
                errorData.m_Position = MathUtils.Center(intersection);
                errorData.m_ErrorType = ErrorType.OverlapExisting;
              }
            }
          }
          else
          {
            Bounds3 intersection1;
            Bounds3 intersection2;
            Bounds3 intersection;
            if (Game.Net.ValidationHelpers.QuadTriangleIntersect(ObjectUtils.CalculateBaseCorners(transform.m_Position, transform.m_Rotation, geometryData.m_Bounds), this.m_Triangle, out intersection1, out intersection2) && MathUtils.Intersect(Game.Net.ValidationHelpers.SetHeightRange(intersection2, this.m_HeightRange), Game.Net.ValidationHelpers.SetHeightRange(intersection1, bounds1.y), out intersection))
            {
              errorData.m_Position = MathUtils.Center(intersection);
              errorData.m_ErrorType = ErrorType.OverlapExisting;
            }
          }
        }
        float2 t;
        if (errorData.m_ErrorType == ErrorType.None && CommonUtils.ExclusiveGroundCollision(mask1, this.m_CollisionMask))
        {
          if ((geometryData.m_Flags & Game.Objects.GeometryFlags.Standing) != Game.Objects.GeometryFlags.None)
          {
            if ((geometryData.m_Flags & (Game.Objects.GeometryFlags.CircularLeg | Game.Objects.GeometryFlags.IgnoreLegCollision)) == Game.Objects.GeometryFlags.CircularLeg)
            {
              Circle2 circle = new Circle2(geometryData.m_LegSize.x * 0.5f, transform.m_Position.xz);
              if (!this.m_Triangle.c.Equals(this.m_Triangle.b) ? MathUtils.Intersect(this.m_Triangle.xz, circle) : MathUtils.Intersect(circle, this.m_Triangle.xz.ab, out t))
              {
                errorData.m_Position = MathUtils.Center(bounds.m_Bounds & this.m_TriangleBounds);
                errorData.m_ErrorType = ErrorType.OverlapExisting;
              }
            }
            else if ((geometryData.m_Flags & Game.Objects.GeometryFlags.IgnoreLegCollision) == Game.Objects.GeometryFlags.None)
            {
              Quad2 xz = ObjectUtils.CalculateBaseCorners(transform.m_Position, transform.m_Rotation, new Bounds3()
              {
                min = {
                  xz = geometryData.m_LegSize.xz * -0.5f
                },
                max = {
                  xz = geometryData.m_LegSize.xz * 0.5f
                }
              }).xz;
              if (!this.m_Triangle.c.Equals(this.m_Triangle.b) ? MathUtils.Intersect(xz, this.m_Triangle.xz) : MathUtils.Intersect(xz, this.m_Triangle.xz.ab, out t))
              {
                errorData.m_Position = MathUtils.Center(bounds.m_Bounds & this.m_TriangleBounds);
                errorData.m_ErrorType = ErrorType.OverlapExisting;
              }
            }
          }
          else if ((geometryData.m_Flags & Game.Objects.GeometryFlags.Circular) != Game.Objects.GeometryFlags.None)
          {
            Circle2 circle = new Circle2(geometryData.m_Size.x * 0.5f, transform.m_Position.xz);
            if (!this.m_Triangle.c.Equals(this.m_Triangle.b) ? MathUtils.Intersect(this.m_Triangle.xz, circle) : MathUtils.Intersect(circle, this.m_Triangle.xz.ab, out t))
            {
              errorData.m_Position = MathUtils.Center(bounds.m_Bounds & this.m_TriangleBounds);
              errorData.m_ErrorType = ErrorType.OverlapExisting;
            }
          }
          else
          {
            Quad2 xz = ObjectUtils.CalculateBaseCorners(transform.m_Position, transform.m_Rotation, geometryData.m_Bounds).xz;
            if (!this.m_Triangle.c.Equals(this.m_Triangle.b) ? MathUtils.Intersect(xz, this.m_Triangle.xz) : MathUtils.Intersect(xz, this.m_Triangle.xz.ab, out t))
            {
              errorData.m_Position = MathUtils.Center(bounds.m_Bounds & this.m_TriangleBounds);
              errorData.m_ErrorType = ErrorType.OverlapExisting;
            }
          }
        }
        if (errorData.m_ErrorType == ErrorType.None)
          return;
        // ISSUE: reference to a compiler-generated field
        if ((errorData.m_ErrorSeverity == ErrorSeverity.Override || errorData.m_ErrorSeverity == ErrorSeverity.Warning) && errorData.m_ErrorType == ErrorType.OverlapExisting && this.m_Data.m_OnFire.HasComponent(errorData.m_PermanentEntity))
        {
          errorData.m_ErrorType = ErrorType.OnFire;
          errorData.m_ErrorSeverity = ErrorSeverity.Error;
        }
        this.m_ErrorQueue.Enqueue(errorData);
      }
    }

    private struct NetIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public Entity m_AreaEntity;
      public Entity m_OriginalAreaEntity;
      public Entity m_IgnoreEntity;
      public Entity m_IgnoreEntity2;
      public Bounds3 m_TriangleBounds;
      public Bounds1 m_HeightRange;
      public Triangle3 m_Triangle;
      public ErrorSeverity m_ErrorSeverity;
      public CollisionMask m_CollisionMask;
      public ValidationSystem.EntityData m_Data;
      public NativeQueue<ErrorData>.ParallelWriter m_ErrorQueue;
      public bool m_EditorMode;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_TriangleBounds.xz);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity edgeEntity2)
      {
        // ISSUE: reference to a compiler-generated field
        if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_TriangleBounds.xz) || this.m_Data.m_Hidden.HasComponent(edgeEntity2))
          return;
        Entity entity = edgeEntity2;
        bool flag1 = false;
        Owner componentData;
        // ISSUE: reference to a compiler-generated field
        while (this.m_Data.m_Owner.TryGetComponent(entity, out componentData))
        {
          entity = componentData.m_Owner;
          flag1 = true;
          if (entity == this.m_AreaEntity || entity == this.m_OriginalAreaEntity)
            return;
        }
        // ISSUE: reference to a compiler-generated field
        if (entity == this.m_IgnoreEntity || entity == this.m_IgnoreEntity2 || !this.m_Data.m_Composition.HasComponent(edgeEntity2))
          return;
        // ISSUE: reference to a compiler-generated field
        Edge edge1_1 = this.m_Data.m_Edge[edgeEntity2];
        // ISSUE: reference to a compiler-generated field
        Composition composition = this.m_Data.m_Composition[edgeEntity2];
        // ISSUE: reference to a compiler-generated field
        EdgeGeometry edgeGeometry1_1 = this.m_Data.m_EdgeGeometry[edgeEntity2];
        // ISSUE: reference to a compiler-generated field
        StartNodeGeometry startNodeGeometry = this.m_Data.m_StartNodeGeometry[edgeEntity2];
        // ISSUE: reference to a compiler-generated field
        EndNodeGeometry endNodeGeometry = this.m_Data.m_EndNodeGeometry[edgeEntity2];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData1 = this.m_Data.m_PrefabComposition[composition.m_Edge];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData2 = this.m_Data.m_PrefabComposition[composition.m_StartNode];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData3 = this.m_Data.m_PrefabComposition[composition.m_EndNode];
        CollisionMask collisionMask1 = NetUtils.GetCollisionMask(netCompositionData1, !this.m_EditorMode | flag1);
        CollisionMask collisionMask2 = NetUtils.GetCollisionMask(netCompositionData2, !this.m_EditorMode | flag1);
        CollisionMask collisionMask3 = NetUtils.GetCollisionMask(netCompositionData3, !this.m_EditorMode | flag1);
        CollisionMask mask1 = collisionMask1 | collisionMask2 | collisionMask3;
        if ((this.m_CollisionMask & mask1) == (CollisionMask) 0)
          return;
        ErrorData errorData = new ErrorData();
        Bounds3 intersection;
        intersection.min = (float3) float.MaxValue;
        intersection.max = (float3) float.MinValue;
        bool flag2 = this.m_Triangle.c.Equals(this.m_Triangle.b);
        if ((mask1 & CollisionMask.OnGround) == (CollisionMask) 0 || MathUtils.Intersect(this.m_TriangleBounds, bounds.m_Bounds))
        {
          if ((collisionMask1 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edge1_1, this.m_AreaEntity, edgeGeometry1_1, this.m_Triangle, netCompositionData1, this.m_HeightRange, ref intersection))
            errorData.m_ErrorType = ErrorType.OverlapExisting;
          if ((collisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edge1_1.m_Start, this.m_AreaEntity, startNodeGeometry.m_Geometry, this.m_Triangle, netCompositionData2, this.m_HeightRange, ref intersection))
            errorData.m_ErrorType = ErrorType.OverlapExisting;
          if ((collisionMask3 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edge1_1.m_End, this.m_AreaEntity, endNodeGeometry.m_Geometry, this.m_Triangle, netCompositionData3, this.m_HeightRange, ref intersection))
            errorData.m_ErrorType = ErrorType.OverlapExisting;
        }
        if (errorData.m_ErrorType == ErrorType.None && CommonUtils.ExclusiveGroundCollision(mask1, this.m_CollisionMask))
        {
          Triangle2 xz;
          if ((collisionMask1 & this.m_CollisionMask) != (CollisionMask) 0)
          {
            if (flag2)
            {
              Edge edge1_2 = edge1_1;
              Entity areaEntity = this.m_AreaEntity;
              EdgeGeometry edgeGeometry1_2 = edgeGeometry1_1;
              xz = this.m_Triangle.xz;
              Line2.Segment ab = xz.ab;
              Bounds3 triangleBounds = this.m_TriangleBounds;
              NetCompositionData prefabCompositionData1 = netCompositionData1;
              ref Bounds3 local = ref intersection;
              if (Game.Net.ValidationHelpers.Intersect(edge1_2, areaEntity, edgeGeometry1_2, ab, triangleBounds, prefabCompositionData1, ref local))
                errorData.m_ErrorType = ErrorType.OverlapExisting;
            }
            else if (Game.Net.ValidationHelpers.Intersect(edge1_1, this.m_AreaEntity, edgeGeometry1_1, this.m_Triangle.xz, this.m_TriangleBounds, netCompositionData1, ref intersection))
              errorData.m_ErrorType = ErrorType.OverlapExisting;
          }
          if ((collisionMask2 & this.m_CollisionMask) != (CollisionMask) 0)
          {
            if (flag2)
            {
              Entity start = edge1_1.m_Start;
              Entity areaEntity = this.m_AreaEntity;
              EdgeNodeGeometry geometry = startNodeGeometry.m_Geometry;
              xz = this.m_Triangle.xz;
              Line2.Segment ab = xz.ab;
              Bounds3 triangleBounds = this.m_TriangleBounds;
              NetCompositionData prefabCompositionData1 = netCompositionData2;
              ref Bounds3 local = ref intersection;
              if (Game.Net.ValidationHelpers.Intersect(start, areaEntity, geometry, ab, triangleBounds, prefabCompositionData1, ref local))
                errorData.m_ErrorType = ErrorType.OverlapExisting;
            }
            else if (Game.Net.ValidationHelpers.Intersect(edge1_1.m_Start, this.m_AreaEntity, startNodeGeometry.m_Geometry, this.m_Triangle.xz, this.m_TriangleBounds, netCompositionData2, ref intersection))
              errorData.m_ErrorType = ErrorType.OverlapExisting;
          }
          if ((collisionMask3 & this.m_CollisionMask) != (CollisionMask) 0)
          {
            if (flag2)
            {
              Entity end = edge1_1.m_End;
              Entity areaEntity = this.m_AreaEntity;
              EdgeNodeGeometry geometry = endNodeGeometry.m_Geometry;
              xz = this.m_Triangle.xz;
              Line2.Segment ab = xz.ab;
              Bounds3 triangleBounds = this.m_TriangleBounds;
              NetCompositionData prefabCompositionData1 = netCompositionData3;
              ref Bounds3 local = ref intersection;
              if (Game.Net.ValidationHelpers.Intersect(end, areaEntity, geometry, ab, triangleBounds, prefabCompositionData1, ref local))
                errorData.m_ErrorType = ErrorType.OverlapExisting;
            }
            else if (Game.Net.ValidationHelpers.Intersect(edge1_1.m_End, this.m_AreaEntity, endNodeGeometry.m_Geometry, this.m_Triangle.xz, this.m_TriangleBounds, netCompositionData3, ref intersection))
              errorData.m_ErrorType = ErrorType.OverlapExisting;
          }
        }
        if (errorData.m_ErrorType == ErrorType.None)
          return;
        errorData.m_ErrorSeverity = this.m_ErrorSeverity;
        errorData.m_TempEntity = this.m_AreaEntity;
        errorData.m_PermanentEntity = edgeEntity2;
        errorData.m_Position = MathUtils.Center(intersection);
        this.m_ErrorQueue.Enqueue(errorData);
      }
    }

    private struct AreaIterator : 
      INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
    {
      public Entity m_AreaEntity;
      public Entity m_IgnoreEntity;
      public Entity m_IgnoreEntity2;
      public Entity m_TopLevelEntity;
      public Bounds3 m_TriangleBounds;
      public Triangle2 m_Triangle;
      public bool m_IgnoreCollisions;
      public bool m_EditorMode;
      public bool m_Essential;
      public AreaGeometryData m_PrefabAreaData;
      public ErrorSeverity m_ErrorSeverity;
      public ValidationSystem.EntityData m_Data;
      public NativeQueue<ErrorData>.ParallelWriter m_ErrorQueue;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_TriangleBounds.xz);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem areaItem2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_TriangleBounds.xz) || this.m_Data.m_Hidden.HasComponent(areaItem2.m_Area) || (this.m_Data.m_Area[areaItem2.m_Area].m_Flags & AreaFlags.Slave) != (AreaFlags) 0)
          return;
        Entity entity1 = areaItem2.m_Area;
        bool flag = false;
        Owner componentData;
        Entity owner;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (; this.m_Data.m_Owner.TryGetComponent(entity1, out componentData) && !this.m_Data.m_Building.HasComponent(entity1); entity1 = owner)
        {
          owner = componentData.m_Owner;
          flag = true;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Data.m_AssetStamp.HasComponent(owner))
            break;
        }
        if (entity1 == this.m_TopLevelEntity)
          return;
        if (this.m_IgnoreEntity != Entity.Null)
        {
          Entity entity2 = entity1;
          // ISSUE: reference to a compiler-generated field
          while (this.m_Data.m_Owner.HasComponent(entity2))
          {
            // ISSUE: reference to a compiler-generated field
            entity2 = this.m_Data.m_Owner[entity2].m_Owner;
          }
          if (entity2 == this.m_IgnoreEntity || entity2 == this.m_IgnoreEntity2)
            return;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        AreaGeometryData areaGeometryData = this.m_Data.m_PrefabAreaGeometry[this.m_Data.m_PrefabRef[areaItem2.m_Area].m_Prefab];
        AreaUtils.SetCollisionFlags(ref areaGeometryData, !this.m_EditorMode | flag);
        if (areaGeometryData.m_Type != this.m_PrefabAreaData.m_Type)
        {
          if ((areaGeometryData.m_Flags & (GeometryFlags.PhysicalGeometry | GeometryFlags.ProtectedArea)) == (GeometryFlags) 0)
            return;
          if ((areaGeometryData.m_Flags & GeometryFlags.ProtectedArea) != (GeometryFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Data.m_Native.HasComponent(areaItem2.m_Area))
              return;
          }
          else if (this.m_IgnoreCollisions || (areaGeometryData.m_Flags & GeometryFlags.PhysicalGeometry) != (GeometryFlags) 0 && (this.m_PrefabAreaData.m_Flags & GeometryFlags.PhysicalGeometry) == (GeometryFlags) 0)
            return;
        }
        else if ((areaGeometryData.m_Flags & (GeometryFlags.PhysicalGeometry | GeometryFlags.ProtectedArea)) == (GeometryFlags) 0 && entity1 != areaItem2.m_Area && this.m_TopLevelEntity != this.m_AreaEntity && (this.m_EditorMode || this.m_IgnoreCollisions || !this.m_Essential))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Triangle2 triangle2 = AreaUtils.GetTriangle2(this.m_Data.m_AreaNodes[areaItem2.m_Area], this.m_Data.m_AreaTriangles[areaItem2.m_Area][areaItem2.m_Triangle], areaGeometryData.m_SnapDistance * 0.01f);
        if (!(!this.m_Triangle.c.Equals(this.m_Triangle.b) ? MathUtils.Intersect(this.m_Triangle, triangle2) : MathUtils.Intersect(triangle2, this.m_Triangle.ab, out float2 _)))
          return;
        ErrorData errorData = new ErrorData()
        {
          m_ErrorSeverity = this.m_ErrorSeverity,
          m_ErrorType = areaGeometryData.m_Type != AreaType.MapTile || this.m_EditorMode ? ErrorType.OverlapExisting : ErrorType.ExceedsCityLimits,
          m_TempEntity = this.m_AreaEntity,
          m_PermanentEntity = areaItem2.m_Area,
          m_Position = MathUtils.Center(bounds.m_Bounds & this.m_TriangleBounds)
        };
        errorData.m_Position.y = MathUtils.Clamp(errorData.m_Position.y, this.m_TriangleBounds.y);
        if (errorData.m_ErrorType == ErrorType.OverlapExisting)
        {
          if (entity1 != areaItem2.m_Area && entity1 != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_Data.m_PrefabRef[entity1];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Data.m_PrefabObjectGeometry.HasComponent(prefabRef.m_Prefab) && (this.m_Data.m_PrefabObjectGeometry[prefabRef.m_Prefab].m_Flags & (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.DeleteOverridden)) == (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.DeleteOverridden) && !this.m_Data.m_Attached.HasComponent(entity1) && (!this.m_Data.m_Temp.HasComponent(entity1) || (this.m_Data.m_Temp[entity1].m_Flags & TempFlags.Essential) == (TempFlags) 0))
            {
              errorData.m_ErrorSeverity = ErrorSeverity.Warning;
              errorData.m_PermanentEntity = entity1;
            }
          }
          if (!this.m_Essential && this.m_TopLevelEntity != this.m_AreaEntity && this.m_TopLevelEntity != Entity.Null)
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
              errorData.m_TempEntity = areaItem2.m_Area;
              errorData.m_PermanentEntity = this.m_TopLevelEntity;
            }
          }
        }
        this.m_ErrorQueue.Enqueue(errorData);
      }
    }

    public struct BrushAreaIterator : 
      INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
    {
      public Entity m_BrushEntity;
      public Brush m_Brush;
      public Bounds3 m_BrushBounds;
      public ValidationSystem.EntityData m_Data;
      public NativeQueue<ErrorData>.ParallelWriter m_ErrorQueue;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_BrushBounds.xz);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem areaItem2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_BrushBounds.xz) || this.m_Data.m_Hidden.HasComponent(areaItem2.m_Area) || (this.m_Data.m_Area[areaItem2.m_Area].m_Flags & AreaFlags.Slave) != (AreaFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        AreaGeometryData areaGeometryData = this.m_Data.m_PrefabAreaGeometry[this.m_Data.m_PrefabRef[areaItem2.m_Area].m_Prefab];
        // ISSUE: reference to a compiler-generated field
        if ((areaGeometryData.m_Flags & GeometryFlags.ProtectedArea) == (GeometryFlags) 0 || !this.m_Data.m_Native.HasComponent(areaItem2.m_Area))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Triangle3 triangle3 = AreaUtils.GetTriangle3(this.m_Data.m_AreaNodes[areaItem2.m_Area], this.m_Data.m_AreaTriangles[areaItem2.m_Area][areaItem2.m_Triangle]);
        ErrorData errorData = new ErrorData();
        if (areaGeometryData.m_Type == AreaType.MapTile)
        {
          Circle2 circle = new Circle2(this.m_Brush.m_Size * 0.4f, this.m_Brush.m_Position.xz);
          if (MathUtils.Intersect(triangle3.xz, circle))
          {
            errorData.m_Position = MathUtils.Center(this.m_BrushBounds & bounds.m_Bounds);
            errorData.m_ErrorType = ErrorType.ExceedsCityLimits;
          }
        }
        if (errorData.m_ErrorType == ErrorType.None)
          return;
        errorData.m_Position.y = MathUtils.Clamp(errorData.m_Position.y, this.m_BrushBounds.y);
        errorData.m_ErrorSeverity = ErrorSeverity.Error;
        errorData.m_TempEntity = this.m_BrushEntity;
        errorData.m_PermanentEntity = areaItem2.m_Area;
        this.m_ErrorQueue.Enqueue(errorData);
      }
    }
  }
}
