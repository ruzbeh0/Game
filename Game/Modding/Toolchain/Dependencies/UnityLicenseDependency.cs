// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.Dependencies.UnityLicenseDependency
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using CliWrap;
using Colossal;
using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Game.Modding.Toolchain.Dependencies
{
  public class UnityLicenseDependency : BaseDependency
  {
    private static readonly string kSerialBasedLicenseFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Unity", "Unity_lic.ulf");
    private static readonly string kNamedUserLicenseFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Unity", "licenses", "UnityEntitlementLicense.xml");

    public override string name => "Unity license";

    public override string icon => "Media/Toolchain/Unity.svg";

    public override bool confirmUninstallation => true;

    public override LocalizedString installDescr
    {
      get => LocalizedString.Id("Options.WARN_TOOLCHAIN_INSTALL_UNITY_LICENSE");
    }

    public override LocalizedString uninstallMessage
    {
      get => LocalizedString.Id("Options.WARN_TOOLCHAIN_UNITY_LICENSE_RETURN");
    }

    public override Type[] dependsOnInstallation
    {
      get => new Type[1]{ typeof (UnityDependency) };
    }

    public override Type[] dependsOnUninstallation
    {
      get => new Type[1]{ typeof (UnityDependency) };
    }

    public bool licenseExists
    {
      get
      {
        return File.Exists(UnityLicenseDependency.kSerialBasedLicenseFile) || File.Exists(UnityLicenseDependency.kNamedUserLicenseFile);
      }
    }

    public override Task<bool> IsInstalled(CancellationToken token)
    {
      return Task.FromResult<bool>(this.licenseExists);
    }

    public override Task<bool> IsUpToDate(CancellationToken token) => Task.FromResult<bool>(true);

    public override Task<bool> NeedDownload(CancellationToken token)
    {
      return Task.FromResult<bool>(false);
    }

    public override Task Download(CancellationToken token) => Task.CompletedTask;

    public override async Task Install(CancellationToken token)
    {
      UnityLicenseDependency source = this;
      token.ThrowIfCancellationRequested();
      try
      {
        IToolchainDependency.log.Debug((object) "Waiting for Unity license");
        // ISSUE: explicit non-virtual call
        __nonvirtual (source.state) = new IToolchainDependency.State(DependencyState.Installing, "WaitingUnityLicense");
        Cli.Wrap(UnityDependency.unityExe).WithArguments((IEnumerable<string>) new string[3]
        {
          "-projectPath",
          UnityModProjectDependency.kProjectUnzipPath,
          "-quit"
        }).WithValidation(CommandResultValidation.None).ExecuteAsync(token);
        // ISSUE: reference to a compiler-generated method
        await AsyncUtils.WaitForAction(new Func<bool>(source.\u003CInstall\u003Eb__22_0), token).ConfigureAwait(false);
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
      UnityLicenseDependency source = this;
      token.ThrowIfCancellationRequested();
      try
      {
        if (!LongFile.Exists(UnityLicenseDependency.kSerialBasedLicenseFile))
          return;
        IToolchainDependency.log.Debug((object) "Return Unity license");
        // ISSUE: explicit non-virtual call
        __nonvirtual (source.state) = new IToolchainDependency.State(DependencyState.Removing, "ReturningUnityLicense");
        CommandResult commandResult = await Cli.Wrap(UnityDependency.unityExe).WithArguments((IEnumerable<string>) new string[2]
        {
          "-returnlicense",
          "-quit"
        }).WithStandardOutputPipe(PipeTarget.ToDelegate((Action<string>) (l => IToolchainDependency.log.Debug((object) l)))).WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => IToolchainDependency.log.Error((object) l)))).ExecuteAsync(token).ConfigureAwait(false);
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

    public override LocalizedString GetLocalizedState(bool includeProgress)
    {
      LocalizedString localizedState;
      switch (this.state.m_State)
      {
        case DependencyState.Installed:
          localizedState = LocalizedString.Id("Options.STATE_TOOLCHAIN[Activated]");
          break;
        case DependencyState.NotInstalled:
          localizedState = LocalizedString.Id("Options.STATE_TOOLCHAIN[NotActivated]");
          break;
        case DependencyState.Installing:
          localizedState = LocalizedString.Id("Options.STATE_TOOLCHAIN[WaitingForActivation]");
          break;
        case DependencyState.Removing:
          localizedState = LocalizedString.Id("Options.STATE_TOOLCHAIN[Returning]");
          break;
        default:
          localizedState = IToolchainDependency.GetLocalizedState(this.state, includeProgress);
          break;
      }
      return localizedState;
    }
  }
}
