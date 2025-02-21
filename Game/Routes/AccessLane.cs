// Decompiled with JetBrains decompiler
// Type: Game.Routes.AccessLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Routes
{
  public struct AccessLane : 
    IComponentData,
    IQueryTypeParameter,
    IEquatable<AccessLane>,
    ISerializable
  {
    public Entity m_Lane;
    public float m_CurvePos;

    public AccessLane(Entity lane, float curvePos)
    {
      this.m_Lane = lane;
      this.m_CurvePos = curvePos;
    }

    public bool Equals(AccessLane other)
    {
      return this.m_Lane.Equals(other.m_Lane) && this.m_CurvePos.Equals(other.m_CurvePos);
    }

    public override int GetHashCode()
    {
      return (17 * 31 + this.m_Lane.GetHashCode()) * 31 + this.m_CurvePos.GetHashCode();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Lane);
      writer.Write(this.m_CurvePos);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Lane);
      reader.Read(out this.m_CurvePos);
    }
  }
}
