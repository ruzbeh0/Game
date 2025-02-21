// Decompiled with JetBrains decompiler
// Type: Game.Simulation.GroundWaterSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Serialization.Entities;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Assertions;
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
  public class GroundWaterSystem : CellMapSystem<GroundWater>, IJobSerializable
  {
    public const int kMaxGroundWater = 10000;
    public const int kMinGroundWaterThreshold = 500;
    public static readonly int kTextureSize = 256;
    private EntityQuery m_ParameterQuery;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 64;

    public static float3 GetCellCenter(int index)
    {
      // ISSUE: reference to a compiler-generated field
      return CellMapSystem<GroundWater>.GetCellCenter(index, GroundWaterSystem.kTextureSize);
    }

    public static bool TryGetCell(float3 position, out int2 cell)
    {
      // ISSUE: reference to a compiler-generated field
      cell = CellMapSystem<GroundWater>.GetCell(position, CellMapSystem<GroundWater>.kMapSize, GroundWaterSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated method
      return GroundWaterSystem.IsValidCell(cell);
    }

    public static bool IsValidCell(int2 cell)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return cell.x >= 0 && cell.y >= 0 && cell.x < GroundWaterSystem.kTextureSize && cell.y < GroundWaterSystem.kTextureSize;
    }

    public static GroundWater GetGroundWater(
      float3 position,
      NativeArray<GroundWater> groundWaterMap)
    {
      // ISSUE: reference to a compiler-generated field
      float2 float2 = CellMapSystem<GroundWater>.GetCellCoords(position, CellMapSystem<GroundWater>.kMapSize, GroundWaterSystem.kTextureSize) - new float2(0.5f, 0.5f);
      int2 cell1 = new int2(Mathf.FloorToInt(float2.x), Mathf.FloorToInt(float2.y));
      int2 cell2 = new int2(cell1.x + 1, cell1.y);
      int2 cell3 = new int2(cell1.x, cell1.y + 1);
      int2 cell4 = new int2(cell1.x + 1, cell1.y + 1);
      // ISSUE: reference to a compiler-generated method
      GroundWater groundWater1 = GroundWaterSystem.GetGroundWater(groundWaterMap, cell1);
      // ISSUE: reference to a compiler-generated method
      GroundWater groundWater2 = GroundWaterSystem.GetGroundWater(groundWaterMap, cell2);
      // ISSUE: reference to a compiler-generated method
      GroundWater groundWater3 = GroundWaterSystem.GetGroundWater(groundWaterMap, cell3);
      // ISSUE: reference to a compiler-generated method
      GroundWater groundWater4 = GroundWaterSystem.GetGroundWater(groundWaterMap, cell4);
      float sx = float2.x - (float) cell1.x;
      float sy = float2.y - (float) cell1.y;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return new GroundWater()
      {
        m_Amount = (short) math.round(GroundWaterSystem.Bilinear(groundWater1.m_Amount, groundWater2.m_Amount, groundWater3.m_Amount, groundWater4.m_Amount, sx, sy)),
        m_Polluted = (short) math.round(GroundWaterSystem.Bilinear(groundWater1.m_Polluted, groundWater2.m_Polluted, groundWater3.m_Polluted, groundWater4.m_Polluted, sx, sy)),
        m_Max = (short) math.round(GroundWaterSystem.Bilinear(groundWater1.m_Max, groundWater2.m_Max, groundWater3.m_Max, groundWater4.m_Max, sx, sy))
      };
    }

    public static void ConsumeGroundWater(
      float3 position,
      NativeArray<GroundWater> groundWaterMap,
      int amount)
    {
      Assert.IsTrue(amount >= 0);
      // ISSUE: reference to a compiler-generated field
      float2 float2 = CellMapSystem<GroundWater>.GetCellCoords(position, CellMapSystem<GroundWater>.kMapSize, GroundWaterSystem.kTextureSize) - new float2(0.5f, 0.5f);
      int2 cell1 = new int2(Mathf.FloorToInt(float2.x), Mathf.FloorToInt(float2.y));
      int2 cell2 = new int2(cell1.x + 1, cell1.y);
      int2 cell3 = new int2(cell1.x, cell1.y + 1);
      int2 cell4 = new int2(cell1.x + 1, cell1.y + 1);
      // ISSUE: reference to a compiler-generated method
      GroundWater groundWater1 = GroundWaterSystem.GetGroundWater(groundWaterMap, cell1);
      // ISSUE: reference to a compiler-generated method
      GroundWater groundWater2 = GroundWaterSystem.GetGroundWater(groundWaterMap, cell2);
      // ISSUE: reference to a compiler-generated method
      GroundWater groundWater3 = GroundWaterSystem.GetGroundWater(groundWaterMap, cell3);
      // ISSUE: reference to a compiler-generated method
      GroundWater groundWater4 = GroundWaterSystem.GetGroundWater(groundWaterMap, cell4);
      float sx = float2.x - (float) cell1.x;
      float sy = float2.y - (float) cell1.y;
      // ISSUE: reference to a compiler-generated method
      float cellAvailable1 = math.ceil(GroundWaterSystem.Bilinear(groundWater1.m_Amount, (short) 0, (short) 0, (short) 0, sx, sy));
      // ISSUE: reference to a compiler-generated method
      float cellAvailable2 = math.ceil(GroundWaterSystem.Bilinear((short) 0, groundWater2.m_Amount, (short) 0, (short) 0, sx, sy));
      // ISSUE: reference to a compiler-generated method
      float cellAvailable3 = math.ceil(GroundWaterSystem.Bilinear((short) 0, (short) 0, groundWater3.m_Amount, (short) 0, sx, sy));
      // ISSUE: reference to a compiler-generated method
      float cellAvailable4 = math.ceil(GroundWaterSystem.Bilinear((short) 0, (short) 0, (short) 0, groundWater4.m_Amount, sx, sy));
      float totalAvailable = cellAvailable1 + cellAvailable2 + cellAvailable3 + cellAvailable4;
      float totalConsumed = math.min((float) amount, totalAvailable);
      if ((double) totalAvailable < (double) amount)
        Debug.LogWarning((object) string.Format("Trying to consume more groundwater than available! amount: {0}, available: {1}", (object) amount, (object) totalAvailable));
      ConsumeFraction(ref groundWater1, cellAvailable1);
      ConsumeFraction(ref groundWater2, cellAvailable2);
      ConsumeFraction(ref groundWater3, cellAvailable3);
      ConsumeFraction(ref groundWater4, cellAvailable4);
      Assert.IsTrue(Mathf.Approximately(totalAvailable, 0.0f));
      Assert.IsTrue(Mathf.Approximately(totalConsumed, 0.0f));
      // ISSUE: reference to a compiler-generated method
      GroundWaterSystem.SetGroundWater(groundWaterMap, cell1, groundWater1);
      // ISSUE: reference to a compiler-generated method
      GroundWaterSystem.SetGroundWater(groundWaterMap, cell2, groundWater2);
      // ISSUE: reference to a compiler-generated method
      GroundWaterSystem.SetGroundWater(groundWaterMap, cell3, groundWater3);
      // ISSUE: reference to a compiler-generated method
      GroundWaterSystem.SetGroundWater(groundWaterMap, cell4, groundWater4);

      void ConsumeFraction(ref GroundWater gw, float cellAvailable)
      {
        if ((double) totalAvailable < 0.5)
          return;
        double num = (double) cellAvailable / (double) totalAvailable;
        totalAvailable -= cellAvailable;
        float y = math.max(0.0f, totalConsumed - totalAvailable);
        double totalConsumed = (double) totalConsumed;
        float amount = math.max(math.round((float) (num * totalConsumed)), y);
        Assert.IsTrue((double) amount <= (double) gw.m_Amount);
        gw.Consume((int) amount);
        totalConsumed -= amount;
      }
    }

    private static GroundWater GetGroundWater(NativeArray<GroundWater> groundWaterMap, int2 cell)
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      return !GroundWaterSystem.IsValidCell(cell) ? new GroundWater() : groundWaterMap[cell.x + GroundWaterSystem.kTextureSize * cell.y];
    }

    private static void SetGroundWater(
      NativeArray<GroundWater> groundWaterMap,
      int2 cell,
      GroundWater gw)
    {
      // ISSUE: reference to a compiler-generated method
      if (!GroundWaterSystem.IsValidCell(cell))
        return;
      // ISSUE: reference to a compiler-generated field
      groundWaterMap[cell.x + GroundWaterSystem.kTextureSize * cell.y] = gw;
    }

    private static float Bilinear(
      short v00,
      short v10,
      short v01,
      short v11,
      float sx,
      float sy)
    {
      return math.lerp(math.lerp((float) v00, (float) v10, sx), math.lerp((float) v01, (float) v11, sx), sy);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.CreateTextures(GroundWaterSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      this.m_ParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<WaterPipeParameterData>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GroundWaterSystem.GroundWaterTickJob jobData = new GroundWaterSystem.GroundWaterTickJob()
      {
        m_GroundWaterMap = this.m_Map,
        m_Parameters = this.m_ParameterQuery.GetSingleton<WaterPipeParameterData>()
      };
      this.Dependency = jobData.Schedule<GroundWaterSystem.GroundWaterTickJob>(JobHandle.CombineDependencies(this.m_WriteDependencies, this.m_ReadDependencies, this.Dependency));
      this.AddWriter(this.Dependency);
      this.Dependency = JobHandle.CombineDependencies(this.m_ReadDependencies, this.m_WriteDependencies, this.Dependency);
    }

    public override JobHandle SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      if (context.purpose != Colossal.Serialization.Entities.Purpose.NewGame || !(context.version < Version.timoSerializationFlow))
        return base.SetDefaults(context);
      for (int index = 0; index < this.m_Map.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        short num = (short) Mathf.RoundToInt(10000f * math.saturate((float) (((double) Mathf.PerlinNoise(32f * ((float) (index % GroundWaterSystem.kTextureSize) / (float) GroundWaterSystem.kTextureSize), 32f * ((float) (index / GroundWaterSystem.kTextureSize) / (float) GroundWaterSystem.kTextureSize)) - 0.60000002384185791) / 0.40000000596046448)));
        GroundWater groundWater = new GroundWater()
        {
          m_Amount = num,
          m_Max = num
        };
        this.m_Map[index] = groundWater;
      }
      return new JobHandle();
    }

    [UnityEngine.Scripting.Preserve]
    public GroundWaterSystem()
    {
    }

    [BurstCompile]
    private struct GroundWaterTickJob : IJob
    {
      public NativeArray<GroundWater> m_GroundWaterMap;
      public WaterPipeParameterData m_Parameters;

      private void HandlePollution(int index, int otherIndex, NativeArray<int2> tmp)
      {
        // ISSUE: reference to a compiler-generated field
        GroundWater groundWater1 = this.m_GroundWaterMap[index];
        // ISSUE: reference to a compiler-generated field
        GroundWater groundWater2 = this.m_GroundWaterMap[otherIndex];
        ref int2 local1 = ref tmp.ElementAt<int2>(index);
        ref int2 local2 = ref tmp.ElementAt<int2>(otherIndex);
        int num1 = (int) groundWater1.m_Polluted + (int) groundWater2.m_Polluted;
        int num2 = (int) groundWater1.m_Amount + (int) groundWater2.m_Amount;
        int num3 = math.clamp(((num2 > 0 ? (int) groundWater1.m_Amount * num1 / num2 : 0) - (int) groundWater1.m_Polluted) / 4, -((int) groundWater2.m_Amount - (int) groundWater2.m_Polluted) / 4, ((int) groundWater1.m_Amount - (int) groundWater1.m_Polluted) / 4);
        local1.y += num3;
        local2.y -= num3;
        Assert.IsTrue(0 <= (int) groundWater1.m_Polluted + local1.y);
        Assert.IsTrue((int) groundWater1.m_Polluted + local1.y <= (int) groundWater1.m_Amount);
        Assert.IsTrue(0 <= (int) groundWater2.m_Polluted + local2.y);
        Assert.IsTrue((int) groundWater2.m_Polluted + local2.y <= (int) groundWater2.m_Amount);
      }

      private void HandleFlow(int index, int otherIndex, NativeArray<int2> tmp)
      {
        // ISSUE: reference to a compiler-generated field
        GroundWater groundWater1 = this.m_GroundWaterMap[index];
        // ISSUE: reference to a compiler-generated field
        GroundWater groundWater2 = this.m_GroundWaterMap[otherIndex];
        ref int2 local1 = ref tmp.ElementAt<int2>(index);
        ref int2 local2 = ref tmp.ElementAt<int2>(otherIndex);
        Assert.IsTrue((int) groundWater2.m_Polluted + local2.y <= (int) groundWater2.m_Amount + local2.x);
        Assert.IsTrue((int) groundWater1.m_Polluted + local1.y <= (int) groundWater1.m_Amount + local1.x);
        float num1 = (int) groundWater1.m_Amount + local1.x != 0 ? 1f * (float) ((int) groundWater1.m_Polluted + local1.y) / (float) ((int) groundWater1.m_Amount + local1.x) : 0.0f;
        float num2 = (int) groundWater2.m_Amount + local2.x != 0 ? 1f * (float) ((int) groundWater2.m_Polluted + local2.y) / (float) ((int) groundWater2.m_Amount + local2.x) : 0.0f;
        int num3 = (int) groundWater1.m_Amount - (int) groundWater1.m_Max;
        int num4 = math.clamp(((int) groundWater2.m_Amount - (int) groundWater2.m_Max - num3) / 4, (int) -groundWater1.m_Amount / 4, (int) groundWater2.m_Amount / 4);
        local1.x += num4;
        local2.x -= num4;
        int num5 = 0;
        if (num4 > 0)
          num5 = (int) ((double) num4 * (double) num2);
        else if (num4 < 0)
          num5 = (int) ((double) num4 * (double) num1);
        local1.y += num5;
        local2.y -= num5;
        Assert.IsTrue(0 <= (int) groundWater1.m_Amount + local1.x);
        Assert.IsTrue((int) groundWater1.m_Amount + local1.x <= (int) groundWater1.m_Max);
        Assert.IsTrue(0 <= (int) groundWater2.m_Amount + local2.x);
        Assert.IsTrue((int) groundWater2.m_Amount + local2.x <= (int) groundWater2.m_Max);
        Assert.IsTrue(0 <= (int) groundWater1.m_Polluted + local1.y);
        Assert.IsTrue((int) groundWater1.m_Polluted + local1.y <= (int) groundWater1.m_Amount + local1.x);
        Assert.IsTrue(0 <= (int) groundWater2.m_Polluted + local2.y);
        Assert.IsTrue((int) groundWater2.m_Polluted + local2.y <= (int) groundWater2.m_Amount + local2.x);
      }

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<int2> tmp = new NativeArray<int2>(this.m_GroundWaterMap.Length, Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_GroundWaterMap.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          int num1 = index % GroundWaterSystem.kTextureSize;
          // ISSUE: reference to a compiler-generated field
          int num2 = index / GroundWaterSystem.kTextureSize;
          // ISSUE: reference to a compiler-generated field
          int num3 = GroundWaterSystem.kTextureSize - 1;
          if (num1 < num3)
          {
            // ISSUE: reference to a compiler-generated method
            this.HandlePollution(index, index + 1, tmp);
          }
          // ISSUE: reference to a compiler-generated field
          if (num2 < GroundWaterSystem.kTextureSize - 1)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.HandlePollution(index, index + GroundWaterSystem.kTextureSize, tmp);
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_GroundWaterMap.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          int num4 = index % GroundWaterSystem.kTextureSize;
          // ISSUE: reference to a compiler-generated field
          int num5 = index / GroundWaterSystem.kTextureSize;
          // ISSUE: reference to a compiler-generated field
          int num6 = GroundWaterSystem.kTextureSize - 1;
          if (num4 < num6)
          {
            // ISSUE: reference to a compiler-generated method
            this.HandleFlow(index, index + 1, tmp);
          }
          // ISSUE: reference to a compiler-generated field
          if (num5 < GroundWaterSystem.kTextureSize - 1)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.HandleFlow(index, index + GroundWaterSystem.kTextureSize, tmp);
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_GroundWaterMap.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          GroundWater groundWater = this.m_GroundWaterMap[index];
          // ISSUE: reference to a compiler-generated field
          groundWater.m_Amount = (short) math.min((int) groundWater.m_Amount + tmp[index].x + Mathf.CeilToInt(this.m_Parameters.m_GroundwaterReplenish * (float) groundWater.m_Max), (int) groundWater.m_Max);
          // ISSUE: reference to a compiler-generated field
          groundWater.m_Polluted = (short) math.clamp((int) groundWater.m_Polluted + tmp[index].y - this.m_Parameters.m_GroundwaterPurification, 0, (int) groundWater.m_Amount);
          // ISSUE: reference to a compiler-generated field
          this.m_GroundWaterMap[index] = groundWater;
        }
        tmp.Dispose();
      }
    }
  }
}
