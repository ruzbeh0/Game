// Decompiled with JetBrains decompiler
// Type: Game.Rendering.TerrainMaterialSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.Serialization.Entities;
using Game.Prefabs;
using Game.Serialization;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Rendering
{
  [FormerlySerializedAs("Colossal.Terrain.TerrainMaterialSystem, Game")]
  [CompilerGenerated]
  public class TerrainMaterialSystem : 
    GameSystemBase,
    IDefaultSerializable,
    ISerializable,
    IPostDeserialize
  {
    private ILog log = LogManager.GetLogger("TerrainTexturing");
    private static readonly float4 kClearViewport = new float4(0.0f, 0.0f, 1f, 1f);
    private const int m_SplatUpdateSize = 128;
    private const int m_SplatRegularUpdateTick = 8;
    private float m_TerrainVTBorder = 1000f;
    private TerrainSystem m_TerrainSystem;
    private WaterRenderSystem m_WaterRenderSystem;
    private PrefabSystem m_PrefabSystem;
    private SnowSystem m_SnowSystem;
    private Material m_SplatMaterial;
    private MaterialPropertyBlock m_Properties = new MaterialPropertyBlock();
    private Mesh m_BlitMesh;
    private RenderTexture m_SplatMap;
    private RenderTexture m_SplatWorldMap;
    private CommandBuffer m_CommandBuffer;
    private Texture2D m_Noise;
    private int m_UpdateIndex;
    private int m_UpdateTick;
    private bool m_ForceUpdateWholeSplatmap;
    private NativeList<Entity> m_MaterialPrefabs;

    private Material splatMaterial
    {
      get => this.m_SplatMaterial;
      set
      {
        if (!((Object) value != (Object) null))
          return;
        this.m_SplatMaterial = new Material(value);
      }
    }

    public Texture splatmap => (Texture) this.m_SplatMap;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterRenderSystem = this.World.GetOrCreateSystemManaged<WaterRenderSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SnowSystem = this.World.GetOrCreateSystemManaged<SnowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CommandBuffer = new CommandBuffer();
      // ISSUE: reference to a compiler-generated field
      this.m_CommandBuffer.name = nameof (TerrainMaterialSystem);
      AssetDatabaseResources.Terrain terrain = Colossal.IO.AssetDatabase.AssetDatabase.global.resources.terrain;
      this.splatMaterial = terrain.splatMaterial;
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalTexture(TerrainMaterialSystem.ShaderID._COTerrainRockDiffuse, (Texture) terrain.rockDiffuse);
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalTexture(TerrainMaterialSystem.ShaderID._COTerrainDirtDiffuse, (Texture) terrain.dirtDiffuse);
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalTexture(TerrainMaterialSystem.ShaderID._COTerrainGrassDiffuse, (Texture) terrain.grassDiffuse);
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalTexture(TerrainMaterialSystem.ShaderID._COTerrainRockNormal, (Texture) terrain.rockNormal);
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalTexture(TerrainMaterialSystem.ShaderID._COTerrainDirtNormal, (Texture) terrain.dirtNormal);
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalTexture(TerrainMaterialSystem.ShaderID._COTerrainGrassNormal, (Texture) terrain.grassNormal);
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalVector(TerrainMaterialSystem.ShaderID._COTerrainTextureTiling, new Vector4(terrain.terrainFarTiling, terrain.terrainCloseTiling, terrain.terrainCloseDirtTiling, 1f));
      // ISSUE: reference to a compiler-generated method
      this.CreateNoiseTexture();
      RenderTexture renderTexture1 = new RenderTexture(4096, 4096, 0, GraphicsFormat.R8G8_UNorm);
      renderTexture1.name = "Splatmap";
      renderTexture1.hideFlags = HideFlags.DontSave;
      // ISSUE: reference to a compiler-generated field
      this.m_SplatMap = renderTexture1;
      // ISSUE: reference to a compiler-generated field
      this.m_SplatMap.Create();
      RenderTexture renderTexture2 = new RenderTexture(1024, 1024, 0, GraphicsFormat.R8G8_UNorm);
      renderTexture2.name = "SplatmapWorld";
      renderTexture2.hideFlags = HideFlags.DontSave;
      // ISSUE: reference to a compiler-generated field
      this.m_SplatWorldMap = renderTexture2;
      // ISSUE: reference to a compiler-generated field
      this.m_SplatWorldMap.Create();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SplatMaterial.SetTexture(TerrainMaterialSystem.ShaderID._NoiseTex, (Texture) this.m_Noise);
      // ISSUE: reference to a compiler-generated field
      this.m_BlitMesh = new Mesh();
      // ISSUE: reference to a compiler-generated field
      this.m_BlitMesh.vertices = new Vector3[3]
      {
        new Vector3(-1f, -1f, 0.0f),
        new Vector3(3f, -1f, 0.0f),
        new Vector3(-1f, 3f, 0.0f)
      };
      // ISSUE: reference to a compiler-generated field
      this.m_BlitMesh.uv = new Vector2[3]
      {
        new Vector2(0.0f, 0.0f),
        new Vector2(2f, 0.0f),
        new Vector2(0.0f, 2f)
      };
      // ISSUE: reference to a compiler-generated field
      this.m_BlitMesh.subMeshCount = 1;
      // ISSUE: reference to a compiler-generated field
      this.m_BlitMesh.SetTriangles(new int[3]{ 0, 2, 1 }, 0);
      // ISSUE: reference to a compiler-generated field
      this.m_BlitMesh.UploadMeshData(true);
      // ISSUE: reference to a compiler-generated field
      this.m_MaterialPrefabs = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    public int GetOrAddMaterialIndex(Entity prefab)
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_MaterialPrefabs.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_MaterialPrefabs[index] == prefab)
          return index;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      TerraformingPrefab prefab1 = this.m_PrefabSystem.GetPrefab<TerraformingPrefab>(prefab);
      Debug.Log((object) ("Adding terrain material: " + prefab1.name), (Object) prefab1);
      // ISSUE: reference to a compiler-generated field
      int length = this.m_MaterialPrefabs.Length;
      // ISSUE: reference to a compiler-generated field
      this.m_MaterialPrefabs.Add(in prefab);
      prefab1.GetComponent<TerrainMaterialProperties>();
      return length;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_MaterialPrefabs);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      reader.Read(this.m_MaterialPrefabs);
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_MaterialPrefabs.Clear();
    }

    public void PatchReferences(ref PrefabReferences references)
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_MaterialPrefabs.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_MaterialPrefabs[index] = references.Check(this.EntityManager, this.m_MaterialPrefabs[index]);
      }
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
    }

    public void ForceUpdateWholeSplatmap() => this.m_ForceUpdateWholeSplatmap = true;

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if ((Object) this.m_SplatMaterial == (Object) null || (Object) this.m_TerrainSystem.GetCascadeTexture() == (Object) null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.log.Trace((object) "Updating..");
      // ISSUE: reference to a compiler-generated field
      ++this.log.indent;
      // ISSUE: reference to a compiler-generated field
      using (new ProfilingScope(this.m_CommandBuffer, ProfilingSampler.Get<ProfileId>(ProfileId.UpdateSplatmap)))
      {
        bool flag = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TerrainSystem.heightMapRenderRequired)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_TerrainSystem.heightMapSliceUpdated[TerrainSystem.baseLod])
          {
            // ISSUE: reference to a compiler-generated field
            this.m_UpdateTick = 0;
            // ISSUE: reference to a compiler-generated field
            float4 viewport = this.m_TerrainSystem.heightMapViewport[TerrainSystem.baseLod];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.UpdateSplatmap(this.m_CommandBuffer, viewport, math.all(viewport == TerrainMaterialSystem.kClearViewport));
            flag = true;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_ForceUpdateWholeSplatmap)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.UpdateSplatmap(this.m_CommandBuffer, TerrainMaterialSystem.kClearViewport, true);
            // ISSUE: reference to a compiler-generated field
            this.m_ForceUpdateWholeSplatmap = false;
            flag = true;
            // ISSUE: reference to a compiler-generated field
            this.m_UpdateTick = 0;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (++this.m_UpdateTick >= 8)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_UpdateTick = 0;
              int num1 = 32;
              // ISSUE: reference to a compiler-generated field
              int num2 = this.m_UpdateIndex % num1;
              // ISSUE: reference to a compiler-generated field
              int num3 = this.m_UpdateIndex / num1;
              // ISSUE: reference to a compiler-generated field
              if (++this.m_UpdateIndex >= num1 * num1)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_UpdateIndex = 0;
              }
              float4 viewport = new float4((float) num2 / (float) num1, (float) num3 / (float) num1, 1f / (float) num1, 1f / (float) num1);
              flag = true;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateSplatmap(this.m_CommandBuffer, viewport, false);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float2 position = this.m_TerrainSystem.playableOffset + viewport.xy * this.m_TerrainSystem.playableArea;
              // ISSUE: reference to a compiler-generated field
              float2 area = viewport.zw * this.m_TerrainSystem.playableArea;
              foreach (WaterSurface instance in WaterSurface.instances)
                instance.UpdateMinMaxArea((Vector2) position, (Vector2) area);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_TerrainSystem.NewMap)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ForceUpdateWholeSplatmap = true;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_TerrainSystem.HandleNewMap();
          foreach (WaterSurface instance in WaterSurface.instances)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            instance.UpdateMinMaxArea((Vector2) this.m_TerrainSystem.worldOffset, (Vector2) this.m_TerrainSystem.worldSize);
          }
        }
        if (flag)
        {
          // ISSUE: reference to a compiler-generated field
          this.log.Trace((object) "Executing command buffer");
          // ISSUE: reference to a compiler-generated field
          Graphics.ExecuteCommandBuffer(this.m_CommandBuffer);
        }
      }
      // ISSUE: reference to a compiler-generated field
      --this.log.indent;
    }

    [Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((Object) this.m_SplatMap);
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((Object) this.m_SplatWorldMap);
      // ISSUE: reference to a compiler-generated field
      this.m_MaterialPrefabs.Dispose();
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((Object) this.m_Noise);
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((Object) this.m_BlitMesh);
    }

    private void UpdateSplatmap(CommandBuffer cmd, float4 viewport, bool bWorldUpdate)
    {
      // ISSUE: reference to a compiler-generated field
      this.log.Trace((object) nameof (UpdateSplatmap));
      cmd.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float4 float4_1 = new float4((this.m_TerrainSystem.worldOffset - this.m_TerrainSystem.playableOffset) / this.m_TerrainSystem.playableArea, this.m_TerrainSystem.worldSize / this.m_TerrainSystem.playableArea);
      // ISSUE: reference to a compiler-generated field
      float4 float4_2 = new float4(this.m_TerrainSystem.heightScaleOffset, 0.0f, 0.0f);
      float4 float4_3 = new float4(0.2f, 0.4f, -1f, 0.0f);
      float4 float4_4 = new float4(500f, 1500f, 0.75f, 0.0f);
      float4 float4_5 = new float4(20f, 300f, 500f, 0.5f);
      float4 float4_6 = new float4(1f, 1f, 1f / 1000f, 1f / 500f);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      Texture cascadeTexture = this.m_TerrainSystem.GetCascadeTexture();
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetTexture(TerrainMaterialSystem.ShaderID._HeightmapArray, cascadeTexture);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetTexture(TerrainMaterialSystem.ShaderID._WaterTexture, this.m_WaterRenderSystem.waterTexture ?? (Texture) Texture2D.blackTexture);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetTexture(TerrainMaterialSystem.ShaderID._NoiseTex, (Texture) this.m_Noise);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetInt(TerrainMaterialSystem.ShaderID._HeightMapArrayIndex, TerrainSystem.baseLod);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetVector(TerrainMaterialSystem.ShaderID._HeightScaleOffset, (Vector4) float4_2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetVector(TerrainMaterialSystem.ShaderID._HeightMapArrayOffsetScale, (Vector4) viewport);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetVector(TerrainMaterialSystem.ShaderID._VTScaleOffset, new Vector4(1f, 1f, 0.0f, 0.0f));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetVector(TerrainMaterialSystem.ShaderID._SplatHeightVariance, (Vector4) float4_3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetVector(TerrainMaterialSystem.ShaderID._SplatRockLimit, (Vector4) float4_4);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetVector(TerrainMaterialSystem.ShaderID._SplatGrassLimit, (Vector4) float4_5);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetVector(TerrainMaterialSystem.ShaderID._NoiseOffset, (Vector4) float4_6);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetVector(TerrainMaterialSystem.ShaderID._WaterTextureOffsetScale, (Vector4) viewport);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetFloat(TerrainMaterialSystem.ShaderID._WorldAdjust, 1f);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Rect pixelRect = new Rect(viewport.x * (float) this.m_SplatMap.width, viewport.y * (float) this.m_SplatMap.height, viewport.z * (float) this.m_SplatMap.width, viewport.w * (float) this.m_SplatMap.height);
      bool flag = (double) viewport.x == 0.0 && (double) viewport.y == 0.0 && (double) viewport.z == 1.0 && (double) viewport.w == 1.0;
      // ISSUE: reference to a compiler-generated field
      cmd.SetRenderTarget((RenderTargetIdentifier) (Texture) this.m_SplatMap, flag ? RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
      cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);
      cmd.SetViewport(pixelRect);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cmd.DrawMesh(this.m_BlitMesh, Matrix4x4.identity, this.m_SplatMaterial, 0, 0, this.m_Properties);
      if (!bWorldUpdate)
        return;
      // ISSUE: reference to a compiler-generated field
      cmd.SetRenderTarget((RenderTargetIdentifier) (Texture) this.m_SplatWorldMap, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cmd.SetViewport(new Rect(0.0f, 0.0f, (float) this.m_SplatWorldMap.width, (float) this.m_SplatWorldMap.height));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetInt(TerrainMaterialSystem.ShaderID._HeightMapArrayIndex, 0);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetVector(TerrainMaterialSystem.ShaderID._HeightMapArrayOffsetScale, new Vector4(0.0f, 0.0f, 1f, 1f));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetVector(TerrainMaterialSystem.ShaderID._WaterTextureOffsetScale, (Vector4) float4_1);
      float4_3.xy *= 0.25f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetVector(TerrainMaterialSystem.ShaderID._SplatHeightVariance, (Vector4) float4_3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Properties.SetFloat(TerrainMaterialSystem.ShaderID._WorldAdjust, 0.7f);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cmd.DrawMesh(this.m_BlitMesh, Matrix4x4.identity, this.m_SplatMaterial, 0, 0, this.m_Properties);
    }

    public void UpdateMaterial(Material material)
    {
      // ISSUE: reference to a compiler-generated field
      material.SetTexture(TerrainMaterialSystem.ShaderID._Splatmap, this.splatmap);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      material.SetTexture(TerrainMaterialSystem.ShaderID._BackdropSnowHeightTexture, (Texture) this.m_SnowSystem.SnowHeightBackdropTexture);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float2 zw = new float2(this.m_TerrainVTBorder) / (this.m_TerrainSystem.playableArea + 2f * this.m_TerrainVTBorder);
      float4 float4_1 = new float4(1f / zw, zw);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float4 float4_2 = new float4(this.m_TerrainSystem.playableArea / (this.m_TerrainSystem.playableArea + 2f * this.m_TerrainVTBorder), zw);
      // ISSUE: reference to a compiler-generated field
      material.SetVector(TerrainMaterialSystem.ShaderID._VTInvBorder, (Vector4) float4_1);
      // ISSUE: reference to a compiler-generated field
      material.SetVector(TerrainMaterialSystem.ShaderID._PlayableScaleOffset, (Vector4) float4_2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalTexture(TerrainMaterialSystem.ShaderID._COSplatmap, (Texture) this.m_SplatMap);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalTexture(TerrainMaterialSystem.ShaderID._COWorldSplatmap, (Texture) this.m_SplatWorldMap);
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalVector(TerrainMaterialSystem.ShaderID._COInvVTBorder, (Vector4) float4_1);
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalVector(TerrainMaterialSystem.ShaderID._COPlayableScaleOffset, (Vector4) float4_2);
    }

    private void CreateNoiseTexture()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Noise = new Texture2D(256, 256, TextureFormat.R8, false);
      byte[] data = new byte[65536];
      for (int index1 = 0; index1 < 256; ++index1)
      {
        for (int index2 = 0; index2 < 256; ++index2)
        {
          float num = Mathf.PerlinNoise((float) index2 * (5f / 128f), (float) index1 * (5f / 128f));
          data[index1 * 256 + index2] = (byte) ((double) byte.MaxValue * (double) num);
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Noise.SetPixelData<byte>(data, 0);
      // ISSUE: reference to a compiler-generated field
      this.m_Noise.Apply();
    }

    [Preserve]
    public TerrainMaterialSystem()
    {
    }

    public class ShaderID
    {
      public static readonly int _HeightmapArray = Shader.PropertyToID("_HeightMapArray");
      public static readonly int _WaterTexture = Shader.PropertyToID(nameof (_WaterTexture));
      public static readonly int _NoiseTex = Shader.PropertyToID(nameof (_NoiseTex));
      public static readonly int _HeightMapArrayOffsetScale = Shader.PropertyToID(nameof (_HeightMapArrayOffsetScale));
      public static readonly int _WaterTextureOffsetScale = Shader.PropertyToID(nameof (_WaterTextureOffsetScale));
      public static readonly int _HeightScaleOffset = Shader.PropertyToID(nameof (_HeightScaleOffset));
      public static readonly int _SplatHeightVariance = Shader.PropertyToID(nameof (_SplatHeightVariance));
      public static readonly int _SplatRockLimit = Shader.PropertyToID(nameof (_SplatRockLimit));
      public static readonly int _SplatGrassLimit = Shader.PropertyToID(nameof (_SplatGrassLimit));
      public static readonly int _WorldAdjust = Shader.PropertyToID(nameof (_WorldAdjust));
      public static readonly int _NoiseOffset = Shader.PropertyToID(nameof (_NoiseOffset));
      public static readonly int _HeightMapArrayIndex = Shader.PropertyToID(nameof (_HeightMapArrayIndex));
      public static readonly int _Splatmap = Shader.PropertyToID(nameof (_Splatmap));
      public static readonly int _VTScaleOffset = Shader.PropertyToID(nameof (_VTScaleOffset));
      public static readonly int _PlayableScaleOffset = Shader.PropertyToID(nameof (_PlayableScaleOffset));
      public static readonly int _VTInvBorder = Shader.PropertyToID("_InvVTBorder");
      public static readonly int _BackdropSnowHeightTexture = Shader.PropertyToID("_BackdropSnowHeight");
      public static readonly int _COSplatmap = Shader.PropertyToID("colossal_Splatmap");
      public static readonly int _COWorldSplatmap = Shader.PropertyToID("colossal_WorldSplatmap");
      public static readonly int _COTerrainRockDiffuse = Shader.PropertyToID("colossal_TerrainRockDiffuse");
      public static readonly int _COTerrainDirtDiffuse = Shader.PropertyToID("colossal_TerrainDirtDiffuse");
      public static readonly int _COTerrainGrassDiffuse = Shader.PropertyToID("colossal_TerrainGrassDiffuse");
      public static readonly int _COTerrainRockNormal = Shader.PropertyToID("colossal_TerrainRockNormal");
      public static readonly int _COTerrainDirtNormal = Shader.PropertyToID("colossal_TerrainDirtNormal");
      public static readonly int _COTerrainGrassNormal = Shader.PropertyToID("colossal_TerrainGrassNormal");
      public static readonly int _COTerrainTextureTiling = Shader.PropertyToID("colossal_TerrainTextureTiling");
      public static readonly int _COPlayableScaleOffset = Shader.PropertyToID("colossal_PlayableScaleOffset");
      public static readonly int _COInvVTBorder = Shader.PropertyToID("colossal_InvVTBorder");
    }
  }
}
