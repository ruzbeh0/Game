// Decompiled with JetBrains decompiler
// Type: Game.Rendering.AreaTriangleData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  public struct AreaTriangleData
  {
    public float3 m_APos;
    public float3 m_BPos;
    public float3 m_CPos;
    public float2 m_APrevXZ;
    public float2 m_BPrevXZ;
    public float2 m_CPrevXZ;
    public float2 m_ANextXZ;
    public float2 m_BNextXZ;
    public float2 m_CNextXZ;
    public float2 m_YMinMax;
    public float4 m_OffsetDir;
    public float m_LodDistanceFactor;
  }
}
