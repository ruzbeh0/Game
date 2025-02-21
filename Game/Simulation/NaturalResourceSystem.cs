// Decompiled with JetBrains decompiler
// Type: Game.Simulation.NaturalResourceSystem
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
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class NaturalResourceSystem : CellMapSystem<NaturalResourceCell>, IJobSerializable
  {
    public const int MAX_BASE_RESOURCES = 10000;
    public const int FERTILITY_REGENERATION_RATE = 800;
    public const int UPDATES_PER_DAY = 8;
    public static readonly int kTextureSize = 256;
    public GroundPollutionSystem m_GroundPollutionSystem;
    private EntityQuery m_PollutionParameterQuery;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 32768;

    public int2 TextureSize
    {
      get => new int2(NaturalResourceSystem.kTextureSize, NaturalResourceSystem.kTextureSize);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionSystem = this.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PollutionParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<PollutionParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.CreateTextures(NaturalResourceSystem.kTextureSize);
    }

    public override JobHandle SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      JobHandle jobHandle = base.SetDefaults(context);
      if (context.purpose == Colossal.Serialization.Entities.Purpose.NewGame)
      {
        jobHandle.Complete();
        for (int index = 0; index < this.m_Map.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num1 = (float) (index % NaturalResourceSystem.kTextureSize) / (float) NaturalResourceSystem.kTextureSize;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          double num2 = (double) (index / NaturalResourceSystem.kTextureSize) / (double) NaturalResourceSystem.kTextureSize;
          float3 float3_1 = new float3(6.1f, 13.9f, 10.7f);
          float3 float3_2 = num1 * float3_1;
          float3 float3_3 = float3_1;
          float3 float3_4 = (float) num2 * float3_3;
          float3 float3_5;
          float3_5.x = Mathf.PerlinNoise(float3_2.x, float3_4.x);
          float3_5.y = Mathf.PerlinNoise(float3_2.y, float3_4.y);
          float3_5.z = Mathf.PerlinNoise(float3_2.z, float3_4.z);
          float3_5 = 10000f * math.saturate((float3_5 - new float3(0.4f, 0.7f, 0.7f)) * new float3(5f, 10f, 10f));
          this.m_Map[index] = new NaturalResourceCell()
          {
            m_Fertility = {
              m_Base = (ushort) float3_5.x
            },
            m_Ore = {
              m_Base = (ushort) float3_5.y
            },
            m_Oil = {
              m_Base = (ushort) float3_5.z
            }
          };
        }
      }
      return jobHandle;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Assert.AreEqual(GroundPollutionSystem.kTextureSize, NaturalResourceSystem.kTextureSize, "Ground pollution and Natural resources need to have the same resolution");
      JobHandle dependencies1;
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new NaturalResourceSystem.RegenerateFertilityJob()
      {
        m_RegenerationRate = 100,
        m_PollutionRate = (this.m_PollutionParameterQuery.GetSingleton<PollutionParameterData>().m_FertilityGroundMultiplier / 8f),
        m_CellData = this.GetData(false, out dependencies1),
        m_PollutionData = this.m_GroundPollutionSystem.GetData(true, out dependencies2),
        m_RandomSeed = RandomSeed.Next()
      }.Schedule<NaturalResourceSystem.RegenerateFertilityJob>(NaturalResourceSystem.kTextureSize * NaturalResourceSystem.kTextureSize, NaturalResourceSystem.kTextureSize, JobHandle.CombineDependencies(dependencies2, dependencies1));
      this.AddWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionSystem.AddReader(jobHandle);
    }

    public float ResourceAmountToArea(float amount)
    {
      float2 float2 = (float2) CellMapSystem<NaturalResourceCell>.kMapSize / (float2) this.TextureSize;
      return (float) ((double) amount * (double) float2.x * (double) float2.y / 10000.0);
    }

    [UnityEngine.Scripting.Preserve]
    public NaturalResourceSystem()
    {
    }

    [BurstCompile]
    private struct RegenerateFertilityJob : IJobParallelFor
    {
      [ReadOnly]
      public int m_RegenerationRate;
      [ReadOnly]
      public float m_PollutionRate;
      public CellMapData<NaturalResourceCell> m_CellData;
      [ReadOnly]
      public CellMapData<GroundPollution> m_PollutionData;
      public RandomSeed m_RandomSeed;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        NaturalResourceCell naturalResourceCell = this.m_CellData.m_Buffer[index];
        // ISSUE: reference to a compiler-generated field
        GroundPollution groundPollution = this.m_PollutionData.m_Buffer[index];
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(1 + index);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        naturalResourceCell.m_Fertility.m_Used = (ushort) math.min((int) naturalResourceCell.m_Fertility.m_Base, math.max(0, (int) naturalResourceCell.m_Fertility.m_Used - this.m_RegenerationRate + MathUtils.RoundToIntRandom(ref random, (float) groundPollution.m_Pollution * this.m_PollutionRate)));
        // ISSUE: reference to a compiler-generated field
        this.m_CellData.m_Buffer[index] = naturalResourceCell;
      }
    }
  }
}
