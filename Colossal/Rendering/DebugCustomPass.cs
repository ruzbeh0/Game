// Decompiled with JetBrains decompiler
// Type: Colossal.Rendering.DebugCustomPass
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Rendering;
using Game.SceneFlow;
using Game.Settings;
using Game.Simulation;
using Game.UI.Debug;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#nullable disable
namespace Colossal.Rendering
{
  public class DebugCustomPass : CustomPass
  {
    private const int kPadding = 10;
    public const DebugCustomPass.TextureDebugMode kGlobalMapStart = DebugCustomPass.TextureDebugMode.HeightMap;
    public const DebugCustomPass.TextureDebugMode kGlobalMapEnd = DebugCustomPass.TextureDebugMode.Wind;
    public const DebugCustomPass.TextureDebugMode kWaterSimulationMapStart = DebugCustomPass.TextureDebugMode.WaterSurfaceSpectrum;
    public const DebugCustomPass.TextureDebugMode kWaterSimulationMapEnd = DebugCustomPass.TextureDebugMode.WaterSurfaceCaustics;
    private Material m_DebugBlitMaterial;
    private MaterialPropertyBlock m_MaterialPropertyBlock;
    private ComputeBuffer m_TopViewRenderIndirectArgs;
    private Material m_TopViewMaterial;
    private RTHandle m_TopViewRenderTexture;

    public int activeInstance { get; set; }

    public int sliceIndex { get; set; }

    public float debugOverlayRatio { get; set; } = 0.333333343f;

    public DebugCustomPass.TextureDebugMode textureDebugMode { get; set; }

    public float zoom { get; set; }

    public bool showExtra { get; set; }

    public float minValue { get; set; }

    public float maxValue { get; set; }

    public float GetDefaultMinValue()
    {
      switch (this.textureDebugMode)
      {
        case DebugCustomPass.TextureDebugMode.WaterVelocity:
          return 0.03f;
        case DebugCustomPass.TextureDebugMode.WaterRawVelocity:
          return 0.5f;
        default:
          return this.GetMinValue();
      }
    }

    public float GetDefaultMaxValue()
    {
      return this.textureDebugMode == DebugCustomPass.TextureDebugMode.WaterVelocity ? 0.5f : this.GetMaxValue();
    }

    public float GetMinValue()
    {
      int textureDebugMode = (int) this.textureDebugMode;
      return 0.0f;
    }

    public float GetMaxValue()
    {
      return this.textureDebugMode == DebugCustomPass.TextureDebugMode.WaterDepth ? 4096f : 1f;
    }

    public bool HasExtra()
    {
      return this.textureDebugMode == DebugCustomPass.TextureDebugMode.WaterVelocity;
    }

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
      this.m_DebugBlitMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Hidden/BH/CustomPass/DebugBlitQuad"));
      this.m_MaterialPropertyBlock = new MaterialPropertyBlock();
      this.m_TopViewRenderIndirectArgs = new ComputeBuffer(8, 4, ComputeBufferType.DrawIndirect);
      this.m_TopViewMaterial = CoreUtils.CreateEngineMaterial(HDRenderPipelineGlobalSettings.instance.renderPipelineResources.shaders.terrainCBTTopViewDebug);
    }

    protected override void Cleanup()
    {
      CoreUtils.Destroy((Object) this.m_DebugBlitMaterial);
      RTHandles.Release(this.m_TopViewRenderTexture);
      this.m_TopViewRenderIndirectArgs.Dispose();
      CoreUtils.Destroy((Object) this.m_TopViewMaterial);
    }

    private static T GetSystem<T>() where T : ComponentSystemBase
    {
      return World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<T>();
    }

    private static float RemToPxScale(HDCamera hdCamera)
    {
      InterfaceSettings userInterface = GameManager.instance?.settings?.userInterface;
      if (userInterface == null || !userInterface.interfaceScaling)
        return 1f;
      return (double) hdCamera.finalViewport.height > 9.0 / 16.0 * (double) hdCamera.finalViewport.width ? hdCamera.finalViewport.width / 1920f : hdCamera.finalViewport.height / 1080f;
    }

    private static int GetRuntimeDebugPanelWidth(HDCamera hdCamera)
    {
      int b = (int) ((DebugCustomPass.GetSystem<DebugUISystem>().visible ? 610.0 : 10.0) * (double) DebugCustomPass.RemToPxScale(hdCamera));
      return Mathf.Min(hdCamera.actualWidth, b);
    }

    private static int GetRuntimePadding(HDCamera hdCamera)
    {
      return (int) (10.0 * (double) DebugCustomPass.RemToPxScale(hdCamera));
    }

    public bool SetupTexture(out Texture tex, out int sliceCount)
    {
      Vector4 vector4 = new Vector4(this.minValue, (float) (1.0 / ((double) this.maxValue - (double) this.minValue)));
      sliceCount = 0;
      tex = (Texture) null;
      switch (this.textureDebugMode)
      {
        case DebugCustomPass.TextureDebugMode.HeightMap:
          tex = DebugCustomPass.GetSystem<TerrainSystem>().heightmap;
          this.m_MaterialPropertyBlock.SetInt("_Slice", 0);
          this.m_MaterialPropertyBlock.SetVector("_ValidRange", vector4);
          break;
        case DebugCustomPass.TextureDebugMode.HeightMapCascades:
          // ISSUE: reference to a compiler-generated method
          tex = DebugCustomPass.GetSystem<TerrainSystem>().GetCascadeTexture();
          this.m_MaterialPropertyBlock.SetInt("_Slice", this.sliceIndex);
          this.m_MaterialPropertyBlock.SetVector("_ValidRange", vector4);
          break;
        case DebugCustomPass.TextureDebugMode.SplatMap:
          tex = DebugCustomPass.GetSystem<TerrainMaterialSystem>().splatmap;
          this.m_MaterialPropertyBlock.SetInt("_Slice", 0);
          this.m_MaterialPropertyBlock.SetVector("_ValidRange", vector4);
          break;
        case DebugCustomPass.TextureDebugMode.WaterDepth:
        case DebugCustomPass.TextureDebugMode.WaterPolution:
        case DebugCustomPass.TextureDebugMode.WaterRawVelocity:
          tex = DebugCustomPass.GetSystem<WaterRenderSystem>().waterTexture;
          this.m_MaterialPropertyBlock.SetInt("_Slice", 0);
          this.m_MaterialPropertyBlock.SetVector("_ValidRange", vector4);
          break;
        case DebugCustomPass.TextureDebugMode.WaterVelocity:
          tex = DebugCustomPass.GetSystem<WaterRenderSystem>().waterTexture;
          this.m_MaterialPropertyBlock.SetInt("_Slice", 0);
          this.m_MaterialPropertyBlock.SetVector("_ValidRange", new Vector4(this.minValue, this.maxValue));
          break;
        case DebugCustomPass.TextureDebugMode.SnowAccumulation:
        case DebugCustomPass.TextureDebugMode.RainAccumulation:
          tex = (Texture) DebugCustomPass.GetSystem<SnowSystem>().SnowDepth;
          this.m_MaterialPropertyBlock.SetInt("_Slice", 0);
          this.m_MaterialPropertyBlock.SetVector("_ValidRange", vector4);
          break;
        case DebugCustomPass.TextureDebugMode.Wind:
          tex = (Texture) DebugCustomPass.GetSystem<WindTextureSystem>().WindTexture;
          this.m_MaterialPropertyBlock.SetInt("_Slice", 0);
          this.m_MaterialPropertyBlock.SetVector("_ValidRange", vector4);
          break;
        case DebugCustomPass.TextureDebugMode.TerrainTesselation:
          tex = this.GetDebugTesselationTexture();
          this.m_MaterialPropertyBlock.SetInt("_Slice", 0);
          this.m_MaterialPropertyBlock.SetVector("_ValidRange", vector4);
          break;
        case DebugCustomPass.TextureDebugMode.WaterSurfaceSpectrum:
          if (WaterSurface.instanceCount > 0)
          {
            tex = (Texture) WaterSurface.instancesAsArray[this.activeInstance].simulation.gpuBuffers.phillipsSpectrumBuffer;
            this.m_MaterialPropertyBlock.SetInt("_Slice", this.sliceIndex);
            this.m_MaterialPropertyBlock.SetVector("_ValidRange", new Vector4(this.minValue, this.maxValue));
            break;
          }
          break;
        case DebugCustomPass.TextureDebugMode.WaterSurfaceDisplacement:
          if (WaterSurface.instanceCount > 0)
          {
            tex = (Texture) WaterSurface.instancesAsArray[this.activeInstance].simulation.gpuBuffers.displacementBuffer;
            this.m_MaterialPropertyBlock.SetInt("_Slice", this.sliceIndex);
            this.m_MaterialPropertyBlock.SetVector("_ValidRange", new Vector4(this.minValue, this.maxValue));
            break;
          }
          break;
        case DebugCustomPass.TextureDebugMode.WaterSurfaceGradient:
        case DebugCustomPass.TextureDebugMode.WaterSurfaceJacobianSurface:
        case DebugCustomPass.TextureDebugMode.WaterSurfaceJacobianDeep:
          if (WaterSurface.instanceCount > 0)
          {
            tex = (Texture) WaterSurface.instancesAsArray[this.activeInstance].simulation.gpuBuffers.additionalDataBuffer;
            this.m_MaterialPropertyBlock.SetInt("_Slice", this.sliceIndex);
            this.m_MaterialPropertyBlock.SetVector("_ValidRange", new Vector4(this.minValue, this.maxValue));
            break;
          }
          break;
        case DebugCustomPass.TextureDebugMode.WaterSurfaceCaustics:
          if (WaterSurface.instanceCount > 0)
          {
            tex = (Texture) WaterSurface.instancesAsArray[this.activeInstance].simulation.gpuBuffers.causticsBuffer;
            this.m_MaterialPropertyBlock.SetInt("_Slice", this.sliceIndex);
            this.m_MaterialPropertyBlock.SetVector("_ValidRange", new Vector4(this.minValue, this.maxValue));
            break;
          }
          break;
        default:
          tex = (Texture) null;
          break;
      }
      if (!((Object) tex != (Object) null))
        return false;
      if (tex.dimension == TextureDimension.Tex2DArray || tex.dimension == TextureDimension.Tex3D)
      {
        if (tex is Texture2DArray texture2Darray)
          sliceCount = texture2Darray.depth - 1;
        if (tex is Texture3D texture3D)
          sliceCount = texture3D.depth - 1;
        if (tex is RenderTexture renderTexture)
          sliceCount = renderTexture.volumeDepth - 1;
      }
      return true;
    }

    private Texture GetDebugTesselationTexture()
    {
      TerrainSurface validSurface = TerrainSurface.GetValidSurface();
      if ((Object) validSurface != (Object) null)
      {
        ComputeBuffer cameraCbtBuffer = validSurface.GetCameraCbtBuffer(Camera.main);
        if (cameraCbtBuffer != null)
        {
          ComputeShader viewDispatchDebug = HDRenderPipelineGlobalSettings.instance.renderPipelineResources.shaders.terrainCBTTopViewDispatchDebug;
          viewDispatchDebug.SetBuffer(0, "u_CbtBuffer", cameraCbtBuffer);
          viewDispatchDebug.SetBuffer(0, "u_DrawCommand", this.m_TopViewRenderIndirectArgs);
          viewDispatchDebug.Dispatch(0, 1, 1, 1);
          Graphics.SetRenderTarget((RenderTexture) this.m_TopViewRenderTexture);
          GL.Clear(true, true, new Color(0.8f, 0.8f, 0.8f, 1f));
          this.m_TopViewMaterial.SetBuffer("u_CbtBuffer", cameraCbtBuffer);
          this.m_TopViewMaterial.SetPass(0);
          int num = GL.wireframe ? 1 : 0;
          GL.wireframe = true;
          Graphics.DrawProceduralIndirectNow(MeshTopology.Triangles, this.m_TopViewRenderIndirectArgs);
          GL.wireframe = num != 0;
          Graphics.SetRenderTarget((RenderTexture) null);
          return (Texture) this.m_TopViewRenderTexture;
        }
      }
      return (Texture) null;
    }

    private void CheckResources(int size)
    {
      if (this.m_TopViewRenderTexture != null && this.m_TopViewRenderTexture.rt.width == size && this.m_TopViewRenderTexture.rt.height == size)
        return;
      this.m_TopViewRenderTexture?.Release();
      this.m_TopViewRenderTexture = RTHandles.Alloc(size, size, msaaSamples: MSAASamples.MSAA8x, name: "CBTTopDownView");
    }

    protected override void Execute(CustomPassContext ctx)
    {
      if (ctx.hdCamera.camera.cameraType != CameraType.Game)
        return;
      float debugOverlayRatio = this.debugOverlayRatio;
      int width = (int) ctx.hdCamera.finalViewport.width;
      int height = (int) ctx.hdCamera.finalViewport.height;
      int b = height;
      int num = (int) ((double) Mathf.Min(width, b) * (double) debugOverlayRatio);
      Rect viewportSize = new Rect((float) DebugCustomPass.GetRuntimeDebugPanelWidth(ctx.hdCamera), (float) (height - num - DebugCustomPass.GetRuntimePadding(ctx.hdCamera)), (float) num, (float) num);
      this.CheckResources(num);
      this.m_MaterialPropertyBlock.Clear();
      this.m_MaterialPropertyBlock.SetFloat("_Zoom", this.zoom);
      this.m_MaterialPropertyBlock.SetInt("_ShowExtra", this.showExtra ? 1 : 0);
      bool applyExposure = false;
      Texture tex;
      if (!this.SetupTexture(out tex, out int _))
        return;
      DebugCustomPass.DisplayTexture(ctx.cmd, viewportSize, tex, this.m_DebugBlitMaterial, (int) this.textureDebugMode, this.m_MaterialPropertyBlock, applyExposure);
    }

    private static void DisplayTexture(
      CommandBuffer cmd,
      Rect viewportSize,
      Texture texture,
      Material debugMaterial,
      int mode,
      MaterialPropertyBlock mpb,
      bool applyExposure)
    {
      mpb.SetFloat(HDShaderIDs._ApplyExposure, applyExposure ? 1f : 0.0f);
      mpb.SetFloat(HDShaderIDs._Mipmap, 0.0f);
      mpb.SetTexture(HDShaderIDs._InputTexture, texture);
      mpb.SetInt("_Mode", mode);
      if (texture.dimension == TextureDimension.Tex2DArray)
        cmd.EnableShaderKeyword("TEXTURE_SOURCE_ARRAY");
      else
        cmd.DisableShaderKeyword("TEXTURE_SOURCE_ARRAY");
      cmd.SetViewport(viewportSize);
      cmd.DrawProcedural(Matrix4x4.identity, debugMaterial, 0, MeshTopology.Triangles, 3, 1, mpb);
    }

    public enum TextureDebugMode
    {
      None,
      HeightMap,
      HeightMapCascades,
      SplatMap,
      WaterDepth,
      WaterVelocity,
      WaterPolution,
      SnowAccumulation,
      RainAccumulation,
      WaterRawVelocity,
      Wind,
      TerrainTesselation,
      WaterSurfaceSpectrum,
      WaterSurfaceDisplacement,
      WaterSurfaceGradient,
      WaterSurfaceJacobianSurface,
      WaterSurfaceJacobianDeep,
      WaterSurfaceCaustics,
    }
  }
}
