// Decompiled with JetBrains decompiler
// Type: Game.Net.LabelPosition
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  [InternalBufferCapacity(0)]
  public struct LabelPosition : IBufferElementData, IEmptySerializable
  {
    public Bezier4x3 m_Curve;
    public int m_ElementIndex;
    public float m_HalfLength;
    public float m_MaxScale;
    public bool m_IsUnderground;
  }
}
