// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaterSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.AssetPipeline.Native;
using Colossal.Entities;
using Colossal.Mathematics;
using Colossal.Rendering;
using Colossal.Serialization.Entities;
using Game.Common;
using Game.Events;
using Game.Prefabs;
using Game.Rendering;
using Game.SceneFlow;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.IO.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Simulation
{
  [FormerlySerializedAs("Colossal.Terrain.WaterSystem, Game")]
  [CompilerGenerated]
  public class WaterSystem : GameSystemBase, IDefaultSerializable, ISerializable, IGPUSystem
  {
    public static readonly int kMapSize = 14336;
    public static readonly float kDefaultMinWaterToRestoreHeight = -5f;
    private static float s_SeaLevel;
    private bool m_Loaded;
    public const int MAX_FLOW_DOWNSCALE = 3;
    private int m_numFlowDownsample = 3;
    public float MaxFlowlengthForRender = 0.4f;
    public float PostFlowspeedMultiplier = 2f;
    private SimulationSystem m_SimulationSystem;
    private TerrainSystem m_TerrainSystem;
    private SoilWaterSystem m_SoilWaterSystem;
    private SnowSystem m_SnowSystem;
    private EntityQuery m_SourceGroup;
    private EntityQuery m_SoilWaterParameterGroup;
    private EntityQuery m_WaterLevelChangeGroup;
    private NativeList<WaterSystem.WaterSourceCache> m_SourceCache1;
    private NativeList<WaterSystem.WaterSourceCache> m_SourceCache2;
    private JobHandle m_SourceHandle;
    private int m_SourceCacheIndex;
    private bool m_FlipSourceCache;
    private int m_lastFrameGridSize;
    private const float kGravity = 9.81f;
    private const int kGridSize = 32;
    private static readonly float kCellSize = 7f;
    public float m_TimeStep = 0.03f;
    public float m_Damping = 0.995f;
    public float m_Evaporation = 0.0001f;
    public float m_RainConstant = 5E-05f;
    public float m_PollutionDecayRate = 1f / 1000f;
    public float m_Fluidness = 0.1f;
    public float m_FlowSpeed = 3f / 1000f;
    public float m_ConstantDepthDepth = 200f;
    private float m_lastFrameTimeStep;
    private WaterSystem.QuadWaterBuffer m_Water;
    private ComputeBuffer m_Active;
    private ComputeBuffer m_CurrentActiveTilesIndices;
    private int m_numThreadGroupsTotal;
    private int m_numThreadGroupsX;
    private int m_numThreadGroupsY;
    private NativeArray<int> m_ActiveCPU;
    private NativeArray<int> m_ActiveCPUTemp;
    private SurfaceDataReader m_depthsReader;
    private SurfaceDataReader m_velocitiesReader;
    private JobHandle m_ActiveReaders;
    private uint m_LastReadyFrame;
    private uint m_PreviousReadyFrame;
    private int m_SubFrame;
    private int2 m_TexSize;
    private bool m_NewMap;
    private int m_terrainChangeCounter;
    private float m_restoreHeightMinWaterHeight = WaterSystem.kDefaultMinWaterToRestoreHeight;
    private ComputeShader m_UpdateShader;
    private int m_VelocityKernel;
    private int m_DownsampleKernel;
    private int m_VerticalBlurKernel;
    private int m_HorizontalBlurKernel;
    private int m_FlowPostProcessKernel;
    private int m_DepthKernel;
    private int m_CopyToHeightmapKernel;
    private int m_RestoreHeightFromHeightmapKernel;
    private int m_AddKernel;
    private int m_AddConstantKernel;
    private int m_EvaporateKernel;
    private int m_ResetKernel;
    private int m_ResetActiveKernel;
    private int m_ResetToLevelKernel;
    private int m_LoadKernel;
    private int m_LoadFlowMapKernel;
    private int m_AddBorderKernel;
    private int m_ID_AddPosition;
    private int m_ID_AddRadius;
    private int m_ID_AddAmount;
    private int m_ID_AddPolluted;
    private int m_ID_AreaX;
    private int m_ID_AreaY;
    private int m_ID_CellsPerArea;
    private int m_ID_AreaCountX;
    private int m_ID_AreaCountY;
    private int m_ID_Evaporation;
    private int m_ID_RainConstant;
    private int m_ID_TerrainScale;
    private int m_ID_Timestep;
    private int m_ID_Fluidness;
    private int m_ID_Damping;
    private int m_ID_FlowInterpolationFatcor;
    private int m_ID_CellSize;
    private int m_ID_SoilWaterDepthConstant;
    private int m_ID_SoilOutputMultiplier;
    private int m_ID_AddBorderPosition;
    private int m_ID_PollutionDecayRate;
    private int m_ID_Previous;
    private int m_ID_Result;
    private int m_ID_Terrain;
    private int m_ID_TerrainLod;
    private int m_ID_MaxVelocity;
    private int m_ID_RestoreHeightMinWaterHeight;
    private int m_ID_Active;
    private ulong m_NextSimulationFrame;
    private ulong m_LastReadbackRequest;
    private ulong m_LastDepthReadbackRequest;
    private CommandBuffer m_CommandBuffer;
    private WaterRenderSystem m_WaterRenderSystem;
    private AsyncGPUReadbackHelper m_AsyncGPUReadback;
    private AsyncGPUReadbackHelper m_SaveAsyncGPUReadback;
    private bool m_PendingActiveReadback;
    private static ProfilerMarker m_DepthUpdate = new ProfilerMarker("UpdateDepthMap");
    private WaterSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_1000286415_0;

    public static float SeaLevel => WaterSystem.s_SeaLevel;

    public int WaterSimSpeed { get; set; }

    public float TimeStepOverride { get; set; }

    public bool Loaded => this.m_Loaded;

    public bool UseActiveCellsCulling { get; set; } = true;

    public int2 TextureSize => this.m_TexSize;

    public RenderTexture WaterTexture => this.m_Water.waterTextures[0];

    public RenderTexture WaterRenderTexture => this.m_Water.waterTextures[1];

    public bool BlurFlowMap { get; set; } = true;

    public bool FlowPostProcess { get; set; } = true;

    public int FlowMapNumDownscale
    {
      get => this.m_numFlowDownsample;
      set
      {
        this.m_numFlowDownsample = value;
        Shader.SetGlobalTexture("colossal_FlowTexture", this.FlowTextureUpdated);
        Shader.SetGlobalVector("colossal_FlowTexture_TexelSize", new Vector4((float) this.FlowTextureUpdated.width, (float) this.FlowTextureUpdated.height, 1f / (float) this.FlowTextureUpdated.width));
      }
    }

    public bool EnableFlowDownscale
    {
      get => this.m_numFlowDownsample > 1;
      set
      {
        if (value)
          this.FlowMapNumDownscale = 3;
        else
          this.FlowMapNumDownscale = 0;
      }
    }

    public Texture FlowTextureUpdated
    {
      get
      {
        return this.FlowMapNumDownscale > 0 ? (Texture) this.m_Water.FlowDownScaled(this.FlowMapNumDownscale - 1) : (Texture) this.WaterRenderTexture;
      }
    }

    public float CellSize => WaterSystem.kCellSize;

    public SurfaceWater GetDepth(float3 position, NativeArray<SurfaceWater> waterMap)
    {
      SurfaceWater depth = new SurfaceWater();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float2 float2_1 = (float) WaterSystem.kMapSize / (float2) this.m_TexSize;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int2 cell = WaterSystem.GetCell(position - new float3(float2_1.x / 2f, 0.0f, float2_1.y / 2f), WaterSystem.kMapSize, this.m_TexSize);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      float2 float2_2 = WaterSystem.GetCellCoords(position, WaterSystem.kMapSize, this.m_TexSize) - new float2(0.5f, 0.5f);
      float2 float2_3 = float2_2 - (float2) cell;
      cell.x = math.max(0, cell.x);
      // ISSUE: reference to a compiler-generated field
      cell.x = math.min(this.m_TexSize.x - 2, cell.x);
      cell.y = math.max(0, cell.y);
      // ISSUE: reference to a compiler-generated field
      cell.y = math.min(this.m_TexSize.y - 2, cell.y);
      // ISSUE: reference to a compiler-generated field
      SurfaceWater water1 = waterMap[cell.x + 1 + this.m_TexSize.x * cell.y];
      // ISSUE: reference to a compiler-generated field
      SurfaceWater water2 = waterMap[cell.x + this.m_TexSize.x * cell.y];
      // ISSUE: reference to a compiler-generated field
      SurfaceWater water3 = waterMap[cell.x + this.m_TexSize.x * (cell.y + 1)];
      // ISSUE: reference to a compiler-generated field
      SurfaceWater water4 = waterMap[cell.x + 1 + this.m_TexSize.x * (cell.y + 1)];
      depth.m_Depth = math.lerp(math.lerp(water1.m_Depth, water2.m_Depth, float2_2.x - (float) cell.x), math.lerp(water3.m_Depth, water4.m_Depth, float2_2.x - (float) cell.x), float2_2.y - (float) cell.y);
      depth.m_Depth = math.max(depth.m_Depth, 0.0f);
      depth.m_Polluted = math.lerp(math.lerp(water1.m_Polluted, water2.m_Polluted, float2_2.x - (float) cell.x), math.lerp(water3.m_Polluted, water4.m_Polluted, float2_2.x - (float) cell.x), float2_2.y - (float) cell.y);
      return depth;
    }

    public NativeArray<SurfaceWater> GetDepths(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_depthsReader.JobWriters;
      // ISSUE: reference to a compiler-generated field
      return this.m_depthsReader.WaterSurfaceCPUArray;
    }

    public WaterSurfaceData GetSurfaceData(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_depthsReader.GetSurfaceData(out deps);
    }

    public WaterSurfaceData GetVelocitiesSurfaceData(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_velocitiesReader.GetSurfaceData(out deps);
    }

    public void AddSurfaceReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_depthsReader.JobReaders = JobHandle.CombineDependencies(this.m_depthsReader.JobReaders, handle);
    }

    public void AddVelocitySurfaceReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_velocitiesReader.JobReaders = JobHandle.CombineDependencies(this.m_velocitiesReader.JobReaders, handle);
    }

    public void AddActiveReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveReaders = JobHandle.CombineDependencies(this.m_ActiveReaders, handle);
    }

    public float2 MapSize
    {
      get => WaterSystem.kCellSize * new float2((float) this.m_TexSize.x, (float) this.m_TexSize.y);
    }

    public static float CalculateSourceMultiplier(WaterSourceData source, float3 pos)
    {
      if ((double) source.m_Radius < 0.0099999997764825821)
        return 0.0f;
      pos.y = 0.0f;
      // ISSUE: reference to a compiler-generated field
      int num1 = Mathf.CeilToInt(source.m_Radius / WaterSystem.kCellSize);
      float num2 = 0.0f;
      float num3 = source.m_Radius * source.m_Radius;
      // ISSUE: reference to a compiler-generated field
      int num4 = Mathf.FloorToInt(pos.x / WaterSystem.kCellSize) - num1;
      // ISSUE: reference to a compiler-generated field
      int num5 = Mathf.FloorToInt(pos.z / WaterSystem.kCellSize) - num1;
      for (int index1 = num4; index1 <= num4 + 2 * num1 + 1; ++index1)
      {
        for (int index2 = num5; index2 <= num5 + 2 * num1 + 1; ++index2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 x = new float3((float) index1 * WaterSystem.kCellSize, 0.0f, (float) index2 * WaterSystem.kCellSize);
          num2 += 1f - math.smoothstep(0.0f, 1f, math.distancesq(x, pos) / num3);
        }
      }
      if ((double) num2 >= 1.0 / 1000.0)
        return 1f / num2;
      UnityEngine.Debug.LogWarning((object) string.Format("Warning: water source at {0} has too small radius to work", (object) pos));
      return 1f;
    }

    public NativeArray<int> GetActive() => this.m_ActiveCPU;

    private static float2 GetCellCoords(float3 position, int mapSize, int2 textureSize)
    {
      float2 float2 = (float) mapSize / (float2) textureSize;
      return new float2(((float) (mapSize / 2) + position.x) / float2.x, ((float) (mapSize / 2) + position.z) / float2.y);
    }

    public static int2 GetCell(float3 position, int mapSize, int2 textureSize)
    {
      // ISSUE: reference to a compiler-generated method
      float2 cellCoords = WaterSystem.GetCellCoords(position, mapSize, textureSize);
      return new int2(Mathf.FloorToInt(cellCoords.x), Mathf.FloorToInt(cellCoords.y));
    }

    public int GridSizeMultiplier { get; set; } = 3;

    public int GridSize => 32 * (1 << this.GridSizeMultiplier);

    public float MaxVelocity { get; set; } = 7f;

    public int MaxSpeed { get; set; }

    public int SimulationCycleSteps => 3;

    private int ReadbackRequestInterval => 8;

    private int DepthReadbackRequestInterval => 30;

    public static float WaveSpeed => WaterSystem.kCellSize / 30f;

    public int2 m_ActiveGridSize { get; private set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated method
      this.InitShader();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SoilWaterSystem = this.World.GetOrCreateSystemManaged<SoilWaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SnowSystem = this.World.GetOrCreateSystemManaged<SnowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterRenderSystem = this.World.GetOrCreateSystemManaged<WaterRenderSystem>();
      this.RequireForUpdate<TerrainPropertiesData>();
      // ISSUE: reference to a compiler-generated field
      this.m_SourceGroup = this.GetEntityQuery(ComponentType.ReadOnly<WaterSourceData>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_SoilWaterParameterGroup = this.GetEntityQuery(ComponentType.ReadOnly<SoilWaterParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_WaterLevelChangeGroup = this.GetEntityQuery(ComponentType.ReadOnly<WaterLevelChange>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Deleted>());
      this.WaterSimSpeed = 1;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      // ISSUE: reference to a compiler-generated field
      this.m_CommandBuffer = new CommandBuffer();
      // ISSUE: reference to a compiler-generated field
      this.m_CommandBuffer.name = "Watersystem";
      // ISSUE: reference to a compiler-generated field
      this.m_SourceCache1 = new NativeList<WaterSystem.WaterSourceCache>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_SourceCache2 = new NativeList<WaterSystem.WaterSourceCache>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated method
      this.InitTextures();
    }

    private bool HasWater(float3 position)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float2 float2_1 = (float) WaterSystem.kMapSize / (float2) this.m_TexSize;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int2 cell = WaterSystem.GetCell(position - new float3(float2_1.x / 2f, 0.0f, float2_1.y / 2f), WaterSystem.kMapSize, this.m_TexSize);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      float2 float2_2 = WaterSystem.GetCellCoords(position, WaterSystem.kMapSize, this.m_TexSize) - new float2(0.5f, 0.5f) - (float2) cell;
      cell.x = math.max(0, cell.x);
      // ISSUE: reference to a compiler-generated field
      cell.x = math.min(this.m_TexSize.x - 2, cell.x);
      cell.y = math.max(0, cell.y);
      // ISSUE: reference to a compiler-generated field
      cell.y = math.min(this.m_TexSize.y - 2, cell.y);
      // ISSUE: reference to a compiler-generated field
      return (double) this.m_depthsReader.GetSurface(cell).m_Depth > 0.0;
    }

    public void TerrainWillChangeFromBrush(Bounds2 area)
    {
      if (GameManager.instance.isLoading)
        return;
      // ISSUE: reference to a compiler-generated field
      if (this.m_terrainChangeCounter == 0)
      {
        // ISSUE: reference to a compiler-generated method
        if (this.HasWater(new float3[5]
        {
          new float3(area.Center().x, 0.0f, area.Center().y),
          new float3(area.x.min, 0.0f, area.y.min),
          new float3(area.x.min, 0.0f, area.y.max),
          new float3(area.x.max, 0.0f, area.y.min),
          new float3(area.x.max, 0.0f, area.y.max)
        }[0]))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_restoreHeightMinWaterHeight = -1000000f;
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_terrainChangeCounter = 15;
      this.WaterSimSpeed = 0;
    }

    public void TerrainWillChange()
    {
      if (GameManager.instance.isLoading)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_terrainChangeCounter = 15;
      this.WaterSimSpeed = 0;
    }

    private void InitTextures()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TexSize = new int2(2048, 2048);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_Water = new WaterSystem.QuadWaterBuffer();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_Water.Init(this.m_TexSize);
      // ISSUE: reference to a compiler-generated field
      int2 int2 = this.m_TexSize / this.GridSize;
      this.m_ActiveGridSize = int2;
      // ISSUE: reference to a compiler-generated field
      this.m_Active = new ComputeBuffer(int2.x * int2.y, UnsafeUtility.SizeOf<int>(), ComputeBufferType.Default);
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentActiveTilesIndices = new ComputeBuffer(int2.x * int2.y, UnsafeUtility.SizeOf<int2>(), ComputeBufferType.Default);
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveCPU = new NativeArray<int>(int2.x * int2.y, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      if (this.m_depthsReader != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_depthsReader.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_velocitiesReader != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_velocitiesReader.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_depthsReader = new SurfaceDataReader(this.WaterTexture, WaterSystem.kMapSize);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      this.m_velocitiesReader = new SurfaceDataReader(this.m_Water.FlowDownScaled(0), WaterSystem.kMapSize);
      // ISSUE: reference to a compiler-generated field
      this.m_NewMap = true;
    }

    private void InitShader()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateShader = Colossal.IO.AssetDatabase.AssetDatabase.global.resources.shaders.waterUpdate;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_VelocityKernel = this.m_UpdateShader.FindKernel("VelocityUpdate");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_DownsampleKernel = this.m_UpdateShader.FindKernel("CSDownsample");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_VerticalBlurKernel = this.m_UpdateShader.FindKernel("CSVerticalBlur");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_HorizontalBlurKernel = this.m_UpdateShader.FindKernel("CSHorizontalBlur");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_FlowPostProcessKernel = this.m_UpdateShader.FindKernel("CSFlowPostProcess");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_DepthKernel = this.m_UpdateShader.FindKernel("DepthUpdate");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CopyToHeightmapKernel = this.m_UpdateShader.FindKernel("CopyToHeightmap");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_RestoreHeightFromHeightmapKernel = this.m_UpdateShader.FindKernel("RestoreHeightFromHeightmap");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AddKernel = this.m_UpdateShader.FindKernel("Add");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AddConstantKernel = this.m_UpdateShader.FindKernel("AddConstant");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_EvaporateKernel = this.m_UpdateShader.FindKernel("Evaporate");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ResetKernel = this.m_UpdateShader.FindKernel("Reset");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ResetActiveKernel = this.m_UpdateShader.FindKernel("ResetActive");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ResetToLevelKernel = this.m_UpdateShader.FindKernel("ResetToLevel");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LoadKernel = this.m_UpdateShader.FindKernel("Load");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AddBorderKernel = this.m_UpdateShader.FindKernel("AddBorder");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_AddAmount = Shader.PropertyToID("addAmount");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_AddPolluted = Shader.PropertyToID("addPolluted");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_AddPosition = Shader.PropertyToID("addPosition");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_AddRadius = Shader.PropertyToID("addRadius");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_AreaX = Shader.PropertyToID("areax");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_AreaY = Shader.PropertyToID("areay");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_CellsPerArea = Shader.PropertyToID("cellsPerArea");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_AreaCountX = Shader.PropertyToID("areaCountX");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_AreaCountY = Shader.PropertyToID("areaCountY");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_Evaporation = Shader.PropertyToID("evaporation");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_RainConstant = Shader.PropertyToID("rainConstant");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_TerrainScale = Shader.PropertyToID("terrainScale");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_Timestep = Shader.PropertyToID("timestep");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_Fluidness = Shader.PropertyToID("fluidness");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_Damping = Shader.PropertyToID("damping");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_CellSize = Shader.PropertyToID("cellSize");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_FlowInterpolationFatcor = Shader.PropertyToID("flowInterpolationFatcor");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_PollutionDecayRate = Shader.PropertyToID("pollutionDecayRate");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_AddBorderPosition = Shader.PropertyToID("addBorderPosition");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_RestoreHeightMinWaterHeight = Shader.PropertyToID("restoreHeightMinWaterHeight");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_Previous = Shader.PropertyToID("_Previous");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_Result = Shader.PropertyToID("_Result");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_Terrain = Shader.PropertyToID("_Terrain");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_TerrainLod = Shader.PropertyToID("_TerrainLod");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_Active = Shader.PropertyToID("_Active");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_MaxVelocity = Shader.PropertyToID("maxVelo");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_SoilWaterDepthConstant = Shader.PropertyToID("soilWaterDepthConstant");
      // ISSUE: reference to a compiler-generated field
      this.m_ID_SoilOutputMultiplier = Shader.PropertyToID("soilOutputMultiplier");
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      Shader.SetGlobalVector("colossal_WaterParams", new Vector4(0.0f, 0.0f, 0.0f, 0.0f));
      // ISSUE: reference to a compiler-generated field
      WaterSystem.s_SeaLevel = 0.0f;
      // ISSUE: reference to a compiler-generated field
      if (this.m_velocitiesReader != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_velocitiesReader.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_depthsReader != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_depthsReader.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_SourceCache2.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_SourceCache1.Dispose();
      // ISSUE: reference to a compiler-generated field
      if (this.m_Active != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Active.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_CurrentActiveTilesIndices != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CurrentActiveTilesIndices.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ActiveCPU.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveReaders.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveCPU.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_CommandBuffer.Release();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_Water.Dispose();
      base.OnDestroy();
    }

    public static bool SourceMatchesDirection(
      WaterSourceData source,
      Game.Objects.Transform transform,
      float2 direction)
    {
      if (source.m_ConstantDepth != 2 && source.m_ConstantDepth != 3)
        return false;
      return (double) math.abs(transform.m_Position.x) > (double) math.abs(transform.m_Position.z) ? (double) math.sign(transform.m_Position.x) != (double) math.sign(direction.x) : (double) math.sign(transform.m_Position.z) != (double) math.sign(direction.y);
    }

    public unsafe void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_PreviousReadyFrame);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastReadyFrame);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_NextSimulationFrame);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_TexSize.x);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_TexSize.y);
      NativeArray<float4> output = new NativeArray<float4>(this.WaterTexture.width * this.WaterTexture.height, Allocator.Persistent);
      AsyncGPUReadback.RequestIntoNativeArray<float4>(ref output, (Texture) this.WaterTexture).WaitForCompletion();
      NativeArray<byte> nativeArray = new NativeArray<byte>(output.Length * UnsafeUtility.SizeOf(typeof (float4)), Allocator.Temp);
      NativeCompression.FilterDataBeforeWrite((IntPtr) output.GetUnsafeReadOnlyPtr<float4>(), (IntPtr) nativeArray.GetUnsafePtr<byte>(), (long) nativeArray.Length, UnsafeUtility.SizeOf(typeof (float4)));
      output.Dispose();
      writer.Write(nativeArray);
      nativeArray.Dispose();
    }

    public unsafe void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Game.Version.waterInterpolationFix)
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_PreviousReadyFrame);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LastReadyFrame);
      }
      else
      {
        float f1;
        reader.Read(out f1);
        float f2;
        reader.Read(out f2);
        // ISSUE: reference to a compiler-generated field
        this.m_PreviousReadyFrame = (uint) Mathf.RoundToInt(f1);
        // ISSUE: reference to a compiler-generated field
        this.m_LastReadyFrame = (uint) Mathf.RoundToInt(f2);
      }
      if (reader.context.version >= Game.Version.waterElectricityID)
      {
        if (reader.context.version < Game.Version.waterOverflowFix)
        {
          uint num;
          reader.Read(out num);
          // ISSUE: reference to a compiler-generated field
          this.m_NextSimulationFrame = (ulong) num;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_NextSimulationFrame);
        }
        int num1;
        reader.Read(out num1);
        int num2;
        reader.Read(out num2);
        bool flag = true;
        // ISSUE: reference to a compiler-generated field
        if (num1 != this.m_TexSize.x)
        {
          // ISSUE: reference to a compiler-generated field
          UnityEngine.Debug.LogWarning((object) ("Saved water width = " + num1.ToString() + ", water tex width = " + this.m_TexSize.x.ToString()));
          flag = false;
        }
        // ISSUE: reference to a compiler-generated field
        if (num2 != this.m_TexSize.y)
        {
          // ISSUE: reference to a compiler-generated field
          UnityEngine.Debug.LogWarning((object) ("Saved water height = " + num2.ToString() + ", water tex height = " + this.m_TexSize.y.ToString()));
          flag = false;
        }
        if (reader.context.version < Game.Version.waterGridNotNeeded)
        {
          int num3;
          reader.Read(out num3);
          if (num3 > 0)
          {
            NativeArray<int> nativeArray = new NativeArray<int>(num1 * num2 / (num3 * num3), Allocator.Temp);
            reader.Read(nativeArray);
            nativeArray.Dispose();
          }
        }
        int num4 = num1 * num2;
        NativeArray<float4> nativeArray1 = new NativeArray<float4>(num4, Allocator.Temp);
        if (reader.context.version >= Game.Version.terrainWaterSnowCompression)
        {
          NativeArray<byte> nativeArray2 = new NativeArray<byte>(num4 * sizeof (float4), Allocator.Temp);
          reader.Read(nativeArray2);
          NativeCompression.UnfilterDataAfterRead((IntPtr) nativeArray2.GetUnsafePtr<byte>(), (IntPtr) nativeArray1.GetUnsafePtr<float4>(), (long) nativeArray2.Length, sizeof (float4));
          nativeArray2.Dispose();
        }
        else
          reader.Read(nativeArray1);
        if (flag)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_depthsReader.LoadData(nativeArray1);
        }
        ComputeBuffer buffer = new ComputeBuffer(num4, UnsafeUtility.SizeOf<float4>(), ComputeBufferType.Default);
        buffer.SetData<float4>(nativeArray1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeIntParam(this.m_UpdateShader, this.m_ID_CellsPerArea, this.GridSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeIntParam(this.m_UpdateShader, this.m_ID_AreaCountX, this.m_TexSize.x / this.GridSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeBufferParam(this.m_UpdateShader, this.m_LoadKernel, "_LoadSource", buffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeTextureParam(this.m_UpdateShader, this.m_LoadKernel, this.m_ID_Result, (RenderTargetIdentifier) (Texture) this.WaterTexture);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.DispatchCompute(this.m_UpdateShader, this.m_LoadKernel, this.m_TexSize.x / 16, this.m_TexSize.y / 16, 1);
        this.CurrentJobSourceCache.Clear();
        this.LastFrameSourceCache.Clear();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.ResetActive(this.m_CommandBuffer);
        // ISSUE: reference to a compiler-generated field
        Graphics.ExecuteCommandBuffer(this.m_CommandBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AsyncGPUReadback.Request(this.m_Active);
        // ISSUE: reference to a compiler-generated field
        this.m_PendingActiveReadback = true;
        // ISSUE: reference to a compiler-generated field
        this.m_LastReadbackRequest = 0UL;
        // ISSUE: reference to a compiler-generated field
        this.m_LastDepthReadbackRequest = 0UL;
        // ISSUE: reference to a compiler-generated field
        this.m_NextSimulationFrame = 0UL;
        // ISSUE: reference to a compiler-generated field
        this.m_PreviousReadyFrame = 0U;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.Clear();
        buffer.Dispose();
        nativeArray1.Dispose();
        // ISSUE: reference to a compiler-generated method
        this.BindTextures();
        // ISSUE: reference to a compiler-generated field
        this.m_depthsReader.ExecuteReadBack();
        // ISSUE: reference to a compiler-generated field
        this.m_velocitiesReader.ExecuteReadBack();
        // ISSUE: reference to a compiler-generated field
        this.m_NewMap = true;
      }
      Shader.SetGlobalVector("colossal_WaterParams", new Vector4(0.0f, 0.0f, 0.0f, 0.0f));
      // ISSUE: reference to a compiler-generated field
      WaterSystem.s_SeaLevel = 0.0f;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LastReadbackRequest = 0UL;
      // ISSUE: reference to a compiler-generated field
      this.m_LastDepthReadbackRequest = 0UL;
      // ISSUE: reference to a compiler-generated field
      this.m_PreviousReadyFrame = 0U;
      // ISSUE: reference to a compiler-generated field
      this.m_LastReadyFrame = 0U;
      // ISSUE: reference to a compiler-generated field
      this.m_NextSimulationFrame = 0UL;
      // ISSUE: reference to a compiler-generated method
      this.Reset();
      // ISSUE: reference to a compiler-generated method
      this.BindTextures();
      // ISSUE: reference to a compiler-generated field
      this.m_NewMap = true;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
      Shader.SetGlobalVector("colossal_WaterParams", new Vector4(0.0f, 0.0f, 0.0f, 0.0f));
      // ISSUE: reference to a compiler-generated field
      WaterSystem.s_SeaLevel = 0.0f;
    }

    private void BindTextures()
    {
      Shader.SetGlobalTexture("colossal_WaterTexture", (Texture) this.WaterTexture);
      Shader.SetGlobalVector("colossal_WaterTexture_TexelSize", new Vector4((float) this.WaterRenderTexture.width, (float) this.WaterRenderTexture.height, 1f / (float) this.WaterRenderTexture.width, 1f / (float) this.WaterRenderTexture.height));
      Shader.SetGlobalTexture("colossal_WaterRenderTexture", (Texture) this.WaterRenderTexture);
      Shader.SetGlobalVector("colossal_WateRenderrTexture_TexelSize", new Vector4((float) this.WaterRenderTexture.width, (float) this.WaterRenderTexture.height, 1f / (float) this.WaterRenderTexture.width, 1f / (float) this.WaterRenderTexture.height));
      Shader.SetGlobalTexture("colossal_FlowTexture", this.FlowTextureUpdated);
      Shader.SetGlobalVector("colossal_FlowTexture_TexelSize", new Vector4((float) this.FlowTextureUpdated.width, (float) this.FlowTextureUpdated.height, 1f / (float) this.FlowTextureUpdated.width));
    }

    private void Reset()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateShader.SetTexture(this.m_ResetKernel, this.m_ID_Result, (Texture) this.WaterTexture);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateShader.Dispatch(this.m_ResetKernel, this.m_TexSize.x / 16, this.m_TexSize.y / 16, 1);
    }

    public void ResetToSealevel()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_SourceGroup.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      EntityManager entityManager = this.World.EntityManager;
      float num = float.MaxValue;
      bool flag = false;
      for (int index = 0; index < entityArray.Length; ++index)
      {
        Entity entity = entityArray[index];
        WaterSourceData componentData = entityManager.GetComponentData<WaterSourceData>(entity);
        if (componentData.m_ConstantDepth == 3)
        {
          num = math.min(num, componentData.m_Amount);
          flag = true;
        }
      }
      entityArray.Dispose();
      if (!flag)
        return;
      // ISSUE: reference to a compiler-generated method
      this.ResetToLevel(num);
    }

    private void ResetToLevel(float level)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CommandBuffer.Clear();
      UnityEngine.Debug.Log((object) level);
      // ISSUE: reference to a compiler-generated field
      using (new ProfilingScope(this.m_CommandBuffer, ProfilingSampler.Get<ProfileId>(ProfileId.WaterResetToLevel)))
      {
        // ISSUE: reference to a compiler-generated field
        int2 int2 = this.m_TexSize / this.GridSize;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_AddAmount, level);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_CellSize, WaterSystem.kCellSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeIntParam(this.m_UpdateShader, this.m_ID_CellsPerArea, this.GridSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_CommandBuffer.SetComputeTextureParam(this.m_UpdateShader, this.m_ResetToLevelKernel, this.m_ID_Terrain, (RenderTargetIdentifier) this.m_TerrainSystem.GetCascadeTexture());
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeVectorParam(this.m_UpdateShader, this.m_ID_TerrainScale, (Vector4) new float4(this.m_TerrainSystem.heightScaleOffset.x, this.m_TerrainSystem.positionOffset.xy, 0.0f));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeIntParam(this.m_UpdateShader, this.m_ID_TerrainLod, TerrainSystem.baseLod);
        for (int val1 = 0; val1 < int2.x; ++val1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComputeIntParam(this.m_UpdateShader, this.m_ID_AreaX, val1);
          for (int val2 = 0; val2 < int2.y; ++val2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComputeIntParam(this.m_UpdateShader, this.m_ID_AreaY, val2);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComputeTextureParam(this.m_UpdateShader, this.m_ResetToLevelKernel, this.m_ID_Result, (RenderTargetIdentifier) (Texture) this.WaterTexture);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.DispatchCompute(this.m_UpdateShader, this.m_ResetToLevelKernel, this.GridSize / 16, this.GridSize / 16, 1);
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      Graphics.ExecuteCommandBuffer(this.m_CommandBuffer);
    }

    private bool BorderCircleIntersection(
      bool isX,
      bool isPositive,
      float2 center,
      float radius,
      out int2 result)
    {
      // ISSUE: reference to a compiler-generated field
      float num1 = (float) WaterSystem.kMapSize / 2f;
      double num2 = (double) radius * (double) radius;
      float num3 = math.abs((float) ((isX ? (double) center.x : (double) center.y) - (isPositive ? (double) num1 : -(double) num1)));
      double num4 = (double) num3 * (double) num3;
      float x = (float) (num2 - num4);
      if ((double) x < 0.0)
      {
        result = new int2();
        return false;
      }
      float num5 = isX ? center.y : center.x;
      float num6 = math.sqrt(x);
      float2 float2 = new float2(num5 - num6 + num1, num5 + num6 + num1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      result = new int2(Mathf.FloorToInt((float) this.TextureSize.x * math.saturate(float2.x / (float) WaterSystem.kMapSize)), Mathf.CeilToInt((float) this.TextureSize.y * math.saturate(float2.y / (float) WaterSystem.kMapSize)));
      int y = (isX ? this.TextureSize.y : this.TextureSize.x) - 2;
      if (isX & isPositive)
        ++y;
      result.y = math.min(result.y, y);
      result.x = math.min(result.x, result.y);
      return true;
    }

    private void SourceStep(CommandBuffer cmd)
    {
      int num1 = 0;
      using (new ProfilingScope(cmd, ProfilingSampler.Get<ProfileId>(ProfileId.SourceStep)))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_CellSize, WaterSystem.kCellSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeVectorParam(this.m_UpdateShader, this.m_ID_TerrainScale, (Vector4) new float4(this.m_TerrainSystem.heightScaleOffset.x, this.m_TerrainSystem.positionOffset.y, 0.0f, 0.0f));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeIntParam(this.m_UpdateShader, this.m_ID_TerrainLod, TerrainSystem.baseLod);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_Timestep, this.GetTimeStep());
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveReaders.Complete();
        float x = float.MaxValue;
        foreach (WaterSystem.WaterSourceCache waterSourceCache in this.LastFrameSourceCache)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float2 center = waterSourceCache.m_Position + this.m_TerrainSystem.positionOffset.xz;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cmd.SetComputeVectorParam(this.m_UpdateShader, this.m_ID_AddPosition, (Vector4) new float4(waterSourceCache.m_Position, 0.0f, 0.0f));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_AddRadius, waterSourceCache.m_Radius);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int num2 = 2 * Mathf.CeilToInt(waterSourceCache.m_Radius / WaterSystem.kCellSize) + 1;
          // ISSUE: reference to a compiler-generated field
          if (waterSourceCache.m_ConstantDepth == 1)
          {
            ++num1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_AddConstantKernel, this.m_ID_Result, (RenderTargetIdentifier) (Texture) this.WaterTexture);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_AddConstantKernel, this.m_ID_Terrain, (RenderTargetIdentifier) this.m_TerrainSystem.GetCascadeTexture());
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cmd.SetComputeBufferParam(this.m_UpdateShader, this.m_AddConstantKernel, this.m_ID_Active, this.m_Active);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_AddAmount, waterSourceCache.m_Amount);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cmd.DispatchCompute(this.m_UpdateShader, this.m_AddConstantKernel, num2, num2, 1);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (waterSourceCache.m_ConstantDepth == 0)
            {
              ++num1;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_AddAmount, (float) this.SimulationCycleSteps * waterSourceCache.m_Multiplier * waterSourceCache.m_Amount);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_AddPolluted, waterSourceCache.m_Polluted);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_AddKernel, this.m_ID_Result, (RenderTargetIdentifier) (Texture) this.WaterTexture);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              cmd.SetComputeBufferParam(this.m_UpdateShader, this.m_AddKernel, this.m_ID_Active, this.m_Active);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              cmd.DispatchCompute(this.m_UpdateShader, this.m_AddKernel, num2, num2, 1);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (waterSourceCache.m_ConstantDepth == 2 || waterSourceCache.m_ConstantDepth == 3)
              {
                // ISSUE: reference to a compiler-generated field
                x = math.min(x, waterSourceCache.m_Amount);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_AddBorderKernel, this.m_ID_Result, (RenderTargetIdentifier) (Texture) this.WaterTexture);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_AddBorderKernel, this.m_ID_Terrain, (RenderTargetIdentifier) this.m_TerrainSystem.GetCascadeTexture());
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                cmd.SetComputeBufferParam(this.m_UpdateShader, this.m_AddBorderKernel, this.m_ID_Active, this.m_Active);
                int4 v = new int4();
                int2 result;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                if (this.BorderCircleIntersection(false, false, center, waterSourceCache.m_Radius, out result))
                {
                  ++num1;
                  v.x = result.x;
                  v.y = 0;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  cmd.SetComputeVectorParam(this.m_UpdateShader, this.m_ID_AddBorderPosition, (Vector4) new float4(v));
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_AddAmount, waterSourceCache.m_Amount);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  cmd.DispatchCompute(this.m_UpdateShader, this.m_AddBorderKernel, result.y - result.x + 1, 1, 1);
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                if (this.BorderCircleIntersection(false, true, center, waterSourceCache.m_Radius, out result))
                {
                  ++num1;
                  v.x = result.x;
                  v.y = this.TextureSize.y - 1;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  cmd.SetComputeVectorParam(this.m_UpdateShader, this.m_ID_AddBorderPosition, (Vector4) new float4(v));
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_AddAmount, waterSourceCache.m_Amount);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  cmd.DispatchCompute(this.m_UpdateShader, this.m_AddBorderKernel, result.y - result.x + 1, 1, 1);
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                if (this.BorderCircleIntersection(true, false, center, waterSourceCache.m_Radius, out result))
                {
                  ++num1;
                  v.x = 0;
                  v.y = result.x;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  cmd.SetComputeVectorParam(this.m_UpdateShader, this.m_ID_AddBorderPosition, (Vector4) new float4(v));
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_AddAmount, waterSourceCache.m_Amount);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  cmd.DispatchCompute(this.m_UpdateShader, this.m_AddBorderKernel, 1, 1, result.y - result.x + 1);
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                if (this.BorderCircleIntersection(true, true, center, waterSourceCache.m_Radius, out result))
                {
                  ++num1;
                  v.x = this.TextureSize.x - 1;
                  v.y = result.x;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  cmd.SetComputeVectorParam(this.m_UpdateShader, this.m_ID_AddBorderPosition, (Vector4) new float4(v));
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_AddAmount, waterSourceCache.m_Amount);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  cmd.DispatchCompute(this.m_UpdateShader, this.m_AddBorderKernel, 1, 1, result.y - result.x + 1);
                }
              }
            }
          }
        }
        if ((double) x == 3.4028234663852886E+38)
          return;
        Shader.SetGlobalVector("colossal_WaterParams", new Vector4(x, 0.0f, 0.0f, 0.0f));
        // ISSUE: reference to a compiler-generated field
        WaterSystem.s_SeaLevel = x;
      }
    }

    private void ResetActive(CommandBuffer cmd)
    {
      // ISSUE: reference to a compiler-generated field
      int2 int2 = this.m_TexSize / this.GridSize;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_CellSize, WaterSystem.kCellSize);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cmd.SetComputeBufferParam(this.m_UpdateShader, this.m_ResetActiveKernel, this.m_ID_Active, this.m_Active);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cmd.SetComputeIntParam(this.m_UpdateShader, this.m_ID_AreaCountX, this.m_TexSize.x / this.GridSize);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cmd.SetComputeIntParam(this.m_UpdateShader, this.m_ID_AreaCountY, this.m_TexSize.y / this.GridSize);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cmd.DispatchCompute(this.m_UpdateShader, this.m_ResetActiveKernel, int2.x, int2.y, 1);
    }

    public float GetTimeStep()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_NewMap)
        return 1f;
      // ISSUE: reference to a compiler-generated field
      if ((double) this.m_SimulationSystem.selectedSpeed == 0.0)
        return 0.0f;
      float num1 = Math.Min(UnityEngine.Time.smoothDeltaTime * 30f, 1f);
      // ISSUE: reference to a compiler-generated field
      float num2 = this.m_SimulationSystem.selectedSpeed * 0.25f;
      if ((double) this.TimeStepOverride > 0.0)
        return this.TimeStepOverride;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_lastFrameTimeStep = math.lerp(this.m_lastFrameTimeStep, math.min(1f, num2 * num1), UnityEngine.Time.smoothDeltaTime * 0.2f);
      // ISSUE: reference to a compiler-generated field
      return this.m_lastFrameTimeStep;
    }

    private void EvaporateStep(CommandBuffer cmd)
    {
      using (new ProfilingScope(cmd, ProfilingSampler.Get<ProfileId>(ProfileId.EvaporateStep)))
      {
        bool flag1 = false;
        // ISSUE: reference to a compiler-generated field
        int2 int2 = this.m_TexSize / this.GridSize;
        // ISSUE: reference to a compiler-generated field
        if (this.m_lastFrameGridSize != this.GridSize)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_AsyncGPUReadback.isPending)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_AsyncGPUReadback.WaitForCompletion();
            // ISSUE: reference to a compiler-generated method
            this.UpdateGPUReadback();
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_ActiveCPU.IsCreated)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ActiveReaders.Complete();
            // ISSUE: reference to a compiler-generated field
            this.m_ActiveCPU.Dispose();
            // ISSUE: reference to a compiler-generated field
            this.m_Active.Dispose();
            // ISSUE: reference to a compiler-generated field
            this.m_CurrentActiveTilesIndices.Dispose();
          }
          this.m_ActiveGridSize = int2;
          // ISSUE: reference to a compiler-generated field
          this.m_Active = new ComputeBuffer(int2.x * int2.y, UnsafeUtility.SizeOf<int>(), ComputeBufferType.Default);
          // ISSUE: reference to a compiler-generated field
          this.m_CurrentActiveTilesIndices = new ComputeBuffer(int2.x * int2.y, UnsafeUtility.SizeOf<int2>(), ComputeBufferType.Default);
          // ISSUE: reference to a compiler-generated field
          this.m_ActiveCPU = new NativeArray<int>(int2.x * int2.y, Allocator.Persistent);
          // ISSUE: reference to a compiler-generated method
          this.ResetActive(cmd);
          // ISSUE: reference to a compiler-generated field
          this.m_lastFrameGridSize = this.GridSize;
          flag1 = true;
        }
        SoilWaterParameterData waterParameterData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SoilWaterParameterGroup.TryGetSingleton<SoilWaterParameterData>(out waterParameterData))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_SoilWaterDepthConstant, waterParameterData.m_MaximumWaterDepth);
        // ISSUE: reference to a compiler-generated field
        int num1 = 262144 / SoilWaterSystem.kUpdatesPerDay / this.SimulationCycleSteps;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_SoilOutputMultiplier, waterParameterData.m_WaterPerUnit / (float) num1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_CellSize, WaterSystem.kCellSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeIntParam(this.m_UpdateShader, this.m_ID_AreaCountX, this.m_TexSize.x / this.GridSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeIntParam(this.m_UpdateShader, this.m_ID_AreaCountY, this.m_TexSize.y / this.GridSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_MaxVelocity, this.MaxVelocity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeBufferParam(this.m_UpdateShader, this.m_EvaporateKernel, this.m_ID_Active, this.m_Active);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_EvaporateKernel, "_Snow", (RenderTargetIdentifier) (Texture) this.m_SnowSystem.SnowDepth);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_EvaporateKernel, this.m_ID_Result, (RenderTargetIdentifier) (Texture) this.WaterTexture);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_Timestep, this.GetTimeStep());
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeIntParam(this.m_UpdateShader, this.m_ID_CellsPerArea, this.GridSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_Evaporation, this.m_Evaporation);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_RainConstant, this.m_RainConstant);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_PollutionDecayRate, this.m_PollutionDecayRate);
        // ISSUE: reference to a compiler-generated field
        bool flag2 = flag1 | this.m_NewMap;
        if (flag2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_Timestep, this.m_TimeStep);
        }
        int num2 = 0;
        uint2[] data = new uint2[int2.y * int2.x];
        for (uint x = 0; (long) x < (long) int2.x; ++x)
        {
          for (uint y = 0; (long) y < (long) int2.y; ++y)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.UseActiveCellsCulling | flag2 || this.m_ActiveCPU[(int) ((long) x + (long) int2.x * (long) y)] > 0)
            {
              uint2 uint2 = new uint2(x, y);
              data[num2++] = uint2;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CurrentActiveTilesIndices.SetData((System.Array) data);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeBufferParam(this.m_UpdateShader, this.m_EvaporateKernel, "_CurrentActiveIndices", this.m_CurrentActiveTilesIndices);
        // ISSUE: reference to a compiler-generated field
        this.m_numThreadGroupsX = num2;
        // ISSUE: reference to a compiler-generated field
        this.m_numThreadGroupsY = this.GridSize / 8;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_numThreadGroupsTotal = this.m_numThreadGroupsX * this.m_numThreadGroupsY;
        // ISSUE: reference to a compiler-generated field
        if (this.m_numThreadGroupsTotal <= 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.DispatchCompute(this.m_UpdateShader, this.m_EvaporateKernel, this.m_numThreadGroupsX, this.m_numThreadGroupsY, this.m_numThreadGroupsY);
      }
    }

    private void VelocityStep(CommandBuffer cmd)
    {
      using (new ProfilingScope(cmd, ProfilingSampler.Get<ProfileId>(ProfileId.VelocityStep)))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_ResetKernel, this.m_ID_Result, (RenderTargetIdentifier) (Texture) this.WaterRenderTexture);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.DispatchCompute(this.m_UpdateShader, this.m_ResetKernel, this.m_TexSize.x / 16, this.m_TexSize.y / 16, 1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_VelocityKernel, this.m_ID_Terrain, (RenderTargetIdentifier) this.m_TerrainSystem.GetCascadeTexture());
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeVectorParam(this.m_UpdateShader, this.m_ID_TerrainScale, (Vector4) new float4(this.m_TerrainSystem.heightScaleOffset.xy, 0.0f, 0.0f));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_CellSize, WaterSystem.kCellSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_VelocityKernel, this.m_ID_Previous, (RenderTargetIdentifier) (Texture) this.WaterTexture);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_VelocityKernel, this.m_ID_Result, (RenderTargetIdentifier) (Texture) this.WaterRenderTexture);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_VelocityKernel, "_DownscaledResult", (RenderTargetIdentifier) (Texture) this.m_Water.FlowDownScaled(0));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeIntParam(this.m_UpdateShader, this.m_ID_CellsPerArea, this.GridSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_Fluidness, this.m_Fluidness);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_Damping, this.m_Damping);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_FlowInterpolationFatcor, this.m_NewMap ? 1f : 0.1f);
        // ISSUE: reference to a compiler-generated field
        int y = (this.m_TexSize / this.GridSize).y;
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveReaders.Complete();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeBufferParam(this.m_UpdateShader, this.m_VelocityKernel, "_CurrentActiveIndices", this.m_CurrentActiveTilesIndices);
        // ISSUE: reference to a compiler-generated field
        if (this.m_numThreadGroupsTotal <= 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.DispatchCompute(this.m_UpdateShader, this.m_VelocityKernel, this.m_numThreadGroupsX, this.m_numThreadGroupsY, this.m_numThreadGroupsY);
      }
    }

    private void UpdateGPUReadback()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_AsyncGPUReadback.isPending || this.m_AsyncGPUReadback.hasError)
        return;
      // ISSUE: reference to a compiler-generated field
      if (this.m_AsyncGPUReadback.done)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveCPUTemp = this.m_AsyncGPUReadback.GetData<int>();
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveReaders.Complete();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveCPU.CopyFrom(this.m_ActiveCPUTemp);
        // ISSUE: reference to a compiler-generated field
        this.m_PendingActiveReadback = false;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_AsyncGPUReadback.IncrementFrame();
    }

    private void UpdateSaveReadback()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_SaveAsyncGPUReadback.isPending || this.m_SaveAsyncGPUReadback.hasError)
        return;
      // ISSUE: reference to a compiler-generated field
      if (this.m_SaveAsyncGPUReadback.done)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.JobSaveToFile(this.m_SaveAsyncGPUReadback.GetData<float4>());
      }
      // ISSUE: reference to a compiler-generated field
      this.m_SaveAsyncGPUReadback.IncrementFrame();
    }

    private void RestoreHeightFromHeightmap(CommandBuffer cmd)
    {
      using (new ProfilingScope(cmd, ProfilingSampler.Get<ProfileId>(ProfileId.CopyToHeightMap)))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_RestoreHeightMinWaterHeight, this.m_restoreHeightMinWaterHeight);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_RestoreHeightFromHeightmapKernel, this.m_ID_Result, (RenderTargetIdentifier) (Texture) this.WaterTexture);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeBufferParam(this.m_UpdateShader, this.m_RestoreHeightFromHeightmapKernel, this.m_ID_Active, this.m_Active);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_RestoreHeightFromHeightmapKernel, this.m_ID_Previous, (RenderTargetIdentifier) (Texture) this.WaterRenderTexture);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_RestoreHeightFromHeightmapKernel, this.m_ID_Terrain, (RenderTargetIdentifier) this.m_TerrainSystem.GetCascadeTexture());
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeBufferParam(this.m_UpdateShader, this.m_RestoreHeightFromHeightmapKernel, "_CurrentActiveIndices", this.m_CurrentActiveTilesIndices);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeVectorParam(this.m_UpdateShader, this.m_ID_TerrainScale, (Vector4) new float4(this.m_TerrainSystem.heightScaleOffset.xy, 0.0f, 0.0f));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeIntParam(this.m_UpdateShader, this.m_ID_TerrainLod, TerrainSystem.baseLod);
        // ISSUE: reference to a compiler-generated field
        if (this.m_numThreadGroupsTotal <= 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.DispatchCompute(this.m_UpdateShader, this.m_RestoreHeightFromHeightmapKernel, this.m_numThreadGroupsX, this.m_numThreadGroupsY, this.m_numThreadGroupsY);
      }
    }

    private void DepthStep(CommandBuffer cmd)
    {
      using (new ProfilingScope(cmd, ProfilingSampler.Get<ProfileId>(ProfileId.DepthStep)))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeFloatParam(this.m_UpdateShader, this.m_ID_CellSize, WaterSystem.kCellSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_DepthKernel, this.m_ID_Previous, (RenderTargetIdentifier) (Texture) this.WaterRenderTexture);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_DepthKernel, this.m_ID_Result, (RenderTargetIdentifier) (Texture) this.WaterTexture);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeBufferParam(this.m_UpdateShader, this.m_DepthKernel, this.m_ID_Active, this.m_Active);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeIntParam(this.m_UpdateShader, this.m_ID_CellsPerArea, this.GridSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeIntParam(this.m_UpdateShader, this.m_ID_AreaCountX, this.m_TexSize.x / this.GridSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeIntParam(this.m_UpdateShader, this.m_ID_AreaCountY, this.m_TexSize.y / this.GridSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_DepthKernel, this.m_ID_Terrain, (RenderTargetIdentifier) this.m_TerrainSystem.GetCascadeTexture());
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_DepthKernel, "_Snow", (RenderTargetIdentifier) (Texture) this.m_SnowSystem.SnowDepth);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeVectorParam(this.m_UpdateShader, this.m_ID_TerrainScale, (Vector4) new float4(this.m_TerrainSystem.heightScaleOffset.x, this.m_TerrainSystem.heightScaleOffset.y, 0.0f, 0.0f));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeIntParam(this.m_UpdateShader, this.m_ID_TerrainLod, TerrainSystem.baseLod);
        // ISSUE: reference to a compiler-generated field
        int y = (this.m_TexSize / this.GridSize).y;
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveReaders.Complete();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeBufferParam(this.m_UpdateShader, this.m_DepthKernel, "_CurrentActiveIndices", this.m_CurrentActiveTilesIndices);
        // ISSUE: reference to a compiler-generated field
        if (this.m_numThreadGroupsTotal > 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cmd.DispatchCompute(this.m_UpdateShader, this.m_DepthKernel, this.m_numThreadGroupsX, this.m_numThreadGroupsY, this.m_numThreadGroupsY);
        }
        if (this.FlowMapNumDownscale > 0)
        {
          // ISSUE: reference to a compiler-generated field
          int2 int2 = this.m_TexSize / 2 / 8;
          int num = this.FlowMapNumDownscale - 1;
          for (int index = 0; index < num; ++index)
          {
            int2 /= 2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_DownsampleKernel, this.m_ID_Previous, (RenderTargetIdentifier) (Texture) this.m_Water.FlowDownScaled(index));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_DownsampleKernel, this.m_ID_Result, (RenderTargetIdentifier) (Texture) this.m_Water.FlowDownScaled(index + 1));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cmd.DispatchCompute(this.m_UpdateShader, this.m_DownsampleKernel, int2.x, int2.y, 1);
          }
          if (this.BlurFlowMap && this.FlowMapNumDownscale > 1)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_VerticalBlurKernel, this.m_ID_Previous, (RenderTargetIdentifier) (Texture) this.m_Water.FlowDownScaled(this.FlowMapNumDownscale - 1));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_VerticalBlurKernel, this.m_ID_Result, (RenderTargetIdentifier) (Texture) this.m_Water.FlowDownScaled(this.FlowMapNumDownscale - 2));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cmd.DispatchCompute(this.m_UpdateShader, this.m_VerticalBlurKernel, int2.x, int2.y, 1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_HorizontalBlurKernel, this.m_ID_Previous, (RenderTargetIdentifier) (Texture) this.m_Water.FlowDownScaled(this.FlowMapNumDownscale - 2));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_HorizontalBlurKernel, this.m_ID_Result, (RenderTargetIdentifier) (Texture) this.m_Water.FlowDownScaled(this.FlowMapNumDownscale - 1));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cmd.DispatchCompute(this.m_UpdateShader, this.m_HorizontalBlurKernel, int2.x, int2.y, 1);
          }
          if (this.FlowPostProcess)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_FlowPostProcessKernel, this.m_ID_Result, (RenderTargetIdentifier) (Texture) this.m_Water.FlowDownScaled(this.FlowMapNumDownscale - 1));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cmd.SetComputeFloatParam(this.m_UpdateShader, "maxFlowlengthForRender", this.MaxFlowlengthForRender);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cmd.SetComputeFloatParam(this.m_UpdateShader, "postFlowspeedMultiplier", this.PostFlowspeedMultiplier);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cmd.DispatchCompute(this.m_UpdateShader, this.m_FlowPostProcessKernel, int2.x, int2.y, 1);
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PreviousReadyFrame = this.m_LastReadyFrame;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastReadyFrame = (uint) (this.m_NextSimulationFrame / (ulong) this.MaxSpeed);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_NextSimulationFrame >= this.m_LastReadbackRequest + (ulong) this.ReadbackRequestInterval && !this.m_PendingActiveReadback)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_LastReadbackRequest = this.m_NextSimulationFrame;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AsyncGPUReadback.Request(this.m_Active);
        // ISSUE: reference to a compiler-generated field
        this.m_PendingActiveReadback = true;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_NextSimulationFrame < this.m_LastDepthReadbackRequest + (ulong) this.DepthReadbackRequestInterval)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastDepthReadbackRequest = this.m_NextSimulationFrame;
      // ISSUE: reference to a compiler-generated field
      this.m_depthsReader.ExecuteReadBack();
      // ISSUE: reference to a compiler-generated field
      this.m_velocitiesReader.ExecuteReadBack();
    }

    private void CopyToHeightmapStep(CommandBuffer cmd)
    {
      using (new ProfilingScope(cmd, ProfilingSampler.Get<ProfileId>(ProfileId.CopyToHeightMap)))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_CopyToHeightmapKernel, this.m_ID_Previous, (RenderTargetIdentifier) (Texture) this.WaterTexture);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeBufferParam(this.m_UpdateShader, this.m_CopyToHeightmapKernel, this.m_ID_Active, this.m_Active);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_CopyToHeightmapKernel, "_WaterOut", (RenderTargetIdentifier) (Texture) this.WaterRenderTexture);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        cmd.SetComputeTextureParam(this.m_UpdateShader, this.m_CopyToHeightmapKernel, this.m_ID_Terrain, (RenderTargetIdentifier) this.m_TerrainSystem.GetCascadeTexture());
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeBufferParam(this.m_UpdateShader, this.m_CopyToHeightmapKernel, "_CurrentActiveIndices", this.m_CurrentActiveTilesIndices);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeVectorParam(this.m_UpdateShader, this.m_ID_TerrainScale, (Vector4) new float4(this.m_TerrainSystem.heightScaleOffset.xy, 0.0f, 0.0f));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.SetComputeIntParam(this.m_UpdateShader, this.m_ID_TerrainLod, TerrainSystem.baseLod);
        // ISSUE: reference to a compiler-generated field
        if (this.m_numThreadGroupsTotal <= 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cmd.DispatchCompute(this.m_UpdateShader, this.m_CopyToHeightmapKernel, this.m_numThreadGroupsX, this.m_numThreadGroupsY, this.m_numThreadGroupsY);
      }
    }

    private void Simulate(CommandBuffer cmd)
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.__query_1000286415_0.HasSingleton<TerrainPropertiesData>())
        return;
      cmd.name = "WaterSimulation";
      using (new ProfilingScope(cmd, ProfilingSampler.Get<ProfileId>(ProfileId.SimulateWater)))
      {
        bool flag = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_terrainChangeCounter > 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_NewMap)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_terrainChangeCounter = 0;
            this.WaterSimSpeed = 1;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            --this.m_terrainChangeCounter;
            // ISSUE: reference to a compiler-generated field
            if (this.m_terrainChangeCounter == 0)
            {
              this.WaterSimSpeed = 1;
              flag = true;
            }
          }
        }
        if (this.WaterSimSpeed > 0)
        {
          for (int index = 0; index < this.WaterSimSpeed; ++index)
          {
            if (flag)
            {
              // ISSUE: reference to a compiler-generated method
              this.RestoreHeightFromHeightmap(cmd);
              flag = false;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_restoreHeightMinWaterHeight = WaterSystem.kDefaultMinWaterToRestoreHeight;
            }
            // ISSUE: reference to a compiler-generated method
            this.EvaporateStep(cmd);
            // ISSUE: reference to a compiler-generated method
            this.SourceStep(cmd);
            // ISSUE: reference to a compiler-generated method
            this.VelocityStep(cmd);
            // ISSUE: reference to a compiler-generated method
            this.DepthStep(cmd);
            // ISSUE: reference to a compiler-generated field
            this.m_NextSimulationFrame += (ulong) (uint) (4 * this.MaxSpeed / this.WaterSimSpeed);
            if (this.WaterSimSpeed > 1)
            {
              Graphics.ExecuteCommandBuffer(cmd);
              cmd.Clear();
            }
          }
          // ISSUE: reference to a compiler-generated method
          this.CopyToHeightmapStep(cmd);
          // ISSUE: reference to a compiler-generated field
          this.m_NewMap = false;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_NextSimulationFrame += (ulong) (uint) this.MaxSpeed;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_PreviousReadyFrame = (uint) (this.m_NextSimulationFrame / (ulong) this.MaxSpeed);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_LastReadyFrame = (uint) (this.m_NextSimulationFrame / (ulong) this.MaxSpeed);
        }
      }
      this.LastFrameSourceCache.Clear();
      // ISSUE: reference to a compiler-generated method
      this.UpdateGPUReadback();
    }

    public bool IsAsync { get; set; }

    public void OnSimulateGPU(CommandBuffer cmd)
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.Loaded || this.m_TexSize.x <= 0)
        return;
      // ISSUE: reference to a compiler-generated method
      this.Simulate(cmd);
    }

    public void Save()
    {
      this.WaterSimSpeed = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_SaveAsyncGPUReadback.Request((Texture) this.WaterTexture, 0, GraphicsFormat.R32G32B32A32_SFloat);
    }

    public void Restart()
    {
      // ISSUE: reference to a compiler-generated field
      int2 int2 = this.m_TexSize / this.GridSize;
      int num1 = int2.x * int2.y;
      NativeArray<int> data1 = new NativeArray<int>(num1, Allocator.TempJob);
      new MemsetNativeArray<int>()
      {
        Source = data1,
        Value = 0
      }.Schedule<MemsetNativeArray<int>>(num1, 64).Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_Active.SetData<int>(data1);
      data1.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int num2 = this.m_TexSize.x * this.m_TexSize.y;
      NativeArray<float4> data2 = new NativeArray<float4>(num2, Allocator.TempJob);
      new MemsetNativeArray<float4>()
      {
        Source = data2,
        Value = ((float4) 0)
      }.Schedule<MemsetNativeArray<float4>>(num2, 64).Complete();
      ComputeBuffer buffer = new ComputeBuffer(num2, UnsafeUtility.SizeOf<float4>(), ComputeBufferType.Default);
      buffer.SetData<float4>(data2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateShader.SetInt(this.m_ID_CellsPerArea, this.GridSize);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateShader.SetInt(this.m_ID_AreaCountX, this.m_TexSize.x / this.GridSize);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateShader.SetBuffer(this.m_LoadKernel, "_LoadSource", buffer);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateShader.SetTexture(this.m_LoadKernel, this.m_ID_Result, (Texture) this.WaterTexture);
      buffer.Dispose();
      data2.Dispose();
    }

    public void JobLoad() => throw new NotImplementedException();

    public unsafe byte[] CreateByteArray<T>(NativeArray<T> src) where T : struct
    {
      int size = UnsafeUtility.SizeOf<T>() * src.Length;
      byte* unsafeReadOnlyPtr = (byte*) src.GetUnsafeReadOnlyPtr<T>();
      byte[] byteArray;
      fixed (byte* destination = byteArray = new byte[size])
        UnsafeUtility.MemCpy((void*) destination, (void*) unsafeReadOnlyPtr, (long) size);
      return byteArray;
    }

    private void JobSaveToFile(NativeArray<float4> buffer) => throw new NotImplementedException();

    [UnityEngine.Scripting.Preserve]
    protected override void OnStopRunning() => base.OnStopRunning();

    private NativeList<WaterSystem.WaterSourceCache> LastFrameSourceCache
    {
      get => this.m_SourceCacheIndex != 0 ? this.m_SourceCache2 : this.m_SourceCache1;
    }

    private NativeList<WaterSystem.WaterSourceCache> CurrentJobSourceCache
    {
      get => this.m_SourceCacheIndex != 1 ? this.m_SourceCache2 : this.m_SourceCache1;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SourceHandle.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SourceCacheIndex = 1 - this.m_SourceCacheIndex;
      this.CurrentJobSourceCache.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterLevelChangeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterSourceData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_WaterLevelChange_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle1;
      JobHandle outJobHandle2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WaterSystem.SourceJob jobData = new WaterSystem.SourceJob()
      {
        m_SourceChunks = this.m_SourceGroup.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle1),
        m_EventChunks = this.m_WaterLevelChangeGroup.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle2),
        m_ChangeType = this.__TypeHandle.__Game_Events_WaterLevelChange_RO_ComponentTypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_SourceType = this.__TypeHandle.__Game_Simulation_WaterSourceData_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_ChangePrefabDatas = this.__TypeHandle.__Game_Prefabs_WaterLevelChangeData_RO_ComponentLookup,
        m_TerrainOffset = this.m_TerrainSystem.positionOffset,
        m_Cache = this.CurrentJobSourceCache
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SourceHandle = jobData.Schedule<WaterSystem.SourceJob>(JobUtils.CombineDependencies(this.Dependency, this.m_SourceHandle, outJobHandle1, outJobHandle2));
      // ISSUE: reference to a compiler-generated field
      this.Dependency = this.m_SourceHandle;
      // ISSUE: reference to a compiler-generated method
      this.UpdateSaveReadback();
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveReaders.Complete();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_1000286415_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<TerrainPropertiesData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [UnityEngine.Scripting.Preserve]
    public WaterSystem()
    {
    }

    bool IGPUSystem.get_Enabled() => this.Enabled;

    [Serializable]
    public struct WaterSource
    {
      public int constantDepth;
      public float amount;
      public float2 position;
      public float radius;
      public float pollution;
      public float floodheight;
    }

    public struct QuadWaterBuffer
    {
      public RenderTexture[] waterTextures;
      public RenderTexture[] downdScaledFlowTextures;
      public RenderTexture[] blurredFlowTextures;

      private RenderTexture CreateRenderTexture(string name, int2 size, GraphicsFormat format)
      {
        RenderTexture renderTexture = new RenderTexture(size.x, size.y, 0, format);
        renderTexture.name = name;
        renderTexture.hideFlags = HideFlags.DontSave;
        renderTexture.enableRandomWrite = true;
        renderTexture.wrapMode = TextureWrapMode.Clamp;
        renderTexture.filterMode = FilterMode.Bilinear;
        renderTexture.Create();
        return renderTexture;
      }

      public void Init(int2 size)
      {
        // ISSUE: reference to a compiler-generated field
        this.waterTextures = new RenderTexture[2];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.waterTextures[0] = this.CreateRenderTexture("WaterRT0", size, GraphicsFormat.R32G32B32A32_SFloat);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.waterTextures[1] = this.CreateRenderTexture("WaterRT1", size, GraphicsFormat.R32G32B32A32_SFloat);
        // ISSUE: reference to a compiler-generated field
        this.downdScaledFlowTextures = new RenderTexture[3];
        for (int index = 0; index < 3; ++index)
        {
          size /= 2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.downdScaledFlowTextures[index] = this.CreateRenderTexture(string.Format("FlowTextureDownScaled{0}", (object) index), size, GraphicsFormat.R16G16B16A16_SFloat);
        }
      }

      public void Dispose()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.waterTextures != null)
        {
          // ISSUE: reference to a compiler-generated field
          foreach (UnityEngine.Object waterTexture in this.waterTextures)
            CoreUtils.Destroy(waterTexture);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.downdScaledFlowTextures != null)
        {
          // ISSUE: reference to a compiler-generated field
          foreach (UnityEngine.Object scaledFlowTexture in this.downdScaledFlowTextures)
            CoreUtils.Destroy(scaledFlowTexture);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.blurredFlowTextures == null)
          return;
        // ISSUE: reference to a compiler-generated field
        foreach (UnityEngine.Object blurredFlowTexture in this.blurredFlowTextures)
          CoreUtils.Destroy(blurredFlowTexture);
      }

      public RenderTexture FlowDownScaled(int index) => this.downdScaledFlowTextures[index];
    }

    private struct WaterSourceCache
    {
      public float2 m_Position;
      public float m_Amount;
      public float m_Polluted;
      public float m_Radius;
      public int m_ConstantDepth;
      public float m_Multiplier;
    }

    [BurstCompile]
    private struct SourceJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_SourceChunks;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_EventChunks;
      [ReadOnly]
      public ComponentTypeHandle<WaterLevelChange> m_ChangeType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<WaterSourceData> m_SourceType;
      [ReadOnly]
      public ComponentLookup<WaterLevelChangeData> m_ChangePrefabDatas;
      public NativeList<WaterSystem.WaterSourceCache> m_Cache;
      public float3 m_TerrainOffset;

      private void HandleSource(WaterSourceData source, Game.Objects.Transform transform)
      {
        // ISSUE: reference to a compiler-generated field
        float3 float3 = transform.m_Position - this.m_TerrainOffset;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        WaterSystem.WaterSourceCache waterSourceCache = new WaterSystem.WaterSourceCache()
        {
          m_Amount = source.m_Amount,
          m_ConstantDepth = source.m_ConstantDepth,
          m_Multiplier = source.m_Multiplier,
          m_Polluted = source.m_Polluted,
          m_Radius = source.m_Radius,
          m_Position = float3.xz
        };
        if (source.m_ConstantDepth == 2 || source.m_ConstantDepth == 3)
        {
          float amount = source.m_Amount;
          WaterLevelTargetType waterLevelTargetType = source.m_ConstantDepth == 2 ? WaterLevelTargetType.River : WaterLevelTargetType.Sea;
          // ISSUE: reference to a compiler-generated field
          for (int index1 = 0; index1 < this.m_EventChunks.Length; ++index1)
          {
            // ISSUE: reference to a compiler-generated field
            ArchetypeChunk eventChunk = this.m_EventChunks[index1];
            // ISSUE: reference to a compiler-generated field
            NativeArray<WaterLevelChange> nativeArray1 = eventChunk.GetNativeArray<WaterLevelChange>(ref this.m_ChangeType);
            // ISSUE: reference to a compiler-generated field
            eventChunk = this.m_EventChunks[index1];
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray2 = eventChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              WaterLevelChange waterLevelChange = nativeArray1[index2];
              Entity prefab = nativeArray2[index2].m_Prefab;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ChangePrefabDatas.HasComponent(prefab))
              {
                // ISSUE: reference to a compiler-generated field
                WaterLevelChangeData changePrefabData = this.m_ChangePrefabDatas[prefab];
                // ISSUE: reference to a compiler-generated method
                if (WaterSystem.SourceMatchesDirection(source, transform, waterLevelChange.m_Direction) && (changePrefabData.m_TargetType & waterLevelTargetType) != WaterLevelTargetType.None)
                  amount += source.m_Multiplier * waterLevelChange.m_Intensity;
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          waterSourceCache.m_Amount = amount;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Cache.Add(in waterSourceCache);
      }

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Cache.Clear();
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_SourceChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk sourceChunk = this.m_SourceChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<WaterSourceData> nativeArray1 = sourceChunk.GetNativeArray<WaterSourceData>(ref this.m_SourceType);
          // ISSUE: reference to a compiler-generated field
          sourceChunk = this.m_SourceChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Objects.Transform> nativeArray2 = sourceChunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
          if (nativeArray2.Length > 0)
          {
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated method
              this.HandleSource(nativeArray1[index2], nativeArray2[index2]);
            }
          }
        }
      }
    }

    private struct ReadCommandHelper
    {
      private long m_Position;

      public long currentPosition => this.m_Position;

      public ReadCommandHelper(int position = 0) => this.m_Position = (long) position;

      public unsafe ReadCommand CreateReadCmd(long size, void* buffer = null)
      {
        ReadCommand readCmd;
        // ISSUE: reference to a compiler-generated field
        readCmd.Offset = this.m_Position;
        readCmd.Size = size;
        readCmd.Buffer = (IntPtr) buffer != IntPtr.Zero ? buffer : UnsafeUtility.Malloc(readCmd.Size, 16, Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        this.m_Position += size;
        return readCmd;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<WaterLevelChange> __Game_Events_WaterLevelChange_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterSourceData> __Game_Simulation_WaterSourceData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<WaterLevelChangeData> __Game_Prefabs_WaterLevelChangeData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_WaterLevelChange_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterLevelChange>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterSourceData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterSourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterLevelChangeData_RO_ComponentLookup = state.GetComponentLookup<WaterLevelChangeData>(true);
      }
    }
  }
}
