// Decompiled with JetBrains decompiler
// Type: Game.Dlc.PdxSdkDlcsMapping
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.PdxSdk;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Dlc
{
  [Preserve]
  public class PdxSdkDlcsMapping : PdxSdkDlcMapper
  {
    private static readonly string[] kCS1TreasureHuntIds = new string[8]
    {
      "BusCO01",
      "BusCO02",
      "BusCOMirrored01",
      "BusCOMirrored02",
      "TramCarCO01",
      "TramEngineCO01",
      "AirplanePassengerCO01",
      "FountainPlaza01"
    };

    public PdxSdkDlcsMapping()
    {
      this.Map(Game.Dlc.Dlc.CS1TreasureHunt, PdxSdkDlcsMapping.kCS1TreasureHuntIds);
    }
  }
}
