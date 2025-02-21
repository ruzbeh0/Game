// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.Dependencies.NodeJSDependency
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using CliWrap;
using CliWrap.Exceptions;
using Colossal;
using Game.SceneFlow;
using Game.Settings;
using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Game.Modding.Toolchain.Dependencies
{
  public class NodeJSDependency : BaseDependency
  {
    public static readonly string kNodeJSVersion = "20.11.0";
    public static readonly string kMinNodeJSVersion = "18.0";
    public static readonly string kNodeJSInstallerUrl = "https://nodejs.org/dist/v" + NodeJSDependency.kNodeJSVersion + "/node-v" + NodeJSDependency.kNodeJSVersion + "-" + RuntimeInformation.OSArchitecture.ToString().ToLower() + ".msi";
    public static readonly string kDefaultInstallationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
    public static readonly string kInstallationFolder = "nodejs";
    private long? m_DownloadSize;
    private string m_InstallationDirectory;

    public override string name => "Node.js";

    public override string icon => "Media/Toolchain/NodeJS.svg";

    public override bool confirmUninstallation => true;

    public string installerPath
    {
      get
      {
        return Path.Combine(Path.GetFullPath(SharedSettings.instance.modding.downloadDirectory), Path.GetFileName(NodeJSDependency.kNodeJSInstallerUrl));
      }
    }

    public override string installationDirectory
    {
      get => this.m_InstallationDirectory;
      set
      {
        this.m_InstallationDirectory = Path.GetFullPath(Path.Combine(value, NodeJSDependency.kInstallationFolder));
      }
    }

    public override bool canChangeInstallationDirectory => true;

    public override LocalizedString installDescr
    {
      get
      {
        return new LocalizedString("Options.WARN_TOOLCHAIN_INSTALL_NODEJS", (string) null, (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
        {
          {
            "NODEJS_VERSION",
            (ILocElement) LocalizedString.Value(NodeJSDependency.kNodeJSVersion)
          },
          {
            "HOST",
            (ILocElement) LocalizedString.Value(new Uri(NodeJSDependency.kNodeJSInstallerUrl).Host)
          }
        });
      }
    }

    public override LocalizedString uninstallMessage
    {
      get
      {
        return new LocalizedString("Options.WARN_TOOLCHAIN_NODEJS_UNINSTALL", (string) null, (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
        {
          {
            "NODEJS_VERSION",
            (ILocElement) LocalizedString.Value(NodeJSDependency.kNodeJSVersion)
          }
        });
      }
    }

    public NodeJSDependency()
    {
      this.installationDirectory = NodeJSDependency.kDefaultInstallationDirectory;
    }

    public override async Task<bool> IsInstalled(CancellationToken token)
    {
      return !string.IsNullOrEmpty(await this.GetNodeVersion(token).ConfigureAwait(false));
    }

    public override async Task<bool> IsUpToDate(CancellationToken token)
    {
      string input = await this.GetNodeVersion(token).ConfigureAwait(false);
      System.Version result1;
      System.Version result2;
      return System.Version.TryParse(NodeJSDependency.kMinNodeJSVersion, out result1) && System.Version.TryParse(input, out result2) && result2 >= result1;
    }

    public override async Task<bool> NeedDownload(CancellationToken token)
    {
      FileInfo installerFile = new FileInfo(this.installerPath);
      if (!installerFile.Exists)
        return true;
      if (installerFile.Length == await this.GetDotNetInstallerSize(token).ConfigureAwait(false))
        return false;
      await AsyncUtils.DeleteFileAsync(this.installerPath, token).ConfigureAwait(false);
      return true;
    }

    private async Task<long> GetDotNetInstallerSize(CancellationToken token)
    {
      this.m_DownloadSize.GetValueOrDefault();
      if (!this.m_DownloadSize.HasValue)
        this.m_DownloadSize = new long?(await IToolchainDependency.GetDownloadSizeAsync(NodeJSDependency.kNodeJSInstallerUrl, token).ConfigureAwait(false));
      return this.m_DownloadSize.Value;
    }

    public override async Task<List<IToolchainDependency.DiskSpaceRequirements>> GetRequiredDiskSpace(
      CancellationToken token)
    {
      NodeJSDependency nodeJsDependency = this;
      List<IToolchainDependency.DiskSpaceRequirements> requests = new List<IToolchainDependency.DiskSpaceRequirements>();
      ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = nodeJsDependency.IsInstalled(token).ConfigureAwait(false);
      if (!await configuredTaskAwaitable)
      {
        requests.Add(new IToolchainDependency.DiskSpaceRequirements()
        {
          m_Path = nodeJsDependency.installationDirectory,
          m_Size = 104857600L
        });
        configuredTaskAwaitable = nodeJsDependency.NeedDownload(token).ConfigureAwait(false);
        if (await configuredTaskAwaitable)
        {
          List<IToolchainDependency.DiskSpaceRequirements> spaceRequirementsList = requests;
          IToolchainDependency.DiskSpaceRequirements spaceRequirements = new IToolchainDependency.DiskSpaceRequirements();
          spaceRequirements.m_Path = nodeJsDependency.installerPath;
          spaceRequirements.m_Size = await nodeJsDependency.GetDotNetInstallerSize(token).ConfigureAwait(false);
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
      return BaseDependency.Download((BaseDependency) this, token, NodeJSDependency.kNodeJSInstallerUrl, this.installerPath, "DownloadingNodeJS");
    }

    public override async Task Install(CancellationToken token)
    {
      NodeJSDependency source = this;
      token.ThrowIfCancellationRequested();
      string path = source.installerPath;
      try
      {
        IToolchainDependency.log.DebugFormat("Installing {0}", (object) source.name);
        // ISSUE: explicit non-virtual call
        __nonvirtual (source.state) = new IToolchainDependency.State(DependencyState.Installing, "InstallingNodeJS");
        CommandResult commandResult = await Cli.Wrap("msiexec").WithArguments("/i \"" + path + "\" /passive /norestart INSTALLDIR=\"" + source.installationDirectory + "\" ").WithStandardOutputPipe(PipeTarget.ToDelegate((Action<string>) (l => IToolchainDependency.log.Debug((object) l)))).WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => IToolchainDependency.log.Error((object) l)))).WithUseShellExecute(true).ExecuteAsync(token).ConfigureAwait(false);
        IToolchainDependency.UpdateProcessEnvVarPathValue();
      }
      catch (ToolchainException ex)
      {
        throw;
      }
      catch (OperationCanceledException ex)
      {
        throw;
      }
      catch (CommandExecutionException ex)
      {
        if (ex.ExitCode == 1602 || ex.ExitCode == 1603)
          throw new ToolchainException(ToolchainError.Install, (IToolchainDependency) source, "Installation canceled by user");
        throw new ToolchainException(ToolchainError.Install, (IToolchainDependency) source, (Exception) ex);
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
      NodeJSDependency source = this;
      try
      {
        IToolchainDependency.log.DebugFormat("Uninstalling {0}", (object) source.name);
        // ISSUE: explicit non-virtual call
        __nonvirtual (source.state) = new IToolchainDependency.State(DependencyState.Removing, "RemovingNodeJS");
        string keyName;
        if (IToolchainDependency.GetUninstaller(new Dictionary<string, string>()
        {
          {
            "DisplayName",
            "Node.js"
          },
          {
            "DisplayVersion",
            source.version
          }
        }, out keyName) == null)
          throw new ToolchainException(ToolchainError.Uninstall, (IToolchainDependency) source, "Uninstaller not found");
        CommandResult commandResult = await Cli.Wrap("msiexec").WithArguments("/x " + keyName + " /passive /norestart").WithStandardOutputPipe(PipeTarget.ToDelegate((Action<string>) (l => IToolchainDependency.log.Debug((object) l)))).WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => IToolchainDependency.log.Error((object) l)))).WithUseShellExecute(true).ExecuteAsync(token).ConfigureAwait(false);
      }
      catch (ToolchainException ex)
      {
        throw;
      }
      catch (OperationCanceledException ex)
      {
        throw;
      }
      catch (CommandExecutionException ex)
      {
        if (ex.ExitCode == 1602 || ex.ExitCode == 1603)
          throw new ToolchainException(ToolchainError.Install, (IToolchainDependency) source, "Uninstallation canceled by user");
        throw new ToolchainException(ToolchainError.Install, (IToolchainDependency) source, (Exception) ex);
      }
      catch (Exception ex)
      {
        throw new ToolchainException(ToolchainError.Uninstall, (IToolchainDependency) source, ex);
      }
    }

    public override string version
    {
      get
      {
        if (base.version == null)
          Task.Run<string>((Func<Task<string>>) (async () => await this.GetNodeVersion(GameManager.instance.terminationToken))).Wait();
        return base.version;
      }
      protected set => base.version = value;
    }

    private async Task<string> GetNodeVersion(CancellationToken token)
    {
      NodeJSDependency nodeJsDependency1 = this;
      string installedVersion = string.Empty;
      List<string> errorText = new List<string>();
      try
      {
        CommandResult commandResult = await Cli.Wrap("node").WithArguments("-v").WithStandardOutputPipe(PipeTarget.ToDelegate((Action<string>) (l => installedVersion = l))).WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => errorText.Add(l)))).WithValidation(CommandResultValidation.None).ExecuteAsync(token).ConfigureAwait(false);
      }
      catch (Win32Exception ex)
      {
        if (ex.ErrorCode != -2147467259)
          IToolchainDependency.log.ErrorFormat((Exception) ex, "Failed to get {0} version", (object) nodeJsDependency1.name);
      }
      catch (Exception ex)
      {
        IToolchainDependency.log.ErrorFormat(ex, "Failed to get {0} version", (object) nodeJsDependency1.name);
      }
      if (errorText.Count > 0)
        IToolchainDependency.log.Warn((object) string.Join<string>('\n', (IEnumerable<string>) errorText));
      NodeJSDependency nodeJsDependency2 = nodeJsDependency1;
      string str1;
      if (!installedVersion.StartsWith('v'))
      {
        str1 = installedVersion;
      }
      else
      {
        string str2 = installedVersion;
        str1 = str2.Substring(1, str2.Length - 1);
      }
      // ISSUE: reference to a compiler-generated method
      nodeJsDependency2.\u003C\u003En__0(str1);
      // ISSUE: reference to a compiler-generated method
      return nodeJsDependency1.\u003C\u003En__1();
    }

    public override LocalizedString GetLocalizedVersion()
    {
      if (!string.IsNullOrEmpty(this.version))
        return base.GetLocalizedVersion();
      return new LocalizedString("Options.WARN_TOOLCHAIN_MIN_VERSION", (string) null, (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
      {
        {
          "MIN_VERSION",
          (ILocElement) LocalizedString.Value(NodeJSDependency.kMinNodeJSVersion)
        }
      });
    }
  }
}
