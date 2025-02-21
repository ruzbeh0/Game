// Decompiled with JetBrains decompiler
// Type: Game.Tools.Animation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Objects;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  public struct Animation : IComponentData, IQueryTypeParameter
  {
    public float3 m_TargetPosition;
    public float3 m_Position;
    public quaternion m_Rotation;
    public float3 m_SwayPivot;
    public float3 m_SwayPosition;
    public float3 m_SwayVelocity;
    public float m_PushFactor;

    public Transform ToTransform() => new Transform(this.m_Position, this.m_Rotation);
  }
}
