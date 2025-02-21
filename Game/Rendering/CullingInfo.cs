// Decompiled with JetBrains decompiler
// Type: Game.Rendering.CullingInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Common;
using Unity.Entities;

#nullable disable
namespace Game.Rendering
{
  public struct CullingInfo : IComponentData, IQueryTypeParameter, IEmptySerializable
  {
    public Bounds3 m_Bounds;
    public float m_Radius;
    public int m_CullingIndex;
    public BoundsMask m_Mask;
    public byte m_MinLod;
    public byte m_PassedCulling;
  }
}
