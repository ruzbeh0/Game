// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TerrainAreaData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct TerrainAreaData : IComponentData, IQueryTypeParameter
  {
    public float m_HeightOffset;
    public float m_SlopeWidth;
    public float m_NoiseScale;
    public float m_NoiseFactor;
  }
}
