// Decompiled with JetBrains decompiler
// Type: Game.Common.QuadTreeBoundsXZ
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Collections;
using Colossal.Mathematics;
using System;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Common
{
  public struct QuadTreeBoundsXZ : IEquatable<QuadTreeBoundsXZ>, IBounds2<QuadTreeBoundsXZ>
  {
    public Bounds3 m_Bounds;
    public BoundsMask m_Mask;
    public byte m_MinLod;
    public byte m_MaxLod;

    public QuadTreeBoundsXZ(Bounds3 bounds)
    {
      this.m_Bounds = bounds;
      this.m_Mask = BoundsMask.Debug | BoundsMask.NormalLayers | BoundsMask.NotOverridden | BoundsMask.NotWalkThrough;
      this.m_MinLod = (byte) 1;
      this.m_MaxLod = (byte) 1;
    }

    public QuadTreeBoundsXZ(Bounds3 bounds, BoundsMask mask, int lod)
    {
      this.m_Bounds = bounds;
      this.m_Mask = mask;
      this.m_MinLod = (byte) lod;
      this.m_MaxLod = (byte) lod;
    }

    public QuadTreeBoundsXZ(Bounds3 bounds, BoundsMask mask, int minLod, int maxLod)
    {
      this.m_Bounds = bounds;
      this.m_Mask = mask;
      this.m_MinLod = (byte) minLod;
      this.m_MaxLod = (byte) maxLod;
    }

    public bool Equals(QuadTreeBoundsXZ other)
    {
      return this.m_Bounds.Equals(other.m_Bounds) & this.m_Mask == other.m_Mask & this.m_MinLod.Equals(other.m_MinLod) & this.m_MaxLod.Equals(other.m_MaxLod);
    }

    public void Reset()
    {
      this.m_Bounds.min = (float3) float.MaxValue;
      this.m_Bounds.max = (float3) float.MinValue;
      this.m_Mask = (BoundsMask) 0;
      this.m_MinLod = byte.MaxValue;
      this.m_MaxLod = (byte) 0;
    }

    public float2 Center() => MathUtils.Center(this.m_Bounds).xz;

    public float2 Size() => MathUtils.Size(this.m_Bounds).xz;

    public QuadTreeBoundsXZ Merge(QuadTreeBoundsXZ other)
    {
      return new QuadTreeBoundsXZ(this.m_Bounds | other.m_Bounds, this.m_Mask | other.m_Mask, math.min((int) this.m_MinLod, (int) other.m_MinLod), math.max((int) this.m_MaxLod, (int) other.m_MaxLod));
    }

    public bool Intersect(QuadTreeBoundsXZ other)
    {
      return MathUtils.Intersect(this.m_Bounds, other.m_Bounds);
    }

    public struct DebugIterator<TItem> : 
      INativeQuadTreeIterator<TItem, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<TItem, QuadTreeBoundsXZ>
      where TItem : unmanaged, IEquatable<TItem>
    {
      private Bounds3 m_Bounds;
      private GizmoBatcher m_GizmoBatcher;

      public DebugIterator(Bounds3 bounds, GizmoBatcher gizmoBatcher)
      {
        this.m_Bounds = bounds;
        this.m_GizmoBatcher = gizmoBatcher;
      }

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds))
          return false;
        this.m_GizmoBatcher.DrawWireCube(MathUtils.Center(bounds.m_Bounds), MathUtils.Size(bounds.m_Bounds), Color.white);
        return true;
      }

      public void Iterate(QuadTreeBoundsXZ bounds, TItem edgeEntity)
      {
        if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds))
          return;
        this.m_GizmoBatcher.DrawWireCube(MathUtils.Center(bounds.m_Bounds), MathUtils.Size(bounds.m_Bounds), Color.red);
      }
    }
  }
}
