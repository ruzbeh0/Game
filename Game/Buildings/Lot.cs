// Decompiled with JetBrains decompiler
// Type: Game.Buildings.Lot
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Buildings
{
  public struct Lot : IComponentData, IQueryTypeParameter, IEmptySerializable
  {
    public float3 m_FrontHeights;
    public float3 m_RightHeights;
    public float3 m_BackHeights;
    public float3 m_LeftHeights;
  }
}
