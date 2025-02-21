// Decompiled with JetBrains decompiler
// Type: Game.Rendering.CameraCollisionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.City;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class CameraCollisionSystem : GameSystemBase
  {
    private CityConfigurationSystem m_CityConfigurationSystem;
    private ToolSystem m_ToolSystem;
    private SearchSystem m_ObjectSearchSystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private float3 m_PreviousPosition;
    private quaternion m_Rotation;
    private float m_MaxForwardOffset;
    private float m_MaxBackwardOffset;
    private float m_MinClearDistance;
    private float m_NearPlane;
    private float m_Smoothing;
    private float2 m_FieldOfView;
    private NativeReference<CameraCollisionSystem.Result> m_Result;
    private CameraCollisionSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_Result = new NativeReference<CameraCollisionSystem.Result>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Result.Dispose();
      base.OnDestroy();
    }

    public void CheckCollisions(
      ref float3 position,
      float3 previousPosition,
      quaternion rotation,
      float maxForwardOffset,
      float maxBackwardOffset,
      float minClearDistance,
      float nearPlane,
      float smoothing,
      float2 fieldOfView)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.actionMode.IsEditor())
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Result.ValueAsRef<CameraCollisionSystem.Result>().m_Position = position;
      // ISSUE: reference to a compiler-generated field
      this.m_PreviousPosition = previousPosition;
      // ISSUE: reference to a compiler-generated field
      this.m_Rotation = rotation;
      // ISSUE: reference to a compiler-generated field
      this.m_MaxForwardOffset = maxForwardOffset;
      // ISSUE: reference to a compiler-generated field
      this.m_MaxBackwardOffset = maxBackwardOffset;
      // ISSUE: reference to a compiler-generated field
      this.m_MinClearDistance = minClearDistance;
      // ISSUE: reference to a compiler-generated field
      this.m_NearPlane = nearPlane;
      // ISSUE: reference to a compiler-generated field
      this.m_Smoothing = smoothing;
      // ISSUE: reference to a compiler-generated field
      this.m_FieldOfView = fieldOfView;
      this.Update();
      // ISSUE: reference to a compiler-generated field
      ref CameraCollisionSystem.Result local = ref this.m_Result.ValueAsRef<CameraCollisionSystem.Result>();
      // ISSUE: reference to a compiler-generated field
      position = local.m_Position;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float3 position = this.m_Result.Value.m_Position;
      // ISSUE: reference to a compiler-generated field
      float3 float3 = math.forward(this.m_Rotation);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float num1 = this.m_MaxForwardOffset + this.m_MinClearDistance;
      // ISSUE: reference to a compiler-generated field
      float maxBackwardOffset = this.m_MaxBackwardOffset;
      Line3.Segment segment = new Line3.Segment(position - float3 * maxBackwardOffset, position + float3 * num1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float2 float2 = math.tan(math.radians(this.m_FieldOfView) * 0.5f) * this.m_MinClearDistance;
      // ISSUE: reference to a compiler-generated field
      float num2 = this.m_MinClearDistance / (num1 + maxBackwardOffset);
      // ISSUE: reference to a compiler-generated field
      float num3 = this.m_NearPlane / (num1 + maxBackwardOffset);
      NativeList<Entity> list = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<CameraCollisionSystem.Collision> nativeQueue = new NativeQueue<CameraCollisionSystem.Collision>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CameraCollisionSystem.FindEntitiesFromTreeJob jobData1 = new CameraCollisionSystem.FindEntitiesFromTreeJob()
      {
        m_Line = segment,
        m_Rotation = this.m_Rotation,
        m_FovOffset = float2,
        m_SearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies),
        m_EntityList = list
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MeshNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MeshIndex_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MeshVertex_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LodMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Bone_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Skeleton_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GrowthScaleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SharedMeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ImpostorData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stack_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_NetObject_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      CameraCollisionSystem.ObjectCollisionJob jobData2 = new CameraCollisionSystem.ObjectCollisionJob()
      {
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_TreeData = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup,
        m_NetObjectData = this.__TypeHandle.__Game_Objects_NetObject_RO_ComponentLookup,
        m_QuantityData = this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup,
        m_StackData = this.__TypeHandle.__Game_Objects_Stack_RO_ComponentLookup,
        m_UnderConstructionData = this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentLookup,
        m_OutsideConnectionData = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabMeshData = this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup,
        m_PrefabImpostorData = this.__TypeHandle.__Game_Prefabs_ImpostorData_RO_ComponentLookup,
        m_PrefabSharedMeshData = this.__TypeHandle.__Game_Prefabs_SharedMeshData_RO_ComponentLookup,
        m_PrefabGrowthScaleData = this.__TypeHandle.__Game_Prefabs_GrowthScaleData_RO_ComponentLookup,
        m_PrefabQuantityObjectData = this.__TypeHandle.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup,
        m_PrefabStackData = this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup,
        m_MeshGroups = this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup,
        m_Skeletons = this.__TypeHandle.__Game_Rendering_Skeleton_RO_BufferLookup,
        m_Bones = this.__TypeHandle.__Game_Rendering_Bone_RO_BufferLookup,
        m_Meshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_SubMeshGroups = this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup,
        m_Lods = this.__TypeHandle.__Game_Prefabs_LodMesh_RO_BufferLookup,
        m_Vertices = this.__TypeHandle.__Game_Prefabs_MeshVertex_RO_BufferLookup,
        m_Indices = this.__TypeHandle.__Game_Prefabs_MeshIndex_RO_BufferLookup,
        m_Nodes = this.__TypeHandle.__Game_Prefabs_MeshNode_RO_BufferLookup,
        m_ProceduralBones = this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup,
        m_Line = segment,
        m_Rotation = this.m_Rotation,
        m_FovOffset = float2,
        m_MinClearRange = num2,
        m_LeftHandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_EntityList = list,
        m_Collisions = nativeQueue.AsParallelWriter()
      };
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CameraCollisionSystem.SelectCameraPositionJob jobData3 = new CameraCollisionSystem.SelectCameraPositionJob()
      {
        m_Line = segment,
        m_PreviousPosition = this.m_PreviousPosition,
        m_MinClearRange = num2,
        m_NearPlaneRange = num3,
        m_Smoothing = this.m_Smoothing,
        m_DeltaTime = UnityEngine.Time.deltaTime,
        m_TerrainData = this.m_TerrainSystem.GetHeightData(),
        m_WaterData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_Collisions = nativeQueue,
        m_Result = this.m_Result
      };
      JobHandle jobHandle1 = jobData1.Schedule<CameraCollisionSystem.FindEntitiesFromTreeJob>(dependencies);
      JobHandle jobHandle2 = jobData2.Schedule<CameraCollisionSystem.ObjectCollisionJob, Entity>(list, 1, JobHandle.CombineDependencies(this.Dependency, jobHandle1));
      JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle2, deps);
      JobHandle jobHandle3 = jobData3.Schedule<CameraCollisionSystem.SelectCameraPositionJob>(dependsOn);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(jobHandle3);
      list.Dispose(jobHandle2);
      nativeQueue.Dispose(jobHandle3);
      jobHandle3.Complete();
    }

    private static void CheckCollisions(
      NativeList<CameraCollisionSystem.Collision> collisions,
      float minClearRange,
      float2 limits)
    {
      if (collisions.Length == 0)
        return;
      collisions.Sort<CameraCollisionSystem.Collision>();
      int num1 = 0;
      // ISSUE: variable of a compiler-generated type
      CameraCollisionSystem.Collision collision1 = collisions[0];
      for (int index = 1; index < collisions.Length; ++index)
      {
        // ISSUE: variable of a compiler-generated type
        CameraCollisionSystem.Collision collision2 = collisions[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((double) collision2.m_LineBounds.min - (double) collision1.m_LineBounds.max < (double) minClearRange)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          collision1.m_LineBounds.max = math.max(collision1.m_LineBounds.max, collision2.m_LineBounds.max);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          collision1.m_CoverAreas += collision2.m_CoverAreas;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          collision1.m_StartEnd |= collision2.m_StartEnd;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          collision1.m_StartEnd &= collision1.m_CoverAreas >= collision1.m_CoverAreas.yx * 0.5f;
          collisions[num1++] = collision1;
          collision1 = collision2;
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      collision1.m_StartEnd &= collision1.m_CoverAreas >= collision1.m_CoverAreas.yx * 0.5f;
      ref NativeList<CameraCollisionSystem.Collision> local1 = ref collisions;
      int index1 = num1;
      int index2 = index1 + 1;
      // ISSUE: variable of a compiler-generated type
      CameraCollisionSystem.Collision collision3 = collision1;
      local1[index1] = collision3;
      collisions.RemoveRange(index2, collisions.Length - index2);
      int num2 = 0;
      collision1 = collisions[0];
      // ISSUE: reference to a compiler-generated field
      if (!collision1.m_StartEnd.x)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        collision1.m_LineBounds.min = math.min(collision1.m_LineBounds.min, limits.x);
        // ISSUE: reference to a compiler-generated field
        collision1.m_StartEnd.x = true;
      }
      for (int index3 = 1; index3 < collisions.Length; ++index3)
      {
        // ISSUE: variable of a compiler-generated type
        CameraCollisionSystem.Collision collision4 = collisions[index3];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!collision1.m_StartEnd.y || !collision4.m_StartEnd.x)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          collision1.m_LineBounds.max = collision4.m_LineBounds.max;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          collision1.m_CoverAreas += collision4.m_CoverAreas;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          collision1.m_StartEnd.y = collision4.m_StartEnd.y;
        }
        else
        {
          collisions[num2++] = collision1;
          collision1 = collision4;
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (!collision1.m_StartEnd.y)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        collision1.m_LineBounds.max = math.max(collision1.m_LineBounds.max, limits.y);
        // ISSUE: reference to a compiler-generated field
        collision1.m_StartEnd.y = true;
      }
      ref NativeList<CameraCollisionSystem.Collision> local2 = ref collisions;
      int index4 = num2;
      int index5 = index4 + 1;
      // ISSUE: variable of a compiler-generated type
      CameraCollisionSystem.Collision collision5 = collision1;
      local2[index4] = collision5;
      collisions.RemoveRange(index5, collisions.Length - index5);
    }

    private static bool Intersect(CameraCollisionSystem.Line line, Bounds3 bounds, out float2 t)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      bounds = MathUtils.Expand(bounds, line.m_Expand) * line.m_Scale;
      // ISSUE: reference to a compiler-generated field
      return MathUtils.Intersect(bounds, line.m_Line, out t);
    }

    private static void CheckTriangleIntersect(
      CameraCollisionSystem.Line line,
      Triangle3 triangle,
      NativeList<CameraCollisionSystem.Collision> collisions)
    {
      // ISSUE: reference to a compiler-generated field
      triangle *= line.m_Scale;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!MathUtils.Intersect(MathUtils.Expand(MathUtils.Bounds(triangle), line.m_Expand), line.m_Line, out float2 _))
        return;
      // ISSUE: reference to a compiler-generated field
      float3 x1 = triangle.a - line.m_Line.a;
      // ISSUE: reference to a compiler-generated field
      float3 x2 = triangle.b - line.m_Line.a;
      // ISSUE: reference to a compiler-generated field
      float3 x3 = triangle.c - line.m_Line.a;
      Bounds2 bounds;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      bounds.max = new float2(math.lengthsq(line.m_XVector), math.lengthsq(line.m_YVector));
      bounds.min = -bounds.max;
      Triangle2 triangle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      triangle1.a = new float2(math.dot(x1, line.m_XVector), math.dot(x1, line.m_YVector));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      triangle1.b = new float2(math.dot(x2, line.m_XVector), math.dot(x2, line.m_YVector));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      triangle1.c = new float2(math.dot(x3, line.m_XVector), math.dot(x3, line.m_YVector));
      float area;
      if (!MathUtils.Intersect(bounds, triangle1, out area))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float3 x4 = line.m_Line.b - line.m_Line.a;
      float3 y = x4 * (1f / math.lengthsq(x4));
      Triangle1 triangle2;
      triangle2.a = math.dot(x1, y);
      triangle2.b = math.dot(x2, y);
      triangle2.c = math.dot(x3, y);
      Bounds1 bounds1 = new Bounds1(float.MaxValue, float.MinValue);
      float2 t;
      if (MathUtils.Intersect(bounds, triangle1.ab, out t))
      {
        t = math.lerp((float2) triangle2.a, (float2) triangle2.b, t);
        bounds1.min = math.min(bounds1.min, math.cmin(t));
        bounds1.max = math.max(bounds1.max, math.cmax(t));
      }
      if (MathUtils.Intersect(bounds, triangle1.bc, out t))
      {
        t = math.lerp((float2) triangle2.b, (float2) triangle2.c, t);
        bounds1.min = math.min(bounds1.min, math.cmin(t));
        bounds1.max = math.max(bounds1.max, math.cmax(t));
      }
      if (MathUtils.Intersect(bounds, triangle1.ca, out t))
      {
        t = math.lerp((float2) triangle2.c, (float2) triangle2.a, t);
        bounds1.min = math.min(bounds1.min, math.cmin(t));
        bounds1.max = math.max(bounds1.max, math.cmax(t));
      }
      if (MathUtils.Intersect(triangle1, bounds.min, out t))
        bounds1 |= MathUtils.Position(triangle2, t);
      if (MathUtils.Intersect(triangle1, new float2(bounds.max.x, bounds.min.y), out t))
        bounds1 |= MathUtils.Position(triangle2, t);
      if (MathUtils.Intersect(triangle1, new float2(bounds.min.x, bounds.max.y), out t))
        bounds1 |= MathUtils.Position(triangle2, t);
      if (MathUtils.Intersect(triangle1, bounds.max, out t))
        bounds1 |= MathUtils.Position(triangle2, t);
      if ((double) bounds1.min > 1.0 || (double) bounds1.max < 0.0)
        return;
      // ISSUE: variable of a compiler-generated type
      CameraCollisionSystem.Collision collision;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      collision.m_LineBounds.min = math.lerp(line.m_CutOffset.x, line.m_CutOffset.y, bounds1.min);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      collision.m_LineBounds.max = math.lerp(line.m_CutOffset.x, line.m_CutOffset.y, bounds1.max);
      if (MathUtils.IsClockwise(triangle1))
      {
        // ISSUE: reference to a compiler-generated field
        collision.m_CoverAreas = new float2(area, 0.0f);
        // ISSUE: reference to a compiler-generated field
        collision.m_StartEnd = new bool2(true, false);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        collision.m_CoverAreas = new float2(0.0f, area);
        // ISSUE: reference to a compiler-generated field
        collision.m_StartEnd = new bool2(false, true);
      }
      collisions.Add(in collision);
    }

    private static void CheckMeshIntersect(
      CameraCollisionSystem.Line line,
      DynamicBuffer<MeshVertex> vertices,
      DynamicBuffer<MeshIndex> indices,
      NativeList<CameraCollisionSystem.Collision> collisions)
    {
      for (int index = 0; index < indices.Length; index += 3)
      {
        Triangle3 triangle = new Triangle3(vertices[indices[index].m_Index].m_Vertex, vertices[indices[index + 1].m_Index].m_Vertex, vertices[indices[index + 2].m_Index].m_Vertex);
        // ISSUE: reference to a compiler-generated method
        CameraCollisionSystem.CheckTriangleIntersect(line, triangle, collisions);
      }
    }

    private static unsafe void CheckMeshIntersect(
      CameraCollisionSystem.Line line,
      DynamicBuffer<MeshVertex> vertices,
      DynamicBuffer<MeshIndex> indices,
      DynamicBuffer<MeshNode> nodes,
      NativeList<CameraCollisionSystem.Collision> collisions)
    {
      int* numPtr = stackalloc int[128];
      int a1 = 0;
      if (nodes.Length != 0)
        numPtr[a1++] = 0;
      while (--a1 >= 0)
      {
        int index = numPtr[a1];
        MeshNode node = nodes[index];
        // ISSUE: reference to a compiler-generated method
        if (CameraCollisionSystem.Intersect(line, node.m_Bounds, out float2 _))
        {
          for (int x = node.m_IndexRange.x; x < node.m_IndexRange.y; x += 3)
          {
            Triangle3 triangle = new Triangle3(vertices[indices[x].m_Index].m_Vertex, vertices[indices[x + 1].m_Index].m_Vertex, vertices[indices[x + 2].m_Index].m_Vertex);
            // ISSUE: reference to a compiler-generated method
            CameraCollisionSystem.CheckTriangleIntersect(line, triangle, collisions);
          }
          numPtr[a1] = node.m_SubNodes1.x;
          int a2 = math.select(a1, a1 + 1, node.m_SubNodes1.x != -1);
          numPtr[a2] = node.m_SubNodes1.y;
          int a3 = math.select(a2, a2 + 1, node.m_SubNodes1.y != -1);
          numPtr[a3] = node.m_SubNodes1.z;
          int a4 = math.select(a3, a3 + 1, node.m_SubNodes1.z != -1);
          numPtr[a4] = node.m_SubNodes1.w;
          int a5 = math.select(a4, a4 + 1, node.m_SubNodes1.w != -1);
          numPtr[a5] = node.m_SubNodes2.x;
          int a6 = math.select(a5, a5 + 1, node.m_SubNodes2.x != -1);
          numPtr[a6] = node.m_SubNodes2.y;
          int a7 = math.select(a6, a6 + 1, node.m_SubNodes2.y != -1);
          numPtr[a7] = node.m_SubNodes2.z;
          int a8 = math.select(a7, a7 + 1, node.m_SubNodes2.z != -1);
          numPtr[a8] = node.m_SubNodes2.w;
          a1 = math.select(a8, a8 + 1, node.m_SubNodes2.w != -1);
        }
      }
    }

    private static unsafe void CheckMeshIntersect(
      CameraCollisionSystem.Line line,
      DynamicBuffer<MeshVertex> vertices,
      DynamicBuffer<MeshIndex> indices,
      DynamicBuffer<MeshNode> nodes,
      DynamicBuffer<ProceduralBone> prefabBones,
      NativeList<CameraCollisionSystem.Collision> collisions)
    {
      int* numPtr = stackalloc int[128];
      for (int index1 = 0; index1 < prefabBones.Length; ++index1)
      {
        int a1 = 0;
        if (math.any(MathUtils.Size(nodes[index1].m_Bounds) > 0.0f))
          numPtr[a1++] = index1;
        while (--a1 >= 0)
        {
          int index2 = numPtr[a1];
          MeshNode node = nodes[index2];
          // ISSUE: reference to a compiler-generated method
          if (CameraCollisionSystem.Intersect(line, node.m_Bounds, out float2 _))
          {
            for (int x = node.m_IndexRange.x; x < node.m_IndexRange.y; x += 3)
            {
              Triangle3 triangle = new Triangle3(vertices[indices[x].m_Index].m_Vertex, vertices[indices[x + 1].m_Index].m_Vertex, vertices[indices[x + 2].m_Index].m_Vertex);
              // ISSUE: reference to a compiler-generated method
              CameraCollisionSystem.CheckTriangleIntersect(line, triangle, collisions);
            }
            numPtr[a1] = node.m_SubNodes1.x;
            int a2 = math.select(a1, a1 + 1, node.m_SubNodes1.x != -1);
            numPtr[a2] = node.m_SubNodes1.y;
            int a3 = math.select(a2, a2 + 1, node.m_SubNodes1.y != -1);
            numPtr[a3] = node.m_SubNodes1.z;
            int a4 = math.select(a3, a3 + 1, node.m_SubNodes1.z != -1);
            numPtr[a4] = node.m_SubNodes1.w;
            int a5 = math.select(a4, a4 + 1, node.m_SubNodes1.w != -1);
            numPtr[a5] = node.m_SubNodes2.x;
            int a6 = math.select(a5, a5 + 1, node.m_SubNodes2.x != -1);
            numPtr[a6] = node.m_SubNodes2.y;
            int a7 = math.select(a6, a6 + 1, node.m_SubNodes2.y != -1);
            numPtr[a7] = node.m_SubNodes2.z;
            int a8 = math.select(a7, a7 + 1, node.m_SubNodes2.z != -1);
            numPtr[a8] = node.m_SubNodes2.w;
            a1 = math.select(a8, a8 + 1, node.m_SubNodes2.w != -1);
          }
        }
      }
    }

    private static unsafe void CheckMeshIntersect(
      CameraCollisionSystem.Line line,
      DynamicBuffer<MeshVertex> vertices,
      DynamicBuffer<MeshIndex> indices,
      DynamicBuffer<MeshNode> nodes,
      DynamicBuffer<ProceduralBone> prefabBones,
      DynamicBuffer<Bone> bones,
      Skeleton skeleton,
      NativeList<CameraCollisionSystem.Collision> collisions)
    {
      int* numPtr = stackalloc int[128];
      for (int index1 = 0; index1 < prefabBones.Length; ++index1)
      {
        int a1 = 0;
        // ISSUE: variable of a compiler-generated type
        CameraCollisionSystem.Line line1 = line;
        ProceduralBone prefabBone1 = prefabBones[index1];
        if (math.any(MathUtils.Size(nodes[prefabBone1.m_BindIndex].m_Bounds) > 0.0f))
        {
          numPtr[a1++] = prefabBone1.m_BindIndex;
          Bone bone1 = bones[skeleton.m_BoneOffset + index1];
          float4x4 float4x4 = float4x4.TRS(bone1.m_Position, bone1.m_Rotation, bone1.m_Scale);
          ProceduralBone prefabBone2;
          for (int parentIndex = prefabBone1.m_ParentIndex; parentIndex >= 0; parentIndex = prefabBone2.m_ParentIndex)
          {
            Bone bone2 = bones[skeleton.m_BoneOffset + parentIndex];
            prefabBone2 = prefabBones[parentIndex];
            float4x4 = math.mul(float4x4.TRS(bone2.m_Position, bone2.m_Rotation, bone2.m_Scale), float4x4);
          }
          float4x4 a2 = math.inverse(math.mul(float4x4, prefabBone1.m_BindPose));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          line1.m_Line.a = math.mul(a2, new float4(line.m_Line.a, 1f)).xyz;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          line1.m_Line.b = math.mul(a2, new float4(line.m_Line.b, 1f)).xyz;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          line1.m_XVector = math.mul(a2, new float4(line.m_XVector, 0.0f)).xyz;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          line1.m_YVector = math.mul(a2, new float4(line.m_YVector, 0.0f)).xyz;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          line1.m_Expand = math.abs(line1.m_XVector) + math.abs(line1.m_YVector);
        }
        while (--a1 >= 0)
        {
          int index2 = numPtr[a1];
          MeshNode node = nodes[index2];
          // ISSUE: reference to a compiler-generated method
          if (CameraCollisionSystem.Intersect(line1, node.m_Bounds, out float2 _))
          {
            for (int x = node.m_IndexRange.x; x < node.m_IndexRange.y; x += 3)
            {
              Triangle3 triangle = new Triangle3(vertices[indices[x].m_Index].m_Vertex, vertices[indices[x + 1].m_Index].m_Vertex, vertices[indices[x + 2].m_Index].m_Vertex);
              // ISSUE: reference to a compiler-generated method
              CameraCollisionSystem.CheckTriangleIntersect(line1, triangle, collisions);
            }
            numPtr[a1] = node.m_SubNodes1.x;
            int a3 = math.select(a1, a1 + 1, node.m_SubNodes1.x != -1);
            numPtr[a3] = node.m_SubNodes1.y;
            int a4 = math.select(a3, a3 + 1, node.m_SubNodes1.y != -1);
            numPtr[a4] = node.m_SubNodes1.z;
            int a5 = math.select(a4, a4 + 1, node.m_SubNodes1.z != -1);
            numPtr[a5] = node.m_SubNodes1.w;
            int a6 = math.select(a5, a5 + 1, node.m_SubNodes1.w != -1);
            numPtr[a6] = node.m_SubNodes2.x;
            int a7 = math.select(a6, a6 + 1, node.m_SubNodes2.x != -1);
            numPtr[a7] = node.m_SubNodes2.y;
            int a8 = math.select(a7, a7 + 1, node.m_SubNodes2.y != -1);
            numPtr[a8] = node.m_SubNodes2.z;
            int a9 = math.select(a8, a8 + 1, node.m_SubNodes2.z != -1);
            numPtr[a9] = node.m_SubNodes2.w;
            a1 = math.select(a9, a9 + 1, node.m_SubNodes2.w != -1);
          }
        }
      }
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
    public CameraCollisionSystem()
    {
    }

    [BurstCompile]
    private struct FindEntitiesFromTreeJob : IJob
    {
      [ReadOnly]
      public Line3.Segment m_Line;
      [ReadOnly]
      public quaternion m_Rotation;
      [ReadOnly]
      public float2 m_FovOffset;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;
      [WriteOnly]
      public NativeList<Entity> m_EntityList;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float3 x1 = math.mul(this.m_Rotation, new float3(this.m_FovOffset.x, 0.0f, 0.0f));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float3 x2 = math.mul(this.m_Rotation, new float3(0.0f, this.m_FovOffset.y, 0.0f));
        float3 float3 = math.abs(x1) + math.abs(x2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CameraCollisionSystem.FindEntitiesFromTreeJob.Iterator iterator = new CameraCollisionSystem.FindEntitiesFromTreeJob.Iterator()
        {
          m_Line = this.m_Line,
          m_Expand = float3,
          m_EntityList = this.m_EntityList
        };
        // ISSUE: reference to a compiler-generated field
        this.m_SearchTree.Iterate<CameraCollisionSystem.FindEntitiesFromTreeJob.Iterator>(ref iterator);
      }

      private struct Iterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Line3.Segment m_Line;
        public float3 m_Expand;
        public NativeList<Entity> m_EntityList;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(MathUtils.Expand(bounds.m_Bounds, this.m_Expand), this.m_Line, out float2 _) && (bounds.m_Mask & BoundsMask.NotOverridden) != 0;
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated method
          if (!this.Intersect(bounds))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_EntityList.Add(in entity);
        }
      }
    }

    [BurstCompile]
    private struct ObjectCollisionJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Tree> m_TreeData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.NetObject> m_NetObjectData;
      [ReadOnly]
      public ComponentLookup<Quantity> m_QuantityData;
      [ReadOnly]
      public ComponentLookup<Stack> m_StackData;
      [ReadOnly]
      public ComponentLookup<UnderConstruction> m_UnderConstructionData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnectionData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.MeshData> m_PrefabMeshData;
      [ReadOnly]
      public ComponentLookup<ImpostorData> m_PrefabImpostorData;
      [ReadOnly]
      public ComponentLookup<SharedMeshData> m_PrefabSharedMeshData;
      [ReadOnly]
      public ComponentLookup<GrowthScaleData> m_PrefabGrowthScaleData;
      [ReadOnly]
      public ComponentLookup<QuantityObjectData> m_PrefabQuantityObjectData;
      [ReadOnly]
      public ComponentLookup<StackData> m_PrefabStackData;
      [ReadOnly]
      public BufferLookup<MeshGroup> m_MeshGroups;
      [ReadOnly]
      public BufferLookup<Skeleton> m_Skeletons;
      [ReadOnly]
      public BufferLookup<Bone> m_Bones;
      [ReadOnly]
      public BufferLookup<SubMesh> m_Meshes;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> m_SubMeshGroups;
      [ReadOnly]
      public BufferLookup<LodMesh> m_Lods;
      [ReadOnly]
      public BufferLookup<MeshVertex> m_Vertices;
      [ReadOnly]
      public BufferLookup<MeshIndex> m_Indices;
      [ReadOnly]
      public BufferLookup<MeshNode> m_Nodes;
      [ReadOnly]
      public BufferLookup<ProceduralBone> m_ProceduralBones;
      [ReadOnly]
      public Line3.Segment m_Line;
      [ReadOnly]
      public quaternion m_Rotation;
      [ReadOnly]
      public float2 m_FovOffset;
      [ReadOnly]
      public float m_MinClearRange;
      [ReadOnly]
      public bool m_LeftHandTraffic;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public NativeList<Entity> m_EntityList;
      [NativeDisableContainerSafetyRestriction]
      public NativeQueue<CameraCollisionSystem.Collision>.ParallelWriter m_Collisions;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_EntityList[index];
        // ISSUE: reference to a compiler-generated field
        Game.Objects.Transform transform = this.m_TransformData[entity];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity];
        ObjectGeometryData componentData1;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
          return;
        if ((componentData1.m_Flags & GeometryFlags.Marker) != GeometryFlags.None)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_OutsideConnectionData.HasComponent(entity))
            return;
        }
        else if ((componentData1.m_Flags & GeometryFlags.Physical) == GeometryFlags.None)
          return;
        quaternion q = math.inverse(transform.m_Rotation);
        // ISSUE: variable of a compiler-generated type
        CameraCollisionSystem.Line line;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        line.m_Line.a = math.mul(q, this.m_Line.a - transform.m_Position);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        line.m_Line.b = math.mul(q, this.m_Line.b - transform.m_Position);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        line.m_XVector = math.mul(q, math.mul(this.m_Rotation, new float3(this.m_FovOffset.x, 0.0f, 0.0f)));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        line.m_YVector = math.mul(q, math.mul(this.m_Rotation, new float3(0.0f, this.m_FovOffset.y, 0.0f)));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        line.m_Expand = math.abs(line.m_XVector) + math.abs(line.m_YVector);
        // ISSUE: reference to a compiler-generated field
        line.m_Scale = (float3) 1f;
        // ISSUE: reference to a compiler-generated field
        line.m_CutOffset = new float2();
        Stack componentData2;
        StackData componentData3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Bounds3 bounds = !this.m_StackData.TryGetComponent(entity, out componentData2) || !this.m_PrefabStackData.TryGetComponent(prefabRef.m_Prefab, out componentData3) ? ObjectUtils.GetBounds(componentData1) : ObjectUtils.GetBounds(componentData2, componentData1, componentData3);
        float2 t;
        // ISSUE: reference to a compiler-generated method
        if (!CameraCollisionSystem.Intersect(line, bounds, out t))
          return;
        float num = math.cmax(MathUtils.Size(bounds));
        // ISSUE: reference to a compiler-generated field
        line.m_CutOffset = new float2(t.x - num, t.y + num);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        line.m_Line = MathUtils.Cut(line.m_Line, line.m_CutOffset);
        NativeList<CameraCollisionSystem.Collision> collisions = new NativeList<CameraCollisionSystem.Collision>((AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated method
        this.RaycastMeshes(collisions, entity, prefabRef, line);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        CameraCollisionSystem.CheckCollisions(collisions, this.m_MinClearRange, t);
        for (int index1 = 0; index1 < collisions.Length; ++index1)
        {
          // ISSUE: variable of a compiler-generated type
          CameraCollisionSystem.Collision collision = collisions[index1] with
          {
            m_CoverAreas = (float2) 0.0f
          };
          // ISSUE: reference to a compiler-generated field
          this.m_Collisions.Enqueue(collision);
        }
        collisions.Dispose();
      }

      private bool HasCachedMesh(Entity mesh, out Entity sharedMesh)
      {
        SharedMeshData componentData;
        // ISSUE: reference to a compiler-generated field
        sharedMesh = !this.m_PrefabSharedMeshData.TryGetComponent(mesh, out componentData) ? mesh : componentData.m_Mesh;
        // ISSUE: reference to a compiler-generated field
        return this.m_Vertices.HasBuffer(sharedMesh);
      }

      private void RaycastMeshes(
        NativeList<CameraCollisionSystem.Collision> collisions,
        Entity entity,
        PrefabRef prefabRef,
        CameraCollisionSystem.Line line)
      {
        DynamicBuffer<SubMesh> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Meshes.TryGetBuffer(prefabRef.m_Prefab, out bufferData1))
          return;
        // ISSUE: reference to a compiler-generated field
        SubMeshFlags subMeshFlags = (SubMeshFlags) (2359296 | (this.m_LeftHandTraffic ? 65536 : 131072));
        int3 tileCounts = (int3) 0;
        float3 offsets = (float3) 0.0f;
        float3 scale = (float3) 0.0f;
        Tree componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TreeData.TryGetComponent(entity, out componentData1))
        {
          GrowthScaleData componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabGrowthScaleData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
          {
            // ISSUE: reference to a compiler-generated field
            subMeshFlags |= BatchDataHelpers.CalculateTreeSubMeshData(componentData1, componentData2, out line.m_Scale);
          }
          else
            subMeshFlags |= SubMeshFlags.RequireAdult;
        }
        Game.Objects.NetObject componentData3;
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetObjectData.TryGetComponent(entity, out componentData3))
          subMeshFlags |= BatchDataHelpers.CalculateNetObjectSubMeshData(componentData3);
        Quantity componentData4;
        // ISSUE: reference to a compiler-generated field
        if (this.m_QuantityData.TryGetComponent(entity, out componentData4))
        {
          QuantityObjectData componentData5;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabQuantityObjectData.TryGetComponent(prefabRef.m_Prefab, out componentData5))
          {
            // ISSUE: reference to a compiler-generated field
            subMeshFlags |= BatchDataHelpers.CalculateQuantitySubMeshData(componentData4, componentData5, this.m_EditorMode);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            subMeshFlags |= BatchDataHelpers.CalculateQuantitySubMeshData(componentData4, new QuantityObjectData(), this.m_EditorMode);
          }
        }
        UnderConstruction componentData6;
        ObjectGeometryData componentData7;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_UnderConstructionData.TryGetComponent(entity, out componentData6) && componentData6.m_NewPrefab == Entity.Null || this.m_DestroyedData.HasComponent(entity) && this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData7) && (componentData7.m_Flags & (GeometryFlags.Physical | GeometryFlags.HasLot)) == (GeometryFlags.Physical | GeometryFlags.HasLot))
          return;
        Stack componentData8;
        StackData componentData9;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_StackData.TryGetComponent(entity, out componentData8) && this.m_PrefabStackData.TryGetComponent(prefabRef.m_Prefab, out componentData9))
          subMeshFlags |= BatchDataHelpers.CalculateStackSubMeshData(componentData8, componentData9, out tileCounts, out offsets, out scale);
        else
          componentData9 = new StackData();
        DynamicBuffer<MeshGroup> bufferData2 = new DynamicBuffer<MeshGroup>();
        int num1 = 1;
        DynamicBuffer<SubMeshGroup> bufferData3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubMeshGroups.TryGetBuffer(prefabRef.m_Prefab, out bufferData3) && this.m_MeshGroups.TryGetBuffer(entity, out bufferData2))
          num1 = bufferData2.Length;
        for (int index1 = 0; index1 < num1; ++index1)
        {
          MeshGroup meshGroup;
          SubMeshGroup subMeshGroup;
          if (bufferData3.IsCreated)
          {
            CollectionUtils.TryGet<MeshGroup>(bufferData2, index1, out meshGroup);
            subMeshGroup = bufferData3[(int) meshGroup.m_SubMeshGroup];
          }
          else
          {
            subMeshGroup.m_SubMeshRange = new int2(0, bufferData1.Length);
            meshGroup = new MeshGroup();
          }
          for (int x = subMeshGroup.m_SubMeshRange.x; x < subMeshGroup.m_SubMeshRange.y; ++x)
          {
            SubMesh subMesh1 = bufferData1[x];
            if ((subMesh1.m_Flags & subMeshFlags) == subMesh1.m_Flags)
            {
              Entity entity1 = Entity.Null;
              Entity sharedMesh;
              // ISSUE: reference to a compiler-generated method
              if (this.HasCachedMesh(subMesh1.m_SubMesh, out sharedMesh))
              {
                entity1 = sharedMesh;
              }
              else
              {
                DynamicBuffer<LodMesh> bufferData4;
                // ISSUE: reference to a compiler-generated field
                if (this.m_Lods.TryGetBuffer(subMesh1.m_SubMesh, out bufferData4))
                {
                  for (int index2 = bufferData4.Length - 1; index2 >= 0; --index2)
                  {
                    // ISSUE: reference to a compiler-generated method
                    if (this.HasCachedMesh(bufferData4[index2].m_LodMesh, out sharedMesh))
                    {
                      entity1 = sharedMesh;
                      break;
                    }
                  }
                }
              }
              // ISSUE: reference to a compiler-generated field
              if (!(entity1 == Entity.Null) && (this.m_PrefabMeshData[entity1].m_State & MeshFlags.Decal) == (MeshFlags) 0)
              {
                int num2 = math.select(math.select(math.select(1, tileCounts.x, (subMesh1.m_Flags & SubMeshFlags.IsStackStart) > (SubMeshFlags) 0), tileCounts.y, (subMesh1.m_Flags & SubMeshFlags.IsStackMiddle) > (SubMeshFlags) 0), tileCounts.z, (subMesh1.m_Flags & SubMeshFlags.IsStackEnd) > (SubMeshFlags) 0);
                if (num2 >= 1)
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<MeshVertex> vertex = this.m_Vertices[entity1];
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<MeshIndex> index3 = this.m_Indices[entity1];
                  DynamicBuffer<MeshNode> bufferData5 = new DynamicBuffer<MeshNode>();
                  DynamicBuffer<ProceduralBone> bufferData6 = new DynamicBuffer<ProceduralBone>();
                  DynamicBuffer<Bone> bufferData7 = new DynamicBuffer<Bone>();
                  DynamicBuffer<Skeleton> dynamicBuffer = new DynamicBuffer<Skeleton>();
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_Nodes.TryGetBuffer(entity1, out bufferData5) && this.m_ProceduralBones.TryGetBuffer(entity1, out bufferData6) && this.m_Bones.TryGetBuffer(entity, out bufferData7))
                  {
                    // ISSUE: reference to a compiler-generated field
                    dynamicBuffer = this.m_Skeletons[entity];
                    if (dynamicBuffer.Length == 0)
                    {
                      bufferData7 = new DynamicBuffer<Bone>();
                      dynamicBuffer = new DynamicBuffer<Skeleton>();
                    }
                  }
                  for (int tileIndex = 0; tileIndex < num2; ++tileIndex)
                  {
                    SubMesh subMesh2 = subMesh1;
                    // ISSUE: variable of a compiler-generated type
                    CameraCollisionSystem.Line line1 = line;
                    if ((subMesh2.m_Flags & (SubMeshFlags.IsStackStart | SubMeshFlags.IsStackMiddle | SubMeshFlags.IsStackEnd)) != (SubMeshFlags) 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      BatchDataHelpers.CalculateStackSubMeshData(componentData9, offsets, scale, tileIndex, subMesh1.m_Flags, ref subMesh2.m_Position, ref line1.m_Scale);
                    }
                    if ((subMesh2.m_Flags & (SubMeshFlags.IsStackStart | SubMeshFlags.IsStackMiddle | SubMeshFlags.IsStackEnd | SubMeshFlags.HasTransform)) != (SubMeshFlags) 0)
                    {
                      quaternion q = math.inverse(subMesh2.m_Rotation);
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      line1.m_Line.a = math.mul(q, line1.m_Line.a - subMesh2.m_Position);
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      line1.m_Line.b = math.mul(q, line1.m_Line.b - subMesh2.m_Position);
                    }
                    ImpostorData componentData10;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_PrefabImpostorData.TryGetComponent(entity1, out componentData10) && (double) componentData10.m_Size != 0.0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      line1.m_Scale *= componentData10.m_Size;
                      // ISSUE: reference to a compiler-generated field
                      line1.m_Line.a -= componentData10.m_Offset;
                      // ISSUE: reference to a compiler-generated field
                      line1.m_Line.b -= componentData10.m_Offset;
                    }
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    line1.m_Expand = math.abs(line1.m_XVector) + math.abs(line1.m_YVector);
                    if (bufferData5.IsCreated)
                    {
                      if (bufferData6.IsCreated)
                      {
                        if (bufferData7.IsCreated)
                        {
                          int index4 = x - subMeshGroup.m_SubMeshRange.x + (int) meshGroup.m_MeshOffset;
                          // ISSUE: reference to a compiler-generated method
                          CameraCollisionSystem.CheckMeshIntersect(line1, vertex, index3, bufferData5, bufferData6, bufferData7, dynamicBuffer[index4], collisions);
                        }
                        else
                        {
                          // ISSUE: reference to a compiler-generated method
                          CameraCollisionSystem.CheckMeshIntersect(line1, vertex, index3, bufferData5, bufferData6, collisions);
                        }
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated method
                        CameraCollisionSystem.CheckMeshIntersect(line1, vertex, index3, bufferData5, collisions);
                      }
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated method
                      CameraCollisionSystem.CheckMeshIntersect(line1, vertex, index3, collisions);
                    }
                  }
                }
              }
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct SelectCameraPositionJob : IJob
    {
      [ReadOnly]
      public Line3.Segment m_Line;
      [ReadOnly]
      public float3 m_PreviousPosition;
      [ReadOnly]
      public float m_MinClearRange;
      [ReadOnly]
      public float m_NearPlaneRange;
      [ReadOnly]
      public float m_Smoothing;
      [ReadOnly]
      public float m_DeltaTime;
      [ReadOnly]
      public TerrainHeightData m_TerrainData;
      [ReadOnly]
      public WaterSurfaceData m_WaterData;
      public NativeQueue<CameraCollisionSystem.Collision> m_Collisions;
      public NativeReference<CameraCollisionSystem.Result> m_Result;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        ref CameraCollisionSystem.Result local = ref this.m_Result.ValueAsRef<CameraCollisionSystem.Result>();
        // ISSUE: reference to a compiler-generated field
        NativeList<CameraCollisionSystem.Collision> collisions = new NativeList<CameraCollisionSystem.Collision>(this.m_Collisions.Count, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: variable of a compiler-generated type
        CameraCollisionSystem.Collision collision1;
        // ISSUE: reference to a compiler-generated field
        while (this.m_Collisions.TryDequeue(out collision1))
          collisions.Add(in collision1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        CameraCollisionSystem.CheckCollisions(collisions, this.m_MinClearRange, new float2(0.0f, 1f));
        float t1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        double num1 = (double) MathUtils.Distance(this.m_Line, local.m_Position, out t1);
        float t2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        double num2 = (double) MathUtils.Distance(this.m_Line, this.m_PreviousPosition, out t2);
        float x = 0.0f;
        float num3 = (float) (((double) t1 + (double) t2) * 0.5);
        float y1 = t1;
        float num4 = float.MaxValue;
        float2 float2 = (float2) t1;
        bool flag = true;
        for (int index = 0; index <= collisions.Length; ++index)
        {
          float y2 = 1f;
          float num5 = 1f;
          if (index < collisions.Length)
          {
            // ISSUE: variable of a compiler-generated type
            CameraCollisionSystem.Collision collision2 = collisions[index];
            // ISSUE: reference to a compiler-generated field
            y2 = collision2.m_LineBounds.min;
            // ISSUE: reference to a compiler-generated field
            num5 = collision2.m_LineBounds.max;
          }
          // ISSUE: reference to a compiler-generated field
          if ((double) y2 - (double) x >= (double) this.m_MinClearRange)
          {
            float offset;
            float num6;
            if ((double) x > (double) t1)
            {
              offset = x;
              // ISSUE: reference to a compiler-generated method
              num6 = math.select(float.MaxValue, math.abs(offset - num3), this.CheckOffset(offset));
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if ((double) y2 - (double) this.m_MinClearRange < (double) t1)
              {
                // ISSUE: reference to a compiler-generated field
                offset = y2 - this.m_MinClearRange;
                // ISSUE: reference to a compiler-generated method
                num6 = math.select(float.MaxValue, math.abs(offset - num3), this.CheckOffset(offset));
              }
              else
              {
                offset = t1;
                num6 = 0.0f;
              }
            }
            if ((double) num6 < (double) num4 || flag && (double) num6 != 3.4028234663852886E+38)
            {
              y1 = offset;
              num4 = num6;
              // ISSUE: reference to a compiler-generated field
              float2 = new float2(x, y2) - this.m_NearPlaneRange;
              flag = false;
            }
          }
          else if (flag)
          {
            float offset;
            float num7;
            if ((double) x > (double) t1)
            {
              offset = x;
              // ISSUE: reference to a compiler-generated method
              num7 = math.select(float.MaxValue, math.abs(offset - num3), this.CheckOffset(offset));
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if ((double) y2 - (double) this.m_MinClearRange < (double) t1)
              {
                // ISSUE: reference to a compiler-generated field
                offset = y2 - this.m_MinClearRange;
                // ISSUE: reference to a compiler-generated method
                num7 = math.select(float.MaxValue, math.abs(offset - num3), this.CheckOffset(offset));
              }
              else
              {
                offset = t1;
                num7 = float.MaxValue;
              }
            }
            if ((double) num7 < (double) num4)
            {
              y1 = offset;
              num4 = num7;
              // ISSUE: reference to a compiler-generated field
              float2 = new float2(x, y2) - this.m_NearPlaneRange;
            }
          }
          x = num5;
        }
        float2.x = math.min(float2.x, y1);
        float2.y = math.max(float2.y, y1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float t3 = math.clamp(y1 + (local.m_Offset - (y1 - t1)) * math.pow(this.m_Smoothing, this.m_DeltaTime), float2.x, float2.y);
        if ((double) t3 != (double) t1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          local.m_Position = MathUtils.Position(this.m_Line, t3);
        }
        // ISSUE: reference to a compiler-generated field
        local.m_Offset = t3 - t1;
        collisions.Dispose();
      }

      private bool CheckOffset(float offset)
      {
        // ISSUE: reference to a compiler-generated field
        float3 worldPosition = MathUtils.Position(this.m_Line, offset);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (double) worldPosition.y >= (double) WaterUtils.SampleHeight(ref this.m_WaterData, ref this.m_TerrainData, worldPosition) + (double) this.m_MinClearRange;
      }
    }

    private struct Line
    {
      public Line3.Segment m_Line;
      public float3 m_XVector;
      public float3 m_YVector;
      public float2 m_CutOffset;
      public float3 m_Expand;
      public float3 m_Scale;
    }

    private struct Collision : IComparable<CameraCollisionSystem.Collision>
    {
      public Bounds1 m_LineBounds;
      public float2 m_CoverAreas;
      public bool2 m_StartEnd;

      public int CompareTo(CameraCollisionSystem.Collision other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_LineBounds.min.CompareTo(other.m_LineBounds.min);
      }
    }

    private struct Result
    {
      public float3 m_Position;
      public float m_Offset;
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Tree> __Game_Objects_Tree_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.NetObject> __Game_Objects_NetObject_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Quantity> __Game_Objects_Quantity_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Stack> __Game_Objects_Stack_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UnderConstruction> __Game_Objects_UnderConstruction_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.MeshData> __Game_Prefabs_MeshData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ImpostorData> __Game_Prefabs_ImpostorData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SharedMeshData> __Game_Prefabs_SharedMeshData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GrowthScaleData> __Game_Prefabs_GrowthScaleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<QuantityObjectData> __Game_Prefabs_QuantityObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StackData> __Game_Prefabs_StackData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<MeshGroup> __Game_Rendering_MeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Skeleton> __Game_Rendering_Skeleton_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Bone> __Game_Rendering_Bone_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> __Game_Prefabs_SubMeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LodMesh> __Game_Prefabs_LodMesh_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshVertex> __Game_Prefabs_MeshVertex_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshIndex> __Game_Prefabs_MeshIndex_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshNode> __Game_Prefabs_MeshNode_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ProceduralBone> __Game_Prefabs_ProceduralBone_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentLookup = state.GetComponentLookup<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_NetObject_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.NetObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Quantity_RO_ComponentLookup = state.GetComponentLookup<Quantity>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stack_RO_ComponentLookup = state.GetComponentLookup<Stack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UnderConstruction_RO_ComponentLookup = state.GetComponentLookup<UnderConstruction>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.MeshData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ImpostorData_RO_ComponentLookup = state.GetComponentLookup<ImpostorData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SharedMeshData_RO_ComponentLookup = state.GetComponentLookup<SharedMeshData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GrowthScaleData_RO_ComponentLookup = state.GetComponentLookup<GrowthScaleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup = state.GetComponentLookup<QuantityObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StackData_RO_ComponentLookup = state.GetComponentLookup<StackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshGroup_RO_BufferLookup = state.GetBufferLookup<MeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Skeleton_RO_BufferLookup = state.GetBufferLookup<Skeleton>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Bone_RO_BufferLookup = state.GetBufferLookup<Bone>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMeshGroup_RO_BufferLookup = state.GetBufferLookup<SubMeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LodMesh_RO_BufferLookup = state.GetBufferLookup<LodMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshVertex_RO_BufferLookup = state.GetBufferLookup<MeshVertex>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshIndex_RO_BufferLookup = state.GetBufferLookup<MeshIndex>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshNode_RO_BufferLookup = state.GetBufferLookup<MeshNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ProceduralBone_RO_BufferLookup = state.GetBufferLookup<ProceduralBone>(true);
      }
    }
  }
}
