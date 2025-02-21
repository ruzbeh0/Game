// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WindSimulationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System.IO;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  public class WindSimulationSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    public static readonly int kUpdateInterval = 512;
    public static readonly int3 kResolution = new int3(WindSystem.kTextureSize, WindSystem.kTextureSize, 16);
    public static readonly float kChangeFactor = 0.02f;
    public static readonly float kTerrainSlowdown = 0.99f;
    public static readonly float kAirSlowdown = 0.995f;
    public static readonly float kVerticalSlowdown = 0.9f;
    private SimulationSystem m_SimulationSystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private ClimateSystem m_ClimateSystem;
    private bool m_Odd;
    private JobHandle m_Deps;
    private NativeArray<WindSimulationSystem.WindCell> m_Cells;

    public float2 constantWind { get; set; }

    private float m_ConstantPressure { get; set; }

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      return phase != SystemUpdatePhase.GameSimulation ? 1 : WindSimulationSystem.kUpdateInterval;
    }

    public unsafe byte[] CreateByteArray<T>(NativeArray<T> src) where T : struct
    {
      int size = UnsafeUtility.SizeOf<T>() * src.Length;
      byte* unsafeReadOnlyPtr = (byte*) src.GetUnsafeReadOnlyPtr<T>();
      byte[] byteArray;
      fixed (byte* destination = byteArray = new byte[size])
        UnsafeUtility.MemCpy((void*) destination, (void*) unsafeReadOnlyPtr, (long) size);
      return byteArray;
    }

    public void DebugSave()
    {
      this.m_Deps.Complete();
      using (System.IO.BinaryWriter binaryWriter = new System.IO.BinaryWriter((Stream) File.OpenWrite(Application.streamingAssetsPath + "/wind_temp.dat")))
      {
        binaryWriter.Write(WindSimulationSystem.kResolution.x);
        binaryWriter.Write(WindSimulationSystem.kResolution.y);
        binaryWriter.Write(WindSimulationSystem.kResolution.z);
        binaryWriter.Write(this.CreateByteArray<WindSimulationSystem.WindCell>(this.m_Cells));
      }
    }

    public unsafe void DebugLoad()
    {
      this.m_Deps.Complete();
      using (System.IO.BinaryReader binaryReader = new System.IO.BinaryReader((Stream) File.OpenRead(Application.streamingAssetsPath + "/wind_temp.dat")))
      {
        int num1 = binaryReader.ReadInt32();
        int num2 = binaryReader.ReadInt32();
        int num3 = binaryReader.ReadInt32();
        int length = num1 * num2 * num3 * UnsafeUtility.SizeOf<WindSimulationSystem.WindCell>();
        byte[] buffer = new byte[length];
        binaryReader.Read(buffer, 0, num1 * num2 * num3 * sizeof (WindSimulationSystem.WindCell));
        byte* unsafePtr = (byte*) this.m_Cells.GetUnsafePtr<WindSimulationSystem.WindCell>();
        for (int index = 0; index < length; ++index)
          unsafePtr[index] = buffer[index];
      }
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Cells.Length);
      writer.Write<WindSimulationSystem.WindCell>(this.m_Cells);
      writer.Write(this.constantWind);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (!(reader.context.version > Version.stormWater))
        return;
      if (reader.context.version > Version.cellMapLengths)
      {
        int num;
        reader.Read(out num);
        if (this.m_Cells.Length == num)
          reader.Read<WindSimulationSystem.WindCell>(this.m_Cells);
        if (reader.context.version > Version.windDirection)
        {
          float2 float2;
          reader.Read(out float2);
          this.constantWind = float2;
        }
        else
          this.constantWind = new float2(0.275f, 0.275f);
      }
      else
        reader.Read<WindSimulationSystem.WindCell>(this.m_Cells);
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      this.m_Deps.Complete();
      for (int index = 0; index < this.m_Cells.Length; ++index)
        this.m_Cells[index] = new WindSimulationSystem.WindCell()
        {
          m_Pressure = this.m_ConstantPressure,
          m_Velocities = new float3(this.constantWind, 0.0f)
        };
    }

    public void SetWind(float2 direction, float pressure)
    {
      this.m_Deps.Complete();
      this.constantWind = direction;
      this.m_ConstantPressure = pressure;
      this.SetDefaults(new Colossal.Serialization.Entities.Context());
    }

    public static float3 GetCenterVelocity(
      int3 cell,
      NativeArray<WindSimulationSystem.WindCell> cells)
    {
      float3 velocities = WindSimulationSystem.GetCell(cell, cells).m_Velocities;
      float3 float3_1 = cell.x > 0 ? WindSimulationSystem.GetCell(cell + new int3(-1, 0, 0), cells).m_Velocities : velocities;
      float3 float3_2 = cell.y > 0 ? WindSimulationSystem.GetCell(cell + new int3(0, -1, 0), cells).m_Velocities : velocities;
      float3 float3_3 = cell.z > 0 ? WindSimulationSystem.GetCell(cell + new int3(0, 0, -1), cells).m_Velocities : velocities;
      return 0.5f * new float3(velocities.x + float3_1.x, velocities.y + float3_2.y, velocities.z + float3_3.z);
    }

    public static float3 GetCellCenter(int index)
    {
      int3 int3 = new int3(index % WindSimulationSystem.kResolution.x, index / WindSimulationSystem.kResolution.x % WindSimulationSystem.kResolution.y, index / (WindSimulationSystem.kResolution.x * WindSimulationSystem.kResolution.y));
      return ((float) CellMapSystem<Wind>.kMapSize * new float3(((float) int3.x + 0.5f) / (float) WindSimulationSystem.kResolution.x, 0.0f, ((float) int3.y + 0.5f) / (float) WindSimulationSystem.kResolution.y) - (float) (CellMapSystem<Wind>.kMapSize / 2)) with
      {
        y = (float) (100.0 + 1024.0 * ((double) int3.z + 0.5) / (double) WindSimulationSystem.kResolution.z)
      };
    }

    public NativeArray<WindSimulationSystem.WindCell> GetCells(out JobHandle deps)
    {
      deps = this.m_Deps;
      return this.m_Cells;
    }

    public void AddReader(JobHandle reader)
    {
      this.m_Deps = JobHandle.CombineDependencies(this.m_Deps, reader);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      this.constantWind = new float2(0.275f, 0.275f);
      this.m_ConstantPressure = 40f;
      this.m_Cells = new NativeArray<WindSimulationSystem.WindCell>(WindSimulationSystem.kResolution.x * WindSimulationSystem.kResolution.y * WindSimulationSystem.kResolution.z, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy() => this.m_Cells.Dispose();

    private WindSimulationSystem.WindCell GetCell(int3 position)
    {
      return this.m_Cells[position.x + position.y * WindSimulationSystem.kResolution.x + position.z * WindSimulationSystem.kResolution.x * WindSimulationSystem.kResolution.y];
    }

    public static WindSimulationSystem.WindCell GetCell(
      int3 position,
      NativeArray<WindSimulationSystem.WindCell> cells)
    {
      int index = position.x + position.y * WindSimulationSystem.kResolution.x + position.z * WindSimulationSystem.kResolution.x * WindSimulationSystem.kResolution.y;
      return index < 0 || index >= cells.Length ? new WindSimulationSystem.WindCell() : cells[index];
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      if (!((Object) this.m_TerrainSystem.heightmap != (Object) null))
        return;
      this.m_Odd = !this.m_Odd;
      if (!this.m_Odd)
      {
        // ISSUE: reference to a compiler-generated method
        TerrainHeightData heightData = this.m_TerrainSystem.GetHeightData();
        float2 float2 = new float2(TerrainUtils.ToWorldSpace(ref heightData, 0.0f), TerrainUtils.ToWorldSpace(ref heightData, (float) ushort.MaxValue));
        JobHandle deps;
        // ISSUE: reference to a compiler-generated method
        this.m_Deps = new WindSimulationSystem.UpdateWindVelocityJob()
        {
          m_Cells = this.m_Cells,
          m_TerrainHeightData = heightData,
          m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
          m_TerrainRange = float2
        }.Schedule<WindSimulationSystem.UpdateWindVelocityJob>(WindSimulationSystem.kResolution.x * WindSimulationSystem.kResolution.y * WindSimulationSystem.kResolution.z, JobHandle.CombineDependencies(this.m_Deps, deps, this.Dependency));
        // ISSUE: reference to a compiler-generated method
        this.m_WaterSystem.AddSurfaceReader(this.m_Deps);
        // ISSUE: reference to a compiler-generated method
        this.m_TerrainSystem.AddCPUHeightReader(this.m_Deps);
      }
      else
        this.m_Deps = new WindSimulationSystem.UpdatePressureJob()
        {
          m_Cells = this.m_Cells,
          m_Wind = (this.constantWind / 10f)
        }.Schedule<WindSimulationSystem.UpdatePressureJob>(WindSimulationSystem.kResolution.x * WindSimulationSystem.kResolution.y * WindSimulationSystem.kResolution.z, JobHandle.CombineDependencies(this.m_Deps, this.Dependency));
      this.Dependency = this.m_Deps;
    }

    [UnityEngine.Scripting.Preserve]
    public WindSimulationSystem()
    {
    }

    public struct WindCell : ISerializable
    {
      public float m_Pressure;
      public float3 m_Velocities;

      public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
      {
        writer.Write(this.m_Pressure);
        writer.Write(this.m_Velocities);
      }

      public void Deserialize<TReader>(TReader reader) where TReader : IReader
      {
        reader.Read(out this.m_Pressure);
        reader.Read(out this.m_Velocities);
      }
    }

    [BurstCompile]
    private struct UpdateWindVelocityJob : IJobFor
    {
      public NativeArray<WindSimulationSystem.WindCell> m_Cells;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      public float2 m_TerrainRange;

      public void Execute(int index)
      {
        int3 int3 = new int3(index % WindSimulationSystem.kResolution.x, index / WindSimulationSystem.kResolution.x % WindSimulationSystem.kResolution.y, index / (WindSimulationSystem.kResolution.x * WindSimulationSystem.kResolution.y));
        bool3 bool3 = new bool3(int3.x >= WindSimulationSystem.kResolution.x - 1, int3.y >= WindSimulationSystem.kResolution.y - 1, int3.z >= WindSimulationSystem.kResolution.z - 1);
        if (bool3.x || bool3.y || bool3.z)
          return;
        int3 position1 = new int3(int3.x, int3.y + 1, int3.z);
        int3 position2 = new int3(int3.x + 1, int3.y, int3.z);
        float3 cellCenter = WindSimulationSystem.GetCellCenter(index) with
        {
          y = math.lerp(this.m_TerrainRange.x, this.m_TerrainRange.y, ((float) int3.z + 0.5f) / (float) WindSimulationSystem.kResolution.z)
        };
        float num1 = WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, cellCenter);
        float num2 = WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, cellCenter);
        float num3 = WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, cellCenter);
        float num4 = (float) ((double) ushort.MaxValue / ((double) this.m_TerrainHeightData.scale.y * (double) WindSimulationSystem.kResolution.z));
        float s1 = math.saturate(((float) (0.5 * ((double) num4 + (double) num1 + (double) num2)) - cellCenter.y) / num4);
        float s2 = math.saturate(((float) (0.5 * ((double) num4 + (double) num1 + (double) num3)) - cellCenter.y) / num4);
        WindSimulationSystem.WindCell cell1 = this.m_Cells[index];
        WindSimulationSystem.WindCell cell2 = WindSimulationSystem.GetCell(new int3(int3.x, int3.y, int3.z + 1), this.m_Cells);
        WindSimulationSystem.WindCell cell3 = WindSimulationSystem.GetCell(position1, this.m_Cells);
        NativeArray<WindSimulationSystem.WindCell> cells = this.m_Cells;
        WindSimulationSystem.WindCell cell4 = WindSimulationSystem.GetCell(position2, cells);
        cell1.m_Velocities.x *= math.lerp(WindSimulationSystem.kAirSlowdown, WindSimulationSystem.kTerrainSlowdown, s2);
        cell1.m_Velocities.y *= math.lerp(WindSimulationSystem.kAirSlowdown, WindSimulationSystem.kTerrainSlowdown, s1);
        cell1.m_Velocities.z *= WindSimulationSystem.kVerticalSlowdown;
        cell1.m_Velocities.x += (float) ((double) WindSimulationSystem.kChangeFactor * (1.0 - (double) s2) * ((double) cell1.m_Pressure - (double) cell4.m_Pressure));
        cell1.m_Velocities.y += (float) ((double) WindSimulationSystem.kChangeFactor * (1.0 - (double) s1) * ((double) cell1.m_Pressure - (double) cell3.m_Pressure));
        cell1.m_Velocities.z += WindSimulationSystem.kChangeFactor * (cell1.m_Pressure - cell2.m_Pressure);
        this.m_Cells[index] = cell1;
      }
    }

    [BurstCompile]
    private struct UpdatePressureJob : IJobFor
    {
      public NativeArray<WindSimulationSystem.WindCell> m_Cells;
      public float2 m_Wind;

      public void Execute(int index)
      {
        int3 int3 = new int3(index % WindSimulationSystem.kResolution.x, index / WindSimulationSystem.kResolution.x % WindSimulationSystem.kResolution.y, index / (WindSimulationSystem.kResolution.x * WindSimulationSystem.kResolution.y));
        bool3 bool3_1 = new bool3(int3.x == 0, int3.y == 0, int3.z == 0);
        bool3 bool3_2 = new bool3(int3.x >= WindSimulationSystem.kResolution.x - 1, int3.y >= WindSimulationSystem.kResolution.y - 1, int3.z >= WindSimulationSystem.kResolution.z - 1);
        if (!bool3_2.x && !bool3_2.y && !bool3_2.z)
        {
          WindSimulationSystem.WindCell cell1 = this.m_Cells[index];
          cell1.m_Pressure -= cell1.m_Velocities.x + cell1.m_Velocities.y + cell1.m_Velocities.z;
          if (!bool3_1.x)
          {
            WindSimulationSystem.WindCell cell2 = WindSimulationSystem.GetCell(new int3(int3.x - 1, int3.y, int3.z), this.m_Cells);
            cell1.m_Pressure += cell2.m_Velocities.x;
          }
          if (!bool3_1.y)
          {
            WindSimulationSystem.WindCell cell3 = WindSimulationSystem.GetCell(new int3(int3.x, int3.y - 1, int3.z), this.m_Cells);
            cell1.m_Pressure += cell3.m_Velocities.y;
          }
          if (!bool3_1.z)
          {
            WindSimulationSystem.WindCell cell4 = WindSimulationSystem.GetCell(new int3(int3.x, int3.y, int3.z - 1), this.m_Cells);
            cell1.m_Pressure += cell4.m_Velocities.z;
          }
          this.m_Cells[index] = cell1;
        }
        if (!bool3_1.x && !bool3_1.y && !bool3_2.x && !bool3_2.y)
          return;
        WindSimulationSystem.WindCell cell = this.m_Cells[index];
        float num1 = math.dot(math.normalize(new float2((float) (int3.x - WindSimulationSystem.kResolution.x / 2), (float) (int3.y - WindSimulationSystem.kResolution.y / 2))), math.normalize(this.m_Wind));
        float num2 = math.pow((1f + (float) int3.z) / (1f + (float) WindSimulationSystem.kResolution.z), 0.142857149f);
        float num3 = (float) (0.10000000149011612 * (2.0 - (double) num1));
        float x = (float) (40.0 - 20.0 * (1.0 + (double) num1)) * math.length(this.m_Wind) * num2;
        cell.m_Pressure = (double) x > (double) cell.m_Pressure ? math.min(x, cell.m_Pressure + num3) : math.max(x, cell.m_Pressure - num3);
        this.m_Cells[index] = cell;
      }
    }
  }
}
