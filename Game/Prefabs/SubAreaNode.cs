// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SubAreaNode
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(4)]
  public struct SubAreaNode : IBufferElementData
  {
    public float3 m_Position;
    public int m_ParentMesh;

    public SubAreaNode(float3 position, int parentMesh)
    {
      this.m_Position = position;
      this.m_ParentMesh = parentMesh;
    }
  }
}
