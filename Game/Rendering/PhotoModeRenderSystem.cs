// Decompiled with JetBrains decompiler
// Type: Game.Rendering.PhotoModeRenderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections.Generic;
using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game.Rendering.CinematicCamera;
using Game.Simulation;
using Game.UI.InGame;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Rendering
{
  public class PhotoModeRenderSystem : GameSystemBase
  {
    private static readonly string[] kApertureFormatNames = new string[14]
    {
      "8mm",
      "Super 8mm",
      "16mm",
      "Super 16mm",
      "35mm 2-perf",
      "35mm Academy",
      "Super-35",
      "35mm TV Projection",
      "35mm Full Aperture",
      "35mm 1.85 Projection",
      "35mm Anamorphic",
      "65mm ALEXA",
      "70mm",
      "70mm IMAX"
    };
    private static readonly Vector2[] kApertureFormatValues = new Vector2[14]
    {
      new Vector2(4.8f, 3.5f),
      new Vector2(5.79f, 4.01f),
      new Vector2(10.26f, 7.49f),
      new Vector2(12.522f, 7.417f),
      new Vector2(21.95f, 9.35f),
      new Vector2(21.946f, 16.002f),
      new Vector2(24.89f, 18.66f),
      new Vector2(20.726f, 15.545f),
      new Vector2(24.892f, 18.669f),
      new Vector2(20.955f, 11.328f),
      new Vector2(21.946f, 18.593f),
      new Vector2(54.12f, 25.59f),
      new Vector2(52.476f, 23.012f),
      new Vector2(70.41f, 52.63f)
    };
    private const string kSensorTypePreset = "SensorTypePreset";
    private const string kCameraApertureShape = "CameraApertureShape";
    private const string kCameraBody = "CameraBody";
    private const string kCameraLens = "CameraLens";
    private const string kCamera = "Camera";
    private const string kColorGrading = "Color";
    private const string kLens = "Lens";
    private const string kWeather = "Weather";
    private const string kEnvironment = "Environment";
    private Volume m_CameraControlVolume;
    private ColorAdjustments m_ColorAdjustments;
    private WhiteBalance m_WhiteBalance;
    private PaniniProjection m_PaniniProjection;
    private Vignette m_Vignette;
    private FilmGrain m_FilmGrain;
    private ShadowsMidtonesHighlights m_ShadowsMidtonesHighlights;
    private Bloom m_Bloom;
    private MotionBlur m_MotionBlur;
    private DepthOfField m_DepthOfField;
    private CloudLayer m_DistanceClouds;
    private VolumetricClouds m_VolumetricClouds;
    private Fog m_Fog;
    private PhysicallyBasedSky m_Sky;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private PlanetarySystem m_PlanetarySystem;
    private ClimateSystem m_ClimateSystem;
    private SimulationSystem m_SimulationSystem;
    private OverridableLensProperty<float> focalLength;
    private OverridableLensProperty<Vector2> sensorSize;
    private OverridableLensProperty<float> aperture;
    private OverridableLensProperty<int> iso;
    private OverridableLensProperty<float> shutterSpeed;
    private OverridableLensProperty<Camera.GateFitMode> gateFitMode;
    private OverridableLensProperty<int> bladeCount;
    private OverridableLensProperty<Vector2> curvature;
    private OverridableLensProperty<float> barrelClipping;
    private OverridableLensProperty<float> anamorphism;
    private OverridableLensProperty<float> focusDistance;
    private OverridableLensProperty<Vector2> lensShift;
    private bool m_Active;
    private List<PhotoModeUIPreset> m_Presets = new List<PhotoModeUIPreset>();

    public OrderedDictionary<string, PhotoModeProperty> photoModeProperties { get; private set; }

    public IReadOnlyCollection<PhotoModeUIPreset> presets
    {
      get => (IReadOnlyCollection<PhotoModeUIPreset>) this.m_Presets;
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.Enabled = false;
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      this.m_CameraControlVolume = VolumeHelper.CreateVolume("CinematicControlVolume", 2000);
      VolumeHelper.GetOrCreateVolumeComponent<ColorAdjustments>(this.m_CameraControlVolume, ref this.m_ColorAdjustments);
      VolumeHelper.GetOrCreateVolumeComponent<WhiteBalance>(this.m_CameraControlVolume, ref this.m_WhiteBalance);
      VolumeHelper.GetOrCreateVolumeComponent<PaniniProjection>(this.m_CameraControlVolume, ref this.m_PaniniProjection);
      VolumeHelper.GetOrCreateVolumeComponent<ShadowsMidtonesHighlights>(this.m_CameraControlVolume, ref this.m_ShadowsMidtonesHighlights);
      VolumeHelper.GetOrCreateVolumeComponent<Vignette>(this.m_CameraControlVolume, ref this.m_Vignette);
      VolumeHelper.GetOrCreateVolumeComponent<Bloom>(this.m_CameraControlVolume, ref this.m_Bloom);
      VolumeHelper.GetOrCreateVolumeComponent<MotionBlur>(this.m_CameraControlVolume, ref this.m_MotionBlur);
      VolumeHelper.GetOrCreateVolumeComponent<DepthOfField>(this.m_CameraControlVolume, ref this.m_DepthOfField);
      VolumeHelper.GetOrCreateVolumeComponent<CloudLayer>(this.m_CameraControlVolume, ref this.m_DistanceClouds);
      VolumeHelper.GetOrCreateVolumeComponent<VolumetricClouds>(this.m_CameraControlVolume, ref this.m_VolumetricClouds);
      VolumeHelper.GetOrCreateVolumeComponent<Fog>(this.m_CameraControlVolume, ref this.m_Fog);
      VolumeHelper.GetOrCreateVolumeComponent<PhysicallyBasedSky>(this.m_CameraControlVolume, ref this.m_Sky);
      this.m_Vignette.mode.Override(VignetteMode.Procedural);
      VolumeHelper.GetOrCreateVolumeComponent<FilmGrain>(this.m_CameraControlVolume, ref this.m_FilmGrain);
      this.m_CameraControlVolume.weight = 0.0f;
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      this.m_PlanetarySystem = this.World.GetOrCreateSystemManaged<PlanetarySystem>();
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      this.focalLength = new OverridableLensProperty<float>(this.m_CameraUpdateSystem, (Action<IGameCameraController, float>) ((c, v) => c.lens.FieldOfView = v), (Func<IGameCameraController, float>) (c => c.lens.FieldOfView));
      this.sensorSize = new OverridableLensProperty<Vector2>(this.m_CameraUpdateSystem, (Action<IGameCameraController, Vector2>) ((c, v) => c.lens.SensorSize = v), (Func<IGameCameraController, Vector2>) (c => c.lens.SensorSize));
      this.aperture = new OverridableLensProperty<float>(this.m_CameraUpdateSystem, (Action<IGameCameraController, float>) ((c, v) => c.lens.Aperture = v), (Func<IGameCameraController, float>) (c => c.lens.Aperture));
      this.iso = new OverridableLensProperty<int>(this.m_CameraUpdateSystem, (Action<IGameCameraController, int>) ((c, v) => c.lens.Iso = v), (Func<IGameCameraController, int>) (c => c.lens.Iso));
      this.shutterSpeed = new OverridableLensProperty<float>(this.m_CameraUpdateSystem, (Action<IGameCameraController, float>) ((c, v) => c.lens.ShutterSpeed = v), (Func<IGameCameraController, float>) (c => c.lens.ShutterSpeed));
      this.gateFitMode = new OverridableLensProperty<Camera.GateFitMode>(this.m_CameraUpdateSystem, (Action<IGameCameraController, Camera.GateFitMode>) ((c, v) => c.lens.GateFit = v), (Func<IGameCameraController, Camera.GateFitMode>) (c => c.lens.GateFit));
      this.bladeCount = new OverridableLensProperty<int>(this.m_CameraUpdateSystem, (Action<IGameCameraController, int>) ((c, v) => c.lens.BladeCount = v), (Func<IGameCameraController, int>) (c => c.lens.BladeCount));
      this.curvature = new OverridableLensProperty<Vector2>(this.m_CameraUpdateSystem, (Action<IGameCameraController, Vector2>) ((c, v) => c.lens.Curvature = v), (Func<IGameCameraController, Vector2>) (c => c.lens.Curvature));
      this.barrelClipping = new OverridableLensProperty<float>(this.m_CameraUpdateSystem, (Action<IGameCameraController, float>) ((c, v) => c.lens.BarrelClipping = v), (Func<IGameCameraController, float>) (c => c.lens.BarrelClipping));
      this.anamorphism = new OverridableLensProperty<float>(this.m_CameraUpdateSystem, (Action<IGameCameraController, float>) ((c, v) => c.lens.Anamorphism = v), (Func<IGameCameraController, float>) (c => c.lens.Anamorphism));
      this.focusDistance = new OverridableLensProperty<float>(this.m_CameraUpdateSystem, (Action<IGameCameraController, float>) ((c, v) => c.lens.FocusDistance = v), (Func<IGameCameraController, float>) (c => c.lens.FocusDistance));
      this.lensShift = new OverridableLensProperty<Vector2>(this.m_CameraUpdateSystem, (Action<IGameCameraController, Vector2>) ((c, v) => c.lens.LensShift = v), (Func<IGameCameraController, Vector2>) (c => c.lens.LensShift));
      this.InitializeProperties();
    }

    protected override void OnGameLoaded(Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      this.focalLength.Sync();
      this.sensorSize.Sync();
      this.aperture.Sync();
      this.iso.Sync();
      this.shutterSpeed.Sync();
      this.gateFitMode.Sync();
      this.bladeCount.Sync();
      this.curvature.Sync();
      this.barrelClipping.Sync();
      this.anamorphism.Sync();
      this.focusDistance.Sync();
      this.lensShift.Sync();
    }

    public void Enable(bool enabled)
    {
      this.m_Active = enabled;
      if (!this.m_Active)
        return;
      this.Enabled = enabled;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      float weight;
      // ISSUE: reference to a compiler-generated method
      int blendWeight = (int) this.m_CameraUpdateSystem.GetBlendWeight(out weight);
      if (blendWeight == 2)
        this.m_CameraControlVolume.weight = weight;
      if (blendWeight == 3)
        this.m_CameraControlVolume.weight = 1f - weight;
      if (blendWeight != 0)
        return;
      if (this.m_Active)
      {
        this.m_CameraControlVolume.weight = 1f;
      }
      else
      {
        this.m_CameraControlVolume.weight = 0.0f;
        this.Enabled = false;
      }
    }

    [Preserve]
    protected override void OnDestroy()
    {
      VolumeHelper.DestroyVolume(this.m_CameraControlVolume);
      base.OnDestroy();
    }

    public void AddProperty(PhotoModeProperty property)
    {
      if (property == null)
        return;
      if (!this.photoModeProperties.ContainsKey(property.id))
        this.photoModeProperties.Add(property.id, property);
      else
        COSystemBase.baseLog.WarnFormat("PhotoModeProperty id {0} already exists", (object) property.id);
    }

    public void AddProperty(PhotoModeProperty[] property)
    {
      foreach (PhotoModeProperty property1 in property)
        this.AddProperty(property1);
    }

    private void InitializeProperties()
    {
      this.photoModeProperties = new OrderedDictionary<string, PhotoModeProperty>();
      this.AddProperty(PhotoModeUtils.GroupTitle("Camera", "CameraBody"));
      PhotoModeProperty[] targetProperties;
      this.AddProperty(targetProperties = PhotoModeUtils.BindProperty("Camera", (Expression<Func<OverridableLensProperty<Vector2>>>) (() => this.sensorSize), 0.1f, 1000f));
      this.AddProperty(PhotoModeUtils.BindProperty("Camera", (Expression<Func<OverridableLensProperty<int>>>) (() => this.iso), 200));
      this.AddProperty(PhotoModeUtils.BindProperty("Camera", (Expression<Func<OverridableLensProperty<float>>>) (() => this.shutterSpeed), 0.000125f, 30f));
      this.AddProperty(PhotoModeUtils.BindProperty<Camera.GateFitMode>("Camera", (Expression<Func<OverridableLensProperty<Camera.GateFitMode>>>) (() => this.gateFitMode)));
      this.AddProperty(new PhotoModeProperty()
      {
        id = "Camera collision",
        group = "Camera",
        setValue = (Action<float>) (value =>
        {
          bool boolean = PhotoModeUtils.FloatToBoolean(value);
          if ((UnityEngine.Object) this.m_CameraUpdateSystem.cinematicCameraController != (UnityEngine.Object) null)
            this.m_CameraUpdateSystem.cinematicCameraController.collisionsEnabled = boolean;
          if (!((UnityEngine.Object) this.m_CameraUpdateSystem.orbitCameraController != (UnityEngine.Object) null))
            return;
          this.m_CameraUpdateSystem.orbitCameraController.collisionsEnabled = boolean;
        }),
        getValue = (Func<float>) (() =>
        {
          bool flag = false;
          if ((UnityEngine.Object) this.m_CameraUpdateSystem.cinematicCameraController != (UnityEngine.Object) null)
            flag |= this.m_CameraUpdateSystem.cinematicCameraController.collisionsEnabled;
          if ((UnityEngine.Object) this.m_CameraUpdateSystem.orbitCameraController != (UnityEngine.Object) null)
            flag |= this.m_CameraUpdateSystem.orbitCameraController.collisionsEnabled;
          return PhotoModeUtils.BooleanToFloat(flag);
        }),
        reset = (Action) (() =>
        {
          if ((UnityEngine.Object) this.m_CameraUpdateSystem.cinematicCameraController != (UnityEngine.Object) null)
            this.m_CameraUpdateSystem.cinematicCameraController.collisionsEnabled = false;
          if (!((UnityEngine.Object) this.m_CameraUpdateSystem.orbitCameraController != (UnityEngine.Object) null))
            return;
          this.m_CameraUpdateSystem.orbitCameraController.collisionsEnabled = false;
        }),
        overrideControl = PhotoModeProperty.OverrideControl.Checkbox
      });
      this.AddProperty(PhotoModeUtils.GroupTitle("Camera", "CameraLens"));
      // ISSUE: method pointer
      // ISSUE: method pointer
      // ISSUE: method pointer
      // ISSUE: method pointer
      this.AddProperty(PhotoModeUtils.BindProperty("Camera", (Expression<Func<OverridableLensProperty<float>>>) (() => this.focalLength), new Func<float>((object) this, __methodptr(\u003CInitializeProperties\u003Eg__MinFocalLength\u007C56_4)), new Func<float>((object) this, __methodptr(\u003CInitializeProperties\u003Eg__MaxFocalLength\u007C56_5)), new Func<float, float>((object) this, __methodptr(\u003CInitializeProperties\u003Eg__FieldOfViewToFocalLength\u007C56_1)), new Func<float, float>((object) this, __methodptr(\u003CInitializeProperties\u003Eg__FocalLengthToFieldOfView\u007C56_0))));
      this.AddProperty(PhotoModeUtils.BindProperty("Camera", (Expression<Func<OverridableLensProperty<Vector2>>>) (() => this.lensShift)));
      this.AddProperty(PhotoModeUtils.BindProperty("Camera", (Expression<Func<OverridableLensProperty<float>>>) (() => this.aperture), 0.7f, 32f));
      this.AddProperty(PhotoModeUtils.GroupTitle("Camera", "CameraApertureShape"));
      this.AddProperty(PhotoModeUtils.BindProperty("Camera", (Expression<Func<OverridableLensProperty<int>>>) (() => this.bladeCount), 3, 11));
      this.AddProperty(PhotoModeUtils.BindProperty("Camera", (Expression<Func<OverridableLensProperty<Vector2>>>) (() => this.curvature), 0.7f, 32f));
      this.AddProperty(PhotoModeUtils.BindProperty("Camera", (Expression<Func<OverridableLensProperty<float>>>) (() => this.barrelClipping), 0.0f, 1f));
      this.AddProperty(PhotoModeUtils.BindProperty("Camera", (Expression<Func<OverridableLensProperty<float>>>) (() => this.anamorphism), -1f, 1f));
      this.AddProperty(new PhotoModeProperty()
      {
        id = "Roll",
        group = "Camera",
        setValue = (Action<float>) (value =>
        {
          if (!((UnityEngine.Object) this.m_CameraUpdateSystem.cinematicCameraController != (UnityEngine.Object) null))
            return;
          this.m_CameraUpdateSystem.cinematicCameraController.dutch = value;
        }),
        getValue = (Func<float>) (() => (UnityEngine.Object) this.m_CameraUpdateSystem.cinematicCameraController != (UnityEngine.Object) null ? this.m_CameraUpdateSystem.cinematicCameraController.dutch : 0.0f),
        min = (Func<float>) (() => -45f),
        max = (Func<float>) (() => 45f),
        reset = (Action) (() =>
        {
          if (!((UnityEngine.Object) this.m_CameraUpdateSystem.cinematicCameraController != (UnityEngine.Object) null))
            return;
          this.m_CameraUpdateSystem.cinematicCameraController.dutch = 0.0f;
        })
      });
      this.AddProperty(PhotoModeUtils.GroupTitle("Lens", this.m_DepthOfField.GetType().Name));
      this.AddProperty(PhotoModeUtils.BindProperty<DepthOfFieldMode>("Lens", (Expression<Func<VolumeParameter<DepthOfFieldMode>>>) (() => this.m_DepthOfField.focusMode)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<MinFloatParameter>>) (() => this.m_DepthOfField.focusDistance), (Func<bool>) (() => this.m_DepthOfField.IsActive() && (VolumeParameter<DepthOfFieldMode>) this.m_DepthOfField.focusMode == DepthOfFieldMode.UsePhysicalCamera)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<MinFloatParameter>>) (() => this.m_DepthOfField.nearFocusStart), (Func<bool>) (() => this.m_DepthOfField.IsActive() && (VolumeParameter<DepthOfFieldMode>) this.m_DepthOfField.focusMode == DepthOfFieldMode.Manual)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<MinFloatParameter>>) (() => this.m_DepthOfField.nearFocusEnd), (Func<bool>) (() => this.m_DepthOfField.IsActive() && (VolumeParameter<DepthOfFieldMode>) this.m_DepthOfField.focusMode == DepthOfFieldMode.Manual)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<MinFloatParameter>>) (() => this.m_DepthOfField.farFocusStart), (Func<bool>) (() => this.m_DepthOfField.IsActive() && (VolumeParameter<DepthOfFieldMode>) this.m_DepthOfField.focusMode == DepthOfFieldMode.Manual)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<MinFloatParameter>>) (() => this.m_DepthOfField.farFocusEnd), (Func<bool>) (() => this.m_DepthOfField.IsActive() && (VolumeParameter<DepthOfFieldMode>) this.m_DepthOfField.focusMode == DepthOfFieldMode.Manual)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<ClampedFloatParameter>>) (() => this.m_DepthOfField.m_NearMaxBlur), (Func<bool>) (() => this.m_DepthOfField.IsActive() && (VolumeParameter<DepthOfFieldMode>) this.m_DepthOfField.focusMode != DepthOfFieldMode.Off)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<ClampedFloatParameter>>) (() => this.m_DepthOfField.m_FarMaxBlur), (Func<bool>) (() => this.m_DepthOfField.IsActive() && (VolumeParameter<DepthOfFieldMode>) this.m_DepthOfField.focusMode != DepthOfFieldMode.Off)));
      this.AddProperty(PhotoModeUtils.GroupTitle("Lens", this.m_MotionBlur.GetType().Name));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<MinFloatParameter>>) (() => this.m_MotionBlur.intensity)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<ClampedFloatParameter>>) (() => this.m_MotionBlur.minimumVelocity)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<ClampedFloatParameter>>) (() => this.m_MotionBlur.maximumVelocity)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<ClampedFloatParameter>>) (() => this.m_MotionBlur.depthComparisonExtent)));
      this.AddProperty(PhotoModeUtils.GroupTitle("Lens", this.m_Bloom.GetType().Name));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<MinFloatParameter>>) (() => this.m_Bloom.threshold)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<ClampedFloatParameter>>) (() => this.m_Bloom.intensity)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<ClampedFloatParameter>>) (() => this.m_Bloom.scatter)));
      this.AddProperty(PhotoModeUtils.GroupTitle("Lens", this.m_Vignette.GetType().Name));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<ClampedFloatParameter>>) (() => this.m_Vignette.intensity)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<ColorParameter>>) (() => this.m_Vignette.color)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<Vector2Parameter>>) (() => this.m_Vignette.center), 0.0f, 1f));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<ClampedFloatParameter>>) (() => this.m_Vignette.smoothness)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<ClampedFloatParameter>>) (() => this.m_Vignette.roundness)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<BoolParameter>>) (() => this.m_Vignette.rounded)));
      this.AddProperty(PhotoModeUtils.GroupTitle("Lens", this.m_FilmGrain.GetType().Name));
      this.AddProperty(PhotoModeUtils.BindProperty<FilmGrainLookup>("Lens", (Expression<Func<VolumeParameter<FilmGrainLookup>>>) (() => this.m_FilmGrain.type)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<ClampedFloatParameter>>) (() => this.m_FilmGrain.intensity)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<ClampedFloatParameter>>) (() => this.m_FilmGrain.response)));
      this.AddProperty(PhotoModeUtils.GroupTitle("Lens", this.m_PaniniProjection.GetType().Name));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<ClampedFloatParameter>>) (() => this.m_PaniniProjection.distance)));
      this.AddProperty(PhotoModeUtils.BindProperty("Lens", (Expression<Func<ClampedFloatParameter>>) (() => this.m_PaniniProjection.cropToFit)));
      this.AddProperty(PhotoModeUtils.GroupTitle("Color", this.m_ColorAdjustments.GetType().Name));
      this.AddProperty(PhotoModeUtils.BindProperty("Color", (Expression<Func<FloatParameter>>) (() => this.m_ColorAdjustments.postExposure)));
      this.AddProperty(PhotoModeUtils.BindProperty("Color", (Expression<Func<ClampedFloatParameter>>) (() => this.m_ColorAdjustments.contrast)));
      this.AddProperty(PhotoModeUtils.BindProperty("Color", (Expression<Func<ColorParameter>>) (() => this.m_ColorAdjustments.colorFilter)));
      this.AddProperty(PhotoModeUtils.BindProperty("Color", (Expression<Func<ClampedFloatParameter>>) (() => this.m_ColorAdjustments.hueShift)));
      this.AddProperty(PhotoModeUtils.BindProperty("Color", (Expression<Func<ClampedFloatParameter>>) (() => this.m_ColorAdjustments.saturation)));
      this.AddProperty(PhotoModeUtils.GroupTitle("Color", this.m_WhiteBalance.GetType().Name));
      this.AddProperty(PhotoModeUtils.BindProperty("Color", (Expression<Func<ClampedFloatParameter>>) (() => this.m_WhiteBalance.temperature)));
      this.AddProperty(PhotoModeUtils.BindProperty("Color", (Expression<Func<ClampedFloatParameter>>) (() => this.m_WhiteBalance.tint)));
      this.AddProperty(PhotoModeUtils.GroupTitle("Color", this.m_ShadowsMidtonesHighlights.GetType().Name));
      this.AddProperty(PhotoModeUtils.BindPropertyW("Color", (Expression<Func<Vector4Parameter>>) (() => this.m_ShadowsMidtonesHighlights.shadows), -1f, 1f));
      this.AddProperty(PhotoModeUtils.BindPropertyW("Color", (Expression<Func<Vector4Parameter>>) (() => this.m_ShadowsMidtonesHighlights.midtones), -1f, 1f));
      this.AddProperty(PhotoModeUtils.BindPropertyW("Color", (Expression<Func<Vector4Parameter>>) (() => this.m_ShadowsMidtonesHighlights.highlights), -1f, 1f));
      this.AddProperty(PhotoModeUtils.GroupTitle("Weather", this.m_DistanceClouds.GetType().Name));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_DistanceClouds.opacity)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<MinFloatParameter>>) (() => this.m_DistanceClouds.layerA.altitude)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ColorParameter>>) (() => this.m_DistanceClouds.layerA.tint)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<FloatParameter>>) (() => this.m_DistanceClouds.layerA.exposure)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_DistanceClouds.layerA.rotation)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_DistanceClouds.layerA.thickness)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_DistanceClouds.layerA.opacityR)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_DistanceClouds.layerA.opacityG)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_DistanceClouds.layerA.opacityB)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_DistanceClouds.layerA.opacityA)));
      this.AddProperty(PhotoModeUtils.GroupTitle("Weather", this.m_VolumetricClouds.GetType().Name));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_VolumetricClouds.densityMultiplier)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_VolumetricClouds.shapeFactor)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<MinFloatParameter>>) (() => this.m_VolumetricClouds.shapeScale)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<Vector3Parameter>>) (() => this.m_VolumetricClouds.shapeOffset)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_VolumetricClouds.erosionFactor)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<MinFloatParameter>>) (() => this.m_VolumetricClouds.erosionScale)));
      this.AddProperty(PhotoModeUtils.BindProperty<VolumetricClouds.CloudErosionNoise>("Weather", (Expression<Func<VolumeParameter<VolumetricClouds.CloudErosionNoise>>>) (() => this.m_VolumetricClouds.erosionNoiseType)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<MinFloatParameter>>) (() => this.m_VolumetricClouds.bottomAltitude)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<MinFloatParameter>>) (() => this.m_VolumetricClouds.altitudeRange)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedIntParameter>>) (() => this.m_VolumetricClouds.numPrimarySteps)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedIntParameter>>) (() => this.m_VolumetricClouds.numLightSteps)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_VolumetricClouds.sunLightDimmer)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_VolumetricClouds.erosionOcclusion)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ColorParameter>>) (() => this.m_VolumetricClouds.scatteringTint)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_VolumetricClouds.powderEffectIntensity)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_VolumetricClouds.multiScattering)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_VolumetricClouds.shadowOpacity)));
      this.AddProperty(PhotoModeUtils.GroupTitle("Weather", this.m_Fog.GetType().Name));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<MinFloatParameter>>) (() => this.m_Fog.meanFreePath)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<FloatParameter>>) (() => this.m_Fog.baseHeight)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<FloatParameter>>) (() => this.m_Fog.maximumHeight)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<MinFloatParameter>>) (() => this.m_Fog.maxFogDistance)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ColorParameter>>) (() => this.m_Fog.tint)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ColorParameter>>) (() => this.m_Fog.albedo)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<MinFloatParameter>>) (() => this.m_Fog.depthExtent)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_Fog.anisotropy)));
      this.AddProperty(PhotoModeUtils.GroupTitle("Weather", this.m_Sky.GetType().Name));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<MinFloatParameter>>) (() => this.m_Sky.auroraBorealisEmissionMultiplier)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<MinFloatParameter>>) (() => this.m_Sky.airMaximumAltitude)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_Sky.airDensityR)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_Sky.airDensityG)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_Sky.airDensityB)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ColorParameter>>) (() => this.m_Sky.airTint)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<MinFloatParameter>>) (() => this.m_Sky.aerosolMaximumAltitude)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_Sky.aerosolDensity)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ColorParameter>>) (() => this.m_Sky.aerosolTint)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_Sky.aerosolAnisotropy)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_Sky.colorSaturation)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_Sky.alphaSaturation)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_Sky.alphaMultiplier)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ColorParameter>>) (() => this.m_Sky.horizonTint)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ClampedFloatParameter>>) (() => this.m_Sky.horizonZenithShift)));
      this.AddProperty(PhotoModeUtils.BindProperty("Weather", (Expression<Func<ColorParameter>>) (() => this.m_Sky.zenithTint)));
      this.AddProperty(new PhotoModeProperty()
      {
        id = "Time of Day",
        group = "Environment",
        setValue = (Action<float>) (value =>
        {
          this.m_PlanetarySystem.time = value;
          this.m_PlanetarySystem.overrideTime = true;
          this.m_PlanetarySystem.Update();
        }),
        getValue = (Func<float>) (() => this.m_PlanetarySystem.time),
        min = (Func<float>) (() => 0.0f),
        max = (Func<float>) (() => 24f),
        setEnabled = (Action<bool>) (enabled =>
        {
          this.m_PlanetarySystem.overrideTime = enabled;
          this.m_PlanetarySystem.Update();
        }),
        isEnabled = (Func<bool>) (() => this.m_PlanetarySystem.overrideTime)
      });
      this.AddProperty(new PhotoModeProperty()
      {
        id = "Simulation Speed",
        group = "Environment",
        setValue = (Action<float>) (value => this.m_SimulationSystem.selectedSpeed = value),
        getValue = (Func<float>) (() => this.m_SimulationSystem.selectedSpeed),
        min = (Func<float>) (() => 0.0f),
        max = (Func<float>) (() => 8f),
        reset = (Action) (() => this.m_SimulationSystem.selectedSpeed = 0.0f)
      });
      this.AddPreset(PhotoModeUtils.CreatePreset("SensorTypePreset", targetProperties[0], targetProperties, PhotoModeRenderSystem.kApertureFormatNames, PhotoModeRenderSystem.kApertureFormatValues));
    }

    private void AddPreset(PhotoModeUIPreset preset) => this.m_Presets.Add(preset);

    public void DisableAllCameraProperties()
    {
      foreach (KeyValuePair<string, PhotoModeProperty> photoModeProperty in this.photoModeProperties)
      {
        Action<bool> setEnabled = photoModeProperty.Value.setEnabled;
        if (setEnabled != null)
          setEnabled(false);
      }
    }

    [Preserve]
    public PhotoModeRenderSystem()
    {
    }
  }
}
