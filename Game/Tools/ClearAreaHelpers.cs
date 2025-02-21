// Decompiled with JetBrains decompiler
// Type: Game.Tools.ClearAreaHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Objects;
using Game.Prefabs;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  public static class ClearAreaHelpers
  {
    public static void FillClearAreas(
      DynamicBuffer<InstalledUpgrade> installedUpgrades,
      Entity ignoreUpgradeOrArea,
      ComponentLookup<Transform> transformData,
      ComponentLookup<Clear> clearAreaData,
      ComponentLookup<PrefabRef> prefabRefData,
      ComponentLookup<ObjectGeometryData> prefabObjectGeometryData,
      BufferLookup<Game.Areas.SubArea> subAreaBuffers,
      BufferLookup<Node> nodeBuffers,
      BufferLookup<Triangle> triangleBuffers,
      ref NativeList<ClearAreaData> clearAreas)
    {
      for (int index = 0; index < installedUpgrades.Length; ++index)
      {
        Entity upgrade = installedUpgrades[index].m_Upgrade;
        DynamicBuffer<Game.Areas.SubArea> bufferData;
        if (!(upgrade == ignoreUpgradeOrArea) && subAreaBuffers.TryGetBuffer(upgrade, out bufferData))
        {
          Transform transform = transformData[upgrade];
          PrefabRef prefabRef = prefabRefData[upgrade];
          ObjectGeometryData objectGeometryData = prefabObjectGeometryData[prefabRef.m_Prefab];
          ClearAreaHelpers.FillClearAreas(bufferData, transform, objectGeometryData, ignoreUpgradeOrArea, ref clearAreaData, ref nodeBuffers, ref triangleBuffers, ref clearAreas);
        }
      }
    }

    public static void FillClearAreas(
      DynamicBuffer<Game.Areas.SubArea> subAreas,
      Transform transform,
      ObjectGeometryData objectGeometryData,
      Entity ignoreArea,
      ref ComponentLookup<Clear> clearAreaData,
      ref BufferLookup<Node> nodeBuffers,
      ref BufferLookup<Triangle> triangleBuffers,
      ref NativeList<ClearAreaData> clearAreas)
    {
      for (int index1 = 0; index1 < subAreas.Length; ++index1)
      {
        Entity area = subAreas[index1].m_Area;
        if (clearAreaData.HasComponent(area) && !(area == ignoreArea))
        {
          if (!clearAreas.IsCreated)
            clearAreas = new NativeList<ClearAreaData>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          DynamicBuffer<Node> nodes = nodeBuffers[area];
          DynamicBuffer<Triangle> dynamicBuffer = triangleBuffers[area];
          float num = (float) ((double) transform.m_Position.y + (double) objectGeometryData.m_Bounds.max.y + 1.0);
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            ClearAreaData clearAreaData1 = new ClearAreaData()
            {
              m_Triangle = AreaUtils.GetTriangle3(nodes, dynamicBuffer[index2]),
              m_TopY = num
            };
            clearAreas.Add(in clearAreaData1);
          }
        }
      }
    }

    public static void FillClearAreas(
      Entity ownerPrefab,
      Transform ownerTransform,
      ComponentLookup<ObjectGeometryData> prefabObjectGeometryData,
      ComponentLookup<AreaGeometryData> prefabAreaGeometryData,
      BufferLookup<Game.Prefabs.SubArea> prefabSubAreas,
      BufferLookup<SubAreaNode> prefabSubAreaNodes,
      ref NativeList<ClearAreaData> clearAreas)
    {
      DynamicBuffer<Game.Prefabs.SubArea> bufferData;
      if (!prefabSubAreas.TryGetBuffer(ownerPrefab, out bufferData))
        return;
      for (int index1 = 0; index1 < bufferData.Length; ++index1)
      {
        Game.Prefabs.SubArea subArea = bufferData[index1];
        if ((prefabAreaGeometryData[subArea.m_Prefab].m_Flags & Game.Areas.GeometryFlags.ClearArea) != (Game.Areas.GeometryFlags) 0)
        {
          if (!clearAreas.IsCreated)
            clearAreas = new NativeList<ClearAreaData>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          int length = subArea.m_NodeRange.y - subArea.m_NodeRange.x;
          DynamicBuffer<SubAreaNode> prefabSubAreaNode = prefabSubAreaNodes[ownerPrefab];
          NativeArray<SubAreaNode> subArray = prefabSubAreaNode.AsNativeArray().GetSubArray(subArea.m_NodeRange.x, length);
          NativeArray<float3> nodes = new NativeArray<float3>(length, Allocator.Temp);
          NativeList<Triangle> triangles = new NativeList<Triangle>((AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated method
          bool isCounterClockwise = (double) GeometrySystem.Area(subArray) > 0.0;
          for (int index2 = 0; index2 < length; ++index2)
            nodes[index2] = AreaUtils.GetExpandedNode(subArray, index2, -0.1f, true, isCounterClockwise);
          // ISSUE: reference to a compiler-generated method
          GeometrySystem.Triangulate<NativeList<Triangle>>(nodes, triangles, new NativeArray<Bounds2>(), 0, isCounterClockwise);
          // ISSUE: reference to a compiler-generated method
          GeometrySystem.EqualizeTriangles<NativeList<Triangle>>(nodes, triangles);
          ObjectGeometryData objectGeometryData = prefabObjectGeometryData[ownerPrefab];
          float num = (float) ((double) ownerTransform.m_Position.y + (double) objectGeometryData.m_Bounds.max.y + 1.0);
          for (int index3 = 0; index3 < triangles.Length; ++index3)
          {
            int3 int3 = triangles[index3].m_Indices + subArea.m_NodeRange.x;
            ClearAreaData clearAreaData = new ClearAreaData()
            {
              m_Triangle = new Triangle3(ObjectUtils.LocalToWorld(ownerTransform, prefabSubAreaNode[int3.x].m_Position), ObjectUtils.LocalToWorld(ownerTransform, prefabSubAreaNode[int3.y].m_Position), ObjectUtils.LocalToWorld(ownerTransform, prefabSubAreaNode[int3.z].m_Position)),
              m_TopY = num
            };
            clearAreas.Add(in clearAreaData);
          }
          nodes.Dispose();
          triangles.Dispose();
        }
      }
    }

    public static void FillClearAreas(
      Entity ownerPrefab,
      Transform ownerTransform,
      DynamicBuffer<Node> nodes,
      bool isComplete,
      ComponentLookup<ObjectGeometryData> prefabObjectGeometryData,
      ref NativeList<ClearAreaData> clearAreas)
    {
      int length = nodes.Length;
      if (length < 3)
        return;
      if (length >= 4 && nodes[0].m_Position.Equals(nodes[length - 1].m_Position))
      {
        isComplete = true;
        --length;
      }
      if (!clearAreas.IsCreated)
        clearAreas = new NativeList<ClearAreaData>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      NativeArray<float3> nodes1 = new NativeArray<float3>(length, Allocator.Temp);
      NativeList<Triangle> triangles = new NativeList<Triangle>((AllocatorManager.AllocatorHandle) Allocator.Temp);
      // ISSUE: reference to a compiler-generated method
      bool isCounterClockwise = (double) GeometrySystem.Area(nodes) > 0.0;
      for (int index = 0; index < length; ++index)
        nodes1[index] = AreaUtils.GetExpandedNode(nodes, index, -0.1f, isComplete, isCounterClockwise);
      // ISSUE: reference to a compiler-generated method
      GeometrySystem.Triangulate<NativeList<Triangle>>(nodes1, triangles, new NativeArray<Bounds2>(), 0, isCounterClockwise);
      // ISSUE: reference to a compiler-generated method
      GeometrySystem.EqualizeTriangles<NativeList<Triangle>>(nodes1, triangles);
      ObjectGeometryData objectGeometryData = prefabObjectGeometryData[ownerPrefab];
      float num = (float) ((double) ownerTransform.m_Position.y + (double) objectGeometryData.m_Bounds.max.y + 1.0);
      for (int index = 0; index < triangles.Length; ++index)
      {
        ClearAreaData clearAreaData = new ClearAreaData()
        {
          m_Triangle = AreaUtils.GetTriangle3(nodes, triangles[index]),
          m_TopY = num
        };
        clearAreas.Add(in clearAreaData);
      }
      nodes1.Dispose();
      triangles.Dispose();
    }

    public static void TransformClearAreas(
      NativeList<ClearAreaData> clearAreas,
      Transform oldTransform,
      Transform newTransform)
    {
      if (!clearAreas.IsCreated)
        return;
      Transform inverseParentTransform = ObjectUtils.InverseTransform(oldTransform);
      for (int index = 0; index < clearAreas.Length; ++index)
      {
        ClearAreaData clearArea = clearAreas[index];
        clearArea.m_Triangle.a = ObjectUtils.LocalToWorld(newTransform, ObjectUtils.WorldToLocal(inverseParentTransform, clearArea.m_Triangle.a));
        clearArea.m_Triangle.b = ObjectUtils.LocalToWorld(newTransform, ObjectUtils.WorldToLocal(inverseParentTransform, clearArea.m_Triangle.b));
        clearArea.m_Triangle.c = ObjectUtils.LocalToWorld(newTransform, ObjectUtils.WorldToLocal(inverseParentTransform, clearArea.m_Triangle.c));
        clearArea.m_TopY += newTransform.m_Position.y - oldTransform.m_Position.y;
        clearAreas[index] = clearArea;
      }
    }

    public static void InitClearAreas(
      NativeList<ClearAreaData> clearAreas,
      Transform topLevelTransform)
    {
      if (!clearAreas.IsCreated)
        return;
      for (int index = 0; index < clearAreas.Length; ++index)
      {
        ClearAreaData clearArea = clearAreas[index];
        clearArea.m_OnGround = math.any(math.abs(clearArea.m_Triangle.y.abc - topLevelTransform.m_Position.y) <= 1f);
        clearArea.m_Triangle.y -= 1f;
        clearAreas[index] = clearArea;
      }
    }

    public static bool ShouldClear(
      NativeList<ClearAreaData> clearAreas,
      float3 position,
      bool onGround)
    {
      if (clearAreas.IsCreated)
      {
        for (int index = 0; index < clearAreas.Length; ++index)
        {
          ClearAreaData clearArea = clearAreas[index];
          float2 t;
          if (MathUtils.Intersect(clearArea.m_Triangle.xz, position.xz, out t))
          {
            if (clearArea.m_OnGround & onGround)
              return true;
            float num1 = MathUtils.Position(clearArea.m_Triangle.y, t);
            float num2 = math.max(clearArea.m_TopY, num1 + 2f);
            if ((double) position.y >= (double) num1 && (double) position.y <= (double) num2)
              return true;
          }
        }
      }
      return false;
    }

    public static bool ShouldClear(
      NativeList<ClearAreaData> clearAreas,
      Bezier4x3 curve,
      bool onGround)
    {
      if (clearAreas.IsCreated)
      {
        Bounds3 bounds3 = MathUtils.Bounds(curve);
        for (int index1 = 0; index1 < clearAreas.Length; ++index1)
        {
          ClearAreaData clearArea = clearAreas[index1];
          if (MathUtils.Intersect(MathUtils.Bounds(clearArea.m_Triangle.xz), bounds3.xz))
          {
            Line3.Segment line;
            line.a = curve.a;
            for (int index2 = 1; index2 <= 16; ++index2)
            {
              line.b = MathUtils.Position(curve, (float) index2 * (1f / 16f));
              float2 t;
              if (MathUtils.Intersect(clearArea.m_Triangle.xz, line.xz, out t))
              {
                if (clearArea.m_OnGround & onGround)
                  return true;
                float3 float3 = MathUtils.Position(line, math.csum(t) * 0.5f);
                if (MathUtils.Intersect(clearArea.m_Triangle.xz, float3.xz, out t))
                {
                  float num1 = MathUtils.Position(clearArea.m_Triangle.y, t);
                  float num2 = math.max(clearArea.m_TopY, num1 + 2f);
                  if ((double) float3.y >= (double) num1 && (double) float3.y <= (double) num2)
                    return true;
                }
              }
              line.a = line.b;
            }
          }
        }
      }
      return false;
    }

    public static bool ShouldClear(
      NativeList<ClearAreaData> clearAreas,
      DynamicBuffer<Node> nodes,
      DynamicBuffer<Triangle> triangles,
      Transform ownerTransform)
    {
      if (clearAreas.IsCreated)
      {
        for (int index = 0; index < triangles.Length; ++index)
        {
          Triangle3 triangle3 = AreaUtils.GetTriangle3(nodes, triangles[index]);
          if (ClearAreaHelpers.ShouldClear(clearAreas, triangle3, ownerTransform))
            return true;
        }
      }
      return false;
    }

    public static bool ShouldClear(
      NativeList<ClearAreaData> clearAreas,
      DynamicBuffer<SubAreaNode> subAreaNodes,
      int2 nodeRange,
      Transform ownerTransform)
    {
      if (clearAreas.IsCreated)
      {
        int length = nodeRange.y - nodeRange.x;
        NativeArray<SubAreaNode> subArray = subAreaNodes.AsNativeArray().GetSubArray(nodeRange.x, length);
        NativeArray<float3> nodes = new NativeArray<float3>(length, Allocator.Temp);
        NativeList<Triangle> triangles = new NativeList<Triangle>((AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated method
        bool isCounterClockwise = (double) GeometrySystem.Area(subArray) > 0.0;
        for (int index = 0; index < length; ++index)
          nodes[index] = AreaUtils.GetExpandedNode(subArray, index, -0.1f, true, isCounterClockwise);
        // ISSUE: reference to a compiler-generated method
        GeometrySystem.Triangulate<NativeList<Triangle>>(nodes, triangles, new NativeArray<Bounds2>(), 0, isCounterClockwise);
        // ISSUE: reference to a compiler-generated method
        GeometrySystem.EqualizeTriangles<NativeList<Triangle>>(nodes, triangles);
        for (int index = 0; index < triangles.Length; ++index)
        {
          int3 int3 = triangles[index].m_Indices + nodeRange.x;
          Triangle3 triangle = new Triangle3(ObjectUtils.LocalToWorld(ownerTransform, subAreaNodes[int3.x].m_Position), ObjectUtils.LocalToWorld(ownerTransform, subAreaNodes[int3.y].m_Position), ObjectUtils.LocalToWorld(ownerTransform, subAreaNodes[int3.z].m_Position));
          if (ClearAreaHelpers.ShouldClear(clearAreas, triangle, ownerTransform))
          {
            nodes.Dispose();
            triangles.Dispose();
            return true;
          }
        }
        nodes.Dispose();
        triangles.Dispose();
      }
      return false;
    }

    private static bool ShouldClear(
      NativeList<ClearAreaData> clearAreas,
      Triangle3 triangle,
      Transform ownerTransform)
    {
      Bounds3 bounds3_1 = MathUtils.Bounds(triangle);
      bool flag = math.any(math.abs(triangle.y.abc - ownerTransform.m_Position.y) <= 1f);
      for (int index = 0; index < clearAreas.Length; ++index)
      {
        ClearAreaData clearArea = clearAreas[index];
        Bounds3 bounds3_2 = MathUtils.Bounds(clearArea.m_Triangle);
        if (MathUtils.Intersect(bounds3_1.xz, bounds3_2.xz) && MathUtils.Intersect(triangle.xz, clearArea.m_Triangle.xz))
        {
          if (clearArea.m_OnGround & flag)
            return true;
          float y = bounds3_2.min.y;
          float num = math.max(clearArea.m_TopY, y + 2f);
          if ((double) bounds3_1.max.y >= (double) y && (double) bounds3_1.min.y <= (double) num)
            return true;
        }
      }
      return false;
    }
  }
}
