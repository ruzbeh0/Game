// Decompiled with JetBrains decompiler
// Type: Game.OrbitCameraController
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Cinemachine;
using Colossal.Mathematics;
using Game.Audio;
using Game.Rendering;
using Game.SceneFlow;
using Game.UI.InGame;
using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game
{
  public class OrbitCameraController : MonoBehaviour, IGameCameraController
  {
    public float2 m_ZoomRange = new float2(10f, 10000f);
    public float m_FollowSmoothing = 0.01f;
    private Entity m_Entity;
    private float m_FollowTimer;
    private float2 m_Rotation;
    private GameObject m_Anchor;
    private CinemachineVirtualCamera m_VCam;
    private CinemachineOrbitalTransposer m_Transposer;
    private CinemachineRestrictToTerrain m_Collider;
    private CameraInput m_CameraInput;
    private CameraUpdateSystem m_CameraUpdateSystem;

    public Entity followedEntity
    {
      get => !this.isActiveAndEnabled ? Entity.Null : this.m_Entity;
      set
      {
        if (!(this.m_Entity != value))
          return;
        this.m_Entity = value;
        this.xOffset = 0.0f;
        this.yOffset = 0.0f;
        this.m_FollowTimer = 0.0f;
        if (!this.isActiveAndEnabled)
          return;
        this.RefreshAudioFollow(value != Entity.Null);
      }
    }

    public OrbitCameraController.Mode mode { get; set; }

    public Vector3 pivot
    {
      get => this.m_Anchor.transform.position;
      set => this.m_Anchor.transform.position = value;
    }

    public Vector3 position
    {
      get => this.transform.position;
      set
      {
        this.transform.position = value;
        this.m_Anchor.transform.position = value + this.m_Anchor.transform.rotation * new Vector3(0.0f, 0.0f, this.zoom);
      }
    }

    public bool controllerEnabled
    {
      get => this.isActiveAndEnabled;
      set => this.gameObject.SetActive(value);
    }

    public bool inputEnabled { get; set; } = true;

    public Vector3 rotation
    {
      get => this.m_Anchor.transform.rotation.eulerAngles;
      set => this.m_Rotation = new float2(value.y, value.x);
    }

    public float zoom { get; set; }

    public float yOffset { get; set; }

    public float xOffset { get; set; }

    public ICinemachineCamera virtualCamera => (ICinemachineCamera) this.m_VCam;

    public ref LensSettings lens => ref this.m_VCam.m_Lens;

    public bool collisionsEnabled
    {
      get => this.m_Collider.enableObjectCollisions;
      set => this.m_Collider.enableObjectCollisions = value;
    }

    public Action EventCameraMove { get; set; }

    private async void Awake()
    {
      OrbitCameraController cameraController = this;
      if (!await GameManager.instance.WaitForReadyState())
        return;
      cameraController.m_Anchor = new GameObject("OrbitCameraAnchor");
      Transform transform = cameraController.m_Anchor.transform;
      cameraController.m_VCam = cameraController.GetComponent<CinemachineVirtualCamera>();
      cameraController.m_Transposer = cameraController.m_VCam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
      cameraController.m_Collider = cameraController.GetComponent<CinemachineRestrictToTerrain>();
      if ((UnityEngine.Object) cameraController.m_VCam != (UnityEngine.Object) null)
      {
        cameraController.m_VCam.LookAt = transform;
        cameraController.m_VCam.Follow = transform;
      }
      cameraController.gameObject.SetActive(false);
      cameraController.m_CameraInput = cameraController.GetComponent<CameraInput>();
      if ((UnityEngine.Object) cameraController.m_CameraInput != (UnityEngine.Object) null)
        cameraController.m_CameraInput.Initialize();
      cameraController.m_CameraUpdateSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<CameraUpdateSystem>();
      cameraController.m_CameraUpdateSystem.orbitCameraController = cameraController;
      // ISSUE: explicit non-virtual call
      // ISSUE: explicit non-virtual call
      __nonvirtual (cameraController.zoom) = Mathf.Clamp(__nonvirtual (cameraController.zoom), cameraController.m_ZoomRange.x, cameraController.m_ZoomRange.y);
    }

    private void OnEnable() => this.RefreshAudioFollow(true);

    private void OnDisable() => this.RefreshAudioFollow(false);

    private void RefreshAudioFollow(bool active)
    {
      if (AudioManager.instance == null)
        return;
      AudioManager.instance.followed = active ? this.m_Entity : Entity.Null;
    }

    private void OnDestroy()
    {
      if ((UnityEngine.Object) this.m_Anchor != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_Anchor);
      if (this.m_CameraUpdateSystem == null)
        return;
      this.m_CameraUpdateSystem.cinematicCameraController = (CinematicCameraController) null;
    }

    public void TryMatchPosition(IGameCameraController other)
    {
      this.rotation = other.rotation;
      this.zoom = Mathf.Clamp(other.zoom, this.m_ZoomRange.x, this.m_ZoomRange.y);
      this.pivot = other.pivot;
    }

    public void UpdateCamera()
    {
      this.m_Collider.Refresh();
      this.m_CameraInput.Refresh();
      if (this.inputEnabled && (UnityEngine.Object) this.m_CameraInput != (UnityEngine.Object) null)
      {
        Vector2 rotate = this.m_CameraInput.rotate;
        this.m_Rotation.x = (float) (((double) this.m_Rotation.x + (double) rotate.x) % 360.0);
        this.m_Rotation.y = Mathf.Clamp((float) (((double) this.m_Rotation.y + 90.0) % 360.0) - rotate.y, 0.0f, 180f) - 90f;
        this.zoom = Mathf.Clamp(math.pow(this.zoom, 1f + this.m_CameraInput.zoom), this.m_ZoomRange.x, this.m_ZoomRange.y);
        if (this.followedEntity == Entity.Null)
        {
          Vector2 move = this.m_CameraInput.move;
          Vector3 terrain = this.m_Collider.ClampToTerrain(this.m_Anchor.transform.position, true, out float _);
          double zoom = (double) this.zoom;
          Vector2 vector2 = move * (float) zoom;
          float terrainHeight;
          this.m_Anchor.transform.position = this.m_Collider.ClampToTerrain(terrain + (Vector3) math.mul(quaternion.AxisAngle(new float3(0.0f, 1f, 0.0f), math.radians(this.m_Anchor.transform.rotation.eulerAngles.y)), new float3(vector2.x, 0.0f, vector2.y)), true, out terrainHeight) with
          {
            y = terrainHeight + 10f
          };
        }
        float3 position;
        float radius;
        if (OrbitCameraController.TryGetPosition(this.followedEntity, World.DefaultGameObjectInjectionWorld.EntityManager, out position, out quaternion _, out radius))
        {
          this.m_Anchor.transform.rotation = (Quaternion) quaternion.Euler(math.radians(this.m_Rotation.y), math.radians(this.m_Rotation.x), 0.0f);
          float3 float3_1 = (float3) this.pivot - position;
          this.m_FollowTimer += Time.deltaTime;
          float num = math.pow(this.m_FollowSmoothing, Time.deltaTime) * math.smoothstep(0.5f, 0.0f, this.m_FollowTimer);
          float3 float3_2 = float3_1 * num;
          this.m_Anchor.transform.position = (Vector3) (position + float3_2 + math.mul((quaternion) this.m_Anchor.transform.rotation, new float3(this.xOffset, this.yOffset, 0.0f)));
        }
        else
          this.m_Anchor.transform.rotation = (Quaternion) quaternion.Euler(math.radians(this.m_Rotation.y), math.radians(this.m_Rotation.x), 0.0f);
        this.m_Transposer.m_FollowOffset.z = -this.zoom - radius;
      }
      Transform transform = this.transform;
      // ISSUE: reference to a compiler-generated method
      AudioManager.instance?.UpdateAudioListener(transform.position, transform.rotation);
      if (!this.m_CameraInput.isMoving && !MapTilesUISystem.mapTileViewActive)
        return;
      Action eventCameraMove = this.EventCameraMove;
      if (eventCameraMove == null)
        return;
      eventCameraMove();
    }

    private static bool TryGetPosition(
      Entity e,
      EntityManager entityManager,
      out float3 position,
      out quaternion rotation,
      out float radius)
    {
      int elementIndex = -1;
      Bounds3 bounds;
      // ISSUE: reference to a compiler-generated method
      if (e != Entity.Null && SelectedInfoUISystem.TryGetPosition(e, entityManager, ref elementIndex, out Entity _, out position, out bounds, out rotation, true))
      {
        position.y = MathUtils.Center(bounds.y);
        float3 float3 = (bounds.max - bounds.min) / 2f;
        radius = Mathf.Min(float3.x, float3.y, float3.z);
        return true;
      }
      position = float3.zero;
      rotation = quaternion.identity;
      radius = 0.0f;
      return false;
    }

    public enum Mode
    {
      Follow,
      PhotoMode,
      Editor,
    }
  }
}
