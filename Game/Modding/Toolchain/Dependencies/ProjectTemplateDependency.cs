// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.Dependencies.ProjectTemplateDependency
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using CliWrap;
using Colossal;
using Colossal.Core;
using Colossal.IO;
using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Game.Modding.Toolchain.Dependencies
{
  public class ProjectTemplateDependency : BaseDependency
  {
    private const string kProjectName = "C# Mod project template";
    private const string kPropsFile = "Mod.props";
    private const string kTargetsFile = "Mod.targets";
    private static readonly string kPropsFileSource = ToolchainDependencyManager.kGameToolingPath + "/Mod.props";
    private static readonly string kTargetsFileSource = ToolchainDependencyManager.kGameToolingPath + "/Mod.targets";
    private static readonly string kPropsFileDeploy = ToolchainDependencyManager.kUserToolingPath + "/Mod.props";
    private static readonly string kTargetsFileDeploy = ToolchainDependencyManager.kUserToolingPath + "/Mod.targets";
    private const string kTemplatePackageId = "ColossalOrder.ModTemplate";
    private const string kTemplateId = "csiimod";
    private static readonly string kTemplatePackageFile = "ColossalOrder.ModTemplate.1.0.0.nupkg";
    private static readonly string kTemplatePackageSource = Path.Combine(ToolchainDependencyManager.kGameToolingPath, ProjectTemplateDependency.kTemplatePackageFile);
    private static readonly string kTemplatePackageInstallation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".templateengine", "packages", ProjectTemplateDependency.kTemplatePackageFile);

    public override string name => "C# template";

    public override string icon => "Media/Menu/ColossalLogo.svg";

    public override LocalizedString installDescr
    {
      get => LocalizedString.Id("Options.WARN_TOOLCHAIN_INSTALL_PROJECT_TEMPLATE");
    }

    public override Type[] dependsOnInstallation
    {
      get => new Type[1]{ typeof (DotNetDependency) };
    }

    public override Type[] dependsOnUninstallation
    {
      get => new Type[1]{ typeof (DotNetDependency) };
    }

    public override IEnumerable<string> envVariables
    {
      get
      {
        yield return "CSII_PATHSET";
        yield return "CSII_INSTALLATIONPATH";
        yield return "CSII_USERDATAPATH";
        yield return "CSII_TOOLPATH";
        yield return "CSII_LOCALMODSPATH";
        yield return "CSII_UNITYMODPROJECTPATH";
        yield return "CSII_UNITYVERSION";
        yield return "CSII_ENTITIESVERSION";
        yield return "CSII_MODPOSTPROCESSORPATH";
        yield return "CSII_MODPUBLISHERPATH";
        yield return "CSII_MANAGEDPATH";
        yield return "CSII_PDXCACHEPATH";
        yield return "CSII_PDXMODSPATH";
        yield return "CSII_ASSEMBLYSEARCHPATH";
      }
    }

    public override async Task<bool> IsInstalled(CancellationToken token)
    {
      try
      {
        if (!LongFile.Exists(ProjectTemplateDependency.kPropsFileDeploy) || !LongFile.Exists(ProjectTemplateDependency.kTargetsFileDeploy))
          return false;
        System.Version version = await DotNetDependency.GetDotnetVersion(token).ConfigureAwait(false);
        if (version.Major < 6)
          return false;
        List<string> errorText = new List<string>();
        CommandResult commandResult = await Cli.Wrap("dotnet").WithArguments(version.Major == 6 ? "new --list csiimod" : "new list csiimod --verbosity q").WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => errorText.Add(l)))).WithValidation(CommandResultValidation.None).ExecuteAsync(token).ConfigureAwait(false);
        if (errorText.Count > 0)
          IToolchainDependency.log.Warn((object) string.Join<string>('\n', (IEnumerable<string>) errorText));
        return commandResult.ExitCode == 0;
      }
      catch (Exception ex)
      {
        IToolchainDependency.log.Error(ex, (object) "Error during mod template check");
        return false;
      }
    }

    public override Task<bool> IsUpToDate(CancellationToken token)
    {
      try
      {
        if ((long) CalculateCache(ProjectTemplateDependency.kPropsFileSource) != (long) CalculateCache(ProjectTemplateDependency.kPropsFileDeploy))
          return Task.FromResult<bool>(false);
        if ((long) CalculateCache(ProjectTemplateDependency.kTargetsFileSource) != (long) CalculateCache(ProjectTemplateDependency.kTargetsFileDeploy))
          return Task.FromResult<bool>(false);
        return (long) CalculateCache(ProjectTemplateDependency.kTemplatePackageSource) != (long) CalculateCache(ProjectTemplateDependency.kTemplatePackageInstallation) ? Task.FromResult<bool>(false) : Task.FromResult<bool>(true);
      }
      catch (Exception ex)
      {
        IToolchainDependency.log.Error(ex, (object) "Error during mod template check");
        return Task.FromResult<bool>(false);
      }

      static ulong CalculateCache(string file)
      {
        return !LongFile.Exists(file) ? 0UL : new Crc(new CrcParameters(64, 4823603603198064275UL, 0UL, 0UL, false, false)).CalculateAsNumeric(LongFile.ReadAllBytes(file));
      }
    }

    public override Task<bool> NeedDownload(CancellationToken token)
    {
      return Task.FromResult<bool>(false);
    }

    public override Task Download(CancellationToken token) => Task.CompletedTask;

    public override async Task Install(CancellationToken token)
    {
      ProjectTemplateDependency source = this;
      token.ThrowIfCancellationRequested();
      try
      {
        IToolchainDependency.log.DebugFormat("Installing {0}", (object) "C# Mod project template");
        // ISSUE: explicit non-virtual call
        __nonvirtual (source.state) = new IToolchainDependency.State(DependencyState.Installing, "InstallingModTemplate");
        IToolchainDependency.log.DebugFormat("Copy mod template properties file '{0}' to '{1}'", (object) ProjectTemplateDependency.kPropsFileSource, (object) ProjectTemplateDependency.kPropsFileDeploy);
        IOUtils.EnsureDirectory(Path.GetDirectoryName(ProjectTemplateDependency.kPropsFileDeploy));
        await AsyncUtils.CopyFileAsync(ProjectTemplateDependency.kPropsFileSource, ProjectTemplateDependency.kPropsFileDeploy, true, token).ConfigureAwait(false);
        IToolchainDependency.log.DebugFormat("Copy mod template targets file '{0}' to '{1}'", (object) ProjectTemplateDependency.kTargetsFileSource, (object) ProjectTemplateDependency.kTargetsFileDeploy);
        IOUtils.EnsureDirectory(Path.GetDirectoryName(ProjectTemplateDependency.kTargetsFileDeploy));
        await AsyncUtils.CopyFileAsync(ProjectTemplateDependency.kTargetsFileSource, ProjectTemplateDependency.kTargetsFileDeploy, true, token).ConfigureAwait(false);
        IToolchainDependency.log.DebugFormat("Install mod template package '{0}'", (object) ProjectTemplateDependency.kTemplatePackageSource);
        System.Version dotnetVersion = await DotNetDependency.GetDotnetVersion(token).ConfigureAwait(false);
        if (dotnetVersion.Major < 6)
          throw new ToolchainException(ToolchainError.Install, (IToolchainDependency) source, ".net6.0 is required");
        List<string> errorText = new List<string>();
        CommandResult commandResult1 = await Cli.Wrap("dotnet").WithArguments(dotnetVersion.Major == 6 ? "new --uninstall ColossalOrder.ModTemplate" : "new uninstall ColossalOrder.ModTemplate --verbosity q").WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => errorText.Add(l)))).WithValidation(CommandResultValidation.None).ExecuteAsync(token).ConfigureAwait(false);
        if (errorText.Count > 0)
          IToolchainDependency.log.Warn((object) string.Join<string>('\n', (IEnumerable<string>) errorText));
        errorText.Clear();
        CommandResult commandResult2 = await Cli.Wrap("dotnet").WithArguments(dotnetVersion.Major == 6 ? "new --install \"" + ProjectTemplateDependency.kTemplatePackageSource + "\" --force" : "new install \"" + ProjectTemplateDependency.kTemplatePackageSource + "\" --force --verbosity q").WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => errorText.Add(l)))).WithValidation(CommandResultValidation.None).ExecuteAsync(token).ConfigureAwait(false);
        if (errorText.Count > 0)
          IToolchainDependency.log.Warn((object) string.Join<string>('\n', (IEnumerable<string>) errorText));
        if (commandResult2.ExitCode != 0)
          throw new ToolchainException(ToolchainError.Install, (IToolchainDependency) source, "Mod template package not installed: code {result.ExitCode}");
        dotnetVersion = (System.Version) null;
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

    public override async Task Uninstall(CancellationToken token)
    {
      ProjectTemplateDependency source = this;
      token.ThrowIfCancellationRequested();
      try
      {
        // ISSUE: explicit non-virtual call
        __nonvirtual (source.state) = new IToolchainDependency.State(DependencyState.Removing, "RemovingProjectTemplate");
        IToolchainDependency.log.DebugFormat("Removing {0}", (object) "C# Mod project template");
        await AsyncUtils.DeleteFileAsync(ProjectTemplateDependency.kPropsFileDeploy, token).ConfigureAwait(false);
        await AsyncUtils.DeleteFileAsync(ProjectTemplateDependency.kTargetsFileDeploy, token).ConfigureAwait(false);
        System.Version version = await DotNetDependency.GetDotnetVersion(token).ConfigureAwait(false);
        if (version.Major < 6)
          throw new ToolchainException(ToolchainError.Uninstall, (IToolchainDependency) source, ".net6.0 is required");
        List<string> errorText = new List<string>();
        CommandResult commandResult = await Cli.Wrap("dotnet").WithArguments(version.Major == 6 ? "new --uninstall ColossalOrder.ModTemplate" : "new uninstall ColossalOrder.ModTemplate").WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => errorText.Add(l)))).WithValidation(CommandResultValidation.None).ExecuteAsync(token).ConfigureAwait(false);
        if (errorText.Count > 0)
          IToolchainDependency.log.Warn((object) string.Join<string>('\n', (IEnumerable<string>) errorText));
        if (commandResult.ExitCode != 0)
          throw new ToolchainException(ToolchainError.Uninstall, (IToolchainDependency) source, "Mod template package not removed: code {result.ExitCode}");
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
      return Task.FromResult<List<IToolchainDependency.DiskSpaceRequirements>>(new List<IToolchainDependency.DiskSpaceRequirements>()
      {
        new IToolchainDependency.DiskSpaceRequirements()
        {
          m_Path = ProjectTemplateDependency.kPropsFileDeploy,
          m_Size = new FileInfo(ProjectTemplateDependency.kPropsFileSource).Length
        },
        new IToolchainDependency.DiskSpaceRequirements()
        {
          m_Path = ProjectTemplateDependency.kTargetsFileDeploy,
          m_Size = new FileInfo(ProjectTemplateDependency.kTargetsFileSource).Length
        },
        new IToolchainDependency.DiskSpaceRequirements()
        {
          m_Path = ProjectTemplateDependency.kTemplatePackageInstallation,
          m_Size = new FileInfo(ProjectTemplateDependency.kTemplatePackageSource).Length
        }
      });
    }
  }
}
