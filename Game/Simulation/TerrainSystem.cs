// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TerrainSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.AssetPipeline.Native;
using Colossal.Entities;
using Colossal.IO.AssetDatabase;
using Colossal.Json;
using Colossal.Mathematics;
using Colossal.Rendering;
using Colossal.Serialization.Entities;
using Game.Areas;
using Game.Assets;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Rendering.Utilities;
using Game.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#nullable disable
namespace Game.Simulation
{
  [FormerlySerializedAs("Colossal.Terrain.TerrainSystem, Game")]
  [CompilerGenerated]
  public class TerrainSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private const float kShiftTerrainAmount = 2000f;
    private const float kSoftenTerrainAmount = 1000f;
    private const float kSlopeAndLevelTerrainAmount = 4000f;
    public static readonly int kDefaultHeightmapWidth = 4096;
    public static readonly int kDefaultHeightmapHeight = TerrainSystem.kDefaultHeightmapWidth;
    private static readonly float2 kDefaultMapSize = new float2(14336f, 14336f);
    private static readonly float2 kDefaultMapOffset = TerrainSystem.kDefaultMapSize * -0.5f;
    private static readonly float2 kDefaultWorldSize = TerrainSystem.kDefaultMapSize * 4f;
    private static readonly float2 kDefaultWorldOffset = TerrainSystem.kDefaultWorldSize * -0.5f;
    private static readonly float2 kDefaultHeightScaleOffset = new float2(4096f, 0.0f);
    private AsyncGPUReadbackHelper m_AsyncGPUReadback;
    private NativeArray<ushort> m_CPUHeights;
    private JobHandle m_CPUHeightReaders;
    private RenderTexture m_Heightmap;
    private RenderTexture m_HeightmapCascade;
    private RenderTexture m_HeightmapDepth;
    private RenderTexture m_WorldMapEditable;
    private Vector4 m_MapOffsetScale;
    private bool m_HeightMapChanged;
    private int4 m_LastPreviewWrite;
    private int4 m_LastWorldPreviewWrite;
    private int4 m_LastWrite;
    private int4 m_LastWorldWrite;
    private int4 m_LastRequest;
    private int m_FailCount;
    private Vector4 m_WorldOffsetScale;
    private bool m_NewMap;
    private bool m_NewMapThisFrame;
    private bool m_Loaded;
    private bool m_UpdateOutOfDate;
    private ComputeShader m_AdjustTerrainCS;
    private int m_ShiftTerrainKernal;
    private int m_BlurHorzKernal;
    private int m_BlurVertKernal;
    private int m_SmoothTerrainKernal;
    private int m_LevelTerrainKernal;
    private int m_SlopeTerrainKernal;
    private CommandBuffer m_CommandBuffer;
    private CommandBuffer m_CascadeCB;
    private Material m_TerrainBlit;
    private Material m_ClipMaterial;
    private EntityQuery m_BrushQuery;
    private NativeList<BuildingUtils.LotInfo> m_BuildingCullList;
    private NativeList<TerrainSystem.LaneSection> m_LaneCullList;
    private NativeList<TerrainSystem.AreaTriangle> m_TriangleCullList;
    private NativeList<TerrainSystem.AreaEdge> m_EdgeCullList;
    private JobHandle m_BuildingCull;
    private JobHandle m_LaneCull;
    private JobHandle m_AreaCull;
    private JobHandle m_ClipMapCull;
    private JobHandle m_CullFinished;
    private NativeParallelHashMap<Entity, Entity> m_BuildingUpgrade;
    private JobHandle m_BuildingUpgradeDependencies;
    public const int kCascadeMax = 4;
    private float4 m_LastCullArea;
    private float4[] m_CascadeRanges;
    private Vector4[] m_ShaderCascadeRanges;
    private float4 m_UpdateArea;
    private float4 m_TerrainChangeArea;
    private bool m_CascadeReset;
    private bool m_RoadUpdate;
    private bool m_AreaUpdate;
    private bool m_TerrainChange;
    private EntityQuery m_BuildingsChanged;
    private EntityQuery m_BuildingGroup;
    private EntityQuery m_RoadsChanged;
    private EntityQuery m_RoadsGroup;
    private EntityQuery m_EditorLotQuery;
    private EntityQuery m_AreasChanged;
    private EntityQuery m_AreasQuery;
    private List<TerrainSystem.CascadeCullInfo> m_CascadeCulling;
    private ManagedStructuredBuffers<TerrainSystem.BuildingLotDraw> m_BuildingInstanceData;
    private ManagedStructuredBuffers<TerrainSystem.LaneDraw> m_LaneInstanceData;
    private ManagedStructuredBuffers<TerrainSystem.AreaTriangle> m_TriangleInstanceData;
    private ManagedStructuredBuffers<TerrainSystem.AreaEdge> m_EdgeInstanceData;
    private Material m_MasterBuildingLotMaterial;
    private Material m_MasterLaneMaterial;
    private Material m_MasterAreaMaterial;
    private Mesh m_LaneMesh;
    private ToolSystem m_ToolSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private GroundHeightSystem m_GroundHeightSystem;
    private RenderingSystem m_RenderingSystem;
    private WaterSystem m_WaterSystem;
    private NativeList<TerrainSystem.ClipMapDraw> m_ClipMapList;
    private ManagedStructuredBuffers<TerrainSystem.ClipMapDraw> m_ClipMapBuffer;
    private ComputeBuffer m_CurrentClipMap;
    private Mesh m_ClipMesh;
    private Mesh m_AreaClipMesh;
    private Mesh.MeshDataArray m_AreaClipMeshData;
    private bool m_HasAreaClipMeshData;
    private JobHandle m_AreaClipMeshDataDeps;
    private TerrainSystem.TerrainMinMaxMap m_TerrainMinMax;
    private TerrainSystem.TypeHandle __TypeHandle;

    public Vector4 VTScaleOffset
    {
      get
      {
        return new Vector4(this.m_WorldOffsetScale.z, this.m_WorldOffsetScale.w, this.m_WorldOffsetScale.x, this.m_WorldOffsetScale.y);
      }
    }

    public bool NewMap => this.m_NewMapThisFrame;

    public Texture heightmap => (Texture) this.m_Heightmap;

    public Vector4 mapOffsetScale => this.m_MapOffsetScale;

    public float2 heightScaleOffset { get; set; }

    public TextureAsset worldMapAsset { get; set; }

    public Texture worldHeightmap { get; set; }

    public Colossal.Hash128 mapReference { get; set; }

    private float GetTerrainAdjustmentSpeed(TerraformingType type)
    {
      if (type == TerraformingType.Shift)
        return 2000f;
      return type == TerraformingType.Soften ? 1000f : 4000f;
    }

    public Bounds GetTerrainBounds()
    {
      return new Bounds((Vector3) new float3(0.0f, (float) (-(double) this.heightScaleOffset.y * 0.5), 0.0f), (Vector3) new float3(14336f, this.heightScaleOffset.x, 14336f));
    }

    public TerrainHeightData GetHeightData(bool waitForPending = false)
    {
      // ISSUE: reference to a compiler-generated field
      if (waitForPending && this.m_HeightMapChanged)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AsyncGPUReadback.WaitForCompletion();
        // ISSUE: reference to a compiler-generated field
        this.m_CPUHeightReaders.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_CPUHeightReaders = new JobHandle();
        // ISSUE: reference to a compiler-generated method
        this.UpdateGPUReadback();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int3 _resolution = !this.m_CPUHeights.IsCreated || (UnityEngine.Object) this.m_HeightmapCascade == (UnityEngine.Object) null || this.m_CPUHeights.Length != this.m_HeightmapCascade.width * this.m_HeightmapCascade.height ? new int3(2, 2, 2) : new int3(this.m_HeightmapCascade.width, 65536, this.m_HeightmapCascade.height);
      float3 float3 = new float3(14336f, math.max(1f, this.heightScaleOffset.x), 14336f);
      float3 _scale = new float3((float) _resolution.x, (float) (_resolution.y - 1), (float) _resolution.z) / float3;
      float3 _offset = -this.positionOffset;
      _offset.xz -= 0.5f / _scale.xz;
      // ISSUE: reference to a compiler-generated field
      return new TerrainHeightData(this.m_CPUHeights, _resolution, _scale, _offset);
    }

    public float2 playableArea { get; private set; }

    public float2 playableOffset { get; private set; }

    public float2 worldSize { get; private set; }

    public float2 worldOffset { get; private set; }

    public float2 worldHeightMinMax { get; private set; }

    public float3 positionOffset
    {
      get => new float3(this.playableOffset.x, this.heightScaleOffset.y, this.playableOffset.y);
    }

    public void AddCPUHeightReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CPUHeightReaders = JobHandle.CombineDependencies(this.m_CPUHeightReaders, handle);
    }

    public bool heightMapRenderRequired { get; private set; }

    public bool[] heightMapSliceUpdated { get; private set; }

    public float4[] heightMapViewport { get; private set; }

    public float4[] heightMapViewportUpdated { get; private set; }

    public float4[] heightMapSliceArea => this.m_CascadeRanges;

    public float4[] heightMapCullArea { get; private set; }

    public bool freezeCascadeUpdates { get; set; }

    public bool[] heightMapSliceUpdatedLast { get; private set; }

    public float4 lastCullArea => this.m_LastCullArea;

    public NativeList<TerrainSystem.LaneSection> GetRoads()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LaneCull.Complete();
      // ISSUE: reference to a compiler-generated field
      return this.m_LaneCullList;
    }

    public static int baseLod { get; private set; }

    public bool GetTerrainBrushUpdate(out float4 viewport)
    {
      // ISSUE: reference to a compiler-generated field
      viewport = this.m_TerrainChangeArea;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_TerrainChange)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainChange = false;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      viewport = new float4(this.m_TerrainChangeArea.x - this.m_CascadeRanges[TerrainSystem.baseLod].x, this.m_TerrainChangeArea.y - this.m_CascadeRanges[TerrainSystem.baseLod].y, this.m_TerrainChangeArea.z - this.m_CascadeRanges[TerrainSystem.baseLod].x, this.m_TerrainChangeArea.w - this.m_CascadeRanges[TerrainSystem.baseLod].y);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      viewport /= new float4(this.m_CascadeRanges[TerrainSystem.baseLod].z - this.m_CascadeRanges[TerrainSystem.baseLod].x, this.m_CascadeRanges[TerrainSystem.baseLod].w - this.m_CascadeRanges[TerrainSystem.baseLod].y, this.m_CascadeRanges[TerrainSystem.baseLod].z - this.m_CascadeRanges[TerrainSystem.baseLod].x, this.m_CascadeRanges[TerrainSystem.baseLod].w - this.m_CascadeRanges[TerrainSystem.baseLod].y);
      viewport.zw -= viewport.xy;
      // ISSUE: reference to a compiler-generated method
      viewport = this.ClipViewport(viewport);
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainChangeArea = viewport;
      return true;
    }

    private ComputeBuffer clipMapBuffer
    {
      get
      {
        if (this.m_CurrentClipMap == null)
        {
          this.m_ClipMapCull.Complete();
          if (this.m_ClipMapList.Length > 0)
          {
            NativeArray<TerrainSystem.ClipMapDraw> data = this.m_ClipMapList.AsArray();
            this.m_ClipMapBuffer.StartFrame();
            this.m_CurrentClipMap = this.m_ClipMapBuffer.Request(data.Length);
            this.m_CurrentClipMap.SetData<TerrainSystem.ClipMapDraw>(data);
            this.m_ClipMapBuffer.EndFrame();
          }
        }
        return this.m_CurrentClipMap;
      }
    }

    private int clipMapInstances
    {
      get
      {
        this.m_ClipMapCull.Complete();
        return this.m_ClipMapList.Length;
      }
    }

    public Mesh areaClipMesh
    {
      get
      {
        if ((UnityEngine.Object) this.m_AreaClipMesh == (UnityEngine.Object) null)
          this.m_AreaClipMesh = new Mesh();
        if (this.m_HasAreaClipMeshData)
        {
          this.m_HasAreaClipMeshData = false;
          this.m_AreaClipMeshDataDeps.Complete();
          Mesh.ApplyAndDisposeWritableMeshData(this.m_AreaClipMeshData, this.m_AreaClipMesh, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
        }
        return this.m_AreaClipMesh;
      }
      private set => this.m_AreaClipMesh = value;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_LastCullArea = float4.zero;
      this.freezeCascadeUpdates = false;
      // ISSUE: reference to a compiler-generated field
      this.m_CPUHeights = new NativeArray<ushort>(4, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_AdjustTerrainCS = Resources.Load<ComputeShader>("AdjustTerrain");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ShiftTerrainKernal = this.m_AdjustTerrainCS.FindKernel("ShiftTerrain");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_BlurHorzKernal = this.m_AdjustTerrainCS.FindKernel("HorzBlur");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_BlurVertKernal = this.m_AdjustTerrainCS.FindKernel("VertBlur");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SmoothTerrainKernal = this.m_AdjustTerrainCS.FindKernel("SmoothTerrain");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LevelTerrainKernal = this.m_AdjustTerrainCS.FindKernel("LevelTerrain");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SlopeTerrainKernal = this.m_AdjustTerrainCS.FindKernel("SlopeTerrain");
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingUpgrade = new NativeParallelHashMap<Entity, Entity>(1024, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_CommandBuffer = new CommandBuffer();
      // ISSUE: reference to a compiler-generated field
      this.m_CommandBuffer.name = "TerrainAdjust";
      // ISSUE: reference to a compiler-generated field
      this.m_CascadeCB = new CommandBuffer();
      // ISSUE: reference to a compiler-generated field
      this.m_CascadeCB.name = "Terrain Cascade";
      // ISSUE: reference to a compiler-generated field
      this.m_MasterBuildingLotMaterial = new Material(Resources.Load<Shader>("BuildingLot"));
      // ISSUE: reference to a compiler-generated field
      this.m_MasterLaneMaterial = new Material(Resources.Load<Shader>("Lane"));
      // ISSUE: reference to a compiler-generated field
      this.m_MasterAreaMaterial = new Material(Resources.Load<Shader>("Area"));
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainBlit = CoreUtils.CreateEngineMaterial(Resources.Load<Shader>("TerrainCascadeBlit"));
      // ISSUE: reference to a compiler-generated field
      this.m_ClipMaterial = CoreUtils.CreateEngineMaterial(Resources.Load<Shader>("RoadClip"));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_TerrainMinMax = new TerrainSystem.TerrainMinMaxMap();
      // ISSUE: reference to a compiler-generated field
      this.m_MapOffsetScale = new Vector4(0.0f, 0.0f, 1f, 1f);
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateArea = float4.zero;
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainChangeArea = float4.zero;
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainChange = false;
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingCullList = new NativeList<BuildingUtils.LotInfo>(1000, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LaneCullList = new NativeList<TerrainSystem.LaneSection>(1000, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_TriangleCullList = new NativeList<TerrainSystem.AreaTriangle>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeCullList = new NativeList<TerrainSystem.AreaEdge>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ClipMapList = new NativeList<TerrainSystem.ClipMapDraw>(1000, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_CascadeCulling = new List<TerrainSystem.CascadeCullInfo>(4);
      for (int index = 0; index < 4; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_CascadeCulling.Add(new TerrainSystem.CascadeCullInfo(this.m_MasterBuildingLotMaterial, this.m_MasterLaneMaterial, this.m_MasterAreaMaterial));
      }
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingInstanceData = new ManagedStructuredBuffers<TerrainSystem.BuildingLotDraw>(10000);
      // ISSUE: reference to a compiler-generated field
      this.m_LaneInstanceData = new ManagedStructuredBuffers<TerrainSystem.LaneDraw>(10000);
      // ISSUE: reference to a compiler-generated field
      this.m_TriangleInstanceData = new ManagedStructuredBuffers<TerrainSystem.AreaTriangle>(1000);
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeInstanceData = new ManagedStructuredBuffers<TerrainSystem.AreaEdge>(1000);
      // ISSUE: reference to a compiler-generated field
      this.m_LastPreviewWrite = int4.zero;
      // ISSUE: reference to a compiler-generated field
      this.m_LastWorldPreviewWrite = int4.zero;
      // ISSUE: reference to a compiler-generated field
      this.m_LastWorldWrite = int4.zero;
      // ISSUE: reference to a compiler-generated field
      this.m_LastWrite = int4.zero;
      // ISSUE: reference to a compiler-generated field
      this.m_LastRequest = int4.zero;
      // ISSUE: reference to a compiler-generated field
      this.m_FailCount = 0;
      TerrainSystem.baseLod = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_NewMap = true;
      // ISSUE: reference to a compiler-generated field
      this.m_NewMapThisFrame = true;
      // ISSUE: reference to a compiler-generated field
      this.m_CascadeReset = true;
      // ISSUE: reference to a compiler-generated field
      this.m_RoadUpdate = false;
      // ISSUE: reference to a compiler-generated field
      this.m_AreaUpdate = false;
      // ISSUE: reference to a compiler-generated field
      this.m_ClipMapBuffer = new ManagedStructuredBuffers<TerrainSystem.ClipMapDraw>(10000);
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentClipMap = (ComputeBuffer) null;
      this.heightMapRenderRequired = false;
      this.heightMapSliceUpdated = new bool[4];
      this.heightMapSliceUpdatedLast = new bool[4];
      this.heightMapViewport = new float4[4];
      this.heightMapViewportUpdated = new float4[4];
      this.heightMapCullArea = new float4[4];
      // ISSUE: reference to a compiler-generated field
      this.m_BrushQuery = this.GetEntityQuery(ComponentType.ReadOnly<Brush>(), ComponentType.Exclude<Hidden>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingsChanged = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Game.Objects.Object>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Buildings.Lot>(),
          ComponentType.ReadOnly<AssetStamp>(),
          ComponentType.ReadOnly<Pillar>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Game.Objects.Object>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Buildings.Lot>(),
          ComponentType.ReadOnly<AssetStamp>(),
          ComponentType.ReadOnly<Pillar>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Game.Objects.Object>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Buildings.Lot>(),
          ComponentType.ReadOnly<AssetStamp>(),
          ComponentType.ReadOnly<Pillar>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingGroup = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Objects.Object>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Buildings.Lot>(),
          ComponentType.ReadOnly<AssetStamp>(),
          ComponentType.ReadOnly<Pillar>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_RoadsChanged = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<EdgeGeometry>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<NodeGeometry>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_RoadsGroup = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<EdgeGeometry>(),
          ComponentType.ReadOnly<NodeGeometry>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AreasChanged = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Clip>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Areas.Terrain>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AreasQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Clip>(),
          ComponentType.ReadOnly<Game.Areas.Terrain>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_EditorLotQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Buildings.Lot>(),
          ComponentType.ReadOnly<Game.Objects.Transform>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Error>(),
          ComponentType.ReadOnly<Warning>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Hidden>(),
          ComponentType.ReadOnly<Deleted>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<AssetStamp>(),
          ComponentType.ReadOnly<Game.Objects.Transform>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Error>(),
          ComponentType.ReadOnly<Warning>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Hidden>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundHeightSystem = this.World.GetOrCreateSystemManaged<GroundHeightSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated method
      this.CreateRoadMeshes();
      // ISSUE: reference to a compiler-generated field
      this.m_Heightmap = (RenderTexture) null;
      // ISSUE: reference to a compiler-generated field
      this.m_HeightmapCascade = (RenderTexture) null;
      // ISSUE: reference to a compiler-generated field
      this.m_HeightmapDepth = (RenderTexture) null;
      // ISSUE: reference to a compiler-generated field
      this.m_WorldMapEditable = (RenderTexture) null;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((UnityEngine.Object) this.m_TerrainBlit);
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((UnityEngine.Object) this.m_ClipMaterial);
      // ISSUE: reference to a compiler-generated field
      if (this.m_CPUHeights.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CPUHeights.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((UnityEngine.Object) this.m_Heightmap);
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((UnityEngine.Object) this.m_HeightmapCascade);
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((UnityEngine.Object) this.m_WorldMapEditable);
      this.worldMapAsset?.Unload(false);
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((UnityEngine.Object) this.m_HeightmapDepth);
      // ISSUE: reference to a compiler-generated field
      if (this.m_BuildingCullList.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CullFinished.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingCullList.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_LaneCullList.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CullFinished.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_LaneCullList.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_TriangleCullList.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CullFinished.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_TriangleCullList.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_EdgeCullList.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CullFinished.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeCullList.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ClipMapList.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ClipMapCull.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_ClipMapList.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_BuildingInstanceData != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingInstanceData.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingInstanceData = (ManagedStructuredBuffers<TerrainSystem.BuildingLotDraw>) null;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_LaneInstanceData != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LaneInstanceData.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_LaneInstanceData = (ManagedStructuredBuffers<TerrainSystem.LaneDraw>) null;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_TriangleInstanceData != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TriangleInstanceData.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_TriangleInstanceData = (ManagedStructuredBuffers<TerrainSystem.AreaTriangle>) null;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_EdgeInstanceData != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeInstanceData.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeInstanceData = (ManagedStructuredBuffers<TerrainSystem.AreaEdge>) null;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ClipMapBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ClipMapBuffer.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_ClipMapBuffer = (ManagedStructuredBuffers<TerrainSystem.ClipMapDraw>) null;
      }
      for (int index = 0; index < 4; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CascadeCulling[index].m_BuildingHandle.IsCompleted)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CascadeCulling[index].m_BuildingHandle.Complete();
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CascadeCulling[index].m_BuildingRenderList.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CascadeCulling[index].m_BuildingRenderList.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CascadeCulling[index].m_LaneHandle.IsCompleted)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CascadeCulling[index].m_LaneHandle.Complete();
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CascadeCulling[index].m_LaneRenderList.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CascadeCulling[index].m_LaneRenderList.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CascadeCulling[index].m_AreaHandle.IsCompleted)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CascadeCulling[index].m_AreaHandle.Complete();
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CascadeCulling[index].m_TriangleRenderList.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CascadeCulling[index].m_TriangleRenderList.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CascadeCulling[index].m_EdgeRenderList.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CascadeCulling[index].m_EdgeRenderList.Dispose();
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_BuildingUpgrade.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingUpgradeDependencies.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingUpgrade.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_CascadeCB.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_CommandBuffer.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainMinMax.Dispose();
      base.OnDestroy();
    }

    private static unsafe void SerializeHeightmap<TWriter>(TWriter writer, Texture heightmap) where TWriter : IWriter
    {
      if ((UnityEngine.Object) heightmap == (UnityEngine.Object) null)
      {
        writer.Write(0);
        writer.Write(0);
      }
      else
      {
        writer.Write(heightmap.width);
        writer.Write(heightmap.height);
        NativeArray<ushort> output = new NativeArray<ushort>(heightmap.width * heightmap.height, Allocator.Persistent);
        AsyncGPUReadback.RequestIntoNativeArray<ushort>(ref output, heightmap).WaitForCompletion();
        NativeArray<byte> nativeArray = new NativeArray<byte>(output.Length * 2, Allocator.Temp);
        NativeCompression.FilterDataBeforeWrite((IntPtr) output.GetUnsafeReadOnlyPtr<ushort>(), (IntPtr) nativeArray.GetUnsafePtr<byte>(), (long) nativeArray.Length, 2);
        output.Dispose();
        writer.Write(nativeArray);
        nativeArray.Dispose();
      }
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.mapReference);
      // ISSUE: reference to a compiler-generated method
      TerrainSystem.SerializeHeightmap<TWriter>(writer, this.worldHeightmap);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      TerrainSystem.SerializeHeightmap<TWriter>(writer, (Texture) this.m_Heightmap);
      writer.Write(this.heightScaleOffset);
      writer.Write(this.playableOffset);
      writer.Write(this.playableArea);
      writer.Write(this.worldOffset);
      writer.Write(this.worldSize);
      writer.Write(this.worldHeightMinMax);
    }

    private static unsafe Texture2D DeserializeHeightmap<TReader>(
      TReader reader,
      string name,
      ref NativeArray<ushort> unfiltered,
      bool makeNoLongerReadable)
      where TReader : IReader
    {
      int width;
      reader.Read(out width);
      int height;
      reader.Read(out height);
      if (width == 0 || height == 0)
        return (Texture2D) null;
      Texture2D texture2D1 = new Texture2D(width, height, GraphicsFormat.R16_UNorm, TextureCreationFlags.DontInitializePixels | TextureCreationFlags.DontUploadUponCreate);
      texture2D1.hideFlags = HideFlags.HideAndDontSave;
      texture2D1.name = name;
      texture2D1.filterMode = FilterMode.Bilinear;
      texture2D1.wrapMode = TextureWrapMode.Clamp;
      Texture2D texture2D2 = texture2D1;
      using (NativeArray<ushort> rawTextureData = texture2D2.GetRawTextureData<ushort>())
      {
        if (reader.context.version >= Game.Version.terrainWaterSnowCompression)
        {
          if (unfiltered.Length != rawTextureData.Length)
            unfiltered.ResizeArray<ushort>(rawTextureData.Length);
          NativeArray<byte> nativeArray = unfiltered.Reinterpret<byte>(2);
          reader.Read(nativeArray);
          NativeCompression.UnfilterDataAfterRead((IntPtr) nativeArray.GetUnsafePtr<byte>(), (IntPtr) rawTextureData.GetUnsafePtr<ushort>(), (long) nativeArray.Length, 2);
        }
        else
          reader.Read(rawTextureData);
        texture2D2.Apply(false, makeNoLongerReadable);
      }
      return texture2D2;
    }

    private TextureAsset LegacyLoadWorldMap()
    {
      MapMetadata asset = Colossal.IO.AssetDatabase.AssetDatabase.global.GetAsset<MapMetadata>(this.mapReference);
      if (!((AssetData) asset != (IAssetData) null))
        return (TextureAsset) null;
      // ISSUE: variable of a compiler-generated type
      TerrainSystem.TerrainDesc terrainDesc = asset.LoadAs<TerrainSystem.TerrainDesc>();
      switch (this.mapReference.ToString())
      {
        case "09976b87610b2235e871ef990fe1d641":
          terrainDesc.worldHeightMapGuid = new Colossal.Hash128("bf0a154278bf44d5ca4615542c6a8328");
          break;
        case "0de682325fee4145d8f6b476abc1b49f":
          terrainDesc.worldHeightMapGuid = new Colossal.Hash128("bbb43ef6eeb2c9a58bcd8f088e86ea4d");
          break;
        case "28793380017d5135995a603dda66f808":
          terrainDesc.worldHeightMapGuid = new Colossal.Hash128("41eb8bd1626d000568fc39f052c32869");
          break;
        case "3a84420e77337665c8e4e20cfc3f8e82":
          terrainDesc.worldHeightMapGuid = new Colossal.Hash128("4281ab9d49876d65aab7cb0860c7e181");
          break;
        case "3ca4d7fcb0152c951a9c64b073ae20eb":
          terrainDesc.worldHeightMapGuid = new Colossal.Hash128("4e96721be8b17b15e83baba2bbf6dde6");
          break;
        case "3e902647f5783aa50afa3a45d4bd567f":
          terrainDesc.worldHeightMapGuid = new Colossal.Hash128("47af40991307d3055b8317f3d8de2986");
          break;
        case "419f928408010f45fa6bff8926656ffe":
          terrainDesc.worldHeightMapGuid = new Colossal.Hash128("7030399925b76835cb458e9eba8959d0");
          break;
        case "5b0804a2f2200b950acca980ed0e79c4":
          terrainDesc.worldHeightMapGuid = new Colossal.Hash128("a4f3ee3c39a4d2a54b638952caed902f");
          break;
        case "bf8036b291428535b986c757fda3e627":
          terrainDesc.worldHeightMapGuid = new Colossal.Hash128("4ea9a2ffbdf3f2e5eaca4b25864d01cd");
          break;
        case "e5740c2468c1bc85fa21203c46ac140d":
          terrainDesc.worldHeightMapGuid = new Colossal.Hash128("22b3607ae209c6a568e68664db333db3");
          break;
        case "f9612145c1e3be953a425a1a50ae2331":
          terrainDesc.worldHeightMapGuid = new Colossal.Hash128("5924ad730abd94854821258372d4fb51");
          break;
        case "fc36d01de91f7f2558871a788caca83e":
          terrainDesc.worldHeightMapGuid = new Colossal.Hash128("b18abfca21d528f549bf2ca158497023");
          break;
      }
      return Colossal.IO.AssetDatabase.AssetDatabase.global.GetAsset<TextureAsset>(terrainDesc.worldHeightMapGuid);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
      if (reader.context.version >= Game.Version.terrainGuidToHash)
      {
        Colossal.Hash128 hash128;
        reader.Read(out hash128);
        this.mapReference = hash128;
      }
      else
      {
        string input;
        reader.Read(out input);
        this.mapReference = (Colossal.Hash128) Guid.Parse(input);
      }
      if (!(reader.context.version >= Game.Version.terrainInSaves))
        throw new NotSupportedException(string.Format("Saves prior to {0} are no longer supported", (object) Game.Version.terrainInSaves));
      TextureAsset textureAsset = (TextureAsset) null;
      Texture2D worldMap;
      if (reader.context.version >= Game.Version.worldmapInSaves)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        worldMap = TerrainSystem.DeserializeHeightmap<TReader>(reader, "LoadedWorldHeightMap", ref this.m_CPUHeights, true);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        textureAsset = this.LegacyLoadWorldMap();
        worldMap = textureAsset?.Load(0) as Texture2D;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      Texture2D inMap = TerrainSystem.DeserializeHeightmap<TReader>(reader, "LoadedHeightmap", ref this.m_CPUHeights, false);
      float2 heightScaleOffset;
      reader.Read(out heightScaleOffset);
      float2 inMapCorner;
      reader.Read(out inMapCorner);
      float2 inMapSize;
      reader.Read(out inMapSize);
      float2 inWorldCorner;
      reader.Read(out inWorldCorner);
      float2 inWorldSize;
      reader.Read(out inWorldSize);
      float2 inWorldHeightMinMax;
      reader.Read(out inWorldHeightMinMax);
      // ISSUE: reference to a compiler-generated method
      this.InitializeTerrainData(inMap, worldMap, heightScaleOffset, inMapCorner, inMapSize, inWorldCorner, inWorldSize, inWorldHeightMinMax);
      if ((AssetData) textureAsset != (IAssetData) this.worldMapAsset)
        this.worldMapAsset?.Unload(false);
      this.worldMapAsset = textureAsset;
      UnityEngine.Object.Destroy((UnityEngine.Object) inMap);
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
      this.mapReference = new Colossal.Hash128();
      // ISSUE: reference to a compiler-generated method
      this.LoadTerrain();
    }

    public void Clear()
    {
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((UnityEngine.Object) this.m_Heightmap);
      this.mapReference = new Colossal.Hash128();
    }

    private void LoadTerrain()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.InitializeTerrainData((Texture2D) null, (Texture2D) null, TerrainSystem.kDefaultHeightScaleOffset, TerrainSystem.kDefaultMapOffset, TerrainSystem.kDefaultMapSize, TerrainSystem.kDefaultWorldOffset, TerrainSystem.kDefaultWorldSize, float2.zero);
    }

    private void InitializeTerrainData(
      Texture2D inMap,
      Texture2D worldMap,
      float2 heightScaleOffset,
      float2 inMapCorner,
      float2 inMapSize,
      float2 inWorldCorner,
      float2 inWorldSize,
      float2 inWorldHeightMinMax)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      Texture2D map = (UnityEngine.Object) inMap != (UnityEngine.Object) null ? inMap : this.CreateDefaultHeightmap((UnityEngine.Object) worldMap != (UnityEngine.Object) null ? worldMap.width : TerrainSystem.kDefaultHeightmapWidth, (UnityEngine.Object) worldMap != (UnityEngine.Object) null ? worldMap.height : TerrainSystem.kDefaultHeightmapHeight);
      // ISSUE: reference to a compiler-generated method
      this.SetHeightmap(map);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.SetWorldHeightmap(worldMap, this.m_ToolSystem.actionMode.IsEditor());
      // ISSUE: reference to a compiler-generated method
      this.FinalizeTerrainData(map, worldMap, heightScaleOffset, inMapCorner, inMapSize, inWorldCorner, inWorldSize, inWorldHeightMinMax);
      if (!((UnityEngine.Object) map != (UnityEngine.Object) inMap))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) map);
    }

    public void ReplaceHeightmap(Texture2D inMap)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      Texture2D textureRGBA64 = (UnityEngine.Object) inMap != (UnityEngine.Object) null ? inMap : this.CreateDefaultHeightmap((UnityEngine.Object) this.worldHeightmap != (UnityEngine.Object) null ? this.worldHeightmap.width : TerrainSystem.kDefaultHeightmapWidth, (UnityEngine.Object) this.worldHeightmap != (UnityEngine.Object) null ? this.worldHeightmap.height : TerrainSystem.kDefaultHeightmapHeight);
      // ISSUE: reference to a compiler-generated method
      Texture2D r16 = TerrainSystem.ToR16(textureRGBA64);
      // ISSUE: reference to a compiler-generated method
      this.SetHeightmap(r16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.FinalizeTerrainData(r16, (Texture2D) null, this.heightScaleOffset, TerrainSystem.kDefaultMapOffset, TerrainSystem.kDefaultMapSize, TerrainSystem.kDefaultWorldOffset, TerrainSystem.kDefaultWorldSize, this.worldHeightMinMax);
      if ((UnityEngine.Object) r16 != (UnityEngine.Object) textureRGBA64)
        UnityEngine.Object.Destroy((UnityEngine.Object) r16);
      if (!((UnityEngine.Object) textureRGBA64 != (UnityEngine.Object) inMap))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) textureRGBA64);
    }

    public void ReplaceWorldHeightmap(Texture2D inMap)
    {
      // ISSUE: reference to a compiler-generated method
      Texture2D r16 = TerrainSystem.ToR16(inMap);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.SetWorldHeightmap(r16, this.m_ToolSystem.actionMode.IsEditor());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.FinalizeTerrainData((Texture2D) null, r16, this.heightScaleOffset, TerrainSystem.kDefaultMapOffset, TerrainSystem.kDefaultMapSize, TerrainSystem.kDefaultWorldOffset, TerrainSystem.kDefaultWorldSize, float2.zero);
      if (!((UnityEngine.Object) r16 != (UnityEngine.Object) inMap) || !((UnityEngine.Object) r16 != (UnityEngine.Object) this.worldHeightmap))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) r16);
    }

    public void SetTerrainProperties(float2 heightScaleOffset)
    {
      // ISSUE: reference to a compiler-generated method
      this.FinalizeTerrainData((Texture2D) null, (Texture2D) null, heightScaleOffset, this.playableOffset, this.playableArea, this.worldOffset, this.worldSize, this.worldHeightMinMax);
    }

    private void SetHeightmap(Texture2D map)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_Heightmap == (UnityEngine.Object) null || this.m_Heightmap.width != map.width || this.m_Heightmap.height != map.height)
      {
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) this.m_Heightmap != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Heightmap.Release();
          // ISSUE: reference to a compiler-generated field
          UnityEngine.Object.Destroy((UnityEngine.Object) this.m_Heightmap);
        }
        RenderTexture renderTexture = new RenderTexture(map.width, map.height, 0, GraphicsFormat.R16_UNorm);
        renderTexture.hideFlags = HideFlags.HideAndDontSave;
        renderTexture.enableRandomWrite = true;
        renderTexture.name = "TerrainHeights";
        renderTexture.filterMode = FilterMode.Bilinear;
        renderTexture.wrapMode = TextureWrapMode.Clamp;
        // ISSUE: reference to a compiler-generated field
        this.m_Heightmap = renderTexture;
        // ISSUE: reference to a compiler-generated field
        this.m_Heightmap.Create();
      }
      // ISSUE: reference to a compiler-generated field
      Graphics.CopyTexture((Texture) map, (Texture) this.m_Heightmap);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!((UnityEngine.Object) this.worldHeightmap != (UnityEngine.Object) null) || this.worldHeightmap.width == this.m_Heightmap.width && this.worldHeightmap.height == this.m_Heightmap.height)
        return;
      // ISSUE: reference to a compiler-generated method
      this.DestroyWorldMap();
    }

    private void SetWorldHeightmap(Texture2D map, bool isEditor)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) map == (UnityEngine.Object) null || map.width != this.m_Heightmap.width || map.height != this.m_Heightmap.height)
      {
        // ISSUE: reference to a compiler-generated method
        this.DestroyWorldMap();
      }
      else if (isEditor)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) this.m_WorldMapEditable == (UnityEngine.Object) null || (UnityEngine.Object) this.worldHeightmap != (UnityEngine.Object) this.m_WorldMapEditable || this.m_WorldMapEditable.width != map.width || this.m_WorldMapEditable.height != map.height)
        {
          // ISSUE: reference to a compiler-generated method
          this.DestroyWorldMap();
          RenderTexture renderTexture = new RenderTexture(map.width, map.height, 0, GraphicsFormat.R16_UNorm);
          renderTexture.hideFlags = HideFlags.HideAndDontSave;
          renderTexture.enableRandomWrite = true;
          renderTexture.name = "TerrainWorldHeights";
          renderTexture.filterMode = FilterMode.Bilinear;
          renderTexture.wrapMode = TextureWrapMode.Clamp;
          // ISSUE: reference to a compiler-generated field
          this.m_WorldMapEditable = renderTexture;
          // ISSUE: reference to a compiler-generated field
          this.m_WorldMapEditable.Create();
          // ISSUE: reference to a compiler-generated field
          this.worldHeightmap = (Texture) this.m_WorldMapEditable;
        }
        // ISSUE: reference to a compiler-generated field
        Graphics.CopyTexture((Texture) map, (Texture) this.m_WorldMapEditable);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) map != (UnityEngine.Object) this.worldHeightmap && ((UnityEngine.Object) this.m_WorldMapEditable != (UnityEngine.Object) null || (UnityEngine.Object) this.worldHeightmap != (UnityEngine.Object) null))
        {
          // ISSUE: reference to a compiler-generated method
          this.DestroyWorldMap();
        }
        this.worldHeightmap = (Texture) map;
      }
    }

    private void FinalizeTerrainData(
      Texture2D map,
      Texture2D worldMap,
      float2 heightScaleOffset,
      float2 inMapCorner,
      float2 inMapSize,
      float2 inWorldCorner,
      float2 inWorldSize,
      float2 inWorldHeightMinMax)
    {
      this.heightScaleOffset = heightScaleOffset;
      if (math.all(inWorldSize == inMapSize) || (UnityEngine.Object) this.worldHeightmap == (UnityEngine.Object) null)
      {
        TerrainSystem.baseLod = 0;
        this.playableArea = inMapSize;
        this.worldSize = inMapSize;
        this.playableOffset = inMapCorner;
        this.worldOffset = inMapCorner;
      }
      else
      {
        TerrainSystem.baseLod = 1;
        this.playableArea = inMapSize;
        this.worldSize = inWorldSize;
        this.playableOffset = inMapCorner;
        this.worldOffset = inWorldCorner;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_NewMap = true;
      // ISSUE: reference to a compiler-generated field
      this.m_NewMapThisFrame = true;
      // ISSUE: reference to a compiler-generated field
      this.m_CascadeReset = true;
      this.worldHeightMinMax = inWorldHeightMinMax;
      // ISSUE: reference to a compiler-generated field
      this.m_WorldOffsetScale = (Vector4) new float4((this.playableOffset - this.worldOffset) / this.worldSize, this.playableArea / this.worldSize);
      float3 float3 = new float3(this.playableArea.x, heightScaleOffset.x, this.playableArea.y);
      float3 xyz1 = 1f / float3;
      float3 xyz2 = -this.positionOffset;
      // ISSUE: reference to a compiler-generated field
      this.m_MapOffsetScale = new Vector4(-this.positionOffset.x, -this.positionOffset.z, 1f / float3.x, 1f / float3.z);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_HeightmapCascade == (UnityEngine.Object) null || this.m_HeightmapCascade.width != this.heightmap.width || this.m_HeightmapCascade.height != this.heightmap.height)
      {
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) this.m_HeightmapCascade != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_HeightmapCascade.Release();
          // ISSUE: reference to a compiler-generated field
          UnityEngine.Object.Destroy((UnityEngine.Object) this.m_HeightmapCascade);
          // ISSUE: reference to a compiler-generated field
          this.m_HeightmapCascade = (RenderTexture) null;
        }
        RenderTexture renderTexture = new RenderTexture(this.heightmap.width, this.heightmap.height, 0, GraphicsFormat.R16_UNorm);
        renderTexture.hideFlags = HideFlags.HideAndDontSave;
        renderTexture.enableRandomWrite = false;
        renderTexture.name = "TerrainHeightsCascade";
        renderTexture.filterMode = FilterMode.Bilinear;
        renderTexture.wrapMode = TextureWrapMode.Clamp;
        renderTexture.dimension = TextureDimension.Tex2DArray;
        renderTexture.volumeDepth = 4;
        // ISSUE: reference to a compiler-generated field
        this.m_HeightmapCascade = renderTexture;
        // ISSUE: reference to a compiler-generated field
        this.m_HeightmapCascade.Create();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_HeightmapDepth == (UnityEngine.Object) null || this.m_HeightmapDepth.width != this.heightmap.width || this.m_HeightmapDepth.height != this.heightmap.height)
      {
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) this.m_HeightmapDepth != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_HeightmapDepth.Release();
          // ISSUE: reference to a compiler-generated field
          UnityEngine.Object.Destroy((UnityEngine.Object) this.m_HeightmapDepth);
          // ISSUE: reference to a compiler-generated field
          this.m_HeightmapDepth = (RenderTexture) null;
        }
        RenderTexture renderTexture = new RenderTexture(this.heightmap.width, this.heightmap.height, 16, RenderTextureFormat.Depth, RenderTextureReadWrite.Linear);
        renderTexture.name = "HeightmapDepth";
        // ISSUE: reference to a compiler-generated field
        this.m_HeightmapDepth = renderTexture;
        // ISSUE: reference to a compiler-generated field
        this.m_HeightmapDepth.Create();
      }
      if ((UnityEngine.Object) map != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        Graphics.CopyTexture((Texture) map, 0, 0, (Texture) this.m_HeightmapCascade, TerrainSystem.baseLod, 0);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_CascadeRanges = new float4[4];
      // ISSUE: reference to a compiler-generated field
      this.m_ShaderCascadeRanges = new Vector4[4];
      for (int index = 0; index < 4; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CascadeRanges[index] = new float4(0.0f, 0.0f, 0.0f, 0.0f);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_CascadeRanges[TerrainSystem.baseLod] = new float4(this.playableOffset, this.playableOffset + this.playableArea);
      if (TerrainSystem.baseLod > 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CascadeRanges[0] = new float4(this.worldOffset, this.worldOffset + this.worldSize);
        if ((UnityEngine.Object) worldMap != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          Graphics.CopyTexture((Texture) worldMap, 0, 0, (Texture) this.m_HeightmapCascade, 0, 0);
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateArea = new float4(this.m_CascadeRanges[TerrainSystem.baseLod]);
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalTexture("colossal_TerrainTexture", (Texture) this.m_Heightmap);
      Shader.SetGlobalVector("colossal_TerrainScale", (Vector4) new float4(xyz1, 0.0f));
      Shader.SetGlobalVector("colossal_TerrainOffset", (Vector4) new float4(xyz2, 0.0f));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalVector("colossal_TerrainCascadeLimit", (Vector4) new float4(0.5f / (float) this.m_HeightmapCascade.width, 0.5f / (float) this.m_HeightmapCascade.height, 0.0f, 0.0f));
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalTexture("colossal_TerrainTextureArray", (Texture) this.m_HeightmapCascade);
      Shader.SetGlobalInt("colossal_TerrainTextureArrayBaseLod", TerrainSystem.baseLod);
      if ((UnityEngine.Object) map != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CPUHeightReaders.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_CPUHeightReaders = new JobHandle();
        // ISSUE: reference to a compiler-generated method
        this.WriteCPUHeights(map.GetRawTextureData<ushort>());
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainMinMax.Init((UnityEngine.Object) this.worldHeightmap != (UnityEngine.Object) null ? 1024 : 512, (UnityEngine.Object) this.worldHeightmap != (UnityEngine.Object) null ? this.worldHeightmap.width : this.m_Heightmap.width);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainMinMax.UpdateMap(this, (Texture) this.m_Heightmap, this.worldHeightmap);
    }

    private void DestroyWorldMap()
    {
      if ((UnityEngine.Object) this.worldHeightmap != (UnityEngine.Object) null)
      {
        if (this.worldHeightmap is RenderTexture worldHeightmap)
          worldHeightmap.Release();
        UnityEngine.Object.Destroy((UnityEngine.Object) this.worldHeightmap);
        this.worldHeightmap = (Texture) null;
      }
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_WorldMapEditable != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_WorldMapEditable.Release();
        // ISSUE: reference to a compiler-generated field
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_WorldMapEditable);
        // ISSUE: reference to a compiler-generated field
        this.m_WorldMapEditable = (RenderTexture) null;
      }
      if (!((AssetData) this.worldMapAsset != (IAssetData) null))
        return;
      this.worldMapAsset.Unload(false);
      this.worldMapAsset = (TextureAsset) null;
    }

    private Texture2D CreateDefaultHeightmap(int width, int height)
    {
      Texture2D targetHeightmap = new Texture2D(width, height, GraphicsFormat.R16_UNorm, TextureCreationFlags.DontInitializePixels | TextureCreationFlags.DontUploadUponCreate);
      targetHeightmap.hideFlags = HideFlags.HideAndDontSave;
      targetHeightmap.name = "DefaultHeightmap";
      targetHeightmap.filterMode = FilterMode.Bilinear;
      targetHeightmap.wrapMode = TextureWrapMode.Clamp;
      // ISSUE: reference to a compiler-generated method
      TerrainSystem.SetDefaultHeights(targetHeightmap);
      return targetHeightmap;
    }

    private static void SetDefaultHeights(Texture2D targetHeightmap)
    {
      NativeArray<ushort> rawTextureData = targetHeightmap.GetRawTextureData<ushort>();
      ushort num = 8191;
      for (int index = 0; index < rawTextureData.Length; ++index)
        rawTextureData[index] = num;
      targetHeightmap.Apply(false, false);
    }

    private static Texture2D ToR16(Texture2D textureRGBA64)
    {
      if (!((UnityEngine.Object) textureRGBA64 != (UnityEngine.Object) null) || textureRGBA64.graphicsFormat == GraphicsFormat.R16_UNorm)
        return textureRGBA64;
      NativeArray<ushort> rawTextureData = textureRGBA64.GetRawTextureData<ushort>();
      NativeArray<ushort> data = new NativeArray<ushort>(textureRGBA64.width * textureRGBA64.height, Allocator.Temp);
      for (int index = 0; index < data.Length; ++index)
        data[index] = rawTextureData[index * 4];
      Texture2D r16 = new Texture2D(textureRGBA64.width, textureRGBA64.height, GraphicsFormat.R16_UNorm, TextureCreationFlags.DontInitializePixels | TextureCreationFlags.DontUploadUponCreate);
      r16.SetPixelData<ushort>(data, 0);
      r16.Apply();
      return r16;
    }

    public static bool IsValidHeightmapFormat(Texture2D tex)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (tex.width != TerrainSystem.kDefaultHeightmapWidth || tex.height != TerrainSystem.kDefaultHeightmapHeight)
        return false;
      return tex.graphicsFormat == GraphicsFormat.R16_UNorm || tex.graphicsFormat == GraphicsFormat.R16G16B16A16_UNorm;
    }

    private void SaveBitmap(NativeArray<ushort> buffer, int width, int height)
    {
      using (System.IO.BinaryWriter binaryWriter = new System.IO.BinaryWriter((Stream) File.OpenWrite("heightmapResult.raw")))
      {
        for (int index1 = 0; index1 < height; ++index1)
        {
          for (int index2 = 0; index2 < width; ++index2)
            binaryWriter.Write(buffer[index2 + index1 * width]);
        }
      }
    }

    private void EnsureCPUHeights(int length)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_CPUHeights.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_CPUHeights.Length == length)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CPUHeights.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_CPUHeights = new NativeArray<ushort>(length, Allocator.Persistent);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CPUHeights = new NativeArray<ushort>(length, Allocator.Persistent);
      }
    }

    private void WriteCPUHeights(NativeArray<ushort> buffer)
    {
      // ISSUE: reference to a compiler-generated method
      this.EnsureCPUHeights(buffer.Length);
      // ISSUE: reference to a compiler-generated field
      this.m_CPUHeights.CopyFrom(buffer);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_GroundHeightSystem.AfterReadHeights();
    }

    private void WriteCPUHeights(NativeArray<ushort> buffer, int4 offsets)
    {
      for (int index = 0; index < offsets.w; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        int dstIndex = (offsets.y + index) * this.m_HeightmapCascade.width + offsets.x;
        // ISSUE: reference to a compiler-generated field
        NativeArray<ushort>.Copy(buffer, index * offsets.z, this.m_CPUHeights, dstIndex, offsets.z);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_GroundHeightSystem.AfterReadHeights();
    }

    private void UpdateGPUReadback()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainMinMax.Update();
      // ISSUE: reference to a compiler-generated field
      if (this.m_AsyncGPUReadback.isPending)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_AsyncGPUReadback.hasError)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_AsyncGPUReadback.done)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.WriteCPUHeights(this.m_AsyncGPUReadback.GetData<ushort>(), this.m_LastRequest);
            // ISSUE: reference to a compiler-generated field
            if (this.m_UpdateOutOfDate)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_UpdateOutOfDate = false;
              // ISSUE: reference to a compiler-generated method
              this.OnHeightsChanged();
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_HeightMapChanged = false;
            }
            // ISSUE: reference to a compiler-generated field
            this.m_FailCount = 0;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_AsyncGPUReadback.IncrementFrame();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (++this.m_FailCount < 10)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_GroundHeightSystem.BeforeReadHeights();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_AsyncGPUReadback.Request((Texture) this.m_HeightmapCascade, 0, this.m_LastRequest.x, this.m_LastRequest.z, this.m_LastRequest.y, this.m_LastRequest.w, TerrainSystem.baseLod, 1);
          }
          else
          {
            COSystemBase.baseLog.Error((object) "m_AsyncGPUReadback.hasError");
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_LastRequest = new int4(0, 0, this.m_HeightmapCascade.width, this.m_HeightmapCascade.height);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_GroundHeightSystem.BeforeReadHeights();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_AsyncGPUReadback.Request((Texture) this.m_HeightmapCascade, 0, 0, this.m_HeightmapCascade.width, 0, this.m_HeightmapCascade.height, TerrainSystem.baseLod, 1);
          }
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_HeightMapChanged = false;
      }
    }

    public void TriggerAsyncChange()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateOutOfDate = this.m_AsyncGPUReadback.isPending;
      // ISSUE: reference to a compiler-generated field
      this.m_HeightMapChanged = true;
      // ISSUE: reference to a compiler-generated field
      if (this.m_UpdateOutOfDate)
        return;
      // ISSUE: reference to a compiler-generated method
      this.OnHeightsChanged();
    }

    public void HandleNewMap() => this.m_NewMap = false;

    private void OnHeightsChanged()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastRequest = this.m_LastWrite;
      // ISSUE: reference to a compiler-generated field
      this.m_LastWrite = int4.zero;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_LastRequest.z == 0 || this.m_LastRequest.w == 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_LastRequest = new int4(0, 0, this.m_HeightmapCascade.width, this.m_HeightmapCascade.height);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_GroundHeightSystem.BeforeReadHeights();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AsyncGPUReadback.Request((Texture) this.m_HeightmapCascade, 0, this.m_LastRequest.x, this.m_LastRequest.z, this.m_LastRequest.y, this.m_LastRequest.w, TerrainSystem.baseLod, 1);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_NewMapThisFrame = this.m_NewMap;
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_Heightmap == (UnityEngine.Object) null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_CPUHeightReaders.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_CPUHeightReaders = new JobHandle();
      if (!this.freezeCascadeUpdates)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateCascades(this.m_Loaded);
        // ISSUE: reference to a compiler-generated field
        this.m_Loaded = false;
      }
      // ISSUE: reference to a compiler-generated method
      this.UpdateGPUReadback();
      // ISSUE: reference to a compiler-generated method
      this.UpdateGPUTerrain();
    }

    private void UpdateGPUTerrain()
    {
      TerrainSurface validSurface = TerrainSurface.GetValidSurface();
      if (!((UnityEngine.Object) validSurface != (UnityEngine.Object) null))
        return;
      validSurface.UsesCascade = true;
      float4x4 areas;
      float4 ranges;
      float4 size;
      // ISSUE: reference to a compiler-generated method
      this.GetCascadeInfo(out int _, out validSurface.BaseLOD, out areas, out ranges, out size);
      validSurface.CascadeArea = (Matrix4x4) areas;
      validSurface.CascadeRanges = (Vector4) ranges;
      validSurface.CascadeSizes = (Vector4) size;
      // ISSUE: reference to a compiler-generated field
      validSurface.CascadeTexture = (Texture) this.m_HeightmapCascade;
      validSurface.TerrainHeightOffset = this.heightScaleOffset.y;
      validSurface.TerrainHeightScale = this.heightScaleOffset.x;
      if (validSurface.RenderClipAreas != null)
        return;
      validSurface.RenderClipAreas = (Action<RenderGraphContext, HDCamera>) ((ctx, hdCamera) =>
      {
        Camera camera = hdCamera.camera;
        bool flag = false;
        float w = math.tan(math.radians(camera.fieldOfView) * 0.5f) * (1f / 500f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ClipMaterial.SetBuffer(TerrainSystem.ShaderID._RoadData, this.clipMapBuffer);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ClipMaterial.SetVector(TerrainSystem.ShaderID._ClipOffset, (Vector4) new float4((float3) camera.transform.position, w));
        if (this.clipMapInstances > 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ctx.cmd.DrawMeshInstancedProcedural(this.m_ClipMesh, 0, this.m_ClipMaterial, 0, this.clipMapInstances);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_RenderingSystem.hideOverlay || this.m_ToolSystem.activeTool == null || (this.m_ToolSystem.activeTool.requireAreas & AreaTypeMask.Surfaces) == AreaTypeMask.None)
        {
          // ISSUE: reference to a compiler-generated field
          ctx.cmd.DrawMesh(this.areaClipMesh, Matrix4x4.identity, this.m_ClipMaterial, 0, 2);
        }
        // ISSUE: reference to a compiler-generated field
        ctx.cmd.DrawProcedural(Matrix4x4.identity, this.m_ClipMaterial, flag ? 4 : 3, MeshTopology.Triangles, 3, 1);
      });
    }

    private void ApplyToTerrain(
      RenderTexture target,
      RenderTexture source,
      float delta,
      TerraformingType type,
      Bounds2 area,
      Brush brush,
      Texture texture,
      bool worldMap)
    {
      if ((UnityEngine.Object) target == (UnityEngine.Object) null || !target.IsCreated())
        return;
      if ((double) delta == 0.0 || (double) brush.m_Strength == 0.0)
      {
        // ISSUE: reference to a compiler-generated field
        if (worldMap && (UnityEngine.Object) source != (UnityEngine.Object) null && this.m_LastWorldPreviewWrite.z != 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.Clear();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.CopyTexture((RenderTargetIdentifier) (Texture) source, 0, 0, this.m_LastWorldPreviewWrite.x, this.m_LastWorldPreviewWrite.y, this.m_LastWorldPreviewWrite.z, this.m_LastWorldPreviewWrite.w, (RenderTargetIdentifier) (Texture) target, 0, 0, this.m_LastWorldPreviewWrite.x, this.m_LastWorldPreviewWrite.y);
          // ISSUE: reference to a compiler-generated field
          Graphics.ExecuteCommandBuffer(this.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated field
          this.m_LastWorldPreviewWrite = int4.zero;
        }
        // ISSUE: reference to a compiler-generated field
        if (worldMap || !((UnityEngine.Object) source != (UnityEngine.Object) null) || this.m_LastPreviewWrite.z == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.Clear();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.CopyTexture((RenderTargetIdentifier) (Texture) source, 0, 0, this.m_LastPreviewWrite.x, this.m_LastPreviewWrite.y, this.m_LastPreviewWrite.z, this.m_LastPreviewWrite.w, (RenderTargetIdentifier) (Texture) target, 0, 0, this.m_LastPreviewWrite.x, this.m_LastPreviewWrite.y);
        // ISSUE: reference to a compiler-generated field
        Graphics.ExecuteCommandBuffer(this.m_CommandBuffer);
        // ISSUE: reference to a compiler-generated field
        this.m_LastPreviewWrite = int4.zero;
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        float x1 = delta * brush.m_Strength * this.GetTerrainAdjustmentSpeed(type) / this.heightScaleOffset.x;
        float2 float2_1 = worldMap ? this.worldSize : this.playableArea;
        float2 float2_2 = worldMap ? this.worldOffset : this.playableOffset;
        float x2 = math.max(float2_1.x, float2_1.y);
        float2 float2_3 = (brush.m_Position.xz - float2_2) / float2_1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_GroundHeightSystem.GetUpdateBuffer().Add(in area);
        // ISSUE: reference to a compiler-generated field
        if ((double) math.lengthsq(this.m_UpdateArea) > 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateArea.xy = math.min(this.m_UpdateArea.xy, area.min);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateArea.zw = math.max(this.m_UpdateArea.zw, area.max);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateArea = new float4(area.min, area.max);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TerrainChange)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TerrainChange = true;
          // ISSUE: reference to a compiler-generated field
          this.m_TerrainChangeArea = new float4(area.min, area.max);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_TerrainChangeArea.xy = math.min(this.m_TerrainChangeArea.xy, area.min);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_TerrainChangeArea.zw = math.max(this.m_TerrainChangeArea.zw, area.max);
        }
        area.min -= float2_2;
        area.max -= float2_2;
        area.min /= float2_1;
        area.max /= float2_1;
        int4 int4_1 = new int4((int) math.max(math.floor(area.min.x * (float) target.width), 0.0f), (int) math.max(math.floor(area.min.y * (float) target.height), 0.0f), (int) math.min(math.ceil(area.max.x * (float) target.width), (float) (target.width - 1)), (int) math.min(math.ceil(area.max.y * (float) target.height), (float) (target.height - 1)));
        Vector4 val1 = new Vector4(float2_3.x, float2_3.y, (float) ((double) brush.m_Size / (double) x2 * 0.5), brush.m_Angle);
        int num1 = int4_1.z - int4_1.x + 1;
        int num2 = int4_1.w - int4_1.y + 1;
        int threadGroupsX = (num1 + 7) / 8;
        int threadGroupsY = (num2 + 7) / 8;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.Clear();
        int4 int4_2 = new int4(math.max(int4_1.x - 2, 0), math.max(int4_1.y - 2, 0), num1 + 4, num2 + 4);
        if (int4_2.x + int4_2.z < 0 || int4_2.x > target.width || int4_2.y + int4_2.w < 0 || int4_2.y > target.height || num1 <= 0 || num2 <= 0)
          return;
        if (int4_2.x + int4_2.z > target.width)
          int4_2.z = target.width - int4_2.x;
        if (int4_2.y + int4_2.w > target.height)
          int4_2.w = target.height - int4_2.y;
        if ((UnityEngine.Object) source != (UnityEngine.Object) null)
        {
          if (worldMap)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_LastWorldPreviewWrite.z == 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.CopyTexture((RenderTargetIdentifier) (Texture) source, (RenderTargetIdentifier) (Texture) target);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.CopyTexture((RenderTargetIdentifier) (Texture) source, 0, 0, this.m_LastWorldPreviewWrite.x, this.m_LastWorldPreviewWrite.y, this.m_LastWorldPreviewWrite.z, this.m_LastWorldPreviewWrite.w, (RenderTargetIdentifier) (Texture) target, 0, 0, this.m_LastWorldPreviewWrite.x, this.m_LastWorldPreviewWrite.y);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_LastWorldPreviewWrite = int4_2;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_LastPreviewWrite.z == 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.CopyTexture((RenderTargetIdentifier) (Texture) source, (RenderTargetIdentifier) (Texture) target);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.CopyTexture((RenderTargetIdentifier) (Texture) source, 0, 0, this.m_LastPreviewWrite.x, this.m_LastPreviewWrite.y, this.m_LastPreviewWrite.z, this.m_LastPreviewWrite.w, (RenderTargetIdentifier) (Texture) target, 0, 0, this.m_LastPreviewWrite.x, this.m_LastPreviewWrite.y);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float4 float4_1 = new float4((float) this.m_LastPreviewWrite.x * (1f / (float) target.width), (float) this.m_LastPreviewWrite.y * (1f / (float) target.width), (float) this.m_LastPreviewWrite.z * (1f / (float) target.width), (float) this.m_LastPreviewWrite.w * (1f / (float) target.width));
              float4 float4_2 = new float4(float2_2 + float4_1.xy * float2_1, float2_2 + (float4_1.xy + float4_1.zw) * float2_1);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_UpdateArea.xy = math.min(this.m_UpdateArea.xy, float4_2.xy);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_UpdateArea.zw = math.max(this.m_UpdateArea.zw, float4_2.zw);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_LastPreviewWrite = int4_2;
          }
        }
        else if (worldMap)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_LastWorldWrite.z == 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LastWorldWrite = int4_2;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int2 int2_1 = new int2(math.min(this.m_LastWorldWrite.x, int4_2.x), math.min(this.m_LastWorldWrite.y, int4_2.y));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int2 int2_2 = new int2(math.max(this.m_LastWorldWrite.x + this.m_LastWorldWrite.z, int4_2.x + int4_2.z), math.max(this.m_LastWorldWrite.y + this.m_LastWorldWrite.w, int4_2.y + int4_2.w));
            // ISSUE: reference to a compiler-generated field
            this.m_LastWorldWrite.xy = int2_1;
            // ISSUE: reference to a compiler-generated field
            this.m_LastWorldWrite.zw = int2_2 - int2_1;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_LastWrite.z == 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LastWrite = int4_2;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int2 int2_3 = new int2(math.min(this.m_LastWrite.x, int4_2.x), math.min(this.m_LastWrite.y, int4_2.y));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int2 int2_4 = new int2(math.max(this.m_LastWrite.x + this.m_LastWrite.z, int4_2.x + int4_2.z), math.max(this.m_LastWrite.y + this.m_LastWrite.w, int4_2.y + int4_2.w));
            // ISSUE: reference to a compiler-generated field
            this.m_LastWrite.xy = int2_3;
            // ISSUE: reference to a compiler-generated field
            this.m_LastWrite.zw = int2_4 - int2_3;
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeVectorParam(this.m_AdjustTerrainCS, TerrainSystem.ShaderID._CenterSizeRotation, val1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeVectorParam(this.m_AdjustTerrainCS, TerrainSystem.ShaderID._Dims, new Vector4(x2, (float) target.width, (float) target.height, 0.0f));
        Vector4 val2 = new Vector4(x1, 0.0f, 0.0f, 0.0f);
        Vector4 val3 = Vector4.zero;
        int kernelIndex;
        switch (type)
        {
          case TerraformingType.Shift:
            // ISSUE: reference to a compiler-generated field
            kernelIndex = this.m_ShiftTerrainKernal;
            break;
          case TerraformingType.Level:
            // ISSUE: reference to a compiler-generated field
            kernelIndex = this.m_LevelTerrainKernal;
            val2.y = (brush.m_Target.y - this.positionOffset.y) / this.heightScaleOffset.x;
            break;
          case TerraformingType.Soften:
            RenderTextureDescriptor desc = new RenderTextureDescriptor()
            {
              autoGenerateMips = false,
              bindMS = false,
              depthBufferBits = 0,
              dimension = TextureDimension.Tex2D,
              enableRandomWrite = true,
              graphicsFormat = GraphicsFormat.R16_UNorm,
              memoryless = RenderTextureMemoryless.None,
              height = num2 + 8,
              width = num1 + 8,
              volumeDepth = 1,
              mipCount = 1,
              msaaSamples = 1,
              sRGB = false,
              useDynamicScale = false,
              useMipMap = false
            };
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.GetTemporaryRT(TerrainSystem.ShaderID._AvgTerrainHeightsTemp, desc);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.GetTemporaryRT(TerrainSystem.ShaderID._BlurTempHorz, desc);
            // ISSUE: reference to a compiler-generated field
            kernelIndex = this.m_SmoothTerrainKernal;
            val2.y = (float) desc.width;
            val2.z = (float) desc.height;
            val3.x = 4f;
            val3.y = 4f;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComputeTextureParam(this.m_AdjustTerrainCS, this.m_BlurHorzKernal, TerrainSystem.ShaderID._Heightmap, (RenderTargetIdentifier) (Texture) target);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComputeVectorParam(this.m_AdjustTerrainCS, TerrainSystem.ShaderID._BrushData, val2);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComputeVectorParam(this.m_AdjustTerrainCS, TerrainSystem.ShaderID._Range, new Vector4((float) (int4_1.x - 4), (float) (int4_1.y - 4), (float) (int4_1.z + 4), (float) (int4_1.w + 4)));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.DispatchCompute(this.m_AdjustTerrainCS, this.m_BlurHorzKernal, (num1 + 15) / 8, num2 + 8, 1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.DispatchCompute(this.m_AdjustTerrainCS, this.m_BlurVertKernal, num1 + 8, (num2 + 15) / 8, 1);
            break;
          case TerraformingType.Slope:
            // ISSUE: reference to a compiler-generated field
            kernelIndex = this.m_SlopeTerrainKernal;
            float3 float3 = brush.m_Target - brush.m_Start;
            val2.y = (brush.m_Target.y - this.positionOffset.y) / this.heightScaleOffset.x;
            val2.z = (brush.m_Start.y - this.positionOffset.y) / this.heightScaleOffset.x;
            val2.w = float3.y / this.heightScaleOffset.x;
            float4 zero = float4.zero with
            {
              xy = math.normalize(float3.xz)
            };
            zero.z = -math.dot((brush.m_Start.xz - float2_2) / float2_1, zero.xy);
            zero.w = math.length(float3.xz) / x2;
            val3 = (Vector4) zero;
            break;
          default:
            // ISSUE: reference to a compiler-generated field
            kernelIndex = this.m_ShiftTerrainKernal;
            break;
        }
        int num3 = 2;
        // ISSUE: reference to a compiler-generated field
        float4 val4 = !((UnityEngine.Object) this.worldHeightmap != (UnityEngine.Object) null) || this.m_ToolSystem.actionMode.IsEditor() ? new float4(-1f, -1f, (float) (target.width + 1), (float) (target.height + 1)) : new float4((float) num3, (float) num3, (float) (target.width - num3), (float) (target.height - num3));
        float val5 = 10f / this.heightScaleOffset.x;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeTextureParam(this.m_AdjustTerrainCS, kernelIndex, TerrainSystem.ShaderID._Heightmap, (RenderTargetIdentifier) (Texture) target);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeTextureParam(this.m_AdjustTerrainCS, kernelIndex, TerrainSystem.ShaderID._BrushTexture, (RenderTargetIdentifier) texture);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeTextureParam(this.m_AdjustTerrainCS, kernelIndex, TerrainSystem.ShaderID._WorldTexture, (RenderTargetIdentifier) ((UnityEngine.Object) this.worldHeightmap != (UnityEngine.Object) null ? this.worldHeightmap : (Texture) Texture2D.whiteTexture));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeTextureParam(this.m_AdjustTerrainCS, kernelIndex, TerrainSystem.ShaderID._WaterTexture, (RenderTargetIdentifier) (Texture) this.m_WaterSystem.WaterTexture);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeVectorParam(this.m_AdjustTerrainCS, TerrainSystem.ShaderID._HeightScaleOffset, (Vector4) new float4(this.heightScaleOffset.x, this.heightScaleOffset.y, 0.0f, 0.0f));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeVectorParam(this.m_AdjustTerrainCS, TerrainSystem.ShaderID._Range, new Vector4((float) int4_1.x, (float) int4_1.y, (float) int4_1.z, (float) int4_1.w));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeVectorParam(this.m_AdjustTerrainCS, TerrainSystem.ShaderID._BrushData, val2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeVectorParam(this.m_AdjustTerrainCS, TerrainSystem.ShaderID._BrushData2, val3);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeVectorParam(this.m_AdjustTerrainCS, TerrainSystem.ShaderID._ClampArea, (Vector4) val4);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeVectorParam(this.m_AdjustTerrainCS, TerrainSystem.ShaderID._WorldOffsetScale, this.m_WorldOffsetScale);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComputeFloatParam(this.m_AdjustTerrainCS, TerrainSystem.ShaderID._EdgeMaxDifference, val5);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.DispatchCompute(this.m_AdjustTerrainCS, kernelIndex, threadGroupsX, threadGroupsY, 1);
        if (type == TerraformingType.Soften)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.ReleaseTemporaryRT(TerrainSystem.ShaderID._AvgTerrainHeightsTemp);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.ReleaseTemporaryRT(TerrainSystem.ShaderID._BlurTempHorz);
        }
        // ISSUE: reference to a compiler-generated field
        Graphics.ExecuteCommandBuffer(this.m_CommandBuffer);
      }
    }

    public void PreviewBrush(TerraformingType type, Bounds2 area, Brush brush, Texture texture)
    {
    }

    public void ApplyBrush(TerraformingType type, Bounds2 area, Brush brush, Texture texture)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.TerrainWillChangeFromBrush(area);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.ApplyToTerrain(this.m_Heightmap, (RenderTexture) null, UnityEngine.Time.unscaledDeltaTime, type, area, brush, texture, false);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.ApplyToTerrain(this.m_WorldMapEditable, (RenderTexture) null, UnityEngine.Time.unscaledDeltaTime, type, area, brush, texture, true);
      // ISSUE: reference to a compiler-generated method
      this.UpdateMinMax(brush, area);
      // ISSUE: reference to a compiler-generated method
      this.TriggerAsyncChange();
    }

    public void UpdateMinMax(Brush brush, Bounds2 area)
    {
      if ((UnityEngine.Object) this.worldHeightmap != (UnityEngine.Object) null)
      {
        area.min -= this.worldOffset;
        area.max -= this.worldOffset;
        area.min /= this.worldSize;
        area.max /= this.worldSize;
      }
      else
      {
        area.min -= this.playableOffset;
        area.max -= this.playableOffset;
        area.min /= this.playableArea;
        area.max /= this.playableArea;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int4 area1 = new int4((int) math.max(math.floor(area.min.x * (float) this.m_Heightmap.width) - 1f, 0.0f), (int) math.max(math.floor(area.min.y * (float) this.m_Heightmap.height) - 1f, 0.0f), (int) math.min(math.ceil(area.max.x * (float) this.m_Heightmap.width) + 1f, (float) (this.m_Heightmap.width - 1)), (int) math.min(math.ceil(area.max.y * (float) this.m_Heightmap.height) + 1f, (float) (this.m_Heightmap.height - 1)));
      area1.zw -= area1.xy;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      area1.zw = math.clamp(area1.zw, new int2(this.m_Heightmap.width / this.m_TerrainMinMax.size, this.m_Heightmap.height / this.m_TerrainMinMax.size), new int2(this.m_Heightmap.width, this.m_Heightmap.height));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainMinMax.RequestUpdate(this, (Texture) this.m_Heightmap, this.worldHeightmap, area1);
    }

    public void GetCascadeInfo(
      out int LODCount,
      out int baseLOD,
      out float4x4 areas,
      out float4 ranges,
      out float4 size)
    {
      LODCount = 4;
      baseLOD = TerrainSystem.baseLod;
      // ISSUE: reference to a compiler-generated field
      if (this.m_CascadeRanges != null)
      {
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        areas = new float4x4(this.m_CascadeRanges[0].x, this.m_CascadeRanges[0].y, this.m_CascadeRanges[0].z, this.m_CascadeRanges[0].w, this.m_CascadeRanges[1].x, this.m_CascadeRanges[1].y, this.m_CascadeRanges[1].z, this.m_CascadeRanges[1].w, this.m_CascadeRanges[2].x, this.m_CascadeRanges[2].y, this.m_CascadeRanges[2].z, this.m_CascadeRanges[2].w, this.m_CascadeRanges[3].x, this.m_CascadeRanges[3].y, this.m_CascadeRanges[3].z, this.m_CascadeRanges[3].w);
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ranges = new float4(math.min(this.m_CascadeRanges[0].z - this.m_CascadeRanges[0].x, this.m_CascadeRanges[0].w - this.m_CascadeRanges[0].y) * 0.75f, math.min(this.m_CascadeRanges[1].z - this.m_CascadeRanges[1].x, this.m_CascadeRanges[1].w - this.m_CascadeRanges[1].y) * 0.75f, math.min(this.m_CascadeRanges[2].z - this.m_CascadeRanges[2].x, this.m_CascadeRanges[2].w - this.m_CascadeRanges[2].y) * 0.75f, math.min(this.m_CascadeRanges[3].z - this.m_CascadeRanges[3].x, this.m_CascadeRanges[3].w - this.m_CascadeRanges[3].y) * 0.75f);
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        size = new float4(math.max(this.m_CascadeRanges[0].z - this.m_CascadeRanges[0].x, this.m_CascadeRanges[0].w - this.m_CascadeRanges[0].y), math.max(this.m_CascadeRanges[1].z - this.m_CascadeRanges[1].x, this.m_CascadeRanges[1].w - this.m_CascadeRanges[1].y), math.max(this.m_CascadeRanges[2].z - this.m_CascadeRanges[2].x, this.m_CascadeRanges[2].w - this.m_CascadeRanges[2].y), math.max(this.m_CascadeRanges[3].z - this.m_CascadeRanges[3].x, this.m_CascadeRanges[3].w - this.m_CascadeRanges[3].y));
      }
      else
      {
        areas = new float4x4();
        ranges = new float4();
        size = new float4();
      }
    }

    public Texture GetCascadeTexture() => (Texture) this.m_HeightmapCascade;

    private bool Overlap(ref float4 A, ref float4 B)
    {
      return (double) A.x <= (double) B.z && (double) B.x <= (double) A.z && (double) A.z >= (double) B.x && (double) B.z >= (double) A.x && (double) A.y <= (double) B.w && (double) B.y <= (double) A.w && (double) A.w >= (double) B.y && (double) B.w >= (double) A.y;
    }

    private float4 ClipViewport(float4 Viewport)
    {
      if ((double) Viewport.x < 0.0)
      {
        Viewport.z = math.max(Viewport.z + Viewport.x, 0.0f);
        Viewport.x = 0.0f;
      }
      else if ((double) Viewport.x > 1.0)
      {
        Viewport.x = 1f;
        Viewport.z = 0.0f;
      }
      if ((double) Viewport.x + (double) Viewport.z > 1.0)
        Viewport.z = math.max(1f - Viewport.x, 0.0f);
      if ((double) Viewport.y < 0.0)
      {
        Viewport.w = math.max(Viewport.w + Viewport.y, 0.0f);
        Viewport.y = 0.0f;
      }
      else if ((double) Viewport.y > 1.0)
      {
        Viewport.y = 1f;
        Viewport.w = 0.0f;
      }
      if ((double) Viewport.y + (double) Viewport.w > 1.0)
        Viewport.w = math.max(1f - Viewport.y, 0.0f);
      return Viewport;
    }

    private void UpdateCascades(bool isLoaded)
    {
      // ISSUE: reference to a compiler-generated field
      float3 position = this.m_CameraUpdateSystem.position;
      float4 float4 = new float4(0);
      // ISSUE: reference to a compiler-generated field
      float4 A = this.m_UpdateArea;
      this.heightMapRenderRequired = (double) math.lengthsq(A) > 0.0;
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateArea = float4.zero;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_RoadUpdate = this.m_CascadeReset;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AreaUpdate = this.m_CascadeReset;
      // ISSUE: reference to a compiler-generated field
      if (this.m_CascadeReset)
      {
        this.heightMapRenderRequired = true;
        // ISSUE: reference to a compiler-generated field
        A = this.m_CascadeRanges[TerrainSystem.baseLod];
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeList<Bounds2> updateBuffer = this.m_GroundHeightSystem.GetUpdateBuffer();
      // ISSUE: reference to a compiler-generated field
      bool flag1 = isLoaded || !this.m_BuildingsChanged.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (flag1 || this.m_ToolSystem.actionMode.IsEditor() && !this.m_EditorLotQuery.IsEmpty)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Game.Objects.Transform> componentTypeHandle1 = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabRef> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Updated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Updated> componentTypeHandle3 = this.__TypeHandle.__Game_Common_Updated_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<ObjectGeometryData> roComponentLookup = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
        float4 area;
        if (flag1)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_BuildingUpgradeDependencies.Complete();
          // ISSUE: reference to a compiler-generated field
          this.m_BuildingUpgradeDependencies = new JobHandle();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<ArchetypeChunk> archetypeChunkArray = (isLoaded ? this.m_BuildingGroup : this.m_BuildingsChanged).ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
          this.CompleteDependency();
          for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
          {
            NativeArray<Entity> nativeArray1 = archetypeChunkArray[index1].GetNativeArray(entityTypeHandle);
            NativeArray<Game.Objects.Transform> nativeArray2 = archetypeChunkArray[index1].GetNativeArray<Game.Objects.Transform>(ref componentTypeHandle1);
            NativeArray<PrefabRef> nativeArray3 = archetypeChunkArray[index1].GetNativeArray<PrefabRef>(ref componentTypeHandle2);
            bool flag2 = archetypeChunkArray[index1].Has<Updated>(ref componentTypeHandle3);
            if (isLoaded)
            {
              this.heightMapRenderRequired = true;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_WaterSystem.TerrainWillChange();
              // ISSUE: reference to a compiler-generated field
              A = this.m_CascadeRanges[TerrainSystem.baseLod];
              break;
            }
            for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
            {
              PrefabRef prefabRef = nativeArray3[index2];
              // ISSUE: reference to a compiler-generated method
              if (this.CalculateBuildingCullArea(nativeArray2[index2], prefabRef.m_Prefab, roComponentLookup, out area))
              {
                updateBuffer.Add(new Bounds2(area.xy, area.zw));
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_WaterSystem.TerrainWillChange();
                if (!this.heightMapRenderRequired)
                {
                  this.heightMapRenderRequired = true;
                  A = area;
                }
                else
                {
                  A.xy = math.min(A.xy, area.xy);
                  A.zw = math.max(A.zw, area.zw);
                }
              }
              Entity prefab;
              // ISSUE: reference to a compiler-generated field
              if (flag2 && this.m_BuildingUpgrade.TryGetValue(nativeArray1[index2], out prefab))
              {
                // ISSUE: reference to a compiler-generated method
                if (prefab != prefabRef.m_Prefab && this.CalculateBuildingCullArea(nativeArray2[index2], prefab, roComponentLookup, out area))
                {
                  if (!this.heightMapRenderRequired)
                  {
                    this.heightMapRenderRequired = true;
                    A = area;
                  }
                  else
                  {
                    A.xy = math.min(A.xy, area.xy);
                    A.zw = math.max(A.zw, area.zw);
                  }
                }
                // ISSUE: reference to a compiler-generated field
                this.m_BuildingUpgrade.Remove(nativeArray1[index2]);
              }
            }
          }
          archetypeChunkArray.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ToolSystem.actionMode.IsEditor() && !this.m_EditorLotQuery.IsEmpty)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_EditorLotQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
          this.CompleteDependency();
          for (int index3 = 0; index3 < archetypeChunkArray.Length; ++index3)
          {
            NativeArray<Game.Objects.Transform> nativeArray4 = archetypeChunkArray[index3].GetNativeArray<Game.Objects.Transform>(ref componentTypeHandle1);
            NativeArray<PrefabRef> nativeArray5 = archetypeChunkArray[index3].GetNativeArray<PrefabRef>(ref componentTypeHandle2);
            for (int index4 = 0; index4 < nativeArray4.Length; ++index4)
            {
              PrefabRef prefabRef = nativeArray5[index4];
              // ISSUE: reference to a compiler-generated method
              if (this.CalculateBuildingCullArea(nativeArray4[index4], prefabRef.m_Prefab, roComponentLookup, out area))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_WaterSystem.TerrainWillChange();
                if (!this.heightMapRenderRequired)
                {
                  this.heightMapRenderRequired = true;
                  A = area;
                }
                else
                {
                  A.xy = math.min(A.xy, area.xy);
                  A.zw = math.max(A.zw, area.zw);
                }
              }
            }
          }
          archetypeChunkArray.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingUpgrade.Clear();
      }
      // ISSUE: reference to a compiler-generated field
      if ((isLoaded ? 1 : (!this.m_RoadsChanged.IsEmptyIgnoreFilter ? 1 : 0)) != 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NativeArray<ArchetypeChunk> archetypeChunkArray = (isLoaded ? this.m_RoadsGroup : this.m_RoadsChanged).ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabRef> componentTypeHandle = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<NetData> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<NetGeometryData> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Composition> roComponentLookup3 = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Orphan> roComponentLookup4 = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<NodeGeometry> roComponentLookup5 = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<EdgeGeometry> roComponentLookup6 = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<StartNodeGeometry> roComponentLookup7 = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<EndNodeGeometry> roComponentLookup8 = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup;
        this.CompleteDependency();
        for (int index5 = 0; index5 < archetypeChunkArray.Length; ++index5)
        {
          NativeArray<Entity> nativeArray6 = archetypeChunkArray[index5].GetNativeArray(entityTypeHandle);
          NativeArray<PrefabRef> nativeArray7 = archetypeChunkArray[index5].GetNativeArray<PrefabRef>(ref componentTypeHandle);
          if (isLoaded)
          {
            this.heightMapRenderRequired = true;
            // ISSUE: reference to a compiler-generated field
            A = this.m_CascadeRanges[TerrainSystem.baseLod];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_WaterSystem.TerrainWillChange();
            break;
          }
          for (int index6 = 0; index6 < nativeArray6.Length; ++index6)
          {
            Entity entity = nativeArray6[index6];
            NetGeometryData componentData;
            if (roComponentLookup2.TryGetComponent(nativeArray7[index6].m_Prefab, out componentData) && (componentData.m_Flags & (Game.Net.GeometryFlags.FlattenTerrain | Game.Net.GeometryFlags.ClipTerrain)) != (Game.Net.GeometryFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_RoadUpdate = true;
              if ((componentData.m_Flags & Game.Net.GeometryFlags.FlattenTerrain) != (Game.Net.GeometryFlags) 0)
              {
                Bounds3 bounds = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
                if (roComponentLookup3.HasComponent(entity))
                {
                  EdgeGeometry edgeGeometry = roComponentLookup6[entity];
                  StartNodeGeometry startNodeGeometry = roComponentLookup7[entity];
                  EndNodeGeometry endNodeGeometry = roComponentLookup8[entity];
                  if (math.any(edgeGeometry.m_Start.m_Length + edgeGeometry.m_End.m_Length > 0.1f))
                    bounds |= edgeGeometry.m_Bounds;
                  if (math.any(startNodeGeometry.m_Geometry.m_Left.m_Length > 0.05f) | math.any(startNodeGeometry.m_Geometry.m_Right.m_Length > 0.05f))
                    bounds |= startNodeGeometry.m_Geometry.m_Bounds;
                  if (math.any(endNodeGeometry.m_Geometry.m_Left.m_Length > 0.05f) | math.any(endNodeGeometry.m_Geometry.m_Right.m_Length > 0.05f))
                    bounds |= endNodeGeometry.m_Geometry.m_Bounds;
                }
                else if (roComponentLookup4.HasComponent(entity))
                {
                  NodeGeometry nodeGeometry = roComponentLookup5[entity];
                  bounds |= nodeGeometry.m_Bounds;
                }
                if ((double) bounds.min.x <= (double) bounds.max.x)
                {
                  NetData netData = roComponentLookup1[nativeArray7[index6].m_Prefab];
                  bounds = MathUtils.Expand(bounds, (float3) (NetUtils.GetTerrainSmoothingWidth(netData) - 8f));
                  updateBuffer.Add(bounds.xz);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.m_WaterSystem.TerrainWillChange();
                  if (!this.heightMapRenderRequired)
                  {
                    this.heightMapRenderRequired = true;
                    A = new float4(bounds.min.xz, bounds.max.xz);
                  }
                  else
                  {
                    A.xy = math.min(A.xy, bounds.min.xz);
                    A.zw = math.max(A.zw, bounds.max.xz);
                  }
                }
              }
            }
          }
        }
        archetypeChunkArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      int num1 = isLoaded ? 1 : (!this.m_AreasChanged.IsEmptyIgnoreFilter ? 1 : 0);
      bool clipAreasChanged = isLoaded;
      if (num1 != 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NativeArray<ArchetypeChunk> archetypeChunkArray = (isLoaded ? this.m_AreasQuery : this.m_AreasChanged).ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Terrain_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Game.Areas.Terrain> componentTypeHandle4 = this.__TypeHandle.__Game_Areas_Terrain_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Clip_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Clip> componentTypeHandle5 = this.__TypeHandle.__Game_Areas_Clip_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Geometry> componentTypeHandle6 = this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentTypeHandle;
        this.CompleteDependency();
        for (int index7 = 0; index7 < archetypeChunkArray.Length; ++index7)
        {
          clipAreasChanged |= archetypeChunkArray[index7].Has<Clip>(ref componentTypeHandle5);
          if (archetypeChunkArray[index7].Has<Game.Areas.Terrain>(ref componentTypeHandle4))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_AreaUpdate = true;
            NativeArray<Geometry> nativeArray = archetypeChunkArray[index7].GetNativeArray<Geometry>(ref componentTypeHandle6);
            if (isLoaded)
            {
              this.heightMapRenderRequired = true;
              // ISSUE: reference to a compiler-generated field
              A = this.m_CascadeRanges[TerrainSystem.baseLod];
              break;
            }
            for (int index8 = 0; index8 < nativeArray.Length; ++index8)
            {
              Bounds3 bounds = nativeArray[index8].m_Bounds;
              if ((double) bounds.min.x <= (double) bounds.max.x)
              {
                updateBuffer.Add(bounds.xz);
                if (!this.heightMapRenderRequired)
                {
                  this.heightMapRenderRequired = true;
                  A = new float4(bounds.min.xz, bounds.max.xz);
                }
                else
                {
                  A.xy = math.min(A.xy, bounds.min.xz);
                  A.zw = math.max(A.zw, bounds.max.xz);
                }
              }
            }
          }
        }
        archetypeChunkArray.Dispose();
      }
      if (this.heightMapRenderRequired)
        A += new float4(-10f, -10f, 10f, 10f);
      float4 area1 = A;
      for (int index = 0; index <= TerrainSystem.baseLod; ++index)
      {
        if (this.heightMapRenderRequired)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.heightMapViewport[index] = new float4(A.x - this.m_CascadeRanges[index].x, A.y - this.m_CascadeRanges[index].y, A.z - this.m_CascadeRanges[index].x, A.w - this.m_CascadeRanges[index].y);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.heightMapViewport[index] /= new float4(this.m_CascadeRanges[index].z - this.m_CascadeRanges[index].x, this.m_CascadeRanges[index].w - this.m_CascadeRanges[index].y, this.m_CascadeRanges[index].z - this.m_CascadeRanges[index].x, this.m_CascadeRanges[index].w - this.m_CascadeRanges[index].y);
          this.heightMapViewport[index].zw -= this.heightMapViewport[index].xy;
          // ISSUE: reference to a compiler-generated method
          this.heightMapViewport[index] = this.ClipViewport(this.heightMapViewport[index]);
          this.heightMapSliceUpdated[index] = (double) this.heightMapViewport[index].w > 0.0 && (double) this.heightMapViewport[index].z > 0.0;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          area1.xy = math.min(area1.xy, this.m_CascadeRanges[index].xy + this.heightMapViewport[index].xy * (this.m_CascadeRanges[index].zw - this.m_CascadeRanges[index].xy));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          area1.zw = math.max(area1.zw, this.m_CascadeRanges[index].xy + (this.heightMapViewport[index].xy + this.heightMapViewport[index].zw) * (this.m_CascadeRanges[index].zw - this.m_CascadeRanges[index].xy));
        }
        else
        {
          this.heightMapViewport[index] = float4.zero;
          this.heightMapSliceUpdated[index] = false;
        }
      }
      for (int index = TerrainSystem.baseLod + 1; index < 4; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float2 float2_1 = (this.m_CascadeRanges[TerrainSystem.baseLod].zw - this.m_CascadeRanges[TerrainSystem.baseLod].xy) / math.pow(2f, (float) (index - TerrainSystem.baseLod));
        float num2 = math.min(float2_1.x, float2_1.y) / 4f;
        float4.xy = position.xz - float2_1 * 0.5f;
        float4.zw = position.xz + float2_1 * 0.5f;
        // ISSUE: reference to a compiler-generated field
        if ((double) float4.x < (double) this.m_CascadeRanges[0].x)
        {
          // ISSUE: reference to a compiler-generated field
          float num3 = this.m_CascadeRanges[0].x - float4.x;
          float4.x += num3;
          float4.z += num3;
        }
        // ISSUE: reference to a compiler-generated field
        if ((double) float4.y < (double) this.m_CascadeRanges[0].y)
        {
          // ISSUE: reference to a compiler-generated field
          float num4 = this.m_CascadeRanges[0].y - float4.y;
          float4.y += num4;
          float4.w += num4;
        }
        // ISSUE: reference to a compiler-generated field
        if ((double) float4.z > (double) this.m_CascadeRanges[0].z)
        {
          // ISSUE: reference to a compiler-generated field
          float num5 = this.m_CascadeRanges[0].z - float4.z;
          float4.x += num5;
          float4.z += num5;
        }
        // ISSUE: reference to a compiler-generated field
        if ((double) float4.w > (double) this.m_CascadeRanges[0].w)
        {
          // ISSUE: reference to a compiler-generated field
          float num6 = this.m_CascadeRanges[0].w - float4.w;
          float4.y += num6;
          float4.w += num6;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float2 float2_2 = math.abs(float4.xy - new float2(this.m_CascadeRanges[index].x, this.m_CascadeRanges[index].y));
        // ISSUE: reference to a compiler-generated field
        if ((double) math.lengthsq(this.m_CascadeRanges[index]) == 0.0 || (double) float2_2.x > (double) num2 || (double) float2_2.y > (double) num2)
        {
          this.heightMapSliceUpdated[index] = true;
          this.heightMapViewport[index] = new float4(0.0f, 0.0f, 1f, 1f);
          // ISSUE: reference to a compiler-generated field
          this.m_CascadeRanges[index] = float4;
          if (this.heightMapRenderRequired)
          {
            // ISSUE: reference to a compiler-generated field
            A.xy = math.min(A.xy, this.m_CascadeRanges[index].xy);
            // ISSUE: reference to a compiler-generated field
            A.zw = math.max(A.zw, this.m_CascadeRanges[index].zw);
            // ISSUE: reference to a compiler-generated field
            area1.xy = math.min(area1.xy, this.m_CascadeRanges[index].xy);
            // ISSUE: reference to a compiler-generated field
            area1.zw = math.max(area1.zw, this.m_CascadeRanges[index].zw);
          }
          else
          {
            this.heightMapRenderRequired = true;
            // ISSUE: reference to a compiler-generated field
            A = this.m_CascadeRanges[index];
            area1 = A;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if ((double) math.lengthsq(A) > 0.0 && this.Overlap(ref A, ref this.m_CascadeRanges[index]))
          {
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
            this.heightMapViewport[index] = new float4(math.clamp(A.x, this.m_CascadeRanges[index].x, this.m_CascadeRanges[index].z) - this.m_CascadeRanges[index].x, math.clamp(A.y, this.m_CascadeRanges[index].y, this.m_CascadeRanges[index].w) - this.m_CascadeRanges[index].y, math.clamp(A.z, this.m_CascadeRanges[index].x, this.m_CascadeRanges[index].z) - this.m_CascadeRanges[index].x, math.clamp(A.w, this.m_CascadeRanges[index].y, this.m_CascadeRanges[index].w) - this.m_CascadeRanges[index].y);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.heightMapViewport[index] /= new float4(this.m_CascadeRanges[index].z - this.m_CascadeRanges[index].x, this.m_CascadeRanges[index].w - this.m_CascadeRanges[index].y, this.m_CascadeRanges[index].z - this.m_CascadeRanges[index].x, this.m_CascadeRanges[index].w - this.m_CascadeRanges[index].y);
            this.heightMapViewport[index].zw -= this.heightMapViewport[index].xy;
            // ISSUE: reference to a compiler-generated method
            this.heightMapViewport[index] = this.ClipViewport(this.heightMapViewport[index]);
            this.heightMapSliceUpdated[index] = (double) this.heightMapViewport[index].w > 0.0 && (double) this.heightMapViewport[index].z > 0.0;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            area1.xy = math.min(area1.xy, this.m_CascadeRanges[index].xy + this.heightMapViewport[index].xy * (this.m_CascadeRanges[index].zw - this.m_CascadeRanges[index].xy));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            area1.zw = math.max(area1.zw, this.m_CascadeRanges[index].xy + (this.heightMapViewport[index].xy + this.heightMapViewport[index].zw) * (this.m_CascadeRanges[index].zw - this.m_CascadeRanges[index].xy));
          }
          else
          {
            this.heightMapSliceUpdated[index] = false;
            this.heightMapViewport[index] = float4.zero;
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (((this.heightMapRenderRequired ? 1 : (this.m_RoadUpdate ? 1 : 0)) | (clipAreasChanged ? 1 : 0)) != 0)
      {
        if (this.heightMapRenderRequired)
        {
          area1 += new float4(-10f, -10f, 10f, 10f);
          // ISSUE: reference to a compiler-generated field
          this.m_LastCullArea = area1;
          this.heightMapSliceUpdatedLast = this.heightMapSliceUpdated;
          this.heightMapViewportUpdated = this.heightMapViewport;
        }
        int laneCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.CullForCascades(area1, this.heightMapRenderRequired, this.m_RoadUpdate, this.m_AreaUpdate, clipAreasChanged, out laneCount);
        if (this.heightMapRenderRequired)
        {
          for (int cascadeIndex = 3; cascadeIndex >= TerrainSystem.baseLod; --cascadeIndex)
          {
            if (this.heightMapSliceUpdated[cascadeIndex])
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.CullCascade(cascadeIndex, this.m_CascadeRanges[cascadeIndex], this.heightMapViewport[cascadeIndex], laneCount);
            }
            else
              this.heightMapCullArea[cascadeIndex] = float4.zero;
          }
        }
        JobHandle.ScheduleBatchedJobs();
      }
      for (int index = 0; index < 4; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        float4 cascadeRange = this.m_CascadeRanges[index];
        cascadeRange.zw = 1f / math.max((float2) (1f / 1000f), cascadeRange.zw - cascadeRange.xy);
        cascadeRange.xy *= cascadeRange.zw;
        // ISSUE: reference to a compiler-generated field
        this.m_ShaderCascadeRanges[index] = (Vector4) cascadeRange;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalVectorArray(TerrainSystem.ShaderID._CascadeRangesID, this.m_ShaderCascadeRanges);
      // ISSUE: reference to a compiler-generated field
      this.m_CascadeReset = false;
    }

    public void RenderCascades()
    {
      if (this.heightMapRenderRequired)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_GroundHeightSystem.BeforeUpdateHeights();
        // ISSUE: reference to a compiler-generated field
        this.m_CascadeCB.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingInstanceData.StartFrame();
        // ISSUE: reference to a compiler-generated field
        this.m_LaneInstanceData.StartFrame();
        // ISSUE: reference to a compiler-generated field
        this.m_TriangleInstanceData.StartFrame();
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeInstanceData.StartFrame();
        if (TerrainSystem.baseLod != 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_TerrainBlit.SetTexture("_WorldMap", (UnityEngine.Object) this.m_WorldMapEditable != (UnityEngine.Object) null ? (Texture) this.m_WorldMapEditable : this.worldHeightmap);
        }
        for (int cascadeIndex = 3; cascadeIndex >= TerrainSystem.baseLod; --cascadeIndex)
        {
          if (this.heightMapSliceUpdated[cascadeIndex])
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.RenderCascade(cascadeIndex, this.m_CascadeRanges[cascadeIndex], this.heightMapViewport[cascadeIndex], ref this.m_CascadeCB);
          }
        }
        if (TerrainSystem.baseLod > 0 && this.heightMapSliceUpdated[0])
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int4 int4 = new int4((int) ((double) this.heightMapViewport[0].x * (double) this.m_HeightmapCascade.width), (int) ((double) this.heightMapViewport[0].y * (double) this.m_HeightmapCascade.height), (int) ((double) this.heightMapViewport[0].z * (double) this.m_HeightmapCascade.width), (int) ((double) this.heightMapViewport[0].w * (double) this.m_HeightmapCascade.height));
          // ISSUE: reference to a compiler-generated field
          if (this.m_LastWorldWrite.z == 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LastWorldWrite = int4;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int2 int2_1 = new int2(math.min(this.m_LastWorldWrite.x, int4.x), math.min(this.m_LastWorldWrite.y, int4.y));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int2 int2_2 = new int2(math.max(this.m_LastWorldWrite.x + this.m_LastWorldWrite.z, int4.x + int4.z), math.max(this.m_LastWorldWrite.y + this.m_LastWorldWrite.w, int4.y + int4.w));
            // ISSUE: reference to a compiler-generated field
            this.m_LastWorldWrite.xy = int2_1;
            // ISSUE: reference to a compiler-generated field
            this.m_LastWorldWrite.zw = int2_2 - int2_1;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.RenderWorldMapToCascade(this.m_CascadeRanges[0], this.heightMapViewport[0], ref this.m_CascadeCB);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingInstanceData.EndFrame();
        // ISSUE: reference to a compiler-generated field
        this.m_LaneInstanceData.EndFrame();
        // ISSUE: reference to a compiler-generated field
        this.m_TriangleInstanceData.EndFrame();
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeInstanceData.EndFrame();
        // ISSUE: reference to a compiler-generated field
        Graphics.ExecuteCommandBuffer(this.m_CascadeCB);
        if (this.heightMapSliceUpdated[TerrainSystem.baseLod])
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int4 int4 = new int4((int) ((double) this.heightMapViewport[TerrainSystem.baseLod].x * (double) this.m_HeightmapCascade.width), (int) ((double) this.heightMapViewport[TerrainSystem.baseLod].y * (double) this.m_HeightmapCascade.height), (int) ((double) this.heightMapViewport[TerrainSystem.baseLod].z * (double) this.m_HeightmapCascade.width), (int) ((double) this.heightMapViewport[TerrainSystem.baseLod].w * (double) this.m_HeightmapCascade.height));
          // ISSUE: reference to a compiler-generated field
          if (this.m_LastWrite.z == 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LastWrite = int4;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int2 int2_3 = new int2(math.min(this.m_LastWrite.x, int4.x), math.min(this.m_LastWrite.y, int4.y));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int2 int2_4 = new int2(math.max(this.m_LastWrite.x + this.m_LastWrite.z, int4.x + int4.z), math.max(this.m_LastWrite.y + this.m_LastWrite.w, int4.y + int4.w));
            // ISSUE: reference to a compiler-generated field
            this.m_LastWrite.xy = int2_3;
            // ISSUE: reference to a compiler-generated field
            this.m_LastWrite.zw = int2_4 - int2_3;
          }
          // ISSUE: reference to a compiler-generated method
          this.TriggerAsyncChange();
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_CascadeReset = false;
    }

    private void CullForCascades(
      float4 area,
      bool heightMapRenderRequired,
      bool roadsChanged,
      bool terrainAreasChanged,
      bool clipAreasChanged,
      out int laneCount)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CullFinished.Complete();
      if (roadsChanged)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ClipMapCull.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_LaneCullList.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_ClipMapList.Clear();
        // ISSUE: reference to a compiler-generated field
        laneCount = this.m_RoadsGroup.CalculateEntityCountWithoutFiltering() * 6;
        // ISSUE: reference to a compiler-generated field
        if (laneCount > this.m_LaneCullList.Capacity)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LaneCullList.Capacity = laneCount + math.max(laneCount / 4, 250);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ClipMapList.Capacity = this.m_LaneCullList.Capacity;
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        laneCount = this.m_LaneCullList.Length;
      }
      if (heightMapRenderRequired)
      {
        NativeQueue<BuildingUtils.LotInfo> nativeQueue = new NativeQueue<BuildingUtils.LotInfo>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_AdditionalBuildingTerraformElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_AssetStampData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Stack_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Lot_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.CullBuildingLotsJob jobData1 = new TerrainSystem.CullBuildingLotsJob()
        {
          m_LotHandle = this.__TypeHandle.__Game_Buildings_Lot_RO_ComponentTypeHandle,
          m_TransformHandle = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
          m_ElevationHandle = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentTypeHandle,
          m_StackHandle = this.__TypeHandle.__Game_Objects_Stack_RO_ComponentTypeHandle,
          m_PrefabRefHandle = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_InstalledUpgradeHandle = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
          m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
          m_PrefabBuildingExtensionData = this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup,
          m_PrefabAssetStampData = this.__TypeHandle.__Game_Prefabs_AssetStampData_RO_ComponentLookup,
          m_OverrideTerraform = this.__TypeHandle.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup,
          m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
          m_AdditionalLots = this.__TypeHandle.__Game_Prefabs_AdditionalBuildingTerraformElement_RO_BufferLookup,
          m_Area = area,
          Result = nativeQueue.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.DequeBuildingLotsJob jobData2 = new TerrainSystem.DequeBuildingLotsJob()
        {
          m_Queue = nativeQueue,
          m_List = this.m_BuildingCullList
        };
        // ISSUE: reference to a compiler-generated field
        EntityQuery buildingGroup = this.m_BuildingGroup;
        JobHandle dependency = this.Dependency;
        JobHandle dependsOn = jobData1.ScheduleParallel<TerrainSystem.CullBuildingLotsJob>(buildingGroup, dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingCull = jobData2.Schedule<TerrainSystem.DequeBuildingLotsJob>(dependsOn);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CullFinished = this.m_BuildingCull;
        // ISSUE: reference to a compiler-generated field
        nativeQueue.Dispose(this.m_BuildingCull);
      }
      if (roadsChanged)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_TerrainComposition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.CullRoadsJob jobData3 = new TerrainSystem.CullRoadsJob()
        {
          m_EntityHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
          m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
          m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
          m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
          m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
          m_NodeGeometryData = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup,
          m_OrphanData = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup,
          m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
          m_TerrainCompositionData = this.__TypeHandle.__Game_Prefabs_TerrainComposition_RO_ComponentLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_NetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
          m_NetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
          m_Area = this.m_CascadeRanges[TerrainSystem.baseLod],
          Result = this.m_LaneCullList.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_LaneCull = jobData3.ScheduleParallel<TerrainSystem.CullRoadsJob>(this.m_RoadsGroup, this.Dependency);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CullFinished = JobHandle.CombineDependencies(this.m_CullFinished, this.m_LaneCull);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.GenerateClipDataJob jobData4 = new TerrainSystem.GenerateClipDataJob()
        {
          m_RoadsToCull = this.m_LaneCullList,
          Result = this.m_ClipMapList.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        this.m_CurrentClipMap = (ComputeBuffer) null;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ClipMapCull = jobData4.Schedule<TerrainSystem.GenerateClipDataJob, TerrainSystem.LaneSection>(this.m_LaneCullList, 128, this.m_LaneCull);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CullFinished = JobHandle.CombineDependencies(this.m_CullFinished, this.m_ClipMapCull);
      }
      if (terrainAreasChanged)
      {
        NativeQueue<TerrainSystem.AreaTriangle> nativeQueue1 = new NativeQueue<TerrainSystem.AreaTriangle>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeQueue<TerrainSystem.AreaEdge> nativeQueue2 = new NativeQueue<TerrainSystem.AreaEdge>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_StorageAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_TerrainAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Storage_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Area_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Clip_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.CullAreasJob jobData5 = new TerrainSystem.CullAreasJob()
        {
          m_ClipType = this.__TypeHandle.__Game_Areas_Clip_RO_ComponentTypeHandle,
          m_AreaType = this.__TypeHandle.__Game_Areas_Area_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_GeometryType = this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentTypeHandle,
          m_StorageType = this.__TypeHandle.__Game_Areas_Storage_RO_ComponentTypeHandle,
          m_NodeType = this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle,
          m_TriangleType = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle,
          m_PrefabTerrainAreaData = this.__TypeHandle.__Game_Prefabs_TerrainAreaData_RO_ComponentLookup,
          m_PrefabStorageAreaData = this.__TypeHandle.__Game_Prefabs_StorageAreaData_RO_ComponentLookup,
          m_Area = this.m_CascadeRanges[TerrainSystem.baseLod],
          m_Triangles = nativeQueue1.AsParallelWriter(),
          m_Edges = nativeQueue2.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.DequeTrianglesJob jobData6 = new TerrainSystem.DequeTrianglesJob()
        {
          m_Queue = nativeQueue1,
          m_List = this.m_TriangleCullList
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.DequeEdgesJob jobData7 = new TerrainSystem.DequeEdgesJob()
        {
          m_Queue = nativeQueue2,
          m_List = this.m_EdgeCullList
        };
        // ISSUE: reference to a compiler-generated field
        JobHandle dependsOn1 = jobData5.ScheduleParallel<TerrainSystem.CullAreasJob>(this.m_AreasQuery, this.Dependency);
        JobHandle job0 = jobData6.Schedule<TerrainSystem.DequeTrianglesJob>(dependsOn1);
        JobHandle dependsOn2 = dependsOn1;
        JobHandle job1 = jobData7.Schedule<TerrainSystem.DequeEdgesJob>(dependsOn2);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaCull = JobHandle.CombineDependencies(job0, job1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CullFinished = JobHandle.CombineDependencies(this.m_CullFinished, this.m_AreaCull);
        // ISSUE: reference to a compiler-generated field
        nativeQueue1.Dispose(this.m_AreaCull);
        // ISSUE: reference to a compiler-generated field
        nativeQueue2.Dispose(this.m_AreaCull);
      }
      if (clipAreasChanged)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_HasAreaClipMeshData)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_HasAreaClipMeshData = true;
          // ISSUE: reference to a compiler-generated field
          this.m_AreaClipMeshData = Mesh.AllocateWritableMeshData(1);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Area_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Clip_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        JobHandle outJobHandle;
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
        TerrainSystem.GenerateAreaClipMeshJob jobData = new TerrainSystem.GenerateAreaClipMeshJob()
        {
          m_Chunks = this.m_AreasQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
          m_ClipType = this.__TypeHandle.__Game_Areas_Clip_RO_ComponentTypeHandle,
          m_AreaType = this.__TypeHandle.__Game_Areas_Area_RO_ComponentTypeHandle,
          m_NodeType = this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle,
          m_TriangleType = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle,
          m_MeshData = this.m_AreaClipMeshData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_AreaClipMeshDataDeps = jobData.Schedule<TerrainSystem.GenerateAreaClipMeshJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_Chunks.Dispose(this.m_AreaClipMeshDataDeps);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CullFinished = JobHandle.CombineDependencies(this.m_CullFinished, this.m_AreaClipMeshDataDeps);
      }
      // ISSUE: reference to a compiler-generated field
      this.Dependency = this.m_CullFinished;
    }

    public void CullClipMapForView(Viewer viewer)
    {
    }

    private void CullCascade(int cascadeIndex, float4 area, float4 viewport, int laneCount)
    {
      if ((double) viewport.z == 0.0 || (double) viewport.w == 0.0)
        UnityEngine.Debug.LogError((object) "Invalid Viewport");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      TerrainSystem.CascadeCullInfo cascadeCullInfo = this.m_CascadeCulling[cascadeIndex];
      // ISSUE: reference to a compiler-generated field
      cascadeCullInfo.m_BuildingHandle.Complete();
      // ISSUE: reference to a compiler-generated field
      cascadeCullInfo.m_BuildingRenderList = new NativeList<TerrainSystem.BuildingLotDraw>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      float2 xy = area.xy;
      float2 float2 = area.zw - area.xy;
      area = new float4(xy.x + float2.x * viewport.x, xy.y + float2.y * viewport.y, xy.x + float2.x * (viewport.x + viewport.z), xy.y + float2.y * (viewport.y + viewport.w));
      area += new float4(-10f, -10f, 10f, 10f);
      this.heightMapCullArea[cascadeIndex] = area;
      NativeQueue<TerrainSystem.BuildingLotDraw> nativeQueue = new NativeQueue<TerrainSystem.BuildingLotDraw>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TerrainSystem.CullBuildingsCascadeJob jobData1 = new TerrainSystem.CullBuildingsCascadeJob()
      {
        m_LotsToCull = this.m_BuildingCullList,
        m_Area = area,
        Result = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TerrainSystem.DequeBuildingDrawsJob jobData2 = new TerrainSystem.DequeBuildingDrawsJob()
      {
        m_Queue = nativeQueue,
        m_List = cascadeCullInfo.m_BuildingRenderList
      };
      // ISSUE: reference to a compiler-generated field
      NativeList<BuildingUtils.LotInfo> buildingCullList = this.m_BuildingCullList;
      // ISSUE: reference to a compiler-generated field
      JobHandle buildingCull = this.m_BuildingCull;
      JobHandle dependsOn = jobData1.Schedule<TerrainSystem.CullBuildingsCascadeJob, BuildingUtils.LotInfo>(buildingCullList, 128, buildingCull);
      // ISSUE: reference to a compiler-generated field
      cascadeCullInfo.m_BuildingHandle = jobData2.Schedule<TerrainSystem.DequeBuildingDrawsJob>(dependsOn);
      // ISSUE: reference to a compiler-generated field
      nativeQueue.Dispose(cascadeCullInfo.m_BuildingHandle);
      // ISSUE: reference to a compiler-generated field
      cascadeCullInfo.m_LaneHandle.Complete();
      // ISSUE: reference to a compiler-generated field
      cascadeCullInfo.m_LaneRenderList = new NativeList<TerrainSystem.LaneDraw>(laneCount, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TerrainSystem.CullRoadsCacscadeJob jobData3 = new TerrainSystem.CullRoadsCacscadeJob()
      {
        m_RoadsToCull = this.m_LaneCullList,
        m_Area = area,
        m_Scale = 1f / this.heightScaleOffset.x,
        Result = cascadeCullInfo.m_LaneRenderList.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cascadeCullInfo.m_LaneHandle = jobData3.Schedule<TerrainSystem.CullRoadsCacscadeJob, TerrainSystem.LaneSection>(this.m_LaneCullList, 128, this.m_LaneCull);
      // ISSUE: reference to a compiler-generated field
      cascadeCullInfo.m_AreaHandle.Complete();
      // ISSUE: reference to a compiler-generated field
      cascadeCullInfo.m_TriangleRenderList = new NativeList<TerrainSystem.AreaTriangle>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      cascadeCullInfo.m_EdgeRenderList = new NativeList<TerrainSystem.AreaEdge>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TerrainSystem.CullTrianglesJob jobData4 = new TerrainSystem.CullTrianglesJob()
      {
        m_Triangles = this.m_TriangleCullList,
        m_Area = area,
        Result = cascadeCullInfo.m_TriangleRenderList
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TerrainSystem.CullEdgesJob jobData5 = new TerrainSystem.CullEdgesJob()
      {
        m_Edges = this.m_EdgeCullList,
        m_Area = area,
        Result = cascadeCullInfo.m_EdgeRenderList
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle job0 = jobData4.Schedule<TerrainSystem.CullTrianglesJob>(this.m_AreaCull);
      // ISSUE: reference to a compiler-generated field
      JobHandle areaCull = this.m_AreaCull;
      JobHandle job1 = jobData5.Schedule<TerrainSystem.CullEdgesJob>(areaCull);
      // ISSUE: reference to a compiler-generated field
      cascadeCullInfo.m_AreaHandle = JobHandle.CombineDependencies(job0, job1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CullFinished = JobHandle.CombineDependencies(this.m_CullFinished, JobHandle.CombineDependencies(cascadeCullInfo.m_BuildingHandle, cascadeCullInfo.m_LaneHandle, cascadeCullInfo.m_AreaHandle));
    }

    private void DrawHeightAdjustments(
      ref CommandBuffer cmdBuffer,
      int cascade,
      float4 area,
      float4 viewport,
      RenderTargetBinding binding,
      ref NativeArray<TerrainSystem.BuildingLotDraw> lots,
      ref NativeArray<TerrainSystem.LaneDraw> lanes,
      ref NativeArray<TerrainSystem.AreaTriangle> triangles,
      ref NativeArray<TerrainSystem.AreaEdge> edges,
      ref Material lotMaterial,
      ref Material laneMaterial,
      ref Material areaMaterial)
    {
      float4 float4 = new float4(-area.xy, 1f / (area.zw - area.xy));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Rect scissor = new Rect(viewport.x * (float) this.m_HeightmapCascade.width, viewport.y * (float) this.m_HeightmapCascade.height, viewport.z * (float) this.m_HeightmapCascade.width, viewport.w * (float) this.m_HeightmapCascade.height);
      if (lots.Length > 0)
      {
        // ISSUE: reference to a compiler-generated field
        ComputeBuffer computeBuffer = this.m_BuildingInstanceData.Request(lots.Length);
        computeBuffer.SetData<TerrainSystem.BuildingLotDraw>(lots);
        computeBuffer.name = string.Format("BuildingLot Buffer Cascade{0}", (object) cascade);
        // ISSUE: reference to a compiler-generated field
        lotMaterial.SetVector(TerrainSystem.ShaderID._TerrainScaleOffsetID, new Vector4(this.heightScaleOffset.x, this.heightScaleOffset.y, 0.0f, 0.0f));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        lotMaterial.SetVector(TerrainSystem.ShaderID._MapOffsetScaleID, this.m_MapOffsetScale);
        // ISSUE: reference to a compiler-generated field
        lotMaterial.SetVector(TerrainSystem.ShaderID._CascadeOffsetScale, (Vector4) float4);
        // ISSUE: reference to a compiler-generated field
        lotMaterial.SetTexture(TerrainSystem.ShaderID._HeightmapID, this.heightmap);
        // ISSUE: reference to a compiler-generated field
        lotMaterial.SetBuffer(TerrainSystem.ShaderID._BuildingLotID, computeBuffer);
      }
      if (lanes.Length > 0)
      {
        // ISSUE: reference to a compiler-generated field
        ComputeBuffer computeBuffer = this.m_LaneInstanceData.Request(lanes.Length);
        computeBuffer.SetData<TerrainSystem.LaneDraw>(lanes);
        computeBuffer.name = string.Format("Lane Buffer Cascade{0}", (object) cascade);
        // ISSUE: reference to a compiler-generated field
        laneMaterial.SetVector(TerrainSystem.ShaderID._TerrainScaleOffsetID, new Vector4(this.heightScaleOffset.x, this.heightScaleOffset.y, 0.0f, 0.0f));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        laneMaterial.SetVector(TerrainSystem.ShaderID._MapOffsetScaleID, this.m_MapOffsetScale);
        // ISSUE: reference to a compiler-generated field
        laneMaterial.SetVector(TerrainSystem.ShaderID._CascadeOffsetScale, (Vector4) float4);
        // ISSUE: reference to a compiler-generated field
        laneMaterial.SetTexture(TerrainSystem.ShaderID._HeightmapID, this.heightmap);
        // ISSUE: reference to a compiler-generated field
        laneMaterial.SetBuffer(TerrainSystem.ShaderID._LanesID, computeBuffer);
      }
      if (triangles.Length > 0 || edges.Length > 0)
      {
        // ISSUE: reference to a compiler-generated field
        ComputeBuffer computeBuffer1 = this.m_TriangleInstanceData.Request(triangles.Length);
        computeBuffer1.SetData<TerrainSystem.AreaTriangle>(triangles);
        computeBuffer1.name = string.Format("Triangle Buffer Cascade{0}", (object) cascade);
        // ISSUE: reference to a compiler-generated field
        ComputeBuffer computeBuffer2 = this.m_EdgeInstanceData.Request(edges.Length);
        computeBuffer2.SetData<TerrainSystem.AreaEdge>(edges);
        computeBuffer2.name = string.Format("Edge Buffer Cascade{0}", (object) cascade);
        // ISSUE: reference to a compiler-generated field
        areaMaterial.SetVector(TerrainSystem.ShaderID._TerrainScaleOffsetID, new Vector4(this.heightScaleOffset.x, this.heightScaleOffset.y, 0.0f, 0.0f));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        areaMaterial.SetVector(TerrainSystem.ShaderID._MapOffsetScaleID, this.m_MapOffsetScale);
        // ISSUE: reference to a compiler-generated field
        areaMaterial.SetVector(TerrainSystem.ShaderID._CascadeOffsetScale, (Vector4) float4);
        // ISSUE: reference to a compiler-generated field
        areaMaterial.SetTexture(TerrainSystem.ShaderID._HeightmapID, this.heightmap);
        // ISSUE: reference to a compiler-generated field
        areaMaterial.SetBuffer(TerrainSystem.ShaderID._TrianglesID, computeBuffer1);
        // ISSUE: reference to a compiler-generated field
        areaMaterial.SetBuffer(TerrainSystem.ShaderID._EdgesID, computeBuffer2);
      }
      if (lots.Length > 0)
        cmdBuffer.DrawProcedural(Matrix4x4.identity, lotMaterial, 1, MeshTopology.Triangles, 6, lots.Length);
      if (lanes.Length > 0)
      {
        // ISSUE: reference to a compiler-generated field
        cmdBuffer.DrawMeshInstancedProcedural(this.m_LaneMesh, 0, laneMaterial, 1, lanes.Length);
      }
      int id = Shader.PropertyToID("_CascadeMinHeights");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cmdBuffer.GetTemporaryRT(id, this.m_HeightmapCascade.width, this.m_HeightmapCascade.height, 0, FilterMode.Point, this.m_HeightmapCascade.graphicsFormat);
      int num1 = math.max(0, Mathf.FloorToInt(scissor.xMin));
      int num2 = math.max(0, Mathf.FloorToInt(scissor.yMin));
      // ISSUE: reference to a compiler-generated field
      int srcWidth = math.min(this.m_HeightmapCascade.width, Mathf.CeilToInt(scissor.xMax)) - num1;
      // ISSUE: reference to a compiler-generated field
      int srcHeight = math.min(this.m_HeightmapCascade.height, Mathf.CeilToInt(scissor.yMax)) - num2;
      // ISSUE: reference to a compiler-generated field
      cmdBuffer.CopyTexture((RenderTargetIdentifier) (Texture) this.m_HeightmapCascade, cascade, 0, num1, num2, srcWidth, srcHeight, (RenderTargetIdentifier) id, 0, 0, num1, num2);
      cmdBuffer.SetRenderTarget(binding, 0, CubemapFace.Unknown, cascade);
      cmdBuffer.EnableScissorRect(scissor);
      if (triangles.Length > 0)
        cmdBuffer.DrawProcedural(Matrix4x4.identity, areaMaterial, 0, MeshTopology.Triangles, 3, triangles.Length);
      if (edges.Length > 0)
        cmdBuffer.DrawProcedural(Matrix4x4.identity, areaMaterial, 1, MeshTopology.Triangles, 6, edges.Length);
      if (lots.Length > 0)
        cmdBuffer.DrawProcedural(Matrix4x4.identity, lotMaterial, 0, MeshTopology.Triangles, 6, lots.Length);
      if (lanes.Length > 0)
      {
        // ISSUE: reference to a compiler-generated field
        cmdBuffer.DrawMeshInstancedProcedural(this.m_LaneMesh, 0, laneMaterial, 0, lanes.Length);
      }
      cmdBuffer.ReleaseTemporaryRT(id);
      if (lots.Length > 0)
        cmdBuffer.DrawProcedural(Matrix4x4.identity, lotMaterial, 2, MeshTopology.Triangles, 6, lots.Length);
      if (lanes.Length <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      cmdBuffer.DrawMeshInstancedProcedural(this.m_LaneMesh, 0, laneMaterial, 2, lanes.Length);
    }

    private void RenderWorldMapToCascade(float4 area, float4 viewport, ref CommandBuffer cmdBuffer)
    {
      // ISSUE: reference to a compiler-generated field
      if (!((UnityEngine.Object) this.m_WorldMapEditable != (UnityEngine.Object) null))
        return;
      bool flag = (double) viewport.x == 0.0 && (double) viewport.y == 0.0 && (double) viewport.z == 1.0 && (double) viewport.w == 1.0;
      // ISSUE: reference to a compiler-generated field
      Texture worldMapEditable = (Texture) this.m_WorldMapEditable;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Rect scissor = new Rect(viewport.x * (float) this.m_HeightmapCascade.width, viewport.y * (float) this.m_HeightmapCascade.height, viewport.z * (float) this.m_HeightmapCascade.width, viewport.w * (float) this.m_HeightmapCascade.height);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      RenderTargetBinding binding = new RenderTargetBinding((RenderTargetIdentifier) (Texture) this.m_HeightmapCascade, flag ? RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, (RenderTargetIdentifier) (Texture) this.m_HeightmapDepth, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare);
      cmdBuffer.SetRenderTarget(binding, 0, CubemapFace.Unknown, 0);
      cmdBuffer.ClearRenderTarget(true, false, UnityEngine.Color.black, 1f);
      cmdBuffer.EnableScissorRect(scissor);
      Vector2 scale = new Vector2(1f, 1f);
      cmdBuffer.Blit(worldMapEditable, (RenderTargetIdentifier) BuiltinRenderTextureType.CurrentActive, scale, new Vector2()
      {
        x = (area.x - this.worldOffset.x) / this.worldSize.x,
        y = (area.y - this.worldOffset.y) / this.worldSize.y
      });
    }

    private void RenderCascade(
      int cascadeIndex,
      float4 area,
      float4 viewport,
      ref CommandBuffer cmdBuffer)
    {
      bool flag1 = true;
      bool flag2 = (double) viewport.x == 0.0 && (double) viewport.y == 0.0 && (double) viewport.z == 1.0 && (double) viewport.w == 1.0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Rect scissor = new Rect(viewport.x * (float) this.m_HeightmapCascade.width, viewport.y * (float) this.m_HeightmapCascade.height, viewport.z * (float) this.m_HeightmapCascade.width, viewport.w * (float) this.m_HeightmapCascade.height);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      RenderTargetBinding binding = new RenderTargetBinding((RenderTargetIdentifier) (Texture) this.m_HeightmapCascade, flag2 & flag1 ? RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, (RenderTargetIdentifier) (Texture) this.m_HeightmapDepth, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare);
      cmdBuffer.SetRenderTarget(binding, 0, CubemapFace.Unknown, cascadeIndex);
      cmdBuffer.ClearRenderTarget(true, false, UnityEngine.Color.black, 1f);
      cmdBuffer.EnableScissorRect(scissor);
      if (flag1)
      {
        double num = cascadeIndex < TerrainSystem.baseLod ? (double) math.pow(2f, (float) -(cascadeIndex - TerrainSystem.baseLod)) : 1.0 / (double) math.pow(2f, (float) (cascadeIndex - TerrainSystem.baseLod));
        float2 float2_1 = (float2) new Vector2((float) num, (float) num);
        float2 float2_2 = (area.xy - this.playableOffset) / this.playableArea;
        if (cascadeIndex == TerrainSystem.baseLod || TerrainSystem.baseLod == 0)
        {
          cmdBuffer.Blit(this.heightmap, (RenderTargetIdentifier) BuiltinRenderTextureType.CurrentActive, (Vector2) float2_1, (Vector2) float2_2);
        }
        else
        {
          cmdBuffer.SetGlobalVector("_CascadeHeightmapOffsetScale", (Vector4) new float4(float2_2, float2_1));
          float2 zw = (area.zw - area.xy) / this.worldSize;
          float2 xy = (area.xy - this.worldOffset) / this.worldSize;
          cmdBuffer.SetGlobalVector("_CascadeWorldOffsetScale", (Vector4) new float4(xy, zw));
          // ISSUE: reference to a compiler-generated field
          cmdBuffer.Blit(this.heightmap, (RenderTargetIdentifier) BuiltinRenderTextureType.CurrentActive, this.m_TerrainBlit);
        }
      }
      Matrix4x4 proj = Matrix4x4.Ortho(area.x, area.z, area.w, area.y, this.heightScaleOffset.x + this.heightScaleOffset.y, this.heightScaleOffset.y);
      proj.m02 *= -1f;
      proj.m12 *= -1f;
      proj.m22 *= -1f;
      proj.m32 *= -1f;
      cmdBuffer.SetViewProjectionMatrices(GL.GetGPUProjectionMatrix(proj, true), Matrix4x4.identity);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      TerrainSystem.CascadeCullInfo cascadeCullInfo = this.m_CascadeCulling[cascadeIndex];
      // ISSUE: reference to a compiler-generated field
      cascadeCullInfo.m_BuildingHandle.Complete();
      // ISSUE: reference to a compiler-generated field
      cascadeCullInfo.m_LaneHandle.Complete();
      // ISSUE: reference to a compiler-generated field
      cascadeCullInfo.m_AreaHandle.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (cascadeCullInfo.m_BuildingRenderList.IsCreated || cascadeCullInfo.m_LaneRenderList.IsCreated || cascadeCullInfo.m_TriangleRenderList.IsCreated || cascadeCullInfo.m_EdgeRenderList.IsCreated)
      {
        NativeArray<TerrainSystem.BuildingLotDraw> lots = new NativeArray<TerrainSystem.BuildingLotDraw>();
        NativeArray<TerrainSystem.LaneDraw> lanes = new NativeArray<TerrainSystem.LaneDraw>();
        NativeArray<TerrainSystem.AreaTriangle> triangles = new NativeArray<TerrainSystem.AreaTriangle>();
        NativeArray<TerrainSystem.AreaEdge> edges = new NativeArray<TerrainSystem.AreaEdge>();
        // ISSUE: reference to a compiler-generated field
        if (cascadeCullInfo.m_BuildingRenderList.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          lots = cascadeCullInfo.m_BuildingRenderList.AsArray();
        }
        // ISSUE: reference to a compiler-generated field
        if (cascadeCullInfo.m_LaneRenderList.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          lanes = cascadeCullInfo.m_LaneRenderList.AsArray();
        }
        // ISSUE: reference to a compiler-generated field
        if (cascadeCullInfo.m_TriangleRenderList.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          triangles = cascadeCullInfo.m_TriangleRenderList.AsArray();
        }
        // ISSUE: reference to a compiler-generated field
        if (cascadeCullInfo.m_EdgeRenderList.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          edges = cascadeCullInfo.m_EdgeRenderList.AsArray();
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.DrawHeightAdjustments(ref cmdBuffer, cascadeIndex, area, viewport, binding, ref lots, ref lanes, ref triangles, ref edges, ref cascadeCullInfo.m_LotMaterial, ref cascadeCullInfo.m_LaneMaterial, ref cascadeCullInfo.m_AreaMaterial);
        // ISSUE: reference to a compiler-generated field
        if (cascadeCullInfo.m_BuildingRenderList.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          cascadeCullInfo.m_BuildingRenderList.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (cascadeCullInfo.m_LaneRenderList.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          cascadeCullInfo.m_LaneRenderList.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (cascadeCullInfo.m_TriangleRenderList.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          cascadeCullInfo.m_TriangleRenderList.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (cascadeCullInfo.m_EdgeRenderList.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          cascadeCullInfo.m_EdgeRenderList.Dispose();
        }
      }
      cmdBuffer.DisableScissorRect();
    }

    private void CreateRoadMeshes()
    {
      Mesh mesh1 = new Mesh();
      mesh1.name = "Lane Mesh";
      // ISSUE: reference to a compiler-generated field
      this.m_LaneMesh = mesh1;
      int num1 = 1;
      int num2 = 8;
      int length1 = (num1 + 1) * (num2 + 1);
      int length2 = num1 * num2 * 2 * 3;
      Vector3[] vector3Array1 = new Vector3[length1];
      Vector2[] vector2Array1 = new Vector2[length1];
      int[] triangles1 = new int[length2];
      for (int index1 = 0; index1 <= num2; ++index1)
      {
        for (int index2 = 0; index2 <= num1; ++index2)
        {
          vector3Array1[index2 + (num1 + 1) * index1] = new Vector3((float) index2 / (float) num1, 0.0f, (float) index1 / (float) num2);
          vector2Array1[index2 + (num1 + 1) * index1] = new Vector2(vector3Array1[index2 + (num1 + 1) * index1].x, vector3Array1[index2 + (num1 + 1) * index1].z);
        }
      }
      int num3 = num1 + 1;
      int num4 = 0;
      for (int index3 = 0; index3 < num2; ++index3)
      {
        for (int index4 = 0; index4 < num1; ++index4)
        {
          int[] numArray1 = triangles1;
          int index5 = num4;
          int num5 = index5 + 1;
          int num6 = index4 + num3 * (index3 + 1);
          numArray1[index5] = num6;
          int[] numArray2 = triangles1;
          int index6 = num5;
          int num7 = index6 + 1;
          int num8 = index4 + 1 + num3 * (index3 + 1);
          numArray2[index6] = num8;
          int[] numArray3 = triangles1;
          int index7 = num7;
          int num9 = index7 + 1;
          int num10 = index4 + 1 + num3 * index3;
          numArray3[index7] = num10;
          int[] numArray4 = triangles1;
          int index8 = num9;
          int num11 = index8 + 1;
          int num12 = index4 + num3 * (index3 + 1);
          numArray4[index8] = num12;
          int[] numArray5 = triangles1;
          int index9 = num11;
          int num13 = index9 + 1;
          int num14 = index4 + 1 + num3 * index3;
          numArray5[index9] = num14;
          int[] numArray6 = triangles1;
          int index10 = num13;
          num4 = index10 + 1;
          int num15 = index4 + num3 * index3;
          numArray6[index10] = num15;
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_LaneMesh.vertices = vector3Array1;
      // ISSUE: reference to a compiler-generated field
      this.m_LaneMesh.uv = vector2Array1;
      // ISSUE: reference to a compiler-generated field
      this.m_LaneMesh.subMeshCount = 1;
      // ISSUE: reference to a compiler-generated field
      this.m_LaneMesh.SetTriangles(triangles1, 0);
      // ISSUE: reference to a compiler-generated field
      this.m_LaneMesh.UploadMeshData(true);
      Mesh mesh2 = new Mesh();
      mesh2.name = "Clip Mesh";
      // ISSUE: reference to a compiler-generated field
      this.m_ClipMesh = mesh2;
      int num16 = length1;
      int length3 = length1 * 2;
      int length4 = length2 * 2 + num2 * 2 * 3 * 2 + num1 * 2 * 3 * 2;
      Vector3[] vector3Array2 = new Vector3[length3];
      Vector2[] vector2Array2 = new Vector2[length3];
      int[] triangles2 = new int[length4];
      for (int index11 = 0; index11 <= num2; ++index11)
      {
        for (int index12 = 0; index12 <= num1; ++index12)
        {
          vector3Array2[index12 + (num1 + 1) * index11] = new Vector3((float) index12 / (float) num1, 1f, (float) index11 / (float) num2);
          vector2Array2[index12 + (num1 + 1) * index11] = new Vector2(vector3Array2[index12 + (num1 + 1) * index11].x, vector3Array2[index12 + (num1 + 1) * index11].z);
          vector3Array2[num16 + index12 + (num1 + 1) * index11] = vector3Array2[index12 + (num1 + 1) * index11];
          vector3Array2[num16 + index12 + (num1 + 1) * index11].y = 0.0f;
          vector2Array2[num16 + index12 + (num1 + 1) * index11] = vector2Array2[index12 + (num1 + 1) * index11];
        }
      }
      int num17 = num1 + 1;
      int num18 = 0;
      for (int index13 = 0; index13 < num2; ++index13)
      {
        for (int index14 = 0; index14 < num1; ++index14)
        {
          int[] numArray7 = triangles2;
          int index15 = num18;
          int num19 = index15 + 1;
          int num20 = index14 + num17 * (index13 + 1);
          numArray7[index15] = num20;
          int[] numArray8 = triangles2;
          int index16 = num19;
          int num21 = index16 + 1;
          int num22 = index14 + 1 + num17 * (index13 + 1);
          numArray8[index16] = num22;
          int[] numArray9 = triangles2;
          int index17 = num21;
          int num23 = index17 + 1;
          int num24 = index14 + 1 + num17 * index13;
          numArray9[index17] = num24;
          int[] numArray10 = triangles2;
          int index18 = num23;
          int num25 = index18 + 1;
          int num26 = index14 + num17 * (index13 + 1);
          numArray10[index18] = num26;
          int[] numArray11 = triangles2;
          int index19 = num25;
          int num27 = index19 + 1;
          int num28 = index14 + 1 + num17 * index13;
          numArray11[index19] = num28;
          int[] numArray12 = triangles2;
          int index20 = num27;
          num18 = index20 + 1;
          int num29 = index14 + num17 * index13;
          numArray12[index20] = num29;
        }
      }
      for (int index21 = 0; index21 < num2; ++index21)
      {
        for (int index22 = 0; index22 < num1; ++index22)
        {
          int[] numArray13 = triangles2;
          int index23 = num18;
          int num30 = index23 + 1;
          int num31 = num16 + (index22 + 1 + num17 * (index21 + 1));
          numArray13[index23] = num31;
          int[] numArray14 = triangles2;
          int index24 = num30;
          int num32 = index24 + 1;
          int num33 = num16 + (index22 + num17 * (index21 + 1));
          numArray14[index24] = num33;
          int[] numArray15 = triangles2;
          int index25 = num32;
          int num34 = index25 + 1;
          int num35 = num16 + (index22 + 1 + num17 * index21);
          numArray15[index25] = num35;
          int[] numArray16 = triangles2;
          int index26 = num34;
          int num36 = index26 + 1;
          int num37 = num16 + (index22 + 1 + num17 * index21);
          numArray16[index26] = num37;
          int[] numArray17 = triangles2;
          int index27 = num36;
          int num38 = index27 + 1;
          int num39 = num16 + (index22 + num17 * (index21 + 1));
          numArray17[index27] = num39;
          int[] numArray18 = triangles2;
          int index28 = num38;
          num18 = index28 + 1;
          int num40 = num16 + (index22 + num17 * index21);
          numArray18[index28] = num40;
        }
      }
      int num41 = 0;
      for (int index29 = 0; index29 < num2; ++index29)
      {
        int[] numArray19 = triangles2;
        int index30 = num18;
        int num42 = index30 + 1;
        int num43 = num41 + num17 * (index29 + 1);
        numArray19[index30] = num43;
        int[] numArray20 = triangles2;
        int index31 = num42;
        int num44 = index31 + 1;
        int num45 = num41 + num17 * index29;
        numArray20[index31] = num45;
        int[] numArray21 = triangles2;
        int index32 = num44;
        int num46 = index32 + 1;
        int num47 = num16 + num41 + num17 * index29;
        numArray21[index32] = num47;
        int[] numArray22 = triangles2;
        int index33 = num46;
        int num48 = index33 + 1;
        int num49 = num16 + num41 + num17 * index29;
        numArray22[index33] = num49;
        int[] numArray23 = triangles2;
        int index34 = num48;
        int num50 = index34 + 1;
        int num51 = num16 + num41 + num17 * (index29 + 1);
        numArray23[index34] = num51;
        int[] numArray24 = triangles2;
        int index35 = num50;
        num18 = index35 + 1;
        int num52 = num41 + num17 * (index29 + 1);
        numArray24[index35] = num52;
      }
      int num53 = num1;
      for (int index36 = 0; index36 < num2; ++index36)
      {
        int[] numArray25 = triangles2;
        int index37 = num18;
        int num54 = index37 + 1;
        int num55 = num53 + num17 * index36;
        numArray25[index37] = num55;
        int[] numArray26 = triangles2;
        int index38 = num54;
        int num56 = index38 + 1;
        int num57 = num53 + num17 * (index36 + 1);
        numArray26[index38] = num57;
        int[] numArray27 = triangles2;
        int index39 = num56;
        int num58 = index39 + 1;
        int num59 = num16 + num53 + num17 * index36;
        numArray27[index39] = num59;
        int[] numArray28 = triangles2;
        int index40 = num58;
        int num60 = index40 + 1;
        int num61 = num16 + num53 + num17 * (index36 + 1);
        numArray28[index40] = num61;
        int[] numArray29 = triangles2;
        int index41 = num60;
        int num62 = index41 + 1;
        int num63 = num16 + num53 + num17 * index36;
        numArray29[index41] = num63;
        int[] numArray30 = triangles2;
        int index42 = num62;
        num18 = index42 + 1;
        int num64 = num53 + num17 * (index36 + 1);
        numArray30[index42] = num64;
      }
      for (int index43 = 0; index43 < num1; ++index43)
      {
        int[] numArray31 = triangles2;
        int index44 = num18;
        int num65 = index44 + 1;
        int num66 = index43;
        numArray31[index44] = num66;
        int[] numArray32 = triangles2;
        int index45 = num65;
        int num67 = index45 + 1;
        int num68 = index43 + num16;
        numArray32[index45] = num68;
        int[] numArray33 = triangles2;
        int index46 = num67;
        int num69 = index46 + 1;
        int num70 = index43 + num16 + 1;
        numArray33[index46] = num70;
        int[] numArray34 = triangles2;
        int index47 = num69;
        int num71 = index47 + 1;
        int num72 = index43 + num16 + 1;
        numArray34[index47] = num72;
        int[] numArray35 = triangles2;
        int index48 = num71;
        int num73 = index48 + 1;
        int num74 = index43 + 1;
        numArray35[index48] = num74;
        int[] numArray36 = triangles2;
        int index49 = num73;
        num18 = index49 + 1;
        int num75 = index43;
        numArray36[index49] = num75;
      }
      for (int index50 = 1; index50 <= num1; ++index50)
      {
        int[] numArray37 = triangles2;
        int index51 = num18;
        int num76 = index51 + 1;
        int num77 = length3 - index50;
        numArray37[index51] = num77;
        int[] numArray38 = triangles2;
        int index52 = num76;
        int num78 = index52 + 1;
        int num79 = length3 - index50 - 1;
        numArray38[index52] = num79;
        int[] numArray39 = triangles2;
        int index53 = num78;
        int num80 = index53 + 1;
        int num81 = length3 - index50 - num16 - 1;
        numArray39[index53] = num81;
        int[] numArray40 = triangles2;
        int index54 = num80;
        int num82 = index54 + 1;
        int num83 = length3 - index50 - num16 - 1;
        numArray40[index54] = num83;
        int[] numArray41 = triangles2;
        int index55 = num82;
        int num84 = index55 + 1;
        int num85 = length3 - index50 - num16;
        numArray41[index55] = num85;
        int[] numArray42 = triangles2;
        int index56 = num84;
        num18 = index56 + 1;
        int num86 = length3 - index50;
        numArray42[index56] = num86;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_ClipMesh.vertices = vector3Array2;
      // ISSUE: reference to a compiler-generated field
      this.m_ClipMesh.uv = vector2Array2;
      // ISSUE: reference to a compiler-generated field
      this.m_ClipMesh.subMeshCount = 1;
      // ISSUE: reference to a compiler-generated field
      this.m_ClipMesh.SetTriangles(triangles2, 0);
      // ISSUE: reference to a compiler-generated field
      this.m_ClipMesh.UploadMeshData(true);
    }

    public bool CalculateBuildingCullArea(
      Game.Objects.Transform transform,
      Entity prefab,
      ComponentLookup<ObjectGeometryData> geometryData,
      out float4 area)
    {
      area = float4.zero;
      ObjectGeometryData componentData;
      if (!geometryData.TryGetComponent(prefab, out componentData))
        return false;
      Bounds3 bounds3 = MathUtils.Expand(ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, componentData), (float3) (ObjectUtils.GetTerrainSmoothingWidth(componentData) - 8f));
      area.xy = bounds3.min.xz;
      area.zw = bounds3.max.xz;
      return true;
    }

    public void OnBuildingMoved(Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Game.Objects.Transform> roComponentLookup1 = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<PrefabRef> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ObjectGeometryData> roComponentLookup3 = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      this.CompleteDependency();
      float4 area = float4.zero;
      if (!roComponentLookup2.HasComponent(entity) || !roComponentLookup1.HasComponent(entity))
        return;
      PrefabRef prefabRef = roComponentLookup2[entity];
      // ISSUE: reference to a compiler-generated method
      if (!this.CalculateBuildingCullArea(roComponentLookup1[entity], prefabRef.m_Prefab, roComponentLookup3, out area))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_GroundHeightSystem.GetUpdateBuffer().Add(new Bounds2(area.xy, area.zw));
      // ISSUE: reference to a compiler-generated field
      if ((double) math.lengthsq(this.m_UpdateArea) > 0.0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateArea.xy = math.min(this.m_UpdateArea.xy, area.xy);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateArea.zw = math.max(this.m_UpdateArea.zw, area.zw);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateArea = area;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateArea += new float4(-10f, -10f, 10f, 10f);
    }

    public void GetLastMinMaxUpdate(out float3 min, out float3 max)
    {
      // ISSUE: reference to a compiler-generated field
      int4 updateArea = this.m_TerrainMinMax.UpdateArea;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      float2 minMax = this.m_TerrainMinMax.GetMinMax(updateArea);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float4 float4 = new float4((float) updateArea.x / (float) this.m_TerrainMinMax.size, (float) updateArea.y / (float) this.m_TerrainMinMax.size, (float) (updateArea.x + updateArea.z) / (float) this.m_TerrainMinMax.size, (float) (updateArea.y + updateArea.w) / (float) this.m_TerrainMinMax.size) * this.worldSize.xyxy + this.worldOffset.xyxy;
      min = new float3(float4.x, minMax.x, float4.y);
      max = new float3(float4.z, minMax.y, float4.w);
    }

    public NativeParallelHashMap<Entity, Entity>.ParallelWriter GetBuildingUpgradeWriter(
      int ExpectedAmount)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingUpgradeDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      if (ExpectedAmount > this.m_BuildingUpgrade.Capacity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingUpgrade.Capacity = ExpectedAmount;
      }
      // ISSUE: reference to a compiler-generated field
      return this.m_BuildingUpgrade.AsParallelWriter();
    }

    public void SetBuildingUpgradeWriterDependency(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingUpgradeDependencies = handle;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
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
    public TerrainSystem()
    {
    }

    public static class ShaderID
    {
      public static readonly int _BlurTempHorz = Shader.PropertyToID(nameof (_BlurTempHorz));
      public static readonly int _AvgTerrainHeightsTemp = Shader.PropertyToID(nameof (_AvgTerrainHeightsTemp));
      public static readonly int _DebugSmooth = Shader.PropertyToID(nameof (_DebugSmooth));
      public static readonly int _Heightmap = Shader.PropertyToID(nameof (_Heightmap));
      public static readonly int _BrushTexture = Shader.PropertyToID(nameof (_BrushTexture));
      public static readonly int _WorldTexture = Shader.PropertyToID(nameof (_WorldTexture));
      public static readonly int _WaterTexture = Shader.PropertyToID(nameof (_WaterTexture));
      public static readonly int _Range = Shader.PropertyToID(nameof (_Range));
      public static readonly int _CenterSizeRotation = Shader.PropertyToID(nameof (_CenterSizeRotation));
      public static readonly int _Dims = Shader.PropertyToID(nameof (_Dims));
      public static readonly int _BrushData = Shader.PropertyToID(nameof (_BrushData));
      public static readonly int _BrushData2 = Shader.PropertyToID(nameof (_BrushData2));
      public static readonly int _ClampArea = Shader.PropertyToID(nameof (_ClampArea));
      public static readonly int _WorldOffsetScale = Shader.PropertyToID(nameof (_WorldOffsetScale));
      public static readonly int _EdgeMaxDifference = Shader.PropertyToID(nameof (_EdgeMaxDifference));
      public static readonly int _BuildingLotID = Shader.PropertyToID("_BuildingLots");
      public static readonly int _LanesID = Shader.PropertyToID("_Lanes");
      public static readonly int _TrianglesID = Shader.PropertyToID("_Triangles");
      public static readonly int _EdgesID = Shader.PropertyToID("_Edges");
      public static readonly int _HeightmapID = Shader.PropertyToID("_BaseHeightMap");
      public static readonly int _TerrainScaleOffsetID = Shader.PropertyToID("_TerrainScaleOffset");
      public static readonly int _MapOffsetScaleID = Shader.PropertyToID("_MapOffsetScale");
      public static readonly int _BrushID = Shader.PropertyToID("_Brush");
      public static readonly int _CascadeRangesID = Shader.PropertyToID("colossal_TerrainCascadeRanges");
      public static readonly int _CascadeOffsetScale = Shader.PropertyToID(nameof (_CascadeOffsetScale));
      public static readonly int _HeightScaleOffset = Shader.PropertyToID(nameof (_HeightScaleOffset));
      public static readonly int _RoadData = Shader.PropertyToID(nameof (_RoadData));
      public static readonly int _ClipOffset = Shader.PropertyToID(nameof (_ClipOffset));
    }

    public struct BuildingLotDraw
    {
      public float2x4 m_HeightsX;
      public float2x4 m_HeightsZ;
      public float3 m_FlatX0;
      public float3 m_FlatZ0;
      public float3 m_FlatX1;
      public float3 m_FlatZ1;
      public float3 m_Position;
      public float3 m_AxisX;
      public float3 m_AxisZ;
      public float2 m_Size;
      public float4 m_MinLimit;
      public float4 m_MaxLimit;
      public float m_Circular;
      public float m_SmoothingWidth;
    }

    public struct LaneSection
    {
      public Bounds2 m_Bounds;
      public float4x3 m_Left;
      public float4x3 m_Right;
      public float3 m_MinOffset;
      public float3 m_MaxOffset;
      public float2 m_ClipOffset;
      public float m_WidthOffset;
      public TerrainSystem.LaneFlags m_Flags;
    }

    public struct LaneDraw
    {
      public float4x3 m_Left;
      public float4x3 m_Right;
      public float4 m_MinOffset;
      public float4 m_MaxOffset;
      public float2 m_WidthOffset;
    }

    public struct AreaTriangle
    {
      public float2 m_PositionA;
      public float2 m_PositionB;
      public float2 m_PositionC;
      public float2 m_NoiseSize;
      public float m_HeightDelta;
    }

    public struct AreaEdge
    {
      public float2 m_PositionA;
      public float2 m_PositionB;
      public float2 m_Angles;
      public float m_SideOffset;
    }

    [Flags]
    public enum LaneFlags
    {
      ShiftTerrain = 1,
      ClipTerrain = 2,
      MiddleLeft = 4,
      MiddleRight = 8,
      InverseClipOffset = 16, // 0x00000010
    }

    private class CascadeCullInfo
    {
      public JobHandle m_BuildingHandle;
      public NativeList<TerrainSystem.BuildingLotDraw> m_BuildingRenderList;
      public Material m_LotMaterial;
      public JobHandle m_LaneHandle;
      public NativeList<TerrainSystem.LaneDraw> m_LaneRenderList;
      public Material m_LaneMaterial;
      public JobHandle m_AreaHandle;
      public NativeList<TerrainSystem.AreaTriangle> m_TriangleRenderList;
      public NativeList<TerrainSystem.AreaEdge> m_EdgeRenderList;
      public Material m_AreaMaterial;

      public CascadeCullInfo(Material building, Material lane, Material area)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LotMaterial = new Material(building);
        // ISSUE: reference to a compiler-generated field
        this.m_LaneMaterial = new Material(lane);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaMaterial = new Material(area);
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingRenderList = new NativeList<TerrainSystem.BuildingLotDraw>();
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingHandle = new JobHandle();
        // ISSUE: reference to a compiler-generated field
        this.m_LaneHandle = new JobHandle();
        // ISSUE: reference to a compiler-generated field
        this.m_LaneRenderList = new NativeList<TerrainSystem.LaneDraw>();
        // ISSUE: reference to a compiler-generated field
        this.m_TriangleRenderList = new NativeList<TerrainSystem.AreaTriangle>();
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeRenderList = new NativeList<TerrainSystem.AreaEdge>();
        // ISSUE: reference to a compiler-generated field
        this.m_AreaHandle = new JobHandle();
      }
    }

    private struct ClipMapDraw
    {
      public float4x3 m_Left;
      public float4x3 m_Right;
      public float m_Height;
      public float m_OffsetFactor;
    }

    private class TerrainMinMaxMap
    {
      private RenderTexture[] m_IntermediateTex;
      private RenderTexture m_DownsampledDetail;
      private RenderTexture m_ResultTex;
      public NativeArray<half4> MinMaxMap;
      private NativeArray<half4> m_UpdateBuffer;
      private AsyncGPUReadbackRequest m_Current;
      private ComputeShader m_Shader;
      private int2 m_IntermediateSize;
      private int2 m_ResultSize;
      private int4 m_UpdatedArea;
      private int4 m_DebugArea;
      private bool m_Pending;
      private bool m_Updated;
      private bool m_Valid;
      private bool m_Partial;
      private int m_Steps;
      private int m_DetailSteps;
      private int m_BlockSize;
      private int m_DetailBlockSize;
      private int m_ID_WorldTexture;
      private int m_ID_DetailTexture;
      private int m_ID_UpdateArea;
      private int m_ID_WorldOffsetScale;
      private int m_ID_DetailOffsetScale;
      private int m_ID_WorldTextureSizeInvSize;
      private int m_ID_Result;
      private int m_KernalCSTerainMinMax;
      private int m_KernalCSWorldTerainMinMax;
      private int m_KernalCSDownsampleMinMax;
      private int2 m_InitValues = int2.zero;
      private Texture m_AsyncNeeded;
      private List<int4> m_UpdatesRequested = new List<int4>();
      private TerrainSystem m_TerrainSystem;
      private JobHandle m_UpdateJob;

      private RenderTexture CreateRenderTexture(string name, int2 size, bool compact)
      {
        RenderTexture renderTexture = new RenderTexture(size.x, size.y, 0, compact ? GraphicsFormat.R16G16_SFloat : GraphicsFormat.R16G16B16A16_SFloat);
        renderTexture.name = name;
        renderTexture.hideFlags = HideFlags.DontSave;
        renderTexture.enableRandomWrite = true;
        renderTexture.wrapMode = TextureWrapMode.Clamp;
        renderTexture.filterMode = FilterMode.Bilinear;
        renderTexture.Create();
        return renderTexture;
      }

      public void Init(int size, int original)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_IntermediateTex != null && size == this.m_InitValues.x && original == this.m_InitValues.y)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateJob.Complete();
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateJob = new JobHandle();
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.Dispose();
          // ISSUE: reference to a compiler-generated field
          this.m_InitValues = new int2(size, original);
          // ISSUE: reference to a compiler-generated field
          this.m_IntermediateSize = (int2) (original / 2);
          // ISSUE: reference to a compiler-generated field
          this.m_ResultSize = (int2) size;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ResultSize.x > this.m_IntermediateSize.x || this.m_ResultSize.y > this.m_IntermediateSize.y)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_ResultSize = this.m_IntermediateSize;
            // ISSUE: reference to a compiler-generated field
            this.m_Steps = 1;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Steps = math.floorlog2(original) + 1 - (math.floorlog2(size) + 1);
          }
          int num = math.max(math.floorlog2(original) - 2, 1);
          int size1 = (int) math.pow(2f, (float) (num - 1));
          // ISSUE: reference to a compiler-generated field
          this.m_DetailSteps = math.floorlog2(original) + 1 - num;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_BlockSize = (int) math.pow(2f, (float) this.m_Steps);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_DetailBlockSize = (int) math.pow(2f, (float) this.m_DetailSteps);
          // ISSUE: reference to a compiler-generated field
          this.m_IntermediateTex = new RenderTexture[2];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_IntermediateTex[0] = this.CreateRenderTexture("HeightMinMax_Setup0", this.m_IntermediateSize, true);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_IntermediateTex[1] = this.CreateRenderTexture("HeightMinMax_Setup1", this.m_IntermediateSize / 2, true);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_DownsampledDetail = this.CreateRenderTexture("HeightMinMax_Detail", (int2) size1, true);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_ResultTex = this.CreateRenderTexture("HeightMinMax_Result", (int2) this.m_ResultSize.x, false);
          // ISSUE: reference to a compiler-generated field
          this.m_Valid = false;
          // ISSUE: reference to a compiler-generated field
          this.m_Partial = false;
          // ISSUE: reference to a compiler-generated field
          this.m_Updated = false;
          // ISSUE: reference to a compiler-generated field
          this.m_Pending = false;
          // ISSUE: reference to a compiler-generated field
          this.MinMaxMap = new NativeArray<half4>(size * size, Allocator.Persistent);
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateBuffer = new NativeArray<half4>(size * size, Allocator.Persistent);
          // ISSUE: reference to a compiler-generated field
          this.m_Shader = Resources.Load<ComputeShader>("TerrainMinMax");
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_KernalCSTerainMinMax = this.m_Shader.FindKernel("CSTerainGenerateMinMax");
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_KernalCSWorldTerainMinMax = this.m_Shader.FindKernel("CSTerainWorldGenerateMinMax");
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_KernalCSDownsampleMinMax = this.m_Shader.FindKernel("CSDownsampleMinMax");
          // ISSUE: reference to a compiler-generated field
          this.m_ID_WorldTexture = Shader.PropertyToID("_WorldTexture");
          // ISSUE: reference to a compiler-generated field
          this.m_ID_DetailTexture = Shader.PropertyToID("_DetailHeightTexture");
          // ISSUE: reference to a compiler-generated field
          this.m_ID_UpdateArea = Shader.PropertyToID("_UpdateArea");
          // ISSUE: reference to a compiler-generated field
          this.m_ID_WorldOffsetScale = Shader.PropertyToID("_WorldOffsetScale");
          // ISSUE: reference to a compiler-generated field
          this.m_ID_DetailOffsetScale = Shader.PropertyToID("_DetailOffsetScale");
          // ISSUE: reference to a compiler-generated field
          this.m_ID_WorldTextureSizeInvSize = Shader.PropertyToID("_WorldTextureSizeInvSize");
          // ISSUE: reference to a compiler-generated field
          this.m_ID_Result = Shader.PropertyToID("ResultMinMax");
        }
      }

      public void Debug(TerrainSystem terrain, Texture map, Texture worldMap)
      {
        using (CommandBuffer commandBuffer = new CommandBuffer()
        {
          name = "DebugMinMax"
        })
        {
          commandBuffer.SetExecutionFlags(CommandBufferExecutionFlags.AsyncCompute);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.RequestUpdate(terrain, map, worldMap, this.m_DebugArea, commandBuffer, true);
          Graphics.ExecuteCommandBuffer(commandBuffer);
          commandBuffer.Dispose();
        }
      }

      public void UpdateMap(TerrainSystem terrain, Texture map, Texture worldMap)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Valid = false;
        // ISSUE: reference to a compiler-generated field
        this.m_Updated = false;
        // ISSUE: reference to a compiler-generated field
        this.m_Partial = false;
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateJob.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateJob = new JobHandle();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Pending && !this.m_Current.done)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Current.WaitForCompletion();
        }
        using (CommandBuffer commandBuffer = new CommandBuffer()
        {
          name = "TerrainMinMaxInit"
        })
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AsyncNeeded = this.RequestUpdate(terrain, map, worldMap, new int4(0, 0, map.width, map.height), commandBuffer);
          Graphics.ExecuteCommandBuffer(commandBuffer);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Pending = true;
      }

      private int4 RemapArea(int4 area, int blockSize, int textureWidth, int textureHeight)
      {
        int2 int2 = area.xy / new int2(blockSize, blockSize) * new int2(blockSize, blockSize);
        area.zw += area.xy - int2;
        area.xy = int2;
        area.zw = (area.zw + new int2(blockSize - 1, blockSize - 1)) / new int2(blockSize, blockSize) * new int2(blockSize, blockSize);
        if (area.z > textureWidth)
          area.z = textureWidth;
        if (area.x + area.z > textureWidth)
          area.x = textureWidth - area.z;
        if (area.w > textureHeight)
          area.w = textureHeight;
        if (area.y + area.w > textureHeight)
          area.y = textureHeight - area.w;
        return area;
      }

      public bool RequestUpdate(TerrainSystem terrain, Texture map, Texture worldMap, int4 area)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Pending || this.m_Updated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UpdatesRequested.Add(area);
          // ISSUE: reference to a compiler-generated field
          this.m_TerrainSystem = terrain;
          return false;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int2 int2 = area.xy / new int2(this.m_BlockSize, this.m_BlockSize) * new int2(this.m_BlockSize, this.m_BlockSize);
        area.zw += area.xy - int2;
        area.xy = int2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        area.zw = (area.zw + new int2(this.m_BlockSize - 1, this.m_BlockSize - 1)) / new int2(this.m_BlockSize, this.m_BlockSize) * new int2(this.m_BlockSize, this.m_BlockSize);
        if (area.z > map.width)
          area.z = map.width;
        if (area.x + area.z > map.width)
          area.x = map.width - area.z;
        if (area.w > map.height)
          area.w = map.height;
        if (area.y + area.w > map.height)
          area.y = map.height - area.w;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        area = this.RemapArea(area, this.m_BlockSize, (UnityEngine.Object) worldMap != (UnityEngine.Object) null ? worldMap.width : map.width, (UnityEngine.Object) worldMap != (UnityEngine.Object) null ? worldMap.height : map.height);
        using (CommandBuffer commandBuffer = new CommandBuffer()
        {
          name = "TerainMinMaxUpdate"
        })
        {
          commandBuffer.SetExecutionFlags(CommandBufferExecutionFlags.AsyncCompute);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AsyncNeeded = this.RequestUpdate(terrain, map, worldMap, area, commandBuffer);
          // ISSUE: reference to a compiler-generated field
          this.m_Pending = true;
          // ISSUE: reference to a compiler-generated field
          this.m_Partial = true;
          Graphics.ExecuteCommandBuffer(commandBuffer);
        }
        return true;
      }

      private bool Downsample(
        CommandBuffer commandBuffer,
        Texture target,
        int steps,
        int4 area,
        ref int4 updated)
      {
        if (steps == 1)
          return false;
        float4 val = new float4((float) area.x, (float) area.y, (float) (area.x / 2), (float) (area.y / 2));
        int num = 1;
        int2 int2 = area.zw / 2;
        int4 int4 = area / 2;
        int4.zw = math.max(int4.zw, new int2(1, 1));
        int2.xy = math.max(int2.xy, new int2(1, 1));
        updated = area / 2;
        updated.zw = math.max(updated.zw, new int2(1, 1));
        // ISSUE: reference to a compiler-generated field
        Texture rt1 = (Texture) this.m_IntermediateTex[1];
        // ISSUE: reference to a compiler-generated field
        Texture rt2 = (Texture) this.m_IntermediateTex[0];
        do
        {
          Texture texture = rt2;
          rt2 = rt1;
          rt1 = texture;
          if (num == steps - 1)
            rt2 = target;
          val.xy = (float2) int4.xy;
          int4 /= 2;
          int4.zw = math.max(int4.zw, new int2(1, 1));
          val.zw = (float2) int4.xy;
          int2 /= 2;
          int2.xy = math.max(int2.xy, new int2(1, 1));
          updated /= 2;
          updated.zw = math.max(updated.zw, new int2(1, 1));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          commandBuffer.SetComputeVectorParam(this.m_Shader, this.m_ID_UpdateArea, (Vector4) val);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          commandBuffer.SetComputeTextureParam(this.m_Shader, this.m_KernalCSDownsampleMinMax, this.m_ID_WorldTexture, (RenderTargetIdentifier) rt1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          commandBuffer.SetComputeTextureParam(this.m_Shader, this.m_KernalCSDownsampleMinMax, this.m_ID_Result, (RenderTargetIdentifier) rt2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          commandBuffer.DispatchCompute(this.m_Shader, this.m_KernalCSDownsampleMinMax, (int2.x + 7) / 8, (int2.y + 7) / 8, 1);
        }
        while (++num < steps);
        return true;
      }

      private Texture RequestUpdate(
        TerrainSystem terrain,
        Texture map,
        Texture worldMap,
        int4 area,
        CommandBuffer commandBuffer,
        bool debug = false)
      {
        if (!debug)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_DebugArea = area;
        }
        int num = (UnityEngine.Object) worldMap != (UnityEngine.Object) null ? 1 : 0;
        float4 val1 = new float4((float) area.x, (float) area.y, (float) (area.x / 2), (float) (area.y / 2));
        float4 val2 = new float4(terrain.heightScaleOffset.y, terrain.heightScaleOffset.x, 0.0f, 0.0f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        commandBuffer.SetComputeVectorParam(this.m_Shader, this.m_ID_WorldOffsetScale, (Vector4) val2);
        int4 zero = int4.zero;
        if (num != 0)
        {
          float4 val3 = new float4((terrain.worldOffset - terrain.playableOffset) / terrain.playableArea, 1f / (float) worldMap.width * (terrain.worldSize / terrain.playableArea));
          float4 x = new float4((float2) area.xy * val3.zw + val3.xy, (float2) (area.xy + area.zw) * val3.zw + val3.xy);
          if (((double) x.x > 1.0 || (double) x.z < 0.0 || (double) x.y > 1.0 ? 0 : ((double) x.w >= 0.0 ? 1 : 0)) != 0)
          {
            float4 float4 = math.clamp(x, float4.zero, new float4(1f, 1f, 1f, 1f));
            float4.zw -= float4.xy;
            float4.xy = math.floor(float4.xy * new float2((float) map.width, (float) map.height));
            float4.zw = math.max(math.ceil(float4.zw * new float2((float) map.width, (float) map.height)), new float2(1f, 1f));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            int4 area1 = this.RemapArea(new int4((int) float4.x, (int) float4.y, (int) float4.z, (int) float4.w), this.m_DetailBlockSize, map.width, map.height);
            float4 val4 = new float4(float4.x, float4.y, float4.x / 2f, float4.y / 2f);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            commandBuffer.SetComputeVectorParam(this.m_Shader, this.m_ID_UpdateArea, (Vector4) val4);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            commandBuffer.SetComputeTextureParam(this.m_Shader, this.m_KernalCSTerainMinMax, this.m_ID_WorldTexture, (RenderTargetIdentifier) map);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            commandBuffer.SetComputeTextureParam(this.m_Shader, this.m_KernalCSTerainMinMax, this.m_ID_Result, (RenderTargetIdentifier) (Texture) this.m_IntermediateTex[0]);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            commandBuffer.DispatchCompute(this.m_Shader, this.m_KernalCSTerainMinMax, (area1.z + 7) / 8, (area1.w + 7) / 8, 1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.Downsample(commandBuffer, (Texture) this.m_DownsampledDetail, this.m_DetailSteps, area1, ref zero);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float4 val5 = new float4((float) this.m_DownsampledDetail.width, (float) this.m_DownsampledDetail.height, 1f / (float) this.m_DownsampledDetail.width, 1f / (float) this.m_DownsampledDetail.height);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          commandBuffer.SetComputeVectorParam(this.m_Shader, this.m_ID_WorldTextureSizeInvSize, (Vector4) val5);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          commandBuffer.SetComputeVectorParam(this.m_Shader, this.m_ID_DetailOffsetScale, (Vector4) val3);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          commandBuffer.SetComputeVectorParam(this.m_Shader, this.m_ID_UpdateArea, (Vector4) val1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          commandBuffer.SetComputeTextureParam(this.m_Shader, this.m_KernalCSWorldTerainMinMax, this.m_ID_WorldTexture, (RenderTargetIdentifier) worldMap);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          commandBuffer.SetComputeTextureParam(this.m_Shader, this.m_KernalCSWorldTerainMinMax, this.m_ID_DetailTexture, (RenderTargetIdentifier) (Texture) this.m_DownsampledDetail);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          commandBuffer.SetComputeTextureParam(this.m_Shader, this.m_KernalCSWorldTerainMinMax, this.m_ID_Result, (RenderTargetIdentifier) (this.m_Steps == 1 ? (Texture) this.m_ResultTex : (Texture) this.m_IntermediateTex[0]));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          commandBuffer.DispatchCompute(this.m_Shader, this.m_KernalCSWorldTerainMinMax, (area.z + 7) / 8, (area.w + 7) / 8, 1);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          commandBuffer.SetComputeVectorParam(this.m_Shader, this.m_ID_UpdateArea, (Vector4) val1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          commandBuffer.SetComputeTextureParam(this.m_Shader, this.m_KernalCSTerainMinMax, this.m_ID_WorldTexture, (RenderTargetIdentifier) map);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          commandBuffer.SetComputeTextureParam(this.m_Shader, this.m_KernalCSTerainMinMax, this.m_ID_Result, (RenderTargetIdentifier) (this.m_Steps == 1 ? (Texture) this.m_ResultTex : (Texture) this.m_IntermediateTex[0]));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          commandBuffer.DispatchCompute(this.m_Shader, this.m_KernalCSTerainMinMax, (area.z + 7) / 8, (area.w + 7) / 8, 1);
        }
        if (!debug)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UpdatedArea = area / 2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_UpdatedArea.zw = math.max(this.m_UpdatedArea.zw, new int2(1, 1));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Downsample(commandBuffer, (Texture) this.m_ResultTex, this.m_Steps, area, ref zero);
        if (!debug)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UpdatedArea = zero;
        }
        // ISSUE: reference to a compiler-generated field
        return (Texture) this.m_ResultTex;
      }

      public unsafe void Update()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateJob.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateJob = new JobHandle();
        // ISSUE: reference to a compiler-generated field
        if (this.m_Pending)
        {
          // ISSUE: reference to a compiler-generated field
          if ((UnityEngine.Object) this.m_AsyncNeeded != (UnityEngine.Object) null)
          {
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
            this.m_Current = !this.m_Partial ? AsyncGPUReadback.RequestIntoNativeArray<half4>(ref this.MinMaxMap, this.m_AsyncNeeded, 0, 0, this.m_ResultSize.x, 0, this.m_ResultSize.y, 0, 1, GraphicsFormat.R16G16B16A16_SFloat, (Action<AsyncGPUReadbackRequest>) (request =>
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Pending = false;
              if (request.hasError)
                return;
              // ISSUE: reference to a compiler-generated field
              this.m_Valid = true;
              // ISSUE: reference to a compiler-generated field
              this.m_Updated = true;
              // ISSUE: reference to a compiler-generated field
              if (!this.m_Partial)
                return;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int num = this.m_UpdatedArea.y * this.m_ResultSize.x + this.m_UpdatedArea.x;
              // ISSUE: reference to a compiler-generated field
              for (int index = 0; index < this.m_UpdatedArea.w; ++index)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                UnsafeUtility.MemCpy((void*) ((float2*) this.MinMaxMap.GetUnsafePtr<half4>() + num), (void*) ((float2*) this.m_UpdateBuffer.GetUnsafePtr<half4>() + index * this.m_UpdatedArea.z), (long) (8 * this.m_UpdatedArea.z));
                // ISSUE: reference to a compiler-generated field
                num += this.m_ResultSize.x;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_Partial = false;
            })) : AsyncGPUReadback.RequestIntoNativeArray<half4>(ref this.m_UpdateBuffer, this.m_AsyncNeeded, 0, this.m_UpdatedArea.x, this.m_UpdatedArea.z, this.m_UpdatedArea.y, this.m_UpdatedArea.w, 0, 1, GraphicsFormat.R16G16B16A16_SFloat, (Action<AsyncGPUReadbackRequest>) (request =>
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Pending = false;
              if (request.hasError)
                return;
              // ISSUE: reference to a compiler-generated field
              this.m_Valid = true;
              // ISSUE: reference to a compiler-generated field
              this.m_Updated = true;
              // ISSUE: reference to a compiler-generated field
              if (!this.m_Partial)
                return;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int num = this.m_UpdatedArea.y * this.m_ResultSize.x + this.m_UpdatedArea.x;
              // ISSUE: reference to a compiler-generated field
              for (int index = 0; index < this.m_UpdatedArea.w; ++index)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                UnsafeUtility.MemCpy((void*) ((float2*) this.MinMaxMap.GetUnsafePtr<half4>() + num), (void*) ((float2*) this.m_UpdateBuffer.GetUnsafePtr<half4>() + index * this.m_UpdatedArea.z), (long) (8 * this.m_UpdatedArea.z));
                // ISSUE: reference to a compiler-generated field
                num += this.m_ResultSize.x;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_Partial = false;
            }));
            // ISSUE: reference to a compiler-generated field
            this.m_AsyncNeeded = (Texture) null;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Current.Update();
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Updated || this.m_UpdatesRequested.Count <= 0)
            return;
          // ISSUE: reference to a compiler-generated field
          int4 area = this.m_UpdatesRequested[0];
          area.zw += area.xy;
          // ISSUE: reference to a compiler-generated field
          for (int index = 1; index < this.m_UpdatesRequested.Count; ++index)
          {
            ref int4 local1 = ref area;
            int2 xy1 = area.xy;
            // ISSUE: reference to a compiler-generated field
            int4 int4 = this.m_UpdatesRequested[index];
            int2 xy2 = int4.xy;
            int2 int2_1 = math.min(xy1, xy2);
            local1.xy = int2_1;
            ref int4 local2 = ref area;
            int2 zw1 = area.zw;
            // ISSUE: reference to a compiler-generated field
            int4 = this.m_UpdatesRequested[index];
            int2 xy3 = int4.xy;
            // ISSUE: reference to a compiler-generated field
            int4 = this.m_UpdatesRequested[index];
            int2 zw2 = int4.zw;
            int2 y = xy3 + zw2;
            int2 int2_2 = math.max(zw1, y);
            local2.zw = int2_2;
          }
          area.zw -= area.xy;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          area.zw = math.clamp(area.zw, new int2(1, 1), new int2(this.m_ResultSize.x, this.m_ResultSize.y));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.RequestUpdate(this.m_TerrainSystem, this.m_TerrainSystem.heightmap, this.m_TerrainSystem.worldHeightmap, area);
          // ISSUE: reference to a compiler-generated field
          this.m_TerrainSystem = (TerrainSystem) null;
          // ISSUE: reference to a compiler-generated field
          this.m_UpdatesRequested.Clear();
        }
      }

      private unsafe void UpdateMinMax(AsyncGPUReadbackRequest request)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Pending = false;
        if (request.hasError)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_Valid = true;
        // ISSUE: reference to a compiler-generated field
        this.m_Updated = true;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Partial)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num = this.m_UpdatedArea.y * this.m_ResultSize.x + this.m_UpdatedArea.x;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_UpdatedArea.w; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          UnsafeUtility.MemCpy((void*) ((float2*) this.MinMaxMap.GetUnsafePtr<half4>() + num), (void*) ((float2*) this.m_UpdateBuffer.GetUnsafePtr<half4>() + index * this.m_UpdatedArea.z), (long) (8 * this.m_UpdatedArea.z));
          // ISSUE: reference to a compiler-generated field
          num += this.m_ResultSize.x;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Partial = false;
      }

      public bool isValid => this.m_Valid;

      public bool isUpdated => this.m_Updated;

      public int size => this.m_ResultSize.x;

      public int4 ComsumeUpdate()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Updated = false;
        // ISSUE: reference to a compiler-generated field
        return this.m_UpdatedArea;
      }

      public int4 UpdateArea => this.m_UpdatedArea;

      public float2 GetMinMax(int4 area)
      {
        float2 minMax = new float2(999999f, 0.0f);
        for (int index1 = 0; index1 < area.z * area.w; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          int index2 = (area.y + index1 / area.z) * this.m_ResultSize.x + area.x + index1 % area.z;
          // ISSUE: reference to a compiler-generated field
          minMax.x = math.min(minMax.x, (float) this.MinMaxMap[index2].x);
          // ISSUE: reference to a compiler-generated field
          minMax.y = math.max(minMax.y, (float) this.MinMaxMap[index2].y);
        }
        return minMax;
      }

      public void RegisterJobUpdate(JobHandle handle)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateJob = JobHandle.CombineDependencies(handle, this.m_UpdateJob);
      }

      public void Dispose()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Current.WaitForCompletion();
        // ISSUE: reference to a compiler-generated field
        this.m_Pending = false;
        // ISSUE: reference to a compiler-generated field
        this.m_AsyncNeeded = (Texture) null;
        // ISSUE: reference to a compiler-generated field
        this.m_Updated = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_IntermediateTex != null)
        {
          // ISSUE: reference to a compiler-generated field
          foreach (UnityEngine.Object @object in this.m_IntermediateTex)
            CoreUtils.Destroy(@object);
        }
        // ISSUE: reference to a compiler-generated field
        CoreUtils.Destroy((UnityEngine.Object) this.m_DownsampledDetail);
        // ISSUE: reference to a compiler-generated field
        CoreUtils.Destroy((UnityEngine.Object) this.m_ResultTex);
        // ISSUE: reference to a compiler-generated field
        if (this.MinMaxMap.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.MinMaxMap.Dispose(this.m_UpdateJob);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_UpdateBuffer.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateBuffer.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateJob = new JobHandle();
      }
    }

    private class TerrainDesc
    {
      public Colossal.Hash128 heightMapGuid { get; set; }

      public Colossal.Hash128 diffuseMapGuid { get; set; }

      public float heightScale { get; set; }

      public float heightOffset { get; set; }

      public Colossal.Hash128 worldHeightMapGuid { get; set; }

      public float2 mapSize { get; set; }

      public float2 worldSize { get; set; }

      public float2 worldHeightMinMax { get; set; }

      private static void SupportValueTypesForAOT() => JSON.SupportTypeForAOT<float2>();
    }

    [BurstCompile]
    private struct CullBuildingLotsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Lot> m_LotHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Elevation> m_ElevationHandle;
      [ReadOnly]
      public ComponentTypeHandle<Stack> m_StackHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<AssetStampData> m_PrefabAssetStampData;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> m_PrefabBuildingExtensionData;
      [ReadOnly]
      public ComponentLookup<BuildingTerraformData> m_OverrideTerraform;
      [ReadOnly]
      public BufferLookup<AdditionalBuildingTerraformElement> m_AdditionalLots;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      [ReadOnly]
      public float4 m_Area;
      public NativeQueue<BuildingUtils.LotInfo>.ParallelWriter Result;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.Lot> nativeArray1 = chunk.GetNativeArray<Game.Buildings.Lot>(ref this.m_LotHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Transform> nativeArray2 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Elevation> nativeArray3 = chunk.GetNativeArray<Game.Objects.Elevation>(ref this.m_ElevationHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Stack> nativeArray4 = chunk.GetNativeArray<Stack>(ref this.m_StackHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefHandle);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeHandle);
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          PrefabRef prefabRef1 = nativeArray5[index1];
          Game.Objects.Transform transform = nativeArray2[index1];
          Game.Objects.Elevation elevation = new Game.Objects.Elevation();
          if (nativeArray3.Length != 0)
            elevation = nativeArray3[index1];
          Game.Buildings.Lot lot = new Game.Buildings.Lot();
          if (nativeArray1.Length != 0)
            lot = nativeArray1[index1];
          // ISSUE: reference to a compiler-generated field
          bool flag1 = this.m_PrefabBuildingData.HasComponent(prefabRef1.m_Prefab);
          // ISSUE: reference to a compiler-generated field
          bool flag2 = !flag1 && this.m_PrefabBuildingExtensionData.HasComponent(prefabRef1.m_Prefab);
          // ISSUE: reference to a compiler-generated field
          bool flag3 = !flag1 && !flag2 && this.m_PrefabAssetStampData.HasComponent(prefabRef1.m_Prefab);
          // ISSUE: reference to a compiler-generated field
          bool defaultNoSmooth = !flag1 && !flag2 && !flag3 && this.m_ObjectGeometryData.HasComponent(prefabRef1.m_Prefab);
          if (flag1 | flag2 | flag3 | defaultNoSmooth)
          {
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_ObjectGeometryData[prefabRef1.m_Prefab];
            Bounds2 xz = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, objectGeometryData).xz;
            float2 extents;
            if (flag1)
            {
              // ISSUE: reference to a compiler-generated field
              extents = new float2(this.m_PrefabBuildingData[prefabRef1.m_Prefab].m_LotSize) * 4f;
            }
            else if (flag2)
            {
              // ISSUE: reference to a compiler-generated field
              BuildingExtensionData buildingExtensionData = this.m_PrefabBuildingExtensionData[prefabRef1.m_Prefab];
              if (buildingExtensionData.m_External)
                extents = new float2(buildingExtensionData.m_LotSize) * 4f;
              else
                continue;
            }
            else if (flag3)
            {
              // ISSUE: reference to a compiler-generated field
              extents = new float2(this.m_PrefabAssetStampData[prefabRef1.m_Prefab].m_Size) * 4f;
            }
            else
            {
              if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Standing) != Game.Objects.GeometryFlags.None)
              {
                extents = objectGeometryData.m_LegSize.xz * 0.5f;
              }
              else
              {
                transform.m_Position.xz += MathUtils.Center(objectGeometryData.m_Bounds.xz);
                extents = MathUtils.Size(objectGeometryData.m_Bounds.xz) * 0.5f;
              }
              if (nativeArray4.Length != 0)
              {
                Stack stack = nativeArray4[index1];
                transform.m_Position.y += stack.m_Range.min - objectGeometryData.m_Bounds.min.y;
              }
            }
            Bounds2 bounds2 = MathUtils.Expand(xz, (float2) (ObjectUtils.GetTerrainSmoothingWidth(objectGeometryData) - 8f));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((double) bounds2.max.x >= (double) this.m_Area.x && (double) bounds2.min.x <= (double) this.m_Area.z && (double) bounds2.max.y >= (double) this.m_Area.y && (double) bounds2.min.y <= (double) this.m_Area.w)
            {
              DynamicBuffer<InstalledUpgrade> upgrades = new DynamicBuffer<InstalledUpgrade>();
              if (bufferAccessor.Length != 0)
                upgrades = bufferAccessor[index1];
              bool hasExtensionLots;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              BuildingUtils.LotInfo lotInfo1 = BuildingUtils.CalculateLotInfo(extents, transform, elevation, lot, prefabRef1, upgrades, this.m_TransformData, this.m_PrefabRefData, this.m_ObjectGeometryData, this.m_OverrideTerraform, this.m_PrefabBuildingExtensionData, defaultNoSmooth, out hasExtensionLots);
              float terrainSmoothingWidth1 = ObjectUtils.GetTerrainSmoothingWidth(extents * 2f);
              lotInfo1.m_Radius += terrainSmoothingWidth1;
              // ISSUE: reference to a compiler-generated field
              this.Result.Enqueue(lotInfo1);
              if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Standing) != Game.Objects.GeometryFlags.None)
              {
                BuildingUtils.LotInfo lotInfo2 = lotInfo1 with
                {
                  m_Extents = MathUtils.Size(objectGeometryData.m_Bounds.xz) * 0.5f
                };
                float terrainSmoothingWidth2 = ObjectUtils.GetTerrainSmoothingWidth(lotInfo2.m_Extents * 2f);
                lotInfo2.m_Position.xz += MathUtils.Center(objectGeometryData.m_Bounds.xz);
                lotInfo2.m_Position.y += objectGeometryData.m_LegSize.y;
                lotInfo2.m_MaxLimit = new float4(terrainSmoothingWidth2, terrainSmoothingWidth2, -terrainSmoothingWidth2, -terrainSmoothingWidth2);
                lotInfo2.m_MinLimit = new float4(-lotInfo2.m_Extents.xy, lotInfo2.m_Extents.xy);
                lotInfo2.m_FrontHeights = new float3();
                lotInfo2.m_RightHeights = new float3();
                lotInfo2.m_BackHeights = new float3();
                lotInfo2.m_LeftHeights = new float3();
                lotInfo2.m_FlatX0 = (float3) (lotInfo2.m_MinLimit.x * 0.5f);
                lotInfo2.m_FlatZ0 = (float3) (lotInfo2.m_MinLimit.y * 0.5f);
                lotInfo2.m_FlatX1 = (float3) (lotInfo2.m_MinLimit.z * 0.5f);
                lotInfo2.m_FlatZ1 = (float3) (lotInfo2.m_MinLimit.w * 0.5f);
                lotInfo2.m_Radius = math.length(lotInfo2.m_Extents) + terrainSmoothingWidth2;
                lotInfo2.m_Circular = math.select(0.0f, 1f, (objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Circular) != 0);
                // ISSUE: reference to a compiler-generated field
                this.Result.Enqueue(lotInfo2);
              }
              DynamicBuffer<AdditionalBuildingTerraformElement> bufferData1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_AdditionalLots.TryGetBuffer(prefabRef1.m_Prefab, out bufferData1))
              {
                for (int index2 = 0; index2 < bufferData1.Length; ++index2)
                {
                  AdditionalBuildingTerraformElement terraformElement = bufferData1[index2];
                  BuildingUtils.LotInfo lotInfo3 = lotInfo1;
                  lotInfo3.m_Position.y += terraformElement.m_HeightOffset;
                  lotInfo3.m_MinLimit = new float4(terraformElement.m_Area.min, terraformElement.m_Area.max);
                  lotInfo3.m_FlatX0 = math.max(lotInfo3.m_FlatX0, (float3) lotInfo3.m_MinLimit.x);
                  lotInfo3.m_FlatZ0 = math.max(lotInfo3.m_FlatZ0, (float3) lotInfo3.m_MinLimit.y);
                  lotInfo3.m_FlatX1 = math.min(lotInfo3.m_FlatX1, (float3) lotInfo3.m_MinLimit.z);
                  lotInfo3.m_FlatZ1 = math.min(lotInfo3.m_FlatZ1, (float3) lotInfo3.m_MinLimit.w);
                  lotInfo3.m_Circular = math.select(0.0f, 1f, terraformElement.m_Circular);
                  lotInfo3.m_MaxLimit = math.select(lotInfo3.m_MinLimit, new float4(terrainSmoothingWidth1, terrainSmoothingWidth1, -terrainSmoothingWidth1, -terrainSmoothingWidth1), terraformElement.m_DontRaise);
                  lotInfo3.m_MinLimit = math.select(lotInfo3.m_MinLimit, new float4(terrainSmoothingWidth1, terrainSmoothingWidth1, -terrainSmoothingWidth1, -terrainSmoothingWidth1), terraformElement.m_DontLower);
                  // ISSUE: reference to a compiler-generated field
                  this.Result.Enqueue(lotInfo3);
                }
              }
              if (hasExtensionLots)
              {
                for (int index3 = 0; index3 < upgrades.Length; ++index3)
                {
                  Entity upgrade = upgrades[index3].m_Upgrade;
                  // ISSUE: reference to a compiler-generated field
                  PrefabRef prefabRef2 = this.m_PrefabRefData[upgrade];
                  BuildingExtensionData componentData1;
                  BuildingTerraformData componentData2;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabBuildingExtensionData.TryGetComponent(prefabRef2.m_Prefab, out componentData1) && !componentData1.m_External && this.m_OverrideTerraform.TryGetComponent(prefabRef2.m_Prefab, out componentData2))
                  {
                    // ISSUE: reference to a compiler-generated field
                    float3 float3 = this.m_TransformData[upgrade].m_Position - transform.m_Position;
                    float num = 0.0f;
                    ObjectGeometryData componentData3;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_ObjectGeometryData.TryGetComponent(prefabRef2.m_Prefab, out componentData3))
                    {
                      bool flag4 = (componentData3.m_Flags & Game.Objects.GeometryFlags.Standing) != 0;
                      num = math.select(0.0f, 1f, (componentData3.m_Flags & (flag4 ? Game.Objects.GeometryFlags.CircularLeg : Game.Objects.GeometryFlags.Circular)) != 0);
                    }
                    if (!math.all(componentData2.m_Smooth + float3.xzxz == lotInfo1.m_MaxLimit) || (double) num != (double) lotInfo1.m_Circular)
                    {
                      BuildingUtils.LotInfo lotInfo4 = lotInfo1 with
                      {
                        m_Circular = num
                      };
                      lotInfo4.m_Position.y += componentData2.m_HeightOffset;
                      lotInfo4.m_MinLimit = componentData2.m_Smooth + float3.xzxz;
                      lotInfo4.m_MaxLimit = lotInfo4.m_MinLimit;
                      lotInfo4.m_MinLimit.xy = math.min(new float2(lotInfo4.m_FlatX0.y, lotInfo4.m_FlatZ0.y), lotInfo4.m_MinLimit.xy);
                      lotInfo4.m_MinLimit.zw = math.max(new float2(lotInfo4.m_FlatX1.y, lotInfo4.m_FlatZ1.y), lotInfo4.m_MinLimit.zw);
                      lotInfo4.m_MinLimit = math.select(lotInfo4.m_MinLimit, new float4(terrainSmoothingWidth1, terrainSmoothingWidth1, -terrainSmoothingWidth1, -terrainSmoothingWidth1), componentData2.m_DontLower);
                      lotInfo4.m_MaxLimit = math.select(lotInfo4.m_MaxLimit, new float4(terrainSmoothingWidth1, terrainSmoothingWidth1, -terrainSmoothingWidth1, -terrainSmoothingWidth1), componentData2.m_DontRaise);
                      // ISSUE: reference to a compiler-generated field
                      this.Result.Enqueue(lotInfo4);
                    }
                    DynamicBuffer<AdditionalBuildingTerraformElement> bufferData2;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_AdditionalLots.TryGetBuffer(prefabRef2.m_Prefab, out bufferData2))
                    {
                      for (int index4 = 0; index4 < bufferData2.Length; ++index4)
                      {
                        AdditionalBuildingTerraformElement terraformElement = bufferData2[index4];
                        BuildingUtils.LotInfo lotInfo5 = lotInfo1;
                        lotInfo5.m_Position.y += terraformElement.m_HeightOffset;
                        lotInfo5.m_MinLimit = new float4(terraformElement.m_Area.min, terraformElement.m_Area.max) + float3.xzxz;
                        lotInfo5.m_FlatX0 = math.max(lotInfo5.m_FlatX0, (float3) lotInfo5.m_MinLimit.x);
                        lotInfo5.m_FlatZ0 = math.max(lotInfo5.m_FlatZ0, (float3) lotInfo5.m_MinLimit.y);
                        lotInfo5.m_FlatX1 = math.min(lotInfo5.m_FlatX1, (float3) lotInfo5.m_MinLimit.z);
                        lotInfo5.m_FlatZ1 = math.min(lotInfo5.m_FlatZ1, (float3) lotInfo5.m_MinLimit.w);
                        lotInfo5.m_Circular = math.select(0.0f, 1f, terraformElement.m_Circular);
                        lotInfo5.m_MaxLimit = math.select(lotInfo5.m_MinLimit, new float4(terrainSmoothingWidth1, terrainSmoothingWidth1, -terrainSmoothingWidth1, -terrainSmoothingWidth1), terraformElement.m_DontRaise);
                        lotInfo5.m_MinLimit = math.select(lotInfo5.m_MinLimit, new float4(terrainSmoothingWidth1, terrainSmoothingWidth1, -terrainSmoothingWidth1, -terrainSmoothingWidth1), terraformElement.m_DontLower);
                        // ISSUE: reference to a compiler-generated field
                        this.Result.Enqueue(lotInfo5);
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    [BurstCompile]
    private struct DequeBuildingLotsJob : IJob
    {
      [ReadOnly]
      public NativeQueue<BuildingUtils.LotInfo> m_Queue;
      public NativeList<BuildingUtils.LotInfo> m_List;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<BuildingUtils.LotInfo> other = this.m_Queue.ToArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        this.m_List.CopyFrom(in other);
        other.Dispose();
      }
    }

    [BurstCompile]
    private struct CullRoadsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityHandle;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<Orphan> m_OrphanData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> m_NodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetData> m_NetData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_NetGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<TerrainComposition> m_TerrainCompositionData;
      [ReadOnly]
      public float4 m_Area;
      public NativeList<TerrainSystem.LaneSection>.ParallelWriter Result;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityHandle);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          Entity entity = nativeArray[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            Entity prefab = this.m_PrefabRefData[entity].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            if (this.m_NetGeometryData.HasComponent(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              NetData net = this.m_NetData[prefab];
              // ISSUE: reference to a compiler-generated field
              NetGeometryData netGeometry = this.m_NetGeometryData[prefab];
              // ISSUE: reference to a compiler-generated field
              if (this.m_CompositionData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                Composition composition = this.m_CompositionData[entity];
                // ISSUE: reference to a compiler-generated field
                EdgeGeometry geometry = this.m_EdgeGeometryData[entity];
                // ISSUE: reference to a compiler-generated field
                StartNodeGeometry startNodeGeometry = this.m_StartNodeGeometryData[entity];
                // ISSUE: reference to a compiler-generated field
                EndNodeGeometry endNodeGeometry = this.m_EndNodeGeometryData[entity];
                if (math.any(geometry.m_Start.m_Length + geometry.m_End.m_Length > 0.1f))
                {
                  // ISSUE: reference to a compiler-generated field
                  NetCompositionData prefabCompositionData = this.m_PrefabCompositionData[composition.m_Edge];
                  TerrainComposition terrainComposition = new TerrainComposition();
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_TerrainCompositionData.HasComponent(composition.m_Edge))
                  {
                    // ISSUE: reference to a compiler-generated field
                    terrainComposition = this.m_TerrainCompositionData[composition.m_Edge];
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.AddEdge(geometry, this.m_Area, net, netGeometry, prefabCompositionData, terrainComposition);
                }
                if (math.any(startNodeGeometry.m_Geometry.m_Left.m_Length > 0.05f) | math.any(startNodeGeometry.m_Geometry.m_Right.m_Length > 0.05f))
                {
                  // ISSUE: reference to a compiler-generated field
                  NetCompositionData prefabCompositionData = this.m_PrefabCompositionData[composition.m_StartNode];
                  TerrainComposition terrainComposition = new TerrainComposition();
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_TerrainCompositionData.HasComponent(composition.m_StartNode))
                  {
                    // ISSUE: reference to a compiler-generated field
                    terrainComposition = this.m_TerrainCompositionData[composition.m_StartNode];
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.AddNode(startNodeGeometry.m_Geometry, this.m_Area, net, netGeometry, prefabCompositionData, terrainComposition);
                }
                if (math.any(endNodeGeometry.m_Geometry.m_Left.m_Length > 0.05f) | math.any(endNodeGeometry.m_Geometry.m_Right.m_Length > 0.05f))
                {
                  // ISSUE: reference to a compiler-generated field
                  NetCompositionData prefabCompositionData = this.m_PrefabCompositionData[composition.m_EndNode];
                  TerrainComposition terrainComposition = new TerrainComposition();
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_TerrainCompositionData.HasComponent(composition.m_EndNode))
                  {
                    // ISSUE: reference to a compiler-generated field
                    terrainComposition = this.m_TerrainCompositionData[composition.m_EndNode];
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.AddNode(endNodeGeometry.m_Geometry, this.m_Area, net, netGeometry, prefabCompositionData, terrainComposition);
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_OrphanData.HasComponent(entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  Orphan orphan = this.m_OrphanData[entity];
                  // ISSUE: reference to a compiler-generated field
                  Game.Net.Node node = this.m_NodeData[entity];
                  // ISSUE: reference to a compiler-generated field
                  NetCompositionData prefabCompositionData = this.m_PrefabCompositionData[orphan.m_Composition];
                  TerrainComposition terrainComposition = new TerrainComposition();
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_TerrainCompositionData.HasComponent(orphan.m_Composition))
                  {
                    // ISSUE: reference to a compiler-generated field
                    terrainComposition = this.m_TerrainCompositionData[orphan.m_Composition];
                  }
                  // ISSUE: reference to a compiler-generated field
                  NodeGeometry nodeGeometry = this.m_NodeGeometryData[entity];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.AddOrphans(node, nodeGeometry, this.m_Area, net, netGeometry, prefabCompositionData, terrainComposition);
                }
              }
            }
          }
        }
      }

      private TerrainSystem.LaneFlags GetFlags(
        NetGeometryData netGeometry,
        NetCompositionData prefabCompositionData)
      {
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.LaneFlags flags = (TerrainSystem.LaneFlags) 0;
        if ((netGeometry.m_Flags & Game.Net.GeometryFlags.ClipTerrain) != (Game.Net.GeometryFlags) 0)
          flags |= TerrainSystem.LaneFlags.ClipTerrain;
        if ((netGeometry.m_Flags & Game.Net.GeometryFlags.FlattenTerrain) != (Game.Net.GeometryFlags) 0)
          flags |= TerrainSystem.LaneFlags.ShiftTerrain;
        return flags;
      }

      private void AddEdge(
        EdgeGeometry geometry,
        float4 area,
        NetData net,
        NetGeometryData netGeometry,
        NetCompositionData prefabCompositionData,
        TerrainComposition terrainComposition)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.LaneFlags flags = this.GetFlags(netGeometry, prefabCompositionData);
        if ((flags & (TerrainSystem.LaneFlags.ShiftTerrain | TerrainSystem.LaneFlags.ClipTerrain)) == (TerrainSystem.LaneFlags) 0)
          return;
        Bounds2 xz = geometry.m_Bounds.xz;
        if (math.any(xz.max < area.xy) || math.any(xz.min > area.zw))
          return;
        if ((prefabCompositionData.m_Flags.m_General & CompositionFlags.General.Tunnel) != (CompositionFlags.General) 0)
          flags |= TerrainSystem.LaneFlags.InverseClipOffset;
        // ISSUE: reference to a compiler-generated method
        this.AddSegment(geometry.m_Start, net, netGeometry, prefabCompositionData, terrainComposition, flags, true);
        // ISSUE: reference to a compiler-generated method
        this.AddSegment(geometry.m_End, net, netGeometry, prefabCompositionData, terrainComposition, flags, false);
      }

      private void MoveTowards(ref float3 position, float3 other, float amount)
      {
        float3 float3_1 = other - position;
        float3 float3_2 = MathUtils.Normalize(float3_1, float3_1.xz);
        position += float3_2 * amount;
      }

      private void AddNode(
        EdgeNodeGeometry node,
        float4 area,
        NetData net,
        NetGeometryData netGeometry,
        NetCompositionData prefabCompositionData,
        TerrainComposition terrainComposition)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.LaneFlags flags = this.GetFlags(netGeometry, prefabCompositionData);
        if ((flags & (TerrainSystem.LaneFlags.ShiftTerrain | TerrainSystem.LaneFlags.ClipTerrain)) == (TerrainSystem.LaneFlags) 0)
          return;
        Bounds2 xz = node.m_Bounds.xz;
        if (math.any(xz.max < area.xy) || math.any(xz.min > area.zw))
          return;
        if ((double) node.m_MiddleRadius > 0.0)
        {
          NetCompositionData compositionData = prefabCompositionData;
          float amount1 = 0.0f;
          float amount2 = 0.0f;
          if ((prefabCompositionData.m_Flags.m_General & CompositionFlags.General.Elevated) != (CompositionFlags.General) 0)
          {
            if ((prefabCompositionData.m_Flags.m_Left & CompositionFlags.Side.HighTransition) != (CompositionFlags.Side) 0)
            {
              amount1 = prefabCompositionData.m_SyncVertexOffsetsLeft.x;
              compositionData.m_Flags.m_General &= ~CompositionFlags.General.Elevated;
              compositionData.m_Flags.m_Left &= ~CompositionFlags.Side.HighTransition;
            }
            else if ((prefabCompositionData.m_Flags.m_Left & CompositionFlags.Side.LowTransition) != (CompositionFlags.Side) 0)
            {
              amount1 = prefabCompositionData.m_SyncVertexOffsetsLeft.x;
              compositionData.m_Flags.m_General &= ~CompositionFlags.General.Elevated;
              compositionData.m_Flags.m_Left &= ~CompositionFlags.Side.LowTransition;
              compositionData.m_Flags.m_Left |= CompositionFlags.Side.Raised;
            }
            if ((prefabCompositionData.m_Flags.m_Right & CompositionFlags.Side.HighTransition) != (CompositionFlags.Side) 0)
            {
              amount2 = 1f - prefabCompositionData.m_SyncVertexOffsetsRight.w;
              compositionData.m_Flags.m_General &= ~CompositionFlags.General.Elevated;
              compositionData.m_Flags.m_Right &= ~CompositionFlags.Side.HighTransition;
            }
            else if ((prefabCompositionData.m_Flags.m_Right & CompositionFlags.Side.LowTransition) != (CompositionFlags.Side) 0)
            {
              amount2 = 1f - prefabCompositionData.m_SyncVertexOffsetsRight.w;
              compositionData.m_Flags.m_General &= ~CompositionFlags.General.Elevated;
              compositionData.m_Flags.m_Right &= ~CompositionFlags.Side.LowTransition;
              compositionData.m_Flags.m_Right |= CompositionFlags.Side.Raised;
            }
          }
          else if ((prefabCompositionData.m_Flags.m_General & CompositionFlags.General.Tunnel) != (CompositionFlags.General) 0)
          {
            flags |= TerrainSystem.LaneFlags.InverseClipOffset;
            if ((prefabCompositionData.m_Flags.m_Left & CompositionFlags.Side.HighTransition) != (CompositionFlags.Side) 0)
            {
              amount1 = prefabCompositionData.m_SyncVertexOffsetsLeft.x;
              compositionData.m_Flags.m_General &= ~CompositionFlags.General.Tunnel;
              compositionData.m_Flags.m_Left &= ~CompositionFlags.Side.HighTransition;
              flags &= ~TerrainSystem.LaneFlags.InverseClipOffset;
            }
            else if ((prefabCompositionData.m_Flags.m_Left & CompositionFlags.Side.LowTransition) != (CompositionFlags.Side) 0)
            {
              amount1 = prefabCompositionData.m_SyncVertexOffsetsLeft.x;
              compositionData.m_Flags.m_General &= ~CompositionFlags.General.Tunnel;
              compositionData.m_Flags.m_Left &= ~CompositionFlags.Side.LowTransition;
              compositionData.m_Flags.m_Left |= CompositionFlags.Side.Lowered;
              flags &= ~TerrainSystem.LaneFlags.InverseClipOffset;
            }
            if ((prefabCompositionData.m_Flags.m_Right & CompositionFlags.Side.HighTransition) != (CompositionFlags.Side) 0)
            {
              amount2 = 1f - prefabCompositionData.m_SyncVertexOffsetsRight.w;
              compositionData.m_Flags.m_General &= ~CompositionFlags.General.Tunnel;
              compositionData.m_Flags.m_Right &= ~CompositionFlags.Side.HighTransition;
              flags &= ~TerrainSystem.LaneFlags.InverseClipOffset;
            }
            else if ((prefabCompositionData.m_Flags.m_Right & CompositionFlags.Side.LowTransition) != (CompositionFlags.Side) 0)
            {
              amount2 = 1f - prefabCompositionData.m_SyncVertexOffsetsRight.w;
              compositionData.m_Flags.m_General &= ~CompositionFlags.General.Tunnel;
              compositionData.m_Flags.m_Right &= ~CompositionFlags.Side.LowTransition;
              compositionData.m_Flags.m_Right |= CompositionFlags.Side.Lowered;
              flags &= ~TerrainSystem.LaneFlags.InverseClipOffset;
            }
          }
          else
          {
            if ((prefabCompositionData.m_Flags.m_Left & CompositionFlags.Side.LowTransition) != (CompositionFlags.Side) 0)
            {
              if ((prefabCompositionData.m_Flags.m_Left & CompositionFlags.Side.Raised) != (CompositionFlags.Side) 0)
              {
                amount1 = prefabCompositionData.m_SyncVertexOffsetsLeft.x;
                compositionData.m_Flags.m_Left &= ~(CompositionFlags.Side.Raised | CompositionFlags.Side.LowTransition);
              }
              else if ((prefabCompositionData.m_Flags.m_Left & CompositionFlags.Side.Lowered) != (CompositionFlags.Side) 0)
              {
                amount1 = prefabCompositionData.m_SyncVertexOffsetsLeft.x;
                compositionData.m_Flags.m_Left &= ~(CompositionFlags.Side.Lowered | CompositionFlags.Side.LowTransition);
              }
              else if ((prefabCompositionData.m_Flags.m_Left & CompositionFlags.Side.SoundBarrier) != (CompositionFlags.Side) 0)
              {
                amount1 = prefabCompositionData.m_SyncVertexOffsetsLeft.x;
                compositionData.m_Flags.m_Left &= ~(CompositionFlags.Side.LowTransition | CompositionFlags.Side.SoundBarrier);
              }
            }
            if ((prefabCompositionData.m_Flags.m_Right & CompositionFlags.Side.LowTransition) != (CompositionFlags.Side) 0)
            {
              if ((prefabCompositionData.m_Flags.m_Right & CompositionFlags.Side.Raised) != (CompositionFlags.Side) 0)
              {
                amount2 = 1f - prefabCompositionData.m_SyncVertexOffsetsRight.w;
                compositionData.m_Flags.m_Right &= ~(CompositionFlags.Side.Raised | CompositionFlags.Side.LowTransition);
              }
              else if ((prefabCompositionData.m_Flags.m_Right & CompositionFlags.Side.Lowered) != (CompositionFlags.Side) 0)
              {
                amount2 = 1f - prefabCompositionData.m_SyncVertexOffsetsRight.w;
                compositionData.m_Flags.m_Right &= ~(CompositionFlags.Side.Lowered | CompositionFlags.Side.LowTransition);
              }
              else if ((prefabCompositionData.m_Flags.m_Right & CompositionFlags.Side.SoundBarrier) != (CompositionFlags.Side) 0)
              {
                amount2 = 1f - prefabCompositionData.m_SyncVertexOffsetsRight.w;
                compositionData.m_Flags.m_Right &= ~(CompositionFlags.Side.LowTransition | CompositionFlags.Side.SoundBarrier);
              }
            }
          }
          if ((double) amount1 != 0.0)
            amount1 *= math.distance(node.m_Left.m_Left.a.xz, node.m_Middle.a.xz);
          if ((double) amount2 != 0.0)
            amount2 *= math.distance(node.m_Middle.a.xz, node.m_Left.m_Right.a.xz);
          Game.Net.Segment left = node.m_Left with
          {
            m_Right = node.m_Middle
          };
          // ISSUE: reference to a compiler-generated method
          this.AddSegment(left, net, netGeometry, prefabCompositionData, terrainComposition, flags | TerrainSystem.LaneFlags.MiddleRight, true);
          left.m_Left = left.m_Right;
          left.m_Right = node.m_Left.m_Right;
          // ISSUE: reference to a compiler-generated method
          this.AddSegment(left, net, netGeometry, prefabCompositionData, terrainComposition, flags | TerrainSystem.LaneFlags.MiddleLeft, true);
          Game.Net.Segment right = node.m_Right with
          {
            m_Right = new Bezier4x3(node.m_Middle.d, node.m_Middle.d, node.m_Middle.d, node.m_Middle.d)
          };
          if ((double) amount1 != 0.0)
          {
            // ISSUE: reference to a compiler-generated method
            this.MoveTowards(ref right.m_Left.a, node.m_Middle.d, amount1);
            // ISSUE: reference to a compiler-generated method
            this.MoveTowards(ref right.m_Left.b, node.m_Middle.d, amount1);
            // ISSUE: reference to a compiler-generated method
            this.MoveTowards(ref right.m_Left.c, node.m_Middle.d, amount1);
            // ISSUE: reference to a compiler-generated method
            this.MoveTowards(ref right.m_Left.d, node.m_Middle.d, amount1);
          }
          // ISSUE: reference to a compiler-generated method
          this.AddSegment(right, net, netGeometry, compositionData, terrainComposition, flags | TerrainSystem.LaneFlags.MiddleRight, false);
          right.m_Left = right.m_Right;
          right.m_Right = node.m_Right.m_Right;
          if ((double) amount2 != 0.0)
          {
            // ISSUE: reference to a compiler-generated method
            this.MoveTowards(ref right.m_Right.a, node.m_Middle.d, amount2);
            // ISSUE: reference to a compiler-generated method
            this.MoveTowards(ref right.m_Right.b, node.m_Middle.d, amount2);
            // ISSUE: reference to a compiler-generated method
            this.MoveTowards(ref right.m_Right.c, node.m_Middle.d, amount2);
            // ISSUE: reference to a compiler-generated method
            this.MoveTowards(ref right.m_Right.d, node.m_Middle.d, amount2);
          }
          // ISSUE: reference to a compiler-generated method
          this.AddSegment(right, net, netGeometry, compositionData, terrainComposition, flags | TerrainSystem.LaneFlags.MiddleLeft, false);
        }
        else if ((double) math.lengthsq(node.m_Left.m_Right.d - node.m_Right.m_Left.d) > 9.9999997473787516E-05)
        {
          Game.Net.Segment left = node.m_Left;
          // ISSUE: reference to a compiler-generated method
          this.AddSegment(left, net, netGeometry, prefabCompositionData, terrainComposition, flags | TerrainSystem.LaneFlags.MiddleRight, true);
          left.m_Left = left.m_Right;
          left.m_Right = node.m_Middle;
          // ISSUE: reference to a compiler-generated method
          this.AddSegment(left, net, netGeometry, prefabCompositionData, terrainComposition, flags | TerrainSystem.LaneFlags.MiddleLeft | TerrainSystem.LaneFlags.MiddleRight, true);
          Game.Net.Segment right = node.m_Right;
          // ISSUE: reference to a compiler-generated method
          this.AddSegment(right, net, netGeometry, prefabCompositionData, terrainComposition, flags | TerrainSystem.LaneFlags.MiddleLeft, true);
          right.m_Right = right.m_Left;
          right.m_Left = node.m_Middle;
          // ISSUE: reference to a compiler-generated method
          this.AddSegment(right, net, netGeometry, prefabCompositionData, terrainComposition, flags | TerrainSystem.LaneFlags.MiddleLeft | TerrainSystem.LaneFlags.MiddleRight, true);
        }
        else
        {
          Game.Net.Segment left = node.m_Left with
          {
            m_Right = node.m_Middle
          };
          // ISSUE: reference to a compiler-generated method
          this.AddSegment(left, net, netGeometry, prefabCompositionData, terrainComposition, flags | TerrainSystem.LaneFlags.MiddleRight, true);
          left.m_Left = node.m_Middle;
          left.m_Right = node.m_Right.m_Right;
          // ISSUE: reference to a compiler-generated method
          this.AddSegment(left, net, netGeometry, prefabCompositionData, terrainComposition, flags | TerrainSystem.LaneFlags.MiddleLeft, true);
        }
      }

      private void AddOrphans(
        Game.Net.Node node,
        NodeGeometry nodeGeometry,
        float4 area,
        NetData net,
        NetGeometryData netGeometry,
        NetCompositionData prefabCompositionData,
        TerrainComposition terrainComposition)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.LaneFlags flags = this.GetFlags(netGeometry, prefabCompositionData);
        if ((flags & (TerrainSystem.LaneFlags.ShiftTerrain | TerrainSystem.LaneFlags.ClipTerrain)) == (TerrainSystem.LaneFlags) 0)
          return;
        Game.Net.Segment segment = new Game.Net.Segment();
        Bounds2 xz = nodeGeometry.m_Bounds.xz;
        if (math.any(xz.max < area.xy) || math.any(xz.min > area.zw))
          return;
        if ((prefabCompositionData.m_Flags.m_General & CompositionFlags.General.Tunnel) != (CompositionFlags.General) 0)
          flags |= TerrainSystem.LaneFlags.InverseClipOffset;
        segment.m_Left.a = new float3(node.m_Position.x - prefabCompositionData.m_Width * 0.5f, node.m_Position.y, node.m_Position.z);
        segment.m_Left.b = new float3(node.m_Position.x - prefabCompositionData.m_Width * 0.5f, node.m_Position.y, node.m_Position.z + prefabCompositionData.m_Width * 0.2761424f);
        segment.m_Left.c = new float3(node.m_Position.x - prefabCompositionData.m_Width * 0.2761424f, node.m_Position.y, node.m_Position.z + prefabCompositionData.m_Width * 0.5f);
        segment.m_Left.d = new float3(node.m_Position.x, node.m_Position.y, node.m_Position.z + prefabCompositionData.m_Width * 0.5f);
        segment.m_Right = new Bezier4x3(node.m_Position, node.m_Position, node.m_Position, node.m_Position);
        segment.m_Length = new float2(prefabCompositionData.m_Width * 1.57079637f, 0.0f);
        // ISSUE: reference to a compiler-generated method
        this.AddSegment(segment, net, netGeometry, prefabCompositionData, terrainComposition, flags | TerrainSystem.LaneFlags.MiddleRight, true);
        CommonUtils.Swap<Bezier4x3>(ref segment.m_Left, ref segment.m_Right);
        segment.m_Right.a.x += prefabCompositionData.m_Width;
        segment.m_Right.b.x += prefabCompositionData.m_Width;
        segment.m_Right.c.x = node.m_Position.x * 2f - segment.m_Right.c.x;
        // ISSUE: reference to a compiler-generated method
        this.AddSegment(segment, net, netGeometry, prefabCompositionData, terrainComposition, flags | TerrainSystem.LaneFlags.MiddleLeft, true);
        segment.m_Left.a = new float3(node.m_Position.x + prefabCompositionData.m_Width * 0.5f, node.m_Position.y, node.m_Position.z);
        segment.m_Left.b = new float3(node.m_Position.x + prefabCompositionData.m_Width * 0.5f, node.m_Position.y, node.m_Position.z - prefabCompositionData.m_Width * 0.2761424f);
        segment.m_Left.c = new float3(node.m_Position.x + prefabCompositionData.m_Width * 0.2761424f, node.m_Position.y, node.m_Position.z - prefabCompositionData.m_Width * 0.5f);
        segment.m_Left.d = new float3(node.m_Position.x, node.m_Position.y, node.m_Position.z - prefabCompositionData.m_Width * 0.5f);
        segment.m_Right = new Bezier4x3(node.m_Position, node.m_Position, node.m_Position, node.m_Position);
        segment.m_Length = new float2(prefabCompositionData.m_Width * 1.57079637f, 0.0f);
        // ISSUE: reference to a compiler-generated method
        this.AddSegment(segment, net, netGeometry, prefabCompositionData, terrainComposition, flags | TerrainSystem.LaneFlags.MiddleRight, true);
        CommonUtils.Swap<Bezier4x3>(ref segment.m_Left, ref segment.m_Right);
        segment.m_Right.a.x -= prefabCompositionData.m_Width;
        segment.m_Right.b.x -= prefabCompositionData.m_Width;
        segment.m_Right.c.x = node.m_Position.x * 2f - segment.m_Right.c.x;
        // ISSUE: reference to a compiler-generated method
        this.AddSegment(segment, net, netGeometry, prefabCompositionData, terrainComposition, flags | TerrainSystem.LaneFlags.MiddleLeft, true);
      }

      private void AddSegment(
        Game.Net.Segment segment,
        NetData net,
        NetGeometryData netGeometry,
        NetCompositionData compositionData,
        TerrainComposition terrainComposition,
        TerrainSystem.LaneFlags flags,
        bool isStart)
      {
        if (math.any(terrainComposition.m_WidthOffset != 0.0f))
        {
          Game.Net.Segment segment1 = segment;
          float4 float4 = 1f / math.max((float4) (1f / 1000f), new float4(math.distance(segment.m_Left.a.xz, segment.m_Right.a.xz), math.distance(segment.m_Left.b.xz, segment.m_Right.b.xz), math.distance(segment.m_Left.c.xz, segment.m_Right.c.xz), math.distance(segment.m_Left.d.xz, segment.m_Right.d.xz)));
          if ((double) terrainComposition.m_WidthOffset.x != 0.0 && (flags & TerrainSystem.LaneFlags.MiddleLeft) == (TerrainSystem.LaneFlags) 0)
            segment.m_Left = MathUtils.Lerp(segment1.m_Left, segment1.m_Right, new Bezier4x1()
            {
              abcd = terrainComposition.m_WidthOffset.x * float4
            });
          if ((double) terrainComposition.m_WidthOffset.y != 0.0 && (flags & TerrainSystem.LaneFlags.MiddleRight) == (TerrainSystem.LaneFlags) 0)
            segment.m_Right = MathUtils.Lerp(segment1.m_Right, segment1.m_Left, new Bezier4x1()
            {
              abcd = terrainComposition.m_WidthOffset.y * float4
            });
        }
        float3 x1 = math.select(new float3(compositionData.m_EdgeHeights.z, compositionData.m_SurfaceHeight.min, compositionData.m_EdgeHeights.w), new float3(compositionData.m_EdgeHeights.x, compositionData.m_SurfaceHeight.min, compositionData.m_EdgeHeights.y), isStart);
        float3 x2 = x1;
        float2 float2_1 = new float2(math.cmin(x1), math.cmax(x2));
        float terrainSmoothingWidth = NetUtils.GetTerrainSmoothingWidth(net);
        float2 float2_2 = float2_1 + terrainComposition.m_ClipHeightOffset;
        float3 x3 = x1 + terrainComposition.m_MinHeightOffset;
        float3 y = x2 + terrainComposition.m_MaxHeightOffset;
        float3 x4 = (float3) 1000000f;
        float3 x5 = (float3) 1000000f;
        float3 float3_1 = (float3) 1000000f;
        if ((compositionData.m_State & CompositionState.HasSurface) == (CompositionState) 0)
        {
          if ((compositionData.m_Flags.m_General & CompositionFlags.General.Tunnel) != (CompositionFlags.General) 0)
          {
            x3 = (float3) 1000000f;
            y = compositionData.m_HeightRange.max + 1f + terrainComposition.m_MaxHeightOffset;
          }
          else
          {
            x3 = compositionData.m_HeightRange.min + terrainComposition.m_MinHeightOffset;
            y = (float3) -1000000f;
          }
        }
        else if ((compositionData.m_Flags.m_General & CompositionFlags.General.Elevated) != (CompositionFlags.General) 0 || (netGeometry.m_MergeLayers & Layer.Waterway) != Layer.None)
        {
          if (((compositionData.m_Flags.m_Left | compositionData.m_Flags.m_Right) & (CompositionFlags.Side.LowTransition | CompositionFlags.Side.HighTransition)) == (CompositionFlags.Side) 0)
            x3 = (float3) compositionData.m_HeightRange.min;
          y = (float3) -1000000f;
        }
        else if ((compositionData.m_Flags.m_General & CompositionFlags.General.Tunnel) != (CompositionFlags.General) 0)
        {
          if ((compositionData.m_Flags.m_Left & CompositionFlags.Side.HighTransition) != (CompositionFlags.Side) 0)
            x4.xy = math.min(x4.xy, x3.xy);
          if ((compositionData.m_Flags.m_Right & CompositionFlags.Side.HighTransition) != (CompositionFlags.Side) 0)
            x4.yz = math.min(x4.yz, x3.yz);
          if (((compositionData.m_Flags.m_Left | compositionData.m_Flags.m_Right) & (CompositionFlags.Side.LowTransition | CompositionFlags.Side.HighTransition)) == (CompositionFlags.Side) 0)
          {
            x3 = (float3) 1000000f;
            y = (float3) (compositionData.m_HeightRange.max + 1f);
          }
          else
          {
            x5 = (float3) (netGeometry.m_ElevationLimit * 3f);
            float3_1 = (float3) (compositionData.m_HeightRange.max + 1f);
            x3 = math.max(x3, (float3) (netGeometry.m_ElevationLimit * 3f));
            float2_2.y = math.max(float2_2.y, netGeometry.m_ElevationLimit * 3f);
          }
        }
        else
        {
          if ((compositionData.m_Flags.m_Left & CompositionFlags.Side.Lowered) != (CompositionFlags.Side) 0)
          {
            if ((compositionData.m_Flags.m_Left & CompositionFlags.Side.LowTransition) != (CompositionFlags.Side) 0)
              x4.xy = math.min(x4.xy, x3.xy);
            x3.xy = math.max(x3.xy, (float2) (netGeometry.m_ElevationLimit * 3f));
            float2_2.y = math.max(float2_2.y, netGeometry.m_ElevationLimit * 3f);
          }
          else if ((compositionData.m_Flags.m_Left & CompositionFlags.Side.Raised) != (CompositionFlags.Side) 0)
            y.xy = (float2) -1000000f;
          if ((compositionData.m_Flags.m_Right & CompositionFlags.Side.Lowered) != (CompositionFlags.Side) 0)
          {
            if ((compositionData.m_Flags.m_Right & CompositionFlags.Side.LowTransition) != (CompositionFlags.Side) 0)
              x4.yz = math.min(x4.yz, x3.yz);
            x3.yz = math.max(x3.yz, (float2) (netGeometry.m_ElevationLimit * 3f));
            float2_2.y = math.max(float2_2.y, netGeometry.m_ElevationLimit * 3f);
          }
          else if ((compositionData.m_Flags.m_Right & CompositionFlags.Side.Raised) != (CompositionFlags.Side) 0)
            y.yz = (float2) -1000000f;
        }
        Bounds3 bounds3_1 = MathUtils.Bounds(segment.m_Left) | MathUtils.Bounds(segment.m_Right);
        bounds3_1.min.xz -= terrainSmoothingWidth;
        bounds3_1.max.xz += terrainSmoothingWidth;
        bounds3_1.min.y += math.cmin(math.min(x3, y));
        bounds3_1.max.y += math.cmax(math.max(x3, y));
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.LaneSection laneSection1 = new TerrainSystem.LaneSection()
        {
          m_Bounds = bounds3_1.xz,
          m_Left = new float4x3(segment.m_Left.a.x, segment.m_Left.a.y, segment.m_Left.a.z, segment.m_Left.b.x, segment.m_Left.b.y, segment.m_Left.b.z, segment.m_Left.c.x, segment.m_Left.c.y, segment.m_Left.c.z, segment.m_Left.d.x, segment.m_Left.d.y, segment.m_Left.d.z),
          m_Right = new float4x3(segment.m_Right.a.x, segment.m_Right.a.y, segment.m_Right.a.z, segment.m_Right.b.x, segment.m_Right.b.y, segment.m_Right.b.z, segment.m_Right.c.x, segment.m_Right.c.y, segment.m_Right.c.z, segment.m_Right.d.x, segment.m_Right.d.y, segment.m_Right.d.z),
          m_MinOffset = x3,
          m_MaxOffset = y,
          m_ClipOffset = float2_2,
          m_WidthOffset = terrainSmoothingWidth,
          m_Flags = flags
        };
        // ISSUE: reference to a compiler-generated field
        this.Result.AddNoResize(laneSection1);
        Bounds3 bounds3_2;
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.LaneSection laneSection2;
        if (math.any(x4 != 1000000f) && (flags & TerrainSystem.LaneFlags.ShiftTerrain) != (TerrainSystem.LaneFlags) 0)
        {
          Bounds1 t1 = new Bounds1(0.0f, 1f);
          Bounds1 t2 = new Bounds1(0.0f, 1f);
          MathUtils.ClampLengthInverse(segment.m_Left.xz, ref t1, 3f);
          MathUtils.ClampLengthInverse(segment.m_Right.xz, ref t2, 3f);
          Game.Net.Segment segment2 = segment with
          {
            m_Left = MathUtils.Cut(segment.m_Left, t1),
            m_Right = MathUtils.Cut(segment.m_Right, t2)
          };
          bounds3_2 = MathUtils.Bounds(segment2.m_Left) | MathUtils.Bounds(segment2.m_Right);
          bounds3_2.min.xz -= terrainSmoothingWidth;
          bounds3_2.max.xz += terrainSmoothingWidth;
          bounds3_2.min.y += math.cmin(math.min(x4, y));
          bounds3_2.max.y += math.cmax(math.max(x4, y));
          // ISSUE: object of a compiler-generated type is created
          laneSection2 = new TerrainSystem.LaneSection();
          // ISSUE: reference to a compiler-generated field
          laneSection2.m_Bounds = bounds3_2.xz;
          // ISSUE: reference to a compiler-generated field
          laneSection2.m_Left = new float4x3(segment2.m_Left.a.x, segment2.m_Left.a.y, segment2.m_Left.a.z, segment2.m_Left.b.x, segment2.m_Left.b.y, segment2.m_Left.b.z, segment2.m_Left.c.x, segment2.m_Left.c.y, segment2.m_Left.c.z, segment2.m_Left.d.x, segment2.m_Left.d.y, segment2.m_Left.d.z);
          // ISSUE: reference to a compiler-generated field
          laneSection2.m_Right = new float4x3(segment2.m_Right.a.x, segment2.m_Right.a.y, segment2.m_Right.a.z, segment2.m_Right.b.x, segment2.m_Right.b.y, segment2.m_Right.b.z, segment2.m_Right.c.x, segment2.m_Right.c.y, segment2.m_Right.c.z, segment2.m_Right.d.x, segment2.m_Right.d.y, segment2.m_Right.d.z);
          // ISSUE: reference to a compiler-generated field
          laneSection2.m_MinOffset = x4;
          // ISSUE: reference to a compiler-generated field
          laneSection2.m_MaxOffset = y;
          // ISSUE: reference to a compiler-generated field
          laneSection2.m_ClipOffset = float2_2;
          // ISSUE: reference to a compiler-generated field
          laneSection2.m_WidthOffset = terrainSmoothingWidth;
          // ISSUE: reference to a compiler-generated field
          laneSection2.m_Flags = flags & ~TerrainSystem.LaneFlags.ClipTerrain;
          // ISSUE: variable of a compiler-generated type
          TerrainSystem.LaneSection laneSection3 = laneSection2;
          // ISSUE: reference to a compiler-generated field
          this.Result.AddNoResize(laneSection3);
        }
        if (!math.any(x5 != 1000000f) && !math.any(float3_1 != 1000000f) || (flags & TerrainSystem.LaneFlags.ShiftTerrain) == (TerrainSystem.LaneFlags) 0)
          return;
        float3 float3_2 = MathUtils.StartTangent(segment.m_Left);
        float3 float3_3 = MathUtils.StartTangent(segment.m_Right);
        float3_2 = MathUtils.Normalize(float3_2, float3_2.xz);
        float3_3 = MathUtils.Normalize(float3_3, float3_3.xz);
        Game.Net.Segment segment3 = segment with
        {
          m_Left = NetUtils.StraightCurve(segment.m_Left.a + float3_2 * 2f, segment.m_Left.a - float3_2 * 2f),
          m_Right = NetUtils.StraightCurve(segment.m_Right.a + float3_3 * 2f, segment.m_Right.a - float3_3 * 2f)
        };
        bounds3_2 = MathUtils.Bounds(segment3.m_Left) | MathUtils.Bounds(segment3.m_Right);
        bounds3_2.min.xz -= terrainSmoothingWidth;
        bounds3_2.max.xz += terrainSmoothingWidth;
        bounds3_2.min.y += math.cmin(math.min(x5, y));
        bounds3_2.max.y += math.cmax(math.max(x5, y));
        // ISSUE: object of a compiler-generated type is created
        laneSection2 = new TerrainSystem.LaneSection();
        // ISSUE: reference to a compiler-generated field
        laneSection2.m_Bounds = bounds3_2.xz;
        // ISSUE: reference to a compiler-generated field
        laneSection2.m_Left = new float4x3(segment3.m_Left.a.x, segment3.m_Left.a.y, segment3.m_Left.a.z, segment3.m_Left.b.x, segment3.m_Left.b.y, segment3.m_Left.b.z, segment3.m_Left.c.x, segment3.m_Left.c.y, segment3.m_Left.c.z, segment3.m_Left.d.x, segment3.m_Left.d.y, segment3.m_Left.d.z);
        // ISSUE: reference to a compiler-generated field
        laneSection2.m_Right = new float4x3(segment3.m_Right.a.x, segment3.m_Right.a.y, segment3.m_Right.a.z, segment3.m_Right.b.x, segment3.m_Right.b.y, segment3.m_Right.b.z, segment3.m_Right.c.x, segment3.m_Right.c.y, segment3.m_Right.c.z, segment3.m_Right.d.x, segment3.m_Right.d.y, segment3.m_Right.d.z);
        // ISSUE: reference to a compiler-generated field
        laneSection2.m_MinOffset = x5;
        // ISSUE: reference to a compiler-generated field
        laneSection2.m_MaxOffset = float3_1;
        // ISSUE: reference to a compiler-generated field
        laneSection2.m_ClipOffset = float2_2;
        // ISSUE: reference to a compiler-generated field
        laneSection2.m_WidthOffset = terrainSmoothingWidth;
        // ISSUE: reference to a compiler-generated field
        laneSection2.m_Flags = flags & ~TerrainSystem.LaneFlags.ClipTerrain;
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.LaneSection laneSection4 = laneSection2;
        // ISSUE: reference to a compiler-generated field
        this.Result.AddNoResize(laneSection4);
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    [BurstCompile]
    private struct DequeBuildingDrawsJob : IJob
    {
      [ReadOnly]
      public NativeQueue<TerrainSystem.BuildingLotDraw> m_Queue;
      public NativeList<TerrainSystem.BuildingLotDraw> m_List;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<TerrainSystem.BuildingLotDraw> other = this.m_Queue.ToArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        this.m_List.CopyFrom(in other);
        other.Dispose();
      }
    }

    [BurstCompile]
    private struct CullBuildingsCascadeJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeList<BuildingUtils.LotInfo> m_LotsToCull;
      [ReadOnly]
      public float4 m_Area;
      public NativeQueue<TerrainSystem.BuildingLotDraw>.ParallelWriter Result;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        if (index >= this.m_LotsToCull.Length)
          return;
        // ISSUE: reference to a compiler-generated field
        BuildingUtils.LotInfo lotInfo = this.m_LotsToCull[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((double) lotInfo.m_Position.x + (double) lotInfo.m_Radius < (double) this.m_Area.x || (double) lotInfo.m_Position.x - (double) lotInfo.m_Radius > (double) this.m_Area.z || (double) lotInfo.m_Position.z + (double) lotInfo.m_Radius < (double) this.m_Area.y || (double) lotInfo.m_Position.z - (double) lotInfo.m_Radius > (double) this.m_Area.w)
          return;
        float2 float2 = 0.5f / math.max((float2) 0.01f, lotInfo.m_Extents);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.BuildingLotDraw buildingLotDraw = new TerrainSystem.BuildingLotDraw()
        {
          m_HeightsX = math.transpose(new float4x2(new float4(lotInfo.m_RightHeights, lotInfo.m_BackHeights.x), new float4(lotInfo.m_FrontHeights.x, lotInfo.m_LeftHeights.zyx))),
          m_HeightsZ = math.transpose(new float4x2(new float4(lotInfo.m_RightHeights.x, lotInfo.m_FrontHeights.zyx), new float4(lotInfo.m_BackHeights, lotInfo.m_LeftHeights.x))),
          m_FlatX0 = lotInfo.m_FlatX0 * float2.x + 0.5f,
          m_FlatZ0 = lotInfo.m_FlatZ0 * float2.y + 0.5f,
          m_FlatX1 = lotInfo.m_FlatX1 * float2.x + 0.5f,
          m_FlatZ1 = lotInfo.m_FlatZ1 * float2.y + 0.5f,
          m_Position = lotInfo.m_Position,
          m_AxisX = math.mul(lotInfo.m_Rotation, new float3(1f, 0.0f, 0.0f)),
          m_AxisZ = math.mul(lotInfo.m_Rotation, new float3(0.0f, 0.0f, 1f)),
          m_Size = lotInfo.m_Extents,
          m_MinLimit = lotInfo.m_MinLimit * float2.xyxy + 0.5f,
          m_MaxLimit = lotInfo.m_MaxLimit * float2.xyxy + 0.5f,
          m_Circular = lotInfo.m_Circular,
          m_SmoothingWidth = ObjectUtils.GetTerrainSmoothingWidth(lotInfo.m_Extents * 2f)
        };
        // ISSUE: reference to a compiler-generated field
        this.Result.Enqueue(buildingLotDraw);
      }
    }

    [BurstCompile]
    private struct CullTrianglesJob : IJob
    {
      [ReadOnly]
      public NativeList<TerrainSystem.AreaTriangle> m_Triangles;
      [ReadOnly]
      public float4 m_Area;
      public NativeList<TerrainSystem.AreaTriangle> Result;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Triangles.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          TerrainSystem.AreaTriangle triangle = this.m_Triangles[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float2 float2_1 = math.min(triangle.m_PositionA, math.min(triangle.m_PositionB, triangle.m_PositionC));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float2 float2_2 = math.max(triangle.m_PositionA, math.max(triangle.m_PositionB, triangle.m_PositionC));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) float2_2.x >= (double) this.m_Area.x && (double) float2_1.x <= (double) this.m_Area.z && (double) float2_2.y >= (double) this.m_Area.y && (double) float2_1.y <= (double) this.m_Area.w)
          {
            // ISSUE: reference to a compiler-generated field
            this.Result.Add(in triangle);
          }
        }
      }
    }

    [BurstCompile]
    private struct CullEdgesJob : IJob
    {
      [ReadOnly]
      public NativeList<TerrainSystem.AreaEdge> m_Edges;
      [ReadOnly]
      public float4 m_Area;
      public NativeList<TerrainSystem.AreaEdge> Result;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Edges.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          TerrainSystem.AreaEdge edge = this.m_Edges[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float2 float2_1 = math.min(edge.m_PositionA, edge.m_PositionB) - edge.m_SideOffset;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float2 float2_2 = math.max(edge.m_PositionA, edge.m_PositionB) + edge.m_SideOffset;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) float2_2.x >= (double) this.m_Area.x && (double) float2_1.x <= (double) this.m_Area.z && (double) float2_2.y >= (double) this.m_Area.y && (double) float2_1.y <= (double) this.m_Area.w)
          {
            // ISSUE: reference to a compiler-generated field
            this.Result.Add(in edge);
          }
        }
      }
    }

    [BurstCompile]
    private struct GenerateClipDataJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeList<TerrainSystem.LaneSection> m_RoadsToCull;
      public NativeList<TerrainSystem.ClipMapDraw>.ParallelWriter Result;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.LaneSection laneSection = this.m_RoadsToCull[index];
        // ISSUE: reference to a compiler-generated field
        if ((laneSection.m_Flags & TerrainSystem.LaneFlags.ClipTerrain) == (TerrainSystem.LaneFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        laneSection.m_ClipOffset.x -= 0.3f;
        // ISSUE: reference to a compiler-generated field
        laneSection.m_ClipOffset.y += 0.3f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        laneSection.m_Left.c1 += laneSection.m_ClipOffset.x;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        laneSection.m_Right.c1 += laneSection.m_ClipOffset.x;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.ClipMapDraw clipMapDraw = new TerrainSystem.ClipMapDraw()
        {
          m_Left = laneSection.m_Left,
          m_Right = laneSection.m_Right,
          m_Height = laneSection.m_ClipOffset.y - laneSection.m_ClipOffset.x,
          m_OffsetFactor = math.select(1f, -1f, (laneSection.m_Flags & TerrainSystem.LaneFlags.InverseClipOffset) != 0)
        };
        // ISSUE: reference to a compiler-generated field
        this.Result.AddNoResize(clipMapDraw);
      }
    }

    [BurstCompile]
    private struct CullAreasJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Clip> m_ClipType;
      [ReadOnly]
      public ComponentTypeHandle<Area> m_AreaType;
      [ReadOnly]
      public ComponentTypeHandle<Geometry> m_GeometryType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Areas.Storage> m_StorageType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> m_NodeType;
      [ReadOnly]
      public BufferTypeHandle<Triangle> m_TriangleType;
      [ReadOnly]
      public ComponentLookup<TerrainAreaData> m_PrefabTerrainAreaData;
      [ReadOnly]
      public ComponentLookup<StorageAreaData> m_PrefabStorageAreaData;
      [ReadOnly]
      public float4 m_Area;
      public NativeQueue<TerrainSystem.AreaTriangle>.ParallelWriter m_Triangles;
      public NativeQueue<TerrainSystem.AreaEdge>.ParallelWriter m_Edges;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Clip>(ref this.m_ClipType))
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Area> nativeArray1 = chunk.GetNativeArray<Area>(ref this.m_AreaType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Geometry> nativeArray2 = chunk.GetNativeArray<Geometry>(ref this.m_GeometryType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Areas.Storage> nativeArray3 = chunk.GetNativeArray<Game.Areas.Storage>(ref this.m_StorageType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Areas.Node> bufferAccessor1 = chunk.GetBufferAccessor<Game.Areas.Node>(ref this.m_NodeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Triangle> bufferAccessor2 = chunk.GetBufferAccessor<Triangle>(ref this.m_TriangleType);
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          Area area = nativeArray1[index1];
          Geometry geometry = nativeArray2[index1];
          PrefabRef prefabRef = nativeArray4[index1];
          DynamicBuffer<Game.Areas.Node> dynamicBuffer1 = bufferAccessor1[index1];
          DynamicBuffer<Triangle> dynamicBuffer2 = bufferAccessor2[index1];
          TerrainAreaData componentData1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) geometry.m_Bounds.max.x >= (double) this.m_Area.x && (double) geometry.m_Bounds.min.x <= (double) this.m_Area.z && (double) geometry.m_Bounds.max.z >= (double) this.m_Area.y && (double) geometry.m_Bounds.min.z <= (double) this.m_Area.w && dynamicBuffer2.Length != 0 && this.m_PrefabTerrainAreaData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
          {
            float2 float2_1 = new float2(componentData1.m_NoiseFactor, componentData1.m_NoiseScale);
            float heightOffset = componentData1.m_HeightOffset;
            float slopeWidth = componentData1.m_SlopeWidth;
            StorageAreaData componentData2;
            // ISSUE: reference to a compiler-generated field
            if (nativeArray3.Length != 0 && this.m_PrefabStorageAreaData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
            {
              float y = (float) (int) ((long) nativeArray3[index1].m_Amount * 100L / (long) math.max(1, AreaUtils.CalculateStorageCapacity(geometry, componentData2))) * 0.015f;
              float x = math.min(1f, y);
              float2_1.x *= math.clamp(2f - y, 0.5f, 1f);
              heightOffset *= x;
              slopeWidth *= math.sqrt(x);
            }
            Game.Areas.Node node;
            for (int index2 = 0; index2 < dynamicBuffer2.Length; ++index2)
            {
              Triangle triangle = dynamicBuffer2[index2];
              // ISSUE: reference to a compiler-generated field
              ref NativeQueue<TerrainSystem.AreaTriangle>.ParallelWriter local1 = ref this.m_Triangles;
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              TerrainSystem.AreaTriangle areaTriangle1 = new TerrainSystem.AreaTriangle();
              ref TerrainSystem.AreaTriangle local2 = ref areaTriangle1;
              node = dynamicBuffer1[triangle.m_Indices.x];
              float2 xz1 = node.m_Position.xz;
              // ISSUE: reference to a compiler-generated field
              local2.m_PositionA = xz1;
              ref TerrainSystem.AreaTriangle local3 = ref areaTriangle1;
              node = dynamicBuffer1[triangle.m_Indices.y];
              float2 xz2 = node.m_Position.xz;
              // ISSUE: reference to a compiler-generated field
              local3.m_PositionB = xz2;
              ref TerrainSystem.AreaTriangle local4 = ref areaTriangle1;
              node = dynamicBuffer1[triangle.m_Indices.z];
              float2 xz3 = node.m_Position.xz;
              // ISSUE: reference to a compiler-generated field
              local4.m_PositionC = xz3;
              // ISSUE: reference to a compiler-generated field
              areaTriangle1.m_NoiseSize = float2_1;
              // ISSUE: reference to a compiler-generated field
              areaTriangle1.m_HeightDelta = heightOffset;
              // ISSUE: variable of a compiler-generated type
              TerrainSystem.AreaTriangle areaTriangle2 = areaTriangle1;
              local1.Enqueue(areaTriangle2);
            }
            // ISSUE: variable of a compiler-generated type
            TerrainSystem.AreaEdge areaEdge1;
            if ((area.m_Flags & AreaFlags.CounterClockwise) != (AreaFlags) 0)
            {
              node = dynamicBuffer1[0];
              float2 xz4 = node.m_Position.xz;
              node = dynamicBuffer1[1];
              float2 float2_2 = node.m_Position.xz;
              node = dynamicBuffer1[2];
              float2 xz5 = node.m_Position.xz;
              float2 float2_3 = math.normalizesafe(float2_2 - xz4);
              float2 toVector = math.normalizesafe(xz5 - float2_2);
              float x = MathUtils.RotationAngleRight(-float2_3, toVector);
              for (int index3 = 0; index3 < dynamicBuffer1.Length; ++index3)
              {
                int num = index3 + 3;
                int index4 = num - math.select(0, dynamicBuffer1.Length, num >= dynamicBuffer1.Length);
                float2 float2_4 = float2_2;
                float2_2 = xz5;
                node = dynamicBuffer1[index4];
                xz5 = node.m_Position.xz;
                float2 float2_5 = toVector;
                toVector = math.normalizesafe(xz5 - float2_2);
                float y = x;
                x = MathUtils.RotationAngleRight(-float2_5, toVector);
                // ISSUE: reference to a compiler-generated field
                ref NativeQueue<TerrainSystem.AreaEdge>.ParallelWriter local = ref this.m_Edges;
                // ISSUE: object of a compiler-generated type is created
                areaEdge1 = new TerrainSystem.AreaEdge();
                // ISSUE: reference to a compiler-generated field
                areaEdge1.m_PositionA = float2_2;
                // ISSUE: reference to a compiler-generated field
                areaEdge1.m_PositionB = float2_4;
                // ISSUE: reference to a compiler-generated field
                areaEdge1.m_Angles = new float2(x, y);
                // ISSUE: reference to a compiler-generated field
                areaEdge1.m_SideOffset = slopeWidth;
                // ISSUE: variable of a compiler-generated type
                TerrainSystem.AreaEdge areaEdge2 = areaEdge1;
                local.Enqueue(areaEdge2);
              }
            }
            else
            {
              node = dynamicBuffer1[0];
              float2 xz6 = node.m_Position.xz;
              node = dynamicBuffer1[1];
              float2 float2_6 = node.m_Position.xz;
              node = dynamicBuffer1[2];
              float2 xz7 = node.m_Position.xz;
              float2 float2_7 = math.normalizesafe(float2_6 - xz6);
              float2 toVector = math.normalizesafe(xz7 - float2_6);
              float y = MathUtils.RotationAngleLeft(-float2_7, toVector);
              for (int index5 = 0; index5 < dynamicBuffer1.Length; ++index5)
              {
                int num = index5 + 3;
                int index6 = num - math.select(0, dynamicBuffer1.Length, num >= dynamicBuffer1.Length);
                float2 float2_8 = float2_6;
                float2_6 = xz7;
                node = dynamicBuffer1[index6];
                xz7 = node.m_Position.xz;
                float2 float2_9 = toVector;
                toVector = math.normalizesafe(xz7 - float2_6);
                float x = y;
                y = MathUtils.RotationAngleLeft(-float2_9, toVector);
                // ISSUE: reference to a compiler-generated field
                ref NativeQueue<TerrainSystem.AreaEdge>.ParallelWriter local = ref this.m_Edges;
                // ISSUE: object of a compiler-generated type is created
                areaEdge1 = new TerrainSystem.AreaEdge();
                // ISSUE: reference to a compiler-generated field
                areaEdge1.m_PositionA = float2_8;
                // ISSUE: reference to a compiler-generated field
                areaEdge1.m_PositionB = float2_6;
                // ISSUE: reference to a compiler-generated field
                areaEdge1.m_Angles = new float2(x, y);
                // ISSUE: reference to a compiler-generated field
                areaEdge1.m_SideOffset = slopeWidth;
                // ISSUE: variable of a compiler-generated type
                TerrainSystem.AreaEdge areaEdge3 = areaEdge1;
                local.Enqueue(areaEdge3);
              }
            }
          }
        }
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    [BurstCompile]
    private struct DequeTrianglesJob : IJob
    {
      [ReadOnly]
      public NativeQueue<TerrainSystem.AreaTriangle> m_Queue;
      public NativeList<TerrainSystem.AreaTriangle> m_List;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<TerrainSystem.AreaTriangle> other = this.m_Queue.ToArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        this.m_List.CopyFrom(in other);
        other.Dispose();
      }
    }

    [BurstCompile]
    private struct DequeEdgesJob : IJob
    {
      [ReadOnly]
      public NativeQueue<TerrainSystem.AreaEdge> m_Queue;
      public NativeList<TerrainSystem.AreaEdge> m_List;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<TerrainSystem.AreaEdge> other = this.m_Queue.ToArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        this.m_List.CopyFrom(in other);
        other.Dispose();
      }
    }

    [BurstCompile]
    private struct GenerateAreaClipMeshJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<Clip> m_ClipType;
      [ReadOnly]
      public ComponentTypeHandle<Area> m_AreaType;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> m_NodeType;
      [ReadOnly]
      public BufferTypeHandle<Triangle> m_TriangleType;
      public Mesh.MeshDataArray m_MeshData;

      public void Execute()
      {
        int vertexCount = 0;
        int indexCount = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Clip>(ref this.m_ClipType))
          {
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Game.Areas.Node> bufferAccessor1 = chunk.GetBufferAccessor<Game.Areas.Node>(ref this.m_NodeType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Triangle> bufferAccessor2 = chunk.GetBufferAccessor<Triangle>(ref this.m_TriangleType);
            for (int index2 = 0; index2 < bufferAccessor1.Length; ++index2)
            {
              DynamicBuffer<Game.Areas.Node> dynamicBuffer1 = bufferAccessor1[index2];
              DynamicBuffer<Triangle> dynamicBuffer2 = bufferAccessor2[index2];
              vertexCount += dynamicBuffer1.Length * 2;
              indexCount += dynamicBuffer2.Length * 6 + dynamicBuffer1.Length * 6;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        Mesh.MeshData meshData = this.m_MeshData[0];
        NativeArray<VertexAttributeDescriptor> attributes = new NativeArray<VertexAttributeDescriptor>(1, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
        attributes[0] = new VertexAttributeDescriptor(dimension: 4);
        meshData.SetVertexBufferParams(vertexCount, attributes);
        meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt32);
        attributes.Dispose();
        meshData.subMeshCount = 1;
        meshData.SetSubMesh(0, new SubMeshDescriptor()
        {
          vertexCount = vertexCount,
          indexCount = indexCount,
          topology = MeshTopology.Triangles
        }, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
        NativeArray<float4> vertexData = meshData.GetVertexData<float4>();
        NativeArray<uint> indexData = meshData.GetIndexData<uint>();
        SubMeshDescriptor subMesh = meshData.GetSubMesh(0);
        Bounds3 bounds = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        int num1 = 0;
        int num2 = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index3 = 0; index3 < this.m_Chunks.Length; ++index3)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index3];
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Clip>(ref this.m_ClipType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Area> nativeArray = chunk.GetNativeArray<Area>(ref this.m_AreaType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Game.Areas.Node> bufferAccessor3 = chunk.GetBufferAccessor<Game.Areas.Node>(ref this.m_NodeType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<Triangle> bufferAccessor4 = chunk.GetBufferAccessor<Triangle>(ref this.m_TriangleType);
            for (int index4 = 0; index4 < nativeArray.Length; ++index4)
            {
              Area area = nativeArray[index4];
              DynamicBuffer<Game.Areas.Node> dynamicBuffer3 = bufferAccessor3[index4];
              DynamicBuffer<Triangle> dynamicBuffer4 = bufferAccessor4[index4];
              int4 int4_1 = num1 + new int4(0, 1, dynamicBuffer3.Length, dynamicBuffer3.Length + 1);
              float x1 = 0.0f;
              float x2 = 0.0f;
              for (int index5 = 0; index5 < dynamicBuffer4.Length; ++index5)
              {
                Triangle triangle = dynamicBuffer4[index5];
                int3 indices = triangle.m_Indices;
                x1 = math.min(x1, triangle.m_HeightRange.min);
                x2 = math.max(x2, triangle.m_HeightRange.max);
                int3 int3_1 = indices + int4_1.x;
                ref NativeArray<uint> local1 = ref indexData;
                int index6 = num2;
                int num3 = index6 + 1;
                int z1 = int3_1.z;
                local1[index6] = (uint) z1;
                ref NativeArray<uint> local2 = ref indexData;
                int index7 = num3;
                int num4 = index7 + 1;
                int y1 = int3_1.y;
                local2[index7] = (uint) y1;
                ref NativeArray<uint> local3 = ref indexData;
                int index8 = num4;
                int num5 = index8 + 1;
                int x3 = int3_1.x;
                local3[index8] = (uint) x3;
                int3 int3_2 = indices + int4_1.z;
                ref NativeArray<uint> local4 = ref indexData;
                int index9 = num5;
                int num6 = index9 + 1;
                int x4 = int3_2.x;
                local4[index9] = (uint) x4;
                ref NativeArray<uint> local5 = ref indexData;
                int index10 = num6;
                int num7 = index10 + 1;
                int y2 = int3_2.y;
                local5[index10] = (uint) y2;
                ref NativeArray<uint> local6 = ref indexData;
                int index11 = num7;
                num2 = index11 + 1;
                int z2 = int3_2.z;
                local6[index11] = (uint) z2;
              }
              if ((area.m_Flags & AreaFlags.CounterClockwise) != (AreaFlags) 0)
              {
                for (int index12 = 0; index12 < dynamicBuffer3.Length; ++index12)
                {
                  int4 int4_2 = index12 + int4_1;
                  int4_2.yw -= math.select(0, dynamicBuffer3.Length, index12 == dynamicBuffer3.Length - 1);
                  ref NativeArray<uint> local7 = ref indexData;
                  int index13 = num2;
                  int num8 = index13 + 1;
                  int x5 = int4_2.x;
                  local7[index13] = (uint) x5;
                  ref NativeArray<uint> local8 = ref indexData;
                  int index14 = num8;
                  int num9 = index14 + 1;
                  int y = int4_2.y;
                  local8[index14] = (uint) y;
                  ref NativeArray<uint> local9 = ref indexData;
                  int index15 = num9;
                  int num10 = index15 + 1;
                  int w1 = int4_2.w;
                  local9[index15] = (uint) w1;
                  ref NativeArray<uint> local10 = ref indexData;
                  int index16 = num10;
                  int num11 = index16 + 1;
                  int w2 = int4_2.w;
                  local10[index16] = (uint) w2;
                  ref NativeArray<uint> local11 = ref indexData;
                  int index17 = num11;
                  int num12 = index17 + 1;
                  int z = int4_2.z;
                  local11[index17] = (uint) z;
                  ref NativeArray<uint> local12 = ref indexData;
                  int index18 = num12;
                  num2 = index18 + 1;
                  int x6 = int4_2.x;
                  local12[index18] = (uint) x6;
                }
              }
              else
              {
                for (int index19 = 0; index19 < dynamicBuffer3.Length; ++index19)
                {
                  int4 int4_3 = index19 + int4_1;
                  int4_3.yw -= math.select(0, dynamicBuffer3.Length, index19 == dynamicBuffer3.Length - 1);
                  ref NativeArray<uint> local13 = ref indexData;
                  int index20 = num2;
                  int num13 = index20 + 1;
                  int x7 = int4_3.x;
                  local13[index20] = (uint) x7;
                  ref NativeArray<uint> local14 = ref indexData;
                  int index21 = num13;
                  int num14 = index21 + 1;
                  int z = int4_3.z;
                  local14[index21] = (uint) z;
                  ref NativeArray<uint> local15 = ref indexData;
                  int index22 = num14;
                  int num15 = index22 + 1;
                  int w3 = int4_3.w;
                  local15[index22] = (uint) w3;
                  ref NativeArray<uint> local16 = ref indexData;
                  int index23 = num15;
                  int num16 = index23 + 1;
                  int w4 = int4_3.w;
                  local16[index23] = (uint) w4;
                  ref NativeArray<uint> local17 = ref indexData;
                  int index24 = num16;
                  int num17 = index24 + 1;
                  int y = int4_3.y;
                  local17[index24] = (uint) y;
                  ref NativeArray<uint> local18 = ref indexData;
                  int index25 = num17;
                  num2 = index25 + 1;
                  int x8 = int4_3.x;
                  local18[index25] = (uint) x8;
                }
              }
              float num18 = x1 - 0.3f;
              float num19 = x2 + 0.3f;
              for (int index26 = 0; index26 < dynamicBuffer3.Length; ++index26)
              {
                float3 position = dynamicBuffer3[index26].m_Position;
                position.y += num18;
                bounds |= position;
                vertexData[num1++] = new float4(position, 0.0f);
              }
              for (int index27 = 0; index27 < dynamicBuffer3.Length; ++index27)
              {
                float3 position = dynamicBuffer3[index27].m_Position;
                position.y += num19;
                bounds |= position;
                vertexData[num1++] = new float4(position, 1f);
              }
            }
          }
        }
        subMesh.bounds = RenderingUtils.ToBounds(bounds);
        meshData.SetSubMesh(0, subMesh, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
      }
    }

    [BurstCompile]
    private struct CullRoadsCacscadeJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeList<TerrainSystem.LaneSection> m_RoadsToCull;
      [ReadOnly]
      public float4 m_Area;
      [ReadOnly]
      public float m_Scale;
      public NativeList<TerrainSystem.LaneDraw>.ParallelWriter Result;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.LaneSection laneSection = this.m_RoadsToCull[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((laneSection.m_Flags & TerrainSystem.LaneFlags.ShiftTerrain) == (TerrainSystem.LaneFlags) 0 || math.any(laneSection.m_Bounds.max < this.m_Area.xy) || math.any(laneSection.m_Bounds.min > this.m_Area.zw))
          return;
        float4 float4_1;
        float4 float4_2;
        float2 float2;
        // ISSUE: reference to a compiler-generated field
        if ((laneSection.m_Flags & (TerrainSystem.LaneFlags.MiddleLeft | TerrainSystem.LaneFlags.MiddleRight)) == (TerrainSystem.LaneFlags.MiddleLeft | TerrainSystem.LaneFlags.MiddleRight))
        {
          // ISSUE: reference to a compiler-generated field
          float4_1 = new float4(laneSection.m_MinOffset.yyy, 1f);
          // ISSUE: reference to a compiler-generated field
          float4_2 = new float4(laneSection.m_MaxOffset.yyy, 1f);
          float2 = (float2) 0.0f;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if ((laneSection.m_Flags & TerrainSystem.LaneFlags.MiddleLeft) != (TerrainSystem.LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            float4_1 = new float4(laneSection.m_MinOffset.yyz, 0.6f);
            // ISSUE: reference to a compiler-generated field
            float4_2 = new float4(laneSection.m_MaxOffset.yyz, 0.6f);
            // ISSUE: reference to a compiler-generated field
            float2 = new float2(0.0f, laneSection.m_WidthOffset);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if ((laneSection.m_Flags & TerrainSystem.LaneFlags.MiddleRight) != (TerrainSystem.LaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              float4_1 = new float4(laneSection.m_MinOffset.xyy, 0.6f);
              // ISSUE: reference to a compiler-generated field
              float4_2 = new float4(laneSection.m_MaxOffset.xyy, 0.6f);
              // ISSUE: reference to a compiler-generated field
              float2 = new float2(laneSection.m_WidthOffset, 0.0f);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              float4_1 = new float4(laneSection.m_MinOffset, 0.8f);
              // ISSUE: reference to a compiler-generated field
              float4_2 = new float4(laneSection.m_MaxOffset, 0.8f);
              // ISSUE: reference to a compiler-generated field
              float2 = (float2) laneSection.m_WidthOffset;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TerrainSystem.LaneDraw laneDraw = new TerrainSystem.LaneDraw()
        {
          m_Left = laneSection.m_Left,
          m_Right = laneSection.m_Right,
          m_MinOffset = float4_1,
          m_MaxOffset = float4_2,
          m_WidthOffset = float2
        };
        // ISSUE: reference to a compiler-generated field
        this.Result.AddNoResize(laneDraw);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Updated> __Game_Common_Updated_RO_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Orphan> __Game_Net_Orphan_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> __Game_Net_NodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Game.Areas.Terrain> __Game_Areas_Terrain_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Clip> __Game_Areas_Clip_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Geometry> __Game_Areas_Geometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Lot> __Game_Buildings_Lot_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Stack> __Game_Objects_Stack_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> __Game_Prefabs_BuildingExtensionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AssetStampData> __Game_Prefabs_AssetStampData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingTerraformData> __Game_Prefabs_BuildingTerraformData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<AdditionalBuildingTerraformElement> __Game_Prefabs_AdditionalBuildingTerraformElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TerrainComposition> __Game_Prefabs_TerrainComposition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Area> __Game_Areas_Area_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Areas.Storage> __Game_Areas_Storage_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> __Game_Areas_Node_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Triangle> __Game_Areas_Triangle_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<TerrainAreaData> __Game_Prefabs_TerrainAreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StorageAreaData> __Game_Prefabs_StorageAreaData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RO_ComponentLookup = state.GetComponentLookup<Orphan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeGeometry_RO_ComponentLookup = state.GetComponentLookup<NodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Terrain_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Areas.Terrain>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Clip_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Clip>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Geometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Geometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Lot_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stack_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Stack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup = state.GetComponentLookup<BuildingExtensionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AssetStampData_RO_ComponentLookup = state.GetComponentLookup<AssetStampData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup = state.GetComponentLookup<BuildingTerraformData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AdditionalBuildingTerraformElement_RO_BufferLookup = state.GetBufferLookup<AdditionalBuildingTerraformElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TerrainComposition_RO_ComponentLookup = state.GetComponentLookup<TerrainComposition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Area_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Area>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Storage_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Areas.Storage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferTypeHandle = state.GetBufferTypeHandle<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TerrainAreaData_RO_ComponentLookup = state.GetComponentLookup<TerrainAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StorageAreaData_RO_ComponentLookup = state.GetComponentLookup<StorageAreaData>(true);
      }
    }
  }
}
