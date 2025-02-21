// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.Dependencies.VSCodeDependency
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using CliWrap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Game.Modding.Toolchain.Dependencies
{
  public class VSCodeDependency : BaseIDEDependency
  {
    public override string name => "VS Code";

    public override string icon => "Media/Toolchain/VSCode.svg";

    public override bool canBeInstalled => false;

    public override bool canBeUninstalled => false;

    public override string minVersion => "1.86";

    protected override async Task<string> GetIDEVersion(CancellationToken token)
    {
      string installedVersion = string.Empty;
      List<string> errorText = new List<string>();
      try
      {
        CommandResult commandResult = await Cli.Wrap("code").WithArguments("--version").WithStandardOutputPipe(PipeTarget.ToDelegate((Action<string>) (l =>
        {
          if (!string.IsNullOrEmpty(installedVersion))
            return;
          installedVersion = l;
        }))).WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => errorText.Add(l)))).WithValidation(CommandResultValidation.None).ExecuteAsync(token).ConfigureAwait(false);
      }
      catch (Win32Exception ex)
      {
        if (ex.ErrorCode != -2147467259)
          ToolchainDependencyManager.log.Error((Exception) ex, (object) "Failed to get VSCode version");
      }
      catch (Exception ex)
      {
        ToolchainDependencyManager.log.Error(ex, (object) "Failed to get VSCode version");
      }
      if (errorText.Count > 0)
        IToolchainDependency.log.Warn((object) string.Join<string>('\n', (IEnumerable<string>) errorText));
      return installedVersion;
    }
  }
}
