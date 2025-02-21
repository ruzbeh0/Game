// Decompiled with JetBrains decompiler
// Type: Game.Rendering.Utilities.ManagedStructuredBuffers`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

#nullable disable
namespace Game.Rendering.Utilities
{
  public class ManagedStructuredBuffers<T> : IDisposable where T : unmanaged
  {
    private const int k_NumBufferedFrames = 3;
    private int m_CurrFrame;
    private int m_CurrSize;
    private List<ComputeBuffer>[] m_UploadBuffers;
    private List<ComputeBuffer> m_ReuseBuffers;

    public ManagedStructuredBuffers(int InitialSize)
    {
      this.m_CurrFrame = 0;
      this.m_CurrSize = InitialSize;
      this.m_UploadBuffers = new List<ComputeBuffer>[3];
      for (int index = 0; index < 3; ++index)
        this.m_UploadBuffers[index] = new List<ComputeBuffer>();
      this.m_ReuseBuffers = new List<ComputeBuffer>();
    }

    public ComputeBuffer Request(int AmountNeeded)
    {
      if (AmountNeeded > this.m_CurrSize)
      {
        this.m_CurrSize = AmountNeeded + 1000;
        this.ReleaseBuffers(this.m_CurrFrame);
      }
      ComputeBuffer computeBuffer;
      if (this.m_ReuseBuffers.Count > 0)
      {
        computeBuffer = this.m_ReuseBuffers[this.m_ReuseBuffers.Count - 1];
        this.m_ReuseBuffers.RemoveAt(this.m_ReuseBuffers.Count - 1);
      }
      else
        computeBuffer = new ComputeBuffer(this.m_CurrSize, UnsafeUtility.SizeOf<T>(), ComputeBufferType.Structured);
      this.m_UploadBuffers[this.m_CurrFrame].Add(computeBuffer);
      return computeBuffer;
    }

    public void StartFrame()
    {
      List<ComputeBuffer> uploadBuffer = this.m_UploadBuffers[this.m_CurrFrame];
      for (int index = 0; index < uploadBuffer.Count; ++index)
      {
        if (uploadBuffer[index].count < this.m_CurrSize)
          uploadBuffer[index].Release();
        else
          this.m_ReuseBuffers.Add(uploadBuffer[index]);
      }
      uploadBuffer.Clear();
    }

    public void EndFrame()
    {
      if (++this.m_CurrFrame < 3)
        return;
      this.m_CurrFrame = 0;
    }

    public void Dispose() => this.ReleaseBuffers();

    private void ReleaseBuffers(int IgnoreIndex = -1)
    {
      for (int index = 0; index < this.m_ReuseBuffers.Count; ++index)
        this.m_ReuseBuffers[index].Release();
      this.m_ReuseBuffers.Clear();
      for (int index = 0; index < 3; ++index)
      {
        if (index != IgnoreIndex)
        {
          foreach (ComputeBuffer computeBuffer in this.m_UploadBuffers[index])
            computeBuffer.Release();
          this.m_UploadBuffers[index].Clear();
        }
      }
    }
  }
}
