// Decompiled with JetBrains decompiler
// Type: Game.UI.GameUIResourceHandler
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using cohtml.Net;
using Colossal.PSI.Common;
using Colossal.UI;
using Game.Prefabs;
using Game.SceneFlow;
using Game.UI.Menu;
using Game.UI.Thumbnails;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;

#nullable disable
namespace Game.UI
{
  public class GameUIResourceHandler : DefaultResourceHandler
  {
    private const string kScreencaptureScheme = "screencapture";
    public const string kScreencaptureProtocol = "screencapture://";
    public const string kScreenshotOpString = "Screenshot";
    private const string kThumbnailScheme = "thumbnail";
    public const string kThumbnailProtocol = "thumbnail://";
    private const string kUserAvatarScheme = "useravatar";
    public const string kUserAvatarProtocol = "useravatar://";
    private Dictionary<string, Camera> m_HostCameraCache = new Dictionary<string, Camera>();

    public GameUIResourceHandler(MonoBehaviour coroutineHost) => this.coroutineHost = coroutineHost;

    public override void OnResourceRequest(IResourceRequest request, IResourceResponse response)
    {
      try
      {
        this.coroutineHost.StartCoroutine(this.TryGetResourceRequestAsync(new GameUIResourceHandler.GameResourceRequestData(request, response)));
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
        response.Finish(ResourceResponse.Status.Failure);
      }
    }

    private Camera GetCameraFromHost(string host)
    {
      Camera cameraFromHost1;
      if (this.m_HostCameraCache.TryGetValue(host, out cameraFromHost1))
      {
        if ((UnityEngine.Object) cameraFromHost1 != (UnityEngine.Object) null && host == cameraFromHost1.tag.ToLowerInvariant())
          return cameraFromHost1;
        this.m_HostCameraCache.Remove(host);
      }
      Camera cameraFromHost2 = (Camera) null;
      for (int index = 0; index < SceneManager.sceneCount; ++index)
      {
        Scene sceneAt = SceneManager.GetSceneAt(index);
        List<GameObject> rootGameObjects = new List<GameObject>(sceneAt.rootCount);
        sceneAt.GetRootGameObjects(rootGameObjects);
        foreach (GameObject gameObject in rootGameObjects)
        {
          foreach (Camera componentsInChild in gameObject.GetComponentsInChildren<Camera>(true))
          {
            if (host == componentsInChild.tag.ToLowerInvariant())
            {
              cameraFromHost2 = componentsInChild;
              break;
            }
          }
        }
      }
      if ((UnityEngine.Object) cameraFromHost2 != (UnityEngine.Object) null)
        this.m_HostCameraCache.Add(host, cameraFromHost2);
      return cameraFromHost2;
    }

    private RenderTexture SetupCameraTarget(string name, Camera camera, int width, int height)
    {
      RenderTexture targetTexture = camera.targetTexture;
      if (targetTexture.name == string.Empty)
        targetTexture.name = name;
      camera.targetTexture = (RenderTexture) null;
      targetTexture.Release();
      targetTexture.width = width;
      targetTexture.height = height;
      targetTexture.Create();
      camera.targetTexture = targetTexture;
      return targetTexture;
    }

    private IEnumerator RequestScreenshot(
      GameUIResourceHandler.GameResourceRequestData requestData)
    {
      GameUIResourceHandler uiResourceHandler = this;
      uiResourceHandler.AddPendingRequest((DefaultResourceHandler.RequestData) requestData);
      yield return (object) new WaitForEndOfFrame();
      if (!requestData.Aborted)
      {
        try
        {
          Camera cameraFromHost = uiResourceHandler.GetCameraFromHost(requestData.UriBuilder.Host);
          if ((UnityEngine.Object) cameraFromHost != (UnityEngine.Object) null)
          {
            UrlQuery query = new UrlQuery(requestData.UriBuilder.Query);
            int result1;
            if (!query.Read("width", out result1))
              result1 = cameraFromHost.pixelWidth;
            int result2;
            if (!query.Read("height", out result2))
              result2 = cameraFromHost.pixelHeight;
            string result3;
            if (!query.Read("op", out result3))
              result3 = (string) null;
            UserImagesManager.ResourceType result4;
            if (!query.Read<UserImagesManager.ResourceType>("alloc", out result4))
              result4 = UserImagesManager.ResourceType.Managed;
            bool isDynamic = false;
            bool result5;
            if (query.Read("liveView", out result5))
              isDynamic = result5;
            MenuHelpers.SaveGamePreviewSettings settings = new MenuHelpers.SaveGamePreviewSettings();
            settings.FromUri(query);
            string name = Uri.UnescapeDataString(requestData.UriBuilder.Path.Substring(1));
            Texture texture;
            if (result3 == "Screenshot")
            {
              texture = uiResourceHandler.userImagesManager.GetUserImageTarget(name, result1, result2);
              if ((UnityEngine.Object) texture == (UnityEngine.Object) null)
                texture = (Texture) ScreenCaptureHelper.CreateRenderTarget(name, result1, result2);
              if (texture is RenderTexture destination)
                ScreenCaptureHelper.CaptureScreenshot(cameraFromHost, destination, settings);
            }
            else
            {
              texture = uiResourceHandler.userImagesManager.GetUserImageTarget(name, result1, result2);
              if ((UnityEngine.Object) texture == (UnityEngine.Object) null)
                texture = (Texture) uiResourceHandler.SetupCameraTarget(name, cameraFromHost, result1, result2);
            }
            if ((UnityEngine.Object) texture != (UnityEngine.Object) null)
            {
              requestData.ReceiveUserImage(uiResourceHandler.userImagesManager.GetUserImageData(texture, result4, isDynamic));
              uiResourceHandler.RespondWithSuccess((DefaultResourceHandler.RequestData) requestData);
              uiResourceHandler.RemovePendingRequest((DefaultResourceHandler.RequestData) requestData);
            }
            else
            {
              requestData.Error = "No available render target for '" + requestData.UriBuilder.Host + "'";
              uiResourceHandler.CheckForFailedRequest((DefaultResourceHandler.RequestData) requestData);
            }
          }
          else
          {
            requestData.Error = "No camera '" + requestData.UriBuilder.Host + "'";
            uiResourceHandler.CheckForFailedRequest((DefaultResourceHandler.RequestData) requestData);
          }
        }
        catch (Exception ex)
        {
          requestData.Error = ex.ToString();
          uiResourceHandler.CheckForFailedRequest((DefaultResourceHandler.RequestData) requestData);
        }
      }
    }

    private IEnumerator TryGetResourceRequestAsync(
      GameUIResourceHandler.GameResourceRequestData requestData)
    {
      GameUIResourceHandler uiResourceHandler = this;
      DefaultResourceHandler.log.Debug((object) string.Format("Requesting resource with URL: {0}", (object) requestData.UriBuilder.Uri));
      if (requestData.IsDataRequest)
      {
        uiResourceHandler.RespondWithSuccess((DefaultResourceHandler.RequestData) requestData);
        yield return (object) 0;
        uiResourceHandler.RemovePendingRequest((DefaultResourceHandler.RequestData) requestData);
      }
      else if (requestData.IsScreenshotRequest)
        yield return (object) uiResourceHandler.RequestScreenshot(requestData);
      else if (requestData.IsThumbnailRequest)
        yield return (object) uiResourceHandler.TryThumbnailRequestAsync(requestData);
      else if (requestData.IsUserAvatarRequest)
        yield return (object) uiResourceHandler.RequestUserAvatarAsync(requestData);
      else
        yield return (object) uiResourceHandler.TryPreloadedResourceRequestAsync((DefaultResourceHandler.ResourceRequestData) requestData);
    }

    private bool UpdateTexture(
      ref Texture target,
      string name,
      (int width, int height, byte[] data) p)
    {
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
      {
        Texture2D texture2D1 = new Texture2D(p.width, p.height, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None);
        texture2D1.name = name;
        texture2D1.hideFlags = HideFlags.HideAndDontSave;
        Texture2D texture2D2 = texture2D1;
        texture2D2.LoadRawTextureData(p.data);
        texture2D2.Apply();
        texture2D2.IncrementUpdateCount();
        target = (Texture) texture2D2;
        return true;
      }
      if (!(target is Texture2D texture2D))
        return false;
      texture2D.LoadRawTextureData(p.data);
      texture2D.Apply();
      texture2D.IncrementUpdateCount();
      return true;
    }

    private IEnumerator RequestUserAvatarAsync(
      GameUIResourceHandler.GameResourceRequestData requestData)
    {
      GameUIResourceHandler uiResourceHandler = this;
      uiResourceHandler.AddPendingRequest((DefaultResourceHandler.RequestData) requestData);
      AvatarSize result1;
      new UrlQuery(requestData.UriBuilder.Query).Read<AvatarSize>("size", out result1);
      string path = requestData.UriBuilder.Path;
      string name = Uri.UnescapeDataString(path.Substring(1, path.Length - 1));
      Task<(int width, int height, byte[] data)> avatarTask = PlatformManager.instance.GetAvatar(result1);
      yield return (object) new WaitUntil((Func<bool>) (() => avatarTask.IsCompleted));
      if (!requestData.Aborted && !avatarTask.IsFaulted)
      {
        (int width, int height, byte[] data) result2 = avatarTask.Result;
        if (result2.data == null)
        {
          requestData.Error = "Getting user avatar failed.";
          uiResourceHandler.CheckForFailedRequest((DefaultResourceHandler.RequestData) requestData);
        }
        else
        {
          Texture userImageTarget = uiResourceHandler.userImagesManager.GetUserImageTarget(name, result2.width, result2.height);
          if (uiResourceHandler.UpdateTexture(ref userImageTarget, name, result2))
          {
            requestData.ReceiveUserImage(uiResourceHandler.userImagesManager.GetUserImageData(userImageTarget, UserImagesManager.ResourceType.Managed, false));
            uiResourceHandler.RespondWithSuccess((DefaultResourceHandler.RequestData) requestData);
            uiResourceHandler.RemovePendingRequest((DefaultResourceHandler.RequestData) requestData);
          }
          else
          {
            requestData.Error = "No available render target.";
            uiResourceHandler.CheckForFailedRequest((DefaultResourceHandler.RequestData) requestData);
          }
        }
      }
    }

    private IEnumerator TryThumbnailRequestAsync(
      GameUIResourceHandler.GameResourceRequestData requestData)
    {
      GameUIResourceHandler uiResourceHandler = this;
      ThumbnailCache thumbnailCache = GameManager.instance?.thumbnailCache;
      if (thumbnailCache != null)
      {
        Camera cameraFromHost = uiResourceHandler.GetCameraFromHost(requestData.UriBuilder.Host);
        ThumbnailCache.ThumbnailInfo info = (ThumbnailCache.ThumbnailInfo) null;
        try
        {
          bool flag = (UnityEngine.Object) cameraFromHost != (UnityEngine.Object) null;
          UrlQuery urlQuery = new UrlQuery(requestData.UriBuilder.Query);
          int result1;
          if (!urlQuery.Read("width", out result1))
            result1 = flag ? cameraFromHost.pixelWidth : 0;
          int result2;
          if (!urlQuery.Read("height", out result2))
            result2 = flag ? cameraFromHost.pixelHeight : 0;
          string paramName = requestData.UriBuilder.Path.Substring(1);
          string[] strArray = paramName.Split('/', StringSplitOptions.None);
          string type = strArray.Length == 2 ? Uri.UnescapeDataString(strArray[0]) : throw new ArgumentException("Invalid url path {0}", paramName);
          string name = Uri.UnescapeDataString(strArray[1]);
          PrefabBase prefab;
          // ISSUE: reference to a compiler-generated method
          if (World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>().TryGetPrefab(new PrefabID(type, name), out prefab))
            info = thumbnailCache.GetThumbnail((object) prefab, result1, result2, cameraFromHost);
        }
        catch (Exception ex)
        {
          requestData.Error = ex.ToString();
          uiResourceHandler.CheckForFailedRequest((DefaultResourceHandler.RequestData) requestData);
          yield break;
        }
        uiResourceHandler.AddPendingRequest((DefaultResourceHandler.RequestData) requestData);
        if (info != (ThumbnailCache.ThumbnailInfo) null)
        {
          while (info.status == ThumbnailCache.Status.Pending)
          {
            if (requestData.Aborted)
              yield break;
            else
              yield return (object) 0;
          }
        }
        if (!requestData.Aborted)
        {
          if (!(info == (ThumbnailCache.ThumbnailInfo) null))
          {
            if (info.status != ThumbnailCache.Status.Unavailable)
            {
              try
              {
                Texture texture = (Texture) info.atlasFrame.texture;
                Rect region = info.region;
                if ((UnityEngine.Object) texture != (UnityEngine.Object) null)
                {
                  requestData.ReceiveUserImage(uiResourceHandler.userImagesManager.GetUserImageData(texture, UserImagesManager.ResourceType.Unmanaged, true, region, ResourceResponse.UserImageData.AlphaPremultiplicationMode.NonPremultiplied));
                  uiResourceHandler.RespondWithSuccess((DefaultResourceHandler.RequestData) requestData);
                  uiResourceHandler.RemovePendingRequest((DefaultResourceHandler.RequestData) requestData);
                }
                else
                {
                  requestData.Error = string.Format("Thumbnail not found {0}", (object) requestData.UriBuilder.Uri);
                  uiResourceHandler.CheckForFailedRequest((DefaultResourceHandler.RequestData) requestData);
                }
              }
              catch (Exception ex)
              {
                requestData.Error = ex.ToString();
                uiResourceHandler.CheckForFailedRequest((DefaultResourceHandler.RequestData) requestData);
              }
              info = (ThumbnailCache.ThumbnailInfo) null;
              yield break;
            }
          }
          requestData.Error = string.Format("Thumbnail not found {0}", (object) requestData.UriBuilder.Uri);
          requestData.IsHandled = true;
          uiResourceHandler.CheckForFailedRequest((DefaultResourceHandler.RequestData) requestData);
        }
      }
      else
      {
        requestData.Error = "Thumbnails are not available at this time '" + requestData.UriBuilder.Host + "'";
        uiResourceHandler.CheckForFailedRequest((DefaultResourceHandler.RequestData) requestData);
      }
    }

    protected class GameResourceRequestData : DefaultResourceHandler.ResourceRequestData
    {
      public GameResourceRequestData(IResourceRequest request, IResourceResponse response)
        : base(request, response)
      {
      }

      public bool IsThumbnailRequest => this.UriBuilder.Scheme == "thumbnail";

      public bool IsScreenshotRequest => this.UriBuilder.Scheme == "screencapture";

      public bool IsUserAvatarRequest => this.UriBuilder.Scheme == "useravatar";
    }
  }
}
