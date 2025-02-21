// Decompiled with JetBrains decompiler
// Type: Game.Areas.LabelExtents
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Areas
{
  [InternalBufferCapacity(2)]
  public struct LabelExtents : IBufferElementData, IEmptySerializable
  {
    public Bounds2 m_Bounds;

    public LabelExtents(float2 min, float2 max) => this.m_Bounds = new Bounds2(min, max);
  }
}
