// Decompiled with JetBrains decompiler
// Type: Game.Serialization.WriteBuffer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Collections;
using Unity.Jobs;

#nullable disable
namespace Game.Serialization
{
  public class WriteBuffer : IWriteBuffer, IDisposable
  {
    private JobHandle m_WriteDependencies;
    private bool m_HasDependencies;
    private bool m_IsDone;

    public NativeList<byte> buffer { get; private set; }

    public bool isCompleted
    {
      get
      {
        if (!this.m_IsDone)
          return false;
        return !this.m_HasDependencies || this.m_WriteDependencies.IsCompleted;
      }
    }

    public WriteBuffer()
    {
      this.buffer = new NativeList<byte>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    public void CompleteDependencies()
    {
      if (!this.m_HasDependencies)
        return;
      this.m_WriteDependencies.Complete();
      this.m_WriteDependencies = new JobHandle();
      this.m_HasDependencies = false;
    }

    private void DisposeBuffers()
    {
      this.CompleteDependencies();
      NativeList<byte> buffer = this.buffer;
      if (buffer.IsCreated)
        buffer.Dispose();
      this.buffer = buffer;
    }

    public void Dispose() => this.DisposeBuffers();

    public void Done(JobHandle handle)
    {
      this.m_WriteDependencies = JobHandle.CombineDependencies(this.m_WriteDependencies, handle);
      this.m_HasDependencies = true;
      this.m_IsDone = true;
    }

    public void Done() => this.m_IsDone = true;
  }
}
