// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CharacterStyleData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct CharacterStyleData : IComponentData, IQueryTypeParameter
  {
    public ActivityMask m_ActivityMask;
    public AnimationLayerMask m_AnimationLayerMask;
    public int m_BoneCount;
    public int m_ShapeCount;
    public int m_RestPoseClipIndex;
  }
}
