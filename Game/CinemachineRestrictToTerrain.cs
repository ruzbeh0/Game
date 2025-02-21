// Decompiled with JetBrains decompiler
// Type: Game.CinemachineRestrictToTerrain
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Cinemachine;
using Colossal.Mathematics;
using Game.Rendering;
using Game.SceneFlow;
using Game.Simulation;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game
{
  public class CinemachineRestrictToTerrain : CinemachineExtension
  {
    public float m_MapSurfacePadding = 1f;
    public bool m_RestrictToMapArea = true;
    private CameraCollisionSystem m_CollisionSystem;
    private CameraUpdateSystem m_CameraSystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;

    public bool enableObjectCollisions { get; set; } = true;

    public Vector3 previousPosition { get; set; }

    protected void Start()
    {
      this.m_CollisionSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<CameraCollisionSystem>();
      this.m_CameraSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<CameraUpdateSystem>();
      this.m_TerrainSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<TerrainSystem>();
      this.m_WaterSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<WaterSystem>();
    }

    public void Refresh() => this.previousPosition = this.transform.position;

    protected override void PostPipelineStageCallback(
      CinemachineVirtualCameraBase vcam,
      CinemachineCore.Stage stage,
      ref CameraState state,
      float deltaTime)
    {
      if (stage != CinemachineCore.Stage.Body)
        return;
      Vector3 terrain = this.ClampToTerrain(state.RawPosition, this.m_RestrictToMapArea, out float _);
      state.RawPosition = terrain;
      Vector3 position;
      if (!this.enableObjectCollisions || !this.CheckForCollision(state.RawPosition, this.previousPosition, state.RawOrientation, out position))
        return;
      state.RawPosition = position;
    }

    public bool CheckForCollision(
      Vector3 currentPosition,
      Vector3 lastPosition,
      Quaternion rotation,
      out Vector3 position)
    {
      if (this.m_CollisionSystem != null && this.m_CameraSystem != null && (Object) this.m_CameraSystem.activeCamera != (Object) null)
      {
        float3 position1 = (float3) currentPosition;
        float3 previousPosition = (float3) lastPosition;
        float nearClipPlane = this.m_CameraSystem.activeCamera.nearClipPlane;
        float2 fieldOfView;
        fieldOfView.y = this.m_CameraSystem.activeCamera.fieldOfView;
        fieldOfView.x = Camera.VerticalToHorizontalFieldOfView(fieldOfView.y, this.m_CameraSystem.activeCamera.aspect);
        // ISSUE: reference to a compiler-generated method
        this.m_CollisionSystem.CheckCollisions(ref position1, previousPosition, (quaternion) rotation, 200f, 200f, (float) ((double) nearClipPlane * 2.0 + 1.0), nearClipPlane, 1f / 1000f, fieldOfView);
        position = (Vector3) position1;
        return true;
      }
      position = Vector3.zero;
      return false;
    }

    public Vector3 ClampToTerrain(
      Vector3 position,
      bool restrictToMapArea,
      out float terrainHeight)
    {
      terrainHeight = 0.0f;
      // ISSUE: reference to a compiler-generated method
      TerrainHeightData heightData = this.m_TerrainSystem.GetHeightData();
      if (heightData.isCreated)
      {
        if (restrictToMapArea)
        {
          Bounds3 bounds = GameManager.instance.gameMode.IsEditor() ? TerrainUtils.GetEditorCameraBounds(this.m_TerrainSystem, ref heightData) : TerrainUtils.GetBounds(ref heightData);
          float3 max = bounds.max with
          {
            y = bounds.min.y + math.max(bounds.max.y - bounds.min.y, 4096f)
          };
          bounds.max = max;
          position = (Vector3) MathUtils.Clamp((float3) position, bounds);
        }
        if (this.m_WaterSystem.Loaded)
        {
          JobHandle deps;
          // ISSUE: reference to a compiler-generated method
          WaterSurfaceData surfaceData = this.m_WaterSystem.GetSurfaceData(out deps);
          deps.Complete();
          if (surfaceData.isCreated)
            terrainHeight = WaterUtils.SampleHeight(ref surfaceData, ref heightData, (float3) position);
        }
        else
          terrainHeight = TerrainUtils.SampleHeight(ref heightData, (float3) position);
        position.y = Mathf.Max(position.y, terrainHeight += this.m_MapSurfacePadding);
      }
      return position;
    }
  }
}
