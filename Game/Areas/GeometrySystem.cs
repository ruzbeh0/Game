// Decompiled with JetBrains decompiler
// Type: Game.Areas.GeometrySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Areas
{
  [CompilerGenerated]
  public class GeometrySystem : GameSystemBase
  {
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private EntityQuery m_UpdatedAreasQuery;
    private EntityQuery m_AllAreasQuery;
    private EntityQuery m_CreatedBuildingsQuery;
    private bool m_Loaded;
    private GeometrySystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedAreasQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Areas.Area>(), ComponentType.ReadOnly<Updated>(), ComponentType.ReadOnly<Node>(), ComponentType.ReadWrite<Triangle>());
      // ISSUE: reference to a compiler-generated field
      this.m_AllAreasQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Areas.Area>(), ComponentType.ReadOnly<Node>(), ComponentType.ReadWrite<Triangle>());
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedBuildingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<Owner>(), ComponentType.Exclude<Temp>());
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
      EntityQuery entityQuery = this.GetLoaded() ? this.m_AllAreasQuery : this.m_UpdatedAreasQuery;
      if (entityQuery.IsEmptyIgnoreFilter)
        return;
      JobHandle outJobHandle1;
      NativeList<Entity> entityListAsync1 = entityQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle1);
      JobHandle outJobHandle2;
      // ISSUE: reference to a compiler-generated field
      NativeList<Entity> entityListAsync2 = this.m_CreatedBuildingsQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Geometry_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Area_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TerrainAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Space_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle deps;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new GeometrySystem.TriangulateAreasJob()
      {
        m_Entities = entityListAsync1.AsDeferredJobArray(),
        m_Buildings = entityListAsync2,
        m_SpaceData = this.__TypeHandle.__Game_Areas_Space_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabTerrainAreaData = this.__TypeHandle.__Game_Prefabs_TerrainAreaData_RO_ComponentLookup,
        m_PrefabAreaGeometryData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(true),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_AreaData = this.__TypeHandle.__Game_Areas_Area_RW_ComponentLookup,
        m_GeometryData = this.__TypeHandle.__Game_Areas_Geometry_RW_ComponentLookup,
        m_Nodes = this.__TypeHandle.__Game_Areas_Node_RW_BufferLookup,
        m_Triangles = this.__TypeHandle.__Game_Areas_Triangle_RW_BufferLookup
      }.Schedule<GeometrySystem.TriangulateAreasJob, Entity>(entityListAsync1, 1, JobUtils.CombineDependencies(this.Dependency, outJobHandle2, outJobHandle1, deps));
      entityListAsync1.Dispose(jobHandle);
      entityListAsync2.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(jobHandle);
      this.Dependency = jobHandle;
    }

    public static void BuildEdgeBounds(
      DynamicBuffer<Node> nodes,
      NativeArray<float3> expandedNodes,
      out NativeArray<Bounds2> edgeBounds,
      out int totalDepth)
    {
      int length1 = -1;
      int num1 = 0;
      for (int length2 = nodes.Length; length2 >= 4; length2 >>= 1)
        length1 += 1 << num1++;
      edgeBounds = new NativeArray<Bounds2>(length1, Allocator.Temp);
      int num2;
      totalDepth = num2 = num1 - 1;
      int num3 = 1 << num2;
      int num4 = length1 - num3;
      int length3 = nodes.Length;
      for (int index1 = 0; index1 < num3; ++index1)
      {
        int index2 = index1 * length3 >> num2;
        int num5 = (index1 + 1) * length3 >> num2;
        Bounds2 bounds2 = MathUtils.Bounds(nodes[index2].m_Position.xz, expandedNodes[index2].xz);
        for (int a = index2 + 1; a <= num5; ++a)
        {
          int index3 = math.select(a, 0, a == length3);
          bounds2 = bounds2 | nodes[index3].m_Position.xz | expandedNodes[index3].xz;
        }
        edgeBounds[num4 + index1] = bounds2;
      }
      while (--num2 > 0)
      {
        int num6 = num4;
        int num7 = 1 << num2;
        num4 -= num7;
        for (int index = 0; index < num7; ++index)
          edgeBounds[num4 + index] = edgeBounds[num6 + (index << 1)] | edgeBounds[num6 + (index << 1) + 1];
      }
    }

    public static void Triangulate<T>(
      NativeArray<float3> nodes,
      T triangles,
      NativeArray<Bounds2> edgeBounds,
      int totalDepth,
      bool isCounterClockwise)
      where T : INativeList<Triangle>
    {
      if (nodes.Length < 3)
      {
        triangles.Clear();
      }
      else
      {
        triangles.Length = nodes.Length - 2;
        int num1 = 0;
        NativeArray<GeometrySystem.Index> indexBuffer = new NativeArray<GeometrySystem.Index>(nodes.Length, Allocator.Temp);
        if (isCounterClockwise)
        {
          for (int index = 0; index < nodes.Length; ++index)
          {
            int num2 = math.select(index - 1, nodes.Length - 1, index == 0);
            int num3 = math.select(index + 1, 0, index + 1 == nodes.Length);
            // ISSUE: object of a compiler-generated type is created
            indexBuffer[index] = new GeometrySystem.Index()
            {
              m_NodeIndex = index,
              m_PrevIndex = num2,
              m_NextIndex = num3,
              m_SkipIndex = num3
            };
          }
        }
        else
        {
          for (int index = 0; index < nodes.Length; ++index)
          {
            int num4 = math.select(index - 1, nodes.Length - 1, index == 0);
            int num5 = math.select(index + 1, 0, index + 1 == nodes.Length);
            // ISSUE: object of a compiler-generated type is created
            indexBuffer[index] = new GeometrySystem.Index()
            {
              m_NodeIndex = nodes.Length - 1 - index,
              m_PrevIndex = num4,
              m_NextIndex = num5,
              m_SkipIndex = num5
            };
          }
        }
        int length = nodes.Length;
        int num6 = 2 * length;
        int index1 = length - 2;
        int index2 = length - 1;
        while (length > 2)
        {
          if (0 >= num6--)
          {
            triangles.Clear();
            break;
          }
          // ISSUE: variable of a compiler-generated type
          GeometrySystem.Index index0 = indexBuffer[index2];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          GeometrySystem.Index index1_1 = indexBuffer[index0.m_NextIndex];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          GeometrySystem.Index index2_1 = indexBuffer[index1_1.m_NextIndex];
          // ISSUE: reference to a compiler-generated method
          if (GeometrySystem.Snip(nodes, index0, index1_1, index2_1, length, totalDepth, edgeBounds, indexBuffer))
          {
            if (num1 == triangles.Length)
            {
              triangles.Clear();
              break;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            triangles[num1++] = new Triangle(index0.m_NodeIndex, index1_1.m_NodeIndex, index2_1.m_NodeIndex);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            index0.m_SkipIndex = math.select(index0.m_SkipIndex, index1_1.m_SkipIndex, index0.m_SkipIndex == index0.m_NextIndex);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            index0.m_NextIndex = index1_1.m_NextIndex;
            // ISSUE: reference to a compiler-generated field
            index2_1.m_PrevIndex = index2;
            indexBuffer[index2] = index0;
            // ISSUE: reference to a compiler-generated field
            indexBuffer[index1_1.m_NextIndex] = index2_1;
            // ISSUE: reference to a compiler-generated field
            if (index1 != index0.m_PrevIndex)
            {
              // ISSUE: variable of a compiler-generated type
              GeometrySystem.Index index3 = indexBuffer[index1];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              GeometrySystem.Index index4 = indexBuffer[index0.m_PrevIndex];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              index3.m_SkipIndex = index0.m_PrevIndex;
              // ISSUE: reference to a compiler-generated field
              index4.m_SkipIndex = index2;
              indexBuffer[index1] = index3;
              // ISSUE: reference to a compiler-generated field
              indexBuffer[index0.m_PrevIndex] = index4;
            }
            index1 = index2;
            num6 = 2 * --length;
          }
          // ISSUE: reference to a compiler-generated field
          index2 = index0.m_SkipIndex;
        }
        if (num1 != triangles.Length)
          triangles.Clear();
        indexBuffer.Dispose();
      }
    }

    public static float Area(DynamicBuffer<Node> nodes)
    {
      int length = nodes.Length;
      float num = 0.0f;
      float3 float3 = (float3) 0.0f;
      if (nodes.Length != 0)
        float3 = nodes[0].m_Position;
      int index1 = length - 1;
      for (int index2 = 0; index2 < length; index1 = index2++)
      {
        Node node = nodes[index1];
        float2 float2_1 = node.m_Position.xz - float3.xz;
        node = nodes[index2];
        float2 float2_2 = node.m_Position.zx - float3.zx;
        float2 float2_3 = float2_1 * float2_2;
        num += float2_3.x - float2_3.y;
      }
      return num * 0.5f;
    }

    public static float Area(NativeArray<SubAreaNode> nodes)
    {
      int length = nodes.Length;
      float num = 0.0f;
      float3 float3 = (float3) 0.0f;
      if (nodes.Length != 0)
        float3 = nodes[0].m_Position;
      int index1 = length - 1;
      for (int index2 = 0; index2 < length; index1 = index2++)
      {
        SubAreaNode node = nodes[index1];
        float2 float2_1 = node.m_Position.xz - float3.xz;
        node = nodes[index2];
        float2 float2_2 = node.m_Position.zx - float3.zx;
        float2 float2_3 = float2_1 * float2_2;
        num += float2_3.x - float2_3.y;
      }
      return num * 0.5f;
    }

    private static bool Snip(
      NativeArray<float3> nodes,
      GeometrySystem.Index index0,
      GeometrySystem.Index index1,
      GeometrySystem.Index index2,
      int nodeCount,
      int totalDepth,
      NativeArray<Bounds2> edgeBounds,
      NativeArray<GeometrySystem.Index> indexBuffer)
    {
      Triangle2 triangle;
      ref Triangle2 local = ref triangle;
      // ISSUE: reference to a compiler-generated field
      float3 node = nodes[index0.m_NodeIndex];
      float2 xz1 = node.xz;
      // ISSUE: reference to a compiler-generated field
      node = nodes[index1.m_NodeIndex];
      float2 xz2 = node.xz;
      // ISSUE: reference to a compiler-generated field
      node = nodes[index2.m_NodeIndex];
      float2 xz3 = node.xz;
      local = new Triangle2(xz1, xz2, xz3);
      float2 float2 = (triangle.b - triangle.a) * (triangle.c - triangle.a).yx;
      if ((double) float2.x - (double) float2.y < 1.4012984643248171E-45)
        return false;
      if (edgeBounds.IsCreated)
      {
        Bounds2 bounds2 = MathUtils.Bounds(triangle);
        int num1 = 0;
        int num2 = 1;
        int num3 = 0;
        while (num2 > 0)
        {
          if (MathUtils.Intersect(edgeBounds[num1 + num3], bounds2))
          {
            if (num2 == totalDepth)
            {
              int num4 = num3 * nodes.Length >> num2;
              int num5 = (num3 + 1) * nodes.Length >> num2;
              for (int index = num4; index < num5; ++index)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (index != index0.m_NodeIndex && index != index1.m_NodeIndex && index != index2.m_NodeIndex && MathUtils.Intersect(triangle, nodes[index].xz))
                  return false;
              }
            }
            else
            {
              num3 <<= 1;
              num1 += 1 << num2++;
              continue;
            }
          }
          while ((num3 & 1) != 0)
          {
            num3 >>= 1;
            num1 -= 1 << --num2;
          }
          ++num3;
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        GeometrySystem.Index index3 = indexBuffer[index2.m_NextIndex];
        for (int index4 = 3; index4 < nodeCount; ++index4)
        {
          // ISSUE: reference to a compiler-generated field
          if (MathUtils.Intersect(triangle, nodes[index3.m_NodeIndex].xz))
            return false;
          // ISSUE: reference to a compiler-generated field
          index3 = indexBuffer[index3.m_NextIndex];
        }
      }
      return true;
    }

    public static void EqualizeTriangles<T>(NativeArray<float3> nodes, T triangles) where T : INativeList<Triangle>
    {
      if (triangles.Length < 2)
        return;
      NativeParallelHashMap<int2, int2> edgeMap = new NativeParallelHashMap<int2, int2>(triangles.Length * 3, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      for (int index = 0; index < triangles.Length; ++index)
      {
        Triangle triangle = triangles[index];
        bool3 bool3 = triangle.m_Indices.yzx - triangle.m_Indices.zxy > 1;
        if (bool3.x)
          edgeMap.TryAdd(triangle.m_Indices.yz, new int2(index, 0));
        if (bool3.y)
          edgeMap.TryAdd(triangle.m_Indices.zx, new int2(index, 1));
        if (bool3.z)
          edgeMap.TryAdd(triangle.m_Indices.xy, new int2(index, 2));
      }
      for (int index = 0; index < 1000; ++index)
      {
        bool flag = false;
        int num = 0;
        while (num < triangles.Length)
        {
          Triangle triangle = triangles[num];
          bool3 bool3 = triangle.m_Indices.zxy - triangle.m_Indices.yzx > 1;
          int2 index2_1;
          // ISSUE: reference to a compiler-generated method
          if (bool3.x && edgeMap.TryGetValue(triangle.m_Indices.zy, out index2_1) && GeometrySystem.TurnEdgeIfNeeded<T>(nodes, triangles, edgeMap, new int2(num, 0), index2_1))
          {
            flag = true;
          }
          else
          {
            int2 index2_2;
            // ISSUE: reference to a compiler-generated method
            if (bool3.y && edgeMap.TryGetValue(triangle.m_Indices.xz, out index2_2) && GeometrySystem.TurnEdgeIfNeeded<T>(nodes, triangles, edgeMap, new int2(num, 1), index2_2))
            {
              flag = true;
            }
            else
            {
              int2 index2_3;
              // ISSUE: reference to a compiler-generated method
              if (bool3.z && edgeMap.TryGetValue(triangle.m_Indices.yx, out index2_3) && GeometrySystem.TurnEdgeIfNeeded<T>(nodes, triangles, edgeMap, new int2(num, 2), index2_3))
                flag = true;
              else
                ++num;
            }
          }
        }
        if (!flag)
          break;
      }
      edgeMap.Dispose();
    }

    private static bool TurnEdgeIfNeeded<T>(
      NativeArray<float3> nodes,
      T triangles,
      NativeParallelHashMap<int2, int2> edgeMap,
      int2 index1,
      int2 index2)
      where T : INativeList<Triangle>
    {
      Triangle triangle1 = triangles[index1.x];
      Triangle triangle2 = triangles[index2.x];
      int2 int2_1 = new int2(index1.y, index2.y);
      int2 int2_2 = new int2(triangle1.m_Indices[int2_1.x], triangle2.m_Indices[int2_1.y]);
      int2 int2_3 = math.select(int2_1 + 1, (int2) 0, int2_1 == 2);
      int2_3 = new int2(triangle1.m_Indices[int2_3.x], triangle2.m_Indices[int2_3.y]);
      Triangle triangle3 = new Triangle(int2_2.x, int2_3.x, int2_2.y);
      Triangle triangle4 = new Triangle(int2_2.y, int2_3.y, int2_2.x);
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      float num = math.min(GeometrySystem.GetEqualizationValue(nodes, triangle1), GeometrySystem.GetEqualizationValue(nodes, triangle2));
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      if ((double) math.min(GeometrySystem.GetEqualizationValue(nodes, triangle3), GeometrySystem.GetEqualizationValue(nodes, triangle4)) <= (double) num)
        return false;
      bool3 bool3_1 = triangle1.m_Indices.yzx - triangle1.m_Indices.zxy > 1;
      bool3 bool3_2 = triangle2.m_Indices.yzx - triangle2.m_Indices.zxy > 1;
      bool3 bool3_3 = triangle3.m_Indices.yzx - triangle3.m_Indices.zxy > 1;
      bool3 bool3_4 = triangle4.m_Indices.yzx - triangle4.m_Indices.zxy > 1;
      if (bool3_1.x)
        edgeMap.Remove(triangle1.m_Indices.yz);
      if (bool3_1.y)
        edgeMap.Remove(triangle1.m_Indices.zx);
      if (bool3_1.z)
        edgeMap.Remove(triangle1.m_Indices.xy);
      if (bool3_2.x)
        edgeMap.Remove(triangle2.m_Indices.yz);
      if (bool3_2.y)
        edgeMap.Remove(triangle2.m_Indices.zx);
      if (bool3_2.z)
        edgeMap.Remove(triangle2.m_Indices.xy);
      if (bool3_3.x)
        edgeMap.TryAdd(triangle3.m_Indices.yz, new int2(index1.x, 0));
      if (bool3_3.y)
        edgeMap.TryAdd(triangle3.m_Indices.zx, new int2(index1.x, 1));
      if (bool3_3.z)
        edgeMap.TryAdd(triangle3.m_Indices.xy, new int2(index1.x, 2));
      if (bool3_4.x)
        edgeMap.TryAdd(triangle4.m_Indices.yz, new int2(index2.x, 0));
      if (bool3_4.y)
        edgeMap.TryAdd(triangle4.m_Indices.zx, new int2(index2.x, 1));
      if (bool3_4.z)
        edgeMap.TryAdd(triangle4.m_Indices.xy, new int2(index2.x, 2));
      triangles[index1.x] = triangle3;
      triangles[index2.x] = triangle4;
      return true;
    }

    private static float GetEqualizationValue(NativeArray<float3> nodes, Triangle triangle)
    {
      Triangle2 triangle2;
      ref Triangle2 local = ref triangle2;
      float3 node = nodes[triangle.m_Indices.x];
      float2 xz1 = node.xz;
      node = nodes[triangle.m_Indices.y];
      float2 xz2 = node.xz;
      node = nodes[triangle.m_Indices.z];
      float2 xz3 = node.xz;
      local = new Triangle2(xz1, xz2, xz3);
      float3 x = new float3(triangle2.a.x, triangle2.b.x, triangle2.c.x);
      float3 float3_1 = new float3(triangle2.a.y, triangle2.b.y, triangle2.c.y);
      float3 float3_2 = x - x.yzx;
      float3 float3_3 = float3_1 - float3_1.yzx;
      double num1 = (double) math.dot(x, float3_1.yzx - float3_1.zxy) * 0.5;
      float num2 = math.csum(math.sqrt(float3_2 * float3_2 + float3_3 * float3_3));
      double num3 = (double) num2 * (double) num2;
      return (float) (num1 / num3);
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
    public GeometrySystem()
    {
    }

    [BurstCompile]
    private struct TriangulateAreasJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Space> m_SpaceData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<TerrainAreaData> m_PrefabTerrainAreaData;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_PrefabAreaGeometryData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Areas.Area> m_AreaData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Geometry> m_GeometryData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Node> m_Nodes;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Triangle> m_Triangles;
      [ReadOnly]
      public bool m_Loaded;
      [ReadOnly]
      public NativeArray<Entity> m_Entities;
      [ReadOnly]
      public NativeList<Entity> m_Buildings;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_Entities[index];
        // ISSUE: reference to a compiler-generated field
        Game.Areas.Area area = this.m_AreaData[entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Node> node = this.m_Nodes[entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Triangle> triangle = this.m_Triangles[entity];
        // ISSUE: reference to a compiler-generated field
        if ((area.m_Flags & AreaFlags.Slave) != (AreaFlags) 0 && this.m_UpdatedData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated method
          this.GenerateSlaveArea(entity, ref area, node);
        }
        bool isComplete = (area.m_Flags & AreaFlags.Complete) != 0;
        // ISSUE: reference to a compiler-generated method
        bool isCounterClockwise = (double) GeometrySystem.Area(node) > 0.0;
        NativeArray<float3> nativeArray = new NativeArray<float3>(node.Length, Allocator.Temp);
        for (int index1 = 0; index1 < node.Length; ++index1)
          nativeArray[index1] = AreaUtils.GetExpandedNode(node, index1, -0.1f, isComplete, isCounterClockwise);
        NativeArray<Bounds2> edgeBounds = new NativeArray<Bounds2>();
        int totalDepth = 0;
        if (node.Length > 20)
        {
          // ISSUE: reference to a compiler-generated method
          GeometrySystem.BuildEdgeBounds(node, nativeArray, out edgeBounds, out totalDepth);
        }
        // ISSUE: reference to a compiler-generated method
        GeometrySystem.Triangulate<DynamicBuffer<Triangle>>(nativeArray, triangle, edgeBounds, totalDepth, isCounterClockwise);
        // ISSUE: reference to a compiler-generated method
        GeometrySystem.EqualizeTriangles<DynamicBuffer<Triangle>>(nativeArray, triangle);
        nativeArray.Dispose();
        if (isCounterClockwise)
          area.m_Flags |= AreaFlags.CounterClockwise;
        else
          area.m_Flags &= ~AreaFlags.CounterClockwise;
        if (triangle.Length == 0)
          area.m_Flags |= AreaFlags.NoTriangles;
        else
          area.m_Flags &= ~AreaFlags.NoTriangles;
        // ISSUE: reference to a compiler-generated field
        this.m_AreaData[entity] = area;
        // ISSUE: reference to a compiler-generated field
        if (this.m_GeometryData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          bool useTerrainHeight = !this.m_SpaceData.HasComponent(entity);
          bool useWaterHeight = false;
          // ISSUE: reference to a compiler-generated method
          bool useTriangleHeight = !useTerrainHeight || this.HasBuildingOwner(entity);
          float heightOffset = 0.0f;
          float nodeDistance = 0.0f;
          float lodBias = 0.0f;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[entity];
          TerrainAreaData componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabTerrainAreaData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
            heightOffset = componentData1.m_HeightOffset;
          AreaGeometryData componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabAreaGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
          {
            useWaterHeight = (componentData2.m_Flags & GeometryFlags.OnWaterSurface) != 0;
            nodeDistance = AreaUtils.GetMinNodeDistance(componentData2.m_Type);
            lodBias = componentData2.m_LodBias;
          }
          // ISSUE: reference to a compiler-generated method
          this.UpdateHeightRange(node, triangle, useTerrainHeight, useWaterHeight, useTriangleHeight, heightOffset);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_GeometryData[entity] = this.CalculateGeometry(node, triangle, edgeBounds, totalDepth, nodeDistance, lodBias, useTerrainHeight, useWaterHeight);
        }
        if (!edgeBounds.IsCreated)
          return;
        edgeBounds.Dispose();
      }

      private void GenerateSlaveArea(Entity entity, ref Game.Areas.Area area, DynamicBuffer<Node> nodes)
      {
        nodes.Clear();
        Owner componentData1;
        Game.Areas.Area componentData2;
        DynamicBuffer<Node> bufferData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_OwnerData.TryGetComponent(entity, out componentData1) || !this.m_AreaData.TryGetComponent(componentData1.m_Owner, out componentData2) || !this.m_Nodes.TryGetBuffer(componentData1.m_Owner, out bufferData1) || bufferData1.Length < 3)
          return;
        if ((componentData2.m_Flags & AreaFlags.Complete) != (AreaFlags) 0)
          area.m_Flags |= AreaFlags.Complete;
        else
          area.m_Flags &= ~AreaFlags.Complete;
        nodes.CopyFrom(bufferData1);
        // ISSUE: reference to a compiler-generated method
        bool isCounterClockwise = (double) GeometrySystem.Area(nodes) > 0.0;
        NativeList<Node> extraNodes = new NativeList<Node>(128, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<int2> extraRanges = new NativeList<int2>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        DynamicBuffer<Game.Objects.SubObject> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubObjects.TryGetBuffer(componentData1.m_Owner, out bufferData2))
        {
          for (int index = 0; index < bufferData2.Length; ++index)
          {
            Game.Objects.SubObject subObject = bufferData2[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingData.HasComponent(subObject.m_SubObject) && !this.m_DeletedData.HasComponent(subObject.m_SubObject))
            {
              // ISSUE: reference to a compiler-generated method
              this.AddObjectHole(subObject.m_SubObject, extraNodes, extraRanges, isCounterClockwise);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Buildings.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Entity building = this.m_Buildings[index];
          // ISSUE: reference to a compiler-generated field
          if (!(this.m_OwnerData[building].m_Owner != componentData1.m_Owner))
          {
            // ISSUE: reference to a compiler-generated method
            this.AddObjectHole(building, extraNodes, extraRanges, isCounterClockwise);
          }
        }
        for (int index1 = extraRanges.Length - 1; index1 >= 0; --index1)
        {
          int2 int2_1 = extraRanges[index1];
          int3 int3 = (int3) -1;
          float num1 = float.MaxValue;
          for (int x1 = int2_1.x; x1 < int2_1.y; ++x1)
          {
            Node node1 = extraNodes[x1];
            for (int index2 = 0; index2 < nodes.Length; ++index2)
            {
              Node node2 = nodes[index2];
              Line2.Segment newEdge = new Line2.Segment(node1.m_Position.xz, node2.m_Position.xz);
              float num2 = math.distancesq(newEdge.a, newEdge.b);
              // ISSUE: reference to a compiler-generated method
              if ((double) num2 < (double) num1 && this.CanAddEdge(newEdge, nodes, extraNodes, extraRanges, index1, new int4(-1, index2, index1, x1)))
              {
                num1 = num2;
                int3 = new int3(x1, -1, index2);
              }
            }
            for (int index3 = 0; index3 < index1; ++index3)
            {
              int2 int2_2 = extraRanges[index3];
              for (int x2 = int2_2.x; x2 < int2_2.y; ++x2)
              {
                Node node3 = extraNodes[x2];
                Line2.Segment newEdge = new Line2.Segment(node1.m_Position.xz, node3.m_Position.xz);
                float num3 = math.distancesq(newEdge.a, newEdge.b);
                // ISSUE: reference to a compiler-generated method
                if ((double) num3 < (double) num1 && this.CanAddEdge(newEdge, nodes, extraNodes, extraRanges, index1, new int4(index3, x2, index1, x1)))
                {
                  num1 = num3;
                  int3 = new int3(x1, index3, x2);
                }
              }
            }
          }
          if (int3.x != -1)
          {
            int num4 = 2 + int2_1.y - int2_1.x;
            if (int3.y == -1)
            {
              Node node4 = nodes[int3.z];
              int num5 = math.select(int3.z, nodes.Length, int3.z == 0);
              nodes.ResizeUninitialized(nodes.Length + num4);
              for (int index4 = nodes.Length - num4 - 1; index4 >= num5; --index4)
                nodes[index4 + num4] = nodes[index4];
              ref DynamicBuffer<Node> local = ref nodes;
              int index5 = num5;
              int num6 = index5 + 1;
              Node node5 = node4;
              local[index5] = node5;
              for (int x = int3.x; x < int2_1.y; ++x)
                nodes[num6++] = extraNodes[x];
              for (int x = int2_1.x; x <= int3.x; ++x)
                nodes[num6++] = extraNodes[x];
            }
            else
            {
              int2 int2_3 = extraRanges[int3.y];
              Node node6 = extraNodes[int3.z];
              int num7 = math.select(int3.z, int2_3.y, int3.z == int2_3.x);
              int3.x += num4;
              int2 int2_4 = int2_1 + num4;
              int2_3.y += num4;
              extraRanges[int3.y] = int2_3;
              extraNodes.ResizeUninitialized(int2_4.y);
              for (int index6 = int2_4.y - num4 - 1; index6 >= num7; --index6)
                extraNodes[index6 + num4] = extraNodes[index6];
              for (int index7 = int3.y + 1; index7 < index1; ++index7)
                extraRanges[index7] += num4;
              ref NativeList<Node> local = ref extraNodes;
              int index8 = num7;
              int num8 = index8 + 1;
              Node node7 = node6;
              local[index8] = node7;
              for (int x = int3.x; x < int2_4.y; ++x)
                extraNodes[num8++] = extraNodes[x];
              for (int x = int2_4.x; x <= int3.x; ++x)
                extraNodes[num8++] = extraNodes[x];
            }
          }
        }
        extraNodes.Dispose();
        extraRanges.Dispose();
      }

      private void AddObjectHole(
        Entity objectEntity,
        NativeList<Node> extraNodes,
        NativeList<int2> extraRanges,
        bool isCounterClockwise)
      {
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[objectEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[this.m_PrefabRefData[objectEntity].m_Prefab];
        Quad3 baseCorners = ObjectUtils.CalculateBaseCorners(transform.m_Position, transform.m_Rotation, objectGeometryData.m_Bounds);
        int2 length = (int2) extraNodes.Length;
        if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Circular) != Game.Objects.GeometryFlags.None)
        {
          float num1 = objectGeometryData.m_Size.x * 0.5f;
          float a = 0.7853982f;
          float num2 = math.select(a, -a, isCounterClockwise);
          for (int index = 0; index < 8; ++index)
          {
            float x = (float) index * num2;
            Node node = new Node(transform.m_Position, float.MinValue);
            node.m_Position.x += math.cos(x) * num1;
            node.m_Position.z += math.sin(x) * num1;
            extraNodes.Add(in node);
          }
        }
        else if (isCounterClockwise)
        {
          extraNodes.Add(new Node(baseCorners.a, float.MinValue));
          extraNodes.Add(new Node(baseCorners.b, float.MinValue));
          extraNodes.Add(new Node(baseCorners.c, float.MinValue));
          extraNodes.Add(new Node(baseCorners.d, float.MinValue));
        }
        else
        {
          extraNodes.Add(new Node(baseCorners.b, float.MinValue));
          extraNodes.Add(new Node(baseCorners.a, float.MinValue));
          extraNodes.Add(new Node(baseCorners.d, float.MinValue));
          extraNodes.Add(new Node(baseCorners.c, float.MinValue));
        }
        length.y = extraNodes.Length;
        extraRanges.Add(in length);
      }

      private bool CanAddEdge(
        Line2.Segment newEdge,
        DynamicBuffer<Node> nodes,
        NativeList<Node> extraNodes,
        NativeList<int2> extraRanges,
        int extraRangeLimit,
        int4 ignoreIndex)
      {
        Line2.Segment line1;
        line1.a = nodes[nodes.Length - 1].m_Position.xz;
        float2 t;
        for (int index = 0; index < nodes.Length; ++index)
        {
          line1.b = nodes[index].m_Position.xz;
          if (MathUtils.Intersect(line1, newEdge, out t) && ignoreIndex.x != -1 | index != ignoreIndex.y & math.select(index, nodes.Length, index == 0) - 1 != ignoreIndex.y)
            return false;
          line1.a = line1.b;
        }
        for (int index = 0; index <= extraRangeLimit; ++index)
        {
          int2 extraRange = extraRanges[index];
          line1.a = extraNodes[extraRange.y - 1].m_Position.xz;
          for (int x = extraRange.x; x < extraRange.y; ++x)
          {
            line1.b = extraNodes[x].m_Position.xz;
            if (MathUtils.Intersect(line1, newEdge, out t) && math.all(ignoreIndex.xz != index | x != ignoreIndex.yw & math.select(x, extraRange.y, x == extraRange.x) - 1 != ignoreIndex.yw))
              return false;
            line1.a = line1.b;
          }
        }
        return true;
      }

      private bool HasBuildingOwner(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          entity = this.m_OwnerData[entity].m_Owner;
          // ISSUE: reference to a compiler-generated field
          if (this.m_BuildingData.HasComponent(entity))
            return true;
        }
        return false;
      }

      private void UpdateHeightRange(
        DynamicBuffer<Node> nodes,
        DynamicBuffer<Triangle> triangles,
        bool useTerrainHeight,
        bool useWaterHeight,
        bool useTriangleHeight,
        float heightOffset)
      {
        for (int index = 0; index < triangles.Length; ++index)
        {
          Triangle triangle = triangles[index];
          Triangle3 triangle3 = AreaUtils.GetTriangle3(nodes, triangle);
          Bounds1 bounds1 = new Bounds1(math.min(0.0f, heightOffset), math.max(0.0f, heightOffset));
          if (useTerrainHeight)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            triangle.m_HeightRange = GeometrySystem.TriangulateAreasJob.GetHeightRange(ref this.m_TerrainHeightData, triangle3);
            if ((double) triangle.m_HeightRange.min > (double) triangle.m_HeightRange.max)
              triangle.m_HeightRange = bounds1;
            else if (useTriangleHeight)
              triangle.m_HeightRange |= bounds1;
            if (useWaterHeight)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              triangle.m_HeightRange |= WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, triangle3.a) - triangle3.a.y;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              triangle.m_HeightRange |= WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, triangle3.b) - triangle3.b.y;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              triangle.m_HeightRange |= WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, triangle3.c) - triangle3.c.y;
            }
          }
          else
            triangle.m_HeightRange = bounds1;
          triangles[index] = triangle;
        }
      }

      private static Bounds1 GetHeightRange(ref TerrainHeightData data, Triangle3 triangle3)
      {
        triangle3.a = TerrainUtils.ToHeightmapSpace(ref data, triangle3.a);
        triangle3.b = TerrainUtils.ToHeightmapSpace(ref data, triangle3.b);
        triangle3.c = TerrainUtils.ToHeightmapSpace(ref data, triangle3.c);
        if ((double) triangle3.b.z < (double) triangle3.a.z)
          CommonUtils.Swap<float3>(ref triangle3.b, ref triangle3.a);
        if ((double) triangle3.c.z < (double) triangle3.a.z)
          CommonUtils.Swap<float3>(ref triangle3.c, ref triangle3.a);
        if ((double) triangle3.c.z < (double) triangle3.b.z)
          CommonUtils.Swap<float3>(ref triangle3.c, ref triangle3.b);
        int z = math.max(0, (int) math.floor(triangle3.a.z));
        int num1 = math.min(data.resolution.z - 1, (int) math.ceil(triangle3.c.z));
        Bounds1 heightRange = new Bounds1(float.MaxValue, float.MinValue);
        if (num1 >= z)
        {
          float zFactorAC = (float) (1.0 / ((double) triangle3.c.z - (double) triangle3.a.z));
          float zFactorAB = (float) (1.0 / ((double) triangle3.b.z - (double) triangle3.a.z));
          float zFactorBC = (float) (1.0 / ((double) triangle3.c.z - (double) triangle3.b.z));
          (float2 left1, float2 right1) = GetLeftRight((float) z);
          float2 float2_1 = left1;
          float2 float2_2 = right1;
          float2 float2_3 = float2_1;
          float2 float2_4 = float2_2;
          for (int index1 = z; index1 <= num1; ++index1)
          {
            float2 left2 = left1;
            float2 right2 = right1;
            float2 float2_5 = new float2(math.min(left1.x, float2_3.x), math.max(right1.x, float2_4.x));
            if (index1 < num1)
            {
              (left2, right2) = GetLeftRight((float) (index1 + 1));
              float2_5 = new float2(math.min(float2_5.x, left2.x), math.max(float2_5.x, right2.x));
            }
            int num2 = math.max(0, (int) math.floor(float2_5.x));
            int num3 = math.min(data.resolution.x - 1, (int) math.ceil(float2_5.y));
            float num4 = (float) (1.0 / ((double) right1.x - (double) left1.x));
            int num5 = index1 * data.resolution.x;
            for (int index2 = num2; index2 <= num3; ++index2)
            {
              float s = math.saturate(((float) index2 - left1.x) * num4);
              float num6 = math.lerp(left1.y, right1.y, s);
              float height = (float) data.heights[num5 + index2];
              heightRange |= height - num6;
            }
            float2 float2_6 = left1;
            float2 float2_7 = right1;
            float2_3 = float2_6;
            float2_4 = float2_7;
            float2 float2_8 = left2;
            float2 float2_9 = right2;
            left1 = float2_8;
            right1 = float2_9;
          }

          (float2 left, float2 right) GetLeftRight(float z)
          {
            float s1 = math.saturate((z - triangle3.a.z) * zFactorAC);
            float2 a = math.lerp(triangle3.a.xy, triangle3.c.xy, s1);
            float2 b;
            if ((double) z <= (double) triangle3.b.z)
            {
              float s2 = math.saturate((z - triangle3.a.z) * zFactorAB);
              b = math.lerp(triangle3.a.xy, triangle3.b.xy, s2);
            }
            else
            {
              float s3 = math.saturate((z - triangle3.b.z) * zFactorBC);
              b = math.lerp(triangle3.b.xy, triangle3.c.xy, s3);
            }
            if ((double) b.x < (double) a.x)
              CommonUtils.Swap<float2>(ref a, ref b);
            return (a, b);
          }
        }
        heightRange.min /= data.scale.y;
        heightRange.max /= data.scale.y;
        return heightRange;
      }

      private Geometry CalculateGeometry(
        DynamicBuffer<Node> nodes,
        DynamicBuffer<Triangle> triangles,
        NativeArray<Bounds2> edgeBounds,
        int totalDepth,
        float nodeDistance,
        float lodBias,
        bool useTerrainHeight,
        bool useWaterHeight)
      {
        Geometry geometry = new Geometry();
        geometry.m_Bounds.min = (float3) float.MaxValue;
        geometry.m_Bounds.max = (float3) float.MinValue;
        if (triangles.Length != 0)
        {
          float num = -1f;
          for (int index = 0; index < triangles.Length; ++index)
          {
            ref Triangle local = ref triangles.ElementAt(index);
            Triangle3 triangle3 = AreaUtils.GetTriangle3(nodes, local);
            geometry.m_Bounds |= MathUtils.Bounds(triangle3);
            geometry.m_SurfaceArea += MathUtils.Area(triangle3.xz);
            int3 int3 = math.abs(local.m_Indices.zxy - local.m_Indices.yzx);
            bool3 bool3_1 = int3 == 1 | int3 == nodes.Length - 1;
            bool3 bool3_2 = !bool3_1;
            float2 bestMinDistance = (float2) -1f;
            float3 bestPosition = new float3();
            if (bool3_2.x)
            {
              float3 position = math.lerp(triangle3.b, triangle3.c, 0.5f);
              // ISSUE: reference to a compiler-generated method
              this.CheckCenterPositionCandidate(ref bestMinDistance, ref bestPosition, position, triangle3, nodes, edgeBounds, totalDepth);
            }
            if (bool3_2.y)
            {
              float3 position = math.lerp(triangle3.c, triangle3.a, 0.5f);
              // ISSUE: reference to a compiler-generated method
              this.CheckCenterPositionCandidate(ref bestMinDistance, ref bestPosition, position, triangle3, nodes, edgeBounds, totalDepth);
            }
            if (bool3_2.z)
            {
              float3 position = math.lerp(triangle3.a, triangle3.b, 0.5f);
              // ISSUE: reference to a compiler-generated method
              this.CheckCenterPositionCandidate(ref bestMinDistance, ref bestPosition, position, triangle3, nodes, edgeBounds, totalDepth);
            }
            if (math.all(bool3_2.xy) & bool3_1.z)
            {
              float3 position = triangle3.c * 0.5f + (triangle3.a + triangle3.b) * 0.25f;
              // ISSUE: reference to a compiler-generated method
              this.CheckCenterPositionCandidate(ref bestMinDistance, ref bestPosition, position, triangle3, nodes, edgeBounds, totalDepth);
            }
            else if (math.all(bool3_2.yz) & bool3_1.x)
            {
              float3 position = triangle3.a * 0.5f + (triangle3.b + triangle3.c) * 0.25f;
              // ISSUE: reference to a compiler-generated method
              this.CheckCenterPositionCandidate(ref bestMinDistance, ref bestPosition, position, triangle3, nodes, edgeBounds, totalDepth);
            }
            else if (math.all(bool3_2.zx) & bool3_1.y)
            {
              float3 position = triangle3.b * 0.5f + (triangle3.c + triangle3.a) * 0.25f;
              // ISSUE: reference to a compiler-generated method
              this.CheckCenterPositionCandidate(ref bestMinDistance, ref bestPosition, position, triangle3, nodes, edgeBounds, totalDepth);
            }
            else
            {
              float3 position = (triangle3.a + triangle3.b + triangle3.c) * 0.333333343f;
              // ISSUE: reference to a compiler-generated method
              this.CheckCenterPositionCandidate(ref bestMinDistance, ref bestPosition, position, triangle3, nodes, edgeBounds, totalDepth);
            }
            float2 float2 = math.sqrt(bestMinDistance) * 4f;
            local.m_MinLod = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float3(float2.x, nodeDistance, float2.y)), lodBias);
            if ((double) bestMinDistance.x > (double) num)
            {
              num = bestMinDistance.x;
              geometry.m_CenterPosition = bestPosition;
            }
          }
        }
        else if (nodes.Length != 0)
        {
          for (int index = 0; index < nodes.Length; ++index)
          {
            float3 position = nodes[index].m_Position;
            geometry.m_Bounds |= position;
            geometry.m_CenterPosition += position;
          }
          geometry.m_CenterPosition /= (float) nodes.Length;
        }
        if (useTerrainHeight)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          geometry.m_CenterPosition.y = !useWaterHeight ? TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, geometry.m_CenterPosition) : WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, geometry.m_CenterPosition);
        }
        return geometry;
      }

      private void CheckCenterPositionCandidate(
        ref float2 bestMinDistance,
        ref float3 bestPosition,
        float3 position,
        Triangle3 triangle,
        DynamicBuffer<Node> nodes,
        NativeArray<Bounds2> edgeBounds,
        int totalDepth)
      {
        float2 a1 = (float2) float.MaxValue;
        float t;
        if (edgeBounds.IsCreated)
        {
          float num1 = math.sqrt(math.max(math.distancesq(position.xz, triangle.a.xz), math.max(math.distancesq(position.xz, triangle.b.xz), math.distancesq(position.xz, triangle.c.xz)))) + 0.1f;
          Bounds2 bounds2 = new Bounds2(position.xz - num1, position.xz + num1);
          int num2 = 0;
          int num3 = 1;
          int num4 = 0;
          int length = nodes.Length;
          while (num3 > 0)
          {
            if (MathUtils.Intersect(edgeBounds[num2 + num4], bounds2))
            {
              if (num3 == totalDepth)
              {
                int num5 = num4 * length >> num3;
                int num6 = (num4 + 1) * length >> num3;
                Line2.Segment line;
                ref Line2.Segment local1 = ref line;
                ref DynamicBuffer<Node> local2 = ref nodes;
                int index = num5;
                int num7 = index + 1;
                float2 xz = local2[index].m_Position.xz;
                local1.a = xz;
                for (int a2 = num7; a2 <= num6; ++a2)
                {
                  line.b = nodes[math.select(a2, 0, a2 == length)].m_Position.xz;
                  float num8 = MathUtils.DistanceSquared(line, position.xz, out t);
                  a1.y = math.select(a1.y, num8, (double) num8 < (double) a1.y);
                  a1 = math.select(a1, new float2(num8, a1.x), (double) num8 < (double) a1.x);
                  line.a = line.b;
                }
              }
              else
              {
                num4 <<= 1;
                num2 += 1 << num3++;
                continue;
              }
            }
            while ((num4 & 1) != 0)
            {
              num4 >>= 1;
              num2 -= 1 << --num3;
            }
            ++num4;
          }
        }
        else
        {
          Line2.Segment line = new Line2.Segment();
          line.a = nodes[nodes.Length - 1].m_Position.xz;
          for (int index = 0; index < nodes.Length; ++index)
          {
            line.b = nodes[index].m_Position.xz;
            float num = MathUtils.DistanceSquared(line, position.xz, out t);
            a1.y = math.select(a1.y, num, (double) num < (double) a1.y);
            a1 = math.select(a1, new float2(num, a1.x), (double) num < (double) a1.x);
            line.a = line.b;
          }
        }
        if ((double) a1.x <= (double) bestMinDistance.x)
          return;
        bestMinDistance = a1;
        bestPosition = position;
      }
    }

    private struct Index
    {
      public int m_NodeIndex;
      public int m_PrevIndex;
      public int m_NextIndex;
      public int m_SkipIndex;
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Space> __Game_Areas_Space_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TerrainAreaData> __Game_Prefabs_TerrainAreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      public ComponentLookup<Game.Areas.Area> __Game_Areas_Area_RW_ComponentLookup;
      public ComponentLookup<Geometry> __Game_Areas_Geometry_RW_ComponentLookup;
      public BufferLookup<Node> __Game_Areas_Node_RW_BufferLookup;
      public BufferLookup<Triangle> __Game_Areas_Triangle_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Space_RO_ComponentLookup = state.GetComponentLookup<Space>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TerrainAreaData_RO_ComponentLookup = state.GetComponentLookup<TerrainAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Area_RW_ComponentLookup = state.GetComponentLookup<Game.Areas.Area>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Geometry_RW_ComponentLookup = state.GetComponentLookup<Geometry>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RW_BufferLookup = state.GetBufferLookup<Node>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RW_BufferLookup = state.GetBufferLookup<Triangle>();
      }
    }
  }
}
