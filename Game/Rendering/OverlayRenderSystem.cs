// Decompiled with JetBrains decompiler
// Type: Game.Rendering.OverlayRenderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;
using UnityEngine.TextCore.LowLevel;

#nullable disable
namespace Game.Rendering
{
  public class OverlayRenderSystem : GameSystemBase
  {
    private RenderingSystem m_RenderingSystem;
    private TerrainSystem m_TerrainSystem;
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_SettingsQuery;
    private Mesh m_BoxMesh;
    private Mesh m_QuadMesh;
    private Material m_ProjectedMaterial;
    private Material m_AbsoluteMaterial;
    private ComputeBuffer m_ArgsBuffer;
    private ComputeBuffer m_ProjectedBuffer;
    private ComputeBuffer m_AbsoluteBuffer;
    private List<uint> m_ArgsArray;
    private int m_ProjectedInstanceCount;
    private int m_AbsoluteInstanceCount;
    private int m_CurveBufferID;
    private int m_GradientScaleID;
    private int m_ScaleRatioAID;
    private int m_FaceDilateID;
    private NativeList<OverlayRenderSystem.CurveData> m_ProjectedData;
    private NativeList<OverlayRenderSystem.CurveData> m_AbsoluteData;
    private NativeValue<OverlayRenderSystem.BoundsData> m_BoundsData;
    private JobHandle m_BufferWriters;
    private TextMeshPro m_TextMesh;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      this.m_SettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<OverlayConfigurationData>());
      this.m_CurveBufferID = Shader.PropertyToID("colossal_OverlayCurveBuffer");
      this.m_GradientScaleID = Shader.PropertyToID("_GradientScale");
      this.m_ScaleRatioAID = Shader.PropertyToID("_ScaleRatioA");
      this.m_FaceDilateID = Shader.PropertyToID("_FaceDilate");
      RenderPipelineManager.beginContextRendering += new Action<ScriptableRenderContext, List<Camera>>(this.Render);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      RenderPipelineManager.beginContextRendering -= new Action<ScriptableRenderContext, List<Camera>>(this.Render);
      if ((UnityEngine.Object) this.m_BoxMesh != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_BoxMesh);
      if ((UnityEngine.Object) this.m_QuadMesh != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_QuadMesh);
      if ((UnityEngine.Object) this.m_ProjectedMaterial != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_ProjectedMaterial);
      if ((UnityEngine.Object) this.m_AbsoluteMaterial != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_AbsoluteMaterial);
      if (this.m_ArgsBuffer != null)
        this.m_ArgsBuffer.Release();
      if (this.m_ProjectedBuffer != null)
        this.m_ProjectedBuffer.Release();
      if (this.m_AbsoluteBuffer != null)
        this.m_AbsoluteBuffer.Release();
      if (this.m_ProjectedData.IsCreated)
        this.m_ProjectedData.Dispose();
      if (this.m_AbsoluteData.IsCreated)
        this.m_AbsoluteData.Dispose();
      if (this.m_BoundsData.IsCreated)
        this.m_BoundsData.Dispose();
      if ((UnityEngine.Object) this.m_TextMesh != (UnityEngine.Object) null)
      {
        for (int index = 0; index < this.m_TextMesh.font.fallbackFontAssetTable.Count; ++index)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.m_TextMesh.font.fallbackFontAssetTable[index]);
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_TextMesh.font);
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_TextMesh.gameObject);
      }
      base.OnDestroy();
    }

    public OverlayRenderSystem.Buffer GetBuffer(out JobHandle dependencies)
    {
      if (!this.m_ProjectedData.IsCreated)
        this.m_ProjectedData = new NativeList<OverlayRenderSystem.CurveData>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      if (!this.m_AbsoluteData.IsCreated)
        this.m_AbsoluteData = new NativeList<OverlayRenderSystem.CurveData>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      if (!this.m_BoundsData.IsCreated)
        this.m_BoundsData = new NativeValue<OverlayRenderSystem.BoundsData>(Allocator.Persistent);
      dependencies = this.m_BufferWriters;
      return new OverlayRenderSystem.Buffer(this.m_ProjectedData, this.m_AbsoluteData, this.m_BoundsData, this.m_TerrainSystem.heightScaleOffset.y - 50f, this.m_TerrainSystem.heightScaleOffset.x + 100f);
    }

    public void AddBufferWriter(JobHandle handle)
    {
      this.m_BufferWriters = JobHandle.CombineDependencies(this.m_BufferWriters, handle);
    }

    public TextMeshPro GetTextMesh()
    {
      if ((UnityEngine.Object) this.m_TextMesh == (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        OverlayConfigurationPrefab singletonPrefab = this.m_PrefabSystem.GetSingletonPrefab<OverlayConfigurationPrefab>(this.m_SettingsQuery);
        GameObject target = new GameObject("TextMeshPro");
        UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) target);
        this.m_TextMesh = target.AddComponent<TextMeshPro>();
        this.m_TextMesh.font = this.CreateFont(singletonPrefab.m_FontInfos[0]);
        this.m_TextMesh.font.fallbackFontAssetTable = new List<TMP_FontAsset>(singletonPrefab.m_FontInfos.Length - 1);
        for (int index = 1; index < singletonPrefab.m_FontInfos.Length; ++index)
          this.m_TextMesh.font.fallbackFontAssetTable.Add(this.CreateFont(singletonPrefab.m_FontInfos[index]));
        this.m_TextMesh.renderer.enabled = false;
      }
      return this.m_TextMesh;
    }

    private TMP_FontAsset CreateFont(FontInfo info)
    {
      TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(info.m_Font, info.m_SamplingPointSize, info.m_AtlasPadding, GlyphRenderMode.SDFAA_HINTED, info.m_AtlasWidth, info.m_AtlasHeight);
      fontAsset.material.SetFloat(this.m_FaceDilateID, 1f);
      return fontAsset;
    }

    public void CopyFontAtlasParameters(Material source, Material target)
    {
      target.SetFloat(this.m_GradientScaleID, source.GetFloat(this.m_GradientScaleID) * 2f);
      target.SetFloat(this.m_ScaleRatioAID, source.GetFloat(this.m_ScaleRatioAID));
      target.mainTexture = source.mainTexture;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      this.m_BufferWriters.Complete();
      this.m_BufferWriters = new JobHandle();
      this.m_ProjectedInstanceCount = 0;
      this.m_AbsoluteInstanceCount = 0;
      if ((!this.m_ProjectedData.IsCreated || this.m_ProjectedData.Length == 0) && (!this.m_AbsoluteData.IsCreated || this.m_AbsoluteData.Length == 0))
        return;
      if (this.m_SettingsQuery.IsEmptyIgnoreFilter)
      {
        if (this.m_ProjectedData.IsCreated)
          this.m_ProjectedData.Clear();
        if (!this.m_AbsoluteData.IsCreated)
          return;
        this.m_AbsoluteData.Clear();
      }
      else
      {
        if (this.m_ProjectedData.IsCreated && this.m_ProjectedData.Length != 0)
        {
          this.m_ProjectedInstanceCount = this.m_ProjectedData.Length;
          this.GetCurveMaterial(ref this.m_ProjectedMaterial, true);
          this.GetCurveBuffer(ref this.m_ProjectedBuffer, this.m_ProjectedInstanceCount);
          this.m_ProjectedBuffer.SetData<OverlayRenderSystem.CurveData>(this.m_ProjectedData.AsArray(), 0, 0, this.m_ProjectedInstanceCount);
          this.m_ProjectedMaterial.SetBuffer(this.m_CurveBufferID, this.m_ProjectedBuffer);
          this.m_ProjectedData.Clear();
        }
        if (!this.m_AbsoluteData.IsCreated || this.m_AbsoluteData.Length == 0)
          return;
        this.m_AbsoluteInstanceCount = this.m_AbsoluteData.Length;
        this.GetCurveMaterial(ref this.m_AbsoluteMaterial, false);
        this.GetCurveBuffer(ref this.m_AbsoluteBuffer, this.m_AbsoluteInstanceCount);
        this.m_AbsoluteBuffer.SetData<OverlayRenderSystem.CurveData>(this.m_AbsoluteData.AsArray(), 0, 0, this.m_AbsoluteInstanceCount);
        this.m_AbsoluteMaterial.SetBuffer(this.m_CurveBufferID, this.m_AbsoluteBuffer);
        this.m_AbsoluteData.Clear();
      }
    }

    private void Render(ScriptableRenderContext context, List<Camera> cameras)
    {
      try
      {
        if (this.m_RenderingSystem.hideOverlay)
          return;
        int count = 0;
        if (this.m_ProjectedInstanceCount != 0)
          count += 5;
        if (this.m_AbsoluteInstanceCount != 0)
          count += 5;
        if (count == 0)
          return;
        if (this.m_ArgsBuffer != null && this.m_ArgsBuffer.count < count)
        {
          this.m_ArgsBuffer.Release();
          this.m_ArgsBuffer = (ComputeBuffer) null;
        }
        if (this.m_ArgsBuffer == null)
        {
          this.m_ArgsBuffer = new ComputeBuffer(count, 4, ComputeBufferType.DrawIndirect);
          this.m_ArgsBuffer.name = "Overlay args buffer";
        }
        if (this.m_ArgsArray == null)
          this.m_ArgsArray = new List<uint>();
        this.m_ArgsArray.Clear();
        Bounds bounds = RenderingUtils.ToBounds(this.m_BoundsData.value.m_CurveBounds);
        int num1 = 0;
        int num2 = 0;
        if (this.m_ProjectedInstanceCount != 0)
        {
          this.GetMesh(ref this.m_BoxMesh, true);
          this.GetCurveMaterial(ref this.m_ProjectedMaterial, true);
          num1 = this.m_ArgsArray.Count;
          this.m_ArgsArray.Add(this.m_BoxMesh.GetIndexCount(0));
          this.m_ArgsArray.Add((uint) this.m_ProjectedInstanceCount);
          this.m_ArgsArray.Add(this.m_BoxMesh.GetIndexStart(0));
          this.m_ArgsArray.Add(this.m_BoxMesh.GetBaseVertex(0));
          this.m_ArgsArray.Add(0U);
        }
        if (this.m_AbsoluteInstanceCount != 0)
        {
          this.GetMesh(ref this.m_QuadMesh, false);
          this.GetCurveMaterial(ref this.m_AbsoluteMaterial, false);
          num2 = this.m_ArgsArray.Count;
          this.m_ArgsArray.Add(this.m_QuadMesh.GetIndexCount(0));
          this.m_ArgsArray.Add((uint) this.m_AbsoluteInstanceCount);
          this.m_ArgsArray.Add(this.m_QuadMesh.GetIndexStart(0));
          this.m_ArgsArray.Add(this.m_QuadMesh.GetBaseVertex(0));
          this.m_ArgsArray.Add(0U);
        }
        foreach (Camera camera in cameras)
        {
          if (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView)
          {
            if (this.m_ProjectedInstanceCount != 0)
              Graphics.DrawMeshInstancedIndirect(this.m_BoxMesh, 0, this.m_ProjectedMaterial, bounds, this.m_ArgsBuffer, num1 * 4, castShadows: ShadowCastingMode.Off, receiveShadows: false, camera: camera);
            if (this.m_AbsoluteInstanceCount != 0)
              Graphics.DrawMeshInstancedIndirect(this.m_QuadMesh, 0, this.m_AbsoluteMaterial, bounds, this.m_ArgsBuffer, num2 * 4, castShadows: ShadowCastingMode.Off, receiveShadows: false, camera: camera);
          }
        }
        this.m_ArgsBuffer.SetData<uint>(this.m_ArgsArray, 0, 0, this.m_ArgsArray.Count);
      }
      finally
      {
      }
    }

    private void GetMesh(ref Mesh mesh, bool box)
    {
      if (!((UnityEngine.Object) mesh == (UnityEngine.Object) null))
        return;
      mesh = new Mesh();
      mesh.name = "Overlay";
      if (box)
      {
        mesh.vertices = new Vector3[8]
        {
          new Vector3(-1f, 0.0f, -1f),
          new Vector3(-1f, 0.0f, 1f),
          new Vector3(1f, 0.0f, 1f),
          new Vector3(1f, 0.0f, -1f),
          new Vector3(-1f, 1f, -1f),
          new Vector3(-1f, 1f, 1f),
          new Vector3(1f, 1f, 1f),
          new Vector3(1f, 1f, -1f)
        };
        mesh.triangles = new int[36]
        {
          0,
          1,
          5,
          5,
          4,
          0,
          3,
          7,
          6,
          6,
          2,
          3,
          0,
          3,
          2,
          2,
          1,
          0,
          4,
          5,
          6,
          6,
          7,
          4,
          0,
          4,
          7,
          7,
          3,
          0,
          1,
          2,
          6,
          6,
          5,
          1
        };
      }
      else
      {
        mesh.vertices = new Vector3[4]
        {
          new Vector3(-1f, 0.0f, -1f),
          new Vector3(-1f, 0.0f, 1f),
          new Vector3(1f, 0.0f, 1f),
          new Vector3(1f, 0.0f, -1f)
        };
        mesh.triangles = new int[12]
        {
          0,
          3,
          2,
          2,
          1,
          0,
          0,
          1,
          2,
          2,
          3,
          0
        };
      }
    }

    private void GetCurveMaterial(ref Material material, bool projected)
    {
      if (!((UnityEngine.Object) material == (UnityEngine.Object) null))
        return;
      // ISSUE: reference to a compiler-generated method
      OverlayConfigurationPrefab singletonPrefab = this.m_PrefabSystem.GetSingletonPrefab<OverlayConfigurationPrefab>(this.m_SettingsQuery);
      material = new Material(singletonPrefab.m_CurveMaterial);
      material.name = "Overlay curves";
      if (!projected)
        return;
      material.EnableKeyword("PROJECTED_MODE");
    }

    private void GetCurveBuffer(ref ComputeBuffer buffer, int count)
    {
      if (buffer != null && buffer.count < count)
      {
        count = math.max(buffer.count * 2, count);
        buffer.Release();
        buffer = (ComputeBuffer) null;
      }
      if (buffer != null)
        return;
      buffer = new ComputeBuffer(math.max(64, count), sizeof (OverlayRenderSystem.CurveData));
      buffer.name = "Overlay curve buffer";
    }

    [Preserve]
    public OverlayRenderSystem()
    {
    }

    public struct CurveData
    {
      public Matrix4x4 m_Matrix;
      public Matrix4x4 m_InverseMatrix;
      public Matrix4x4 m_Curve;
      public Color m_OutlineColor;
      public Color m_FillColor;
      public float2 m_Size;
      public float2 m_DashLengths;
      public float2 m_Roundness;
      public float m_OutlineWidth;
      public float m_FillStyle;
    }

    public struct BoundsData
    {
      public Bounds3 m_CurveBounds;
    }

    [Flags]
    public enum StyleFlags
    {
      Grid = 1,
      Projected = 2,
    }

    public struct Buffer
    {
      private NativeList<OverlayRenderSystem.CurveData> m_ProjectedCurves;
      private NativeList<OverlayRenderSystem.CurveData> m_AbsoluteCurves;
      private NativeValue<OverlayRenderSystem.BoundsData> m_Bounds;
      private float m_PositionY;
      private float m_ScaleY;

      public Buffer(
        NativeList<OverlayRenderSystem.CurveData> projectedCurves,
        NativeList<OverlayRenderSystem.CurveData> absoluteCurves,
        NativeValue<OverlayRenderSystem.BoundsData> bounds,
        float positionY,
        float scaleY)
      {
        this.m_ProjectedCurves = projectedCurves;
        this.m_AbsoluteCurves = absoluteCurves;
        this.m_Bounds = bounds;
        this.m_PositionY = positionY;
        this.m_ScaleY = scaleY;
      }

      public void DrawCircle(Color color, float3 position, float diameter)
      {
        this.DrawCircleImpl(color, color, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new float2(0.0f, 1f), position, diameter);
      }

      public void DrawCircle(
        Color outlineColor,
        Color fillColor,
        float outlineWidth,
        OverlayRenderSystem.StyleFlags styleFlags,
        float2 direction,
        float3 position,
        float diameter)
      {
        this.DrawCircleImpl(outlineColor, fillColor, outlineWidth, styleFlags, direction, position, diameter);
      }

      private void DrawCircleImpl(
        Color outlineColor,
        Color fillColor,
        float outlineWidth,
        OverlayRenderSystem.StyleFlags styleFlags,
        float2 direction,
        float3 position,
        float diameter)
      {
        OverlayRenderSystem.CurveData curveData;
        curveData.m_Size = new float2(diameter, diameter);
        curveData.m_DashLengths = new float2(0.0f, diameter);
        curveData.m_Roundness = new float2(1f, 1f);
        curveData.m_OutlineWidth = outlineWidth;
        curveData.m_FillStyle = (float) (styleFlags & OverlayRenderSystem.StyleFlags.Grid);
        curveData.m_Curve = new Matrix4x4((Vector4) new float4(position, 0.0f), (Vector4) new float4(position, 0.0f), (Vector4) new float4(position, 0.0f), (Vector4) new float4(position, 0.0f));
        curveData.m_OutlineColor = outlineColor.linear;
        curveData.m_FillColor = fillColor.linear;
        Bounds3 bounds;
        if ((styleFlags & OverlayRenderSystem.StyleFlags.Projected) != (OverlayRenderSystem.StyleFlags) 0)
        {
          curveData.m_Matrix = this.FitBox(direction, position, diameter, out bounds);
          curveData.m_InverseMatrix = curveData.m_Matrix.inverse;
          this.m_ProjectedCurves.Add(in curveData);
        }
        else
        {
          curveData.m_Matrix = this.FitQuad(direction, position, diameter, out bounds);
          curveData.m_InverseMatrix = curveData.m_Matrix.inverse;
          this.m_AbsoluteCurves.Add(in curveData);
        }
        OverlayRenderSystem.BoundsData boundsData = this.m_Bounds.value;
        boundsData.m_CurveBounds |= bounds;
        this.m_Bounds.value = boundsData;
      }

      public void DrawLine(Color color, Line3.Segment line, float width)
      {
        float length = MathUtils.Length(line.xz);
        this.DrawCurveImpl(color, color, 0.0f, (OverlayRenderSystem.StyleFlags) 0, NetUtils.StraightCurve(line.a, line.b), width, length + width * 2f, 0.0f, new float2(), length);
      }

      public void DrawLine(
        Color outlineColor,
        Color fillColor,
        float outlineWidth,
        OverlayRenderSystem.StyleFlags styleFlags,
        Line3.Segment line,
        float width)
      {
        float length = MathUtils.Length(line.xz);
        this.DrawCurveImpl(outlineColor, fillColor, outlineWidth, styleFlags, NetUtils.StraightCurve(line.a, line.b), width, length + width * 2f, 0.0f, new float2(), length);
      }

      public void DrawLine(Color color, Line3.Segment line, float width, float2 roundness)
      {
        float length = MathUtils.Length(line.xz);
        this.DrawCurveImpl(color, color, 0.0f, (OverlayRenderSystem.StyleFlags) 0, NetUtils.StraightCurve(line.a, line.b), width, length + width * 2f, 0.0f, roundness, length);
      }

      public void DrawLine(
        Color outlineColor,
        Color fillColor,
        float outlineWidth,
        OverlayRenderSystem.StyleFlags styleFlags,
        Line3.Segment line,
        float width,
        float2 roundness)
      {
        float length = MathUtils.Length(line.xz);
        this.DrawCurveImpl(outlineColor, fillColor, outlineWidth, styleFlags, NetUtils.StraightCurve(line.a, line.b), width, length + width * 2f, 0.0f, roundness, length);
      }

      public void DrawDashedLine(
        Color color,
        Line3.Segment line,
        float width,
        float dashLength,
        float gapLength)
      {
        this.DrawCurveImpl(color, color, 0.0f, (OverlayRenderSystem.StyleFlags) 0, NetUtils.StraightCurve(line.a, line.b), width, dashLength, gapLength, new float2(), MathUtils.Length(line.xz));
      }

      public void DrawDashedLine(
        Color outlineColor,
        Color fillColor,
        float outlineWidth,
        OverlayRenderSystem.StyleFlags styleFlags,
        Line3.Segment line,
        float width,
        float dashLength,
        float gapLength)
      {
        this.DrawCurveImpl(outlineColor, fillColor, outlineWidth, styleFlags, NetUtils.StraightCurve(line.a, line.b), width, dashLength, gapLength, new float2(), MathUtils.Length(line.xz));
      }

      public void DrawDashedLine(
        Color color,
        Line3.Segment line,
        float width,
        float dashLength,
        float gapLength,
        float2 roundness)
      {
        this.DrawCurveImpl(color, color, 0.0f, (OverlayRenderSystem.StyleFlags) 0, NetUtils.StraightCurve(line.a, line.b), width, dashLength, gapLength, roundness, MathUtils.Length(line.xz));
      }

      public void DrawDashedLine(
        Color outlineColor,
        Color fillColor,
        float outlineWidth,
        OverlayRenderSystem.StyleFlags styleFlags,
        Line3.Segment line,
        float width,
        float dashLength,
        float gapLength,
        float2 roundness)
      {
        this.DrawCurveImpl(outlineColor, fillColor, outlineWidth, styleFlags, NetUtils.StraightCurve(line.a, line.b), width, dashLength, gapLength, roundness, MathUtils.Length(line.xz));
      }

      public void DrawCurve(Color color, Bezier4x3 curve, float width)
      {
        float length = MathUtils.Length(curve.xz);
        this.DrawCurveImpl(color, color, 0.0f, (OverlayRenderSystem.StyleFlags) 0, curve, width, length + width * 2f, 0.0f, new float2(), length);
      }

      public void DrawCurve(
        Color outlineColor,
        Color fillColor,
        float outlineWidth,
        OverlayRenderSystem.StyleFlags styleFlags,
        Bezier4x3 curve,
        float width)
      {
        float length = MathUtils.Length(curve.xz);
        this.DrawCurveImpl(outlineColor, fillColor, outlineWidth, styleFlags, curve, width, length + width * 2f, 0.0f, new float2(), length);
      }

      public void DrawCurve(Color color, Bezier4x3 curve, float width, float2 roundness)
      {
        float length = MathUtils.Length(curve.xz);
        this.DrawCurveImpl(color, color, 0.0f, (OverlayRenderSystem.StyleFlags) 0, curve, width, length + width * 2f, 0.0f, roundness, length);
      }

      public void DrawCurve(
        Color outlineColor,
        Color fillColor,
        float outlineWidth,
        OverlayRenderSystem.StyleFlags styleFlags,
        Bezier4x3 curve,
        float width,
        float2 roundness)
      {
        float length = MathUtils.Length(curve.xz);
        this.DrawCurveImpl(outlineColor, fillColor, outlineWidth, styleFlags, curve, width, length + width * 2f, 0.0f, roundness, length);
      }

      public void DrawDashedCurve(
        Color color,
        Bezier4x3 curve,
        float width,
        float dashLength,
        float gapLength)
      {
        this.DrawCurveImpl(color, color, 0.0f, (OverlayRenderSystem.StyleFlags) 0, curve, width, dashLength, gapLength, new float2(), MathUtils.Length(curve.xz));
      }

      public void DrawDashedCurve(
        Color outlineColor,
        Color fillColor,
        float outlineWidth,
        OverlayRenderSystem.StyleFlags styleFlags,
        Bezier4x3 curve,
        float width,
        float dashLength,
        float gapLength)
      {
        this.DrawCurveImpl(outlineColor, fillColor, outlineWidth, styleFlags, curve, width, dashLength, gapLength, new float2(), MathUtils.Length(curve.xz));
      }

      private void DrawCurveImpl(
        Color outlineColor,
        Color fillColor,
        float outlineWidth,
        OverlayRenderSystem.StyleFlags styleFlags,
        Bezier4x3 curve,
        float width,
        float dashLength,
        float gapLength,
        float2 roundness,
        float length)
      {
        if ((double) length < 0.0099999997764825821)
          return;
        OverlayRenderSystem.CurveData curveData;
        curveData.m_Size = new float2(width, length);
        curveData.m_DashLengths = new float2(gapLength, dashLength);
        curveData.m_Roundness = roundness;
        curveData.m_OutlineWidth = outlineWidth;
        curveData.m_FillStyle = (float) (styleFlags & OverlayRenderSystem.StyleFlags.Grid);
        curveData.m_Curve = (Matrix4x4) OverlayRenderSystem.Buffer.BuildCurveMatrix(curve, length);
        curveData.m_OutlineColor = outlineColor.linear;
        curveData.m_FillColor = fillColor.linear;
        Bounds3 bounds;
        if ((styleFlags & OverlayRenderSystem.StyleFlags.Projected) != (OverlayRenderSystem.StyleFlags) 0)
        {
          curveData.m_Matrix = this.FitBox(curve, width, out bounds);
          curveData.m_InverseMatrix = curveData.m_Matrix.inverse;
          this.m_ProjectedCurves.Add(in curveData);
        }
        else
        {
          curveData.m_Matrix = this.FitQuad(curve, width, out bounds);
          curveData.m_InverseMatrix = curveData.m_Matrix.inverse;
          this.m_AbsoluteCurves.Add(in curveData);
        }
        OverlayRenderSystem.BoundsData boundsData = this.m_Bounds.value;
        boundsData.m_CurveBounds |= bounds;
        this.m_Bounds.value = boundsData;
      }

      private Matrix4x4 FitBox(
        float2 direction,
        float3 position,
        float extend,
        out Bounds3 bounds)
      {
        bounds = new Bounds3(position, position);
        bounds.min.xz -= extend;
        bounds.max.xz += extend;
        bounds.min.y = this.m_PositionY;
        bounds.max.y = this.m_ScaleY;
        position.y = this.m_PositionY;
        quaternion q = quaternion.RotateY(math.atan2(direction.x, direction.y));
        float3 s = new float3(extend, this.m_ScaleY, extend);
        return Matrix4x4.TRS((Vector3) position, (Quaternion) q, (Vector3) s);
      }

      private Matrix4x4 FitQuad(
        float2 direction,
        float3 position,
        float extend,
        out Bounds3 bounds)
      {
        bounds = new Bounds3(position, position);
        bounds.min.xz -= extend;
        bounds.max.xz += extend;
        quaternion q = quaternion.RotateY(math.atan2(direction.x, direction.y));
        float3 s = new float3(extend, 1f, extend);
        return Matrix4x4.TRS((Vector3) position, (Quaternion) q, (Vector3) s);
      }

      private Matrix4x4 FitBox(Bezier4x3 curve, float extend, out Bounds3 bounds)
      {
        bounds = MathUtils.Bounds(curve);
        bounds.min.xz -= extend;
        bounds.max.xz += extend;
        bounds.min.y = this.m_PositionY;
        bounds.max.y = this.m_ScaleY;
        float3 pos = new float3(0.0f, this.m_PositionY, 0.0f);
        quaternion identity = quaternion.identity;
        float3 s = new float3(0.0f, this.m_ScaleY, 0.0f);
        float2 float2_1 = curve.d.xz - curve.a.xz;
        quaternion q;
        if (MathUtils.TryNormalize(ref float2_1))
        {
          float2 y1 = MathUtils.Right(float2_1);
          float2 x1 = curve.b.xz - curve.a.xz;
          float2 x2 = curve.c.xz - curve.a.xz;
          float2 x3 = curve.d.xz - curve.a.xz;
          float2 y2 = new float2(math.dot(x1, y1), math.dot(x1, float2_1));
          float2 x4 = new float2(math.dot(x2, y1), math.dot(x2, float2_1));
          float2 y3 = new float2(math.dot(x3, y1), math.dot(x3, float2_1));
          float2 x5 = math.min(math.min((float2) 0.0f, y2), math.min(x4, y3));
          float2 y4 = math.max(math.max((float2) 0.0f, y2), math.max(x4, y3));
          float2 float2_2 = math.lerp(x5, y4, 0.5f);
          q = quaternion.LookRotation(new float3(float2_1.x, 0.0f, float2_1.y), new float3(0.0f, 1f, 0.0f));
          pos.xz = curve.a.xz + math.rotate(q, new float3(float2_2.x, 0.0f, float2_2.y)).xz;
          s.xz = (y4 - x5) * 0.5f + extend;
        }
        else
        {
          pos.xz = MathUtils.Center(bounds.xz);
          q = quaternion.identity;
          s.xz = MathUtils.Extents(bounds.xz);
        }
        return Matrix4x4.TRS((Vector3) pos, (Quaternion) q, (Vector3) s);
      }

      private Matrix4x4 FitQuad(Bezier4x3 curve, float extend, out Bounds3 bounds)
      {
        bounds = MathUtils.Bounds(curve);
        bounds.min.xz -= extend;
        bounds.max.xz += extend;
        float3 pos = MathUtils.Center(bounds);
        quaternion q = quaternion.identity;
        float3 s = (float3) 0.0f with
        {
          xz = MathUtils.Extents(bounds.xz),
          y = 1f
        };
        float3 x1 = curve.d - curve.a;
        float num1 = math.length(x1);
        if ((double) num1 > 0.10000000149011612)
        {
          float3 float3_1 = x1 / num1;
          float3 a1 = math.cross(float3_1, curve.b - curve.a);
          float3 a2 = math.cross(float3_1, curve.d - curve.c);
          float3 float3_2 = math.select(a1, -a1, (double) a1.y < 0.0) + math.select(a2, -a2, (double) a2.y < 0.0);
          float num2 = math.length(float3_2);
          float3 float3_3 = math.normalize(math.lerp(new float3(0.0f, 1f, 0.0f), float3_2, math.saturate((float) ((double) num2 / (double) num1 * 10.0))));
          float3 y1 = math.cross(float3_3, float3_1);
          if (MathUtils.TryNormalize(ref y1))
          {
            float3 x2 = curve.b - curve.a;
            float3 x3 = curve.c - curve.a;
            float3 x4 = curve.d - curve.a;
            float2 y2 = new float2(math.dot(x2, y1), math.dot(x2, float3_1));
            float2 x5 = new float2(math.dot(x3, y1), math.dot(x3, float3_1));
            float2 y3 = new float2(math.dot(x4, y1), math.dot(x4, float3_1));
            float2 x6 = math.min(math.min((float2) 0.0f, y2), math.min(x5, y3));
            float2 y4 = math.max(math.max((float2) 0.0f, y2), math.max(x5, y3));
            float2 float2 = math.lerp(x6, y4, 0.5f);
            q = quaternion.LookRotation(float3_1, float3_3);
            pos = curve.a + math.rotate(q, new float3(float2.x, 0.0f, float2.y));
            s.xz = (y4 - x6) * 0.5f + extend;
          }
        }
        return Matrix4x4.TRS((Vector3) pos, (Quaternion) q, (Vector3) s);
      }

      private static float4x4 BuildCurveMatrix(Bezier4x3 curve, float length)
      {
        float2 float2_1;
        float2_1.x = math.distance(curve.a, curve.b);
        float2_1.y = math.distance(curve.c, curve.d);
        float2 float2_2 = float2_1 / length;
        return new float4x4()
        {
          c0 = new float4(curve.a, 0.0f),
          c1 = new float4(curve.b, float2_2.x),
          c2 = new float4(curve.c, 1f - float2_2.y),
          c3 = new float4(curve.d, 1f)
        };
      }
    }
  }
}
