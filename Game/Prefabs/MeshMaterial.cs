// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MeshMaterial
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct MeshMaterial : IBufferElementData
  {
    public int m_StartIndex;
    public int m_IndexCount;
    public int m_StartVertex;
    public int m_VertexCount;
    public int m_MaterialIndex;

    public MeshMaterial(
      int startIndex,
      int indexCount,
      int startVertex,
      int vertexCount,
      int materialIndex)
    {
      this.m_StartIndex = startIndex;
      this.m_IndexCount = indexCount;
      this.m_StartVertex = startVertex;
      this.m_VertexCount = vertexCount;
      this.m_MaterialIndex = materialIndex;
    }
  }
}
