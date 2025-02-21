// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AirPollutionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
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

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class AirPollutionSystem : CellMapSystem<AirPollution>, IJobSerializable
  {
    private static readonly int kSpread = 3;
    public static readonly int kTextureSize = 256;
    public static readonly int kUpdatesPerDay = 128;
    private WindSystem m_WindSystem;
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_PollutionParameterQuery;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / AirPollutionSystem.kUpdatesPerDay;
    }

    public int2 TextureSize
    {
      get => new int2(AirPollutionSystem.kTextureSize, AirPollutionSystem.kTextureSize);
    }

    public static float3 GetCellCenter(int index)
    {
      // ISSUE: reference to a compiler-generated field
      return CellMapSystem<AirPollution>.GetCellCenter(index, AirPollutionSystem.kTextureSize);
    }

    public static AirPollution GetPollution(float3 position, NativeArray<AirPollution> pollutionMap)
    {
      AirPollution pollution1 = new AirPollution();
      // ISSUE: reference to a compiler-generated field
      float num = (float) CellMapSystem<AirPollution>.kMapSize / (float) AirPollutionSystem.kTextureSize;
      // ISSUE: reference to a compiler-generated field
      int2 cell = CellMapSystem<AirPollution>.GetCell(position - new float3(num / 2f, 0.0f, num / 2f), CellMapSystem<AirPollution>.kMapSize, AirPollutionSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      float2 float2 = CellMapSystem<AirPollution>.GetCellCoords(position, CellMapSystem<AirPollution>.kMapSize, AirPollutionSystem.kTextureSize) - new float2(0.5f, 0.5f);
      // ISSUE: reference to a compiler-generated field
      int2 int2 = math.clamp(cell, (int2) 0, (int2) (AirPollutionSystem.kTextureSize - 2));
      // ISSUE: reference to a compiler-generated field
      short pollution2 = pollutionMap[int2.x + AirPollutionSystem.kTextureSize * int2.y].m_Pollution;
      // ISSUE: reference to a compiler-generated field
      short pollution3 = pollutionMap[int2.x + 1 + AirPollutionSystem.kTextureSize * int2.y].m_Pollution;
      // ISSUE: reference to a compiler-generated field
      short pollution4 = pollutionMap[int2.x + AirPollutionSystem.kTextureSize * (int2.y + 1)].m_Pollution;
      // ISSUE: reference to a compiler-generated field
      short pollution5 = pollutionMap[int2.x + 1 + AirPollutionSystem.kTextureSize * (int2.y + 1)].m_Pollution;
      pollution1.m_Pollution = (short) math.round(math.lerp(math.lerp((float) pollution2, (float) pollution3, float2.x - (float) int2.x), math.lerp((float) pollution4, (float) pollution5, float2.x - (float) int2.x), float2.y - (float) int2.y));
      return pollution1;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.CreateTextures(AirPollutionSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      this.m_WindSystem = this.World.GetOrCreateSystemManaged<WindSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PollutionParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<PollutionParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PollutionParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AirPollutionSystem.AirPollutionMoveJob jobData = new AirPollutionSystem.AirPollutionMoveJob()
      {
        m_PollutionMap = this.m_Map,
        m_WindMap = this.m_WindSystem.GetMap(true, out dependencies),
        m_PollutionParameters = this.m_PollutionParameterQuery.GetSingleton<PollutionParameterData>(),
        m_Random = RandomSeed.Next(),
        m_Frame = this.m_SimulationSystem.frameIndex
      };
      this.Dependency = jobData.Schedule<AirPollutionSystem.AirPollutionMoveJob>(JobUtils.CombineDependencies(dependencies, this.m_WriteDependencies, this.m_ReadDependencies, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      this.m_WindSystem.AddReader(this.Dependency);
      this.AddWriter(this.Dependency);
      this.Dependency = JobHandle.CombineDependencies(this.m_ReadDependencies, this.m_WriteDependencies, this.Dependency);
    }

    [UnityEngine.Scripting.Preserve]
    public AirPollutionSystem()
    {
    }

    [BurstCompile]
    private struct AirPollutionMoveJob : IJob
    {
      public NativeArray<AirPollution> m_PollutionMap;
      [ReadOnly]
      public NativeArray<Wind> m_WindMap;
      public PollutionParameterData m_PollutionParameters;
      public RandomSeed m_Random;
      public uint m_Frame;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<AirPollution> nativeArray = new NativeArray<AirPollution>(this.m_PollutionMap.Length, Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_Random.GetRandom((int) this.m_Frame);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_PollutionMap.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          float3 cellCenter = AirPollutionSystem.GetCellCenter(index);
          // ISSUE: reference to a compiler-generated field
          Wind wind = WindSystem.GetWind(cellCenter, this.m_WindMap);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          short pollution = AirPollutionSystem.GetPollution(cellCenter - this.m_PollutionParameters.m_WindAdvectionSpeed * new float3(wind.m_Wind.x, 0.0f, wind.m_Wind.y), this.m_PollutionMap).m_Pollution;
          nativeArray[index] = new AirPollution()
          {
            m_Pollution = pollution
          };
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num1 = (float) this.m_PollutionParameters.m_AirFade / (float) AirPollutionSystem.kUpdatesPerDay;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < AirPollutionSystem.kTextureSize; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < AirPollutionSystem.kTextureSize; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = index1 * AirPollutionSystem.kTextureSize + index2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int num2 = math.clamp((int) nativeArray[index3].m_Pollution + (index2 > 0 ? (int) nativeArray[index3 - 1].m_Pollution >> AirPollutionSystem.kSpread : 0) + (index2 < AirPollutionSystem.kTextureSize - 1 ? (int) nativeArray[index3 + 1].m_Pollution >> AirPollutionSystem.kSpread : 0) + (index1 > 0 ? (int) nativeArray[index3 - AirPollutionSystem.kTextureSize].m_Pollution >> AirPollutionSystem.kSpread : 0) + (index1 < AirPollutionSystem.kTextureSize - 1 ? (int) nativeArray[index3 + AirPollutionSystem.kTextureSize].m_Pollution >> AirPollutionSystem.kSpread : 0) - (((int) nativeArray[index3].m_Pollution >> AirPollutionSystem.kSpread - 2) + MathUtils.RoundToIntRandom(ref random, num1)), 0, (int) short.MaxValue);
            // ISSUE: reference to a compiler-generated field
            this.m_PollutionMap[index3] = new AirPollution()
            {
              m_Pollution = (short) num2
            };
          }
        }
        nativeArray.Dispose();
      }
    }
  }
}
