// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WatercraftData
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
  public struct WatercraftData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public SizeClass m_SizeClass;
    public EnergyTypes m_EnergyType;
    public float m_MaxSpeed;
    public float m_Acceleration;
    public float m_Braking;
    public float2 m_Turning;
    public float m_AngularAcceleration;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((byte) this.m_SizeClass);
      writer.Write((byte) this.m_EnergyType);
      writer.Write(this.m_MaxSpeed);
      writer.Write(this.m_Acceleration);
      writer.Write(this.m_Braking);
      writer.Write(this.m_Turning);
      writer.Write(this.m_AngularAcceleration);
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
      reader.Read(out this.m_Turning);
      reader.Read(out this.m_AngularAcceleration);
      this.m_SizeClass = (SizeClass) num1;
      this.m_EnergyType = (EnergyTypes) num2;
    }
  }
}
