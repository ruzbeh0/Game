// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TerrainComposition
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct TerrainComposition : IComponentData, IQueryTypeParameter
  {
    public float2 m_WidthOffset;
    public float2 m_ClipHeightOffset;
    public float3 m_MinHeightOffset;
    public float3 m_MaxHeightOffset;
  }
}
