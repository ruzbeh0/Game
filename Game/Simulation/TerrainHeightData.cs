// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TerrainHeightData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Collections;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct TerrainHeightData
  {
    public NativeArray<ushort> heights { get; private set; }

    public int3 resolution { get; private set; }

    public float3 scale { get; private set; }

    public float3 offset { get; private set; }

    public bool isCreated => this.heights.IsCreated;

    public TerrainHeightData(
      NativeArray<ushort> _heights,
      int3 _resolution,
      float3 _scale,
      float3 _offset)
    {
      this.heights = _heights;
      this.resolution = _resolution;
      this.scale = _scale;
      this.offset = _offset;
    }
  }
}
