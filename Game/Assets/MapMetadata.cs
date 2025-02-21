// Decompiled with JetBrains decompiler
// Type: Game.Assets.MapMetadata
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.PSI.Common;
using System;

#nullable disable
namespace Game.Assets
{
  public class MapMetadata : Metadata<MapInfo>
  {
    public const string kExtension = ".MapMetadata";
    public static readonly Func<string> kPersistentLocation = (Func<string>) (() => "Maps/" + PlatformManager.instance.userSpecificPath);

    protected override void OnPostLoad()
    {
      if (this.state != LoadState.Full || !this.database.dataSource.Contains(this.guid))
        return;
      this.target.id = this.identifier;
      SourceMeta meta = this.GetMeta();
      this.target.metaData = this;
      this.target.isReadonly = !meta.belongsToCurrentUser;
      this.target.cloudTarget = meta.remoteStorageSourceName;
    }
  }
}
