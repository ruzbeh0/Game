// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SubNet
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
  public struct SubNet : IBufferElementData
  {
    public Entity m_Prefab;
    public Bezier4x3 m_Curve;
    public int2 m_NodeIndex;
    public int2 m_ParentMesh;
    public NetInvertMode m_InvertMode;
    public CompositionFlags m_Upgrades;
    public bool2 m_Snapping;
  }
}
