// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.MenuHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.PSI.Common;
using Colossal.UI;
using Colossal.UI.Binding;
using Game.Assets;
using Game.SceneFlow;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.UI.Menu
{
  public static class MenuHelpers
  {
    private static ILog log = LogManager.GetLogger("SceneFlow");
    private static TextureAsset m_DefaultPreview;
    private static TextureAsset m_DefaultThumbnail;
    public const int kPreviewWidth = 680;
    public const int kPreviewHeight = 383;

    public static TextureAsset defaultPreview
    {
      get
      {
        if ((AssetData) MenuHelpers.m_DefaultPreview == (IAssetData) null)
          MenuHelpers.m_DefaultPreview = Colossal.IO.AssetDatabase.AssetDatabase.global.GetAsset<TextureAsset>("cc1e5421d5a16f15bbd580cffdbee7d4");
        return MenuHelpers.m_DefaultPreview;
      }
    }

    public static TextureAsset defaultThumbnail
    {
      get
      {
        if ((AssetData) MenuHelpers.m_DefaultThumbnail == (IAssetData) null)
          MenuHelpers.m_DefaultThumbnail = Colossal.IO.AssetDatabase.AssetDatabase.global.GetAsset<TextureAsset>("735aa687f0dd7cda5e7d1aa4c4987b26");
        return MenuHelpers.m_DefaultThumbnail;
      }
    }

    public static SaveGameMetadata GetLastModifiedSave()
    {
      SaveGameMetadata lastModifiedSave = (SaveGameMetadata) null;
      DateTime dateTime = DateTime.MinValue;
      foreach (SaveGameMetadata asset in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<SaveGameMetadata>(new SearchFilter<SaveGameMetadata>()))
      {
        DateTime lastModified = asset.target.lastModified;
        if (lastModified > dateTime)
        {
          dateTime = lastModified;
          lastModifiedSave = asset;
        }
      }
      return lastModifiedSave;
    }

    public static bool hasPreviouslySavedGame
    {
      get
      {
        SaveGameMetadata saveGameMetadata = GameManager.instance.settings.userState.lastSaveGameMetadata;
        return saveGameMetadata != null && saveGameMetadata.isValidSaveGame;
      }
    }

    public static void UpdateMeta<T>(ValueBinding<List<T>> binding, Func<Metadata<T>, bool> filter = null) where T : IContentPrerequisite
    {
      List<T> objList = binding.value;
      objList.Clear();
      foreach (Metadata<T> asset in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<Metadata<T>>(new SearchFilter<Metadata<T>>()))
      {
        try
        {
          if (asset.target.contentPrerequisite != null)
          {
            if (!((IEnumerable<string>) asset.target.contentPrerequisite).All<string>((Func<string, bool>) (x =>
            {
              DlcId dlcId = PlatformManager.instance.GetDlcId(x);
              return dlcId != DlcId.Invalid && PlatformManager.instance.IsDlcOwned(dlcId);
            })))
              continue;
          }
          if (filter != null)
          {
            if (!filter(asset))
              continue;
          }
          objList.Add(asset.target);
        }
        catch (Exception ex)
        {
          MenuHelpers.log.WarnFormat(ex, "An error occured while updating {0}", (object) asset);
        }
      }
      binding.TriggerUpdate();
    }

    public static List<string> GetAvailableCloudTargets()
    {
      return Colossal.IO.AssetDatabase.AssetDatabase.global.GetAvailableRemoteStorages().Select<(string, ILocalAssetDatabase), string>((Func<(string, ILocalAssetDatabase), string>) (x => x.name)).ToList<string>();
    }

    public static (string name, ILocalAssetDatabase db) GetSanitizedCloudTarget(string cloudTarget)
    {
      (string, ILocalAssetDatabase) sanitizedCloudTarget = ();
      foreach ((string name, ILocalAssetDatabase db) availableRemoteStorage in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAvailableRemoteStorages())
      {
        if (availableRemoteStorage.name == cloudTarget)
          return availableRemoteStorage;
        if (availableRemoteStorage.name == "Local")
          sanitizedCloudTarget = availableRemoteStorage;
      }
      return sanitizedCloudTarget;
    }

    public class SaveGamePreviewSettings
    {
      public bool stylized { get; set; }

      public float stylizedRadius { get; set; }

      public TextureAsset overlayImage { get; set; }

      public SaveGamePreviewSettings() => this.SetDefaults();

      public void SetDefaults()
      {
        this.stylized = false;
        this.stylizedRadius = 0.0f;
        this.overlayImage = (TextureAsset) null;
      }

      public void FromUri(UrlQuery query)
      {
        bool result1;
        if (query.Read("stylized", out result1))
          this.stylized = result1;
        float result2;
        if (query.Read("stylizedRadius", out result2))
          this.stylizedRadius = result2;
        TextureAsset result3;
        if (!query.ReadAsset<TextureAsset>("overlayImage", out result3))
          return;
        this.overlayImage = result3;
      }

      public string ToUri()
      {
        return string.Format("stylized={0}&stylizedRadius={1}&overlayImage={2}", (object) this.stylized, (object) this.stylizedRadius, (object) this.overlayImage?.guid);
      }
    }
  }
}
