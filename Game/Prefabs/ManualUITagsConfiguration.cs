// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ManualUITagsConfiguration
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class ManualUITagsConfiguration : PrefabBase
  {
    [Header("Chirper")]
    public UITagPrefab m_ChirperPanel;
    public UITagPrefab m_ChirperPanelButton;
    public UITagPrefab m_ChirperPanelChirps;
    [Header("City Info Panel")]
    public UITagPrefab m_CityInfoPanel;
    public UITagPrefab m_CityInfoPanelButton;
    public UITagPrefab m_CityInfoPanelDemandPage;
    public UITagPrefab m_CityInfoPanelDemandTab;
    public UITagPrefab m_CityInfoPanelPoliciesPage;
    public UITagPrefab m_CityInfoPanelPoliciesTab;
    [Header("Economy Panel")]
    public UITagPrefab m_EconomyPanelBudgetBalance;
    public UITagPrefab m_EconomyPanelBudgetExpenses;
    public UITagPrefab m_EconomyPanelBudgetPage;
    public UITagPrefab m_EconomyPanelBudgetRevenue;
    public UITagPrefab m_EconomyPanelBudgetTab;
    public UITagPrefab m_EconomyPanelButton;
    public UITagPrefab m_EconomyPanelLoansAccept;
    public UITagPrefab m_EconomyPanelLoansPage;
    public UITagPrefab m_EconomyPanelLoansSlider;
    public UITagPrefab m_EconomyPanelLoansTab;
    public UITagPrefab m_EconomyPanelProductionPage;
    public UITagPrefab m_EconomyPanelProductionResources;
    public UITagPrefab m_EconomyPanelProductionTab;
    public UITagPrefab m_EconomyPanelServicesBudget;
    public UITagPrefab m_EconomyPanelServicesList;
    public UITagPrefab m_EconomyPanelServicesPage;
    public UITagPrefab m_EconomyPanelServicesTab;
    public UITagPrefab m_EconomyPanelTaxationEstimate;
    public UITagPrefab m_EconomyPanelTaxationPage;
    public UITagPrefab m_EconomyPanelTaxationRate;
    public UITagPrefab m_EconomyPanelTaxationTab;
    public UITagPrefab m_EconomyPanelTaxationType;
    [Header("Event Journal")]
    public UITagPrefab m_EventJournalPanel;
    public UITagPrefab m_EventJournalPanelButton;
    [Header("Infoviews")]
    public UITagPrefab m_InfoviewsButton;
    public UITagPrefab m_InfoviewsMenu;
    public UITagPrefab m_InfoviewsPanel;
    public UITagPrefab m_InfoviewsFireHazard;
    [Header("Life Path Panel")]
    public UITagPrefab m_LifePathPanel;
    public UITagPrefab m_LifePathPanelBackButton;
    public UITagPrefab m_LifePathPanelButton;
    public UITagPrefab m_LifePathPanelChirps;
    public UITagPrefab m_LifePathPanelDetails;
    [Header("Map Tile Panel")]
    public UITagPrefab m_MapTilePanel;
    public UITagPrefab m_MapTilePanelButton;
    public UITagPrefab m_MapTilePanelResources;
    public UITagPrefab m_MapTilePanelPurchase;
    [Header("Photo Mode Panel")]
    public UITagPrefab m_PhotoModePanel;
    public UITagPrefab m_PhotoModePanelButton;
    public UITagPrefab m_PhotoModePanelHideUI;
    public UITagPrefab m_PhotoModePanelTakePicture;
    public UITagPrefab m_PhotoModeTab;
    public UITagPrefab m_PhotoModePanelTitle;
    public UITagPrefab m_PhotoModeCinematicCameraToggle;
    [Header("Cinematic Camera Panel")]
    public UITagPrefab m_CinematicCameraPanel;
    public UITagPrefab m_CinematicCameraPanelCaptureKey;
    public UITagPrefab m_CinematicCameraPanelPlay;
    public UITagPrefab m_CinematicCameraPanelStop;
    public UITagPrefab m_CinematicCameraPanelHideUI;
    public UITagPrefab m_CinematicCameraPanelSaveLoad;
    public UITagPrefab m_CinematicCameraPanelReset;
    public UITagPrefab m_CinematicCameraPanelTimelineSlider;
    public UITagPrefab m_CinematicCameraPanelTransformCurves;
    public UITagPrefab m_CinematicCameraPanelPropertyCurves;
    public UITagPrefab m_CinematicCameraPanelPlaybackDurationSlider;
    [Header("Progression Panel")]
    public UITagPrefab m_ProgressionPanel;
    public UITagPrefab m_ProgressionPanelButton;
    public UITagPrefab m_ProgressionPanelDevelopmentNode;
    public UITagPrefab m_ProgressionPanelDevelopmentPage;
    public UITagPrefab m_ProgressionPanelDevelopmentService;
    public UITagPrefab m_ProgressionPanelDevelopmentTab;
    public UITagPrefab m_ProgressionPanelDevelopmentUnlockableNode;
    public UITagPrefab m_ProgressionPanelDevelopmentUnlockNode;
    public UITagPrefab m_ProgressionPanelMilestoneRewards;
    public UITagPrefab m_ProgressionPanelMilestoneRewardsMoney;
    public UITagPrefab m_ProgressionPanelMilestoneRewardsDevPoints;
    public UITagPrefab m_ProgressionPanelMilestoneRewardsMapTiles;
    public UITagPrefab m_ProgressionPanelMilestonesList;
    public UITagPrefab m_ProgressionPanelMilestonesPage;
    public UITagPrefab m_ProgressionPanelMilestonesTab;
    public UITagPrefab m_ProgressionPanelMilestoneXP;
    [Header("Radio Panel")]
    public UITagPrefab m_RadioPanel;
    public UITagPrefab m_RadioPanelAdsToggle;
    public UITagPrefab m_RadioPanelButton;
    public UITagPrefab m_RadioPanelNetworks;
    public UITagPrefab m_RadioPanelStations;
    public UITagPrefab m_RadioPanelVolumeSlider;
    [Header("Statistics Panel")]
    public UITagPrefab m_StatisticsPanel;
    public UITagPrefab m_StatisticsPanelButton;
    public UITagPrefab m_StatisticsPanelMenu;
    public UITagPrefab m_StatisticsPanelTimeScale;
    [Header("Toolbar")]
    public UITagPrefab m_ToolbarBulldozerBar;
    public UITagPrefab m_ToolbarDemand;
    public UITagPrefab m_ToolbarSimulationDateTime;
    public UITagPrefab m_ToolbarSimulationSpeed;
    public UITagPrefab m_ToolbarSimulationToggle;
    public UITagPrefab m_ToolbarUnderground;
    [Header("Tool Options")]
    public UITagPrefab m_ToolOptions;
    public UITagPrefab m_ToolOptionsBrushSize;
    public UITagPrefab m_ToolOptionsBrushStrength;
    public UITagPrefab m_ToolOptionsElevation;
    public UITagPrefab m_ToolOptionsElevationDecrease;
    public UITagPrefab m_ToolOptionsElevationIncrease;
    public UITagPrefab m_ToolOptionsElevationStep;
    public UITagPrefab m_ToolOptionsModes;
    public UITagPrefab m_ToolOptionsModesComplexCurve;
    public UITagPrefab m_ToolOptionsModesContinuous;
    public UITagPrefab m_ToolOptionsModesGrid;
    public UITagPrefab m_ToolOptionsModesReplace;
    public UITagPrefab m_ToolOptionsModesSimpleCurve;
    public UITagPrefab m_ToolOptionsModesStraight;
    public UITagPrefab m_ToolOptionsParallelMode;
    public UITagPrefab m_ToolOptionsParallelModeOffset;
    public UITagPrefab m_ToolOptionsParallelModeOffsetDecrease;
    public UITagPrefab m_ToolOptionsParallelModeOffsetIncrease;
    public UITagPrefab m_ToolOptionsSnapping;
    public UITagPrefab m_ToolOptionsThemes;
    public UITagPrefab m_ToolOptionsAssetPacks;
    public UITagPrefab m_ToolOptionsUnderground;
    [Header("Transportation Overview Panel")]
    public UITagPrefab m_TransportationOverviewPanel;
    public UITagPrefab m_TransportationOverviewPanelButton;
    public UITagPrefab m_TransportationOverviewPanelLegend;
    public UITagPrefab m_TransportationOverviewPanelLines;
    public UITagPrefab m_TransportationOverviewPanelTabCargo;
    public UITagPrefab m_TransportationOverviewPanelTabPublicTransport;
    public UITagPrefab m_TransportationOverviewPanelTransportTypes;
    [Header("Selected Info Panel")]
    public UITagPrefab m_SelectedInfoPanel;
    public UITagPrefab m_SelectedInfoPanelTitle;
    public UITagPrefab m_SelectedInfoPanelPolicies;
    public UITagPrefab m_SelectedInfoPanelDelete;
    [Header("General")]
    public UITagPrefab m_PauseMenuButton;
    public UITagPrefab m_UpgradeGrid;
    public UITagPrefab m_AssetGrid;
    public UITagPrefab m_ActionHints;
    [Header("Editor")]
    public UITagPrefab m_AssetImportButton;
    public UITagPrefab m_EditorInfoViewsPanel;
    public UITagPrefab m_ResetTODButton;
    public UITagPrefab m_SimulationPlayButton;
    public UITagPrefab m_TutorialsToggle;
    public UITagPrefab m_WorkspaceTitleBar;
    public UITagPrefab m_SelectProjectRoot;
    public UITagPrefab m_SelectAssets;
    public UITagPrefab m_SelectTemplate;
    public UITagPrefab m_ImportButton;
    public UITagPrefab m_ModifyTerrainButton;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_ChirperPanel);
      prefabs.Add((PrefabBase) this.m_ChirperPanelButton);
      prefabs.Add((PrefabBase) this.m_ChirperPanelChirps);
      prefabs.Add((PrefabBase) this.m_CityInfoPanel);
      prefabs.Add((PrefabBase) this.m_CityInfoPanelButton);
      prefabs.Add((PrefabBase) this.m_CityInfoPanelDemandPage);
      prefabs.Add((PrefabBase) this.m_CityInfoPanelDemandTab);
      prefabs.Add((PrefabBase) this.m_CityInfoPanelPoliciesPage);
      prefabs.Add((PrefabBase) this.m_CityInfoPanelPoliciesTab);
      prefabs.Add((PrefabBase) this.m_EconomyPanelBudgetBalance);
      prefabs.Add((PrefabBase) this.m_EconomyPanelBudgetExpenses);
      prefabs.Add((PrefabBase) this.m_EconomyPanelBudgetPage);
      prefabs.Add((PrefabBase) this.m_EconomyPanelBudgetRevenue);
      prefabs.Add((PrefabBase) this.m_EconomyPanelBudgetTab);
      prefabs.Add((PrefabBase) this.m_EconomyPanelButton);
      prefabs.Add((PrefabBase) this.m_EconomyPanelLoansAccept);
      prefabs.Add((PrefabBase) this.m_EconomyPanelLoansPage);
      prefabs.Add((PrefabBase) this.m_EconomyPanelLoansSlider);
      prefabs.Add((PrefabBase) this.m_EconomyPanelLoansTab);
      prefabs.Add((PrefabBase) this.m_EconomyPanelProductionPage);
      prefabs.Add((PrefabBase) this.m_EconomyPanelProductionResources);
      prefabs.Add((PrefabBase) this.m_EconomyPanelProductionTab);
      prefabs.Add((PrefabBase) this.m_EconomyPanelServicesBudget);
      prefabs.Add((PrefabBase) this.m_EconomyPanelServicesList);
      prefabs.Add((PrefabBase) this.m_EconomyPanelServicesPage);
      prefabs.Add((PrefabBase) this.m_EconomyPanelServicesTab);
      prefabs.Add((PrefabBase) this.m_EconomyPanelTaxationEstimate);
      prefabs.Add((PrefabBase) this.m_EconomyPanelTaxationPage);
      prefabs.Add((PrefabBase) this.m_EconomyPanelTaxationRate);
      prefabs.Add((PrefabBase) this.m_EconomyPanelTaxationTab);
      prefabs.Add((PrefabBase) this.m_EconomyPanelTaxationType);
      prefabs.Add((PrefabBase) this.m_EventJournalPanel);
      prefabs.Add((PrefabBase) this.m_EventJournalPanelButton);
      prefabs.Add((PrefabBase) this.m_InfoviewsButton);
      prefabs.Add((PrefabBase) this.m_InfoviewsMenu);
      prefabs.Add((PrefabBase) this.m_InfoviewsPanel);
      prefabs.Add((PrefabBase) this.m_InfoviewsFireHazard);
      prefabs.Add((PrefabBase) this.m_LifePathPanel);
      prefabs.Add((PrefabBase) this.m_LifePathPanelBackButton);
      prefabs.Add((PrefabBase) this.m_LifePathPanelButton);
      prefabs.Add((PrefabBase) this.m_LifePathPanelChirps);
      prefabs.Add((PrefabBase) this.m_LifePathPanelDetails);
      prefabs.Add((PrefabBase) this.m_MapTilePanel);
      prefabs.Add((PrefabBase) this.m_MapTilePanelButton);
      prefabs.Add((PrefabBase) this.m_MapTilePanelResources);
      prefabs.Add((PrefabBase) this.m_MapTilePanelPurchase);
      prefabs.Add((PrefabBase) this.m_PhotoModePanel);
      prefabs.Add((PrefabBase) this.m_PhotoModePanelButton);
      prefabs.Add((PrefabBase) this.m_PhotoModePanelHideUI);
      prefabs.Add((PrefabBase) this.m_PhotoModePanelTakePicture);
      prefabs.Add((PrefabBase) this.m_PhotoModeTab);
      prefabs.Add((PrefabBase) this.m_PhotoModePanelTitle);
      prefabs.Add((PrefabBase) this.m_PhotoModeCinematicCameraToggle);
      prefabs.Add((PrefabBase) this.m_CinematicCameraPanel);
      prefabs.Add((PrefabBase) this.m_CinematicCameraPanelCaptureKey);
      prefabs.Add((PrefabBase) this.m_CinematicCameraPanelPlay);
      prefabs.Add((PrefabBase) this.m_CinematicCameraPanelStop);
      prefabs.Add((PrefabBase) this.m_CinematicCameraPanelHideUI);
      prefabs.Add((PrefabBase) this.m_CinematicCameraPanelSaveLoad);
      prefabs.Add((PrefabBase) this.m_CinematicCameraPanelReset);
      prefabs.Add((PrefabBase) this.m_CinematicCameraPanelTimelineSlider);
      prefabs.Add((PrefabBase) this.m_CinematicCameraPanelTransformCurves);
      prefabs.Add((PrefabBase) this.m_CinematicCameraPanelPropertyCurves);
      prefabs.Add((PrefabBase) this.m_ProgressionPanel);
      prefabs.Add((PrefabBase) this.m_ProgressionPanelButton);
      prefabs.Add((PrefabBase) this.m_ProgressionPanelDevelopmentNode);
      prefabs.Add((PrefabBase) this.m_ProgressionPanelDevelopmentPage);
      prefabs.Add((PrefabBase) this.m_ProgressionPanelDevelopmentService);
      prefabs.Add((PrefabBase) this.m_ProgressionPanelDevelopmentTab);
      prefabs.Add((PrefabBase) this.m_ProgressionPanelDevelopmentUnlockableNode);
      prefabs.Add((PrefabBase) this.m_ProgressionPanelDevelopmentUnlockNode);
      prefabs.Add((PrefabBase) this.m_ProgressionPanelMilestoneRewards);
      prefabs.Add((PrefabBase) this.m_ProgressionPanelMilestoneRewardsMoney);
      prefabs.Add((PrefabBase) this.m_ProgressionPanelMilestoneRewardsDevPoints);
      prefabs.Add((PrefabBase) this.m_ProgressionPanelMilestoneRewardsMapTiles);
      prefabs.Add((PrefabBase) this.m_ProgressionPanelMilestonesList);
      prefabs.Add((PrefabBase) this.m_ProgressionPanelMilestonesPage);
      prefabs.Add((PrefabBase) this.m_ProgressionPanelMilestonesTab);
      prefabs.Add((PrefabBase) this.m_ProgressionPanelMilestoneXP);
      prefabs.Add((PrefabBase) this.m_RadioPanel);
      prefabs.Add((PrefabBase) this.m_RadioPanelAdsToggle);
      prefabs.Add((PrefabBase) this.m_RadioPanelButton);
      prefabs.Add((PrefabBase) this.m_RadioPanelNetworks);
      prefabs.Add((PrefabBase) this.m_RadioPanelStations);
      prefabs.Add((PrefabBase) this.m_RadioPanelVolumeSlider);
      prefabs.Add((PrefabBase) this.m_StatisticsPanel);
      prefabs.Add((PrefabBase) this.m_StatisticsPanelButton);
      prefabs.Add((PrefabBase) this.m_StatisticsPanelMenu);
      prefabs.Add((PrefabBase) this.m_StatisticsPanelTimeScale);
      prefabs.Add((PrefabBase) this.m_ToolbarBulldozerBar);
      prefabs.Add((PrefabBase) this.m_ToolbarDemand);
      prefabs.Add((PrefabBase) this.m_ToolbarSimulationDateTime);
      prefabs.Add((PrefabBase) this.m_ToolbarSimulationSpeed);
      prefabs.Add((PrefabBase) this.m_ToolbarSimulationToggle);
      prefabs.Add((PrefabBase) this.m_ToolbarUnderground);
      prefabs.Add((PrefabBase) this.m_ToolOptions);
      prefabs.Add((PrefabBase) this.m_ToolOptionsBrushSize);
      prefabs.Add((PrefabBase) this.m_ToolOptionsBrushStrength);
      prefabs.Add((PrefabBase) this.m_ToolOptionsElevation);
      prefabs.Add((PrefabBase) this.m_ToolOptionsElevationDecrease);
      prefabs.Add((PrefabBase) this.m_ToolOptionsElevationIncrease);
      prefabs.Add((PrefabBase) this.m_ToolOptionsElevationStep);
      prefabs.Add((PrefabBase) this.m_ToolOptionsModes);
      prefabs.Add((PrefabBase) this.m_ToolOptionsModesComplexCurve);
      prefabs.Add((PrefabBase) this.m_ToolOptionsModesContinuous);
      prefabs.Add((PrefabBase) this.m_ToolOptionsModesGrid);
      prefabs.Add((PrefabBase) this.m_ToolOptionsModesReplace);
      prefabs.Add((PrefabBase) this.m_ToolOptionsModesSimpleCurve);
      prefabs.Add((PrefabBase) this.m_ToolOptionsModesStraight);
      prefabs.Add((PrefabBase) this.m_ToolOptionsParallelMode);
      prefabs.Add((PrefabBase) this.m_ToolOptionsParallelModeOffset);
      prefabs.Add((PrefabBase) this.m_ToolOptionsParallelModeOffsetDecrease);
      prefabs.Add((PrefabBase) this.m_ToolOptionsParallelModeOffsetIncrease);
      prefabs.Add((PrefabBase) this.m_ToolOptionsSnapping);
      prefabs.Add((PrefabBase) this.m_ToolOptionsThemes);
      prefabs.Add((PrefabBase) this.m_ToolOptionsAssetPacks);
      prefabs.Add((PrefabBase) this.m_ToolOptionsUnderground);
      prefabs.Add((PrefabBase) this.m_TransportationOverviewPanel);
      prefabs.Add((PrefabBase) this.m_TransportationOverviewPanelButton);
      prefabs.Add((PrefabBase) this.m_TransportationOverviewPanelLegend);
      prefabs.Add((PrefabBase) this.m_TransportationOverviewPanelLines);
      prefabs.Add((PrefabBase) this.m_TransportationOverviewPanelTabCargo);
      prefabs.Add((PrefabBase) this.m_TransportationOverviewPanelTabPublicTransport);
      prefabs.Add((PrefabBase) this.m_TransportationOverviewPanelTransportTypes);
      prefabs.Add((PrefabBase) this.m_SelectedInfoPanel);
      prefabs.Add((PrefabBase) this.m_SelectedInfoPanelTitle);
      prefabs.Add((PrefabBase) this.m_PauseMenuButton);
      prefabs.Add((PrefabBase) this.m_UpgradeGrid);
      prefabs.Add((PrefabBase) this.m_AssetGrid);
      prefabs.Add((PrefabBase) this.m_ActionHints);
      prefabs.Add((PrefabBase) this.m_AssetImportButton);
      prefabs.Add((PrefabBase) this.m_EditorInfoViewsPanel);
      prefabs.Add((PrefabBase) this.m_ResetTODButton);
      prefabs.Add((PrefabBase) this.m_SimulationPlayButton);
      prefabs.Add((PrefabBase) this.m_TutorialsToggle);
      prefabs.Add((PrefabBase) this.m_WorkspaceTitleBar);
      prefabs.Add((PrefabBase) this.m_SelectProjectRoot);
      prefabs.Add((PrefabBase) this.m_SelectAssets);
      prefabs.Add((PrefabBase) this.m_SelectTemplate);
      prefabs.Add((PrefabBase) this.m_ImportButton);
      prefabs.Add((PrefabBase) this.m_ModifyTerrainButton);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ManualUITagsConfigurationData>());
    }
  }
}
