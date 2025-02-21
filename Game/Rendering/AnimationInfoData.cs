// Decompiled with JetBrains decompiler
// Type: Game.Rendering.AnimationInfoData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  public struct AnimationInfoData
  {
    public int m_Offset;
    public int m_Hierarchy;
    public int m_Shapes;
    public int m_Bones;
    public int m_InverseBones;
    public int m_ShapeCount;
    public int m_BoneCount;
    public int m_Type;
    public float3 m_PositionMin;
    public float3 m_PositionRange;
  }
}
