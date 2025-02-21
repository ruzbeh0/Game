// Decompiled with JetBrains decompiler
// Type: Game.Routes.VehicleTiming
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Routes
{
  public struct VehicleTiming : IComponentData, IQueryTypeParameter, ISerializable
  {
    public uint m_LastDepartureFrame;
    public float m_AverageTravelTime;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_LastDepartureFrame);
      writer.Write(this.m_AverageTravelTime);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_LastDepartureFrame);
      reader.Read(out this.m_AverageTravelTime);
    }
  }
}
