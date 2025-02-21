// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.WatercraftNavigationLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Vehicles
{
  [InternalBufferCapacity(8)]
  public struct WatercraftNavigationLane : IBufferElementData, ISerializable
  {
    public Entity m_Lane;
    public float2 m_CurvePosition;
    public WatercraftLaneFlags m_Flags;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Lane);
      writer.Write(this.m_CurvePosition);
      writer.Write((uint) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Lane);
      reader.Read(out this.m_CurvePosition);
      uint num;
      reader.Read(out num);
      this.m_Flags = (WatercraftLaneFlags) num;
    }
  }
}
