// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.BetaFilter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;

#nullable disable
namespace Game.UI.Menu
{
  public static class BetaFilter
  {
    private static HashSet<string> s_Options = new HashSet<string>();

    public static IReadOnlyCollection<string> options
    {
      get => (IReadOnlyCollection<string>) BetaFilter.s_Options;
    }

    public static void AddOption(string option) => BetaFilter.s_Options.Add(option);

    public static void AddOptions(params string[] options)
    {
      BetaFilter.s_Options.UnionWith((IEnumerable<string>) options);
    }

    static BetaFilter() => BetaFilter.AddOptions("Input.Editor", "Modding");
  }
}
