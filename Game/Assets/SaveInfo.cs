// Decompiled with JetBrains decompiler
// Type: Game.Assets.SaveInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.Json;
using Colossal.UI;
using Colossal.UI.Binding;
using Game.UI.Menu;
using System;
using System.Collections.Generic;

#nullable disable
namespace Game.Assets
{
  public class SaveInfo : IJsonWritable, IContentPrerequisite
  {
    [DecodeAlias(new string[] {"previewAsset"})]
    public TextureAsset preview { get; set; }

    public string theme { get; set; }

    public string cityName { get; set; }

    public int population { get; set; }

    public int money { get; set; }

    public int xp { get; set; }

    public SimulationDateTime simulationDate { get; set; }

    public System.Collections.Generic.Dictionary<string, bool> options { get; set; }

    public string[] contentPrerequisite { get; set; }

    public string mapName { get; set; }

    public SaveGameData saveGameData { get; set; }

    public string[] modsEnabled { get; set; }

    [Exclude]
    public string id { get; set; }

    [Exclude]
    public string displayName { get; set; }

    [Exclude]
    public string path { get; set; }

    [Exclude]
    public bool isReadonly { get; set; }

    [Exclude]
    public string cloudTarget { get; set; }

    [Exclude]
    public DateTime lastModified { get; set; }

    public bool autoSave { get; set; }

    [Exclude]
    public SaveGameMetadata metaData { get; set; }

    public Guid sessionGuid { get; set; }

    [Exclude]
    public bool locked { get; set; }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("id");
      writer.Write(this.id);
      writer.PropertyName("displayName");
      writer.Write(this.displayName);
      writer.PropertyName("path");
      writer.Write(this.path);
      writer.PropertyName("preview");
      writer.Write(this.preview.ToUri(MenuHelpers.defaultPreview));
      writer.PropertyName("theme");
      writer.Write(this.theme);
      writer.PropertyName("cityName");
      writer.Write(this.cityName);
      writer.PropertyName("population");
      writer.Write(this.population);
      writer.PropertyName("money");
      writer.Write(this.money);
      writer.PropertyName("xp");
      writer.Write(this.xp);
      writer.PropertyName("simulationDate");
      writer.Write<SimulationDateTime>(this.simulationDate);
      writer.PropertyName("options");
      writer.Write((IReadOnlyDictionary<string, bool>) this.options);
      writer.PropertyName("locked");
      writer.Write(this.locked);
      writer.PropertyName("mapName");
      writer.Write(this.mapName);
      writer.PropertyName("lastModified");
      writer.Write(this.lastModified.ToString("o"));
      writer.PropertyName("isReadonly");
      writer.Write(this.isReadonly);
      writer.PropertyName("cloudTarget");
      writer.Write(this.cloudTarget);
      writer.PropertyName("autoSave");
      writer.Write(this.autoSave);
      writer.PropertyName("modsEnabled");
      writer.Write(this.modsEnabled ?? Array.Empty<string>());
      writer.TypeEnd();
    }

    public SaveInfo Copy()
    {
      return new SaveInfo()
      {
        preview = this.preview,
        theme = this.theme,
        cityName = this.cityName,
        population = this.population,
        money = this.money,
        xp = this.xp,
        simulationDate = this.simulationDate,
        options = this.options != null ? new System.Collections.Generic.Dictionary<string, bool>((IDictionary<string, bool>) this.options) : this.options,
        contentPrerequisite = this.contentPrerequisite != null ? (string[]) this.contentPrerequisite.Clone() : this.contentPrerequisite,
        mapName = this.mapName,
        saveGameData = this.saveGameData,
        id = this.id,
        displayName = this.displayName,
        path = this.path,
        isReadonly = this.isReadonly,
        cloudTarget = this.cloudTarget,
        lastModified = this.lastModified,
        autoSave = this.autoSave,
        metaData = this.metaData,
        sessionGuid = this.sessionGuid,
        locked = this.locked,
        modsEnabled = this.modsEnabled
      };
    }
  }
}
