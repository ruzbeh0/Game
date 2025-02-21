// Decompiled with JetBrains decompiler
// Type: Game.UI.Thumbnails.ThumbnailCache
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.IO.AssetDatabase;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Game.UI.Thumbnails
{
  public class ThumbnailCache : IDisposable
  {
    private System.Collections.Generic.Dictionary<ThumbnailCache.ThumbnailKey, ThumbnailCache.ThumbnailInfo> m_CacheData;

    public ThumbnailCache()
    {
      this.m_CacheData = new System.Collections.Generic.Dictionary<ThumbnailCache.ThumbnailKey, ThumbnailCache.ThumbnailInfo>();
    }

    private void OnAtlasAssetChanged(AssetChangedEventArgs args)
    {
      if (args.change == ChangeType.BulkAssetsChange)
      {
        this.m_CacheData.Clear();
        foreach (AtlasAsset asset in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<AtlasAsset>(new SearchFilter<AtlasAsset>()))
          this.LoadAtlas(asset);
      }
      else if (args.change == ChangeType.AssetAdded || args.change == ChangeType.AssetUpdated)
      {
        this.LoadAtlas((AtlasAsset) args.asset);
      }
      else
      {
        if (args.change != ChangeType.AssetDeleted)
          return;
        this.UnloadAtlas((AtlasAsset) args.asset);
      }
    }

    private void LoadAtlas(AtlasAsset asset)
    {
      try
      {
        AtlasFrame atlasFrame = asset.Load();
        foreach (AtlasFrame.Entry entry in asset)
        {
          System.Collections.Generic.Dictionary<ThumbnailCache.ThumbnailKey, ThumbnailCache.ThumbnailInfo> cacheData = this.m_CacheData;
          string name = entry.name;
          Rect region = entry.region;
          int width = (int) region.width;
          region = entry.region;
          int height = (int) region.height;
          ThumbnailCache.ThumbnailKey key = new ThumbnailCache.ThumbnailKey(name, width, height);
          cacheData.Add(key, new ThumbnailCache.ThumbnailInfo()
          {
            atlasFrame = atlasFrame,
            region = entry.region,
            status = ThumbnailCache.Status.Ready
          });
        }
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
      }
    }

    private void UnloadAtlas(AtlasAsset asset)
    {
      try
      {
        using (asset)
        {
          asset.Load();
          foreach (AtlasFrame.Entry entry in asset)
            this.m_CacheData.Remove(new ThumbnailCache.ThumbnailKey(entry.name, (int) entry.region.width, (int) entry.region.height));
          asset.Unload(false);
        }
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
      }
    }

    public void Initialize()
    {
      Colossal.IO.AssetDatabase.AssetDatabase.global.onAssetDatabaseChanged.Subscribe<AtlasAsset>(new EventDelegate<AssetChangedEventArgs>(this.OnAtlasAssetChanged), AssetChangedEventArgs.Default);
    }

    public ThumbnailCache.ThumbnailInfo GetCachedThumbnail(ThumbnailCache.ThumbnailKey key)
    {
      ThumbnailCache.ThumbnailInfo thumbnailInfo;
      return this.m_CacheData.TryGetValue(key, out thumbnailInfo) ? thumbnailInfo : (ThumbnailCache.ThumbnailInfo) null;
    }

    public void Refresh()
    {
      foreach (KeyValuePair<ThumbnailCache.ThumbnailKey, ThumbnailCache.ThumbnailInfo> keyValuePair in this.m_CacheData)
        keyValuePair.Value.status = ThumbnailCache.Status.Pending;
    }

    public ThumbnailCache.ThumbnailInfo GetThumbnail(
      object obj,
      int width,
      int height,
      Camera camera = null)
    {
      ThumbnailCache.ThumbnailInfo thumbnail = (ThumbnailCache.ThumbnailInfo) null;
      UnityEngine.Object @object = obj as UnityEngine.Object;
      if (@object != (UnityEngine.Object) null)
        this.m_CacheData.TryGetValue(new ThumbnailCache.ThumbnailKey(@object.name, width, height), out thumbnail);
      return thumbnail;
    }

    public void Update()
    {
    }

    public void Dispose()
    {
      foreach (KeyValuePair<ThumbnailCache.ThumbnailKey, ThumbnailCache.ThumbnailInfo> keyValuePair in this.m_CacheData)
        keyValuePair.Value.baseObjectRef = (object) null;
    }

    public enum Status
    {
      Ready,
      Pending,
      Unavailable,
      Refresh,
    }

    public class ThumbnailInfo : IEquatable<ThumbnailCache.ThumbnailInfo>
    {
      public object baseObjectRef;
      public Camera camera;
      public AtlasFrame atlasFrame;
      public Rect region;
      public ThumbnailCache.Status status;

      public static bool operator ==(
        ThumbnailCache.ThumbnailInfo left,
        ThumbnailCache.ThumbnailInfo right)
      {
        return object.Equals((object) left, (object) right);
      }

      public static bool operator !=(
        ThumbnailCache.ThumbnailInfo left,
        ThumbnailCache.ThumbnailInfo right)
      {
        return !object.Equals((object) left, (object) right);
      }

      public override bool Equals(object obj)
      {
        ThumbnailCache.ThumbnailInfo other = obj as ThumbnailCache.ThumbnailInfo;
        return (object) other != null && this.Equals(other);
      }

      public bool Equals(ThumbnailCache.ThumbnailInfo other)
      {
        object baseObjectRef1 = this.baseObjectRef;
        Camera camera1 = this.camera;
        AtlasFrame atlasFrame1 = this.atlasFrame;
        Rect region1 = this.region;
        object baseObjectRef2 = other.baseObjectRef;
        Camera camera2 = other.camera;
        AtlasFrame atlasFrame2 = other.atlasFrame;
        Rect region2 = other.region;
        object obj = baseObjectRef2;
        return baseObjectRef1 == obj && (UnityEngine.Object) camera1 == (UnityEngine.Object) camera2 && atlasFrame1 == atlasFrame2 && region1 == region2;
      }

      public override int GetHashCode()
      {
        return (this.baseObjectRef, this.camera, this.atlasFrame, this.region.GetHashCode()).GetHashCode();
      }
    }

    public readonly struct ThumbnailKey : IEquatable<ThumbnailCache.ThumbnailKey>
    {
      public ThumbnailKey(string name, int width, int height)
      {
        this.name = name;
        this.width = width;
        this.height = height;
      }

      public string name { get; }

      public int width { get; }

      public int height { get; }

      public static bool operator ==(
        ThumbnailCache.ThumbnailKey left,
        ThumbnailCache.ThumbnailKey right)
      {
        return object.Equals((object) left, (object) right);
      }

      public static bool operator !=(
        ThumbnailCache.ThumbnailKey left,
        ThumbnailCache.ThumbnailKey right)
      {
        return !object.Equals((object) left, (object) right);
      }

      public override bool Equals(object obj)
      {
        return obj is ThumbnailCache.ThumbnailKey other && this.Equals(other);
      }

      public bool Equals(ThumbnailCache.ThumbnailKey other)
      {
        string name1 = this.name;
        int width1 = this.width;
        int height1 = this.height;
        string name2 = other.name;
        int width2 = other.width;
        int height2 = other.height;
        string str = name2;
        return name1 == str && width1 == width2 && height1 == height2;
      }

      public override int GetHashCode() => (this.name, this.width, this.height).GetHashCode();

      public override string ToString()
      {
        return string.Format("{0}_{1}x{2}", (object) this.name, (object) this.width, (object) this.height);
      }
    }
  }
}
