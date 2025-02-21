// Decompiled with JetBrains decompiler
// Type: Game.Rendering.OverlayInfomodeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.City;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class OverlayInfomodeSystem : GameSystemBase
  {
    private TerrainRenderSystem m_TerrainRenderSystem;
    private WaterRenderSystem m_WaterRenderSystem;
    private GroundWaterSystem m_GroundWaterSystem;
    private GroundPollutionSystem m_GroundPollutionSystem;
    private NoisePollutionSystem m_NoisePollutionSystem;
    private AirPollutionSystem m_AirPollutionSystem;
    private WindSystem m_WindSystem;
    private CitySystem m_CitySystem;
    private TelecomPreviewSystem m_TelecomCoverageSystem;
    private NaturalResourceSystem m_NaturalResourceSystem;
    private LandValueSystem m_LandValueSystem;
    private PopulationToGridSystem m_PopulationToGridSystem;
    private AvailabilityInfoToGridSystem m_AvailabilityInfoToGridSystem;
    private ToolSystem m_ToolSystem;
    private EntityQuery m_InfomodeQuery;
    private EntityQuery m_HappinessParameterQuery;
    private Texture2D m_TerrainTexture;
    private Texture2D m_WaterTexture;
    private Texture2D m_WindTexture;
    private JobHandle m_Dependency;
    private OverlayInfomodeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainRenderSystem = this.World.GetOrCreateSystemManaged<TerrainRenderSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterRenderSystem = this.World.GetOrCreateSystemManaged<WaterRenderSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundWaterSystem = this.World.GetOrCreateSystemManaged<GroundWaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionSystem = this.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NoisePollutionSystem = this.World.GetOrCreateSystemManaged<NoisePollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionSystem = this.World.GetOrCreateSystemManaged<AirPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WindSystem = this.World.GetOrCreateSystemManaged<WindSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomCoverageSystem = this.World.GetOrCreateSystemManaged<TelecomPreviewSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NaturalResourceSystem = this.World.GetOrCreateSystemManaged<NaturalResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LandValueSystem = this.World.GetOrCreateSystemManaged<LandValueSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PopulationToGridSystem = this.World.GetOrCreateSystemManaged<PopulationToGridSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AvailabilityInfoToGridSystem = this.World.GetOrCreateSystemManaged<AvailabilityInfoToGridSystem>();
      Texture2D texture2D1 = new Texture2D(1, 1, TextureFormat.RGBA32, false, true);
      texture2D1.name = "TerrainInfoTexture";
      texture2D1.hideFlags = HideFlags.HideAndDontSave;
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainTexture = texture2D1;
      Texture2D texture2D2 = new Texture2D(1, 1, TextureFormat.RGBA32, false, true);
      texture2D2.name = "WaterInfoTexture";
      texture2D2.hideFlags = HideFlags.HideAndDontSave;
      // ISSUE: reference to a compiler-generated field
      this.m_WaterTexture = texture2D2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Texture2D texture2D3 = new Texture2D(this.m_WindSystem.TextureSize.x, this.m_WindSystem.TextureSize.y, GraphicsFormat.R16G16B16A16_SFloat, 1, TextureCreationFlags.None);
      texture2D3.name = "WindInfoTexture";
      texture2D3.hideFlags = HideFlags.HideAndDontSave;
      // ISSUE: reference to a compiler-generated field
      this.m_WindTexture = texture2D3;
      // ISSUE: reference to a compiler-generated field
      this.m_InfomodeQuery = this.GetEntityQuery(ComponentType.ReadOnly<InfomodeActive>(), ComponentType.ReadOnly<InfoviewHeatmapData>());
      // ISSUE: reference to a compiler-generated field
      this.m_HappinessParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<CitizenHappinessParameterData>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((Object) this.m_TerrainTexture);
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((Object) this.m_WaterTexture);
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((Object) this.m_WindTexture);
      base.OnDestroy();
    }

    public void ApplyOverlay()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((Object) this.m_TerrainRenderSystem.overrideOverlaymap == (Object) this.m_TerrainTexture)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Dependency.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_TerrainTexture.Apply();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((Object) this.m_TerrainRenderSystem.overlayExtramap == (Object) this.m_WindTexture)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Dependency.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_WindTexture.Apply();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!((Object) this.m_WaterRenderSystem.overrideOverlaymap == (Object) this.m_WaterTexture))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_Dependency.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterTexture.Apply();
    }

    private NativeArray<byte> GetTerrainTextureData<T>(CellMapData<T> cellMapData) where T : struct, ISerializable
    {
      // ISSUE: reference to a compiler-generated method
      return this.GetTerrainTextureData(cellMapData.m_TextureSize);
    }

    private NativeArray<byte> GetTerrainTextureData(int2 size)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_TerrainTexture.width != size.x || this.m_TerrainTexture.height != size.y)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TerrainTexture.Reinitialize(size.x, size.y);
        // ISSUE: reference to a compiler-generated field
        this.m_TerrainRenderSystem.overrideOverlaymap = (Texture) null;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((Object) this.m_TerrainRenderSystem.overrideOverlaymap != (Object) this.m_TerrainTexture)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TerrainRenderSystem.overrideOverlaymap = (Texture) this.m_TerrainTexture;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        OverlayInfomodeSystem.ClearJob jobData = new OverlayInfomodeSystem.ClearJob()
        {
          m_TextureData = this.m_TerrainTexture.GetRawTextureData<byte>()
        };
        // ISSUE: reference to a compiler-generated field
        this.m_Dependency = jobData.Schedule<OverlayInfomodeSystem.ClearJob>(this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.Dependency = this.m_Dependency;
      }
      // ISSUE: reference to a compiler-generated field
      return this.m_TerrainTexture.GetRawTextureData<byte>();
    }

    private NativeArray<byte> GetWaterTextureData<T>(CellMapData<T> cellMapData) where T : struct, ISerializable
    {
      // ISSUE: reference to a compiler-generated method
      return this.GetWaterTextureData(cellMapData.m_TextureSize);
    }

    private NativeArray<byte> GetWaterTextureData(int2 size)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_WaterTexture.width != size.x || this.m_WaterTexture.height != size.y)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_WaterTexture.Reinitialize(size.x, size.y);
        // ISSUE: reference to a compiler-generated field
        this.m_WaterRenderSystem.overrideOverlaymap = (Texture) null;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((Object) this.m_WaterRenderSystem.overrideOverlaymap != (Object) this.m_WaterTexture)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_WaterRenderSystem.overrideOverlaymap = (Texture) this.m_WaterTexture;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        OverlayInfomodeSystem.ClearJob jobData = new OverlayInfomodeSystem.ClearJob()
        {
          m_TextureData = this.m_WaterTexture.GetRawTextureData<byte>()
        };
        // ISSUE: reference to a compiler-generated field
        this.m_Dependency = jobData.Schedule<OverlayInfomodeSystem.ClearJob>(this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.Dependency = this.m_Dependency;
      }
      // ISSUE: reference to a compiler-generated field
      return this.m_WaterTexture.GetRawTextureData<byte>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainRenderSystem.overrideOverlaymap = (Texture) null;
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainRenderSystem.overlayExtramap = (Texture) null;
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainRenderSystem.overlayArrowMask = new float4();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterRenderSystem.overrideOverlaymap = (Texture) null;
      // ISSUE: reference to a compiler-generated field
      this.m_WaterRenderSystem.overlayExtramap = (Texture) null;
      // ISSUE: reference to a compiler-generated field
      this.m_WaterRenderSystem.overlayPollutionMask = new float4();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterRenderSystem.overlayArrowMask = new float4();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_InfomodeQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_InfomodeQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfoviewHeatmapData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<InfoviewHeatmapData> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_InfoviewHeatmapData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<InfomodeActive> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle;
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          NativeArray<InfoviewHeatmapData> nativeArray1 = archetypeChunk.GetNativeArray<InfoviewHeatmapData>(ref componentTypeHandle1);
          NativeArray<InfomodeActive> nativeArray2 = archetypeChunk.GetNativeArray<InfomodeActive>(ref componentTypeHandle2);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            InfoviewHeatmapData infoviewHeatmapData = nativeArray1[index2];
            InfomodeActive infomodeActive = nativeArray2[index2];
            switch (infoviewHeatmapData.m_Type)
            {
              case HeatmapData.GroundWater:
                JobHandle dependencies1;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                OverlayInfomodeSystem.GroundWaterJob jobData1 = new OverlayInfomodeSystem.GroundWaterJob()
                {
                  m_ActiveData = infomodeActive,
                  m_MapData = this.m_GroundWaterSystem.GetData(true, out dependencies1)
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                jobData1.m_TextureData = this.GetTerrainTextureData<GroundWater>(jobData1.m_MapData);
                JobHandle jobHandle1 = jobData1.Schedule<OverlayInfomodeSystem.GroundWaterJob>(JobHandle.CombineDependencies(this.Dependency, dependencies1));
                // ISSUE: reference to a compiler-generated field
                this.m_GroundWaterSystem.AddReader(jobHandle1);
                // ISSUE: reference to a compiler-generated field
                this.m_Dependency = jobHandle1;
                this.Dependency = jobHandle1;
                break;
              case HeatmapData.GroundPollution:
                // ISSUE: reference to a compiler-generated field
                CitizenHappinessParameterData singleton1 = this.m_HappinessParameterQuery.GetSingleton<CitizenHappinessParameterData>();
                JobHandle dependencies2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                OverlayInfomodeSystem.GroundPollutionJob jobData2 = new OverlayInfomodeSystem.GroundPollutionJob()
                {
                  m_ActiveData = infomodeActive,
                  m_MapData = this.m_GroundPollutionSystem.GetData(true, out dependencies2),
                  m_Multiplier = (float) (256.0 / ((double) singleton1.m_MaxAirAndGroundPollutionBonus * (double) singleton1.m_PollutionBonusDivisor))
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                jobData2.m_TextureData = this.GetTerrainTextureData<GroundPollution>(jobData2.m_MapData);
                JobHandle jobHandle2 = jobData2.Schedule<OverlayInfomodeSystem.GroundPollutionJob>(JobHandle.CombineDependencies(this.Dependency, dependencies2));
                // ISSUE: reference to a compiler-generated field
                this.m_GroundPollutionSystem.AddReader(jobHandle2);
                // ISSUE: reference to a compiler-generated field
                this.m_Dependency = jobHandle2;
                this.Dependency = jobHandle2;
                break;
              case HeatmapData.AirPollution:
                // ISSUE: reference to a compiler-generated field
                CitizenHappinessParameterData singleton2 = this.m_HappinessParameterQuery.GetSingleton<CitizenHappinessParameterData>();
                JobHandle dependencies3;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                OverlayInfomodeSystem.AirPollutionJob jobData3 = new OverlayInfomodeSystem.AirPollutionJob()
                {
                  m_ActiveData = infomodeActive,
                  m_MapData = this.m_AirPollutionSystem.GetData(true, out dependencies3),
                  m_Multiplier = (float) (256.0 / ((double) singleton2.m_MaxAirAndGroundPollutionBonus * (double) singleton2.m_PollutionBonusDivisor))
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                jobData3.m_TextureData = this.GetTerrainTextureData<AirPollution>(jobData3.m_MapData);
                JobHandle jobHandle3 = jobData3.Schedule<OverlayInfomodeSystem.AirPollutionJob>(JobHandle.CombineDependencies(this.Dependency, dependencies3));
                // ISSUE: reference to a compiler-generated field
                this.m_AirPollutionSystem.AddReader(jobHandle3);
                // ISSUE: reference to a compiler-generated field
                this.m_Dependency = jobHandle3;
                this.Dependency = jobHandle3;
                break;
              case HeatmapData.Wind:
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_TerrainRenderSystem.overlayExtramap = (Texture) this.m_WindTexture;
                // ISSUE: reference to a compiler-generated field
                this.m_TerrainRenderSystem.overlayArrowMask = new float4()
                {
                  [infomodeActive.m_Index - 1] = 1f
                };
                JobHandle dependencies4;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                JobHandle jobHandle4 = new OverlayInfomodeSystem.WindJob()
                {
                  m_MapData = this.m_WindSystem.GetData(true, out dependencies4),
                  m_TextureData = this.m_WindTexture.GetRawTextureData<half4>()
                }.Schedule<OverlayInfomodeSystem.WindJob>(JobHandle.CombineDependencies(this.Dependency, dependencies4));
                // ISSUE: reference to a compiler-generated field
                this.m_WindSystem.AddReader(jobHandle4);
                // ISSUE: reference to a compiler-generated field
                this.m_Dependency = jobHandle4;
                this.Dependency = jobHandle4;
                break;
              case HeatmapData.WaterFlow:
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_WaterRenderSystem.overlayExtramap = this.m_WaterRenderSystem.flowTexture;
                // ISSUE: reference to a compiler-generated field
                this.m_WaterRenderSystem.overlayArrowMask = new float4()
                {
                  [infomodeActive.m_Index - 5] = 1f
                };
                break;
              case HeatmapData.TelecomCoverage:
                JobHandle dependencies5;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                OverlayInfomodeSystem.TelecomCoverageJob jobData4 = new OverlayInfomodeSystem.TelecomCoverageJob()
                {
                  m_ActiveData = infomodeActive,
                  m_MapData = this.m_TelecomCoverageSystem.GetData(true, out dependencies5)
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                jobData4.m_TextureData = this.GetTerrainTextureData<TelecomCoverage>(jobData4.m_MapData);
                JobHandle jobHandle5 = jobData4.Schedule<OverlayInfomodeSystem.TelecomCoverageJob>(JobHandle.CombineDependencies(this.Dependency, dependencies5));
                // ISSUE: reference to a compiler-generated field
                this.m_TelecomCoverageSystem.AddReader(jobHandle5);
                // ISSUE: reference to a compiler-generated field
                this.m_Dependency = jobHandle5;
                this.Dependency = jobHandle5;
                break;
              case HeatmapData.Fertility:
                JobHandle dependencies6;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                OverlayInfomodeSystem.FertilityJob jobData5 = new OverlayInfomodeSystem.FertilityJob()
                {
                  m_ActiveData = infomodeActive,
                  m_MapData = this.m_NaturalResourceSystem.GetData(true, out dependencies6)
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                jobData5.m_TextureData = this.GetTerrainTextureData<NaturalResourceCell>(jobData5.m_MapData);
                JobHandle jobHandle6 = jobData5.Schedule<OverlayInfomodeSystem.FertilityJob>(JobHandle.CombineDependencies(this.Dependency, dependencies6));
                // ISSUE: reference to a compiler-generated field
                this.m_NaturalResourceSystem.AddReader(jobHandle6);
                // ISSUE: reference to a compiler-generated field
                this.m_Dependency = jobHandle6;
                this.Dependency = jobHandle6;
                break;
              case HeatmapData.Ore:
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
                JobHandle dependencies7;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                OverlayInfomodeSystem.OreJob jobData6 = new OverlayInfomodeSystem.OreJob()
                {
                  m_ActiveData = infomodeActive,
                  m_MapData = this.m_NaturalResourceSystem.GetData(true, out dependencies7),
                  m_City = this.m_CitySystem.City,
                  m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                jobData6.m_TextureData = this.GetTerrainTextureData<NaturalResourceCell>(jobData6.m_MapData);
                JobHandle jobHandle7 = jobData6.Schedule<OverlayInfomodeSystem.OreJob>(JobHandle.CombineDependencies(this.Dependency, dependencies7));
                // ISSUE: reference to a compiler-generated field
                this.m_NaturalResourceSystem.AddReader(jobHandle7);
                // ISSUE: reference to a compiler-generated field
                this.m_Dependency = jobHandle7;
                this.Dependency = jobHandle7;
                break;
              case HeatmapData.Oil:
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
                JobHandle dependencies8;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                OverlayInfomodeSystem.OilJob jobData7 = new OverlayInfomodeSystem.OilJob()
                {
                  m_ActiveData = infomodeActive,
                  m_MapData = this.m_NaturalResourceSystem.GetData(true, out dependencies8),
                  m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                jobData7.m_TextureData = this.GetTerrainTextureData<NaturalResourceCell>(jobData7.m_MapData);
                JobHandle jobHandle8 = jobData7.Schedule<OverlayInfomodeSystem.OilJob>(JobHandle.CombineDependencies(this.Dependency, dependencies8));
                // ISSUE: reference to a compiler-generated field
                this.m_NaturalResourceSystem.AddReader(jobHandle8);
                // ISSUE: reference to a compiler-generated field
                this.m_Dependency = jobHandle8;
                this.Dependency = jobHandle8;
                break;
              case HeatmapData.LandValue:
                JobHandle dependencies9;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                OverlayInfomodeSystem.LandValueJob jobData8 = new OverlayInfomodeSystem.LandValueJob()
                {
                  m_ActiveData = infomodeActive,
                  m_MapData = this.m_LandValueSystem.GetData(true, out dependencies9)
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                jobData8.m_TextureData = this.GetTerrainTextureData<LandValueCell>(jobData8.m_MapData);
                JobHandle jobHandle9 = jobData8.Schedule<OverlayInfomodeSystem.LandValueJob>(JobHandle.CombineDependencies(dependencies9, this.Dependency));
                // ISSUE: reference to a compiler-generated field
                this.m_LandValueSystem.AddReader(jobHandle9);
                // ISSUE: reference to a compiler-generated field
                this.m_Dependency = jobHandle9;
                this.Dependency = jobHandle9;
                break;
              case HeatmapData.Attraction:
                JobHandle dependencies10;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                OverlayInfomodeSystem.AttractionJob jobData9 = new OverlayInfomodeSystem.AttractionJob()
                {
                  m_ActiveData = infomodeActive,
                  m_MapData = this.m_AvailabilityInfoToGridSystem.GetData(true, out dependencies10)
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                jobData9.m_TextureData = this.GetTerrainTextureData<AvailabilityInfoCell>(jobData9.m_MapData);
                JobHandle jobHandle10 = jobData9.Schedule<OverlayInfomodeSystem.AttractionJob>(JobHandle.CombineDependencies(this.Dependency, dependencies10));
                // ISSUE: reference to a compiler-generated field
                this.m_AvailabilityInfoToGridSystem.AddReader(jobHandle10);
                // ISSUE: reference to a compiler-generated field
                this.m_Dependency = jobHandle10;
                this.Dependency = jobHandle10;
                break;
              case HeatmapData.Customers:
                JobHandle dependencies11;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                OverlayInfomodeSystem.CustomerJob jobData10 = new OverlayInfomodeSystem.CustomerJob()
                {
                  m_ActiveData = infomodeActive,
                  m_MapData = this.m_AvailabilityInfoToGridSystem.GetData(true, out dependencies11)
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                jobData10.m_TextureData = this.GetTerrainTextureData<AvailabilityInfoCell>(jobData10.m_MapData);
                JobHandle jobHandle11 = jobData10.Schedule<OverlayInfomodeSystem.CustomerJob>(JobHandle.CombineDependencies(this.Dependency, dependencies11));
                // ISSUE: reference to a compiler-generated field
                this.m_AvailabilityInfoToGridSystem.AddReader(jobHandle11);
                // ISSUE: reference to a compiler-generated field
                this.m_Dependency = jobHandle11;
                this.Dependency = jobHandle11;
                break;
              case HeatmapData.Workplaces:
                JobHandle dependencies12;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                OverlayInfomodeSystem.WorkplaceJob jobData11 = new OverlayInfomodeSystem.WorkplaceJob()
                {
                  m_ActiveData = infomodeActive,
                  m_MapData = this.m_AvailabilityInfoToGridSystem.GetData(true, out dependencies12)
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                jobData11.m_TextureData = this.GetTerrainTextureData<AvailabilityInfoCell>(jobData11.m_MapData);
                JobHandle jobHandle12 = jobData11.Schedule<OverlayInfomodeSystem.WorkplaceJob>(JobHandle.CombineDependencies(this.Dependency, dependencies12));
                // ISSUE: reference to a compiler-generated field
                this.m_AvailabilityInfoToGridSystem.AddReader(jobHandle12);
                // ISSUE: reference to a compiler-generated field
                this.m_Dependency = jobHandle12;
                this.Dependency = jobHandle12;
                break;
              case HeatmapData.Services:
                JobHandle dependencies13;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                OverlayInfomodeSystem.ServiceJob jobData12 = new OverlayInfomodeSystem.ServiceJob()
                {
                  m_ActiveData = infomodeActive,
                  m_MapData = this.m_AvailabilityInfoToGridSystem.GetData(true, out dependencies13)
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                jobData12.m_TextureData = this.GetTerrainTextureData<AvailabilityInfoCell>(jobData12.m_MapData);
                JobHandle jobHandle13 = jobData12.Schedule<OverlayInfomodeSystem.ServiceJob>(JobHandle.CombineDependencies(this.Dependency, dependencies13));
                // ISSUE: reference to a compiler-generated field
                this.m_AvailabilityInfoToGridSystem.AddReader(jobHandle13);
                // ISSUE: reference to a compiler-generated field
                this.m_Dependency = jobHandle13;
                this.Dependency = jobHandle13;
                break;
              case HeatmapData.Noise:
                // ISSUE: reference to a compiler-generated field
                CitizenHappinessParameterData singleton3 = this.m_HappinessParameterQuery.GetSingleton<CitizenHappinessParameterData>();
                JobHandle dependencies14;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                OverlayInfomodeSystem.NoisePollutionJob jobData13 = new OverlayInfomodeSystem.NoisePollutionJob()
                {
                  m_ActiveData = infomodeActive,
                  m_MapData = this.m_NoisePollutionSystem.GetData(true, out dependencies14),
                  m_Multiplier = (float) (256.0 / ((double) singleton3.m_MaxNoisePollutionBonus * (double) singleton3.m_PollutionBonusDivisor))
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                jobData13.m_TextureData = this.GetTerrainTextureData<NoisePollution>(jobData13.m_MapData);
                JobHandle jobHandle14 = jobData13.Schedule<OverlayInfomodeSystem.NoisePollutionJob>(JobHandle.CombineDependencies(this.Dependency, dependencies14));
                // ISSUE: reference to a compiler-generated field
                this.m_NoisePollutionSystem.AddReader(jobHandle14);
                // ISSUE: reference to a compiler-generated field
                this.m_Dependency = jobHandle14;
                this.Dependency = jobHandle14;
                break;
              case HeatmapData.WaterPollution:
                // ISSUE: reference to a compiler-generated field
                this.m_WaterRenderSystem.overlayPollutionMask = new float4()
                {
                  [infomodeActive.m_Index - 5] = 1f
                };
                break;
              case HeatmapData.Population:
                JobHandle dependencies15;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                OverlayInfomodeSystem.PopulationJob jobData14 = new OverlayInfomodeSystem.PopulationJob()
                {
                  m_ActiveData = infomodeActive,
                  m_MapData = this.m_PopulationToGridSystem.GetData(true, out dependencies15)
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                jobData14.m_TextureData = this.GetTerrainTextureData<PopulationCell>(jobData14.m_MapData);
                JobHandle jobHandle15 = jobData14.Schedule<OverlayInfomodeSystem.PopulationJob>(JobHandle.CombineDependencies(dependencies15, this.Dependency));
                // ISSUE: reference to a compiler-generated field
                this.m_PopulationToGridSystem.AddReader(jobHandle15);
                // ISSUE: reference to a compiler-generated field
                this.m_Dependency = jobHandle15;
                this.Dependency = jobHandle15;
                break;
              case HeatmapData.GroundWaterPollution:
                JobHandle dependencies16;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                OverlayInfomodeSystem.GroundWaterPollutionJob jobData15 = new OverlayInfomodeSystem.GroundWaterPollutionJob()
                {
                  m_ActiveData = infomodeActive,
                  m_MapData = this.m_GroundWaterSystem.GetData(true, out dependencies16)
                };
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                jobData15.m_TextureData = this.GetTerrainTextureData<GroundWater>(jobData15.m_MapData);
                JobHandle jobHandle16 = jobData15.Schedule<OverlayInfomodeSystem.GroundWaterPollutionJob>(JobHandle.CombineDependencies(this.Dependency, dependencies16));
                // ISSUE: reference to a compiler-generated field
                this.m_GroundWaterSystem.AddReader(jobHandle16);
                // ISSUE: reference to a compiler-generated field
                this.m_Dependency = jobHandle16;
                this.Dependency = jobHandle16;
                break;
            }
          }
        }
        archetypeChunkArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!((Object) this.m_ToolSystem.activeInfoview != (Object) null))
        return;
      // ISSUE: reference to a compiler-generated field
      if ((Object) this.m_TerrainRenderSystem.overrideOverlaymap == (Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        this.GetTerrainTextureData((int2) 1);
      }
      // ISSUE: reference to a compiler-generated field
      if (!((Object) this.m_WaterRenderSystem.overrideOverlaymap == (Object) null))
        return;
      // ISSUE: reference to a compiler-generated method
      this.GetWaterTextureData((int2) 1);
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
    public OverlayInfomodeSystem()
    {
    }

    [BurstCompile]
    private struct ClearJob : IJob
    {
      public NativeArray<byte> m_TextureData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_TextureData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TextureData[index] = (byte) 0;
        }
      }
    }

    [BurstCompile]
    private struct GroundWaterJob : IJob
    {
      [ReadOnly]
      public InfomodeActive m_ActiveData;
      [ReadOnly]
      public CellMapData<GroundWater> m_MapData;
      public NativeArray<byte> m_TextureData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int num = this.m_ActiveData.m_Index - 1;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MapData.m_TextureSize.y; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_MapData.m_TextureSize.x; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index2 + index1 * this.m_MapData.m_TextureSize.x;
            // ISSUE: reference to a compiler-generated field
            GroundWater groundWater = this.m_MapData.m_Buffer[index3];
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index3 * 4 + num] = (byte) math.clamp((int) groundWater.m_Amount / 32, 0, (int) byte.MaxValue);
          }
        }
      }
    }

    [BurstCompile]
    private struct GroundPollutionJob : IJob
    {
      [ReadOnly]
      public InfomodeActive m_ActiveData;
      [ReadOnly]
      public CellMapData<GroundPollution> m_MapData;
      public NativeArray<byte> m_TextureData;
      public float m_Multiplier;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int num = this.m_ActiveData.m_Index - 1;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MapData.m_TextureSize.y; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_MapData.m_TextureSize.x; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index2 + index1 * this.m_MapData.m_TextureSize.x;
            // ISSUE: reference to a compiler-generated field
            GroundPollution groundPollution = this.m_MapData.m_Buffer[index3];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index3 * 4 + num] = (byte) math.clamp(Mathf.RoundToInt((float) groundPollution.m_Pollution * this.m_Multiplier), 0, (int) byte.MaxValue);
          }
        }
      }
    }

    [BurstCompile]
    private struct NoisePollutionJob : IJob
    {
      [ReadOnly]
      public InfomodeActive m_ActiveData;
      [ReadOnly]
      public CellMapData<NoisePollution> m_MapData;
      public NativeArray<byte> m_TextureData;
      public float m_Multiplier;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int num = this.m_ActiveData.m_Index - 1;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MapData.m_TextureSize.y; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_MapData.m_TextureSize.x; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index2 + index1 * this.m_MapData.m_TextureSize.x;
            // ISSUE: reference to a compiler-generated field
            NoisePollution noisePollution = this.m_MapData.m_Buffer[index3];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index3 * 4 + num] = (byte) math.clamp(Mathf.RoundToInt((float) noisePollution.m_Pollution * this.m_Multiplier), 0, (int) byte.MaxValue);
          }
        }
      }
    }

    [BurstCompile]
    private struct AirPollutionJob : IJob
    {
      [ReadOnly]
      public InfomodeActive m_ActiveData;
      [ReadOnly]
      public CellMapData<AirPollution> m_MapData;
      public NativeArray<byte> m_TextureData;
      public float m_Multiplier;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int num = this.m_ActiveData.m_Index - 1;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MapData.m_TextureSize.y; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_MapData.m_TextureSize.x; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index2 + index1 * this.m_MapData.m_TextureSize.x;
            // ISSUE: reference to a compiler-generated field
            AirPollution airPollution = this.m_MapData.m_Buffer[index3];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index3 * 4 + num] = (byte) math.clamp(Mathf.RoundToInt((float) airPollution.m_Pollution * this.m_Multiplier), 0, (int) byte.MaxValue);
          }
        }
      }
    }

    [BurstCompile]
    private struct WindJob : IJob
    {
      [ReadOnly]
      public CellMapData<Wind> m_MapData;
      public NativeArray<half4> m_TextureData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MapData.m_TextureSize.y; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_MapData.m_TextureSize.x; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index2 + index1 * this.m_MapData.m_TextureSize.x;
            // ISSUE: reference to a compiler-generated field
            Wind wind = this.m_MapData.m_Buffer[index3];
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index3] = new half4((half) wind.m_Wind.x, (half) wind.m_Wind.y, (half) 0.0f, (half) 0.0f);
          }
        }
      }
    }

    [BurstCompile]
    private struct TelecomCoverageJob : IJob
    {
      [ReadOnly]
      public InfomodeActive m_ActiveData;
      [ReadOnly]
      public CellMapData<TelecomCoverage> m_MapData;
      public NativeArray<byte> m_TextureData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int num = this.m_ActiveData.m_Index - 1;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MapData.m_TextureSize.y; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_MapData.m_TextureSize.x; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index2 + index1 * this.m_MapData.m_TextureSize.x;
            // ISSUE: reference to a compiler-generated field
            TelecomCoverage telecomCoverage = this.m_MapData.m_Buffer[index3];
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index3 * 4 + num] = (byte) math.clamp(telecomCoverage.networkQuality, 0, (int) byte.MaxValue);
          }
        }
      }
    }

    [BurstCompile]
    private struct FertilityJob : IJob
    {
      [ReadOnly]
      public InfomodeActive m_ActiveData;
      [ReadOnly]
      public CellMapData<NaturalResourceCell> m_MapData;
      public NativeArray<byte> m_TextureData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int num1 = this.m_ActiveData.m_Index - 1;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MapData.m_TextureSize.y; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_MapData.m_TextureSize.x; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index2 + index1 * this.m_MapData.m_TextureSize.x;
            // ISSUE: reference to a compiler-generated field
            NaturalResourceCell naturalResourceCell = this.m_MapData.m_Buffer[index3];
            float num2 = math.saturate(((float) naturalResourceCell.m_Fertility.m_Base - (float) naturalResourceCell.m_Fertility.m_Used) * 0.0001f);
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index3 * 4 + num1] = (byte) math.clamp(Mathf.RoundToInt(num2 * (float) byte.MaxValue), 0, (int) byte.MaxValue);
          }
        }
      }
    }

    [BurstCompile]
    private struct OreJob : IJob
    {
      [ReadOnly]
      public InfomodeActive m_ActiveData;
      [ReadOnly]
      public CellMapData<NaturalResourceCell> m_MapData;
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      public NativeArray<byte> m_TextureData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int num1 = this.m_ActiveData.m_Index - 1;
        DynamicBuffer<CityModifier> modifiers = new DynamicBuffer<CityModifier>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CityModifiers.HasBuffer(this.m_City))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          modifiers = this.m_CityModifiers[this.m_City];
        }
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MapData.m_TextureSize.y; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_MapData.m_TextureSize.x; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index2 + index1 * this.m_MapData.m_TextureSize.x;
            // ISSUE: reference to a compiler-generated field
            NaturalResourceCell naturalResourceCell = this.m_MapData.m_Buffer[index3];
            float num2 = (float) naturalResourceCell.m_Ore.m_Base;
            if (modifiers.IsCreated)
              CityUtils.ApplyModifier(ref num2, modifiers, CityModifierType.OreResourceAmount);
            num2 -= (float) naturalResourceCell.m_Ore.m_Used;
            num2 = math.saturate(num2 * 0.0001f);
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index3 * 4 + num1] = (byte) math.clamp(Mathf.RoundToInt(num2 * (float) byte.MaxValue), 0, (int) byte.MaxValue);
          }
        }
      }
    }

    [BurstCompile]
    private struct OilJob : IJob
    {
      [ReadOnly]
      public InfomodeActive m_ActiveData;
      [ReadOnly]
      public CellMapData<NaturalResourceCell> m_MapData;
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      public NativeArray<byte> m_TextureData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int num1 = this.m_ActiveData.m_Index - 1;
        DynamicBuffer<CityModifier> modifiers = new DynamicBuffer<CityModifier>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CityModifiers.HasBuffer(this.m_City))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          modifiers = this.m_CityModifiers[this.m_City];
        }
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MapData.m_TextureSize.y; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_MapData.m_TextureSize.x; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index2 + index1 * this.m_MapData.m_TextureSize.x;
            // ISSUE: reference to a compiler-generated field
            NaturalResourceCell naturalResourceCell = this.m_MapData.m_Buffer[index3];
            float num2 = (float) naturalResourceCell.m_Oil.m_Base;
            if (modifiers.IsCreated)
              CityUtils.ApplyModifier(ref num2, modifiers, CityModifierType.OilResourceAmount);
            num2 -= (float) naturalResourceCell.m_Oil.m_Used;
            num2 = math.saturate(num2 * 0.0001f);
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index3 * 4 + num1] = (byte) math.clamp(Mathf.RoundToInt(num2 * (float) byte.MaxValue), 0, (int) byte.MaxValue);
          }
        }
      }
    }

    [BurstCompile]
    private struct LandValueJob : IJob
    {
      [ReadOnly]
      public InfomodeActive m_ActiveData;
      [ReadOnly]
      public CellMapData<LandValueCell> m_MapData;
      public NativeArray<byte> m_TextureData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int num = this.m_ActiveData.m_Index - 1;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MapData.m_TextureSize.y; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_MapData.m_TextureSize.x; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index2 + index1 * this.m_MapData.m_TextureSize.x;
            // ISSUE: reference to a compiler-generated field
            LandValueCell landValueCell = this.m_MapData.m_Buffer[index3];
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index3 * 4 + num] = (byte) math.clamp(Mathf.RoundToInt(landValueCell.m_LandValue * 0.51f), 0, (int) byte.MaxValue);
          }
        }
      }
    }

    [BurstCompile]
    private struct PopulationJob : IJob
    {
      [ReadOnly]
      public InfomodeActive m_ActiveData;
      [ReadOnly]
      public CellMapData<PopulationCell> m_MapData;
      public NativeArray<byte> m_TextureData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int num = this.m_ActiveData.m_Index - 1;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MapData.m_TextureSize.y; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_MapData.m_TextureSize.x; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index2 + index1 * this.m_MapData.m_TextureSize.x;
            // ISSUE: reference to a compiler-generated field
            PopulationCell populationCell = this.m_MapData.m_Buffer[index3];
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index3 * 4 + num] = (byte) math.clamp(Mathf.RoundToInt(populationCell.Get() * 0.249023438f), 0, (int) byte.MaxValue);
          }
        }
      }
    }

    [BurstCompile]
    private struct AttractionJob : IJob
    {
      [ReadOnly]
      public InfomodeActive m_ActiveData;
      [ReadOnly]
      public CellMapData<AvailabilityInfoCell> m_MapData;
      public NativeArray<byte> m_TextureData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int num = this.m_ActiveData.m_Index - 1;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MapData.m_TextureSize.y; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_MapData.m_TextureSize.x; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index2 + index1 * this.m_MapData.m_TextureSize.x;
            // ISSUE: reference to a compiler-generated field
            AvailabilityInfoCell availabilityInfoCell = this.m_MapData.m_Buffer[index3];
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index3 * 4 + num] = (byte) math.clamp(Mathf.RoundToInt(availabilityInfoCell.m_AvailabilityInfo.x * ((float) byte.MaxValue / 16f)), 0, (int) byte.MaxValue);
          }
        }
      }
    }

    [BurstCompile]
    private struct CustomerJob : IJob
    {
      [ReadOnly]
      public InfomodeActive m_ActiveData;
      [ReadOnly]
      public CellMapData<AvailabilityInfoCell> m_MapData;
      public NativeArray<byte> m_TextureData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int num = this.m_ActiveData.m_Index - 1;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MapData.m_TextureSize.y; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_MapData.m_TextureSize.x; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index2 + index1 * this.m_MapData.m_TextureSize.x;
            // ISSUE: reference to a compiler-generated field
            AvailabilityInfoCell availabilityInfoCell = this.m_MapData.m_Buffer[index3];
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index3 * 4 + num] = (byte) math.clamp(Mathf.RoundToInt(availabilityInfoCell.m_AvailabilityInfo.y * ((float) byte.MaxValue / 16f)), 0, (int) byte.MaxValue);
          }
        }
      }
    }

    [BurstCompile]
    private struct WorkplaceJob : IJob
    {
      [ReadOnly]
      public InfomodeActive m_ActiveData;
      [ReadOnly]
      public CellMapData<AvailabilityInfoCell> m_MapData;
      public NativeArray<byte> m_TextureData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int num = this.m_ActiveData.m_Index - 1;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MapData.m_TextureSize.y; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_MapData.m_TextureSize.x; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index2 + index1 * this.m_MapData.m_TextureSize.x;
            // ISSUE: reference to a compiler-generated field
            AvailabilityInfoCell availabilityInfoCell = this.m_MapData.m_Buffer[index3];
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index3 * 4 + num] = (byte) math.clamp(Mathf.RoundToInt(availabilityInfoCell.m_AvailabilityInfo.z * ((float) byte.MaxValue / 16f)), 0, (int) byte.MaxValue);
          }
        }
      }
    }

    [BurstCompile]
    private struct ServiceJob : IJob
    {
      [ReadOnly]
      public InfomodeActive m_ActiveData;
      [ReadOnly]
      public CellMapData<AvailabilityInfoCell> m_MapData;
      public NativeArray<byte> m_TextureData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int num = this.m_ActiveData.m_Index - 1;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MapData.m_TextureSize.y; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_MapData.m_TextureSize.x; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index2 + index1 * this.m_MapData.m_TextureSize.x;
            // ISSUE: reference to a compiler-generated field
            AvailabilityInfoCell availabilityInfoCell = this.m_MapData.m_Buffer[index3];
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index3 * 4 + num] = (byte) math.clamp(Mathf.RoundToInt(availabilityInfoCell.m_AvailabilityInfo.w * ((float) byte.MaxValue / 16f)), 0, (int) byte.MaxValue);
          }
        }
      }
    }

    [BurstCompile]
    private struct GroundWaterPollutionJob : IJob
    {
      [ReadOnly]
      public InfomodeActive m_ActiveData;
      [ReadOnly]
      public CellMapData<GroundWater> m_MapData;
      public NativeArray<byte> m_TextureData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int num = this.m_ActiveData.m_Index - 1;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MapData.m_TextureSize.y; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_MapData.m_TextureSize.x; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index2 + index1 * this.m_MapData.m_TextureSize.x;
            // ISSUE: reference to a compiler-generated field
            GroundWater groundWater = this.m_MapData.m_Buffer[index3];
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index3 * 4 + num] = (byte) math.clamp(math.min((int) groundWater.m_Amount / 32, (int) groundWater.m_Polluted * 256 / math.max(1, (int) groundWater.m_Amount)), 0, (int) byte.MaxValue);
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<InfoviewHeatmapData> __Game_Prefabs_InfoviewHeatmapData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfomodeActive> __Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewHeatmapData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewHeatmapData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfomodeActive_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfomodeActive>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
