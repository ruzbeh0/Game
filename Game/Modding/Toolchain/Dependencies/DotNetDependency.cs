// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.Dependencies.DotNetDependency
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
  public class DotNetDependency : BaseDependency
  {
    private const string kDependencyName = ".Net";
    private static readonly string sDotNetVersion = "8.0";
    public static readonly string sDotNetInstallerUrl = "https://aka.ms/dotnet/" + DotNetDependency.sDotNetVersion + "/dotnet-sdk-win-" + RuntimeInformation.OSArchitecture.ToString().ToLower() + ".exe";
    public static readonly string kDefaultInstallationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
    public static readonly string kInstallationFolder = "dotnet";
    private long? m_DownloadSize;
    private string m_InstallationDirectory;

    public override string name => ".Net SDK";

    public override string icon => "Media/Toolchain/DotNet.svg";

    public override bool confirmUninstallation => true;

    public string installerPath
    {
      get
      {
        return Path.Combine(Path.GetFullPath(SharedSettings.instance.modding.downloadDirectory), Path.GetFileName(DotNetDependency.sDotNetInstallerUrl));
      }
    }

    public override string installationDirectory
    {
      get => this.m_InstallationDirectory;
      set
      {
        this.m_InstallationDirectory = Path.GetFullPath(Path.Combine(value, DotNetDependency.kInstallationFolder));
      }
    }

    public override bool canChangeInstallationDirectory => true;

    public override LocalizedString installDescr
    {
      get
      {
        return new LocalizedString("Options.WARN_TOOLCHAIN_INSTALL_DOTNET", (string) null, (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
        {
          {
            "DOTNET_VERSION",
            (ILocElement) LocalizedString.Value(DotNetDependency.sDotNetVersion)
          },
          {
            "HOST",
            (ILocElement) LocalizedString.Value(new Uri(DotNetDependency.sDotNetInstallerUrl).Host)
          }
        });
      }
    }

    public override LocalizedString uninstallMessage
    {
      get
      {
        return new LocalizedString("Options.WARN_TOOLCHAIN_DOTNET_UNINSTALL", (string) null, (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
        {
          {
            "DOTNET_VERSION",
            (ILocElement) LocalizedString.Value(this.version)
          }
        });
      }
    }

    public DotNetDependency()
    {
      this.installationDirectory = DotNetDependency.kDefaultInstallationDirectory;
    }

    public override async Task<bool> IsInstalled(CancellationToken token)
    {
      return (await DotNetDependency.GetDotnetVersion(token).ConfigureAwait(false)).Major >= 6;
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
        this.m_DownloadSize = new long?(await IToolchainDependency.GetDownloadSizeAsync(DotNetDependency.sDotNetInstallerUrl, token).ConfigureAwait(false));
      return this.m_DownloadSize.Value;
    }

    public override async Task<List<IToolchainDependency.DiskSpaceRequirements>> GetRequiredDiskSpace(
      CancellationToken token)
    {
      DotNetDependency dotNetDependency = this;
      List<IToolchainDependency.DiskSpaceRequirements> requests = new List<IToolchainDependency.DiskSpaceRequirements>();
      ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = dotNetDependency.IsInstalled(token).ConfigureAwait(false);
      if (!await configuredTaskAwaitable)
      {
        requests.Add(new IToolchainDependency.DiskSpaceRequirements()
        {
          m_Path = dotNetDependency.installationDirectory,
          m_Size = 1073741824L
        });
        configuredTaskAwaitable = dotNetDependency.NeedDownload(token).ConfigureAwait(false);
        if (await configuredTaskAwaitable)
        {
          List<IToolchainDependency.DiskSpaceRequirements> spaceRequirementsList = requests;
          IToolchainDependency.DiskSpaceRequirements spaceRequirements = new IToolchainDependency.DiskSpaceRequirements();
          spaceRequirements.m_Path = dotNetDependency.installerPath;
          spaceRequirements.m_Size = await dotNetDependency.GetDotNetInstallerSize(token).ConfigureAwait(false);
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
      return BaseDependency.Download((BaseDependency) this, token, DotNetDependency.sDotNetInstallerUrl, this.installerPath, "DownloadingDotNet");
    }

    public override async Task Install(CancellationToken token)
    {
      DotNetDependency source = this;
      token.ThrowIfCancellationRequested();
      string path = source.installerPath;
      try
      {
        IToolchainDependency.log.DebugFormat("Installing {0}", (object) ".Net");
        // ISSUE: explicit non-virtual call
        __nonvirtual (source.state) = new IToolchainDependency.State(DependencyState.Installing, "InstallingDotNet");
        CommandResult commandResult = await Cli.Wrap(path).WithArguments("/install /quiet /norestart INSTALLDIR=\"" + Path.Combine(source.installationDirectory, DotNetDependency.kInstallationFolder) + "\" ").WithStandardOutputPipe(PipeTarget.ToDelegate((Action<string>) (l => IToolchainDependency.log.Debug((object) l)))).WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => IToolchainDependency.log.Error((object) l)))).WithUseShellExecute(true).ExecuteAsync(token).ConfigureAwait(false);
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
      DotNetDependency source = this;
      try
      {
        IToolchainDependency.log.DebugFormat("Uninstalling {0}", (object) ".Net");
        // ISSUE: explicit non-virtual call
        __nonvirtual (source.state) = new IToolchainDependency.State(DependencyState.Removing, "RemovingDotNet");
        string keyName;
        if (IToolchainDependency.GetUninstaller(new Dictionary<string, string>()
        {
          {
            "DisplayName",
            "Microsoft .NET SDK " + source.version + " (" + RuntimeInformation.OSArchitecture.ToString().ToLower() + ")"
          }
        }, out keyName) == null)
          throw new ToolchainException(ToolchainError.Uninstall, (IToolchainDependency) source, "Uninstaller not found");
        CommandResult commandResult = await Cli.Wrap(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Package Cache", keyName, "dotnet-sdk-" + source.version + "-win-" + RuntimeInformation.OSArchitecture.ToString().ToLower() + ".exe")).WithArguments("/uninstall /quiet").WithStandardOutputPipe(PipeTarget.ToDelegate((Action<string>) (l => IToolchainDependency.log.Debug((object) l)))).WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => IToolchainDependency.log.Error((object) l)))).WithUseShellExecute(true).ExecuteAsync(token).ConfigureAwait(false);
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
          Task.Run<string>((Func<Task<string>>) (async () => await this.GetVersion(GameManager.instance.terminationToken))).Wait();
        return base.version;
      }
      protected set => base.version = value;
    }

    private async Task<string> GetVersion(CancellationToken token)
    {
      System.Version version = await DotNetDependency.GetDotnetVersion(token).ConfigureAwait(false);
      base.version = version.Major != 0 ? version.ToString() : string.Empty;
      return base.version;
    }

    public override LocalizedString GetLocalizedVersion()
    {
      if (!string.IsNullOrEmpty(this.version))
        return base.GetLocalizedVersion();
      return new LocalizedString("Options.WARN_TOOLCHAIN_MIN_VERSION", (string) null, (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
      {
        {
          "MIN_VERSION",
          (ILocElement) LocalizedString.Value("6.0")
        }
      });
    }

    public static async Task<System.Version> GetDotnetVersion(CancellationToken token)
    {
      System.Version installedVersion = new System.Version();
      List<string> errorText = new List<string>();
      try
      {
        CommandResult commandResult = await Cli.Wrap("dotnet").WithArguments("--version").WithStandardOutputPipe(PipeTarget.ToDelegate((Action<string>) (l =>
        {
          System.Version result;
          if (System.Version.TryParse(l, out result))
          {
            installedVersion = result;
          }
          else
          {
            int length = l.IndexOf('-');
            if (length > 0 && System.Version.TryParse(l.Substring(0, length), out result))
              installedVersion = result;
            else
              IToolchainDependency.log.ErrorFormat("Failed to parse {0} version number \"{1}\"", (object) ".Net", (object) l);
          }
        }))).WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => errorText.Add(l)))).WithValidation(CommandResultValidation.None).ExecuteAsync(token).ConfigureAwait(false);
      }
      catch (Win32Exception ex)
      {
        if (ex.ErrorCode != -2147467259)
          IToolchainDependency.log.ErrorFormat((Exception) ex, "Failed to get {0} version", (object) ".Net");
        if (errorText.Count > 0)
          IToolchainDependency.log.Warn((object) string.Join<string>('\n', (IEnumerable<string>) errorText));
      }
      catch (Exception ex)
      {
        IToolchainDependency.log.ErrorFormat(ex, "Failed to get {0} version", (object) ".Net");
        if (errorText.Count > 0)
          IToolchainDependency.log.Warn((object) string.Join<string>('\n', (IEnumerable<string>) errorText));
      }
      return installedVersion;
    }
  }
}
