// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.ParkedCar
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Vehicles
{
  public struct ParkedCar : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Lane;
    public float m_CurvePosition;

    public ParkedCar(Entity lane, float curvePosition)
    {
      this.m_Lane = lane;
      this.m_CurvePosition = curvePosition;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Lane);
      writer.Write(this.m_CurvePosition);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Lane);
      reader.Read(out this.m_CurvePosition);
    }
  }
}
