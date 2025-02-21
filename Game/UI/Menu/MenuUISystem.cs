// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.MenuUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Entities;
using Colossal.IO.AssetDatabase;
using Colossal.PSI.Common;
using Colossal.Serialization.Entities;
using Colossal.UI;
using Colossal.UI.Binding;
using Game.Assets;
using Game.City;
using Game.Modding;
using Game.PSI.PdxSdk;
using Game.SceneFlow;
using Game.Serialization;
using Game.Settings;
using Game.Simulation;
using Game.UI.InGame;
using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Menu
{
  [CompilerGenerated]
  public class MenuUISystem : UISystemBase, IPreDeserialize
  {
    private const string kPreviewName = "SaveGamePanel";
    private const int kPreviewWidth = 680;
    private const int kPreviewHeight = 383;
    private const string kGroup = "menu";
    private CityConfigurationSystem m_CityConfigurationSystem;
    private TimeSystem m_TimeSystem;
    private MapMetadataSystem m_MapMetadataSystem;
    private StandaloneAssetUploadPanelUISystem m_AssetUploadPanelUISystem;
    private GameScreenUISystem m_GameScreenUISystem;
    private ValueBinding<int> m_ActiveScreenBinding;
    private GetterValueBinding<List<MenuUISystem.ThemeInfo>> m_ThemesBinding;
    private ValueBinding<List<MapInfo>> m_MapsBinding;
    private ValueBinding<HashSet<int>> m_AvailableMapFilters;
    private ValueBinding<int> m_SelectedMapFilter;
    private ValueBinding<List<SaveInfo>> m_SavesBinding;
    private ValueBinding<string> m_SavePreviewBinding;
    private ValueBinding<string> m_LastSaveNameBinding;
    private ValueBinding<int> m_SaveGameSlotsBinding;
    private GetterValueBinding<MenuUISystem.SaveabilityStatus> m_SaveabilityBinding;
    private ValueBinding<List<string>> m_AvailableCloudTargetsBinding;
    private GetterValueBinding<string> m_SelectedCloudTargetBinding;
    private MenuUISystem.DefaultGameOptions m_DefaultGameOptions;
    private GetterValueBinding<bool> m_CanContinueBinding;
    private MenuHelpers.SaveGamePreviewSettings m_PreviewSettings = new MenuHelpers.SaveGamePreviewSettings();
    private EntityQuery m_XPQuery;
    private bool m_IsLoading;
    private string m_LastSelectedCloudTarget;
    private PdxModsUI m_ModsUI;
    private static int s_PreviewId;

    private bool IsEditorEnabled()
    {
      return !GameManager.instance.configuration.disableModding && Platform.PC.IsPlatformSet(Application.platform);
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MapMetadataSystem = this.World.GetOrCreateSystemManaged<MapMetadataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AssetUploadPanelUISystem = this.World.GetOrCreateSystemManaged<StandaloneAssetUploadPanelUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GameScreenUISystem = this.World.GetOrCreateSystemManaged<GameScreenUISystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_DefaultGameOptions = new MenuUISystem.DefaultGameOptions();
      // ISSUE: reference to a compiler-generated field
      Colossal.IO.AssetDatabase.AssetDatabase.global.LoadSettings("Save Preview Settings", (object) this.m_PreviewSettings);
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ActiveScreenBinding = new ValueBinding<int>("menu", "activeScreen", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CanContinueBinding = new GetterValueBinding<bool>("menu", "canContinueGame", (Func<bool>) (() => MenuHelpers.hasPreviouslySavedGame))));
      this.AddBinding((IBinding) new ValueBinding<bool>("menu", "canExitGame", !Application.isConsolePlatform));
      this.AddBinding((IBinding) new ValueBinding<string>("menu", "gameVersion", Game.Version.current.fullVersion));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SavePreviewBinding = new ValueBinding<string>("menu", "savePreview", (string) null, (IWriter<string>) new StringWriter().Nullable<string>())));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<string>("menu", "mapName", (Func<string>) (() => this.m_MapMetadataSystem.mapName), (IWriter<string>) new StringWriter().Nullable<string>()));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_LastSaveNameBinding = new ValueBinding<string>("menu", "lastSaveName", (string) null, (IWriter<string>) new StringWriter().Nullable<string>())));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SaveGameSlotsBinding = new ValueBinding<int>("menu", "saveGameSlots", -1)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AvailableCloudTargetsBinding = new ValueBinding<List<string>>("menu", "availableCloudTargets", MenuHelpers.GetAvailableCloudTargets(), (IWriter<List<string>>) new ListWriter<string>())));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) (this.m_SelectedCloudTargetBinding = new GetterValueBinding<string>("menu", "selectedCloudTarget", (Func<string>) (() => MenuHelpers.GetSanitizedCloudTarget(SharedSettings.instance.userState.lastCloudTarget).name), (IWriter<string>) new StringWriter().Nullable<string>())));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SaveabilityBinding = new GetterValueBinding<MenuUISystem.SaveabilityStatus>("menu", "saveabilityStatus", (Func<MenuUISystem.SaveabilityStatus>) (() =>
      {
        int count = MenuHelpers.GetAvailableCloudTargets().Count;
        // ISSUE: object of a compiler-generated type is created
        return new MenuUISystem.SaveabilityStatus()
        {
          canSave = count > 0,
          reasonHash = count > 0 ? (string) null : "NoLocations"
        };
      }), (IWriter<MenuUISystem.SaveabilityStatus>) new ValueWriter<MenuUISystem.SaveabilityStatus>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      this.AddBinding((IBinding) (this.m_ThemesBinding = new GetterValueBinding<List<MenuUISystem.ThemeInfo>>("menu", "themes", (Func<List<MenuUISystem.ThemeInfo>>) (() => new List<MenuUISystem.ThemeInfo>()
      {
        new MenuUISystem.ThemeInfo()
        {
          id = "European",
          icon = "Media/Game/Themes/European.svg"
        },
        new MenuUISystem.ThemeInfo()
        {
          id = "North American",
          icon = "Media/Game/Themes/North American.svg"
        }
      }), (IWriter<List<MenuUISystem.ThemeInfo>>) new ListWriter<MenuUISystem.ThemeInfo>((IWriter<MenuUISystem.ThemeInfo>) new ValueWriter<MenuUISystem.ThemeInfo>()))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_MapsBinding = new ValueBinding<List<MapInfo>>("menu", "maps", new List<MapInfo>(), (IWriter<List<MapInfo>>) new ListWriter<MapInfo>((IWriter<MapInfo>) new ValueWriter<MapInfo>()))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_AvailableMapFilters = new ValueBinding<HashSet<int>>("menu", "availableMapFilters", this.GetAvailableMapFilters(), (IWriter<HashSet<int>>) new CollectionWriter<int>())));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SelectedMapFilter = new ValueBinding<int>("menu", "selectedMapFilter", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SavesBinding = new ValueBinding<List<SaveInfo>>("menu", "saves", new List<SaveInfo>(), (IWriter<List<SaveInfo>>) new ListWriter<SaveInfo>((IWriter<SaveInfo>) new ValueWriter<SaveInfo>()))));
      this.AddBinding((IBinding) new GetterValueBinding<List<string>>("menu", "creditFiles", (Func<List<string>>) (() => new List<string>()
      {
        "Media/Menu/Credits.md",
        "Media/Menu/Licenses.md"
      }), (IWriter<List<string>>) new ListWriter<string>((IWriter<string>) new StringWriter())));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<MenuUISystem.DefaultGameOptions>("menu", "defaultGameOptions", (Func<MenuUISystem.DefaultGameOptions>) (() => this.m_DefaultGameOptions), (IWriter<MenuUISystem.DefaultGameOptions>) new ValueWriter<MenuUISystem.DefaultGameOptions>()));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("menu", "modsEnabled", new Func<bool>(ModManager.AreModsEnabled)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("menu", "pdxModsUIEnabled", (Func<bool>) (() => !GameManager.instance.configuration.disablePDXSDK && !GameManager.instance.configuration.disableModding)));
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new ValueBinding<bool>("menu", "hideModsUIButton", !this.IsModdingEnabled()));
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new ValueBinding<bool>("menu", "hideEditorButton", !this.IsEditorEnabled()));
      this.AddBinding((IBinding) new ValueBinding<bool>("menu", "displayModdingBetaBanners", true));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("menu", "hasCompletedTutorials", (Func<bool>) (() => SharedSettings.instance.userState.shownTutorials.ContainsValue(true))));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("menu", "showTutorials", (Func<bool>) (() => SharedSettings.instance.gameplay.showTutorials)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("menu", "dismissLoadGameConfirmation", (Func<bool>) (() => SharedSettings.instance.userInterface.dismissedConfirmations.Contains("LoadGame"))));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("menu", "isModsUIActive", (Func<bool>) (() => this.m_ModsUI.isActive)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<int>("menu", "setActiveScreen", new Action<int>(this.m_ActiveScreenBinding.Update)));
      this.AddBinding((IBinding) new TriggerBinding("menu", "continueGame", (Action) (() => TaskManager.instance.EnqueueTask("SaveLoadGame", (Func<Task>) (async () =>
      {
        try
        {
          SaveGameMetadata lastSave = GameManager.instance.settings.userState.lastSaveGameMetadata;
          if ((AssetData) lastSave != (IAssetData) null && lastSave.isValidSaveGame)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_MapMetadataSystem.mapName = lastSave.target.mapName;
            PlatformManager.instance.achievementsEnabled = !lastSave.target.isReadonly;
            int num = await GameManager.instance.Load(GameMode.Game, Purpose.LoadGame, (IAssetData) lastSave) ? 1 : 0;
            SaveInfo target = lastSave.target;
            // ISSUE: reference to a compiler-generated field
            this.m_LastSaveNameBinding.Update(target.autoSave ? (string) null : target.displayName);
            if (!target.autoSave)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LastSelectedCloudTarget = target.metaData.database.dataSource.remoteStorageSourceName;
            }
          }
          lastSave = (SaveGameMetadata) null;
        }
        catch (OperationCanceledException ex)
        {
        }
        catch (Exception ex)
        {
          COSystemBase.baseLog.Error(ex);
        }
      }), 1))));
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding<MenuUISystem.NewGameArgs>("menu", "newGame", (Action<MenuUISystem.NewGameArgs>) (args => TaskManager.instance.EnqueueTask("SaveLoadGame", (Func<Task>) (() => this.NewGame(args)), 1)), (IReader<MenuUISystem.NewGameArgs>) new ValueReader<MenuUISystem.NewGameArgs>()));
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding<MenuUISystem.LoadGameArgs, bool>("menu", "loadGame", (Action<MenuUISystem.LoadGameArgs, bool>) ((args, dismiss) => TaskManager.instance.EnqueueTask("SaveLoadGame", (Func<Task>) (() => this.LoadGame(args, dismiss)), 1)), (IReader<MenuUISystem.LoadGameArgs>) new ValueReader<MenuUISystem.LoadGameArgs>()));
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding<string>("menu", "saveGame", (Action<string>) (saveName => TaskManager.instance.EnqueueTask("SaveLoadGame", (Func<Task>) (() => this.SaveGame(saveName)), 1))));
      this.AddBinding((IBinding) new TriggerBinding<string>("menu", "deleteSave", (Action<string>) (guid =>
      {
        try
        {
          // ISSUE: reference to a compiler-generated field
          SaveHelpers.DeleteSaveGame(this.m_SavesBinding.value.Find((Predicate<SaveInfo>) (x => x.id == guid)).metaData);
        }
        catch (OperationCanceledException ex)
        {
        }
        catch (Exception ex)
        {
          COSystemBase.baseLog.Error(ex);
        }
      })));
      this.AddBinding((IBinding) new TriggerBinding<string>("menu", "shareSave", (Action<string>) (id =>
      {
        // ISSUE: reference to a compiler-generated field
        foreach (SaveInfo saveInfo in this.m_SavesBinding.value)
        {
          if (saveInfo.id == id)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_AssetUploadPanelUISystem.Show((AssetData) saveInfo.metaData);
            break;
          }
        }
      })));
      this.AddBinding((IBinding) new TriggerBinding<string>("menu", "shareMap", (Action<string>) (id =>
      {
        // ISSUE: reference to a compiler-generated field
        foreach (MapInfo mapInfo in this.m_MapsBinding.value)
        {
          if (mapInfo.id == id)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_AssetUploadPanelUISystem.Show((AssetData) mapInfo.metaData);
            break;
          }
        }
      })));
      this.AddBinding((IBinding) new TriggerBinding("menu", "quicksave", (Action) (() => TaskManager.instance.EnqueueTask("SaveLoadGame", (Func<Task>) (async () =>
      {
        try
        {
          // ISSUE: reference to a compiler-generated field
          string saveName = this.m_LastSaveNameBinding.value;
          if (string.IsNullOrEmpty(saveName))
          {
            // ISSUE: reference to a compiler-generated field
            saveName = this.m_CityConfigurationSystem.cityName;
          }
          if (string.IsNullOrEmpty(saveName))
            saveName = "SaveGame";
          // ISSUE: reference to a compiler-generated field
          ILocalAssetDatabase targetDatabase = MenuHelpers.GetSanitizedCloudTarget(this.m_LastSelectedCloudTarget).db;
          if (targetDatabase.name != null)
          {
            RenderTexture savePreview = ScreenCaptureHelper.CreateRenderTarget("SaveGamePanel", 680, 383);
            // ISSUE: reference to a compiler-generated field
            ScreenCaptureHelper.CaptureScreenshot(Camera.main, savePreview, this.m_PreviewSettings);
            // ISSUE: reference to a compiler-generated method
            SaveInfo saveInfo = this.GetSaveInfo(false);
            // ISSUE: reference to a compiler-generated method
            if (await this.HandlesOverwrite(targetDatabase, saveName))
            {
              int num = await GameManager.instance.Save(saveName, saveInfo, targetDatabase, (Texture) savePreview) ? 1 : 0;
            }
            CoreUtils.Destroy((UnityEngine.Object) savePreview);
            savePreview = (RenderTexture) null;
            saveInfo = (SaveInfo) null;
          }
          saveName = (string) null;
          targetDatabase = (ILocalAssetDatabase) null;
        }
        catch (OperationCanceledException ex)
        {
        }
        catch (Exception ex)
        {
          COSystemBase.baseLog.Error(ex);
        }
      }), 1))));
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding<bool>("menu", "quickload", (Action<bool>) (dismiss => TaskManager.instance.EnqueueTask("SaveLoadGame", (Func<Task>) (() => this.QuickLoad(dismiss)), 1))));
      this.AddBinding((IBinding) new TriggerBinding("menu", "startEditor", (Action) (async () =>
      {
        try
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CityConfigurationSystem.overrideLoadedOptions = false;
          // ISSUE: reference to a compiler-generated field
          this.m_CityConfigurationSystem.overrideThemeName = (string) null;
          int num = await GameManager.instance.Load(GameMode.Editor, Purpose.NewMap).ConfigureAwait(false) ? 1 : 0;
        }
        catch (OperationCanceledException ex)
        {
        }
        catch (Exception ex)
        {
          COSystemBase.baseLog.Error(ex);
        }
      })));
      this.AddBinding((IBinding) new TriggerBinding("menu", "showPdxModsUI", (Action) (() =>
      {
        if (!PlatformManager.instance.hasUgcPrivilege)
          return;
        if (Platform.PlayStation.IsPlatformSet(Application.platform))
        {
          // ISSUE: reference to a compiler-generated field
          GameManager.instance.userInterface.paradoxBindings.OnPSModsUIOpened(new Action(this.m_ModsUI.Show));
          // ISSUE: reference to a compiler-generated field
          this.m_ModsUI.platform.onModsUIClosed -= (Action) (async () =>
          {
            // ISSUE: reference to a compiler-generated field
            List<PDX.SDK.Contracts.Service.Mods.Models.Mod> modsInActivePlayset = await this.m_ModsUI.platform.GetModsInActivePlayset();
            if (modsInActivePlayset == null || modsInActivePlayset.Count <= 0)
              return;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            GameManager.instance.userInterface.paradoxBindings.OnPSModsUIClosed((Action) null, new Action(this.m_ModsUI.platform.DeactivateActivePlayset), new Action(this.m_ModsUI.Show));
          });
          // ISSUE: reference to a compiler-generated field
          this.m_ModsUI.platform.onModsUIClosed += (Action) (async () =>
          {
            // ISSUE: reference to a compiler-generated field
            List<PDX.SDK.Contracts.Service.Mods.Models.Mod> modsInActivePlayset = await this.m_ModsUI.platform.GetModsInActivePlayset();
            if (modsInActivePlayset == null || modsInActivePlayset.Count <= 0)
              return;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            GameManager.instance.userInterface.paradoxBindings.OnPSModsUIClosed((Action) null, new Action(this.m_ModsUI.platform.DeactivateActivePlayset), new Action(this.m_ModsUI.Show));
          });
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ModsUI.Show();
        }
      })));
      this.AddBinding((IBinding) new TriggerBinding("menu", "exitToMainMenu", (Action) (async () =>
      {
        try
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CityConfigurationSystem.overrideLoadedOptions = false;
          // ISSUE: reference to a compiler-generated field
          this.m_CityConfigurationSystem.overrideThemeName = (string) null;
          int num = await GameManager.instance.MainMenu() ? 1 : 0;
        }
        catch (OperationCanceledException ex)
        {
        }
        catch (Exception ex)
        {
          COSystemBase.baseLog.Error(ex);
        }
      })));
      this.AddBinding((IBinding) new TriggerBinding<bool>("menu", "onSaveGameScreenVisibilityChanged", (Action<bool>) (visible =>
      {
        if (visible)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_SavePreviewBinding.Update(string.Format("{0}{1}/{2}?width={3}&height={4}&op={5}&{6}#{7}", (object) "screencapture://", (object) Camera.main.tag.ToLowerInvariant(), (object) "SaveGamePanel", (object) 680, (object) 383, (object) "Screenshot", (object) this.m_PreviewSettings.ToUri(), (object) MenuUISystem.s_PreviewId++));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SavePreviewBinding.Update((string) null);
        }
      })));
      this.AddBinding((IBinding) new TriggerBinding<bool, bool>("menu", "applyTutorialSettings", (Action<bool, bool>) ((showTutorials, resetTutorials) =>
      {
        SharedSettings.instance.gameplay.showTutorials = showTutorials;
        if (!resetTutorials)
          return;
        SharedSettings.instance.userState.ResetTutorials();
      })));
      this.AddBinding((IBinding) new TriggerBinding<string>("menu", "selectCloudTarget", (Action<string>) (cloudTarget =>
      {
        SharedSettings.instance.userState.lastCloudTarget = cloudTarget;
        Debug.Log((object) ("Max file length " + MenuHelpers.GetSanitizedCloudTarget(cloudTarget).db.dataSource.maxSupportedFileLength.ToString()));
      })));
      this.AddBinding((IBinding) new TriggerBinding<int>("menu", "selectMapFilter", (Action<int>) (tab =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedMapFilter.Update(tab);
        // ISSUE: reference to a compiler-generated method
        this.UpdateMaps();
      })));
      // ISSUE: reference to a compiler-generated field
      this.m_XPQuery = this.GetEntityQuery(ComponentType.ReadOnly<XP>());
      Colossal.IO.AssetDatabase.AssetDatabase.global.onAssetDatabaseChanged.Subscribe((EventDelegate<AssetChangedEventArgs>) (args => GameManager.instance.RunOnMainThread((Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AvailableCloudTargetsBinding.Update(MenuHelpers.GetAvailableCloudTargets());
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedCloudTargetBinding.Update();
        // ISSUE: reference to a compiler-generated method
        this.UpdateMaps(args);
        // ISSUE: reference to a compiler-generated method
        this.UpdateSaves(args);
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
      Colossal.IO.AssetDatabase.AssetDatabase.global.onAssetDatabaseChanged.Subscribe<MapMetadata>((EventDelegate<AssetChangedEventArgs>) (args => GameManager.instance.RunOnMainThread((Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AvailableMapFilters.Update(this.GetAvailableMapFilters());
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_AvailableMapFilters.value.Contains(this.m_SelectedMapFilter.value))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedMapFilter.Update(this.m_AvailableMapFilters.value.Count > 0 ? this.m_AvailableMapFilters.value.First<int>() : -1);
        }
        // ISSUE: reference to a compiler-generated field
        MenuHelpers.UpdateMeta<MapInfo>(this.m_MapsBinding, (Func<Metadata<MapInfo>, bool>) (meta =>
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_AvailableMapFilters.value.Count <= 1 || this.m_SelectedMapFilter.value < 0)
            return true;
          // ISSUE: reference to a compiler-generated method
          bool flag = MenuUISystem.IsDefaultAsset((IAssetData) meta);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return this.m_SelectedMapFilter.value == 0 & flag || this.m_SelectedMapFilter.value == 1 && !flag;
        }));
      }))), AssetChangedEventArgs.Default);
      Colossal.IO.AssetDatabase.AssetDatabase.global.onAssetDatabaseChanged.Subscribe<SaveGameMetadata>((EventDelegate<AssetChangedEventArgs>) (args => GameManager.instance.RunOnMainThread((Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        MenuHelpers.UpdateMeta<SaveInfo>(this.m_SavesBinding);
        // ISSUE: reference to a compiler-generated field
        this.m_CanContinueBinding.Update();
      }))), AssetChangedEventArgs.Default);
      SharedSettings.instance.userState.onSettingsApplied += (OnSettingsAppliedHandler) (setting =>
      {
        if (!(setting is UserState))
          return;
        // ISSUE: reference to a compiler-generated field
        GameManager.instance.RunOnMainThread((Action) (() => this.m_CanContinueBinding.Update()));
      });
      // ISSUE: reference to a compiler-generated field
      this.m_LastSelectedCloudTarget = SharedSettings.instance.userState.lastCloudTarget;
      // ISSUE: reference to a compiler-generated field
      this.m_ModsUI = new PdxModsUI();
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ModsUI.Dispose();
      SharedSettings.instance.userState.onSettingsApplied -= (OnSettingsAppliedHandler) (setting =>
      {
        if (!(setting is UserState))
          return;
        // ISSUE: reference to a compiler-generated field
        GameManager.instance.RunOnMainThread((Action) (() => this.m_CanContinueBinding.Update()));
      });
      Colossal.IO.AssetDatabase.AssetDatabase.global.onAssetDatabaseChanged.Unsubscribe((EventDelegate<AssetChangedEventArgs>) (args => GameManager.instance.RunOnMainThread((Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AvailableCloudTargetsBinding.Update(MenuHelpers.GetAvailableCloudTargets());
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedCloudTargetBinding.Update();
        // ISSUE: reference to a compiler-generated method
        this.UpdateMaps(args);
        // ISSUE: reference to a compiler-generated method
        this.UpdateSaves(args);
      }))));
      Colossal.IO.AssetDatabase.AssetDatabase.global.onAssetDatabaseChanged.Unsubscribe((EventDelegate<AssetChangedEventArgs>) (args => GameManager.instance.RunOnMainThread((Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AvailableMapFilters.Update(this.GetAvailableMapFilters());
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_AvailableMapFilters.value.Contains(this.m_SelectedMapFilter.value))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedMapFilter.Update(this.m_AvailableMapFilters.value.Count > 0 ? this.m_AvailableMapFilters.value.First<int>() : -1);
        }
        // ISSUE: reference to a compiler-generated field
        MenuHelpers.UpdateMeta<MapInfo>(this.m_MapsBinding, (Func<Metadata<MapInfo>, bool>) (meta =>
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_AvailableMapFilters.value.Count <= 1 || this.m_SelectedMapFilter.value < 0)
            return true;
          // ISSUE: reference to a compiler-generated method
          bool flag = MenuUISystem.IsDefaultAsset((IAssetData) meta);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return this.m_SelectedMapFilter.value == 0 & flag || this.m_SelectedMapFilter.value == 1 && !flag;
        }));
      }))));
      Colossal.IO.AssetDatabase.AssetDatabase.global.onAssetDatabaseChanged.Unsubscribe((EventDelegate<AssetChangedEventArgs>) (args => GameManager.instance.RunOnMainThread((Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        MenuHelpers.UpdateMeta<SaveInfo>(this.m_SavesBinding);
        // ISSUE: reference to a compiler-generated field
        this.m_CanContinueBinding.Update();
      }))));
      base.OnDestroy();
    }

    private bool IsModsUIActive() => this.m_ModsUI.isActive;

    private void OnSettingsApplied(Setting setting)
    {
      if (!(setting is UserState))
        return;
      // ISSUE: reference to a compiler-generated field
      GameManager.instance.RunOnMainThread((Action) (() => this.m_CanContinueBinding.Update()));
    }

    private void UpdateClouds(AssetChangedEventArgs args)
    {
      GameManager.instance.RunOnMainThread((Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AvailableCloudTargetsBinding.Update(MenuHelpers.GetAvailableCloudTargets());
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedCloudTargetBinding.Update();
        // ISSUE: reference to a compiler-generated method
        this.UpdateMaps(args);
        // ISSUE: reference to a compiler-generated method
        this.UpdateSaves(args);
      }));
    }

    private void UpdateMaps(AssetChangedEventArgs args)
    {
      GameManager.instance.RunOnMainThread((Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AvailableMapFilters.Update(this.GetAvailableMapFilters());
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_AvailableMapFilters.value.Contains(this.m_SelectedMapFilter.value))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedMapFilter.Update(this.m_AvailableMapFilters.value.Count > 0 ? this.m_AvailableMapFilters.value.First<int>() : -1);
        }
        // ISSUE: reference to a compiler-generated field
        MenuHelpers.UpdateMeta<MapInfo>(this.m_MapsBinding, (Func<Metadata<MapInfo>, bool>) (meta =>
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_AvailableMapFilters.value.Count <= 1 || this.m_SelectedMapFilter.value < 0)
            return true;
          // ISSUE: reference to a compiler-generated method
          bool flag = MenuUISystem.IsDefaultAsset((IAssetData) meta);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return this.m_SelectedMapFilter.value == 0 & flag || this.m_SelectedMapFilter.value == 1 && !flag;
        }));
      }));
    }

    private void UpdateMaps()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AvailableMapFilters.Update(this.GetAvailableMapFilters());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_AvailableMapFilters.value.Contains(this.m_SelectedMapFilter.value))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedMapFilter.Update(this.m_AvailableMapFilters.value.Count > 0 ? this.m_AvailableMapFilters.value.First<int>() : -1);
      }
      // ISSUE: reference to a compiler-generated field
      MenuHelpers.UpdateMeta<MapInfo>(this.m_MapsBinding, (Func<Metadata<MapInfo>, bool>) (meta =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_AvailableMapFilters.value.Count <= 1 || this.m_SelectedMapFilter.value < 0)
          return true;
        // ISSUE: reference to a compiler-generated method
        bool flag = MenuUISystem.IsDefaultAsset((IAssetData) meta);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_SelectedMapFilter.value == 0 & flag || this.m_SelectedMapFilter.value == 1 && !flag;
      }));
    }

    private void UpdateSaves(AssetChangedEventArgs args)
    {
      GameManager.instance.RunOnMainThread((Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        MenuHelpers.UpdateMeta<SaveInfo>(this.m_SavesBinding);
        // ISSUE: reference to a compiler-generated field
        this.m_CanContinueBinding.Update();
      }));
    }

    private void ApplyTutorialSettings(bool showTutorials, bool resetTutorials)
    {
      SharedSettings.instance.gameplay.showTutorials = showTutorials;
      if (!resetTutorials)
        return;
      SharedSettings.instance.userState.ResetTutorials();
    }

    public void PreDeserialize(Context context)
    {
      if (context.purpose != Purpose.Cleanup)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveScreenBinding.Update(0);
    }

    private void OnSaveGameScreenVisibilityChanged(bool visible)
    {
      if (visible)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SavePreviewBinding.Update(string.Format("{0}{1}/{2}?width={3}&height={4}&op={5}&{6}#{7}", (object) "screencapture://", (object) Camera.main.tag.ToLowerInvariant(), (object) "SaveGamePanel", (object) 680, (object) 383, (object) "Screenshot", (object) this.m_PreviewSettings.ToUri(), (object) MenuUISystem.s_PreviewId++));
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SavePreviewBinding.Update((string) null);
      }
    }

    private bool IsModdingEnabled() => !GameManager.instance.configuration.disableModding;

    private bool IsPdxModsUIEnabled()
    {
      return !GameManager.instance.configuration.disablePDXSDK && !GameManager.instance.configuration.disableModding;
    }

    private void ShowModsUI()
    {
      if (!PlatformManager.instance.hasUgcPrivilege)
        return;
      if (Platform.PlayStation.IsPlatformSet(Application.platform))
      {
        // ISSUE: reference to a compiler-generated field
        GameManager.instance.userInterface.paradoxBindings.OnPSModsUIOpened(new Action(this.m_ModsUI.Show));
        // ISSUE: reference to a compiler-generated field
        this.m_ModsUI.platform.onModsUIClosed -= (Action) (async () =>
        {
          // ISSUE: reference to a compiler-generated field
          List<PDX.SDK.Contracts.Service.Mods.Models.Mod> modsInActivePlayset = await this.m_ModsUI.platform.GetModsInActivePlayset();
          if (modsInActivePlayset == null || modsInActivePlayset.Count <= 0)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          GameManager.instance.userInterface.paradoxBindings.OnPSModsUIClosed((Action) null, new Action(this.m_ModsUI.platform.DeactivateActivePlayset), new Action(this.m_ModsUI.Show));
        });
        // ISSUE: reference to a compiler-generated field
        this.m_ModsUI.platform.onModsUIClosed += (Action) (async () =>
        {
          // ISSUE: reference to a compiler-generated field
          List<PDX.SDK.Contracts.Service.Mods.Models.Mod> modsInActivePlayset = await this.m_ModsUI.platform.GetModsInActivePlayset();
          if (modsInActivePlayset == null || modsInActivePlayset.Count <= 0)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          GameManager.instance.userInterface.paradoxBindings.OnPSModsUIClosed((Action) null, new Action(this.m_ModsUI.platform.DeactivateActivePlayset), new Action(this.m_ModsUI.Show));
        });
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ModsUI.Show();
      }
    }

    private async void OnPSModsUIClosed()
    {
      // ISSUE: reference to a compiler-generated field
      List<PDX.SDK.Contracts.Service.Mods.Models.Mod> modsInActivePlayset = await this.m_ModsUI.platform.GetModsInActivePlayset();
      if (modsInActivePlayset == null || modsInActivePlayset.Count <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      GameManager.instance.userInterface.paradoxBindings.OnPSModsUIClosed((Action) null, new Action(this.m_ModsUI.platform.DeactivateActivePlayset), new Action(this.m_ModsUI.Show));
    }

    private List<string> GetCreditFiles()
    {
      return new List<string>()
      {
        "Media/Menu/Credits.md",
        "Media/Menu/Licenses.md"
      };
    }

    private List<MenuUISystem.ThemeInfo> GetThemes()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      return new List<MenuUISystem.ThemeInfo>()
      {
        new MenuUISystem.ThemeInfo()
        {
          id = "European",
          icon = "Media/Game/Themes/European.svg"
        },
        new MenuUISystem.ThemeInfo()
        {
          id = "North American",
          icon = "Media/Game/Themes/North American.svg"
        }
      };
    }

    private void SafeContinueGame()
    {
      TaskManager.instance.EnqueueTask("SaveLoadGame", (Func<Task>) (async () =>
      {
        try
        {
          SaveGameMetadata lastSave = GameManager.instance.settings.userState.lastSaveGameMetadata;
          if ((AssetData) lastSave != (IAssetData) null && lastSave.isValidSaveGame)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_MapMetadataSystem.mapName = lastSave.target.mapName;
            PlatformManager.instance.achievementsEnabled = !lastSave.target.isReadonly;
            int num = await GameManager.instance.Load(GameMode.Game, Purpose.LoadGame, (IAssetData) lastSave) ? 1 : 0;
            SaveInfo target = lastSave.target;
            // ISSUE: reference to a compiler-generated field
            this.m_LastSaveNameBinding.Update(target.autoSave ? (string) null : target.displayName);
            if (!target.autoSave)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LastSelectedCloudTarget = target.metaData.database.dataSource.remoteStorageSourceName;
            }
          }
          lastSave = (SaveGameMetadata) null;
        }
        catch (OperationCanceledException ex)
        {
        }
        catch (Exception ex)
        {
          COSystemBase.baseLog.Error(ex);
        }
      }), 1);
    }

    private async Task ContinueGame()
    {
      try
      {
        SaveGameMetadata lastSave = GameManager.instance.settings.userState.lastSaveGameMetadata;
        if ((AssetData) lastSave != (IAssetData) null && lastSave.isValidSaveGame)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_MapMetadataSystem.mapName = lastSave.target.mapName;
          PlatformManager.instance.achievementsEnabled = !lastSave.target.isReadonly;
          int num = await GameManager.instance.Load(GameMode.Game, Purpose.LoadGame, (IAssetData) lastSave) ? 1 : 0;
          SaveInfo target = lastSave.target;
          // ISSUE: reference to a compiler-generated field
          this.m_LastSaveNameBinding.Update(target.autoSave ? (string) null : target.displayName);
          if (!target.autoSave)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LastSelectedCloudTarget = target.metaData.database.dataSource.remoteStorageSourceName;
          }
        }
        lastSave = (SaveGameMetadata) null;
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.Error(ex);
      }
    }

    private void SafeNewGame(MenuUISystem.NewGameArgs args)
    {
      // ISSUE: reference to a compiler-generated method
      TaskManager.instance.EnqueueTask("SaveLoadGame", (Func<Task>) (() => this.NewGame(args)), 1);
    }

    private async Task NewGame(MenuUISystem.NewGameArgs args)
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        MapInfo mapInfo = this.m_MapsBinding.value.Find((Predicate<MapInfo>) (x => x.id == args.mapId));
        // ISSUE: reference to a compiler-generated field
        this.m_MapMetadataSystem.mapName = mapInfo.displayName;
        // ISSUE: reference to a compiler-generated field
        this.m_CityConfigurationSystem.overrideLoadedOptions = true;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CityConfigurationSystem.overrideThemeName = args.theme;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.ApplyOptions(args.cityName, args.options);
        PlatformManager.instance.achievementsEnabled = true;
        // ISSUE: reference to a compiler-generated field
        this.m_TimeSystem.startingYear = mapInfo.startingYear != -1 ? mapInfo.startingYear : DateTime.Now.Year;
        int num = await GameManager.instance.Load(GameMode.Game, Purpose.NewGame, (IAssetData) mapInfo.metaData) ? 1 : 0;
        // ISSUE: reference to a compiler-generated field
        this.m_LastSaveNameBinding.Update((string) null);
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.Error(ex);
      }
    }

    private void SafeLoadGame(MenuUISystem.LoadGameArgs args, bool dismiss)
    {
      // ISSUE: reference to a compiler-generated method
      TaskManager.instance.EnqueueTask("SaveLoadGame", (Func<Task>) (() => this.LoadGame(args, dismiss)), 1);
    }

    private async Task LoadGame(MenuUISystem.LoadGameArgs args, bool dismiss)
    {
      try
      {
        if (dismiss)
        {
          SharedSettings.instance.userInterface.dismissedConfirmations.Add(nameof (LoadGame));
          SharedSettings.instance.userInterface.Apply();
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        SaveInfo saveInfo = this.m_SavesBinding.value.Find((Predicate<SaveInfo>) (x => x.id == args.saveId));
        // ISSUE: reference to a compiler-generated field
        this.m_MapMetadataSystem.mapName = saveInfo.mapName;
        // ISSUE: reference to a compiler-generated field
        this.m_CityConfigurationSystem.overrideLoadedOptions = true;
        // ISSUE: reference to a compiler-generated field
        this.m_CityConfigurationSystem.overrideThemeName = (string) null;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.ApplyOptions(args.cityName, args.options);
        PlatformManager.instance.achievementsEnabled = !saveInfo.isReadonly;
        int num = await GameManager.instance.Load(GameMode.Game, Purpose.LoadGame, (IAssetData) saveInfo.metaData) ? 1 : 0;
        // ISSUE: reference to a compiler-generated field
        this.m_LastSaveNameBinding.Update(saveInfo.autoSave ? (string) null : saveInfo.displayName);
        if (!saveInfo.autoSave)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LastSelectedCloudTarget = saveInfo.metaData.database.dataSource.remoteStorageSourceName;
        }
        saveInfo = (SaveInfo) null;
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.Error(ex);
      }
    }

    private void ApplyOptions(string cityName, System.Collections.Generic.Dictionary<string, bool> options)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem.overrideCityName = cityName;
      if (options == null)
        return;
      UserState userState = SharedSettings.instance.userState;
      bool flag1;
      if (options.TryGetValue("leftHandTraffic", out flag1))
      {
        // ISSUE: reference to a compiler-generated field
        userState.leftHandTraffic = this.m_CityConfigurationSystem.overrideLeftHandTraffic = flag1;
      }
      bool flag2;
      if (options.TryGetValue("naturalDisasters", out flag2))
      {
        // ISSUE: reference to a compiler-generated field
        userState.naturalDisasters = this.m_CityConfigurationSystem.overrideNaturalDisasters = flag2;
      }
      bool flag3;
      if (options.TryGetValue("unlockAll", out flag3))
      {
        // ISSUE: reference to a compiler-generated field
        userState.unlockAll = this.m_CityConfigurationSystem.overrideUnlockAll = flag3;
      }
      bool flag4;
      if (options.TryGetValue("unlimitedMoney", out flag4))
      {
        // ISSUE: reference to a compiler-generated field
        userState.unlimitedMoney = this.m_CityConfigurationSystem.overrideUnlimitedMoney = flag4;
      }
      bool flag5;
      if (options.TryGetValue("unlockMapTiles", out flag5))
      {
        // ISSUE: reference to a compiler-generated field
        userState.unlockMapTiles = this.m_CityConfigurationSystem.overrideUnlockMapTiles = flag5;
      }
      userState.ApplyAndSave();
    }

    public SaveInfo GetSaveInfo(bool autoSave)
    {
      // ISSUE: variable of a compiler-generated type
      CitySystem existingSystemManaged = this.World.GetExistingSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated method
      DateTime currentDateTime = this.World.GetExistingSystemManaged<TimeSystem>().GetCurrentDateTime();
      Population componentData = this.EntityManager.GetComponentData<Population>(existingSystemManaged.City);
      // ISSUE: reference to a compiler-generated field
      this.m_MapMetadataSystem.Update();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return new SaveInfo()
      {
        theme = this.m_MapMetadataSystem.theme,
        cityName = this.m_CityConfigurationSystem.cityName,
        population = componentData.m_Population,
        money = existingSystemManaged.moneyAmount,
        xp = existingSystemManaged.XP,
        simulationDate = new SimulationDateTime(currentDateTime.Year, currentDateTime.DayOfYear - 1, currentDateTime.Hour, currentDateTime.Minute),
        options = new System.Collections.Generic.Dictionary<string, bool>()
        {
          {
            "leftHandTraffic",
            this.m_CityConfigurationSystem.leftHandTraffic
          },
          {
            "naturalDisasters",
            this.m_CityConfigurationSystem.naturalDisasters
          },
          {
            "unlockAll",
            this.m_CityConfigurationSystem.unlockAll
          },
          {
            "unlimitedMoney",
            this.m_CityConfigurationSystem.unlimitedMoney
          },
          {
            "unlockMapTiles",
            this.m_CityConfigurationSystem.unlockMapTiles
          }
        },
        mapName = this.m_MapMetadataSystem.mapName,
        autoSave = autoSave,
        modsEnabled = this.m_CityConfigurationSystem.usedMods.ToArray<string>()
      };
    }

    private void SafeSaveGame(string saveName)
    {
      // ISSUE: reference to a compiler-generated method
      TaskManager.instance.EnqueueTask("SaveLoadGame", (Func<Task>) (() => this.SaveGame(saveName)), 1);
    }

    private async Task SaveGame(string saveName)
    {
      try
      {
        Texture savePreview = UIManager.defaultUISystem.userImagesManager.GetUserImageTarget("SaveGamePanel", 680, 383);
        ILocalAssetDatabase targetDatabase = MenuHelpers.GetSanitizedCloudTarget(SharedSettings.instance.userState.lastCloudTarget).db;
        // ISSUE: reference to a compiler-generated method
        SaveInfo saveInfo = this.GetSaveInfo(false);
        // ISSUE: reference to a compiler-generated method
        if (await this.HandlesOverwrite(targetDatabase, saveName))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_GameScreenUISystem.SetScreen(10);
          int num = await GameManager.instance.Save(saveName, saveInfo, targetDatabase, savePreview) ? 1 : 0;
          // ISSUE: reference to a compiler-generated field
          this.m_LastSaveNameBinding.Update(saveName);
          // ISSUE: reference to a compiler-generated field
          this.m_LastSelectedCloudTarget = saveInfo.metaData.database.dataSource.remoteStorageSourceName;
        }
        savePreview = (Texture) null;
        targetDatabase = (ILocalAssetDatabase) null;
        saveInfo = (SaveInfo) null;
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.Error(ex);
      }
    }

    private bool SaveExists(ILocalAssetDatabase database, string name, out PackageAsset asset)
    {
      return database.Exists<PackageAsset>(SaveHelpers.GetAssetDataPath<SaveGameMetadata>(database, name), out asset);
    }

    private Task<bool> HandlesOverwrite(ILocalAssetDatabase database, string saveName)
    {
      InterfaceSettings userInterface = SharedSettings.instance.userInterface;
      PackageAsset asset;
      // ISSUE: reference to a compiler-generated method
      if (this.SaveExists(database, saveName, out asset))
      {
        if (!userInterface.dismissedConfirmations.Contains("SaveGame"))
        {
          TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
          GameManager.instance.RegisterCancellationOnQuit(tcs, false);
          GameManager.instance.userInterface.appBindings.ShowConfirmationDialog(new DismissibleConfirmationDialog(new LocalizedString?((LocalizedString) "Common.DIALOG_TITLE[Warning]"), (LocalizedString) "Common.DIALOG_MESSAGE[Overwrite]", (LocalizedString) "Common.DIALOG_ACTION[Yes]", new LocalizedString?((LocalizedString) "Common.DIALOG_ACTION[No]"), Array.Empty<LocalizedString>()), (Action<int, bool>) ((msg, dismiss) =>
          {
            if (msg == 0)
            {
              if (dismiss)
              {
                SharedSettings.instance.userInterface.dismissedConfirmations.Add("SaveGame");
                SharedSettings.instance.userInterface.ApplyAndSave();
              }
              database.DeleteAsset<PackageAsset>(asset);
            }
            tcs.SetResult(msg == 0);
          }));
          return tcs.Task;
        }
        database.DeleteAsset<PackageAsset>(asset);
      }
      return Task.FromResult<bool>(true);
    }

    private void SafeQuickSave()
    {
      TaskManager.instance.EnqueueTask("SaveLoadGame", (Func<Task>) (async () =>
      {
        try
        {
          // ISSUE: reference to a compiler-generated field
          string saveName = this.m_LastSaveNameBinding.value;
          if (string.IsNullOrEmpty(saveName))
          {
            // ISSUE: reference to a compiler-generated field
            saveName = this.m_CityConfigurationSystem.cityName;
          }
          if (string.IsNullOrEmpty(saveName))
            saveName = "SaveGame";
          // ISSUE: reference to a compiler-generated field
          ILocalAssetDatabase targetDatabase = MenuHelpers.GetSanitizedCloudTarget(this.m_LastSelectedCloudTarget).db;
          if (targetDatabase.name != null)
          {
            RenderTexture savePreview = ScreenCaptureHelper.CreateRenderTarget("SaveGamePanel", 680, 383);
            // ISSUE: reference to a compiler-generated field
            ScreenCaptureHelper.CaptureScreenshot(Camera.main, savePreview, this.m_PreviewSettings);
            // ISSUE: reference to a compiler-generated method
            SaveInfo saveInfo = this.GetSaveInfo(false);
            // ISSUE: reference to a compiler-generated method
            if (await this.HandlesOverwrite(targetDatabase, saveName))
            {
              int num = await GameManager.instance.Save(saveName, saveInfo, targetDatabase, (Texture) savePreview) ? 1 : 0;
            }
            CoreUtils.Destroy((UnityEngine.Object) savePreview);
            savePreview = (RenderTexture) null;
            saveInfo = (SaveInfo) null;
          }
          saveName = (string) null;
          targetDatabase = (ILocalAssetDatabase) null;
        }
        catch (OperationCanceledException ex)
        {
        }
        catch (Exception ex)
        {
          COSystemBase.baseLog.Error(ex);
        }
      }), 1);
    }

    private async Task QuickSave()
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        string saveName = this.m_LastSaveNameBinding.value;
        if (string.IsNullOrEmpty(saveName))
        {
          // ISSUE: reference to a compiler-generated field
          saveName = this.m_CityConfigurationSystem.cityName;
        }
        if (string.IsNullOrEmpty(saveName))
          saveName = "SaveGame";
        // ISSUE: reference to a compiler-generated field
        ILocalAssetDatabase targetDatabase = MenuHelpers.GetSanitizedCloudTarget(this.m_LastSelectedCloudTarget).db;
        if (targetDatabase.name != null)
        {
          RenderTexture savePreview = ScreenCaptureHelper.CreateRenderTarget("SaveGamePanel", 680, 383);
          // ISSUE: reference to a compiler-generated field
          ScreenCaptureHelper.CaptureScreenshot(Camera.main, savePreview, this.m_PreviewSettings);
          // ISSUE: reference to a compiler-generated method
          SaveInfo saveInfo = this.GetSaveInfo(false);
          // ISSUE: reference to a compiler-generated method
          if (await this.HandlesOverwrite(targetDatabase, saveName))
          {
            int num = await GameManager.instance.Save(saveName, saveInfo, targetDatabase, (Texture) savePreview) ? 1 : 0;
          }
          CoreUtils.Destroy((UnityEngine.Object) savePreview);
          savePreview = (RenderTexture) null;
          saveInfo = (SaveInfo) null;
        }
        saveName = (string) null;
        targetDatabase = (ILocalAssetDatabase) null;
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.Error(ex);
      }
    }

    private void SafeQuickLoad(bool dismiss)
    {
      // ISSUE: reference to a compiler-generated method
      TaskManager.instance.EnqueueTask("SaveLoadGame", (Func<Task>) (() => this.QuickLoad(dismiss)), 1);
    }

    private async Task QuickLoad(bool dismiss)
    {
      try
      {
        if (dismiss)
        {
          SharedSettings.instance.userInterface.dismissedConfirmations.Add("LoadGame");
          SharedSettings.instance.userInterface.Apply();
        }
        if (!MenuHelpers.hasPreviouslySavedGame)
          return;
        // ISSUE: reference to a compiler-generated method
        await this.ContinueGame();
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.Error(ex);
      }
    }

    public void DeleteSave(string guid)
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        SaveHelpers.DeleteSaveGame(this.m_SavesBinding.value.Find((Predicate<SaveInfo>) (x => x.id == guid)).metaData);
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.Error(ex);
      }
    }

    public void ShareSave(string id)
    {
      // ISSUE: reference to a compiler-generated field
      foreach (SaveInfo saveInfo in this.m_SavesBinding.value)
      {
        if (saveInfo.id == id)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_AssetUploadPanelUISystem.Show((AssetData) saveInfo.metaData);
          break;
        }
      }
    }

    public void ShareMap(string id)
    {
      // ISSUE: reference to a compiler-generated field
      foreach (MapInfo mapInfo in this.m_MapsBinding.value)
      {
        if (mapInfo.id == id)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_AssetUploadPanelUISystem.Show((AssetData) mapInfo.metaData);
          break;
        }
      }
    }

    private async void StartEditor()
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CityConfigurationSystem.overrideLoadedOptions = false;
        // ISSUE: reference to a compiler-generated field
        this.m_CityConfigurationSystem.overrideThemeName = (string) null;
        int num = await GameManager.instance.Load(GameMode.Editor, Purpose.NewMap).ConfigureAwait(false) ? 1 : 0;
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.Error(ex);
      }
    }

    private async void ExitToMainMenu()
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CityConfigurationSystem.overrideLoadedOptions = false;
        // ISSUE: reference to a compiler-generated field
        this.m_CityConfigurationSystem.overrideThemeName = (string) null;
        int num = await GameManager.instance.MainMenu() ? 1 : 0;
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.Error(ex);
      }
    }

    private void SelectCloudTarget(string cloudTarget)
    {
      SharedSettings.instance.userState.lastCloudTarget = cloudTarget;
      Debug.Log((object) ("Max file length " + MenuHelpers.GetSanitizedCloudTarget(cloudTarget).db.dataSource.maxSupportedFileLength.ToString()));
    }

    public void OpenScreen(MenuUISystem.MenuScreen screen)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveScreenBinding.Update((int) screen);
    }

    private MenuUISystem.SaveabilityStatus GetSaveabilityStatus()
    {
      int count = MenuHelpers.GetAvailableCloudTargets().Count;
      // ISSUE: object of a compiler-generated type is created
      return new MenuUISystem.SaveabilityStatus()
      {
        canSave = count > 0,
        reasonHash = count > 0 ? (string) null : "NoLocations"
      };
    }

    private static bool IsDefaultAsset(IAssetData asset) => asset.database == Colossal.IO.AssetDatabase.AssetDatabase.game;

    private HashSet<int> GetAvailableMapFilters()
    {
      HashSet<int> availableMapFilters = new HashSet<int>(2);
      foreach (Metadata<MapInfo> asset in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<Metadata<MapInfo>>(new SearchFilter<Metadata<MapInfo>>()))
      {
        // ISSUE: reference to a compiler-generated method
        availableMapFilters.Add(MenuUISystem.IsDefaultAsset((IAssetData) asset) ? 0 : 1);
      }
      return availableMapFilters;
    }

    private void OnSelectMapFilter(int tab)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedMapFilter.Update(tab);
      // ISSUE: reference to a compiler-generated method
      this.UpdateMaps();
    }

    private bool FilterMaps(Metadata<MapInfo> meta)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_AvailableMapFilters.value.Count <= 1 || this.m_SelectedMapFilter.value < 0)
        return true;
      // ISSUE: reference to a compiler-generated method
      bool flag = MenuUISystem.IsDefaultAsset((IAssetData) meta);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_SelectedMapFilter.value == 0 & flag || this.m_SelectedMapFilter.value == 1 && !flag;
    }

    [Preserve]
    public MenuUISystem()
    {
    }

    private enum MapFilter
    {
      None = -1, // 0xFFFFFFFF
      Default = 0,
      Custom = 1,
    }

    public enum MenuScreen
    {
      Menu,
      NewGame,
      LoadGame,
      Options,
      Credits,
    }

    public class ThemeInfo : IJsonWritable
    {
      public string id { get; set; }

      public string icon { get; set; }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin("menu.ThemeInfo");
        writer.PropertyName("id");
        writer.Write(this.id);
        writer.PropertyName("icon");
        writer.Write(this.icon);
        writer.TypeEnd();
      }
    }

    public struct NewGameArgs : IJsonReadable
    {
      public string mapId;
      public string cityName;
      public string theme;
      public System.Collections.Generic.Dictionary<string, bool> options;

      public void Read(IJsonReader reader)
      {
        long num = (long) reader.ReadMapBegin();
        reader.ReadProperty("mapId");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.mapId);
        reader.ReadProperty("cityName");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.cityName);
        reader.ReadProperty("theme");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.theme);
        reader.ReadProperty("options");
        ulong capacity = reader.ReadMapBegin();
        // ISSUE: reference to a compiler-generated field
        this.options = new System.Collections.Generic.Dictionary<string, bool>((int) capacity);
        for (ulong index = 0; index < capacity; ++index)
        {
          reader.ReadMapKeyValue();
          string key;
          reader.Read(out key);
          bool flag;
          reader.Read(out flag);
          // ISSUE: reference to a compiler-generated field
          this.options.Add(key, flag);
        }
        reader.ReadMapEnd();
        reader.ReadMapEnd();
      }
    }

    public struct LoadGameArgs : IJsonReadable
    {
      public string saveId;
      public string cityName;
      public System.Collections.Generic.Dictionary<string, bool> options;

      public void Read(IJsonReader reader)
      {
        long num = (long) reader.ReadMapBegin();
        reader.ReadProperty("saveId");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.saveId);
        reader.ReadProperty("cityName");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.cityName);
        reader.ReadProperty("options");
        ulong capacity = reader.ReadMapBegin();
        // ISSUE: reference to a compiler-generated field
        this.options = new System.Collections.Generic.Dictionary<string, bool>((int) capacity);
        for (ulong index = 0; index < capacity; ++index)
        {
          reader.ReadMapKeyValue();
          string key;
          reader.Read(out key);
          bool flag;
          reader.Read(out flag);
          // ISSUE: reference to a compiler-generated field
          this.options.Add(key, flag);
        }
        reader.ReadMapEnd();
        reader.ReadMapEnd();
      }
    }

    private class DefaultGameOptions : IJsonWritable
    {
      public bool leftHandTraffic => SharedSettings.instance.userState.leftHandTraffic;

      public bool naturalDisasters => SharedSettings.instance.userState.naturalDisasters;

      public bool unlockAll => SharedSettings.instance.userState.unlockAll;

      public bool unlimitedMoney => SharedSettings.instance.userState.unlimitedMoney;

      public bool unlockMapTiles => SharedSettings.instance.userState.unlockMapTiles;

      public void Write(IJsonWriter writer)
      {
        writer.MapBegin(5U);
        writer.Write("leftHandTraffic");
        writer.Write(this.leftHandTraffic);
        writer.Write("naturalDisasters");
        writer.Write(this.naturalDisasters);
        writer.Write("unlockAll");
        writer.Write(this.unlockAll);
        writer.Write("unlimitedMoney");
        writer.Write(this.unlimitedMoney);
        writer.Write("unlockMapTiles");
        writer.Write(this.unlockMapTiles);
        writer.MapEnd();
      }

      public System.Collections.Generic.Dictionary<string, bool> MergeOptions(
        System.Collections.Generic.Dictionary<string, bool> options)
      {
        System.Collections.Generic.Dictionary<string, bool> dictionary = new System.Collections.Generic.Dictionary<string, bool>();
        dictionary["leftHandTraffic"] = this.leftHandTraffic;
        dictionary["naturalDisasters"] = this.naturalDisasters;
        dictionary["unlockAll"] = this.unlockAll;
        dictionary["unlimitedMoney"] = this.unlimitedMoney;
        dictionary["unlockMapTiles"] = this.unlockMapTiles;
        if (options != null)
        {
          foreach (KeyValuePair<string, bool> option in options)
            dictionary[option.Key] = option.Value;
        }
        return dictionary;
      }
    }

    public struct SaveabilityStatus : IJsonWritable
    {
      public bool canSave;
      public string reasonHash;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(nameof (SaveabilityStatus));
        writer.PropertyName("canSave");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.canSave);
        writer.PropertyName("reasonHash");
        // ISSUE: reference to a compiler-generated field
        if (this.canSave)
        {
          writer.WriteNull();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          writer.Write(this.reasonHash);
        }
        writer.TypeEnd();
      }
    }
  }
}
