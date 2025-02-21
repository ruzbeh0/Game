// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CellMapSystem`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public abstract class CellMapSystem<T> : GameSystemBase where T : struct, ISerializable
  {
    public static readonly int kMapSize = 14336;
    protected JobHandle m_ReadDependencies;
    protected JobHandle m_WriteDependencies;
    protected NativeArray<T> m_Map;
    protected int2 m_TextureSize;

    public JobHandle Serialize<TWriter>(EntityWriterData writerData, JobHandle inputDeps) where TWriter : struct, IWriter
    {
      T obj = default (T);
      int num = 0;
      if (obj is IStrideSerializable strideSerializable)
      {
        TWriter writer = writerData.GetWriter<TWriter>();
        num = strideSerializable.GetStride(writer.context);
      }
      JobHandle job1 = new CellMapSystem<T>.SerializeJob<TWriter>()
      {
        m_Stride = num,
        m_Map = this.m_Map,
        m_WriterData = writerData
      }.Schedule<CellMapSystem<T>.SerializeJob<TWriter>>(JobHandle.CombineDependencies(inputDeps, this.m_WriteDependencies));
      this.m_ReadDependencies = JobHandle.CombineDependencies(this.m_ReadDependencies, job1);
      return job1;
    }

    public virtual JobHandle Deserialize<TReader>(EntityReaderData readerData, JobHandle inputDeps) where TReader : struct, IReader
    {
      T obj = default (T);
      int num = 0;
      if (obj is IStrideSerializable strideSerializable)
      {
        TReader reader = readerData.GetReader<TReader>();
        num = strideSerializable.GetStride(reader.context);
      }
      this.m_WriteDependencies = new CellMapSystem<T>.DeserializeJob<TReader>()
      {
        m_Stride = num,
        m_Map = this.m_Map,
        m_ReaderData = readerData
      }.Schedule<CellMapSystem<T>.DeserializeJob<TReader>>(JobHandle.CombineDependencies(inputDeps, this.m_ReadDependencies, this.m_WriteDependencies));
      return this.m_WriteDependencies;
    }

    public virtual JobHandle SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      this.m_WriteDependencies = new CellMapSystem<T>.SetDefaultsJob()
      {
        m_Map = this.m_Map
      }.Schedule<CellMapSystem<T>.SetDefaultsJob>(JobHandle.CombineDependencies(this.m_ReadDependencies, this.m_WriteDependencies));
      return this.m_WriteDependencies;
    }

    public NativeArray<T> GetMap(bool readOnly, out JobHandle dependencies)
    {
      dependencies = readOnly ? this.m_WriteDependencies : JobHandle.CombineDependencies(this.m_ReadDependencies, this.m_WriteDependencies);
      return this.m_Map;
    }

    public CellMapData<T> GetData(bool readOnly, out JobHandle dependencies)
    {
      dependencies = readOnly ? this.m_WriteDependencies : JobHandle.CombineDependencies(this.m_ReadDependencies, this.m_WriteDependencies);
      return new CellMapData<T>()
      {
        m_Buffer = this.m_Map,
        m_CellSize = (float2) CellMapSystem<T>.kMapSize / (float2) this.m_TextureSize,
        m_TextureSize = this.m_TextureSize
      };
    }

    public void AddReader(JobHandle jobHandle)
    {
      this.m_ReadDependencies = JobHandle.CombineDependencies(this.m_ReadDependencies, jobHandle);
    }

    public void AddWriter(JobHandle jobHandle) => this.m_WriteDependencies = jobHandle;

    public static float3 GetCellCenter(int index, int textureSize)
    {
      int num1 = index % textureSize;
      int num2 = index / textureSize;
      int num3 = CellMapSystem<T>.kMapSize / textureSize;
      return new float3((float) (-0.5 * (double) CellMapSystem<T>.kMapSize + ((double) num1 + 0.5) * (double) num3), 0.0f, (float) (-0.5 * (double) CellMapSystem<T>.kMapSize + ((double) num2 + 0.5) * (double) num3));
    }

    public static Bounds3 GetCellBounds(int index, int textureSize)
    {
      int num1 = index % textureSize;
      int num2 = index / textureSize;
      int num3 = CellMapSystem<T>.kMapSize / textureSize;
      return new Bounds3(new float3(-0.5f * (float) CellMapSystem<T>.kMapSize + (float) (num1 * num3), -100000f, -0.5f * (float) CellMapSystem<T>.kMapSize + (float) (num2 * num3)), new float3((float) (-0.5 * (double) CellMapSystem<T>.kMapSize + ((double) num1 + 1.0) * (double) num3), 100000f, (float) (-0.5 * (double) CellMapSystem<T>.kMapSize + ((double) num2 + 1.0) * (double) num3)));
    }

    public static float3 GetCellCenter(int2 cell, int textureSize)
    {
      int num = CellMapSystem<T>.kMapSize / textureSize;
      return new float3((float) (-0.5 * (double) CellMapSystem<T>.kMapSize + ((double) cell.x + 0.5) * (double) num), 0.0f, (float) (-0.5 * (double) CellMapSystem<T>.kMapSize + ((double) cell.y + 0.5) * (double) num));
    }

    public static float2 GetCellCoords(float3 position, int mapSize, int textureSize)
    {
      return (0.5f + position.xz / (float) mapSize) * (float) textureSize;
    }

    public static int2 GetCell(float3 position, int mapSize, int textureSize)
    {
      return (int2) math.floor(CellMapSystem<T>.GetCellCoords(position, mapSize, textureSize));
    }

    protected void CreateTextures(int textureSize)
    {
      this.m_Map = new NativeArray<T>(textureSize * textureSize, Allocator.Persistent);
      this.m_TextureSize = new int2(textureSize, textureSize);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      this.m_ReadDependencies.Complete();
      this.m_WriteDependencies.Complete();
      this.m_Map.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected CellMapSystem()
    {
    }

    [BurstCompile]
    internal struct SerializeJob<TWriter> : IJob where TWriter : struct, IWriter
    {
      [ReadOnly]
      public int m_Stride;
      [ReadOnly]
      public NativeArray<T> m_Map;
      public EntityWriterData m_WriterData;

      public void Execute()
      {
        TWriter writer = this.m_WriterData.GetWriter<TWriter>();
        if (this.m_Stride != 0 && this.m_Map.Length != 0)
        {
          NativeList<byte> buffer = new NativeList<byte>(1000, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          this.m_WriterData.GetWriter<TWriter>(buffer).Write<T>(this.m_Map);
          writer.Write(-this.m_Map.Length);
          writer.Write(buffer.Length);
          writer.Write(buffer.AsArray(), this.m_Stride);
          buffer.Dispose();
        }
        else
        {
          writer.Write(this.m_Map.Length);
          writer.Write<T>(this.m_Map);
        }
      }
    }

    [BurstCompile]
    internal struct DeserializeJob<TReader> : IJob where TReader : struct, IReader
    {
      [ReadOnly]
      public int m_Stride;
      public NativeArray<T> m_Map;
      public EntityReaderData m_ReaderData;

      public void Execute()
      {
        TReader reader = this.m_ReaderData.GetReader<TReader>();
        if (!(reader.context.version > Version.stormWater))
          return;
        if (reader.context.version > Version.cellMapLengths)
        {
          int num;
          reader.Read(out num);
          if (this.m_Map.Length == num)
          {
            reader.Read<T>(this.m_Map);
          }
          else
          {
            if (this.m_Map.Length != -num)
              return;
            int length;
            reader.Read(out length);
            NativeArray<byte> buffer = new NativeArray<byte>(length, Allocator.Temp);
            NativeReference<int> position = new NativeReference<int>(0, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            reader.Read(buffer, this.m_Stride);
            this.m_ReaderData.GetReader<TReader>(buffer, position).Read<T>(this.m_Map);
            buffer.Dispose();
            position.Dispose();
          }
        }
        else
          reader.Read<T>(this.m_Map);
      }
    }

    [BurstCompile]
    private struct SetDefaultsJob : IJob
    {
      public NativeArray<T> m_Map;

      public void Execute()
      {
        for (int index = 0; index < this.m_Map.Length; ++index)
          this.m_Map[index] = default (T);
      }
    }
  }
}
