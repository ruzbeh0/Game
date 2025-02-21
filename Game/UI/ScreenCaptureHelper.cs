// Decompiled with JetBrains decompiler
// Type: Game.UI.ScreenCaptureHelper
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.UI;
using Game.Rendering;
using Game.UI.Menu;
using System;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#nullable disable
namespace Game.UI
{
  public static class ScreenCaptureHelper
  {
    private static ILog log = LogManager.GetLogger("SceneFlow");

    public static RenderTexture CreateRenderTarget(
      string name,
      int width,
      int height,
      GraphicsFormat format = GraphicsFormat.R8G8B8A8_UNorm)
    {
      RenderTexture renderTarget = new RenderTexture(width, height, 0, format, 0);
      renderTarget.name = name;
      renderTarget.hideFlags = HideFlags.HideAndDontSave;
      renderTarget.Create();
      return renderTarget;
    }

    public static void CaptureScreenshot(
      Camera camera,
      RenderTexture destination,
      MenuHelpers.SaveGamePreviewSettings settings)
    {
      if ((UnityEngine.Object) destination == (UnityEngine.Object) null || (UnityEngine.Object) camera == (UnityEngine.Object) null)
        return;
      HDRPDotsInputs.punctualLightsJobHandle.Complete();
      RenderPipelineSettings.ColorBufferFormat colorBufferFormat = (QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel()) as HDRenderPipelineAsset).currentPlatformRenderPipelineSettings.colorBufferFormat;
      RenderTexture source = new RenderTexture(destination.width, destination.height, 16, (GraphicsFormat) colorBufferFormat, 0);
      HDAdditionalCameraData component = camera.GetComponent<HDAdditionalCameraData>();
      bool dynamicResolution = component.allowDynamicResolution;
      component.allowDynamicResolution = false;
      // ISSUE: variable of a compiler-generated type
      RenderingSystem existingSystemManaged = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<RenderingSystem>();
      existingSystemManaged.hideOverlay = true;
      UIManager.defaultUISystem.enabled = false;
      RenderTexture targetTexture = camera.targetTexture;
      camera.targetTexture = source;
      camera.Render();
      camera.targetTexture = targetTexture;
      existingSystemManaged.hideOverlay = false;
      UIManager.defaultUISystem.enabled = true;
      Material mat = new Material(Shader.Find("Hidden/ScreenCaptureCompose"));
      if (settings.stylized)
        mat.EnableKeyword("STYLIZE");
      else
        mat.DisableKeyword("STYLIZE");
      mat.SetFloat("_Radius", settings.stylizedRadius);
      TextureAsset overlayImage = settings.overlayImage;
      if ((AssetData) overlayImage != (IAssetData) null)
        mat.SetTexture("_Overlay", overlayImage.Load(0));
      Graphics.Blit((Texture) source, destination, mat, 0);
      if ((AssetData) overlayImage != (IAssetData) null)
        overlayImage.Unload(false);
      UnityEngine.Object.Destroy((UnityEngine.Object) mat);
      destination.IncrementUpdateCount();
      component.allowDynamicResolution = dynamicResolution;
      UnityEngine.Object.Destroy((UnityEngine.Object) source);
    }

    public class AsyncRequest
    {
      private readonly TaskCompletionSource<bool> m_TaskCompletionSource;
      private NativeArray<byte> m_Data;

      public int width { get; }

      public int height { get; }

      public GraphicsFormat format { get; }

      public AsyncRequest(Texture previewTexture)
      {
        this.width = previewTexture.width;
        this.height = previewTexture.height;
        this.format = previewTexture.graphicsFormat;
        int mipchainSize = TextureUtils.ComputeMipchainSize(previewTexture.width, previewTexture.height, previewTexture.graphicsFormat, 1);
        this.m_Data = new NativeArray<byte>(mipchainSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        ScreenCaptureHelper.log.DebugFormat("Issued request {0}x{1} size: {2}", (object) this.width, (object) this.height, (object) mipchainSize);
        this.m_TaskCompletionSource = new TaskCompletionSource<bool>();
        AsyncGPUReadback.RequestIntoNativeArray<byte>(ref this.m_Data, previewTexture, callback: new Action<AsyncGPUReadbackRequest>(this.OnCompleted));
      }

      private void OnCompleted(AsyncGPUReadbackRequest request)
      {
        if (this.width == request.width && this.height == request.height && this.m_Data.Length == request.layerDataSize)
        {
          if (!request.done)
          {
            ScreenCaptureHelper.log.ErrorFormat("Waiting for request {0}x{1} size: {2}. This should never happen!", (object) request.width, (object) request.height, (object) request.layerDataSize);
            request.WaitForCompletion();
          }
          if (request.done && request.hasError)
            ScreenCaptureHelper.log.ErrorFormat("Request failed {0}x{1} size: {2}. Result will be incorrect.", (object) request.width, (object) request.height, (object) request.layerDataSize);
          this.m_TaskCompletionSource.SetResult(true);
        }
        else
        {
          ScreenCaptureHelper.log.WarnFormat("Request failed {0}x{1} size: {2}. Completed successfully but not matching any request.", (object) request.width, (object) request.height, (object) request.layerDataSize);
          this.m_TaskCompletionSource.SetResult(false);
        }
      }

      public async Task Dispose()
      {
        ScreenCaptureHelper.log.DebugFormat("Manual release of request {0}x{1}. Probably due to an error.", (object) this.width, (object) this.height);
        await this.Complete();
        this.m_Data.Dispose();
      }

      public Task Complete() => (Task) this.m_TaskCompletionSource.Task;

      public ref NativeArray<byte> result => ref this.m_Data;
    }
  }
}
