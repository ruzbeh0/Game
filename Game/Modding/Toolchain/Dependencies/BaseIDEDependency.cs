// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.Dependencies.BaseIDEDependency
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.SceneFlow;
using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Game.Modding.Toolchain.Dependencies
{
  public abstract class BaseIDEDependency : BaseDependency
  {
    public abstract string minVersion { get; }

    public virtual bool isMinVersion
    {
      get
      {
        System.Version result1;
        System.Version result2;
        return System.Version.TryParse(this.version, out result1) && System.Version.TryParse(this.minVersion, out result2) && result1 >= result2;
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

    protected abstract Task<string> GetIDEVersion(CancellationToken token);

    public async Task<string> GetVersion(CancellationToken token)
    {
      BaseIDEDependency baseIdeDependency = this;
      // ISSUE: reference to a compiler-generated method
      string str = baseIdeDependency.\u003C\u003En__0();
      if (str == null)
        str = await baseIdeDependency.GetIDEVersion(token).ConfigureAwait(false);
      baseIdeDependency.version = str;
      return baseIdeDependency.version;
    }

    public override async Task<bool> IsInstalled(CancellationToken token)
    {
      return !string.IsNullOrEmpty(await this.GetVersion(token).ConfigureAwait(false));
    }

    public override async Task<bool> IsUpToDate(CancellationToken token)
    {
      System.Version result1;
      System.Version result2;
      return System.Version.TryParse(await this.GetVersion(token).ConfigureAwait(false), out result1) && System.Version.TryParse(this.minVersion, out result2) && result1 >= result2;
    }

    public override LocalizedString GetLocalizedState(bool includeProgress)
    {
      LocalizedString localizedState;
      switch (this.state.m_State)
      {
        case DependencyState.Installed:
          localizedState = LocalizedString.Id("Options.STATE_TOOLCHAIN[Detected]");
          break;
        case DependencyState.NotInstalled:
          localizedState = LocalizedString.Id("Options.STATE_TOOLCHAIN[NotDetected]");
          break;
        case DependencyState.Outdated:
          localizedState = new LocalizedString("Options.STATE_TOOLCHAIN[DetectedOutdated]", (string) null, (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
          {
            {
              "VERSION",
              (ILocElement) LocalizedString.Value(this.version)
            }
          });
          break;
        default:
          localizedState = base.GetLocalizedState(includeProgress);
          break;
      }
      return localizedState;
    }

    public override LocalizedString GetLocalizedVersion()
    {
      LocalizedString localizedVersion;
      if (this.state.m_State == DependencyState.Installed)
        localizedVersion = base.GetLocalizedVersion();
      else
        localizedVersion = new LocalizedString("Options.WARN_TOOLCHAIN_MIN_VERSION", (string) null, (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
        {
          {
            "MIN_VERSION",
            (ILocElement) LocalizedString.Value(this.minVersion)
          }
        });
      return localizedVersion;
    }
  }
}
