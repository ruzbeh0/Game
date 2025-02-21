// Decompiled with JetBrains decompiler
// Type: Game.Serialization.WriteSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.AssetPipeline.Native;
using Colossal.Compression;
using Colossal.Entities;
using Colossal.IO.AssetDatabase;
using Colossal.Serialization.Entities;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Serialization
{
  public class WriteSystem : GameSystemBase, IWriteBufferProvider<Game.Serialization.WriteBuffer>
  {
    private SaveGameSystem m_SerializationSystem;
    private SerializerSystem m_SerializerSystem;
    private List<(Game.Serialization.WriteBuffer, BufferFormat)> m_Buffers;
    private JobHandle m_WriteDependency;
    private GCHandle m_WriterHandle;

    public JobHandle writeDependency => this.m_WriteDependency;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_SerializationSystem = this.World.GetOrCreateSystemManaged<SaveGameSystem>();
      this.m_SerializerSystem = this.World.GetOrCreateSystemManaged<SerializerSystem>();
      this.m_Buffers = new List<(Game.Serialization.WriteBuffer, BufferFormat)>();
    }

    [Preserve]
    protected override void OnDestroy()
    {
      this.m_WriteDependency.Complete();
      for (int index = 0; index < this.m_Buffers.Count; ++index)
        this.m_Buffers[index].Item1.Dispose();
      base.OnDestroy();
    }

    public Game.Serialization.WriteBuffer AddBuffer(BufferFormat format)
    {
      int count = 0;
      for (int index = 0; index < this.m_Buffers.Count; ++index)
      {
        (Game.Serialization.WriteBuffer buffer, BufferFormat format1) = this.m_Buffers[index];
        if (buffer.isCompleted)
        {
          this.WriteBuffer(buffer, format1);
          ++count;
        }
        else
          break;
      }
      if (count != 0)
        this.m_Buffers.RemoveRange(0, count);
      Game.Serialization.WriteBuffer writeBuffer = new Game.Serialization.WriteBuffer();
      this.m_Buffers.Add((writeBuffer, format));
      return writeBuffer;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      for (int index = 0; index < this.m_Buffers.Count; ++index)
      {
        (Game.Serialization.WriteBuffer, BufferFormat) buffer = this.m_Buffers[index];
        this.WriteBuffer(buffer.Item1, buffer.Item2);
      }
      this.m_Buffers.Clear();
      if (!this.m_WriterHandle.IsAllocated)
        return;
      WriteSystem.DisposeWriterJob jobData = new WriteSystem.DisposeWriterJob()
      {
        m_WriterHandle = this.m_WriterHandle
      };
      this.m_WriterHandle = new GCHandle();
      this.m_WriteDependency = jobData.Schedule<WriteSystem.DisposeWriterJob>(this.m_WriteDependency);
    }

    private void WriteBuffer(Game.Serialization.WriteBuffer buffer, BufferFormat format)
    {
      if (!this.m_WriterHandle.IsAllocated)
        this.m_WriterHandle = GCHandle.Alloc((object) new StreamBinaryWriter(this.m_SerializationSystem.stream));
      buffer.CompleteDependencies();
      if (format == BufferFormat.Raw)
      {
        this.m_SerializerSystem.totalSize += 4 + buffer.buffer.Length;
        this.m_WriteDependency = new WriteSystem.WriteRawBufferJob()
        {
          m_Buffer = buffer.buffer,
          m_WriterHandle = this.m_WriterHandle
        }.Schedule<WriteSystem.WriteRawBufferJob>(this.m_WriteDependency);
        buffer.buffer.Dispose(this.m_WriteDependency);
      }
      else if (format.IsCompressed())
      {
        this.m_SerializerSystem.totalSize += 8 + buffer.buffer.Length;
        CompressionFormat compressionFormat = SerializationUtils.BufferToCompressionFormat(format);
        CompressedBytesStorage compressedBytesStorage;
        ref CompressedBytesStorage local = ref compressedBytesStorage;
        int format1 = (int) compressionFormat;
        NativeList<byte> buffer1 = buffer.buffer;
        int length = buffer1.Length;
        local = new CompressedBytesStorage((CompressionFormat) format1, length, Allocator.Persistent);
        int num = 3;
        int format2 = (int) compressionFormat;
        buffer1 = buffer.buffer;
        NativeSlice<byte> inData = (NativeSlice<byte>) buffer1.AsArray();
        CompressedBytesStorage outCompressedData = compressedBytesStorage;
        JobHandle jobHandle1 = new JobHandle();
        int compressionLevel = num;
        JobHandle jobHandle2 = CompressionUtils.Compress((CompressionFormat) format2, inData, outCompressedData, jobHandle1, compressionLevel);
        WriteSystem.WriteCompressedBufferJob jobData = new WriteSystem.WriteCompressedBufferJob()
        {
          m_CompressedData = compressedBytesStorage,
          m_UncompressedSize = buffer.buffer.Length,
          m_WriterHandle = this.m_WriterHandle
        };
        buffer.buffer.Dispose(jobHandle2);
        this.m_WriteDependency = jobData.Schedule<WriteSystem.WriteCompressedBufferJob>(JobHandle.CombineDependencies(this.m_WriteDependency, jobHandle2));
        compressedBytesStorage.Dispose(this.m_WriteDependency);
      }
      else
        COSystemBase.baseLog.WarnFormat("Unsupported BufferFormat {0}", (object) format);
    }

    private static unsafe void WriteData<T>(StreamBinaryWriter writer, T data) where T : unmanaged
    {
      void* data1 = UnsafeUtility.AddressOf<T>(ref data);
      writer.WriteBytes(data1, sizeof (T));
    }

    private static unsafe void WriteData(StreamBinaryWriter writer, NativeArray<byte> data)
    {
      void* unsafeReadOnlyPtr = data.GetUnsafeReadOnlyPtr<byte>();
      writer.WriteBytes(unsafeReadOnlyPtr, data.Length);
    }

    private static unsafe void WriteData(StreamBinaryWriter writer, NativeSlice<byte> data)
    {
      void* unsafeReadOnlyPtr = data.GetUnsafeReadOnlyPtr<byte>();
      writer.WriteBytes(unsafeReadOnlyPtr, data.Length);
    }

    [Preserve]
    public WriteSystem()
    {
    }

    private struct WriteRawBufferJob : IJob
    {
      [ReadOnly]
      public NativeList<byte> m_Buffer;
      public GCHandle m_WriterHandle;

      public void Execute()
      {
        StreamBinaryWriter target = (StreamBinaryWriter) this.m_WriterHandle.Target;
        WriteSystem.WriteData<int>(target, this.m_Buffer.Length);
        WriteSystem.WriteData(target, this.m_Buffer.AsArray());
      }
    }

    private struct WriteCompressedBufferJob : IJob
    {
      [ReadOnly]
      public CompressedBytesStorage m_CompressedData;
      public int m_UncompressedSize;
      public GCHandle m_WriterHandle;

      public unsafe void Execute()
      {
        StreamBinaryWriter target = (StreamBinaryWriter) this.m_WriterHandle.Target;
        WriteSystem.BufferHeader data;
        data.size = this.m_UncompressedSize;
        void* bytes = (void*) this.m_CompressedData.GetBytes(out data.compressedSize);
        WriteSystem.WriteData<WriteSystem.BufferHeader>(target, data);
        target.WriteBytes(bytes, data.compressedSize);
      }
    }

    private struct DisposeWriterJob : IJob
    {
      public GCHandle m_WriterHandle;

      public void Execute()
      {
        ((StreamBinaryWriter) this.m_WriterHandle.Target).Dispose();
        this.m_WriterHandle.Free();
      }
    }

    private struct BufferHeader
    {
      public int size;
      public int compressedSize;
    }
  }
}
