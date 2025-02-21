// Decompiled with JetBrains decompiler
// Type: Game.Assets.MapInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.Json;
using Colossal.Mathematics;
using Colossal.UI;
using Colossal.UI.Binding;
using Game.UI;
using Game.UI.Menu;
using System;

#nullable disable
namespace Game.Assets
{
  public class MapInfo : IJsonWritable, IContentPrerequisite, IComparable<MapInfo>
  {
    [Exclude]
    public string id { get; set; }

    public string displayName { get; set; }

    public TextureAsset thumbnail { get; set; }

    public TextureAsset preview { get; set; }

    public string theme { get; set; }

    public Bounds1 temperatureRange { get; set; }

    public float cloudiness { get; set; }

    public float precipitation { get; set; }

    public float latitude { get; set; }

    public float longitude { get; set; }

    public float buildableLand { get; set; }

    public float area { get; set; }

    public float waterAvailability { get; set; }

    public MapMetadataSystem.Resources resources { get; set; }

    public MapMetadataSystem.Connections connections { get; set; }

    public string[] contentPrerequisite { get; set; }

    public bool nameAsCityName { get; set; }

    public int startingYear { get; set; } = -1;

    public MapData mapData { get; set; }

    [Exclude]
    public MapMetadata metaData { get; set; }

    public Guid sessionGuid { get; set; }

    public LocaleAsset[] localeAssets { get; set; }

    public PrefabAsset climate { get; set; }

    [Exclude]
    public bool isReadonly { get; set; }

    [Exclude]
    public string cloudTarget { get; set; }

    [Exclude]
    public bool locked { get; set; }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("id");
      writer.Write(this.id);
      writer.PropertyName("displayName");
      writer.Write(this.displayName);
      writer.PropertyName("thumbnail");
      writer.Write(this.thumbnail.ToUri(MenuHelpers.defaultThumbnail));
      writer.PropertyName("preview");
      writer.Write(this.preview.ToUri(MenuHelpers.defaultPreview));
      writer.PropertyName("theme");
      writer.Write(this.theme);
      writer.PropertyName("temperatureRange");
      writer.Write(this.temperatureRange);
      writer.PropertyName("cloudiness");
      writer.Write(this.cloudiness);
      writer.PropertyName("precipitation");
      writer.Write(this.precipitation);
      writer.PropertyName("latitude");
      writer.Write(this.latitude);
      writer.PropertyName("longitude");
      writer.Write(this.longitude);
      writer.PropertyName("area");
      writer.Write(this.area);
      writer.PropertyName("buildableLand");
      writer.Write(this.buildableLand);
      writer.PropertyName("waterAvailability");
      writer.Write(this.waterAvailability);
      writer.PropertyName("resources");
      writer.Write<MapMetadataSystem.Resources>(this.resources);
      writer.PropertyName("connections");
      writer.Write<MapMetadataSystem.Connections>(this.connections);
      writer.PropertyName("contentPrerequisite");
      writer.Write(this.contentPrerequisite);
      writer.PropertyName("locked");
      writer.Write(this.locked);
      writer.PropertyName("nameAsCityName");
      writer.Write(this.nameAsCityName);
      writer.PropertyName("startingYear");
      writer.Write(this.startingYear);
      writer.PropertyName("isReadonly");
      writer.Write(this.isReadonly);
      writer.PropertyName("cloudTarget");
      writer.Write(this.cloudTarget);
      writer.TypeEnd();
    }

    public int CompareTo(MapInfo other)
    {
      return string.Compare(this.id, other.id, StringComparison.OrdinalIgnoreCase);
    }

    public MapInfo Copy()
    {
      return new MapInfo()
      {
        id = this.id,
        displayName = this.displayName,
        thumbnail = this.thumbnail,
        preview = this.preview,
        theme = this.theme,
        temperatureRange = this.temperatureRange,
        cloudiness = this.cloudiness,
        precipitation = this.precipitation,
        latitude = this.latitude,
        longitude = this.longitude,
        buildableLand = this.buildableLand,
        area = this.area,
        waterAvailability = this.waterAvailability,
        resources = this.resources,
        connections = this.connections,
        contentPrerequisite = this.contentPrerequisite,
        nameAsCityName = this.nameAsCityName,
        startingYear = this.startingYear,
        mapData = this.mapData,
        metaData = this.metaData,
        sessionGuid = this.sessionGuid,
        localeAssets = this.localeAssets,
        climate = this.climate,
        locked = this.locked
      };
    }
  }
}
