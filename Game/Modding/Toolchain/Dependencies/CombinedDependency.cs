// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.Dependencies.CombinedDependency
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Game.Modding.Toolchain.Dependencies
{
  [DebuggerDisplay("{state}")]
  public abstract class CombinedDependency : IToolchainDependency
  {
    private IToolchainDependency.State m_State;
    private System.Version m_Version;

    public event IToolchainDependency.ProgressDelegate onNotifyProgress;

    public abstract IEnumerable<IToolchainDependency> dependencies { get; }

    protected abstract bool isAsync { get; }

    public abstract CombinedDependency.CombineType type { get; }

    public string name => this.GetType().Name;

    public virtual LocalizedString localizedName
    {
      get => LocalizedString.Id("Options.OPTION[ModdingSettings." + this.GetType().Name + "]");
    }

    public virtual string version { get; protected set; }

    string IToolchainDependency.version
    {
      get => this.version;
      set => this.version = value;
    }

    public string icon => (string) null;

    public virtual bool confirmUninstallation => false;

    public virtual bool canBeInstalled => true;

    public virtual bool canBeUninstalled => true;

    public virtual bool canChangeInstallationDirectory => false;

    public virtual string installationDirectory { get; set; } = string.Empty;

    public virtual LocalizedString description => new LocalizedString();

    public virtual LocalizedString installDescr => new LocalizedString();

    public virtual LocalizedString uninstallDescr => new LocalizedString();

    public virtual LocalizedString uninstallMessage => new LocalizedString();

    public IToolchainDependency.State state
    {
      get => this.m_State;
      set
      {
        this.m_State = value;
        IToolchainDependency.ProgressDelegate onNotifyProgress = this.onNotifyProgress;
        if (onNotifyProgress == null)
          return;
        onNotifyProgress((IToolchainDependency) this, value);
      }
    }

    public bool needDownload { get; protected set; }

    bool IToolchainDependency.needDownload
    {
      get => this.needDownload;
      set => this.needDownload = value;
    }

    public List<IToolchainDependency.DiskSpaceRequirements> spaceRequirements { get; protected set; } = new List<IToolchainDependency.DiskSpaceRequirements>();

    List<IToolchainDependency.DiskSpaceRequirements> IToolchainDependency.spaceRequirements
    {
      get => this.spaceRequirements;
      set => this.spaceRequirements = value;
    }

    IEnumerable<string> IToolchainDependency.envVariables
    {
      get
      {
        foreach (IToolchainDependency dependency in this.dependencies)
        {
          foreach (string envVariable in dependency.envVariables)
            yield return envVariable;
        }
      }
    }

    public virtual Type[] dependsOnInstallation => Array.Empty<Type>();

    public virtual Type[] dependsOnUninstallation => Array.Empty<Type>();

    private async Task<bool> GetCombinedResult(
      CancellationToken token,
      Func<IToolchainDependency, CancellationToken, Task<bool>> getTaskPredicate)
    {
      if (this.isAsync)
      {
        List<Task<bool>> tasks;
        using (CancellationTokenSource anyTokenSource = new CancellationTokenSource())
        {
          using (CancellationTokenSource combinedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, anyTokenSource.Token))
          {
            tasks = this.dependencies.Select<IToolchainDependency, Task<bool>>((Func<IToolchainDependency, Task<bool>>) (d => getTaskPredicate(d, combinedTokenSource.Token))).ToList<Task<bool>>();
            switch (this.type)
            {
              case CombinedDependency.CombineType.OR:
                while (tasks.Count > 0)
                {
                  Task<bool> task = await Task.WhenAny<bool>((IEnumerable<Task<bool>>) tasks);
                  tasks.Remove(task);
                  if (task.IsCompletedSuccessfully && task.Result)
                  {
                    anyTokenSource.Cancel();
                    return true;
                  }
                }
                return false;
              case CombinedDependency.CombineType.AND:
                while (tasks.Count > 0)
                {
                  Task<bool> task = await Task.WhenAny<bool>((IEnumerable<Task<bool>>) tasks);
                  tasks.Remove(task);
                  if (task.IsFaulted || !task.Result)
                  {
                    anyTokenSource.Cancel();
                    return false;
                  }
                }
                return true;
            }
          }
        }
        tasks = (List<Task<bool>>) null;
      }
      else
      {
        switch (this.type)
        {
          case CombinedDependency.CombineType.OR:
            foreach (IToolchainDependency dependency in this.dependencies)
            {
              if (await getTaskPredicate(dependency, token))
                return true;
            }
            return false;
          case CombinedDependency.CombineType.AND:
            foreach (IToolchainDependency dependency in this.dependencies)
            {
              if (!await getTaskPredicate(dependency, token))
                return false;
            }
            return true;
        }
      }
      return false;
    }

    private async Task GetCombinedResult(
      CombinedDependency.CombineType combineType,
      CancellationToken token,
      Func<IToolchainDependency, CancellationToken, Task> getTaskPredicate)
    {
      List<Exception> errors;
      Task task;
      if (this.isAsync)
      {
        List<Task> tasks;
        using (CancellationTokenSource anyTokenSource = new CancellationTokenSource())
        {
          using (CancellationTokenSource combinedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, anyTokenSource.Token))
          {
            tasks = this.dependencies.Select<IToolchainDependency, Task>((Func<IToolchainDependency, Task>) (d => getTaskPredicate(d, combinedTokenSource.Token))).ToList<Task>();
            switch (combineType)
            {
              case CombinedDependency.CombineType.OR:
                errors = new List<Exception>();
                while (tasks.Count > 0)
                {
                  Task task1 = await Task.WhenAny((IEnumerable<Task>) tasks);
                  tasks.Remove(task1);
                  if (task1.IsCompletedSuccessfully)
                  {
                    anyTokenSource.Cancel();
                    return;
                  }
                  if (task1.Exception != null)
                    errors.AddRange((IEnumerable<Exception>) task1.Exception.InnerExceptions);
                }
                if (errors.Count != 0)
                  throw new AggregateException((IEnumerable<Exception>) errors);
                return;
              case CombinedDependency.CombineType.AND:
                while (tasks.Count > 0)
                {
                  Task task2 = await Task.WhenAny((IEnumerable<Task>) tasks);
                  tasks.Remove(task2);
                  if (task2.IsFaulted)
                    throw task2.Exception;
                }
                return;
              case CombinedDependency.CombineType.ALL:
                await Task.WhenAll((IEnumerable<Task>) tasks);
                return;
              default:
                errors = (List<Exception>) null;
                break;
            }
          }
        }
        tasks = (List<Task>) null;
      }
      else
      {
        switch (combineType)
        {
          case CombinedDependency.CombineType.OR:
            errors = new List<Exception>();
            foreach (IToolchainDependency dependency in this.dependencies)
            {
              task = getTaskPredicate(dependency, token);
              await task;
              if (task.IsCompletedSuccessfully)
                return;
              if (task.Exception != null)
                errors.AddRange((IEnumerable<Exception>) task.Exception.InnerExceptions);
              task = (Task) null;
            }
            if (errors.Count != 0)
              throw new AggregateException((IEnumerable<Exception>) errors);
            break;
          case CombinedDependency.CombineType.AND:
            foreach (IToolchainDependency dependency in this.dependencies)
            {
              task = getTaskPredicate(dependency, token);
              await task;
              task = !task.IsFaulted ? (Task) null : throw task.Exception;
            }
            break;
          case CombinedDependency.CombineType.ALL:
            foreach (IToolchainDependency dependency in this.dependencies)
              await getTaskPredicate(dependency, token);
            break;
          default:
            errors = (List<Exception>) null;
            break;
        }
      }
    }

    public virtual async Task Refresh(CancellationToken token)
    {
      CombinedDependency dependency = this;
      await dependency.GetCombinedResult(CombinedDependency.CombineType.ALL, token, (Func<IToolchainDependency, CancellationToken, Task>) ((d, t) => d.Refresh(t)));
      await IToolchainDependency.Refresh((IToolchainDependency) dependency, token);
    }

    public Task<bool> IsInstalled(CancellationToken token)
    {
      return this.GetCombinedResult(token, (Func<IToolchainDependency, CancellationToken, Task<bool>>) ((d, t) => d.IsInstalled(t)));
    }

    public Task<bool> IsUpToDate(CancellationToken token)
    {
      return this.GetCombinedResult(token, (Func<IToolchainDependency, CancellationToken, Task<bool>>) ((d, t) => d.IsUpToDate(t)));
    }

    public Task<bool> NeedDownload(CancellationToken token)
    {
      return this.GetCombinedResult(token, (Func<IToolchainDependency, CancellationToken, Task<bool>>) ((d, t) => d.NeedDownload(t)));
    }

    public Task Download(CancellationToken token)
    {
      return this.GetCombinedResult(this.type, token, (Func<IToolchainDependency, CancellationToken, Task>) ((d, t) => d.Download(t)));
    }

    public Task Install(CancellationToken token)
    {
      return this.GetCombinedResult(this.type, token, (Func<IToolchainDependency, CancellationToken, Task>) ((d, t) => d.Install(t)));
    }

    public Task Uninstall(CancellationToken token)
    {
      return this.GetCombinedResult(this.type, token, (Func<IToolchainDependency, CancellationToken, Task>) ((d, t) => d.Download(t)));
    }

    public Task<List<IToolchainDependency.DiskSpaceRequirements>> GetRequiredDiskSpace(
      CancellationToken token)
    {
      return Task.FromResult<List<IToolchainDependency.DiskSpaceRequirements>>(new List<IToolchainDependency.DiskSpaceRequirements>());
    }

    public virtual LocalizedString GetLocalizedState(bool includeProgress)
    {
      return IToolchainDependency.GetLocalizedState(this.state, includeProgress);
    }

    public enum CombineType
    {
      OR,
      AND,
      ALL,
    }
  }
}
