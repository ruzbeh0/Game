// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.Dependencies.MainDependency
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Game.Modding.Toolchain.Dependencies
{
  public class MainDependency : IToolchainDependency
  {
    public event IToolchainDependency.ProgressDelegate onNotifyProgress;

    public string name => this.GetType().Name;

    public LocalizedString localizedName
    {
      get => LocalizedString.Id("Options.OPTION[ModdingSettings.ToolchainDeployment]");
    }

    public DeploymentAction availableActions => DeploymentAction.None;

    public LocalizedString GetLocalizedState(bool includeProgress)
    {
      return ToolchainDeployment.dependencyManager.cachedState.GetLocalizedState(includeProgress);
    }

    public override int GetHashCode()
    {
      return ToolchainDeployment.dependencyManager.cachedState.GetHashCode();
    }

    public IToolchainDependency.State state
    {
      get => ToolchainDeployment.dependencyManager.cachedState.toDependencyState;
      set
      {
      }
    }

    string IToolchainDependency.version { get; set; }

    bool IToolchainDependency.needDownload { get; set; }

    List<IToolchainDependency.DiskSpaceRequirements> IToolchainDependency.spaceRequirements { get; set; }

    IEnumerable<string> IToolchainDependency.envVariables
    {
      get
      {
        yield break;
      }
    }

    public string icon => (string) null;

    public bool confirmUninstallation { get; }

    public bool canBeInstalled { get; }

    public bool canBeUninstalled { get; }

    public bool canChangeInstallationDirectory => false;

    public string installationDirectory { get; set; } = string.Empty;

    public LocalizedString description
    {
      get => LocalizedString.Id("Options.OPTION_DESCRIPTION[ModdingSettings.ToolchainDeployment]");
    }

    public LocalizedString installDescr { get; }

    public LocalizedString uninstallDescr { get; }

    public LocalizedString uninstallMessage { get; }

    public Type[] dependsOnInstallation { get; }

    public Type[] dependsOnUninstallation { get; }

    public Task<bool> IsInstalled(CancellationToken token) => throw new NotSupportedException();

    public Task<bool> IsUpToDate(CancellationToken token) => throw new NotSupportedException();

    public Task<bool> NeedDownload(CancellationToken token) => throw new NotSupportedException();

    public Task Download(CancellationToken token) => throw new NotSupportedException();

    public Task Install(CancellationToken token) => throw new NotSupportedException();

    public Task Uninstall(CancellationToken token) => throw new NotSupportedException();

    public Task<List<IToolchainDependency.DiskSpaceRequirements>> GetRequiredDiskSpace(
      CancellationToken token)
    {
      throw new NotSupportedException();
    }

    public Task Refresh(CancellationToken token) => throw new NotSupportedException();
  }
}
