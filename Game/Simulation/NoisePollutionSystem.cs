// Decompiled with JetBrains decompiler
// Type: Game.Simulation.NoisePollutionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  public class NoisePollutionSystem : CellMapSystem<NoisePollution>, IJobSerializable
  {
    public static readonly int kTextureSize = 256;
    public static readonly int kUpdatesPerDay = 128;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      return 262144 / NoisePollutionSystem.kUpdatesPerDay;
    }

    public int2 TextureSize
    {
      get => new int2(NoisePollutionSystem.kTextureSize, NoisePollutionSystem.kTextureSize);
    }

    public static float3 GetCellCenter(int index)
    {
      return CellMapSystem<NoisePollution>.GetCellCenter(index, NoisePollutionSystem.kTextureSize);
    }

    public static NoisePollution GetPollution(
      float3 position,
      NativeArray<NoisePollution> pollutionMap)
    {
      NoisePollution pollution1 = new NoisePollution();
      float num = (float) CellMapSystem<NoisePollution>.kMapSize / (float) NoisePollutionSystem.kTextureSize;
      int2 cell = CellMapSystem<NoisePollution>.GetCell(position - new float3(num / 2f, 0.0f, num / 2f), CellMapSystem<NoisePollution>.kMapSize, NoisePollutionSystem.kTextureSize);
      float2 float2 = CellMapSystem<NoisePollution>.GetCellCoords(position, CellMapSystem<NoisePollution>.kMapSize, NoisePollutionSystem.kTextureSize) - new float2(0.5f, 0.5f);
      int2 int2 = math.clamp(cell, (int2) 0, (int2) (NoisePollutionSystem.kTextureSize - 2));
      short pollution2 = pollutionMap[int2.x + NoisePollutionSystem.kTextureSize * int2.y].m_Pollution;
      short pollution3 = pollutionMap[int2.x + 1 + NoisePollutionSystem.kTextureSize * int2.y].m_Pollution;
      short pollution4 = pollutionMap[int2.x + NoisePollutionSystem.kTextureSize * (int2.y + 1)].m_Pollution;
      short pollution5 = pollutionMap[int2.x + 1 + NoisePollutionSystem.kTextureSize * (int2.y + 1)].m_Pollution;
      pollution1.m_Pollution = (short) Mathf.RoundToInt(math.lerp(math.lerp((float) pollution2, (float) pollution3, float2.x - (float) int2.x), math.lerp((float) pollution4, (float) pollution5, float2.x - (float) int2.x), float2.y - (float) int2.y));
      return pollution1;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.CreateTextures(NoisePollutionSystem.kTextureSize);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle dependencies;
      NoisePollutionSystem.NoisePollutionSwapJob jobData1 = new NoisePollutionSystem.NoisePollutionSwapJob()
      {
        m_PollutionMap = this.GetMap(false, out dependencies)
      };
      NoisePollutionSystem.NoisePollutionClearJob jobData2 = new NoisePollutionSystem.NoisePollutionClearJob()
      {
        m_PollutionMap = jobData1.m_PollutionMap
      };
      JobHandle jobHandle = jobData1.Schedule<NoisePollutionSystem.NoisePollutionSwapJob>(this.m_Map.Length, 4, dependencies);
      int length = this.m_Map.Length;
      JobHandle dependsOn = jobHandle;
      this.AddWriter(jobData2.Schedule<NoisePollutionSystem.NoisePollutionClearJob>(length, 64, dependsOn));
    }

    [UnityEngine.Scripting.Preserve]
    public NoisePollutionSystem()
    {
    }

    [BurstCompile]
    private struct NoisePollutionSwapJob : IJobParallelFor
    {
      [NativeDisableParallelForRestriction]
      public NativeArray<NoisePollution> m_PollutionMap;

      public void Execute(int index)
      {
        NoisePollution pollution = this.m_PollutionMap[index];
        int num1 = index % NoisePollutionSystem.kTextureSize;
        int num2 = index / NoisePollutionSystem.kTextureSize;
        short pollutionTemp1 = num1 > 0 ? this.m_PollutionMap[index - 1].m_PollutionTemp : (short) 0;
        short pollutionTemp2 = num1 < NoisePollutionSystem.kTextureSize - 1 ? this.m_PollutionMap[index + 1].m_PollutionTemp : (short) 0;
        short pollutionTemp3 = num2 > 0 ? this.m_PollutionMap[index - NoisePollutionSystem.kTextureSize].m_PollutionTemp : (short) 0;
        short pollutionTemp4 = num2 < NoisePollutionSystem.kTextureSize - 1 ? this.m_PollutionMap[index + NoisePollutionSystem.kTextureSize].m_PollutionTemp : (short) 0;
        short pollutionTemp5 = num1 <= 0 || num2 <= 0 ? (short) 0 : this.m_PollutionMap[index - 1 - NoisePollutionSystem.kTextureSize].m_PollutionTemp;
        short pollutionTemp6 = num1 >= NoisePollutionSystem.kTextureSize - 1 || num2 <= 0 ? (short) 0 : this.m_PollutionMap[index + 1 - NoisePollutionSystem.kTextureSize].m_PollutionTemp;
        short pollutionTemp7 = num1 <= 0 || num2 >= NoisePollutionSystem.kTextureSize - 1 ? (short) 0 : this.m_PollutionMap[index - 1 + NoisePollutionSystem.kTextureSize].m_PollutionTemp;
        short pollutionTemp8 = num1 >= NoisePollutionSystem.kTextureSize - 1 || num2 >= NoisePollutionSystem.kTextureSize - 1 ? (short) 0 : this.m_PollutionMap[index + 1 + NoisePollutionSystem.kTextureSize].m_PollutionTemp;
        pollution.m_Pollution = (short) ((int) pollution.m_PollutionTemp / 4 + ((int) pollutionTemp1 + (int) pollutionTemp2 + (int) pollutionTemp3 + (int) pollutionTemp4) / 8 + ((int) pollutionTemp5 + (int) pollutionTemp6 + (int) pollutionTemp7 + (int) pollutionTemp8) / 16);
        this.m_PollutionMap[index] = pollution;
      }
    }

    [BurstCompile]
    private struct NoisePollutionClearJob : IJobParallelFor
    {
      public NativeArray<NoisePollution> m_PollutionMap;

      public void Execute(int index)
      {
        NoisePollution pollution = this.m_PollutionMap[index] with
        {
          m_PollutionTemp = 0
        };
        this.m_PollutionMap[index] = pollution;
      }
    }
  }
}
