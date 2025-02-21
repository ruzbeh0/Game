// Decompiled with JetBrains decompiler
// Type: Game.Rendering.InterpolatedTransform
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Events;
using Game.Objects;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  public struct InterpolatedTransform : IComponentData, IQueryTypeParameter, IEmptySerializable
  {
    public float3 m_Position;
    public quaternion m_Rotation;
    public TransformFlags m_Flags;

    public InterpolatedTransform(Transform transform)
    {
      this.m_Position = transform.m_Position;
      this.m_Rotation = transform.m_Rotation;
      this.m_Flags = (TransformFlags) 0;
    }

    public InterpolatedTransform(WeatherPhenomenon weatherPhenomenon)
    {
      this.m_Position = weatherPhenomenon.m_HotspotPosition;
      this.m_Rotation = quaternion.identity;
      this.m_Flags = (TransformFlags) 0;
    }

    public Transform ToTransform() => new Transform(this.m_Position, this.m_Rotation);
  }
}
