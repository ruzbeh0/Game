// Decompiled with JetBrains decompiler
// Type: Game.Routes.RouteLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Routes
{
  public struct RouteLane : IComponentData, IQueryTypeParameter, IEquatable<RouteLane>, ISerializable
  {
    public Entity m_StartLane;
    public Entity m_EndLane;
    public float m_StartCurvePos;
    public float m_EndCurvePos;

    public RouteLane(Entity startLane, Entity endLane, float startCurvePos, float endCurvePos)
    {
      this.m_StartLane = startLane;
      this.m_EndLane = endLane;
      this.m_StartCurvePos = startCurvePos;
      this.m_EndCurvePos = endCurvePos;
    }

    public bool Equals(RouteLane other)
    {
      return this.m_StartLane.Equals(other.m_StartLane) && this.m_EndLane.Equals(other.m_EndLane) && this.m_StartCurvePos.Equals(other.m_StartCurvePos) && this.m_EndCurvePos.Equals(other.m_EndCurvePos);
    }

    public override int GetHashCode()
    {
      return (((17 * 31 + this.m_StartLane.GetHashCode()) * 31 + this.m_EndLane.GetHashCode()) * 31 + this.m_StartCurvePos.GetHashCode()) * 31 + this.m_EndCurvePos.GetHashCode();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_StartLane);
      writer.Write(this.m_EndLane);
      writer.Write(this.m_StartCurvePos);
      writer.Write(this.m_EndCurvePos);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_StartLane);
      reader.Read(out this.m_EndLane);
      reader.Read(out this.m_StartCurvePos);
      reader.Read(out this.m_EndCurvePos);
    }
  }
}
