// Decompiled with JetBrains decompiler
// Type: Game.SceneFlow.GameManager
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using cohtml.Net;
using Colossal;
using Colossal.AssetPipeline.Importers;
using Colossal.FileSystem;
using Colossal.IO.AssetDatabase;
using Colossal.IO.AssetDatabase.VirtualTexturing;
using Colossal.Json;
using Colossal.Localization;
using Colossal.Logging;
using Colossal.Logging.Backtrace;
using Colossal.PSI.Common;
using Colossal.PSI.Environment;
using Colossal.PSI.PdxSdk;
using Colossal.Reflection;
using Colossal.UI;
using Game.Assets;
using Game.Audio;
using Game.Common;
using Game.Debug;
using Game.Input;
using Game.Modding;
using Game.Prefabs;
using Game.PSI;
using Game.PSI.PdxSdk;
using Game.Rendering;
using Game.Serialization;
using Game.Settings;
using Game.UI;
using Game.UI.Localization;
using Game.UI.Menu;
using Game.UI.Thumbnails;
using Mono.Options;
using PDX.SDK;
using PDX.SDK.Contracts.Service.Mods.Enums;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering.HighDefinition;

#nullable disable
namespace Game.SceneFlow
{
  public class GameManager : MonoBehaviour
  {
    private GameManager.Configuration m_Configuration;
    [SerializeField]
    private string m_AdditionalCommandLineToggles;
    private static ILog log;
    private InputManager m_InputManager;
    private ModManager m_ModManager;
    private CancellationTokenSource m_Cts;
    private readonly CancellationTokenSource m_QuitRequested = new CancellationTokenSource();
    private readonly TaskCompletionSource<bool> m_WorldReadySource = new TaskCompletionSource<bool>();
    public GameObject[] m_SettingsDependantObjects;
    private int m_MainThreadId;
    private GameManager.State m_State;
    private OverlayScreen m_InitialEngagementScreen = OverlayScreen.Engagement;
    private bool m_IsEngagementStarted;
    public const string kInMainMenuState = "#StatusInMainMenu";
    public const string kInGameState = "#StatusInGame";
    public const string kInEditorState = "#StatusInEditor";
    [SerializeField]
    private string m_UILocation;
    private UIManager m_UIManager;
    private UIInputSystem m_UIInputSystem;
    private readonly ConcurrentDictionary<Guid, Func<bool>> m_Updaters = new ConcurrentDictionary<Guid, Func<bool>>();
    private const string kBootTask = "Boot";
    private LayerMask m_DefaultCullingMask;
    private LayerMask m_DefaultVolumeLayerMask;
    private ConsoleWindow m_Console;
    private static string s_ModdingRuntime;
    private World m_World;
    private UpdateSystem m_UpdateSystem;
    private LoadGameSystem m_DeserializationSystem;
    private SaveGameSystem m_SerializationSystem;
    private PrefabSystem m_PrefabSystem;

    public bool isLoading => false;

    public Stage GetCurrentStage() => Stage.None;

    public Stage GetStageLoadingInProgress() => Stage.None;

    public void LoadMainMenuStage()
    {
    }

    private void OnGUI()
    {
      if (!this.shouldUpdateWorld || this.m_Cts.IsCancellationRequested)
        return;
      TerrainDebugSystem systemManaged = this.m_World.GetOrCreateSystemManaged<TerrainDebugSystem>();
      if (!systemManaged.Enabled)
        return;
      systemManaged.RenderDebugUI();
    }

    public string[] cmdLine { get; private set; }

    public GameManager.Configuration configuration
    {
      get
      {
        if (this.m_Configuration == null)
          this.m_Configuration = new GameManager.Configuration();
        return this.m_Configuration;
      }
    }

    private static string[] MergeAdditionalCommandLineArguments(
      string[] cmdLineArgs,
      string additionalCmdLine)
    {
      HashSet<string> stringSet;
      if (!string.IsNullOrEmpty(additionalCmdLine))
        stringSet = ((IEnumerable<string>) additionalCmdLine.Split(new char[1]
        {
          ' '
        }, StringSplitOptions.RemoveEmptyEntries)).ToHashSet<string>();
      else
        stringSet = new HashSet<string>();
      if (stringSet.Count <= 0)
        return cmdLineArgs;
      string[] array = new string[cmdLineArgs.Length + stringSet.Count];
      cmdLineArgs.CopyTo((System.Array) array, 0);
      stringSet.CopyTo(array, cmdLineArgs.Length);
      return array;
    }

    private GameManager.Configuration.StdoutCaptureMode GetStdoutCaptureMode(string option)
    {
      switch (option)
      {
        case "console":
          return GameManager.Configuration.StdoutCaptureMode.Console;
        case "capture":
          return GameManager.Configuration.StdoutCaptureMode.CaptureOnly;
        case "redirect":
          return GameManager.Configuration.StdoutCaptureMode.Redirect;
        default:
          return GameManager.Configuration.StdoutCaptureMode.None;
      }
    }

    private void ParseOptions()
    {
      OptionSet optionSet = new OptionSet()
      {
        {
          "cleanupSettings",
          "Cleanup unchanged settings",
          (Action<string>) (option => this.configuration.cleanupSettings = option != null)
        }
      }.Add("saveAllSettings", "Dump all settings regardless if they have changed", (Action<string>) (option => this.configuration.saveAllSettings = option != null)).Add("logsEffectiveness=", "Override effectiveness level of all logs", (Action<string>) (option => LogManager.SetDefaultEffectiveness(Colossal.Logging.Level.GetLevel(option)))).Add("duplicateLogToDefault", "Duplicate logs to default log handler", (Action<string>) (option => this.configuration.duplicateLogToDefault = option != null)).Add("developerMode", "Enable developer mode", (Action<string>) (option => this.configuration.developerMode = option != null)).Add("uiDeveloperMode", "Enable UI debugger and memory tracker", (Action<string>) (option => this.configuration.uiDeveloperMode = option != null)).Add("qaDeveloperMode", "Enable tests and automation", (Action<string>) (option => this.configuration.qaDeveloperMode = option != null)).Add("help", "Display usage", (Action<string>) (option => this.configuration.showHelp = option)).Add("disableAssets", "Disables assets", (Action<string>) (option => this.configuration.noAssets = option != null)).Add("disableThumbnails", "Disable thumbnails", (Action<string>) (option => this.configuration.noThumbnails = option != null)).Add("disablePdxSdk", "Disables PDX SDK integration", (Action<string>) (option => this.configuration.disablePDXSDK = option != null)).Add("disableModding", "Disable modding", (Action<string>) (option =>
      {
        this.configuration.disableModding = option != null;
        this.configuration.disableCodeModding = this.configuration.disableModding;
      })).Add("disableCodeModding", "Disable code modding", (Action<string>) (option => this.configuration.disableCodeModding = option != null)).Add("disableUserSection", "Disable user section in main menu", (Action<string>) (option => this.configuration.disableUserSection = option != null)).Add("startGame=", "Auto start the game with the asset referenced", (Action<string>) (option => this.configuration.startGame = Colossal.Hash128.Parse(option))).Add("profile=", "Enable profiling to the specific file", (Action<string>) (option => this.configuration.profilerTarget = option)).Add("captureStdout=", "Capture all logs on stdout. Options: \"console\",\"capture\"", (Action<string>) (option => this.configuration.captureStdout = this.GetStdoutCaptureMode(option))).Add("continuelastsave", "Auto start the game with the asset referenced", (Action<string>) (option => this.configuration.continuelastsave = option != null));
      try
      {
        this.cmdLine = System.Environment.GetCommandLineArgs();
        this.cmdLine = GameManager.MergeAdditionalCommandLineArguments(this.cmdLine, this.m_AdditionalCommandLineToggles);
        optionSet.Parse((IEnumerable<string>) this.cmdLine);
        GameManager.log.InfoFormat("Command line: {0}", (object) string.Join("\n", GameManager.MaskArguments(this.cmdLine)));
        if (this.configuration.showHelp == null)
          return;
        using (TextWriter o = (TextWriter) new StringWriter())
        {
          optionSet.WriteOptionDescriptions(o);
          this.configuration.showHelp = o.ToString();
        }
      }
      catch (OptionException ex)
      {
        UnityEngine.Debug.LogException((Exception) ex);
      }
    }

    private static string[] MaskArguments(string[] cmdLine)
    {
      try
      {
        HashSet<string> stringSet = new HashSet<string>()
        {
          "pdx-launcher-session-token",
          "paradox-account-userid",
          "accessToken",
          "hubSessionId",
          "licensingIpc"
        };
        string[] strArray = (string[]) cmdLine.Clone();
        for (int index = 0; index < strArray.Length; ++index)
        {
          string str1 = strArray[index].TrimStart('-');
          int length = str1.IndexOf('=');
          if (length != -1)
          {
            string input = str1.Substring(length + 1);
            string str2 = str1.Substring(0, length);
            strArray[index] = str2 + "=" + input.Sensitive();
          }
          else if (stringSet.Contains(str1) && index + 1 < strArray.Length)
            strArray[index + 1] = strArray[index + 1].Sensitive();
        }
        return strArray;
      }
      catch (Exception ex)
      {
        GameManager.log.Warn(ex, (object) "An error occured parsing the command line for logging");
        return System.Array.Empty<string>();
      }
    }

    public static GameManager instance { get; private set; }

    public bool isMainThread => Thread.CurrentThread.ManagedThreadId == this.m_MainThreadId;

    public GameMode gameMode { get; private set; } = GameMode.Other;

    public bool isGameLoading
    {
      get => this.state == GameManager.State.Booting || this.state == GameManager.State.Loading;
    }

    public SharedSettings settings { get; private set; }

    public InputManager inputManager => this.m_InputManager;

    public ModManager modManager => this.m_ModManager;

    public CancellationToken terminationToken
    {
      get => this.m_Cts == null ? CancellationToken.None : this.m_Cts.Token;
    }

    public async void RegisterCancellationOnQuit(TaskCompletionSource<bool> tcs, bool stateOnCancel)
    {
      await using (this.m_QuitRequested.Token.Register((System.Action) (() => tcs.TrySetResult(stateOnCancel))))
      {
        int num = await tcs.Task ? 1 : 0;
      }
    }

    public GameManager.State state => this.m_State;

    public bool shouldUpdateManager => this.m_State >= GameManager.State.UIReady;

    public bool shouldUpdateWorld => this.m_State >= GameManager.State.WorldReady;

    [RuntimeInitializeOnLoadMethod]
    private static void SetupCustomAssetTypes()
    {
      System.Environment.SetEnvironmentVariable("UNITY_EXT_LOGGING", "1", EnvironmentVariableTarget.Process);
      DefaultAssetFactory.instance.AddSupportedType<SaveGameMetadata>(".SaveGameMetadata", (Func<SaveGameMetadata>) (() => new SaveGameMetadata()));
      DefaultAssetFactory.instance.AddSupportedType<MapMetadata>(".MapMetadata", (Func<MapMetadata>) (() => new MapMetadata()));
      DefaultAssetFactory.instance.AddSupportedType<CinematicCameraAsset>(".CinematicCamera", (Func<CinematicCameraAsset>) (() => new CinematicCameraAsset()));
    }

    private async void Awake()
    {
      GameManager gameManager = this;
      try
      {
        if (!gameManager.CheckValidity())
          return;
        using (PerformanceCounter.Start((Action<TimeSpan>) (t => GameManager.log?.InfoFormat("GameManager created! ({0}ms)", (object) t.TotalMilliseconds))))
        {
          Task checkCapabilities = gameManager.CheckCapabilities();
          GameManager.DetectModdingRuntime();
          BacktraceHelper.SetDefaultAttributes(GameManager.GetDefaultBacktraceAttributes());
          gameManager.EnableMemoryLeaksDetection();
          Application.wantsToQuit += new Func<bool>(gameManager.WantsToQuit);
          gameManager.m_MainThreadId = Thread.CurrentThread.ManagedThreadId;
          gameManager.m_State = GameManager.State.Booting;
          Application.focusChanged += new Action<bool>(gameManager.FocusChanged);
          GameManager.SetNativeStackTrace();
          gameManager.m_Cts = new CancellationTokenSource();
          CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
          LogManager.SetDefaultEffectiveness(Colossal.Logging.Level.Info);
          BacktraceHelper.SetDefaultAttributes(GameManager.GetDefaultBacktraceAttributes());
          GameManager.log = LogManager.GetLogger("SceneFlow");
          gameManager.TryCatchUnhandledExceptions();
          gameManager.ParseOptions();
          gameManager.InitConsole();
          if (!gameManager.HandleConfiguration())
          {
            GameManager.QuitGame();
            return;
          }
          gameManager.DisableCameraRendering();
          await gameManager.PreparePersistentStorage();
          gameManager.HandleUserFolderVersion();
          await checkCapabilities;
          GameManager.instance = gameManager;
          gameManager.Initialize();
          checkCapabilities = (Task) null;
        }
      }
      catch (Exception ex)
      {
        GameManager.log.Fatal(ex);
        GameManager.QuitGame();
      }
    }

    private Task CheckCapabilities() => Capabilities.CacheCapabilities();

    private async void Initialize()
    {
      GameManager gameManager = this;
      try
      {
        using (PerformanceCounter.Start((Action<TimeSpan>) (t => GameManager.log?.InfoFormat("GameManager initialized! ({0}ms)", (object) t.TotalMilliseconds))))
        {
          GameManager.ListHarmonyPatches();
          TaskManager taskManager = TaskManager.instance;
          gameManager.m_InputManager = new InputManager();
          gameManager.m_InputManager.Initialize();
          Colossal.IO.AssetDatabase.AssetDatabase.global.SetSettingsConfiguration(gameManager.configuration.saveAllSettings, gameManager.configuration.cleanupSettings);
          await gameManager.InitializePlatformManager();
          // ISSUE: reference to a compiler-generated method
          await taskManager.SharedTask("CacheAssets", new Func<Task>(gameManager.\u003CInitialize\u003Eb__61_1));
          // ISSUE: reference to a compiler-generated method
          Task caching = taskManager.SharedTask("CacheAssets", new Func<Task>(gameManager.\u003CInitialize\u003Eb__61_2));
          gameManager.InitializeLocalization();
          gameManager.settings = new SharedSettings(gameManager.localizationManager);
          gameManager.CreateWorld();
          gameManager.m_InputManager.SetDefaultControlScheme();
          await gameManager.InitializeUI();
          TaskManager.instance.onNotifyProgress += new OnNotifyProgress(gameManager.NotifyProgress);
          gameManager.ReportBootProgress(0.0f);
          Task engagement = gameManager.SetInitialEngagementScreenActive();
          Task loading = gameManager.SetScreenActive<LoadingScreen>();
          await gameManager.SetScreenActive<SplashScreenSequence>();
          Task assetLoading = gameManager.LoadUnityPrefabs();
          GameManager.log.Info((object) GameManager.GetVersionsInfo());
          GameManager.log.Info((object) GameManager.GetSystemInfoString());
          gameManager.configuration.LogConfiguration();
          await engagement;
          gameManager.RegisterDeviceAndUserListeners();
          gameManager.m_ModManager = new ModManager(gameManager.configuration.disableCodeModding);
          await caching;
          await gameManager.RegisterPdxSdk();
          gameManager.ReportBootProgress(0.3f);
          gameManager.settings.LoadUserSettings();
          gameManager.ReportBootProgress(0.5f);
          gameManager.CreateSystems();
          gameManager.InitializeModManager();
          gameManager.settings.Apply();
          gameManager.EnableSettingsDependantObjects();
          await assetLoading;
          gameManager.ReportBootProgress(0.8f);
          gameManager.LoadPrefabs();
          gameManager.InitializeThumbnails();
          if (!gameManager.configuration.startGame.isValid && !gameManager.configuration.continuelastsave)
            gameManager.MainMenu();
          gameManager.m_State = GameManager.State.WorldReady;
          gameManager.ReportBootProgress(1f);
          await Task.WhenAll(loading, PlatformManager.instance.WaitForAchievements());
          gameManager.EnableCameraRendering();
          gameManager.m_WorldReadySource.TrySetResult(true);
          bool flag = true;
          if (gameManager.configuration.startGame.isValid)
            flag = await gameManager.AutoLoad(gameManager.configuration.startGame);
          else if (gameManager.configuration.continuelastsave)
            flag = await gameManager.AutoLoad((IAssetData) gameManager.settings.userState.lastSaveGameMetadata);
          if (!flag)
          {
            int num = await gameManager.MainMenu() ? 1 : 0;
          }
          taskManager = (TaskManager) null;
          caching = (Task) null;
          engagement = (Task) null;
          loading = (Task) null;
          assetLoading = (Task) null;
        }
        GameManager.log.Info((object) "Asset caching completed");
      }
      catch (OperationCanceledException ex)
      {
        UnityEngine.Debug.Log((object) "GameManager termination requested before initialization completed");
      }
      catch (Exception ex)
      {
        GameManager.log.Fatal(ex);
        GameManager.QuitGame();
      }
    }

    private void InitializeModManager(bool ignoreParadox = false)
    {
      if (this.m_UpdateSystem == null || !ignoreParadox && !Colossal.IO.AssetDatabase.AssetDatabase<ParadoxMods>.instance.isCached)
        return;
      this.m_ModManager.Initialize(this.m_UpdateSystem);
    }

    private void EnableSettingsDependantObjects()
    {
      this.m_Cts.Token.ThrowIfCancellationRequested();
      foreach (GameObject settingsDependantObject in this.m_SettingsDependantObjects)
        settingsDependantObject.SetActive((bool) (UnityEngine.Object) settingsDependantObject);
    }

    public async Task<bool> WaitForReadyState()
    {
      GameManager gameManager = this;
      try
      {
        // ISSUE: reference to a compiler-generated method
        await using (gameManager.m_Cts.Token.Register(new System.Action(gameManager.\u003CWaitForReadyState\u003Eb__64_0)))
        {
          int num = await gameManager.m_WorldReadySource.Task.ConfigureAwait(false) ? 1 : 0;
        }
        return gameManager.m_WorldReadySource.Task.IsCompletedSuccessfully;
      }
      catch (OperationCanceledException ex)
      {
        return false;
      }
    }

    private void Update()
    {
      if (this.shouldUpdateManager && !this.m_Cts.IsCancellationRequested)
      {
        this.m_InputManager.Update();
        this.m_UIInputSystem.DispatchInputEvents(this.m_InputManager.activeControlScheme == InputManager.ControlScheme.KeyboardAndMouse);
        this.UpdateWorld();
        this.UpdateUI();
        this.PostUpdateWorld();
      }
      this.UpdateUpdaters();
      this.UpdatePlatforms();
    }

    private void LateUpdate()
    {
      if (this.m_Cts.IsCancellationRequested)
        return;
      this.LateUpdateWorld();
    }

    public static void QuitGame() => Application.Quit();

    public void FocusChanged(bool hasFocus) => this.m_InputManager?.OnFocusChanged(hasFocus);

    private bool WantsToQuit()
    {
      if (this.m_State != GameManager.State.Quitting && this.m_State != GameManager.State.Terminated)
      {
        this.TerminateGame();
        return false;
      }
      if (this.m_State != GameManager.State.Quitting)
        return true;
      UnityEngine.Debug.LogWarning((object) "TerminateGame is already in progress, please wait.");
      return false;
    }

    private void OnDestroy() => Application.wantsToQuit -= new Func<bool>(this.WantsToQuit);

    private async Task TerminateGame()
    {
      GameManager gameManager = this;
      if (gameManager.m_Cts == null)
      {
        gameManager.m_State = GameManager.State.Terminated;
        GameManager.QuitGame();
      }
      else
      {
        if (gameManager.m_State == GameManager.State.Quitting)
          return;
        if (gameManager.m_State == GameManager.State.Terminated)
          return;
        try
        {
          using (PerformanceCounter.Start((Action<TimeSpan>) (t => GameManager.log?.InfoFormat("GameManager destroyed ({0}ms)", (object) t.TotalMilliseconds))))
          {
            GameManager.State quittingState = gameManager.m_State;
            gameManager.m_State = GameManager.State.Quitting;
            gameManager.m_QuitRequested.Cancel();
            await TaskManager.instance.Complete("SaveLoadGame");
            if (quittingState >= GameManager.State.WorldReady)
            {
              LauncherSettings.SaveSettings(gameManager.settings);
              await TaskManager.instance.SharedTask("CacheAssets", (Func<Task>) (() => Colossal.IO.AssetDatabase.AssetDatabase.global.SaveSettings()));
            }
            TaskManager.instance.onNotifyProgress -= new OnNotifyProgress(gameManager.NotifyProgress);
            gameManager.m_Cts.Cancel();
            gameManager.m_ModManager?.Dispose();
            gameManager.DestroyWorld();
            gameManager.m_InputManager?.Dispose();
            gameManager.DisposeThumbnails();
            bool flag1 = await gameManager.DisposePlatforms().AwaitWithTimeout(TimeSpan.FromSeconds(10.0));
            gameManager.ReleaseUI();
            gameManager.StopAllCoroutines();
            bool flag = flag1;
            bool flag2 = flag & await TaskManager.instance.CompleteAndClear().AwaitWithTimeout(TimeSpan.FromSeconds(10.0));
            VolumeHelper.Dispose();
            Colossal.IO.AssetDatabase.AssetDatabase.global.Dispose();
            LogManager.ReleaseResources();
            Colossal.Gizmos.ReleaseResources();
            LogManager.stdOutActive = false;
            gameManager.ReleaseConsole();
            GameManager.instance = (GameManager) null;
            Application.focusChanged -= new Action<bool>(gameManager.FocusChanged);
            if (flag2)
              UnityEngine.Debug.Log((object) "Game terminated successfully");
            else
              UnityEngine.Debug.Log((object) "Game terminated due to timeout");
          }
        }
        catch (Exception ex)
        {
          GameManager.instance = (GameManager) null;
          GameManager.log.Error(ex);
        }
        finally
        {
          gameManager.m_State = GameManager.State.Terminated;
          GameManager.QuitGame();
        }
      }
    }

    private Task SetInitialEngagementScreenActive()
    {
      this.m_IsEngagementStarted = true;
      if (!PlatformManager.instance.requiresEngagement)
        return Task.CompletedTask;
      Task task;
      switch (this.m_InitialEngagementScreen)
      {
        case OverlayScreen.UserLoggedOut:
          task = this.SetScreenActive<LoggedOutScreen>();
          break;
        case OverlayScreen.ControllerDisconnected:
          task = this.SetScreenActive<ControllerDisconnectedScreen>();
          break;
        case OverlayScreen.ControllerPairingChanged:
          task = this.SetScreenActive<ControllerPairingScreen>();
          break;
        default:
          task = this.SetScreenActive<EngagementScreen>();
          break;
      }
      return task;
    }

    private void RegisterDeviceAndUserListeners()
    {
      this.m_InputManager.EventActiveDeviceAssociationLost += new System.Action(this.HandleDeviceAssociationLost);
      this.m_InputManager.EventActiveDeviceDisconnected += new System.Action(this.HandleDeviceDisconnected);
      this.m_InputManager.EventDevicePaired += new System.Action(this.HandleDevicePaired);
      PlatformManager.instance.onUserUpdated += new OnUserUpdatedEventHandler(this.HandleUserUpdated);
    }

    private void HandleDeviceAssociationLost()
    {
      if (this.m_IsEngagementStarted)
      {
        this.SetScreenActive<ControllerPairingScreen>();
      }
      else
      {
        if (this.m_InitialEngagementScreen <= OverlayScreen.ControllerPairingChanged)
          return;
        this.m_InitialEngagementScreen = OverlayScreen.ControllerPairingChanged;
      }
    }

    private void HandleDeviceDisconnected()
    {
      if (this.m_IsEngagementStarted)
      {
        this.SetScreenActive<ControllerDisconnectedScreen>();
      }
      else
      {
        if (this.m_InitialEngagementScreen <= OverlayScreen.ControllerDisconnected)
          return;
        this.m_InitialEngagementScreen = OverlayScreen.ControllerDisconnected;
      }
    }

    private void HandleDevicePaired()
    {
      if (this.m_IsEngagementStarted)
        return;
      this.m_InitialEngagementScreen = OverlayScreen.Engagement;
    }

    private void HandleUserUpdated(IPlatformServiceIntegration psi, UserChangedFlags flags)
    {
      if (this.m_IsEngagementStarted)
      {
        if (!flags.HasFlag((Enum) UserChangedFlags.UserSigningOut) || flags.HasFlag((Enum) UserChangedFlags.ChangingUser))
          return;
        this.SetScreenActive<LoggedOutScreen>();
      }
      else if (flags.HasFlag((Enum) UserChangedFlags.UserSigningOut) && this.m_InitialEngagementScreen > OverlayScreen.UserLoggedOut)
      {
        this.m_InitialEngagementScreen = OverlayScreen.UserLoggedOut;
      }
      else
      {
        if (!flags.HasFlag((Enum) UserChangedFlags.UserSignedInAgain))
          return;
        this.m_InitialEngagementScreen = OverlayScreen.Engagement;
      }
    }

    public event GameManager.EventGameSaveLoad onGameSaveLoad;

    public event GameManager.EventGamePreload onGamePreload;

    public event GameManager.EventGamePreload onGameLoadingComplete;

    private void CleanupMemory()
    {
      UnityEngine.Resources.UnloadUnusedAssets();
      foreach (Colossal.UI.UISystem uiSystem in (IEnumerable<Colossal.UI.UISystem>) UIManager.UISystems)
        uiSystem.ClearCachedUnusedImages();
      GC.Collect();
    }

    private Task SaveSimulationData(Colossal.Serialization.Entities.Purpose purpose, Stream stream)
    {
      this.CleanupMemory();
      this.m_SerializationSystem.stream = stream;
      this.m_SerializationSystem.context = new Colossal.Serialization.Entities.Context(purpose, Game.Version.current);
      return this.m_SerializationSystem.RunOnce();
    }

    public Task<bool> Save(
      string saveName,
      SaveInfo meta,
      ILocalAssetDatabase database,
      UnityEngine.Texture savePreview)
    {
      return this.Save(saveName, meta, database, new ScreenCaptureHelper.AsyncRequest(savePreview));
    }

    public async Task<bool> Save(
      string saveName,
      SaveInfo meta,
      ILocalAssetDatabase database,
      ScreenCaptureHelper.AsyncRequest previewRequest)
    {
      GameManager.log.Info((object) ("Save " + saveName + " to " + database.name));
      GameManager.EventGameSaveLoad onGameSaveLoad1 = this.onGameSaveLoad;
      if (onGameSaveLoad1 != null)
        onGameSaveLoad1(saveName, true);
      bool flag;
      using (ILocalAssetDatabase saveDatabase = Colossal.IO.AssetDatabase.AssetDatabase.GetTransient())
      {
        meta.sessionGuid = Game.PSI.Telemetry.GetCurrentSession();
        meta.lastModified = DateTime.Now;
        AssetDataPath tempSavePath = (AssetDataPath) saveName;
        SaveGameData saveGameData = saveDatabase.AddAsset<SaveGameData>(tempSavePath);
        await this.SaveSimulationData(Colossal.Serialization.Entities.Purpose.SaveGame, saveGameData.GetWriteStream());
        meta.saveGameData = saveGameData;
        if (previewRequest != null)
        {
          await previewRequest.Complete();
          using (TextureImporter.Texture uncompressed1Mip = TextureImporter.Texture.CreateUncompressed1Mip(saveName, previewRequest.width, previewRequest.height, false, previewRequest.result))
          {
            using (TextureAsset textureAsset = saveDatabase.AddAsset((TextureImporter.ITexture) uncompressed1Mip))
            {
              meta.preview = textureAsset;
              textureAsset.Save(0, false);
            }
          }
        }
        PackageAsset p1 = await Task.Run<PackageAsset>((Func<PackageAsset>) (() =>
        {
          SaveGameMetadata saveGameMetadata = saveDatabase.AddAsset<SaveGameMetadata>(tempSavePath);
          saveGameMetadata.target = meta;
          saveGameMetadata.Save(false);
          AssetDataPath assetDataPath = SaveHelpers.GetAssetDataPath<SaveGameMetadata>(database, saveName);
          PackageAsset asset;
          if (database.Exists<PackageAsset>(assetDataPath, out asset))
            database.DeleteAsset<PackageAsset>(asset);
          PackageAsset packageAsset = database.AddAsset(assetDataPath, saveDatabase);
          packageAsset.Save(false);
          this.settings.userState.lastSaveGameMetadata = saveGameMetadata;
          this.settings.userState.ApplyAndSave();
          Launcher.SaveLastSaveMetadata(meta);
          return packageAsset;
        }));
        GameManager.EventGameSaveLoad onGameSaveLoad2 = this.onGameSaveLoad;
        if (onGameSaveLoad2 != null)
          onGameSaveLoad2(saveName, false);
        GameManager.log.InfoFormat("Saving completed {0}", (object) p1);
        flag = true;
      }
      return flag;
    }

    private Task LoadSimulationData(Colossal.Serialization.Entities.Purpose purpose, AsyncReadDescriptor dataDescriptor)
    {
      GameManager.EventGamePreload onGamePreload = this.onGamePreload;
      if (onGamePreload != null)
        onGamePreload(purpose, this.gameMode);
      this.CleanupMemory();
      this.m_DeserializationSystem.dataDescriptor = dataDescriptor;
      this.m_DeserializationSystem.context = new Colossal.Serialization.Entities.Context(purpose, Game.Version.current);
      return this.m_DeserializationSystem.RunOnce();
    }

    private async Task<bool> Load(
      GameMode mode,
      Colossal.Serialization.Entities.Purpose purpose,
      AsyncReadDescriptor descriptor,
      Guid sessionGuid)
    {
      GameManager.log.InfoFormat("Loading mode {0} with purpose {1}", (object) mode, (object) purpose);
      if (descriptor == AsyncReadDescriptor.Invalid && purpose != Colossal.Serialization.Entities.Purpose.NewGame && purpose != Colossal.Serialization.Entities.Purpose.NewMap && purpose != Colossal.Serialization.Entities.Purpose.Cleanup)
      {
        GameManager.log.WarnFormat("Invalid descriptor provided with purpose {0}", (object) purpose);
        return false;
      }
      GameMode oldMode = this.gameMode;
      this.gameMode = mode;
      this.m_State = GameManager.State.Loading;
      if (mode.IsGameOrEditor())
      {
        TaskManager taskManager = TaskManager.instance;
        taskManager.ScheduleGroup(ProgressTracker.Group.Group1, 1);
        taskManager.ScheduleGroup(ProgressTracker.Group.Group2, 1);
        taskManager.ScheduleGroup(ProgressTracker.Group.Group3, 1);
        taskManager.progress.Report(new ProgressTracker("LoadTextures", ProgressTracker.Group.Group1)
        {
          progress = 0.0f
        });
        TaskProgress progress1 = taskManager.progress;
        ProgressTracker progressTracker1 = new ProgressTracker("LoadMeshes", ProgressTracker.Group.Group2);
        progressTracker1.progress = 0.0f;
        ProgressTracker progressTracker2 = progressTracker1;
        progress1.Report(progressTracker2);
        TaskProgress progress2 = taskManager.progress;
        progressTracker1 = new ProgressTracker("LoadSimulation", ProgressTracker.Group.Group3);
        progressTracker1.progress = 0.0f;
        ProgressTracker progressTracker3 = progressTracker1;
        progress2.Report(progressTracker3);
        TextureStreamingSystem tss = this.m_World.GetExistingSystemManaged<TextureStreamingSystem>();
        this.RegisterUpdater((Func<bool>) (() =>
        {
          float num = (float) (((double) tss.VTMaterialAssetsProgression + (double) tss.VTMaterialDuplicatesProgression) * 0.5);
          taskManager.progress.Report(new ProgressTracker("LoadTextures", ProgressTracker.Group.Group1)
          {
            progress = num
          });
          return (double) num >= 1.0;
        }));
      }
      Task loading = this.SetScreenActive<LoadingScreen>();
      await Task.Yield();
      if (mode != GameMode.MainMenu || oldMode != GameMode.Other)
        await this.LoadSimulationData(purpose, descriptor);
      switch (this.gameMode)
      {
        case GameMode.Game:
          Game.PSI.Telemetry.OpenSession(sessionGuid);
          this.userInterface.appBindings.SetGameActive();
          break;
        case GameMode.Editor:
          Game.PSI.Telemetry.OpenSession(sessionGuid);
          this.userInterface.appBindings.SetEditorActive();
          break;
        case GameMode.MainMenu:
          Game.PSI.Telemetry.CloseSession();
          this.userInterface.appBindings.SetMainMenuActive();
          Cursor.lockState = CursorLockMode.None;
          break;
      }
      PlatformManager.instance.SetRichPresence(this.gameMode.ToRichPresence());
      await loading;
      this.CleanupMemory();
      GameManager.EventGamePreload gameLoadingComplete = this.onGameLoadingComplete;
      if (gameLoadingComplete != null)
        gameLoadingComplete(purpose, mode);
      this.m_State = GameManager.State.WorldReady;
      GameManager.log.Info((object) "Loading completed");
      return true;
    }

    private Guid GetSessionGuid(Colossal.Serialization.Entities.Purpose purpose, Guid existingGuid)
    {
      return purpose == Colossal.Serialization.Entities.Purpose.NewMap || purpose == Colossal.Serialization.Entities.Purpose.NewGame ? Guid.NewGuid() : existingGuid;
    }

    public Task<bool> Load(GameMode mode, Colossal.Serialization.Entities.Purpose purpose, IAssetData asset = null)
    {
      GameManager.log.InfoFormat("Starting game from '{0}'", (object) asset);
      switch (asset)
      {
        case MapMetadata mapMetadata:
          using (mapMetadata)
          {
            AsyncReadDescriptor descriptor = AsyncReadDescriptor.Invalid;
            MapInfo target = mapMetadata.target;
            if ((AssetData) target.mapData == (IAssetData) null)
              GameManager.log.WarnFormat("The mapData referenced by '{0}' (meta: '{1}') doesn't not exist'", (object) target.id, (object) asset);
            else
              descriptor = target.mapData.GetAsyncReadDescriptor();
            return this.Load(mode, purpose, descriptor, this.GetSessionGuid(purpose, target.sessionGuid));
          }
        case SaveGameMetadata saveGameMetadata:
          using (saveGameMetadata)
          {
            AsyncReadDescriptor descriptor = AsyncReadDescriptor.Invalid;
            SaveInfo target = saveGameMetadata.target;
            if ((AssetData) target.saveGameData == (IAssetData) null)
              GameManager.log.WarnFormat("The saveGameData referenced by '{0}' (meta: '{1}') doesn't not exist'", (object) target.id, (object) asset);
            else
              descriptor = target.saveGameData.GetAsyncReadDescriptor();
            return this.Load(mode, purpose, descriptor, this.GetSessionGuid(purpose, target.sessionGuid));
          }
        case MapData mapData:
          GameManager.log.Warn((object) "Loading with MapData. Session guid will be lost, rather use metadata if available");
          return this.Load(mode, purpose, mapData.GetAsyncReadDescriptor(), Guid.NewGuid());
        case SaveGameData saveGameData:
          GameManager.log.Warn((object) "Loading with SaveGameData. Session guid will be lost, rather use metadata if available");
          return this.Load(mode, purpose, saveGameData.GetAsyncReadDescriptor(), Guid.NewGuid());
        case null:
          return this.Load(mode, purpose, AsyncReadDescriptor.Invalid, Guid.NewGuid());
        default:
          GameManager.log.WarnFormat("Couldn't start game from '{0}'", (object) asset);
          return Task.FromResult<bool>(false);
      }
    }

    public Task<bool> Load(GameMode mode, Colossal.Serialization.Entities.Purpose purpose, Colossal.Hash128 guid)
    {
      IAssetData assetData;
      if (Colossal.IO.AssetDatabase.AssetDatabase.global.TryGetAsset(guid, out assetData))
        return this.Load(mode, purpose, assetData);
      GameManager.log.WarnFormat("Couldn't load '{0}'. Asset doesn't exist!", (object) guid);
      return Task.FromResult<bool>(false);
    }

    private Task<bool> AutoLoad(IAssetData asset)
    {
      switch (asset)
      {
        case MapData _:
        case MapMetadata _:
          return this.Load(GameMode.Game, Colossal.Serialization.Entities.Purpose.NewGame, asset);
        case SaveGameData _:
        case SaveGameMetadata _:
          return this.Load(GameMode.Game, Colossal.Serialization.Entities.Purpose.LoadGame, asset);
        default:
          GameManager.log.WarnFormat("Couldn't load '{0}'. Asset doesn't exist!", (object) asset);
          return Task.FromResult<bool>(false);
      }
    }

    private Task<bool> AutoLoad(Colossal.Hash128 guid)
    {
      IAssetData assetData;
      if (Colossal.IO.AssetDatabase.AssetDatabase.global.TryGetAsset(guid, out assetData))
        return this.AutoLoad(assetData);
      GameManager.log.WarnFormat("Couldn't load '{0}'. Asset doesn't exist!", (object) guid);
      return Task.FromResult<bool>(false);
    }

    public async Task<bool> MainMenu()
    {
      try
      {
        bool ret = await this.Load(GameMode.MainMenu, Colossal.Serialization.Entities.Purpose.Cleanup);
        bool flag = ret;
        ret = flag & await this.WaitForReadyState();
        if (ret)
        {
          // ISSUE: reference to a compiler-generated method
          await AudioManager.instance.ResetAudioOnMainThread();
          // ISSUE: reference to a compiler-generated method
          await AudioManager.instance.PlayMenuMusic("Main Menu Theme");
        }
        return ret;
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        GameManager.log.Error(ex);
      }
      return false;
    }

    private Task PreparePersistentStorage()
    {
      EnvPath.RegisterSpecialPath<SaveGameMetadata>(SaveGameMetadata.kPersistentLocation);
      EnvPath.RegisterSpecialPath<SaveGameData>(SaveGameMetadata.kPersistentLocation);
      EnvPath.RegisterSpecialPath<MapMetadata>(MapMetadata.kPersistentLocation);
      EnvPath.RegisterSpecialPath<MapData>(MapMetadata.kPersistentLocation);
      EnvPath.RegisterSpecialPath<CinematicCameraAsset>(CinematicCameraAsset.kPersistentLocation);
      return EnvPath.WipeTempPath();
    }

    private async Task RegisterPdxSdk()
    {
      GameManager gameManager1 = this;
      if (!gameManager1.configuration.disablePDXSDK)
      {
        GameManager gameManager = gameManager1;
        PdxSdkConfiguration pdxConfiguration = new PdxSdkConfiguration()
        {
          language = gameManager1.localizationManager.activeLocaleId,
          gameNamespace = "cities_skylines_2",
          gameVersion = Game.Version.current.fullVersion,
          environment = ProductEnvironment.Live
        };
        await PlatformManager.instance.RegisterPSI<PdxSdkPlatform>((Func<PdxSdkPlatform>) (() =>
        {
          CancellationTokenSource cts = new CancellationTokenSource();
          PdxSdkPlatform pdxSdkPlatform = new PdxSdkPlatform(pdxConfiguration);
          gameManager.localizationManager.onActiveDictionaryChanged += (System.Action) (() => pdxSdkPlatform.ChangeLanguage(gameManager.localizationManager.activeLocaleId));
          // ISSUE: reference to a compiler-generated method
          pdxSdkPlatform.onLegalDocumentStatusChanged += new OnLegalDocumentStatusChangedEventHandler(gameManager.\u003CRegisterPdxSdk\u003Eb__107_2);
          pdxSdkPlatform.onNoLogin += (System.Action) (async () =>
          {
            try
            {
              if (gameManager.state == GameManager.State.Quitting || gameManager.configuration.disableModding)
                return;
              await RegisterDatabase();
            }
            catch (OperationCanceledException ex)
            {
            }
            catch (Exception ex)
            {
              gameManager.InitializeModManager(true);
              PdxSdkPlatform.log.Error(ex);
            }
            finally
            {
              cts = new CancellationTokenSource();
            }
          });
          pdxSdkPlatform.onLoggedIn += (OnLoggedInEventHandler) (async (firstName, lastName, email, accountLinkState, firstTime) =>
          {
            try
            {
              if (gameManager.state == GameManager.State.Quitting)
                return;
              Task task = Task.CompletedTask;
              if (!gameManager.configuration.disableModding)
                task = pdxSdkPlatform.SyncMods();
              if (gameManager.configuration.disableModding)
                return;
              await task;
              await RegisterDatabase();
            }
            catch (OperationCanceledException ex)
            {
            }
            catch (Exception ex)
            {
              gameManager.InitializeModManager(true);
              PdxSdkPlatform.log.Error(ex);
            }
            finally
            {
              cts = new CancellationTokenSource();
            }
          });
          pdxSdkPlatform.onLoggedOut += (OnLoggedOutEventHandler) (async id =>
          {
            cts.Cancel();
            cts = new CancellationTokenSource();
            if (gameManager.configuration.disableModding)
              return;
            if (!await Colossal.IO.AssetDatabase.AssetDatabase.global.UnregisterDatabase((IAssetDatabase) Colossal.IO.AssetDatabase.AssetDatabase<ParadoxMods>.instance))
              return;
            Colossal.IO.AssetDatabase.AssetDatabase<ParadoxMods>.instance.Dispose();
          });
          // ISSUE: reference to a compiler-generated method
          pdxSdkPlatform.onContentUnlocked += new ContentUnlockedEventHandler(gameManager.\u003CRegisterPdxSdk\u003Eb__107_6);
          pdxSdkPlatform.onDataSyncConflict += (System.Action) (() => NotificationSystem.Push("PDXDataSyncConflict", titleId: "ActionRequired", textId: "PDXDataSyncConflict", progressState: new ProgressState?(ProgressState.Warning), onClicked: (System.Action) (async () =>
          {
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            gameManager.userInterface.appBindings.ShowConfirmationDialog((Game.UI.ConfirmationDialog) new ParadoxCloudConflictResolutionDialog(), (Action<int>) (msg => tcs.SetResult(msg)));
            int task = await tcs.Task;
            if (tcs.Task.Result != -1)
            {
              NotificationSystem.Pop("PDXDataSyncConflict");
              NotificationSystem.Push("PDXDataSyncConflictResolving", titleId: "PDXDataSyncConflict", textId: "PDXDataSyncConflictResolving", progressState: new ProgressState?(ProgressState.Indeterminate));
              if (await pdxSdkPlatform.SyncModConflict(tcs.Task.Result == 0 ? SyncDirection.Downstream : SyncDirection.Upstream))
                NotificationSystem.Pop("PDXDataSyncConflictResolving", 1f, titleId: "PDXDataSyncConflict", textId: "PDXDataSyncConflictResolved", progressState: new ProgressState?(ProgressState.Complete));
              else
                NotificationSystem.Pop("PDXDataSyncConflictResolving", 1f, titleId: "PDXDataSyncConflict", textId: "PDXDataSyncConflictFailed", progressState: new ProgressState?(ProgressState.Failed));
            }
          })));
          pdxSdkPlatform.onModSyncCompleted += (ModSyncEventHandler) (psi =>
          {
            if (pdxSdkPlatform.HasLocalChanges())
              return;
            NotificationSystem.Pop("PDXDataSyncConflict");
          });
          return pdxSdkPlatform;

          async Task RegisterDatabase()
          {
            cts.Token.ThrowIfCancellationRequested();
            Colossal.IO.AssetDatabase.AssetDatabase<ParadoxMods> modsDatabase = Colossal.IO.AssetDatabase.AssetDatabase<ParadoxMods>.instance;
            if (modsDatabase.dataSource is ParadoxModsDataSource dataSource2)
            {
              // ISSUE: method pointer
              dataSource2.onAfterActivePlaysetOrModStatusChanged -= new System.Action((object) gameManager, __methodptr(\u003CRegisterPdxSdk\u003Eg__OnActivePlaysetChanged\u007C107_12));
              // ISSUE: method pointer
              modsDatabase.onAssetDatabaseChanged.Unsubscribe(new EventDelegate<AssetChangedEventArgs>((object) gameManager, __methodptr(\u003CRegisterPdxSdk\u003Eg__OnAssetChanged\u007C107_13)));
              dataSource2.onEntryIsInActivePlaysetChanged -= new Action<Colossal.Hash128, bool>(OnEntryIsInActivePlaysetChanged);
              await Colossal.IO.AssetDatabase.AssetDatabase.global.RegisterDatabase((IAssetDatabase) modsDatabase);
              // ISSUE: method pointer
              modsDatabase.onAssetDatabaseChanged.Subscribe(new EventDelegate<AssetChangedEventArgs>((object) gameManager, __methodptr(\u003CRegisterPdxSdk\u003Eg__OnAssetChanged\u007C107_13)));
              dataSource2.onEntryIsInActivePlaysetChanged += new Action<Colossal.Hash128, bool>(OnEntryIsInActivePlaysetChanged);
              // ISSUE: method pointer
              dataSource2.onAfterActivePlaysetOrModStatusChanged += new System.Action((object) gameManager, __methodptr(\u003CRegisterPdxSdk\u003Eg__OnActivePlaysetChanged\u007C107_12));
              await dataSource2.Populate();
            }
            gameManager.InitializeModManager(!modsDatabase.isCached);
            dataSource2 = (ParadoxModsDataSource) null;

            void OnEntryIsInActivePlaysetChanged(Colossal.Hash128 guid, bool isInActivePlayset)
            {
              IAssetData asset;
              if (!modsDatabase.TryGetAsset(guid, out asset))
                return;
              switch (asset)
              {
                case ExecutableAsset executableAsset2:
                  executableAsset2.isInActivePlayset = isInActivePlayset;
                  break;
                case UIModuleAsset uiModuleAsset2:
                  uiModuleAsset2.isInActivePlayset = isInActivePlayset;
                  break;
                case SurfaceAsset _:
                case MidMipCacheAsset _:
                  gameManager.m_World.GetOrCreateSystemManaged<TextureStreamingSystem>()?.MarkVTAssetsDirty();
                  break;
                case PrefabAsset prefabAsset2:
                  try
                  {
                    GameManager.log.DebugFormat("OnEntryIsInActivePlaysetChanged: {0} ({1})", (object) asset.name, (object) isInActivePlayset);
                    PrefabBase prefab = prefabAsset2.Load() as PrefabBase;
                    if (isInActivePlayset)
                    {
                      // ISSUE: reference to a compiler-generated method
                      if (!gameManager.m_PrefabSystem.AddPrefab(prefab))
                        break;
                      GameManager.log.DebugFormat("Loaded {0}", (object) prefab.name);
                      break;
                    }
                    // ISSUE: reference to a compiler-generated method
                    if (!gameManager.m_PrefabSystem.RemovePrefab(prefab))
                      break;
                    GameManager.log.DebugFormat("Removed {0}", (object) prefab.name);
                    break;
                  }
                  catch (Exception ex)
                  {
                    GameManager.log.Error(ex);
                    break;
                  }
              }
            }
          }
        }), gameManager1.m_Cts.Token).ConfigureAwait(false);
      }
      else
      {
        PlatformManager.instance.EnableSharing();
        await Task.CompletedTask;
      }
    }

    private void TelemetryReady()
    {
      Game.PSI.Telemetry.FireSessionStartEvents();
      PlatformManager.instance.onAchievementUpdated += (AchievementUpdatedEventHandler) ((p, a) => Game.PSI.Telemetry.AchievementUnlocked(a));
    }

    private async Task InitializePlatformManager()
    {
      PlatformManager.instance.RegisterRichPresenceKey("#StatusInMainMenu", (Func<string>) (() => "In Main-Menu"));
      PlatformManager.instance.RegisterRichPresenceKey("#StatusInGame", (Func<string>) (() => "In-Game"));
      PlatformManager.instance.RegisterRichPresenceKey("#StatusInEditor", (Func<string>) (() => "In-Editor"));
      await PlatformManager.instance.RegisterPSI<IPlatformServiceIntegration>(PlatformSupport.kCreateSteamPlatform, this.m_Cts.Token).ConfigureAwait(false);
      await PlatformManager.instance.RegisterPSI<IPlatformServiceIntegration>(PlatformSupport.kCreateDiscordRichPresence, this.m_Cts.Token).ConfigureAwait(false);
      if (!await PlatformManager.instance.Initialize(this.m_Cts.Token).ConfigureAwait(false))
      {
        GameManager.log.ErrorFormat("A platform service integration failed to initialize");
        GameManager.QuitGame();
      }
      EnvPath.UpdateSpecialPathCache();
      await Colossal.IO.AssetDatabase.AssetDatabase.global.RegisterDatabase((IAssetDatabase) Colossal.IO.AssetDatabase.AssetDatabase<SteamCloud>.instance);
      await Colossal.IO.AssetDatabase.ContentHelper.RegisterContent();
    }

    private void UpdatePlatforms() => PlatformManager.instance.Update();

    private async Task DisposePlatforms()
    {
      Task task = PlatformManager.instance.Dispose(true, CancellationToken.None);
      while (!task.IsCompleted)
      {
        this.Update();
        await Task.Delay(500);
      }
      await task;
      task = (Task) null;
    }

    public static UIInputSystem UIInputSystem => GameManager.instance?.m_UIInputSystem;

    public LocalizationManager localizationManager { get; private set; }

    public UserInterface userInterface { get; private set; }

    private async Task InitializeUI()
    {
      GameManager coroutineHost = this;
      coroutineHost.m_Cts.Token.ThrowIfCancellationRequested();
      ILog uiLog = UIManager.log;
      try
      {
        uiLog.Info((object) "Bootstrapping cohtmlUISystem");
        coroutineHost.m_UIManager = new UIManager(coroutineHost.configuration.uiDeveloperMode);
        Colossal.UI.UISystem.Settings settings = Colossal.UI.UISystem.Settings.New with
        {
          localizationManager = (ILocalizationManager) new UILocalizationManager(coroutineHost.localizationManager),
          resourceHandler = (IResourceHandler) new GameUIResourceHandler((MonoBehaviour) coroutineHost)
        };
        Colossal.UI.UISystem system = coroutineHost.m_UIManager.CreateUISystem(settings);
        await Colossal.IO.AssetDatabase.AssetDatabase.global.RegisterUILocations((Action<string, string>) ((uri, path) => system.AddHostLocation(uri, path)));
        coroutineHost.m_UIInputSystem = new UIInputSystem(system);
        coroutineHost.userInterface = new UserInterface(coroutineHost.m_UILocation, coroutineHost.localizationManager, system);
        coroutineHost.m_World.GetOrCreateSystem<NotificationUISystem>();
        coroutineHost.m_World.GetOrCreateSystem<OptionsUISystem>();
        coroutineHost.settings.RegisterInOptionsUI();
        coroutineHost.m_State = GameManager.State.UIReady;
        coroutineHost.m_InputManager.CheckConflicts();
        GameManager.log.DebugFormat("Time to UI {0}s", (object) Time.realtimeSinceStartup);
        await coroutineHost.userInterface.WaitForBindings();
        uiLog = (ILog) null;
      }
      catch (Exception ex)
      {
        uiLog.Error(ex);
        uiLog = (ILog) null;
      }
    }

    private void CreateUISystems()
    {
      foreach (System.Type type in ReflectionUtils.GetAllTypesDerivedFrom<UISystemBase>())
      {
        if (!type.IsAbstract)
          this.m_World.GetOrCreateSystem(type);
      }
    }

    private void UpdateUI()
    {
      this.m_UIManager.Update();
      this.userInterface.Update();
    }

    private void ReleaseUI()
    {
      this.userInterface?.Dispose();
      this.m_UIInputSystem?.Dispose();
      this.m_UIManager?.Dispose();
    }

    public Task SetScreenActive<T>() where T : IScreenState, new()
    {
      return new T().Execute(this, this.m_Cts.Token);
    }

    private void UpdateUpdaters()
    {
      foreach (KeyValuePair<Guid, Func<bool>> updater in this.m_Updaters)
      {
        if (updater.Value())
          this.UnregisterUpdater(updater.Key);
      }
    }

    public Guid RegisterUpdater(System.Action action)
    {
      return this.RegisterUpdater((Func<bool>) (() =>
      {
        action();
        return true;
      }));
    }

    public Guid RegisterUpdater(Func<bool> func)
    {
      if (func == null)
        return Guid.Empty;
      Guid guid = Guid.NewGuid();
      this.m_Updaters.TryAdd(guid, func);
      GameManager.log.DebugFormat("Updater {0} registered with guid {1}", (object) func.Method.Name, (object) guid.ToLowerNoDashString());
      return guid;
    }

    public bool UnregisterUpdater(Guid guid)
    {
      Func<bool> func;
      if (this.m_Updaters.TryRemove(guid, out func))
      {
        GameManager.log.DebugFormat("Updater {0} with {1} unregistered", (object) guid.ToLowerNoDashString(), (object) func.Method.Name);
        return true;
      }
      GameManager.log.DebugFormat("Updater {0} was not found");
      return false;
    }

    private void ReportBootProgress(float progress)
    {
      TaskManager.instance.progress.Report(new ProgressTracker("Boot", ProgressTracker.Group.Group3)
      {
        progress = progress
      });
    }

    private void NotifyProgress(string identifier, int progress)
    {
      string identifier1 = identifier;
      string str1 = identifier;
      string str2 = identifier;
      ProgressState? nullable1 = new ProgressState?(ProgressState.Progressing);
      int? nullable2 = new int?(progress);
      LocalizedString? title1 = new LocalizedString?();
      LocalizedString? text1 = new LocalizedString?();
      string titleId1 = str1;
      string textId1 = str2;
      ProgressState? progressState1 = nullable1;
      int? progress1 = nullable2;
      NotificationSystem.Push(identifier1, title1, text1, titleId1, textId1, progressState: progressState1, progress: progress1);
      if (progress < 100)
        return;
      string identifier2 = identifier;
      string str3 = identifier;
      string str4 = identifier;
      nullable1 = new ProgressState?(ProgressState.Complete);
      nullable2 = new int?(progress);
      LocalizedString? title2 = new LocalizedString?();
      LocalizedString? text2 = new LocalizedString?();
      string titleId2 = str3;
      string textId2 = str4;
      ProgressState? progressState2 = nullable1;
      int? progress2 = nullable2;
      NotificationSystem.Pop(identifier2, 2f, title2, text2, titleId2, textId2, progressState: progressState2, progress: progress2);
    }

    private void EnableMemoryLeaksDetection()
    {
      NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;
    }

    public void RunOnMainThread(System.Action action)
    {
      if (this.isMainThread)
        action();
      else
        this.RegisterUpdater(action);
    }

    public ThumbnailCache thumbnailCache { get; private set; }

    private void InitializeThumbnails()
    {
      this.m_Cts.Token.ThrowIfCancellationRequested();
      this.thumbnailCache = new ThumbnailCache();
      this.thumbnailCache.Initialize();
    }

    private void DisposeThumbnails() => this.thumbnailCache?.Dispose();

    private Task LoadUnityPrefabs()
    {
      return !this.configuration.noAssets ? (Task) this.LoadAssetLibraryAsync() : Task.CompletedTask;
    }

    private void LoadPrefabs()
    {
      int count = 0;
      using (PerformanceCounter.Start((Action<TimeSpan>) (t => GameManager.log.InfoFormat("Loaded {1} prefabs in {0}s", (object) t.TotalSeconds, (object) count))))
      {
        foreach (PrefabAsset asset in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<PrefabAsset>(new SearchFilter<PrefabAsset>()))
        {
          if (asset.Load() is PrefabBase prefab)
          {
            // ISSUE: reference to a compiler-generated method
            this.m_PrefabSystem.AddPrefab(prefab);
            count++;
          }
        }
      }
    }

    private async Task<AssetLibrary> LoadAssetLibraryAsync()
    {
      AssetLibrary asset;
      using (PerformanceCounter.Start((Action<TimeSpan>) (t => GameManager.log.InfoFormat("LoadAssetLibraryAsync performed in {0}ms", (object) t.TotalMilliseconds))))
      {
        UnityEngine.ResourceRequest asyncLoad = UnityEngine.Resources.LoadAsync<AssetLibrary>("GameAssetLibrary");
        while (!asyncLoad.isDone)
        {
          this.m_Cts.Token.ThrowIfCancellationRequested();
          await Task.Yield();
        }
        ((AssetLibrary) asyncLoad.asset).Load(this.m_PrefabSystem, this.m_Cts.Token);
        asset = asyncLoad.asset as AssetLibrary;
      }
      return asset;
    }

    private void DisableCameraRendering()
    {
      Camera main = Camera.main;
      if (!((UnityEngine.Object) main != (UnityEngine.Object) null))
        return;
      this.m_DefaultCullingMask = (LayerMask) main.cullingMask;
      main.cullingMask = 0;
      HDAdditionalCameraData component = main.GetComponent<HDAdditionalCameraData>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      this.m_DefaultVolumeLayerMask = component.volumeLayerMask;
      component.volumeLayerMask = (LayerMask) 0;
    }

    private void EnableCameraRendering()
    {
      this.m_Cts.Token.ThrowIfCancellationRequested();
      Camera main = Camera.main;
      if (!((UnityEngine.Object) main != (UnityEngine.Object) null))
        return;
      main.cullingMask = (int) this.m_DefaultCullingMask;
      HDAdditionalCameraData component = main.GetComponent<HDAdditionalCameraData>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.volumeLayerMask = this.m_DefaultVolumeLayerMask;
    }

    [DllImport("user32.dll")]
    private static extern bool SetWindowText(IntPtr hWnd, string lpString);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr FindWindow(string strClassName, string strWindowName);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [RuntimeInitializeOnLoadMethod]
    private static void SetWindowsTitle()
    {
      IntPtr window = GameManager.FindWindow((string) null, Application.productName);
      if (!(window != IntPtr.Zero))
        return;
      GameManager.SetWindowText(window, "Cities: Skylines II");
    }

    private void InitConsole()
    {
      if (this.configuration.captureStdout == GameManager.Configuration.StdoutCaptureMode.None)
        return;
      if (this.configuration.captureStdout != GameManager.Configuration.StdoutCaptureMode.Redirect)
        this.m_Console = new ConsoleWindow(Application.productName, this.configuration.captureStdout == GameManager.Configuration.StdoutCaptureMode.Console);
      LogManager.stdOutActive = true;
      GameManager.log.Info((object) "\u001B[1m\u001B[38;2;0;135;215mWelcome to Cities: Skylines II\u001B[0m");
      GameManager.log.Info((object) "\u001B[1m\u001B[38;2;0;135;215mColossal Order Oy - 2023\u001B[0m");
      Thread.Sleep(1000);
    }

    private void ReleaseConsole() => this.m_Console?.Dispose();

    private void TryCatchUnhandledExceptions()
    {
      System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (EventHandler<UnobservedTaskExceptionEventArgs>) ((sender, e) =>
      {
        e.SetObserved();
        GameManager.log.Critical((Exception) e.Exception, (object) "Unobserved exception triggered");
      });
      AppDomain.CurrentDomain.UnhandledException += (UnhandledExceptionEventHandler) ((sender, e) =>
      {
        Exception exceptionObject = (Exception) e.ExceptionObject;
        GameManager.log.Critical(exceptionObject, (object) "Unhandled domain exception triggered");
      });
    }

    private bool CheckValidity()
    {
      try
      {
        int num = GameManager.instance.enabled ? 1 : 0;
      }
      catch (MissingReferenceException ex)
      {
        this.enabled = false;
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
        GameManager.QuitGame();
        return false;
      }
      catch
      {
      }
      if (!((UnityEngine.Object) GameManager.instance != (UnityEngine.Object) null) || !((UnityEngine.Object) GameManager.instance != (UnityEngine.Object) this))
        return true;
      this.enabled = false;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      return false;
    }

    public static string GetVersionsInfo()
    {
      StringBuilder b = new StringBuilder();
      string str = "Mono";
      b.AppendLine(string.Format("Date: {0}", (object) DateTime.UtcNow));
      b.AppendLine(string.Format("Game version: {0} {1}", (object) Game.Version.current.fullVersion, (object) Application.platform.ToPlatform()));
      b.AppendLine("Game configuration: " + (UnityEngine.Debug.isDebugBuild ? "Development" : "Release") + " (" + str + ")");
      b.AppendLine("COre version: " + Colossal.Core.Version.current.fullVersion);
      b.AppendLine("Localization version: " + Colossal.Localization.Version.current.fullVersion);
      b.AppendLine("UI version: " + Colossal.UI.Version.current.fullVersion);
      b.AppendLine("Unity version: " + Application.unityVersion);
      b.AppendLine(string.Format("Cohtml version: {0}", (object) Versioning.Build));
      b.AppendLine("ATL Version: " + ATL.Version.getVersion());
      PlatformManager.instance.LogVersion(b);
      foreach (IDlc enumerateLocalDlC in PlatformManager.instance.EnumerateLocalDLCs())
        b.AppendLine(enumerateLocalDlC.internalName.Nicify() + ": " + enumerateLocalDlC.version.fullVersion);
      if (Application.genuineCheckAvailable)
        b.AppendLine(string.Format("Genuine: {0}", (object) Application.genuine));
      return b.ToString().TrimEnd();
    }

    public static string GetSystemInfoString()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("Type: " + SystemInfo.deviceType.ToString());
      stringBuilder1.AppendLine("OS: " + SystemInfo.operatingSystem);
      stringBuilder1.AppendLine("System memory: " + FormatUtils.FormatBytes((long) SystemInfo.systemMemorySize * 1024L * 1024L));
      stringBuilder1.AppendLine("Graphics device: " + SystemInfo.graphicsDeviceName + " (Version: " + SystemInfo.graphicsDeviceVersion + ")");
      stringBuilder1.AppendLine("Graphics memory: " + FormatUtils.FormatBytes((long) SystemInfo.graphicsMemorySize * 1024L * 1024L));
      stringBuilder1.AppendLine("Max texture size: " + SystemInfo.maxTextureSize.ToString());
      stringBuilder1.AppendLine("Shader level: " + SystemInfo.graphicsShaderLevel.ToString());
      stringBuilder1.AppendLine("3D textures: " + SystemInfo.supports3DTextures.ToString());
      stringBuilder1.AppendLine("Shadows: " + SystemInfo.supportsShadows.ToString());
      stringBuilder1.AppendLine("Compute: " + SystemInfo.supportsComputeShaders.ToString());
      stringBuilder1.AppendLine("CPU: " + SystemInfo.processorType);
      stringBuilder1.AppendLine("Core count: " + SystemInfo.processorCount.ToString());
      stringBuilder1.AppendLine("Platform: " + Application.platform.ToString());
      stringBuilder1.AppendLine("Screen resolution: " + Screen.currentResolution.width.ToString() + "x" + Screen.currentResolution.height.ToString() + "x" + ((int) Screen.currentResolution.refreshRateRatio.value).ToString());
      StringBuilder stringBuilder2 = stringBuilder1;
      int num = Screen.width;
      string str1 = num.ToString();
      num = Screen.height;
      string str2 = num.ToString();
      string str3 = "Window resolution: " + str1 + "x" + str2;
      stringBuilder2.AppendLine(str3);
      stringBuilder1.AppendLine("DPI: " + Screen.dpi.ToString());
      stringBuilder1.AppendLine("Rendering Threading Mode: " + SystemInfo.renderingThreadingMode.ToString());
      stringBuilder1.AppendLine("CLR: " + System.Environment.Version?.ToString());
      stringBuilder1.AppendLine("Modding runtime: " + GameManager.s_ModdingRuntime);
      System.Type type = System.Type.GetType("Mono.Runtime");
      if (type != (System.Type) null)
      {
        MethodInfo method = type.GetMethod("GetDisplayName", BindingFlags.Static | BindingFlags.NonPublic);
        if (method != (MethodInfo) null)
          stringBuilder1.AppendLine("Scripting runtime: Mono " + method.Invoke((object) null, (object[]) null)?.ToString());
      }
      return stringBuilder1.ToString().TrimEnd();
    }

    private static System.Collections.Generic.Dictionary<string, string> GetDefaultBacktraceAttributes()
    {
      return new System.Collections.Generic.Dictionary<string, string>()
      {
        ["game.version"] = Game.Version.current.fullVersion,
        ["cohtml.version"] = Versioning.Build.ToString(),
        ["pdxsdk.version"] = SDKVersion.Version,
        ["atl.version"] = ATL.Version.getVersion(),
        ["game.moddingRuntime"] = GameManager.s_ModdingRuntime
      };
    }

    private static void ListHarmonyPatches()
    {
      ILog logger = LogManager.GetLogger("Modding");
      logger.InfoFormat("Modding runtime: {0}", (object) GameManager.s_ModdingRuntime);
      try
      {
        GameManager.LocalTypeCache typeCache = new GameManager.LocalTypeCache();
        Assembly assembly = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => a.GetName().Name.Contains("Harmony")));
        if (assembly == (Assembly) null)
          return;
        GameManager.log.Info((object) "Harmony found.");
        System.Type type1 = assembly.GetType("Harmony.HarmonyInstance", false);
        if ((object) type1 == null)
          type1 = assembly.GetType("HarmonyLib.Harmony", false);
        System.Type type2 = type1;
        if (type2 == (System.Type) null)
        {
          logger.Info((object) "HarmonyInstance/Harmony class not found.");
        }
        else
        {
          MethodInfo method1 = typeCache.GetMethod(type2, "GetAllPatchedMethods", BindingFlags.Static | BindingFlags.Public);
          if (method1 == (MethodInfo) null)
            logger.Info((object) "Method GetAllPatchedMethods not found.");
          else if (!(method1.Invoke((object) null, (object[]) null) is IEnumerable<MethodBase> methodBases))
          {
            logger.Info((object) "No patched methods found.");
          }
          else
          {
            MethodInfo method2 = typeCache.GetMethod(type2, "GetPatchInfo", BindingFlags.Static | BindingFlags.Public);
            if (method2 == (MethodInfo) null)
            {
              logger.Info((object) "Method GetPatchInfo not found.");
            }
            else
            {
              System.Type type3 = assembly.GetType("HarmonyLib.Patches", false);
              if (type3 == (System.Type) null)
              {
                logger.Info((object) "Patches class not found.");
              }
              else
              {
                foreach (MethodBase methodBase in methodBases)
                {
                  logger.InfoFormat("Patched Method: {0}.{1}", (object) (methodBase.DeclaringType?.FullName ?? "<Global Type>"), (object) methodBase.Name);
                  object patchInfo = method2.Invoke((object) null, new object[1]
                  {
                    (object) methodBase
                  });
                  GameManager.PrintPatchDetails(logger, patchInfo, type3, typeCache);
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        GameManager.log.Warn(ex, (object) "ListHarmonyPatches failed");
      }
    }

    private static void PrintPatchDetails(
      ILog moddingLog,
      object patchInfo,
      System.Type patchInfoType,
      GameManager.LocalTypeCache typeCache)
    {
      if (patchInfo == null)
        return;
      FieldInfo field1 = typeCache.GetField(patchInfoType, "Prefixes");
      FieldInfo field2 = typeCache.GetField(patchInfoType, "Postfixes");
      FieldInfo field3 = typeCache.GetField(patchInfoType, "Transpilers");
      FieldInfo field4 = typeCache.GetField(patchInfoType, "Finalizers");
      IEnumerable<object> patches1 = field1?.GetValue(patchInfo) as IEnumerable<object>;
      IEnumerable<object> patches2 = field2?.GetValue(patchInfo) as IEnumerable<object>;
      IEnumerable<object> patches3 = field3?.GetValue(patchInfo) as IEnumerable<object>;
      IEnumerable<object> patches4 = field4?.GetValue(patchInfo) as IEnumerable<object>;
      GameManager.PrintIndividualPatches(moddingLog, "Prefixes", patches1, typeCache);
      GameManager.PrintIndividualPatches(moddingLog, "Postfixes", patches2, typeCache);
      GameManager.PrintIndividualPatches(moddingLog, "Transpilers", patches3, typeCache);
      GameManager.PrintIndividualPatches(moddingLog, "Finalizers", patches4, typeCache);
    }

    private static void PrintIndividualPatches(
      ILog moddingLog,
      string patchType,
      IEnumerable<object> patches,
      GameManager.LocalTypeCache typeCache)
    {
      if (patches == null || !patches.Any<object>())
        return;
      moddingLog.InfoFormat(" {0}:", (object) patchType);
      using (moddingLog.indent.scoped)
      {
        foreach (object patch in patches)
        {
          MethodBase methodBase = typeCache.GetProperty(patch.GetType(), "PatchMethod").GetValue(patch, (object[]) null) as MethodBase;
          if (methodBase != (MethodBase) null)
          {
            string p1 = methodBase.DeclaringType?.FullName ?? "<Global Method>";
            moddingLog.InfoFormat("Patch Method: {0}.{1}", (object) p1, (object) methodBase.Name);
          }
        }
      }
    }

    private static void DetectModdingRuntime()
    {
      GameManager.s_ModdingRuntime = GameManager.DetectModdingRuntimeName();
    }

    private static string DetectModdingRuntimeName()
    {
      try
      {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
          if (assembly.GetName().Name.Equals("BepInEx", StringComparison.OrdinalIgnoreCase))
            return string.Format("{0} {1}", (object) assembly.GetName().Name, (object) assembly.GetName().Version);
        }
        foreach (Assembly assembly in assemblies)
        {
          if (assembly.GetName().Name.Contains("BepInEx", StringComparison.OrdinalIgnoreCase))
            return string.Format("{0} {1}", (object) assembly.GetName().Name, (object) assembly.GetName().Version);
          if (((IEnumerable<System.Type>) assembly.GetTypes()).Any<System.Type>((Func<System.Type, bool>) (t => t.Namespace != null && t.Namespace.StartsWith("BepInEx"))))
            return string.Format("{0} {1}", (object) assembly.GetName().Name, (object) assembly.GetName().Version);
        }
        return "Builtin";
      }
      catch
      {
        return "Unknown";
      }
    }

    private static void SetNativeStackTrace()
    {
      Application.SetStackTraceLogType(LogType.Assert, StackTraceLogType.Full);
      Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.Full);
      Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);
      Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.Full);
      Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.Full);
      UnityEngine.Debug.Log((object) ("Game version: " + Game.Version.current.fullVersion));
      UnityEngine.Debug.Log((object) GameManager.GetSystemInfoString());
    }

    private bool HandleConfiguration()
    {
      if (this.configuration.showHelp != null)
      {
        Console.WriteLine(this.configuration.showHelp);
        return false;
      }
      if (!string.IsNullOrEmpty(this.configuration.profilerTarget))
      {
        Profiler.logFile = this.configuration.profilerTarget;
        Profiler.enableBinaryLog = true;
        Profiler.enabled = true;
      }
      return true;
    }

    private void HandleUserFolderVersion()
    {
      try
      {
        Colossal.Version version = Game.Version.current;
        string path = EnvPath.kUserDataPath + "/version";
        if (LongFile.Exists(path))
          version = new Colossal.Version(LongFile.ReadAllText(path));
        if (version < Game.Version.current)
        {
          UnityEngine.Debug.Log((object) ("Persistent folder version is outdated " + version.fullVersion + " (Game: " + Game.Version.current.fullVersion + ")"));
          if (version < new Colossal.Version("1.0.6f2"))
          {
            UnityEngine.Debug.Log((object) "User settings deleted due to outdated persistent folder version. Backups were created ending with ~");
            DeleteSettings(EnvPath.kUserDataPath);
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
          }
        }
        LongFile.WriteAllText(path, Game.Version.current.fullVersion);
      }
      catch (Exception ex)
      {
        GameManager.log.Error(ex);
      }

      static void DeleteSettings(string settingsPath)
      {
        foreach (FileInfo enumerateFile in new DirectoryInfo(settingsPath).EnumerateFiles("*", SearchOption.AllDirectories))
        {
          if (enumerateFile.Extension.ToLower() == ".coc")
          {
            string str = Path.ChangeExtension(enumerateFile.FullName, ".coc~");
            LongFile.Delete(str);
            enumerateFile.MoveTo(str);
          }
        }
      }
    }

    private void InitializeLocalization()
    {
      this.m_Cts.Token.ThrowIfCancellationRequested();
      this.localizationManager = new LocalizationManager("en-US", SystemLanguage.English, "English");
      this.localizationManager.LoadAvailableLocales();
    }

    private void CreateWorld()
    {
      this.m_Cts.Token.ThrowIfCancellationRequested();
      GameManager.log.Info((object) "Creating ECS world");
      CORuntimeApplication.Initialize();
      this.m_World = new World("Game");
      World.DefaultGameObjectInjectionWorld = this.m_World;
      this.m_PrefabSystem = this.m_World.GetOrCreateSystemManaged<PrefabSystem>();
      this.m_UpdateSystem = this.m_World.GetOrCreateSystemManaged<UpdateSystem>();
      this.m_DeserializationSystem = this.m_World.GetOrCreateSystemManaged<LoadGameSystem>();
      this.m_SerializationSystem = this.m_World.GetOrCreateSystemManaged<SaveGameSystem>();
    }

    private void CreateSystems()
    {
      this.m_Cts.Token.ThrowIfCancellationRequested();
      GameManager.log.Info((object) "Creating ECS systems");
      SystemOrder.Initialize(this.m_UpdateSystem);
      Game.PSI.Telemetry.gameplayData = new Game.PSI.Telemetry.GameplayData(this.m_World);
    }

    private void UpdateWorld()
    {
      if (!this.shouldUpdateWorld)
        return;
      CORuntimeApplication.ResetUpdateAllocator(this.m_World);
      this.m_UpdateSystem.Update(SystemUpdatePhase.MainLoop);
    }

    private void PostUpdateWorld()
    {
      if (!this.shouldUpdateWorld)
        return;
      this.m_UpdateSystem.Update(SystemUpdatePhase.Cleanup);
    }

    private void LateUpdateWorld()
    {
      if (!this.shouldUpdateWorld)
        return;
      this.m_UpdateSystem.Update(SystemUpdatePhase.LateUpdate);
      this.m_UpdateSystem.Update(SystemUpdatePhase.DebugGizmos);
      CORuntimeApplication.Update();
    }

    private void DestroyWorld()
    {
      Game.PSI.Telemetry.gameplayData = (Game.PSI.Telemetry.GameplayData) null;
      World.DisposeAllWorlds();
      CORuntimeApplication.Shutdown();
    }

    public void TakeScreenshot() => this.StartCoroutine(this.CaptureScreenshot());

    private IEnumerator CaptureScreenshot()
    {
      yield return (object) new WaitForEndOfFrame();
      ScreenUtility.CaptureScreenshot();
    }

    public class Configuration
    {
      public Colossal.Hash128 startGame;
      public bool disablePDXSDK;
      public bool noAssets;
      public bool noThumbnails;
      public string profilerTarget;
      public bool saveAllSettings;
      public bool cleanupSettings = true;
      public bool developerMode;
      public bool uiDeveloperMode;
      public bool qaDeveloperMode;
      public bool duplicateLogToDefault;
      public bool disableUserSection;
      public bool disableModding;
      public bool disableCodeModding;
      public GameManager.Configuration.StdoutCaptureMode captureStdout;
      public string showHelp;
      public bool continuelastsave;

      public override string ToString() => this.ToJSONString<GameManager.Configuration>();

      public void LogConfiguration()
      {
        GameManager.log.InfoFormat("Configuration: {0}", (object) this);
      }

      public enum StdoutCaptureMode
      {
        None,
        Console,
        CaptureOnly,
        Redirect,
      }
    }

    public enum State
    {
      Booting,
      Terminated,
      UIReady,
      WorldReady,
      Quitting,
      Loading,
    }

    public delegate void EventGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode);

    public delegate void EventGameSaveLoad(string saveName, bool start);

    public class LocalTypeCache
    {
      private readonly System.Collections.Generic.Dictionary<(System.Type type, string name, BindingFlags bindingFlags), MethodInfo> m_MethodCache = new System.Collections.Generic.Dictionary<(System.Type, string, BindingFlags), MethodInfo>();
      private readonly System.Collections.Generic.Dictionary<(System.Type type, string name), PropertyInfo> m_PropertyCache = new System.Collections.Generic.Dictionary<(System.Type, string), PropertyInfo>();
      private readonly System.Collections.Generic.Dictionary<(System.Type type, string name), FieldInfo> m_FieldCache = new System.Collections.Generic.Dictionary<(System.Type, string), FieldInfo>();

      public MethodInfo GetMethod(System.Type type, string methodName, BindingFlags bindingFlags)
      {
        (System.Type, string, BindingFlags) key = (type, methodName, bindingFlags);
        MethodInfo method;
        if (!this.m_MethodCache.TryGetValue(key, out method))
        {
          method = type.GetMethod(methodName, bindingFlags);
          this.m_MethodCache[key] = method;
        }
        return method;
      }

      public PropertyInfo GetProperty(System.Type type, string propertyName)
      {
        (System.Type, string) key = (type, propertyName);
        PropertyInfo property;
        if (!this.m_PropertyCache.TryGetValue(key, out property))
        {
          property = type.GetProperty(propertyName);
          this.m_PropertyCache[key] = property;
        }
        return property;
      }

      public FieldInfo GetField(System.Type type, string propertyName)
      {
        (System.Type, string) key = (type, propertyName);
        FieldInfo field;
        if (!this.m_FieldCache.TryGetValue(key, out field))
        {
          field = type.GetField(propertyName);
          this.m_FieldCache[key] = field;
        }
        return field;
      }
    }
  }
}
