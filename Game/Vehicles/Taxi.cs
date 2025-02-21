// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.Taxi
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Vehicles
{
  public struct Taxi : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_TargetRequest;
    public TaxiFlags m_State;
    public float m_PathElementTime;
    public float m_StartDistance;
    public float m_MaxBoardingDistance;
    public float m_MinWaitingDistance;
    public int m_ExtraPathElementCount;
    public ushort m_NextStartingFee;
    public ushort m_CurrentFee;

    public Taxi(TaxiFlags flags)
    {
      this.m_TargetRequest = Entity.Null;
      this.m_State = flags;
      this.m_PathElementTime = 0.0f;
      this.m_StartDistance = 0.0f;
      this.m_MaxBoardingDistance = 0.0f;
      this.m_MinWaitingDistance = 0.0f;
      this.m_ExtraPathElementCount = 0;
      this.m_NextStartingFee = (ushort) 0;
      this.m_CurrentFee = (ushort) 0;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TargetRequest);
      writer.Write((uint) this.m_State);
      writer.Write(this.m_PathElementTime);
      writer.Write(this.m_ExtraPathElementCount);
      writer.Write(this.m_StartDistance);
      writer.Write(this.m_NextStartingFee);
      writer.Write(this.m_CurrentFee);
      writer.Write(this.m_MaxBoardingDistance);
      writer.Write(this.m_MinWaitingDistance);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.reverseServiceRequests)
        reader.Read(out this.m_TargetRequest);
      uint num;
      reader.Read(out num);
      reader.Read(out this.m_PathElementTime);
      reader.Read(out this.m_ExtraPathElementCount);
      this.m_State = (TaxiFlags) num;
      if (reader.context.version >= Version.taxiFee)
      {
        reader.Read(out this.m_StartDistance);
        reader.Read(out this.m_NextStartingFee);
        reader.Read(out this.m_CurrentFee);
      }
      if (!(reader.context.version >= Version.roadPatchImprovements))
        return;
      reader.Read(out this.m_MaxBoardingDistance);
      reader.Read(out this.m_MinWaitingDistance);
    }
  }
}
