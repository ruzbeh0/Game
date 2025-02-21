// Decompiled with JetBrains decompiler
// Type: Game.Dlc.SteamworksDlcsMapping
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Steamworks;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Dlc
{
  [Preserve]
  public class SteamworksDlcsMapping : SteamworksDlcMapper
  {
    private const uint kProjectWashingtonId = 2427731;
    private const uint kProjectCaliforniaId = 2427730;
    private const uint kExpansionPass = 2472660;
    private const uint kProjectFloridaId = 2427740;
    private const uint kProjectNewJerseyId = 2427743;

    public SteamworksDlcsMapping()
    {
      this.Map(Game.Dlc.Dlc.LandmarkBuildings, 2427731U);
      this.Map(Game.Dlc.Dlc.SanFranciscoSet, 2427730U);
    }
  }
}
