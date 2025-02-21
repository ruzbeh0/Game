// Decompiled with JetBrains decompiler
// Type: Game.Rendering.BatchData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Rendering
{
  public struct BatchData
  {
    public Entity m_LodMesh;
    public int m_VTIndex0;
    public int m_VTIndex1;
    public float m_VTSizeFactor;
    public BatchRenderFlags m_RenderFlags;
    public byte m_ShadowCastingMode;
    public byte m_Layer;
    public byte m_SubMeshIndex;
    public byte m_MinLod;
    public byte m_ShadowLod;
    public byte m_LodIndex;
    public float m_ShadowArea;
    public float m_ShadowHeight;
  }
}
