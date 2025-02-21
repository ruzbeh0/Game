// Decompiled with JetBrains decompiler
// Type: Game.PSI.ModTags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game.Assets;
using Game.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.PSI
{
  public static class ModTags
  {
    private static ILog sLog = LogManager.GetLogger("Platforms");
    public static readonly int kMaxTags = 10;
    private static readonly Type[] sExcludePropTypes = new Type[4]
    {
      typeof (PillarObject),
      typeof (TreeObject),
      typeof (PlantObject),
      typeof (NetObject)
    };

    public static void GetTags(
      AssetData asset,
      HashSet<string> tags,
      HashSet<string> typeTags,
      HashSet<string> validTags)
    {
      ModTags.GetAssetTypeTags(asset, tags, typeTags, validTags);
      switch (asset)
      {
        case MapMetadata map:
          ModTags.GetMapTags(map, tags, typeTags, validTags);
          break;
        case SaveGameMetadata save:
          ModTags.GetSaveTags(save, tags, typeTags, validTags);
          break;
        case PrefabAsset prefabAsset:
          if (prefabAsset.Load() is PrefabBase prefab)
          {
            ModTags.GetPrefabTags(prefab, tags, typeTags, validTags);
            break;
          }
          break;
      }
      tags.UnionWith((IEnumerable<string>) typeTags);
      while (tags.Count > ModTags.kMaxTags)
      {
        string p2 = tags.FirstOrDefault<string>((Func<string, bool>) (tag => !typeTags.Contains(tag)));
        if (p2 != null)
        {
          tags.Remove(p2);
        }
        else
        {
          p2 = tags.FirstOrDefault<string>();
          if (p2 == null)
            break;
          tags.Remove(p2);
          typeTags.Remove(p2);
        }
        ModTags.sLog.WarnFormat("Generated mod tags for {0} exceed max count, removing {1}", (object) asset.name, (object) p2);
      }
    }

    private static void GetMapTags(
      MapMetadata map,
      HashSet<string> tags,
      HashSet<string> typeTags,
      HashSet<string> validTags)
    {
      if (!validTags.Contains(map.target.theme))
        return;
      tags.Add(map.target.theme);
    }

    private static void GetSaveTags(
      SaveGameMetadata save,
      HashSet<string> tags,
      HashSet<string> typeTags,
      HashSet<string> validTags)
    {
      if (!validTags.Contains(save.target.theme))
        return;
      tags.Add(save.target.theme);
    }

    private static void GetPrefabTags(
      PrefabBase prefab,
      HashSet<string> tags,
      HashSet<string> typeTags,
      HashSet<string> validTags)
    {
      foreach (string componentTag in ModTags.GetComponentTags((ComponentBase) prefab, validTags, typeof (PrefabBase)))
        typeTags.Add(componentTag);
      foreach (ComponentBase component in prefab.components)
      {
        foreach (string componentTag in ModTags.GetComponentTags(component, validTags, typeof (ComponentBase)))
          tags.Add(componentTag);
      }
    }

    private static IEnumerable<string> GetComponentTags(
      ComponentBase component,
      HashSet<string> validTags,
      Type terminateAtType)
    {
      Type type;
      for (type = component.GetType(); type != terminateAtType && type != (Type) null; type = type.BaseType)
      {
        if (!type.IsDefined(typeof (ExcludeGeneratedModTagAttribute), false))
        {
          string componentTag = type.Name.Replace("Prefab", string.Empty).Replace("Object", string.Empty);
          if (validTags.Contains(componentTag))
            yield return componentTag;
        }
      }
      type = (Type) null;
      foreach (string modTag in component.modTags)
      {
        if (validTags.Contains(modTag))
          yield return modTag;
      }
    }

    private static void GetAssetTypeTags(
      AssetData assetData,
      HashSet<string> tags,
      HashSet<string> typeTags,
      HashSet<string> validTags)
    {
      for (Type type = assetData.GetType(); type != typeof (AssetData); type = type.BaseType)
      {
        if (!type.IsDefined(typeof (ExcludeGeneratedModTagAttribute), false))
        {
          string str = type.Name.Replace("Metadata", "").Replace("Asset", "");
          if (validTags.Contains(str))
            typeTags.Add(str);
        }
      }
      foreach (string modTag in assetData.modTags)
      {
        if (validTags.Contains(modTag))
          typeTags.Add(modTag);
      }
    }

    public static IEnumerable<string> GetEnumFlagTags<T>(T value, T defaultValue) where T : Enum
    {
      bool flag1 = false;
      foreach (T flag2 in Enum.GetValues(typeof (T)))
      {
        if (value.HasFlag((Enum) flag2))
        {
          yield return value.ToString();
          flag1 = true;
        }
      }
      if (!flag1)
        yield return defaultValue.ToString();
    }

    public static bool IsProp(PrefabBase prefab)
    {
      if (!(prefab.GetType() == typeof (StaticObjectPrefab)))
        return false;
      foreach (Type sExcludePropType in ModTags.sExcludePropTypes)
      {
        if (prefab.TryGet(sExcludePropType, out ComponentBase _))
          return false;
      }
      return true;
    }
  }
}
