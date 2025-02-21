// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.MapPanelSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.Localization;
using Colossal.PSI.Common;
using Colossal.PSI.PdxSdk;
using Colossal.UI;
using Game.Areas;
using Game.Assets;
using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Prefabs.Climate;
using Game.Reflection;
using Game.SceneFlow;
using Game.Serialization;
using Game.Simulation;
using Game.UI.Localization;
using Game.UI.Menu;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  [CompilerGenerated]
  public class MapPanelSystem : EditorPanelSystemBase
  {
    private TerrainSystem m_TerrainSystem;
    private SaveGameSystem m_SaveGameSystem;
    private MapMetadataSystem m_MapMetadataSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private IMapTilePurchaseSystem m_MapTilePurchaseSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private PrefabSystem m_PrefabSystem;
    private EditorAssetUploadPanel m_AssetUploadPanel;
    private EntityQuery m_TimeQuery;
    private EntityQuery m_ThemeQuery;
    public bool m_MapNameAsCityName;
    public int m_StartingYear;
    public int m_StartingMonth;
    public float m_StartingTime;
    public bool m_CurrentYearAsStartingYear;
    private IconButtonGroup m_ThemeButtonGroup;
    private LocalizationField m_MapNameLocalization;
    private LocalizationField m_MapDescriptionLocalization;
    private MapPanelSystem.PreviewInfo m_Preview;
    private MapPanelSystem.PreviewInfo m_Thumbnail;
    private Button m_MapTileSelectionButton;
    private static readonly string kSelectStartingTilesPrompt = "Editor.SELECT_STARTING_TILES";
    private static readonly string kStopSelectingStartingTilesPrompt = "Editor.STOP_SELECTING_STARTING_TILES";
    private MapRequirementSystem m_MapRequirementSystem;
    private PdxSdkPlatform m_Platform;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SaveGameSystem = this.World.GetOrCreateSystemManaged<SaveGameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MapMetadataSystem = this.World.GetOrCreateSystemManaged<MapMetadataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MapTilePurchaseSystem = (IMapTilePurchaseSystem) this.World.GetOrCreateSystemManaged<MapTilePurchaseSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_MapRequirementSystem = this.World.GetOrCreateSystemManaged<MapRequirementSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AssetUploadPanel = this.World.GetOrCreateSystemManaged<EditorAssetUploadPanel>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ThemeQuery = this.GetEntityQuery(ComponentType.ReadOnly<ThemeData>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_Preview = new MapPanelSystem.PreviewInfo(new IconButton()
      {
        action = (System.Action) (() => this.activeSubPanel = (IEditorPanel) new LoadAssetPanel((LocalizedString) "Editor.PREVIEW", EditorPrefabUtils.GetUserImages(), (LoadAssetPanel.LoadCallback) (guid =>
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_Preview.Set(Colossal.IO.AssetDatabase.AssetDatabase.global.GetAsset<ImageAsset>(guid));
          this.CloseSubPanel();
        }), new System.Action(((EditorPanelSystemBase) this).CloseSubPanel)))
      });
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_Thumbnail = new MapPanelSystem.PreviewInfo(new IconButton()
      {
        action = (System.Action) (() => this.activeSubPanel = (IEditorPanel) new LoadAssetPanel((LocalizedString) "Editor.THUMBNAIL", EditorPrefabUtils.GetUserImages(), (LoadAssetPanel.LoadCallback) (guid =>
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_Thumbnail.Set(Colossal.IO.AssetDatabase.AssetDatabase.global.GetAsset<ImageAsset>(guid));
          this.CloseSubPanel();
        }), new System.Action(((EditorPanelSystemBase) this).CloseSubPanel)))
      });
      this.title = (LocalizedString) "Editor.MAP";
      IWidget[] widgetArray1 = new IWidget[2];
      IWidget[] children1 = new IWidget[2];
      EditorSection editorSection1 = new EditorSection();
      editorSection1.displayName = (LocalizedString) "Editor.MAP_SETTINGS";
      editorSection1.expanded = true;
      EditorSection editorSection2 = editorSection1;
      IWidget[] widgetArray2 = new IWidget[13];
      Game.UI.Widgets.Group group1 = new Game.UI.Widgets.Group();
      group1.displayName = (LocalizedString) "Editor.MAP_NAME";
      group1.tooltip = new LocalizedString?((LocalizedString) "Editor.MAP_NAME_TOOLTIP");
      // ISSUE: reference to a compiler-generated field
      group1.children = (IList<IWidget>) new IWidget[1]
      {
        (IWidget) (this.m_MapNameLocalization = new LocalizationField((LocalizedString) "Editor.MAP_NAME"))
      };
      widgetArray2[0] = (IWidget) group1;
      Game.UI.Widgets.Group group2 = new Game.UI.Widgets.Group();
      group2.displayName = (LocalizedString) "Editor.MAP_DESCRIPTION";
      group2.tooltip = new LocalizedString?((LocalizedString) "Editor.MAP_DESCRIPTION_TOOLTIP");
      // ISSUE: reference to a compiler-generated field
      group2.children = (IList<IWidget>) new IWidget[1]
      {
        (IWidget) (this.m_MapDescriptionLocalization = new LocalizationField((LocalizedString) "Editor.MAP_DESCRIPTION"))
      };
      widgetArray2[1] = (IWidget) group2;
      ToggleField toggleField1 = new ToggleField();
      toggleField1.displayName = (LocalizedString) "Editor.MAP_NAME_AS_DEFAULT";
      toggleField1.tooltip = new LocalizedString?((LocalizedString) "Editor.MAP_NAME_AS_DEFAULT_TOOLTIP");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      toggleField1.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => this.m_MapNameAsCityName), (Action<bool>) (value => this.m_MapNameAsCityName = value));
      widgetArray2[2] = (IWidget) toggleField1;
      widgetArray2[3] = (IWidget) new Divider();
      IntInputField intInputField1 = new IntInputField();
      intInputField1.displayName = (LocalizedString) "Editor.STARTING_YEAR";
      intInputField1.tooltip = new LocalizedString?((LocalizedString) "Editor.STARTING_YEAR_TOOLTIP");
      // ISSUE: reference to a compiler-generated field
      intInputField1.disabled = (Func<bool>) (() => this.m_CurrentYearAsStartingYear);
      intInputField1.min = 0;
      intInputField1.max = 3000;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      intInputField1.accessor = (ITypedValueAccessor<int>) new DelegateAccessor<int>((Func<int>) (() => !this.m_CurrentYearAsStartingYear ? this.m_StartingYear : DateTime.Now.Year), (Action<int>) (value =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_StartingYear = value;
        // ISSUE: reference to a compiler-generated method
        this.ApplyTime();
      }));
      widgetArray2[4] = (IWidget) intInputField1;
      ToggleField toggleField2 = new ToggleField();
      toggleField2.displayName = (LocalizedString) "Editor.CURRENT_YEAR_AS_DEFAULT";
      toggleField2.tooltip = new LocalizedString?((LocalizedString) "Editor.CURRENT_YEAR_AS_DEFAULT_TOOLTIP");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      toggleField2.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => this.m_CurrentYearAsStartingYear), (Action<bool>) (value => this.m_CurrentYearAsStartingYear = value));
      widgetArray2[5] = (IWidget) toggleField2;
      IntInputField intInputField2 = new IntInputField();
      intInputField2.displayName = (LocalizedString) "Editor.STARTING_MONTH";
      intInputField2.tooltip = new LocalizedString?((LocalizedString) "Editor.STARTING_MONTH_TOOLTIP");
      intInputField2.min = 1;
      intInputField2.max = 12;
      // ISSUE: reference to a compiler-generated field
      intInputField2.accessor = (ITypedValueAccessor<int>) new DelegateAccessor<int>((Func<int>) (() => this.m_StartingMonth), (Action<int>) (value =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_StartingMonth = value;
        // ISSUE: reference to a compiler-generated method
        this.ApplyTime();
      }));
      widgetArray2[6] = (IWidget) intInputField2;
      TimeSliderField timeSliderField = new TimeSliderField();
      timeSliderField.displayName = (LocalizedString) "Editor.STARTING_TIME";
      timeSliderField.tooltip = new LocalizedString?((LocalizedString) "Editor.STARTING_TIME_TOOLTIP");
      timeSliderField.min = 0.0f;
      timeSliderField.max = 0.999305546f;
      // ISSUE: reference to a compiler-generated field
      timeSliderField.accessor = (ITypedValueAccessor<float>) new DelegateAccessor<float>((Func<float>) (() => this.m_StartingTime), (Action<float>) (value =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_StartingTime = value;
        // ISSUE: reference to a compiler-generated method
        this.ApplyTime();
      }));
      widgetArray2[7] = (IWidget) timeSliderField;
      Game.UI.Widgets.Group group3 = new Game.UI.Widgets.Group();
      group3.displayName = (LocalizedString) "Editor.CAMERA_STARTING_POSITION";
      group3.tooltip = new LocalizedString?((LocalizedString) "Editor.CAMERA_STARTING_POSITION_TOOLTIP");
      Game.UI.Widgets.Group group4 = group3;
      IWidget[] widgetArray3 = new IWidget[4];
      Float3InputField float3InputField = new Float3InputField();
      float3InputField.displayName = (LocalizedString) "Editor.CAMERA_PIVOT";
      float3InputField.tooltip = new LocalizedString?((LocalizedString) "Editor.CAMERA_PIVOT_TOOLTIP");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float3InputField.accessor = (ITypedValueAccessor<float3>) new DelegateAccessor<float3>((Func<float3>) (() => this.m_CityConfigurationSystem.m_CameraPivot), (Action<float3>) (value => this.m_CityConfigurationSystem.m_CameraPivot = value));
      widgetArray3[0] = (IWidget) float3InputField;
      Float2InputField float2InputField = new Float2InputField();
      float2InputField.displayName = (LocalizedString) "Editor.CAMERA_ANGLE";
      float2InputField.tooltip = new LocalizedString?((LocalizedString) "Editor.CAMERA_ANGLE_TOOLTIP");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float2InputField.accessor = (ITypedValueAccessor<float2>) new DelegateAccessor<float2>((Func<float2>) (() => this.m_CityConfigurationSystem.m_CameraAngle), (Action<float2>) (value => this.m_CityConfigurationSystem.m_CameraAngle = value));
      widgetArray3[1] = (IWidget) float2InputField;
      FloatInputField floatInputField = new FloatInputField();
      floatInputField.displayName = (LocalizedString) "Editor.CAMERA_ZOOM";
      floatInputField.tooltip = new LocalizedString?((LocalizedString) "Editor.CAMERA_ZOOM_TOOLTIP");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      floatInputField.accessor = (ITypedValueAccessor<double>) new DelegateAccessor<double>((Func<double>) (() => (double) this.m_CityConfigurationSystem.m_CameraZoom), (Action<double>) (value => this.m_CityConfigurationSystem.m_CameraZoom = (float) value));
      widgetArray3[2] = (IWidget) floatInputField;
      Button button1 = new Button();
      button1.displayName = (LocalizedString) "Editor.CAPTURE_CAMERA_POSITION";
      button1.tooltip = new LocalizedString?((LocalizedString) "Editor.CAPTURE_CAMERA_POSITION_TOOLTIP");
      button1.action = (System.Action) (() =>
      {
        CameraController cameraController;
        if (!CameraController.TryGet(out cameraController))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CityConfigurationSystem.m_CameraPivot = (float3) cameraController.pivot;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CityConfigurationSystem.m_CameraAngle = cameraController.angle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CityConfigurationSystem.m_CameraZoom = cameraController.zoom;
      });
      widgetArray3[3] = (IWidget) button1;
      group4.children = (IList<IWidget>) widgetArray3;
      widgetArray2[8] = (IWidget) group3;
      Button button2 = new Button();
      // ISSUE: reference to a compiler-generated field
      button2.displayName = (LocalizedString) MapPanelSystem.kSelectStartingTilesPrompt;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      button2.action = (System.Action) (() => this.m_MapTilePurchaseSystem.selecting = !this.m_MapTilePurchaseSystem.selecting);
      Button button3 = button2;
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileSelectionButton = button2;
      widgetArray2[9] = (IWidget) button3;
      EditorSection editorSection3 = new EditorSection();
      editorSection3.displayName = (LocalizedString) "Editor.THEME";
      editorSection3.tooltip = new LocalizedString?((LocalizedString) "Editor.THEME_TOOLTIP");
      editorSection3.expanded = true;
      // ISSUE: reference to a compiler-generated field
      editorSection3.children = (IList<IWidget>) new IWidget[1]
      {
        (IWidget) (this.m_ThemeButtonGroup = new IconButtonGroup())
      };
      widgetArray2[10] = (IWidget) editorSection3;
      Game.UI.Widgets.Group group5 = new Game.UI.Widgets.Group();
      group5.displayName = (LocalizedString) "Editor.PREVIEW";
      group5.tooltip = new LocalizedString?((LocalizedString) "Editor.PREVIEW_TOOLTIP");
      // ISSUE: reference to a compiler-generated field
      group5.children = (IList<IWidget>) new IWidget[1]
      {
        (IWidget) this.m_Preview.button
      };
      widgetArray2[11] = (IWidget) group5;
      Game.UI.Widgets.Group group6 = new Game.UI.Widgets.Group();
      group6.displayName = (LocalizedString) "Editor.THUMBNAIL";
      group6.tooltip = new LocalizedString?((LocalizedString) "Editor.THUMBNAIL_TOOLTIP");
      // ISSUE: reference to a compiler-generated field
      group6.children = (IList<IWidget>) new IWidget[1]
      {
        (IWidget) this.m_Thumbnail.button
      };
      widgetArray2[12] = (IWidget) group6;
      editorSection2.children = (IList<IWidget>) widgetArray2;
      children1[0] = (IWidget) editorSection1;
      EditorSection editorSection4 = new EditorSection();
      editorSection4.displayName = (LocalizedString) "Editor.CHECKLIST";
      editorSection4.tooltip = new LocalizedString?((LocalizedString) "Editor.CHECKLIST_TOOLTIP");
      EditorSection editorSection5 = editorSection4;
      IWidget[] widgetArray4 = new IWidget[2];
      Game.UI.Widgets.Group group7 = new Game.UI.Widgets.Group();
      group7.displayName = (LocalizedString) "Editor.CHECKLIST_REQUIRED";
      group7.tooltip = new LocalizedString?((LocalizedString) "Editor.CHECKLIST_REQUIRED_TOOLTIP");
      Game.UI.Widgets.Group group8 = group7;
      IWidget[] widgetArray5 = new IWidget[4];
      ToggleField toggleField3 = new ToggleField();
      toggleField3.displayName = (LocalizedString) "Editor.CHECKLIST_STARTING_TILES";
      toggleField3.tooltip = new LocalizedString?((LocalizedString) "Editor.CHECKLIST_STARTING_TILES_TOOLTIP");
      toggleField3.disabled = (Func<bool>) (() => true);
      // ISSUE: reference to a compiler-generated field
      toggleField3.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => this.m_MapRequirementSystem.hasStartingArea));
      widgetArray5[0] = (IWidget) toggleField3;
      ToggleField toggleField4 = new ToggleField();
      toggleField4.displayName = (LocalizedString) "Editor.CHECKLIST_WATER";
      toggleField4.tooltip = new LocalizedString?((LocalizedString) "Editor.CHECKLIST_WATER_TOOLTIP");
      toggleField4.disabled = (Func<bool>) (() => true);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      toggleField4.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => this.m_MapRequirementSystem.StartingAreaHasResource(MapFeature.SurfaceWater) || this.m_MapRequirementSystem.StartingAreaHasResource(MapFeature.GroundWater)));
      widgetArray5[1] = (IWidget) toggleField4;
      ToggleField toggleField5 = new ToggleField();
      toggleField5.displayName = (LocalizedString) "Editor.CHECKLIST_ROAD_CONNECTION";
      toggleField5.tooltip = new LocalizedString?((LocalizedString) "Editor.CHECKLIST_ROAD_CONNECTION_TOOLTIP");
      toggleField5.disabled = (Func<bool>) (() => true);
      // ISSUE: reference to a compiler-generated field
      toggleField5.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => this.m_MapRequirementSystem.roadConnection));
      widgetArray5[2] = (IWidget) toggleField5;
      ToggleField toggleField6 = new ToggleField();
      toggleField6.displayName = (LocalizedString) "Editor.CHECKLIST_NAME";
      toggleField6.tooltip = new LocalizedString?((LocalizedString) "Editor.CHECKLIST_NAME_TOOLTIP");
      toggleField6.disabled = (Func<bool>) (() => true);
      // ISSUE: reference to a compiler-generated field
      toggleField6.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => this.m_MapNameLocalization.IsValid()));
      widgetArray5[3] = (IWidget) toggleField6;
      group8.children = (IList<IWidget>) widgetArray5;
      widgetArray4[0] = (IWidget) group7;
      Game.UI.Widgets.Group group9 = new Game.UI.Widgets.Group();
      group9.displayName = (LocalizedString) "Editor.CHECKLIST_OPTIONAL";
      group9.tooltip = new LocalizedString?((LocalizedString) "Editor.CHECKLIST_OPTIONAL_TOOLTIP");
      Game.UI.Widgets.Group group10 = group9;
      IWidget[] widgetArray6 = new IWidget[7];
      ToggleField toggleField7 = new ToggleField();
      toggleField7.displayName = (LocalizedString) "Editor.CHECKLIST_TRAIN_CONNECTION";
      toggleField7.tooltip = new LocalizedString?((LocalizedString) "Editor.CHECKLIST_TRAIN_CONNECTION_TOOLTIP");
      toggleField7.disabled = (Func<bool>) (() => true);
      // ISSUE: reference to a compiler-generated field
      toggleField7.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => this.m_MapRequirementSystem.trainConnection));
      widgetArray6[0] = (IWidget) toggleField7;
      ToggleField toggleField8 = new ToggleField();
      toggleField8.displayName = (LocalizedString) "Editor.CHECKLIST_AIR_CONNECTION";
      toggleField8.tooltip = new LocalizedString?((LocalizedString) "Editor.CHECKLIST_AIR_CONNECTION_TOOLTIP");
      toggleField8.disabled = (Func<bool>) (() => true);
      // ISSUE: reference to a compiler-generated field
      toggleField8.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => this.m_MapRequirementSystem.airConnection));
      widgetArray6[1] = (IWidget) toggleField8;
      ToggleField toggleField9 = new ToggleField();
      toggleField9.displayName = (LocalizedString) "Editor.CHECKLIST_ELECTRICITY_CONNECTION";
      toggleField9.tooltip = new LocalizedString?((LocalizedString) "Editor.CHECKLIST_ELECTRICITY_CONNECTION_TOOLTIP");
      toggleField9.disabled = (Func<bool>) (() => true);
      // ISSUE: reference to a compiler-generated field
      toggleField9.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => this.m_MapRequirementSystem.electricityConnection));
      widgetArray6[2] = (IWidget) toggleField9;
      ToggleField toggleField10 = new ToggleField();
      toggleField10.displayName = (LocalizedString) "Editor.CHECKLIST_OIL";
      toggleField10.tooltip = new LocalizedString?((LocalizedString) "Editor.CHECKLIST_OIL_TOOLTIP");
      toggleField10.disabled = (Func<bool>) (() => true);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      toggleField10.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => this.m_MapRequirementSystem.MapHasResource(MapFeature.Oil)));
      widgetArray6[3] = (IWidget) toggleField10;
      ToggleField toggleField11 = new ToggleField();
      toggleField11.displayName = (LocalizedString) "Editor.CHECKLIST_ORE";
      toggleField11.tooltip = new LocalizedString?((LocalizedString) "Editor.CHECKLIST_ORE_TOOLTIP");
      toggleField11.disabled = (Func<bool>) (() => true);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      toggleField11.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => this.m_MapRequirementSystem.MapHasResource(MapFeature.Ore)));
      widgetArray6[4] = (IWidget) toggleField11;
      ToggleField toggleField12 = new ToggleField();
      toggleField12.displayName = (LocalizedString) "Editor.CHECKLIST_FOREST";
      toggleField12.tooltip = new LocalizedString?((LocalizedString) "Editor.CHECKLIST_FOREST_TOOLTIP");
      toggleField12.disabled = (Func<bool>) (() => true);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      toggleField12.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => this.m_MapRequirementSystem.MapHasResource(MapFeature.Forest)));
      widgetArray6[5] = (IWidget) toggleField12;
      ToggleField toggleField13 = new ToggleField();
      toggleField13.displayName = (LocalizedString) "Editor.CHECKLIST_FERTILE";
      toggleField13.tooltip = new LocalizedString?((LocalizedString) "Editor.CHECKLIST_FERTILE_TOOLTIP");
      toggleField13.disabled = (Func<bool>) (() => true);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      toggleField13.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => this.m_MapRequirementSystem.MapHasResource(MapFeature.FertileLand)));
      widgetArray6[6] = (IWidget) toggleField13;
      group10.children = (IList<IWidget>) widgetArray6;
      widgetArray4[1] = (IWidget) group9;
      editorSection5.children = (IList<IWidget>) widgetArray4;
      children1[1] = (IWidget) editorSection4;
      widgetArray1[0] = (IWidget) Scrollable.WithChildren((IList<IWidget>) children1);
      Button[] children2 = new Button[3];
      Button button4 = new Button();
      button4.displayName = (LocalizedString) "Editor.LOAD_MAP";
      button4.tooltip = new LocalizedString?((LocalizedString) "Editor.LOAD_MAP_TOOLTIP");
      // ISSUE: reference to a compiler-generated method
      button4.action = (System.Action) (() => this.activeSubPanel = (IEditorPanel) new LoadAssetPanel((LocalizedString) "Editor.LOAD_MAP", this.GetMaps(), (LoadAssetPanel.LoadCallback) (guid => GameManager.instance.userInterface.appBindings.ShowConfirmationDialog(new Game.UI.ConfirmationDialog(new LocalizedString?(), (LocalizedString) "Common.DIALOG_MESSAGE[ProgressLoss]", (LocalizedString) "Common.DIALOG_ACTION[Yes]", new LocalizedString?((LocalizedString) "Common.DIALOG_ACTION[No]"), System.Array.Empty<LocalizedString>()), (Action<int>) (ret =>
      {
        if (ret != 0)
          return;
        // ISSUE: reference to a compiler-generated method
        this.LoadMap(guid);
      }))), new System.Action(((EditorPanelSystemBase) this).CloseSubPanel)));
      children2[0] = button4;
      Button button5 = new Button();
      button5.displayName = (LocalizedString) "Editor.SAVE_MAP";
      button5.tooltip = new LocalizedString?((LocalizedString) "Editor.SAVE_MAP_TOOLTIP");
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      button5.action = (System.Action) (() => this.activeSubPanel = (IEditorPanel) new SaveAssetPanel((LocalizedString) "Editor.SAVE_MAP", this.GetMaps(), new Colossal.Hash128?(this.m_TerrainSystem.mapReference), (SaveAssetPanel.SaveCallback) ((name, overwriteGuid) => this.OnSaveMap(name, overwriteGuid)), new System.Action(((EditorPanelSystemBase) this).CloseSubPanel)));
      children2[1] = button5;
      Button button6 = new Button();
      button6.displayName = (LocalizedString) "GameListScreen.GAME_OPTION[shareMap]";
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      button6.action = (System.Action) (() => this.activeSubPanel = (IEditorPanel) new SaveAssetPanel((LocalizedString) "Editor.SAVE_MAP_SHARE", this.GetMaps(), new Colossal.Hash128?(this.m_TerrainSystem.mapReference), (SaveAssetPanel.SaveCallback) ((name, overwriteGuid) => this.OnSaveMap(name, overwriteGuid, (Action<MapMetadata>) (map =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AssetUploadPanel.Show((AssetData) map);
        // ISSUE: reference to a compiler-generated field
        this.activeSubPanel = (IEditorPanel) this.m_AssetUploadPanel;
      }))), new System.Action(((EditorPanelSystemBase) this).CloseSubPanel), (LocalizedString) "Editor.SAVE_SHARE"));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      button6.hidden = (Func<bool>) (() => this.m_Platform == null || !this.m_Platform.cachedLoggedIn);
      children2[2] = button6;
      widgetArray1[1] = (IWidget) ButtonRow.WithChildren(children2);
      this.children = (IList<IWidget>) widgetArray1;
      // ISSUE: reference to a compiler-generated field
      this.m_Platform = PlatformManager.instance.GetPSI<PdxSdkPlatform>("PdxSdk");
      PlatformManager.instance.onPlatformRegistered += (PlatformRegisteredHandler) (psi =>
      {
        if (!(psi is PdxSdkPlatform pdxSdkPlatform2))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_Platform = pdxSdkPlatform2;
      });
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      if (serializationContext.purpose != Colossal.Serialization.Entities.Purpose.NewMap && serializationContext.purpose != Colossal.Serialization.Entities.Purpose.LoadMap)
        return;
      // ISSUE: reference to a compiler-generated field
      if (this.m_TimeQuery.IsEmptyIgnoreFilter)
        this.EntityManager.AddComponentData<TimeData>(this.EntityManager.CreateEntity(), new TimeData()
        {
          m_StartingYear = DateTime.Now.Year,
          m_StartingMonth = (byte) 6,
          m_StartingHour = (byte) 6,
          m_StartingMinutes = (byte) 0
        });
      // ISSUE: reference to a compiler-generated method
      this.FetchThemes();
      // ISSUE: reference to a compiler-generated method
      this.FetchTime();
      // ISSUE: reference to a compiler-generated field
      MapMetadata asset = Colossal.IO.AssetDatabase.AssetDatabase.global.GetAsset<MapMetadata>(this.m_TerrainSystem.mapReference);
      // ISSUE: reference to a compiler-generated field
      this.m_MapMetadataSystem.mapName = !string.IsNullOrEmpty(asset?.target?.displayName) ? asset.target.displayName : Guid.NewGuid().ToString();
      // ISSUE: reference to a compiler-generated method
      this.InitLocalization(asset);
      // ISSUE: reference to a compiler-generated method
      this.InitPreview(asset);
    }

    [Preserve]
    protected override void OnStartRunning()
    {
      base.OnStartRunning();
      this.activeSubPanel = (IEditorPanel) null;
      // ISSUE: reference to a compiler-generated field
      this.m_MapRequirementSystem.Enabled = true;
    }

    [Preserve]
    protected override void OnStopRunning()
    {
      base.OnStopRunning();
      // ISSUE: reference to a compiler-generated field
      this.m_MapTilePurchaseSystem.selecting = false;
      // ISSUE: reference to a compiler-generated field
      this.m_MapRequirementSystem.Enabled = false;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      // ISSUE: reference to a compiler-generated field
      this.m_MapRequirementSystem.Update();
      // ISSUE: reference to a compiler-generated field
      if (this.m_MapTilePurchaseSystem.selecting)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateMapTileButton(MapPanelSystem.kStopSelectingStartingTilesPrompt);
        // ISSUE: reference to a compiler-generated method
        this.UpdateStartingTiles();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateMapTileButton(MapPanelSystem.kSelectStartingTilesPrompt);
      }
    }

    private void FetchThemes()
    {
      List<IconButton> iconButtonList = new List<IconButton>();
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_ThemeQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      foreach (Entity entity in entityArray)
      {
        Entity theme = entity;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ThemePrefab prefab = this.m_PrefabSystem.GetPrefab<ThemePrefab>(theme);
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iconButtonList.Add(new IconButton()
        {
          icon = ImageSystem.GetIcon((PrefabBase) prefab) ?? "Media/Editor/Object.svg",
          tooltip = new LocalizedString?(LocalizedString.Id("Assets.THEME[" + prefab.name + "]")),
          action = (System.Action) (() => this.m_CityConfigurationSystem.defaultTheme = theme),
          selected = (Func<bool>) (() => this.m_CityConfigurationSystem.defaultTheme == theme)
        });
      }
      entityArray.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ThemeButtonGroup.children = iconButtonList.ToArray();
    }

    private void FetchTime()
    {
      // ISSUE: reference to a compiler-generated field
      TimeData singleton = this.m_TimeQuery.GetSingleton<TimeData>();
      // ISSUE: reference to a compiler-generated field
      this.m_StartingYear = singleton.m_StartingYear;
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentYearAsStartingYear = true;
      // ISSUE: reference to a compiler-generated field
      this.m_StartingMonth = (int) singleton.m_StartingMonth + 1;
      // ISSUE: reference to a compiler-generated field
      this.m_StartingTime = singleton.TimeOffset;
    }

    private void SetStartingYear(int value)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_StartingYear = value;
      // ISSUE: reference to a compiler-generated method
      this.ApplyTime();
    }

    private void SetStartingMonth(int value)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_StartingMonth = value;
      // ISSUE: reference to a compiler-generated method
      this.ApplyTime();
    }

    private void SetStartingTime(float value)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_StartingTime = value;
      // ISSUE: reference to a compiler-generated method
      this.ApplyTime();
    }

    private void ApplyTime()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      TimeData component = new TimeData()
      {
        m_StartingYear = this.m_StartingYear,
        m_StartingMonth = (byte) (this.m_StartingMonth - 1),
        TimeOffset = this.m_StartingTime
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.CreateCommandBuffer().SetComponent<TimeData>(this.m_TimeQuery.GetSingletonEntity(), component);
    }

    private void ShowLoadMapPanel()
    {
      // ISSUE: reference to a compiler-generated method
      this.activeSubPanel = (IEditorPanel) new LoadAssetPanel((LocalizedString) "Editor.LOAD_MAP", this.GetMaps(), (LoadAssetPanel.LoadCallback) (guid => GameManager.instance.userInterface.appBindings.ShowConfirmationDialog(new Game.UI.ConfirmationDialog(new LocalizedString?(), (LocalizedString) "Common.DIALOG_MESSAGE[ProgressLoss]", (LocalizedString) "Common.DIALOG_ACTION[Yes]", new LocalizedString?((LocalizedString) "Common.DIALOG_ACTION[No]"), System.Array.Empty<LocalizedString>()), (Action<int>) (ret =>
      {
        if (ret != 0)
          return;
        // ISSUE: reference to a compiler-generated method
        this.LoadMap(guid);
      }))), new System.Action(((EditorPanelSystemBase) this).CloseSubPanel));
    }

    private void ShowSaveMapPanel()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.activeSubPanel = (IEditorPanel) new SaveAssetPanel((LocalizedString) "Editor.SAVE_MAP", this.GetMaps(), new Colossal.Hash128?(this.m_TerrainSystem.mapReference), (SaveAssetPanel.SaveCallback) ((name, overwriteGuid) => this.OnSaveMap(name, overwriteGuid)), new System.Action(((EditorPanelSystemBase) this).CloseSubPanel));
    }

    private void ShowShareMapPanel()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.activeSubPanel = (IEditorPanel) new SaveAssetPanel((LocalizedString) "Editor.SAVE_MAP_SHARE", this.GetMaps(), new Colossal.Hash128?(this.m_TerrainSystem.mapReference), (SaveAssetPanel.SaveCallback) ((name, overwriteGuid) => this.OnSaveMap(name, overwriteGuid, (Action<MapMetadata>) (map =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AssetUploadPanel.Show((AssetData) map);
        // ISSUE: reference to a compiler-generated field
        this.activeSubPanel = (IEditorPanel) this.m_AssetUploadPanel;
      }))), new System.Action(((EditorPanelSystemBase) this).CloseSubPanel), (LocalizedString) "Editor.SAVE_SHARE");
    }

    private IEnumerable<AssetItem> GetMaps()
    {
      foreach (MapMetadata asset in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<MapMetadata>(new SearchFilter<MapMetadata>()))
      {
        using (asset)
        {
          if (asset.database != Colossal.IO.AssetDatabase.AssetDatabase.game)
          {
            AssetItem map;
            // ISSUE: reference to a compiler-generated method
            if (this.TryGetAssetItem(asset, out map))
              yield return map;
          }
          else
            continue;
        }
      }
    }

    private bool TryGetAssetItem(MapMetadata asset, out AssetItem item)
    {
      try
      {
        MapInfo target = asset.target;
        SourceMeta meta = asset.GetMeta();
        ref AssetItem local = ref item;
        AssetItem assetItem = new AssetItem();
        assetItem.guid = asset.guid;
        assetItem.fileName = meta.fileName;
        assetItem.displayName = (LocalizedString) meta.fileName;
        assetItem.image = target.thumbnail.ToUri(MenuHelpers.defaultPreview);
        assetItem.badge = meta.remoteStorageSourceName;
        local = assetItem;
        return true;
      }
      catch (Exception ex)
      {
        this.log.Error(ex);
        item = (AssetItem) null;
      }
      return false;
    }

    private void ShowPreviewPicker()
    {
      this.activeSubPanel = (IEditorPanel) new LoadAssetPanel((LocalizedString) "Editor.PREVIEW", EditorPrefabUtils.GetUserImages(), (LoadAssetPanel.LoadCallback) (guid =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_Preview.Set(Colossal.IO.AssetDatabase.AssetDatabase.global.GetAsset<ImageAsset>(guid));
        this.CloseSubPanel();
      }), new System.Action(((EditorPanelSystemBase) this).CloseSubPanel));
    }

    private void ShowThumbnailPicker()
    {
      this.activeSubPanel = (IEditorPanel) new LoadAssetPanel((LocalizedString) "Editor.THUMBNAIL", EditorPrefabUtils.GetUserImages(), (LoadAssetPanel.LoadCallback) (guid =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_Thumbnail.Set(Colossal.IO.AssetDatabase.AssetDatabase.global.GetAsset<ImageAsset>(guid));
        this.CloseSubPanel();
      }), new System.Action(((EditorPanelSystemBase) this).CloseSubPanel));
    }

    private void OnSelectPreview(Colossal.Hash128 guid)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_Preview.Set(Colossal.IO.AssetDatabase.AssetDatabase.global.GetAsset<ImageAsset>(guid));
      this.CloseSubPanel();
    }

    private void OnSelectThumbnail(Colossal.Hash128 guid)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_Thumbnail.Set(Colossal.IO.AssetDatabase.AssetDatabase.global.GetAsset<ImageAsset>(guid));
      this.CloseSubPanel();
    }

    private void OnLoadMap(Colossal.Hash128 guid)
    {
      GameManager.instance.userInterface.appBindings.ShowConfirmationDialog(new Game.UI.ConfirmationDialog(new LocalizedString?(), (LocalizedString) "Common.DIALOG_MESSAGE[ProgressLoss]", (LocalizedString) "Common.DIALOG_ACTION[Yes]", new LocalizedString?((LocalizedString) "Common.DIALOG_ACTION[No]"), System.Array.Empty<LocalizedString>()), (Action<int>) (ret =>
      {
        if (ret != 0)
          return;
        // ISSUE: reference to a compiler-generated method
        this.LoadMap(guid);
      }));
    }

    private void LoadMap(Colossal.Hash128 guid)
    {
      this.CloseSubPanel();
      MapMetadata assetData;
      if (!Colossal.IO.AssetDatabase.AssetDatabase.global.TryGetAsset<MapMetadata>(guid, out assetData))
        return;
      GameManager.instance.Load(GameMode.Editor, Colossal.Serialization.Entities.Purpose.LoadMap, (IAssetData) assetData).ConfigureAwait(false);
    }

    private void InitPreview(MapMetadata asset = null)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_Preview.Set(asset?.target?.preview, MenuHelpers.defaultPreview);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_Thumbnail.Set(asset?.target?.thumbnail, MenuHelpers.defaultThumbnail);
    }

    private void InitLocalization(MapMetadata asset = null)
    {
      if ((AssetData) asset != (IAssetData) null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_MapNameLocalization.Initialize((IEnumerable<LocaleAsset>) asset.target.localeAssets, string.Format("Maps.MAP_TITLE[{0}]", (object) asset.target.displayName));
        // ISSUE: reference to a compiler-generated field
        this.m_MapDescriptionLocalization.Initialize((IEnumerable<LocaleAsset>) asset.target.localeAssets, string.Format("Maps.MAP_DESCRIPTION[{0}]", (object) asset.target.displayName));
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_MapNameLocalization.Initialize();
        // ISSUE: reference to a compiler-generated field
        this.m_MapDescriptionLocalization.Initialize();
      }
    }

    private void OnSaveMap(string fileName, Colossal.Hash128? overwriteGuid, Action<MapMetadata> callback = null)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_MapMetadataSystem.Update();
      if (overwriteGuid.HasValue)
      {
        GameManager.instance.userInterface.appBindings.ShowConfirmationDialog(new Game.UI.ConfirmationDialog(new LocalizedString?(), (LocalizedString) "Common.DIALOG_MESSAGE[OverwriteMap]", (LocalizedString) "Common.DIALOG_ACTION[Yes]", new LocalizedString?((LocalizedString) "Common.DIALOG_ACTION[No]"), System.Array.Empty<LocalizedString>()), (Action<int>) (ret =>
        {
          if (ret != 0)
            return;
          this.CloseSubPanel();
          MapMetadata asset = Colossal.IO.AssetDatabase.AssetDatabase.global.GetAsset<MapMetadata>(overwriteGuid.Value);
          SourceMeta meta = asset.GetMeta();
          // ISSUE: reference to a compiler-generated method
          this.SaveMap(asset.name, asset.target, asset.database, AssetDataPath.Create(meta.subPath, meta.fileName), asset.database != Colossal.IO.AssetDatabase.AssetDatabase.game, callback);
        }));
      }
      else
      {
        this.CloseSubPanel();
        // ISSUE: reference to a compiler-generated method
        this.SaveMap(fileName, (MapInfo) null, Colossal.IO.AssetDatabase.AssetDatabase.user, SaveHelpers.GetAssetDataPath<MapMetadata>(Colossal.IO.AssetDatabase.AssetDatabase.user, fileName), true, callback);
      }
    }

    private async void SaveMap(
      string fileName,
      MapInfo existing,
      ILocalAssetDatabase finalDb,
      AssetDataPath packagePath,
      bool embedLocalization,
      Action<MapMetadata> callback = null)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_MapMetadataSystem.mapName = fileName;
      using (ILocalAssetDatabase db = Colossal.IO.AssetDatabase.AssetDatabase.GetTransient())
      {
        // ISSUE: reference to a compiler-generated method
        MapInfo info = this.GetMapInfo(existing);
        MapMetadata meta = db.AddAsset<MapMetadata>((AssetDataPath) fileName);
        meta.target = info;
        MapData mapData = db.AddAsset<MapData>((AssetDataPath) fileName);
        info.mapData = mapData;
        // ISSUE: reference to a compiler-generated method
        info.climate = this.SaveClimate(db);
        // ISSUE: reference to a compiler-generated field
        this.m_SaveGameSystem.context = new Colossal.Serialization.Entities.Context(Colossal.Serialization.Entities.Purpose.SaveMap, Game.Version.current);
        // ISSUE: reference to a compiler-generated field
        this.m_SaveGameSystem.stream = mapData.GetWriteStream();
        // ISSUE: reference to a compiler-generated field
        this.m_TerrainSystem.mapReference = meta.guid;
        // ISSUE: reference to a compiler-generated field
        await this.m_SaveGameSystem.RunOnce();
        TextureAsset asset1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_Preview.CopyToTextureAsset(db, (AssetDataPath) this.m_Preview.name, out asset1))
        {
          asset1.Save(0, false);
          info.preview = asset1;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_Preview.Equals(this.m_Thumbnail))
        {
          info.thumbnail = asset1;
        }
        else
        {
          TextureAsset asset2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_Thumbnail.CopyToTextureAsset(db, (AssetDataPath) this.m_Thumbnail.name, out asset2))
          {
            asset2.Save(0, false);
            info.thumbnail = asset2;
          }
        }
        // ISSUE: reference to a compiler-generated method
        info.localeAssets = !embedLocalization ? (LocaleAsset[]) null : this.SaveLocalization(db, fileName);
        meta.Save(false);
        PackageAsset asset3;
        if (finalDb.Exists<PackageAsset>(packagePath, out asset3))
        {
          Colossal.Hash128 guid = asset3.guid;
          finalDb.DeleteAsset<PackageAsset>(asset3);
          finalDb.AddAsset<PackageAsset, ILocalAssetDatabase>(packagePath, db, guid).Save(false);
        }
        else
          finalDb.AddAsset(packagePath, db).Save(false);
        string message = await finalDb.ResaveCache();
        if (!string.IsNullOrWhiteSpace(message))
          Debug.Log((object) message);
        GameManager.instance.RunOnMainThread((System.Action) (() =>
        {
          PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.Cartography);
          // ISSUE: reference to a compiler-generated method
          this.InitPreview(meta);
          if (callback == null)
            return;
          callback(meta);
        }));
        info = (MapInfo) null;
      }
    }

    private PrefabAsset SaveClimate(ILocalAssetDatabase database)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ClimatePrefab prefab = this.m_PrefabSystem.GetPrefab<ClimatePrefab>(this.World.GetOrCreateSystemManaged<ClimateSystem>().currentClimate);
      if (prefab.builtin)
        return (PrefabAsset) null;
      PrefabBase data = prefab.Clone();
      data.name = prefab.name;
      data.asset = database.AddAsset<PrefabAsset, ScriptableObject>((AssetDataPath) data.name, (ScriptableObject) data);
      data.asset.Save(false);
      return data.asset;
    }

    private LocaleAsset[] SaveLocalization(ILocalAssetDatabase db, string fileName)
    {
      System.Collections.Generic.Dictionary<string, LocaleData> localeDatas = new System.Collections.Generic.Dictionary<string, LocaleData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MapNameLocalization.BuildLocaleData(string.Format("Maps.MAP_TITLE[{0}]", (object) this.m_MapMetadataSystem.mapName), localeDatas, this.m_MapMetadataSystem.mapName);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MapDescriptionLocalization.BuildLocaleData(string.Format("Maps.MAP_DESCRIPTION[{0}]", (object) this.m_MapMetadataSystem.mapName), localeDatas);
      List<LocaleAsset> localeAssetList = new List<LocaleAsset>(localeDatas.Keys.Count);
      foreach (string key in localeDatas.Keys)
      {
        AssetDataPath assetDataPath = SaveHelpers.GetAssetDataPath<MapMetadata>(db, fileName + "_" + key);
        LocaleAsset localeAsset = db.AddAsset<LocaleAsset>(assetDataPath);
        LocalizationManager localizationManager = GameManager.instance.localizationManager;
        localeAsset.SetData(localeDatas[key], localizationManager.LocaleIdToSystemLanguage(key), GameManager.instance.localizationManager.GetLocalizedName(key));
        localeAsset.Save(false);
        localeAssetList.Add(localeAsset);
      }
      return localeAssetList.ToArray();
    }

    private void ShareMap(MapMetadata map)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_AssetUploadPanel.Show((AssetData) map);
      // ISSUE: reference to a compiler-generated field
      this.activeSubPanel = (IEditorPanel) this.m_AssetUploadPanel;
    }

    private MapInfo GetMapInfo(MapInfo merge = null)
    {
      MapInfo mapInfo = merge ?? new MapInfo();
      // ISSUE: reference to a compiler-generated field
      mapInfo.displayName = this.m_MapMetadataSystem.mapName;
      // ISSUE: reference to a compiler-generated field
      mapInfo.theme = this.m_MapMetadataSystem.theme;
      // ISSUE: reference to a compiler-generated field
      mapInfo.temperatureRange = this.m_MapMetadataSystem.temperatureRange;
      // ISSUE: reference to a compiler-generated field
      mapInfo.cloudiness = this.m_MapMetadataSystem.cloudiness;
      // ISSUE: reference to a compiler-generated field
      mapInfo.precipitation = this.m_MapMetadataSystem.precipitation;
      // ISSUE: reference to a compiler-generated field
      mapInfo.latitude = this.m_MapMetadataSystem.latitude;
      // ISSUE: reference to a compiler-generated field
      mapInfo.longitude = this.m_MapMetadataSystem.longitude;
      // ISSUE: reference to a compiler-generated field
      mapInfo.area = this.m_MapMetadataSystem.area;
      // ISSUE: reference to a compiler-generated field
      mapInfo.waterAvailability = this.m_MapMetadataSystem.waterAvailability;
      // ISSUE: reference to a compiler-generated field
      mapInfo.resources = this.m_MapMetadataSystem.resources;
      // ISSUE: reference to a compiler-generated field
      mapInfo.connections = this.m_MapMetadataSystem.connections;
      // ISSUE: reference to a compiler-generated field
      mapInfo.nameAsCityName = this.m_MapNameAsCityName;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      mapInfo.startingYear = this.m_CurrentYearAsStartingYear ? -1 : this.m_StartingYear;
      // ISSUE: reference to a compiler-generated field
      mapInfo.buildableLand = this.m_MapMetadataSystem.buildableLand;
      return mapInfo;
    }

    private void CaptureCameraProperties()
    {
      CameraController cameraController;
      if (!CameraController.TryGet(out cameraController))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem.m_CameraPivot = (float3) cameraController.pivot;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem.m_CameraAngle = cameraController.angle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem.m_CameraZoom = cameraController.zoom;
    }

    private void ToggleMapTileSelection()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MapTilePurchaseSystem.selecting = !this.m_MapTilePurchaseSystem.selecting;
    }

    private void UpdateStartingTiles()
    {
    }

    private void UpdateMapTileButton(string text)
    {
      // ISSUE: reference to a compiler-generated field
      if (!(this.m_MapTileSelectionButton.displayName.value != text))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileSelectionButton.displayName = (LocalizedString) text;
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileSelectionButton.SetPropertiesChanged();
    }

    [Preserve]
    public MapPanelSystem()
    {
    }

    private struct PreviewInfo : IEquatable<MapPanelSystem.PreviewInfo>
    {
      private ImageAsset m_ImageAsset;
      private TextureAsset m_TextureAsset;

      public Texture texture
      {
        get
        {
          Texture2D texture = this.m_ImageAsset?.Load(true);
          if (texture != null)
            return (Texture) texture;
          return this.m_TextureAsset?.Load(-1);
        }
      }

      public IconButton button { get; set; }

      public string name
      {
        get
        {
          string name = this.m_ImageAsset?.name;
          if (name != null)
            return name;
          return this.m_TextureAsset?.name;
        }
      }

      public void Set(ImageAsset imageAsset, TextureAsset fallback = null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ImageAsset = imageAsset;
        // ISSUE: reference to a compiler-generated field
        this.m_TextureAsset = (TextureAsset) null;
        this.button.icon = (imageAsset != null ? imageAsset.ToUri() : (string) null) ?? (fallback != null ? fallback.ToUri((TextureAsset) null, 0) : (string) null);
      }

      public void Set(TextureAsset textureAsset, TextureAsset fallback = null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TextureAsset = textureAsset;
        // ISSUE: reference to a compiler-generated field
        this.m_ImageAsset = (ImageAsset) null;
        this.button.icon = (textureAsset != null ? textureAsset.ToUri((TextureAsset) null, 0) : (string) null) ?? (fallback != null ? fallback.ToUri((TextureAsset) null, 0) : (string) null);
      }

      public PreviewInfo(IconButton button)
      {
        this.button = button;
        // ISSUE: reference to a compiler-generated field
        this.m_ImageAsset = (ImageAsset) null;
        // ISSUE: reference to a compiler-generated field
        this.m_TextureAsset = (TextureAsset) null;
      }

      public bool CopyToTextureAsset(
        ILocalAssetDatabase db,
        AssetDataPath path,
        out TextureAsset asset)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((AssetData) this.m_ImageAsset != (IAssetData) null || (AssetData) this.m_TextureAsset != (IAssetData) null)
        {
          asset = db.AddAsset<TextureAsset>(path);
          // ISSUE: reference to a compiler-generated field
          if ((AssetData) this.m_ImageAsset != (IAssetData) null)
          {
            // ISSUE: reference to a compiler-generated field
            Texture2D src = this.m_ImageAsset.Load(false);
            Texture2D dst = new Texture2D(src.width, src.height, src.graphicsFormat, TextureCreationFlags.DontInitializePixels);
            Graphics.CopyTexture((Texture) src, (Texture) dst);
            asset.SetData((Texture) dst);
            return true;
          }
          // ISSUE: reference to a compiler-generated field
          using (Stream readStream = this.m_TextureAsset.GetReadStream())
          {
            using (Stream writeStream = asset.GetWriteStream())
            {
              readStream.CopyTo(writeStream);
              return true;
            }
          }
        }
        else
        {
          asset = (TextureAsset) null;
          return false;
        }
      }

      public bool Equals(MapPanelSystem.PreviewInfo other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (AssetData) this.m_ImageAsset == (IAssetData) other.m_ImageAsset && (AssetData) this.m_TextureAsset == (IAssetData) other.m_TextureAsset;
      }
    }
  }
}
