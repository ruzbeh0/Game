// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.EditorPrefabUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Annotations;
using Colossal.IO.AssetDatabase;
using Colossal.UI;
using Game.Prefabs;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace Game.UI.Editor
{
  public static class EditorPrefabUtils
  {
    private static readonly System.Collections.Generic.Dictionary<System.Type, string[]> s_PrefabTypes = new System.Collections.Generic.Dictionary<System.Type, string[]>();
    private static readonly System.Collections.Generic.Dictionary<System.Type, string[]> s_PrefabTags = new System.Collections.Generic.Dictionary<System.Type, string[]>();
    public static readonly LocalizedString kNone = LocalizedString.Id("Editor.NONE_VALUE");

    public static string GetPrefabTypeName(System.Type type) => type.FullName;

    [CanBeNull]
    public static PrefabBase GetPrefabByID([CanBeNull] string prefabID) => (PrefabBase) null;

    [CanBeNull]
    public static T GetPrefabByID<T>([CanBeNull] string prefabID) where T : PrefabBase
    {
      return default (T);
    }

    [CanBeNull]
    public static string GetPrefabID(PrefabBase prefab)
    {
      string id;
      return (UnityEngine.Object) prefab != (UnityEngine.Object) null && Colossal.IO.AssetDatabase.AssetDatabase.global.resources.prefabsMap.TryGetGuid((UnityEngine.Object) prefab, out id) ? id : (string) null;
    }

    public static string[] GetPrefabTypes(System.Type type)
    {
      string[] prefabTypes;
      if (EditorPrefabUtils.s_PrefabTypes.TryGetValue(type, out prefabTypes))
        return prefabTypes;
      List<string> stringList = new List<string>();
      for (System.Type type1 = type; type1 != (System.Type) null && type1 != typeof (PrefabBase) && typeof (PrefabBase).IsAssignableFrom(type1); type1 = type1.BaseType)
        stringList.Add(EditorPrefabUtils.GetPrefabTypeName(type1));
      return EditorPrefabUtils.s_PrefabTypes[type] = stringList.ToArray();
    }

    public static string[] GetPrefabTags(System.Type type)
    {
      string[] prefabTags;
      if (EditorPrefabUtils.s_PrefabTags.TryGetValue(type, out prefabTags))
        return prefabTags;
      List<string> stringList = new List<string>();
      for (System.Type c = type; c != (System.Type) null && c != typeof (PrefabBase) && typeof (PrefabBase).IsAssignableFrom(c); c = c.BaseType)
      {
        string str = c.Name;
        if (str.Length > 6 && str.EndsWith("Prefab"))
          str = str.Substring(0, str.Length - 6);
        stringList.Add(str.ToLowerInvariant());
      }
      return EditorPrefabUtils.s_PrefabTags[type] = stringList.ToArray();
    }

    public static void SavePrefab(PrefabBase prefab)
    {
      (prefab.asset ?? Colossal.IO.AssetDatabase.AssetDatabase.user.AddAsset(AssetDataPath.Create("StreamingData~/" + prefab.name, prefab.name ?? ""), (ScriptableObject) prefab)).Save(false);
    }

    public static IEnumerable<LocaleAsset> GetLocaleAssets(PrefabBase prefab)
    {
      if ((AssetData) prefab.asset != (IAssetData) null && prefab.asset.database != Colossal.IO.AssetDatabase.AssetDatabase.game)
      {
        foreach (LocaleAsset asset in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<LocaleAsset>(SearchFilter<LocaleAsset>.ByCondition((Func<LocaleAsset, bool>) (a => a.subPath == prefab.asset.subPath))))
          yield return asset;
      }
    }

    public static IEnumerable<EditorPrefabUtils.IconInfo> GetIcons(PrefabBase prefab)
    {
      foreach (ComponentBase comp in prefab.components)
      {
        FieldInfo[] fieldInfoArray = comp.GetType().GetFields();
        for (int index = 0; index < fieldInfoArray.Length; ++index)
        {
          FieldInfo element = fieldInfoArray[index];
          if (!(element.FieldType != typeof (string)))
          {
            CustomFieldAttribute customAttribute = element.GetCustomAttribute<CustomFieldAttribute>();
            if (customAttribute != null && !(customAttribute.Factory != typeof (UIIconField)))
            {
              string uri = (string) element.GetValue((object) comp);
              ImageAsset imageAsset;
              if (UIExtensions.TryGetImageAsset(uri, out imageAsset))
                yield return new EditorPrefabUtils.IconInfo()
                {
                  m_Asset = imageAsset,
                  m_URI = uri,
                  m_Field = element,
                  m_Component = comp
                };
            }
          }
        }
        fieldInfoArray = (FieldInfo[]) null;
      }
    }

    public static LocalizedString GetPrefabLabel(PrefabBase prefab)
    {
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
        return EditorPrefabUtils.kNone;
      if ((AssetData) prefab.asset != (IAssetData) null)
      {
        SourceMeta meta = prefab.asset.GetMeta();
        if (prefab.asset.database == Colossal.IO.AssetDatabase.AssetDatabase<ParadoxMods>.instance)
          return LocalizedString.Value(string.Format("{0} - ({1})", (object) prefab.name, (object) meta.platformID));
        if (prefab.asset.database == Colossal.IO.AssetDatabase.AssetDatabase.user)
        {
          string str = meta.isPackaged ? meta.packageName : prefab.asset.name;
          return LocalizedString.Value(prefab.name + " - (" + str + ")");
        }
      }
      return LocalizedString.Value(prefab.name);
    }

    public static IEnumerable<AssetItem> GetUserImages()
    {
      AssetItem userImage1 = new AssetItem();
      userImage1.guid = new Colossal.Hash128();
      userImage1.displayName = EditorPrefabUtils.kNone;
      yield return userImage1;
      foreach (ImageAsset asset in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<ImageAsset>(SearchFilter<ImageAsset>.ByCondition((Func<ImageAsset, bool>) (a =>
      {
        string subPath = a.GetMeta().subPath;
        return subPath != null && subPath.StartsWith(ScreenUtility.kScreenshotDirectory);
      }))))
      {
        using (asset)
        {
          AssetItem userImage2 = new AssetItem();
          userImage2.guid = asset.guid;
          userImage2.fileName = asset.name;
          userImage2.displayName = (LocalizedString) asset.name;
          userImage2.image = asset.ToUri();
          yield return userImage2;
        }
      }
    }

    public struct IconInfo
    {
      public ImageAsset m_Asset;
      public string m_URI;
      public FieldInfo m_Field;
      public ComponentBase m_Component;
    }
  }
}
