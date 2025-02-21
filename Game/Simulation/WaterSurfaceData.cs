// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaterSurfaceData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Collections;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct WaterSurfaceData
  {
    public NativeArray<SurfaceWater> depths { get; private set; }

    public int3 resolution { get; private set; }

    public float3 scale { get; private set; }

    public float3 offset { get; private set; }

    public bool isCreated => this.depths.IsCreated;

    public WaterSurfaceData(
      NativeArray<SurfaceWater> _depths,
      int3 _resolution,
      float3 _scale,
      float3 _offset)
    {
      this.depths = _depths;
      this.resolution = _resolution;
      this.scale = _scale;
      this.offset = _offset;
    }
  }
}
