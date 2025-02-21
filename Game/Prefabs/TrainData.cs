// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrainData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Net;
using Game.Vehicles;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct TrainData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public TrackTypes m_TrackType;
    public EnergyTypes m_EnergyType;
    public TrainFlags m_TrainFlags;
    public float m_MaxSpeed;
    public float m_Acceleration;
    public float m_Braking;
    public float2 m_Turning;
    public float2 m_BogieOffsets;
    public float2 m_AttachOffsets;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((byte) this.m_TrackType);
      writer.Write((byte) this.m_EnergyType);
      writer.Write((byte) this.m_TrainFlags);
      writer.Write(this.m_MaxSpeed);
      writer.Write(this.m_Acceleration);
      writer.Write(this.m_Braking);
      writer.Write(this.m_Turning);
      writer.Write(this.m_BogieOffsets);
      writer.Write(this.m_AttachOffsets);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      byte num1;
      reader.Read(out num1);
      byte num2;
      reader.Read(out num2);
      if (reader.context.version >= Version.trainPrefabFlags)
      {
        byte num3;
        reader.Read(out num3);
        this.m_TrainFlags = (TrainFlags) num3;
      }
      reader.Read(out this.m_MaxSpeed);
      reader.Read(out this.m_Acceleration);
      reader.Read(out this.m_Braking);
      reader.Read(out this.m_Turning);
      reader.Read(out this.m_BogieOffsets);
      reader.Read(out this.m_AttachOffsets);
      this.m_TrackType = (TrackTypes) num1;
      this.m_EnergyType = (EnergyTypes) num2;
    }
  }
}
