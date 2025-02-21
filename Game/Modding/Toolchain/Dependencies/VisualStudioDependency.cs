// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.Dependencies.VisualStudioDependency
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using CliWrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Game.Modding.Toolchain.Dependencies
{
  public class VisualStudioDependency : BaseIDEDependency
  {
    public override string name => "Visual Studio";

    public override string icon => "Media/Toolchain/VisualStudio.svg";

    public override bool canBeInstalled => false;

    public override bool canBeUninstalled => false;

    public static string vsWhere => ToolchainDependencyManager.kGameToolingPath + "/vswhere.exe";

    public override string minVersion => "17.8";

    protected override Task<string> GetIDEVersion(CancellationToken token)
    {
      try
      {
        return VisualStudioDependency.GetIDEVersion(token, "-prerelease -format json");
      }
      catch
      {
        try
        {
          return VisualStudioDependency.GetIDEVersion(token, "-prerelease -format json");
        }
        catch (Exception ex)
        {
          ToolchainDependencyManager.log.Error(ex, (object) "Failed to get Visual Studio version");
          return Task.FromResult<string>(string.Empty);
        }
      }
    }

    private static async Task<string> GetIDEVersion(CancellationToken token, string arguments)
    {
      VsWhereResult vsWhereResult = await VisualStudioDependency.QueryVsWhere(token, arguments);
      return ((IEnumerable<VsWhereEntry>) vsWhereResult.entries).Any<VsWhereEntry>() ? vsWhereResult.entries[0].catalog.buildVersion : string.Empty;
    }

    private static async Task<VsWhereResult> QueryVsWhere(CancellationToken token, string arguments)
    {
      StringBuilder vsWhereResult = new StringBuilder();
      List<string> errorText = new List<string>();
      vsWhereResult.AppendLine("{ \"entries\": ");
      CommandResult commandResult = await Cli.Wrap(VisualStudioDependency.vsWhere).WithArguments("-prerelease -latest -format json").WithStandardOutputPipe(PipeTarget.ToDelegate((Action<string>) (l => vsWhereResult.Append(l)))).WithStandardErrorPipe(PipeTarget.ToDelegate((Action<string>) (l => errorText.Add(l)))).WithValidation(CommandResultValidation.None).ExecuteAsync(token).ConfigureAwait(false);
      vsWhereResult.AppendLine("}");
      if (errorText.Count > 0)
        IToolchainDependency.log.Warn((object) string.Join<string>('\n', (IEnumerable<string>) errorText));
      return VsWhereResult.FromJson(vsWhereResult.ToString());
    }
  }
}
