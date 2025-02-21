// Decompiled with JetBrains decompiler
// Type: Game.Areas.AreaUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Simulation;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Areas
{
  public static class AreaUtils
  {
    public const float NODE_DISTANCE_TOLERANCE = 0.1f;

    public static float GetMinNodeDistance(AreaGeometryData areaData)
    {
      return areaData.m_SnapDistance * 0.5f;
    }

    public static Triangle3 GetTriangle3(DynamicBuffer<Node> nodes, Triangle triangle)
    {
      return new Triangle3(nodes[triangle.m_Indices.x].m_Position, nodes[triangle.m_Indices.y].m_Position, nodes[triangle.m_Indices.z].m_Position);
    }

    public static float3 GetElevations(DynamicBuffer<Node> nodes, Triangle triangle)
    {
      return new float3(nodes[triangle.m_Indices.x].m_Elevation, nodes[triangle.m_Indices.y].m_Elevation, nodes[triangle.m_Indices.z].m_Elevation);
    }

    public static Bounds3 GetBounds(
      Triangle triangle,
      Triangle3 triangle3,
      AreaGeometryData areaData)
    {
      Bounds3 bounds = MathUtils.Bounds(triangle3);
      bounds.min.y += triangle.m_HeightRange.min;
      bounds.max.y += triangle.m_HeightRange.max + areaData.m_MaxHeight;
      return bounds;
    }

    public static int CalculateStorageCapacity(Geometry geometry, StorageAreaData prefabStorageData)
    {
      return Mathf.RoundToInt(geometry.m_SurfaceArea * (1f / 64f) * (float) prefabStorageData.m_Capacity);
    }

    public static float CalculateStorageObjectArea(
      Geometry geometry,
      Storage storage,
      StorageAreaData prefabStorageData)
    {
      float y = geometry.m_SurfaceArea * (1f / 64f) * (float) prefabStorageData.m_Capacity;
      return math.min(0.25f, math.sqrt((float) storage.m_Amount / math.max(1f, y))) * geometry.m_SurfaceArea;
    }

    public static float CalculateExtractorObjectArea(
      Geometry geometry,
      Extractor extractor,
      ExtractorAreaData extractorAreaData)
    {
      return math.min(extractor.m_TotalExtracted * extractorAreaData.m_ObjectSpawnFactor, geometry.m_SurfaceArea * extractorAreaData.m_MaxObjectArea);
    }

    public static Triangle2 GetTriangle2(DynamicBuffer<Node> nodes, Triangle triangle)
    {
      Node node = nodes[triangle.m_Indices.x];
      float2 xz1 = node.m_Position.xz;
      node = nodes[triangle.m_Indices.y];
      float2 xz2 = node.m_Position.xz;
      node = nodes[triangle.m_Indices.z];
      float2 xz3 = node.m_Position.xz;
      return new Triangle2(xz1, xz2, xz3);
    }

    public static Triangle2 GetTriangle2(
      DynamicBuffer<Node> nodes,
      Triangle triangle,
      float edgeShrink)
    {
      Triangle2 triangle2 = AreaUtils.GetTriangle2(nodes, triangle);
      bool3 bool3 = AreaUtils.IsEdge(nodes, triangle);
      float2 float2_1 = math.normalizesafe(triangle2.b - triangle2.a) * edgeShrink;
      float2 float2_2 = math.normalizesafe(triangle2.c - triangle2.b) * edgeShrink;
      float2 float2_3 = math.normalizesafe(triangle2.a - triangle2.c) * edgeShrink;
      if (bool3.x)
      {
        triangle2.a -= float2_3;
        triangle2.b += float2_2;
      }
      if (bool3.y)
      {
        triangle2.b -= float2_1;
        triangle2.c += float2_3;
      }
      if (bool3.z)
      {
        triangle2.c -= float2_2;
        triangle2.a += float2_1;
      }
      return triangle2;
    }

    public static bool3 IsEdge(DynamicBuffer<Node> nodes, Triangle triangle)
    {
      int3 int3 = math.abs(triangle.m_Indices - triangle.m_Indices.yzx);
      return int3 == 1 | int3 == nodes.Length - 1;
    }

    public static quaternion CalculateLabelRotation(float3 cameraRight)
    {
      return quaternion.LookRotation(new float3(0.0f, -1f, 0.0f), math.cross(cameraRight, math.up()));
    }

    public static float3 CalculateLabelPosition(Geometry geometry) => geometry.m_CenterPosition;

    public static float CalculateLabelScale(float3 cameraPosition, float3 labelPosition)
    {
      return math.max(0.01f, math.sqrt(math.distance(cameraPosition, labelPosition) * (1f / 1000f)));
    }

    public static float4x4 CalculateLabelMatrix(
      float3 cameraPosition,
      float3 labelPosition,
      quaternion labelRotation)
    {
      float labelScale = AreaUtils.CalculateLabelScale(cameraPosition, labelPosition);
      return float4x4.TRS(labelPosition, labelRotation, (float3) labelScale);
    }

    public static bool CheckOption(District district, DistrictOption option)
    {
      return (district.m_OptionMask & 1U << (int) (option & (DistrictOption) 31)) > 0U;
    }

    public static void ApplyModifier(
      ref float value,
      DynamicBuffer<DistrictModifier> modifiers,
      DistrictModifierType type)
    {
      if ((DistrictModifierType) modifiers.Length <= type)
        return;
      float2 delta = modifiers[(int) type].m_Delta;
      value += delta.x;
      value += value * delta.y;
    }

    public static bool HasOption(DistrictOptionData optionData, DistrictOption option)
    {
      return (optionData.m_OptionMask & 1U << (int) (option & (DistrictOption) 31)) > 0U;
    }

    public static bool CheckServiceDistrict(
      Entity district,
      Entity service,
      BufferLookup<ServiceDistrict> serviceDistricts)
    {
      DynamicBuffer<ServiceDistrict> bufferData;
      if (!serviceDistricts.TryGetBuffer(service, out bufferData) || bufferData.Length == 0)
        return true;
      return !(district == Entity.Null) && CollectionUtils.ContainsValue<ServiceDistrict>(bufferData, new ServiceDistrict(district));
    }

    public static bool CheckServiceDistrict(
      Entity district1,
      Entity district2,
      Entity service,
      BufferLookup<ServiceDistrict> serviceDistricts)
    {
      if (!serviceDistricts.HasBuffer(service))
        return true;
      DynamicBuffer<ServiceDistrict> serviceDistrict = serviceDistricts[service];
      if (serviceDistrict.Length == 0)
        return true;
      if (district1 == Entity.Null && district2 == Entity.Null)
        return false;
      return CollectionUtils.ContainsValue<ServiceDistrict>(serviceDistrict, new ServiceDistrict(district1)) || CollectionUtils.ContainsValue<ServiceDistrict>(serviceDistrict, new ServiceDistrict(district2));
    }

    public static bool CheckServiceDistrict(
      Entity building,
      DynamicBuffer<ServiceDistrict> serviceDistricts,
      ref ComponentLookup<CurrentDistrict> currentDistricts)
    {
      CurrentDistrict componentData;
      return !serviceDistricts.IsCreated || serviceDistricts.Length == 0 || !currentDistricts.TryGetComponent(building, out componentData) || CollectionUtils.ContainsValue<ServiceDistrict>(serviceDistricts, new ServiceDistrict(componentData.m_District));
    }

    public static CollisionMask GetCollisionMask(AreaGeometryData areaGeometryData)
    {
      CollisionMask collisionMask = CollisionMask.OnGround | CollisionMask.Overground | CollisionMask.ExclusiveGround;
      if (areaGeometryData.m_Type != AreaType.Lot)
        collisionMask |= CollisionMask.Underground;
      return collisionMask;
    }

    public static bool TryGetRandomObjectLocation(
      ref Unity.Mathematics.Random random,
      ObjectGeometryData objectGeometryData,
      Area area,
      Geometry geometry,
      float extraRadius,
      DynamicBuffer<Node> nodes,
      DynamicBuffer<Triangle> triangles,
      NativeList<AreaUtils.ObjectItem> objects,
      out Game.Objects.Transform transform)
    {
      transform.m_Position = AreaUtils.GetRandomPosition(ref random, geometry, nodes, triangles);
      float radius = (objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Circular) == Game.Objects.GeometryFlags.None ? math.length(MathUtils.Size(objectGeometryData.m_Bounds.xz)) * 0.5f : objectGeometryData.m_Size.x * 0.5f;
      bool randomObjectLocation = AreaUtils.TryFitInside(ref transform.m_Position, radius, extraRadius, area, nodes, objects, true);
      if (objects.IsCreated)
      {
        float num1 = (float) (((double) radius + (double) extraRadius) * 0.5);
        int index1 = 0;
        for (int index2 = 0; index2 < objects.Length; ++index2)
        {
          AreaUtils.ObjectItem objectItem = objects[index2];
          if ((double) objectItem.m_Circle.radius < (double) num1)
          {
            float num2 = radius + objectItem.m_Circle.radius;
            if ((double) math.distancesq(transform.m_Position.xz, objectItem.m_Circle.position) < (double) num2 * (double) num2)
              objects[index1++] = objectItem;
          }
        }
        if (index1 < objects.Length)
          objects.RemoveRange(index1, objects.Length - index1);
      }
      transform.m_Rotation = AreaUtils.GetRandomRotation(ref random, transform.m_Position, nodes);
      if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Circular) == Game.Objects.GeometryFlags.None)
        transform.m_Position.xz -= math.rotate(transform.m_Rotation, MathUtils.Center(objectGeometryData.m_Bounds)).xz;
      return randomObjectLocation;
    }

    public static float3 GetRandomPosition(
      ref Unity.Mathematics.Random random,
      Geometry geometry,
      DynamicBuffer<Node> nodes,
      DynamicBuffer<Triangle> triangles)
    {
      float num = random.NextFloat(geometry.m_SurfaceArea);
      for (int index = 0; index < triangles.Length; ++index)
      {
        Triangle3 triangle3 = AreaUtils.GetTriangle3(nodes, triangles[index]);
        num -= MathUtils.Area(triangle3.xz);
        if ((double) num <= 0.0)
        {
          float2 float2 = random.NextFloat2((float2) 1f);
          float2 t = math.select(float2, 1f - float2, (double) math.csum(float2) > 1.0);
          return MathUtils.Position(triangle3, t);
        }
      }
      if (nodes.Length >= 2)
        return math.lerp(nodes[0].m_Position, nodes[1].m_Position, random.NextFloat(1f));
      return nodes.Length == 1 ? nodes[0].m_Position : new float3();
    }

    public static quaternion GetRandomRotation(
      ref Unity.Mathematics.Random random,
      float3 position,
      DynamicBuffer<Node> nodes)
    {
      float2 float2 = new float2();
      float num1 = float.MaxValue;
      Line2.Segment line;
      line.a = nodes[nodes.Length - 1].m_Position.xz;
      for (int index = 0; index < nodes.Length; ++index)
      {
        line.b = nodes[index].m_Position.xz;
        float num2 = MathUtils.DistanceSquared(line, position.xz, out float _);
        if ((double) num2 < (double) num1)
        {
          float2 = line.b - line.a;
          num1 = num2;
        }
        line.a = line.b;
      }
      return quaternion.RotateY(!MathUtils.TryNormalize(ref float2) ? random.NextFloat(6.28318548f) : math.atan2(float2.x, float2.y) + (float) random.NextInt(4) * 1.57079637f);
    }

    public static bool TryFitInside(
      ref float3 position,
      float radius,
      float extraRadius,
      Area area,
      DynamicBuffer<Node> nodes,
      NativeList<AreaUtils.ObjectItem> objects,
      bool canOverride = false)
    {
      float num1 = radius + extraRadius;
      float num2 = num1 * num1;
      bool flag = false;
      Line2.Segment line;
      line.a = nodes[nodes.Length - 1].m_Position.xz;
      for (int index = 0; index < nodes.Length; ++index)
      {
        line.b = nodes[index].m_Position.xz;
        float t;
        if ((double) MathUtils.DistanceSquared(line, position.xz, out t) < (double) num2)
        {
          float2 float2_1 = math.normalizesafe(line.b - line.a);
          float2 y = position.xz - MathUtils.Position(line, t);
          float num3 = math.dot(float2_1, y);
          float2 x = (area.m_Flags & AreaFlags.CounterClockwise) == (AreaFlags) 0 ? MathUtils.Right(float2_1) : MathUtils.Left(float2_1);
          float2 float2_2 = x * (float) ((double) math.sqrt(num2 - num3 * num3) - (double) math.dot(x, y) + 0.0099999997764825821);
          position.xz += float2_2;
          flag = true;
        }
        line.a = line.b;
      }
      if (objects.IsCreated)
      {
        float num4 = (float) (((double) radius + (double) extraRadius) * 0.5);
        for (int index = 0; index < objects.Length; ++index)
        {
          AreaUtils.ObjectItem objectItem = objects[index];
          if (!canOverride || (double) objectItem.m_Circle.radius >= (double) num4)
          {
            float num5 = radius + objectItem.m_Circle.radius;
            float x = math.distancesq(position.xz, objectItem.m_Circle.position);
            if ((double) x < (double) num5 * (double) num5)
            {
              float num6 = math.sqrt(x);
              float2 float2 = (objectItem.m_Circle.position - position.xz) * (float) ((double) num5 / (double) num6 - 1.0);
              position.xz += float2;
              flag = true;
            }
          }
        }
      }
      if (!flag)
        return true;
      return !AreaUtils.IntersectEdges(position, radius, extraRadius, nodes) && !AreaUtils.IntersectObjects(position, radius, extraRadius, objects, canOverride);
    }

    public static bool IntersectArea(
      float3 position,
      float radius,
      DynamicBuffer<Node> nodes,
      DynamicBuffer<Triangle> triangles)
    {
      Circle2 circle = new Circle2(radius, position.xz);
      for (int index = 0; index < triangles.Length; ++index)
      {
        if (MathUtils.Intersect(AreaUtils.GetTriangle2(nodes, triangles[index]), circle))
          return true;
      }
      return false;
    }

    public static bool IntersectEdges(
      float3 position,
      float radius,
      float extraRadius,
      DynamicBuffer<Node> nodes)
    {
      float num1 = radius + extraRadius;
      float num2 = num1 * num1;
      Line2.Segment line;
      line.a = nodes[nodes.Length - 1].m_Position.xz;
      for (int index = 0; index < nodes.Length; ++index)
      {
        line.b = nodes[index].m_Position.xz;
        if ((double) MathUtils.DistanceSquared(line, position.xz, out float _) < (double) num2)
          return true;
        line.a = line.b;
      }
      return false;
    }

    public static bool IntersectObjects(
      float3 position,
      float radius,
      float extraRadius,
      NativeList<AreaUtils.ObjectItem> objects,
      bool canOverride = false)
    {
      if (objects.IsCreated)
      {
        float num1 = (float) (((double) radius + (double) extraRadius) * 0.5);
        for (int index = 0; index < objects.Length; ++index)
        {
          AreaUtils.ObjectItem objectItem = objects[index];
          if (!canOverride || (double) objectItem.m_Circle.radius >= (double) num1)
          {
            float num2 = radius + objectItem.m_Circle.radius;
            if ((double) math.distancesq(position.xz, objectItem.m_Circle.position) < (double) num2 * (double) num2)
              return true;
          }
        }
      }
      return false;
    }

    public static float GetMinNodeDistance(AreaType areaType)
    {
      switch (areaType)
      {
        case AreaType.Lot:
          return 8f;
        case AreaType.District:
          return 32f;
        case AreaType.MapTile:
          return 64f;
        case AreaType.Space:
          return 1f;
        case AreaType.Surface:
          return 0.75f;
        default:
          return 1f;
      }
    }

    public static AreaTypeMask GetTypeMask(AreaType type)
    {
      return type != AreaType.None ? (AreaTypeMask) (1 << (int) (type & (AreaType) 31)) : AreaTypeMask.None;
    }

    public static float3 GetExpandedNode(
      DynamicBuffer<Node> nodes,
      int index,
      float expandAmount,
      bool isComplete,
      bool isCounterClockwise)
    {
      if (!isComplete)
      {
        if (nodes.Length == 1)
          return nodes[index].m_Position;
        if (index == 0)
        {
          float2 xz1 = nodes[math.select(index + 1, 0, index == nodes.Length - 1)].m_Position.xz;
          float3 position = nodes[index].m_Position;
          float2 xz2 = position.xz;
          float2 forward = math.normalizesafe(xz1 - xz2);
          float2 float2 = math.select(MathUtils.Left(forward), MathUtils.Right(forward), isCounterClockwise) - forward;
          position.xz += float2 * expandAmount;
          return position;
        }
        if (index == nodes.Length - 1)
        {
          float2 xz3 = nodes[math.select(index - 1, nodes.Length - 1, index == 0)].m_Position.xz;
          float3 position = nodes[index].m_Position;
          float2 xz4 = position.xz;
          float2 forward = math.normalizesafe(xz3 - xz4);
          float2 float2 = math.select(MathUtils.Right(forward), MathUtils.Left(forward), isCounterClockwise) - forward;
          position.xz += float2 * expandAmount;
          return position;
        }
      }
      float2 xz5 = nodes[math.select(index - 1, nodes.Length - 1, index == 0)].m_Position.xz;
      float2 xz6 = nodes[math.select(index + 1, 0, index == nodes.Length - 1)].m_Position.xz;
      float3 position1 = nodes[index].m_Position;
      float2 float2_1 = math.normalizesafe(xz5 - position1.xz);
      float2 xz7 = position1.xz;
      float2 y = math.normalizesafe(xz6 - xz7);
      float2 x = math.select(MathUtils.Right(float2_1), MathUtils.Left(float2_1), isCounterClockwise);
      double num1 = (double) math.acos(math.clamp(math.dot(float2_1, y), -1f, 1f));
      float num2 = math.sign(math.dot(x, y));
      float num3 = math.tan((float) (num1 * 0.5));
      float2 float2_2 = x + float2_1 * math.select(num2 / num3, 0.0f, (double) num3 < 1.0 / 1000.0);
      position1.xz += float2_2 * expandAmount;
      return position1;
    }

    public static float3 GetExpandedNode(
      NativeArray<SubAreaNode> nodes,
      int index,
      float expandAmount,
      bool isComplete,
      bool isCounterClockwise)
    {
      if (!isComplete)
      {
        if (nodes.Length == 1)
          return nodes[index].m_Position;
        if (index == 0)
        {
          float2 xz1 = nodes[math.select(index + 1, 0, index == nodes.Length - 1)].m_Position.xz;
          float3 position = nodes[index].m_Position;
          float2 xz2 = position.xz;
          float2 forward = math.normalizesafe(xz1 - xz2);
          float2 float2 = math.select(MathUtils.Left(forward), MathUtils.Right(forward), isCounterClockwise) - forward;
          position.xz += float2 * expandAmount;
          return position;
        }
        if (index == nodes.Length - 1)
        {
          float2 xz3 = nodes[math.select(index - 1, nodes.Length - 1, index == 0)].m_Position.xz;
          float3 position = nodes[index].m_Position;
          float2 xz4 = position.xz;
          float2 forward = math.normalizesafe(xz3 - xz4);
          float2 float2 = math.select(MathUtils.Right(forward), MathUtils.Left(forward), isCounterClockwise) - forward;
          position.xz += float2 * expandAmount;
          return position;
        }
      }
      float2 xz5 = nodes[math.select(index - 1, nodes.Length - 1, index == 0)].m_Position.xz;
      float2 xz6 = nodes[math.select(index + 1, 0, index == nodes.Length - 1)].m_Position.xz;
      float3 position1 = nodes[index].m_Position;
      float2 float2_1 = math.normalizesafe(xz5 - position1.xz);
      float2 xz7 = position1.xz;
      float2 y = math.normalizesafe(xz6 - xz7);
      float2 x = math.select(MathUtils.Right(float2_1), MathUtils.Left(float2_1), isCounterClockwise);
      double num1 = (double) math.acos(math.clamp(math.dot(float2_1, y), -1f, 1f));
      float num2 = math.sign(math.dot(x, y));
      float num3 = math.tan((float) (num1 * 0.5));
      float2 float2_2 = x + float2_1 * math.select(num2 / num3, 0.0f, (double) num3 < 1.0 / 1000.0);
      position1.xz += float2_2 * expandAmount;
      return position1;
    }

    public static float3 GetExpandedNode<TNodeList>(
      TNodeList nodes,
      int index,
      int prevIndex,
      int nextIndex,
      float expandAmount,
      bool isCounterClockwise)
      where TNodeList : INativeList<Node>
    {
      Node node = nodes[prevIndex];
      float2 xz1 = node.m_Position.xz;
      node = nodes[nextIndex];
      float2 xz2 = node.m_Position.xz;
      float3 position = nodes[index].m_Position;
      float2 xz3 = position.xz;
      float2 float2_1 = math.normalizesafe(xz1 - xz3);
      float2 y = math.normalizesafe(xz2 - position.xz);
      float2 x = math.select(MathUtils.Right(float2_1), MathUtils.Left(float2_1), isCounterClockwise);
      double num1 = (double) math.acos(math.clamp(math.dot(float2_1, y), -1f, 1f));
      float num2 = math.sign(math.dot(x, y));
      float num3 = math.tan((float) (num1 * 0.5));
      float2 float2_2 = x + float2_1 * math.select(num2 / num3, 0.0f, (double) num3 < 1.0 / 1000.0);
      position.xz += float2_2 * expandAmount;
      return position;
    }

    public static bool SelectAreaPrefab(
      DynamicBuffer<PlaceholderObjectElement> placeholderElements,
      ComponentLookup<SpawnableObjectData> spawnableDatas,
      NativeParallelHashMap<Entity, int> selectedSpawnables,
      ref Unity.Mathematics.Random random,
      out Entity result,
      out int seed)
    {
      int max = 0;
      bool flag = false;
      result = Entity.Null;
      seed = 0;
      for (int index = 0; index < placeholderElements.Length; ++index)
      {
        PlaceholderObjectElement placeholderElement = placeholderElements[index];
        SpawnableObjectData spawnableData = spawnableDatas[placeholderElement.m_Object];
        int num = 0;
        if (selectedSpawnables.IsCreated && selectedSpawnables.TryGetValue(placeholderElement.m_Object, out num))
        {
          if (!flag)
          {
            max = 0;
            flag = true;
          }
        }
        else if (flag)
          continue;
        max += spawnableData.m_Probability;
        if (random.NextInt(max) < spawnableData.m_Probability)
        {
          result = placeholderElement.m_Object;
          seed = flag ? num : random.NextInt();
        }
      }
      if (!(result != Entity.Null))
        return false;
      if (!flag && selectedSpawnables.IsCreated)
        selectedSpawnables.Add(result, seed);
      return true;
    }

    public static void FindAreaPath(
      ref Unity.Mathematics.Random random,
      NativeList<PathElement> path,
      DynamicBuffer<Game.Net.SubLane> lanes,
      Entity startEntity,
      float startCurvePos,
      Entity endEntity,
      float endCurvePos,
      ComponentLookup<Lane> laneData,
      ComponentLookup<Curve> curveData)
    {
      if (startEntity == endEntity)
      {
        path.Add(new PathElement(startEntity, new float2(startCurvePos, endCurvePos)));
      }
      else
      {
        NativeParallelMultiHashMap<PathNode, Entity> parallelMultiHashMap = new NativeParallelMultiHashMap<PathNode, Entity>(lanes.Length * 2, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeParallelHashMap<PathNode, PathElement> nativeParallelHashMap = new NativeParallelHashMap<PathNode, PathElement>(lanes.Length + 1, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeMinHeap<AreaUtils.FixPathItem> nativeMinHeap = new NativeMinHeap<AreaUtils.FixPathItem>(lanes.Length, Allocator.Temp);
        for (int index = 0; index < lanes.Length; ++index)
        {
          Entity subLane = lanes[index].m_SubLane;
          Lane lane = laneData[subLane];
          parallelMultiHashMap.Add(lane.m_StartNode, subLane);
          parallelMultiHashMap.Add(lane.m_EndNode, subLane);
        }
        Lane lane1 = laneData[endEntity];
        Curve curve1 = curveData[endEntity];
        float cost1 = random.NextFloat(0.5f, 1f) * curve1.m_Length * endCurvePos;
        float cost2 = (float) ((double) random.NextFloat(0.5f, 1f) * (double) curve1.m_Length * (1.0 - (double) endCurvePos));
        nativeMinHeap.Insert(new AreaUtils.FixPathItem(lane1.m_StartNode, new PathElement(endEntity, new float2(0.0f, endCurvePos)), cost1));
        nativeMinHeap.Insert(new AreaUtils.FixPathItem(lane1.m_EndNode, new PathElement(endEntity, new float2(1f, endCurvePos)), cost2));
        while (nativeMinHeap.Length != 0)
        {
          AreaUtils.FixPathItem fixPathItem = nativeMinHeap.Extract();
          if (nativeParallelHashMap.TryAdd(fixPathItem.m_Node, fixPathItem.m_PathElement))
          {
            if (fixPathItem.m_PathElement.m_Target == startEntity)
            {
              path.Add(in fixPathItem.m_PathElement);
              Lane lane2 = laneData[startEntity];
              PathElement pathElement;
              Lane lane3;
              for (PathNode key = (double) fixPathItem.m_PathElement.m_TargetDelta.y == 0.0 ? lane2.m_StartNode : lane2.m_EndNode; nativeParallelHashMap.TryGetValue(key, out pathElement); key = (double) pathElement.m_TargetDelta.y == 0.0 ? lane3.m_StartNode : lane3.m_EndNode)
              {
                path.Add(in pathElement);
                if (!(pathElement.m_Target == endEntity))
                  lane3 = laneData[pathElement.m_Target];
                else
                  break;
              }
              break;
            }
            Entity entity;
            NativeParallelMultiHashMapIterator<PathNode> it;
            if (parallelMultiHashMap.TryGetFirstValue(fixPathItem.m_Node, out entity, out it))
            {
              do
              {
                if (!(entity == fixPathItem.m_PathElement.m_Target))
                {
                  Lane lane4 = laneData[entity];
                  Curve curve2 = curveData[entity];
                  if (lane4.m_EndNode.Equals(fixPathItem.m_Node))
                  {
                    if (entity == startEntity)
                    {
                      float num = (float) ((double) random.NextFloat(0.5f, 1f) * (double) curve2.m_Length * (1.0 - (double) startCurvePos));
                      nativeMinHeap.Insert(new AreaUtils.FixPathItem(lane4.m_MiddleNode, new PathElement(startEntity, new float2(startCurvePos, 1f)), fixPathItem.m_Cost + num));
                    }
                    else if (!nativeParallelHashMap.ContainsKey(lane4.m_StartNode))
                    {
                      float num = random.NextFloat(0.5f, 1f) * curve2.m_Length;
                      nativeMinHeap.Insert(new AreaUtils.FixPathItem(lane4.m_StartNode, new PathElement(entity, new float2(0.0f, 1f)), fixPathItem.m_Cost + num));
                    }
                  }
                  else if (lane4.m_StartNode.Equals(fixPathItem.m_Node))
                  {
                    if (entity == startEntity)
                    {
                      float num = random.NextFloat(0.5f, 1f) * curve2.m_Length * startCurvePos;
                      nativeMinHeap.Insert(new AreaUtils.FixPathItem(lane4.m_MiddleNode, new PathElement(startEntity, new float2(startCurvePos, 0.0f)), fixPathItem.m_Cost + num));
                    }
                    else if (!nativeParallelHashMap.ContainsKey(lane4.m_EndNode))
                    {
                      float num = random.NextFloat(0.5f, 1f) * curve2.m_Length;
                      nativeMinHeap.Insert(new AreaUtils.FixPathItem(lane4.m_EndNode, new PathElement(entity, new float2(1f, 0.0f)), fixPathItem.m_Cost + num));
                    }
                  }
                }
              }
              while (parallelMultiHashMap.TryGetNextValue(out entity, ref it));
            }
          }
        }
        parallelMultiHashMap.Dispose();
        nativeParallelHashMap.Dispose();
        nativeMinHeap.Dispose();
      }
    }

    public static Node AdjustPosition(
      Node node,
      ref TerrainHeightData terrainHeightData,
      ref WaterSurfaceData waterSurfaceData)
    {
      Node node1 = node;
      node1.m_Position.y = WaterUtils.SampleHeight(ref waterSurfaceData, ref terrainHeightData, node.m_Position);
      return node1;
    }

    public static Node AdjustPosition(Node node, ref TerrainHeightData terrainHeightData)
    {
      Node node1 = node;
      node1.m_Position.y = TerrainUtils.SampleHeight(ref terrainHeightData, node.m_Position);
      return node1;
    }

    public static void SetCollisionFlags(ref AreaGeometryData areaGeometryData, bool ignoreMarkers)
    {
      if (ignoreMarkers)
        return;
      switch (areaGeometryData.m_Type)
      {
        case AreaType.Space:
        case AreaType.Surface:
          areaGeometryData.m_Flags |= GeometryFlags.PhysicalGeometry;
          break;
      }
    }

    public struct ObjectItem
    {
      public Circle2 m_Circle;
      public Entity m_Entity;

      public ObjectItem(float radius, float2 position, Entity entity)
      {
        this.m_Circle = new Circle2(radius, position);
        this.m_Entity = entity;
      }
    }

    private struct FixPathItem : ILessThan<AreaUtils.FixPathItem>
    {
      public PathNode m_Node;
      public PathElement m_PathElement;
      public float m_Cost;

      public FixPathItem(PathNode node, PathElement pathElement, float cost)
      {
        this.m_Node = node;
        this.m_PathElement = pathElement;
        this.m_Cost = cost;
      }

      public bool LessThan(AreaUtils.FixPathItem other)
      {
        return (double) this.m_Cost < (double) other.m_Cost;
      }
    }
  }
}
