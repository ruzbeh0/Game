// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.IToolchainDependency
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Logging;
using Colossal.PSI.Environment;
using Game.Modding.Toolchain.Dependencies;
using Game.UI.Localization;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Game.Modding.Toolchain
{
  public interface IToolchainDependency
  {
    const string CSII_PATHSET = "CSII_PATHSET";
    const string CSII_INSTALLATIONPATH = "CSII_INSTALLATIONPATH";
    const string CSII_USERDATAPATH = "CSII_USERDATAPATH";
    const string CSII_TOOLPATH = "CSII_TOOLPATH";
    const string CSII_LOCALMODSPATH = "CSII_LOCALMODSPATH";
    const string CSII_UNITYMODPROJECTPATH = "CSII_UNITYMODPROJECTPATH";
    const string CSII_UNITYVERSION = "CSII_UNITYVERSION";
    const string CSII_ENTITIESVERSION = "CSII_ENTITIESVERSION";
    const string CSII_MODPOSTPROCESSORPATH = "CSII_MODPOSTPROCESSORPATH";
    const string CSII_MODPUBLISHERPATH = "CSII_MODPUBLISHERPATH";
    const string CSII_MANAGEDPATH = "CSII_MANAGEDPATH";
    const string CSII_PDXCACHEPATH = "CSII_PDXCACHEPATH";
    const string CSII_PDXMODSPATH = "CSII_PDXMODSPATH";
    const string CSII_ASSEMBLYSEARCHPATH = "CSII_ASSEMBLYSEARCHPATH";
    const string kEntitiesVersion = "1.0.16";

    protected static ILog log => ToolchainDependencyManager.log;

    static async Task<long> GetDownloadSizeAsync(string url, CancellationToken token, int timeout = 3000)
    {
      using (CancellationTokenSource timeoutTokenSource = new CancellationTokenSource(timeout))
      {
        using (CancellationTokenSource combinedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, timeoutTokenSource.Token))
        {
          try
          {
            using (HttpClient client = new HttpClient())
            {
              HttpResponseMessage httpResponseMessage = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url), combinedTokenSource.Token).ConfigureAwait(false);
              if (httpResponseMessage.IsSuccessStatusCode)
              {
                long? contentLength = httpResponseMessage.Content.Headers.ContentLength;
                if (contentLength.HasValue)
                {
                  contentLength = httpResponseMessage.Content.Headers.ContentLength;
                  return contentLength.Value;
                }
              }
              return -1;
            }
          }
          catch (OperationCanceledException ex)
          {
            if (timeoutTokenSource.IsCancellationRequested)
              IToolchainDependency.log.Error((object) ("Get download size request timeout: " + url));
            return -1;
          }
          catch (Exception ex)
          {
            IToolchainDependency.log.Error(ex, (object) ("Get download size error: " + url));
            return -1;
          }
        }
      }
    }

    event IToolchainDependency.ProgressDelegate onNotifyProgress;

    string name { get; }

    LocalizedString localizedName { get; }

    string version { get; protected set; }

    IToolchainDependency.State state { get; set; }

    bool needDownload { get; protected set; }

    List<IToolchainDependency.DiskSpaceRequirements> spaceRequirements { get; protected set; }

    bool confirmUninstallation { get; }

    bool canBeInstalled { get; }

    bool canBeUninstalled { get; }

    string installationDirectory { get; set; }

    bool canChangeInstallationDirectory { get; }

    string icon { get; }

    DeploymentAction availableActions
    {
      get
      {
        DeploymentAction availableActions = DeploymentAction.None;
        if (this.installAvailable)
          availableActions |= DeploymentAction.Install;
        if (this.updateAvailable)
          availableActions |= DeploymentAction.Update;
        if (this.uninstallAvailable)
          availableActions |= DeploymentAction.Uninstall;
        return availableActions;
      }
    }

    bool installAvailable
    {
      get => this.canBeInstalled && this.state.m_State == DependencyState.NotInstalled;
    }

    bool uninstallAvailable
    {
      get => this.canBeUninstalled && this.state.m_State != DependencyState.NotInstalled;
    }

    bool updateAvailable => this.canBeInstalled && this.state.m_State == DependencyState.Outdated;

    LocalizedString description { get; }

    LocalizedString installDescr { get; }

    LocalizedString uninstallDescr { get; }

    LocalizedString uninstallMessage { get; }

    Type[] dependsOnInstallation { get; }

    Type[] dependsOnUninstallation { get; }

    Task<bool> IsInstalled(CancellationToken token);

    Task<bool> IsUpToDate(CancellationToken token);

    Task<bool> NeedDownload(CancellationToken token);

    Task Download(CancellationToken token);

    Task Install(CancellationToken token);

    Task Uninstall(CancellationToken token);

    Task<List<IToolchainDependency.DiskSpaceRequirements>> GetRequiredDiskSpace(
      CancellationToken token);

    IEnumerable<string> envVariables { get; }

    static bool CheckEnvVariables(IToolchainDependency dependency, bool checkValue = false)
    {
      return dependency.envVariables.All<string>((Func<string, bool>) (envVariable =>
      {
        string environmentVariable = System.Environment.GetEnvironmentVariable(envVariable, EnvironmentVariableTarget.User);
        return envVariable == "CSII_PATHSET" | checkValue ? IToolchainDependency.envVars[envVariable] == environmentVariable : environmentVariable != null;
      }));
    }

    Task Refresh(CancellationToken token);

    static async Task Refresh(IToolchainDependency dependency, CancellationToken token)
    {
      dependency.version = (string) null;
      if (!await dependency.IsInstalled(token))
        dependency.state = (IToolchainDependency.State) DependencyState.NotInstalled;
      else
        dependency.state = await dependency.IsUpToDate(token) ? (IToolchainDependency.CheckEnvVariables(dependency) ? (IToolchainDependency.State) DependencyState.Installed : (IToolchainDependency.State) DependencyState.Outdated) : (IToolchainDependency.State) DependencyState.Outdated;
      if ((DependencyState) dependency.state != DependencyState.Installed)
      {
        IToolchainDependency toolchainDependency = dependency;
        toolchainDependency.needDownload = await dependency.NeedDownload(token);
        toolchainDependency = (IToolchainDependency) null;
        toolchainDependency = dependency;
        toolchainDependency.spaceRequirements = await dependency.GetRequiredDiskSpace(token);
        toolchainDependency = (IToolchainDependency) null;
      }
      else
      {
        dependency.needDownload = false;
        dependency.spaceRequirements = new List<IToolchainDependency.DiskSpaceRequirements>();
      }
    }

    static int InstallSorting(IToolchainDependency x, IToolchainDependency y)
    {
      if (((IEnumerable<Type>) x.dependsOnInstallation).Contains<Type>(y.GetType()))
        return 1;
      return ((IEnumerable<Type>) y.dependsOnInstallation).Contains<Type>(x.GetType()) ? -1 : 0;
    }

    static int UninstallSorting(IToolchainDependency x, IToolchainDependency y)
    {
      if (((IEnumerable<Type>) x.dependsOnUninstallation).Contains<Type>(y.GetType()))
        return -1;
      return ((IEnumerable<Type>) y.dependsOnUninstallation).Contains<Type>(x.GetType()) ? 1 : 0;
    }

    LocalizedString GetLocalizedState(bool includeProgress)
    {
      return IToolchainDependency.GetLocalizedState(this.state, includeProgress);
    }

    LocalizedString GetLocalizedVersion() => LocalizedString.Value(this.version);

    static LocalizedString GetLocalizedState(IToolchainDependency.State state, bool includeProgress)
    {
      switch (state.m_State)
      {
        case DependencyState.Installing:
          if (state.m_Progress.HasValue)
            break;
          goto default;
        case DependencyState.Downloading:
          if (state.m_Progress.HasValue)
            break;
          goto default;
        case DependencyState.Removing:
          if (!state.m_Progress.HasValue)
            goto default;
          else
            break;
        default:
          return LocalizedString.Id(string.Format("Options.STATE_TOOLCHAIN[{0}]", (object) state.m_State));
      }
      return new LocalizedString((string) null, includeProgress ? "{STATE} {PROGRESS}%" : "{STATE}", (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
      {
        {
          "STATE",
          (ILocElement) LocalizedString.Id(string.Format("Options.STATE_TOOLCHAIN[{0}]", (object) state.m_State))
        },
        {
          "PROGRESS",
          (ILocElement) LocalizedString.Value(state.m_Progress.ToString())
        }
      });
    }

    static RegistryKey GetUninstaller(Dictionary<string, string> check, out string keyName)
    {
      return IToolchainDependency.GetUninstaller("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", check, out keyName) ?? IToolchainDependency.GetUninstaller("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall", check, out keyName) ?? (RegistryKey) null;
    }

    static RegistryKey GetUninstaller(
      string uninstallKeyName,
      Dictionary<string, string> check,
      out string keyName)
    {
      keyName = (string) null;
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(uninstallKeyName);
      if (registryKey == null)
        return (RegistryKey) null;
      foreach (string subKeyName in registryKey.GetSubKeyNames())
      {
        RegistryKey uninstaller = registryKey.OpenSubKey(subKeyName);
        if (uninstaller != null)
        {
          bool flag = false;
          foreach (KeyValuePair<string, string> keyValuePair in check)
          {
            if (uninstaller.GetValue(keyValuePair.Key) as string != keyValuePair.Value)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            keyName = subKeyName;
            return uninstaller;
          }
        }
      }
      return (RegistryKey) null;
    }

    static void UpdateProcessEnvVarPathValue()
    {
      HashSet<string> allVars = new HashSet<string>();
      Add(EnvironmentVariableTarget.Process);
      Add(EnvironmentVariableTarget.User);
      Add(EnvironmentVariableTarget.Machine);
      System.Environment.SetEnvironmentVariable("PATH", string.Join<string>(';', (IEnumerable<string>) allVars), EnvironmentVariableTarget.Process);

      sealed void Add(EnvironmentVariableTarget target)
      {
        string environmentVariable = System.Environment.GetEnvironmentVariable("PATH", target);
        if (environmentVariable == null)
          return;
        foreach (string str in environmentVariable.Split(';', StringSplitOptions.RemoveEmptyEntries))
          allVars.Add(str);
      }
    }

    static IReadOnlyDictionary<string, string> envVars { get; } = (IReadOnlyDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        nameof (CSII_INSTALLATIONPATH),
        System.Environment.CurrentDirectory
      },
      {
        nameof (CSII_USERDATAPATH),
        Path.GetFullPath(EnvPath.kUserDataPath)
      },
      {
        nameof (CSII_TOOLPATH),
        Path.GetFullPath(ToolchainDependencyManager.kUserToolingPath)
      },
      {
        nameof (CSII_LOCALMODSPATH),
        Path.GetFullPath(Path.Combine(EnvPath.kUserDataPath, "Mods"))
      },
      {
        nameof (CSII_UNITYMODPROJECTPATH),
        Path.GetFullPath(Path.Combine(ToolchainDependencyManager.kUserToolingPath, "UnityModsProject"))
      },
      {
        nameof (CSII_UNITYVERSION),
        UnityDependency.sUnityVersion
      },
      {
        nameof (CSII_ENTITIESVERSION),
        "1.0.16"
      },
      {
        nameof (CSII_MODPOSTPROCESSORPATH),
        Path.GetFullPath(Path.Combine(ToolchainDependencyManager.kGameToolingPath, "ModPostProcessor", "ModPostProcessor.exe"))
      },
      {
        nameof (CSII_MODPUBLISHERPATH),
        Path.GetFullPath(Path.Combine(ToolchainDependencyManager.kGameToolingPath, "ModPublisher", "ModPublisher.exe"))
      },
      {
        nameof (CSII_PDXCACHEPATH),
        Path.GetFullPath(Path.Combine(EnvPath.kUserDataPath, ".pdxsdk"))
      },
      {
        nameof (CSII_PDXMODSPATH),
        Path.GetFullPath(Path.Combine(EnvPath.kCacheDataPath, "Mods"))
      },
      {
        nameof (CSII_PATHSET),
        "Build"
      },
      {
        nameof (CSII_MANAGEDPATH),
        Path.Combine(System.Environment.CurrentDirectory, "Cities2_Data", "Managed")
      },
      {
        nameof (CSII_ASSEMBLYSEARCHPATH),
        string.Empty
      }
    };

    delegate void ProgressDelegate(
      IToolchainDependency dependency,
      IToolchainDependency.State state);

    [DebuggerDisplay("{m_Path}: {m_Size}")]
    struct DiskSpaceRequirements
    {
      public string m_Path;
      public long m_Size;
    }

    [DebuggerDisplay("{m_State}: {m_Progress}")]
    struct State
    {
      public DependencyState m_State;
      public int? m_Progress;
      public LocalizedString m_Details;

      public State(DependencyState state, string details = null, int? progress = null)
      {
        this.m_State = state;
        this.m_Progress = progress;
        this.m_Details = string.IsNullOrEmpty(details) ? new LocalizedString() : LocalizedString.Id("Options.STATE_TOOLCHAIN[" + details + "]");
      }

      public static implicit operator DependencyState(IToolchainDependency.State state)
      {
        return state.m_State;
      }

      public static explicit operator IToolchainDependency.State(DependencyState state)
      {
        return new IToolchainDependency.State(state);
      }

      public override int GetHashCode()
      {
        return HashCode.Combine<DependencyState, int?, LocalizedString>(this.m_State, this.m_Progress, this.m_Details);
      }
    }
  }
}
