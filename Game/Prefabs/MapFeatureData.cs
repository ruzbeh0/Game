// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MapFeatureData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(8)]
  public struct MapFeatureData : IBufferElementData
  {
    public float m_Cost;

    public MapFeatureData(float cost) => this.m_Cost = cost;
  }
}
