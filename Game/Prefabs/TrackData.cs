// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrackData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Net;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct TrackData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public TrackTypes m_TrackType;
    public float m_SpeedLimit;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((byte) this.m_TrackType);
      writer.Write(this.m_SpeedLimit);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      byte num;
      reader.Read(out num);
      reader.Read(out this.m_SpeedLimit);
      this.m_TrackType = (TrackTypes) num;
    }
  }
}
