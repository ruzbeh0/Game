// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SubLane
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
  public struct SubLane : IBufferElementData
  {
    public Entity m_Prefab;
    public Bezier4x3 m_Curve;
    public int2 m_NodeIndex;
    public int2 m_ParentMesh;
  }
}
