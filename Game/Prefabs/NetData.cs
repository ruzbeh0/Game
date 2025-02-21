// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Net;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct NetData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public EntityArchetype m_NodeArchetype;
    public EntityArchetype m_EdgeArchetype;
    public Layer m_RequiredLayers;
    public Layer m_ConnectLayers;
    public Layer m_LocalConnectLayers;
    public CompositionFlags.General m_GeneralFlagMask;
    public CompositionFlags.Side m_SideFlagMask;
    public float m_NodePriority;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((uint) this.m_RequiredLayers);
      writer.Write((uint) this.m_ConnectLayers);
      writer.Write((uint) this.m_LocalConnectLayers);
      writer.Write(this.m_NodePriority);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      uint num1;
      reader.Read(out num1);
      uint num2;
      reader.Read(out num2);
      uint num3;
      reader.Read(out num3);
      reader.Read(out this.m_NodePriority);
      this.m_RequiredLayers = (Layer) num1;
      this.m_ConnectLayers = (Layer) num2;
      this.m_LocalConnectLayers = (Layer) num3;
    }
  }
}
