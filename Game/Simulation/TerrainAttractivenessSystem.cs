// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TerrainAttractivenessSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TerrainAttractivenessSystem : CellMapSystem<TerrainAttractiveness>, IJobSerializable
  {
    public static readonly int kTextureSize = 128;
    public static readonly int kUpdatesPerDay = 16;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private ZoneAmbienceSystem m_ZoneAmbienceSystem;
    private EntityQuery m_AttractivenessParameterGroup;
    private NativeArray<float3> m_AttractFactorData;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / TerrainAttractivenessSystem.kUpdatesPerDay;
    }

    public int2 TextureSize
    {
      get
      {
        return new int2(TerrainAttractivenessSystem.kTextureSize, TerrainAttractivenessSystem.kTextureSize);
      }
    }

    public static float3 GetCellCenter(int index)
    {
      // ISSUE: reference to a compiler-generated field
      return CellMapSystem<TerrainAttractiveness>.GetCellCenter(index, TerrainAttractivenessSystem.kTextureSize);
    }

    public static float EvaluateAttractiveness(
      float terrainHeight,
      TerrainAttractiveness attractiveness,
      AttractivenessParameterData parameters)
    {
      double num1 = (double) parameters.m_ForestEffect * (double) attractiveness.m_ForestBonus;
      float num2 = parameters.m_ShoreEffect * attractiveness.m_ShoreBonus;
      float num3 = math.min(parameters.m_HeightBonus.z, math.max(0.0f, terrainHeight - parameters.m_HeightBonus.x) * parameters.m_HeightBonus.y);
      double num4 = (double) num2;
      return (float) (num1 + num4) + num3;
    }

    public static float EvaluateAttractiveness(
      float3 position,
      CellMapData<TerrainAttractiveness> data,
      TerrainHeightData heightData,
      AttractivenessParameterData parameters,
      NativeArray<int> factors)
    {
      float num = TerrainUtils.SampleHeight(ref heightData, position);
      // ISSUE: reference to a compiler-generated method
      TerrainAttractiveness attractiveness1 = TerrainAttractivenessSystem.GetAttractiveness(position, data.m_Buffer);
      float attractiveness2 = parameters.m_ForestEffect * attractiveness1.m_ForestBonus;
      // ISSUE: reference to a compiler-generated method
      AttractionSystem.SetFactor(factors, AttractionSystem.AttractivenessFactor.Forest, attractiveness2);
      float attractiveness3 = parameters.m_ShoreEffect * attractiveness1.m_ShoreBonus;
      // ISSUE: reference to a compiler-generated method
      AttractionSystem.SetFactor(factors, AttractionSystem.AttractivenessFactor.Beach, attractiveness3);
      float attractiveness4 = math.min(parameters.m_HeightBonus.z, math.max(0.0f, num - parameters.m_HeightBonus.x) * parameters.m_HeightBonus.y);
      // ISSUE: reference to a compiler-generated method
      AttractionSystem.SetFactor(factors, AttractionSystem.AttractivenessFactor.Height, attractiveness4);
      return attractiveness2 + attractiveness3 + attractiveness4;
    }

    public static TerrainAttractiveness GetAttractiveness(
      float3 position,
      NativeArray<TerrainAttractiveness> attractivenessMap)
    {
      TerrainAttractiveness attractiveness1 = new TerrainAttractiveness();
      // ISSUE: reference to a compiler-generated field
      int2 cell = CellMapSystem<TerrainAttractiveness>.GetCell(position, CellMapSystem<TerrainAttractiveness>.kMapSize, TerrainAttractivenessSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      float2 cellCoords = CellMapSystem<TerrainAttractiveness>.GetCellCoords(position, CellMapSystem<TerrainAttractiveness>.kMapSize, TerrainAttractivenessSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (cell.x < 0 || cell.x >= TerrainAttractivenessSystem.kTextureSize || cell.y < 0 || cell.y >= TerrainAttractivenessSystem.kTextureSize)
        return attractiveness1;
      // ISSUE: reference to a compiler-generated field
      TerrainAttractiveness attractiveness2 = attractivenessMap[cell.x + TerrainAttractivenessSystem.kTextureSize * cell.y];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      TerrainAttractiveness terrainAttractiveness1 = cell.x < TerrainAttractivenessSystem.kTextureSize - 1 ? attractivenessMap[cell.x + 1 + TerrainAttractivenessSystem.kTextureSize * cell.y] : new TerrainAttractiveness();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      TerrainAttractiveness terrainAttractiveness2 = cell.y < TerrainAttractivenessSystem.kTextureSize - 1 ? attractivenessMap[cell.x + TerrainAttractivenessSystem.kTextureSize * (cell.y + 1)] : new TerrainAttractiveness();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      TerrainAttractiveness terrainAttractiveness3 = cell.x >= TerrainAttractivenessSystem.kTextureSize - 1 || cell.y >= TerrainAttractivenessSystem.kTextureSize - 1 ? new TerrainAttractiveness() : attractivenessMap[cell.x + 1 + TerrainAttractivenessSystem.kTextureSize * (cell.y + 1)];
      attractiveness1.m_ForestBonus = (float) (short) Mathf.RoundToInt(math.lerp(math.lerp(attractiveness2.m_ForestBonus, terrainAttractiveness1.m_ForestBonus, cellCoords.x - (float) cell.x), math.lerp(terrainAttractiveness2.m_ForestBonus, terrainAttractiveness3.m_ForestBonus, cellCoords.x - (float) cell.x), cellCoords.y - (float) cell.y));
      attractiveness1.m_ShoreBonus = (float) (short) Mathf.RoundToInt(math.lerp(math.lerp(attractiveness2.m_ShoreBonus, terrainAttractiveness1.m_ShoreBonus, cellCoords.x - (float) cell.x), math.lerp(terrainAttractiveness2.m_ShoreBonus, terrainAttractiveness3.m_ShoreBonus, cellCoords.x - (float) cell.x), cellCoords.y - (float) cell.y));
      return attractiveness1;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.CreateTextures(TerrainAttractivenessSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneAmbienceSystem = this.World.GetOrCreateSystemManaged<ZoneAmbienceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AttractivenessParameterGroup = this.GetEntityQuery(ComponentType.ReadOnly<AttractivenessParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AttractFactorData = new NativeArray<float3>(this.m_Map.Length, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_AttractFactorData.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      TerrainHeightData heightData = this.m_TerrainSystem.GetHeightData();
      JobHandle deps;
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TerrainAttractivenessSystem.TerrainAttractivenessPrepareJob jobData1 = new TerrainAttractivenessSystem.TerrainAttractivenessPrepareJob()
      {
        m_AttractFactorData = this.m_AttractFactorData,
        m_TerrainData = heightData,
        m_WaterData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_ZoneAmbienceData = this.m_ZoneAmbienceSystem.GetData(true, out dependencies)
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TerrainAttractivenessSystem.TerrainAttractivenessJob jobData2 = new TerrainAttractivenessSystem.TerrainAttractivenessJob()
      {
        m_Scale = heightData.scale.x * (float) TerrainAttractivenessSystem.kTextureSize,
        m_AttractFactorData = this.m_AttractFactorData,
        m_AttractivenessMap = this.m_Map,
        m_AttractivenessParameters = this.m_AttractivenessParameterGroup.GetSingleton<AttractivenessParameterData>()
      };
      int length = this.m_Map.Length;
      JobHandle dependsOn = JobHandle.CombineDependencies(deps, dependencies, this.Dependency);
      JobHandle jobHandle = jobData1.ScheduleBatch<TerrainAttractivenessSystem.TerrainAttractivenessPrepareJob>(length, 4, dependsOn);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneAmbienceSystem.AddReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(jobHandle);
      this.Dependency = jobData2.ScheduleBatch<TerrainAttractivenessSystem.TerrainAttractivenessJob>(this.m_Map.Length, 4, JobHandle.CombineDependencies(this.m_WriteDependencies, this.m_ReadDependencies, jobHandle));
      this.AddWriter(this.Dependency);
      this.Dependency = JobHandle.CombineDependencies(this.m_ReadDependencies, this.m_WriteDependencies, this.Dependency);
    }

    [UnityEngine.Scripting.Preserve]
    public TerrainAttractivenessSystem()
    {
    }

    [BurstCompile]
    private struct TerrainAttractivenessPrepareJob : IJobParallelForBatch
    {
      [ReadOnly]
      public TerrainHeightData m_TerrainData;
      [ReadOnly]
      public WaterSurfaceData m_WaterData;
      [ReadOnly]
      public CellMapData<ZoneAmbienceCell> m_ZoneAmbienceData;
      public NativeArray<float3> m_AttractFactorData;

      public void Execute(int startIndex, int count)
      {
        for (int index = startIndex; index < startIndex + count; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          float3 cellCenter = TerrainAttractivenessSystem.GetCellCenter(index);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_AttractFactorData[index] = new float3(WaterUtils.SampleDepth(ref this.m_WaterData, cellCenter), TerrainUtils.SampleHeight(ref this.m_TerrainData, cellCenter), ZoneAmbienceSystem.GetZoneAmbience(GroupAmbienceType.Forest, cellCenter, this.m_ZoneAmbienceData.m_Buffer, 1f));
        }
      }
    }

    [BurstCompile]
    private struct TerrainAttractivenessJob : IJobParallelForBatch
    {
      [ReadOnly]
      public NativeArray<float3> m_AttractFactorData;
      [ReadOnly]
      public float m_Scale;
      public NativeArray<TerrainAttractiveness> m_AttractivenessMap;
      public AttractivenessParameterData m_AttractivenessParameters;

      public void Execute(int startIndex, int count)
      {
        for (int index1 = startIndex; index1 < startIndex + count; ++index1)
        {
          // ISSUE: reference to a compiler-generated method
          float3 cellCenter = TerrainAttractivenessSystem.GetCellCenter(index1);
          float2 float2 = (float2) 0;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int num1 = Mathf.CeilToInt(math.max(this.m_AttractivenessParameters.m_ForestDistance, this.m_AttractivenessParameters.m_ShoreDistance) / this.m_Scale);
          for (int index2 = -num1; index2 <= num1; ++index2)
          {
            for (int index3 = -num1; index3 <= num1; ++index3)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int index4 = math.min(TerrainAttractivenessSystem.kTextureSize - 1, math.max(0, index1 % TerrainAttractivenessSystem.kTextureSize + index2)) + math.min(TerrainAttractivenessSystem.kTextureSize - 1, math.max(0, index1 / TerrainAttractivenessSystem.kTextureSize + index3)) * TerrainAttractivenessSystem.kTextureSize;
              // ISSUE: reference to a compiler-generated field
              float3 float3 = this.m_AttractFactorData[index4];
              // ISSUE: reference to a compiler-generated method
              float num2 = math.distance(TerrainAttractivenessSystem.GetCellCenter(index4), cellCenter);
              // ISSUE: reference to a compiler-generated field
              float2.x = math.max(float2.x, math.saturate((float) (1.0 - (double) num2 / (double) this.m_AttractivenessParameters.m_ForestDistance)) * float3.z);
              // ISSUE: reference to a compiler-generated field
              float2.y = math.max(float2.y, math.saturate((float) (1.0 - (double) num2 / (double) this.m_AttractivenessParameters.m_ShoreDistance)) * ((double) float3.x > 2.0 ? 1f : 0.0f));
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_AttractivenessMap[index1] = new TerrainAttractiveness()
          {
            m_ForestBonus = float2.x,
            m_ShoreBonus = float2.y
          };
        }
      }
    }
  }
}
