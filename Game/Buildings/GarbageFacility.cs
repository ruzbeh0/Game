// Decompiled with JetBrains decompiler
// Type: Game.Buildings.GarbageFacility
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct GarbageFacility : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_GarbageDeliverRequest;
    public Entity m_GarbageReceiveRequest;
    public Entity m_TargetRequest;
    public GarbageFacilityFlags m_Flags;
    public float m_AcceptGarbagePriority;
    public float m_DeliverGarbagePriority;
    public int m_ProcessingRate;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_GarbageDeliverRequest);
      writer.Write(this.m_GarbageReceiveRequest);
      writer.Write(this.m_TargetRequest);
      writer.Write((byte) this.m_Flags);
      writer.Write(this.m_AcceptGarbagePriority);
      writer.Write(this.m_DeliverGarbagePriority);
      writer.Write(this.m_ProcessingRate);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.transferRequestRefactoring)
      {
        reader.Read(out this.m_GarbageDeliverRequest);
        reader.Read(out this.m_GarbageReceiveRequest);
      }
      else if (reader.context.version >= Version.garbageFacilityRefactor2)
        reader.Read(out Entity _);
      if (reader.context.version >= Version.reverseServiceRequests2)
        reader.Read(out this.m_TargetRequest);
      byte num;
      reader.Read(out num);
      Context context = reader.context;
      if (context.version >= Version.garbageFacilityRefactor)
      {
        reader.Read(out this.m_AcceptGarbagePriority);
        reader.Read(out this.m_DeliverGarbagePriority);
      }
      else
        reader.Read(out int _);
      context = reader.context;
      if (context.version >= Version.garbageProcessing)
      {
        context = reader.context;
        if (context.version < Version.powerPlantConsumption)
          reader.Read(out float _);
      }
      context = reader.context;
      if (context.version >= Version.powerPlantConsumption)
        reader.Read(out this.m_ProcessingRate);
      this.m_Flags = (GarbageFacilityFlags) num;
    }
  }
}
