// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WindSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Rendering;
using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  public class WindSystem : CellMapSystem<Wind>, IJobSerializable
  {
    public static readonly int kTextureSize = 64;
    public static readonly int kUpdateInterval = 512;
    public WindSimulationSystem m_WindSimulationSystem;
    public WindTextureSystem m_WindTextureSystem;
    public TerrainSystem m_TerrainSystem;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      return phase != SystemUpdatePhase.GameSimulation ? 1 : WindSystem.kUpdateInterval;
    }

    public int2 TextureSize => new int2(WindSystem.kTextureSize, WindSystem.kTextureSize);

    public static float3 GetCellCenter(int index)
    {
      return CellMapSystem<Wind>.GetCellCenter(index, WindSystem.kTextureSize);
    }

    public static Wind GetWind(float3 position, NativeArray<Wind> windMap)
    {
      int2 int2 = math.clamp(CellMapSystem<Wind>.GetCell(position, CellMapSystem<Wind>.kMapSize, WindSystem.kTextureSize), (int2) 0, (int2) (WindSystem.kTextureSize - 1));
      float2 cellCoords = CellMapSystem<Wind>.GetCellCoords(position, CellMapSystem<Wind>.kMapSize, WindSystem.kTextureSize);
      int num1 = math.min(WindSystem.kTextureSize - 1, int2.x + 1);
      int num2 = math.min(WindSystem.kTextureSize - 1, int2.y + 1);
      return new Wind()
      {
        m_Wind = math.lerp(math.lerp(windMap[int2.x + WindSystem.kTextureSize * int2.y].m_Wind, windMap[num1 + WindSystem.kTextureSize * int2.y].m_Wind, cellCoords.x - (float) int2.x), math.lerp(windMap[int2.x + WindSystem.kTextureSize * num2].m_Wind, windMap[num1 + WindSystem.kTextureSize * num2].m_Wind, cellCoords.x - (float) int2.x), cellCoords.y - (float) int2.y)
      };
    }

    public override JobHandle Deserialize<TReader>(EntityReaderData readerData, JobHandle inputDeps)
    {
      this.m_WindTextureSystem.RequireUpdate();
      if (readerData.GetReader<TReader>().context.version > Game.Version.cellMapLengths)
        return base.Deserialize<TReader>(readerData, inputDeps);
      this.m_Map.Dispose();
      this.m_Map = new NativeArray<Wind>(65536, Allocator.Persistent);
      inputDeps = base.Deserialize<TReader>(readerData, inputDeps);
      inputDeps.Complete();
      this.m_Map.Dispose();
      this.m_Map = new NativeArray<Wind>(WindSystem.kTextureSize * WindSystem.kTextureSize, Allocator.Persistent);
      return inputDeps;
    }

    public override JobHandle SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      this.m_WindTextureSystem.RequireUpdate();
      for (int index = 0; index < this.m_Map.Length; ++index)
        this.m_Map[index] = new Wind()
        {
          m_Wind = this.m_WindSimulationSystem.constantWind
        };
      return new JobHandle();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_WindSimulationSystem = this.World.GetOrCreateSystemManaged<WindSimulationSystem>();
      this.m_WindTextureSystem = this.World.GetOrCreateSystemManaged<WindTextureSystem>();
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      this.CreateTextures(WindSystem.kTextureSize);
      for (int index = 0; index < this.m_Map.Length; ++index)
        this.m_Map[index] = new Wind()
        {
          m_Wind = this.m_WindSimulationSystem.constantWind
        };
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      TerrainHeightData heightData = this.m_TerrainSystem.GetHeightData();
      if (!heightData.isCreated)
        return;
      JobHandle deps;
      this.Dependency = new WindSystem.WindCopyJob()
      {
        m_WindMap = this.m_Map,
        m_Source = this.m_WindSimulationSystem.GetCells(out deps),
        m_TerrainHeightData = heightData
      }.Schedule<WindSystem.WindCopyJob>(this.m_Map.Length, JobHandle.CombineDependencies(deps, JobHandle.CombineDependencies(this.m_WriteDependencies, this.m_ReadDependencies, this.Dependency)));
      this.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(this.Dependency);
      this.m_WindSimulationSystem.AddReader(this.Dependency);
      this.m_WindTextureSystem.RequireUpdate();
    }

    [UnityEngine.Scripting.Preserve]
    public WindSystem()
    {
    }

    [BurstCompile]
    private struct WindCopyJob : IJobFor
    {
      public NativeArray<Wind> m_WindMap;
      [ReadOnly]
      public NativeArray<WindSimulationSystem.WindCell> m_Source;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;

      public void Execute(int index)
      {
        float3 cellCenter = WindSimulationSystem.GetCellCenter(index);
        cellCenter.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, cellCenter) + 25f;
        float num = math.max(0.0f, (float) ((double) WindSimulationSystem.kResolution.z * ((double) cellCenter.y - (double) TerrainUtils.ToWorldSpace(ref this.m_TerrainHeightData, 0.0f)) / (double) TerrainUtils.ToWorldSpace(ref this.m_TerrainHeightData, (float) ushort.MaxValue) - 0.5));
        int3 cell1 = new int3(index % WindSystem.kTextureSize, index / WindSystem.kTextureSize, Math.Min(Mathf.FloorToInt(num), WindSimulationSystem.kResolution.z - 1));
        int3 cell2 = new int3(cell1.x, cell1.y, Math.Min(cell1.z + 1, WindSimulationSystem.kResolution.z - 1));
        float2 xy1 = WindSimulationSystem.GetCenterVelocity(cell1, this.m_Source).xy;
        NativeArray<WindSimulationSystem.WindCell> source = this.m_Source;
        float2 xy2 = WindSimulationSystem.GetCenterVelocity(cell2, source).xy;
        float2 float2 = math.lerp(xy1, xy2, math.frac(num));
        this.m_WindMap[index] = new Wind()
        {
          m_Wind = float2
        };
      }
    }
  }
}
