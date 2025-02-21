// Decompiled with JetBrains decompiler
// Type: Game.Simulation.RandomTrafficRequest
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Net;
using Game.Vehicles;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct RandomTrafficRequest : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Target;
    public RoadTypes m_RoadType;
    public TrackTypes m_TrackType;
    public EnergyTypes m_EnergyTypes;
    public SizeClass m_SizeClass;
    public RandomTrafficRequestFlags m_Flags;

    public RandomTrafficRequest(
      Entity target,
      RoadTypes roadType,
      TrackTypes trackType,
      EnergyTypes energyTypes,
      SizeClass sizeClass,
      RandomTrafficRequestFlags flags)
    {
      this.m_Target = target;
      this.m_RoadType = roadType;
      this.m_TrackType = trackType;
      this.m_EnergyTypes = energyTypes;
      this.m_SizeClass = sizeClass;
      this.m_Flags = flags;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Target);
      writer.Write((byte) this.m_RoadType);
      writer.Write((byte) this.m_TrackType);
      writer.Write((byte) this.m_EnergyTypes);
      writer.Write((byte) this.m_SizeClass);
      writer.Write((byte) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Target);
      if (reader.context.version >= Version.randomTrafficTypes)
      {
        byte num1;
        reader.Read(out num1);
        byte num2;
        reader.Read(out num2);
        byte num3;
        reader.Read(out num3);
        byte num4;
        reader.Read(out num4);
        this.m_RoadType = (RoadTypes) num1;
        this.m_TrackType = (TrackTypes) num2;
        this.m_EnergyTypes = (EnergyTypes) num3;
        this.m_SizeClass = (SizeClass) num4;
      }
      if (!(reader.context.version >= Version.randomTrafficFlags))
        return;
      byte num;
      reader.Read(out num);
      this.m_Flags = (RandomTrafficRequestFlags) num;
    }
  }
}
