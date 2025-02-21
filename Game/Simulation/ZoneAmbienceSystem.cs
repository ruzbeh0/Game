// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ZoneAmbienceSystem
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
  public class ZoneAmbienceSystem : CellMapSystem<ZoneAmbienceCell>, IJobSerializable
  {
    public static readonly int kTextureSize = 64;
    public static readonly int kUpdatesPerDay = 128;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      return 262144 / ZoneAmbienceSystem.kUpdatesPerDay;
    }

    public int2 TextureSize
    {
      get => new int2(ZoneAmbienceSystem.kTextureSize, ZoneAmbienceSystem.kTextureSize);
    }

    public static float3 GetCellCenter(int index)
    {
      return CellMapSystem<ZoneAmbienceCell>.GetCellCenter(index, ZoneAmbienceSystem.kTextureSize);
    }

    public static float GetZoneAmbienceNear(
      GroupAmbienceType type,
      float3 position,
      NativeArray<ZoneAmbienceCell> zoneAmbienceMap,
      float nearWeight,
      float maxPerCell)
    {
      int2 cell = CellMapSystem<ZoneAmbienceCell>.GetCell(position, CellMapSystem<ZoneAmbienceCell>.kMapSize, ZoneAmbienceSystem.kTextureSize);
      float num1 = 0.0f;
      float num2 = 0.0f;
      for (int index1 = cell.x - 2; index1 <= cell.x + 2; ++index1)
      {
        for (int index2 = cell.y - 2; index2 <= cell.y + 2; ++index2)
        {
          if (index1 >= 0 && index1 < ZoneAmbienceSystem.kTextureSize && index2 >= 0 && index2 < ZoneAmbienceSystem.kTextureSize)
          {
            int index3 = index1 + ZoneAmbienceSystem.kTextureSize * index2;
            float num3 = math.max(1f, math.pow(math.distance(ZoneAmbienceSystem.GetCellCenter(index3), position) / 10f, 1f + nearWeight));
            num1 += math.min(maxPerCell, zoneAmbienceMap[index3].m_Value.GetAmbience(type)) / num3;
            num2 += 1f / num3;
          }
        }
      }
      return num1 / num2;
    }

    public static float GetZoneAmbience(
      GroupAmbienceType type,
      float3 position,
      NativeArray<ZoneAmbienceCell> zoneAmbienceMap,
      float maxPerCell)
    {
      int2 cell = CellMapSystem<ZoneAmbienceCell>.GetCell(position, CellMapSystem<ZoneAmbienceCell>.kMapSize, ZoneAmbienceSystem.kTextureSize);
      float num1 = 0.0f;
      float num2 = 0.0f;
      for (int index1 = cell.x - 2; index1 <= cell.x + 2; ++index1)
      {
        for (int index2 = cell.y - 2; index2 <= cell.y + 2; ++index2)
        {
          if (index1 >= 0 && index1 < ZoneAmbienceSystem.kTextureSize && index2 >= 0 && index2 < ZoneAmbienceSystem.kTextureSize)
          {
            int index3 = index1 + ZoneAmbienceSystem.kTextureSize * index2;
            float num3 = math.max(1f, math.distancesq(ZoneAmbienceSystem.GetCellCenter(index3), position) / 10f);
            num1 += math.min(maxPerCell, zoneAmbienceMap[index3].m_Value.GetAmbience(type)) / num3;
            num2 += 1f / num3;
          }
        }
      }
      return num1 / num2;
    }

    public static ZoneAmbienceCell GetZoneAmbience(
      float3 position,
      NativeArray<ZoneAmbienceCell> zoneAmbienceMap)
    {
      ZoneAmbienceCell zoneAmbience = new ZoneAmbienceCell();
      int2 cell = CellMapSystem<ZoneAmbienceCell>.GetCell(position, CellMapSystem<ZoneAmbienceCell>.kMapSize, ZoneAmbienceSystem.kTextureSize);
      ZoneAmbiences zoneAmbiences = new ZoneAmbiences();
      float num1 = 0.0f;
      for (int index1 = cell.x - 2; index1 <= cell.x + 2; ++index1)
      {
        for (int index2 = cell.y - 2; index2 <= cell.y + 2; ++index2)
        {
          if (index1 >= 0 && index1 < ZoneAmbienceSystem.kTextureSize && index2 >= 0 && index2 < ZoneAmbienceSystem.kTextureSize)
          {
            int index3 = index1 + ZoneAmbienceSystem.kTextureSize * index2;
            float num2 = math.max(1f, math.distancesq(ZoneAmbienceSystem.GetCellCenter(index3), position) / 10f);
            zoneAmbiences += zoneAmbienceMap[index3].m_Value / num2;
            num1 += 1f / num2;
          }
        }
      }
      zoneAmbience.m_Value = zoneAmbiences / num1;
      return zoneAmbience;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.CreateTextures(ZoneAmbienceSystem.kTextureSize);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      this.Dependency = new ZoneAmbienceSystem.ZoneAmbienceUpdateJob()
      {
        m_ZoneMap = this.m_Map
      }.Schedule<ZoneAmbienceSystem.ZoneAmbienceUpdateJob>(ZoneAmbienceSystem.kTextureSize * ZoneAmbienceSystem.kTextureSize, ZoneAmbienceSystem.kTextureSize, JobHandle.CombineDependencies(this.m_WriteDependencies, this.m_ReadDependencies, this.Dependency));
      this.AddWriter(this.Dependency);
      this.Dependency = JobHandle.CombineDependencies(this.m_ReadDependencies, this.m_WriteDependencies, this.Dependency);
    }

    [UnityEngine.Scripting.Preserve]
    public ZoneAmbienceSystem()
    {
    }

    [BurstCompile]
    private struct ZoneAmbienceUpdateJob : IJobParallelFor
    {
      public NativeArray<ZoneAmbienceCell> m_ZoneMap;

      public void Execute(int index)
      {
        ZoneAmbienceCell zone = this.m_ZoneMap[index];
        this.m_ZoneMap[index] = new ZoneAmbienceCell()
        {
          m_Value = zone.m_Accumulator,
          m_Accumulator = new ZoneAmbiences()
        };
      }
    }
  }
}
