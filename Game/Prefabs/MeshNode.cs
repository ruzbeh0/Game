// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MeshNode
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct MeshNode : IBufferElementData
  {
    public Bounds3 m_Bounds;
    public int2 m_IndexRange;
    public int4 m_SubNodes1;
    public int4 m_SubNodes2;
  }
}
