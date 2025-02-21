// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.AssetUploadUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.PSI.Common;
using Colossal.UI;
using Game.Assets;
using Game.Prefabs;
using Game.UI.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Game.UI.Menu
{
  public static class AssetUploadUtils
  {
    private static ILog sLog = LogManager.GetLogger("AssetUpload");

    public static bool LockLinkType(string url, out string type)
    {
      string str = url.ToLower().Trim();
      foreach (IModsUploadSupport.ExternalLinkInfo kAcceptedType in IModsUploadSupport.ExternalLinkInfo.kAcceptedTypes)
      {
        foreach (string urL in kAcceptedType.m_URLs)
        {
          if (str.StartsWith(urL + "/") && str.Length >= urL.Length + 2)
          {
            type = kAcceptedType.m_Type;
            return true;
          }
          if (str.StartsWith("https://" + urL + "/") && str.Length >= urL.Length + 10)
          {
            type = kAcceptedType.m_Type;
            return true;
          }
        }
      }
      type = (string) null;
      return false;
    }

    public static bool ValidateExternalLink(IModsUploadSupport.ExternalLinkData link)
    {
      string type;
      return string.IsNullOrWhiteSpace(link.m_URL) || AssetUploadUtils.LockLinkType(link.m_URL, out type) && type == link.m_Type;
    }

    public static IModsUploadSupport.ExternalLinkData defaultExternalLink
    {
      get
      {
        return new IModsUploadSupport.ExternalLinkData()
        {
          m_Type = IModsUploadSupport.ExternalLinkInfo.kAcceptedTypes[0].m_Type,
          m_URL = string.Empty
        };
      }
    }

    public static bool ValidateExternalLinks(
      IEnumerable<IModsUploadSupport.ExternalLinkData> links)
    {
      foreach (IModsUploadSupport.ExternalLinkData link in links)
      {
        if (!AssetUploadUtils.ValidateExternalLink(link))
          return false;
      }
      return true;
    }

    public static bool ValidateForumLink(string link)
    {
      return string.IsNullOrWhiteSpace(link) || link.ToLower().Contains("paradoxplaza.com");
    }

    public static AssetData CopyPreviewImage(
      AssetData asset,
      ILocalAssetDatabase database,
      AssetDataPath path)
    {
      try
      {
        switch (asset)
        {
          case ImageAsset imageAsset:
            return (AssetData) imageAsset.Save(ImageAsset.FileFormat.JPG, path, database);
          case TextureAsset textureAsset:
            return (AssetData) textureAsset.SaveAsImageAsset(ImageAsset.FileFormat.JPG, path, database);
        }
      }
      catch (Exception ex)
      {
        AssetUploadUtils.sLog.Error(ex);
      }
      return (AssetData) null;
    }

    public static void CopyAsset(
      AssetData asset,
      ILocalAssetDatabase database,
      System.Collections.Generic.Dictionary<AssetData, AssetData> processed,
      HashSet<IModsUploadSupport.ModInfo.ModDependency> externalReferences,
      bool copyReverseDependencies,
      bool binaryPackAssets,
      int platformID = 0)
    {
      switch (asset)
      {
        case MapMetadata metadata1:
          AssetUploadUtils.CopyMap(metadata1, database, processed);
          break;
        case SaveGameMetadata metadata2:
          AssetUploadUtils.CopySave(metadata2, database, processed);
          break;
        case PrefabAsset prefabAsset:
          AssetUploadUtils.CopyPrefab(prefabAsset, database, processed, externalReferences, copyReverseDependencies, binaryPackAssets, platformID);
          break;
        default:
          AssetUploadUtils.CopyAssetGeneric(asset, database, processed, true);
          break;
      }
    }

    public static void CopyMap(
      MapMetadata metadata,
      ILocalAssetDatabase database,
      System.Collections.Generic.Dictionary<AssetData, AssetData> processed)
    {
      MapInfo mapInfo = metadata.target.Copy();
      MapData mapData = AssetUploadUtils.CopyAssetGeneric<MapData>(mapInfo.mapData, database, processed);
      mapInfo.mapData = mapData;
      if ((AssetData) metadata.target.preview != (IAssetData) null)
      {
        TextureAsset textureAsset = AssetUploadUtils.CopyAssetGeneric<TextureAsset>(mapInfo.preview, database, processed);
        mapInfo.preview = textureAsset;
      }
      if ((AssetData) metadata.target.thumbnail != (IAssetData) null)
      {
        TextureAsset textureAsset = AssetUploadUtils.CopyAssetGeneric<TextureAsset>(mapInfo.thumbnail, database, processed);
        mapInfo.thumbnail = textureAsset;
      }
      if (mapInfo.localeAssets != null)
      {
        LocaleAsset[] localeAssetArray = new LocaleAsset[mapInfo.localeAssets.Length];
        for (int index = 0; index < mapInfo.localeAssets.Length; ++index)
          localeAssetArray[index] = AssetUploadUtils.CopyAssetGeneric<LocaleAsset>(mapInfo.localeAssets[index], database, processed);
        mapInfo.localeAssets = localeAssetArray;
      }
      if ((AssetData) mapInfo.climate != (IAssetData) null)
        mapInfo.climate = AssetUploadUtils.CopyAssetGeneric<PrefabAsset>(mapInfo.climate, database, processed);
      MapMetadata mapMetadata = AssetUploadUtils.CopyAssetGeneric<MapMetadata>(metadata, database, processed);
      mapInfo.metaData = mapMetadata;
      mapMetadata.target = mapInfo;
      mapMetadata.Save(false);
    }

    public static void CopySave(
      SaveGameMetadata metadata,
      ILocalAssetDatabase database,
      System.Collections.Generic.Dictionary<AssetData, AssetData> processed)
    {
      SaveInfo saveInfo = metadata.target.Copy();
      SaveGameData saveGameData = AssetUploadUtils.CopyAssetGeneric<SaveGameData>(saveInfo.saveGameData, database, processed);
      saveInfo.saveGameData = saveGameData;
      if ((AssetData) metadata.target.preview != (IAssetData) null)
      {
        TextureAsset textureAsset = AssetUploadUtils.CopyAssetGeneric<TextureAsset>(saveInfo.preview, database, processed);
        saveInfo.preview = textureAsset;
      }
      SaveGameMetadata saveGameMetadata = AssetUploadUtils.CopyAssetGeneric<SaveGameMetadata>(metadata, database, processed);
      saveInfo.metaData = saveGameMetadata;
      saveGameMetadata.target = saveInfo;
      saveGameMetadata.Save(false);
    }

    public static void CopyPrefab(
      PrefabAsset prefabAsset,
      ILocalAssetDatabase database,
      System.Collections.Generic.Dictionary<AssetData, AssetData> processed,
      HashSet<IModsUploadSupport.ModInfo.ModDependency> externalReferences,
      bool copyReverseDependencies,
      bool binaryPackAssets,
      int platformID = 0)
    {
      HashSet<AssetData> dependencies = new HashSet<AssetData>();
      AssetUploadUtils.CollectPrefabAssetDependencies(prefabAsset, dependencies, copyReverseDependencies);
      foreach (AssetData assetData1 in dependencies)
      {
        if (!processed.ContainsKey(assetData1))
        {
          SourceMeta meta = assetData1.GetMeta();
          if (meta.platformID > 0 && meta.platformID != platformID)
          {
            externalReferences.Add(new IModsUploadSupport.ModInfo.ModDependency()
            {
              m_Id = meta.platformID,
              m_Version = meta.platformVersion
            });
          }
          else
          {
            AssetData assetData2;
            if (binaryPackAssets && assetData1 is PrefabAsset prefabAsset2)
            {
              PrefabBase prefabBase = (PrefabBase) prefabAsset2.Load();
              PrefabBase data = prefabBase.Clone(prefabBase.name);
              PrefabAsset prefabAsset1 = database.AddAsset<PrefabAsset, ScriptableObject>((AssetDataPath) assetData1.name, (ScriptableObject) data, assetData1.guid);
              prefabAsset1.Save(ContentType.Binary, false, true);
              assetData2 = (AssetData) prefabAsset1;
            }
            else
              assetData2 = AssetUploadUtils.CopyAssetGeneric(assetData1, database, processed, !(assetData1 is LocaleAsset));
            processed[assetData1] = assetData2;
          }
        }
      }
    }

    public static T CopyAssetGeneric<T>(
      T asset,
      ILocalAssetDatabase database,
      System.Collections.Generic.Dictionary<AssetData, AssetData> processed,
      bool keepGuid = false)
      where T : AssetData
    {
      AssetData assetData;
      if (processed.TryGetValue((AssetData) asset, out assetData))
        return assetData as T;
      using (Stream readStream = asset.GetReadStream())
      {
        T obj = database.AddAsset<T>((AssetDataPath) asset.name, keepGuid ? asset.guid : new Colossal.Hash128());
        using (Stream writeStream = obj.GetWriteStream())
          readStream.CopyTo(writeStream);
        processed.Add((AssetData) asset, (AssetData) obj);
        return obj;
      }
    }

    public static AssetData CopyAssetGeneric(
      AssetData asset,
      ILocalAssetDatabase database,
      System.Collections.Generic.Dictionary<AssetData, AssetData> processed,
      bool keepGuid = false)
    {
      AssetData assetData1;
      if (processed.TryGetValue(asset, out assetData1))
        return assetData1;
      using (Stream readStream = asset.GetReadStream())
      {
        AssetData assetData2 = database.AddAsset((AssetDataPath) asset.name, asset.GetType(), keepGuid ? asset.guid : new Colossal.Hash128()) as AssetData;
        using (Stream writeStream = assetData2.GetWriteStream())
          readStream.CopyTo(writeStream);
        processed.Add(asset, assetData2);
        return assetData2;
      }
    }

    public static bool TryGetPreview(AssetData asset, out AssetData result)
    {
      switch (asset)
      {
        case SaveGameMetadata saveGameMetadata:
          result = (AssetData) saveGameMetadata.target.preview;
          return result != (IAssetData) null;
        case MapMetadata mapMetadata:
          result = (AssetData) mapMetadata.target.preview;
          return result != (IAssetData) null;
        case PrefabAsset prefabAsset:
          UIObject component;
          ImageAsset imageAsset;
          if (prefabAsset.Load() is PrefabBase prefabBase && prefabBase.TryGet<UIObject>(out component) && UIExtensions.TryGetImageAsset(component.m_Icon, out imageAsset))
          {
            result = (AssetData) imageAsset;
            return true;
          }
          break;
      }
      result = (AssetData) null;
      return false;
    }

    public static string GetImageURI(AssetData asset)
    {
      switch (asset)
      {
        case ImageAsset asset1:
          return asset1.ToUri();
        case TextureAsset asset2:
          return asset2.ToUri((TextureAsset) null, 0);
        default:
          return MenuHelpers.defaultPreview.ToUri((TextureAsset) null, 0);
      }
    }

    public static void CollectPrefabAssetDependencies(
      PrefabAsset prefabAsset,
      HashSet<AssetData> dependencies,
      bool collectReverseDependencies)
    {
      HashSet<PrefabBase> prefabs = new HashSet<PrefabBase>();
      AssetUploadUtils.CollectPrefabDependencies(prefabAsset.Load() as PrefabBase, prefabs, collectReverseDependencies);
      foreach (PrefabBase prefab in prefabs)
      {
        List<AssetData> assetDataList = new List<AssetData>();
        List<AssetData> assets = assetDataList;
        AssetUploadUtils.GetAssets(prefab, assets);
        foreach (AssetData assetData in assetDataList)
        {
          if (assetData.database != Colossal.IO.AssetDatabase.AssetDatabase.game)
            dependencies.Add(assetData);
        }
      }
    }

    public static void CollectPrefabDependencies(
      PrefabBase mainPrefab,
      HashSet<PrefabBase> prefabs,
      bool collectReverseDependencies)
    {
      if (prefabs.Contains(mainPrefab))
        return;
      Stack<PrefabBase> prefabBaseStack = new Stack<PrefabBase>();
      prefabBaseStack.Push(mainPrefab);
      prefabs.Add(mainPrefab);
      PrefabBase prefab;
      while (prefabBaseStack.TryPop(ref prefab))
      {
        List<PrefabBase> prefabBaseList = new List<PrefabBase>();
        List<ComponentBase> list = new List<ComponentBase>();
        prefab.GetComponents<ComponentBase>(list);
        foreach (ComponentBase componentBase in list)
          componentBase.GetDependencies(prefabBaseList);
        if (collectReverseDependencies)
          AssetUploadUtils.CollectExtraPrefabDependencies(prefab, mainPrefab, prefabBaseList);
        foreach (PrefabBase prefabBase in prefabBaseList)
        {
          if ((UnityEngine.Object) prefabBase != (UnityEngine.Object) null && (AssetData) prefabBase.asset != (IAssetData) null && prefabBase.asset.database != Colossal.IO.AssetDatabase.AssetDatabase.game && !prefabs.Contains(prefabBase))
          {
            prefabBaseStack.Push(prefabBase);
            prefabs.Add(prefabBase);
          }
        }
      }
    }

    private static void CollectExtraPrefabDependencies(
      PrefabBase prefab,
      PrefabBase mainPrefab,
      List<PrefabBase> prefabDependencies)
    {
      AssetPackItem component1;
      switch (prefab)
      {
        case ZonePrefab zonePrefab when (UnityEngine.Object) prefab == (UnityEngine.Object) mainPrefab || prefab.TryGet<AssetPackItem>(out component1) && component1.m_Packs != null && ((IEnumerable<PrefabBase>) component1.m_Packs).Contains<PrefabBase>(mainPrefab):
          using (IEnumerator<PrefabAsset> enumerator = Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<PrefabAsset>(new SearchFilter<PrefabAsset>()).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              SpawnableBuilding component2;
              if (enumerator.Current.Load() is PrefabBase prefabBase && prefabBase.TryGet<SpawnableBuilding>(out component2) && (UnityEngine.Object) component2.m_ZoneType == (UnityEngine.Object) zonePrefab)
                prefabDependencies.Add(prefabBase);
            }
            break;
          }
        case AssetPackPrefab assetPackPrefab when (UnityEngine.Object) prefab == (UnityEngine.Object) mainPrefab:
          using (IEnumerator<PrefabAsset> enumerator = Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<PrefabAsset>(new SearchFilter<PrefabAsset>()).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              AssetPackItem component3;
              if (enumerator.Current.Load() is PrefabBase prefabBase && prefabBase.TryGet<AssetPackItem>(out component3) && component3.m_Packs != null && ((IEnumerable<AssetPackPrefab>) component3.m_Packs).Contains<AssetPackPrefab>(assetPackPrefab))
                prefabDependencies.Add(prefabBase);
            }
            break;
          }
      }
    }

    private static void GetAssets(PrefabBase prefab, List<AssetData> assets)
    {
      assets.Add((AssetData) prefab.asset);
      if (prefab is RenderPrefab renderPrefab)
      {
        assets.Add((AssetData) renderPrefab.geometryAsset);
        foreach (SurfaceAsset surfaceAsset in renderPrefab.surfaceAssets)
        {
          assets.Add((AssetData) surfaceAsset);
          surfaceAsset.LoadProperties(false);
          foreach (TextureAsset textureAsset in surfaceAsset.textures.Values)
            assets.Add((AssetData) textureAsset);
        }
      }
      foreach (LocaleAsset localeAsset in EditorPrefabUtils.GetLocaleAssets(prefab))
        assets.Add((AssetData) localeAsset);
      foreach (EditorPrefabUtils.IconInfo icon in EditorPrefabUtils.GetIcons(prefab))
        assets.Add((AssetData) icon.m_Asset);
    }

    public static void CreateThumbnailAtlas(
      System.Collections.Generic.Dictionary<AssetData, AssetData> processed,
      ILocalAssetDatabase database)
    {
      AtlasFrame atlas = new AtlasFrame(0, 0, false, 0);
      HashSet<AssetData> assetDataSet = new HashSet<AssetData>();
      foreach (AssetData key1 in processed.Keys)
      {
        if (key1 is PrefabAsset key2)
        {
          foreach (EditorPrefabUtils.IconInfo icon in EditorPrefabUtils.GetIcons((PrefabBase) key2.Load()))
          {
            AssetData assetData;
            if (processed.TryGetValue((AssetData) icon.m_Asset, out assetData))
            {
              PrefabAsset prefabAsset = (PrefabAsset) processed[(AssetData) key2];
              ComponentBase componentExactly = ((ComponentBase) prefabAsset.Load()).GetComponentExactly(icon.m_Component.GetType());
              Debug.Log((object) string.Format("{0}: {1}\n{2}.{3}: {4}", (object) prefabAsset.name, (object) assetData.name, (object) icon.m_Field.DeclaringType?.Name, (object) icon.m_Field.Name, icon.m_Field.GetValue((object) componentExactly)));
              if (atlas.TryAdd(assetData.name, ((ImageAsset) assetData).Load(-1)))
              {
                icon.m_Field.SetValue((object) componentExactly, (object) "thumbnail://insert thumbnail URI here");
                assetDataSet.Add(assetData);
                prefabAsset.Save(true);
              }
            }
          }
        }
      }
      if (assetDataSet.Count <= 0)
        return;
      database.AddAsset((AssetDataPath) "ThumbnailAtlas", atlas);
      foreach (AssetData asset in assetDataSet)
        database.DeleteAsset<AssetData>(asset);
    }
  }
}
