// Decompiled with JetBrains decompiler
// Type: Game.Areas.Expand
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Areas
{
  [InternalBufferCapacity(4)]
  public struct Expand : IBufferElementData, IEmptySerializable
  {
    public float2 m_Offset;

    public Expand(float2 offset) => this.m_Offset = offset;
  }
}
