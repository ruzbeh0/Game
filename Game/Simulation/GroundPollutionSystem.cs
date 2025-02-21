// Decompiled with JetBrains decompiler
// Type: Game.Simulation.GroundPollutionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Common;
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
  public class GroundPollutionSystem : CellMapSystem<GroundPollution>, IJobSerializable
  {
    public static readonly int kTextureSize = 256;
    public static readonly int kUpdatesPerDay = 128;
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_PollutionParameterGroup;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / GroundPollutionSystem.kUpdatesPerDay;
    }

    public int2 TextureSize
    {
      get => new int2(GroundPollutionSystem.kTextureSize, GroundPollutionSystem.kTextureSize);
    }

    public static float3 GetCellCenter(int index)
    {
      // ISSUE: reference to a compiler-generated field
      return CellMapSystem<GroundPollution>.GetCellCenter(index, GroundPollutionSystem.kTextureSize);
    }

    public static GroundPollution GetPollution(
      float3 position,
      NativeArray<GroundPollution> pollutionMap)
    {
      GroundPollution pollution1 = new GroundPollution();
      // ISSUE: reference to a compiler-generated field
      int2 cell = CellMapSystem<GroundPollution>.GetCell(position, CellMapSystem<GroundPollution>.kMapSize, GroundPollutionSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      float2 cellCoords = CellMapSystem<GroundPollution>.GetCellCoords(position, CellMapSystem<GroundPollution>.kMapSize, GroundPollutionSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (cell.x < 0 || cell.x >= GroundPollutionSystem.kTextureSize || cell.y < 0 || cell.y >= GroundPollutionSystem.kTextureSize)
        return pollution1;
      // ISSUE: reference to a compiler-generated field
      GroundPollution pollution2 = pollutionMap[cell.x + GroundPollutionSystem.kTextureSize * cell.y];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      GroundPollution groundPollution1 = cell.x < GroundPollutionSystem.kTextureSize - 1 ? pollutionMap[cell.x + 1 + GroundPollutionSystem.kTextureSize * cell.y] : new GroundPollution();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      GroundPollution groundPollution2 = cell.y < GroundPollutionSystem.kTextureSize - 1 ? pollutionMap[cell.x + GroundPollutionSystem.kTextureSize * (cell.y + 1)] : new GroundPollution();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      GroundPollution groundPollution3 = cell.x >= GroundPollutionSystem.kTextureSize - 1 || cell.y >= GroundPollutionSystem.kTextureSize - 1 ? new GroundPollution() : pollutionMap[cell.x + 1 + GroundPollutionSystem.kTextureSize * (cell.y + 1)];
      pollution1.m_Pollution = (short) Mathf.RoundToInt(math.lerp(math.lerp((float) pollution2.m_Pollution, (float) groundPollution1.m_Pollution, cellCoords.x - (float) cell.x), math.lerp((float) groundPollution2.m_Pollution, (float) groundPollution3.m_Pollution, cellCoords.x - (float) cell.x), cellCoords.y - (float) cell.y));
      return pollution1;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.CreateTextures(GroundPollutionSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      this.m_PollutionParameterGroup = this.GetEntityQuery(ComponentType.ReadOnly<PollutionParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PollutionParameterGroup);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GroundPollutionSystem.PollutionFadeJob jobData = new GroundPollutionSystem.PollutionFadeJob()
      {
        m_PollutionMap = this.m_Map,
        m_PollutionParameters = this.m_PollutionParameterGroup.GetSingleton<PollutionParameterData>(),
        m_Random = RandomSeed.Next(),
        m_Frame = this.m_SimulationSystem.frameIndex
      };
      this.Dependency = jobData.Schedule<GroundPollutionSystem.PollutionFadeJob>(JobHandle.CombineDependencies(this.m_WriteDependencies, this.m_ReadDependencies, this.Dependency));
      this.AddWriter(this.Dependency);
      this.Dependency = JobHandle.CombineDependencies(this.m_ReadDependencies, this.m_WriteDependencies, this.Dependency);
    }

    [UnityEngine.Scripting.Preserve]
    public GroundPollutionSystem()
    {
    }

    [BurstCompile]
    private struct PollutionFadeJob : IJob
    {
      public NativeArray<GroundPollution> m_PollutionMap;
      public PollutionParameterData m_PollutionParameters;
      public RandomSeed m_Random;
      public uint m_Frame;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_Random.GetRandom((int) this.m_Frame);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_PollutionMap.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          GroundPollution pollution = this.m_PollutionMap[index];
          if (pollution.m_Pollution > (short) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            pollution.m_Pollution = (short) math.max(0, (int) this.m_PollutionMap[index].m_Pollution - MathUtils.RoundToIntRandom(ref random, (float) this.m_PollutionParameters.m_GroundFade / (float) GroundPollutionSystem.kUpdatesPerDay));
          }
          // ISSUE: reference to a compiler-generated field
          this.m_PollutionMap[index] = pollution;
        }
      }
    }
  }
}
