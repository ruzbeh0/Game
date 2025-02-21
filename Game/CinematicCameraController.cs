// Decompiled with JetBrains decompiler
// Type: Game.CinematicCameraController
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Cinemachine;
using Game.Audio;
using Game.Rendering;
using Game.SceneFlow;
using System;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game
{
  public class CinematicCameraController : MonoBehaviour, IGameCameraController
  {
    [SerializeField]
    private float m_MinMoveSpeed = 5f;
    [SerializeField]
    private float m_MaxMoveSpeed = 1000f;
    [SerializeField]
    private float m_MinZoomSpeed = 10f;
    [SerializeField]
    private float m_MaxZoomSpeed = 4000f;
    [SerializeField]
    private float m_RotateSpeed = 0.5f;
    [SerializeField]
    private float m_MaxHeight = 5000f;
    [SerializeField]
    private float m_MaxMovementSpeedHeight = 1000f;
    private Transform m_Anchor;
    private CinemachineVirtualCamera m_VCam;
    private CinemachineRestrictToTerrain m_RestrictToTerrain;
    private CameraInput m_CameraInput;
    private CameraUpdateSystem m_CameraUpdateSystem;

    public ICinemachineCamera virtualCamera => (ICinemachineCamera) this.m_VCam;

    public float zoom
    {
      get => this.m_Anchor.position.y;
      set
      {
        this.m_Anchor.position = this.m_Anchor.position with
        {
          y = value
        };
      }
    }

    public Vector3 pivot
    {
      get => this.m_Anchor.position;
      set => this.m_Anchor.position = value;
    }

    public Vector3 position
    {
      get => this.pivot;
      set => this.pivot = value;
    }

    public Vector3 rotation
    {
      get => this.m_Anchor.rotation.eulerAngles;
      set => this.m_Anchor.rotation = Quaternion.Euler(value);
    }

    public bool controllerEnabled
    {
      get => this.isActiveAndEnabled;
      set => this.gameObject.SetActive(value);
    }

    public bool collisionsEnabled
    {
      get => this.m_RestrictToTerrain.enableObjectCollisions;
      set => this.m_RestrictToTerrain.enableObjectCollisions = value;
    }

    public ref LensSettings lens => ref this.m_VCam.m_Lens;

    public Action eventCameraMove { get; set; }

    public float fov
    {
      get => this.m_VCam.m_Lens.FieldOfView;
      set => this.m_VCam.m_Lens.FieldOfView = value;
    }

    public float dutch
    {
      get => this.m_VCam.m_Lens.Dutch;
      set => this.m_VCam.m_Lens.Dutch = value;
    }

    public bool inputEnabled { get; set; } = true;

    private async void Awake()
    {
      CinematicCameraController cameraController = this;
      if (!await GameManager.instance.WaitForReadyState())
        return;
      cameraController.m_Anchor = new GameObject("CinematicCameraControllerAnchor").transform;
      cameraController.m_VCam = cameraController.GetComponent<CinemachineVirtualCamera>();
      cameraController.m_VCam.Follow = cameraController.m_Anchor;
      cameraController.m_RestrictToTerrain = cameraController.GetComponent<CinemachineRestrictToTerrain>();
      cameraController.m_CameraInput = cameraController.GetComponent<CameraInput>();
      if ((UnityEngine.Object) cameraController.m_CameraInput != (UnityEngine.Object) null)
        cameraController.m_CameraInput.Initialize();
      cameraController.m_CameraUpdateSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<CameraUpdateSystem>();
      cameraController.m_CameraUpdateSystem.cinematicCameraController = cameraController;
      cameraController.gameObject.SetActive(false);
    }

    public void TryMatchPosition(IGameCameraController other)
    {
      this.position = other.position;
      this.rotation = other.rotation;
    }

    public void UpdateCamera()
    {
      if ((UnityEngine.Object) this.m_CameraInput != (UnityEngine.Object) null)
      {
        this.m_CameraInput.Refresh();
        if (this.m_CameraInput.any)
        {
          Action eventCameraMove = this.eventCameraMove;
          if (eventCameraMove != null)
            eventCameraMove();
        }
        if (this.inputEnabled)
          this.UpdateController(this.m_CameraInput);
      }
      // ISSUE: reference to a compiler-generated method
      AudioManager.instance?.UpdateAudioListener(this.transform.position, this.transform.rotation);
    }

    private void UpdateController(CameraInput input)
    {
      this.m_RestrictToTerrain.Refresh();
      Vector3 position1 = this.m_Anchor.position;
      float terrainHeight1;
      this.m_RestrictToTerrain.ClampToTerrain(position1, true, out terrainHeight1);
      float t = Mathf.Min(position1.y - terrainHeight1, this.m_MaxMovementSpeedHeight) / this.m_MaxMovementSpeedHeight;
      Vector2 vector2_1 = input.move * Mathf.Lerp(this.m_MinMoveSpeed, this.m_MaxMoveSpeed, t);
      Vector2 vector2_2 = input.rotate * this.m_RotateSpeed;
      float num = input.zoom * Mathf.Lerp(this.m_MinZoomSpeed, this.m_MaxZoomSpeed, t);
      Vector3 eulerAngles = this.m_Anchor.rotation.eulerAngles;
      float terrainHeight2;
      Vector3 terrain = this.m_RestrictToTerrain.ClampToTerrain(position1 + Quaternion.AngleAxis(eulerAngles.y, Vector3.up) * new Vector3(vector2_1.x, -num, vector2_1.y), true, out terrainHeight2);
      terrain.y = Mathf.Min(terrain.y, terrainHeight2 + this.m_MaxHeight);
      Quaternion rotation = Quaternion.Euler(Mathf.Clamp((float) (((double) eulerAngles.x + 90.0) % 360.0) - vector2_2.y, 0.0f, 180f) - 90f, eulerAngles.y + vector2_2.x, 0.0f);
      Vector3 position2;
      this.m_Anchor.position = !this.m_RestrictToTerrain.enableObjectCollisions || !this.m_RestrictToTerrain.CheckForCollision(terrain, this.m_RestrictToTerrain.previousPosition, rotation, out position2) ? terrain : position2;
      this.m_Anchor.rotation = rotation;
    }

    private void OnDestroy()
    {
      if ((UnityEngine.Object) this.m_Anchor != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_Anchor.gameObject);
      if (this.m_CameraUpdateSystem == null)
        return;
      this.m_CameraUpdateSystem.cinematicCameraController = (CinematicCameraController) null;
    }
  }
}
