// Decompiled with JetBrains decompiler
// Type: Game.UI.Thumbnails.ThumbnailCustomPass
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering.RendererUtils;

#nullable disable
namespace Game.UI.Thumbnails
{
  public class ThumbnailCustomPass : CustomPass
  {
    public LayerMask m_ThumbnailLayer = (LayerMask) 0;
    private ShaderTagId[] m_ShaderTags;
    private RTHandle m_ThumbnailBuffer;
    private RTHandle m_ThumbnailDepthBuffer;
    private bool m_CanRender;

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
      this.m_ShaderTags = new ShaderTagId[3]
      {
        new ShaderTagId("Forward"),
        new ShaderTagId("ForwardOnly"),
        new ShaderTagId("SRPDefaultUnlit")
      };
    }

    public void AllocateRTHandles(int width, int height)
    {
      if (this.m_ThumbnailBuffer != null && (Object) this.m_ThumbnailBuffer.rt != (Object) null && (this.m_ThumbnailBuffer.rt.width != width || this.m_ThumbnailBuffer.rt.height != height))
      {
        this.m_ThumbnailBuffer.Release();
        this.m_ThumbnailBuffer = (RTHandle) null;
        if (this.m_ThumbnailDepthBuffer != null)
        {
          this.m_ThumbnailDepthBuffer.Release();
          this.m_ThumbnailDepthBuffer = (RTHandle) null;
        }
      }
      QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel());
      if (this.m_ThumbnailBuffer == null)
        this.m_ThumbnailBuffer = RTHandles.Alloc(width, height, colorFormat: GraphicsFormat.R8G8B8A8_UNorm, useDynamicScale: true, name: "Thumbnail Color Buffer");
      if (this.m_ThumbnailDepthBuffer == null)
        this.m_ThumbnailDepthBuffer = RTHandles.Alloc(width, height, depthBufferBits: DepthBits.Depth16, colorFormat: GraphicsFormat.R16_UInt, dimension: TextureXR.dimension, name: "Thumbnail Depth Buffer");
      this.m_CanRender = true;
    }

    protected override void Execute(CustomPassContext ctx)
    {
      if (!this.m_CanRender)
        return;
      RendererListDesc desc = new RendererListDesc(this.m_ShaderTags, ctx.cullingResults, ctx.hdCamera.camera)
      {
        rendererConfiguration = PerObjectData.LightProbe | PerObjectData.LightProbeProxyVolume | PerObjectData.Lightmaps,
        renderQueueRange = RenderQueueRange.all,
        sortingCriteria = SortingCriteria.BackToFront,
        excludeObjectMotionVectors = true,
        layerMask = (int) this.m_ThumbnailLayer,
        stateBlock = new RenderStateBlock?(new RenderStateBlock(RenderStateMask.Depth | RenderStateMask.Stencil)
        {
          depthState = new DepthState(compareFunction: CompareFunction.LessEqual)
        })
      };
      int globalInt = Shader.GetGlobalInt("colossal_InfoviewOn");
      ctx.cmd.DisableShaderKeyword("INFOVIEW_ON");
      ctx.cmd.SetGlobalInt("colossal_InfoviewOn", 0);
      CoreUtils.SetRenderTarget(ctx.cmd, this.m_ThumbnailBuffer, this.m_ThumbnailDepthBuffer, ClearFlag.All);
      CoreUtils.DrawRendererList(ctx.renderContext, ctx.cmd, ctx.renderContext.CreateRendererList(desc));
      ctx.cmd.SetGlobalInt("colossal_InfoviewOn", globalInt);
    }

    public RenderTexture GetBuffer() => this.m_ThumbnailBuffer.rt;

    public void Release()
    {
      if (this.m_ThumbnailBuffer != null)
        this.m_ThumbnailBuffer.Release();
      if (this.m_ThumbnailDepthBuffer != null)
        this.m_ThumbnailDepthBuffer.Release();
      this.m_CanRender = false;
    }

    protected override void Cleanup() => this.Release();
  }
}
