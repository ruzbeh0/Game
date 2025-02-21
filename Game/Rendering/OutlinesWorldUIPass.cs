// Decompiled with JetBrains decompiler
// Type: Game.Rendering.OutlinesWorldUIPass
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Settings;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering.RendererUtils;

#nullable disable
namespace Game.Rendering
{
  public class OutlinesWorldUIPass : CustomPass
  {
    public LayerMask m_OutlineLayer = (LayerMask) 0;
    public Material m_FullscreenOutline;
    public float m_MaxDistance = 16000f;
    private MaterialPropertyBlock m_OutlineProperties;
    private ShaderTagId[] m_ShaderTags;
    private RTHandle m_OutlineBuffer;
    private CustomSampler m_OutlinesSampler;

    private void CheckResource()
    {
      AntiAliasingQualitySettings qualitySetting = SharedSettings.instance?.graphics?.GetQualitySetting<AntiAliasingQualitySettings>();
      MSAASamples msaaSamples = qualitySetting != null ? qualitySetting.outlinesMSAA : MSAASamples.None;
      if (msaaSamples < MSAASamples.None)
        msaaSamples = MSAASamples.None;
      if (msaaSamples > MSAASamples.MSAA8x)
        msaaSamples = MSAASamples.MSAA8x;
      if (this.m_OutlineBuffer != null && !((Object) this.m_OutlineBuffer.rt == (Object) null) && (MSAASamples) this.m_OutlineBuffer.rt.antiAliasing == msaaSamples)
        return;
      this.ReleaseResources();
      this.CreateResources(msaaSamples);
    }

    private void CreateResources(MSAASamples msaaSamples)
    {
      this.m_OutlineBuffer = RTHandles.Alloc(Vector2.one, TextureXR.slices, dimension: TextureXR.dimension, msaaSamples: msaaSamples, name: "Outline Buffer");
    }

    private void ReleaseResources()
    {
      if (this.m_OutlineBuffer == null)
        return;
      this.m_OutlineBuffer.Release();
    }

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
      this.m_OutlinesSampler = CustomSampler.Create("Outlines pass");
      this.m_OutlineProperties = new MaterialPropertyBlock();
      this.m_ShaderTags = new ShaderTagId[3]
      {
        new ShaderTagId("Forward"),
        new ShaderTagId("ForwardOnly"),
        new ShaderTagId("SRPDefaultUnlit")
      };
    }

    protected override void AggregateCullingParameters(
      ref ScriptableCullingParameters cullingParameters,
      HDCamera hdCamera)
    {
      cullingParameters.cullingMask |= (uint) (int) this.m_OutlineLayer;
    }

    private static RendererListDesc CreateOpaqueRendererListDesc(
      CullingResults cull,
      Camera camera,
      ShaderTagId passName,
      PerObjectData rendererConfiguration = PerObjectData.None,
      RenderQueueRange? renderQueueRange = null,
      RenderStateBlock? stateBlock = null,
      Material overrideMaterial = null,
      bool excludeObjectMotionVectors = false)
    {
      return new RendererListDesc(passName, cull, camera)
      {
        rendererConfiguration = rendererConfiguration,
        renderQueueRange = renderQueueRange.HasValue ? renderQueueRange.Value : HDRenderQueue.k_RenderQueue_AllOpaque,
        sortingCriteria = SortingCriteria.CommonOpaque,
        stateBlock = stateBlock,
        overrideMaterial = overrideMaterial,
        excludeObjectMotionVectors = excludeObjectMotionVectors
      };
    }

    private static RendererListDesc CreateTransparentRendererListDesc(
      CullingResults cull,
      Camera camera,
      ShaderTagId passName,
      PerObjectData rendererConfiguration = PerObjectData.None,
      RenderQueueRange? renderQueueRange = null,
      RenderStateBlock? stateBlock = null,
      Material overrideMaterial = null,
      bool excludeObjectMotionVectors = false)
    {
      return new RendererListDesc(passName, cull, camera)
      {
        rendererConfiguration = rendererConfiguration,
        renderQueueRange = renderQueueRange.HasValue ? renderQueueRange.Value : HDRenderQueue.k_RenderQueue_AllTransparent,
        sortingCriteria = SortingCriteria.CommonTransparent | SortingCriteria.RendererPriority,
        stateBlock = stateBlock,
        overrideMaterial = overrideMaterial,
        excludeObjectMotionVectors = excludeObjectMotionVectors
      };
    }

    private void DrawOutlineMeshes(CustomPassContext ctx)
    {
      RendererListDesc desc = new RendererListDesc(this.m_ShaderTags, ctx.cullingResults, ctx.hdCamera.camera)
      {
        rendererConfiguration = PerObjectData.LightProbe | PerObjectData.LightProbeProxyVolume | PerObjectData.Lightmaps,
        renderQueueRange = RenderQueueRange.all,
        sortingCriteria = SortingCriteria.BackToFront,
        excludeObjectMotionVectors = false,
        layerMask = (int) this.m_OutlineLayer
      };
      ctx.cmd.EnableShaderKeyword("SHADERPASS_OUTLINES");
      ctx.cmd.SetGlobalFloat(OutlinesWorldUIPass.ShaderID._Outlines_MaxDistance, this.m_MaxDistance);
      CoreUtils.SetRenderTarget(ctx.cmd, this.m_OutlineBuffer, ClearFlag.Color);
      CoreUtils.DrawRendererList(ctx.renderContext, ctx.cmd, ctx.renderContext.CreateRendererList(desc));
      ctx.cmd.DisableShaderKeyword("SHADERPASS_OUTLINES");
    }

    private void DrawAfterDRSObjects(CustomPassContext ctx)
    {
      float currentScale = DynamicResolutionHandler.instance.GetCurrentScale();
      ctx.cmd.SetGlobalFloat(OutlinesWorldUIPass.ShaderID._DRSScale, currentScale);
      ctx.cmd.SetGlobalFloat(OutlinesWorldUIPass.ShaderID._DRSScaleSquared, currentScale * currentScale);
      CoreUtils.DrawRendererList(ctx.renderContext, ctx.cmd, ctx.renderContext.CreateRendererList(OutlinesWorldUIPass.CreateOpaqueRendererListDesc(ctx.cameraCullingResults, ctx.hdCamera.camera, HDShaderPassNames.s_ForwardOnlyName, renderQueueRange: new RenderQueueRange?(HDRenderQueue.k_RenderQueue_AfterDRSOpaque))));
      CoreUtils.DrawRendererList(ctx.renderContext, ctx.cmd, ctx.renderContext.CreateRendererList(OutlinesWorldUIPass.CreateTransparentRendererListDesc(ctx.cameraCullingResults, ctx.hdCamera.camera, HDShaderPassNames.s_ForwardOnlyName, renderQueueRange: new RenderQueueRange?(HDRenderQueue.k_RenderQueue_AfterDRSTransparent))));
    }

    protected override void Execute(CustomPassContext ctx)
    {
      this.CheckResource();
      using (new ProfilingScope(ctx.cmd, new ProfilingSampler("Outlines and World UI Pass")))
      {
        this.DrawOutlineMeshes(ctx);
        CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer);
        this.DrawAfterDRSObjects(ctx);
        this.m_OutlineProperties.SetTexture(OutlinesWorldUIPass.ShaderID._OutlineBuffer, (Texture) this.m_OutlineBuffer);
        CoreUtils.DrawFullScreen(ctx.cmd, this.m_FullscreenOutline, this.m_OutlineProperties);
      }
    }

    protected override void Cleanup() => this.ReleaseResources();

    private static class ShaderID
    {
      public static readonly int _OutlineBuffer = Shader.PropertyToID(nameof (_OutlineBuffer));
      public static readonly int _Outlines_MaxDistance = Shader.PropertyToID(nameof (_Outlines_MaxDistance));
      public static readonly int _DRSScale = Shader.PropertyToID(nameof (_DRSScale));
      public static readonly int _DRSScaleSquared = Shader.PropertyToID(nameof (_DRSScaleSquared));
    }
  }
}
