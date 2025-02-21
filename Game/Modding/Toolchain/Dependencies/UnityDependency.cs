// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.Dependencies.UnityDependency
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using CliWrap;
using Colossal;
using Game.Settings;
using Game.UI.Localization;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
namespace Game.Modding.Toolchain.Dependencies
{
  public class UnityDependency : BaseDependency
  {
    public const string kUnityInstallerUrl = "https://download.unity3d.com/download_unity/c3ae09b9f03c/Windows64EditorInstaller/UnitySetup64-2022.3.44f1.exe";
    public static readonly string sUnityVersion = Application.unityVersion;
    public static readonly string kDefaultInstallationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
    public static readonly string kInstallationFolder = "Unity";
    private static string sUnityPath;
    private long? m_DownloadSize;
    private string m_InstallationDirectory;

    public override string name => "Unity editor";

    public override string icon => "Media/Toolchain/Unity.svg";

    public override string version
    {
      get => UnityDependency.sUnityVersion;
      protected set
      {
      }
    }

    public static string unityPath
    {
      get
      {
        if (string.IsNullOrEmpty(UnityDependency.sUnityPath))
        {
          string path1 = "SOFTWARE\\Unity Technologies\\Installer\\Unity " + UnityDependency.sUnityVersion + "\\";
          string path2 = "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Unity " + UnityDependency.sUnityVersion + "\\";
          int depth = 0;
          string path3;
          if (!UnityDependency.TryGetRegistryKeyValue(Registry.CurrentUser, path1, "Location x64", out path3))
          {
            depth = 2;
            if (!UnityDependency.TryGetRegistryKeyValue(Registry.LocalMachine, path2, "UninstallString", out path3))
              UnityDependency.TryGetRegistryKeyValue(Registry.LocalMachine, path2, "DisplayIcon", out path3);
          }
          string parentPath;
          if (UnityDependency.TryGetParentPath(path3, depth, out parentPath))
            UnityDependency.sUnityPath = parentPath;
        }
        return UnityDependency.sUnityPath;
      }
    }

    public static string unityExe
    {
      get
      {
        return UnityDependency.unityPath != null ? Path.Combine(UnityDependency.unityPath, "Editor", "Unity.exe") : (string) null;
      }
    }

    public static string unityUninstallerExe
    {
      get
      {
        return UnityDependency.sUnityPath != null ? Path.Combine(UnityDependency.sUnityPath, "Editor", "Uninstall.exe") : (string) null;
      }
    }

    public string installerPath
    {
      get
      {
        return Path.Combine(Path.GetFullPath(SharedSettings.instance.modding.downloadDirectory), Path.GetFileName("https://download.unity3d.com/download_unity/c3ae09b9f03c/Windows64EditorInstaller/UnitySetup64-2022.3.44f1.exe"));
      }
    }

    public override string installationDirectory
    {
      get => this.m_InstallationDirectory;
      set
      {
        this.m_InstallationDirectory = Path.GetFullPath(Path.Combine(value, UnityDependency.kInstallationFolder));
      }
    }

    public override bool canChangeInstallationDirectory => true;

    public override bool confirmUninstallation => true;

    public override LocalizedString installDescr
    {
      get
      {
        return new LocalizedString("Options.WARN_TOOLCHAIN_INSTALL_UNITY", (string) null, (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
        {
          {
            "UNITY_VERSION",
            (ILocElement) LocalizedString.Value(UnityDependency.sUnityVersion)
          },
          {
            "HOST",
            (ILocElement) LocalizedString.Value(new Uri("https://download.unity3d.com/download_unity/c3ae09b9f03c/Windows64EditorInstaller/UnitySetup64-2022.3.44f1.exe").Host)
          }
        });
      }
    }

    public override LocalizedString uninstallMessage
    {
      get
      {
        return new LocalizedString("Options.WARN_TOOLCHAIN_UNITY_UNINSTALL", (string) null, (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
        {
          {
            "UNITY_VERSION",
            (ILocElement) LocalizedString.Value(UnityDependency.sUnityVersion)
          }
        });
      }
    }

    public UnityDependency()
    {
      UnityDependency.sUnityPath = (string) null;
      this.installationDirectory = UnityDependency.kDefaultInstallationDirectory;
    }

    public override Task<bool> IsInstalled(CancellationToken token)
    {
      return Task.FromResult<bool>(UnityDependency.unityExe != null && LongFile.Exists(UnityDependency.unityExe));
    }

    public override Task<bool> IsUpToDate(CancellationToken token) => this.IsInstalled(token);

    public override async Task<bool> NeedDownload(CancellationToken token)
    {
      FileInfo installerFile = new FileInfo(this.installerPath);
      if (!installerFile.Exists)
        return true;
      if (installerFile.Length == await this.GetUnityInstallerSize(token).ConfigureAwait(false))
        return false;
      await AsyncUtils.DeleteFileAsync(this.installerPath, token).ConfigureAwait(false);
      return true;
    }

    private async Task<long> GetUnityInstallerSize(CancellationToken token)
    {
      this.m_DownloadSize.GetValueOrDefault();
      if (!this.m_DownloadSize.HasValue)
        this.m_DownloadSize = new long?(await IToolchainDependency.GetDownloadSizeAsync("https://download.unity3d.com/download_unity/c3ae09b9f03c/Windows64EditorInstaller/UnitySetup64-2022.3.44f1.exe", token).ConfigureAwait(false));
      return this.m_DownloadSize.Value;
    }

    public override async Task<List<IToolchainDependency.DiskSpaceRequirements>> GetRequiredDiskSpace(
      CancellationToken token)
    {
      UnityDependency unityDependency = this;
      List<IToolchainDependency.DiskSpaceRequirements> requests = new List<IToolchainDependency.DiskSpaceRequirements>();
      ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = unityDependency.IsInstalled(token).ConfigureAwait(false);
      if (!await configuredTaskAwaitable)
      {
        requests.Add(new IToolchainDependency.DiskSpaceRequirements()
        {
          m_Path = unityDependency.installationDirectory,
          m_Size = 6442450944L
        });
        configuredTaskAwaitable = unityDependency.NeedDownload(token).ConfigureAwait(false);
        if (await configuredTaskAwaitable)
        {
          List<IToolchainDependency.DiskSpaceRequirements> spaceRequirementsList = requests;
          IToolchainDependency.DiskSpaceRequirements spaceRequirements = new IToolchainDependency.DiskSpaceRequirements();
          spaceRequirements.m_Path = unityDependency.installerPath;
          spaceRequirements.m_Size = await unityDependency.GetUnityInstallerSize(token).ConfigureAwait(false);
          spaceRequirementsList.Add(spaceRequirements);
          spaceRequirementsList = (List<IToolchainDependency.DiskSpaceRequirements>) null;
          spaceRequirements = new IToolchainDependency.DiskSpaceRequirements();
        }
      }
      List<IToolchainDependency.DiskSpaceRequirements> requiredDiskSpace = requests;
      requests = (List<IToolchainDependency.DiskSpaceRequirements>) null;
      return requiredDiskSpace;
    }

    public override Task Download(CancellationToken token)
    {
      return BaseDependency.Download((BaseDependency) this, token, "https://download.unity3d.com/download_unity/c3ae09b9f03c/Windows64EditorInstaller/UnitySetup64-2022.3.44f1.exe", this.installerPath, "DownloadingUnity");
    }

    public override async Task Install(CancellationToken token)
    {
      UnityDependency source = this;
      token.ThrowIfCancellationRequested();
      string path = source.installerPath;
      try
      {
        IToolchainDependency.log.Debug((object) "Installing Unity");
        // ISSUE: explicit non-virtual call
        __nonvirtual (source.state) = new IToolchainDependency.State(DependencyState.Installing, "InstallingUnity");
        CommandResult commandResult = await Cli.Wrap(path).WithArguments("/S").WithStandardOutputPipe(PipeTarget.ToDelegate((Action<string>) (l => IToolchainDependency.log.Debug((object) l)))).WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => IToolchainDependency.log.Error((object) l)))).WithUseShellExecute(true).ExecuteAsync(token).ConfigureAwait(false);
      }
      catch (ToolchainException ex)
      {
        throw;
      }
      catch (OperationCanceledException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new ToolchainException(ToolchainError.Install, (IToolchainDependency) source, ex);
      }
      finally
      {
        await AsyncUtils.DeleteFileAsync(path, token).ConfigureAwait(false);
      }
      path = (string) null;
    }

    public override async Task Uninstall(CancellationToken token)
    {
      UnityDependency source = this;
      token.ThrowIfCancellationRequested();
      try
      {
        IToolchainDependency.log.Debug((object) "Uninstalling Unity");
        // ISSUE: explicit non-virtual call
        __nonvirtual (source.state) = new IToolchainDependency.State(DependencyState.Removing, "RemovingUnity");
        string unityUninstallerExe = UnityDependency.unityUninstallerExe;
        if (unityUninstallerExe != null)
        {
          CommandResult commandResult = await Cli.Wrap(unityUninstallerExe).WithArguments("/S /D=" + Path.Combine(source.installationDirectory, UnityDependency.kInstallationFolder)).WithStandardOutputPipe(PipeTarget.ToDelegate((Action<string>) (l => IToolchainDependency.log.Debug((object) l)))).WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => IToolchainDependency.log.Error((object) l)))).WithUseShellExecute(true).ExecuteAsync(token).ConfigureAwait(false);
        }
        string unityPath = UnityDependency.unityPath;
        if (unityPath != null)
          await AsyncUtils.WaitForAction((Func<bool>) (() => !LongDirectory.Exists(unityPath)), token).ConfigureAwait(false);
      }
      catch (OperationCanceledException ex)
      {
        throw;
      }
      catch (ToolchainException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new ToolchainException(ToolchainError.Uninstall, (IToolchainDependency) source, ex);
      }
    }

    private static bool TryGetRegistryKeyValue(
      RegistryKey registry,
      string path,
      string key,
      out string value)
    {
      value = (string) null;
      RegistryKey registryKey = (RegistryKey) null;
      try
      {
        registryKey = registry.OpenSubKey(path);
        if (registryKey != null)
        {
          object obj = registryKey.GetValue(key);
          if (obj != null)
          {
            value = obj.ToString();
            return true;
          }
        }
      }
      catch (Exception ex)
      {
        IToolchainDependency.log.Error(ex, (object) ("Failed checking registry key " + registry.Name + "\\" + path + key));
      }
      finally
      {
        registryKey?.Dispose();
      }
      return false;
    }

    private static bool TryGetParentPath(string path, int depth, out string parentPath)
    {
      parentPath = (string) null;
      if (string.IsNullOrEmpty(path))
        return false;
      DirectoryInfo directoryInfo = new DirectoryInfo(path);
      for (int index = 0; index < depth; ++index)
      {
        directoryInfo = directoryInfo.Parent;
        if (directoryInfo == null)
          return false;
      }
      if (!directoryInfo.Exists)
        return false;
      parentPath = directoryInfo.FullName;
      return true;
    }
  }
}
