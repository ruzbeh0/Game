// Decompiled with JetBrains decompiler
// Type: Colossal.Rendering.VTTextureRequester
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase.VirtualTexturing;
using Colossal.Mathematics;
using System;
using Unity.Collections;
using UnityEngine;

#nullable disable
namespace Colossal.Rendering
{
  public class VTTextureRequester : IDisposable
  {
    private NativeList<int>[] m_TexturesIndices;
    private NativeList<int>[] m_StackGlobalIndices;
    private NativeList<Bounds2>[] m_TextureBounds;
    private NativeList<float>[] m_TexturesMaxPixels;
    private TextureStreamingSystem m_TextureStreamingSystem;
    private int m_RequestedThisFrame;

    public int stacksCount => this.m_TexturesIndices.Length;

    public int registeredCount
    {
      get
      {
        int registeredCount = 0;
        foreach (NativeList<int> texturesIndex in this.m_TexturesIndices)
          registeredCount += texturesIndex.Length;
        return registeredCount;
      }
    }

    public int requestCount => this.m_RequestedThisFrame;

    public VTTextureRequester(TextureStreamingSystem textureStreamingSystem)
    {
      this.m_TextureStreamingSystem = textureStreamingSystem;
      this.m_TexturesIndices = new NativeList<int>[2];
      this.m_TexturesIndices[0] = new NativeList<int>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_TexturesIndices[1] = new NativeList<int>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_StackGlobalIndices = new NativeList<int>[2];
      this.m_StackGlobalIndices[0] = new NativeList<int>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_StackGlobalIndices[1] = new NativeList<int>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_TexturesMaxPixels = new NativeList<float>[2];
      this.m_TexturesMaxPixels[0] = new NativeList<float>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_TexturesMaxPixels[1] = new NativeList<float>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_TextureBounds = new NativeList<Bounds2>[2];
      this.m_TextureBounds[0] = new NativeList<Bounds2>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_TextureBounds[1] = new NativeList<Bounds2>(100, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    public NativeList<float>[] TexturesMaxPixels => this.m_TexturesMaxPixels;

    public void Dispose()
    {
      for (int index = 0; index < this.m_TexturesIndices.Length; ++index)
        this.m_TexturesIndices[index].Dispose();
      for (int index = 0; index < this.m_StackGlobalIndices.Length; ++index)
        this.m_StackGlobalIndices[index].Dispose();
      for (int index = 0; index < this.m_TexturesMaxPixels.Length; ++index)
        this.m_TexturesMaxPixels[index].Dispose();
      for (int index = 0; index < this.m_TextureBounds.Length; ++index)
        this.m_TextureBounds[index].Dispose();
    }

    public void Clear()
    {
      for (int index = 0; index < this.m_TexturesIndices.Length; ++index)
        this.m_TexturesIndices[index].Clear();
      for (int index = 0; index < this.m_StackGlobalIndices.Length; ++index)
        this.m_StackGlobalIndices[index].Clear();
      for (int index = 0; index < this.m_TexturesMaxPixels.Length; ++index)
        this.m_TexturesMaxPixels[index].Clear();
      for (int index = 0; index < this.m_TextureBounds.Length; ++index)
        this.m_TextureBounds[index].Clear();
    }

    public void UpdateTexturesVTRequests()
    {
      this.m_RequestedThisFrame = 0;
      float num = 1f;
      if ((double) this.m_TextureStreamingSystem.workingSetLodBias > 1.0)
        num = 1f / this.m_TextureStreamingSystem.workingSetLodBias;
      for (int index1 = 0; index1 < 2; ++index1)
      {
        NativeList<int> texturesIndex = this.m_TexturesIndices[index1];
        NativeList<int> stackGlobalIndex = this.m_StackGlobalIndices[index1];
        NativeList<float> texturesMaxPixel = this.m_TexturesMaxPixels[index1];
        for (int index2 = 0; index2 < texturesIndex.Length; ++index2)
        {
          float maxPixel = texturesMaxPixel[index2] * num;
          if ((double) maxPixel > 4.0)
          {
            this.m_TextureStreamingSystem.RequestRegion(stackGlobalIndex[index2], texturesIndex[index2], maxPixel, this.m_TextureBounds[index1][index2]);
            texturesMaxPixel[index2] = -1f;
            ++this.m_RequestedThisFrame;
          }
        }
      }
    }

    public int RegisterTexture(
      int stackConfigIndex,
      int stackGlobalIndex,
      int vtIndex,
      Bounds2 bounds)
    {
      NativeList<int> texturesIndex = this.m_TexturesIndices[stackConfigIndex];
      NativeList<int> stackGlobalIndex1 = this.m_StackGlobalIndices[stackConfigIndex];
      NativeList<Bounds2> textureBound = this.m_TextureBounds[stackConfigIndex];
      for (int index = 0; index < texturesIndex.Length; ++index)
      {
        if (texturesIndex[index] == vtIndex && stackGlobalIndex1[index] == stackGlobalIndex && textureBound[index].Equals(bounds))
          return index;
      }
      texturesIndex.Add(in vtIndex);
      this.m_TexturesMaxPixels[stackConfigIndex].Add(-1f);
      textureBound.Add(in bounds);
      stackGlobalIndex1.Add(in stackGlobalIndex);
      return texturesIndex.Length - 1;
    }

    public int GetTextureIndex(int stackIndex, int texturesIndex)
    {
      return this.m_TexturesIndices[stackIndex][texturesIndex];
    }

    public void UpdateMaxPixel(int stackIndex, int texturesIndex, float maxPixel)
    {
      this.m_TexturesMaxPixels[stackIndex][texturesIndex] = Mathf.Max(this.m_TexturesMaxPixels[stackIndex][texturesIndex], maxPixel);
    }
  }
}
