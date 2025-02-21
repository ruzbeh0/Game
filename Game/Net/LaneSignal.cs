// Decompiled with JetBrains decompiler
// Type: Game.Net.LaneSignal
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  public struct LaneSignal : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Petitioner;
    public Entity m_Blocker;
    public ushort m_GroupMask;
    public sbyte m_Priority;
    public sbyte m_Default;
    public LaneSignalType m_Signal;
    public LaneSignalFlags m_Flags;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Petitioner);
      writer.Write(this.m_Blocker);
      writer.Write(this.m_GroupMask);
      writer.Write(this.m_Priority);
      writer.Write(this.m_Default);
      writer.Write((byte) this.m_Signal);
      writer.Write((byte) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.trafficImprovements)
      {
        reader.Read(out this.m_Petitioner);
        reader.Read(out this.m_Blocker);
      }
      reader.Read(out this.m_GroupMask);
      reader.Read(out this.m_Priority);
      if (reader.context.version >= Version.levelCrossing)
        reader.Read(out this.m_Default);
      byte num1;
      reader.Read(out num1);
      this.m_Signal = (LaneSignalType) num1;
      if (!(reader.context.version >= Version.trafficFlowFixes))
        return;
      byte num2;
      reader.Read(out num2);
      this.m_Flags = (LaneSignalFlags) num2;
    }
  }
}
