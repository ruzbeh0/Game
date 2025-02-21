// Decompiled with JetBrains decompiler
// Type: Game.Net.NodeGeometry
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  public struct NodeGeometry : IComponentData, IQueryTypeParameter, IEmptySerializable
  {
    public Bounds3 m_Bounds;
    public float m_Position;
    public float m_Flatness;
    public float m_Offset;
  }
}
