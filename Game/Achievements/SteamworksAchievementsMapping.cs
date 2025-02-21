// Decompiled with JetBrains decompiler
// Type: Game.Achievements.SteamworksAchievementsMapping
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Steamworks;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Achievements
{
  [Preserve]
  public class SteamworksAchievementsMapping : SteamworksAchievementsMapper
  {
    public SteamworksAchievementsMapping()
    {
      this.Map(Game.Achievements.Achievements.MyFirstCity, "ACH_MYFIRSTCITY");
      this.Map(Game.Achievements.Achievements.TheInspector, "ACH_THEINSPECTOR");
      this.Map(Game.Achievements.Achievements.HappytobeofService, "ACH_HAPPYTOBEOFSERVICE");
      this.Map(Game.Achievements.Achievements.RoyalFlush, "ACH_ROYALFLUSH");
      this.Map(Game.Achievements.Achievements.KeyToTheCity, "ACH_KEYTOTHECITY");
      this.Map(Game.Achievements.Achievements.SixFigures, "ACH_SIXFIGURES", "STAT_POPULATION");
      this.Map(Game.Achievements.Achievements.GoAnywhere, "ACH_GOANYWHERE", "STAT_TRANSPORTLINES");
      this.Map(Game.Achievements.Achievements.TheSizeOfGolfBalls, "ACH_THESIZEOFGOLFBALLS");
      this.Map(Game.Achievements.Achievements.OutforaSpin, "ACH_OUTFORASPIN");
      this.Map(Game.Achievements.Achievements.NowTheyreAllAshTrees, "ACH_NOWTHEYREALLASHTREES");
      this.Map(Game.Achievements.Achievements.ZeroEmission, "ACH_ZEROEMISSION");
      this.Map(Game.Achievements.Achievements.UpAndAway, "ACH_UPANDAWAY", "STAT_AIRPORTS");
      this.Map(Game.Achievements.Achievements.MakingAMark, "ACH_MAKINGAMARK", "STAT_SIGNATUREBUILDINGS");
      this.Map(Game.Achievements.Achievements.EverythingTheLightTouches, "ACH_EVERYTHINGTHELIGHTTOUCHES", "STAT_MAPTILES");
      this.Map(Game.Achievements.Achievements.CallingtheShots, "ACH_CALLINGTHESHOTS", "STAT_CALLINGTHESHOTS");
      this.Map(Game.Achievements.Achievements.WideVariety, "ACH_WIDEVARIETY", "STAT_WIDEVARIETY");
      this.Map(Game.Achievements.Achievements.ExecutiveDecision, "ACH_EXECUTIVEDECISION");
      this.Map(Game.Achievements.Achievements.AllSmiles, "ACH_ALLSMILES");
      this.Map(Game.Achievements.Achievements.YouLittleStalker, "ACH_YOULITTLESTALKER");
      this.Map(Game.Achievements.Achievements.IMadeThis, "ACH_IMADETHIS");
      this.Map(Game.Achievements.Achievements.Cartography, "ACH_CARTOGRAPHY");
      this.Map(Game.Achievements.Achievements.TheExplorer, "ACH_THEEXPLORER", "STAT_MAPTILES");
      this.Map(Game.Achievements.Achievements.TheLastMileMarker, "ACH_THELASTMILEMARKER", "STAT_MILESTONES");
      this.Map(Game.Achievements.Achievements.FourSeasons, "ACH_FOURSEASONS");
      this.Map(Game.Achievements.Achievements.Spiderwebbing, "ACH_SPIDERWEBBING", "STAT_TRANSPORTLINES");
      this.Map(Game.Achievements.Achievements.Snapshot, "ACH_SNAPSHOT");
      this.Map(Game.Achievements.Achievements.ThisIsNotMyHappyPlace, "ACH_THISISNOTMYHAPPYPLACE");
      this.Map(Game.Achievements.Achievements.TheArchitect, "ACH_THEARCHITECT", "STAT_SIGNATUREBUILDINGS");
      this.Map(Game.Achievements.Achievements.SimplyIrresistible, "ACH_SIMPLYIRRESISTIBLE");
      this.Map(Game.Achievements.Achievements.TopoftheClass, "ACH_TOPOFTHECLASS");
      this.Map(Game.Achievements.Achievements.TheDeepEnd, "ACH_THEDEEPEND", "STAT_LOAN");
      this.Map(Game.Achievements.Achievements.Groundskeeper, "ACH_GROUNDSKEEPER", "STAT_PARKS");
      this.Map(Game.Achievements.Achievements.ColossalGardener, "ACH_COLOSSALGARDENER");
      this.Map(Game.Achievements.Achievements.StrengthThroughDiversity, "ACH_STRENGTHTHROUGHDIVERSITY");
      this.Map(Game.Achievements.Achievements.SquasherDowner, "ACH_SQUASHERDOWNER", "STAT_BUILDINGSBULLDOZED");
      this.Map(Game.Achievements.Achievements.ALittleBitofTLC, "ACH_ALITTLEBITOFTLC", "STAT_TREATEDCITIZENS");
      this.Map(Game.Achievements.Achievements.WelcomeOneandAll, "ACH_WELCOMEONEANDALL", "STAT_TOURISTS");
      this.Map(Game.Achievements.Achievements.OneofEverything, "ACH_ONEOFEVERYTHING");
    }
  }
}
