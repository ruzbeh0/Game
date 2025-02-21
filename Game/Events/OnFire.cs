// Decompiled with JetBrains decompiler
// Type: Game.Events.OnFire
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Events
{
  public struct OnFire : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Event;
    public Entity m_RescueRequest;
    public float m_Intensity;
    public uint m_RequestFrame;

    public OnFire(Entity _event, float intensity, uint requestFrame = 0)
    {
      this.m_Event = _event;
      this.m_RescueRequest = Entity.Null;
      this.m_Intensity = intensity;
      this.m_RequestFrame = requestFrame;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Event);
      writer.Write(this.m_RescueRequest);
      writer.Write(this.m_Intensity);
      writer.Write(this.m_RequestFrame);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Event);
      reader.Read(out this.m_RescueRequest);
      reader.Read(out this.m_Intensity);
      reader.Read(out this.m_RequestFrame);
    }
  }
}
