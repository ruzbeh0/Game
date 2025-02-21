// Decompiled with JetBrains decompiler
// Type: Game.PSI.PdxSdk.Launcher
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Json;
using Colossal.PSI.Environment;
using Game.Assets;
using Game.SceneFlow;
using System;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.PSI.PdxSdk
{
  public static class Launcher
  {
    private const string kLastSaveInfoFileName = "continue_game.json";
    private static readonly string kLastSaveInfoPath = EnvPath.kUserDataPath + "/continue_game.json";

    private static string LocalizedString(string id, string def)
    {
      string str;
      return GameManager.instance.localizationManager.activeDictionary.TryGetValue(id, out str) ? str : def;
    }

    private static string FormatMoney(int money, bool unlimitedMoney)
    {
      return unlimitedMoney ? Launcher.LocalizedString("Menu.UNLIMITED_MONEY_LABEL", "Unlimited") : string.Format(Launcher.LocalizedString("Common.VALUE_MONEY", "{0}¢{1}").Replace("SIGN", "0").Replace("VALUE", "1"), (double) math.sign((float) money) < 0.0 ? (object) "-" : (object) "", (object) money);
    }

    public static void SaveLastSaveMetadata(SaveInfo saveInfo)
    {
      try
      {
        Launcher.SaveInfoData data = new Launcher.SaveInfoData()
        {
          title = saveInfo.cityName,
          desc = string.Format("{0}: {1} {2}: {3}", (object) Launcher.LocalizedString("GameListScreen.POPULATION_LABEL", "Population"), (object) saveInfo.population, (object) Launcher.LocalizedString("GameListScreen.MONEY_LABEL", "Money"), (object) Launcher.FormatMoney(saveInfo.money, saveInfo.options != null && saveInfo.options["unlimitedMoney"])),
          date = saveInfo.lastModified.ToString("s"),
          rawGameVersion = Game.Version.current.shortVersion
        };
        File.WriteAllText(Launcher.kLastSaveInfoPath, JSON.Dump((object) data));
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
      }
    }

    public static void DeleteLastSaveMetadata()
    {
      if (!File.Exists(Launcher.kLastSaveInfoPath))
        return;
      File.Delete(Launcher.kLastSaveInfoPath);
    }

    internal static class LocaleID
    {
      public const string kPopulation = "Population";
      public const string kPopulationID = "GameListScreen.POPULATION_LABEL";
      public const string kMoney = "Money";
      public const string kMoneyID = "GameListScreen.MONEY_LABEL";
      public const string kMoneyValue = "{0}¢{1}";
      public const string kMoneyValueID = "Common.VALUE_MONEY";
      public const string kUnlimitedMoney = "Unlimited";
      public const string kUnlimitedMoneyID = "Menu.UNLIMITED_MONEY_LABEL";
    }

    private class SaveInfoData
    {
      public string title;
      public string desc;
      public string date;
      public string rawGameVersion;
    }
  }
}
