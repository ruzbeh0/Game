// Decompiled with JetBrains decompiler
// Type: DepthFadePass
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#nullable disable
internal class DepthFadePass : CustomPass
{
  public float m_MotionAdaptation = 2f;
  public float m_DepthAdaptationThreshold = 0.7f;
  private RTHandle m_LinearDepthBuffer;
  private RTHandle[] m_HistoryBuffers;
  private int currentBuffer;
  private ComputeShader m_ComputeShader;
  private int m_LinearizePassKernel;
  private int m_HistoryPassKernel;

  protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
  {
    this.CreateResources();
    this.m_ComputeShader = Resources.Load<ComputeShader>(nameof (DepthFadePass));
    this.m_LinearizePassKernel = this.m_ComputeShader.FindKernel("LinearizePass");
    this.m_HistoryPassKernel = this.m_ComputeShader.FindKernel("HistoryPass");
  }

  protected override void Execute(CustomPassContext ctx)
  {
    if (!ctx.hdCamera.RequiresCameraJitter())
    {
      this.ReleaseResources();
      ctx.cmd.DisableShaderKeyword("DEPTH_FADE_FROM_TEXTURE");
    }
    else
    {
      this.CreateResources();
      int threadGroupsX = (this.m_LinearDepthBuffer.rt.width + 7) / 8;
      int threadGroupsY = (this.m_LinearDepthBuffer.rt.height + 7) / 8;
      ctx.cmd.SetComputeTextureParam(this.m_ComputeShader, this.m_LinearizePassKernel, DepthFadePass.ShaderID._LinearDepthTarget, (RenderTargetIdentifier) this.m_LinearDepthBuffer);
      ctx.cmd.DispatchCompute(this.m_ComputeShader, this.m_LinearizePassKernel, threadGroupsX, threadGroupsY, 1);
      RTHandle historyBuffer1 = this.m_HistoryBuffers[this.currentBuffer];
      this.currentBuffer = (this.currentBuffer + 1) % 2;
      RTHandle historyBuffer2 = this.m_HistoryBuffers[this.currentBuffer];
      ctx.cmd.SetComputeTextureParam(this.m_ComputeShader, this.m_HistoryPassKernel, DepthFadePass.ShaderID._LinearDepth, (RenderTargetIdentifier) this.m_LinearDepthBuffer);
      ctx.cmd.SetComputeTextureParam(this.m_ComputeShader, this.m_HistoryPassKernel, DepthFadePass.ShaderID._DepthHistory, (RenderTargetIdentifier) historyBuffer1);
      ctx.cmd.SetComputeTextureParam(this.m_ComputeShader, this.m_HistoryPassKernel, DepthFadePass.ShaderID._DepthHistoryTarget, (RenderTargetIdentifier) historyBuffer2);
      DepthFadePass.ShaderVariablesDepthFadePass data = new DepthFadePass.ShaderVariablesDepthFadePass()
      {
        _TextureSizes = new Vector4((float) historyBuffer1.rt.width, (float) historyBuffer1.rt.height, (float) this.m_LinearDepthBuffer.rt.width, (float) this.m_LinearDepthBuffer.rt.height),
        _MotionAdaptation = this.m_MotionAdaptation,
        _DepthAdaptationThreshold = this.m_DepthAdaptationThreshold
      };
      ConstantBuffer.Push<DepthFadePass.ShaderVariablesDepthFadePass>(ctx.cmd, in data, this.m_ComputeShader, DepthFadePass.ShaderID._ShaderVariables);
      ctx.cmd.DispatchCompute(this.m_ComputeShader, this.m_HistoryPassKernel, threadGroupsX, threadGroupsY, 1);
      ctx.cmd.SetGlobalTexture(DepthFadePass.ShaderID._GlobalDepthFadeTex, (RenderTargetIdentifier) historyBuffer2);
      ctx.cmd.EnableShaderKeyword("DEPTH_FADE_FROM_TEXTURE");
    }
  }

  protected override void Cleanup() => this.ReleaseResources();

  private void CreateResources()
  {
    if (this.m_LinearDepthBuffer == null)
      this.m_LinearDepthBuffer = RTHandles.Alloc(Vector2.one, TextureXR.slices, colorFormat: GraphicsFormat.R32_SFloat, dimension: TextureXR.dimension, enableRandomWrite: true, useDynamicScale: true, name: "DepthFade Intermediate Linear Depth");
    if (this.m_HistoryBuffers != null)
      return;
    this.m_HistoryBuffers = new RTHandle[2];
    for (int index = 0; index < this.m_HistoryBuffers.Length; ++index)
      this.m_HistoryBuffers[index] = RTHandles.Alloc(Vector2.one, TextureXR.slices, colorFormat: GraphicsFormat.R8G8B8A8_UNorm, dimension: TextureXR.dimension, enableRandomWrite: true, name: "DepthFade History " + index.ToString());
  }

  private void ReleaseResources()
  {
    if (this.m_LinearDepthBuffer != null)
    {
      this.m_LinearDepthBuffer.Release();
      this.m_LinearDepthBuffer = (RTHandle) null;
    }
    if (this.m_HistoryBuffers == null)
      return;
    for (int index = 0; index < this.m_HistoryBuffers.Length; ++index)
    {
      this.m_HistoryBuffers[index]?.Release();
      this.m_HistoryBuffers[index] = (RTHandle) null;
    }
    this.m_HistoryBuffers = (RTHandle[]) null;
  }

  private static class ShaderID
  {
    public static readonly int _LinearDepthTarget = Shader.PropertyToID(nameof (_LinearDepthTarget));
    public static readonly int _LinearDepth = Shader.PropertyToID(nameof (_LinearDepth));
    public static readonly int _DepthHistory = Shader.PropertyToID(nameof (_DepthHistory));
    public static readonly int _DepthHistoryTarget = Shader.PropertyToID(nameof (_DepthHistoryTarget));
    public static readonly int _ShaderVariables = Shader.PropertyToID("_ShaderVariablesDepthFadePass");
    public static readonly int _GlobalDepthFadeTex = Shader.PropertyToID("_DepthFadeTex");
  }

  private struct ShaderVariablesDepthFadePass
  {
    public Vector4 _TextureSizes;
    public float _MotionAdaptation;
    public float _DepthAdaptationThreshold;
  }
}
