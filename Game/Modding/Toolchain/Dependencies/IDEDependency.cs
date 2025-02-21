// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.Dependencies.IDEDependency
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.Modding.Toolchain.Dependencies
{
  public class IDEDependency : CombinedDependency
  {
    private BaseIDEDependency[] ides = new BaseIDEDependency[3]
    {
      (BaseIDEDependency) new VisualStudioDependency(),
      (BaseIDEDependency) new RiderDependency(),
      (BaseIDEDependency) new VSCodeDependency()
    };

    public override IEnumerable<IToolchainDependency> dependencies
    {
      get => (IEnumerable<IToolchainDependency>) this.ides;
    }

    public override CombinedDependency.CombineType type => CombinedDependency.CombineType.OR;

    protected override bool isAsync => true;

    public override bool canBeInstalled => false;

    public override bool canBeUninstalled => false;

    public override LocalizedString GetLocalizedState(bool includeProgress)
    {
      switch (this.state.m_State)
      {
        case DependencyState.Installed:
          Dictionary<string, ILocElement> args = new Dictionary<string, ILocElement>()
          {
            {
              "STATE",
              (ILocElement) LocalizedString.Id("Options.STATE_TOOLCHAIN[Detected]")
            }
          };
          foreach (BaseIDEDependency ide in this.ides)
          {
            if (ide.isMinVersion)
              args.Add(string.Format("Item{0}", (object) args.Count), (ILocElement) new LocalizedString((string) null, "{NAME}", (IReadOnlyDictionary<string, ILocElement>) new Dictionary<string, ILocElement>()
              {
                {
                  "NAME",
                  (ILocElement) ide.localizedName
                }
              }));
          }
          return new LocalizedString((string) null, "{STATE} (" + string.Join(", ", args.Keys.Skip<string>(1).Select<string, string>((Func<string, string>) (k => "{" + k + "}"))) + ")", (IReadOnlyDictionary<string, ILocElement>) args);
        case DependencyState.NotInstalled:
          return LocalizedString.Id("Options.STATE_TOOLCHAIN[NotDetected]");
        default:
          return base.GetLocalizedState(includeProgress);
      }
    }
  }
}
