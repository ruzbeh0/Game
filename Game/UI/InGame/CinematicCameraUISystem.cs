// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.CinematicCameraUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.IO.AssetDatabase;
using Colossal.Mathematics;
using Colossal.PSI.Environment;
using Colossal.Serialization.Entities;
using Colossal.UI.Binding;
using Game.Assets;
using Game.CinematicCamera;
using Game.Input;
using Game.Rendering;
using Game.Rendering.CinematicCamera;
using Game.SceneFlow;
using Game.Settings;
using Game.Tutorials;
using Game.UI.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class CinematicCameraUISystem : UISystemBase
  {
    private static readonly string kGroup = "cinematicCamera";
    private static readonly CinematicCameraSequence.CinematicCameraCurveModifier[] kEmptyModifierArray = Array.Empty<CinematicCameraSequence.CinematicCameraCurveModifier>();
    private static readonly string kCaptureKeyframeTutorialTag = "CinematicCameraPanelCaptureKey";
    private PhotoModeRenderSystem m_PhotoModeRenderSystem;
    private TutorialUITriggerSystem m_TutorialUITriggerSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private GetterValueBinding<CinematicCameraAsset[]> m_Assets;
    private ValueBinding<CinematicCameraAsset> m_LastLoaded;
    private ValueBinding<CinematicCameraSequence.CinematicCameraCurveModifier[]> m_TransformAnimationCurveBinding;
    private ValueBinding<CinematicCameraSequence.CinematicCameraCurveModifier[]> m_ModifierAnimationCurveBinding;
    private GetterValueBinding<List<string>> m_AvailableCloudTargetsBinding;
    private GetterValueBinding<string> m_SelectedCloudTargetBinding;
    private CinematicCameraSequence m_ActiveAutoplaySequence;
    private IGameCameraController m_PreviousController;
    private ProxyAction m_MoveAction;
    private ProxyAction m_ZoomAction;
    private ProxyAction m_RotateAction;
    private bool m_Playing;

    public CinematicCameraSequence activeSequence { get; set; } = new CinematicCameraSequence();

    private float m_TimelinePositionBindingValue => MathUtils.Snap(this.t, 0.05f);

    private float t { get; set; }

    private bool playing
    {
      get => this.m_Playing;
      set
      {
        if (value == this.m_Playing)
          return;
        this.m_CameraUpdateSystem.cinematicCameraController.inputEnabled = !value;
        this.m_CameraUpdateSystem.orbitCameraController.inputEnabled = !value;
        if (!this.m_Playing)
        {
          this.m_PreviousController = this.m_CameraUpdateSystem.activeCameraController;
          this.m_CameraUpdateSystem.activeCameraController = (IGameCameraController) this.m_CameraUpdateSystem.cinematicCameraController;
        }
        else
          this.m_CameraUpdateSystem.activeCameraController = this.m_PreviousController;
        this.m_Playing = value;
      }
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<float>(CinematicCameraUISystem.kGroup, "setPlaybackDuration", (Action<float>) (duration => this.activeSequence.playbackDuration = Mathf.Max(duration, this.activeSequence.timelineLength))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<float>(CinematicCameraUISystem.kGroup, "setTimelinePosition", (Action<float>) (position =>
      {
        this.playing = false;
        this.t = position;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.activeSequence.Refresh(position, (IDictionary<string, PhotoModeProperty>) this.m_PhotoModeRenderSystem.photoModeProperties, this.m_CameraUpdateSystem.activeCameraController);
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding(CinematicCameraUISystem.kGroup, "togglePlayback", (Action) (() =>
      {
        this.playing = !this.playing;
        if ((double) this.t <= (double) this.activeSequence.playbackDuration - 0.10000000149011612)
          return;
        this.t = 0.0f;
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding(CinematicCameraUISystem.kGroup, "stopPlayback", (Action) (() =>
      {
        this.t = 0.0f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.activeSequence.Refresh(this.t, (IDictionary<string, PhotoModeProperty>) this.m_PhotoModeRenderSystem.photoModeProperties, this.m_CameraUpdateSystem.activeCameraController);
        this.playing = false;
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<string, string>(CinematicCameraUISystem.kGroup, "captureKey", (Action<string, string>) ((id, property) =>
      {
        if (id == "Property")
        {
          // ISSUE: reference to a compiler-generated field
          foreach (PhotoModeProperty photoModeProperty in (IEnumerable<PhotoModeProperty>) this.m_PhotoModeRenderSystem.photoModeProperties.Values)
          {
            if (PhotoModeUtils.ExtractPropertyID(photoModeProperty) == property)
            {
              // ISSUE: reference to a compiler-generated method
              this.ToggleModifier(photoModeProperty);
              break;
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.OnCaptureTransform();
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding<int, int>(CinematicCameraUISystem.kGroup, "removeCameraTransformKey", (Action<int, int>) ((curveIndex, index) => this.OnRemoveKeyFrame("Transform", curveIndex, index))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new CallBinding<string, int, int, Keyframe, int>(CinematicCameraUISystem.kGroup, "moveKeyFrame", (Func<string, int, int, Keyframe, int>) ((id, curveIndex, index, keyframe) =>
      {
        CinematicCameraSequence.CinematicCameraCurveModifier[] modifiers;
        ValueBinding<CinematicCameraSequence.CinematicCameraCurveModifier[]> binding;
        // ISSUE: reference to a compiler-generated method
        this.GetData(id, out modifiers, out binding);
        int num = this.activeSequence.MoveKeyframe(modifiers[curveIndex], index, keyframe);
        binding.Update(modifiers);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.activeSequence.Refresh(this.t, (IDictionary<string, PhotoModeProperty>) this.m_PhotoModeRenderSystem.photoModeProperties, this.m_CameraUpdateSystem.activeCameraController);
        return num;
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<string, int, int>(CinematicCameraUISystem.kGroup, "removeKeyFrame", (Action<string, int, int>) ((id, curveIndex, index) =>
      {
        if (id == "Property")
        {
          this.activeSequence.RemoveModifierKey(this.activeSequence.modifiers[curveIndex].id, index);
          // ISSUE: reference to a compiler-generated field
          this.m_ModifierAnimationCurveBinding.Update(this.activeSequence.modifiers.ToArray());
        }
        else
        {
          this.activeSequence.RemoveCameraTransform(curveIndex, index);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_TransformAnimationCurveBinding.Update(this.GetTransformCurves());
        }
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new CallBinding<string, float, float, int, int>(CinematicCameraUISystem.kGroup, "addKeyFrame", (Func<string, float, float, int, int>) ((id, time, value, curveIndex) =>
      {
        if (id == "Property")
        {
          int num = this.activeSequence.AddModifierKey(this.activeSequence.modifiers[curveIndex].id, time, value);
          // ISSUE: reference to a compiler-generated field
          this.m_ModifierAnimationCurveBinding.Update(this.activeSequence.modifiers.ToArray());
          return num;
        }
        int num1 = this.activeSequence.transforms[curveIndex].curve.AddKey(time, value);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_TransformAnimationCurveBinding.Update(this.GetTransformCurves());
        return num1;
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding(CinematicCameraUISystem.kGroup, "reset", (Action) (() =>
      {
        this.activeSequence.Reset();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_TransformAnimationCurveBinding.Update(this.GetTransformCurves());
        // ISSUE: reference to a compiler-generated field
        this.m_ModifierAnimationCurveBinding.Update(this.activeSequence.modifiers.ToArray());
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>(CinematicCameraUISystem.kGroup, "loop", (Func<bool>) (() => this.activeSequence.loop)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<bool>(CinematicCameraUISystem.kGroup, "toggleLoop", (Action<bool>) (loop =>
      {
        this.activeSequence.loop = loop;
        if (!loop)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_ModifierAnimationCurveBinding.Update(this.activeSequence.modifiers.ToArray());
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_TransformAnimationCurveBinding.Update(this.GetTransformCurves());
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new CallBinding<float[]>(CinematicCameraUISystem.kGroup, "getControllerDelta", (Func<float[]>) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        Vector2 vector2 = this.m_MoveAction.ReadValue<Vector2>() * UnityEngine.Time.deltaTime;
        return new float[2]{ vector2.x, vector2.y };
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new CallBinding<float[]>(CinematicCameraUISystem.kGroup, "getControllerPanDelta", (Func<float[]>) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        Vector2 vector2 = this.m_RotateAction.ReadValue<Vector2>() * UnityEngine.Time.deltaTime;
        return new float[2]{ vector2.x, vector2.y };
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new CallBinding<float>(CinematicCameraUISystem.kGroup, "getControllerZoomDelta", (Func<float>) (() => this.m_ZoomAction.ReadValue<float>() * UnityEngine.Time.deltaTime)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<bool>(CinematicCameraUISystem.kGroup, "toggleCurveEditorFocus", (Action<bool>) (focused =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.orbitCameraController.inputEnabled = !focused;
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.cinematicCameraController.inputEnabled = !focused;
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>(CinematicCameraUISystem.kGroup, "playbackDuration", (Func<float>) (() => this.activeSequence.playbackDuration)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding(CinematicCameraUISystem.kGroup, "onAfterPlaybackDurationChange", (Action) (() => this.activeSequence.AfterModifications())));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>(CinematicCameraUISystem.kGroup, "timelinePosition", (Func<float>) (() => this.m_TimelinePositionBindingValue)));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>(CinematicCameraUISystem.kGroup, "timelineLength", (Func<float>) (() => this.activeSequence.timelineLength)));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>(CinematicCameraUISystem.kGroup, "playing", (Func<bool>) (() => this.playing)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<string, string>(CinematicCameraUISystem.kGroup, "save", (Action<string, string>) ((name, hash) =>
      {
        ILocalAssetDatabase db = MenuHelpers.GetSanitizedCloudTarget(SharedSettings.instance.userState.lastCloudTarget).db;
        if (string.IsNullOrEmpty(hash))
        {
          AssetDataPath name1 = (AssetDataPath) name;
          if (!db.dataSource.isRemoteStorageSource)
          {
            string specialPath = EnvPath.GetSpecialPath<CinematicCameraAsset>();
            if (specialPath != null)
              name1 = AssetDataPath.Create(specialPath, name);
          }
          CinematicCameraAsset newValue = db.AddAsset<CinematicCameraAsset>(name1);
          newValue.target = this.activeSequence;
          newValue.Save(false);
          // ISSUE: reference to a compiler-generated field
          this.m_LastLoaded.Update(newValue);
          // ISSUE: reference to a compiler-generated field
          this.m_Assets.Update();
        }
        else
        {
          Colossal.Hash128 guid = new Colossal.Hash128(hash);
          CinematicCameraAsset asset = db.GetAsset<CinematicCameraAsset>(guid);
          if (!((AssetData) asset != (IAssetData) null))
            return;
          asset.target = this.activeSequence;
          asset.Save(false);
          // ISSUE: reference to a compiler-generated field
          this.m_LastLoaded.Update(asset);
          // ISSUE: reference to a compiler-generated field
          this.m_Assets.Update();
        }
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<string, string>(CinematicCameraUISystem.kGroup, "load", (Action<string, string>) ((hash, storage) =>
      {
        Colossal.Hash128 guid = new Colossal.Hash128(hash);
        CinematicCameraAsset asset = MenuHelpers.GetSanitizedCloudTarget(storage).db.GetAsset<CinematicCameraAsset>(guid);
        if (!((AssetData) asset != (IAssetData) null))
          return;
        asset.Load();
        if (asset.target == null)
          return;
        this.activeSequence = asset.target;
        // ISSUE: reference to a compiler-generated field
        this.m_LastLoaded.Update(asset);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_TransformAnimationCurveBinding.Update(this.GetTransformCurves());
        // ISSUE: reference to a compiler-generated field
        this.m_ModifierAnimationCurveBinding.Update(this.activeSequence.modifiers.ToArray());
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_LastLoaded = new ValueBinding<CinematicCameraAsset>(CinematicCameraUISystem.kGroup, "lastLoaded", (CinematicCameraAsset) null, (IWriter<CinematicCameraAsset>) new ValueWriter<CinematicCameraAsset>().Nullable<CinematicCameraAsset>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_Assets = new GetterValueBinding<CinematicCameraAsset[]>(CinematicCameraUISystem.kGroup, "assets", (Func<CinematicCameraAsset[]>) (() => Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<CinematicCameraAsset>(new SearchFilter<CinematicCameraAsset>()).ToArray<CinematicCameraAsset>()), (IWriter<CinematicCameraAsset[]>) new ArrayWriter<CinematicCameraAsset>((IWriter<CinematicCameraAsset>) new ValueWriter<CinematicCameraAsset>()))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<string, string>(CinematicCameraUISystem.kGroup, "delete", (Action<string, string>) ((hash, storage) =>
      {
        Colossal.Hash128 guid = new Colossal.Hash128(hash);
        MenuHelpers.GetSanitizedCloudTarget(storage).db.DeleteAsset(guid);
        // ISSUE: reference to a compiler-generated field
        this.m_Assets.Update();
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_TransformAnimationCurveBinding = new ValueBinding<CinematicCameraSequence.CinematicCameraCurveModifier[]>(CinematicCameraUISystem.kGroup, "transformAnimationCurves", CinematicCameraUISystem.kEmptyModifierArray, (IWriter<CinematicCameraSequence.CinematicCameraCurveModifier[]>) new ListWriter<CinematicCameraSequence.CinematicCameraCurveModifier>((IWriter<CinematicCameraSequence.CinematicCameraCurveModifier>) new ValueWriter<CinematicCameraSequence.CinematicCameraCurveModifier>()))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ModifierAnimationCurveBinding = new ValueBinding<CinematicCameraSequence.CinematicCameraCurveModifier[]>(CinematicCameraUISystem.kGroup, "modifierAnimationCurves", CinematicCameraUISystem.kEmptyModifierArray, (IWriter<CinematicCameraSequence.CinematicCameraCurveModifier[]>) new ListWriter<CinematicCameraSequence.CinematicCameraCurveModifier>((IWriter<CinematicCameraSequence.CinematicCameraCurveModifier>) new ValueWriter<CinematicCameraSequence.CinematicCameraCurveModifier>()))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AvailableCloudTargetsBinding = new GetterValueBinding<List<string>>(CinematicCameraUISystem.kGroup, "availableCloudTargets", new Func<List<string>>(MenuHelpers.GetAvailableCloudTargets), (IWriter<List<string>>) new ListWriter<string>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) (this.m_SelectedCloudTargetBinding = new GetterValueBinding<string>(CinematicCameraUISystem.kGroup, "selectedCloudTarget", (Func<string>) (() => MenuHelpers.GetSanitizedCloudTarget(SharedSettings.instance.userState.lastCloudTarget).name))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<string>(CinematicCameraUISystem.kGroup, "selectCloudTarget", (Action<string>) (cloudTarget => SharedSettings.instance.userState.lastCloudTarget = cloudTarget)));
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialUITriggerSystem = this.World.GetOrCreateSystemManaged<TutorialUITriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PhotoModeRenderSystem = this.World.GetOrCreateSystemManaged<PhotoModeRenderSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MoveAction = InputManager.instance.FindAction("Camera", "Move");
      // ISSUE: reference to a compiler-generated field
      this.m_ZoomAction = InputManager.instance.FindAction("Camera", "Zoom");
      // ISSUE: reference to a compiler-generated field
      this.m_RotateAction = InputManager.instance.FindAction("Camera", "Rotate");
      Colossal.IO.AssetDatabase.AssetDatabase.global.onAssetDatabaseChanged.Subscribe((EventDelegate<AssetChangedEventArgs>) (args => GameManager.instance.RunOnMainThread((Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AvailableCloudTargetsBinding.Update();
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedCloudTargetBinding.Update();
      }))), (Predicate<AssetChangedEventArgs>) (args =>
      {
        ChangeType change = args.change;
        int num;
        switch (change)
        {
          case ChangeType.DatabaseRegistered:
          case ChangeType.DatabaseUnregistered:
            num = 0;
            break;
          default:
            num = change != ChangeType.BulkAssetsChange ? 1 : 0;
            break;
        }
        return num == 0;
      }), AssetChangedEventArgs.Default);
      // ISSUE: reference to a compiler-generated field
      Colossal.IO.AssetDatabase.AssetDatabase.global.onAssetDatabaseChanged.Subscribe<CinematicCameraAsset>((EventDelegate<AssetChangedEventArgs>) (args => GameManager.instance.RunOnMainThread((Action) (() => this.m_Assets.Update()))), AssetChangedEventArgs.Default);
      // ISSUE: reference to a compiler-generated method
      this.Reset();
    }

    public void ToggleModifier(PhotoModeProperty p)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialUITriggerSystem.ActivateTrigger(CinematicCameraUISystem.kCaptureKeyframeTutorialTag);
      // ISSUE: reference to a compiler-generated field
      foreach (PhotoModeProperty propertyComponent in PhotoModeUtils.ExtractMultiPropertyComponents(p, (IDictionary<string, PhotoModeProperty>) this.m_PhotoModeRenderSystem.photoModeProperties))
      {
        if (p.min != null && p.max != null)
          this.activeSequence.AddModifierKey(propertyComponent.id, this.t, propertyComponent.getValue(), propertyComponent.min(), propertyComponent.max());
        else
          this.activeSequence.AddModifierKey(propertyComponent.id, this.t, propertyComponent.getValue());
      }
      // ISSUE: reference to a compiler-generated field
      this.m_ModifierAnimationCurveBinding.Update(this.activeSequence.modifiers.ToArray());
    }

    protected override void OnGameLoaded(Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_CameraUpdateSystem.cinematicCameraController != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.cinematicCameraController.eventCameraMove -= (Action) (() =>
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.playing || !(this.m_CameraUpdateSystem.activeCameraController is CinematicCameraController) && (!(this.m_CameraUpdateSystem.activeCameraController is OrbitCameraController) || this.m_CameraUpdateSystem.orbitCameraController.mode != OrbitCameraController.Mode.PhotoMode))
            return;
          this.playing = false;
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.cinematicCameraController.eventCameraMove += (Action) (() =>
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.playing || !(this.m_CameraUpdateSystem.activeCameraController is CinematicCameraController) && (!(this.m_CameraUpdateSystem.activeCameraController is OrbitCameraController) || this.m_CameraUpdateSystem.orbitCameraController.mode != OrbitCameraController.Mode.PhotoMode))
            return;
          this.playing = false;
        });
      }
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_CameraUpdateSystem.orbitCameraController != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.orbitCameraController.EventCameraMove -= (Action) (() =>
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.playing || !(this.m_CameraUpdateSystem.activeCameraController is CinematicCameraController) && (!(this.m_CameraUpdateSystem.activeCameraController is OrbitCameraController) || this.m_CameraUpdateSystem.orbitCameraController.mode != OrbitCameraController.Mode.PhotoMode))
            return;
          this.playing = false;
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.orbitCameraController.EventCameraMove += (Action) (() =>
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.playing || !(this.m_CameraUpdateSystem.activeCameraController is CinematicCameraController) && (!(this.m_CameraUpdateSystem.activeCameraController is OrbitCameraController) || this.m_CameraUpdateSystem.orbitCameraController.mode != OrbitCameraController.Mode.PhotoMode))
            return;
          this.playing = false;
        });
      }
      if (serializationContext.purpose != Purpose.Cleanup)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveAutoplaySequence = (CinematicCameraSequence) null;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Playing = false;
      // ISSUE: reference to a compiler-generated method
      this.Reset();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (!this.playing)
        return;
      // ISSUE: reference to a compiler-generated method
      this.UpdatePlayback();
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_CameraUpdateSystem?.cinematicCameraController != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.cinematicCameraController.eventCameraMove -= (Action) (() =>
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.playing || !(this.m_CameraUpdateSystem.activeCameraController is CinematicCameraController) && (!(this.m_CameraUpdateSystem.activeCameraController is OrbitCameraController) || this.m_CameraUpdateSystem.orbitCameraController.mode != OrbitCameraController.Mode.PhotoMode))
            return;
          this.playing = false;
        });
      }
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_CameraUpdateSystem?.orbitCameraController != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.orbitCameraController.EventCameraMove -= (Action) (() =>
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.playing || !(this.m_CameraUpdateSystem.activeCameraController is CinematicCameraController) && (!(this.m_CameraUpdateSystem.activeCameraController is OrbitCameraController) || this.m_CameraUpdateSystem.orbitCameraController.mode != OrbitCameraController.Mode.PhotoMode))
            return;
          this.playing = false;
        });
      }
      base.OnDestroy();
    }

    private void OnToggleLoop(bool loop)
    {
      this.activeSequence.loop = loop;
      if (!loop)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_ModifierAnimationCurveBinding.Update(this.activeSequence.modifiers.ToArray());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TransformAnimationCurveBinding.Update(this.GetTransformCurves());
    }

    private int OnAddKeyFrame(string id, float time, float value, int curveIndex)
    {
      if (id == "Property")
      {
        int num = this.activeSequence.AddModifierKey(this.activeSequence.modifiers[curveIndex].id, time, value);
        // ISSUE: reference to a compiler-generated field
        this.m_ModifierAnimationCurveBinding.Update(this.activeSequence.modifiers.ToArray());
        return num;
      }
      int num1 = this.activeSequence.transforms[curveIndex].curve.AddKey(time, value);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TransformAnimationCurveBinding.Update(this.GetTransformCurves());
      return num1;
    }

    private void Reset()
    {
      this.activeSequence.Reset();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TransformAnimationCurveBinding.Update(this.GetTransformCurves());
      // ISSUE: reference to a compiler-generated field
      this.m_ModifierAnimationCurveBinding.Update(this.activeSequence.modifiers.ToArray());
    }

    private void OnSetTimelinePosition(float position)
    {
      this.playing = false;
      this.t = position;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.activeSequence.Refresh(position, (IDictionary<string, PhotoModeProperty>) this.m_PhotoModeRenderSystem.photoModeProperties, this.m_CameraUpdateSystem.activeCameraController);
    }

    private void OnSetPlaybackDuration(float duration)
    {
      this.activeSequence.playbackDuration = Mathf.Max(duration, this.activeSequence.timelineLength);
    }

    private void OnCapture(string id, string property)
    {
      if (id == "Property")
      {
        // ISSUE: reference to a compiler-generated field
        foreach (PhotoModeProperty photoModeProperty in (IEnumerable<PhotoModeProperty>) this.m_PhotoModeRenderSystem.photoModeProperties.Values)
        {
          if (PhotoModeUtils.ExtractPropertyID(photoModeProperty) == property)
          {
            // ISSUE: reference to a compiler-generated method
            this.ToggleModifier(photoModeProperty);
            break;
          }
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.OnCaptureTransform();
      }
    }

    private void OnCaptureTransform()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialUITriggerSystem.ActivateTrigger(CinematicCameraUISystem.kCaptureKeyframeTutorialTag);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.activeSequence.AddCameraTransform(this.t, this.m_CameraUpdateSystem.activeCameraController.position, this.m_CameraUpdateSystem.activeCameraController.rotation);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TransformAnimationCurveBinding.Update(this.GetTransformCurves());
    }

    private void Save(string name, string hash = null)
    {
      ILocalAssetDatabase db = MenuHelpers.GetSanitizedCloudTarget(SharedSettings.instance.userState.lastCloudTarget).db;
      if (string.IsNullOrEmpty(hash))
      {
        AssetDataPath name1 = (AssetDataPath) name;
        if (!db.dataSource.isRemoteStorageSource)
        {
          string specialPath = EnvPath.GetSpecialPath<CinematicCameraAsset>();
          if (specialPath != null)
            name1 = AssetDataPath.Create(specialPath, name);
        }
        CinematicCameraAsset newValue = db.AddAsset<CinematicCameraAsset>(name1);
        newValue.target = this.activeSequence;
        newValue.Save(false);
        // ISSUE: reference to a compiler-generated field
        this.m_LastLoaded.Update(newValue);
        // ISSUE: reference to a compiler-generated field
        this.m_Assets.Update();
      }
      else
      {
        Colossal.Hash128 guid = new Colossal.Hash128(hash);
        CinematicCameraAsset asset = db.GetAsset<CinematicCameraAsset>(guid);
        if (!((AssetData) asset != (IAssetData) null))
          return;
        asset.target = this.activeSequence;
        asset.Save(false);
        // ISSUE: reference to a compiler-generated field
        this.m_LastLoaded.Update(asset);
        // ISSUE: reference to a compiler-generated field
        this.m_Assets.Update();
      }
    }

    private void Load(string hash, string storage)
    {
      Colossal.Hash128 guid = new Colossal.Hash128(hash);
      CinematicCameraAsset asset = MenuHelpers.GetSanitizedCloudTarget(storage).db.GetAsset<CinematicCameraAsset>(guid);
      if (!((AssetData) asset != (IAssetData) null))
        return;
      asset.Load();
      if (asset.target == null)
        return;
      this.activeSequence = asset.target;
      // ISSUE: reference to a compiler-generated field
      this.m_LastLoaded.Update(asset);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TransformAnimationCurveBinding.Update(this.GetTransformCurves());
      // ISSUE: reference to a compiler-generated field
      this.m_ModifierAnimationCurveBinding.Update(this.activeSequence.modifiers.ToArray());
    }

    private void Delete(string hash, string storage)
    {
      Colossal.Hash128 guid = new Colossal.Hash128(hash);
      MenuHelpers.GetSanitizedCloudTarget(storage).db.DeleteAsset(guid);
      // ISSUE: reference to a compiler-generated field
      this.m_Assets.Update();
    }

    private void OnAssetsChanged(AssetChangedEventArgs args)
    {
      // ISSUE: reference to a compiler-generated field
      GameManager.instance.RunOnMainThread((Action) (() => this.m_Assets.Update()));
    }

    private void OnCloudTargetsChanged(AssetChangedEventArgs args)
    {
      GameManager.instance.RunOnMainThread((Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AvailableCloudTargetsBinding.Update();
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedCloudTargetBinding.Update();
      }));
    }

    private CinematicCameraAsset[] UpdateAssets()
    {
      return Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<CinematicCameraAsset>(new SearchFilter<CinematicCameraAsset>()).ToArray<CinematicCameraAsset>();
    }

    private void OnRemoveSelectedTransform(int curveIndex, int index)
    {
      // ISSUE: reference to a compiler-generated method
      this.OnRemoveKeyFrame("Transform", curveIndex, index);
    }

    private void OnRemoveKeyFrame(string id, int curveIndex, int index)
    {
      if (id == "Property")
      {
        this.activeSequence.RemoveModifierKey(this.activeSequence.modifiers[curveIndex].id, index);
        // ISSUE: reference to a compiler-generated field
        this.m_ModifierAnimationCurveBinding.Update(this.activeSequence.modifiers.ToArray());
      }
      else
      {
        this.activeSequence.RemoveCameraTransform(curveIndex, index);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_TransformAnimationCurveBinding.Update(this.GetTransformCurves());
      }
    }

    private void GetData(
      string id,
      out CinematicCameraSequence.CinematicCameraCurveModifier[] modifiers,
      out ValueBinding<CinematicCameraSequence.CinematicCameraCurveModifier[]> binding)
    {
      if (id == "Position")
      {
        modifiers = ((IEnumerable<CinematicCameraSequence.CinematicCameraCurveModifier>) this.activeSequence.transforms).ToArray<CinematicCameraSequence.CinematicCameraCurveModifier>();
        // ISSUE: reference to a compiler-generated field
        binding = this.m_TransformAnimationCurveBinding;
      }
      else
      {
        modifiers = this.activeSequence.modifiers.ToArray();
        // ISSUE: reference to a compiler-generated field
        binding = this.m_ModifierAnimationCurveBinding;
      }
    }

    private int OnMoveKeyFrame(string id, int curveIndex, int index, Keyframe keyframe)
    {
      CinematicCameraSequence.CinematicCameraCurveModifier[] modifiers;
      ValueBinding<CinematicCameraSequence.CinematicCameraCurveModifier[]> binding;
      // ISSUE: reference to a compiler-generated method
      this.GetData(id, out modifiers, out binding);
      int num = this.activeSequence.MoveKeyframe(modifiers[curveIndex], index, keyframe);
      binding.Update(modifiers);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.activeSequence.Refresh(this.t, (IDictionary<string, PhotoModeProperty>) this.m_PhotoModeRenderSystem.photoModeProperties, this.m_CameraUpdateSystem.activeCameraController);
      return num;
    }

    private void UpdatePlayback()
    {
      this.t += UnityEngine.Time.unscaledDeltaTime;
      // ISSUE: reference to a compiler-generated field
      CinematicCameraSequence cinematicCameraSequence = this.m_ActiveAutoplaySequence ?? this.activeSequence;
      if ((double) this.t >= (double) cinematicCameraSequence.playbackDuration)
      {
        if (cinematicCameraSequence.loop)
          this.t -= cinematicCameraSequence.playbackDuration;
        else
          this.playing = false;
      }
      this.t = Mathf.Min(this.t, cinematicCameraSequence.playbackDuration);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cinematicCameraSequence.Refresh(this.t, (IDictionary<string, PhotoModeProperty>) this.m_PhotoModeRenderSystem.photoModeProperties, this.m_CameraUpdateSystem.activeCameraController);
    }

    private void PausePlayback()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.playing || !(this.m_CameraUpdateSystem.activeCameraController is CinematicCameraController) && (!(this.m_CameraUpdateSystem.activeCameraController is OrbitCameraController) || this.m_CameraUpdateSystem.orbitCameraController.mode != OrbitCameraController.Mode.PhotoMode))
        return;
      this.playing = false;
    }

    private void TogglePlayback()
    {
      this.playing = !this.playing;
      if ((double) this.t <= (double) this.activeSequence.playbackDuration - 0.10000000149011612)
        return;
      this.t = 0.0f;
    }

    private void StopPlayback()
    {
      this.t = 0.0f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.activeSequence.Refresh(this.t, (IDictionary<string, PhotoModeProperty>) this.m_PhotoModeRenderSystem.photoModeProperties, this.m_CameraUpdateSystem.activeCameraController);
      this.playing = false;
    }

    public void Autoplay(CinematicCameraAsset sequence)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveAutoplaySequence = sequence.target;
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveAutoplaySequence.loop = true;
      this.t = 0.0f;
      this.playing = true;
    }

    public void StopAutoplay()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveAutoplaySequence = (CinematicCameraSequence) null;
      this.t = 0.0f;
      this.playing = false;
    }

    private CinematicCameraSequence.CinematicCameraCurveModifier[] GetTransformCurves()
    {
      if (this.activeSequence.transformCount <= 0)
      {
        // ISSUE: reference to a compiler-generated field
        return CinematicCameraUISystem.kEmptyModifierArray;
      }
      List<CinematicCameraSequence.CinematicCameraCurveModifier> cameraCurveModifierList = new List<CinematicCameraSequence.CinematicCameraCurveModifier>();
      foreach (CinematicCameraSequence.CinematicCameraCurveModifier transform in this.activeSequence.transforms)
      {
        if (transform.curve != null)
          cameraCurveModifierList.Add(transform);
      }
      return cameraCurveModifierList.ToArray();
    }

    private float[] GetControllerDelta()
    {
      // ISSUE: reference to a compiler-generated field
      Vector2 vector2 = this.m_MoveAction.ReadValue<Vector2>() * UnityEngine.Time.deltaTime;
      return new float[2]{ vector2.x, vector2.y };
    }

    private float[] GetControllerPanDelta()
    {
      // ISSUE: reference to a compiler-generated field
      Vector2 vector2 = this.m_RotateAction.ReadValue<Vector2>() * UnityEngine.Time.deltaTime;
      return new float[2]{ vector2.x, vector2.y };
    }

    private float GetControllerZoomDelta() => this.m_ZoomAction.ReadValue<float>() * UnityEngine.Time.deltaTime;

    private void OnCurveEditorFocusChange(bool focused)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem.orbitCameraController.inputEnabled = !focused;
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem.cinematicCameraController.inputEnabled = !focused;
    }

    [Preserve]
    public CinematicCameraUISystem()
    {
    }
  }
}
