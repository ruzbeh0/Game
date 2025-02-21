// Decompiled with JetBrains decompiler
// Type: Game.Rendering.UndergroundPass
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering.RendererUtils;

#nullable disable
namespace Game.Rendering
{
  public class UndergroundPass : CustomPass
  {
    private UndergroundViewSystem m_UndergroundViewSystem;
    private ShaderTagId[] m_ShaderTags;
    private ComputeShader m_ComputeShader;
    private Material m_ContourMaterial;
    private int m_TunnelMask;
    private int m_MarkerMask;
    private int m_PipelineMask;
    private int m_SubPipelineMask;
    private int m_CameraColorBuffer;
    private int m_UndergroundColorBuffer;
    private int m_UndergroundDepthBuffer;
    private int m_UndergroundFlags;
    private int m_UndergroundPassKernel;
    private int m_ContourPassKernel;

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
      foreach (World world in World.All)
      {
        if (world.IsCreated && (world.Flags & WorldFlags.Simulation) == WorldFlags.Simulation)
        {
          this.m_UndergroundViewSystem = world.GetExistingSystemManaged<UndergroundViewSystem>();
          if (this.m_UndergroundViewSystem != null)
            break;
        }
      }
      this.m_ShaderTags = new ShaderTagId[3]
      {
        HDShaderPassNames.s_ForwardName,
        HDShaderPassNames.s_ForwardOnlyName,
        HDShaderPassNames.s_SRPDefaultUnlitName
      };
      this.m_ComputeShader = Resources.Load<ComputeShader>(nameof (UndergroundPass));
      this.m_ContourMaterial = new Material(Resources.Load<Shader>("TerrainHeights"));
      this.m_TunnelMask = 1 << LayerMask.NameToLayer("Tunnel") | 1 << LayerMask.NameToLayer("Moving");
      this.m_MarkerMask = 1 << LayerMask.NameToLayer("Marker");
      this.m_PipelineMask = 1 << LayerMask.NameToLayer("Pipeline");
      this.m_SubPipelineMask = 1 << LayerMask.NameToLayer("SubPipeline");
      this.m_CameraColorBuffer = Shader.PropertyToID("_CameraColorBuffer");
      this.m_UndergroundColorBuffer = Shader.PropertyToID("_UndergroundColorBuffer");
      this.m_UndergroundDepthBuffer = Shader.PropertyToID("_UndergroundDepthBuffer");
      this.m_UndergroundFlags = Shader.PropertyToID("_UndergroundFlags");
      this.m_UndergroundPassKernel = this.m_ComputeShader.FindKernel(nameof (UndergroundPass));
      this.m_ContourPassKernel = this.m_ComputeShader.FindKernel("ContourPass");
    }

    protected override void AggregateCullingParameters(
      ref ScriptableCullingParameters cullingParameters,
      HDCamera hdCamera)
    {
      if (this.m_UndergroundViewSystem == null)
        return;
      if (this.m_UndergroundViewSystem.tunnelsOn)
      {
        cullingParameters.cullingMask |= (uint) this.m_TunnelMask;
        if (this.m_UndergroundViewSystem.markersOn)
          cullingParameters.cullingMask |= (uint) this.m_MarkerMask;
      }
      if (this.m_UndergroundViewSystem.pipelinesOn)
        cullingParameters.cullingMask |= (uint) this.m_PipelineMask;
      if (!this.m_UndergroundViewSystem.subPipelinesOn)
        return;
      cullingParameters.cullingMask |= (uint) this.m_SubPipelineMask;
    }

    protected override void Execute(CustomPassContext ctx)
    {
      if (this.m_UndergroundViewSystem == null || !this.m_UndergroundViewSystem.tunnelsOn && !this.m_UndergroundViewSystem.pipelinesOn && !this.m_UndergroundViewSystem.subPipelinesOn && !this.m_UndergroundViewSystem.contourLinesOn)
        return;
      UndergroundPass.ComputeFlags val = this.m_UndergroundViewSystem.undergroundOn ? UndergroundPass.ComputeFlags.FadeCameraColor | UndergroundPass.ComputeFlags.EmphasizeCustomColor : (UndergroundPass.ComputeFlags) 0;
      if (this.m_UndergroundViewSystem.contourLinesOn)
      {
        TerrainSurface validSurface = TerrainSurface.GetValidSurface();
        CBTSubdivisionTerrainEngine cbt = validSurface.cbt;
        if (cbt != null && cbt.IsValid && (Object) validSurface.material != (Object) null)
        {
          CoreUtils.SetRenderTarget(ctx.cmd, ctx.customColorBuffer.Value, ctx.customDepthBuffer.Value, ClearFlag.All);
          HDRenderPipeline.TerrainRenderingParameters parameters = HDRenderPipeline.PrepareTerrainRenderingParameters(ctx.hdCamera.camera.transform.position, validSurface);
          this.m_ContourMaterial.CopyPropertiesFromMaterial(validSurface.material);
          parameters.terrainMaterial = this.m_ContourMaterial;
          HDRenderPipeline.RenderTerrainSurfaceCBT(ctx.cmd, 0, validSurface, ctx.hdCamera.camera, parameters);
          Texture cameraColorBuffer = (Texture) ctx.cameraColorBuffer;
          ctx.cmd.SetComputeTextureParam(this.m_ComputeShader, this.m_ContourPassKernel, this.m_CameraColorBuffer, (RenderTargetIdentifier) cameraColorBuffer);
          ctx.cmd.SetComputeTextureParam(this.m_ComputeShader, this.m_ContourPassKernel, this.m_UndergroundColorBuffer, (RenderTargetIdentifier) ctx.customColorBuffer.Value);
          ctx.cmd.SetComputeTextureParam(this.m_ComputeShader, this.m_ContourPassKernel, this.m_UndergroundDepthBuffer, (RenderTargetIdentifier) ctx.customDepthBuffer.Value);
          ctx.cmd.DispatchCompute(this.m_ComputeShader, this.m_ContourPassKernel, cameraColorBuffer.width + 15 >> 4, cameraColorBuffer.height + 15 >> 4, 1);
        }
      }
      RendererListDesc rendererListDesc;
      ScriptableRenderContext renderContext1;
      if (this.m_UndergroundViewSystem.tunnelsOn)
      {
        RenderStateBlock renderStateBlock = new RenderStateBlock(RenderStateMask.Depth);
        renderStateBlock.depthState = DepthState.defaultValue;
        renderStateBlock.stencilState = StencilState.defaultValue;
        rendererListDesc = new RendererListDesc(this.m_ShaderTags, ctx.cullingResults, ctx.hdCamera.camera);
        rendererListDesc.rendererConfiguration = PerObjectData.LightProbe | PerObjectData.LightProbeProxyVolume | PerObjectData.Lightmaps;
        rendererListDesc.renderQueueRange = RenderQueueRange.all;
        rendererListDesc.sortingCriteria = SortingCriteria.CommonOpaque;
        rendererListDesc.excludeObjectMotionVectors = false;
        rendererListDesc.stateBlock = new RenderStateBlock?(renderStateBlock);
        rendererListDesc.layerMask = this.m_TunnelMask;
        RendererListDesc desc = rendererListDesc;
        if (this.m_UndergroundViewSystem.markersOn)
          desc.layerMask |= this.m_MarkerMask;
        if (this.m_UndergroundViewSystem.pipelinesOn && !this.m_UndergroundViewSystem.subPipelinesOn)
          desc.layerMask |= this.m_PipelineMask;
        ctx.cmd.EnableShaderKeyword("DECALS_OFF");
        ctx.cmd.DisableShaderKeyword("DECALS_4RT");
        CoreUtils.SetRenderTarget(ctx.cmd, ctx.customColorBuffer.Value, ctx.customDepthBuffer.Value, ClearFlag.All);
        ScriptableRenderContext renderContext2 = ctx.renderContext;
        CommandBuffer cmd = ctx.cmd;
        renderContext1 = ctx.renderContext;
        RendererList rendererList = renderContext1.CreateRendererList(desc);
        CoreUtils.DrawRendererList(renderContext2, cmd, rendererList);
        ctx.cmd.EnableShaderKeyword("DECALS_4RT");
        ctx.cmd.DisableShaderKeyword("DECALS_OFF");
        Texture cameraColorBuffer = (Texture) ctx.cameraColorBuffer;
        ctx.cmd.SetComputeTextureParam(this.m_ComputeShader, this.m_UndergroundPassKernel, this.m_CameraColorBuffer, (RenderTargetIdentifier) cameraColorBuffer);
        ctx.cmd.SetComputeTextureParam(this.m_ComputeShader, this.m_UndergroundPassKernel, this.m_UndergroundColorBuffer, (RenderTargetIdentifier) ctx.customColorBuffer.Value);
        ctx.cmd.SetComputeTextureParam(this.m_ComputeShader, this.m_UndergroundPassKernel, this.m_UndergroundDepthBuffer, (RenderTargetIdentifier) ctx.customDepthBuffer.Value);
        ctx.cmd.SetComputeIntParam(this.m_ComputeShader, this.m_UndergroundFlags, (int) (val | UndergroundPass.ComputeFlags.FadeNearSurface));
        ctx.cmd.DispatchCompute(this.m_ComputeShader, this.m_UndergroundPassKernel, cameraColorBuffer.width + 15 >> 4, cameraColorBuffer.height + 15 >> 4, 1);
        val &= ~UndergroundPass.ComputeFlags.FadeCameraColor;
      }
      if (!this.m_UndergroundViewSystem.subPipelinesOn && (!this.m_UndergroundViewSystem.pipelinesOn || this.m_UndergroundViewSystem.tunnelsOn))
        return;
      RenderStateBlock renderStateBlock1 = new RenderStateBlock(RenderStateMask.Depth);
      renderStateBlock1.depthState = DepthState.defaultValue;
      renderStateBlock1.stencilState = StencilState.defaultValue;
      rendererListDesc = new RendererListDesc(this.m_ShaderTags, ctx.cullingResults, ctx.hdCamera.camera);
      rendererListDesc.rendererConfiguration = PerObjectData.LightProbe | PerObjectData.LightProbeProxyVolume | PerObjectData.Lightmaps;
      rendererListDesc.renderQueueRange = RenderQueueRange.all;
      rendererListDesc.sortingCriteria = SortingCriteria.CommonOpaque;
      rendererListDesc.excludeObjectMotionVectors = false;
      rendererListDesc.stateBlock = new RenderStateBlock?(renderStateBlock1);
      rendererListDesc.layerMask = this.m_UndergroundViewSystem.pipelinesOn ? this.m_PipelineMask | this.m_SubPipelineMask : this.m_SubPipelineMask;
      RendererListDesc desc1 = rendererListDesc;
      if (this.m_UndergroundViewSystem.pipelinesOn)
        desc1.layerMask |= this.m_PipelineMask;
      if (this.m_UndergroundViewSystem.subPipelinesOn)
        desc1.layerMask |= this.m_SubPipelineMask;
      ctx.cmd.EnableShaderKeyword("DECALS_OFF");
      ctx.cmd.DisableShaderKeyword("DECALS_4RT");
      CoreUtils.SetRenderTarget(ctx.cmd, ctx.customColorBuffer.Value, ctx.customDepthBuffer.Value, ClearFlag.All);
      ScriptableRenderContext renderContext3 = ctx.renderContext;
      CommandBuffer cmd1 = ctx.cmd;
      renderContext1 = ctx.renderContext;
      RendererList rendererList1 = renderContext1.CreateRendererList(desc1);
      CoreUtils.DrawRendererList(renderContext3, cmd1, rendererList1);
      ctx.cmd.EnableShaderKeyword("DECALS_4RT");
      ctx.cmd.DisableShaderKeyword("DECALS_OFF");
      Texture cameraColorBuffer1 = (Texture) ctx.cameraColorBuffer;
      ctx.cmd.SetComputeTextureParam(this.m_ComputeShader, this.m_UndergroundPassKernel, this.m_CameraColorBuffer, (RenderTargetIdentifier) cameraColorBuffer1);
      ctx.cmd.SetComputeTextureParam(this.m_ComputeShader, this.m_UndergroundPassKernel, this.m_UndergroundColorBuffer, (RenderTargetIdentifier) ctx.customColorBuffer.Value);
      ctx.cmd.SetComputeTextureParam(this.m_ComputeShader, this.m_UndergroundPassKernel, this.m_UndergroundDepthBuffer, (RenderTargetIdentifier) ctx.customDepthBuffer.Value);
      ctx.cmd.SetComputeIntParam(this.m_ComputeShader, this.m_UndergroundFlags, (int) val);
      ctx.cmd.DispatchCompute(this.m_ComputeShader, this.m_UndergroundPassKernel, cameraColorBuffer1.width + 15 >> 4, cameraColorBuffer1.height + 15 >> 4, 1);
    }

    protected override void Cleanup()
    {
    }

    private enum ComputeFlags
    {
      FadeCameraColor = 1,
      FadeNearSurface = 2,
      EmphasizeCustomColor = 4,
    }
  }
}
