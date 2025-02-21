// Decompiled with JetBrains decompiler
// Type: Game.Serialization.ReadSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Compression;
using Colossal.Entities;
using Colossal.IO.AssetDatabase;
using Colossal.Serialization.Entities;
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Serialization
{
  public class ReadSystem : GameSystemBase, IReadBufferProvider<ReadBuffer>
  {
    private LoadGameSystem m_DeserializationSystem;
    private SerializerSystem m_SerializerSystem;
    private StreamBinaryReader m_Reader;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_DeserializationSystem = this.World.GetOrCreateSystemManaged<LoadGameSystem>();
      this.m_SerializerSystem = this.World.GetOrCreateSystemManaged<SerializerSystem>();
    }

    [Preserve]
    protected override void OnDestroy()
    {
      this.Clear();
      base.OnDestroy();
    }

    public ReadBuffer GetBuffer(BufferFormat format)
    {
      if (this.m_DeserializationSystem.dataDescriptor == AsyncReadDescriptor.Invalid)
        return (ReadBuffer) null;
      if (this.m_Reader == null)
        this.m_Reader = new StreamBinaryReader(this.m_DeserializationSystem.dataDescriptor);
      ReadSystem.BufferHeader data = new ReadSystem.BufferHeader();
      if (format.IsCompressed())
      {
        ReadSystem.ReadData<ReadSystem.BufferHeader>(this.m_Reader, out data);
        this.m_SerializerSystem.totalSize += sizeof (ReadSystem.BufferHeader);
      }
      else
      {
        ReadSystem.ReadData<int>(this.m_Reader, out data.size);
        this.m_SerializerSystem.totalSize += 4;
      }
      ReadBuffer buffer = new ReadBuffer(data.size);
      this.m_SerializerSystem.totalSize += data.size;
      if (format.IsCompressed())
      {
        NativeArray<byte> nativeArray = new NativeArray<byte>(data.compressedSize, Allocator.Persistent);
        ReadSystem.ReadData(this.m_Reader, nativeArray);
        using (PerformanceCounter.Start((Action<TimeSpan>) (t => COSystemBase.baseLog.VerboseFormat("Decompressed in {0}ms", (object) t.TotalMilliseconds))))
          CompressionUtils.Decompress(SerializationUtils.BufferToCompressionFormat(format), (NativeSlice<byte>) nativeArray, (NativeSlice<byte>) buffer.buffer).Complete();
        nativeArray.Dispose();
      }
      else if (format == BufferFormat.Raw)
        ReadSystem.ReadData(this.m_Reader, buffer.buffer);
      else
        COSystemBase.baseLog.WarnFormat("Unsupported BufferFormat {0}", (object) format);
      return buffer;
    }

    public ReadBuffer GetBuffer(BufferFormat format, out JobHandle dependency)
    {
      dependency = new JobHandle();
      if (this.m_DeserializationSystem.dataDescriptor == AsyncReadDescriptor.Invalid)
        return (ReadBuffer) null;
      if (this.m_Reader == null)
        this.m_Reader = new StreamBinaryReader(this.m_DeserializationSystem.dataDescriptor);
      ReadSystem.BufferHeader data = new ReadSystem.BufferHeader();
      if (format.IsCompressed())
      {
        ReadSystem.ReadData<ReadSystem.BufferHeader>(this.m_Reader, out data);
        this.m_SerializerSystem.totalSize += sizeof (ReadSystem.BufferHeader);
      }
      else
      {
        ReadSystem.ReadData<int>(this.m_Reader, out data.size);
        this.m_SerializerSystem.totalSize += 4;
      }
      ReadBuffer buffer = new ReadBuffer(data.size);
      this.m_SerializerSystem.totalSize += data.size;
      if (format.IsCompressed())
      {
        NativeArray<byte> nativeArray = new NativeArray<byte>(data.compressedSize, Allocator.Persistent);
        ReadSystem.ReadData(this.m_Reader, nativeArray, out dependency);
        dependency = CompressionUtils.Decompress(SerializationUtils.BufferToCompressionFormat(format), (NativeSlice<byte>) nativeArray, (NativeSlice<byte>) buffer.buffer, dependency);
        nativeArray.Dispose(dependency);
      }
      else if (format == BufferFormat.Raw)
        ReadSystem.ReadData(this.m_Reader, buffer.buffer, out dependency);
      else
        COSystemBase.baseLog.WarnFormat("Unsupported BufferFormat {0}", (object) format);
      return buffer;
    }

    [Preserve]
    protected override void OnUpdate() => this.Clear();

    private void Clear()
    {
      if (this.m_Reader == null)
        return;
      this.m_Reader.Dispose();
      this.m_Reader = (StreamBinaryReader) null;
    }

    private static unsafe void ReadData<T>(StreamBinaryReader reader, out T data) where T : unmanaged
    {
      data = default (T);
      void* data1 = UnsafeUtility.AddressOf<T>(ref data);
      reader.ReadBytes(data1, sizeof (T));
    }

    private static unsafe void ReadData(StreamBinaryReader reader, NativeArray<byte> data)
    {
      void* unsafePtr = data.GetUnsafePtr<byte>();
      reader.ReadBytes(unsafePtr, data.Length);
    }

    private static unsafe void ReadData(
      StreamBinaryReader reader,
      NativeArray<byte> data,
      out JobHandle dependency)
    {
      void* unsafePtr = data.GetUnsafePtr<byte>();
      reader.ReadBytes(unsafePtr, data.Length, out dependency);
    }

    [Preserve]
    public ReadSystem()
    {
    }

    private struct BufferHeader
    {
      public int size;
      public int compressedSize;
    }
  }
}
