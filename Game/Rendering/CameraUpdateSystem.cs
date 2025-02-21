// Decompiled with JetBrains decompiler
// Type: Game.Rendering.CameraUpdateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Cinemachine;
using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game.Common;
using Game.Input;
using Game.Settings;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Assertions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class CameraUpdateSystem : GameSystemBase
  {
    private RaycastSystem m_RaycastSystem;
    private Volume m_Volume;
    private DepthOfField m_DepthOfField;
    private HDShadowSettings m_ShadowSettings;
    private float4 m_StoredShadowSplitsAndDistance;
    private float4 m_StoredShadowBorders;
    private InputActivator[] m_CameraActionActivators;
    private InputBarrier[] m_CameraActionBarriers;

    public Viewer activeViewer { get; private set; }

    public CameraController gamePlayController { get; set; }

    public CinematicCameraController cinematicCameraController { get; set; }

    public OrbitCameraController orbitCameraController { get; set; }

    public Camera activeCamera
    {
      get => this.activeViewer?.camera;
      set
      {
        if ((UnityEngine.Object) value == (UnityEngine.Object) null)
        {
          if (this.activeViewer == null)
            return;
          this.activeViewer = (Viewer) null;
          COSystemBase.baseLog.DebugFormat("Resetting activeViewer to null");
        }
        else
        {
          this.activeViewer = new Viewer(value);
          COSystemBase.baseLog.DebugFormat("Setting activeViewer with {0}", (object) value.name);
        }
      }
    }

    public float nearClipPlane { get; private set; }

    public float3 position { get; private set; }

    public float3 direction { get; private set; }

    public float zoom { get; private set; }

    protected override void OnGameLoaded(Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      this.activeCamera = Camera.main;
    }

    public CameraBlend GetBlendWeight(out float weight)
    {
      if (CinemachineCore.Instance.BrainCount > 0)
      {
        CinemachineBrain activeBrain = CinemachineCore.Instance.GetActiveBrain(0);
        if ((UnityEngine.Object) activeBrain != (UnityEngine.Object) null && activeBrain.IsBlending)
        {
          CinemachineBlend activeBlend = activeBrain.ActiveBlend;
          if (activeBlend.IsValid && !activeBlend.IsComplete)
          {
            weight = activeBlend.BlendWeight;
            if (activeBlend.CamB == this.cinematicCameraController.virtualCamera)
              return CameraBlend.ToCinematicCamera;
            if (activeBlend.CamA == this.cinematicCameraController.virtualCamera)
              return CameraBlend.FromCinematicCamera;
          }
        }
      }
      weight = 1f;
      return CameraBlend.None;
    }

    public bool TryGetViewer(out Viewer viewer)
    {
      viewer = this.activeViewer;
      return this.activeViewer != null;
    }

    public bool TryGetLODParameters(out LODParameters lodParameters)
    {
      if (this.activeViewer != null)
        return this.activeViewer.TryGetLODParameters(out lodParameters);
      lodParameters = new LODParameters();
      return false;
    }

    private bool CheckOrCacheViewer()
    {
      if (this.activeViewer != null && (UnityEngine.Object) this.activeViewer.camera != (UnityEngine.Object) null)
      {
        this.nearClipPlane = this.activeViewer.nearClipPlane;
        this.position = this.activeViewer.position;
        this.direction = this.activeViewer.forward;
        IGameCameraController cameraController = this.activeCameraController;
        this.zoom = cameraController != null ? cameraController.zoom : this.zoom;
        // ISSUE: reference to a compiler-generated field
        this.activeViewer.Raycast(this.m_RaycastSystem);
        return true;
      }
      this.nearClipPlane = 0.0f;
      this.position = float3.zero;
      this.direction = new float3(0.0f, 0.0f, 1f);
      this.activeCamera = (Camera) null;
      this.zoom = 0.0f;
      return false;
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RaycastSystem = this.World.GetOrCreateSystemManaged<RaycastSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_Volume = VolumeHelper.CreateVolume("CameraControllerVolume", 51);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      VolumeHelper.GetOrCreateVolumeComponent<DepthOfField>(this.m_Volume, ref this.m_DepthOfField);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      VolumeHelper.GetOrCreateVolumeComponent<HDShadowSettings>(this.m_Volume, ref this.m_ShadowSettings);
      ProxyActionMap actionMap = InputManager.instance.FindActionMap("Camera");
      // ISSUE: reference to a compiler-generated field
      this.m_CameraActionActivators = actionMap.actions.Values.Select<ProxyAction, InputActivator>((Func<ProxyAction, InputActivator>) (a => new InputActivator(true, "CameraUpdateSystem(" + a.name + ")", a))).ToArray<InputActivator>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraActionBarriers = actionMap.actions.Values.Select<ProxyAction, InputBarrier>((Func<ProxyAction, InputBarrier>) (a => new InputBarrier("CameraUpdateSystem(" + a.name + ")", a, InputManager.DeviceType.Mouse))).ToArray<InputBarrier>();
    }

    private void UpdateDepthOfField(float distance)
    {
      Game.Settings.GraphicsSettings graphics = SharedSettings.instance?.graphics;
      if (graphics == null)
        return;
      if (graphics.depthOfFieldMode == Game.Settings.GraphicsSettings.DepthOfFieldMode.TiltShift)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_DepthOfField.focusMode.Override(UnityEngine.Rendering.HighDefinition.DepthOfFieldMode.Manual);
        // ISSUE: reference to a compiler-generated field
        this.m_DepthOfField.nearFocusStart.Override(distance - distance * graphics.tiltShiftNearStart);
        // ISSUE: reference to a compiler-generated field
        this.m_DepthOfField.nearFocusEnd.Override(distance - distance * graphics.tiltShiftNearEnd);
        // ISSUE: reference to a compiler-generated field
        this.m_DepthOfField.farFocusStart.Override(distance + distance * graphics.tiltShiftFarStart);
        // ISSUE: reference to a compiler-generated field
        this.m_DepthOfField.farFocusEnd.Override(distance + distance * graphics.tiltShiftFarEnd);
      }
      else if (graphics.depthOfFieldMode == Game.Settings.GraphicsSettings.DepthOfFieldMode.Physical)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_DepthOfField.focusMode.Override(UnityEngine.Rendering.HighDefinition.DepthOfFieldMode.UsePhysicalCamera);
        // ISSUE: reference to a compiler-generated field
        this.m_DepthOfField.focusDistanceMode.Override(FocusDistanceMode.Volume);
        // ISSUE: reference to a compiler-generated field
        this.m_DepthOfField.focusDistance.Override(distance);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_DepthOfField.focusMode.Override(UnityEngine.Rendering.HighDefinition.DepthOfFieldMode.Off);
      }
    }

    private void UpdateShadows(Viewer viewer)
    {
      Camera camera = viewer.camera;
      if (!(bool) (UnityEngine.Object) camera)
        return;
      // ISSUE: reference to a compiler-generated field
      if ((double) math.lengthsq(this.m_StoredShadowSplitsAndDistance) == 0.0)
      {
        HDCamera hdCamera = HDCamera.GetOrCreate(camera);
        if (hdCamera != null)
        {
          HDShadowSettings component = hdCamera.volumeStack.GetComponent<HDShadowSettings>();
          float w = component.maxShadowDistance.value;
          float[] cascadeShadowSplits = component.cascadeShadowSplits;
          float[] cascadeShadowBorders = component.cascadeShadowBorders;
          // ISSUE: reference to a compiler-generated field
          this.m_StoredShadowSplitsAndDistance = new float4(cascadeShadowSplits[0] * w, cascadeShadowSplits[1] * w, cascadeShadowSplits[2] * w, w);
          // ISSUE: reference to a compiler-generated field
          this.m_StoredShadowBorders = new float4(cascadeShadowBorders[0], cascadeShadowBorders[1], cascadeShadowBorders[2], cascadeShadowBorders[3]);
        }
      }
      if (!viewer.shadowsAdjustFarDistance)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ShadowSettings.maxShadowDistance.overrideState = false;
        // ISSUE: reference to a compiler-generated field
        this.m_ShadowSettings.cascadeShadowSplit0.overrideState = false;
        // ISSUE: reference to a compiler-generated field
        this.m_ShadowSettings.cascadeShadowSplit1.overrideState = false;
        // ISSUE: reference to a compiler-generated field
        this.m_ShadowSettings.cascadeShadowSplit2.overrideState = false;
        // ISSUE: reference to a compiler-generated field
        this.m_ShadowSettings.cascadeShadowBorder0.overrideState = false;
        // ISSUE: reference to a compiler-generated field
        this.m_ShadowSettings.cascadeShadowBorder1.overrideState = false;
        // ISSUE: reference to a compiler-generated field
        this.m_ShadowSettings.cascadeShadowBorder2.overrideState = false;
        // ISSUE: reference to a compiler-generated field
        this.m_ShadowSettings.cascadeShadowBorder3.overrideState = false;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        float w = this.m_StoredShadowSplitsAndDistance.w;
        ViewerDistances viewerDistances = viewer.viewerDistances;
        double farthestSurface = (double) viewerDistances.farthestSurface;
        viewerDistances = viewer.viewerDistances;
        double distanceToSeaLevel = (double) viewerDistances.maxDistanceToSeaLevel;
        float y1 = math.lerp((float) farthestSurface, (float) distanceToSeaLevel, 0.2f) * 1.1f;
        float x1 = math.min(w, y1);
        // ISSUE: reference to a compiler-generated field
        float x2 = this.m_StoredShadowSplitsAndDistance.x;
        // ISSUE: reference to a compiler-generated field
        float y2 = this.m_StoredShadowSplitsAndDistance.y;
        // ISSUE: reference to a compiler-generated field
        float z = this.m_StoredShadowSplitsAndDistance.z;
        float x3 = math.clamp(x2, 15f, x1 * 0.15f);
        float x4 = math.clamp(y2, 45f, x1 * 0.3f);
        float x5 = math.clamp(z, 135f, x1 * 0.6f);
        float ground = viewer.viewerDistances.ground;
        float num1 = math.min(x3, ground * 5f);
        float num2 = math.min(x4, ground * 30f);
        float num3 = math.min(x5, ground * 200f);
        float x6 = math.max(x1, num3 * 1.2f);
        // ISSUE: reference to a compiler-generated field
        this.m_ShadowSettings.maxShadowDistance.Override(x6);
        // ISSUE: reference to a compiler-generated field
        this.m_ShadowSettings.cascadeShadowSplit0.Override(num1 / x6);
        // ISSUE: reference to a compiler-generated field
        this.m_ShadowSettings.cascadeShadowSplit1.Override(num2 / x6);
        // ISSUE: reference to a compiler-generated field
        this.m_ShadowSettings.cascadeShadowSplit2.Override(num3 / x6);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ShadowSettings.cascadeShadowBorder0.Override(this.m_StoredShadowBorders.x);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ShadowSettings.cascadeShadowBorder1.Override(this.m_StoredShadowBorders.y);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ShadowSettings.cascadeShadowBorder2.Override(this.m_StoredShadowBorders.z);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ShadowSettings.cascadeShadowBorder3.Override(this.m_StoredShadowBorders.w);
      }
    }

    [Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      VolumeHelper.DestroyVolume(this.m_Volume);
      if ((UnityEngine.Object) this.gamePlayController != (UnityEngine.Object) null)
        this.gamePlayController.controllerEnabled = false;
      if ((UnityEngine.Object) this.cinematicCameraController != (UnityEngine.Object) null)
        this.cinematicCameraController.controllerEnabled = false;
      if (!((UnityEngine.Object) this.orbitCameraController != (UnityEngine.Object) null))
        return;
      this.orbitCameraController.controllerEnabled = false;
    }

    public IGameCameraController activeCameraController
    {
      get
      {
        if ((UnityEngine.Object) this.gamePlayController != (UnityEngine.Object) null && this.gamePlayController.controllerEnabled)
        {
          Assert.IsFalse((UnityEngine.Object) this.cinematicCameraController != (UnityEngine.Object) null && this.cinematicCameraController.controllerEnabled);
          Assert.IsFalse((UnityEngine.Object) this.orbitCameraController != (UnityEngine.Object) null && this.orbitCameraController.controllerEnabled);
          return (IGameCameraController) this.gamePlayController;
        }
        if ((UnityEngine.Object) this.cinematicCameraController != (UnityEngine.Object) null && this.cinematicCameraController.controllerEnabled)
        {
          Assert.IsFalse((UnityEngine.Object) this.gamePlayController != (UnityEngine.Object) null && this.gamePlayController.controllerEnabled);
          Assert.IsFalse((UnityEngine.Object) this.orbitCameraController != (UnityEngine.Object) null && this.orbitCameraController.controllerEnabled);
          return (IGameCameraController) this.cinematicCameraController;
        }
        if (!((UnityEngine.Object) this.orbitCameraController != (UnityEngine.Object) null) || !this.orbitCameraController.controllerEnabled)
          return (IGameCameraController) null;
        Assert.IsFalse((UnityEngine.Object) this.gamePlayController != (UnityEngine.Object) null && this.gamePlayController.controllerEnabled);
        Assert.IsFalse((UnityEngine.Object) this.cinematicCameraController != (UnityEngine.Object) null && this.cinematicCameraController.controllerEnabled);
        return (IGameCameraController) this.orbitCameraController;
      }
      set
      {
        if ((UnityEngine.Object) this.gamePlayController != (UnityEngine.Object) null && value != this.gamePlayController)
          this.gamePlayController.controllerEnabled = false;
        if ((UnityEngine.Object) this.cinematicCameraController != (UnityEngine.Object) null && value != this.cinematicCameraController)
          this.cinematicCameraController.controllerEnabled = false;
        if ((UnityEngine.Object) this.orbitCameraController != (UnityEngine.Object) null && value != this.orbitCameraController)
          this.orbitCameraController.controllerEnabled = false;
        if (value == null)
          return;
        value.controllerEnabled = true;
      }
    }

    [Preserve]
    protected override void OnUpdate()
    {
      int num = this.activeViewer == null ? 0 : ((UnityEngine.Object) this.activeViewer.camera != (UnityEngine.Object) null ? 1 : 0);
      float distance = 0.0f;
      if (num != 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.activeViewer.UpdateRaycast(this.m_RaycastSystem, this.CheckedStateRef.WorldUnmanaged.Time.DeltaTime);
        distance = this.activeViewer.viewerDistances.focus;
        // ISSUE: reference to a compiler-generated method
        this.UpdateShadows(this.activeViewer);
      }
      // ISSUE: reference to a compiler-generated method
      this.UpdateDepthOfField(distance);
      this.activeCameraController?.UpdateCamera();
      for (int index = 0; index < CinemachineCore.Instance.BrainCount; ++index)
        CinemachineCore.Instance.GetActiveBrain(index).ManualUpdate();
      // ISSUE: reference to a compiler-generated method
      this.CheckOrCacheViewer();
      // ISSUE: reference to a compiler-generated method
      this.RefreshInput();
    }

    private void RefreshInput()
    {
      // ISSUE: reference to a compiler-generated field
      foreach (InputActivator cameraActionActivator in this.m_CameraActionActivators)
        cameraActionActivator.enabled = this.activeCameraController != null;
      // ISSUE: reference to a compiler-generated field
      foreach (InputBarrier cameraActionBarrier in this.m_CameraActionBarriers)
      {
        if (this.activeCameraController == null)
          cameraActionBarrier.blocked = false;
        else if (!InputManager.instance.mouseOverUI)
          cameraActionBarrier.blocked = false;
        else if (cameraActionBarrier.actions.All<ProxyAction>((Func<ProxyAction, bool>) (a => !a.IsInProgress())))
          cameraActionBarrier.blocked = true;
      }
    }

    [Preserve]
    public CameraUpdateSystem()
    {
    }
  }
}
