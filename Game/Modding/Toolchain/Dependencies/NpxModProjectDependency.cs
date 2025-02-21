// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.Dependencies.NpxModProjectDependency
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using CliWrap;
using Colossal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Game.Modding.Toolchain.Dependencies
{
  public class NpxModProjectDependency : BaseDependency
  {
    private const string kProjectName = "UI Mod project template";
    private const string kModuleNamespace = "@colossalorder";
    private const string kModuleName = "create-csii-ui-mod";
    private static readonly string kNpxPackagePath = Path.GetFullPath(Path.Combine(ToolchainDependencyManager.kGameToolingPath, "npx-create-csii-ui-mod"));

    public override Type[] dependsOnInstallation
    {
      get => new Type[1]{ typeof (NodeJSDependency) };
    }

    public override Type[] dependsOnUninstallation
    {
      get => new Type[1]{ typeof (NodeJSDependency) };
    }

    public override IEnumerable<string> envVariables
    {
      get
      {
        yield return "CSII_PATHSET";
        yield return "CSII_USERDATAPATH";
      }
    }

    public override string name => "UI template";

    public override string icon => "Media/Menu/ColossalLogo.svg";

    private async Task<string> GetGlobalNodeModulePath(CancellationToken token)
    {
      string path = string.Empty;
      List<string> errorText = new List<string>();
      try
      {
        CommandResult commandResult = await Cli.Wrap("npm").WithArguments("config get prefix").WithStandardOutputPipe(PipeTarget.ToDelegate((Action<string>) (l => path = l))).WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => errorText.Add(l)))).WithValidation(CommandResultValidation.None).ExecuteAsync(token).ConfigureAwait(false);
      }
      catch (Win32Exception ex)
      {
        if (ex.ErrorCode != -2147467259)
          IToolchainDependency.log.Error((Exception) ex, (object) "Failed to get global npm module path");
        if (errorText.Count > 0)
          IToolchainDependency.log.Warn((object) string.Join<string>('\n', (IEnumerable<string>) errorText));
      }
      catch (Exception ex)
      {
        IToolchainDependency.log.Error(ex, (object) "Failed to get global npm module path");
        if (errorText.Count > 0)
          IToolchainDependency.log.Warn((object) string.Join<string>('\n', (IEnumerable<string>) errorText));
      }
      return path;
    }

    public override async Task<bool> IsInstalled(CancellationToken token)
    {
      string str = await this.GetGlobalNodeModulePath(token).ConfigureAwait(false);
      return LongDirectory.Exists(str) && LongDirectory.Exists(Path.GetFullPath(Path.Combine(str, "node_modules", "@colossalorder")));
    }

    public override async Task<bool> IsUpToDate(CancellationToken token)
    {
      string str = await this.GetGlobalNodeModulePath(token).ConfigureAwait(false);
      string targetPath;
      return !LongDirectory.Exists(str) || !LongFile.TryGetSymlinkTarget(Path.Combine(str, "node_modules", "@colossalorder", "create-csii-ui-mod"), out targetPath) || targetPath == NpxModProjectDependency.kNpxPackagePath;
    }

    public override Task<bool> NeedDownload(CancellationToken token)
    {
      return Task.FromResult<bool>(false);
    }

    public override Task Download(CancellationToken token) => Task.CompletedTask;

    public override async Task Install(CancellationToken token)
    {
      NpxModProjectDependency source = this;
      token.ThrowIfCancellationRequested();
      List<string> output = new List<string>();
      List<string> errorText = new List<string>();
      try
      {
        IToolchainDependency.log.DebugFormat("Installing {0}", (object) "UI Mod project template");
        // ISSUE: explicit non-virtual call
        __nonvirtual (source.state) = new IToolchainDependency.State(DependencyState.Installing, "InstallingNpxModsTemplate");
        CommandResult commandResult = await Cli.Wrap("npm").WithArguments("link").WithWorkingDirectory(NpxModProjectDependency.kNpxPackagePath).WithStandardOutputPipe(PipeTarget.ToDelegate((Action<string>) (l => output.Add(l)))).WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => errorText.Add(l)))).WithValidation(CommandResultValidation.None).ExecuteAsync(token).ConfigureAwait(false);
        if (errorText.Count <= 0)
          ;
        else
          IToolchainDependency.log.WarnFormat("{0}\n\n{1}", (object) string.Join<string>('\n', (IEnumerable<string>) output), (object) string.Join<string>('\n', (IEnumerable<string>) errorText));
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
        throw new ToolchainException(ToolchainError.Install, (IToolchainDependency) source, ex);
      }
    }

    private static async Task DeleteNpxModule(
      string globalNodeModulePath,
      string moduleNamespace,
      string moduleName,
      CancellationToken token)
    {
      if (string.IsNullOrEmpty(globalNodeModulePath))
        throw new ArgumentException("Directory path cannot be null or empty.", nameof (globalNodeModulePath));
      if (string.IsNullOrEmpty(moduleName))
        throw new ArgumentException("File prefix cannot be null or empty.", nameof (moduleName));
      string[] strArray = Directory.Exists(globalNodeModulePath) ? LongDirectory.GetFiles(globalNodeModulePath, moduleName + "*") : throw new DirectoryNotFoundException("The specified directory was not found: " + globalNodeModulePath);
      for (int index = 0; index < strArray.Length; ++index)
        await AsyncUtils.DeleteFileAsync(strArray[index], token).ConfigureAwait(false);
      strArray = (string[]) null;
      await AsyncUtils.DeleteDirectoryAsync(Path.Combine(globalNodeModulePath, "node_modules", moduleNamespace), true, token).ConfigureAwait(false);
    }

    public override async Task Uninstall(CancellationToken token)
    {
      NpxModProjectDependency source = this;
      token.ThrowIfCancellationRequested();
      try
      {
        IToolchainDependency.log.DebugFormat("Deleting {0}", (object) "UI Mod project template");
        // ISSUE: explicit non-virtual call
        __nonvirtual (source.state) = new IToolchainDependency.State(DependencyState.Removing, "RemovingNpxModsTemplate");
        string str = await source.GetGlobalNodeModulePath(token).ConfigureAwait(false);
        if (!LongDirectory.Exists(str))
          return;
        await NpxModProjectDependency.DeleteNpxModule(str, "@colossalorder", "create-csii-ui-mod", token);
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

    public override Task<List<IToolchainDependency.DiskSpaceRequirements>> GetRequiredDiskSpace(
      CancellationToken token)
    {
      return Task.FromResult<List<IToolchainDependency.DiskSpaceRequirements>>(new List<IToolchainDependency.DiskSpaceRequirements>());
    }
  }
}
