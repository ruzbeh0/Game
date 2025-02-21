// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.Dependencies.BaseDependency
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

#nullable disable
namespace Game.Modding.Toolchain.Dependencies
{
  [DebuggerDisplay("{m_State}")]
  public abstract class BaseDependency : IToolchainDependency
  {
    private IToolchainDependency.State m_State;

    public event IToolchainDependency.ProgressDelegate onNotifyProgress;

    public virtual LocalizedString localizedName
    {
      get => LocalizedString.Id("Options.OPTION[ModdingSettings." + this.GetType().Name + "]");
    }

    public virtual string name => this.GetType().Name;

    public virtual string version { get; protected set; }

    string IToolchainDependency.version
    {
      get => this.version;
      set => this.version = value;
    }

    public virtual string icon => (string) null;

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

    public virtual bool confirmUninstallation => false;

    public virtual bool canBeInstalled => true;

    public virtual bool canBeUninstalled => true;

    public virtual bool canChangeInstallationDirectory => false;

    public virtual string installationDirectory { get; set; } = string.Empty;

    public virtual LocalizedString description
    {
      get
      {
        return LocalizedString.Id("Options.OPTION_DESCRIPTION[ModdingSettings." + this.GetType().Name + "]");
      }
    }

    public virtual LocalizedString installDescr => this.localizedName;

    public virtual LocalizedString uninstallDescr => this.localizedName;

    public virtual LocalizedString uninstallMessage => new LocalizedString();

    public virtual Type[] dependsOnInstallation => Array.Empty<Type>();

    public virtual Type[] dependsOnUninstallation => Array.Empty<Type>();

    public virtual Task<bool> IsInstalled(CancellationToken token) => Task.FromResult<bool>(false);

    public virtual Task<bool> IsUpToDate(CancellationToken token) => Task.FromResult<bool>(true);

    public virtual Task<bool> NeedDownload(CancellationToken token) => Task.FromResult<bool>(false);

    public virtual Task Download(CancellationToken token) => Task.CompletedTask;

    public virtual Task Install(CancellationToken token) => Task.CompletedTask;

    public virtual Task Uninstall(CancellationToken token) => Task.CompletedTask;

    public virtual Task Refresh(CancellationToken token)
    {
      return IToolchainDependency.Refresh((IToolchainDependency) this, token);
    }

    public virtual Task<List<IToolchainDependency.DiskSpaceRequirements>> GetRequiredDiskSpace(
      CancellationToken token)
    {
      return Task.FromResult<List<IToolchainDependency.DiskSpaceRequirements>>(new List<IToolchainDependency.DiskSpaceRequirements>());
    }

    public virtual IEnumerable<string> envVariables
    {
      get
      {
        yield break;
      }
    }

    public virtual LocalizedString GetLocalizedState(bool includeProgress)
    {
      return IToolchainDependency.GetLocalizedState(this.state, includeProgress);
    }

    public virtual LocalizedString GetLocalizedVersion() => LocalizedString.Value(this.version);

    public override int GetHashCode()
    {
      return HashCode.Combine<IToolchainDependency.State, DeploymentAction>(this.m_State, this.availableActions);
    }

    public override string ToString() => this.name;

    protected static async Task Download(
      BaseDependency dependency,
      CancellationToken token,
      string url,
      string pathOnDisk,
      string detail)
    {
      token.ThrowIfCancellationRequested();
      IToolchainDependency.log.DebugFormat("Downloading {0}", (object) dependency.name);
      dependency.state = new IToolchainDependency.State(DependencyState.Downloading, detail);
      try
      {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        webRequest.downloadHandler = (DownloadHandler) new DownloadHandlerFile(pathOnDisk)
        {
          removeFileOnAbort = true
        };
        UnityWebRequest.Result result = await webRequest.SendWebRequest().ConfigureAwait(new Func<UnityWebRequestAsyncOperation, bool>(UpdateProgress), token, 7f);
        token.ThrowIfCancellationRequested();
        switch (result)
        {
          case UnityWebRequest.Result.ConnectionError:
            throw new ToolchainException(ToolchainError.Download, (IToolchainDependency) dependency, "Connection Error: " + webRequest.error);
          case UnityWebRequest.Result.ProtocolError:
            throw new ToolchainException(ToolchainError.Download, (IToolchainDependency) dependency, "HTTP Error: " + webRequest.error);
          case UnityWebRequest.Result.DataProcessingError:
            throw new ToolchainException(ToolchainError.Download, (IToolchainDependency) dependency, "Error: " + webRequest.error);
          default:
            IToolchainDependency.log.DebugFormat("{0} download finished", (object) dependency.name);
            webRequest = (UnityWebRequest) null;
            break;
        }
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
        throw new ToolchainException(ToolchainError.Download, (IToolchainDependency) dependency, ex);
      }

      bool UpdateProgress(UnityWebRequestAsyncOperation asyncOperation)
      {
        int num = asyncOperation.isDone ? 1 : 0;
        if (num != 0)
          return num != 0;
        dependency.state = new IToolchainDependency.State(DependencyState.Downloading, detail, new int?((int) ((double) asyncOperation.progress * 100.0)));
        return num != 0;
      }
    }
  }
}
