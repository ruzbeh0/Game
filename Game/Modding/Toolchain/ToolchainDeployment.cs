// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.ToolchainDeployment
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Game.Modding.Toolchain.Dependencies;
using Game.SceneFlow;
using Game.UI;
using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Game.Modding.Toolchain
{
  public static class ToolchainDeployment
  {
    public static ToolchainDependencyManager dependencyManager { get; } = new ToolchainDependencyManager();

    static ToolchainDeployment()
    {
      ToolchainDeployment.dependencyManager.Register<UnityDependency>();
      ToolchainDeployment.dependencyManager.Register<UnityLicenseDependency>();
      ToolchainDeployment.dependencyManager.Register<UnityModProjectDependency>();
      ToolchainDeployment.dependencyManager.Register<DotNetDependency>();
      ToolchainDeployment.dependencyManager.Register<ProjectTemplateDependency>();
      ToolchainDeployment.dependencyManager.Register<NodeJSDependency>();
      ToolchainDeployment.dependencyManager.Register<NpxModProjectDependency>();
      ToolchainDeployment.dependencyManager.Register<IDEDependency>();
    }

    public static async void RunWithUI(
      DeploymentAction action,
      List<IToolchainDependency> dependencies = null,
      Action<bool> callback = null)
    {
      (List<IToolchainDependency>, List<IToolchainDependency>) filtered;
      if (ToolchainDeployment.dependencyManager.isInProgress)
      {
        filtered = ();
      }
      else
      {
        filtered = ToolchainDependencyManager.DependencyFilter.Process(action, dependencies);
        if (filtered.Item1.Count == 0)
        {
          Action<bool> action1 = callback;
          if (action1 != null)
            action1(dependencies == null);
          ToolchainDependencyManager.State currentState = await ToolchainDeployment.dependencyManager.GetCurrentState();
          filtered = ();
        }
        else
        {
          filtered.Item1.Sort(action < DeploymentAction.Uninstall ? new Comparison<IToolchainDependency>(IToolchainDependency.InstallSorting) : new Comparison<IToolchainDependency>(IToolchainDependency.UninstallSorting));
          TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
          Dictionary<string, ILocElement> args = new Dictionary<string, ILocElement>();
          foreach (IToolchainDependency toolchainDependency in filtered.Item1)
            args.Add(string.Format("Item{0}", (object) args.Count), (ILocElement) (action < DeploymentAction.Uninstall ? toolchainDependency.installDescr : toolchainDependency.uninstallDescr));
          GameManager.instance.userInterface.appBindings.ShowConfirmationDialog(new ConfirmationDialog(new LocalizedString?((LocalizedString) "Common.DIALOG_TITLE[Warning]"), LocalizedString.Id(action < DeploymentAction.Uninstall ? "Options.WARN_TOOLCHAIN_INSTALL_NEW" : "Options.WARN_TOOLCHAIN_UNINSTALL_NEW"), new LocalizedString?(new LocalizedString((string) null, string.Join("\n\n", args.Keys.Select<string, string>((Func<string, string>) (key => "- {" + key + "}"))), (IReadOnlyDictionary<string, ILocElement>) args)), false, (LocalizedString) "Common.DIALOG_ACTION[Yes]", new LocalizedString?((LocalizedString) "Common.DIALOG_ACTION[No]"), Array.Empty<LocalizedString>()), (Action<int>) (msg => tcs.SetResult(msg == 0)));
          bool goAhead = await tcs.Task;
          if (goAhead)
            await ToolchainDeployment.RunImpl(action, filtered.Item1);
          Action<bool> action2 = callback;
          if (action2 == null)
          {
            filtered = ();
          }
          else
          {
            action2(goAhead);
            filtered = ();
          }
        }
      }
    }

    public static async Task Run(DeploymentAction action, List<IToolchainDependency> dependencies = null)
    {
      if (ToolchainDeployment.dependencyManager.isInProgress)
        return;
      (List<IToolchainDependency> accepted, List<IToolchainDependency> discarded) tuple = ToolchainDependencyManager.DependencyFilter.Process(action, dependencies);
      if (tuple.accepted.Count == 0)
        return;
      await ToolchainDeployment.RunImpl(action, tuple.accepted);
    }

    private static async Task RunImpl(
      DeploymentAction action,
      List<IToolchainDependency> dependencies)
    {
      if (action < DeploymentAction.Uninstall)
        await TaskManager.instance.SharedTask<List<IToolchainDependency>>("InstallToolchain", new Func<List<IToolchainDependency>, CancellationToken, Task>(ToolchainDeployment.dependencyManager.Install), dependencies, GameManager.instance.terminationToken);
      else
        await TaskManager.instance.SharedTask<List<IToolchainDependency>>("UninstallToolchain", new Func<List<IToolchainDependency>, CancellationToken, Task>(ToolchainDeployment.dependencyManager.Uninstall), dependencies, GameManager.instance.terminationToken);
    }
  }
}
