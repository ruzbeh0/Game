// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.ToolchainDependencyManager
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO;
using Colossal.Logging;
using Colossal.Logging.Diagnostics;
using Colossal.PSI.Common;
using Colossal.PSI.Environment;
using Game.Modding.Toolchain.Dependencies;
using Game.PSI;
using Game.SceneFlow;
using Game.Settings;
using Game.UI;
using Game.UI.Localization;
using Game.UI.Menu;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Modding.Toolchain
{
  public class ToolchainDependencyManager : IEnumerable<IToolchainDependency>, IEnumerable
  {
    private const string kToolchain = "Toolchain";
    private const string kInstallingToolchain = "InstallingToolchain";
    private const string kUninstallingToolchain = "UninstallingToolchain";
    private const string kInstallingToolchainFailed = "InstallingToolchainFailed";
    private const string kUninstallingToolchainFailed = "InstallingToolchainFailed";
    public static readonly string kUserToolingPath = Path.Combine(EnvPath.kCacheDataPath, "Modding");
    public static readonly string kGameToolingPath = Path.Combine(EnvPath.kContentPath, "Game", ".ModdingToolchain");
    private readonly List<IToolchainDependency> m_Dependencies = new List<IToolchainDependency>();
    public static readonly ILog log = LogManager.GetLogger("Modding").SetShowsErrorsInUI(false);
    public static readonly MainDependency m_MainDependency = new MainDependency();
    public ToolchainDependencyManager.State m_State;
    private const string kInstalledKey = "SOFTWARE\\Colossal Order\\Cities Skylines II\\";
    private const string kInstalledValue = "ModdingToolchainInstalled";

    public bool isInProgress { get; private set; }

    public event Action<ToolchainDependencyManager.State> OnStateChanged;

    public IReadOnlyList<IToolchainDependency> dependencies
    {
      get => (IReadOnlyList<IToolchainDependency>) this.m_Dependencies;
    }

    public IEnumerator<IToolchainDependency> GetEnumerator()
    {
      return (IEnumerator<IToolchainDependency>) this.m_Dependencies.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public ToolchainDependencyManager.State cachedState
    {
      get => this.m_State;
      set
      {
        this.m_State = value;
        Action<ToolchainDependencyManager.State> onStateChanged = this.OnStateChanged;
        if (onStateChanged != null)
          onStateChanged(value);
        if (value.m_Status == ModdingToolStatus.Idle)
          return;
        NotificationSystem.Push("Toolchain", text: new LocalizedString?(value.GetLocalizedState(false)), titleId: "Toolchain", progressState: new ProgressState?(!value.m_Progress.HasValue ? ProgressState.Indeterminate : ProgressState.Progressing), progress: new int?(value.m_Progress.GetValueOrDefault()));
      }
    }

    public async Task<ToolchainDependencyManager.State> GetCurrentState()
    {
      ToolchainDependencyManager dependencyManager = this;
      // ISSUE: reference to a compiler-generated method
      DeploymentState state = await Task.Run<DeploymentState>(new Func<Task<DeploymentState>>(dependencyManager.\u003CGetCurrentState\u003Eb__25_0));
      dependencyManager.cachedState = dependencyManager.cachedState.WithState(state);
      return dependencyManager.cachedState;
    }

    private static bool isInstalled
    {
      get => SharedSettings.instance.modding.isInstalled;
      set => SharedSettings.instance.modding.isInstalled = value;
    }

    public void Register<T>() where T : IToolchainDependency, new()
    {
      this.m_Dependencies.Add((IToolchainDependency) new T());
    }

    public async Task Install(
      List<IToolchainDependency> dependenciesToInstall,
      CancellationToken token)
    {
      ToolchainDependencyManager dependencyManager = this;
      if (dependencyManager.isInProgress)
        return;
      ToolchainDependencyManager.State state;
      try
      {
        dependencyManager.isInProgress = true;
        ToolchainDependencyManager.log.Info((object) "Start modding toolchain installation");
        NotificationSystem.Push("Toolchain", titleId: "Toolchain", textId: "InstallingToolchain", progressState: new ProgressState?(ProgressState.Indeterminate), onClicked: new Action(ToolchainDependencyManager.OpenOptions));
        state = dependencyManager.cachedState.WithStatus(ModdingToolStatus.Installing);
        DeploymentState deploymentState = await dependencyManager.GetDeploymentState(token);
        dependencyManager.cachedState = state.WithState(deploymentState).WithStages(dependenciesToInstall.Count<IToolchainDependency>((Func<IToolchainDependency, bool>) (d => d.state.m_State != 0))).WithProgress(new int?());
        state = new ToolchainDependencyManager.State();
        dependenciesToInstall.Sort(new Comparison<IToolchainDependency>(IToolchainDependency.InstallSorting));
        List<IToolchainDependency.DiskSpaceRequirements> requirements = new List<IToolchainDependency.DiskSpaceRequirements>();
        foreach (IToolchainDependency toolchainDependency in dependenciesToInstall)
          requirements.AddRange((IEnumerable<IToolchainDependency.DiskSpaceRequirements>) toolchainDependency.spaceRequirements);
        string message;
        if (!ToolchainDependencyManager.CheckFreeSpace(requirements, out message))
          throw new ToolchainException(ToolchainError.NotEnoughSpace, (IToolchainDependency) null, message);
        foreach (IToolchainDependency toolchainDependency in dependenciesToInstall)
          toolchainDependency.state = (IToolchainDependency.State) DependencyState.Queued;
        foreach (IToolchainDependency dependency in dependenciesToInstall)
        {
          dependencyManager.cachedState = dependencyManager.cachedState.WithNextStage();
          dependency.onNotifyProgress += new IToolchainDependency.ProgressDelegate(dependencyManager.SetProgress);
          if (dependency.needDownload)
          {
            dependency.state = (IToolchainDependency.State) DependencyState.Downloading;
            await dependency.Download(token);
          }
          dependency.state = (IToolchainDependency.State) DependencyState.Installing;
          await dependency.Install(token);
          dependency.onNotifyProgress -= new IToolchainDependency.ProgressDelegate(dependencyManager.SetProgress);
          ToolchainDependencyManager.UserEnvironmentVariableManager.SetEnvVars(dependency.envVariables.ToArray<string>());
          await dependency.Refresh(token);
        }
        ToolchainDependencyManager.isInstalled = true;
        NotificationSystem.Pop("Toolchain", 1f, titleId: "Toolchain", textId: "InstallingToolchain", progressState: new ProgressState?(ProgressState.Complete));
      }
      catch (OperationCanceledException ex)
      {
        ToolchainDependencyManager.log.Info((object) "Installation canceled");
        SetFailedNotification();
      }
      catch (AggregateException ex1)
      {
        foreach (Exception innerException in ex1.InnerExceptions)
        {
          if (innerException is ToolchainException ex2)
            ProcessToolchainException(ex2);
          else
            ProcessException(innerException);
        }
        SetFailedNotification();
      }
      catch (ToolchainException ex)
      {
        ProcessToolchainException(ex);
        SetFailedNotification();
      }
      catch (Exception ex)
      {
        ProcessException(ex);
        SetFailedNotification();
      }
      finally
      {
        state = dependencyManager.cachedState.WithStatus(ModdingToolStatus.Idle);
        DeploymentState deploymentState = await dependencyManager.GetDeploymentState(token, true, false);
        dependencyManager.cachedState = state.WithState(deploymentState).WithStages(0).WithProgress(new int?());
        state = new ToolchainDependencyManager.State();
        dependencyManager.isInProgress = false;
      }

      static void ProcessToolchainException(ToolchainException ex)
      {
        switch (ex.error)
        {
          case ToolchainError.NotEnoughSpace:
            ToolchainDependencyManager.log.Error((Exception) null, (object) ("Not enough space on disk to install modding toolchain:\n" + ex.Message));
            ToolchainDependencyManager.ShowErrorDialog(new LocalizedString(string.IsNullOrEmpty(ex.Message) ? "Options.ERROR_TOOLCHAIN_NO_SPACE" : "Options.ERROR_TOOLCHAIN_NO_SPACE_DETAILS", (string) null, (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
            {
              {
                "DETAILS",
                (ILocElement) LocalizedString.Value(ex.Message)
              }
            }));
            break;
          case ToolchainError.Download:
            ToolchainDependencyManager.log.Error(ex.InnerException, (object) string.Format("Error while downloading dependency \"{0}\": {1}", (object) ex.source.localizedName, (object) ex.Message));
            ToolchainDependencyManager.ShowErrorDialog(new LocalizedString(string.IsNullOrEmpty(ex.Message) ? "Options.ERROR_TOOLCHAIN_DEPENDENCY_DOWNLOAD" : "Options.ERROR_TOOLCHAIN_DEPENDENCY_DOWNLOAD_DETAILS", (string) null, (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
            {
              {
                "DEPENDENCY_NAME",
                (ILocElement) ex.source.localizedName
              },
              {
                "DETAILS",
                (ILocElement) LocalizedString.Value(ex.Message)
              }
            }), ex.InnerException);
            break;
          case ToolchainError.Install:
            ToolchainDependencyManager.log.Error(ex.InnerException, (object) string.Format("Error while installing dependency \"{0}\": {1}", (object) ex.source, (object) ex.Message));
            ToolchainDependencyManager.ShowErrorDialog(new LocalizedString(string.IsNullOrEmpty(ex.Message) ? "Options.ERROR_TOOLCHAIN_DEPENDENCY_INSTALL" : "Options.ERROR_TOOLCHAIN_DEPENDENCY_INSTALL_DETAILS", (string) null, (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
            {
              {
                "DEPENDENCY_NAME",
                (ILocElement) ex.source.localizedName
              },
              {
                "DETAILS",
                (ILocElement) LocalizedString.Value(ex.Message)
              }
            }), ex.InnerException);
            break;
        }
      }

      static void ProcessException(Exception ex)
      {
        ToolchainDependencyManager.log.Error(ex, (object) "Unknown error while modding toolchain installation");
        ToolchainDependencyManager.ShowErrorDialog(LocalizedString.Id("Options.ERROR_TOOLCHAIN_INSTALL_UNKNOWN"), ex);
      }

      static void SetFailedNotification()
      {
        NotificationSystem.Pop("Toolchain", 5f, textId: "InstallingToolchainFailed", progressState: new ProgressState?(ProgressState.Failed));
      }
    }

    public async Task Uninstall(
      List<IToolchainDependency> dependenciesToUninstall,
      CancellationToken token)
    {
      ToolchainDependencyManager dependencyManager = this;
      if (dependencyManager.isInProgress)
        return;
      ToolchainDependencyManager.State state;
      try
      {
        dependencyManager.isInProgress = true;
        ToolchainDependencyManager.log.Info((object) "Start modding toolchain uninstallation");
        NotificationSystem.Push("Toolchain", titleId: "Toolchain", textId: "UninstallingToolchain", progressState: new ProgressState?(ProgressState.Indeterminate), onClicked: new Action(ToolchainDependencyManager.OpenOptions));
        state = dependencyManager.cachedState.WithStatus(ModdingToolStatus.Uninstalling);
        DeploymentState deploymentState = await dependencyManager.GetDeploymentState(token);
        dependencyManager.cachedState = state.WithState(deploymentState).WithStages(dependenciesToUninstall.Count<IToolchainDependency>((Func<IToolchainDependency, bool>) (d => d.state.m_State != DependencyState.NotInstalled))).WithProgress(new int?());
        state = new ToolchainDependencyManager.State();
        dependenciesToUninstall.Sort(new Comparison<IToolchainDependency>(IToolchainDependency.UninstallSorting));
        foreach (IToolchainDependency toolchainDependency in dependenciesToUninstall)
          toolchainDependency.state = (IToolchainDependency.State) DependencyState.Queued;
        foreach (IToolchainDependency dependency in dependenciesToUninstall)
        {
          dependencyManager.cachedState = dependencyManager.cachedState.WithNextStage();
          dependency.onNotifyProgress += new IToolchainDependency.ProgressDelegate(dependencyManager.SetProgress);
          bool flag = !dependency.confirmUninstallation;
          if (!flag)
          {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            LocalizedString message = dependency.uninstallMessage;
            if (message.Equals(new LocalizedString()))
              message = new LocalizedString("Options.WARN_TOOLCHAIN_DEPENDENCY_UNINSTALL", (string) null, (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
              {
                {
                  "DEPENDENCY_NAME",
                  (ILocElement) dependency.localizedName
                }
              });
            GameManager.instance.userInterface.appBindings.ShowConfirmationDialog(new Game.UI.ConfirmationDialog(new LocalizedString?((LocalizedString) "Common.DIALOG_TITLE[Warning]"), message, (LocalizedString) "Common.DIALOG_ACTION[Yes]", new LocalizedString?((LocalizedString) "Common.DIALOG_ACTION[No]"), Array.Empty<LocalizedString>()), (Action<int>) (msg => tcs.SetResult(msg == 0)));
            flag = await tcs.Task;
          }
          if (flag)
          {
            dependency.state = (IToolchainDependency.State) DependencyState.Removing;
            await dependency.Uninstall(token);
          }
          dependency.onNotifyProgress -= new IToolchainDependency.ProgressDelegate(dependencyManager.SetProgress);
          await dependency.Refresh(token);
        }
        ToolchainDependencyManager.UserEnvironmentVariableManager.RemoveEnvVars(dependencyManager.dependencies.Where<IToolchainDependency>((Func<IToolchainDependency, bool>) (d => d.state.m_State != DependencyState.NotInstalled)).SelectMany<IToolchainDependency, string>((Func<IToolchainDependency, IEnumerable<string>>) (d => d.envVariables)).Distinct<string>().ToArray<string>());
        NotificationSystem.Pop("Toolchain", 1f, titleId: "Toolchain", textId: "UninstallingToolchain", progressState: new ProgressState?(ProgressState.Complete));
      }
      catch (OperationCanceledException ex)
      {
        ToolchainDependencyManager.log.Info((object) "Uninstallation canceled");
        SetFailedNotification();
      }
      catch (AggregateException ex1)
      {
        foreach (Exception innerException in ex1.InnerExceptions)
        {
          if (innerException is ToolchainException ex2)
            ProcessToolchainException(ex2);
          else
            ProcessException(innerException);
        }
        SetFailedNotification();
      }
      catch (ToolchainException ex)
      {
        ProcessToolchainException(ex);
        SetFailedNotification();
      }
      catch (Exception ex)
      {
        ProcessException(ex);
        SetFailedNotification();
      }
      finally
      {
        state = dependencyManager.cachedState.WithStatus(ModdingToolStatus.Idle);
        DeploymentState deploymentState = await dependencyManager.GetDeploymentState(token, true, false);
        dependencyManager.cachedState = state.WithState(deploymentState).WithStages(0).WithProgress(new int?());
        state = new ToolchainDependencyManager.State();
        dependencyManager.isInProgress = false;
      }

      static void ProcessToolchainException(ToolchainException ex)
      {
        ToolchainDependencyManager.log.Error(ex.InnerException, (object) string.Format("Error while uninstalling dependency \"{0}\": {1}", (object) ex.source, (object) ex.Message));
        ToolchainDependencyManager.ShowErrorDialog(new LocalizedString(string.IsNullOrEmpty(ex.Message) ? "Options.ERROR_TOOLCHAIN_DEPENDENCY_UNINSTALL" : "Options.ERROR_TOOLCHAIN_DEPENDENCY_UNINSTALL_DETAILS", (string) null, (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
        {
          {
            "DEPENDENCY_NAME",
            (ILocElement) ex.source.localizedName
          },
          {
            "DETAILS",
            (ILocElement) LocalizedString.Value(ex.Message)
          }
        }), ex.InnerException);
      }

      static void ProcessException(Exception ex)
      {
        ToolchainDependencyManager.log.Error(ex, (object) "Unknown error while modding toolchain uninstallation");
        ToolchainDependencyManager.ShowErrorDialog(LocalizedString.Id("Options.ERROR_TOOLCHAIN_UNINSTALL_UNKNOWN"), ex);
      }

      static void SetFailedNotification()
      {
        NotificationSystem.Pop("Toolchain", 5f, textId: "InstallingToolchainFailed", progressState: new ProgressState?(ProgressState.Failed));
      }
    }

    private static void OpenOptions()
    {
      // ISSUE: reference to a compiler-generated method
      World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<OptionsUISystem>()?.OpenPage("Modding", "General", false);
    }

    private static void ShowErrorDialog(LocalizedString message, Exception ex = null)
    {
      ErrorDialogManager.ShowErrorDialog(new Game.UI.ErrorDialog()
      {
        localizedTitle = LocalizedString.Id("Options.ERROR_TOOLCHAIN"),
        localizedMessage = message,
        actions = Game.UI.ErrorDialog.Actions.None,
        severity = Game.UI.ErrorDialog.Severity.Error,
        errorDetails = ex != null ? StackTraceHelper.ExtractStackTraceFromException(ex) : (string) null
      });
    }

    private void SetProgress(
      IToolchainDependency dependency,
      IToolchainDependency.State dependencyState)
    {
      this.cachedState = this.cachedState.WithProgress(dependencyState.m_Progress, dependencyState.m_Details);
    }

    private async Task<DeploymentState> GetDeploymentState(
      CancellationToken token,
      bool forceRefresh = false,
      bool throwException = true)
    {
      try
      {
        token.ThrowIfCancellationRequested();
        bool isAnyDependencyOutdated = false;
        bool isAnyDependencyNotInstalled = false;
        IToolchainDependency.UpdateProcessEnvVarPathValue();
        await Task.WhenAll((forceRefresh ? (IEnumerable<IToolchainDependency>) this.m_Dependencies : this.m_Dependencies.Where<IToolchainDependency>((Func<IToolchainDependency, bool>) (d => (DependencyState) d.state == DependencyState.Unknown))).Select<IToolchainDependency, Task>((Func<IToolchainDependency, Task>) (d => d.Refresh(token))));
        foreach (IToolchainDependency dependency in this.m_Dependencies)
        {
          switch (dependency.state.m_State)
          {
            case DependencyState.NotInstalled:
              isAnyDependencyNotInstalled = true;
              continue;
            case DependencyState.Outdated:
              isAnyDependencyOutdated = true;
              continue;
            default:
              continue;
          }
        }
        return ToolchainDependencyManager.isInstalled ? (!isAnyDependencyNotInstalled ? (!isAnyDependencyOutdated ? DeploymentState.Installed : DeploymentState.Outdated) : DeploymentState.Invalid) : DeploymentState.NotInstalled;
      }
      catch (OperationCanceledException ex)
      {
        return DeploymentState.Unknown;
      }
      catch (Exception ex)
      {
        ToolchainDependencyManager.log.Warn(ex, (object) "Exception occured during GetDeploymentState");
        if (!throwException)
          return DeploymentState.Unknown;
        throw;
      }
    }

    private static bool CheckFreeSpace(
      List<IToolchainDependency.DiskSpaceRequirements> requirements,
      out string message)
    {
      Dictionary<string, long> dictionary = new Dictionary<string, long>();
      foreach (IToolchainDependency.DiskSpaceRequirements requirement in requirements)
      {
        string pathRoot = Path.GetPathRoot(Path.GetFullPath(requirement.m_Path));
        if (!dictionary.ContainsKey(pathRoot))
          dictionary[pathRoot] = 0L;
        dictionary[pathRoot] += requirement.m_Size;
      }
      List<string> values = new List<string>();
      foreach (KeyValuePair<string, long> keyValuePair in dictionary)
      {
        long available;
        IOUtils.GetStorageStatus(keyValuePair.Key, out long _, out available);
        if (available < keyValuePair.Value)
        {
          if (keyValuePair.Value < 1000L)
            values.Add(string.Format("Disk {0}: {1}B", (object) keyValuePair.Key[0], (object) keyValuePair.Value));
          else if (keyValuePair.Value < 1000000L)
            values.Add(string.Format("Disk {0}: {1:F1}KB", (object) keyValuePair.Key[0], (object) (float) ((double) math.ceil((float) keyValuePair.Value / 100f) / 10.0)));
          else if (keyValuePair.Value < 1000000000L)
            values.Add(string.Format("Disk {0}: {1:F1}MB", (object) keyValuePair.Key[0], (object) (float) ((double) math.ceil((float) keyValuePair.Value / 100000f) / 10.0)));
          else
            values.Add(string.Format("Disk {0}: {1:F1}GB", (object) keyValuePair.Key[0], (object) (float) ((double) math.ceil((float) keyValuePair.Value / 1E+08f) / 10.0)));
        }
      }
      message = string.Join("\n", (IEnumerable<string>) values);
      return values.Count == 0;
    }

    internal static class UserEnvironmentVariableManager
    {
      private const string kRegistryKeyPath = "Environment";
      private const uint WM_SETTINGCHANGE = 26;
      private static readonly IntPtr HWND_BROADCAST = (IntPtr) (int) ushort.MaxValue;

      [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
      private static extern bool SendNotifyMessage(
        IntPtr hWnd,
        uint Msg,
        UIntPtr wParam,
        string lParam);

      public static void BroadcastUserEnvironmentVariableChange()
      {
        ToolchainDependencyManager.UserEnvironmentVariableManager.SendNotifyMessage(ToolchainDependencyManager.UserEnvironmentVariableManager.HWND_BROADCAST, 26U, UIntPtr.Zero, "Environment");
      }

      public static void SetUserEnvironmentVariableNoBroadcast(string variableName, string value)
      {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Environment", true))
        {
          if (registryKey == null)
            throw new InvalidOperationException("Failed to open registry key: HKEY_CURRENT_USER\\Environment");
          registryKey.SetValue(variableName, (object) value, RegistryValueKind.String);
        }
      }

      public static void RemoveUserEnvironmentVariableNoBroadcast(string variableName)
      {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Environment", true))
        {
          if (registryKey == null)
            throw new InvalidOperationException("Failed to open registry key: HKEY_CURRENT_USER\\Environment");
          if (registryKey.GetValue(variableName) == null)
            return;
          registryKey.DeleteValue(variableName);
        }
      }

      public static void SetEnvVars(params string[] requiredEnvVariables)
      {
        if (requiredEnvVariables.Length == 0)
          return;
        foreach (string str1 in ((IEnumerable<string>) requiredEnvVariables).Append<string>("CSII_PATHSET"))
        {
          string str2;
          if (IToolchainDependency.envVars.TryGetValue(str1, out str2))
          {
            ToolchainDependencyManager.log.Debug((object) ("Set environment variable " + str1 + "=" + str2));
            ToolchainDependencyManager.UserEnvironmentVariableManager.SetUserEnvironmentVariableNoBroadcast(str1, str2);
          }
        }
        ToolchainDependencyManager.UserEnvironmentVariableManager.BroadcastUserEnvironmentVariableChange();
      }

      public static void RemoveEnvVars(params string[] requiredEnvVariables)
      {
        foreach (string variableName in IToolchainDependency.envVars.Keys.Except<string>(requiredEnvVariables.Length == 0 ? (IEnumerable<string>) requiredEnvVariables : ((IEnumerable<string>) requiredEnvVariables).Append<string>("CSII_PATHSET")))
        {
          if (!((IEnumerable<string>) requiredEnvVariables).Contains<string>(variableName))
          {
            ToolchainDependencyManager.log.Debug((object) ("Remove environment variable " + variableName));
            ToolchainDependencyManager.UserEnvironmentVariableManager.RemoveUserEnvironmentVariableNoBroadcast(variableName);
          }
        }
        ToolchainDependencyManager.UserEnvironmentVariableManager.BroadcastUserEnvironmentVariableChange();
      }
    }

    public struct State
    {
      public ModdingToolStatus m_Status;
      public DeploymentState m_State;
      public int m_CurrentStage;
      public int m_TotalStages;
      public int? m_Progress;
      public LocalizedString m_Details;

      public IToolchainDependency.State toDependencyState
      {
        get
        {
          DependencyState state;
          if (this.m_Status == ModdingToolStatus.Installing)
            state = DependencyState.Installing;
          else if (this.m_Status == ModdingToolStatus.Uninstalling)
          {
            state = DependencyState.Removing;
          }
          else
          {
            DependencyState dependencyState;
            switch (this.m_State)
            {
              case DeploymentState.Unknown:
                dependencyState = DependencyState.Unknown;
                break;
              case DeploymentState.Installed:
                dependencyState = DependencyState.Installed;
                break;
              case DeploymentState.NotInstalled:
                dependencyState = DependencyState.NotInstalled;
                break;
              case DeploymentState.Outdated:
                dependencyState = DependencyState.Outdated;
                break;
              case DeploymentState.Invalid:
                dependencyState = DependencyState.Unknown;
                break;
              default:
                dependencyState = DependencyState.Unknown;
                break;
            }
            state = dependencyState;
          }
          return new IToolchainDependency.State(state, progress: this.m_Progress);
        }
      }

      public ToolchainDependencyManager.State WithStatus(ModdingToolStatus status)
      {
        this.m_Status = status;
        return this;
      }

      public ToolchainDependencyManager.State WithState(DeploymentState state)
      {
        this.m_State = state;
        return this;
      }

      public ToolchainDependencyManager.State WithStages(int stages)
      {
        this.m_TotalStages = stages;
        this.m_CurrentStage = 0;
        return this;
      }

      public ToolchainDependencyManager.State WithNextStage()
      {
        this.m_CurrentStage = Math.Min(this.m_CurrentStage + 1, this.m_TotalStages);
        this.m_Progress = new int?();
        this.m_Details = (LocalizedString) (string) null;
        return this;
      }

      public ToolchainDependencyManager.State WithProgress(int? progress, LocalizedString details = default (LocalizedString))
      {
        this.m_Progress = progress;
        this.m_Details = details;
        return this;
      }

      public override int GetHashCode()
      {
        return HashCode.Combine<DeploymentState, int, int, int?, LocalizedString>(this.m_State, this.m_CurrentStage, this.m_TotalStages, this.m_Progress, this.m_Details);
      }

      public LocalizedString GetLocalizedState(bool includeProgress)
      {
        if (this.m_Status == ModdingToolStatus.Idle)
          return LocalizedString.Id(string.Format("Options.STATE_TOOLCHAIN[{0}]", (object) this.m_State));
        return new LocalizedString((string) null, !this.m_Progress.HasValue || !includeProgress ? "[{CURRENT_STAGE}/{TOTAL_STAGES}] {DETAILS}" : "[{CURRENT_STAGE}/{TOTAL_STAGES}] {DETAILS} {PROGRESS}%", (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
        {
          {
            "CURRENT_STAGE",
            (ILocElement) LocalizedString.Value(this.m_CurrentStage.ToString())
          },
          {
            "TOTAL_STAGES",
            (ILocElement) LocalizedString.Value(this.m_TotalStages.ToString())
          },
          {
            "DETAILS",
            (ILocElement) this.m_Details
          },
          {
            "PROGRESS",
            (ILocElement) LocalizedString.Value(this.m_Progress.ToString())
          }
        });
      }

      public static implicit operator DeploymentState(ToolchainDependencyManager.State state)
      {
        return state.m_State;
      }

      public static explicit operator ToolchainDependencyManager.State(DeploymentState state)
      {
        return new ToolchainDependencyManager.State()
        {
          m_State = state
        };
      }
    }

    [DebuggerDisplay("{m_Dependency} {result}")]
    public class DependencyFilter
    {
      public readonly DeploymentAction m_Action;
      public readonly IToolchainDependency m_Dependency;
      public readonly HashSet<Type> m_DependsOn = new HashSet<Type>();
      public readonly HashSet<Type> m_IsDependencyOf = new HashSet<Type>();

      public ToolchainDependencyManager.FilterResult result { get; private set; }

      public DependencyFilter(DeploymentAction action, IToolchainDependency dependency)
      {
        this.m_Action = action;
        this.m_Dependency = dependency;
      }

      public static (List<IToolchainDependency> accepted, List<IToolchainDependency> discarded) Process(
        DeploymentAction action,
        List<IToolchainDependency> dependenciesToFilter)
      {
        if (dependenciesToFilter == null)
          dependenciesToFilter = ToolchainDeployment.dependencyManager.m_Dependencies;
        Dictionary<Type, ToolchainDependencyManager.DependencyFilter> filters = new Dictionary<Type, ToolchainDependencyManager.DependencyFilter>();
        for (int index = 0; index < dependenciesToFilter.Count; ++index)
        {
          IToolchainDependency dependency = dependenciesToFilter[index];
          ToolchainDependencyManager.DependencyFilter dependencyFilter = new ToolchainDependencyManager.DependencyFilter(action, dependency);
          filters[dependency.GetType()] = dependencyFilter;
          switch (action)
          {
            case DeploymentAction.Install:
            case DeploymentAction.Update:
            case DeploymentAction.Repair:
              foreach (Type type in dependency.dependsOnInstallation)
              {
                Type subDependencyType = type;
                dependencyFilter.m_DependsOn.Add(subDependencyType);
                if (dependenciesToFilter.Find((Predicate<IToolchainDependency>) (d => d.GetType() == subDependencyType)) == null)
                {
                  IToolchainDependency toolchainDependency = ToolchainDeployment.dependencyManager.m_Dependencies.Find((Predicate<IToolchainDependency>) (d => d.GetType() == subDependencyType));
                  if (toolchainDependency != null)
                    dependenciesToFilter.Add(toolchainDependency);
                }
              }
              break;
            case DeploymentAction.Uninstall:
              using (IEnumerator<IToolchainDependency> enumerator = ToolchainDeployment.dependencyManager.m_Dependencies.Where<IToolchainDependency>((Func<IToolchainDependency, bool>) (d => ((IEnumerable<Type>) d.dependsOnUninstallation).Contains<Type>(dependency.GetType()))).GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  IToolchainDependency subDependency = enumerator.Current;
                  dependencyFilter.m_IsDependencyOf.Add(subDependency.GetType());
                  if (dependenciesToFilter.Find((Predicate<IToolchainDependency>) (d => d.GetType() == subDependency.GetType())) == null)
                    dependenciesToFilter.Add(subDependency);
                }
                break;
              }
          }
        }
        foreach (ToolchainDependencyManager.DependencyFilter dependencyFilter in filters.Values)
          dependencyFilter.SetBackwardDependencies(filters);
        foreach (ToolchainDependencyManager.DependencyFilter dependencyFilter in filters.Values)
          dependencyFilter.CheckIfCanBeProcessed(filters);
        foreach (ToolchainDependencyManager.DependencyFilter dependencyFilter in filters.Values)
          dependencyFilter.CheckIfNeedToBeProcessed(filters);
        List<IToolchainDependency> list1 = filters.Values.Where<ToolchainDependencyManager.DependencyFilter>((Func<ToolchainDependencyManager.DependencyFilter, bool>) (f => f.result != ToolchainDependencyManager.FilterResult.Complete && f.result != ToolchainDependencyManager.FilterResult.Invalid)).Select<ToolchainDependencyManager.DependencyFilter, IToolchainDependency>((Func<ToolchainDependencyManager.DependencyFilter, IToolchainDependency>) (f => f.m_Dependency)).ToList<IToolchainDependency>();
        List<IToolchainDependency> list2 = dependenciesToFilter.Except<IToolchainDependency>((IEnumerable<IToolchainDependency>) list1).ToList<IToolchainDependency>();
        return (list1, list2);
      }

      private void SetBackwardDependencies(
        Dictionary<Type, ToolchainDependencyManager.DependencyFilter> filters)
      {
        switch (this.m_Action)
        {
          case DeploymentAction.Install:
          case DeploymentAction.Update:
          case DeploymentAction.Repair:
            using (HashSet<Type>.Enumerator enumerator = this.m_DependsOn.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Type current = enumerator.Current;
                ToolchainDependencyManager.DependencyFilter dependencyFilter;
                if (filters.TryGetValue(current, out dependencyFilter))
                  dependencyFilter.m_IsDependencyOf.Add(this.m_Dependency.GetType());
              }
              break;
            }
          case DeploymentAction.Uninstall:
            using (HashSet<Type>.Enumerator enumerator = this.m_IsDependencyOf.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Type current = enumerator.Current;
                ToolchainDependencyManager.DependencyFilter dependencyFilter;
                if (filters.TryGetValue(current, out dependencyFilter))
                  dependencyFilter.m_DependsOn.Add(this.m_Dependency.GetType());
              }
              break;
            }
        }
      }

      private void CheckIfCanBeProcessed(
        Dictionary<Type, ToolchainDependencyManager.DependencyFilter> filters)
      {
        if (this.result != ToolchainDependencyManager.FilterResult.Unchecked)
          return;
        this.result = ToolchainDependencyManager.FilterResult.InProgress;
        switch (this.m_Action)
        {
          case DeploymentAction.Install:
          case DeploymentAction.Update:
          case DeploymentAction.Repair:
            if (this.m_Dependency.state.m_State != DependencyState.Installed && !this.m_Dependency.canBeInstalled)
            {
              this.result = ToolchainDependencyManager.FilterResult.Invalid;
              return;
            }
            if (this.m_Dependency.state.m_State == DependencyState.Installed)
            {
              this.result = ToolchainDependencyManager.FilterResult.Complete;
              return;
            }
            using (HashSet<Type>.Enumerator enumerator = this.m_DependsOn.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Type current = enumerator.Current;
                ToolchainDependencyManager.DependencyFilter dependencyFilter;
                if (!filters.TryGetValue(current, out dependencyFilter))
                {
                  this.result = ToolchainDependencyManager.FilterResult.Invalid;
                  return;
                }
                dependencyFilter.CheckIfCanBeProcessed(filters);
                if (dependencyFilter.result == ToolchainDependencyManager.FilterResult.Invalid)
                {
                  this.result = ToolchainDependencyManager.FilterResult.Invalid;
                  return;
                }
              }
              break;
            }
          case DeploymentAction.Uninstall:
            if (!this.m_Dependency.canBeUninstalled)
            {
              this.result = ToolchainDependencyManager.FilterResult.Invalid;
              return;
            }
            if (this.m_Dependency.state.m_State == DependencyState.NotInstalled)
            {
              this.result = ToolchainDependencyManager.FilterResult.Complete;
              return;
            }
            using (HashSet<Type>.Enumerator enumerator = this.m_IsDependencyOf.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Type current = enumerator.Current;
                ToolchainDependencyManager.DependencyFilter dependencyFilter;
                if (filters.TryGetValue(current, out dependencyFilter))
                  dependencyFilter.CheckIfCanBeProcessed(filters);
              }
              break;
            }
        }
        this.result = ToolchainDependencyManager.FilterResult.Valid;
      }

      private void CheckIfNeedToBeProcessed(
        Dictionary<Type, ToolchainDependencyManager.DependencyFilter> filters)
      {
        switch (this.m_Action)
        {
          case DeploymentAction.Install:
          case DeploymentAction.Update:
          case DeploymentAction.Repair:
            if (this.result != ToolchainDependencyManager.FilterResult.Invalid)
              break;
            using (HashSet<Type>.Enumerator enumerator = this.m_IsDependencyOf.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Type current = enumerator.Current;
                ToolchainDependencyManager.DependencyFilter dependencyFilter;
                if (filters.TryGetValue(current, out dependencyFilter))
                  dependencyFilter.result = ToolchainDependencyManager.FilterResult.Invalid;
              }
              break;
            }
          case DeploymentAction.Uninstall:
            if (this.result != ToolchainDependencyManager.FilterResult.Invalid)
              break;
            using (HashSet<Type>.Enumerator enumerator = this.m_IsDependencyOf.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Type current = enumerator.Current;
                ToolchainDependencyManager.DependencyFilter dependencyFilter;
                if (filters.TryGetValue(current, out dependencyFilter))
                  dependencyFilter.result = ToolchainDependencyManager.FilterResult.Invalid;
              }
              break;
            }
        }
      }
    }

    public enum FilterResult
    {
      Unchecked,
      Valid,
      Invalid,
      Complete,
      InProgress,
    }
  }
}
