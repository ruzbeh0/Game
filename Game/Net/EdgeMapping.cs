// Decompiled with JetBrains decompiler
// Type: Game.Net.EdgeMapping
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  public struct EdgeMapping : IComponentData, IQueryTypeParameter, IEmptySerializable
  {
    public Entity m_Parent1;
    public Entity m_Parent2;
    public float2 m_CurveDelta1;
    public float2 m_CurveDelta2;
  }
}
