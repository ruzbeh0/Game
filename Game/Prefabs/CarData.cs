// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CarData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Vehicles;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct CarData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public SizeClass m_SizeClass;
    public EnergyTypes m_EnergyType;
    public float m_MaxSpeed;
    public float m_Acceleration;
    public float m_Braking;
    public float m_PivotOffset;
    public float2 m_Turning;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((byte) this.m_SizeClass);
      writer.Write((byte) this.m_EnergyType);
      writer.Write(this.m_MaxSpeed);
      writer.Write(this.m_Acceleration);
      writer.Write(this.m_Braking);
      writer.Write(this.m_PivotOffset);
      writer.Write(this.m_Turning);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      byte num1;
      reader.Read(out num1);
      byte num2;
      reader.Read(out num2);
      reader.Read(out this.m_MaxSpeed);
      reader.Read(out this.m_Acceleration);
      reader.Read(out this.m_Braking);
      reader.Read(out this.m_PivotOffset);
      reader.Read(out this.m_Turning);
      this.m_SizeClass = (SizeClass) num1;
      this.m_EnergyType = (EnergyTypes) num2;
    }
  }
}
