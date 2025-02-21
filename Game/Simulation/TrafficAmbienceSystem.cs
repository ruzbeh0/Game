// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TrafficAmbienceSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public class TrafficAmbienceSystem : CellMapSystem<TrafficAmbienceCell>, IJobSerializable
  {
    public static readonly int kTextureSize = 64;
    public static readonly int kUpdatesPerDay = 1024;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      return 262144 / TrafficAmbienceSystem.kUpdatesPerDay;
    }

    public int2 TextureSize
    {
      get => new int2(TrafficAmbienceSystem.kTextureSize, TrafficAmbienceSystem.kTextureSize);
    }

    public static float3 GetCellCenter(int index)
    {
      return CellMapSystem<TrafficAmbienceCell>.GetCellCenter(index, TrafficAmbienceSystem.kTextureSize);
    }

    public static TrafficAmbienceCell GetTrafficAmbience2(
      float3 position,
      NativeArray<TrafficAmbienceCell> trafficAmbienceMap,
      float maxPerCell)
    {
      TrafficAmbienceCell trafficAmbience2 = new TrafficAmbienceCell();
      int2 cell = CellMapSystem<TrafficAmbienceCell>.GetCell(position, CellMapSystem<TrafficAmbienceCell>.kMapSize, TrafficAmbienceSystem.kTextureSize);
      float num1 = 0.0f;
      float num2 = 0.0f;
      for (int index1 = cell.x - 2; index1 <= cell.x + 2; ++index1)
      {
        for (int index2 = cell.y - 2; index2 <= cell.y + 2; ++index2)
        {
          if (index1 >= 0 && index1 < TrafficAmbienceSystem.kTextureSize && index2 >= 0 && index2 < TrafficAmbienceSystem.kTextureSize)
          {
            int index3 = index1 + TrafficAmbienceSystem.kTextureSize * index2;
            float num3 = math.max(1f, math.distancesq(TrafficAmbienceSystem.GetCellCenter(index3), position));
            num1 += math.min(maxPerCell, trafficAmbienceMap[index3].m_Traffic) / num3;
            num2 += 1f / num3;
          }
        }
      }
      trafficAmbience2.m_Traffic = num1 / num2;
      return trafficAmbience2;
    }

    public static TrafficAmbienceCell GetTrafficAmbience(
      float3 position,
      NativeArray<TrafficAmbienceCell> trafficAmbienceMap)
    {
      TrafficAmbienceCell trafficAmbience = new TrafficAmbienceCell();
      int2 cell = CellMapSystem<TrafficAmbienceCell>.GetCell(position, CellMapSystem<TrafficAmbienceCell>.kMapSize, TrafficAmbienceSystem.kTextureSize);
      if (cell.x < 0 || cell.x >= TrafficAmbienceSystem.kTextureSize || cell.y < 0 || cell.y >= TrafficAmbienceSystem.kTextureSize)
        return new TrafficAmbienceCell()
        {
          m_Accumulator = 0.0f,
          m_Traffic = 0.0f
        };
      float2 cellCoords = CellMapSystem<TrafficAmbienceCell>.GetCellCoords(position, CellMapSystem<TrafficAmbienceCell>.kMapSize, TrafficAmbienceSystem.kTextureSize);
      float traffic = trafficAmbienceMap[cell.x + TrafficAmbienceSystem.kTextureSize * cell.y].m_Traffic;
      float y1 = cell.x < TrafficAmbienceSystem.kTextureSize - 1 ? trafficAmbienceMap[cell.x + 1 + TrafficAmbienceSystem.kTextureSize * cell.y].m_Traffic : 0.0f;
      float x = cell.y < TrafficAmbienceSystem.kTextureSize - 1 ? trafficAmbienceMap[cell.x + TrafficAmbienceSystem.kTextureSize * (cell.y + 1)].m_Traffic : 0.0f;
      float y2 = cell.x >= TrafficAmbienceSystem.kTextureSize - 1 || cell.y >= TrafficAmbienceSystem.kTextureSize - 1 ? 0.0f : trafficAmbienceMap[cell.x + 1 + TrafficAmbienceSystem.kTextureSize * (cell.y + 1)].m_Traffic;
      trafficAmbience.m_Traffic = math.lerp(math.lerp(traffic, y1, cellCoords.x - (float) cell.x), math.lerp(x, y2, cellCoords.x - (float) cell.x), cellCoords.y - (float) cell.y);
      return trafficAmbience;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.CreateTextures(TrafficAmbienceSystem.kTextureSize);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      this.Dependency = new TrafficAmbienceSystem.TrafficAmbienceUpdateJob()
      {
        m_TrafficMap = this.m_Map
      }.Schedule<TrafficAmbienceSystem.TrafficAmbienceUpdateJob>(TrafficAmbienceSystem.kTextureSize * TrafficAmbienceSystem.kTextureSize, TrafficAmbienceSystem.kTextureSize, JobHandle.CombineDependencies(this.m_WriteDependencies, this.m_ReadDependencies, this.Dependency));
      this.AddWriter(this.Dependency);
      this.Dependency = JobHandle.CombineDependencies(this.m_ReadDependencies, this.m_WriteDependencies, this.Dependency);
    }

    [UnityEngine.Scripting.Preserve]
    public TrafficAmbienceSystem()
    {
    }

    [BurstCompile]
    private struct TrafficAmbienceUpdateJob : IJobParallelFor
    {
      public NativeArray<TrafficAmbienceCell> m_TrafficMap;

      public void Execute(int index)
      {
        TrafficAmbienceCell traffic = this.m_TrafficMap[index];
        this.m_TrafficMap[index] = new TrafficAmbienceCell()
        {
          m_Traffic = traffic.m_Accumulator,
          m_Accumulator = 0.0f
        };
      }
    }
  }
}
