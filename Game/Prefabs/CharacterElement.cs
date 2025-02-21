// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CharacterElement
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Rendering;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct CharacterElement : IBufferElementData
  {
    public Entity m_Style;
    public BlendWeights m_ShapeWeights;
    public BlendWeights m_TextureWeights;
    public BlendWeights m_OverlayWeights;
    public BlendWeights m_MaskWeights;
    public int m_RestPoseClipIndex;
    public int m_CorrectiveClipIndex;
  }
}
