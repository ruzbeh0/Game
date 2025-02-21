// Decompiled with JetBrains decompiler
// Type: Game.Assets.SaveGameMetadata
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.IO.AssetDatabase;
using Colossal.PSI.Common;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Game.Assets
{
  public class SaveGameMetadata : Metadata<SaveInfo>
  {
    public const string kExtension = ".SaveGameMetadata";
    public static readonly Func<string> kPersistentLocation = (Func<string>) (() => "Saves/" + PlatformManager.instance.userSpecificPath);

    protected override void OnPostLoad()
    {
      if (this.state != LoadState.Full)
        return;
      if (!this.database.dataSource.Contains(this.guid))
        return;
      this.target.id = this.identifier;
      SourceMeta meta = this.GetMeta();
      this.target.metaData = this;
      PackageAsset assetData1;
      if (meta.isPackaged && this.database.TryGetAsset<PackageAsset>(meta.package, out assetData1))
        this.target.displayName = assetData1.GetMeta().displayName;
      else
        this.target.displayName = meta.displayName;
      this.target.path = meta.uri;
      this.target.isReadonly = !meta.belongsToCurrentUser;
      this.target.lastModified = meta.lastWriteTime.ToLocalTime();
      this.target.cloudTarget = meta.remoteStorageSourceName;
      if (!((AssetData) this.target.saveGameData == (IAssetData) null))
        return;
      SaveGameData assetData2;
      if (this.database.TryGetAsset<SaveGameData>(Hash128.CreateGuid(Path.ChangeExtension(meta.uri, SaveGameData.kExtensions[1])), out assetData2))
      {
        this.target.saveGameData = assetData2;
      }
      else
      {
        if (!meta.isPackaged)
          return;
        this.target.saveGameData = this.database.GetAsset<SaveGameData>(SearchFilter<SaveGameData>.ByCondition((Func<SaveGameData, bool>) (a => a.GetMeta().package == meta.package)));
      }
    }

    public bool isValidSaveGame
    {
      get
      {
        return this.isValid && (AssetData) this.target.saveGameData != (IAssetData) null && !this.target.isReadonly && this.target.saveGameData.isValid;
      }
    }

    public override IEnumerable<string> modTags
    {
      get
      {
        foreach (string modTag in base.modTags)
          yield return modTag;
        yield return "Savegame";
      }
    }
  }
}
