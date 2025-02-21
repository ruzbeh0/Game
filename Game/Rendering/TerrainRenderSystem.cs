// Decompiled with JetBrains decompiler
// Type: Game.Rendering.TerrainRenderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Prefabs;
using Game.Simulation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Rendering
{
  [FormerlySerializedAs("Colossal.Terrain.TerrainRenderSystem, Game")]
  public class TerrainRenderSystem : GameSystemBase
  {
    private TerrainSystem m_TerrainSystem;
    private TerrainMaterialSystem m_TerrainMaterialSystem;
    private OverlayInfomodeSystem m_OverlayInfomodeSystem;
    private SnowSystem m_SnowSystem;
    private Material m_CachedMaterial;

    public Texture overrideOverlaymap { get; set; }

    public Texture overlayExtramap { get; set; }

    public float4 overlayArrowMask { get; set; }

    private Material material
    {
      get => this.m_CachedMaterial;
      set
      {
        if ((Object) this.m_CachedMaterial != (Object) null)
          Object.DestroyImmediate((Object) this.m_CachedMaterial);
        this.m_CachedMaterial = new Material(value);
        this.m_CachedMaterial.SetFloat(TerrainRenderSystem.ShaderID._CODecalLayerMask, (float) math.asuint(1));
      }
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.RequireForUpdate<TerrainPropertiesData>();
      this.material = Colossal.IO.AssetDatabase.AssetDatabase.global.resources.terrain.renderMaterial;
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      this.m_TerrainMaterialSystem = this.World.GetOrCreateSystemManaged<TerrainMaterialSystem>();
      this.m_OverlayInfomodeSystem = this.World.GetOrCreateSystemManaged<OverlayInfomodeSystem>();
      this.m_SnowSystem = this.World.GetOrCreateSystemManaged<SnowSystem>();
    }

    [Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      CoreUtils.Destroy((Object) this.m_CachedMaterial);
    }

    private void UpdateMaterial()
    {
      TerrainSurface validSurface = TerrainSurface.GetValidSurface();
      int baseLOD;
      float4x4 areas;
      float4 ranges;
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.GetCascadeInfo(out int _, out baseLOD, out areas, out ranges, out float4 _);
      Shader.SetGlobalMatrix(TerrainRenderSystem.ShaderID._COTerrainTextureArrayLODArea, (Matrix4x4) areas);
      Shader.SetGlobalVector(TerrainRenderSystem.ShaderID._COTerrainTextureArrayLODRange, (Vector4) ranges);
      Shader.SetGlobalInt(TerrainRenderSystem.ShaderID._COTerrainTextureArrayBaseLod, baseLOD);
      Shader.SetGlobalVector(TerrainRenderSystem.ShaderID._COTerrainHeightScaleOffset, (Vector4) new float4(this.m_TerrainSystem.heightScaleOffset.x, this.m_TerrainSystem.heightScaleOffset.y, 0.0f, 0.0f));
      if ((Object) validSurface == (Object) null)
        return;
      Material material = (Object) this.material == (Object) null ? validSurface.material : this.material;
      if ((Object) material == (Object) null)
        return;
      this.SetKeywords(material);
      material.SetMatrix(TerrainRenderSystem.ShaderID._LODArea, (Matrix4x4) areas);
      material.SetVector(TerrainRenderSystem.ShaderID._LODRange, (Vector4) ranges);
      material.SetVector(TerrainRenderSystem.ShaderID._TerrainScaleOffset, (Vector4) new float4(this.m_TerrainSystem.heightScaleOffset.x, this.m_TerrainSystem.heightScaleOffset.y, 0.0f, 0.0f));
      material.SetVector(TerrainRenderSystem.ShaderID._VTScaleOffset, this.m_TerrainSystem.VTScaleOffset);
      Texture heightmap = this.m_TerrainSystem.heightmap;
      Texture overrideOverlaymap = this.overrideOverlaymap;
      Texture snowDepth = (Texture) this.m_SnowSystem.SnowDepth;
      // ISSUE: reference to a compiler-generated method
      Texture cascadeTexture = this.m_TerrainSystem.GetCascadeTexture();
      Texture splatmap = this.m_TerrainMaterialSystem.splatmap;
      if ((Object) heightmap != (Object) null)
        material.SetTexture(TerrainRenderSystem.ShaderID._HeightMap, heightmap);
      if ((Object) splatmap != (Object) null)
        material.SetTexture(TerrainRenderSystem.ShaderID._SplatMap, splatmap);
      if ((Object) cascadeTexture != (Object) null)
        material.SetTexture(TerrainRenderSystem.ShaderID._HeightMapArray, cascadeTexture);
      if ((Object) overrideOverlaymap != (Object) null)
        material.SetTexture(TerrainRenderSystem.ShaderID._BaseColorMap, overrideOverlaymap);
      if ((Object) this.overlayExtramap != (Object) null)
        material.SetTexture(TerrainRenderSystem.ShaderID._OverlayExtra, this.overlayExtramap);
      if ((Object) snowDepth != (Object) null)
        material.SetTexture(TerrainRenderSystem.ShaderID._SnowMap, snowDepth);
      material.SetVector(TerrainRenderSystem.ShaderID._OverlayArrowMask, (Vector4) this.overlayArrowMask);
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainMaterialSystem.UpdateMaterial(material);
      validSurface.material = material;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      this.UpdateMaterial();
      if (!this.m_TerrainSystem.heightMapRenderRequired)
        return;
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.RenderCascades();
    }

    private void SetKeywords(Material materialToUpdate)
    {
      if ((Object) this.overlayExtramap != (Object) null)
      {
        if ((Object) this.overrideOverlaymap == (Object) null)
          this.overrideOverlaymap = (Texture) Texture2D.whiteTexture;
        materialToUpdate.EnableKeyword("OVERRIDE_OVERLAY_EXTRA");
        materialToUpdate.DisableKeyword("OVERRIDE_OVERLAY_SIMPLE");
      }
      else if ((Object) this.overrideOverlaymap != (Object) null)
      {
        materialToUpdate.DisableKeyword("OVERRIDE_OVERLAY_EXTRA");
        materialToUpdate.EnableKeyword("OVERRIDE_OVERLAY_SIMPLE");
      }
      else
      {
        materialToUpdate.DisableKeyword("OVERRIDE_OVERLAY_EXTRA");
        materialToUpdate.DisableKeyword("OVERRIDE_OVERLAY_SIMPLE");
      }
      if (TerrainSystem.baseLod == 0)
        materialToUpdate.DisableKeyword("_PLAYABLEWORLDSELECT");
      else
        materialToUpdate.EnableKeyword("_PLAYABLEWORLDSELECT");
    }

    public Bounds GetCascadeRegion(int index)
    {
      Bounds cascadeRegion = new Bounds();
      if (index >= 0 && index < this.m_TerrainSystem.heightMapSliceArea.Length)
      {
        float3 min = new float3(this.m_TerrainSystem.heightMapSliceArea[index].x, this.m_TerrainSystem.heightScaleOffset.x, this.m_TerrainSystem.heightMapSliceArea[index].y);
        float3 max = new float3(this.m_TerrainSystem.heightMapSliceArea[index].z, 0.0f, this.m_TerrainSystem.heightMapSliceArea[index].w);
        cascadeRegion.SetMinMax((Vector3) min, (Vector3) max);
      }
      return cascadeRegion;
    }

    public Bounds GetCascadeViewport(int index)
    {
      Bounds cascadeViewport = new Bounds();
      if (index >= 0 && index < this.m_TerrainSystem.heightMapViewportUpdated.Length)
      {
        float2 xy = this.m_TerrainSystem.heightMapSliceArea[index].xy;
        float2 float2 = this.m_TerrainSystem.heightMapSliceArea[index].zw - this.m_TerrainSystem.heightMapSliceArea[index].xy;
        float3 zero1 = float3.zero;
        float3 zero2 = float3.zero;
        zero1.xz = xy + float2 * this.m_TerrainSystem.heightMapViewportUpdated[index].xy;
        zero2.xz = xy + float2 * (this.m_TerrainSystem.heightMapViewportUpdated[index].xy + this.m_TerrainSystem.heightMapViewportUpdated[index].zw);
        zero1.y = 0.0f;
        zero2.y = this.m_TerrainSystem.heightScaleOffset.x;
        cascadeViewport.SetMinMax((Vector3) zero1, (Vector3) zero2);
      }
      return cascadeViewport;
    }

    public Bounds GetCascadeCullArea(int index)
    {
      Bounds cascadeCullArea = new Bounds();
      if (index >= 0 && index < this.m_TerrainSystem.heightMapCullArea.Length)
      {
        float3 min = new float3(this.m_TerrainSystem.heightMapCullArea[index].x, float.MaxValue, this.m_TerrainSystem.heightMapCullArea[index].y);
        float3 max = new float3(this.m_TerrainSystem.heightMapCullArea[index].z, float.MinValue, this.m_TerrainSystem.heightMapCullArea[index].w);
        cascadeCullArea.SetMinMax((Vector3) min, (Vector3) max);
      }
      return cascadeCullArea;
    }

    public Bounds GetLastCullArea()
    {
      Bounds lastCullArea = new Bounds();
      float3 min = new float3(this.m_TerrainSystem.lastCullArea.x, float.MaxValue, this.m_TerrainSystem.lastCullArea.y);
      float3 max = new float3(this.m_TerrainSystem.lastCullArea.z, float.MinValue, this.m_TerrainSystem.lastCullArea.w);
      lastCullArea.SetMinMax((Vector3) min, (Vector3) max);
      return lastCullArea;
    }

    [Preserve]
    public TerrainRenderSystem()
    {
    }

    public class ShaderID
    {
      public static readonly int _COTerrainTextureArrayLODArea = Shader.PropertyToID("colossal_TerrainTextureArrayLODArea");
      public static readonly int _COTerrainTextureArrayLODRange = Shader.PropertyToID("colossal_TerrainTextureArrayLODRange");
      public static readonly int _COTerrainTextureArrayBaseLod = Shader.PropertyToID("colossal_TerrainTextureArrayBaseLod");
      public static readonly int _COTerrainHeightScaleOffset = Shader.PropertyToID("colossal_TerrainHeightScaleOffset");
      public static readonly int _LODArea = Shader.PropertyToID(nameof (_LODArea));
      public static readonly int _LODRange = Shader.PropertyToID(nameof (_LODRange));
      public static readonly int _TerrainScaleOffset = Shader.PropertyToID(nameof (_TerrainScaleOffset));
      public static readonly int _VTScaleOffset = Shader.PropertyToID(nameof (_VTScaleOffset));
      public static readonly int _HeightMap = Shader.PropertyToID(nameof (_HeightMap));
      public static readonly int _SplatMap = Shader.PropertyToID(nameof (_SplatMap));
      public static readonly int _HeightMapArray = Shader.PropertyToID(nameof (_HeightMapArray));
      public static readonly int _BaseColorMap = Shader.PropertyToID(nameof (_BaseColorMap));
      public static readonly int _OverlayExtra = Shader.PropertyToID(nameof (_OverlayExtra));
      public static readonly int _SnowMap = Shader.PropertyToID(nameof (_SnowMap));
      public static readonly int _OverlayArrowMask = Shader.PropertyToID(nameof (_OverlayArrowMask));
      public static readonly int _OverlayPollutionMask = Shader.PropertyToID(nameof (_OverlayPollutionMask));
      public static readonly int _CODecalLayerMask = Shader.PropertyToID("colossal_DecalLayerMask");
    }
  }
}
