// Decompiled with JetBrains decompiler
// Type: Game.Rendering.Viewer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Rendering.Legacy;
using Game.Simulation;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#nullable disable
namespace Game.Rendering
{
  public class Viewer
  {
    private ViewerDistances m_ViewerDistances;
    private float m_TargetFocusDistance;
    private float m_FocusDistanceVelocity;
    private const int kCenterSampleCount = 4;
    private static int[] kSamplePattern32 = new int[64]
    {
      1,
      1,
      4,
      1,
      2,
      -1,
      -4,
      0,
      -6,
      1,
      -7,
      -1,
      -2,
      2,
      7,
      2,
      2,
      3,
      -5,
      4,
      -1,
      4,
      4,
      4,
      -7,
      5,
      -3,
      5,
      6,
      5,
      1,
      6,
      -4,
      7,
      5,
      7,
      -1,
      -2,
      6,
      -2,
      -6,
      -3,
      -3,
      -3,
      0,
      -4,
      4,
      -4,
      2,
      -5,
      7,
      -5,
      -7,
      -6,
      -3,
      -6,
      5,
      -6,
      -5,
      -7,
      -1,
      -7,
      3,
      -7
    };

    public ViewerDistances viewerDistances => this.m_ViewerDistances;

    public float visibilityDistance => this.camera.farClipPlane;

    public float nearClipPlane => this.camera.nearClipPlane;

    public float3 position => (float3) this.camera.transform.position;

    public float3 forward => (float3) this.camera.transform.forward;

    public float3 right => (float3) this.camera.transform.right;

    public Camera camera { get; private set; }

    public LegacyFrustumPlanes frustumPlanes => Viewer.CalculateFrustumPlanes(this.camera);

    public Bounds bounds => this.UpdateBounds();

    public bool shadowsAdjustStartDistance { get; set; } = true;

    public float pushCullingNearPlaneMultiplier { get; set; } = 0.9f;

    public float pushCullingNearPlaneValue { get; set; } = 100f;

    public bool shadowsAdjustFarDistance { get; set; } = true;

    public Viewer(Camera camera) => this.camera = camera;

    public bool TryGetLODParameters(out LODParameters lodParameters)
    {
      ScriptableCullingParameters cullingParameters;
      if (this.camera.TryGetCullingParameters(out cullingParameters))
      {
        lodParameters = cullingParameters.lodParameters;
        return true;
      }
      lodParameters = new LODParameters();
      return false;
    }

    protected Bounds UpdateBounds()
    {
      Transform transform = this.camera.transform;
      float num1 = math.tan(math.radians(this.camera.fieldOfView * 0.5f));
      float num2 = num1 * this.camera.aspect;
      float3 forward = (float3) transform.forward;
      float3 right = (float3) transform.right;
      float3 up = (float3) transform.up;
      float farClipPlane = this.camera.farClipPlane;
      float nearClipPlane = this.camera.nearClipPlane;
      float3 position = this.position;
      float3 point1 = position + forward * farClipPlane - farClipPlane * right * num2 + up * num1 * farClipPlane;
      float3 point2 = position + forward * farClipPlane + farClipPlane * right * num2 - up * num1 * farClipPlane;
      float3 point3 = position + forward * farClipPlane - farClipPlane * right * num2 - up * num1 * farClipPlane;
      float3 point4 = position + forward * farClipPlane + farClipPlane * right * num2 + up * num1 * farClipPlane;
      float3 point5 = position + forward * nearClipPlane;
      Bounds bounds = new Bounds(Vector3.zero, Vector3.one * float.NegativeInfinity);
      bounds.Encapsulate((Vector3) point1);
      bounds.Encapsulate((Vector3) point2);
      bounds.Encapsulate((Vector3) point3);
      bounds.Encapsulate((Vector3) point4);
      bounds.Encapsulate((Vector3) point5);
      return bounds;
    }

    private static LegacyFrustumPlanes ExtractProjectionPlanes(float4x4 worldToProjectionMatrix)
    {
      LegacyFrustumPlanes projectionPlanes = new LegacyFrustumPlanes();
      float4 float4_1 = new float4(worldToProjectionMatrix.c0.w, worldToProjectionMatrix.c1.w, worldToProjectionMatrix.c2.w, worldToProjectionMatrix.c3.w);
      float4 float4_2 = new float4(worldToProjectionMatrix.c0.x, worldToProjectionMatrix.c1.x, worldToProjectionMatrix.c2.x, worldToProjectionMatrix.c3.x);
      float3 x1 = new float3(float4_2.x + float4_1.x, float4_2.y + float4_1.y, float4_2.z + float4_1.z);
      float num1 = 1f / math.length(x1);
      projectionPlanes.left.normal = x1 * num1;
      projectionPlanes.left.distance = (float4_2.w + float4_1.w) * num1;
      x1 = new float3(-float4_2.x + float4_1.x, -float4_2.y + float4_1.y, -float4_2.z + float4_1.z);
      float num2 = 1f / math.length(x1);
      projectionPlanes.right.normal = x1 * num2;
      projectionPlanes.right.distance = (-float4_2.w + float4_1.w) * num2;
      float4_2 = new float4(worldToProjectionMatrix.c0.y, worldToProjectionMatrix.c1.y, worldToProjectionMatrix.c2.y, worldToProjectionMatrix.c3.y);
      float3 x2 = (float3) new Vector3(float4_2.x + float4_1.x, float4_2.y + float4_1.y, float4_2.z + float4_1.z);
      float num3 = 1f / math.length(x2);
      projectionPlanes.bottom.normal = x2 * num3;
      projectionPlanes.bottom.distance = (float4_2.w + float4_1.w) * num3;
      float3 x3 = (float3) new Vector3(-float4_2.x + float4_1.x, -float4_2.y + float4_1.y, -float4_2.z + float4_1.z);
      float num4 = 1f / math.length(x3);
      projectionPlanes.top.normal = x3 * num4;
      projectionPlanes.top.distance = (-float4_2.w + float4_1.w) * num4;
      float4_2 = new float4(worldToProjectionMatrix.c0.z, worldToProjectionMatrix.c1.z, worldToProjectionMatrix.c2.z, worldToProjectionMatrix.c3.z);
      float3 x4 = (float3) new Vector3(float4_2.x + float4_1.x, float4_2.y + float4_1.y, float4_2.z + float4_1.z);
      float num5 = 1f / math.length(x4);
      projectionPlanes.zNear.normal = x4 * num5;
      projectionPlanes.zNear.distance = (float4_2.w + float4_1.w) * num5;
      float3 x5 = (float3) new Vector3(-float4_2.x + float4_1.x, -float4_2.y + float4_1.y, -float4_2.z + float4_1.z);
      float num6 = 1f / math.length(x5);
      projectionPlanes.zFar.normal = x5 * num6;
      projectionPlanes.zFar.distance = (-float4_2.w + float4_1.w) * num6;
      return projectionPlanes;
    }

    private static LegacyFrustumPlanes CalculateFrustumPlanes(Camera camera)
    {
      return Viewer.ExtractProjectionPlanes((float4x4) (camera.projectionMatrix * camera.worldToCameraMatrix));
    }

    public void UpdateRaycast(RaycastSystem raycast, float deltaTime)
    {
      float3 position = this.position;
      // ISSUE: reference to a compiler-generated method
      NativeArray<RaycastResult> result = raycast.GetResult((object) this);
      float x1 = this.visibilityDistance;
      float x2 = 0.0f;
      float num1 = 0.0f;
      float num2 = 0.0f;
      float x3 = -1f;
      for (int index = 0; index < result.Length; ++index)
      {
        RaycastResult raycastResult = result[index];
        if (!(raycastResult.m_Owner == Entity.Null))
        {
          float y = math.distance(position, raycastResult.m_Hit.m_HitPosition);
          if (index == 0)
            this.m_ViewerDistances.ground = y;
          else if (index - 1 < 4)
          {
            x3 = math.max(x3, y);
          }
          else
          {
            x1 = math.min(x1, y);
            x2 = math.max(x2, y);
            num1 += y;
            ++num2;
          }
        }
      }
      this.m_ViewerDistances.closestSurface = x1;
      this.m_ViewerDistances.farthestSurface = x2;
      this.m_ViewerDistances.averageSurface = (float) (((double) x1 + (double) x2) / 2.0);
      if ((double) num2 > 0.0)
        this.m_ViewerDistances.averageSurface = num1 / num2;
      if ((double) x3 >= 0.0)
        this.m_TargetFocusDistance = x3;
      this.m_ViewerDistances.focus = MathUtils.SmoothDamp(this.m_ViewerDistances.focus, this.m_TargetFocusDistance, ref this.m_FocusDistanceVelocity, 0.3f, float.MaxValue, deltaTime);
      if (!((Object) this.camera != (Object) null))
        return;
      this.camera.focusDistance = this.m_ViewerDistances.focus;
      this.UpdatePushNearCullingPlane();
      this.UpdateDistanceToSeaLevel();
    }

    private void UpdateDistanceToSeaLevel()
    {
      float x = 0.0f;
      UnityEngine.Plane plane = new UnityEngine.Plane(Vector3.up, -WaterSystem.SeaLevel);
      for (int index = 0; index < 4; ++index)
      {
        Ray ray = this.camera.ViewportPointToRay(new Vector3((index & 1) != 0 ? 1f : 0.0f, (index & 2) != 0 ? 1f : 0.0f, 0.0f));
        float enter;
        x = plane.Raycast(ray, out enter) ? math.max(x, enter) : this.visibilityDistance;
      }
      this.m_ViewerDistances.maxDistanceToSeaLevel = x;
    }

    private void UpdatePushNearCullingPlane()
    {
      HDCamera hdCamera = HDCamera.GetOrCreate(this.camera);
      if (hdCamera == null)
        return;
      if (this.shadowsAdjustStartDistance)
      {
        float num = math.clamp((this.m_ViewerDistances.closestSurface - this.pushCullingNearPlaneValue) * this.pushCullingNearPlaneMultiplier, this.nearClipPlane, this.visibilityDistance * 0.1f);
        hdCamera.overrideNearPlaneForCullingOnly = num;
      }
      else
        hdCamera.overrideNearPlaneForCullingOnly = 0.0f;
    }

    public void Raycast(RaycastSystem raycast)
    {
      float3 position = this.position;
      RaycastInput input = new RaycastInput()
      {
        m_Flags = (RaycastFlags) 0,
        m_CollisionMask = (CollisionMask.OnGround | CollisionMask.Overground),
        m_NetLayerMask = Layer.All
      } with
      {
        m_TypeMask = TypeMask.Terrain | TypeMask.Water,
        m_Line = new Line3.Segment(position, position + (float3) Vector3.down * this.visibilityDistance)
      };
      // ISSUE: reference to a compiler-generated method
      raycast.AddInput((object) this, input);
      for (int index = 0; index < Viewer.kSamplePattern32.Length; index += 2)
      {
        Ray ray = this.camera.ViewportPointToRay(new Vector3((float) ((double) Viewer.kSamplePattern32[index] / 7.0 * 0.5 + 0.5), (float) ((double) Viewer.kSamplePattern32[index + 1] / 7.0 * 0.5 + 0.5), 0.0f));
        input.m_TypeMask = index >= 8 ? TypeMask.Terrain | TypeMask.Water : TypeMask.Terrain | TypeMask.StaticObjects | TypeMask.MovingObjects | TypeMask.Net | TypeMask.Water;
        input.m_Line = new Line3.Segment(position, position + (float3) ray.direction * this.visibilityDistance);
        // ISSUE: reference to a compiler-generated method
        raycast.AddInput((object) this, input);
      }
    }
  }
}
